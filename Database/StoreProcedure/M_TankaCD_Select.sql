 BEGIN TRY 
 Drop Procedure dbo.[M_TankaCD_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_TankaCD_Select]    */
CREATE PROCEDURE [dbo].[M_TankaCD_Select](
    -- Add the parameters for the stored procedure here
    @TankaCD  varchar(13),
    @ChangeDate varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT top 1 [TankaCD]
          ,CONVERT(varchar, ChangeDate,111) AS ChangeDate
          ,[TankaName]
          ,[GeneralRate]
          ,[MemberRate]
          ,[ClientRate]
          ,[SaleRate]
          ,[WebRate]
          ,[Remarks]
          ,[RoundKBN]
          ,[DeleteFlg]
          ,[UsedFlg]
        ,[InsertOperator]
        ,CONVERT(varchar,[InsertDateTime]) AS InsertDateTime
        ,[UpdateOperator]
        ,CONVERT(varchar,[UpdateDateTime]) AS UpdateDateTime
    FROM M_TankaCD

    WHERE [TankaCD] = @TankaCD
    AND ChangeDate <= CONVERT(DATE, @ChangeDate)
    ORDER BY ChangeDate desc
    ;
END


