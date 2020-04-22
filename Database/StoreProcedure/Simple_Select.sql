 BEGIN TRY 
 Drop Procedure dbo.[Simple_Select]
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
CREATE PROCEDURE [dbo].[Simple_Select]
	-- Add the parameters for the stored procedure here
	@CheckType varchar(2),
	@ChangeDate date,
	@CD1 varchar(50),
	@CD2 varchar(50),
	@CD3 varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @CheckType = '1'
		select UsedFlg from M_Souko where SoukoCD=@CD1 and ChangeDate=@ChangeDate
	if @CheckType = '2'
		select 1 from M_Souko where SoukoCD=@CD1 and ChangeDate<=@ChangeDate
	if @CheckType = '3'
		select UsedFlg from M_Bank where BankCD=@CD1 and ChangeDate=@ChangeDate
	if @CheckType = '4'
		select 1 from M_Bank where BankCD=@CD1 and ChangeDate<=@ChangeDate
	if @CheckType = '5'
		select 1 from M_Kouza where KouzaCD=@CD1 and ChangeDate=@ChangeDate
	if @CheckType = '7'
		select UsedFlg from M_BankBranch where BranchCD=@CD1 and BankCD=@CD2 and ChangeDate=@ChangeDate
	if @CheckType = '8'
		select 1 from M_BankBranch where BranchCD=@CD1 and BankCD=@CD2 and ChangeDate<=@ChangeDate
	if @CheckType = '9'
		select 1 from M_Customer where CustomerCD=@CD1
	if @CheckType = '10'
		select * from D_Cost where CostNO=@CD1 
	if @CheckType = '11'
		select 1 from M_Staff where StaffCD=@CD1 and ChangeDate=@ChangeDate
	if @CheckType = '12'
		select APIKey,[State],UpdateDateTime from D_APIControl where APIKey = @CD1
	if @CheckType = '13'
		select APIKey,ChangeDate,StoreCD,TESTMode,ApplicationID0,ApplicationID1,YahooSecretKey,RefreshToken,StoreAccount0,StoreAccount1,ServiceSecret,LicenseKey,StoreCD,ShopAccount from M_API where APIKey = @CD1
	if @CheckType = '14'
		select [Key],Char1,Num1 from M_MultiPorpose where ID = @CD1 order by Num1
	if @CheckType = '15'
		select Left([Key],2),Char1 from M_MultiPorpose where ID = @CD1 order by [Key]
	if @CheckType = '16'
		select * from D_Sales where SalesNo = @CD1 and BillingType = @CD2
	if @CheckType = '17'
		select * from M_Store where StoreCD = @CD1 and ChangeDate <= @ChangeDate
	if @CheckType = '18'
		select * from M_PrefixNumber where Prefix = @CD1
	if @CheckType = '19'
		select top 1 * from D_PayPlan where Number = @CD1 and PayCloseNO is not null and DeleteOperator is null and DeleteDateTime is null
	if @CheckType = '20'
		select Botton,MasterKBN from M_StoreBottunDetails where GroupNO = @CD1 and BottunName = @CD2
	if @CheckType = '21'
		select [Key],Char1,Num1 from M_MultiPorpose where ID = @CD1 order by [Key]
	if @CheckType = '22'
		select MenuName,MenuID from M_Menu Order By MenuID
	if @CheckType = '23'
		select AuthorizationsName,AuthorizationsCD from F_Authorizations(GetDate()) Order By AuthorizationsCD
	if @CheckType = '24'
		select StoreAuthorizationsName,StoreAuthorizationsCD,StoreCD from F_StoreAuthorizations(GetDate()) Order By StoreAuthorizationsCD
	if @CheckType = '25'
		select StoreName,StoreCD from M_Store where ChangeDate<= @ChangeDate and (StoreKBN =2 or StoreKBN  = 3)
	if @CheckType = '26'
		select top 1 change from D_StoreCalculation
	if @CheckType = '27'
		select StoreCD from M_Store where StoreCD = @CD1 and ChangeDate <=@ChangeDate and DeleteFlg = @CD2
--Souko ChangeDate 
	if @CheckType='29'
		select 1 from M_Vendor where VendorCD=@CD1  and ChangeDate<=@ChangeDate
	if @CheckType='28'
		select 1 from M_Vendor where VendorCD=@CD1  and ChangeDate<=@ChangeDate
	if @CheckType ='30'
		select * from M_ZipCode where ZipCD1 = @CD1 and ZipCD2 = @CD2
	--
	if @CheckType = '31'
	select UsedFlg from M_BankShiten where BranchCD=@CD1 and BankCD=@CD2 and ChangeDate=@ChangeDate
	
	if @CheckType='32'
	select 1 from M_Staff where StaffCD = @CD1 and ChangeDate<=@ChangeDate

	--if @CheckType = '31'
	--select UsedFlg from M_BankShiten where BranchCD=@CD1 and BankCD=@CD2 and ChangeDate=@ChangeDate
	if @CheckType ='33'
	select 1 from M_Carrier where CarrierCD = @CD1 and ChangeDate <= @ChangeDate

	if @CheckType='34'
	select 1 from M_StoreAuthorizations where  StoreAuthorizationsCD=@CD1 and ChangeDate=@ChangeDate and StoreCD=@CD2
	
	if @CheckType ='35'
	select * from M_DenominationKBN where MainFLG = 1 Order By DenominationCD

	if @CheckType = '36'
	select CustomerName from M_Customer where CustomerCD = @CD1 and DeleteFlg = 0  and ChangeDate <= GETDATE()

	if @CheckType = '37'
	select Char1 from M_MultiPorpose where ID = @CD1 and [Key] = @CD2

	if @CheckType = '38'
	select [Key],Char1 from M_MultiPorpose where ID = @CD1

	if @CheckType='39'
		select 1 from M_SKU where SKUCD=@CD1  and ChangeDate<=@ChangeDate

	if @CheckType='40'
		select 1 from M_Souko where SoukoCD=@CD1  and DeleteFlg=0

	if @CheckType = '41' 
		select 1 from M_Staff  where StaffCD = @CD1 and ChangeDate <= @ChangeDate

	if @CheckType = '42'
		select * from M_MultiPorpose where ID = @CD1 and [Key] = @CD2

	if @CheckType = '43'
		select 1 from M_Calendar where CalendarDate = @ChangeDate
	
	if @CheckType = '44'
		select VendorName From M_Vendor where VendorCD = @CD1 and PayeeFlg = 1 and ChangeDate <= @ChangeDate

	if @CheckType='45'
	select * from M_Customer where CustomerCD=@CD1 and ChangeDate<=@ChangeDate

	if @CheckType='46'
	select 1 from D_Picking where PickingNO=@CD1 and DeleteDateTime is null

	if @CheckType='47'--売上元帳印刷
	select 1 from M_StoreClose
	where StoreCD=@CD1 and ((@CD2 is null or(FiscalYYYYMM<@CD2)) or( (@CD2 is null or (FiscalYYYYMM=@CD2)) and (ClosePosition1=0 and ClosePosition3=0 )))

	if @CheckType='48'--売上元帳印刷
	select 1 from M_StoreClose
	where StoreCD=@CD1 and ((@CD2 is null or(FiscalYYYYMM>@CD2)) or( (@CD2 is null or(FiscalYYYYMM=@CD2)) and (ClosePosition1=0 and ClosePosition3=1)))

	if @CheckType='49'--ShiharaiShimeShori
		select 1 from M_Control mc inner join M_FiscalYear mf on mf.FiscalYear=mc.FiscalYear
		where mc.MainKey='1' 	and mf.InputPossibleStartDate<=@ChangeDate  and mf.InputPossibleEndDate>=@ChangeDate

	if @CheckType='50'
	select MonthlySummuryKBN from M_Control where MainKey=1

	if @CheckType='51'
	select Max(DepositNO) as DepositNO from D_DepositHistory  where  Program=@CD1

	if @CheckType='52'
	select Max(DepositNO) as DepositNO from D_DepositHistory where Program=@CD1

	if @CheckType='53'
	   select 1 from M_MultiPorpose where [Key]=@CD1 and [ID]=@CD2 

	if @CheckType='54'
	select 1 from M_MultiPorpose where  [ID]=@CD2 and Char1=@CD3 and Char2=@CD1 

	if @CheckType='55'
	  select 1 from M_Program where ProgramID=@CD1


	  if @CheckType='56'
	  select 1 from M_Brand where BrandCD=@CD1

	   if @CheckType='57'
	  select 1 from M_MultiPorpose where ID='202' and [Key]=@CD1
END

