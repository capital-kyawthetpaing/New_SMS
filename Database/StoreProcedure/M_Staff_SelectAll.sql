 BEGIN TRY 
 Drop Procedure dbo.[M_Staff_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_Staff_SelectAll]    */
CREATE PROCEDURE [dbo].[M_Staff_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,	--0:基準日、1:履歴
    @ChangeDate varchar(10),
    @StoreCD  varchar(4),
    @StaffCDFrom  varchar(10),
    @StaffCDTo  varchar(10),
    @StaffName  varchar(40),
    @StaffKana  varchar(40)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --表示対象＝基準日の場合
    IF @DisplayKbn = 0
        BEGIN
        SELECT MS.StaffCD
              ,MS.StaffName
              ,MS.StaffKana
              ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = MS.StoreCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
              ORDER BY A.ChangeDate desc) AS StoreName
              ,CONVERT(varchar,MS.JoinDate,111) AS JoinDate
              ,CONVERT(varchar,MS.LeaveDate,111) AS LeaveDate
              ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate

        from M_Staff MS
            INNER JOIN (SELECT MSS.StaffCD, MAX(MSS.ChangeDate) AS ChangeDate
            FROM M_Staff MSS
            WHERE MSS.StaffCD >= ISNULL(@StaffCDFrom, '')
            AND  MSS.StaffCD <= ISNULL(@StaffCDTo, 'ZZZZ')
            AND MSS.ChangeDate <= CONVERT(DATE, @ChangeDate)
            AND MSS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MSS.StoreCD END)
            AND MSS.StaffName LIKE '%' + CASE WHEN @StaffName <> '' THEN @StaffName ELSE MSS.StaffName END + '%'
            AND ISNULL(MSS.StaffKana,'') LIKE '%' + CASE WHEN @StaffKana <> '' THEN @StaffKana ELSE ISNULL(MSS.StaffKana,'') END + '%'
            AND MSS.DeleteFlg = 0
            GROUP BY MSS.StaffCD
            )MSS ON  MSS.StaffCD = MS.StaffCD
            AND MSS.ChangeDate = MS.ChangeDate
        ORDER BY MS.StaffCD, MS.ChangeDate
        ;
        END
    ELSE
        BEGIN
        SELECT MS.StaffCD
              ,MS.StaffName
              ,MS.StaffKana
              ,(SELECT top 1 A.StoreName 
              FROM M_Store A 
              WHERE A.StoreCD = MS.StoreCD AND A.ChangeDate <= CONVERT(DATE, @ChangeDate)
              ORDER BY A.ChangeDate desc) AS StoreName
              ,CONVERT(varchar,MS.JoinDate,111) AS JoinDate
              ,CONVERT(varchar,MS.LeaveDate,111) AS LeaveDate
              ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate

        from M_Staff MS

        WHERE MS.StaffCD >= ISNULL(@StaffCDFrom, '')
        AND  MS.StaffCD <= ISNULL(@StaffCDTo, 'ZZZZ')
        AND MS.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE MS.StoreCD END)
        AND MS.StaffName LIKE '%' + CASE WHEN @StaffName <> '' THEN @StaffName ELSE MS.StaffName END + '%'
        AND ISNULL(MS.StaffKana,'') LIKE '%' + CASE WHEN @StaffKana <> '' THEN @StaffKana ELSE ISNULL(MS.StaffKana,'') END + '%'

        ORDER BY MS.StaffCD, MS.ChangeDate
        END
END


