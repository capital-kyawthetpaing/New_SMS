 BEGIN TRY 
 Drop Procedure dbo.[M_StoreAuthorizations_AccessCheck]
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
CREATE PROCEDURE [dbo].[M_StoreAuthorizations_AccessCheck]
	-- Add the parameters for the stored procedure here
	@ProgramID varchar(30),
	@StaffCD varchar(10),
	@PC varchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	Declare @ChangeDate date=getdate()

	select *,
	msautho.StoreAuthorizationsCD
	from M_Authorizations as mautho inner join M_Staff as ms on mautho.AuthorizationsCD=ms.AuthorizationsCD and  mautho.ChangeDate<=getdate()
	inner join M_StoreAuthorizations as msautho on ms.StoreAuthorizationsCD=msautho.StoreAuthorizationsCD and msautho.ChangeDate=mautho.ChangeDate
	--and msautho.pr
	where ms.StaffCD = @StaffCD

END

