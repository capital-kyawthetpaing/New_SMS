 BEGIN TRY 
 Drop Procedure dbo.[RakutenOrderDataDetail_Insert]
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
CREATE PROCEDURE [dbo].[RakutenOrderDataDetail_Insert]
	-- Add the parameters for the stored procedure here
	@JuChuuXml as xml,
	@ShippingXml as xml,
	@CouponXml as xml,
	@ChangeReasonXml as xml,
	@JuChuuDetailXml as xml,
	@ShippingDetailXml as xml,
	@Operator as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @InsertDateTime datetime=getdate()

	Exec dbo.D_RakutenJuchuu_Insert @JuChuuXml,@Operator,@InsertDateTime

	Exec dbo.D_RakutenShipping_Insert @ShippingXml,@Operator,@InsertDateTime

	Exec dbo.D_RakutenCoupon_Insert @CouponXml,@Operator,@InsertDateTime

	Exec dbo.D_RakutenChangeReason_Insert @ChangeReasonXml,@Operator,@InsertDateTime

	Exec dbo.D_RakutenJuchuuDetails_Insert @JuChuuDetailXml,@Operator,@InsertDateTime

	Exec dbo.D_RakutenShippingDetails_Insert @ShippingDetailXml,@Operator,@InsertDateTime

END

