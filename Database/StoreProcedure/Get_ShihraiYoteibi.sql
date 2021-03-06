BEGIN TRY 
Drop Procedure [dbo].[Get_ShihraiYoteibi]
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
CREATE PROCEDURE [dbo].[Get_ShihraiYoteibi]
	-- Add the parameters for the stored procedure here
	@KaisyuShiharaiKbn as tinyint,
	@CustomerCD as varchar(13),
    @ChangeDate as varchar(10),
    @TyohaKbn as tinyint
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @result as varchar(10),@Yoteibi as varchar(10);
    -- Insert statements for procedure here
	EXEC dbo.Fnc_PlanDate_SP 
		@KaisyuShiharaiKbn, 
		@CustomerCD, 
		@ChangeDate, 
		@TyohaKbn, 
		@Yoteibi OUTPUT;

	--IF ISNULL(@Yoteibi,'') = ''
 --               BEGIN
 --                   SET @Yoteibi = 0;
 --                   --RETURN @Yoteibi;
 --               END

	SELECT @Yoteibi as Yoteibi
END
GO
