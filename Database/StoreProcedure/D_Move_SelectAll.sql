 BEGIN TRY 
 Drop Procedure dbo.[D_Move_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/****** Object:  StoredProcedure [D_Move_SelectAll]    */
CREATE PROCEDURE D_Move_SelectAll(
    -- Add the parameters for the stored procedure here
    @MoveDateFrom  varchar(10),
    @MoveDateTo  varchar(10),
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
      
    SELECT DH.MoveNO
            , DH.MovePurposeKBN
            ,(SELECT M.MovePurposeName FROM M_MovePurpose AS M
            	WHERE M.MovePurposeKBN = DH.MovePurposeKBN) AS MovePurposeName
          ,DH.FromSoukoCD
          ,(SELECT top 1 A.SoukoName
          FROM M_Souko A 
          WHERE A.SoukoCD = DH.FromSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MoveDate
          ORDER BY A.ChangeDate desc) AS FromSoukoName
          ,DH.ToSoukoCD
          ,(SELECT top 1 A.SoukoName
          FROM M_Souko A 
          WHERE A.SoukoCD = DH.ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MoveDate
          ORDER BY A.ChangeDate desc) AS ToSoukoName
          ,CONVERT(varchar,DH.MoveDate,111) AS MoveDate
          ,(SELECT top 1 A.StaffName
              FROM M_Staff AS A 
              WHERE A.StaffCD = DH.StaffCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MoveDate
              ORDER BY A.ChangeDate desc) AS Staff
          ,(SELECT top 1 M.SKUName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.MoveDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
    
    from D_Move AS DH
    
    LEFT OUTER JOIN (SELECT DM.MoveNO, MIN(DM.MoveRows) AS MoveRows
                    FROM D_MoveDetails AS DM
                    WHERE DM.DeleteDateTime IS NULL
                    GROUP BY DM.MoveNO) AS DMM
    ON DMM.MoveNO = DH.MoveNO
    LEFT OUTER JOIN D_MoveDetails AS DM 
        ON DM.MoveNO = DMM.MoveNO 
        AND DM.MoveRows = DMM.MoveRows 
    	AND DM.DeleteDateTime IS NULL
    WHERE DH.MoveDate >= (CASE WHEN @MoveDateFrom <> '' THEN CONVERT(DATE, @MoveDateFrom) ELSE DH.MoveDate END)
    AND DH.MoveDate <= (CASE WHEN @MoveDateTo <> '' THEN CONVERT(DATE, @MoveDateTo) ELSE DH.MoveDate END)
    AND DH.MovePurposeKBN = (CASE WHEN ISNULL(@MovePurposeKBN,0) <> 0 THEN @MovePurposeKBN ELSE DH.MovePurposeKBN END)
    
    AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
    AND ISNULL(DH.FromSoukoCD,'') = (CASE WHEN @FromSoukoCD <> '' THEN @FromSoukoCD ELSE ISNULL(DH.FromSoukoCD,'') END)
    AND ISNULL(DH.ToSoukoCD,'') = (CASE WHEN @ToSoukoCD <> '' THEN @ToSoukoCD ELSE ISNULL(DH.ToSoukoCD,'') END)
    AND DH.DeleteDateTime IS NULL
    
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.ChangeDate <= DH.MoveDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.MoveDate
        AND MS.StoreCD = DH.StoreCD
        )
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
        from M_StoreAuthorizations MS
        INNER JOIN M_Staff AS MF
        ON MF.StaffCD = @Operator
        AND MF.ChangeDate <= DH.MoveDate
        AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
        AND MF.DeleteFlg = 0
        where MS.ChangeDate <= DH.MoveDate
        AND MS.StoreCD = ISNULL(DH.ToStoreCD,MS.StoreCD)
        )
        
    ORDER BY DH.MoveDate, DH.MoveNO
    ;

END

GO
