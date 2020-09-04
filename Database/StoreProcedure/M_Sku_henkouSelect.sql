USE [CapitalSMS]
GO
/****** Object:  StoredProcedure [dbo].[M_Sku_henkouSelect]    Script Date: 2020-08-27 14:28:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[M_Sku_henkouSelect]
	-- Add the parameters for the stored procedure here
		@Item as varchar(13),
		@StartDate as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select	
		mi.ItemName as ItemName,
								mi.MakerItem as MakerItem,
								mi.SizeCount as nsc,
								mi.SizeCount as osc,
								Cast(ms.SizeName as varchar(15)) as SizeName,
								ms.DeleteFlg as DeleteFalg,
								
								mi.ColorCount as ncc,
								mi.ColorCount as occ,
								ms.ColorName as ColorName,
								ms.DeleteFlg as DeleteFlag,
								
								ms.JanCD as JanCD,
								ms.AdminNO as AdminNO,
								ms.SKUCD,
								
								ms.SizeNo,
								ms.ColorNo,
								
								ms.VirtualFlg as VirtualFlag
								
								
								
								from M_ITEM mi left outer join M_SKU ms on ms.ItemCD= mi.ITemCD and ms.ChangeDate = mi.ChangeDate
								where (@Item is null or (mi.ITemCD=@Item))
								and (@StartDate is null or ( mi.ChangeDate = @StartDate) )
								order by ms.SizeNO , ms.ColorNO
END
