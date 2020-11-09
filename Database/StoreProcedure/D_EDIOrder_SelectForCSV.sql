

/****** Object:  StoredProcedure [dbo].[D_EDIOrder_SelectForCSV]    Script Date: 2020/11/09 10:56:44 ******/
DROP PROCEDURE [dbo].[D_EDIOrder_SelectForCSV]
GO

/****** Object:  StoredProcedure [dbo].[D_EDIOrder_SelectForCSV]    Script Date: 2020/11/09 10:56:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


--  ======================================================================
--       Program Call    EDIî≠íç
--       Program ID      D_EDIOrder_SelectForCSV
--       Create date:    2019.11.25
--    ======================================================================
CREATE PROCEDURE [dbo].[D_EDIOrder_SelectForCSV](
    @EDIOrderNo  varchar(13)
)AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DH.RecordKBN
       ,DH.DataKBN
       ,DH.CapitalCD
       ,DH.CapitalName
       ,DH.OrderCD
       ,DH.OrderName
       ,DH.SalesCD
       ,DH.SalesName
       ,DM.DestinationCD
       ,DM.DestinationName
       ,DM.OrderNO
       ,DM.OrderRows
       ,DM.OrderLines
       ,FORMAT(DM.OrderDate,'yyyy/MM/dd')
       ,DM.ArriveDate
       ,DM.OrderKBN
       ,DM.MakerItemKBN
       ,DM.MakerItem
       ,DM.SKUCD
       ,DM.SizeName
       ,DM.ColorName
       ,DM.TaniCD
       ,Format(DM.OrderUnitPrice,'###,###,##0')
       ,Format(DM.OrderPriceWithoutTax,'###,###,##0')
       ,DM.BrandName
       ,DM.SKUName
       --,DM.AdminNO
       ,DM.JanCD
       ,DM.OrderSu
       ,RIGHT(DM.OrderGroupNO,8)
       ,DM.AnswerSu
       ,DM.NextDate
       ,DM.OrderGroupRows
       ,DM.ErrorMessage
       ,DH.VendorCD              
    FROM [D_EDIOrder] DH
    INNER JOIN [D_EDIOrderDetails] DM ON DH.EDIOrderNO = DM.EDIOrderNO
    WHERE (@EDIOrderNo IS NULL AND DH.OutputDatetime IS NULL) OR (@EDIOrderNo IS NOT NULL AND DH.EDIOrderNo = @EDIOrderNo) 
    ORDER BY  DH.VendorCD
             ,DH.EDIOrderNO
             ,DM.EDIOrderRows
    ;
END

GO


