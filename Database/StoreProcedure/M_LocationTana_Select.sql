 BEGIN TRY 
 Drop Procedure dbo.[M_LocationTana_Select]
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
CREATE PROCEDURE [dbo].[M_LocationTana_Select]
	-- Add the parameters for the stored procedure here
	@SoukoCD as varchar(6),
	@TanaCD as varchar(10)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--Select * 
	--From M_Location
	--Where SoukoCD = @SoukoCD 
	--AND TanaCD = @TanaCD
	--AND ChangeDate <= @ChangeDate
    declare @ChangeDate as datetime = getdate();

	Select * 
	From F_Location(@ChangeDate) 
	Where SoukoCD = @SoukoCD AND TanaCD = @TanaCD 

END

