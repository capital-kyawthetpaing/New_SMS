

DROP  PROCEDURE [dbo].[D_Juchuu_SelectData]
GO
DROP  PROCEDURE [dbo].[CheckJuchuData]
GO
DROP  PROCEDURE [dbo].[CheckJuchuDetailsData]
GO
DROP  PROCEDURE [dbo].[PRC_TempoJuchuuNyuuryoku]
GO
DROP TYPE [dbo].[T_Juchuu]
GO

--  ======================================================================
--       Program Call    受注入力
--       Program ID      TempoJuchuuNyuuryoku
--       Create date:    2019.6.19
--    ======================================================================
CREATE PROCEDURE D_Juchuu_SelectData
    (@OperateMode    tinyint,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO varchar(11),
    @Tennic tinyint
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

--        IF @OperateMode = 2   --修正時
--        BEGIN
            SELECT DH.JuchuuNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.JuchuuDate,111) AS JuchuuDate
                  ,DH.JuchuuTime
                  ,DH.ReturnFLG
                  ,DH.SoukoCD 
                  ,DH.JuchuuKBN
                  ,DH.SiteKBN
                  ,CONVERT(varchar,DH.SiteJuchuuDateTime,111) AS SiteJuchuuDateTime
                  ,DH.SiteJuchuuNO
                  ,DH.InportErrFLG
                  ,DH.OnHoldFLG
                  ,DH.IdentificationFLG
                  ,CONVERT(varchar,DH.TorikomiDateTime,111) AS TorikomiDateTime
                  ,DH.StaffCD
                  ,DH.CustomerCD
                  ,DH.CustomerName
                  ,DH.CustomerName2
                  ,DH.AliasKBN
                  ,DH.ZipCD1
                  ,DH.ZipCD2
                  ,DH.Address1
                  ,DH.Address2
                  ,DH.Tel11
                  ,DH.Tel12
                  ,DH.Tel13
                  ,DH.Tel21
                  ,DH.Tel22
                  ,DH.Tel23
                  ,DH.CustomerKanaName
                  ,DH.DeliveryCD
                  ,DH.DeliveryName
                  ,DH.DeliveryName2
                  ,DH.DeliveryAliasKBN
                  ,DH.DeliveryZipCD1
                  ,DH.DeliveryZipCD2
                  ,DH.DeliveryAddress1
                  ,DH.DeliveryAddress2
                  ,DH.DeliveryTel11
                  ,DH.DeliveryTel12
                  ,DH.DeliveryTel13 
                  ,DH.JuchuuCarrierCD
                  ,DH.DecidedCarrierFLG
                  ,DH.LastCarrierCD
                  ,CONVERT(varchar,DH.NameSortingDateTime,111) AS NameSortingDateTime
                  ,DH.NameSortingStaffCD
                  ,DH.CurrencyCD
                  ,DH.JuchuuGaku AS SUM_JuchuuGaku
                  ,DH.Discount
                  ,DH.HanbaiHontaiGaku
                  ,DH.HanbaiTax8
                  ,DH.HanbaiTax10
                  ,DH.HanbaiGaku
                  ,DH.CostGaku AS SUM_CostGaku
                  ,DH.ProfitGaku AS SUM_ProfitGaku
                  ,DH.Coupon
                  --,DH.Point
                  ,DH.PayCharge
                  ,DH.Adjustments
                  ,DH.Postage
                  ,DH.GiftWrapCharge
                  ,DH.InvoiceGaku
                  ,DH.PaymentMethodCD
                  ,DH.PaymentPlanNO
                  ,DH.CardProgressKBN
                  ,DH.CardCompany
                  ,DH.CardNumber
                  ,DH.PaymentProgressKBN
                  ,DH.PresentFLG
                  ,CONVERT(varchar,DH.SalesPlanDate,111) AS SalesPlanDate
                  ,CONVERT(varchar,DH.FirstPaypentPlanDate,111) AS FirstPaypentPlanDate
                  ,CONVERT(varchar,DH.LastPaymentPlanDate,111) AS LastPaymentPlanDate
                  ,DH.DemandProgressKBN
                  ,DH.CommentDemand
                  ,CONVERT(varchar,DH.CancelDate,111) AS CancelDate
                  ,DH.CancelReasonKBN
                  ,DH.CancelRemarks
                  ,DH.NoMailFLG
                  ,DH.IndividualContactKBN
                  ,DH.TelephoneContactKBN
                  ,DH.LastMailKBN
                  ,DH.LastMailPatternCD
                  ,CONVERT(varchar,DH.LastMailDatetime,111) AS LastMailDatetime
                  ,DH.LastMailName
                  ,DH.NextMailKBN
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  ,CONVERT(varchar,DH.LastDepositeDate,111) AS LastDepositeDate
                  ,CONVERT(varchar,DH.LastOrderDate,111) AS LastOrderDate
                  ,CONVERT(varchar,DH.LastArriveDate,111) AS LastArriveDate
                  ,CONVERT(varchar,DH.LastSalesDate,111) AS LastSalesDate
                  ,DH.MitsumoriNO
                  ,CONVERT(varchar,DH.JuchuuDateTime,111) AS JuchuuDateTime
                  
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.JuchuuRows
                  ,DM.DisplayRows
                  ,DM.SiteJuchuuRows
                  ,DM.AdminNO AS SKUNO
                  ,DM.SKUCD
                  ,DM.JanCD
                  ,DM.SKUName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.NotPrintFLG
                  ,DM.NotOrderFLG
                  ,DM.DirectFLG
                  ,DM.SetKBN
                  ,DM.SetRows
                  ,DM.JuchuuSuu
                  ,DM.JuchuuUnitPrice
                  ,DM.TaniCD
                  ,DM.JuchuuGaku
                  ,DM.JuchuuHontaiGaku
                  ,DM.JuchuuTax
                  ,DM.JuchuuTaxRitsu
                  ,DM.CostUnitPrice
                  ,DM.CostGaku
                  ,DM.ProfitGaku
                  ,DM.SoukoCD AS M_SoukoCD
                  ,DM.HikiateSu
                  ,DM.DeliveryOrderSu
                  ,DM.DeliverySu
                  ,DM.HikiateFLG
                  ,(SELECT DO.OrderNO 
                  	FROM D_OrderDetails AS DO 
                    WHERE DO.OrderNO = DM.LastOrderNO
                    AND DO.OrderRows = DM.LastOrderRows
                    AND DO.DeleteDateTime IS NULL
                  ) AS JuchuuOrderNO
                  ,DM.VendorCD
                  ,DM.LastOrderNO
                  ,DM.LastOrderRows
                  ,CONVERT(varchar,DM.LastOrderDateTime,111) AS LastOrderDateTime
                  ,CONVERT(varchar,DM.DesiredDeliveryDate,111) AS DesiredDeliveryDate
                  ,DM.AnswerFLG
                  ,(SELECT ISNULL(CONVERT(varchar,DO.ArrivePlanDate,111), CONVERT(varchar,DM.ArrivePlanDate,111))
                    FROM D_OrderDetails AS DO 
                    WHERE DO.OrderNO = DM.LastOrderNO
                    AND DO.OrderRows = DM.LastOrderRows
                    AND DO.DeleteDateTime IS NULL
                  ) AS ArrivePlanDate
                  
                  ,(SELECT CONVERT(varchar,DP.PaymentPlanDate,111) 
                    FROM D_PurchaseDetails AS DPD 
                    LEFT OUTER JOIN D_Purchase AS DP 
                    ON DP.PurchaseNO = DPD.PurchaseNO
                    AND DP.DeleteDateTime IS NULL
                    WHERE DPD.OrderNO = DM.LastOrderNO
                    AND DPD.OrderRows = DM.LastOrderRows 
                    AND DPD.DeleteDateTime IS NULL
                  )AS PaymentPlanDate	--明細部.支払予定日

                  ,CONVERT(varchar,DPC.CollectClearDate,111) AS D_CollectClearDate	--明細部.入金日

                  ,DM.ArrivePlanNO
                  ,CONVERT(varchar,DM.ArriveDateTime,111) AS ArriveDateTime
                  ,DM.ArriveNO
                  ,DM.ArribveNORows
                  ,DM.DeliveryPlanNO
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore
                  ,DM.IndividualClientName
                  ,CONVERT(varchar,DM.SalesDate,111) AS SalesDate
                  ,DM.SalesNO
                  ,DM.DepositeDetailNO
                  
                  ,DM.ExpressFLG
                  ,DM.OrderUnitPrice
                  ,CONVERT(varchar,DM.ShippingPlanDate,111) AS ShippingPlanDate
                  
                  --【Data Area Footer】
                  ,DS.NouhinsyoComment
                  ,MAX(CONVERT(varchar,A.SalesDate,111)) OVER() AS D_Sales_SalesDate	--売上日
                  ,SUM(B.CollectGaku) OVER() AS CollectGaku	--入金額
                  ,MAX(CONVERT(varchar,C.BillingCloseDate,111)) OVER() AS BillingCloseDate	--請求締日
                  ,MAX(CONVERT(varchar,C.CollectPlanDate,111)) OVER() AS CollectPlanDate	--（請求時）入金予定日
                  ,MAX(CONVERT(varchar,D.CollectClearDate,111)) OVER() AS CollectClearDate --最終入金日

              FROM D_Juchuu DH

              LEFT OUTER JOIN D_JuchuuDetails AS DM ON DH.JuchuuNO = DM.JuchuuNO AND DM.DeleteDateTime IS NULL
              LEFT OUTER JOIN D_StoreJuchuu AS DS ON DH.JuchuuNO = DS.JuchuuNO
              LEFT OUTER JOIN (
                SELECT M.JuchuuNO,M.JuchuuRows, MAX(H.SalesDate) AS SalesDate 
                  FROM D_SalesDetails AS M
                  INNER JOIN D_Sales AS H ON H.SalesNO = M.SalesNO AND H.DeleteDateTime IS NULL
                    WHERE M.DeleteDateTime IS NULL
                    GROUP BY M.JuchuuNO,M.JuchuuRows
                  )AS A ON A.JuchuuNO = DM.JuchuuNO AND A.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN(
--                SELECT M.JuchuuNO,M.JuchuuRows,SUM(H.CollectGaku) AS CollectGaku	2019.10.23 chg
                SELECT M.JuchuuNO,M.JuchuuRows,SUM(H.CollectAmount) AS CollectGaku
                FROM D_CollectBilling AS H
                INNER JOIN D_CollectPlanDetails AS M ON M.CollectPlanNO = H.CollectPlanNO
                AND H.DeleteDateTime IS NULL
                WHERE M.DeleteDateTime IS NULL
                GROUP BY M.JuchuuNO,M.JuchuuRows
                ) AS B ON B.JuchuuNO = DM.JuchuuNO
              AND B.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN (
                  SELECT M.JuchuuNO,M.JuchuuRows,Max(H.BillingCloseDate) AS BillingCloseDate
                  ,Max(H.CollectPlanDate) AS CollectPlanDate
                  FROM D_Billing AS H
                  INNER JOIN D_BillingDetails AS HM ON HM.BillingNO = H.BillingNO
                  AND HM.DeleteDateTime IS NULL
                  INNER JOIN D_CollectPlanDetails M ON M.CollectPlanNO = HM.CollectPlanNO
                  AND M.CollectPlanRows = HM.CollectPlanRows
                  AND M.DeleteDateTime IS NULL
                  WHERE H.DeleteDateTime IS NULL
                  GROUP BY M.JuchuuNO,M.JuchuuRows
              ) AS C ON C.JuchuuNO = DM.JuchuuNO
              AND C.JuchuuRows = DM.JuchuuRows
              LEFT OUTER JOIN (
                  SELECT DCB.JuchuuNO,DCB.JuchuuRows,Max(DPC.CollectClearDate) AS CollectClearDate
                    FROM D_CollectPlanDetails AS DCB
                  LEFT OUTER JOIN D_CollectBilling AS DCB2 ON DCB2.CollectPlanNO = DCB.CollectPlanNO
                    AND DCB2.DeleteDateTime IS NULL
                      LEFT OUTER JOIN D_PaymentConfirm AS DPC ON DPC.ConfirmNO = DCB2.ConfirmNO 
		                AND DPC.DeleteDateTime IS NULL
                  WHERE DCB.DeleteDateTime IS NULL
                  GROUP BY DCB.JuchuuNO,DCB.JuchuuRows
              ) AS D ON D.JuchuuNO = DM.JuchuuNO
              AND D.JuchuuRows = DM.JuchuuRows
                
              --明細部の入金日を求めるためのサブクエリ
              LEFT OUTER JOIN (SELECT DC.JuchuuNO,DC.JuchuuRows,MAX(DPC.CollectClearDate) AS CollectClearDate
                            FROM D_CollectPlanDetails AS DC 
                              LEFT OUTER JOIN D_CollectBillingDetails AS DCB ON DCB.CollectPlanNO = DC.CollectPlanNO
                                AND DCB.CollectPlanRows = DC.CollectPlanRows 
                                AND DCB.DeleteDateTime IS NULL
                              LEFT OUTER JOIN D_CollectBilling AS DCB2 ON DCB2.CollectPlanNO = DCB.CollectPlanNO
                                AND DCB2.DeleteDateTime IS NULL
                              LEFT OUTER JOIN D_PaymentConfirm AS DPC ON DPC.ConfirmNO = DCB2.ConfirmNO 
                                AND DPC.DeleteDateTime IS NULL
                            
                            WHERE DC.DeleteDateTime IS NULL
                            GROUP BY DC.JuchuuNO,DC.JuchuuRows
               )AS DPC
               ON DPC.JuchuuNO = DM.JuchuuNO
               AND DPC.JuchuuRows = DM.JuchuuRows 
                
              WHERE ((@Tennic = 0 AND DH.JuchuuNO = @JuchuuNO) OR (@Tennic = 1 AND DH.JuchuuProcessNO = @JuchuuNO))
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.JuchuuNO, DM.DisplayRows
                ;
--        END

END

GO

CREATE PROCEDURE CheckJuchuData
    (@JuchuuNO varchar(11)
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
    
    SET @ERRNO = '';
    
    --既に売上済み警告
    --以下の条件でレコードがあれば売上済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.JuchuuNO)
    FROM D_SalesDetails A
    WHERE A.JuchuuNO = @JuchuuNO
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E165';
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
        SET @ERRNO = 'E159';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    --既に出荷指示済み警告
    --以下の条件でレコードがあれば出荷指示済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_InstructionDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @JuchuuNO
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E160';
        SELECT @ERRNO AS errno;
        RETURN;
    END;

    --既にピッキングリスト完了済み警告
    --以下の条件でレコードがあればピッキング済として警告メッセージを表示する
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
    
    SELECT @ERRNO AS errno;

END

GO

CREATE PROCEDURE CheckJuchuDetailsData
    (@JuchuuNO varchar(11),
     @JuchuuRows int
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
    DECLARE @STATUS varchar(10);
    DECLARE @STATUS2 varchar(10);
    DECLARE @CNT int;
    DECLARE @FLG int;
    DECLARE @FLG2 int;
    
    SET @STATUS = '';
    SET @STATUS2 = '';
    SET @FLG = 0;
    SET @FLG2 = 0;

    --既に仕入済み警告
    --以下の条件でレコードがあれば発注済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.JuchuuNO)
    FROM D_PurchaseDetails D
    INNER JOIN D_ArrivalDetails C ON C.ArrivalNO = D.ArrivalNO
    INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
    INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
    WHERE A.JuchuuNO = @JuchuuNO
    AND A.JuchuuRows = @JuchuuRows
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND C.DeleteDateTime IS NULL
    AND D.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS = '仕入済';
        SET @FLG = 1;
    END;

    IF @FLG = 0
    BEGIN
        --既に入荷済み警告
        --以下の条件でレコードがあれば発注済として警告メッセージを表示する
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_ArrivalDetails C
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.JuchuuRows = @JuchuuRows
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @STATUS = '入荷済';
            SET @FLG = 1;
        END;

        IF @FLG = 0
        BEGIN
            --既に発注済み警告
            --以下の条件でレコードがあれば発注済として警告メッセージを表示する
            SELECT @CNT = COUNT(A.JuchuuNO)
            FROM D_OrderDetails A 
            WHERE A.JuchuuNO = @JuchuuNO
            AND A.JuchuuRows = @JuchuuRows
            AND A.DeleteDateTime IS NULL
            ;

            IF @CNT > 0 
            BEGIN
                SET @STATUS = '発注済';
                SET @FLG = 1;
            END;
        END;
    END;

    --既に売上済み警告
    --以下の条件でレコードがあれば売上済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.JuchuuNO)
    FROM D_SalesDetails A
    WHERE A.JuchuuNO = @JuchuuNO
    AND A.JuchuuRows = @JuchuuRows
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '売上済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;
    
    --既に出荷済み警告
    --以下の条件でレコードがあれば出荷済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_ShippingDetails A
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ShippingKBN = 1
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '出荷済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;

    --既に出荷指示済み警告
    --以下の条件でレコードがあれば出荷指示済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_InstructionDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = '出荷指示済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;

    --既にピッキングリスト完了済み警告
    --以下の条件でレコードがあればピッキング済として警告メッセージを表示する
    SELECT @CNT = COUNT(A.Number)
    FROM D_PickingDetails B
    INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
    WHERE A.Number = @JuchuuNO
    AND A.NumberRows = @JuchuuRows
    AND A.ReserveKBN = 1
    AND A.DeleteDateTime IS NULL
    AND B.DeleteDateTime IS NULL
    AND B.PickingDoneDateTime IS NOT NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @STATUS2 = 'ピッキング済';
        SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;
        RETURN;
    END;
    
    SELECT @STATUS AS STATUS, @STATUS2 AS STATUS2;

END

GO

CREATE TYPE T_Juchuu AS TABLE
    (
    [JuchuuRows] [int],
    [DisplayRows] [int],
    [SiteJuchuuRows] [int] ,
    [NotPrintFLG] [tinyint] ,
    [AddJuchuuRows] [int],
    [NotOrderFLG] [tinyint] ,
    [DirectFLG] [tinyint] ,
    [SKUNO] [int] ,
    [SKUCD] [varchar](30) ,
    [JanCD] [varchar](13) ,
    [SKUName] [varchar](80) ,
    [ColorName] [varchar](20) ,
    [SizeName] [varchar](20) ,
    [SetKBN] [tinyint] ,
--  [SetRows] [tinyint] ,
    [JuchuuSuu] [int] ,
    [JuchuuUnitPrice] [money] ,
    [TaniCD] [varchar](2) ,
    [JuchuuGaku] [money] ,
    [JuchuuHontaiGaku] [money] ,
    [JuchuuTax] [money] ,
    [JuchuuTaxRitsu] [int] ,
    [CostUnitPrice] [money] ,
    [CostGaku] [money] ,
    [ProfitGaku] [money] ,
    [SoukoCD] [varchar](6) ,
    [VendorCD] [varchar](13) ,
    [ArrivePlanDate] [date] ,
    [CommentOutStore] [varchar](80) ,
    [CommentInStore] [varchar](80) ,
    [IndividualClientName] [varchar](80) ,
    [ZaikoKBN] [tinyint] ,
    [TemporaryNO] [varchar](11) ,	--仮引当番号
    [ExpressFLG] [tinyint] ,
    [ShippingPlanDate] [date] ,
    [OrderUnitPrice] [money] ,
    [JuchuuNO] [varchar](11) ,	--Tennic=1の場合に使用
    [DesiredDeliveryDate]  [varchar](10) ,
    [UpdateFlg][tinyint]
    )
GO

CREATE PROCEDURE PRC_TempoJuchuuNyuuryoku
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO   varchar(11),
    @JuchuuProcessNO  varchar(11),
    @StoreCD   varchar(4),
    @JuchuuDate  varchar(10),
    @ReturnFLG tinyint ,
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    @CustomerCD   varchar(13),
    @CustomerName   varchar(80),
    @CustomerName2   varchar(40),
    @AliasKBN   tinyint,
    @ZipCD1   varchar(3),
    @ZipCD2   varchar(4),
    @Address1   varchar(100),
    @Address2   varchar(100),
    @Tel11   varchar(5),
    @Tel12   varchar(4),
    @Tel13   varchar(4),
    @Tel21   varchar(5),
    @Tel22   varchar(4),
    @Tel23   varchar(4),
    
    @DeliveryCD   varchar(13),
    @DeliveryName   varchar(80),
    @DeliveryName2   varchar(40),
    @DeliveryAliasKBN   tinyint,
    @DeliveryZipCD1   varchar(3),
    @DeliveryZipCD2   varchar(4),
    @DeliveryAddress1   varchar(100),
    @DeliveryAddress2   varchar(100),
    @DeliveryTel11   varchar(5),
    @DeliveryTel12   varchar(4),
    @DeliveryTel13   varchar(4),
--    @DeliveryTel21   varchar(5),
--    @DeliveryTel22   varchar(4),
--    @DeliveryTel23   varchar(4),
   
    @JuchuuGaku money ,
    @Discount money ,
    @HanbaiHontaiGaku money ,
    @HanbaiTax8 money ,
    @HanbaiTax10 money ,
    @HanbaiGaku money ,
    @CostGaku money ,
    @ProfitGaku money ,
    @Point money ,
    @InvoiceGaku money ,
    @PaymentMethodCD varchar(3) ,
    @PaymentPlanNO int ,
    @SalesPlanDate date ,
    @FirstPaypentPlanDate date ,
    @LastPaymentPlanDate date ,
    @CommentOutStore varchar(80) ,
    @CommentInStore varchar(80) ,
    @MitsumoriNO varchar(11), 
    @NouhinsyoComment varchar(700),

    @Table  T_Juchuu READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutJuchuuNo varchar(11) OUTPUT
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
    DECLARE @ReserveNO varchar(11);
    DECLARE @Tennic tinyint;
    DECLARE @DeliveryPlanNO  varchar(11);
    DECLARE @tblUpdateFlg tinyint;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    SET @Tennic = (SeLECT M.Tennic FROM M_Control AS M WHERE M.MainKey = 1);
    
    --新規・変更Mode時、
    IF @OperateMode <= 2
    BEGIN
        IF ISNULL(@JuchuuNO,'') <>'' OR ISNULL(@JuchuuProcessNO,'') <> ''
        BEGIN
            --最初に引当情報をクリアし、在庫に戻す 
            --【D_Stock】 
            UPDATE [D_Stock]
               SET [AllowableSu] = [D_Stock].[AllowableSu] + A.ReserveSu
                  ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] + A.ReserveSu
                  ,[ReserveSu] = [D_Stock].[ReserveSu] - A.ReserveSu
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
             FROM (SELECT R.StockNO, SUM(R.ReserveSu) AS ReserveSu 
                    FROM D_Reserve AS R
                    WHERE R.ReserveKBN = 1
                    AND R.DeleteDateTime IS NULL
                     AND ((@Tennic = 0 AND R.[Number] = @JuchuuNO)
                        OR (@Tennic = 1 AND R.[Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                                WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                                AND H.DeleteDateTime IS NULL)))
                	GROUP BY R.StockNO)AS A 
             WHERE A.StockNO = [D_Stock].StockNO
             ;
             
            --カーソル定義
            DECLARE CUR_UPD CURSOR FOR
                SELECT JuchuuNo
                FROM D_Juchuu
                WHERE ((@Tennic = 0 AND JuchuuNO = @JuchuuNO)
                    OR (@Tennic = 1 AND JuchuuNO IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                            WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                            AND H.DeleteDateTime IS NULL)))
                ORDER BY JuchuuNo;
            
            DECLARE @updJuchuuNo varchar(11);
            
            --カーソルオープン
            OPEN CUR_UPD;

            --最初の1行目を取得して変数へ値をセット
            FETCH NEXT FROM CUR_UPD
            INTO @updJuchuuNo;
            
            --データの行数分ループ処理を実行する
            WHILE @@FETCH_STATUS = 0
            BEGIN
            -- ========= ループ内の実際の処理 ここから===
            
                --ログのSEQ獲得のために更新
                UPDATE D_Juchuu SET
                    [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                WHERE JuchuuNO = @updJuchuuNo
                ;
                
                --20200225　テーブル転送仕様Ｊ②
                UPDATE D_JuchuuDetails SET
                    HikiateSu = D_JuchuuDetails.HikiateSu - A.ReserveSu
                    ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                                WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                                WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu = 0 THEN 3
                                ELSE 0 END )
                FROM (SELECT R.StockNO, R.[Number], R.NumberRows, SUM(R.ReserveSu) AS ReserveSu 
                    FROM D_Reserve AS R
                    WHERE R.ReserveKBN = 1
                    AND R.DeleteDateTime IS NULL
                    AND R.[Number] = @updJuchuuNo
                	GROUP BY R.StockNO, R.[Number], R.NumberRows)AS A  
                WHERE A.[Number] = D_JuchuuDetails.JuchuuNO
                AND A.NumberRows = D_JuchuuDetails.JuchuuRows
                AND D_JuchuuDetails.JuchuuNO = @updJuchuuNo
                AND D_JuchuuDetails.DeleteDateTime IS NULL
                ;
	            
                -- ========= ループ内の実際の処理 ここまで===
                --次の行のデータを取得して変数へ値をセット
                FETCH NEXT FROM CUR_UPD
                INTO @updJuchuuNo;

            END
            
            --カーソルを閉じる
            CLOSE CUR_UPD;
            DEALLOCATE CUR_UPD;
             
             
             --テーブル転送仕様Ｈ・（削除処理）
             --【D_Reserve】
             DELETE FROM D_Reserve
             WHERE ReserveKBN = 1
             AND ((@Tennic = 0 AND [Number] = @JuchuuNO)
                OR (@Tennic = 1 AND [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO)))
             ;
        END
        
        --引当可能ロジック
        DECLARE @return_value int,
                @Result tinyint,
                @Error tinyint,
                @LastDay varchar(10),
                @OutKariHikiateNo varchar(11),
                @AdminNO int,
                @DSoukoCD varchar(6),
                @Suryo int,
                @JuchuuRows int,
                @DisplayRows int,
                @DirectFlg tinyint,
                @KariHikiateNo varchar(11),
                @tblJuchuuNo varchar(11);
        
        --カーソル定義
        DECLARE CUR_AAA CURSOR FOR
            SELECT tbl.SKUNO,tbl.SoukoCD,tbl.JuchuuSuu
                ,(CASE @Tennic WHEN 1 THEN (CASE ISNULL(tbl.JuchuuNo,'') WHEN '' THEN tbl.DisplayRows ELSE 1 END) 
                	ELSE tbl.JuchuuRows END) AS JuchuuRows
                ,tbl.DisplayRows 
                --,tbl.ZaikoKBN
                ,tbl.DirectFlg
                ,tbl.TemporaryNO
                ,(CASE @Tennic WHEN 1 THEN tbl.JuchuuNo ELSE @JuchuuNo END) AS JuchuuNo
            FROM @Table tbl
            ORDER BY JuchuuNo, JuchuuRows 
            
        DECLARE @TAB TABLE(
             JNO varchar(11),
             GNO int NOT NULL
            ,HNO varchar(11)
        );
        
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO  @AdminNO,@DSoukoCD,@Suryo,@JuchuuRows,@DisplayRows,@DirectFlg,@KariHikiateNo,@tblJuchuuNo;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
            --IF @ZaikoKBN = 1
            IF @DirectFlg = 0	--中身はDirectFlg
            BEGIN
                EXEC Fnc_Reserve_SP
                    @AdminNO,
                    @JuchuuDate,
                    @StoreCD,
                    @DSoukoCD,
                    @Suryo,
                    1,  --@DenType
                    @tblJuchuuNo,  --新規時の受注番号を採番するべき？
                    @JuchuuRows,
                    @KariHikiateNo,
                    @Result OUTPUT,
                    @Error OUTPUT,
                    @LastDay OUTPUT,
                    @OutKariHikiateNo OUTPUT
                    ;
            END
            ELSE
            BEGIN
                SET @OutKariHikiateNo = null;
            END
            
            IF @Tennic = 0
            BEGIN
                INSERT INTO @TAB VALUES(@tblJuchuuNo,@JuchuuRows, @OutKariHikiateNo);
            END
            ELSE
            BEGIN
                INSERT INTO @TAB VALUES(@tblJuchuuNo,@DisplayRows, @OutKariHikiateNo);
            END
            -- ========= ループ内の実際の処理 ここまで===

            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @AdminNO,@DSoukoCD,@Suryo,@JuchuuRows,@DisplayRows,@DirectFlg,@KariHikiateNo,@tblJuchuuNo;

        END
        
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
    END

    
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        IF @Tennic = 1
        BEGIN
            --伝票番号採番
            EXEC Fnc_GetNumber
                30,             --in伝票種別 1
                @JuchuuDate, --in基準日
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
    END
       
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
        
        IF @Tennic = 1
        BEGIN
            --行削除されたデータはDELETE処理
            UPDATE D_JuchuuDetails
                SET [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                   ,[DeleteOperator]     =  @Operator  
                   ,[DeleteDateTime]     =  @SYSDATETIME
             WHERE [JuchuuNO] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                        AND H.DeleteDateTime IS NULL)
             AND NOT EXISTS (SELECT 1 FROM @Table tbl 
                        WHERE D_JuchuuDetails.[JuchuuRows] = 1 
                        AND D_JuchuuDetails.[JuchuuNO] = tbl.JuchuuNO) 
             ;
             
            UPDATE [D_Juchuu]
                SET [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                   ,[DeleteOperator]     =  @Operator  
                   ,[DeleteDateTime]     =  @SYSDATETIME
             WHERE JuchuuProcessNO = @JuchuuProcessNO
             AND NOT EXISTS (SELECT 1 FROM @Table AS tbl WHERE D_Juchuu.[JuchuuNO] = tbl.JuchuuNO)
            ;

            --行削除されたデータはDELETE処理
            DELETE FROM [D_DeliveryPlan]
            WHERE [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO)
            AND NOT EXISTS (SELECT 1 FROM @Table AS tbl WHERE D_DeliveryPlan.[Number] = tbl.JuchuuNO)
             ;
                                        
            DELETE FROM D_DeliveryPlanDetails
             WHERE [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                    WHERE H.JuchuuProcessNO = @JuchuuProcessNO)
             AND NOT EXISTS (SELECT 1 FROM @Table AS tbl WHERE D_DeliveryPlanDetails.[Number] = tbl.JuchuuNO)
             ;

            UPDATE [D_DeliveryPlan] SET
                   [DeliveryName] = @DeliveryName
                  ,[DeliveryZip1CD]   = @DeliveryZipCD1
                  ,[DeliveryZip2CD]   = @DeliveryZipCD2
                  ,[DeliveryAddress1] = @DeliveryAddress1
                  ,[DeliveryAddress2] = @DeliveryAddress2
                  ,[DeliveryTelphoneNO] = @DeliveryTel11 + '-' + @DeliveryTel12 + '-' + @DeliveryTel13  --[]
                  ,[PaymentMethodCD]    = @PaymentMethodCD
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
             WHERE EXISTS (SELECT 1 FROM @Table AS tbl WHERE D_DeliveryPlan.[Number] = tbl.JuchuuNO)
            ;

        END
                
        UPDATE D_Juchuu
           SET [StoreCD] = @StoreCD                         
              ,[JuchuuDate] = @JuchuuDate
              ,[ReturnFLG] =  @ReturnFLG
              ,[SoukoCD] =    @SoukoCD
              ,[StaffCD] = @StaffCD                         
              ,[CustomerCD] = @CustomerCD                   
              ,[CustomerName] = @CustomerName               
              ,[CustomerName2] = @CustomerName2             
              ,[AliasKBN] = @AliasKBN
              ,[ZipCD1] = @ZipCD1                           
              ,[ZipCD2] = @ZipCD2                           
              ,[Address1] = @Address1                       
              ,[Address2] = @Address2                       
              ,[Tel11] = @Tel11                             
              ,[Tel12] = @Tel12                             
              ,[Tel13] = @Tel13                             
              ,[Tel21] = @Tel21                             
              ,[Tel22] = @Tel22                             
              ,[Tel23] = @Tel23              
              ,[DeliveryCD]       = @DeliveryCD
              ,[DeliveryName]     = @DeliveryName
              ,[DeliveryName2]    = @DeliveryName2
              ,[DeliveryAliasKBN] = @DeliveryAliasKBN
              ,[DeliveryZipCD1]   = @DeliveryZipCD1
              ,[DeliveryZipCD2]   = @DeliveryZipCD2
              ,[DeliveryAddress1] = @DeliveryAddress1
              ,[DeliveryAddress2] = @DeliveryAddress2
              ,[DeliveryTel11]    = @DeliveryTel11
              ,[DeliveryTel12]    = @DeliveryTel12
              ,[DeliveryTel13]    = @DeliveryTel13
              ,[JuchuuGaku] =       @JuchuuGaku
              ,[Discount] =         @Discount
              ,[HanbaiHontaiGaku] = @HanbaiHontaiGaku
              ,[HanbaiTax8] =       @HanbaiTax8
              ,[HanbaiTax10] =      @HanbaiTax10
              ,[HanbaiGaku] =       @HanbaiGaku
              ,[CostGaku] =         @CostGaku
              ,[ProfitGaku] =       @ProfitGaku                          
              --,[Point] =             @Point       
              ,[InvoiceGaku] =       @InvoiceGaku
              ,[PaymentMethodCD]  = @PaymentMethodCD
              ,[SalesPlanDate] =         @SalesPlanDate
              ,[FirstPaypentPlanDate] =  @FirstPaypentPlanDate
              ,[LastPaymentPlanDate] =   @LastPaymentPlanDate
              ,[CommentOutStore] =       @CommentOutStore
              ,[CommentInStore] =        @CommentInStore       
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE (@Tennic = 0 AND JuchuuNO = @JuchuuNO)
         	OR (@Tennic = 1 AND JuchuuProcessNO = @JuchuuProcessNO)
         AND DeleteDateTime IS NULL
         ;

        --【D_StoreJuchuu】
        UPDATE  [D_StoreJuchuu]
           SET  [NouhinsyoComment] = @NouhinsyoComment
        WHERE ((@Tennic = 0 AND [JuchuuNO] = @JuchuuNO)
        	OR (@Tennic = 1 AND [JuchuuNO] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                        AND H.DeleteDateTime IS NULL)))
        ;

    END

    IF @OperateMode <= 2    --新規・修正時
    BEGIN
        --カーソル定義
        DECLARE CUR_DETAIL CURSOR FOR
            SELECT ISNULL(tbl.JuchuuNo,@JuchuuNo) AS JuchuuNo,tbl.JuchuuRows,tbl.DisplayRows
            	,tbl.UpdateFlg
            FROM @Table tbl
            ORDER BY ISNULL(tbl.JuchuuNo,@JuchuuNo), tbl.JuchuuRows,tbl.DisplayRows; 
        
        DECLARE @FIRST_FLG tinyint;
        SET @FIRST_FLG = 1;
        
        --カーソルオープン
        OPEN CUR_DETAIL;
        
        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_DETAIL
        INTO  @tblJuchuuNo,@JuchuuRows,@DisplayRows,@tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
        	--テニックの場合は行追加時も。テニックでない場合は新規時のみ1回採番
            IF (@Tennic = 1 AND @tblUpdateFlg =0) OR (@OperateMode = 1 AND @Tennic = 0 AND @FIRST_FLG = 1)
            BEGIN
                --伝票番号採番
                EXEC Fnc_GetNumber
                    1,             --in伝票種別 1
                    @JuchuuDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @JuchuuNO OUTPUT
                    ;
                
                IF ISNULL(@JuchuuNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END

                SET @tblJuchuuNo = @JuchuuNO;
                
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
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[JuchuuDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             VALUES
                   (@JuchuuNO
                   ,@JuchuuProcessNO                      
                   ,@StoreCD                          
                   ,convert(date,@JuchuuDate)                    
                   ,NULL    --JuchuuTime
                   ,@ReturnFLG
                   ,@SoukoCD
                   ,3   --JuchuuKBN
                   ,0   --SiteKBN
                   ,NULL    --SiteJuchuuDateTime
                   ,NULL    --SiteJuchuuNO
                   ,0   --InportErrFLG
                   ,0   --OnHoldFLG
                   ,0   --IdentificationFLG
                   ,NULL    --TorikomiDateTime
                   ,@StaffCD
                   ,@CustomerCD
                   ,@CustomerName
                   ,@CustomerName2
                   ,@AliasKBN
                   ,@ZipCD1
                   ,@ZipCD2
                   ,@Address1
                   ,@Address2
                   ,@Tel11
                   ,@Tel12
                   ,@Tel13
                   ,NULL    --Tel21
                   ,NULL    --Tel22
                   ,NULL    --Tel23
                   ,NULL    --CustomerKanaName
                   ,@DeliveryCD
                   ,@DeliveryName
                   ,@DeliveryName2
                   ,@DeliveryAliasKBN
                   ,@DeliveryZipCD1
                   ,@DeliveryZipCD2
                   ,@DeliveryAddress1
                   ,@DeliveryAddress2
                   ,@DeliveryTel11
                   ,@DeliveryTel12
                   ,@DeliveryTel13
                   ,NULL    --Tel21
                   ,NULL    --Tel22
                   ,NULL    --Tel23
                   ,NULL   --,[DeliveryKanaName]
                   ,NULL    --JuchuuCarrierCD
                   ,0   --DecidedCarrierFLG
                   ,NULL    --LastCarrierCD
                   ,NULL    --NameSortingDateTime
                   ,NULL    --NameSortingStaffCD
                   ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
                   ,@JuchuuGaku
                   ,@Discount
                   ,@HanbaiHontaiGaku
                   ,@HanbaiTax8
                   ,@HanbaiTax10
                   ,@HanbaiGaku
                   ,@CostGaku
                   ,@ProfitGaku
                   ,0   --Coupon
                   --,@Point
                   ,0   --PayCharge
                   ,0   --Adjustments
                   ,0   --Postage
                   ,0   --GiftWrapCharge
                   ,@InvoiceGaku
                   ,@PaymentMethodCD
                   ,@PaymentPlanNO
                   ,0   --CardProgressKBN
                   ,NULL    --CardCompany
                   ,NULL    --CardNumber
                   ,0   --PaymentProgressKBN
                   ,0   --PresentFLG
                   ,@SalesPlanDate
                   ,@FirstPaypentPlanDate
                   ,@LastPaymentPlanDate
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
                   ,@CommentOutStore
                   ,@CommentInStore
                   ,NULL    --LastDepositeDate
                   ,NULL    --LastOrderDate
                   ,NULL    --LastArriveDate
                   ,NULL    --LastSalesDate
                   ,@MitsumoriNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL                  
                   ,NULL
                   );               

                --【D_StoreJuchuu】
                INSERT INTO [D_StoreJuchuu]
                       ([JuchuuNO]
                       ,[NouhinsyoComment])
                 VALUES
                       (@JuchuuNO
                       ,@NouhinsyoComment
                       )
                       ;
                       
                --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
                --M_Control.TennicFLG＝1
                IF @Tennic = 1
                BEGIN
                	
                    --【D_DeliveryPlan】配送予定情報　Table転送仕様Ｋ
                    --伝票番号採番
                    EXEC Fnc_GetNumber
                        19,             --in伝票種別 19
                        @JuchuuDate,      --in基準日
                        @StoreCD,       --in店舗CD
                        @Operator,
                        @DeliveryPlanNO OUTPUT
                        ;
                    
                    IF ISNULL(@DeliveryPlanNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END

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
                           ,@DeliveryName
                           ,NULL    --[DeliverySoukoCD]
                           ,@DeliveryZipCD1
                           ,@DeliveryZipCD2
                           ,@DeliveryAddress1
                           ,@DeliveryAddress2
                           ,NULL    --[DeliveryMailAddress]
                           ,@DeliveryTel11 + '-' + @DeliveryTel12 + '-' + @DeliveryTel13    --[DeliveryTelphoneNO]
                           ,NULL    --[DeliveryFaxNO]
                           ,NULL    --[DecidedDeliveryDate]
                           ,NULL    --[DecidedDeliveryTime]
                           ,NULL    --[CarrierCD]
                           ,@PaymentMethodCD    --[PaymentMethodCD]
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
                           ;            
                           
                           
                END
            END		--ヘッダをINSERTするかどうかの条件
            
            IF (@Tennic = 1 OR (@Tennic = 0 AND @FIRST_FLG = 1))
            BEGIN
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
                           ,(CASE @Tennic WHEN 1 THEN 1 ELSE tbl.JuchuuRows END)                       
                           ,tbl.DisplayRows                      
                           ,0   --SiteJuchuuRows
                           ,tbl.NotPrintFLG
                           ,tbl.AddJuchuuRows
                           ,tbl.NotOrderFLG
                           ,tbl.ExpressFLG
                           ,tbl.SKUNO
                           ,tbl.SKUCD
                           ,tbl.JanCD
                           ,tbl.SKUName
                           ,tbl.ColorName
                           ,tbl.SizeName
                           ,tbl.SetKBN
                           ,0 AS SetRows
                           ,tbl.JuchuuSuu
                           ,tbl.JuchuuUnitPrice
                           ,tbl.TaniCD
                           ,tbl.JuchuuGaku
                           ,tbl.JuchuuHontaiGaku
                           ,tbl.JuchuuTax
                           ,tbl.JuchuuTaxRitsu
                           ,tbl.CostUnitPrice
                           ,tbl.CostGaku
                           ,tbl.OrderUnitPrice
                           ,tbl.ProfitGaku
                           ,tbl.SoukoCD
                           ,0 --HikiateSu
                           ,0 --DeliveryOrderSu
                           ,0 --DeliverySu
                           ,tbl.DirectFLG
                           ,0 --HikiateFLG
                           ,NULL --JuchuuOrderNO
                           ,tbl.VendorCD
                           ,NULL    --LastOrderNO
                           ,0   --LastOrderRows
                           ,NULL    --LastOrderDateTime
                           ,tbl.DesiredDeliveryDate
                           ,0   --AnswerFLG
                           ,tbl.ArrivePlanDate
                           ,NULL    --ArrivePlanNO
                           ,NULL    --ArriveDateTime
                           ,NULL    --ArriveNO
                           ,0   --ArribveNORows
                           ,@DeliveryPlanNO		--(CASE @OperateMode WHEN 1 THEN @DeliveryPlanNO ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
                           ,tbl.CommentOutStore
                           ,tbl.CommentInStore
                           ,tbl.IndividualClientName
                           ,tbl.ShippingPlanDate
                           ,NULL    --SalesDate
                           ,NULL    --SalesNO
                           ,NULL    --DepositeDetailNO
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME

                      FROM @Table tbl
                      WHERE tbl.UpdateFlg = 0
                      AND (@Tennic = 0  OR (@Tennic = 1 AND tbl.DisplayRows = @DisplayRows))  --★
                      ;
            END

            --修正時のみ（行削除データと修正データの更新）
            IF @OperateMode = 2 AND (@Tennic = 1 OR (@Tennic = 0 AND @FIRST_FLG = 1))
            BEGIN
                IF @Tennic = 0
                BEGIN
                    --ログのSEQ獲得のために更新
                    UPDATE D_Juchuu SET
                        [UpdateOperator]     =  @Operator  
                       ,[UpdateDateTime]     =  @SYSDATETIME
                    WHERE JuchuuNO = @JuchuuNo
                     AND NOT EXISTS (SELECT 1 FROM @Table tbl 
                                    INNER JOIN D_JuchuuDetails AS DM 
                                    ON tbl.JuchuuRows = DM.JuchuuRows
                                    AND DM.JuchuuNO = D_Juchuu.JuchuuNO)
                                    ;

                    --行削除されたデータはDELETE処理
                    UPDATE D_JuchuuDetails
                        SET [UpdateOperator]     =  @Operator  
                           ,[UpdateDateTime]     =  @SYSDATETIME
                           ,[DeleteOperator]     =  @Operator  
                           ,[DeleteDateTime]     =  @SYSDATETIME
                     WHERE [JuchuuNO] = @JuchuuNo
                     AND NOT EXISTS (SELECT 1 FROM @Table tbl 
                                WHERE tbl.JuchuuRows = D_JuchuuDetails.[JuchuuRows])
                     ;
                END
            	
                --ログのSEQ獲得のために更新
                UPDATE D_Juchuu SET
                    [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                WHERE JuchuuNO = @tblJuchuuNo
                AND EXISTS(SELECT 1 FROM @Table tbl WHERE tbl.UpdateFlg = 1
                	AND (@Tennic = 0 OR (@Tennic = 1 AND tbl.JuchuuNO = D_Juchuu.JuchuuNO)))
                ;
                
                UPDATE [D_JuchuuDetails]
                   SET [DisplayRows] = tbl.DisplayRows                  
                       ,[NotPrintFLG] = tbl.NotPrintFLG
                       ,[AddJuchuuRows] = tbl.AddJuchuuRows
                       ,[NotOrderFLG] = tbl.NotOrderFLG
                       ,[ExpressFLG] = tbl.ExpressFLG
                       ,[AdminNO] = tbl.SKUNO                              
                       ,[SKUCD] = tbl.SKUCD                              
                       ,[JanCD] = tbl.JanCD                              
                       ,[SKUName] = tbl.SKUName                          
                       ,[ColorName] = tbl.ColorName                      
                       ,[SizeName] = tbl.SizeName                        
                       ,[SetKBN] = tbl.SetKBN                            
                       ,[JuchuuSuu] = tbl.JuchuuSuu                      
                       ,[JuchuuUnitPrice] = tbl.JuchuuUnitPrice          
                       ,[TaniCD] = tbl.TaniCD                            
                       ,[JuchuuGaku] = tbl.JuchuuGaku                    
                       ,[JuchuuHontaiGaku] = tbl.JuchuuHontaiGaku        
                       ,[JuchuuTax] = tbl.JuchuuTax                      
                       ,[JuchuuTaxRitsu] = tbl.JuchuuTaxRitsu            
                       ,[CostUnitPrice] = tbl.CostUnitPrice              
                       ,[CostGaku] = tbl.CostGaku   
                       ,[OrderUnitPrice] = tbl.OrderUnitPrice
                       ,[ProfitGaku] = tbl.ProfitGaku                    
                       ,[SoukoCD] = tbl.SoukoCD    
                       ,[DirectFLG] = tbl.DirectFLG                        
                       ,[VendorCD] = tbl.VendorCD                
                       ,[DesiredDeliveryDate] = tbl.DesiredDeliveryDate            
                       ,[ArrivePlanDate] = tbl.ArrivePlanDate            
                       ,[CommentOutStore] = tbl.CommentOutStore          
                       ,[CommentInStore] = tbl.CommentInStore            
                       ,[IndividualClientName] = tbl.IndividualClientName
                       ,[ShippingPlanDate]     = tbl.ShippingPlanDate
                       ,[UpdateOperator]     =  @Operator  
                       ,[UpdateDateTime]     =  @SYSDATETIME
                FROM D_JuchuuDetails
                INNER JOIN @Table AS tbl
                 ON ((@Tennic = 1 AND D_JuchuuDetails.JuchuuRows = 1 AND D_JuchuuDetails.JuchuuNO = tbl.JuchuuNO) 
                 	OR (@Tennic = 0 AND tbl.JuchuuRows = D_JuchuuDetails.JuchuuRows))
                 AND tbl.UpdateFlg = 1
                WHERE D_JuchuuDetails.JuchuuNO = @tblJuchuuNo
                 ;
            END
        
            IF @FIRST_FLG = 1
            BEGIN
                --【D_Mitsumori】
                UPDATE D_Mitsumori
                SET JuchuuFLG = 1
                   ,[UpdateOperator] =  @Operator  
                   ,[UpdateDateTime] =  @SYSDATETIME
                WHERE MitsumoriNO = @MitsumoriNO
                ;
            END
            
            IF ((@Tennic = 0 AND @FIRST_FLG = 1) OR @Tennic = 1)
            BEGIN 
                --【D_Reserve】INSERT処理　受注行数分のレコードを作成(引当在庫レコードの件数分)
                --カーソル定義
                DECLARE CUR_TAB CURSOR FOR
                    SELECT T.HNO, T.GNO, A.KeySEQ
                    FROM @TAB AS T
                    INNER JOIN D_TemporaryReserve AS A
                    ON A.TemporaryNO = T.HNO
                    WHERE (@Tennic = 0 OR (@Tennic = 1 AND T.GNO = @DisplayRows))
                    ORDER BY T.GNO, A.KeySEQ
                    ;
                
                DECLARE @GNO int
                    ,@HNO varchar(11)
                    ,@KeySEQ int;
                    
                --カーソルオープン
                OPEN CUR_TAB;

                --最初の1行目を取得して変数へ値をセット
                FETCH NEXT FROM CUR_TAB
                INTO  @HNO, @GNO, @KeySEQ;
                
                --データの行数分ループ処理を実行する
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ループ内の実際の処理 ここから===
                
                    --伝票番号採番
                    EXEC Fnc_GetNumber
                        12,             --in伝票種別 12
                        @JuchuuDate,    --in基準日
                        @StoreCD,       --in店舗CD
                        @Operator,
                        @ReserveNO OUTPUT
                        ;
                    
                    IF ISNULL(@ReserveNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --Table転送仕様Ｈ
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
                           ,[ShippingPlanDate]
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
                           ,@tblJuchuuNo		--★
                           ,(CASE @Tennic WHEN 1 THEN 1 ELSE tbl.JuchuuRows END) AS JuchuuRows
                           ,A.StockNO
                           ,tbl.SoukoCD
                           ,tbl.JanCD
                           ,tbl.SKUCD
                           ,tbl.SKUNO
                           ,A.ReserveSu
                           ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                                   THEN NULL
                                   ELSE CONVERT(date,@SYSDATETIME) END)       --[ShippingPossibleDate]
                           ,NULL AS ShippingPlanDate
                           ,(CASE WHEN ISNULL(B.ArrivalYetFLG,0)=1 
                                   THEN 0
                                   ELSE A.ReserveSu END)       --[ShippingPossibleSU]
                           ,NULL    --[ShippingOrderNO]
                           ,0       --[ShippingOrderRows]
                           ,NULL    --[CompletedPickingNO]
                           ,0       --[CompletedPickingRow]
                           ,NULL    --[CompletedPickingDate]
                           ,0   --[ShippingSu]
                           ,0   --[ReturnKBN]
                           ,0   --[OriginalReserveNO]
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL    --[DeleteOperator]
                           ,NULL    --[DeleteDateTime]
                    
                    FROM D_TemporaryReserve A
                    INNER JOIN  @Table tbl 
                    ON ((@Tennic = 0 AND tbl.JuchuuRows = @GNO)        
                    	OR (@Tennic = 1 AND tbl.DisplayRows = @GNO)) --★Tennicの場合でも画面の行番号が格納されている
                    LEFT OUTER JOIN D_Stock B ON B.StockNO = A.StockNO
                    WHERE  A.TemporaryNO = @HNO
                    AND A.KeySEQ = @KeySEQ
                    ;

                    --【D_Stock】
                    UPDATE [D_Stock]
                       SET [AllowableSu] = [D_Stock].[AllowableSu] - A.UpdateSu
                          ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] - A.UpdateSu
                          ,[ReserveSu] = [D_Stock].[ReserveSu] + A.UpdateSu
                          ,[UpdateOperator] = @Operator
                          ,[UpdateDateTime] = @SYSDATETIME
                     FROM D_TemporaryReserve AS A 
                   --     INNER JOIN  @Table tbl ON A.TemporaryNO = (SELECT HNO FROM @TAB WHERE GNO = tbl.JuchuuRows)
                     WHERE A.StockNO = [D_Stock].StockNO
                     AND A.TemporaryNO = @HNO
                     AND A.KeySEQ = @KeySEQ
                     ;
                                 
                    --ログのSEQ獲得のために更新
                    UPDATE D_Juchuu SET
                        [UpdateOperator]     =  @Operator  
                       ,[UpdateDateTime]     =  @SYSDATETIME
                    WHERE JuchuuNO = @tblJuchuuNo
                    ;
                    
                    --20200225　テーブル転送仕様Ｊ
                    UPDATE D_JuchuuDetails SET
                        HikiateSu = D_JuchuuDetails.HikiateSu + A.ReserveSu
                        ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                                    WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                                    WHEN D_JuchuuDetails.HikiateSu + A.ReserveSu = 0 THEN 3
                                    ELSE 0 END )
                    FROM D_TemporaryReserve A
                    INNER JOIN @Table tbl 
                    ON ((@Tennic = 0 AND tbl.JuchuuRows = @GNO)        
                     OR (@Tennic = 1 AND tbl.DisplayRows = @GNO)) --★Tennicの場合でも画面の行番号が格納されている
                    
                    WHERE A.TemporaryNO = @HNO
                    AND A.KeySEQ = @KeySEQ  
                    AND D_JuchuuDetails.JuchuuNO = @tblJuchuuNo
                    AND ((@Tennic = 0 AND D_JuchuuDetails.JuchuuRows = tbl.JuchuuRows) 
                      OR (@Tennic = 1 AND D_JuchuuDetails.JuchuuRows = 1))
                    ;

                    -- ========= ループ内の実際の処理 ここまで===

                    --次の行のデータを取得して変数へ値をセット
                    FETCH NEXT FROM CUR_TAB
                    INTO  @HNO, @GNO, @KeySEQ;

                END
                
                --カーソルを閉じる
                CLOSE CUR_TAB;
                DEALLOCATE CUR_TAB;
            END
            
            --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
            --M_Control.TennicFLG＝1
            IF @Tennic = 1
            BEGIN
                --【D_DeliveryPlanDetails】配送予定明細　Table転送仕様Ｌ
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
                    @DeliveryPlanNO		--(CASE @OperateMode WHEN 1 THEN @DeliveryPlanNO ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
                   ,tbl.JuchuuRows AS DeliveryPlanRows
                   ,@tblJuchuuNO AS Number
                   ,1 As NumberRows
                   ,tbl.CommentInStore
                   ,tbl.CommentOutStore
                   ,(CASE DM.DirectFLG WHEN 1 THEN 1 ELSE DM.HikiateFLG END)
                   ,0   --UpdateCancelKBN]
                   ,NULL    --DeliveryOrderComIn]
                   ,NULL    --DeliveryOrderComOut]                        
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

                  FROM @Table tbl
                  INNER JOIN D_JuchuuDetails AS DM
                  ON DM.JuchuuRows = 1	--tbl.JuchuuRows	★
                  AND DM.JuchuuNO = @tblJuchuuNO
                  WHERE tbl.UpdateFlg = 0
                  AND tbl.DisplayRows = @DisplayRows		--★
                  ;
                  
                 
                UPDATE [D_DeliveryPlanDetails] SET
                       [CommentInStore]  = tbl.CommentInStore
                      ,[CommentOutStore] = tbl.CommentOutStore
                      ,[HikiateFLG]      = (CASE DM.DirectFLG WHEN 1 THEN 1 ELSE DM.HikiateFLG END)
                      ,[UpdateOperator]  =  @Operator  
                      ,[UpdateDateTime]  =  @SYSDATETIME
                      
                 FROM @Table AS tbl
                  INNER JOIN D_JuchuuDetails AS DM
                  ON DM.JuchuuRows = tbl.JuchuuRows
                  AND DM.JuchuuNO = @tblJuchuuNO
                 WHERE @tblJuchuuNO = D_DeliveryPlanDetails.Number
                 AND tbl.JuchuuNO = D_DeliveryPlanDetails.Number
                 AND 1 = D_DeliveryPlanDetails.NumberRows	--tbl.JuchuuRows
                 AND tbl.UpdateFlg = 1
                ;
                
                --出荷予定　D_DeliveryPlan Table転送仕様Ｋ
                UPDATE [D_DeliveryPlan] SET
                    HikiateFLG = 1
                WHERE @tblJuchuuNO = D_DeliveryPlan.Number
                AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS DD
                    WHERE DD.DeliveryPlanNO = D_DeliveryPlan.DeliveryPlanNO
                    AND DD.HikiateFLG = 0)
                ;
                
            END            
            -- ========= ループ内の実際の処理 ここまで===
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_DETAIL
            INTO @tblJuchuuNo,@JuchuuRows,@DisplayRows,@tblUpdateFlg;
            
	        SET @FIRST_FLG = 0;
        END

    END
 
    IF @OperateMode = 3    --削除
    BEGIN
        SET @OperateModeNm = '削除';
        
        --【D_Mitsumori】
        UPDATE D_Mitsumori
        SET JuchuuFLG = 0	--受注成約FLG＝０にUPDATE
           ,[UpdateOperator] =  @Operator  
           ,[UpdateDateTime] =  @SYSDATETIME
        WHERE MitsumoriNO = @MitsumoriNO
        ;

        --【D_StoreJuchuu】
        
        --テニックの場合（店舗受注入力の結果を出荷指示に表示する）
        --M_Control.TennicFLG＝1
        IF @Tennic = 1
        BEGIN
            DELETE FROM [D_DeliveryPlan]
            WHERE [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO);
                                        
        	--Table転送仕様Ｌ②
            DELETE FROM [D_DeliveryPlanDetails]
            WHERE [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO);	--= @JuchuuNO;
        END
        
        --【D_Stock】
        UPDATE [D_Stock]
           SET [AllowableSu] = [D_Stock].[AllowableSu] + A.ReserveSu
              ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] + A.ReserveSu
              ,[ReserveSu] = [D_Stock].[ReserveSu] - A.ReserveSu
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
         FROM (SELECT R.StockNO, SUM(R.ReserveSu) AS ReserveSu 
                    FROM D_Reserve AS R
                    WHERE R.ReserveKBN = 1
                     AND R.DeleteDateTime IS NULL
                     AND ((@Tennic = 0 AND R.Number = @JuchuuNO)
                        OR (@Tennic = 1 AND R.Number IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                                    WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                                    AND H.DeleteDateTime IS NULL)))
                	GROUP BY R.StockNO)AS A 
         WHERE A.StockNO = [D_Stock].StockNO
         AND [D_Stock].DeleteDateTime IS NULL
         ;

                   
        /* --20200225　テーブル転送仕様Ｊ②
        UPDATE D_JuchuuDetails SET
            HikiateSu = D_JuchuuDetails.HikiateSu - A.ReserveSu
            ,HikiateFlg = (CASE WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu >= D_JuchuuDetails.JuchuuSuu THEN 1
                        WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu < D_JuchuuDetails.JuchuuSuu THEN 2
                        WHEN D_JuchuuDetails.HikiateSu - A.ReserveSu = 0 THEN 3
                        ELSE 0 END )
        FROM D_Reserve A
        WHERE A.ReserveKBN = 1
        AND A.[Number] = D_JuchuuDetails.JuchuuNO
        AND A.NumberRows = D_JuchuuDetails.JuchuuRows
        AND ((@Tennic = 0 AND D_JuchuuDetails.JuchuuNO = @JuchuuNO)
        	OR (@Tennic = 1 AND D_JuchuuDetails.JuchuuNO IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO
                                        AND H.DeleteDateTime IS NULL)))
        ;
        */
        --【D_Reserve】
        DELETE FROM D_Reserve
        WHERE ReserveKBN = 1
        AND ((@Tennic = 0 AND [Number] = @JuchuuNO)
         	OR (@Tennic = 1 AND [Number] IN (SELECT H.JuchuuNO FROM D_Juchuu AS H 
                                        WHERE H.JuchuuProcessNO = @JuchuuProcessNO)))
        ;

 		--カーソル定義
        DECLARE CUR_AAA CURSOR FOR
            SELECT ISNULL(tbl.JuchuuNo,@JuchuuNo) AS JuchuuNo
            FROM @Table tbl
            GROUP BY ISNULL(tbl.JuchuuNo,@JuchuuNo)
            ORDER BY JuchuuNo
        
        --カーソルオープン
        OPEN CUR_AAA;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @tblJuchuuNo;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===

            UPDATE [D_Juchuu]
                SET [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                   ,[DeleteOperator]     =  @Operator  
                   ,[DeleteDateTime]     =  @SYSDATETIME
             WHERE JuchuuNO = @tblJuchuuNo
               AND DeleteDateTime IS NULL
               ;
            
            --削除 Table転送仕様Ｊ②
            UPDATE [D_JuchuuDetails]
                SET [UpdateOperator]     =  @Operator  
                   ,[UpdateDateTime]     =  @SYSDATETIME
                   ,[DeleteOperator]     =  @Operator  
                   ,[DeleteDateTime]     =  @SYSDATETIME
             WHERE JuchuuNO = @tblJuchuuNo
               AND DeleteDateTime IS NULL
             ;

            IF @Tennic = 0
            BEGIN
                BREAK;
            END;

            -- ========= ループ内の実際の処理 ここまで===
            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_AAA
            INTO @tblJuchuuNo;	         
		END
		
        --カーソルを閉じる
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
    END
    
    
    --処理履歴データへ更新
    SET @KeyItem = @JuchuuNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TempoJuchuuNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutJuchuuNo = @JuchuuNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
