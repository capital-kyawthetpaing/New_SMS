
 BEGIN TRY 
 Drop Procedure dbo.[M_CustomerSKUPrice_Import]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE M_CustomerSKUPrice_Import
	-- Add the parameters for the stored procedure here

			@xml as xml  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		declare @DocHandle as int, @dtn as datetime = getdate();
			exec sp_xml_preparedocument @DocHandle output, @xml
	select * into #tempItem
	 FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
		     KBN  varchar(10)
			,CustomerCD varchar(20)
			,CustomerName varchar(200)
			,StartDate date
			,EndDate date
			,AdminNO varchar(100)
			,JANCD varchar(100)
			,SKUCD varchar(50)
			,ShouhinName varchar(200)
			,Remark varchar(500)
			,SalePOT varchar(50)
			,DeleteFlg varchar(50)
			,PC varchar(100)
			,[Key] varchar(300)
			,Operator varchar(300)
			,ProgramID varchar(300)



			)
					exec sp_xml_removedocument @DocHandle;

						Update msp set
					msp.CustomerName = fc.CustomerName
					,msp.TekiyouShuuryouDate = Convert (varchar(10), mt.EndDate,126  )
					,msp.JanCD = fs.JANCD
					,msp.SKUCD = fs.SKUCD
					,msp.SKUName = fs.SKUName
					,msp.SalePriceOutTax =mt.SalePOT
					,msp.Remarks = mt.Remark
					,msp.DeleteFlg = mt.DeleteFlg
					,msp.UpdateDateTime = @dtn


					from M_CustomerSKUPrice msp 
					inner join #tempItem mt on msp.CustomerCD= mt.CustomerCD and  convert(varchar(10),mt.StartDate,126) = Convert (varchar(10),msp.TekiyouKaisiDate,126  ) and  msp.AdminNo = mt.AdminNo 
					inner join F_Customer(getdate()) fc on fc.CustomerCD = msp.CustomerCD
					inner join F_SKu(getdate()) fs on fs.AdminNo = msp.AdminNo
					
					


					insert into M_CustomerSKUPrice
					select   
					 mt.CustomerCD							
					,fc.CustomerName							
					,mt.StartDate							
					,mt.EndDate							
					,mt.AdminNO							
					,fs.JanCD							
					,fs.SKUCD							
					,fs.SKUName							
					,mt.SalePOT							
					,mt.Remark							
					,mt.DeleteFlg							
					,(select Max(Operator) from #tempItem)							
					,@dtn				
					,(select Max(Operator) from #tempItem)						
					,@dtn					

					 from 
					#tempItem mt  
					inner join F_SKU(getdate()) fs on fs.AdminNo = mt.AdminNo 
					inner join F_Customer(getdate())  fc on fc.CustomerCD= mt.CustomerCD
					where not exists( 

					select 1 from M_CustomerSKUPrice mcp 
					where
					 mcp.AdminNo = mt.AdminNo 
					and convert(varchar(10),mt.StartDate,126) = Convert (varchar(10),mcp.TekiyouKaisiDate,126  ) 
					and mcp.CustomerCD = mt.CustomerCD
					)
					
---------------------------------------------------------------------------------------------------------------------

declare  
@opt as varchar(50)= (select Max(Operator) from #tempItem) ,
@PID as varchar(200) = (select Max(ProgramID) from #tempItem),
@PC as varchar(50) = (select Max(PC) from #tempItem),
@K as varchar(500) = (select Max([key]) from #tempItem);
					EXEC L_Log_Insert
					@opt
					,  @PID      
					, @PC            
					,null    
					,@K

			drop table #tempItem
	

END
GO
