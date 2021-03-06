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
	@StoreCD varchar(4)
	
AS
BEGIN
declare @currentdate as date =getdate();

	select MSBG.BottunName AS ButtomName,MSBG.GroupNO AS GroupNO ,
		 MSBG.MasterKBN AS MasterKBN, MBD.BottunName AS btndetailBottunName , 
		 MBD.GroupNO AS btndetailGroupNO,
		 MBD.Horizontal AS Horizontal,MBD.Vertical AS Vertical,
		 MBD.Botton AS Button,
		 MBD.AdminNO,
		 MBD.JanCD,
		 MBD.CustomerCD,
		CASE 
	WHEN MSBG.MasterKBN=1 THEN fsku.SKUName
	WHEN MSBG.MasterKBN=2 THEN fc.CustomerName
	END
	AS Name
	from M_StoreBottunGroup MSBG
		left outer join M_StoreBottunDetails MBD on MBD.StoreCD=msbg.StoreCD
												and MBD.ProgramKBN=msbg.ProgramKBN
												and MBD.GroupNO=msbg.GroupNO
		left outer join F_SKU(@currentdate) fsku on  fsku.JanCD =(case when msbg.MasterKBN=1 then MBD.Botton  end)
		left outer join F_Customer(@currentdate) fc on fc.CustomerCD = (case when msbg.MasterKBN=2 then MBD.Botton end)
	where isnull(fsku.DeleteFlg,0)=0 
	and isnull(fc.DeleteFlg,0)=0
	and MSBG.StoreCD=@StoreCD

	order by msbg.GroupNO,MBD.Horizontal,MBD.Vertical asc



END
