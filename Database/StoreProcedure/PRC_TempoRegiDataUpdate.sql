 BEGIN TRY 
 Drop Procedure dbo.[PRC_TempoRegiDataUpdate]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗レジ データ更新
--       Program ID      TempoRegiDataUpdate
--       Create date:    2020.01.10
--    ======================================================================
                   
CREATE PROCEDURE [dbo].[PRC_TempoRegiDataUpdate]
    (@OperateMode    int,                 -- 1:通常、2:返品→4、3;訂正→2、4:取消→3
    @SalesNO  varchar(11),
    @StoreCD  varchar(4),
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutCollectPlanNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @MODE_NORMAL tinyint;
    DECLARE @MODE_HENPIN tinyint;
    DECLARE @MODE_TEISEI tinyint;
    DECLARE @MODE_CANCEL tinyint;   
    
    SET @MODE_NORMAL = 1;
    SET @MODE_HENPIN = 4;
    SET @MODE_TEISEI = 2;
    SET @MODE_CANCEL = 3;   
	
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @SYSDATE date;
    DECLARE @SYSDATE_VAR varchar(10);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @COUNT int;
    
    DECLARE @DataNo   int;
    DECLARE @DataRows int;
    DECLARE @RecoredKBN tinyint;
    DECLARE @JuchuuNO varchar(11);
    DECLARE @JuchuuRows int;
    DECLARE @AdminNO int;
    DECLARE @SalesDate date;
    DECLARE @SalesDate_VAR varchar(10);
    DECLARE @CustomerCD  varchar(13);
    DECLARE @ZaikoKBN tinyint;
    DECLARE @Program varchar(50);

    DECLARE @DiscountAmount money;
    DECLARE @BillingAmount money;
    DECLARE @PointAmount money;
    --DECLARE @CardDenominationCD, varchar(3),;
    DECLARE @CardAmount money;
    DECLARE @CashAmount money;
    DECLARE @DepositAmount money;
    DECLARE @RefundAmount money;
    DECLARE @CreditAmount money;
    --DECLARE @Denomination1CD, varchar(3),;
    DECLARE @Denomination1Amount money;
    --DECLARE @Denomination2CD, varchar(3),;
    DECLARE @Denomination2Amount money;
    DECLARE @AdvanceAmount money;
    DECLARE @SoukoCD varchar(6);
	DECLARE @SalesSU int;

    --引当可能ロジック
    DECLARE @return_value int,
            @Result tinyint,
            @Error tinyint,
            @LastDay varchar(10),
            @OutKariHikiateNo varchar(11);

    DECLARE @ReserveNO varchar(11);
    DECLARE @KeySeq int;
    DECLARE @StockNO varchar(11);
    DECLARE @StockSu int;
    DECLARE @Yoteibi  varchar(10);
    DECLARE @FirstCollectPlanDate date;		--@YoteibiをdateにCONVETしたもの
    DECLARE @CollectPlanNO int;
    DECLARE @CollectPlanRows int;
    DECLARE @BillingNO varchar(11);
    DECLARE @CollectNO varchar(11);
    DECLARE @BillingCD varchar(13);
    DECLARE @ConfirmNO varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(date,@SYSDATETIME);
    SET @SYSDATE_VAR = CONVERT(varchar,@SYSDATE,111);
    SET @Program = 'TempoRegiDataUpdate';
    SET @COUNT = 0;
    
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        Select B.DataNo, B.DataRows
            ,A.RecoredKBN, B.JuchuuNO, B.JuchuuRows, B.AdminNO, A.SalesDate, A.CustomerCD
            ,A.DiscountAmount
            ,A.BillingAmount
            ,A.PointAmount
            --,A.CardDenominationCD
            ,A.CardAmount
            ,A.CashAmount
            ,A.DepositAmount
            ,A.RefundAmount
            ,A.CreditAmount
            --,A.Denomination1CD
            ,A.Denomination1Amount
            --,A.Denomination2CD
            ,A.Denomination2Amount
            ,A.AdvanceAmount
            
            ,(Select top 1 M.SoukoCD
                From M_Souko AS M   
                Where A.StoreCD = @StoreCD     
                And M.ChangeDate <= A.SalesDate
                And M.DeleteFlg = 0         
                And M.SoukoType = 3		--店舗メイン倉庫（←実質は店頭）
                ORDER BY M.ChangeDate desc) AS SoukoCD
            ,B.SalesSU
            ,CONVERT(varchar,A.SalesDate,111)
            
        From D_SalesTran AS A
        INNER JOIN  D_SalesDetailsTran AS B
        ON  A.DataNo = B.DataNo
        AND B.DeleteDateTime IS NULL
        Where A.StoreCD = @StoreCD
        AND A.SalesNO = @SalesNO
        AND A.StoreSalesUpdateFLG = 0
        Order by B.DataNo, B.DataRows
        ;

    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO  @DataNo,@DataRows,@RecoredKBN,@JuchuuNO, @JuchuuRows, @AdminNO, @SalesDate, @CustomerCD
        ,@DiscountAmount 
        ,@BillingAmount 
        ,@PointAmount 
        --,@CardDenominationCD, varchar(3),;
        ,@CardAmount 
        ,@CashAmount 
        ,@DepositAmount 
        ,@RefundAmount 
        ,@CreditAmount 
        --,@Denomination1CD, varchar(3),;
        ,@Denomination1Amount 
        --,@Denomination2CD, varchar(3),;
        ,@Denomination2Amount
        ,@AdvanceAmount
        ,@SoukoCD
        ,@SalesSU
        ,@SalesDate_VAR
	    ;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===
    	
        --全ての場合
        --商品情報を獲得
        SET @ZaikoKBN = (SELECT top 1 A.ZaikoKBN
                        FROM M_SKU AS A
                        WHERE A.AdminNO = @AdminNO
                        AND A.DeleteFLG = 0
                        AND A.ChangeDate <= @SalesDate
                        ORDER BY A.ChangeDate desc);
                        
        --1.D_SalesTran RecoredKBN＝1（赤）の場合Parameter受取.modeは、３，４のみ
        IF @RecoredKBN = 1
        BEGIN
            IF @OperateMode IN (@MODE_TEISEI,@MODE_CANCEL)
            BEGIN
                --★１件目のD_SalesDetailsTran.JuchuuNOを覚えておく
                --在庫管理する場合
                --  M_SKU.ZaikoKBN＝１の場合（＝０の場合は、何もしない）
                IF @ZaikoKBN = 1
                BEGIN
                    --テーブル転送仕様２－②Update在庫      ←順番注意
                    UPDATE [D_Stock] SET
                           [StockSu] = [StockSu] + DR.ReserveSu
                          ,[AllowableSu] = [AllowableSu] + DR.ReserveSu
                          ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + DR.ReserveSu
                          ,[ShippingSu] = [D_Stock].[ShippingSu] - DR.ReserveSu
                          ,[UpdateOperator] =  @Operator  
                          ,[UpdateDateTime] =  @SYSDATETIME
                          
                    FROM D_Reserve AS DR
                    WHERE D_Stock.DeleteDateTime IS NULL
                    AND D_Stock.StockNO = DR.StockNO
                    AND DR.[Number] = @JuchuuNO
                    AND DR.DeleteDateTime IS NULL
                    ;
                    
                    --テーブル転送仕様３－②insert入出庫履歴←順番注意
                    --【D_Warehousing】
                    INSERT INTO [D_Warehousing]
                       ([WarehousingDate]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[StockNO]
                       ,[JanCD]
                       ,[AdminNO]
                       ,[SKUCD]
                       ,[WarehousingKBN]
                       ,[DeleteFlg]
                       ,[Number]
                       ,[NumberRow]
                       ,[VendorCD]
                       ,[ToStoreCD]
                       ,[ToSoukoCD]
                       ,[ToRackNO]
                       ,[ToStockNO]
                       ,[FromStoreCD]
                       ,[FromSoukoCD]
                       ,[FromRackNO]
                       ,[CustomerCD]
                       ,[Quantity]
                       ,[UnitPrice]
                       ,[Amount]
                       ,[Program]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT @SalesDate --WarehousingDate
                       ,DJ.SoukoCD
                       ,DS.RackNO    --RackNO
                       ,DS.StockNO
                       ,DM.JanCD
                       ,DM.AdminNO
                       ,DM.SKUCD
                       ,24   --WarehousingKBN 24：売上変更
                       ,0  --DeleteFlg
                       ,DM.SalesNO  --Number
                       ,DM.SalesRows --NumberRow
                       ,NULL    --VendorCD
                       ,NULL --ToStoreCD
                       ,NULL --ToSoukoCD
                       ,NULL--ToRackNO
                       ,NULL    --ToStockNO
                       ,NULL	--FromStoreCD
                       ,NULL	--FromSoukoCD
                       ,NULL	--FromRackNO
                       ,DH.CustomerCD    --CustomerCD
                       ,(-1)*DR.ReserveSu   --Quantity
                       ,0	--UnitPrice
                       ,0	--Amount
                       ,@Program  --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL
                    FROM D_SalesDetailsTran AS DM
                    INNER JOIN D_SalesTran AS DH
        			ON DH.DataNo = DM.DataNo
                    INNER JOIN D_Reserve AS DR
                    ON DR.[Number] = DM.JuchuuNO
                    AND DR.DeleteDateTime IS NULL
                    INNER JOIN D_Stock AS DS
                    ON DS.DeleteDateTime IS NULL
                    AND DS.StockNO = DR.StockNO
                    LEFT OUTER JOIN D_Juchuu AS DJ
                    ON DJ.JuchuuNO = DR.[Number]
                    --D_Juchuu.DeleteDateTimeは条件に含めない
                    WHERE DM.DataNo = @DataNo
                    AND DM.DataRows = @DataRows
                    AND DM.DeleteDateTime IS NULL
                    ;
                    
                    --テーブル転送仕様１－②Update引当      ←順番注意
                    UPDATE D_Reserve SET
                         [UpdateOperator]     =  @Operator  
                        ,[UpdateDateTime]     =  @SYSDATETIME
                        ,[DeleteOperator]     =  @Operator  
                        ,[DeleteDateTime]     =  @SYSDATETIME
                    WHERE [Number] = @JuchuuNO
                    AND DeleteDateTime IS NULL
                    ;
                END
                
                --以下、在庫管理する・しないに関わらず
                --①掛売の場合
                --掛額  ≠  0（ D_SalesTran  CreditAmount≠0 ） 
                IF @CreditAmount <> 0
                BEGIN
                    --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                
                    --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
				END
				
                --現金の場合
                --現金額≠0（D_SalesTran.CashAmount≠0）
                IF @CashAmount <> 0
                BEGIN
                    --テーブル転送仕様11－⑨Update入金消込明細 D_CollectBillingDetails
                    EXEC UPDATE_D_CollectBillingDetails_11_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様10－⑨Update入金消込請求 D_CollectBilling
                    EXEC UPDATE_D_CollectBilling_10_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                    
                    --テーブル転送仕様９－⑨Update入金消込D_PaymentConfirm
                    EXEC UPDATE_D_PaymentConfirm_9_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                    
                    --テーブル転送仕様８－⑨Update入金 D_Collect
                    EXEC UPDATE_D_Collect_8_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
	                
	                --テーブル転送仕様７－⑨Update請求明細D_BillingDetails
	                EXEC UPDATE_D_BillingDetails_7_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
	                
	                --テーブル転送仕様６－⑨Update請求 D_Billing
	                EXEC UPDATE_D_Billing_6_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
	                
	                --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                    
                    --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
				END
				
                --③カード系の場合  paypayなどもココ
                --カード系の場合、請求先は店舗で購入したお客さん本人でなく、カード会社やpaypayなどになる
                --カード額≠0（D_SalesTran.CardAmount≠0）
                IF @CardAmount <> 0
                BEGIN
	                --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
	                --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                END
                
                --その他入金１額≠0（D_SalesTran.Denomination1Amount≠
                IF @Denomination1Amount <> 0
                BEGIn
                    --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
	                --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                END        
                --その他入金２額≠0（D_SalesTran.Denomination2Amount≠
                IF @Denomination2Amount <> 0
                BEGIN
                    --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
	                --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
	                --テーブル転送仕様７－⑨Update請求明細①　D_BillingDetails
	                EXEC UPDATE_D_BillingDetails_7_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
	                --テーブル転送仕様６－⑨Update請求①　D_Billing
	                EXEC UPDATE_D_Billing_6_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                END
                --④前受金の場合（野球のチーム監督などが「先に払っておいた金額」を取り崩しする場合など）
                --前受金≠0（D_SalesTran.AdvanceAmount≠0）
                IF @AdvanceAmount <> 0
                BEGIN
                    --テーブル転送仕様12－⑨Update入金D_Collect
                    UPDATE [D_Collect] SET
                         [ConfirmAmount] = [ConfirmAmount] - DS.AdvanceAmount
                        ,[UpdateOperator]     =  @Operator  
                        ,[UpdateDateTime]     =  @SYSDATETIME
                    FROM D_StoreAdvance AS DS                    
                    WHERE DS.SalesNO = @SalesNO
                    AND D_Collect.CollectNO = DS.CollectNO
                    AND D_Collect.DeleteDateTime IS NULL
                    ;

                    --テーブル転送仕様11－⑨Update入金消込明細 D_CollectBillingDetails
                    EXEC UPDATE_D_CollectBillingDetails_11_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様10－⑨Update入金消込請求 D_CollectBilling
                    EXEC UPDATE_D_CollectBilling_10_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様９－⑨Update入金消込 D_PaymentConfirm
                    EXEC UPDATE_D_PaymentConfirm_9_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様８－⑨Update入金 D_Collect
                    EXEC UPDATE_D_Collect_8_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様７－⑨Update請求明細D_BillingDetails
	                EXEC UPDATE_D_BillingDetails_7_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様６－⑨Update請求D_Billing
	                EXEC UPDATE_D_Billing_6_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様５－⑨Update回収予定明細　D_CollectPlanDetails
                    EXEC UPDATE_D_CollectPlanDetails_5_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                        
                    --テーブル転送仕様４－⑨Update回収予定　D_CollectPlan
                    EXEC UPDATE_D_CollectPlan_4_9
                        @SalesNO 
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                END                
            END
        END

        --2.D_SalesTran.RecoredKBN＝0（黒）の場合Parameter受取.modeは、１，２，３のみ            
        ELSE IF @RecoredKBN = 0
        BEGIN
            
            IF @OperateMode IN (@MODE_NORMAL,@MODE_HENPIN,@MODE_TEISEI)
            BEGIN
                --★１件目のD_SalesDetailsTran.JuchuuNOを覚えておく
                --在庫管理する場合
                --  M_SKU.ZaikoKBN＝１の場合（＝０の場合は、何もしない）
                IF @ZaikoKBN = 1
                BEGIN
                    --Parameter受取.mode＝１通常 or ３訂正の場合
                    IF @OperateMode IN (@MODE_NORMAL,@MODE_TEISEI)
                    BEGIN
                        --Function_商品引当.
                        EXEC Fnc_Reserve_SP
                            @AdminNO,
                            @SalesDate_VAR,
                            @StoreCD,
                            @SoukoCD,
                            @SalesSU,
                            1,  --@DenType
                            @JuchuuNO, 
                            @JuchuuRows,
                            NULL,
                            @Result OUTPUT,
                            @Error OUTPUT,
                            @LastDay OUTPUT,
                            @OutKariHikiateNo OUTPUT
                            ;
                        --テーブル転送仕様２－① Update 在庫     ←順番注意
                        UPDATE [D_Stock] SET
                               [StockSu] = [StockSu] - DT.ReserveSu
                              ,[AllowableSu] = [AllowableSu] - DT.ReserveSu
                              ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - DT.ReserveSu
                              ,[ShippingSu] = [ShippingSu] + DT.ReserveSu
                              ,[UpdateOperator] =  @Operator  
                              ,[UpdateDateTime] =  @SYSDATETIME
                              
                        FROM D_TemporaryReserve AS DT
                        WHERE D_Stock.DeleteDateTime IS NULL
                        AND D_Stock.StockNO = DT.StockNO
                        AND DT.TemporaryNO = @OutKariHikiateNo
                        ;
                        
                        --テーブル転送仕様３－① insert 入出庫履歴     ←順番注意  
                        --【D_Warehousing】
                        INSERT INTO [D_Warehousing]
                           ([WarehousingDate]
                           ,[SoukoCD]
                           ,[RackNO]
                           ,[StockNO]
                           ,[JanCD]
                           ,[AdminNO]
                           ,[SKUCD]
                           ,[WarehousingKBN]
                           ,[DeleteFlg]
                           ,[Number]
                           ,[NumberRow]
                           ,[VendorCD]
                           ,[ToStoreCD]
                           ,[ToSoukoCD]
                           ,[ToRackNO]
                           ,[ToStockNO]
                           ,[FromStoreCD]
                           ,[FromSoukoCD]
                           ,[FromRackNO]
                           ,[CustomerCD]
                           ,[Quantity]
                           ,[UnitPrice]
                           ,[Amount]
                           ,[Program]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                        SELECT @SalesDate --WarehousingDate
                           ,DJ.SoukoCD
                           ,DS.RackNO    --RackNO
                           ,DS.StockNO
                           ,DM.JanCD
                           ,DM.AdminNO
                           ,DM.SKUCD
                           ,25   --WarehousingKBN 25：店頭売上
                           ,0  --DeleteFlg
                           ,DM.SalesNO  --Number
                           ,DM.SalesRows --NumberRow
                           ,NULL    --VendorCD
                           ,NULL --ToStoreCD
                           ,NULL --ToSoukoCD
                           ,NULL--ToRackNO
                           ,NULL    --ToStockNO
                           ,NULL    --FromStoreCD
                           ,NULL    --FromSoukoCD
                           ,NULL    --FromRackNO
                           ,DH.CustomerCD    --CustomerCD
                           ,DT.ReserveSu   --Quantity
                           ,0   --UnitPrice     2020/01/27 add
                           ,0   --Amount        2020/01/27 add
                           ,@Program  --Program
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL
                           ,NULL
                        FROM D_SalesDetailsTran AS DM
                        INNER JOIN D_SalesTran AS DH
                        ON DH.DataNo = DM.DataNo
	                    INNER JOIN D_TemporaryReserve AS DT
	                    ON DT.TemporaryNO = @OutKariHikiateNo	--out仮引当番号	複数Recordありの可能性
                        INNER JOIN D_Stock AS DS
                        ON DS.DeleteDateTime IS NULL
                        AND DS.StockNO = DT.StockNO
                        LEFT OUTER JOIN D_Juchuu AS DJ
                        ON DJ.JuchuuNO = DT.[Number]
                        --D_Juchuu.DeleteDateTimeは条件に含めない
                        WHERE DM.DataNo = @DataNo
                        AND DM.DataRows = @DataRows
                        AND DM.DeleteDateTime IS NULL
                        ;
                          
                        --テーブル転送仕様１－① insert 引当     ←順番注意
                        --D_TemporaryReserveに該当のレコードは複数。
                        --各レコードで処理必要。
                        DECLARE CUR_Tem CURSOR FOR
                            SELECT A.KeySeq
                            FROM D_TemporaryReserve A
                            LEFT OUTER JOIN D_Stock B ON B.StockNO = A.StockNO
                            WHERE A.TemporaryNO = @OutKariHikiateNo
                            ORDER BY A.KeySeq
                            ;
                            
                        --カーソルオープン
                        OPEN CUR_Tem;

                        --最初の1行目を取得して変数へ値をセット
                        FETCH NEXT FROM CUR_Tem
                        INTO  @KeySeq;
                        
                        --データの行数分ループ処理を実行する
                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                        -- ========= ループ内の実際の処理 ここから===
                            --伝票番号採番
                            EXEC Fnc_GetNumber
                                12,             --in伝票種別 12
                                @SalesDate_VAR,    --in基準日
                                @StoreCD,       --in店舗CD
                                @Operator,
                                @ReserveNO OUTPUT
                                ;
                            
                            IF ISNULL(@ReserveNO,'') = ''
                            BEGIN
                                SET @W_ERR = 1;
                                RETURN @W_ERR;
                            END
                            
                            --【D_Reserve】
                            INSERT INTO [D_Reserve]
                                   ([ReserveNO]
                                   ,[ReserveKBN]
                                   ,[Number]
                                   ,[NumberRows]
                                   ,[StockNO]
                                   ,[SoukoCD]
                                   ,[JanCD]
                                   ,[SKUCD]
                                   ,[AdminNO]
                                   ,[ReserveSu]
                                   ,[ShippingPossibleDate]
                                   ,[ShippingPossibleSU]
                                   ,[ShippingOrderNO]
                                   ,[ShippingOrderRows]
                                   ,[CompletedPickingNO]
                                   ,[CompletedPickingRow]
                                   ,[CompletedPickingDate]
                                   ,[ShippingSu]
                                   ,[ReturnKBN]
                                   ,[OriginalReserveNO]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime]
                                   ,[DeleteOperator]
                                   ,[DeleteDateTime])
                            SELECT @ReserveNO
                                   ,A.ReserveKBN
                                   ,A.Number
                                   ,A.NumberRows
                                   ,A.StockNO
                                   ,B.SoukoCD
                                   ,B.JanCD
                                   ,B.SKUCD
                                   ,B.AdminNO
                                   ,A.ReserveSu
                                   ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                                   THEN NULL
                                   ELSE @SalesDate END)       --[ShippingPossibleDate]
                                   ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                                   THEN 0
                                   ELSE A.ReserveSu END)       --[ShippingPossibleSU]
                                   ,NULL    --[ShippingOrderNO]
                                   ,0       --[ShippingOrderRows]
                                   ,NULL    --[CompletedPickingNO]
                                   ,0       --[CompletedPickingRow]
                                   ,@SalesDate    --[CompletedPickingDate]
                                   ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                                   THEN 0
                                   ELSE A.ReserveSu END)       --[ShippingSu]
                                   ,0   --[ReturnKBN]
                                   ,0   --[OriginalReserveNO]
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,NULL    --[DeleteOperator]
                                   ,NULL    --[DeleteDateTime]
                            
                            FROM D_TemporaryReserve A
                            LEFT OUTER JOIN D_Stock B ON B.StockNO = A.StockNO
                            WHERE  A.KeySeq = @KeySeq
                            ;
                            -- ========= ループ内の実際の処理 ここまで===

                            --次の行のデータを取得して変数へ値をセット
                            FETCH NEXT FROM CUR_Tem
                            INTO  @KeySeq;

                        END
                        
                        --カーソルを閉じる
                        CLOSE CUR_Tem;
                        DEALLOCATE CUR_Tem;
                 	END
                    
                    --Parameter受取.mode＝２返品の場合
                    ELSE
                    BEGIN
                    	--StockSu が多い順、在庫番号順の最初の１レコードを探す。
                        DECLARE CUR_Stock CURSOR FOR 
                            SELECT DS.StockNO, DM.SalesSu
                                FROM D_Stock AS DS
                                INNER JOIN D_SalesDetailsTran AS DM
                                ON DM.DataNo = @DataNo
                                AND DM.DataRows = @DataRows
                                AND DM.DeleteDateTime IS NULL
                                INNER JOIN D_Juchuu AS DJ
                                ON DJ.JuchuuNO = DM.JuchuuNO
                                --D_Juchuu.DeleteDateTimeは条件に含めない
                                WHERE DS.DeleteDateTime IS NULL
                                AND DS.SoukoCD = DJ.SoukoCD
                                AND DS.AdminNO = DM.AdminNO
                                AND DS.ArrivalYetFLG = 0
                                ORDER BY DS.StockSu desc    
                                ;
                        
                        SET @StockNO = '';
                        
                        --カーソルオープン
                        OPEN CUR_Stock;

                        --最初の1行目を取得して変数へ値をセット
                        FETCH NEXT FROM CUR_Stock
                        INTO  @StockNO, @StockSu;
                        
                        --データの行数分ループ処理を実行する
                        IF @@FETCH_STATUS = 0
                        BEGIN
                            --テーブル転送仕様２－③ Update or insert       在庫  
                            UPDATE [D_Stock] SET
                                   [StockSu] = [StockSu] - @StockSu
                                  ,[AllowableSu] = [AllowableSu] - @StockSu
                                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @StockSu
                                  ,[ShippingSu] = [ShippingSu] + @StockSu
                                  ,[UpdateOperator] =  @Operator  
                                  ,[UpdateDateTime] =  @SYSDATETIME
                            WHERE StockNO = @StockNO
                            ;
                          
                        END
                        
                        --カーソルを閉じる
                        CLOSE CUR_Stock;
                        DEALLOCATE CUR_Stock;
                        
                        IF @StockNO = ''
                        BEGIN
                            --伝票番号採番●ToStockNO
                            EXEC Fnc_GetNumber
                                21,        --in伝票種別 21
                                @SalesDate_VAR, --in基準日
                                @StoreCD,  --in店舗CD
                                @Operator,
                                @StockNO OUTPUT
                                ;
                            
                            IF ISNULL(@StockNO,'') = ''
                            BEGIN
                                SET @W_ERR = 1;
                                RETURN @W_ERR;
                            END
                            
                            --【D_Stock】Insert    
                            INSERT INTO [D_Stock]
                                   ([StockNO]
                                   ,[SoukoCD]
                                   ,[RackNO]
                                   ,[ArrivalPlanNO]
                                   ,[SKUCD]
                                   ,[AdminNO]
                                   ,[JanCD]
                                   ,[ArrivalYetFLG]
                                   ,[ArrivalPlanKBN]
                                   ,[ArrivalPlanDate]
                                   ,[ArrivalDate]
                                   ,[StockSu]
                                   ,[PlanSu]
                                   ,[AllowableSu]
                                   ,[AnotherStoreAllowableSu]
                                   ,[ReserveSu]
                                   ,[InstructionSu]
                                   ,[ShippingSu]
                                   ,[OriginalStockNO]
                                   ,[ExpectReturnDate]
                                   ,[ReturnDate]
                                   ,[ReturnSu]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime]
                                   ,[DeleteOperator]
                                   ,[DeleteDateTime])
                            SELECT
                                    @StockNO
                                   ,@SoukoCD
                                   ,(SELECT top 1 TanaCD
                                        FROM M_Location AS M
                                        WHERE M.SoukoCD = @SoukoCD
                                        AND M.MainFlg = 1
                                        AND M.DeleteFlg = 0
                                        AND M.ChangeDate <= @SalesDate
                                        ORDER BY M.ChangeDate desc
                                        )   --RackNO
                                   ,NULL   --ArrivalPlanNO
                                   ,DM.SKUCD
                                   ,DM.AdminNO
                                   ,DM.JanCD
                                   ,0   --ArrivalYetFLG(0:入荷済、1:未入荷)
                                   ,0   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                                   ,NULL    --ArrivalPlanDate
                                   ,@SYSDATE    --ArrivalDate
                                   ,DM.SalesSu  --StockSu
                                   ,0   --PlanSu
                                   ,DM.SalesSu   --AllowableSu
                                   ,DM.SalesSu   --AnotherStoreAllowableSu
                                   ,0    --ReserveSu
                                   ,0   --InstructionSu
                                   ,0   --ShippingSu
                                   ,NULL    --OriginalStockNO
                                   ,NULL    --ExpectReturnDate
                                   ,NULL    --ReturnDate
                                   ,0   --ReturnSu
                             
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,NULL                  
                                   ,NULL
                                FROM D_SalesDetailsTran AS DM
                                WHERE DM.DataNo = @DataNo
                                AND DM.DataRows = @DataRows
                                AND DM.DeleteDateTime IS NULL
                                ;
                        END
                        
                        --テーブル転送仕様３－③ insert   入出庫履歴     D_Warehousing 
                        --【D_Warehousing】
                        INSERT INTO [D_Warehousing]
                           ([WarehousingDate]
                           ,[SoukoCD]
                           ,[RackNO]
                           ,[StockNO]
                           ,[JanCD]
                           ,[AdminNO]
                           ,[SKUCD]
                           ,[WarehousingKBN]
                           ,[DeleteFlg]
                           ,[Number]
                           ,[NumberRow]
                           ,[VendorCD]
                           ,[ToStoreCD]
                           ,[ToSoukoCD]
                           ,[ToRackNO]
                           ,[ToStockNO]
                           ,[FromStoreCD]
                           ,[FromSoukoCD]
                           ,[FromRackNO]
                           ,[CustomerCD]
                           ,[Quantity]
                           ,[UnitPrice]
                           ,[Amount]
                           ,[Program]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                        SELECT @SalesDate --WarehousingDate
                           ,(SELECT DS.SoukoCD FROM D_Stock AS DS WHERE DS.StockNO = @StockNO)
                           ,(SELECT DS.RackNO FROM D_Stock AS DS WHERE DS.StockNO = @StockNO)    --RackNO
                           ,@StockNO
                           ,DM.JanCD
                           ,DM.AdminNO
                           ,DM.SKUCD
                           ,23   --WarehousingKBN 23：売上返品
                           ,0  --DeleteFlg
                           ,DM.SalesNO  --Number
                           ,DM.SalesRows --NumberRow
                           ,NULL    --VendorCD
                           ,NULL --ToStoreCD
                           ,NULL --ToSoukoCD
                           ,NULL--ToRackNO
                           ,NULL    --ToStockNO
                           ,NULL    --FromStoreCD
                           ,NULL    --FromSoukoCD
                           ,NULL    --FromRackNO
                           ,DH.CustomerCD    --CustomerCD
                           ,(-1)*DM.SalesSU   --Quantity
                           ,0	--UnitPrice
                           ,0	--Amount
                           ,@Program  --Program
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL
                           ,NULL
                        FROM D_SalesDetailsTran AS DM
                        INNER JOIN D_SalesTran AS DH
                        ON DH.DataNo = DM.DataNo
                        LEFT OUTER JOIN D_Juchuu AS DJ
                        ON DJ.JuchuuNO = DM.JuchuuNO
                        --D_Juchuu.DeleteDateTimeは条件に含めない
                        WHERE DM.DataNo = @DataNo
                        AND DM.DataRows = @DataRows
                        AND DM.DeleteDateTime IS NULL
                        ;
                	END
                END
                
    			SET @COUNT = @COUNT + 1;
    			
                --以下、在庫管理する・しないに関わらず
                --↓①②③④で複数の条件満たす場合あり
                --①掛売の場合
                --掛額      ≠ 0 （ D_SalesTran.CreditAmount≠ 0 ）   ←請求は締請求なので、ここでは回収予定だけ作成する。                
                IF @CreditAmount <> 0
                BEGIN
                    --回収日再計算
                    EXEC Fnc_PlanDate_SP
                        0,             --0:回収,1:支払
                        @CustomerCD,   --in顧客CD
                        @SalesDate_VAR,    --in基準日
                        0,              --帳端区分
                        @Yoteibi OUTPUT
                        ;
                    SET @FirstCollectPlanDate = CONVERT(date, @Yoteibi);
                    
                    --全額が掛の場合       （ D_SalesTran.CreditAmount＝ D_SalesTran.BillingAmount     ）         
                    IF @CreditAmount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN                
                            -- テーブル転送仕様４－①－a Insert   回収予定　D_CollectPlan 
                            EXEC INSERT_D_CollectPlan_4_1
                                @DataNo 
                                ,0  --Datatype
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ;
                        END
                                   
                        --売上明細ごとに回収予定明細を作成  
                        -- テーブル転送仕様５－①－a Insert   回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	1
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@FirstCollectPlanDate
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,NULL
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                        
                    END                                      
                    --一部が掛の場合       （ D_SalesTran.CreditAmount≠ D_SalesTran.BillingAmount     ）         
                    ELSE
                    BEGIN
                        --回収予定明細は１件だけ作成
                        --（売上明細１件目で作成）"    
                        IF @COUNT = 1
                        BEGIN
                        	-- テーブル転送仕様４－①－b Insert   回収予定　D_CollectPlan 
                            EXEC INSERT_D_CollectPlan_4_1
                                @DataNo 
                                ,1	--Datatype
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                            	,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ;

	                        -- テーブル転送仕様５－①－b Insert   回収予定明細　D_CollectPlanDetails
	                        --一部の場合は掛で１レコードのみ
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                            	1	--KBN
                                ,@DataNo 
                                ,@DataRows
                                ,1
                                ,@Operator
                                ,@SYSDATETIME
                            	,@FirstCollectPlanDate
                                ,@CollectPlanNO
                            	,@CollectPlanRows OUTPUT
                                ,NULL
                                ,@CollectNO
	                            ,@ConfirmNO OUTPUT
	                            ,@W_ERR OUTPUT
                                ;
                        END
                    END
                END
                
                --現金の場合
                --現金額≠0（D_SalesTran.CashAmount≠0）
                IF @CashAmount <> 0
                BEGIN
                    --全額が現金の場合（D_SalesTran.CashAmount＝D_SalesTran.BillingAmount）
                    IF @CashAmount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－②－a　Insert回収予定　D_CollectPlan 
                            --売上明細ごとに回収予定明細を作成
                            EXEC INSERT_D_CollectPlan_4_2_6
                            	2
                                ,@DataNo 
                                ,0
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                ,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                            
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                        --テーブル転送仕様５－②－a　Insert回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	2	--KBN
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@SYSDATE
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,@BillingNO
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                            
                        IF @W_ERR = 1
                        BEGIN 
                            RETURN @W_ERR;
                        END
                    END
                    
                    --一部が現金の場合（D_SalesTran.CashAmount≠D_SalesTran.BillingAmount）
                    ELSE
                    BEGIN
                        --回収予定明細は１件だけ作成
                        --（売上明細１件目で作成）
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－②－b　Insert回収予定　D_CollectPlan
                            EXEC INSERT_D_CollectPlan_4_2_6
                            	2
                                ,@DataNo 
                                ,1
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                ,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                            
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                            
                            --テーブル転送仕様５－②－bInsert回収予定明細　D_CollectPlanDetails
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                                2
                                ,@DataNo 
                                ,@DataRows
                                ,0
                                ,@Operator
                                ,@SYSDATETIME
                                ,@SYSDATE
                                ,@CollectPlanNO
                                ,@CollectPlanRows OUTPUT
                                ,@BillingNO
                                ,@CollectNO
                                ,@ConfirmNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END                    
                    END                

                    IF @COUNT = 1
                    BEGIN
                        --テーブル転送仕様10－①Insert入金消込請求D_CollectBilling
                        EXEC INSERT_D_CollectBilling_10_1
                            @ConfirmNO
                            ,@CollectPlanNO
                            ,@Operator
                            ,@SYSDATETIME
                            ;
                        
                   END
                    
                    --テーブル転送仕様11－①Insert入金消込明細D_CollectBillingDetails
                    EXEC INSERT_D_CollectBilling_11_1
                        @ConfirmNO
                        ,@CollectPlanNO
                        ,@CollectPlanRows
                        ,@Operator
                        ,@SYSDATETIME
                        ;
                            
                END	--現金の場合
                
                --③カード系の場合
                IF @CardAmount <> 0	--カード額≠0（	D_SalesTran.CardAmount≠0）
                BEGIN
                    SET @BillingCD = (SELECT top 1 A.BillingCD 
                                        FROM D_SalesTran AS DH 
                                        INNER JOIN D_StorePayment AS DS
                                        ON DS.SalesNO = DH.SalesNO
                                        AND DS.StoreCD = DH.StoreCD
                                        INNER JOIN M_DenominationKBN AS A
                                        ON A.DenominationCD = DS.CardDenominationCD
                                        WHERE DH.DataNo = @DataNo
                                        ORDER BY DS.SalesNORows DESC
                                    );

                    --回収日再計算
                    EXEC Fnc_PlanDate_SP
                        0,             --0:回収,1:支払
                        @BillingCD,    --in顧客CD 請求先CD
                        @SYSDATE_VAR,  --in基準日
                        0,              --帳端区分
                        @Yoteibi OUTPUT
                        ;
                        
                    SET @FirstCollectPlanDate = CONVERT(date, @Yoteibi);
                    
                	--全額がカードの場合（D_SalesTran.CardAmount＝D_SalesTran.BillingAmount）
                    IF @CardAmount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－③－a　Insert回収予定　D_CollectPlan                       
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	3
                                ,@DataNo 
                                ,0
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                        
                        --売上明細ごとに回収予定明細を作成
                        --テーブル転送仕様５－③－a　Insert回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	3	--KBN カード
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@FirstCollectPlanDate
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,@BillingNO
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                            
                        IF @W_ERR = 1
                        BEGIN 
                            RETURN @W_ERR;
                        END
                    END
                    
                    --一部がカードの場合（D_SalesTran.CardAmount≠D_SalesTran.BillingAmount）
                    ELSE
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－③－b　Insert回収予定　D_CollectPlan
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	3
                                ,@DataNo 
                                ,1
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                            
                            --回収予定明細は１件だけ作成
                            --（売上明細１件目で作成）
                            --テーブル転送仕様５－③－b　Insert回収予定明細　D_CollectPlanDetails
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                                3   --KBN カード
                                ,@DataNo 
                                ,@DataRows
                                ,1
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@CollectPlanNO
                                ,@CollectPlanRows OUTPUT
                                ,@BillingNO
                                ,@CollectNO
                                ,@ConfirmNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                    END
                END		--③カード系の場合
                
                --その他入金１額≠0（D_SalesTran.Denomination1Amount
                IF @Denomination1Amount <> 0
                BEGIN
                    SET @BillingCD = (SELECT top 1 A.BillingCD 
                                        FROM D_SalesTran AS DH 
                                        INNER JOIN D_StorePayment AS DS
                                        ON DS.SalesNO = DH.SalesNO
                                        AND DS.StoreCD = DH.StoreCD
                                        INNER JOIN M_DenominationKBN AS A
                                        ON A.DenominationCD = DS.Denomination1CD
                                        WHERE DH.DataNo = @DataNo
                                        ORDER BY DS.SalesNORows DESC
                                    );

                    --回収日再計算
                    EXEC Fnc_PlanDate_SP
                        0,             --0:回収,1:支払
                        @BillingCD,    --in顧客CD 請求先CD
                        @SYSDATE_VAR,  --in基準日
                        0,              --帳端区分
                        @Yoteibi OUTPUT
                        ;
                        
                    SET @FirstCollectPlanDate = CONVERT(date, @Yoteibi);
                    
                	--全額がその他入金１の場合（D_SalesTran.Denomination1Amount＝D_SalesTran.BillingAmount）
                    IF @Denomination1Amount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－④－a　Insert回収予定　D_CollectPlan                       
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	4
                                ,@DataNo 
                                ,0
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                        
                        --売上明細ごとに回収予定明細を作成
                        --テーブル転送仕様５－④－a　Insert回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	4	--KBN その他入金１
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@FirstCollectPlanDate
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,@BillingNO
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                            
                        IF @W_ERR = 1
                        BEGIN 
                            RETURN @W_ERR;
                        END
                    END
                    
                    --一部がその他入金１の場合（D_SalesTran.Denomination1Amount	z≠D_SalesTran.BillingAmount）
                    ELSE
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－④－b　Insert回収予定　D_CollectPlan
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	4
                                ,@DataNo 
                                ,1
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                            
                            --回収予定明細は１件だけ作成
                            --（売上明細１件目で作成）
                            --テーブル転送仕様５－④－b　Insert回収予定明細　D_CollectPlanDetails
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                                4   --KBN その他入金１
                                ,@DataNo 
                                ,@DataRows
                                ,1
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@CollectPlanNO
                                ,@CollectPlanRows OUTPUT
                                ,@BillingNO
                                ,@CollectNO
                                ,@ConfirmNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                    END
                END		--その他入金１の場合
                
                --その他入金２額≠0（D_SalesTran.Denomination2Amount
                IF @Denomination2Amount <> 0	
                BEGIN
                    SET @BillingCD = (SELECT top 1 A.BillingCD 
                                        FROM D_SalesTran AS DH 
                                        INNER JOIN D_StorePayment AS DS
                                        ON DS.SalesNO = DH.SalesNO
                                        AND DS.StoreCD = DH.StoreCD
                                        INNER JOIN M_DenominationKBN AS A
                                        ON A.DenominationCD = DS.Denomination2CD
                                        WHERE DH.DataNo = @DataNo
                                        ORDER BY DS.SalesNORows DESC
                                    );

                    --回収日再計算
                    EXEC Fnc_PlanDate_SP
                        0,             --0:回収,1:支払
                        @BillingCD,    --in顧客CD 請求先CD
                        @SYSDATE_VAR,  --in基準日
                        0,              --帳端区分
                        @Yoteibi OUTPUT
                        ;
                        
                    SET @FirstCollectPlanDate = CONVERT(date, @Yoteibi);
                    
                	--全額がその他入金２の場合（D_SalesTran.Denomination2Amount＝D_SalesTran.BillingAmount）
                    IF @Denomination2Amount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－④－a　Insert回収予定　D_CollectPlan                       
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	5
                                ,@DataNo 
                                ,0
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                        
                        --売上明細ごとに回収予定明細を作成
                        --テーブル転送仕様５－④－a　Insert回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	5	--KBN その他入金２
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@FirstCollectPlanDate
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,@BillingNO
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                            
                        IF @W_ERR = 1
                        BEGIN 
                            RETURN @W_ERR;
                        END
                    END
                    
                    --一部がその他入金１の場合（D_SalesTran.Denomination2Amount	z≠D_SalesTran.BillingAmount）
                    ELSE
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－④－b　Insert回収予定　D_CollectPlan
                            EXEC INSERT_D_CollectPlan_4_3_4_5
                            	5
                                ,@DataNo 
                                ,1
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                --,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                            
                            --回収予定明細は１件だけ作成
                            --（売上明細１件目で作成）
                            --テーブル転送仕様５－④－b　Insert回収予定明細　D_CollectPlanDetails
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                                5   --KBN その他入金２
                                ,@DataNo 
                                ,@DataRows
                                ,1
                                ,@Operator
                                ,@SYSDATETIME
                                ,@FirstCollectPlanDate
                                ,@CollectPlanNO
                                ,@CollectPlanRows OUTPUT
                                ,@BillingNO
                                ,@CollectNO
                                ,@ConfirmNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                    END
                END		--その他入金２の場合
                
                --④前受金の場合（野球のチーム監督などが「先に払っておいた金額」を取り崩しする場合など）
                --前受金≠0（D_SalesTran.AdvanceAmount≠0）
                IF @AdvanceAmount <> 0	
                BEGIN
                	--全額が前受金の場合（D_SalesTran.AdvanceAmount＝D_SalesTran.BillingAmount）
                    IF @AdvanceAmount = @BillingAmount
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－⑥－a　Insert回収予定　D_CollectPlan                     
                            EXEC INSERT_D_CollectPlan_4_2_6
                            	6
                                ,@DataNo 
                                ,0
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                ,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                            
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                        
                        --売上明細ごとに回収予定明細を作成
                        --テーブル転送仕様５－⑥－a　Insert回収予定明細　D_CollectPlanDetails
                        EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                        	6	--KBN 前受金
                            ,@DataNo 
                            ,@DataRows
                            ,0
                            ,@Operator
                            ,@SYSDATETIME
                            ,@SYSDATE
                            ,@CollectPlanNO
                            ,@CollectPlanRows OUTPUT
                            ,@BillingNO
                            ,@CollectNO
                            ,@ConfirmNO OUTPUT
                            ,@W_ERR OUTPUT
                            ;
                            
                        IF @W_ERR = 1
                        BEGIN 
                            RETURN @W_ERR;
                        END
                    END
                    
                    --一部が前受金の場合（D_SalesTran.AdvanceAmount≠D_SalesTran.BillingAmount）
                    ELSE
                    BEGIN
                        IF @COUNT = 1
                        BEGIN
                            --テーブル転送仕様４－⑥－b　Insert回収予定　D_CollectPlan
                            EXEC INSERT_D_CollectPlan_4_2_6
                            	6
                                ,@DataNo 
                                ,1
                                ,@Program
                                ,@Operator
                                ,@SYSDATETIME
                                ,@JuchuuNO
                                ,@CollectPlanNO OUTPUT
                                ,@BillingNO OUTPUT
                                ,@CollectNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                            
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                            
                            --回収予定明細は１件だけ作成
                            --（売上明細１件目で作成）
                            --テーブル転送仕様５－⑥－b　Insert回収予定明細　D_CollectPlanDetails
                            EXEC INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6
                                6   --KBN 前受金
                                ,@DataNo 
                                ,@DataRows
                                ,1
                                ,@Operator
                                ,@SYSDATETIME
                                ,@SYSDATE
                                ,@CollectPlanNO
                                ,@CollectPlanRows OUTPUT
                                ,@BillingNO
                                ,@CollectNO
                                ,@ConfirmNO OUTPUT
                                ,@W_ERR OUTPUT
                                ;
                                
                            IF @W_ERR = 1
                            BEGIN 
                                RETURN @W_ERR;
                            END
                        END
                    END
                    
                    IF @COUNT = 1
                    BEGIN
                        --テーブル転送仕様10－①Insert入金消込請求D_CollectBilling
                        EXEC INSERT_D_CollectBilling_10_1
                            @ConfirmNO
                            ,@CollectPlanNO
                            ,@Operator
                            ,@SYSDATETIME
                            ;
                    
                        --テーブル転送仕様12－①Update入金D_Collect 以前の前受金分の入金を更新
                        --入金　D_Collect（前受金）前受金の取り崩し 
                        UPDATE [D_Collect] SET
                            [ConfirmAmount] = [ConfirmAmount] + DS.AdvanceAmount
                            ,[UpdateOperator]     =  @Operator  
                            ,[UpdateDateTime]     =  @SYSDATETIME
                        FROM D_StoreAdvance AS DS                                       
                        WHERE DS.SalesNO = @SalesNO
                        AND D_Collect.CollectNO = DS.CollectNO
                        AND D_Collect.DeleteDateTime IS NULL
                        ;
                    END
                    
                    --テーブル転送仕様11－①Insert入金消込明細D_CollectBillingDetails
                    EXEC INSERT_D_CollectBilling_11_1
                        @ConfirmNO
                        ,@CollectPlanNO
                        ,@CollectPlanRows
                        ,@Operator
                        ,@SYSDATETIME
                        ;

                    
                END		--前受金の場合

            END
        END		--2.D_SalesTran.RecoredKBN＝0（黒）の場合

        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO  @DataNo,@DataRows,@RecoredKBN,@JuchuuNO, @JuchuuRows, @AdminNO, @SalesDate, @CustomerCD
            ,@DiscountAmount 
            ,@BillingAmount 
            ,@PointAmount 
            --,@CardDenominationCD, varchar(3),;
            ,@CardAmount 
            ,@CashAmount 
            ,@DepositAmount 
            ,@RefundAmount 
            ,@CreditAmount 
            --,@Denomination1CD, varchar(3),;
            ,@Denomination1Amount 
            --,@Denomination2CD, varchar(3),;
            ,@Denomination2Amount 
            ,@AdvanceAmount
            ,@SoukoCD
            ,@SalesSU
        	,@SalesDate_VAR
            ;
    END
    
    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
    
    --１：通常⇒普通の販売
    --          あらたに売上、黒伝票

    --２：返品⇒マイナス販売、元の伝票とは無関係
    --          新たにマイナス売上、黒伝票（赤伝票でない）

    --３：訂正⇒販売の変更、元の伝票に対する変更    
    --          元の伝票の赤、黒伝票

    --４：取消⇒販売を無かったことに。元の伝票に対する削除      
    --          元の伝票の赤伝票    

	--全ての場合
	UPDATE D_SalesTran SET
		StoreSalesUpdateFLG = 1
		,StoreSalesUpdateTime = @SYSDATETIME
	WHERE StoreCD = @StoreCD
	AND SalesNO = @SalesNO
	AND StoreSalesUpdateFLG = 0
	;
	                
    --処理履歴データへ更新
    IF @OperateMode = @MODE_NORMAL
    BEGIN
    	SET @OperateModeNm = '通常';
    END
    ELSE IF @OperateMode = @MODE_HENPIN
    BEGIN
    	SET @OperateModeNm = '返品';
    END
    ELSE IF @OperateMode = @MODE_TEISEI
    BEGIN
    	SET @OperateModeNm = '訂正';
    END
    ELSE IF @OperateMode = @MODE_CANCEL
    BEGIN
    	SET @OperateModeNm = '取消';
    END
    
    SET @KeyItem = @SalesNO;
        
    --Table転送仕様Ｚ InsertL_Log 
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutCollectPlanNO = CONVERT(varchar,@CollectPlanNO);
    
--<<OWARI>>
  return @W_ERR;

END



