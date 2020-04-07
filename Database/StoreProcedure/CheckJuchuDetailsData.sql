 BEGIN TRY 
 Drop Procedure dbo.[CheckJuchuDetailsData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckJuchuDetailsData]
    (@JuchuuNO varchar(11),
     @JuchuuRows int
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
    DECLARE @STATUS varchar(10);
    DECLARE @STATUS2 varchar(10);
    DECLARE @CNT int;
    DECLARE @FLG int;
    DECLARE @FLG2 int;
    
    SET @STATUS = '';
    SET @STATUS2 = '';
    SET @FLG = 0;
    SET @FLG2 = 0;

    --既に仕入済み警告
    --以下の条件でレコードがあれば発注済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.JuchuuNO)
    FROM D_PurchaseDetails D
    INNER JOIN D_ArrivalDetails C ON C.ArrivalNO = D.ArrivalNO
    INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
    INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
    WHERE A.JuchuuNO = @JuchuuNO
    AND A.JuchuuRows = @JuchuuRows
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND C.DeleteDateTime IS NULL
    AND D.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS = '仕入済';
        SET @FLG = 1;
    END;

    IF @FLG = 0
    BEGIN
        --既に入荷済み警告
        --以下の条件でレコードがあれば発注済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_ArrivalDetails C
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.JuchuuRows = @JuchuuRows
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @STATUS = '入荷済';
            SET @FLG = 1;
        END;

        IF @FLG = 0
        BEGIN
            --既に発注済み警告
            --以下の条件でレコードがあれば発注済として警告メッセージを表示する
            SELECT @CNT = COUNT(A.JuchuuNO)
            FROM D_OrderDetails A 
            WHERE A.JuchuuNO = @JuchuuNO
            AND A.JuchuuRows = @JuchuuRows
            AND A.DeleteDateTime IS NULL
            ;

            IF @CNT > 0 
            BEGIN
                SET @STATUS = '発注済';
                SET @FLG = 1;
            END;
        END;
    END;

    --既に売上済み警告
    --以下の条件でレコードがあれば売上済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.JuchuuNO)
    FROM D_SalesDetails A
    WHERE A.JuchuuNO = @JuchuuNO
    AND A.JuchuuRows = @JuchuuRows
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '売上済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;
    
    --既に出荷済み警告
    --以下の条件でレコードがあれば出荷済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_ShippingDetails A
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ShippingKBN = 1
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '出荷済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;

    --既に出荷指示済み警告
    --以下の条件でレコードがあれば出荷指示済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_InstructionDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '出荷指示済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;

    --既にピッキングリスト完了済み警告
    --以下の条件でレコードがあればピッキング済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_PickingDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND B.PickingDoneDateTime IS NOT NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = 'ピッキング済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;
    
    SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;

END


