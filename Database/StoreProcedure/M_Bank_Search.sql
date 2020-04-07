 BEGIN TRY 
 Drop Procedure dbo.[M_Bank_Search]
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
CREATE PROCEDURE [dbo].[M_Bank_Search]
	-- Add the parameters for the stored procedure here
	@Fields as varchar(1000),
	@BankCDFrom as varchar(6),
	@BankCDTo as varchar(6),
	@BankName as varchar(40),
	@BankKana varchar(40),
	@ChangeDate as date,
	@SearchType as int,
	@DeleteFlg as int,
	@OrderBy as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	--declare @query as varchar(1000)

	If @SearchType=0
	begin
		select
		--ROW_NUMBER() OVER(ORDER BY mb.BankCD,mb.ChangeDate) AS RowNo,
		fb.BankCD,
		fb.BankName,
		CONVERT(VARCHAR(10), fb.ChangeDate , 111) as ChangeDate
		from F_Bank(cast(@ChangeDate as varchar(10))) fb
		where (@BankCDFrom is null or ( fb.BankCD >= @BankCDFrom))
		and	(@BankCDTo is null or ( fb.BankCD <= @BankCDTo))
		and (@BankName is null or (fb.BankName like '%' + @BankName + '%'))
		and (@BankKana is null or (fb.BankKana like '%' + @BankKana + '%'))
		and fb.ChangeDate <= @ChangeDate and fb.DeleteFlg=@DeleteFlg
		order by BankCD,ChangeDate
	end
	Else
	begin
		select
			--ROW_NUMBER() OVER(ORDER BY mb.BankCD,mb.ChangeDate) AS RowNo,
			mb.BankCD,
			mb.BankName,
			CONVERT(VARCHAR(10), mb.ChangeDate , 111) as ChangeDate
			from M_Bank mb
			where (@BankCDFrom is null or ( mb.BankCD >= @BankCDFrom))
			and	(@BankCDTo is null or ( mb.BankCD <= @BankCDTo))
			and (@BankName is null or (mb.BankName like '%' + @BankName + '%'))
			and (@BankKana is null or (mb.BankKana like '%' + @BankKana + '%'))
			and mb.ChangeDate <= @ChangeDate and mb.DeleteFlg=@DeleteFlg
			order by BankCD,ChangeDate
	end
END

