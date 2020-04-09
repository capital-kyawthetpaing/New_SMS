 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Souko_Insert_Update]
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@ChangeDate date,
	@SoukoName varchar(40),
	@StoreCD varchar(4),
	@HikiateOrder tinyint,
	@SoukoType tinyint,
	@UnitPriceCalcKBN tinyint,
	@MakerCD varchar(13),
	@IdouCount tinyint,
	@ZipCD1 varchar(3),
	@ZipCD2 varchar(4),
	@Address1 varchar(100),
	@Address2 varchar(100),
	@TelephoneNO varchar(15),
	@FaxNO varchar(15),
	@Remarks varchar(500),
	@DeleteFlg tinyint,
	@Operator varchar(10),
	@LocationXml as xml,
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100),
	@Mode as tinyint-- 1 - insert, 2 - update
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	CREATE TABLE [dbo].[#T_Location]
	(	 TanaCD[varchar](10)  collate Japanese_CI_AS)

	declare @currentDate as datetime = getdate();

			if @Mode = 1--insert mode
				begin	
					--souko insert				
					insert into M_Souko
					(
						SoukoCD,
						ChangeDate,
						SoukoName,
						StoreCD,
						HikiateOrder,
						SoukoType,
						UnitPriceCalcKBN,
						MakerCD,
						IdouCount,
						ZipCD1,
						ZipCD2,
						Address1,
						Address2,
						TelephoneNO,
						FaxNO,
						Remarks,
						DeleteFlg,
						InsertOperator,
						InsertDateTime,
						UpdateOperator,
						UpdateDateTime
					)
					values
					(
						@SoukoCD,
						@ChangeDate,
						@SoukoName,
						@StoreCD,
						@HikiateOrder,
						@SoukoType,
						@UnitPriceCalcKBN,
						@MakerCD,
						@IdouCount,
						@ZipCD1,
						@ZipCD2,
						@Address1,
						@Address2,
						@TelephoneNO,
						@FaxNO,
						@Remarks,
						@DeleteFlg,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)
				end
			else if @Mode = 2--update mode
				begin
					update M_Souko
					set
						SoukoCD = @SoukoCD,
						ChangeDate = @ChangeDate,
						SoukoName = @SoukoName,
						StoreCD = @StoreCD,
						HikiateOrder = @HikiateOrder,
						SoukoType= @SoukoType,
						UnitPriceCalcKBN= @UnitPriceCalcKBN,
						MakerCD= @MakerCD,
						IdouCount= @IdouCount,
						ZipCD1= @ZipCD1,
						ZipCD2= @ZipCD2,
						Address1= @Address1,
						Address2= @Address2,
						TelephoneNO= @TelephoneNO,
						FaxNO= @FaxNO,
						Remarks= @Remarks,
						DeleteFlg= @DeleteFlg,
						InsertOperator= @Operator,
						InsertDateTime= @currentDate,
						UpdateOperator= @Operator,
						UpdateDateTime = @currentDate
					where SoukoCD=@SoukoCD
						and ChangeDate=@ChangeDate

					delete from M_Location
					where SoukoCD = @SoukoCD 
					and ChangeDate = @ChangeDate						
				end

			declare @DocHandle int
					
			exec sp_xml_preparedocument @DocHandle OUTPUT, @LocationXml
					
			--location insert
			insert into #T_Location(TanaCD)
			select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				TanaCD VARCHAR(10)		 
			)
			exec sp_xml_removedocument @DocHandle;
			
			delete from M_Location
			where SoukoCD = @SoukoCD
			and ChangeDate = @ChangeDate

			insert into M_Location
			(
				SoukoCD,
				TanaCD,
				ChangeDate,
				DeleteFlg,			
				InsertOperator,
				InsertDateTime,
				UpdateOperator,
				UpdateDateTime
			)
			select
				@SoukoCD,
				tl.TanaCD,
				@ChangeDate,
				@DeleteFlg,	
				@Operator,
				@currentDate,
				@Operator,
				@currentDate
			from #T_Location tl

			where tl.TanaCD is not null
					
			--log insert
			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

		DROP TABLE #T_Location
END

