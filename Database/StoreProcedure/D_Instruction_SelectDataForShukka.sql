BEGIN TRY 
 Drop Procedure [dbo].[D_Instruction_SelectDataForShukka]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--********************************************--
--                                            --
--            出荷指示データ抽出              --
--                                            --
--********************************************--
CREATE PROCEDURE [dbo].[D_Instruction_SelectDataForShukka]
    (@InstructionNo varchar(11),
     @ShippingDate  varchar(10)    
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT (CASE WHEN DJM.UpdateCancelKBN = 1 THEN 1 ELSE 0 END) AS UpdateKBN
           ,DI.OntheDayFLG
           --,DI.DeliveryName
           ,(CASE WHEN DI.InstructionKBN = 2 THEN MS.SoukoName ELSE DI.DeliveryName END) AS DeliveryName
           ,(CASE WHEN DI.DecidedDeliveryDate IS NOT NULL AND DI.DecidedDeliveryTime IS NOT NULL THEN 1 ELSE 0 END) AS DecidedDeliveryKbn
           ,(SELECT top 1 A.CarrierCD
               FROM M_Carrier A 
              WHERE A.CarrierCD = DI.CarrierCD 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CarrierCD
           ,(CASE WHEN DI.CashOnDelivery = 1 THEN 1 ELSE 0 END) AS CashOnDelivery
           ,1 AS UnitsCount
           ,(CASE WHEN DJS.NouhinsyoFLG = 1 THEN 1 ELSE 0 END) AS NouhinsyoFLG
           ,CONVERT(varchar,DI.DecidedDeliveryDate,111) AS DecidedDeliveryDate
           ,DI.DecidedDeliveryTime
           ,DI.CommentInStore
           ,(SELECT top 1 B.Char1
               FROM M_SKU A 
               LEFT JOIN M_MultiPorpose B ON A.TaniCD = B.[Key]
                                         AND 201 = B.ID
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS TANI
           ,DIM.JanCD
           ,(SELECT top 1 A.SKUName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
           ,(SELECT top 1 A.ColorName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
           ,(SELECT top 1 A.SizeName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SizeName
           ,(SELECT top 1 A.CommentOutStore
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CommentOutStore
           ,DIM.InstructionSu
           ,0 AS ShippingSu
           ,DJM.JuchuuNO
           ,DJM.UpdateCancelKBN
           ,CASE WHEN DJ.CancelDate IS NULL THEN '0' ELSE '1' END AS CancelKbn
           ,DIM.SKUCD
           ,DIM.AdminNO
           ,DI.FromSoukoCD AS SoukoCD
           ,DI.InstructionKBN
           ,DIM.InstructionNO
           ,DIM.InstructionRows
           ,DRM.Number
           ,DRM.NumberRows
           ,DRM.ReserveNO
           ,DRM.ReserveKBN
           ,DRM.StockNO
           ,DM.ToStoreCD
           ,DM.ToSoukoCD
           ,NULL AS ToRackNO
           ,NULL AS ToStockNO
           ,DM.StoreCD AS FromStoreCD
           ,DM.FromSoukoCD AS FromSoukoCD
           ,DMM.FromRackNO
           ,DJ.CustomerCD
    FROM D_Instruction AS DI
    INNER JOIN D_InstructionDetails AS DIM ON DI.InstructionNO = DIM.InstructionNO
                                          AND DIM.InstructionKBN IN (1,2)
                                          AND DIM.DeleteDateTime IS NULL   
    LEFT OUTER JOIN D_Reserve AS DRM ON DIM.InstructionNO = DRM.ShippingOrderNO
                                    AND DIM.InstructionRows = DRM.ShippingOrderRows
                                    AND DRM.DeleteDateTime IS NULL                                                     
    LEFT OUTER JOIN D_JuchuuDetails AS DJM ON DRM.Number = DJM.JuchuuNO
                                          AND DRM.NumberRows = DJM.JuchuuRows
                                          AND DRM.ReserveKBN = 1
                                          AND DJM.DeleteDateTime IS NULL                                                          
    LEFT OUTER JOIN D_Juchuu AS DJ ON DJM.JuchuuNO = DJ.JuchuuNO
                                  AND DJ.DeleteDateTime IS NULL  
    LEFT OUTER JOIN D_JuchuuStatus AS DJS ON DJ.JuchuuNO = DJS.JuchuuNO   
    LEFT OUTER JOIN D_MoveDetails AS DMM ON DRM.[Number] = DMM.MoveNO
                                        AND DRM.NumberRows = DMM.MoveRows
                                        AND DRM.ReserveKBN = 2
                                        AND DMM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Move AS DM ON DMM.MoveNO = DM.MoveNO
                                AND DM.DeleteDateTime IS NULL     
    LEFT OUTER JOIN F_Souko(GETDATE()) AS MS ON MS.SoukoCD = DI.DeliverySoukoCD
                                            AND MS.DeleteFlg = 0     
                                            AND MS.ChangeDate <= DI.InstructionDate
    WHERE DI.DeleteDateTime IS NULL
      AND DI.InstructionNO = @InstructionNo
    ORDER BY DIM.InstructionRows
    ;

END


