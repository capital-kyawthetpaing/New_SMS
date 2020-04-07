 BEGIN TRY 
 Drop Procedure dbo.[Check_RegisteredMenu]
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
CREATE PROCEDURE [dbo].[Check_RegisteredMenu] 
	-- Add the parameters for the stored procedure here
	@StaffCD as varchar(15)  
	 as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	 



           select 1 from M_Menu where MenuID = 
		   (select top 1 MenuCD from M_Staff where StaffCD = @StaffCD) 
           and DeleteFlg = 0
 

END

