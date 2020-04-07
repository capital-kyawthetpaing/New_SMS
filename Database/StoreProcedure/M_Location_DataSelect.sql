 BEGIN TRY 
 Drop Procedure dbo.[M_Location_DataSelect]
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
CREATE PROCEDURE [dbo].[M_Location_DataSelect]
	-- Add the parameters for the stored procedure here
	@Unregister as tinyint,
	@Register as tinyint 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if @Unregister = 1 and @Register = 1
				begin	
				    Select 
					case when ds.RackNO is null and ds1.RackNO is null then null
						when ds.RackNO is null and ds1.RackNo is not null then ds1.RackNo  
						when ds.RackNO is not null then ds.RackNo end  as RackNo1,
					ds1.RackNO,
					ds.SKUCD,
					ms.SKUName,
					ms.ColorName,
					ms.SizeName,
					ds.JanCD,
					ds.StockSu,
					ds.StockNO
					From D_Stock ds
					Left Outer Join M_SKU ms on ms.JanCD = ds.JanCD 
											and ms.SKUCD = ds.SKUCD 
											and ms.ChangeDate <= ds.ArrivalDate
				
					Left Outer Join D_Stock ds1 on ds1.SoukoCD = ds.SoukoCD 
												and ds1.SKUCD  = ds.SKUCD 
												and ds1.StockSu > 0 
												and ds1.ArrivalDate <= GETDATE ()
					Where ms.DeleteFlg = 0 
					AND ds.StockSu > 0 
					AND ds.DeleteDateTime is null
					AND ds.RackNO is null or ds.RackNO is not null
					Order by ds.RackNO,ds1.RackNO,ds.SKUCD,ds.StockSu Asc 
				end
	 else if @Unregister = 1
				begin	
				    Select 
					case when ds.RackNO is null and ds1.RackNO is null then null
						when ds.RackNO is null and ds1.RackNo is not null then ds1.RackNo  
						when ds.RackNO is not null then ds.RackNo end  as RackNo1,
					ds1.RackNO,
					ds.SKUCD,
					ms.SKUName,
					ms.ColorName,
					ms.SizeName,
					ds.JanCD,
					ds.StockSu,
					ds.StockNO
					From D_Stock ds
					Left Outer Join M_SKU ms on ms.JanCD = ds.JanCD 
											and ms.SKUCD = ds.SKUCD 
											and ms.ChangeDate <= ds.ArrivalDate
				
					Left Outer Join D_Stock ds1 on ds1.SoukoCD = ds.SoukoCD 
												and ds1.SKUCD  = ds.SKUCD 
												and ds1.StockSu > 0 
												and ds1.ArrivalDate <= GETDATE ()
					Where ms.DeleteFlg = 0 
					AND ds.StockSu > 0 
					AND ds.DeleteDateTime is null
					AND ds.RackNO is null 					
					Order by ds.RackNO,ds1.RackNO,ds.SKUCD,ds.StockSu Asc 
				end
		else if @Register = 1
				begin	
				    Select 
					case when ds.RackNO is null and ds1.RackNO is null then null
						when ds.RackNO is null and ds1.RackNo is not null then ds1.RackNo  
						when ds.RackNO is not null then ds.RackNo end  as RackNo1,
					ds1.RackNO,
					ds.SKUCD,
					ms.SKUName,
					ms.ColorName,
					ms.SizeName,
					ds.JanCD,
					ds.StockSu,
					ds.StockNO
					From D_Stock ds
					Left Outer Join M_SKU ms on ms.JanCD = ds.JanCD 
											and ms.SKUCD = ds.SKUCD 
											and ms.ChangeDate <= ds.ArrivalDate
				
					Left Outer Join D_Stock ds1 on ds1.SoukoCD = ds.SoukoCD 
												and ds1.SKUCD  = ds.SKUCD 
												and ds1.StockSu > 0 
												and ds1.ArrivalDate <= GETDATE ()
					Where ms.DeleteFlg = 0 
					AND ds.StockSu > 0 
					AND ds.DeleteDateTime is null
					AND  ds.RackNO is not null				
					Order by ds.RackNO,ds1.RackNO,ds.SKUCD,ds.StockSu Asc 
				end
	
END

