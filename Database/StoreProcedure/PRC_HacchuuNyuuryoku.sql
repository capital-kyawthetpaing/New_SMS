DROP  PROCEDURE [dbo].[D_Order_SelectData]
GO
DROP  PROCEDURE [dbo].[SelectApprovalData]
GO
DROP  PROCEDURE [dbo].[PRC_HacchuuNyuuryoku]
GO


--  ======================================================================
--       Program Call    î≠íçì¸óÕ
--       Program ID      HacchuuNyuuryoku
--       Create date:    2019.8.26
--    ======================================================================
CREATE PROCEDURE D_Order_SelectData
    (@OperateMode    tinyint,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
    @OrderNO varchar(11)
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

--        IF @OperateMode = 2   --èCê≥éû
--        BEGIN
            SELECT DH.OrderNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.OrderDate,111) AS OrderDate
                  ,DH.ReturnFLG
                  ,DH.StaffCD
                  ,DH.OrderCD
                  ,DH.OrderPerson
                  ,DH.DestinationKBN
                  ,DH.DestinationName
                  ,DH.AliasKBN
                  ,DH.DestinationZip1CD
                  ,DH.DestinationZip2CD
                  ,DH.DestinationAddress1
                  ,DH.DestinationAddress2
                  ,DH.DestinationTelphoneNO
                  ,DH.DestinationFaxNO
                  ,DH.DestinationSoukoCD
                  ,DH.ApprovalStageFLG
                  ,(CASE DH.ApprovalStageFLG WHEN 0 THEN 'ãpâ∫' WHEN 1 THEN 'ê\êøíÜ' WHEN 9 THEN 'è≥îFçœ' ELSE 'è≥îFíÜ' END) AS ApprovalStage
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.OrderRows
                  ,DM.DisplayRows
                  ,DM.AdminNO
                  ,DM.SKUCD
                  ,DM.JanCD
                  ,DM.MakerItem
                  ,DM.ItemName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.Rate
                  ,CONVERT(varchar,DM.ArrivePlanDate,111) AS ArrivePlanDate
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore

                  ,DM.OrderSu
                  ,DM.OrderUnitPrice
                  ,DM.TaniCD
                  ,DM.OrderHontaiGaku
                  
                  ,CONVERT(varchar,DM.DesiredDeliveryDate,111) AS DesiredDeliveryDate
                  ,DM.EDIFLG
                  

              FROM D_Order DH
              LEFT OUTER JOIN D_OrderDetails AS DM ON DH.OrderNO = DM.OrderNO AND DM.DeleteDateTime IS NULL
           
              WHERE DH.OrderNO = @OrderNO 
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.OrderNO, DM.OrderRows
                ;
--        END

END

GO

CREATE PROCEDURE SelectApprovalData
    (    @Operator  varchar(10)
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
    SELECT distinct M.StoreCD
        ,M.ApprovalStaffCD11
        ,M.ApprovalStaffCD12
        ,M.ApprovalStaffCD21
        ,M.ApprovalStaffCD22
        ,M.ApprovalStaffCD31
        ,M.ApprovalStaffCD32
    FROM M_Store AS M
    INNER JOIN (SELECT M.StoreCD,MAX(M.ChangeDate) AS ChangeDate
    	FROM M_Store AS M
    	WHERE M.ChangeDate <= CONVERT(date, SYSDATETIME())
    	GROUP BY M.StoreCD) AS MM
    ON M.StoreCD = MM.StoreCD
    AND M.ChangeDate = MM.ChangeDate
    WHERE (M.ApprovalStaffCD11 = @Operator
        OR M.ApprovalStaffCD12 = @Operator
        OR M.ApprovalStaffCD21 = @Operator
        OR M.ApprovalStaffCD22 = @Operator
        OR M.ApprovalStaffCD31 = @Operator
        OR M.ApprovalStaffCD32 = @Operator)
    AND M.DeleteFlg = 0
    ;

END

GO


--CREATE TYPE T_Order AS TABLE
--    (
--    [OrderRows] [int],
--    [DisplayRows] [int],
--    [SKUNO] [int] ,
--    [SKUCD] [varchar](30) ,
--    [JanCD] [varchar](13) ,
--    [MakerItem] [varchar](30) ,
--    [SKUName] [varchar](80) ,
--    [ColorName] [varchar](20) ,
--    [SizeName] [varchar](20) ,
--    [OrderSuu] [int] ,
--    [OrderUnitPrice] [money] ,
--    [TaniCD] [varchar](2) ,
--    [PriceOutTax] [money] ,
--    [Rate] [decimal](5,2) ,
--    [OrderGaku] [money] ,
--    [OrderHontaiGaku] [money] ,
--    [OrderTax] [money] ,
--    [OrderTaxRitsu] [int] ,
--    [SoukoCD] [varchar](6) ,
--    [DirectFLG] [tinyint],
--    [EDIFLG] [tinyint] ,
--    [DesiredDeliveryDate] [date],
--    [CommentOutStore] [varchar](80) ,
--    [CommentInStore] [varchar](80) ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE PRC_HacchuuNyuuryoku
    (@OperateMode    int,                 -- èàóùãÊï™Åi1:êVãK 2:èCê≥ 3:çÌèúÅj
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

    @CommentOutStore varchar(500) ,
    @CommentInStore varchar(500) ,
    @ArrivalPlanDate varchar(10),
    @ApprovalEnabled tinyint,	--è≥îFÉ{É^ÉìÇ™óòópÇ≈Ç´Ç»Ç¢èÍçá=0
    @ApprovalStageFLG int,

    @Table  T_Order READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutOrderNo varchar(11) OUTPUT
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
    DECLARE @ReserveNO varchar(11);
    
    DECLARE @Num1 int;
    DECLARE @SYSDATE date;
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

	SELECT @Num1 = ISNULL((SELECT A.Num1 FROM M_MultiPorpose AS A WHERE A.ID = 318 AND A.[Key] = 1 ),0);
	SET @SYSDATE = CONVERT(date, @SYSDATETIME);
	
    --êVãK--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = 'êVãK';
        
        --ì`ï[î‘çÜçÃî‘
        EXEC Fnc_GetNumber
            2,             --inì`ï[éÌï  2
            @OrderDate, --inäÓèÄì˙
            @StoreCD,       --inìXï‹CD
            @Operator,
            @OrderNO OUTPUT
            ;
        
        IF ISNULL(@OrderNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --ÅyD_OrderÅzTableì]ëóédólÇ`
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
          ,[ArrivalPlanDate]
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
           ,@ArrivalPlanDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @SYSDATE END) --ApprovalDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @SYSDATE END) --LastApprovalDate
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN NULL
            ELSE @Operator END) --LastApprovalStaffCD
           ,(CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalStageFLG >= 9 THEN @ApprovalStageFLG ELSE 1 END)
            ELSE 10 END)--ApprovalStageFLG
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
        
    --ïœçX--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = 'ïœçX';
		
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
              
              ,[ApprovalDate] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN @SYSDATE ELSE NULL END)
                                        ELSE @SYSDATE END) --ApprovalDate
              ,[LastApprovalDate] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE WHEN @ApprovalStageFLG >= 9 THEN @SYSDATE WHEN @ApprovalStageFLG = -1 THEN NULL ELSE [LastApprovalDate] END) 
                                                                                ELSE NULL END)
                                            ELSE @SYSDATE END) --LastApprovalDate
              ,[LastApprovalStaffCD] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE WHEN @ApprovalStageFLG >= 9 THEN @Operator WHEN @ApprovalStageFLG = -1 THEN NULL ELSE [LastApprovalStaffCD] END) 
                                                                                ELSE NULL END)
                                            ELSE @Operator END) --LastApprovalStaffCD
              ,[ApprovalStageFLG] = (CASE WHEN @OrderHontaiGaku > @Num1 THEN (CASE WHEN @ApprovalEnabled = 1 THEN (CASE @ApprovalStageFLG WHEN -1 THEN 0 ELSE @ApprovalStageFLG END) ELSE 1 END)
                                        ELSE 10 END) --ApprovalStageFLG
              ,[ArrivalPlanDate]  = @ArrivalPlanDate
      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE OrderNO = @OrderNO
           ;
    END
    
    ELSE IF @OperateMode = 3 --çÌèú--
    BEGIN
        SET @OperateModeNm = 'çÌèú';
        
        UPDATE [D_Order]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [OrderNO] = @OrderNO
         ;

    END
    
    --ÅyD_OrderDetailsÅz
    IF @OperateMode <= 2    --êVãKÅEèCê≥éû
    BEGIN
        
        --çsçÌèúÇ≥ÇÍÇΩÉfÅ[É^ÇÕDELETEèàóù
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
                   ,[MakerItem]
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
                   ,tbl.MakerItem
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
               ,[MakerItem] = tbl.MakerItem                
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
    ELSE    --çÌèú
    BEGIN
        UPDATE [D_OrderDetails]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [OrderNO] = @OrderNO
         ;
    END
	
    --èàóùóöóÉfÅ[É^Ç÷çXêV
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

GO
