 BEGIN TRY 
 Drop Procedure dbo.[D_TenzikaiJuchuu_SearchData]
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
CREATE PROCEDURE [dbo].[D_TenzikaiJuchuu_SearchData] 
	-- Add the parameters for the stored procedure here
	@JuchuuDateFrom as date,
	@JuchuuDateTo as date,
	@Year as varchar(6),
	@Season as varchar(6),
	@StaffCD as varchar(10),
	@CustomerCD as varchar(13),
	@VendorCD as varchar(13),
	@ProductName as varchar(50),  
	@ItemCD as varchar(30),
	@SKUCD as varchar(40),
	@JanCD as varchar(13)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--Select
	--dtj.TenzikaiJuchuuNO,
	--MAX(dtj.JuchuuDate) ,
	--(select MAX(fv.VendorName) from F_Vendor(dtj.JuchuuDate) fv where fv.VendorCD=@VendorCD) as VendorName,
	--MAX(dtj.LastYearTerm),
	--MAX(dtj.LastSeason),
	--(select case when fc.VariousFLG =1 then Max(dtj.CustomerName)
	--when fc.VariousFLG=0 then Max(fc.CustomerName)
	--end
	--from F_Customer(dtj.JuchuuDate) fc where fc.CustomerCD=dtj.CustomerCD and fc.DeleteFlg=0) as CustomerName

	--From D_TenzikaiJuchuu dtj
	--Left Outer Join D_TenzikaiJuchuuDetails dtjd on dtjd.TenzikaiJuchuuNO = dtj.TenzikaiJuchuuNO and dtjd.DeleteDateTime = null
	--Left Outer Join F_SKU(getdate()) fsku on fsku.AdminNO = dtjd.AdminNO
	--Where dtj.DeleteDateTime = null and
	--dtj.JuchuuDate >= @JuchuuDateFrom and
	--dtj.JuchuuDate <= @JuchuuDateTo and
	--dtj.LastYearTerm = @Year and
	--dtj.LastSeason = @Season and
	--dtj.StaffCD = @StaffCD and
	--dtj.VendorCD = @VendorCD and
	--dtj.CustomerCD = @CustomerCD and
	--fsku.KanaName = @ProductName and
	--(@ItemCD is null or(fsku.ITemCD in (select * from SplitString(@ItemCD,',')))) and
	--(@SKUCD is null or (fsku.SKUCD in (select * from SplitString(@SKUCD,','))))and
	--(@JanCD is null or(fsku.JanCD in (select * from SplitString(@JanCD,','))))
	--Group by dtj.TenzikaiJuchuuNO,VendorName,CustomerName
	--Order by dtj.TenzikaiJuchuuNO asc						

	Select
	dtj.TenzikaiJuchuuNO,
	MAX(dtj.JuchuuDate) as JuchuuDate,
	(select fv.VendorName from F_Vendor(max(dtj.JuchuuDate)) fv where fv.VendorCD=@VendorCD) as VendorName,
	MAX(dtj.LastYearTerm) as LastYearTerm,
	MAX(dtj.LastSeason)as LastSeason ,
	(select case when fc.VariousFLG =1 then Max(dtj.CustomerName)
	when fc.VariousFLG=0 then fc.CustomerName
	end
	from F_Customer(max(dtj.JuchuuDate)) fc where fc.CustomerCD=max(dtj.CustomerCD) and fc.DeleteFlg=0) as CustomerName
	
	From D_TenzikaiJuchuu dtj
	Left Outer Join D_TenzikaiJuchuuDetails dtjd on dtjd.TenzikaiJuchuuNO = dtj.TenzikaiJuchuuNO and dtjd.DeleteDateTime is null
	Left Outer Join F_SKU(getdate()) fsku on fsku.AdminNO = dtjd.AdminNO
	Where dtj.DeleteDateTime is null and
	(@JuchuuDateFrom is null or dtj.JuchuuDate >= @JuchuuDateFrom) and
	(@JuchuuDateTo is null or  dtj.JuchuuDate <= @JuchuuDateTo )and
	(@Year is null or dtj.LastYearTerm = @Year) and
	(@Season is null or dtj.LastSeason = @Season) and
	(@StaffCD is null or dtj.StaffCD = @StaffCD) and
	(@VendorCD is null or dtj.VendorCD = @VendorCD) and
	(@CustomerCD is null or dtj.CustomerCD = @CustomerCD) and
	(@ProductName is null or fsku.KanaName = @ProductName) and
	(@ItemCD is null or(fsku.ITemCD in (select * from SplitString(@ItemCD,',')))) and
	(@SKUCD is null or (fsku.SKUCD in (select * from SplitString(@SKUCD,','))))and
	(@JanCD is null or(fsku.JanCD in (select * from SplitString(@JanCD,','))))
	Group by dtj.TenzikaiJuchuuNO
	Order by dtj.TenzikaiJuchuuNO asc
	
END

GO
