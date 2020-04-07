 BEGIN TRY 
 Drop Procedure dbo.[M_Staff_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Staff_Insert_Update]
	-- Add the parameters for the stored procedure here
@StaffCD varchar(10),
@ChangeDate date,
@StaffName varchar(40),
@StaffKana varchar(40),
@StoreCD varchar(4),
@BMNCD varchar(2),
@MenuCD varchar(4),
@StoreMenuCD varchar(4),
@AuthorizationsCD varchar(4),
@StoreAuthorizationsCD varchar(5),
@PositionCD varchar(3),
@JoinDate date,
@LeaveDate date,
@Passward varchar(10),
@Remarks varchar(500),
@ReceiptPrint varchar(3),
@DeleteFlg tinyint,
@Operator varchar(10),
@Program as varchar(30),
@PC as varchar(30),
@OperateMode as varchar(10),
@KeyItem as varchar(100),
@Mode as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @currentDate as datetime = getdate();

    -- Insert statements for procedure here
	if @Mode = 1--insert mode
	begin
		Insert Into M_Staff(
		StaffCD,
		ChangeDate,
		StaffName,
		StaffKana,
		StoreCD,
		BMNCD,
		MenuCD,
		StoreMenuCD,
		AuthorizationsCD,
		StoreAuthorizationsCD,
		PositionCD,
		JoinDate,
		LeaveDate,
		[Password],
		Remarks,
		ReceiptPrint,
		DeleteFlg,
		UsedFlg,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime)
		Values
		(
		@StaffCD ,
		@ChangeDate ,
		@StaffName,
		@StaffKana ,
		@StoreCD,
		@BMNCD,
		@MenuCD,
		@StoreMenuCD,
		@AuthorizationsCD,
		@StoreAuthorizationsCD ,
		@PositionCD ,
		@JoinDate,
		@LeaveDate,
		@Passward ,
		@Remarks ,
		@ReceiptPrint ,
		@DeleteFlg  ,
		0,
		@Operator,
		@currentDate ,
		@Operator ,
		@currentDate)
	end
	
	else if @Mode = 2--update mode
	begin
		update M_Staff 
		set 
		StaffCD = @StaffCD,
		ChangeDate = @ChangeDate,
		StaffName = @StaffName,
		StaffKana = @StaffKana,
		StoreCD = @StoreCD,
		BMNCD = @BMNCD,
		MenuCD = @MenuCD,
		StoreMenuCD=@StoreMenuCD,
		AuthorizationsCD = @AuthorizationsCD,
		StoreAuthorizationsCD = @StoreAuthorizationsCD,
		PositionCD = @PositionCD,
		JoinDate = @JoinDate,
		LeaveDate = @LeaveDate,
		[Password] = @Passward,
		Remarks = @Remarks,
		ReceiptPrint = @ReceiptPrint,
		DeleteFlg = @DeleteFlg,
		UpdateOperator = @currentDate,
		UpdateDateTime = @currentDate
		Where StaffCD = @StaffCD
		AND ChangeDate = @ChangeDate
	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

