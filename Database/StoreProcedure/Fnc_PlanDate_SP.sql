 BEGIN TRY 
 Drop Procedure dbo.[Fnc_PlanDate_SP]
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
CREATE PROCEDURE [dbo].[Fnc_PlanDate_SP]
(   
    -- Add the parameters for the function here
    @KaisyuShiharaiKbn tinyint,		--0:回収,1:支払
    @CustomerCD  varchar(13),		--顧客、仕入先
    @ChangeDate  varchar(10),		--計上日
    @TyohaKbn tinyint,				--帳端区分
    
    @Yoteibi  varchar(10) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt int;
    
    DECLARE @HolidayKBN tinyint;    --●HolidayKBN		0:前営業日 1:当日 2:次営業日
    DECLARE @BillingCloseDate tinyint;--●請求締日			31:月末日
    DECLARE @CollectPlanMonth tinyint;--●回収予定月	0:当月、1:翌月
    DECLARE @CollectPlanDate tinyint;--●回収予定日		31:月末日
    DECLARE @BankDayOff tinyint;	--M_Calendar 銀行休日区分
    
    SET @Yoteibi =null;

--BEGIN TRY

    --【チェック】
    --①in計上日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、エラー★とする
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
                );
 
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
	--in回収予定区分 ＝ 0:回収 の場合
	IF @KaisyuShiharaiKbn = 0
	BEGIN        
        --②in取引先CDが顧客マスター(M_Customer)に存在しなければエラー★とする
        SET @iCnt = (SELECT COUNT(*) FROM M_Customer M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.CustomerCD = @CustomerCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            RETURN;
        END
        
        SELECT top 1 @HolidayKBN=HolidayKBN, @BillingCloseDate=BillingCloseDate
        ,@CollectPlanMonth=CollectPlanMonth, @CollectPlanDate=CollectPlanDate
        FROM M_Customer M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.CustomerCD = @CustomerCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
        
    END
    
    --in回収予定区分 ＝ 1:支払 の場合
	ELSE
	BEGIN
        --②in取引先CDが仕入先マスター(M_Vendor)に存在しなければエラー★とする
        SET @iCnt = (SELECT COUNT(*) FROM M_Vendor M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.VendorCD = @CustomerCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            RETURN;
        END
        
        SELECT top 1 @HolidayKBN=HolidayKBN, @BillingCloseDate=PaymentCloseDay
        ,@CollectPlanMonth=PaymentPlanKBN, @CollectPlanDate=PaymentPlanDay
        FROM M_Vendor M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.VendorCD = @CustomerCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
	
	END

    --★請求締日を求める
    DECLARE @W_DAY date;
    IF @BillingCloseDate = 31
    BEGIN
        --計上日＝締日（31）の場合
        IF CONVERT(date, @ChangeDate) = DATEADD(DAY,-1,DATEADD(MONTH,1,DATEADD(DAY,1-DATEPART(DAY,CONVERT(date, @ChangeDate)),CONVERT(date, @ChangeDate))))
        BEGIN
            SET @W_DAY = CONVERT(date, @ChangeDate);
        END
        ELSE
        BEGIN
            SET @W_DAY = DATEADD(DAY,-1,DATEADD(MONTH,1,DATEADD(DAY,1-DATEPART(DAY,CONVERT(date, @ChangeDate)),CONVERT(date, @ChangeDate))));
        END
    END
    ELSE IF DAY(CONVERT(date, @ChangeDate)) <= @BillingCloseDate
    BEGIN
        SET @W_DAY = DATEADD(DAY, 1-DATEPART(DAY, CONVERT(date, @ChangeDate))-1+@BillingCloseDate, CONVERT(date, @ChangeDate)); --先月末に締日を足す
    END
    ELSE
    BEGIN
        SET @W_DAY = DATEADD(DAY,-1+@BillingCloseDate,DATEADD(MONTH,1,DATEADD(DAY,1-DATEPART(DAY,CONVERT(date, @ChangeDate)),CONVERT(date, @ChangeDate)))); --当月末に締日を足す
    END
    
    --【予定日を求める】
    --in計上日の年月に　●回収予定月 nヶ月　を加算し、回収予定日 n日　を結合する。
    --ただし、帳端区分がゼロ（今回）で無い場合は、帳端区分の数値分、回収予定月を加算。
    --また、31日が存在しない月などは考慮が必要。
    SET @W_DAY = DATEADD(month, @CollectPlanMonth+@TyohaKbn, @W_DAY);
    
    IF @CollectPlanDate = 31
    BEGIN
        SET @Yoteibi = CONVERT(varchar, 
                            DATEADD(DAY,-1,DATEADD(MONTH,1,DATEADD(DAY,1-DATEPART(DAY,@W_DAY),@W_DAY))) --当月末日
                            ,111)
    END
    ELSE
    BEGIN
        SET @Yoteibi = CONVERT(varchar, 
                            DATEADD(dd, @CollectPlanDate, 
                            DATEADD(DAY,-1,DATEADD(MONTH,1,DATEADD(DAY,1-DATEPART(DAY,DATEADD(MONTH, -1, @W_DAY)),DATEADD(MONTH, -1, @W_DAY))))) --前月末日に回収予定日ｎ日を加算
                            ,111)
    END
    
    --【休業日のチェックを行う】
    --1-2で算出した★回収予定日の休日チェックを行う
    --休日区分が 0:営業日 の場合
    SELECT @BankDayOff = M.BankDayOff
    FROM M_Calendar AS M
    WHERE M.CalendarDate = CONVERT(date,@Yoteibi)
    ;
    
    IF @BankDayOff = 0
    BEGIN
    	RETURN;
    END
    
    --休日区分が 1:休業日 の場合
    --●HolidayKBN 0:前営業日 1:当日 2:次営業日 に基づいて回収予定日を調整する
    --※回収予定日±1日でカレンダーチェックを行い、●HolidayKBNが 0:営業日　であればその日。
    --　 1:休業日であればさらに±1日してチェックする。
    --out予定日＝★回収予定日に対して調整後の回収予定日
    --とし、処理終了
    IF @HolidayKBN = 0	--0:前営業日
    BEGIN
        SELECT top 1 @Yoteibi = CONVERT(varchar,M.CalendarDate,111)
        --, ROW_NUMBER() OVER (ORDER BY M.CalendarDate desc) AS RowNumber
        FROM M_Calendar AS M
        WHERE M.CalendarDate <= CONVERT(date,@Yoteibi)
        AND M.BankDayOff = 0
        ORDER BY M.CalendarDate desc
        ;
    END
	ELSE
	BEGIN
        SELECT top 1 @Yoteibi = CONVERT(varchar,M.CalendarDate,111)
        --, ROW_NUMBER() OVER (ORDER BY M.CalendarDate) AS RowNumber
        FROM M_Calendar AS M
        WHERE M.CalendarDate >= CONVERT(date,@Yoteibi)
        AND M.BankDayOff = 0
        ORDER BY M.CalendarDate 
        ;
    END
	
--END TRY

--BEGIN CATCH
--    RETURN;
--END CATCH

END

