BEGIN TRY 
 Drop Procedure [dbo].[CheckShukkaData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--********************************************--
--                                            --
--        出荷済み・売上済みチェック          --
--                                            --
--********************************************--
CREATE PROCEDURE [dbo].[CheckShukkaData]
    (@ShippingNO varchar(11),
     @InstructionNO varchar(11)
    )AS
    

BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    
    SET @ERRNO = '';
    
    
    --売上済み
    --以下の条件でレコードがあれば売上済としてエラーメッセージを表示する
    IF @ShippingNO IS NOT NULL
    BEGIN
        SELECT @CNT = COUNT(DA.ShippingNO)
        FROM D_Shipping AS DA    
        WHERE DA.ShippingNO = @ShippingNO
          AND DA.DeleteDateTime IS NULL
          AND DA.InvoiceNO IS NOT NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E201';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
    END
    --出荷済み
    --以下の条件でレコードがあれば出荷済としてエラーメッセージを表示する
    ELSE
    BEGIN
        SELECT @CNT = COUNT(DA.ShippingNO)
        FROM D_Shipping AS DA    
        WHERE DA.InstructionNO = @InstructionNO
          AND DA.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E214';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

    END
    
    SELECT @ERRNO AS errno;

END

