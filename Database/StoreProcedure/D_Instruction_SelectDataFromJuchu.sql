
/****** Object:  StoredProcedure [dbo].[D_Instruction_SelectDataFromJuchu]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Instruction_SelectDataFromJuchu]
GO

/****** Object:  StoredProcedure [D_Instruction_SelectAll]    */
CREATE PROCEDURE D_Instruction_SelectDataFromJuchu(
    -- Add the parameters for the stored procedure here
    @DeliveryPlanDateFrom  varchar(10),
    @DeliveryPlanDateTo  varchar(10),
    @StoreCD  varchar(4),
    @Chk1 tinyint,
    @Chk2 tinyint,
    --@Chk3 tinyint,
    --@Chk4 tinyint,
    @Chk5 tinyint,
    @DeliveryName  varchar(80),
    @ChkHakkozumi tinyint,
    @ChkSyukkazumi tinyint,
    @ChkSyukkaFuka tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --画面項目転送表01　(出荷指示未作成分)・・・受注情報から表示
    SELECT  DJ.ExpressFLG
            ,(CASE WHEN DJ.FareLevel > SUM(DJ.JuchuuHontaiGaku) OVER(PARTITION BY DJ.CustomerCD) THEN 1 ELSE 0 END) AS UntinFlg
            ,DJ.JuchuuNO + '-' + RIGHT('00' + CONVERT(varchar,DJ.JuchuuRows),3) AS JuchuuNO
            ,NULL AS PrintDate		--出荷指示書
            ,DJ.CustomerCD
            ,DJ.CustomerName
            ,DJ.DeliveryCD
            ,DJ.DeliveryName
            ,DJ.DeliveryAddress1
            ,CONVERT(varchar,DJ.DeliveryPlanDate,111) As DeliveryPlanDate	--出荷予定日
            ,DJ.Minyukin    --
            ,DJ.HanbaiHontaiGaku
            ,NULL AS InstructionNO--出荷指示番号
            ,NULL AS ShippingDate	--出荷日
            ,DJ.CommentOutStore
            ,DJ.CommentInStore
            ,DJ.AdminNO
            ,DJ.JANCD
            ,DJ.SKUCD
            ,DJ.SKUName
            ,DJ.ColorName
            ,DJ.SizeName
            ,DJ.JuchuuSuu
            ,DJ.SoukoName
            ,DJ.ShukkaShubetsu
            ,DJ.DeliveryPlanNO--配送予定番号 AS DeliveryPlanNO	--配送予定番号
            ,DJ.DeliveryPlanRows
            ,0 AS InstructionRows	--出荷指示明細連番
            ,DJ.JuchuuNo As JuchuNo
            --※D_Juchuu②：D_Juchuu①を顧客CDでグループ化してSUM(D_Juchuu①.明細受注本体額)をSELECT
            ,SUM(DJ.JuchuuHontaiGaku) OVER(PARTITION BY DJ.CustomerCD) AS JuchuuHontaiGaku
            ,0 AS CNT

            ,1 AS KBN	--出荷指示未作成分
        FROM (SELECT DM.ExpressFLG
                    ,DM.JuchuuNO
                    ,DM.JuchuuRows
                    ,DH.CustomerName
                    ,DH.DeliveryCD
                    ,DH.DeliveryName
                    ,DH.DeliveryAddress1
                    --,(CASE WHEN DM.ShippingPlanDate IS NULL THEN NULL 
                    --       ELSE (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DM.ShippingPlanDate) < SYSDATETIME() THEN CONVERT(varchar, SYSDATETIME(),111)
                    --                  ELSE CONVERT(varchar, DM.ShippingPlanDate,111) END)  END) AS DeliveryPlanDate	--出荷予定日
                    ,(CASE WHEN DM.ShippingPlanDate IS NULL THEN CONVERT(varchar, GETDATE(), 111)
                      ELSE CONVERT(varchar, DM.ShippingPlanDate,111) END) AS DeliveryPlanDate
                    --未入金
                    ,DH.InvoiceGaku
                    ,DB1.TotalCollectAmount AS TotalCollectAmount_DB1	 --D_CollectBilling①
                    -- D_CollectPlan①.TotalCollectPlanGaku－D_CollectBilling②.TotalCollectAmount＋D_Juchuu.InvoiceGaku
                    ,DB2.TotalCollectAmount AS TotalCollectAmount_DB2
                    ,MD.DepositLaterFLG

                    ,(CASE @ChkSyukkaFuka WHEN 0 THEN 0 --Form.出荷不可分(入金不足)を含む≠ONの場合、OFF
                    	                  ELSE (CASE MD.DepositLaterFLG WHEN 0 THEN 0 ELSE 1 END) END) AS Minyukin --= 0（先入金）の場合、OFF
                    
                    ,DH.HanbaiHontaiGaku
                    ,DM.CommentOutStore
                    ,DM.CommentInStore
                    ,DM.AdminNO
                    ,DM.JANCD
                    ,DM.SKUCD
                    ,DM.SKUName
                    ,DM.ColorName
                    ,DM.SizeName
                    ,DM.JuchuuSuu
                    ,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
                       WHERE M.SoukoCD = DM.SoukoCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS SoukoName
                    ,1 As ShukkaShubetsu
                    ,DH.JuchuuDate
                    ,DH.CustomerCD
                    ,DM.JuchuuHontaiGaku
                    ,(SELECT top 1 M.CollectCD FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS CollectCD
                    ,(SELECT top 1 M.BillingCD FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS BillingCD
                    ,(SELECT top 1 M.FareLevel FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS FareLevel 
                    ,DD.DeliveryPlanNO--配送予定番号
                    ,DD.DeliveryPlanRows--配送予定番号
            FROM D_JuchuuDetails AS DM
           INNER JOIN D_Juchuu AS DH
              ON DH.JuchuuNo = DM.JuchuuNo
             AND DH.DeleteDateTime IS NULL
           INNER JOIN D_DeliveryPlanDetails AS DD
              ON DD.Number = DM.JuchuuNo
             AND DD.NumberRows = DM.JuchuuRows
           --AND DD.DeleteDateTime IS NULL
            LEFT OUTER JOIN M_ZipCode AS MZ
              ON MZ.ZipCD1 = DH.DeliveryZipCD1
             AND MZ.ZipCD2 = DH.DeliveryZipCD2
            LEFT OUTER JOIN M_DenominationKBN As MD
              ON MD.DenominationCD = DH.PaymentMethodCD
            
            --①回収予定から入金済額を集計（D_CollectBillingはCollectPlanNOでGroupby 必要)
            LEFT OUTER JOIN D_CollectPlan AS DC
              ON DC.JuchuuNO = DH.JuchuuNO
             AND DC.DeleteDateTime IS NULL
            
            --D_CollectBilling①
            LEFT OUTER JOIN (
                SELECT CollectPlanNO AS CollectPlanNO                                  
                      ,SUM(CollectAmount) AS TotalCollectAmount                                    
                 FROM D_CollectBilling                                    
                WHERE DeleteDateTime IS NULL                                  
                GROUP BY CollectPlanNO                                   
            ) AS DB1
            ON DB1.CollectPlanNO = DC.CollectPlanNO

            --※D_CollectBilling②：CollectPlanNOでグループ化（D_CollectBilling.DeleteDateTime IS NULL）してSUM(D_CollectBilling.CollectAmount)をSELECT
            LEFT OUTER JOIN (
                SELECT CollectPlanNO AS CollectPlanNO
                      ,SUM(CollectAmount) AS TotalCollectAmount
                 FROM D_CollectBilling
                WHERE DeleteDateTime IS NULL
                GROUP BY CollectPlanNO
            ) AS DB2
            ON DB2.CollectPlanNO = DC.CollectPlanNO
                
            WHERE DM.ShippingPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DM.ShippingPlanDate END)
            AND DM.ShippingPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DM.ShippingPlanDate END)
            AND DD.HikiateFLG = 1
            AND DH.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
            AND DM.DeleteDateTime IS NULL
            AND ((@Chk1 = 1 AND DM.ExpressFLG = 0)    --通常
                OR (@Chk2 = 1 AND DM.ExpressFLG = 1)    --至急CheckBox=ONの時
                )
            AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B     --完了していない引当なし
                           WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                           --AND B.DeleteDateTime IS NULL
                           AND B.HikiateFLG = 0
                           AND B.UpdateCancelKBN <> 9)
            AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                           WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO     --出荷指示未作成
                           AND DI.DeleteDateTime IS NULL)
                           
            AND EXISTS(SELECT DR.ReserveNO FROM D_Reserve AS DR
                       WHERE DR.Number = DM.JuchuuNo
                       AND DR.NumberRows = DM.JuchuuRows
                       AND DR.DeleteDateTime IS NULL)

        ) AS DJ
        
        --③入金元での未消込入金額を集計（D_CollectはCollectCustomerCDでGroup by必要）
        --※D_Collect①：CollectCustomerCDでグループ化（D_Collect.DeleteDateTime IS NULL）してSUM(D_Collect.ConfirmSource),SUM(D_Collect.CollectAmount)をSELECT
        LEFT OUTER JOIN (
            SELECT CollectCustomerCD AS CollectCustomerCD                              
                  ,SUM(ConfirmSource) AS TotalConfirmSource                                
                  ,SUM(CollectAmount) AS TotalCollectAmount                                
            FROM D_Collect                               
            WHERE DeleteDateTime IS NULL                              
            GROUP BY CollectCustomerCD                               
        ) AS DC1
        ON DC1.CollectCustomerCD = DJ.CollectCD

        --④入金元に対応する請求先への受注を取得して未消込債権を集計（Group by必要）
        --※D_CollectPlan①：CustomerCDでグループ化（D_CollectPlan.DeleteDateTime IS NULL）してSUM(D_CollectPlan.CollectPlanGaku)をSELECT
        LEFT OUTER JOIN (
            SELECT DC.CustomerCD AS CustomerCD
            	  ,SUM(DC.CollectPlanGaku) AS TotalCollectPlanGaku
            FROM D_CollectPlan AS DC
            WHERE DC.DeleteDateTime IS NULL
            GROUP BY DC.CustomerCD
        ) AS DCP1
        ON DCP1.CustomerCD = DJ.BillingCD

        --出荷不可分(入金不足)を含む　CheckBox=ONの時（＝入金不足も表示する←つまり入金の状態に関係なく表示する）
        WHERE ((@ChkSyukkaFuka = 1 AND DJ.DepositLaterFLG IN (0,1))
            OR (@ChkSyukkaFuka = 0 AND ((DJ.DepositLaterFLG = 1 AND (DJ.InvoiceGaku <= DJ.TotalCollectAmount_DB1 OR DCP1.TotalCollectPlanGaku-DJ.TotalCollectAmount_DB2+DJ.InvoiceGaku<=DC1.TotalConfirmSource-DC1.TotalCollectAmount))
                                    OR DJ.DepositLaterFLG = 0)))
    UNION ALL
    
    --画面項目転送表02　(出荷指示未作成分)・・・配送予定情報(受注以外)から表示
    SELECT  DD.ExpressFLG
    		,0 AS Untin
            ,NULL AS JuchuuNO
            ,NULL AS PrintDate		--出荷指示書
            ,NULL AS CustomerCD
            ,DD.DeliveryName AS CustomerName
            ,DD.CarrierCD AS DeliveryCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DD.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DD.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS DeliveryName
            ,DD.DeliveryAddress1
            ,(CASE WHEN DD.DecidedDeliveryDate IS NOT NULL THEN 
                    (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DD.DecidedDeliveryDate) < SYSDATETIME() THEN
                               CONVERT(varchar,SYSDATETIME(),111)
                          ELSE CONVERT(varchar,DD.DecidedDeliveryDate,111) END)
                   ELSE CONVERT(varchar,DD.DeliveryPlanDate,111) END) AS DeliveryPlanDate   --出荷予定日
            ,0 AS Minyukin    --
            ,0 AS HanbaiHontaiGaku
            ,NULL AS InstructionNO  --出荷指示番号
            ,NULL AS ShippingDate	--出荷日
            ,DD.CommentOutStore
            ,DD.CommentInStore
            ,DR.AdminNO
            ,(SELECT top 1 M.JANCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.SKUCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ColorName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 M.SizeName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SizeName
            ,DMD.MoveSu AS JuchuuSuu
            --,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
            --  WHERE M.SoukoCD = DD.DeliverySoukoCD AND M.ChangeDate <= DD.DeliveryPlanDate
            --  AND M.DeleteFlg = 0 
            --  AND M.StoreCD = @StoreCD
            --  ORDER BY M.ChangeDate desc) AS SoukoName
            ,NULL As SoukoName
            ,DD.DeliveryKBN As ShukkaShubetsu
            ,DM.DeliveryPlanNO--配送予定番号
            ,DM.DeliveryPlanRows
            ,0 AS InstructionRows	--出荷指示明細連番
            ,NULL As JuchuNo
            --※D_Juchuu②：D_Juchuu①を顧客CDでグループ化してSUM(D_Juchuu①.明細受注本体額)をSELECT
            ,0 AS JuchuuHontaiGaku
            ,0 AS CNT

            ,2 AS KBN	--配送予定情報

        FROM D_DeliveryPlan AS DD
        INNER JOIN D_DeliveryPlanDetails AS DM
        ON DM.DeliveryPlanNO = DD.DeliveryPlanNO
        --AND DM.DeleteDateTime IS NULL
        AND DM.UpdateCancelKBN <> 9
        INNER JOIN D_Reserve AS DR
        ON DR.Number = DM.Number
        AND DR.NumberRows = DM.NumberRows
        AND DR.DeleteDateTime IS NULL
        /*LEFT OUTER JIN D_Stock AS DS
        ON DS.StockNO = DR.StockNO
        AND DS.DeleteDateTime IS NULL*/
        INNER JOIN D_MoveDetails AS DMD
        ON DMD.MoveNO = DR.Number
        AND DMD.MoveRows = DR.NumberRows
        AND DMD.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN M_ZipCode MZ
        ON MZ.ZipCD1 = DD.DeliveryZip1CD
        AND MZ.ZipCD2 = DD.DeliveryZip2CD

        WHERE DD.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DD.DeliveryPlanDate END)
        AND DD.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DD.DeliveryPlanDate END)
        AND DD.HikiateFLG = 1
        AND DD.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        --AND DD.DeleteDateTime IS NULL
        
        AND ((@Chk1 = 1 AND DD.ExpressFLG <> 1 AND DD.DeliveryKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DD.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk5 = 1 AND DD.DeliveryKBN = 2)
            )
        --AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
        --           WHERE M.SoukoCD = DD.DeliverySoukoCD
        --           AND M.DeleteFlg = 0
        --           AND M.StoreCD = @StoreCD
        --    )
        --※完了していない引当なし
        AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B
                       WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                       --AND B.DeleteDateTime IS NULL
                       AND B.HikiateFLG = 0
                       AND B.UpdateCancelKBN <> 9)
        --※出荷指示未作成
        AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                       WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO
                       AND DI.DeleteDateTime IS NULL)
	UNION ALL
	
    --画面項目転送表03　(出荷指示作成済分)・・・出荷指示データから表示
    SELECT  DI.ExpressFLG
            --今回抽出対象の出荷指示明細(出荷指示番号単位で)に運賃商品が含まれている場合、
            ,(CASE WHEN MM.Num1 = DIM.AdminNO THEN 1 ELSE 0 END) AS Untin
            ,DR.Number + '-' + RIGHT('00' + CONVERT(varchar,DR.NumberRows),3) AS JuchuuNO
            ,CONVERT(varchar,DI.PrintDate,111) AS PrintDate		--出荷指示書
            ,NULL AS CustomerCD
            ,DI.DeliveryName AS CustomerName
            ,DI.CarrierCD AS DeliveryCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DI.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DI.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS DeliveryName
            ,DI.DeliveryAddress1
            ,CONVERT(varchar,DI.DeliveryPlanDate,111) AS DeliveryPlanDate   --出荷予定日
            ,0 AS Minyukin    --
            ,NULL AS HanbaiHontaiGaku
            ,DI.InstructionNO		--出荷指示番号
            ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate	--出荷日
            ,DI.CommentOutStore
            ,DI.CommentInStore
            ,DR.AdminNO
            ,(SELECT top 1 M.JANCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.SKUCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ColorName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 M.SizeName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SizeName
            ,DIM.InstructionSu AS JuchuuSuu
            --,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
            --  WHERE M.SoukoCD = DI.DeliverySoukoCD 
            --  AND M.ChangeDate <= DI.DeliveryPlanDate
            --  AND M.DeleteFlg = 0
            --  AND M.StoreCD = @StoreCD
            --  ORDER BY M.ChangeDate desc) AS SoukoName
            ,NULL AS SoukoName
            ,DI.InstructionKBN As ShukkaShubetsu
            ,DI.DeliveryPlanNO	--配送予定番号
            ,DM.DeliveryPlanRows
            ,DIM.InstructionRows	--出荷指示明細連番
            ,NULL As JuchuNo
            --※D_Juchuu②：D_Juchuu①を顧客CDでグループ化してSUM(D_Juchuu①.明細受注本体額)をSELECT
            ,0 AS JuchuuHontaiGaku
            ,(SELECT COUNT(*)
                FROM D_InstructionDetails AS DISM
                INNER JOIN D_Instruction DISH
                ON DISH.InstructionNO = DISM.InstructionNO
                LEFT OUTER JOIN D_ShippingDetails AS DSPM
                ON DSPM.InstructionNO = DISM.InstructionNO
                AND DSPM.InstructionRows = DISM.InstructionRows
                WHERE DSPM.InstructionNO IS NULL
                AND DISM.InstructionNO <> DI.InstructionNO
                AND DISM.InstructionRows <> DIM.InstructionRows
                AND DISH.DeliveryName = DI.DeliveryName
             ) AS CNT
            ,3 AS KBN	--配送予定情報

        FROM D_Instruction AS DI
        INNER JOIN D_InstructionDetails AS DIM
        ON DIM.InstructionNO = DI.InstructionNO
        AND DIM.DeleteDateTime IS NULL
        INNER JOIN D_Reserve AS DR
        ON DR.ReserveNO = DIM.ReserveNO
        AND DR.DeleteDateTime IS NULL
        INNER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL
        INNER JOIN D_DeliveryPlanDetails AS DM
        ON DR.Number = DM.Number
        AND DR.NumberRows = DM.NumberRows
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.InstructionNO = DI.InstructionNO
        AND DS.DeleteDateTime IS NULL
        LEFT OUTER JOIN M_MultiPorpose AS MM
        ON MM.ID = 227
        AND MM.[KEY] = 1
        
        WHERE DI.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DI.DeliveryPlanDate END)
        AND DI.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DI.DeliveryPlanDate END)

        AND DI.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND DI.DeleteDateTime IS NULL
        AND DIM.AdminNO <> MM.Num1	--運賃明細は表示対象外

        AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
            )
        --AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
        --        WHERE M.SoukoCD = DI.DeliverySoukoCD
        --        AND M.DeleteFlg = 0
        --        AND M.StoreCD = @StoreCD
        --    )
        AND (@ChkHakkozumi = 1 OR (@ChkHakkozumi = 0 AND DI.PrintDate IS NULL))
        AND (@ChkSyukkazumi = 1 OR (@ChkSyukkazumi = 0 AND NOT EXISTS(SELECT 1 FROM D_Shipping AS A
                                                                      WHERE A.InstructionNO = DI.InstructionNO
                                                                      AND A.DeleteDateTime IS NULL)))        
    --※画面項目転送表01のデータも含めての並び順
    --出荷予定日,出荷先名(顧客名),受注番号-明細連番,出荷指示番号
    ORDER BY DeliveryPlanDate, DeliveryName, JuchuuNO, InstructionNO	
    ;

END

GO
