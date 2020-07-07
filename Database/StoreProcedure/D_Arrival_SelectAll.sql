BEGIN TRY 
 Drop Procedure D_Arrival_SelectAll
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [D_Arrival_SelectAll]    */
CREATE PROCEDURE D_Arrival_SelectAll(
    -- Add the parameters for the stored procedure here
    @ArrivalDateFrom  varchar(10),
    @ArrivalDateTo  varchar(10),
    @SoukoCD  varchar(6),
    @SKUCD varchar(300),			--カンマ区切り
    @JanCD varchar(300), 		--カンマ区切り
    @VendorDeliveryNo varchar(15) 
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForSearchArrivalNO]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForSearchArrivalNO];
      END
      
    SELECT DH.ArrivalNO
    		,DH.InputDate
          --,CONVERT(varchar,DH.InputDate,111) AS InputDate
          ,DH.SKUCD
          ,DH.JANCD

            ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.ArrivalDate
             AND M.AdminNO = DH.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName

            ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.ArrivalDate
             AND M.AdminNO = DH.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.ArrivalDate
             AND M.AdminNO = DH.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
            ,DH.VendorDeliveryNo 
            ,DH.ArrivalSu
            
          ,0 AS Check1  --ITemCD用チェック
          ,0 AS Check2  --SKUCD用チェック
          ,0 AS Check3  --JANCD用チェック
          ,1 AS DelFlg
    INTO #TableForSearchArrivalNO 
    
    from D_Arrival AS DH
        WHERE DH.ArrivalDate >= (CASE WHEN @ArrivalDateFrom <> '' THEN CONVERT(DATE, @ArrivalDateFrom) ELSE DH.ArrivalDate END)
        AND DH.ArrivalDate <= (CASE WHEN @ArrivalDateTo <> '' THEN CONVERT(DATE, @ArrivalDateTo) ELSE DH.ArrivalDate END)
        AND DH.SoukoCD = @SoukoCD
        AND ISNULL(DH.VendorDeliveryNo,'') = (CASE WHEN @VendorDeliveryNo <> '' THEN @VendorDeliveryNo ELSE ISNULL(DH.VendorDeliveryNo,'') END)
       
        AND DH.DeleteDateTime IS NULL
             
    ORDER BY DH.ArrivalNO
    ;
    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    
    --SKUCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@SKUCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchArrivalNO
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
                    UPDATE #TableForSearchArrivalNO
                    SET Check2 = 0
                    WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,LEN(@SKUCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @SKUCD, @INDEX);
            	
                UPDATE #TableForSearchArrivalNO
                SET Check2 = 0
                WHERE SKUCD = SUBSTRING(@SKUCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchArrivalNO
        WHERE  Check2 = 1
        ;
    END;
    
    --JANCDより条件に合わないデータをテーブルから削除
    IF ISNULL(@JANCD,'') <> ''
    BEGIN
    	    	
        UPDATE #TableForSearchArrivalNO
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
                    UPDATE #TableForSearchArrivalNO
                    SET Check3 = 0
                    WHERE JANCD = SUBSTRING(@JANCD,@INDEX,LEN(@JANCD)-@INDEX+1)
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @JANCD, @INDEX);
            	
                UPDATE #TableForSearchArrivalNO
                SET Check3 = 0
                WHERE JANCD = SUBSTRING(@JANCD,@INDEX,@NEXT_INDEX-@INDEX)
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchArrivalNO
        WHERE  Check3 = 1
        ;
    END;
    
    SELECT *
    FROM #TableForSearchArrivalNO AS DH
    ORDER BY DH.ArrivalNO
    ;
END

GO
