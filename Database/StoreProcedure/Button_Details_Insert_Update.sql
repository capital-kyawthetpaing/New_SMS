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
@MasterKBN varchar(12),
--@mode as tinyint,

@Operator varchar(10),
 @Program as varchar(30),
 @PC as varchar(30)
 --@OperateMode as varchar(10),
 --@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;
    -- Insert statements for procedure here
	begin
	
	EXEC dbo.M_StoreButton_Insert_Update @GroupXML,@StoreCD,@MasterKBN,@Operator,@Program,@PC

	EXEC dbo.M_StoreButtonDetails_Insert_Update  @GroupDetailXML,@StoreCD,@MasterKBN,@Operator,@Program,@PC

	--EXEC dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

	end
END

