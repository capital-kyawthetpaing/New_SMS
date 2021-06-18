IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_UpdatePrintDate')
begin
    DROP PROCEDURE PRC_Hacchuusho_UpdatePrintDate
end
GO

IF EXISTS (select * from sys.table_types where user_type_id = Type_id(N'T_Hacchuusho'))
BEGIN
    DROP TYPE [T_Hacchuusho]
END

CREATE TYPE [T_Hacchuusho] AS TABLE(
	[OrderNO]     varchar(11) COLLATE database_default NULL
)
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
