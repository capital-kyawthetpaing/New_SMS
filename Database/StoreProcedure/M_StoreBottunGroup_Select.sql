 BEGIN TRY 
 Drop Procedure dbo.[M_StoreBottunGroup_Select]
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
CREATE PROCEDURE [dbo].[M_StoreBottunGroup_Select]
	-- Add the parameters for the stored procedure here
	--@Mode int
	
AS
BEGIN
declare @currentdate as date =getdate();

select MSBG.BottunName AS ButtomName,MSBG.GroupNO AS GroupNO ,
 MSBG.MasterKBN AS MasterKBN, MBD.BottunName AS btndetailBottunName , 
 MBD.GroupNO AS btndetailGroupNO,
 MBD.Horizontal AS Horizontal,MBD.Vertical AS Vertical,
 MBD.Botton AS Button,
CASE 
WHEN MSBG.MasterKBN=1 THEN fsku.SKUName
WHEN MSBG.MasterKBN=2 THEN fc.CustomerName
END
AS Name
from M_StoreBottunGroup MSBG
left outer join M_StoreBottunDetails MBD
on MBD.StoreCD=msbg.StoreCD
and MBD.ProgramKBN=msbg.ProgramKBN
and MBD.GroupNO=msbg.GroupNO
left outer join F_SKU(@currentdate) fsku 
on  fsku.JanCD =(case when msbg.MasterKBN=1 then MBD.Botton  end)
left outer join F_Customer(@currentdate) fc
on fc.CustomerCD = (case when msbg.MasterKBN=2 then MBD.Botton end)
where isnull(fsku.DeleteFlg,0)=0 and isnull(fc.DeleteFlg,0)=0

order by msbg.GroupNO,MBD.Horizontal,MBD.Vertical asc

-- Select  MSBG.BottunName AS ButtomName,MSBG.GroupNO AS GroupNO ,
-- MSBG.MasterKBN AS MasterKBN, 
--MBD.BottunName AS btndetailBottunName , MBD.GroupNO AS btndetailGroupNO,MBD.Horizontal AS Horizontal,MBD.Vertical AS Vertical,MBD.Botton AS Button,
--CASE 
--WHEN MSBG.MasterKBN=1 THEN fsku.SKUName
--WHEN MSBG.MasterKBN=2 THEN fc.CustomerName
--END
--AS Name
----INTO #temp
--from M_StoreBottunGroup AS MSBG
--Left Outer JOIN M_StoreBottunDetails AS MBD
--ON MSBG.StoreCD=MBD.StoreCD
--AND MSBG.ProgramKBN=MBD.ProgramKBN
--AND MSBG.GroupNO=MBD.GroupNO
--Left Outer JOIN  F_SKU(@currentdate) fsku on fsku.JanCD=MBD.Botton
--Left Outer JOIN  F_Customer(@currentdate) AS fc ON fc.CustomerCD=MBD.Botton
----WHERE fsku.DeleteFlg=0 AND  fc.DeleteFlg=0
--Order by
--MSBG.GroupNO,MBD.Horizontal,MBD.Vertical ASC
--DECLARE
--	 @query NVARCHAR(MAX),
--	 @cols NVARCHAR (MAX),
--	@cols1 NVARCHAR (MAX)
--	if @Mode = 1
--	begin
--		select  @cols= COALESCE (@cols + ',ISNULL(['+Convert(varchar,GroupNO)+'],0) as '+'['+Convert(varchar,GroupNO)+']' , 'ISNULL(['+Convert(varchar,GroupNO)+'],0) as '+ '['+Convert(varchar,GroupNO)+']') from #temp WHERE GroupNO IS NOT NULL
--	    select @cols1= COALESCE (@cols1 + ',['+Convert(varchar,GroupNO)+']', '['+Convert(varchar,GroupNO)+']') from #temp WHERE GroupNO IS NOT NULL
		

--	set @query='select '+@cols+ '
--	from
--	(
--	select ButtomName as btn,GroupNO
--	from #temp) p
--	PIVOT (
--		MAX(btn)
--		FOR [GroupNO] in ('+@cols1+')
--		) as x'
 
-- end
-- else if @Mode = 2
-- begin
--		select  @cols= COALESCE (@cols + ',ISNULL(['+Convert(varchar,btndetailGroupNO)+'],0) as '+'['+Convert(varchar,btndetailGroupNO)+']' , 'ISNULL(['+Convert(varchar,btndetailGroupNO)+'],0) as '+ '['+Convert(varchar,btndetailGroupNO)+']') from #temp WHERE btndetailGroupNO IS NOT NULL
--	    select @cols1= COALESCE (@cols1 + ',['+Convert(varchar,btndetailGroupNO)+']', '['+Convert(varchar,btndetailGroupNO)+']') from #temp  WHERE btndetailGroupNO IS NOT NULL
		

--	set @query='select '+@cols+ '
--	from
--	(
--	select btndetailBottunName as btn,btndetailGroupNO
--	from #temp) p
--	PIVOT (
--		MAX(btn)
--		FOR [btndetailGroupNO] in ('+@cols1+')
--		) as x'
 
-- end
--	--select @query
--EXEC SP_EXECUTESQL @query

END
