SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�܃��W �̎�������@�폜�ς݂�����ԍ��`�F�b�N
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2020.06.13
--    ======================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo')
  DROP PROCEDURE [dbo].[D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo]
GO

CREATE PROCEDURE [dbo].[D_CheckDeleteSalseNO_ForTempoRegiRyousyuusyo]
    (
     @SalesNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(sales.SalesNo) DeleteSalesNoCount
      FROM D_Sales sales
     WHERE sales.SalesNo = @SalesNO
       AND sales.DeleteDateTime IS NOT NULL
         ;
END
