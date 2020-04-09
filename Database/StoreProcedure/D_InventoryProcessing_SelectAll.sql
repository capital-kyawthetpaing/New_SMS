 BEGIN TRY 
 Drop Procedure dbo.[D_InventoryProcessing_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [D_InventoryProcessing_SelectAll]    */
CREATE PROCEDURE [dbo].[D_InventoryProcessing_SelectAll](
    -- Add the parameters for the stored procedure here
--    @InventoryDate varchar(10),
    @SoukoCD  varchar(6)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DI.InventoryNO
       ,DI.SoukoCD
       ,DI.FromRackNO
       ,DI.ToRackNO
       ,CONVERT(varchar,DI.InventoryDate,111) AS InventoryDate
       ,(CASE DI.InventoryKBN WHEN 1 THEN '棚卸締'      --"1:棚卸締、2:棚卸締キャンセル,3:棚卸確定"                                             
            WHEN 2 THEN '棚卸締キャンセル'
            WHEN 3 THEN '棚卸確定'
            ELSE '' END) AS InventoryKBN

       ,DI.ProcessingDateTime
       ,DI.StaffCD

    FROM D_InventoryProcessing AS DI

    WHERE DI.DeleteDateTime IS NULL
    AND DI.SoukoCD = @SoukoCD
--    AND DI.InventoryDate = CONVERT(date, @InventoryDate)
    ORDER BY DI.ProcessingDateTime desc
        ,DI.InventoryDate desc
        ,DI.InventoryKBN
        ,DI.FromRackNO desc
        ,DI.ToRackNO desc

    ;

END


