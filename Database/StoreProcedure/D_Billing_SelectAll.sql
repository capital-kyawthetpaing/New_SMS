 BEGIN TRY 
 Drop Procedure dbo.[D_Billing_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Sales_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Billing_SelectAll](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @CustomerCD  varchar(13),
    @ChangeDate  varchar(10),
    @BillingCloseDate tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    SELECT DB.BillingNO
            ,DB.BillingCustomerCD AS CustomerCD
          ,(SELECT top 1 A.CustomerName
              FROM M_Customer A 
              WHERE A.CustomerCD = DB.BillingCustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DB.BillingCloseDate
              ORDER BY A.ChangeDate desc) AS CustomerName 
            ,DB.BillingGaku
            ,CONVERT(varchar,DB.CollectPlanDate,111) AS CollectPlanDate
			,SUM(DB.BillingGaku) OVER() AS SUM_BillingGaku

    FROM D_Billing AS DB
    
    WHERE DB.StoreCD = @StoreCD
    AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
    AND DB.BillingCloseDate = CONVERT(date, @ChangeDate)   
    AND DB.DeleteDateTime IS NULL
    AND EXISTS (SELECT MC.CustomerCD
        FROM M_Customer AS MC 
        WHERE MC.ChangeDate <= DB.BillingCloseDate
        AND MC.BillingCloseDate = (CASE WHEN @BillingCloseDate <> 0 THEN @BillingCloseDate ELSE MC.BillingCloseDate END)
        AND MC.DeleteFlg = 0
        AND MC.CustomerCD = DB.BillingCustomerCD)
    ORDER BY DB.BillingNO, DB.BillingCustomerCD, DB.CollectPlanDate
    ;

END


