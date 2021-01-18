use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.D_TenjiI_Details_Update
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[D_TenjiI_Details_Update]
	-- Add the parameters for the stored procedure here
	
		@xml as xml ,
		@xml2 as xml ,
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
				,SKUCD varchar(400)
				,ShouName varchar(400)
				,Color varchar(10)
				,ColorName varchar(200)
				,Size varchar(10)
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
		----------------------------------------------------------------------------------A-- Update D_TenzikaiJuchuu
		update  D_TenzikaiJuchuu set
		--TenzikaiJuchuuNO
		JuchuuDate						   		 	=@JuchuuBi  		
		,SoukoCD						   		 	=@ShuuKaSouKo 	
		,StaffCD						   		 	=@TantouStaffu	
		,VendorCD						   		 	=@Shiiresaki 	
		,LastYearTerm					   		 	=@Nendo			
		,LastSeason						   		 	=@ShiZun			
		,CustomerCD						   		 	=@Kokyaku		
		,CustomerName					   		 	=@K_Name1		
		,CustomerName2					   		 	=@K_name2		
		,AliasKBN						   		 	=@K_radio		
		,ZipCD1							   		 	=@K_Zip1			
		,ZipCD2							   		 	=@K_Zip2			
		,Address1						   		 	=@K_Address1		
		,Address2						   		 	=@K_Address2		
		,Tel11							   		 	=@K_Denwa1	 	
		,Tel12							   		 	=@K_Denwa2	 	
		,Tel13							   		 	=@K_Denwa3	 	
		--,Tel21							   		 	=@KDenwa21	 	
		--,Tel22							   		 	=@KDenwa22		
		--,Tel23							   		 	=@KDenwa23		
		,CustomerKanaName				   		 	=@KkanaMei		
		,DeliveryCD						   		 	=@HaisoSaki		
		,DeliveryName					   		 	=@H_Name1	 	
		,DeliveryName2					   		 	=@H_name2	 	
		,DeliveryAliasKBN				   		 	=@H_radio	 	
		,DeliveryZipCD1					   		 	=@H_Zip1			
		,DeliveryZipCD2					   		 	=@H_Zip2			
		,DeliveryAddress1				   		 	=@H_Address1		
		,DeliveryAddress2				   		 	=@H_Address2		
		,DeliveryTel11					   		 	=@H_Denwa1		
		,DeliveryTel12					   		 	=@H_Denwa2	 	
		,DeliveryTel13					   		 	=@H_Denwa3	 	
		--,DeliveryTel21					   		 	=@HDenwa21	 	
		--,DeliveryTel22					   		 	=@HDenwa22	 	
		--,DeliveryTel23					   		 	=@HDenwa23	 	
		--,DeliveryKanaName				   		 	=@HkanaMei
		--,CurrencyCD						   			=(Select top 1 CurrencyCD from M_Control	Where MainKey =	1)		
		,JuchuuGaku						   		 	=@ZeiKomi		
		,HanbaiHontaiGaku				   		 	=@Zeinu			
		,HanbaiTax8						   		 	=@Keijen 		
		,HanbaiTax10					   		 	=@Tsuujou		
		,HanbaiGaku						   		 	=@ZeiKomiSou		
		,CostGaku						   		 	=@GenkaGaku		
		,ProfitGaku						   		 	=@ArariGaku 		
		,PaymentMethodCD				   		 	=@YoteiKinShu	
		,SalesPlanDate					   		 	=@UriageYoteiBi	
		--,JuchuuHurikaeZumiFLG			   		 	=@Sumi  			
		--,JuchuuHurikaeDateTime			   		 	=@Nichi			
		--,InsertOperator					   		 	=@InsertOpt		
		--,InsertDateTime					   		 	=@InsertDt		
		,UpdateOperator					   		 	=@InsertOpt		
		,UpdateDateTime					   		 	=@InsertDt		
		--,DeleteOperator					   		 	=@DeleteOpt		
		--,DeleteDateTime					   		 	=@DeleteDt		
			where TenzikaiJuchuuNO =@TenjiCD 


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
		set @LastRow = (select Isnull(Max(TenzikaiJuchuuRows),0)  from  D_TenzikaiJuchuuDetails);
		set @LastTenjiRow = (select Isnull(Max(TenzikaiJuchuuRows),0)  from  D_TenzikaiJuchuuDetails);
		--select @TenjiCD,@LastRow
		-----------------------------------------------------------------------------------B-Update D_TenzikaiJuchuuDetails
		Update  d set 
					-- TenzikaiJuchuuNO								=TenjiCD			
					--,TenzikaiJuchuuRows								=@LastRow + [No]
					--,DisplayRows									=[No]				
					 d.JanCD											=t.[SCJAN]				
					,d.AdminNO										=t.[AdminNo]			
					,d.SKUCD											=t.[SKUCD]				
					,d.SKUName										=t.[ShouName]			
					,d.ColorNO										=t.[Color]				
					,d.SizeNO											=t.[Size]			
					,d.ColorName										=t.[ColorName]				
					,d.SizeName										=t.[SizeName]			
					,d.JuchuuSuu										=t.[JuchuuSuu]			
					,d.JuchuuUnitPrice								=t.[HanbaiTanka]		
					,d.TaniCD											=t.[TenI]				
					,d.JuchuuGaku   									=t.[zeikomijuchuu]		
					,d.JuchuuHontaiGaku  								=t.[ZeinuJuchuu]		
					,d.JuchuuTax   									=(t.[Tsuujou] + t.[Keigen]) 
					,d.JuchuuTaxRitsu									=t.[ZeinuTanku]		
					,d.TaxRateFLG										=t.[TaxRateFlg]		
					,d.OrderUnitPrice									=t.[HacchuTanka]		
					,d.ProfitGaku										=t.[ArariGaku]			
					,d.SoukoCD										=t.[ShuukaSou]			
					,d.DirectFLG										=t.[ChoukuSou]			
					,d.ArrivePlanDate									=t.[NyuuKayo]			
					,d.CommentOutStore								=t.[ShanaiBi]			
					,d.CommentInStore									=t.[ShagaiBi]			
					,d.IndividualClientName							=t.[KobeTsu]			
					,d.ShippingPlanDate								=t.[ShuukaYo]		
																		
					--,InsertOperator									=@InsertOpt		
					--,InsertDateTime									=@InsertDt		
					,d.UpdateOperator									=@InsertOpt		
					,d.UpdateDateTime									=@InsertDt	
					--,DeleteOperator									=null		
					--,DeleteDateTime									=null		
						
						
					from D_TenzikaiJuchuuDetails d inner join  #tempTenji t on d.TenzikaiJuchuuNO = t.TenjiCD and  d.TenzikaiJuchuuRows = t.TenjiRow
		-----------------------------------------------------------------------------------B-Insert if not exist
		declare @lastUpdatedTenjiRow as int;
		set @lastUpdatedTenjiRow=(select max(TenzikaiJuchuuRows) from D_TenzikaiJuchuuDetails);
		insert into D_TenzikaiJuchuuDetails(
					 TenzikaiJuchuuNO
					,TenzikaiJuchuuRows
					,DisplayRows
					,JanCD
					,AdminNO
					,SKUCD
					,SKUName
					,ColorNO
					,SizeNO
					,ColorName
					,SizeName
					,JuchuuSuu
					,JuchuuUnitPrice
					,TaniCD
					,JuchuuGaku  -- komi
					,JuchuuHontaiGaku -- zeinu
					,JuchuuTax  -- tsuuJou + keijen
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
					,DeleteDateTime
						)
						select 
						 @TenjiCD			
						--,@LastRow + t.[No]
						,@LastRow + (ROW_NUMBER() OVER (order by t.[No]))
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
	 					,@InsertOpt		
		 				,@InsertDt		
		 				,@InsertOpt		
		 				,@InsertDt		
		 				,null		
		 				,null		
						from #tempTenji t WHERE 
						TenjiCD  in (select  TenzikaiJuchuuNO  FROM D_TenzikaiJuchuuDetails ) and 
						((TenjiRow not in (select  TenzikaiJuchuuRows  FROM D_TenzikaiJuchuuDetails ))
						 or (TenjiRow is null or TenjiRow ='') )
						
					
		-----------------------------------------------------------------------------------D-L_TenzikaiJuchuuDetailsHistory
		declare @LastRowLog as int,@LastRowSeq as int;
		set @LastRowLog = (select Isnull(Max(HistorySEQ),0)  from  L_TenzikaiJuchuuHistory);
		set @LastRowSeq = (select Isnull(Max(HistorySEQRows),0)  from  L_TenzikaiJuchuuDetailsHistory);


		insert Into L_TenzikaiJuchuuDetailsHistory    --//for Update
		 
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
						Case When @LastRowLog =0  then 1 else @LastRowLog end	---tr
						--,@LastRowSeq+ dt.DisplayRows
					,@LastRowSeq + (ROW_NUMBER() OVER (order by t.[No]))	---tr
					,t.TenjiCD												---tr
					
					--,@LastRow + t.[No]
					--,Case when dt.TenzikaiJuchuuRows is not null then dt.TenzikaiJuchuuRows else (select top 1 TenzikaiJuchuuRows from D_TenzikaiJuchuuDetails where TenzikaiJuchuuNO = dt.TenzikaiJuchuuNO  and DisplayRows = dt.DisplayRows ) end
				    , dt.TenzikaiJuchuuRows
					,dt.[DisplayRows]				
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
								from #tempTenji t 
								
								
								inner join D_TenzikaiJuchuuDetails dt on 
								t.TenjiCD = dt.TenzikaiJuchuuNO and (t.TenjiRow =dt.TenzikaiJuchuuRows )



						insert Into L_TenzikaiJuchuuDetailsHistory    --//for insert
		 
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

						Case When @LastRowLog =0  then 1 else @LastRowLog end	---tr
						,(select Isnull(Max(HistorySEQRows),0)  from  L_TenzikaiJuchuuDetailsHistory) +(ROW_NUMBER() OVER (order by TenzikaiJuchuuRows)) 
						,TenzikaiJuchuuNO
						,TenzikaiJuchuuRows
						,DisplayRows
						,JanCD
						,AdminNO
						,SKUCD
						,SKUName
						,ColorNO
						,SizeNO
						,ColorName
						,SizeName
						,JuchuuSuu
						,JuchuuUnitPrice
						,TaniCD
						,JuchuuGaku  -- komi
						,JuchuuHontaiGaku -- zeinu
						,JuchuuTax  -- tsuuJou + keijen
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
						,DeleteDateTime
						from D_TenzikaiJuchuuDetails where TenzikaiJuchuuNO=  @TenjiCD and  TenzikaiJuchuuRows >@lastUpdatedTenjiRow
		-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		DECLARE @DocHandle2 int
		EXEC sp_xml_preparedocument @DocHandle2 OUTPUT, @Xml2
		
		select * INTO #tempTenji2 FROM OPENXML (@DocHandle2, '/NewDataSet/test',2)
		WITH
		(
		TenjiRow int 
		 )
		EXEC sp_xml_removedocument @DocHandle2; 

		update D_TenzikaiJuchuuDetails set DeleteDateTime=@JuchuuBi  where TenzikaiJuchuuRows in (select TenjiRow from #tempTenji2) and TenzikaiJuchuuNO = @TenjiCD
		-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
			EXEC L_Log_Insert
					 @InsertOperator  
					,@Program        
					,@PC             
					,@OperateMode    
					,@KeyItem
					
		drop table #tempTenji
		drop table #tempTenji2
END
