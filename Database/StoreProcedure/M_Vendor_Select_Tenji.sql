-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Alter PROCEDURE M_Vendor_Select_Tenji
	-- Add the parameters for the stored procedure here
							@ChangeDate as datetime,
							@DeleteFlg as varchar,
							@VendorFlg as Varchar(10),
							@VendorCD as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		
					

						Select 
			mv.VendorCD,
			mv.VendorName,
			mv.DeleteFlg,
			REPLACE(CONVERT(VARCHAR(10), mv.ChangeDate , 111),'-','/') as ChangeDate
		From M_Vendor mv
			inner join F_Vendor(getdate()) fv on mv.VendorCD = fv.VendorCD  and mv.ChangeDate  = fv.ChangeDate 
			where mv.DeleteFlg = @DeleteFlg 
			and  fv.VendorCD = @VendorCD
			and mv.VendorFlg = @VendorFlg
			and mv.Changedate < =@ChangeDate
END
GO
