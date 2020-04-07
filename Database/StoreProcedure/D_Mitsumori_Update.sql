 BEGIN TRY 
 Drop Procedure dbo.[D_Mitsumori_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[D_Mitsumori_Update]
    (@Table  T_MitsumoriPrint READONLY,
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
    
        SET @OperateModeNm = '変更';

        UPDATE D_Mitsumori
           SET [PrintDateTime] = @SYSDATETIME                
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE MitsumoriNO IN (SELECT tbl.MitsumoriNO FROM @Table tbl)
           ;

    --カーソルの値を取得する変数宣言
    DECLARE @W_COL1 varchar(11);
    
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.MitsumoriNO
        FROM   @Table tbl
        ORDER BY tbl.MitsumoriNO

    --カーソルオープン
    OPEN CUR_AAA;

    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO @W_COL1;

    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN

        -- ========= ループ内の実際の処理 ここから===
        --処理履歴データへ更新
        SET @KeyItem =@W_COL1;
            
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            'Mitsumorisyo',
            @PC,
            NULL,
            @KeyItem;
        -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO @W_COL1;

    END

    --カーソルを閉じる
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
    
--<<OWARI>>
  return @W_ERR;

END


