USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 領収書印刷
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2019.11.13
--    ======================================================================
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
          ,store.StoreName
          ,store.Address1
          ,store.Address2
          ,'電話 ' + store.TelphoneNO AS TelphoneNO
          ,store.ReceiptPrint + '-' + staff.ReceiptPrint AS ReceiptPrint
          ,multiPorpose.Char1
          ,multiPorpose.Char2
          ,ctrl.MainKey
          ,store.StoreCD
          ,store.ChangeDate
          ,sales.SalesDate
      FROM D_Sales sales
      LEFT OUTER JOIN M_Control ctrl ON ctrl.MainKey = 1
      LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) as RANK
                             ,StoreCD
                             ,StoreName
                             ,Address1
                             ,Address2
                             ,TelphoneNO
                             ,ChangeDate
                             ,ReceiptPrint
                             ,DeleteFlg 
                         FROM M_Store 
                      ) store ON store.RANK = 1
                             AND store.StoreCD = sales.StoreCD
                             AND store.ChangeDate <= sales.SalesDate
      LEFT OUTER JOIN (SELECT ROW_NUMBER() OVER(PARTITION BY StaffCD ORDER BY ChangeDate DESC) AS RANK
                             ,StaffCD
                             ,ChangeDate
                             ,ReceiptPrint
                             ,DeleteFlg
                         FROM M_Staff
                      ) staff ON staff.RANK = 1
                             AND staff.StaffCD = sales.StaffCD
                             AND staff.ChangeDate <= sales.SalesDate
      LEFT OUTER JOIN M_multiPorpose multiPorpose ON multiPorpose.ID = 305
                                                 AND multiPorpose.[Key] = sales.SalesNO
      WHERE sales.DeleteDateTime IS NULL
        AND store.DeleteFlg = 0
        AND staff.DeleteFlg = 0
        AND sales.SalesNo = @SalesNO
        ;
END
GO


