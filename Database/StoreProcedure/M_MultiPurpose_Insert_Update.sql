 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_Insert_Update]
	-- Add the parameters for the stored procedure here
	@ID int,
	@Key varchar(50),
	--@IDName varchar(50),
	@Text1 varchar(100),
	@Text2 varchar(100),
	@Text3 varchar(100),
	@Text4 varchar(100),
	@Text5 varchar(100),
	@Digital1 int,
	@Digital2 int,
	@Digital3 int,
	@Digital4 int,
	@Digital5 int,
	@Day1 datetime,
	@Day2 datetime,
	@Day3 datetime,
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
					insert into M_MultiPorpose
					(
						ID,
						[Key],
						--IDName,
						Char1,
						Char2,
						Char3,
						Char4,
						Char5,
						Num1,
						Num2,
						Num3,
						Num4,
						Num5,
						Date1,
						Date2,
						Date3,
						InsertOperator,
						InsertDateTime,
						UpdateOperator,
						UpdateDateTime
					)
					values
					(
						@ID,
						@Key,
						--@IDName,
						@Text1,
						@Text2,
						@Text3,
						@Text4,
						@Text5,
						@Digital1,
						@Digital2,
						@Digital3,
						@Digital4,
						@Digital5,
						@Day1,
						@Day2,
						@Day3,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)
				end
			else if @Mode = 2--update mode
				begin
					update M_MultiPorpose
					set
						ID=@ID,
						[Key]=@Key ,	
						Char1=@Text1,
						Char2=@Text2,
						Char3=@Text3,
						Char4=@Text4,
						Char5=@Text5,
						Num1=@Digital1,
						Num2=@Digital2,
						Num3=@Digital3,
						Num4=@Digital4,
						Num5=@Digital5,
						Date1=@Day1,
						Date2=@Day2,
						Date3=@Day3,					
						UpdateOperator= @Operator,
						UpdateDateTime = @currentDate

					where ID=@ID
						and [Key]= @Key			
				end

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

