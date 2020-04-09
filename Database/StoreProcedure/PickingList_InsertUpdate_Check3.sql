 BEGIN TRY 
 Drop Procedure dbo.[PickingList_InsertUpdate_Check3]
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
CREATE PROCEDURE [dbo].[PickingList_InsertUpdate_Check3](
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@ShippingPlanDateFrom date,
	@ShippingPlanDateTo date,
	@PickingNO varchar(11),
	@Operator varchar(10),
	@DateTime datetime

	)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		--For sheet テーブル転送仕様Ａ
		Insert into D_Picking(
		PickingNO 
		,PickingKBN 
		,SoukoCD 
		,PrintDateTime 
		,PrintStaffCD 
		,InsertOperator 
		,InsertDateTime 
		,UpdateOperator
		,UpdateDateTime 
		,DeleteOperator
		,DeleteDateTime)
		values(@PickingNO,
		2,
		@SoukoCD,
		NULL,
		NULL,
		@Operator,
		@DateTime,
		@Operator,
		@DateTime,
		NULL,
		NULL)

		--For sheet テーブル転送仕様D
		Declare @rows int
		set @rows=0

		Insert into D_PickingDetails(
		 PickingNO
		,PickingRows
		,ReserveNO
		,ShippingPlanDate
		,PickingDate
		,PickingDoneDateTime
		,StaffCD
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		,DeleteOperator
		,DeleteDateTime
		)

		Select 
		@PickingNO,
		@rows + 1,
		dpd.ReserveNO,
		dpd.ShippingPlanDate,
		NULL,
		NULL,
		NULL,
		@Operator,
		@DateTime,
		@Operator,
		@DateTime,
		NULL,
		NULL
		from D_PickingDetails as dpd
		inner join D_Picking as dp on dp.PickingNO=dpd.PickingNO
									and dpd.DeleteDateTime is null
									and dpd.ReturnListDatetime is null
									and dpd.PickingDoneDateTime is not null
									and dpd.ShippingPlanDate between @ShippingPlanDateFrom and @ShippingPlanDateTo
		inner join D_Reserve as dr on dr.ReserveNO=dpd.ReserveNO
									and dr.DeleteDateTime is null
									and dr.ReserveKBN=1
									and dr.SoukoCD=@SoukoCD
									and dr.ReserveSu>0
									and dr.ShippingPossibleSu>0
									and dr.ShippingPlanDate between @ShippingPlanDateFrom and @ShippingPlanDateTo
		inner join D_JuchuuDetails as djd on djd.JuchuuNO=dr.Number
											and djd.JuchuuRows=dr.NumberRows
											and djd.UpdateCancelKBN=9
		inner join D_Stock as ds on ds.StockNO=dr.StockNO
									and ds.DeleteDateTime is null
									and ds.SoukoCD=@SoukoCD
		where dp.DeleteDateTime is null
		and dp.SoukoCD=@SoukoCD
		and dp.PickingKBN=1
		Order by ds.RackNO,dr.JanCD



		--For sheet テーブル転送仕様F
		Update D_PickingDetails
		set ReturnListDatetime=@DateTime
		where DeleteDateTime is null
		and ReturnListDatetime is null
		and PickingDoneDateTime is not null
		and ShippingPlanDate between @ShippingPlanDateFrom and @ShippingPlanDateTo

END
