 BEGIN TRY 
 Drop Procedure dbo.[M_Settlement_Bind]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Settlement_Bind]
    -- Add the parameters for the stored procedure here
    @FileKBN as tinyint
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT M.PatternCD
          ,M.PatternName
          ,M.FileKBN
          ,M.FirstRecordNo
          ,M.RecordKBN
          ,M.MatchingKBN
          ,M.InsertOperator
          ,M.InsertDateTime
          ,M.UpdateOperator
          ,M.UpdateDateTime
    FROM M_Settlement AS M
--  WHERE ms.FileKBN = @FileKBN
    ORDER BY M.PatternCD
END


