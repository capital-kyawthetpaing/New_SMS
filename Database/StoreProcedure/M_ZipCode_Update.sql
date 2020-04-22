 BEGIN TRY 
 Drop Procedure dbo.[M_ZipCode_Update]
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
CREATE PROCEDURE [dbo].[M_ZipCode_Update]
	-- Add the parameters for the stored procedure here
	@Xml xml,
	@Zip1From varchar(3) ,
	@Zip2From varchar(3),
	@Zip1To varchar(4),
	@Zip2To varchar(4),
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    CREATE TABLE [dbo].[#temp](
	[ZipCD1] [varchar](3) NOT NULL,
	[ZipCD2] [varchar](4)  NOT NULL,
	[Address1] [varchar] (80) collate Japanese_CI_AS NULL,
	[Address2] [varchar] (80) collate Japanese_CI_AS NULL
	CONSTRAINT [PK_temp] PRIMARY KEY CLUSTERED
	(
	[ZipCD1] ASC,
	[ZipCD2] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

    declare @currentDate as datetime = getdate();
	DECLARE @DocHandle int
	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @Xml
	
	INSERT INTO #temp SELECT * FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
	WITH(ZipCD1 VARCHAR(3),ZipCD2 VARCHAR(4),Address1 VARCHAR(80),Address2 VARCHAR(80))
	EXEC sp_xml_removedocument @DocHandle; 

	Delete #temp where #temp.ZipCD1 IS NULL

	IF @Zip1From IS NULL AND @Zip1To IS NULL AND @Zip2From IS NULL AND @Zip2To IS NULL
	begin
		Delete M_ZipCode 

		Insert into M_ZipCode(ZipCD1,ZipCD2,Address1,Address2,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
		select *,
		@Operator,
		@currentDate,
		@Operator,
		@currentDate 
		from #temp 
		Order By #temp.ZipCD1, #temp.ZipCD2
	end

	ELSE IF @Zip1From IS NOT NULL AND @Zip1To IS NOT NULL AND @Zip2From IS NOT NULL AND @Zip2To IS NOT NULL
	begin 
		Delete  M_ZipCode 
		Where ZipCD1 >= @Zip1From
		AND ZipCD1 <= @Zip1To
		AND ZipCD2 >= @Zip2From
		AND ZipCD2 <= @Zip2To

		Insert into M_ZipCode(ZipCD1,ZipCD2,Address1,Address2,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
		Select *,
		@Operator,
		@currentDate,
		@Operator,
		@currentDate 
		from #temp 
		order by #temp.ZipCD1, #temp.ZipCD2
	End

	drop table #temp

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

