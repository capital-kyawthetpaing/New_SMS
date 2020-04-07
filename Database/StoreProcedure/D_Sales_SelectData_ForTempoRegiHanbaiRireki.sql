 BEGIN TRY 
 Drop Procedure dbo.[D_Sales_SelectData_ForTempoRegiHanbaiRireki]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 販売履歴照会
--       Program ID      TempoRegiHanbaiRireki
--       Create date:    2019.9.19
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Sales_SelectData_ForTempoRegiHanbaiRireki]
    (    @JuchuuNO   varchar(11)
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
    SELECT DH.SalesNO
          ,DH.StoreCD
          ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
          ,(SELECT top 1 A.StoreName 
          FROM M_Store A 
          WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.SalesDate
          AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS StoreName	--受注毎に最初のレコードにだけ表示
          ,ROW_NUMBER() OVER(PARTITION BY DH.SalesNO ORDER BY DM.SalesRows) ROWNUM
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.SalesDate
          AND A.DeleteFlg = 0
          ORDER BY A.ChangeDate desc) AS StaffName

          ,DM.SalesRows
          ,DM.AdminNO
          ,DM.SKUCD
          ,DM.JanCD
          ,DM.SalesSu
          ,DM.SalesGaku
          
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate 
          ORDER BY A.ChangeDate desc) AS SKUName
         ,(SELECT top 1 (CASE M.VariousFLG WHEN 0 THEN ISNULL(M.ColorName,'') + ' ' + ISNULL(M.SizeName,'') 
                            ELSE ISNULL(DM.ColorName,'')  + ' ' + ISNULL(DM.SizeName,'') END) AS ColorSizeName
           FROM M_SKU AS M
            WHERE M.AdminNO = DM.AdminNO
             AND M.ChangeDate <= DH.SalesDate
             ORDER BY M.ChangeDate desc) AS ColorSizeName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.ColorName ELSE A.ColorName END) AS ColorName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate 
          ORDER BY A.ChangeDate desc) AS ColorName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SizeName ELSE A.SizeName END) AS SizeName 
          FROM M_SKU A 
          WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.SalesDate 
          ORDER BY A.ChangeDate desc) AS SizeName
          
      FROM D_Sales DH

      INNER JOIN D_SalesDetails AS DM ON DH.SalesNO = DM.SalesNO AND DM.DeleteDateTime IS NULL
        
      WHERE DM.JuchuuNO = @JuchuuNO               
      AND DH.DeleteDateTime IS Null
      ORDER BY DH.SalesDate desc, DH.SalesNO, DM.SalesRows	--DisplayRows
      ;
END


