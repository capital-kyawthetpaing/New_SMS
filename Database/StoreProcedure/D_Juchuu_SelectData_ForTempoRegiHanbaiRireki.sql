

/****** Object:  StoredProcedure [dbo].[D_Juchuu_SelectData_ForTempoRegiHanbaiRireki]    Script Date: 2020/12/24 15:47:36 ******/
DROP PROCEDURE [dbo].[D_Juchuu_SelectData_ForTempoRegiHanbaiRireki]
GO

/****** Object:  StoredProcedure [dbo].[D_Juchuu_SelectData_ForTempoRegiHanbaiRireki]    Script Date: 2020/12/24 15:47:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 販売履歴照会
--       Program ID      TempoRegiHanbaiRireki
--       Create date:    2019.9.19
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Juchuu_SelectData_ForTempoRegiHanbaiRireki]
    (    @CustomerCD   varchar(13)
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
    SELECT DH.JuchuuNO
          ,DH.StoreCD
          ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
          ,(SELECT top 1 A.StoreName 
          FROM M_Store A 
          WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.JuchuuDate
          AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS StoreName	--受注毎に最初のレコードにだけ表示
          ,ROW_NUMBER() OVER(PARTITION BY DH.JuchuuNO ORDER BY DM.JuchuuRows) ROWNUM
		  ,ROW_NUMBER() OVER(PARTITION BY DS.SalesNO ORDER BY DS.JuchuuRows) ROWNUMS
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.JuchuuDate
          AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS StaffName

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
          
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.JuchuuDate 
          	AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS SKUName
         ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN ISNULL(M.ColorName,'') + ' ' + ISNULL(M.SizeName,'') 
                            ELSE ISNULL(DM.ColorName,'')  + ' ' + ISNULL(DM.SizeName,'') END) AS ColorSizeName
           FROM M_SKU AS M
            WHERE M.AdminNO = DM.AdminNO
             AND M.ChangeDate <= DH.JuchuuDate
          	AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorSizeName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.ColorName ELSE A.ColorName END) AS ColorName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.JuchuuDate 
          	AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS ColorName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SizeName ELSE A.SizeName END) AS SizeName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.JuchuuDate 
          	AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS SizeName
          
          ,(Select SUM(DR.ShippingPossibleSU) - SUM(DR.ShippingSu)
            from D_Reserve AS DR
            Where DR.[Number] = DM.JuchuuNO
            AND DR.NumberRows = DM.JuchuuRows
            AND DR.DeleteDateTime IS NULL
            Group by DR.[Number],DR.NumberRows) AS ShippingPossibleSU
            
           ,ISNULL((Select SUM(DS.SalesSu)
            from D_SalesDetails AS DS
            Where DS.JuchuuNO = DM.JuchuuNO
            AND DS.JuchuuRows = DM.JuchuuRows
            AND DS.DeleteDateTime IS NULL
            Group by DS.JuchuuNO,DS.JuchuuRows),0) AS SalesSu

        /*   ,(Select distinct DO.OrderNO
            from D_OrderDetails AS DO
            Where DO.JuchuuNO = DM.JuchuuNO
            AND DO.JuchuuRows = DM.JuchuuRows
            AND DO.DeleteDateTime IS NULL) AS OrderNO */

		   ,NULL AS OrderNO
           ,DM.DirectFLG
            
          ,DM.JuchuuSuu - ISNULL((Select SUM(DS.SalesSu)
            from D_SalesDetails AS DS
            Where DS.JuchuuNO = DM.JuchuuNO
            AND DS.JuchuuRows = DM.JuchuuRows
            AND DS.DeleteDateTime IS NULL
            Group by DS.JuchuuNO,DS.JuchuuRows),0) AS JuchuuZan
           
           ,ISNULL((SELECT COUNT(distinct DS.SalesNO) 
             from D_SalesDetails AS DS 
             Where DS.JuchuuNO = DM.JuchuuNO
            AND DS.DeleteDateTime IS NULL
            Group by DS.JuchuuNO),0) AS CNT
           
           --,(SELECT MAX(DS.SalesNO) 
           --  from D_SalesDetails AS DS 
           --  Where DS.JuchuuNO = DM.JuchuuNO
           -- AND DS.DeleteDateTime IS NULL
           -- Group by DS.JuchuuNO) AS SalesNO
           ,DS.SalesNO

      FROM D_Juchuu DH

      INNER JOIN D_JuchuuDetails AS DM ON DH.JuchuuNO = DM.JuchuuNO AND DM.DeleteDateTime IS NULL
      LEFT OUTER JOIN (SELECT A.SalesNO, A.JuchuuNO,Max(A.JuchuuRows)as JuchuuRows FROM D_SalesDetails AS A
                        WHERE A.DeleteDateTime IS NULL
                        GROUP BY A.SalesNO, A.JuchuuNO
        )AS DS ON DS.JuchuuNO = DM.JuchuuNO

      WHERE DH.CustomerCD = @CustomerCD               
      AND DH.DeleteDateTime IS Null
      ORDER BY DH.JuchuuDate desc, DH.JuchuuNO desc, DM.DisplayRows
      ;
END


GO


