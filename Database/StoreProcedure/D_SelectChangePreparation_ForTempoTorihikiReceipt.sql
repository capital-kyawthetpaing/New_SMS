SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    店舗レジ 取引レシート出力　釣銭準備情報取得
--       Program ID      TempoRegiTorihikiReceipt
--       Create date:    2020.02.24
--       Update date:    2020.06.13  ロジック変更
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectChangePreparation_ForTempoTorihikiReceipt')
  DROP PROCEDURE [dbo].[D_SelectChangePreparation_ForTempoTorihikiReceipt]
GO


CREATE PROCEDURE [dbo].[D_SelectChangePreparation_ForTempoTorihikiReceipt]
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

    SELECT CONVERT(DATE, history.DepositDateTime) RegistDate                                -- 登録日
          ,FORMAT(history.DepositDateTime, 'yyyy/MM/dd HH:mm') ChangePreparationDate        -- 釣銭準備日
          ,'現金' ChangePreparationName                                                     -- 釣銭準備名
          ,history.DepositGaku ChangePreparationAmount                                      -- 釣銭準備額
          ,history.Remark ChangePreparationRemark                                           -- 釣銭準備考
          ,staff.ReceiptPrint StaffReceiptPrint                                             -- 担当レシート表記
          ,store.ReceiptPrint StoreReceiptPrint                                             -- 店舗レシート表記
      FROM D_DepositHistory history
      LEFT OUTER JOIN M_DenominationKBN denominationKbn ON denominationKbn.DenominationCD = history.DenominationCD
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
     WHERE history.DepositNO = @DepositNO
       AND history.DataKBN = 3
       AND history.DepositKBN = 6
       AND history.CancelKBN = 0
       AND store.DeleteFlg <> 1
       AND staff.DeleteFlg <> 1
        ;
END

GO
