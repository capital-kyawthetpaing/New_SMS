 BEGIN TRY 
 Drop Procedure dbo.[M_Location_SelectForTanaoroshi]
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
Create PROCEDURE [dbo].[M_Location_SelectForTanaoroshi]
    -- Add the parameters for the stored procedure here
    @InventoryDate varchar(10),
    @SoukoCD varchar(13),
    @TanaCDFrom varchar(13),
    @TanaCDTo varchar(13)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT ML.SoukoCD
        , ML.TanaCD
        , ML.ChangeDate
    FROM M_Location AS ML

    WHERE ML.SoukoCD = @SoukoCD
    AND ML.TanaCD >= (CASE WHEN @TanaCDFrom <> '' THEN @TanaCDFrom ELSE ML.TanaCD END)
    AND ML.TanaCD <= (CASE WHEN @TanaCDTo <> '' THEN @TanaCDTo ELSE ML.TanaCD END)
    AND ML.DeleteFlg = 0
    AND ML.ChangeDate <= @InventoryDate
    ORDER BY ML.TanaCD
END


