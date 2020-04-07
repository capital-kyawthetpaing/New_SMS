 BEGIN TRY 
 Drop Procedure dbo.[D_Order_ExecForNyuka]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  ======================================================================
--       Program Call    入荷入力
--       Program ID      NyuukaNyuuryoku
--       Create date:    2020.03.26
--    ======================================================================

CREATE PROCEDURE [dbo].[D_Order_ExecForNyuka]
    (@StoreCD   varchar(4),    
    @ChangeDate  varchar(10),
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    
    @OrderWayKBN tinyint,
    @OrderCD varchar(13),
    @OrderPerson varchar(50),
    @AliasKBN tinyint,

    @AdminNO int ,
    @SKUCD   varchar(30),
    @SKUName varchar(80) ,
    @JANCD   varchar(13),
    @ColorName varchar (20) ,
    @SizeName varchar (20) ,
    @OrderSuu int  ,		--追加入荷予定数 ArrivalPlanSu
    @OrderUnitPrice money  ,	
    @TaniCD varchar (2) ,
    @PriceOutTax money  ,
    @Rate decimal (5,2) ,
    @OrderHontaiGaku money  ,
    @OrderTax money  ,
    @OrderTaxRitsu int  ,
    @Operator  varchar(10),
    @OutOrderNo varchar(11) OUTPUT
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
    DECLARE @OrderNO varchar(11);
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO  varchar(11);
    --DECLARE @ReserveNO varchar(11);
    DECLARE @SYSDATE date;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @SYSDATE = CONVERT(date, @SYSDATETIME);
	
    --伝票番号採番
    EXEC Fnc_GetNumber
        2,             --in伝票種別 2
        @ChangeDate, --in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @OrderNO OUTPUT
        ;
    
    IF ISNULL(@OrderNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END
    
    --【D_Order】テーブル転送仕様①
    INSERT INTO [D_Order]
       ([OrderNO]
       ,[OrderProcessNO]
       ,[StoreCD]
       ,[OrderDate]
       ,[ReturnFLG]
       ,[OrderDataKBN]
       ,[OrderWayKBN]
       ,[OrderCD]
       ,[OrderPerson]
       ,[AliasKBN]
       ,[DestinationKBN]
       ,[DestinationName]
       ,[DestinationZip1CD]
       ,[DestinationZip2CD]
       ,[DestinationAddress1]
       ,[DestinationAddress2]
       ,[DestinationTelphoneNO]
       ,[DestinationFaxNO]
       ,[DestinationSoukoCD]
       ,[CurrencyCD]
       ,[OrderHontaiGaku]
       ,[OrderTax8]
       ,[OrderTax10]
       ,[OrderGaku]
       ,[CommentOutStore]
       ,[CommentInStore]
       ,[StaffCD]
       ,[FirstArriveDate]
       ,[LastArriveDate]
       ,[ApprovalDate]
       ,[LastApprovalDate]
       ,[LastApprovalStaffCD]
       ,[ApprovalStageFLG]
       ,[FirstPrintDate]
       ,[LastPrintDate]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
	VALUES
       (@OrderNO     
       ,NULL                 
       ,@StoreCD                          
       ,convert(date,@ChangeDate)
       ,0	--ReturnFLG
       ,2   --OrderDataKBN
       ,@OrderWayKBN
       ,@OrderCD
       ,@OrderPerson
       ,@AliasKBN
       ,2	--DestinationKBN
       ,NULL	--DestinationName
       ,NULL	--ZipCD1
       ,NULL	--ZipCD2
       ,NULL	--Address1
       ,NULL	--Address2
       ,NULL	--DestinationTelphoneNO
       ,NULL	--DestinationFaxNO
       ,@SoukoCD	--DestinationSoukoCD
       ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.[MainKey] = 1)--CurrencyCD
       ,@OrderHontaiGaku
       ,(CASE @OrderTaxRitsu WHEN 8 THEN @OrderTax ELSE 0 END) --AS OrderTax8
       ,(CASE @OrderTaxRitsu WHEN 10 THEN @OrderTax ELSE 0 END) --AS OrderTax10
       ,@OrderHontaiGaku + @OrderTax --AS OrderGaku
       ,NULL		--CommentOutStore
       ,NULL		--CommentInStore
       ,@StaffCD
       ,@SYSDATE    --FirstArriveDate
       ,@SYSDATE    --LastArriveDate
       ,@SYSDATE 	--ApprovalDate
       ,@SYSDATE 	--LastApprovalDate
       ,NULL 		--LastApprovalStaffCD
       ,10 			--ApprovalStageFLG
       ,@SYSDATE    --FirstPrintDate
       ,@SYSDATE    --LastPrintDate

       ,'Nyuuka'	--Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
       ,NULL                  
       ,NULL
       );               

	--【D_OrderDetails】テーブル転送仕様②
    INSERT INTO [D_OrderDetails]
               ([OrderNO]
               ,[OrderRows]
               ,[DisplayRows]
               ,[JuchuuNO]
               ,[JuchuuRows]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[ItemName]
               ,[ColorName]
               ,[SizeName]

               ,[OrderSu]
               ,[TaniCD]
               ,[PriceOutTax]
               ,[Rate]
               ,[OrderUnitPrice]
               ,[OrderHontaiGaku]
               ,[OrderTax]
               ,[OrderTaxRitsu]
               ,[OrderGaku]
               ,[SoukoCD]
               ,[DirectFLG]
               
               ,[EDIFLG]
               ,[DesiredDeliveryDate]
               ,[ArrivePlanDate]
               ,[TotalArrivalSu]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[FirstOrderNO]
               ,[FirstOrderRows]
               ,[AnswerFLG]
               --,[EDIOutputDatetime]

               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
         SELECT @OrderNO                         
               ,1 AS OrderRows                       
               ,1 AS DisplayRows 

               ,NULL	--JuchuuNO
               ,0	--JuchuuRows
               ,@SKUCD
               ,@AdminNO
               ,@JanCD
               ,@SKUName
               ,@ColorName
               ,@SizeName

               ,@OrderSuu 
               ,@TaniCD
               ,@PriceOutTax
               ,@Rate
               ,@OrderUnitPrice
               ,@OrderHontaiGaku
               ,@OrderTax
               ,@OrderTaxRitsu
               ,@OrderHontaiGaku + @OrderTax AS OrderGaku
               ,@SoukoCD
               ,0 AS DirectFLG
               
               ,0 AS EDIFLG
               ,@SYSDATE AS DesiredDeliveryDate
               ,@SYSDATE AS ArrivePlanDate
               ,0	--TotalArrivalSu
               ,NULL AS CommentOutStore
               ,NULL AS CommentInStore
               ,@OrderNO        --FirstOrderNO               
               ,1	--FirstOrderRows
               ,0	--AnswerFLG
               --,[EDIOutputDatetime]

               ,'Nyuuka'
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
          ;
    
    --【L_OrderHistory】
    --【L_OrderDetailsHistory】
    --【D_ArrivalPlan】テーブル転送仕様⑤
    --伝票番号採番
    EXEC Fnc_GetNumber
        22,             --in伝票種別 5
        @ChangeDate, --in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @ArrivalPlanNO OUTPUT
        ;
    
    IF ISNULL(@ArrivalPlanNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END

    --【D_ArrivalPlan】（Insert）
    INSERT INTO [D_ArrivalPlan]
           ([ArrivalPlanNO]
           ,[ArrivalPlanKBN]
           ,[Number]
           ,[NumberRows]
           ,[NumberSEQ]
           ,[ArrivalPlanDate]
           ,[ArrivalPlanMonth]
           ,[ArrivalPlanCD]
           ,[CalcuArrivalPlanDate]
           ,[ArrivalPlanUpdateDateTime]
           ,[StaffCD]
           ,[LastestFLG]
           ,[EDIImportNO]
           ,[SoukoCD]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[ArrivalPlanSu]
           ,[ArrivalSu]
           ,[OriginalArrivalPlanNO]
           ,[OrderCD]
           ,[FromSoukoCD]
           ,[ToStoreCD]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            @ArrivalPlanNO
           ,1 AS ArrivalPlanKBN
           ,@OrderNO AS Number
           ,1 AS NumberRows
           ,1 AS NumberSEQ
           ,@SYSDATE AS ArrivalPlanDate
           ,NULL AS ArrivalPlanMonth
           ,NULL AS ArrivalPlanCD
           ,@SYSDATE AS CalcuArrivalPlanDate
           ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
           ,@StaffCD
           ,1 AS LastestFLG
           ,NULL AS EDIImportNO
           ,@SoukoCD
           ,@SKUCD
           ,@AdminNO
           ,@JanCD
           ,@OrderSuu	--ArrivalPlanSu	追加入荷予定数
           ,0   --ArrivalSu
           ,NULL AS ArrivalPlanNO	--元のレコードのArrivalPlanNO
           ,@OrderCD
           ,NULL AS FromSoukoCD
           ,NULL AS ToStoreCD
           ,'Nyuuka'	--Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
     ;
    
    --【D_Stock】テーブル転送仕様⑥
    --伝票番号採番
    EXEC Fnc_GetNumber
        21,             --in伝票種別 21
        @ChangeDate, --in基準日
        @StoreCD,       --in店舗CD
        @Operator,
        @StockNO OUTPUT
        ;
    
    IF ISNULL(@StockNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END
    
    --Form.Details.Sub.在庫番号＝Null
    --【D_Stock】（Insert）
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
           ,NULL    --RackNO
           ,@ArrivalPlanNO
           ,@SKUCD
           ,@AdminNO
           ,@JanCD
           ,1   --  ArrivalYetFLG
           ,1 AS ArrivalPlanKBN
           ,@SYSDATE AS ArrivalPlanDate
           ,NULL    --ArrivalDate
           ,0   --StockSu
           ,@OrderSuu AS PlanSu	--追加入荷予定数
           ,0   --AllowableSu
           ,0   --AnotherStoreAllowableSu
           ,0    --ReserveSu
           ,0 AS InstructionSu
           ,0 AS ShippingSu
           ,NULL AS OriginalStockNO
           ,NULL AS ExpectReturnDate
           ,NULL ASReturnDate
           ,0 AS ReturnSu
     
           ,'Nyuuka'	--Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
    ;
	
    SET @OutOrderNo = @OrderNo;


--<<OWARI>>
  --return @W_ERR;

END


