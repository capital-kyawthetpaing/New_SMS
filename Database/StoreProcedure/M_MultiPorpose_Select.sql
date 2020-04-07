 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPorpose_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_MultiPorpose_Select]    */
CREATE PROCEDURE [dbo].[M_MultiPorpose_Select](
    -- Add the parameters for the stored procedure here
    @ID  int,
    @Key varchar(50)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT [ID]
          ,[Key]
          ,[IDName]
          ,[Char1]
          ,[Char2]
          ,[Char3]
          ,[Char4]
          ,[Char5]
          ,[Num1]
          ,[Num2]
          ,[Num3]
          ,[Num4]
          ,[Num5]
          ,[Date1]
          ,[Date2]
          ,[Date3]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
        ,CONVERT(varchar,SYSDATETIME(),108) AS HHMISS
	FROM M_MultiPorpose

    WHERE [ID] = @ID
    AND [Key] = @Key
    ;
END


