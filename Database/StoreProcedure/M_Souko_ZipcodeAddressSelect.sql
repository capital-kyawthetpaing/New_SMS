 BEGIN TRY 
 Drop Procedure dbo.[M_Souko_ZipcodeAddressSelect]
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
CREATE PROCEDURE [dbo].[M_Souko_ZipcodeAddressSelect]
	-- Add the parameters for the stored procedure here
	@ZipCD1  varchar(3),
    @ZipCD2 varchar(4),
	@SoukoCD varchar(10),
	@ChangeDate date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT [ZipCD1]
	      ,[ZipCD2]
	      ,[Address1]
	      ,[Address2]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
	FROM M_souko

    WHERE Soukocd=@SoukoCD
	AND ChangeDate=@ChangeDate
	AND (@ZipCD1 is null or ([ZipCD1] = @ZipCD1))
    AND (@ZipCD2 is null or ([ZipCD2] = @ZipCD2))
    ;
END
