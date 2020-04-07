 BEGIN TRY 
 Drop Procedure dbo.[Select_SearchName]
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
CREATE PROCEDURE [dbo].[Select_SearchName]
	-- Add the parameters for the stored procedure here
	@ChangeDate date,
	@CD1 varchar(50),
	@CD2 varchar(50),
	@CD3 varchar(50),
	@Type int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if(@Type=1)   --For Bank
	begin

		select fb.BankName as 'Name'
		from  F_Bank(cast(@ChangeDate as varchar(10))) fb
		where fb.BankCD=@CD1 
		and fb.ChangeDate <= @ChangeDate

	end

	else if(@Type=2)	 --For Branch
	begin

		select fbs.BranchName as 'Name'
		from  F_BankShiten(cast(@ChangeDate as varchar(10))) fbs
		where fbs.BankCD=@CD1 
		and fbs.BranchCD=@CD2
		and fbs.ChangeDate <= @ChangeDate

	end

	else if(@Type = 3) --For Store
	begin 

		select fs.StoreName as 'Name'
		from  F_Store(cast(@ChangeDate as varchar(10)))  fs
		where fs.StoreCD = @CD1
		and fs.ChangeDate <= @ChangeDate

	end

	else if(@Type= 4)   -- For Vendor
	begin

		select fv.VendorName as 'Name'
		from  F_Vendor(cast(@ChangeDate as varchar(10)))  fv 
		where fv.VendorCD = @CD1
		and fv.ChangeDate <= @ChangeDate

	end

	else if(@Type= 5)   -- For Staff
	begin

		select fs.StaffName as 'Name'
		from F_Staff(cast(@ChangeDate as varchar(10)))  fs
		where fs.StaffCD = @CD1
		and fs.ChangeDate <= @ChangeDate

	end

	else if(@Type= 6)   --For SKU
	begin
		Select fs.SKUName as 'Name'
		from  F_SKU(cast(@ChangeDate as varchar(10)))  fs
		where fs.SKUCD = @CD1
		and fs.ChangeDate <= @ChangeDate
	end

	else if(@Type= 7)   --For Kouza
	begin
		Select fk.KouzaName as 'Name'
		from  F_Kouza(cast(@ChangeDate as varchar(10)))  fk
		where fk.KouzaCD = @CD1
		and fk.ChangeDate <= @ChangeDate
	end

	else if(@Type= 8)   --For Customer
	begin
		Select fc.CustomerName as 'Name'
		from  F_Customer(cast(@ChangeDate as varchar(10)))  fc
		where fc.CustomerCD = @CD1
		and fc.ChangeDate <= @ChangeDate
	end

	else if(@Type= 9)   --For HanyouKeyStart
	begin
		Select Char1 as 'Name'
		from  M_MultiPorpose
		where [key]=@CD1
		and ID=@CD2
	end

	else if(@Type= 10)   --For HanyouKeyEnd
	begin
		Select Char2 as 'Name'
		from  M_MultiPorpose
		where ID=@CD2
		and Char1=@CD3 
		--and Char2=@CD1 
		and [key]=@CD1 
	end

	else if(@Type= 11)   --For Brand
	begin
		select BrandName as Name
		From M_Brand 
		where BrandCD=@CD1

	end

	else if(@Type= 12)   --For Sport
	begin
		select Char1 as Name
		From M_MultiPorpose 
		Where ID='202'
		and [Key] = @CD1

	end

	else if(@Type= 13)   --For Segment
	begin
		select Char1 as Name
		From M_MultiPorpose 
		Where ID='203'
		and [Key] = @CD1

	end
END

