 BEGIN TRY 
 Drop Procedure dbo.[Fnc_UnitPrice]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE Fnc_UnitPrice
(   
    -- Add the parameters for the function here
    @AdminNo     int,	
    @ChangeDate  varchar(10),
    @CustomerCD  varchar(13),
    @StoreCD  varchar(4),
    @SaleKbn tinyint,
    @Suryo   int	--in数量は今後の拡張性のため（今は利用しない）
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @ZeikomiTanka  decimal(10, 0);
    DECLARE @ZeinukiTanka  decimal(10, 0);
    DECLARE @Zeiritsu  decimal(3, 1);
    DECLARE @Zei       money;
    DECLARE @GenkaTanka  decimal(10, 0);
    DECLARE @Bikou varchar;
    
    IF ISNULL(@ChangeDate,'') = ''
    	SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
    EXEC Fnc_UnitPrice_SP
        @AdminNo     ,    
        @ChangeDate ,
        @CustomerCD ,
        @StoreCD  ,
        @SaleKbn ,
        @Suryo   ,
        @ZeikomiTanka  OUTPUT,
        @ZeinukiTanka  OUTPUT,
        @Zeiritsu  OUTPUT  ,
        @Zei       OUTPUT,
        @GenkaTanka OUTPUT,
        @Bikou  OUTPUT
        ;
	
    -- Insert statements for procedure here
    SELECT  @ZeikomiTanka  AS ZeikomiTanka,
	    @ZeinukiTanka  AS ZeinukiTanka,
	    @Zeiritsu  AS Zeiritsu ,
	    @Zei       AS Zei,
	    @GenkaTanka AS GenkaTanka,
	    @Bikou  AS Bikou
	    ;   
END

