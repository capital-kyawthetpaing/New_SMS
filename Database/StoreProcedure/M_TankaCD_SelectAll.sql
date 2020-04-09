 BEGIN TRY 
 Drop Procedure dbo.[M_TankaCD_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_TankaCD_SelectAll]    */
CREATE PROCEDURE [dbo].[M_TankaCD_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,    --0:基準日、1:履歴
    @ChangeDate varchar(10),
    @TankaCDFrom  varchar(13),
    @TankaCDTo  varchar(13),
    @TankaName  varchar(20)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --表示対象＝基準日の場合
    IF @DisplayKbn = 0
        BEGIN
        SELECT MT.TankaCD
            ,CONVERT(varchar,MT.ChangeDate,111) AS ChangeDate
            ,MT.TankaName
              ,MT.GeneralRate
              ,MT.MemberRate
              ,MT.ClientRate
              ,MT.SaleRate
              ,MT.WebRate
            ,CASE MT.RoundKBN WHEN 1 THEN '切上' WHEN 2 THEN '切捨' WHEN 3 THEN '四捨五入' ELSE '' END AS RoundKBN
              ,MT.Remarks
        from M_TankaCD MT
            INNER JOIN (SELECT MKK.TankaCD, MAX(MKK.ChangeDate) AS ChangeDate
            FROM M_TankaCD MKK
            WHERE MKK.TankaCD >= ISNULL(@TankaCDFrom, '')
            AND  MKK.TankaCD <= ISNULL(@TankaCDTo, 'ZZZZ')
            AND MKK.ChangeDate <= CONVERT(DATE, @ChangeDate)
            AND MKK.TankaName LIKE '%' + CASE WHEN @TankaName <> '' THEN @TankaName ELSE MKK.TankaName END + '%'
            AND MKK.DeleteFlg = 0
            GROUP BY MKK.TankaCD
            )MKK ON  MKK.TankaCD = MT.TankaCD
            AND MKK.ChangeDate = MT.ChangeDate
        ORDER BY MT.TankaCD, MT.ChangeDate
        ;
        END
    ELSE
        BEGIN
        SELECT MT.TankaCD
            ,CONVERT(varchar,MT.ChangeDate,111) AS ChangeDate
            ,MT.TankaName
              ,MT.GeneralRate
              ,MT.MemberRate
              ,MT.ClientRate
              ,MT.SaleRate
              ,MT.WebRate
            ,CASE MT.RoundKBN WHEN 1 THEN '切上' WHEN 2 THEN '切捨' WHEN 3 THEN '四捨五入' ELSE '' END AS RoundKBN
              ,MT.Remarks
        from M_TankaCD MT

        WHERE MT.TankaCD >= ISNULL(@TankaCDFrom, '')
        AND  MT.TankaCD <= ISNULL(@TankaCDTo, 'ZZZZ')
        AND MT.TankaName LIKE '%' + CASE WHEN @TankaName <> '' THEN @TankaName ELSE MT.TankaName END + '%'

        ORDER BY MT.TankaCD, MT.ChangeDate
        ;
        END
END


