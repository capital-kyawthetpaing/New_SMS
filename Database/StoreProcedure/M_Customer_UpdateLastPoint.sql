--  ======================================================================
--       �ŐV�|�C���g�X�V�A�����������O�X�V
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
CREATE PROCEDURE [dbo].[M_Customer_UpdateLastPoint]
(
    @CustomerCD varchar(13),
    @IssuePoint money,
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
    SET @KeyItem = @CustomerCD;

    UPDATE M_Customer
       SET LastPoint = LastPoint - @IssuePoint
          ,UpdateOperator = @Operator
          ,UpdateDateTime = @SYSDATETIME
     WHERE CustomerCD = @CustomerCD
       AND ChangeDate = (SELECT customer.ChangeDate
                           FROM (SELECT ROW_NUMBER() OVER(PARTITION BY CustomerCD ORDER BY ChangeDate DESC) RANK
                                       ,CustomerCD
                                       ,ChangeDate
                                   FROM M_Customer) customer
                          WHERE customer.CustomerCD = @CustomerCD
                            AND customer.RANK = 1)

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


