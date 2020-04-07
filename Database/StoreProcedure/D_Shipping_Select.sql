 BEGIN TRY 
 Drop Procedure dbo.[D_Shipping_Select]
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
CREATE PROCEDURE [dbo].[D_Shipping_Select]
	-- Add the parameters for the stored procedure here
	@SoukoCD as nvarchar(11),
	@ShippingStartDate as date,
	@ShippingEndDate as date,
	@Number as varchar(11),
	@DeliverySoukoCD as nvarchar(6),
	@CarrierCD as nvarchar(4),
	@ItemCD as varchar(30),
	@SKUCD as varchar(30),
	@JanCD as varchar(13),
	@ShippingKBN as tinyint,
	@InvoiceNO as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here

	Select 
	ds.ShippingNO,
	(CASE 
		  WHEN ds.ShippingKBN = 2 THEN mvp.MovePurposeName
		  Else N'販売'
	END) AS Movement,
	ds.ShippingDate,
	di.DeliveryName,
	dsd.SKUCD,
	dsd.JanCD,
	dsd.SKUName,
	dsd.ColorName + '' + dsd.SizeName as ColorSize,
	di.CommentOutStore,
	dsd.ShippingSu,
	(CASE 
		  WHEN dsd.ShippingKBN = '1' THEN dsd.Number
		  Else  null
	END) AS OrderNumber,
	mc.CarrierName,
	dsale.SalesDate,
	mstaff.StaffName
	From D_Shipping ds
	Left Outer Join D_ShippingDetails dsd on dsd.ShippingNO = ds.ShippingNO 
	Left Outer Join D_InstructionDetails did on did.InstructionNO = dsd.InstructionNO and did.InstructionRows = dsd.InstructionRows
	Left Outer Join D_Instruction di on di.InstructionNO = did.InstructionNO
	Left Outer Join D_Reserve dr on dr.ShippingOrderNO = did.InstructionNO and dr.ShippingOrderRows = did.InstructionRows and dr.ReserveKBN = '2'
	Left Outer Join D_DeliveryPlan ddp on ddp.DeliveryKBN = 2 and ddp.Number = dsd.Number
	Left Outer Join D_MoveDetails dmd on dmd.MoveNO = dr.Number and dmd.MoveRows = dr.NumberRows and dmd.DeliveryPlanNO = ddp.DeliveryPlanNO
	Left Outer Join D_Move dm on dm.MoveNO = dmd.MoveNO 
	Left Outer Join D_Sales dsale on dsale.ShippingNO = ds.ShippingNO 
	Left Outer Join F_Carrier(GETDATE()) mc on mc.CarrierCD = ds.CarrierCD and mc.ChangeDate <= ds.ShippingDate
	Left Outer Join F_Staff(GETDATE())  mstaff on mstaff.StaffCD = ds.StaffCD 	
	Left Outer Join M_MovePurpose mvp on mvp.MovePurposeKBN = dm.MovePurposeKBN

	Where ds.DeleteDateTime is null and 
	dsd.DeleteDateTime is null and
	did.DeleteDateTime is null and
	di.DeleteDateTime is null and
	dr.DeleteDateTime is null and 
	dmd.DeleteDateTime is null and
	dm.DeleteDateTime is null and
	ds.DeleteDateTime is null and
	mc.DeleteFlg = 0 and 
	mstaff.DeleteFlg = 0 and 
	ds.SoukoCD = @SoukoCD and
	(@ShippingStartDate is null or ds.ShippingDate >= @ShippingStartDate) and
	 (@ShippingEndDate is null or ds.ShippingDate <= @ShippingEndDate) and
	 (@ShippingKBN is null or ( (@ShippingKBN = 0 or (ds.ShippingKBN <> 2)) ))and
	 (@ShippingKBN is null or ( (@ShippingKBN = 1 or (ds.ShippingKBN = 2)) )) and
	--ds.ShippingKBN <> 2 and ds.ShippingKBN = 2 and
	(@Number is null or dsd.Number = @Number) and
	(@DeliverySoukoCD is null or di.DeliverySoukoCD = @DeliverySoukoCD) and  
	(@InvoiceNO is null or ((@InvoiceNO = 1 or (ds.InvoiceNO is not null)) )) and 
	(@InvoiceNO is null or ((@InvoiceNO = 0 or (ds.InvoiceNO is null )) )) and
	--ds.InvoiceNO is not null and 
	--ds.InvoiceNO is null and 
	(@CarrierCD is null or ds.CarrierCD = @CarrierCD) and 
	(@ITEMCD is null or dsd.SKUCD in (Select SKUCD from M_SKU where ITemCD in (select Item from SplitString(@ITEMCD,',')))) and 
	(@JanCD is null or (dsd.JanCD in  (select Item from SplitString(@JanCD,','))))and
	(@SKUCD is null  or( dsd.SKUCD in (select Item from SplitString(@SKUCD,','))))
	Order by dsd.ShippingNO Asc, dsd.ShippingRows Asc , ds.ShippingDate Asc , dsd.SKUCD Asc ,dsd.JANCD Asc



END

