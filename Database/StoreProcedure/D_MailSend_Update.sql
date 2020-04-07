 BEGIN TRY 
 Drop Procedure dbo.[D_MailSend_Update]
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
CREATE PROCEDURE[dbo].[D_MailSend_Update]
	-- Add the parameters for the stored procedure here	
	@MailCount as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	declare @currdate as datetime = getdate();
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		UPDATE D_Mail
		SET		MailDateTime=@currdate
				,UpdateOperator='MailSend'
				,UpdateDateTime=@currdate
		WHERE MailCounter=@MailCount		
END

