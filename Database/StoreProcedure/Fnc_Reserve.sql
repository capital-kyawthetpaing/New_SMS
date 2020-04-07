 BEGIN TRY 
 Drop Procedure dbo.[Fnc_Reserve]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Fnc_Reserve]
(   
    -- Add the parameters for the function here
    @AdminNO     int,    
    @ChangeDate  varchar(10),
    @StoreCD  varchar(4),
    @SoukoCD  varchar(6),
    @Suryo   int,
    @DenType tinyint,
    @DenNo varchar(11),
    @DenGyoNo int,
    @KariHikiateNo varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    DECLARE @Result tinyint;
    DECLARE @Error  tinyint;
    DECLARE @LastDay varchar(10);
    DECLARE @OutKariHikiateNo varchar(11);
    
    IF ISNULL(@ChangeDate,'') = ''
        SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
    EXEC Fnc_Reserve_SP
        @AdminNO     ,    
        @ChangeDate ,
        @StoreCD,
        @SoukoCD,
        @Suryo,
        @DenType ,
        @DenNo  ,
        @DenGyoNo ,
        @KariHikiateNo   ,
        @Result  OUTPUT,
        @Error  OUTPUT,
        @LastDay  OUTPUT  ,
        @OutKariHikiateNo OUTPUT
        ;
    
    -- Insert statements for procedure here
    SELECT  @Result  AS Result,
        @Error  AS Error,
        @LastDay  AS LastDay ,
        @OutKariHikiateNo AS OutKariHikiateNo
        ;   
END


