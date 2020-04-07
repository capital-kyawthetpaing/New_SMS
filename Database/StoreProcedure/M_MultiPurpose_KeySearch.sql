 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPurpose_KeySearch]
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
CREATE PROCEDURE [dbo].[M_MultiPurpose_KeySearch] 
	-- Add the parameters for the stored procedure here
	@ID int,
	@KeyFrom varchar(50),
	@KeyTo varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Select 	
	[Key],
	Char1,
	Char2,
	Char3,
	Num1,
	Num2,
	Num3,
	Date1
	From M_MultiPorpose 
	Where ID = @ID AND
	(@KeyFrom is null or[Key] >= @KeyFrom )AND
	(@KeyTo is null or[Key] <= @KeyTo)
	Order by [Key]

END

