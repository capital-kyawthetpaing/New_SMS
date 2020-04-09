 BEGIN TRY 
 Drop Procedure dbo.[M_Program_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  ProgramdProcedure [M_Program_Select]    */
CREATE PROCEDURE [dbo].[M_Program_Select](
    -- Add the parameters for the Programd procedure here
    @ProgramID  varchar(20)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [ProgramID]
	      ,[ProgramName]
	      ,[Type]
		  ,[ProgramEXE]
		  ,[FileDrive]
		  ,[FilePass]
		  ,[FileName]
	      ,[InsertOperator]
	      ,[InsertDateTime]
	      ,[UpdateOperator]
	      ,[UpdateDateTime]
	  FROM M_Program
    
    WHERE ProgramID = @ProgramID
    ;
END


