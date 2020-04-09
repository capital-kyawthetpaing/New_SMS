 BEGIN TRY 
 Drop Procedure dbo.[SelectPrintData]
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
CREATE PROCEDURE  [dbo].[SelectPrintData]
	-- Add the parameters for the stored procedure here
	@StoreName as varchar(50),
	@SalesNo as varchar(15)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	
--select (mc.PostalAccountA +mc.PostalAccountB ) as P_Account, 
--mc.PostalAccountNo as P_AccountNo ,  
--                ds.SalesGaku as SalesGaku,
--                mc.CompanyName as Company,
--                @StoreName as StoreName,
--                ds.SalesNO as SalesNo,
--                ds.CustomerCD as CustomerCD,
--                (Case When mcu.LongName1 is null then '' else mcu.LongName1 end + Case When mcu.LongName2 is null then '' else mcu.LongName2 end) as LongName, 
--                 (Case When mcu.AliasKBN = 1 then N'様' else N'御中' End) as AliasKBN 
                
--                  from D_Sales ds 
--                left outer join M_Control mc on mc.MainKey =1  
--                left outer join M_Customer mcu on mcu.CustomerCD =  ds.CustomerCD and
--                 mcu.ChangeDate = ( select top 1 M_Customer.ChangeDate from M_Customer where ChangeDate <= ds.SalesDate and M_Customer.CustomerCD =  ds.CustomerCD  order by M_Customer.ChangeDate asc )  
                
--                where ds.DeleteDateTime is null
--                and mcu.DeleteFlg = 0 
--                and ds.SalesNO = @SalesNo
--                order by ds.SalesNO asc  
select distinct top 1 (mc.PostalAccountA +mc.PostalAccountB ) as P_Account, 
				mc.PostalAccountNo as P_AccountNo ,  
                ds.SalesGaku as SalesGaku,
                mc.CompanyName as Company,
                @StoreName as StoreName,
                ds.SalesNO as SalesNo,
                ds.CustomerCD as CustomerCD
				,
                (Case When mcu.LongName1 is null then '' else mcu.LongName1 end + Case When mcu.LongName2 is null then '' else mcu.LongName2 end) as LongName
				, 
                (Case When mcu.AliasKBN = 1 then N'様' else N'御中' End) as AliasKBN 
                from D_Sales ds 
                left outer join M_Control mc on mc.MainKey =1  
                left outer join M_Customer mcu on mcu.CustomerCD =  ds.CustomerCD 
				left outer join  F_Customer(getdate()) fc on fc.ChangeDate <= ds.SalesDate and fc.DeleteFlg = 0
                where
				 ds.DeleteDateTime is null
                and
				 ds.SalesNO = @SalesNo
                order by ds.SalesNO asc  

END

--select * from  D_Sales where salesNo='99999999999' and BillingType = 0

