

BEGIN TRY 
 Drop PROCEDURE dbo.[D_MoveRequest_SelectDataForIdouIrai]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRY 
 Drop PROCEDURE dbo.[PRC_ZaikoIdouIraiNyuuryoku]
END try
BEGIN CATCH END CATCH 
DROP TYPE [dbo].[T_IdoIrai]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





--  ======================================================================
--       Program Call    �݌Ɉړ��˗�����
--       Program ID      ZaikoIdouIraiNyuuryoku
--       Create date:    2019.12.11
--    ======================================================================
CREATE PROCEDURE D_MoveRequest_SelectDataForIdouIrai
    (@RequestNO varchar(11)
    )AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

--        IF @OperateMode = 2   --�C����
--        BEGIN
    SELECT DH.RequestNO
          ,DH.StoreCD
          ,CONVERT(varchar,DH.RequestDate,111) AS RequestDate
          ,DH.MovePurposeKBN
          ,DH.FromStoreCD
          ,DH.FromSoukoCD
          ,DH.ToStoreCD
          ,DH.ToSoukoCD
          ,DH.StaffCD
          ,DH.AnswerDateTime
          ,DH.AnswerStaffCD
          ,DH.InsertOperator
          ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
          ,DH.UpdateOperator
          ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
          ,DH.DeleteOperator
          ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
          ,DM.RequestRows
          ,DM.SKUCD
          ,DM.AdminNO
          ,DM.JanCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DH.RequestDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
          
          ,DM.RequestSu
          ,CONVERT(varchar,DM.ExpectedDate,111) AS ExpectedDate
          ,DM.CommentInStore

      FROM D_MoveRequest DH
      LEFT OUTER JOIN D_MoveRequestDetailes AS DM 
      ON DH.RequestNO = DM.RequestNO 
      AND DM.DeleteDateTime IS NULL                     
      
      WHERE DH.RequestNO = @RequestNO           
      AND DH.DeleteDateTime IS Null
      ORDER BY DM.RequestRows
      ;
      
END

GO

CREATE TYPE T_IdoIrai AS TABLE
    (
    [RequestRows] [int],

    [SKUCD] [varchar](30) ,
    [AdminNO] [int] ,
    [JanCD] [varchar](13) ,
    [RequestSu] [int] ,
    [OldRequestSu] [int] ,

    [ExpectedDate] date,
    [CommentInStore] varchar(80),
    [UpdateFlg][tinyint]
    )
GO

CREATE PROCEDURE PRC_ZaikoIdouIraiNyuuryoku
   (@OperateMode    int,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    @RequestNO   varchar(11),
    @StoreCD   varchar(4),
    @MovePurposeKBN tinyint,
--    @MovePurposeType tinyint,
    @RequestDate  varchar(10),
    @FromStoreCD varchar(4),
    @FromSoukoCD varchar(6),
    @ToStoreCD varchar(4),
    @ToSoukoCD varchar(6),
    @StaffCD   varchar(10),

    @Table  T_IdoIrai READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutRequestNO varchar(11) OUTPUT
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
    DECLARE @Program varchar(100); 
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @Program = 'ZaikoIdouIraiNyuuryoku';    
    
    DECLARE @KBN_TENPOKAN tinyint;
    SET @KBN_TENPOKAN = 1;

    DECLARE CUR_Store CURSOR FOR
        SELECT top 1 A.MailAddress1, A.MailAddress2, A.MailAddress3
        FROM M_Store AS A
        WHERE A.StoreCD = @ToStoreCD 
        AND A.DeleteFlg = 0 
        AND A.ChangeDate <= @RequestDate
        ORDER BY A.ChangeDate desc
        ;
    
	DECLARE @MailAddress1 varchar(100);
    DECLARE @MailAddress2 varchar(100);
    DECLARE @MailAddress3 varchar(100);
    DECLARE @Rows int;
    DECLARE @MailCounter int;

    DECLARE @MailFlg tinyint;
    SET @MailFlg = (SELECT M.MailFlg FROM M_MovePurpose AS M WHERE M.MovePurposeKBN = 1);

    --�V�K--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '�V�K';

        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            24,             --in�`�[��� 24
            @RequestDate, --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @RequestNO OUTPUT
            ;
        
        IF ISNULL(@RequestNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --�yD_MoveRequest�z�ړ��˗��@Table�]���d�l�`
        INSERT INTO [D_MoveRequest]
           ([RequestNO]
           ,[StoreCD]
           ,[RequestDate]
           ,[MovePurposeKBN]
           ,[FromStoreCD]
           ,[FromSoukoCD]
           ,[ToStoreCD]
           ,[ToSoukoCD]
           ,[RequestInputDateTime]
           ,[StaffCD]
           ,[AnswerDateTime]
           ,[AnswerStaffCD]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     	VALUES
           (@RequestNO
           ,@StoreCD
           ,convert(date,@RequestDate)
           ,@MovePurposeKBN
           ,@FromStoreCD
           ,@FromSoukoCD
           ,@ToStoreCD
           ,@ToSoukoCD
           ,SYSDATETIME()	--RequestInputDateTime
           ,@StaffCD
           ,NULL	--AnswerDateTime
           ,NULL	--AnswerStaffCD

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               
	END
	
	IF @OperateMode <= 2	--�V�K�E�C���@�ǉ��s�̂��߂̍X�V
	BEGIN
        --�yD_MoveRequestDetailes�z�ړ��˗����ׁ@Table�]���d�l�a
        INSERT INTO [D_MoveRequestDetailes]
                   ([RequestNO]
                   ,[RequestRows]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[RequestSu]
                   ,[ExpectedDate]
                   ,[CommentInStore]
                   ,[AnswerKBN]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @RequestNO                         
                   ,tbl.RequestRows                       
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.RequestSu
                   ,tbl.ExpectedDate
                   ,tbl.CommentInStore
                   ,0	--AnswerKBN
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
	END
	
    --�V�K--
    IF @OperateMode = 1
    BEGIN
        --MailFlg = 1�̏ꍇ
        IF @MailFlg = 1
        BEGIN
            --�yD_Mail�z���[���A�����e�@Table�]���d�l�b
            INSERT INTO [D_Mail]
               ([MailCounter]
               ,[MailType]
               ,[MailKBN]
               ,[Number]
               ,[MailNORows]
               ,[MailDateTime]
               ,[StaffCD]
               ,[ContactKBN]
               ,[MailPatternCD]
               ,[MailSubject]
               ,[MailPriority]
               ,[ReMailFlg]
               ,[UnitKBN]
               ,[SendedDateTime]
               ,[SenderKBN]
               ,[SenderCD]
               ,[SenderAddress]
               ,[MailContent]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
            VALUES(
                ISNULL((SELECT MAX(A.MailCounter) FROM D_Mail AS A),0) +1 
               ,9   --MailType
               ,51  --MailKBN
               ,@RequestNO  --Number
               ,1   --MailNORows
               ,@SYSDATETIME    --MailDateTime
               ,@StaffCD
               ,1   --ContactKBN
               ,(SELECT top 1 A.MoveMailPatternCD FROM F_Store(@RequestDate) AS A
                  WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)  
               ,(SELECT top 1 B.MailSubject 
                    FROM F_Store(@RequestDate) AS A
                    INNER JOIN M_MailPattern AS B
                    ON B.MailPatternCD = A.MoveMailPatternCD
                   WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)   --MailSubject
               ,(SELECT top 1 B.MailPriority 
                    FROM F_Store(@RequestDate) AS A
                    INNER JOIN M_MailPattern AS B
                    ON B.MailPatternCD = A.MoveMailPatternCD
                  WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)   --MailPriority
               ,0   --ReMailFlg
               ,2   --UnitKBN
               ,NULL    --SendedDateTime
               ,1   --SenderKBN
               ,@ToStoreCD   --SenderCD
               ,(SELECT A.SenderAddress FROM M_MailServer A WHERE A.SenderKBN = 1 AND A.SenderCD = @ToStoreCD)  --SenderAddress
               ,(SELECT top 1 B.MailText 
                   FROM F_Store(@RequestDate) AS A
                  INNER JOIN M_MailPattern AS B
                     ON B.MailPatternCD = A.MoveMailPatternCD
                  WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)   --MailContent

               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               );

            --�yD_MailAddress�z���[���A������@Table�]���d�l�c
            SET @MailCounter = (SELECT MAX(A.MailCounter) FROM D_Mail AS A WHERE A.[Number] = @RequestNO);
            SET @Rows = 1;
            
            --���א���Insert��
            --�J�[�\���I�[�v��
            OPEN CUR_Store;

            --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_Store
            INTO @MailAddress1, @MailAddress2, @MailAddress3;
            
            --�f�[�^�̍s�������[�v���������s����
            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- ========= ���[�v���̎��ۂ̏��� ��������===
                IF ISNULL(@MailAddress1,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         1  --@KBN ,
                         ,@MailAddress1
                         ,@Rows
                         ,@MailCounter
                        ;
                    
                    SET @Rows = @Rows + 1;
                END
                IF ISNULL(@MailAddress2,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         1  --@KBN ,
                         ,@MailAddress2
                         ,@Rows
                         ,@MailCounter 
                        ;
                    
                    SET @Rows = @Rows + 1;
                END
                IF ISNULL(@MailAddress3,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         1  --@KBN ,
                         ,@MailAddress3
                         ,@Rows
                         ,@MailCounter
                        ;
                END
                --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_Store
                INTO @MailAddress1, @MailAddress2, @MailAddress3;
            END            --LOOP�̏I���
            
            --�J�[�\�������
            CLOSE CUR_Store;
            DEALLOCATE CUR_Store;
		END
    END
    
    --�ύX--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '�ύX';
        
        --�yD_MoveRequest�z�݌Ɉړ��˗��@Table�]���d�l�`
        UPDATE [D_MoveRequest]
           SET [StoreCD]        = @StoreCD                         
              ,[RequestDate]    = convert(date,@RequestDate)
              ,[MovePurposeKBN] = @MovePurposeKBN
              ,[FromStoreCD]    = @FromStoreCD
              ,[FromSoukoCD]    = @FromSoukoCD
              ,[ToStoreCD]      = @ToStoreCD
              ,[ToSoukoCD]      = @ToSoukoCD
              ,[StaffCD]        = @StaffCD  
              ,[UpdateOperator] =  @Operator  
              ,[UpdateDateTime] =  @SYSDATETIME
         WHERE RequestNO = @RequestNO
           ;

        --�yD_MoveRequestDetailes�z�݌Ɉړ��˗����ׁ@Table�]���d�l�a�@
        UPDATE [D_MoveRequestDetailes]
           SET  [SKUCD]          = tbl.SKUCD
               ,[AdminNO]        = tbl.AdminNO
               ,[JanCD]          = tbl.JanCD
               ,[RequestSu]      = tbl.RequestSu
               ,[ExpectedDate]   = tbl.ExpectedDate
               ,[CommentInStore] = tbl.CommentInStore    
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
        FROM D_MoveRequestDetailes
        INNER JOIN @Table tbl
         ON @RequestNO       = D_MoveRequestDetailes.RequestNO
         AND tbl.RequestRows = D_MoveRequestDetailes.RequestRows
         AND tbl.UpdateFlg   = 1
         ;

        --�폜�s
        --�yD_MoveRequestDetailes�z�݌Ɉړ��˗����ׁ@Table�]���d�l�a�@
        UPDATE [D_MoveRequestDetailes]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
        FROM D_MoveRequestDetailes
        INNER JOIN @Table tbl
           ON @RequestNO      = D_MoveRequestDetailes.RequestNO
          AND tbl.RequestRows = D_MoveRequestDetailes.RequestRows
          AND tbl.UpdateFlg   = 2
         ;
        
        --MailFlg = 1�̏ꍇ
        IF @MailFlg = 1
        BEGIN
            --�yD_Mail�z���[���A�����e�@Table�]���d�l�b
            UPDATE [D_Mail]
            SET [MailType]     = 9
               ,[MailKBN]      = 51
               ,[Number]       = @RequestNO
              -- ,[MailNORows] = (SELECT MAX(A.MailNORows) FROM D_Mail AS A WHERE A.[Number] = @RequestNO) + 1
               ,[MailDateTime] = @SYSDATETIME
               ,[StaffCD]      = @StaffCD
               ,[ContactKBN]   = 1
               ,[MailPatternCD] = (SELECT top 1 A.MoveMailPatternCD FROM F_Store(@RequestDate) AS A
                                  WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0) 
               ,[MailSubject] = (SELECT top 1 B.MailSubject 
                                   FROM F_Store(@RequestDate) AS A
                                  INNER JOIN M_MailPattern AS B
                                     ON B.MailPatternCD = A.MoveMailPatternCD
                                  WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)
               ,[MailPriority] = (SELECT top 1 B.MailPriority 
                                    FROM F_Store(@RequestDate) AS A
                                   INNER JOIN M_MailPattern AS B
                                      ON B.MailPatternCD = A.MoveMailPatternCD
                                   WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)
               ,[ReMailFlg]      = 1
               ,[UnitKBN]        = 2
               ,[SendedDateTime] = NULL
               ,[SenderKBN]      = 1
               ,[SenderCD]       = @ToStoreCD
               ,[SenderAddress]  = (SELECT A.SenderAddress FROM M_MailServer A WHERE A.SenderKBN = 1 AND A.SenderCD = @ToStoreCD)
               ,[MailContent]    = (SELECT top 1 B.MailText 
                                      FROM F_Store(@RequestDate) AS A
                                     INNER JOIN M_MailPattern AS B
                                        ON B.MailPatternCD = A.MoveMailPatternCD
                                     WHERE A.StoreCD = @ToStoreCD AND A.DeleteFlg = 0)
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
            WHERE [Number]  = @RequestNO
            AND MailCounter = (SELECT MAX(A.MailCounter) FROM D_Mail AS A WHERE A.[Number] = @RequestNO)
           -- AND MailNORows = (SELECT MAX(A.MailNORows) FROM D_Mail AS A WHERE A.[Number] = @RequestNO)
            ;

            --�yD_MailAddress�z���[���A������@Table�]���d�l�c
            SET @MailCounter = (SELECT MAX(A.MailCounter) FROM D_Mail AS A WHERE A.[Number] = @RequestNO);
            
            SET @Rows = 1;
            
            --���א���Insert��
            --�J�[�\���I�[�v��
            OPEN CUR_Store;

            --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_Store
            INTO @MailAddress1, @MailAddress2, @MailAddress3;
            
            --�f�[�^�̍s�������[�v���������s����
            WHILE @@FETCH_STATUS = 0
            BEGIN
                -- ========= ���[�v���̎��ۂ̏��� ��������===
                IF ISNULL(@MailAddress1,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         2  --@KBN ,
                         ,@MailAddress1
                         ,@Rows
                         ,@MailCounter
                        ;
                    
                    SET @Rows = @Rows + 1;
                END
                IF ISNULL(@MailAddress2,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         2  --@KBN ,
                         ,@MailAddress2
                         ,@Rows
                         ,@MailCounter 
                        ;
                    
                    SET @Rows = @Rows + 1;
                END
                IF ISNULL(@MailAddress3,'') <> ''
                BEGIN
                    EXEC INSERT_UPDATE_D_MailAddress
                         2  --@KBN ,
                         ,@MailAddress3
                         ,@Rows
                         ,@MailCounter
                        ;
                END
                --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_Store
                INTO @MailAddress1, @MailAddress2, @MailAddress3;
            END            --LOOP�̏I���
            
            --�J�[�\�������
            CLOSE CUR_Store;
            DEALLOCATE CUR_Store;
		END
    END    
  
    ELSE IF @OperateMode = 3 --�폜--
    BEGIN
        SET @OperateModeNm = '�폜';

        --�yD_MoveRequest�z�ړ��@�e�[�u���]���d�lA�A
        UPDATE [D_MoveRequest]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [RequestNO] = @RequestNO
         ;
             
        --�yD_MoveRequestDetailes�z�ړ����ׁ@Table�]���d�l�a�A
        UPDATE [D_MoveRequestDetailes]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [RequestNO] = @RequestNO
         AND [DeleteDateTime] IS NULL
         ;
    END
    
    --���������f�[�^�֍X�V
    SET @KeyItem = @RequestNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutRequestNO = @RequestNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO

BEGIN TRY 
 Drop PROCEDURE dbo.[INSERT_UPDATE_D_MailAddress]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [INSERT_UPDATE_D_MailAddress]
(
    @KBN tinyint,
    @MailAddress  varchar(100),
    @Rows int,
    @MailCounter  int
)AS
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN

    IF @KBN = 1
    BEGIN
        --�yD_MailAddress�z���[���A������@Table�]���d�l�c
        INSERT INTO [D_MailAddress]
           ([MailCounter]
           ,[AddressRows]
           ,[AddressKBN]
           ,[Address])
        VALUES(
            @MailCounter
           ,@Rows   --AddressRows
           ,1   --AddressKBN
           ,@MailAddress  --Address
           );
    END
    
    ELSE IF @KBN = 2
    BEGIN
        UPDATE [D_MailAddress]
        SET [AddressKBN] = 1
           ,[Address] = @MailAddress
        WHERE MailCounter = @MailCounter
        AND AddressRows = @Rows
        ;  
    END

END


GO