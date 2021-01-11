 BEGIN TRY 
 Drop Procedure dbo.[M_Calendar_SelectForFB]
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
CREATE PROCEDURE [dbo].[M_Calendar_SelectForFB] 
	-- Add the parameters for the stored procedure here
	@CalendarDate Date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select mc.CalendarDate,mc.BankDayOff from M_Calendar mc
	Where mc.CalendarDate = @CalendarDate
END