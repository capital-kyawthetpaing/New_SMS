BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseKekkaTouroku]
END TRY

BEGIN CATCH END CATCH 

DROP TYPE [dbo].[T_Nayose]
GO

--  ======================================================================
--       Program Call    ���񂹌��ʓo�^
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
    (
    @Table       T_Nayose READONLY,
    @Operator    varchar(10),
    @PC          varchar(30)
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(20);
    DECLARE @KeyItem varchar(100);
        
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '���񂹌��ʓo�^';
    
	--�e�[�u���]���d�l�`
    UPDATE D_Juchuu SET
        [CustomerCD]             = tbl.CustomerCD
       ,[IdentificationFLG]      = 0
       ,[NayoseKekkaTourokuDate] = CONVERT(date,@SYSDATETIME)
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_Juchuu.JuchuuNO = tbl.JuchuuNo
    ;
    
    --�e�[�u���]���d�l�a
    UPDATE D_JuchuuOnHold SET
        [DisappeareDateTime]     = @SYSDATETIME
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_JuchuuOnHold.JuchuuNO = tbl.JuchuuNo
    AND D_JuchuuOnHold.OnHoldCD = '001'
    ;    
    
    --���������f�[�^�֍X�V
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
