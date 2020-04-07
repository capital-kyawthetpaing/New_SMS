 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPorpose_GaisyaSelect]
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
CREATE PROCEDURE [dbo].[M_MultiPorpose_GaisyaSelect]
	-- Add the parameters for the stored procedure here
	@ID as int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Key],[Key]+':'+Char1 as Char1
	FROM M_MultiPorpose
	WHERE ID = @ID
	Order by [Key]
END

