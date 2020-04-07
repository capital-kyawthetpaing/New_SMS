 BEGIN TRY 
 Drop Procedure dbo.[CheckBillingDate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckBillingDate]
    (@StoreCD varchar(4),
     @CustomerCD varchar(13),
     @BillingDate varchar(10)
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
    
    --締処理済チェック D_BillingProcessing 下記のSelectができたらエラー）
    --その店舗、請求先でその入金日を含む期間ですでに請求処理が済んでいる
    SELECT A.ProcessingNO
    FROM D_BillingProcessing A
    WHERE A.StoreCD = @StoreCD
    AND A.CustomerCD = (SELECT top 1 M.BillingCD
                        FROM M_Customer AS M
                        WHERE M.CustomerCD = @CustomerCD
                        AND M.ChangeDate <= @BillingDate
                        ORDER BY M.ChangeDate desc)
    AND A.ProcessingKBN <> 2	--（2:締キャンセル）
    AND A.BillingDate >= @BillingDate
    AND A.DeleteDateTime IS NULL
    ;

END


