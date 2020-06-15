 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Select01]
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
CREATE PROCEDURE [dbo].[D_Pay_Select01]
	-- Add the parameters for the stored procedure here
	@LargePayNo as varchar(11),
	@PayNo as varchar(11)

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --支払登録
    --画面項目転送表01（第一画面ヘッダ部）
    SELECT
        CONVERT(varchar, dp.PayDate,111) as 'PayDate',
        dp.StaffCD,
        (select top 1 M.StaffName 
            from M_Staff AS M 
            where M.StaffCD=dp.StaffCD 
            and M.DeleteFlg=0
            and M.ChangeDate <= dp.PayDate
            order by M.ChangeDate desc) as 'StaffName',
        dp.PayeeCD,
        (SELECT top 1 fv.VendorName
            from M_Vendor as fv 
            WHERE fv.VendorCD=dp.PayeeCD
            and fv.MoneyPayeeFlg=1
            and fv.DeleteFlg=0
            and fv.ChangeDate <= dp.PayDate
            order by fv.ChangeDate desc) as 'VendorName',

        CONVERT(varchar, dp.PayPlanDate,111) as 'PayPlanDate',
        Isnull(PAY1.PayPlanGaku,0) as PayPlanGaku,
        Isnull(PAY1.PayConfirmGaku,0) as PayConfirmGaku ,
        Isnull(PAY1.PayPlanGaku,0)- Isnull(PAY1.PayConfirmGaku,0) as 'PayGaku',
        Isnull(dp.TransferGaku,0) 'TransferGaku',
        Isnull(dp.TransferFeeGaku,0) 'TransferFeeGaku',
        case when dp.FeeKBN=1 then N'自社' else N'相手負担' end 'FeeKBN',
        Isnull(dp.CashGaku,0) + Isnull(dp.BillGaku,0) + Isnull(dp.ERMCGaku,0) + Isnull(dp.CardGaku,0) + Isnull(dp.OffsetGaku,0) + Isnull(dp.OtherGaku1,0) + Isnull(dp.OtherGaku2,0) as 'Gaku',
        Isnull(PAY1.PayPlanGaku,0)- Isnull(PAY1.PayConfirmGaku,0) as 'PayPlan',
        Isnull(dp.PayCloseNO,0) as 'PayCloseNO',
        '' 'PayCloseDate',
        '' 'HontaiGaku8',
        '' 'HontaiGaku10',
        '' 'TaxGaku8',
        '' 'TaxGaku10'
        ,dp.DeleteDateTime
        ,dp.FBCreateDate
        
        from D_Pay as dp
        LEFT OUTER JOIN
        (select
            dp.LargePayNO,
            dp.PayNO,
            Sum(ISNULL(dpp.PayPlanGaku,0)) PayPlanGaku,
            Sum(ISNULL(dpp.PayConfirmGaku,0)) PayConfirmGaku
            from D_PayDetails dpd
            left outer join D_PayPlan dpp on dpp.PayPlanNO = dpd.PayPlanNO 
            left Outer join D_Pay dp on dp.PayNO = dpd.PayNO
            where dpd.DeleteDateTime is null 
            and dpp.DeleteDateTime is null 
            and dp.DeleteDateTime is null
            and dp.FBCreateDate is null 
            and (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
            and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
            group by dp.LargePayNO,dp.PayNO
        ) AS PAY1
        ON PAY1.LargePayNO=dp.LargePayNO 
        and PAY1.PayNO=dp.PayNO

        where --dp.DeleteDateTime is null
        --and dp.FBCreateDate is null
        --and 
        (@LargePayNo is Null) or (@LargePayNo is not Null and dp.LargePayNO = @LargePayNo)
        and (@PayNo is null) or (@PayNo is not null and dp.PayNO = @PayNo)
        order by dp.PayNO asc
    
END

