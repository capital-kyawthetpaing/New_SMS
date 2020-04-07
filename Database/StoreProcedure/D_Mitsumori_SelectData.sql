 BEGIN TRY 
 Drop Procedure dbo.[D_Mitsumori_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[D_Mitsumori_SelectData]
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @MitsumoriNO varchar(11)
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
            SELECT DH.MitsumoriNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.MitsumoriDate,111) AS MitsumoriDate
                  ,DH.StaffCD
                  ,DH.CustomerCD
                  ,DH.CustomerName
                  ,DH.CustomerName2
                  ,DH.AliasKBN
                  ,DH.ZipCD1
                  ,DH.ZipCD2
                  ,DH.Address1
                  ,DH.Address2
                  ,DH.Tel11
                  ,DH.Tel12
                  ,DH.Tel13
                  ,DH.Tel21
                  ,DH.Tel22
                  ,DH.Tel23
                  ,DH.JuchuuChanceKBN
                  ,DH.MitsumoriName
                  ,DH.DeliveryDate
                  ,DH.PaymentTerms
                  ,DH.DeliveryPlace
                  ,DH.ValidityPeriod
                  ,DH.MitsumoriHontaiGaku AS SUM_MitsumoriHontaiGaku
                  ,DH.MitsumoriTax8
                  ,DH.MitsumoriTax10
                  ,DH.MitsumoriGaku AS SUM_MitsumoriGaku
                  ,DH.CostGaku AS SUM_CostGaku
                  ,DH.ProfitGaku AS SUM_ProfitGaku
                  ,DH.RemarksInStore
                  ,DH.RemarksOutStore
                  ,DH.PrintDateTime
                  ,DH.JuchuuFLG
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.MitsumoriRows
                  ,DM.DisplayRows
                  ,DM.AdminNO AS SKUNO
                  ,DM.SKUCD
                  ,DM.JanCD
                  ,DM.SKUName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.NotPrintFLG
                  ,DM.SetKBN
                  ,DM.MitsumoriSuu
                  ,DM.MitsumoriUnitPrice
                  ,DM.TaniCD
                  ,DM.MitsumoriGaku
                  ,DM.MitsumoriHontaiGaku
                  ,DM.MitsumoriTax
                  ,DM.MitsumoriTaxRitsu
                  ,DM.CostUnitPrice
                  ,DM.CostGaku
                  ,DM.ProfitGaku
                  ,DM.CommentInStore
                  ,DM.CommentOutStore
                  ,DM.IndividualClientName

              FROM D_Mitsumori DH
              LEFT OUTER JOIN D_MitsumoriDetails DM ON DH.MitsumoriNO = DM.MitsumoriNO
              WHERE DH.MitsumoriNO = @MitsumoriNO 
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.MitsumoriNO, DM.DisplayRows
                ;
--        END

END


