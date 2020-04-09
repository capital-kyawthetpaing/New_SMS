 BEGIN TRY 
 Drop Procedure dbo.[M_Carrier_Select]
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
CREATE PROCEDURE [dbo].[M_Carrier_Select]
	-- Add the parameters for the stored procedure here
	@ShippingCD as varchar(3),
	@ChangeDate as date

AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * 
	FROM M_Carrier
	WHERE CarrierCD = @ShippingCD AND ChangeDate = @ChangeDate
END

