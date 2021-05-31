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
    INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,Tel12,Tel13,@MailAddress,@Address1,@Address2 ;
    
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ループ内の実際の処理 ここから===*************************CUR_Stock
        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,Tel12,Tel13,@MailAddress,@Address1,@Address2 ;
        
        SET @W_ERR = 0;
        
        
        
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

    SET @OutJuchuuNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
