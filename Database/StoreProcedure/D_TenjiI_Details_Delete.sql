use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.D_TenjiI_Details_Delete
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[D_TenjiI_Details_Delete]
	-- Add the parameters for the stored procedure here
	
		@xml as xml ,
		@TenjiCD as varchar(11),
		@JuchuuBi as date,
		@ShuuKaSouKo as varchar(20),
		@TantouStaffu as varchar(50),
		@Shiiresaki as varchar(50),
		@Nendo as varchar(10),
		@ShiZun as varchar(50),
		@Kokyaku as varchar(20),
		@K_Name1 as varchar(200),
		@K_name2 as varchar(200),
		@K_radio as tinyint,
		@K_Zip1 as varchar(200),
		@K_Zip2 as varchar(200),
		@K_Address1 as varchar(500),
		@K_Address2 as varchar(500)	,
		@K_Denwa1 as varchar(200),		
		@K_Denwa2 as  varchar(200),
		@K_Denwa3 as  varchar(200),
		@KDenwa21 as  varchar(200),
		@KDenwa22 as  varchar(200),
		@KDenwa23 as  varchar(200),
		@KkanaMei as  varchar(200),
		@HaisoSaki as  varchar(200),
		@H_Name1 as  varchar(200),
		@H_name2 as  varchar(200),
		@H_radio as tinyint,
		@H_Zip1 as  varchar(200),
		@H_Zip2 as  varchar(200),
		@H_Address1 as  varchar(200),
		@H_Address2 as  varchar(200),

		@H_Denwa1 as  varchar(200),	
		@H_Denwa2 as  varchar(200),
		@H_Denwa3 as  varchar(200),
		@HDenwa21 as  varchar(200),
		@HDenwa22 as  varchar(200),
		@HDenwa23 as  varchar(200),
		@HkanaMei as  varchar(200),

		@ZeiKomi as Money,
		@Zeinu as money,

		@Keijen as money,
		@Tsuujou as money,

		@ZeiKomiSou as Money,

		@GenkaGaku as Money,
		@ArariGaku as money,

		@YoteiKinShu as varchar(200),
		@UriageYoteiBi as datetime,

		@Sumi as tinyint,
		@Nichi as datetime,

		@InsertOpt as varchar(50),
		@InsertDt as datetime,
		@DeleteOpt as varchar(50),
		@DeleteDt as datetime,
		@StoreCD as varchar(20),
		@InsertOperator as varchar(50),
		@PC as varchar(50),
		@Program as varchar(50),
		@OperateMode as varchar(50),
		@KeyItem as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
		DECLARE @DocHandle int
		EXEC sp_xml_preparedocument @DocHandle OUTPUT, @Xml
		

		--------------------------------------------------------------------------------Temp
		select * INTO #tempTenji FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
		WITH
		(
				 TenjiRow int
				,TenjiCD varchar(13)
				,[No] int
				,GYONO tinyint
				,Chk tinyint
				,SCJAN varchar(13)
				,AdminNo int
				,SKUCD varchar(200)
				,ShouName varchar(200)
				,Color int
				,ColorName varchar(200)
				,Size int
				,SizeName varchar(200)
				,ShuukaYo date
				,ChoukuSou tinyint
				,ShuukaSou varchar(20)
				,HacchuTanka money
				,NyuuKayo date
				,JuchuuSuu money
				,TenI varchar(2)
				,TeniName varchar(100)
				,HanbaiTanka money
				,ZeinuJuchuu money
				,zeikomijuchuu money
				,ArariGaku money
				,ZeiNu varchar
				,ZeinuTanku varchar(10)
				,ShanaiBi varchar(200)		
				,ShagaiBi varchar(200)
				,KobeTsu varchar(500)
				,TorokuFlg tinyint
				,TaxRateFlg tinyint
				,Tsuujou money
				,Keigen money
				)
		EXEC sp_xml_removedocument @DocHandle; 
		--declare @TenjiCD as varchar(20);
		--	set @TenjiCD= (select dbo.[Fnc_TenjiGetNumber](13,getdate(),@StoreCD));
		----------------------------------------------------------------------------------A-- Delete D_TenzikaiJuchuu
		delete from D_TenzikaiJuchuu where TenzikaiJuchuuNO=@TenjiCD 


		-- select *  from  D_TenzikaiJuchuu
		----------------------------------------------------------------------------------C--L_TenzikaiJuchuuHistory
		insert into L_TenzikaiJuchuuHistory(
		 
		TenzikaiJuchuuNO
		,JuchuuDate
		,SoukoCD
		,StaffCD
		,VendorCD
		,LastYearTerm
		,LastSeason
		,CustomerCD
		,CustomerName
		,CustomerName2
		,AliasKBN
		,ZipCD1
		,ZipCD2
		,Address1
		,Address2
		,Tel11
		,Tel12
		,Tel13
		,Tel21
		,Tel22
		,Tel23
		,CustomerKanaName
		,DeliveryCD
		,DeliveryName
		,DeliveryName2
		,DeliveryAliasKBN
		,DeliveryZipCD1
		,DeliveryZipCD2
		,DeliveryAddress1
		,DeliveryAddress2
		,DeliveryTel11
		,DeliveryTel12
		,DeliveryTel13
		,DeliveryTel21
		,DeliveryTel22
		,DeliveryTel23
		,DeliveryKanaName
		,JuchuuGaku
		,HanbaiHontaiGaku
		,HanbaiTax8
		,HanbaiTax10
		,HanbaiGaku
		,CostGaku
		,ProfitGaku
		,PaymentMethodCD
		,SalesPlanDate
		,JuchuuHurikaeZumiFLG
		,JuchuuHurikaeDateTime
		,InsertOperator
		,InsertDateTime
		,UpdateOperator
		,UpdateDateTime
		,DeleteOperator
		,DeleteDateTime
		)
		values
		(
			@TenjiCD		
		 	,@JuchuuBi  		
		 	,@ShuuKaSouKo 	
		 	,@TantouStaffu	
		 	,@Shiiresaki 	
		 	,@Nendo			
		 	,@ShiZun			
		 	,@Kokyaku		
		 	,@K_Name1		
		 	,@K_name2		
		 	,@K_radio		
		 	,@K_Zip1			
		 	,@K_Zip2			
		 	,@K_Address1		
		 	,@K_Address2		
		 	,@K_Denwa1	 	
		 	,@K_Denwa2	 	
		 	,@K_Denwa3	 	
		 	,@KDenwa21	 	
		 	,@KDenwa22		
		 	,@KDenwa23		
		 	,@KkanaMei		
		 	,@HaisoSaki		
		 	,@H_Name1	 	
		 	,@H_name2	 	
		 	,@H_radio	 	
		 	,@H_Zip1			
		 	,@H_Zip2			
		 	,@H_Address1		
		 	,@H_Address2		
		 	,@H_Denwa1		
		 	,@H_Denwa2	 	
		 	,@H_Denwa3	 	
		 	,@HDenwa21	 	
		 	,@HDenwa22	 	
		 	,@HDenwa23	 	
		 	,@HkanaMei	
		 	,@ZeiKomi		
		 	,@Zeinu			
		 	,@Keijen 		
		 	,@Tsuujou		
		 	,@ZeiKomiSou		
		 	,@GenkaGaku		
		 	,@ArariGaku 		
		 	,@YoteiKinShu	
		 	,@UriageYoteiBi	
		 	,@Sumi  			
		 	,@Nichi			
		 	,@InsertOpt		
		 	,@InsertDt		
		 	,@InsertOpt		
		 	,@InsertDt		
		 	,null		
		 	,null	
		)
		 	--declare @TenjiCD1 as varchar(20),
			declare  @LastRow as int;
			declare @LastTenjiRow as int;
		--set @TenjiCD1 = (select dbo.[Fnc_TenjiGetNumber](13,getdate(),'0001'));
		set @LastRow = (select Isnull(Max(TenzikaiJuchuuRows),0)  from  D_TenzikaiJuchuuDetails where TenzikaiJuchuuNO = @TenjiCD);
		set @LastTenjiRow = (select Isnull(Max(TenzikaiJuchuuRows),0)  from  D_TenzikaiJuchuuDetails where TenzikaiJuchuuNO = @TenjiCD);
		--select @TenjiCD,@LastRow
		-----------------------------------------------------------------------------------B-Delete D_TenzikaiJuchuuDetails
		
		-----------------------------------------------------------------------------------B-Insert if not exist

		-----------------------------------------------------------------------------------D-L_TenzikaiJuchuuDetailsHistory
		declare @LastRowLog as int,@LastRowSeq as int;
		set @LastRowLog = (select Isnull(Max(HistorySEQ),0)  from  L_TenzikaiJuchuuHistory );
		set @LastRowSeq = (select Isnull(Max(HistorySEQRows),0)  from  L_TenzikaiJuchuuDetailsHistory );



		--insert Into L_TenzikaiJuchuuDetailsHistory 
		 
		--				(HistorySEQ
		--				,HistorySEQRows
		--				,TenzikaiJuchuuNO
		--				,TenzikaiJuchuuRows
		--				,DisplayRows
		--				,SKUCD
		--				,AdminNO
		--				,JanCD
		--				,SKUName
		--				,SizeNO
		--				,ColorNO
		--				,ColorName
		--				,SizeName
		--				,JuchuuSuu
		--				,JuchuuUnitPrice
		--				,TaniCD
		--				,JuchuuGaku
		--				,JuchuuHontaiGaku
		--				,JuchuuTax
		--				,JuchuuTaxRitsu
		--				,TaxRateFLG
		--				,OrderUnitPrice
		--				,ProfitGaku
		--				,SoukoCD
		--				,DirectFLG
		--				,ArrivePlanDate
		--				,CommentOutStore
		--				,CommentInStore
		--				,IndividualClientName
		--				,ShippingPlanDate
		--				,InsertOperator
		--				,InsertDateTime
		--				,UpdateOperator
		--				,UpdateDateTime
		--				,DeleteOperator
		--				,DeleteDateTime)
		--				select 
		--				Case When @LastRowLog =0  then 1 else @LastRowLog end
		--				,@LastRowSeq+ dt.DisplayRows
		--				,tj.TenzikaiJuchuuNO
		--				,tj.TenzikaiJuchuuRows
		--				,tj.DisplayRows
		--				,tj.SKUCD
		--				,tj.AdminNO
		--				,tj.JanCD
		--				,tj.SKUName
		--				,tj.SizeNO
		--				,tj.ColorNO
		--				,tj.ColorName
		--				,tj.SizeName
		--				,tj.JuchuuSuu
		--				,tj.JuchuuUnitPrice
		--				,tj.TaniCD
		--				,tj.JuchuuGaku
		--				,tj.JuchuuHontaiGaku
		--				,tj.JuchuuTax
		--				,tj.JuchuuTaxRitsu
		--				,tj.TaxRateFLG
		--				,tj.OrderUnitPrice
		--				,tj.ProfitGaku
		--				,tj.SoukoCD
		--				,tj.DirectFLG
		--				,tj.ArrivePlanDate
		--				,tj.CommentOutStore
		--				,tj.CommentInStore
		--				,tj.IndividualClientName
		--				,tj.ShippingPlanDate
		--				,tj.InsertOperator
		--				,tj.InsertDateTime
		--				,tj.UpdateOperator
		--				,tj.UpdateDateTime
		--				,tj.DeleteOperator
		--				,tj.DeleteDateTime
		--				from #tempTenji tj left outer join D_TenzikaiJuchuuDetails dt on tj.TenjiCD = dt.TenzikaiJuchuuNO
			insert Into L_TenzikaiJuchuuDetailsHistory 
		 
						(HistorySEQ
						,HistorySEQRows
						,TenzikaiJuchuuNO
						,TenzikaiJuchuuRows
						,DisplayRows
						,JanCD
						,AdminNO
						,SKUCD
						,SKUName
						,SizeNO
						,ColorNO
						,ColorName
						,SizeName
						,JuchuuSuu
						,JuchuuUnitPrice
						,TaniCD
						,JuchuuGaku
						,JuchuuHontaiGaku
						,JuchuuTax
						,JuchuuTaxRitsu
						,TaxRateFLG
						,OrderUnitPrice
						,ProfitGaku
						,SoukoCD
						,DirectFLG
						,ArrivePlanDate
						,CommentOutStore
						,CommentInStore
						,IndividualClientName
						,ShippingPlanDate
						,InsertOperator
						,InsertDateTime
						,UpdateOperator
						,UpdateDateTime
						,DeleteOperator
						,DeleteDateTime)
						select 
						Case When @LastRowLog =0  then 1 else @LastRowLog end
						,@LastRowSeq+ dt.DisplayRows
						 ,t.TenjiCD			
						--,@LastRow + t.[No]
					,dt.TenzikaiJuchuuRows--,t.[No]--	,@LastRow + t.[No]--(ROW_NUMBER() OVER (order by t.[No]))
						,t.[No]				
					,t.[SCJAN]				
					,t.[AdminNo]			
					 ,t.[SKUCD]
					,t.[ShouName]			
					,t.[Color]		
					,t.[Size]	
					,t.[ColorName]		
					,t.[SizeName]			
					,t.[JuchuuSuu]			
					,t.[HanbaiTanka]		
					,t.[TenI]				
					,t.[zeikomijuchuu]		
					,t.[ZeinuJuchuu]		
					,(t.[Tsuujou] + t.[Keigen]) as JuuChuuTax
					,t.[ZeinuTanku]		
					,t.[TaxRateFlg]		
					,t.[HacchuTanka]		
					,t.[ArariGaku]			
					,t.[ShuukaSou]			
					,t.[ChoukuSou]			
					,t.[NyuuKayo]			
					,t.[ShanaiBi]			
					,t.[ShagaiBi]			
					,t.[KobeTsu]			
					,t.[ShuukaYo]		
						,null		
						,null	
						,@InsertOperator		
						,@InsertDt		
						,null		
						,null
								from #tempTenji t left outer join D_TenzikaiJuchuuDetails dt on t.TenjiCD = dt.TenzikaiJuchuuNO and t.TenjiRow =dt.TenzikaiJuchuuRows
		

		delete from D_TenzikaiJuchuuDetails where TenzikaiJuchuuNO=@TenjiCD 
			EXEC L_Log_Insert
					 @InsertOperator  
					,@Program        
					,@PC             
					,@OperateMode    
					,@KeyItem   

		drop table #tempTenji


END
