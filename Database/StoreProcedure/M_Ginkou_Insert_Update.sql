 BEGIN TRY 
 Drop Procedure dbo.[M_Ginkou_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Ginkou_Insert_Update] 
	-- Add the parameters for the stored procedure here
	

	@BankCD as varchar(4),
      @ChangeDate as date,
      @BankName as varchar(30),
      @BankKana as varchar(30),
      @Remarks as varchar(500),
      @DeleteFlg as tinyint,
      @UsedFlg as tinyint,
      @InsertOperator as varchar(10),
	  @Program as varchar(30),
	  @PC as varchar(30),
	  @OperateMode as varchar(10),
	  @KeyItem as varchar(100),
      @Mode  as tinyint -- 1 is for insert and 2 is for update


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
declare @currentDate as datetime = getdate();

begin try
	


		if @Mode = 1--insert mode
				begin	
                    insert into M_bank (BanKCD, ChangeDate,BankName,BankKana, Remarks, DeleteFlg, UsedFlg, InsertOperator,InsertDateTime, UpdateOperator, UpdateDateTime )
                     values( @BankCD,@ChangeDate,@BankName,@BankKana,@Remarks,@DeleteFlg,@UsedFlg,  @InsertOperator,@currentDate,@InsertOperator,@currentDate)

				end
	    else if @Mode = 2--update mode
				begin


					Update M_Bank Set BankName= @BankName, BankKana = @BanKKana, Remarks =@Remarks , DeleteFlg = @DeleteFlg , UpdateOperator = @InsertOperator, UpdateDateTime = @currentDate 
					where BankCD =@BankCD and ChangeDate = @ChangeDate
				End

				
			exec dbo.L_Log_Insert @InsertOperator,@Program,@PC,@OperateMode,@KeyItem
		
	end try
	begin catch
		declare @ErrorNumber INT = ERROR_NUMBER();
		declare @ErrorLine INT = ERROR_LINE();
		declare @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		declare @ErrorSeverity INT = ERROR_SEVERITY();
		declare @ErrorState INT = ERROR_STATE();

	

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	end catch


END


