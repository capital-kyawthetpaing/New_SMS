 BEGIN TRY 
 Drop Procedure dbo.[M_SearchVendor]
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
CREATE PROCEDURE [dbo].[M_SearchVendor]
@ChangeDate as date,
@VendorName as varchar(50),
@VendorKana as varchar(20),
@VendorCDFrom as varchar(13),
@VendorCDTo as varchar(13),
@NotDisplayNote as varchar(500),
@keyword1 as varchar(80),
@keyword2 as varchar(80),
@keyword3 as varchar(80),
@VendorKBN as tinyint

AS
BEGIN

	
	select  VendorCD,VendorName,VendorKana,NotDisplyNote,convert(varchar(10),ChangeDate,111) as ChangeDate
	from F_Vendor(@ChangeDate)
	 where (VendorFlg=(case when @VendorKBN=1 then 1 else VendorFlg end) and DeleteFlg=0)
	 and (PayeeFlg=(case when @VendorKBN=2 then 1 else PayeeFlg end) and DeleteFlg=0)
	 and (MoneyPayeeFlg=(case when @VendorKBN=3 then 1 else MoneyPayeeFlg end) and DeleteFlg=0)
	 and  DeleteFlg=0
	 and (@VendorName is null or VendorName like '%'+@VendorName+'%') 
	 and (@VendorKana is null or (VendorKana=@VendorKana))
	 and((@keyword1 is null and @keyword2 is null and @keyword3 is null)		or (( NotDisplyNote like '%' + @keyword1 + '%')		or ( NotDisplyNote like '%' + @keyword2 + '%')		or (NotDisplyNote like '%' + @keyword3 + '%'))		or (( DisplayNote like '%' + @keyword1 + '%')		or ( DisplayNote like '%' + @keyword2 + '%')		or (DisplayNote like '%' + @keyword3 + '%')))
		and (@VendorCDFrom is null or ( VendorCD >= @VendorCDFrom))		and (@VendorCDTo is null or ( VendorCD <= @VendorCDTo))
		order by VendorCD
	

	 
END
