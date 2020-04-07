 BEGIN TRY 
 Drop Procedure dbo.[M_StoreButtonGroup_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_StoreButtonGroup_Insert_Update]
	-- Add the parameters for the stored procedure here
   @StoreCD varchar(4),
   @ProgramKBN tinyint,
   @GroupNO tinyint,
   @BottunName varchar(12),
   @MasterKBN tinyint,
   @InsertOperator varchar(10),
   @UpdateOperator varchar(10),
   @Mode tinyint,
   @Operator varchar(10),
   @Program as varchar(30),
   @PC as varchar(30),
   @OperateMode as varchar(10),
   @KeyItem as varchar(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @currentDate as datetime = getdate();

    -- Insert statements for procedure here
	if @Mode = 1--insert mode
	--IF not Exists(select 1 from M_StoreBottunGroup where StoreCD=@StoreCD and ProgramKBN=@ProgramKBN and GroupNO=@GroupNO)
	begin
		Insert Into M_StoreBottunGroup(
		StoreCD,
		ProgramKBN,
		GroupNO,
		BottunName,
		MasterKBN,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime
		)
		Values
		(
		@StoreCD,
		@ProgramKBN,
		@GroupNO,
		@BottunName ,
		@MasterKBN,
		@InsertOperator,
		@currentDate,
		@UpdateOperator ,
		@currentDate
		)
	end
	
	else if @Mode = 2--update mode
	begin
		update M_StoreBottunGroup
		set 
		GroupNO=@GroupNO,
		BottunName=@BottunName,
		MasterKBN=@MasterKBN,
		UpdateOperator=@UpdateOperator,
		UpdateDateTime=@currentDate
		where StoreCD=@StoreCD
		AND ProgramKBN=1
		AND GroupNO=@GroupNO
		
	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

