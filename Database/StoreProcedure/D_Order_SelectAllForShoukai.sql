 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectAllForShoukai] 
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Order_SelectAllForShoukai]    */
CREATE PROCEDURE [dbo].[D_Order_SelectAllForShoukai](
    -- Add the parameters for the stored procedure here
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),
    @ArrivalPlanDateFrom  varchar(10),
    @ArrivalPlanDateTo  varchar(10),
    @ArrivalDateFrom  varchar(10),
    @ArrivalDateTo  varchar(10),
    @PurchaseDateFrom  varchar(10),
    @PurchaseDateTo  varchar(10),
    
    @ChkMikakutei tinyint,
    @ArrivalPlan int,
    @ChkKanbai tinyint,
    @ChkFuyo tinyint,
    @ChkNyukaZumi tinyint,
    @ChkMiNyuka tinyint,
    @ChkJuchuAri tinyint,
    @ChkZaiko tinyint,
    @ChkMisyonin tinyint,
    @ChkSyoninzumi tinyint,
    @ChkChokuso tinyint,
    @ChkSouko  tinyint,
    @ChkNet  tinyint,
    @ChkFax  tinyint,
    @ChkEdi  tinyint,
    
    @OrderCD  varchar(13),
    @StoreCD  varchar(4),
    @MakerItem varchar(30) ,
    @SKUName varchar(100),
    @ITemCD varchar(300) ,      --âJâôâ}ïµÉÏéÞ
    @SKUCD varchar(300),            --âJâôâ}ïµÉÏéÞ
    @JanCD varchar(300),        --âJâôâ}ïµÉÏéÞ
    
    @JuchuuNO  varchar(11),
    @DestinationSoukoCD varchar(11),
    @Operator  varchar(10),
    @PC  varchar(30)

)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
	
    DECLARE @SYSDATETIME datetime;
    SET @SYSDATETIME = SYSDATETIME();

	IF right(@JanCD,1)=','
	begin 
		set @JanCD=left(@JanCD, len(@JanCD)-1)
	end
	
    
    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForHacchuuShoukai]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForHacchuuShoukai];
      END
      
    SELECT DH.OrderNO
          ,ISNULL(DM.OrderRows,0) AS OrderRows
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          ,DH.OrderCD          
          ,(SELECT top 1 A.VendorName
          FROM M_Vendor A 
          WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
		  AND A.VendorFlg = 1
          ORDER BY A.ChangeDate desc) AS VendorName 
          
          ,DA.ArrivalPlanDate

          ,(SELECT top 1 D.ArrivalPlanMonth
            FROM D_ArrivalPlan AS D 
            WHERE D.DeleteDateTime IS NULL
            AND DM.OrderNO = D.Number AND DM.OrderRows = D.NumberRows
            ORDER BY D.ArrivalPlanMonth desc
            ) AS ArrivalPlanMonth	--ù\ÆÞîÄ

          ,(SELECT top 1 D.ArrivalPlanCD
            FROM D_ArrivalPlan AS D 
            WHERE D.DeleteDateTime IS NULL
            AND DM.OrderNO = D.Number AND DM.OrderRows = D.NumberRows
            ORDER BY D.ArrivalPlanMonth desc	--ù\ÆÞîÄé¬ì┼æÕé╠Ælé╠âîâRü[âhé╠ù\ÆÞÅ¾ïÁé­ì╠ùp									
            ) AS ArrivalPlanCD	--ù\ÆÞÅ¾ïÁ

          ,DL.ArrivalDate	--ô³ëÎô·
          ,ISNULL(DL2.ArrivalSu,0) AS ArrivalSu
          ,DP.PurchaseDate	--Ädô³ô·
          
          ,CONVERT(varchar,DH.InsertDateTime,111) AS InsertDateTime

			,DH.DestinationKBN AS Destination
            ,(CASE DH.DestinationKBN WHEN 1 THEN 'üZ' ElSE '' END) AS DestinationKBN
            ,(CASE DH.DestinationKBN WHEN 2 THEN (SELECT top 1 A.SoukoName
                                                FROM M_Souko AS A 
                                                WHERE A.SoukoCD = DH.DestinationSoukoCD AND A.DeleteFlg = 0 
                                                AND A.ChangeDate <= DH.OrderDate
                                                ORDER BY A.ChangeDate desc) 
                                        WHEN 1 THEN (SELECT top 1 A.DeliveryName
                                            FROM D_DeliveryPlan A 
                                            WHERE A.DeliveryPlanNO = DJ.DeliveryPlanNO)
                                            ELSE '' END) AS DeliveryName
           
           ,DH.OrderWayKBN AS OrderWay                             
           ,(CASE DH.OrderWayKBN WHEN 1 THEN N'Netö¡Æì' WHEN 2 THEN N'FAXö¡Æì' WHEN 3 THEN N'EDIö¡Æì' ELSE '' END) OrderWayKBN
    		
    	   ,DH.ApprovalStageFLG AS ApprovalStage
           ,(CASE DH.ApprovalStageFLG WHEN 9 THEN N'Å│öFì¤'
                                      WHEN 1 THEN N'É\É┐'
                                      WHEN 0 THEN N'ïpë║'
                                      WHEN 10 THEN N'Å│öFòsùv'
                                      ELSE N'Å│öFÆå' END) AS ApprovalStageFLG
           
          ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.OrderDate
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS StoreName
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS StaffName
          
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
            ,(SELECT top 1 (CASE M.VariousFLG WHEN 1 THEN DM.MakerItem ELSE M.MakerItem  END) AS MakerItem 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS MakerItem
             
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.ColorName ELSE A.ColorName END) AS ColorName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.OrderDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DM.SizeName ELSE A.SizeName END) AS SizeName 
              FROM M_SKU A 
              WHERE A.AdminNO = DM.AdminNO AND A.ChangeDate <= DH.OrderDate 
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SizeName
             
              ,DM.CommentOutStore
              ,DM.CommentInStore

              ,DM.OrderSu
              ,DM.OrderUnitPrice
              ,DM.OrderHontaiGaku
              ,DM.JuchuuNO
              ,DM.ArrivePlanDate
                              
              ,(SELECT top 1 M.Num2 
                FROM M_MultiPorpose AS M 
                INNER JOIN D_ArrivalPlan AS D ON D.ArrivalPlanCD = M.Char1
                AND D.ArrivalPlanKBN = 1
                AND D.DeleteDateTime IS NULL
                WHERE M.ID = 206
                AND DH.OrderNO = D.Number
                ) AS Num2 
              
          ,0 AS Check1  --ITemCDùpâ`âFâbâN
          ,0 AS Check2  --SKUCDùpâ`âFâbâN
          ,0 AS Check3  --JANCDùpâ`âFâbâN
          ,1 AS DelFlg
    INTO #TableForHacchuuShoukai 
    
    from D_Order AS DH
    LEFT OUTER JOIN D_OrderDetails AS DM 
        ON DM.OrderNO = DH.OrderNO 
        AND DM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_JuchuuDetails AS DJ 
        ON DJ.JuchuuNO = DM.JuchuuNO 
        AND DJ.JuchuuRows = DM.JuchuuRows 
        AND DJ.DeleteDateTime IS NULL
    
    --ô³ëÎù\ÆÞô·
    LEFT OUTER JOIN (SELECT  D.Number, D.NumberRows
    		, CONVERT(varchar, MAX(D.ArrivalPlanDate), 111) AS ArrivalPlanDate
            FROM D_ArrivalPlan AS D 
            WHERE D.DeleteDateTime IS NULL
            AND D.ArrivalPlanDate >= (CASE WHEN @ArrivalPlanDateFrom <> '' THEN CONVERT(DATE, @ArrivalPlanDateFrom) ELSE D.ArrivalPlanDate END)
            AND D.ArrivalPlanDate <= (CASE WHEN @ArrivalPlanDateTo <> '' THEN CONVERT(DATE, @ArrivalPlanDateTo) ELSE D.ArrivalPlanDate END)
            GROUP BY D.Number, D.NumberRows
            ) AS DA ON DM.OrderNO = DA.Number AND DM.OrderRows = DA.NumberRows

	--ô³ëÎô·
    LEFT OUTER JOIN (SELECT B.Number, B.NumberRows
    		,CONVERT(varchar, MAX(D.ArrivalDate), 111) AS ArrivalDate
            FROM D_Arrival AS D
            INNER JOIN D_ArrivalDetails AS DA
            ON DA.ArrivalNO = D.ArrivalNO
            AND DA.DeleteDateTime IS NULL
    		INNER JOIN D_ArrivalPlan AS B ON B.ArrivalPlanNO = DA.ArrivalPlanNO
            AND B.DeleteDateTime IS NULL
            
            WHERE D.DeleteDateTime IS NULL
            AND D.ArrivalDate >= (CASE WHEN @ArrivalDateFrom <> '' THEN CONVERT(DATE, @ArrivalDateFrom) ELSE D.ArrivalDate END)
            AND D.ArrivalDate <= (CASE WHEN @ArrivalDateTo <> '' THEN CONVERT(DATE, @ArrivalDateTo) ELSE D.ArrivalDate END)
            GROUP BY B.Number, B.NumberRows
            ) AS DL ON DM.OrderNO = DL.Number AND DM.OrderRows = DL.NumberRows
    
    --ô³ëÎÉö
    LEFT OUTER JOIN (SELECT B.Number, B.NumberRows
    		,SUM(D.ArrivalSu) AS ArrivalSu
            FROM D_Arrival AS D
            INNER JOIN D_ArrivalDetails AS DA
            ON DA.ArrivalNO = D.ArrivalNO
            AND DA.DeleteDateTime IS NULL
    		INNER JOIN D_ArrivalPlan AS B ON B.ArrivalPlanNO = DA.ArrivalPlanNO
            AND B.DeleteDateTime IS NULL
            
            WHERE D.DeleteDateTime IS NULL
    --éáéªé─Å­îÅé­ìiéþé©é╔ô³ëÎÉöé╠ìçîvé­îvÄZ
    --        AND D.ArrivalDate >= (CASE WHEN @ArrivalDateFrom <> '' THEN CONVERT(DATE, @ArrivalDateFrom) ELSE D.ArrivalDate END)
    --        AND D.ArrivalDate <= (CASE WHEN @ArrivalDateTo <> '' THEN CONVERT(DATE, @ArrivalDateTo) ELSE D.ArrivalDate END)
            GROUP BY B.Number, B.NumberRows
            ) AS DL2 ON DM.OrderNO = DL2.Number AND DM.OrderRows = DL2.NumberRows
            
    --Ädô³ô·
    LEFT OUTER JOIN (SELECT D.OrderNO, D.OrderRows
    		,CONVERT(varchar, MAX(C.PurchaseDate), 111) AS PurchaseDate     
            FROM D_PurchaseDetails AS D
            INNER JOIN D_Purchase AS C ON C.PurchaseNO = D.PurchaseNO
            AND C.DeleteDateTime IS NULL
            
            WHERE D.DeleteDateTime IS NULL
            AND C.PurchaseDate >= (CASE WHEN @PurchaseDateFrom <> '' THEN CONVERT(DATE, @PurchaseDateFrom) ELSE C.PurchaseDate END)
            AND C.PurchaseDate <= (CASE WHEN @PurchaseDateTo <> '' THEN CONVERT(DATE, @PurchaseDateTo) ELSE C.PurchaseDate END)
            GROUP BY D.OrderNO, D.OrderRows
            ) AS DP ON DP.OrderNO = DM.OrderNO AND DP.OrderRows = DM.OrderRows
                        
    WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
        AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
       
        AND DH.OrderCD = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE DH.OrderCD END)
        AND DH.StoreCD = @StoreCD
        AND DH.DestinationSoukoCD = (CASE WHEN @DestinationSoukoCD <> '' THEN @DestinationSoukoCD ELSE DH.DestinationSoukoCD END)
       
        AND DH.DeleteDateTime IS NULL
             
    ORDER BY DH.OrderNO
    ;
    
    ALTER TABLE [#TableForHacchuuShoukai] ADD PRIMARY KEY CLUSTERED ([OrderNO], [OrderRows])
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ;
    IF @ChkMikakutei = 1 OR @ChkFuyo = 1 OR @ChkKanbai = 1 
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;

        --ô·òtûóèmÆÞò¬ONé╠ÅÛìç
        IF @ChkMikakutei = 1
        BEGIN
            --ô³ëÎù\ÆÞâfü[â^é¬æÂì¦éÁé╚éó
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE NOT EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                WHERE DA.Number = #TableForHacchuuShoukai.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );
            
            --ù\ÆÞô·é╚éÁé┼ô³ëÎù\ÆÞâfü[â^é¬æÂì¦éÀéÚ
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                WHERE DA.Number = #TableForHacchuuShoukai.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.ArrivalPlanCD IS NULL
                AND DA.ArrivalPlanDate IS NULL
                AND DA.ArrivalPlanMonth IS NULL
                AND DA.DeleteDateTime IS NULL
            );
            
            --Å{üAù\û±üAûóÆÞüAèmöFÆåé┼ô³ëÎù\ÆÞâfü[â^é¬æÂì¦éÀéÚ
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE EXISTS(
                SELECT DA.ArrivalPlanDate
                FROM D_ArrivalPlan AS DA
                INNER JOIN M_MultiPorpose AS M
                ON M.ID = 206
                AND M.[Key] = DA.ArrivalPlanCD
                AND M.Num2 IN (1,2,4,6)
                --ô³ëÎù\ÆÞïµò¬
                AND M.Num2 = (CASE WHEN @ArrivalPlan <> 0 THEN @ArrivalPlan ELSE M.Num2 END)
                WHERE DA.Number = #TableForHacchuuShoukai.OrderNO
                AND DA.ArrivalPlanKBN = 1
                AND DA.DeleteDateTime IS NULL
            );
            
        END

        --òsùvò¬ONé╠ÅÛìç
        IF @ChkFuyo = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ArrivePlanDate IS NULL
            AND Num2 = 5
            ;
        END
        
        --è«öäò¬ONé╠ÅÛìç
        IF @ChkKanbai = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ArrivePlanDate IS NULL
            AND Num2 = 3
            ;
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END

    IF @ChkNyukaZumi = 1 OR @ChkMiNyuka = 1 
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;

        --ô³ëÎì¤é¦ONé╠ÅÛìç
        IF @ChkNyukaZumi = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ArrivalSu >= OrderSu
            AND ArrivalSu > 0
            ;
            
        END

        --ûóô³ëÎONé╠ÅÛìç
        IF @ChkMiNyuka = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ArrivalSu < OrderSu 
            ;
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
    IF @ChkJuchuAri = 1 OR @ChkZaiko = 1
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        --Ä¾ÆìéáéÞ
        IF @ChkJuchuAri = 1 
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE JuchuuNO IS NOT NULL 
            ;
        END
        
        --ì¦î╔òÔÅ[ò¬
        IF @ChkZaiko = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE JuchuuNO IS NULL 
            ;
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
        
    END
    
    IF @ChkMisyonin = 1 OR @ChkSyoninzumi = 1
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        --ûóÅ│öF
        IF @ChkMisyonin = 1 
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ApprovalStage < 9
            ;
        END
        
        --Å│öFì¤
        IF @ChkSyoninzumi = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE ApprovalStage >= 9
            ;
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
    IF @ChkChokuso = 1 OR @ChkSouko = 1
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        --Æ╝æù
        IF @ChkChokuso = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE Destination = 1
            ;
        END
        
        --æqî╔
        IF @ChkSouko = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE Destination = 2
            ;
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
    IF @ChkNet= 1 OR @ChkFax= 1 OR @ChkEdi = 1
    BEGIN
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        --NETö¡Æì
        IF @ChkNet= 1 
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE OrderWay = 1
            ;
        END

        --FAXö¡Æì
        IF @ChkFax= 1 
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE OrderWay = 2
            ;
        END

        --EDIö¡Æì
        IF @ChkEdi = 1
        BEGIN
            UPDATE #TableForHacchuuShoukai
            SET DelFlg = 0
            WHERE OrderWay = 3
            ;
        END

                
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END

	IF ISNULL(@ArrivalPlanDateFrom,'') <> '' OR ISNULL(@ArrivalPlanDateTo,'') <> '' 
	BEGIN
        DELETE FROM #TableForHacchuuShoukai
        WHERE ArrivalPlanDate IS NULL
	END
	
    IF ISNULL(@SKUName,'') <> ''
    BEGIN

        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 0
        WHERE SKUName LIKE '%' + @SKUName + '%'
        ;
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END
    
    IF ISNULL(@MakerItem,'') <> ''
    BEGIN

        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 1
        ;
        
        UPDATE #TableForHacchuuShoukai
        SET DelFlg = 0
        WHERE MakerItem LIKE '%' + @MakerItem + '%'
        ;
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE DelFlg = 1
        ;
    END

    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    
    --ITEMéµéÞÅ­îÅé╔ìçéÝé╚éóâfü[â^é­âeü[âuâïé®éþìÝÅ£
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
                
        UPDATE #TableForHacchuuShoukai
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
                    --âfü[â^é¬êÛé┬é╠é¦é╠ÅÛìç
                    UPDATE #TableForHacchuuShoukai
                    SET Check1 = 0
                    WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
                
                UPDATE #TableForHacchuuShoukai
                SET Check1 = 0
                WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE  Check1 = 1
        ;
    END;
    
    --SKUCDéµéÞÅ­îÅé╔ìçéÝé╚éóâfü[â^é­âeü[âuâïé®éþìÝÅ£
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
                
        UPDATE #TableForHacchuuShoukai
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
                    --âfü[â^é¬êÛé┬é╠é¦é╠ÅÛìç
                    UPDATE #TableForHacchuuShoukai
                    SET Check2 = 0
                    WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
                
                UPDATE #TableForHacchuuShoukai
                SET Check2 = 0
                WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCDéµéÞÅ­îÅé╔ìçéÝé╚éóâfü[â^é­âeü[âuâïé®éþìÝÅ£
    IF ISNULL(@JanCD,'') <> ''
    BEGIN
                
        UPDATE #TableForHacchuuShoukai
        SET Check3 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @JanCD, @INDEX) = 0
            BEGIN
                IF LEN(@JanCD)-@INDEX >= 0
                BEGIN
                    --âfü[â^é¬êÛé┬é╠é¦é╠ÅÛìç
                    UPDATE #TableForHacchuuShoukai
                    SET Check3 = 0
                    WHERE JANCD = SUBSTRING(@JanCD,@INDEX,LEN(@JanCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
                SET @NEXT_INDEX = CHARINDEX(',', @JanCD, @INDEX);
                
                UPDATE #TableForHacchuuShoukai
                SET Check3 = 0
                WHERE JANCD = SUBSTRING(@JanCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
                SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForHacchuuShoukai
        WHERE  Check3 = 1
        ;
    END;
    
    --üyL_LogüzINSERT
    --ÅêùØùÜù­âfü[â^éÍìXÉV     
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'HacchuuShoukai',
        @PC,
        NULL,
        NULL;
        
    SELECT *
    FROM #TableForHacchuuShoukai AS DH
    ORDER BY DH.OrderNO, DH.OrderRows
    ;
END


GO
