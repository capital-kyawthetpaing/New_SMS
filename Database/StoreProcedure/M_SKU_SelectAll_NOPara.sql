BEGIN TRY 
 Drop Procedure [dbo].[M_SKU_SelectAll_NOPara]
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
Create PROCEDURE [dbo].[M_SKU_SelectAll_NOPara]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- For Tenzitest
	SELECT  JanCD,SKUCD,AdminNO  From F_SKU(getdate()) where  SetKBN=0
END
