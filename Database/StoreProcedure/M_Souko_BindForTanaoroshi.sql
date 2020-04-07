 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_BindForTanaoroshi]
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
Create PROCEDURE [dbo].[M_Souko_BindForTanaoroshi]
    -- Add the parameters for the stored procedure here
    @StoreCD as varchar(4),
    @ChangeDate as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select fs.SoukoCD,fs.SoukoName 
	FROM F_Souko(@ChangeDate) fs 
	WHERE Fs.StoreCD = @StoreCD
	ORDER BY fs.SoukoCD
END


