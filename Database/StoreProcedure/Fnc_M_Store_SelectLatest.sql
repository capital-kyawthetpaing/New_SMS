
IF EXISTS (select * from sys.objects where name = 'Fnc_M_Store_SelectLatest')
begin
    DROP FUNCTION Fnc_M_Store_SelectLatest
end
GO


CREATE FUNCTION Fnc_M_Store_SelectLatest(  
    @ChangeDate varchar(10)  
)
RETURNS TABLE  
AS  
/*    
***************************************************************************************************    
**  機能：M_Storeから最新のレコード取得
**    
**************************************************************************************************************************   
*/    
RETURN(
    SELECT main.StoreCD  
        ,CONVERT(varchar,main.ChangeDate,111) AS ChangeDate  
        ,main.StoreKBN  
        ,main.StorePlaceKBN  
        ,main.StoreName  
        ,main.MallCD  
        ,main.ZipCD1  
        ,main.ZipCD2  
        ,main.Address1  
        ,main.Address2  
        ,main.MailAddress  
        ,main.TelphoneNO  
        ,main.FaxNO  
        ,main.KouzaCD  
        ,main.ApprovalStaffCD11  
        ,main.ApprovalStaffCD12  
        ,main.ApprovalStaffCD21  
        ,main.ApprovalStaffCD22  
        ,main.ApprovalStaffCD31  
        ,main.ApprovalStaffCD32  
        ,main.DeliveryDate  
        ,main.PaymentTerms  
        ,main.DeliveryPlace  
        ,main.ValidityPeriod  
        ,main.Print1  
        ,main.Print2  
        ,main.Print3  
        ,main.Print4  
        ,main.Print5  
        ,main.Print6  
        ,main.Remarks  
        ,main.DeleteFlg  
        ,main.UsedFlg  
        ,main.InsertOperator  
        ,CONVERT(varchar,main.InsertDateTime) AS InsertDateTime  
        ,main.UpdateOperator  
        ,CONVERT(varchar,main.UpdateDateTime) AS UpdateDateTime  
  
    FROM M_Store main
    INNER JOIN (SELECT StoreCD  
                      ,MAX(ChangeDate) AS ChangeDate      
                  FROM M_Store   
                  WHERE ChangeDate <= CAST(@ChangeDate AS date)
                    AND DeleteFlg = 0
                  GROUP BY StoreCD)  AS sub  
      ON  main.StoreCD = sub.StoreCD  
      AND main.ChangeDate = sub.ChangeDate
    WHERE main.DeleteFlg = 0
  )   
  