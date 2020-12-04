/****** Object:  StoredProcedure [dbo].[PRC_SiharaiToroku]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [dbo].[PRC_SiharaiToroku]
GO

/****** Object:  StoredProcedure [dbo].[PRC_SiharaiToroku]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    支払登録
--       Program ID      SiharaiToroku
--       Create date:    2020.06.14
--    ======================================================================
--CREATE TYPE T_Pay AS TABLE
--    (
--    [Rows] [int],
--    [PayNO]  varchar(11),
--    [PayeeCD]  varchar(13),
--    [PayPlanDate]  date,
--    [HontaiGaku8]  money,
--    [HontaiGaku10]  money,
--    [TaxGaku8]  money,
--    [TaxGaku10]  money,
--    [PayGaku]  money,
--    [NotPaidGaku]  money,
--    [TransferGaku]  money,
--    [TransferFeeGaku]  money,
--    [FeeKBN]  tinyint,
--    [MotoKouzaCD]  varchar(3),
--    [BankCD]  varchar(4),
--    [BranchCD]  varchar(3),
--    [KouzaKBN]  tinyint,
--    [KouzaNO]  varchar(7),
--    [KouzaMeigi]  varchar(40),
--    [CashGaku]  money,
--    [BillGaku]  money,
--    [BillDate]  date,
--    [BillNO]  varchar(20),
--    [ERMCGaku]  money,
--    [ERMCDate]  date,
--    [ERMCNO]  varchar(20),
--    [CardGaku]  money,
--    [OffsetGaku]  money,
--    [OtherGaku1]  money,
--    [Account1]  varchar(10),
--    [SubAccount1]  varchar(10),
--    [OtherGaku2]  money,
--    [Account2]  varchar(10),
--    [SubAccount2]  varchar(10),
--    [UpdateFlg][tinyint]		--新規時：0、修正：1、行削除：2
--    )
--GO
--
--CREATE TYPE T_PayDetails AS TABLE
--    (
--    [PayeeCD]  varchar(13),
--    [PayPlanDate]  date,
--    [PayNORows] [int],
--    [PayPlanNO] [int],
--    [PayGaku]  money,
--    [PayConfirmFinishedKBN] [tinyint],
--    [ProcessingKBN] [tinyint],
--    [UpdateFlg][tinyint]		--新規時：0、修正：1、行削除：2
--    )
--GO

CREATE PROCEDURE PRC_SiharaiToroku
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @PayNO      varchar(11),
    @LargePayNO varchar(11),

    @PayDate  varchar(10),
    @StaffCD   varchar(10),

    @Table   T_Pay READONLY,
    @TableD  T_PayDetails READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutPayNO varchar(11) OUTPUT
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
	SET @Program = 'SiharaiToroku';	

    --カーソル定義
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.PayNO,tbl.PayeeCD,tbl.PayPlanDate
              ,tbl.Rows, tbl.UpdateFlg
        FROM @Table AS tbl
        ORDER BY tbl.PayeeCD,tbl.PayPlanDate
        ;
        
    DECLARE @tblPayNO varchar(11);
    DECLARE @tblPayeeCD varchar(13);
    DECLARE @tblPayPlanDate date;
    DECLARE @BreakKey   varchar(35);
    DECLARE @tblRows int;
    DECLARE @tblUpdateFlg int;
    DECLARE @PayNORows int;
    
    DECLARE @StoreCD   varchar(4);
    
    SET @StoreCD = (SELECT top 1 A.StoreCD 
                    FROM M_Store As A 
                    WHERE A.StorePlaceKbn = 1
                    AND A.DeleteFlg = 0
                    AND A.ChangeDate <= CONVERT(date, @SYSDATETIME)
                    ORDER BY A.ChangeDate desc);
    
    SET @BreakKey = '';
    

    --変更・削除　修正前金額を更新（赤データ）--
    IF @OperateMode >= 2
    BEGIN
        --D_MonthlyDebt       Insert/Update   Table転送仕様Ｇ ②
        --Update 該当年月、店舗、支払先
        UPDATE [D_MonthlyDebt]
            SET
             [PayGaku] = [D_MonthlyDebt].PayGaku - tbl.PayGaku
            ,[OffsetGaku] = [D_MonthlyDebt].OffsetGaku - tbl.OffsetGaku
            ,[BalanceGaku] = [D_MonthlyDebt].LastBalanceGaku + [D_MonthlyDebt].DebtGaku - [D_MonthlyDebt].PayGaku + tbl.PayGaku
            ,[UpdateOperator]     =  @Operator  
            ,[UpdateDateTime]     =  @SYSDATETIME
        FROM (SELECT A.PayeeCD, SUBSTRING(CONVERT(varchar, A.PayDate, 112),1,6) As YYYYMM
                ,SUM(A.PayGaku) AS PayGaku 
                ,SUM(A.OffsetGaku) AS OffsetGaku
                
              FROM D_Pay AS A
              WHERE A.PayNO = (CASE WHEN @PayNO <> '' THEN @PayNO ELSE A.PayNO END)
              AND A.LargePayNO = (CASE WHEN @LargePayNO <> '' THEN @LargePayNO ELSE A.LargePayNO END)
              GROUP BY A.PayeeCD, A.PayDate) AS tbl
        WHERE [D_MonthlyDebt].[YYYYMM] = tbl.YYYYMM --CONVERT(int, SUBSTRING(@PayDate,1,4) + SUBSTRING(@PayDate,6,2))
        AND [D_MonthlyDebt].[StoreCD] = @StoreCD
        AND [D_MonthlyDebt].[PayeeCD] = tbl.PayeeCD
        AND [D_MonthlyDebt].[DeleteDateTime] IS NULL     
        ;
	END    
    
    --変更--
    IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';

        --Table転送仕様Ａ
        UPDATE [D_Pay]
           SET [InputDateTime] = @SYSDATETIME
              ,[StaffCD]          = @StaffCD
              ,[PayDate]          = @PayDate
              ,[PayGaku]          = tbl.PayGaku
              ,[NotPaidGaku]      = tbl.NotPaidGaku
              ,[TransferGaku]     = tbl.TransferGaku
              ,[TransferFeeGaku]  = tbl.TransferFeeGaku
              ,[FeeKBN]           = tbl.FeeKBN
              ,[MotoKouzaCD]      = tbl.MotoKouzaCD
              ,[BankCD]           = tbl.BankCD
              ,[BranchCD]         = tbl.BranchCD
              ,[KouzaKBN]         = tbl.KouzaKBN
              ,[KouzaNO]          = tbl.KouzaNO
              ,[KouzaMeigi]       = tbl.KouzaMeigi
              ,[CashGaku]         = 0
              ,[BillGaku]         = tbl.BillGaku
              ,[BillDate]         = tbl.BillDate
              ,[BillNO]           = tbl.BillNO
              ,[ERMCGaku]         = tbl.ERMCGaku
              ,[ERMCDate]         = tbl.ERMCDate
              ,[ERMCNO]           = tbl.ERMCNO
              ,[CardGaku]         = tbl.CardGaku
              ,[OffsetGaku]       = tbl.OffsetGaku
              ,[OtherGaku1]       = tbl.OtherGaku1
              ,[Account1]         = tbl.Account1
              ,[SubAccount1]      = tbl.SubAccount1
              ,[OtherGaku2]       = tbl.OtherGaku2
              ,[Account2]         = tbl.Account2
              ,[SubAccount2]      = tbl.SubAccount2
              ,[UpdateOperator]   = @Operator  
              ,[UpdateDateTime]   = @SYSDATETIME
        FROM  @Table AS tbl
        WHERE D_Pay.PayNO = (CASE WHEN @PayNO <> '' THEN @PayNO ELSE D_Pay.PayNO END)
        AND D_Pay.LargePayNO = (CASE WHEN @LargePayNO <> '' THEN @LargePayNO ELSE D_Pay.LargePayNO END)
        AND D_Pay.PayeeCD = tbl.PayeeCD
        AND D_Pay.PayPlanDate = tbl.PayPlanDate
        ;            
        
        IF ISNULL(@LargePayNO,'') = ''
        BEGIN        
            DELETE FROM D_PayDetails
            WHERE PayNO = @PayNO
            ;
        END
        ELSE
        BEGIN
            DELETE FROM D_PayDetails
            WHERE EXISTS(SELECT 1 FROM D_Pay AS D
                         WHERE D.LargePayNO = @LargePayNO
                         AND D.PayNO = D_PayDetails.PayNO)
            AND DeleteDateTime IS NULL
            ;
        END
        
        SET @PayNORows = 1;
    END
    
    --新規：修正--
    IF @OperateMode <= 2
    BEGIN
        IF @OperateMode = 1
        BEGIN
            SET @OperateModeNm = '新規';

            --伝票番号採番
            EXEC Fnc_GetNumber
                29,             --in伝票種別 29
                @PayDate,      --in基準日
                @StoreCD,      --in店舗CD
                @Operator,
                @LargePayNO OUTPUT
                ;
            
            IF ISNULL(@LargePayNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
        
        END
        
        --明細数分Insert★
        --カーソルオープン
        OPEN CUR_TABLE;

        --最初の1行目を取得して変数へ値をセット
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPayNO, @tblPayeeCD, @tblPayPlanDate, @tblRows, @tblUpdateFlg;
        
        --データの行数分ループ処理を実行する
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ループ内の実際の処理 ここから===

            IF @OperateMode = 1
            BEGIN
                --PayeeCD,StaffCD,PayDateごとに採番
                IF @BreakKey <> @tblPayeeCD + ' ' + CONVERT(varchar,@tblPayPlanDate,111)
                BEGIN
                    
                    --伝票番号採番
                    EXEC Fnc_GetNumber
                        9,             --in伝票種別 9
                        @PayDate,      --in基準日
                        @StoreCD,      --in店舗CD
                        @Operator,
                        @PayNO OUTPUT
                        ;
                    
                    IF ISNULL(@PayNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --D_Pay               Insert          Table転送仕様
                    INSERT INTO [D_Pay]
                       ([PayNO]
                       ,[LargePayNO]
                       ,[PayCloseNO]
                       ,[PayCloseDate]
                       ,[PayeeCD]
                       ,[InputDateTime]
                       ,[StaffCD]
                       ,[PayDate]
                       ,[PayPlanDate]
                       ,[HontaiGaku8]
                       ,[HontaiGaku10]
                       ,[TaxGaku8]
                       ,[TaxGaku10]
                       ,[PayGaku]
                       ,[NotPaidGaku]
                       ,[TransferGaku]
                       ,[TransferFeeGaku]
                       ,[FeeKBN]
                       ,[MotoKouzaCD]
                       ,[BankCD]
                       ,[BranchCD]
                       ,[KouzaKBN]
                       ,[KouzaNO]
                       ,[KouzaMeigi]
                       ,[CashGaku]
                       ,[BillGaku]
                       ,[BillDate]
                       ,[BillNO]
                       ,[ERMCGaku]
                       ,[ERMCDate]
                       ,[ERMCNO]
                       ,[CardGaku]
                       ,[OffsetGaku]
                       ,[OtherGaku1]
                       ,[Account1]
                       ,[SubAccount1]
                       ,[OtherGaku2]
                       ,[Account2]
                       ,[SubAccount2]
                       ,[FBCreateDate]
                       ,[FBCreateNO]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT 
                        @PayNO
                       ,@LargePayNO
                       ,DP.PayCloseNO
                       ,DP.PayCloseDate
                       ,@tblPayeeCD
                       ,@SYSDATETIME    AS InputDateTime
                       ,@StaffCD
                       ,@PayDate
                       ,tbl.PayPlanDate
                       ,ISNULL(DP.HontaiGaku8,0)
                       ,ISNULL(DP.HontaiGaku10,0)
                       ,ISNULL(DP.TaxGaku8,0)
                       ,ISNULL(DP.TaxGaku10,0)
                       ,tbl.PayGaku
                       ,tbl.NotPaidGaku
                       ,tbl.TransferGaku
                       ,tbl.TransferFeeGaku
                       ,tbl.FeeKBN
                       ,tbl.MotoKouzaCD
                       ,tbl.BankCD
                       ,tbl.BranchCD
                       ,tbl.KouzaKBN
                       ,tbl.KouzaNO
                       ,tbl.KouzaMeigi
                       ,tbl.CashGaku
                       ,tbl.BillGaku
                       ,tbl.BillDate
                       ,tbl.BillNO
                       ,tbl.ERMCGaku
                       ,tbl.ERMCDate
                       ,tbl.ERMCNO
                       ,0 AS CardGaku
                       ,tbl.OffsetGaku
                       ,tbl.OtherGaku1
                       ,tbl.Account1
                       ,tbl.SubAccount1
                       ,tbl.OtherGaku2
                       ,tbl.Account2
                       ,tbl.SubAccount2
                       ,NULL AS FBCreateDate
                       ,NULL AS FBCreateNO
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                    FROM @Table AS tbl
                    LEFT OUTER JOIN (SELECT DP.PayeeCD, DP.PayPlanDate
                                           ,MAX(DP.PayCloseNO) AS PayCloseNO
                                           ,MAX(DP.PayCloseDate) AS PayCloseDate
                                           ,SUM(ISNULL(DP.HontaiGaku8,0)) AS HontaiGaku8
                                           ,SUM(ISNULL(DP.HontaiGaku10,0)) AS HontaiGaku10
                                           ,SUM(ISNULL(DP.TaxGaku8,0)) AS TaxGaku8
                                           ,SUM(ISNULL(DP.TaxGaku10,0)) AS TaxGaku10
                                    FROM D_PayPlan AS DP
                                    WHERE DP.DeleteDateTime IS NULL
                                    AND DP.PayConfirmFinishedKBN = 0
                                    GROUP BY DP.PayeeCD, DP.PayPlanDate
                    )AS DP            
                    ON DP.PayeeCD = tbl.PayeeCD
                    AND DP.PayPlanDate = tbl.PayPlanDate
                    WHERE tbl.Rows = @tblRows
                    ;            
                    
                    SET @PayNORows = 1;
                END
            END
            
            --D_PayDetails        Insert          Table転送仕様Ｂ
            INSERT INTO [D_PayDetails]
               ([PayNO]
               ,[PayNORows]
               ,[PayPlanNO]
               ,[PayGaku]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
             SELECT
                ISNULL(@PayNO, @tblPayNO)
               ,tblD.PayNORows
               ,tblD.PayPlanNO
               ,tblD.PayGaku
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
            FROM @TableD AS tblD
            WHERE tblD.PayeeCD = @tblPayeeCD
            AND tblD.PayPlanDate = @tblPayPlanDate
            ;
            
            IF @PayNORows = 1
            BEGIN
                IF ISNULL(@PayNO,'') = ''
                BEGIN
                --L_PayHistory        Insert          Table転送仕様Ｃ         
                --L_PayDetailsHistory Insert          Table転送仕様Ｄ
                    exec dbo.L_PayHistory_Insert @tblPayNO,@LargePayNO;
                END
                ELSE
                BEGIN
                    exec dbo.L_PayHistory_Insert @PayNO,@LargePayNO;
                END
                
                SET @BreakKey = @tblPayeeCD + ' ' + CONVERT(varchar,@tblPayPlanDate,111);
            END
            
            SET @PayNORows = @PayNORows + 1;
            
            --処理履歴データへ更新
            SET @KeyItem = ISNULL(@PayNO, @tblPayNO);
                
            EXEC L_Log_Insert_SP
                @SYSDATETIME,
                @Operator,
                @Program,
                @PC,
                @OperateModeNm,
                @KeyItem;
	            
            --次の行のデータを取得して変数へ値をセット
            FETCH NEXT FROM CUR_TABLE
            INTO @tblPayNO, @tblPayeeCD, @tblPayPlanDate, @tblRows, @tblUpdateFlg;
        END            --LOOPの終わり
        
        --カーソルを閉じる
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
        
        --D_PayPlan           Update          Table転送仕様Ｅ
        UPDATE [D_PayPlan]
           SET 
            [PayConfirmGaku]         = [PayConfirmGaku] + tblD.PayGaku
           ,[PayConfirmFinishedKBN]  = tblD.PayConfirmFinishedKBN   
           ,[UpdateOperator]         =  @Operator  
           ,[UpdateDateTime]         =  @SYSDATETIME
        FROM @TableD AS tblD
        WHERE [D_PayPlan].[PayPlanNO] = tblD.PayPlanNO
        AND [D_PayPlan].[DeleteDateTime] IS NULL           
        ;
                
        --D_PayCloseHistory   Update          Table転送仕様Ｆ
        Update [D_PayCloseHistory]
            SET
            ProcessingKBN = tblD.ProcessingKBN
        FROM @TableD AS tblD
        INNER JOIN D_PayPlan AS DP
        ON DP.PayPlanNO = tblD.PayPlanNO
        WHERE [D_PayCloseHistory].[PayCloseNO] = DP.PayCloseNO
        AND [D_PayCloseHistory].[DeleteDateTime] IS NULL           
        ;

        --D_MonthlyDebt       Insert/Update   Table転送仕様Ｇ 
        --Update 該当年月、店舗、支払先が月別債務に存在しなければInsert
        UPDATE [D_MonthlyDebt]
            SET
             [PayGaku]        = [D_MonthlyDebt].PayGaku + tbl.PayGaku
            ,[OffsetGaku]     = [D_MonthlyDebt].OffsetGaku + tbl.OffsetGaku
            ,[BalanceGaku]    = [D_MonthlyDebt].LastBalanceGaku + [D_MonthlyDebt].DebtGaku - ([D_MonthlyDebt].PayGaku + tbl.PayGaku)
            ,[UpdateOperator] =  @Operator  
            ,[UpdateDateTime] =  @SYSDATETIME
        FROM (SELECT A.PayeeCD, SUM(A.PayGaku) AS PayGaku 
                    ,SUM(A.OffsetGaku) AS OffsetGaku
              FROM @Table AS A
              GROUP BY A.PayeeCD) AS tbl
        WHERE [D_MonthlyDebt].[YYYYMM] = CONVERT(int, SUBSTRING(@PayDate,1,4) + SUBSTRING(@PayDate,6,2))
        AND [D_MonthlyDebt].[StoreCD] = @StoreCD
        AND [D_MonthlyDebt].[PayeeCD] = tbl.PayeeCD
        AND [D_MonthlyDebt].[DeleteDateTime] IS NULL     
        ;
        
        INSERT INTO [D_MonthlyDebt]
           ([YYYYMM]
           ,[StoreCD]
           ,[PayeeCD]
           ,[LastBalanceGaku]
           ,[HontaiGaku0]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[HontaiGaku]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[TaxGaku]
           ,[DebtGaku]
           ,[PayGaku]
           ,[OffsetGaku]
           ,[BalanceGaku]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            CONVERT(int, SUBSTRING(@PayDate,1,4) + SUBSTRING(@PayDate,6,2))	--YYYYMM, int,
           ,@StoreCD
           ,tbl.PayeeCD
           ,0	--LastBalanceGaku, money,>
           ,0	--HontaiGaku0, money,>
           ,0	--HontaiGaku8, money,>
           ,0	--HontaiGaku10, money,>
           ,0	--HontaiGaku, money,>
           ,0	--TaxGaku8, money,>
           ,0	--TaxGaku10, money,>
           ,0	--TaxGaku, money,>
           ,0	--DebtGaku, money,>
           ,tbl.PayGaku		--PayGaku, money,>
           ,tbl.OffsetGaku	--OffsetGaku, money,>
           ,0 - tbl.PayGaku	--BalanceGaku, money,>
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
      FROM (SELECT A.PayeeCD, SUM(A.PayGaku) AS PayGaku 
                  ,SUM(A.OffsetGaku) AS OffsetGaku
            FROM @Table AS A
            GROUP BY A.PayeeCD) AS tbl
      WHERE NOT EXISTS(SELECT 1 FROM D_MonthlyDebt AS D
                       WHERE D.[YYYYMM] = CONVERT(int, SUBSTRING(@PayDate,1,4) + SUBSTRING(@PayDate,6,2))
                       AND D.[StoreCD] = @StoreCD
                       AND D.[PayeeCD] = tbl.PayeeCD
                       )
      ;
        
        
    END
    
    ELSE    --削除
    BEGIN
    
        --テーブル転送仕様Ａ②
        Update D_Pay
        set UpdateOperator= @Operator,
            UpdateDateTime=@SYSDATETIME,
            DeleteOperator=@Operator,
            DeleteDateTime=@SYSDATETIME
        WHERE PayNO = (CASE WHEN @PayNO <> '' THEN @PayNO ELSE PayNO END)
        AND LargePayNO = (CASE WHEN @LargePayNO <> '' THEN @LargePayNO ELSE LargePayNO END)
        
        --テーブル転送仕様Ｂ②
        IF ISNULL(@LargePayNO,'') = ''
        BEGIN
            Update D_PayDetails
            set UpdateOperator=@Operator,
                UpdateDateTime=@SYSDATETIME,
                DeleteOperator=@Operator,
                DeleteDateTime=@SYSDATETIME
            where PayNO = @PayNO
            AND DeleteDateTime IS NULL
            ;
        END
        ELSE
        BEGIN
            Update D_PayDetails
            set UpdateOperator=@Operator,
                UpdateDateTime=@SYSDATETIME,
                DeleteOperator=@Operator,
                DeleteDateTime=@SYSDATETIME
            where EXISTS(SELECT 1 FROM D_Pay AS D
                         WHERE D.LargePayNO = @LargePayNO
                         AND D.PayNO = D_PayDetails.PayNO)
            AND DeleteDateTime IS NULL
            ;
        END 
        ;

        ---sheetC,D
        exec dbo.L_PayHistory_Insert @PayNO,@LargePayNO

    ----SheetE②
        IF ISNULL(@LargePayNO,'') = ''
        BEGIN
            Update D_PayPlan
                set 
                PayConfirmGaku = PayConfirmGaku + (-1) * dpd.PayGaku,
                PayConfirmFinishedKBN = 0,
                UpdateOperator = @Operator,
                UpdateDateTime = @SYSDATETIME
            from D_PayPlan as dpp 
            inner join D_PayDetails as dpd
            on dpd.PayPlanNO = dpp.PayPlanNO
            Where dpd.PayNO = @PayNO
            ;
        END
        ELSE
        BEGIN
            Update D_PayPlan
                set 
                PayConfirmGaku = PayConfirmGaku + (-1) * dpd.PayGaku,
                PayConfirmFinishedKBN = 0,
                UpdateOperator = @Operator,
                UpdateDateTime = @SYSDATETIME
            from D_PayPlan as dpp 
            inner join D_PayDetails dpd
            on dpd.PayPlanNO =  dpp.PayPlanNO
            where EXISTS(SELECT 1 FROM D_Pay AS D
                         WHERE D.LargePayNO = @LargePayNO
                         AND D.PayNO = dpd.PayNO)
            ;
        END
        
    ----SheetF②
        Update D_PayCloseHistory
            Set ProcessingKBN = 1,
            UpdateOperator = @Operator,
            UpdateDateTime = @SYSDATETIME
        From D_PayCloseHistory dpch 
        inner join D_Pay dp 
        on dp.PayCloseNO = dpch.PayCloseNO
        Where dp.PayNO = (CASE WHEN @PayNO <> '' THEN @PayNO ELSE dp.PayNO END)
        AND dp.LargePayNO = (CASE WHEN @LargePayNO <> '' THEN @LargePayNO ELSE LargePayNO END)
        ;

        --処理履歴データへ更新
        SET @KeyItem = ISNULL(@LargePayNO,@PayNO);
            
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            @Program,
            @PC,
            @OperateModeNm,
            @KeyItem;
    END

    SET @OutPayNO = @PayNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO

