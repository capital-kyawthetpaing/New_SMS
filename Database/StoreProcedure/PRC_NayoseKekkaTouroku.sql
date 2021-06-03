BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseKekkaTouroku]
END TRY

BEGIN CATCH END CATCH 

DROP TYPE [dbo].[T_Nayose]
GO

--  ======================================================================
--       Program Call    名寄せ結果登録
--       Program ID      NayoseKekkaTouroku
--       Create date:    2021.5.30
--    ======================================================================

CREATE TYPE T_Nayose AS TABLE
    (
    [JuchuuNO]     [varchar](11) ,
    [CustomerCD]   [varchar](13),
    [UpdateFlg]    [tinyint]
    )
GO

CREATE PROCEDURE PRC_NayoseKekkaTouroku
    (
	@NayoseKekkaTourokuDate as VarChar(10),
    @Table       T_Nayose READONLY,
    @Operator    varchar(10),
    @PC          varchar(30)
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
        
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '名寄せ結果登録';
    
    IF ISNULL(@NayoseKekkaTourokuDate,'') <> ''
    BEGIN
        --テーブル転送仕様Ａ
        UPDATE D_Juchuu SET
            [CustomerCD]             = NULL
           ,[IdentificationFLG]      = 1
           ,[NayoseKekkaTourokuDate] = NULL
           ,[UpdateOperator]         = @Operator  
           ,[UpdateDateTime]         = @SYSDATETIME
         FROM @Table tbl
        WHERE D_Juchuu.JuchuuNO = tbl.JuchuuNo
          AND tbl.UpdateFlg = 1
        ;
        
        --テーブル転送仕様Ｂ
        UPDATE D_JuchuuOnHold SET
            [DisappeareDateTime]     = NULL
           ,[UpdateOperator]         = @Operator  
           ,[UpdateDateTime]         = @SYSDATETIME
         FROM @Table tbl
        WHERE D_JuchuuOnHold.JuchuuNO = tbl.JuchuuNo
          AND D_JuchuuOnHold.OnHoldCD = '001'
          AND tbl.UpdateFlg = 1
        ; 
    END
    
	--テーブル転送仕様Ａ
    UPDATE D_Juchuu SET
        [CustomerCD]             = tbl.CustomerCD
       ,[IdentificationFLG]      = 0
       ,[NayoseKekkaTourokuDate] = CONVERT(date,@SYSDATETIME)
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
     FROM @Table tbl
    WHERE D_Juchuu.JuchuuNO = tbl.JuchuuNo
      AND tbl.UpdateFlg = 0
    ;
    
    --テーブル転送仕様Ｂ
    UPDATE D_JuchuuOnHold SET
        [DisappeareDateTime]     = @SYSDATETIME
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
     FROM @Table tbl
    WHERE D_JuchuuOnHold.JuchuuNO = tbl.JuchuuNo
      AND D_JuchuuOnHold.OnHoldCD = '001'
      AND tbl.UpdateFlg = 0
    ;    
    
    --処理履歴データへ更新
    SET @KeyItem = NULL;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NayoseKekkaTouroku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    
--<<OWARI>>
  return @W_ERR;

END

GO
