 BEGIN TRY 
 Drop Procedure dbo.[D_Collect_SelectForSearch]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Collect_SelectForSearch]    */
CREATE PROCEDURE D_Collect_SelectForSearch(
    -- Add the parameters for the stored procedure here
    @CollectDateFrom  varchar(10),
    @CollectDateTo    varchar(10),
    @DateFrom         varchar(10),
    @DateTo           varchar(10),
 --   @StoreCD  varchar(4),
    @CustomerCD       varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DH.CollectNO
          ,(CASE DH.InputKBN WHEN 1 THEN 'éÊçû'
                             WHEN 2 THEN 'ì¸óÕ'
                             WHEN 3 THEN 'POS'
                             ELSE '' END) AS InputKBN
          ,DH.StoreCD
          ,(SELECT top 1 A.StoreName
            FROM M_Store A 
            WHERE A.StoreCD = DH.StoreCD 
            AND A.ChangeDate <= DH.CollectDate
            AND A.DeleteFlg = 0
            ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.StaffCD
          ,(SELECT TOP 1 A.StaffName
            FROM M_Staff AS A
            WHERE A.StaffCD = DH.StaffCD 
            AND A.ChangeDate <= DH.CollectDate 
            AND A.DeleteFlg = 0
            ORDER BY A.ChangeDate DESC) AS StaffName
          ,CONVERT(varchar,DH.InputDatetime,111) AS InputDatetime
          ,DH.WebCollectNO
          ,DH.WebCollectType
          ,(SELECT M.PatternName FROM M_Settlement AS M WHERE M.PatternCD = DH.WebCollectType) AS WebCollectTypeName
          ,DH.CollectCustomerCD
          ,(SELECT TOP 1 A.CustomerName
            FROM M_Customer AS A
            WHERE A.CustomerCD = DH.CollectCustomerCD 
            AND A.ChangeDate <= DH.CollectDate
            AND A.DeleteFlg = 0
            ORDER BY A.ChangeDate DESC) AS CustomerName
                    
          ,CONVERT(varchar,DH.CollectDate,111) AS CollectDate
          ,DH.PaymentMethodCD
          ,(SELECT M.DenominationName FROM M_DenominationKBN AS M WHERE M.DenominationCD = DH.PaymentMethodCD) AS PaymentMethodName
          ,DH.KouzaCD
          ,DH.BillDate
          ,DH.CollectAmount
          ,DH.FeeDeduction
          ,DH.Deduction1
          ,DH.Deduction2
          ,DH.DeductionConfirm
          ,DH.ConfirmSource
          ,DH.ConfirmAmount
          ,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
          ,DH.InsertOperator
          ,DH.InsertDateTime
          ,DH.UpdateOperator
          ,DH.UpdateDateTime
          ,DH.DeleteOperator
          ,DH.DeleteDateTime
    FROM [D_Collect] AS DH

    WHERE DH.CollectDate >= (CASE WHEN @CollectDateFrom <> '' THEN CONVERT(DATE, @CollectDateFrom) ELSE DH.CollectDate END)
    AND DH.CollectDate <= (CASE WHEN @CollectDateTo <> '' THEN CONVERT(DATE, @CollectDateTo) ELSE DH.CollectDate END)
    AND CONVERT(DATE,DH.InputDatetime) >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE CONVERT(DATE,DH.InputDatetime) END)
    AND CONVERT(DATE,DH.InputDatetime) <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE CONVERT(DATE,DH.InputDatetime) END)
    AND (DH.InputKBN IN (1,2) OR (DH.InputKBN = 3 AND DH.AdvanceFLG = 1))

--     AND DH.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DH.StoreCD END)
    AND ISNULL(DH.CollectCustomerCD,'') = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE ISNULL(DH.CollectCustomerCD,'') END)
    AND DH.DeleteDateTime IS NULL
    ORDER BY DH.CollectDate desc, DH.CollectNO
    ;

END

GO
