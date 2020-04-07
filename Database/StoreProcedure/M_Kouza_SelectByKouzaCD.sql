 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_SelectByKouzaCD]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_Kouza_Select]    */
CREATE PROCEDURE [dbo].[M_Kouza_SelectByKouzaCD](
    -- Add the parameters for the stored procedure here
    @KouzaCD  varchar(3),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
   -- Insert statements for procedure here
	SELECT top 1 [KouzaCD]
	,CONVERT(varchar, mkz.ChangeDate,111) AS ChangeDate
	,[KouzaName]
	,mkz.BankCD
	,mkz.BranchCD
	,fb.BankName
	,fbs.BranchName
	,[KouzaKBN]
	,[KouzaMeigi]
	,[KouzaNO]
	,[Print1]
	,[Print2]
	,[Print3]
	,[Print4]
	,IsNull(FORMAT(Convert(Int,Fee11), '#,#'),0) as 'Fee11'
    ,IsNull(FORMAT(Convert(Int,Tax11), '#,#'),0) as 'Tax11'
	,IsNull(FORMAT(Convert(Int,Amount1), '#,#'),0) as 'Amount1'
    ,IsNull(FORMAT(Convert(Int,Fee12), '#,#'),0) as 'Fee12'
	,IsNull(FORMAT(Convert(Int,Tax12), '#,#'),0) as 'Tax12'
    ,IsNull(FORMAT(Convert(Int,Fee21), '#,#'),0) as 'Fee21'
	,IsNull(FORMAT(Convert(Int,Tax21), '#,#'),0) as 'Tax21'
	,IsNull(FORMAT(Convert(Int,Amount2), '#,#'),0) as 'Amount2'
    ,IsNull(FORMAT(Convert(Int,Fee22), '#,#'),0) as 'Fee22'
	,IsNull(FORMAT(Convert(Int,Tax22), '#,#'),0) as 'Tax22'
    ,IsNull(FORMAT(Convert(Int,Fee31), '#,#'),0) as 'Fee31'
    ,IsNull(FORMAT(Convert(Int,Tax31), '#,#'),0) as 'Tax31'
	,IsNull(FORMAT(Convert(Int,Amount3), '#,#'),0) as 'Amount3'
    ,IsNull(FORMAT(Convert(Int,Fee32), '#,#'),0) as 'Fee32'
	,IsNull(FORMAT(Convert(Int,Tax32), '#,#'),0) as 'Tax32'
	,[CompanyCD]
	,[CompanyName]
	,mkz.[Remarks]
	,mkz.[DeleteFlg]
	,mkz.[UsedFlg]
	,mkz.[InsertOperator]
	,CONVERT(varchar,mkz.[InsertDateTime]) AS InsertDateTime
	,mkz.[UpdateOperator]
	,CONVERT(varchar,mkz.[UpdateDateTime]) AS UpdateDateTime
	FROM M_Kouza as mkz
	left join F_Bank(cast(@ChangeDate as varchar(10))) fb on fb.BankCD=mkz.BankCD
	left join F_BankShiten(cast(@ChangeDate as varchar(10))) fbs on mkz.BankCD = fbs.BankCD and mkz.BranchCD = fbs.BranchCD
	--left join (
	--		select mb.BankCD,mb.BankName,mb.ChangeDate from M_Bank mb inner join F_Bank(cast(@ChangeDate as varchar(10))) fb 
	--		on mb.BankCD=fb.BankCD) mfb  on mkz.BankCD = mfb.BankCD
	--outer apply (
	--	select mbs.BranchCD,mbs.BranchName,mbs.ChangeDate from M_BankShiten mbs inner join F_BankShiten(mkz.BankCD,cast(@ChangeDate as varchar(10))) fbs
	--	 on mbs.BranchCD=fbs.BranchCD) mfbs

	WHERE (@KouzaCD is null or(mkz.KouzaCD = @KouzaCD))
	and (@ChangeDate is null or (mkz.ChangeDate = @ChangeDate))

END


