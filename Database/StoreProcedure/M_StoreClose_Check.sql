 BEGIN TRY 
 Drop Procedure dbo.[M_StoreClose_Check]
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
CREATE PROCEDURE [dbo].[M_StoreClose_Check]
	-- Add the parameters for the stored procedure here
@Mode as TINYINT,
@StoreCD as VARCHAR(4),
@YYYYMM as INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Mode=1
	begin
		select FiscalYYYYMM,ClosePosition1,ClosePosition3
		from M_StoreClose
		where (StoreCD=@StoreCD OR FiscalYYYYMM<@YYYYMM OR (FiscalYYYYMM=@YYYYMM and ClosePosition1=0 and ClosePosition3=0))
	end

	else if @Mode=2
	begin
		select FiscalYYYYMM,ClosePosition1,ClosePosition3
		from M_StoreClose
		where (StoreCD=@StoreCD OR FiscalYYYYMM<@YYYYMM OR (FiscalYYYYMM=@YYYYMM and ClosePosition2=0 and ClosePosition4=0))
	end

	else if @Mode=3
	begin
	   select FiscalYYYYMM,ClosePosition2,ClosePosition5
	   from M_StoreClose
	   where (StoreCD=@StoreCD OR FiscalYYYYMM<@YYYYMM OR (FiscalYYYYMM=@YYYYMM and ClosePosition2=0 and ClosePosition5=0))
	end

END

