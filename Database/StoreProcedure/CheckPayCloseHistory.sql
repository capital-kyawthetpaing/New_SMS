 BEGIN TRY 
 Drop Procedure dbo.[CheckPayCloseHistory]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE CheckPayCloseHistory
    (@PayeeCD varchar(11),
     @PayCloseDate varchar(10)
    )AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    
    SELECT DP.PayCloseNO
    FROM D_PayCloseHistory AS DP
    WHERE DP.PayeeCD = @PayeeCD
    AND DP.ProcessingKBN <> 2	--（2:締キャンセル）
	AND DP.PayCloseDate >= @PayCloseDate
    ;

END

