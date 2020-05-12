
 BEGIN TRY 
 Drop Procedure dbo.D_EDIOrderDetails_SelectForPrint
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_EDIOrderDetails_SelectForPrint]    */
CREATE PROCEDURE D_EDIOrderDetails_SelectForPrint(
    -- Add the parameters for the stored procedure here
    @EDIOrderNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DE.EDIOrderNO
          ,DE.EDIOrderRows
          ,DE.OrderNO
          ,DE.OrderRows
          ,DE.OrderLines
          ,DE.OrderDate
          ,DE.ArriveDate
          ,DE.OrderKBN
          ,DE.MakerItemKBN
          ,DE.MakerItem
          ,DE.SKUCD
          ,DE.SizeName
          ,DE.ColorName
          ,DE.TaniCD
          ,DE.OrderUnitPrice
          ,DE.OrderPriceWithoutTax
          ,DE.BrandName
          ,DE.SKUName
          ,DE.AdminNO
          ,DE.JanCD
          ,DE.OrderSu
          ,DE.OrderGroupNO
          ,DE.AnswerSu
          ,DE.NextDate
          ,DE.OrderGroupRows
          ,DE.ErrorMessage
          ,DE.InsertOperator
          ,DE.InsertDateTime
          ,DE.UpdateOperator
          ,DE.UpdateDateTime
          ,CONVERT(varchar, DO.OrderDate, 111) AS OrderDate
          ,DM.JuchuuNO
          

    FROM D_EDIOrderDetails AS DE
    LEFT OUTER JOIN D_OrderDetails AS DM
    ON DM.OrderNO = DE.OrderNO
    AND DM.OrderRows = DE.OrderRows
    AND DM.DeleteDateTime IS NULL
    INNER JOIN D_Order AS DO
    ON DO.OrderRows = DE.OrderRows
    AND DO.DeleteDateTime IS NULL
    
    WHERE DE.EDIOrderNO = @EDIOrderNO
--    AND DE.ArriveDate IS NOT NULL

    ORDER BY DE.EDIOrderNO
    ;

END

GO
