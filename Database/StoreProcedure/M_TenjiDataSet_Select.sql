use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.M_TenjiDataSet_Select
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[M_TenjiDataSet_Select]
	-- Add the parameters for the stored procedure here
	
  @Tenjino as varchar(11)
   
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	

	--select Convert (varchar,getdate() ,111)

--declare 

--  @Tenjino as varchar(11)='00000002184'
--   ; select * 

     select Distinct
	 top 1
	 Convert(varchar,j.JuChuuDate,111)  as JuChuuDate
	 ,j.SoukoCD 
	 ,( select top 1 SoukoName from F_Souko(getdate()) where  SoukoCD= j.SoukoCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) as SoukoName
	 ,j.StaffCD	
	 , (select top 1 StaffName from F_Staff(getdate()) where StaffCD = j.staffCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) as StaffName
	 ,j.VendorCD 
	-- , (select top 1 StaffName from F_Staff(getdate()) where StaffCD = j.staffCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) as StaffName
	 ,j.LastYearTerm
	 ,j.LastSeason

 ,j.CustomerCD
	 ,(select top   1  m.variousFlg from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) as CustomerVariousFlg
	
	 ,(Case When  (select top   1  m.variousFlg from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then  (select  ((   m.CustomerName ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.CustomerName end   ) as  CustomerName
		,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.ZipCD1 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.ZipCD1 end    as  CZipCD1
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.ZipCD2 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.ZipCD2 end    as  CZipCD2
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	  then (select  ((   m.Address1 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.Address1 end    as  CAddress1
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Address2 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.Address2 end    as  CAddress2
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel11 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.Tel11 end    as  CTel11
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel12 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.Tel12 end    as  CTel12
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.CustomerCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel13 ))from F_customer(getdate()) m where m.CustomerCD= (j.CustomerCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.Tel13 end    as  CTel13
	,j.CustomerName2
	, Case when j.AliasKBN = 1 then N'様' else N'御中' End   as CKBN

	, j.DeliveryCD
	,(select top   1  m.variousFlg from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) as DeliveryVariousFlg
	 ,(Case When  (select top   1  m.variousFlg from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then  (select  ((   m.CustomerName ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryName end   ) as  DeliveryName
		,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.ZipCD1 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryZipCD1 end    as  DeliveryZipCD1
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.ZipCD2 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryZipCD2 end    as  DeliveryZipCD2
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	  then (select  ((   m.Address1 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryAddress1 end    as  DeliveryAddress1
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Address2 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryAddress2 end    as  DeliveryAddress2
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel11 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryTel11 end    as  DeliveryTel11
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel12 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryTel12 end    as  DeliveryTel12
		 ,Case When  (select top 1 m.variousFlg from F_customer(getdate()) m where m.CustomerCD= j.DeliveryCD and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate) = 0 
	 then (select  ((   m.Tel13 ))from F_customer(getdate()) m where m.CustomerCD= (j.DeliveryCD) and DeleteFlg = 0 and ChangeDate <= j.JuChuuDate)  else   j.DeliveryTel13 end    as  DeliveryTel13
	,j.DeliveryName2
	, Case when j.DeliveryAliasKBN = 1 then N'様' else N'御中' End   as DKBN


	,j.PaymentMethodCD 
	,(select DenominationName from  M_DenominationKBN where DenominationCD =j.PaymentMethodCD	) as 	 DenominationName							
	,Convert(varchar,j.SalesPlanDate, 111) as SalesPlanDate


	 
	 from D_TenzikaiJuchuu j left Outer join 
				D_TenzikaiJuchuuDetails d on j.TenzikaiJuchuuNO = d.TenzikaiJuchuuNO

				where 
				 j.DeleteDateTime is null
				 and d.DeleteDateTime is null
				 and j.TenzikaiJuchuuNO = @Tenjino 
				 
				 
				 --order by d.DisplayRows asc 
				-- select top 1 * from D_TenzikaiJuchuu
				
				-- select top 1 * from M_customer
				-- @Tenjino as varchar(11)='00000002184'
   

						select
						d.TenzikaiJuchuuRows as TenjiRow
						,d.JANCD																	
						,d.SKUCD
						,d.AdminNo																	
						,d.SKUName																	
						,d.ColorNO																	
						,d.ColorName																	
						,d.SizeNO																	
						,d.SizeName		
						,d.DirectFLG															
						, COnvert ( varchar,d.ShippingPlanDate, 111) as 	ShippingPlanDate																
						,d.SoukoCD																	
						,d.OrderUnitPrice																	
						,COnvert ( varchar,d.ArrivePlanDate, 111) as 	ArrivePlanDate																		
						,d.JuchuuSuu										
						--,(Select Char1 From M_MultiPorpose Where M_MultiPorpose.ID='201' And	M_MultiPorpose.[Key]=d.TaniCD	)	as Tani		
						,d.TaniCD as Tani
						,d.JuchuuUnitPrice																	
						,d.JuchuuHontaiGaku																	
						,d.JuchuuGaku																	
						,d.ProfitGaku																	
						,Case when d.TaxRateFLG= 0 then N'非税' else N'税抜' end	as ZeiHyouki															
						,d.JuchuuTaxRitsu																	
						,d.CommentOutStore																	
						,d.CommentInStore																	
						,d.IndividualClientName																	
						,1	as TorokuFlg																
						,d.TaxRateFLG																	

				from  D_TenzikaiJuchuu j left Outer join 
				D_TenzikaiJuchuuDetails d on j.TenzikaiJuchuuNO = d.TenzikaiJuchuuNO

				where 
				 j.DeleteDateTime is null
				 and d.DeleteDateTime is null
				 and j.TenzikaiJuchuuNO = @Tenjino
				 
				 order by d.DisplayRows asc 
END