 BEGIN TRY 
 Drop Procedure dbo.[SettingGetAllPermission]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SettingGetAllPermission]
	-- Add the parameters for the stored procedure here
	@StaffCD as varchar(50) ,
	@Admin as tinyint ,
	@Setting as tinyint ,
	@Default as tinyint 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	--declare
	--@StaffCD as varchar(50) ,
	--@Admin as varchar(1) ,
	--@Setting as varchar(1),
	--@Default as varchar(1)


    select *  from  (select distinct Cast( ms.AdminKBN as bit) as Admin,Cast( ms.SettingKBN as bit) as Setting, Cast( ms.DefaultKBN as bit) as [Default], 
	case when ms.MenuKBN =1 then 'MainMenu' else 'StoreMenu' End as  MenuKBN, fs.StaffCD, fs.StaffName, Convert (varchar, ms.UpdateDateTime,111) as  UpdateDateTime from
						F_Authorizations(getdate()) fa
						inner join F_AuthorizationsDetails(getdate()) fad on fa.AuthorizationsCD =  fad.AuthorizationsCD
						inner join F_Staff(getdate()) fs on fs.AuthorizationsCD = fad.AuthorizationsCD and ((fs.StaffCD =@StaffCD) or @StaffCD is null )
						left  join M_Setting ms on ms.StaffCD = fs.StaffCD and ms.DeleteFlg = 0	  ) a
						--where a.MenuKBN is not null
						where 
						
						(@Admin = 0 or ([Admin] =  @Admin))
						and  (@Setting =0  or (Setting  = @Setting))
						and (@Default =0  or ([Default]  = @Default))

						 order by  a.UpdateDateTime desc


END
