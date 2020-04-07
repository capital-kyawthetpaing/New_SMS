 BEGIN TRY 
 Drop Procedure dbo.[M_StoreButton_Insert_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_StoreButton_Insert_Update]
	-- Add the parameters for the stored procedure here
		@GroupXML as XML,
		@StoreCD as varchar(4),
		@MasterKBN as  tinyint,
		--------------LOG
@Operator varchar(10),
 @Program as varchar(30),
 @PC as varchar(30)
 --@KeyItem as varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#tempButtonGroup]
			(
				[GroupNO][int],
				[ButtomName][varchar](12) collate Japanese_CI_AS
			)

declare @InsertDateTime datetime=getdate();		
DECLARE @OperateDateTime datetime=getdate();
SET @OperateDateTime = SYSDATETIME();
DECLARE @OperateDate date= CONVERT (date, @OperateDateTime);
DECLARE @OperateTime time(7)= CONVERT (time, @OperateDateTime);
declare @idoc  int
--Create Table #tempButtonGroup (GroupNO   int,	ButtomName nvarchar(12))

--DECLARE @DocHandle int
	
--	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @GroupXML

--	INSERT INTO #tempButtonGroup SELECT *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
--	WITH(GroupNO   int,	ButtomName nvarchar(12))
	--EXEC sp_xml_removedocument @idoc; 
			exec sp_xml_preparedocument @idoc output, @GroupXML
			insert into #tempButtonGroup
			SELECT *  FROM openxml(@idoc,'/NewDataSet/test',2)
			WITH
			(
				GroupNO   int,
				ButtomName varchar(12) collate Japanese_CI_AS
				--MasterKBN  tinyint
			)
			exec sp_xml_removedocument @idoc

	INSERT INTO L_Log(InsertOperator,Program,PC,OperateMode,KeyItem,OperateDate,OperateTime)
	SELECT 					@Operator,@Program,@PC,'新規',(convert(varchar,tgb_L.GroupNO)+' '+tgb_L.ButtomName),@OperateDate,@OperateTime
	FROM #tempButtonGroup tgb_L
	WHERE NOT EXISTS (SELECT * FROM  M_StoreBottunGroup msb where msb.GroupNO = tgb_L.GroupNO)

	INSERT INTO M_StoreBottunGroup(StoreCD,ProgramKBN,GroupNO,BottunName,MasterKBN,InsertDateTime,UpdateOperator,UpdateDateTime)
	SELECT 								@StoreCD,'1',GroupNO,ButtomName,	@MasterKBN,	@InsertDateTime,@StoreCD,	@InsertDateTime	
	FROM #tempButtonGroup tgb
	WHERE NOT EXISTS (SELECT * FROM  M_StoreBottunGroup msb where msb.GroupNO = tgb.GroupNO)


	INSERT INTO L_Log(InsertOperator,Program,PC,OperateMode,KeyItem,OperateDate,OperateTime)
	SELECT 					@Operator,@Program,@PC,'変更',(convert(varchar,xm.GroupNO)+' '+xm.ButtomName),@OperateDate,@OperateTime
FROM M_StoreBottunGroup 
inner join #tempButtonGroup  xm 
on xm.GroupNO=M_StoreBottunGroup.GroupNO
where  not exists(select * from M_StoreBottunGroup where  M_StoreBottunGroup.BottunName=xm.ButtomName)

	UPDATE M_StoreBottunGroup 
		SET 		GroupNO= xm.GroupNO, 
		BottunName = xm.ButtomName, 
		MasterKBN= @MasterKBN, 
		--InsertDateTime = @InsertDateTime, 
		UpdateOperator =@StoreCD, 
		UpdateDateTime= @InsertDateTime 
FROM M_StoreBottunGroup 
inner join #tempButtonGroup  xm 
on xm.GroupNO=M_StoreBottunGroup.GroupNO
where  not exists(select * from M_StoreBottunGroup where  M_StoreBottunGroup.BottunName=xm.ButtomName)

Drop table #tempButtonGroup


--EXEC dbo.L_Log_Insert @Operator,@Program,@PC,'更新',@KeyItem
END

