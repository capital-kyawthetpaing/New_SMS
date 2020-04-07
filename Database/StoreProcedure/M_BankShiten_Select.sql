 BEGIN TRY 
 Drop Procedure dbo.[M_BankShiten_Select]
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
CREATE PROCEDURE [dbo].[M_BankShiten_Select]
	-- Add the parameters for the stored procedure here
	@BankCD varchar(4),
	@BranchCD varchar(3),
    @ChangeDate varchar(10)
	--@DeleteFlg tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	  SELECT BranchName,BranchKana,Remarks,DeleteFlg,UsedFlg
		FROM M_BankBranch 		

		WHERE [BankCD]=@BankCD
		AND	[BranchCD] = @BranchCD
		AND	ChangeDate=CONVERT(DATE, @ChangeDate)
		
		ORDER BY ChangeDate desc;
END

