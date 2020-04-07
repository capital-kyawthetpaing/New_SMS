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
CREATE PROCEDURE [dbo].[Fnc_UnitPrice_SP]
(   
    -- Add the parameters for the function here
    @SKUCD      varchar(30),	
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
                );
 
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
    --②inSKUCDがSKUマスター(M_SKU)に存在しなければ
    SET @iCnt = (SELECT COUNT(*) FROM M_SKU M
                WHERE M.ChangeDate <= convert(date,@ChangeDate)
                AND M.SKUCD = @SKUCD
                AND M.DeleteFLG = 0
                );
                
    IF @iCnt = 0
    BEGIN
        RETURN;
    END
    
	SELECT top 1 @TaxRateFLG=TaxRateFLG FROM M_SKU M
    WHERE M.ChangeDate <= convert(date,@ChangeDate)
    AND M.SKUCD = @SKUCD
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
        
        SET @WebTempo = 1;
        
        SELECT top 1 @StoreKBN=StoreKBN FROM M_Store M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.StoreCD = @StoreCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
        
        IF @StoreKBN = 2	--2　:WEB店
        BEGIN
	        SELECT top 1 @WebStoreCD=StoreCD FROM M_Store M
	        WHERE M.ChangeDate <= convert(date,@ChangeDate)
	        AND M.DeleteFLG = 0
	        AND M.StoreKBN = 3	--3　:WEBまとめ店舗
	        ORDER BY M.ChangeDate desc
	        ;
        END
    END
    ELSE
    BEGIN
        SET @WebStoreCD = null;
        SET @WebTempo=0;
    END
    
    --⑤inSale区分≠Nullの場合、0と1以外であれば、	
	IF ISNULL(@SaleKbn,0) NOT IN (0,1)
		RETURN;
		

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
    AND MI.SKUCD = @SKUCD
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
    AND MI.SKUCD = @SKUCD
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
    AND MI.SKUCD = @SKUCD
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
    AND MI.SKUCD = @SKUCD
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
    AND MI.SKUCD = @SKUCD
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
    AND MI.SKUCD = @SKUCD
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
        IF @CustomerKBN = 3 --●CustomerKBN＝3の場合
        BEGIN
    
            SET @ZeikomiTanka = @ClientPriceWithTax;
            SET @ZeinukiTanka = @ClientPriceOutTax;
        END
        
        ELSE --●CustomerKBN<>3の場合
        BEGIN
            IF @SaleKbn = 1
            BEGIN   
                SET @ZeikomiTanka = @SalePriceWithTax;
                SET @ZeinukiTanka = @SalePriceOutTax;
            END
            ELSE IF @SaleKbn = 0
            BEGIN
                IF @CustomerKBN = 1 --●CustomerKBN＝1の場合
                BEGIN
                    SET @ZeikomiTanka = @MemberPriceWithTax;
                    SET @ZeinukiTanka = @MemberPriceOutTax;
                END
                ELSE IF @CustomerKBN = 2 --●CustomerKBN＝2の場合
                BEGIN
                    SET @ZeikomiTanka = @GeneralPriceWithTax;
                    SET @ZeinukiTanka = @GeneralPriceOutTax;
                END
                
            END
        END
    END
    
    SET @Zei = @ZeikomiTanka - @ZeinukiTanka;
    SET @Bikou = @Remarks;

	SELECT @GenkaTanka=LastCost
	FROM M_SKULastCost
	WHERE SKUCD = @SKUCD
	;
	
--END TRY

--BEGIN CATCH
---    RETURN;
--END CATCH

END

