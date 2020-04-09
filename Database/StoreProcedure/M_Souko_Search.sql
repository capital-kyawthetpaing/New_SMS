 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_Search]
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
CREATE PROCEDURE [dbo].[M_Souko_Search]
	-- Add the parameters for the stored procedure here
	
	@Fields as varchar(1000),
	@SoukoCDFrom as varchar(6),
	@SoukoCDTo as varchar(6),
	@SoukoName as varchar(40),
	@StoreCD varchar(4),
	@SoukoType tinyint,
	@DeleteFlg as tinyint,
	@ChangeDate as date,
	--@OrderBy as varchar(50),
	@SearchType varchar(1)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Insert statements for procedure here
	declare @query as varchar(1000)
	if @SearchType = 1
	begin
	select
		ROW_NUMBER() OVER(ORDER BY ms.SoukoCD,ms.ChangeDate) AS RowNo,
		ms.SoukoCD,
		ms.SoukoName,
		mstore.StoreName,
		case when ms.SoukoType = 1 then 'WebMain倉庫'
		     when ms.SoukoType = 2 then 'Web倉庫' 
			 when ms.SoukoType = 3 then '店舗Main倉庫'
			 when ms.SoukoType = 4 then '店舗倉庫'
			 when ms.SoukoType = 5 then 'メーカー'
			 when ms.SoukoType = 6 then '仮想倉庫'
			 when ms.SoukoType = 7 then '店外倉庫'
			 when ms.SoukoType = 8 then '返品倉庫' end as SoukoType,
		--REPLACE(CONVERT(VARCHAR(10), ms.ChangeDate , 111),'/','-') as ChangeDate
		CONVERT(VARCHAR(10), ms.ChangeDate , 111) as ChangeDate
	from M_Souko ms
	--inner join F_Souko(@SoukoCD,cast(@ChangeDate as varchar(10))) fs on ms.SoukoCD=fs.SoukoCD and ms.ChangeDate=fs.ChangeDate
	inner join F_Souko(cast(@ChangeDate as varchar(10))) fs on ms.SoukoCD = fs.SoukoCD and ms.ChangeDate = fs.ChangeDate 
	inner join F_Store (cast(@ChangeDate as varchar(10))) mstore on mstore.StoreCD = ms.StoreCD
	where (@SoukoCDFrom is null or ( ms.SoukoCD >= @SoukoCDFrom))
	and	(@SoukoCDTo is null or ( ms.SoukoCD <= @SoukoCDTo))
	and (@SoukoName is null or (ms.SoukoName like '%' + @SoukoName + '%'))
	and (@StoreCD is null or(ms.StoreCD = @StoreCD))
	and (@SoukoType is null or(ms.SoukoType = @SoukoType))
	and ms.DeleteFlg = @DeleteFlg
	and ms.ChangeDate <= @ChangeDate
	order by SoukoCD,ChangeDate

	end
	else if @SearchType = 2
	begin
		select
		ms.SoukoCD,
		SoukoName,
		mstore.StoreName,
		case when ms.SoukoType = 1 then 'WebMain倉庫'
		     when ms.SoukoType = 2 then 'Web倉庫' 
			 when ms.SoukoType = 3 then '店舗Main倉庫'
			 when ms.SoukoType = 4 then '店舗倉庫'
			 when ms.SoukoType = 5 then 'メーカー'
			 when ms.SoukoType = 6 then '仮想倉庫'
			 when ms.SoukoType = 7 then '店外倉庫'
			 when ms.SoukoType = 8 then '返品倉庫' end as SoukoType,
		--REPLACE(CONVERT(VARCHAR(10), ms.ChangeDate , 111),'/','-') as ChangeDate
		CONVERT(VARCHAR(10), ms.ChangeDate , 111) as ChangeDate
	from M_Souko ms
	left join  F_Store (cast(@ChangeDate as varchar(10))) mstore on mstore.StoreCD = ms.StoreCD
	where (@SoukoCDFrom is null or ( ms.SoukoCD >= @SoukoCDFrom))
	and	(@SoukoCDTo is null or ( ms.SoukoCD <= @SoukoCDTo))
	and (@SoukoName is null or (ms.SoukoName like '%' + @SoukoName + '%'))
	and (@StoreCD is null or(ms.StoreCD = @StoreCD))
	and (@SoukoType is null or(ms.SoukoType = @SoukoType))
	and ms.DeleteFlg = @DeleteFlg
	and ms.ChangeDate <= @ChangeDate
	order by SoukoCD,ChangeDate

	end
END

