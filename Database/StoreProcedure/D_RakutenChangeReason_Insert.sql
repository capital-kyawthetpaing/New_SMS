 BEGIN TRY 
 Drop Procedure dbo.[D_RakutenChangeReason_Insert]
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
CREATE PROCEDURE [dbo].[D_RakutenChangeReason_Insert](
	-- Add the parameters for the stored procedure here
	@ChangeReasonXml as xml,
	@Operator as varchar(10),
	@InsertDateTime as datetime)
AS
BEGIN

CREATE TABLE [dbo].[#tempChangeReason]
	(
				InportSEQ int,
				StoreCD varchar(4) collate Japanese_CI_AS,
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50) collate Japanese_CI_AS,
				reasonRows int,
				changeId	int,
				changeType			tinyint,
				changeTypeDetail	tinyint,
				changeReason		tinyint,
				changeReasonDetail	tinyint,
				changeApplyDatetime	datetime,
				changeFixDatetime	datetime,
				changeCmplDatetime	datetime
				
			)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @ChangeReasonXml
	insert into #tempChangeReason
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				InportSEQ int,
				StoreCD varchar(4),
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50),
				reasonRows int,
				changeId	int,
				changeType			tinyint,
				changeTypeDetail	tinyint,
				changeReason		tinyint,
				changeReasonDetail	tinyint,
				changeApplyDatetime	datetime,
				changeFixDatetime	datetime,
				changeCmplDatetime	datetime
				
			)
			exec sp_xml_removedocument @DocHandle;

			INSERT INTO D_RakutenChangeReason(InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,reasonRows,changeId,changeType,changeTypeDetail,changeReason,changeReasonDetail,changeApplyDatetime,changeFixDatetime,changeCmplDatetime,
				InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime

)
			select InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,reasonRows,changeId,changeType,changeTypeDetail,changeReason,changeReasonDetail,changeApplyDatetime,changeFixDatetime,changeCmplDatetime,
			@Operator,@InsertDateTime,@Operator,@InsertDateTime
			from #tempChangeReason

			drop table #tempChangeReason
	
END

