 BEGIN TRY 
 Drop Procedure dbo.[M_Store_SelectByApiKey]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_Store_SelectByApiKey]    */
CREATE PROCEDURE [dbo].[M_Store_SelectByApiKey](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @ChangeDate varchar(10),
    @APIKey tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT FS.StoreCD
        ,CONVERT(varchar,FS.ChangeDate,111) AS ChangeDate

    from F_Store(@ChangeDate) AS FS

    WHERE FS.StoreCD <> @StoreCD
    AND FS.APIKey = @APIKey
    AND FS.StoreKBN = 2
    AND FS.DeleteFlg = 0
    ;
END


