BEGIN TRY 
 Drop Procedure D_MoveRequest_SelectAll
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [D_MoveRequest_SelectAll]    */
CREATE PROCEDURE D_MoveRequest_SelectAll(
    -- Add the parameters for the stored procedure here
    @RequestDateFrom  varchar(10),
    @RequestDateTo  varchar(10),
    @MovePurposeKBN tinyint,
    @FromSoukoCD varchar(6),
    @ToSoukoCD varchar(6),
    @StaffCD  varchar(10),
    
    @Operator  varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
      
    SELECT DH.RequestNO
            , DH.MovePurposeKBN
            ,(SELECT M.MovePurposeName FROM M_MovePurpose AS M
            	WHERE M.MovePurposeKBN = DH.MovePurposeKBN) AS MovePurposeName
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
          ,CONVERT(varchar,DH.RequestDate,111) AS RequestDate
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
    
    from D_MoveRequest AS DH
    
    LEFT OUTER JOIN (SELECT DM.RequestNO, MIN(DM.RequestRows) AS RequestRows
                    FROM D_MoveRequestDetailes AS DM
                    WHERE DM.DeleteDateTime IS NULL
                    GROUP BY DM.RequestNO) AS DMM
    ON DMM.RequestNO = DH.RequestNO
    LEFT OUTER JOIN D_MoveRequestDetailes AS DM 
        ON DM.RequestNO = DMM.RequestNO 
        AND DM.RequestRows = DMM.RequestRows 
    	AND DM.DeleteDateTime IS NULL
    WHERE DH.RequestDate >= (CASE WHEN @RequestDateFrom <> '' THEN CONVERT(DATE, @RequestDateFrom) ELSE DH.RequestDate END)
    AND DH.RequestDate <= (CASE WHEN @RequestDateTo <> '' THEN CONVERT(DATE, @RequestDateTo) ELSE DH.RequestDate END)
    AND DH.MovePurposeKBN = (CASE WHEN ISNULL(@MovePurposeKBN,0) <> 0 THEN @MovePurposeKBN ELSE DH.MovePurposeKBN END)
    
    AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
    AND DH.FromSoukoCD = (CASE WHEN @FromSoukoCD <> '' THEN @FromSoukoCD ELSE DH.FromSoukoCD END)
    AND DH.ToSoukoCD = (CASE WHEN @ToSoukoCD <> '' THEN @ToSoukoCD ELSE DH.ToSoukoCD END)
    AND DH.DeleteDateTime IS NULL
    
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.ChangeDate <= DH.RequestDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.RequestDate
        AND MS.StoreCD = DH.FromStoreCD
        )
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
        
    ORDER BY DH.RequestDate, DH.RequestNO
    ;

END

GO
