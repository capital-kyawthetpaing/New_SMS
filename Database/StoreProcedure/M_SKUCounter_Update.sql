BEGIN TRY 
 Drop Procedure [dbo].[M_SKUCounter_Update]
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
Create PROCEDURE [dbo].[M_SKUCounter_Update]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#TAdminNo](
	[AdminNO] [int] NULL,)


	Insert into #TAdminNo
	select AdminNO 
	from M_SKUCounter
	Where MainKEY=1
	

	Update M_SKUCounter
	set AdminNO=tp.AdminNO +1
	from #TAdminNo as tp
	where MainKEY=1
	
	select AdminNO from M_SKUCounter Where MainKEY=1
	
	drop table #TAdminNo
END
GO
