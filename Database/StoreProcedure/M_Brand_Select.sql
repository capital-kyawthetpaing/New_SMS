 BEGIN TRY 
 Drop Procedure dbo.[M_Brand_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/****** Object:  StoredProcedure [M_Brand_Select]    */

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[M_Brand_Select]
	-- Add the parameters for the stored procedure here
	@BrandCD varchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MS.BrandCD
    	  ,MS.BrandName 
          ,MS.BrandKana
          ,MS.UsedFlg
          ,MS.InsertOperator
          ,CONVERT(varchar,MS.InsertDateTime) AS InsertDateTime
          ,MS.UpdateOperator
          ,CONVERT(varchar,MS.UpdateDateTime) AS UpdateDateTime
    FROM M_Brand MS
    WHERE MS.BrandCD = @BrandCD
    
    ORDER BY MS.BrandCD
END



