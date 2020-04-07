 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Update]
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
CREATE PROCEDURE [dbo].[D_Pay_Update]
	-- Add the parameters for the stored procedure here
	@StaffCD  varchar(10),
	@PayDate  date,
	@StoreCD varchar(6),
	@Operator varchar(10),
	@Program as varchar(20),
	@PC as varchar(10),
	@Xml1 as xml,
	@Xml2 as xml,
	@Xml3 as xml,
	@TotalPayGaku as money,
	@OperateMode as Varchar(10),
	@PayNO as varchar(11),
	@LargePayNO as varchar(11)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		declare 
		@DateTime datetime=getdate(),@PayDate1 varchar(10)=CONVERT(VARCHAR(10), @PayDate, 111)

		CREATE TABLE [dbo].[#tempPay]
		(
				PayNO varchar(11) collate Japanese_CI_AS,
				LargePayNO varchar(11) collate Japanese_CI_AS,
				StaffName varchar(6) collate Japanese_CI_AS,
				KouzaCD varchar(3) collate Japanese_CI_AS,
				PayeeCD varchar(13) collate Japanese_CI_AS,
				VendorName varchar(50) collate Japanese_CI_AS,
				PayPlanDate date,
				PayPlanGaku money,
				PayConfirmGaku money,
				PayGaku money,
				TransferGaku money,
				TransferFeeGaku money,
				FeeKBN varchar(11) collate Japanese_CI_AS ,
				Gaku money,
				PayPlan money,
				PayCloseNO varchar(11) collate Japanese_CI_AS,
				PayCloseDate date,
				HontaiGaku8 money,
				HontaiGaku10 money,
				TaxGaku8 money,
				TaxGaku10 money	,
				BankCD varchar(4) collate Japanese_CI_AS,				
				BranchCD varchar(3) collate Japanese_CI_AS,				
				KouzaKBN tinyint,
				KouzaNO varchar(7) collate Japanese_CI_AS,
				KouzaMeigi varchar(40) collate Japanese_CI_AS,				
				CashGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20) collate Japanese_CI_AS,
				ERMCGaku money,
				ERMCNO varchar(20) collate Japanese_CI_AS,
				ERMCDate date,
				CardGaku money,
				OffsetGaku money,				
				OtherGaku1 money,
				Account1 varchar(10) collate Japanese_CI_AS,
				SubAccount1 varchar(10) collate Japanese_CI_AS,
				OtherGaku2 money,
				Account2 varchar(10) collate Japanese_CI_AS,
				SubAccount2 varchar(10) collate Japanese_CI_AS
									
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
				FeeKBN varchar(11),
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
				PayeeCD varchar(13) collate Japanese_CI_AS,
				VendorName varchar(50) collate Japanese_CI_AS,
				BankCD varchar(4) collate Japanese_CI_AS,
				BankName varchar(30) collate Japanese_CI_AS,
				BranchCD varchar(3) collate Japanese_CI_AS,
				BranchName varchar(30) collate Japanese_CI_AS,
				KouzaKBN tinyint,
				KouzaNO varchar(7) collate Japanese_CI_AS,
				KouzaMeigi varchar(40) collate Japanese_CI_AS,
				FeeKBN tinyint,
				Fee int,
				CashGaku money,
				OffsetGaku money,
				BillGaku money,
				BillDate date,
				BillNO varchar(20) collate Japanese_CI_AS,
				ERMCGaku money,
				ERMCNO varchar(20) collate Japanese_CI_AS,
				ERMCDate date,
				OtherGaku1 money,
				Account1 varchar(10) collate Japanese_CI_AS,
				SubAccount1 varchar(10) collate Japanese_CI_AS,
				OtherGaku2 money,
				Account2 varchar(10) collate Japanese_CI_AS,
				SubAccount2 varchar(10) collate Japanese_CI_AS		
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
				Fee int,
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
				SubAccount1 varchar(10),
				OtherGaku2 money,
				Account2 varchar(10),
				SubAccount2 varchar(10)		
				--CardGaku money,
				) 
		EXEC sp_xml_removedocument @DocHandle2; 

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

		update #tempPay 
		set PayNO = @PayNO,
		LargePayNO = @LargePayNO,
		BankCD = d2.BankCD ,
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
		SubAccount1 = d2.SubAccount1,
		OtherGaku2 = d2.OtherGaku2,
		Account2 = d2.Account2,
		SubAccount2 = d2.SubAccount2
		from #tempPay d1 inner join #tempPay2 d2 
		on d2.PayeeCD = d1.PayeeCD and d2.PayPlanDate = d1.PayPlanDate

---SheetA		
				Update D_Pay 
				Set 
				InputDateTime = @DateTime,
				StaffCD  = @StaffCD,
				PayDate = @PayDate,
				PayGaku  = t1.PayGaku,
				TransferGaku  = t1.TransferGaku,
				TransferFeeGaku =t1.TransferFeeGaku,
				FeeKBN  = CAST(case when t1.FeeKBN= N'自社' then 1 else 2 end as tinyint),
				MotoKouzaCD  = t1.KouzaCD ,
				BankCD  = t1.BankCD,
				BranchCD  = t1.BranchCD,
				KouzaKBN   = t1.KouzaKBN,
				KouzaNO  = t1.KouzaNO,
				KouzaMeigi  = t1.KouzaMeigi,
				CashGaku  = t1.CashGaku,
				BillGaku   = t1.BillGaku,
				BillDate  = t1.BillDate,
				BillNO  = t1.BillNO,
				ERMCGaku  = t1.ERMCGaku,
				ERMCNO  = t1.ERMCNO,
				ERMCDate  = t1.ERMCDate,
				CardGaku  = t1.CardGaku,
				OffsetGaku  = t1.OffsetGaku,
				OtherGaku1  = t1.OtherGaku1,
				Account1  = t1.Account1,
				SubAccount1  = t1.SubAccount1,
				OtherGaku2  = t1.OtherGaku2,
				Account2  = t1.Account2,
				SubAccount2  = t1.SubAccount2,
				UpdateOperator	 = @Operator,
				UpdateDateTime	 = @DateTime
				From D_Pay dp inner join #tempPay  t1 on dp.PayNO = t1.PayNO collate Japanese_CI_AS and dp.LargePayNO = t1.LargePayNO collate Japanese_CI_AS
				where dp.PayNO = @PayNO and dp.LargePayNO  = @LargePayNO
				

---SheetB
				Delete D_PayDetails where PayNO = @PayNO

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
				UnpaidAmount1,
				@Operator,
				@DateTime,
				@Operator,
				@DateTime,
				Null,
				Null
				from #tempPay3 t3
				--where PayeeCD=t3.PayeeCD
				--and PayPlanDate=t3.PayPlanDate

---SheetC,D
				exec dbo.L_PayHistory_Insert @PayNO,@LargePayNO

---SheetE

				Insert into D_PayPlan(
				PayConfirmGaku,
				PayConfirmFinishedKBN,
				UpdateOperator,
				UpdateDateTime)
				select 
				UnpaidAmount1,
				case 
					when UnpaidAmount2 = 0 then 1
					else  0 
				end as UnpaidAmount2,
				@Operator,
				@DateTime
				from #tempPay3 t3
				where PayeeCD= t3.PayeeCD
				and PayPlanDate=t3.PayPlanDate

---SheetF      
				update  D_PayCloseHistory  
				Set ProcessingKBN = (case when convert(int,t1.PayConfirmGaku) <> 0 then 3 else 1 end ), 				
				UpdateOperator = @Operator,
				UpdateDateTime = @DateTime
				from D_PayCloseHistory as dpc inner join #tempPay t1
					on t1.PayCloseNO collate Japanese_CI_AS =  dpc.PayCloseNO

----SheetY
				IF NOT EXISTS ( Select * from D_Exclusive Where DataKBN = '9' and Number = @PayNO )
				    BEGIN
				    	insert into D_Exclusive(
				    	DataKBN,
				    	Number,
				    	OperateDataTime,
				    	Operator,
				    	Program,
				    	PC)
				    	values(
				    	9,
				    	@PayNO,
				    	@DateTime,
				    	@Operator,
				    	@Program,
				    	@PC)
				    END
				Else
					BEGIN
						Delete From D_Exclusive Where DataKBN = '9' and Number = @PayNO
						insert into D_Exclusive(
				    	DataKBN,
				    	Number,
				    	OperateDataTime,
				    	Operator,
				    	Program,
				    	PC)
				    	values(
				    	9,
				    	@PayNO,
				    	@DateTime,
				    	@Operator,
				    	@Program,
				    	@PC)
					END	
----SheetZ
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

		--FETCH NEXT FROM Location_Cursor INTO
		--				 @PayNO ,
		--				 @LargePayNO ,
		--				 @StaffName ,
		--				 @KouzaCD ,
		--				 @PayeeCD  ,
		--				 @VendorName,
		--				 @PayPlanDate ,
		--				 @PayPlanGaku,
		--				 @PayConfirmGaku,
		--				 @PayGaku,
		--				 @TransferGaku,
		--				 @TransferFeeGaku,
		--				 @FeeKBN,
		--				 @Gaku,
		--				 @PayPlan,
		--				 @PayCloseNO,
		--				 @PayCloseDate,
		--				 @HontaiGaku8,
		--				 @HontaiGaku10,
		--				 @TaxGaku8,
		--				 @TaxGaku10,
		--				 @BankCD ,				
		--				 @BranchCD,
		--				 @KouzaKBN ,
		--				 @KouzaNO,
		--				 @KouzaMeigi,
		--				 @CashGaku,
		--				 @BillGaku ,
		--				 @BillDate,
		--				 @BillNO,
		--				 @ERMCGaku ,
		--				 @ERMCNO,
		--				 @ERMCDate,
		--				 @CardGaku,
		--				 @OffsetGaku,
		--				 @OtherGaku1,
		--				 @Account1,
		--				 @SubAccount1,
		--				 @OtherGaku2,
		--				 @Account2,
		--				 @SubAccount2
		--END

		--CLOSE Location_Cursor
		--DEALLOCATE Location_Cursor

		drop table #tempPay
		drop table #tempPay2
		drop table #tempPay3


END

