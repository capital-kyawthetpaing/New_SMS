 BEGIN TRY 
 Drop Procedure dbo.[M_Staff_Display]
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
CREATE PROCEDURE [dbo].[M_Staff_Display]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10),
	@ChangeDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		staff.StaffName,
		staff.StaffKana,
		staff.StoreCD,
		q1.StoreName,
		staff.BMNCD,
		MP.Char1,
		staff.MenuCD,
		q2.MenuName,
		staff.StoreMenuCD,
		q2.MenuName,
		staff.AuthorizationsCD,
		q3.AuthorizationsName,
		staff.StoreAuthorizationsCD,
		q4.StoreAuthorizationsName,
		staff.PositionCD,
		MTP.Char1,
		staff.[Password],
		Convert(varchar(10),staff.JoinDate,111) AS JoinDate,
		Convert(varchar(10),staff.LeaveDate,111) As LeaveDate,
		staff.ReceiptPrint,
		staff.Remarks,
		staff.DeleteFlg
	FROM M_Staff staff LEFT JOIN

(SELECT S.StoreCD,S.StoreName FROM M_Store S INNER JOIN F_Store(cast(@ChangeDate as varchar(10))) fs ON S.StoreCD = fs.StoreCD and fs.ChangeDate = s.ChangeDate ) q1 
ON staff.StoreCD = q1.StoreCD 
INNER JOIN

M_Menu q2 
ON staff.MenuCD = q2.MenuID
INNER JOIN

(SELECT A.AuthorizationsCD,A.AuthorizationsName from M_Authorizations A INNER JOIN F_Authorizations(cast(@ChangeDate as varchar(10))) fa ON A.AuthorizationsCD = fa.AuthorizationsCD AND fa.ChangeDate = A.ChangeDate) q3 
ON staff.AuthorizationsCD  = q3.AuthorizationsCD
INNER JOIN

(SELECT SA.StoreAuthorizationsCD,SA.StoreAuthorizationsName FROM M_StoreAuthorizations SA INNER JOIN F_StoreAuthorizations(cast(@ChangeDate as varchar(10))) fsa ON SA.StoreAuthorizationsCD = fsa.StoreAuthorizationsCD ) q4 
ON staff.StoreAuthorizationsCD = q4.StoreAuthorizationsCD
INNER JOIN

M_MultiPorpose MP ON staff.BMNCD = MP.[Key] AND MP.ID = '209'
INNER JOIN
M_MultiPorpose MTP ON staff.PositionCD = MTP.[Key] AND MTP.ID= '214'

WHERE staff.ChangeDate =@ChangeDate
AND staff.StaffCD = @StaffCD

END



