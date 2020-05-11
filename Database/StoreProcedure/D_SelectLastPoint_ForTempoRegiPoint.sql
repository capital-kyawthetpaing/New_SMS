--  ======================================================================
--       最新ポイント取得
--  ======================================================================
USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ ポイント引換券印刷
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.17
--    ======================================================================
CREATE PROCEDURE [dbo].[D_SelectLastPoint_ForTempoRegiPoint]
(
     @CustomerCD varchar(13)
)AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT customer.LastPoint
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) RANK
                  ,*
              FROM M_Customer
             WHERE CustomerCD = @CustomerCD
               AND ChangeDate <= CONVERT(DATE, GETDATE())
               AND DeleteFlg = 0
           ) customer
     WHERE customer.RANK = 1
         ;
END

