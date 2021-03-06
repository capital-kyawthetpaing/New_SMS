BEGIN TRY 
 Drop Procedure [dbo].[D_Exclusive_DeleteByKBN]
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
Create PROCEDURE [dbo].[D_Exclusive_DeleteByKBN]
	-- Add the parameters for the stored procedure here
	@Operator as varchar(10),
	@Program as varchar(100),
	@PC as varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Delete from D_Exclusive
	Where DataKBN =9
	AND Operator=@Operator
	AND Program =@Program
	AND PC =@PC
END
