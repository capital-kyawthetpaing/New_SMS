BEGIN TRY 
 Drop Procedure [dbo].[M_OrderRate_Update]
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
CREATE  PROCEDURE [dbo].[M_OrderRate_Update]
@delXml xml,
@insertXml xml,
@VendorCD as varchar(13),
@StoreCD as varchar(4),
@ChangeDate as date,
@Rate as varchar(50),
@Operator as varchar(10),
@Program as varchar(100),
@PC as varchar(30),
@OperateMode as varchar(50),
@KeyItem as varchar(100)

AS
BEGIN
	--CREATE TABLE [dbo].[#temp](
 --   [KeySEQ][int],
	--[VendorCD][varchar](13) collate Japanese_CI_AS  NULL,
	--[StoreCD][varchar](4) collate Japanese_CI_AS  NULL,
	--[BrandCD][varchar](6) collate Japanese_CI_AS  NULL,
	--[SportsCD] [varchar] (6) collate Japanese_CI_AS  NULL,
	--[SegmentCD] [varchar] (6) collate Japanese_CI_AS  NULL,
	--[LastYearTerm] [varchar] (6) collate Japanese_CI_AS  NULL,
	--[LastSeason] [varchar] (6) collate Japanese_CI_AS  NULL,
	--[ChangeDate][date],
	----[Rate][varchar](50)
	--[Rate] [decimal](5, 2) NOT NULL CONSTRAINT [DF_M_OrderRate_Rate]  DEFAULT ((0))
	
	--)
	----CONSTRAINT [PK_temp] PRIMARY KEY CLUSTERED
	----(
	--[KeySEQ] ASC
	--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	--) ON [PRIMARY]
	DECLARE @DocDel int
	EXEC sp_xml_preparedocument @DocDel OUTPUT, @delXml

	SELECT *
    Into #tmpdel
	FROM OPENXML (@DocDel, '/NewDataSet/test',2)
	WITH(
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4)
	)
	EXEC sp_xml_removedocument @DocDel; 
	--Delete from M_OrderRate where VendorCD=@VendorCD and StoreCD=@StoreCD and ChangeDate=@ChangeDate and Rate=@Rate
	Delete m
    FROM M_OrderRate as m
    INNER JOIN #tmpdel as d
    ON d.VendorCD=m.VendorCD
    WHERE d.VendorCD=m.VendorCD
    and d.StoreCD=m.StoreCD

	DECLARE @DocHandle int	
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @insertXml

	 SELECT * 
	  INTO #temp  
	  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
	WITH(
	--KeySEQ int,
	VendorCD VARCHAR(13),
	StoreCD VARCHAR(4)
	,
	BrandCD VARCHAR(6),
	SportsCD VARCHAR(6),
	SegmentCD VARCHAR(6),
	LastYearTerm VARCHAR(6),
	LastSeason VARCHAR(6),
	ChangeDate Date,
	Rate Decimal(5,2)
	)
	EXEC sp_xml_removedocument @DocHandle; 

	--Delete #temp where #temp.VendorCD IS NULL
	--Select * from #temp
	--Delete from M_OrderRate where VendorCD=@VendorCD and StoreCD=@StoreCD

	--begin
	declare @currentDate as datetime = getdate();
	
	Insert 
	Into M_OrderRate
	Select 
	 tm.VendorCD,
	 tm.StoreCD,
	 tm.BrandCD,
	 tm.SportsCD,
	 tm.SegmentCD,
	 tm.LastYearTerm,
	 tm.LastSeason,
	 tm.ChangeDate,
	 tm.Rate,
	 @Operator as 'InsertOperator',
	 @currentDate  as 'InsertDateTime',
	 @Operator as 'UpdateOperator',
	 @currentDate  as 'UpdateDateTime'


	From #temp  as  tm
	
	Insert into M_OrderRate(VendorCD,StoreCD,BrandCD,SportsCD,SegmentCD,LastYearTerm,LastSeason,ChangeDate,Rate,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
	Values
	(
	 @VendorCD,
	 @StoreCD,
	 NULL,
	 NULL,
	 NULL,
	 NULL,
	 NULL,
	 @ChangeDate,
	 @Rate,
	 NULL,
	 NULL,
	 NULL,
	 NULL
	)
	--end
	drop table #temp
	drop table #tmpdel

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem


END
