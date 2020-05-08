
IF EXISTS (select * from sys.objects where name = 'Fnc_M_Vendor_SelectLatest')
begin
    DROP FUNCTION Fnc_M_Vendor_SelectLatest
end
GO

CREATE FUNCTION Fnc_M_Vendor_SelectLatest(  
    @ChangeDate varchar(10)  
)
RETURNS TABLE  
AS  
/*    
***************************************************************************************************    
**  機能：M_Vendorから最新のレコード取得
**    
**************************************************************************************************************************   
*/    
RETURN(
      SELECT
       main.VendorCD
      ,CONVERT(varchar,main.ChangeDate,111) AS ChangeDate  
      ,main.ShoguchiFlg
      ,main.VendorName
      ,main.VendorLongName1
      ,main.VendorLongName2
      ,main.VendorPostName
      ,main.VendorPositionName
      ,main.VendorStaffName
      ,main.VendorKana
      ,main.PayeeFlg
      ,main.MoneyPayeeFlg
      ,main.PayeeCD
      ,main.MoneyPayeeCD
      ,main.ZipCD1
      ,main.ZipCD2
      ,main.Address1
      ,main.Address2
      ,main.MailAddress
      ,main.FaxNO
      ,main.PaymentCloseDay
      ,main.PaymentPlanKBN
      ,main.PaymentPlanDay
      ,main.HolidayKBN
      ,main.BankCD
      ,main.BranchCD
      ,main.KouzaKBN
      ,main.KouzaNO
      ,main.KouzaMeigi
      ,main.KouzaCD
      ,main.NetFlg
      ,main.LastOrderDate
      ,main.StaffCD
      ,main.AnalyzeCD1
      ,main.AnalyzeCD2
      ,main.AnalyzeCD3
      ,main.DisplayOrder
      ,main.NotDisplyNote
      ,main.DisplayNote
      ,main.DeleteFlg
      ,main.UsedFlg
      ,main.InsertOperator
      ,CONVERT(varchar,main.InsertDateTime) AS InsertDateTime  
      ,main.UpdateOperator
      ,CONVERT(varchar,main.UpdateDateTime) AS UpdateDateTime  
    FROM M_Vendor main
    INNER JOIN (SELECT VendorCD  
                      ,MAX(ChangeDate) AS ChangeDate      
                  FROM M_Vendor   
                  WHERE ChangeDate <= CAST(@ChangeDate AS date)
                    AND DeleteFlg = 0
                  GROUP BY VendorCD)  AS sub  
      ON  main.VendorCD = sub.VendorCD  
      AND main.ChangeDate = sub.ChangeDate
    WHERE main.DeleteFlg = 0
  )   
  
