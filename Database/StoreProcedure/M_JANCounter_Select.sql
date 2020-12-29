 BEGIN TRY 
 Drop Procedure dbo.[M_JANCounter_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_JANCounter_Select]    */
CREATE PROCEDURE M_JANCounter_Select(
    -- Add the parameters for the stored procedure here
    @MainKEY int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT M.MainKEY
          ,M.JanCount
          ,M.UpdatingFlg
          ,M.BeforeJanCount
          ,M.UpdateOperator
          ,CONVERT(varchar,M.UpdateDateTime) AS UpdateDateTime

    from M_JANCounter M
    
    WHERE M.MainKEY = @MainKEY
    ;
END

GO

