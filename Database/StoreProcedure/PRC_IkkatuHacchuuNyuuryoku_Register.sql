
/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_Register]    Script Date: 2020/09/25 11:36:50 ******/
DROP PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_Register]
GO

/****** Object:  UserDefinedTableType [dbo].[T_IkkatuHacchuuNyuuryoku]    Script Date: 2020/10/12 4:33:52 ******/
DROP TYPE [dbo].[T_IkkatuHacchuuNyuuryoku]
GO

/****** Object:  UserDefinedTableType [dbo].[T_IkkatuHacchuuNyuuryoku]    Script Date: 2020/10/12 4:33:52 ******/
CREATE TYPE [dbo].[T_IkkatuHacchuuNyuuryoku] AS TABLE(
	[GyouNO] [varchar](100) NULL,
	[HacchuuNO] [varchar](100) NULL,
	[SiiresakiCD] [varchar](100) NULL,
	[SiiresakiName] [varchar](100) NULL,
	[ChokusouFLG] [varchar](100) NULL,
	[NetFLG] [varchar](100) NULL,
	[NounyuusakiName] [varchar](100) NULL,
	[NounyuusakiJuusho] [varchar](201) NULL,
	[JuchuuNO] [varchar](100) NULL,
	[SKUCD] [varchar](100) NULL,
	[JANCD] [varchar](100) NULL,
	[ShouhinName] [varchar](100) NULL,
	[BrandName] [varchar](100) NULL,
	[SizeName] [varchar](100) NULL,
	[ColorName] [varchar](100) NULL,
	[HacchuuChuuiZikou] [varchar](100) NULL,
	[EDIFLG] [varchar](100) NULL,
	[MakerShouhinCD] [varchar](100) NULL,
	[KibouNouki] [varchar](100) NULL,
	[ShanaiBikou] [varchar](500) NULL,
	[ShagaiBikou] [varchar](500) NULL,
	[TaniName] [varchar](100) NULL,
	[HacchuuSuu] [varchar](100) NULL,
	[HacchuuTanka] [varchar](100) NULL,
	[Hacchuugaku] [varchar](100) NULL,
	[TaishouFLG] [varchar](100) NULL,
	[NounyuusakiYuubinNO1] [varchar](100) NULL,
	[NounyuusakiYuubinNO2] [varchar](100) NULL,
	[NounyuusakiJuusho1] [varchar](100) NULL,
	[NounyuusakiJuusho2] [varchar](100) NULL,
	[NounyuusakiMailAddress] [varchar](100) NULL,
	[NounyuusakiTELNO] [varchar](100) NULL,
	[NounyuusakiFAXNO] [varchar](100) NULL,
	[SoukoCD] [varchar](100) NULL,
	[TaxRate] [varchar](100) NULL,
	[JuchuuRows] [varchar](100) NULL,
	[VariousFLG] [varchar](100) NULL,
	[AdminNO] [varchar](100) NULL,
	[SKUName] [varchar](100) NULL,
	[Teika] [varchar](100) NULL,
	[Kakeritu] [varchar](100) NULL,
	[HacchuuShouhizeigaku] [varchar](100) NULL,
	[TaniCD] [varchar](100) NULL,
	[OrderRows] [varchar](100) NULL,
	[IsYuukouTaishouFLG] [varchar](100) NULL
)
GO

/****** Object:  StoredProcedure [dbo].[PRC_IkkatuHacchuuNyuuryoku_Register]    Script Date: 2020/09/25 11:36:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[PRC_IkkatuHacchuuNyuuryoku_Register](
     @p_OperateMode               int                  -- 処理区分（1:新規 2:修正 3:削除）  
    ,@p_Operator                  varchar(10)   
    ,@p_PC                        varchar(30)   
    ,@p_StoreCD                   varchar(4)   
    ,@p_StaffCD                   varchar(10)   
    ,@p_OrderDate                 date
    ,@p_OrderNO                   varchar(11)   
    ,@p_OrderProcessNO            varchar(11)   
    ,@p_IkkatuHacchuuMode         varchar(1) --0:Net発注、1:FAX発注  
    ,@p_TIkkatuHacchuuNyuuryoku   dbo.T_IkkatuHacchuuNyuuryoku  READONLY
)AS
BEGIN

--********************************************--      
--                                            --      
--                 処理開始                   --      
--                                            --      
--********************************************--      
    DECLARE @W_ERR  tinyint      
    DECLARE @SYSDATETIME datetime      
    DECLARE @p_OperateModeNm varchar(10)      
    DECLARE @KeyItem varchar(100)      
    DECLARE @ReserveNO varchar(11)      
    DECLARE @OrderNO varchar(11)      
    DECLARE @OrderProcessNO varchar(11)      
          
    DECLARE @Num1 int      
    DECLARE @SYSDATE date      
          
    SET @W_ERR = 0      
    SET @SYSDATETIME = SYSDATETIME()      
      
    SELECT @Num1 = ISNULL((SELECT A.Num1 FROM M_MultiPorpose AS A WHERE A.ID = 318 AND A.[Key] = 1 ),0)      
    SET @SYSDATE = CONVERT(datetime, @SYSDATETIME)      
       
/****************************************    
    更新用ワークテーブル作成    
*****************************************/    
    --D_OrderDetails更新用ワーク    
    CREATE TABLE #DOrderDetails(    
     [OrderNO] [varchar](11) NOT NULL,    
     [OrderRows] [int] NOT NULL,    
     [DisplayRows] [int] NOT NULL,    
     [JuchuuNO] [varchar](11) NULL,    
     [JuchuuRows] [int] NOT NULL,    
     [JuchuuOrderNO] [varchar](11) NULL,    
     [SKUCD] [varchar](30) NULL,    
     [AdminNO] [int] NOT NULL,    
     [JanCD] [varchar](13) NULL,    
     [MakerItem] [varchar](50) NULL,    
     [ItemName] [varchar](80) NULL,    
     [ColorName] [varchar](20) NULL,    
     [SizeName] [varchar](20) NULL,    
     [Remarks] [varchar](30) NULL,    
     [OrderSu] [int] NOT NULL,    
     [TaniCD] [varchar](2) NULL,    
     [PriceOutTax] [money] NULL,    
     [Rate] [decimal](5, 2) NULL,    
     [OrderUnitPrice] [money] NOT NULL,    
     [OrderHontaiGaku] [money] NOT NULL,    
     [OrderTax] [money] NOT NULL,    
     [OrderTaxRitsu] [int] NOT NULL,    
     [OrderGaku] [money] NOT NULL,    
     [SoukoCD] [varchar](6) NULL,    
     [DirectFLG] [tinyint] NOT NULL,    
     [NotNetFLG] [tinyint] NULL,    
     [EDIFLG] [tinyint] NULL,    
     [DesiredDeliveryDate] [date] NULL,    
     [ArrivePlanDate] [date] NULL,    
     [TotalArrivalSu] [int] NULL,    
     [CommentOutStore] [varchar](80) NULL,    
     [CommentInStore] [varchar](80) NULL,    
     [FirstOrderNO] [varchar](11) NULL,    
     [FirstOrderRows] [int] NOT NULL,    
     [CancelOrderNO] [varchar](11) NULL,    
     [AnswerFLG] [tinyint] NULL,    
     [EDIOutputDatetime] [datetime] NULL,    
     [LastArrivePlanNO] [int] NULL,    
     [LastArriveDatetime] [datetime] NULL,    
     [LastArriveNO] [varchar](11) NULL,    
     [InsertOperator] [varchar](11) NULL,    
     [InsertDateTime] [datetime] NULL,    
     [UpdateOperator] [varchar](11) NULL,    
     [UpdateDateTime] [datetime] NULL,    
     [DeleteOperator] [varchar](11) NULL,    
     [DeleteDateTime] [datetime] NULL    
        )    
    --D_Order更新用ワーク    
    CREATE TABLE #DOrder(    
     [OrderNO] [varchar](11) NOT NULL,    
     [OrderProcessNO] [varchar](11) NULL,    
     [StoreCD] [varchar](4) NULL,    
     [OrderDate] [date] NULL,    
     [ReturnFLG] [tinyint] NOT NULL,    
     [OrderDataKBN] [tinyint] NOT NULL,    
     [OrderWayKBN] [tinyint] NOT NULL,    
     [OrderCD] [varchar](13) NULL,    
     [OrderPerson] [varchar](50) NULL,    
     [AliasKBN] [tinyint] NOT NULL,    
     [DestinationKBN] [tinyint] NOT NULL,    
     [DestinationName] [varchar](80) NULL,    
     [DestinationZip1CD] [varchar](3) NULL,    
     [DestinationZip2CD] [varchar](4) NULL,    
     [DestinationAddress1] [varchar](100) NULL,    
     [DestinationAddress2] [varchar](100) NULL,    
     [DestinationTelphoneNO] [varchar](15) NULL,    
     [DestinationFaxNO] [varchar](15) NULL,    
     [DestinationSoukoCD] [varchar](6) NULL,    
     [CurrencyCD] [varchar](2) NULL,    
     [OrderHontaiGaku] [money] NOT NULL,    
     [OrderTax8] [money] NOT NULL,    
     [OrderTax10] [money] NOT NULL,    
     [OrderGaku] [money] NOT NULL,    
     [CommentOutStore] [varchar](80) NULL,    
     [CommentInStore] [varchar](80) NULL,    
     [StaffCD] [varchar](10) NULL,    
     [FirstArriveDate] [date] NULL,    
     [LastArriveDate] [date] NULL,    
     [ApprovalDate] [date] NULL,    
     [LastApprovalDate] [date] NULL,    
     [LastApprovalStaffCD] [varchar](10) NULL,    
     [ApprovalStageFLG] [tinyint] NOT NULL,    
     [FirstPrintDate] [date] NULL,    
     [LastPrintDate] [date] NULL,    
     [ArrivePlanDate] [date] NULL,    
     [InsertOperator] [varchar](10) NULL,    
     [InsertDateTime] [datetime] NULL,    
     [UpdateOperator] [varchar](10) NULL,    
     [UpdateDateTime] [datetime] NULL,    
     [DeleteOperator] [varchar](10) NULL,    
     [DeleteDateTime] [datetime] NULL    
        )    
    
/****************************************    
    更新用ワークテーブルINSERT    
*****************************************/    
   --画面情報（@p_TIkkatuHacchuuNyuuryoku）から  更新用ワーク作成    
   INSERT INTO #DOrderDetails    
   (    
    OrderNO                 
   ,OrderRows               
   ,DisplayRows             
   ,JuchuuNO                
   ,JuchuuRows              
   ,JuchuuOrderNO           
   ,SKUCD                   
   ,AdminNO                 
   ,JanCD                   
   ,MakerItem
   ,ItemName                
   ,ColorName               
   ,SizeName                
   ,Remarks                 
   ,OrderSu                 
   ,TaniCD                  
   ,PriceOutTax             
   ,Rate                    
   ,OrderUnitPrice          
   ,OrderHontaiGaku         
   ,OrderTax                
   ,OrderTaxRitsu           
   ,OrderGaku               
   ,SoukoCD                 
   ,DirectFLG               
   ,NotNetFLG               
   ,EDIFLG                  
   ,DesiredDeliveryDate     
   ,ArrivePlanDate          
   ,TotalArrivalSu          
   ,CommentOutStore         
   ,CommentInStore          
   ,FirstOrderNO            
   ,FirstOrderRows          
   ,CancelOrderNO           
   ,AnswerFLG               
   ,EDIOutputDatetime       
   ,LastArrivePlanNO        
   ,LastArriveDatetime      
   ,LastArriveNO            
   ,InsertOperator          
   ,InsertDateTime          
   ,UpdateOperator          
   ,UpdateDateTime          
   ,DeleteOperator          
   ,DeleteDateTime          
   )    
    SELECT OrderNO                  = CASE WHEN @p_OperateMode = 1 THEN CAST(DENSE_RANK()OVER(ORDER BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho) AS varchar) ELSE MAIN.HacchuuNO END --新規の場合は後で採番、キーだけふっとく    
          ,OrderRows                = CASE WHEN @p_OperateMode = 1 THEN CAST(ROW_NUMBER()OVER(PARTITION BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho ORDER BY MAIN.GyouNO) AS varchar) ELSE MAIN.OrderRows END    
          ,DisplayRows              = ROW_NUMBER()OVER(PARTITION BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho ORDER BY MAIN.GyouNO)    
          ,JuchuuNO                 = MAIN.JuchuuNO                  
          ,JuchuuRows               = ISNULL(MAIN.JuchuuRows,0)
          ,JuchuuOrderNO            = null    
          ,SKUCD                    = MAIN.SKUCD                     
          ,AdminNO                  = MAIN.AdminNO                   
          ,JanCD                    = MAIN.JanCD                     
          ,MakerItem                = MAIN.MakerShouhinCD
          ,ItemName                 = MAIN.ShouhinName    
          ,ColorName                = MAIN.ColorName                 
          ,SizeName                 = MAIN.SizeName                  
          ,Remarks                  = MAIN.HacchuuChuuiZikou                   
          ,OrderSu                  = CAST(MAIN.HacchuuSuu as decimal)    
          ,TaniCD                   = MAIN.TaniCD    
          ,PriceOutTax              = CAST(MAIN.Teika as money)    
          ,Rate                     = ISNULL(CAST(NULLIF(MAIN.Kakeritu,'') as decimal),0)    
          ,OrderUnitPrice           = CAST(ISNULL(MAIN.HacchuuTanka,0) as money)                     
          ,OrderHontaiGaku          = CAST(ISNULL(MAIN.Hacchuugaku,0) as money)    
          ,OrderTax                 = CAST(ISNULL(MAIN.Hacchuugaku,0) as money) * CAST(NULLIF(MAIN.TaxRate,'') as decimal) / 100    
          ,OrderTaxRitsu            = CAST(NULLIF(MAIN.TaxRate,'') as decimal)    
          ,OrderGaku                = CAST(ISNULL(MAIN.Hacchuugaku,0) as money) + CAST(ISNULL(MAIN.Hacchuugaku,0) as money) * CAST(NULLIF(MAIN.TaxRate,'') as decimal) / 100    
          ,SoukoCD                  = MAIN.SoukoCD                   
          ,DirectFLG                = CASE WHEN MAIN.ChokusouFLG = '○' THEN 1 ELSE 0 END    
          ,NotNetFLG                = 0    
          ,EDIFLG                   = MAIN.EDIFLG
          ,DesiredDeliveryDate      = MAIN.KibouNouki    
          ,ArrivePlanDate           = null    
          ,TotalArrivalSu           = 0    
          ,CommentOutStore          = MAIN.ShagaiBikou    
          ,CommentInStore           = MAIN.ShanaiBikou    
          ,FirstOrderNO             = null        
          ,FirstOrderRows           = ROW_NUMBER()OVER(PARTITION BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho ORDER BY MAIN.GyouNO)    
          ,CancelOrderNO            = null    
          ,AnswerFLG                = 0    
          ,EDIOutputDatetime        = null    
          ,LastArrivePlanNO         = null    
          ,LastArriveDatetime       = null    
          ,LastArriveNO             = null    
          ,InsertOperator           = @p_Operator    
          ,InsertDateTime           = @SYSDATETIME    
          ,UpdateOperator           = @p_Operator    
          ,UpdateDateTime           = @SYSDATETIME    
          ,DeleteOperator           = null    
          ,DeleteDateTime           = null       
    FROM @p_TIkkatuHacchuuNyuuryoku MAIN    
    
   INSERT INTO #DOrder    
   (    
     OrderNO                       
    ,OrderProcessNO                
    ,StoreCD                       
    ,OrderDate                     
    ,ReturnFLG                     
    ,OrderDataKBN                  
    ,OrderWayKBN                   
    ,OrderCD                       
    ,OrderPerson                   
    ,AliasKBN                      
    ,DestinationKBN                
    ,DestinationName               
    ,DestinationZip1CD             
    ,DestinationZip2CD             
    ,DestinationAddress1           
    ,DestinationAddress2           
    ,DestinationTelphoneNO         
    ,DestinationFaxNO              
    ,DestinationSoukoCD            
    ,CurrencyCD                    
    ,OrderHontaiGaku               
    ,OrderTax8                     
    ,OrderTax10                    
    ,OrderGaku                     
    ,CommentOutStore               
    ,CommentInStore                
    ,StaffCD                       
    ,FirstArriveDate               
    ,LastArriveDate                
    ,ApprovalDate                  
    ,LastApprovalDate              
    ,LastApprovalStaffCD           
    ,ApprovalStageFLG              
    ,FirstPrintDate                
    ,LastPrintDate                 
    ,ArrivePlanDate                 
    ,InsertOperator                
    ,InsertDateTime                
    ,UpdateOperator                
    ,UpdateDateTime                
    ,DeleteOperator                
    ,DeleteDateTime                
   )    
    SELECT OrderNO                      = CASE WHEN @p_OperateMode = 1 THEN CAST(ROW_NUMBER()OVER(ORDER BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho) AS varchar) ELSE MAX(MAIN.HacchuuNO) END --後で採番、キーだけふっとく    
          ,OrderProcessNO               = NULL --後で採番    
          ,StoreCD        = @p_StoreCD    
          ,OrderDate                    = @p_OrderDate    
          ,ReturnFLG                    = '0'    
          ,OrderDataKBN                 = '1'    
          ,OrderWayKBN                  = CASE WHEN @p_IkkatuHacchuuMode = '0' THEN '1' ELSE '2' END
          ,OrderCD                      = MAIN.SiiresakiCD    
          ,OrderPerson                  = null
          ,AliasKBN                     = '1'    
          ,DestinationKBN               = CASE WHEN MAX(MAIN.ChokusouFLG) = '○' THEN 1 ELSE 2 END    
          ,DestinationName              = MAIN.NounyuusakiName    
          ,DestinationZip1CD            = MAX(MAIN.NounyuusakiYuubinNO1)    
          ,DestinationZip2CD            = MAX(MAIN.NounyuusakiYuubinNO2)    
          ,DestinationAddress1          = MAX(MAIN.NounyuusakiJuusho1)    
          ,DestinationAddress2          = MAX(MAIN.NounyuusakiJuusho2)    
          ,DestinationTelphoneNO        = MAX(MAIN.NounyuusakiTELNO)    
          ,DestinationFaxNO             = MAX(MAIN.NounyuusakiFAXNO)    
          ,DestinationSoukoCD           = MAX(MAIN.SoukoCD)    
          ,CurrencyCD                   = MAX(MCON.CurrencyCD)    
          ,OrderHontaiGaku              = SUM(CAST(ISNULL(MAIN.Hacchuugaku,0) AS money))    
          ,OrderTax8                    = SUM(CASE WHEN CAST(NULLIF(MAIN.TaxRate,'') as decimal) = 8 THEN ISNULL(CAST(MAIN.HacchuuShouhizeigaku AS money),0) ELSE 0 END)    
          ,OrderTax10                   = SUM(CASE WHEN CAST(NULLIF(MAIN.TaxRate,'') as decimal) = 10 THEN ISNULL(CAST(MAIN.HacchuuShouhizeigaku AS money),0) ELSE 0 END)    
          ,OrderGaku                    = SUM(CAST(ISNULL(MAIN.Hacchuugaku,0) AS money)) + SUM(CASE WHEN CAST(NULLIF(MAIN.TaxRate,'') as decimal) = 8.0 THEN CAST(ISNULL(MAIN.HacchuuShouhizeigaku,0) AS money) ELSE 0 END) + SUM(CASE WHEN CAST(NULLIF(MAIN.TaxRate,'') as decimal) = 10.0 THEN ISNULL(CAST(MAIN.HacchuuShouhizeigaku AS money),0) ELSE 0 END)    
          ,CommentOutStore              = MAX(MAIN.ShagaiBikou)    
          ,CommentInStore               = MAX(MAIN.ShanaiBikou)    
          ,StaffCD                      = @p_StaffCD    
          ,FirstArriveDate              = null    
          ,LastArriveDate               = null    
          ,ApprovalDate                 = @SYSDATE    
          ,LastApprovalDate             = @SYSDATE    
          ,LastApprovalStaffCD          = @p_Operator    
          ,ApprovalStageFLG             = 10    
          ,FirstPrintDate               = null    
          ,LastPrintDate                = null    
          ,ArrivePlanDate               = MAX(CAST(MAIN.KibouNouki as date))
          ,InsertOperator               = @p_Operator    
          ,InsertDateTime               = @SYSDATETIME    
          ,UpdateOperator               = @p_Operator    
          ,UpdateDateTime               = @SYSDATETIME    
          ,DeleteOperator               = null    
          ,DeleteDateTime               = null       
    FROM @p_TIkkatuHacchuuNyuuryoku MAIN    
    LEFT JOIN M_Control MCON    
    ON MCON.MainKey = '1'    
    GROUP BY MAIN.SiiresakiCD,MAIN.NounyuusakiName,MAIN.NounyuusakiJuusho    
    
    
/****************************************    
    更新(新規)    
*****************************************/    
IF @p_OperateMode = 1      
    BEGIN      
        SET @p_OperateModeNm = '新規'      
            
        --伝票番号採番（発注処理番号）    
        EXEC Fnc_GetNumber      
            20,             --in伝票種別 11      
            @p_OrderDate,   --in基準日      
            @p_StoreCD,       --in店舗CD      
            @p_Operator,      
            @OrderProcessNO OUTPUT    
    
        UPDATE #DOrder    
        SET OrderProcessNO = @OrderProcessNO    
    
        --ループ    
        DECLARE @cur_OrderNO varchar(11)    
        DECLARE @CUR CURSOR     
        SET @CUR = CURSOR LOCAL FAST_FORWARD FOR    
    
            SELECT OrderNO    
            FROM #DOrder    
        
        OPEN @CUR    
        FETCH NEXT FROM @CUR    
        INTO @cur_OrderNO    
    
        WHILE @@FETCH_STATUS = 0    
        BEGIN    
    
            --伝票番号採番（発注番号）     
            EXEC Fnc_GetNumber      
                2,             --in伝票種別 2
                @p_OrderDate,    --in基準日      
                @p_StoreCD,       --in店舗CD      
                @p_Operator,      
                @OrderNO OUTPUT      
                    
            UPDATE #DOrder    
            SET OrderNO = @OrderNO    
            WHERE OrderNO = @cur_OrderNO    
    
            UPDATE #DOrderDetails    
            SET OrderNO = @OrderNO    
               ,FirstOrderNO = @OrderNO    
            WHERE OrderNO = @cur_OrderNO    
    
        FETCH NEXT FROM @CUR    
        INTO @cur_OrderNO    
    
        END    
    
        CLOSE @CUR    
        DEALLOCATE @CUR    
  
        --D_Order更新    
        INSERT INTO D_Order(    
               OrderNO    
              ,OrderProcessNO    
              ,StoreCD    
              ,OrderDate    
              ,ReturnFLG    
              ,OrderDataKBN    
              ,OrderWayKBN    
              ,OrderCD    
              ,OrderPerson    
              ,AliasKBN    
              ,DestinationKBN    
              ,DestinationName    
              ,DestinationZip1CD    
              ,DestinationZip2CD    
              ,DestinationAddress1    
              ,DestinationAddress2    
              ,DestinationTelphoneNO    
              ,DestinationFaxNO    
              ,DestinationSoukoCD    
              ,CurrencyCD    
              ,OrderHontaiGaku    
              ,OrderTax8    
              ,OrderTax10    
              ,OrderGaku    
              ,CommentOutStore    
              ,CommentInStore    
              ,StaffCD    
              ,FirstArriveDate    
              ,LastArriveDate    
              ,ApprovalDate    
              ,LastApprovalDate    
              ,LastApprovalStaffCD    
              ,ApprovalStageFLG    
              ,FirstPrintDate    
              ,LastPrintDate    
              ,ArrivalPlanDate
              ,InsertOperator    
              ,InsertDateTime    
              ,UpdateOperator    
              ,UpdateDateTime    
              ,DeleteOperator    
              ,DeleteDateTime    
              )    
        SELECT OrderNO    
              ,OrderProcessNO    
              ,StoreCD    
              ,OrderDate    
              ,ReturnFLG    
              ,OrderDataKBN    
              ,OrderWayKBN    
              ,OrderCD    
              ,OrderPerson    
              ,AliasKBN    
              ,DestinationKBN    
              ,DestinationName    
              ,DestinationZip1CD    
              ,DestinationZip2CD    
              ,DestinationAddress1    
              ,DestinationAddress2    
              ,DestinationTelphoneNO    
              ,DestinationFaxNO    
              ,DestinationSoukoCD    
              ,CurrencyCD    
              ,OrderHontaiGaku    
              ,OrderTax8    
              ,OrderTax10    
              ,OrderGaku    
              ,CommentOutStore    
              ,CommentInStore    
              ,StaffCD    
              ,FirstArriveDate    
              ,LastArriveDate    
              ,ApprovalDate    
              ,LastApprovalDate    
              ,LastApprovalStaffCD    
              ,ApprovalStageFLG    
              ,FirstPrintDate    
              ,LastPrintDate    
              ,ArrivePlanDate
              ,InsertOperator    
              ,InsertDateTime    
              ,UpdateOperator    
              ,UpdateDateTime    
              ,DeleteOperator    
              ,DeleteDateTime    
        FROM #DOrder    
    
       --D_OrderDetails更新    
       INSERT INTO D_OrderDetails(    
               OrderNO    
              ,OrderRows    
              ,DisplayRows    
              ,JuchuuNO    
              ,JuchuuRows    
              ,SKUCD    
              ,AdminNO    
              ,JanCD    
              ,MakerItem
              ,ItemName    
              ,ColorName    
              ,SizeName    
              ,Remarks    
              ,OrderSu    
              ,TaniCD    
              ,PriceOutTax    
              ,Rate    
              ,OrderUnitPrice    
              ,OrderHontaiGaku    
              ,OrderTax    
              ,OrderTaxRitsu    
              ,OrderGaku    
              ,SoukoCD    
              ,DirectFLG    
              ,NotNetFLG    
              ,EDIFLG    
              ,DesiredDeliveryDate    
              ,ArrivePlanDate    
              ,TotalArrivalSu    
              ,CommentOutStore    
              ,CommentInStore    
              ,FirstOrderNO    
              ,FirstOrderRows    
              ,CancelOrderNO    
              ,AnswerFLG    
              ,EDIOutputDatetime    
              ,InsertOperator    
              ,InsertDateTime    
              ,UpdateOperator    
              ,UpdateDateTime    
              ,DeleteOperator    
              ,DeleteDateTime    
              )    
        SELECT OrderNO    
              ,OrderRows    
              ,DisplayRows    
              ,JuchuuNO    
              ,JuchuuRows    
              ,SKUCD    
              ,AdminNO    
              ,JanCD    
              ,MakerItem
              ,ItemName    
              ,ColorName    
              ,SizeName    
              ,Remarks    
              ,OrderSu    
              ,TaniCD    
              ,PriceOutTax    
              ,Rate    
              ,OrderUnitPrice    
              ,OrderHontaiGaku    
              ,OrderTax    
              ,OrderTaxRitsu    
              ,OrderGaku    
              ,SoukoCD    
              ,DirectFLG    
              ,NotNetFLG    
              ,EDIFLG    
              ,DesiredDeliveryDate    
              ,ArrivePlanDate    
              ,TotalArrivalSu    
              ,CommentOutStore    
              ,CommentInStore    
              ,FirstOrderNO    
              ,FirstOrderRows    
              ,CancelOrderNO    
              ,AnswerFLG    
              ,EDIOutputDatetime    
              ,InsertOperator    
              ,InsertDateTime    
              ,UpdateOperator    
              ,UpdateDateTime    
              ,DeleteOperator    
              ,DeleteDateTime    
        FROM #DOrderDetails    
      
       --D_LastOrder更新    
        INSERT INTO D_LastOrder(  
               JuchuuNO  
              ,JuchuuRows  
              ,OrderSEQ  
              ,OrderNO  
              ,OrderRows  
              ,OrderDate  
              ,StaffCD  
              ,OrderCD  
              ,CancelOrderFLG  
              ,CancelOrderDate  
              ,InsertOperator  
              ,InsertDateTime  
              ,UpdateOperator  
              ,UpdateDateTime  
              ,DeleteOperator  
              ,DeleteDateTime  
              )  
        SELECT JuchuuNO             = MAIN.JuchuuNO  
              ,JuchuuRows           = MAIN.JuchuuRows  
              ,OrderSEQ             = ISNULL(SUB.MAXOrderSEQ,0) + 1  
              ,OrderNO              = MAIN.OrderNO  
              ,OrderRows            = MAIN.OrderRows  
              ,OrderDate            = @SYSDATE  
              ,StaffCD              = @p_StaffCD  
              ,OrderCD              = DODH.OrderCD  
              ,CancelOrderFLG       = NULL  
              ,CancelOrderDate      = NULL  
              ,InsertOperator       = @p_Operator    
              ,InsertDateTime       = @SYSDATETIME    
              ,UpdateOperator       = @p_Operator    
              ,UpdateDateTime       = @SYSDATETIME    
              ,DeleteOperator       = null    
              ,DeleteDateTime       = null      
        FROM #DOrderDetails MAIN  
        INNER JOIN #DOrder DODH  
        ON MAIN.OrderNO = DODH.OrderNO  
        OUTER APPLY(SELECT Max(OrderSEQ) MAXOrderSEQ  
                      FROM D_LastOrder  
                     WHERE JuchuuNO = MAIN.JuchuuNO  
                       AND JuchuuRows = MAIN.JuchuuRows  
                       AND DeleteDateTime IS NULL  
                    GROUP BY JuchuuNO,JuchuuRows  
                   )SUB  
          
       --D_LastOrder更新    
 --その（受注番号＋明細番号の）商品の手配ができないため最初に発注した時と違う仕入先に発注した場合に、以前の発注先に対して「キャンセル」の発注を自動的に行うための処理  
        UPDATE MAIN  
        SET    CancelOrderFLG       = 1  
              ,CancelOrderDate      = NULL  
              ,UpdateOperator       = @p_Operator    
              ,UpdateDateTime       = @SYSDATETIME    
        FROM D_LastOrder MAIN  
        INNER JOIN D_OrderDetails DODD_mae  
        ON  DODD_mae.OrderNO = MAIN.OrderNO  
        AND DODD_mae.OrderRows = MAIN.OrderRows  
        AND DODD_mae.DeleteDateTime IS NULL  
        INNER JOIN D_Order DODH_mae  
        ON  DODH_mae.OrderNO = DODD_mae.OrderNO  
        AND DODH_mae.DeleteDateTime IS NULL  
        INNER JOIN #DOrderDetails DODD  
        ON  MAIN.JuchuuNO = DODD.JuchuuNO  
        AND MAIN.JuchuuRows = DODD.JuchuuRows  
        INNER JOIN #DOrder DODH  
        ON  DODH.OrderNO = DODD.OrderNO  
        WHERE DODH.OrderCD <> DODH_mae.OrderCD  
  
    END      
    
/****************************************    
    更新(変更)    
*****************************************/    
IF @p_OperateMode = 2    
    BEGIN      
        SET @p_OperateModeNm = '変更'      
    
        --D_OrderDetails削除    
        UPDATE DODD    
        SET UpdateOperator     =  @p_Operator        
           ,UpdateDateTime     =  @SYSDATETIME      
           ,DeleteOperator     =  @p_Operator        
           ,DeleteDateTime     =  @SYSDATETIME      
        FROM D_OrderDetails DODD    
        INNER JOIN D_Order DODH    
        ON DODD.OrderNO = DODH.OrderNO    
        WHERE (DODD.OrderNO = @p_OrderNO    
               OR    
               DODH.OrderProcessNO = @p_OrderProcessNO    
              )    
          AND NOT EXISTS(SELECT *    
                           FROM #DOrderDetails SUB    
                          WHERE SUB.OrderRows = DODD.OrderRows    
                        )    
          AND DODD.DeleteDateTime IS NULL    
  
        --D_Order削除    
        UPDATE DODH    
        SET UpdateOperator     =  @p_Operator        
           ,UpdateDateTime     =  @SYSDATETIME      
           ,DeleteOperator     =  @p_Operator        
           ,DeleteDateTime     =  @SYSDATETIME      
        FROM D_Order DODH    
        INNER JOIN (SELECT DODD.OrderNO    
                          ,MAX(CASE WHEN DeleteDateTime IS NULL THEN 1 ELSE 0 END) IsExistNotDeleteRow    
                      FROM D_OrderDetails DODD    
                     GROUP BY DODD.OrderNO    
                   ) DODD    
        ON DODH.OrderNO = DODD.OrderNO    
        WHERE (DODH.OrderNO = @p_OrderNO    
               OR    
               DODH.OrderProcessNO = @p_OrderProcessNO    
              )    
          AND DODD.IsExistNotDeleteRow = 0    
          AND DODH.DeleteDateTime IS NULL    
    
        --D_Order更新    
        UPDATE MAIN   
        SET    OrderDate            = SUB.OrderDate    
              ,OrderCD              = SUB.OrderCD    
              ,OrderHontaiGaku      = SUB.OrderHontaiGaku    
              ,OrderTax8            = SUB.OrderTax8    
              ,OrderTax10           = SUB.OrderTax10    
              ,OrderGaku            = SUB.OrderGaku    
              ,CommentOutStore      = SUB.CommentOutStore    
              ,CommentInStore       = SUB.CommentInStore    
              ,StaffCD              = SUB.StaffCD    
              ,UpdateOperator       = SUB.UpdateOperator    
              ,UpdateDateTime       = SUB.UpdateDateTime    
        FROM D_Order MAIN  
        INNER JOIN #DOrder SUB  
        ON MAIN.OrderNO = SUB.OrderNO  
    
       --D_OrderDetails更新    
       UPDATE MAIN    
       SET     OrderSu                  = SUB.OrderSu    
              ,OrderHontaiGaku          = SUB.OrderHontaiGaku   
              ,OrderTax                 = SUB.OrderTax    
              ,OrderGaku                = SUB.OrderGaku    
              ,OrderUnitPrice           = SUB.OrderUnitPrice  
              ,EDIFLG                   = SUB.EDIFLG    
              ,DesiredDeliveryDate      = SUB.DesiredDeliveryDate   
              ,CommentOutStore          = SUB.CommentOutStore   
              ,CommentInStore           = SUB.CommentInStore    
              ,UpdateOperator           = SUB.UpdateOperator    
              ,UpdateDateTime           = SUB.UpdateDateTime    
        FROM D_OrderDetails MAIN  
        INNER JOIN #DOrderDetails SUB  
        ON MAIN.OrderNO = SUB.OrderNO  
        AND MAIN.OrderRows = SUB.OrderRows  
          
       --D_LastOrder更新    
        UPDATE MAIN  
        SET    UpdateOperator       = @p_Operator    
              ,UpdateDateTime       = @SYSDATETIME    
        FROM D_LastOrder MAIN  
        INNER JOIN #DOrderDetails DODD  
        ON  MAIN.JuchuuNO = DODD.JuchuuNO  
        AND MAIN.JuchuuRows = DODD.JuchuuRows  
        INNER JOIN #DOrder DODH  
        ON  DODH.OrderNO = DODD.OrderNO  
        INNER JOIN D_Order DODH_mae  
        ON  DODH_mae.OrderNO = DODH.OrderNO  
      
    END      
        
    ELSE IF @p_OperateMode = 3 --削除--      
    BEGIN      
        SET @p_OperateModeNm = '削除'    
              
        UPDATE D_Order      
            SET UpdateOperator     =  @p_Operator        
               ,UpdateDateTime     =  @SYSDATETIME      
               ,DeleteOperator     =  @p_Operator        
               ,DeleteDateTime     =  @SYSDATETIME      
        WHERE (OrderNO = @p_OrderNO    
               OR    
               OrderProcessNO = @p_OrderProcessNO    
              )    
        AND DeleteDateTime IS NULL    
      
        UPDATE DODD    
            SET UpdateOperator     =  @p_Operator        
               ,UpdateDateTime     =  @SYSDATETIME      
               ,DeleteOperator     =  @p_Operator        
               ,DeleteDateTime     =  @SYSDATETIME     
        FROM D_OrderDetails DODD    
        INNER JOIN D_Order DODH    
        ON  DODD.OrderNO = DODH.OrderNO    
        WHERE (DODD.OrderNO = @p_OrderNO    
               OR    
               DODH.OrderProcessNO = @p_OrderProcessNO    
              )    
        AND DODD.DeleteDateTime IS NULL    
      
        UPDATE MAIN  
        SET    UpdateOperator       = @p_Operator    
              ,UpdateDateTime       = @SYSDATETIME    
              ,DeleteOperator       = null    
              ,DeleteDateTime       = null      
        FROM D_LastOrder MAIN  
        INNER JOIN #DOrderDetails DODD  
        ON  MAIN.JuchuuNO = DODD.JuchuuNO  
        AND MAIN.JuchuuRows = DODD.JuchuuRows  
      
    END     
       
    --処理履歴データへ更新      
    SET @KeyItem = @OrderNO      
              
    EXEC L_Log_Insert_SP      
        @SYSDATETIME,      
        @p_Operator,      
        'IkkatsuHacchuuNyuuryoku',      
        @p_PC,      
        @p_OperateModeNm,      
        @KeyItem      

END
GO


