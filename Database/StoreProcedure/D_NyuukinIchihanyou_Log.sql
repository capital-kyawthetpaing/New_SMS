 BEGIN TRY 
 Drop Procedure dbo.[D_NyuukinIchihanyou_Log]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_NyuukinIchihanyou_Log] 
	-- Add the parameters for the stored procedure here
    (@Table D_NyuuKinIchiHanyou READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
	@ProgramID as varchar (30)
	)as
BEGIN


	

	DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
        SET @OperateModeNm = '変更';

    --カーソルの値を取得する変数宣言
    DECLARE @W_COL1 varchar(11);
    
    --カーソル定義
    DECLARE CUR_AAA CURSOR FOR
        SELECT Distinct tbl.StoreCD
        FROM   @Table tbl
        ORDER BY StoreCD asc

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
            @ProgramID,
            @PC,
            NULL,
            @KeyItem;
		--print ( @SYSDATETIME +
		--+ @Operator+
		-- 'KeihiltiranHyou'+
		-- @PC+
		-- NULL+
		-- @KeyItem)
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

