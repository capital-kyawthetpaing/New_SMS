IF OBJECT_ID ( 'D_Shipping_SelectAll', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Shipping_SelectAll]
GO

/****** Object:  StoredProcedure [D_Shipping_SelectAll]    */
CREATE PROCEDURE D_Shipping_SelectAll(
    @ShippingDateFrom  varchar(10),
    @ShippingDateTo  varchar(10),
    @SoukoCD  varchar(6),
    @SKUCD varchar(300),			--カンマ区切り
    @JanCD varchar(300) 		--カンマ区切り
)AS
BEGIN
    SET NOCOUNT ON;

    IF OBJECT_ID( N'[dbo]..[#TableForSearchShippingNO]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForSearchShippingNO];
      END
      
    SELECT DH.ShippingNO
    	  ,DH.InputDateTime
          ,DM.SKUCD
          ,DM.JANCD
          ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
             WHERE M.ChangeDate <= DH.ShippingDate
               AND M.AdminNO = DM.AdminNO
               AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
              FROM M_SKU AS M 
             WHERE M.ChangeDate <= DH.ShippingDate
               AND M.AdminNO = DM.AdminNO
               AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
              FROM M_SKU AS M 
             WHERE M.ChangeDate <= DH.ShippingDate
               AND M.AdminNO = DM.AdminNO
               AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
            ,DM.ShippingSu
          ,0 AS Check1  --ITemCD用チェック
          ,0 AS Check2  --SKUCD用チェック
          ,0 AS Check3  --JANCD用チェック
          ,1 AS DelFlg
    INTO #TableForSearchShippingNO 
    
    FROM D_Shipping AS DH
   INNER JOIN D_ShippingDetails DM ON DH.ShippingNO = DM.ShippingNO
                                  AND DM.DeleteDateTime IS NULL
   WHERE DH.ShippingDate >= (CASE WHEN @ShippingDateFrom <> '' THEN CONVERT(DATE, @ShippingDateFrom) ELSE DH.ShippingDate END)
     AND DH.ShippingDate <= (CASE WHEN @ShippingDateTo <> '' THEN CONVERT(DATE, @ShippingDateTo) ELSE DH.ShippingDate END)
     AND DH.SoukoCD = @SoukoCD       
     AND DH.DeleteDateTime IS NULL
             
    ORDER BY DH.ShippingNO
    ;
    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    
    --SKUCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchShippingNO
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
                    UPDATE #TableForSearchShippingNO
                    SET Check2 = 0
                    WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
            	
                UPDATE #TableForSearchShippingNO
                SET Check2 = 0
                WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchShippingNO
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchShippingNO
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
                    UPDATE #TableForSearchShippingNO
                    SET Check3 = 0
                    WHERE JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
            	
                UPDATE #TableForSearchShippingNO
                SET Check3 = 0
                WHERE JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchShippingNO
        WHERE  Check3 = 1
        ;
    END;
    
    SELECT *
    FROM #TableForSearchShippingNO AS DH
    ORDER BY DH.ShippingNO
    ;
END

GO
