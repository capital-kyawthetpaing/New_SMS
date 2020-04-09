 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Insert]
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
CREATE PROCEDURE [dbo].[D_Pay_Insert]
	-- Add the parameters for the stored procedure here		

		@StaffCD varchar(10),
		@PayDate date,	
		@StoreCD varchar(6),
		@Operator varchar(10),
		@Program as varchar(20),
		@PC as varchar(10),
		@Xml1 as Xml,
		@Xml2 as Xml,
		@Xml3 as Xml,
		@TotalPayGaku as money,
		@OperateMode as Varchar(10)
		

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare 
	--@PayNO as varchar(11),@LargePayNO as varchar(11),
	@DateTime datetime=getdate(), @PayDate1 varchar(10)=CONVERT(VARCHAR(10), @PayDate, 111)


	--テーブル転送仕様Ａ
	--EXEC Fnc_GetNumber
 --           9,-------------in伝票種別 
 --           @PayDate1,----in基準日
 --           @StoreCD,
 --           @Operator,
 --           @PayNO OUTPUT
 --           ;

	--	 IF ISNULL(@PayNO,'') = ''
 --           BEGIN
 --               RETURN '1';
 --           END

	--EXEC Fnc_GetNumber
 --           18,-------------in伝票種別 
 --           @PayDate1,----in基準日
 --           @StoreCD,
 --           @Operator,
 --           @LargePayNO OUTPUT
 --           ;

	--	 IF ISNULL(@LargePayNO,'') = ''
 --           BEGIN
 --               RETURN '1';
 --           END

	CREATE TABLE [dbo].[#tempPay]
	(
				PayNO varchar(11),
				LargePayNO varchar(11),
				StaffName varchar(6),
				KouzaCD varchar(3),
				PayeeCD varchar(13),
				VendorName varchar(50) collate Japanese_CI_AS ,
				PayPlanDate date,
				PayPlanGaku money,
				PayConfirmGaku money,
				PayGaku money,
				TransferGaku money,
				TransferFeeGaku money,
				FeeKBN tinyint,
				Gaku money,
				PayPlan money,
				PayCloseNO varchar(11) collate Japanese_CI_AS ,
				PayCloseDate date,
				HontaiGaku8 money,
				HontaiGaku10 money,
				TaxGaku8 money,
				TaxGaku10 money	,
				BankCD varchar(4),				
				BranchCD varchar(3),				
				KouzaKBN tinyint,
				KouzaNO varchar(7),
				KouzaMeigi varchar(40),				
				CashGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20),
				ERMCGaku money,
				ERMCNO varchar(20),
				ERMCDate date,
				CardGaku money,
				OffsetGaku money,				
				OtherGaku1 money,
				Account1 varchar(10),
				SubAccount1 varchar(10),
				OtherGaku2 money,
				Account2 varchar(10),
				SubAccount2 varchar(10)
									
	)

		DECLARE @DocHandle int
		EXEC sp_xml_preparedocument @DocHandle OUTPUT, @Xml1
		
		INSERT INTO #tempPay select * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
		WITH(PayNO varchar(11),
				LargePayNO varchar(11),
				StaffName varchar(6),
				KouzaCD varchar(3),
				PayeeCD varchar(13),
				VendorName varchar(50),
				PayPlanDate date,
				PayPlanGaku money,
				PayConfirmGaku money,
				PayGaku money,
				TransferGaku money,
				TransferFeeGaku money,
				FeeKBN tinyint,
				Gaku money,
				PayPlan money,
				PayCloseNO varchar(11),
				PayCloseDate date,
				HontaiGaku8 money,
				HontaiGaku10 money,
				TaxGaku8 money,
				TaxGaku10 money,
				BankCD varchar(4),				
				BranchCD varchar(3),				
				KouzaKBN tinyint,
				KouzaNO varchar(7),
				KouzaMeigi varchar(40),				
				CashGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20),
				ERMCGaku money,
				ERMCNO varchar(20),
				ERMCDate date,
				CardGaku money,
				OffsetGaku money,
				OtherGaku1 money,
				Account1 varchar(10),
				SubAccount1 varchar(10),
				OtherGaku2 money,
				Account2 varchar(10),
				SubAccount2 varchar(10)
				) 
		EXEC sp_xml_removedocument @DocHandle; 


		CREATE TABLE [dbo].[#tempPay2]
			(
				PayPlanDate date,
				PayeeCD varchar(13),
				VendorName varchar(50) collate Japanese_CI_AS ,
				BankCD varchar(4),
				BankName varchar(30),
				BranchCD varchar(3),
				BranchName varchar(30),
				KouzaKBN tinyint,
				KouzaNO varchar(7),
				KouzaMeigi varchar(40),
				FeeKBN tinyint,
				Fee varchar(10),
				CashGaku money,
				OffsetGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20),
				ERMCGaku money,
				ERMCNO varchar(20),
				ERMCDate date,
				OtherGaku1 money,
				Account1 varchar(10),
				start1 varchar(30),
				SubAccount1 varchar(10),
				end1label varchar(30),
				OtherGaku2 money,
				Account2 varchar(10),
				start2 varchar(30),
				SubAccount2 varchar(10),
				end2label varchar(30)
			)
		DECLARE @DocHandle2 int

		EXEC sp_xml_preparedocument @DocHandle2 OUTPUT, @Xml2
		
		INSERT INTO #tempPay2 select * FROM OPENXML (@DocHandle2, '/NewDataSet/test',2)
		WITH(
				PayPlanDate date,
				PayeeCD varchar(13),
				VendorName varchar(50),
				BankCD varchar(4),
				BankName varchar(30),
				BranchCD varchar(3),
				BranchName varchar(30),
				KouzaKBN tinyint,
				KouzaNO varchar(7),
				KouzaMeigi varchar(40),
				FeeKBN tinyint,
				Fee varchar(10),
				CashGaku money,
				OffsetGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20),
				ERMCGaku money,
				ERMCNO varchar(20),
				ERMCDate date,
				OtherGaku1 money,
				Account1 varchar(10),
				start1 varchar(30),
				SubAccount1 varchar(10),
				end1label varchar(30),
				OtherGaku2 money,
				Account2 varchar(10),
				start2 varchar(30),
				SubAccount2 varchar(10),
				end2label varchar(30)	
				--CardGaku money,
				) 
		EXEC sp_xml_removedocument @DocHandle2; 

		update #tempPay 
		set BankCD = d2.BankCD ,
		BranchCD = d2.BranchCD,
		KouzaKBN = d2.KouzaKBN,
		KouzaNO = d2.KouzaNO,
		KouzaMeigi = d2.KouzaMeigi,
		CashGaku = d2.CashGaku,
		BillGaku = d2.BillGaku,
		BillDate = d2.BillDate,
		BillNO = d2.BillNO,
		ERMCGaku = d2.ERMCGaku,
		ERMCNO = d2.ERMCNO,
		ERMCDate = d2.ERMCDate,
		CardGaku = '0',
		OffsetGaku = d2.OffsetGaku,
		OtherGaku1 = d2.OtherGaku1,
		Account1 = d2.Account1,
		--start1 = d2.start1,
		SubAccount1 = d2.SubAccount1,
		--end1label = d2.end1label,
		OtherGaku2 = d2.OtherGaku2,
		Account2 = d2.Account2,
		--start2 = d2.start2,
		SubAccount2 = d2.SubAccount2
		--end2label = d2.end2label
		from #tempPay d1 inner join #tempPay2 d2 
		on d2.PayeeCD = d1.PayeeCD and d2.PayPlanDate = d1.PayPlanDate


		CREATE TABLE [dbo].[#tempPay3]
		(
			PayPlanNO int,
			PayPlanDate date,
			PayeeCD varchar(13),
			Number varchar(11),
			RecordedDate date,
			PayPlanGaku money,
			PayConfirmGaku money,
			UnpaidAmount1 money,
			UnpaidAmount2 money,
			PayPlanGaku1 money
		)
		DECLARE @DocHandle3 int

		EXEC sp_xml_preparedocument @DocHandle3 OUTPUT, @Xml3

		INSERT INTO #tempPay3 select * FROM OPENXML (@DocHandle3, '/NewDataSet/test',2)
		WITH (
				PayPlanNO int,
				PayPlanDate date,
				PayeeCD varchar(13),
				Number varchar(11),
				RecordedDate date,
				PayPlanGaku money,
				PayConfirmGaku money,
				UnpaidAmount1 money,
				UnpaidAmount2 money,
				PayPlanGaku1 money
		)
		EXEC sp_xml_removedocument @DocHandle3;
		
		DECLARE	 @PayNO VARCHAR(11),
				 @LargePayNO VARCHAR(11),
				 @StaffName VARCHAR(6),
				 @KouzaCD VARCHAR(3),
				 @PayeeCD VARCHAR(13) ,
				 @VendorName VARCHAR(50),
				 @PayPlanDate DATE ,
				 @PayPlanGaku 	MONEY,
				 @PayConfirmGaku 	MONEY,
				 @PayGaku 	MONEY,
				 @TransferGaku 	MONEY,
				 @TransferFeeGaku 	MONEY,
				 @FeeKBN TINYINT,
				 @Gaku 	MONEY,
				 @PayPlan 	MONEY,
				 @PayCloseNO VARCHAR(11),
				 @PayCloseDate DATE,
				 @HontaiGaku8 	MONEY,
				 @HontaiGaku10 	MONEY,
				 @TaxGaku8 	MONEY,
				 @TaxGaku10 	MONEY,
				 @BankCD varchar(4),				
				 @BranchCD varchar(3),
				 @KouzaKBN tinyint,
				 @KouzaNO varchar(7),
				 @KouzaMeigi varchar(40),
				 @CashGaku money,
				 @BillGaku money,
				 @BillDate date,
				 @BillNO varchar(20),
				 @ERMCGaku money,
				 @ERMCNO varchar(20),
				 @ERMCDate date,
				 @CardGaku money,
				 @OffsetGaku money,
				 @OtherGaku1 money,
				 @Account1 varchar(10),
				 --@start1 varchar(30),
				 @SubAccount1 varchar(10),
				 --@end1label varchar(30),
				 @OtherGaku2 money,
				 @Account2 varchar(10),
				 --@start2 varchar(30),
				 @SubAccount2 varchar(10)	
				 --@end2label varchar(30)
		
		DECLARE Location_Cursor CURSOR FOR 
		    SELECT PayNO ,
				   LargePayNO ,
				   StaffName ,
				   KouzaCD ,
				   PayeeCD  ,
				   VendorName,
				   PayPlanDate ,
				   PayPlanGaku,
				   PayConfirmGaku,
				   PayGaku,
				   TransferGaku,
				   TransferFeeGaku,
				   FeeKBN,
				   Gaku,
				   PayPlan,
				   PayCloseNO,
				   PayCloseDate,
				   HontaiGaku8,
				   HontaiGaku10,
				   TaxGaku8,
				   TaxGaku10,
				   BankCD ,				
				   BranchCD,
				   KouzaKBN ,
				   KouzaNO,
				   KouzaMeigi,
				   CashGaku,
				   BillGaku ,
				   BillDate,
				   BillNO,
				   ERMCGaku ,
				   ERMCNO,
				   ERMCDate,
				   CardGaku,
				   OffsetGaku,
				   OtherGaku1,
				   Account1,
				   --start1,
				   SubAccount1,
				   --end1label,
				   OtherGaku2,
				   Account2,
				   --start2,
				   SubAccount2
				   --end2label
		
		  FROM #tempPay 

		OPEN Location_Cursor
		
		FETCH NEXT FROM Location_Cursor INTO
						 @PayNO ,
						 @LargePayNO ,
						 @StaffName ,
						 @KouzaCD ,
						 @PayeeCD  ,
						 @VendorName,
						 @PayPlanDate ,
						 @PayPlanGaku,
						 @PayConfirmGaku,
						 @PayGaku,
						 @TransferGaku,
						 @TransferFeeGaku,
						 @FeeKBN,
						 @Gaku,
						 @PayPlan,
						 @PayCloseNO,
						 @PayCloseDate,
						 @HontaiGaku8,
						 @HontaiGaku10,
						 @TaxGaku8,
						 @TaxGaku10,
						 @BankCD ,				
						 @BranchCD,
						 @KouzaKBN ,
						 @KouzaNO,
						 @KouzaMeigi,
						 @CashGaku,
						 @BillGaku ,
						 @BillDate,
						 @BillNO,
						 @ERMCGaku ,
						 @ERMCNO,
						 @ERMCDate,
						 @CardGaku,
						 @OffsetGaku,
						 @OtherGaku1,
						 @Account1,
						 @SubAccount1,
						 @OtherGaku2,
						 @Account2,
						 @SubAccount2 	
		
		WHILE @@FETCH_STATUS = 0
		BEGIN		
			EXEC Fnc_GetNumber
		            9,-------------in伝票種別 
		            @PayDate1,----in基準日
		            @StoreCD,
		            @Operator,
		            @PayNO OUTPUT
		            ;
				if(@PayNO is null) 
				break
		
				EXEC Fnc_GetNumber
		            18,-------------in伝票種別 
		            @PayDate1,----in基準日
		            @StoreCD,
		            @Operator,
		            @LargePayNO OUTPUT
		            ;
				if(@LargePayNO is null) 
				break
				
---SheetA
				Insert into D_Pay(
				PayNO
				,LargePayNO
				,PayCloseNO
				,PayCloseDate
				,PayeeCD
				,InputDateTime
				,StaffCD
				,PayDate
				,PayPlanDate
				,HontaiGaku8
				,HontaiGaku10
				,TaxGaku8
				,TaxGaku10
				,PayGaku
				,TransferGaku
				,TransferFeeGaku
				,FeeKBN
				,MotoKouzaCD
				,BankCD ,				
				 BranchCD,
				 KouzaKBN ,
				 KouzaNO,
				 KouzaMeigi,
				 CashGaku,
				 BillGaku ,
				 BillDate,
				 BillNO,
				 ERMCGaku ,
				 ERMCNO,
				 ERMCDate,
				 CardGaku,
				 OffsetGaku,
				 OtherGaku1,
				 Account1,
				 SubAccount1,
				 OtherGaku2,
				 Account2,
				 SubAccount2
				,FBCreateDate
				,FBCreateNO
				,InsertOperator
				,InsertDateTime
				,UpdateOperator
				,UpdateDateTime
				,DeleteOperator
				,DeleteDateTime
				)
				select
				 @PayNO
				,@LargePayNO
				,@PayCloseNO
				,@PayCloseDate
				,@PayeeCD
				,@DateTime
				,@StaffCD
				,@PayDate
				,@PayPlanDate
				,@HontaiGaku8
				,@HontaiGaku10
				,@TaxGaku8
				,@TaxGaku10
				,@PayGaku
				,@TransferGaku
				,@TransferFeeGaku
				,@FeeKBN
				,@KouzaCD 
				,@BankCD ,				
				 @BranchCD,
				 @KouzaKBN ,
				 @KouzaNO,
				 @KouzaMeigi,
				 @CashGaku,
				 @BillGaku ,
				 @BillDate,
				 @BillNO,
				 @ERMCGaku ,
				 @ERMCNO,
				 @ERMCDate,
				 @CardGaku,
				 @OffsetGaku,
				 @OtherGaku1,
				 @Account1,
				 @SubAccount1,
				 @OtherGaku2,
				 @Account2,
				 @SubAccount2
				,NULL
				,NULL
				,@Operator
				,@DateTime
				,@Operator
				,@DateTime
				,NULL
				,NULL
				
---SheetB
				Insert into D_PayDetails(
				PayNO,
				PayNORows,
				PayPlanNO,
				PayGaku,
				InsertOperator,
				InsertDateTime,
				UpdateOperator,
				UpdateDateTime,
				DeleteOperator,
				DeleteDateTime)
				select 
				@PayNO,
				row_number() over (order by (select NULL)),
				PayPlanNO,
				PayPlanGaku,
				@Operator,
				@DateTime,
				@Operator,
				@DateTime,
				Null,
				Null
				from #tempPay3
				where PayeeCD=@PayeeCD
				and PayPlanDate=@PayPlanDate

---SheetC,D
				exec dbo.L_PayHistory_Insert @PayNO,@LargePayNO

---SheetE				
				update  D_PayPlan  
				Set PayConfirmGaku = t3.UnpaidAmount1, 
				PayConfirmFinishedKBN = (case when convert(int,t3.UnpaidAmount2) = 0 then 1 else 0 end ),
				UpdateOperator = @Operator,
				UpdateDateTime = @DateTime
				from D_PayPlan as dpp inner join #tempPay3 t3
					on t3.PayPlanNO=  dpp.PayPlanNO
				--where dpp.PayeeCD = @PayeeCD and dpp.PayPlanDate = @PayPlanDate

---SheetF
				update  D_PayCloseHistory  
				Set ProcessingKBN = (case when convert(int,t1.PayConfirmGaku) <> 0 then 3 else 1 end ), 			
				UpdateOperator = @Operator,
				UpdateDateTime = @DateTime
				from D_PayCloseHistory as dpc inner join #tempPay t1
					on t1.PayCloseNO collate Japanese_CI_AS =  dpc.PayCloseNO
				--where dpc.PayeeCD = @PayeeCD and dpc.PayCloseDate = @PayPlanDate
			
---sheetG
				 IF EXISTS (select * from D_MonthlyDebt where PayeeCD = @PayeeCD and StoreCD = @StoreCD and YYYYMM = Cast(CONVERT(varchar(6),@PayDate, 112) as int))
						BEGIN
							Update D_MonthlyDebt
							Set PayGaku = ISNULL(dmd.PayGaku +@TotalPayGaku,0),
							OffsetGaku = ISNULL(dmd.OffsetGaku+t2.OffsetGaku,0), 
							BalanceGaku = ISNULL(LastBalanceGaku,0) + ISNULL(dmd.DebtGaku,0) - ISNULL(PayGaku,0) + ISNULL(@TotalPayGaku,0),
							UpdateOperator = @Operator,
							UpdateDateTime = @DateTime
							From D_MonthlyDebt as dmd inner join #tempPay2 t2 
							on t2.PayeeCD collate Japanese_CI_AS = dmd.PayeeCD
							where dmd.PayeeCD = @PayeeCD and dmd.StoreCD = @StoreCD and dmd.YYYYMM = Cast(CONVERT(varchar(6),@PayDate, 112) as int)						
						END
					ELSE
						BEGIN
							 Insert INTO D_MonthlyDebt (
							 YYYYMM,
							 StoreCD,
							 PayeeCD,
							 PayGaku,
							 OffsetGaku,
							 BalanceGaku,
							 InsertOperator,
							 InsertDateTime)
							 Select DISTINCT
							 Cast(CONVERT(varchar(6),@PayDate, 112) as int),
							 @StoreCD,
							 @PayeeCD,
							 ISNULL(dmd.PayGaku,0) +ISNULL(@TotalPayGaku,0),
							 ISNULL(dmd.OffsetGaku,0)+ISNULL(t2.OffsetGaku,0),
							 ISNULL(dmd.LastBalanceGaku,0) + ISNULL(dmd.DebtGaku,0) - ISNULL(dmd.PayGaku,0) + ISNULL(@TotalPayGaku,0),
							 @Operator,
							 @DateTime
							 From D_MonthlyDebt as dmd inner join  #tempPay2 t2 
							 on t2.PayeeCD collate Japanese_CI_AS = dmd.PayeeCD 
						END

---SheetZ
				Insert into L_Log(
				--SEQ,
				OperateDate,
				OperateTime,
				InsertOperator,
				Program,
				PC,
				OperateMode,
				KeyItem)
				select 
				--IDENT_CURRENT('L_Log') AS SEQ,
				CONVERT(VARCHAR(10), getdate(), 111),
				CONVERT(VARCHAR(10), getdate(), 108),
				@Operator,
				@Program,
				@PC,
				@OperateMode,
				@PayNO
		
		FETCH NEXT FROM Location_Cursor INTO
						 @PayNO ,
						 @LargePayNO ,
						 @StaffName ,
						 @KouzaCD ,
						 @PayeeCD  ,
						 @VendorName,
						 @PayPlanDate ,
						 @PayPlanGaku,
						 @PayConfirmGaku,
						 @PayGaku,
						 @TransferGaku,
						 @TransferFeeGaku,
						 @FeeKBN,
						 @Gaku,
						 @PayPlan,
						 @PayCloseNO,
						 @PayCloseDate,
						 @HontaiGaku8,
						 @HontaiGaku10,
						 @TaxGaku8,
						 @TaxGaku10,
						 @BankCD ,				
						 @BranchCD,
						 @KouzaKBN ,
						 @KouzaNO,
						 @KouzaMeigi,
						 @CashGaku,
						 @BillGaku ,
						 @BillDate,
						 @BillNO,
						 @ERMCGaku ,
						 @ERMCNO,
						 @ERMCDate,
						 @CardGaku,
						 @OffsetGaku,
						 @OtherGaku1,
						 @Account1,
						 --@start1,
						 @SubAccount1,
						 --@end1label,
						 @OtherGaku2,
						 @Account2,
						 --@start2,
						 @SubAccount2
						 --@end2label
		END
		CLOSE Location_Cursor
		DEALLOCATE Location_Cursor

		drop table #tempPay
		drop table #tempPay2
		drop table #tempPay3
END

