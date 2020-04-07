 BEGIN TRY 
 Drop Procedure dbo.[NyuukinKesikomiItiranHyou_Report]
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
CREATE PROCEDURE [dbo].[NyuukinKesikomiItiranHyou_Report]
	-- Add the parameters for the stored procedure here
	@CollectDateF AS date,
	@CollectDateT AS date,
	@InputDateTimeF AS date,
	@InputDateTimeT AS date,
	@WebCollectType AS VARCHAR(3),
	@CollectCustomerCD AS VARCHAR(13)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--Create temp table of D_BillingDetails
	CREATE TABLE [dbo].[#Tmp_BillingDetails](
	[BillingNO] [VARCHAR](11) NOT NULL,
	[SalesNO] [VARCHAR](11) NULL,
	[SalesRows] [INT],
	[CollectPlanNO] [INT],
	[BillingGaku] [MONEY]
	CONSTRAINT [PK_Tmp_BillingDetails] PRIMARY KEY CLUSTERED
		(
		[BillingNO] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	)
	ON [PRIMARY]

	--Insert data of D_BillingDetails into temp table
	INSERT INTO [#Tmp_BillingDetails]
		SELECT 
			MAX([BillingNO]),
			[SalesNO],
			 min([SalesRows]),
			 MAX([CollectPlanNO]),
			 MAX([BillingGaku])
		FROM [dbo].[D_BillingDetails]
		WHERE DeleteDateTime IS NULL
		AND SalesRows = '1'
		GROUP BY SalesNO

	--Query for Report Export
	SELECT 
		CONVERT(varchar,collect.CollectDate,111) AS [CollectDate],
		collect.CollectNO AS [CollectNO],
		dKBN.DenominationName AS [DenominationName],
		collect.CollectCustomerCD AS [CollectCustomerCD],
		cust.CustomerName AS [CustomerName],
		bDetails.SalesNO AS [SalesNO],
		sDetails.SKUName AS [SKUName],
		bDetails.BillingGaku AS [BillingGaku],
		cBilling.CollectAmount AS [CollectAmount],
		bDetails.BillingGaku - cBilling.CollectAmount AS [Reconciliation_Balance]
	FROM D_PaymentConfirm pConfirm 
	LEFT OUTER JOIN D_Collect collect ON collect.CollectNO =  pConfirm.CollectNO
	LEFT OUTER JOIN D_CollectBilling cBilling ON cBilling.ConfirmNO = pConfirm.ConfirmNO
	LEFT OUTER JOIN #Tmp_BillingDetails bDetails ON bDetails.CollectPlanNO = cBilling.CollectPlanNO
	LEFT OUTER JOIN D_Billing billing ON billing.BillingNO COLLATE DATABASE_DEFAULT = bDetails.BillingNO COLLATE DATABASE_DEFAULT
	LEFT OUTER JOIN D_SalesDetails sDetails ON sDetails.SalesNO COLLATE DATABASE_DEFAULT =bDetails.SalesNO COLLATE DATABASE_DEFAULT AND sDetails.SalesRows = bDetails.SalesRows 
	LEFT OUTER JOIN M_DenominationKBN dKBN ON dKBN.DenominationCD = collect.PaymentMethodCD
	LEFT OUTER JOIN M_Settlement sett ON sett.PatternCD = collect.WebCollectType
	LEFT OUTER JOIN M_Customer cust ON cust.CustomerCD = collect.CollectCustomerCD AND cust.ChangeDate <= collect.CollectDate
	WHERE pConfirm.DeleteDateTime IS NULL
	AND collect.DeleteDateTime IS NULL
	AND cBilling.DeleteDateTime IS NULL
	AND billing.DeleteDateTime IS NULL
	AND sDetails.DeleteDateTime IS NULL
	AND billing.BillingConfirmFlg = 1
	AND cust.DeleteFlg = 0
	AND ( @CollectDateF IS NULL OR (collect.CollectDate >= @CollectDateF))
	AND ( @CollectDateT IS NULL OR (collect.CollectDate <= @CollectDateT))
	AND ( @InputDateTimeF IS NULL OR (collect.InputDatetime >= @InputDateTimeF))
	AND ( @InputDateTimeT IS NULL OR (collect.InputDatetime <= @InputDateTimeT))
	AND ( @WebCollectType IS NULL OR (collect.WebCollectType = @WebCollectType))
	AND ( @CollectCustomerCD IS NULL OR (collect.CollectCustomerCD = @CollectCustomerCD))
	ORDER BY
	collect.StoreCD ASC,
	sett.PatternName ASC,
	collect.CollectDate ASC,
	collect.CollectNO ASC,
	collect.PaymentMethodCD ASC
	
	drop table [#Tmp_BillingDetails]

END

