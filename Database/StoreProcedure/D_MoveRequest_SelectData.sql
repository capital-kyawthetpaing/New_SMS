 BEGIN TRY 
 Drop Procedure dbo.[D_MoveRequest_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[D_MoveRequest_SelectData]
    (@RequestNO varchar(11)
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
    SELECT NULL AS MoveNO
          ,DH.StoreCD
          --,CONVERT(varchar,ISNULL(DMH.MoveDate,@SYSDATE),111) AS MoveDate
          ,NULL AS MoveDate
          ,DH.RequestNO
          ,DH.MovePurposeKBN
          ,DH.FromStoreCD
          ,DH.FromSoukoCD
          ,DH.ToStoreCD
          ,DH.ToSoukoCD
          ,DH.StaffCD
          
          ,DM.RequestRows AS MoveRows
          ,DM.SKUCD
          ,DM.AdminNO
          ,DM.JanCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
          
          ,0 AS MoveSu
          ,DM.RequestSu
          ,0 AS EvaluationPrice
          ,NULL AS FromRackNO
          ,NULL AS ToRackNO
          ,NULL AS DeliveryPlanNO
          ,NULL AS ExpectReturnDate
          ,NULL AS VendorCD
          ,DM.CommentInStore
          ,DM.RequestRows
          
          ,ISNULL(DM.AnswerKBN,0) AS AnswerKBN
          ,CONVERT(varchar,DM.ExpectedDate,111) AS ExpectedDate
          ,DM.CommentInStore AS R_CommentInStore
          
          --回答済件数
          ,SUM(CASE WHEN ISNULL(DM.AnswerKBN,0) > 0 THEN 1 ELSE 0 END) OVER() AS CNT_Ans
          --未回答件数
          ,SUM(CASE WHEN ISNULL(DM.AnswerKBN,0) = 0 THEN 1 ELSE 0 END) OVER() AS CNT_UnAns

          ,NULL AS WarehousingNO
          ,NULL AS StockNO
          ,NULL AS ReserveNO
          ,NULL AS DeliveryPlanNO
          ,0 AS DeliveryPlanRows
          
      --画面転送表15
      FROM D_MoveRequest DH
      LEFT OUTER JOIN D_MoveRequestDetailes AS DM 
      ON DH.RequestNO = DM.RequestNO 
      AND DM.DeleteDateTime IS NULL                     

      WHERE DH.RequestNO = @RequestNO           
      AND DH.DeleteDateTime IS Null
      ORDER BY DM.RequestRows
      ;
      
END


