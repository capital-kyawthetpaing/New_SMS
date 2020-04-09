 BEGIN TRY 
 Drop Procedure dbo.[Amazon_DRequest]
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
CREATE PROCEDURE [dbo].[Amazon_DRequest]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @seq as int ;
	set @seq = (select Max(InportSEQ) from D_AmazonRequest where APIKey =11)

	select * from D_AmazonRequest where  InportSEQ=@seq
END

