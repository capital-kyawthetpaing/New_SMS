 BEGIN TRY 
 Drop Procedure dbo.[INSERT_D_DepositHistory_Discount]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_D_DepositHistory_Discount]
(
    @SalesNO varchar(11),
    @SIGN int,
    @Operator  varchar(10),
    @SYSDATETIME  datetime,
    @Keijobi  varchar(10),
    @Program varchar(50),
    @StoreCD varchar(4),
    @CancelKBN tinyint,
    @RecoredKBN tinyint,
    @Discount money
)AS
--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

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
 	VALUES(
       @StoreCD
       ,@SYSDATETIME  --DepositDateTime
       ,3   --DataKBN 1:販売情報、2:販売明細情報、3:入出金情報
       ,1   --DepositKBN 1;販売,2:入金,3;支払,4:両替入,5:両替出,6:釣銭準備,7:商品券釣
       ,@CancelKBN   --CancelKBN 1:取消、2:返品、3:訂正
       ,@RecoredKBN   --RecoredKBN
       ,(SELECT M.DenominationCD FROM M_DenominationKBN AS M
       	WHERE M.SystemKBN = 8 AND M.MainFLG = 1)   --DenominationCD
       ,@SIGN*@Discount   --DepositGaku
       ,NULL    --Remark
       ,(CASE @CancelKBN WHEN 3 THEN convert(date,@Keijobi) 
                   WHEN 1 THEN convert(date,@Keijobi) 
                   ELSE convert(date,@SYSDATETIME) END)  --AccountingDate, date,>
       ,@SalesNO    --Number, varchar(11),>
       ,0   --Rows, tinyint,>
       ,0   --ExchangeMoney, money,>
       ,0   --ExchangeDenomination, int,>
       ,0   --ExchangeCount, int,>
       ,NULL  --AdminNO, int,>
       ,NULL  --SKUCD, varchar(30),>
       ,NULL  --JanCD, varchar(13),>
       ,0   --SalesSU, money,>
       ,0   --SalesUnitPrice, money,>
       ,0   --SalesGaku, money,>
       ,0   --SalesTax, money,>
       ,0   --SalesTaxRate, int,>
       ,0   --TotalGaku, money,>
       ,0   --Refund, money,>
       ,0   --IsIssued, tinyint,>
       ,@Program    --Program, varchar(100),>
       ,@Operator
       ,@SYSDATETIME
       ,@Operator
       ,@SYSDATETIME
       )
   ;

END



