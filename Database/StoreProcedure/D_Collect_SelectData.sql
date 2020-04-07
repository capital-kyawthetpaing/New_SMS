 BEGIN TRY 
 Drop Procedure dbo.[D_Collect_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [D_Collect_SelectData]    */
CREATE PROCEDURE [dbo].[D_Collect_SelectData](
    -- Add the parameters for the stored procedure here
    @CollectNO  varchar(11),
    @ConfirmNO  varchar(11) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    --修正、照会時に使用
    
    -- Insert statements for procedure here
    DECLARE @CollectCustomerCD varchar(13);
    
    IF ISNULL(@CollectNO,'') <> ''
    BEGIN
        SELECT @CollectCustomerCD = DH.CollectCustomerCD
        FROM [D_Collect] AS DH
        WHERE DH.CollectNO = @CollectNO
        ;
        
        --取込種別
        IF ISNULL(@CollectCustomerCD,'') = ''
        BEGIN
            --画面転送表01
            SELECT DH.CollectNO
            	  ,(SELECT top 1 DP.ConfirmNO FROM D_PaymentConfirm AS DP WHERE DP.CollectNO = @CollectNO) AS ConfirmNO
                  ,DH.InputKBN
                  ,DH.StoreCD
                  ,(SELECT top 1 A.StoreName
                      FROM M_Store A 
                      WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.CollectDate
                            AND A.DeleteFlg = 0
                      ORDER BY A.ChangeDate desc) AS StoreName
                  ,DH.StaffCD
                  ,(SELECT TOP 1 A.StaffName
                            FROM M_Staff AS A
                            WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.CollectDate 
                            AND A.DeleteFlg = 0
                            ORDER BY A.ChangeDate DESC) AS StaffName
                  ,CONVERT(varchar,DH.InputDatetime,111) AS InputDatetime
                  ,DH.WebCollectNO
                  ,DH.WebCollectType
                  ,(SELECT M.PatternName FROM M_Settlement AS M WHERE M.PatternCD = DH.WebCollectType) AS WebCollectTypeName
                  ,DH.CollectCustomerCD
                  ,(SELECT TOP 1 A.CustomerName
                            FROM M_Customer AS A
                            WHERE A.CustomerCD = DH.CollectCustomerCD AND A.ChangeDate <= DH.CollectDate
                            AND A.DeleteFlg = 0
                            ORDER BY A.ChangeDate DESC) AS CustomerName
                            
                  ,CONVERT(varchar,DH.CollectDate,111) AS CollectDate
                  ,DH.PaymentMethodCD
                  ,(SELECT M.DenominationName FROM M_DenominationKBN AS M WHERE M.DenominationCD = DH.PaymentMethodCD) AS PaymentMethodName
                  ,DH.KouzaCD
                  ,CONVERT(varchar,DH.BillDate,111) AS BillDate
                  ,DH.CollectAmount
                  ,DH.FeeDeduction
                  ,DH.Deduction1
                  ,DH.Deduction2
                  ,DH.DeductionConfirm
                  ,DH.ConfirmSource
                  ,DH.ConfirmAmount
                  ,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
                  ,DH.Remark
                  ,DH.InsertOperator
                  ,DH.InsertDateTime
                  ,DH.UpdateOperator
                  ,DH.UpdateDateTime
                  ,DH.DeleteOperator
                  ,DH.DeleteDateTime
                  
                  ,DW.BillingCloseDate
                  ,DW.BillingNO
                  ,DW.SalesDate
                  ,DW.SalesNO
                  ,DW.JuchuuNO
                  ,DW.StoreName AS DetailStoreName
                  ,DW.BillingCustomerCD
                  ,DW.BillingCustomerName
                  ,DW.BillingGaku
                  ,DW.CollectAmount AS D_CollectAmount
                  ,DW.NowCollectAmount
                  ,DW.Minyukin
                  ,DW.SKUCD
                  ,DW.SKUName
                  ,DW.CommentInStore
                  ,DW.CollectPlanNO
                  ,DW.CollectPlanRows
                            
              FROM [D_Collect] AS DH
              LEFT OUTER JOIN (
              		--一時ワークテーブル「D_WebCollect①」(画面転送表01で「D_WebCollect①」として使用)
                        SELECT DW.CollectNO      --入金番号
                            ,CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate   --請求日
                            ,DB.BillingNO           --請求番号
                            ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate           --売上日
                            ,DCM.SalesNO            --売上番号
                            ,DCM.JuchuuNO           --受注番号
                            ,DWM.StoreCD             --店舗
                            ,(SELECT top 1 A.StoreName
                              FROM M_Store A 
                              WHERE A.StoreCD = DWM.StoreCD AND A.ChangeDate <= DWM.WebCollectDate
                                    AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc) AS StoreName
                            ,DB.BillingCustomerCD   --請求顧客
                            ,(SELECT TOP 1 A.CustomerName
                                    FROM M_Customer AS A
                                    WHERE A.CustomerCD = DB.BillingCustomerCD AND A.ChangeDate <= DB.BillingCloseDate
                                    AND A.DeleteFlg = 0
                                    ORDER BY A.ChangeDate DESC) AS BillingCustomerName
                            ,ISNULL(DBM.BillingGaku,0) AS BillingGaku       --請求額
                            ,ISNULL(DCBD.CollectAmount,0) AS CollectAmount
                            ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS NowCollectAmount --今回入金額
                            ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS Minyukin              --未入金額
                            ,ISNULL(DSM.SKUCD,DJM.SKUCD) As SKUCD      --SKUCD
                            ,ISNULL(DSM.SKUName,DJM.SKUName) As SKUName    --商品名
                            ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) As CommentInStore     --備考
                            ,CONVERT(varchar,DC.FirstCollectPlanDate,111) AS FirstCollectPlanDate    --当初回収予定日
                            ,CONVERT(varchar,DC.NextCollectPlanDate,111) AS NextCollectPlanDate      --次回収予定日
                            ,DWM.CollectPlanNO
                            ,DWM.CollectPlanRows
                            
                        FROM D_WebCollect AS DW
                        LEFT OUTER JOIN D_WebCollectDetails AS DWM
                        ON DWM.WebCollectNO = DW.WebCollectNO
                        AND DWM.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_CollectPlanDetails AS DCM
                        ON DCM.CollectPlanNO = DWM.CollectPlanNO
                        AND DCM.CollectPlanRows = DWM.CollectPlanRows
                        AND DCM.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_CollectPlan AS DC
                        ON DC.CollectPlanNO = DCM.CollectPlanNO
                        AND DC.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_BillingDetails AS DBM
                        ON DBM.CollectPlanNO = DCM.CollectPlanNO
                        AND DBM.CollectPlanRows = DCM.CollectPlanRows
                        AND DBM.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_Billing AS DB
                        ON DB.BillingNO = DBM.BillingNO
                        AND DB.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_SalesDetails AS DSM
                        ON DSM.SalesNO = DCM.SalesNO
                        AND DSM.SalesRows = DCM.SalesRows
                        AND DSM.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_JuchuuDetails AS DJM
                        ON DJM.JuchuuNO = DCM.JuchuuNO
                        AND DJM.JuchuuRows = DCM.JuchuuRows
                        AND DJM.DeleteDateTime IS NULL
                        LEFT OUTER JOIN D_Sales AS DS
                        ON DS.SalesNO = DCM.SalesNO
                        AND DS.DeleteDateTime IS NULL
                        
                        LEFT OUTER JOIN (SELECT SUM(DCBD.CollectAmount) AS CollectAmount
                            , DCBD.CollectPlanNO, DCBD.CollectPlanRows

                            FROM D_CollectBillingDetails AS DCBD
                            INNER JOIN D_CollectBilling AS DCB
                            ON DCB.CollectPlanNO = DCBD.CollectPlanNO
                            AND DCB.ConfirmNO = DCBD.ConfirmNO
                            AND DCB.DeleteDateTime IS NULL
                            
                            WHERE DCBD.DeleteDateTime IS NULL
                            GROUP BY DCBD.CollectPlanNO, DCBD.CollectPlanRows
                            ) AS DCBD
                        ON DCBD.CollectPlanNO = DCM.CollectPlanNO
                        AND DCBD.CollectPlanRows = DCM.CollectPlanRows
                        
                        WHERE DW.DeleteDateTime IS NULL
                        AND DW.CollectNO = @CollectNO
                        AND DB.BillingConfirmFlg = 1
                        AND DB.BillingType = 2
                ) AS DW
                ON DW.CollectNO = DH.CollectNO

                WHERE DH.CollectNO = @CollectNO
                --AND DH.DeleteDateTime IS NULL
               
            --当初回収予定日,次回収予定日,請求日,請求番号   
            ORDER BY FirstCollectPlanDate,NextCollectPlanDate,BillingCloseDate,BillingNO
            ;

        END
        
        --入金顧客
        ELSE
        BEGIN
        	--入金番号入力時（修正・照会時）
            --画面転送表01 
            SELECT DH.CollectNO
            	  ,NULL AS ConfirmNO
                  ,DH.InputKBN
                  ,DH.StoreCD
                  ,(SELECT top 1 A.StoreName
                      FROM M_Store A 
                      WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.CollectDate
                            AND A.DeleteFlg = 0
                      ORDER BY A.ChangeDate desc) AS StoreName
                  ,DH.StaffCD
                  ,(SELECT TOP 1 A.StaffName
                            FROM M_Staff AS A
                            WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.CollectDate 
                            AND A.DeleteFlg = 0
                            ORDER BY A.ChangeDate DESC) AS StaffName
                  ,CONVERT(varchar,DH.InputDatetime,111) AS InputDatetime
                  ,DH.WebCollectNO
                  ,DH.WebCollectType
                  ,(SELECT M.PatternName FROM M_Settlement AS M WHERE M.PatternCD = DH.WebCollectType) AS WebCollectTypeName
                  ,DH.CollectCustomerCD
                  ,(SELECT TOP 1 A.CustomerName
                            FROM M_Customer AS A
                            WHERE A.CustomerCD = DH.CollectCustomerCD AND A.ChangeDate <= DH.CollectDate
                            AND A.DeleteFlg = 0
                            ORDER BY A.ChangeDate DESC) AS CustomerName
                            
                  ,CONVERT(varchar,DH.CollectDate,111) AS CollectDate
                  ,DH.PaymentMethodCD
                  ,(SELECT M.DenominationName FROM M_DenominationKBN AS M WHERE M.DenominationCD = DH.PaymentMethodCD) AS PaymentMethodName
                  ,DH.KouzaCD
                  ,CONVERT(varchar,DH.BillDate,111) AS BillDate
                  ,DH.CollectAmount
                  ,DH.FeeDeduction
                  ,DH.Deduction1
                  ,DH.Deduction2
                  ,DH.DeductionConfirm
                  ,DH.ConfirmSource
                  ,DH.ConfirmAmount
                  ,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
                  ,DH.Remark
                  ,DH.InsertOperator
                  ,DH.InsertDateTime
                  ,DH.UpdateOperator
                  ,DH.UpdateDateTime
                  ,DH.DeleteOperator
                  ,DH.DeleteDateTime
                                    
              FROM [D_Collect] AS DH

              WHERE DH.CollectNO = @CollectNO
                --AND DH.DeleteDateTime IS NULL
              ;
        END
	END
	ELSE
	BEGIN	--入金消込番号入力時
        --画面転送表02　入金顧客
        SELECT DW.CollectNO
        	  ,DW.ConfirmNO
        	  
              ,DH.InputKBN
              ,DH.StoreCD
              ,(SELECT top 1 A.StoreName
                  FROM M_Store A 
                  WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.CollectDate
                        AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc) AS StoreName
              ,DH.StaffCD
              ,(SELECT TOP 1 A.StaffName
                        FROM M_Staff AS A
                        WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.CollectDate 
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS StaffName
              ,CONVERT(varchar,DH.InputDatetime,111) AS InputDatetime
              ,DH.WebCollectNO
              ,DH.WebCollectType
              ,(SELECT M.PatternName FROM M_Settlement AS M WHERE M.PatternCD = DH.WebCollectType) AS WebCollectTypeName
              ,DH.CollectCustomerCD
              ,(SELECT TOP 1 A.CustomerName
                        FROM M_Customer AS A
                        WHERE A.CustomerCD = DH.CollectCustomerCD AND A.ChangeDate <= DH.CollectDate
                        AND A.DeleteFlg = 0
                        ORDER BY A.ChangeDate DESC) AS CustomerName
                        
              ,CONVERT(varchar,DH.CollectDate,111) AS CollectDate
              ,DH.PaymentMethodCD
              ,(SELECT M.DenominationName FROM M_DenominationKBN AS M WHERE M.DenominationCD = DH.PaymentMethodCD) AS PaymentMethodName
              ,DH.KouzaCD
              ,CONVERT(varchar,DH.BillDate,111) AS BillDate
              ,DH.CollectAmount
              ,DH.FeeDeduction
              ,DH.Deduction1
              ,DH.Deduction2
              ,DH.DeductionConfirm
              ,DH.ConfirmSource
              --,DH.ConfirmAmount
              ,DW.CollectAmount AS ConfirmAmount
              --,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
              ,DH.ConfirmSource - DW.CollectAmount AS ConfirmZan
              ,DH.Remark
              ,DH.InsertOperator
              ,DH.InsertDateTime
              ,DH.UpdateOperator
              ,DH.UpdateDateTime
              ,DH.DeleteOperator
              ,DH.DeleteDateTime
              
              ,DW.CollectClearDate
              ,DW.BillingCloseDate
              ,DW.BillingNO
              ,DW.SalesDate
              ,DW.SalesNO
              ,DW.JuchuuNO
              ,DW.StoreName AS DetailStoreName
              ,DW.BillingCustomerCD
              ,DW.BillingCustomerName
              ,DW.BillingGaku
              ,DW.CollectAmount AS D_CollectAmount
              ,DW.NowCollectAmount
              ,DW.Minyukin
              ,DW.SKUCD
              ,DW.SKUName
              ,DW.CommentInStore
              ,DW.CollectPlanNO
              ,DW.CollectPlanRows
          FROM 
          (
                --一時ワークテーブル「D_Billing①」(画面転送表02で「D_Billing①」として使用)
                --一時ワークテーブル「D_Billing①」
                SELECT DP.CollectNO      --入金番号
                    ,DP.ConfirmNO
                    ,CONVERT(varchar,DP.CollectClearDate,111) AS CollectClearDate   --入金消込日
                    ,CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate   --請求日
                    ,DB.BillingNO           --請求番号
                    ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate    --売上日
                    ,DCM.SalesNO            --売上番号
                    ,DCM.JuchuuNO           --受注番号
                    ,DB.StoreCD             --店舗
                    ,(SELECT top 1 A.StoreName
                      FROM M_Store A 
                      WHERE A.StoreCD = DB.StoreCD AND A.ChangeDate <= DB.BillingCloseDate
                            AND A.DeleteFlg = 0
                      ORDER BY A.ChangeDate desc) AS StoreName
                    ,DB.BillingCustomerCD   --請求顧客
                    ,(SELECT TOP 1 A.CustomerName
                            FROM M_Customer AS A
                            WHERE A.CustomerCD = DB.BillingCustomerCD AND A.ChangeDate <= DB.BillingCloseDate
                            AND A.DeleteFlg = 0
                            ORDER BY A.ChangeDate DESC) AS BillingCustomerName
                    ,ISNULL(DBM.BillingGaku,0) AS BillingGaku       --請求額
                    ,ISNULL(DCBM.CollectAmount,0) AS CollectAmount	--入金済額
                    ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBM.CollectAmount,0) AS NowCollectAmount --今回入金額
                    ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBM.CollectAmount,0) AS Minyukin              --未入金額
                    ,ISNULL(DSM.SKUCD,DJM.SKUCD) As SKUCD      --SKUCD
                    ,ISNULL(DSM.SKUName,DJM.SKUName) As SKUName    --商品名
                    ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) As CommentInStore     --備考
                    ,CONVERT(varchar,DC.FirstCollectPlanDate,111) AS FirstCollectPlanDate    --当初回収予定日
                    ,CONVERT(varchar,DC.NextCollectPlanDate,111) AS NextCollectPlanDate      --次回収予定日
                    ,DCM.CollectPlanNO
                    ,DCM.CollectPlanRows
                FROM D_PaymentConfirm AS DP
                LEFT OUTER JOIN D_CollectBilling AS DCB
                ON DCB.ConfirmNO = DP.ConfirmNO
                AND DCB.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_CollectBillingDetails AS DCBM
                ON DCBM.ConfirmNO = DCB.ConfirmNO
                AND DCBM.CollectPlanNO = DCB.CollectPlanNO
                AND DCBM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_CollectPlanDetails AS DCM
                ON DCM.CollectPlanNO = DCBM.CollectPlanNO
                AND DCM.CollectPlanRows = DCBM.CollectPlanRows
                AND DCM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_CollectPlan AS DC
                ON DC.CollectPlanNO = DCM.CollectPlanNO
                AND DC.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_BillingDetails AS DBM
                ON DBM.CollectPlanNO = DCM.CollectPlanNO
                AND DBM.CollectPlanRows = DCM.CollectPlanRows
                AND DBM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Billing AS DB
                ON DB.BillingNO = DBM.BillingNO
                AND DB.DeleteDateTime IS NULL                
                LEFT OUTER JOIN D_SalesDetails AS DSM
                ON DSM.SalesNO = DCM.SalesNO
                AND DSM.SalesRows = DCM.SalesRows
                AND DSM.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_Sales AS DS
                ON DS.SalesNO = DCM.SalesNO
                AND DS.DeleteDateTime IS NULL
                LEFT OUTER JOIN D_JuchuuDetails AS DJM
                ON DJM.JuchuuNO = DCM.JuchuuNO
                AND DJM.JuchuuRows = DCM.JuchuuRows
                AND DJM.DeleteDateTime IS NULL
                
                WHERE DP.ConfirmNO = @ConfirmNO
                AND DB.BillingConfirmFlg = 1
                AND DB.BillingType = 2
                
            ) AS DW		--メインテーブル
            LEFT OUTER JOIN D_Collect AS DH
            ON DW.CollectNO = DH.CollectNO
            AND DH.DeleteDateTime IS NULL
           
        --当初回収予定日,次回収予定日,請求日,請求番号   
        ORDER BY FirstCollectPlanDate,NextCollectPlanDate,BillingCloseDate,BillingNO
        ;

	END        
END


