 BEGIN TRY 
 Drop Procedure dbo.[D_JuChuu_DataSelect]
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
CREATE PROCEDURE [dbo].[D_JuChuu_DataSelect]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @currentdate as date = getdate()

    -- Insert statements for procedure here
	Select Count(JuchuuNo) as JuchuuNo 
	From D_Juchuu
	Where DeleteDateTime is null and JuchuuDate = @currentdate and StoreCD = @StoreCD and JuchuuKBN = 2 or JuchuuKBN = 3
END

