--  ======================================================================
--       ���������s�P�ʎ擾
--    ======================================================================
USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�܃��W �|�C���g���������
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.17
--    ======================================================================
CREATE PROCEDURE [dbo].[D_SelectTicketUnit_ForTempRegiPoint]
(
     @StoreCD varchar(4)
)AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT TicketUnit
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) RANK
                  ,TicketUnit
              FROM M_StorePoint
             WHERE StoreCD = @StoreCD
               AND DeleteFlg = 0) storePoint
         ;
END

