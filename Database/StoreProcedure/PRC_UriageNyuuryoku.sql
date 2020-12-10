

DROP  PROCEDURE [dbo].[D_Sales_SelectDataForUriageNyuuryoku]
GO
DROP  PROCEDURE [dbo].[CheckSalesData]
GO
DROP  PROCEDURE [dbo].[PRC_UriageNyuuryoku]
GO
DROP TYPE [dbo].[T_Uriage]
GO

--  ======================================================================
--       Program Call    îÑè„ì¸óÕ
--       Program ID      UriageNyuuryoku
--       Create date:    2020.8.19
--    ======================================================================
CREATE PROCEDURE D_Sales_SelectDataForUriageNyuuryoku
    (@OperateMode    tinyint,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
    @SalesNO varchar(11),
    @Tennic tinyint
    )AS
    
--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
   SELECT DH.SalesNO
         ,DH.StoreCD
         ,CONVERT(varchar,DH.SalesDate,111) AS SalesDate
         ,DH.ShippingNO
         ,DH.CustomerCD
         ,DH.CustomerName
         ,DH.CustomerName2
         ,DH.BillingType
         ,DH.SalesHontaiGaku
         ,DH.SalesHontaiGaku0
         ,DH.SalesHontaiGaku8
         ,DH.SalesHontaiGaku10
         ,DH.SalesTax
         ,DH.SalesTax8
         ,DH.SalesTax10
         ,DH.SalesGaku
         ,DH.LastPoint
         ,DH.WaitingPoint
         ,DH.StaffCD
         ,CONVERT(varchar,DH.PrintDate,111) AS PrintDate
         ,DH.PrintStaffCD
         ,DH.Discount
         ,DH.CostGaku
         ,DH.ProfitGaku
         ,DH.PurchaseNO
         ,DH.SalesEntryKBN

         ,DH.NouhinsyoComment
         
         ,DH.InsertOperator
         ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
         ,DH.UpdateOperator
         ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
         ,DH.DeleteOperator
         ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
         
         ,DM.SalesRows
         ,DM.JuchuuNO
         ,DM.JuchuuRows
         ,DM.NotPrintFLG
         ,DM.ShippingNO
         ,DM.AdminNO
         ,DM.SKUCD
         ,DM.JanCD
         ,DM.SKUName
         ,DM.ColorName
         ,DM.SizeName
         ,DM.SalesSU
         ,DM.SalesUnitPrice
         ,DM.TaniCD
         ,DM.SalesHontaiGaku AS D_SalesHontaiGaku
         ,DM.SalesTax AS D_SalesTax
         ,DM.SalesGaku AS D_SalesGaku
         ,DM.SalesTaxRitsu
         ,DM.ProperGaku
         ,DM.DiscountGaku
         ,DM.CostUnitPrice
         ,DM.CostGaku AS D_CostGaku
         ,DM.ProfitGaku AS D_ProfitGaku
         ,DM.CommentOutStore
         ,DM.CommentInStore
         ,DM.IndividualClientName
         ,DM.DeliveryNoteFLG
         ,DM.BillingPrintFLG
         ,DM.PurchaseNO
         ,DM.PurchaseRows
         
         ,DP.VendorCD
         ,(SELECT top 1 M.PayeeCD 
           FROM M_Vendor AS M 
           WHERE M.VendorCD = DP.VendorCD 
           AND M.ChangeDate <= DH.SalesDate
           AND M.DeleteFlg = 0
           ORDER BY M.ChangeDate desc) AS PayeeCD
         ,DP.PaymentPlanDate
         ,DP.PurchaseGaku
         ,DP.PurchaseTax
         ,DPM.PurchaserUnitPrice
         ,DPM.PurchaseGaku AS D_PurchaseGaku
         ,DC.PaymentMethodCD
         ,DBM.BillingNO

    FROM D_Sales DH

    LEFT OUTER JOIN D_SalesDetails    AS DM  ON DH.SalesNO = DM.SalesNO AND DM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_PurchaseDetails AS DPM ON DPM.PurchaseNO = DM.PurchaseNO AND DPM.PurchaseRows = DM.PurchaseRows AND DPM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Purchase        AS DP  ON DP.PurchaseNO = DPM.PurchaseNO AND DP.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_CollectPlan     AS DC  ON DH.SalesNO = DC.SalesNO AND DC.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_BillingDetails  AS DBM ON DBM.SalesNO = DM.SalesNO AND DBM.SalesRows = DM.SalesRows AND DBM.DeleteDateTime IS NULL
    WHERE DH.SalesNO = @SalesNO 
--              AND DH.DeleteDateTime IS Null
    ORDER BY DH.SalesNO, DM.SalesRows
    ;

END

GO


CREATE PROCEDURE CheckSalesData
    (@SalesNO varchar(11),
    @PurchaseNO  varchar(11),
    @StoreCD   varchar(4)
    )AS
    
--********************************************--
--                                            --
--                 èàóùäJén                   --
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
    
    --ä˘Ç…ì¸ã‡è¡çûçœÇ›ÇÃèÍçáÅAÉGÉâÅ[
    --à»â∫ÇÃèåèÇ≈ÉåÉRÅ[ÉhÇ™Ç†ÇÍÇŒì¸ã‡çœÇ∆ÇµÇƒÉGÉâÅ[ÉÅÉbÉZÅ[ÉWÇï\é¶Ç∑ÇÈ
    SELECT @CNT = COUNT(A.SalesNO)
    FROM D_Sales AS A
    INNER JOIN D_CollectPlan AS B
    ON B.SalesNO = A.SalesNO
    INNER JOIN (SELECT DCB.CollectPlanNO
            FROM D_PaymentConfirm AS DP
            LEFT OUTER JOIN D_CollectBilling AS DCB
            ON DCB.ConfirmNO = DP.ConfirmNO
            AND DCB.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectBillingDetails AS DCBD
            ON DCBD.ConfirmNO = DCB.ConfirmNO
            AND DCBD.CollectPlanNO = DCB.CollectPlanNO
            AND DCBD.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlanDetails AS DCPD
            ON DCPD.CollectPlanNO = DCBD.CollectPlanNO
            AND DCPD.CollectPlanRows = DCBD.CollectPlanRows
            AND DCPD.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_CollectPlan AS DCP
            ON DCP.CollectPlanNO = DCPD.CollectPlanNO
            AND DCP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Collect AS DC
            ON DC.CollectNO = DP.CollectNO
            AND DC.DeleteDateTime IS NULL
            WHERE DCP.StoreCD = @StoreCD
            GROUP BY DCB.CollectPlanNO
        ) AS C
    ON C.CollectPlanNO = B.CollectPlanNO
    WHERE A.SalesNO = @SalesNO
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E246';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
    
    --í˜èàóùçœÉ`ÉFÉbÉN
    --D_PayPlanÇ…ÅAéxï•í˜î‘çÜÇ™ÉZÉbÉgÇ≥ÇÍÇƒÇ¢ÇÍÇŒÅAÉGÉâÅ[Åiâ∫ãLÇÃSelectÇ™Ç≈Ç´ÇΩÇÁÉGÉâÅ[Åj
    SELECT @CNT = COUNT(A.Number)
    FROM D_PayPlan A
    WHERE A.Number = @PurchaseNO
    AND A.PayPlanKBN = 1
    AND A.PayCloseNO IS NOT NULL
    AND A.DeleteDateTime IS NULL
    ;

    IF @CNT > 0 
    BEGIN
        SET @ERRNO = 'E176';
        SELECT @ERRNO AS errno;
        RETURN;
    END;
   
    SELECT @ERRNO AS errno;

END

GO

CREATE TYPE T_Uriage AS TABLE
    (
    [SalesRows] [int],
    [DisplayRows] [int],
    [SiteJuchuuRows] [int] ,
    [NotPrintFLG] [tinyint] ,
    [AddSalesRows] [int],
    [AdminNO] [int] ,
    [SKUCD] [varchar](30) ,
    [JanCD] [varchar](13) ,
    [MakerItem] [varchar](50) ,
    [SKUName] [varchar](80) ,
    [ColorName] [varchar](20) ,
    [SizeName] [varchar](20) ,
    
    [DiscountKbn] [tinyint] ,
    [SalesSu] [int] ,
    [SalesUnitPrice] [money] ,
    [TaniCD] [varchar](2) ,
    [SalesGaku] [money] ,
    [SalesHontaiGaku] [money] ,
    [SalesTax] [money] ,
    [SalesTaxRitsu] [int] ,
    [CostUnitPrice] [money] ,
    [CostGaku] [money] ,
    [ProfitGaku] [money] ,
    [VendorCD] [varchar](13) ,
    [PaymentPlanDate] [date] ,
    [CommentOutStore] [varchar](80) ,
    [CommentInStore] [varchar](80) ,
    [IndividualClientName] [varchar](80) ,

	[PayeeCD] [varchar](13) ,
    [OrderUnitPrice] [money] ,
    [OrderTax] [money] ,
    [OrderKeigenTax] [money] ,
    [OrderGaku] [money] ,
    [PurchaseNO] [varchar](11) ,
    --[BillingNO] [varchar](11) ,
    [UpdateFlg][tinyint]
    )
GO

CREATE PROCEDURE PRC_UriageNyuuryoku
    (@OperateMode    int,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
    @SalesNO   varchar(11),
    @PurchaseNO  varchar(11),
    @BillingNO  varchar(11),
    @StoreCD   varchar(4),
    @SalesDate  varchar(10),
    @BillingType tinyint ,
    @ReturnFLG tinyint ,
    @StaffCD   varchar(10),
    @CustomerCD   varchar(13),
    @CustomerName   varchar(80),
    @CustomerName2   varchar(40),
    @BillingCD       varchar(13),
    @CollectPlanDate  varchar(10),
    @PaymentPlanDate varchar(10),
    
    @SalesGaku money ,
    --@Discount money ,
    @SalesHontaiGaku money ,
--    @SalesHontaiGaku0 money ,
--    @SalesHontaiGaku8 money ,
--    @SalesHontaiGaku10 money ,

    @SalesTax8 money ,
    @SalesTax10 money ,
    @CostGaku money ,
    @ProfitGaku money ,
    @PaymentMethodCD varchar(3) ,
    @NouhinsyoComment varchar(700),

    @Table  T_Uriage READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutSalesNO varchar(500) OUTPUT
)AS

--********************************************--
--                                            --
--                 èàóùäJén                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    --DECLARE @PurchaseNO varchar(11);
    --DECLARE @BillingNO  varchar(11);
    DECLARE @Program varchar(20);
    DECLARE @Tennic tinyint;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @Program = 'UriageNyuuryoku';
    SET @OutSalesNO = '';
    
    SET @Tennic = (SeLECT M.Tennic FROM M_Control AS M WHERE M.MainKey = 1);
    
    --ÉJÅ[É\ÉãíËã`
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.VendorCD, tbl.SalesRows, tbl.UpdateFlg
        	,ROW_NUMBER() OVER(PARTITION BY tbl.VendorCD ORDER BY tbl.SalesRows) AS RowNO
        FROM @Table AS tbl
        ORDER BY tbl.VendorCD, tbl.SalesRows
        ;
    
    DECLARE @tblVendorCD varchar(13);
    DECLARE @OldtblVendorCD varchar(13);
    DECLARE @tblSalesRows int;
    DECLARE @tblRows int;
    DECLARE @tblUpdateFlg int;
    
    SET @OldtblVendorCD = '';

    --ïœçX--
    IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = 'ïœçX';
        
        --ÅyD_PurchaseHistoryÅzInsertÅ@Tableì]ëóédólÇbÅ@ê‘
        --ÅyD_PurchaseDetailsHistoryÅzInsertÅ@Tableì]ëóédólÇcÅ@ê‘
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;

        --ÅyD_PurchaseÅzUpdate Tableì]ëóédólÇ`
        UPDATE D_Purchase
           SET [StoreCD] = @StoreCD                         
              ,[PurchaseDate] = convert(date,@SalesDate)
              ,[VendorCD] = det.VendorCD
              ,[CalledVendorCD] = det.VendorCD
              ,[CalculationGaku] = @SalesGaku
              ,[AdjustmentGaku] = 0
              ,[PurchaseGaku] = @SalesGaku
              ,[PurchaseTax] = @SalesTax8 + @SalesTax10
              ,[TotalPurchaseGaku] = @SalesGaku + @SalesTax8 + @SalesTax10
              ,[InputDate]  = @SYSDATETIME
              ,[StaffCD]         = @StaffCD
              ,[PaymentPlanDate] = @PaymentPlanDate      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         FROM (SELECT top 1 tbl.VendorCD FROM @Table AS tbl) AS det
         WHERE PurchaseNO = @PurchaseNO
           ;
           
        UPDATE [D_PurchaseDetails]
           SET [DisplayRows] = tbl.DisplayRows        
               ,[SKUCD]              = tbl.SKUCD
               ,[AdminNO]            = tbl.AdminNO
               ,[JanCD]              = tbl.JanCD
               ,[MakerItem]          = tbl.MakerItem
               ,[ItemName]           = tbl.SKUName
               ,[ColorName]          = tbl.ColorName
               ,[SizeName]           = tbl.SizeName
               ,[PurchaseSu]         = tbl.SalesSu
               ,[TaniCD]             = tbl.TaniCD
               ,[TaniName]           = (SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 201 AND M.[Key] = tbl.TaniCD)
               ,[PurchaserUnitPrice] = tbl.OrderUnitPrice
               ,[CalculationGaku]    = tbl.OrderGaku
               ,[PurchaseGaku]       = tbl.OrderGaku
               ,[PurchaseTax]        = tbl.OrderTax
               ,[TotalPurchaseGaku]  = tbl.OrderGaku + tbl.OrderTax --TotalPurchaseGaku
               ,[CurrencyCD]         = (SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)	--CurrencyCD
               ,[TaxRitsu]           = tbl.SalesTaxRitsu
               ,[CommentOutStore]    = tbl.CommentOutStore
               ,[CommentInStore]     = tbl.CommentInStore         
              -- ,[StockNO]            = @StockNO	ïœçXÇ»Çµ
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM @Table tbl
        WHERE @PurchaseNO = D_PurchaseDetails.PurchaseNO
         AND tbl.SalesRows = D_PurchaseDetails.PurchaseRows
         AND tbl.UpdateFlg = 1
         ;
        
        --çsí«â¡ï™
       --D_PurchaseDetails     Insert  Tableì]ëóédólÇa
        INSERT INTO [D_PurchaseDetails]
               ([PurchaseNO]
               ,[PurchaseRows]
               ,[DisplayRows]
               ,[ArrivalNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[MakerItem]
               ,[ItemName]
               ,[ColorName]
               ,[SizeName]
               ,[Remark]
               ,[PurchaseSu]
               ,[TaniCD]
               ,[TaniName]
               ,[PurchaserUnitPrice]
               ,[CalculationGaku]
               ,[AdjustmentGaku]
               ,[PurchaseGaku]
               ,[PurchaseTax]
               ,[TotalPurchaseGaku]
               ,[CurrencyCD]
               ,[TaxRitsu]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[ReturnNO]
               ,[ReturnRows]
               ,[OrderUnitPrice]
               ,[OrderNO]
               ,[OrderRows]
               ,[StockNO]
               ,[DifferenceFlg]
               ,[DeliveryNo]

               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
         SELECT @PurchaseNO                         
               ,tbl.SalesRows AS PurchaseRows                       
               ,tbl.DisplayRows 
               ,NULL    --ArrivalNO   
               ,tbl.SKUCD
               ,tbl.AdminNO
               ,tbl.JanCD
               ,tbl.MakerItem
               ,tbl.SKUName AS ItemName
               ,tbl.ColorName
               ,tbl.SizeName
               ,NULL    --Remark
               ,tbl.SalesSu AS PurchaseSu
               ,tbl.TaniCD
               ,(SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 201 AND M.[Key] = tbl.TaniCD) AS TaniName
               ,tbl.OrderUnitPrice
               ,tbl.OrderGaku
               ,0   --AdjustmentGaku
               ,tbl.OrderGaku
               ,tbl.OrderTax
               ,tbl.OrderGaku + tbl.OrderTax+ tbl.OrderKeigenTax --TotalPurchaseGaku
               ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)   --CurrencyCD
               ,tbl.SalesTaxRitsu AS TaxRitsu
               ,NULL    --CommentOutStore
               ,NULL    --CommentInStore
               ,NULL    --ReturnNO
               ,NULL    --ReturnRows
               ,0       --OrderUnitPrice
               ,NULL    --OrderNO
               ,0       --OrderRows
               ,NULL    --StockNO
               ,1       --DifferenceFlg
               ,NULL    --DeliveryNo
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

          FROM @Table AS tbl
          WHERE tbl.UpdateFlg = 0
          ;
        
        --çsçÌèúï™
        --Tableì]ëóédólÇ`áA
        UPDATE [D_PurchaseDetails]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE NOT EXISTS(SELECT 1 FROM @Table AS tbl
                    WHERE tbl.SalesRows = [D_PurchaseDetails].PurchaseRows)
         AND [D_PurchaseDetails].DeleteDateTime IS NULL
         AND [D_PurchaseDetails].PurchaseNO = @PurchaseNO
         ;
        
        --D_PurchaseHistory     Insert  Tableì]ëóédólÇb
        --D_PurchaseDetailsHistory  Insert  Tableì]ëóédólÇc
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,1  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;

        --ÅyD_PayPlanÅzUpdateÅ@Tableì]ëóédólÇd éxï•ó\íË
        UPDATE [D_PayPlan]
           SET 
            [PayeeCD]            = tbl.PayeeCD
           ,[RecordedDate]       = @SalesDate   --RecordedDate
           ,[PayPlanDate]        = @PaymentPlanDate    --PayPlanDate
           ,[PayPlanGaku]        = tbl.PayPlanGaku    --PayPlanGaku
           ,[UpdateOperator]     = @Operator  
           ,[UpdateDateTime]     = @SYSDATETIME
        FROM (SELECT tbl.PayeeCD, SUM(tbl.OrderGaku + tbl.OrderTax+ tbl.OrderKeigenTax) As PayPlanGaku
            FROM @Table AS tbl
            GROUP BY tbl.PayeeCD) AS tbl
        WHERE [Number] = @PurchaseNO
        AND [DeleteDateTime] IS NULL           
           ;
           
        --Update Tableì]ëóédólÇe
        UPDATE D_Sales SET
            [StoreCD]           = @StoreCD
           ,[SalesDate]         = CONVERT(date, @SalesDate)
           ,[CustomerCD]        = @CustomerCD
           ,[CustomerName]      = @CustomerName
           ,[CustomerName2]     = @CustomerName2
           ,[BillingType]       = @BillingType
           ,[SalesHontaiGaku]   = tbl.SalesHontaiGaku
           ,[SalesHontaiGaku0]  = tbl.SalesHontaiGaku0
           ,[SalesHontaiGaku8]  = tbl.SalesHontaiGaku8
           ,[SalesHontaiGaku10] = tbl.SalesHontaiGaku10
           ,[SalesTax]          = tbl.SalesTax
           ,[SalesTax8]         = tbl.SalesTax8
           ,[SalesTax10]        = tbl.SalesTax10
           ,[SalesGaku]         = tbl.SalesGaku
           ,[StaffCD]           = @Operator   --StaffCD
           ,[CostGaku]          = tbl.CostGaku
           ,[ProfitGaku]        = tbl.ProfitGaku
           ,[PurchaseNO]        = @PurchaseNO
           ,[NouhinsyoComment]  = @NouhinsyoComment
           ,[UpdateOperator]    = @Operator
           ,[UpdateDateTime]    = @SYSDATETIME
        FROM (SELECT SUM(tbl.SalesHontaiGaku) AS SalesHontaiGaku
                    ,SUM(CASE tbl.SalesTaxRitsu WHEN 0 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku0
                    ,SUM(CASE tbl.SalesTaxRitsu WHEN 8 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku8
                    ,SUM(CASE tbl.SalesTaxRitsu WHEN 10 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku10
                    ,SUM(tbl.SalesTax) AS SalesTax
                    ,SUM(CASE tbl.SalesTaxRitsu WHEN 8 THEN tbl.SalesTax ELSE 0 END) AS SalesTax8
                    ,SUM(CASE tbl.SalesTaxRitsu WHEN 10 THEN tbl.SalesTax ELSE 0 END) AS SalesTax10
                    ,SUM(tbl.SalesGaku) AS SalesGaku
                    ,SUM(tbl.CostGaku) AS CostGaku
                    ,SUM(tbl.ProfitGaku) AS ProfitGaku
        
            FROM @Table tbl
            ) AS tbl
        WHERE @SalesNO = D_Sales.SalesNO
         ;

        --Update Tableì]ëóédólÇf
        UPDATE D_SalesDetails SET
            [AdminNO]               = tbl.AdminNO
           ,[SKUCD]                 = tbl.SKUCD
           ,[JanCD]                 = tbl.JanCD
           ,[SKUName]               = tbl.SKUName
           ,[ColorName]             = tbl.ColorName
           ,[SizeName]              = tbl.SizeName
           ,[SalesSU]               = tbl.SalesSu        --SalesSU
           ,[SalesUnitPrice]        = tbl.SalesUnitPrice  --SalesUnitPrice
           ,[TaniCD]                = tbl.TaniCD
           ,[SalesHontaiGaku]       = tbl.SalesHontaiGaku   --SalesHontaiGaku
           ,[SalesTax]              = tbl.SalesTax        --SalesTax
           ,[SalesGaku]             = tbl.SalesGaku       --SalesGaku
           ,[SalesTaxRitsu]         = tbl.SalesTaxRitsu   --SalesTaxRitsu
           ,[ProperGaku]            = tbl.SalesGaku --AS ProperGaku
           ,[DiscountGaku]          = (CASE tbl.DiscountKbn WHEN 1 THEN tbl.SalesGaku ELSE 0 END) --As DiscountGaku
           ,[CostUnitPrice]         = tbl.CostUnitPrice
           ,[CostGaku]              = tbl.CostGaku
           ,[ProfitGaku]            = tbl.ProfitGaku
           ,[CommentOutStore]       = tbl.CommentOutStore
           ,[CommentInStore]        = tbl.CommentInStore
           ,[IndividualClientName]  = tbl.IndividualClientName    --IndividualClientName
           ,[UpdateOperator]        = @Operator
           ,[UpdateDateTime]        = @SYSDATETIME
        FROM @Table tbl
        WHERE @SalesNO = D_SalesDetails.SalesNO
		 AND D_SalesDetails.SalesRows = tbl.SalesRows
         AND tbl.UpdateFlg = 1
         ;

        --çsí«â¡ï™
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
           ,[ProperGaku]
           ,[DiscountGaku]
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
           ,tbl.SalesRows
           ,NULL AS JuchuuNO
           ,0 AS JuchuuRows
           ,tbl.NotPrintFLG
           ,0 AS AddSalesRows   --å„Ç≈çXêV
           ,NULL AS ShippingNO
           ,tbl.AdminNO
           ,tbl.SKUCD
           ,tbl.JanCD
           ,tbl.SKUName
           ,tbl.ColorName
           ,tbl.SizeName
           ,tbl.SalesSu        --SalesSU
           ,tbl.SalesUnitPrice  --SalesUnitPrice
           ,tbl.TaniCD
           ,tbl.SalesHontaiGaku   --SalesHontaiGaku
           ,tbl.SalesTax        --SalesTax
           ,tbl.SalesGaku       --SalesGaku
           ,tbl.SalesTaxRitsu   --SalesTaxRitsu
           ,tbl.SalesGaku AS ProperGaku
           ,(CASE tbl.DiscountKbn WHEN 1 THEN tbl.SalesGaku ELSE 0 END) As DiscountGaku
           ,tbl.CostUnitPrice
           ,tbl.CostGaku
           ,tbl.ProfitGaku
           ,tbl.CommentOutStore
           ,tbl.CommentInStore
           ,tbl.IndividualClientName    --IndividualClientName
           ,0       --DeliveryNoteFLG, tinyint,>
           ,0       --BillingPrintFLG, tinyint,>
           ,@PurchaseNO
           ,ROW_NUMBER() OVER(ORDER BY tbl.SalesRows) AS PurchaseRows
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
        FROM @Table tbl
        WHERE tbl.UpdateFlg = 0
        ;
          
        --çsçÌèúï™
        --Tableì]ëóédólÇfáA
        UPDATE D_SalesDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE NOT EXISTS(SELECT 1 FROM @Table AS tbl
                    WHERE tbl.SalesRows = [D_SalesDetails].SalesRows)
         AND SalesNO = @SalesNO
         ; 
        
        --D_SalesTran           Insert  Tableì]ëóédólÇk
        --D_SalesDetailsTran    Insert  Tableì]ëóédólÇl
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,1  --ProcessKBN tinyint,
            ,0  --RecoredKBN
            ,1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@SalesDate
            ;

        --Update Tableì]ëóédólÇgáAçXêV
        UPDATE D_CollectPlan SET
            [StoreCD]              = DS.StoreCD
           ,[CustomerCD]           = @BillingCD       --CustomerCD                       
           ,[HontaiGaku]           = DS.SalesHontaiGaku      --HontaiGaku
           ,[HontaiGaku0]          = DS.SalesHontaiGaku0     --HontaiGaku0
           ,[HontaiGaku8]          = DS.SalesHontaiGaku8     --HontaiGaku8
           ,[HontaiGaku10]         = DS.SalesHontaiGaku10    --HontaiGaku10
           ,[Tax]                  = DS.SalesTax     --Tax
           ,[Tax8]                 = DS.SalesTax8    --Tax8
           ,[Tax10]                = DS.SalesTax10   --Tax10
           ,[CollectPlanGaku]      = DS.SalesGaku    --CollectPlanGaku
           ,[BillingType]          = @BillingType
           ,[BillingDate]          = (CASE WHEN @BillingType = 1 THEN DS.SalesDate
                                        ELSE NULL END) --<BillingDate, date,>
           
           ,[BillingNO]            = (CASE WHEN @BillingType = 1 THEN @BillingNO
                                        ELSE NULL END)  --BillingNO
           ,[PaymentMethodCD]      = @PaymentMethodCD
           ,[FirstCollectPlanDate] = CONVERT(date, @CollectPlanDate)
           ,[UpdateOperator]       = @Operator
           ,[UpdateDateTime]       = @SYSDATETIME
        FROM D_Sales AS DS
        WHERE DS.SalesNO = @SalesNO
        AND DS.SalesNO = D_CollectPlan.SalesNO
        AND D_CollectPlan.DeleteDateTime IS NULL
        ;

        --D_CollectPlanDetails  Update Tableì]ëóédólÇháAçXêV
        UPDATE D_CollectPlanDetails SET
            [HontaiGaku]            = DSM.SalesHontaiGaku --HontaiGaku
           ,[Tax]                   = DSM.SalesTax    --Tax
           ,[CollectPlanGaku]       = DSM.SalesGaku   --CollectPlanGaku
           ,[TaxRitsu]              = DSM.SalesTaxRitsu   --TaxRitsu
           ,[FirstCollectPlanDate]  = CONVERT(date, @CollectPlanDate)
           ,[UpdateOperator]        = @Operator
           ,[UpdateDateTime]        = @SYSDATETIME
        FROM D_SalesDetails AS DSM
        WHERE DSM.SalesNO = @SalesNO
        AND DSM.SalesNO = D_CollectPlanDetails.SalesNO
        AND DSM.SalesRows = D_CollectPlanDetails.SalesRows
        AND DSM.DeleteDateTime IS NULL
        AND D_CollectPlanDetails.DeleteDateTime IS NULL
        ;

		--çsí«â¡ï™
        --D_CollectPlanDetails  Insert  Tableì]ëóédólÇh
        INSERT INTO [D_CollectPlanDetails]
           ([CollectPlanNO]
           ,[CollectPlanRows]
           ,[SalesNO]
           ,[SalesRows]
           ,[JuchuuNO]
           ,[JuchuuRows]
           ,[JuchuuKBN]
           ,[HontaiGaku]
           ,[Tax]
           ,[CollectPlanGaku]
           ,[TaxRitsu]
           ,[FirstCollectPlanDate]
           ,[PaymentProgressKBN]
           ,[BillingPrintFLG]
           ,[AdjustTax]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT
           (SELECT top 1 DH.CollectPlanNO 
            FROM D_CollectPlan AS DH
            WHERE DH.SalesNO = @SalesNO
            ORDER BY CollectPlanNO desc)
           ,DSM.SalesRows   --CollectPlanRows
           ,DSM.SalesNO
           ,DSM.SalesRows
           ,DSM.JuchuuNO
           ,DSM.JuchuuRows
           ,3 AS JuchuuKBN	--3:ìXï‹äOè§
           ,DSM.SalesHontaiGaku --HontaiGaku
           ,DSM.SalesTax    --Tax
           ,DSM.SalesGaku   --CollectPlanGaku
           ,DSM.SalesTaxRitsu   --TaxRitsu
           ,CONVERT(date, @CollectPlanDate)
           ,0   --PaymentProgressKBN
           ,0   --BillingPrintFLG
           ,0   --AdjustTax
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
       FROM D_SalesDetails AS DSM
       INNER JOIN @Table AS tbl
       ON tbl.SalesRows = DSM.SalesRows
       WHERE DSM.SalesNO = @SalesNO
       AND DSM.DeleteDateTime IS NULL
       AND tbl.UpdateFlg = 0
       ;
		
        --çsçÌèúï™
        --âÒé˚ó\íËñæç◊
        UPDATE D_CollectPlanDetails SET
           [UpdateOperator]     =  @Operator  
          ,[UpdateDateTime]     =  @SYSDATETIME
          ,[DeleteOperator]     =  @Operator  
          ,[DeleteDateTime]     =  @SYSDATETIME
        WHERE SalesNO = @SalesNO
        AND NOT EXISTS(SELECT 1 FROM @Table AS tbl
                    WHERE tbl.SalesRows = [D_CollectPlanDetails].SalesRows)
        ;
        
        --D_Billing             Update Tableì]ëóédólÇiáAçXêV
        UPDATE D_Billing SET
        	[StoreCD]                  = DS.StoreCD
           ,[BillingCloseDate]         = CONVERT(date, @SalesDate)
           ,[CollectPlanDate]          = CONVERT(date, @CollectPlanDate)
           ,[BillingCustomerCD]        = @BillingCD       --BillingCustomerCD
           ,[SumBillingHontaiGaku]     = DS.SalesHontaiGaku  --SumBillingHontaiGaku 
           ,[SumBillingHontaiGaku0]    = DS.SalesHontaiGaku0 --SumBillingHontaiGaku0 
           ,[SumBillingHontaiGaku8]    = DS.SalesHontaiGaku8 --SumBillingHontaiGaku8 
           ,[SumBillingHontaiGaku10]   = DS.SalesHontaiGaku10    --SumBillingHontaiGaku10 
           ,[SumBillingTax]            = DS.SalesTax --SumBillingTax 
           ,[SumBillingTax8]           = DS.SalesTax8    --SumBillingTax8 
           ,[SumBillingTax10]          = DS.SalesTax10   --SumBillingTax10 
           ,[SumBillingGaku]           = DS.SalesGaku    --SumBillingGaku 
           ,[TotalBillingHontaiGaku]   = DS.SalesHontaiGaku        
           ,[TotalBillingHontaiGaku0]  = DS.SalesHontaiGaku0
           ,[TotalBillingHontaiGaku8]  = DS.SalesHontaiGaku8
           ,[TotalBillingHontaiGaku10] = DS.SalesHontaiGaku10
           ,[TotalBillingTax]          = DS.SalesTax
           ,[TotalBillingTax8]         = DS.SalesTax8
           ,[TotalBillingTax10]        = DS.SalesTax10
           ,[BillingGaku]              = DS.SalesGaku
           ,[UpdateOperator]           = @Operator  
           ,[UpdateDateTime]           = @SYSDATETIME
        FROM D_Sales AS DS
        INNER JOIN D_BillingDetails AS DBM
        ON DBM.SalesNO = DS.SalesNO
        AND DBM.DeleteDateTime IS NULL
        WHERE D_Billing.DeleteDateTime IS NULL
         AND DBM.BillingNO = D_Billing.BillingNO
         AND DBM.SalesNO = @SalesNO
         ;
        --ÅEForm.êøãÅÉ{É^ÉìÅÅÅuë¶êøãÅÅvÇÃèÍçáÇÃÇ›
        IF @@ROWCOUNT = 0 AND @BillingType = 1
        BEGIN
            --ì`ï[î‘çÜçÃî‘
            EXEC Fnc_GetNumber
                15,             --inì`ï[éÌï  15
                @SalesDate,    --inäÓèÄì˙
                @StoreCD,       --inìXï‹CD
                @Operator,
                @BillingNO OUTPUT
                ;
                    
            IF ISNULL(@BillingNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END

            --D_Billing             Insert  Tableì]ëóédólÇi
            INSERT INTO [D_Billing]
                   ([BillingNO]
                   ,[BillingType]   --2019.10.23 add
                   ,[StoreCD]
                   ,[BillingCloseDate]
                   ,[CollectPlanDate]
                   ,[BillingCustomerCD]
                   ,[ProcessingNO]
                   ,[SumBillingHontaiGaku]
                   ,[SumBillingHontaiGaku0]
                   ,[SumBillingHontaiGaku8]
                   ,[SumBillingHontaiGaku10]
                   ,[SumBillingTax]
                   ,[SumBillingTax8]
                   ,[SumBillingTax10]
                   ,[SumBillingGaku]
                   ,[AdjustHontaiGaku8]
                   ,[AdjustHontaiGaku10]
                   ,[AdjustTax8]
                   ,[AdjustTax10]
                   ,[TotalBillingHontaiGaku]
                   ,[TotalBillingHontaiGaku0]
                   ,[TotalBillingHontaiGaku8]
                   ,[TotalBillingHontaiGaku10]
                   ,[TotalBillingTax]
                   ,[TotalBillingTax8]
                   ,[TotalBillingTax10]
                   ,[BillingGaku]
                   ,[PrintDateTime]
                   ,[PrintStaffCD]
                   ,[CollectDate]
                   ,[LastCollectDate]
                   ,[CollectStaffCD]
                   ,[CollectGaku]
                   ,[LastBillingGaku]
                   ,[LastCollectGaku]
                   ,[BillingConfirmFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             SELECT
                   @BillingNO
                   ,1   --BillingType   2019.10.23 add
                   ,DS.StoreCD
                   ,CONVERT(date, @SalesDate)
                   ,CONVERT(date, @CollectPlanDate)
                   ,@BillingCD       --BillingCustomerCD
                   ,NULL    --ProcessingNO
                   ,DS.SalesHontaiGaku  --SumBillingHontaiGaku 
                   ,DS.SalesHontaiGaku0 --SumBillingHontaiGaku0 
                   ,DS.SalesHontaiGaku8 --SumBillingHontaiGaku8 
                   ,DS.SalesHontaiGaku10    --SumBillingHontaiGaku10 
                   ,DS.SalesTax --SumBillingTax 
                   ,DS.SalesTax8    --SumBillingTax8 
                   ,DS.SalesTax10   --SumBillingTax10 
                   ,DS.SalesGaku    --SumBillingGaku 
                   ,0   --AdjustHontaiGaku8 
                   ,0   --AdjustHontaiGaku10 
                   ,0   --AdjustTax8 
                   ,0   --AdjustTax10 
                   ,DS.SalesHontaiGaku        
                   ,DS.SalesHontaiGaku0
                   ,DS.SalesHontaiGaku8
                   ,DS.SalesHontaiGaku10
                   ,DS.SalesTax
                   ,DS.SalesTax8
                   ,DS.SalesTax10
                   ,DS.SalesGaku
                   ,NULL    --PrintDateTime
                   ,NULL    --PrintStaffCD
                   ,NULL    --CollectDate
                   ,NULL    --LastCollectDate
                   ,NULL    --CollectStaffCD
                   ,0   --CollectGaku-
                   ,0   --LastBillingGaku
                   ,0   --LastCollectGaku 
                   ,1   -- BillingConfirmFlg
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
               FROM D_Sales AS DS
               WHERE DS.SalesNO = @SalesNO
               ;

        END
        ELSE IF @BillingType = 0
        BEGIN
             --Tableì]ëóédólÇiáAÅ@çÌèú
             UPDATE D_Billing SET
                   [UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  ,[DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
             FROM D_BillingDetails AS DBM
             WHERE DBM.BillingNO = D_Billing.BillingNO
             AND DBM.SalesNO = @SalesNO
             ;
        END
        
        --D_BillingDetails  Update Tableì]ëóédólÇjáAçXêV
        UPDATE D_BillingDetails SET
        	[StoreCD]           = DS.StoreCD
           ,[BillingCloseDate]  = CONVERT(date, @SalesDate)-- AS BillingCloseDate
           ,[CustomerCD]        = DS.CustomerCD
           ,[SalesNO]           = DSM.SalesNO
           ,[SalesRows]         = DSM.SalesRows 
           ,[CollectPlanNO]     = DM.CollectPlanNO 
           ,[CollectPlanRows]   = DM.CollectPlanRows 
           ,[BillingHontaiGaku] = DSM.SalesHontaiGaku 
           ,[BillingTax]        = DSM.SalesTax 
           ,[BillingGaku]       = DSM.SalesGaku   --CollectPlanGaku 
           ,[TaxRitsu]          = DSM.SalesTaxRitsu 
           ,[UpdateOperator]    = @Operator  
           ,[UpdateDateTime]    = @SYSDATETIME
        FROM D_Sales AS DS
        INNER JOIN D_SalesDetails AS DSM
        ON DSM.SalesNO = DS.SalesNO
        AND DSM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_CollectPlanDetails AS DM
        ON DM.SalesNO = DSM.SalesNO
        AND DM.SalesRows = DSM.SalesRows
        AND DM.DeleteDateTime IS Null
        WHERE DSM.SalesNO = @SalesNO
        AND DSM.SalesNO = D_BillingDetails.SalesNO
        AND DSM.SalesRows = D_BillingDetails.SalesRows
        AND DS.DeleteDateTime IS NULL
        AND D_BillingDetails.DeleteDateTime IS NULL
        ;

        --çsí«â¡ï™
        --D_BillingDetails      Insert  Tableì]ëóédólÇj
        INSERT INTO [D_BillingDetails]
           ([BillingNO]
           ,[BillingType]   --2019.10.23 add
           ,[BillingRows]
           ,[StoreCD]
           ,[BillingCloseDate]
           ,[CustomerCD]
           ,[SalesNO]
           ,[SalesRows]
           ,[CollectPlanNO]
           ,[CollectPlanRows]
           ,[BillingHontaiGaku]
           ,[BillingTax]
           ,[BillingGaku]
           ,[TaxRitsu]
           ,[InvoiceFLG]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
           @BillingNO
           ,1   --BillingType   2019.10.23 add
           ,DSM.SalesRows AS BillingRows
           ,DS.StoreCD
           ,CONVERT(date, @SalesDate) AS BillingCloseDate
           ,DS.CustomerCD
           ,DSM.SalesNO
           ,DSM.SalesRows 
           ,DM.CollectPlanNO 
           ,DM.CollectPlanRows 
           ,DSM.SalesHontaiGaku 
           ,DSM.SalesTax 
           ,DSM.SalesGaku   --CollectPlanGaku 
           ,DSM.SalesTaxRitsu 
           ,0   --InvoiceFLG 
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           
        FROM D_SalesDetails AS DSM
        INNER JOIN @Table AS tbl
        ON tbl.SalesRows = DSM.SalesRows
       
        LEFT OUTER JOIN D_CollectPlanDetails AS DM
        ON DM.SalesNO = DSM.SalesNO
        AND DM.SalesRows = DSM.SalesRows
        AND DM.DeleteDateTime IS Null
        
        LEFT OUTER JOIN D_Sales AS DS
        ON DS.SalesNO = DSM.SalesNO
        AND DS.DeleteDateTime IS Null

        WHERE DSM.SalesNO = @SalesNO    
        AND DSM.DeleteDateTime IS Null             
        AND tbl.UpdateFlg = 0
        ;

		
		
        --çsçÌèúï™
         --êøãÅñæç◊
         --Tableì]ëóédólÇjáAÅ@çÌèú
         UPDATE D_BillingDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
         AND NOT EXISTS(SELECT 1 FROM @Table AS tbl
                    WHERE tbl.SalesRows = [D_BillingDetails].SalesRows)
         ;
               

	END
	    
    --êVãKModeéûÅA
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = 'êVãK';
        
        --ñæç◊êîï™InsertÅö
        --ÉJÅ[É\ÉãÉIÅ[ÉvÉì
        OPEN CUR_TABLE;

        --ç≈èâÇÃ1çsñ⁄ÇéÊìæÇµÇƒïœêîÇ÷ílÇÉZÉbÉg
        FETCH NEXT FROM CUR_TABLE
        INTO @tblVendorCD, @tblSalesRows, @tblUpdateFlg, @tblRows;
        
        --ÉfÅ[É^ÇÃçsêîï™ÉãÅ[ÉvèàóùÇé¿çsÇ∑ÇÈ
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ÉãÅ[Évì‡ÇÃé¿ç€ÇÃèàóù Ç±Ç±Ç©ÇÁ===
            IF @OldtblVendorCD <> @tblVendorCD
            BEGIN
            	SET @OldtblVendorCD = @tblVendorCD;
            	
                --ì`ï[î‘çÜçÃî‘
                EXEC Fnc_GetNumber
                    4,             --inì`ï[éÌï  4
                    @SalesDate, --inäÓèÄì˙
                    @StoreCD,       --inìXï‹CD
                    @Operator,
                    @PurchaseNO OUTPUT
                    ;
                
                IF ISNULL(@PurchaseNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --D_PayPlan             Insert  Tableì]ëóédólÇd
                INSERT INTO [D_PayPlan]
                   ([PayPlanKBN]
                   ,[Number]
                   ,[StoreCD]
                   ,[PayeeCD]
                   ,[RecordedDate]
                   ,[PayPlanDate]
                   ,[HontaiGaku8]
                   ,[HontaiGaku10]
                   ,[TaxGaku8]
                   ,[TaxGaku10]
                   ,[PayPlanGaku]
                   ,[PayConfirmGaku]
                   ,[PayConfirmFinishedKBN]
                   ,[PayCloseDate]
                   ,[PayCloseNO]
                   ,[Program]
                   ,[PayConfirmFinishedDate]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT        
                    1   --PayPlanKBN
                   ,@PurchaseNO --Number
                   ,@StoreCD
                   ,MAX(tbl.PayeeCD) AS PayeeCD
                   ,convert(date,@SalesDate)   --RecordedDate
                   ,MAX(convert(date,tbl.PaymentPlanDate))    --PayPlanDate
                   ,0   --HontaiGaku8
                   ,0   --HontaiGaku10
                   ,0   --TaxGaku8
                   ,0   --TaxGaku10
                   ,SUM(tbl.OrderGaku + tbl.OrderTax+ tbl.OrderKeigenTax)    --PayPlanGaku
                   ,0   --PayConfirmGaku
                   ,0   --PayConfirmFinishedKBN
                   ,NULL    --PayCloseDate
                   ,NULL    --PayCloseNO
                   ,@Program
                   ,NULL    --PayConfirmFinishedDate
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
                FROM @Table AS tbl
                  WHERE tbl.UpdateFlg = 0
                  AND tbl.VendorCD = @tblVendorCD
                  GROUP BY tbl.VendorCD
                  ;
                
                
                --íºëOÇ…çÃî‘Ç≥ÇÍÇΩ IDENTITY óÒÇÃílÇéÊìæÇ∑ÇÈ
                DECLARE @PayPlanNO int;
                SET @PayPlanNO = @@IDENTITY;
                
                --D_Purchase            Insert  Tableì]ëóédólÇ`
                INSERT INTO [D_Purchase]
                   ([PurchaseNO]
                   ,[StoreCD]
                   ,[PurchaseDate]
                   ,[CancelFlg]
                   ,[ProcessKBN]
                   ,[ReturnsFlg]
                   ,[VendorCD]
                   ,[CalledVendorCD]
                   ,[CalculationGaku]
                   ,[AdjustmentGaku]
                   ,[PurchaseGaku]
                   ,[PurchaseTax]
                   ,[TotalPurchaseGaku]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[ExpectedDateFrom]
                   ,[ExpectedDateTo]
                   ,[InputDate]
                   ,[StaffCD]
                   ,[PaymentPlanDate]
                   ,[PayPlanNO]
                   ,[OutputDateTime]
                   ,[StockAccountFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                    @PurchaseNO
                   ,@StoreCD
                   ,convert(date,@SalesDate)
                   ,0   --CancelFlg
                   ,6   --ProcessKBN
                   ,0   --ReturnsFlg
                   ,tbl.VendorCD
                   ,tbl.VendorCD
                   ,SUM(tbl.OrderGaku)
                   ,0 AS AdjustmentGaku
                   ,SUM(tbl.OrderGaku)
                   ,SUM(tbl.OrderTax+tbl.OrderKeigenTax)
                   ,SUM(tbl.OrderGaku + tbl.OrderTax+tbl.OrderKeigenTax) --TotalPurchaseGaku
                   ,NULL    --CommentOutStore
                   ,NULL    --CommentInStore
                   ,NULL    --ExpectedDateFrom
                   ,NULL    --ExpectedDateTo
                   ,@SYSDATETIME    --InputDate
                   ,@StaffCD
                   ,MAX(tbl.PaymentPlanDate) AS PaymentPlanDate
                   ,@PayPlanNO
                   ,NULL    --OutputDateTime
                   ,0   --StockAccountFlg

                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL                  
                   ,NULL
                FROM @Table AS tbl
                  WHERE tbl.UpdateFlg = 0
                  AND tbl.VendorCD = @tblVendorCD
                  GROUP BY tbl.VendorCD
                  ;
                   
                --D_PurchaseDetails     Insert  Tableì]ëóédólÇa
                INSERT INTO [D_PurchaseDetails]
                       ([PurchaseNO]
                       ,[PurchaseRows]
                       ,[DisplayRows]
                       ,[ArrivalNO]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[MakerItem]
                       ,[ItemName]
                       ,[ColorName]
                       ,[SizeName]
                       ,[Remark]
                       ,[PurchaseSu]
                       ,[TaniCD]
                       ,[TaniName]
                       ,[PurchaserUnitPrice]
                       ,[CalculationGaku]
                       ,[AdjustmentGaku]
                       ,[PurchaseGaku]
                       ,[PurchaseTax]
                       ,[TotalPurchaseGaku]
                       ,[CurrencyCD]
                       ,[TaxRitsu]
                       ,[CommentOutStore]
                       ,[CommentInStore]
                       ,[ReturnNO]
                       ,[ReturnRows]
                       ,[OrderUnitPrice]
                       ,[OrderNO]
                       ,[OrderRows]
                       ,[StockNO]
                       ,[DifferenceFlg]
                       ,[DeliveryNo]

                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                 SELECT @PurchaseNO                         
                       ,ROW_NUMBER() OVER(PARTITION BY tbl.VendorCD ORDER BY tbl.SalesRows) AS PurchaseRows                       
                       ,tbl.DisplayRows 
                       ,NULL    --ArrivalNO   
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,tbl.MakerItem
                       ,tbl.SKUName AS ItemName
                       ,tbl.ColorName
                       ,tbl.SizeName
                       ,NULL    --Remark
                       ,tbl.SalesSu AS PurchaseSu
                       ,tbl.TaniCD
                       ,(SELECT M.Char1 FROM M_MultiPorpose AS M WHERE M.ID = 201 AND M.[Key] = tbl.TaniCD) AS TaniName
                       ,tbl.OrderUnitPrice
                       ,tbl.OrderGaku
                       ,0   --AdjustmentGaku
                       ,tbl.OrderGaku
                       ,tbl.OrderTax
                       ,tbl.OrderGaku + tbl.OrderTax+ tbl.OrderKeigenTax --TotalPurchaseGaku
                       ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)   --CurrencyCD
                       ,tbl.SalesTaxRitsu AS TaxRitsu
                       ,NULL    --CommentOutStore
                       ,NULL    --CommentInStore
                       ,NULL    --ReturnNO
                       ,NULL    --ReturnRows
                       ,0       --OrderUnitPrice
                       ,NULL    --OrderNO
                       ,0       --OrderRows
                       ,NULL    --StockNO
                       ,1       --DifferenceFlg
                       ,NULL    --DeliveryNo
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME

                  FROM @Table AS tbl
                  WHERE tbl.UpdateFlg = 0
                  AND tbl.VendorCD = @tblVendorCD
                  ;

                --D_PurchaseHistory     Insert  Tableì]ëóédólÇb
                --D_PurchaseDetailsHistory  Insert  Tableì]ëóédólÇc
                EXEC INSERT_D_PurchaseHistory
                    @PurchaseNO    -- varchar(11),
                    ,1  --@RecoredKBN
                    ,@SYSDATETIME   --  datetime,
                    ,@Operator  --varchar(10),
                    ;

                --D_Sales               Insert  Tableì]ëóédólÇe
                --ì`ï[î‘çÜçÃî‘
                EXEC Fnc_GetNumber
                    3,          --inì`ï[éÌï  3
                    @SalesDate, --inäÓèÄì˙
                    @StoreCD,    --inìXï‹CD
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
                   ,[Age]
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
                   ,CONVERT(date, @SalesDate)
                   ,NULL    --ShippingNO
                   ,@CustomerCD
                   ,@CustomerName
                   ,@CustomerName2
                   ,@BillingType
                   ,5 AS Age    --5:ÇªÇÃëº
                   ,SUM(tbl.SalesHontaiGaku) AS SalesHontaiGaku
                   ,SUM(CASE tbl.SalesTaxRitsu WHEN 0 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku0
                   ,SUM(CASE tbl.SalesTaxRitsu WHEN 8 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku8
                   ,SUM(CASE tbl.SalesTaxRitsu WHEN 10 THEN tbl.SalesHontaiGaku ELSE 0 END) AS SalesHontaiGaku10
                   ,SUM(tbl.SalesTax) AS SalesTax
                   ,SUM(CASE tbl.SalesTaxRitsu WHEN 8 THEN tbl.SalesTax ELSE 0 END) AS SalesTax8
                   ,SUM(CASE tbl.SalesTaxRitsu WHEN 10 THEN tbl.SalesTax ELSE 0 END) AS SalesTax10
                   ,SUM(tbl.SalesGaku) AS SalesGaku
                   ,0   --LastPoint, money,>
                   ,0   --WaitingPoint, money,>
                   ,@Operator   --StaffCD
                   ,NULL    --PrintDate
                   ,NULL    --PrintStaffCD
                   ,0 As Discount
                   ,0 As Discount8
                   ,0 As Discount10
                   ,0 As DiscountTax
                   ,0 As DiscountTax8
                   ,0 As DiscountTax10
                   ,SUM(tbl.CostGaku) AS CostGaku
                   ,SUM(tbl.ProfitGaku) AS ProfitGaku
                   ,@PurchaseNO
                   ,1 AS SalesEntryKBN
                   ,@NouhinsyoComment
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
                FROM @Table AS tbl
                  WHERE tbl.UpdateFlg = 0
                  AND tbl.VendorCD = @tblVendorCD
                  GROUP BY tbl.VendorCD
                  ;
                
                --D_SalesDetails        Insert  Tableì]ëóédólÇf
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
                   ,[ProperGaku]
                   ,[DiscountGaku]
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
                   ,ROW_NUMBER() OVER(ORDER BY tbl.SalesRows) AS SalesRows
                   ,NULL AS JuchuuNO
                   ,0 AS JuchuuRows
                   ,tbl.NotPrintFLG
                   ,0 AS AddSalesRows   --å„Ç≈çXêV
                   ,NULL AS ShippingNO
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,tbl.JanCD
                   ,tbl.SKUName
                   ,tbl.ColorName
                   ,tbl.SizeName
                   ,tbl.SalesSu        --SalesSU
                   ,tbl.SalesUnitPrice  --SalesUnitPrice
                   ,tbl.TaniCD
                   ,tbl.SalesHontaiGaku   --SalesHontaiGaku
                   ,tbl.SalesTax        --SalesTax
                   ,tbl.SalesGaku       --SalesGaku
                   ,tbl.SalesTaxRitsu   --SalesTaxRitsu
                   ,tbl.SalesGaku AS ProperGaku
                   ,(CASE tbl.DiscountKbn WHEN 1 THEN tbl.SalesGaku ELSE 0 END) As DiscountGaku           
                   ,tbl.CostUnitPrice
		           ,tbl.CostGaku
		           ,tbl.ProfitGaku
                   ,tbl.CommentOutStore
                   ,tbl.CommentInStore
                   ,tbl.IndividualClientName    --IndividualClientName
                   ,0       --DeliveryNoteFLG, tinyint,>
                   ,0       --BillingPrintFLG, tinyint,>
                   ,@PurchaseNO
				   ,ROW_NUMBER() OVER(PARTITION BY tbl.VendorCD ORDER BY tbl.SalesRows) AS PurchaseRows
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              AND tbl.VendorCD = @tblVendorCD
              --AND (@Tennic = 0  OR (@Tennic = 1 AND tbl.DisplayRows = @DisplayRows))  --Åö
              ;

                --ì`ï[î‘çÜçÃî‘
                EXEC Fnc_GetNumber
                    15,             --inì`ï[éÌï  15
                    @SalesDate,    --inäÓèÄì˙
                    @StoreCD,       --inìXï‹CD
                    @Operator,
                    @BillingNO OUTPUT
                    ;
                        
                IF ISNULL(@BillingNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                    
                --D_CollectPlan         Insert  Tableì]ëóédólÇg
                INSERT INTO [D_CollectPlan]
                   (--[CollectPlanNO]
                   [SalesNO]
                   ,[JuchuuNO]
                   ,[JuchuuKBN]
                   ,[StoreCD]
                   ,[CustomerCD]
                   ,[HontaiGaku]
                   ,[HontaiGaku0]
                   ,[HontaiGaku8]
                   ,[HontaiGaku10]
                   ,[Tax]
                   ,[Tax8]
                   ,[Tax10]
                   ,[CollectPlanGaku]
                   ,[AdjustTax8]
                   ,[AdjustTax10]
                   ,[BillingType]
                   ,[BillingDate]
                   ,[BillingNO]
                   ,[MonthlyBillingNO]
                   ,[PaymentMethodCD]
                   ,[CardProgressKBN]
                   ,[PaymentProgressKBN]
                   ,[InvalidFLG]
                   ,[BillingCloseDate]
                   ,[FirstCollectPlanDate]
                   ,[ReminderFLG]
                   ,[NoReminderDate]
                   ,[NextCollectPlanDate]
                   ,[ActionCD]
                   ,[NextActionCD]
                   ,[LastReminderNO]
                   ,[Program]
                   ,[BillingConfirmFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                   --CollectPlanNO
                   @SalesNO
                   ,NULL As JuchuuNO
                   ,3 As JuchuuKBN
                   ,DS.StoreCD
                   ,@BillingCD       --CustomerCD                       
                   ,DS.SalesHontaiGaku      --HontaiGaku
                   ,DS.SalesHontaiGaku0     --HontaiGaku0
                   ,DS.SalesHontaiGaku8     --HontaiGaku8
                   ,DS.SalesHontaiGaku10    --HontaiGaku10
                   ,DS.SalesTax     --Tax
                   ,DS.SalesTax8    --Tax8
                   ,DS.SalesTax10   --Tax10
                   ,DS.SalesGaku    --CollectPlanGaku
                   ,0 AS AdjustTax8
                   ,0 AS AdjustTax10
                   ,@BillingType   --BillingType
                   ,(CASE WHEN @BillingType = 1 THEN DS.SalesDate
                        ELSE NULL END) --<BillingDate, date,>
                   ,(CASE WHEN @BillingType = 1 THEN @BillingNO
                   		ELSE NULL END)	--BillingNO
                   ,NULL	--MonthlyBillingNO
                   ,@PaymentMethodCD
                   ,0   --CardProgressKBN
                   ,0   --PaymentProgressKBN
                   ,0   --InvalidFLG
                   ,NULL    --BillingCloseDate, date,>
                   ,CONVERT(date, @CollectPlanDate)
                   ,0   --ReminderFLG
                   ,NULL    --NoReminderDate
                   ,NULL    --NextCollectPlanDate
                   ,NULL    --ActionCD
                   ,NULL    --NextActionCD
                   ,NULL    --LastReminderNO
                   ,@Program  --Program
                   ,(CASE WHEN @BillingType = 1 THEN 1
                   		ELSE 0 END)   --BillingConfirmFlg
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
               FROM D_Sales AS DS
               WHERE DS.SalesNO = @SalesNO
               ;

                --D_CollectPlanDetails  Insert  Tableì]ëóédólÇh
                INSERT INTO [D_CollectPlanDetails]
                   ([CollectPlanNO]
                   ,[CollectPlanRows]
                   ,[SalesNO]
                   ,[SalesRows]
                   ,[JuchuuNO]
                   ,[JuchuuRows]
                   ,[JuchuuKBN]
                   ,[HontaiGaku]
                   ,[Tax]
                   ,[CollectPlanGaku]
                   ,[TaxRitsu]
                   ,[FirstCollectPlanDate]
                   ,[PaymentProgressKBN]
                   ,[BillingPrintFLG]
                   ,[AdjustTax]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT
                   (SELECT top 1 DH.CollectPlanNO 
                    FROM D_CollectPlan AS DH
                    WHERE DH.SalesNO = @SalesNO
                    ORDER BY CollectPlanNO desc)
                   ,DSM.SalesRows   --CollectPlanRows
                   ,DSM.SalesNO
                   ,DSM.SalesRows
                   ,DSM.JuchuuNO
                   ,DSM.JuchuuRows
                   ,3 AS JuchuuKBN	--3:ìXï‹äOè§
                   ,DSM.SalesHontaiGaku --HontaiGaku
                   ,DSM.SalesTax    --Tax
                   ,DSM.SalesGaku   --CollectPlanGaku
                   ,DSM.SalesTaxRitsu   --TaxRitsu
                   ,CONVERT(date, @CollectPlanDate)
                   ,0   --PaymentProgressKBN
                   ,0   --BillingPrintFLG
                   ,0   --AdjustTax
                   ,@Operator
                   ,@SYSDATETIME
                   ,@Operator
                   ,@SYSDATETIME
                   ,NULL --DeleteOperator
                   ,NULL --DeleteDateTime
               FROM D_SalesDetails AS DSM
               WHERE DSM.SalesNO = @SalesNO
               AND DSM.DeleteDateTime IS NULL
               ;
                
                --ÅEForm.êøãÅÉ{É^ÉìÅÅÅuë¶êøãÅÅvÇÃèÍçáÇÃÇ›
                IF @BillingType = 1
                BEGIN
                    --D_Billing             Insert  Tableì]ëóédólÇi
                    INSERT INTO [D_Billing]
                           ([BillingNO]
                           ,[BillingType]   --2019.10.23 add
                           ,[StoreCD]
                           ,[BillingCloseDate]
                           ,[CollectPlanDate]
                           ,[BillingCustomerCD]
                           ,[ProcessingNO]
                           ,[SumBillingHontaiGaku]
                           ,[SumBillingHontaiGaku0]
                           ,[SumBillingHontaiGaku8]
                           ,[SumBillingHontaiGaku10]
                           ,[SumBillingTax]
                           ,[SumBillingTax8]
                           ,[SumBillingTax10]
                           ,[SumBillingGaku]
                           ,[AdjustHontaiGaku8]
                           ,[AdjustHontaiGaku10]
                           ,[AdjustTax8]
                           ,[AdjustTax10]
                           ,[TotalBillingHontaiGaku]
                           ,[TotalBillingHontaiGaku0]
                           ,[TotalBillingHontaiGaku8]
                           ,[TotalBillingHontaiGaku10]
                           ,[TotalBillingTax]
                           ,[TotalBillingTax8]
                           ,[TotalBillingTax10]
                           ,[BillingGaku]
                           ,[PrintDateTime]
                           ,[PrintStaffCD]
                           ,[CollectDate]
                           ,[LastCollectDate]
                           ,[CollectStaffCD]
                           ,[CollectGaku]
                           ,[LastBillingGaku]
                           ,[LastCollectGaku]
                           ,[BillingConfirmFlg]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                     SELECT
                           @BillingNO
                           ,1   --BillingType   2019.10.23 add
                           ,DS.StoreCD
                           ,CONVERT(date, @SalesDate)
                           ,CONVERT(date, @CollectPlanDate)
                           ,@BillingCD       --BillingCustomerCD
                           ,NULL    --ProcessingNO
                           ,DS.SalesHontaiGaku  --SumBillingHontaiGaku 
                           ,DS.SalesHontaiGaku0 --SumBillingHontaiGaku0 
                           ,DS.SalesHontaiGaku8 --SumBillingHontaiGaku8 
                           ,DS.SalesHontaiGaku10    --SumBillingHontaiGaku10 
                           ,DS.SalesTax --SumBillingTax 
                           ,DS.SalesTax8    --SumBillingTax8 
                           ,DS.SalesTax10   --SumBillingTax10 
                           ,DS.SalesGaku    --SumBillingGaku 
                           ,0   --AdjustHontaiGaku8 
                           ,0   --AdjustHontaiGaku10 
                           ,0   --AdjustTax8 
                           ,0   --AdjustTax10 
                           ,DS.SalesHontaiGaku        
                           ,DS.SalesHontaiGaku0
                           ,DS.SalesHontaiGaku8
                           ,DS.SalesHontaiGaku10
                           ,DS.SalesTax
                           ,DS.SalesTax8
                           ,DS.SalesTax10
                           ,DS.SalesGaku
                           ,NULL    --PrintDateTime
                           ,NULL    --PrintStaffCD
                           ,NULL    --CollectDate
                           ,NULL    --LastCollectDate
                           ,NULL    --CollectStaffCD
                           ,0   --CollectGaku-
                           ,0   --LastBillingGaku
                           ,0   --LastCollectGaku 
                           ,1   -- BillingConfirmFlg
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                       FROM D_Sales AS DS
                       WHERE DS.SalesNO = @SalesNO
                       ;

                    --D_BillingDetails      Insert  Tableì]ëóédólÇj
                    INSERT INTO [D_BillingDetails]
                       ([BillingNO]
                       ,[BillingType]   --2019.10.23 add
                       ,[BillingRows]
                       ,[StoreCD]
                       ,[BillingCloseDate]
                       ,[CustomerCD]
                       ,[SalesNO]
                       ,[SalesRows]
                       ,[CollectPlanNO]
                       ,[CollectPlanRows]
                       ,[BillingHontaiGaku]
                       ,[BillingTax]
                       ,[BillingGaku]
                       ,[TaxRitsu]
                       ,[InvoiceFLG]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                       @BillingNO
                       ,1   --BillingType   2019.10.23 add
                       ,DSM.SalesRows AS BillingRows
                       ,DS.StoreCD
                       ,CONVERT(date, @SalesDate) AS BillingCloseDate
                       ,DS.CustomerCD
                       ,DSM.SalesNO
                       ,DSM.SalesRows 
                       ,DM.CollectPlanNO 
                       ,DM.CollectPlanRows 
                       ,DSM.SalesHontaiGaku 
                       ,DSM.SalesTax 
                       ,DSM.SalesGaku   --CollectPlanGaku 
                       ,DSM.SalesTaxRitsu 
                       ,0   --InvoiceFLG 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                       
                    FROM D_SalesDetails AS DSM
                    
                    LEFT OUTER JOIN D_CollectPlanDetails AS DM
                    ON DM.SalesNO = DSM.SalesNO
                    AND DM.SalesRows = DSM.SalesRows
                    AND DM.DeleteDateTime IS Null
                    
                    LEFT OUTER JOIN D_Sales AS DS
                    ON DS.SalesNO = DSM.SalesNO
                    AND DS.DeleteDateTime IS Null

                    WHERE DSM.SalesNO = @SalesNO    
                    AND DSM.DeleteDateTime IS Null             
                    ;

                END
                
                --D_SalesTran           Insert  Tableì]ëóédólÇk
                --D_SalesDetailsTran    Insert  Tableì]ëóédólÇl
                EXEC INSERT_D_SalesTran
                    @SalesNO 
                    ,1  --ProcessKBN tinyint,
                    ,0  --RecoredKBN
                    ,1 --SIGN int,
                    ,@Operator
                    ,@SYSDATETIME
                    ,@SalesDate
                    ;
                    
                --èàóùóöóÉfÅ[É^Ç÷çXêV
                SET @KeyItem = @SalesNO;
                    
                EXEC L_Log_Insert_SP
                    @SYSDATETIME,
                    @Operator,
                    @Program,
                    @PC,
                    @OperateModeNm,
                    @KeyItem;

			    SET @OutSalesNO = @OutSalesNO + @SalesNO + ',';
            END --édì¸êÊBreak
            

            --éüÇÃçsÇÃÉfÅ[É^ÇéÊìæÇµÇƒïœêîÇ÷ílÇÉZÉbÉg
            FETCH NEXT FROM CUR_TABLE
            INTO @tblVendorCD, @tblSalesRows, @tblUpdateFlg, @tblRows;
        END            --LOOPÇÃèIÇÌÇË
        
        --ÉJÅ[É\ÉãÇï¬Ç∂ÇÈ
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
            
    END
    ELSE IF @OperateMode = 3 --çÌèú--
    BEGIN
        SET @OperateModeNm = 'çÌèú';
        
        --ÅyD_PurchaseHistoryÅzInsertÅ@Tableì]ëóédólÇbÅ@ê‘
        --ÅyD_PurchaseDetailsHistoryÅzInsertÅ@Tableì]ëóédólÇcÅ@ê‘
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;
        
        --Tableì]ëóédólÇ`áA
        UPDATE [D_Purchase]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE EXISTS(SELECT 1 FROM D_Sales AS DS
                    WHERE DS.PurchaseNO = [D_Purchase].PurchaseNO
                    AND DS.SalesNO = @SalesNO)
         AND [D_Purchase].DeleteDateTime IS NULL
         ;
        
        --Tableì]ëóédólÇaáA
        UPDATE [D_PurchaseDetails]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE EXISTS(SELECT 1 FROM D_Sales AS DS
                    WHERE DS.PurchaseNO = [D_PurchaseDetails].PurchaseNO
                    AND DS.SalesNO = @SalesNO)
         AND [D_PurchaseDetails].DeleteDateTime IS NULL
         ;

		--Tableì]ëóédólÇdáA
        UPDATE [D_PayPlan]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE EXISTS(SELECT 1 FROM D_Sales AS DS
                    WHERE DS.PurchaseNO = [D_PayPlan].Number
                    AND DS.SalesNO = @SalesNO)
         AND [DeleteDateTime] IS NULL
         ;
         
        --Tableì]ëóédólÇeáA
		UPDATE D_Sales SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ;
        
        --Tableì]ëóédólÇfáA
        UPDATE D_SalesDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
           ; 

         --âÒé˚ó\íËñæç◊
         --Tableì]ëóédólÇgáA çÌèú
         UPDATE D_CollectPlanDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
         ;
         --âÒé˚ó\íË
         --Tableì]ëóédólÇháAÅ@çÌèú
         UPDATE D_CollectPlan SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
         ;
         
         --êøãÅ
         --Tableì]ëóédólÇiáAÅ@çÌèú
         UPDATE D_Billing SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         FROM D_BillingDetails AS DBM
         WHERE DBM.BillingNO = D_Billing.BillingNO
         AND DBM.SalesNO = @SalesNO
         ;
         
         --êøãÅñæç◊
         --Tableì]ëóédólÇjáAÅ@çÌèú
         UPDATE D_BillingDetails SET
               [UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
              ,[DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE SalesNO = @SalesNO
         ;
         
        --D_SalesTran           Insert  Tableì]ëóédólÇk
        --D_SalesDetailsTran    Insert  Tableì]ëóédólÇl
        EXEC INSERT_D_SalesTran
            @SalesNO 
            ,3  --ProcessKBN tinyint,
            ,1  --RecoredKBN
            ,-1 --SIGN int,
            ,@Operator
            ,@SYSDATETIME
            ,@SalesDate
            ;
    END
    
    IF @OperateMode >= 2 --èCê≥/çÌèú--
    BEGIN
        --èàóùóöóÉfÅ[É^Ç÷çXêV
        SET @KeyItem = @SalesNO;
            
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            @Program,
            @PC,
            @OperateModeNm,
            @KeyItem;

        SET @OutSalesNO = @SalesNO;
    END
    
--<<OWARI>>
  return @W_ERR;

END

GO
