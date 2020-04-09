 BEGIN TRY 
 Drop Procedure dbo.[D_KeihiPrint]
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
CREATE PROCEDURE [dbo].[D_KeihiPrint]
	-- Add the parameters for the stored procedure here

	@RecordDateFrom as varchar(10)    ,  
	@RecordDateTo   as varchar(10)    ,  
	@ExpanseEntryDateFrom as varchar(10),
	@ExpanseEntryDateTo   as varchar(10),
	@InsertDateTimeFrom as varchar(10)  ,
	@InsertDateTimeTo   as varchar(10)  ,
	@printTarget as varchar(10),    
	@StoreName as varchar(50)       
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		if @printTarget = '0'
	Begin
	select
		CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
        CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
		dc.CostNO as CostNo,
		@StoreName as StoreName,
		CONVERT(varchar(10), dc.RecordedDate,111 ) as RecordedDate ,
		CONVERT(varchar(10), dc.PayPlanDate,111 )  as PayPlanDate, 
		CONVERT(varchar(10), dc.InputDateTime,111 )  as InputDateTime,
		dc.CostNO as CostNo,
		 --+ mv.VendorName as VendorCD,
		mv.VendorCD+mv.VendorName as VendorName,
		dcd.Summary as Summary,
		mmp.Char1 as Char1,
		dcd.CostGaku as CostGaku,
		dc.TotalGaku as TotalGaku
		from F_cost(getdate()) dc left outer Join
		D_CostDetails dcd on dcd.CostNO  = dc.CostNO left outer join
        D_PayPlan dpp on dpp.PayPlanKBN = 2 and dpp.Number = dc.CostNO left outer join
		F_Vendor(getdate()) mv on mv.VendorCD = dc.PayeeCD and
	    mv.ChangeDate <= dc.RecordedDate and mv.DeleteFlg =0 left outer join
		M_MultiPorpose mmp on mmp.ID = 209 and  mmp.[Key] = dcd.DepartmentCD 


where
	dc.DeleteDateTime is Null	 
	AND (@RecordDateFrom is null or (dc.RecordedDate>=CONVERT(DATE, @RecordDateFrom)))
	AND (@RecordDateTo is null or  (dc.RecordedDate<=CONVERT(DATE, @RecordDateTo)))
	AND (@ExpanseEntryDateFrom is null or (dc.PayPlanDate>=CONVERT(DATETIME, @ExpanseEntryDateFrom)))
	AND (@ExpanseEntryDateTo is null or  (dc.PayPlanDate<=CONVERT(DATETIME, @ExpanseEntryDateTo)))
	AND (@InsertDateTimeFrom is null or  (dc.InsertDateTime>=CONVERT(DATE, @InsertDateTimeFrom)))
	AND (@InsertDateTimeTo is null or  (dc.InsertDateTime<=CONVERT(DATE, @InsertDateTimeTo)))
	AND dpp.PayConfirmFinishedKBN = 0 
    order by dc.CostNO asc , dcd.CostRows asc
	End
	else if @printTarget = '1'
	Begin
	select
		CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
        CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
		dc.CostNO as CostNo,
		@StoreName as StoreName,
		CONVERT(varchar(10), dc.RecordedDate,111 ) as RecordedDate ,
		CONVERT(varchar(10), dc.PayPlanDate,111 )  as PayPlanDate, 
		CONVERT(varchar(10), dc.InputDateTime,111 )  as InputDateTime,
		dc.CostNO as CostNo,
		 --+ mv.VendorName as VendorCD,
		mv.VendorCD+mv.VendorName as VendorName,
		dcd.Summary as Summary,
		mmp.Char1 as Char1,
		dcd.CostGaku as CostGaku,
		dc.TotalGaku as TotalGaku
		from F_cost(getdate()) dc left outer Join
		D_CostDetails dcd on dcd.CostNO  = dc.CostNO left outer join
        D_PayPlan dpp on dpp.PayPlanKBN = 2 and dpp.Number = dc.CostNO left outer join
		F_Vendor(getdate()) mv on mv.VendorCD = dc.PayeeCD and
	    mv.ChangeDate <= dc.RecordedDate and mv.DeleteFlg =0 left outer join
		M_MultiPorpose mmp on mmp.ID = 209 and  mmp.[Key] = dcd.DepartmentCD 

where
	dc.DeleteDateTime is Null	 
	--AND	mv.DeleteFlg   = 0
	AND (@RecordDateFrom is null or (dc.RecordedDate>=CONVERT(DATE, @RecordDateFrom)))
	AND (@RecordDateTo is null or  (dc.RecordedDate<=CONVERT(DATE, @RecordDateTo)))
	AND (@ExpanseEntryDateFrom is null or (dc.PayPlanDate>=CONVERT(DATETIME, @ExpanseEntryDateFrom)))
	AND (@ExpanseEntryDateTo is null or  (dc.PayPlanDate<=CONVERT(DATETIME, @ExpanseEntryDateTo)))
	AND (@InsertDateTimeFrom is null or  (dc.InsertDateTime>=CONVERT(DATE, @InsertDateTimeFrom)))
	AND (@InsertDateTimeTo is null or  (dc.InsertDateTime<=CONVERT(DATE, @InsertDateTimeTo)))
	AND dpp.PayConfirmFinishedKBN = 1 
    order by dc.CostNO asc , dcd.CostRows asc
	End
	else
	Begin
	select
		CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
        CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
		dc.CostNO as CostNo,
		@StoreName as StoreName,
		CONVERT(varchar(10), dc.RecordedDate,111 ) as RecordedDate ,
		CONVERT(varchar(10), dc.PayPlanDate,111 )  as PayPlanDate, 
		CONVERT(varchar(10), dc.InputDateTime,111 )  as InputDateTime,
		dc.CostNO as CostNo,
		 --+ mv.VendorName as VendorCD,
		mv.VendorCD+mv.VendorName as VendorName,
		dcd.Summary as Summary,
		mmp.Char1 as Char1,
		dcd.CostGaku as CostGaku,
		dc.TotalGaku as TotalGaku
		from F_cost(getdate()) dc left outer Join
		D_CostDetails dcd on dcd.CostNO  = dc.CostNO left outer join
        D_PayPlan dpp on dpp.PayPlanKBN = 2 and dpp.Number = dc.CostNO left outer join
		F_Vendor(getdate()) mv on mv.VendorCD = dc.PayeeCD and
	    mv.ChangeDate <= dc.RecordedDate and mv.DeleteFlg =0 left outer join
		M_MultiPorpose mmp on mmp.ID = 209 and  mmp.[Key] = dcd.DepartmentCD 


where
	dc.DeleteDateTime is Null	 
	AND (@RecordDateFrom is null or (dc.RecordedDate>=CONVERT(DATE, @RecordDateFrom)))
	AND (@RecordDateTo is null or  (dc.RecordedDate<=CONVERT(DATE, @RecordDateTo)))
	AND (@ExpanseEntryDateFrom is null or (dc.PayPlanDate>=CONVERT(DATETIME, @ExpanseEntryDateFrom)))
	AND (@ExpanseEntryDateTo is null or  (dc.PayPlanDate<=CONVERT(DATETIME, @ExpanseEntryDateTo)))
	AND (@InsertDateTimeFrom is null or  (dc.InsertDateTime>=CONVERT(DATE, @InsertDateTimeFrom)))
	AND (@InsertDateTimeTo is null or  (dc.InsertDateTime<=CONVERT(DATE, @InsertDateTimeTo)))
    order by dc.CostNO asc , dcd.CostRows asc
	End

END

