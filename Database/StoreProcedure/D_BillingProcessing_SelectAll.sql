 BEGIN TRY 
 Drop Procedure dbo.[D_BillingProcessing_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Sales_SelectAll]    */
CREATE PROCEDURE [dbo].[D_BillingProcessing_SelectAll](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    SELECT DB.ProcessingDateTime
            ,CONVERT(varchar,DB.BillingDate,111) AS BillingDate
            ,DB.CustomerCD
          ,(SELECT top 1 A.CustomerName
              FROM M_Customer A 
              WHERE A.CustomerCD = DB.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DB.BillingDate
              ORDER BY A.ChangeDate desc) AS CustomerName 
            ,(CASE DB.ProcessingKBN WHEN 1 THEN '請求締' WHEN 2 THEN '請求締キャンセル' ELSE '請求確定' END) AS ProcessingKBN

    FROM D_BillingProcessing AS DB
    WHERE DB.StoreCD = @StoreCD
    ORDER BY DB.ProcessingDateTime desc, DB.BillingDate desc, DB.CustomerCD asc, DB.ProcessingKBN desc
    ;

END


