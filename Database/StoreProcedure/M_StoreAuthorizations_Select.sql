 BEGIN TRY 
 Drop Procedure dbo.[M_StoreAuthorizations_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_StoreAuthorizations_Select]    */
-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_StoreAuthorizations_Select]
    -- Add the parameters for the stored procedure here
    @StoreAuthorizationsCD varchar(4)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    select MS.StoreCD
    from M_StoreAuthorizations MS
    where MS.StoreAuthorizationsCD = @StoreAuthorizationsCD
    AND MS.ChangeDate <= GETDATE()
    GROUP BY MS.StoreCD
	;
	
END


