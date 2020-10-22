
/****** Object:  StoredProcedure [dbo].[M_Souko_BindForTanaoroshi]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[M_Souko_BindForTanaoroshi]
GO

/****** Object:  StoredProcedure [dbo].[M_Souko_BindForTanaoroshi]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Souko_BindForTanaoroshi]
    -- Add the parameters for the stored procedure here
    @Operator   as varchar(10),
    @ChangeDate as varchar(10)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT FS.SoukoCD,FS.SoukoName 
    FROM F_Souko(@ChangeDate) AS FS 
    WHERE FS.DeleteFlg = 0
    AND FS.SoukoType <> 5
    --Œ ŒÀ‚Ì‚ ‚é“X•Ü‚Ì‚Ý
    AND EXISTS(select MS.StoreCD
               from M_StoreAuthorizations MS
               INNER JOIN M_Staff AS MF
               ON MF.StaffCD = @Operator
               AND MF.ChangeDate <= CONVERT(date, @ChangeDate)
               --AND MF.StoreCD = MS.StoreCD
               AND MF.StoreAuthorizationsCD = MS.StoreAuthorizationsCD
               AND MF.DeleteFlg = 0
               where MS.ChangeDate <= CONVERT(date, @ChangeDate)
               AND MS.StoreCD = FS.StoreCD
               )
    ORDER BY FS.StoreCD, FS.SoukoCD
END

GO


