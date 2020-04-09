 BEGIN TRY 
 Drop Procedure dbo.[CheckPayPlanData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckPayPlanData]
    (@PurchaseNO varchar(11)
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
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    
    SET @ERRNO = '';
    
    --締処理済チェック D_PayPlanに、支払締番号がセットされていれば、エラー（下記のSelectができたらエラー）
    SELECT @CNT = COUNT(A.Number)
    FROM D_PayPlan A
    WHERE A.Number = @PurchaseNO
    AND A.PayPlanKBN = 1
    AND A.PayCloseNO IS NOT NULL
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E176';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    SELECT @ERRNO AS errno;

END


