 BEGIN TRY 
 Drop Procedure dbo.[M_Store_Bind_Juchu]
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
CREATE PROCEDURE [dbo].[M_Store_Bind_Juchu]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(4),
	@ChangeDate as date,
	@DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select fs.StoreCD,fs.StoreName from  F_Store(cast(@ChangeDate as varchar(10))) fs
	where (@StoreCD is null or (fs.StoreCD = @StoreCD ))
	AND fs.StoreKBN = 1
END


