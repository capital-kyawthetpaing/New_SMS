 BEGIN TRY 
 Drop Procedure dbo.[M_Customer_CustomerName_Select]
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
CREATE PROCEDURE [dbo].[M_Customer_CustomerName_Select]
	-- Add the parameters for the stored procedure here

@JanCD varchar(13)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select CustomerCD,CustomerName
	from M_Customer
	where CustomerCD=@JanCD
	and ChangeDate<=convert(Date,GETDATE())

END

