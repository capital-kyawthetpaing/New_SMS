 BEGIN TRY 
 Drop Procedure dbo.[D_Juchu_SelectAllForShoukai]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Juchu_SelectAllForShoukai]    */
CREATE PROCEDURE [dbo].[D_Juchu_SelectAllForShoukai](
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
    
    @ChkMihachu tinyint,
    @ChkNokiKaito  tinyint,
    @ChkMinyuka  tinyint,
    @ChkMisiire  tinyint,
    @ChkHachuAll  tinyint,
    
    @CustomerCD  varchar(13),
    @OrderCD  varchar(13),
    @StoreCD  varchar(4),
    @KanaName varchar(30) ,
    @Tel1 varchar(5) ,     
    @Tel2 varchar(4) ,     
    @Tel3 varchar(4) , 
    @StaffCD varchar(30) ,    
    
    @SKUName varchar(100),
    @SKUCD varchar(300),            --�J���}��؂�
    @JanCD varchar(300),        --�J���}��؂�
    
    @JuchuuNOFrom  varchar(11),
    @JuchuuNOTo varchar(11),
    @Operator  varchar(10),
    @PC  varchar(30)

)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @SYSDATETIME datetime;
    SET @SYSDATETIME = SYSDATETIME();
    
    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForJuchuuShoukai]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForJuchuuShoukai];
      END
      
    SELECT DH.JuchuuNO
          ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.JuchuuDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.CustomerCD
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
          FROM M_Customer A 
          WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.JuchuuDate
          ORDER BY A.ChangeDate desc) AS CustomerName 
          ,ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'') AS TEL
          ,(SELECT TOP 1 A.KanaName
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS KanaName

          ,(SELECT TOP 1 A.Tel11
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel11
          ,(SELECT TOP 1 A.Tel12
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel12
          ,(SELECT TOP 1 A.Tel13
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel13
          ,(SELECT TOP 1 A.Tel21
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel21
          ,(SELECT TOP 1 A.Tel22
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel22
          ,(SELECT TOP 1 A.Tel23
                    FROM M_Customer AS A
                    WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                    ORDER BY A.ChangeDate DESC) AS Tel23

         ,(SELECT top 1 H.OrderCD 
           FROM D_OrderDetails AS M
           INNER JOIN D_Order AS H ON H.OrderNO = M.OrderNO
          AND H.DeleteDateTime IS NULL
          WHERE M.DeleteDateTime IS NULL
          AND DH.JuchuuNO = M.JuchuuNO
            ) AS OrderCD
            
          ,DH.JuchuuGaku
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.JuchuuDate
          ORDER BY A.ChangeDate desc) AS StaffName

         ,(SELECT CONVERT(varchar, MAX(H.OrderDate), 111) 
           FROM D_OrderDetails AS M
           INNER JOIN D_Order AS H ON H.OrderNO = M.OrderNO
          AND H.DeleteDateTime IS NULL
          WHERE M.DeleteDateTime IS NULL
          AND DH.JuchuuNO = M.JuchuuNO
            ) AS OrderDate  --������
    
          ,(SELECT CONVERT(varchar, MAX(D.ArrivalDate), 111) 
            FROM D_Stock AS D 
            INNER JOIN D_Reserve AS R ON R.StockNO = D.StockNO
            AND R.ReserveKBN = 1
            AND R.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = R.Number
            ) AS ArrivalDate    --���ד�

          ,(SELECT CONVERT(varchar, MAX(D.ArrivalPlanDate), 111) 
            FROM D_Stock AS D 
            INNER JOIN D_Reserve AS R ON R.StockNO = D.StockNO
            AND R.ReserveKBN = 1
            AND R.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = R.Number
            ) AS ArrivalPlanDate   --���ח\���

          ,(SELECT CONVERT(varchar, MAX(S.SalesDate), 111) 
            FROM D_SalesDetails AS D 
            INNER JOIN D_Sales AS S ON S.SalesNO = D.SalesNO
            AND S.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = D.JuchuuNO
            ) AS SalesDate  --�����

          ,(SELECT CONVERT(varchar, MAX(BH.BillingCloseDate), 111) 
            FROM D_SalesDetails AS D 
            INNER JOIN D_BillingDetails AS BM ON BM.SalesNO = D.SalesNO
            AND BM.SalesRows = D.SalesRows
            AND BM.DeleteDateTime IS NULL
            INNER JOIN D_Billing AS BH ON BH.BillingNO = BM.BillingNO
            AND BH.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = D.JuchuuNO
            ) AS BillingCloseDate   --������

          ,(SELECT CONVERT(varchar, MAX(DP.CollectClearDate), 111) 
            FROM D_CollectPlan AS D 
            INNER JOIN D_CollectBilling AS DC ON DC.CollectPlanNO = D.CollectPlanNO
            AND DC.DeleteDateTime IS NULL
            INNER JOIN D_PaymentConfirm AS DP ON DP.ConfirmNO = DC.ConfirmNO
            AND DP.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            AND DH.JuchuuNO = D.JuchuuNO
            ) AS CollectClearDate   --�����
            
           ,(SELECT M.DenominationName FROM M_DenominationKBN AS M
            WHERE M.DenominationCD = DH.PaymentMethodCD) AS DenominationName    --������@
            
            ,(CASE WHEN ISNULL(DC.CollectAmount,0)=0 THEN '������' 
                WHEN ISNULL(DC.CollectAmount,0) <> DH.JuchuuGaku THEN '�ꕔ����'
                WHEN ISNULL(DC.CollectAmount,0) >= DH.JuchuuGaku THEN '�������'
                ELSE '' END) AS CollectAmount

          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SKUName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO 
              AND A.ChangeDate <= DH.JuchuuDate 
              ORDER BY A.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ITemCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS ITemCD
            ,(SELECT top 1 M.SKUCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.JANCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.MakerItem 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.JuchuuDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS MakerItem
             
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.ColorName ELSE A.ColorName END) AS ColorName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.JuchuuDate 
              ORDER BY A.ChangeDate desc) AS ColorName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SizeName ELSE A.SizeName END) AS SizeName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.JuchuuDate 
              ORDER BY A.ChangeDate desc) AS SizeName
             
              ,DH.CommentOutStore
              ,DH.CommentInStore
              ,DH.ReturnFLG

          ,0 AS Check1  --ITemCD�p�`�F�b�N
          ,0 AS Check2  --SKUCD�p�`�F�b�N
          ,0 AS Check3  --JANCD�p�`�F�b�N
          ,1 AS DelFlg
    INTO #TableForJuchuuShoukai 
    
    from D_Juchuu AS DH
    LEFT OUTER JOIN (SELECT DM.JuchuuNO, MIN(DM.JuchuuRows) AS JuchuuRows
            FROM D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            GROUP BY DM.JuchuuNO) AS DDM
        ON DDM.JuchuuNO = DH.JuchuuNO 
    LEFT OUTER JOIN D_JuchuuDetails AS DM 
    ON  DM.JuchuuNO = DDM.JuchuuNO 
    AND  DM.JuchuuRows = DDM.JuchuuRows 
    
    LEFT OUTER JOIN (SELECT D.JuchuuNO, SUM(DC.CollectAmount) AS CollectAmount
            FROM D_CollectPlanDetails AS D 
            INNER JOIN D_CollectBillingDetails AS DC ON DC.CollectPlanNO = D.CollectPlanNO
            AND DC.CollectPlanRows = D.CollectPlanRows
            AND DC.DeleteDateTime IS NULL
            WHERE D.DeleteDateTime IS NULL
            GROUP BY D.JuchuuNO
            ) AS DC ON DH.JuchuuNO = DC.JuchuuNO   --������
                     
    WHERE DH.JuchuuDate >= (CASE WHEN @JuchuuDateFrom <> '' THEN CONVERT(DATE, @JuchuuDateFrom) ELSE DH.JuchuuDate END)
        AND DH.JuchuuDate <= (CASE WHEN @JuchuuDateTo <> '' THEN CONVERT(DATE, @JuchuuDateTo) ELSE DH.JuchuuDate END)
        AND DH.JuchuuNO >= (CASE WHEN @JuchuuNOFrom <> '' THEN @JuchuuNOFrom ELSE DH.JuchuuNO END)
        AND DH.JuchuuNO <= (CASE WHEN @JuchuuNOTo <> '' THEN @JuchuuNOTo ELSE DH.JuchuuNO END)
        AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)

        AND DH.DeleteDateTime IS NULL
             
    ORDER BY DH.JuchuuNO
    ;

    ALTER TABLE [#TableForJuchuuShoukai] ADD PRIMARY KEY CLUSTERED ([JuchuuNO])
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ;
    
    IF @ChkMihikiate = 1 OR @ChkMiseikyu = 1 OR @ChkMiuriage = 1 OR @ChkMinyukin = 1 
    BEGIN
    
        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 1
        ;
        
        --�i����ON�̏ꍇ
        IF @ChkMihikiate = 1
        BEGIN
            --������
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from M_SKU AS M
                INNER JOIN D_JuchuuDetails AS DM ON DM.AdminNO = M.AdminNO
                AND DM.DeleteDateTime IS NULL
                Where M.DeleteFlg = 0    
                AND M.ZaikoKBN = 1
                AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO   
                AND M.ChangeDate <= #TableForJuchuuShoukai.JuchuuDate  
                --������Select�ł��Ȃ��󒍖��ׂ�����
                AND NOT EXISTS(SELECT 1 FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
                    AND DR.DeleteDateTime IS NULL
                    ));
                
            --������͈ꕔ�ł��������������
            UPDATE #TableForJuchuuShoukai
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
                AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO   
                AND M.ChangeDate <= #TableForJuchuuShoukai.JuchuuDate  
                AND DR.ReserveSu <> DM.JuchuuSuu
                );
        END

        --������ON�̏ꍇ
        --�����͏o���Ă��邪���オ�S�Ăł��Ă��Ȃ��󒍖��ׂ��P���ł�܂܂�Ă���󒍂�\������
        IF @ChkMiuriage = 1 
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                --�����͏o���Ă���
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
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DR.ReserveSu = DM.JuchuuSuu
            --���㖾�ׂ�Select�ł��Ȃ��󒍖��ׂ�����
            AND NOT EXISTS(SELECT 1 FROM D_SalesDetails AS DS
                WHERE DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                AND DS.DeleteDateTime IS NULL
            ));
            
            
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                --�����͏o���Ă���
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
                INNER JOIN (SELECT DR.Number, DR.NumberRows, SUM(DR.ReserveSu) AS ReserveSu 
                    FROM D_Reserve DR   
                    WHERE DR.ReserveKBN = 1
                    AND DR.DeleteDateTime IS NULL
                    GROUP BY DR.Number, DR.NumberRows) AS DR
                    ON DR.Number = DM.JuchuuNO
                    AND DR.NumberRows = DM.JuchuuRows
                --������͈ꕔ�ł�����㐔������
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DR.ReserveSu = DM.JuchuuSuu
            AND DS.SalesSU <> DM.JuchuuSuu	
			);
        END
        
        --������ON
        --����͏o���Ă��邪������������󒍖��ׂ��P���ł�܂܂�Ă���󒍂�\������
        IF @ChkMiseikyu = 1
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --����͂ł��Ă���
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DS.SalesSU = DM.JuchuuSuu	
            AND 
                --�������ׂ�Select�ł��Ȃ��󒍖��ׂ�����
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
			
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --����͂ł��Ă���
                INNER JOIN (SELECT DS.JuchuuNO, DS.JuchuuRows, SUM(DS.SalesSU) AS SalesSU
                	FROM D_SalesDetails AS DS
                	WHERE DS.DeleteDateTime IS NULL
                	GROUP BY DS.JuchuuNO, DS.JuchuuRows
                ) AS DS
                ON DS.JuchuuNO = DM.JuchuuNO
                AND DS.JuchuuRows = DM.JuchuuRows
                
                --������͈ꕔ�ł���������z������
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
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DS.SalesSU = DM.JuchuuSuu	
            AND DB.BillingGaku <> DM.JuchuuGaku
			);
        END
        
        --������ON�̏ꍇ
        IF @ChkMinyukin = 1
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --�����͂ł��Ă���
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
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DB.BillingGaku = DM.JuchuuGaku
            AND 
                --������ׂ�Select�ł��Ȃ��󒍖��ׂ�����
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
			
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(

                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --�����͂ł��Ă���
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
                
                --������͈ꕔ�ł������z������
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
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DB.BillingGaku = DM.JuchuuGaku
            AND DC.CollectAmount <> DM.JuchuuGaku
			);
        END
        
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE DelFlg = 1
        ;
    END
        
    --�ʏ�ON,�ԕiON
	IF @ChkTujo = 1 OR @ChkHenpin = 1
    BEGIN    
        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 1
        ;
        
        --�ʏ�
        IF @ChkTujo = 1 
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE ReturnFLG = 0
            ;
        END
        
        --�ԕi
        IF @ChkHenpin = 1
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE ReturnFLG = 1
            ;
        END
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
 	IF @ChkMihachu = 1 OR @ChkNokiKaito = 1 OR @ChkMinyuka= 1 OR @ChkMisiire= 1
    BEGIN    
        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 1
        ;
        
        --������ON
        IF @ChkMihachu = 1
        BEGIN
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                --�����K�v�Ȗ��ׂŖ����������Ă��Ȃ���̂�����
                INNER JOIN (SELECT DO.JuchuuNO, DO.JuchuuRows, SUM(DO.OrderSu) AS OrderSu
                    FROM D_OrderDetails AS DO
                    WHERE DO.DeleteDateTime IS NULL
                    GROUP BY DO.JuchuuNO, DO.JuchuuRows
                    ) AS DB ON DB.JuchuuNO = DM.JuchuuNO
                    AND DB.JuchuuRows = DM.JuchuuRows
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DB.OrderSu < DM.JuchuuSuu
            AND DM.HikiateFLG <> 1
			);

        END
        
        --�񓚑҂�ON
        IF @ChkNokiKaito = 1
        BEGIN
        	--�[���񓚂���Ă��Ȃ�����������
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                WHERE DM.DeleteDateTime IS NULL
                AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                AND DM.HikiateFLG <> 1
                AND NOT EXISTS(
                                
                --�@D_ArrivalPlan�����݂��Ȃ�   
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
            
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DM.HikiateFLG <> 1
            AND EXISTS(
            	--D_ArrivalPlan�����݂��Ă�A
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
        
        --������ON
        IF @ChkMinyuka= 1 
        BEGIN
        	--�[���񓚂���Ă��邪���ׂ�����
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 
            WHERE DM.DeleteDateTime IS NULL
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DM.HikiateFLG <> 1
            AND EXISTS(
            	--�@D_ArrivalPlan�����݂���A
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
                    	AND	MM.Num2 IN (1,2,4,6)))
                    --�@��D_ArrivalPlan�ɑ΂��āA���ׂ������̂�̂�����
                    AND DA.ArrivalPlanSu<> DA.ArrivalSu
                    )
			);
        END

        --���d��ON
        IF @ChkMisiire= 1 
        BEGIN
        	--���ׂ������ǎd���������ȉ���Select�Ň@�̏ꍇ�A���o�ΏۂƂ���
        	--�@D_PurchaseDetails�����݂��Ȃ�
            UPDATE #TableForJuchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                Select DM.JuchuuNO                                                                                          
                from D_JuchuuDetails AS DM 

                WHERE DM.DeleteDateTime IS NULL
                AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                AND DM.HikiateFLG <> 1
                AND NOT EXISTS(
                                
                --�@D_ArrivalPlan�����݂��Ȃ�   
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
                
        DELETE FROM #TableForJuchuuShoukai
        WHERE DelFlg = 1
        ;
    END

    IF ISNULL(@SKUName,'') <> ''
    BEGIN

        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 1
        ;
        
        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 0
        WHERE EXISTS(
            SELECT DM.JuchuuNO                                                                                          
            FROM D_JuchuuDetails AS DM 
            WHERE DM.SKUName LIKE '%' + @SKUName + '%'
            AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
            AND DM.DeleteDateTime IS NULL
        );
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
    IF ISNULL(@KanaName,'') <> ''
    BEGIN

        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 1
        ;
        
        UPDATE #TableForJuchuuShoukai
        SET DelFlg = 0
        WHERE KanaName LIKE '%' + @KanaName + '%'
        ;
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    

    UPDATE #TableForJuchuuShoukai
    SET DelFlg = 1
    ;
    
    UPDATE #TableForJuchuuShoukai
    SET DelFlg = 0
    WHERE (ISNULL(Tel11,'') = (CASE WHEN @Tel1 <> '' THEN @Tel1 ELSE ISNULL(Tel11,'') END) 
    	OR ISNULL(Tel21,'') = (CASE WHEN @Tel1 <> '' THEN @Tel1 ELSE ISNULL(Tel21,'') END)) 
    AND (ISNULL(Tel12,'') = (CASE WHEN @Tel2 <> '' THEN @Tel2 ELSE ISNULL(Tel12,'') END) 
        OR ISNULL(Tel22,'') = (CASE WHEN @Tel2 <> '' THEN @Tel2 ELSE ISNULL(Tel22,'') END)) 
    AND (ISNULL(Tel13,'') = (CASE WHEN @Tel3 <> '' THEN @Tel3 ELSE ISNULL(Tel13,'') END) 
        OR ISNULL(Tel23,'') = (CASE WHEN @Tel3 <> '' THEN @Tel3 ELSE ISNULL(Tel23,'') END))
    AND ISNULL(OrderCD,'') = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE ISNULL(OrderCD,'') END)
    AND ISNULL(SalesDate,'') >= (CASE WHEN @SalesDateFrom <> '' THEN @SalesDateFrom ELSE ISNULL(SalesDate,'') END)
    AND ISNULL(SalesDate,'') <= (CASE WHEN @SalesDateTo <> '' THEN @SalesDateTo ELSE ISNULL(SalesDate,'') END)
    AND ISNULL(BillingCloseDate,'') >= (CASE WHEN @BillingCloseDateFrom <> '' THEN @BillingCloseDateFrom ELSE ISNULL(BillingCloseDate,'') END)
    AND ISNULL(BillingCloseDate,'') <= (CASE WHEN @BillingCloseDateTo <> '' THEN @BillingCloseDateTo ELSE ISNULL(BillingCloseDate,'') END)
    AND ISNULL(CollectClearDate,'') >= (CASE WHEN @CollectClearDateFrom <> '' THEN @CollectClearDateFrom ELSE ISNULL(CollectClearDate,'') END)
    AND ISNULL(CollectClearDate,'') <= (CASE WHEN @CollectClearDateTo <> '' THEN @CollectClearDateTo ELSE ISNULL(CollectClearDate,'') END)
    ;

    DELETE FROM #TableForJuchuuShoukai
    WHERE DelFlg = 1
    ;

    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    /*
    --ITEM������ɍ���Ȃ��f�[�^��e�[�u������폜
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuShoukai
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
                    --�f�[�^����݂̂̏ꍇ
                    UPDATE #TableForJuchuuShoukai
                    SET Check1 = 0
                    WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
                
                UPDATE #TableForJuchuuShoukai
                SET Check1 = 0
                WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE  Check1 = 1
        ;
    END;
    */
    --SKUCD������ɍ���Ȃ��f�[�^��e�[�u������폜
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuShoukai
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
                    --�f�[�^����݂̂̏ꍇ
                    UPDATE #TableForJuchuuShoukai
                    SET Check2 = 0
                    WHERE EXISTS(
                        SELECT DM.JuchuuNO                                                                                          
                        FROM D_JuchuuDetails AS DM 
                        WHERE DM.SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                        AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                        AND DM.DeleteDateTime IS NULL
                    );
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
                
                UPDATE #TableForJuchuuShoukai
                SET Check2 = 0
                WHERE EXISTS(
                    SELECT DM.JuchuuNO                                                                                          
                    FROM D_JuchuuDetails AS DM 
                    WHERE DM.SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                    AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                    AND DM.DeleteDateTime IS NULL
                );
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCD������ɍ���Ȃ��f�[�^��e�[�u������폜
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
                
        UPDATE #TableForJuchuuShoukai
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
                    --�f�[�^����݂̂̏ꍇ
                    UPDATE #TableForJuchuuShoukai
                    SET Check3 = 0
                    WHERE EXISTS(
                        SELECT DM.JuchuuNO                                                                                          
                        FROM D_JuchuuDetails AS DM 
                        WHERE DM.JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                        AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                        AND DM.DeleteDateTime IS NULL
                    );
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
                
                UPDATE #TableForJuchuuShoukai
                SET Check3 = 0
                WHERE EXISTS(
                    SELECT DM.JuchuuNO                                                                                          
                    FROM D_JuchuuDetails AS DM 
                    WHERE DM.JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                    AND DM.JuchuuNO = #TableForJuchuuShoukai.JuchuuNO
                    AND DM.DeleteDateTime IS NULL
                );
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForJuchuuShoukai
        WHERE  Check3 = 1
        ;
    END;
    
    --�yL_Log�zINSERT
    --��������f�[�^�֍X�V     
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TempoJuchuuShoukai',
        @PC,
        NULL,
        NULL;
    
    SELECT *
    FROM #TableForJuchuuShoukai AS DH
    ORDER BY DH.JuchuuNO
    ;
END


