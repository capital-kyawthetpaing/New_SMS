BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_JuchuuDataCheck]
END TRY

BEGIN CATCH END CATCH 

BEGIN TRY 
 DROP PROCEDURE [dbo].[PRC_JuchuuDataCheck_Sub]
END TRY

BEGIN CATCH END CATCH 
GO

CREATE PROCEDURE PRC_JuchuuDataCheck_Sub
    (@JuchuuNO   varchar(11),
     @OnHoldCD   varchar(3),
     @Operator   varchar(10),
     @SYSDATETIME  datetime,
     @WRK_HoryuFLG tinyint OUTPUT 
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN

    --�e�[�u���]���d�l�`�ɏ]���Ď󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
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
       ,@OnHoldCD
       ,@SYSDATETIME AS OccorDateTime
       ,NULL AS DisappeareDateTime
       ,NULL AS Remarks
       ,@Operator AS StaffCD
       ,@Operator AS InsertOperator
       ,@SYSDATETIME AS InsertDateTime
       ,@Operator AS UpdateOperator
       ,@SYSDATETIME AS UpdateDateTime
    WHERE NOT EXISTS(SELECT 1 FROM D_JuchuuOnHold AS DJ
                      WHERE DJ.JuchuuNO = @JuchuuNO
                        AND DJ.OnHoldCD = @OnHoldCD)
    ;

    IF @@ROWCOUNT = 0
    BEGIN
    	SET @WRK_HoryuFLG = 0
    END
    ELSE
    BEGIN
    	SET @WRK_HoryuFLG = 1
    END

END

GO


--  ======================================================================
--       Program Call    �󒍃f�[�^�`�F�b�N����
--       Program ID      JuchuuDataCheck
--       Create date:    2021.6.7
--    ======================================================================

CREATE PROCEDURE PRC_JuchuuDataCheck
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
    DECLARE @OperateModeNm varchar(30);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 1;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '�󒍃f�[�^�`�F�b�N����';
    
    --���񂹑Ώۂ̎󒍃f�[�^�𒊏o���A�v���O�������󒍃��[�N�֍X�V�B
    DECLARE CUR_Juchu CURSOR FOR
        SELECT DH.JuchuuNO      --D��.�󒍔ԍ��A          
              --,DM.JuchuuRows    --D�󒍖���.���טA��            
              ,(SELECT top 1 A.AttentionFLG
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS AttentionFLG        --M�ڋq.���v���ӌڋq�A            
              ,(SELECT top 1 A.ConfirmFLG
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS ConfirmFLG      --M�ڋq.����v�m�F�ڋq�A            
              ,DM.DirectFLG     --D�󒍖���.����FLG�A           
              ,DH.Address1      --D��.�Z���P�A            
              ,DH.Address2      --D��.�Z���Q�A
              ,(SELECT COUNT(D.address2)
                  from d_juchuu AS D
                 where D.Address2  LIKE '%[0-9]%'
                   AND D.JuchuuNO = D.JuchuuNO) AS CHK_Address2            
              ,MZ.Address1 AS M_Address1        --M�X�֔ԍ��ϊ�.�Z���P as �Z���P_�l�A           
              ,MZ.Address2 AS M_Address2        --M�X�֔ԍ��ϊ�.�Z���Q as �Z���Q_�l�A           
              ,DH.JuchuuGaku        --D��.�󒍑��z�A          
              ,(SELECT top 1 A.UnpaidAmount
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS UnpaidAmount        --M�ڋq.�������z�A          
              ,(SELECT top 1 A.UnpaidCount
                  FROM M_Customer AS A
                 WHERE A.CustomerCD = DH.CustomerCD AND A.ChangeDate <= DH.JuchuuDate 
                   AND A.DeleteFlg = 0
                 ORDER BY A.ChangeDate DESC) AS UnpaidCount     --M�ڋq.�����������A            
              ,(SELECT top 1 MM3.Num1 --ConfirmCD
                  FROM M_SKU AS M 
                 INNER JOIN M_MultiPorpose AS MM3
                    ON MM3.ID = 313 AND MM3.[Key] = M.ConfirmCD
                 WHERE M.ChangeDate <= DH.JuchuuDate 
                   AND M.AdminNO = DM.AdminNO
                   AND M.DeleteFlg = 0
                 ORDER BY M.ChangeDate desc) AS ConfirmKBN   --M�r�j�t.�v�m�F�i�敪�A            
              ,DH.CardProgressKBN       --D��.�J�[�h�󋵁A            
              ,MC.SpecialFlg        --M�敪�ϊ�.�ݒ�l          
              --,DH.PaymentMethodCD       --D��.�\��������@�A          
              ,DH.PaymentProgressKBN        --D��.�����󋵋敪�A          
              ,DH.CommentCustomer AS H_CommentCustomer      --D��.�󒍃R�����g�ڋq as �󒍃R�����g�ڋq_�g�A           
              ,DH.CommentCapital AS H_CommentCapital        --D��.�󒍃R�����g�L���s�^�� as �󒍃R�����g�L���s�^��_�g�A           
              ,'DM.'CommentCustomer       --D�󒍖���.�󒍃R�����g�ڋq�A          
              ,'DM.'CommentCapital        --D�󒍖���.�󒍃R�����g�L���s�^���A            
              ,DS.IncludeFLG            --D�󒍃X�e�[�^�X.�����L��FLG�A         
              ,DS.SpecFLG               --D�󒍃X�e�[�^�X.�X�y�b�N�v���L��FLG�A         
              ,DS.NoshiFLG              --D�󒍃X�e�[�^�X.�̂��Ώ�FLG�A         
              ,DS.GiftFLG               --D�󒍃X�e�[�^�X.�M�t�g�Ώ�FLG�A           
              ,DH.CustomerName          --D��.�ڋq���A            
              ,DH.DeliveryName          --D��.�z���於�A          
              ,DH.KaolaetcFLG           --D��.�R�A����FLG�A           
              ,MM1.Char1 AS Zip         --M�ėp_1.�����^�P�A            
              ,DH.JuchuuDate AS JuchuuDate      --D��.�󒍓�          
              ,DM.AnswerFLG         --D�󒍖���.�񓚔[���o�^����A          
              ,DM.ArrivePlanDate AS ArrivePlanDate      --D�󒍖���.���ח\���          
              ,(SELECT MM2.Num1 FROM M_MultiPorpose AS MM2  --���񓚔[������t���f�p��M�ėp
                 WHERE MM2.ID = 231 AND MM2.[Key] = 1) AS Nissu --M�ėp_2.�����^1�A         
              
        from D_Juchuu AS DH
       INNER JOIN D_JuchuuDetails AS DM
          ON DM.JuchuuNO = DH.JuchuuNO
         AND DM.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_JuchuuStatus AS DS
          ON DS.JuchuuNO = DH.JuchuuNO
        LEFT OUTER JOIN M_ZipCode AS MZ
          ON MZ.ZipCD1 = DH.ZipCD1
         AND MZ.ZipCD2 = DH.ZipCD2
        LEFT OUTER JOIN M_Conversion AS MC
          ON MC.KBNCD = '101'
         AND MC.SiteKBN = DH.SiteKBN
        LEFT OUTER JOIN M_MultiPorpose AS MM1   --�����ꔻ�f�p��M�ėp
          ON MM1.ID = 230 AND MM1.[Key] = DH.ZipCD1    

        WHERE DH.JuchuuKBN = 1  --�󒍎�ʋ敪              
          AND DH.CustomerCD IS NOT NULL
          AND DM.DeliverySu <> DM.JuchuuSuu
          --AND DM.CancelDate IS NULL
          AND DH.DeleteDateTime IS NULL
            ;
            
    DECLARE @JuchuuNO varchar(11);
    DECLARE @AttentionFLG tinyint;
    DECLARE @ConfirmFLG tinyint;
    DECLARE @DirectFLG tinyint;
    DECLARE @Address1   varchar(100);
    DECLARE @Address2   varchar(100);
    DECLARE @CHK_Address2 tinyint;
    DECLARE @M_Address1   varchar(100);
    DECLARE @M_Address2   varchar(100);
    DECLARE @JuchuuGaku money;
    DECLARE @UnpaidAmount money;
    DECLARE @UnpaidCount int;
    DECLARE @ConfirmKBN tinyint;
    DECLARE @CardProgressKBN tinyint;
    DECLARE @SpecialFlg tinyint;
    DECLARE @PaymentProgressKBN tinyint;
    DECLARE @H_CommentCustomer   varchar(400);
    DECLARE @H_CommentCapital   varchar(400);
    DECLARE @CommentCustomer   varchar(400);
    DECLARE @CommentCapital   varchar(400);
    DECLARE @IncludeFLG tinyint;
    DECLARE @SpecFLG   tinyint;
    DECLARE @NoshiFLG  tinyint;
    DECLARE @GiftFLG   tinyint;
    DECLARE @CustomerName   varchar(80);
    DECLARE @DeliveryName   varchar(80);
    DECLARE @KaolaetcFLG   tinyint;
    DECLARE @Zip   varchar(100);
    DECLARE @JuchuuDate date;
    DECLARE @AnswerFLG tinyint;
    DECLARE @ArrivePlanDate  date;
    DECLARE @Nissu  int;
    DECLARE @WRK_HoryuFLG tinyint;
    
    --�J�[�\���I�[�v��
    OPEN CUR_Juchu;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO
        ,@AttentionFLG
        ,@ConfirmFLG
        ,@DirectFLG
        ,@Address1
        ,@Address2
        ,@CHK_Address2
        ,@M_Address1
        ,@M_Address2
        ,@JuchuuGaku
        ,@UnpaidAmount
        ,@UnpaidCount
        ,@ConfirmKBN
        ,@CardProgressKBN
        ,@SpecialFlg
        ,@PaymentProgressKBN
        ,@H_CommentCustomer
        ,@H_CommentCapital
        ,@CommentCustomer
        ,@CommentCapital
        ,@IncludeFLG
        ,@SpecFLG
        ,@NoshiFLG
        ,@GiftFLG
        ,@CustomerName
        ,@DeliveryName
        ,@KaolaetcFLG
        ,@Zip
        ,@JuchuuDate
        ,@AnswerFLG
        ,@ArrivePlanDate
        ,@Nissu;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===*************************CUR_Stock
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO
	        ,@AttentionFLG
	        ,@ConfirmFLG
	        ,@DirectFLG
	        ,@Address1
	        ,@Address2
	        ,@CHK_Address2
	        ,@M_Address1
	        ,@M_Address2
	        ,@JuchuuGaku
	        ,@UnpaidAmount
	        ,@UnpaidCount
	        ,@ConfirmKBN
	        ,@CardProgressKBN
	        ,@SpecialFlg
	        ,@PaymentProgressKBN
	        ,@H_CommentCustomer
	        ,@H_CommentCapital
	        ,@CommentCustomer
	        ,@CommentCapital
	        ,@IncludeFLG
	        ,@SpecFLG
	        ,@NoshiFLG
	        ,@GiftFLG
	        ,@CustomerName
	        ,@DeliveryName
	        ,@KaolaetcFLG
	        ,@Zip
	        ,@JuchuuDate
	        ,@AnswerFLG
	        ,@ArrivePlanDate
	        ,@Nissu;

        --1.�󒍃��[�N��1�����[�h

        SET @W_ERR = 0;
        
        --0 �� WRK_�ۗ�FLG ��WRK_�ۗ�FLG �̓v���O���������[�J���ϐ�
        SET @WRK_HoryuFLG = 0;

        --2.�e�ۗ��`�F�b�N
        --�@�v���ӌڋq
        IF @AttentionFLG = 1
        BEGIN
            --D�󒍃��[�N.���v���ӌڋq �� 1 �̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'002'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
		END
        
        --�A�v�m�F�ڋq
        IF @WRK_HoryuFLG = 0 AND @ConfirmFLG = 1
        BEGIN
            --D�󒍃��[�N.�v�m�F�ڋq �� 1 �̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'003'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END

        --�B����)�Z���s���S
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND @CHK_Address2 = 0 
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�Z���Q �ɐ���(1,2,3,4,5,6,7,8,9,0�̂����ꂩ)���܂܂�Ă��Ȃ���
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'004'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�C����)�X�֔ԍ��s����
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND ISNULL(@Address1,'') <> ISNULL(@M_Address1,'') 
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�Z���P <> D�󒍃��[�N.�Z���P_�l�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'005'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�D����)�敥���������i3���ȏ�j
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidAmount,0)+@JuchuuGaku >= 30000
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and D�󒍃��[�N.�����󋵋敪 �� 0(������) and�@D�󒍃��[�N.�������z + D�󒍃��[�N.�󒍑��z �� 30000�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'006'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
        END
        
        --�E����)�敥���������i3���ȏ�j
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidCount,0)+1 >= 3
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and�@�󒍃��[�N.�����󋵋敪 �� 0(������) and�@D�󒍃��[�N.���������� + 1 �� 3�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'007'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�F�v�m�F���i
        IF @WRK_HoryuFLG = 0 AND ISNULL(@ConfirmKBN,0) = 1
        BEGIN
        	--D�󒍃��[�N.�v�m�F���i �� 1�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'008'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
        END
        
        --�G����)�J�[�h���ϕs��	
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND @CardProgressKBN <> 0
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�J�[�h�� <> 0�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'009'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                
        END
        
        
        --�H����)�敥��������
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 1 AND ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0
        BEGIN
            --D�󒍃��[�N.����FLG �� 1(:����)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and�@D�󒍃��[�N.�����󋵋敪 �� 0(������)�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'010'
                ,@Operator 
                ,@SYSDATETIME 
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�I�󒍔��l����
        IF @WRK_HoryuFLG = 0 AND ISNULL(@H_CommentCustomer,'') = '' AND ISNULL(@H_CommentCapital,'') = '' 
           AND ISNULL(@CommentCustomer,'') = '' AND ISNULL(@CommentCapital,'') = '' 
        BEGIN
            --D�󒍃��[�N.�󒍃R�����g�ڋq_�g is NULL�@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^��_�g is NULL�@����
            --D�󒍃��[�N.�󒍃R�����g�ڋq is NULL�@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^�� is NULL
        	SELECT NULL;
        END
        ELSE
        BEGIN
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'011'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�J�����L���m�F
        IF @WRK_HoryuFLG = 0 AND CHARINDEX('����',@H_CommentCustomer) = 0 AND CHARINDEX('����',@H_CommentCapital) = 0 
           AND CHARINDEX('����',@CommentCustomer) = 0 AND CHARINDEX('����',@CommentCapital) = 0 
           AND ISNULL(@IncludeFLG,0) = 0
        BEGIN
            --D�󒍃��[�N.�󒍃R�����g�ڋq_�g �Ɂu�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^��_�g �Ɂu�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�ڋq �Ɂu�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^�� �Ɂu�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃X�e�[�^�X.�����L��FLG �� 0
            SELECT NULL;
        END
        ELSE
        BEGIN
        	--D�󒍃X�e�[�^�X.�����L��FLG�Ɂu1�v���Z�b�g��Update�B
		    UPDATE D_JuchuuStatus SET
		       [IncludeFLG] = 1
		      ,[UpdateOperator] = @Operator
		      ,[UpdateDateTime] = @SYSDATETIME
		    WHERE JuchuuNO = @JuchuuNO
		    ;
		    
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'012'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�K�X�y�b�N�v���L��
        IF @WRK_HoryuFLG = 0 AND CHARINDEX('�X�y�b�N�v��',@H_CommentCustomer) = 0 AND CHARINDEX('�X�y�b�N�v��',@H_CommentCapital) = 0 
           AND CHARINDEX('�X�y�b�N�v��',@CommentCustomer) = 0 AND CHARINDEX('�X�y�b�N�v��',@CommentCapital) = 0 
           AND ISNULL(@SpecFLG,0) = 0
        BEGIN
            --D�󒍃��[�N.�󒍃R�����g�ڋq_�g �Ɂu�X�y�b�N�v���v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^��_�g �Ɂu�X�y�b�N�v���v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�ڋq �Ɂu�X�y�b�N�v���v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^�� �Ɂu�X�y�b�N�v���v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃X�e�[�^�X.�X�y�b�N�v���L��FLG �� 0
            SELECT NULL;
        END
        ELSE
        BEGIN
            --D�󒍃X�e�[�^�X.�X�y�b�N�v���L��FLG�Ɂu1�v���Z�b�g��Update�B
		    UPDATE D_JuchuuStatus SET
		       [SpecFLG] = 1
		      ,[UpdateOperator] = @Operator
		      ,[UpdateDateTime] = @SYSDATETIME
		    WHERE JuchuuNO = @JuchuuNO
		    ;
		    
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'013'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�L�u�̂��v�̗L���m�F
        IF @WRK_HoryuFLG = 0 AND CHARINDEX('�̂�����',@H_CommentCustomer) = 0 AND CHARINDEX('�̂�����',@H_CommentCapital) = 0 
           AND CHARINDEX('�̂�����',@CommentCustomer) = 0 AND CHARINDEX('�̂�����',@CommentCapital) = 0 
           AND ISNULL(@NoshiFLG,0) = 0
        BEGIN
            --D�󒍃��[�N.�󒍃R�����g�ڋq_�g �Ɂu�̂�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^��_�g �Ɂu�̂�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�ڋq �Ɂu�̂�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^�� �Ɂu�̂�����v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃X�e�[�^�X.�̂��Ώ�FLG �� 0
            SELECT NULL;
        END
        ELSE
        BEGIN
        	--D�󒍃X�e�[�^�X.�̂��Ώ�FLG�Ɂu1�v���Z�b�g��Update�B
		    UPDATE D_JuchuuStatus SET
		       [NoshiFLG] = 1
		      ,[UpdateOperator] = @Operator
		      ,[UpdateDateTime] = @SYSDATETIME
		    WHERE JuchuuNO = @JuchuuNO
		    ;
		    
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'014'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�M�u�M�t�g�v�̗L���m�F
        IF @WRK_HoryuFLG = 0 AND CHARINDEX('���b�s���O',@H_CommentCustomer) = 0 AND CHARINDEX('���b�s���O',@H_CommentCapital) = 0 
           AND CHARINDEX('���b�s���O',@CommentCustomer) = 0 AND CHARINDEX('���b�s���O',@CommentCapital) = 0 
           AND ISNULL(@GiftFLG,0) = 0
        BEGIN
            --D�󒍃��[�N.�󒍃R�����g�ڋq_�g �Ɂu���b�s���O�v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^��_�g �Ɂu���b�s���O�v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�ڋq �Ɂu���b�s���O�v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃��[�N.�󒍃R�����g�L���s�^�� �Ɂu���b�s���O�v�Ƃ����������܂܂�Ȃ��@����
            --D�󒍃X�e�[�^�X.�M�t�g�Ώ�FLG �� 0
            SELECT NULL;
        END
        ELSE
        BEGIN
        	--D�󒍃X�e�[�^�X.�M�t�g�Ώ�FLG�Ɂu1�v���Z�b�g��Update�B
		    UPDATE D_JuchuuStatus SET
		       [GiftFLG] = 1
		      ,[UpdateOperator] = @Operator
		      ,[UpdateDateTime] = @SYSDATETIME
		    WHERE JuchuuNO = @JuchuuNO
		    ;
		    
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'015'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�N�˗��偂������
        IF @WRK_HoryuFLG = 0 AND ISNULL(@CustomerName,'') <> ISNULL(@DeliveryName,'')
        BEGIN
            --D�󒍃��[�N.�ڋq���@���@D�󒍃��[�N.�z���於�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'016'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�O�y�VKaola���`�F�b�N
        IF @WRK_HoryuFLG = 0 AND ISNULL(@KaolaetcFLG,'') <> 0
        BEGIN
            --D�󒍃��[�N.�R�A����FLG�@���@0�@�̎��ȊO
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'017'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
                                    
        END
        
        --�P�����悪����
        IF @WRK_HoryuFLG = 0 AND ISNULL(@Zip,'') <> ''
        BEGIN
            --D�󒍃��[�N.�����^�P IS NOT NULL�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'018'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
        END

		--�Q�����悪�C�O
        IF @WRK_HoryuFLG = 0 AND CHARINDEX('�s',@Address1) = 0 AND CHARINDEX('��',@Address1) = 0 
           AND CHARINDEX('�{',@Address1) = 0 AND CHARINDEX('��',@Address1) = 0 
        BEGIN
            --D�󒍃��[�N.�Z���P �Ɂu�s�v�u���v�u�{�v�u���v�̑S�Ă̕������܂܂�Ȃ���
            SELECT NULL;
        END
        ELSE
        BEGIN
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'019'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;
             
        END
        
        --�R�񓚔[������t
        IF @WRK_HoryuFLG = 0 AND @AnswerFLG = 1 AND @ArrivePlanDate IS NOT NULL
           AND DATEDIFF(day,@JuchuuDate, @ArrivePlanDate) >= ISNULL(@Nissu,0)
        BEGIN
            --D�󒍃��[�N.�񓚔[���o�^���聁1 ���� D�󒍃��[�N.���ח\��� IS NOT NULL�@�̎�
            --DATEDIFF�֐��ŁAD�󒍃��[�N.�󒍓���D�󒍃��[�N.���ח\����̓��������� �� D�󒍃��[�N.�����^1�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'020'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --�S���[���A�h���X�G���[
        IF @WRK_HoryuFLG = 0 
        BEGIN
        	SELECT NULL;
        END
        
        --21�Z���s���S
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND @CHK_Address2 = 0 
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�Z���Q �ɐ���(1,2,3,4,5,6,7,8,9,0�̂����ꂩ)���܂܂�Ă��Ȃ���
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'022'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --22�X�֔ԍ��s����
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND @Address1 <> ISNULL(@M_Address1,'') 
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�Z���P <> D�󒍃��[�N.�Z���P_�l�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'023'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --23�敥���������i3���ȏ�j
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidAmount,0)+@JuchuuGaku >= 30000
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and D�󒍃��[�N.�����󋵋敪 �� 0(������) and�@D�󒍃��[�N.�������z + D�󒍃��[�N.�󒍑��z �� 30000�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'024'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --24�敥���������i3���ȏ�j
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 AND ISNULL(@UnpaidCount,0)+1 >= 3
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and�@�󒍃��[�N.�����󋵋敪 �� 0(������) and�@D�󒍃��[�N.���������� + 1 �� 3�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'025'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --25�J�[�h���ϕs��
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND @CardProgressKBN <> 0
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�J�[�h�� <> 0�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'026'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --26�敥��������
        IF @WRK_HoryuFLG = 0 AND @DirectFLG = 0 AND  ISNULL(@SpecialFlg,0) = 1 AND @PaymentProgressKBN = 0 
        BEGIN
            --D�󒍃��[�N.����FLG �� 0(:�����ł͂Ȃ�)�@����
            --D�󒍃��[�N.�ݒ�l �� 1�@and�@D�󒍃��[�N.�����󋵋敪 �� 0(������)�@�̎�
            --��LSelect������0���̎��A�e�[�u���]���d�l�`�ɏ]����D�󒍕ۗ��x��(D_JuchuuOnHold)�̃��R�[�h�ǉ��B
            EXEC PRC_JuchuuDataCheck_Sub
                @JuchuuNO
                ,'027'
                ,@Operator 
                ,@SYSDATETIME
                ,@WRK_HoryuFLG OUTPUT
                ;

        END
        
        --27
        --D��.�ۗ�FLG�ɁuWRK_�ۗ�FLG�v���Z�b�g��Update�B
        UPDATE D_Juchuu SET
           [OnHoldFLG] = @WRK_HoryuFLG
          ,[UpdateOperator] = @Operator
          ,[UpdateDateTime] = @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        ;
        
        --D�󒍃X�e�[�^�X.�ۗ�FLG�ɁuWRK_�ۗ�FLG�v���Z�b�g��Update�B
        UPDATE D_JuchuuStatus SET
           [OnHoldFLG] = @WRK_HoryuFLG
          ,[UpdateOperator] = @Operator
          ,[UpdateDateTime] = @SYSDATETIME
        WHERE JuchuuNO = @JuchuuNO
        ;
            
        --���́u1.�󒍃��[�N��1�����[�h�v�ցB
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
    SET @KeyItem = NULL;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'JuchuuDataCheck',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutErrNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
