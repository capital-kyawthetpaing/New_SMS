DROP  PROCEDURE [dbo].[PRC_NayoseSyoriAll]
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
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,@Tel12,@Tel13,@MailAddress,@Address1,@Address2 ;
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
                     AND dbo.Fnc_MailAdress(FC.MailAddress) = dbo.Fnc_MailAdress(DH.MailAddress)                                      
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
                                  AND dbo.Fnc_MailAdress(FC.MailAddress) = dbo.Fnc_MailAdress(DH.MailAddress)                                                  
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
			
			--テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード追加もしくは変更。
			
			--次の「1.受注ワークを1件リード」へ。
		END
		ELSE IF @CNT1 = 0
		BEGIN
			--上記Select件数が0件の時、②の名寄せチェックへ
			
			--②名前 ＆ 電話番号
        	SET @CNT2= (SELECT FC.CustomerCD
	                      from D_Juchuu AS DH
	                     INNER JOIN F_Customer(GETDATE()) FC
	                        ON FC.CustomerName = DH.CustomerName
	                       AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
	                       AND FC.StoreKBN = 1
	                     WHERE DH.JuchuuNO = @JuchuuNO
	                       AND DH.DeleteDateTime IS NULL
	        			)
	        IF @CNT2 >= 1
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
	            
	            --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。
	            
	            --次の「1.受注ワークを1件リード」へ。
	        END
	        ELSE IF @CNT2 = 0
	        BEGIN
	        	--上記Select件数が0件の時、③の名寄せチェックへ
	        	
				--③名前 ＆ メールアドレス
		        SET @CNT3 = (SELECT FC.CustomerCD
		                       from D_Juchuu AS DH
		                      INNER JOIN F_Customer(GETDATE()) FC
		                         ON FC.CustomerName = DH.CustomerName
		                        AND dbo.Fnc_MailAdress(FC.MailAddress) = dbo.Fnc_MailAdress(DH.MailAddress) 
		                        AND FC.StoreKBN = 1
		                      WHERE DH.JuchuuNO = @JuchuuNO
		                        AND DH.DeleteDateTime IS NULL
		        			)
		        IF @CNT3 >= 1
		        BEGIN
		        	--上記Select件数が1件の時、
		        	--D_Juchuu.保留FLG、名寄せ対象FLGに1をUpdate。
		            UPDATE D_Juchuu SET
		                 [OnHoldFLG]         = 1
		                ,[IdentificationFLG] = 1
		                ,[UpdateOperator]    = @Operator  
		                ,[UpdateDateTime]    = @SYSDATETIME
		            WHERE JuchuuNO = @JuchuuNO
		            ;
		            
		            --テーブル転送仕様Ｂに従って受注保留警告(D_JuchuuOnHold)のレコード追加。
		            
		            --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。
		            
	            	--次の「1.受注ワークを1件リード」へ。

		        END
		        ELSE IF @CNT3 = 0
		        BEGIN
		        	--④の名寄せチェックへ
		        	--④名前 ＆ 住所１	
		        	SET @CNT4= (SELECT FC.CustomerCD
			                      from D_Juchuu AS DH
			                     INNER JOIN F_Customer(GETDATE()) FC
			                        ON FC.CustomerName = DH.CustomerName
			                       AND dbo.Fnc_AdressHalfToFull(FC.Address1) = dbo.Fnc_AdressHalfToFull(DH.Address1)
			                       AND FC.StoreKBN = 1
			                     WHERE DH.JuchuuNO = @JuchuuNO
			                       AND DH.DeleteDateTime IS NULL
			        			)
		        	IF @CNT4 >= 1
		        	BEGIN
		        		--上記Select件数が1件の時、
			        	--D_Juchuu.保留FLG、名寄せ対象FLGに1をUpdate。
			            UPDATE D_Juchuu SET
			                 [OnHoldFLG]         = 1
			                ,[IdentificationFLG] = 1
			                ,[UpdateOperator]    = @Operator  
			                ,[UpdateDateTime]    = @SYSDATETIME
			            WHERE JuchuuNO = @JuchuuNO
			            ;
		            
                        --テーブル転送仕様Ｂに従って受注保留警告(D_JuchuuOnHold)のレコード追加。
                        
                        --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード変更。
                        
                        --次の「1.受注ワークを1件リード」へ。

		        	END
		        	ELSE IF @CNT4 = 0
		        	BEGIN
		        		--上記Select件数が0件の時、
		        		--⑤の顧客マスタ登録へ
		        		--テーブル転送仕様Ａに従って顧客マスタのレコード追加。
		        		
		        		
		        		
		        		--採番した顧客CDをD_Juchuu.顧客CDにUpdate。
			            UPDATE D_Juchuu SET
			                 [CustomerCD]        = @CustomerCD
			                ,[UpdateOperator]    = @Operator  
			                ,[UpdateDateTime]    = @SYSDATETIME
			            WHERE JuchuuNO = @JuchuuNO
			            ;

		        		


		        	END
		        END		        
	        END
	    END
		ELSE
		BEGIN
			--上記Select件数が複数件の時、
			--D_Juchuu.保留FLG、名寄せ対象FLGに1をUpdate。
            UPDATE D_Juchuu SET
                 [OnHoldFLG]         = 1
                ,[IdentificationFLG] = 1
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --テーブル転送仕様Ｂに従って受注保留警告(D_JuchuuOnHold)のレコード追加。
            
            --テーブル転送仕様Ｃに従って受注ステータス(D_JuchuuStatus)のレコード追加もしくは変更。
            
            --次の「1.受注ワークを1件リード」へ。

		END
        
    END     --LOOPの終わり***************************************CUR_Stock
    
    --カーソルを閉じる
    CLOSE CUR_Juchu;
    DEALLOCATE CUR_Juchu;

    IF @W_ERR = 1
    BEGIN
        SET @OutErrNo = 'S013';
        return @W_ERR;
    END

/*
	                                                                        
	--テーブル転送仕様Ａ
    UPDATE D_Juchuu SET
        [CustomerCD]             = tbl.CustomerCD
       ,[IdentificationFLG]      = 0
       ,[NayoseKekkaTourokuDate] = CONVERT(date,@SYSDATETIME)
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_Juchuu.JuchuuNO = tbl.JuchuuNo
    ;
    
    --テーブル転送仕様Ｂ
    UPDATE D_JuchuuOnHold SET
        [DisappeareDateTime]     = @SYSDATETIME
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_JuchuuOnHold.JuchuuNO = tbl.JuchuuNo
    AND D_JuchuuOnHold.OnHoldCD = '001'
    ;    
*/
    --処理履歴データへ更新
    SET @KeyItem = '';
        
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
