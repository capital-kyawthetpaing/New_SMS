 BEGIN TRY 
 Drop Procedure dbo.[D_ArrivalPlan_SelectForPrint]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_ArrivalPlan_SelectForPrint]    */
CREATE PROCEDURE D_ArrivalPlan_SelectForPrint(
    -- Add the parameters for the stored procedure here
    @ArrivalPlanDateFrom  varchar(10),
    @ArrivalPlanDateTo  varchar(10),
    @ArrivalPlanMonthFrom  int,
    @ArrivalPlanMonthTo  int,
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),
    
    @ChkMikakutei tinyint,
    @ArrivalPlan int,
    @ChkKanbai tinyint,
    @ChkFuyo tinyint,
    
    @OrderCD  varchar(13),
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DA.ArrivalPlanNO
    	  ,DH.OrderNO
          ,ISNULL(DM.OrderRows,0) AS OrderRows
          ,DM.DisplayRows
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,DH.OrderCD          
          ,(SELECT top 1 A.VendorName + ' 御中'
          FROM M_Vendor A 
          WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS VendorName 

          ,DM.JuchuuNO
          ,DM.JuchuuRows
          ,ROW_NUMBER() OVER(PARTITION BY DH.OrderNO ORDER BY DM.DisplayRows) ROWNUM

          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN ISNULL(DM.ColorName,'') + ISNULL(DM.SizeName,'') 
          	  ELSE ISNULL(A.ColorName,'') + ISNULL(A.SizeName,'') END) AS ColorName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO 
              AND A.ChangeDate <= DH.OrderDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
          --,DM.ColorName
          ,DM.SizeName
          
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.ItemName ELSE A.SKUName END) AS SKUName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO 
              AND A.ChangeDate <= DH.OrderDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ITemCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ITemCD
            ,(SELECT top 1 M.AdminNO 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS AdminNO
            ,(SELECT top 1 M.SKUCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.JANCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.MakerItem 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS MakerItem
             
          ,DM.CommentOutStore
          ,DM.CommentInStore

          ,DM.OrderSu
          ,DM.OrderUnitPrice
          ,DM.OrderHontaiGaku
          
          ,DA.ArrivalPlanMonth      --予定月
          ,DA.ArrivalPlanCD       --予定状況 
          ,DA.ArrivalPlanSu-ArrivalSu AS ArrivalPlanSu
          ,(CASE WHEN DA.ArrivalPlanDate IS NULL THEN
            (CASE WHEN DA.ArrivalPlanMonth = 0 THEN M.Char1 
             ELSE (CASE WHEN M.Num2 = 1 THEN SUBSTRING(CONVERT(varchar,DA.ArrivalPlanMonth),1,4) + '/' + SUBSTRING(CONVERT(varchar,DA.ArrivalPlanMonth),5,2) + M.Char1 
             		ELSE SUBSTRING(CONVERT(varchar,DA.ArrivalPlanMonth),1,4) + '/' + SUBSTRING(CONVERT(varchar,DA.ArrivalPlanMonth),5,2) END) END)
            ELSE CONVERT(varchar,DA.ArrivalPlanDate,111) END) AS ArrivalPlanDate
                          
          ,(SELECT top 1 M.Char1 
            FROM M_MultiPorpose AS M 
            WHERE M.ID = 201
            AND M.[Key] = DM.TaniCD
            ) AS TaniName 
            
           ,(SELECT top 1 A.Print1 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print1
           ,(SELECT top 1 A.Print2 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print2
           ,(SELECT top 1 A.Print3 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print3
           ,(SELECT top 1 A.Print4 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print4
           ,(SELECT top 1 A.Print5 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print5
           ,(SELECT top 1 A.Print6 
           FROM M_Store A 
           WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
           AND A.StoreKBN IN (1,3)
           ORDER BY A.ChangeDate desc) AS Print6
          ,1 AS DelFlg
    
    INTO #TableForKaitouNoukiKakuninsho
    from D_ArrivalPlan AS DA
        
	INNER JOIN  D_OrderDetails AS DM 
        ON DM.OrderNO = DA.Number 
        AND DM.OrderRows = DA.NumberRows
        AND DM.DeleteDateTime IS NULL
        
	INNER JOIN  D_Order AS DH
        ON DM.OrderNO = DH.OrderNO 
        AND DH.DeleteDateTime IS NULL
    
    LEFT OUTER JOIN M_MultiPorpose AS M ON M.ID = 206 AND M.[Key] = DA.ArrivalPlanCD
    
    WHERE DH.StoreCD = @StoreCD
        AND DH.OrderCD = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE DH.OrderCD END)
        AND DH.DestinationKBN = 2   --直送で無い
        AND DH.OrderWayKBN = 2  --Net発注でない
        AND DH.ReturnFLG = 0    --キャンセルでない
        --AND DH.ApprovalStageFLG >= 1 --却下以外

        AND DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
        AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)

        AND DA.ArrivalSu <> DA.ArrivalPlanSu
        AND DA.ArrivalPlanKBN = 1
        AND DA.LastestFLG = 1
        AND DA.DeleteDateTime IS NULL
        AND ISNULL(DA.ArrivalPlanDate,'') >= (CASE WHEN @ArrivalPlanDateFrom <> '' THEN CONVERT(DATE, @ArrivalPlanDateFrom) ELSE ISNULL(DA.ArrivalPlanDate,'') END)
        AND ISNULL(DA.ArrivalPlanDate,'') <= (CASE WHEN @ArrivalPlanDateTo <> '' THEN CONVERT(DATE, @ArrivalPlanDateTo) ELSE ISNULL(DA.ArrivalPlanDate,'') END)
        AND ((DA.ArrivalPlanMonth >= (CASE WHEN @ArrivalPlanMonthFrom <> 0 THEN @ArrivalPlanMonthFrom ELSE DA.ArrivalPlanMonth END)
                AND DA.ArrivalPlanMonth <= (CASE WHEN @ArrivalPlanMonthTo <> 0 THEN @ArrivalPlanMonthTo ELSE DA.ArrivalPlanMonth END))
            OR (CONVERT(VARCHAR, ISNULL(DA.ArrivalPlanDate,''), 112) / 100 >= (CASE WHEN @ArrivalPlanMonthFrom <> 0 THEN @ArrivalPlanMonthFrom ELSE CONVERT(VARCHAR, ISNULL(DA.ArrivalPlanDate,''), 112) / 100 END)
                AND CONVERT(VARCHAR, ISNULL(DA.ArrivalPlanDate,''), 112) / 100 <= (CASE WHEN @ArrivalPlanMonthTo <> 0 THEN @ArrivalPlanMonthTo ELSE CONVERT(VARCHAR, ISNULL(DA.ArrivalPlanDate,''), 112) / 100 END))
            )
    ORDER BY DH.OrderNO
    ;
    
  
    ALTER TABLE [#TableForKaitouNoukiKakuninsho] ALTER COLUMN [OrderNO] VARCHAR(11) NOT NULL;
    ALTER TABLE [#TableForKaitouNoukiKakuninsho] ALTER COLUMN [OrderRows] int NOT NULL;
    ALTER TABLE [#TableForKaitouNoukiKakuninsho] ALTER COLUMN [ArrivalPlanNO] VARCHAR(11) NOT NULL;
    
    ALTER TABLE [#TableForKaitouNoukiKakuninsho] ADD PRIMARY KEY CLUSTERED ([OrderNO], [OrderRows], [ArrivalPlanNO])
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ;

    IF (ISNULL(@OrderDateFrom,'') <> '' OR ISNULL(@OrderDateTo,'') <> '')
    	AND (@ChkMikakutei = 1 OR @ChkFuyo = 1 OR @ChkKanbai = 1)
    BEGIN
        UPDATE #TableForKaitouNoukiKakuninsho
        SET DelFlg = 1
        ;

        --日付未確定分ONの場合
        IF @ChkMikakutei = 1
        BEGIN
            --入荷予定データが存在しない
            UPDATE #TableForKaitouNoukiKakuninsho
            SET DelFlg = 0
            WHERE NOT EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                WHERE DA.Number = #TableForKaitouNoukiKakuninsho.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );

            --予定日なしで入荷予定データが存在する
            UPDATE #TableForKaitouNoukiKakuninsho
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                WHERE DA.Number = #TableForKaitouNoukiKakuninsho.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.ArrivalPlanCD IS NULL
                AND DA.ArrivalPlanDate IS NULL
                AND DA.ArrivalPlanMonth IS NULL
                AND DA.DeleteDateTime IS NULL
            );
            
            --旬、予約、未定、確認中で入荷予定データが存在する
            UPDATE #TableForKaitouNoukiKakuninsho
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                INNER JOIN M_MultiPorpose AS M
                ON M.ID = 206
                AND M.[Key] = DA.ArrivalPlanCD
                AND M.Num2 IN (1,2,4,6)
                --入荷予定区分
                AND M.Num2 = (CASE WHEN @ArrivalPlan <> 0 THEN @ArrivalPlan ELSE M.Num2 END)
                WHERE DA.Number = #TableForKaitouNoukiKakuninsho.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );
            
        END

        --不要分ONの場合
        IF @ChkFuyo = 1
        BEGIN
            UPDATE #TableForKaitouNoukiKakuninsho
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                INNER JOIN M_MultiPorpose AS M
                ON M.ID = 206
                AND M.[Key] = DA.ArrivalPlanCD
                AND M.Num2 = 5
                WHERE DA.Number = #TableForKaitouNoukiKakuninsho.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );
        END
        
        --完売分ONの場合
        IF @ChkKanbai = 1
        BEGIN
            UPDATE #TableForKaitouNoukiKakuninsho
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                INNER JOIN M_MultiPorpose AS M
                ON M.ID = 206
                AND M.[Key] = DA.ArrivalPlanCD
                AND M.Num2 = 3
                WHERE DA.Number = #TableForKaitouNoukiKakuninsho.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );
        END
        
        DELETE FROM #TableForKaitouNoukiKakuninsho
        WHERE DelFlg = 1
        ;
    END  
    
    
    SELECT *
    FROM #TableForKaitouNoukiKakuninsho AS DH
    ORDER BY DH.OrderNO, DH.DisplayRows
    ;

END

GO
