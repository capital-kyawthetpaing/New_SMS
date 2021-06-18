BEGIN TRY 
 Drop Procedure dbo.[M_Store_Bind]
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
CREATE PROCEDURE [dbo].[M_Store_Bind]
	-- Add the parameters for the stored procedure here
	@StoreCD as varchar(4),
	@ChangeDate as date,
	@Type as tinyint,
	@DeleteFlg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if @Type = 1
		begin
			select * from F_Store(@ChangeDate) fs
			where DeleteFlg = 0 
			and (StoreKBN = 2 OR StoreKBN = 3)
			order by StoreCD
		end
	if @Type = 2
		begin
			select StoreCD,StoreName from F_Store(@ChangeDate) fs
			where DeleteFlg = 0 
			order by StoreCD
		end
	if @Type = 3 -- MasterTouroku_Settouchi 2020_04_21
		begin
			select * from F_Store(@ChangeDate) fs
			where DeleteFlg = 0 
			and (StoreKBN = 1 OR StoreKBN = 2)
			order by StoreCD

		end

		if @Type = 4
		begin
			select * from F_Store(@ChangeDate) fs
			where DeleteFlg = 0 
			and StoreKBN != 3
			order by StoreCD
		end
END
