/****** Object:  StoredProcedure [D_Purchase_SelectAll]    */
CREATE PROCEDURE D_Purchase_SelectAll(
    -- Add the parameters for the stored procedure here
    @PurchaseDateFrom  varchar(10),
    @PurchaseDateTo  varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @VendorCD  varchar(13),
    @SKUName varchar(100),
    @ITemCD varchar(300) ,		--�J���}��؂�
    @SKUCD varchar(300),			--�J���}��؂�
    @JanCD varchar(300), 		--�J���}��؂�
    @MakerItem varchar(30) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForSearchPurchaseNO]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForSearchPurchaseNO];
      END
      
    SELECT DH.PurchaseNO
            ,(CASE DH.ReturnsFlg WHEN 1 THEN '�ԕi'
                                 ELSE (CASE DH.ProcessKBN WHEN 1 THEN '���׎d��'
                                        WHEN 2 THEN '�d��'
                                        ELSE ''END) END) AS ReturnsFlg
          ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
          ,DH.VendorCD
          ,(SELECT top 1 A.VendorName
          FROM M_Vendor A 
          WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
		  AND A.VendorFlg = 1
          ORDER BY A.ChangeDate desc) AS VendorName
          ,DH.StaffCD + ' ' + ISNULL((SELECT top 1 A.StaffName
                                      FROM M_Staff AS A 
                                      WHERE A.StaffCD = DH.StaffCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
                                      ORDER BY A.ChangeDate desc),'') AS Staff
          ,DM.DisplayRows
          ,DM.PurchaseSu
          ,DM.PurchaserUnitPrice
          ,DM.PurchaseGaku
          ,DM.SKUCD
          ,DM.JanCD
          ,DM.ItemName
          ,DM.ColorName + ' ' + DM.SizeName AS ColorName

          ,(SELECT top 1 M.SKUName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.PurchaseDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.MakerItem
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.PurchaseDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS MakerItem
            ,(SELECT top 1 M.ITemCD 
            FROM M_SKU AS M
            WHERE M.ChangeDate <= DH.PurchaseDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ITemCD

          ,0 AS Check1  --ITemCD�p�`�F�b�N
          ,0 AS Check2  --SKUCD�p�`�F�b�N
          ,0 AS Check3  --JANCD�p�`�F�b�N
          ,0 AS Check4  --MakerItem�p�`�F�b�N
          ,1 AS DelFlg
    INTO #TableForSearchPurchaseNO 
    
    from D_Purchase AS DH
    LEFT OUTER JOIN D_PurchaseDetails AS DM ON DM.PurchaseNO = DH.PurchaseNO AND DM.DeleteDateTime IS NULL
        WHERE DH.PurchaseDate >= (CASE WHEN @PurchaseDateFrom <> '' THEN CONVERT(DATE, @PurchaseDateFrom) ELSE DH.PurchaseDate END)
        AND DH.PurchaseDate <= (CASE WHEN @PurchaseDateTo <> '' THEN CONVERT(DATE, @PurchaseDateTo) ELSE DH.PurchaseDate END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DH.VendorCD END)
        AND DH.DeleteDateTime IS NULL
    ORDER BY DH.PurchaseNO
    ;

	IF ISNULL(@SKUName,'') <> ''
	BEGIN

        UPDATE #TableForSearchPurchaseNO
        SET DelFlg = 1
        ;
        
        UPDATE #TableForSearchPurchaseNO
        SET DelFlg = 0
        WHERE SKUName LIKE '%' + @SKUName + '%'
        ;
        
        DELETE FROM #TableForSearchPurchaseNO
        WHERE DelFlg = 1
        ;
	END
    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    
    --ITEM�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchPurchaseNO
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
                    UPDATE #TableForSearchPurchaseNO
                    SET Check1 = 0
                    WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
            	
                UPDATE #TableForSearchPurchaseNO
                SET Check1 = 0
                WHERE ITemCD = SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchPurchaseNO
        WHERE  Check1 = 1
        ;
    END;
	
    --SKUCD�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchPurchaseNO
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
                    UPDATE #TableForSearchPurchaseNO
                    SET Check2 = 0
                    WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
            	
                UPDATE #TableForSearchPurchaseNO
                SET Check2 = 0
                WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchPurchaseNO
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCD�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchPurchaseNO
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
                    UPDATE #TableForSearchPurchaseNO
                    SET Check3 = 0
                    WHERE JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
            	
                UPDATE #TableForSearchPurchaseNO
                SET Check3 = 0
                WHERE JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchPurchaseNO
        WHERE  Check3 = 1
        ;
    END;
    
    --MakerItemD�������ɍ���Ȃ��f�[�^���e�[�u������폜
	IF ISNULL(@MakerItem,'') <> ''
	BEGIN

        UPDATE #TableForSearchPurchaseNO
        SET Check4 = 1
        ;
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @MakerItem, @INDEX) = 0
            BEGIN
                IF LEN(@MakerItem)-@INDEX >= 0
                BEGIN
                    --�f�[�^����݂̂̏ꍇ
                    UPDATE #TableForSearchPurchaseNO
                    SET Check3 = 0
                    WHERE MakerItem = SUBSTRING(@MakerItem,@INDEX,LEN(@MakerItem)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @MakerItem, @INDEX);
            	
                UPDATE #TableForSearchPurchaseNO
                SET Check3 = 0
                WHERE MakerItem = SUBSTRING(@MakerItem,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchPurchaseNO
        WHERE  Check4 = 1
        ;
	END
	
    SELECT DH.*
    FROM #TableForSearchPurchaseNO AS DH
    INNER JOIN (SELECT tbl.PurchaseNO, MIN(tbl.DisplayRows) AS DisplayRows 
    			FROM #TableForSearchPurchaseNO AS tbl
    GROUP BY tbl.PurchaseNO
    )AS DM
    ON DM.PurchaseNO = DH.PurchaseNO AND DM.DisplayRows = DH.DisplayRows
    ORDER BY DH.PurchaseNO
    ;
END

GO
