 BEGIN TRY 
 Drop Procedure dbo.[D_DeliveryPlan_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    出荷指示登録（第２画面）データ抽出
--       Program ID      ShukkaShijiTouroku
--       Create date:    2020.11.23
--    ======================================================================
CREATE PROCEDURE [dbo].[D_DeliveryPlan_SelectData]
    (    @DeliveryPlanNO   varchar(11)
    )AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

	--画面項目転送表03　第2画面
    -- Insert statements for procedure here
    SELECT DH.DeliveryPlanNO
          ,DH.DeliveryKBN
          ,DH.Number
          ,DH.DeliveryName
          ,DH.DeliverySoukoCD
          ,DH.DeliveryZip1CD
          ,DH.DeliveryZip2CD
          ,DH.DeliveryAddress1
          ,DH.DeliveryAddress2
          ,DH.DeliveryMailAddress
          ,DH.DeliveryTelphoneNO
          ,DH.DeliveryFaxNO
          ,CONVERT(varchar,DH.DecidedDeliveryDate,111) + ' ' + ISNULL(DH.DecidedDeliveryTime,'') AS DecidedDeliveryDate
          ,DH.DecidedDeliveryTime
          ,DH.CarrierCD
          ,DH.PaymentMethodCD
          ,DH.CommentInStore
          ,DH.CommentOutStore
          ,DH.InvoiceNO
          ,CONVERT(varchar,DH.DeliveryPlanDate,111) AS DeliveryPlanDate
          ,DH.HikiateFLG
          ,DH.IncludeFLG
          ,DH.OntheDayFLG
          ,DH.ExpressFLG
          ,DH.InsertOperator
          ,DH.InsertDateTime
          ,DH.UpdateOperator
          ,DH.UpdateDateTime
          ,DM.Number AS DM_Number
          ,DR.SKUCD
          ,DR.JanCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.DecidedDeliveryDate
             AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.DecidedDeliveryDate
             AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.DecidedDeliveryDate
             AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
          ,(CASE DH.DeliveryKBN WHEN 1 THEN DJ.JuchuuSuu ELSE DMM.MoveSu END) AS JuchuuSuu
          ,DS.RackNO
          ,DR.ShippingPossibleSu

      FROM D_DeliveryPlan AS DH
      INNER JOIN D_DeliveryPlanDetails AS DM
      ON DH.DeliveryPlanNO = DM.DeliveryPlanNO
      --AND DM.DeleteDateTime IS Null
      AND DM.UpdateCancelKBN <> 9
      INNER JOIN D_Reserve AS DR
      ON DR.Number = DM.Number
      AND DR.NumberRows = DM.NumberRows
      AND DR.DeleteDateTime IS Null
      LEFT OUTER JOIN D_Stock AS DS
      ON DS.StockNO = DR.StockNO
      AND DS.DeleteDateTime IS Null
      LEFT OUTER JOIN D_JuchuuDetails AS DJ
      ON DJ.JuchuuNO = DR.Number
      AND DJ.JuchuuRows = DR.NumberRows
      AND DJ.DeleteDateTime IS Null
      LEFT OUTER JOIN D_MoveDetails AS DMM
      ON DMM.MoveNO = DR.Number
      AND DMM.MoveRows = DR.NumberRows
      AND DMM.DeleteDateTime IS Null

      WHERE DH.DeliveryPlanNO = @DeliveryPlanNO               
      --AND DH.DeleteDateTime IS Null
      ORDER BY DM.Number, DM.NumberRows, DR.ReserveNO
      ;
END


