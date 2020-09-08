/****** Object:  StoredProcedure [D_Stock_SelectZaiko]    */
CREATE PROCEDURE D_Stock_SelectZaiko(
    -- Add the parameters for the stored procedure here
    @SoukoCD  varchar(6),
--    @RackNO  varchar(11),
    @AdminNO int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DS.StockNO
          ,DS.ArrivalPlanNO
          ,DS.SKUCD
          ,DS.AdminNO
          ,DS.JanCD
          ,DS.ArrivalYetFLG
          ,DS.ArrivalPlanKBN
          ,DS.ArrivalPlanDate
          ,DS.ArrivalDate
          ,DS.StockSu
          ,DS.PlanSu
          ,DS.AllowableSu
          ,DS.AnotherStoreAllowableSu
          ,DS.ReserveSu
          ,DS.InstructionSu
          ,DS.ShippingSu
          ,DS.OriginalStockNO
          ,DS.ExpectReturnDate
          ,DS.ReturnPlanSu
          ,DS.VendorCD
          ,DS.ReturnDate
          ,DS.ReturnSu

    from D_Stock DS
    
    WHERE DS.SoukoCD = @SoukoCD
--    AND DS.RackNO = @RackNO	�I�Ԃ͊֌W�Ȃ��ɍ݌ɂ�����ƃG���[
    AND DS.AdminNO = @AdminNO
    AND DS.StockSu > 0
    ORDER BY DS.StockNO
    ;
END

GO
