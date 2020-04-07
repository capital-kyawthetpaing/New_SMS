 BEGIN TRY 
 Drop Procedure dbo.[M_Calendar_Update]
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
CREATE PROCEDURE [dbo].[M_Calendar_Update]
	-- Add the parameters for the stored procedure here
	@DayOffXml as xml,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#tempDayOff]
	(
				CalendarDate Date,
				BankDayOff Int,
				DayOff1 Int,
				DayOff2 Int,
				DayOff3 Int,
				DayOff4 Int,
				DayOff5 Int,
				DayOff6 Int,
				DayOff7 Int,
				DayOff8 Int,
				DayOff9 Int
	)
	declare @DocHandle int
	exec sp_xml_preparedocument @DocHandle output, @DayOffXml
	insert into #tempDayOff
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				CalendarDate Date,
				BankDayOff Int,
				DayOff1 Int,
				DayOff2 Int,
				DayOff3 Int,
				DayOff4 Int,
				DayOff5 Int,
				DayOff6 Int,
				DayOff7 Int,
				DayOff8 Int,
				DayOff9 Int
			)
			exec sp_xml_removedocument @DocHandle;

			update mc
			set BankDayOff=tday.BankDayOff,
			DayOff1=tday.DayOff1,
			DayOff2=tday.DayOff2,
			DayOff3=tday.DayOff3,
			DayOff4=tday.DayOff4,
			DayOff5=tday.DayOff5,
			DayOff6=tday.DayOff6,
			DayOff7=tday.DayOff7,
			DayOff8=tday.DayOff8,
			DayOff9=tday.DayOff9
			from M_Calendar as mc inner join #tempDayOff as tday
			on mc.CalendarDate=tday.CalendarDate

			--log insert
			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

