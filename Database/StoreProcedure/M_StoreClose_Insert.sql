 BEGIN TRY 
 Drop Procedure dbo.[M_StoreClose_Insert]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_StoreClose_Insert]    */
CREATE PROCEDURE [dbo].[M_StoreClose_Insert](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @FiscalYYYYMM int,
    @Operator varchar(10),
    @Mode tinyint	--1:全社
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Mode = 2
    BEGIN
        INSERT INTO M_StoreClose
            ([StoreCD]
              ,[FiscalYYYYMM]
              ,[ClosePosition1]
              ,[ClosePosition2]
              ,[ClosePosition3]
              ,[ClosePosition4]
              ,[ClosePosition5]
              ,[MonthlyClaimsFLG]
              ,[MonthlyClaimsDateTime]
              ,[MonthlyDebtFLG]
              ,[MonthlyDebtDateTime]
              ,[MonthlyStockFLG]
              ,[MonthlyStockDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime])
        SELECT @StoreCD
              ,@FiscalYYYYMM
              ,0 AS ClosePosition1
              ,0 AS ClosePosition2
              ,0 AS ClosePosition3
              ,0 AS ClosePosition4
              ,0 AS ClosePosition5
              ,0 AS MonthlyClaimsFLG
              ,NULL AS MonthlyClaimsDateTime
              ,0 AS MonthlyDebtFLG
              ,NULL AS MonthlyDebtDateTime
              ,0 AS MonthlyStockFLG
              ,NULL AS MonthlyStockDateTime
              ,@Operator AS UpdateOperator
              ,SYSDATETIME() AS UpdateDateTime
        ;
    END
    ELSE
    BEGIN
        DECLARE @ChangeDate varchar(10);
        SET @ChangeDate = SUBSTRING(CONVERT(varchar, @FiscalYYYYMM),1,4) + '/' + SUBSTRING(CONVERT(varchar, @FiscalYYYYMM),5,2) + '/01';
        
        INSERT INTO M_StoreClose
            ([StoreCD]
              ,[FiscalYYYYMM]
              ,[ClosePosition1]
              ,[ClosePosition2]
              ,[ClosePosition3]
              ,[ClosePosition4]
              ,[ClosePosition5]
              ,[MonthlyClaimsFLG]
              ,[MonthlyClaimsDateTime]
              ,[MonthlyDebtFLG]
              ,[MonthlyDebtDateTime]
              ,[MonthlyStockFLG]
              ,[MonthlyStockDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime])
        SELECT FS.StoreCD
              ,@FiscalYYYYMM
              ,0 AS ClosePosition1
              ,0 AS ClosePosition2
              ,0 AS ClosePosition3
              ,0 AS ClosePosition4
              ,0 AS ClosePosition5
              ,0 AS MonthlyClaimsFLG
              ,NULL AS MonthlyClaimsDateTime
              ,0 AS MonthlyDebtFLG
              ,NULL AS MonthlyDebtDateTime
              ,0 AS MonthlyStockFLG
              ,NULL AS MonthlyStockDateTime
              ,@Operator AS UpdateOperator
              ,SYSDATETIME() AS UpdateDateTime
        FROM F_Store(@ChangeDate) AS FS
        WHERE FS.StoreKBN <> 2
        AND NOT EXISTS(SELECT A.StoreCD FROM M_StoreClose AS A
        		WHERE A.StoreCD = FS.StoreCD
        		AND A.FiscalYYYYMM = @FiscalYYYYMM)
        ;

	END
END


