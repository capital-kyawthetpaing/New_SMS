 BEGIN TRY 
 Drop Procedure dbo.[_Ponpare_InsertAllDetails]
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
CREATE PROCEDURE [dbo].[_Ponpare_InsertAllDetails]
	-- Add the parameters for the stored procedure here
	
	 	@StoreCD as varchar(15)  ,
				@API_Key as varchar(15) ,
				@LastUpdatedBefore as dateTime ,
				@LastUpdatedAfter as datetime,
				@xmlJuchuu as xml,
				@xmlJuchuuDetails as xml,
				@xmlCoupon as xml,
				@xmlEnclose as xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    	 declare	@Date as DateTime= getdate() 
			
				 declare @val as int 
			     set @val = (select Max(IsNull(InportSEQ,0))+1 from  D_APIRireki);
	             if (@val is null)
	             Begin
	             set @val=1;
	             End

------------------------------------------------------------------------------------------------------------------------Juchuu
				

				  declare @DocHandledetails int;
 	             exec sp_xml_preparedocument @DocHandledetails output, @xmlJuchuu
	             select * into #tmp
	               FROM OPENXML (@DocHandledetails, '/NewDataSet/test',2)
				 with (
				 StoreCD varchar(4),
				 APIKey int,
				 InportSEQRows int,
				 orderNo varchar(50),
				 orderDateTime varchar(19),
				 orderSts varchar(50),
				 pymntSts varchar(50),
				 depositDate varchar(10),
				 sendDate varchar(10),
				 dlvKind varchar(1),
				 dlvDesiredDate varchar(10),
				 dlvDesiredTimeZoneKind varchar(1),
				 dlvDesiredTimeZoneFrom varchar(2),
				 dlvDesiredTimeZoneTo varchar(1),
				 shopUserNameInCharge varchar(41),
				 orderMemo varchar(200),
				 messageToCustomer varchar(41),
				 useTerminal varchar(1),
				 mailcarrierCode varchar(1),
				 giftApplyFlg tinyint,
				 orderNote varchar(2000),
				 taxRate varchar(1),
				 dlvAddrCautionFlg tinyint,
				 yellowUserFlg tinyint,
				 memberKind varchar(1),
				 enclosableFlg tinyint,
				 itemAmount money,
				 taxAmount money,
				 dlvFee money,
				 pymntFee money,
				 totalAmount money,
				 usePointAmount money,
				 useCouponTotalAmount money,
				 useCouponShopAmount money,
				 useCouponOtherAmount money,
				 useCouponTotalCnt int,
				 useCouponShopCnt int,
				 useCouponOtherCnt int,
				 totalPymntAmount money ,
				 totalPymntAmountInit money,
				 customerZip1 varchar(3),
				 customerZip2 varchar(4),
				 customerPref varchar(4),
				 customerAddress varchar(240),
				 customerLastName varchar(40),
				 customerFirstName varchar(40),
				 customerLastNameKana varchar(60),
				 customerFirstNameKana varchar(60),
				 customerTel varchar(17),
				 customerEmail varchar(200),
				 pymntMethodId varchar(3),
				 pymntMethodName varchar(255),
				 cardBrand varchar(1),
				 cardNo varchar(40),
				 cardSignature varchar(40),
				 cardExpire varchar(5),
				 cardPymntMethod varchar(1),
				 dlvMethodId varchar(3),
				 dlvMethodName varchar(512),
				 dlvAddrZip1 varchar(3),
				 dlvAddrZip2 varchar(4),
				 dlvAddrPref varchar(4),
				 dlvAddrAddress varchar(240),
				 dlvAddrLastName varchar(40),
				 dlvAddrFirstName varchar(40),
				 dlvAddrLastNameKana varchar(60),
				 dlvAddrFirstNameKana varchar(60),
				 dlvAddrTel varchar(17),
				 slipNo varchar(43),
				 noshi varchar(255),
				 wrappingKind1 varchar(1),
				 wrappingName1 varchar(255),
				 wrappingPrice1 money,
				 wrappingTaxKind1 varchar(1),
				 wrappingDelFlg1 tinyint,
				 wrappingKind2 varchar(1),
				 wrappingName2 varchar(255),
				 wrappingPrice2 money,
				 wrappingTaxKind2 varchar(1),
				 wrappingDelFlg2 tinyint,
				 encloseKind varchar(1),
				 encloseOrderNo varchar(50),
				 encloseItemAmount money,
				 encloseTaxAmount money,
				 encloseDlvFee money,
				 enclosePymntFee money,
				 encloseTotalAmount money,
				 encloseUsePointAmount money,
				 encloseUseCouponAmount money,
				 encloseTotalPymntAmount money ,
				 cardUpdatingIconFlg tinyint,
				 cardUpdatedIconFlg tinyint,
				 fraudOrderAlert varchar(3),
				 nxDayDlvFlg tinyint

				 ) exec sp_xml_removedocument @DocHandledetails;

				 insert into D_ponpareJuchuu select @val as InportSEQ, *,null as Insertoperator, @Date as InsertDateTime, null as updateOperator, @Date as UpdateDateTime from #tmp  


				drop table #tmp
				 
--------------------------------------------------------------------------------------------------------------------------JuchuuDetial

					declare @DocHdle int;exec sp_xml_preparedocument @DocHdle output, @xmlJuchuuDetails
					select * into #dtemp
					  FROM OPENXML (@DocHdle, '/NewDataSet/test',2)
				 with 
				 (
				  StoreCD varchar(4),
				  APIKey int,
				  InportSEQRows int,
				  orderNo varchar (50),
				  orderRows int,
				  orderItemSubNo int,
				  itemName varchar(255),
				  itemId varchar(13),
				  itemManageId varchar(32),
				  HSkuItemId varchar(32),
				  VSkuItemId varchar(32),
				  salePrice money,
				  itemCnt int,
				  incShippingFlg tinyint,
				  taxKind varchar(1),
				  itemTaxRateKbn varchar(2),
				  itemTaxRate varchar(1),
				  incCodFeeFlg money,
				  getPointRate int,
				  getPoint money,
				  purchaseOption varchar(133),
				  invKind varchar(1),
				  itemDelFlg tinyint
				 )
				 exec sp_xml_removedocument @DocHdle;
					--@xmlJuchuuDetails
					 	-- SELECT COUNT(*)	FROM INFORMATION_SCHEMA.COLUMNS	WHERE table_name = 'D_PonpareJuchuuDetails'

					 --SELECT COUNT(*) FROM tempdb.sys.columns WHERE object_id = object_id('tempdb..#tmp')
				
				Insert into D_PonpareJuchuuDetails
				select @val, *, null ,@date, null, @date from #dtemp

				--SELECT COUNT(*) FROM tempdb.sys.columns WHERE object_id = object_id('tempdb..#dtemp')
			
				drop table #dtemp

---------------------------------------------------------------------------------------------------------------------JuchuuCoupon
					declare @DocHdleCoupon int;exec sp_xml_preparedocument @DocHdleCoupon output, @xmlCoupon
					select * into #Coupontemp
					  FROM OPENXML (@DocHdleCoupon, '/NewDataSet/test',2)
				 with 
				 (
				 StoreCD varchar(4),
				 APIKey int,
				 InportSEQRows int,
				 orderNo varchar(50),
				 couponRows int,
				 couponCode varchar(12),
				 orderItemSubNo int,
				 itemManageId varchar(12),
				 couponName varchar(225),
				 couponCnt int,
				 couponCapitalKind int,
				 discountType int,
				 expiryDate varchar(12),
				 couponAmount money
				 )
				  exec sp_xml_removedocument @DocHdleCoupon;

				  insert into D_PonpareCoupon
				  select @val ,* , null, @date , null, @date from #Coupontemp

				 drop table #Coupontemp
------------------------------------------------------------------------------------------------------------------------------JuchuuEnclose
				declare @DocHdleEnclose int;exec sp_xml_preparedocument @DocHdleEnclose output, @xmlEnclose
							select * into #Enclosetemp
							  FROM OPENXML (@DocHdleEnclose, '/NewDataSet/test',2)
						 with 
						(
						 StoreCD varchar(4),
						 APIKey int,
						 InportSEQRows int,
						 orderNo varchar(50),
						 encloseRows int,
						 encloseOrderNo varchar(50)
						)
						 exec sp_xml_removedocument @DocHdleEnclose;


						insert into D_PonpareEnclose
						select @val,*, null, @Date, null, @Date from  #Enclosetemp

						 Drop table #Enclosetemp


--------------------------------------------------------------------------------------------------------------------------JuchuuDetial
																	
END
