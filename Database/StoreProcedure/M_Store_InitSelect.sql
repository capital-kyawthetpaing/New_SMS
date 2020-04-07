 BEGIN TRY 
 Drop Procedure dbo.[M_Store_InitSelect]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [M_Saff_InitSelect]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Store_InitSelect]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--select top 1 MS.* 
	--,CONVERT(VARCHAR,GETDATE(),111) sysDate
	--from M_Staff MS
	--where MS.StaffCD = @StaffCD
	--AND MS.ChangeDate <= GETDATE()
	--order by MS.ChangeDate desc

	Declare
	@Datetime datetime=getdate()

	select top 1 StoreCD,StoreName,CONVERT(VARCHAR,GETDATE(),111) sysDate
	from M_Store ms
	where ms.StoreCD= (select mst.StoreCD 
						from F_Staff(@Datetime) mst
						where mst.StaffCD=@StaffCD)
	and ms.ChangeDate<=@Datetime
	order by ms.ChangeDate desc

END


