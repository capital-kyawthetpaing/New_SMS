 BEGIN TRY 
 Drop Procedure dbo.[SettingDefault]
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
Create PROCEDURE [dbo].[SettingDefault]
	-- Add the parameters for the stored procedure here
	
	@StaffCD as varchar(10),
	@MenuCD as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	  update M_setting set DefaultKBN= 1 where StaffCD = @StaffCD and MenuKBN=@MenuCD

END
