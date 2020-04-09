 BEGIN TRY 
 Drop Procedure dbo.[D_APIRireki_Insert_Update]
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
CREATE PROCEDURE [dbo].[D_APIRireki_Insert_Update]
	-- Add the parameters for the stored procedure here
	@Num1 int,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
   
	declare @currentDate as datetime = getdate();
		update M_MultiPorpose
		set 
		Num1=@Num1,
		InsertOperator=@Operator,
		UpdateDateTime=@currentDate
		where ID=302 AND [Key]=1
		exec dbo.L_Log_Insert  @Operator,@Program,@PC,@OperateMode,@KeyItem

END

