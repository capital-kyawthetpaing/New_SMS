BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_JuchuuDataCheck]
END TRY

BEGIN CATCH END CATCH 

BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_JuchuuDataCheck_Sub]
END TRY

BEGIN CATCH END CATCH 
GO

CREATE PROCEDURE PRC_JuchuuDataCheck_Sub
    (@JuchuuNO   varchar(11),
     @OnHoldCD   varchar(3),
     @Operator   varchar(10),
     @SYSDATETIME  datetime,
     @WRK_HoryuFLG tinyint OUTPUT 
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN

    --テーブル転送仕様Ａに従って受注保留警告(D_JuchuuOnHold)のレコード追加。
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
       ,@OnHoldCD
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
                        AND DJ.OnHoldCD = @OnHoldCD)
    ;

    IF @@ROWCOUNT = 0
    BEGIN
        SET @WRK_HoryuFLG = 0
    END
    ELSE
    BEGIN
        SET @WRK_HoryuFLG = 1
    END

END

GO

--  ======================================================================
--       Program Call    受注データチェック処理
--       Program ID      JuchuuDataCheck
--       Create date:    2021.6.7
--    ======================================================================

CREATE PROCEDURE PRC_JuchuuDataCheck
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
    DECLARE @OperateModeNm varchar(30);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 1;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '受注データチェック処理';
    
    --名寄せ対象の受注データを抽出し、プログラム内受注ワークへ更新。
    DECLARE CUR_Juchu CURSOR FOR
        SELECT DH.JuchuuNO      --D受注.受注番号、          
              --,DM.JuchuuRows    --D受注明細.明細連番            
              ,(SELECT top 1 A.AttentionFLG
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS AttentionFLG        --M顧客.債権要注意顧客、            
              ,(SELECT top 1 A.ConfirmFLG
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS ConfirmFLG      --M顧客.取引要確認顧客、            
              ,DM.DirectFLG     --D受注明細.直送FLG、           
              ,DH.Address1      --D受注.住所１、            
              ,DH.Address2      --D受注.住所２、
              ,(SELECT COUNT(D.address2)
                  from d_juchuu AS D
                 where D.Address2  LIKE '%[0-9]%'
                   AND D.JuchuuNO = D.JuchuuNO) AS CHK_Address2
              ,DH.MailAddress          
              ,MZ.Address1 AS M_Address1        --M郵便番号変換.住所１ as 住所１_Ｍ、           
              ,MZ.Address2 AS M_Address2        --M郵便番号変換.住所２ as 住所２_Ｍ、           
              ,DH.JuchuuGaku        --D受注.受注総額、          
              ,(SELECT top 1 A.UnpaidAmount
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS UnpaidAmount        --M顧客.未入金額、          
              ,(SELECT top 1 A.UnpaidCount
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS UnpaidCount     --M顧客.未入金件数、            
              ,(SELECT top 1 MM3.Num1 --ConfirmCD
                  FROM M_SKU AS M 
                 INNER JOIN M_MultiPorpose AS MM3
                    ON MM3.ID = 313 AND MM3.[Key] = M.ConfirmCD
                 WHERE M.ChangeDate <= DH.JuchuuDate 
                   AND M.AdminNO = DM.AdminNO
                   AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS ConfirmKBN   --MＳＫＵ.要確認品区分、            
              ,DH.CardProgressKBN       --D受注.カード状況、            
              ,MC.SpecialFlg        --M区分変換.設定値          
              --,DH.PaymentMethodCD       --D受注.予定入金方法、          
              ,DH.PaymentProgressKBN        --D受注.入金状況区分、          
              ,DH.CommentCustomer AS H_CommentCustomer      --D受注.受注コメント顧客 as 受注コメント顧客_Ｈ、           
              ,DH.CommentCapital AS H_CommentCapital        --D受注.受注コメントキャピタル as 受注コメントキャピタル_Ｈ、           
              ,DM.CommentCustomer       --D受注明細.受注コメント顧客、          
              ,DM.CommentCapital        --D受注明細.受注コメントキャピタル、            
              ,DS.IncludeFLG            --D受注ステータス.同梱有無FLG、         
              ,DS.SpecFLG               --D受注ステータス.スペック計測有無FLG、         
              ,DS.NoshiFLG              --D受注ステータス.のし対象FLG、         
              ,DS.GiftFLG               --D受注ステータス.ギフト対象FLG、           
              ,DH.CustomerName          --D受注.顧客名、            
              ,DH.DeliveryName          --D受注.配送先名、          
              ,DH.KaolaetcFLG           --D受注.コアラ等FLG、           
              ,MM1.Char1 AS Zip         --M汎用_1.文字型１、            
              ,DH.JuchuuDate AS JuchuuDate      --D受注.受注日          
              ,DM.AnswerFLG         --D受注明細.回答納期登録あり、          
              ,DM.ArrivePlanDate AS ArrivePlanDate      --D受注明細.入荷予定日          
              ,(SELECT MM2.Num1 FROM M_MultiPorpose AS MM2  --※回答納期先日付判断用のM汎用
                 WHERE MM2.ID = 231 AND MM2.[Key] = 1) AS Nissu --M汎用_2.数字型1、         
              
        from D_Juchuu AS DH
       INNER JOIN D_JuchuuDetails AS DM
          ON DM.JuchuuNO = DH.JuchuuNO
         AND DM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuStatus AS DS
          ON DS.JuchuuNO = DH.JuchuuNO
        LEFT OUTER JOIN M_ZipCode AS MZ
          ON MZ.ZipCD1 = DH.ZipCD1
         AND MZ.ZipCD2 = DH.ZipCD2
        LEFT OUTER JOIN M_Conversion AS MC
          ON MC.KBNCD = '101'
         AND MC.SiteKBN = DH.SiteKBN
        LEFT OUTER JOIN M_MultiPorpose AS MM1   --※沖縄判断用のM汎用
          ON MM1.ID = 230 AND MM1.[Key] = DH.ZipCD1    

       WHERE DH.JuchuuKBN = 1  --受注種別区分              
         AND DH.CustomerCD IS NOT NULL
         AND DM.DeliverySu <> DM.JuchuuSuu
         AND DM.CancelDate IS NULL
         AND DH.DeleteDateTime IS NULL
           ;
            
    DECLARE @JuchuuNO varchar(11);
    DECLARE @AttentionFLG tinyint;
    DECLARE @ConfirmFLG tinyint;
    DECLARE @DirectFLG tinyint;
    DECLARE @Address1   varchar(100);
    DECLARE @Address2   varchar(100);
    DECLARE @CHK_Address2 tinyint;
    DECLARE @MailAddress  varchar(100);
    DECLARE @M_Address1   varchar(100);
    DECLARE @M_Address2   varchar(100);
    DECLARE @JuchuuGaku money;
    DECLARE @UnpaidAmount money;
    DECLARE @UnpaidCount int;
    DECLARE @ConfirmKBN tinyint;
    DECLARE @CardProgressKBN tinyint;
    DECLARE @SpecialFlg tinyint;
    DECLARE @PaymentProgressKBN tinyint;
    DECLARE @H_CommentCustomer   varchar(400);
    DECLARE @H_CommentCapital   varchar(400);
    DECLARE @CommentCustomer   varchar(400);
    DECLARE @CommentCapital   varchar(400);
    DECLARE @IncludeFLG tinyint;
    DECLARE @SpecFLG   tinyint;
    DECLARE @NoshiFLG  tinyint;
    DECLARE @GiftFLG   tinyint;
    DECLARE @CustomerName   varchar(80);
    DECLARE @DeliveryName   varchar(80);
    DECLARE @KaolaetcFLG   tinyint;
    DECLARE @Zip   varchar(100);
    DECLARE @JuchuuDate date;
    DECLARE @AnswerFLG tinyint;
    DECLARE @ArrivePlanDate  date;
    DECLARE @Nissu  int;
    DECLARE @WRK_HoryuFLG tinyint;
    DECLARE @ReturnFLG tinyint;
    
    --カーソルオープン
    OPEN CUR_Juchu;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO
        ,@AttentionFLG
        ,@ConfirmFLG
        ,@DirectFLG
        ,@Address1
        ,@Address2
        ,@CHK_Address2
        ,@MailAddress
        ,@M_Address1
        ,@M_Address2
        ,@JuchuuGaku
        ,@UnpaidAmount
        ,@UnpaidCount
        ,@ConfirmKBN
        ,@CardProgressKBN
        ,@SpecialFlg
        ,@PaymentProgressKBN
        ,@H_CommentCustomer
        ,@H_CommentCapital
        ,@CommentCustomer
        ,@CommentCapital
        ,@IncludeFLG
        ,@SpecFLG
        ,@NoshiFLG
        ,@GiftFLG
        ,@CustomerName
        ,@DeliveryName
        ,@KaolaetcFLG
        ,@Zip
        ,@JuchuuDate
        ,@AnswerFLG
        ,@ArrivePlanDate
        ,@Nissu;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock

        --1.受注ワークを1件リード

        SET @W_ERR = 0;
        
        --0 → WRK_保留FLG ※WRK_保留FLG はプログラム内ローカル変数
        SET @WRK_HoryuFLG = 0;

        --2.各保留チェック
        --①要注意顧客
        IF @AttentionFLG = 1
        BEGIN
            --D受注ワーク.債権要注意顧客 ＝ 1 の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'002'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
		END
        
        --②要確認顧客
        IF @ConfirmFLG = 1
        BEGIN
            --D受注ワーク.要確認顧客 ＝ 1 の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'003'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END

        --③直送)住所不完全
        IF @DirectFLG = 1 AND @CHK_Address2 = 0 
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.住所２ に数字(1,2,3,4,5,6,7,8,9,0のいずれか)が含まれていない時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'004'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --④直送)郵便番号不整合
        IF @DirectFLG = 1 AND ISNULL(@Address1,'') <> ISNULL(@M_Address1,'') 
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.住所１ <> D受注ワーク.住所１_Ｍ　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'005'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑤直送)先払い未入金（3万以上）
        IF @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidAmount,0)+@JuchuuGaku >= 30000
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.設定値 ＝ 1　and D受注ワーク.入金状況区分 ＝ 0(未入金) and　D受注ワーク.未入金額 + D受注ワーク.受注総額 ≧ 30000　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'006'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑥直送)先払い未入金（3件以上）
        IF @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidCount,0)+1 >= 3
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.設定値 ＝ 1　and　受注ワーク.入金状況区分 ＝ 0(未入金) and　D受注ワーク.未入金件数 + 1 ≧ 3　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'007'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑦要確認商品
        IF ISNULL(@ConfirmKBN,0) = 1
        BEGIN
        	--D受注ワーク.要確認商品 ＝ 1　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'008'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑧直送)カード決済不可	
        IF @DirectFLG = 1 AND @CardProgressKBN <> 0
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.カード状況 <> 0　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'009'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                
        END
        
        
        --⑨直送)先払い未決済
        IF @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0
        BEGIN
            --D受注ワーク.直送FLG ＝ 1(:直送)　かつ
            --D受注ワーク.設定値 ＝ 1　and　D受注ワーク.入金状況区分 ＝ 0(未入金)　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'010'
                ,@Operator 
                ,@SYSDATETIME 
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --⑩受注備考あり
        IF ISNULL(@H_CommentCustomer,'') = '' AND ISNULL(@H_CommentCapital,'') = '' 
           AND ISNULL(@CommentCustomer,'') = '' AND ISNULL(@CommentCapital,'') = '' 
        BEGIN
            --D受注ワーク.受注コメント顧客_Ｈ is NULL　かつ
            --D受注ワーク.受注コメントキャピタル_Ｈ is NULL　かつ
            --D受注ワーク.受注コメント顧客 is NULL　かつ
            --D受注ワーク.受注コメントキャピタル is NULL
        	SELECT NULL;
        END
        ELSE
        BEGIN
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'011'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑪同梱有無確認
        IF CHARINDEX('同梱',@H_CommentCustomer) = 0 AND CHARINDEX('同梱',@H_CommentCapital) = 0 
           AND CHARINDEX('同梱',@CommentCustomer) = 0 AND CHARINDEX('同梱',@CommentCapital) = 0 
           AND ISNULL(@IncludeFLG,0) = 0
        BEGIN
            --D受注ワーク.受注コメント顧客_Ｈ に「同梱」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル_Ｈ に「同梱」という文字が含まれない　かつ
            --D受注ワーク.受注コメント顧客 に「同梱」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル に「同梱」という文字が含まれない　かつ
            --D受注ステータス.同梱有無FLG ＝ 0
            SELECT NULL;
        END
        ELSE
        BEGIN
            --D受注ステータス.同梱有無FLGに「1」をセットしUpdate。
            UPDATE D_JuchuuStatus SET
               [IncludeFLG] = 1
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'012'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑫スペック計測有無
        IF CHARINDEX('スペック計測',@H_CommentCustomer) = 0 AND CHARINDEX('スペック計測',@H_CommentCapital) = 0 
           AND CHARINDEX('スペック計測',@CommentCustomer) = 0 AND CHARINDEX('スペック計測',@CommentCapital) = 0 
           AND ISNULL(@SpecFLG,0) = 0
        BEGIN
            --D受注ワーク.受注コメント顧客_Ｈ に「スペック計測」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル_Ｈ に「スペック計測」という文字が含まれない　かつ
            --D受注ワーク.受注コメント顧客 に「スペック計測」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル に「スペック計測」という文字が含まれない　かつ
            --D受注ステータス.スペック計測有無FLG ＝ 0
            SELECT NULL;
        END
        ELSE 
        BEGIN
            --D受注ステータス.スペック計測有無FLGに「1」をセットしUpdate。
            UPDATE D_JuchuuStatus SET
               [SpecFLG] = 1
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'013'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --⑬「のし」の有無確認
        IF CHARINDEX('のしあり',@H_CommentCustomer) = 0 AND CHARINDEX('のしあり',@H_CommentCapital) = 0 
           AND CHARINDEX('のしあり',@CommentCustomer) = 0 AND CHARINDEX('のしあり',@CommentCapital) = 0 
           AND ISNULL(@NoshiFLG,0) = 0
        BEGIN
            --D受注ワーク.受注コメント顧客_Ｈ に「のしあり」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル_Ｈ に「のしあり」という文字が含まれない　かつ
            --D受注ワーク.受注コメント顧客 に「のしあり」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル に「のしあり」という文字が含まれない　かつ
            --D受注ステータス.のし対象FLG ＝ 0
            SELECT NULL;
        END
        ELSE
        BEGIN
            --D受注ステータス.のし対象FLGに「1」をセットしUpdate。
            UPDATE D_JuchuuStatus SET
               [NoshiFLG] = 1
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'014'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --⑭「ギフト」の有償確認
        IF CHARINDEX('ラッピング',@H_CommentCustomer) = 0 AND CHARINDEX('ラッピング',@H_CommentCapital) = 0 
           AND CHARINDEX('ラッピング',@CommentCustomer) = 0 AND CHARINDEX('ラッピング',@CommentCapital) = 0 
           AND ISNULL(@GiftFLG,0) = 0
        BEGIN
            --D受注ワーク.受注コメント顧客_Ｈ に「ラッピング」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル_Ｈ に「ラッピング」という文字が含まれない　かつ
            --D受注ワーク.受注コメント顧客 に「ラッピング」という文字が含まれない　かつ
            --D受注ワーク.受注コメントキャピタル に「ラッピング」という文字が含まれない　かつ
            --D受注ステータス.ギフト対象FLG ＝ 0
            SELECT NULL;
        END
        ELSE
        BEGIN
            --D受注ステータス.ギフト対象FLGに「1」をセットしUpdate。
            UPDATE D_JuchuuStatus SET
               [GiftFLG] = 1
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'015'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑮依頼主≠発送先
        IF ISNULL(@CustomerName,'') <> ISNULL(@DeliveryName,'')
        BEGIN
            --D受注ワーク.顧客名　＝　D受注ワーク.配送先名　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'016'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --⑯楽天Kaola分チェック
        IF ISNULL(@KaolaetcFLG,'') <> 0
        BEGIN
            --D受注ワーク.コアラ等FLG　＝　0　の時以外
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'017'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END                                    
        END
        
        --⑰発送先が沖縄
        IF ISNULL(@Zip,'') <> ''
        BEGIN
            --D受注ワーク.文字型１ IS NOT NULL　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'018'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END

		--⑱発送先が海外
        IF CHARINDEX('都',@Address1) = 0 AND CHARINDEX('道',@Address1) = 0 
           AND CHARINDEX('府',@Address1) = 0 AND CHARINDEX('県',@Address1) = 0 
        BEGIN
            --D受注ワーク.住所１ に「都」「道」「府」「県」の全ての文字が含まれない時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'019'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END             
        END
        
        --⑲回答納期先日付
        IF @AnswerFLG = 1 AND @ArrivePlanDate IS NOT NULL
           AND DATEDIFF(day,@JuchuuDate, @ArrivePlanDate) >= ISNULL(@Nissu,0)
        BEGIN
            --D受注ワーク.回答納期登録あり＝1 かつ D受注ワーク.入荷予定日 IS NOT NULL　の時
            --DATEDIFF関数で、D受注ワーク.受注日とD受注ワーク.入荷予定日の日数を求め ≧ D受注ワーク.数字型1　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'020'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --⑳メールアドレスエラー
        IF @WRK_HoryuFLG = 0 
        BEGIN
        	SELECT NULL;
        	
        	
        END
        
        --21住所不完全
        IF @DirectFLG = 0 AND @CHK_Address2 = 0 
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.住所２ に数字(1,2,3,4,5,6,7,8,9,0のいずれか)が含まれていない時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'022'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --22郵便番号不整合
        IF @DirectFLG = 0 AND @Address1 <> ISNULL(@M_Address1,'') 
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.住所１ <> D受注ワーク.住所１_Ｍ　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'023'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --23先払い未入金（3万以上）
        IF @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidAmount,0)+@JuchuuGaku >= 30000
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.設定値 ＝ 1　and D受注ワーク.入金状況区分 ＝ 0(未入金) and　D受注ワーク.未入金額 + D受注ワーク.受注総額 ≧ 30000　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'024'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --24先払い未入金（3件以上）
        IF @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidCount,0)+1 >= 3
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.設定値 ＝ 1　and　受注ワーク.入金状況区分 ＝ 0(未入金) and　D受注ワーク.未入金件数 + 1 ≧ 3　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'025'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --25カード決済不可
        IF @DirectFLG = 0 AND @CardProgressKBN <> 0
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.カード状況 <> 0　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'026'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --26先払い未決済
        IF @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 
        BEGIN
            --D受注ワーク.直送FLG ＝ 0(:直送ではない)　かつ
            --D受注ワーク.設定値 ＝ 1　and　D受注ワーク.入金状況区分 ＝ 0(未入金)　の時
            --上記Select件数が0件の時、テーブル転送仕様Ａに従ってD受注保留警告(D_JuchuuOnHold)のレコード追加。
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'027'
                ,@Operator 
                ,@SYSDATETIME
                ,@ReturnFLG OUTPUT
                ;
            
            IF @ReturnFLG = 1
            BEGIN
            	SET @WRK_HoryuFLG = 1;
            END
        END
        
        --27
        --D受注.保留FLGに「WRK_保留FLG」をセットしUpdate。
        UPDATE D_Juchuu SET
           [OnHoldFLG] = @WRK_HoryuFLG
          ,[UpdateOperator] = @Operator
          ,[UpdateDateTime] = @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        ;
        
        --D受注ステータス.保留FLGに「WRK_保留FLG」をセットしUpdate。
        UPDATE D_JuchuuStatus SET
           [OnHoldFLG] = @WRK_HoryuFLG
          ,[UpdateOperator] = @Operator
          ,[UpdateDateTime] = @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        ;
            
        --次の「1.受注ワークを1件リード」へ。
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO
            ,@AttentionFLG
            ,@ConfirmFLG
            ,@DirectFLG
            ,@Address1
            ,@Address2
            ,@CHK_Address2
            ,@MailAddress
            ,@M_Address1
            ,@M_Address2
            ,@JuchuuGaku
            ,@UnpaidAmount
            ,@UnpaidCount
            ,@ConfirmKBN
            ,@CardProgressKBN
            ,@SpecialFlg
            ,@PaymentProgressKBN
            ,@H_CommentCustomer
            ,@H_CommentCapital
            ,@CommentCustomer
            ,@CommentCapital
            ,@IncludeFLG
            ,@SpecFLG
            ,@NoshiFLG
            ,@GiftFLG
            ,@CustomerName
            ,@DeliveryName
            ,@KaolaetcFLG
            ,@Zip
            ,@JuchuuDate
            ,@AnswerFLG
            ,@ArrivePlanDate
            ,@Nissu;
        
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
        'JuchuuDataCheck',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutErrNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
