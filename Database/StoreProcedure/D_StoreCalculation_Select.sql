 BEGIN TRY 
 Drop Procedure dbo.[D_StoreCalculation_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_StoreCalculation_Select]    */
CREATE PROCEDURE D_StoreCalculation_Select(
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.StoreCD
    from D_StoreCalculation AS MS
    
    WHERE MS.StoreCD = @StoreCD
    AND MS.CalculationDate = CONVERT(DATE, @ChangeDate)
    ;
END

GO
