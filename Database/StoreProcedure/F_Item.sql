
 BEGIN TRY 
  DROP FUNCTION [F_Item]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create FUNCTION [dbo].[F_Item]
(	
	-- Add the parameters for the function here
	@ChangeDate as date
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	select mi.*, mi.OrderLot as _OrderLot, mi.ExhibitionSegmentCD as _ExhibitionSegmentCD , mi.SoldOutFlg as _SoldOutflg , mi.NormalCost as _NormalCost
	
	from M_ITEM mi
	inner join 
	(
		select  ITemCD, MAX(ChangeDate) as ChangeDate
		from    dbo.M_ITEM
		where (@ChangeDate is null or (ChangeDate <= @ChangeDate))
		group by ITemCD
	) temp_Souko on mi.ITemCD = temp_Souko.ITemCD and mi.ChangeDate = temp_Souko.ChangeDate
)
