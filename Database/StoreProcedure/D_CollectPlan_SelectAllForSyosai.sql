 BEGIN TRY 
 Drop Procedure dbo.[D_CollectPlan_SelectAllForSyosai]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_CollectPlan_SelectAllForSyosai]    */
CREATE PROCEDURE [dbo].[D_CollectPlan_SelectAllForSyosai](
    -- Add the parameters for the stored procedure here
    @CollectPlanNO  int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    

    SELECT (SELECT M.DenominationName
             FROM M_DenominationKBN AS M
             WHERE M.DenominationCD = DCT.PaymentMethodCD) AS DenominationName

            ,CONVERT(varchar,DCT.CollectDate,111) AS CollectDate
            ,DCT.CollectAmount AS CollectAmount
    FROM D_PaymentConfirm AS DP    
    LEFT OUTER JOIN D_CollectBilling AS DB
    ON DB.ConfirmNO = DP.ConfirmNO
    AND DB.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Collect AS DCT
    ON DCT.CollectNO = DP.CollectNO
    AND DCT.DeleteDateTime IS NULL
    WHERE DP.DeleteDateTime IS NULL
    AND DB.CollectPlanNO = @CollectPlanNO
    ORDER BY DCT.CollectDate
        ,DCT.PaymentMethodCD 
        ,DCT.CollectAmount
        ;
END


