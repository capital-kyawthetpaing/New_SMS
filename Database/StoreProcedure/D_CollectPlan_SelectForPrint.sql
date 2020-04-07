 BEGIN TRY 
 Drop Procedure dbo.[D_CollectPlan_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_CollectPlan_SelectForPrint]    */
CREATE PROCEDURE [dbo].[D_CollectPlan_SelectForPrint](
    -- Add the parameters for the stored procedure here
    @DateFrom  varchar(10),
    @DateTo  varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @CustomerCD  varchar(13),
    @PrintFLG TinyInt,
    @PaymentProgressKBN TinyInt,
    @DetailOn TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	--���׈������ꍇ
    IF @DetailOn = 1
    BEGIN
        SELECT DH.StoreCD
              ,(SELECT top 1 A.StoreName
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.NextCollectPlanDate
                        AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName
              ,(SELECT TOP 1 A.StaffCD
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS StaffCD
              ,(SELECT TOP 1 B.StaffName
                        FROM M_Customer AS A
                        LEFT OUTER JOIN M_Staff AS B
                        ON B.StaffCD = A.StaffCD
                        AND B.ChangeDate <= DH.NextCollectPlanDate
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS StaffName

              ,CONVERT(varchar,DH.NextCollectPlanDate,111) AS NextCollectPlanDate   --����\���
    --          ,���L������i��NULL�̎���today)�|NextCollectPlanDate  ���ʁ��O�̎��̂ݕ\�� --�x������
                ,(CASE WHEN DH.NextCollectPlanDate < ISNULL(A.CollectDate, CONVERT(date, SYSDATETIME())) THEN
                     CONVERT(varchar, DATEDIFF(day, DH.NextCollectPlanDate, ISNULL(A.CollectDate, CONVERT(date, SYSDATETIME())))) 
                     ELSE NULL END) AS DelayDays
              ,DH.JuchuuNO
              ,DH.SalesNO
              ,DS.SalesDate
              ,DH.BillingNO
              ,DH.CollectPlanGaku	--�ō������z
              
              ,A.CollectDate	--�����
              ,A.CollectNO
              ,A.CollectGaku	--����z
              ,DH.CollectPlanGaku-A.CollectGaku AS KaisyuGaku	--����\��z=�ō������z�|����z

              ,DB.BillingCustomerCD
              ,(SELECT TOP 1 A.CustomerName
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS CustomerName
                        
              ,DSD.SKUCD
              ,DSD.SKUName
              ,DH.PaymentProgressKBN
			  ,@DetailOn AS DetailOn

            from D_CollectPlan DH
            LEFT OUTER JOIN D_Sales DS
            ON DS.SalesNO = DH.SalesNO
            LEFT OUTER JOIN D_Billing DB
            ON DB.BillingNO = DH.BillingNO
            LEFT OUTER JOIN (SELECT A.CollectPlanNO, MIN(A.ConfirmNO) AS ConfirmNO
                            FROM D_CollectBilling A 
                            GROUP BY A.CollectPlanNO) AS DC
            ON DC.CollectPlanNO = DH.CollectPlanNO
            LEFT OUTER JOIN D_PaymentConfirm DP
            ON DP.ConfirmNO = DC.ConfirmNO

            LEFT OUTER JOIN D_CollectPlanDetails AS DCD
            ON DCD.CollectPlanNO = DH.CollectPlanNO
            LEFT OUTER JOIN D_SalesDetails AS DSD 
            ON DSD.SalesNO = DCD.SalesNO
            AND DSD.SalesRows = DCD.SalesRows
            
            --�T�u�N�G���i��A�c����\��ԍ��P�ʂ̓���ԍ��A�����ς݊z��擾)
--            LEFT OUTER JOIN (SELECT DH.CollectPlanNO, SUM(ISNULL(DB.CollectGaku,0)) AS CollectGaku	2019.10.23 chg
            LEFT OUTER JOIN (SELECT DH.CollectPlanNO, SUM(ISNULL(DB.CollectAmount,0)) AS CollectGaku
                ,MIN(DP.CollectNO) AS CollectNO, MIN(DC.CollectDate) AS CollectDate
                from D_CollectPlan DH
                LEFT OUTER JOIN D_CollectBilling AS DB
                ON DB.CollectPlanNO = DH.CollectPlanNO
                AND DB.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_PaymentConfirm DP
                ON DP.ConfirmNO = DB.ConfirmNO
                AND DP.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Collect AS DC
                ON DC.CollectNO = DP.CollectNO
                AND DC.DeleteDateTime IS NULL
                GROUP BY DH.CollectPlanNO
            ) AS A
            ON A.CollectPlanNO = DH.CollectPlanNO

            WHERE DH.NextCollectPlanDate >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE DH.NextCollectPlanDate END)
            AND DH.NextCollectPlanDate <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE DH.NextCollectPlanDate END)
            AND DH.StoreCD = @StoreCD
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)

            AND DH.DeleteDateTime IS NULL
            AND EXISTS(SELECT A.StaffCD
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        AND A.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE A.StaffCD END)
                        )
            AND DH.InvalidFLG = 0
            AND DH.BillingType = 2

        ORDER BY StoreCD,StaffCD,NextCollectPlanDate,SalesDate
        ;
    END  
    ELSE
    BEGIN
        SELECT DH.StoreCD
              ,(SELECT top 1 A.StoreName
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.NextCollectPlanDate
                        AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName
              ,(SELECT TOP 1 A.StaffCD
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS StaffCD
              ,(SELECT TOP 1 B.StaffName
                        FROM M_Customer AS A
                        LEFT OUTER JOIN M_Staff AS B
                        ON B.StaffCD = A.StaffCD
                        AND B.ChangeDate <= DH.NextCollectPlanDate
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS StaffName

              ,CONVERT(varchar,DH.NextCollectPlanDate,111) AS NextCollectPlanDate   --����\���
    --          ,���L������i��NULL�̎���today)�|NextCollectPlanDate  ���ʁ��O�̎��̂ݕ\�� --�x������
                ,(CASE WHEN DH.NextCollectPlanDate < ISNULL(A.CollectDate, CONVERT(date, SYSDATETIME())) THEN
                     CONVERT(varchar, DATEDIFF(day, DH.NextCollectPlanDate, ISNULL(A.CollectDate, CONVERT(date, SYSDATETIME())))) 
                     ELSE NULL END) AS DelayDays
              ,DH.JuchuuNO
              ,DH.SalesNO
              ,DS.SalesDate
              ,DH.BillingNO
              ,DH.CollectPlanGaku	--�ō������z
              
              ,A.CollectDate
              ,A.CollectNO
              ,A.CollectGaku	--����z
              ,DH.CollectPlanGaku-A.CollectGaku AS KaisyuGaku	--����\��z=�ō������z�|����z
                
              ,DB.BillingCustomerCD
              ,(SELECT TOP 1 A.CustomerName
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS CustomerName
                        
              ,NULL SKUCD
              ,NULL SKUName
              ,DH.PaymentProgressKBN
			  ,@DetailOn AS DetailOn

            from D_CollectPlan DH
            LEFT OUTER JOIN D_Sales DS
            ON DS.SalesNO = DH.SalesNO
            LEFT OUTER JOIN D_Billing DB
            ON DB.BillingNO = DH.BillingNO
            LEFT OUTER JOIN (SELECT A.CollectPlanNO, MIN(A.ConfirmNO) AS ConfirmNO
                            FROM D_CollectBilling A 
                            GROUP BY A.CollectPlanNO) AS DC
            ON DC.CollectPlanNO = DH.CollectPlanNO
            LEFT OUTER JOIN D_PaymentConfirm DP
            ON DP.ConfirmNO = DC.ConfirmNO

            --�T�u�N�G���i��A�c����\��ԍ��P�ʂ̓���ԍ��A�����ς݊z��擾)
--            LEFT OUTER JOIN (SELECT DH.CollectPlanNO, SUM(ISNULL(DB.CollectGaku,0)) AS CollectGaku	2019.10.23 chg
            LEFT OUTER JOIN (SELECT DH.CollectPlanNO, SUM(ISNULL(DB.CollectAmount,0)) AS CollectGaku
                ,MIN(DP.CollectNO) AS CollectNO, MIN(DC.CollectDate) AS CollectDate
                from D_CollectPlan DH
                LEFT OUTER JOIN D_CollectBilling AS DB
                ON DB.CollectPlanNO = DH.CollectPlanNO
                AND DB.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_PaymentConfirm DP
                ON DP.ConfirmNO = DB.ConfirmNO
                AND DP.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Collect AS DC
                ON DC.CollectNO = DP.CollectNO
                AND DC.DeleteDateTime IS NULL
                GROUP BY DH.CollectPlanNO
            ) AS A
            ON A.CollectPlanNO = DH.CollectPlanNO
            
            WHERE DH.NextCollectPlanDate >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE DH.NextCollectPlanDate END)
            AND DH.NextCollectPlanDate <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE DH.NextCollectPlanDate END)
            AND DH.StoreCD = @StoreCD
            AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)

            AND DH.DeleteDateTime IS NULL
            AND EXISTS(SELECT A.StaffCD
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.NextCollectPlanDate 
                        AND A.DeleteFlg = 0
                        AND A.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE A.StaffCD END)
                        )
            AND DH.InvalidFLG = 0
            AND DH.BillingType = 2

        ORDER BY StoreCD,StaffCD,NextCollectPlanDate,SalesDate
        ;
    
    END
END


