 BEGIN TRY 
 Drop Procedure dbo.[SelectApprovalData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SelectApprovalData]
    (    @Operator  varchar(10)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT distinct M.StoreCD
        ,M.ApprovalStaffCD11
        ,M.ApprovalStaffCD12
        ,M.ApprovalStaffCD21
        ,M.ApprovalStaffCD22
        ,M.ApprovalStaffCD31
        ,M.ApprovalStaffCD32
    FROM M_Store AS M
    INNER JOIN (SELECT M.StoreCD,MAX(M.ChangeDate) AS ChangeDate
    	FROM M_Store AS M
    	WHERE M.ChangeDate <= CONVERT(date, SYSDATETIME())
    	GROUP BY M.StoreCD) AS MM
    ON M.StoreCD = MM.StoreCD
    AND M.ChangeDate = MM.ChangeDate
    WHERE (M.ApprovalStaffCD11 = @Operator
        OR M.ApprovalStaffCD12 = @Operator
        OR M.ApprovalStaffCD21 = @Operator
        OR M.ApprovalStaffCD22 = @Operator
        OR M.ApprovalStaffCD31 = @Operator
        OR M.ApprovalStaffCD32 = @Operator)
    AND M.DeleteFlg = 0
    ;

END


