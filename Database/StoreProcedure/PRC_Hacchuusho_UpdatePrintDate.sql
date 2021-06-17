IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_UpdatePrintDate')
begin
    DROP PROCEDURE PRC_Hacchuusho_UpdatePrintDate
end
GO

CREATE PROCEDURE PRC_Hacchuusho_UpdatePrintDate(
     @p_Operator        varchar(10)
    ,@p_tblHacchusho    T_Hacchuusho READONLY
)AS
BEGIN

    DECLARE @SYSDATETIME datetime
    DECLARE @SYSDATE date
    SET @SYSDATETIME = SYSDATETIME()
    SET @SYSDATE = CONVERT(datetime, @SYSDATETIME)

    UPDATE DODH
    SET DODH.FirstPrintDate = ISNULL(DODH.FirstPrintDate, @SYSDATE)
       ,DODH.LastPrintDate = @SYSDATE
    FROM D_Order DODH
    INNER JOIN @p_tblHacchusho tbl
    ON tbl.OrderNO = DODH.OrderNO

END
GO
