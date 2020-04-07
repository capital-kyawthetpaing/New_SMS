 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_SelectByJanCD]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_SKU_SelectByJanCD]    */
CREATE PROCEDURE M_SKU_SelectByJanCD(
    -- Add the parameters for the stored procedure here
    @ITemCD varchar(30),
    @JanCD varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.AdminNO
          ,MS.SKUCD
    from F_SKU(@ChangeDate) AS MS
    
    WHERE MS.ITemCD <> @ITemCD
    --AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
	AND MS.JanCD = @JanCD
	AND MS.DeleteFlg = 0
    ;
END

