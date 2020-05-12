--  ======================================================================  
--       Program Call    î≠íçèë  
--       Program ID      IkkatuHacchuuNyuuryoku
--       Create date:    2020.01.09
--    ======================================================================  
  
IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_D_Order_SelectByKey')
begin
    DROP PROCEDURE PRC_Hacchuusho_D_Order_SelectByKey
end
GO

CREATE PROCEDURE PRC_Hacchuusho_D_Order_SelectByKey(
 @p_OrderNO              varchar(11)
)
AS
BEGIN

    SELECT *
    FROM D_Order
    WHERE OrderNO = @p_OrderNO

END
