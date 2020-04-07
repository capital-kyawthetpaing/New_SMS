 BEGIN TRY 
 Drop Procedure dbo.[D_Picking_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Picking_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Picking_SelectAll](
    -- Add the parameters for the stored procedure here
    @PrintDateTimeFrom  varchar(10),
    @PrintDateTimeTo  varchar(10),
    @SoukoCD  varchar(6)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DH.PickingNO
          ,CONVERT(varchar,DH.PrintDateTime,111) AS PrintDateTime
          ,(CASE WHEN Dh.PickingKBN = 2 THEN '〇' ELSE '' END) AS PickingKBN

          ,(SELECT top 1 M.SKUName 
          FROM M_SKU AS M 
          WHERE M.ChangeDate <= DR.ShippingPlanDate
           AND M.AdminNO = DR.AdminNO
            AND M.DeleteFlg = 0
           ORDER BY M.ChangeDate desc) AS SKUName
           
           ,DD.DeliveryName

    from D_Picking AS DH
    INNER JOIN D_PickingDetails AS DM
    ON DH.PickingNO = DM.PickingNO
    AND DM.DeleteDateTime IS NULL
    AND DM.PickingRows = 1
    
    LEFT OUTER JOIN D_Reserve AS DR
    ON DR.ReserveNO = DM.ReserveNO
    AND DR.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_DeliveryPlan AS DD
    ON DD.Number = DR.Number
	
    WHERE DH.PrintDateTime >= (CASE WHEN @PrintDateTimeFrom <> '' THEN CONVERT(DATE, @PrintDateTimeFrom) ELSE DH.PrintDateTime END)
    AND DH.PrintDateTime <= (CASE WHEN @PrintDateTimeTo <> '' THEN CONVERT(DATE, @PrintDateTimeTo) ELSE DH.PrintDateTime END)
    AND DH.SoukoCD = @SoukoCD

    AND DH.DeleteDateTime IS NULL
         
    ORDER BY DH.PickingNO
    ;

END


