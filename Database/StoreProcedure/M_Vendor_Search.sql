 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Search]
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
CREATE PROCEDURE [dbo].[M_Vendor_Search]
	-- Add the parameters for the stored procedure here
	
	@ChangeDate as date,
	@DisplayKBN as tinyint,
	@VendorCDFrom as varchar(13),
	@VendorCDTo as varchar(13),
	@VendorName as varchar(50),
	@VendorKana as varchar(20),
	@DeleteFlg as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	IF @DisplayKbn = 0
	   BEGIN
			Select 
			mv.VendorCD,
			mv.VendorName,
			REPLACE(CONVERT(VARCHAR(10), mv.ChangeDate , 111),'-','/') as ChangeDate
		From M_Vendor mv
			inner join F_Vendor(cast(@ChangeDate as varchar(10))) fv on mv.VendorCD = fv.VendorCD  and mv.ChangeDate  = fv.ChangeDate 
			where mv.DeleteFlg = @DeleteFlg 
			and (@VendorCDFrom is null or mv.VendorCD >= @VendorCDFrom)
			and (@VendorCDTo is null or mv.VendorCD <= @VendorCDTo)
			and (@VendorName is null or mv.VendorName like '%'+ @VendorName + '%')
			and (@VendorKana is null or mv.VendorKana like '%'+ @VendorKana +'%')
			order by mv.VendorCD,ChangeDate
			

	   END
     ELSE
	   BEGIN
			Select
			mv.VendorCD,
			mv.VendorName,
			REPLACE(CONVERT(VARCHAR(10), mv.ChangeDate , 111),'-','/') as ChangeDate
			From M_Vendor mv
			where mv.DeleteFlg = @DeleteFlg 
			and (@VendorCDFrom is null or mv.VendorCD >= @VendorCDFrom)
			and (@VendorCDTo is null or mv.VendorCD <= @VendorCDTo)
			and (@VendorName is null or mv.VendorName like '%'+ @VendorName + '%')
			and (@VendorKana is null or mv.VendorKana like '%'+ @VendorKana +'%')
			and mv.ChangeDate <= @ChangeDate and mv.DeleteFlg=@DeleteFlg
			order by mv.VendorCD,mv.ChangeDate 
	   END

END

