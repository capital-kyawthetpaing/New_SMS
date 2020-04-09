 BEGIN TRY 
 Drop Procedure dbo.[WowmaDetail_Insert]
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
CREATE PROCEDURE [dbo].[WowmaDetail_Insert]
	-- Add the parameters for the stored procedure here
	@JuChuuXml as xml,
	@DetailXml as xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	begin

		declare @InsertDateTime datetime=getdate()

		Exec dbo.D_WowmaJuchuu_Insert @JuChuuXml,@InsertDateTime

		Exec dbo.D_WowmaJuchuuDetails_Insert @DetailXml,@InsertDateTime

	end
END

