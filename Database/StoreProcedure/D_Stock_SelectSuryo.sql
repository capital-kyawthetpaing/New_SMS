BEGIN TRY 
 Drop Procedure D_Stock_SelectSuryo
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [D_Stock_SelectSuryo]    */
CREATE PROCEDURE D_Stock_SelectSuryo(
    -- Add the parameters for the stored procedure here
    @SoukoCD  varchar(6),
    @RackNO  varchar(11),
    @AdminNO int,
    @Suryo int
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT SUM(DS.StockSu) AS StockSu
          ,SUM(DS.AllowableSu) AS AllowableSu
    from D_Stock DS
    
    WHERE DS.SoukoCD = @SoukoCD
    AND DS.RackNO = @RackNO	--’I”Ô‚ÉÝŒÉ‚ª‘«‚è‚È‚¢‚ÆƒGƒ‰[
    AND DS.AdminNO = @AdminNO
    AND DS.DeleteDateTime is null 
    AND DS.AllowableSu > 0 
    AND DS.ArrivalYetFlg = 0 
--    AND DS.StockSu >= @Suryo
--    AND DS.AllowableSu >= @Suryo
--    ORDER BY DS.StockNO
	GROUP BY DS.RackNO
    ;
END

GO
