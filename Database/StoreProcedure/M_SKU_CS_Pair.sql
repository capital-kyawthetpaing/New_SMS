
 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_CS_Pair]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE M_SKU_CS_Pair
	-- Add the parameters for the stored procedure here
	@AdminNo as varchar(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	 select SizeNo , ColorNo from F_SKU(getdate()) where AdminNO= @AdminNO

END
GO