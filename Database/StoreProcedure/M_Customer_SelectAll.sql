 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_SelectAll]
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
CREATE PROCEDURE [dbo].[M_Customer_SelectAll]
	-- Add the parameters for the stored procedure here
	 @CustomerCD  varchar(13)
	 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare  @Date as datetime=getdate()
	SELECT   
	CustomerCD,
	FirstName,
	LastName,
	GroupName,
	KanaName,
	Sex,
	CONVERT(VARCHAR(10),Birthdate,111) AS Birthdate ,
	[Tel11],
    [Tel12],
    [Tel13],
     [Tel21],
     [Tel22],
     [Tel23] ,    
	MailAddress,
	MailAddress2,
	DMFlg,
	DeleteFlg,
	ZipCD1,
	ZipCD2,
	Address1,
	Address2
	

	FROM M_Customer
	WHERE DeleteFlg=0 AND StoreKBN=2 AND CustomerKBN=1 AND BillingType=1 AND BillingFLG=1 AND CollectFLG=1 
	AND CustomerCD=@CustomerCD
	AND ChangeDate<=(	select  fc.ChangeDate  from F_Customer(cast(@Date as varchar)) fc where fc.CustomerCD=@CustomerCD) 
END
