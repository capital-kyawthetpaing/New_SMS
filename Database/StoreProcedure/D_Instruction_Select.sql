 BEGIN TRY 
 Drop Procedure dbo.[D_Instruction_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [D_Instruction_Select]    */
CREATE PROCEDURE [dbo].[D_Instruction_Select](
    -- Add the parameters for the stored procedure here
    @InstructionNO  varchar(11),
    @SoukoCD varchar(6)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  DI.InstructionKBN
             
        FROM D_Instruction AS DI

        WHERE DI.InstructionNO = @InstructionNO 
        AND DI.DeliverySoukoCD = @SoukoCD
        AND DI.DeleteDateTime IS NULL
        ;

END


