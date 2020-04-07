 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Souko_Select]
-- Add the parameters for the stored procedure here
@SoukoCD as varchar(6),
@ChangeDate as date
--@MakerCD as varchar(13)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
select	ms.SoukoName,
ms.StoreCD,
ms.ZipCD1,
ms.ZipCD2,
ms.Address1,
ms.Address2,
ms.TelephoneNo,
ms.FaxNO,
ms.SoukoType,
ms.MakerCD,
fv.VendorName as MakerName ,
ms.HikiateOrder,
ms.UnitPriceCalcKBN,
ms.IdouCount,
ms.Remarks,
ms.DeleteFlg from M_Souko ms 
inner join (select * from F_Store(@ChangeDate)) fs on fs.StoreCD=ms.StoreCD
left join (select * from F_Vendor(@ChangeDate)) fv on fv.VendorCD=ms.MakerCD
--left join (select * from F_Vendor(@ChangeDate)) fv on fv.VendorCD=@MakerCD
--inner join (select * from F_Store(cast(@ChangeDate as varchar(10)),0)) fs on ms.StoreCD = fs.StoreCD
--left join (select * from F_Vendor(cast(@ChangeDate as varchar(10)),0)) fv on ms.MakerCD = fv.VendorCD
where (@SoukoCD is null or(ms.SoukoCD = @SoukoCD))
and (@ChangeDate is null or (ms.ChangeDate = @ChangeDate))
and fs.DeleteFlg=0
and ISNULL(fv.DeleteFlg,0)=0

END
