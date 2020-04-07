 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_Select_ByJanCD]
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
CREATE PROCEDURE [dbo].[M_SKU_Select_ByJanCD]
	-- Add the parameters for the stored procedure here

@JanCD varchar(13)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here


	select JanCD,SKUCD,SKUName,SizeName,ColorName
	from F_SKU(cast(convert(Date,GETDATE()) as varchar(10)))
	where JanCD = @JanCD

END

