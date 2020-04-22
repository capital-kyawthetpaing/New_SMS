 BEGIN TRY 
 Drop Procedure dbo.[M_Location_SelectData]
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
CREATE PROCEDURE [dbo].[M_Location_SelectData]
    -- Add the parameters for the stored procedure here
    @SoukoCD varchar(13),
    @TanaCD varchar(13),
    @ChangeDate date
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT ML.SoukoCD
        , ML.TanaCD
        , ML.ChangeDate
    FROM M_Location AS ML   
    WHERE ML.SoukoCD = @SoukoCD
    AND ML.TanaCD = @TanaCD
    AND ML.ChangeDate <= @ChangeDate
    AND ML.DeleteFlg = 0
    ORDER BY ML.ChangeDate desc
END

