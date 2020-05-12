 BEGIN TRY 
 Drop Procedure dbo.[D_DepositHistory_UpdateIssued]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    �X�܃��W �̎������ �X�܎������ ���s�ύX�V
--       Program ID      TempoRegiRyousyuusyo
--       Create date:    2019.11.23
--    ======================================================================
CREATE PROCEDURE [dbo].[D_DepositHistory_UpdateIssued]
(
    @IsIssued tinyint,
    @SalesNO varchar(11),
    @Operator varchar(10),
    @Program varchar(100),
    @PC varchar(30)
)AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @SYSDATETIME datetime;
    DECLARE @KeyItem varchar(100);

    SET @SYSDATETIME = SYSDATETIME();
    SET @KeyItem = @SalesNO;

    UPDATE D_DepositHistory
       SET IsIssued = @IsIssued
          ,UpdateOperator = @Operator
          ,UpdateDateTime = @SYSDATETIME
     WHERE Number = @KeyItem
       AND IsIssued = 0
     ;
    
    --���������f�[�^�֍X�V
    EXEC L_Log_Insert_SP
        @SYSDATETIME
       ,@Operator
       ,@Program
       ,@PC
       ,NULL
       ,@KeyItem
    ;
END
GO


