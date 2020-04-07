 BEGIN TRY 
 Drop Procedure dbo.[M_DenominationKBN_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_DenominationKBN_Select]    */

CREATE PROCEDURE [dbo].[M_DenominationKBN_Select](
    -- Add the parameters for the stored procedure here
    @DenominationCD  varchar(3)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    -- Insert statements for procedure here
    SELECT [DenominationCD]
          ,[DenominationName]
          ,[SystemKBN]
          ,[CardCompany]
          ,(SELECT IDName FROM M_MultiPorpose AS M WHERE M.ID =303 AND M.[Key] = CardCompany) AS CardCompanyName
          ,[CalculationKBN]
          ,[MainFLG]
    FROM M_DenominationKBN
    WHERE DenominationCD = @DenominationCD
    ;
END


