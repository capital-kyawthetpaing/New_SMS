
IF EXISTS (select * from sys.objects where name = 'Fnc_M_SalesTax_SelectLatest')
begin
    DROP FUNCTION Fnc_M_SalesTax_SelectLatest
end
GO


CREATE FUNCTION Fnc_M_SalesTax_SelectLatest(      
    @ChangeDate varchar(10)      
)    
RETURNS TABLE      
AS      
/*        
***************************************************************************************************        
**  機能：M_SalesTaxから最新のレコード取得    
**        
**************************************************************************************************************************       
*/        
RETURN(   
    SELECT TOP 1  
           main.ChangeDate  
          ,main.TaxRate1  
          ,main.TaxRate2  
          ,main.FractionKBN  
    FROM M_SalesTax main    
    WHERE main.ChangeDate <= CAST(@ChangeDate AS date)    
    ORDER BY main.ChangeDate DESC  
  )       
  
  
  