--  ======================================================================
--       ���i���������擾
--  ======================================================================
USE [CAP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�܃��W �|�C���g���������
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.22
--  ======================================================================
CREATE PROCEDURE [dbo].[D_Coupon_Select]
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
          ,Print1
          ,Bold1
          ,Size1
          ,Print2
          ,Bold2
          ,Size2
          ,Print3
          ,Bold3
          ,Size3
          ,Print4
          ,Bold4
          ,Size4
          ,Print5
          ,Bold5
          ,Size5
          ,Print6
          ,Bold6
          ,Size6
          ,Print7
          ,Bold7
          ,Size7
          ,Print8
          ,Bold8
          ,Size8
          ,Print9
          ,Bold9
          ,Size9
          ,Print10
          ,Bold10
          ,Size10
          ,Print11
          ,Bold11
          ,Size11
          ,Print12
          ,Bold12
          ,Size12
      FROM (SELECT ROW_NUMBER() OVER(PARTITION BY StoreCD ORDER BY ChangeDate DESC) RANK
                  ,TicketUnit
                  ,Print1
                  ,Bold1
                  ,Size1
                  ,Print2
                  ,Bold2
                  ,Size2
                  ,Print3
                  ,Bold3
                  ,Size3
                  ,Print4
                  ,Bold4
                  ,Size4
                  ,Print5
                  ,Bold5
                  ,Size5
                  ,Print6
                  ,Bold6
                  ,Size6
                  ,Print7
                  ,Bold7
                  ,Size7
                  ,Print8
                  ,Bold8
                  ,Size8
                  ,Print9
                  ,Bold9
                  ,Size9
                  ,Print10
                  ,Bold10
                  ,Size10
                  ,Print11
                  ,Bold11
                  ,Size11
                  ,Print12
                  ,Bold12
                  ,Size12
              FROM M_StorePoint
             WHERE StoreCD = @StoreCD
               AND DeleteFlg = 0) storePoint
         ;
END
GO

