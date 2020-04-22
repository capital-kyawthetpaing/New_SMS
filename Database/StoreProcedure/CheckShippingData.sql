 BEGIN TRY 
 Drop Procedure dbo.[CheckShippingData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE CheckShippingData
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
    
    
    --既に出荷済み
    --以下の条件でレコードがあれば出荷済としてエラーメッセージを表示する
    SELECT @CNT = COUNT(DM.PurchaseNO)
    
    FROM D_PurchaseDetails AS DM
    INNER JOIN D_Warehousing AS DW
    ON DW.Number = DM.PurchaseNO
    AND DW.NumberRow = DM.PurchaseRows
    AND DW.WarehousingKBN = 20
    AND DW.DeleteDateTime IS NULL
    INNER JOIN D_Stock AS DS
    ON DS.StockNO = DW.StockNO
    AND DS.DeleteDateTime IS NULL
    INNER JOIN D_Reserve AS DR
    ON DR.StockNO = DS.StockNO
    AND DR.DeleteDateTime IS NULL
    INNER JOIN D_PickingDetails AS DP
    ON DP.ReserveNO = DR.ReserveNO
    AND DP.DeleteDateTime IS NULL
    INNER JOIN D_InstructionDetails AS DI
    ON DI.ReserveNO = DP.ReserveNO
    AND DI.DeleteDateTime IS NULL
    INNER JOIN D_ShippingDetails AS DSM
    ON DSM.InstructionNO = DI.InstructionNO
    AND DSM.InstructionRows = DI.InstructionRows
    AND DSM.DeleteDateTime IS NULL
    WHERE DM.PurchaseNO = @PurchaseNO
    AND DM.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E159';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    
    SELECT @ERRNO AS errno;

END

