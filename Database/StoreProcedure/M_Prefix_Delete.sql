 BEGIN TRY 
 Drop Procedure dbo.[M_Prefix_Delete]
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
CREATE PROCEDURE [dbo].[M_Prefix_Delete]
	-- Add the parameters for the stored procedure here
	@StoreCD VARCHAR(4),
	@SeqKBN TINYINT,
	@ChangeDate date,
	@Operator VARCHAR(10),
	@Program VARCHAR(30),
	@PC VARCHAR(30),
	@OperateMode VARCHAR(10),
	@KeyItem VARCHAR(100)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM M_Prefix
		WHERE StoreCD = @StoreCD
		AND SeqKBN = @SeqKBN
		AND ChangeDate = @ChangeDate
			  	
	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

