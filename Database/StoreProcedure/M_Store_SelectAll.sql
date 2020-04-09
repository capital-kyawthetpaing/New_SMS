 BEGIN TRY 
 Drop Procedure dbo.[M_Store_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Store_SelectAll]    */
CREATE PROCEDURE [dbo].[M_Store_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,	--0:基準日、1:履歴
    @ChangeDate varchar(10),
    @StoreCDFrom  varchar(4),
    @StoreCDTo  varchar(4),
    @StoreName  varchar(40),
    @StoreKBN1 tinyint,		--チェックありの場合は１を設定、それ以外は０
    @StoreKBN2 tinyint,		--チェックありの場合は２を設定、それ以外は０
    @StoreKBN3 tinyint		--チェックありの場合は３を設定、それ以外は０
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --表示対象＝基準日の場合
    IF @DisplayKbn = 0
        BEGIN
        SELECT MS.StoreCD
            ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
            ,CASE MS.StoreKBN WHEN 1 THEN '実店舗' WHEN 2 THEN 'WEB店' WHEN 3 THEN 'WEBまとめ店舗' END AS StoreKBN --1:実店舗、2:WEB店、3:WEBまとめ店舗
            ,MS.StorePlaceKBN
            ,MS.StoreName

        from M_Store MS
            INNER JOIN (SELECT MSS.StoreCD, MAX(MSS.ChangeDate) AS ChangeDate
            FROM M_Store MSS
            WHERE MSS.StoreCD >= ISNULL(@StoreCDFrom, '')
            AND  MSS.StoreCD <= ISNULL(@StoreCDTo, 'ZZZZ')
            AND MSS.StoreKBN IN (@StoreKBN1,@StoreKBN2,@StoreKBN3)
            AND MSS.ChangeDate <= CONVERT(DATE, @ChangeDate)
            AND MSS.StoreName LIKE '%' + CASE WHEN @StoreName <> '' THEN @StoreName ELSE MSS.StoreName END + '%'
            GROUP BY MSS.StoreCD
            )MSS ON  MSS.StoreCD = MS.StoreCD
            AND MSS.ChangeDate = MS.ChangeDate
        ORDER BY MS.StoreCD, MS.ChangeDate
        ;
        END
    ELSE
        BEGIN
        SELECT MS.StoreCD
            ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
            ,CASE MS.StoreKBN WHEN 1 THEN '実店舗' WHEN 2 THEN 'WEB店' WHEN 3 THEN 'WEBまとめ店舗' END AS StoreKBN --1:実店舗、2:WEB店、3:WEBまとめ店舗
            ,MS.StorePlaceKBN
            ,MS.StoreName

        from M_Store MS

        WHERE MS.StoreCD >= ISNULL(@StoreCDFrom, '')
        AND  MS.StoreCD <= ISNULL(@StoreCDTo, 'ZZZZ')
        AND MS.StoreKBN IN (@StoreKBN1,@StoreKBN2,@StoreKBN3)
        AND MS.StoreName LIKE '%' + CASE WHEN @StoreName <> '' THEN @StoreName ELSE MS.StoreName END + '%'

        ORDER BY MS.StoreCD, MS.ChangeDate
        END
END


