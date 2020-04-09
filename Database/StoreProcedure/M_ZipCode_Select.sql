 BEGIN TRY 
 Drop Procedure dbo.[M_ZipCode_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_ZipCode_Select]    */
CREATE PROCEDURE [dbo].[M_ZipCode_Select](
    -- Add the parameters for the stored procedure here
    @ZipCD1  varchar(3),
    @ZipCD2 varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [ZipCD1]
	      ,[ZipCD2]
	      ,[Address1]
	      ,[Address2]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
	FROM M_ZipCode
	---KTP 2019-06-03 filter require condition only
    WHERE (@ZipCD1 is null or ([ZipCD1] = @ZipCD1))
    AND (@ZipCD2 is null or ([ZipCD2] = @ZipCD2))
    ;
END


