 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Order_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Order_SelectAll](
    -- Add the parameters for the stored procedure here
    @OrderDateFrom  varchar(10),
    @OrderDateTo  varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @OrderCD  varchar(13),
    @SKUName varchar(100),
    @ITemCD varchar(300) ,		--カンマ区切り
    @SKUCD varchar(300),			--カンマ区切り
    @JanCD varchar(300), 		--カンマ区切り
    @MakerItem varchar(30) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForSearchHacchuuNO]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForSearchHacchuuNO];
      END
      
    SELECT DH.OrderNO
            ,(CASE DH.ApprovalStageFLG WHEN 9 THEN '承認済'
                                        WHEN 1 THEN '申請'
                                        WHEN 0 THEN '却下'
                                        ELSE '承認中' END) AS ApprovalStageFLG
          ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
          
          ,CONVERT(varchar,DH.InsertDateTime,111) AS InsertDateTime
          ,DH.OrderCD
--          ,DH.CustomerName
          ,(SELECT top 1 A.VendorName
          FROM M_Vendor A 
          WHERE A.VendorCD = DH.OrderCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
          ORDER BY A.ChangeDate desc) AS VendorName 
          
            ,(CASE DH.DestinationKBN WHEN 2 THEN (SELECT top 1 A.SoukoName
                                        FROM M_Souko AS A 
                                        WHERE A.SoukoCD = DH.DestinationSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
                                        ORDER BY A.ChangeDate desc) 
                                        WHEN 1 THEN (SELECT top 1 A.DeliveryName
                                        FROM D_DeliveryPlan A WHERE A.DeliveryPlanNO = DJ.DeliveryPlanNO)
                                        ELSE '' END) AS DeliveryName
                                        
            ,(CASE DH.DestinationKBN WHEN 2 THEN (SELECT top 1 A.SoukoName
                                        FROM M_Souko AS A 
                                        WHERE A.SoukoCD = DH.DestinationSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.OrderDate
                                        ORDER BY A.ChangeDate desc) 
                                        WHEN 1 THEN (SELECT top 1 ISNULL(A.DeliveryAddress1+ ' ','') + ISNULL(A.DeliveryAddress2,'')
                                        FROM D_DeliveryPlan A 
                                        WHERE A.DeliveryPlanNO = DJ.DeliveryPlanNO
                                        AND A.DeliveryKBN >= 1 )
                                        ELSE '' END) AS DeliveryAddress

            ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ITemCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS ITemCD
            ,(SELECT top 1 M.SKUCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.JANCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.MakerItem 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.AdminNO
             ORDER BY M.ChangeDate desc) AS MakerItem
          ,0 AS Check1  --ITemCD用チェック
          ,0 AS Check2  --SKUCD用チェック
          ,0 AS Check3  --JANCD用チェック
          ,1 AS DelFlg
    INTO #TableForSearchHacchuuNO 
    
    from D_Order AS DH
    LEFT OUTER JOIN D_OrderDetails AS DM ON DM.OrderNO = DH.OrderNO AND DM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_JuchuuDetails AS DJ ON DJ.JuchuuNO = DM.JuchuuNO AND DJ.JuchuuRows = DM.JuchuuRows AND DJ.DeleteDateTime IS NULL
        WHERE DH.OrderDate >= (CASE WHEN @OrderDateFrom <> '' THEN CONVERT(DATE, @OrderDateFrom) ELSE DH.OrderDate END)
        AND DH.OrderDate <= (CASE WHEN @OrderDateTo <> '' THEN CONVERT(DATE, @OrderDateTo) ELSE DH.OrderDate END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.OrderCD = (CASE WHEN @OrderCD <> '' THEN @OrderCD ELSE DH.OrderCD END)
        
        AND DH.DeleteDateTime IS NULL
          
        /*AND EXISTS(SELECT M.SKUName 
            FROM M_SKU AS M 
            WHERE DM.OrderNO = DH.OrderNO 
             AND DM.DeleteDateTime IS NULL
             AND M.ChangeDate <= DH.OrderDate
             AND M.AdminNO = DM.SKUNO
             
             )*/
             
    ORDER BY DH.OrderNO
    ;

	IF ISNULL(@SKUName,'') <> ''
	BEGIN

        UPDATE #TableForSearchHacchuuNO
        SET DelFlg = 1
        ;
        
        UPDATE #TableForSearchHacchuuNO
        SET DelFlg = 0
        WHERE SKUName LIKE '%' + @SKUName + '%'
        ;
        
        DELETE FROM #TableForSearchHacchuuNO
        WHERE DelFlg = 1
        ;
	END
	
	IF ISNULL(@MakerItem,'') <> ''
	BEGIN

        UPDATE #TableForSearchHacchuuNO
        SET DelFlg = 1
        ;
        
        UPDATE #TableForSearchHacchuuNO
        SET DelFlg = 0
        WHERE MakerItem LIKE '%' + @MakerItem + '%'
        ;
        
        DELETE FROM #TableForSearchHacchuuNO
        WHERE DelFlg = 1
        ;
	END

    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    
    --ITEMより条件に合わないデータをテーブルから削除
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchHacchuuNO
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
                    UPDATE #TableForSearchHacchuuNO
                    SET Check1 = 0
                    WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
            	
                UPDATE #TableForSearchHacchuuNO
                SET Check1 = 0
                WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchHacchuuNO
        WHERE  Check1 = 1
        ;
    END;
	
    --SKUCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchHacchuuNO
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
                    UPDATE #TableForSearchHacchuuNO
                    SET Check2 = 0
                    WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
            	
                UPDATE #TableForSearchHacchuuNO
                SET Check2 = 0
                WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchHacchuuNO
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchHacchuuNO
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
                    UPDATE #TableForSearchHacchuuNO
                    SET Check3 = 0
                    WHERE JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
            	
                UPDATE #TableForSearchHacchuuNO
                SET Check3 = 0
                WHERE JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchHacchuuNO
        WHERE  Check3 = 1
        ;
    END;
    
    SELECT DH.OrderNO
        , MAX(DH.ApprovalStageFLG) AS ApprovalStageFLG
        , MAX(DH.OrderDate) AS OrderDate
        , MAX(DH.VendorName) AS VendorName
        , MAX(DH.DeliveryName) AS DeliveryName
        , MAX(DH.DeliveryAddress) AS DeliveryAddress
    FROM #TableForSearchHacchuuNO AS DH
    GROUP BY DH.OrderNO
    ORDER BY DH.OrderNO
    ;
END


