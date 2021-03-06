 BEGIN TRY 
 Drop Procedure dbo.[D_Stock_DataSelect]
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
CREATE PROCEDURE [dbo].[D_Stock_DataSelect]
	-- Add the parameters for the stored procedure here
	@JanCD as varchar(13),
	@Operator as varchar(10),
	@DataCheck as tinyint,
	@ItemCD as varchar(30),
	@SKUName as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Select 
	ms.SoukoName,
	--msku.ItemCD + ' ' + IsNull(fs.JanCD,'') + char(13) + char(10) + IsNull(msku.SKUName,'')  +
	----fs.JanCD,
	----IsNull(msku.ColorName,'') + ' ' + IsNull(msku.SizeName,'') as ColorSize,
	--  ' '  + IsNull(msku.ColorName,'') + ' ' + IsNull(msku.SizeName,'') as ItemCD,
	----msku.SKUName,
	msku.ItemCD,
	IsNull(fs.JanCD,'') as JanCD,
	IsNull(msku.SKUName,'') as SKUName,
	IsNull(msku.ColorName,'') + ' ' + IsNull(msku.SizeName,'') as ColorSize,
	case when fs.ArrivalYetFLG = 0 then Null
				when fs.ArrivalYetFLG = 1 then fs.ArrivalPlanDate end as StockDate,
	case when fs.ArrivalYetFLG = 0 then fs.StockSu
				when fs.ArrivalYetFLG = 1 then fs.PlanSu end as StockNum,
	case when ms.StoreCD = ms1 .StoreCD then FORMAT(fs.AllowableSu, '#,##0')
			    else FORMAT(fs.AnotherStoreAllowableSu, '#,##0') end as KanoSu
	From D_Stock fs
	left outer join F_SKU(getdate()) msku on fs.AdminNO = msku.AdminNO and msku.SKUCD = fs.SKUCD  and msku.DeleteFlg = 0
	left outer join F_Souko(getdate()) ms on ms.SoukoCD = fs.SoukoCD and  ms.DeleteFlg = 0
	left outer join F_Staff(getdate()) ms1 on ms1.StaffCD = @Operator and ms1.DeleteFlg = 0
	where fs.DeleteDateTime is null
	and (@DataCheck = 1 or ((@JanCD is null or (fs.JanCD = @JanCD))))
	and (@ItemCD is NUll or (msku.ITemCD Like   '%' +@ItemCD + '%'))
	and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%'))
	and fs.ArrivalPlanKBN != 1 
	and ((ArrivalYetFLG = 0 and StockSu > 0)
		or ( ArrivalYetFLG = 1 and PlanSu > 0))
	and (@DataCheck = 0 or (msku.ITemCD in (select distinct ItemCD from M_SKU where DeleteFlg = 0 and (@JanCD is null or (JanCD = @JanCD) )
	and (@ItemCD is NUll or (msku.ITemCD Like   '%' +@ItemCD + '%'))
	and (@SKUName is NUll or (msku.SKUName Like   '%' +@SKUName + '%')) )))
	order by fs.SoukoCD asc,msku.ITemCD asc ,msku.ColorNO asc ,msku.SizeNO asc,fs.ArrivalPlanDate asc,
			 case when fs.ArrivalYetFLG = 0 then fs.StockSu  
			      when fs.ArrivalYetFLG = 1 then fs.PlanSu end asc,
			 case when ms.StoreCD = ms1.StoreCD then fs.AllowableSu  
			      when ms.StoreCD != ms1.StoreCD then fs.AnotherStoreAllowableSu end asc

END

