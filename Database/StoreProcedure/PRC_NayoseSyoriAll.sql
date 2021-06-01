BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseSyoriAll]
END TRY

BEGIN CATCH END CATCH 

BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_NayoseSyoriAll_Sub]
END TRY

BEGIN CATCH END CATCH 
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
       ,ISNULL((SELECT MAX(D.OnHoldRows) FROM D_JuchuuOnHold As D WHERE D.JuchuuNO = @JuchuuNO),0)+1 AS OnHoldRows
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
                        SET @CustomerCD =(SELECT CustomerCount+1 FROM M_CustomerCounter WHERE MainKEY = 1);

                        UPDATE M_CustomerCounter SET
                        	[CustomerCount] = [CustomerCount] +1
                        WHERE MainKEY = 1
                        ;
                        
                        INSERT INTO [M_Customer]
                                   ([CustomerCD]
                                   ,[ChangeDate]
                                   ,[VariousFLG]
                                   ,[CustomerName]
                                   ,[LastName]
                                   ,[FirstName]
                                   ,[LongName1]
                                   ,[LongName2]
                                   ,[KanaName]
                                   ,[StoreKBN]
                                   ,[CustomerKBN]
                                   ,[StoreTankaKBN]
                                   ,[AliasKBN]
                                   ,[BillingType]
                                   ,[GroupName]
                                   ,[BillingFLG]
                                   ,[CollectFLG]
                                   ,[BillingCD]
                                   ,[CollectCD]
                                   ,[BirthDate]
                                   ,[Sex]
                                   ,[Tel11]
                                   ,[Tel12]
                                   ,[Tel13]
                                   ,[Tel21]
                                   ,[Tel22]
                                   ,[Tel23]
                                   ,[ZipCD1]
                                   ,[ZipCD2]
                                   ,[Address1]
                                   ,[Address2]
                                   ,[MailAddress]
                                   ,[TankaCD]
                                   ,[PointFLG]
                                   ,[LastPoint]
                                   ,[WaitingPoint]
                                   ,[TotalPoint]
                                   ,[TotalPurchase]
                                   ,[UnpaidAmount]
                                   ,[UnpaidCount]
                                   ,[LastSalesDate]
                                   ,[LastSalesStoreCD]
                                   ,[MainStoreCD]
                                   ,[StaffCD]
                                   ,[AttentionFLG]
                                   ,[ConfirmFLG]
                                   ,[ConfirmComment]
                                   ,[BillingCloseDate]
                                   ,[CollectPlanMonth]
                                   ,[CollectPlanDate]
                                   ,[HolidayKBN]
                                   ,[TaxTiming]
                                   ,[TaxPrintKBN]
                                   ,[TaxFractionKBN]
                                   ,[AmountFractionKBN]
                                   ,[CreditLevel]
                                   ,[CreditCard]
                                   ,[CreditInsurance]
                                   ,[CreditDeposit]
                                   ,[CreditETC]
                                   ,[CreditAmount]
                                   ,[CreditAdditionAmount]
                                   ,[CreditCheckKBN]
                                   ,[CreditMessage]
                                   ,[FareLevel]
                                   ,[Fare]
                                   ,[PaymentMethodCD]
                                   ,[KouzaCD]
                                   ,[DisplayOrder]
                                   ,[PaymentUnit]
                                   ,[NoInvoiceFlg]
                                   ,[CountryKBN]
                                   ,[CountryName]
                                   ,[RegisteredNumber]
                                   ,[DMFlg]
                                   ,[RemarksOutStore]
                                   ,[RemarksInStore]
                                   ,[AnalyzeCD1]
                                   ,[AnalyzeCD2]
                                   ,[AnalyzeCD3]
                                   ,[DeleteFlg]
                                   ,[UsedFlg]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime])
                             SELECT
                                    @CustomerCD
                                   ,GETDATE() --ChangeDate
                                   ,0 --VariousFLG, tinyint,>
                                   ,DH.CustomerName
                                   ,NULL--<LastName, varchar(20),>
                                   ,NULL--<FirstName, varchar(20),>
                                   ,NULL--<LongName1, varchar(50),>
                                   ,NULL--<LongName2, varchar(50),>
                                   ,DH.CustomerKanaName
                                   ,1	--StoreKBN, tinyint,>
                                   ,0	--CustomerKBN, tinyint,>
                                   ,1	--StoreTankaKBN, tinyint,>
                                   ,1	--AliasKBN, tinyint,>
                                   ,1	--BillingType, tinyint,>
                                   ,NULL	--<GroupName, varchar(40),>
                                   ,1	--BillingFLG, tinyint,>
                                   ,1	--CollectFLG, tinyint,>
                                   ,@CustomerCD	--BillingCD, varchar(13),>
                                   ,@CustomerCD	--CollectCD, varchar(13),>
                                   ,NULL	--<BirthDate, date,>
                                   ,0	--Sex, tinyint,>
                                   ,DH.Tel11
                                   ,DH.Tel12
                                   ,DH.Tel13
                                   ,DH.Tel21
                                   ,DH.Tel22
                                   ,DH.Tel23
                                   ,DH.ZipCD1
                                   ,DH.ZipCD2
                                   ,DH.Address1
                                   ,DH.Address2
                                   ,DH.MailAddress
                                   ,'0000000000000'		--TankaCD, varchar(13),>
                                   ,0	--PointFLG, tinyint,>
                                   ,0	--LastPoint, money,>
                                   ,0	--WaitingPoint, money,>
                                   ,0	--TotalPoint, money,>
                                   ,0	--TotalPurchase, money,>
                                   ,0	--UnpaidAmount, money,>
                                   ,0	--UnpaidCount, money,>
                                   ,NULL	--LastSalesDate, date,>
                                   ,NULL	--LastSalesStoreCD, varchar(4),>
                                   ,NULL	--MainStoreCD, varchar(4),>
                                   ,NULL	--StaffCD, varchar(10),>
                                   ,0	--AttentionFLG, tinyint,>
                                   ,0	--ConfirmFLG, tinyint,>
                                   ,NULL	--ConfirmComment, varchar(50),>
                                   ,0	--BillingCloseDate, tinyint,>
                                   ,0	--CollectPlanMonth, tinyint,>
                                   ,0	--CollectPlanDate, tinyint,>
                                   ,0	--HolidayKBN, tinyint,>
                                   ,0	--TaxTiming, tinyint,>
                                   ,0	--TaxPrintKBN, tinyint,>
                                   ,0	--TaxFractionKBN, tinyint,>
                                   ,0	--AmountFractionKBN, tinyint,>
                                   ,0	--CreditLevel, tinyint,>
                                   ,0	--CreditCard, money,>
                                   ,0	--CreditInsurance, money,>
                                   ,0	--CreditDeposit, money,>
                                   ,0	--CreditETC, money,>
                                   ,0	--CreditAmount, money,>
                                   ,0	--CreditAdditionAmount, money,>
                                   ,0	--CreditCheckKBN, tinyint,>
                                   ,NULL	--CreditMessage, varchar(100),>
                                   ,0	--FareLevel, money,>
                                   ,0	--Fare, money,>
                                   ,NULL	--PaymentMethodCD, varchar(3),>
                                   ,NULL	--KouzaCD, varchar(3),>
                                   ,0	--DisplayOrder, int,>
                                   ,0	--PaymentUnit, tinyint,>
                                   ,0	--NoInvoiceFlg, tinyint,>
                                   ,0	--CountryKBN, tinyint,>
                                   ,NULL	--CountryName, varchar(30),>
                                   ,NULL	--RegisteredNumber, varchar(15),>
                                   ,0	--DMFlg, tinyint,>
                                   ,NULL	--RemarksOutStore, varchar(500),>
                                   ,NULL	--RemarksInStore, varchar(500),>
                                   ,NULL	--AnalyzeCD1, varchar(10),>
                                   ,NULL	--AnalyzeCD2, varchar(10),>
                                   ,NULL	--AnalyzeCD3, varchar(10),>
                                   ,0	--DeleteFlg, tinyint,>
                                   ,1	--UsedFlg, tinyint,>
                                   ,@Operator AS InsertOperator
                                   ,@SYSDATETIME AS InsertDateTime
                                   ,@Operator AS UpdateOperator
                                   ,@SYSDATETIME AS UpdateDateTime
                                  from D_Juchuu AS DH
                                 WHERE DH.JuchuuNO = @JuchuuNO
                                   AND DH.DeleteDateTime IS NULL
                                   ;

                        
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
        
    END     --LOOP�̏I���***************************************CUR_Juchu
    
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
