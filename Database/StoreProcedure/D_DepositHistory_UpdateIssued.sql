 BEGIN TRY 
 Drop Procedure dbo.[D_DepositHistory_UpdateIssued]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ 領収書印刷 店舗取引履歴 発行済更新
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
--                 処理開始                   --
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
    
    --処理履歴データへ更新
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


