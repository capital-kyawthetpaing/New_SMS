 BEGIN TRY 
 Drop Procedure dbo.[M_Store_BindData]
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
CREATE PROCEDURE [dbo].[M_Store_BindData]
	-- Add the parameters for the stored procedure here
	 @StoreCD  varchar(4),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--declare @dataCD as varchar(4) = (Select StoreCD From M_Staff Where StaffCD = @StoreCD and ChangeDate <= @ChangeDate)

	--Select StoreName ,StoreCD
	--From M_Store
	--Where StoreCD = @dataCD and ChangeDate <= @ChangeDate

	select * 
	from  F_Store(@ChangeDate) fst
	inner join F_Staff(@ChangeDate) fs on fst.StoreCD=fs.StoreCD 
	where fs.StoreCD=@StoreCD

END

