

/****** Object:  StoredProcedure [dbo].[ZaikoMotochoulnsatsu_Report]    Script Date: 2020/11/03 19:54:15 ******/
DROP PROCEDURE [dbo].[ZaikoMotochoulnsatsu_Report]
GO

/****** Object:  StoredProcedure [dbo].[ZaikoMotochoulnsatsu_Report]    Script Date: 2020/11/03 19:54:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ZaikoMotochoulnsatsu_Report]
	-- Add the parameters for the stored procedure here
	@YYYYMMFrom as int ,
	@YYYYMMTo as int ,
	@SoukoCD as varchar(6) ,
	@itemcd as varchar(30) ,
	@sku as varchar(32) ,
	@jan as varchar(193) ,
	@makeritem as varchar(50) ,
	@skuName as varchar(100),
	@related as tinyint ,--1 item, 2 maker,3 normal
	@targetDateFrom as date ,
	@targerDateTo as date 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	/* Create temp table for AdminNO */
	CREATE TABLE [dbo].[#Tmp_AdminNo](
	[AdminNO] [int] NULL,
	[SKUCD][varchar](40) NULL,
	[SKUName] [varchar] (100) collate Japanese_CI_AS NULL,
	[JanCD] [varchar] (13) NULL,
	[ColorName][varchar](20) collate Japanese_CI_AS NULL,
	[SizeName][varchar](20) collate Japanese_CI_AS NULL,
	[BrandCD][varchar](6) NULL
	)
	
	/* Create temp table for D_MonthlyStock1 */
	CREATE TABLE [dbo].[#Tmp_D_MonthlyStock1](
	--[Order][int] NOT NULL,
	[SEQ][int] NOT NULL,
	[JanCD] [varchar] (13) collate Japanese_CI_AS NOT NULL,
	[WarehousingNo] [int] NULL,
	[AdminNO] [int] NOT NULL,
	[SKUCD] [varchar] (40) collate Japanese_CI_AS NOT NULL,
	[SoukoCD] [varchar] (6) collate Japanese_CI_AS NOT NULL,
	[WarehousingDate] [varchar] (7) NOT NULL,
	[StockFlg] [varchar] (30) collate Japanese_CI_AS NULL,
	[Number] [varchar] (11) collate Japanese_CI_AS NULL,
	[NumberRow] [int] NULL,
	[Info] [varchar] (100) collate Japanese_CI_AS  NULL,
	[ImportQty] [int] NULL,
	[ExportQty] [int] NULL,
	[StockQty] [int] NOT NULL
	CONSTRAINT [PK_Tmp_D_MonthlyStock1] PRIMARY KEY CLUSTERED
	(
	[JanCD] ASC,
	[SoukoCD] ASC,
	[WarehousingDate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	/* Create temp table for D_Warehousing1 */
	CREATE TABLE [dbo].[#Tmp_D_Warehousing1](
	--[Order][int] NOT NULL,
	[SEQ][int] NOT NULL,
	[JanCD] [varchar] (13) collate Japanese_CI_AS NOT NULL,
	[WarehousingNo] [int] NOT NULL,
	[AdminNO] [int] NOT NULL,
	[SKUCD] [varchar] (40) collate Japanese_CI_AS NOT NULL,
	[SoukoCD] [varchar] (6) collate Japanese_CI_AS NOT NULL,
	[WarehousingDate] [varchar] (10) NULL,
	[StockFlg] [varchar] (30) collate Japanese_CI_AS NULL,
	[Number] [varchar] (11) collate Japanese_CI_AS NULL,
	[NumberRow] [int] NULL,
	[Info] [varchar] (100) collate Japanese_CI_AS  NULL,
	[ImportQty] [int] NULL,
	[ExportQty] [int] NULL,
	[StockQty] [int] NULL,
	CONSTRAINT [PK_Tmp_D_Warehousing1] PRIMARY KEY CLUSTERED
	(
	[JanCD] ASC,
	[WarehousingNo] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	/* Create temp table for D_MonthlyStock2 */
	CREATE TABLE [dbo].[#Tmp_D_MonthlyStock2](
	--[Order][int] NOT NULL,
	[SEQ][int] NOT NULL,
	[JanCD] [varchar] (13) collate Japanese_CI_AS NOT NULL,
	[WarehousingNo] [int] NULL,
	[AdminNO] [int] NOT NULL,
	[SKUCD] [varchar] (40) collate Japanese_CI_AS NOT NULL,
	[SoukoCD] [varchar] (6) collate Japanese_CI_AS NOT NULL,
	[WarehousingDate] [varchar] (7) NOT NULL,
	[StockFlg] [varchar] (30) collate Japanese_CI_AS NULL,
	[Number] [varchar] (11) collate Japanese_CI_AS NULL,
	[NumberRow] [int] NULL,
	[Info] [varchar] (100) collate Japanese_CI_AS  NULL,
	[ImportQty] [int] NULL,
	[ExportQty] [int] NULL,
	[StockQty] [int] NOT NULL
	CONSTRAINT [PK_Tmp_D_MonthlyStock2] PRIMARY KEY CLUSTERED
	(
	[JanCD] ASC,
	[SoukoCD] ASC,
	[WarehousingDate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	
		/* Get AdminNO From M_SKU table according to detail of Doc. and  Insert Data into AdminNo temp table */
		if @related = 1
		begin
			insert into #Tmp_AdminNo
			select AdminNO,SKUCD,SKUName,JanCD,ColorName,SizeName,BrandCD from F_SKU(@targetDateFrom)  where ITemCD in
			(
			select ITemCD from F_SKU(@targetDateFrom) 
			where (@makeritem is null or (MakerItem IN (select * from SplitString(@makeritem,','))))
			and (@itemcd is null or(ITemCD IN (select * from SplitString(@itemcd,','))))
			and (@sku is null or (SKUCD IN (select * from SplitString(@sku,','))))
			and (@jan is null or(JanCD IN (select * from SplitString(@jan,','))))
			and (@skuName is null or(SKUName like '%' + @skuName + '%'))
			)
		end
		else if @related = 2
		begin
			insert into #Tmp_AdminNo
			select AdminNO,SKUCD,SKUName,JanCD,ColorName,SizeName,BrandCD from F_SKU(@targetDateFrom) where MakerItem in
			(
			select MakerItem from F_SKU(@targetDateFrom)
			where (@makeritem is null or (MakerItem IN (select * from SplitString(@makeritem,','))))
			and (@itemcd is null or(ITemCD IN (select * from SplitString(@itemcd,','))))
			and (@sku is null or (SKUCD IN (select * from SplitString(@sku,','))))
			and (@jan is null or(JanCD IN (select * from SplitString(@jan,','))))
			and (@skuName is null or(SKUName like '%' + @skuName + '%'))
			)
		end
		else if @related = 3
		begin
			insert into #Tmp_AdminNo
			select AdminNO,SKUCD,SKUName,JanCD,ColorName,SizeName,BrandCD from F_SKU(@targetDateFrom)
			where (@makeritem is null or (MakerItem IN (select * from SplitString(@makeritem,','))))
			and (@itemcd is null or(ITemCD IN (select * from SplitString(@itemcd,','))))
			and (@sku is null or (SKUCD IN (select * from SplitString(@sku,','))))
			and (@jan is null or(JanCD IN (select * from SplitString(@jan,','))))
			and (@skuName is null or(SKUName like '%' + @skuName + '%'))
		end
	
	
		/* Insert Data into D_MonthlyStock1 temp table */
		INSERT INTO #Tmp_D_MonthlyStock1
		SELECT
			--ROW_NUMBER() Over(PARTITION BY SKUCD ORDER BY SKUCD) AS [Order],
			1 AS [SEQ],
			JANCD AS [JANCD],
			NULL AS [WarehousingNO],
			AdminNO,
			SKUCD,
			SoukoCD,
			SUBSTRING(CONVERT(varchar,YYYYMM),1,4)+'/'+SUBSTRING(CONVERT(varchar,YYYYMM),5,2) AS [WarehousingDate],-------------------------日付
			'残',---------------------------------------------------------------入出庫区分
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			CASE 
				WHEN ThisMonthInventry IS NULL THEN 0
				ELSE ThisMonthInventry 
			END AS [LastMonthQuantity]------------------------------------------在庫数
		FROM D_MonthlyStock
		WHERE YYYYMM = (select * from F_DecreaseMonth(@YYYYMMFrom,1))
	--	and YYYYMM = @YYYYMMTo
		and SoukoCD = @SoukoCD
		and AdminNO in
		(
			select AdminNo from #Tmp_AdminNo
		)
		ORDER BY SKUCD ASC
		
	
		/* Insert Data into D_Warehousing1 temp table */
		INSERT INTO #Tmp_D_Warehousing1
		SELECT
			2 AS [SEQ],
			DW.JanCD AS [JANCD],
			DW.WarehousingNO AS [WarehousingNO],
			DW.AdminNO AS [AdminNO],
			DW.SKUCD AS [SKUCD],
			DW.SoukoCD AS [SoukoCD],
			CONVERT(varchar,DW.WarehousingDate,111) AS [WarehousingDate],-----------------------------日付
			/*
			CASE
				WHEN DW.WarehousingKBN = 1 THEN '入荷'
				WHEN DW.WarehousingKBN = 3 THEN '出荷'
				WHEN DW.WarehousingKBN = 5 THEN '出荷売上'
				WHEN DW.WarehousingKBN = 11 THEN '店舗内倉庫移動出'
				WHEN DW.WarehousingKBN = 12 THEN '店舗内倉庫移動入'
				WHEN DW.WarehousingKBN = 13 THEN '店舗間移動出'
				WHEN DW.WarehousingKBN = 14 THEN '店舗間移動入'
				WHEN DW.WarehousingKBN = 15 THEN '商品CD付替'
				WHEN DW.WarehousingKBN = 16 THEN '返品対象'
				WHEN DW.WarehousingKBN = 17 THEN '棚卸加算'
				WHEN DW.WarehousingKBN = 18 THEN '棚卸減算'
				WHEN DW.WarehousingKBN = 19 THEN '調整追加'
				WHEN DW.WarehousingKBN = 20 THEN '調整削除'
				WHEN DW.WarehousingKBN = 21 THEN '返品'
				WHEN DW.WarehousingKBN = 22 THEN '棚番移動'
				WHEN DW.WarehousingKBN = 23 THEN '売上返品'
				WHEN DW.WarehousingKBN = 24 THEN '売上変更'
				WHEN DW.WarehousingKBN = 25 THEN '店頭売上'
				WHEN DW.WarehousingKBN = 30 THEN '仕入'
			END AS [StockFlg], ------------------------------入出庫区分
			*/
			MWK.WarehousingName AS [WarehousingKBN], 
			DW.Number AS [Number],-----------------------------------------------伝票番号
			DW.NumberRow AS [NumberRow],-----------------------------------------伝票番号連番
			CASE
				WHEN DW.WarehousingKBN = 1 OR DW.WarehousingKBN = 2 THEN '仕入先：' + DW.VendorCD + ' ' + mv.VendorShortName
				WHEN DW.WarehousingKBN = 3 OR DW.WarehousingKBN = 4 THEN '会員：' + DW.CustomerCD + ' ' + MC.CustomerName
				WHEN DW.WarehousingKBN = 13 OR DW.WarehousingKBN = 14 THEN '移動元倉庫：' + DW.FromSoukoCD + ' ' + MSF.SoukoName
				WHEN DW.WarehousingKBN = 11 OR DW.WarehousingKBN = 12 THEN '移動先倉庫：' + DW.ToSoukoCD + ' ' + MST.SoukoName
				WHEN DW.WarehousingKBN = 26 THEN '移動元倉庫：' + DW.FromSoukoCD + ' ' + MSF.SoukoName
				WHEN DW.WarehousingKBN = 16 THEN '移動先倉庫：' + DW.ToSoukoCD + ' ' + MST.SoukoName
				WHEN DW.WarehousingKBN = 43 OR DW.WarehousingKBN = 44 THEN '移動元店舗：' + DW.FromStoreCD + ' ' + MMF.StoreName
				WHEN DW.WarehousingKBN = 41 OR DW.WarehousingKBN = 42 THEN '移動先店舗：' + DW.ToStoreCD + ' ' + MMT.StoreName
				WHEN DW.WarehousingKBN = 17 OR
				DW.WarehousingKBN = 18 OR
				DW.WarehousingKBN = 19 OR
				DW.WarehousingKBN = 20 OR
				DW.WarehousingKBN = 23 then NULL
			END AS [Info],--------------------------------------------------------情報
			Case 
				WHEN MWK.CalculationKBN = 1 THEN Quantity
					--(Select Quantity from D_Warehousing where WarehousingDate >= @targetDateFrom and WarehousingDate <= @targerDateTo and SoukoCD = @SoukoCD  and MWK.CalculationKBN = 1 and WarehousingKBN IS NOT NULL )
				WHEN MWK.CalculationKBN = 2 THEN NULL END AS [ImportQty],--------入庫数
			
			Case 
				WHEN MWK.CalculationKBN = 2 THEN Quantity * (-1)
					--(Select Quantity  from D_Warehousing where WarehousingDate >= @targetDateFrom and WarehousingDate <= @targerDateTo and SoukoCD = @SoukoCD and AdminNO in (select AdminNO from #Tmp_AdminNo) and MWK.CalculationKBN = 2)
				WHEN MWK.CalculationKBN = 1 THEN NULL END AS [ExportQty],--------出庫数
			0 AS [StockQ]--------------------------------------------------在庫数
		FROM D_Warehousing DW
		left outer join D_MonthlyStock DM ON DM.YYYYMM = REPLACE(CONVERT(varchar(7),DW.WarehousingDate,111),'/','') AND DM.SoukoCD = DW.SoukoCD AND DM.AdminNO = DW.AdminNO
		left outer join F_Vendor(@targetDateFrom) mv ON mv.VendorCD = DW.VendorCD
		left outer join F_Customer(@targetDateFrom) MC ON MC.CustomerCD = DW.CustomerCD
		left outer join F_Souko(@targetDateFrom) MSF ON MSF.SoukoCD = DW.FromSoukoCD
		left outer join F_Souko(@targetDateFrom) MST ON MST.SoukoCD = DW.ToSoukoCD	
		left outer join F_Store(@targetDateFrom) MMF ON MMF.StoreCD = DW.FromStoreCD
		left outer join F_Store(@targetDateFrom)  MMT ON MMT.StoreCD = DW.ToStoreCD	
		left outer join M_WarehousingKBN MWK ON MWK.WarehousingKBN = DW.WarehousingKBN
		WHERE DW.WarehousingDate >= @targetDateFrom
		AND DW.WarehousingDate <= @targerDateTo
		AND DW.SoukoCD = @SoukoCD
		AND MWK.CalculationKBN > 0
		AND DW.AdminNO in
		(
			select AdminNo from #Tmp_AdminNo
		)
		ORDER BY DW.SKUCD ASC
		
	
		/* Insert Data into D_MonthlyStock2 temp table */
		INSERT INTO #Tmp_D_MonthlyStock2
		SELECT
			3 AS [SEQ],
			JANCD AS [JANCD],
			NULL AS [WarehousingNO],
			AdminNO AS [AdminNO],
			SKUCD AS [SKUCD],
			SoukoCD AS [SoukoCD],
			SUBSTRING(CONVERT(varchar,YYYYMM),1,4)+'/'+SUBSTRING(CONVERT(varchar,YYYYMM),5,2) As [WarehousingDate],
			'残',----------------------------------------------------------------入出庫区分
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			ThisMonthInventry AS ThisMonthQuantity-------------------------------在庫数
		FROM D_MonthlyStock
--		WHERE YYYYMM = @YYYYMMFrom
		Where YYYYMM = @YYYYMMTo
		AND SoukoCD = @SoukoCD
		AND AdminNO in
		(
			select AdminNo from #Tmp_AdminNo
		)
		ORDER BY SKUCD ASC

		CREATE TABLE [dbo].[#Tmp](
		[Order][int] NULL,
		[SEQ][int] NOT NULL,
		[JanCD] [varchar] (13) collate Japanese_CI_AS NOT NULL,
		[WarehousingNo] [int] NULL,
		[AdminNO] [int] NOT NULL,
		[SKUCD] [varchar] (40) collate Japanese_CI_AS NOT NULL,
		[SoukoCD] [varchar] (6) collate Japanese_CI_AS NOT NULL,
		[WarehousingDate] [varchar] (10) NOT NULL,
		[StockFlg] [varchar] (30) collate Japanese_CI_AS NULL,
		[Number] [varchar] (11) collate Japanese_CI_AS NULL,
		[NumberRow] [int] NULL,
		[Info] [varchar] (100) collate Japanese_CI_AS  NULL,
		[ImportQty] [int] NULL,
		[ExportQty] [int] NULL,
		[StockQty] [int] NOT NULL,
		[SKUCD1][varchar](30) collate Japanese_CI_AS NULL,
		[SKUName][varchar](100) collate Japanese_CI_AS NULL,
		[ColorName][varchar](20) collate Japanese_CI_AS NULL,
		[SizeName][varchar](20) collate Japanese_CI_AS NULL,
		[BrandName][varchar](40) collate Japanese_CI_AS NULL)
	
		INSERT INTO #Tmp 
		select 
		0,
		dm1.*,
		ad.SKUCD,
		ad.SKUName,
		ad.ColorName,
		ad.SizeName,
		(select BrandName from M_Brand where BrandCD COLLATE DATABASE_DEFAULT = ad.BrandCD COLLATE DATABASE_DEFAULT) AS BrandName
		from #Tmp_D_MonthlyStock1 dm1 inner join #Tmp_AdminNo ad on ad.SKUCD COLLATE DATABASE_DEFAULT = dm1.SKUCD COLLATE DATABASE_DEFAULT
		UNION 
		select 
		0,
		dw1.*,
		ad.SKUCD,
		ad.SKUName,
		ad.ColorName,
		ad.SizeName,
		(select BrandName from M_Brand where BrandCD COLLATE DATABASE_DEFAULT = ad.BrandCD COLLATE DATABASE_DEFAULT) AS BrandName
		from #Tmp_D_Warehousing1 dw1 inner join #Tmp_AdminNo ad on ad.SKUCD COLLATE DATABASE_DEFAULT = dw1.SKUCD COLLATE DATABASE_DEFAULT
		UNION 
		select 
		0,
		dm2.*,
		ad.SKUCD,
		ad.SKUName,
		ad.ColorName,
		ad.SizeName,
		(select BrandName from M_Brand where BrandCD COLLATE DATABASE_DEFAULT = ad.BrandCD COLLATE DATABASE_DEFAULT) AS BrandName
		from #Tmp_D_MonthlyStock2 dm2 inner join #Tmp_AdminNo ad on ad.SKUCD COLLATE DATABASE_DEFAULT = dm2.SKUCD COLLATE DATABASE_DEFAULT
		ORDER BY SEQ,ad.SKUCD

		Update #Tmp
		SET StockQty = ImportQty
		WHERE ImportQty IS NOT NULL

		Update #Tmp
		SET StockQty = -ExportQty
		WHERE ExportQty IS NOT NULL

		Update #Tmp SET [Order] = 1 WHERE SEQ = 1 OR SEQ = 3
		
		CREATE SEQUENCE idnum
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		
		UPDATE #Tmp
		SET [Order] = NEXT VALUE FOR idnum
		WHERE SEQ = 2
		DROP SEQUENCE idnum

		SELECT 
		SEQ,
		JanCD,
		WarehousingNo,
		AdminNO,
		SKUCD,
		SoukoCD,
		WarehousingDate,
		StockFlg,
		Number,
		NumberRow,
		Info,
		ImportQty,
		ExportQty,
		CASE
			WHEN SEQ = 1 or SEQ = 3
				THEN StockQty
			ELSE SUM(StockQty) OVER (Partition by SKUCD ORDER BY SKUCD,SEQ,[Order])
			END AS StockQty,
		SKUName,
		ColorName,
		SizeName,
		BrandName
		FROM #Tmp 
		ORDER BY SKUCD,SEQ
		
	drop table #Tmp
	drop table #Tmp_AdminNo
	drop table #Tmp_D_MonthlyStock1
	drop table #Tmp_D_Warehousing1	
	drop table #Tmp_D_MonthlyStock2
END

GO


