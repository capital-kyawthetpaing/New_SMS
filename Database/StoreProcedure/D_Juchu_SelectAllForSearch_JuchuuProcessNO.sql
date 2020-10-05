BEGIN TRY 
 Drop Procedure dbo.[D_Juchu_SelectAllForSearch_JuchuuProcessNO]
END try
BEGIN CATCH END CATCH 

/****** Object:  StoredProcedure [dbo].[D_Juchu_SelectAllForSearch_JuchuuProcessNO]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Juchu_SelectAllForSearch_JuchuuProcessNO]    */
CREATE PROCEDURE D_Juchu_SelectAllForSearch_JuchuuProcessNO(
    -- Add the parameters for the stored procedure here
    @JuchuuDateFrom  varchar(10),
    @JuchuuDateTo  varchar(10),
    @SalesDateFrom  varchar(10),
    @SalesDateTo  varchar(10),
    @BillingCloseDateFrom  varchar(10),
    @BillingCloseDateTo  varchar(10),
    @CollectClearDateFrom  varchar(10),
    @CollectClearDateTo  varchar(10),
    
    @ChkMihikiate tinyint,
    @ChkMiuriage tinyint,
    @ChkMiseikyu tinyint,
    @ChkMinyukin tinyint,
    @ChkAll tinyint,
    
    @ChkTujo tinyint,
    @ChkHenpin tinyint,
    @ChkGaisho int,
    @ChkTento int,
    @ChkWeb int,
    
    @ChkMihachu tinyint,
    @ChkNokiKaito  tinyint,
    @ChkMinyuka  tinyint,
    @ChkMisiire  tinyint,
    @ChkHachuAll  tinyint,
    
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80),
    @OrderCD  varchar(13),
    @OrderName  varchar(80),
    @StoreCD  varchar(4),
    @KanaName varchar(30) ,
    @Tel1 varchar(5) ,     
    @Tel2 varchar(4) ,     
    @Tel3 varchar(4) , 
    @StaffCD varchar(30) ,    
    
    @SKUName varchar(100),
    @SKUCD varchar(300),            --カンマ区切り
    @JanCD varchar(300),        --カンマ区切り
    
    @JuchuuProcessNOFrom  varchar(11),
    @JuchuuProcessNOTo varchar(11),
    @JuchuuNOFrom  varchar(11),
    @JuchuuNOTo varchar(11),
    @Operator  varchar(10)

)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @SYSDATETIME datetime;
    SET @SYSDATETIME = SYSDATETIME();
    
    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForJuchuuProcessNO]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForJuchuuProcessNO];
      END
      
    SELECT DH.JuchuuProcessNO
    	  ,DH.JuchuuNO
          ,ISNULL(DM.JuchuuRows,0) AS JuchuuRows
          ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
          ,DH.StoreCD
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.JuchuuDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.CustomerCD AS CustomerCD
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
              FROM M_Customer A 
              WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.JuchuuDate
              ORDER BY A.ChangeDate desc)  AS CustomerName 

          ,ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'') AS TEL
          ,(SELECT TOP 1 A.KanaName
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS KanaName

          ,(SELECT TOP 1 A.Tel11
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate  AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel11
          ,(SELECT TOP 1 A.Tel12
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel12
          ,(SELECT TOP 1 A.Tel13
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel13
          ,(SELECT TOP 1 A.Tel21
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel21
          ,(SELECT TOP 1 A.Tel22
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel22
          ,(SELECT TOP 1 A.Tel23
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate AND A.DeleteFlg = 0
                    ORDER BY A.ChangeDate DESC) AS Tel23

         ,(SELECT top 1 H.OrderCD 
               FROM D_OrderDetails AS M
               INNER JOIN D_Order AS H ON H.OrderNO = M.OrderNO
              AND H.DeleteDateTime IS NULL
              WHERE M.DeleteDateTime IS NULL
              AND DM.JuchuuNO = M.JuchuuNO
              AND DM.JuchuuRows = M.JuchuuRows
              AND DO.OrderDate = H.OrderDate
            ) AS OrderCD
         ,(SELECT top 1 MV.VendorName 
               FROM M_Vendor AS MV
               WHERE MV.VendorCD = (SELECT top 1 H.OrderCD 
                       FROM D_OrderDetails AS M
                       INNER JOIN D_Order AS H ON H.OrderNO = M.OrderNO
                      AND H.DeleteDateTime IS NULL
                      WHERE M.DeleteDateTime IS NULL
                      AND DM.JuchuuNO = M.JuchuuNO
                      AND DM.JuchuuRows = M.JuchuuRows
                      AND DO.OrderDate = H.OrderDate)
               AND MV.ChangeDate <= DO.OrderDate 
               AND MV.DeleteFlg = 0
               ORDER BY MV.ChangeDate desc
            ) AS OrderName
          
          ,(SELECT top 1 A.StaffName 
              FROM M_Staff A 
              WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.JuchuuDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StaffName

          ,CONVERT(varchar,DO.OrderDate, 111) AS OrderDate  --発注日
    
          ,(SELECT CONVERT(varchar, MAX(D.ArrivalDate), 111) 
                FROM D_Stock AS D 
                INNER JOIN D_Reserve AS R ON R.StockNO = D.StockNO
                AND R.ReserveKBN = 1
                AND R.DeleteDateTime IS NULL
                WHERE D.DeleteDateTime IS NULL
                AND DM.JuchuuNO = R.Number
                AND DM.JuchuuRows = R.NumberRows
            ) AS ArrivalDate    --入荷日

          ,(SELECT CONVERT(varchar, MAX(D.ArrivalPlanDate), 111) 
                FROM D_Stock AS D 
                INNER JOIN D_Reserve AS R ON R.StockNO = D.StockNO
                AND R.ReserveKBN = 1
                AND R.DeleteDateTime IS NULL
                WHERE D.DeleteDateTime IS NULL
                AND DM.JuchuuNO = R.Number
                AND DM.JuchuuRows = R.NumberRows
            ) AS ArrivalPlanDate   --入荷予定日

          ,(SELECT CONVERT(varchar, MAX(S.SalesDate), 111) 
            FROM D_SalesDetails AS D 
            INNER JOIN D_Sales AS S ON S.SalesNO = D.SalesNO
            AND S.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
              AND DM.JuchuuNO = D.JuchuuNO
              AND DM.JuchuuRows = D.JuchuuRows
            ) AS SalesDate  --売上日

          ,(SELECT CONVERT(varchar, MAX(BH.BillingCloseDate), 111) 
            FROM D_SalesDetails AS D 
            INNER JOIN D_BillingDetails AS BM ON BM.SalesNO = D.SalesNO
            AND BM.SalesRows = D.SalesRows
            AND BM.DeleteDateTime IS NULL
            INNER JOIN D_Billing AS BH ON BH.BillingNO = BM.BillingNO
            AND BH.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
              AND DM.JuchuuNO = D.JuchuuNO
              AND DM.JuchuuRows = D.JuchuuRows
            ) AS BillingCloseDate   --請求日

          ,(SELECT CONVERT(varchar, MAX(DP.CollectClearDate), 111) 
            FROM D_CollectPlan AS D 
            INNER JOIN D_CollectBilling AS DC ON DC.CollectPlanNO = D.CollectPlanNO
            AND DC.DeleteDateTime IS NULL
            INNER JOIN D_PaymentConfirm AS DP ON DP.ConfirmNO = DC.ConfirmNO
            AND DP.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = D.JuchuuNO
            ) AS CollectClearDate   --入金日
            
           ,(SELECT M.DenominationName FROM M_DenominationKBN AS M
            	WHERE M.DenominationCD = DH.PaymentMethodCD) AS DenominationName    --入金方法
            
            ,(CASE WHEN ISNULL(DC.CollectAmount,0)=0 THEN '未入金' 
                WHEN ISNULL(DC.CollectAmount,0) <> DH.JuchuuGaku THEN '一部入金'
                WHEN ISNULL(DC.CollectAmount,0) >= DH.JuchuuGaku THEN '入金完了'
                ELSE '' END) AS CollectAmount

          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO 
              AND A.ChangeDate <= DH.JuchuuDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ITemCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ITemCD
            ,(SELECT top 1 M.SKUCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUCD

          ,DM.JANCD
          ,DM.ColorName AS ColorName
          ,DM.SizeName AS SizeName
          ,DM.JuchuuSuu
          ,DM.JuchuuUnitPrice
          ,DM.JuchuuGaku
             
          ,DH.CommentOutStore
          ,DH.CommentInStore
          ,DM.CommentOutStore AS CommentOutStoreM
          ,DM.CommentInStore AS CommentInStoreM
          ,DH.ReturnFLG

          ,0 AS Check1  --ITemCD用チェック
          ,0 AS Check2  --SKUCD用チェック
          ,0 AS Check3  --JANCD用チェック
          ,1 AS DelFlg
    INTO #TableForJuchuuProcessNO 
    
    from D_Juchuu AS DH
/*    LEFT OUTER JOIN (SELECT DM.JuchuuNO, MIN(DM.JuchuuRows) AS JuchuuRows
            FROM D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            GROUP BY DM.JuchuuNO) AS DDM
        ON DDM.JuchuuNO = DH.JuchuuNO 
        */
    LEFT OUTER JOIN D_JuchuuDetails AS DM 
    ON  DM.JuchuuNO = DH.JuchuuNO 
    AND DM.DeleteDateTime IS NULL
    
    LEFT OUTER JOIN (SELECT M.JuchuuNO,M.JuchuuRows, MAX(H.OrderDate) AS OrderDate 
               FROM D_OrderDetails AS M
               INNER JOIN D_Order AS H ON H.OrderNO = M.OrderNO
              AND H.DeleteDateTime IS NULL
              WHERE M.DeleteDateTime IS NULL
              GROUP BY M.JuchuuNO, M.JuchuuRows
            ) AS DO ON DM.JuchuuNO = DO.JuchuuNO
            AND DM.JuchuuRows = DO.JuchuuRows
            
    LEFT OUTER JOIN (SELECT D.JuchuuNO, SUM(DC.CollectAmount) AS CollectAmount
            FROM D_CollectPlanDetails AS D 
            INNER JOIN D_CollectBillingDetails AS DC ON DC.CollectPlanNO = D.CollectPlanNO
            AND DC.CollectPlanRows = D.CollectPlanRows
            AND DC.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            GROUP BY D.JuchuuNO
            ) AS DC ON DH.JuchuuNO = DC.JuchuuNO   --入金状態
                     
    WHERE DH.JuchuuDate >= (CASE WHEN @JuchuuDateFrom <> '' THEN CONVERT(DATE, @JuchuuDateFrom) ELSE DH.JuchuuDate END)
        AND DH.JuchuuDate <= (CASE WHEN @JuchuuDateTo <> '' THEN CONVERT(DATE, @JuchuuDateTo) ELSE DH.JuchuuDate END)
        AND DH.JuchuuProcessNO >= (CASE WHEN @JuchuuProcessNOFrom <> '' THEN @JuchuuProcessNOFrom ELSE DH.JuchuuProcessNO END)
        AND DH.JuchuuProcessNO <= (CASE WHEN @JuchuuProcessNOTo <> '' THEN @JuchuuProcessNOTo ELSE DH.JuchuuProcessNO END)
        AND DH.JuchuuNO >= (CASE WHEN @JuchuuNOFrom <> '' THEN @JuchuuNOFrom ELSE DH.JuchuuNO END)
        AND DH.JuchuuNO <= (CASE WHEN @JuchuuNOTo <> '' THEN @JuchuuNOTo ELSE DH.JuchuuNO END)
        AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
        AND DH.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DH.StoreCD END)
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.JuchuuKBN IN (@ChkGaisho, @ChkTento, @ChkWeb)
        AND DH.DeleteDateTime IS NULL
        AND DH.JuchuuProcessNO IS NOT NULL

    --権限のある店舗のみ
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.ChangeDate <= DH.JuchuuDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.JuchuuDate
        AND MS.StoreCD = DH.StoreCD
        )
        
    ORDER BY DH.JuchuuProcessNO, DH.JuchuuNO
    ;

    ALTER TABLE [#TableForJuchuuProcessNO] ALTER COLUMN [JuchuuProcessNO] VARCHAR(11) NOT NULL;
    ALTER TABLE [#TableForJuchuuProcessNO] ADD PRIMARY KEY CLUSTERED ([JuchuuProcessNO],[JuchuuNO],[JuchuuRows])
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ;
    
    IF @ChkMihikiate = 1 OR @ChkMiseikyu = 1 OR @ChkMiuriage = 1 OR @ChkMinyukin = 1 
    BEGIN
    
        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 1
        ;
        
        --進捗状況ONの場合
        IF @ChkMihikiate = 1
        BEGIN
            --未引当
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from M_SKU AS M
                INNER JOIN D_JuchuuDetails AS DM ON DM.AdminNO = M.AdminNO
                AND DM.DeleteDateTime IS NULL
                Where M.DeleteFlg = 0    
                AND M.ZaikoKBN = 1
                AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows   
                AND M.ChangeDate <= #TableForJuchuuProcessNO.JuchuuDate  
                --引当をSelectできない受注明細がある
                AND NOT EXISTS(SELECT 1 FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
                    AND DR.DeleteDateTime IS NULL
                    ));
                
            --もしくは一部でも未引当数がある
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from M_SKU AS M
                INNER JOIN D_JuchuuDetails AS DM ON DM.AdminNO = M.AdminNO
                AND DM.DeleteDateTime IS NULL
                INNER JOIN (SELECT DR.Number, DR.NumberRows, SUM(DR.ReserveSu) AS ReserveSu 
                    FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.DeleteDateTime IS NULL
                    GROUP BY DR.Number, DR.NumberRows) AS DR
                    ON DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
                Where M.DeleteFlg = 0    
                AND M.ZaikoKBN = 1
                AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO   
                AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                AND M.ChangeDate <= #TableForJuchuuProcessNO.JuchuuDate  
                AND DR.ReserveSu <> DM.JuchuuSuu
                );
        END

        --未売上ONの場合
        --引当は出来ているが売上が全てできていない受注明細が１件でも含まれている受注を表示する
        IF @ChkMiuriage = 1 
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                --引当は出来ている
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
                INNER JOIN (SELECT DR.Number, DR.NumberRows, SUM(DR.ReserveSu) AS ReserveSu 
                    FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.DeleteDateTime IS NULL
                    GROUP BY DR.Number, DR.NumberRows) AS DR
                    ON DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DR.ReserveSu = DM.JuchuuSuu
            --売上明細をSelectできない受注明細がある
            AND NOT EXISTS(SELECT 1 FROM D_SalesDetails AS DS
                WHERE DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                AND DS.DeleteDateTime IS NULL
            ));
            
            
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                --引当は出来ている
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
                INNER JOIN (SELECT DR.Number, DR.NumberRows, SUM(DR.ReserveSu) AS ReserveSu 
                    FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.DeleteDateTime IS NULL
                    GROUP BY DR.Number, DR.NumberRows) AS DR
                    ON DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
                --もしくは一部でも未売上数がある
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DR.ReserveSu = DM.JuchuuSuu
            AND DS.SalesSU <> DM.JuchuuSuu	
			);
        END
        
        --未請求ON
        --売上は出来ているが未請求がある受注明細が１件でも含まれている受注を表示する
        IF @ChkMiseikyu = 1
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --売上はできている
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DS.SalesSU = DM.JuchuuSuu	
            AND 
                --請求明細をSelectできない受注明細がある
                NOT EXISTS (SELECT DB.SalesNO
                    FROM D_BillingDetails DB
                    INNER JOIN D_SalesDetails AS DSD
                    ON DSD.SalesNO = DB.SalesNO
                    AND DSD.SalesRows = DB.SalesRows
                    AND DSD.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    AND DSD.JuchuuNO = DM.JuchuuNO
                    AND DSD.JuchuuRows = DM.JuchuuRows
                    )
            );
            
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --売上はできている
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                
                --もしくは一部でも未売請求額がある
                INNER JOIN (SELECT DSD.JuchuuNO, DSD.JuchuuRows, SUM(DB.BillingGaku) AS BillingGaku
                    FROM D_BillingDetails DB
                    INNER JOIN D_SalesDetails AS DSD
                    ON DSD.SalesNO = DB.SalesNO
                    AND DSD.SalesRows = DB.SalesRows
                    AND DSD.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    GROUP BY DSD.JuchuuNO, DSD.JuchuuRows
                    ) AS DB ON DB.JuchuuNO = DM.JuchuuNO
                    AND DB.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DS.SalesSU = DM.JuchuuSuu	
            AND DB.BillingGaku <> DM.JuchuuGaku
			);
        END
        
        --未入金ONの場合
        IF @ChkMinyukin = 1
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --請求はできている
                INNER JOIN (SELECT DSD.JuchuuNO, DSD.JuchuuRows, SUM(DB.BillingGaku) AS BillingGaku
                    FROM D_BillingDetails DB
                    INNER JOIN D_SalesDetails AS DSD
                    ON DSD.SalesNO = DB.SalesNO
                    AND DSD.SalesRows = DB.SalesRows
                    AND DSD.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    GROUP BY DSD.JuchuuNO, DSD.JuchuuRows
                    ) AS DB ON DB.JuchuuNO = DM.JuchuuNO
                    AND DB.JuchuuRows = DM.JuchuuRows
                
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DB.BillingGaku = DM.JuchuuGaku
            AND 
                --入金明細をSelectできない受注明細がある
                NOT EXISTS (SELECT DB.CollectPlanNO
                    FROM D_CollectBillingDetails DB
                    INNER JOIN D_CollectPlanDetails AS DP
                    ON DP.CollectPlanNO = DB.CollectPlanNO
                    AND DP.CollectPlanRows = DB.CollectPlanRows
                    AND DP.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    AND DP.JuchuuNO = DM.JuchuuNO
                    AND DP.JuchuuRows = DM.JuchuuRows
                    )
            );
            
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --請求はできている
                INNER JOIN (SELECT DSD.JuchuuNO, DSD.JuchuuRows, SUM(DB.BillingGaku) AS BillingGaku
                    FROM D_BillingDetails DB
                    INNER JOIN D_SalesDetails AS DSD
                    ON DSD.SalesNO = DB.SalesNO
                    AND DSD.SalesRows = DB.SalesRows
                    AND DSD.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    GROUP BY DSD.JuchuuNO, DSD.JuchuuRows
                    ) AS DB ON DB.JuchuuNO = DM.JuchuuNO
                    AND DB.JuchuuRows = DM.JuchuuRows
                
                --もしくは一部でも未入金額がある
                INNER JOIN (SELECT DP.JuchuuNO, DP.JuchuuRows, SUM(DB.CollectAmount) AS CollectAmount
                    FROM D_CollectBillingDetails DB
                    INNER JOIN D_CollectPlanDetails AS DP
                    ON DP.CollectPlanNO = DB.CollectPlanNO
                    AND DP.CollectPlanRows = DB.CollectPlanRows
                    AND DP.DeleteDateTime IS NULL
                    WHERE DB.DeleteDateTime IS NULL
                    GROUP BY DP.JuchuuNO, DP.JuchuuRows
                    ) AS DC ON DC.JuchuuNO = DM.JuchuuNO
                    AND DC.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DB.BillingGaku = DM.JuchuuGaku
            AND DC.CollectAmount <> DM.JuchuuGaku
            );
        END
        
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE DelFlg = 1
        ;
    END
        
    --通常ON,返品ON
	IF @ChkTujo = 1 OR @ChkHenpin = 1
    BEGIN    
        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 1
        ;
        
        --通常
        IF @ChkTujo = 1 
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE ReturnFLG = 0
            ;
        END
        
        --返品
        IF @ChkHenpin = 1
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE ReturnFLG = 1
            ;
        END
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE DelFlg = 1
        ;
    END
    
 	IF @ChkMihachu = 1 OR @ChkNokiKaito = 1 OR @ChkMinyuka= 1 OR @ChkMisiire= 1
    BEGIN    
        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 1
        ;
        
        --未発注ON
        IF @ChkMihachu = 1
        BEGIN
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --発注必要な明細で未だ発注していないものがある
                INNER JOIN (SELECT DO.JuchuuNO, DO.JuchuuRows, SUM(DO.OrderSu) AS OrderSu
                    FROM D_OrderDetails AS DO
                    WHERE DO.DeleteDateTime IS NULL
                    GROUP BY DO.JuchuuNO, DO.JuchuuRows
                    ) AS DB ON DB.JuchuuNO = DM.JuchuuNO
                    AND DB.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DB.OrderSu < DM.JuchuuSuu
            AND DM.HikiateFLG <> 1
            );

        END
        
        --回答待ちON
        IF @ChkNokiKaito = 1
        BEGIN
        	--納期回答されていない発注がある
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                WHERE DM.DeleteDateTime IS NULL
                AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                AND DM.HikiateFLG <> 1
                AND NOT EXISTS(
                                
                --①D_ArrivalPlanが存在しない   
                    SELECT DO.JuchuuNO, DO.JuchuuRows
                    FROM D_OrderDetails AS DO
                    INNER JOIN D_ArrivalPlan AS DA
                    ON DA.ArrivalPlanKBN = 1
                    AND DA.Number = DO.OrderNO
                    AND DA.NumberRows = DO.OrderRows
                    AND DA.DeleteDatetime IS NULL
                    AND DA.LastestFLG = 1

                    WHERE DO.DeleteDateTime IS NULL
                    AND DO.JuchuuNO = DM.JuchuuNO
                    AND DO.JuchuuRows = DM.JuchuuRows
                    )
            );
            
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DM.HikiateFLG <> 1
            AND EXISTS(
            	--D_ArrivalPlanが存在しても、
                    SELECT DO.JuchuuNO, DO.JuchuuRows
                    FROM D_OrderDetails AS DO
                    INNER JOIN D_ArrivalPlan AS DA
                    ON DA.ArrivalPlanKBN = 1
                    AND DA.Number = DO.OrderNO
                    AND DA.NumberRows = DO.OrderRows
                    AND DA.DeleteDatetime IS NULL
                    AND DA.LastestFLG = 1
                    LEFT OUTER JOIN M_MultiPorpose AS MM
                    ON MM.ID = 206
                    AND MM.Char1 = DA.ArrivalPlanCD

                    WHERE DO.DeleteDateTime IS NULL
                    AND DO.JuchuuNO = DM.JuchuuNO
                    AND DO.JuchuuRows = DM.JuchuuRows
                    
                    AND ((DA.ArrivalPlanCD IS NULL
                    AND DA.ArrivalPlanDate IS NULL
                    AND ISNULL(DA.ArrivalPlanMonth,0) = 0)
                    	OR
                    	MM.Num2 IN (1,2,4,6))
                    )
            );
        END
        
        --未入荷ON
        IF @ChkMinyuka= 1 
        BEGIN
        	--納期回答されているが入荷が未だ
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DM.HikiateFLG <> 1
            AND EXISTS(
            	--①D_ArrivalPlanが存在する、
                    SELECT DO.JuchuuNO, DO.JuchuuRows
                    FROM D_OrderDetails AS DO
                    INNER JOIN D_ArrivalPlan AS DA
                    ON DA.ArrivalPlanKBN = 1
                    AND DA.Number = DO.OrderNO
                    AND DA.NumberRows = DO.OrderRows
                    AND DA.DeleteDatetime IS NULL
                    AND DA.LastestFLG = 1
                    
                    LEFT OUTER JOIN M_MultiPorpose AS MM
                    ON MM.ID = 206
                    AND MM.Char1 = DA.ArrivalPlanCD

                    WHERE DO.DeleteDateTime IS NULL
                    AND DO.JuchuuNO = DM.JuchuuNO
                    AND DO.JuchuuRows = DM.JuchuuRows
                    
                    AND (DA.ArrivalPlanDate IS NOT NULL
                        OR (ISNULL(DA.ArrivalPlanMonth,0) <> 0
                        AND MM.Num2 IN (1,2,4,6)))
                    --①のD_ArrivalPlanに対して、入荷が未だのものがある
                    AND DA.ArrivalPlanSu<> DA.ArrivalSu
                    )
            );
        END

        --未仕入ON
        IF @ChkMisiire= 1 
        BEGIN
        	--入荷したけど仕入が未だ以下のSelectで①の場合、抽出対象とする
        	--①D_PurchaseDetailsが存在しない
            UPDATE #TableForJuchuuProcessNO
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                WHERE DM.DeleteDateTime IS NULL
                AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                AND DM.HikiateFLG <> 1
                AND NOT EXISTS(
                                
                --①D_ArrivalPlanが存在しない   
                    SELECT DO.JuchuuNO, DO.JuchuuRows
                    FROM D_OrderDetails AS DO
                    INNER JOIN D_ArrivalPlan AS DA
                    ON DA.ArrivalPlanKBN = 1
                    AND DA.Number = DO.OrderNO
                    AND DA.NumberRows = DO.OrderRows
                    AND DA.DeleteDatetime IS NULL
                    AND DA.LastestFLG = 1
                    AND DA.ArrivalPlanSu = DA.ArrivalSu
                    INNER JOIN D_ArrivalDetails AS DD
                    ON DD.ArrivalPlanNO = DA.ArrivalPlanNO
                    AND DD.DeleteDateTime IS NULL
                    INNER JOIN D_PurchaseDetails AS DP
                    ON DP.ArrivalNO = DD.ArrivalNO
                    AND DP.DeleteDateTime IS NULL

                    WHERE DO.DeleteDateTime IS NULL
                    AND DO.JuchuuNO = DM.JuchuuNO
                    AND DO.JuchuuRows = DM.JuchuuRows
                    )
            );
        END
                
        DELETE FROM #TableForJuchuuProcessNO
        WHERE DelFlg = 1
        ;
    END

    IF ISNULL(@SKUName,'') <> ''
    BEGIN

        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 1
        ;
        
        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 0
        WHERE EXISTS(
            SELECT DM.JuchuuNO                                                                                          
            FROM D_JuchuuDetails AS DM 
            WHERE DM.SKUName LIKE '%' + @SKUName + '%'
            AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
            AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
            AND DM.DeleteDateTime IS NULL
        );
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE DelFlg = 1
        ;
    END
    /*
    IF ISNULL(@KanaName,'') <> ''
    BEGIN

        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 1
        ;
        
        UPDATE #TableForJuchuuProcessNO
        SET DelFlg = 0
        WHERE KanaName LIKE '%' + @KanaName + '%'
        ;
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE DelFlg = 1
        ;
    END
    */
    
    UPDATE #TableForJuchuuProcessNO
    SET DelFlg = 1
    ;
    
    UPDATE #TableForJuchuuProcessNO
    SET DelFlg = 0
    WHERE (ISNULL(Tel11,'') = (CASE WHEN @Tel1 <> '' THEN @Tel1 ELSE ISNULL(Tel11,'') END) 
    	OR ISNULL(Tel21,'') = (CASE WHEN @Tel1 <> '' THEN @Tel1 ELSE ISNULL(Tel21,'') END)) 
    AND (ISNULL(Tel12,'') = (CASE WHEN @Tel2 <> '' THEN @Tel2 ELSE ISNULL(Tel12,'') END) 
        OR ISNULL(Tel22,'') = (CASE WHEN @Tel2 <> '' THEN @Tel2 ELSE ISNULL(Tel22,'') END)) 
    AND (ISNULL(Tel13,'') = (CASE WHEN @Tel3 <> '' THEN @Tel3 ELSE ISNULL(Tel13,'') END) 
        OR ISNULL(Tel23,'') = (CASE WHEN @Tel3 <> '' THEN @Tel3 ELSE ISNULL(Tel23,'') END))
    AND ISNULL(KanaName,'') LIKE '%' + (CASE WHEN @KanaName <> '' THEN @KanaName ELSE ISNULL(KanaName,'') END) + '%'
    AND ISNULL(OrderCD,'') = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE ISNULL(OrderCD,'') END)
    AND ISNULL(OrderName,'') LIKE '%' + (CASE WHEN @OrderName <> '' THEN @OrderName ELSE ISNULL(OrderName,'') END) + '%'
    AND ISNULL(SalesDate,'') >= (CASE WHEN @SalesDateFrom <> '' THEN @SalesDateFrom ELSE ISNULL(SalesDate,'') END)
    AND ISNULL(SalesDate,'') <= (CASE WHEN @SalesDateTo <> '' THEN @SalesDateTo ELSE ISNULL(SalesDate,'') END)
    AND ISNULL(BillingCloseDate,'') >= (CASE WHEN @BillingCloseDateFrom <> '' THEN @BillingCloseDateFrom ELSE ISNULL(BillingCloseDate,'') END)
    AND ISNULL(BillingCloseDate,'') <= (CASE WHEN @BillingCloseDateTo <> '' THEN @BillingCloseDateTo ELSE ISNULL(BillingCloseDate,'') END)
    AND ISNULL(CollectClearDate,'') >= (CASE WHEN @CollectClearDateFrom <> '' THEN @CollectClearDateFrom ELSE ISNULL(CollectClearDate,'') END)
    AND ISNULL(CollectClearDate,'') <= (CASE WHEN @CollectClearDateTo <> '' THEN @CollectClearDateTo ELSE ISNULL(CollectClearDate,'') END)
    ;

    DELETE FROM #TableForJuchuuProcessNO
    WHERE DelFlg = 1
    ;

    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    /*
    --ITEMより条件に合わないデータをテーブルから削除
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuProcessNO
        SET Check1 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @ITemCD, @INDEX) = 0
            BEGIN
                IF LEN(@ITemCD)-@INDEX >= 0
                BEGIN
                    --データが一つのみの場合
                    UPDATE #TableForJuchuuProcessNO
                    SET Check1 = 0
                    WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
                
                UPDATE #TableForJuchuuProcessNO
                SET Check1 = 0
                WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE  Check1 = 1
        ;
    END;
    */
    --SKUCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuProcessNO
        SET Check2 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @SKUCD, @INDEX) = 0
            BEGIN
                IF LEN(@SKUCD)-@INDEX >= 0
                BEGIN
                    --データが一つのみの場合
                    UPDATE #TableForJuchuuProcessNO
                    SET Check2 = 0
                    WHERE EXISTS(
                        SELECT DM.JuchuuNO                                                                                          
                        FROM D_JuchuuDetails AS DM 
                        WHERE DM.SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                        AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                        AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                        AND DM.DeleteDateTime IS NULL
                    );
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
                
                UPDATE #TableForJuchuuProcessNO
                SET Check2 = 0
                WHERE EXISTS(
                    SELECT DM.JuchuuNO                                                                                          
                    FROM D_JuchuuDetails AS DM 
                    WHERE DM.SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                    AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                    AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows  
                    AND DM.DeleteDateTime IS NULL
                );
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuProcessNO
        SET Check3 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @JANCD, @INDEX) = 0
            BEGIN
                IF LEN(@JANCD)-@INDEX >= 0
                BEGIN
                    --データが一つのみの場合
                    UPDATE #TableForJuchuuProcessNO
                    SET Check3 = 0
                    WHERE EXISTS(
                        SELECT DM.JuchuuNO                                                                                          
                        FROM D_JuchuuDetails AS DM 
                        WHERE DM.JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                        AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                        AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                        AND DM.DeleteDateTime IS NULL
                    );
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
                
                UPDATE #TableForJuchuuProcessNO
                SET Check3 = 0
                WHERE EXISTS(
                    SELECT DM.JuchuuNO                                                                                          
                    FROM D_JuchuuDetails AS DM 
                    WHERE DM.JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                    AND DM.JuchuuNO = #TableForJuchuuProcessNO.JuchuuNO
                    AND DM.JuchuuRows = #TableForJuchuuProcessNO.JuchuuRows 
                    AND DM.DeleteDateTime IS NULL
                );
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuProcessNO
        WHERE  Check3 = 1
        ;
    END;
    
    SELECT (CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                 DH.StoreCD
                 ELSE '' END) AS StoreCD
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.StoreName
                   ELSE '' END) AS StoreName
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CustomerCD
                   ELSE '' END) AS CustomerCD
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CustomerName
                   ELSE '' END) AS CustomerName
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.TEL
                   ELSE '' END) AS TEL
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.JuchuuDate
                   ELSE '' END) AS JuchuuDate
            ,DH.JuchuuNO AS JuchuuNO
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.JuchuuProcessNO
                   ELSE '' END) AS JuchuuProcessNO
            ,DH.SKUName
            ,DH.JANCD
            ,DH.SizeName
            ,DH.ColorName
            ,DH.JuchuuSuu
            ,DH.JuchuuUnitPrice
            ,DH.JuchuuGaku
             
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CommentOutStore
                   ELSE '' END) AS CommentOutStore
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CommentInStore
                   ELSE '' END) AS CommentInStore
            ,DH.CommentOutStoreM
            ,DH.CommentInStoreM
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.StaffName
                   ELSE '' END) AS StaffName
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.DenominationName
                   ELSE '' END) AS DenominationName
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CollectAmount
                   ELSE '' END) AS CollectAmount
            ,DH.OrderDate
            ,DH.OrderCD
            ,DH.OrderName
            ,DH.ArrivalPlanDate
            ,DH.ArrivalDate
            ,(CASE WHEN (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.SalesDate
                   ELSE '' END) AS SalesDate
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.BillingCloseDate
                   ELSE '' END) AS BillingCloseDate
            ,(CASE WHEN  (DM.JuchuuNO = DH.JuchuuNO AND DH.JuchuuRows = DM.JuchuuRows) THEN
                   DH.CollectClearDate
                   ELSE '' END) AS CollectClearDate
            
    FROM #TableForJuchuuProcessNO AS DH
    LEFT OUTER JOIN (SELECT JuchuuProcessNO, JuchuuNO, JuchuuRows
                           ,ROW_NUMBER() OVER(PARTITION BY JuchuuProcessNO ORDER BY JuchuuNO,JuchuuRows) AS ROWNO
                     FROM #TableForJuchuuProcessNO)AS DM 
                     ON DM.JuchuuProcessNO = DH.JuchuuProcessNO
                     AND DM.JuchuuNO = DH.JuchuuNO
                     AND DM.ROWNO = 1
    ORDER BY DH.JuchuuProcessNO, DH.JuchuuNO
    ;
END

GO
