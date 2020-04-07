 BEGIN TRY 
 Drop Procedure dbo.[D_PayClose_Search]
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
CREATE PROCEDURE [dbo].[D_PayClose_Search]
	-- Add the parameters for the stored procedure here
	 @PayeeCD as varchar(13),
	 @ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  
	
			FORMAT(dph.PayCloseProcessingDateTime,'yyyy-MM-dd hh:mm') as PayCloseProcessingDateTime,
			dph.PayCloseDate,
			dph.PayeeCD,
			mv.VendorName,
			case 
					when ProcessingKBN=1 then '支払締' 
					when ProcessingKBN=2 then '支払締キャンセル' 
					else '支払確定' 
			end as ProcessingKBN

	FROM	D_PayCloseHistory as dph
			left outer join F_Vendor(@ChangeDate) as mv on mv.PayeeCD =dph.PayeeCD and mv.PayeeFlg=1
													
	WHERE 
			dph.DeleteDateTime is null
			and IsNull(mv.deleteflg,0) =0
			and dph.PayeeCD=@PayeeCD
	ORDER BY 
			dph.PayCloseProcessingDateTime,
			dph.PayeeCD DESC
END

