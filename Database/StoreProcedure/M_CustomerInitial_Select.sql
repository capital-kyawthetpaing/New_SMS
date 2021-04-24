BEGIN TRY 
 Drop Procedure [dbo].[M_CustomerInitial_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_CustomerInitial_Select]    */
CREATE PROCEDURE [dbo].[M_CustomerInitial_Select](
    -- Add the parameters for the stored procedure here
    @StoreKBN  tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT *
    FROM M_CustomerInitial
    WHERE [StoreKBN] = @StoreKBN
    ;
END


GO
