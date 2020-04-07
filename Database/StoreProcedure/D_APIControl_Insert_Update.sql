 BEGIN TRY 
 Drop Procedure dbo.[D_APIControl_Insert_Update]
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
CREATE PROCEDURE [dbo].[D_APIControl_Insert_Update]
	-- Add the parameters for the stored procedure here
	@APIKey int,
	@Operator varchar(10),
	@State int
AS
BEGIN
   
	declare @currentDate as datetime = getdate();
		update D_APIControl
		set 
		State=@State,
		UpdateOperator=@Operator,
		UpdateDateTime=@currentDate
		where APIKey=@APIKey
		

END


