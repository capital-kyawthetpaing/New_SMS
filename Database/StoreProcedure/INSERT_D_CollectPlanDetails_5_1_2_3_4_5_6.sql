 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INSERT_D_CollectPlanDetails_5_1_2_3_4_5_6]
(
	@KBN tinyint,	--1:掛,2:現金,3:カード,4:その他①,5:その他,6:前受金
    @DataNo int,
    @DataRows int,
    @Datatype tinyint,		--0:全額,1:一部
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @FirstCollectPlanDate date,
    @CollectPlanNO int,
    @CollectPlanRows int OUTPUT,
    @BillingNO varchar(11),
    @CollectNO varchar(11),
    @ConfirmNO varchar(11) OUTPUT,
    @W_ERR tinyint OUTPUT
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
	
    DECLARE @D_CollectPlanDetailsInserted TABLE (
         CollectPlanRows int
         );

    DECLARE @SYSDATE_VAR varchar(10);
    DECLARE @StoreCD varchar(4);
    
    SET @W_ERR = 0;
    SET @SYSDATE_VAR = CONVERT(varchar,@SYSDATETIME,111);
    
    --テーブル転送仕様５－①－a 回収予定明細(全額掛)
    --テーブル転送仕様５－①－b 回収予定明細(一部掛)
    --テーブル転送仕様５－②－a 回収予定明細(全額現金)
    --テーブル転送仕様５－②－b 回収予定明細(一部現金)
    --テーブル転送仕様５－③－a 回収予定明細(全額カード等)
    --テーブル転送仕様５－③－b 回収予定明細(一部カード等)
    --テーブル転送仕様５－④－a 回収予定明細(全額その他①)
    --テーブル転送仕様５－④－b 回収予定明細(一部その他①)
    --テーブル転送仕様５－⑤－a 回収予定明細(全額その他②)
    --テーブル転送仕様５－⑤－b 回収予定明細(一部その他②)
    --テーブル転送仕様５－⑥－a 回収予定明細(全額前受金)
    --テーブル転送仕様５－⑥－b 回収予定明細(一部前受金)
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
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime]
       ,[DeleteOperator]
       ,[DeleteDateTime])
    OUTPUT INSERTED.CollectPlanRows INTO @D_CollectPlanDetailsInserted(CollectPlanRows)
    SELECT
        @CollectPlanNO
       ,(CASE @Datatype WHEN 0 THEN DM.SalesRows ELSE 1 END)  --CollectPlanRows
       ,DM.SalesNO
       ,DM.SalesRows
       ,DM.JuchuuNO
       ,DM.JuchuuRows
       ,2   --JuchuuKBN ２（店舗）
       ,(CASE @Datatype WHEN 0 THEN DM.SalesHontaiGaku ELSE 0 END)--HontaiGaku
       ,(CASE @Datatype WHEN 0 THEN DM.SalesTax ELSE 0 END)    --Tax
       ,(CASE @Datatype WHEN 0 THEN DM.SalesGaku ELSE 
            (CASE @KBN WHEN 3 THEN DH.CardAmount
                WHEN 4 THEN DH.Denomination1Amount
                WHEN 5 THEN DH.Denomination2Amount
                WHEN 6 THEN DH.AdvanceAmount
                ELSE DH.CreditAmount END) END)   --CollectPlanGaku
       ,DM.SalesTaxRitsu   --TaxRitsu
       ,@FirstCollectPlanDate		--FirstCollectPlanDate
       ,0   --PaymentProgressKBN
       ,0   --BillingPrintFLG
       ,@Operator
       ,@SYSDATETIME
       ,@Operator
       ,@SYSDATETIME
       ,NULL --DeleteOperator
       ,NULL --DeleteDateTime
   FROM D_SalesDetailsTran AS DM
   INNER JOIN D_SalesTran AS DH
   ON DH.DataNo = DM.DataNo
    WHERE DM.DataNo = @DataNo
    AND DM.DataRows = @DataRows
    AND DM.DeleteDateTime IS NULL
	;
    
    SET @CollectPlanRows = (SELECT A.CollectPlanRows FROM @D_CollectPlanDetailsInserted AS A);
    
    IF @KBN IN (2,3,4,5,6)     --現金,カード,その他①,その他②,6:前受金
    BEGIN
        --テーブル転送仕様７－①Insert請求明細①　D_BillingDetails
        --請求明細は回収予定明細のレコードごとに１レコード
        INSERT INTO [D_BillingDetails]
           ([BillingNO]
           ,[BillingType]   
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
           ,1   --BillingType  1:即
           ,DM.CollectPlanRows AS BillingRows
           ,DC.StoreCD
           ,CONVERT(date,@SYSDATETIME)	--★DC.SalesDate AS BillingCloseDate
           ,DC.CustomerCD
           ,DM.SalesNO
           ,DM.SalesRows 
           ,DM.CollectPlanNO 
           ,DM.CollectPlanRows 
           ,DM.HontaiGaku 
           ,DM.Tax 
           ,DM.CollectPlanGaku   --CollectPlanGaku 
           ,DM.TaxRitsu 
           ,0   --InvoiceFLG 
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           
        FROM D_CollectPlanDetails AS DM
        LEFT OUTER JOIN D_CollectPlan AS DC
        ON DC.CollectPlanNO = DM.CollectPlanNO
        AND DC.DeleteDateTime IS Null

        WHERE DM.CollectPlanNO = @CollectPlanNO       
        AND DM.CollectPlanRows = @CollectPlanRows
        AND DM.DeleteDateTime IS Null               
        ;

		IF @KBN IN (2,6)	--現金,6:前受金
		BEGIN
			SET @StoreCD = (SELECT StoreCD FROM D_CollectPlan
							WHERE CollectPlanNO = @CollectPlanNO);
							
            --テーブル転送仕様９－①Insert入金消込①D_PaymentConfirm
            --伝票番号採番
            EXEC Fnc_GetNumber
                8,             --in伝票種別 8
                @SYSDATE_VAR, --D_CollectPlan.BillingDate in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @ConfirmNO OUTPUT
                ;

            IF ISNULL(@ConfirmNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --店頭で入金なので今作る、回収予定明細ごと
            --D_PaymentConfirm      
            INSERT INTO [D_PaymentConfirm]
               ([ConfirmNO]
               ,[CollectNO]
               ,[CollectClearDate]
               ,[StaffCD]
               ,[ConfirmDateTime]
               ,[ConfirmAmount]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @ConfirmNO
               ,@CollectNO
               ,CONVERT(date,@SYSDATETIME)   --CollectClearDate, date,>
               ,@Operator
               ,@SYSDATETIME    --ConfirmDateTime, datetime,>
               ,DM.CollectPlanGaku   --ConfirmAmount
               ,@Operator   --InsertOperator, varchar(10),>
               ,@SYSDATETIME    --InsertDateTime, datetime,>
               ,@Operator   --UpdateOperator, varchar(10),>
               ,@SYSDATETIME    --UpdateDateTime, datetime,>
               ,NULL    --DeleteOperator, varchar(10),>
               ,NULL    --DeleteDateTime, datetime,>
            FROM D_CollectPlanDetails AS DM
            WHERE DM.CollectPlanNO = @CollectPlanNO       
            AND DM.CollectPlanRows = @CollectPlanRows
            AND DM.DeleteDateTime IS Null               
            ;
        END
	END
END


