 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_IDSelect]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_IDSelect]
	-- Add the parameters for the stored procedure here
	@ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @ID = 0 
	   begin 
			Select Char1 From M_MultiPorpose mmp
			Where mmp.ID = 000 
	   end
	else
	   begin 
			Select Char1 From M_MultiPorpose mmp
			Where mmp.ID = 000 AND mmp.[Key]  = CONVERT(varchar, @ID,111) 
	   end
		 
END

