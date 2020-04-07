 BEGIN TRY 
 Drop Procedure dbo.[M_BankShiten_Search]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	kyaw thet paing	<Author,,Name>
-- Create date: 2019-06-17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_BankShiten_Search]
	-- Add the parameters for the stored procedure here
	@BankCD varchar(4),
	@ChangeDate date,
	@DeleteFlg tinyint,
	@BranchCDFrom varchar(3),
	@BranchCDTo varchar(3),
	@BranchName varchar(30),
	@KanaName varchar(30),
	@SearchType varchar(1)--1->max,2->history,
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	if @SearchType = 1 
		begin
			select
			mbs.BranchCD,
			--mbs.BranchCD,
			mbs.BranchName,
			--mbs.BranchKana,
			CONVERT(VARCHAR(10), mbs.ChangeDate , 111) as ChangeDate
			from M_BankBranch mbs
			inner join F_BankShiten(cast(@ChangeDate as varchar(10))) fbs on mbs.BankCD = fbs.BankCD and  mbs.BranchCD = fbs.BranchCD and mbs.ChangeDate = fbs.ChangeDate and fbs.BankCD = @BankCD
			where (@BranchCDFrom is null or ( mbs.BranchCD >= @BranchCDFrom))
			and	(@BranchCDTo is null or ( mbs.BranchCD <= @BranchCDTo))
			and (@BranchName is null or (mbs.BranchName like '%' + @BranchName + '%'))
			and (@KanaName is null or (mbs.BranchKana like '%' + @KanaName + '%'))
			and mbs.DeleteFlg = @DeleteFlg
			and mbs.ChangeDate <= @ChangeDate
			order by mbs.BankCD,mbs.BranchCD,ChangeDate
		end
	else
		begin
			select
			mbs.BranchCD,
			--mbs.BranchCD,
			mbs.BranchName,
			--mbs.BranchKana,
			CONVERT(VARCHAR(10), mbs.ChangeDate , 111) as ChangeDate
			from M_BankBranch mbs
			where (@BranchCDFrom is null or ( mbs.BranchCD >= @BranchCDFrom))
			and	(@BranchCDTo is null or ( mbs.BranchCD <= @BranchCDTo))
			and (@BranchName is null or (mbs.BranchName like '%' + @BranchName + '%'))
			and (@KanaName is null or (mbs.BranchKana like '%' + @KanaName + '%'))
			and mbs.DeleteFlg = @DeleteFlg
			and mbs.ChangeDate <= @ChangeDate
			and BankCD = @BankCD
			order by mbs.BankCD,mbs.BranchCD,ChangeDate
		end
END

