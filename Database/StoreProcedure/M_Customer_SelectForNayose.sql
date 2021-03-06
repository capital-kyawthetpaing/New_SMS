 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_SelectForNayose]
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
CREATE PROCEDURE [dbo].[M_Customer_SelectForNayose]
	-- Add the parameters for the stored procedure here
	 @StoreKBN  tinyint
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    declare  @Date as datetime=getdate()
    
    SELECT 
    CustomerCD
    CustomerName,
    ChangeDate,
    [Tel11],
    [Tel12],
    [Tel13],
    [Tel21],
    [Tel22],
    [Tel23] ,    
    MailAddress,
    ZipCD1,
    ZipCD2,
    Address1,
    Address2
    
    FROM F_Customer(cast(@Date as varchar)) 
    WHERE StoreKBN=@StoreKBN
    AND DeleteFlg = 0
    ORDER BY CustomerCD
END
