 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Customer_Insert_Update]
	-- Add the parameters for the stored procedure here
	@CustomerCD as varchar(13),
	@StoreCD as varchar(4),
	@CustomerName as varchar(80),
	@LastName as varchar(20),
	@FirstName as varchar(20),
	@LongName1 as varchar(50),
	@KanaName as varchar(30),
	@StoreKBN as tinyint,
	@CustomerKBN as tinyint,
	@AliasKBN as tinyint,
	@BillingType as tinyint,
	@GroupName as varchar(40),
	@BillingFLG as tinyint,
	@CollectFLG as tinyint,
	@BillingCD as varchar(13),
	@CollectCD as varchar(13),
	@Birthdate as date,
	@Sex as tinyint,
	@Tel11 as varchar(5),
	@Tel12 as varchar(4),
	@Tel13 as varchar(4),
	@Tel21 as varchar(5),
	@Tel22 as varchar(4),
	@Tel23 as varchar(4),
	@ZipCD1 as varchar(3),
	@ZipCD2 as varchar(4),
	@Address1 as varchar(100),
	@Address2 as varchar(100),
	@MailAddress as varchar(100),
	--@MailAddress2 as varchar(100),
	@TankaCD as varchar(13),
	@PointFLG as tinyint,
	@MainStoreCD as varchar(4),
	@StaffCD as varchar(10),
	@BillingCloseDate as tinyint,
	@CollectPlanDate as tinyint,
	@TaxFractionKBN as  tinyint,
	@AmountFractionKBN as tinyint,
	@PaymentUnit as tinyint,
	@DMFlg as tinyint,
	@DeleteFlg as tinyint,
    @Operator as varchar(10),
	 @Program as varchar(30),
	 @PC as varchar(30),
	 @OperateMode as varchar(10),
	 @KeyItem as varchar(100),
	@Mode as tinyint
	


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @CurrentDate as datetime=getdate();

	if @Mode=1

	begin

	insert into M_Customer(CustomerCD,
							ChangeDate,
							CustomerName,
							LastName,
							FirstName,
							LongName1,
							KanaName,
							StoreKBN,
							CustomerKBN,
							AliasKBN,
							BillingType,
							GroupName,
							BillingFLG,
							CollectFLG,
							BillingCD,
							CollectCD,
							Birthdate,
							Sex,
							Tel11,
							Tel12,
							Tel13,
							Tel21,
							Tel22,
							Tel23,
							ZipCD1,
							ZipCD2,
							Address1,
							Address2,
							MailAddress,
							--MailAddress2,
							TankaCD,
							PointFLG,
							MainStoreCD,
							StaffCD,
							BillingCloseDate,
							CollectPlanDate,
							TaxTiming,
							TaxFractionKBN,
							AmountFractionKBN,
							PaymentUnit,
							DMFlg,
							InsertOperator,
							InsertDateTime,
							UpdateOperator,
							UpdateDateTime)

			values(@CustomerCD,
					@CurrentDate,
					@CustomerName,
					@LastName,
					@FirstName,
					@LongName1,
					@KanaName,
					@StoreKBN ,
					@CustomerKBN,
					@AliasKBN ,
					@BillingType,
					@GroupName,
					@BillingFLG ,
					@CollectFLG,
					@BillingCD ,
					@CollectCD ,
					@Birthdate,
					@Sex,
					@Tel11,
					@Tel12,
					@Tel13,
					@Tel21,
					@Tel22,
					@Tel23,
					@ZipCD1,
					@ZipCD2 ,
					@Address1,
					@Address2,
					@MailAddress,
					--@MailAddress2,
					@TankaCD ,
					@PointFLG,
					@MainStoreCD,
					@StaffCD,
					@BillingCloseDate,
					@CollectPlanDate,
					'1',
					@TaxFractionKBN,
					@AmountFractionKBN,
					@PaymentUnit,
					@DMFlg,
					@Operator,
					@CurrentDate,
					@Operator,
					@CurrentDate)

	insert into M_CustomerMail(CustomerCD,StoreCD,ChangeDate,SEQ,MailAddress1,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	values						(@CustomerCD,@StoreCD,@CurrentDate,1,@MailAddress,@Operator,@CurrentDate,@Operator,@CurrentDate)

	end

	else if @Mode=2
	begin	
		update M_Customer

					set		CustomerName = @CustomerName,
							LastName = @LastName,
							FirstName=@FirstName,
							LongName1=@LongName1,
							KanaName=@KanaName,
							GroupName =@GroupName,
							BillingCD = @BillingCD,
							CollectCD = @CollectCD,
							Birthdate = @Birthdate,
							Sex =@Sex,
							Tel11 =@Tel11,
							Tel12=@Tel12,
							Tel13=@Tel13,
							Tel21=@Tel21,
							Tel22=@Tel22,
							Tel23=@Tel23,
							ZipCD1=@ZipCD1,
							ZipCD2=@ZipCD2,
							Address1=@Address1,
							Address2=@Address2,
							MailAddress=@MailAddress,
							--MailAddress2=@MailAddress2,
							MainStoreCD=@MainStoreCD,
							StaffCD=@StaffCD,
							DMFlg=@DMFlg,
							DeleteFlg=@DeleteFlg,
							UpdateOperator=@Operator,
							UpdateDateTime=@CurrentDate

					where	CustomerCD=@CustomerCD


		update		M_CustomerMail

		set			MailAddress1 =@MailAddress,
					UpdateOperator=@Operator,
					DeleteFlg=@DeleteFlg,
					UpdateDateTime=@CurrentDate

		where		CustomerCD=@CustomerCD
	end

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END
