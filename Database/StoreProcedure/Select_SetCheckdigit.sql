 BEGIN TRY 
 Drop Procedure dbo.[Select_SetCheckdigit]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [Select_SetCheckdigit]    */
CREATE PROCEDURE Select_SetCheckdigit(
    -- Add the parameters for the stored procedure here
    @inJAN12 varchar(12)    
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select dbo.Fnc_SetCheckdigit(@inJAN12) AS JANCD
    ;
END

GO
