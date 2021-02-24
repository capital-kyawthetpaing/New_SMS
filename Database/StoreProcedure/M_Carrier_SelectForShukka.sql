BEGIN TRY 
 Drop Procedure dbo.[M_Carrier_SelectForShukka]
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
CREATE PROCEDURE [dbo].[M_Carrier_SelectForShukka]
    @CarrierCD as varchar(11),
    @ChangeDate as date

AS  
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 A.CarrierName,A.NormalFLG, A.SCatKBN2
    FROM M_Carrier A
    WHERE A.CarrierCD = @CarrierCD
      AND A.ChangeDate <= @ChangeDate
      AND A.DeleteFlg = 0
    ORDER BY A.ChangeDate DESC
END

GO