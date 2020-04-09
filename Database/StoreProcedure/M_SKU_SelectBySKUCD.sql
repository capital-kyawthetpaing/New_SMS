 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectBySKUCD]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_SKU_SelectBySKUCD]    */
CREATE PROCEDURE M_SKU_SelectBySKUCD(
    -- Add the parameters for the stored procedure here
    @SKUCD varchar(30),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.AdminNO
          ,MS.SKUCD
          ,MS.SKUName
    from F_SKU(@ChangeDate) AS MS
    
    WHERE MS.SKUCD = @SKUCD
    --AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    AND MS.DeleteFlg = 0
    ORDER BY MS.ChangeDate
    ;
END

