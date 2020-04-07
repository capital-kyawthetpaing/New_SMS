 BEGIN TRY 
 Drop Procedure dbo.[D_Exclusive_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Exclusive_Select]    */
CREATE PROCEDURE [dbo].[D_Exclusive_Select](
    -- Add the parameters for the stored procedure here
    @DataKBN tinyint,
    @Number varchar(20)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT M.DataKBN
        ,M.[Number]
        ,M.OperateDataTime
        ,M.Operator
        ,M.Program
        ,M.PC

    from D_Exclusive M
    
    WHERE M.DataKBN = @DataKBN
    AND M.[Number] = @Number
    ;
END


