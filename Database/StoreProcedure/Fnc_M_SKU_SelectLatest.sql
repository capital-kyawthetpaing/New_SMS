IF EXISTS (select * from sys.objects where name = 'Fnc_M_SKU_SelectLatest')
begin
    DROP FUNCTION Fnc_M_SKU_SelectLatest
end
GO
  
CREATE FUNCTION Fnc_M_SKU_SelectLatest(    
    @ChangeDate varchar(10)    
)  
RETURNS TABLE    
AS    
/*      
***************************************************************************************************      
**  機能：M_SKUから最新のレコード取得  
**      
**************************************************************************************************************************     
*/      
RETURN( 
  SELECT 
       main.AdminNO
      ,main.SKUCD
      ,CONVERT(varchar,main.ChangeDate,111) AS ChangeDate
      ,main.VariousFLG
      ,main.SKUName
      ,main.KanaName
      ,main.SKUShortName
      ,main.EnglishName
      ,main.ITemCD
      ,main.ColorNO
      ,main.SizeNO
      ,main.JanCD
      ,main.SetKBN
      ,main.PresentKBN
      ,main.SampleKBN
      ,main.DiscountKBN
      ,main.ColorName
      ,main.SizeName
      ,main.WebFlg
      ,main.RealStoreFlg
      ,main.MainVendorCD
      ,main.MakerVendorCD
      ,main.BrandCD
      ,main.MakerItem
      ,main.TaniCD
      ,main.SportsCD
   --   ,main.ClassificationA
   --   ,main.ClassificationB
   --   ,main.ClassificationC
      ,main.ZaikoKBN
      ,main.Rack
      ,main.VirtualFlg
      ,main.DirectFlg
      ,main.ReserveCD
      ,main.NoticesCD
      ,main.PostageCD
      ,main.ManufactCD
      ,main.ConfirmCD
      ,main.WebStockFlg
      ,main.StopFlg
      ,main.DiscontinueFlg
      ,main.InventoryAddFlg
      ,main.MakerAddFlg
      ,main.StoreAddFlg
      ,main.NoNetOrderFlg
      ,main.EDIOrderFlg
      ,main.CatalogFlg
      ,main.ParcelFlg
      ,main.AutoOrderFlg
      ,main.TaxRateFLG
      ,main.CostingKBN
      ,main.PriceWithTax
      ,main.PriceOutTax
      ,main.OrderPriceWithTax
      ,main.OrderPriceWithoutTax
      ,main.SaleStartDate
      ,main.WebStartDate
      ,main.OrderAttentionCD
      ,main.OrderAttentionNote
      ,main.CommentInStore
      ,main.CommentOutStore
      ,main.ApprovalDate
      ,main.DeleteFlg
      ,main.UsedFlg
      ,main.InsertOperator
      ,CONVERT(varchar,main.InsertDateTime) AS InsertDateTime    
      ,main.UpdateOperator
      ,CONVERT(varchar,main.UpdateDateTime) AS UpdateDateTime    
    FROM M_SKU main  
    INNER JOIN (SELECT AdminNO
                      ,MAX(ChangeDate) AS ChangeDate        
                  FROM M_SKU     
                  WHERE ChangeDate <= CAST(@ChangeDate AS date)  
                    AND DeleteFlg = 0  
                  GROUP BY AdminNO)  AS sub    
      ON  main.AdminNO = sub.AdminNO    
      AND main.ChangeDate = sub.ChangeDate  
    WHERE main.DeleteFlg = 0  
  )     


