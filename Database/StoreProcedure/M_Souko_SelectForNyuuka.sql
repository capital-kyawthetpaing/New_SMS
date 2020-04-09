 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_SelectForNyuuka]
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
CREATE PROCEDURE [dbo].[M_Souko_SelectForNyuuka]
    -- Add the parameters for the stored procedure here
    @StoreCD as varchar(4),
    @SoukoType as tinyint,
    @ChangeDate as date,
    @DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select top 1 ms.SoukoCD,ms.SoukoName from M_Souko ms
	inner join F_Souko(cast(@ChangeDate as varchar(10))) fs on ms.SoukoCD = fs.SoukoCD
	and ms.ChangeDate = fs.ChangeDate
	AND ms.SoukoType = @SoukoType
	AND ms.StoreCD = @StoreCD
	ORDER BY ms.ChangeDate desc
END


