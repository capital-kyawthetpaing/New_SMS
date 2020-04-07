 BEGIN TRY 
 Drop Procedure dbo.[Get_StoreName]
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
CREATE PROCEDURE [dbo].[Get_StoreName]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @todayDate as datetime = GETDATE();
    -- Insert statements for procedure here

	SELECT StoreName 
	FROM M_Store
	WHERE StoreCD =
	(SELECT StoreCD FROM M_Staff
	WHERE StaffCD = @StaffCD
	AND ChangeDate <= @todayDate)
	AND ChangeDate <= @todayDate
END

