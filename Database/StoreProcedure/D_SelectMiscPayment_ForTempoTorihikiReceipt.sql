SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 取引レシート出力　雑出金印刷情報取得
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ロジック変更
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectMiscPayment_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectMiscPayment_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectMiscPayment_ForTempoTorihikiReceipt]
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

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                              -- 登録日
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') MiscPaymentDateTime        -- 雑支払日
          ,denominationKbn.DenominationName MiscPaymentName                               -- 雑支払名
          ,history.DepositGaku MiscPaymentAmount                                          -- 雑支払額
          ,history.Remark MiscPaymentRemark                                               -- 雑支払備考
          ,staff.ReceiptPrint StaffReceiptPrint                                           -- 担当レシート表記
          ,store.ReceiptPrint StoreReceiptPrint                                           -- 店舗レシート表記
      FROM D_DepositHistory history
      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
      LEFT OUTER JOIN F_Store(CONVERT(DATE, GETDATE())) AS store
        ON store.StoreCD = history.StoreCD
       AND store.DeleteFlg = 0
      LEFT OUTER JOIN F_Staff(CONVERT(DATE, GETDATE())) AS staff
        ON staff.StoreCD = history.StoreCD
       AND staff.StaffCD = @StaffCD
       AND staff.DeleteFlg = 0
       
     WHERE history.DepositNO = @DepositNO
       AND history.DataKBN = 3
       AND history.DepositKBN = 3
       AND history.CancelKBN = 0
           ;
END

GO

