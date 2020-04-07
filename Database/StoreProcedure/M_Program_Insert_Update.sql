 BEGIN TRY 
 Drop Procedure dbo.[M_Program_Insert_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[M_Program_Insert_Update]
@ProgramID  as varchar(20),
@ProgramName as varchar(100),
@Type as tinyint,
@ProgramEXE as varchar(100),
@FileDrive as varchar(1),
@FilePass as varchar(100),
@FileName as varchar(50),
@Operator as varchar(10),
@Program as varchar(30),
@PC as varchar(30),
@OperateMode as varchar(10),
@KeyItem as varchar(100),
@Mode as tinyint

AS
BEGIN
	declare @currentDate as datetime = getdate();
	if @Mode = 1--insert mode
		begin	
		 insert into M_Program
		 (
		   ProgramID,
		   ProgramName,
		   Type,
		   ProgramEXE,
		   FileDrive,
		   FilePass,
		   FileName,
		   InsertOperator,
		   InsertDateTime,
		   UpdateOperator,
		   UpdateDateTime
		 )
		 values
		 (
		  @ProgramID,
		  @ProgramName,
		  @Type,
		  @ProgramEXE,
		  @FileDrive,
		  @FilePass,
		  @FileName,
		  @Operator,
		  @currentDate,
		  @Operator,
		  @currentDate
		 )
		 end
		 	
		else if @Mode = 2--update mode
				begin
				Update M_Program
				 set 
				  ProgramID=@ProgramID,
				  ProgramName=@ProgramName,
				  Type=@Type,
				  ProgramEXE=@ProgramEXE,
				  FileDrive=@FileDrive,
				  FilePass=@FilePass,
				  FileName=@FileName,
				  UpdateOperator= @Operator,
				  UpdateDateTime = @currentDate

				  where ProgramID=@ProgramID
				
				end		
				exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem								
	
END

