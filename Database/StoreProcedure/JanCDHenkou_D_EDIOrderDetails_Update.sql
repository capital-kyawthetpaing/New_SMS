 BEGIN TRY 
 Drop Procedure [dbo].[JanCDHenkou_D_EDIOrderDetails_Update]
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
CREATE PROCEDURE [dbo].[JanCDHenkou_D_EDIOrderDetails_Update]
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
	[BrandCD] [varchar](6)  NULL,
	[BrandName] [varchar](13)  NULL,
	[ITemCD] [varchar](30)  NULL,
	[SKUName] [varchar](40)  NULL,
	[SizeName] [varchar](20)  NULL,
	[ColorName] [varchar](20)  NULL,
	[GenJanCD2] [varchar](13)  NULL,
	[NewJanCD] [varchar](13) NOT NULL,
	[SKUCD] [varchar](40)  NULL,
	[AdminNO] int NULL 
	CONSTRAINT [PK_temp] PRIMARY KEY CLUSTERED
	(
	[GenJanCD] ASC,
	[NewJanCD] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

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
	Update D_EDIOrderDetails
	SET JanCD = tmp.NewJanCD ,
		UpdateOperator = @Operator,
		UpdateDateTime = @Datetime
	FROM #temp tmp 
	INNER JOIN D_EDIOrderDetails ded 
	ON ded.JanCD = tmp.GenJanCD
	AND ded.AdminNO = tmp.AdminNO

	Drop table #temp
END
