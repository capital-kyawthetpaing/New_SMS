use[CapitalSMS]
Go
 BEGIN TRY 
 Drop Procedure dbo.GetMaxTaxRate
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[GetMaxTaxRate]
	-- Add the parameters for the stored procedure here
	
	 @Flg as varchar(20),
	 @ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Case When @Flg=1 then Max(TaxRate1)   else max(TaxRate2)  end as TaxRate , 
				Max(FractionKBN)  as KBN
				
				from M_Salestax WHERE ChangeDate <= @ChangeDate
END
