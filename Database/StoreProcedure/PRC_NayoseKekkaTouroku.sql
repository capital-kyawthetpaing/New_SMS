DROP  PROCEDURE [dbo].[PRC_NayoseKekkaTouroku]
GO
DROP TYPE [dbo].[T_Nayose]
GO

--  ======================================================================
--       Program Call    名寄せ結果登録
--       Program ID      NayoseKekkaTouroku
--       Create date:    2021.5.30
--    ======================================================================

CREATE TYPE T_Nayose AS TABLE
    (
    [JuchuuNO] [varchar](11) ,
    [CustomerCD] [varchar](13) 
    )
GO

CREATE PROCEDURE PRC_NayoseKekkaTouroku
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @JuchuuNO    varchar(11),

    @Table       T_Nayose READONLY,
    @Operator    varchar(10),
    @PC          varchar(30),
    @OutJuchuuNo varchar(11) OUTPUT
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
    
    --処理履歴データへ更新
    SET @KeyItem = '';
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NayoseKekkaTouroku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutJuchuuNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
