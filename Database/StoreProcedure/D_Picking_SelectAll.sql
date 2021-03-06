 BEGIN TRY 
 Drop Procedure dbo.[D_Picking_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Picking_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Picking_SelectAll](
    -- Add the parameters for the stored procedure here
    @PrintDateTimeFrom  varchar(10),
    @PrintDateTimeTo  varchar(10),
    @SoukoCD  varchar(6)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
   Select DP.PickingNO,
		DP.SoukoCD
		,CONVERT(varchar, DP.PrintDateTime,111) AS PrintDateTime
        ,(CASE WHEN  DP.PickingKBN = 2 THEN '〇' ELSE '' END) AS PickingKBN
		,FS.SKUName
		,(CASE WHEN  DDP.DeliveryKBN = 2 THEN MS.SoukoName  Else  DDP.DeliveryName END)  as  DeliveryName  
		

	From D_Picking as DP
	Inner JOin D_PickingDetails as DPD 
	on DPD.PickingNO = DP.PickingNO
	and DPD.DeleteDateTime is Null 
	and DPD.PickingRows = 1

	Left outer Join D_Reserve as DR
	on DR.ReserveNo =DPD.ReserveNo
	and DR.DeleteDateTime is Null

	Left outer join F_SKU(getdate()) as FS
	on FS.AdminNo=DR.AdminNo
	and FS.ChangeDate <= DR.ShippingPlanDate

	Left outer join D_DeliveryPlan as DDP
	on DDP.Number = DR.Number 

	Left outer join F_Souko(getdate()) as MS
	on MS.SoukoCD = DDP.DeliverySoukoCD
	and MS.DeleteFlg=0
	and MS.ChangeDate <= DDP.DeliveryPlanDate

	Where DP.DeleteDateTime is Null
	and DP.SoukoCD =@SoukoCD
	AND (@PrintDateTimeFrom is Null or CONVERT(varchar, DP.PrintDateTime,111) >=   @PrintDateTimeFrom )
    AND ( @PrintDateTimeTo is Null or CONVERT(varchar, DP.PrintDateTime,111) <=   @PrintDateTimeTo )
	ORDER BY DP.PickingNO
    ;

END



