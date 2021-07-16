BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectForSearchProduct]
END try
BEGIN CATCH END CATCH 

/****** Object:  StoredProcedure [dbo].[M_SKU_SelectForSearchProduct]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SKU_SelectForSearchProduct]    */
CREATE PROCEDURE M_SKU_SelectForSearchProduct(
    -- Add the parameters for the stored procedure here
    @MainVendorCD varchar(13),
    @BrandCD varchar(6),
    @SKUName varchar(100),
    @JanCD varchar(13), 
    @SKUCD varchar(30),
    @CommentInStore varchar(300) ,		--カンマ区切り
    @OrOrAnd tinyInt,
    @ReserveCD varchar(3),
    @NoticesCD varchar(3),
    @PostageCD varchar(3),
    @OrderAttentionCD varchar(3),
    @SportsCD varchar(6),
    @TagName1 varchar(20),
    @TagName2 varchar(20),
    @TagName3 varchar(20),
    @TagName4 varchar(20),
    @TagName5 varchar(20),
    
    @YearTerm varchar(6),
    @Season varchar(6),
    @CatalogNO varchar(20),
    @InstructionsNO varchar(1000),
    
    @MakerItem varchar(300) ,	--カンマ区切り
    @ITemCD varchar(300) ,		--カンマ区切り
    @InputDateFrom varchar(10),
    @InputDateTo varchar(10), 
    @UpdateDateFrom  varchar(10),
    @UpdateDateTo  varchar(10),
    @ApprovalDateFrom varchar(10),
    @ApprovalDateTo varchar(10),
    @ChangeDate varchar(10),
    @SearchFlg tinyInt,
    @ItemOrMaker tinyInt,
    @NotApproved tinyint = 0
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @dChangeDate date
    IF ISDATE(@ChangeDate) = 0
        SET @dChangeDate = CONVERT(date, '1900-1-1')
    ELSE
        SET @dChangeDate = CONVERT(date, @ChangeDate)

    --------------------------------------------------------
    -- Expand a comma-separated string into a table variable
    --------------------------------------------------------
    -- @CommentInStoreList 
    DECLARE @CommentInStoreList TABLE (seq int identity, Item varchar(1000))
    IF ISNULL(@CommentInStore,'') <> ''
    BEGIN
        INSERT INTO @CommentInStoreList (Item) SELECT CONCAT('%',TRIM(Item),'%') FROM dbo.SplitString(@CommentInStore , ',' )
    END

    -- @MakerItem
    DECLARE @MakerItemList TABLE (seq int identity, Item varchar(1000))
    IF ISNULL(@MakerItem,'') <> ''
    BEGIN
        INSERT INTO @MakerItemList (Item) SELECT CONCAT('%',TRIM(Item),'%') FROM dbo.SplitString(@MakerItem , ',' )

    END

    -- @ItemCD
    DECLARE @ItemList TABLE (seq int identity, Item varchar(1000))
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
        INSERT INTO @ItemList (Item) SELECT CONCAT('%',TRIM(Item),'%') FROM dbo.SplitString(@ITemCD , ',' )
    END

    IF OBJECT_ID( N'[tempdb]..[#tmpSKUTag]', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#TableForSearchProduct];
    END

    --------------------------------
    -- create temp table(M_SKUTag)
    --------------------------------
    CREATE TABLE #tmpSKUTag
    (
         AdminNO int not null
        ,ChangeDate date not null
        ,TagName1 varchar(20) collate database_default
        ,TagName2 varchar(20) collate database_default
        ,TagName3 varchar(20) collate database_default
        ,TagName4 varchar(20) collate database_default
        ,TagName5 varchar(20) collate database_default
    )
    ALTER table #tmpSKUTag ADD PRIMARY KEY CLUSTERED 
    (
         AdminNO asc
        ,ChangeDate asc
    )

    INSERT INTO #tmpSKUTag
    SELECT
         W.AdminNo
        ,W.ChangeDate
        ,MAX(CASE WHEN RowNum=1 THEN  W.TagName ELSE NULL END) AS TagName1
        ,MAX(CASE WHEN RowNum=2 THEN  W.TagName ELSE NULL END) AS TagName2
        ,MAX(CASE WHEN RowNum=3 THEN  W.TagName ELSE NULL END) AS TagName3
        ,MAX(CASE WHEN RowNum=4 THEN  W.TagName ELSE NULL END) AS TagName4
        ,MAX(CASE WHEN RowNum=5 THEN  W.TagName ELSE NULL END) AS TagName5
        FROM (SELECT AdminNO
                    ,ChangeDate
                    ,SEQ
                    ,TagName
                    ,ROW_NUMBER() OVER(PARTITION BY AdminNO, ChangeDate ORDER BY SEQ) AS RowNum
                    FROM M_SKUTag
        )AS W  
        GROUP BY W.AdminNO, W.ChangeDate

    ---------------------------
    -- create temp table
    ---------------------------
    IF OBJECT_ID( N'[tempdb]..[#TableForSearchProduct]', N'U' ) IS NOT NULL
    BEGIN
        DROP TABLE [#TableForSearchProduct];
    END

    CREATE TABLE [#TableForSearchProduct]
    (
         AdminNO            int	
        ,SKUCD              varchar(30)	    COLLATE database_default
        ,ChangeDate         varchar(10)	    COLLATE database_default --ChangeDate date	
        ,VariousFLG         tinyint	
        ,SKUName            varchar(100)    COLLATE database_default
        ,SKUShortName       varchar(40)	    COLLATE database_default
        ,ITemCD	            varchar(30)	    COLLATE database_default
        ,ColorNO	        int	
        ,SizeNO	            int	
        ,JanCD	            varchar(13)	    COLLATE database_default
        ,SetKBN             varchar(2)      COLLATE database_default --SetKBN tinyint	
        ,PresentKBN	        tinyint	
        ,SampleKBN	        tinyint	
        ,DiscountKBN	    tinyint	
        ,ColorName	        varchar(20)	    COLLATE database_default
        ,SizeName	        varchar(20)	    COLLATE database_default
        ,MainVendorCD	    varchar(13)	    COLLATE database_default
        ,MainVendorName     varchar(50)     COLLATE database_default --M_Vendor
        ,MakerVendorCD	    varchar(13)	    COLLATE database_default
        ,BrandCD	        varchar(6)	    COLLATE database_default
        ,BrandName          varchar(40)     COLLATE database_default --M_Brand
        ,MakerItem	        varchar(50)	    COLLATE database_default
        ,TaniCD	            varchar(2)	    COLLATE database_default
        ,SportsCD	        varchar(6)	    COLLATE database_default
        ,SportsName         varchar(100)	COLLATE database_default --M_MultiPorpose(202)

        ,TagName1           varchar(10)	    COLLATE database_default
        ,TagName2           varchar(10)	    COLLATE database_default
        ,TagName3           varchar(10)	    COLLATE database_default
        ,TagName4           varchar(10)	    COLLATE database_default
        ,TagName5           varchar(10)	    COLLATE database_default

        ,ZaikoKBN	        tinyint	
        ,Rack	            varchar(10)	    COLLATE database_default
        ,ReserveCD	        varchar(3)	    COLLATE database_default
        ,ReserveName        varchar(100)	COLLATE database_default --M_MultiPorpose(311)
        ,NoticesCD	        varchar(3)	    COLLATE database_default
        ,NoticesName        varchar(100)	COLLATE database_default --M_MultiPorpose(310)
        ,PostageCD	        varchar(3)	    COLLATE database_default
        ,PostageName        varchar(100)	COLLATE database_default --M_MultiPorpose(309)
        ,TaxRateFLG	        tinyint	
        ,PriceWithTax	    money	
        ,PriceOutTax	    money	
        ,OrderPriceWithTax	    money	
        ,OrderPriceWithoutTax	money	
        ,CommentInStore	    varchar(300)	COLLATE database_default
        ,CommentOutStore	varchar(300)	COLLATE database_default
        ,ApprovalDate	    varchar(10)	    COLLATE database_default --ApprovalDate	date
        ,DeleteFlg	        tinyint	
        ,UsedFlg	        tinyint	
        ,InsertOperator	    varchar(10)	    COLLATE database_default --InsertDateTime datetime
        ,InsertDateTime	    varchar(10)	    COLLATE database_default
        ,UpdateOperator	    varchar(10)	    COLLATE database_default --UpdateDateTime datetime
        ,UpdateDateTime	    varchar(10)	    COLLATE database_default

        ,YearTerm           varchar(6)	    COLLATE database_default
        ,Season             varchar(6)	    COLLATE database_default
        ,CatalogNO          varchar(20)	    COLLATE database_default
        ,InstructionsNO     varchar(1000)	COLLATE database_default
        ,Check1             tinyint         --備考用チェック
    )

    ---------------------------
    -- insert into temp table
    ---------------------------      
    IF ISNULL(@JanCD,'') <> ''
    BEGIN
        -- use indexes(JanCD)
        INSERT INTO [#TableForSearchProduct]
        SELECT MS.AdminNO
              ,MS.SKUCD
              ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,MS.VariousFLG
              ,MS.SKUName
              ,MS.SKUShortName
              ,MS.ITemCD
              ,MS.ColorNO
              ,MS.SizeNO
              ,MS.JanCD
              ,(CASE MS.SetKBN WHEN 1 THEN '〇' ELSE '' END) AS SetKBN
              ,MS.PresentKBN
              ,MS.SampleKBN
              ,MS.DiscountKBN
              ,MS.ColorName
              ,MS.SizeName
              ,MS.MainVendorCD
              ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
              ,MS.MakerVendorCD
              ,MS.BrandCD
              ,MB.BrandName AS BrandName
              ,MS.MakerItem
              ,MS.TaniCD
              ,MS.SportsCD
              ,MPs.Char1 AS SportsName

              ,W.TagName1
              ,W.TagName2
              ,W.TagName3
              ,W.TagName4
              ,W.TagName5
              ,MS.ZaikoKBN
              ,MS.Rack
              ,MS.ReserveCD
              ,MPr.Char1 AS ReserveName
              ,MS.NoticesCD
              ,MPn.Char1 AS NoticesName
              ,MS.PostageCD
              ,MPp.Char1 AS PostageName

              ,MS.TaxRateFLG
              ,MS.PriceWithTax
              ,MS.PriceOutTax
              ,MS.OrderPriceWithTax
              ,MS.OrderPriceWithoutTax
              ,MS.CommentInStore
              ,MS.CommentOutStore
              ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
              ,MS.DeleteFlg
              ,MS.UsedFlg
              ,MS.InsertOperator
              ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
              ,MS.UpdateOperator
              ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
        
              ,MI.YearTerm
              ,MI.Season
              ,MI.CatalogNO
              ,MI.InstructionsNO
              ,0 AS Check1  --備考用チェック

        --from F_SKU(@dChangeDate) MS
        FROM M_SKU MS

	    CROSS APPLY
	    (
		    SELECT TOP 1
                 s.ChangeDate
		    FROM M_SKU s
		    WHERE s.AdminNO = MS.AdminNO 
            AND   s.ChangeDate <= @dChangeDate
		    ORDER BY s.ChangeDate DESC
	    ) temp_Store
 
        LEFT OUTER JOIN #tmpSKUTag AS W  
        ON  W.AdminNo = MS.AdminNo
        AND W.ChangeDate = MS.ChangeDate
	
        LEFT OUTER JOIN M_Brand MB
        ON MB.BrandCD = MS.BrandCD
        AND MB.DeleteFlg = 0

        LEFT OUTER JOIN M_MultiPorpose MPs ON MPS.ID='202' and MPs.[Key] = MS.SportsCD
        LEFT OUTER JOIN M_MultiPorpose MPr on MPS.ID='311' and MPr.[Key] = MS.ReserveCD
        LEFT OUTER JOIN M_MultiPorpose MPn on MPS.ID='310' and MPn.[Key] = MS.NoticesCD
        LEFT OUTER JOIN M_MultiPorpose MPp on MPS.ID='309' and MPp.[Key] = MS.PostageCD

        OUTER APPLY 
        (
            SELECT TOP 1
                 s.AdminNO
                ,s.ChangeDate
                ,s.YearTerm
                ,s.Season
                ,s.CatalogNO
                ,s.InstructionsNO
            FROM M_SKUInfo s 
            WHERE s.AdminNO = MS.AdminNO
            AND   s.ChangeDate = MS.ChangeDate
            ORDER BY s.SEQ desc
        ) as MI

        WHERE 
        (
           (ISNULL(@TagName1,'') = '' OR W.TagName1 = @TagName1)
        OR (ISNULL(@TagName2,'') = '' OR W.TagName2 = @TagName2)
        OR (ISNULL(@TagName3,'') = '' OR W.TagName3 = @TagName3)
        OR (ISNULL(@TagName4,'') = '' OR W.TagName4 = @TagName4)
        OR (ISNULL(@TagName5,'') = '' OR W.TagName5 = @TagName5)
        )

        AND MS.DeleteFlg = 0
        AND MS.ChangeDate = temp_Store.ChangeDate
        AND MS.JanCD = @JanCD

        AND (ISNULL(@SKUCD,'') = '' OR  MS.SKUCD = @SKUCD)
        AND (ISNULL(@MainVendorCD,'') = '' OR MS.MainVendorCD = @MainVendorCD)
        AND (ISNULL(@BrandCD,'') = '' OR MS.BrandCD = @BrandCD)
        AND (ISNULL(@SportsCD,'') = '' OR MS.SportsCD = @SportsCD)
        AND (ISNULL(@ReserveCD,'') = '' OR MS.ReserveCD = @ReserveCD)
        AND (ISNULL(@NoticesCD,'') = '' OR MS.NoticesCD = @NoticesCD)
        AND (ISNULL(@PostageCD,'') = '' OR MS.PostageCD = @PostageCD)
        AND (ISNULL(@SKUName,'') = '' OR MS.SKUName LIKE '%'+ @SKUName+'%')

        AND (ISNULL(@InputDateFrom,'') = '' OR MS.InsertDateTime >= CONVERT(datetime, @InputDateFrom))
        AND (ISNULL(@InputDateTo,'') = '' OR MS.InsertDateTime < DATEADD(d, 1, CONVERT(datetime, @InputDateTo)))

        AND (ISNULL(@UpdateDateFrom,'') = '' OR MS.UpdateDateTime >= CONVERT(datetime, @UpdateDateFrom))
        AND (ISNULL(@UpdateDateTo,'') = '' OR MS.UpdateDateTime < DATEADD(d, 1, CONVERT(datetime, @UpdateDateTo)))
 
        AND (ISNULL(@ApprovalDateFrom,'') = '' OR MS.ApprovalDate >= CONVERT(date, @ApprovalDateFrom))
        AND (ISNULL(@ApprovalDateTo,'') = '' OR MS.ApprovalDate <= CONVERT(date, @ApprovalDateTo))
        AND (ISNULL(@NotApproved,0) = 0 OR MS.ApprovalDate IS NULL)

        AND (ISNULL(@YearTerm,'') = '' OR  MI.YearTerm = @YearTerm)
        AND (ISNULL(@Season,'') = '' OR  MI.Season = @Season)
        AND (ISNULL(@CatalogNO,'') = '' OR  MI.CatalogNO = @CatalogNO)
        AND (ISNULL(@InstructionsNO,'') = '' OR  MI.InstructionsNO = @InstructionsNO)

        --Even when selecting "AND", search for data with "OR" first.
        AND (ISNULL(@CommentInStore,'')='' OR EXISTS(SELECT * FROM @CommentInStoreList s WHERE MS.CommentInStore like s.Item))
        AND (ISNULL(@MakerItem,'')='' OR EXISTS(SELECT * FROM @MakerItemList s WHERE MS.MakerItem like s.Item))
        AND (ISNULL(@ITemCD,'')='' OR EXISTS(SELECT * FROM @ItemList s WHERE MS.ITemCD like s.Item))
    END

    ELSE IF ISNULL(@BrandCD,'') <> ''
    BEGIN
        -- use indexes(BrandCD)
        INSERT INTO [#TableForSearchProduct]
        SELECT MS.AdminNO
              ,MS.SKUCD
              ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,MS.VariousFLG
              ,MS.SKUName
              ,MS.SKUShortName
              ,MS.ITemCD
              ,MS.ColorNO
              ,MS.SizeNO
              ,MS.JanCD
              ,(CASE MS.SetKBN WHEN 1 THEN '〇' ELSE '' END) AS SetKBN
              ,MS.PresentKBN
              ,MS.SampleKBN
              ,MS.DiscountKBN
              ,MS.ColorName
              ,MS.SizeName
              ,MS.MainVendorCD
              ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
              ,MS.MakerVendorCD
              ,MS.BrandCD
              ,MB.BrandName AS BrandName
              ,MS.MakerItem
              ,MS.TaniCD
              ,MS.SportsCD
              ,MPs.Char1 AS SportsName

              ,W.TagName1
              ,W.TagName2
              ,W.TagName3
              ,W.TagName4
              ,W.TagName5
              ,MS.ZaikoKBN
              ,MS.Rack
              ,MS.ReserveCD
              ,MPr.Char1 AS ReserveName
              ,MS.NoticesCD
              ,MPn.Char1 AS NoticesName
              ,MS.PostageCD
              ,MPp.Char1 AS PostageName

              ,MS.TaxRateFLG
              ,MS.PriceWithTax
              ,MS.PriceOutTax
              ,MS.OrderPriceWithTax
              ,MS.OrderPriceWithoutTax
              ,MS.CommentInStore
              ,MS.CommentOutStore
              ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
              ,MS.DeleteFlg
              ,MS.UsedFlg
              ,MS.InsertOperator
              ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
              ,MS.UpdateOperator
              ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
        
              ,MI.YearTerm
              ,MI.Season
              ,MI.CatalogNO
              ,MI.InstructionsNO
              ,0 AS Check1  --備考用チェック
          
        --from F_SKU(@dChangeDate) MS
        FROM M_SKU MS

	    CROSS APPLY
	    (
		    SELECT TOP 1 s.ChangeDate
		    FROM M_SKU s
		    WHERE s.AdminNO = MS.AdminNO 
            AND   s.ChangeDate <= @dChangeDate
		    ORDER BY s.ChangeDate DESC
	    ) temp_Store

        LEFT OUTER JOIN #tmpSKUTag AS W  
        ON  W.AdminNo = MS.AdminNo
        AND W.ChangeDate = MS.ChangeDate
	
        LEFT OUTER JOIN M_Brand MB
        ON MB.BrandCD = MS.BrandCD
        AND MB.DeleteFlg = 0

        LEFT OUTER JOIN M_MultiPorpose MPs ON MPS.ID='202' and MPs.[Key] = MS.SportsCD
        LEFT OUTER JOIN M_MultiPorpose MPr on MPS.ID='311' and MPr.[Key] = MS.ReserveCD
        LEFT OUTER JOIN M_MultiPorpose MPn on MPS.ID='310' and MPn.[Key] = MS.NoticesCD
        LEFT OUTER JOIN M_MultiPorpose MPp on MPS.ID='309' and MPp.[Key] = MS.PostageCD

        OUTER APPLY 
        (
            SELECT TOP 1
                 s.AdminNO
                ,s.ChangeDate
                ,s.YearTerm
                ,s.Season
                ,s.CatalogNO
                ,s.InstructionsNO
            FROM M_SKUInfo s 
            WHERE s.AdminNO = MS.AdminNO
            AND   s.ChangeDate = MS.ChangeDate
            ORDER BY s.SEQ desc
        ) as MI
 
        WHERE 
        (
           (ISNULL(@TagName1,'') = '' OR W.TagName1 = @TagName1)
        OR (ISNULL(@TagName2,'') = '' OR W.TagName2 = @TagName2)
        OR (ISNULL(@TagName3,'') = '' OR W.TagName3 = @TagName3)
        OR (ISNULL(@TagName4,'') = '' OR W.TagName4 = @TagName4)
        OR (ISNULL(@TagName5,'') = '' OR W.TagName5 = @TagName5)
        )

        AND MS.DeleteFlg = 0
        AND MS.ChangeDate = temp_Store.ChangeDate
 
	    AND (ISNULL(@JanCD,'') = '' OR MS.JanCD = @JanCD)
        AND (ISNULL(@SKUCD,'') = '' OR  MS.SKUCD = @SKUCD)
        AND (ISNULL(@MainVendorCD,'') = '' OR MS.MainVendorCD = @MainVendorCD)

        AND MS.BrandCD = @BrandCD

        AND (ISNULL(@SportsCD,'') = '' OR MS.SportsCD = @SportsCD)
        AND (ISNULL(@ReserveCD,'') = '' OR MS.ReserveCD = @ReserveCD)
        AND (ISNULL(@NoticesCD,'') = '' OR MS.NoticesCD = @NoticesCD)
        AND (ISNULL(@PostageCD,'') = '' OR MS.PostageCD = @PostageCD)
        AND (ISNULL(@SKUName,'') = '' OR MS.SKUName LIKE '%'+ @SKUName+'%')

        AND (ISNULL(@InputDateFrom,'') = '' OR MS.InsertDateTime >= CONVERT(datetime, @InputDateFrom))
        AND (ISNULL(@InputDateTo,'') = '' OR MS.InsertDateTime < DATEADD(d, 1, CONVERT(datetime, @InputDateTo)))

        AND (ISNULL(@UpdateDateFrom,'') = '' OR MS.UpdateDateTime >= CONVERT(datetime, @UpdateDateFrom))
        AND (ISNULL(@UpdateDateTo,'') = '' OR MS.UpdateDateTime < DATEADD(d, 1, CONVERT(datetime, @UpdateDateTo)))
 
        AND (ISNULL(@ApprovalDateFrom,'') = '' OR MS.ApprovalDate >= CONVERT(date, @ApprovalDateFrom))
        AND (ISNULL(@ApprovalDateTo,'') = '' OR MS.ApprovalDate <= CONVERT(date, @ApprovalDateTo))
        AND (ISNULL(@NotApproved,0) = 0 OR MS.ApprovalDate IS NULL)

        AND (ISNULL(@YearTerm,'') = '' OR  MI.YearTerm = @YearTerm)
        AND (ISNULL(@Season,'') = '' OR  MI.Season = @Season)
        AND (ISNULL(@CatalogNO,'') = '' OR  MI.CatalogNO = @CatalogNO)
        AND (ISNULL(@InstructionsNO,'') = '' OR  MI.InstructionsNO = @InstructionsNO)

        --Even when selecting "AND", search for data with "OR" first.
        AND (ISNULL(@CommentInStore,'')='' OR EXISTS(SELECT * FROM @CommentInStoreList s WHERE MS.CommentInStore like s.Item))
        AND (ISNULL(@MakerItem,'')='' OR EXISTS(SELECT * FROM @MakerItemList s WHERE MS.MakerItem like s.Item))
        AND (ISNULL(@ITemCD,'')='' OR EXISTS(SELECT * FROM @ItemList s WHERE MS.ITemCD like s.Item))
    END
    ELSE
    BEGIN
        INSERT INTO [#TableForSearchProduct] 
        SELECT MS.AdminNO
              ,MS.SKUCD
              ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,MS.VariousFLG
              ,MS.SKUName
              ,MS.SKUShortName
              ,MS.ITemCD
              ,MS.ColorNO
              ,MS.SizeNO
              ,MS.JanCD
              ,(CASE MS.SetKBN WHEN 1 THEN '〇' ELSE '' END) AS SetKBN
              ,MS.PresentKBN
              ,MS.SampleKBN
              ,MS.DiscountKBN
              ,MS.ColorName
              ,MS.SizeName
              ,MS.MainVendorCD
              ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
              ,MS.MakerVendorCD
              ,MS.BrandCD
              ,MB.BrandName AS BrandName
              ,MS.MakerItem
              ,MS.TaniCD
              ,MS.SportsCD
              ,MPs.Char1 AS SportsName

              ,W.TagName1
              ,W.TagName2
              ,W.TagName3
              ,W.TagName4
              ,W.TagName5
              ,MS.ZaikoKBN
              ,MS.Rack
              ,MS.ReserveCD
              ,MPr.Char1 AS ReserveName
              ,MS.NoticesCD
              ,MPn.Char1 AS NoticesName
              ,MS.PostageCD
              ,MPp.Char1 AS PostageName

              ,MS.TaxRateFLG
              ,MS.PriceWithTax
              ,MS.PriceOutTax
              ,MS.OrderPriceWithTax
              ,MS.OrderPriceWithoutTax
              ,MS.CommentInStore
              ,MS.CommentOutStore
              ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
              ,MS.DeleteFlg
              ,MS.UsedFlg
              ,MS.InsertOperator
              ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
              ,MS.UpdateOperator
              ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
        
              ,MI.YearTerm
              ,MI.Season
              ,MI.CatalogNO
              ,MI.InstructionsNO
              ,0 AS Check1  --備考用チェック
          
        --from F_SKU(@dChangeDate) MS
        FROM M_SKU MS

	    CROSS APPLY
	    (
		    SELECT TOP 1 s.ChangeDate
		    FROM M_SKU s
		    WHERE s.AdminNO = MS.AdminNO 
            AND   s.ChangeDate <= @dChangeDate
		    ORDER BY s.ChangeDate DESC
	    ) temp_Store

        LEFT OUTER JOIN #tmpSKUTag AS W  
        ON  W.AdminNo = MS.AdminNo
        AND W.ChangeDate = MS.ChangeDate
	
        LEFT OUTER JOIN M_Brand MB
        ON MB.BrandCD = MS.BrandCD
        AND MB.DeleteFlg = 0

        LEFT OUTER JOIN M_MultiPorpose MPs ON MPS.ID='202' and MPs.[Key] = MS.SportsCD
        LEFT OUTER JOIN M_MultiPorpose MPr on MPS.ID='311' and MPr.[Key] = MS.ReserveCD
        LEFT OUTER JOIN M_MultiPorpose MPn on MPS.ID='310' and MPn.[Key] = MS.NoticesCD
        LEFT OUTER JOIN M_MultiPorpose MPp on MPS.ID='309' and MPp.[Key] = MS.PostageCD

        OUTER APPLY 
        (
            SELECT TOP 1
                 s.AdminNO
                ,s.ChangeDate
                ,s.YearTerm
                ,s.Season
                ,s.CatalogNO
                ,s.InstructionsNO
            FROM M_SKUInfo s 
            WHERE s.AdminNO = MS.AdminNO
            AND   s.ChangeDate = MS.ChangeDate
            ORDER BY s.SEQ desc
        ) as MI
 
        WHERE 
        (
           (ISNULL(@TagName1,'') = '' OR W.TagName1 = @TagName1)
        OR (ISNULL(@TagName2,'') = '' OR W.TagName2 = @TagName2)
        OR (ISNULL(@TagName3,'') = '' OR W.TagName3 = @TagName3)
        OR (ISNULL(@TagName4,'') = '' OR W.TagName4 = @TagName4)
        OR (ISNULL(@TagName5,'') = '' OR W.TagName5 = @TagName5)
        )

        AND MS.DeleteFlg = 0
        AND MS.ChangeDate = temp_Store.ChangeDate
 
	    AND (ISNULL(@JanCD,'') = '' OR MS.JanCD = @JanCD)
        AND (ISNULL(@SKUCD,'') = '' OR  MS.SKUCD = @SKUCD)
        AND (ISNULL(@MainVendorCD,'') = '' OR MS.MainVendorCD = @MainVendorCD)
        AND (ISNULL(@BrandCD,'') = '' OR MS.BrandCD = @BrandCD)
        AND (ISNULL(@SportsCD,'') = '' OR MS.SportsCD = @SportsCD)
        AND (ISNULL(@ReserveCD,'') = '' OR MS.ReserveCD = @ReserveCD)
        AND (ISNULL(@NoticesCD,'') = '' OR MS.NoticesCD = @NoticesCD)
        AND (ISNULL(@PostageCD,'') = '' OR MS.PostageCD = @PostageCD)
        AND (ISNULL(@SKUName,'') = '' OR MS.SKUName LIKE '%'+ @SKUName+'%')

        AND (ISNULL(@InputDateFrom,'') = '' OR MS.InsertDateTime >= CONVERT(datetime, @InputDateFrom))
        AND (ISNULL(@InputDateTo,'') = '' OR MS.InsertDateTime < DATEADD(d, 1, CONVERT(datetime, @InputDateTo)))

        AND (ISNULL(@UpdateDateFrom,'') = '' OR MS.UpdateDateTime >= CONVERT(datetime, @UpdateDateFrom))
        AND (ISNULL(@UpdateDateTo,'') = '' OR MS.UpdateDateTime < DATEADD(d, 1, CONVERT(datetime, @UpdateDateTo)))
 
        AND (ISNULL(@ApprovalDateFrom,'') = '' OR MS.ApprovalDate >= CONVERT(date, @ApprovalDateFrom))
        AND (ISNULL(@ApprovalDateTo,'') = '' OR MS.ApprovalDate <= CONVERT(date, @ApprovalDateTo))
        AND (ISNULL(@NotApproved,0) = 0 OR MS.ApprovalDate IS NULL)

        AND (ISNULL(@YearTerm,'') = '' OR  MI.YearTerm = @YearTerm)
        AND (ISNULL(@Season,'') = '' OR  MI.Season = @Season)
        AND (ISNULL(@CatalogNO,'') = '' OR  MI.CatalogNO = @CatalogNO)
        AND (ISNULL(@InstructionsNO,'') = '' OR  MI.InstructionsNO = @InstructionsNO)

        --Even when selecting "AND", search for data with "OR" first.
        AND (ISNULL(@CommentInStore,'')='' OR EXISTS(SELECT * FROM @CommentInStoreList s WHERE MS.CommentInStore like s.Item))
        AND (ISNULL(@MakerItem,'')='' OR EXISTS(SELECT * FROM @MakerItemList s WHERE MS.MakerItem like s.Item))
        AND (ISNULL(@ITemCD,'')='' OR EXISTS(SELECT * FROM @ItemList s WHERE MS.ITemCD like s.Item))
    END
    
 
IF @OrOrAnd = 1	--Only when "AND" is selected.
BEGIN

    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    DECLARE @COMMENT varchar(80);
    DECLARE @FULL_COMMENT varchar(300);
    
    --備考データより条件に合わないデータをテーブルから削除
    IF ISNULL(@CommentInStore,'') <> ''
    BEGIN
	    create NONCLUSTERED INDEX tmpTable_INDEX_CommentInStore ON #TableForSearchProduct
	     (
	    	CommentInStore asc
	     )
	     
        UPDATE #TableForSearchProduct
        SET Check1 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @CommentInStore, @INDEX) = 0
            BEGIN
                IF LEN(@CommentInStore)-@INDEX >= 0
                BEGIN
                    --データが一つのみの場合
                    UPDATE #TableForSearchProduct
                    SET Check1 = 0
                    WHERE CommentInStore LIKE '%' + SUBSTRING(@CommentInStore,@INDEX,LEN(@CommentInStore)-@INDEX+1) + '%'
                    ;
                    
                    IF @OrOrAnd = 1		--Andの場合
                    BEGIN
                        DELETE FROM #TableForSearchProduct
                        WHERE  Check1 = 1
                        ;
                    END
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @CommentInStore, @INDEX);
            	
                UPDATE #TableForSearchProduct
                SET Check1 = 0
                WHERE CommentInStore LIKE '%' + SUBSTRING(@CommentInStore,@INDEX,@NEXT_INDEX-@INDEX) + '%'
                ;

                IF @OrOrAnd = 1		--Andの場合
                BEGIN
                    DELETE FROM #TableForSearchProduct
                    WHERE  Check1 = 1
                    ;
                    
                    UPDATE #TableForSearchProduct
                    SET Check1 = 1
			        ;
                END
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchProduct
        WHERE  Check1 = 1
        ;
    END;

END --Only when "AND" is selected.



	--関連検索の場合
    IF @SearchFlg = 1
    BEGIN

        CREATE NONCLUSTERED INDEX tmpTable_INDEX_ITEM ON #TableForSearchProduct
        (
            ItemCD asc
        )

        IF @ItemOrMaker = 0
        BEGIN
            -- Insert statements for procedure here
            SELECT TOP 1001
                   MS.AdminNO
                  ,MS.SKUCD
                  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
                  ,MS.VariousFLG
                  ,MS.SKUName
                  ,MS.SKUShortName
                  ,MS.ITemCD
                  ,MS.ColorNO
                  ,MS.SizeNO
                  ,MS.JanCD
                  ,(CASE MS.SetKBN WHEN 1 THEN '〇' ELSE '' END) AS SetKBN
                  ,MS.PresentKBN
                  ,MS.SampleKBN
                  ,MS.DiscountKBN
                  ,MS.ColorName
                  ,MS.SizeName
                  ,MS.MainVendorCD
                  ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
                  ,MS.MakerVendorCD
                  ,MS.BrandCD
                  ,MB.BrandName AS BrandName
                  ,MS.MakerItem
                  ,MS.TaniCD
                  ,MS.SportsCD
                  ,MPs.Char1 AS SportsName
                  ,W.TagName1
                  ,W.TagName2
                  ,W.TagName3
                  ,W.TagName4
                  ,W.TagName5
          
                  ,MS.ZaikoKBN
                  ,MS.Rack
                  ,MS.ReserveCD
                  ,MPr.Char1 AS ReserveName
                  ,MS.NoticesCD
                  ,MPn.Char1 AS NoticesName
                  ,MS.PostageCD
                  ,MPp.Char1 AS PostageName

                  ,MS.TaxRateFLG
                  ,MS.PriceWithTax
                  ,MS.PriceOutTax
                  ,MS.OrderPriceWithTax
                  ,MS.OrderPriceWithoutTax
                  ,MS.CommentInStore
                  ,MS.CommentOutStore
                  ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
                  ,MS.DeleteFlg
                  ,MS.UsedFlg
                  ,MS.InsertOperator
                  ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
                  ,MS.UpdateOperator
                  ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
                
                  ,MI.YearTerm
                  ,MI.Season
                  ,MI.CatalogNO
                  ,MI.InstructionsNO
            
            FROM F_SKU(@dChangeDate) MS

            LEFT OUTER JOIN #tmpSKUTag AS W  
            ON  W.AdminNo = MS.AdminNo
            AND W.ChangeDate = MS.ChangeDate
	
            LEFT OUTER JOIN M_Brand MB
            ON MB.BrandCD = MS.BrandCD
            AND MB.DeleteFlg = 0

            LEFT OUTER JOIN M_MultiPorpose MPs ON MPS.ID='202' and MPs.[Key] = MS.SportsCD
            LEFT OUTER JOIN M_MultiPorpose MPr on MPS.ID='311' and MPr.[Key] = MS.ReserveCD
            LEFT OUTER JOIN M_MultiPorpose MPn on MPS.ID='310' and MPn.[Key] = MS.NoticesCD
            LEFT OUTER JOIN M_MultiPorpose MPp on MPS.ID='309' and MPp.[Key] = MS.PostageCD

            OUTER APPLY 
            (
                SELECT TOP 1
                     s.AdminNO
                    ,s.ChangeDate
                    ,s.YearTerm
                    ,s.Season
                    ,s.CatalogNO
                    ,s.InstructionsNO
                FROM M_SKUInfo s 
                WHERE s.AdminNO = MS.AdminNO
                AND   s.ChangeDate = MS.ChangeDate
                ORDER BY s.SEQ desc
            ) as MI
            
            WHERE EXISTS (SELECT MT.ItemCD
                from #TableForSearchProduct AS MT
                WHERE MS.ItemCD = MT.ItemCD
            )
            AND MS.DeleteFlg = 0
            ORDER BY MS.SKUCD
            ;
        END
        ELSE
        BEGIN
            -- Insert statements for procedure here
            SELECT TOP 1001
                   MS.AdminNO
                  ,MS.SKUCD
                  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
                  ,MS.VariousFLG
                  ,MS.SKUName
                  ,MS.SKUShortName
                  ,MS.ITemCD
                  ,MS.ColorNO
                  ,MS.SizeNO
                  ,MS.JanCD
                  ,(CASE MS.SetKBN WHEN 1 THEN '〇' ELSE '' END) AS SetKBN
                  ,MS.PresentKBN
                  ,MS.SampleKBN
                  ,MS.DiscountKBN
                  ,MS.ColorName
                  ,MS.SizeName
                  ,MS.MainVendorCD
                  ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
                  ,MS.MakerVendorCD
                  ,MS.BrandCD
                  ,MB.BrandName AS BrandName
                  ,MS.MakerItem
                  ,MS.TaniCD
                  ,MS.SportsCD
                  ,MPs.Char1 AS SportsName
                  ,W.TagName1
                  ,W.TagName2
                  ,W.TagName3
                  ,W.TagName4
                  ,W.TagName5
                  
                  ,MS.ZaikoKBN
                  ,MS.Rack
                  ,MS.ReserveCD
                  ,MPr.Char1 AS ReserveName
                  ,MS.NoticesCD
                  ,MPn.Char1 AS NoticesName
                  ,MS.PostageCD
                  ,MPp.Char1 AS PostageName

                  ,MS.TaxRateFLG
                  ,MS.PriceWithTax
                  ,MS.PriceOutTax
                  ,MS.OrderPriceWithTax
                  ,MS.OrderPriceWithoutTax
                  ,MS.CommentInStore
                  ,MS.CommentOutStore
                  ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
                  ,MS.DeleteFlg
                  ,MS.UsedFlg
                  ,MS.InsertOperator
                  ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
                  ,MS.UpdateOperator
                  ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
                
                  ,MI.YearTerm
                  ,MI.Season
                  ,MI.CatalogNO
                  ,MI.InstructionsNO
            
            FROM F_SKU(@dChangeDate) MS

            LEFT OUTER JOIN #tmpSKUTag AS W  
            ON  W.AdminNo = MS.AdminNo
            AND W.ChangeDate = MS.ChangeDate
	
            LEFT OUTER JOIN M_Brand MB
            ON MB.BrandCD = MS.BrandCD
            AND MB.DeleteFlg = 0

            LEFT OUTER JOIN M_MultiPorpose MPs ON MPS.ID='202' and MPs.[Key] = MS.SportsCD
            LEFT OUTER JOIN M_MultiPorpose MPr on MPS.ID='311' and MPr.[Key] = MS.ReserveCD
            LEFT OUTER JOIN M_MultiPorpose MPn on MPS.ID='310' and MPn.[Key] = MS.NoticesCD
            LEFT OUTER JOIN M_MultiPorpose MPp on MPS.ID='309' and MPp.[Key] = MS.PostageCD

            OUTER APPLY 
            (
                SELECT TOP 1
                     s.AdminNO
                    ,s.ChangeDate
                    ,s.YearTerm
                    ,s.Season
                    ,s.CatalogNO
                    ,s.InstructionsNO
                FROM M_SKUInfo s 
                WHERE s.AdminNO = MS.AdminNO
                AND   s.ChangeDate = MS.ChangeDate
                ORDER BY s.SEQ desc
            ) as MI
            
            WHERE EXISTS(SELECT MT.MakerItem
                from #TableForSearchProduct AS MT
                WHERE MS.MakerItem = MT.MakerItem
            )
            AND MS.DeleteFlg = 0
            ORDER BY MS.SKUCD
            ;
        END

    END
    ELSE
    BEGIN
        CREATE NONCLUSTERED INDEX tmpTable_INDEX_SKUCD ON #TableForSearchProduct
        (
            SKUCD asc
        )

        SELECT top 1001 MS.*
        FROM #TableForSearchProduct AS MS
        ORDER BY MS.SKUCD
    END
END
