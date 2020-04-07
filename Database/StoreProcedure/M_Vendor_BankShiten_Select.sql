 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_BankShiten_Select]
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
CREATE PROCEDURE [dbo].[M_Vendor_BankShiten_Select]
	-- Add the parameters for the stored procedure here
	@BankCD varchar(4),
	@ChangeDate date,
	@BranchCD varchar(3)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select mbs.BranchCD,mbs.BranchName From M_BankShiten mbs 
	inner join (select * from F_BankShiten(@BankCD,cast(@ChangeDate as varchar(10)))) fbs on mbs.BranchCD = fbs.BranchCD 
	Where mbs.BankCD = @BankCD
	AND mbs.BranchCD = @BranchCD
	AND mbs.DeleteFlg = 0
	AND mbs.ChangeDate <= @ChangeDate
END
