 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [M_Kouza_Select]    */
CREATE PROCEDURE [dbo].[M_Kouza_Select](
    -- Add the parameters for the stored procedure here
    @KouzaCD  varchar(3),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 [KouzaCD]
          ,CONVERT(varchar, ChangeDate,111) AS ChangeDate
          ,[KouzaName]
          ,[BankCD]
          ,[BranchCD]
          ,[KouzaKBN]
          ,[KouzaMeigi]
          ,[KouzaNO]
          ,[Print1]
          ,[Print2]
          ,[Print3]
          ,[Print4]
          ,[Fee11]
          ,[Tax11]
          ,[Amount1]
          ,[Fee12]
          ,[Tax12]
          ,[Fee21]
          ,[Tax21]
          ,[Amount2]
          ,[Fee22]
          ,[Tax22]
          ,[Fee31]
          ,[Tax31]
          ,[Amount3]
          ,[Fee32]
          ,[Tax32]
          ,[CompanyCD]
          ,[CompanyName]
          ,[Remarks]
          ,[DeleteFlg]
          ,[UsedFlg]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
    FROM M_Kouza

    WHERE [KouzaCD] = @KouzaCD
    AND ChangeDate <= CONVERT(DATE, @ChangeDate)
    ORDER BY ChangeDate desc
    ;
END


