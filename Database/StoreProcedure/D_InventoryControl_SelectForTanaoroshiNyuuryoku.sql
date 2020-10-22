 BEGIN TRY 
 Drop Procedure dbo.[D_InventoryControl_SelectForTanaoroshiNyuuryoku]
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
CREATE PROCEDURE [dbo].[D_InventoryControl_SelectForTanaoroshiNyuuryoku]
    -- Add the parameters for the stored procedure here
    @SoukoCD varchar(13),
    @InventoryDate varchar(10),
    @RackNO varchar(13)
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
    AND DI.RackNO = @RackNO
    ;
END

GO


