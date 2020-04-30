IF OBJECT_ID ( 'D_Order_SelectForEDIHacchuu', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Order_SelectForEDIHacchuu]
GO

/****** Object:  StoredProcedure [D_Order_SelectForEDIHacchuu]    */
CREATE PROCEDURE D_Order_SelectForEDIHacchuu(
    -- Add the parameters for the stored procedure here
    @OrderNo  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
        
    SELECT DH.OrderNO, DH.DeleteDateTime, DH.StoreCD
    from D_Order AS DH
    WHERE DH.OrderNO = @OrderNO       
    ;
END

GO
