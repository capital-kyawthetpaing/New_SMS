 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Kouza_Select]
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
CREATE PROCEDURE [dbo].[M_Vendor_Kouza_Select]
	-- Add the parameters for the stored procedure here
	 @KouzaCD  varchar(3),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select mk.KouzaCD,mk.KouzaName,mk.DeleteFlg From M_Kouza mk 
	inner join (select * from F_Kouza(cast(@ChangeDate as varchar(10)))) fk on mk.KouzaCD = fk.KouzaCD
	Where mk.KouzaCD = @KouzaCD
	AND mk.ChangeDate <= @ChangeDate
END

