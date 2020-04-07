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
	@DataCheck as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--if @DataCheck = 0
	--begin
	--	SELECT msk.SoukoName,
	--	ds.JanCD +' ' + ms.ColorName +' ' +ms.SizeName  as JanCD ,
	--	--ms.ColorName +' ' +ms.SizeName as SKU,
	--	ds.ArrivalPlanDate,
	--	CASE WHEN ds.ArrivalYetFLG = 0 THEN  FORMAT(ds.StockSu, '#,##0')  
	--		 WHEN ds.ArrivalYetFLG = 1 THEN FORMAT(ds.StockSu, '#,##0')    END  ZaikouSu,
	--	 CASE WHEN msk.StoreCD = mstaff .StoreCD  THEN   FORMAT(ds.AllowableSu, '#,##0')
	--	     WHEN msk.StoreCD != mstaff .StoreCD  THEN   FORMAT(ds.AnotherStoreAllowableSu, '#,##0') END KanoSu
	--	From D_Stock ds
	--	Left outer join M_SKU ms on ms.JanCD = ds.JanCD and ms.SKUCD = ds.SKUCD and ms.ChangeDate <= ds.ArrivalDate
	--	Left outer join M_Souko msk on msk.SoukoCD = ds.SoukoCD and msk.ChangeDate <= ds.ArrivalDate 
	--	Left outer join M_Staff mstaff on mstaff.StaffCD = @Operator and mstaff.ChangeDate <= ds.ArrivalDate
	--	Where ms.DeleteFlg = 0 AND
	--	msk.DeleteFlg = 0 AND
	--	mstaff.DeleteFlg = 0 AND
	--	ds.JanCD = @JanCD AND
	--	ds.DeleteDateTime is null
	--	Order by ds.SoukoCD asc,ds.JanCD asc ,ms.ColorNO asc ,ms.SizeNO asc,ds.ArrivalPlanDate asc,
	--	CASE WHEN ds.ArrivalYetFLG = 0 THEN ds.StockSu  
	--	     WHEN ds.ArrivalYetFLG = 1 THEN ds.PlanSu  
	--	     WHEN msk.StoreCD = mstaff .StoreCD  THEN ds.AllowableSu  
	--	     WHEN msk.StoreCD != mstaff .StoreCD  THEN ds.AnotherStoreAllowableSu END asc
	--end

	--else if @DataCheck = 1
	--begin
	--	SELECT msk.SoukoName,
	--	ds.JanCD +' ' + ms.ColorName +' ' +ms.SizeName  as JanCD ,
	--	--ms.ColorName +' ' +ms.SizeName as SKU,
	--	ds.ArrivalPlanDate,
	--	CASE WHEN ds.ArrivalYetFLG = 0 THEN  FORMAT(ds.StockSu, '#,##0')  
	--		 WHEN ds.ArrivalYetFLG = 1 THEN FORMAT(ds.StockSu, '#,##0')    END  ZaikouSu,
	--	 CASE WHEN msk.StoreCD = mstaff .StoreCD  THEN   FORMAT(ds.AllowableSu, '#,##0')
	--	     WHEN msk.StoreCD != mstaff .StoreCD  THEN   FORMAT(ds.AnotherStoreAllowableSu, '#,##0') END KanoSu
	--	From D_Stock ds
	--	Left outer join M_SKU ms on ms.JanCD = ds.JanCD and ms.SKUCD = ds.SKUCD and ms.ChangeDate <= ds.ArrivalDate
	--	Left outer join M_Souko msk on msk.SoukoCD = ds.SoukoCD and msk.ChangeDate <= ds.ArrivalDate 
	--	Left outer join M_Staff mstaff on mstaff.StaffCD = @Operator and mstaff.ChangeDate <= ds.ArrivalDate
	--	Where ms.DeleteFlg = 0 AND
	--	msk.DeleteFlg = 0 AND
	--	mstaff.DeleteFlg = 0 AND
	--	ms.ITemCD = (select Top 1 ITemCD  From M_SKU ms Where ms.DeleteFlg = 0 AND ms.JanCD = @JanCD ) AND
	--	ds.DeleteDateTime is null
	--	Order by ds.SoukoCD asc,ds.JanCD asc ,ms.ColorNO asc ,ms.SizeNO asc,ds.ArrivalPlanDate asc,
	--	CASE WHEN ds.ArrivalYetFLG = 0 THEN ds.StockSu  
	--	     WHEN ds.ArrivalYetFLG = 1 THEN ds.PlanSu  
	--	     WHEN msk.StoreCD = mstaff .StoreCD  THEN ds.AllowableSu  
	--	     WHEN msk.StoreCD != mstaff .StoreCD  THEN ds.AnotherStoreAllowableSu END asc
	--end

	
	select ms.SoukoName,
		   IsNull(fs.JanCD,'')+ ' ' + IsNull(msku.ColorName,'') + ' ' + IsNull(msku.SizeName,'') as JanCD,
		   CONVERT(char(10), fs.ArrivalPlanDate,111) as ArrivalPlanDate, 
		   case when fs.ArrivalYetFLG = 0 then ISNULL(FORMAT(fs.StockSu, '#,##0'),0)
				when fs.ArrivalYetFLG = 1 then ISNULL(FORMAT(fs.PlanSu, '#,##0'),0)  end as ZaikouSu,
		   case when ms.StoreCD = ms1 .StoreCD then FORMAT(fs.AllowableSu, '#,##0')
			    else FORMAT(fs.AnotherStoreAllowableSu, '#,##0') end as KanoSu
	from F_Stock(getdate()) fs
	left outer join F_SKU(getdate()) msku on fs.JanCD = msku.JanCD and msku.SKUCD = fs.SKUCD  and msku.DeleteFlg = 0
	left outer join F_Souko(getdate()) ms on ms.SoukoCD = fs.SoukoCD and  ms.DeleteFlg = 0
	left outer join F_Staff(getdate()) ms1 on ms1.StaffCD = @Operator and ms1.DeleteFlg = 0
	where fs.DeleteDateTime is null
	and (@DataCheck = 1 or (fs.JanCD = @JanCD))
	and (@DataCheck = 0 or (msku.ITemCD in (select distinct ItemCD from M_SKU where DeleteFlg = 0 and JanCD = @JanCD)))
	order by fs.SoukoCD asc,fs.JanCD asc ,msku.ColorNO asc ,msku.SizeNO asc,fs.ArrivalPlanDate asc,
			 case when fs.ArrivalYetFLG = 0 then fs.StockSu  
			      when fs.ArrivalYetFLG = 1 then fs.PlanSu end asc,
			 case when ms.StoreCD = ms1 .StoreCD then fs.AllowableSu  
			      else fs.AnotherStoreAllowableSu end asc

END

