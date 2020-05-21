
DROP TRIGGER Trg_DMitsumori_Insert
GO
DROP TRIGGER Trg_DMitsumori_Update
GO

DROP TRIGGER Trg_DMitsumoriDetails_Insert
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE TRIGGER Trg_DMitsumori_Insert
ON D_Mitsumori
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

        INSERT INTO [L_MitsumoriHistory]
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
     SELECT
            MitsumoriNO                      
           ,StoreCD                          
           ,MitsumoriDate                    
           ,StaffCD                          
           ,CustomerCD                       
           ,CustomerName                     
           ,CustomerName2                    
           ,AliasKBN                         
           ,ZipCD1                           
           ,ZipCD2                           
           ,Address1                         
           ,Address2                         
           ,Tel11                            
           ,Tel12                            
           ,Tel13                            
           ,Tel21                            
           ,Tel22                            
           ,Tel23                            
           ,JuchuuChanceKBN                  
           ,MitsumoriName                    
           ,DeliveryDate                     
           ,PaymentTerms                     
           ,DeliveryPlace                    
           ,ValidityPeriod                   
           ,MitsumoriHontaiGaku              
           ,MitsumoriTax8                    
           ,MitsumoriTax10                   
           ,MitsumoriGaku                    
           ,CostGaku                         
           ,ProfitGaku                       
           ,RemarksInStore                   
           ,RemarksOutStore                  
           ,PrintDateTime
           ,JuchuuFLG
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;  
END
GO

CREATE TRIGGER Trg_DMitsumori_Update
ON D_Mitsumori
AFTER UPDATE
AS
BEGIN
	
        INSERT INTO [L_MitsumoriHistory]
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
     SELECT
            MitsumoriNO                      
           ,StoreCD                          
           ,MitsumoriDate                    
           ,StaffCD                          
           ,CustomerCD                       
           ,CustomerName                     
           ,CustomerName2                    
           ,AliasKBN                         
           ,ZipCD1                           
           ,ZipCD2                           
           ,Address1                         
           ,Address2                         
           ,Tel11                            
           ,Tel12                            
           ,Tel13                            
           ,Tel21                            
           ,Tel22                            
           ,Tel23                            
           ,JuchuuChanceKBN                  
           ,MitsumoriName                    
           ,DeliveryDate                     
           ,PaymentTerms                     
           ,DeliveryPlace                    
           ,ValidityPeriod                   
           ,MitsumoriHontaiGaku              
           ,MitsumoriTax8                    
           ,MitsumoriTax10                   
           ,MitsumoriGaku                    
           ,CostGaku                         
           ,ProfitGaku                       
           ,RemarksInStore                   
           ,RemarksOutStore                  
           ,PrintDateTime
           ,JuchuuFLG
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
           ,DeleteOperator
           ,DeleteDateTime
           FROM    inserted;

END
GO

CREATE TRIGGER Trg_DMitsumoriDetails_Insert
ON D_MitsumoriDetails
AFTER INSERT
AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    INSERT INTO [L_MitsumoriDetailsHistory]
               ([HistorySEQ]
               ,[HistorySEQRows]
               ,[MitsumoriNO]
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
         SELECT (SELECT IDENT_CURRENT('L_MitsumoriHistory'))
         	   ,ROW_NUMBER() OVER(ORDER BY MitsumoriRows) 
         	   ,MitsumoriNO                         
               ,MitsumoriRows                       
               ,DisplayRows                      
               ,NotPrintFLG
               ,AdminNO                            
               ,SKUCD                            
               ,JanCD                            
               ,SKUName                          
               ,ColorName                        
               ,SizeName                         
               ,SetKBN                           
               ,MitsumoriSuu                     
               ,MitsumoriUnitPrice               
               ,TaniCD                           
               ,MitsumoriGaku                    
               ,MitsumoriHontaiGaku              
               ,MitsumoriTax                     
               ,MitsumoriTaxRitsu                
               ,CostUnitPrice                    
               ,CostGaku                         
               ,ProfitGaku                       
               ,CommentInStore                   
               ,CommentOutStore                  
               ,IndividualClientName             
	           ,InsertOperator
	           ,InsertDateTime
	           ,UpdateOperator
	           ,UpdateDateTime
           FROM    inserted;  
END
GO

