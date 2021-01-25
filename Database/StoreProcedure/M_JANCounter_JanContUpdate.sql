BEGIN TRY 
 Drop Procedure [dbo].[M_JANCounter_JanContUpdate]
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
Create PROCEDURE [dbo].[M_JANCounter_JanContUpdate]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	CREATE TABLE [dbo].[#TJanCount](
	JanCount [int] NULL,)


	Insert into #TJanCount
	select JanCount 
	from M_JANCounter
	Where MainKEY=1
	
	Update M_JANCounter
	set JanCount=tp.JanCount +1
	from #TJanCount as tp
	where MainKEY=1

	
	select JanCount from M_JANCounter Where MainKEY=1

	drop table #TJanCount
	
END
GO
