 BEGIN TRY 
 Drop Procedure dbo.[M_VendorFTP_Select]
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
CREATE PROCEDURE [dbo].[M_VendorFTP_Select] 
	-- Add the parameters for the stored procedure here
	@VendorCD varchar(12),
	@ChangeDate date	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1
	   [VendorCD]
      ,[ChangeDate]
      ,[FTPServer]
      ,[FTPFolder]
      ,[CreateServer]
      ,[CreateFolder]
      ,[FileName]
      ,[TitleFLG]
      ,[LoginID]
      ,[Password]
      ,[MailTitle]
      ,[MailPatternCD]
      ,[SenderMailAddress]
      ,[MailPriority]
      ,[CapitalCD]
      ,[CapitalName]
      ,[OrderCD]
      ,[OrderName]
      ,[SalesCD]
      ,[SalesName]
      ,[DestinationCD]
      ,[DestinationName]
	From M_VendorFTP mv 
	Where (@VendorCD is null or(mv.VendorCD = @VendorCD))
      and (@ChangeDate is null or (mv.ChangeDate = @ChangeDate))
    Order by mv.ChangeDate DESC;
END


