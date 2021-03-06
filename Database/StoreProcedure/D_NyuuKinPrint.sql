 BEGIN TRY 
 Drop Procedure dbo.[D_NyuuKinPrint]
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

CREATE PROCEDURE [dbo].[D_NyuuKinPrint]
	-- Add the parameters for the stored procedure here
@paystart as Date ,
@payend as Date ,
@payinputstart as Date ,
@payinputend as Date ,

@StoreName as varchar(50) ,
@WebCollectType as varchar(3),
@CollectCustomerCD as varchar(13) ,
@Is_All as int ,
@StoreCD as varchar(10)
AS
BEGIN

 if (@Is_All = 0)
 Begin
 select distinct *  from (
				select 
				dc.StoreCD as StoreCD,
				CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
				CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
				@StoreName as StoreName,
				CONVERT(varchar(10),dc.CollectDate,111 )	 as CollectDate,
				CONVERT(varchar(10),cast (dc.InputDatetime as date),111 )	 as InputDateTime,
				dc.CollectNO as CollectNo,
				Isnull(ms.PatternName , dc.CollectCustomerCD +' '+fc.CustomerName ) as Vendor,
				--(Case When Isnull(ms.PatternName , dc.CollectCustomerCD +' '+mc.CustomerName )='1' then mc.CustomerName Else dc.CollectCustomerCD +' '+mc.CustomerName End ) as Vendor,
				md.denominationName as DenominationName,
				Convert(bigint,dc.ConfirmSource ) as ConfirmSource,
				Convert(bigint,dpc.ConfirmAmount1 )as ConfirmAmount,
				Convert (bigint,(dc.ConfirmSource -dpc.ConfirmAmount1)) as Confirmbalance
				from D_Collect  dc
				left outer join (select Collectno as Collectno1, Sum(ConfirmAmount) as ConfirmAmount1 from D_paymentConfirm where DeleteDateTime is null group by Collectno ) dpc on dpc.Collectno1 = dc.Collectno
				left outer join M_DenominationKBN md on dc.PaymentMethodCD = md.DenominationCD
				left outer join M_settlement ms on dc.WebCollectType = ms.PatternCD
				--left outer join M_Customer mc on dc.CollectCustomerCD= mc.CustomerCD 
			    --left outer join  F_Customer(getdate()) fc on fc.ChangeDate <= dc.CollectDate and   fc.DeleteFlg = 0
			    left outer join  F_Customer(getdate()) fc on dc.CollectCustomerCD= fc.CustomerCD 
				                                          and fc.ChangeDate <= dc.CollectDate 
														  and   fc.DeleteFlg = 0

				where 
							dc.DeleteDateTime is null and 
				 dc.StoreCD  = @StoreCD and
				(dc.InputKBN = 1 or
				 dc.InputKBN = 2 or
				 (dc.InputKBN = 3 and dc.AdvanceFlg = 1 )) and
			    (@paystart is null  or (dc.CollectDate >= @paystart) )and
				(@payend is null or (dc.CollectDate  <= @payend)) and 
				(@payinputstart is null or  (dc.InputDatetime >= @payinputstart)) and 
				(@payinputend is null or(dc.InputDatetime <= @payinputend)) and 
				(@WebCollectType is null or (dc.WebCollectType =  @WebCollectType ))and
				(@CollectCustomerCD is null or (dc.CollectCustomerCD= @CollectCustomerCD))  and
			    (dc.ConfirmSource - dpc.ConfirmAmount1 >  cast (0.0 as money))
				) a 
				order by 
				a.StoreCD,
				a.CollectDate,
				a.InputDatetime,
				a.CollectNO asc
End
				else 

BEGIN
				select distinct *  from (
				select distinct
				dc.StoreCD as StoreCD,
				CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
				CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
				@StoreName as StoreName,
			CONVERT(varchar(10),dc.CollectDate,111 )	 as CollectDate,
			CONVERT(varchar(10),cast (dc.InputDatetime as date),111 )	 as InputDateTime,
				dc.CollectNO as CollectNo,
				Isnull(ms.PatternName , dc.CollectCustomerCD +' '+fc.CustomerName ) as Vendor,
			--	(Case When Isnull(ms.PatternName , dc.CollectCustomerCD +' '+mc.CustomerName )='1' then mc.CustomerName Else dc.CollectCustomerCD +' '+mc.CustomerName End ) as Vendor,
				md.denominationName as DenominationName,
				Convert(bigint,dc.ConfirmSource ) as ConfirmSource,
				Convert(bigint,dpc.ConfirmAmount1 )as ConfirmAmount,
				Convert (bigint,(dc.ConfirmSource -dpc.ConfirmAmount1) ) as Confirmbalance
				from D_Collect  dc
				left outer join (select Collectno as Collectno1, Sum(ConfirmAmount) as ConfirmAmount1 from D_paymentConfirm where DeleteDateTime is null  group by Collectno ) dpc on dpc.Collectno1 = dc.Collectno
				left outer join M_DenominationKBN md on dc.PaymentMethodCD = md.DenominationCD
				left outer join M_settlement ms on dc.WebCollectType = ms.PatternCD
				--left outer join M_Customer mc on dc.CollectCustomerCD= mc.CustomerCD 
			    -- left outer join  F_Customer(getdate()) fc on fc.ChangeDate <= dc.CollectDate and   fc.DeleteFlg = 0
			     left outer join  F_Customer(getdate()) fc on dc.CollectCustomerCD= fc.CustomerCD 
				                                           and fc.ChangeDate <= dc.CollectDate 
														   and   fc.DeleteFlg = 0
				where 
					dc.DeleteDateTime is null and 
				 dc.StoreCD  = @StoreCD and
				(dc.InputKBN = 1 or
				 dc.InputKBN = 2 or
				 (dc.InputKBN = 3 and dc.AdvanceFlg = 1 )) and
			    (@paystart is null  or (dc.CollectDate >= @paystart) )and
				(@payend is null or (dc.CollectDate  <= @payend)) and 
				(@payinputstart is null or  (dc.InputDatetime >= @payinputstart)) and 
				(@payinputend is null or(dc.InputDatetime <= @payinputend)) and 
				(@WebCollectType is null or (dc.WebCollectType =  @WebCollectType ))and
			(@CollectCustomerCD is null or (dc.CollectCustomerCD= @CollectCustomerCD))
			) a 
				order by 
				a.StoreCD,
				a.CollectDate,
				a.InputDatetime,
				a.CollectNO asc
END
END


