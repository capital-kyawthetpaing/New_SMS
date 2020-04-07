 BEGIN TRY 
 Drop Procedure dbo.[M_Prefix_Display]
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
CREATE PROCEDURE [dbo].[M_Prefix_Display] (
	-- Add the parameters for the stored procedure here
	@StoreCD VARCHAR(4),
	@SeqKBN  TINYINT,
	@ChangeDate Date
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Prefix] 
	FROM M_Prefix
	WHERE StoreCD = @StoreCD
	AND SeqKBN = @SeqKBN 
	AND ChangeDate = @ChangeDate
	;
END



