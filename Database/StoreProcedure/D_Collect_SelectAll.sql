 BEGIN TRY 
 Drop Procedure dbo.[D_Collect_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Collect_SelectAll]    */
CREATE PROCEDURE D_Collect_SelectAll(
    -- Add the parameters for the stored procedure here
    @CollectDateFrom  varchar(10),
    @CollectDateTo    varchar(10),
    @DateFrom         varchar(10),
    @DateTo           varchar(10),
    @StoreCD          varchar(4),
    @StaffCD          varchar(10),
    @CustomerCD       varchar(13),
    @WebCollectType   varchar(13),
    @ChkZan TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --Žc‚ ‚è
    IF @ChkZan = 1
    BEGIN
        SELECT DH.CollectNO
              ,DH.InputKBN
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
              ,DH.ConfirmSource
              ,DH.ConfirmAmount
              ,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
              ,DH.InsertOperator
              ,DH.InsertDateTime
              ,DH.UpdateOperator
              ,DH.UpdateDateTime
              ,DH.DeleteOperator
              ,DH.DeleteDateTime
              ,(SELECT top 1 D.ConfirmNO
                FROM D_PaymentConfirm AS D
                WHERE D.CollectNO = DH.CollectNO
                AND D.DeleteDateTime IS NULL
                ORDER BY D.ConfirmDateTime desc
                ) AS ConfirmNO

          FROM [D_Collect] AS DH

            WHERE DH.CollectDate >= (CASE WHEN @CollectDateFrom <> '' THEN CONVERT(DATE, @CollectDateFrom) ELSE DH.CollectDate END)
            AND DH.CollectDate <= (CASE WHEN @CollectDateTo <> '' THEN CONVERT(DATE, @CollectDateTo) ELSE DH.CollectDate END)
            AND CONVERT(DATE,DH.InputDatetime) >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE CONVERT(DATE,DH.InputDatetime) END)
            AND CONVERT(DATE,DH.InputDatetime) <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE CONVERT(DATE,DH.InputDatetime) END)

            AND DH.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DH.StoreCD END)
            AND DH.CollectCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CollectCustomerCD END)
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND ISNULL(DH.WebCollectType,'') = (CASE WHEN @WebCollectType <> '' THEN @WebCollectType ELSE ISNULL(DH.WebCollectType,'') END)
            AND DH.ConfirmSource - DH.ConfirmAmount > 0
            AND DH.DeleteDateTime IS NULL
            AND (DH.InputKBN IN (1,2) OR (DH.InputKBN = 3 AND DH.AdvanceFLG = 1))
            
        ORDER BY DH.CollectDate desc,DH.CollectNO
        ;
    END  
    ELSE
    BEGIN
        SELECT DH.CollectNO
              ,DH.InputKBN
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
              ,DH.ConfirmSource
              ,DH.ConfirmAmount
              ,DH.ConfirmSource - DH.ConfirmAmount AS ConfirmZan
              ,DH.InsertOperator
              ,DH.InsertDateTime
              ,DH.UpdateOperator
              ,DH.UpdateDateTime
              ,DH.DeleteOperator
              ,DH.DeleteDateTime
              ,(SELECT top 1 D.ConfirmNO
                FROM D_PaymentConfirm AS D
                WHERE D.CollectNO = DH.CollectNO
                AND D.DeleteDateTime IS NULL
                ORDER BY D.ConfirmDateTime desc
                ) AS ConfirmNO
                
          FROM [D_Collect] AS DH

            WHERE DH.CollectDate >= (CASE WHEN @CollectDateFrom <> '' THEN CONVERT(DATE, @CollectDateFrom) ELSE DH.CollectDate END)
            AND DH.CollectDate <= (CASE WHEN @CollectDateTo <> '' THEN CONVERT(DATE, @CollectDateTo) ELSE DH.CollectDate END)
            AND CONVERT(DATE,DH.InputDatetime) >= (CASE WHEN @DateFrom <> '' THEN CONVERT(DATE, @DateFrom) ELSE CONVERT(DATE,DH.InputDatetime) END)
            AND CONVERT(DATE,DH.InputDatetime) <= (CASE WHEN @DateTo <> '' THEN CONVERT(DATE, @DateTo) ELSE CONVERT(DATE,DH.InputDatetime) END)

            AND DH.StoreCD = (CASE WHEN @StoreCD <> '' THEN @StoreCD ELSE DH.StoreCD END)
            AND DH.CollectCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CollectCustomerCD END)
            AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
            AND ISNULL(DH.WebCollectType,'') = (CASE WHEN @WebCollectType <> '' THEN @WebCollectType ELSE ISNULL(DH.WebCollectType,'') END)
--            AND DH.ConfirmSource - DH.ConfirmAmount > 0
            AND DH.DeleteDateTime IS NULL
            AND (DH.InputKBN IN (1,2) OR (DH.InputKBN = 3 AND DH.AdvanceFLG = 1))

        ORDER BY DH.CollectDate desc,DH.CollectNO
    
    END
END

GO
