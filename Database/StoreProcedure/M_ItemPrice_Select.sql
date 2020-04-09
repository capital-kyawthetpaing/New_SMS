 BEGIN TRY 
 Drop Procedure dbo.[M_ItemPrice_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_ItemPrice_Select]    */
CREATE PROCEDURE [dbo].[M_ItemPrice_Select](
    -- Add the parameters for the stored procedure here
    @ITemCD  varchar(30),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MI.ITemCD
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

    from M_ItemPrice MI
    
    WHERE MI.ITemCD = @ITemCD
    AND MI.ChangeDate = CONVERT(DATE, @ChangeDate)		--一致データのみ
    ORDER BY ChangeDate desc
    ;
END


