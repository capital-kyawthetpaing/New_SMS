
/****** Object:  StoredProcedure [dbo].[SelectHonshaStore]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[SelectHonshaStore]
GO

/****** Object:  StoredProcedure [dbo].[SelectHonshaStore]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [SelectHonshaStore]    */
CREATE PROCEDURE SelectHonshaStore(
    -- Add the parameters for the stored procedure here
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
        ,MS.ZipCD1
        ,MS.ZipCD2
        ,MS.Address1
        ,MS.Address2
        ,MS.MailAddress1
        ,MS.MailAddress2
        ,MS.MailAddress3
        ,MS.TelephoneNO
        ,MS.FaxNO
        ,MS.KouzaCD
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
        ,MS.Remarks
        ,MS.DeleteFlg
        ,MS.UsedFlg
        ,MS.InsertOperator
        ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
        ,MS.UpdateOperator
        ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime

    from M_Store MS
    
    WHERE MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    AND MS.StoreKBN = 1	--ŽÀ“X•Ü
    AND MS.StorePlaceKBN = 1	--–{ŽÐ
    ORDER BY ChangeDate desc
    ;
END

GO
