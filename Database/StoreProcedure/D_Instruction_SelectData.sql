 BEGIN TRY 
 Drop Procedure dbo.[D_Instruction_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Instruction_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Instruction_SelectData](
    -- Add the parameters for the stored procedure here
    @DeliveryPlanDateFrom  varchar(10),
    @DeliveryPlanDateTo  varchar(10),
    @StoreCD  varchar(4),
    @Chk1 tinyint,
    @Chk2 tinyint,
    @Chk3 tinyint,
    @Chk4 tinyint,
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
    --画面項目転送表01　(出荷指示未作成分)・・・配送予定情報から表示
    SELECT  (CASE WHEN DD.DecidedDeliveryDate IS NOT NULL THEN 
                      (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DD.DecidedDeliveryDate) < SYSDATETIME() THEN
                                 CONVERT(varchar,SYSDATETIME(),111)
                            ELSE CONVERT(varchar,DD.DecidedDeliveryDate,111) END)
             ELSE CONVERT(varchar,DD.DeliveryPlanDate,111) END) AS DeliveryPlanDate   --出荷予定日
            ,NULL AS InstructionNO	--出荷指示番号
            ,DD.Number
            ,(CASE WHEN DD.DeliveryKBN = 2 THEN 
                 (SELECT M.SoukoName FROM F_Souko(DD.DeliveryPlanDate) AS M
                   WHERE M.SoukoCD = DD.DeliverySoukoCD
                     AND M.DeleteFlg = 0
                     --AND M.StoreCD = @StoreCD
                 )
            ELSE DD.DeliveryName END) AS DeliveryName
            
            ,DD.DeliveryAddress1
            ,DD.CarrierCD
            ,(SELECT top 1 A.CarrierName
              FROM M_Carrier AS A
              WHERE A.CarrierCD = DD.CarrierCD
              AND A.DeleteFlg = 0
              AND A.ChangeDate <= DD.DeliveryPlanDate
              ORDER BY A.ChangeDate desc) AS CarrierName
            ,NULL AS PrintDate		--出荷指示書
            ,NULL AS ShippingDate	--出荷
            ,DD.CommentOutStore
            ,DD.CommentInStore
            ,DD.DeliveryPlanNO
            ,DD.DeliveryKBN As InstructionKBN
            ,DD.OntheDayFLG
            ,DD.ExpressFLG
            ,CONVERT(varchar,DD.DecidedDeliveryDate,111) + ' ' + ISNULL(DD.DecidedDeliveryTime,'') AS DecidedDeliveryDate	--着指定日時
            ,1 AS KBN	--出荷指示未作成分
            
            ,DJ.CustomerCD
            ,(SELECT top 1 M.CollectCD FROM M_Customer AS M
               WHERE M.CustomerCD = DJ.CustomerCD
                 AND M.ChangeDate <= DJ.JuchuuDate
                 AND M.DeleteFlg = 0
               ORDER BY M.ChangeDate desc) AS CollectCD
            ,DJ.JuchuuDate
            ,MD.DepositLaterFLG
            ,W.CollectAmount  --①
            ,W.InvoiceGaku    --②
            ,(CASE WHEN @ChkSyukkaFuka = 1 THEN 1 ELSE (CASE WHEN DD.DeliveryKBN <> 1 THEN 1 ELSE 0 END) END) AS OkFlg
            
            --test用
            /*
            ,W.CollectAmount  --①
            ,W.InvoiceGaku    --②
            ,X.CollectAmount  --③
            ,Y.CollectPlanGaku --④
            ,Y.CollectAmount
            */
        INTO #TableForInstruction
        
        FROM D_DeliveryPlan AS DD
        LEFT OUTER JOIN D_Juchuu AS DJ
          ON DJ.JuchuuNO = DD.Number
         AND DJ.DeleteDateTime IS NULL

        LEFT OUTER JOIN D_Move AS DM
          ON DM.MoveNO = DD.Number
         AND DM.DeleteDateTime IS NULL
         
        LEFT OUTER JOIN M_ZipCode MZ
          ON MZ.ZipCD1 = DD.DeliveryZip1CD
         AND MZ.ZipCD2 = DD.DeliveryZip2CD
         
        LEFT OUTER JOIN M_DenominationKBN AS MD
          ON MD.DenominationCD = DD.PaymentMethodCD
          
        LEFT OUTER JOIN (SELECT DC.JuchuuNO
                               ,SUM(DCB.CollectAmount) AS CollectAmount  --①
                               ,MAX(DJ.InvoiceGaku) AS InvoiceGaku       --②
             FROM D_CollectPlan AS DC
             LEFT OUTER JOIN D_CollectBilling AS DCB
               ON DCB.CollectPlanNO = DC.CollectPlanNO
              AND DCB.DeleteDateTime IS NULL
             LEFT OUTER JOIN D_Juchuu AS DJ
               ON DJ.JuchuuNO = DC.JuchuuNO
              AND DJ.DeleteDateTime IS NULL
             
            GROUP BY DC.JuchuuNO
        )AS W
        ON W.JuchuuNO = DD.Number
/*
        LEFT OUTER JOIN (SELECT MAIN.CustomerCD
                               ,MAIN.JuchuuDate
                               ,SUM(DC.ConfirmSource-DC.CollectAmount) AS CollectAmount  --③
                           FROM(
                                  SELECT DJ2.CustomerCD
                                        ,DJ2.JuchuuDate
                                        ,(SELECT top 1 M.CollectCD FROM M_Customer AS M
                                           WHERE M.CustomerCD = DJ2.CustomerCD
                                             AND M.ChangeDate <= DJ2.JuchuuDate
                                             AND M.DeleteFlg = 0
                                           ORDER BY M.ChangeDate desc) AS CollectCD
                                           
                                    FROM D_Juchuu AS DJ2
                                   WHERE DJ2.DeleteDateTime IS NULL
                                   GROUP BY DJ2.CustomerCD, DJ2.JuchuuDate
                          )MAIN
                        INNER JOIN D_Collect AS DC
                           ON DC.CollectCustomerCD = MAIN.CollectCD
                          AND DC.DeleteDateTime IS NULL
                        GROUP BY MAIN.CustomerCD
        ) AS X
        ON X.CustomerCD = DJ.CustomerCD
        AND X.JuchuuDate = DJ.JuchuuDate
        
        LEFT OUTER JOIN (SELECT DP.CustomerCD
                               ,SUM(DP.CollectPlanGaku) AS CollectPlanGaku
                               ,SUM(DCB.CollectAmount)  AS CollectAmount  --④
                           
                           FROM D_CollectPlan AS DP
                       --      ON DP.CustomerCD = MC2.BillingCD
                           LEFT OUTER JOIN D_CollectBilling AS DCB
                             ON DCB.CollectPlanNO = DP.CollectPlanNO
                            AND DCB.DeleteDateTime IS NULL
                            
                           LEFT OUTER JOIN (
                                  SELECT DJ2.JuchuuNO
                                        ,DJ2.JuchuuDate
                                        ,(SELECT top 1 M.CollectCD FROM M_Customer AS M
                                           WHERE M.CustomerCD = DJ2.CustomerCD
                                             AND M.ChangeDate <= DJ2.JuchuuDate
                                             AND M.DeleteFlg = 0
                                           ORDER BY M.ChangeDate desc) AS CollectCD
                                    FROM D_Juchuu AS DJ2
                                   WHERE DJ2.DeleteDateTime IS NULL
                          )MAIN

                         WHERE DP.DeleteDateTime IS NULL
                           AND EXISTS(SELECT 1 FROM M_Customer AS MC2
                                       WHERE MC2.CollectCD = MAIN.CollectCD
                                         AND MC2.ChangeDate <= MAIN.JuchuuDate
                                         AND MC2.DeleteFlg = 0)
                         LEFT OUTER JOIN
            
            GROUP BY DP.CustomerCD
        )AS Y
        ON Y.CustomerCD = DJ.CustomerCD
        */
        WHERE ISNULL(DD.DeliveryPlanDate,'') >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE ISNULL(DD.DeliveryPlanDate,'') END)
        AND ISNULL(DD.DeliveryPlanDate,'') <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE ISNULL(DD.DeliveryPlanDate,'') END)
        AND DD.HikiateFLG = 1
        AND ISNULL(DD.DeliveryName,'') LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND ((DD.DeliveryKBN = 1 AND DJ.StoreCD = @StoreCD) OR (DD.DeliveryKBN = 2 AND DM.StoreCD = @StoreCD))
        
        AND ((@Chk1 = 1 AND DD.ExpressFLG <> 1 AND DD.DecidedDeliveryDate IS NULL AND DD.OntheDayFLG <> 1 AND DD.DeliveryKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DD.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk3 = 1 AND DD.DecidedDeliveryDate IS NOT NULL)
            OR (@Chk4 = 1 AND DD.OntheDayFLG = 1)
            OR (@Chk5 = 1 AND DD.DeliveryKBN = 2)
            )
        AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B
                    WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                    --AND B.DeleteDateTime IS NULL
                    AND B.HikiateFLG = 0
                    AND B.UpdateCancelKBN <> 9)
        AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                    WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO
                    AND DI.DeleteDateTime IS NULL)
        
        AND (DD.DeliveryKBN = 1
        --出荷不可分(入金不足)を含む　CheckBox=ONの時（＝入金不足も表示する←つまり入金の状態に関係なく表示する）
        AND ((@ChkSyukkaFuka = 1 AND MD.DepositLaterFLG IN (1,0))
        --出荷不可分(入金不足)を含む　CheckBox=OFFの時（＝入金不足は表示しない←入金済か、未消込が十分ある）
          OR (@ChkSyukkaFuka = 0 
--              AND (MD.DepositLaterFLG = 1
--	              AND (W.InvoiceGaku <= ISNULL(W.CollectAmount,0)   --受注額≦入金額　←入金があった
--	                OR (ISNULL(Y.CollectPlanGaku,0) - ISNULL(Y.CollectAmount,0) + DJ.InvoiceGaku <= ISNULL(X.CollectAmount,0)		--★途中
--              )))
--              OR MD.DepositLaterFLG = 0
          ))
          OR DD.DeliveryKBN <> 1)

    UNION ALL
    
    --画面項目転送表02　(出荷指示作成済分)・・・出荷指示データから表示
    SELECT  CONVERT(varchar,DI.DeliveryPlanDate,111) AS DeliveryPlanDate
            ,DI.InstructionNO
            ,DD.Number
            ,(CASE WHEN DI.InstructionKBN = 2 THEN 
                 (SELECT M.SoukoName FROM F_Souko(DI.InstructionDate) AS M
                   WHERE M.SoukoCD = DI.DeliverySoukoCD
                     AND M.DeleteFlg = 0
                 )
             ELSE DI.DeliveryName END) AS DeliveryName
            ,DI.DeliveryAddress1
            ,DI.CarrierCD
            ,(SELECT top 1 A.CarrierName
              FROM M_Carrier AS A
              WHERE A.CarrierCD = DI.CarrierCD
              AND A.DeleteFlg = 0
              AND A.ChangeDate <= DI.InstructionDate
              ORDER BY A.ChangeDate desc) AS CarrierName
            ,CONVERT(varchar,DI.PrintDate,111) AS PrintDate
            ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
            ,DI.CommentOutStore
            ,DI.CommentInStore
            ,DI.DeliveryPlanNO
            ,DI.InstructionKBN
            ,DI.OntheDayFLG
            ,DI.ExpressFLG
            ,CONVERT(varchar,DI.DecidedDeliveryDate,111) + ' ' + ISNULL(DI.DecidedDeliveryTime,'') AS DecidedDeliveryDate
            ,(CASE WHEN DS.InstructionNO IS NULL THEN 1 ELSE 2 END) AS KBN	--出荷指示作成済分
            
            ,NULL AS CustomerCD
            ,NULL AS CollectCD
            ,NULL AS JuchuuDate
            ,NULL AS DepositLaterFLG
            ,NULL AS CollectAmount  --①
            ,NULL AS InvoiceGaku    --②
            ,1 AS OkFlg
            
        FROM D_Instruction AS DI
        LEFT OUTER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.InstructionNO = DI.InstructionNO
        AND DS.DeleteDateTime IS NULL
        
        WHERE ISNULL(DI.DeliveryPlanDate,'') >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE ISNULL(DI.DeliveryPlanDate,'') END)
        AND ISNULL(DI.DeliveryPlanDate,'') <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE ISNULL(DI.DeliveryPlanDate,'') END)

        AND ISNULL(DI.DeliveryName,'') LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND DI.DeleteDateTime IS NULL
        AND EXISTS(SELECT M.SoukoName FROM F_Souko(DI.InstructionDate) AS M
                   WHERE M.SoukoCD = DI.FromSoukoCD
                     AND M.DeleteFlg = 0
                     AND M.StoreCD = @StoreCD)
        AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.DecidedDeliveryDate IS NULL AND DI.OntheDayFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk3 = 1 AND DI.DecidedDeliveryDate IS NOT NULL)
            OR (@Chk4 = 1 AND DI.OntheDayFLG = 1)
            OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
            )
        AND (@ChkHakkozumi = 1 OR (@ChkHakkozumi = 0 AND DI.PrintDate IS NULL))
        AND (@ChkSyukkazumi = 1 OR (@ChkSyukkazumi = 0 AND NOT EXISTS(SELECT 1 FROM D_Shipping AS A
                                            WHERE A.InstructionNO = DI.InstructionNO
                                            AND A.DeleteDateTime IS NULL)))        
    --※画面項目転送表01のデータも含めての並び順
    ORDER BY DeliveryPlanDate, DeliveryPlanNO	
    ;

    --出荷不可分(入金不足)を含む　CheckBox=OFFの時（＝入金不足は表示しない←入金済か、未消込が十分ある）
    IF @ChkSyukkaFuka = 0 
    BEGIN
        UPDATE #TableForInstruction SET
              OkFlg = 1
        WHERE KBN = 1
          AND OkFlg = 0
          AND ((DepositLaterFLG = 1 AND InvoiceGaku <= CollectAmount)   --受注額≦入金額　←入金があった
              OR DepositLaterFLG = 0)
        ;
        
        DECLARE CUR_CHK CURSOR FOR
         SELECT T.CustomerCD, T.CollectCD, T.JuchuuDate, T.InvoiceGaku
           FROM #TableForInstruction AS T
          WHERE T.KBN = 1
            AND T.OkFlg = 0
            AND T.DepositLaterFLG = 1
            ;
          
        DECLARE @ChkCustomerCD varchar(13);
        DECLARE @ChkCollectCD varchar(13);
        DECLARE @InvoiceGaku money;
        DECLARE @CNT int;
        DECLARE @JuchuuDate date;

        --カーソルオープン
        OPEN CUR_CHK;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_CHK
        INTO  @ChkCustomerCD,@ChkCollectCD,@JuchuuDate,@InvoiceGaku;
         
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            SET @CNT = (SELECT COUNT(*)
                          FROM F_Customer(@JuchuuDate) AS MC2

                          LEFT OUTER JOIN (SELECT DC.CollectCustomerCD
                                                 ,SUM(DC.ConfirmSource-DC.CollectAmount) AS CollectAmount
                                             FROM D_Collect AS DC
                                            WHERE DC.DeleteDateTime IS NULL
                                            GROUP BY DC.CollectCustomerCD
                                          ) AS X
                            ON X.CollectCustomerCD = MC2.CollectCD
                            
                          LEFT OUTER JOIN(SeLECT DP.CustomerCD
                                                ,SUM(DP.CollectPlanGaku) AS CollectPlanGaku
                                                ,SUM(DCB.CollectAmount)  AS CollectAmount  --④
                                            FROM D_CollectPlan AS DP
                                            LEFT OUTER JOIN D_CollectBilling AS DCB
                                              ON DCB.CollectPlanNO = DP.CollectPlanNO
                                             AND DCB.DeleteDateTime IS NULL  
                                           WHERE DP.DeleteDateTime IS NULL
                                           GROUP BY DP.CustomerCD         
                                         )AS Y
                            ON Y.CustomerCD = MC2.BillingCD
                         WHERE MC2.CollectCD = @ChkCollectCD
                           AND MC2.DeleteFlg = 0
                           AND ISNULL(Y.CollectPlanGaku,0) - ISNULL(Y.CollectAmount,0) + @InvoiceGaku <= ISNULL(X.CollectAmount,0)
                         );
                
            IF @CNT > 0
            BEGIN
                UPDATE #TableForInstruction SET
                    OkFlg = 1
                WHERE KBN = 1
                  AND OkFlg = 0
                  AND CustomerCD = @ChkCustomerCD
                  AND CollectCD = @ChkCollectCD
                  AND JuchuuDate = @JuchuuDate
                ;
            END
           
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_CHK
            INTO  @ChkCustomerCD,@ChkCollectCD,@JuchuuDate,@InvoiceGaku;
        END
    END

    SELECT * FROM #TableForInstruction
     WHERE OkFlg = 1
    --※画面項目転送表01のデータも含めての並び順
    ORDER BY DeliveryPlanDate, DeliveryPlanNO	
    ;
    
END


