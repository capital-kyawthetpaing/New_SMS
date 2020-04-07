 BEGIN TRY 
 Drop Procedure dbo.[D_MenuMessageSelect]
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
CREATE PROCEDURE [dbo].[D_MenuMessageSelect]
	-- Add the parameters for the stored procedure here
@Staff_CD as varchar(6)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select fm.Message from F_Staff(getdate())  fs
				inner join  F_MenuMessage(getDate()) fm on fm.StoreCD=  fs.StoreCD 

				where fs.StaffCD =@Staff_CD
END

