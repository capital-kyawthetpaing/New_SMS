
 BEGIN TRY 
 Drop Procedure dbo.[TanaireList_DataSelect]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[TanaireList_DataSelect]
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@SKUCD varchar(30),
	@ArrivalStartDate  varchar(10),
	@ArrivalEndDate  varchar(10),
	@RegisterFlg nvarchar(10),
	@LocationFlg nvarchar(10),
	@Option varchar(2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare	@query nvarchar(max)
	--@ChangeDate date=getdate()
	
	if(@Option='2')
	begin
		set @query='

		SELECT ds.RackNO,
		ds.SKUCD,
		case when ds.RackNO is null 
			then
			 case when fs.RackNO is null then '''' else fs.RackNO end
			else ''''
		end as ''RackNo1'',
		msku.Rack,
		msku.SKUName,
		ds.StockSu,
		ds.JanCD,
		msku.ColorName,
		msku.SizeName,
		CONVERT(VARCHAR(10), ds.ArrivalDate , 111) as ArrivalDate
		from D_Stock as ds 
		left outer join F_SKU(CONVERT(VARCHAR(10), getdate() , 120)) as msku on ds.JanCD=msku.JanCD and ds.SKUCD=msku.SKUCD and msku.AdminNO<=ds.AdminNO
		left outer join F_Stock(CONVERT(VARCHAR(10), getdate() , 120)) as fs on fs.SoukoCD=ds.SoukoCD and fs.SKUCD=ds.SKUCD and fs.StockSu>0
		where ds.DeleteDateTime is null
		--and msku.DeleteFlg=0
		and ds.StockSu>0
		and ds.SoukoCD= '''+ @SoukoCD
		+''' and ds.ArrivalDate>= ''' +@ArrivalStartDate+''' and ds.ArrivalDate<= '''+ @ArrivalEndDate +''''

		if(@RegisterFlg='–¢“o˜^')
			set @query+=' and ds.RackNO is null '
		if(@RegisterFlg='“o˜^Ï')
			set @query+=' and ds.RackNO is not null '
		if(@RegisterFlg='—¼•û')
			set @query+='';

		if(@LocationFlg='‚ ‚è')
			set @query+=' and fs.RackNO is not null '
		if(@LocationFlg='‚È‚µ')
			set @query+=' and fs.RackNO is null '
		if(@LocationFlg='—¼•û')
			set @query+='';

		set @query += ' order by ds.SKUCD asc '

		Exec(@query)
	end

	Else
	begin
		select ds.RackNO,
		ds.SKUCD,
		'' as RackNo1,
		'test' as Rack,
		msku.SKUName,
		ds.StockSu,
		ds.JanCD,
		msku.ColorName,
		msku.SizeName,
		CONVERT(VARCHAR(10), ds.ArrivalDate , 111) as ArrivalDate
		from D_Stock as ds 
		left outer join F_SKU(CONVERT(VARCHAR(10), getdate() , 120)) as msku on ds.SKUCD=msku.SKUCD and msku.AdminNO=ds.AdminNO
		where ds.SoukoCD=@SoukoCD
		and ds.SKUCD=@SKUCD
		and ds.ArrivalDate>=@ArrivalStartDate and ds.ArrivalDate<=@ArrivalEndDate
		order by ds.SKUCD,ds.ArrivalDate,ds.RackNO
	end

END
