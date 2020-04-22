 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_ZipCodeSelect]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Vendor_ZipCodeSelect]
	-- Add the parameters for the stored procedure here
	@VendorCD as varchar(13),
	@ChangeDate as date,
	@ZipCode1 as varchar(3),
	@ZipCode2 as varchar(4)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from M_Vendor where VendorCD= @VendorCD 
	and ChangeDate = @ChangeDate
	and ZipCD1 = @ZipCode1 
	and ZipCD2= @ZipCode2
END
