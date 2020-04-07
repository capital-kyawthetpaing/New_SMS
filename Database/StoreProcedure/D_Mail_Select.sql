 BEGIN TRY 
 Drop Procedure dbo.[D_Mail_Select]
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
CREATE PROCEDURE [dbo].[D_Mail_Select]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
SELECT  mms.SenderAddress,mms.SenderServer,mms.Account,mms.Password--mailserver
		--Mailの内容
		,dm.MailSubject
		,dm.MailContent
		,dma.AddressKBN
		,dma.Address
		--添付File
		,dmf.CreateServer
		,dmf.CreateFolder
		,dmf.FileName
		 FROM  D_Mail dm
				INNER  JOIN  D_MailAddress dma
				ON  dma.MailCounter=dma.MailCounter
				INNER  JOIN  M_MailServer mms
				ON dm.SenderKBN=mms.SenderKBN and dm.SenderCD=mms.SenderCD 
				LEFT  OUTER  JOIN  D_MailFile dmf
				ON dm.MailCounter=dmf.MailCounter
				WHERE dm.ContactKBN=1
				AND  dm.SendedDateTime is null
		ORDER BY dm.MailPriority,InsertDateTime
END

