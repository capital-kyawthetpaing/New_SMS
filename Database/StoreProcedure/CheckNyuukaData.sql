 BEGIN TRY 
 Drop Procedure dbo.[CheckNyuukaData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckNyuukaData]
    (@ArrivalNO varchar(11)
    )AS
    
--入荷番号入力時の進捗チェック。ほぼ発注入力と同じ内容
--変更時は発注入力も考慮すること
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
    SELECT @CNT = COUNT(DA.ArrivalNO)
    FROM D_ArrivalDetails AS DA
    INNER JOIN D_ArrivalPlan AS DP
    ON DP.ArrivalPlanNO = DA.ArrivalPlanNO
    AND DP.DeleteDateTime IS NULL
    INNER JOIN D_Stock AS DS
    ON  DS.ArrivalPlanNO = DP.ArrivalPlanNO
    AND DS.DeleteDateTime IS NULL
    INNER JOIN D_Reserve DR 
    ON DR.StockNO = DS.StockNO
    AND DR.DeleteDateTime IS NULL
    INNER JOIN D_ShippingDetails AS A
    ON A.InstructionNO = DR.ShippingOrderNO
    AND A.InstructionRows = DR.ShippingOrderRows
    AND A.DeleteDateTime IS NULL
    
    WHERE DA.ArrivalNO = @ArrivalNO
    AND DA.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E182';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --既にピッキングリスト完了済み警告
    --以下の条件でレコードがあればピッキング済として警告メッセージを表示する
    SELECT @CNT = COUNT(DA.ArrivalNO)
    FROM D_ArrivalDetails AS DA
    INNER JOIN D_ArrivalPlan AS DP
    ON DP.ArrivalPlanNO = DA.ArrivalPlanNO
    AND DP.DeleteDateTime IS NULL
    INNER JOIN D_Stock AS DS
    ON  DS.ArrivalPlanNO = DP.ArrivalPlanNO
    AND DS.DeleteDateTime IS NULL
    INNER JOIN D_Reserve DR 
    ON DR.StockNO = DS.StockNO
    AND DR.DeleteDateTime IS NULL
    INNER JOIN D_PickingDetails AS B
    ON B.ReserveNO = DR.ReserveNO
    AND B.DeleteDateTime IS NULL
    
    WHERE DA.ArrivalNO = @ArrivalNO
    AND DA.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'Q311';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --既に出荷指示済み警告
    --以下の条件でレコードがあれば出荷指示済として警告メッセージを表示する
    SELECT @CNT = COUNT(DA.ArrivalNO)
    FROM D_ArrivalDetails AS DA
    INNER JOIN D_ArrivalPlan AS DP
    ON DP.ArrivalPlanNO = DA.ArrivalPlanNO
    AND DP.DeleteDateTime IS NULL
    INNER JOIN D_Stock AS DS
    ON  DS.ArrivalPlanNO = DP.ArrivalPlanNO
    AND DS.DeleteDateTime IS NULL
    INNER JOIN D_Reserve DR 
    ON DR.StockNO = DS.StockNO
    AND DR.DeleteDateTime IS NULL
    INNER JOIN D_InstructionDetails AS B
    ON B.ReserveNO = DR.ReserveNO
    AND B.DeleteDateTime IS NULL
    
    WHERE DA.ArrivalNO = @ArrivalNO
    AND DA.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'Q312';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    SELECT @ERRNO AS errno;

END


