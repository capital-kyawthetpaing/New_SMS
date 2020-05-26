
/****** Object:  StoredProcedure [dbo].[M_MovePurpose_Select]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[M_MovePurpose_Select]
GO

/****** Object:  StoredProcedure [dbo].[M_MovePurpose_Select]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_MovePurpose_Select]
    -- Add the parameters for the stored procedure here
    @MovePurposeKBN as tinyint,
    @MoveFLG as tinyint,
    @MoveRequestFLG as tinyint
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT M.MovePurposeKBN
          ,M.MovePurposeName
          ,M.DisplayOrder
          ,M.MovePurposeType
          ,M.MoveRequestFLG
          ,M.MoveFLG
          ,M.ToSoukoFLG
          ,M.MailFLG 
    FROM M_MOVEPURPOSE M
    WHERE M.MoveFLG = (CASE WHEN @MoveFLG = 1 THEN 1 ELSE M.MoveFLG END)
    AND M.MoveRequestFLG = (CASE WHEN @MoveRequestFLG = 1 THEN 1 ELSE M.MoveRequestFLG END)
    AND M.MovePurposeKBN = @MovePurposeKBN
    
END

GO


