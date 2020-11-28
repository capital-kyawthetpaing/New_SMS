BEGIN TRY 
 Drop Procedure [dbo].[M_TenzikaiShouhin_SelectByTenziName]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[M_TenzikaiShouhin_SelectByTenziName]
	-- Add the parameters for the stored procedure here
	@tenzikainame as varchar(80)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		SELECT 
		Distinct 
		 fv.VendorCD,
		 fv.VendorName,
		 mt.LastYearTerm,
		 mt.LastSeason
		
		 from M_TenzikaiShouhin as mt
		 left outer join F_Vendor(GETDATE()) as fv on fv.VendorCD=mt.VendorCD and mt.DeleteFlg=0
		 where TenzikaiName=@tenzikainame

END
GO
