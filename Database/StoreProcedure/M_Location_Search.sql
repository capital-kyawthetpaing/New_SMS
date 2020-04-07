 BEGIN TRY 
 Drop Procedure dbo.[M_Location_Search]
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
CREATE PROCEDURE [dbo].[M_Location_Search] 
	-- Add the parameters for the stored procedure here
	@SoukoCD as varchar(6),
	@ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select * 
	From F_Location(@ChangeDate) 
	Where SoukoCD = @SoukoCD AND DeleteFlg = 0
	order by SoukoCD,TanaCD
	 
	
END

