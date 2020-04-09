 BEGIN TRY 
 Drop Procedure dbo.[M_MakerBrand_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_MakerBrand_SelectAll]    */
CREATE PROCEDURE [dbo].[M_MakerBrand_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,	--0:����A1:����
    @ChangeDate varchar(10),
    @BrandName  varchar(50),
    @MakerCD  varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --�\���Ώہ�����̏ꍇ
    IF @DisplayKbn = 0
        BEGIN
        SELECT MS.BrandCD
        	  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,(SELECT A.BrandName FROM M_Brand A WHERE A.BrandCD = MS.BrandCD) AS BrandName
              ,MS.MakerCD
              ,(SELECT top 1 A.VendorName 
              FROM M_Vendor A 
              WHERE A.VendorCD = MS.MakerCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
              ORDER BY A.ChangeDate desc) AS MakerName

        from M_MakerBrand MS
            INNER JOIN (SELECT MSS.BrandCD, MSS.MakerCD, MAX(MSS.ChangeDate) AS ChangeDate
            FROM M_MakerBrand MSS
            WHERE MSS.ChangeDate <= CONVERT(DATE, @ChangeDate)
            AND MSS.MakerCD = (CASE WHEN @MakerCD <> '' THEN @MakerCD ELSE MSS.MakerCD END)
            AND EXISTS(SELECT 1 FROM M_Brand M WHERE M.BrandName LIKE '%' + CASE WHEN @BrandName <> '' THEN @BrandName ELSE M.BrandName END + '%' 
                        AND M.BrandCD = MSS.BrandCD)
            AND MSS.DeleteFlg = 0
            GROUP BY MSS.BrandCD, MSS.MakerCD
            )MSS ON  MSS.BrandCD = MS.BrandCD AND MSS.MakerCD = MS.MakerCD
            AND MSS.ChangeDate = MS.ChangeDate
        ORDER BY MS.BrandCD, MS.MakerCD, MS.ChangeDate
        ;
        END
    ELSE
        BEGIN
        SELECT MS.BrandCD
        	  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,(SELECT A.BrandName FROM M_Brand A WHERE A.BrandCD = MS.BrandCD) AS BrandName
              ,MS.MakerCD
              ,(SELECT top 1 A.VendorName 
              FROM M_Vendor A 
              WHERE A.VendorCD = MS.MakerCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
              ORDER BY A.ChangeDate desc) AS MakerName

        from M_MakerBrand MS

        WHERE MS.MakerCD = (CASE WHEN @MakerCD <> '' THEN @MakerCD ELSE MS.MakerCD END)
            AND EXISTS(SELECT 1 FROM M_Brand M WHERE M.BrandName LIKE '%' + CASE WHEN @BrandName <> '' THEN @BrandName ELSE M.BrandName END + '%' 
                        AND M.BrandCD = MS.BrandCD)
       
        ORDER BY MS.BrandCD, MS.MakerCD, MS.ChangeDate
        END
END


