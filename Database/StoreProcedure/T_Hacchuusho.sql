IF EXISTS (select * from sys.table_types where user_type_id = Type_id(N'T_Hacchuusho'))
BEGIN
    DROP TYPE [T_Hacchuusho]
END

CREATE TYPE [T_Hacchuusho] AS TABLE(
	[OrderNO]     varchar(11) COLLATE database_default NULL
)
GO
