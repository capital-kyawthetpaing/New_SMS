 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Vendor_Insert_Update] 
	-- Add the parameters for the stored procedure here
	@VendorCD	AS  varchar(13),                       
	@ChangeDate	AS date,
	@ShoguchiFlg AS tinyint,
	@VendorName	 AS varchar(50),
	@VendorShortName AS varchar(20),    --Add By SawLay
	@VendorLongName1  AS varchar(80),
	@VendorLongName2  AS varchar(80),
	@VendorPostName	  AS varchar(40),
	@VendorPositionName AS varchar(40),
	@VendorStaffName AS varchar(40),
	@VendorKana	AS varchar(20),
	@PayeeFlg  AS tinyint,
    @MoneyPayeeFlg AS tinyint,
	@PayeeCD AS varchar(10),
	@MoneyPayeeCD AS varchar(10),
	@ZipCD1	AS varchar(3),
	@ZipCD2	AS varchar(4),
	@Address1  AS varchar(100),
	@Address2  AS varchar(100),
	@MailAddress  AS varchar(100),
	@TelphoneNO	  AS varchar(15),
	@FaxNO   AS varchar(15),
	@PaymentCloseDay  AS tinyint,
	@PaymentPlanKBN	  AS tinyint,
	@PaymentPlanDay	 AS tinyint,
	@HolidayKBN	AS tinyint,
	@BankCD	AS varchar(4),
	@BranchCD AS varchar(3),
	@KouzaKBN AS tinyint,
	@KouzaNO AS varchar(7),
	@KouzaMeigi	 AS varchar(40),
	@KouzaCD AS varchar(3),
	@TaxTiming AS tinyint,   --Add By SawLay
	@TaxFractionKBN AS tinyint,  --Add By SawLay
	@AmountFractionKBN AS tinyint,   --Add By SawLay
	@NetFlg	 AS tinyint,
	@EDIFlg AS tinyint,  --Add By SawLay
	@EDIVendorCD AS varchar(13),  --Add By SawLay
	@StaffCD   AS varchar(10),
	@AnalyzeCD1   AS varchar(10),
	@AnalyzeCD2   AS varchar(10),
	@AnalyzeCD3   AS varchar(10),
	@DisplayOrder   AS int,
	@NotDisplayNote  AS varchar(500),
	@DisplayNote  AS varchar(500),
	@DeleteFlg  AS tinyint,
	@Operator varchar(10),
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

	declare @currentDate as datetime = getdate();
    -- Insert statements for procedure here
	if @Mode = 1--insert mode
				begin										
					insert into M_Vendor
					(
						VendorCD,
						ChangeDate,
						ShoguchiFlg,
						VendorName,
						VendorShortName, --Add By SawLay
						VendorLongName1,
						VendorLongName2,
						VendorPostName,
						VendorPositionName,
						VendorStaffName,
						VendorKana,
						VendorFlg,  --Add By SawLay
						PayeeFlg,
						MoneyPayeeFlg,
						PayeeCD,
						MoneyPayeeCD,
						ZipCD1,
						ZipCD2,
						Address1,
						Address2,
						MailAddress1,
						TelephoneNO,
						FaxNO,
						PaymentCloseDay,
						PaymentPlanKBN,
						PaymentPlanDay,
						HolidayKBN,
						BankCD,
						BranchCD,
						KouzaKBN,
						KouzaNO,
						KouzaMeigi,
						KouzaCD,
						TaxTiming,  --Add By SawLay
						TaxFractionKBN,  --Add By SawLay
						AmountFractionKBN,  --Add By SawLay
						NetFlg,
						EDIFlg, --Add By SawLay
						EDIVendorCD, --Add By SawLay
						LastOrderDate,
						StaffCD,
						AnalyzeCD1,
						AnalyzeCD2,
						AnalyzeCD3,
						DisplayOrder,
						NotDisplyNote,
						DisplayNote,
						DeleteFlg ,
						UsedFlg ,
						InsertOperator ,
						InsertDateTime,
						UpdateOperator ,
						UpdateDateTime
					)
					values
					(		
					    @VendorCD,
					    @ChangeDate,
					    @ShoguchiFlg,
					    @VendorName,
						@VendorShortName,  --Add By SawLay
					    @VendorLongName1,
					    @VendorLongName2,
					    @VendorPostName,
					    @VendorPositionName,
					    @VendorStaffName,
					    @VendorKana,
						1,     --Add By SawLay
					    @PayeeFlg,
					    @MoneyPayeeFlg,
					    @PayeeCD,
					    @MoneyPayeeCD,
					    @ZipCD1,
					    @ZipCD2,
					    @Address1,
					    @Address2,
					    @MailAddress,
					    @TelphoneNO,
					    @FaxNO,
					    @PaymentCloseDay,
					    @PaymentPlanKBN,
					    @PaymentPlanDay,
					    @HolidayKBN,
					    @BankCD,
					    @BranchCD,
					    @KouzaKBN,
					    @KouzaNO,
					    @KouzaMeigi,
					    @KouzaCD,
						@TaxTiming,  --Add By SawLay
						@TaxFractionKBN,  --Add By SawLay
						@AmountFractionKBN,  --Add By SawLay
					    @NetFlg,
						@EDIFlg, --Add By SawLay
						@EDIVendorCD,  --Add By SawLay
					    Null,
					    @StaffCD,
					    @AnalyzeCD1,
					    @AnalyzeCD2,
					    @AnalyzeCD3,
					    @DisplayOrder,
					    @NotDisplayNote,
					    @DisplayNote,
						@DeleteFlg,
						0,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)
				end
				else if @Mode = 2--update mode
				begin
					update M_Vendor
					set
					VendorCD = @VendorCD,
					ChangeDate = @ChangeDate,
					ShoguchiFlg  = @ShoguchiFlg,
					VendorName  = @VendorName,
					VendorShortName = @VendorShortName,  --Add By SawLay
					VendorLongName1 = @VendorLongName1,
					VendorLongName2 = @VendorLongName2,
					VendorPostName = @VendorPostName,
					VendorPositionName = @VendorPositionName,
					VendorStaffName = @VendorStaffName,
					VendorKana = @VendorKana,
					VendorFlg = 1,  --Add By SawLay
					PayeeFlg = @PayeeFlg,
					MoneyPayeeFlg = @MoneyPayeeFlg,
					PayeeCD = @PayeeCD,
					MoneyPayeeCD = @MoneyPayeeCD,
					ZipCD1 = @ZipCD1,
					ZipCD2 = @ZipCD2,
					Address1 = @Address1,
					Address2 = @Address2,
					MailAddress1 = @MailAddress,
					TelephoneNO = @TelphoneNO,
					FaxNO = @FaxNO,
					PaymentCloseDay = @PaymentCloseDay,
					PaymentPlanKBN = @PaymentPlanKBN,
					PaymentPlanDay = @PaymentPlanDay,
					HolidayKBN = @HolidayKBN,
					BankCD = @BankCD,
					BranchCD = @BranchCD,
					KouzaKBN = @KouzaKBN,
					KouzaNO = @KouzaNO,
					KouzaMeigi = @KouzaMeigi,
					KouzaCD = @KouzaCD,
					TaxTiming = @TaxTiming, --Add By SawLay
					TaxFractionKBN = @TaxFractionKBN, --Add By SawLay
					AmountFractionKBN = @AmountFractionKBN, --Add By SawLay
					NetFlg = @NetFlg,
					EDIFlg =  @EDIFlg, --Add By SawLay
					EDIVendorCD = @EDIVendorCD, --Add By SawLay
					LastOrderDate = Null,
					StaffCD = @StaffCD,
					AnalyzeCD1 = @AnalyzeCD1,
					AnalyzeCD2 = @AnalyzeCD2,
					AnalyzeCD3 = @AnalyzeCD3,
					DisplayOrder = @DisplayOrder,
					NotDisplyNote = @NotDisplayNote,
					DisplayNote = @DisplayNote,
					DeleteFlg= @DeleteFlg,
					UpdateOperator = @Operator, 
					UpdateDateTime = @currentDate

										   
					where VendorCD = @VendorCD	  
						and ChangeDate = @ChangeDate 	
				end

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END


