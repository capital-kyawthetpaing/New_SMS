 BEGIN TRY 
 Drop Procedure dbo.[PRC_HacchuuNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_HacchuuNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @OrderNO   varchar(11),
    @StoreCD   varchar(4),
    @OrderDate  varchar(10),
    @ReturnFLG tinyint ,
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    @OrderCD   varchar(13),
    @OrderPerson   varchar(50),
    @AliasKBN   tinyint,
    @DestinationKBN tinyint,
    @DestinationName varchar(80),
    @ZipCD1   varchar(3),
    @ZipCD2   varchar(4),
    @Address1   varchar(100),
    @Address2   varchar(100),
    @DestinationTelphoneNO   varchar(15),
    @DestinationFaxNO   varchar(15),
    @DestinationSoukoCD  varchar(6),
    @OrderHontaiGaku money ,
    @OrderTax8 money ,
    @OrderTax10 money ,
    @OrderGaku money ,

    @CommentOutStore varchar(80) ,
    @CommentInStore varchar(80) ,
    @ApprovalEnabled tinyint,	--承認ボタンが利用できない場合=0
    @ApprovalStageFLG int,

    @Table  T_Order READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
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
    DECLARE @ReserveNO varchar(11);
    
    DECLARE @Num1 int;
    DECLARE @SYSDATE date;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

	SELECT @Num1 = ISNULL((SELECT A.Num1 FROM M_MultiPorpose AS A WHERE A.ID = 318 AND A.[Key] = 1 ),0);
	SET @SYSDATE = CONVERT(date, @SYSDATETIME);
	
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --伝票番号採番
        EXEC Fnc_GetNumber
            2,             --in伝票種別 2
            @OrderDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @OrderNO OUTPUT
            ;
        
        IF ISNULL(@OrderNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --【D_Order】Table転送仕様Ａ
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
           ,convert(date,@OrderDate)  
           ,@ReturnFLG
           ,2   --OrderDataKBN
           ,2   --OrderWayKBN
           ,@OrderCD
           ,@OrderPerson
           ,@AliasKBN
           ,@DestinationKBN
           ,@DestinationName
           ,@ZipCD1
           ,@ZipCD2
           ,@Address1
           ,@Address2
           ,@DestinationTelphoneNO
           ,@DestinationFaxNO
           ,@DestinationSoukoCD
           ,(SELECT A.CurrencyCD FROM M_Control AS A WHERE A.[MainKey] = 1)--CurrencyCD
           ,@OrderHontaiGaku
           ,@OrderTax8
           ,@OrderTax10
           ,@OrderGaku
           ,@CommentOutStore
           ,@CommentInStore
           ,@StaffCD
           ,NULL    --FirstArriveDate
           ,NULL    --LastArriveDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @SYSDATE END) --ApprovalDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @SYSDATE END) --LastApprovalDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @Operator END) --LastApprovalStaffCD
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN 1
            ELSE 10 END) --ApprovalStageFLG
           ,NULL    --FirstPrintDate
           ,NULL    --LastPrintDate

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

    END
        
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
		
        UPDATE D_Order
           SET [StoreCD] = @StoreCD                         
              ,[OrderDate] = @OrderDate

              ,[OrderCD] = @OrderCD
              ,[OrderPerson] = @OrderPerson
              ,[AliasKBN] = @AliasKBN
              ,[DestinationKBN] = @DestinationKBN
              ,[DestinationName] = @DestinationName
              ,[DestinationZip1CD] = @ZipCD1
              ,[DestinationZip2CD] = @ZipCD2
              ,[DestinationAddress1] = @Address1
              ,[DestinationAddress2] = @Address2
              ,[DestinationTelphoneNO] = @DestinationTelphoneNO
              ,[DestinationFaxNO] = @DestinationFaxNO
              ,[DestinationSoukoCD] = @DestinationSoukoCD
              ,[OrderHontaiGaku] = @OrderHontaiGaku
              ,[OrderTax8]       = @OrderTax8
              ,[OrderTax10]      = @OrderTax10
              ,[OrderGaku]       = @OrderGaku
              ,[CommentOutStore] = @CommentOutStore
              ,[CommentInStore]  = @CommentInStore
              ,[StaffCD]         = @StaffCD

              ,[ApprovalDate] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN @SYSDATE ELSE [ApprovalDate] END)
                                        ELSE @SYSDATE END) --ApprovalDate
              ,[LastApprovalDate] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE @ApprovalStageFLG WHEN 9 THEN @SYSDATE WHEN -1 THEN NULL ELSE [ApprovalDate] END) ELSE [ApprovalDate] END)
                                            ELSE @SYSDATE END) --LastApprovalDate
              ,[LastApprovalStaffCD] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE @ApprovalStageFLG WHEN 9 THEN @Operator WHEN -1 THEN NULL ELSE [LastApprovalStaffCD] END) ELSE [LastApprovalStaffCD] END)
                                            ELSE @Operator END) --LastApprovalStaffCD
              ,[ApprovalStageFLG] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE @ApprovalStageFLG WHEN -1 THEN 0 ELSE @ApprovalStageFLG END) ELSE [ApprovalStageFLG] END)
                                        ELSE 10 END) --ApprovalStageFLG
      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE OrderNO = @OrderNO
           ;
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        UPDATE [D_Order]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [OrderNO] = @OrderNO
         ;

    END
    
    --【D_OrderDetails】
    IF @OperateMode <= 2    --新規・修正時
    BEGIN
        
        --行削除されたデータはDELETE処理
        UPDATE D_OrderDetails
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [OrderNO] = @OrderNO
         AND NOT EXISTS (SELECT 1 FROM @Table tbl WHERE tbl.OrderRows = D_OrderDetails.[OrderRows]
         )
         ;
         
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
                   ,tbl.OrderRows                       
                   ,tbl.DisplayRows 

                   ,NULL	--JuchuuNO
                   ,0	--JuchuuRows
                   ,tbl.SKUCD
                   ,tbl.SKUNO
                   ,tbl.JanCD
                   ,tbl.SKUName
                   ,tbl.ColorName
                   ,tbl.SizeName

                   ,tbl.OrderSuu
                   ,tbl.TaniCD
                   ,tbl.PriceOutTax
                   ,tbl.Rate
                   ,tbl.OrderUnitPrice
                   ,tbl.OrderHontaiGaku
                   ,tbl.OrderTax
                   ,tbl.OrderTaxRitsu
                   ,tbl.OrderGaku
                   ,tbl.SoukoCD
                   ,tbl.DirectFLG
                   
                   ,tbl.EDIFLG
                   ,tbl.DesiredDeliveryDate
                   ,NULL	--ArrivePlanDate
                   ,0	--TotalArrivalSu
                   ,tbl.CommentOutStore
                   ,tbl.CommentInStore
                   ,@OrderNO        --FirstOrderNO               
                   ,tbl.OrderRows	--FirstOrderRows
                   ,0	--AnswerFLG
                   --,[EDIOutputDatetime]

                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;

        UPDATE [D_OrderDetails]
           SET [DisplayRows] = tbl.DisplayRows                  
               ,[AdminNO] = tbl.SKUNO                              
               ,[SKUCD] = tbl.SKUCD                              
               ,[JanCD] = tbl.JanCD                              
               ,[ItemName] = tbl.SKUName                          
               ,[ColorName] = tbl.ColorName                      
               ,[SizeName] = tbl.SizeName   
               ,[OrderSu] = tbl.OrderSuu  
               ,[PriceOutTax] = tbl.PriceOutTax     
               ,[Rate] = tbl.Rate                      
               ,[OrderUnitPrice] = tbl.OrderUnitPrice          
               ,[TaniCD] = tbl.TaniCD      
               ,[OrderGaku] = tbl.OrderGaku                    
               ,[OrderHontaiGaku] = tbl.OrderHontaiGaku        
               ,[OrderTax] = tbl.OrderTax                      
               ,[OrderTaxRitsu] = tbl.OrderTaxRitsu        
                  
               ,[SoukoCD] = tbl.SoukoCD                            
               ,[DirectFLG] = tbl.DirectFLG              
               ,[EDIFLG] = tbl.EDIFLG                        
               ,[DesiredDeliveryDate] = tbl.DesiredDeliveryDate  
               ,[CommentOutStore] = tbl.CommentOutStore          
               ,[CommentInStore] = tbl.CommentInStore            
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM D_OrderDetails
        INNER JOIN @Table tbl
         ON @OrderNO = D_OrderDetails.OrderNO
         AND tbl.OrderRows = D_OrderDetails.OrderRows
         AND tbl.UpdateFlg = 1
         ;
    END
    ELSE    --削除
    BEGIN
        UPDATE [D_OrderDetails]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [OrderNO] = @OrderNO
         ;
    END
	
    --処理履歴データへ更新
    SET @KeyItem = @OrderNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'HacchuuNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutOrderNo = @OrderNO;
    
--<<OWARI>>
  return @W_ERR;

END


