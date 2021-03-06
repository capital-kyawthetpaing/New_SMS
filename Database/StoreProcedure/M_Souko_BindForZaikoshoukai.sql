BEGIN TRY 
 Drop Procedure dbo.[M_Souko_BindForZaikoshoukai]
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
Create PROCEDURE [dbo].[M_Souko_BindForZaikoshoukai] 
	-- Add the parameters for the stored procedure here
	@storecd as varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		select SoukoCD,SoukoName
		from F_Souko(getdate()) as fs
		where fs.DeleteFlg=0
		and fs.StoreCD=@storecd
END
