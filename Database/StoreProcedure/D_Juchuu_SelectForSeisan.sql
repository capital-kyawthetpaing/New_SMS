 BEGIN TRY 
 Drop Procedure dbo.[D_Juchuu_SelectForSeisan]
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
CREATE PROCEDURE [dbo].[D_Juchuu_SelectForSeisan]
	-- Add the parameters for the stored procedure here
	@StoreCD as VarChar(6) ,
	@Date as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Create table #tmp_JuChuu
	(
		NumOfCustomer varchar(11),
		JuchuuDate date
	)

	Insert Into #tmp_JuChuu(NumOfCustomer,JuchuuDate)
	SELECT	 
	Count(Distinct djc.JuchuuNo) as NumOfCustomer,
	djc.JuchuuDate
	FROM F_Sales(cast(@Date as varchar(10))) fs
	Left Outer Join D_CollectPlan dcp on dcp.SalesNO = fs.SalesNO
	Left Outer Join D_Juchuu djc on djc.JuchuuNo = dcp.JuchuuNO
	--Left Outer Join M_Store ms on ms.StoreCD = ds.StoreCD and ms.ChangeDate <= ds.SalesDate
	Left Outer Join F_Store (cast(@Date as varchar(10))) fstore on fstore.StoreCD = fs.StoreCD and fstore.ChangeDate <= fs.SalesDate
	WHERE fs.DeleteDateTime is null 
	and fs.SalesDate = @Date
	and fs.StoreCD = @StoreCD 
	and dcp.DeleteDateTime is null 
	and dcp.InvalidFLG = 0
	and djc.JuchuuKBN in (2,3)
	and fstore.DeleteFlg = 0
	and fstore.StoreKBN = 1
	GROUP BY djc.JuchuuDate

	Select 
	IsNull(FORMAT(Convert(Int,tmpjc.NumOfCustomer),'#,#'),0) as NumOfCustomer
	From #tmp_JuChuu tmpjc 
	Where tmpjc.JuchuuDate = @Date

	drop table #tmp_JuChuu

END
