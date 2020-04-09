 BEGIN TRY 
 Drop Procedure dbo.[M_SKUPrice_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[M_SKUPrice_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
--    @SyoKBN tinyint,
    @ItemFrom varchar(30),
    @ItemTo varchar(30),
    @StoreCD varchar(4),
    @TankaCD varchar(13),
    @BrandCD varchar(6),
    @ITemName varchar(80)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    
    IF @OperateMode = 1		--新規時
    BEGIN
		SELECT ROW_NUMBER() OVER(PARTITION BY M.SKUCD ORDER BY M.ChangeDate desc) AS Row
              ,M.SKUCD
              ,M.AdminNo	--2009.10.16 add
              ,M.JanCD
              ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
              
              ,(CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END) AS TankaCD
                
              ,ISNULL((SELECT top 1 MI.PriceWithoutTax FROM M_JANOrderPrice MI --SKU発注単価マスタ
                WHERE MI.JanCD = M.JanCD
                AND MI.VendorCD = M.MainVendorCD
                AND MI.ChangeDate <= M.ChangeDate
                ORDER BY MI.ChangeDate desc)     --税抜単価
              ,(SELECT top 1 MI.PriceWithoutTax FROM M_JANOrderPrice MI --SKU発注単価マスタ
                WHERE MI.JanCD = M.JanCD
                AND MI.ChangeDate <= M.ChangeDate
                ORDER BY MI.ChangeDate desc)) AS GenkaWithoutTax --税抜単価（仕入先条件なし）
                
              ,M.SKUName
              ,M.PriceWithTax
              ,M.PriceOutTax AS PriceWithoutTax

              ,0 AS GeneralRate
              ,0 AS GeneralPriceWithTax
              ,0 AS GeneralPriceOutTax
              ,0 AS MemberRate
              ,0 AS MemberPriceWithTax
              ,0 AS MemberPriceOutTax
              ,0 AS ClientRate
              ,0 AS ClientPriceWithTax
              ,0 AS ClientPriceOutTax
              ,0 AS SaleRate
              ,0 AS SalePriceWithTax
              ,0 AS SalePriceOutTax
              ,0 AS WebRate
              ,0 AS WebPriceWithTax
              ,0 AS WebPriceOutTax
              ,NULL AS Remarks
              ,NULL AS UpdateOperator
              ,M.TaxRateFLG
                
            FROM M_SKU AS M     --SKUマスタ

            WHERE M.SKUCD >= ISNULL(@ItemFrom, '')
            AND M.SKUCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
            AND M.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE M.BrandCD END
            AND M.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE M.SKUName END + '%'
            
             --AND M.  = 0--架空商品
            AND M.PresentKBN = 0--プレゼント品区分
            AND M.SampleKBN = 0--サンプル品区分
            AND M.DeleteFlg = 0 

            AND NOT EXISTS(SELECT 1 FROM M_SKUPrice A 
            WHERE A.SKUCD = M.SKUCD 
            AND A.TankaCD = CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END 
            AND A.StoreCD = CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE A.StoreCD END
            AND A.DeleteFlg = 0)
            
            ORDER BY M.SKUCD, M.ChangeDate desc
            ;
                
    END
    
    ELSE IF @OperateMode = 2        --修正時
    BEGIN
    
		SELECT ROW_NUMBER() OVER(PARTITION BY M.SKUCD ORDER BY M.ChangeDate desc) AS Row
              ,M.SKUCD
              ,M.AdminNo	--2009.10.16 add
              ,M.JanCD
              ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
              
              ,(CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END) AS TankaCD
                
              ,ISNULL((SELECT top 1 MI.PriceWithoutTax FROM M_JANOrderPrice MI --SKU発注単価マスタ
                WHERE MI.JanCD = M.JanCD
                AND MI.VendorCD = M.MainVendorCD
                AND MI.ChangeDate <= M.ChangeDate
                ORDER BY MI.ChangeDate desc)     --税抜単価
              ,(SELECT top 1 MI.PriceWithoutTax FROM M_JANOrderPrice MI --SKU発注単価マスタ
                WHERE MI.JanCD = M.JanCD
                AND MI.ChangeDate <= M.ChangeDate
                ORDER BY MI.ChangeDate desc)) AS GenkaWithoutTax --税抜単価（仕入先条件なし）
                
              ,M.SKUName
              ,M.PriceWithTax
              ,M.PriceOutTax AS PriceWithoutTax

              ,0 AS GeneralRate
              ,0 AS GeneralPriceWithTax
              ,0 AS GeneralPriceOutTax
              ,0 AS MemberRate
              ,0 AS MemberPriceWithTax
              ,0 AS MemberPriceOutTax
              ,0 AS ClientRate
              ,0 AS ClientPriceWithTax
              ,0 AS ClientPriceOutTax
              ,0 AS SaleRate
              ,0 AS SalePriceWithTax
              ,0 AS SalePriceOutTax
              ,0 AS WebRate
              ,0 AS WebPriceWithTax
              ,0 AS WebPriceOutTax
              ,NULL AS Remarks
              ,NULL AS UpdateOperator
              ,M.TaxRateFLG
                
              FROM M_SKU AS M     --SKUマスタ

                WHERE M.SKUCD >= ISNULL(@ItemFrom, '')
                AND M.SKUCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
                AND M.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE M.BrandCD END
                AND M.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE M.SKUName END + '%'
                
                 --AND M.  = 0--架空商品
                AND M.PresentKBN = 0--プレゼント品区分
                AND M.SampleKBN = 0--サンプル品区分
                AND M.DeleteFlg = 0 
                
                AND NOT EXISTS(SELECT 1 FROM M_SKUPrice A 
                WHERE A.SKUCD = M.SKUCD 
                AND A.TankaCD = CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END 
                AND A.StoreCD = CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE A.StoreCD END
                AND A.DeleteFlg = 0)
                
        UNION ALL
        SELECT ROW_NUMBER() OVER(PARTITION BY M.SKUCD ORDER BY M.ChangeDate desc) AS Row
              ,M.SKUCD
              ,M.AdminNo	--2009.10.16 add
              ,(SELECT top 1 MI.JanCD FROM M_SKU MI
              WHERE MI.SKUCD = M.SKUCD
              AND MI.ChangeDate <= M.ChangeDate
              AND MI.DeleteFlg = 0 
              ORDER BY MI.ChangeDate desc) AS JanCD
              ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
              ,M.TankaCD
            , ISNULL((SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.JanCD, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_SKU MI
                        WHERE MI.SKUCD = M.SKUCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.SKUName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_JANOrderPrice B
                      ON B.JanCD = A.JanCD
                        AND B.VendorCD = A.MainVendorCD
                        AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ),
                  (SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.JanCD, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_SKU MI
                        WHERE MI.SKUCD = M.SKUCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.SKUName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_JANOrderPrice B
                      ON B.JanCD = A.JanCD
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ) ) AS GenkaWithoutTax        --原価(税抜)=税抜単価（仕入先条件なし）
              
              ,(SELECT top 1 MI.SKUName FROM M_SKU MI
              WHERE MI.SKUCD = M.SKUCD
              AND MI.ChangeDate <= M.ChangeDate
              AND MI.DeleteFlg = 0
              ORDER BY MI.ChangeDate desc) AS SKUName

              ,M.PriceWithTax
              ,M.PriceWithoutTax
              ,M.GeneralRate
              ,M.GeneralPriceWithTax
              ,M.GeneralPriceOutTax
              ,M.MemberRate
              ,M.MemberPriceWithTax
              ,M.MemberPriceOutTax
              ,M.ClientRate
              ,M.ClientPriceWithTax
              ,M.ClientPriceOutTax
              ,M.SaleRate
              ,M.SalePriceWithTax
              ,M.SalePriceOutTax
              ,M.WebRate
              ,M.WebPriceWithTax
              ,M.WebPriceOutTax
              ,M.Remarks
              ,M.UpdateOperator
              ,(SELECT top 1 MI.TaxRateFLG FROM M_SKU MI
	              WHERE MI.SKUCD = M.SKUCD
	              AND MI.ChangeDate <= M.ChangeDate
	              AND MI.DeleteFlg = 0
	              ORDER BY MI.ChangeDate desc) AS TaxRateFLG
          FROM M_SKUPrice AS M     --SKU販売単価マスタ

            WHERE M.SKUCD >= ISNULL(@ItemFrom, '')
            AND  M.SKUCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
            AND M.TankaCD = CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END
            AND M.StoreCD = CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE M.StoreCD END
            AND EXISTS (SELECT * FROM M_SKU I 
                WHERE I.SKUCD = M.SKUCD
                AND I.ChangeDate <= M.ChangeDate
                AND I.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE I.BrandCD END
                AND I.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE I.SKUName END + '%'
                --AND I.  = 0--架空商品
                AND I.PresentKBN = 0--プレゼント品区分
                AND I.SampleKBN = 0--サンプル品区分
                AND I.DeleteFlg = 0
                )
        ORDER BY SKUCD, ChangeDate desc
        ;
                
	END
	
    ELSE    --削除・照会
    BEGIN
        SELECT ROW_NUMBER() OVER(PARTITION BY M.SKUCD ORDER BY M.ChangeDate desc) AS Row
              ,M.SKUCD
              ,M.AdminNo	--2009.10.16 add
              ,(SELECT top 1 MI.JanCD FROM M_SKU MI
              WHERE MI.SKUCD = M.SKUCD
              AND MI.ChangeDate <= M.ChangeDate
              AND MI.DeleteFlg = 0 
              ORDER BY MI.ChangeDate desc) AS JanCD
              ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
              ,M.TankaCD
            , ISNULL((SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.JanCD, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_SKU MI
                        WHERE MI.SKUCD = M.SKUCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.SKUName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_JANOrderPrice B
                      ON B.JanCD = A.JanCD
                        AND B.VendorCD = A.MainVendorCD
                        AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ),
                  (SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.JanCD, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_SKU MI
                        WHERE MI.SKUCD = M.SKUCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.SKUName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_JANOrderPrice B
                      ON B.JanCD = A.JanCD
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ) ) AS GenkaWithoutTax        --原価(税抜)=税抜単価（仕入先条件なし）
              
              ,(SELECT top 1 MI.SKUName FROM M_SKU MI
              WHERE MI.SKUCD = M.SKUCD
              AND MI.ChangeDate <= M.ChangeDate
              AND MI.DeleteFlg = 0
              ORDER BY MI.ChangeDate desc) AS SKUName

              ,M.PriceWithTax
              ,M.PriceWithoutTax
              ,M.GeneralRate
              ,M.GeneralPriceWithTax
              ,M.GeneralPriceOutTax
              ,M.MemberRate
              ,M.MemberPriceWithTax
              ,M.MemberPriceOutTax
              ,M.ClientRate
              ,M.ClientPriceWithTax
              ,M.ClientPriceOutTax
              ,M.SaleRate
              ,M.SalePriceWithTax
              ,M.SalePriceOutTax
              ,M.WebRate
              ,M.WebPriceWithTax
              ,M.WebPriceOutTax
              ,M.Remarks
              ,M.DeleteFlg
              ,M.UsedFlg
              ,M.InsertOperator
              ,CONVERT(varchar,M.InsertDateTime) AS InsertDateTime
              ,M.UpdateOperator
              ,CONVERT(varchar,M.UpdateDateTime) AS UpdateDateTime
              ,(SELECT top 1 MI.TaxRateFLG FROM M_SKU MI
	              WHERE MI.SKUCD = M.SKUCD
	              AND MI.ChangeDate <= M.ChangeDate
	              AND MI.DeleteFlg = 0
	              ORDER BY MI.ChangeDate desc) AS TaxRateFLG
          FROM M_SKUPrice AS M     --SKU販売単価マスタ

            WHERE M.SKUCD >= ISNULL(@ItemFrom, '')
            AND  M.SKUCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
            AND M.TankaCD = CASE WHEN @TankaCD <> '' THEN @TankaCD ELSE REPLICATE('0',13) END
            AND M.StoreCD = CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE M.StoreCD END
            AND EXISTS (SELECT * FROM M_SKU I 
                WHERE I.SKUCD = M.SKUCD
                AND I.ChangeDate <= M.ChangeDate
                AND I.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE I.BrandCD END
                AND I.SKUName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE I.SKUName END + '%'
                --AND I.  = 0--架空商品
                AND I.PresentKBN = 0--プレゼント品区分
                AND I.SampleKBN = 0--サンプル品区分
                AND I.DeleteFlg = 0
                )
    ORDER BY M.SKUCD, M.ChangeDate desc
    ;
    
    END

END


