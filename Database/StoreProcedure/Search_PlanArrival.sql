 BEGIN TRY 
 Drop Procedure dbo.[Search_PlanArrival]
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
CREATE PROCEDURE [dbo].[Search_PlanArrival]
	-- Add the parameters for the stored procedure here
	 @ChangeDate as date,
	@SoukoCD as varchar(6),
	@adminNO as int
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
					SELECT
					MAX(ISNULL(Convert(varchar(10),dap.ArrivalPlanDate), Convert(varchar(10),dap.ArrivalPlanMonth) + mmp.Char1)) as ArrivalPlanDate , 
					 MAX(fsk.SoukoName) as SoukoName,
					 MAX(dap.ArrivalPlanSu) as ArrivalPlanSu,
					 MAX(dap.Number) as Number,
					SUM(ds.AllowableSU) as AllowableSU
					from D_ArrivalPlan as dap
					left outer join F_Souko(@ChangeDate) as fsk on fsk.SoukoCD =dap.SoukoCD
					left 
					outer join F_SKU(@ChangeDate) as fs on fs.AdminNO= @adminNO
					left outer join F_Store(@ChangeDate) as fst on fst.StoreCD=fsk.SoukoCD
					left outer join M_Brand as mb on mb.BrandCD =fs.BrandCD
					left outer join D_Stock as ds on ds.ArrivalPlanNO = dap.ArrivalPlanNO
					and ds.DeleteDateTime is Null
					left outer join M_MultiPorpose as mmp on mmp.[Key]=dap.ArrivalPlanCD
					and ID='206'

					where dap.AdminNO =@adminNO
					and dap.SoukoCD=@SoukoCD
					and dap.CalcuArrivalPlanDate >= @ChangeDate
					and dap.DeleteDateTime is Null
					group by dap.ArrivalPlanCD 
					--order by CalcuArrivalPlanDate



				
END
