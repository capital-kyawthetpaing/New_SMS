 BEGIN TRY 
 Drop Procedure dbo.[PRC_MasterTouroku_HanbaiTanka_SKU]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_MasterTouroku_HanbaiTanka_SKU]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @StoreCD  varchar(4),
    @TankaCD  varchar(13),
    @GeneralRate decimal(5,2), 
    @MemberRate  decimal(5,2), 
    @ClientRate  decimal(5,2), 
    @SaleRate    decimal(5,2), 
    @WebRate     decimal(5,2), 
    @Table  T_ItemTanka READONLY,
    @DeleteFlg tinyint,
    @UsedFlg tinyint,
    @Operator  varchar(10),
    @PC  varchar(30)
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
    
    --新規/修正--
    IF @OperateMode <= 2
    BEGIN
        SET @OperateModeNm = '新規';
        
        INSERT INTO [M_SKUPrice]
                   ([TankaCD]
                   ,[StoreCD]
                   ,[AdminNO]	--2009.10.16 add
                   ,[SKUCD]
                   ,[ChangeDate]
                   ,[PriceWithTax]
                   ,[PriceWithoutTax]
                   ,[GeneralRate]
                   ,[GeneralPriceWithTax]
                   ,[GeneralPriceOutTax]
                   ,[MemberRate]
                   ,[MemberPriceWithTax]
                   ,[MemberPriceOutTax]
                   ,[ClientRate]
                   ,[ClientPriceWithTax]
                   ,[ClientPriceOutTax]
                   ,[SaleRate]
                   ,[SalePriceWithTax]
                   ,[SalePriceOutTax]
                   ,[WebRate]
                   ,[WebPriceWithTax]
                   ,[WebPriceOutTax]
                   ,[Remarks]
                   ,[DeleteFlg]
                   ,[UsedFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT
                    @TankaCD
                    ,@StoreCD
                    ,tbl.AdminNO	--2009.10.16 add
                    ,tbl.ITemCD
                    ,tbl.ChangeDate
                    ,tbl.PriceWithTax
                    ,tbl.PriceWithoutTax
                    ,@GeneralRate
                    ,tbl.GeneralPriceWithTax
                    ,tbl.GeneralPriceOutTax
                    ,@MemberRate
                    ,tbl.MemberPriceWithTax
                    ,tbl.MemberPriceOutTax
                    ,@ClientRate
                    ,tbl.ClientPriceWithTax
                    ,tbl.ClientPriceOutTax
                    ,@SaleRate
                    ,tbl.SalePriceWithTax
                    ,tbl.SalePriceOutTax
                    ,@WebRate
                    ,tbl.WebPriceWithTax
                    ,tbl.WebPriceOutTax
                    ,tbl.Remarks
                   ,@DeleteFlg
                   ,@UsedFlg
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                  FROM @Table tbl
                  WHERE tbl.UpdateFlg = 0
                    ;
                    
        UPDATE [M_SKUPrice]
           SET
                [PriceWithTax]       =  tbl.PriceWithTax
               ,[PriceWithoutTax]    =  tbl.PriceWithoutTax
               ,[GeneralRate]        =  @GeneralRate
               ,[GeneralPriceWithTax]=  tbl.GeneralPriceWithTax
               ,[GeneralPriceOutTax] =  tbl.GeneralPriceOutTax
               ,[MemberRate]         =  @MemberRate
               ,[MemberPriceWithTax] =  tbl.MemberPriceWithTax
               ,[MemberPriceOutTax]  =  tbl.MemberPriceOutTax
               ,[ClientRate]         =  @ClientRate
               ,[ClientPriceWithTax] =  tbl.ClientPriceWithTax
               ,[ClientPriceOutTax]  =  tbl.ClientPriceOutTax
               ,[SaleRate]           =  @SaleRate
               ,[SalePriceWithTax]   =  tbl.SalePriceWithTax
               ,[SalePriceOutTax]    =  tbl.SalePriceOutTax
               ,[WebRate]            =  @WebRate
               ,[WebPriceWithTax]    =  tbl.WebPriceWithTax
               ,[WebPriceOutTax]     =  tbl.WebPriceOutTax
               ,[Remarks]            =  tbl.Remarks
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
         FROM M_SKUPrice
         INNER JOIN @Table tbl 
         ON --tbl.ITemCD = M_SKUPrice.SKUCD
         	tbl.AdminNO = M_SKUPrice.AdminNO
         AND tbl.ChangeDate = M_SKUPrice.ChangeDate
         AND tbl.UpdateFlg = 1
         WHERE TankaCD = @TankaCD
         AND StoreCD = @StoreCD
         ;

    END

    ELSE IF @OperateMode = 3 --削除--
    BEGIN
        SET @OperateModeNm = '削除';
        
        DELETE FROM [M_SKUPrice]
         WHERE EXISTS(SELECT 1 FROM @Table tbl
            WHERE --tbl.ITemCD = M_SKUPrice.SKUCD
         	tbl.AdminNO = M_SKUPrice.AdminNO
            AND tbl.ChangeDate = M_SKUPrice.ChangeDate)
            AND TankaCD = @TankaCD
            AND StoreCD = @StoreCD
         ;

    END
        
    --カーソルの値を取得する変数宣言
    DECLARE @W_COL1 varchar(30);
    DECLARE @W_COL2 varchar(10);
    DECLARE @W_COL3 tinyint;
    DECLARE @W_COL4 int;	--2019.10.16 add

    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.ITemCD,tbl.ChangeDate,tbl.UpdateFlg,tbl.AdminNO
        FROM   @Table tbl
        ORDER BY tbl.ITemCD,tbl.ChangeDate

    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO @W_COL1,@W_COL2,@W_COL3,@W_COL4;

    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN

        -- ========= ループ内の実際の処理 ここから===

        --処理履歴データへ更新
        --SET @KeyItem = @StoreCD + ' ' + @TankaCD + ' ' + @W_COL1 + ' ' + @W_COL2;
        SET @KeyItem = @StoreCD + ' ' + @TankaCD + ' ' + CONVERT(varchar,@W_COL4) + ' ' + @W_COL2;
        
        IF @OperateMode <> 3 --削除--
            IF @W_COL3 = 0
                SET @OperateModeNm = '新規';
            ELSE
                SET @OperateModeNm = '修正';
                
            EXEC L_Log_Insert_SP
                @SYSDATETIME,
                @Operator,
                'MasterTouroku_HanbaiTanka',
                @PC,
                @OperateModeNm,
                @KeyItem;
        -- ========= ループ内の実際の処理 ここまで===


        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @W_COL1,@W_COL2,@W_COL3,@W_COL4;

    END

    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
 

--<<OWARI>>
  return @W_ERR;

END


