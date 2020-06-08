SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�܃��W �|�C���g���������
--       Program ID      TempoRegiPoint
--       Create date:    2019.12.17
--       Update date:    2020.06.06  ChangeDate���ŐV�̃f�[�^���擾����悤�ɏC��
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_SelectTicketUnit_ForTempRegiPoint')
  DROP PROCEDURE [dbo].[D_SelectTicketUnit_ForTempRegiPoint]
GO


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
               AND ChangeDate <= CONVERT (date, SYSDATETIME())
               AND DeleteFlg = 0) storePoint
     WHERE storePoint.RANK = 1
         ;
END

