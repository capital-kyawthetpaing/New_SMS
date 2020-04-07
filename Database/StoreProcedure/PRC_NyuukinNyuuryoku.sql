 BEGIN TRY 
 Drop Procedure dbo.[PRC_NyuukinNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    入金入力
--       Program ID      PRC_NyuukinNyuuryoku
--       Create date:    2019.11.01
--    ======================================================================

--CREATE TYPE T_Collect AS TABLE
--    (
--    [GyoNO] [int],
--    [ConfirmNO] [varchar](11) ,		--未使用
--    [WebCollectNO]  [varchar](11) ,
--    [WebCollectType] [varchar](3),
--    [CollectPlanNO] [int],
--    [CollectPlanRows] [int],

--    [ConfirmAmount] [money] ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_NyuukinNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @StoreCD   varchar(4),
    @CollectNO   varchar(11),
    @ConfirmNO   varchar(11),
    @CollectDate  varchar(10),
    @StaffCD   varchar(10),
    @CollectCustomerCD   varchar(13),
    
    --取込種別用
    @WebCollectNO  varchar(11),
    @WebCollectType  varchar(3),
    
    @PaymentMethodCD   varchar(3),
    @KouzaCD           varchar(3),
    @BillDate  varchar(10),
    @CollectClearDate  varchar(10),

    @Head_CollectAmount  money,
    @FeeDeduction money,
    @Deduction1 money,
    @Deduction2 money,
    @DeductionConfirm money,
    @ConfirmSource money,
    @Head_ConfirmAmount money,
    @Remark varchar(200),
    
    @Table  T_Collect READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutCollectNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

    DECLARE @CollectPlanNO int;
    DECLARE @OldCollectPlanNO int;
    DECLARE @SumConfirmAmount money;
--    DECLARE @CollectAmount money;
--    DECLARE @CollectPlanRows int;
    DECLARE @NewConfirmNO varchar(11);
    DECLARE @tblGyoNO int;
    
    --明細部をCollectPlanNO毎に消込額を集計
    --カーソルは明細1件毎、明細件数分D_CollectBillingDetails、CollectPlanNO件数分D_CollectBillingが作成される
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.GyoNO, tbl.CollectPlanNO
        	, SUM(tbl.ConfirmAmount) OVER(PARTITION BY tbl.CollectPlanNO) AS SumConfirmAmount
        FROM @Table AS tbl
        --WHERE
        ORDER BY tbl.CollectPlanNO
        ;
        
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';

        --D_Collect             Insert Table転送仕様Ａ
        --伝票番号採番
        EXEC Fnc_GetNumber
            7,             --in伝票種別 7
            @CollectDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @CollectNO OUTPUT
            ;
        
        IF ISNULL(@CollectNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END

        INSERT INTO [D_Collect]
           ([CollectNO]
           ,[InputKBN]
           ,[StoreCD]
           ,[StaffCD]
           ,[InputDatetime]
           ,[WebCollectNO]
           ,[WebCollectType]
           ,[CollectCustomerCD]
           ,[CollectDate]
           ,[PaymentMethodCD]
           ,[KouzaCD]
           ,[BillDate]
           ,[CollectAmount]
           ,[FeeDeduction]
           ,[Deduction1]
           ,[Deduction2]
           ,[DeductionConfirm]
           ,[ConfirmSource]
           --,[ConfirmAmount]
           ,[Remark]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@CollectNO
           ,2   --InputKBN, tinyint     2:入力
           ,@StoreCD
           ,@StaffCD
           ,@SYSDATETIME    --InputDatetime
           ,@WebCollectNO
           ,@WebCollectType
           ,@CollectCustomerCD
           ,@CollectDate
           ,@PaymentMethodCD
           ,@KouzaCD
           
           ,@BillDate
           ,@Head_CollectAmount     --CollectAmount
           ,@FeeDeduction
           ,@Deduction1
           ,@Deduction2
           ,@DeductionConfirm
           ,@ConfirmSource
           --,@Head_ConfirmAmount     --ConfirmAmount 消込額は入金データには更新しない
           ,@Remark
           ,@Operator   --InsertOperator, varchar(10),>
           ,@SYSDATETIME    --InsertDateTime, datetime,>
           ,@Operator   --UpdateOperator, varchar(10),>
           ,@SYSDATETIME    --UpdateDateTime, datetime,>
           ,NULL    --DeleteOperator, varchar(10),>
           ,NULL    --DeleteDateTime, datetime,>
           );

    --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblGyoNO, @CollectPlanNO,@SumConfirmAmount;
        
        SET @OldCollectPlanNO = '';
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            IF @OldCollectPlanNO <> @CollectPlanNO
            BEGIN
                SET @OldCollectPlanNO = @CollectPlanNO;
            
                --伝票番号採番
                EXEC Fnc_GetNumber
                    8,             --in伝票種別 8
                    @CollectDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @NewConfirmNO OUTPUT
                    ;
                
                IF ISNULL(@NewConfirmNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --D_CollectBilling      Insert Table転送仕様Ｂ
                INSERT INTO [D_CollectBilling]
                   ([ConfirmNO]
                   ,[CollectPlanNO]
                   ,[CollectAmount]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                    @NewConfirmNO
                   ,tbl.CollectPlanNO
                   ,@SumConfirmAmount   --CollectAmount
                   ,@Operator   --InsertOperator, varchar(10),>
                   ,@SYSDATETIME    --InsertDateTime, datetime,>
                   ,@Operator   --UpdateOperator, varchar(10),>
                   ,@SYSDATETIME    --UpdateDateTime, datetime,>
                   ,NULL    --DeleteOperator, varchar(10),>
                   ,NULL    --DeleteDateTime, datetime,>
                FROM @Table AS tbl
                WHERE tbl.GyoNO = @tblGyoNO;
                
                --D_PaymentConfirm      Insert Table転送仕様Ｄ
                INSERT INTO [D_PaymentConfirm]
                   ([ConfirmNO]
                   ,[CollectNO]
                   ,[CollectClearDate]
                   ,[StaffCD]
                   ,[ConfirmDateTime]
                   ,[ConfirmAmount]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                    @NewConfirmNO
                   ,@CollectNO
                   ,CONVERT(date,@CollectClearDate)     --CollectClearDate, date,>
                   ,@StaffCD
                   ,@SYSDATETIME    --ConfirmDateTime, datetime,>
                   ,@SumConfirmAmount   --ConfirmAmount
                   ,@Operator   --InsertOperator, varchar(10),>
                   ,@SYSDATETIME    --InsertDateTime, datetime,>
                   ,@Operator   --UpdateOperator, varchar(10),>
                   ,@SYSDATETIME    --UpdateDateTime, datetime,>
                   ,NULL    --DeleteOperator, varchar(10),>
                   ,NULL    --DeleteDateTime, datetime,>
                FROM @Table AS tbl
                WHERE tbl.GyoNO = @tblGyoNO;
            END
            
            --D_CollectBillingDetails   Insert Table転送仕様Ｃ
            INSERT INTO [D_CollectBillingDetails]
               ([ConfirmNO]
               ,[CollectPlanNO]
               ,[CollectPlanRows]
               ,[CollectAmount]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @NewConfirmNO
               ,tbl.CollectPlanNO
               ,tbl.CollectPlanRows
               ,tbl.ConfirmAmount
               ,@Operator   --InsertOperator, varchar(10),>
               ,@SYSDATETIME    --InsertDateTime, datetime,>
               ,@Operator   --UpdateOperator, varchar(10),>
               ,@SYSDATETIME    --UpdateDateTime, datetime,>
               ,NULL    --DeleteOperator, varchar(10),>
               ,NULL    --DeleteDateTime, datetime,>
            FROM @Table AS tbl
            WHERE tbl.GyoNO = @tblGyoNO;

            --取込種別の場合
            IF ISNULL(@WebCollectType,'') <> ''
            BEGIN
            	--テーブル転送仕様Ｅ
                UPDATE [D_WebCollect]
                SET CollectDatetime = @SYSDATETIME
                    ,CollectNO = @CollectNO
                FROM @Table AS tbl
                WHERE tbl.GyoNO = @tblGyoNO
                AND tbl.WebCollectNO = D_WebCollect.WebCollectNO
                ;
                
                --テーブル転送仕様Ｆ Web決済情報明細　D_WebCollectDetails
                UPDATE [D_WebCollectDetails]
                SET WebCollectAmount = DC.CollectAmount
                FROM @Table AS tbl
                INNER JOIN D_CollectBillingDetails AS DC
                ON DC.CollectPlanNO = tbl.CollectPlanNO
                AND DC.CollectPlanRows = tbl.CollectPlanRows
                AND DC.ConfirmNO = @NewConfirmNO
                WHERE tbl.GyoNO = @tblGyoNO
                AND tbl.WebCollectNO = D_WebCollectDetails.WebCollectNO
                AND DC.CollectPlanNO = D_WebCollectDetails.CollectPlanNO
                AND DC.CollectPlanRows = D_WebCollectDetails.CollectPlanRows                
                ;
            END
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
        	INTO @tblGyoNO, @CollectPlanNO,@SumConfirmAmount;
        END            --LOOPの終わり*******************************************************CUR_TABLE
    END
        
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
        
        --D_Collect                 Update  Table転送仕様Ａ 
        UPDATE [D_Collect]
           SET [StaffCD] = @StaffCD
              ,[InputDatetime] = @SYSDATETIME   --InputDatetime, datetime,>
              ,[WebCollectNO] = @WebCollectNO
              ,[WebCollectType] = @WebCollectType
              ,[CollectCustomerCD] = @CollectCustomerCD
              ,[CollectDate] = @CollectDate
              ,[PaymentMethodCD] = @PaymentMethodCD
              ,[KouzaCD] = @KouzaCD
              ,[BillDate] = @BillDate
              ,[CollectAmount] = @Head_CollectAmount
              ,[FeeDeduction] = @FeeDeduction
              ,[Deduction1] = @Deduction1
              ,[Deduction2] = @Deduction2
              ,[DeductionConfirm] = @DeductionConfirm
              ,[ConfirmSource] = @ConfirmSource
              --,[ConfirmAmount] = @Head_ConfirmAmount	消込額は入金データには更新しない
              ,[Remark] = @Remark
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE CollectNO = @CollectNO;
        
        IF ISNULL(@ConfirmNO,'') <> ''
        BEGIN
            --D_CollectBilling          Update  Table転送仕様Ｂ 
            UPDATE [D_CollectBilling]
               SET [CollectAmount] = (SELECT SUM(tbl.ConfirmAmount)
                                                 FROM @Table AS tbl
                                                 WHERE tbl.CollectPlanNO = D_CollectBilling.CollectPlanNO)
                  ,[UpdateOperator] =  @Operator  
                  ,[UpdateDateTime] =  @SYSDATETIME
            FROM @Table AS tbl
             WHERE D_CollectBilling.ConfirmNO = @ConfirmNO
               AND D_CollectBilling.CollectPlanNO = tbl.CollectPlanNO
               ;
            
            --D_CollectBillingDetails   Update  Table転送仕様Ｃ 
            UPDATE [D_CollectBillingDetails]
               SET [CollectAmount] = tbl.ConfirmAmount
                  ,[UpdateOperator] =  @Operator  
                  ,[UpdateDateTime] =  @SYSDATETIME
             FROM @Table AS tbl
             WHERE D_CollectBillingDetails.ConfirmNO = @ConfirmNO
               AND D_CollectBillingDetails.CollectPlanNO = tbl.CollectPlanNO
               AND D_CollectBillingDetails.CollectPlanRows = tbl.CollectPlanRows;

            --D_PaymentConfirm          Update  Table転送仕様Ｄ 
            UPDATE [D_PaymentConfirm]
               SET [StaffCD] = @StaffCD
                  ,[ConfirmDateTime] = @SYSDATETIME     --ConfirmDateTime, datetime,>
                  ,[ConfirmAmount] = (SELECT SUM(tbl.ConfirmAmount)
                                                 FROM @Table AS tbl)
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
             FROM @Table AS tbl
             WHERE D_PaymentConfirm.ConfirmNO = @ConfirmNO
             AND D_PaymentConfirm.CollectNO = @CollectNO
             ;
        END
        
        --取込種別の場合
        IF ISNULL(@WebCollectType,'') <> ''
        BEGIN
        	--テーブル転送仕様Ｅ
            UPDATE [D_WebCollect]
            SET CollectDatetime = @SYSDATETIME
                ,CollectNO = @CollectNO
            FROM @Table AS tbl
            WHERE tbl.GyoNO = @tblGyoNO
            AND tbl.WebCollectNO = D_WebCollect.WebCollectNO
            ;
            
            --テーブル転送仕様Ｆ Web決済情報明細　D_WebCollectDetails
            UPDATE [D_WebCollectDetails]
            SET WebCollectAmount = D_WebCollectDetails.WebCollectAmount + DC.CollectAmount
            FROM @Table AS tbl
            INNER JOIN D_CollectBillingDetails AS DC
            ON DC.CollectPlanNO = tbl.CollectPlanNO
            AND DC.CollectPlanRows = tbl.CollectPlanRows
            AND DC.ConfirmNO = @ConfirmNO
            WHERE tbl.GyoNO = @tblGyoNO
            AND tbl.WebCollectNO = D_WebCollectDetails.WebCollectNO
            AND DC.CollectPlanNO = D_WebCollectDetails.CollectPlanNO
            AND DC.CollectPlanRows = D_WebCollectDetails.CollectPlanRows                
            ;
        END
    END
    
    --L_Log Insert Table転送仕様Ｚ
    --処理履歴データへ更新
    SET @KeyItem = @CollectNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NyuukinNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutCollectNO = @CollectNO;
    
--<<OWARI>>
  return @W_ERR;

END


