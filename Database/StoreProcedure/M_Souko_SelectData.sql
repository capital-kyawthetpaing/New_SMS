

/****** Object:  StoredProcedure [dbo].[M_Souko_SelectData]    Script Date: 6/11/2019 2:17:53 PM ******/
DROP PROCEDURE [dbo].[M_Souko_SelectData]
GO

/****** Object:  StoredProcedure [dbo].[M_Souko_SelectData]    Script Date: 6/11/2019 2:17:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Souko_SelectData]
	-- Add the parameters for the stored procedure here
	@SoukoCD varchar(13),
	@ChangeDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MS.SoukoName
		, MS.StoreCD
		, MS.SoukoType
	FROM M_Souko AS MS	
	WHERE MS.SoukoCD = @SoukoCD
	AND MS.ChangeDate <= @ChangeDate
	AND MS.DeleteFlg = 0
	ORDER BY MS.ChangeDate desc
END

GO


