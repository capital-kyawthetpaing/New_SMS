 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_Search]
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
CREATE PROCEDURE [dbo].[M_Kouza_Search]
	-- Add the parameters for the stored procedure here
	@Fields as varchar(1000),
	@KouzaCDFrom as varchar(6),
	@KouzaCDTo as varchar(6),
	@KouzaName as varchar(40),
	@ChangeDate as date,
	@DeleteFlg tinyint,
	@OrderBy as varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	declare @query as varchar(1000)

	select
		ROW_NUMBER() OVER(ORDER BY mkz.KouzaCD,mkz.ChangeDate) AS RowNo,
		mkz.KouzaCD,
		mkz.KouzaName,
		fb.BankName,
		mbs.BranchName,
		mkz.Remarks,
		mkz.KouzaNO,
		REPLACE(CONVERT(VARCHAR(10), mkz.ChangeDate , 111),'/','-') as ChangeDate
	from M_Kouza mkz 
	inner join F_Bank(cast(@ChangeDate as varchar(10))) fb on mkz.BankCD = fb.BankCD and mkz.ChangeDate = fb.ChangeDate 
	inner join F_BankShiten(@ChangeDate) mbs on mkz.BranchCD=mbs.BranchCD and mbs.BankCD=fb.BankCD
	where (@KouzaCDFrom is null or ( mkz.KouzaCD >= @KouzaCDFrom))
	and	(@KouzaCDTo is null or ( mkz.KouzaCD <= @KouzaCDTo))
	and (@KouzaName is null or (mkz.KouzaName like '%' + @KouzaName + '%'))
	and mkz.ChangeDate <= @ChangeDate
	order by KouzaCD,ChangeDate
END

