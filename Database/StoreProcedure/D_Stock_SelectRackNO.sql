/****** Object:  StoredProcedure [D_Stock_SelectRackNO]    */
CREATE PROCEDURE D_Stock_SelectRackNO(
    -- Add the parameters for the stored procedure here
    @SoukoCD  varchar(6),
    @AdminNO int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT SUM(DS.StockSu) AS StockSu
          ,SUM(DS.AllowableSu) AS AllowableSu
          ,DS.RackNO
    from D_Stock DS
    
    WHERE DS.SoukoCD = @SoukoCD
    AND DS.AdminNO = @AdminNO
    AND DS.DeleteDateTime is null 
    AND DS.AllowableSu > 0 
    AND DS.ArrivalYetFlg = 0    
	GROUP BY DS.RackNO
    ;
END

GO
