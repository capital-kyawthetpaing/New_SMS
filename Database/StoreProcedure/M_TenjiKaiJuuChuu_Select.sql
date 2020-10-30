 BEGIN TRY 
 Drop Procedure [dbo].[M_TenjiKaiJuuChuu_Select]
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
Create PROCEDURE [dbo].[M_TenjiKaiJuuChuu_Select]
	-- Add the parameters for the stored procedure here

					
					@xml as xml ,
					@CustomerCD as varchar (20),
					@JuuChuuBi as varchar(10),
					@LastyearTerm as varchar(10),
					@LastSeason as varchar(50),
					@VendorCD as varchar(20),
					@SoukoName as varchar(50),
					@DesiredDate1 as date,
					@DesiredDate2 as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--declare
	--										@xml as xml ,
	--													@CustomerCD as varchar (20)='0000000000007',
	--													@JuuChuuBi as varchar(10)='2020-09-06',
	--													@LastyearTerm as varchar(10) ='2020',
	--													@LastSeason as varchar(50)='Q2',
	--													@VendorCD as varchar(20)='100005',
	--													@SoukoName as varchar(50) ='SouKoName',
	--													@DesiredDate1 as varchar(10)='2020-09-30',
	--													@DesiredDate2 as varchar(10)='2020-11-20'
																		declare @DocHandle int
														exec sp_xml_preparedocument @DocHandle output, @Xml
														select * into #temp FROM
														OPENXML (@DocHandle, '/NewDataSet/test',2)
														with
														(
														仮JANCD varchar(50),
														SKUCD varchar(50),
														商品名 varchar(50),
														カラー名 varchar(8),
														サイズ名 varchar(8),
														販売予定日 varchar(20),
														即納数 varchar(20),
														希望日1 varchar(20),
														希望日2 varchar(20)
														)
														
														--@xml as xml ,
														--@CustomerCD as varchar (20)='0000000000007',
														--@JuuChuuBi as varchar(10)='2020-09-06',
														--@LastyearTerm as varchar(10) ='2020',
														--@LastSeason as varchar(50)='Q2',
														--@VendorCD as varchar(20)='100005',
														--@SoukoName as varchar(50) ='SouKoName',
														--@DesiredDate1 as varchar(10)='2020-09-30',
														--@DesiredDate2 as varchar(10)='2020-11-20'
														EXEc sp_xml_removedocument @DocHandle;

														select MAX(mt.JanCD) as JanCD ,
														Max(mt.SKUCD) as SKUCD,
														MAx(mt.SKUName) as SKUNAME,
														Max(mt.Siiretanka) as SiireTanka,
														Max(Char1) as Char1,
														Max(mt.SalePriceOutTax) as SalePriceOutTax,
														
														Case When Max(mt.TaxRateFLG) = 0 then '非税' else '税抜' end as TaxNotation,
														Case when max(mt.taxRateFLG) = 0 then '0%' when Max(mt.TaxRateFLG)= 1 then
														Cast(Max(ms.TaxRate1) as varchar(10))+'%' when Max(mt.TaxRateFLG)=2 then
														Cast(Max(ms.TaxRate2) as varchar(10))+'%' end as TaxRate,
														Case when max(mt.taxRateFLG) = 0 then '0' when Max(mt.TaxRateFLG)= 1 then
														Cast((Max(ms.TaxRate1) +100)/100 as varchar(10)) when Max(mt.TaxRateFLG)=2 then
														Cast((Max(ms.TaxRate2)+100)/100 as varchar(10)) end as TaxCalculation,
														Max(ms.FractionKBN) as FractionKBN,
														Max(mt.TaxRateFLG) as TaxRateFLG,
														Max(mt.TaniCD) as TaniCD
														into #temp1
														from M_TenzikaiShouhin mt
														
														left outer join F_Customer(getdate()) mc on mc.CustomerCD= @CustomerCD and mc.ChangeDate <= @JuuChuuBi
														left outer join F_vendor(getdate()) mv on mv.vendorCD = mt.vendorCD and mv.ChangeDate <= @JuuChuuBi
														left outer join M_Multiporpose mm on mm.ID= '201' and mm.[Key] = mt.TaniCD
														left outer join M_SalesTax ms on ms.ChangeDate <= @JuuChuuBi
														where
														mt.TankaCD = mc.TankaCD
														and mt.LastyearTerm = @LastyearTerm
														and mt.LastSeason = @LastSeason
														and mt.VendorCD = @VendorCD
														and mt.DeleteFlg = '0'
														and mc.DeleteFlg='0'
														and mv.DeleteFlg='0'
														group by mt.ExhibitionCommonCd

														--Gamen-2
														select
														t.仮JANCD as _1JanCD,
														t.SKUCD as _2SKUCD,
														t.商品名 as _3SKUName,
														--t1.JanCD as _1JanCD,
														--t1.SKUCD as _2SKUCD,
														--t1.SKUName as _3SKUName,
														null as _4ColorNo ,
														t.カラー名 as _5ColorName,
														null as _6SizeNo,
														t.サイズ名 as _7SizeName,
													--	t.販売予定日 as _8ShuukaYoteiBi,
														Convert(varchar,  CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End, 111) as _8ShuukaYoteiBi,
														@SoukoName as _9SoukoName,
														format(t1.SiireTanka, 'N0')  as _10SiireTanka, 
														Convert ( varchar,CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End, 111) as _11NyuukaYoteiHyou ,
														format(Cast(t.即納数 as int), 'N0')  as _12SoukunoSu,
														t1.TaniCD as _13TaniCD,
														format(t1.SalePriceOutTax, 'N0')  as _14SalePriceOutTax,
														format(Round(t1.SalePriceOutTax * t.即納数,0), 'N0') as _15OrderExcTax,
														format(Round(t1.SalePriceOutTax *t.即納数* t1.TaxCalculation,0), 'N0')  as _16OrderIncTax,
														format(Round((t1.SalePriceOutTax - t1.SiireTanka)* t.[即納数],0), 'N0')  as _17TotalProfit,
														t1.Taxnotation as _18Taxnotation ,
														t1.TaxRate as _19TaxRate,
														null as _20ExternaRemarks ,
														null as _21InternalRemarks,
														null as _22kobetsuHanbai,
														case when t1.JanCD is null then 0 else 1 end as _23TorokuFlag,
														t1.janCD as _24TaniCD,
														t1.TaxRateFlg as _25TaxRateFlg
													    into #gamen2
														from #temp t
														left outer join #temp1 t1 on t.[仮JANCD] = t1.JanCD
														where isnull(t.[即納数],0) <>0
														order by _1JANCD asc,
														_2SKUCD asc
														--select Cast(format(, 'N0') as varchar(12))

													    -- gamen-3  
													   	select
																t.仮JANCD as _1JanCD,
														t.SKUCD as _2SKUCD,
														t.商品名 as _3SKUName,
														--t1.JanCD as _1JanCD,
														--t1.SKUCD as _2SKUCD,
														--t1.SKUName as _3SKUName,
														null as _4ColorNo ,
														t.カラー名 as _5ColorName,
														null as _6SizeNo,
														t.サイズ名 as _7SizeName,
														Convert(varchar,@DesiredDate1,111) as _8ShuukaYoteiBi,
														@SoukoName as _9SoukoName,
														Cast(format(t1.SiireTanka, 'N0') as varchar(12)) as _10SiireTanka, 
														Convert ( varchar,CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End,111) as _11NyuukaYoteiHyou ,
														
														format(Cast(t.即納数 as int), 'N0') as _12SoukunoSu,
														t1.TaniCD as _13TaniCD,
														format(t1.SalePriceOutTax, 'N0')  as _14SalePriceOutTax,
														format(Round(t1.SalePriceOutTax * t.即納数,0), 'N0')  as _15OrderExcTax,
														format(Round(t1.SalePriceOutTax *t.即納数* t1.TaxCalculation,0), 'N0')  as _16OrderIncTax,
														format(Round((t1.SalePriceOutTax - t1.SiireTanka)* t.[即納数],0), 'N0') as _17TotalProfit,
														t1.Taxnotation as _18Taxnotation ,
														t1.TaxRate as _19TaxRate,
														null as _20ExternaRemarks ,
														null as _21InternalRemarks,
														null as _22kobetsuHanbai,
														case when t1.JanCD is null then 0 else 1 end as _23TorokuFlag,
														t1.janCD as _24TaniCD,
														t1.TaxRateFlg as _25TaxRateFlg
													    into #gamen3
														from #temp t
														left outer join #temp1 t1 on t.[仮JANCD] = t1.JanCD
														where isnull(t.[希望日1],0) <>0

														--and t.[販売予定日] < '2020-09-30'
														and @DesiredDate1 > 
														(CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End ) 
														order by _1JANCD asc,
														_2SKUCD asc

														--gamen--4
														
													   	select
																t.仮JANCD as _1JanCD,
														t.SKUCD as _2SKUCD,
														t.商品名 as _3SKUName,
														--t1.JanCD as _1JanCD,
														--t1.SKUCD as _2SKUCD,
														--t1.SKUName as _3SKUName,
														null as _4ColorNo ,
														t.カラー名 as _5ColorName,
														null as _6SizeNo,
														t.サイズ名 as _7SizeName,
														Convert(varchar,@DesiredDate2, 111) as _8ShuukaYoteiBi,
														@SoukoName as _9SoukoName,
														format(t1.SiireTanka, 'N0')  as _10SiireTanka, 
														Convert ( varchar,CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End,111) as _11NyuukaYoteiHyou ,
														
														format(Cast(t.即納数 as int), 'N0') as _12SoukunoSu,
														t1.TaniCD as _13TaniCD,
														format(t1.SalePriceOutTax, 'N0')  as _14SalePriceOutTax,
														format(Round(t1.SalePriceOutTax * t.即納数,0), 'N0') as _15OrderExcTax,
														format(Round(t1.SalePriceOutTax *t.即納数* t1.TaxCalculation,0), 'N0')  as _16OrderIncTax,
														format(Round((t1.SalePriceOutTax - t1.SiireTanka)* t.[即納数],0), 'N0')  as _17TotalProfit,
														t1.Taxnotation as _18Taxnotation ,
														t1.TaxRate as _19TaxRate,
														null as _20ExternaRemarks ,
														null as _21InternalRemarks,
														null as _22kobetsuHanbai,
														case when t1.JanCD is null then 0 else 1 end as _23TorokuFlag,
														t1.janCD as _24TaniCD,
														t1.TaxRateFlg as _25TaxRateFlg
													    into #gamen4
														from #temp t
														left outer join #temp1 t1 on t.[仮JANCD] = t1.JanCD
														where isnull(t.[希望日2],0) <>0

														--and t.[販売予定日] < '2020-09-30'
														and @DesiredDate2 > 
														(CASE WHEN ISNUMERIC(REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '')) = 1
														THEN CASE WHEN RIGHT(t.販売予定日, 2) = '上旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '10' AS DATE)
														WHEN RIGHT(t.販売予定日, 2) = '中旬'
														THEN CAST('2020' + '/' + REPLACE(t.販売予定日, RIGHT(t.販売予定日, 3), '') + '/' + '20' AS DATE)
														ELSE CASE WHEN ISDATE('2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01') = 1
														THEN CAST(DATEADD(D, -1 ,DATEADD(M, 1, '2020' + '/' + REPLACE(t.販売予定日,RIGHT(t.販売予定日, 3), '') + '/' + '01')) AS DATE)
														ELSE NULL End End End ) 
														order by _1JANCD asc,
														_2SKUCD asc

														--select * from #temp

														select * into #res
														 from (
														select * from #gamen2
														union 
														select * from #gamen3
														union 
														select * from #gamen4) a

														select * from #res

														drop table #temp
														drop table #temp1
														drop table #gamen2
														drop table #gamen3
														drop table #gamen4
														drop table #res

													---	select * from #temp t left outer join #temp1 t1 on t.[仮JANCD] = t1.JanCD

--select * from M_SalesTax
--select * from #temp
--select * from #temp1
-- select

-- select * from SMSLOCAL.dbo.M_Multiporpose where ID='201' and [Key] ='05'

--select * from M_TenzikaiShouhin

----update M_TenzikaiShouhin set hanBaiTanka= '200'
-- select * from F_Customer(getdate())
--select * from M_SalesTax
END