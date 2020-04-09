 BEGIN TRY 
 Drop Procedure dbo.[M_Staff_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_Saff_Select]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Staff_Select]
	-- Add the parameters for the stored procedure here
	@StaffCD varchar(10),
    @ChangeDate varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  top 1 MS.StaffCD
          ,CONVERT(varchar, MS.ChangeDate,111) AS ChangeDate
          ,MS.StaffName
          ,MS.StaffKana
          ,MS.StoreCD
          ,MS.BMNCD
          ,MS.MenuCD
          ,MS.AuthorizationsCD
          ,MS.StoreAuthorizationsCD
          ,MS.PositionCD
          ,CONVERT(varchar,MS.JoinDate,111) AS JoinDate
          ,CONVERT(varchar,MS.LeaveDate,111) AS LeaveDate
          ,MS.Password
          ,MS.Remarks
          ,MS.DeleteFlg
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    FROM F_Staff(cast(@ChangeDate as varchar(10))) MS
    WHERE MS.StaffCD = @StaffCD
    AND MS.ChangeDate <= CONVERT(DATE, @ChangeDate)
    ORDER BY MS.ChangeDate desc
END


