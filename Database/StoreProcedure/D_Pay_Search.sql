 BEGIN TRY 
 Drop Procedure dbo.[D_Pay_Search]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_Pay_Search]
    -- Add the parameters for the stored procedure here
    @PayDateFrom as date,
    @PayDateTo as date,
    @InputDateTimeFrom as date,
    @InputDateTimeTo as date

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT
    dp.LargePayNO,
    Count(*) as NumPayee,
    Max(dp.PayDate) as PayDate,
    Max(CONVERT(varchar, dp.InputDateTime,111)) as InputDateTime,
    (SELECT top 1 fs.StaffName 
     FROM M_Staff AS fs 
     WHERE fs.StaffCD = Max(dp.StaffCD) AND fs.ChangeDate <= Max(dp.PayDate)
     ORDER BY fs.ChangeDate desc ) AS StaffName
    From D_Pay dp
    --Left Outer Join F_Staff(GetDate()) fs on fs.StaffCD = dp.StaffCD
    Where dp.DeleteDateTime is null 
    and (( @PayDateFrom is Null) or (@PayDateFrom is not Null and dp.PayDate >= @PayDateFrom))
    and ((@PayDateTo is Null) or (@PayDateTo is not Null and dp.PayDate <= @PayDateTo))
    and ((@InputDateTimeFrom is Null) or (@InputDateTimeFrom is not Null and  dp.InputDateTime >= @InputDateTimeFrom))
    and ((@InputDateTimeTo is Null) or (@InputDateTimeTo is not Null and dp.InputDateTime <= @InputDateTimeTo))
    Group by dp.LargePayNO
    Order by dp.LargePayNO Asc
END
