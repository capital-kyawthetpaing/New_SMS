 BEGIN TRY 
 Drop Procedure dbo.[D_Sales_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    売上入力
--       Program ID      
--       Create date:    2019.
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Sales_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @SalesNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

        SELECT DH.SalesNO
              ,DH.StoreCD
              ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
              ,DH.ShippingNO
              ,DH.CustomerCD
              ,DH.CustomerName
              ,DH.CustomerName2
              ,DH.BillingType
              ,DH.SalesHontaiGaku
              ,DH.SalesHontaiGaku0
              ,DH.SalesHontaiGaku8
              ,DH.SalesHontaiGaku10
              ,DH.SalesTax
              ,DH.SalesTax8
              ,DH.SalesTax10
              ,DH.SalesGaku
              ,DH.LastPoint
              ,DH.WaitingPoint
              ,DH.StaffCD
              ,CONVERT(varchar,DH.PrintDate,111) AS PrintDate
              ,DH.PrintStaffCD
              
              ,DH.InsertOperator
              ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
              ,DH.UpdateOperator
              ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
              ,DH.DeleteOperator
              ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
              
              ,DM.SalesRows
              ,DM.JuchuuNO
              ,DM.JuchuuRows
              ,DM.ShippingNO
              ,DM.AdminNO
              ,DM.SKUCD
              ,DM.JanCD
              ,DM.SKUName
              ,DM.ColorName
              ,DM.SizeName
              ,DM.SalesSU
              ,DM.SalesUnitPrice
              ,DM.TaniCD
              ,DM.SalesHontaiGaku
              ,DM.SalesTax
              ,DM.SalesGaku
              ,DM.SalesTaxRitsu
              ,DM.CommentOutStore
              ,DM.CommentInStore
              ,DM.IndividualClientName
              ,DM.DeliveryNoteFLG
              ,DM.BillingPrintFLG

          FROM D_Sales DH

          LEFT OUTER JOIN D_SalesDetails AS DM ON DH.SalesNO = DM.SalesNO AND DM.DeleteDateTime IS NULL
            
          WHERE DH.SalesNO = @SalesNO 
--              AND DH.DeleteDateTime IS Null
            ORDER BY DH.SalesNO, DM.SalesRows
            ;

END


