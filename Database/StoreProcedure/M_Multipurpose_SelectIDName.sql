 BEGIN TRY 
 Drop Procedure dbo.[M_Multipurpose_SelectIDName]
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
CREATE PROCEDURE [dbo].[M_Multipurpose_SelectIDName]
	-- Add the parameters for the stored procedure here
	@ID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	[ID],
	IDName
	From M_MultiPorpose 
	Where ID = @ID
	Order by [Key]

	
END

