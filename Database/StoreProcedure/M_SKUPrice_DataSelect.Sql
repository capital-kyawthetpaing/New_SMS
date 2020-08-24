 BEGIN TRY 
 Drop Procedure [dbo].[M_SKUPrice_DataSelect]
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
CREATE PROCEDURE [dbo].[M_SKUPrice_DataSelect]
@SKUCD as varchar(50),
@StartDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Select  ChangeDate
	From M_SKUPrice
	where TankaCD>=0
	and TankaCD<=6
	and StoreCD='0000'
	and SKUCD=@SKUCD
	and ChangeDate=@StartDate
	and DeleteFlg=0
END
	

GO

