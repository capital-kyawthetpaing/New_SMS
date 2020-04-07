 BEGIN TRY 
 Drop Procedure dbo.[M_Control_PaymentSelect]
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
CREATE PROCEDURE [dbo].[M_Control_PaymentSelect]
	-- Add the parameters for the stored procedure here
	@InputPossibleStartDate as Date,
	@InputPossibleEndDate as Date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * From M_Control mc
	Inner Join M_FiscalYear mf on mf.FiscalYear = mc.FiscalYear
	Where mc.[MainKey] = '1'
	and mf.InputPossibleStartDate <= @InputPossibleStartDate
	and mf.InputPossibleEndDate >= @InputPossibleEndDate

END

