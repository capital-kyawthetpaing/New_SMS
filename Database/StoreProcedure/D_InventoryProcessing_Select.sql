 BEGIN TRY 
 Drop Procedure dbo.[D_InventoryProcessing_Select]
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
CREATE PROCEDURE [dbo].[D_InventoryProcessing_Select]
    -- Add the parameters for the stored procedure here
    @SoukoCD varchar(13),
    @InventoryDate varchar(10),
    @TanaCDFrom varchar(13),
    @TanaCDTo varchar(13),
    @InventoryKBN varchar(1)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DI.SoukoCD
        , DI.InventoryDate
        , DI.InventoryKBN
    FROM D_InventoryProcessing AS DI

    WHERE DI.SoukoCD = @SoukoCD
    AND DI.InventoryDate = CONVERT(date, @InventoryDate)
    AND ISNULL(DI.FromRackNO,'') = (CASE WHEN ISNULL(@TanaCDFrom,'') <> '' THEN @TanaCDFrom ELSE '' END)
    AND ISNULL(DI.ToRackNO,'') = (CASE WHEN ISNULL(@TanaCDTo,'') <> '' THEN @TanaCDTo ELSE '' END)
    AND DI.InventoryKBN = (CASE WHEN ISNULL(@InventoryKBN,'') <> '' THEN CONVERT(int, @InventoryKBN)
    		 ELSE DI.InventoryKBN END)

    ORDER BY DI.UpdateDateTime desc, DI.InventoryKBN desc
END


