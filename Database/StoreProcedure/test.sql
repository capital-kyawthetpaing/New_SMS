 BEGIN TRY 
 Drop Procedure dbo.[test]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[test]
AS 
BEGIN

    SELECT * FROM sys.databases

    RETURN 2
END

