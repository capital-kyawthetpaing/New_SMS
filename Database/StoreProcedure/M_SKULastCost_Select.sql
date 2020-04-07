 BEGIN TRY 
 Drop Procedure dbo.[M_SKULastCost_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SKULastCost_Select]    */
CREATE PROCEDURE [dbo].[M_SKULastCost_Select](
    -- Add the parameters for the stored procedure here
    @AdminNO int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.SKUCD
          ,MS.LastCost
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime

    from M_SKULastCost MS
    
    WHERE MS.AdminNO = @AdminNO
    ;
END


