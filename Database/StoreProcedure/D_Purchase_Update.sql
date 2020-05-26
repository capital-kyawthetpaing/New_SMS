--  ======================================================================
--       Program Call    ���Ϗ�
--       Program ID      ShiireTankaTeiseiIraisho
--       Create date:    2019.11.29
--    ======================================================================
CREATE TYPE T_PurchasePrint AS TABLE
    (
    [PurchaseNo] [varchar](11)
    )
GO

CREATE PROCEDURE D_Purchase_Update
    (@Table  T_PurchasePrint READONLY,
    @Operator  varchar(10),
    @PC  varchar(30)
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
        SET @OperateModeNm = '�ύX';

        UPDATE D_Purchase
           SET [OutputDateTime] = @SYSDATETIME                
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE PurchaseNO IN (SELECT tbl.PurchaseNO FROM @Table tbl)
           ;

/*
    --�J�[�\���̒l���擾����ϐ��錾
    DECLARE @W_COL1 varchar(11);
    
    --�J�[�\����`
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.PurchaseNO
        FROM   @Table tbl
        ORDER BY tbl.PurchaseNO

    --�J�[�\���I�[�v��
    OPEN CUR_AAA;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_AAA
    INTO @W_COL1;

    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN

        -- ========= ���[�v���̎��ۂ̏��� ��������===
        --���������f�[�^�֍X�V
        SET @KeyItem =@W_COL1;
            
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            'ShiireTankaTeiseiIraisho',
            @PC,
            NULL,
            @KeyItem;
        -- ========= ���[�v���̎��ۂ̏��� �����܂�===

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO @W_COL1;

    END

    --�J�[�\�������
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
*/
--<<OWARI>>
  return @W_ERR;

END

GO
