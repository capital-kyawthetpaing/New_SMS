 BEGIN TRY 
 Drop Procedure dbo.[D_Juchuu_SelectData_ForTempoShukkaNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 出荷売上入力
--       Program ID      TempoShukkaNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Juchuu_SelectData_ForTempoShukkaNyuuryoku]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO varchar(11),
    @SalesNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @OperateMode = 1
    BEGIN
        SELECT DH.JuchuuNO
              ,DH.StoreCD
              ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SKUName ELSE DM.SKUName END) AS SKUName
                FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN ISNULL(M.ColorName,'') + ' ' + ISNULL(M.SizeName,'') 
                                ELSE ISNULL(DM.ColorName,'')  + ' ' + ISNULL(DM.SizeName,'') END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ColorSizeName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.ColorName ELSE DM.ColorName END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SizeName ELSE DM.SizeName END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS SizeName
            ,(SELECT top 1 M.TaxRateFLG
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS TaxRateFLG
            ,(SELECT top 1 M.ZaikoKBN
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ZaikoKBN
              ,DM.JuchuuRows
              ,DM.DisplayRows
              ,DM.AdminNO AS SKUNO
              ,DM.SKUCD
              ,DM.JanCD
              ,DM.JuchuuSuu
              ,DM.JuchuuUnitPrice
              ,DM.JuchuuGaku
              ,DM.JuchuuHontaiGaku
              ,DM.JuchuuTax
              ,DM.JuchuuTaxRitsu
              ,DM.CostUnitPrice
              ,DM.CostGaku
              ,DM.ProfitGaku
              ,DM.HikiateSu
              ,DM.DeliverySu
              ,DM.DirectFLG
              ,DM.NotPrintFLG
              ,DM.JuchuuRows
              ,DM.AddJuchuuRows

              ,0 AS ShippingSu
              ,0 AS SalesUnitPrice
              ,0 AS SalesGaku
              ,0 AS SalesTax
              ,0 AS SalesSU
              
              ,ISNULL((SELECT SUM(R.ShippingPossibleSU) - SUM(R.ShippingSu) 
                FROM D_Reserve AS R 
                WHERE R.Number = DM.JuchuuNO	--@JuchuuNO 
                AND R.NumberRows = DM.JuchuuRows --1
                AND R.DeleteDateTime IS Null
                GROUP BY R.Number),0) AS SyukkaKanouSu

              ,(CASE DH.PresentFLG WHEN 1 THEN 'プレゼント品あり ' ELSE '' END)
              + ISNULL((SELECT top 1 M.CommentOutStore
                FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc),'') AS SyohinChuijiko
                 
          FROM D_Juchuu DH

          LEFT OUTER JOIN D_JuchuuDetails AS DM ON DH.JuchuuNO = DM.JuchuuNO AND DM.DeleteDateTime IS NULL
            
          WHERE DH.JuchuuNO = @JuchuuNO               
          AND DH.DeleteDateTime IS Null
            ORDER BY DH.JuchuuNO, DM.DisplayRows
            ;
	END
	ELSE IF @OperateMode = 3
	BEGIN
        SELECT DH.JuchuuNO
              ,DH.StoreCD
              ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SKUName ELSE DM.SKUName END) AS SKUName
                FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0  
                 ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN ISNULL(M.ColorName,'') + ' ' + ISNULL(M.SizeName,'') 
                                ELSE ISNULL(DM.ColorName,'')  + ' ' + ISNULL(DM.SizeName,'') END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ColorSizeName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.ColorName ELSE DM.ColorName END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN M.SizeName ELSE DM.SizeName END) AS ColorSizeName
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS SizeName
            ,(SELECT top 1 M.TaxRateFLG
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0  
                 ORDER BY M.ChangeDate desc) AS TaxRateFLG
            ,(SELECT top 1 M.ZaikoKBN
               FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc) AS ZaikoKBN
              ,DM.JuchuuRows
              ,DM.DisplayRows
              ,DM.AdminNO AS SKUNO
              ,DM.SKUCD
              ,DM.JanCD
              ,DM.JuchuuSuu
              ,DM.JuchuuUnitPrice
              ,DM.JuchuuGaku
              ,DM.JuchuuHontaiGaku
              ,DM.JuchuuTax
              ,DM.JuchuuTaxRitsu
              ,DM.CostUnitPrice
              ,DM.CostGaku
              ,DM.ProfitGaku
              ,DM.HikiateSu
              ,DM.DeliverySu
              ,DM.DirectFLG
              ,DM.NotPrintFLG
              ,DM.JuchuuRows
              ,DM.AddJuchuuRows
              
              ,0 AS ShippingSu
              ,0 AS SyukkaKanouSu

              ,(CASE DH.PresentFLG WHEN 1 THEN 'プレゼント品あり ' ELSE '' END)
              + ISNULL((SELECT top 1 M.CommentOutStore
                FROM M_SKU AS M
                WHERE M.AdminNO = DM.AdminNO
                 AND M.ChangeDate <= DH.JuchuuDate
                 AND M.DeleteFlg = 0 
                 ORDER BY M.ChangeDate desc),'') AS SyohinChuijiko
                 
                 
              ,(SELECT TOP 1 A.StaffName
                        FROM M_Staff AS A
                        WHERE A.StaffCD = DSH.StaffCD 
                        AND A.ChangeDate <= DSH.SalesDate 
                        ORDER BY A.ChangeDate DESC) AS StaffName

              ,CONVERT(varchar,DSH.SalesDate,111) AS SalesDate   
              ,DSH.BillingType
              ,DS.SalesUnitPrice
              ,DS.SalesGaku
              ,DS.SalesTax
              ,DS.SalesSU

          FROM D_Juchuu DH

          LEFT OUTER JOIN D_JuchuuDetails AS DM ON DH.JuchuuNO = DM.JuchuuNO AND DM.DeleteDateTime IS NULL
          LEFT OUTER JOIN D_SalesDetails AS DS 
            ON DS.SalesNo = @SalesNo 
            AND DS.JuchuuNO = DM.JuchuuNO 
            AND DS.JuchuuRows = DM.JuchuuRows
            AND DS.DeleteDateTime IS NULL
          LEFT OUTER JOIN D_Sales AS DSH
            ON DSH.SalesNo = DS.SalesNo
            AND DSH.DeleteDateTime IS NULL

          WHERE DH.JuchuuNO = @JuchuuNO               
          AND DH.DeleteDateTime IS Null
            ORDER BY DH.JuchuuNO, DM.DisplayRows
            ;
	END

END


