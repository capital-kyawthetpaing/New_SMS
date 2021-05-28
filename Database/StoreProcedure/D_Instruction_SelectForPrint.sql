
/****** Object:  StoredProcedure [dbo].[D_Instruction_SelectForPrint]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Instruction_SelectForPrint]
GO

/****** Object:  StoredProcedure [D_Instruction_SelectForPrint]    */
CREATE PROCEDURE D_Instruction_SelectForPrint(
    -- Add the parameters for the stored procedure here
    @DeliveryPlanDate  varchar(10),
    @SoukoCD  varchar(6),
    @Chk1 tinyint,
    @Chk2 tinyint,
    @Chk3 tinyint,
    @Chk4 tinyint,
    @Chk5 tinyint,
    @CarrierCD  varchar(3),
    @InstructionNO varchar(11),
    @ChkMihakko tinyint,
    @ChkSaihakko tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  1 AS KBN
    		,'出荷指示書' As Title
            ,(CASE WHEN @ChkSaihakko = 1 THEN '※再発行※' ELSE '' END) AS Saihakkou  --再発行.CheckBox＝ONの時 "※再発行※"
            ,'着指定日時' AS Label1
            ,(CASE MAX(DJ.NouhinsyoFLG) WHEN 1 THEN (CASE MAX(DJ.SeikyuusyoFLG) WHEN 1 THEN '納品書・請求書同封'
                            WHEN 0 THEN '納品書同封' END)
                WHEN 0 THEN (CASE MAX(DJ.SeikyuusyoFLG) WHEN 1 THEN '請求書同封'
                            WHEN 0 THEN '' END)
                ELSE '' END) AS Nouhinsyo_SeikyuusyoFLG
            ,'配送会社' AS Label2
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A 
                  WHERE A.CarrierCD = MAX(DI.CarrierCD) 
                  AND A.DeleteFlg = 0 
                  AND A.ChangeDate <= MAX(DI.DeliveryPlanDate)
                  ORDER BY A.ChangeDate desc) AS CarrierName 

            ,(CASE MAX(DI.OntheDayFLG) WHEN 1 THEN '即日出荷' else '' END) AS OntheDayFLG
            ,(CASE MAX(DD.IncludeFLG) WHEN 1 THEN '同梱あり' else '' END) AS IncludeFLG
            ,'お届け先' AS Label3
            
    	    ,MAX(CONVERT(varchar,DI.InstructionDate,111)) AS InstructionDate
    	    ,MAX(ISNULL(CONVERT(varchar,DI.DecidedDeliveryDate,111),'') + ' ' + ISNULL(DI.DecidedDeliveryTime,'')) AS DecidedDeliveryDate      
            ,DI.InstructionNO                                               
            ,DI.DeliveryPlanNO             
            ,MAX(DI.DeliveryName) AS DeliveryName                                                
            ,MAX(DI.CommentInStore) AS CommentInStore
            ,MAX(DI.CommentOutStore) AS CommentOutStore

            --【Details】
            ,dbo.Fnc_SetCheckdigit(DR.Number) AS JANCD
            ,'納品書(' + dbo.Fnc_SetCheckdigit(DR.Number) + ')' AS SKUName
            ,NULL AS Number		--JuchuuNO,IdoNO
            ,0 AS NumberRows
            ,NULL AS RackNO
            ,NULL AS SKUCD
            ,NULL AS ColorSizeName
            ,NULL AS BrandName
            ,NULL AS TagName
            ,NULL AS D_CommentInStore
            ,NULL AS InstructionSu
            ,''   AS UpdateCancelKBN
            ,NULL AS TaniName
            ,DR.Number AS JuchuNO
        FROM D_Instruction AS DI
        INNER JOIN D_InstructionDetails AS DM 
        ON DM.InstructionNO = DI.InstructionNO 
        AND DM.DeleteDateTime IS NULL

        INNER JOIN D_Reserve AS DR
        ON DR.ReserveNO = DM.ReserveNO
        AND DR.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL
        
        INNER JOIN D_JuchuuStatus AS DJ
        ON DJ.JuchuuNO = DR.Number

        WHERE ((@ChkMihakko = 1 
            AND DI.DeliveryPlanDate = (CASE WHEN @DeliveryPlanDate <> '' THEN CONVERT(DATE, @DeliveryPlanDate) ELSE DI.DeliveryPlanDate END)
            AND DI.PrintDate IS NULL
            AND DI.FromSoukoCD = (CASE WHEN @SoukoCD <> '' THEN @SoukoCD ELSE DI.FromSoukoCD END)
            AND DI.CarrierCD = (CASE WHEN @CarrierCD <> '' THEN @CarrierCD ELSE DI.CarrierCD END)            
            AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.DecidedDeliveryDate IS NULL AND DI.OntheDayFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
                OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
                OR (@Chk3 = 1 AND DI.DecidedDeliveryDate IS NOT NULL)
                OR (@Chk4 = 1 AND DI.OntheDayFLG = 1)
                OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
                )
        ) 
        OR @ChkMihakko = 0)
        
        AND ((@ChkSaihakko = 1
        	AND DI.InstructionNO = (CASE WHEN @InstructionNO <> '' THEN @InstructionNO ELSE DI.InstructionNO END)
        )
        OR @ChkSaihakko = 0)
        
        AND DI.DeleteDateTime IS NULL
        AND DI.InstructionKBN = 1
        AND DJ.NouhinsyoFLG = 1		--★
		GROUP BY DI.InstructionNO, DR.Number,DI.DeliveryPlanNO,DI.DeliveryPlanDate

    UNION ALL
    
    SELECT  1 AS KBN
    		,'出荷指示書' As Title
            ,(CASE WHEN @ChkSaihakko = 1 THEN '※再発行※' ELSE '' END) AS Saihakkou  --再発行.CheckBox＝ONの時 "※再発行※"
            ,'着指定日時' AS Label1
            ,(CASE DJ.NouhinsyoFLG WHEN 1 THEN (CASE DJ.SeikyuusyoFLG WHEN 1 THEN '納品書・請求書同封'
                            WHEN 0 THEN '納品書同封' END)
                WHEN 0 THEN (CASE DJ.SeikyuusyoFLG WHEN 1 THEN '請求書同封'
                            WHEN 0 THEN '' END)
                ELSE '' END) AS Nouhinsyo_SeikyuusyoFLG
            ,'配送会社' AS Label2
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A 
                  WHERE A.CarrierCD = DI.CarrierCD 
                  AND A.DeleteFlg = 0 
                  AND A.ChangeDate <= DI.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS CarrierName 

            ,(CASE DI.OntheDayFLG WHEN 1 THEN '即日出荷' else '' END) AS OntheDayFLG
            ,(CASE DD.IncludeFLG WHEN 1 THEN '同梱あり' else '' END) AS IncludeFLG
            ,'お届け先' AS Label3
            
    	    ,CONVERT(varchar,DI.InstructionDate,111) AS InstructionDate
    	    ,ISNULL(CONVERT(varchar,DI.DecidedDeliveryDate,111),'') + ' ' + ISNULL(DI.DecidedDeliveryTime,'') AS DecidedDeliveryDate      
            ,DI.InstructionNO                                               
            ,DI.DeliveryPlanNO             
            ,DI.DeliveryName                                                
            ,DI.CommentInStore
            ,DI.CommentOutStore

            --【Details】
            ,DM.JANCD
            ,(SELECT top 1 M.SKUName
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS SKUName
            ,DR.Number		--JuchuuNO,IdoNO
            ,DR.NumberRows
            ,DS.RackNO
            ,DM.SKUCD
            ,(SELECT top 1 ISNULL(M.ColorName,'') + ' '+ ISNULL(M.SizeName,'') 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS ColorSizeName
            ,(SELECT top 1 MB.BrandName
                FROM M_SKU AS M 
                INNER JOIN M_Brand AS MB
                ON MB.BrandCD = M.BrandCD
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS BrandName
            ,(SELECT top 1 M.TagName
                FROM M_SKUTag AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.SEQ = 0
                 ORDER BY M.ChangeDate desc) AS TagName
            ,DM.CommentInStore AS D_CommentInStore
            ,DM.InstructionSu
            ,(CASE WHEN DJM.UpdateCancelKBN = 9 THEN 'キ' ELSE '' END) AS UpdateCancelKBN
            ,(SELECT top 1 MM.Char1 
                FROM M_SKU AS M 
                INNER JOIN M_MultiPorpose AS MM ON MM.ID = 201 AND MM.[Key] = M.TaniCD
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS TaniName
            ,DR.Number AS JuchuNO
             
        FROM D_Instruction AS DI
        INNER JOIN D_InstructionDetails AS DM 
        ON DM.InstructionNO = DI.InstructionNO 
        AND DM.DeleteDateTime IS NULL

        INNER JOIN D_Reserve AS DR
        ON DR.ReserveNO = DM.ReserveNO
        AND DR.DeleteDateTime IS NULL

        LEFT OUTER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_JuchuuStatus AS DJ
        ON DJ.JuchuuNO = DR.Number
        
        LEFT OUTER JOIN D_Stock AS DS
        ON DS.StockNO = DR.StockNO
        AND DS.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DR.Number
        AND DJM.JuchuuRows = DR.NumberRows
        AND DJM.DeleteDateTime IS NULL
        
        WHERE ((@ChkMihakko = 1 
            AND DI.DeliveryPlanDate = (CASE WHEN @DeliveryPlanDate <> '' THEN CONVERT(DATE, @DeliveryPlanDate) ELSE DI.DeliveryPlanDate END)
            AND DI.PrintDate IS NULL
            AND DI.FromSoukoCD = (CASE WHEN @SoukoCD <> '' THEN @SoukoCD ELSE DI.FromSoukoCD END)
            AND DI.CarrierCD = (CASE WHEN @CarrierCD <> '' THEN @CarrierCD ELSE DI.CarrierCD END)            
            AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.DecidedDeliveryDate IS NULL AND DI.OntheDayFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
                OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
                OR (@Chk3 = 1 AND DI.DecidedDeliveryDate IS NOT NULL)
                OR (@Chk4 = 1 AND DI.OntheDayFLG = 1)
                OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
                )
        ) 
        OR @ChkMihakko = 0)
        
        AND ((@ChkSaihakko = 1
        	AND DI.InstructionNO = (CASE WHEN @InstructionNO <> '' THEN @InstructionNO ELSE DI.InstructionNO END)
        )
        OR @ChkSaihakko = 0)
        
        AND DI.DeleteDateTime IS NULL
        AND DI.InstructionKBN = 1

    UNION ALL
    SELECT  2 AS KBN
    		, '移動指示書' As Title
            ,(CASE WHEN @ChkSaihakko = 1 THEN '※再発行※' ELSE '' END) AS Saihakkou  --再発行.CheckBox＝ONの時 "※再発行※"
            ,'' AS Label1
            ,'' AS Nouhinsyo_SeikyuusyoFLG
            ,'' AS Label2
            ,'' AS CarrierName 

            ,'' AS OntheDayFLG
            ,'' AS IncludeFLG
            ,'出荷先' AS Label3
            
    	    ,CONVERT(varchar,DI.InstructionDate,111) AS InstructionDate
    	    ,'' AS DecidedDeliveryDate      
            ,DI.InstructionNO                                               
            ,DI.DeliveryPlanNO             
            --,DI.DeliveryName
            ,(SELECT top 1 M.SoukoName
                FROM M_Souko AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.SoukoCD = DI.DeliverySoukoCD
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS DeliveryName                                                
            ,DI.CommentInStore
            ,DI.CommentOutStore

            --【Details】
            ,DM.JANCD
            ,(SELECT top 1 M.SKUName
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS SKUName
            ,DR.Number		--JuchuuNO,IdoNO
            ,DR.NumberRows
            ,DS.RackNO
            ,DM.SKUCD
            ,(SELECT top 1 ISNULL(M.ColorName,'') + ' '+ ISNULL(M.SizeName,'') 
                FROM M_SKU AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS ColorSizeName
            ,(SELECT top 1 MB.BrandName
                FROM M_SKU AS M 
                INNER JOIN M_Brand AS MB
                ON MB.BrandCD = M.BrandCD
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS BrandName
            ,(SELECT top 1 M.TagName
                FROM M_SKUTag AS M 
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.SEQ = 0
                 ORDER BY M.ChangeDate desc) AS TagName
            ,DM.CommentInStore AS D_CommentInStore
            ,DM.InstructionSu
            ,'' AS UpdateCancelKBN
            ,(SELECT top 1 MM.Char1 
                FROM M_SKU AS M 
                INNER JOIN M_MultiPorpose AS MM ON MM.ID = 201 AND MM.[Key] = M.TaniCD
                WHERE M.ChangeDate <= DI.DeliveryPlanDate
                 AND M.AdminNO = DM.AdminNO
                  AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS TaniName
            ,DR.Number AS JuchuNO
             
        FROM D_Instruction AS DI
        INNER JOIN D_InstructionDetails AS DM 
        ON DM.InstructionNO = DI.InstructionNO 
        AND DM.DeleteDateTime IS NULL

        INNER JOIN D_Reserve AS DR
        ON DR.ReserveNO = DM.ReserveNO
        AND DR.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_JuchuuStatus AS DJ
        ON DJ.JuchuuNO = DR.Number
        
        LEFT OUTER JOIN D_Stock AS DS
        ON DS.StockNO = DR.StockNO
        AND DS.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN D_JuchuuDetails AS DJM
        ON DJM.JuchuuNO = DR.Number
        AND DJM.JuchuuRows = DR.NumberRows
        AND DJM.DeleteDateTime IS NULL
        
        WHERE ((@ChkMihakko = 1 
            AND DI.DeliveryPlanDate = (CASE WHEN @DeliveryPlanDate <> '' THEN CONVERT(DATE, @DeliveryPlanDate) ELSE DI.DeliveryPlanDate END)
            AND DI.PrintDate IS NULL
            AND DI.FromSoukoCD = (CASE WHEN @SoukoCD <> '' THEN @SoukoCD ELSE DI.FromSoukoCD END)
            AND DI.CarrierCD = (CASE WHEN @CarrierCD <> '' THEN @CarrierCD ELSE DI.CarrierCD END)
        ) 
        OR @ChkMihakko = 0)
        
        AND ((@ChkSaihakko = 1
        	AND DI.InstructionNO = (CASE WHEN @InstructionNO <> '' THEN @InstructionNO ELSE DI.InstructionNO END)
        )
        OR @ChkSaihakko = 0)
        
        AND DI.DeleteDateTime IS NULL
        AND DI.InstructionKBN = 2	--★            
    
    ORDER BY InstructionNO, JuchuNO, NumberRows

    ;

END

GO
