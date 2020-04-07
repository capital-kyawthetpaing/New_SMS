 BEGIN TRY 
 Drop Procedure dbo.[M_ItemPrice_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    販売単価マスタ
--       Program ID      MasterTouroku_HanbaiTanka
--       Create date:    2019.5.27
--    ======================================================================
CREATE PROCEDURE [dbo].[M_ItemPrice_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
--    @SyoKBN tinyint,
    @ItemFrom varchar(30),
    @ItemTo varchar(30),
    @BrandCD varchar(6),
    @ITemName varchar(40)
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
    
        --【データエリア】　商品分類：ITEMCD の場合
--        IF @SyoKBN = 1
--        BEGIN
            IF @OperateMode = 1		--新規時
            BEGIN
                SELECT ROW_NUMBER() OVER(PARTITION BY M.ITemCD ORDER BY M.ChangeDate desc) AS Row
                      ,M.ITemCD
                      ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
                      
                      ,ISNULL((SELECT top 1 MI.PriceWithoutTax FROM M_ItemOrderPrice MI    --ITEM発注単価マスタ
                        WHERE MI.MakerItem = M.MakerItem
                        AND MI.VendorCD = M.MainVendorCD
                        AND MI.ChangeDate <= M.ChangeDate
                        ORDER BY MI.ChangeDate desc)     --税抜単価
                          ,(SELECT top 1 MI.PriceWithoutTax FROM M_ItemOrderPrice MI    --ITEM発注単価マスタ
                            WHERE MI.MakerItem = M.MakerItem
                            AND MI.ChangeDate <= M.ChangeDate
                            ORDER BY MI.ChangeDate desc)) AS GenkaWithoutTax --原価(税抜)=税抜単価（仕入先条件なし）
                        
                      ,M.ITemName
                      ,M.PriceWithTax
                      --,M.PriceWithoutTax
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
                        
                  FROM M_Item AS M     --ITEMマスタ

                    WHERE M.ITemCD >= ISNULL(@ItemFrom, '')
                    AND  M.ITemCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
                    AND M.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE M.BrandCD END
                    AND M.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE M.ITemName END + '%'
                     --AND M.  = 0--架空商品
                    AND M.PresentKBN = 0--プレゼント品区分
                    AND M.SampleKBN = 0--サンプル品区分
                    AND M.DeleteFlg = 0 
                    AND NOT EXISTS(SELECT 1 FROM M_ItemPrice A WHERE A.ITemCD = M.ITemCD AND A.DeleteFlg = 0)
                ORDER BY ITemCD, ChangeDate desc
                ;
            END
            
            ELSE IF @OperateMode = 2	--修正時
            BEGIN
                SELECT ROW_NUMBER() OVER(PARTITION BY M.ITemCD ORDER BY M.ChangeDate desc) AS Row
                      ,M.ITemCD
                      ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
                      
                      ,ISNULL((SELECT top 1 MI.PriceWithoutTax FROM M_ItemOrderPrice MI    --ITEM発注単価マスタ
                        WHERE MI.MakerItem = M.MakerItem
                        AND MI.VendorCD = M.MainVendorCD
                        AND MI.ChangeDate <= M.ChangeDate
                        ORDER BY MI.ChangeDate desc)     --税抜単価
                          ,(SELECT top 1 MI.PriceWithoutTax FROM M_ItemOrderPrice MI    --ITEM発注単価マスタ
                            WHERE MI.MakerItem = M.MakerItem
                            AND MI.ChangeDate <= M.ChangeDate
                            ORDER BY MI.ChangeDate desc)) AS GenkaWithoutTax --原価(税抜)=税抜単価（仕入先条件なし）
                        
                      ,M.ITemName
                      ,M.PriceWithTax
                      --,M.PriceWithoutTax
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
                        
                  FROM M_Item AS M     --ITEMマスタ

                    WHERE M.ITemCD >= ISNULL(@ItemFrom, '')
                    AND  M.ITemCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
                    AND M.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE M.BrandCD END
                    AND M.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE M.ITemName END + '%'
                     --AND M.  = 0--架空商品
                    AND M.PresentKBN = 0--プレゼント品区分
                    AND M.SampleKBN = 0--サンプル品区分
                    AND M.DeleteFlg = 0 
                    AND NOT EXISTS(SELECT 1 FROM M_ItemPrice A WHERE A.ITemCD = M.ITemCD AND A.DeleteFlg = 0)
                    
                UNION ALL
                SELECT ROW_NUMBER() OVER(PARTITION BY M.ITemCD ORDER BY M.ChangeDate desc) AS Row
                      ,M.ITemCD
                      ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
                      
                    , ISNULL((SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.MakerItem, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_Item MI
                        WHERE MI.ITemCD = M.ITemCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.ITemName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_ItemOrderPrice B
                      ON B.MakerItem = A.MakerItem
                      AND B.VendorCD = A.MainVendorCD
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ),
                      (SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.MakerItem, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_Item MI
                        WHERE MI.ITemCD = M.ITemCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.ITemName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_ItemOrderPrice B
                      ON B.MakerItem = A.MakerItem
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ) ) AS GenkaWithoutTax		--原価(税抜)=税抜単価（仕入先条件なし）
                      
                      ,(SELECT top 1 MI.ItemName FROM M_Item MI
                      WHERE MI.ITemCD = M.ITemCD
                      AND MI.ChangeDate <= M.ChangeDate
                      AND MI.DeleteFlg = 0
                      ORDER BY MI.ChangeDate desc) AS ItemName

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
                      ,(SELECT top 1 MI.TaxRateFLG FROM M_Item MI
	                      WHERE MI.ITemCD = M.ITemCD
	                      AND MI.ChangeDate <= M.ChangeDate
	                      AND MI.DeleteFlg = 0
	                      ORDER BY MI.ChangeDate desc) AS TaxRateFLG
                      
                  FROM M_ItemPrice AS M     --ITEM販売単価マスタ

                    WHERE M.ITemCD >= ISNULL(@ItemFrom, '')
                    AND  M.ITemCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
                    AND EXISTS (SELECT * FROM M_Item I 
                        WHERE I.ITemCD = M.ITemCD
                        AND I.ChangeDate <= M.ChangeDate
                        AND I.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE I.BrandCD END
                        AND I.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE I.ITemName END + '%'
                        --AND I.  = 0--架空商品
                        AND I.PresentKBN = 0--プレゼント品区分
                        AND I.SampleKBN = 0--サンプル品区分
                        AND I.DeleteFlg = 0
                        ) 
                ORDER BY ITemCD, ChangeDate desc
                ;
            END
            
            ELSE    --削除・照会
            BEGIN
                SELECT ROW_NUMBER() OVER(PARTITION BY M.ITemCD ORDER BY M.ChangeDate desc) AS Row
                      ,M.ITemCD
                      ,CONVERT(varchar,M.ChangeDate,111) AS ChangeDate
                      
                    , ISNULL((SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.MakerItem, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_Item MI
                        WHERE MI.ITemCD = M.ITemCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.ITemName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_ItemOrderPrice B
                      ON B.MakerItem = A.MakerItem
                      AND B.VendorCD = A.MainVendorCD
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ),
                      (SELECT top 1 B.PriceWithoutTax 
                    FROM (SELECT top 1 MI.MakerItem, MI.MainVendorCD, MI.ChangeDate 
                        FROM M_Item MI
                        WHERE MI.ITemCD = M.ITemCD
                        AND MI.ChangeDate <= M.ChangeDate
                        AND MI.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE MI.BrandCD END
                        AND MI.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE MI.ITemName END + '%'
                        --AND MI.  = 0--架空商品
                        AND MI.PresentKBN = 0--プレゼント品区分
                        AND MI.SampleKBN = 0--サンプル品区分
                        AND MI.DeleteFlg = 0
                        ORDER BY MI.ChangeDate desc) AS A
                      LEFT OUTER JOIN M_ItemOrderPrice B
                      ON B.MakerItem = A.MakerItem
                      AND B.ChangeDate <= A.ChangeDate
                      ORDER BY B.ChangeDate desc
                      ) ) AS GenkaWithoutTax		--原価(税抜)=税抜単価（仕入先条件なし）
                      
                      ,(SELECT top 1 MI.ItemName FROM M_Item MI
                      WHERE MI.ITemCD = M.ITemCD
                      AND MI.ChangeDate <= M.ChangeDate
                      AND MI.DeleteFlg = 0
                      ORDER BY MI.ChangeDate desc) AS ItemName

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
                      ,(SELECT top 1 MI.TaxRateFLG FROM M_Item MI
	                      WHERE MI.ITemCD = M.ITemCD
	                      AND MI.ChangeDate <= M.ChangeDate
	                      AND MI.DeleteFlg = 0
	                      ORDER BY MI.ChangeDate desc) AS TaxRateFLG
                      
                  FROM M_ItemPrice AS M     --ITEM販売単価マスタ

                    WHERE M.ITemCD >= ISNULL(@ItemFrom, '')
                    AND  M.ITemCD <= ISNULL(@ItemTo, REPLICATE('Z',30))
                    AND EXISTS (SELECT * FROM M_Item I 
                        WHERE I.ITemCD = M.ITemCD
                        AND I.ChangeDate <= M.ChangeDate
                        AND I.BrandCD = CASE WHEN @BrandCD <> '' THEN @BrandCD ELSE I.BrandCD END
                        AND I.ITemName LIKE '%' + CASE WHEN @ITemName <> '' THEN @ITemName ELSE I.ITemName END + '%'
                        --AND I.  = 0--架空商品
                        AND I.PresentKBN = 0--プレゼント品区分
                        AND I.SampleKBN = 0--サンプル品区分
                        AND I.DeleteFlg = 0
                        )
            ORDER BY M.ITemCD, M.ChangeDate desc
            ;
            
            END
        
--        END

END


