 BEGIN TRY 
 Drop Procedure dbo.[D_Exclusive_Insert]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Exclusive_Insert]    */
CREATE PROCEDURE [dbo].[D_Exclusive_Insert](
    -- Add the parameters for the stored procedure here
    @DataKBN tinyint,
    @Number varchar(20),
    @Operator  varchar(10),
    @Program  varchar(100),
    @PC  varchar(30)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    INSERT INTO D_Exclusive
           ([DataKBN]
           ,[Number]
           ,[OperateDataTime]
           ,[Operator]
           ,[Program]
           ,[PC])
     VALUES
           (@DataKBN
           ,@Number
           ,SYSDATETIME()
           ,@Operator
           ,@Program
           ,@PC
           );

END


