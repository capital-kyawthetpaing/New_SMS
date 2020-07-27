 BEGIN TRY 
 Drop Procedure dbo.[D_Store_CalculationDelete]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE D_Store_CalculationDelete
	-- Add the parameters for the stored procedure here
	 @StoreCD as VarChar(6) ,
	 @Date as date,
	 @Operator varchar(10),
	 @Program as varchar(30),
	 @PC as varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @OperateMode as varchar(10) = null;
	Declare @KeyItem as varchar(100)= @StoreCD + Convert(varchar(10),@Date);
    -- Insert statements for procedure here
	Delete 
	From D_StoreCalculation
	Where CalculationDate = @Date and StoreCD = @StoreCD

	exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

END
GO
