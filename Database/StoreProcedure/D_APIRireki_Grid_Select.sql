 BEGIN TRY 
 Drop Procedure dbo.[D_APIRireki_Grid_Select]
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
CREATE PROCEDURE [dbo].[D_APIRireki_Grid_Select]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select dapir.InsertDateTime ,
	(select StoreName from F_Store(dapir.InsertDateTime) where StoreCD = dapir.StoreCD) as StoreName,
	(select StaffName from F_Staff(dapir.InsertDateTime) where StaffCD = dapir.InsertOperator) as StaffName,
	(select Count(*) from D_APIDetail)AS CONT ,SUM(JU.InportErrFLG) AS InportErrFLG,SUM(JU.OnHoldFLG) AS OnHoldFLG,SUM(JU.IdentificationFLG) AS IdentificationFLG 
	from D_APIRireki AS dapir
	INNER JOIN D_APIDetail AS DE
	ON DE.InportSEQ=dapir.InportSEQ
	INNER JOIN D_Juchuu AS JU
	ON JU.SiteJuchuuNO=DE.OrderId
	group by DE.InportSEQ,dapir.InportSEQ,dapir.InsertDateTime,dapir.InsertOperator,dapir.StoreCD
	order by dapir.InportSEQ DESC
	  
END

