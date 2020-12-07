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
@Mode as tinyint,
@DisplayKBN as TinyInt,
@StartChangeDate as Date,
@EndChangeDate as Date,
@SKUCDFrom as Varchar(30),
@SKUCDTo as Varchar(30),
@BrandCD as varchar(20),
@SKUName as varchar(30),
@SegmentCD as varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @DisplayKbn = 0
     begin
										if @Mode = 1   -- select New Mode
									Begin
										select * into #tmp from (
										Select 
										--fsprice.TankaCD,
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
										and (@EndChangeDate IS NULL OR (fsprice.ChangeDate<=@EndChangeDate))
										and fs.ChangeDate <= fsprice.ChangeDate
										and (@SKUCDFrom IS NULL OR (fs.SKUCD>=@SKUCDFrom))
										and (@SKUCDTo IS NULL OR (fs.SKUCD<=@SKUCDTo))
										and (@BrandCD is null or (fs.BrandCD=@BrandCD))
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and (@SegmentCD is null or (fs.ExhibitionSegmentCD=@SegmentCD))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										and
										NOT EXISTS(SELECT 1 FROM F_SKUPrice(getdate()) A 
										WHERE A.SKUCD = fs.SKUCD 
										AND A.StoreCD = '0000'
										AND A.DeleteFlg = 0)

										Group by fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate) t
										Select 
										    --tp.TankaCD,
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
											CAST(tp.UnitPrice AS INT) AS UnitPrice,
											CAST(tp.StandardSalesUnitPrice AS INT) AS StandardSalesUnitPrice,
											CAST(tp.Rank1UnitPrice AS INT) AS Rank1UnitPrice,
											CAST(tp.Rank2UnitPrice AS INT) AS Rank2UnitPrice, 
											CAST(tp.Rank3UnitPrice AS INT) AS Rank3UnitPrice,
											CAST(tp.Rank4UnitPrice AS INT) AS Rank4UnitPrice,
											CAST(tp.Rank5UnitPrice AS INT) AS Rank5UnitPrice,
											tp.Remarks
											from #tmp tp  -- M_SKUPrice1
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
											drop table #tmp

											End
									else   --2/3/4 Update Mode/Delete/Show Mode
									 
									Begin
										select * into #tmp_U from (
										Select 
										--fsprice.TankaCD,
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
										and (@EndChangeDate IS NULL OR (fsprice.ChangeDate<=@EndChangeDate))
										and fs.ChangeDate <= fsprice.ChangeDate
										and (@SKUCDFrom IS NULL OR (fs.SKUCD>=@SKUCDFrom))
										and (@SKUCDTo IS NULL OR (fs.SKUCD<=@SKUCDTo))
										and (@BrandCD is null or (fs.BrandCD=@BrandCD))
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and (@SegmentCD is null or (fs.ExhibitionSegmentCD=@SegmentCD))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										Group by fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate) t
										
										
										Select 
										    --tp.TankaCD,
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
											CAST(tp.UnitPrice as INT)AS UnitPrice,
											CAST(tp.StandardSalesUnitPrice AS INT) AS StandardSalesUnitPrice,
											CAST(tp.Rank1UnitPrice AS INT) AS Rank1UnitPrice,
											CAST(tp.Rank2UnitPrice AS INT) AS Rank2UnitPrice, 
											CAST(tp.Rank3UnitPrice AS INT) AS Rank3UnitPrice,
											CAST(tp.Rank4UnitPrice AS INT) AS Rank4UnitPrice,
											CAST(tp.Rank5UnitPrice AS INT) AS Rank5UnitPrice,
											tp.Remarks
											from #tmp_U tp  -- M_SKUPrice1
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
											drop table #tmp_U

									End
 End
 

     Else if @DisplayKBN=1
     Begin
	 if @Mode=1
	 begin
     select * into #Tp from (
										Select 
										--fsprice.StoreCD,
										--fsprice.TankaCD,
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
										and (@BrandCD is null or (fs.BrandCD=@BrandCD))
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and (@SegmentCD is null or (fs.ExhibitionSegmentCD=@SegmentCD))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										and
										NOT EXISTS(SELECT 1 FROM M_SKUPrice A 
										WHERE A.SKUCD = fs.SKUCD 
										AND A.StoreCD = '0000'
										AND A.DeleteFlg = 0)
										Group by fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate) t

										Select 
										    --tp.TankaCD,
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
											CAST(tp.UnitPrice AS INT)AS UnitPrice,
											CAST(tp.StandardSalesUnitPrice AS INT) AS StandardSalesUnitPrice,
											CAST(tp.Rank1UnitPrice AS INT) AS Rank1UnitPrice,
											CAST(tp.Rank2UnitPrice AS INT) AS Rank2UnitPrice, 
											CAST(tp.Rank3UnitPrice AS INT) AS Rank3UnitPrice,
											CAST(tp.Rank4UnitPrice AS INT) AS Rank4UnitPrice,
											CAST(tp.Rank5UnitPrice AS INT) AS Rank5UnitPrice,
											tp.Remarks
											from #Tp tp  -- M_SKUPrice1
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
											drop table #Tp
End	
else
									
									Begin
										select * into #tmp_U2 from (
										Select 
										--fsprice.TankaCD,
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
										and fsprice.DeleteFLG=0
										and (@StartChangeDate IS NULL OR (fsprice.ChangeDate>=@StartChangeDate))
										and (@EndChangeDate IS NULL OR (fsprice.ChangeDate>=@EndChangeDate))
										and fs.ChangeDate <= fsprice.ChangeDate
										and (@SKUCDFrom IS NULL OR (fs.SKUCD>=@SKUCDFrom))
										and (@SKUCDTo IS NULL OR (fs.SKUCD>=@SKUCDTo))
										and (@BrandCD is null or (fs.BrandCD=@BrandCD))
										and (@SKUName IS NULL OR   (fs.SKUName LIKE '%'+@SKUName+'%'))
										and (@SegmentCD is null or (fs.ExhibitionSegmentCD=@SegmentCD))
										and fs.VirtualFlg=0
										and fs.PresentKBN=0
										and fs.SampleKBN=0
										and fs.DeleteFLG=0
										Group by fs.AdminNO,fsprice.AdminNO,fsprice.ChangeDate) t
										
										
										Select 
										    --tp.TankaCD,
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
											CAST(tp.UnitPrice as INT)AS UnitPrice,
											CAST(tp.StandardSalesUnitPrice AS INT) AS StandardSalesUnitPrice,
											CAST(tp.Rank1UnitPrice AS INT) AS Rank1UnitPrice,
											CAST(tp.Rank2UnitPrice AS INT) AS Rank2UnitPrice, 
											CAST(tp.Rank3UnitPrice AS INT) AS Rank3UnitPrice,
											CAST(tp.Rank4UnitPrice AS INT) AS Rank4UnitPrice,
											CAST(tp.Rank5UnitPrice AS INT) AS Rank5UnitPrice,
											tp.Remarks
											from #tmp_U2 tp  -- M_SKUPrice1
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
											drop table #tmp_U2

									End
    
--drop table #tmp
--drop table #Temp	
				
END
END


GO

