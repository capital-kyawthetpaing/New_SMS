 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_Delete]
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
CREATE PROCEDURE [dbo].[M_Souko_Delete]
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(6),
	@ChangeDate date,
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

    -- Insert statements for procedure here
	begin try
		begin transaction

			delete from M_Souko
			where SoukoCD = @SoukoCD
			and ChangeDate = @ChangeDate

			delete from M_Location
			where SoukoCD = @SoukoCD
			and ChangeDate = @ChangeDate

			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem
		commit transaction
	end try
	begin catch
		declare @ErrorNumber INT = ERROR_NUMBER();
		declare @ErrorLine INT = ERROR_LINE();
		declare @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		declare @ErrorSeverity INT = ERROR_SEVERITY();
		declare @ErrorState INT = ERROR_STATE();

		IF @@TRANCOUNT > 0
          rollback transaction;

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	end catch
END


