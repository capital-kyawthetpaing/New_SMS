use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.M_Souko_BindForShukka
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
CREATE PROCEDURE [dbo].[M_Souko_BindForShukka]
	-- Add the parameters for the stored procedure here
	@ChangeDate as date,
	@DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ms.SoukoCD,ms.SoukoName from M_Souko ms
	inner join F_Souko(cast(@ChangeDate as varchar(10))) fs on ms.SoukoCD = fs.SoukoCD
	and ms.ChangeDate = fs.ChangeDate
	AND MS.SoukoType IN (1,2,3,4)
	AND MS.DeleteFlg = 0
	ORDER BY ms.SoukoCD
END

GO


