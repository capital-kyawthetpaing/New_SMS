 BEGIN TRY 
 Drop Procedure dbo.[Fnc_TAXCalculation_SP]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Fnc_TAXCalculation_SP]
(   
    -- Add the parameters for the function here
    @Mode      tinyint,	--1:消費税計算,2:消費税分解計算
    @ChangeDate  varchar(10),
    @TaxRateFLG tinyint,	--0:非課税、1:通常課税、2:軽減課税
    @Kingaku   money,
    @Kingaku1  money OUTPUT,
    @Kingaku2  money OUTPUT,
    @Zeiritsu  decimal(3, 1) OUTPUT  
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt int;
    DECLARE @TaxRate1 decimal(3,1);
    DECLARE @TaxRate2 decimal(3,1);
    DECLARE @FractionKBN tinyint;
    
    SET @Kingaku1 = 0;
    SET @Kingaku2 = 0;
    SET @Zeiritsu = 0;

--BEGIN TRY
    --【チェック】
    --①in計算モードが、1と2以外であれば、エラーとする
    IF @Mode NOT IN (1,2)
    	RETURN;
    
    --②in基準日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
                );
 
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
    
    --【税率を求める】
    Select top 1 @TaxRate1=TaxRate1,@TaxRate2=TaxRate2,@FractionKBN=FractionKBN
    From M_SalesTax
    Where ChangeDate <= convert(date,@ChangeDate)
    ORDER BY ChangeDate desc
    ;
	
    --in軽減税率FLG ＝ 1の場合
    IF @TaxRateFLG = 1
    BEGIN
    	SET @Zeiritsu = @TaxRate1;
    END
    ELSE IF @TaxRateFLG = 2
    BEGIN
    	SET @Zeiritsu = @TaxRate2;
    END
    ELSE
    	SET @Zeiritsu = 0;
    	

	DECLARE @Zei money;
	
	--in計算モード＝１の場合
    IF @Mode = 1
    BEGIN
        --税額＝in金額×(TaxRate1 × 0.01)←TaxRate18 or 10なので、計算時に0.08や0.10にする 
        IF @FractionKBN = 1 --＝1なら、計算した税額の小数点以下を切り捨て
            SET @Zei = FLOOR(@Kingaku*@Zeiritsu*0.01);
        ELSE IF @FractionKBN = 2    --＝2なら、計算した税額の小数点以下を四捨五入
            SET @Zei = ROUND(@Kingaku*@Zeiritsu*0.01, 0);
        ELSE IF @FractionKBN = 3    --＝3なら、計算した税額の小数点以下を切り上げ
            SET @Zei = CEILING(@Kingaku*@Zeiritsu*0.01);
            
            
        SET @Kingaku1 = @Zei;
        SET @Kingaku2 = 0;
    END

	--in計算モード＝2の場合
    ELSE IF @Mode = 2
    BEGIN
        --税額＝in金額×TaxRate1 ÷ (TaxRate1 ＋ 100) 
        IF @FractionKBN = 1 --＝1なら、計算した税額の小数点以下を切り捨て
            SET @Zei = FLOOR(@Kingaku*@Zeiritsu/(@Zeiritsu+100));
        ELSE IF @FractionKBN = 2    --＝2なら、計算した税額の小数点以下を四捨五入
            SET @Zei = ROUND(@Kingaku*@Zeiritsu/(@Zeiritsu+100), 0);
        ELSE IF @FractionKBN = 3    --＝3なら、計算した税額の小数点以下を切り上げ
            SET @Zei = CEILING(@Kingaku*@Zeiritsu/(@Zeiritsu+100));
            
            
        SET @Kingaku1 = @Kingaku - @Zei;
        SET @Kingaku2 = @Zei;
    END

--END TRY

--BEGIN CATCH
--    SET @Kingaku1 = 0;
--    SET @Kingaku2 = 0;
--    SET @Zeiritsu = 0;
--END CATCH

END

