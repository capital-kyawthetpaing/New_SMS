 BEGIN TRY 
 Drop Procedure [dbo].[M_Customer_Display]
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
	@CustomerName varchar(80),
	@MainStoreCD varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @todayDate as date = GETDATE();
	SELECT 
	ROW_NUMBER() Over( Order BY customer.CustomerCD ASC ) AS [No],
 	ISNULL(customer.CustomerCD, '') + char(13) + char(10) AS [CustomerCD],
	(ISNULL(customer.CustomerName, '') + char(13) + char(10) + ISNULL(customer.KanaName, '')) AS [CustomerName],
	CASE 
		WHEN customer.Tel11 IS NOT NULL 
			THEN ISNULL(customer.ZipCD1, '') + '-' + ISNULL(customer.ZipCD2, '') + char(13) + char(10) + ISNULL(customer.Tel11,'') + '-'+ ISNULL(customer.Tel12,'') + '-' + ISNULL(customer.Tel13,'') 
		WHEN customer.Tel11 IS NULL 
			THEN ISNULL(customer.ZipCD1,'') + '-' + ISNULL(customer.ZipCD2,'') + char(13) + char(10) + ISNULL(customer.Tel21,'') + '-'+ ISNULL(customer.Tel22,'') + '-' + ISNULL(customer.Tel23,'')  
	END AS [ZipCD],
	(ISNULL(customer.Address1, '')  + char(13) + char(10) + ISNULL(customer.Address2, '')) AS [Address],
	(ISNULL(Convert(varchar(10),customer.LastSalesDate,111),'') + char(13) + char(10) + ISNULL(Convert(varchar(4),store.StoreName),'')) AS SaleDate_StoreCD
	FROM F_Customer(@todayDate) customer
	LEFT JOIN F_Store(@todayDate) store ON store.StoreCD = customer.LastSalesStoreCD
	WHERE customer.StoreKBN = 2
	AND customer.DeleteFlg = 0
	AND (@BirthDate IS NULL OR customer.Birthdate = @BirthDate)
	AND (@CustomerCD IS  NULL OR customer.CustomerCD = @CustomerCD)
	AND (@KanaName IS NULL OR customer.KanaName LIKE '%' + @KanaName + '%')
	ANd (@CustomerName IS NULL OR customer.CustomerName LIKE '%' + @CustomerName + '%')
	AND (( @Telephone IS NULL OR( customer.Tel11 + customer.Tel12 + customer.Tel13 LIKE '%' + @Telephone + '%'))
	OR (@Telephone IS NULL OR (customer.Tel21 + customer.Tel22 + customer.Tel23 LIKE '%' + @Telephone + '%')))
	AND (@MainStoreCD IS NULL OR(customer.MainStoreCD = @MainStoreCD))
	ORDER BY customer.CustomerCD ASC 
END




GO
