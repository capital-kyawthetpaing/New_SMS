 BEGIN TRY 
 Drop Procedure dbo.[M_Brand_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [M_Brand_SelectAll]    */
CREATE PROCEDURE [dbo].[M_Brand_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,	--0:基準日、1:履歴
    @ChangeDate varchar(10),
    @BrandName  varchar(50),
    @MakerCD  varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --表示対象＝基準日の場合
    IF @DisplayKbn = 0
    BEGIN
        SELECT MB.BrandCD
           ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
          ,MB.BrandName
          ,MS.MakerCD
          ,(SELECT top 1 A.VendorName 
              FROM M_Vendor A 
              WHERE A.VendorCD = MS.MakerCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
              AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS MakerName

        from M_Brand MB
        LEFT OUTER JOIN (SELECT MSS.BrandCD, MSS.MakerCD, MAX(MSS.ChangeDate) AS ChangeDate
                FROM M_MakerBrand MSS
                WHERE MSS.ChangeDate <= CONVERT(DATE, @ChangeDate)
                AND MSS.DeleteFlg = 0
                GROUP BY MSS.BrandCD, MSS.MakerCD
                )MS ON  MS.BrandCD = MB.BrandCD

        WHERE MB.BrandName LIKE '%' + CASE WHEN @BrandName <> '' THEN @BrandName ELSE MB.BrandName END + '%' 
        AND ISNULL(MS.MakerCD,'') = (CASE WHEN @MakerCD <> '' THEN @MakerCD ELSE ISNULL(MS.MakerCD,'') END)
        ORDER BY MB.BrandCD, MS.MakerCD, MS.ChangeDate
        ;
    END
    ELSE
    BEGIN
        SELECT MB.BrandCD
        	  ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
              ,MB.BrandName
              ,MS.MakerCD
              ,(SELECT top 1 A.VendorName 
                  FROM M_Vendor A 
                  WHERE A.VendorCD = MS.MakerCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
                  AND A.DeleteFlg = 0
                  ORDER BY A.ChangeDate desc) AS MakerName

        from M_Brand MB
            LEFT OUTER JOIN M_MakerBrand MS
            ON  MS.BrandCD = MB.BrandCD
            AND MS.DeleteFlg = 0
            
        WHERE MB.BrandName LIKE '%' + CASE WHEN @BrandName <> '' THEN @BrandName ELSE MB.BrandName END + '%' 
        AND ISNULL(MS.MakerCD,'') = (CASE WHEN @MakerCD <> '' THEN @MakerCD ELSE ISNULL(MS.MakerCD,'') END)
        ORDER BY MB.BrandCD, MS.MakerCD, MS.ChangeDate
    END
END


