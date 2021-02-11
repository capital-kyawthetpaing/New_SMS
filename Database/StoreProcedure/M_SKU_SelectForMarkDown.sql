IF OBJECT_ID ( 'M_SKU_SelectForMarkDown', 'P' ) IS NOT NULL
    Drop Procedure dbo.[M_SKU_SelectForMarkDown]
GO


--  ======================================================================
--       Program Call    マークダウン入力
--       Program ID      MarkDownNyuuryoku
--       Create date:    2020.06.20
--    ======================================================================

--********************************************--
--                                            --
--                M_SKU抽出                   --
--                                            --
--********************************************--
CREATE PROCEDURE M_SKU_SelectForMarkDown(
    @JanCD varchar(13),
    @AdminNO int,
    @ChangeDate varchar(10)
)AS
BEGIN

    SET NOCOUNT ON;

    WITH SKU AS (
                SELECT MS.JanCD , MS.ChangeDate, MS.AdminNO
                  FROM ( SELECT JanCD , ChangeDate, AdminNO
                              , RANK() OVER(PARTITION BY JanCD ORDER BY ChangeDate DESC, AdminNO DESC) RANK
                           FROM M_SKU
                          WHERE DeleteFlg = 0
                            AND SetKBN = 0
                            AND JanCD  = @JanCD
                            AND AdminNO = (CASE WHEN @AdminNO <> 0 THEN @AdminNO ELSE AdminNO END)
                            AND ChangeDate <= @ChangeDate
                       ) MS
                 WHERE MS.RANK = 1      
    )
    SELECT MS.AdminNO          
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.ITemCD
          ,MS.SKUName
          ,MS.ColorName
          ,MS.SizeName
          ,MS.MakerItem
          ,MS.SKUCD
          ,MS.PriceOutTax
          ,MS.NormalCost
          ,(CASE MS.TaxRateFLG WHEN 1 THEN (SELECT TOP 1 A.TaxRate1 FROM M_SalesTax A WHERE A.ChangeDate <= @ChangeDate ORDER BY A.ChangeDate DESC)
                               WHEN 2 THEN (SELECT TOP 1 A.TaxRate2 FROM M_SalesTax A WHERE A.ChangeDate <= @ChangeDate ORDER BY A.ChangeDate DESC)
                               ELSE 0 END ) AS TaxRate
          ,MS.TaniCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.[ID]= 201 AND A.[KEY]= MS.TaniCD) AS TaniName          
    FROM  SKU
    INNER JOIN M_SKU MS ON SKU.AdminNO = MS.AdminNO
                       AND SKU.ChangeDate = MS.ChangeDate
    ;
    
END

