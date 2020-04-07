 BEGIN TRY 
 Drop Procedure dbo.[M_Site_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Site_Select]    */
CREATE PROCEDURE M_Site_Select(
    -- Add the parameters for the stored procedure here
    @AdminNO int,
    @APIKey tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 [ShouhinCD] AS ItemSKUCD
        ,[APIKey]
        ,[SiteURL]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
    FROM M_Site

    WHERE AdminNO = @AdminNO
    AND APIKey = CASE WHEN @APIKey <> null THEN @APIKey ELSE APIKey END
    ORDER BY APIKey
    ;
END

