BEGIN TRY 
 Drop Procedure [dbo].[CheckShipping]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--********************************************--
--                                            --
--             出荷チェック               --
--                                            --
--********************************************--
CREATE PROCEDURE [dbo].[CheckShipping]
    (@ShippingNO varchar(11)
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT DS.ShippingNO
          ,DS.ShippingDate
          ,DS.DeleteDateTime
      FROM D_Shipping DS
     WHERE DS.ShippingNO = @ShippingNO
     ;

END

