 BEGIN TRY 
 Drop Procedure dbo.[M_Renban_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Renban_Insert_Update]
	-- Add the parameters for the stored procedure here
	@PrefixValue varchar(3),
	@Continuous int,
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
					insert into M_PrefixNumber
					(Prefix,
					Prefix2,
					SeqNumber,
					InsertOperator,
				    InsertDateTime,
					UpdateOperator,
	                UpdateDateTime
					)
					values
				   (
				   @PrefixValue,
				   @PrefixValue,
				   @Continuous,
				   @Operator,
				   @currentDate,
				   @Operator,
				   @currentDate
				   )
	 end

			else if @Mode = 2--update mode
				begin
					update M_PrefixNumber
					set 
					    SeqNumber=@Continuous,
					    UpdateOperator=@Operator,
						UpdateDateTime=@currentDate

						where Prefix=@PrefixValue
		end
		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

	


