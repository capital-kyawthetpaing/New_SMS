 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_KeySelect]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_KeySelect]
	-- Add the parameters for the stored procedure here
	@ID int,
	@Key varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select 
	Char1,
	Char2,
	Char3,
	Char4,
	Char5,
	Num1,
	Num2,
	Num3,
	Num4,
	Num5,
	FORMAT (Date1, 'yyyy/MM/dd HH:mm:ss ') as Date1,
	FORMAT (Date2, 'yyyy/MM/dd HH:mm:ss ') as Date2,
	FORMAT (Date3, 'yyyy/MM/dd HH:mm:ss ') as Date3
	From M_MultiPorpose mmp
	Where mmp.ID = @ID AND mmp.[Key] = @Key
END

