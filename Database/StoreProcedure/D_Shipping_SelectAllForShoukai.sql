 BEGIN TRY 
 Drop Procedure dbo.[D_Shipping_SelectAllForShoukai]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[D_Shipping_SelectAllForShoukai]    Script Date: 6/11/2019 2:21:19 PM ******/
--DROP PROCEDURE [D_Shipping_SelectAllForShoukai]
--GO

/****** Object:  StoredProcedure [D_Shipping_SelectAllForShoukai]    */
CREATE PROCEDURE [dbo].[D_Shipping_SelectAllForShoukai](
    -- Add the parameters for the stored procedure here
    @ShippingNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DS.ShippingNO
    		,CONVERT(varchar,DS.InputDateTime,111) AS InputDateTime
          ,CONVERT(varchar,DS.SalesDateTime,111) +' '+SUBSTRING(CONVERT(varchar,DS.SalesDateTime,108),1,5) AS SalesDateTime
          ,W.DeliveryName
          ,W.ShippingSu
          ,(SELECT top 1 M.SKUName FROM M_SKU AS M 
            WHERE M.AdminNO = DM.AdminNO
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS SKUName

    FROM D_Shipping AS DS
    LEFT OUTER JOIN D_ShippingDetails AS DM
    ON DM.ShippingNO = DS.ShippingNO
    AND DM.DeleteDateTime IS NULL
    AND DM.ShippingRows = 1
    
    --一時ワークテーブル「D_Shipping①」(画面転送表01で「D_Shipping①」として使用)                                                                                                                                                                                                                                                              
    LEFT OUTER JOIN (
        SELECT DS.ShippingNO
            ,MAX(DI.DeliveryName) AS DeliveryName
            ,SUM(DM.ShippingSu) AS ShippingSu
        
        FROM D_Shipping AS DS
        LeFT OUTER JOIN D_Instruction AS DI
        ON DI.InstructionNO = DS.InstructionNO
        AND DI.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_ShippingDetails AS DM
        ON DM.ShippingNO = DS.ShippingNO
        AND DM.DeleteDateTime IS NULL
        
        WHERE DS.DeleteDateTime IS NULL
        AND DS.SalesDateTime IS NOT NULL
        AND DS.ShippingKBN = 1
        GROUP BY DS.ShippingNO
    ) AS W
    ON W.ShippingNO = DS.ShippingNO

    WHERE DS.DeleteDateTime IS NULL
    AND DS.SalesDateTime IS NOT NULL
    AND DS.ShippingKBN = 1
    ORDER BY DS.SalesDateTime desc, DS.InputDateTime desc ,DS.ShippingNO desc
    ;

END


