 BEGIN TRY 
 Drop Procedure dbo.[M_ProgramSearch]
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
Create PROCEDURE [dbo].[M_ProgramSearch]
@ProgramID  varchar(20),
@ProgramName varchar(100)

AS
BEGIN
select *
from M_Program
where (@ProgramID is null or (ProgramID like '%'+@ProgramID+'%'))
and (@ProgramName is null or (ProgramName like '%'+@ProgramName+'%'))
order by ProgramID


END

