SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 取引レシート出力　入金印刷情報取得
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ロジック変更
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectDeposit_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectDeposit_ForTempoTorihikiReceipt]
(
    @DepositNO        int,
    @StaffCD          varchar(10)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                      -- 登録日
          ,customer.CustomerCD                                                    -- 入金元CD
          ,customer.CustomerName                                                  -- 入金元名
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') DepositDate        -- 入金日
          ,denominationKbn.DenominationName DepositName                           -- 入金名
          ,history.DepositGaku DepositAmount                                      -- 入金額
          ,history.Remark DepositRemark                                           -- 入金備考
          ,staff.ReceiptPrint StaffReceiptPrint                                   -- 担当レシート表記
          ,store.ReceiptPrint StoreReceiptPrint                                   -- 店舗レシート表記
      FROM D_DepositHistory history
      LEFT OUTER JOIN D_Sales sales ON sales.SalesNO = history.Number
      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
      LEFT OUTER JOIN (
                       SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) AS RANK
                             ,CustomerCD
                             ,CustomerName
                             ,ChangeDate
                             ,DeleteFlg
                         FROM M_Customer
                      ) customer ON customer.RANK = 1
                                AND customer.CustomerCD = history.CustomerCD
                                AND customer.ChangeDate <= history.AccountingDate
      LEFT OUTER JOIN (
                       SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) as RANK
                             ,StoreCD
                             ,ReceiptPrint
                             ,DeleteFlg 
                         FROM M_Store 
                      ) store ON store.RANK = 1
                             AND store.StoreCD = history.StoreCD
      LEFT OUTER JOIN (
                       SELECT ROW_NUMBER() OVER(PARTITION BY StaffCD ORDER BY ChangeDate DESC) AS RANK
                             ,StaffCD
                             ,StoreCD
                             ,ReceiptPrint
                             ,DeleteFlg
                         FROM M_Staff
                      ) staff ON staff.RANK = 1
                             AND staff.StoreCD = history.StoreCD
                             AND staff.StaffCD = @StaffCD
     WHERE history.DataKBN = 3
       AND history.DepositKBN = 2
       AND history.DepositNO = @DepositNO
       AND history.CancelKBN = 0
       AND history.CustomerCD IS NOT NULL
       AND sales.DeleteDateTime IS NULL
       AND sales.BillingType = 1
       AND customer.DeleteFlg = 0
       AND store.DeleteFlg <> 1
       AND staff.DeleteFlg <> 1
        ;
END

GO
