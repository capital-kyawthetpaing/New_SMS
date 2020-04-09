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
@Is_All as int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	



--select ConfirmSource, ConfirmAmount from D_Collect



 if (@Is_All = 0)
 Begin

				select 
				dc.StoreCD as StoreCD,
				CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
				CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
				@StoreName as StoreName,
			CONVERT(varchar(10),dc.CollectDate,111 )	 as CollectDate,
			CONVERT(varchar(10),cast (dc.InputDatetime as date),111 )	 as InputDateTime,
				dc.CollectNO as CollectNo,
				(Case When Isnull(ms.PatternName , dc.CollectCustomerCD +' '+mc.CustomerName )='1' then mc.CustomerName Else dc.CollectCustomerCD +' '+mc.CustomerName End ) as Vendor,
				md.denominationName as DenominationName,
				cast(dc.ConfirmSource as int) as ConfirmSource,
				cast(dc.ConfirmAmount as  int)as ConfirmAmount,
				Cast ((dc.ConfirmSource -dc.ConfirmAmount) as int) as Confirmbalance
				from D_Collect  dc
				left outer join M_DenominationKBN md on dc.PaymentMethodCD = md.DenominationCD
				left outer join M_settlement ms on dc.WebCollectType = ms.PatternCD
				left outer join M_Customer mc on dc.CollectCustomerCD= mc.CustomerCD 
			    left outer join  F_Customer(getdate()) fc on fc.ChangeDate <= dc.CollectDate and   fc.DeleteFlg = 0

				where 
				dc.DeleteDateTime is null and 
			    @paystart is null or (dc.CollectDate >= @paystart) and
				@payend is null or (dc.CollectDate  <= @payend) and 
				@payinputstart is null or (dc.InputDatetime >= @payinputstart) and 
				@payinputend is null or (dc.InputDatetime <= @payinputend) and 
				dc.WebCollectNO =  @WebCollectType and
				dc.CollectCustomerCD= @CollectCustomerCD and
			    (dc.ConfirmSource - dc.ConfirmAmount > 0)
					
				order by 
				dc.StoreCD,
				dc.CollectDate,
				dc.InputDatetime,
				dc.CollectNO asc
End
				else 

BEGIN
				
				select 
				dc.StoreCD as StoreCD,
				CONVERT(varchar(10),getdate(),111 )  as  yyyymmdd,
				CONVERT(varCHAR( 5),GETDATE(),114) as mmss,
				@StoreName as StoreName,
			CONVERT(varchar(10),dc.CollectDate,111 )	 as CollectDate,
			CONVERT(varchar(10),cast (dc.InputDatetime as date),111 )	 as InputDateTime,
				dc.CollectNO as CollectNo,
				(Case When Isnull(ms.PatternName , dc.CollectCustomerCD +' '+mc.CustomerName )='1' then mc.CustomerName Else dc.CollectCustomerCD +' '+mc.CustomerName End ) as Vendor,
				md.denominationName as DenominationName,
				cast(dc.ConfirmSource as int) as ConfirmSource,
				cast(dc.ConfirmAmount as  int)as ConfirmAmount,
				Cast ((dc.ConfirmSource -dc.ConfirmAmount) as int) as Confirmbalance
				from D_Collect  dc
				left outer join M_DenominationKBN md on dc.PaymentMethodCD = md.DenominationCD
				left outer join M_settlement ms on dc.WebCollectType = ms.PatternCD
				left outer join M_Customer mc on dc.CollectCustomerCD= mc.CustomerCD 
			    left outer join  F_Customer(getdate()) fc on fc.ChangeDate <= dc.CollectDate and   fc.DeleteFlg = 0

				where 
				dc.DeleteDateTime is null and 
			    @paystart is null or (dc.CollectDate >= @paystart) and
				@payend is null or (dc.CollectDate  <= @payend) and 
				@payinputstart is null or (dc.InputDatetime >= @payinputstart) and 
				@payinputend is null or (dc.InputDatetime <= @payinputend) and 
				dc.WebCollectNO =  @WebCollectType and
				dc.CollectCustomerCD= @CollectCustomerCD 
					
				order by 
				dc.StoreCD,
				dc.CollectDate,
				dc.InputDatetime,
				dc.CollectNO asc
END

				
				
				
				

			

				



END

