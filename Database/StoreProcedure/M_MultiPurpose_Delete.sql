 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_Delete]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_Delete]
	-- Add the parameters for the stored procedure here
	@ID as Int,
	@Key as varchar(50),
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
			Delete From M_MultiPorpose 
			Where ID = @ID AND [Key] = @Key
			
			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
		
END

