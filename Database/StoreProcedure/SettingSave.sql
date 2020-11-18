 BEGIN TRY 
 Drop Procedure dbo.[SettingSave]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SettingSave]
	-- Add the parameters for the stored procedure here
	@StaffCD as varchar(10),
	@IName as varchar(200),
	@LName as varchar(200),
	@MName as varchar(200),
	@IconName as varbinary(max),
	@L_LogoName as varbinary(max),
	@M_LogoName as varbinary(max),
	
	@ThemeKBN as varchar(100),
	@MenuKBN as tinyint,
	@FSKBN as tinyint,
	@FWKBN as tinyint,
	
	@M_HoverKBN as tinyint,
	@M_NormalKBN as tinyint,
	@HTopic as varchar(100),
	@Setting_Path as varchar(100),
	@PC as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
			--@StaffCD as varchar(10)='0001',
			--@IconName as varchar(100)='CAPITALSMS/1I_20201111_090908.ico',
			--@L_LogoName as varchar(100)='CAPITALSMS/1L_20201111_090901.PNG',
			--@M_LogoName as varchar(100)='CAPITALSMS/1M_20201111_090905.jpg',

			--@ThemeKBN as varchar(100)='ffffff80',
			--@MenuKBN as tinyint=1,
			--@FSKBN as tinyint=1,
			--@FWKBN as tinyint=1,

			--@M_HoverKBN as tinyint=1,
			--@M_NormalKBN as tinyint=1,
			--@HTopic as varchar(100)='',
			--@Setting_Path as varchar(100)='Setting/CAPITALSMS',
			--@PC as varchar(100)='MYA_40PC'


			---declare 
			--@Date as datetime ;
	--	Set --	@Date= getdate();
	   
	 --  delete from M_setting where StaffCD= @StaffCD and MenuKBN = @MenuKBN
		--insert into M_Setting (StaffCD,IconName,L_LogoName,M_LogoName, IconCD,L_LogoCD,M_LogoCD,ThemeKBN,MenuKBN,FSKBN,FWKBN,M_HoverKBN,M_NormalKBN,HTopic,Setting_Path,PC,DefaultKBN,InsertDateTime,InsertOperator,UpdateDateTime,UpdateOperator )
			--	values(@StaffCD,@IName,@LName,@MName,@IconName,@L_LogoName,@M_LogoName,@ThemeKBN,@MenuKBN,@FSKBN,@FWKBN,@M_HoverKBN,@M_NormalKBN,@HTopic,@Setting_Path,@PC,0,@Date,@StaffCD,@Date,@StaffCD)
						declare 
			@Date as datetime ,
			@AdminKBN as int , @SettingKBN as int , @DefaultKBN as int; 
		Set @Date= getdate();
		set @AdminKBN = (select top 1 AdminKBN from  M_setting where StaffCD= @StaffCD and MenuKBN = @MenuKBN);
		set @SettingKBN = (select top 1 SettingKBN from  M_setting where StaffCD= @StaffCD and MenuKBN = @MenuKBN);
		set @DefaultKBN = (select top 1 DefaultKBN from  M_setting where StaffCD= @StaffCD and MenuKBN = @MenuKBN);
		
	   delete from M_setting where StaffCD= @StaffCD and MenuKBN = @MenuKBN
		insert into M_Setting (StaffCD,AdminKBN, SettingKBN , IconName,L_LogoName,M_LogoName, IconCD,L_LogoCD,M_LogoCD,ThemeKBN,MenuKBN,FSKBN,FWKBN,M_HoverKBN,M_NormalKBN,HTopic,Setting_Path,PC,DefaultKBN,InsertDateTime,InsertOperator,UpdateDateTime,UpdateOperator )
				values(@StaffCD,@AdminKBN,@SettingKBN, @IName,@LName,@MName,@IconName,@L_LogoName,@M_LogoName,@ThemeKBN,@MenuKBN,@FSKBN,@FWKBN,@M_HoverKBN,@M_NormalKBN,@HTopic,@Setting_Path,@PC,0,@Date,@StaffCD,@Date,@StaffCD)

END