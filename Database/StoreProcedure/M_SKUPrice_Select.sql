 BEGIN TRY 
 Drop Procedure dbo.[M_SKUPrice_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_SKUPrice_Select]    */
CREATE PROCEDURE [dbo].[M_SKUPrice_Select](
    -- Add the parameters for the stored procedure here
    @StoreCD varchar(4),
    @TankaCD varchar(13),
    @SKUCD  varchar(30),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MI.TankaCD
            ,MI.StoreCD
            ,MI.SKUCD
        	,CONVERT(varchar,MI.ChangeDate,111) AS ChangeDate
          ,MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
        ,MI.Remarks
        ,MI.DeleteFlg
        ,MI.UsedFlg
        ,MI.InsertOperator
        ,CONVERT(varchar,MI.InsertDateTime) AS InsertDateTime
        ,MI.UpdateOperator
        ,CONVERT(varchar,MI.UpdateDateTime) AS UpdateDateTime

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = @StoreCD
    AND MI.TankaCD = @TankaCD
    AND MI.SKUCD = @SKUCD
    AND MI.ChangeDate = CONVERT(DATE, @ChangeDate)		--一致データのみ
    ORDER BY ChangeDate desc
    ;
END


