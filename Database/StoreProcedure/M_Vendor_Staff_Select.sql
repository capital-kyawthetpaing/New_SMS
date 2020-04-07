 BEGIN TRY 
 Drop Procedure dbo.[M_Vendor_Staff_Select]
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
CREATE PROCEDURE [dbo].[M_Vendor_Staff_Select]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
	 ms.StaffCD,
	 ms.StaffName,
	 convert(varchar(10),ms.LeaveDate,111) as LeaveDate,
	 ms.DeleteFlg 
	 From M_Staff ms 
	inner join (select * from F_Staff(cast(@ChangeDate as varchar(10)))) fs on ms.StaffCD = fs.StaffCD
	Where ms.StaffCD = @StaffCD
	AND ms.ChangeDate <= @ChangeDate
END

