 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectByItemCD]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_SKU_SelectByItemCD]    */
CREATE PROCEDURE M_SKU_SelectByItemCD(
    -- Add the parameters for the stored procedure here
    @ITemCD varchar(30),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.AdminNO
          ,MS.SKUCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.VariousFLG
          ,MS.SKUName
          ,MS.KanaName
          ,MS.SKUShortName
          ,ISNULL(MS.EnglishName,'') AS EnglishName
          ,MS.ITemCD
          ,MS.ColorNO
          ,MS.SizeNO
          ,ISNULL(MS.JanCD,'') AS JanCD
          ,MS.SetKBN
          ,MS.PresentKBN
          ,MS.SampleKBN
          ,MS.DiscountKBN
          ,MS.ColorName
          ,MS.SizeName
          ,MS.WebFlg
          ,MS.RealStoreFlg
          ,ISNULL(MS.MainVendorCD,'') AS MainVendorCD
          ,ISNULL(MS.MakerVendorCD,'') AS MakerVendorCD
          ,MS.BrandCD
          ,MS.MakerItem
          ,MS.TaniCD
          ,MS.SportsCD
          ,MS.SegmentCD
          ,MS.ZaikoKBN
          ,ISNULL(MS.Rack,'') AS Rack
          ,MS.VirtualFlg
          ,MS.DirectFlg
          ,MS.ReserveCD
          ,MS.NoticesCD
          ,MS.PostageCD
          ,MS.ManufactCD
          ,MS.ConfirmCD
          ,MS.WebStockFlg
          ,MS.StopFlg
          ,MS.DiscontinueFlg
          ,MS.InventoryAddFlg
          ,MS.MakerAddFlg
          ,MS.StoreAddFlg
          ,MS.NoNetOrderFlg
          ,MS.EDIOrderFlg
          ,MS.CatalogFlg
          ,MS.ParcelFlg
          ,MS.AutoOrderFlg
          ,MS.TaxRateFLG
          ,MS.CostingKBN
          ,MS.SaleExcludedFlg
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
          ,MS.Rate
          ,ISNULL(CONVERT(varchar,MS.SaleStartDate,111),'') AS SaleStartDate
          ,ISNULL(CONVERT(varchar,MS.WebStartDate,111),'') AS WebStartDate
          ,ISNULL(MS.OrderAttentionCD,'') AS OrderAttentionCD
          ,ISNULL(MS.OrderAttentionNote,'') AS OrderAttentionNote
          ,ISNULL(MS.CommentInStore,'') AS CommentInStore
          ,ISNULL(MS.CommentOutStore,'') AS CommentOutStore
          ,ISNULL(MS.LastYearTerm,'') AS LastYearTerm
          ,ISNULL(MS.LastSeason,'') AS LastSeason
          ,ISNULL(MS.LastCatalogNO,'') AS LastCatalogNO
          ,ISNULL(MS.LastCatalogPage,'') AS LastCatalogPage
          ,ISNULL(MS.LastCatalogText,'') AS LastCatalogText
          ,ISNULL(MS.LastInstructionsNO,'') AS LastInstructionsNO
          ,ISNULL(CONVERT(varchar,MS.LastInstructionsDate,111),'') AS LastInstructionsDate
          ,ISNULL(MS.WebAddress,'') AS WebAddress
          ,MS.SetAdminCD
          ,ISNULL(MS.SetItemCD,'') AS SetItemCD
          ,ISNULL(MS.SetSKUCD,'') AS SetSKUCD
          ,MS.SetSU
          ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate 
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
          
          ,(SELECT A.BrandName FROM M_Brand AS A WHERE A.BrandCD = MS.BrandCD) AS BrandName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 201 AND A.[Key] = MS.TaniCD) AS TaniName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 202 AND A.[Key] = MS.SportsCD) AS SportsName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 311 AND A.[Key] = MS.ReserveCD) AS ReserveName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 310 AND A.[Key] = MS.NoticesCD) AS NoticesName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 309 AND A.[Key] = MS.PostageCD) AS PostageName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 312 AND A.[Key] = MS.ManufactCD) AS ManufactName
          ,(SELECT A.Char1 FROM M_MultiPorpose AS A WHERE A.ID = 313 AND A.[Key] = MS.ConfirmCD) AS ConfirmName
          ,(CASE MS.TaxRateFLG WHEN 0 THEN '非課税' 
                WHEN 1 THEN '通常課税'
                WHEN 2 THEN '軽減課税'
                END) AS TaxRateFLGName  --0:非課税、1:通常課税、2:軽減課税
          ,(CASE MS.CostingKBN WHEN 1 THEN '標準原価'
                WHEN 2 THEN '総平均原価'
                END) AS CostingKBNName  --1:標準原価、2:総平均原価
          
          ,ISNULL(MT.TagName1,'') AS TagName1
          ,ISNULL(MT.TagName2,'') AS TagName2
          ,ISNULL(MT.TagName3,'') AS TagName3
          ,ISNULL(MT.TagName4,'') AS TagName4
          ,ISNULL(MT.TagName5,'') AS TagName5
          ,ISNULL(MT.TagName6,'') AS TagName6
          ,ISNULL(MT.TagName7,'') AS TagName7
          ,ISNULL(MT.TagName8,'') AS TagName8
          ,ISNULL(MT.TagName9,'') AS TagName9
          ,ISNULL(MT.TagName10,'') AS TagName10
          
    from M_SKU MS
    LEFT OUTER JOIN (
        SELECT T.AdminNO
        	,MAX(CASE T.SEQ WHEN 1 THEN T.TagName ELSE '' END) AS TagName1
            ,MAX(CASE T.SEQ WHEN 2 THEN T.TagName ELSE '' END) AS TagName2
            ,MAX(CASE T.SEQ WHEN 3 THEN T.TagName ELSE '' END) AS TagName3
            ,MAX(CASE T.SEQ WHEN 4 THEN T.TagName ELSE '' END) AS TagName4
            ,MAX(CASE T.SEQ WHEN 5 THEN T.TagName ELSE '' END) AS TagName5
            ,MAX(CASE T.SEQ WHEN 6 THEN T.TagName ELSE '' END) AS TagName6
            ,MAX(CASE T.SEQ WHEN 7 THEN T.TagName ELSE '' END) AS TagName7
            ,MAX(CASE T.SEQ WHEN 8 THEN T.TagName ELSE '' END) AS TagName8
            ,MAX(CASE T.SEQ WHEN 9 THEN T.TagName ELSE '' END) AS TagName9
            ,MAX(CASE T.SEQ WHEN 10 THEN T.TagName ELSE '' END) AS TagName10
        FROM M_SKUTag AS T
        WHERE T.ChangeDate = CONVERT(DATE, @ChangeDate)
        GROUP BY T.AdminNO
        ) AS MT
    ON MT.AdminNO = MS.AdminNO

    WHERE MS.ITemCD = (CASE WHEN @ITemCD <> '' THEN @ITemCD ELSE MS.ITemCD END)
    AND MS.ChangeDate = CONVERT(DATE, @ChangeDate)
    ORDER BY MS.SizeNO, MS.ColorNO
    ;
END

