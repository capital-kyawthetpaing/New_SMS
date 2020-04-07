 BEGIN TRY 
 Drop Procedure dbo.[M_SalesTax_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SalesTax_Select]    */
CREATE PROCEDURE [dbo].[M_SalesTax_Select](
    -- Add the parameters for the stored procedure here
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 M.ChangeDate
      ,M.TaxRate1
      ,M.TaxRate2
      ,M.FractionKBN
    from M_SalesTax M
    WHERE M.ChangeDate <= convert(date,@ChangeDate)
    ORDER BY M.ChangeDate desc
    ;
END


