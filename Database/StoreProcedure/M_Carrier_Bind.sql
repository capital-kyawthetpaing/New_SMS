 BEGIN TRY 
 Drop Procedure dbo.[M_Carrier_Bind]
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
CREATE PROCEDURE [dbo].[M_Carrier_Bind]
    -- Add the parameters for the stored procedure here
    @ChangeDate as date,
    @DeleteFlg as tinyint
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    select ms.CarrierCD,ms.CarrierName from M_Carrier ms
    inner join F_Carrier(cast(@ChangeDate as varchar(10))) fs on ms.CarrierCD = fs.CarrierCD
    and ms.ChangeDate = fs.ChangeDate
    AND ms.DeleteFlg = @DeleteFlg
    ORDER BY ms.CarrierCD
END


