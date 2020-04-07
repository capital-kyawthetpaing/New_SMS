 BEGIN TRY 
 Drop Procedure dbo.[M_Location_Select]
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
CREATE PROCEDURE [dbo].[M_Location_Select]
	-- Add the parameters for the stored procedure here
	@SoukoCD as varchar(6),
	@ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	--select ROW_NUMBER() OVER(ORDER BY SoukoCD ASC) AS No,
	select TanaCD
	from M_Location
	where SoukoCD=@SoukoCD 
	and ChangeDate = @ChangeDate

END

