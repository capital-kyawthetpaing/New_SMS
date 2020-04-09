 BEGIN TRY 
 Drop Procedure dbo.[M_Staff_LoginSelect]
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
CREATE PROCEDURE [dbo].[M_Staff_LoginSelect]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10),
	@Password varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select top 1 * from M_Staff 
	where StaffCD = @StaffCD
	and [Password] = @Password
	order by ChangeDate desc
END


