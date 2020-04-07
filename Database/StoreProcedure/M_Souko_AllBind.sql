 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_AllBind]
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
CREATE PROCEDURE [dbo].[M_Souko_AllBind] 
	-- Add the parameters for the stored procedure here
	@SoukoType as tinyint,
	@ChangeDate as date,
	@DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	select ms.SoukoCD,ms.SoukoName 
	from F_Souko(cast(@ChangeDate as varchar(10))) ms 
	where ms.SoukoType IN (1,2,3,4)
	and ms.DeleteFlg=@DeleteFlg
	ORDER BY ms.SoukoCD
						
END

