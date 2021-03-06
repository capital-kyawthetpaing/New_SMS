BEGIN TRY 
 Drop Procedure [dbo].[M_CarrierDeliveryTime_Bind] 
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_CarrierDeliveryTime_Bind] 
    @CarrierCD as varchar(3)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT M.DeliveryTime, M.DeliveryTimeName
    FROM M_CarrierDeliveryTime M
    WHERE M.CarrierCD = @CarrierCD
    ORDER BY M.OrderPriority
    ;


END

GO
