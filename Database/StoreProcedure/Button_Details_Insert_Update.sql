 BEGIN TRY 
 Drop Procedure dbo.[Button_Details_Insert_Update]
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
CREATE PROCEDURE[dbo].[Button_Details_Insert_Update]
	-- Add the parameters for the stored procedure here
	@GroupDetailXML as xml,
	@GroupXML as XML,
	@StoreCD as varchar(4),
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;
    -- Insert statements for procedure here
	begin

	Declare
	@InsertDateTime dateTime=getdate()
	
	EXEC dbo.M_StoreButton_Insert_Update @GroupXML,@StoreCD,@InsertDateTime,@Operator,@Program,@PC

	EXEC dbo.M_StoreButtonDetails_Insert_Update  @GroupDetailXML,@StoreCD,@InsertDateTime,@Operator,@Program,@PC

	--EXEC dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

	end
END


