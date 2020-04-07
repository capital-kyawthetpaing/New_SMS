 BEGIN TRY 
 Drop Procedure dbo.[M_Calendar_SelectDayKBN]
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
CREATE PROCEDURE [dbo].[M_Calendar_SelectDayKBN]
	-- Add the parameters for the stored procedure here
	@Date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	SELECT  day(CalendarDate)as 'Day',DayKBN
		FROM M_Calendar
		where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, convert(varchar,@Date,25)))
		and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0, convert(varchar,@Date,25)))
		order by Day asc


END

