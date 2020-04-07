 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_IDSearch]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_IDSearch]
	-- Add the parameters for the stored procedure here
	@IDFrom int,
	@IDTo int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	[Key],
	Char1
	From M_MultiPorpose 
	Where ID = 000 AND
	(@IDFrom is null or [Key] >= CONVERT(varchar, @IDFrom,111)) AND
	(@IDTo is null or [Key] <= CONVERT(varchar, @IDTo,111))
	Order by [Key]

	
END

