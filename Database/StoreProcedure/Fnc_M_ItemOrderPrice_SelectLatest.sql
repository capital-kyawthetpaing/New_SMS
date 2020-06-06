IF EXISTS (select * from sys.objects where name = 'Fnc_M_ItemOrderPrice_SelectLatest')
begin
    DROP FUNCTION Fnc_M_ItemOrderPrice_SelectLatest
end
GO
  
CREATE FUNCTION Fnc_M_ItemOrderPrice_SelectLatest(    
    @ChangeDate varchar(10)    
)  
RETURNS TABLE    
AS    
/*      
***************************************************************************************************      
**  機能：M_ItemOrderPriceから最新のレコード取得  
**      
**************************************************************************************************************************     
*/      
RETURN( 
    SELECT main.VendorCD
          ,main.StoreCD
          ,main.MakerItem
          ,CONVERT(varchar,main.ChangeDate,111) AS ChangeDate    
          ,main.Rate
          ,main.PriceWithoutTax
          ,main.DeleteFlg
          ,main.UsedFlg
          ,main.InsertOperator
          ,CONVERT(varchar,main.InsertDateTime) AS InsertDateTime    
          ,main.UpdateOperator
          ,CONVERT(varchar,main.UpdateDateTime) AS UpdateDateTime    
    FROM M_ItemOrderPrice main  
    INNER JOIN (SELECT MakerItem
                      ,StoreCD
                      ,VendorCD
                      ,MAX(ChangeDate) AS ChangeDate        
                  FROM M_ItemOrderPrice     
                  WHERE ChangeDate <= CAST(@ChangeDate AS date)  
                    AND DeleteFlg = 0  
                  GROUP BY MakerItem,StoreCD,VendorCD)  AS sub    
      ON  main.MakerItem = sub.MakerItem    
      AND main.StoreCD = sub.StoreCD    
      AND main.VendorCD = sub.VendorCD    
      AND main.ChangeDate = sub.ChangeDate  
    WHERE main.DeleteFlg = 0  
  )     


