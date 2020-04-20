 BEGIN TRY 
 Drop Procedure dbo.[HMENU]
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
CREATE PROCEDURE [dbo].[HMENU]
	-- Add the parameters for the stored procedure here
@Staff_CD varchar(15),
@IsStore tinyint 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @IsStore =1
	Begin
--select distinct 
--mmd.BusinessID,mmd.BusinessSEQ,m.Char1,mp.ProgramName as ProgramID,mp.ProgramID as ProgramID_ID ,mmd.ProgramSEQ,mad.Insertable,mad.Updatable,mad.Deletable,mad.Inquirable,mad.Printable,mad.Outputable,mad.Runable
--from F_Staff(getdate()) fs
--left outer join F_Menu(getdate()) fm on fs.MenuCD = fm.MenuID
--left outer join M_MenuDetails mmd on mmd.MenuID = fs.StoreMenuCD
--left outer join M_AuthorizationsDetails mad on mad.AuthorizationsCD = fs.AuthorizationsCD
--left outer join M_MultiPorpose m on mmd.BusinessID= m.[Key] and m.ID='223' 
--left  outer join M_Program mp on mp.ProgramID = mmd.ProgramID 
--where fm.DeleteFlg = 0
--and fs.StaffCD = @Staff_CD
--and    (mad.Insertable	=	1
--or    mad.Updatable	=	    1
--or    mad.Deletable	=	    1
--or    mad.Inquirable=    	1
--or    mad.Printable	=	    1
--or    mad.Outputable=	    1
--or    mad.Runable	=	    1)




select * from (
select 
fmd.BusinessID,fmd.BusinessSEQ,m.Char1,mp.ProgramName as ProgramID,mp.ProgramID as ProgramID_ID,
fmd.ProgramSEQ,a.Insertable,a.Updatable,a.Deletable,a.Inquirable,a.Printable,a.Outputable,a.Runable
from M_menu fm
inner join F_Menu_Details(Getdate()) fmd on fm.MenuID = fmd.MenuID  and fm.DeleteFlg = 0
left join M_Program mp on fmd.ProgramID = mp.ProgramID 
left outer join M_MultiPorpose m on fmd.BusinessID= m.[Key] and m.ID='223' 
inner join 
(select fs.MenuCD,fs.StoreMenuCD, ProgramID,fad.Insertable,fad.Updatable,fad.Deletable,fad.Inquirable,fad.Printable,fad.Outputable,fad.Runable from
F_Authorizations(getdate()) fa
inner join F_AuthorizationsDetails(getdate()) fad on fa.AuthorizationsCD =  fad.AuthorizationsCD
inner join F_Staff(getdate()) fs on fs.AuthorizationsCD = fad.AuthorizationsCD
where StaffCD =  @Staff_CD
and 
(    fad.Insertable	=	1
or  fad.Updatable	=	    1
or  fad.Deletable	=	    1
or  fad.Inquirable=    	1
or  fad.Printable	=	    1
or  fad.Outputable=	    1
or  fad.Runable	=	    1  )

) a on a.StoreMenuCD = fmd.MenuID and mp.ProgramId= a.ProgramID ) b order by b.Businessseq , b.programseq asc
End
else
Begin

--select distinct 
--mmd.BusinessID,mmd.BusinessSEQ,m.Char1,mp.ProgramName as ProgramID,mp.ProgramID as ProgramID_ID ,mmd.ProgramSEQ,mad.Insertable,mad.Updatable,mad.Deletable,mad.Inquirable,mad.Printable,mad.Outputable,mad.Runable
--from F_Staff(getdate()) fs
--left outer join F_Menu(getdate()) fm on fs.MenuCD = fm.MenuID
--left outer join M_MenuDetails mmd on mmd.MenuID = fs.MenuCD
--left outer join M_AuthorizationsDetails mad on mad.AuthorizationsCD = fs.AuthorizationsCD
--left outer join M_MultiPorpose m on mmd.BusinessID= m.[Key] and m.ID='223' 
--left  outer join M_Program mp on mp.ProgramID = mmd.ProgramID 
--where fm.DeleteFlg = 0
--and fs.StaffCD = @Staff_CD
--and ( mad.Insertable	=	1
--or    mad.Updatable	=	    1
--or    mad.Deletable	=	    1
--or    mad.Inquirable=    	1
--or    mad.Printable	=	    1
--or    mad.Outputable=	    1
--or    mad.Runable	=	    1      )
select * from (
select 
fmd.BusinessID,fmd.BusinessSEQ,m.Char1,mp.ProgramName as ProgramID,mp.ProgramID as ProgramID_ID,
fmd.ProgramSEQ,a.Insertable,a.Updatable,a.Deletable,a.Inquirable,a.Printable,a.Outputable,a.Runable
from M_menu fm
inner join F_Menu_Details(Getdate()) fmd on fm.MenuID = fmd.MenuID  and fm.DeleteFlg = 0
left join M_Program mp on fmd.ProgramID = mp.ProgramID 
left outer join M_MultiPorpose m on fmd.BusinessID= m.[Key] and m.ID='223' 
inner join 
(select fs.MenuCD,fs.StoreMenuCD, ProgramID,fad.Insertable,fad.Updatable,fad.Deletable,fad.Inquirable,fad.Printable,fad.Outputable,fad.Runable from
F_Authorizations(getdate()) fa
inner join F_AuthorizationsDetails(getdate()) fad on fa.AuthorizationsCD =  fad.AuthorizationsCD
inner join F_Staff(getdate()) fs on fs.AuthorizationsCD = fad.AuthorizationsCD
where StaffCD =  @Staff_CD
and 
(    fad.Insertable	=	1
or  fad.Updatable	=	    1
or  fad.Deletable	=	    1
or  fad.Inquirable=    	1
or  fad.Printable	=	    1
or  fad.Outputable=	    1
or  fad.Runable	=	    1  )

) a on a.MenuCD = fmd.MenuID and mp.ProgramId= a.ProgramID ) b  order by b.Businessseq , b.programseq asc

End

END



--Update M_Staff set StoreMenuCD= MenuCD 
