SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジポイント引換券印刷　会員情報取得
--       Program ID      TempoRegiPoint
--       Create date:    2020.05.30
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_GetCustomer_ForTempoRegiPoint')
  DROP PROCEDURE [dbo].[D_GetCustomer_ForTempoRegiPoint]
GO


CREATE PROCEDURE [dbo].[D_GetCustomer_ForTempoRegiPoint]
(
    @CustomerCD  varchar(13)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT customer.CustomerCD
          ,customer.CustomerName
          ,customer.DeleteFlg
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) RANK
                  ,*
              FROM M_Customer
             WHERE CustomerCD = @CustomerCD
           ) customer
     WHERE customer.RANK = 1
         ;
END
