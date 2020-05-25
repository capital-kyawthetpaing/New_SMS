
/****** Object:  StoredProcedure [dbo].[M_MovePurpose_Bind]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[M_MovePurpose_Bind]
GO

/****** Object:  StoredProcedure [dbo].[M_MovePurpose_Bind]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_MovePurpose_Bind]
    -- Add the parameters for the stored procedure here
    @MoveFLG as tinyint,
    @MoveRequestFLG as tinyint
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT CONVERT(varchar, M.MovePurposeKBN) AS MovePurposeKBN
          ,M.MovePurposeName 
    FROM M_MOVEPURPOSE M
    WHERE M.MoveFLG = (CASE WHEN @MoveFLG = 1 THEN 1 ELSE M.MoveFLG END)
    AND M.MoveRequestFLG = (CASE WHEN @MoveRequestFLG = 1 THEN 1 ELSE M.MoveRequestFLG END)
    ORDER BY M.DisplayOrder
END

GO


