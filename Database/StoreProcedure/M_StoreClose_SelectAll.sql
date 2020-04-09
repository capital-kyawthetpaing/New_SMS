 BEGIN TRY 
 Drop Procedure dbo.[M_StoreClose_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_StoreClose_SelectAll]    */
CREATE PROCEDURE [dbo].[M_StoreClose_SelectAll](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @FiscalYYYYMM int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.StoreCD
          ,MS.FiscalYYYYMM
          ,MS.ClosePosition1
          ,MS.ClosePosition2
          ,MS.ClosePosition3
          ,MS.ClosePosition4
          ,MS.ClosePosition5
          ,MS.MonthlyClaimsFLG
          ,MS.MonthlyClaimsDateTime
          ,MS.MonthlyDebtFLG
          ,MS.MonthlyDebtDateTime
          ,MS.MonthlyStockFLG
          ,MS.MonthlyStockDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    from M_StoreClose MS
    
    WHERE MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
    AND MS.FiscalYYYYMM <= (CASE WHEN @FiscalYYYYMM <> 0 THEN @FiscalYYYYMM ELSE MS.FiscalYYYYMM END)
    ORDER BY MS.StoreCD, MS.FiscalYYYYMM desc
    ;
END


