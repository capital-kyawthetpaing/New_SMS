
IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_Register')
begin
    DROP PROCEDURE PRC_Hacchuusho_Register
end
GO

CREATE PROCEDURE PRC_Hacchuusho_Register(
     @p_Operator                  varchar(10)   
    ,@p_StoreCD                   varchar(4)   
    ,@p_StaffCD                   varchar(10)   
    ,@p_OrderCD                   varchar(10)   
    ,@p_OrderNO                   varchar(11)   
)AS
BEGIN


    DECLARE @SYSDATETIME datetime    
    DECLARE @SYSDATE date    
    SET @SYSDATETIME = SYSDATETIME()    
    SET @SYSDATE = CONVERT(datetime, @SYSDATETIME)    

    DECLARE @OrderDate date    
    DECLARE @OrderProcessNO varchar(11)
    DECLARE @OrderNO varchar(11)

    SELECT @OrderDate = OrderDate
    FROM D_Order
    WHERE OrderNO = @p_OrderNO

    --ì`ï[î‘çÜçÃî‘Åiî≠íçèàóùî‘çÜÅj  
    EXEC Fnc_GetNumber    
        20,             --inì`ï[éÌï  11    
        @OrderDate,   --inäÓèÄì˙    
        @p_StoreCD,       --inìXï‹CD    
        @p_Operator,    
        @OrderProcessNO OUTPUT  

    --ì`ï[î‘çÜçÃî‘Åiî≠íçî‘çÜÅj   
    EXEC Fnc_GetNumber    
        11,             --inì`ï[éÌï  11    
        @OrderDate,    --inäÓèÄì˙    
        @p_StoreCD,       --inìXï‹CD    
        @p_Operator,    
        @OrderNO OUTPUT    

    --D_OrderDetailsçXêV  
    INSERT INTO D_OrderDetails(  
             OrderNO  
            ,OrderRows  
            ,DisplayRows  
            ,JuchuuNO  
            ,JuchuuRows  
    --        ,JuchuuOrderNO  
            ,SKUCD  
            ,AdminNO  
            ,JanCD  
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
    --        ,LastArrivePlanNO  
    --        ,LastArriveDatetime  
    --        ,LastArriveNO  
            ,InsertOperator  
            ,InsertDateTime  
            ,UpdateOperator  
            ,UpdateDateTime  
            ,DeleteOperator  
            ,DeleteDateTime  
            )  
    SELECT   OrderNO                    = @OrderNO
            ,OrderRows                  = ROW_NUMBER()OVER(PARTITION BY DLOR.OrderCD,DLOR.OrderNO ORDER BY DLOR.OrderRows)  
            ,DisplayRows                = ROW_NUMBER()OVER(PARTITION BY DLOR.OrderCD,DLOR.OrderNO ORDER BY DLOR.OrderRows)
            ,JuchuuNO                   = DLOR.JuchuuNO
            ,JuchuuRows                 = DLOR.JuchuuRows
       --     ,JuchuuOrderNO              = null
            ,SKUCD                      = DODD.SKUCD
            ,AdminNO                    = DODD.AdminNO
            ,JanCD                      = DODD.JanCD
            ,ItemName                   = DODD.ItemName
            ,ColorName                  = DODD.ColorName
            ,SizeName                   = DODD.SizeName
            ,Remarks                    = DODD.Remarks
            ,OrderSu                    = DODD.OrderSu
            ,TaniCD                     = DODD.TaniCD
            ,PriceOutTax                = DODD.PriceOutTax
            ,Rate                       = DODD.Rate
            ,OrderUnitPrice             = DODD.OrderUnitPrice
            ,OrderHontaiGaku            = DODD.OrderHontaiGaku
            ,OrderTax                   = DODD.OrderTax
            ,OrderTaxRitsu              = DODD.OrderTaxRitsu
            ,OrderGaku                  = DODD.OrderGaku
            ,SoukoCD                    = DODD.SoukoCD
            ,DirectFLG                  = 0
            ,NotNetFLG                  = 0
            ,EDIFLG                     = 0
            ,DesiredDeliveryDate        = DODD.DesiredDeliveryDate  
            ,ArrivePlanDate             = null
            ,TotalArrivalSu             = 0
            ,CommentOutStore            = null
            ,CommentInStore             = null
            ,FirstOrderNO               = DLOR.OrderNO
            ,FirstOrderRows             = DLOR.OrderRows
            ,CancelOrderNO              = null
            ,AnswerFLG                  = 0
            ,EDIOutputDatetime          = null
      --      ,LastArrivePlanNO           = null
      --      ,LastArriveDatetime         = null
      --      ,LastArriveNO               = null
            ,InsertOperator             = @p_Operator
            ,InsertDateTime             = @SYSDATETIME
            ,UpdateOperator             = @p_Operator
            ,UpdateDateTime             = @SYSDATETIME
            ,DeleteOperator             = null
            ,DeleteDateTime             = null
    FROM D_LastOrder DLOR
    INNER JOIN D_OrderDetails DODD
    ON DODD.OrderNO = DLOR.OrderNO
    AND DODD.OrderRows = DLOR.OrderRows
    WHERE DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)
    AND DLOR.OrderNO = @p_OrderNO

    --D_OrderçXêV  
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
            ,InsertOperator       
            ,InsertDateTime       
            ,UpdateOperator       
            ,UpdateDateTime       
            ,DeleteOperator       
            ,DeleteDateTime       
            )  
    SELECT   OrderNO                            = @OrderNO  
            ,OrderProcessNO                     = @OrderProcessNO  
            ,StoreCD                            = DODH.StoreCD  
            ,OrderDate                          = @SYSDATE 
            ,ReturnFLG                          = 1
            ,OrderDataKBN                       = 3
            ,OrderWayKBN                        = 3
            ,OrderCD                            = DODH.OrderCD  
            ,OrderPerson                        = DODH.OrderPerson  
            ,AliasKBN                           = 0
            ,DestinationKBN                     = 1
            ,DestinationName                    = null
            ,DestinationZip1CD                  = null
            ,DestinationZip2CD                  = null
            ,DestinationAddress1                = null
            ,DestinationAddress2                = null
            ,DestinationTelphoneNO              = null  
            ,DestinationFaxNO                   = null
            ,DestinationSoukoCD                 = null
            ,CurrencyCD                         = MCON.CurrencyCD  
            ,OrderHontaiGaku                    = SUB_DODD.SumOrderHontaiGaku
            ,OrderTax8                          = SUB_DODD.SumOrderTax8  
            ,OrderTax10                         = SUB_DODD.SumOrderTax10  
            ,OrderGaku                          = SUB_DODD.SumOrderGaku  
            ,CommentOutStore                    = null
            ,CommentInStore                     = null
            ,StaffCD                            = null
            ,FirstArriveDate                    = null
            ,LastArriveDate                     = null
            ,ApprovalDate                       = @SYSDATE
            ,LastApprovalDate                   = @SYSDATE
            ,LastApprovalStaffCD                = @p_Operator  
            ,ApprovalStageFLG                   = 10
            ,FirstPrintDate                     = @SYSDATE
            ,LastPrintDate                      = @SYSDATE
            ,InsertOperator                     = @p_Operator
            ,InsertDateTime                     = @SYSDATETIME 
            ,UpdateOperator                     = @p_Operator
            ,UpdateDateTime                     = @SYSDATETIME 
            ,DeleteOperator                     = null
            ,DeleteDateTime                     = null
    FROM D_LastOrder DLOR
    INNER JOIN D_Order DODH
    ON DODH.OrderNO = DLOR.OrderNO
    CROSS APPLY(SELECT SUM(OrderHontaiGaku) SumOrderHontaiGaku
                      ,SUM(OrderGaku) SumOrderGaku
                      ,SUM(CASE WHEN DODD.OrderTaxRitsu = 2 THEN OrderTax ELSE 0 END) SumOrderTax8
                      ,SUM(CASE WHEN DODD.OrderTaxRitsu = 1 THEN OrderTax ELSE 0 END) SumOrderTax10
                  FROM D_OrderDetails DODD
                 WHERE DODD.OrderNO = DLOR.OrderNO)SUB_DODD
    INNER JOIN M_Control MCON
    ON MCON.MainKey = 1
    WHERE DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)
    AND DLOR.OrderNO = @p_OrderNO
    
    UPDATE DODH
    SET  FirstPrintDate    = @SYSDATE
        ,LastPrintDate     = @SYSDATE
    FROM D_Order DODH
    INNER JOIN D_LastOrder DLOR
    ON DODH.OrderNO = DLOR.OrderNO
    WHERE (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)
    AND DLOR.OrderNO = @p_OrderNO

		---PTK Oride
	UPDATE DODH
    SET  FirstPrintDate    = @SYSDATE
        ,LastPrintDate     = @SYSDATE
    FROM D_Order DODH 
	WHERE DODH.OrderNO = @p_OrderNO and FirstPrintDate is null

    UPDATE DODD
    SET  CancelOrderNO     = @OrderNO
        ,UpdateOperator    = @p_Operator
        ,UpdateDateTime    = @SYSDATETIME
    FROM D_OrderDetails DODD
    INNER JOIN D_LastOrder DLOR
    ON DODD.OrderNO = DLOR.OrderNO
    AND DODD.OrderRows = DLOR.OrderRows
    WHERE DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)
    AND DLOR.OrderNO = @p_OrderNO

    UPDATE DLOR
    SET  CancelOrderDate   = @SYSDATE
        ,UpdateOperator    = @p_Operator
        ,UpdateDateTime    = @SYSDATETIME
    FROM D_LastOrder DLOR
    INNER JOIN D_OrderDetails DODD
    ON DLOR.OrderNO = DODD.OrderNO
    AND DLOR.OrderRows = DODD.OrderRows
    WHERE DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)
    AND DLOR.OrderNO = @p_OrderNO

END
GO

