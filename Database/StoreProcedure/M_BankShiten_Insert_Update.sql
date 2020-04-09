 BEGIN TRY 
 Drop Procedure dbo.[M_BankShiten_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_BankShiten_Insert_Update]
	-- Add the parameters for the stored procedure here

		@BankCD as varchar(4),
		@BranchCD  as varchar(3),
		@ChangeDate date,
		@BranchName varchar(30),
		@BranchKana varchar (30),
		@Remarks varchar(500),

		@DeleteFlg tinyint,
		@Operator varchar(10),
		@Program as varchar(30),
		@PC as varchar(30),
		@OperateMode as varchar(10),
		@KeyItem as varchar(100),
		@Mode as tinyint-- 1 - insert, 2 - update

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @currentDate as datetime = getdate();

	if @Mode = 1--insert mode
	begin

	insert into M_BankBranch(BankCD,BranchCD,ChangeDate,BranchName,BranchKana,Remarks,DeleteFlg,UsedFlg,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	values(@BankCD,@BranchCD,@ChangeDate,@BranchName,@BranchKana,@Remarks,@DeleteFlg,'0' ,@Operator,@currentDate ,@Operator ,@currentDate)

	end

	else if @Mode=2 --update mode
	begin	
	update	 M_BankBranch

	set		BranchName=@BranchName,
			BranchKana=@BranchKana,
			Remarks=@Remarks,
			DeleteFlg=@DeleteFlg,
			UpdateOperator=@Operator,
			UpdateDateTime=@currentDate
						
	where	BankCD=@BankCD
	and		BranchCD=@BranchCD
	and		ChangeDate=@ChangeDate	 
	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

