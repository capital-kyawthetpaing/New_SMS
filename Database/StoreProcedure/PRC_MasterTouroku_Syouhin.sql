 BEGIN TRY 
 Drop Procedure dbo.[PRC_MasterTouroku_Syouhin]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    商品マスタ
--       Program ID      MasterTouroku_Syouhin
--       Create date:    2020.3.8
--    ======================================================================
--CREATE TYPE [T_Site] AS TABLE(
--	[AdminNO]  int
--   ,[ColorNO]  int
--   ,[SizeNO]  int
--   ,[APIKey] tinyint
--   ,[ShouhinCD] varchar(30)
--   ,[SiteURL] varchar(300)
--)
--GO

--CREATE TYPE [T_SKU] AS TABLE(
--    [RowNO]  int
--   ,[AdminNO]  int
--   ,[SKUCD]  varchar(30)
--   ,[ChangeDate]  date
--   ,[VariousFLG]  tinyint
--   ,[SKUName]  varchar(100)
--   ,[KanaName]  varchar(50)
--   ,[SKUShortName]  varchar(40)
--   ,[EnglishName]  varchar(80)
--   ,[ITemCD]  varchar(30)
--   ,[ColorNO]  int
--   ,[SizeNO]  int
--   ,[JanCD]  varchar(13)
--   ,[SetKBN]  tinyint
--   ,[PresentKBN]  tinyint
--   ,[SampleKBN]  tinyint
--   ,[DiscountKBN]  tinyint
--   ,[ColorName]  varchar(20)
--   ,[SizeName]  varchar(20)
--   ,[WebFlg]  tinyint
--   ,[RealStoreFlg]  tinyint
--   ,[MainVendorCD]  varchar(13)
--   ,[MakerVendorCD]  varchar(13)
--   ,[BrandCD]  varchar(6)
--   ,[MakerItem]  varchar(50)
--   ,[TaniCD]  varchar(2)
--   ,[SportsCD]  varchar(6)
--   ,[SegmentCD] varchar(6)
--   ,[ZaikoKBN]  tinyint
--   ,[Rack]  varchar(10)
--   ,[VirtualFlg]  tinyint
--   ,[DirectFlg]  tinyint
--   ,[ReserveCD]  varchar(3)
--   ,[NoticesCD]  varchar(3)
--   ,[PostageCD]  varchar(3)
--   ,[ManufactCD]  varchar(3)
--   ,[ConfirmCD]  varchar(3)
--   ,[WebStockFlg]  tinyint
--   ,[StopFlg]  tinyint
--   ,[DiscontinueFlg]  tinyint
--   ,[InventoryAddFlg]  tinyint
--   ,[MakerAddFlg]  tinyint
--   ,[StoreAddFlg]  tinyint
--   ,[NoNetOrderFlg]  tinyint
--   ,[EDIOrderFlg]  tinyint
--   ,[CatalogFlg]  tinyint
--   ,[ParcelFlg]  tinyint
--   ,[AutoOrderFlg]  tinyint
--   ,[TaxRateFLG]  tinyint
--   ,[CostingKBN]  tinyint
--   ,[SaleExcludedFlg]  tinyint
--   ,[PriceWithTax]  money
--   ,[PriceOutTax]  money
--   ,[OrderPriceWithTax]  money
--   ,[OrderPriceWithoutTax]  money
--   ,[Rate] decimal(5,2)
--   ,[SaleStartDate]  date
--   ,[WebStartDate]  date
--   ,[OrderAttentionCD]  varchar(3)
--   ,[OrderAttentionNote]  varchar(100)
--   ,[CommentInStore]  varchar(300)
--   ,[CommentOutStore]  varchar(300)
--   ,[LastYearTerm]  varchar(6)
--   ,[LastSeason]  varchar(6)
--   ,[LastCatalogNO]  varchar(20)
--   ,[LastCatalogPage]  varchar(20)
--   ,[LastCatalogText]  varchar(1000)
--   ,[LastInstructionsNO]  varchar(1000)
--   ,[LastInstructionsDate]  date
--   ,[WebAddress]  varchar(200)
--   ,[ApprovalDate]  date
--   ,[TagName1]  varchar(20)
--   ,[TagName2]  varchar(20)
--   ,[TagName3]  varchar(20)
--   ,[TagName4]  varchar(20)
--   ,[TagName5]  varchar(20)
--   ,[TagName6]  varchar(20)
--   ,[TagName7]  varchar(20)
--   ,[TagName8]  varchar(20)
--   ,[TagName9]  varchar(20)
--   ,[TagName10]  varchar(20)
--   ,[SetAdminCD]  int
--   ,[SetItemCD]  varchar(30)
--   ,[SetSKUCD]  varchar(30)
--   ,[SetSU]  int
--   ,[DeleteFlg] [tinyint]
--   ,[UpdateFlg] [tinyint] NULL
--)
--GO

CREATE PROCEDURE PRC_MasterTouroku_Syouhin
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）

    @ITemCD  varchar(30) ,
    @ChangeDate  date ,
    @VariousFLG  tinyint ,
    @ITemName  varchar(40) ,
    @KanaName  varchar(50) ,
    @SKUShortName  varchar(40) ,
    @EnglishName  varchar(80) ,
 --   @ColorNO  int ,
 --   @SizeNO  int ,
 --   @JanCD  varchar(13) ,
    @SetKBN  tinyint ,
    @PresentKBN  tinyint ,
    @SampleKBN  tinyint ,
    @DiscountKBN  tinyint ,
    @ColorName  varchar(20) ,
    @SizeName  varchar(20) ,
    @WebFlg  tinyint ,
    @RealStoreFlg  tinyint ,
    @MainVendorCD  varchar(13) ,
    --@MakerVendorCD  varchar(13) ,
    @BrandCD  varchar(6) ,
    @MakerItem  varchar(50) ,
    @TaniCD  varchar(2) ,
    @SportsCD  varchar(6) ,
    @SegmentCD varchar(6),
    @ZaikoKBN  tinyint ,
    @Rack  varchar(10) ,
    @VirtualFlg  tinyint ,
    @DirectFlg  tinyint ,
    @ReserveCD  varchar(3) ,
    @NoticesCD  varchar(3) ,
    @PostageCD  varchar(3) ,
    @ManufactCD  varchar(3) ,
    @ConfirmCD  varchar(3) ,
    @WebStockFlg  tinyint ,
    @StopFlg  tinyint ,
    @DiscontinueFlg  tinyint ,
    @InventoryAddFlg  tinyint ,
    @MakerAddFlg  tinyint ,
    @StoreAddFlg  tinyint ,
    @NoNetOrderFlg  tinyint ,
    @EDIOrderFlg  tinyint ,
    @CatalogFlg  tinyint ,
    @ParcelFlg  tinyint ,
    @AutoOrderFlg  tinyint ,
    @TaxRateFLG  tinyint ,
    @CostingKBN  tinyint ,
    @SaleExcludedFlg  tinyint ,
    @PriceWithTax  money ,
    @PriceOutTax  money ,
    @OrderPriceWithTax  money ,
    @OrderPriceWithoutTax  money ,
    @Rate decimal(5,2),
    @SaleStartDate  date ,
    @WebStartDate  date ,
    @OrderAttentionCD  varchar(3) ,
    @OrderAttentionNote  varchar(100) ,
    @CommentInStore  varchar(300) ,
    @CommentOutStore  varchar(300) ,
    @LastYearTerm  varchar(6) ,
    @LastSeason  varchar(6) ,
    @LastCatalogNO  varchar(20) ,
    @LastCatalogPage  varchar(20) ,
    @LastCatalogText  varchar(1000) ,
    @LastInstructionsNO  varchar(1000) ,
    @LastInstructionsDate  date ,
    @WebAddress  varchar(200) ,
    @ApprovalDate  date ,
    @DeleteFlg  tinyint ,
    @UsedFlg  tinyint ,
    
    @Table  T_SKU READONLY,
    @SiteTable T_Site READONLY,
    @Operator  varchar(10),
    @PC  varchar(30)
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
    SET @KeyItem = @ITemCD + ' ' + CONVERT(varchar, @ChangeDate,111);
    
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.RowNO, tbl.UpdateFlg, tbl.AdminNO
        	,tbl.TagName1,tbl.TagName2,tbl.TagName3,tbl.TagName4,tbl.TagName5
        	,tbl.TagName6,tbl.TagName7,tbl.TagName8,tbl.TagName9,tbl.TagName10
        FROM @Table AS tbl
        ORDER BY tbl.RowNO
        ;
        
    DECLARE @RowNO int;
    DECLARE @UpdateFlg tinyint;
    DECLARE @AdminNO int;
    DECLARE @TagName1 varchar(20);
    DECLARE @TagName2 varchar(20);
    DECLARE @TagName3 varchar(20);
    DECLARE @TagName4 varchar(20);
    DECLARE @TagName5 varchar(20);
    DECLARE @TagName6 varchar(20);
    DECLARE @TagName7 varchar(20);
    DECLARE @TagName8 varchar(20);
    DECLARE @TagName9 varchar(20);
    DECLARE @TagName10 varchar(20);
    DECLARE @NewAdminNO int;
    
    IF @OperateMode >= 2 --変更・削除
    BEGIN
    	--テーブル転送仕様Ｃ
        DELETE FROM [M_SKUTag]
        WHERE EXISTS(SELECT 1 FROM M_SKU AS MS 
                    WHERE MS.AdminNO = M_SKUTag.AdminNO 
                    AND MS.ChangeDate = M_SKUTag.ChangeDate
                    AND MS.ITemCD = @ITemCD  
                    AND MS.ChangeDate = @ChangeDate);
               
        --テーブル転送仕様Ｇ
        DELETE FROM [M_Site]
        WHERE EXISTS(SELECT 1 FROM M_SKU AS MS 
                    WHERE MS.AdminNO = M_Site.AdminNO
                    AND MS.ITemCD = @ITemCD  
                    AND MS.ChangeDate = @ChangeDate);

        --テーブル転送仕様Ｅ
        --JAN発注単価マスタ（M_JANOrderPrice）
        DELETE FROM [M_JANOrderPrice]
        WHERE EXISTS(SELECT 1 FROM M_SKU AS MS
            WHERE MS.ITemCD = @ITemCD
            AND MS.ChangeDate = @ChangeDate
            AND MS.JanCD = M_JANOrderPrice.JanCD
            AND MS.MainVendorCD = M_JANOrderPrice.VendorCD
            AND MS.ChangeDate = M_JANOrderPrice.ChangeDate
            );
    END
    
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --テーブル転送仕様Ａ
		INSERT INTO [M_ITEM]
           ([ITemCD]
           ,[ChangeDate]
           ,[VariousFLG]
           ,[ITemName]
           ,[KanaName]
           ,[ITEMShortName]	--[SKUShortName]
           ,[EnglishName]
           ,[SetKBN]
           ,[PresentKBN]
           ,[SampleKBN]
           ,[DiscountKBN]
           ,[ColorName]
           ,[SizeName]
           ,[WebFlg]
           ,[RealStoreFlg]
           ,[MainVendorCD]
           ,[MakerVendorCD]
           ,[BrandCD]
           ,[MakerItem]
           ,[TaniCD]
           ,[SportsCD]
           ,[SegmentCD]
           ,[ZaikoKBN]
           ,[Rack]
           ,[VirtualFlg]
           ,[DirectFlg]
           ,[ReserveCD]
           ,[NoticesCD]
           ,[PostageCD]
           ,[ManufactCD]
           ,[ConfirmCD]
           ,[WebStockFlg]
           ,[StopFlg]
           ,[DiscontinueFlg]
           ,[InventoryAddFlg]
           ,[MakerAddFlg]
           ,[StoreAddFlg]
           ,[NoNetOrderFlg]
           ,[EDIOrderFlg]
           ,[CatalogFlg]
           ,[ParcelFlg]
           ,[AutoOrderFlg]
           ,[TaxRateFLG]
           ,[CostingKBN]
           ,[SaleExcludedFlg]
           ,[PriceWithTax]
           ,[PriceOutTax]
           ,[OrderPriceWithTax]
           ,[OrderPriceWithoutTax]
           ,[Rate]
           ,[SaleStartDate]
           ,[WebStartDate]
           ,[OrderAttentionCD]
           ,[OrderAttentionNote]
           ,[CommentInStore]
           ,[CommentOutStore]
           ,[LastYearTerm]
           ,[LastSeason]
           ,[LastCatalogNO]
           ,[LastCatalogPage]
           ,[LastCatalogText]
           ,[LastInstructionsNO]
           ,[LastInstructionsDate]
           ,[WebAddress]
           ,[ApprovalDate]
           ,[ApprovalDateTime]
           ,[DeleteFlg]
           ,[UsedFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	VALUES
           (@ITemCD
           ,@ChangeDate
           ,@VariousFLG
           ,@ITemName
           ,@KanaName
           ,@SKUShortName
           ,@EnglishName
           ,@SetKBN
           ,@PresentKBN
           ,@SampleKBN
           ,@DiscountKBN
           ,@ColorName
           ,@SizeName
           ,@WebFlg
           ,@RealStoreFlg
           ,@MainVendorCD
           ,@MainVendorCD 	--MakerVendorCD
           ,@BrandCD
           ,@MakerItem
           ,@TaniCD
           ,@SportsCD
           ,@SegmentCD
           ,@ZaikoKBN
           ,@Rack
           ,@VirtualFlg
           ,@DirectFlg
           ,@ReserveCD
           ,@NoticesCD
           ,@PostageCD
           ,@ManufactCD
           ,@ConfirmCD
           ,@WebStockFlg
           ,@StopFlg
           ,@DiscontinueFlg
           ,@InventoryAddFlg
           ,@MakerAddFlg
           ,@StoreAddFlg
           ,@NoNetOrderFlg
           ,@EDIOrderFlg
           ,@CatalogFlg
           ,@ParcelFlg
           ,@AutoOrderFlg
           ,@TaxRateFLG
           ,@CostingKBN
           ,@SaleExcludedFlg
           ,@PriceWithTax
           ,@PriceOutTax
           ,@OrderPriceWithTax
           ,@OrderPriceWithoutTax
           ,@Rate
           ,@SaleStartDate
           ,@WebStartDate
           ,@OrderAttentionCD
           ,@OrderAttentionNote
           ,@CommentInStore
           ,@CommentOutStore
           ,@LastYearTerm
           ,@LastSeason
           ,@LastCatalogNO
           ,@LastCatalogPage
           ,@LastCatalogText
           ,@LastInstructionsNO
           ,@LastInstructionsDate
           ,@WebAddress
           ,@ApprovalDate
           ,(CASE @ApprovalDate WHEN NULL THEN NULL
                ELSE @SYSDATETIME END)  --ApprovalDateTime
           ,@DeleteFlg
           ,@UsedFlg
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
		);

        --テーブル転送仕様Ｄ
        --ITEM販売単価マスタ（M_ItemPrice）
        INSERT INTO [M_ItemPrice]
           ([ITemCD]
           ,[ChangeDate]
           ,[PriceWithTax]
           ,[PriceWithoutTax]
           ,[GeneralRate]
           ,[GeneralPriceWithTax]
           ,[GeneralPriceOutTax]
           ,[MemberRate]
           ,[MemberPriceWithTax]
           ,[MemberPriceOutTax]
           ,[ClientRate]
           ,[ClientPriceWithTax]
           ,[ClientPriceOutTax]
           ,[SaleRate]
           ,[SalePriceWithTax]
           ,[SalePriceOutTax]
           ,[WebRate]
           ,[WebPriceWithTax]
           ,[WebPriceOutTax]
           ,[Remarks]
           ,[DeleteFlg]
           ,[UsedFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     VALUES (
     		@ITemCD
           ,@ChangeDate
           ,@PriceWithTax
           ,@PriceOutTax
           ,@Rate	--[GeneralRate]
           ,@PriceWithTax	--[GeneralPriceWithTax]
           ,@PriceOutTax	--[GeneralPriceOutTax]
           ,@Rate	--[MemberRate]
           ,@PriceWithTax	--[MemberPriceWithTax]
           ,@PriceOutTax	--[MemberPriceOutTax]
           ,@Rate	--[ClientRate]
           ,@PriceWithTax	--[ClientPriceWithTax]
           ,@PriceOutTax	--[ClientPriceOutTax]
           ,@Rate	--[SaleRate]
           ,@PriceWithTax	--[SalePriceWithTax]
           ,@PriceOutTax	--[SalePriceOutTax]
           ,@Rate	--[WebRate]
           ,@PriceWithTax	--[WebPriceWithTax]
           ,@PriceOutTax	--[WebPriceOutTax]
           ,NULL	--[Remarks]
           ,0	--DeleteFlg
           ,0	--UsedFlg
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           );

    END;
    
    ELSE IF @OperateMode = 2 --変更--
    BEGIN
        SET @OperateModeNm = '変更';
        
        --テーブル転送仕様Ａ
        UPDATE [M_ITEM] SET
           [VariousFLG]             = @VariousFLG
           ,[ITemName]               = @ITemName
           ,[KanaName]               = @KanaName
           ,[ITEMShortName]	--[SKUShortName]           
           							 = @SKUShortName
           ,[EnglishName]            = @EnglishName
           ,[SetKBN]                 = @SetKBN
           ,[PresentKBN]             = @PresentKBN
           ,[SampleKBN]              = @SampleKBN
           ,[DiscountKBN]            = @DiscountKBN
           ,[ColorName]              = @ColorName
           ,[SizeName]               = @SizeName
           ,[WebFlg]                 = @WebFlg
           ,[RealStoreFlg]           = @RealStoreFlg
           ,[MainVendorCD]           = @MainVendorCD
           ,[MakerVendorCD]          = @MainVendorCD
           ,[BrandCD]                = @BrandCD
           ,[MakerItem]              = @MakerItem
           ,[TaniCD]                 = @TaniCD
           ,[SportsCD]               = @SportsCD
           ,[SegmentCD]              = @SegmentCD
           ,[ZaikoKBN]               = @ZaikoKBN
           ,[Rack]                   = @Rack
           ,[VirtualFlg]             = @VirtualFlg
           ,[DirectFlg]              = @DirectFlg
           ,[ReserveCD]              = @ReserveCD
           ,[NoticesCD]              = @NoticesCD
           ,[PostageCD]              = @PostageCD
           ,[ManufactCD]             = @ManufactCD
           ,[ConfirmCD]              = @ConfirmCD
           ,[WebStockFlg]            = @WebStockFlg
           ,[StopFlg]                = @StopFlg
           ,[DiscontinueFlg]         = @DiscontinueFlg
           ,[InventoryAddFlg]        = @InventoryAddFlg
           ,[MakerAddFlg]            = @MakerAddFlg
           ,[StoreAddFlg]            = @StoreAddFlg
           ,[NoNetOrderFlg]          = @NoNetOrderFlg
           ,[EDIOrderFlg]            = @EDIOrderFlg
           ,[CatalogFlg]             = @CatalogFlg
           ,[ParcelFlg]              = @ParcelFlg
           ,[AutoOrderFlg]           = @AutoOrderFlg
           ,[TaxRateFLG]             = @TaxRateFLG
           ,[CostingKBN]             = @CostingKBN
           ,[SaleExcludedFlg]        = @SaleExcludedFlg
           ,[PriceWithTax]           = @PriceWithTax
           ,[PriceOutTax]            = @PriceOutTax
           ,[OrderPriceWithTax]      = @OrderPriceWithTax
           ,[OrderPriceWithoutTax]   = @OrderPriceWithoutTax
           ,[Rate]                   = @Rate
           ,[SaleStartDate]          = @SaleStartDate
           ,[WebStartDate]           = @WebStartDate
           ,[OrderAttentionCD]       = @OrderAttentionCD
           ,[OrderAttentionNote]     = @OrderAttentionNote
           ,[CommentInStore]         = @CommentInStore
           ,[CommentOutStore]        = @CommentOutStore
           ,[LastYearTerm]           = @LastYearTerm
           ,[LastSeason]             = @LastSeason
           ,[LastCatalogNO]          = @LastCatalogNO
           ,[LastCatalogPage]        = @LastCatalogPage
           ,[LastCatalogText]        = @LastCatalogText
           ,[LastInstructionsNO]     = @LastInstructionsNO
           ,[LastInstructionsDate]   = @LastInstructionsDate
           ,[WebAddress]             = @WebAddress
           ,[ApprovalDate]           = @ApprovalDate
           ,[ApprovalDateTime]       = (CASE @ApprovalDate WHEN NULL THEN NULL
                						ELSE @SYSDATETIME END) 
           ,[DeleteFlg]              = @DeleteFlg
           ,[UsedFlg]                = @UsedFlg
          ,[UpdateOperator]       = @Operator  
          ,[UpdateDateTime]       = @SYSDATETIME
        WHERE [ITemCD] = @ITemCD  
       AND [ChangeDate] = @ChangeDate
       ;
        
        --テーブル転送仕様Ｄ
		UPDATE[M_ItemPrice] SET
       		[PriceWithTax]    = @PriceWithTax
           ,[PriceWithoutTax] = @PriceOutTax
           ,[UpdateOperator]  = @Operator  
           ,[UpdateDateTime]   = @SYSDATETIME
        WHERE [ITemCD] = @ITemCD  
       AND [ChangeDate] = @ChangeDate
       ;

    END

    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';

        DELETE FROM [M_ITEM]
         WHERE [ITemCD] = @ITemCD  
           AND [ChangeDate] = @ChangeDate
           ;
           
		--テーブル転送仕様Ｄ
        DELETE FROM [M_ItemPrice]
         WHERE [ITemCD] = @ITemCD  
           AND [ChangeDate] = @ChangeDate
           ;

        --テーブル転送仕様Ｆ
        --ITEM発注単価マスタ（M_ItemOrderPrice）
        DELETE FROM [M_ItemOrderPrice]
        WHERE EXISTS(SELECT 1 FROM M_SKU AS MS 
                    WHERE MS.ITemCD = @ITemCD  
                    AND MS.ChangeDate = @ChangeDate
                    AND MS.MakerItem = M_ItemOrderPrice.MakerItem
                    AND MS.MainVendorCD =M_ItemOrderPrice.VendorCD
                    AND MS.ChangeDate = M_ItemOrderPrice.ChangeDate
                    );
  
        DELETE FROM [M_SKUInfo]
        WHERE EXISTS(SELECT 1 FROM M_SKU AS MS 
                    WHERE MS.ITemCD = @ITemCD  
                    AND MS.ChangeDate = @ChangeDate
                    AND MS.AdminNO = M_SKUInfo.AdminNO
                    AND MS.ChangeDate =M_SKUInfo.ChangeDate
                    AND 1 = M_SKUInfo.SEQ
                    );
                                                           
        DELETE FROM [M_SKU]
         WHERE [ITemCD] = @ITemCD  
           AND [ChangeDate] = @ChangeDate
           ;


    END

    IF @OperateMode <= 2
    BEGIN
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @RowNO, @UpdateFlg, @AdminNO
        	,@TagName1,@TagName2,@TagName3,@TagName4,@TagName5
        	,@TagName6,@TagName7,@TagName8,@TagName9,@TagName10;

        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            IF @UpdateFlg = 0 OR @OperateMode = 1
            BEGIN
                --テーブル転送仕様Ｂ
                INSERT INTO [M_SKU]
                       ([AdminNO]
                      ,[SKUCD]
                      ,[ChangeDate]
                      ,[VariousFLG]
                      ,[SKUName]
                      ,[KanaName]
                      ,[SKUShortName]
                      ,[EnglishName]
                      ,[ITemCD]
                      ,[ColorNO]
                      ,[SizeNO]
                      ,[JanCD]
                      ,[SetKBN]
                      ,[PresentKBN]
                      ,[SampleKBN]
                      ,[DiscountKBN]
                      ,[ColorName]
                      ,[SizeName]
                      ,[WebFlg]
                      ,[RealStoreFlg]
                      ,[MainVendorCD]
                      ,[MakerVendorCD]
                      ,[BrandCD]
                      ,[MakerItem]
                      ,[TaniCD]
                      ,[SportsCD]
                      ,[SegmentCD]
                      ,[ZaikoKBN]
                      ,[Rack]
                      ,[VirtualFlg]
                      ,[DirectFlg]
                      ,[ReserveCD]
                      ,[NoticesCD]
                      ,[PostageCD]
                      ,[ManufactCD]
                      ,[ConfirmCD]
                      ,[WebStockFlg]
                      ,[StopFlg]
                      ,[DiscontinueFlg]
                      ,[InventoryAddFlg]
                      ,[MakerAddFlg]
                      ,[StoreAddFlg]
                      ,[NoNetOrderFlg]
                      ,[EDIOrderFlg]
                      ,[CatalogFlg]
                      ,[ParcelFlg]
                      ,[AutoOrderFlg]
                      ,[TaxRateFLG]
                      ,[CostingKBN]
                      ,[SaleExcludedFlg]
                      ,[PriceWithTax]
                      ,[PriceOutTax]
                      ,[OrderPriceWithTax]
                      ,[OrderPriceWithoutTax]
                      ,[Rate]
                      ,[SaleStartDate]
                      ,[WebStartDate]
                      ,[OrderAttentionCD]
                      ,[OrderAttentionNote]
                      ,[CommentInStore]
                      ,[CommentOutStore]
                      ,[LastYearTerm]
                      ,[LastSeason]
                      ,[LastCatalogNO]
                      ,[LastCatalogPage]
                      ,[LastCatalogText]
                      ,[LastInstructionsNO]
                      ,[LastInstructionsDate]
                      ,[WebAddress]
                      ,[SetAdminCD]
                      ,[SetItemCD]
                      ,[SetSKUCD]
                      ,[SetSU]
                      ,[ApprovalDate]
                      ,[DeleteFlg]
                      ,[UsedFlg]
                      ,[InsertOperator]
                      ,[InsertDateTime]
                      ,[UpdateOperator]
                      ,[UpdateDateTime])
                 SELECT
                        (SELECT A.AdminNO + 1 FROM M_SKUCounter AS A
                            WHERE A.MainKey = 1) AS AdminNO
                       ,tbl.SKUCD
                       ,tbl.ChangeDate
                       ,tbl.VariousFLG
                       ,tbl.SKUName
                       ,tbl.KanaName
                       ,tbl.SKUShortName
                       ,tbl.EnglishName
                       ,tbl.ITemCD
                       ,tbl.ColorNO
                       ,tbl.SizeNO
                       ,tbl.JanCD
                       ,tbl.SetKBN
                       ,tbl.PresentKBN
                       ,tbl.SampleKBN
                       ,tbl.DiscountKBN
                       ,tbl.ColorName
                       ,tbl.SizeName
                       ,tbl.WebFlg
                       ,tbl.RealStoreFlg
                       ,tbl.MainVendorCD
                       ,tbl.MakerVendorCD
                       ,tbl.BrandCD
                       ,tbl.MakerItem
                       ,tbl.TaniCD
                       ,tbl.SportsCD
                       ,tbl.SegmentCD
                       ,tbl.ZaikoKBN
                       ,tbl.Rack
                       ,tbl.VirtualFlg
                       ,tbl.DirectFlg
                       ,tbl.ReserveCD
                       ,tbl.NoticesCD
                       ,tbl.PostageCD
                       ,tbl.ManufactCD
                       ,tbl.ConfirmCD
                       ,tbl.WebStockFlg
                       ,tbl.StopFlg
                       ,tbl.DiscontinueFlg
                       ,tbl.InventoryAddFlg
                       ,tbl.MakerAddFlg
                       ,tbl.StoreAddFlg
                       ,tbl.NoNetOrderFlg
                       ,tbl.EDIOrderFlg
                       ,tbl.CatalogFlg
                       ,tbl.ParcelFlg
                       ,tbl.AutoOrderFlg
                       ,tbl.TaxRateFLG
                       ,tbl.CostingKBN
                       ,tbl.SaleExcludedFlg
                       ,tbl.PriceWithTax
                       ,tbl.PriceOutTax
                       ,tbl.OrderPriceWithTax
                       ,tbl.OrderPriceWithoutTax
                       ,tbl.Rate
                       ,tbl.SaleStartDate
                       ,tbl.WebStartDate
                       ,tbl.OrderAttentionCD
                       ,tbl.OrderAttentionNote
                       ,tbl.CommentInStore
                       ,tbl.CommentOutStore
                       ,tbl.LastYearTerm
                       ,tbl.LastSeason
                       ,tbl.LastCatalogNO
                       ,tbl.LastCatalogPage
                       ,tbl.LastCatalogText
                       ,tbl.LastInstructionsNO
                       ,tbl.LastInstructionsDate
                       ,tbl.WebAddress
                       ,tbl.SetAdminCD
                       ,tbl.SetItemCD
                       ,tbl.SetSKUCD
                       ,tbl.SetSU
                       ,tbl.ApprovalDate
                       ,tbl.DeleteFlg
                       ,0 AS UsedFlg
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                    FROM @Table AS tbl
                    WHERE tbl.RowNO = @RowNO;

                UPDATE M_SKUCounter SET
                    AdminNO = AdminNO + 1
                WHERE MainKey = 1
                ;
                
                SET @NewAdminNO = (SELECT MAX(AdminNO) FROM M_SKUCounter WHERE MainKey = 1);
                
                --テーブル転送仕様Ｃ
                --SKUタグマスタ（M_SKUTag）
                EXEC INSERT_M_SKUTag
                    @TagName1 ,@NewAdminNO ,@ChangeDate;
                   
                EXEC INSERT_M_SKUTag
                    @TagName2 ,@NewAdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                    @TagName3 ,@NewAdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                    @TagName4 ,@NewAdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                    @TagName5 ,@NewAdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                    @TagName6 ,@NewAdminNO ,@ChangeDate;
                    
                EXEC INSERT_M_SKUTag
                    @TagName7 ,@NewAdminNO ,@ChangeDate;
                    
                EXEC INSERT_M_SKUTag
                    @TagName8 ,@NewAdminNO ,@ChangeDate;
                    
                EXEC INSERT_M_SKUTag
                    @TagName9 ,@NewAdminNO ,@ChangeDate;
                    
                EXEC INSERT_M_SKUTag
                    @TagName10 ,@NewAdminNO ,@ChangeDate;
                
                --テーブル転送仕様Ｇ
                INSERT INTO [M_Site]
                    ([AdminNO]
                   ,[APIKey]
                   ,[ShouhinCD]
                   ,[SiteURL]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                SELECT
                    @NewAdminNO --[AdminNO]
                   ,ST.APIKey
                   ,ST.ShouhinCD
                   ,(SELECT top 1 A.ShopURL FROM M_API AS A 
                        WHERE A.APIKey = ST.APIKey
                        AND A.ChangeDate <= @ChangeDate
                        ORDER BY A.ChangeDate DESC) + ST.SiteURL + '.html'
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                FROM @SiteTable AS ST
                INNER JOIN @Table AS tbl 
                ON tbl.RowNO = @RowNO
                AND tbl.ColorNO = ST.ColorNO
                AND tbl.SizeNO = ST.SizeNO
                ;
                
                --テーブル転送仕様Ｈ
                INSERT INTO [M_SKUInfo]
                   ([AdminNO]
                   ,[ChangeDate]
                   ,[SEQ]
                   ,[YearTerm]
                   ,[Season]
                   ,[CatalogNO]
                   ,[CatalogPage]
                   ,[CatalogText]
                   ,[InstructionsNO]
                   ,[InstructionsDate]
                   ,[DeleteFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                SELECT @NewAdminNO  --[AdminNO]
                   ,@ChangeDate
                   ,1   --[SEQ]
                   ,tbl.LastYearTerm    --[YearTerm]
                   ,tbl.LastSeason      --[Season]
                   ,tbl.LastCatalogNO   --[CatalogNO]
                   ,tbl.LastCatalogPage --[CatalogPage]
                   ,tbl.LastCatalogText --[CatalogText]
                   ,tbl.LastInstructionsNO      --[InstructionsNO]
                   ,tbl.LastInstructionsDate    --[InstructionsDate]
                   ,0 --[DeleteFlg]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                FROM @Table AS tbl
                WHERE tbl.RowNO = @RowNO;

            END
        
            ELSE IF @UpdateFlg = 1
            BEGIN
                --テーブル転送仕様Ｂ
                UPDATE [M_SKU]
                   SET [SKUCD]           = tbl.SKUCD
                      ,[SKUName]         = tbl.SKUName
                      ,[KanaName]        = tbl.KanaName
                      ,[SKUShortName]    = tbl.SKUShortName
                      ,[EnglishName]     = tbl.EnglishName
                      ,[ITemCD]          = tbl.ITemCD
                      ,[ColorNO]         = tbl.ColorNO
                      ,[SizeNO]          = tbl.SizeNO
                      ,[JanCD]           = tbl.JanCD
                      ,[SetKBN]          = tbl.SetKBN
                      ,[PresentKBN]      = tbl.PresentKBN
                      ,[SampleKBN]       = tbl.SampleKBN
                      ,[DiscountKBN]     = tbl.DiscountKBN
                      ,[ColorName]       = tbl.ColorName
                      ,[SizeName]        = tbl.SizeName
                      ,[WebFlg]          = tbl.WebFlg
                      ,[RealStoreFlg]    = tbl.RealStoreFlg
                      ,[MainVendorCD]    = tbl.MainVendorCD
                      ,[MakerVendorCD]   = tbl.MakerVendorCD
                      ,[BrandCD]         = tbl.BrandCD
                      ,[MakerItem]       = tbl.MakerItem
                      ,[TaniCD]          = tbl.TaniCD
                      ,[SportsCD]        = tbl.SportsCD
                      ,[SegmentCD]       = tbl.SegmentCD
                      ,[ZaikoKBN]        = tbl.ZaikoKBN
                      ,[Rack ]           = tbl.Rack
                      ,[VirtualFlg]      = tbl.VirtualFlg
                      ,[DirectFlg]       = tbl.DirectFlg
                      ,[ReserveCD]       = tbl.ReserveCD
                      ,[NoticesCD]       = tbl.NoticesCD
                      ,[PostageCD]       = tbl.PostageCD
                      ,[ManufactCD]      = tbl.ManufactCD
                      ,[ConfirmCD]       = tbl.ConfirmCD
                      ,[WebStockFlg]     = tbl.WebStockFlg
                      ,[StopFlg]         = tbl.StopFlg
                      ,[DiscontinueFlg]  = tbl.DiscontinueFlg
                      ,[InventoryAddFlg] = tbl.InventoryAddFlg
                      ,[MakerAddFlg]     = tbl.MakerAddFlg
                      ,[StoreAddFlg]     = tbl.StoreAddFlg
                      ,[NoNetOrderFlg]   = tbl.NoNetOrderFlg
                      ,[EDIOrderFlg]     = tbl.EDIOrderFlg
                      ,[CatalogFlg]      = tbl.CatalogFlg
                      ,[ParcelFlg]       = tbl.ParcelFlg
                      ,[AutoOrderFlg]    = tbl.AutoOrderFlg
                      ,[TaxRateFLG]      = tbl.TaxRateFLG
                      ,[CostingKBN]      = tbl.CostingKBN
                      ,[SaleExcludedFlg] = tbl.SaleExcludedFlg
                      ,[PriceWithTax]    = tbl.PriceWithTax
                      ,[PriceOutTax]     = tbl.PriceOutTax
                      ,[OrderPriceWithTax]   = tbl.OrderPriceWithTax
                      ,[OrderPriceWithoutTax] = tbl.OrderPriceWithoutTax
                      ,[Rate]               = tbl.Rate
                      ,[SaleStartDate]      = tbl.SaleStartDate
                      ,[WebStartDate]       = tbl.WebStartDate
                      ,[OrderAttentionCD]   = tbl.OrderAttentionCD
                      ,[OrderAttentionNote] = tbl.OrderAttentionNote
                      ,[CommentInStore]     = tbl.CommentInStore
                      ,[CommentOutStore]    = tbl.CommentOutStore
                      ,[LastYearTerm]       = tbl.LastYearTerm
                      ,[LastSeason]         = tbl.LastSeason
                      ,[LastCatalogNO]      = tbl.LastCatalogNO
                      ,[LastCatalogPage]    = tbl.LastCatalogPage
                      ,[LastCatalogText]    = tbl.LastCatalogText
                      ,[LastInstructionsNO] = tbl.LastInstructionsNO
                      ,[LastInstructionsDate] = tbl.LastInstructionsDate
                      ,[WebAddress]           = tbl.WebAddress
                      ,[SetAdminCD]           = tbl.SetAdminCD
                      ,[SetItemCD]            = tbl.SetItemCD
                      ,[SetSKUCD]             = tbl.SetSKUCD
                      ,[SetSU]                = tbl.SetSU
                      ,[ApprovalDate]         = tbl.ApprovalDate
                      ,[DeleteFlg]            = tbl.DeleteFlg
                      ,[UpdateOperator]       = @Operator  
                      ,[UpdateDateTime]       = @SYSDATETIME
                 FROM @Table AS tbl
                 WHERE [M_SKU].[AdminNO] = tbl.AdminNO  
                   AND [M_SKU].[ChangeDate] = @ChangeDate
                   AND tbl.RowNO = @RowNO
                   ;

                --テーブル転送仕様Ｃ
                --SKUタグマスタ（M_SKUTag）
                EXEC INSERT_M_SKUTag
                	@TagName1 ,@AdminNO ,@ChangeDate;
                   
                EXEC INSERT_M_SKUTag
                	@TagName2 ,@AdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                	@TagName3 ,@AdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                	@TagName4 ,@AdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                	@TagName5 ,@AdminNO ,@ChangeDate;
                
                EXEC INSERT_M_SKUTag
                	@TagName6 ,@AdminNO ,@ChangeDate;
                	
                EXEC INSERT_M_SKUTag
                	@TagName7 ,@AdminNO ,@ChangeDate;
                	
                EXEC INSERT_M_SKUTag
                	@TagName8 ,@AdminNO ,@ChangeDate;
                	
                EXEC INSERT_M_SKUTag
                	@TagName9 ,@AdminNO ,@ChangeDate;
                	
                EXEC INSERT_M_SKUTag
                	@TagName10 ,@AdminNO ,@ChangeDate;

                INSERT INTO [M_Site]
                    ([AdminNO]
                   ,[APIKey]
                   ,[ShouhinCD]
                   ,[SiteURL]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                SELECT
                    @AdminNO --[AdminNO]
                   ,ST.APIKey
                   ,ST.ShouhinCD
                   ,(SELECT top 1 A.ShopURL FROM M_API AS A 
                        WHERE A.APIKey = ST.APIKey
                        AND A.ChangeDate <= @ChangeDate
                        ORDER BY A.ChangeDate DESC) + ST.SiteURL + '.html'
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                FROM @SiteTable AS ST
                INNER JOIN @Table AS tbl 
                ON tbl.RowNO = @RowNO
                AND tbl.ColorNO = ST.ColorNO
                AND tbl.SizeNO = ST.SizeNO
                ;
                
                --テーブル転送仕様Ｈ
                UPDATE [M_SKUInfo] SET
                    [YearTerm]           = tbl.LastYearTerm   
                   ,[Season]             = tbl.LastSeason     
                   ,[CatalogNO]          = tbl.LastCatalogNO  
                   ,[CatalogPage]        = tbl.LastCatalogPage
                   ,[CatalogText]        = tbl.LastCatalogText
                   ,[InstructionsNO]     = tbl.LastInstructionsNO  
                   ,[InstructionsDate]   = tbl.LastInstructionsDate
                   ,[UpdateOperator]     = @Operator
                   ,[UpdateDateTime]     = @SYSDATETIME
                 FROM @Table AS tbl
                 WHERE [M_SKUInfo].[AdminNO] = tbl.AdminNO  
                   AND [M_SKUInfo].[ChangeDate] = @ChangeDate
                   AND [M_SKUInfo].[SEQ] = 1
                   AND tbl.RowNO = @RowNO
                ;
            END
                
            --画面.主要仕入先CDに入力があった場合
            --テーブル転送仕様Ｅ
            UPDATE [M_JANOrderPrice] SET
                [Rate]             = tbl.Rate
               ,[PriceWithoutTax]  = tbl.PriceOutTax
               ,[UpdateOperator]   = @Operator  
               ,[UpdateDateTime]   = @SYSDATETIME
             FROM @Table AS tbl
             WHERE [M_JANOrderPrice].[JANCD] = tbl.JANCD  
               AND [M_JANOrderPrice].[VendorCD] = tbl.MainVendorCD
               AND [M_JANOrderPrice].[ChangeDate] = @ChangeDate
               AND tbl.RowNO = @RowNO
               AND ISNULL(tbl.MainVendorCD,'') <> '';    
            
            IF @@ROWCOUNT = 0
            BEGIN
                --画面.主要仕入先CDに入力があった場合
                --テーブル転送仕様Ｅ
                --JAN発注単価マスタ（M_JANOrderPrice）
                INSERT INTO [M_JANOrderPrice]
                       ([JanCD]
                       ,[VendorCD]
                       ,[ChangeDate]
                       ,[Rate]
                       ,[PriceWithoutTax]
                       ,[Remarks]
                       ,[DeleteFlg]
                       ,[UsedFlg]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                 SELECT
                        tbl.JanCD
                       ,tbl.MainVendorCD
                       ,@ChangeDate
                       ,tbl.Rate
                       ,tbl.PriceOutTax
                       ,NULL AS Remarks
                       ,0   --[DeleteFlg]
                       ,0   --[UsedFlg]
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                FROM @Table AS tbl
                WHERE tbl.RowNO = @RowNO
                AND ISNULL(tbl.MainVendorCD,'') <> '';           
            
            END
               
            --画面.主要仕入先CDに入力があった場合                
            --テーブル転送仕様Ｆ
            --ITEM発注単価マスタ（M_ItemOrderPrice）
            UPDATE [M_ItemOrderPrice] SET
                [Rate]             = tbl.Rate
               ,[PriceWithoutTax]  = tbl.PriceOutTax
               ,[UpdateOperator]   = @Operator  
               ,[UpdateDateTime]   = @SYSDATETIME
             FROM @Table AS tbl
             WHERE [M_ItemOrderPrice].[MakerItem] = tbl.MakerItem  
               AND [M_ItemOrderPrice].[VendorCD] = tbl.MainVendorCD
               AND [M_ItemOrderPrice].[ChangeDate] = @ChangeDate
               AND tbl.RowNO = @RowNO
               ;
            
            IF @@ROWCOUNT = 0
            BEGIN
            	--テーブル転送仕様Ｆ
                --ITEM発注単価マスタ（M_ItemOrderPrice）
                INSERT INTO [M_ItemOrderPrice]
                   ([MakerItem]
                   ,[VendorCD]
                   ,[ChangeDate]
                   ,[Rate]
                   ,[PriceWithoutTax]
                   --,[Remarks]
                   ,[DeleteFlg]
                   ,[UsedFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                 SELECT
                    tbl.MakerItem
                   ,tbl.MainVendorCD
                   ,@ChangeDate
                   ,tbl.Rate
                   ,tbl.PriceOutTax
                   --,NULL AS Remarks
                   ,0   --[DeleteFlg]
                   ,0   --[UsedFlg]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                FROM @Table AS tbl
                WHERE tbl.RowNO = @RowNO
                AND ISNULL(tbl.MainVendorCD,'') <> '';
            END
                                                    
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @RowNO, @UpdateFlg, @AdminNO
            ,@TagName1,@TagName2,@TagName3,@TagName4,@TagName5
            ,@TagName6,@TagName7,@TagName8,@TagName9,@TagName10;

        END
    END
    
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME ,
        @Operator,
        'MasterTouroku_Syouhin',
        @PC,
        @OperateModeNm,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END

