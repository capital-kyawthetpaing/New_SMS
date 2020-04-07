 BEGIN TRY 
 Drop Procedure dbo.[M_StoreButtonGroup_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_StoreButtonGroup_SelectAll]    */
CREATE PROCEDURE [dbo].[M_StoreButtonGroup_SelectAll](
    -- Add the parameters for the stored procedure here
    @ProgramKBN tinyint,	--1:受注＆出荷売上
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

-- Insert statements for procedure here
    SELECT MS.StoreCD
        ,MS.GroupNO
        ,MS.BottunName
        ,MS.MasterKBN	--1:商品、2:顧客

    from M_StoreBottunGroup MS
    WHERE MS.StoreCD = @StoreCD
    AND MS.ProgramKBN = @ProgramKBN
    ORDER BY MS.GroupNO
    ;

END


