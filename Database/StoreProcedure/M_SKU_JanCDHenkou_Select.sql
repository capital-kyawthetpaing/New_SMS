 BEGIN TRY 
 Drop Procedure dbo.[M_SKU_JanCDHenkou_Select]
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
CREATE  PROCEDURE [dbo].[M_SKU_JanCDHenkou_Select]
	-- Add the parameters for the stored procedure here
	@xml AS XML
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#temp](
	[現JANCD] [varchar](13) NOT NULL,
	[新JANCD] [varchar](13) NOT NULL
	
	CONSTRAINT [PK_temp] PRIMARY KEY CLUSTERED
	(
	[現JANCD] ASC,
	[新JANCD] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	DECLARE @DocHandle int
	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @xml
	
	INSERT INTO #temp SELECT * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
	WITH([現JANCD] VARCHAR(13),[新JANCD] VARCHAR(13))
	EXEC sp_xml_removedocument @DocHandle; 

	SELECT 
	tmp.現JANCD AS GenJanCD,
	sku.BrandCD AS BrandCD,
	(select BrandName from M_Brand where BrandCD = sku.BrandCD) AS BrandName,
	sku.ITemCD AS ITemCD,
	sku.SKUName AS SKUName,
	sku.SizeName As SizeName,
	SKU.ColorName AS ColorName,
	tmp.現JANCD AS GenJanCD2,
	tmp.新JANCD AS NewJanCD,
	sku.SKUCD AS SKUCD,
	sku.AdminNO AS AdminNO
	FROM #temp tmp 
	INNER JOIN F_SKU(Getdate()) sku ON sku.JanCD = tmp.現JANCD

	DROP TABLE #temp
END
