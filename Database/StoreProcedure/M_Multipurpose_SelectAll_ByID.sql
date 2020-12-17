BEGIN TRY 
 Drop Procedure [dbo].[M_Multipurpose_SelectAll_ByID]
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
Create PROCEDURE [dbo].[M_Multipurpose_SelectAll_ByID]
	-- Add the parameters for the stored procedure here
	@Type int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if(@Type = 1)
	begin
	SELECT Char1 from M_MultiPorpose where ID='226'
	end 
	else 
	SELECT Char1 from M_MultiPorpose where ID='201'
END
GO
