IF OBJECT_ID ( 'D_MarkDown_SelectData', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_MarkDown_SelectData]
GO

--  ======================================================================
--       Program Call    マークダウン入力
--       Program ID      MarkDownNyuuryoku
--       Create date:    2020.06.20
--    ======================================================================
CREATE PROCEDURE D_MarkDown_SelectData
    (@MarkDownNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 データ抽出                 --
--                                            --
--********************************************--

BEGIN

    SET NOCOUNT ON;

    SELECT DH.MarkDownNO
          ,DH.StoreCD
          ,DH.SoukoCD
          ,CONVERT(varchar,DH.MarkDownDate,111) AS MarkDownDate
          ,DH.ReplicaNO
          ,DH.StaffCD
          ,DH.VendorCD
          ,CONVERT(varchar,DH.CostingDate,111) AS CostingDate
          ,CONVERT(varchar,DH.UnitPriceDate,111) AS UnitPriceDate
          ,CONVERT(varchar,DH.ExpectedPurchaseDate,111) AS ExpectedPurchaseDate
          ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
          ,DH.Comment
          ,DH.MDPurchaseNO
          ,DH.PurchaseNO
          ,(SELECT TOP 1 PayeeCD FROM M_Vendor
            WHERE VendorCD = DH.VendorCD AND ChangeDate <= DH.PurchaseDate
            ORDER BY ChangeDate DESC) AS PayeeCD
          ,DM.MDPurchaseRows
          ,DM.SKUCD
          ,DM.AdminNO
          ,DM.JanCD
          ,DM.TaniCD
          ,DM.PriceOutTax
          ,DM.EvaluationPrice
          ,DM.StockSu
          ,DM.CalculationSu
          ,DM.Rate
          ,DM.MarkDownUnitPrice
          ,DM.MarkDownUnitPrice - DM.EvaluationPrice AS MarkDownSagakuPrice
          ,DM.MarkDownGaku
          ,DM.PurchaseRows
          ,DM.PurchaseSu
          ,DM.PurchaserUnitPrice
          ,DM.PurchaserUnitPrice - DM.EvaluationPrice AS PurchaserSagakuPrice
          ,DM.PurchaseGaku
          ,ISNULL(DPM.PurchaseTax,0) AS PurchaseTax
          ,DM.InsertOperator
          ,Format(DM.InsertDateTime, 'yyyy/MM/dd HH:mm:ss') AS InsertDateTime
      FROM D_MarkDown DH
     INNER JOIN D_MarkDownDetails DM ON DH.MarkDownNO = DM.MarkDownNO
     LEFT JOIN D_PurchaseDetails DPM ON DM.PurchaseNO = DPM.PurchaseNO
                                    AND DM.PurchaseRows = DPM.PurchaseRows
     WHERE DH.MarkDownNO = @MarkDownNO
     ORDER BY DM.MarkDownRows
     ;

END
