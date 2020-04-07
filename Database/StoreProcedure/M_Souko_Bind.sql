 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_Bind]
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
CREATE PROCEDURE [dbo].[M_Souko_Bind] 
	-- Add the parameters for the stored procedure here
	@SoukoType as tinyint,
	@ChangeDate as date,
	@DeleteFlg as tinyint
	--@data as varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--Select TOP 1 SoukoName,SoukoCD
	--From M_Souko
	--Where StoreCD = (Select Top 1 StoreCD From M_Staff Where StaffCD = @data)

	select ms.SoukoCD,ms.SoukoName from --M_Souko ms
	--inner join 
	F_Souko(cast(@ChangeDate as varchar(10))) ms 

--	AND ms.SoukoType = @SoukoType
	where ms.SoukoType IN (3,4)
	ORDER BY ms.SoukoCD
						
END

