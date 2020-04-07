 BEGIN TRY 
 Drop Procedure dbo.[M_Authorizations_AccessCheck]
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
CREATE PROCEDURE [dbo].[M_Authorizations_AccessCheck] 
	-- Add the parameters for the stored procedure here
	@ProgramID varchar(30),
	@StaffCD varchar(10),
	@PC varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists(select 1 from M_Program where ProgramID = @ProgramID) 
	begin
		select 'S102' as MessageID
		return;
	end

	if not exists (
	select 1 from F_Authorizations(getdate()) fa
	inner join F_Staff(getdate()) fs on fa.AuthorizationsCD = fs.AuthorizationsCD
	where fs.StaffCD = @StaffCD)

	--select 1 from M_AuthorizationsDetails mad
	--inner join V_Authorizations va on va.AuthorizationsCD = mad.AuthorizationsCD and va.ChangeDate = mad.ChangeDate
	--inner join (select ms.* from M_Staff ms inner join V_Staff vs on ms.StaffCD = vs.StaffCD and ms.ChangeDate = vs.ChangeDate) s on s.AuthorizationsCD = va.AuthorizationsCD 
	--where s.StaffCD = @StaffCD)

	begin
		select 'S003' As MessageID
		return;
	end

	select Insertable,Updatable,Deletable,Inquirable,Printable,Outputable,Runable,
		   case when mp.[Type] = 1 and Insertable = 0 and Updatable = 0 and Deletable = 0 and Inquirable = 0 then 'S003' 
				when mp.[Type] = 2 and Printable = 0 then 'S003'
				when mp.[Type] = 3 and Printable = 0 and Outputable = 0 then 'S003'
				when mp.[Type] = 4 and Outputable = 0 then 'S003'
				when mp.[Type] = 5 and Inquirable = 0 then 'S003'
				when mp.[Type] = 6 and Runable = 0 then 'S003'
				else 'allow' end as MessageID,
				mp.ProgramID,
				mp.ProgramName,
				mp.Type,			
				fs.StoreAuthorizationsCD,
				 CONVERT(varchar(10),fs.ChangeDate,111)	as ChangeDate
	from F_AuthorizationsDetails(getdate()) fad --inner join
	--V_Authorizations va on va.AuthorizationsCD = mad.AuthorizationsCD
	inner join M_Program mp on mp.ProgramID = fad.ProgramID
	inner join F_Staff(getdate()) fs on fs.AuthorizationsCD = fad.AuthorizationsCD
	--(select ms.* from M_Staff ms inner join V_Staff vs on ms.StaffCD = vs.StaffCD and ms.ChangeDate = vs.ChangeDate) s on s.AuthorizationsCD = va.AuthorizationsCD 
	where fs.StaffCD = @StaffCD
	and mp.ProgramID = @ProgramID

	declare @OperateDate as date = convert(date, getdate()),
			@OperateTime as time = cast(getdate() as time) 

	exec dbo.L_Log_Insert @StaffCD,@ProgramID,@PC,'Open',NULL
END

