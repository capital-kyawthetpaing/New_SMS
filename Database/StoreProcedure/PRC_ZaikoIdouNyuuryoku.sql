

/****** Object:  StoredProcedure [dbo].[PRC_ZaikoIdouNyuuryoku]    Script Date: 2020/10/01 19:37:40 ******/
DROP PROCEDURE [dbo].[PRC_ZaikoIdouNyuuryoku]
GO

/****** Object:  StoredProcedure [dbo].[PRC_ZaikoIdouNyuuryoku]    Script Date: 2020/10/01 19:37:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--CREATE TYPE T_Ido AS TABLE
--    (
--    [MoveRows] [int],

--    [SKUCD] [varchar](30) ,
--    [AdminNO] [int] ,
--    [JanCD] [varchar](13) ,
--    [MoveSu] [int] ,
--    [OldMoveSu] [int] ,
--    [EvaluationPrice] [money] ,
--    [FromRackNO]  [varchar](11) ,
--    [ToRackNO]  [varchar](11) ,
--    [NewSKUCD] [varchar](30) ,
--    [NewAdminNO] [int] ,
--    [NewJanCD] [varchar](13) ,
--    [DeliveryPlanNO]  [varchar](11) ,
--    [ExpectReturnDate] date,
--    [VendorCD] varchar(13),
--    [CommentInStore] varchar(80),
--    [RequestRows] int,
--    [AnswerKBN] tinyint,

----    [StockNO] [varchar](11) ,
--    [ArrivalPlanNO] [varchar](11) ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_ZaikoIdouNyuuryoku]
   (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @MoveNO   varchar(11),
    @StoreCD   varchar(4),
    @RequestNO   varchar(11),
    @MovePurposeKBN tinyint,
    @MovePurposeType tinyint,
    @MoveDate  varchar(10),
    @FromStoreCD varchar(4),
    @FromSoukoCD varchar(6),
    @ToStoreCD varchar(4),
    @ToSoukoCD varchar(6),
    @StaffCD   varchar(10),

    @Table  T_Ido READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutMoveNO varchar(11) OUTPUT
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
    DECLARE @Program varchar(100); 
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @Program = 'ZaikoIdouNyuuryoku';    
    
    DECLARE @KBN_TENPOKAN   tinyint;
    DECLARE @KBN_TENPONAI   tinyint;
    DECLARE @KBN_SYOCD      tinyint;
    DECLARE @KBN_CHOSEI_ADD tinyint;
    DECLARE @KBN_CHOSEI_DEL tinyint;
    DECLARE @KBN_LOCATION   tinyint;  
    DECLARE @KBN_HENPIN     tinyint;

    SET @KBN_TENPOKAN   = 1;
    SET @KBN_TENPONAI   = 2;
    SET @KBN_SYOCD      = 3;
    SET @KBN_CHOSEI_ADD = 4;
    SET @KBN_CHOSEI_DEL = 5;
    SET @KBN_LOCATION   = 6;  
    SET @KBN_HENPIN     = 7;
    
    DECLARE @ReserveNO       varchar(11);
    DECLARE @StockNO         varchar(11);
    DECLARE @DeliveryPlanNO  varchar(11);
    DECLARE @ArrivalPlanNO   varchar(11);
    DECLARE @NewMoveNO       varchar(11);
    
    --カーソル定義
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.MoveRows, tbl.FromRackNO, tbl.AdminNO, tbl.MoveSu, tbl.UpdateFlg
        FROM @Table AS tbl
        ORDER BY tbl.MoveRows
        ;
    DECLARE @tblMoveRows int;
    DECLARE @tblFromRackNO varchar(11);
    DECLARE @tblAdminNO int;
    DECLARE @tblMoveSu int;
    DECLARE @tblUpdateFlg int;
        
    --移動依頼あり*******************************************************************************
    IF ISNULL(@RequestNO,' ') <>' '
    BEGIN        
        --Form.Detail Display  Area. AnswerKBNが0の行のみ更新する。
		--1または9の回答済の行は更新しない。
        --【D_MoveRequestDetailes】移動依頼明細更新　テーブル転送仕様F①
        UPDATE [D_MoveRequestDetailes] SET
             [AnswerKBN]     = tbl.AnswerKBN		--1:受諾、9:非受諾
            ,[UpdateOperator]     =  @Operator  
            ,[UpdateDateTime]     =  @SYSDATETIME
        FROM @Table tbl
        WHERE tbl.RequestRows = D_MoveRequestDetailes.RequestRows
        AND D_MoveRequestDetailes.RequestNO = @RequestNO
        AND D_MoveRequestDetailes.AnswerKBN = 0
        ;
        
        --【D_MoveRequest】移動依頼更新　テーブル転送仕様E①
        UPDATE [D_MoveRequest] SET
             [AnswerDateTime] = (CASE (SELECT SUM(DM.AnswerKBN) FROM D_MoveRequestDetailes AS DM
                                       WHERE DM.RequestNO = @RequestNO
                                       AND DM.DeleteDateTime IS NULL)
                                       WHEN 0 THEN NULL
                                       ELSE convert(datetime,@MoveDate) END)
            ,[AnswerStaffCD] = (CASE (SELECT SUM(DM.AnswerKBN) FROM D_MoveRequestDetailes AS DM
                                      WHERE DM.RequestNO = @RequestNO
                                      AND DM.DeleteDateTime IS NULL)
                                      WHEN 0 THEN NULL
                                      ELSE @StaffCD END)
            ,[UpdateOperator]     =  @Operator  
            ,[UpdateDateTime]     =  @SYSDATETIME  
        WHERE RequestNO = @RequestNO
        ;
    END
        
    --変更・削除--*******************************************************************************変更
    IF @OperateMode >= 2
    BEGIN
    	IF @OperateMode = 2
    	BEGIN
        	SET @OperateModeNm = '変更';
        END
        
        --移動区分=店舗間移動
        IF @MovePurposeType = @KBN_TENPOKAN
        BEGIN
        
            --【D_Stock】在庫　移動元　テーブル転送仕様G②
            --移動数を在庫で引当→赤データ作成
            --カーソル定義(D_Stock_SelectSuryo参照)
            DECLARE CUR_Stock_G2 CURSOR FOR
                SELECT DS.StockNO
                      ,tbl.OldMoveSu
                 FROM D_Stock AS DS
                 INNER JOIN D_Warehousing AS DW
                 ON DW.WarehousingKBN = 90   --WarehousingKBN
                 AND DW.DeleteFlg = 0
                 AND DW.DeleteDateTime IS NULL
                 AND DW.[Number] = @MoveNO
                 AND DW.StockNO = DS.StockNO
                  
                  INNER JOIN @Table tbl 
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	行削除データも赤データは作成しなければならない
                 WHERE DS.DeleteDateTime IS NULL
                ;
            DECLARE @OldMoveSu int;
            
            OPEN CUR_Stock_G2;

            --最初の1行目を取得して変数へ値をセット
            FETCH NEXT FROM CUR_Stock_G2
            INTO @StockNO, @OldMoveSu;
            
            --データの行数分ループ処理を実行する
            WHILE @@FETCH_STATUS = 0
            BEGIN
                --【D_Stock】在庫　移動元　テーブル転送仕様Ｇ②
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] + @OldMoveSu
                      ,[AllowableSu] = [AllowableSu] + @OldMoveSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + @OldMoveSu
                      --,[ReturnSu]           = [ReturnSu] + @OldMoveSu		2020.09.18 del
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                 WHERE DeleteDateTime IS NULL
                 AND StockNO = @StockNO
                  ;
                
                --次の行のデータを取得して変数へ値をセット
                FETCH NEXT FROM CUR_Stock_G2
                INTO @StockNO, @OldMoveSu;
            END		--LOOPの終わり***************************************CUR_Stock
            
            --カーソルを閉じる
            CLOSE CUR_Stock_G2;
            DEALLOCATE CUR_Stock_G2;
	        
            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ②赤　（Ｃに対する赤データ　WarehousingKBN＝11）            
            --C②(90)
            INSERT INTO [D_Warehousing]
               ([WarehousingDate]
               ,[SoukoCD]
               ,[RackNO]
               ,[StockNO]
               ,[JanCD]
               ,[AdminNO]
               ,[SKUCD]
               ,[WarehousingKBN]
               ,[DeleteFlg]
               ,[Number]
               ,[NumberRow]
               ,[VendorCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[ToRackNO]
               ,[ToStockNO]
               ,[FromStoreCD]
               ,[FromSoukoCD]
               ,[FromRackNO]
               ,[CustomerCD]
               ,[Quantity]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT DW.WarehousingDate
               ,DW.SoukoCD
               ,DW.RackNO
               ,DW.StockNO          --(D_Stock)☆(移動元)と同じ値
               ,tbl.JanCD
               ,tbl.AdminNO
               ,tbl.SKUCD
               ,90   --WarehousingKBN
               ,1    --DeleteFlg
               ,@MoveNO  --Number★
               ,tbl.MoveRows --NumberRow■
               ,NULL    --VendorCD
               ,DW.ToStoreCD
               ,DW.ToSoukoCD
               ,DW.ToRackNO
               ,DW.ToStockNO
               ,DW.FromStoreCD
               ,DW.FromSoukoCD
               ,DW.FromRackNO
               ,NULL    --CustomerCD
               ,DW.Quantity * (-1)
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              INNER JOIN D_Warehousing AS DW
              ON DW.WarehousingKBN = 90
              AND DW.DeleteFlg = 0
              AND DW.DeleteDateTime IS NULL
              AND DW.[Number] = @MoveNO
              AND DW.NumberRow = tbl.MoveRows
              ;
            
            --赤データの元の黒データのDeleteFlgを1にUPDATE
            UPDATE D_Warehousing SET
                DeleteFlg = 1
                ,UpdateOperator = @Operator
                ,UpdateDateTime = @SYSDATETIME
            FROM @Table tbl
            WHERE WarehousingKBN = 90
              AND DeleteFlg = 0
              AND DeleteDateTime IS NULL
              AND [Number] = @MoveNO
              AND NumberRow = tbl.MoveRows
              ;            
            
            IF @OperateMode = 2
            BEGIN
                --【D_Move】在庫移動　テーブル転送仕様A①
                UPDATE [D_Move]
                   SET [StoreCD] = @StoreCD                         
                      ,[MoveDate] = convert(date,@MoveDate)
                      ,[RequestNO] = @RequestNO
                      ,[MovePurposeKBN] = @MovePurposeKBN
                      ,[FromSoukoCD] = @FromSoukoCD
                      ,[ToStoreCD] = @ToStoreCD
                      ,[ToSoukoCD] = @ToSoukoCD
                      ,[StaffCD]         = @StaffCD  
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                 WHERE MoveNO = @MoveNO
                   ;

                --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ①
                UPDATE [D_MoveDetails]
                   SET  [SKUCD]          = tbl.SKUCD
                       ,[AdminNO]        = tbl.AdminNO
                       ,[JanCD]          = tbl.JanCD
                       ,[MoveSu]         = tbl.MoveSu
                       ,[EvaluationPrice] = tbl.EvaluationPrice
                       ,[FromRackNO]      = tbl.FromRackNO
                       ,[ToRackNO]        = tbl.ToRackNO
                       ,[NewSKUCD]        = tbl.NewSKUCD
                       ,[NewAdminNO]      = tbl.NewAdminNO
                       ,[NewJanCD]        = tbl.NewJanCD
                       ,[DeliveryPlanNO]  = tbl.DeliveryPlanNO
                       ,[ExpectReturnDate] = tbl.ExpectReturnDate
                       ,[VendorCD]         = tbl.VendorCD
                       ,[CommentInStore]   = tbl.CommentInStore
                       ,[RequestRows]      = tbl.RequestRows
                       ,[TotalArrivalSu]   = 0       
                       ,[UpdateOperator]   =  @Operator  
                       ,[UpdateDateTime]   =  @SYSDATETIME
                FROM D_MoveDetails
                INNER JOIN @Table tbl
                 ON @MoveNO = D_MoveDetails.MoveNO
                 AND tbl.MoveRows = D_MoveDetails.MoveRows
                 AND tbl.UpdateFlg = 1
                 ;
                
                --行追加データ
                --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ
                INSERT INTO [D_MoveDetails]
                           ([MoveNO]
                           ,[MoveRows]
                           ,[SKUCD]
                           ,[AdminNO]
                           ,[JanCD]
                           ,[MoveSu]
                           ,[EvaluationPrice]
                           ,[FromRackNO]
                           ,[ToRackNO]
                           ,[NewJanCD]
                           ,[NewAdminNO]
                           ,[NewSKUCD]
                           ,[DeliveryPlanNO]
                           ,[ExpectReturnDate]
                           ,[VendorCD]
                           ,[CommentInStore]
                           ,[RequestRows]
                           ,[TotalArrivalSu]

                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime])
                     SELECT @MoveNO                         
                           ,tbl.MoveRows                       
                           ,tbl.SKUCD
                           ,tbl.AdminNO
                           ,tbl.JanCD
                           ,tbl.MoveSu
                           ,tbl.EvaluationPrice
                           ,tbl.FromRackNO
                           ,tbl.ToRackNO
                           ,tbl.NewJanCD
                           ,tbl.NewAdminNO
                           ,tbl.NewSKUCD
                           ,@DeliveryPlanNO     --★要確認
                           ,tbl.ExpectReturnDate
                           ,tbl.VendorCD
                           ,tbl.CommentInStore
                           ,tbl.RequestRows
                           ,0 AS TotalArrivalSu
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME

                      FROM @Table tbl
                      WHERE tbl.UpdateFlg = 0
                      ;
                
                --行削除データ
                --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ②
                UPDATE [D_MoveDetails]
                    SET [DeleteOperator]     =  @Operator  
                       ,[DeleteDateTime]     =  @SYSDATETIME
                 WHERE [MoveNO] = @MoveNO
                 AND EXISTS(SELECT tbl.MoveRows FROM @Table tbl
                            WHERE tbl.MoveRows = D_MoveDetails.MoveRows
                            AND tbl.UpdateFlg = 2)
                 AND [DeleteDateTime] IS NULL
                 ;
            END
        END
        ELSE IF @OperateMode = 2	--移動区分<>店舗間移動の修正時
        BEGIN
            EXEC PRC_ZaikoIdou_A2B2
                @MoveNO,
                @Operator,
                @SYSDATETIME
                ;
            
            --伝票番号採番
            EXEC Fnc_GetNumber
                18,        --in伝票種別 18
                @MoveDate, --in基準日
                @StoreCD,  --in店舗CD
                @Operator,
                @NewMoveNO OUTPUT
                ;
            
            IF ISNULL(@NewMoveNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --【D_Move】在庫移動　Table転送仕様Ａ
            INSERT INTO [D_Move]
               ([MoveNO]
               ,[StoreCD]
               ,[MoveDate]
               ,[RequestNO]
               ,[MovePurposeKBN]
               ,[FromSoukoCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[MoveInputDateTime]
               ,[StaffCD]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            VALUES
               (@NewMoveNO
               ,@StoreCD
               ,convert(date,@MoveDate)
               ,@RequestNO
               ,@MovePurposeKBN
               ,@FromSoukoCD
               ,@ToStoreCD
               ,@ToSoukoCD
               ,SYSDATETIME()   --MoveInputDateTime
               ,@StaffCD

               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
               );               

            --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ
            INSERT INTO [D_MoveDetails]
                       ([MoveNO]
                       ,[MoveRows]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[MoveSu]
                       ,[EvaluationPrice]
                       ,[FromRackNO]
                       ,[ToRackNO]
                       ,[NewJanCD]
                       ,[NewAdminNO]
                       ,[NewSKUCD]
                       ,[DeliveryPlanNO]
                       ,[ExpectReturnDate]
                       ,[VendorCD]
                       ,[CommentInStore]
                       ,[RequestRows]
                       ,[TotalArrivalSu]

                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
             SELECT @NewMoveNO                         
                   ,tbl.MoveRows                       
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.MoveSu
                   ,tbl.EvaluationPrice
                   ,tbl.FromRackNO
                   ,tbl.ToRackNO
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD
                   ,@DeliveryPlanNO
                   ,tbl.ExpectReturnDate
                   ,tbl.VendorCD
                   ,tbl.CommentInStore
                   ,tbl.RequestRows
                   ,0 AS TotalArrivalSu
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg <> 2 --修正時なので
              ;
        END

  	--赤データ作成***************************************************************************削除・修正時
        --移動区分<>店舗間移動
        IF @MovePurposeType <> @KBN_TENPOKAN
        BEGIN
            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ②赤　（Ｃに対する赤データ　WarehousingKBN＝11）            
            --C②(12),C②(15),C②(19),C②(20),C②(22),C②(16)
            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｄ②赤　（Ｄに対する赤データ　WarehousingKBN＝13）
            --D②(15),D②(22),D②(16)
            INSERT INTO [D_Warehousing]
               ([WarehousingDate]
               ,[SoukoCD]
               ,[RackNO]
               ,[StockNO]
               ,[JanCD]
               ,[AdminNO]
               ,[SKUCD]
               ,[WarehousingKBN]
               ,[DeleteFlg]
               ,[Number]
               ,[NumberRow]
               ,[VendorCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[ToRackNO]
               ,[ToStockNO]
               ,[FromStoreCD]
               ,[FromSoukoCD]
               ,[FromRackNO]
               ,[CustomerCD]
               ,[Quantity]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT DW.WarehousingDate
               ,DW.SoukoCD
               ,DW.RackNO
               ,DW.StockNO			--(D_Stock)☆(移動元)と同じ値
               ,tbl.JanCD	--JanCD
               ,tbl.AdminNO	--AdminNO
               ,tbl.SKUCD	--SKUCD
               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 12 
                                       WHEN @KBN_SYOCD      THEN 31		--15 C②
                                       WHEN @KBN_CHOSEI_ADD THEN 19
                                       WHEN @KBN_CHOSEI_DEL THEN 20
                                       WHEN @KBN_LOCATION   THEN 22
                                       WHEN @KBN_HENPIN     THEN CASE WHEN DW.Quantity < 0 then 16 else 26 end  --2020/10/01 Fukuda
                                       ELSE 0 END)   --WarehousingKBN
               ,1  --DeleteFlg
               ,@MoveNO  --Number★
               ,tbl.MoveRows --NumberRow■
               ,NULL	--VendorCD
               ,DW.ToStoreCD
               ,DW.ToSoukoCD
               ,DW.ToRackNO
               ,DW.ToStockNO
               ,DW.FromStoreCD
               ,DW.FromSoukoCD
               ,DW.FromRackNO
               ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                       ELSE NULL END)    --CustomerCD
               ,DW.Quantity * (-1)
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              INNER JOIN D_Warehousing AS DW
              ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11
                                                            WHEN @KBN_SYOCD      THEN 31	--15
                                                            WHEN @KBN_CHOSEI_ADD THEN 19
                                                            WHEN @KBN_CHOSEI_DEL THEN 20
                                                            WHEN @KBN_LOCATION   THEN 22
                                                            WHEN @KBN_HENPIN     THEN CASE WHEN DW.Quantity < 0 then 16 else 26 end  --2020/10/01 Fukuda
                                                            ELSE 0 END)
              AND DW.DeleteFlg = 0
              AND DW.DeleteDateTime IS NULL
              AND DW.[Number] = @MoveNO
              AND DW.NumberRow = tbl.MoveRows
              ;

            --D②(15)
            IF @MovePurposeType = @KBN_SYOCD
            BEGIN
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT DW.WarehousingDate
                   ,DW.SoukoCD
                   ,DW.RackNO
                   ,DW.StockNO          --(D_Stock)☆(移動元)と同じ値
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD                   
                   ,32     --15 D②--WarehousingKBN
                   ,1  --DeleteFlg
                   ,@MoveNO  --Number★
                   ,tbl.MoveRows --NumberRow■
                   ,NULL    --VendorCD
                   ,DW.ToStoreCD
                   ,DW.ToSoukoCD
                   ,DW.ToRackNO
                   ,DW.ToStockNO
                   ,DW.FromStoreCD
                   ,DW.FromSoukoCD
                   ,DW.FromRackNO
                   ,tbl.JanCD    --CustomerCD
                   ,DW.Quantity * (-1)
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = 32    --15                                                                
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.NumberRow = tbl.MoveRows
                  ;
            END
                          
            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
            BEGIN
	            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｄ②赤　（Ｄに対する赤データ　WarehousingKBN＝13）
	            --D②(14)
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @MoveDate --WarehousingDate
                   ,DW.SoukoCD
                   ,DW.RackNO
                   ,DW.StockNO      --(D_Stock)●(移動先)と同じ値
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 14   --WarehousingKBN
                                            ELSE 0 END)
                   ,1  --DeleteFlg
                   ,@MoveNO  --Number
                   ,tbl.MoveRows --NumberRow
                   ,NULL    --VendorCD
	               ,DW.ToStoreCD
	               ,DW.ToSoukoCD
	               ,DW.ToRackNO
	               ,DW.ToStockNO
	               ,DW.FromStoreCD
	               ,DW.FromSoukoCD
	               ,DW.FromRackNO
                   ,NULL    --CustomerCD
                   ,DW.Quantity * (-1)  --Quantity 移動数 * -1(マイナス値とする)
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN =  (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13
                                                            ELSE 0 END)
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.NumberRow = tbl.MoveRows
                  ;
            END
        END          
    END
    
    --新規--************************************************************************************新規
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';

        --伝票番号採番
        EXEC Fnc_GetNumber
            18,        --in伝票種別 18
            @MoveDate, --in基準日
            @StoreCD,  --in店舗CD
            @Operator,
            @NewMoveNO OUTPUT
            ;
        
        IF ISNULL(@NewMoveNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --移動区分=店舗間移動,返品
        IF @MovePurposeType IN (@KBN_TENPOKAN ,@KBN_HENPIN) 
        BEGIN
            --【D_DeliveryPlan】配送予定情報　テーブル転送仕様J
            --伝票番号採番
            EXEC Fnc_GetNumber
                19,             --in伝票種別 19
                @MoveDate, --in基準日
                @StoreCD,       --in店舗CD
                @Operator,
                @DeliveryPlanNO OUTPUT
                ;
            
            IF ISNULL(@DeliveryPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END

            INSERT INTO [D_DeliveryPlan]
                   ([DeliveryPlanNO]
                   ,[DeliveryKBN]
                   ,[Number]
                   ,[DeliveryName]
                   ,[DeliverySoukoCD]
                   ,[DeliveryZip1CD]
                   ,[DeliveryZip2CD]
                   ,[DeliveryAddress1]
                   ,[DeliveryAddress2]
                   ,[DeliveryMailAddress]
                   ,[DeliveryTelphoneNO]
                   ,[DeliveryFaxNO]
                   ,[DecidedDeliveryDate]
                   ,[DecidedDeliveryTime]
                   ,[CarrierCD]
                   ,[PaymentMethodCD]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[InvoiceNO]
                   ,[DeliveryPlanDate]
                   ,[HikiateFLG]
                   ,[IncludeFLG]
                   ,[OntheDayFLG]
                   ,[ExpressFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
            SELECT
                    @DeliveryPlanNO
                   ,2 AS DeliveryKBN	--(1:販売、2:倉庫移動)
                   ,@NewMoveNO	AS Number
                   ,NULL	--[DeliveryName]
                   ,@FromSoukoCD	--[DeliverySoukoCD]
                   ,NULL	--[DeliveryZip1CD]
                   ,NULL	--[DeliveryZip2CD]
                   ,NULL	--[DeliveryAddress1]
                   ,NULL	--[DeliveryAddress2]
                   ,NULL	--[DeliveryMailAddress]
                   ,NULL	--[DeliveryTelphoneNO]
                   ,NULL	--[DeliveryFaxNO]
                   ,NULL	--[DecidedDeliveryDate]
                   ,NULL	--[DecidedDeliveryTime]
                   ,NULL	--[CarrierCD]
                   ,NULL	--[PaymentMethodCD]
                   ,NULL	--[CommentInStore]
                   ,NULL	--[CommentOutStore]
                   ,NULL	--[InvoiceNO]
                   ,NULL	--[DeliveryPlanDate]
                   ,0	--[HikiateFLG]
                   ,0	--[IncludeFLG]
                   ,0	--[OntheDayFLG]
                   ,0	--[ExpressFLG]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
              ;            
        END  
        
        --【D_Move】在庫移動　Table転送仕様Ａ
        INSERT INTO [D_Move]
           ([MoveNO]
           ,[StoreCD]
           ,[MoveDate]
           ,[RequestNO]
           ,[MovePurposeKBN]
           ,[FromSoukoCD]
           ,[ToStoreCD]
           ,[ToSoukoCD]
           ,[MoveInputDateTime]
           ,[StaffCD]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     	VALUES
           (@NewMoveNO
           ,@StoreCD
           ,convert(date,@MoveDate)
           ,@RequestNO
           ,@MovePurposeKBN
           ,@FromSoukoCD
           ,@ToStoreCD
           ,@ToSoukoCD
           ,SYSDATETIME()	--MoveInputDateTime
           ,@StaffCD

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

        --【D_MoveDetails】在庫移動明細　Table転送仕様Ｂ
        INSERT INTO [D_MoveDetails]
                   ([MoveNO]
                   ,[MoveRows]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[MoveSu]
                   ,[EvaluationPrice]
                   ,[FromRackNO]
                   ,[ToRackNO]
                   ,[NewJanCD]
                   ,[NewAdminNO]
                   ,[NewSKUCD]
                   ,[DeliveryPlanNO]
                   ,[ExpectReturnDate]
                   ,[VendorCD]
                   ,[CommentInStore]
                   ,[RequestRows]
                   ,[TotalArrivalSu]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @NewMoveNO                         
                   ,tbl.MoveRows                       
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.MoveSu
                   ,tbl.EvaluationPrice
                   ,tbl.FromRackNO
                   ,tbl.ToRackNO
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD
                   ,@DeliveryPlanNO
                   ,tbl.ExpectReturnDate
                   ,tbl.VendorCD
                   ,tbl.CommentInStore
                   ,tbl.RequestRows
                   ,0 AS TotalArrivalSu
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
              
        --移動区分=店舗間移動,返品
        IF @MovePurposeType IN (@KBN_TENPOKAN, @KBN_HENPIN)
        BEGIN
            --【D_DeliveryPlanDetails】配送予定明細　テーブル転送仕様K
            INSERT INTO [D_DeliveryPlanDetails]
               ([DeliveryPlanNO]
               ,[DeliveryPlanRows]
               ,[Number]
               ,[NumberRows]
               ,[CommentInStore]
               ,[CommentOutStore]
               ,[HikiateFLG]
               ,[UpdateCancelKBN]
               ,[DeliveryOrderComIn]
               ,[DeliveryOrderComOut]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
             SELECT  
                @DeliveryPlanNO
               ,tbl.MoveRows AS DeliveryPlanRows
               ,@NewMoveNO AS Number
               ,tbl.MoveRows  As NumberRows
               ,NULL	--CommentInStore]
               ,NULL	--CommentOutStore]
               ,0	--HikiateFLG]
               ,0	--UpdateCancelKBN]
               ,NULL	--DeliveryOrderComIn]
               ,NULL	--DeliveryOrderComOut]                        
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
        END

        DECLARE @StockSu int;
        DECLARE @AllowableSu int;
        DECLARE @WIdoSu int;
        DECLARE @WUpdSu int;
        DECLARE @ToStockNO varchar(11);

        --明細数分Insert★
        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===
        	SET @ArrivalPlanNO = NULL;
        	
            IF @MovePurposeType = @KBN_TENPOKAN
            BEGIN
               --伝票番号採番
                EXEC Fnc_GetNumber
                    22,             --in伝票種別 5
                    @MoveDate,      --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @ArrivalPlanNO OUTPUT
                    ;
                
                IF ISNULL(@ArrivalPlanNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
            
                --【D_ArrivalPlan】入荷予定情報　Table転送仕様Ｌ
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
                       ,2 AS ArrivalPlanKBN
                       ,@NewMoveNO	AS Number
                       ,tbl.MoveRows AS NumberRows
                       ,tbl.MoveRows AS NumberSEQ
                       ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                      FROM M_Souko A 
                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) AS ArrivalPlanDate
                       ,0 AS ArrivalPlanMonth
                       ,NULL AS ArrivalPlanCD
                       ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                      FROM M_Souko A 
                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) AS CalcuArrivalPlanDate
                       ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                       ,@StaffCD
                       ,1 AS LastestFLG
                       ,NULL AS EDIImportNO
                       ,@ToSoukoCD
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,tbl.MoveSu AS ArrivalPlanSu
                       ,0   --ArrivalSu
                       ,NULL AS ArrivalPlanNO    
                       ,NULL AS OrderCD
                       ,@FromSoukoCD
                       ,@ToStoreCD
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
            END
            
            SET @ToStockNO = NULL;
            --【D_Stock】在庫　移動先　テーブル転送仕様Ｈ/Ｈ③店舗移動/H④返品
            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
            BEGIN
                --伝票番号採番●ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in伝票種別 21
                    @MoveDate, --in基準日
                    @StoreCD,  --in店舗CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Stock】Insert    
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
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN @FromSoukoCD
                                               ELSE ISNULL(@ToSoukoCD,@FromSoukoCD) END)    --SoukoCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN NULL
                                               WHEN @KBN_SYOCD THEN tbl.FromRackNO  
                                               ELSE tbl.ToRackNO END)   --RackNO
                       ,@ArrivalPlanNO    --ArrivalPlanNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewSKUCD
                                               ELSE tbl.SKUCD END)
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewAdminNO
                                               ELSE tbl.AdminNO END)
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewJanCD
                                               ELSE tbl.JanCD END)
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 1   
                                               ELSE 0 END)          --  ArrivalYetFLG(0:入荷済、1:未入荷)
                       ,3   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                       ,(SELECT A.ArrivalPlanDate FROM D_ArrivalPlan AS A
                          WHERE A.ArrivalPlanNO = @ArrivalPlanNO)    --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 0
                                               WHEN @KBN_HENPIN   THEN 0
                                               ELSE tbl.MoveSu    END)  --StockSu
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN tbl.MoveSu
                                               ELSE 0 END)   --PlanSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                               ELSE tbl.MoveSu END)   --AllowableSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                               ELSE tbl.MoveSu END)  --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL    --OriginalStockNO
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.ExpectReturnDate
                                               ELSE NULL END)    --ExpectReturnDate
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.MoveSu
                                               ELSE 0 END)      --ReturnPlanSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.VendorCD
                                               ELSE NULL END) --[VendorCD]
                       ,NULL   --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
            END      
        
            SET @WIdoSu = @tblMoveSu;

            --【D_Stock】在庫　移動元　テーブル転送仕様G①
            --移動数を在庫で引当
            --カーソル定義(D_Stock_SelectSuryo参照)
            DECLARE CUR_Stock CURSOR FOR
                SELECT DS.StockSu
                    ,DS.AllowableSu
                    ,DS.StockNO
                from D_Stock DS
                WHERE DS.SoukoCD = @FromSoukoCD
                AND DS.RackNO = @tblFromRackNO
                AND DS.AdminNO = @tblAdminNO
                AND DS.DeleteDateTime is null 
                AND DS.AllowableSu > 0 
                AND DS.ArrivalYetFlg = 0 
                ;
            
            --カーソルオープン
            OPEN CUR_Stock;

            --最初の1行目を取得して変数へ値をセット
            FETCH NEXT FROM CUR_Stock
            INTO @StockSu, @AllowableSu, @StockNO;
            
            --データの行数分ループ処理を実行する
            WHILE @@FETCH_STATUS = 0
            BEGIN
            -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock
                IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                BEGIN
                	SET @WUpdSu = @WIdoSu;
                END
                ELSE IF @StockSu < @AllowableSu
                BEGIN
                	SET @WUpdSu = @StockSu;
                END
                ELSE
                BEGIN
                	SET @WUpdSu = @AllowableSu;
                END
                
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] - @WUpdSu
                      ,[AllowableSu] = [AllowableSu] - @WUpdSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                      --,[ExpectReturnDate]   = tbl.ExpectReturnDate	2020.09.18 del
                      --,[VendorCD]           = tbl.VendorCD
                      --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Stock AS DS
                 INNER JOIN @Table tbl
                 ON tbl.MoveRows = @tblMoveRows
                 WHERE DS.DeleteDateTime IS NULL
                 AND DS.StockNO = @StockNO
                ;
            
                --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ
                --C(11),C(15),C(19),C(20),C(22),C(16),C(90)
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @MoveDate --WarehousingDate
                   ,@FromSoukoCD AS SoukoCD
                   ,tbl.FromRackNO    --RackNO
                   ,@StockNO	--(D_Stock)☆(移動元)と同じ値
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                           WHEN @KBN_SYOCD      THEN 31	--15
                                           WHEN @KBN_CHOSEI_ADD THEN 19
                                           WHEN @KBN_CHOSEI_DEL THEN 20
                                           WHEN @KBN_LOCATION   THEN 22
                                           WHEN @KBN_HENPIN     THEN 16 --
                                           WHEN @KBN_TENPOKAN   THEN 90
                                           ELSE 0 END)   --WarehousingKBN
                   ,0  --DeleteFlg
                   ,@NewMoveNO  --Number
                   ,tbl.MoveRows --NumberRow
                   ,NULL    --VendorCD
                   
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                           WHEN @KBN_SYOCD      THEN @FromStoreCD
                                           WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                           WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                           WHEN @KBN_LOCATION   THEN @FromStoreCD
                                           WHEN @KBN_HENPIN     THEN @ToStoreCD		--2020.10.6
                                           WHEN @KBN_TENPOKAN   THEN @ToStoreCD
                                           ELSE NULL END)	--ToStoreCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                           WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                           WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                           WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                           WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                           WHEN @KBN_HENPIN     THEN @ToSoukoCD		--2020.10.6
                                           WHEN @KBN_TENPOKAN   THEN @ToSoukoCD
                                           ELSE NULL END)	--ToSoukoCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                           WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                           WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                           WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                           WHEN @KBN_LOCATION   THEN tbl.ToRackNO		--2020.10.29 chg
                                           WHEN @KBN_HENPIN     THEN tbl.ToRackNO		--2020.09.18 add
                                           WHEN @KBN_TENPOKAN   THEN NULL
                                           ELSE NULL END)	--ToRackNO
                   
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO    -- (D_Stock)●(移動先)と同じ値
                                           WHEN @KBN_SYOCD      THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                           WHEN @KBN_CHOSEI_ADD THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                           WHEN @KBN_CHOSEI_DEL THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                           WHEN @KBN_LOCATION   THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                           WHEN @KBN_HENPIN     THEN @ToStockNO    -- (D_Stock)●(移動先)と同じ値
                                           WHEN @KBN_TENPOKAN   THEN @ToStockNO    -- (D_Stock)●(移動先)と同じ値
                                           ELSE NULL END)  --ToStockNO
                   ,@FromStoreCD	--FromStoreCD
                   ,@FromSoukoCD	--FromSoukoCD
                   ,tbl.FromRackNO	--FromRackNO
                   ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                           ELSE NULL END)    --CustomerCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                           WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                           WHEN @KBN_CHOSEI_ADD THEN @WUpdSu   --移動数(プラス値とする)
                                           WHEN @KBN_CHOSEI_DEL THEN @WUpdSu   --移動数(プラス値とする)
                                           WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                           WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                           WHEN @KBN_TENPOKAN   THEN @WUpdSu * (-1) --Quantity
                                           ELSE @WUpdSu END) 
                   
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
                      
                IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL, @KBN_TENPOKAN)
                BEGIN
                    --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｄ
                    --D(13),D(15),D(22),D(16)
                    INSERT INTO [D_Warehousing]
                       ([WarehousingDate]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[StockNO]
                       ,[JanCD]
                       ,[AdminNO]
                       ,[SKUCD]
                       ,[WarehousingKBN]
                       ,[DeleteFlg]
                       ,[Number]
                       ,[NumberRow]
                       ,[VendorCD]
                       ,[ToStoreCD]
                       ,[ToSoukoCD]
                       ,[ToRackNO]
                       ,[ToStockNO]
                       ,[FromStoreCD]
                       ,[FromSoukoCD]
                       ,[FromRackNO]
                       ,[CustomerCD]
                       ,[Quantity]
                       ,[Program]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT @MoveDate --WarehousingDate
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN ISNULL(@ToSoukoCD,@FromSoukoCD)
                                               WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                               WHEN @KBN_LOCATION THEN @FromSoukoCD
                                               WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                               ELSE NULL END) AS SoukoCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                               WHEN @KBN_SYOCD    THEN tbl.FromRackNO 
                                               WHEN @KBN_LOCATION THEN tbl.ToRackNO 		--2020.10.29 chg
                                               WHEN @KBN_HENPIN   THEN tbl.ToRackNO
                                               ELSE tbl.ToRackNO  END)   --RackNO　商品CD付替時のみ移動元棚番
                       ,@ToStockNO  --(D_Stock)●(移動先)と同じ値
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                               ELSE tbl.JanCD END)  --JanCD
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewAdminNO
                                               ELSE tbl.AdminNO END)    --AdminNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewSKUCD
                                               ELSE tbl.SKUCD END)      --SKUCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13 
                                               WHEN @KBN_SYOCD    THEN 32	--15
                                               WHEN @KBN_LOCATION THEN 22
                                               WHEN @KBN_HENPIN   THEN 26 --16 2020/10/01 Fukuda
                                               ELSE 0 END)   --WarehousingKBN
                       ,0  --DeleteFlg
                       ,@NewMoveNO  --Number
                       ,tbl.MoveRows --NumberRow
                       ,NULL     --VendorCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToStoreCD
                                               WHEN @KBN_SYOCD    THEN @FromStoreCD
                                               WHEN @KBN_LOCATION THEN @FromStoreCD
                                               WHEN @KBN_HENPIN   THEN @ToStoreCD
                                               ELSE NULL END)	--ToStoreCD
                       
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToSoukoCD
                                               WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                               WHEN @KBN_LOCATION THEN @FromSoukoCD
                                               WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                               ELSE NULL END)	--ToSoukoCD
                                            
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                               WHEN @KBN_SYOCD    THEN tbl.FromRackNO
                                               WHEN @KBN_LOCATION THEN tbl.ToRackNO     --2020.10.29 chg
                                               WHEN @KBN_HENPIN   THEN tbl.ToRackNO     --2020.09.18 add
                                               ELSE NULL END)	--ToRackNO
                                            
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @StockNO    --(D_Stock)☆(移動元)と同じ値
                                               WHEN @KBN_SYOCD    THEN @ToStockNO  --(D_Stock)●(移動先)と同じ値
                                               WHEN @KBN_LOCATION THEN @ToStockNO  --(D_Stock)●(移動先)と同じ値
                                               WHEN @KBN_HENPIN   THEN @StockNO    --(D_Stock)☆(移動元)と同じ値
                                               ELSE NULL END)	--ToStockNO
                       ,@FromStoreCD    --FromStoreCD
                       ,@FromSoukoCD    --FromSoukoCD
                       ,tbl.FromRackNO  --FromRackNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.JanCD
                                               ELSE NULL END)    --CustomerCD
                       ,@WUpdSu  --Quantity
                       ,@Program  --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL

                      FROM @Table tbl
                      WHERE tbl.MoveRows = @tblMoveRows
                      ;
                END
                
                SET @ReserveNO = '';
                --移動区分=店舗間移動
                IF @MovePurposeType = @KBN_TENPOKAN
                BEGIN
                    --【D_Reserve】引当　テーブル転送仕様I①
                    --伝票番号採番
                    EXEC Fnc_GetNumber
                        12,        --in伝票種別 12
                        @MoveDate, --in基準日
                        @StoreCD,  --in店舗CD
                        @Operator,
                        @ReserveNO OUTPUT
                        ;
                    
                    IF ISNULL(@ReserveNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --【D_Reserve】（Insert）
                    INSERT INTO [D_Reserve]
                           ([ReserveNO]
                           ,[ReserveKBN]
                           ,[Number]
                           ,[NumberRows]
                           ,[StockNO]
                           ,[SoukoCD]
                           ,[JanCD]
                           ,[SKUCD]
                           ,[AdminNO]
                           ,[ReserveSu]
                           ,[ShippingPossibleDate]
                           ,[ShippingPossibleSU]
                           ,[ShippingOrderNO]
                           ,[ShippingOrderRows]
                           ,[CompletedPickingNO]
                           ,[CompletedPickingRow]
                           ,[CompletedPickingDate]
                           ,[ShippingSu]
                           ,[ReturnKBN]
                           ,[OriginalReserveNO]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                     SELECT
                           @ReserveNO
                           ,2 AS ReserveKBN     --2 (1:受注、2:移動)
                           ,@NewMoveNO AS Number
                           ,@tblMoveRows AS NumberRows
                           ,@StockNO
                           ,@FromSoukoCD
                           ,tbl.JanCD
                           ,tbl.SKUCD
                           ,tbl.AdminNO
                           ,0 AS ReserveSu --明細入荷数
                           ,NULL    --ShippingPossibleDate
                           ,0       --ShippingPossibleSU
                           ,NULL    --ShippingOrderNO
                           ,0       --ShippingOrderRows
                           ,NULL    --CompletedPickingNO
                           ,0       --CompletedPickingRow
                           ,NULL    --CompletedPickingDate
                           ,0       --ShippingSu
                           ,0       --ReturnKBN
                           ,NULL    --OriginalReserveNO
                     
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                     FROM @Table tbl
                     WHERE tbl.MoveRows = @tblMoveRows
                    ;
                END
                
                SET @WIdoSu = @WIdoSu - @WUpdSu;
                
                IF @WIdoSu = 0
                BEGIN
                	--次の明細レコードへ
                	BREAK;
                END
                
                --次の行のデータを取得して変数へ値をセット
                FETCH NEXT FROM CUR_Stock
                INTO @StockSu, @AllowableSu, @StockNO;
            END		--LOOPの終わり***************************************CUR_Stock
            
            --カーソルを閉じる
            CLOSE CUR_Stock;
            DEALLOCATE CUR_Stock;
            
            IF @MovePurposeType = @KBN_CHOSEI_ADD AND @WIdoSu > 0
            BEGIN
                --Insert
                --【D_Stock】在庫　移動元　テーブル転送仕様H⑨
                --伝票番号採番●ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in伝票種別 21
                    @MoveDate, --in基準日
                    @StoreCD,  --in店舗CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Stock】Insert    
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
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,@FromSoukoCD	--SoukoCD
                       ,tbl.FromRackNO	--RackNO
                       ,NULL    --ArrivalPlanNO
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,0   --  ArrivalYetFLG(0:入荷済、1:未入荷)
                       ,3   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)	2020.09.18
                       ,NULL   --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,@WIdoSu		--tbl.MoveSu  --StockSu
                       ,0   --PlanSu
                       ,@WIdoSu		--tbl.MoveSu   --AllowableSu
                       ,@WIdoSu		--tbl.MoveSu   --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL  --OriginalStockNO
                       ,NULL  --ExpectReturnDate
                       ,0 	--ReturnPlanSu
                       ,NULL  --[VendorCD]
                       ,NULL   --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
                  
                --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ
                --C(19)
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @MoveDate --WarehousingDate
                   ,@FromSoukoCD AS SoukoCD
                   ,tbl.FromRackNO    --RackNO
                   ,@ToStockNO	--(D_Stock)☆(移動元)と同じ値
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,19 --WarehousingKBN
                   ,0  --DeleteFlg
                   ,@NewMoveNO  --Number
                   ,tbl.MoveRows --NumberRow
                   ,NULL    --VendorCD
                   ,@FromStoreCD
                   ,@FromSoukoCD
                   ,tbl.FromRackNO
                   ,@ToStockNO    -- (D_Stock)☆(移動元)と同じ値--ToStockNO
                   ,@FromStoreCD
                   ,@FromSoukoCD
                   ,tbl.FromRackNO
                   ,NULL    --CustomerCD
                   ,@WIdoSu   --移動数(プラス値とする)                   
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
                   
            END
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
        END            --LOOPの終わり*******************************************************CUR_TABLE
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
--        DEALLOCATE CUR_TABLE;
    END
    
    ELSE IF @OperateMode = 3 --削除***************************************************************削除
    BEGIN
        SET @OperateModeNm = '削除';
        
        EXEC PRC_ZaikoIdou_A2B2
            @MoveNO,
            @Operator,
            @SYSDATETIME
            ;
            
        --移動区分=店舗間移動
        IF @MovePurposeType = @KBN_TENPOKAN
        BEGIN
            --【D_Reserve】引当  Table転送仕様I②
            UPDATE [D_Reserve] SET
                   [UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  ,[DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
                  
             FROM @Table AS tbl
             WHERE @MoveNO = D_Reserve.Number
             --AND tbl.MoveRows = D_Reserve.NumberRows
            ;
        END
        --移動区分=店舗間移動,返品
        IF @MovePurposeType IN (@KBN_TENPOKAN, @KBN_HENPIN)
        BEGIN
            --【D_DeliveryPlan】配送予定情報  Table転送仕様J②
            UPDATE [D_DeliveryPlan] SET
                   [Number] = NULL
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
             WHERE @MoveNO = D_DeliveryPlan.Number
            ;
            
            --【D_DeliveryPlanDetails】配送予定情報　Table転送仕様K②
            UPDATE [D_DeliveryPlanDetails] SET
                   [Number] = NULL
                  ,[NumberRows] = 0
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM @Table AS tbl
             WHERE @MoveNO = D_DeliveryPlanDetails.Number
             --AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
            ;
            
            IF @MovePurposeType = @KBN_TENPOKAN
            BEGIN
                UPDATE [D_ArrivalPlan] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
                WHERE @MoveNO = D_ArrivalPlan.Number
                ;
            END
        END
    END
    
    
    IF @OperateMode >= 2    --削除・修正時**************************************************削除・修正時
    BEGIN
        --移動区分=店舗間移動
        IF @MovePurposeType = @KBN_TENPOKAN  
        BEGIN
            --【D_Stock】在庫　移動先　テーブル転送仕様Ｈ②赤
            UPDATE [D_Stock] SET
                   [PlanSu] = [PlanSu] - tbl.OldMoveSu
                  ,[AllowableSu] = [AllowableSu] - tbl.OldMoveSu
                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - tbl.OldMoveSu
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM D_Stock AS DS
             INNER JOIN D_ArrivalPlan AS DA
             ON DA.ArrivalPlanNO = DS.ArrivalPlanNO
             INNER JOIN @Table tbl
             ON DA.ArrivalPlanNO = tbl.ArrivalPlanNO
             WHERE DS.DeleteDateTime IS NULL
             AND DS.PlanSu > 0		--勝手に追加
            ;

            IF @OperateMode = 2--*******************************************************修正
            BEGIN 
                --【D_DeliveryPlan】配送予定情報  Table転送仕様J①
                UPDATE [D_DeliveryPlan] SET
                       [DeliveryKBN] = 2    --(1:販売、2:倉庫移動)
                      ,[Number]      = @MoveNO
                      ,[DeliverySoukoCD]    =  @ToSoukoCD
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlan.Number
                ;
	            
                --【D_DeliveryPlanDetails】配送予定情報　Table転送仕様K①
                UPDATE [D_DeliveryPlanDetails] SET
                       [UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlanDetails.Number
                 AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
                ; 

                --【D_ArrivalPlan】     Update   Table転送仕様Ｌ
                UPDATE [D_ArrivalPlan] SET
                   [ArrivalPlanDate] = DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                      FROM M_Souko A 
                                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) --AS ArrivalPlanDate
                  ,[CalcuArrivalPlanDate] = DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                      FROM M_Souko A 
                                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) --AS ArrivalPlanDate
                  ,[SoukoCD]        = @ToSoukoCD
                  ,[SKUCD]          = tbl.SKUCD
                  ,[AdminNO]        = tbl.AdminNO
                  ,[JanCD]          = tbl.JanCD
                  ,[ArrivalPlanSu]  = tbl.MoveSu
                  ,[FromSoukoCD   ] = @FromSoukoCD
                  ,[ToStoreCD]      = @ToStoreCD
                  ,[UpdateOperator] =  @Operator  
                  ,[UpdateDateTime] =  @SYSDATETIME
                
                  FROM @Table AS tbl
                 WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
                   AND tbl.MoveRows      = D_ArrivalPlan.NumberRows
                   AND tbl.UpdateFlg     = 1
                ;
                
                --行削除
                UPDATE [D_ArrivalPlan] SET
                   [DeleteOperator] = @Operator  
                  ,[DeleteDateTime] = @SYSDATETIME
                
                 FROM @Table AS tbl
                 WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
                   AND tbl.MoveRows      = D_ArrivalPlan.NumberRows
                   AND tbl.UpdateFlg     = 2
                ;
                
                --明細数分Insert★
                --カーソルオープン
                OPEN CUR_TABLE;

                --最初の1行目を取得して変数へ値をセット
                FETCH NEXT FROM CUR_TABLE
                INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
                
                --データの行数分ループ処理を実行する
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ループ内の実際の処理 ここから===
                    IF @tblUpdateFlg = 0    --追加行のみ
                    BEGIN
                      --伝票番号採番
                        EXEC Fnc_GetNumber
                            22,             --in伝票種別 5
                            @MoveDate,      --in基準日
                            @StoreCD,       --in店舗CD
                            @Operator,
                            @ArrivalPlanNO OUTPUT
                            ;
                        
                        IF ISNULL(@ArrivalPlanNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                    
                        --【D_ArrivalPlan】入荷予定情報　Table転送仕様Ｌ
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
                               ,2 AS ArrivalPlanKBN
                               ,@MoveNO  AS Number
                               ,tbl.MoveRows AS NumberRows
                               ,tbl.MoveRows AS NumberSEQ
                               ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                      FROM M_Souko A 
                                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate))  AS ArrivalPlanDate
                               ,0 AS ArrivalPlanMonth
                               ,NULL AS ArrivalPlanCD
                               ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                      FROM M_Souko A 
                                                      WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                      ORDER BY A.ChangeDate desc),convert(date,@MoveDate))  AS CalcuArrivalPlanDate
                               ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                               ,@StaffCD
                               ,1 AS LastestFLG
                               ,NULL AS EDIImportNO
                               ,@ToSoukoCD
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,tbl.MoveSu AS ArrivalPlanSu
                               ,0   --ArrivalSu
                               ,NULL AS ArrivalPlanNO    
                               ,NULL AS OrderCD
                               ,@FromSoukoCD
                               ,@ToStoreCD
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                      	WHERE tbl.MoveRows = @tblMoveRows
                          ;
                	END		--追加行のみ

                    IF @tblUpdateFlg <> 2    --削除行以外
                    BEGIN
                        --【D_Stock】在庫　移動先　テーブル転送仕様Ｈ③店舗移動
                        --伝票番号採番●ToStockNO
                        EXEC Fnc_GetNumber
                            21,        --in伝票種別 21
                            @MoveDate, --in基準日
                            @StoreCD,  --in店舗CD
                            @Operator,
                            @ToStockNO OUTPUT
                            ;
                        
                        IF ISNULL(@ToStockNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                        
                        --【D_Stock】Insert    
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
                                @ToStockNO
                               ,@ToSoukoCD
                               ,NULL   --RackNO
                               ,(CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                                                    ELSE tbl.ArrivalPlanNO END)    --ArrivalPlanNO
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,1   --  ArrivalYetFLG(0:入荷済、1:未入荷)
                               ,3   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                               ,(SELECT A.ArrivalPlanDate FROM D_ArrivalPlan AS A
                                  WHERE A.ArrivalPlanNO = (CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                                                                              ELSE tbl.ArrivalPlanNO END))    --ArrivalPlanDate
                               ,NULL    --ArrivalDate
                               ,0  --StockSu
                               ,tbl.MoveSu   --PlanSu
                               ,tbl.MoveSu   --AllowableSu
                               ,tbl.MoveSu   --AnotherStoreAllowableSu
                               ,0    --ReserveSu
                               ,0   --InstructionSu
                               ,0   --ShippingSu
                               ,NULL    --OriginalStockNO
                               ,NULL    --ExpectReturnDate
                               ,NULL    --ReturnDate
                               ,0   --ReturnSu
                         
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;  
                      

                        SET @WIdoSu = @tblMoveSu;

                        --【D_Stock】在庫　移動元　テーブル転送仕様G①
                        --移動数を在庫で引当
                        --カーソル定義(D_Stock_SelectSuryo参照)
                        DECLARE CUR_Stock CURSOR FOR
                            SELECT DS.StockSu
                                ,DS.AllowableSu
                                ,DS.StockNO
                            from D_Stock DS
                            WHERE DS.SoukoCD = @FromSoukoCD
                            AND DS.RackNO = @tblFromRackNO
                            AND DS.AdminNO = @tblAdminNO
                            AND DS.DeleteDateTime is null 
                            AND DS.AllowableSu > 0 
                            AND DS.ArrivalYetFlg = 0 
                            ;
                            
                        --カーソルオープン
                        OPEN CUR_Stock;

                        --最初の1行目を取得して変数へ値をセット
                        FETCH NEXT FROM CUR_Stock
                        INTO @StockSu, @AllowableSu, @StockNO;
                        
                        --データの行数分ループ処理を実行する
                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                        -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock
                            IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                            BEGIN
                                SET @WUpdSu = @WIdoSu;
                            END
                            ELSE IF @StockSu < @AllowableSu
                            BEGIN
                                SET @WUpdSu = @StockSu;
                            END
                            ELSE
                            BEGIN
                                SET @WUpdSu = @AllowableSu;
                            END
                            
                            UPDATE [D_Stock] SET
                                   [StockSu] = [StockSu] - @WUpdSu
                                  ,[AllowableSu] = [AllowableSu] - @WUpdSu
                                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                                  --,[ExpectReturnDate]   = tbl.ExpectReturnDate	2020.09.18 del
                                  --,[VendorCD]           = tbl.VendorCD
                                  --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                                  ,[UpdateOperator]     =  @Operator  
                                  ,[UpdateDateTime]     =  @SYSDATETIME
                                  
                             FROM D_Stock AS DS
                             INNER JOIN @Table tbl
                             ON tbl.MoveRows = @tblMoveRows
                             WHERE DS.DeleteDateTime IS NULL
                             AND DS.StockNO = @StockNO
                            ;

                            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ
                            --C(90)
                            INSERT INTO [D_Warehousing]
                               ([WarehousingDate]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[StockNO]
                               ,[JanCD]
                               ,[AdminNO]
                               ,[SKUCD]
                               ,[WarehousingKBN]
                               ,[DeleteFlg]
                               ,[Number]
                               ,[NumberRow]
                               ,[VendorCD]
                               ,[ToStoreCD]
                               ,[ToSoukoCD]
                               ,[ToRackNO]
                               ,[ToStockNO]
                               ,[FromStoreCD]
                               ,[FromSoukoCD]
                               ,[FromRackNO]
                               ,[CustomerCD]
                               ,[Quantity]
                               ,[Program]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                            SELECT @MoveDate --WarehousingDate
                               ,@FromSoukoCD AS SoukoCD
                               ,tbl.FromRackNO    --RackNO
                               ,@StockNO    --(D_Stock)☆(移動元)と同じ値
                               ,tbl.JanCD
                               ,tbl.AdminNO
                               ,tbl.SKUCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                                       WHEN @KBN_SYOCD      THEN 31		--15
                                                       WHEN @KBN_CHOSEI_ADD THEN 19
                                                       WHEN @KBN_CHOSEI_DEL THEN 20
                                                       WHEN @KBN_LOCATION   THEN 22
                                                       WHEN @KBN_HENPIN     THEN 16 --
                                                       WHEN @KBN_TENPOKAN   THEN 90
                                                       ELSE 0 END)   --WarehousingKBN
                               ,0  --DeleteFlg
                               ,@MoveNO  --Number	店舗移動の場合は採番しなおさない
                               ,tbl.MoveRows --NumberRow
                               ,NULL    --VendorCD
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                                       WHEN @KBN_SYOCD      THEN @FromStoreCD
                                                       WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                                       WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                                       WHEN @KBN_LOCATION   THEN @FromStoreCD
                                                       WHEN @KBN_HENPIN     THEN @ToStoreCD		--2020.10.6
                                                       WHEN @KBN_TENPOKAN   THEN @ToStoreCD
                                                       ELSE NULL END)
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                                       WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                                       WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                                       WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN     THEN @ToSoukoCD		--2020.10.6
                                                       WHEN @KBN_TENPOKAN   THEN @ToSoukoCD
                                                       ELSE NULL END)
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                                       WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                                       WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                                       WHEN @KBN_LOCATION   THEN tbl.FromRackNO
                                                       WHEN @KBN_HENPIN     THEN tbl.ToRackNO		--2020.09.18 add
                                                       WHEN @KBN_TENPOKAN   THEN NULL
                                                       ELSE NULL END)
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO   -- (D_Stock)●(移動先)と同じ値
                                                       WHEN @KBN_SYOCD      THEN @StockNO     --(D_Stock)☆(移動元)と同じ値
                                                       WHEN @KBN_CHOSEI_ADD THEN @StockNO     --(D_Stock)☆(移動元)と同じ値
                                                       WHEN @KBN_CHOSEI_DEL THEN @StockNO     --(D_Stock)☆(移動元)と同じ値
                                                       WHEN @KBN_LOCATION   THEN @StockNO     --(D_Stock)☆(移動元)と同じ値
                                                       WHEN @KBN_HENPIN     THEN @ToStockNO   -- (D_Stock)●(移動先)と同じ値
                                                       WHEN @KBN_TENPOKAN   THEN @ToStockNO   -- (D_Stock)●(移動先)と同じ値
                                                       ELSE NULL END)  --ToStockNO
                               ,@FromStoreCD
                               ,@FromSoukoCD
                               ,tbl.FromRackNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                                       ELSE NULL END)    --CustomerCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_CHOSEI_ADD THEN @WUpdSu          --移動数(プラス値とする)
                                                       WHEN @KBN_CHOSEI_DEL THEN @WUpdSu          --移動数(プラス値とする)
                                                       WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_TENPOKAN   THEN @WUpdSu * (-1)   --Quantity
                                                       ELSE @WUpdSu END) 
                               
                               ,@Program  --Program
                               
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL
                               ,NULL

                              FROM @Table tbl
                              WHERE tbl.MoveRows = @tblMoveRows
                              ;

                            --【D_Reserve】引当　テーブル転送仕様I①
                            --伝票番号採番
                            EXEC Fnc_GetNumber
                                12,        --in伝票種別 12
                                @MoveDate, --in基準日
                                @StoreCD,  --in店舗CD
                                @Operator,
                                @ReserveNO OUTPUT
                                ;
                            
                            IF ISNULL(@ReserveNO,'') = ''
                            BEGIN
                                SET @W_ERR = 1;
                                RETURN @W_ERR;
                            END
                            
                            --【D_Reserve】（Insert）
                            INSERT INTO [D_Reserve]
                                   ([ReserveNO]
                                   ,[ReserveKBN]
                                   ,[Number]
                                   ,[NumberRows]
                                   ,[StockNO]
                                   ,[SoukoCD]
                                   ,[JanCD]
                                   ,[SKUCD]
                                   ,[AdminNO]
                                   ,[ReserveSu]
                                   ,[ShippingPossibleDate]
                                   ,[ShippingPossibleSU]
                                   ,[ShippingOrderNO]
                                   ,[ShippingOrderRows]
                                   ,[CompletedPickingNO]
                                   ,[CompletedPickingRow]
                                   ,[CompletedPickingDate]
                                   ,[ShippingSu]
                                   ,[ReturnKBN]
                                   ,[OriginalReserveNO]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime]
                                   ,[DeleteOperator]
                                   ,[DeleteDateTime])
                             SELECT
                                   @ReserveNO
                                   ,2 AS ReserveKBN     --2 (1:受注、2:移動)
                                   ,@MoveNO AS Number
                                   ,@tblMoveRows AS NumberRows
                                   ,@StockNO
                                   ,@FromSoukoCD
                                   ,tbl.JanCD
                                   ,tbl.SKUCD
                                   ,tbl.AdminNO
                                   ,0 AS ReserveSu --明細入荷数
                                   ,NULL    --ShippingPossibleDate
                                   ,0       --ShippingPossibleSU
                                   ,NULL    --ShippingOrderNO
                                   ,0       --ShippingOrderRows
                                   ,NULL    --CompletedPickingNO
                                   ,0       --CompletedPickingRow
                                   ,NULL    --CompletedPickingDate
                                   ,0       --ShippingSu
                                   ,0       --ReturnKBN
                                   ,NULL    --OriginalReserveNO
                             
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,NULL                  
                                   ,NULL
                             FROM @Table tbl
                             WHERE @tblMoveRows = tbl.MoveRows
                            ;

                            SET @WIdoSu = @WIdoSu - @WUpdSu;
                            
                            IF @WIdoSu = 0
                            BEGIN
                                --次の明細レコードへ
                                BREAK;
                            END
                            
                            --次の行のデータを取得して変数へ値をセット
                            FETCH NEXT FROM CUR_Stock
                            INTO @StockSu, @AllowableSu, @StockNO;
                        END     --LOOPの終わり***************************************CUR_Stock
                        
                        --カーソルを閉じる
                        CLOSE CUR_Stock;
                        DEALLOCATE CUR_Stock;
                                                                
                        --次の行のデータを取得して変数へ値をセット
                        FETCH NEXT FROM CUR_TABLE
                        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
                    END            --LOOPの終わり
                END		--削除行以外
                
                --カーソルを閉じる
                CLOSE CUR_TABLE;
                --DEALLOCATE CUR_TABLE;
        	END        	
        END
        
    	--赤データ作成
        ELSE --移動区分<>店舗間移動
        BEGIN
            --【D_Stock】在庫　移動元　テーブル転送仕様Ｇ②(Ｈ②のデータが含まれないように考慮)
            UPDATE [D_Stock] SET
                   [StockSu] = [StockSu] + tbl.OldMoveSu
                  ,[AllowableSu] = [AllowableSu] + tbl.OldMoveSu
                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + tbl.OldMoveSu
                  --,[ReturnSu]           = [ReturnSu] + tbl.OldMoveSu	2020.09.18 del
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                 FROM D_Stock AS DS
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 11 
                                          WHEN @KBN_SYOCD THEN 31		--15
                                          WHEN @KBN_CHOSEI_ADD THEN 19
                                          WHEN @KBN_CHOSEI_DEL THEN 20
                                          WHEN @KBN_LOCATION THEN 22
                                          WHEN @KBN_HENPIN THEN 16
                                        ELSE 0 END)   --WarehousingKBN
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.StockNO = DS.StockNO
                  AND (@MovePurposeType IN (@KBN_TENPONAI, @KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
                  	OR (@MovePurposeType IN (@KBN_SYOCD, @KBN_LOCATION, @KBN_HENPIN) AND DW.Quantity < 0)	--Gのデータのみ
                  	)
                  
                  INNER JOIN @Table tbl 
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	行削除データも赤データは作成しなければならない
                 WHERE DS.DeleteDateTime IS NULL
                  ;

            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)	--, @KBN_HENPIN
            BEGIN
                --【D_Stock】在庫　移動先　テーブル転送仕様Ｈ②赤
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] - tbl.OldMoveSu
                      ,[AllowableSu] = [AllowableSu] - tbl.OldMoveSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - tbl.OldMoveSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Stock AS DS
                 INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13
                                                                WHEN @KBN_SYOCD THEN 32		--15
                                                                WHEN @KBN_LOCATION THEN 22
                                                                WHEN @KBN_HENPIN THEN 26 --16 2020/10/01 Fukuda
                                                                ELSE 0 END)
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.StockNO = DS.StockNO
                  AND (@MovePurposeType IN (@KBN_TENPONAI)
                  	OR (@MovePurposeType IN (@KBN_SYOCD, @KBN_LOCATION, @KBN_HENPIN) AND DW.Quantity > 0)	--Hのデータのみ
                  	)
                  	
                 INNER JOIN @Table tbl
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	行削除データも赤データは作成しなければならない
                 WHERE DS.DeleteDateTime IS NULL
                ;
            END
            
           
            --移動区分=返品
            IF @MovePurposeType = @KBN_HENPIN
            BEGIN
             /*返品の赤もG②　2020/02/18
            	--【D_Stock】在庫　移動先　テーブル転送仕様Ｈ④返品
                --伝票番号採番●ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in伝票種別 21
                    @MoveDate, --in基準日
                    @StoreCD,  --in店舗CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --【D_Stock】Insert    
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
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,@ToSoukoCD
                       ,tbl.ToRackNO   --RackNO
                       ,(CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                            ELSE tbl.ArrivalPlanNO END)    --ArrivalPlanNO
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,1   --  ArrivalYetFLG(0:入荷済、1:未入荷)
                       ,3   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                       ,NULL    --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,0  --StockSu
                       ,0   --PlanSu
                       ,0   --AllowableSu
                       ,0   --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL    --OriginalStockNO
                       ,tbl.ExpectReturnDate    --ExpectReturnDate
                       ,tbl.MoveSu  --ReturnPlanSu
                       ,tbl.VendorCD
                       ,NULL    --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;  
*/
                --【D_DeliveryPlan】配送予定情報  Table転送仕様J②
                UPDATE [D_DeliveryPlan] SET
                       [Number] = NULL
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                 WHERE @MoveNO = D_DeliveryPlan.Number
                ;
                
                --【D_DeliveryPlanDetails】配送予定情報　Table転送仕様K②
                UPDATE [D_DeliveryPlanDetails] SET
                       [Number] = NULL
                      ,[NumberRows] = 0
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlanDetails.Number
                 AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
                ;
            END
        END
    END
    
    --修正時の黒　Ｇ、Ｈ、Ｊ、Ｋ***************************************************************修正時
    IF @OperateMode = 2
    BEGIN
        IF @MovePurposeType <> @KBN_TENPOKAN  --移動区分<>店舗間移動
        BEGIN
            --移動区分=返品
            IF @MovePurposeType = @KBN_HENPIN
            BEGIN
                --【D_DeliveryPlan】配送予定情報　テーブル転送仕様J
                --伝票番号採番
                EXEC Fnc_GetNumber
                    19,        --in伝票種別 19
                    @MoveDate, --in基準日
                    @StoreCD,       --in店舗CD
                    @Operator,
                    @DeliveryPlanNO OUTPUT
                    ;
                
                IF ISNULL(@DeliveryPlanNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                INSERT INTO [D_DeliveryPlan]
                       ([DeliveryPlanNO]
                       ,[DeliveryKBN]
                       ,[Number]
                       ,[DeliveryName]
                       ,[DeliverySoukoCD]
                       ,[DeliveryZip1CD]
                       ,[DeliveryZip2CD]
                       ,[DeliveryAddress1]
                       ,[DeliveryAddress2]
                       ,[DeliveryMailAddress]
                       ,[DeliveryTelphoneNO]
                       ,[DeliveryFaxNO]
                       ,[DecidedDeliveryDate]
                       ,[DecidedDeliveryTime]
                       ,[CarrierCD]
                       ,[PaymentMethodCD]
                       ,[CommentInStore]
                       ,[CommentOutStore]
                       ,[InvoiceNO]
                       ,[DeliveryPlanDate]
                       ,[HikiateFLG]
                       ,[IncludeFLG]
                       ,[OntheDayFLG]
                       ,[ExpressFLG]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                SELECT
                        @DeliveryPlanNO
                       ,2 AS DeliveryKBN    --(1:販売、2:倉庫移動)
                       ,@MoveNO AS Number
                       ,NULL    --[DeliveryName]
                       ,@FromSoukoCD    --[DeliverySoukoCD]
                       ,NULL    --[DeliveryZip1CD]
                       ,NULL    --[DeliveryZip2CD]
                       ,NULL    --[DeliveryAddress1]
                       ,NULL    --[DeliveryAddress2]
                       ,NULL    --[DeliveryMailAddress]
                       ,NULL    --[DeliveryTelphoneNO]
                       ,NULL    --[DeliveryFaxNO]
                       ,NULL    --[DecidedDeliveryDate]
                       ,NULL    --[DecidedDeliveryTime]
                       ,NULL    --[CarrierCD]
                       ,NULL    --[PaymentMethodCD]
                       ,NULL    --[CommentInStore]
                       ,NULL    --[CommentOutStore]
                       ,NULL    --[InvoiceNO]
                       ,NULL    --[DeliveryPlanDate]
                       ,0   --[HikiateFLG]
                       ,0   --[IncludeFLG]
                       ,0   --[OntheDayFLG]
                       ,0   --[ExpressFLG]
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                  ;
                  
                --【D_DeliveryPlanDetails】配送予定明細　テーブル転送仕様K
                INSERT INTO [D_DeliveryPlanDetails]
                   ([DeliveryPlanNO]
                   ,[DeliveryPlanRows]
                   ,[Number]
                   ,[NumberRows]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[HikiateFLG]
                   ,[UpdateCancelKBN]
                   ,[DeliveryOrderComIn]
                   ,[DeliveryOrderComOut]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                 SELECT  
                    @DeliveryPlanNO
                   ,tbl.MoveRows AS DeliveryPlanRows
                   ,@MoveNO AS Number
                   ,tbl.MoveRows  As NumberRows
                   ,NULL    --CommentInStore]
                   ,NULL    --CommentOutStore]
                   ,0   --HikiateFLG]
                   ,0   --UpdateCancelKBN]
                   ,NULL    --DeliveryOrderComIn]
                   ,NULL    --DeliveryOrderComOut]                        
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                  FROM @Table tbl
                  WHERE tbl.UpdateFlg <> 2
                  ;
            END

            --明細数分Insert★
            --カーソルオープン
            OPEN CUR_TABLE;

            --最初の1行目を取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
            INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
            
            --データの行数分ループ処理を実行する
            WHILE @@FETCH_STATUS = 0
            BEGIN
            -- ========= ループ内の実際の処理 ここから===
                IF @tblUpdateFlg <> 2    --削除行以外
                BEGIN
                    IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)   --店舗間移動も省く（上で処理済み）
                    BEGIN
                        SET @ToStockNO = '';
                        --【D_Stock】在庫　移動先　テーブル転送仕様Ｈ

                        --伝票番号採番●StockNO
                        EXEC Fnc_GetNumber
                            21,        --in伝票種別 21
                            @MoveDate, --in基準日
                            @StoreCD,       --in店舗CD
                            @Operator,
                            @ToStockNO OUTPUT
                            ;
                            
                        IF ISNULL(@ToStockNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                            
                        --【D_Stock】Insert    
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
                               ,[ReturnPlanSu]
                               ,[VendorCD]
                               ,[ReturnDate]
                               ,[ReturnSu]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                         SELECT
                                @ToStockNO      --●
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN @FromSoukoCD
                                                       ELSE ISNULL(@ToSoukoCD,@FromSoukoCD) END)   --SoukoCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN NULL
                                                       WHEN @KBN_SYOCD THEN tbl.FromRackNO 
                                                       ELSE tbl.ToRackNO END)    --RackNO
                               ,NULL    --ArrivalPlanNO
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 1   --店舗間移動の場合
                                                       ELSE 0 END)                 --  ArrivalYetFLG(0:入荷済、1:未入荷)
                               ,3   --ArrivalPlanKBN(1:受発注分、2:発注分、3:移動分)
                               ,NULL    --ArrivalPlanDate
                               ,NULL    --ArrivalDate
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 0
                                                       WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)   --StockSu
                               ,0  --PlanSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)    --AllowableSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)    --AnotherStoreAllowableSu
                               ,0    --ReserveSu
                               ,0   --InstructionSu
                               ,0   --ShippingSu
                               ,NULL    --OriginalStockNO
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.ExpectReturnDate
                                                       ELSE NULL END)    --ExpectReturnDate
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.MoveSu
                                                       ELSE 0 END)     --ReturnPlanSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.VendorCD
                                                       ELSE NULL END) --[VendorCD]
                               ,NULL    --ReturnDate
                               ,0   --ReturnSu
                         
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;
                    END

                    SET @WIdoSu = @tblMoveSu;
                    --【D_Stock】在庫　移動元　テーブル転送仕様Ｇ
                    --移動数を在庫で引当                    --カーソル定義(D_Stock_SelectSuryo参照)
                    DECLARE CUR_Stock CURSOR FOR
                        SELECT DS.StockSu
                            ,DS.AllowableSu
                            ,DS.StockNO
                        from D_Stock DS
                        WHERE DS.SoukoCD = @FromSoukoCD
                        AND DS.RackNO = @tblFromRackNO
                        AND DS.AdminNO = @tblAdminNO
                        AND DS.DeleteDateTime is null 
                        AND DS.AllowableSu > 0 
                        AND DS.ArrivalYetFlg = 0 
                        ;
                        
                    --カーソルオープン
                    OPEN CUR_Stock;

                    --最初の1行目を取得して変数へ値をセット
                    FETCH NEXT FROM CUR_Stock
                    INTO @StockSu, @AllowableSu, @StockNO;
                    
                    --データの行数分ループ処理を実行する
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                    -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock
                        IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                        BEGIN
                            SET @WUpdSu = @WIdoSu;
                        END
                        ELSE IF @StockSu < @AllowableSu
                        BEGIN
                            SET @WUpdSu = @StockSu;
                        END
                        ELSE
                        BEGIN
                            SET @WUpdSu = @AllowableSu;
                        END
                        
                        UPDATE [D_Stock] SET
                               [StockSu] = [StockSu] - @WUpdSu
                              ,[AllowableSu] = [AllowableSu] - @WUpdSu
                              ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                              --,[ExpectReturnDate]   = tbl.ExpectReturnDate    2020.09.18 del
                              --,[VendorCD]           = tbl.VendorCD
                              --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                              ,[UpdateOperator]     =  @Operator  
                              ,[UpdateDateTime]     =  @SYSDATETIME
                              
                         FROM D_Stock AS DS
                         INNER JOIN @Table tbl
                         ON tbl.MoveRows = @tblMoveRows
                         WHERE DS.DeleteDateTime IS NULL
                         AND DS.StockNO = @StockNO
                        ;
                        
                        --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｃ   @KBN_CHOSEI_ADD, @KBN_CHOSEI_DELのときのStockNOは？
                        --C(11),C(15),C(19),C(20),C(22),C(16)
                        INSERT INTO [D_Warehousing]
                           ([WarehousingDate]
                           ,[SoukoCD]
                           ,[RackNO]
                           ,[StockNO]
                           ,[JanCD]
                           ,[AdminNO]
                           ,[SKUCD]
                           ,[WarehousingKBN]
                           ,[DeleteFlg]
                           ,[Number]
                           ,[NumberRow]
                           ,[VendorCD]
                           ,[ToStoreCD]
                           ,[ToSoukoCD]
                           ,[ToRackNO]
                           ,[ToStockNO]
                           ,[FromStoreCD]
                           ,[FromSoukoCD]
                           ,[FromRackNO]
                           ,[CustomerCD]
                           ,[Quantity]
                           ,[Program]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                        SELECT @MoveDate --WarehousingDate
                           ,@FromSoukoCD AS SoukoCD
                           ,tbl.FromRackNO    --RackNO
                           ,@StockNO    --(D_Stock)☆(移動元)と同じ値
                           ,tbl.JanCD
                           ,tbl.AdminNO
                           ,tbl.SKUCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                                   WHEN @KBN_SYOCD      THEN 31		--15
                                                   WHEN @KBN_CHOSEI_ADD THEN 19
                                                   WHEN @KBN_CHOSEI_DEL THEN 20
                                                   WHEN @KBN_LOCATION   THEN 22
                                                   WHEN @KBN_HENPIN     THEN 16 --
                                                   ELSE 0 END)   --WarehousingKBN
                           ,0  --DeleteFlg
                           ,@NewMoveNO  --Number
                           ,tbl.MoveRows --NumberRow
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                                   ELSE NULL END)    --VendorCD
                           
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                                   WHEN @KBN_SYOCD      THEN @FromStoreCD
                                                   WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                                   WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                                   WHEN @KBN_LOCATION   THEN @FromStoreCD
                                                   WHEN @KBN_HENPIN     THEN @ToStoreCD		--2020.10.6
                                                   ELSE NULL END)	--ToStoreCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                                   WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                                   WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                                   WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                                   WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                                   WHEN @KBN_HENPIN     THEN @ToSoukoCD		--2020.10.6
                                                   ELSE NULL END)	--ToSoukoCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                                   WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                                   WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                                   WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                                   WHEN @KBN_LOCATION   THEN tbl.ToRackNO   --2020.10.29 chg
                                                   WHEN @KBN_HENPIN     THEN tbl.ToRackNO   --2020.09.18 add
                                                   ELSE NULL END)	--ToRackNO
                           
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO    -- (D_Stock)●(移動先)と同じ値
                                                   WHEN @KBN_SYOCD      THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                                   WHEN @KBN_CHOSEI_ADD THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                                   WHEN @KBN_CHOSEI_DEL THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                                   WHEN @KBN_LOCATION   THEN @StockNO      --(D_Stock)☆(移動元)と同じ値
                                                   WHEN @KBN_HENPIN     THEN @ToStockNO    -- (D_Stock)●(移動先)と同じ値
                                                   ELSE NULL END)  --ToStockNO
                           ,@FromStoreCD	--FromStoreCD
                           ,@FromSoukoCD	--FromSoukoCD
                           ,tbl.FromRackNO	--FromRackNO
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                                   ELSE NULL END)    --CustomerCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_CHOSEI_ADD THEN @WUpdSu          --移動数(プラス値とする)
                                                   WHEN @KBN_CHOSEI_DEL THEN @WUpdSu          --移動数(プラス値とする)
                                                   WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                                   ELSE @WUpdSu END) 
                           
                           ,@Program  --Program
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL
                           ,NULL

                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;
                          
                        IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
                        BEGIN
                            --【D_Warehousing】入出庫履歴　テーブル転送仕様Ｄ
                            --D(13),D(15),D(22),D(16)
                            INSERT INTO [D_Warehousing]
                               ([WarehousingDate]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[StockNO]
                               ,[JanCD]
                               ,[AdminNO]
                               ,[SKUCD]
                               ,[WarehousingKBN]
                               ,[DeleteFlg]
                               ,[Number]
                               ,[NumberRow]
                               ,[VendorCD]
                               ,[ToStoreCD]
                               ,[ToSoukoCD]
                               ,[ToRackNO]
                               ,[ToStockNO]
                               ,[FromStoreCD]
                               ,[FromSoukoCD]
                               ,[FromRackNO]
                               ,[CustomerCD]
                               ,[Quantity]
                               ,[Program]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                            SELECT @MoveDate --WarehousingDate
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN ISNULL(@ToSoukoCD,@FromSoukoCD)
                                                       WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                       ELSE NULL END) AS SoukoCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD    THEN tbl.FromRackNO 
                                                       WHEN @KBN_LOCATION THEN tbl.ToRackNO   --2020.10.29 chg 
                                                       WHEN @KBN_HENPIN   THEN tbl.ToRackNO
                                                       ELSE tbl.ToRackNO END)   --RackNO　商品CD付替時のみ移動元棚番
                               ,@ToStockNO  --(D_Stock)●(移動先)と同じ値
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                                       ELSE tbl.JanCD END)  --JanCD
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewAdminNO
                                                       ELSE tbl.AdminNO END)    --AdminNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewSKUCD
                                                       ELSE tbl.SKUCD END)      --SKUCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13 
                                                       WHEN @KBN_SYOCD    THEN 32		--15
                                                       WHEN @KBN_LOCATION THEN 22
                                                       WHEN @KBN_HENPIN   THEN 26 --16 2020/10/01 Fukuda 
                                                       ELSE 0 END)   --WarehousingKBN
                               ,0  --DeleteFlg
                               ,@NewMoveNO  --Number
                               ,tbl.MoveRows --NumberRow
                               ,NULL    --VendorCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToStoreCD
                                                       WHEN @KBN_SYOCD    THEN @FromStoreCD
                                                       WHEN @KBN_LOCATION THEN @FromStoreCD
                                                       WHEN @KBN_HENPIN   THEN @ToStoreCD
                                                       ELSE NULL END)
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToSoukoCD
                                                       WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                       ELSE NULL END)
                                                    
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD    THEN tbl.FromRackNO
                                                       WHEN @KBN_LOCATION THEN tbl.ToRackNO     --2020.10.29 chg
                                                       WHEN @KBN_HENPIN   THEN tbl.ToRackNO     --2020.09.18 add
                                                       ELSE NULL END)
                                                    
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @StockNO    --(D_Stock)☆(移動元)と同じ値
                                                       WHEN @KBN_SYOCD    THEN @ToStockNO  --(D_Stock)●(移動先)と同じ値
                                                       WHEN @KBN_LOCATION THEN @ToStockNO  --(D_Stock)●(移動先)と同じ値
                                                       WHEN @KBN_HENPIN   THEN @StockNO    --(D_Stock)☆(移動元)と同じ値
                                                       ELSE NULL END)
                               ,@FromStoreCD
                               ,@FromSoukoCD
                               ,tbl.FromRackNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.JanCD
                                                       ELSE NULL END)    --CustomerCD
                               ,@WUpdSu  --Quantity
                               ,@Program  --Program
                               
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL
                               ,NULL

                              FROM @Table tbl
                              WHERE tbl.MoveRows = @tblMoveRows
                              ;
                        END                                        

                        SET @WIdoSu = @WIdoSu - @WUpdSu;
                        
                        IF @WIdoSu = 0
                        BEGIN
                            --次の明細レコードへ
                            BREAK;
                        END
                        
                        --次の行のデータを取得して変数へ値をセット
                        FETCH NEXT FROM CUR_Stock
                        INTO @StockSu, @AllowableSu, @StockNO;
                    END     --LOOPの終わり***************************************CUR_Stock
                    
                    --カーソルを閉じる
                    CLOSE CUR_Stock;
                    DEALLOCATE CUR_Stock;
                        
                END     --削除行以外
                
                --次の行のデータを取得して変数へ値をセット
                FETCH NEXT FROM CUR_TABLE
                INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
            END            --LOOPの終わり
            
            --カーソルを閉じる
            CLOSE CUR_TABLE;
            DEALLOCATE CUR_TABLE;
            
        END		--移動区分<>店舗間移動
    END
	
    --処理履歴データへ更新
    SET @KeyItem = ISNULL(@NewMoveNO,@MoveNO);
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutMoveNO = @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


GO


