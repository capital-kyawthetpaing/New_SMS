 BEGIN TRY 
 Drop Procedure dbo.[D_Picking_Update]
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
CREATE PROCEDURE [dbo].[D_Picking_Update]
	-- Add the parameters for the stored procedure here
	@PickingNo varchar(11),
	@Operator varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	declare
	@Datetime datetime=getdate()

	Update D_Picking
	set PrintDateTime=@Datetime,
	PrintStaffCD=@Operator,
	UpdateDateTime=@Datetime,
	UpdateOperator=@Operator
	where PickingNO=@PickingNo

END

