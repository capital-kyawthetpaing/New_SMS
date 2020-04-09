 BEGIN TRY 
 Drop Procedure dbo.[D_Exclusive_Delete]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Exclusive_Delete]    */
CREATE PROCEDURE [dbo].[D_Exclusive_Delete](
    -- Add the parameters for the stored procedure here
    @DataKBN tinyint,
    @Number varchar(20)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    DELETE from D_Exclusive
    WHERE DataKBN = @DataKBN
    AND [Number] = @Number
    ;
END


