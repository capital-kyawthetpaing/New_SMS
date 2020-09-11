-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Alter PROCEDURE JuuChuuBiCheck 
	-- Add the parameters for the stored procedure here
	
	@Bi as Date 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			 if  (select COunt(*) from M_Calendar where CalendarDate= @Bi) = 0
			  Begin select 'E103' Result End else if (select Count (*) 
			  from M_Control mc inner Join M_FiscalYear mf on mc.FiscalYear= mf.FiscalYear where  mc.MainKey = 1 
			  and mf.InputPossibleStartDate <= @Bi and  mf.InputPossibleEndDate >= @Bi) = 0 
			  Begin 
			  select 'E115' as Result End
			  else
			  begin
			  select 'OK' as Result
			  End
END
GO
