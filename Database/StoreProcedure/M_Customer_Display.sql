 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_Display]
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
CREATE PROCEDURE [dbo].[M_Customer_Display]
	-- Add the parameters for the stored procedure here
	@Telephone varchar(13),
	--@Tel2 varchar(4),
	--@Tel3 varchar(4),
	@BirthDate date,
	@CustomerCD varchar(13),
	@KanaName varchar(30),
	@CustomerName varchar(80)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @todayDate as date = GETDATE();
	SELECT 
	ROW_NUMBER() Over( Order BY CustomerCD ASC ) AS No,
	CustomerCD + char(13) + char(10) AS CustomerCD,
	CustomerName + char(13) + char(10) + KanaName AS CustomerName,
	CASE 
		WHEN M_Customer.Tel11 IS NOT NULL THEN ZipCD1 + '-' + ZipCD2 + char(13) + char(10) + Tel11 + '-'+ Tel12 + '-' + Tel13 
		WHEN M_Customer.Tel11 IS NULL THEN ZipCD1 + '-' + ZipCD2 + char(13) + char(10) + Tel21 + '-'+ Tel22 + '-' + Tel23  
	END AS ZipCD,
	Address1  + char(13) + char(10) + Address2 AS [Address],
	Convert(varchar(10),LastSalesDate,111) + char(13) + char(10) + Convert(varchar(4),LastSalesStoreCD) AS SaleDate_StoreCD
	FROM M_Customer 
	WHERE DeleteFlg = 0
	AND ChangeDate <= @todayDate
	AND StoreKBN = 2
	AND (@BirthDate IS NULL OR Birthdate = @BirthDate)
	AND (@CustomerCD IS  NULL OR CustomerCD = @CustomerCD)
	AND (@KanaName IS NULL OR KanaName LIKE '%' + @KanaName + '%')
	ANd (@CustomerName IS NULL OR CustomerName LIKE '%' + @CustomerName + '%')
	--AND ((Tel11 IS NOT NULL AND ( @Telephone IS NULL OR Tel11 + Tel12 + Tel13 = @Telephone ))
	--OR (Tel11 IS NULL AND ( @Telephone IS NULL OR Tel21 + Tel22 + Tel23 = @Telephone )))
	AND (( @Telephone IS NULL OR( Tel11 + Tel12 + Tel13 = @Telephone ))
	OR (@Telephone IS NULL OR (Tel21 + Tel22 + Tel23 = @Telephone )))
	ORDER BY CustomerCD ASC 
END

