 BEGIN TRY 
 Drop Procedure dbo.[M_DenominationKBN_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_DenominationKBN_SelectAll]    */

CREATE PROCEDURE M_DenominationKBN_SelectAll(
    -- Add the parameters for the stored procedure here
    @SystemKBN  tinyint
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
        
--        ,RIGHT('  ' +[Key],3) + ':' + [Char1] AS KeyAndChar1  --ComboBox用
    FROM M_DenominationKBN
    WHERE SystemKBN = (CASE WHEN ISNULL(@SystemKBN,0) = 0 THEN SystemKBN ELSE @SystemKBN END)
    AND StoreNotDisplayFLG = 0  --2019.12.20
    ORDER BY [DenominationCD]
    ;
END

