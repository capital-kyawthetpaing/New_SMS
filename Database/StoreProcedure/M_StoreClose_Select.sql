

BEGIN TRY 
 Drop PROCEDURE dbo.[M_StoreClose_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_StoreClose_Select]    */
CREATE PROCEDURE M_StoreClose_Select(
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
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    from M_StoreClose MS
    
    WHERE MS.StoreCD = @StoreCD
    AND MS.FiscalYYYYMM = @FiscalYYYYMM
    ;
END

GO
