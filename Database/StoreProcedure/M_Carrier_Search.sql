 BEGIN TRY 
 Drop Procedure dbo.[M_Carrier_Search]
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
CREATE PROCEDURE [dbo].[M_Carrier_Search]
	-- Add the parameters for the stored procedure here

	@ChangeDate as date,
	@DisplayKBN as tinyint,
	@ShippingCDFrom as varchar(13),
	@ShippingCDTo as varchar(13),
	@ShippingName as varchar(50),
	@DeleteFlg as tinyint


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	IF @DisplayKbn = 0
	   BEGIN
			Select 
			mc.CarrierCD,
			mc.CarrierName,
			REPLACE(CONVERT(VARCHAR(10), mc.ChangeDate , 111),'-','/') as ChangeDate
		From M_Carrier mc
			inner join F_Carrier(cast(@ChangeDate as varchar(10))) fc on mc.CarrierCD = fc.CarrierCD  and mc.ChangeDate = fc.ChangeDate 
			where mc.DeleteFlg = @DeleteFlg 
			and (@ShippingCDFrom is null or mc.CarrierCD >= @ShippingCDFrom)
			and (@ShippingCDTo is null or mc.CarrierCD <= @ShippingCDTo)
			and (@ShippingName is null or mc.CarrierName like '%'+ @ShippingName + '%')
			order by mc.CarrierCD,mc.ChangeDate
			

	   END
     ELSE
	   BEGIN
			Select
			mc.CarrierCD,
			mc.CarrierName,
			REPLACE(CONVERT(VARCHAR(10), mc.ChangeDate , 111),'-','/') as ChangeDate
			From M_Carrier mc
			where mc.DeleteFlg = @DeleteFlg 
			and (@ShippingCDFrom is null or mc.CarrierCD >= @ShippingCDFrom)
			and (@ShippingCDTo is null or mc.CarrierCD <= @ShippingCDTo)
			and (@ShippingName is null or mc.CarrierName like '%'+ @ShippingName + '%')
			--and mc.ChangeDate <= @ChangeDate and mc.DeleteFlg=@DeleteFlg
			order by mc.CarrierCD,mc.ChangeDate
	   END

END

