 BEGIN TRY 
 Drop Procedure dbo.[M_StoreButtonDetails_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[M_StoreButtonDetails_SelectAll](
    -- Add the parameters for the stored procedure here
    @ProgramKBN tinyint,	--1:受注＆出荷売上
    @StoreCD  varchar(4),
    @GroupNO  tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

-- Insert statements for procedure here
    SELECT MS.StoreCD
        ,MS.GroupNO
        ,MS.Horizontal
        ,MS.Vertical
        ,MS.MasterKBN	--1:商品、2:顧客
        ,MS.Botton
        ,MS.BottunName
        ,MS.AdminNO
        ,MS.JanCD
        ,MS.CustomerCD

    from M_StoreBottunDetails MS
    WHERE MS.StoreCD = @StoreCD
    AND MS.ProgramKBN = @ProgramKBN
    AND MS.GroupNO = (CASE ISNULL(@GroupNO,'') WHEN '' THEN
      				(SELECT MIN(A.GroupNO) 
                    FROM M_StoreBottunGroup AS A 
                    WHERE A.BottunName IS NOT NULL
                    AND A.StoreCD = @StoreCD
                    AND A.ProgramKBN = @ProgramKBN)
                    ELSE @GroupNO END)
    ORDER BY MS.Horizontal, MS.Vertical
    ;

END

