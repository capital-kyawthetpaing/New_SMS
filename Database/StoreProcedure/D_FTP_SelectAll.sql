 BEGIN TRY 
 Drop Procedure dbo.[D_FTP_SelectAll]
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
CREATE PROCEDURE [dbo].[D_FTP_SelectAll]
	-- Add the parameters for the stored procedure here
	@type int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CONVERT(varchar,FTPDateTime,111)+' '+SUBSTRING(CONVERT(varchar,FTPDateTime,108),1,5) AS ImportDateTime,
	IsNull(ftp.VendorCD,'') + ' ' +IsNull(fv.VendorName,'') as 'Vendor',
	FTPFile
	from D_FTP as ftp inner join F_Vendor(getdate()) as fv on ftp.VendorCD=fv.VendorCD
	where FTPType=@type
	order by ftp.UpdateDateTime desc
END

