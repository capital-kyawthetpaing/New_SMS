BEGIN TRY 
 Drop Procedure dbo.D_MoveRequest_SelectAllForShoukai
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_MoveRequest_SelectAllForShoukai]    */
CREATE PROCEDURE D_MoveRequest_SelectAllForShoukai(
    -- Add the parameters for the stored procedure here
    @AnswerDateFrom  varchar(10),
    @AnswerDateTo  varchar(10),
    @FromStoreCD varchar(6),
    @AnswerKBN tinyint,
    @Operator  varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
      
    SELECT DH.RequestNO
    	 ,CONVERT(varchar, DH.RequestDate, 111) AS RequestDate
    	 ,CONVERT(varchar, DH.AnswerDateTime, 111) AS AnswerDateTime

         ,DH.MovePurposeKBN
         ,M.MovePurposeName
         ,(SELECT top 1 A.StoreName
          FROM M_Store A 
          WHERE A.StoreCD = DH.ToStoreCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.RequestDate
          ORDER BY A.ChangeDate desc) AS FromStoreName
         ,DH.FromSoukoCD
         ,(SELECT top 1 A.SoukoName
          FROM M_Souko A 
          WHERE A.SoukoCD = DH.FromSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.RequestDate
          ORDER BY A.ChangeDate desc) AS FromSoukoName
         ,DH.ToSoukoCD
         ,(SELECT top 1 A.SoukoName
          FROM M_Souko A 
          WHERE A.SoukoCD = DH.ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.RequestDate
          ORDER BY A.ChangeDate desc) AS ToSoukoName
         ,(SELECT top 1 A.StaffName
              FROM M_Staff AS A 
              WHERE A.StaffCD = DH.StaffCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.RequestDate
              ORDER BY A.ChangeDate desc) AS Staff
         ,(SELECT top 1 M.SKUName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
         ,(SELECT top 1 M.ColorName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
         ,(SELECT top 1 M.SizeName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
         ,DM.SKUCD
         ,DM.RequestSu
         ,DMM.AnswerKBN_CNT1
         ,DMM.AnswerKBN_CNT9
    
    from D_MoveRequest AS DH
    INNER JOIN M_MovePurpose AS M
    ON M.MovePurposeKBN = DH.MovePurposeKBN
    AND M.MovePurposeType = 1	--“X•ÜŠÔˆÚ“®‚Ì‚Ý
            	
    LEFT OUTER JOIN (SELECT DM.RequestNO, MIN(DM.RequestRows) AS RequestRows
                    ,SUM(CASE WHEN ISNULL(DM.AnswerKBN,0) = 1 THEN 1 ELSE 0 END) AS AnswerKBN_CNT1
                    ,SUM(CASE WHEN ISNULL(DM.AnswerKBN,0) = 9 THEN 1 ELSE 0 END) AS AnswerKBN_CNT9
                    FROM D_MoveRequestDetailes AS DM
                    WHERE DM.DeleteDateTime IS NULL
                    GROUP BY DM.RequestNO) AS DMM
    ON DMM.RequestNO = DH.RequestNO
	
    LEFT OUTER JOIN D_MoveRequestDetailes AS DM 
        ON DM.RequestNO = DMM.RequestNO 
        AND DM.RequestRows = DMM.RequestRows 
    	AND DM.DeleteDateTime IS NULL
    WHERE ISNULL(DH.AnswerDateTime,'') >= (CASE WHEN @AnswerDateFrom <> '' THEN CONVERT(DATE, @AnswerDateFrom) ELSE ISNULL(DH.AnswerDateTime,'') END)
    AND ISNULL(DH.AnswerDateTime,'') <= (CASE WHEN @AnswerDateTo <> '' THEN CONVERT(DATE, @AnswerDateTo) ELSE ISNULL(DH.AnswerDateTime,'') END)
    --AND DH.FromStoreCD = (CASE WHEN @FromStoreCD <> '' THEN @FromStoreCD ELSE DH.FromStoreCD END)
    AND DH.ToStoreCD = (CASE WHEN @FromStoreCD <> '' THEN @FromStoreCD ELSE DH.ToStoreCD END)
    AND ISNULL(DM.AnswerKBN,0) =(CASE WHEN @AnswerKBN = 1 THEN ISNULL(DM.AnswerKBN,0) ELSE 0 END)
    AND DH.DeleteDateTime IS NULL
    
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.StoreCD = MS.StoreCD
        AND MF.ChangeDate <= DH.RequestDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.RequestDate
        AND MF.StoreCD = DH.FromStoreCD
        )
/*
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.ChangeDate <= DH.RequestDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.RequestDate
        AND MS.StoreCD = DH.ToStoreCD
        )
*/        
    ORDER BY DH.RequestDate, DH.RequestNO
    ;

END

GO
