 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectForSearchProduct]
END try
BEGIN CATCH END CATCH 

/****** Object:  StoredProcedure [dbo].[M_SKU_SelectForSearchProduct]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SKU_SelectForSearchProduct]    */
CREATE PROCEDURE M_SKU_SelectForSearchProduct(
    -- Add the parameters for the stored procedure here
    @MainVendorCD varchar(13),
    @BrandCD varchar(6),
    @SKUName varchar(100),
    @JanCD varchar(13), 
    @SKUCD varchar(30),
    @CommentInStore varchar(300) ,		--�J���}��؂�
    @OrOrAnd tinyInt,
    @ReserveCD varchar(3),
    @NoticesCD varchar(3),
    @PostageCD varchar(3),
    @OrderAttentionCD varchar(3),
    @SportsCD varchar(6),
--    @ClassificationA varchar(6),
--    @ClassificationB varchar(6),
--    @ClassificationC varchar(6),
    @TagName1 varchar(20),
    @TagName2 varchar(20),
    @TagName3 varchar(20),
    @TagName4 varchar(20),
    @TagName5 varchar(20),
    
    @YearTerm varchar(6),
    @Season varchar(6),
    @CatalogNO varchar(20),
    @InstructionsNO varchar(1000),
    
    @MakerItem varchar(300) ,	--�J���}��؂�
    @ITemCD varchar(300) ,		--�J���}��؂�
    @InputDateFrom varchar(10),
    @InputDateTo varchar(10), 
    @UpdateDateFrom  varchar(10),
    @UpdateDateTo  varchar(10),
    @ApprovalDateFrom varchar(10),
    @ApprovalDateTo varchar(10),
    @ChangeDate varchar(10),
    @SearchFlg tinyInt,
    @ItemOrMaker tinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    IF OBJECT_ID( N'[dbo]..[#TableForSearchProduct]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForSearchProduct];
      END
      
    -- Insert statements for procedure here
    SELECT MS.AdminNO
          ,MS.SKUCD
          ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MS.VariousFLG
          ,MS.SKUName
          ,MS.SKUShortName
          ,MS.ITemCD
          ,MS.ColorNO
          ,MS.SizeNO
          ,MS.JanCD
          ,(CASE MS.SetKBN WHEN 1 THEN '�Z' ELSE '' END) AS SetKBN
          ,MS.PresentKBN
          ,MS.SampleKBN
          ,MS.DiscountKBN
          ,MS.ColorName
          ,MS.SizeName
          ,MS.MainVendorCD
          ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
          ,MS.MakerVendorCD
          ,MS.BrandCD
          ,(SELECT A.BrandName FROM M_Brand A WHERE A.BrandCD = MS.BrandCD) AS BrandName
          ,MS.MakerItem
          ,MS.TaniCD
          ,MS.SportsCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '202' AND A.[Key] = MS.SportsCD) AS SportsName

          ,W.TagName1
          ,W.TagName2
          ,W.TagName3
          ,W.TagName4
          ,W.TagName5
          ,MS.ZaikoKBN
          ,MS.Rack
          ,MS.ReserveCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '311' AND A.[Key] = MS.ReserveCD) AS ReserveName
          ,MS.NoticesCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '310' AND A.[Key] = MS.NoticesCD) AS NoticesName
          ,MS.PostageCD
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '309' AND A.[Key] = MS.PostageCD) AS PostageName

          ,MS.TaxRateFLG
          ,MS.PriceWithTax
          ,MS.PriceOutTax
          ,MS.OrderPriceWithTax
          ,MS.OrderPriceWithoutTax
          ,MS.CommentInStore
          ,MS.CommentOutStore
          ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
        
          ,MI.YearTerm
          ,MI.Season
          ,MI.CatalogNO
          ,MI.InstructionsNO
    
          ,0 AS Check1  --���l�p�`�F�b�N
          ,0 AS Check2  --���[�J�[���iCD�p�`�F�b�N
          ,0 AS Check3  --���[�J�[���iCD�p�`�F�b�N
          ,0 AS Check4  --�^�O�p�`�F�b�N
    INTO #TableForSearchProduct 
          
    from F_SKU(@ChangeDate) MS

    LEFT OUTER JOIN
    (SELECT W.AdminNo
        ,W.ChangeDate
        ,MAX(CASE WHEN RowNum=1 THEN  W.[TagName] ELSE NULL END) AS TagName1
        ,MAX(CASE WHEN RowNum=2 THEN  W.[TagName] ELSE NULL END) AS TagName2
        ,MAX(CASE WHEN RowNum=3 THEN  W.[TagName] ELSE NULL END) AS TagName3
        ,MAX(CASE WHEN RowNum=4 THEN  W.[TagName] ELSE NULL END) AS TagName4
        ,MAX(CASE WHEN RowNum=5 THEN  W.[TagName] ELSE NULL END) AS TagName5
        FROM (
            SELECT [AdminNO]
                  ,[ChangeDate]
                  ,T.[SEQ]
                  ,T.[TagName]
                  ,ROW_NUMBER() OVER(PARTITION BY T.[AdminNO], T.[ChangeDate] ORDER BY T.SEQ) AS RowNum
              FROM [dbo].[M_SKUTag] T
            )AS W  
		GROUP BY W.AdminNo, W.ChangeDate
    ) AS W ON W.AdminNo = MS.AdminNo AND W.ChangeDate = MS.ChangeDate
	
    LEFT OUTER JOIN M_SKUInfo MI ON MI.AdminNO = MS.AdminNO AND MI.ChangeDate = MS.ChangeDate
    LEFT OUTER JOIN (SELECT MI.AdminNO,MI.ChangeDate,MAX(MI.SEQ) AS SEQ FROM M_SKUInfo MI GROUP BY MI.AdminNO,MI.ChangeDate) AS MMI
        ON MMI.AdminNO = MI.AdminNO AND MMI.ChangeDate = MI.ChangeDate AND MMI.SEQ = MI.SEQ
    --INNER JOIN(SELECT M.AdminNO, MAX(M.ChangeDate) AS ChangeDate
    --    FROM M_SKU M
   --     LEFT OUTER JOIN M_SKUInfo MI ON MI.AdminNO = M.AdminNO AND MI.ChangeDate = M.ChangeDate
   --     LEFT OUTER JOIN (SELECT MI.AdminNO,MI.ChangeDate,MAX(MI.SEQ) AS SEQ FROM M_SKUInfo MI GROUP BY MI.AdminNO,MI.ChangeDate) AS MMI
   --         ON MMI.AdminNO = MI.AdminNO AND MMI.ChangeDate = MI.ChangeDate AND MMI.SEQ = MI.SEQ
      
     --   AND M.ChangeDate <= CONVERT(DATE, @ChangeDate)
      --  AND M.DeleteFlg = 0 �ŐV�f�[�^���폜����Ă���ꍇ�͍폜�������邽��
      --  GROUP BY M.AdminNO
    --)AS M ON M.AdminNO = MS.AdminNO AND M.ChangeDate = MS.ChangeDate
    
    WHERE (ISNULL(W.TagName1,'') =  (CASE WHEN @TagName1 <> '' THEN @TagName1 ELSE ISNULL(W.TagName1,'') END)
    OR ISNULL(W.TagName2,'') =  (CASE WHEN @TagName2 <> '' THEN @TagName2 ELSE ISNULL(W.TagName2,'') END)
    OR ISNULL(W.TagName3,'') =  (CASE WHEN @TagName3 <> '' THEN @TagName3 ELSE ISNULL(W.TagName3,'') END)
    OR ISNULL(W.TagName4,'') =  (CASE WHEN @TagName4 <> '' THEN @TagName4 ELSE ISNULL(W.TagName4,'') END)
    OR ISNULL(W.TagName5,'') =  (CASE WHEN @TagName5 <> '' THEN @TagName5 ELSE ISNULL(W.TagName5,'') END)
    )
    AND MS.DeleteFlg = 0
	AND ISNULL(MS.JanCD,'') = (CASE WHEN @JanCD <> '' THEN @JanCD ELSE ISNULL(MS.JanCD,'') END)
    AND MS.SKUCD = (CASE WHEN @SKUCD <> '' THEN @SKUCD ELSE MS.SKUCD END)
    AND ISNULL(MS.MainVendorCD,'') = (CASE WHEN @MainVendorCD <> '' THEN @MainVendorCD ELSE ISNULL(MS.MainVendorCD,'') END)
    AND ISNULL(MS.BrandCD,'') =  (CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE ISNULL(MS.BrandCD,'') END)
    AND ISNULL(MS.SportsCD,'') = (CASE WHEN @SportsCD <> '' THEN @SportsCD ELSE ISNULL(MS.SportsCD,'') END)
    AND ISNULL(MS.ReserveCD,'') = (CASE WHEN @ReserveCD <> '' THEN @ReserveCD ELSE ISNULL(MS.ReserveCD,'') END)
    AND ISNULL(MS.NoticesCD,'') = (CASE WHEN @NoticesCD <> '' THEN @NoticesCD ELSE ISNULL(MS.NoticesCD,'') END)
    AND ISNULL(MS.PostageCD,'') = (CASE WHEN @PostageCD <> '' THEN @PostageCD ELSE ISNULL(MS.PostageCD,'') END)
    
    AND MS.SKUName LIKE '%' +  (CASE WHEN @SKUName <> '' THEN @SKUName ELSE MS.SKUName END) + '%'

    AND CONVERT(DATE,MS.InsertDateTime) >= (CASE WHEN @InputDateFrom <> '' THEN CONVERT(DATE, @InputDateFrom) ELSE CONVERT(DATE,MS.InsertDateTime) END)
    AND CONVERT(DATE,MS.InsertDateTime) <= (CASE WHEN @InputDateTo <> '' THEN CONVERT(DATE, @InputDateTo) ELSE CONVERT(DATE,MS.InsertDateTime) END)
    AND CONVERT(DATE,MS.UpdateDateTime) >= (CASE WHEN @UpdateDateFrom <> '' THEN CONVERT(DATE, @UpdateDateFrom) ELSE CONVERT(DATE,MS.UpdateDateTime) END)
    AND CONVERT(DATE,MS.UpdateDateTime) <= (CASE WHEN @UpdateDateTo <> '' THEN CONVERT(DATE, @UpdateDateTo) ELSE CONVERT(DATE,MS.UpdateDateTime) END)
    AND ISNULL(MS.ApprovalDate,SYSDATETIME()) >= (CASE WHEN @ApprovalDateFrom <> '' THEN CONVERT(DATE, @ApprovalDateFrom) ELSE ISNULL(MS.ApprovalDate,SYSDATETIME()) END)
    AND ISNULL(MS.ApprovalDate,SYSDATETIME()) <= (CASE WHEN @ApprovalDateTo <> '' THEN CONVERT(DATE, @ApprovalDateTo) ELSE ISNULL(MS.ApprovalDate,SYSDATETIME()) END)

    AND ISNULL(MI.YearTerm,'') = (CASE WHEN @YearTerm <> '' THEN @YearTerm ELSE ISNULL(MI.YearTerm,'') END)
    AND ISNULL(MI.Season,'') = (CASE WHEN @Season <> '' THEN @Season ELSE ISNULL(MI.Season,'') END)
    AND ISNULL(MI.CatalogNO,'') = (CASE WHEN @CatalogNO <> '' THEN @CatalogNO ELSE ISNULL(MI.CatalogNO,'') END)
    AND ISNULL(MI.InstructionsNO,'') = (CASE WHEN @InstructionsNO <> '' THEN @InstructionsNO ELSE ISNULL(MI.InstructionsNO,'') END)
    
    ;
    
    --ALTER TABLE [#TableForSearchProduct] ALTER COLUMN [SKUCD] VARCHAR(40) NOT NULL;
    
    ALTER TABLE [#TableForSearchProduct] ADD PRIMARY KEY CLUSTERED ([AdminNO])
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    ;
    
    create NONCLUSTERED INDEX tmpTable_INDEX_1 ON #TableForSearchProduct
     (
    	SKUCD asc
     )
     
    IF ISNULL(@TagName1,'') <>'' 
    BEGIN
    	UPDATE #TableForSearchProduct
        SET Check4 = 1
        WHERE TagName1 = @TagName1
        ;
    END

    IF ISNULL(@TagName2,'') <>'' 
    BEGIN
    	UPDATE #TableForSearchProduct
        SET Check4 = 1
        WHERE TagName2 = @TagName2
        ;
    END

    IF ISNULL(@TagName3,'') <>'' 
    BEGIN
    	UPDATE #TableForSearchProduct
        SET Check4 = 1
        WHERE TagName3 = @TagName3
        ;
    END

    IF ISNULL(@TagName4,'') <>'' 
    BEGIN
    	UPDATE #TableForSearchProduct
        SET Check4 = 1
        WHERE TagName4 = @TagName4
        ;
    END

    IF ISNULL(@TagName5,'') <>'' 
    BEGIN
    	UPDATE #TableForSearchProduct
        SET Check4 = 1
        WHERE TagName5 = @TagName5
        ;
    END
    
    IF ISNULL(@TagName1,'') <>'' OR ISNULL(@TagName2,'') <>'' OR ISNULL(@TagName3,'') <>'' OR ISNULL(@TagName4,'') <>'' OR ISNULL(@TagName5,'') <>''
    BEGIN
    	DELETE FROM #TableForSearchProduct
    	WHERE Check4 = 0
    	;
    END
    
    DECLARE @VALID_DATA tinyint;
    DECLARE @INDEX int;
    DECLARE @NEXT_INDEX int;
    DECLARE @COMMENT varchar(80);
    DECLARE @FULL_COMMENT varchar(300);
    
    --���l�f�[�^�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@CommentInStore,'') <> ''
    BEGIN
	    create NONCLUSTERED INDEX tmpTable_INDEX_CommentInStore ON #TableForSearchProduct
	     (
	    	CommentInStore asc
	     )
	     
        UPDATE #TableForSearchProduct
        SET Check1 = 1
        ;
        
        SET @VALID_DATA = 1;
        SET @INDEX = 1;
        
        WHILE @VALID_DATA = 1
        BEGIN
            IF CHARINDEX(',', @CommentInStore, @INDEX) = 0
            BEGIN
                IF LEN(@CommentInStore)-@INDEX >= 0
                BEGIN
                    --�f�[�^����݂̂̏ꍇ
                    UPDATE #TableForSearchProduct
                    SET Check1 = 0
                    WHERE CommentInStore LIKE '%' + SUBSTRING(@CommentInStore,@INDEX,LEN(@CommentInStore)-@INDEX+1) + '%'
                    ;
                    
                    IF @OrOrAnd = 1		--And�̏ꍇ
                    BEGIN
                        DELETE FROM #TableForSearchProduct
                        WHERE  Check1 = 1
                        ;
                    END
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @CommentInStore, @INDEX);
            	
                UPDATE #TableForSearchProduct
                SET Check1 = 0
                WHERE CommentInStore LIKE '%' + SUBSTRING(@CommentInStore,@INDEX,@NEXT_INDEX-@INDEX) + '%'
                ;

                IF @OrOrAnd = 1		--And�̏ꍇ
                BEGIN
                    DELETE FROM #TableForSearchProduct
                    WHERE  Check1 = 1
                    ;
                    
                    UPDATE #TableForSearchProduct
                    SET Check1 = 1
			        ;
                END
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchProduct
        WHERE  Check1 = 1
        ;
    END;

    create NONCLUSTERED INDEX tmpTable_INDEX_MakerItem ON #TableForSearchProduct
     (
    	MakerItem asc
     )     
    --���[�J�[���iCD�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@MakerItem,'') <> ''
    BEGIN

        UPDATE #TableForSearchProduct
        SET Check2 = 1
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
                    UPDATE #TableForSearchProduct
                    SET Check2 = 0
                    WHERE MakerItem LIKE '%' + SUBSTRING(@MakerItem,@INDEX,LEN(@MakerItem)-@INDEX+1) + '%'
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @MakerItem, @INDEX);
            	
                UPDATE #TableForSearchProduct
                SET Check2 = 0
                WHERE MakerItem LIKE '%' + SUBSTRING(@MakerItem,@INDEX,@NEXT_INDEX-@INDEX) + '%'
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchProduct
        WHERE  Check2 = 1
        ;
    END;

    create NONCLUSTERED INDEX tmpTable_INDEX_ITemCD ON #TableForSearchProduct
     (
        ITemCD asc
     ) 
     
    --ITEM�������ɍ���Ȃ��f�[�^���e�[�u������폜
    IF ISNULL(@ITemCD,'') <> ''
    BEGIN

	     
        UPDATE #TableForSearchProduct
        SET Check3 = 1
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
                    UPDATE #TableForSearchProduct
                    SET Check3 = 0
                    WHERE ITemCD LIKE '%' + SUBSTRING(@ITemCD,@INDEX,LEN(@ITemCD)-@INDEX+1) + '%'
                    ;
                    
                    BREAK;
                END
            END
            ELSE
            BEGIN
            	SET @NEXT_INDEX = CHARINDEX(',', @ITemCD, @INDEX);
            	
                UPDATE #TableForSearchProduct
                SET Check3 = 0
                WHERE ITemCD LIKE '%' + SUBSTRING(@ITemCD,@INDEX,@NEXT_INDEX-@INDEX) + '%'
                ;
                
	            SET @INDEX = @NEXT_INDEX + 1;
            END
        END
        
        DELETE FROM #TableForSearchProduct
        WHERE  Check3 = 1
        ;
    END;
    
	--�֘A�����̏ꍇ
    IF @SearchFlg = 1
    BEGIN
        IF @ItemOrMaker = 0
        BEGIN
            -- Insert statements for procedure here
            SELECT MS.AdminNO
                  ,MS.SKUCD
                  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
                  ,MS.VariousFLG
                  ,MS.SKUName
                  ,MS.SKUShortName
                  ,MS.ITemCD
                  ,MS.ColorNO
                  ,MS.SizeNO
                  ,MS.JanCD
                  ,(CASE MS.SetKBN WHEN 1 THEN '�Z' ELSE '' END) AS SetKBN
                  ,MS.PresentKBN
                  ,MS.SampleKBN
                  ,MS.DiscountKBN
                  ,MS.ColorName
                  ,MS.SizeName
                  ,MS.MainVendorCD
                  ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
                  ,MS.MakerVendorCD
                  ,MS.BrandCD
                  ,(SELECT A.BrandName FROM M_Brand A WHERE A.BrandCD = MS.BrandCD) AS BrandName
                  ,MS.MakerItem
                  ,MS.TaniCD
                  ,MS.SportsCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '202' AND A.[Key] = MS.SportsCD) AS SportsName
                  ,W.TagName1
                  ,W.TagName2
                  ,W.TagName3
                  ,W.TagName4
                  ,W.TagName5
          
                  ,MS.ZaikoKBN
                  ,MS.Rack
                  ,MS.ReserveCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '311' AND A.[Key] = MS.ReserveCD) AS ReserveName
                  ,MS.NoticesCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '310' AND A.[Key] = MS.NoticesCD) AS NoticesName
                  ,MS.PostageCD
                 ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '309' AND A.[Key] = MS.PostageCD) AS PostageName

                  ,MS.TaxRateFLG
                  ,MS.PriceWithTax
                  ,MS.PriceOutTax
                  ,MS.OrderPriceWithTax
                  ,MS.OrderPriceWithoutTax
                  ,MS.CommentInStore
                  ,MS.CommentOutStore
                  ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
                  ,MS.DeleteFlg
                  ,MS.UsedFlg
                  ,MS.InsertOperator
                  ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
                  ,MS.UpdateOperator
                  ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
                
                  ,MI.YearTerm
                  ,MI.Season
                  ,MI.CatalogNO
                  ,MI.InstructionsNO
            
            FROM F_SKU(@ChangeDate) MS
            LEFT OUTER JOIN
                (SELECT W.AdminNo
                ,W.ChangeDate
                ,MAX(CASE WHEN RowNum=1 THEN  W.[TagName] ELSE NULL END) AS TagName1
                ,MAX(CASE WHEN RowNum=2 THEN  W.[TagName] ELSE NULL END) AS TagName2
                ,MAX(CASE WHEN RowNum=3 THEN  W.[TagName] ELSE NULL END) AS TagName3
                ,MAX(CASE WHEN RowNum=4 THEN  W.[TagName] ELSE NULL END) AS TagName4
                ,MAX(CASE WHEN RowNum=5 THEN  W.[TagName] ELSE NULL END) AS TagName5
                FROM (
                    SELECT [AdminNO]
                          ,[ChangeDate]
                          ,T.[SEQ]
                          ,T.[TagName]
                          ,ROW_NUMBER() OVER(PARTITION BY T.[AdminNO], T.[ChangeDate] ORDER BY T.SEQ) AS RowNum
                      FROM [dbo].[M_SKUTag] T
                    )AS W  
                GROUP BY W.AdminNo, W.ChangeDate
            ) AS W ON W.AdminNo = MS.AdminNo AND W.ChangeDate = MS.ChangeDate
            LEFT OUTER JOIN M_SKUInfo MI ON MI.AdminNO = MS.AdminNO AND MI.ChangeDate = MS.ChangeDate
            --INNER JOIN(SELECT M.AdminNO, MAX(M.ChangeDate) AS ChangeDate
            --    FROM M_SKU M
            --    WHERE M.ChangeDate <= CONVERT(DATE, @ChangeDate)
            --    GROUP BY M.AdminNO
            --)AS M ON M.AdminNO = MS.AdminNO AND M.ChangeDate = MS.ChangeDate
            
            WHERE EXISTS (SELECT MT.ItemCD
                from #TableForSearchProduct AS MT
                WHERE MS.ItemCD = MT.ItemCD
            )
            AND MS.DeleteFlg = 0
            ORDER BY MS.SKUCD
            ;
        END
        ELSE
        BEGIN
            -- Insert statements for procedure here
            SELECT MS.AdminNO
                  ,MS.SKUCD
                  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
                  ,MS.VariousFLG
                  ,MS.SKUName
                  ,MS.SKUShortName
                  ,MS.ITemCD
                  ,MS.ColorNO
                  ,MS.SizeNO
                  ,MS.JanCD
                  ,(CASE MS.SetKBN WHEN 1 THEN '�Z' ELSE '' END) AS SetKBN
                  ,MS.PresentKBN
                  ,MS.SampleKBN
                  ,MS.DiscountKBN
                  ,MS.ColorName
                  ,MS.SizeName
                  ,MS.MainVendorCD
                  ,(SELECT top 1 A.VendorName FROM M_Vendor A WHERE A.VendorCD = MS.MainVendorCD AND A.ChangeDate <= MS.ChangeDate) AS VendorName
                  ,MS.MakerVendorCD
                  ,MS.BrandCD
                  ,(SELECT A.BrandName FROM M_Brand A WHERE A.BrandCD = MS.BrandCD) AS BrandName
                  ,MS.MakerItem
                  ,MS.TaniCD
                  ,MS.SportsCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '202' AND A.[Key] = MS.SportsCD) AS SportsName
                  ,W.TagName1
                  ,W.TagName2
                  ,W.TagName3
                  ,W.TagName4
                  ,W.TagName5
                  
                  ,MS.ZaikoKBN
                  ,MS.Rack
                  ,MS.ReserveCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '311' AND A.[Key] = MS.ReserveCD) AS ReserveName
                  ,MS.NoticesCD
                  ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '310' AND A.[Key] = MS.NoticesCD) AS NoticesName
                  ,MS.PostageCD
                 ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID = '309' AND A.[Key] = MS.PostageCD) AS PostageName

                  ,MS.TaxRateFLG
                  ,MS.PriceWithTax
                  ,MS.PriceOutTax
                  ,MS.OrderPriceWithTax
                  ,MS.OrderPriceWithoutTax
                  ,MS.CommentInStore
                  ,MS.CommentOutStore
                  ,CONVERT(varchar,MS.ApprovalDate,111) AS ApprovalDate
                  ,MS.DeleteFlg
                  ,MS.UsedFlg
                  ,MS.InsertOperator
                  ,CONVERT(varchar,MS.InsertDateTime,111) AS InsertDateTime
                  ,MS.UpdateOperator
                  ,CONVERT(varchar,MS.UpdateDateTime,111) AS UpdateDateTime
                
                  ,MI.YearTerm
                  ,MI.Season
                  ,MI.CatalogNO
                  ,MI.InstructionsNO
            
            FROM F_SKU(@ChangeDate) MS
            LEFT OUTER JOIN
            (SELECT W.AdminNo
                ,W.ChangeDate
                ,MAX(CASE WHEN RowNum=1 THEN  W.[TagName] ELSE NULL END) AS TagName1
                ,MAX(CASE WHEN RowNum=2 THEN  W.[TagName] ELSE NULL END) AS TagName2
                ,MAX(CASE WHEN RowNum=3 THEN  W.[TagName] ELSE NULL END) AS TagName3
                ,MAX(CASE WHEN RowNum=4 THEN  W.[TagName] ELSE NULL END) AS TagName4
                ,MAX(CASE WHEN RowNum=5 THEN  W.[TagName] ELSE NULL END) AS TagName5
                FROM (
                    SELECT [AdminNO]
                          ,[ChangeDate]
                          ,T.[SEQ]
                          ,T.[TagName]
                          ,ROW_NUMBER() OVER(PARTITION BY T.[AdminNO], T.[ChangeDate] ORDER BY T.SEQ) AS RowNum
                      FROM [dbo].[M_SKUTag] T
                    )AS W  
                GROUP BY W.AdminNo, W.ChangeDate
            ) AS W ON W.AdminNo = MS.AdminNo AND W.ChangeDate = MS.ChangeDate
            LEFT OUTER JOIN M_SKUInfo MI ON MI.AdminNO = MS.AdminNO AND MI.ChangeDate = MS.ChangeDate
           -- INNER JOIN(SELECT M.AdminNO, MAX(M.ChangeDate) AS ChangeDate
           --     FROM M_SKU M
           --     WHERE M.ChangeDate <= CONVERT(DATE, @ChangeDate)
           --     GROUP BY M.AdminNO
           -- )AS M ON M.AdminNO = MS.AdminNO AND M.ChangeDate = MS.ChangeDate
            
            WHERE EXISTS(SELECT MT.MakerItem
                from #TableForSearchProduct AS MT
                WHERE MS.MakerItem = MT.MakerItem
            )
            AND MS.DeleteFlg = 0
            ORDER BY MS.SKUCD
            ;
        END

    END
    ELSE
    BEGIN
        SELECT top 1001 MS.*
        FROM #TableForSearchProduct AS MS
        ORDER BY MS.SKUCD
        ;
    END
END

