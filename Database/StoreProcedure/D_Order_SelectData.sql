 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    発注入力
--       Program ID      HacchuuNyuuryoku
--       Create date:    2019.8.26
--    ======================================================================
CREATE PROCEDURE [dbo].[D_Order_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @OrderNO varchar(11)
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

--        IF @OperateMode = 2   --修正時
--        BEGIN
            SELECT DH.OrderNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
                  ,DH.ReturnFLG
                  ,DH.StaffCD
                  ,DH.OrderCD
                  ,DH.OrderPerson
                  ,DH.DestinationKBN
                  ,DH.DestinationName
                  ,DH.AliasKBN
                  ,DH.DestinationZip1CD
                  ,DH.DestinationZip2CD
                  ,DH.DestinationAddress1
                  ,DH.DestinationAddress2
                  ,DH.DestinationTelphoneNO
                  ,DH.DestinationFaxNO
                  ,DH.DestinationSoukoCD
                  ,DH.ApprovalStageFLG
                  ,(CASE DH.ApprovalStageFLG WHEN 0 THEN '却下' WHEN 1 THEN '申請中' WHEN 9 THEN '承認済' ELSE '承認中' END) AS ApprovalStage
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.OrderRows
                  ,DM.DisplayRows
                  ,DM.AdminNO
                  ,DM.SKUCD
                  ,DM.JanCD
                  ,DM.ItemName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.Rate
                  ,CONVERT(varchar,DM.ArrivePlanDate,111) AS ArrivePlanDate
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore

                  ,DM.OrderSu
                  ,DM.OrderUnitPrice
                  ,DM.TaniCD
                  ,DM.OrderHontaiGaku
                  
                  ,CONVERT(varchar,DM.DesiredDeliveryDate,111) AS DesiredDeliveryDate
                  ,DM.EDIFLG
                  

              FROM D_Order DH
              LEFT OUTER JOIN D_OrderDetails AS DM ON DH.OrderNO = DM.OrderNO AND DM.DeleteDateTime IS NULL
           
              WHERE DH.OrderNO = @OrderNO 
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.OrderNO, DM.OrderRows
                ;
--        END

END


