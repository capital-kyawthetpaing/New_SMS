 BEGIN TRY 
 Drop Procedure dbo.[M_Renban_Delete]
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
CREATE PROCEDURE [dbo].[M_Renban_Delete]
	-- Add the parameters for the stored procedure here
	@PrefixValue varchar(3),
	@Continuous int,
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
	Delete From M_PrefixNumber
			Where Prefix = @PrefixValue

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END


