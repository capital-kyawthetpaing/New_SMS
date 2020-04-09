 BEGIN TRY 
 Drop Procedure dbo.[M_StaffAccess]
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
CREATE PROCEDURE [dbo].[M_StaffAccess] 
	-- Add the parameters for the stored procedure here
	@StaffCD as varchar(15)  ,
	 @Password as varchar(50)
	 as
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

if not exists(select 1 from F_Staff(Getdate()) where StaffCD=@StaffCD ) 
	begin
		select 'E101' as MessageID
		return;
	end
	
	select top 1 MS.* 
	,CONVERT(VARCHAR,GETDATE(),111) sysDate
	into #temp
	from M_Staff MS
	where MS.StaffCD = @StaffCD
	AND MS.ChangeDate <= GETDATE()
	order by MS.ChangeDate desc

	--select * from #temp
	if not exists(select 1 from #temp where Password = @Password) 
	begin
		select 'E166' as MessageID
		drop table #temp
		return;
	end
	if  exists(select 1 from #temp where DeleteFlg = 1) 
	begin
		select 'E119' as MessageID
		drop table #temp
		return;
	end

	

		if  exists(select 1 from #temp where LeaveDate < GETDATE() and LeaveDate is not null  ) 
	begin
		select 'E135' as MessageID
		drop table #temp
		return;
	end

	 

	select 'Allow' as MessageID,* from #temp

	drop table #temp



END

