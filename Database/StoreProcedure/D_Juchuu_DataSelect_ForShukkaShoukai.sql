 BEGIN TRY 
 Drop Procedure dbo.[D_Juchuu_DataSelect_ForShukkaShoukai]
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
CREATE PROCEDURE [dbo].[D_Juchuu_DataSelect_ForShukkaShoukai] 
	-- Add the parameters for the stored procedure here
	@JuchuuNO as varchar(11)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * 
	From D_Juchuu dj
	where dj.JuchuuNO = @JuchuuNO
END

