 BEGIN TRY 
 Drop Procedure dbo.[Amazon_Select_AllOrderByLastRireki]
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
CREATE PROCEDURE [dbo].[Amazon_Select_AllOrderByLastRireki]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

	select * from D_AmazonList where InportSEQ = (select Max(InportSEQ) from D_AmazonList ) Order by D_AmazonList.InportSEQRows 


END

