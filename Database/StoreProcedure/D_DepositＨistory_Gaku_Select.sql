 BEGIN TRY 
 Drop Procedure dbo.[D_DepositＨistory_Gaku_Select]
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
CREATE PROCEDURE [dbo].[D_DepositＨistory_Gaku_Select] 
	-- Add the parameters for the stored procedure here
	@DepositKBN as tinyint,
	@DepositKBN1 as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select SUM(DepositGaku) DepositGaku
	From D_DepositＨistory
	Where DepositKBN = @DepositKBN or DepositKBN = @DepositKBN1
END

