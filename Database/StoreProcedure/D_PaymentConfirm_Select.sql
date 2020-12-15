 BEGIN TRY 
 Drop Procedure dbo.[D_PaymentConfirm_Select]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_PaymentConfirm_Select]    */
CREATE PROCEDURE D_PaymentConfirm_Select(
    -- Add the parameters for the stored procedure here
    @CollectNO  varchar(11),
    @ConfirmNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DH.CollectNO
    FROM [D_PaymentConfirm] AS DH

    WHERE DH.ConfirmDateTime > (SELECT D.ConfirmDateTime
                                FROM D_PaymentConfirm AS D
                                WHERE D.CollectNO = @CollectNO
                                AND D.ConfirmNO = @ConfirmNO
                                )
    AND DH.CollectNO = @CollectNO
    AND DH.DeleteDateTime IS NULL
    ;
END

GO
