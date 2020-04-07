 BEGIN TRY 
 Drop Procedure dbo.[Fnc_Reserve_SP]
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
CREATE PROCEDURE [dbo].[Fnc_Reserve_SP]
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
    @KariHikiateNo varchar(11),
    
    @Result tinyint OUTPUT,     --1:引当ＯＫ、2:引当一部ＯＫ、3:引当０
    @Error  tinyint OUTPUT, 
    @LastDay varchar(10) OUTPUT  ,
    @OutKariHikiateNo varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt int;
    DECLARE @StorePlaceKBN tinyint;
    DECLARE @SoukoStorePlaceKBN tinyint;
    DECLARE @StoreKBN tinyint;
    DECLARE @PlanDate date;
    DECLARE @StockStoreCD varchar(4);
    DECLARE @IdouCount tinyint;
    DECLARE @StoreIdouCount tinyint;
    DECLARE @wIdoCount tinyint;

    DECLARE @ZaikoKBN tinyint;
    DECLARE @DateChangeDate date;
    DECLARE @SetSU int;
    
    SET @DateChangeDate = CONVERT(date,@ChangeDate);
    SET @SetSU = 1;
    
--    DECLARE @SetAdminCD int;
    
    SET @Result = 0;
    SET @Error = 0;
    SET @LastDay = NULL;
    SET @OutKariHikiateNo = NULL;

--    SET @SetAdminCD = 0;
        
--BEGIN TRY
    --前処理-------------------------------------------------------------------------
    --【セット品の場合に、その構成している商品に洗替する】
    SET @iCnt = (SELECT COUNT(*) FROM F_SKU(@DateChangeDate) AS F
                WHERE F.AdminNO = @AdminNO
                AND F.SetKbn = 1
                AND F.DeleteFLG = 0
                );
                
    IF @iCnt > 0
    BEGIN
        SELECT @SetSU=F.SetSU
        FROM F_SKU(@DateChangeDate) AS F
        WHERE F.AdminNO = @AdminNO
        AND F.SetKbn = 1
        AND F.DeleteFLG = 0
        ;
    END

	--処理---------------------------------------------------------------------------

    --【引当対象在庫を作成する。】
    --①対象のレコードを削除する
    DELETE FROM D_TemporaryReserve
    WHERE [ReserveKBN] = @DenType
    AND [Number] = @DenNo
    AND [NumberRows] = @DenGyoNo
    ;
    
    DELETE FROM D_TemporaryReserve
    WHERE TemporaryNO = @KariHikiateNo
    ;

    DELETE FROM D_StockForReserve
    WHERE NeedStoreCD = @StoreCD
    AND AdminNO = @AdminNO
    ;

    --【チェック】
    --①in基準日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
                );
 
    IF @iCnt = 0
    BEGIN
        SET @Error = 1;
        RETURN;
    END
    
    --②inSKUCDがSKUマスター(M_SKU)に存在しなければ
    SET @iCnt = (SELECT COUNT(*) FROM M_SKU M
                WHERE M.ChangeDate <= convert(date,@ChangeDate)
                AND M.AdminNO = @AdminNO
                AND M.DeleteFLG = 0
                );
                
    IF @iCnt = 0
    BEGIN
        SET @Error = 1;
        RETURN;
    END
    ELSE
    BEGIN
    	SELECT @ZaikoKBN=M.ZaikoKBN FROM F_SKU(@DateChangeDate) AS M
        WHERE M.AdminNO = @AdminNO
        AND M.DeleteFLG = 0
        ;
    END
  
    --③in店舗CD≠Nullの場合、以下の条件で店舗マスター(M_Store)に存在しなければ、   
    IF ISNULL(@StoreCD,'') <> ''
    BEGIN
        SET @iCnt = (SELECT COUNT(*) FROM M_Store M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.StoreCD = @StoreCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            SET @Error = 1;
            RETURN;
        END
        
        --SET @WebTempo = 1;
        
        SELECT top 1 @StorePlaceKBN=StorePlaceKBN, @StoreKBN=StoreKBN FROM M_Store M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.StoreCD = @StoreCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
        
        IF @StoreKBN = 2    --2　:WEB店
        BEGIN
            SELECT top 1 @StoreCD=StoreCD,@StorePlaceKBN=StorePlaceKBN FROM M_Store M
            WHERE M.ChangeDate <= convert(date,@ChangeDate)
            AND M.DeleteFLG = 0
            AND M.StoreKBN = 3  --3　:WEBまとめ店舗
            ORDER BY M.ChangeDate desc
            ;
        END
    END

    --④    in倉庫CD≠Nullの場合、以下の条件で倉庫マスター(M_Souko)に存在しなければ、
    IF ISNULL(@SoukoCD,'') <> ''
    BEGIN
        SET @iCnt = (SELECT COUNT(*) FROM M_Souko M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.SoukoCD = @SoukoCD
                    AND M.DeleteFLG = 0
                    );
    END            
    IF @iCnt = 0
    BEGIN
        SET @Error = 1;
        RETURN;
    END
    
    /*
    --M_Store②
    SELECT top 1 @SoukoStorePlaceKBN=(SELECT top 1 S.StorePlaceKBN FROM M_Store AS S
                                        WHERE M.StoreCD = S.StoreCD 
                                        AND M.ChangeDate >= S.ChangeDate
                                        ORDER BY S.ChangeDate desc)
    FROM  M_Souko M
    WHERE M.ChangeDate <= convert(date,@ChangeDate)
    AND M.SoukoCD = @SoukoCD
    AND M.DeleteFLG = 0
    ORDER BY M.ChangeDate desc
    ;
    */
    
    --【仮引当番号を採番する】
    --伝票番号採番
    EXEC Fnc_GetNumber
        13,             --in伝票種別 13
        @ChangeDate, --in基準日
        @StoreCD,       --in店舗CD
        ' ',  --Operator
        @OutKariHikiateNo OUTPUT
        ;
    
        
    DECLARE @StockNO varchar(11);
    DECLARE @MSouko_SoukoCD varchar(6);
    DECLARE @MSouko_ChangeDate date;
    
    --【在庫管理する場合】
    --M_SKU.ZaikoKBN＝1 の場合
    IF @ZaikoKBN = 1
    BEGIN
    
        --【引当対象在庫を作成する。】
        --a in店舗CD＝D_Stock.StoreCDの場合 ←同じ店舗  ★M_SoukoのStoreCD
        --D_Stock.ArrivalPlanDate   
        DECLARE CUR_STOCK CURSOR FOR
            SELECT D.StockNO
            		, ISNULL(D.ArrivalPlanDate, convert(date,SYSDATETIME())) AS PlanDate
                    , D.SoukoCD
                    , (CASE D.ArrivalYetFLG WHEN 0 THEN D.ArrivalPlanDate ELSE convert(date,SYSDATETIME()) END) AS ChangeDate
            FROM D_Stock AS D
            WHERE D.AdminNO = @AdminNO
            AND D.AllowableSu > 0
            AND D.DeleteDateTime IS NULL
            AND D.ArrivalPlanKBN <> 1   --1:受発注分を除く
            AND EXISTS(SELECT 1 FROM M_Souko AS M 
                WHERE M.DeleteFLG = 0
                AND M.HikiateOrder <> 0     --引当対象の倉庫
                AND M.ChangeDate <= (CASE D.ArrivalYetFLG WHEN 0 THEN D.ArrivalPlanDate
                ELSE convert(date,SYSDATETIME()) END)
                AND M.SoukoCD = D.SoukoCD)
            ORDER BY D.StockNO
            ;
        
        --カーソルオープン
        OPEN CUR_STOCK;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_STOCK
        INTO @StockNO
          ,@PlanDate
          ,@MSouko_SoukoCD
          ,@MSouko_ChangeDate;

        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
            
            SELECT top 1 @StockStoreCD = M.StoreCD--, @PlanDate = ISNULL(D.ArrivalPlanDate, convert(date,SYSDATETIME()))
                ,@IdouCount = M.IdouCount, @StoreIdouCount = M.StoreIdouCount
                ,@SoukoStorePlaceKBN=S.StorePlaceKBN
            FROM F_Souko(@MSouko_ChangeDate) AS M 
            LEFT OUTER JOIN (SELECT S.StorePlaceKBN,S.StoreCD,S.ChangeDate 
                            FROM M_Store AS S
                            WHERE S.DeleteFLG = 0
                            --ORDER BY S.ChangeDate desc
                ) AS S
                ON S.StoreCD = M.StoreCD AND M.ChangeDate >= S.ChangeDate
            WHERE M.SoukoCD = @MSouko_SoukoCD
            ORDER BY S.ChangeDate desc
            ;             
            
            --in店舗CD≠D_Stock.StoreCDの場合←違う店舗
            IF @StoreCD <> @StockStoreCD
            BEGIN
                --←対本社への移動（どちらかの店舗が本社）
                IF @StorePlaceKBN = 1 OR @SoukoStorePlaceKBN = 1
                BEGIN
                    --M_Souko.IdouCountをw移動日数とする
                    SET @wIdoCount = @IdouCount;
                END
                ELSE
                BEGIN
                    --M_Souko.StoreIdouCountをw移動日数とする
                    SET @wIdoCount = @StoreIdouCount;
                END
                
                --D_Stock.ArrivalPlanDateに対し、w移動日数後の日付を換算入荷予定日として求める
                SELECT @PlanDate = dbo.GetPlanDate(@PlanDate, @wIdoCount, @StorePlaceKBN);

            END
            
            --①テーブル転送仕様Ａに従って、対象SKUCDのD_StockForReserveに追加する
            INSERT INTO [D_StockForReserve]
                   ([NeedStoreCD]
                   ,[StockNO]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[StockStoreCD]
                   ,[SoukoCD]
                   ,[RackNo]
                   ,[CalcuArrivalPlanDate]
                   ,[HikiateOrder]
                   ,[AllowableSu]
        --         ,[ReserveSu]
                   )
             SELECT @StoreCD        
                   ,D.StockNO        
                   ,D.SKUCD              
                   ,D.AdminNO            
                   ,D.JanCD              
                   ,@StockStoreCD       
                   ,D.SoukoCD            
                   ,D.RackNo
                   ,@PlanDate
                   ,(SELECT M.HikiateOrder FROM F_Souko(@MSouko_ChangeDate) AS M WHERE M.SoukoCD = @MSouko_SoukoCD)
                   ,(CASE WHEN @StoreCD <> @StockStoreCD THEN D.AnotherStoreAllowableSu ELSE D.AllowableSu END)
            FROM D_Stock AS D
            WHERE D.StockNO = @StockNO
            ;             

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_STOCK
            INTO @StockNO
              ,@PlanDate
	          ,@MSouko_SoukoCD
	          ,@MSouko_ChangeDate;
        END
        
        --カーソルを閉じる
        CLOSE CUR_STOCK;
        DEALLOCATE CUR_STOCK;

        --②既に該当受注で引当している数量を加える（その受注等で以前に引当していた数量をいったん戻す）ため、    
        --テーブル転送仕様Ｂに従って対象SKUCDのD_StockForReserveに追加する
        IF ISNULL(@DenNO,'') <> ''
        BEGIN
            DECLARE CUR_DENNO CURSOR FOR
                SELECT D.StockNO
                        , ISNULL(D.ArrivalPlanDate, convert(date,SYSDATETIME())) AS PlanDate
                        , D.SoukoCD
                        , (CASE D.ArrivalYetFLG WHEN 0 THEN D.ArrivalPlanDate ELSE convert(date,SYSDATETIME()) END) AS ChangeDate
                        ,R.ReserveSu - D.ShippingSu AS HikiateSu
                FROM D_Stock AS D
                INNER JOIN D_Reserve AS R 
                ON R.StockNO = D.StockNO AND R.ReserveKBN = 1 
                AND R.[Number] = @DenNO AND R.NumberRows = @DenGyoNo
                AND R.AdminNO = @AdminNO AND R.ReserveSu > R.ShippingSu

                WHERE EXISTS(SELECT 1 FROM M_Souko AS M 
                    WHERE M.DeleteFLG = 0
                  --  AND M.HikiateOrder <> 0     --引当対象の倉庫
                    AND M.ChangeDate <= (CASE D.ArrivalYetFLG WHEN 0 THEN D.ArrivalPlanDate
                    ELSE convert(date,SYSDATETIME()) END)
                    AND M.SoukoCD = D.SoukoCD)
                ORDER BY D.StockNO
                ;
        	DECLARE @HikiateSu int;
        	
            --カーソルオープン
            OPEN CUR_DENNO;
            
            --最初の1行目を取得して変数へ値をセット
            FETCH NEXT FROM CUR_DENNO
            INTO @StockNO
              ,@PlanDate
              ,@MSouko_SoukoCD
              ,@MSouko_ChangeDate
              ,@HikiateSu;

            --データの行数分ループ処理を実行する
            WHILE @@FETCH_STATUS = 0
            BEGIN
                  
                UPDATE [D_StockForReserve]
                   SET [HikiateOrder] = (SELECT M.HikiateOrder FROM F_Souko(@MSouko_ChangeDate) AS M WHERE M.SoukoCD = @MSouko_SoukoCD)
                      ,[AllowableSu] = D_StockForReserve.AllowableSu + @HikiateSu	--R.ReserveSu - D.ShippingSu
                 FROM D_Stock AS D
                WHERE D_StockForReserve.[NeedStoreCD] = @StoreCD
                AND D_StockForReserve.[StockNO] = D.StockNO
                AND D_StockForReserve.[SKUCD] = D.SKUCD
                AND D_StockForReserve.[AdminNO] = D.AdminNO
                AND D_StockForReserve.[SoukoCD] = D.SoukoCD
                AND D_StockForReserve.[RackNo] = D.RackNo
                AND D.StockNO = @StockNO
                ;
                
                IF @@ROWCOUNT = 0 
                BEGIN
                    --a in店舗CD＝D_Stock.StoreCDの場合 ←同じ店舗  ★M_SoukoのStoreCD
                    --D_Stock.ArrivalPlanDate   
                    SELECT top 1 @StockStoreCD = M.StoreCD--, @PlanDate = ISNULL(D.ArrivalPlanDate, convert(date,SYSDATETIME()))
                        ,@IdouCount = M.IdouCount, @StoreIdouCount = M.StoreIdouCount
                        ,@SoukoStorePlaceKBN=S.StorePlaceKBN
                    FROM F_Souko(@MSouko_ChangeDate) AS M 
                    LEFT OUTER JOIN (SELECT S.StorePlaceKBN,S.StoreCD,S.ChangeDate 
                                    FROM M_Store AS S
                                    WHERE S.DeleteFLG = 0
                                    --ORDER BY S.ChangeDate desc
                        ) AS S
                        ON S.StoreCD = M.StoreCD AND M.ChangeDate >= S.ChangeDate
                    WHERE M.SoukoCD = @MSouko_SoukoCD
                    ORDER BY S.ChangeDate desc
                    ;                    
                    
                    --in店舗CD≠D_Stock.StoreCDの場合←違う店舗
                    IF @StoreCD <> @StockStoreCD
                    BEGIN
                        --←対本社への移動（どちらかの店舗が本社）
                        IF @StorePlaceKBN = 1 OR @SoukoStorePlaceKBN = 1
                            --M_Souko.IdouCountをw移動日数とする
                            SET @wIdoCount = @IdouCount;
                        ELSE
                            --M_Souko.StoreIdouCountをw移動日数とする
                            SET @wIdoCount = @StoreIdouCount;
                            
                        --D_Stock.ArrivalPlanDateに対し、w移動日数後の日付を換算入荷予定日として求める
                        SELECT @PlanDate = dbo.GetPlanDate(@PlanDate, @wIdoCount, @StorePlaceKBN);

                    END
                    
                    INSERT INTO [D_StockForReserve]
                           ([NeedStoreCD]
                           ,[StockNO]
                           ,[SKUCD]
                           ,[AdminNO]
                           ,[JanCD]
                           ,[StockStoreCD]
                           ,[SoukoCD]
                           ,[RackNo]
                           ,[CalcuArrivalPlanDate]
                           ,[HikiateOrder]
                           ,[AllowableSu]
                           ,[ReserveSu]
                           )
                     SELECT
                            @StoreCD        
                           ,D.StockNO            
                           ,D.SKUCD              
                           ,D.AdminNO            
                           ,D.JanCD              
                           ,@StockStoreCD       
                           ,D.SoukoCD            
                           ,D.RackNo
                           ,@PlanDate
                           ,(SELECT M.HikiateOrder FROM F_Souko(@MSouko_ChangeDate) AS M WHERE M.SoukoCD = @MSouko_SoukoCD)
                           ,@HikiateSu	--R.ReserveSu - D.ShippingSu
                           ,0 AS ReserveSu
                    FROM D_Stock AS D
                    WHERE D.StockNO = @StockNO
                    ;
                END
                
                --次の行のデータを取得して変数へ値をセット
                FETCH NEXT FROM CUR_DENNO
                INTO @StockNO
                  ,@PlanDate
                  ,@MSouko_SoukoCD
                  ,@MSouko_ChangeDate
              	  ,@HikiateSu;
                
            END
            
            --カーソルを閉じる
            CLOSE CUR_DENNO;
            DEALLOCATE CUR_DENNO;
        END
            
        --【引当を行う。】
        DECLARE @wSuu int;
        DECLARE @wSumHikSu int;
        DECLARE @wLastDay date;
        DECLARE @wHikSu int;
        --DECLARE @StockNO varchar(11);
        DECLARE @AvailableSu int;
        DECLARE @CalcuArrivalPlanDate date;
        DECLARE @RackNo varchar(11);
        --①
        SET @wSuu = @Suryo;
        SET @wSumHikSu = 0;
        SET @wLastDay = NULL;
        SET @wHikSu = 0;
        
        --②引当処理在庫(D_StockForReserve）を以下の条件でSelectし、引当を行う
        --カーソル定義
        DECLARE CUR1 CURSOR FOR
            SELECT D.StockNO, D.AllowableSu - D.ReserveSu AS AvailableSu
                ,D.CalcuArrivalPlanDate
                ,D.RackNo
            FROM D_StockForReserve AS D
            WHERE D.NeedStoreCD = @StoreCD
            AND D.AdminNO = @AdminNO
            AND D.AllowableSu > D.ReserveSu
            ORDER BY D.CalcuArrivalPlanDate, D.HikiateOrder
            ;

        --カーソルオープン
        OPEN CUR1;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR1
        INTO @StockNO
          ,@AvailableSu
          ,@CalcuArrivalPlanDate
          ,@RackNo;

        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
            --D_StockForReserve.AllowableSu－ReserveSu≧w対象数なら 
            IF @AvailableSu >= @wSuu
            BEGIN
                SET @wHikSu = @wSuu;
                SET @wSumHikSu = @wSumHikSu + @wHikSu;
                SET @wLastDay = @CalcuArrivalPlanDate;
            
                --テーブル転送仕様Ｃ－①に従って、D_StockForReserveをUpdateする 
                UPDATE D_StockForReserve
                SET AllowableSu = AllowableSu-@wHikSu * @SetSu   --★確認
                WHERE NeedStoreCD = @StoreCD
                AND StockNO = @StockNO
                ;

                --テーブル転送仕様Ｄに従って、D_TemporaryReserveをInsertする
                INSERT INTO D_TemporaryReserve
                   ([TemporaryNO]
                   ,[ReserveKBN]
                   ,[Number]
                   ,[NumberRows]
                   ,[ZaikoKBN]
                   ,[StockNO]
                   ,[ReserveSu]
                   ,[UpdateSU]
                   ,[InsertDateTime])
                VALUES
                   (@OutKariHikiateNo      
                   ,@DenType       
                   ,@DenNo           
                   ,@DenGyoNo       
                   ,@ZaikoKBN
                   ,@StockNO    --update          
                   ,@wHikSu        
                   ,@wHikSu * @SetSu--UpdateSU
                   ,SYSDATETIME()  
                   );
                
                --引当処理終了
                BREAK;
            END
            ELSE
            BEGIN
                --D_StockForReserve.AllowableSu－ReserveSu＜w対象数なら
                --w引当数←D_StockForReserve.AllowableSu－ReserveSuとする
                SET @wHikSu = @AvailableSu;
                SET @wSumHikSu = @wSumHikSu + @wHikSu;
                
                --テーブル転送仕様Ｃ－②に従って、D_StockForReserveをUpdateする
                UPDATE D_StockForReserve
                SET AllowableSu = AllowableSu-@wHikSu * @SetSu
                WHERE NeedStoreCD = @StoreCD
                AND StockNO = @StockNO
                ;

                --テーブル転送仕様Ｄに従って、D_TemporaryReserveをInsertする
                INSERT INTO D_TemporaryReserve
                   ([TemporaryNO]
                   ,[ReserveKBN]
                   ,[Number]
                   ,[NumberRows]
                   ,[ZaikoKBN]
                   ,[StockNO]
                   ,[ReserveSu]
                   ,[UpdateSU]
                   ,[InsertDateTime])
                VALUES
                   (@OutKariHikiateNo      
                   ,@DenType       
                   ,@DenNo           
                   ,@DenGyoNo       
                   ,@ZaikoKBN
                   ,@StockNO    --update          
                   ,@wHikSu        
                   ,@wHikSu * @SetSu--UpdateSU
                   ,SYSDATETIME()  
                   );
                   
                SET @wSuu = @wSuu - @wHikSu;
                SET @wLastDay = @CalcuArrivalPlanDate;

            END
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR1
            INTO @StockNO
              ,@AvailableSu
              ,@CalcuArrivalPlanDate
              ,@RackNo;
        END
        
        --カーソルを閉じる
        CLOSE CUR1;
        DEALLOCATE CUR1;

    END --【在庫管理する場合】
    ELSE--【在庫管理しない場合】
    BEGIN
        --テーブル転送仕様Ｅに従って、D_TemporaryReserveをInsertする
        INSERT INTO D_TemporaryReserve
           ([TemporaryNO]
           ,[ReserveKBN]
           ,[Number]
           ,[NumberRows]
           ,[ZaikoKBN]
           ,[StockNO]
           ,[ReserveSu]
           ,[UpdateSU]
           ,[InsertDateTime])
        VALUES
           (@OutKariHikiateNo      
           ,@DenType       
           ,@DenNo           
           ,@DenGyoNo       
           ,@ZaikoKBN
           ,'99999999999'    --update          
           ,@Suryo
           ,@Suryo
           ,SYSDATETIME()  
           );
        

    END

    --【在庫管理する場合】
    --M_SKU.ZaikoKBN＝1 の場合
    IF @ZaikoKBN = 1
    BEGIN
        --【Return values】
        IF @wSumHikSu = 0
        BEGIN
        
            SET @Result = 3;    --1:引当ＯＫ、2:引当一部ＯＫ、3:引当０
            SET @Error = 0;
            SET @LastDay = CONVERT(varchar, @wLastDay, 111);
        END
        ELSE IF @wSumHikSu = @Suryo
        BEGIN
        
            SET @Result = 1;    --1:引当ＯＫ、2:引当一部ＯＫ、3:引当０
            SET @Error = 0;
            SET @LastDay = CONVERT(varchar, @wLastDay, 111);
        END
        ELSE IF @wSumHikSu < @Suryo
        BEGIN
        
            SET @Result = 2;    --1:引当ＯＫ、2:引当一部ＯＫ、3:引当０
            SET @Error = 0;
            SET @LastDay = CONVERT(varchar, @wLastDay, 111);
        END
    END--【在庫管理する場合】
    ELSE--【在庫管理しない場合】
    BEGIN
        
            SET @Result = 1;    --1:引当ＯＫ、2:引当一部ＯＫ、3:引当０
            SET @Error = 0;
            SET @LastDay = CONVERT(varchar, SYSDATETIME(), 111);
    END
    
--END TRY

--BEGIN CATCH
	--PRINT 'In catch block.';  
--    THROW; 
--    RETURN;
--END CATCH

END

