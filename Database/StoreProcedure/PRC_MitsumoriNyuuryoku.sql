 BEGIN TRY 
 Drop Procedure dbo.[PRC_MitsumoriNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PRC_MitsumoriNyuuryoku]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @MitsumoriNO   varchar(11),
    @StoreCD   varchar(4),
    @MitsumoriDate  varchar(10),
    @StaffCD   varchar(10),
    @CustomerCD   varchar(13),
    @CustomerName   varchar(80),
    @CustomerName2   varchar(20),
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
    @JuchuuChanceKBN   varchar(3),
    @MitsumoriName   varchar(100),
    @DeliveryDate   varchar(50),
    @PaymentTerms   varchar(50),
    @DeliveryPlace   varchar(50),
    @ValidityPeriod   varchar(50),
    @MitsumoriHontaiGaku   money,
    @MitsumoriTax8   money,
    @MitsumoriTax10   money,
    @MitsumoriGaku   money,
    @CostGaku   money,
    @ProfitGaku   money,
    @RemarksInStore   varchar(500),
    @RemarksOutStore   varchar(500),

    @Table  T_Mitsumori READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutMitsumoriNo varchar(11) OUTPUT
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
        
        --伝票番号採番
        EXEC Fnc_GetNumber
            11,             --in伝票種別 11
            @MitsumoriDate, --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @MitsumoriNO OUTPUT
            ;
        
        IF ISNULL(@MitsumoriNO,'') = ''
        BEGIN
        	SET @W_ERR = 1;
        	RETURN @W_ERR;
        END
        
        INSERT INTO [D_Mitsumori]
           ([MitsumoriNO]
           ,[StoreCD]
           ,[MitsumoriDate]
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
           ,[JuchuuChanceKBN]
           ,[MitsumoriName]
           ,[DeliveryDate]
           ,[PaymentTerms]
           ,[DeliveryPlace]
           ,[ValidityPeriod]
           ,[MitsumoriHontaiGaku]
           ,[MitsumoriTax8]
           ,[MitsumoriTax10]
           ,[MitsumoriGaku]
           ,[CostGaku]
           ,[ProfitGaku]
           ,[RemarksInStore]
           ,[RemarksOutStore]
           ,[PrintDateTime]
           ,[JuchuuFLG]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@MitsumoriNO                      
           ,@StoreCD                          
           ,convert(date,@MitsumoriDate)                    
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
           ,@Tel21                            
           ,@Tel22                            
           ,@Tel23                            
           ,@JuchuuChanceKBN                  
           ,@MitsumoriName                    
           ,@DeliveryDate                     
           ,@PaymentTerms                     
           ,@DeliveryPlace                    
           ,@ValidityPeriod                   
           ,@MitsumoriHontaiGaku              
           ,@MitsumoriTax8                    
           ,@MitsumoriTax10                   
           ,@MitsumoriGaku                    
           ,@CostGaku                         
           ,@ProfitGaku                       
           ,@RemarksInStore                   
           ,@RemarksOutStore                  
           ,NULL
           ,0
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

        UPDATE D_Mitsumori
           SET [StoreCD] = @StoreCD                         
              ,[MitsumoriDate] = @MitsumoriDate
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
              ,[JuchuuChanceKBN] = @JuchuuChanceKBN         
              ,[MitsumoriName] = @MitsumoriName             
              ,[DeliveryDate] = @DeliveryDate               
              ,[PaymentTerms] = @PaymentTerms               
              ,[DeliveryPlace] = @DeliveryPlace             
              ,[ValidityPeriod] = @ValidityPeriod           
              ,[MitsumoriHontaiGaku] = @MitsumoriHontaiGaku
              ,[MitsumoriTax8] = @MitsumoriTax8
              ,[MitsumoriTax10] = @MitsumoriTax10
              ,[MitsumoriGaku] = @MitsumoriGaku
              ,[CostGaku] = @CostGaku
              ,[ProfitGaku] = @ProfitGaku
              ,[RemarksInStore] = @RemarksInStore            
              ,[RemarksOutStore] = @RemarksOutStore          
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE MitsumoriNO = @MitsumoriNO
           ;

		DELETE FROM D_MitsumoriDetails
		WHERE MitsumoriNO = @MitsumoriNO
           ;
    END
    
    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        UPDATE [D_Mitsumori]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [MitsumoriNO] = @MitsumoriNO
         ;
         

    END
    
    IF @OperateMode <= 2	--新規・修正時
    BEGIN
        INSERT INTO [D_MitsumoriDetails]
                   ([MitsumoriNO]
                   ,[MitsumoriRows]
                   ,[DisplayRows]
                   ,[NotPrintFLG]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[JanCD]
                   ,[SKUName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[SetKBN]
                   ,[MitsumoriSuu]
                   ,[MitsumoriUnitPrice]
                   ,[TaniCD]
                   ,[MitsumoriGaku]
                   ,[MitsumoriHontaiGaku]
                   ,[MitsumoriTax]
                   ,[MitsumoriTaxRitsu]
                   ,[CostUnitPrice]
                   ,[CostGaku]
                   ,[ProfitGaku]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[IndividualClientName]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @MitsumoriNO                         
                   ,tbl.MitsumoriRows                       
                   ,tbl.DisplayRows                      
                   ,tbl.NotPrintFLG
                   ,tbl.SKUNO                            
                   ,tbl.SKUCD                            
                   ,tbl.JanCD                            
                   ,tbl.SKUName                          
                   ,tbl.ColorName                        
                   ,tbl.SizeName                         
                   ,tbl.SetKBN                           
                   ,tbl.MitsumoriSuu                     
                   ,tbl.MitsumoriUnitPrice               
                   ,tbl.TaniCD                           
                   ,tbl.MitsumoriGaku                    
                   ,tbl.MitsumoriHontaiGaku              
                   ,tbl.MitsumoriTax                     
                   ,tbl.MitsumoriTaxRitsu                
                   ,tbl.CostUnitPrice                    
                   ,tbl.CostGaku                         
                   ,tbl.ProfitGaku                       
                   ,tbl.CommentInStore                   
                   ,tbl.CommentOutStore                  
                   ,tbl.IndividualClientName             
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              ;
    END

    --処理履歴データへ更新
    SET @KeyItem = @MitsumoriNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'MitsumoriNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

	SET @OutMitsumoriNo = @MitsumoriNO;
	
--<<OWARI>>
  return @W_ERR;

END


