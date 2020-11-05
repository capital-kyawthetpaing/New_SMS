/****** Object:  StoredProcedure [D_Order_Select]    */
CREATE PROCEDURE D_Order_Select(
    -- Add the parameters for the stored procedure here
    @OrderNo  varchar(11),
    @OrderRows  int,
    @OrderCD  varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    IF ISNULL(@OrderRows,0) = 0
    BEGIN
        SELECT DH.OrderNO
        from D_Order AS DH
        WHERE DH.OrderNO = @OrderNO
        AND DH.OrderCD = @OrderCD
        
        AND DH.DeleteDateTime IS NULL          
        ;
    END
    ELSE
    BEGIN
        SELECT DM.OrderNO
        	,DM.JanCD
        	,DM.OrderSu
        from D_OrderDetails AS DM
        WHERE DM.OrderNO = @OrderNO
        AND DM.OrderRows = @OrderRows
        
        AND DM.DeleteDateTime IS NULL          
        ;
    END
    
END

GO
