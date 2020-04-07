 BEGIN TRY 
 Drop Procedure dbo.[Fnc_TAXCalculation]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Fnc_TAXCalculation]
(   
    -- Add the parameters for the function here
    @Mode      tinyint,	--1:消費税計算,2:消費税分解計算
    @ChangeDate  varchar(10),
    @TaxRateFLG tinyint,	--0:非課税、1:通常課税、2:軽減課税
    @Kingaku   money
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @Kingaku1  money;
    DECLARE @Kingaku2  money;
    DECLARE @Zeiritsu  decimal(3, 1);  
    
    IF ISNULL(@ChangeDate,'') = ''
    	SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
	EXEC Fnc_TAXCalculation_SP
            @Mode      ,    --1:消費税計算,2:消費税分解計算
            @ChangeDate  ,
            @TaxRateFLG ,   --0:非課税、1:通常課税、2:軽減課税
            @Kingaku   ,
            @Kingaku1 OUTPUT ,
            @Kingaku2 OUTPUT ,
            @Zeiritsu OUTPUT  
        ;
	
    -- Insert statements for procedure here
    SELECT  @Kingaku1 AS Kingaku1  ,
	    @Kingaku2 AS Kingaku2 ,
	    @Zeiritsu AS Zeiritsu
	    ;   
END


