 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Bank_Select]
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
CREATE PROCEDURE [dbo].[M_Vendor_Bank_Select]
	-- Add the parameters for the stored procedure here
	@BankCD varchar(4),
	@ChangeDate date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select m.BankCD,m.BankName From M_Bank m 
	inner join (select * from F_Bank(cast(@ChangeDate as varchar(10)))) fb on m.BankCD = fb.BankCD
	Where m.BankCD = @BankCD
	AND m.DeleteFlg = 0
	AND m.ChangeDate <= @ChangeDate

END

