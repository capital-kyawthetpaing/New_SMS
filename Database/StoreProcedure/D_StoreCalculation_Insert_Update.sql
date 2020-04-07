 BEGIN TRY 
 Drop Procedure dbo.[D_StoreCalculation_Insert_Update]
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
CREATE PROCEDURE [dbo].[D_StoreCalculation_Insert_Update]
	-- Add the parameters for the stored procedure here
	@StoreCD as VarChar(6) ,
	@Change as int,
    @10000yen as Int,
    @5000yen as Int , 
	@2000yen as Int, 
    @1000yen as Int, 
    @500yen as Int, 
    @100yen as Int, 
    @50yen as Int, 
    @10yen as Int,
    @5yen as Int, 
    @1yen as Int, 
    @etcyen as Int, 
    @Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @CalculationDate as date = getdate();
	Declare @OperateMode as varchar(10) = null;
	Declare @KeyItem as varchar(100)= @StoreCD + Convert(varchar(10),@CalculationDate);
	
	IF not Exists(select * from D_StoreCalculation where CalculationDate = @CalculationDate and StoreCD = @StoreCD) 
	begin
	
		Insert into D_StoreCalculation( StoreCD,
										CalculationDate,
										Change ,
										[10000yen],
										[5000yen],
										[2000yen],
										[1000yen],
										[500yen],
										[100yen],
										[50yen],
										[10yen],
										[5yen],
										[1yen],
										Etcyen,
										InsertOperator,
										InsertDateTime,
										UpdateOperator,
										UpdateDateTime)


								values (@StoreCD,
										@CalculationDate,
										@Change,
										@10000yen,
										@5000yen ,
										@2000yen,
										@1000yen ,
										@500yen ,
										@100yen ,
										@50yen ,
										@10yen ,
										@5yen , 
										@1yen , 
										@etcyen ,				
										@Operator,@CalculationDate,@Operator,@CalculationDate)

	end

	Else 
    begin
		Update D_StoreCalculation
		set 
		    [10000yen] = @10000yen,
			[5000yen] = @5000yen,
			[2000yen] = @2000yen,
			[1000yen] = @1000yen,
			[500yen] = @500yen,
			[100yen] = @100yen,
			[50yen] = @50yen,
			[10yen] = @10yen,
			[5yen] = @5yen,
			[1yen] = @1yen,
			Etcyen = @etcyen,
			InsertOperator = @Operator,
			InsertDateTime = @CalculationDate,
			UpdateOperator = @Operator ,
			UpdateDateTime = @CalculationDate

			where StoreCD=@StoreCD
			and CalculationDate = @CalculationDate

	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

