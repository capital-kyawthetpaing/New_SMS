 BEGIN TRY 
 Drop Procedure dbo.[D_InventoryControl_Select]
END try
BEGIN CATCH END CATCH 

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_InventoryControl_Select]
    -- Add the parameters for the stored procedure here
    @SoukoCD varchar(13),
    @InventoryDate varchar(10),
    @TanaCDFrom varchar(13),
    @TanaCDTo varchar(13)--,
--    @InventoryKBN varchar(1)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DI.SoukoCD
        , DI.RackNO
        , DI.InventoryDate
        , DI.InventoryNO
    FROM D_InventoryControl AS DI

    WHERE DI.SoukoCD = @SoukoCD
    AND DI.InventoryDate = CONVERT(date, @InventoryDate)
    AND DI.InventoryKBN = 1
    AND DI.RackNO >= (CASE WHEN @TanaCDFrom <> '' THEN @TanaCDFrom ELSE DI.RackNO END)
    AND DI.RackNO <= (CASE WHEN @TanaCDTo <> '' THEN @TanaCDTo ELSE DI.RackNO END)
--    AND DI.InventoryKBN = (CASE WHEN ISNULL(@InventoryKBN,'') <> '' THEN CONVERT(int, @InventoryKBN)
--    		 ELSE DI.InventoryKBN END)

    ORDER BY DI.RackNO
END

GO


