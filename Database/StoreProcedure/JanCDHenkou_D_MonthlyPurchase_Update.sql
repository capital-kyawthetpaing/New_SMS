BEGIN TRY 
 Drop Procedure [dbo].[JanCDHenkou_D_MonthlyPurchase_Update]
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
CREATE PROCEDURE [dbo].[JanCDHenkou_D_MonthlyPurchase_Update]
	-- Add the parameters for the stored procedure here
	@xml AS XML,
	@Datetime as datetime,
	@Operator as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#temp](
	[GenJanCD] [varchar](13) NOT NULL,
	[BrandCD] [varchar](6) NOT NULL,
	[BrandName] [varchar](13) NOT NULL,
	[ITemCD] [varchar](30) NOT NULL,
	[SKUName] [varchar](40) NOT NULL,
	[SizeName] [varchar](20) NOT NULL,
	[ColorName] [varchar](20) NOT NULL,
	[GenJanCD2] [varchar](13) NOT NULL,
	[NewJanCD] [varchar](13) NOT NULL,
	[SKUCD] [varchar](40) NOT NULL,
	[AdminNO] int NOT NULL )

	DECLARE @DocHandle int
	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @xml
	
	INSERT INTO #temp SELECT * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
	WITH([GenJanCD] [varchar](13),
	[BrandCD] [varchar](6),
	[BrandName] [varchar](13),
	[ITemCD] [varchar](30),
	[SKUName] [varchar](40),
	[SizeName] [varchar](20),
	[ColorName] [varchar](20),
	[GenJanCD2] [varchar](13),
	[NewJanCD] [varchar](13),
	[SKUCD] [varchar](40),
	[AdminNO] int)
	EXEC sp_xml_removedocument @DocHandle; 

    -- Insert statements for procedure here
	Update D_MonthlyPurchase
	SET JanCD = tmp.NewJanCD,
		UpdateOperator = @Operator,
		UpdateDateTime = @Datetime
	FROM #temp tmp 
	INNER JOIN D_MonthlyPurchase dmp 
	ON dmp.JanCD = tmp.GenJanCD
	AND dmp.AdminNO = tmp.AdminNO

	Drop table #temp
END
