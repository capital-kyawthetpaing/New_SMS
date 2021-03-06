BEGIN TRY 
 Drop Procedure [dbo].[CheckInstruction]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--********************************************--
--                                            --
--             出荷指示チェック               --
--                                            --
--********************************************--
CREATE PROCEDURE [dbo].[CheckInstruction]
    (@InstructionNo varchar(11)
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT DH.InstructionNO
          ,DH.DeleteDateTime
      FROM D_Instruction DH
     WHERE DH.InstructionNO = @InstructionNo
     ;

END

