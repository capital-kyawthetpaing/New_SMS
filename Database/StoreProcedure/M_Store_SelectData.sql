 BEGIN TRY 
 Drop Procedure dbo.[M_Store_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_Store_SelectData]    */
CREATE PROCEDURE [dbo].[M_Store_SelectData](
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.StoreCD
        ,CONVERT(varchar,MS.ChangeDate,111) AS ChangeDate
        ,MS.StoreKBN
        ,MS.StorePlaceKBN
        ,MS.StoreName
        ,MS.MallCD
        ,MS.APIKey
        ,MS.ZipCD1
        ,MS.ZipCD2
        ,MS.Address1
        ,MS.Address2
        ,MS.MailAddress1
        ,MS.TelephoneNO
        ,MS.FaxNO
        ,MS.KouzaCD
        ,MS.ReceiptPrint
        ,MS.ApprovalStaffCD11
        ,MS.ApprovalStaffCD12
        ,MS.ApprovalStaffCD21
        ,MS.ApprovalStaffCD22
        ,MS.ApprovalStaffCD31
        ,MS.ApprovalStaffCD32
        ,MS.DeliveryDate
        ,MS.PaymentTerms
        ,MS.DeliveryPlace
        ,MS.ValidityPeriod
        ,MS.Print1
        ,MS.Print2
        ,MS.Print3
        ,MS.Print4
        ,MS.Print5
        ,MS.Print6
        ,MS.MoveMailPatternCD
        ,MS.Remarks
        ,MS.DeleteFlg
        ,MS.UsedFlg
        ,MS.InsertOperator
        ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
        ,MS.UpdateOperator
        ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
        ,MM.Char1 AS MallNM
        ,MK2.KouzaName
        ,MF112.StaffName AS ApprovalStaffNM11
        ,MF122.StaffName AS ApprovalStaffNM12
        ,MF212.StaffName AS ApprovalStaffNM21
        ,MF222.StaffName AS ApprovalStaffNM22
        ,MF312.StaffName AS ApprovalStaffNM31
        ,MF322.StaffName AS ApprovalStaffNM32

        ,MS.MailAddress2
        ,MS.MailAddress3
        ,(SELECT A.MailPatternName FROM M_MailPattern AS A WHERE A.MailPatternCD = MS.MoveMailPatternCD) AS MailPatternName
        
    from M_Store MS
    
    LEFT OUTER JOIN M_MultiPorpose MM
    ON MM.[Key] = MS.MallCD
    AND MM.ID = 212     --モールCD (4)      ★共通変数で持たせるべき
    
    LEFT OUTER JOIN (SELECT A.KouzaCD, MAX(A.ChangeDate) ChangeDate
        FROM M_Kouza A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        GROUP BY A.KouzaCD
        ) AS MK
    ON MK.KouzaCD = MS.KouzaCD
    
    LEFT OUTER JOIN M_Kouza MK2
    ON MK2.KouzaCD = MK.KouzaCD
    AND MK2.ChangeDate = MK.ChangeDate
    
    --承認者11～32
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF11
    ON MF11.StaffCD = MS.ApprovalStaffCD11
    
    LEFT OUTER JOIN M_Staff MF112
    ON MF112.StaffCD = MF11.StaffCD
    AND MF112.ChangeDate = MF11.ChangeDate
    
    
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF12
    ON MF12.StaffCD = MS.ApprovalStaffCD12
    
    LEFT OUTER JOIN M_Staff MF122
    ON MF122.StaffCD = MF12.StaffCD
    AND MF122.ChangeDate = MF12.ChangeDate
    
    
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF21
    ON MF21.StaffCD = MS.ApprovalStaffCD21
    
    LEFT OUTER JOIN M_Staff MF212
    ON MF212.StaffCD = MF21.StaffCD
    AND MF212.ChangeDate = MF21.ChangeDate
    
    
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF22
    ON MF22.StaffCD = MS.ApprovalStaffCD22
    
    LEFT OUTER JOIN M_Staff MF222
    ON MF222.StaffCD = MF22.StaffCD
    AND MF222.ChangeDate = MF22.ChangeDate
    
    
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF31
    ON MF31.StaffCD = MS.ApprovalStaffCD31
    
    LEFT OUTER JOIN M_Staff MF312
    ON MF312.StaffCD = MF31.StaffCD
    AND MF312.ChangeDate = MF31.ChangeDate
    
    
    LEFT OUTER JOIN (SELECT A.StaffCD, MAX(A.ChangeDate) ChangeDate  
        FROM M_Staff A
        WHERE A.ChangeDate <= CONVERT(DATE, @ChangeDate)
        AND (A.LeaveDate IS NULL
            OR A.LeaveDate > CONVERT(DATE, @ChangeDate))
        GROUP BY A.StaffCD
    ) AS MF32
    ON MF32.StaffCD = MS.ApprovalStaffCD32
    
    LEFT OUTER JOIN M_Staff MF322
    ON MF322.StaffCD = MF32.StaffCD
    AND MF322.ChangeDate = MF32.ChangeDate
    
    
    WHERE MS.StoreCD = @StoreCD
    AND MS.ChangeDate = CONVERT(DATE, @ChangeDate)
    ;
END


