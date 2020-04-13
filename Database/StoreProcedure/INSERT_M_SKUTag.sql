 BEGIN TRY 
 Drop Procedure dbo.[INSERT_M_SKUTag]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE INSERT_M_SKUTag
(
	@TagName varchar(20)
   ,@AdminNO int
   ,@ChangeDate date
)AS
BEGIN
	
    IF ISNULL(@TagName,'') <> ''
    BEGIN
        INSERT INTO [M_SKUTag]
           ([AdminNO]
           ,[ChangeDate]
           ,[SEQ]
           ,[TagName])
        SELECT @AdminNO
           ,CONVERT(date, @ChangeDate)
           ,ISNULL((SELECT MAX(A.SEQ) FROM M_SKUTag AS A
                WHERE A.AdminNO = @AdminNO
                AND A.ChangeDate = @ChangeDate
            ),0) + 1 AS SEQ
           ,@TagName AS TagName
        ;
    END
	
END

GO

