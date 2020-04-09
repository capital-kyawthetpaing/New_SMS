 BEGIN TRY 
 Drop Procedure dbo.[Fnc_PlanDate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Fnc_PlanDate]
(   
    -- Add the parameters for the function here
    @KaisyuShiharaiKbn tinyint,		--0:回収,1:支払
    @CustomerCD  varchar(13),		--顧客、仕入先
    @ChangeDate  varchar(10),		--計上日
    @TyohaKbn tinyint				--帳端区分
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @Yoteibi  varchar(10);
    
    IF ISNULL(@ChangeDate,'') = ''
    	SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
    EXEC Fnc_PlanDate_SP
        @KaisyuShiharaiKbn     , 
        @CustomerCD ,   
        @ChangeDate ,
        @TyohaKbn  ,
        @Yoteibi  OUTPUT
        ;
	
    -- Insert statements for procedure here
    SELECT  @Yoteibi  AS Yoteibi
	    ;   
END


