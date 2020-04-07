 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_Search]
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
CREATE PROCEDURE[dbo].[M_Customer_Search]
	-- Add the parameters for the stored procedure here
@RefDate as date,@CustomerKBN as tinyint,@CustName as varchar(80),@KanaName as varchar(30),@BirthDate as date,@TelephoneNo as varchar(15),@StoreKBN as tinyint,@keyword1 as varchar(80),@keyword2 as varchar(80),@keyword3 as varchar(80),@StoreCD as varchar(4),@CustFrom as varchar(13),@CustTo as varchar(13)
--@KeyWordType as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	SET NOCOUNT ON;
    -- Insert statements for procedure here--IF @KeyWordType=1SELECT fc.CustomerCD,fc.CustomerName,fs.StoreName,case when fc.Tel11 is not null then (fc.Tel11+''+fc.Tel12+''+fc.Tel13) when fc.Tel21 is not null then (fc.Tel21+''+fc.Tel22+''+fc.Tel23) end as TelephoneNo,CONVERT(VARCHAR(10),fc.Birthdate,111) AS Birthdate,fc.KanaName,(ISNULL(fc.Address1,'')+' '+ISNULL(fc.Address2,'') ) as [Address],fc.RemarksInStore,CONVERT(VARCHAR(10),fc.ChangeDate,111) as ChangeDateFROM F_Customer(@RefDate) fcLEFT OUTER JOIN  F_Store(@RefDate) fsON fs.StoreCD=fc.MainStoreCDand fs.DeleteFlg=0where BillingFLG = ( case when @CustomerKBN = 1 then 1 else BillingFLG end)and CollectFLG = ( case when @CustomerKBN = 2 then 1 else CollectFLG end)and (@CustomerKBN <> 3 or exists (select 1 from F_Customer(@RefDate) where DeleteFlg = 0 and CustomerCD = fc.CollectCD))and fc.DeleteFlg = 0and (@CustName is null or (fc.CustomerName like '%' + @CustName + '%'))and (@KanaName is null or (fc.KanaName like '%' + @KanaName + '%'))and (@BirthDate is null or (fc.Birthdate=@BirthDate))and (@TelephoneNo is null or (fc.Tel11+''+fc.Tel12+''+fc.Tel13 = @TelephoneNo) or (fc.Tel21+''+fc.Tel22+''+fc.Tel23 = @TelephoneNo))  and (@StoreKBN is null or (fc.StoreKBN = @StoreKBN))and (@StoreKBN is not null or ( fc.StoreKBN in (1,2)))and((@keyword1 is null and @keyword2 is null and @keyword3 is null)or (( fc.RemarksInStore like '%' + @keyword1 + '%')or ( fc.RemarksInStore like '%' + @keyword2 + '%')or (fc.RemarksInStore like '%' + @keyword3 + '%'))or (( fc.RemarksOutStore like '%' + @keyword1 + '%')or ( fc.RemarksOutStore like '%' + @keyword2 + '%')or (fc.RemarksOutStore like '%' + @keyword3 + '%')))and (@StoreCD is null or (MainStoreCD = @StoreCD))and (@CustFrom is null or ( CustomerCD >= @CustFrom))and (@CustTo is null or ( CustomerCD <= @CustTo))

ORDER BY fc.CustomerCD--IF @KeyWordType=2--begin--SELECT fc.CustomerCD,--fc.CustomerName,--fs.StoreName,--case when fc.Tel11 is not null then (fc.Tel11+''+fc.Tel12+''+fc.Tel13) when fc.Tel21 is not null then (fc.Tel21+''+fc.Tel22+''+fc.Tel23) end as TelephoneNo,--fc.Birthdate,--fc.KanaName,--(ISNULL(fc.Address1,'')+' '+ISNULL(fc.Address2,'') ) as [Address],--fc.RemarksInStore,--CONVERT(VARCHAR(10),fc.ChangeDate,111) as ChangeDate--FROM F_Customer(@RefDate) fc--LEFT OUTER JOIN  F_Store(@RefDate) fs--ON fs.StoreCD=fc.MainStoreCD--and fs.DeleteFlg=0--where BillingFLG = ( case when @CustomerKBN = 1 then 1 else BillingFLG end)--and CollectFLG = ( case when @CustomerKBN = 2 then 1 else CollectFLG end)--and (@CustomerKBN <> 3 or exists (select 1 from F_Customer(@RefDate) where DeleteFlg = 0 and CustomerCD = fc.CollectCD))--and fc.DeleteFlg = 0--and (@CustName is null or (fc.CustomerName like '%' + @CustName + '%'))--and (@KanaName is null or (fc.KanaName like '%' + @KanaName + '%'))--and (@BirthDate is null or (fc.Birthdate=@BirthDate))--and (@TelephoneNo is null or (fc.Tel11+''+fc.Tel12+''+fc.Tel13 = @TelephoneNo) or (fc.Tel21+''+fc.Tel22+''+fc.Tel23 = @TelephoneNo))  --and (@StoreKBN is null or (fc.StoreKBN = @StoreKBN))--and (@StoreKBN is not null or ( fc.StoreKBN in (1,2)))--and((@keyword1 is null and @keyword2 is null and @keyword3 is null)--and (( fc.RemarksInStore like '%' + @keyword1 + '%')--and ( fc.RemarksInStore like '%' + @keyword2 + '%')--and (fc.RemarksInStore like '%' + @keyword3 + '%'))--and (( fc.RemarksOutStore like '%' + @keyword1 + '%')--and ( fc.RemarksOutStore like '%' + @keyword2 + '%')--and (fc.RemarksOutStore like '%' + @keyword3 + '%')))--and (@StoreCD is null or (MainStoreCD = @StoreCD))--and (@CustFrom is null or ( CustomerCD >= @CustFrom))--and (@CustTo is null or ( CustomerCD <= @CustTo))
--ORDER BY fc.CustomerCD--end

END

