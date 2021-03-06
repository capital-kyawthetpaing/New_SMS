 BEGIN TRY 
 Drop Procedure dbo.[D_Exclusive_DeleteForSiharai]
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
CREATE PROCEDURE D_Exclusive_DeleteForSiharai
	-- Add the parameters for the stored procedure here
	@DataKBN tinyint,
    @Number varchar(20),
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
	Where DataKBN =@DataKBN
	AND Number = @Number
	AND Operator=@Operator
	AND Program =@Program
	AND PC =@PC
END
GO