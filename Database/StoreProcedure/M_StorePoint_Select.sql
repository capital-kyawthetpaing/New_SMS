 BEGIN TRY 
 Drop Procedure dbo.[M_StorePoint_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Saff_Select]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_StorePoint_Select]
	-- Add the parameters for the stored procedure here
	@StoreCD varchar(10),
	@ChangeDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

		select top 1
		ISNULL(CONVERT(varchar, msp.ChangeDate,111),CONVERT(varchar, GETDATE(),111)) as 'ChangeDate',
		ISNULL(msp.PointRate,0) as 'PointRate',
		ISNULL(msp.ServicedayRate,0) as 'ServicedayRate',
		ISNULL(msp.ExpirationDate,0) as 'ExpirationDate',
		ISNULL(FORMAT(Convert(Int,msp.MaxPoint), '#,#'),0) as 'MaxPoint',
		ISNULL(FORMAT(Convert(Int,msp.TicketUnit), '#,#'),0) as 'TicketUnit',
		ISNULL(msp.Print1,'') as 'Print1',
		ISNULL(msp.Size1,1) as 'Size1',
		ISNULL(msp.Bold1,'OFF') as 'Bold1',

		ISNULL(msp.Print2,'') as 'Print2',
		ISNULL(msp.Size2,1) as 'Size2',
		ISNULL(msp.Bold2,'OFF') as 'Bold2',

		ISNULL(msp.Print3,'') as 'Print3',
		ISNULL(msp.Size3,1) as 'Size3',
		ISNULL(msp.Bold3,'OFF') as 'Bold3',
	

		ISNULL(msp.Print4,'') as 'Print4',
		ISNULL(msp.Size4,1) as 'Size4',
		ISNULL(msp.Bold4,'OFF') as 'Bold4',
	

		ISNULL(msp.Print5,'') as 'Print5',
		ISNULL(msp.Size5,1) as 'Size5',
		ISNULL(msp.Bold5,'OFF') as 'Bold5',
	

		ISNULL(msp.Print6,'') as 'Print6',
		ISNULL(msp.Size6,1) as 'Size6',
		ISNULL(msp.Bold6,'OFF') as 'Bold6',
	

		ISNULL(msp.Print7,'') as 'Print7',
		ISNULL(msp.Size7,1) as 'Size7',
		ISNULL(msp.Bold7,'OFF') as 'Bold7',
	

		ISNULL(msp.Print8,'') as 'Print8',
		ISNULL(msp.Size8,1) as 'Size8',
		ISNULL(msp.Bold8,'OFF') as 'Bold8',
	

		ISNULL(msp.Print9,'') as 'Print9',
		ISNULL(msp.Size9,1) as 'Size9',
		ISNULL(msp.Bold9,'OFF') as 'Bold9',
	

		ISNULL(msp.Print10,'') as 'Print10',
		ISNULL(msp.Size10,1) as 'Size10',
		ISNULL(msp.Bold10,'OFF') as 'Bold10',
	

		ISNULL(msp.Print11,'') as 'Print11',
		ISNULL(msp.Size11,1) as 'Size11',
		ISNULL(msp.Bold11,'OFF') as 'Bold11',
	

		ISNULL(msp.Print12,'') as 'Print12',
		ISNULL(msp.Size12,1) as 'Size12',
		ISNULL(msp.Bold12,'OFF') as 'Bold12',
	
		msp.DeleteFlg  
		
		from M_StorePoint as msp
		inner join F_StorePoint(cast(@ChangeDate as varchar(10))) fs on msp.StoreCD = fs.StoreCD and msp.ChangeDate = fs.ChangeDate 
		where msp.StoreCD=@StoreCD
		and msp.ChangeDate <= convert(Date,@ChangeDate)
		order by msp.ChangeDate desc



	--Else
	--begin
	--	select top 1
	--	ISNULL(CONVERT(varchar, ChangeDate,111),CONVERT(varchar, GETDATE(),111)) as 'ChangeDate',
	--	ISNULL(PointRate,0) as 'PointRate',
	--	ISNULL(ServicedayRate,0) as 'ServicedayRate',
	--	ISNULL(ExpirationDate,0) as 'ExpirationDate',
	--	ISNULL(MaxPoint,0) as 'MaxPoint',
	--	ISNULL(TicketUnit,0) as 'TicketUnit',
	--	ISNULL(Print1,'') as 'Print1',
	--	ISNULL(Size1,1) as 'Size1',
	--	ISNULL(Bold1,'OFF') as 'Bold1',

	--	ISNULL(Print2,'') as 'Print2',
	--	ISNULL(Size2,1) as 'Size2',
	--	ISNULL(Bold2,'OFF') as 'Bold2',

	--	ISNULL(Print3,'') as 'Print3',
	--	ISNULL(Size3,1) as 'Size3',
	--	ISNULL(Bold3,'OFF') as 'Bold3',
	

	--	ISNULL(Print4,'') as 'Print4',
	--	ISNULL(Size4,1) as 'Size4',
	--	ISNULL(Bold4,'OFF') as 'Bold4',
	

	--	ISNULL(Print5,'') as 'Print5',
	--	ISNULL(Size5,1) as 'Size5',
	--	ISNULL(Bold5,'OFF') as 'Bold5',
	

	--	ISNULL(Print6,'') as 'Print6',
	--	ISNULL(Size6,1) as 'Size6',
	--	ISNULL(Bold6,'OFF') as 'Bold6',
	

	--	ISNULL(Print7,'') as 'Print7',
	--	ISNULL(Size7,1) as 'Size7',
	--	ISNULL(Bold7,'OFF') as 'Bold7',
	

	--	ISNULL(Print8,'') as 'Print8',
	--	ISNULL(Size8,1) as 'Size8',
	--	ISNULL(Bold8,'OFF') as 'Bold8',
	

	--	ISNULL(Print9,'') as 'Print9',
	--	ISNULL(Size9,1) as 'Size9',
	--	ISNULL(Bold9,'OFF') as 'Bold9',
	

	--	ISNULL(Print10,'') as 'Print10',
	--	ISNULL(Size10,1) as 'Size10',
	--	ISNULL(Bold10,'OFF') as 'Bold10',
	

	--	ISNULL(Print11,'') as 'Print11',
	--	ISNULL(Size11,1) as 'Size11',
	--	ISNULL(Bold11,'OFF') as 'Bold11',
	

	--	ISNULL(Print12,'') as 'Print12',
	--	ISNULL(Size12,1) as 'Size12',
	--	ISNULL(Bold12,'OFF') as 'Bold12',
	
	--	DeleteFlg  
		

	--	from M_StorePoint as msp
	--	inner join F_StorePoint(cast(@ChangeDate as varchar(10))) fs on msp.StoreCD = fs.StoreCD and msp.ChangeDate = fs.ChangeDate 
	--	where msp.StoreCD=@StoreCD
	--	and msp.ChangeDate <= convert(Date,@ChangeDate)
	--	order by msp.ChangeDate desc
	--end
END


