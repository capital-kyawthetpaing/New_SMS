DROP  PROCEDURE [dbo].[D_TenzikaiJuchuu_SelectAll]
GO
DROP  PROCEDURE [dbo].[CheckTenzikaiJuchuu]
GO

DROP  PROCEDURE [dbo].[PRC_TenzikaiJuchuuJouhouHurikaeShori]
GO


--  ======================================================================
--       Program Call    展示会受注情報振替処理
--       Program ID      TenzikaiJuchuuJouhouHurikaeShori
--       Create date:    2020.9.10
--    ======================================================================
CREATE PROCEDURE D_TenzikaiJuchuu_SelectAll
    (
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.TenzikaiJuchuuNO
    FROM D_TenzikaiJuchuu AS A
    LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS B
    ON B.TenzikaiJuchuuNO = A.TenzikaiJuchuuNO
    AND B.DeleteDateTime IS NULL
    WHERE A.DeleteDateTime IS NULL
    --一時ワークテーブル「M_SKU①」(一時ワークテーブル「D_TaishougaiJuchuu①」、テーブル転送仕様Ｂで「M_SKU①」として使用)	
    AND NOT EXISTS(SELECT MT.JanCD AS TenzikaiShouhinJanCD
                         ,F.AdminNO
                         ,F.SKUCD
                         ,F.JanCD
                         ,F.SKUName
                         ,F.ColorName
                         ,F.SizeName
                   FROM F_SKU(convert(date,SYSDATETIME())) AS F
                   --一時ワークテーブル「M_TenzikaiShouhin①」(一時ワークテーブル「M_SKU①」で「M_TenzikaiShouhin①」として使用)
                   INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                     ,MAX(MTS.JanCD) AS JanCD 
                               FROM M_TenzikaiShouhin AS MTS
                               WHERE MTS.DeleteFLG = 0
                               AND MTS.VendorCD = @VendorCD
                               AND MTS.LastYearTerm = @LastYearTerm
                               AND MTS.LastSeason = @LastSeason
                               GROUP BY MTS.ExhibitionCommonCD
                               ) AS MT 
                   ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                   WHERE F.DeleteFlg = 0
                   AND MT.JanCD = B.JanCD)
    AND A.JuchuuHurikaeZumiFLG = 0
    AND A.VendorCD = @VendorCD
    AND A.LastYearTerm = @LastYearTerm
    AND A.LastSeason = @LastSeason
    GROUP BY A.TenzikaiJuchuuNO
    ORDER BY A.TenzikaiJuchuuNO
    ;

END

GO

CREATE PROCEDURE CheckTenzikaiJuchuu
    (    
    @TorokuDate     varchar(10),
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    DECLARE @JuchuuNO varchar(11);
    
    SET @ERRNO = '';
    
        --該当の展示会受注番号を取得(Obtain the relevant "展示会受注番号")
        --(Table_TenzikaiJuchuuNO)
/*        SELECT DT.TenzikaiJuchuuNO                                                                                 
        From D_TenzikaiJuchuu AS DT                                                                                 
        Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
        AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
        AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
        AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
        ;
        */
        
	--カーソル定義
    DECLARE CUR_Juchu CURSOR FOR
        --該当の展示会受注番号に紐づく受注番号を取得(Obtain "受注番号" associated with the corresponding "展示会受注番号")
        --(Table_JuchuuNO)
        SELECT A.JuchuuNO
        From D_Juchuu AS A
        --該当の展示会受注番号を取得(Obtain the relevant "展示会受注番号")
        --(Table_TenzikaiJuchuuNO)
        Inner Join (SELECT DT.TenzikaiJuchuuNO                                                                                 
                    From D_TenzikaiJuchuu AS DT                                                                                 
                    Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
                    AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
                    AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
                    AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
                    AND DT.DeleteDateTime IS NULL
        )   --Table_TenzikaiJuchuuNO 
        AS B
        On A.TenzikaiJuchuuNO = B.TenzikaiJuchuuNO
        Where A.DeleteDateTime IS NULL
        ;

    --カーソルオープン
    OPEN CUR_Juchu;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===

        --既に売上済み警告
        --以下の条件でレコードがあれば売上済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_SalesDetails A
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E168';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        --既に出荷済み警告
        --以下の条件でレコードがあれば出荷済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.Number)
        FROM D_ShippingDetails A
        WHERE A.Number = @JuchuuNO
        AND A.ShippingKBN = 1
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E167';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --既にピッキングリスト完了済み警告
        --以下の条件でレコードがあればピッキング済としてエラーメッセージを表示する
        SELECT @CNT = COUNT(A.Number)
        FROM D_PickingDetails B
        INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
        WHERE A.Number = @JuchuuNO
        AND A.ReserveKBN = 1
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND B.PickingDoneDateTime IS NOT NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E161';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        --既に仕入済み警告
        --以下の条件でレコードがあれば発注済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_PurchaseDetails D
        INNER JOIN D_ArrivalDetails C ON C.ArrivalNO = D.ArrivalNO
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        AND D.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E164';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --既に入荷済み警告
        --以下の条件でレコードがあれば発注済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_ArrivalDetails C
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E163';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --既に発注済み警告
        --以下の条件でレコードがあれば発注済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_OrderDetails A 
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E162';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        -- ========= ループ内の実際の処理 ここまで===
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO;

    END
    
    --カーソルを閉じる
    CLOSE CUR_Juchu;
    DEALLOCATE CUR_Juchu;
    
    SELECT @ERRNO AS errno;

END

GO

CREATE PROCEDURE PRC_TenzikaiJuchuuJouhouHurikaeShori
   (@OperateMode    int,         -- 処理区分（1:新規 2:修正 3:削除）
    
    @TorokuDate     varchar(10),
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6),

    @Operator  varchar(10),
    @PC  varchar(30)
--    @OutJuchuuProcessNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @DeliveryPlanNO  varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    DECLARE @JuchuuProcessNO     varchar(11);
    DECLARE @JuchuuNO            varchar(11);
    DECLARE @StoreCD             varchar(4);
    DECLARE @TenzikaiJuchuuNO    varchar(11);
    DECLARE @JuchuuDate          date;
    DECLARE @varJuchuuDate       varchar(10);
    DECLARE @TenzikaiJuchuuRows  int;
    DECLARE @oldTenzikaiJuchuuNO varchar(11);
    DECLARE @DisplayRows         int;
    
    SET @StoreCD = '0000';
    SET @oldTenzikaiJuchuuNO = '';
    
    --カーソル定義
    DECLARE CUR_Tenzi CURSOR FOR
        --一時ワークテーブル「D_TenzikaiJuchuu①」(テーブル転送仕様Ａで「D_TenzikaiJuchuu①」として使用)
        SELECT
             DT.TenzikaiJuchuuNO    
            ,DT.JuchuuDate
            ,DM.TenzikaiJuchuuRows
        FROM D_TenzikaiJuchuu AS DT
        LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS DM
        ON DM.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
        AND DM.DeleteDateTime IS NULL
        WHERE DT.DeleteDateTime IS NULL
        AND DT.JuchuuHurikaeZumiFLG = 0
        AND DT.VendorCD = @VendorCD
        AND DT.LastYearTerm = @LastYearTerm
        AND DT.LastSeason = @LastSeason
        --一時ワークテーブル「D_TaishougaiJuchuu①」(一時ワークテーブル「D_TenzikaiJuchuu①」、画面転送表01で「D_TaishougaiJuchuu①」として使用)
        AND NOT EXISTS(SELECT 1 
                FROM D_TenzikaiJuchuu AS A
                LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS B
                ON B.TenzikaiJuchuuNO = A.TenzikaiJuchuuNO
                AND B.DeleteDateTime IS NULL
                WHERE A.DeleteDateTime IS NULL
                --一時ワークテーブル「M_SKU①」(一時ワークテーブル「D_TaishougaiJuchuu①」、テーブル転送仕様Ｂで「M_SKU①」として使用)	
                AND NOT EXISTS(SELECT MT.JanCD AS TenzikaiShouhinJanCD
                                     ,F.AdminNO
                                     ,F.SKUCD
                                     ,F.JanCD
                                     ,F.SKUName
                                     ,F.ColorName
                                     ,F.SizeName
                               FROM F_SKU(convert(date,@SYSDATETIME)) AS F
                               --一時ワークテーブル「M_TenzikaiShouhin①」(一時ワークテーブル「M_SKU①」で「M_TenzikaiShouhin①」として使用)
                               INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                                 ,MAX(MTS.JanCD) AS JanCD 
                                           FROM M_TenzikaiShouhin AS MTS
                                           WHERE MTS.DeleteFLG = 0
                                           AND MTS.VendorCD = @VendorCD
                                           AND MTS.LastYearTerm = @LastYearTerm
                                           AND MTS.LastSeason = @LastSeason
                                           GROUP BY MTS.ExhibitionCommonCD
                                           ) AS MT 
                               ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                               WHERE F.DeleteFlg = 0
                               AND MT.JanCD = B.JanCD)
                AND A.JuchuuHurikaeZumiFLG = 0
                AND A.VendorCD = @VendorCD
                AND A.LastYearTerm = @LastYearTerm
                AND A.LastSeason = @LastSeason
                AND A.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
        )
        ORDER BY DT.TenzikaiJuchuuNO, DM.TenzikaiJuchuuRows
        ;

    
    --新規--
    IF @OperateMode = 1
    BEGIN
        --カーソルオープン
        OPEN CUR_Tenzi;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_Tenzi
        INTO @TenzikaiJuchuuNO, @JuchuuDate, @TenzikaiJuchuuRows;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            --※展示会受注番号単位で採番
            SET @varJuchuuDate = convert(varchar, @JuchuuDate,111);
            
            IF @oldTenzikaiJuchuuNO <> @TenzikaiJuchuuNO
            BEGIN
                SET @oldTenzikaiJuchuuNO = @TenzikaiJuchuuNO;
                SET @DisplayRows = 1;
                
                --伝票番号採番
                EXEC Fnc_GetNumber
                    30,             --in伝票種別 1
                    @varJuchuuDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @JuchuuProcessNO OUTPUT
                    ;
                
                IF ISNULL(@JuchuuProcessNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
            END
            
            --※明細単位で採番
            --伝票番号採番
            EXEC Fnc_GetNumber
                1,             --in伝票種別 1
                @varJuchuuDate, --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @JuchuuNO OUTPUT
                ;
            
            IF ISNULL(@JuchuuNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_DeliveryPlan】配送予定情報　Table転送仕様G
            --伝票番号採番
            EXEC Fnc_GetNumber
                19,             --in伝票種別 19
                @varJuchuuDate,      --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @DeliveryPlanNO OUTPUT
                ;
            
            IF ISNULL(@DeliveryPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
                    
            --【D_Juchuu】
            INSERT INTO [D_Juchuu]
               ([JuchuuNO]
               ,[JuchuuProcessNO]
               ,[StoreCD]
               ,[JuchuuDate]
               ,[JuchuuTime]
               ,[ReturnFLG]
               ,[SoukoCD]
               ,[JuchuuKBN]
               ,[SiteKBN]
               ,[SiteJuchuuDateTime]
               ,[SiteJuchuuNO]
               ,[InportErrFLG]
               ,[OnHoldFLG]
               ,[IdentificationFLG]
               ,[TorikomiDateTime]
               ,[StaffCD]
               ,[CustomerCD]
               ,[CustomerName]
               ,[CustomerName2]
               ,[AliasKBN]
               ,[ZipCD1]
               ,[ZipCD2]
               ,[Address1]
               ,[Address2]
               ,[Tel11]
               ,[Tel12]
               ,[Tel13]
               ,[Tel21]
               ,[Tel22]
               ,[Tel23]
               ,[CustomerKanaName]
               ,[DeliveryCD]
               ,[DeliveryName]
               ,[DeliveryName2]
               ,[DeliveryAliasKBN]
               ,[DeliveryZipCD1]
               ,[DeliveryZipCD2]
               ,[DeliveryAddress1]
               ,[DeliveryAddress2]
               ,[DeliveryTel11]
               ,[DeliveryTel12]
               ,[DeliveryTel13]
               ,[DeliveryTel21]
               ,[DeliveryTel22]
               ,[DeliveryTel23]
               ,[DeliveryKanaName]
               ,[JuchuuCarrierCD]
               ,[DecidedCarrierFLG]
               ,[LastCarrierCD]
               ,[NameSortingDateTime]
               ,[NameSortingStaffCD]
               ,[CurrencyCD]
               ,[JuchuuGaku]
               ,[Discount]
               ,[HanbaiHontaiGaku]
               ,[HanbaiTax8]
               ,[HanbaiTax10]
               ,[HanbaiGaku]
               ,[CostGaku]
               ,[ProfitGaku]
               ,[Coupon]
               --,[Point]
               ,[PayCharge]
               ,[Adjustments]
               ,[Postage]
               ,[GiftWrapCharge]
               ,[InvoiceGaku]
               ,[PaymentMethodCD]
               ,[PaymentPlanNO]
               ,[CardProgressKBN]
               ,[CardCompany]
               ,[CardNumber]
               ,[PaymentProgressKBN]
               ,[PresentFLG]
               ,[SalesPlanDate]
               ,[FirstPaypentPlanDate]
               ,[LastPaymentPlanDate]
               ,[DemandProgressKBN]
               ,[CommentDemand]
               ,[CancelDate]
               ,[CancelReasonKBN]
               ,[CancelRemarks]
               ,[NoMailFLG]
               ,[IndividualContactKBN]
               ,[TelephoneContactKBN]
               ,[LastMailKBN]
               ,[LastMailPatternCD]
               ,[LastMailDatetime]
               ,[LastMailName]
               ,[NextMailKBN]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[LastDepositeDate]
               ,[LastOrderDate]
               ,[LastArriveDate]
               ,[LastSalesDate]
               ,[MitsumoriNO]
               ,[TenzikaiJuchuuNO]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[JuchuuDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @JuchuuNO
               ,@JuchuuProcessNO                      
               ,@StoreCD                          
               ,@JuchuuDate
               ,NULL    --JuchuuTime
               ,0 AS ReturnFLG
               ,DT.SoukoCD
               ,3   --JuchuuKBN
               ,0   --SiteKBN
               ,NULL    --SiteJuchuuDateTime
               ,NULL    --SiteJuchuuNO
               ,0   --InportErrFLG
               ,0   --OnHoldFLG
               ,0   --IdentificationFLG
               ,NULL    --TorikomiDateTime
               ,DT.StaffCD
               ,DT.CustomerCD
               ,DT.CustomerName
               ,DT.CustomerName2
               ,DT.AliasKBN
               ,DT.ZipCD1
               ,DT.ZipCD2
               ,DT.Address1
               ,DT.Address2
               ,DT.Tel11
               ,DT.Tel12
               ,DT.Tel13
               ,NULL    --Tel21
               ,NULL    --Tel22
               ,NULL    --Tel23
               ,NULL    --CustomerKanaName
               ,DT.DeliveryCD
               ,DT.DeliveryName
               ,DT.DeliveryName2
               ,DT.DeliveryAliasKBN
               ,DT.DeliveryZipCD1
               ,DT.DeliveryZipCD2
               ,DT.DeliveryAddress1
               ,DT.DeliveryAddress2
               ,DT.DeliveryTel11
               ,DT.DeliveryTel12
               ,DT.DeliveryTel13
               ,NULL    --Tel21
               ,NULL    --Tel22
               ,NULL    --Tel23
               ,NULL   --,[DeliveryKanaName]
               ,NULL    --JuchuuCarrierCD
               ,0   --DecidedCarrierFLG
               ,NULL    --LastCarrierCD
               ,NULL    --NameSortingDateTime
               ,NULL    --NameSortingStaffCD
               ,DT.CurrencyCD   --CurrencyCD
               ,DT.JuchuuGaku
               ,0 As Discount
               ,DT.HanbaiHontaiGaku
               ,DT.HanbaiTax8
               ,DT.HanbaiTax10
               ,DT.HanbaiGaku
               ,DT.CostGaku
               ,DT.ProfitGaku
               ,0   --Coupon
               --,@Point
               ,0   --PayCharge
               ,0   --Adjustments
               ,0   --Postage
               ,0   --GiftWrapCharge
               ,0   --InvoiceGaku
               ,DT.PaymentMethodCD
               ,0       --PaymentPlanNO
               ,0       --CardProgressKBN
               ,NULL    --CardCompany
               ,NULL    --CardNumber
               ,0   --PaymentProgressKBN
               ,0   --PresentFLG
               ,DT.SalesPlanDate
               ,NULL --FirstPaypentPlanDate
               ,NULL --LastPaymentPlanDate
               ,0   --DemandProgressKBN
               ,NULL    --CommentDemand
               ,NULL    --CancelDate
               ,NULL    --CancelReasonKBN
               ,NULL    --CancelRemarks
               ,0   --NoMailFLG
               ,0   --IndividualContactKBN
               ,0   --TelephoneContactKBN
               ,NULL    --LastMailKBN
               ,NULL    --LastMailPatternCD
               ,NULL    --LastMailDatetime
               ,NULL    --LastMailName
               ,NULL    --NextMailKBN
               ,NULL    --CommentOutStore
               ,NULL    --CommentInStore
               ,NULL    --LastDepositeDate
               ,NULL    --LastOrderDate
               ,NULL    --LastArriveDate
               ,NULL    --LastSalesDate
               ,NULL    --MitsumoriNO
               ,@TenzikaiJuchuuNO
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL                  
               ,NULL
           FROM D_TenzikaiJuchuu AS DT
           WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
           ;               

            --【D_JuchuuDetails】
            --Table転送仕様Ｂ
            INSERT INTO [D_JuchuuDetails]
                       ([JuchuuNO]
                       ,[JuchuuRows]
                       ,[DisplayRows]
                       ,[SiteJuchuuRows]
                       ,[NotPrintFLG]
                       ,[AddJuchuuRows]
                       ,[NotOrderFLG]
                       ,[ExpressFLG]
                       ,[AdminNO]
                       ,[SKUCD]
                       ,[JanCD]
                       ,[SKUName]
                       ,[ColorName]
                       ,[SizeName]
                       ,[SetKBN]
                       ,[SetRows]
                       ,[JuchuuSuu]
                       ,[JuchuuUnitPrice]
                       ,[TaniCD]
                       ,[JuchuuGaku]
                       ,[JuchuuHontaiGaku]
                       ,[JuchuuTax]
                       ,[JuchuuTaxRitsu]
                       ,[CostUnitPrice]
                       ,[CostGaku]
                       ,[OrderUnitPrice]
                       ,[ProfitGaku]
                       ,[SoukoCD]
                       ,[HikiateSu]
                       ,[DeliveryOrderSu]
                       ,[DeliverySu]
                       ,[DirectFLG]
                       ,[HikiateFLG]
                       ,[JuchuuOrderNO]
                       ,[VendorCD]
                       ,[LastOrderNO]
                       ,[LastOrderRows]
                       ,[LastOrderDateTime]
                       ,[DesiredDeliveryDate]
                       ,[AnswerFLG]
                       ,[ArrivePlanDate]
                       ,[ArrivePlanNO]
                       ,[ArriveDateTime]
                       ,[ArriveNO]
                       ,[ArribveNORows]
                       ,[DeliveryPlanNO]
                       ,[CommentOutStore]
                       ,[CommentInStore]
                       ,[IndividualClientName]
                       ,[ShippingPlanDate]
                       ,[SalesDate]
                       ,[SalesNO]
                       ,[DepositeDetailNO]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                 SELECT @JuchuuNO                         
                       ,1 AS JuchuuRows                     
                       ,@DisplayRows                      
                       ,0   --SiteJuchuuRows
                       ,0 AS NotPrintFLG
                       ,0 AS AddJuchuuRows
                       ,0 AS NotOrderFLG
                       ,0 AS ExpressFLG
                       ,FS.AdminNO
                       ,FS.SKUCD
                       ,FS.JanCD
                       ,FS.SKUName
                       ,FS.ColorName
                       ,FS.SizeName
                       ,0 AS SetKBN
                       ,0 AS SetRows
                       ,DM.JuchuuSuu
                       ,DM.JuchuuUnitPrice
                       ,DM.TaniCD
                       ,DM.JuchuuGaku
                       ,DM.JuchuuHontaiGaku
                       ,DM.JuchuuTax
                       ,DM.JuchuuTaxRitsu
                       ,DM.OrderUnitPrice AS CostUnitPrice
                       ,DM.OrderUnitPrice * DM.JuchuuSuu AS CostGaku
                       ,DM.OrderUnitPrice
                       ,DM.ProfitGaku
                       ,DM.SoukoCD
                       ,0 --HikiateSu
                       ,0 --DeliveryOrderSu
                       ,0 --DeliverySu
                       ,0 --DirectFLG
                       ,0 --HikiateFLG
                       ,NULL --JuchuuOrderNO
                       ,DT.VendorCD
                       ,NULL    --LastOrderNO
                       ,0   --LastOrderRows
                       ,NULL    --LastOrderDateTime
                       ,NULL	--DesiredDeliveryDate
                       ,0   --AnswerFLG
                       ,DM.ArrivePlanDate
                       ,NULL    --ArrivePlanNO
                       ,NULL    --ArriveDateTime
                       ,NULL    --ArriveNO
                       ,0   --ArribveNORows
                       ,@DeliveryPlanNO		--(CASE @OperateMode WHEN 1 THEN @DeliveryPlanNO ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
                       ,DM.CommentOutStore
                       ,DM.CommentInStore
                       ,DM.IndividualClientName
                       ,DM.ShippingPlanDate
                       ,NULL    --SalesDate
                       ,NULL    --SalesNO
                       ,NULL    --DepositeDetailNO
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME

                   FROM D_TenzikaiJuchuu AS DT
                   INNER JOIN D_TenzikaiJuchuuDetails AS DM
                   ON DM.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
                   AND DM.DeleteDateTime IS NULL
                   LEFT OUTER JOIN (SELECT MT.JanCD AS TenzikaiShouhinJanCD
                                     ,F.AdminNO
                                     ,F.SKUCD
                                     ,F.JanCD
                                     ,F.SKUName
                                     ,F.ColorName
                                     ,F.SizeName
                               FROM F_SKU(convert(date,@SYSDATETIME)) AS F
                               --一時ワークテーブル「M_TenzikaiShouhin①」(一時ワークテーブル「M_SKU①」で「M_TenzikaiShouhin①」として使用)
                               INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                                 ,MAX(MTS.JanCD) AS JanCD 
                                           FROM M_TenzikaiShouhin AS MTS
                                           WHERE MTS.DeleteFLG = 0
                                           AND MTS.VendorCD = @VendorCD
                                           AND MTS.LastYearTerm = @LastYearTerm
                                           AND MTS.LastSeason = @LastSeason
                                           GROUP BY MTS.ExhibitionCommonCD
                                           ) AS MT 
                               ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                               WHERE F.DeleteFlg = 0) AS FS
                   ON FS.TenzikaiShouhinJanCD = DM.JanCD
                   WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
                   AND DM.TenzikaiJuchuuRows = @TenzikaiJuchuuRows
                  ;
                  
            SET @DisplayRows = @DisplayRows + 1;
            
            --【D_StoreJuchuu】Table転送仕様Ｃ
            INSERT INTO [D_StoreJuchuu]
                   ([JuchuuNO]
                   ,[NouhinsyoComment])
             VALUES
                   (@JuchuuNO
                   ,NULL    --NouhinsyoComment
                   )
                   ;
            
            --【D_DeliveryPlan】Table転送仕様Ｇ
            INSERT INTO [D_DeliveryPlan]
                   ([DeliveryPlanNO]
                   ,[DeliveryKBN]
                   ,[Number]
                   ,[DeliveryName]
                   ,[DeliverySoukoCD]
                   ,[DeliveryZip1CD]
                   ,[DeliveryZip2CD]
                   ,[DeliveryAddress1]
                   ,[DeliveryAddress2]
                   ,[DeliveryMailAddress]
                   ,[DeliveryTelphoneNO]
                   ,[DeliveryFaxNO]
                   ,[DecidedDeliveryDate]
                   ,[DecidedDeliveryTime]
                   ,[CarrierCD]
                   ,[PaymentMethodCD]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[InvoiceNO]
                   ,[DeliveryPlanDate]
                   ,[HikiateFLG]
                   ,[IncludeFLG]
                   ,[OntheDayFLG]
                   ,[ExpressFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
            SELECT
                    @DeliveryPlanNO
                   ,1 AS DeliveryKBN    --(1:販売、2:倉庫移動)
                   ,@JuchuuNO   AS Number
                   ,DT.DeliveryName
                   ,NULL    --[DeliverySoukoCD]
                   ,DT.DeliveryZipCD1
                   ,DT.DeliveryZipCD2
                   ,DT.DeliveryAddress1
                   ,DT.DeliveryAddress2
                   ,NULL    --[DeliveryMailAddress]
                   ,DT.DeliveryTel11 + '-' + DT.DeliveryTel12 + '-' + DT.DeliveryTel13    --[DeliveryTelphoneNO]
                   ,NULL    --[DeliveryFaxNO]
                   ,NULL    --[DecidedDeliveryDate]
                   ,NULL    --[DecidedDeliveryTime]
                   ,NULL    --[CarrierCD]
                   ,DT.PaymentMethodCD    --[PaymentMethodCD]
                   ,NULL    --[CommentInStore]
                   ,NULL    --[CommentOutStore]
                   ,NULL    --[InvoiceNO]
                   ,NULL    --[DeliveryPlanDate]
                   ,0   --[HikiateFLG]
                   ,0   --[IncludeFLG]
                   ,0   --[OntheDayFLG]
                   ,0   --[ExpressFLG]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
            FROM D_TenzikaiJuchuu AS DT
            WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
            ;
                       
            --【D_DeliveryPlanDetails】配送予定明細　Table転送仕様Ｈ
            INSERT INTO [D_DeliveryPlanDetails]
               ([DeliveryPlanNO]
               ,[DeliveryPlanRows]
               ,[Number]
               ,[NumberRows]
               ,[CommentInStore]
               ,[CommentOutStore]
               ,[HikiateFLG]
               ,[UpdateCancelKBN]
               ,[DeliveryOrderComIn]
               ,[DeliveryOrderComOut]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
             SELECT  
                @DeliveryPlanNO --ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
               ,DM.JuchuuRows AS DeliveryPlanRows
               ,@JuchuuNO AS Number
               ,DM.JuchuuRows  As NumberRows
               ,DM.CommentInStore
               ,DM.CommentOutStore
               ,0   --HikiateFLG
               ,0   --UpdateCancelKBN
               ,NULL    --DeliveryOrderComIn
               ,NULL    --DeliveryOrderComOut                        
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

              FROM D_JuchuuDetails AS DM
              WHERE DM.JuchuuNO = @JuchuuNO
              ;
            
            --【D_TenzikaiJuchuu】 Table転送仕様Ｉ（更新（Update））
            UPDATE D_TenzikaiJuchuu SET
                [JuchuuHurikaeZumiFLG]  = 1
               ,[JuchuuHurikaeDateTime] = @SYSDATETIME
               ,[UpdateOperator]        = @Operator  
               ,[UpdateDateTime]        = @SYSDATETIME
            WHERE DeleteDateTime IS NULL
            AND JuchuuHurikaeZumiFLG = 0
            AND TenzikaiJuchuuNO = @TenzikaiJuchuuNO
            ;

            -- ========= ループ内の実際の処理 ここまで===
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_Tenzi
            INTO @TenzikaiJuchuuNO, @JuchuuDate, @TenzikaiJuchuuRows;

        END
        
        --カーソルを閉じる
        CLOSE CUR_Tenzi;
        DEALLOCATE CUR_Tenzi;

    END
    
    --削除を選択している場合、(If you have selected by "削除",)
    ELSE IF @OperateMode = 3
    BEGIN
        --カーソル定義
        DECLARE CUR_Juchu CURSOR FOR
            --該当の展示会受注番号に紐づく受注番号を取得(Obtain "受注番号" associated with the corresponding "展示会受注番号")
            --(Table_JuchuuNO)
            SELECT A.JuchuuNO
            From D_Juchuu AS A
            --該当の展示会受注番号を取得(Obtain the relevant "展示会受注番号")
            --(Table_TenzikaiJuchuuNO)
            Inner Join (SELECT DT.TenzikaiJuchuuNO                                                                                 
                        From D_TenzikaiJuchuu AS DT                                                                                 
                        Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
                        AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
                        AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
                        AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
                        And DT.DeleteDateTime IS NULL
            )   --Table_TenzikaiJuchuuNO 
            AS B
            On A.TenzikaiJuchuuNO = B.TenzikaiJuchuuNO
            Where A.DeleteDateTime IS NULL            
            ;
            
        --カーソルオープン
        OPEN CUR_Juchu;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- ========= ループ内の実際の処理 ここから===
            --D_Juchuu Delete
            Delete FROM D_Juchuu 
            WHERE JuchuuNO = @JuchuuNO
            AND DeleteDateTime IS NULL
            ;
            
            --D_JuchuuDetails
            DELETE FROM D_JuchuuDetails
            WHERE JuchuuNO = @JuchuuNO
            AND DeleteDateTime IS NULL
            ;
            
            --D_StoreJuchuu 
            Delete FROM D_StoreJuchuu
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --D_DeliveryPlan
            Delete FROM D_DeliveryPlan
            WHERE Number = @JuchuuNO
            ;
            
            --D_DeliveryPlanDetails
            Delete FROM D_DeliveryPlanDetails
            WHERE Number = @JuchuuNO
            ;

            -- ========= ループ内の実際の処理 ここまで===
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_Juchu
            INTO @JuchuuNO;

        END
        
        --カーソルを閉じる
        CLOSE CUR_Juchu;
        DEALLOCATE CUR_Juchu;

        --【D_TenzikaiJuchuu】 Table転送仕様Ｉ（削除（Update））
        UPDATE D_TenzikaiJuchuu SET
            [JuchuuHurikaeZumiFLG]  = 0
           ,[JuchuuHurikaeDateTime] = NULL
           ,[UpdateOperator]        = @Operator  
           ,[UpdateDateTime]        = @SYSDATETIME
        WHERE DeleteDateTime IS NULL
        AND VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE VendorCD END)
        AND LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE LastYearTerm END)
        AND LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE LastSeason END)
        AND JuchuuHurikaeZumiFLG = 1
        AND CAST(InsertDateTime AS DATE) = @TorokuDate
        ;

    END
     
    --処理履歴データへ更新
    --SET @KeyItem = @JuchuuNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TenzikaiJuchuuJouhouHurikaeShori',
        @PC,
        NULL,
        @KeyItem;

    --SET @OutJuchuuProcessNO = @JuchuuProcessNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
