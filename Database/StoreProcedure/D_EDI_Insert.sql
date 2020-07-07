BEGIN TRY 
 Drop Procedure D_EDI_Insert
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	BEGIN TRY
	Drop Type dbo.[T_Edi]
	END try
	BEGIN CATCH END CATCH

--  ======================================================================
--       Program Call    EDI回答納期登録
--       Program ID      EDIKaitouNoukiTouroku
--       Create date:    2019.10.22
--    ======================================================================

CREATE TYPE T_Edi AS TABLE
    (
   [EDIImportRows]           [int]  --取込行番号 [2]
  ,[OrderNO]                 [varchar] (11) --発注番号
  ,[OrderRows]               [int]          --発注明細連番
  ,[ArrivalPlanDate]         [date]   --入荷予定日
  ,[ArrivalPlanMonth]        [int]    --入荷予定月
  ,[ArrivalPlanCD]           [varchar] (4)   --入荷予定状況CD
  ,[ArrivalPlanSu]           [int]   --入荷予定数
  ,[VendorComment]           [varchar] (30)  --仕入先コメント
  ,[ErrorKBN]                [tinyint]   --エラー区分
  ,[ErrorText]               [varchar] (60)  --エラー内容
  ,[EDIOrderNO]              [varchar] (11)  --EDI発注番号
  ,[EDIOrderRows]            [int]           --行番号
  ,[CSVRecordKBN]            [varchar] (1)   --レコード区分
  ,[CSVDataKBN]              [varchar] (2)   --データ区分
  ,[CSVCapitalCD]            [varchar] (13)  --発注者会社部署CD
  ,[CSVCapitalName]          [varchar] (20)  --発注者企業部署名
  ,[CSVOrderCD]              [varchar] (13)  --受注者会社部署CD
  ,[CSVOrderName]            [varchar] (20)  --受注者企業部署名
  ,[CSVSalesCD]              [varchar] (13)  --販売店会社部署CD
  ,[CSVSalesName]            [varchar] (20)  --販売店会社部署名
  ,[CSVDestinationCD]        [varchar] (13)  --出荷先会社部署CD
  ,[CSVDestinationName]      [varchar] (20)  --出荷先会社部署名
  ,[CSVOrderNO]              [varchar] (11)  --発注NO
  ,[CSVOrderRows]            [varchar] (5)   --発注NO行
  ,[CSVOrderLines]           [varchar] (5)   --発注NO列
  ,[CSVOrderDate]            [varchar] (15)  --発注日
  ,[CSVArriveDate]           [varchar] (15)  --納品日
  ,[CSVOrderKBN]             [varchar] (4)   --発注区分
  ,[CSVMakerItemKBN]         [varchar] (8)   --発注者商品分類
  ,[CSVMakerItem]            [varchar] (50)  --発注者商品CD
  ,[CSVSKUCD]                [varchar] (30)  --受注者品番
  ,[CSVSizeName]             [varchar] (20)  --メーカー規格1
  ,[CSVColorName]            [varchar] (20)  --メーカー規格2
  ,[CSVTaniCD]               [varchar] (3)   --単位
  ,[CSVOrderUnitPrice]       [varchar] (15)  --取引単価
  ,[CSVOrderPriceWithoutTax] [varchar] (15)  --標準上代
  ,[CSVBrandName]            [varchar] (20)  --ブランド略名
  ,[CSVSKUName]              [varchar] (15)  --商品略名
  ,[CSVJanCD]                [varchar] (13)  --JANCD
  ,[CSVOrderSu]              [varchar] (10)  --発注数
  ,[CSVOrderGroupNO]         [varchar] (11)  --予備１（発注グループ番号）
  ,[CSVAnswerSu]             [varchar] (10)  --予備２（引当数）
  ,[CSVNextDate]             [varchar] (10)  --予備３（次回予定日）
  ,[CSVOrderGroupRows]       [varchar] (5)   --予備４（発注グループ行）
  ,[CSVErrorMessage]         [varchar] (10)  --予備５（エラーメッセージ）
    )
GO

CREATE PROCEDURE D_EDI_Insert
    (@VendorCD   varchar(13),
    @ImportFile   varchar(200),
    @OrderDetailsSu  int,
    @ImportDetailsSu  int,
    @ErrorSu  int,

    @Table  T_Edi READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    
    @OutEDIImportNO varchar(11) OUTPUT
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
    DECLARE @SYSDATE varchar(10);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(varchar,@SYSDATETIME,111);
    
    INSERT INTO [D_EDI]
           ([ImportDateTime]
           ,[StaffCD]
           ,[VendorCD]
           ,[ImportFile]
           ,[OrderDetailsSu]
           ,[ImportDetailsSu]
           ,[ErrorSu]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@SYSDATETIME    --ImportDateTime, datetime,>
           ,NULL    --StaffCD, varchar(10),>
           ,@VendorCD
           ,@ImportFile
           ,@OrderDetailsSu
           ,@ImportDetailsSu
           ,@ErrorSu
           ,NULL    --InsertOperator, varchar(10),>
           ,@SYSDATETIME    --InsertDateTime, datetime,>
           ,NULL    --UpdateOperator, varchar(10),>
           ,@SYSDATETIME    --UpdateDateTime, datetime,>
           ,NULL    --DeleteOperator, varchar(10),>
           ,NULL    --DeleteDateTime, datetime,>
           );

    SELECT @OutEDIImportNO = MAX(EDIImportNO)
    FROM D_EDI
    ;
    
    INSERT INTO [D_EDIDetail]
           ([EDIImportNO]
           ,[EDIImportRows]
           ,[OrderNO]
           ,[OrderRows]
           ,[ArrivalPlanDate]
           ,[ArrivalPlanMonth]
           ,[ArrivalPlanCD]
           ,[ArrivalPlanSu]
           ,[VendorComment]
           ,[ErrorKBN]
           ,[ErrorText]
           ,[EDIOrderNO]
           ,[EDIOrderRows]
           ,[CSVRecordKBN]
           ,[CSVDataKBN]
           ,[CSVCapitalCD]
           ,[CSVCapitalName]
           ,[CSVOrderCD]
           ,[CSVOrderName]
           ,[CSVSalesCD]
           ,[CSVSalesName]
           ,[CSVDestinationCD]
           ,[CSVDestinationName]
           ,[CSVOrderNO]
           ,[CSVOrderRows]
           ,[CSVOrderLines]
           ,[CSVOrderDate]
           ,[CSVArriveDate]
           ,[CSVOrderKBN]
           ,[CSVMakerItemKBN]
           ,[CSVMakerItem]
           ,[CSVSKUCD]
           ,[CSVSizeName]
           ,[CSVColorName]
           ,[CSVTaniCD]
           ,[CSVOrderUnitPrice]
           ,[CSVOrderPriceWithoutTax]
           ,[CSVBrandName]
           ,[CSVSKUName]
           ,[CSVJanCD]
           ,[CSVOrderSu]
           ,[CSVOrderGroupNO]
           ,[CSVAnswerSu]
           ,[CSVNextDate]
           ,[CSVOrderGroupRows]
           ,[CSVErrorMessage]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            @OutEDIImportNO     --<EDIImportNO, int,>
           ,tbl.EDIImportRows
           ,tbl.OrderNO
           ,tbl.OrderRows
           ,tbl.ArrivalPlanDate
           ,tbl.ArrivalPlanMonth
           ,tbl.ArrivalPlanCD
           ,tbl.ArrivalPlanSu
           ,tbl.VendorComment
           ,tbl.ErrorKBN
           ,tbl.ErrorText
           ,tbl.EDIOrderNO
           ,tbl.EDIOrderRows
           ,tbl.CSVRecordKBN
           ,tbl.CSVDataKBN
           ,tbl.CSVCapitalCD
           ,tbl.CSVCapitalName
           ,tbl.CSVOrderCD
           ,tbl.CSVOrderName
           ,tbl.CSVSalesCD
           ,tbl.CSVSalesName
           ,tbl.CSVDestinationCD
           ,tbl.CSVDestinationName
           ,tbl.CSVOrderNO
           ,tbl.CSVOrderRows
           ,tbl.CSVOrderLines
           ,tbl.CSVOrderDate
           ,tbl.CSVArriveDate
           ,tbl.CSVOrderKBN
           ,tbl.CSVMakerItemKBN
           ,tbl.CSVMakerItem
           ,tbl.CSVSKUCD
           ,tbl.CSVSizeName
           ,tbl.CSVColorName
           ,tbl.CSVTaniCD
           ,tbl.CSVOrderUnitPrice
           ,tbl.CSVOrderPriceWithoutTax
           ,tbl.CSVBrandName
           ,tbl.CSVSKUName
           ,tbl.CSVJanCD
           ,tbl.CSVOrderSu
           ,tbl.CSVOrderGroupNO
           ,tbl.CSVAnswerSu
           ,tbl.CSVNextDate
           ,tbl.CSVOrderGroupRows
           ,tbl.CSVErrorMessage
           ,NULL    --InsertOperator, varchar(10),>
           ,@SYSDATETIME    --InsertDateTime, datetime,>
           ,NULL    --UpdateOperator, varchar(10),>
           ,@SYSDATETIME    --UpdateDateTime, datetime,>
           ,NULL    --DeleteOperator, varchar(10),>
           ,NULL    --DeleteDateTime, datetime,>
      FROM @Table tbl
      ;

    --エラー番号＝０の場合（エラーでない場合）かつ、CSV.納品日≠Nullの場合
    --テーブル転送仕様Ｃ(履歴ファイルエラー回避のためヘッダ部も更新)
    UPDATE D_Order SET
        --,UpdateOperator = 
        UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_Order.OrderNO
    ;
    
    UPDATE D_OrderDetails SET
        ArrivePlanDate = tbl.CSVArriveDate
        --,UpdateOperator = 
        ,UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_OrderDetails.OrderNO
    AND tbl.OrderRows = D_OrderDetails.OrderRows
    ;
    
    --テーブル転送仕様Ｄ
    UPDATE D_ArrivalPlan SET
        LastestFLG = 9
        --,UpdateOperator = 
        ,UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_ArrivalPlan.Number
    AND tbl.OrderRows = D_ArrivalPlan.NumberRows
    ;
    
    --テーブル転送仕様Ｅ
    DECLARE @EDIImportRows int;
    DECLARE @StoreCD varchar(4);
    DECLARE @CSVOrderDate varchar(15);
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO varchar(11);
    DECLARE @ReserveNO varchar(11);
    DECLARE @JuchuuNO varchar(11);
    
   --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.EDIImportRows, DH.StoreCD
        ,CONVERT(varchar,CONVERT(date,tbl.CSVOrderDate),111) AS CSVOrderDate
        ,(SELECT DM.JuchuuNO FROM D_OrderDetails AS DM 
            WHERE tbl.OrderNO = DM.OrderNO
            AND tbl.OrderRows = DM.OrderRows) AS JuchuuNO
        FROM @Table AS tbl
        INNER JOIN D_Order AS DH
        ON tbl.OrderNO = DH.OrderNO
        WHERE tbl.ErrorKBN = 0
        AND ISNULL(tbl.CSVArriveDate,'') <> ''
        ORDER BY tbl.EDIImportRows;
    
    --カーソルオープン
    OPEN CUR_AAA;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO  @EDIImportRows,@StoreCD,@CSVOrderDate,@JuchuuNO;
     
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        --伝票番号採番
        EXEC Fnc_GetNumber
            22,         --in伝票種別 22
            @CSVOrderDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @ArrivalPlanNO OUTPUT
            ;
            
        IF ISNULL(@ArrivalPlanNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        INSERT INTO [D_ArrivalPlan]
               ([ArrivalPlanNO]
               ,[ArrivalPlanKBN]
               ,[Number]
               ,[NumberRows]
               ,[NumberSEQ]
               ,[ArrivalPlanDate]
               ,[ArrivalPlanMonth]
               ,[ArrivalPlanCD]
               ,[CalcuArrivalPlanDate]
               ,[ArrivalPlanUpdateDateTime]
               ,[StaffCD]
               ,[LastestFLG]
               ,[EDIImportNO]
               ,[SoukoCD]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[ArrivalPlanSu]
               ,[ArrivalSu]
               ,[OriginalArrivalPlanNO]
               ,[OrderCD]
               ,[FromSoukoCD]
               ,[ToStoreCD]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
         SELECT
               @ArrivalPlanNO
               ,1   --ArrivalPlanKBN
               ,tbl.OrderNo     --Number
               ,tbl.OrderRows   --NumberRows
               ,1   --NumberSEQ
               ,tbl.CSVArriveDate
               ,0   --ArrivalPlanMonth
               ,NULL    --ArrivalPlanCD
               ,(CASE WHEN ISNULL(tbl.CSVArriveDate,'') = '' THEN NULL
                ELSE tbl.CSVArriveDate END)
               ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
               ,NULL   --StaffCD
               ,1   --LastestFLG
               ,@OutEDIImportNO
               ,DH.DestinationSoukoCD --SoukoCD
               ,(SELECT top 1 M.SKUCD
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,(SELECT top 1 M.AdminNO
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,tbl.CSVJanCD
               ,tbl.CSVAnswerSu
               ,0   --ArrivalSu
               ,NULL    --OriginalArrivalPlanNO
               ,@VendorCD
               ,NULL    --FromSoukoCD
               ,NULL    --ToStoreCD
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           INNER JOIN D_Order AS DH
            ON tbl.OrderNO = DH.OrderNO
           WHERE tbl.EDIImportRows = @EDIImportRows
           ;
           
        --テーブル転送仕様Ｆ
        --伝票番号採番
        EXEC Fnc_GetNumber
            21,         --in伝票種別 21:在庫
            @SYSDATE, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @StockNO OUTPUT
            ;

        IF ISNULL(@StockNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        INSERT INTO [D_Stock]
               ([StockNO]
               ,[SoukoCD]
               ,[RackNO]
               ,[ArrivalPlanNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[ArrivalYetFLG]
               ,[ArrivalPlanKBN]
               ,[ArrivalPlanDate]
               ,[ArrivalDate]
               ,[StockSu]
               ,[PlanSu]
               ,[AllowableSu]
               ,[AnotherStoreAllowableSu]
               ,[ReserveSu]
               ,[InstructionSu]
               ,[ShippingSu]
               ,[OriginalStockNO]
               ,[ExpectReturnDate]
               ,[ReturnDate]
               ,[ReturnSu]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
         SELECT
               @StockNO
               ,DH.DestinationSoukoCD
               ,NULL    --RackNO
               ,@ArrivalPlanNO
               ,(SELECT top 1 M.SKUCD
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,(SELECT top 1 M.AdminNO
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,tbl.CSVJanCD
               ,1   --ArrivalYetFLG
               ,1   --ArrivalPlanKBN
               ,tbl.CSVArriveDate
               ,NULL    --ArrivalDate
               ,0   --StockSu
               ,tbl.CSVAnswerSu
               ,0   --AllowableSu
               ,0   --AnotherStoreAllowableSu
               ,tbl.CSVAnswerSu --ReserveSu
               ,0   --InstructionSu
               ,0   --ShippingSu
               ,NULL    --OriginalStockNO
               ,NULL    --ExpectReturnDate
               ,NULL    --ReturnDate
               ,0   --ReturnSu
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           INNER JOIN D_Order AS DH
            ON tbl.OrderNO = DH.OrderNO
           WHERE tbl.EDIImportRows = @EDIImportRows
           ;
        
        --以下の場合に追加・更新（受発注でかつ、予定日がある程度はっきりしている場合に追加・更新）
        --D_OrderDetails.JuchuuNO≠Null 時
        --かつ
        --ＣＳＶ納品日≠Null
        IF ISNULL(@JuchuuNO,'') <> ''
        BEGIN
               --テーブル転送仕様Ｇ
            --伝票番号採番
            EXEC Fnc_GetNumber
                12,         --in伝票種別 12:引当
                @SYSDATE    , --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @ReserveNO OUTPUT
                ;

            IF ISNULL(@ReserveNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            INSERT INTO [D_Reserve]
                   ([ReserveNO]
                   ,[ReserveKBN]
                   ,[Number]
                   ,[NumberRows]
                   ,[StockNO]
                   ,[SoukoCD]
                   ,[JanCD]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[ReserveSu]
                   ,[ShippingPossibleDate]
                   ,[ShippingPossibleSU]
                   ,[ShippingOrderNO]
                   ,[ShippingOrderRows]
                   ,[CompletedPickingNO]
                   ,[CompletedPickingRow]
                   ,[CompletedPickingDate]
                   ,[ShippingSu]
                   ,[ReturnKBN]
                   ,[OriginalReserveNO]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             SELECT
                    @ReserveNO
                   ,1   --ReserveKBN
                   ,DM.JuchuuNO
                   ,DM.JuchuuRows
                   ,@StockNO
                   ,DH.DestinationSoukoCD
                   ,tbl.CSVJanCD
                   ,(SELECT top 1 M.SKUCD
                        FROM M_SKU AS M
                        WHERE M.SetKBN = 0
                        AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                        AND M.DeleteFlg = 0
                        AND M.JANCD = tbl.CSVJanCD
                    ORDER BY M.ChangeDate desc)
                   ,(SELECT top 1 M.AdminNO
                        FROM M_SKU AS M
                        WHERE M.SetKBN = 0
                        AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                        AND M.DeleteFlg = 0
                        AND M.JANCD = tbl.CSVJanCD
                    ORDER BY M.ChangeDate desc)
                   ,tbl.CSVAnswerSu   --ReserveSu
                   ,NULL    --ShippingPossibleDate
                   ,0   --ShippingPossibleSU
                   ,NULL    --ShippingOrderNO
                   ,0    --ShippingOrderRows
                   ,0   --CompletedPickingNO
                   ,0   --CompletedPickingRow
                   ,NULL    --CompletedPickingDate
                   ,0   --ShippingSu
                   ,0   --ReturnKBN
                   ,NULL    --OriginalReserveNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL    --DeleteOperator
                   ,NULL    --DeleteDateTime
               FROM  @Table tbl
               INNER JOIN D_Order AS DH
                ON tbl.OrderNO = DH.OrderNO
               INNER JOIN D_OrderDetails AS DM
                ON tbl.OrderNO = DM.OrderNO
                AND tbl.OrderRows = DM.OrderRows
               WHERE tbl.EDIImportRows = @EDIImportRows
               ;
        END           
           
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO  @EDIImportRows,@StoreCD,@CSVOrderDate,@JuchuuNO;

    END
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'EDIKaitouNoukiTouroku',
        @PC,
        NULL,
        NULL;
    
--<<OWARI>>
  return @W_ERR;

END

GO
