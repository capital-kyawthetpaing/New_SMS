 BEGIN TRY 
 Drop Procedure dbo.[M_API_InsertRefreshToken_Yahoo]
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
CREATE PROCEDURE [dbo].[M_API_InsertRefreshToken_Yahoo]
	-- Add the parameters for the stored procedure here
	@APIKey tinyint,
	@ChangeDate date,
	@Token varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE M_API SET RefreshToken = @Token
	WHERE APIKey = @APIKey
	AND ChangeDate = @ChangeDate
END

