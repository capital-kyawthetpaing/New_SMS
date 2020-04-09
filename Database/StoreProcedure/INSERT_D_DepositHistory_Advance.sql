 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_DepositHistory_Advance]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_D_DepositHistory_Advance]
(
    @SalesNO varchar(11),
    @SIGN int,
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @Keijobi  varchar(10),
    @Program varchar(50),
    @StoreCD varchar(4),
    @CancelKBN tinyint,
    @RecoredKBN tinyint,
--    @DataNo int,
	@CustomerCD varchar(13),
    @AdvanceAmount money
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

    INSERT INTO [D_DepositHistory]
       ([StoreCD]
       ,[DepositDateTime]
       ,[DataKBN]
       ,[DepositKBN]
       ,[CancelKBN]
       ,[RecoredKBN]
       ,[DenominationCD]
       ,[DepositGaku]
       ,[Remark]
       ,[AccountingDate]
       ,[Number]
       ,[Rows]
       ,[ExchangeMoney]
       ,[ExchangeDenomination]
       ,[ExchangeCount]
       ,[AdminNO]
       ,[SKUCD]
       ,[JanCD]
       ,[SalesSU]
       ,[SalesUnitPrice]
       ,[SalesGaku]
       ,[SalesTax]
       ,[SalesTaxRate]
       ,[TotalGaku]
       ,[Refund]
       ,[IsIssued]
       ,[Program]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime])
 	VALUES(
       @StoreCD
       ,@SYSDATETIME  --DepositDateTime
       ,3   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
       ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
       ,@CancelKBN   --CancelKBN 1:取消、2:返品、3:訂正
       ,@RecoredKBN   --RecoredKBN
       ,(SELECT M.DenominationCD FROM M_DenominationKBN AS M
       	WHERE M.SystemKBN = 13 AND M.MainFLG = 1)    --DenominationCD
       ,@SIGN*@AdvanceAmount   --DepositGaku
       ,NULL    --Remark
       ,(CASE @CancelKBN WHEN 3 THEN convert(date,@Keijobi) 
                   WHEN 1 THEN convert(date,@Keijobi) 
                   ELSE convert(date,@SYSDATETIME) END)  --AccountingDate, date,>
       ,@SalesNO    --Number, varchar(11),>
       ,0   --Rows, tinyint,>★
       ,0   --ExchangeMoney, money,>
       ,0   --ExchangeDenomination, int,>
       ,0   --ExchangeCount, int,>
       ,NULL  --AdminNO, int,>
       ,NULL  --SKUCD, varchar(30),>
       ,NULL  --JanCD, varchar(13),>
       ,0   --SalesSU, money,>
       ,0   --SalesUnitPrice, money,>
       ,0   --SalesGaku, money,>
       ,0   --SalesTax, money,>
       ,0   --SalesTaxRate, int,>
       ,0   --TotalGaku, money,>
       ,0   --Refund, money,>
       ,0   --IsIssued, tinyint,>
       ,@Program    --Program, varchar(100),>
       ,@Operator
       ,@SYSDATETIME
       ,@Operator
       ,@SYSDATETIME
       )
   ;

    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT DH.CollectNO
			,DH.ConfirmSource - DH.ConfirmAmount AS MaeukeKin
        FROM [D_Collect] AS DH

        WHERE DH.StoreCD = @StoreCD
        AND DH.CollectCustomerCD = @CustomerCD
        AND DH.AdvanceFLG = 1	--@AdvanceFLG
        AND DH.ConfirmSource - DH.ConfirmAmount > 0
        AND DH.DeleteDateTime IS NULL
        ORDER BY DH.CollectDate
    ;

	DECLARE @CollectNO varchar(11);
	DECLARE @MaeukeKin money;
	
    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO  @CollectNO,@MaeukeKin;
    
    DECLARE @wHikiate money;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
        --(ConfirmSource－ConfirmAmount）から、順番に"画面.前受金から額"を引当する                                              
        IF @MaeukeKin >= @AdvanceAmount
        BEGIN
            SET @wHikiate = @AdvanceAmount;
            SET @AdvanceAmount = 0;     --★OK?
        END
        ELSE
        BEGIN
            SET @wHikiate = @MaeukeKin;
            SET @AdvanceAmount = @AdvanceAmount - @MaeukeKin;
        END
        
        --テーブル転送仕様Ｋ Insert 店舗前受金　D_StoreAdvance  
        INSERT INTO [D_StoreAdvance]
               ([SalesNO]
               ,[SalesNORows]
               ,[StoreCD]
               ,[CollectNO]
               ,[Mode]
               ,[AdvanceAmount]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
        SELECT
            @SalesNO    --SalesNO
           ,IDENT_CURRENT('D_SalesTran')    --SalesNORows
           ,@StoreCD    --StoreCD
           ,@CollectNO    --CollectNO
           ,(CASE @CancelKBN WHEN 0 THEN 1
           		WHEN 1 THEN 4
           		ELSE @CancelKBN END)    --Mode	1:通常、2:返品、3;訂正、4:取消
           ,@wHikiate    --AdvanceAmount
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ;
           
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
    	INTO  @CollectNO,@MaeukeKin;

    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
END


