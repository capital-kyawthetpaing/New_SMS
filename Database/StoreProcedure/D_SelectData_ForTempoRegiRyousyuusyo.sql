SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 領収書印刷
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2019.11.13
--       Update date:    2020.05.23  TelphoneNO → TelephoneNO
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectData_ForTempoRegiRyousyuusyo')
  DROP PROCEDURE [dbo].[D_SelectData_ForTempoRegiRyousyuusyo]
GO


CREATE PROCEDURE [dbo].[D_SelectData_ForTempoRegiRyousyuusyo]
    (
     @SalesNO varchar(11),
     @PrintDate varchar(10)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT sales.SalesNO
          ,CASE
             WHEN @PrintDate IS NULL THEN sales.InsertDateTime
             ELSE @PrintDate
           END AS UriageDateTime
          ,NULL AS AiteName
          ,CAST(FLOOR(sales.SalesGaku) AS decimal) SalesGaku
          ,CAST(FLOOR(sales.SalesTax) AS decimal) SalesTax
          ,ctrl.CompanyName
          ,(SELECT top 1 store.StoreName 
            FROM M_Store as store
            where store.StoreCD = sales.StoreCD
            AND store.DeleteFlg = 0
            AND store.ChangeDate <= sales.SalesDate
            ORDER BY store.ChangeDate desc) AS StoreName
          ,(SELECT top 1 store.Address1 
            FROM M_Store as store
            where store.StoreCD = sales.StoreCD
            AND store.DeleteFlg = 0
            AND store.ChangeDate <= sales.SalesDate
            ORDER BY store.ChangeDate desc) AS Address1
          ,(SELECT top 1 store.Address2 
            FROM M_Store as store
            where store.StoreCD = sales.StoreCD
            AND store.DeleteFlg = 0
            AND store.ChangeDate <= sales.SalesDate
            ORDER BY store.ChangeDate desc) AS Address2
          ,'電話 ' + (SELECT top 1 store.TelephoneNO 
                      FROM M_Store as store
                      where store.StoreCD = sales.StoreCD
                      AND store.ChangeDate <= sales.SalesDate
                      ORDER BY store.ChangeDate desc) AS TelephoneNO
          ,(SELECT top 1 store.ReceiptPrint 
            FROM M_Store as store
            where store.StoreCD = sales.StoreCD
            AND store.DeleteFlg = 0
            AND store.ChangeDate <= sales.SalesDate
            ORDER BY store.ChangeDate desc) + '-' + 
           (SELECT top 1 staff.ReceiptPrint 
            FROM M_Staff as staff
            where staff.StaffCD = sales.StaffCD
            AND staff.DeleteFlg = 0
            AND staff.ChangeDate <= sales.SalesDate
            ORDER BY staff.ChangeDate desc) AS ReceiptPrint
          ,multiPorpose.Char1
          ,multiPorpose.Char2
          ,ctrl.MainKey
          ,sales.StoreCD
          ,(SELECT top 1 store.ChangeDate 
            FROM M_Store as store
            where store.StoreCD = sales.StoreCD
            AND store.DeleteFlg = 0
            AND store.ChangeDate <= sales.SalesDate
            ORDER BY store.ChangeDate desc) AS ChangeDate
          ,sales.SalesDate
      FROM D_Sales sales
      LEFT OUTER JOIN M_Control ctrl ON ctrl.MainKey = 1
      LEFT OUTER JOIN M_multiPorpose multiPorpose 
      ON multiPorpose.ID = 305
      AND multiPorpose.[Key] = sales.SalesNO
      WHERE sales.DeleteDateTime IS NULL
        AND sales.SalesNo = @SalesNO
        ;
END
