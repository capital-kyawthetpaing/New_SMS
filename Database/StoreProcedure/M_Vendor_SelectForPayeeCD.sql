
/****** Object:  StoredProcedure [dbo].[M_Vendor_SelectForPayeeCD]    Script Date: 6/11/2019 2:20:04 PM ******/
DROP PROCEDURE [dbo].[M_Vendor_SelectForPayeeCD]
GO

/****** Object:  StoredProcedure [dbo].[M_Vendor_SelectForPayeeCD]    Script Date: 6/11/2019 2:20:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Vendor_SelectForPayeeCD]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Vendor_SelectForPayeeCD]
	-- Add the parameters for the stored procedure here
	@VendorCD varchar(13),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  top 1 MS.VendorCD
          ,CONVERT(varchar, MS.ChangeDate,111) AS ChangeDate
          ,MS.ShoguchiFlg
          ,MS.VendorName
          ,MS.VendorLongName1
          ,MS.VendorLongName2
          ,MS.VendorPostName
          ,MS.VendorPositionName
          ,MS.VendorStaffName
          ,MS.VendorKana
          ,MS.PayeeFlg
          ,MS.MoneyPayeeFlg
          ,MS.PayeeCD
          ,MS.MoneyPayeeCD
          ,MS.ZipCD1
          ,MS.ZipCD2
          ,MS.Address1
          ,MS.Address2
          ,MS.MailAddress1
          ,MS.MailAddress2
          ,MS.MailAddress3
          ,MS.TelephoneNO
          ,MS.FaxNO
          ,MS.PaymentCloseDay
          ,MS.PaymentPlanKBN
          ,MS.PaymentPlanDay
          ,MS.HolidayKBN
          ,MS.BankCD
          ,MS.BranchCD
          ,MS.KouzaKBN
          ,MS.KouzaNO
          ,MS.KouzaMeigi
          ,MS.KouzaCD
          ,MS.TaxTiming
          ,MS.TaxFractionKBN
          ,MS.AmountFractionKBN
          ,MS.NetFlg
          ,MS.LastOrderDate
          ,MS.StaffCD
          ,MS.AnalyzeCD1
          ,MS.AnalyzeCD2
          ,MS.AnalyzeCD3
          ,MS.DisplayOrder
          ,MS.NotDisplyNote
          ,MS.DisplayNote
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    FROM M_Vendor MS
    WHERE MS.VendorCD = @VendorCD
    AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    AND MS.PayeeFlg = 1
    AND MS.DeleteFlg = 0
    ORDER BY MS.ChangeDate desc
END


GO


