

/****** Object:  StoredProcedure [dbo].[PRC_TempoRegiHanbaiTouroku]    Script Date: 2021/02/18 20:36:11 ******/
DROP PROCEDURE [dbo].[PRC_TempoRegiHanbaiTouroku]
GO

/****** Object:  StoredProcedure [dbo].[PRC_TempoRegiHanbaiTouroku]    Script Date: 2021/02/18 20:36:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    “X•ÜƒŒƒW ”Ì”„“o˜^
--       Program ID      TempoRegiHanbaiTouroku
--       Create date:    2019.10.19
--    ======================================================================
--Alter TYPE [T_TempoHanbai] AS TABLE(
--    [JuchuuRows] [int] NULL,
--    [DisplayRows] [int] NULL,
--    [AdminNO] [int] NULL,
--    [SKUCD] [varchar](30) NULL,
--    [JanCD] [varchar](13) NULL,

--    [JuchuuSuu] [int] NULL,
--    [JuchuuUnitPrice] [money] NULL,
--    [TaniCD] [varchar](2) NULL,
--    [JuchuuGaku] [money] NULL,
--    [JuchuuHontaiGaku] [money] NULL,
--    [JuchuuTax] [money] NULL,
--    [JuchuuTaxRitsu] [int] NULL,
--	[ProperGaku] [money] NULL,	--2019.12.12 add

--    [UpdateFlg] [tinyint] NULL
--)
--GO
                   
                   
CREATE PROCEDURE [dbo].[PRC_TempoRegiHanbaiTouroku]
    (@OperateMode    int,                 -- ˆ—‹æ•ªi1:V‹K 2:C³ 3:íœj
    @SalesNO  varchar(11),
    @JuchuuNO  varchar(11),
    @StoreCD   varchar(4),
    @CustomerCD   varchar(13),
    @Age  tinyint,	--2019.12.16 add
    @CustomerChangedate varchar(10),
    
    @SalesRate int,
    @AdvanceAmount money,
    @PointAmount money,
    @CashAmount money,
    @RefundAmount money,
    @DenominationCD varchar(3),
    @CardAmount money,
    @Discount money,
    @CreditAmount money,
    @Denomination1Amount money,
    @DenominationCD1 varchar(3),
    @Denomination2Amount money,
    @DenominationCD2 varchar(3),
    @DepositAmount money,
    @Keijobi  varchar(10),
    
    --‘æˆê‰æ–Ê
    @SalesGaku money,   --‚¨”ƒã‹àŠzŒv
    @SalesTax money,    --‚¤‚¿ÅŠz
    
    @SalesHontaiGaku0 money,
    @SalesHontaiGaku8 money,
    @SalesHontaiGaku10 money,
    @HanbaiTax10 money,
    @HanbaiTax8 money,
    @InvoiceGaku money,
    @Discount8 money,
    @Discount10 money,
    @DiscountTax money,
    @DiscountTax8 money,
    @DiscountTax10 money,

    --‘æ“ñ‰æ–Ê
    @TotalAmount money,		--x•¥Œv
    
    @Table  T_TempoHanbai READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutSalesNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 ˆ—ŠJn                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @ChangeDate varchar(10);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @Haspo tinyint;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @ChangeDate = convert(VARCHAR,@SYSDATETIME,111);
    SET @Haspo = (SELECT HASPO FROM M_Control WHERE [MainKey] = 1);
    
    DECLARE @Program varchar(50);
    
    DECLARE @OldAdvanceAmount money;
    DECLARE @OldPointAmount money;
    DECLARE @OldCashAmount money;
    DECLARE @OldRefundAmount money;
    DECLARE @OldDenominationCD varchar(3);
    DECLARE @OldCardAmount money;
    DECLARE @OldDiscount money;
    DECLARE @OldCreditAmount money;
    DECLARE @OldDenomination1Amount money;
    DECLARE @OldDenominationCD1 varchar(3);
    DECLARE @OldDenomination2Amount money;
    DECLARE @OldDenominationCD2 varchar(3);
    
    DECLARE @LastPoint money;
    DECLARE @SUMGaku money;
    
    SET @Program = 'TempoRegiHanbaiTouroku';
	
    SET @SUMGaku = (SELECT SUM((CASE FS.SaleExcludedFlg WHEN 0 THEN (CASE FS.DiscountKBN WHEN 1 THEN -1*tbl.JuchuuHontaiGaku ELSE tbl.JuchuuHontaiGaku END)
                                ELSE 0 END))
                    FROM @Table tbl
                    INNER JOIN F_SKU(@ChangeDate) AS FS
                    ON FS.AdminNO = tbl.AdminNO
                    WHERE tbl.UpdateFlg = 0);
            
    --¡‰ñ‚Ì”Ì”„‚Å“Á‰¿•i‚ğœ‚¢‚½–{‘ÌŠz‚É‘Î‚µ‚Äİ’è‚µ‚½—¦‚Åƒ|ƒCƒ“ƒg‚ğ•t—^‚·‚é
    SET @LastPoint = FLOOR(@SUMGaku * (SELECT top 1 PointRate 
                        FROM M_StorePoint AS M 
                        WHERE M.StoreCD = @StoreCD
                        AND M.ChangeDate <= @ChangeDate
                        ORDER BY M.ChangeDate desc)/100);
                        
    --XVˆ—i’Êí”Ì”„j-----------------------------------------------------------------
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = NULL;

        --ƒgƒŠƒK[‚ª‚ ‚é‚Ì‚Åƒwƒbƒ_•”‚©‚çInsert
        --“`•[”Ô†Ì”Ô
        EXEC Fnc_GetNumber
            1,             --in“`•[í•Ê 1
            @ChangeDate    , --inŠî€“ú
            @StoreCD,       --in“X•ÜCD
            @Operator,
            @JuchuuNO OUTPUT
            ;
        
        IF ISNULL(@JuchuuNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚e(’Êíj     Insertó’ D_Juchuu
        --yD_Juchuuz
        INSERT INTO [D_Juchuu]
           ([JuchuuNO]
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
           ,[Point]
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
           ,@StoreCD                          
           ,convert(date,@SYSDATETIME)                    
           ,convert(Time,@SYSDATETIME)    --JuchuuTime
           ,0   --ReturnFLG
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            AND M.SoukoType= 3
            ORDER BY M.ChangeDate desc)

           ,2   --JuchuuKBN 2   “X“ª
           ,0   --SiteKBN
           ,NULL    --SiteJuchuuDateTime
           ,NULL    --SiteJuchuuNO
           ,0   --InportErrFLG
           ,0   --OnHoldFLG
           ,0   --IdentificationFLG
           ,NULL    --TorikomiDateTime
           ,@Operator
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
            */
           ,(SELECT top 1 M.AliasKBN FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel11 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel12 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel13 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel21 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel21
           ,(SELECT top 1 M.Tel22 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel22
           ,(SELECT top 1 M.Tel23 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel23
           ,(SELECT top 1 M.KanaName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --CustomerKanaName
           ,NULL    --JuchuuCarrierCD
           ,0   --DecidedCarrierFLG
           ,NULL    --LastCarrierCD
           ,NULL    --NameSortingDateTime
           ,NULL    --NameSortingStaffCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
           ,@SalesGaku + @Discount
           ,@Discount
           ,@SalesGaku - @SalesTax
           ,@HanbaiTax8
           ,@HanbaiTax10
           ,@SalesGaku
           ,0   --CostGaku
           ,@SalesGaku - @SalesTax
           ,0   --Coupon
           ,0   --Point
           ,0   --PayCharge
           ,0   --Adjustments
           ,0   --Postage
           ,0   --GiftWrapCharge
           ,@InvoiceGaku
           ,NULL	--PaymentMethodCD
           ,0	--PaymentPlanNO
           ,0   --CardProgressKBN
           ,NULL    --CardCompany
           ,NULL    --CardNumber
           ,0   --PaymentProgressKBN
           ,0   --PresentFLG
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
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
           ,convert(date,@SYSDATETIME)    --LastDepositeDate
           ,NULL    --LastOrderDate
           ,NULL    --LastArriveDate
           ,convert(date,@SYSDATETIME)    --LastSalesDate
           ,NULL    --MitsumoriNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL                  
           ,NULL
           );               

        --ƒe[ƒuƒ‹“]‘—d—l‚f(’Êíj     Insertó’–¾× D_JuchuuDetails    
       INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
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
                   ,tbl.JuchuuRows                       
                   ,tbl.DisplayRows                      
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,tbl.JuchuuGaku
                   ,tbl.JuchuuHontaiGaku
                   ,tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,0	--CostUnitPrice
                   ,0	--CostGaku
                   ,tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,tbl.JuchuuSuu --HikiateSu
                   ,tbl.JuchuuSuu --DeliveryOrderSu
                   ,tbl.JuchuuSuu --DeliverySu
                   ,0	--DirectFLG
                   ,0 --HikiateFLG
                   ,NULL --JuchuuOrderNO
                   ,NULL	--VendorCD
                   ,NULL    --LastOrderNO
                   ,0   --LastOrderRows
                   ,NULL    --LastOrderDateTime
                   ,NULL    --DesiredDeliveryDate
                   ,0   --AnswerFLG
                   ,NULL	--ArrivePlanDate
                   ,NULL    --ArrivePlanNO
                   ,NULL    --ArriveDateTime
                   ,NULL    --ArriveNO
                   ,0   --ArribveNORows
                   ,NULL    --DeliveryPlanNO
                   ,NULL	--CommentOutStore
                   ,NULL	--CommentInStore
                   ,NULL	--IndividualClientName
                   ,convert(date,@SYSDATETIME)  --ShippingPlanDate      2020.01.30 add
                   ,convert(date,@SYSDATETIME)  --SalesDate     2020.01.30 add
                   ,NULL    --SalesNO
                   ,NULL    --DepositeDetailNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
                                                                       
        --ƒe[ƒuƒ‹“]‘—d—l‚e—š—ğ        Insertó’—š—ğ 
        --ƒe[ƒuƒ‹“]‘—d—l‚f—š—ğ        Insertó’–¾×—š—ğ
        --Trigger
         
        --ƒe[ƒuƒ‹“]‘—d—l‚`(’Êíj     Insert”„ã D_Sales
        --“`•[”Ô†Ì”Ô
        EXEC Fnc_GetNumber
            3,          --in“`•[í•Ê 3
            @ChangeDate    , --inŠî€“ú
            @StoreCD,    --in“X•ÜCD
            @Operator,
            @SalesNO OUTPUT
            ;
            
        IF ISNULL(@SalesNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --Table“]‘—d—l‚f Insert ”„ã
        INSERT INTO [D_Sales]
           ([SalesNO]
           ,[StoreCD]
           ,[SalesDate]
           ,[ShippingNO]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[BillingType]
           ,[Age]	--2019.12.16 add
           ,[SalesHontaiGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesTax]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesGaku]
           ,[LastPoint]
           ,[WaitingPoint]
           ,[StaffCD]
           ,[PrintDate]
           ,[PrintStaffCD]
           ,[Discount]
           ,[Discount8]
           ,[Discount10]
           ,[DiscountTax]
           ,[DiscountTax8]
           ,[DiscountTax10]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[PurchaseNO]
           ,[SalesEntryKBN]
           ,[NouhinsyoComment]
           
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
           @SalesNO
           ,@StoreCD
           ,CONVERT(date, @SYSDATETIME)
           ,NULL	--ShippingNO
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
                WHERE M.CustomerCD = @CustomerCD
                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
                WHERE M.CustomerCD = @CustomerCD
                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                AND M.DeleteFlg = 0
                ORDER BY M.ChangeDate desc)
            */
           ,1	--BillingType
           ,@Age	--2019.12.16 add
           ,@SalesGaku-@SalesTax    --SalesHontaiGaku
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku0
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku8
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END) --SalesHontaiGaku10
           ,SUM(tbl.JuchuuTax) 
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuTax ELSE 0 END)  --SalesTax8
           ,SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuTax ELSE 0 END) --SalesTax10
           ,@SalesGaku
           ,@LastPoint
           ,0	--<WaitingPoint, money,>
           ,@Operator   --StaffCD
           ,NULL    --PrintDate
           ,NULL    --PrintStaffCD
           ,@Discount
           ,@Discount8
           ,@Discount10
           ,@DiscountTax
           ,@DiscountTax8
           ,@DiscountTax10
           ,0 AS CostGaku
           ,0 AS ProfitGaku
           ,NULL AS PurchaseNO
           ,2 AS SalesEntryKBN
           ,NULL AS NouhinsyoComment
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DJM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DJM.JuchuuRows
       WHERE DJM.JuchuuNO = @JuchuuNO
       GROUP BY DJM.JuchuuNO
       ;
                                          
        --ƒe[ƒuƒ‹“]‘—d—l‚a(’Êí)      Insert”„ã–¾×   D_SalesDetails  
       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[NotPrintFLG]
           ,[AddSalesRows]
           ,[ShippingNO]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[TaniCD]
           ,[SalesHontaiGaku]
           ,[SalesTax]
           ,[SalesGaku]
           ,[SalesTaxRitsu]
           ,[ProperGaku]    --2019.12.12 add
           ,[DiscountGaku]  --2019.12.12 add
           ,[CostUnitPrice]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[DeliveryNoteFLG]
           ,[BillingPrintFLG]
           ,[PurchaseNO]
           ,[PurchaseRows]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
            @SalesNO
           ,DM.JuchuuRows
           ,DM.JuchuuNO
           ,DM.JuchuuRows
           ,0 AS NotPrintFLG
           ,0 AS AddSalesRows
           ,NULL	--ShippingNO
           ,DM.AdminNO
           ,DM.SKUCD
           ,DM.JanCD
           ,DM.SKUName
           ,DM.ColorName
           ,DM.SizeName
           ,DM.JuchuuSuu	--SalesSU
           ,DM.JuchuuUnitPrice  --SalesUnitPrice
           ,DM.TaniCD
           ,DM.JuchuuHontaiGaku    --SalesHontaiGaku
           ,DM.JuchuuTax
           ,DM.JuchuuGaku   --SalesGaku
           ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
           ,0   --tbl.ProperGaku    --2019.12.12 add
           ,0   --tbl.ProperGaku - DM.JuchuuGaku AS DiscountGaku    --2019.12.12 add
           ,0 AS CostUnitPrice
           ,0 AS CostGaku
           ,0 AS ProfitGaku
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,NULL  --IndividualClientName
           ,0   --<DeliveryNoteFLG, tinyint,>
           ,0   --<BillingPrintFLG, tinyint,>
           ,NULL AS PurchaseNO
           ,0 AS PurchaseRows
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DM.JuchuuRows
       WHERE DM.JuchuuNO = @JuchuuNO
       ;
            
        --ƒe[ƒuƒ‹“]‘—d—l‚g         Insert“X•Ü“ü‹à
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
                   ,[PurchaseAmount]
                   ,[TaxAmount]
                   ,[DiscountAmount]
                   ,[BillingAmount]
                   ,[PointAmount]
                   ,[CardDenominationCD]
                   ,[CardAmount]
                   ,[CashAmount]
                   ,[DepositAmount]
                   ,[RefundAmount]
                   ,[CreditAmount]
                   ,[Denomination1CD]
                   ,[Denomination1Amount]
                   ,[Denomination2CD]
                   ,[Denomination2Amount]
                   ,[AdvanceAmount]
                   ,[TotalAmount]
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             VALUES
                   (@SalesNO
                   ,(SELECT ISNULL(MAX(DataNo),0)+1 FROM D_SalesTran)
                   ,@StoreCD
                   ,1	--1:’ÊíA2:•Ô•iA3;’ù³A4:æÁ
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME)
                   ;
                                       
        --ƒe[ƒuƒ‹“]‘—d—l‚h	Insert”„ã„ˆÚE•
        --ƒe[ƒuƒ‹“]‘—d—l‚i	Insert”„ã–¾×„ˆÚE•                   
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,1  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1  --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
            
        --ƒe[ƒuƒ‹“]‘—d—l‚b(’Êí)      Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ”„ã‚Ìƒwƒbƒ_î•ñ    
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,0	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,@SalesGaku - @SalesTax	--SalesGaku, money,>
           ,@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>š
           ,@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
			;

        --ƒe[ƒuƒ‹“]‘—d—l‚c(’Êí) Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ”„ã‚Ì–¾×î•ñ  
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,2	--DataKBN
           ,1	--DepositKBN
           ,0	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,tbl.JuchuuRows	--Rows, tinyint,>š
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku  --TotalGaku, money,>
           ,0   --Refund, money,>
           ,tbl.ProperGaku                          --ProperGaku 2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku         --DiscountGaku 2019.12.12 add
           ,0   --IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
	      FROM @Table tbl
	      WHERE tbl.UpdateFlg = 0
       ;

        --“ü‹àFƒ|ƒCƒ“ƒg‚Ìê‡
        IF @PointAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡@(’Êí) insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        END
        
        --Œ»‹à‚ ‚è‚Ì    
        IF @CashAmount <> 0
        BEGIN
        	----ƒe[ƒuƒ‹“]‘—d—l‚dè„€'(’Êí) insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  Œ»‹à‚ ‚è‚Ì    
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;

        END
        
        --ƒJ[ƒh‚ ‚è‚Ì  
        IF @CardAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡B(’Êí) Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ƒJ[ƒh‚ ‚è‚Ì  
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END
        
        --’lˆø‚«‚ ‚è‚Ì  
        IF @Discount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡C(’Êí) Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ’lˆø‚«‚ ‚è‚Ì  
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;
        END
        
        --‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì    ‚»‚Ì‘¼‚Ìê‡ MAX‚QƒŒƒR[ƒh
        IF @Denomination1Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        IF @Denomination2Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END

        IF @CreditAmount <> 0
        BEGIN
            --ƒe[ƒuƒ‹“]‘—d—l‚d‡E(’Êí)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        Š|‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1          --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program   --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@CreditAmount
                ;
        END
        
        --“ü‹àF‘Oó‹à‚Ìê‡
        IF @AdvanceAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡F(’Êí) insert“X•Ü“üo‹à—š—ğ@D_DepositHistory  ƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,0  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;
        END

        --ƒe[ƒuƒ‹“]‘—d—l‚k ‰ïˆõƒ}ƒXƒ^[
        UPDATE M_Customer SET
            LastPoint = LastPoint + @LastPoint - (CASE @Haspo WHEN 1 THEN @PointAmount ELSE 0 END) --ƒ|ƒCƒ“ƒgÁ‰»
            ,TotalPoint = TotalPoint + @LastPoint
            ,TotalPurchase = TotalPurchase + @SalesGaku-@SalesTax
            ,LastSalesDate = @ChangeDate
            ,LastSalesStoreCD = @StoreCD
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE CustomerCD = @CustomerCD
        AND ChangeDate = @CustomerChangeDate
        AND CustomerKBN <> 2
        AND VariousFLG = 0	--0‚Ì‚Æ‚«iNot”Œûj
        ;
    END
    
    --XVˆ—i•Ô•ij-------------------------------------------------------------
    ELSE IF @OperateMode = 4 --•Ô•i
    BEGIN
        SET @OperateModeNm = '•Ô•i';
        
        --ƒe[ƒuƒ‹“]‘—d—l‚k ‰ïˆõƒ}ƒXƒ^[ ƒ|ƒCƒ“ƒg‰ÁZiæÁj
        UPDATE M_Customer SET
            LastPoint = LastPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO) + (CASE @Haspo WHEN 1 THEN @PointAmount ELSE 0 END) --ƒ|ƒCƒ“ƒgÁ‰»iæÁj
            ,TotalPoint = TotalPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,TotalPurchase = TotalPurchase - (SELECT DS. SalesHontaiGaku FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE CustomerCD = @CustomerCD
        AND ChangeDate = @CustomerChangeDate
        AND CustomerKBN <> 2
        AND VariousFLG = 0	--0‚Ì‚Æ‚«iNot”Œûj
        ;
        
        --yD_Juchuuz
        --“`•[”Ô†Ì”Ô
        EXEC Fnc_GetNumber
            1,             --in“`•[í•Ê 1
            @ChangeDate    , --inŠî€“ú
            @StoreCD,       --in“X•ÜCD
            @Operator,
            @JuchuuNO OUTPUT
            ;
        
        IF ISNULL(@JuchuuNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END

        --ƒe[ƒuƒ‹“]‘—d—l‚e(•Ô•ij Insertó’ D_Juchuu   
        --yD_Juchuuz
        INSERT INTO [D_Juchuu]
           ([JuchuuNO]
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
           ,[Point]
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
           ,@StoreCD                          
           ,convert(date,@SYSDATETIME)                    
           ,convert(Time,@SYSDATETIME)    --JuchuuTime
           ,1   --ReturnFLGš
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            AND M.SoukoType= 3
            ORDER BY M.ChangeDate desc)

           ,2   --JuchuuKBN 2   “X“ª
           ,0   --SiteKBN
           ,NULL    --SiteJuchuuDateTime
           ,NULL    --SiteJuchuuNO
           ,0   --InportErrFLG
           ,0   --OnHoldFLG
           ,0   --IdentificationFLG
           ,NULL    --TorikomiDateTime
           ,@Operator
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL	--CustomerName2
           ,(SELECT top 1 M.AliasKBN FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.ZipCD2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address1 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Address2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel11 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel12 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel13 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,(SELECT top 1 M.Tel21 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel21
           ,(SELECT top 1 M.Tel22 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel22
           ,(SELECT top 1 M.Tel23 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --Tel23
           ,(SELECT top 1 M.KanaName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)    --CustomerKanaName
           ,NULL    --JuchuuCarrierCD
           ,0   --DecidedCarrierFLG
           ,NULL    --LastCarrierCD
           ,NULL    --NameSortingDateTime
           ,NULL    --NameSortingStaffCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.MainKey = 1)   --CurrencyCD
           ,-1* (@SalesGaku + @Discount)
           ,-1* @Discount
           ,-1* (@SalesGaku - @SalesTax)
           ,-1* @HanbaiTax8
           ,-1* @HanbaiTax10
           ,-1* @SalesGaku
           ,0   --CostGaku
           ,-1* (@SalesGaku - @SalesTax)
           ,0   --Coupon
           ,0   --Point
           ,0   --PayCharge
           ,0   --Adjustments
           ,0   --Postage
           ,0   --GiftWrapCharge
           ,-1* @InvoiceGaku
           ,NULL	--PaymentMethodCD
           ,0	--PaymentPlanNO
           ,0   --CardProgressKBN
           ,NULL    --CardCompany
           ,NULL    --CardNumber
           ,0   --PaymentProgressKBN
           ,0   --PresentFLG
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
           ,convert(date,@SYSDATETIME)
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
           ,convert(date,@SYSDATETIME)    --LastDepositeDate
           ,NULL    --LastOrderDate
           ,NULL    --LastArriveDate
           ,convert(date,@SYSDATETIME)    --LastSalesDate
           ,NULL    --MitsumoriNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL                  
           ,NULL
           );                     
                         
       --ƒe[ƒuƒ‹“]‘—d—l‚f(•Ô•ij Insertó’–¾×  D_JuchuuDetails
       INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
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
                   ,tbl.JuchuuRows                       
                   ,tbl.DisplayRows                      
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
                        WHERE M.AdminNO = tbl.AdminNO
                        AND M.ChangeDate <= convert(date,@SYSDATETIME)
                        AND M.DeleteFLG = 0
                        ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,-1* tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,-1* tbl.JuchuuGaku
                   ,-1* tbl.JuchuuHontaiGaku
                   ,-1* tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,0	--CostUnitPrice
                   ,0	--CostGaku
                   ,-1* tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@SYSDATETIME)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,-1* tbl.JuchuuSuu --HikiateSu
                   ,-1* tbl.JuchuuSuu --DeliveryOrderSu
                   ,-1* tbl.JuchuuSuu --DeliverySu
                   ,0	--DirectFLG
                   ,0 --HikiateFLG
                   ,NULL --JuchuuOrderNO
                   ,NULL	--VendorCD
                   ,NULL    --LastOrderNO
                   ,0   --LastOrderRows
                   ,NULL    --LastOrderDateTime
                   ,NULL    --DesiredDeliveryDate
                   ,0   --AnswerFLG
                   ,NULL	--ArrivePlanDate
                   ,NULL    --ArrivePlanNO
                   ,NULL    --ArriveDateTime
                   ,NULL    --ArriveNO
                   ,0   --ArribveNORows
                   ,NULL    --DeliveryPlanNO
                   ,NULL	--CommentOutStore
                   ,NULL	--CommentInStore
                   ,NULL	--IndividualClientName
                   ,NULL    --ShippingPlanDate      2020.01.30 add
                   ,NULL    --SalesDate
                   ,NULL    --SalesNO
                   ,NULL    --DepositeDetailNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
                                                                                
        --ƒe[ƒuƒ‹“]‘—d—l‚e—š—ğ    Insertó’—š—ğ  
        --ƒe[ƒuƒ‹“]‘—d—l‚f—š—ğ    Insertó’–¾×—š—ğ                                                                                      
        --ƒe[ƒuƒ‹“]‘—d—l‚`(•Ô•ij Insert”„ã D_Sales
        --“`•[”Ô†Ì”Ô
        EXEC Fnc_GetNumber
            3,          --in“`•[í•Ê 3
            @ChangeDate    , --inŠî€“ú
            @StoreCD,    --in“X•ÜCD
            @Operator,
            @SalesNO OUTPUT
            ;

        IF ISNULL(@SalesNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
                                                                               
        INSERT INTO [D_Sales]
           ([SalesNO]
           ,[StoreCD]
           ,[SalesDate]
           ,[ShippingNO]
           ,[CustomerCD]
           ,[CustomerName]
           ,[CustomerName2]
           ,[BillingType]
           ,[SalesHontaiGaku]
           ,[SalesHontaiGaku0]
           ,[SalesHontaiGaku8]
           ,[SalesHontaiGaku10]
           ,[SalesTax]
           ,[SalesTax8]
           ,[SalesTax10]
           ,[SalesGaku]
           ,[LastPoint]
           ,[WaitingPoint]
           ,[StaffCD]
           ,[PrintDate]
           ,[PrintStaffCD]
           ,[Discount]
           ,[Discount8]
           ,[Discount10]
           ,[DiscountTax]
           ,[DiscountTax8]
           ,[DiscountTax10]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[PurchaseNO]
           ,[SalesEntryKBN]
           ,[NouhinsyoComment]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
           @SalesNO
           ,@StoreCD
           ,CONVERT(date, @SYSDATETIME)
           ,NULL	--ShippingNO
           ,@CustomerCD
           ,(SELECT top 1 M.CustomerName FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)
           ,NULL
           /*(SELECT top 1 M.CustomerName2 FROM M_Customer AS M
            WHERE M.CustomerCD = @CustomerCD
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc)*/
           ,1	--BillingType
           ,(-1)*(@SalesGaku-@SalesTax)    --SalesHontaiGaku
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 0 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku0
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END)  --SalesHontaiGaku8
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuGaku - tbl.JuchuuTax ELSE 0 END) --SalesHontaiGaku10
           ,(-1)*SUM(tbl.JuchuuTax) 
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 8 THEN tbl.JuchuuTax ELSE 0 END)  --SalesTax8
           ,(-1)*SUM(CASE WHEN DJM.JuchuuTaxRitsu = 10 THEN tbl.JuchuuTax ELSE 0 END) --SalesTax10
           ,(-1)*@SalesGaku
           ,(-1)*@LastPoint
           ,0	--<WaitingPoint, money,>
           ,@Operator   --StaffCD
           ,NULL    --PrintDate
           ,NULL    --PrintStaffCD
           ,@Discount
           ,@Discount8
           ,@Discount10
           ,@DiscountTax
           ,@DiscountTax8
           ,@DiscountTax10
           ,0 AS CostGaku
           ,0 AS ProfitGaku
           ,NULL AS PurchaseNO
           ,2 AS SalesEntryKBN
           ,NULL AS NouhinsyoComment
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DJM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DJM.JuchuuRows
       WHERE DJM.JuchuuNO = @JuchuuNO
       GROUP BY DJM.JuchuuNO
       ;
   
        --ƒe[ƒuƒ‹“]‘—d—l‚a(•Ô•i)  Insert”„ã–¾×  D_SalesDetails                                                              
       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[NotPrintFLG]
           ,[AddSalesRows]
           ,[ShippingNO]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[TaniCD]
           ,[SalesHontaiGaku]
           ,[SalesTax]
           ,[SalesGaku]
           ,[SalesTaxRitsu]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[CostUnitPrice]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[DeliveryNoteFLG]
           ,[BillingPrintFLG]
           ,[PurchaseNO]
           ,[PurchaseRows]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
            @SalesNO
           ,DM.JuchuuRows
           ,DM.JuchuuNO
           ,DM.JuchuuRows
           ,0 AS NotPrintFLG
           ,0 AS AddSalesRows
           ,NULL	--ShippingNO
           ,DM.AdminNO
           ,DM.SKUCD
           ,DM.JanCD
           ,DM.SKUName
           ,DM.ColorName
           ,DM.SizeName
           ,DM.JuchuuSuu	--SalesSU
           ,DM.JuchuuUnitPrice  --SalesUnitPrice
           ,DM.TaniCD
           ,DM.JuchuuHontaiGaku    --SalesHontaiGaku
           ,DM.JuchuuTax
           ,DM.JuchuuGaku	--SalesGaku
           ,DM.JuchuuTaxRitsu   --SalesTaxRitsu
           ,0	---1* tbl.ProperGaku	--2019.12.12 add
		   ,0	---1* (tbl.ProperGaku - DM.JuchuuGaku) AS DiscountGaku	--2019.12.12 add
           ,0 AS CostUnitPrice
           ,0 AS CostGaku
           ,0 AS ProfitGaku
           ,NULL    --CommentOutStore
           ,NULL    --CommentInStore
           ,NULL  --IndividualClientName
           ,1	--<DeliveryNoteFLG, tinyint,>
           ,1	--<BillingPrintFLG, tinyint,>
           ,NULL AS PurchaseNO
           ,0 AS PurchaseRows
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_JuchuuDetails AS DM
       INNER JOIN @Table AS tbl ON tbl.JuchuuRows = DM.JuchuuRows
       WHERE DM.JuchuuNO = @JuchuuNO
       ;
 
        --ƒe[ƒuƒ‹“]‘—d—l‚`—š—ğ    Insert”„ã—š—ğ
        --ƒe[ƒuƒ‹“]‘—d—l‚a—š—ğ    Insert”„ã–¾×—š—ğ

        --ƒe[ƒuƒ‹“]‘—d—l‚g         Insert“X•Ü“ü‹à
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
                   ,[PurchaseAmount]
                   ,[TaxAmount]
                   ,[DiscountAmount]
                   ,[BillingAmount]
                   ,[PointAmount]
                   ,[CardDenominationCD]
                   ,[CardAmount]
                   ,[CashAmount]
                   ,[DepositAmount]
                   ,[RefundAmount]
                   ,[CreditAmount]
                   ,[Denomination1CD]
                   ,[Denomination1Amount]
                   ,[Denomination2CD]
                   ,[Denomination2Amount]
                   ,[AdvanceAmount]
                   ,[TotalAmount]
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT
                    @SalesNO
                   ,(SELECT ISNULL(MAX(DataNo),0)+1 FROM D_SalesTran)
                   ,@StoreCD
                   ,2	--1:’ÊíA2:•Ô•iA3;’ù³A4:æÁ
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ;

        --ƒe[ƒuƒ‹“]‘—d—l‚h        Insert”„ã„ˆÚE•                                  
        --ƒe[ƒuƒ‹“]‘—d—l‚i        Insert”„ã–¾×„ˆÚE•
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,4  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1 --SIGN int,		D_Sales‚Æ“¯‚¶•„†‚Ì‚½‚ß
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
                                                                                                                                                
        --ƒe[ƒuƒ‹“]‘—d—l‚b(•Ô•i)  Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory”„ã‚Ìƒwƒbƒ_î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,2	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@SYSDATETIME)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(-1)*(@SalesGaku - @SalesTax)	--SalesGaku, money,>
           ,(-1)*@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>š
           ,(-1)*@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
			;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚c(•Ô•i)  Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory”„ã‚Ì–¾×î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:”Ì”„î•ñA2:”Ì”„–¾×î•ñA3:“üo‹àî•ñ
           ,1   --DepositKBN 1;”Ì”„,2:“ü‹à,3;x•¥,4:—¼‘Ö“ü,5:—¼‘Öo,6:’Ş‘K€”õ,7:¤•iŒ”’Ş
           ,2   --CancelKBN 1:æÁA2:•Ô•iA3:’ù³
           ,0   --RecoredKBN 0:•A1:Ô
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@SYSDATETIME)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,tbl.JuchuuRows   --Rows, tinyint,>š
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,tbl.ProperGaku                                  --2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku --2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
          FROM @Table tbl
          WHERE tbl.UpdateFlg = 0
          ;

        --“ü‹àFƒ|ƒCƒ“ƒg‚Ìê‡
        IF @PointAmount <> 0
        BEGIN
            --ƒe[ƒuƒ‹“]‘—d—l‚d‡@(•Ô•i)insert“X•Ü“üo‹à—š—ğ@D_DepositHistoryƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,-1 --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        END

        --Œ»‹à‚ ‚è‚Ì    
        IF @CashAmount <> 0
        BEGIN
        --ƒe[ƒuƒ‹“]‘—d—l‚dè„€(•Ô•i)insert“X•Ü“üo‹à—š—ğ@D_DepositHistoryŒ»‹à‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;
        END
                
        --ƒJ[ƒh‚ ‚è‚Ì  
        IF @CardAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡B(•Ô•i)Insert“X•Ü“üo‹à—š—ğ@D_DepositHistoryƒJ[ƒh‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END

        --’lˆø‚«‚ ‚è‚Ì  
        IF @Discount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡C(•Ô•i)Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory’lˆø‚«‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;
        END
                                      
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡D(•Ô•i)Insert“X•Ü“üo‹à—š—ğ@D_DepositHistory‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì
        --‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì    ‚»‚Ì‘¼‚Ìê‡ MAX‚QƒŒƒR[ƒh
        IF @Denomination1Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        
        IF @Denomination2Amount <> 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,-1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END

        --ƒe[ƒuƒ‹“]‘—d—l‚d‡E(•Ô•i)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        Š|‚ ‚è‚Ì
        IF @CreditAmount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1          --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program   --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@CreditAmount
                ;
        END
        
        --“ü‹àF‘Oó‹à‚Ìê‡
        IF @AdvanceAmount <> 0
        BEGIN
            --ƒe[ƒuƒ‹“]‘—d—l‚d‡F(•Ô•i)insert“X•Ü“üo‹à—š—ğ@D_DepositHistoryƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,-1 --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;
        END
    END

    --XVˆ—i’ù³j-------------------------------------------------------------
    ELSE IF @OperateMode = 2 --’ù³--
    BEGIN
        SET @OperateModeNm = '’ù³';

        SELECT @OldDiscount = DS.DiscountAmount
        --  ,DS.BillingAmount
            ,@OldAdvanceAmount = DS.AdvanceAmount
            ,@OldPointAmount = DS.PointAmount
            ,@OldDenominationCD = DS.CardDenominationCD
            ,@OldCardAmount = DS.CardAmount
            ,@OldCashAmount = DS.CashAmount
        --  ,DS.DepositAmount
            ,@OldRefundAmount = DS.RefundAmount
            ,@OldCreditAmount = DS.CreditAmount
            ,@OldDenominationCD1 = DS.Denomination1CD
            ,@OldDenomination1Amount = DS.Denomination1Amount
            ,@OldDenominationCD2 = DS.Denomination2CD
            ,@OldDenomination2Amount = DS.Denomination2Amount
        FROM D_StorePayment As DS
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚k ‰ïˆõƒ}ƒXƒ^[ ƒ|ƒCƒ“ƒg‰ÁZiæÁj
        UPDATE M_Customer SET
            LastPoint = LastPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO) + (CASE @Haspo WHEN 1 THEN @OldPointAmount ELSE 0 END)
            ,TotalPoint = TotalPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,TotalPurchase = TotalPurchase - (SELECT DS. SalesHontaiGaku FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE CustomerCD = @CustomerCD
        AND ChangeDate = @CustomerChangeDate
        AND CustomerKBN <> 2
        AND VariousFLG = 0	--0‚Ì‚Æ‚«iNot”Œûj
        ;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚c(’ù³)	Insert “X•Ü“üo‹à—š—ğ@D_DepositHistory	”„ã‚Ì–¾×î•ñ	  ’ù³‘O
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:”Ì”„î•ñA2:”Ì”„–¾×î•ñA3:“üo‹àî•ñ
           ,1   --DepositKBN 1;”Ì”„,2:“ü‹à,3;x•¥,4:—¼‘Ö“ü,5:—¼‘Öo,6:’Ş‘K€”õ,7:¤•iŒ”’Ş
           ,3   --CancelKBN 1:æÁA2:•Ô•iA3:’ù³
           ,1   --RecoredKBN 0:•A1:Ô
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,DM.SalesRows   --Rows, tinyint,>
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,DM.AdminNO	--AdminNO, int,>
           ,DM.SKUCD	--SKUCD, varchar(30),>
           ,DM.JanCD	--JanCD, varchar(13),>
           ,DM.SalesSU	--SalesSU, money,>
           ,DM.SalesUnitPrice	--SalesUnitPrice, money,>
           ,DM.SalesGaku - DM.SalesTax	--SalesGaku, money,>
           ,DM.SalesTax	--SalesTax, money,>
           ,DM.SalesTaxRitsu 	--SalesTaxRate, int,>
           ,DM.SalesGaku 	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,DM.ProperGaku	--2019.12.12 add
		   ,DM.DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_SalesDetails AS DM	
       WHERE DM.SalesNO = @SalesNO
       ;

        
        IF @OldPointAmount <> 0
        BEGIN
	        --ƒe[ƒuƒ‹“]‘—d—l‚d‡@(’ù³)insert “X•Ü“üo‹à—š—ğ@D_DepositHistory ƒ|ƒCƒ“ƒg‚ ‚è‚Ì  ’ù³‘O
	        EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint, 1:æÁA2:•Ô•iA3:’ù³
                ,1	--@RecoredKBN tinyint,0:•A1:Ô
                ,@OldPointAmount
                ;
        END        
        
		--Œ»‹à‚ ‚è‚Ì    
		IF @OldCashAmount <> 0
		BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚dè„€(’ù³)insert “X•Ü“üo‹à—š—ğ@D_DepositHistory Œ»‹à‚ ‚è‚Ì      ’ù³‘O
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCashAmount
                ,@OldRefundAmount
                ;
        END
        
        --ƒJ[ƒh‚ ‚è‚Ì  
        IF @OldCardAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡B(’ù³)Insert “X•Ü“üo‹à—š—ğ@D_DepositHistory ƒJ[ƒh‚ ‚è‚Ì    ’ù³‘O
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD
                ,@OldCardAmount
                ;
        
        END
        
        --’lˆø‚«‚ ‚è‚Ì  
        IF @OldDiscount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡C(’ù³)Insert “X•Ü“üo‹à—š—ğ@D_DepositHistory ’lˆø‚«‚ ‚è‚Ì    ’ù³‘O
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,2  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDiscount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡D(’ù³)Insert “X•Ü“üo‹à—š—ğ@D_DepositHistory ‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì ’ù³‘O
        IF @OldDenomination1Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD1
                ,@OldDenomination1Amount
                ;
        END
        
        IF @OldDenomination2Amount <> 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD2
                ,@OldDenomination2Amount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡E(’ù³)Insert “X•Ü“üo‹à—š—ğ@D_DepositHistory Š|‚ ‚è‚Ì        ’ù³‘O
        IF @OldCreditAmount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCreditAmount
                ;
        END
        
		--‘Oó‹à‚ ‚è‚Ì    
		IF @OldAdvanceAmount <> 0
		BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚dè„€(’ù³)insert “X•Ü“üo‹à—š—ğ@D_DepositHistory Œ»‹à‚ ‚è‚Ì      ’ù³‘O
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@CustomerCD
                ,@OldAdvanceAmount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚e(’ù³jUpdate ó’@      D_Juchuu
        UPDATE D_Juchuu SET
               [JuchuuDate] = CONVERT(date, @Keijobi)
              ,[JuchuuGaku] =       @SalesGaku + @Discount
              ,[Discount] =         @Discount
              ,[HanbaiHontaiGaku] = @SalesGaku - @SalesTax
              ,[HanbaiTax8] =       @HanbaiTax8
              ,[HanbaiTax10] =      @HanbaiTax10
              ,[HanbaiGaku] =       @SalesGaku
              ,[CostGaku] = 0
              ,[ProfitGaku] =       @SalesGaku - @SalesTax                             
              ,[InvoiceGaku] =      @InvoiceGaku
              ,[SalesPlanDate] = CONVERT(date, @Keijobi)
              ,[FirstPaypentPlanDate] = CONVERT(date, @Keijobi)
              ,[LastPaymentPlanDate] = CONVERT(date, @Keijobi) 
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ; 
        
        --ƒe[ƒuƒ‹“]‘—d—l‚f(’ù³jDelete ó’–¾×    D_JuchuuDetails
                            --& Insert
	    IF OBJECT_ID( N'[dbo]..[#Table_D_JuchuuDetails_T]', N'U' ) IS NOT NULL
	    BEGIN
	        DROP TABLE [#Table_D_JuchuuDetails_T];
	    END
      
        SELECT * 
        INTO #Table_D_JuchuuDetails_T
        FROM [D_JuchuuDetails]
        WHERE JuchuuNO = @JuchuuNO
        ;
        
        DELETE FROM [D_JuchuuDetails]
        WHERE JuchuuNO = @JuchuuNO
        ;
        
		INSERT INTO [D_JuchuuDetails]
                   ([JuchuuNO]
                   ,[JuchuuRows]
                   ,[DisplayRows]
                   ,[SiteJuchuuRows]
                   ,[NotPrintFLG]
                   ,[AddJuchuuRows]
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
                   ,tbl.JuchuuRows
                   ,tbl.DisplayRows
                   ,0   --SiteJuchuuRows
                   ,0	--NotPrintFLG
                   ,0	--AddJuchuuRows
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,(SELECT top 1 M.SKUName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS SKUName
                    ,(SELECT top 1 M.ColorName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS ColorName
                   ,(SELECT top 1 M.SizeName FROM M_SKU AS M
	                WHERE M.AdminNO = tbl.AdminNO
	                AND M.ChangeDate <= convert(date,@SYSDATETIME)
	                AND M.DeleteFLG = 0
	                ORDER BY M.ChangeDate DESC) AS SizeName
                   ,0 AS SetKBN
                   ,0 AS SetRows
                   ,tbl.JuchuuSuu
                   ,tbl.JuchuuUnitPrice
                   ,tbl.TaniCD
                   ,tbl.JuchuuGaku
                   ,tbl.JuchuuHontaiGaku
                   ,tbl.JuchuuTax
                   ,tbl.JuchuuTaxRitsu
                   ,ISNULL(DM.CostUnitPrice,0)	--CostUnitPrice
                   ,ISNULL(DM.CostGaku,0)	--CostGaku
                   ,tbl.JuchuuHontaiGaku     --ProfitGaku
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M 
                                WHERE M.StoreCD = @StoreCD
                                AND M.ChangeDate <= convert(date,@Keijobi)
                                AND M.DeleteFlg = 0
                                AND M.SoukoType= 3
                                ORDER BY M.ChangeDate desc)
                   ,tbl.JuchuuSuu --HikiateSu
                   ,tbl.JuchuuSuu --DeliveryOrderSu
                   ,tbl.JuchuuSuu --DeliverySu
                   ,DM.DirectFLG
                   ,ISNULL(DM.HikiateFLG,0)
                   ,DM.JuchuuOrderNO
                   ,DM.VendorCD
                   ,DM.LastOrderNO
                   ,ISNULL(DM.LastOrderRows,0)
                   ,DM.LastOrderDateTime
                   ,DM.DesiredDeliveryDate
                   ,ISNULL(DM.AnswerFLG,0)
                   ,DM.ArrivePlanDate
                   ,ISNULL(DM.ArrivePlanNO,0)	--int‚Å‚¢‚¢Hš
                   ,DM.ArriveDateTime
                   ,DM.ArriveNO
                   ,ISNULL(DM.ArribveNORows,0)
                   ,ISNULL(DM.DeliveryPlanNO,0)
                   ,DM.CommentOutStore
                   ,DM.CommentInStore
                   ,DM.IndividualClientName
                   ,DM.ShippingPlanDate      --2020.01.30 add
                   ,DM.SalesDate
                   ,DM.SalesNO
                   ,DM.DepositeDetailNO
                   ,DM.InsertOperator
                   ,DM.InsertDateTime
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table AS tbl
              LEFT OUTER JOIN #Table_D_JuchuuDetails_T AS DM
              ON tbl.JuchuuRows = DM.JuchuuRows
              WHERE tbl.UpdateFlg = 0
              ;
                            
        --ƒe[ƒuƒ‹“]‘—d—l‚e—š—ğ    Insert ó’—š—ğ
        --ƒe[ƒuƒ‹“]‘—d—l‚f—š—ğ    Insert ó’–¾×—š—ğ             
        --ƒe[ƒuƒ‹“]‘—d—l‚h        Insert ”„ã„ˆÚEÔ
        --ƒe[ƒuƒ‹“]‘—d—l‚i        Insert ”„ã–¾×„ˆÚEÔ
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,2  --ProcessKBN tinyint,
            ,1	--RecoredKBN
            ,-1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚`(’ù³j Update ”„ã     D_Sales
        UPDATE D_Sales SET
               [SalesDate] = CONVERT(date, @Keijobi)
              ,[Age] = @Age	--2019.12.16 add
              ,[SalesHontaiGaku] = @SalesGaku - @SalesTax
              ,[SalesHontaiGaku0] = @SalesHontaiGaku0
              ,[SalesHontaiGaku8] = @SalesHontaiGaku8
              ,[SalesHontaiGaku10] = @SalesHontaiGaku10
              ,[SalesTax] =   @SalesTax
              ,[SalesTax8] =  @HanbaiTax8
              ,[SalesTax10] = @HanbaiTax10
              ,[SalesGaku] =  @SalesGaku
              ,[LastPoint] = @LastPoint
              ,[StaffCD] = @Operator
              ,[Discount] = @Discount
              ,[Discount8] = @Discount8
              ,[Discount10] = @Discount10
              ,[DiscountTax] = @DiscountTax
              ,[DiscountTax8] = @DiscountTax8
              ,[DiscountTax10] = @DiscountTax10
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
        
        --ƒe[ƒuƒ‹“]‘—d—l‚a(’ù³)  Delete ”„ã–¾×     D_SalesDetails      
        --          &   Insert      
        SELECT * 
        INTO #Table_D_SalesDetails
        FROM [D_SalesDetails]
        WHERE SalesNO = @SalesNO
        ;
        
        DELETE FROM [D_SalesDetails]
        WHERE SalesNO = @SalesNO
        ;

       INSERT INTO [D_SalesDetails]
           ([SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[NotPrintFLG]
           ,[AddSalesRows]
           ,[ShippingNO]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[TaniCD]
           ,[SalesHontaiGaku]
           ,[SalesTax]
           ,[SalesGaku]
           ,[SalesTaxRitsu]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[CostUnitPrice]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[IndividualClientName]
           ,[DeliveryNoteFLG]
           ,[BillingPrintFLG]
           ,[PurchaseNO]
           ,[PurchaseRows]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
            @SalesNO
           ,tbl.JuchuuRows
           ,@JuchuuNO
           ,tbl.JuchuuRows
           ,0 AS NotPrintFLG
           ,0 AS AddSalesRows
           ,DM.ShippingNO
           ,tbl.AdminNO
           ,tbl.SKUCD
           ,tbl.JanCD
           ,(SELECT top 1 M.SKUName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS SKUName
            ,(SELECT top 1 M.ColorName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS ColorName
           ,(SELECT top 1 M.SizeName FROM M_SKU AS M
            WHERE M.AdminNO = tbl.AdminNO
            AND M.ChangeDate <= convert(date,@SYSDATETIME)
            AND M.DeleteFLG = 0
            ORDER BY M.ChangeDate DESC) AS SizeName
           ,tbl.JuchuuSuu	--SalesSU
           ,tbl.JuchuuUnitPrice  --SalesUnitPrice
           ,tbl.TaniCD
           ,tbl.JuchuuHontaiGaku    --SalesHontaiGaku
           ,tbl.JuchuuTax
           ,tbl.JuchuuGaku	--SalesGaku
           ,tbl.JuchuuTaxRitsu   --SalesTaxRitsu
           ,DM.ProperGaku	--tbl.ProperGaku	--2019.12.12 add
           ,DM.DiscountGaku	--tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku	--2019.12.12 add
           ,DM.CostUnitPrice
           ,DM.CostGaku
           ,DM.ProfitGaku
           ,DM.CommentOutStore
           ,DM.CommentInStore
           ,DM.IndividualClientName
           ,ISNULL(DM.DeliveryNoteFLG,1)
           ,ISNULL(DM.BillingPrintFLG,1)
           ,DM.PurchaseNO
           ,DM.PurchaseRows
           ,ISNULL(DM.InsertOperator,@Operator)
           ,ISNULL(DM.InsertDateTime,@SYSDATETIME)
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM @Table tbl
       LEFT OUTER JOIN #Table_D_SalesDetails AS DM
       ON tbl.JuchuuRows = DM.SalesRows
       WHERE tbl.UpdateFlg = 0
       ; 	--s’Ç‰Á‚Ì’ù³‚É‚Â‚¢‚Ä—vŠm”F
        
        --ƒe[ƒuƒ‹“]‘—d—l‚g        Delete “X•Ü“ü‹à@
        DELETE FROM D_StorePayment
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;

        --ƒe[ƒuƒ‹“]‘—d—l‚g         Insert“X•Ü“ü‹à
        INSERT INTO [D_StorePayment]
                   ([SalesNO]
                   ,[SalesNORows]
                   ,[StoreCD]
                   ,[Mode]		
                   ,[PurchaseAmount]
                   ,[TaxAmount]
                   ,[DiscountAmount]
                   ,[BillingAmount]
                   ,[PointAmount]
                   ,[CardDenominationCD]
                   ,[CardAmount]
                   ,[CashAmount]
                   ,[DepositAmount]
                   ,[RefundAmount]
                   ,[CreditAmount]
                   ,[Denomination1CD]
                   ,[Denomination1Amount]
                   ,[Denomination2CD]
                   ,[Denomination2Amount]
                   ,[AdvanceAmount]
                   ,[TotalAmount]
                   ,[SalesRate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             VALUES
                   (@SalesNO
                   ,(SELECT ISNULL(MAX(DataNo),0)+1 FROM D_SalesTran)
                   ,@StoreCD
                   ,3	--1:’ÊíA2:•Ô•iA3;’ù³A4:æÁ
                   ,@SalesGaku  --PurchaseAmount
                   ,@SalesTax   --TaxAmount
                   ,@Discount   --DiscountAmount
                   ,@InvoiceGaku    --BillingAmount
                   ,@PointAmount
                   ,@DenominationCD --CardDenominationCD
                   ,@CardAmount
                   ,@CashAmount
                   ,@DepositAmount
                   ,@RefundAmount
                   ,@CreditAmount
                   ,@DenominationCD1
                   ,@Denomination1Amount
                   ,@DenominationCD2
                   ,@Denomination2Amount
                   ,@AdvanceAmount
                   ,@TotalAmount
                   ,@SalesRate
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME)
                   ;
                   
        --ƒe[ƒuƒ‹“]‘—d—l‚h        Insert ”„ã„ˆÚE•
        --ƒe[ƒuƒ‹“]‘—d—l‚i        Insert ”„ã–¾×„ˆÚE•
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,2  --ProcessKBN tinyint,
            ,0	--RecoredKBN
            ,1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;

        --ƒe[ƒuƒ‹“]‘—d—l‚b(’ù³)  Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ”„ã‚Ìƒwƒbƒ_î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 VALUES
           (@StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,2	--CancelKBN
           ,0	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@Keijobi)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(@SalesGaku - @SalesTax)	--SalesGaku, money,>
           ,@SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>š
           ,@SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME)
            ;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚c(’ù³)  Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ”„ã‚Ì–¾×î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]    --2019.12.12 add
           ,[DiscountGaku]  --2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:”Ì”„î•ñA2:”Ì”„–¾×î•ñA3:“üo‹àî•ñ
           ,1   --DepositKBN 1;”Ì”„,2:“ü‹à,3;x•¥,4:—¼‘Ö“ü,5:—¼‘Öo,6:’Ş‘K€”õ,7:¤•iŒ”’Ş
           ,3   --CancelKBN 1:æÁA2:•Ô•iA3:’ù³
           ,0   --RecoredKBN 0:•A1:Ô
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,tbl.JuchuuRows   --Rows, tinyint,>š
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,tbl.AdminNO	--AdminNO, int,>
           ,tbl.SKUCD	--SKUCD, varchar(30),>
           ,tbl.JanCD	--JanCD, varchar(13),>
           ,tbl.JuchuuSuu	--SalesSU, money,>
           ,tbl.JuchuuUnitPrice	--SalesUnitPrice, money,>
           ,tbl.JuchuuHontaiGaku	--SalesGaku, money,>
           ,tbl.JuchuuTax	--SalesTax, money,>
           ,tbl.JuchuuTaxRitsu 	--SalesTaxRate, int,>
           ,tbl.JuchuuGaku	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,tbl.ProperGaku  --2019.12.12 add
           ,tbl.ProperGaku - tbl.JuchuuGaku AS DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM @Table tbl
          WHERE tbl.UpdateFlg = 0
       ;
        
        
        IF @PointAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡@(’ù³)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@PointAmount
                ;
        
        END
        
        --Œ»‹à‚ ‚è‚Ì    
        IF @CashAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚dè„€'(’ù³)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        Œ»‹à‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --RecoredKBN
                ,@CashAmount
                ,@RefundAmount
                ;
        END
        
        
        --ƒJ[ƒh‚ ‚è‚Ì  
        IF @CardAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡B(’ù³)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ƒJ[ƒh‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@DenominationCD
                ,@CardAmount
                ;
        END

        
        --’lˆø‚«‚ ‚è‚Ì  
        IF @Discount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡C(’ù³)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ’lˆø‚«‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@Discount
                ;        
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡D(’ù³)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì
        IF @Denomination1Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO        -- varchar(11),
                ,1              --@SIGN ,
                ,@Operator      --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program       --varchar(50)
                ,@StoreCD       -- varchar(4),
                ,3              --@CancelKBN tinyint,
                ,0              --@RecoredKBN
                ,@DenominationCD1
                ,@Denomination1Amount
                ;
        END
        
        IF @Denomination2Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program       --varchar(50)
                ,@StoreCD       -- varchar(4),
                ,3              --@CancelKBN tinyint,
                ,0              --@RecoredKBN
                ,@DenominationCD2
                ,@Denomination2Amount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡E(’ù³)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        Š|‚ ‚è‚Ì
        IF @CreditAmount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0  --@RecoredKBN
                ,@CreditAmount
                ;
        END
        
        IF @AdvanceAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡F(’ù³)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory        ƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,3  --@CancelKBN tinyint,
                ,0	--@RecoredKBN tinyint,
                ,@CustomerCD
                ,@AdvanceAmount
                ;        
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚k ‰ïˆõƒ}ƒXƒ^[
        UPDATE M_Customer SET
            LastPoint = LastPoint + @LastPoint - (CASE @Haspo WHEN 1 THEN @PointAmount ELSE 0 END)
            ,TotalPoint = TotalPoint + @LastPoint
            ,TotalPurchase = TotalPurchase + @SalesGaku-@SalesTax
            ,LastSalesDate = @ChangeDate
            ,LastSalesStoreCD = @StoreCD
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE CustomerCD = @CustomerCD
        AND ChangeDate = @CustomerChangeDate
        AND CustomerKBN <> 2
        AND VariousFLG = 0	--0‚Ì‚Æ‚«iNot”Œûj
        ;
    END
    
    ELSE IF @OperateMode = 3 --æÁ---------------------------------------------------------------
    BEGIN
        SET @OperateModeNm = 'æÁ';

        --ƒe[ƒuƒ‹“]‘—d—l‚k ‰ïˆõƒ}ƒXƒ^[ ƒ|ƒCƒ“ƒg‰ÁZiæÁj
        UPDATE M_Customer SET
            LastPoint = LastPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO) + (CASE @Haspo WHEN 1 THEN @PointAmount ELSE 0 END)
            ,TotalPoint = TotalPoint - (SELECT DS.LastPoint FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,TotalPurchase = TotalPurchase - (SELECT DS. SalesHontaiGaku FROM D_Sales AS DS WHERE DS.SalesNO = @SalesNO)
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE CustomerCD = @CustomerCD
        AND ChangeDate = @CustomerChangeDate
        AND CustomerKBN <> 2
        AND VariousFLG = 0	--0‚Ì‚Æ‚«iNot”Œûj
        ;
        
        --ƒe[ƒuƒ‹“]‘—d—l‚e(æÁj Update  ó’@D_Juchuu
        --yD_Juchuuz
        UPDATE D_Juchuu
           SET [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ;   
        
        --ƒe[ƒuƒ‹“]‘—d—l‚f(æÁj Update  ó’–¾×D_JuchuuDetails
        --yD_JuchuuDetailsz
        UPDATE D_JuchuuDetails
           SET [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE JuchuuNO = @JuchuuNO
           ;   
        
        --ƒe[ƒuƒ‹“]‘—d—l‚e—š—ğ    Insert  ó’—š—ğ
        --ƒe[ƒuƒ‹“]‘—d—l‚f—š—ğ    Insert  ó’–¾×—š—ğ
        --ƒe[ƒuƒ‹“]‘—d—l‚h        Insert      ”„ã„ˆÚEÔ
        --ƒe[ƒuƒ‹“]‘—d—l‚i        Insert      ”„ã–¾×„ˆÚEÔ
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,3  --ProcessKBN tinyint,
            ,1	--RecoredKBN
            ,-1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@Keijobi
            ;
            
        --ƒe[ƒuƒ‹“]‘—d—l‚`(æÁj Update  ”„ãD_Sales 
        UPDATE D_Sales SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
                    
        --ƒe[ƒuƒ‹“]‘—d—l‚a(æÁ)  Update  ”„ã–¾×D_SalesDetails
        UPDATE D_SalesDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 
                      
        --ƒe[ƒuƒ‹“]‘—d—l‚b(æÁ)  Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory”„ã‚Ìƒwƒbƒ_î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
    	 SELECT
            @StoreCD
           ,@SYSDATETIME	--DepositDateTime
           ,1	--DataKBN
           ,1	--DepositKBN
           ,1	--CancelKBN
           ,1	--RecoredKBN
           ,NULL	--DenominationCD
           ,0	--DepositGaku
           ,NULL	--Remark
           ,convert(date,@Keijobi)	--AccountingDate, date,>
           ,@SalesNO	--Number, varchar(11),>
           ,0	--Rows, tinyint,>
           ,0	--ExchangeMoney, money,>
           ,0	--ExchangeDenomination, int,>
           ,0	--ExchangeCount, int,>
           ,NULL	--AdminNO, int,>
           ,NULL	--SKUCD, varchar(30),>
           ,NULL	--JanCD, varchar(13),>
           ,0	--SalesSU, money,>
           ,0	--SalesUnitPrice, money,>
           ,(DH.SalesGaku - DH.SalesTax)	--SalesGaku, money,>
           ,DH.SalesTax	--SalesTax, money,>
           ,0	--<SalesTaxRate, int,>
           ,DH.SalesGaku		--TotalGaku, money,>
           ,0	--Refund, money,>
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_Sales AS DH
       WHERE DH.SalesNO = @SalesNO
			;
         
        --ƒe[ƒuƒ‹“]‘—d—l‚c(æÁ)  Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory”„ã‚Ì–¾×î•ñ
        INSERT INTO [D_DepositHistory]
           ([StoreCD]
           ,[DepositDateTime]
           ,[DataKBN]
           ,[DepositKBN]
           ,[CancelKBN]
           ,[RecoredKBN]
           ,[DenominationCD]
           ,[DepositGaku]
           ,[Remark]
           ,[AccountingDate]
           ,[Number]
           ,[Rows]
           ,[ExchangeMoney]
           ,[ExchangeDenomination]
           ,[ExchangeCount]
           ,[AdminNO]
           ,[SKUCD]
           ,[JanCD]
           ,[SalesSU]
           ,[SalesUnitPrice]
           ,[SalesGaku]
           ,[SalesTax]
           ,[SalesTaxRate]
           ,[TotalGaku]
           ,[Refund]
           ,[ProperGaku]	--2019.12.12 add
		   ,[DiscountGaku]	--2019.12.12 add
           ,[IsIssued]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     	SELECT
           @StoreCD
           ,@SYSDATETIME  --DepositDateTime
           ,2   --DataKBN 1:”Ì”„î•ñA2:”Ì”„–¾×î•ñA3:“üo‹àî•ñ
           ,1   --DepositKBN 1;”Ì”„,2:“ü‹à,3;x•¥,4:—¼‘Ö“ü,5:—¼‘Öo,6:’Ş‘K€”õ,7:¤•iŒ”’Ş
           ,1   --CancelKBN 1:æÁA2:•Ô•iA3:’ù³
           ,1   --RecoredKBN 0:•A1:Ô
           ,NULL   --DenominationCD
           ,0   --DepositGaku
           ,NULL    --Remark
           ,convert(date,@Keijobi)  --AccountingDate, date,>
           ,@SalesNO    --Number, varchar(11),>
           ,DM.SalesRows   --Rows, tinyint,>š
           ,0   --ExchangeMoney, money,>
           ,0   --ExchangeDenomination, int,>
           ,0   --ExchangeCount, int,>
           ,DM.AdminNO	--AdminNO, int,>
           ,DM.SKUCD	--SKUCD, varchar(30),>
           ,DM.JanCD	--JanCD, varchar(13),>
           ,DM.SalesSU	--SalesSU, money,>
           ,DM.SalesUnitPrice	--SalesUnitPrice, money,>
           ,DM.SalesGaku - DM.SalesTax	--SalesGaku, money,>
           ,DM.SalesTax	--SalesTax, money,>
           ,DM.SalesTaxRitsu 	--SalesTaxRate, int,>
           ,DM.SalesGaku  	--TotalGaku, money,>
           ,0	--Refund, money,>
           ,DM.ProperGaku	--2019.12.12 add
		   ,DM.ProperGaku - DM.SalesGaku AS DiscountGaku	--2019.12.12 add
           ,0	--IsIssued, tinyint,>
           ,@Program	--Program, varchar(100),>
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
       FROM D_SalesDetails AS DM	--šSalesDetails‚É•ÏX•K—v‚©‚à
          INNER JOIN @Table tbl
          ON tbl.JuchuuRows = DM.SalesRows
          WHERE tbl.UpdateFlg = 0
          AND DM.SalesNO = @SalesNO
       ;

        SELECT @OldDiscount = DS.DiscountAmount
        --  ,DS.BillingAmount
            ,@OldAdvanceAmount = DS.AdvanceAmount
            ,@OldPointAmount = DS.PointAmount
            ,@OldDenominationCD = DS.CardDenominationCD
            ,@OldCardAmount = DS.CardAmount
            ,@OldCashAmount = DS.CashAmount
        --  ,DS.DepositAmount
            ,@OldRefundAmount = DS.RefundAmount
            ,@OldCreditAmount = DS.CreditAmount
            ,@OldDenominationCD1 = DS.Denomination1CD
            ,@OldDenomination1Amount = DS.Denomination1Amount
            ,@OldDenominationCD2 = DS.Denomination2CD
            ,@OldDenomination2Amount = DS.Denomination2Amount
        FROM D_StorePayment As DS
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
        
        IF @OldPointAmount <> 0
        BEGIN
            --ƒe[ƒuƒ‹“]‘—d—l‚d‡@(æÁ)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistoryƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Point
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint, 1:æÁA2:•Ô•iA3:’ù³
                ,1  --@RecoredKBN tinyint,0:•A1:Ô
                ,@OldPointAmount
                ;
        END 
         
        
        --Œ»‹à‚ ‚è‚Ì    
        IF @OldCashAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚dè„€'(æÁ)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistoryŒ»‹à‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Cash
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCashAmount
                ,@OldRefundAmount
                ;
		END
		                
        --ƒJ[ƒh‚ ‚è‚Ì  
        IF @OldCardAmount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡B(æÁ)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistoryƒJ[ƒh‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Card
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD
                ,@OldCardAmount
                ;
        END
        
        --’lˆø‚«‚ ‚è‚Ì  
        IF @OldDiscount <> 0
        BEGIN
        	--ƒe[ƒuƒ‹“]‘—d—l‚d‡C(æÁ)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory’lˆø‚«‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Discount
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDiscount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡D(æÁ)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistory‚»‚Ì‘¼‡@A‡A‚ ‚è‚Ì
        IF @OldDenomination1Amount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD1
                ,@OldDenomination1Amount
                ;
        END
        
        IF @OldDenomination2Amount <> 0
        BEGIN	
        	EXEC INSERT_D_DepositHistory_Other
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldDenominationCD2
                ,@OldDenomination2Amount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚d‡E(æÁ)Insert  “X•Ü“üo‹à—š—ğ@D_DepositHistoryŠ|‚ ‚è‚Ì
        IF @OldCreditAmount <> 0
        BEGIN
        	EXEC INSERT_D_DepositHistory_Credit
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint,
                ,1  --@RecoredKBN
                ,@OldCreditAmount
                ;
        END

        IF @OldAdvanceAmount <> 0
        BEGIN
            --ƒe[ƒuƒ‹“]‘—d—l‚d‡F(æÁ)insert  “X•Ü“üo‹à—š—ğ@D_DepositHistoryƒ|ƒCƒ“ƒg‚ ‚è‚Ì
            EXEC INSERT_D_DepositHistory_Advance
                @SalesNO    -- varchar(11),
                ,1  --@SIGN ,
                ,@Operator  --varchar(10),
                ,@SYSDATETIME   --  datetime,
                ,@Keijobi
                ,@Program --varchar(50)
                ,@StoreCD   -- varchar(4),
                ,1  --@CancelKBN tinyint, 1:æÁA2:•Ô•iA3:’ù³
                ,1  --@RecoredKBN tinyint,0:•A1:Ô
                ,@CustomerCD
                ,@OldAdvanceAmount
                ;
        END
        
        --ƒe[ƒuƒ‹“]‘—d—l‚g        Delete “X•Ü“ü‹à@
        DELETE FROM D_StorePayment
        WHERE SalesNO = @SalesNO
        AND StoreCD = @StoreCD
        ;
    END
    
    --ˆ——š—ğƒf[ƒ^‚ÖXV
    SET @KeyItem = @SalesNO;
        
    --Table“]‘—d—l‚y InsertL_Log 
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutSalesNO = @SalesNO;
    
--<<OWARI>>
  return @W_ERR;

END


GO


