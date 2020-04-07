 BEGIN TRY 
 Drop Procedure dbo.[M_Prefix_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Prefix_Insert_Update] 
	-- Add the parameters for the stored procedure here
	@StoreCD varchar(6),
	@SeqKBN tinyint,
	@ChangeDate date,
	@Prefix varchar(5),
	@Mode as tinyint,-- 1 - insert, 2 - update

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
	declare @currentDate as datetime = getdate();

		if @Mode = 1--insert mode
				begin
					--M_Prefix insert
					INSERT INTO M_Prefix 
					( 
						StoreCD,
						SeqKBN,
						ChangeDate,
						Prefix,
						InsertOperator,
						InsertDateTime,
						UpdateOperator,
						UpdateDateTime 
					)
					VALUES
					(
						@StoreCD,
						@SeqKBN,
						@ChangeDate,
						@Prefix,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)

				end
			else if @Mode = 2--update mode
				begin
					UPDATE M_Prefix
					SET 
						Prefix = @Prefix,
						UpdateOperator = @Operator,
						UpdateDateTime = @currentDate
					WHERE StoreCD = @StoreCD
					AND SeqKBN = @SeqKBN
					AND ChangeDate = @ChangeDate
				end

		--log insert
		exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END

