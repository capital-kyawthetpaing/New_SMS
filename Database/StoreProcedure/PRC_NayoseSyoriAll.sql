DROP  PROCEDURE [dbo].[PRC_NayoseSyoriAll]
GO
DROP  PROCEDURE [dbo].[PRC_NayoseSyoriAll_Sub]
GO

CREATE PROCEDURE PRC_NayoseSyoriAll_Sub
    (@JuchuuNO   varchar(11),
     @Operator   varchar(10),
     @SYSDATETIME  datetime
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
	--��LSelect������1���̎��AD_Juchuu.�ۗ�FLG�A���񂹑Ώ�FLG��1��Update�B
    UPDATE D_Juchuu SET
         [OnHoldFLG]         = 1
        ,[IdentificationFLG] = 1
        ,[UpdateOperator]    = @Operator  
        ,[UpdateDateTime]    = @SYSDATETIME
    WHERE JuchuuNO = @JuchuuNO
    ;
    
    --�e�[�u���]���d�l�a�ɏ]���Ď󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
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
       ,1 AS OnHoldRows
       ,'001' AS OnHoldCD
       ,@SYSDATETIME AS OccorDateTime
       ,NULL AS DisappeareDateTime
       ,NULL AS Remarks
       ,@Operator AS StaffCD
       ,@Operator AS InsertOperator
       ,@SYSDATETIME AS InsertDateTime
       ,@Operator AS UpdateOperator
       ,@SYSDATETIME AS UpdateDateTime
    FROM DUAL
    WHERE NOT EXISTS(SELECT 1 FROM D_JuchuuOnHold AS DJ
                      WHERE DJ.JuchuuNO = @JuchuuNO
                        AND DJ.OnHoldCD = '001')
    ;

    --�e�[�u���]���d�l�b�ɏ]���Ď󒍃X�e�[�^�X(D_JuchuuStatus)�̃��R�[�h�ύX�B
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
        SELECT 
            @JuchuuNO
           ,0 AS OnHoldFLG
           ,0 AS WarningFLG
           ,0 AS IncludeFLG
           ,0 AS GiftFLG
           ,0 AS NoshiFLG
           ,0 AS SpecFLG
           ,0 AS NouhinsyoFLG
           ,0 AS RyousyusyoFLG
           ,0 AS SeikyuusyoFLG
           ,0 AS SonotoFLG
           ,0 AS OrderMailFLG
           ,0 AS NyuukaYoteiMailFLG
           ,0 AS SyukkaYoteiMailFLG
           ,0 AS SyukkaAnnaiMailFLG
           ,0 AS NyuukinMailFLG
           ,0 AS FollowupMailFLG
           ,0 AS Demand1MailFLG
           ,0 AS Demand2MailFLG
           ,0 AS Demand3MailFLG
           ,0 AS Demand4MailFLG
           ,@Operator AS InsertOperator
           ,@SYSDATETIME AS InsertDateTime
           ,@Operator AS UpdateOperator
           ,@SYSDATETIME AS UpdateDateTime
         FROM DUAL
         ;
	END
END

GO


--  ======================================================================
--       Program Call    ���񂹌��ʓo�^
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
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(20);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 1;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '���񂹏���';
    
    --���񂹑Ώۂ̎󒍃f�[�^�𒊏o���A�v���O�������󒍃��[�N�֍X�V�B
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
        WHERE DH.JuchuuKBN = 1	--�󒍎�ʋ敪              
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

    --�J�[�\���I�[�v��
    OPEN CUR_Juchu;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,@Tel12,@Tel13,@MailAddress,@Address1,@Address2 ;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===*************************CUR_Stock
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO,@CustomerName,@CustomerName2,@Tel11,@Tel12,@Tel13,@MailAddress,@Address1,@Address2 ;
        --�󒍃��[�N��1�����[�h

        SET @W_ERR = 0;
        
        --���񂹗p�̌ڋq�}�X�^�𒊏o���A�v���O�������ڋq���[�N�֍X�V
        
        --�ڋq���[�N�Ɩ��񂹃`�F�b�N
        --�@���O �� �d�b�ԍ� �� ���[���A�h���X��v
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
            
            --��LSelect������1���̎��AD_Juchuu.�ڋqCD��M_Customer.�ڋqCD��Update�B
            --D_Juchuu.���񂹑Ώ�FLG��0��Update�B
            UPDATE D_Juchuu SET
                 [CustomerCD]        = @CustomerCD
                ,[IdentificationFLG] = 0
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --�e�[�u���]���d�l�a�ɏ]���Ď󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ύX�B(��1�j
            Update D_JuchuuOnHold set
                 [DisappeareDateTime] = @SYSDATETIME	--��������
                ,[UpdateOperator]     = @Operator  
                ,[UpdateDateTime]     = @SYSDATETIME
             WHERE D_JuchuuOnHold.JuchuuNO = @JuchuuNO
               AND D_JuchuuOnHold.OnHoldCD = '001'
               ;

            --�e�[�u���]���d�l�b�ɏ]���Ď󒍃X�e�[�^�X(D_JuchuuStatus)�̃��R�[�h�ǉ��������͕ύX�B
            --Update(�@�́�1�A�D�́�1�̎��j
            UPDATE D_JuchuuStatus SET
                 [OnHoldFLG] = 0    --�ۗ��L��FLG
                ,[UpdateOperator]    = @Operator  
                ,[UpdateDateTime]    = @SYSDATETIME
             WHERE D_JuchuuStatus.JuchuuNO = @JuchuuNO
               ;
            --���́u1.�󒍃��[�N��1�����[�h�v�ցB
        END
        ELSE IF @CNT1 = 0
        BEGIN
            --��LSelect������0���̎��A�A�̖��񂹃`�F�b�N��
            
            --�A���O �� �d�b�ԍ�
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
                EXEC PRC_NayoseSyoriAll_Sub
                    @JuchuuNO
                    ,@Operator 
                    ,@SYSDATETIME
                    ;

                --���́u1.�󒍃��[�N��1�����[�h�v�ցB
            END
            ELSE IF @CNT2 = 0
            BEGIN
                --��LSelect������0���̎��A�B�̖��񂹃`�F�b�N��
                
                --�B���O �� ���[���A�h���X
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
                    --��LSelect������1���̎��A
                    EXEC PRC_NayoseSyoriAll_Sub
                        @JuchuuNO
                        ,@Operator 
                        ,@SYSDATETIME
                        ;
                    
                    --���́u1.�󒍃��[�N��1�����[�h�v�ցB
                END
                ELSE IF @CNT3 = 0
                BEGIN
                    --�C�̖��񂹃`�F�b�N��
                    --�C���O �� �Z���P  
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
                        --��LSelect������1���̎��A
                        EXEC PRC_NayoseSyoriAll_Sub
                            @JuchuuNO
                            ,@Operator 
                            ,@SYSDATETIME
                            ;
                        
                        --���́u1.�󒍃��[�N��1�����[�h�v�ցB

                    END
                    ELSE IF @CNT4 = 0
                    BEGIN
                        --��LSelect������0���̎��A
                        --�D�̌ڋq�}�X�^�o�^��
                        --�e�[�u���]���d�l�`�ɏ]���Čڋq�}�X�^�̃��R�[�h�ǉ��B
                        
                        
                        
                        --�̔Ԃ����ڋqCD��D_Juchuu.�ڋqCD��Update�B
                        UPDATE D_Juchuu SET
                             [CustomerCD]        = @CustomerCD
                            ,[UpdateOperator]    = @Operator  
                            ,[UpdateDateTime]    = @SYSDATETIME
                        WHERE JuchuuNO = @JuchuuNO
                        ;

                        --�e�[�u���]���d�l�b�ɏ]���Ď󒍃X�e�[�^�X(D_JuchuuStatus)�̃��R�[�h�ύX�B(��1)
                        --Update(�@�́�1�A�D�́�1�̎��j
                        UPDATE D_JuchuuStatus SET
                             [OnHoldFLG] = 0    --�ۗ��L��FLG
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
            --��LSelect�������������̎��A
            EXEC PRC_NayoseSyoriAll_Sub
                @JuchuuNO
                ,@Operator 
                ,@SYSDATETIME
                ;
            
            --���́u1.�󒍃��[�N��1�����[�h�v�ցB
		END
        
    END     --LOOP�̏I���***************************************CUR_Stock
    
    --�J�[�\�������
    CLOSE CUR_Juchu;
    DEALLOCATE CUR_Juchu;

    IF @W_ERR = 1
    BEGIN
        SET @OutErrNo = 'S013';
        return @W_ERR;
    END

    --���������f�[�^�֍X�V
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
