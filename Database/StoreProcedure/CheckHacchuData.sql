 BEGIN TRY 
 Drop Procedure dbo.[CheckHacchuData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--発注番号入力時の進捗チェック。ほぼ受注入力と同じ内容
--変更時は受注入力も考慮すること
CREATE PROCEDURE [dbo].[CheckHacchuData]
    (@OrderNO varchar(11)
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
    
    
    --既に出荷済み警告
    --以下の条件でレコードがあれば出荷済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_ShippingDetails A
    WHERE A.Number = @OrderNO
    AND A.ShippingKBN = 1
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E159';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    --既に出荷指示済み警告
    --以下の条件でレコードがあれば出荷指示済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_InstructionDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @OrderNO
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E160';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    --既にピッキングリスト完了済み警告
    --以下の条件でレコードがあればピッキング済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_PickingDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @OrderNO
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND B.PickingDoneDateTime IS NOT NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E161';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --既に仕入済み警告
    --以下の条件でレコードがあれば発注済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.OrderNO)
    FROM D_PurchaseDetails D
    INNER JOIN D_ArrivalDetails C ON C.ArrivalNO = D.ArrivalNO
    INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
    INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
    WHERE A.OrderNO = @OrderNO
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND C.DeleteDateTime IS NULL
    AND D.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E164';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    --既に入荷済み警告
    --以下の条件でレコードがあれば発注済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.OrderNO)
    FROM D_ArrivalDetails C
    INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
    INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
    WHERE A.OrderNO = @OrderNO
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND C.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E163';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    SELECT @ERRNO AS errno;

END


