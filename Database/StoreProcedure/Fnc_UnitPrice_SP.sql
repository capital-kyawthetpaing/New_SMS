 BEGIN TRY 
 Drop Procedure dbo.[Fnc_UnitPrice_SP]
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
CREATE PROCEDURE Fnc_UnitPrice_SP
(   
    -- Add the parameters for the function here
    @AdminNo     int,	
    @ChangeDate  varchar(10),
    @CustomerCD  varchar(13),
    @StoreCD  varchar(4),
    @SaleKbn tinyint,
    @Suryo   int,
    
    @ZeikomiTanka  decimal(10, 0) OUTPUT,
    @ZeinukiTanka  decimal(10, 0) OUTPUT,
    @Zeiritsu  decimal(3, 1) OUTPUT  ,
    @Zei       money OUTPUT,
    @GenkaTanka  decimal(10, 0) OUTPUT,
    @Bikou varchar(500) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt int;
    DECLARE @TaxRateFLG tinyint;
    DECLARE @TankaCD varchar(13);
    DECLARE @CustomerKBN tinyint;
    DECLARE @WebTempo tinyint;
    DECLARE @StoreKBN tinyint;
    DECLARE @WebStoreCD varchar(4);
    
    DECLARE @TaxRate1 decimal(3,1);
    DECLARE @TaxRate2 decimal(3,1);
    DECLARE @FractionKBN tinyint;
    DECLARE @SaleExcludedFlg tinyint;
    
    DECLARE @GeneralSaleRate     decimal(3,1);
    DECLARE @GeneralSaleFraction tinyint;
    DECLARE @MemberSaleRate      decimal(3,1);
    DECLARE @MemberSaleFraction  tinyint;
    DECLARE @ClientSaleRate      decimal(3,1);
    DECLARE @ClientSaleFraction  tinyint;
    
    SET @ZeikomiTanka = 0;
    SET @ZeinukiTanka = 0;
    SET @Zeiritsu = 0;
    SET @Zei = 0;
    SET @GenkaTanka = 0;
    SET @Bikou =null;

--BEGIN TRY
    --【チェック】
    --①in基準日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
              --  AND M.CalendarCD = 0	--2019.12.16 add★
                );
 
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
    --②inAdminNOがSKUマスター(M_SKU)に存在しなければ
    SET @iCnt = (SELECT COUNT(*) FROM M_SKU M
                WHERE M.ChangeDate <= convert(date,@ChangeDate)
                AND M.AdminNo = @AdminNo
                AND M.DeleteFLG = 0
                );
                
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
	SELECT top 1 @TaxRateFLG=TaxRateFLG
		,@SaleExcludedFlg=SaleExcludedFlg
	FROM M_SKU M
    WHERE M.ChangeDate <= convert(date,@ChangeDate)
    AND M.AdminNo = @AdminNo
    AND M.DeleteFLG = 0
    ORDER BY M.ChangeDate desc
    ;
    
    --③in顧客CD≠Nullの場合、以下の条件で得意先マスター(M_Customer)に存在しなければ、  
    IF ISNULL(@CustomerCD,'') <> ''
    BEGIN
        SET @iCnt = (SELECT COUNT(*) FROM M_Customer M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.CustomerCD = @CustomerCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            RETURN;
        END
        
        SELECT top 1 @TankaCD=TankaCD, @CustomerKBN=CustomerKBN FROM M_Customer M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.CustomerCD = @CustomerCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
    END
    ELSE
    BEGIN
        SET @TankaCD = null;
        SET @CustomerKBN=0;
    END
    
    --④in店舗CD≠Nullの場合、以下の条件で店舗マスター(M_Store)に存在しなければ、   
    IF ISNULL(@StoreCD,'') <> ''
    BEGIN
        SET @iCnt = (SELECT COUNT(*) FROM M_Store M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.StoreCD = @StoreCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            RETURN;
        END
        
        SELECT top 1 @StoreKBN=StoreKBN FROM M_Store M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.StoreCD = @StoreCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
        
        IF @StoreKBN = 1    --1　:実店
        BEGIN
            SET @WebStoreCD = null;
            SET @WebTempo = 0;
        END
        ELSE IF @StoreKBN = 2   --2　:WEB店
        BEGIN
            SET @WebTempo = 1;
            
            SELECT top 1 @WebStoreCD=StoreCD FROM M_Store M
            WHERE M.ChangeDate <= convert(date,@ChangeDate)
            AND M.DeleteFLG = 0
            AND M.StoreKBN = 3  --3　:WEBまとめ店舗
            ORDER BY M.ChangeDate desc
            ;
        END
    END
    ELSE
    BEGIN
        SET @WebStoreCD = null;
        SET @WebTempo=0;
    END
    
/*    --⑤inSale区分≠Nullの場合、0と1以外であれば、	
	IF ISNULL(@SaleKbn,0) NOT IN (0,1)
		RETURN;
*/		
	--Sale期間の判断
    SET @iCnt = (SELECT COUNT(*) FROM M_Sale
                Where StoreCD = @StoreCD
                AND StartDate <= convert(date,@ChangeDate)
                AND EndDate >= convert(date,@ChangeDate)
                );
                
    IF @iCnt = 0
    BEGIN
        SET @SaleKbn = 0;
    END
    ELSE
    BEGIN
        Select top 1 @SaleKbn=ISNULL(SaleFlg,0)     --SaleFlg＝１ならマスター単価、2なら割引率
            ,@GeneralSaleRate      =GeneralSaleRate
            ,@GeneralSaleFraction  =GeneralSaleFraction
            ,@MemberSaleRate       =MemberSaleRate
            ,@MemberSaleFraction   =MemberSaleFraction
            ,@ClientSaleRate       =ClientSaleRate
            ,@ClientSaleFraction   =ClientSaleFraction
        From M_Sale
        Where StoreCD = @StoreCD
        AND StartDate <= convert(date,@ChangeDate)
        AND EndDate >= convert(date,@ChangeDate)
        ;
        
        --Sale期間でも対象外商品は通常価格
        IF @SaleExcludedFlg = 1
        BEGIN
        	SET @SaleKbn = 0;
        END
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

	DECLARE @DataFlg tinyint;
	SET @DataFlg = 0;
	
    --カーソル定義
    DECLARE CUR1 CURSOR FOR
    --【単価を求める】①
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = REPLICATE('0',4)
    AND MI.TankaCD = REPLICATE('0',13)
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;

    --カーソル定義
    DECLARE CUR2 CURSOR FOR
    --【単価を求める】②
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = @WebStoreCD
    AND MI.TankaCD = REPLICATE('0',13)
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;
    
    --カーソル定義
    DECLARE CUR3 CURSOR FOR
    --【単価を求める】③
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = @StoreCD
    AND MI.TankaCD = REPLICATE('0',13)
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;
    
    --カーソル定義
    DECLARE CUR4 CURSOR FOR
    --【単価を求める】④
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = REPLICATE('0',4)
    AND MI.TankaCD = @TankaCD
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;

    --カーソル定義
    DECLARE CUR5 CURSOR FOR
    --【単価を求める】⑤
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = @WebStoreCD
    AND MI.TankaCD = @TankaCD
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;
    
    --カーソル定義
    DECLARE CUR6 CURSOR FOR
    --【単価を求める】⑥
    SELECT top 1 MI.PriceWithTax
          ,MI.PriceWithoutTax
          ,MI.GeneralRate
          ,MI.GeneralPriceWithTax
          ,MI.GeneralPriceOutTax
          ,MI.MemberRate
          ,MI.MemberPriceWithTax
          ,MI.MemberPriceOutTax
          ,MI.ClientRate
          ,MI.ClientPriceWithTax
          ,MI.ClientPriceOutTax
          ,MI.SaleRate
          ,MI.SalePriceWithTax
          ,MI.SalePriceOutTax
          ,MI.WebRate
          ,MI.WebPriceWithTax
          ,MI.WebPriceOutTax
          ,MI.Remarks

    from M_SKUPrice MI
    
    WHERE MI.StoreCD = @StoreCD
    AND MI.TankaCD = @TankaCD
    AND MI.AdminNo = @AdminNo
    AND MI.ChangeDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidStartDate <= CONVERT(DATE, @ChangeDate)
--    AND MI.ValidEndDate >= CONVERT(DATE, @ChangeDate)
    AND MI.DeleteFlg = 0
    ORDER BY ChangeDate desc
    ;
    
    --【単価の決定】
    DECLARE @PriceWithTax decimal(10, 0);
    DECLARE @PriceWithoutTax decimal (10, 0);
    DECLARE @GeneralPriceWithTax decimal (10, 0);
    DECLARE @GeneralPriceOutTax decimal (10, 0);
    DECLARE @MemberPriceWithTax decimal (10, 0);
    DECLARE @MemberPriceOutTax decimal (10, 0);
    DECLARE @ClientPriceWithTax decimal (10, 0);
    DECLARE @ClientPriceOutTax decimal (10, 0);
    DECLARE @SalePriceWithTax decimal (10, 0);
    DECLARE @SalePriceOutTax decimal (10, 0);
    DECLARE @WebPriceWithTax decimal (10, 0);
    DECLARE @WebPriceOutTax decimal (10, 0);
    DECLARE @Remarks Varchar (500);
    DECLARE @GeneralRate decimal(5,2);
    DECLARE @MemberRate  decimal(5,2);
    DECLARE @ClientRate  decimal(5,2); 
    DECLARE @SaleRate    decimal(5,2); 
    DECLARE @WebRate     decimal(5,2); 
    
    --カーソルオープン
    OPEN CUR6;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR6
    INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

    --データの行数分ループ処理を実行する
    IF @@FETCH_STATUS = 0
    BEGIN
		SET @DataFlg = 1;

    END

    --カーソルを閉じる
    CLOSE CUR6;
    DEALLOCATE CUR6;
    
    IF @DataFlg = 0
    BEGIN
        --カーソルオープン
        OPEN CUR5;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR5
        INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

        --データの行数分ループ処理を実行する
        IF @@FETCH_STATUS = 0
        BEGIN
            SET @DataFlg = 1;

        END

        --カーソルを閉じる
        CLOSE CUR5;
        DEALLOCATE CUR5;
    END
    
    IF @DataFlg = 0
    BEGIN
        --カーソルオープン
        OPEN CUR4;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR4
        INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

        --データの行数分ループ処理を実行する
        IF @@FETCH_STATUS = 0
        BEGIN
            SET @DataFlg = 1;

        END

        --カーソルを閉じる
        CLOSE CUR4;
        DEALLOCATE CUR4;
    END
    
    IF @DataFlg = 0
    BEGIN
        --カーソルオープン
        OPEN CUR3;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR3
        INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

        --データの行数分ループ処理を実行する
        IF @@FETCH_STATUS = 0
        BEGIN
            SET @DataFlg = 1;

        END

        --カーソルを閉じる
        CLOSE CUR3;
        DEALLOCATE CUR3;
    END
    
    
    IF @DataFlg = 0
    BEGIN
        --カーソルオープン
        OPEN CUR2;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR2
        INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

        --データの行数分ループ処理を実行する
        IF @@FETCH_STATUS = 0
        BEGIN
            SET @DataFlg = 1;

        END

        --カーソルを閉じる
        CLOSE CUR2;
        DEALLOCATE CUR2;
    END
    
    IF @DataFlg = 0
    BEGIN
        --カーソルオープン
        OPEN CUR1;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR1
        INTO @PriceWithTax
          ,@PriceWithoutTax
          ,@GeneralRate
          ,@GeneralPriceWithTax
          ,@GeneralPriceOutTax
          ,@MemberRate
          ,@MemberPriceWithTax
          ,@MemberPriceOutTax
          ,@ClientRate
          ,@ClientPriceWithTax
          ,@ClientPriceOutTax
          ,@SaleRate
          ,@SalePriceWithTax
          ,@SalePriceOutTax
          ,@WebRate
          ,@WebPriceWithTax
          ,@WebPriceOutTax
          ,@Remarks;

        --データの行数分ループ処理を実行する
        IF @@FETCH_STATUS = 0
        BEGIN
            SET @DataFlg = 1;

        END

        --カーソルを閉じる
        CLOSE CUR1;
        DEALLOCATE CUR1;
    END
    
    --【Return values】
    IF @WebTempo = 1	--●Web店舗＝1の場合
    BEGIN
    
	    SET @ZeikomiTanka = @WebPriceWithTax;
	    SET @ZeinukiTanka = @WebPriceOutTax;

    
    END
    ELSE    --●Web店舗＝0の場合
    BEGIN
        IF @SaleKbn = 0     --←Saleしていない
        BEGIN
            IF @CustomerKBN = 3 --●CustomerKBN＝3の場合
            BEGIN
                SET @ZeikomiTanka = @ClientPriceWithTax;    --税込外商単価
                SET @ZeinukiTanka = @ClientPriceOutTax;     --税抜外商単価
            END
            ELSE IF @CustomerKBN = 2 --●CustomerKBN＝2の場合
            BEGIN
                SET @ZeikomiTanka = @GeneralPriceWithTax;   --税込一般単価
                SET @ZeinukiTanka = @GeneralPriceOutTax;    --税抜一般単価
            END
            ELSE IF @CustomerKBN = 1 --●CustomerKBN＝1の場合
            BEGIN
                SET @ZeikomiTanka = @MemberPriceWithTax;    --税込会員単価
                SET @ZeinukiTanka = @MemberPriceOutTax;     --税抜会員単価
            END
        END
        ELSE IF @SaleKbn = 1	--←マスター額でのSale
        BEGIN
            SET @ZeikomiTanka = @SalePriceWithTax;
            SET @ZeinukiTanka = @SalePriceOutTax;
        END
        ELSE IF @SaleKbn = 2	--←マスター割引率でのSale
        BEGIN        	
            IF @CustomerKBN = 3 --●CustomerKBN＝3の場合
            BEGIN
                SET @PriceWithTax = @ClientPriceWithTax;    --税込外商単価
                SET @PriceWithoutTax = @ClientPriceOutTax;      --税抜外商単価
                SET @SaleRate = @ClientSaleRate;
                SET @FractionKBN = @ClientSaleFraction;

            END
            ELSE IF @CustomerKBN = 2 --●CustomerKBN＝2の場合
            BEGIN
                SET @PriceWithTax = @GeneralPriceWithTax;   --税込一般単価
                SET @PriceWithoutTax = @GeneralPriceOutTax;    --税抜一般単価
                SET @SaleRate = @GeneralSaleRate;
                SET @FractionKBN = @GeneralSaleFraction;
            END
            ELSE IF @CustomerKBN = 1 --●CustomerKBN＝1の場合
            BEGIN
                SET @PriceWithTax = @MemberPriceWithTax;    --税込会員単価
                SET @PriceWithoutTax = @MemberPriceOutTax;     --税抜会員単価
                SET @SaleRate = @MemberSaleRate;
                SET @FractionKBN = @MemberSaleFraction;
            END
            
            IF @FractionKBN = 1 --＝1なら、計算した税額の小数点以下を切り捨て
            BEGIN
                SET @ZeikomiTanka = FLOOR(@PriceWithTax * (100-@SaleRate)/100);
                SET @ZeinukiTanka = FLOOR(@PriceWithoutTax * (100-@SaleRate)/100);
            END
            ELSE IF @FractionKBN = 2    --＝2なら、計算した税額の小数点以下を四捨五入
            BEGIN
                SET @ZeikomiTanka = ROUND(@PriceWithTax * (100-@SaleRate)/100, 0);
                SET @ZeinukiTanka = ROUND(@PriceWithoutTax * (100-@SaleRate)/100, 0);
            END
            ELSE IF @FractionKBN = 3    --＝3なら、計算した税額の小数点以下を切り上げ
            BEGIN
                SET @ZeikomiTanka = CEILING(@PriceWithTax * (100-@SaleRate)/100);
                SET @ZeinukiTanka = CEILING(@PriceWithoutTax * (100-@SaleRate)/100);
	        END
	    END
    END
    
    SET @Zei = @ZeikomiTanka - @ZeinukiTanka;
    SET @Bikou = @Remarks;

	SELECT @GenkaTanka=LastCost
	FROM M_SKULastCost
	WHERE AdminNo = @AdminNo
	;
	
--END TRY

--BEGIN CATCH
---    RETURN;
--END CATCH

END
