BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseSyoriAll]
END TRY

BEGIN CATCH END CATCH 

BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseSyoriAll_Sub]
END TRY

BEGIN CATCH END CATCH 
GO

CREATE PROCEDURE PRC_NayoseSyoriAll_Sub
    (@JuchuuNO   varchar(11),
     @Operator   varchar(10),
     @SYSDATETIME  datetime
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
	--上記Select件数が1件の時、D_Juchuu.保留FLG、名寄せ対象FLGに1をUpdate。
    UPDATE D_Juchuu SET
         [OnHoldFLG]         = 1
        ,[IdentificationFLG] = 1
        ,[UpdateOperator]    = @Operator  
        ,[UpdateDateTime]    = @SYSDATETIME
    WHERE JuchuuNO = @JuchuuNO
    ;
    
    --テーブル転送仕様Ｂに従って受注保留警告(D_JuchuuOnHold)のレコード追加。
    INSERT INTO D_JuchuuOnHold
       ([JuchuuNO]
       ,[OnHoldRows]
       ,[OnHoldCD]
       ,[OccorDateTime]
       ,[DisappeareDateTime]
       ,[Remarks]
       ,[StaffCD]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime])
    SELECT
       @JuchuuNO
       ,ISNULL((SELECT MAX(D.OnHoldRows) FROM D_JuchuuOnHold As D WHERE D.JuchuuNO = @JuchuuNO),0)+1 AS OnHoldRows
       ,'001' AS OnHoldCD
       ,@SYSDATETIME AS OccorDateTime
       ,NULL AS DisappeareDateTime
       ,NULL AS Remarks
       ,@Operator AS StaffCD
       ,@Operator AS InsertOperator
       ,@SYSDATETIME AS InsertDateTime
       ,@Operator AS UpdateOperator
       ,@SYSDATETIME AS UpdateDateTime
    WHERE NOT EXISTS(SELECT 1 FROM D_JuchuuOnHold AS DJ
                      WHERE DJ.JuchuuNO = @JuchuuNO
                        AND DJ.OnHoldCD = '001')
    ;

    --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。
    UPDATE D_JuchuuStatus SET
       [OnHoldFLG] = 0
      ,[WarningFLG] = 0
      ,[IncludeFLG] = 0
      ,[GiftFLG] = 0
      ,[NoshiFLG] = 0
      ,[SpecFLG] = 0
      ,[NouhinsyoFLG] = 0
      ,[RyousyusyoFLG] = 0
      ,[SeikyuusyoFLG] = 0
      ,[SonotoFLG] = 0
      ,[OrderMailFLG] = 0
      ,[NyuukaYoteiMailFLG] = 0
      ,[SyukkaYoteiMailFLG] = 0
      ,[SyukkaAnnaiMailFLG] = 0
      ,[NyuukinMailFLG] = 0
      ,[FollowupMailFLG] = 0
      ,[Demand1MailFLG] = 0
      ,[Demand2MailFLG] = 0
      ,[Demand3MailFLG] = 0
      ,[Demand4MailFLG] = 0
      ,[UpdateOperator] = @Operator
      ,[UpdateDateTime] = @SYSDATETIME
    WHERE JuchuuNO = @JuchuuNO
    ;
    
    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO D_JuchuuStatus
           ([JuchuuNO]
           ,[OnHoldFLG]
           ,[WarningFLG]
           ,[IncludeFLG]
           ,[GiftFLG]
           ,[NoshiFLG]
           ,[SpecFLG]
           ,[NouhinsyoFLG]
           ,[RyousyusyoFLG]
           ,[SeikyuusyoFLG]
           ,[SonotoFLG]
           ,[OrderMailFLG]
           ,[NyuukaYoteiMailFLG]
           ,[SyukkaYoteiMailFLG]
           ,[SyukkaAnnaiMailFLG]
           ,[NyuukinMailFLG]
           ,[FollowupMailFLG]
           ,[Demand1MailFLG]
           ,[Demand2MailFLG]
           ,[Demand3MailFLG]
           ,[Demand4MailFLG]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        VALUES( 
            @JuchuuNO
           ,0 --AS OnHoldFLG
           ,0 --AS WarningFLG
           ,0 --AS IncludeFLG
           ,0 --AS GiftFLG
           ,0 --AS NoshiFLG
           ,0 --AS SpecFLG
           ,0 --AS NouhinsyoFLG
           ,0 --AS RyousyusyoFLG
           ,0 --AS SeikyuusyoFLG
           ,0 --AS SonotoFLG
           ,0 --AS OrderMailFLG
           ,0 --AS NyuukaYoteiMailFLG
           ,0 --AS SyukkaYoteiMailFLG
           ,0 --AS SyukkaAnnaiMailFLG
           ,0 --AS NyuukinMailFLG
           ,0 --AS FollowupMailFLG
           ,0 --AS Demand1MailFLG
           ,0 --AS Demand2MailFLG
           ,0 --AS Demand3MailFLG
           ,0 --AS Demand4MailFLG
           ,@Operator    --AS InsertOperator
           ,@SYSDATETIME --AS InsertDateTime
           ,@Operator    --AS UpdateOperator
           ,@SYSDATETIME --AS UpdateDateTime
         )
         ;
	END
END

GO


--  ======================================================================
--       Program Call    名寄せ結果登録
--       Program ID      NayoseSyoriAll
--       Create date:    2021.5.31
--    ======================================================================

CREATE PROCEDURE PRC_NayoseSyoriAll
    (@Operator   varchar(10),
    @PC          varchar(30),
    @OutErrNo    varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(20);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 1;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '名寄せ処理';
    
    --名寄せ対象の受注データを抽出し、プログラム内受注ワークへ更新。
    DECLARE CUR_Juchu CURSOR FOR
        SELECT DH.JuchuuNO
              ,DH.CustomerName
              ,DH.CustomerName2
              ,DH.Tel11
              ,DH.Tel12
              ,DH.Tel13
              ,DH.MailAddress
              --,DH.ZipCD1
              --,DH.ZipCD2
              ,DH.Address1
              ,DH.Address2
              
        from D_Juchuu AS DH                         
        WHERE DH.JuchuuKBN = 1	--受注種別区分              
          AND DH.CustomerCD IS NULL
          AND DH.DeleteDateTime IS NULL
            ;
            
	DECLARE @CNT1 int;
	DECLARE @CNT2 int;
	DECLARE @CNT3 int;
	DECLARE @CNT4 int;
    DECLARE @JuchuuNO varchar(11);
    DECLARE @CustomerCD varchar(13);
    DECLARE @CustomerName   varchar(80);
    DECLARE @CustomerName2   varchar(20);
    DECLARE @ZipCD1 varchar(3);
    DECLARE @ZipCD2 varchar(4);
    DECLARE @Address1   varchar(100);
    DECLARE @Address2   varchar(100);
    DECLARE @Tel11  varchar(5);
    DECLARE @Tel12  varchar(4);
    DECLARE @Tel13  varchar(4);
    DECLARE @MailAddress varchar(100);

    --カーソルオープン
    OPEN CUR_Juchu;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,@Tel12,@Tel13,@MailAddress,@Address1,@Address2 ;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock
        --受注ワークを1件リード

        SET @W_ERR = 0;
        
        --名寄せ用の顧客マスタを抽出し、プログラム内顧客ワークへ更新
        
        --顧客ワークと名寄せチェック
        --①名前 ＆ 電話番号 ＆ メールアドレス一致
        SET @CNT1= (SELECT COUNT(FC.CustomerCD)
                    from D_Juchuu AS DH
                   INNER JOIN F_Customer(GETDATE()) FC
                      ON FC.CustomerName = DH.CustomerName
                     AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
                     AND dbo.Fnc_MailAdress(ISNULL(FC.MailAddress,'')) = dbo.Fnc_MailAdress(ISNULL(DH.MailAddress,''))                                      
                     AND FC.StoreKBN = 1
                     AND FC.DeleteFlg = 0
                   WHERE DH.JuchuuNO = @JuchuuNO
                     AND DH.DeleteDateTime IS NULL
                    )
        IF @CNT1 = 1
        BEGIN
            SET @CustomerCD = (SELECT FC.CustomerCD
                                 from D_Juchuu AS DH
                                INNER JOIN F_Customer(GETDATE()) FC
                                   ON FC.CustomerName = DH.CustomerName
                                  AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
                                  AND dbo.Fnc_MailAdress(ISNULL(FC.MailAddress,'')) = dbo.Fnc_MailAdress(ISNULL(DH.MailAddress,''))                                                 
                                  AND FC.StoreKBN = 1
                                  AND FC.DeleteFlg = 0
                                WHERE DH.JuchuuNO = @JuchuuNO
                                  AND DH.DeleteDateTime IS NULL
                                );
            
            --上記Select件数が1件の時、D_Juchuu.顧客CDにM_Customer.顧客CDをUpdate。
            --D_Juchuu.名寄せ対象FLGに0をUpdate。
            UPDATE D_Juchuu SET
                 [CustomerCD]        = @CustomerCD
                ,[IdentificationFLG] = 0
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --テーブル転送仕様Ｂに従って受注保留警告(D_JuchuuOnHold)のレコード変更。(※1）
            Update D_JuchuuOnHold set
                 [DisappeareDateTime] = @SYSDATETIME	--解消日時
                ,[UpdateOperator]     = @Operator  
                ,[UpdateDateTime]     = @SYSDATETIME
             WHERE D_JuchuuOnHold.JuchuuNO = @JuchuuNO
               AND D_JuchuuOnHold.OnHoldCD = '001'
               ;

            --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。(※1)
            --Update(①の※1、⑤の※1の時）
            UPDATE D_JuchuuStatus SET
                 [OnHoldFLG] = 0    --保留有無FLG
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
             WHERE D_JuchuuStatus.JuchuuNO = @JuchuuNO
               ;
            
            --D_Juchuu.保留FLGに0をUpdate。
            UPDATE D_Juchuu SET
                 [OnHoldFLG]         = 0
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
              AND NOT EXISTS(SELECT 1 FROM D_JuchuuOnHold AS D
                                     WHERE D.JuchuuNO = D_Juchuu.JuchuuNO
                                       AND D.OnHoldCD <> '001'
                                       AND D.DisappeareDateTime IS NOT NULL)
            ;
            
            --次の「1.受注ワークを1件リード」へ。
        END
        ELSE IF @CNT1 = 0
        BEGIN
            --上記Select件数が0件の時、②の名寄せチェックへ
            
            --②名前 ＆ 電話番号
            SET @CNT2= (SELECT COUNT(FC.CustomerCD)
                          from D_Juchuu AS DH
                         INNER JOIN F_Customer(GETDATE()) FC
                            ON FC.CustomerName = DH.CustomerName
                           AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
                           AND FC.StoreKBN = 1
                           AND FC.DeleteFlg = 0
                         WHERE DH.JuchuuNO = @JuchuuNO
                           AND DH.DeleteDateTime IS NULL
                        )
            IF @CNT2 >= 1
            BEGIN
                EXEC PRC_NayoseSyoriAll_Sub
                    @JuchuuNO
                    ,@Operator 
                    ,@SYSDATETIME
                    ;

                --次の「1.受注ワークを1件リード」へ。
            END
            ELSE IF @CNT2 = 0
            BEGIN
                --上記Select件数が0件の時、③の名寄せチェックへ
                
                --③名前 ＆ メールアドレス
                SET @CNT3 = (SELECT COUNT(FC.CustomerCD)
                               from D_Juchuu AS DH
                              INNER JOIN F_Customer(GETDATE()) FC
                                 ON FC.CustomerName = DH.CustomerName
                                AND dbo.Fnc_MailAdress(FC.MailAddress) = dbo.Fnc_MailAdress(DH.MailAddress) 
                                AND FC.StoreKBN = 1
                                AND FC.DeleteFlg = 0
                              WHERE DH.JuchuuNO = @JuchuuNO
                                AND DH.DeleteDateTime IS NULL
                            )
                IF @CNT3 >= 1
                BEGIN
                    --上記Select件数が1件の時、
                    EXEC PRC_NayoseSyoriAll_Sub
                        @JuchuuNO
                        ,@Operator 
                        ,@SYSDATETIME
                        ;
                    
                    --次の「1.受注ワークを1件リード」へ。
                END
                ELSE IF @CNT3 = 0
                BEGIN
                    --④の名寄せチェックへ
                    --④名前 ＆ 住所１  
                    SET @CNT4= (SELECT COUNT(FC.CustomerCD)
                                  from D_Juchuu AS DH
                                 INNER JOIN F_Customer(GETDATE()) FC
                                    ON FC.CustomerName = DH.CustomerName
                                   AND dbo.Fnc_AdressSpacePullOut(dbo.Fnc_AdressHalfToFull(FC.Address1)) = dbo.Fnc_AdressSpacePullOut(dbo.Fnc_AdressHalfToFull(DH.Address1))
                                   AND FC.StoreKBN = 1
                                   AND FC.DeleteFlg = 0
                                 WHERE DH.JuchuuNO = @JuchuuNO
                                   AND DH.DeleteDateTime IS NULL
                                )
                    IF @CNT4 >= 1
                    BEGIN
                        --上記Select件数が1件の時、
                        EXEC PRC_NayoseSyoriAll_Sub
                            @JuchuuNO
                            ,@Operator 
                            ,@SYSDATETIME
                            ;
                        
                        --次の「1.受注ワークを1件リード」へ。
                    END
                    ELSE IF @CNT4 = 0
                    BEGIN
                        --上記Select件数が0件の時、
                        --⑤の顧客マスタ登録へ
                        --テーブル転送仕様Ａに従って顧客マスタのレコード追加。
                        SET @CustomerCD =(SELECT CustomerCount+1 FROM M_CustomerCounter WHERE MainKEY = 1);

                        UPDATE M_CustomerCounter SET
                        	[CustomerCount] = [CustomerCount] +1
                        WHERE MainKEY = 1
                        ;
                        
                        INSERT INTO [M_Customer]
                                   ([CustomerCD]
                                   ,[ChangeDate]
                                   ,[VariousFLG]
                                   ,[CustomerName]
                                   ,[LastName]
                                   ,[FirstName]
                                   ,[LongName1]
                                   ,[LongName2]
                                   ,[KanaName]
                                   ,[StoreKBN]
                                   ,[CustomerKBN]
                                   ,[StoreTankaKBN]
                                   ,[AliasKBN]
                                   ,[BillingType]
                                   ,[GroupName]
                                   ,[BillingFLG]
                                   ,[CollectFLG]
                                   ,[BillingCD]
                                   ,[CollectCD]
                                   ,[BirthDate]
                                   ,[Sex]
                                   ,[Tel11]
                                   ,[Tel12]
                                   ,[Tel13]
                                   ,[Tel21]
                                   ,[Tel22]
                                   ,[Tel23]
                                   ,[ZipCD1]
                                   ,[ZipCD2]
                                   ,[Address1]
                                   ,[Address2]
                                   ,[MailAddress]
                                   ,[TankaCD]
                                   ,[PointFLG]
                                   ,[LastPoint]
                                   ,[WaitingPoint]
                                   ,[TotalPoint]
                                   ,[TotalPurchase]
                                   ,[UnpaidAmount]
                                   ,[UnpaidCount]
                                   ,[LastSalesDate]
                                   ,[LastSalesStoreCD]
                                   ,[MainStoreCD]
                                   ,[StaffCD]
                                   ,[AttentionFLG]
                                   ,[ConfirmFLG]
                                   ,[ConfirmComment]
                                   ,[BillingCloseDate]
                                   ,[CollectPlanMonth]
                                   ,[CollectPlanDate]
                                   ,[HolidayKBN]
                                   ,[TaxTiming]
                                   ,[TaxPrintKBN]
                                   ,[TaxFractionKBN]
                                   ,[AmountFractionKBN]
                                   ,[CreditLevel]
                                   ,[CreditCard]
                                   ,[CreditInsurance]
                                   ,[CreditDeposit]
                                   ,[CreditETC]
                                   ,[CreditAmount]
                                   ,[CreditAdditionAmount]
                                   ,[CreditCheckKBN]
                                   ,[CreditMessage]
                                   ,[FareLevel]
                                   ,[Fare]
                                   ,[PaymentMethodCD]
                                   ,[KouzaCD]
                                   ,[DisplayOrder]
                                   ,[PaymentUnit]
                                   ,[NoInvoiceFlg]
                                   ,[CountryKBN]
                                   ,[CountryName]
                                   ,[RegisteredNumber]
                                   ,[DMFlg]
                                   ,[RemarksOutStore]
                                   ,[RemarksInStore]
                                   ,[AnalyzeCD1]
                                   ,[AnalyzeCD2]
                                   ,[AnalyzeCD3]
                                   ,[DeleteFlg]
                                   ,[UsedFlg]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime])
                             SELECT
                                    @CustomerCD
                                   ,GETDATE() --ChangeDate
                                   ,0 --VariousFLG, tinyint,>
                                   ,DH.CustomerName
                                   ,NULL--<LastName, varchar(20),>
                                   ,NULL--<FirstName, varchar(20),>
                                   ,NULL--<LongName1, varchar(50),>
                                   ,NULL--<LongName2, varchar(50),>
                                   ,DH.CustomerKanaName
                                   ,1	--StoreKBN, tinyint,>
                                   ,0	--CustomerKBN, tinyint,>
                                   ,1	--StoreTankaKBN, tinyint,>
                                   ,1	--AliasKBN, tinyint,>
                                   ,1	--BillingType, tinyint,>
                                   ,NULL	--<GroupName, varchar(40),>
                                   ,1	--BillingFLG, tinyint,>
                                   ,1	--CollectFLG, tinyint,>
                                   ,@CustomerCD	--BillingCD, varchar(13),>
                                   ,@CustomerCD	--CollectCD, varchar(13),>
                                   ,NULL	--<BirthDate, date,>
                                   ,0	--Sex, tinyint,>
                                   ,DH.Tel11
                                   ,DH.Tel12
                                   ,DH.Tel13
                                   ,DH.Tel21
                                   ,DH.Tel22
                                   ,DH.Tel23
                                   ,DH.ZipCD1
                                   ,DH.ZipCD2
                                   ,DH.Address1
                                   ,DH.Address2
                                   ,DH.MailAddress
                                   ,'0000000000000'		--TankaCD, varchar(13),>
                                   ,0	--PointFLG, tinyint,>
                                   ,0	--LastPoint, money,>
                                   ,0	--WaitingPoint, money,>
                                   ,0	--TotalPoint, money,>
                                   ,0	--TotalPurchase, money,>
                                   ,0	--UnpaidAmount, money,>
                                   ,0	--UnpaidCount, money,>
                                   ,NULL	--LastSalesDate, date,>
                                   ,NULL	--LastSalesStoreCD, varchar(4),>
                                   ,NULL	--MainStoreCD, varchar(4),>
                                   ,NULL	--StaffCD, varchar(10),>
                                   ,0	--AttentionFLG, tinyint,>
                                   ,0	--ConfirmFLG, tinyint,>
                                   ,NULL	--ConfirmComment, varchar(50),>
                                   ,0	--BillingCloseDate, tinyint,>
                                   ,0	--CollectPlanMonth, tinyint,>
                                   ,0	--CollectPlanDate, tinyint,>
                                   ,0	--HolidayKBN, tinyint,>
                                   ,0	--TaxTiming, tinyint,>
                                   ,0	--TaxPrintKBN, tinyint,>
                                   ,0	--TaxFractionKBN, tinyint,>
                                   ,0	--AmountFractionKBN, tinyint,>
                                   ,0	--CreditLevel, tinyint,>
                                   ,0	--CreditCard, money,>
                                   ,0	--CreditInsurance, money,>
                                   ,0	--CreditDeposit, money,>
                                   ,0	--CreditETC, money,>
                                   ,0	--CreditAmount, money,>
                                   ,0	--CreditAdditionAmount, money,>
                                   ,0	--CreditCheckKBN, tinyint,>
                                   ,NULL	--CreditMessage, varchar(100),>
                                   ,0	--FareLevel, money,>
                                   ,0	--Fare, money,>
                                   ,NULL	--PaymentMethodCD, varchar(3),>
                                   ,NULL	--KouzaCD, varchar(3),>
                                   ,0	--DisplayOrder, int,>
                                   ,0	--PaymentUnit, tinyint,>
                                   ,0	--NoInvoiceFlg, tinyint,>
                                   ,0	--CountryKBN, tinyint,>
                                   ,NULL	--CountryName, varchar(30),>
                                   ,NULL	--RegisteredNumber, varchar(15),>
                                   ,0	--DMFlg, tinyint,>
                                   ,NULL	--RemarksOutStore, varchar(500),>
                                   ,NULL	--RemarksInStore, varchar(500),>
                                   ,NULL	--AnalyzeCD1, varchar(10),>
                                   ,NULL	--AnalyzeCD2, varchar(10),>
                                   ,NULL	--AnalyzeCD3, varchar(10),>
                                   ,0	--DeleteFlg, tinyint,>
                                   ,1	--UsedFlg, tinyint,>
                                   ,@Operator AS InsertOperator
                                   ,@SYSDATETIME AS InsertDateTime
                                   ,@Operator AS UpdateOperator
                                   ,@SYSDATETIME AS UpdateDateTime
                                  from D_Juchuu AS DH
                                 WHERE DH.JuchuuNO = @JuchuuNO
                                   AND DH.DeleteDateTime IS NULL
                                   ;

                        
                        --採番した顧客CDをD_Juchuu.顧客CDにUpdate。
                        UPDATE D_Juchuu SET
                             [CustomerCD]        = @CustomerCD
                            ,[UpdateOperator]    = @Operator  
                            ,[UpdateDateTime]    = @SYSDATETIME
                        WHERE JuchuuNO = @JuchuuNO
                        ;

                        --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。(※1)
                        --Update(①の※1、⑤の※1の時）
                        UPDATE D_JuchuuStatus SET
                             [OnHoldFLG] = 0    --保留有無FLG
                            ,[UpdateOperator]    = @Operator  
                            ,[UpdateDateTime]    = @SYSDATETIME
                         WHERE D_JuchuuStatus.JuchuuNO = @JuchuuNO
                           ;
                    END
                END             
            END
        END
        ELSE
        BEGIN
            --上記Select件数が複数件の時、
            EXEC PRC_NayoseSyoriAll_Sub
                @JuchuuNO
                ,@Operator 
                ,@SYSDATETIME
                ;
            
            --次の「1.受注ワークを1件リード」へ。
		END
		
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,@Tel12,@Tel13,@MailAddress,@Address1,@Address2 ;

    END     --LOOPの終わり***************************************CUR_Juchu
    
    --カーソルを閉じる
    CLOSE CUR_Juchu;
    DEALLOCATE CUR_Juchu;

    IF @W_ERR = 1
    BEGIN
        SET @OutErrNo = 'S013';
        return @W_ERR;
    END

    --処理履歴データへ更新
    SET @KeyItem = NULL;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NayoseSyoriAll',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutErrNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
