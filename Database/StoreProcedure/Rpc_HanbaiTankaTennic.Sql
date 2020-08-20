 BEGIN TRY 
 Drop Procedure [dbo].[Rpc_HanbaiTankaTennic]
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
CREATE PROCEDURE [dbo].[Rpc_HanbaiTankaTennic]
@DisplayKBN as TinyInt,
@StartChangeDate as Date,
@EndChangeDate as Date,
@SKUCDFrom as Varchar(30),
@SKUCDTo as Varchar(30),
@BrandCD as varchar(20),
@SKUName as varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @DisplayKbn = 0
     begin
										select * into #tmp from (
										Select 
										fsprice.TankaCD,
										fs.AdminNO,
										MAX(fs.SKUCD) as SKUCD,
										MAX (fs.JanCD) as JANCD,
										MAX(fs.MainVendorCD) as MainVendorCD,
										MAX(fs.ChangeDate)as RevisionDate,
										fsprice.ChangeDate as StartDate,
										MAX(fsprice.TekiyouShuuryouDate) as EndDate,
										MAX(fs.SKUName) as ProductName,
										MAX(fsprice.PriceWithoutTax) as UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '0' THEN fsprice.SalePriceOutTax ELSE 0 END) as StandardSalesUnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '1' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank1UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '2' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank2UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '3' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank3UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '4' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank4UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '5' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank5UnitPrice,
										MAX(fsprice.Remarks) as Remarks
										from F_SKU(getdate()) fs
										Left outer join F_SKUPrice(getdate()) fsprice on fsprice.AdminNO=fs.AdminNO
										where fsprice.StoreCD ='0000'
										and fsprice.DeleteFLG=0
										and (@StartChangeDate IS NULL OR (fsprice.ChangeDate>=@StartChangeDate))
										and (@EndChangeDate IS NULL OR (fsprice.ChangeDate>=@EndChangeDate))
										and fs.ChangeDate <= fsprice.ChangeDate
										and (@SKUCDFrom IS NULL OR (fs.SKUCD>=@SKUCDFrom))
										and (@SKUCDTo IS NULL OR (fs.SKUCD>=@SKUCDTo))
										and  fs.BrandCD=@BrandCD
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										Group by fsprice.TankaCD,fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate) t
										
										
										Select 
										    tp.TankaCD,
										    '0000' as StoreCD,
											tp.AdminNo,
											tp.SKUCD,
											tp.JANCD,
											--tp.AdminNO,
											CONVERT(VARCHAR(10),tp.StartDate , 111) as StartDate,
											CONVERT(VARCHAR(10),tp.EndDate,111)as EndDate,
											tp.ProductName as SKUName,
											ISNULL (mj.PriceWithoutTax , mj2.PriceWithoutTax)  as CostUnitPrice,
											--(Case When mj.PriceWithoutTax = null then mj2.PriceWithoutTax else mj.PriceWithoutTax end) as CostUnitPrice,
											tp.UnitPrice,
											tp.StandardSalesUnitPrice,
											tp.Rank1UnitPrice,
											tp.Rank2UnitPrice,
											tp.Rank3UnitPrice,
											tp.Rank4UnitPrice,
											tp.Rank5UnitPrice,
											tp.Remarks
											from #Tmp tp  -- M_SKUPrice1
											--Left outer join F_SKUPrice(getdate()) mprice on mprice.AdminNO=tp.AdminNO
											Left outer join F_JANOrderPrice(getdate()) mj on mj.AdminNO=tp.AdminNO
																				And mj.VendorCD=tp.MainVendorCD
																				And mj.StoreCD='0000'
																				And mj.ChangeDate<=tp.RevisionDate
																				And mj.DeleteFlg=0
											Left outer join (select AdminNo, ChangeDate,StoreCD, Max(PriceWithoutTax) as PricewithoutTax from M_JanOrderPrice group by AdminNo,StoreCD, ChangeDate ) mj2 on mj.AdminNO=tp.AdminNO
																				And mj2.StoreCD='0000'
																				And mj2.ChangeDate<=tp.RevisionDate
																			   --where mj2.DeleteFlg = 0
																				And mj.DeleteFlg=0
																				
											order by
										    tp.SKUCD ASC,tp.StartDate,tp.EndDate DESC
 End
 

     Else if @DisplayKBN=1
     Begin
     select * into #Temp from (
										Select 
										fsprice.StoreCD,
										fsprice.TankaCD,
										fs.AdminNO,
										MAX(fs.SKUCD) as SKUCD,
										MAX (fs.JanCD) as JANCD,
										MAX(fs.MainVendorCD) as MainVendorCD,
										MAX(fs.ChangeDate)as RevisionDate,
										fsprice.ChangeDate as StartDate,
										MAX(fsprice.TekiyouShuuryouDate) as EndDate,
										MAX(fs.SKUName) as ProductName,
										MAX(fsprice.PriceWithoutTax) as UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '0' THEN fsprice.SalePriceOutTax ELSE 0 END) as StandardSalesUnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '1' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank1UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '2' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank2UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '3' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank3UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '4' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank4UnitPrice,
										MAX(CASE WHEN fsprice.TankaCD = '5' THEN fsprice.SalePriceOutTax ELSE 0 END) as Rank5UnitPrice,
										MAX(fsprice.Remarks) as Remarks
										from M_SKU fs
										Left outer join M_SKUPrice fsprice on fsprice.AdminNO=fs.AdminNO
										where fsprice.StoreCD ='0000'
										and fsprice.DeleteFLG ='0'
										and (@StartChangeDate IS NULL OR (fsprice.ChangeDate>=@StartChangeDate))
										and (@EndChangeDate IS NULL OR (fsprice.ChangeDate>=@EndChangeDate))
										and fs.ChangeDate <= fsprice.ChangeDate
										and (@SKUCDFrom IS NULL OR (fs.SKUCD>=@SKUCDFrom))
										and (@SKUCDTo IS NULL OR (fs.SKUCD>=@SKUCDTo))
										and  fs.BrandCD=@BrandCD
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										Group by fsprice.TankaCD,fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate, fsprice.StoreCD) t
										Select 
										    tp.TankaCD,
										    '0000' as StoreCD,
											tp.AdminNo,
											tp.SKUCD,
											tp.JANCD,
											--tp.AdminNO,
											CONVERT(VARCHAR(10),tp.StartDate , 111) as StartDate,
											CONVERT(VARCHAR(10),tp.EndDate,111)as EndDate,
											tp.ProductName as SKUName,
											ISNULL (mj.PriceWithoutTax , mj2.PriceWithoutTax)  as CostUnitPrice,
											--(Case When mj.PriceWithoutTax = null then mj2.PriceWithoutTax else mj.PriceWithoutTax end) as CostUnitPrice,
											tp.UnitPrice,
											tp.StandardSalesUnitPrice,
											tp.Rank1UnitPrice,
											tp.Rank2UnitPrice,
											tp.Rank3UnitPrice,
											tp.Rank4UnitPrice,
											tp.Rank5UnitPrice,
											tp.Remarks
											from #Temp tp  -- M_SKUPrice1
											--Left outer join F_SKUPrice(getdate()) mprice on mprice.AdminNO=tp.AdminNO
											Left outer join M_JANOrderPrice mj on mj.AdminNO=tp.AdminNO
																				And mj.VendorCD=tp.MainVendorCD
																				And mj.StoreCD='0000'
																				And mj.ChangeDate<=tp.RevisionDate
																				And mj.DeleteFlg=0
											Left outer join (select AdminNo, ChangeDate,StoreCD, Max(PriceWithoutTax) as PricewithoutTax from M_JanOrderPrice group by AdminNo,StoreCD, ChangeDate ) mj2 on mj.AdminNO=tp.AdminNO
																				And mj2.StoreCD='0000'
																				And mj2.ChangeDate<=tp.RevisionDate
																			   --where mj2.DeleteFlg = 0
																				And mj.DeleteFlg=0
											order by
										    tp.SKUCD ASC,tp.StartDate,tp.EndDate DESC
End	
drop table #tmp
drop table #Temp	
				
END

GO

