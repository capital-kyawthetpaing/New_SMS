
/****** Object:  UserDefinedFunction [dbo].[Fnc_Credit]    Script Date: 6/11/2019 2:30:20 PM ******/
DROP PROCEDURE [dbo].[Fnc_Credit]
GO
/****** Object:  UserDefinedFunction [dbo].[Fnc_Credit]    Script Date: 6/11/2019 2:30:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Fnc_Credit
(   
    -- Add the parameters for the function here
    @Operator  varchar(10),
    @PC  varchar(30),
    @ChangeDate  varchar(10),
    @CustomerCD  varchar(13)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
	DECLARE @CreditCheckKBN tinyint;
    DECLARE @CreditMessage varchar(500);
    DECLARE @SaikenGaku  decimal(10, 0);
    DECLARE @CreditAmount  decimal(10, 0);

    IF ISNULL(@ChangeDate,'') = ''
    	SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
    EXEC Fnc_Credit_SP
        @Operator,
        @PC,
        @ChangeDate ,
        @CustomerCD ,
        @CreditCheckKBN OUTPUT,
        @CreditMessage  OUTPUT,
        @SaikenGaku     OUTPUT,
        @CreditAmount   OUTPUT
        ;

    SELECT  @CreditCheckKBN AS CreditCheckKBN,
            @CreditMessage  AS CreditMessage,
            @SaikenGaku     AS SaikenGaku,
            @CreditAmount   AS CreditAmount
    ;   
END

GO

DROP PROCEDURE [dbo].[Fnc_Credit_SP]
GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE Fnc_Credit_SP
(   
    -- Add the parameters for the function here
    @Operator  varchar(10),
    @PC  varchar(30),
    @ChangeDate  varchar(10),
    @CustomerCD  varchar(13),
    
    @CreditCheckKBN tinyint OUTPUT,
    @CreditMessage  varchar(500) OUTPUT,
    @SaikenGaku     decimal(10, 0) OUTPUT,
    @CreditAmount   decimal(10, 0) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt        int;
    --DECLARE @CreditAmount money;
    DECLARE @CreditAdditionAmount money;
    --DECLARE @CreditCheckKBN tinyint;
    --DECLARE @CreditMessage varchar(100);
    DECLARE @JuchuuGaku  money;
    
    DECLARE @ChangeDateYM     int;
    DECLARE @FiscalYYYYMM     int;
    DECLARE @ClosePosition1   tinyint;
    DECLARE @ClosePosition3   tinyint;
    DECLARE @BalanceGaku      money;
    
    SET @CreditCheckKBN = 0;
    SET @CreditMessage =null;
    SET @SaikenGaku = 0;
    SET @CreditAmount = 0;
    SET @BalanceGaku = 0;
    SET @JuchuuGaku = 0;

BEGIN TRY
    --【チェック】
    --①in基準日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
                );
 
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
    --②in顧客CDが以下の条件で得意先マスター(M_Customer)に存在しなければ、  
    SET @iCnt = (SELECT COUNT(*) FROM F_Customer(convert(date,@ChangeDate)) M
                WHERE M.CustomerCD = @CustomerCD
                AND M.DeleteFLG = 0
                );
                
    IF @iCnt = 0
    BEGIN
        RETURN;
    END

    SELECT top 1 @CreditAmount=CreditAmount
         , @CreditAdditionAmount=CreditAdditionAmount
         , @CreditCheckKBN = CreditCheckKBN
         , @CreditMessage = CreditMessage
    FROM F_Customer(convert(date,@ChangeDate)) M
    WHERE M.CustomerCD = @CustomerCD
    AND M.DeleteFLG = 0
    ORDER BY M.ChangeDate desc
    ;

	--●CreditCheckKBN＝0:なしの場合
    IF @CreditCheckKBN = 0
    BEGIN
        RETURN;
    END
    
    --【債権残を求める】
    SET @iCnt = (Select COUNT(*)
                 From M_StoreClose
                 Where StoreCD = '0000')

    IF @iCnt <> 0
    BEGIN
        Select top 1 @FiscalYYYYMM = FiscalYYYYMM
              ,@ClosePosition1 = ClosePosition1
              ,@ClosePosition3 = ClosePosition3
        From M_StoreClose
        Where StoreCD = '0000'
        Order By FiscalYYYYMM desc
        ;
    END
    
    SET @ChangeDateYM = convert(int,SUBSTRING(convert(varchar,convert(date,@ChangeDate),112),1,6));
    
    --FiscalYYYYMM＜in基準日の年月またはFiscalYYYYMM＝in基準日の年月かつClosePosition1＝0かつClosePosition3＝0
    IF @iCnt = 0 OR (@iCnt > 0 AND (@FiscalYYYYMM < @ChangeDateYM OR (@FiscalYYYYMM = @ChangeDateYM AND @ClosePosition1 = 0 AND @ClosePosition3 = 0)))
    BEGIN

        --GetsujiSaikenKeisanSyori.exe を起動する
        EXEC PRC_GetsujiSaikenKeisanSyori
                @ChangeDateYM--@FiscalYYYYMM",
                ,'0000'--@StoreCD",  
                ,1--@Mode", 
                ,@Operator--@Operator", 
                ,@PC--@PC", 
                ;
	END

    SET @iCnt = (Select BalanceGaku
                 From  D_MonthlyClaims
                 Where DeleteDateTime IS NULL
                 And   YYYYMM = @ChangeDateYM--in基準日の年月          
                 And   StoreCD = '0000'
                 And   CustomerCD = @CustomerCD
                );

    IF @iCnt > 0
    BEGIN
        Select @BalanceGaku = BalanceGaku
        From  D_MonthlyClaims
        Where DeleteDateTime IS NULL
        And   YYYYMM = @ChangeDateYM--in基準日の年月          
        And   StoreCD = '0000'
        And   CustomerCD = @CustomerCD
        ;
    END
	
    --【受注残を求める】
    SELECT @JuchuuGaku= SUM(B.JuchuuGaku)
    From  D_JuchuuDetails AS A     
    Inner Join D_Juchuu AS B     
    On B.JuchuuNO = A.JuchuuNO      
    Where A.DeleteDateTime IS NULL
    And   B.DeleteDateTime IS NULL
    And   A.UpdateCancelKBN <> 9 --9:キャンセル
    And   B.CustomerCD = @CustomerCD
    And   ((A.ArrivePlanDate IS NULL)
            Or (A.ArrivePlanDate IS NOT NULL
            	--入荷予定日がセットされている場合、３ヶ月先以降は除く
                And A.ArrivePlanDate >= DATEADD(M, 3, CONVERT(DATE, @ChangeDate))	
                )
            )              
    And NOT EXISTS (
                  Select *
                  From  D_SalesDetails              
                  Where DeleteDateTime IS NULL
                  And   JuchuuNO  = A.JuchuuNO  
                  And   JuchuuRows = A.JuchuuRows                  
         )
    ;

    
    SET @SaikenGaku = @BalanceGaku + @JuchuuGaku;
    
END TRY

BEGIN CATCH
    RETURN;
END CATCH

END
GO


