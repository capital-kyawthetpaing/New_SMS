BEGIN TRY 
 Drop Procedure dbo.[GetNouki]
END try
BEGIN CATCH END CATCH 
/****** Object:  StoredProcedure [dbo].[GetNouki]    Script Date: 12/11/2019 2:00:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNouki]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(4),
    @ChangeDate	  varchar(10)	--出荷予定日
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Nouki as varchar(10);
	DECLARE @StorePlaceKBN tinyint;

    -- Insert statements for procedure here
    SELECT top 1 @StorePlaceKBN = M.StorePlaceKBN
    FROM M_Store AS M
    WHERE M.StoreCD = @StoreCD
    AND M.ChangeDate <= CONVERT(date,@ChangeDate)
    ORDER BY M.ChangeDate desc
    ;
    
    IF @StorePlaceKBN = 1
    BEGIN  
        SELECT top 1 @Nouki = CONVERT(varchar,M.CalendarDate,111)
        FROM M_Calendar AS M
        WHERE M.CalendarDate < CONVERT(date,@ChangeDate)
        AND M.DayOff1 = 0
        ORDER BY M.CalendarDate desc
        ;
    
    END
    ELSE IF @StorePlaceKBN = 2
    BEGIN
        SELECT top 1 @Nouki = CONVERT(varchar,M.CalendarDate,111)
        FROM M_Calendar AS M
        WHERE M.CalendarDate < CONVERT(date,@ChangeDate)
        AND M.DayOff2 = 0
        ORDER BY M.CalendarDate desc
        ;

    END
    ELSE IF @StorePlaceKBN = 3
    BEGIN
        SELECT top 1 @Nouki = CONVERT(varchar,M.CalendarDate,111)
        FROM M_Calendar AS M
        WHERE M.CalendarDate < CONVERT(date,@ChangeDate)
        AND M.DayOff3 = 0
        ORDER BY M.CalendarDate desc
        ;

    END
    ELSE IF @StorePlaceKBN = 4
    BEGIN
        SELECT top 1 @Nouki = CONVERT(varchar,M.CalendarDate,111)
        FROM M_Calendar AS M
        WHERE M.CalendarDate < CONVERT(date,@ChangeDate)
        AND M.DayOff4 = 0
        ORDER BY M.CalendarDate desc
        ;

    END
    ELSE IF @StorePlaceKBN = 5
    BEGIN
        SELECT top 1 @Nouki = CONVERT(varchar,M.CalendarDate,111)
        FROM M_Calendar AS M
        WHERE M.CalendarDate < CONVERT(date,@ChangeDate)
        AND M.DayOff5 = 0
        ORDER BY M.CalendarDate desc
        ;

    END

	SELECT  @Nouki as Nouki
END


GO
