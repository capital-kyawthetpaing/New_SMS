 BEGIN TRY 
 Drop Procedure dbo.[D_FTP_Insert]
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
CREATE PROCEDURE [dbo].[D_FTP_Insert] 
	-- Add the parameters for the stored procedure here
	@FTPType tinyint,
	@VendorCD varchar(13),
	@FTPFile varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Declare
	@Datetime datetime = getdate()

	Insert into D_FTP(
	FTPType,
	FTPDateTime,
	VendorCD,
	FTPFile,
	UpdateDateTime
	)
	values(
	@FTPType,
	@Datetime,
	@VendorCD,
	@FTPFile,
	@Datetime
	)


END

