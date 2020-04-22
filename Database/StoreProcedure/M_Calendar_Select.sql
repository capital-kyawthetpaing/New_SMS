 BEGIN TRY 
 Drop Procedure dbo.[M_Calendar_Select]
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
CREATE PROCEDURE [dbo].[M_Calendar_Select]
	-- Add the parameters for the stored procedure here
	@Month datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE
	 @cols NVARCHAR (MAX),
	 @cols1 NVARCHAR (MAX),
	 @days int, 
	 @query NVARCHAR(MAX)
	SELECT @days=DAY(EOMONTH(Replace(@Month,'/','-'))) 

	declare @day int=1
	while(@day<=@days)
	begin
		set @cols= COALESCE (@cols + ',ISNULL(['+Convert(varchar,@day)+'],0) as '+'['+Convert(varchar,@day)+']' , 'ISNULL(['+Convert(varchar,@day)+'],0) as '+ '['+Convert(varchar,@day)+']')
		set @cols1= COALESCE (@cols1 + ',['+Convert(varchar,@day)+']', '['+Convert(varchar,@day)+']')
		set @day=@day+1
	end
	
	set @query='

			select * from
				(SELECT top 1 '+@cols+',Flag
				 FROM 
					(
					  SELECT  day(CalendarDate)as ''day'',BankDayOff, BankDayOff as bf ,''1'' as Flag
					  FROM M_Calendar
					  where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					  and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
					) p
					PIVOT (
					 max(bf)
					  FOR [day] in ('+@cols1+')
					  ) x
					  order by BankDayOff desc) bf

				 union all 

			 select * from
				 (SELECT top 1 '+@cols+',Flag
				 FROM 
					(
					  SELECT  day(CalendarDate)as ''day'',DayOff1, DayOff1 as f1,''1'' as Flag
					  FROM M_Calendar
					  where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					  and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
					) p
					PIVOT (
					 max(f1)
					  FOR [day] in ('+@cols1+')
					  ) x
					  order by DayOff1 desc) f1

			 union all 

				select * from
				  (
				   SELECT top 1 '+@cols+',Flag
					FROM 
					(
					  SELECT  day(CalendarDate)as ''day'',DayOff2,DayOff2 as f2,''1'' as Flag
					  FROM M_Calendar
					  where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					  and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
					) p
					PIVOT (
					 max(f2)
					  FOR [day] in ('+@cols1+')
					  ) x
					  order by DayOff2 desc) f2

			union all

				select * from
				  (
				   SELECT top 1 '+@cols+',Flag
					FROM 
					(
					  SELECT  day(CalendarDate)as ''day'',DayOff3,DayOff3 as f3,''1'' as Flag
					  FROM M_Calendar
					  where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					  and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
					) p
					PIVOT (
					 max(f3)
					  FOR [day] in ('+@cols1+')
					  ) x
					  order by DayOff3 desc) f3

			union all 

			   select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff4,DayOff4 as f4,''1'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f4)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff4 desc) f4 

		   union all 

			  select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff5,DayOff5 as f5,''1'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f5)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff5 desc) f5
		 union all 

			  select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff6,DayOff6 as f6,''1'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f6)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff6 desc) f6	
					
		union all 

			  select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff7,DayOff7 as f7,''1'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f7)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff7 desc) f7 

		union all 

			  select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff8,DayOff8 as f8,''0'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f8)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff8 desc) f8
					  
		union all 

			  select * from
				(
				SELECT top 1 '+@cols+',Flag
				FROM 
				(
					SELECT  day(CalendarDate)as ''day'',DayOff9,DayOff9 as f9,''0'' as Flag
					FROM M_Calendar
					where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
					and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
				) p
				PIVOT (
					max(f9)
					FOR [day] in ('+@cols1+')
					) x
					order by DayOff9 desc) f9

		union all 

			select * from
			(
			SELECT top 1 '+@cols+',Flag
			FROM 
			(
				SELECT  day(CalendarDate)as ''day'',DayOff10,DayOff10 as f10,''0'' as Flag
				FROM M_Calendar
				where MONTH(CalendarDate) = MONTH(dateadd(dd, 0, '+''''+convert(varchar,@Month,25)+''''+'))
				and YEAR(CalendarDate)=YEAR(dateadd(YYYY, 0,'+''''+convert(varchar,@Month,25)+''''+'))
			) p
			PIVOT (
				max(f10)
				FOR [day] in ('+@cols1+')
				) x
				order by DayOff10 desc) f10
					  
					  '



	EXEC SP_EXECUTESQL @query

END


