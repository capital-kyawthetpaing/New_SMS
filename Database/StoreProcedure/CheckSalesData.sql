/****** Object:  StoredProcedure [dbo].[CheckSalesData]    Script Date: 2021/06/03 11:50:53 ******/
IF EXISTS (SELECT * FROM sys.procedures WHERE name like '%CheckSalesData%' and type like '%P%')
DROP PROCEDURE [dbo].[CheckSalesData]
GO

/****** Object:  StoredProcedure [dbo].[CheckSalesData]    Script Date: 2021/06/03 11:50:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[CheckSalesData]
    (@SalesNO varchar(11),
    @PurchaseNO  varchar(11),
    @StoreCD   varchar(4)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    
    SET @ERRNO = '';

	--既に請求締済みの場合、エラー
    --以下の条件でレコードがあれば請求締済としてエラーメッセージを表示する
    SELECT @CNT = COUNT(A.SalesNO)
    FROM D_Sales AS A
    INNER JOIN D_BillingDetails AS B
    ON B.SalesNO = A.SalesNO
    WHERE A.SalesNO = @SalesNO
	AND A.BillingType = 2 --締請求
    AND A.DeleteDateTime IS NULL
	AND B.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E176';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --既に入金消込済みの場合、エラー
    --以下の条件でレコードがあれば入金済としてエラーメッセージを表示する
    SELECT @CNT = COUNT(A.SalesNO)
    FROM D_Sales AS A
    INNER JOIN D_CollectPlan AS B
    ON B.SalesNO = A.SalesNO
    INNER JOIN (SELECT DCB.CollectPlanNO
            FROM D_PaymentConfirm AS DP
            LEFT OUTER JOIN D_CollectBilling AS DCB
            ON DCB.ConfirmNO = DP.ConfirmNO
            AND DCB.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectBillingDetails AS DCBD
            ON DCBD.ConfirmNO = DCB.ConfirmNO
            AND DCBD.CollectPlanNO = DCB.CollectPlanNO
            AND DCBD.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlanDetails AS DCPD
            ON DCPD.CollectPlanNO = DCBD.CollectPlanNO
            AND DCPD.CollectPlanRows = DCBD.CollectPlanRows
            AND DCPD.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlan AS DCP
            ON DCP.CollectPlanNO = DCPD.CollectPlanNO
            AND DCP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Collect AS DC
            ON DC.CollectNO = DP.CollectNO
            AND DC.DeleteDateTime IS NULL
            WHERE DCP.StoreCD = @StoreCD
            GROUP BY DCB.CollectPlanNO
        ) AS C
    ON C.CollectPlanNO = B.CollectPlanNO
    WHERE A.SalesNO = @SalesNO
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E246';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --締処理済チェック
    --D_PayPlanに、支払締番号がセットされていれば、エラー（下記のSelectができたらエラー）
    SELECT @CNT = COUNT(A.Number)
    FROM D_PayPlan A
    WHERE A.Number = @PurchaseNO
    AND A.PayPlanKBN = 1
    AND A.PayCloseNO IS NOT NULL
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E176';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
   
    SELECT @ERRNO AS errno;

END

GO


