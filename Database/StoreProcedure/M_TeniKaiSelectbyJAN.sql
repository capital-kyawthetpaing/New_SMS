use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.M_TeniKaiSelectbyJAN
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
Create PROCEDURE [dbo].[M_TeniKaiSelectbyJAN]
	-- Add the parameters for the stored procedure here
	 @CustomerCD as varchar(20),
	 @JanCD as varchar(13),
	 @LastYearTerm as varchar (10),
	 @LastSeason as varchar(50),
	 @VendorCD as varchar(15),
	 @ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		
	Select		
		A.SKUCD
		,A.SKUName
		,Null as ColorNo
		,A.ColorName as ColorName
		,null as SizeNo
		,A.SizeName as SizeName
		,A.SiireTanka as ShiireTanka
		,A.SalePriceOutTax as SalePriceOutTax
		,null as AdminNo
		,A.TaniCD
		,mp.Char1 as TaniName
		,A.TaxRateFlg as TaxRateFlg																																							
		From	M_TenzikaiShouhin A	
		left outer join 
		M_multiporpose mp on mp.[ID] =201 and mp.[KEY] = A.TaniCD and deleteFlg = 0 																														
		Left Outer Join		
		M_Customer	B																												
		On			B.CustomerCD					=	@CustomerCD
		AND	A.TankaCD=B.TankaCD																											
		Where		A.JanCD							=	@JanCD																													
		AND			A.LastYearTerm					=	@LastYearTerm																														
		AND			A.LastSeason					=	@LastSeason																													
		AND			A.VendorCD						=	@VendorCD																														
		AND			A.DeleteFLG						=	0																														
		AND			B.DeleteFLG						=	0																														
		AND			B.ChangeDate					<=	@ChangeDate										
																		
END
