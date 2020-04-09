 BEGIN TRY 
 Drop Procedure dbo.[M_Control_RecordCheck]
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
CREATE PROCEDURE [dbo].[M_Control_RecordCheck]
 @RecordDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * 
	FROM M_Control c 
	INNER JOIN M_FiscalYear fy ON fy.FiscalYear = c.FiscalYear
	WHERE c.MainKey = 1 
	AND fy.InputPossibleStartDate <= @RecordDate
	AND fy.InputPossibleEndDate >= @RecordDate
END

