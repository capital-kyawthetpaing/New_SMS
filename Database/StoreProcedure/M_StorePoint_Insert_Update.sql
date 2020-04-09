 BEGIN TRY 
 Drop Procedure dbo.[M_StorePoint_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_StorePoint_Insert_Update]
	-- Add the parameters for the stored procedure here
	@StoreCD varchar(4),
	@ChangeDate date,
	@PointRate decimal,
	@ServicedayRate decimal,
	@ExpirationDate int,
	@MaxPoint  money,
	@TicketUnit money,
	
	@Print1 varchar(10),
	@Size1 	 tinyint,
	@Bold1	 tinyint,
	
	@Print2 varchar(10),
	@Size2 tinyint,
	@Bold2 tinyint,
	
	@Print3 varchar(10),
	@Size3 tinyint,
	@Bold3 tinyint,
	
	@Print4 varchar(10),
	@Size4 tinyint,
	@Bold4 tinyint,
	
	@Print5 varchar(10),
	@Size5 tinyint,
	@Bold5 tinyint,
	
	@Print6 varchar(10),
	@Size6 tinyint,
	@Bold6 tinyint,
	
	@Print7 varchar(10),
	@Size7 tinyint,
	@Bold7 tinyint,
	
	@Print8 varchar(10),
	@Size8 tinyint,
	@Bold8 tinyint,
	
	@Print9 varchar(10),
	@Size9 tinyint,
	@Bold9 tinyint,
	
	@Print10 varchar(10),
	@Size10 tinyint,
	@Bold10 tinyint,
	
	@Print11 varchar(10),
	@Size11 tinyint,
	@Bold11 tinyint,
	
	@Print12 varchar(10),
	@Size12 tinyint,
	@Bold12	tinyint,
	
	@DeleteFlg tinyint,
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

    -- Insert statements for procedure here
	Declare
	@Datetime datetime=getdate()

	IF not Exists(select 1 from M_StorePoint where StoreCD=@StoreCD and ChangeDate=@ChangeDate)
	begin
		Insert into M_StorePoint(StoreCD,ChangeDate,PointRate,ServicedayRate,
		ExpirationDate,MaxPoint,TicketUnit,
					Print1,Bold1,Size1,
					Print2,Bold2,Size2,
					Print3,Bold3,Size3,
					Print4,Bold4,Size4,
					Print5,Bold5,Size5,
					Print6,Bold6,Size6,
					Print7,Bold7,Size7,
					Print8,Bold8,Size8,
					Print9,Bold9,Size9,
					Print10,Bold10,Size10,
					Print11,Bold11,Size11,
					Print12,Bold12,Size12,
					DeleteFlg,
					InsertOperator,
					InsertDateTime,
					UpdateOperator,
					UpdateDateTime)
		values (@StoreCD,@ChangeDate, @PointRate ,@ServicedayRate ,
		@ExpirationDate ,@MaxPoint  ,@TicketUnit ,
				@Print1,@Bold1,@Size1 ,
				@Print2,@Bold2,@Size2 ,
				@Print3,@Bold3,@Size3 ,
				@Print4,@Bold4,@Size4 ,
				@Print5,@Bold5,@Size5 , 
				@Print6,@Bold6,@Size6, 
				@Print7,@Bold7,@Size7 ,
				@Print8,@Bold8,@Size8 ,
				@Print9,@Bold9,@Size9 ,
				@Print10,@Bold10,@Size10 ,
				@Print11,@Bold11,@Size11 ,
				@Print12,@Bold12,@Size12 ,
				@DeleteFlg,
				@Operator,
				@Datetime,@Operator,
				@Datetime
				)
	end

	Else
	begin

		Update M_StorePoint
		set StoreCD			= @StoreCD,
			ChangeDate		= @ChangeDate,
			PointRate		= @PointRate,
			ServicedayRate	= @ServicedayRate,
			ExpirationDate	= @ExpirationDate,
			MaxPoint		= @MaxPoint,
			TicketUnit		= @TicketUnit,
			Print1			= @Print1,
			Bold1			= @Bold1,
			Size1			= @Size1,

			Print2			= @Print2,
			Bold2			= @Bold2,
			Size2			= @Size2,

			Print3			= @Print3,
			Bold3			= @Bold3,
			Size3			= @Size3,

			Print4			= @Print4,
			Bold4			= @Bold4,
			Size4			=@Size4,

			Print5			= @Print5,
			Bold5			= @Bold5,
			Size5			= @Size5,

			Print6			= @Print6,
			Bold6			= @Bold6,
			Size6			=@Size6,

			Print7			= @Print7,
			Bold7			= @Bold7,
			Size7			= @Size7,

			Print8			=@Print8,
			Bold8			=@Bold8,
			Size8			= @Size8,

			Print9			= @Print9,
			Bold9			= @Bold9,
			Size9			= @Size9,

			Print10			= @Print10,
			Bold10			= @Bold10,
			Size10			= @Size10,

			Print11			= @Print11,
			Bold11			= @Bold11,
			Size11			= @Size11,

			Print12			= @Print12,
			Bold12			= @Bold12,
			Size12			= @Size12,

			DeleteFlg		= @DeleteFlg,
			UpdateOperator	=@Operator,
			UpdateDateTime	=@Datetime
			where StoreCD=@StoreCD
			and ChangeDate=@ChangeDate

	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
	
END

