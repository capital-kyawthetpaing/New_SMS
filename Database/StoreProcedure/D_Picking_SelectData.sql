 BEGIN TRY 
 Drop Procedure dbo.[D_Picking_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Picking_SelectData]    */
CREATE PROCEDURE [dbo].[D_Picking_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @PickingNO  varchar(11),
    @ShippingPlanDateFrom  varchar(10),
    @ShippingPlanDateTo  varchar(10),
    @JuchuuNO  varchar(11),
    @JanCD varchar(300), 		--カンマ区切り
    @SKUCD varchar(300)			--カンマ区切り
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DH.PickingNO
          ,CONVERT(varchar,DH.PrintDateTime,111) AS PrintDateTime
          ,DH.PickingKBN
          ,DH.SoukoCD
          ,DH.PrintStaffCD
          ,DH.InsertOperator
          ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
          ,DH.UpdateOperator
          ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
          ,DH.DeleteOperator
          ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
          
          ,DM.PickingRows
          ,DM.ReserveNO
          ,CONVERT(varchar,DM.ShippingPlanDate,111) AS ShippingPlanDate
          ,CONVERT(varchar,DM.PickingDate,111) AS PickingDate
          ,DM.PickingDoneDateTime
          ,DM.StaffCD
          
          ,DR.Number
          ,DR.AdminNO
          ,DR.SKUCD
          ,DR.JanCD
          ,DR.ShippingPossibleSu

          ,(SELECT top 1 M.SKUName 
          FROM M_SKU AS M 
          WHERE M.ChangeDate <= DR.ShippingPlanDate
           AND M.AdminNO = DR.AdminNO
            AND M.DeleteFlg = 0
           ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
          FROM M_SKU AS M 
          WHERE M.ChangeDate <= DR.ShippingPlanDate
           AND M.AdminNO = DR.AdminNO
            AND M.DeleteFlg = 0
           ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
          FROM M_SKU AS M 
          WHERE M.ChangeDate <= DR.ShippingPlanDate
           AND M.AdminNO = DR.AdminNO
            AND M.DeleteFlg = 0
           ORDER BY M.ChangeDate desc) AS SizeName
           
           ,DD.DeliveryName

    from D_Picking AS DH
    INNER JOIN D_PickingDetails AS DM
    ON DH.PickingNO = DM.PickingNO
    AND DM.DeleteDateTime IS NULL
    
    LEFT OUTER JOIN D_Reserve AS DR
    ON DR.ReserveNO = DM.ReserveNO
    AND DR.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_DeliveryPlan AS DD
    ON DD.Number = DR.Number
	
    WHERE DH.PickingNO = (CASE WHEN @PickingNO <> '' THEN @PickingNO ELSE DH.PickingNO END)
    AND DM.ShippingPlanDate >= (CASE WHEN @ShippingPlanDateFrom <> '' THEN CONVERT(DATE, @ShippingPlanDateFrom) ELSE DM.ShippingPlanDate END)
    AND DM.ShippingPlanDate <= (CASE WHEN @ShippingPlanDateTo <> '' THEN CONVERT(DATE, @ShippingPlanDateTo) ELSE DM.ShippingPlanDate END)
    AND ISNULL(DR.Number,'') = (CASE WHEN @JuchuuNO <> '' THEN @JuchuuNO ELSE ISNULL(DR.Number,'') END)
    AND CHARINDEX(ISNULL(DR.JanCD,''), (CASE WHEN @JanCD <> '' THEN @JanCD ELSE ISNULL(DR.JanCD,'') END)) > 0 
    AND CHARINDEX(ISNULL(DR.SKUCD,''), (CASE WHEN @SKUCD <> '' THEN @SKUCD ELSE ISNULL(DR.SKUCD,'') END)) > 0 
    ORDER BY DM.PickingNO, DM.PickingRows
    ;

END


