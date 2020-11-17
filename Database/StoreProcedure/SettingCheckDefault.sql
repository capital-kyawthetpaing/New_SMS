 
 BEGIN TRY 
 Drop Procedure dbo.[SettingCheckDefault]
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
Create PROCEDURE [dbo].[SettingCheckDefault]
	-- Add the parameters for the stored procedure here
	@MenuCD as tinyint,
	@StaffCD as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top 1 * from M_Setting where MenuKBN = @MenuCD   and StaffCD = @StaffCD
END
