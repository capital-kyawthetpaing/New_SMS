
/****** Object:  StoredProcedure [dbo].[D_Collect_SelectData]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Collect_SelectData]
GO


/****** Object:  StoredProcedure [D_Collect_SelectData]    */
CREATE PROCEDURE D_Collect_SelectData(
    -- Add the parameters for the stored procedure here
    @CollectNO  varchar(11),
    @ConfirmNO  varchar(11) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    --èCê≥ÅAè∆âÔéûÇ…égóp
    
    -- Insert statements for procedure here
    DECLARE @CollectCustomerCD varchar(13);
    
    IF ISNULL(@ConfirmNO,'') = ''
    BEGIN
        SELECT @CollectCustomerCD = DH.CollectCustomerCD
        FROM [D_Collect] AS DH
        WHERE DH.CollectNO = @CollectNO
        ;
        
        --éÊçûéÌï 
        IF ISNULL(@CollectCustomerCD,'') = ''
        BEGIN
            --âÊñ ì]ëóï\01
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
                  ,DW.NowCollectAmount AS OldConfirmAmount
                  ,DW.Minyukin
                  ,DW.SKUCD
                  ,DW.SKUName
                  ,DW.CommentInStore
                  ,DW.CollectPlanNO
                  ,DW.CollectPlanRows
                            
              FROM [D_Collect] AS DH
              LEFT OUTER JOIN (
              		--àÍéûÉèÅ[ÉNÉeÅ[ÉuÉãÅuD_WebCollectá@Åv(âÊñ ì]ëóï\01Ç≈ÅuD_WebCollectá@ÅvÇ∆ÇµÇƒégóp)
                        SELECT DW.CollectNO      --ì¸ã‡î‘çÜ
                            ,CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate   --êøãÅì˙
                            ,DB.BillingNO           --êøãÅî‘çÜ
                            ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate           --îÑè„ì˙
                            ,DCM.SalesNO            --îÑè„î‘çÜ
                            ,DCM.JuchuuNO           --éÛíçî‘çÜ
                            ,DWM.StoreCD             --ìXï‹
                            ,(SELECT top 1 A.StoreName
                              FROM M_Store A 
                              WHERE A.StoreCD = DWM.StoreCD AND A.ChangeDate <= DWM.WebCollectDate
                                    AND A.DeleteFlg = 0
                              ORDER BY A.ChangeDate desc) AS StoreName
                            ,DB.BillingCustomerCD   --êøãÅå⁄ãq
                            ,(SELECT TOP 1 A.CustomerName
                                    FROM M_Customer AS A
                                    WHERE A.CustomerCD = DB.BillingCustomerCD AND A.ChangeDate <= DB.BillingCloseDate
                                    AND A.DeleteFlg = 0
                                    ORDER BY A.ChangeDate DESC) AS BillingCustomerName
                            ,ISNULL(DBM.BillingGaku,0) AS BillingGaku       --êøãÅäz
                            ,ISNULL(DCBD.CollectAmount,0) AS CollectAmount
                            ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS NowCollectAmount --ç°âÒì¸ã‡äz
                            ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBD.CollectAmount,0) AS Minyukin              --ñ¢ì¸ã‡äz
                            ,ISNULL(DSM.SKUCD,DJM.SKUCD) As SKUCD      --SKUCD
                            ,ISNULL(DSM.SKUName,DJM.SKUName) As SKUName    --è§ïiñº
                            ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) As CommentInStore     --îıçl
                            ,CONVERT(varchar,DC.FirstCollectPlanDate,111) AS FirstCollectPlanDate    --ìñèââÒé˚ó\íËì˙
                            ,CONVERT(varchar,DC.NextCollectPlanDate,111) AS NextCollectPlanDate      --éüâÒé˚ó\íËì˙
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
               
            --ìñèââÒé˚ó\íËì˙,éüâÒé˚ó\íËì˙,êøãÅì˙,êøãÅî‘çÜ   
            ORDER BY FirstCollectPlanDate,NextCollectPlanDate,BillingCloseDate,BillingNO
            ;

        END
        
        --ì¸ã‡å⁄ãq
        ELSE
        BEGIN
        	--ì¸ã‡î‘çÜì¸óÕéûÅièCê≥ÅEè∆âÔéûÅj
            --âÊñ ì]ëóï\01 
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
                  ,DH.ConfirmAmount AS OldConfirmAmount
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
    BEGIN   --ì¸ã‡è¡çûî‘çÜì¸óÕéû
        --âÊñ ì]ëóï\02Å@ì¸ã‡å⁄ãq
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
              --,DW.CollectAmount AS ConfirmAmount
              ,DW.SumCollectAmount AS ConfirmAmount
              --,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
              --,DH.ConfirmSource - DW.CollectAmount AS ConfirmZan
              ,DH.ConfirmSource - DH.ConfirmAmount + DW.SumCollectAmount AS ConfirmZan
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
              --,DW.CollectAmount AS D_CollectAmount
              ,DW.CollectAmount - DW.NowCollectAmount AS D_CollectAmount
              ,DW.NowCollectAmount
              ,DW.NowCollectAmount AS OldConfirmAmount
              ,DW.Minyukin
              ,DW.SKUCD
              ,DW.SKUName
              ,DW.CommentInStore
              ,DW.CollectPlanNO
              ,DW.CollectPlanRows
          FROM 
          (
                --àÍéûÉèÅ[ÉNÉeÅ[ÉuÉãÅuD_Billingá@Åv(âÊñ ì]ëóï\02Ç≈ÅuD_Billingá@ÅvÇ∆ÇµÇƒégóp)
                --àÍéûÉèÅ[ÉNÉeÅ[ÉuÉãÅuD_Billingá@Åv
                SELECT DP.CollectNO      --ì¸ã‡î‘çÜ
                    ,DP.ConfirmNO
                    ,CONVERT(varchar,DP.CollectClearDate,111) AS CollectClearDate   --ì¸ã‡è¡çûì˙
                    ,CONVERT(varchar,DB.BillingCloseDate,111) AS BillingCloseDate   --êøãÅì˙
                    ,DB.BillingNO           --êøãÅî‘çÜ
                    ,CONVERT(varchar,DS.SalesDate,111) AS SalesDate    --îÑè„ì˙
                    ,DCM.SalesNO            --îÑè„î‘çÜ
                    ,DCM.JuchuuNO           --éÛíçî‘çÜ
                    ,DB.StoreCD             --ìXï‹
                    ,(SELECT top 1 A.StoreName
                      FROM M_Store A 
                      WHERE A.StoreCD = DB.StoreCD 
                      AND A.ChangeDate <= DB.BillingCloseDate
                      AND A.DeleteFlg = 0
                      ORDER BY A.ChangeDate desc) AS StoreName
                    ,DB.BillingCustomerCD   --êøãÅå⁄ãq
                    ,(SELECT TOP 1 A.CustomerName
                      FROM M_Customer AS A
                      WHERE A.CustomerCD = DB.BillingCustomerCD 
                      AND A.ChangeDate <= DB.BillingCloseDate
                      AND A.DeleteFlg = 0
                      ORDER BY A.ChangeDate DESC) AS BillingCustomerName
                    ,ISNULL(DBM.BillingGaku,0) AS BillingGaku       --êøãÅäz
                    --,ISNULL(DCBM.CollectAmount,0) AS CollectAmount	--ì¸ã‡çœäz
                    ,ISNULL(DCBD.CollectAmount,0) AS CollectAmount	--ì¸ã‡çœäz
                    --,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBM.CollectAmount,0) AS NowCollectAmount --ç°âÒì¸ã‡äz(ìoò^éûÇ…çXêVÇµÇΩâÊñ ÇÃè¡çûã‡äz)
                    ,ISNULL(DCBM.CollectAmount,0) AS NowCollectAmount      --ç°âÒì¸ã‡äz
                    ,ISNULL(DBM.BillingGaku,0) -ISNULL(DCBM.CollectAmount,0) AS Minyukin              --ñ¢ì¸ã‡äz
                    ,ISNULL(DSM.SKUCD,DJM.SKUCD) As SKUCD      --SKUCD
                    ,ISNULL(DSM.SKUName,DJM.SKUName) As SKUName    --è§ïiñº
                    ,ISNULL(DSM.CommentInStore,DJM.CommentInStore) As CommentInStore     --îıçl
                    ,CONVERT(varchar,DC.FirstCollectPlanDate,111) AS FirstCollectPlanDate    --ìñèââÒé˚ó\íËì˙
                    ,CONVERT(varchar,DC.NextCollectPlanDate,111) AS NextCollectPlanDate      --éüâÒé˚ó\íËì˙
                    ,DCM.CollectPlanNO
                    ,DCM.CollectPlanRows
                    ,SUM(ISNULL(DCBM.CollectAmount,0)) OVER() AS SumCollectAmount
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

                LEFT OUTER JOIN (SELECT SUM(DCBD.CollectAmount) AS CollectAmount
                                      , DCBD.CollectPlanNO
                                      , DCBD.CollectPlanRows
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
                
                WHERE DP.ConfirmNO = @ConfirmNO
                AND DB.BillingConfirmFlg = 1
                AND DB.BillingType = 2
                
            ) AS DW		--ÉÅÉCÉìÉeÅ[ÉuÉã
            LEFT OUTER JOIN D_Collect AS DH
            ON DW.CollectNO = DH.CollectNO
            AND DH.DeleteDateTime IS NULL
           
        --ìñèââÒé˚ó\íËì˙,éüâÒé˚ó\íËì˙,êøãÅì˙,êøãÅî‘çÜ   
        ORDER BY FirstCollectPlanDate,NextCollectPlanDate,BillingCloseDate,BillingNO
        ;

	END        
END

GO
