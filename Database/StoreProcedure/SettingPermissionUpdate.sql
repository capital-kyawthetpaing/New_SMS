 BEGIN TRY 
 Drop Procedure dbo.[SettingPermissionUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE  [dbo].[SettingPermissionUpdate]
	-- Add the parameters for the stored procedure here

@StaffCD as varchar(4),
@Date as  Datetime ,
@Setting_Path as varchar(200),
@PC as varchar(100) ,
@Xml as xml
AS
BEGIN

DECLARE @DocHandle int
		EXEC sp_xml_preparedocument @DocHandle OUTPUT, @Xml
		

		--------------------------------------------------------------------------------Temp
		select * INTO #temp FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
		WITH
		(
		MenuKBN varchar(50),
		StaffCD varchar(10),
		StaffName varchar(200),
		UpdateDateTime datetime,
		AdminKBN bit,
		SettingKBN bit,
		DefaultKBN bit,
		colAll bit,
		LastDate datetime
	)
		EXEC sp_xml_removedocument @DocHandle; 
		update #temp set LastDate = @Date
		delete  from #temp where colAll <> 1

		Update ms set ms.AdminKBN = tp.AdminKBN ,
		 ms.SettingKBN = tp.SettingKBN 
		 , ms.DefaultKBN = tp.DefaultKBN
		 , UpdateDateTime = tp.LastDate 
		 ,ms.UpdateOperator = @StaffCD

	--	select tp.* 
		from M_Setting ms
		inner join #temp tp on ms.StaffCD = tp.StaffCd and ms.MenuKBN = (Case when tp.MenuKBN = 'MainMenu' then 1 else 2 end)

		insert into M_Setting (
						StaffCD , SettingKBN , AdminKBN , DefaultKBN, IconCD, L_LogoCD, M_LogoCD, ThemeKBN, MenuKBN  , FSKBN , TCKBN, FWKBN, M_HoverKBN, M_NormalKBN , HTCKBN
					 , HTopic, Setting_Path , PC, InsertDateTime,InsertOperator,UpdatedateTime,UpdateOperator )

					  select 
					  tp.StaffCd, tp.SettingKBN, tp.AdminKBN ,tp.DefaultKBN, (select top 1 [IMage] from M_SettingIMage where SettingImageCD =1) ,  (select top 1 [IMage] from M_SettingIMage where SettingImageCD =2) ,
					   (select top 1 [IMage] from M_SettingIMage where SettingImageCD =3) ,  'LightGray' , 1 , 3,null,1, 1,1,null, 
					   null, @Setting_Path, @PC , tp.LastDate , @StaffCD , tp.LastDate , @StaffCD 
					  from 
		M_Setting ms right join #temp tp on ms.StaffCD = tp.StaffCd and ms.MenuKBN = (Case when tp.MenuKBN = 'MainMenu' then 1 else 2 end) where ms.StaffCD is null and ms.MenuKBN is null
		drop table #temp
END
