 BEGIN TRY 
 Drop Procedure dbo.[PRC_ShukkaShijiTouroku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    出荷指示登録
--       Program ID      ShukkaShijiTouroku
--       Create date:    2020.1.26
--    ======================================================================

--CREATE TYPE [T_Shukka] AS TABLE(
--    [InstructionNO] [varchar](11) NULL,
--    [InstructionKBN] [tinyint] NULL,
--    [DeliveryPlanNO] [varchar](11) NULL,
--    [GyoNO] [int] NULL,
--    [DeliveryPlanDate] [date] NULL,
--    [CarrierCD] [varchar](3) NULL,
--	[CommentOutStore] [varchar](80) NULL,
--	[CommentInStore] [varchar](80) NULL,
--    [OntheDayFLG] [tinyint] NULL,
--    [ExpressFLG] [tinyint] NULL,
--    [UpdateFlg] [tinyint] NULL
--)
--GO

CREATE PROCEDURE [dbo].[PRC_ShukkaShijiTouroku]
    (@StoreCD   varchar(4),

    @Table  T_Shukka READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutInstructionNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @SYSDATE date;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @SYSDATE = CONVERT(date,@SYSDATETIME);

    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.InstructionNO, tbl.InstructionKBN, tbl.GyoNO 
        	,CONVERT(varchar, tbl.DeliveryPlanDate, 111)
        FROM @Table tbl
        ORDER BY tbl.GyoNO
        ;
	DECLARE @InstructionNO  varchar(11);
	DECLARE @InstructionKBN tinyint;
	DECLARE @GyoNO int;
	DECLARE @DeliveryPlanDate varchar(10);

    DECLARE @D_InstructionDetailsInserted TABLE (
         InstructionRows int
         ,ReserveNO varchar(11)
         );
         
    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO @InstructionNO, @InstructionKBN, @GyoNO, @DeliveryPlanDate
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        --Form.Detail.出荷指示番号＝""の時 （出荷指示データ未作成）
        IF ISNULL(@InstructionNO,'') = ''
        BEGIN
            --D_Instruction Insert時 "新規" D_Instruction Update時 "変更"
            SET @OperateModeNm = '新規';    

            --【D_Instruction】Insert Table転送仕様Ａ 出荷指示
            --伝票番号採番
            EXEC Fnc_GetNumber
                14,          --in伝票種別 14 出荷指示
                @DeliveryPlanDate, --in基準日
                @StoreCD,    --in店舗CD
                @Operator,
                @InstructionNO OUTPUT
                ;
            
            IF ISNULL(@InstructionNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            INSERT INTO [D_Instruction]
               ( [InstructionNO]
              ,[DeliveryPlanNO]
              ,[InstructionKBN]
              ,[InstructionDate]
              ,[DeliveryPlanDate]
              ,[FromSoukoCD]
              ,[DeliveryName]
              ,[DeliverySoukoCD]
              ,[DeliveryZip1CD]
              ,[DeliveryZip2CD]
              ,[DeliveryAddress1]
              ,[DeliveryAddress2]
              ,[DeliveryMailAddress]
              ,[DeliveryTelphoneNO]
              ,[DeliveryFaxNO]
              ,[DecidedDeliveryDate]
              ,[DecidedDeliveryTime]
              ,[CarrierCD]
              ,[CashOnDelivery]
              ,[PaymentMethodCD]
              ,[CommentOutStore]
              ,[CommentInStore]
              ,[InvoiceNO]
              ,[OntheDayFLG]
              ,[ExpressFLG]
              ,[PrintDate]
              ,[PrintStaffCD]
              ,[InsertOperator]
              ,[InsertDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime]
              ,[DeleteOperator]
              ,[DeleteDateTime]
            )SELECT
               @InstructionNO
               ,tbl.DeliveryPlanNO
               ,DD.DeliveryKBN	--InstructionKBN
               ,@SYSDATE	--InstructionDate
               ,tbl.DeliveryPlanDate
               ,(SELECT top 1 DR.SoukoCD	--FromSoukoCD
                           FROM  D_Reserve AS DR
                           INNER JOIN D_DeliveryPlanDetails AS DM
                           ON DM.DeliveryPlanNO = DD.DeliveryPlanNO
                           WHERE DR.Number = DM.Number
                            AND DR.NumberRows = DM.NumberRows
                            AND DR.DeleteDateTime IS NULL
                            ORDER BY DM.DeliveryPlanRows, DR.ReserveNO)
               ,DD.DeliveryName
               ,DD.DeliverySoukoCD
               ,DD.DeliveryZip1CD
               ,DD.DeliveryZip2CD
               ,DD.DeliveryAddress1
               ,DD.DeliveryAddress2
               ,DD.DeliveryMailAddress
               ,DD.DeliveryTelphoneNO
               ,DD.DeliveryFaxNO
               ,DD.DecidedDeliveryDate
               ,DD.DecidedDeliveryTime
               ,tbl.CarrierCD
               ,0	--CashOnDelivery
               ,DD.PaymentMethodCD
               ,tbl.CommentOutStore
               ,tbl.CommentInStore
               ,DD.InvoiceNO
               ,tbl.OntheDayFLG
               ,tbl.ExpressFLG
               ,NULL	--PrintDate
               ,NULL	--PrintStaffCD
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM @Table AS tbl
            INNER JOIN D_DeliveryPlan AS DD
            ON DD.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND DD.DeliveryKBN = tbl.InstructionKBN

            WHERE tbl.InstructionNO IS NULL
            AND tbl.InstructionKBN = @InstructionKBN
            AND tbl.GyoNO = @GyoNO
            ;

            --【D_InstructionDetails】Insert  Table転送仕様Ｂ 出荷指示明細
            INSERT INTO [D_InstructionDetails]
               ([InstructionNO]
               ,[InstructionRows]
               ,[InstructionKBN]
               ,[ReserveNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[InstructionSu]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            OUTPUT INSERTED.InstructionRows, INSERTED.ReserveNO INTO @D_InstructionDetailsInserted(InstructionRows,ReserveNO)
            SELECT
                @InstructionNO
               ,ROW_NUMBER() OVER(ORDER BY DD.DeliveryPlanNO, DM.DeliveryPlanRows, DM.Number, DM.NumberRows)
               ,DD.DeliveryKBN   --InstructionKBN
               ,DR.ReserveNO
               ,DR.SKUCD
               ,ISNULL(DR.AdminNO,0)
               ,DR.JanCD
               ,DM.CommentOutStore
               ,DM.CommentInStore
               ,ISNULL(DR.ReserveSu,0) --InstructionSu
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM @Table AS tbl
            INNER JOIN D_DeliveryPlan AS DD
            ON DD.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND DD.DeliveryKBN = tbl.InstructionKBN
            INNER JOIN D_DeliveryPlanDetails AS DM
            ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
            LEFT OUTER JOIN D_Reserve AS DR
            ON DR.Number = DM.Number
            AND DR.NumberRows = DM.NumberRows
            AND DR.DeleteDateTime IS NULL
            WHERE tbl.InstructionNO IS NULL
            AND tbl.InstructionKBN = @InstructionKBN
            AND tbl.GyoNO = @GyoNO
            ORDER BY DD.DeliveryPlanNO, DM.DeliveryPlanRows, DM.Number, DM.NumberRows
            ;
    
            --【D_Reserve】      Update Table転送仕様Ｃ 引当
            UPDATE D_Reserve
                SET [UpdateOperator]   =  @Operator  
                   ,[UpdateDateTime]   =  @SYSDATETIME
                   ,[ShippingOrderNO]  = @InstructionNO
                   ,[ShippingOrderRows]= (SELECT DI.InstructionRows
                                        FROM @D_InstructionDetailsInserted AS DI
                                        WHERE DI.ReserveNO = D_Reserve.ReserveNO
                                        )
             FROM D_DeliveryPlanDetails AS DM
             INNER JOIN @Table AS tbl
             ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
             AND tbl.GyoNO = @GyoNO
             WHERE D_Reserve.Number = DM.Number
                AND D_Reserve.NumberRows = DM.NumberRows
                AND D_Reserve.DeleteDateTime IS NULL
             ;
                     
            --【D_Stock】        Update Table転送仕様Ｄ 在庫
            UPDATE [D_Stock]
               SET [ReserveSu] = [D_Stock].[ReserveSu] - (CASE ISNULL(F.SetKBN,0) WHEN 1 THEN DR.ReserveSu*F.SetSu
                                ELSE DR.ReserveSu END)   --引当数
                  ,[InstructionSu] = [D_Stock].[InstructionSu] + (CASE ISNULL(F.SetKBN,0) WHEN 1 THEN DR.ReserveSu*F.SetSu
                                ELSE DR.ReserveSu END)    --出荷指示数
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_DeliveryPlanDetails AS DM
            INNER JOIN @Table AS tbl
            ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND tbl.GyoNO = @GyoNO
            INNER JOIN D_Reserve AS DR
            ON DR.Number = DM.Number
            AND DR.NumberRows = DM.NumberRows
            AND DR.DeleteDateTime IS NULL
            LEFT OUTER JOIN F_SKU(@SYSDATE) AS F
            ON F.AdminNO = DR.AdminNO
            AND F.DeleteFlg = 0
            WHERE D_Stock.StockNO = DR.StockNO
            AND D_Stock.DeleteDateTime IS NULL
            ;
            
            --【D_JuchuuDetails】Update Table転送仕様Ｅ 受注明細    
            UPDATE [D_JuchuuDetails]
               SET [DeliveryOrderSu] = [D_JuchuuDetails].[DeliveryOrderSu] + DR.ReserveSu   --出荷指示数
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_DeliveryPlanDetails AS DM
            INNER JOIN @Table AS tbl
            ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND tbl.GyoNO = @GyoNO
            INNER JOIN D_Reserve AS DR
            ON DR.Number = DM.Number
            AND DR.NumberRows = DM.NumberRows
            AND DR.DeleteDateTime IS NULL
            WHERE D_JuchuuDetails.JuchuuNO = DM.Number
            AND D_JuchuuDetails.JuchuuRows = DM.NumberRows
            AND D_JuchuuDetails.DeleteDateTime IS NULL
            ;
        END
        ELSE
        BEGIN
        	--D_Instruction Insert時 "新規" D_Instruction Update時 "変更"
            SET @OperateModeNm = '変更';    

            UPDATE [D_Instruction]
               SET [DeliveryPlanDate] = tbl.DeliveryPlanDate
                  ,[CarrierCD] = tbl.CarrierCD
                  ,[CommentOutStore] = tbl.CommentOutStore
                  ,[CommentInStore] = tbl.CommentInStore
                  ,[OntheDayFLG] = tbl.OntheDayFLG
                  ,[ExpressFLG] = tbl.ExpressFLG
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             FROM @Table AS tbl
             WHERE D_Instruction.InstructionNO = tbl.InstructionNO
             ;

            UPDATE [D_InstructionDetails]
               SET [UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             WHERE InstructionNO = @InstructionNO
             AND DeleteDateTime IS NULL
             ;
        END

        
        --【L_Log】Insert Table転送仕様Ｚ（処理モード：新規）処理履歴 
        --処理履歴データへ更新
        SET @KeyItem = @InstructionNO;

        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            'ShukkaShijiTouroku',
            @PC,
            @OperateModeNm,
            @KeyItem;
        
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
    	INTO @InstructionNO, @InstructionKBN, @GyoNO, @DeliveryPlanDate
    

    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
	DEALLOCATE CUR_AAA;

    SET @OutInstructionNO = @InstructionNO;
    
--<<OWARI>>
  return @W_ERR;

END



