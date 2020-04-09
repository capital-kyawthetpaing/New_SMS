 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_Kouza_SelectAll]    */
CREATE PROCEDURE [dbo].[M_Kouza_SelectAll](
    -- Add the parameters for the stored procedure here
    @DisplayKbn tinyint,	--0:基準日、1:履歴
    @ChangeDate varchar(10),
    @KouzaCDFrom  varchar(3),
    @KouzaCDTo  varchar(3),
    @KouzaName  varchar(50)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --表示対象＝基準日の場合
    IF @DisplayKbn = 0
        BEGIN
        SELECT MK.KouzaCD
            ,CONVERT(varchar,MK.ChangeDate,111) AS ChangeDate
            ,MK.KouzaName
            ,(SELECT top 1 MB.BankName FROM M_Bank MB 
                WHERE MB.BankCD =MK. BankCD AND MB.ChangeDate <= CONVERT(DATE, @ChangeDate)
                 ORDER BY MB.ChangeDate desc) BankName
            ,(SELECT top 1 MS.BranchName FROM M_BankBranch MS 
                WHERE MS.BankCD =MK.BankCD AND MS.BranchCD = MK.BranchCD AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
                 ORDER BY MS.ChangeDate desc) BranchName
            ,CASE MK.KouzaKBN WHEN 1 THEN '普' WHEN 2 THEN '当' ELSE '' END AS KouzaKBN
            ,MK.KouzaNO
        from M_Kouza MK
            INNER JOIN (SELECT MKK.KouzaCD, MAX(MKK.ChangeDate) AS ChangeDate
            FROM M_Kouza MKK
            WHERE MKK.KouzaCD >= ISNULL(@KouzaCDFrom, '')
            AND  MKK.KouzaCD <= ISNULL(@KouzaCDTo, 'ZZZZ')
            AND MKK.ChangeDate <= CONVERT(DATE, @ChangeDate)
            AND MKK.KouzaName LIKE '%' + CASE WHEN @KouzaName <> '' THEN @KouzaName ELSE MKK.KouzaName END + '%'
            AND MKK.DeleteFlg = 0
            GROUP BY MKK.KouzaCD
            )MKK ON  MKK.KouzaCD = MK.KouzaCD
            AND MKK.ChangeDate = MK.ChangeDate
        ORDER BY MK.KouzaCD, MK.ChangeDate
        ;
        END
    ELSE
        BEGIN
        SELECT MK.KouzaCD
            ,CONVERT(varchar,MK.ChangeDate,111) AS ChangeDate
            ,MK.KouzaName
            ,(SELECT top 1 MB.BankName FROM M_Bank MB 
                WHERE MB.BankCD =MK. BankCD AND MB.ChangeDate <= CONVERT(DATE, @ChangeDate)
                 ORDER BY MB.ChangeDate desc) BankName
            ,(SELECT top 1 MS.BranchName FROM M_BankBranch MS 
                WHERE MS.BankCD =MK.BankCD AND MS.BranchCD = MK.BranchCD AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
                 ORDER BY MS.ChangeDate desc) BranchName
            ,CASE MK.KouzaKBN WHEN 1 THEN '普' WHEN 2 THEN '当' ELSE '' END AS KouzaKBN
            ,MK.KouzaNO
        from M_Kouza MK

        WHERE MK.KouzaCD >= ISNULL(@KouzaCDFrom, '')
        AND  MK.KouzaCD <= ISNULL(@KouzaCDTo, 'ZZZZ')
        AND MK.KouzaName LIKE '%' + CASE WHEN @KouzaName <> '' THEN @KouzaName ELSE MK.KouzaName END + '%'

        ORDER BY MK.KouzaCD, MK.ChangeDate
        ;
        END
END


