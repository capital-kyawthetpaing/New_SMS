 BEGIN TRY 
 Drop Procedure dbo.[D_PaymentConfirm_SelectForSearch]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_PaymentConfirm_SelectForSearch]    */
CREATE PROCEDURE D_PaymentConfirm_SelectForSearch(
    -- Add the parameters for the stored procedure here
    @CollectClearDateFrom  varchar(10),
    @CollectClearDateTo    varchar(10),
    @DateFrom              varchar(10),
    @DateTo                varchar(10)
 --   @StoreCD  varchar(4),
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  DH.ConfirmNO
           ,DH.CollectNO
           ,CONVERT(varchar,DH.CollectClearDate,111) AS CollectClearDate
           ,DH.StaffCD
           ,CONVERT(varchar,DH.ConfirmDateTime,111) AS ConfirmDateTime
           ,DH.ConfirmAmount

          ,(SELECT TOP 1 A.StaffName
            FROM M_Staff AS A
            WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.CollectClearDate 
            AND A.DeleteFlg = 0
            ORDER BY A.ChangeDate DESC) AS StaffName
    FROM D_PaymentConfirm AS DH

    WHERE DH.CollectClearDate >= (CASE WHEN @CollectClearDateFrom <> '' THEN CONVERT(DATE, @CollectClearDateFrom) ELSE DH.CollectClearDate END)
    AND DH.CollectClearDate   <= (CASE WHEN @CollectClearDateTo <> ''   THEN CONVERT(DATE, @CollectClearDateTo)   ELSE DH.CollectClearDate END)
    AND CONVERT(DATE,DH.ConfirmDateTime) >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE CONVERT(DATE,DH.ConfirmDateTime) END)
    AND CONVERT(DATE,DH.ConfirmDateTime) <= (CASE WHEN @DateTo <> ''   THEN CONVERT(DATE, @DateTo)   ELSE CONVERT(DATE,DH.ConfirmDateTime) END)
    AND DH.DeleteDateTime IS NULL
    AND (DH.InputKBN IN (1,2) OR (DH.InputKBN = 3 AND DH.AdvanceFLG = 1))

    ORDER BY DH.CollectClearDate desc, DH.ConfirmNO, DH.CollectNO
    ;

END

GO
