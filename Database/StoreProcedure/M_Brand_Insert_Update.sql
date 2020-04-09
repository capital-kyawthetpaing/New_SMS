 BEGIN TRY 
 Drop Procedure dbo.[M_Brand_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Brand_Insert_Update]
	-- Add the parameters for the stored procedure here
	 @Brand as varchar(6),
     @BrandName as varchar(40),
	 @BrandKana as varchar(20), 
	 @Operator as varchar(10),
	 @Program as varchar(30),
	 @PC as varchar(30),
	 @OperateMode as varchar(10),
	 @KeyItem as varchar(100),
	 @Mode as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @currentDate as datetime = getdate();

    -- Insert statements for procedure here
	if @Mode = 1--insert mode
				begin										
					insert into M_Brand
					(
						BrandCD,
						BrandName,
						BrandKana,
						UsedFlg,
						InsertOperator,
						InsertDateTime,
						UpdateOperator,
						UpdateDateTime
					)
					values
					(
						@Brand ,
						@BrandName,
						@BrandKana,
						0,
						@Operator,
						@currentDate,
						@Operator,
						@currentDate
					)
				end
	 else if @Mode = 2--update mode
				begin
					update M_Brand
					set
						BrandCD = @Brand,
						BrandName = @BrandName,
						BrandKana=@BrandKana,					  					
						UpdateOperator= @Operator,
						UpdateDateTime = @currentDate

					where BrandCD = @Brand
						
				end

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
END

