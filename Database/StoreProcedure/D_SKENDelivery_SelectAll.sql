 BEGIN TRY 
 Drop Procedure dbo.[D_SKENDelivery_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[D_SKENDelivery_SelectAll]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	dskend.SKENNouhinshoNO,
	CONVERT(varchar,dskend.ImportDateTime,111)+' '+SUBSTRING(CONVERT(varchar,dskend.ImportDateTime,108),1,5) AS ImportDateTime,
	dskend.VendorCD,
	dskend.VendorCD +' '+fv.VendorName as 'Vendor',
	dskend.ImportDetailsSu,
	dskend.ErrorSu,
	dskend.ImportFile
	
	from D_SKENDelivery as dskend 
	left join F_Vendor(GETDATE()) as fv on dskend.VendorCD=fv.VendorCD
	Order by dskend.SKENNouhinshoNO desc
END

