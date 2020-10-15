SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 取引レシート出力　両替印刷情報取得
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ロジック変更
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectExchange_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectExchange_ForTempoTorihikiReceipt]
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

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                           -- 登録日
          ,ABS(history.ExchangeCount) ExchangeCount                                    -- 両替回数
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') ExchangeDateTime        -- 両替日
          ,denominationKbn.DenominationName ExchangeName                               -- 両替名
          ,ABS(history.DepositGaku) ExchangeAmount                                     -- 両替額
          ,history.ExchangeDenomination                                                -- 両替紙幣
          ,history.Remark ExchangeRemark                                               -- 両替備考
          ,staff.ReceiptPrint StaffReceiptPrint                                        -- 担当レシート表記
          ,store.ReceiptPrint StoreReceiptPrint                                        -- 店舗レシート表記
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
        AND history.DepositKBN = 5
        AND history.CancelKBN = 0
           ;
END

GO
