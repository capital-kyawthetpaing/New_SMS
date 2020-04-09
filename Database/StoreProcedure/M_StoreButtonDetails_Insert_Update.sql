 BEGIN TRY 
 Drop Procedure dbo.[M_StoreButtonDetails_Insert_Update]
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
CREATE  PROCEDURE [dbo].[M_StoreButtonDetails_Insert_Update]
	-- Add the parameters for the stored procedure here
	@GroupDetailXML as xml,
	@StoreCD as varchar(4),
	@MasterKBN varchar(12),
	--@mode as tinyint

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
	CREATE TABLE [dbo].[#tempBtnGroupDetails]
	(
			  GroupNO tinyint,
			  Horizontal tinyint,
			  Vertical tinyint,
			  Button  varchar(13) collate Japanese_CI_AS,
			  btndetailBottunName  varchar(18) collate Japanese_CI_AS
			)

    -- Insert statements for procedure here
	declare @idoc int;
	declare @ProgramKBN as tinyint=1;
	declare @InsertDateTime as datetime=getdate();
	DECLARE @OperateDateTime datetime;
SET @OperateDateTime = SYSDATETIME();
DECLARE @OperateDate date= CONVERT (date, @OperateDateTime);
DECLARE @OperateTime time(7)= CONVERT (time, @OperateDateTime);


	exec sp_xml_preparedocument @idoc output, @GroupDetailXML
	insert into #tempBtnGroupDetails
			SELECT * FROM openxml(@idoc,'/NewDataSet/test',2)
			WITH
			(
			  GroupNO tinyint,
			  Horizontal tinyint,
			  Vertical tinyint,
			  Button  varchar(13),
			  btndetailBottunName  varchar(18)
			)
		exec sp_xml_removedocument @idoc
			--SET @KeyItem=CONVERT(VARCHAR,tgbd_L.GroupNO)+';'+CONVERT(VARCHAR,tgbd_L.Vertical)+';'+CONVERT(VARCHAR,tgbd_L.Horizontal)+';'+tgbd_L.btndetailBottunName;
	INSERT INTO  L_Log(InsertOperator,Program,PC,OperateMode,KeyItem,OperateDate,OperateTime)
	SELECT 					@Operator,@Program,@PC,'新規',(CONVERT(VARCHAR,tgbd_L.GroupNO)+':'+CONVERT(VARCHAR,tgbd_L.Horizontal)+':'+CONVERT(VARCHAR,tgbd_L.Vertical)+' '+tgbd_L.btndetailBottunName),@OperateDate,@OperateTime
	FROM #tempBtnGroupDetails tgbd_L
	WHERE NOT EXISTS (SELECT * FROM  M_StoreBottunDetails msbd where msbd.GroupNO = tgbd_L.GroupNO and  msbd.Vertical=tgbd_L.Vertical and msbd.Horizontal=tgbd_L.Horizontal)
	INSERT INTO M_StoreBottunDetails(
		StoreCD,
		ProgramKBN,
		GroupNO,
		Horizontal,
		Vertical,
		MasterKBN,
		Botton,
		AdminNO,
		BottunName,
		InsertOperator,
		InsertDateTime,
		UpdateOperator,
		UpdateDateTime
		)
		SELECT 
		@StoreCD,
		@ProgramKBN,
		GroupNO,
		Horizontal,
		Vertical,
		@MasterKBN,
		 Button,
		 @MasterKBN,
		 btndetailBottunName,
		 @StoreCD,
		 @InsertDateTime,
		 @StoreCD,
		 @InsertDateTime
		FROM #tempBtnGroupDetails tbgd
		WHERE NOT EXISTS (SELECT * FROM M_StoreBottunDetails MSBD WHERE MSBD.GroupNO=tbgd.GroupNO AND MSBD.Vertical=tbgd.Vertical AND MSBD.Horizontal=tbgd.Horizontal)
	
	INSERT INTO L_Log(InsertOperator,Program,PC,OperateMode,KeyItem,OperateDate,OperateTime)
			SELECT 					@Operator,@Program,@PC,'変更',(CONVERT(VARCHAR,xm.GroupNO)+':'+CONVERT(VARCHAR,xm.Horizontal)+':'+CONVERT(VARCHAR,xm.Vertical)+' '+xm.btndetailBottunName),@OperateDate,@OperateTime
			FROM M_StoreBottunGroup 
			INNER JOIN #tempBtnGroupDetails  xm 
			ON  xm.GroupNO=M_StoreBottunGroup.GroupNO
where  not exists(select * from M_StoreBottunDetails MSBD where  MSBD.BottunName=xm.btndetailBottunName)
	UPDATE M_StoreBottunDetails
		 SET GroupNO=xmdetails.GroupNO,
		 Horizontal=xmdetails.Horizontal,
		 Vertical=xmdetails.Vertical,
		  MasterKBN=@MasterKBN,
		 Botton=xmdetails.Button,
		 AdminNO=@MasterKBN,
		 BottunName=xmdetails.btndetailBottunName,
		UpdateOperator=@StoreCD,
		UpdateDateTime=@InsertDateTime
		 FROM #tempBtnGroupDetails xmdetails	
		 INNER JOIN M_StoreBottunDetails MSBD
		 ON  MSBD.GroupNO=xmdetails.GroupNO
		 and MSBD.Horizontal=xmdetails.Horizontal
		 and MSBD.Vertical=xmdetails.Vertical
		 where  not exists(select * from M_StoreBottunDetails MSBD where  MSBD.BottunName=xmdetails.btndetailBottunName)
		--end
DROP TABLE #tempBtnGroupDetails
END

