IF OBJECT_ID ( 'D_EDIOrder_Select', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_EDIOrder_Select]
GO

/****** Object:  StoredProcedure [D_EDIOrder_Select]    */
CREATE PROCEDURE D_EDIOrder_Select(
    -- Add the parameters for the stored procedure here
    @EDIOrderNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
        
    SELECT DH.EDIOrderNO
          ,DH.StoreCD
      FROM D_EDIOrder AS DH
     WHERE DH.EDIOrderNO = @EDIOrderNO       
    ;
END

GO
