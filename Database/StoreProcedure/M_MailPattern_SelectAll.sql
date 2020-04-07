 BEGIN TRY 
 Drop Procedure dbo.[M_MailPattern_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [M_MailPattern_SelectAll]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_MailPattern_SelectAll]
	-- Add the parameters for the stored procedure here
	@MailPatternCD varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.MailPatternCD
    	  ,MS.MailPatternName 
          ,MS.MailType               
          ,MS.MailKBN                
          ,MS.MailPriority           
          ,MS.MailSubject            
          ,MS.MailText               
    FROM M_MailPattern MS
    
    ORDER BY MS.MailPatternCD
    ;
END



