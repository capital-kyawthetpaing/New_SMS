 BEGIN TRY 
 Drop Procedure dbo.[M_Store_Bind_Mitsumori]
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
CREATE PROCEDURE [dbo].[M_Store_Bind_Mitsumori]
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
	select ms.StoreCD,ms.StoreName from M_Store ms
	inner join F_Store(cast(@ChangeDate as varchar(10))) fs on ms.StoreCD = fs.StoreCD
	and ms.ChangeDate = fs.ChangeDate
	where (@StoreCD is null or (fs.StoreCD = @StoreCD ))
	AND ms.StoreKBN IN (1,3)
END


