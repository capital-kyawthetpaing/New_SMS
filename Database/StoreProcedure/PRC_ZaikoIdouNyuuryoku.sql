

/****** Object:  StoredProcedure [dbo].[PRC_ZaikoIdouNyuuryoku]    Script Date: 2020/10/01 19:37:40 ******/
DROP PROCEDURE [dbo].[PRC_ZaikoIdouNyuuryoku]
GO

/****** Object:  StoredProcedure [dbo].[PRC_ZaikoIdouNyuuryoku]    Script Date: 2020/10/01 19:37:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--CREATE TYPE T_Ido AS TABLE
--    (
--    [MoveRows] [int],

--    [SKUCD] [varchar](30) ,
--    [AdminNO] [int] ,
--    [JanCD] [varchar](13) ,
--    [MoveSu] [int] ,
--    [OldMoveSu] [int] ,
--    [EvaluationPrice] [money] ,
--    [FromRackNO]  [varchar](11) ,
--    [ToRackNO]  [varchar](11) ,
--    [NewSKUCD] [varchar](30) ,
--    [NewAdminNO] [int] ,
--    [NewJanCD] [varchar](13) ,
--    [DeliveryPlanNO]  [varchar](11) ,
--    [ExpectReturnDate] date,
--    [VendorCD] varchar(13),
--    [CommentInStore] varchar(80),
--    [RequestRows] int,
--    [AnswerKBN] tinyint,

----    [StockNO] [varchar](11) ,
--    [ArrivalPlanNO] [varchar](11) ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_ZaikoIdouNyuuryoku]
   (@OperateMode    int,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    @MoveNO   varchar(11),
    @StoreCD   varchar(4),
    @RequestNO   varchar(11),
    @MovePurposeKBN tinyint,
    @MovePurposeType tinyint,
    @MoveDate  varchar(10),
    @FromStoreCD varchar(4),
    @FromSoukoCD varchar(6),
    @ToStoreCD varchar(4),
    @ToSoukoCD varchar(6),
    @StaffCD   varchar(10),

    @Table  T_Ido READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutMoveNO varchar(11) OUTPUT
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
    SET @Program = 'ZaikoIdouNyuuryoku';    
    
    DECLARE @KBN_TENPOKAN   tinyint;
    DECLARE @KBN_TENPONAI   tinyint;
    DECLARE @KBN_SYOCD      tinyint;
    DECLARE @KBN_CHOSEI_ADD tinyint;
    DECLARE @KBN_CHOSEI_DEL tinyint;
    DECLARE @KBN_LOCATION   tinyint;  
    DECLARE @KBN_HENPIN     tinyint;

    SET @KBN_TENPOKAN   = 1;
    SET @KBN_TENPONAI   = 2;
    SET @KBN_SYOCD      = 3;
    SET @KBN_CHOSEI_ADD = 4;
    SET @KBN_CHOSEI_DEL = 5;
    SET @KBN_LOCATION   = 6;  
    SET @KBN_HENPIN     = 7;
    
    DECLARE @ReserveNO       varchar(11);
    DECLARE @StockNO         varchar(11);
    DECLARE @DeliveryPlanNO  varchar(11);
    DECLARE @ArrivalPlanNO   varchar(11);
    DECLARE @NewMoveNO       varchar(11);
    
    --�J�[�\����`
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.MoveRows, tbl.FromRackNO, tbl.AdminNO, tbl.MoveSu, tbl.UpdateFlg
        FROM @Table AS tbl
        ORDER BY tbl.MoveRows
        ;
    DECLARE @tblMoveRows int;
    DECLARE @tblFromRackNO varchar(11);
    DECLARE @tblAdminNO int;
    DECLARE @tblMoveSu int;
    DECLARE @tblUpdateFlg int;
        
    --�ړ��˗�����*******************************************************************************
    IF ISNULL(@RequestNO,' ') <>' '
    BEGIN        
        --Form.Detail Display  Area. AnswerKBN��0�̍s�̂ݍX�V����B
        --1�܂���9�̉񓚍ς̍s�͍X�V���Ȃ��B
        --�yD_MoveRequestDetailes�z�ړ��˗����׍X�V�@�e�[�u���]���d�lF�@
        UPDATE [D_MoveRequestDetailes] SET
             [AnswerKBN]          = (CASE @OperateMode WHEN 3 THEN 0 ELSE tbl.AnswerKBN END)	--1:����A9:����
            ,[UpdateOperator]     =  @Operator  
            ,[UpdateDateTime]     =  @SYSDATETIME
        FROM @Table tbl
        WHERE tbl.RequestRows = D_MoveRequestDetailes.RequestRows
        AND D_MoveRequestDetailes.RequestNO = @RequestNO
        AND (D_MoveRequestDetailes.AnswerKBN = 0 OR @OperateMode = 3)
        ;
        
        --�yD_MoveRequest�z�ړ��˗��X�V�@�e�[�u���]���d�lE�@
        UPDATE [D_MoveRequest] SET
             [AnswerDateTime] = (CASE (SELECT SUM(DM.AnswerKBN) FROM D_MoveRequestDetailes AS DM
                                       WHERE DM.RequestNO = @RequestNO
                                       AND DM.DeleteDateTime IS NULL)
                                       WHEN 0 THEN NULL
                                       ELSE convert(datetime,@MoveDate) END)
            ,[AnswerStaffCD] = (CASE (SELECT SUM(DM.AnswerKBN) FROM D_MoveRequestDetailes AS DM
                                      WHERE DM.RequestNO = @RequestNO
                                      AND DM.DeleteDateTime IS NULL)
                                      WHEN 0 THEN NULL
                                      ELSE @StaffCD END)
            ,[UpdateOperator]     =  @Operator  
            ,[UpdateDateTime]     =  @SYSDATETIME  
        WHERE RequestNO = @RequestNO
        ;
    END
        
    --�ύX�E�폜--*******************************************************************************�ύX
    IF @OperateMode >= 2
    BEGIN
    	IF @OperateMode = 2
    	BEGIN
        	SET @OperateModeNm = '�ύX';
        END
        
        --�ړ��敪=�X�܊Ԉړ�
        IF @MovePurposeType = @KBN_TENPOKAN
        BEGIN
        
            --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�lG�A
            --�ړ������݌ɂň������ԃf�[�^�쐬
            --�J�[�\����`(D_Stock_SelectSuryo�Q��)
            DECLARE CUR_Stock_G2 CURSOR FOR
                SELECT DS.StockNO
                      ,tbl.OldMoveSu
                 FROM D_Stock AS DS
                 INNER JOIN D_Warehousing AS DW
                 ON DW.WarehousingKBN = 90   --WarehousingKBN
                 AND DW.DeleteFlg = 0
                 AND DW.DeleteDateTime IS NULL
                 AND DW.[Number] = @MoveNO
                 AND DW.StockNO = DS.StockNO
                  
                  INNER JOIN @Table tbl 
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	�s�폜�f�[�^���ԃf�[�^�͍쐬���Ȃ���΂Ȃ�Ȃ�
                 WHERE DS.DeleteDateTime IS NULL
                ;
            DECLARE @OldMoveSu int;
            
            OPEN CUR_Stock_G2;

            --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_Stock_G2
            INTO @StockNO, @OldMoveSu;
            
            --�f�[�^�̍s�������[�v���������s����
            WHILE @@FETCH_STATUS = 0
            BEGIN
                --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�l�f�A
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] + @OldMoveSu
                      ,[AllowableSu] = [AllowableSu] + @OldMoveSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + @OldMoveSu
                      --,[ReturnSu]           = [ReturnSu] + @OldMoveSu		2020.09.18 del
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                 WHERE DeleteDateTime IS NULL
                 AND StockNO = @StockNO
                  ;
                
                --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_Stock_G2
                INTO @StockNO, @OldMoveSu;
            END		--LOOP�̏I���***************************************CUR_Stock
            
            --�J�[�\�������
            CLOSE CUR_Stock_G2;
            DEALLOCATE CUR_Stock_G2;
	        
            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b�A�ԁ@�i�b�ɑ΂���ԃf�[�^�@WarehousingKBN��11�j            
            --C�A(90)
            INSERT INTO [D_Warehousing]
               ([WarehousingDate]
               ,[SoukoCD]
               ,[RackNO]
               ,[StockNO]
               ,[JanCD]
               ,[AdminNO]
               ,[SKUCD]
               ,[WarehousingKBN]
               ,[DeleteFlg]
               ,[Number]
               ,[NumberRow]
               ,[VendorCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[ToRackNO]
               ,[ToStockNO]
               ,[FromStoreCD]
               ,[FromSoukoCD]
               ,[FromRackNO]
               ,[CustomerCD]
               ,[Quantity]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT DW.WarehousingDate
               ,DW.SoukoCD
               ,DW.RackNO
               ,DW.StockNO          --(D_Stock)��(�ړ���)�Ɠ����l
               ,tbl.JanCD
               ,tbl.AdminNO
               ,tbl.SKUCD
               ,90   --WarehousingKBN
               ,1    --DeleteFlg
               ,@MoveNO  --Number��
               ,tbl.MoveRows --NumberRow��
               ,NULL    --VendorCD
               ,DW.ToStoreCD
               ,DW.ToSoukoCD
               ,DW.ToRackNO
               ,DW.ToStockNO
               ,DW.FromStoreCD
               ,DW.FromSoukoCD
               ,DW.FromRackNO
               ,NULL    --CustomerCD
               ,DW.Quantity * (-1)
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              INNER JOIN D_Warehousing AS DW
              ON DW.WarehousingKBN = 90
              AND DW.DeleteFlg = 0
              AND DW.DeleteDateTime IS NULL
              AND DW.[Number] = @MoveNO
              AND DW.NumberRow = tbl.MoveRows
              ;
            
            --�ԃf�[�^�̌��̍��f�[�^��DeleteFlg��1��UPDATE
            UPDATE D_Warehousing SET
                DeleteFlg = 1
                ,UpdateOperator = @Operator
                ,UpdateDateTime = @SYSDATETIME
            FROM @Table tbl
            WHERE WarehousingKBN = 90
              AND DeleteFlg = 0
              AND DeleteDateTime IS NULL
              AND [Number] = @MoveNO
              AND NumberRow = tbl.MoveRows
              ;            
            
            IF @OperateMode = 2
            BEGIN
                --�yD_Move�z�݌Ɉړ��@�e�[�u���]���d�lA�@
                UPDATE [D_Move]
                   SET [StoreCD] = @StoreCD                         
                      ,[MoveDate] = convert(date,@MoveDate)
                      ,[RequestNO] = @RequestNO
                      ,[MovePurposeKBN] = @MovePurposeKBN
                      ,[FromSoukoCD] = @FromSoukoCD
                      ,[ToStoreCD] = @ToStoreCD
                      ,[ToSoukoCD] = @ToSoukoCD
                      ,[StaffCD]         = @StaffCD  
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                 WHERE MoveNO = @MoveNO
                   ;

                --�yD_MoveDetails�z�݌Ɉړ����ׁ@Table�]���d�l�a�@
                UPDATE [D_MoveDetails]
                   SET  [SKUCD]          = tbl.SKUCD
                       ,[AdminNO]        = tbl.AdminNO
                       ,[JanCD]          = tbl.JanCD
                       ,[MoveSu]         = tbl.MoveSu
                       ,[EvaluationPrice] = tbl.EvaluationPrice
                       ,[FromRackNO]      = tbl.FromRackNO
                       ,[ToRackNO]        = tbl.ToRackNO
                       ,[NewSKUCD]        = tbl.NewSKUCD
                       ,[NewAdminNO]      = tbl.NewAdminNO
                       ,[NewJanCD]        = tbl.NewJanCD
                       ,[DeliveryPlanNO]  = tbl.DeliveryPlanNO
                       ,[ExpectReturnDate] = tbl.ExpectReturnDate
                       ,[VendorCD]         = tbl.VendorCD
                       ,[CommentInStore]   = tbl.CommentInStore
                       ,[RequestRows]      = tbl.RequestRows
                       ,[TotalArrivalSu]   = 0       
                       ,[UpdateOperator]   =  @Operator  
                       ,[UpdateDateTime]   =  @SYSDATETIME
                FROM D_MoveDetails
                INNER JOIN @Table tbl
                 ON @MoveNO = D_MoveDetails.MoveNO
                 AND tbl.MoveRows = D_MoveDetails.MoveRows
                 AND tbl.UpdateFlg = 1
                 ;
                
                --�s�ǉ��f�[�^
                --�yD_MoveDetails�z�݌Ɉړ����ׁ@Table�]���d�l�a
                INSERT INTO [D_MoveDetails]
                           ([MoveNO]
                           ,[MoveRows]
                           ,[SKUCD]
                           ,[AdminNO]
                           ,[JanCD]
                           ,[MoveSu]
                           ,[EvaluationPrice]
                           ,[FromRackNO]
                           ,[ToRackNO]
                           ,[NewJanCD]
                           ,[NewAdminNO]
                           ,[NewSKUCD]
                           ,[DeliveryPlanNO]
                           ,[ExpectReturnDate]
                           ,[VendorCD]
                           ,[CommentInStore]
                           ,[RequestRows]
                           ,[TotalArrivalSu]

                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime])
                     SELECT @MoveNO                         
                           ,tbl.MoveRows                       
                           ,tbl.SKUCD
                           ,tbl.AdminNO
                           ,tbl.JanCD
                           ,tbl.MoveSu
                           ,tbl.EvaluationPrice
                           ,tbl.FromRackNO
                           ,tbl.ToRackNO
                           ,tbl.NewJanCD
                           ,tbl.NewAdminNO
                           ,tbl.NewSKUCD
                           ,@DeliveryPlanNO     --���v�m�F
                           ,tbl.ExpectReturnDate
                           ,tbl.VendorCD
                           ,tbl.CommentInStore
                           ,tbl.RequestRows
                           ,0 AS TotalArrivalSu
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME

                      FROM @Table tbl
                      WHERE tbl.UpdateFlg = 0
                      ;
                
                --�s�폜�f�[�^
                --�yD_MoveDetails�z�݌Ɉړ����ׁ@Table�]���d�l�a�A
                UPDATE [D_MoveDetails]
                    SET [DeleteOperator]     =  @Operator  
                       ,[DeleteDateTime]     =  @SYSDATETIME
                 WHERE [MoveNO] = @MoveNO
                 AND EXISTS(SELECT tbl.MoveRows FROM @Table tbl
                            WHERE tbl.MoveRows = D_MoveDetails.MoveRows
                            AND tbl.UpdateFlg = 2)
                 AND [DeleteDateTime] IS NULL
                 ;
            END
        END
        ELSE IF @OperateMode = 2	--�ړ��敪<>�X�܊Ԉړ��̏C����
        BEGIN
            EXEC PRC_ZaikoIdou_A2B2
                @MoveNO,
                @Operator,
                @SYSDATETIME
                ;
            
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                18,        --in�`�[��� 18
                @MoveDate, --in���
                @StoreCD,  --in�X��CD
                @Operator,
                @NewMoveNO OUTPUT
                ;
            
            IF ISNULL(@NewMoveNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --�yD_Move�z�݌Ɉړ��@Table�]���d�l�`
            INSERT INTO [D_Move]
               ([MoveNO]
               ,[StoreCD]
               ,[MoveDate]
               ,[RequestNO]
               ,[MovePurposeKBN]
               ,[FromSoukoCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[MoveInputDateTime]
               ,[StaffCD]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            VALUES
               (@NewMoveNO
               ,@StoreCD
               ,convert(date,@MoveDate)
               ,@RequestNO
               ,@MovePurposeKBN
               ,@FromSoukoCD
               ,@ToStoreCD
               ,@ToSoukoCD
               ,SYSDATETIME()   --MoveInputDateTime
               ,@StaffCD

               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL                  
               ,NULL
               );               

            --�yD_MoveDetails�z�݌Ɉړ����ׁ@Table�]���d�l�a
            INSERT INTO [D_MoveDetails]
                       ([MoveNO]
                       ,[MoveRows]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[MoveSu]
                       ,[EvaluationPrice]
                       ,[FromRackNO]
                       ,[ToRackNO]
                       ,[NewJanCD]
                       ,[NewAdminNO]
                       ,[NewSKUCD]
                       ,[DeliveryPlanNO]
                       ,[ExpectReturnDate]
                       ,[VendorCD]
                       ,[CommentInStore]
                       ,[RequestRows]
                       ,[TotalArrivalSu]

                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
             SELECT @NewMoveNO                         
                   ,tbl.MoveRows                       
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.MoveSu
                   ,tbl.EvaluationPrice
                   ,tbl.FromRackNO
                   ,tbl.ToRackNO
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD
                   ,@DeliveryPlanNO
                   ,tbl.ExpectReturnDate
                   ,tbl.VendorCD
                   ,tbl.CommentInStore
                   ,tbl.RequestRows
                   ,0 AS TotalArrivalSu
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg <> 2 --�C�����Ȃ̂�
              ;
        END

  	--�ԃf�[�^�쐬***************************************************************************�폜�E�C����
        --�ړ��敪<>�X�܊Ԉړ�
        IF @MovePurposeType <> @KBN_TENPOKAN
        BEGIN
            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b�A�ԁ@�i�b�ɑ΂���ԃf�[�^�@WarehousingKBN��11�j            
            --C�A(12),C�A(15),C�A(19),C�A(20),C�A(22),C�A(16)
            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�c�A�ԁ@�i�c�ɑ΂���ԃf�[�^�@WarehousingKBN��13�j
            --D�A(15),D�A(22),D�A(16)
            INSERT INTO [D_Warehousing]
               ([WarehousingDate]
               ,[SoukoCD]
               ,[RackNO]
               ,[StockNO]
               ,[JanCD]
               ,[AdminNO]
               ,[SKUCD]
               ,[WarehousingKBN]
               ,[DeleteFlg]
               ,[Number]
               ,[NumberRow]
               ,[VendorCD]
               ,[ToStoreCD]
               ,[ToSoukoCD]
               ,[ToRackNO]
               ,[ToStockNO]
               ,[FromStoreCD]
               ,[FromSoukoCD]
               ,[FromRackNO]
               ,[CustomerCD]
               ,[Quantity]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT DW.WarehousingDate
               ,DW.SoukoCD
               ,DW.RackNO
               ,DW.StockNO			--(D_Stock)��(�ړ���)�Ɠ����l
               ,tbl.JanCD	--JanCD
               ,tbl.AdminNO	--AdminNO
               ,tbl.SKUCD	--SKUCD
               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 12 
                                       WHEN @KBN_SYOCD      THEN 31		--15 C�A
                                       WHEN @KBN_CHOSEI_ADD THEN 19
                                       WHEN @KBN_CHOSEI_DEL THEN 20
                                       WHEN @KBN_LOCATION   THEN 22
                                       WHEN @KBN_HENPIN     THEN CASE WHEN DW.Quantity < 0 then 16 else 26 end  --2020/10/01 Fukuda
                                       ELSE 0 END)   --WarehousingKBN
               ,1  --DeleteFlg
               ,@MoveNO  --Number��
               ,tbl.MoveRows --NumberRow��
               ,NULL	--VendorCD
               ,DW.ToStoreCD
               ,DW.ToSoukoCD
               ,DW.ToRackNO
               ,DW.ToStockNO
               ,DW.FromStoreCD
               ,DW.FromSoukoCD
               ,DW.FromRackNO
               ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                       ELSE NULL END)    --CustomerCD
               ,DW.Quantity * (-1)
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              INNER JOIN D_Warehousing AS DW
              ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11
                                                            WHEN @KBN_SYOCD      THEN 31	--15
                                                            WHEN @KBN_CHOSEI_ADD THEN 19
                                                            WHEN @KBN_CHOSEI_DEL THEN 20
                                                            WHEN @KBN_LOCATION   THEN 22
                                                            WHEN @KBN_HENPIN     THEN CASE WHEN DW.Quantity < 0 then 16 else 26 end  --2020/10/01 Fukuda
                                                            ELSE 0 END)
              AND DW.DeleteFlg = 0
              AND DW.DeleteDateTime IS NULL
              AND DW.[Number] = @MoveNO
              AND DW.NumberRow = tbl.MoveRows
              ;

            --D�A(15)
            IF @MovePurposeType = @KBN_SYOCD
            BEGIN
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT DW.WarehousingDate
                   ,DW.SoukoCD
                   ,DW.RackNO
                   ,DW.StockNO          --(D_Stock)��(�ړ���)�Ɠ����l
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD                   
                   ,32     --15 D�A--WarehousingKBN
                   ,1  --DeleteFlg
                   ,@MoveNO  --Number��
                   ,tbl.MoveRows --NumberRow��
                   ,NULL    --VendorCD
                   ,DW.ToStoreCD
                   ,DW.ToSoukoCD
                   ,DW.ToRackNO
                   ,DW.ToStockNO
                   ,DW.FromStoreCD
                   ,DW.FromSoukoCD
                   ,DW.FromRackNO
                   ,tbl.JanCD    --CustomerCD
                   ,DW.Quantity * (-1)
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = 32    --15                                                                
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.NumberRow = tbl.MoveRows
                  ;
            END
                          
            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
            BEGIN
	            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�c�A�ԁ@�i�c�ɑ΂���ԃf�[�^�@WarehousingKBN��13�j
	            --D�A(14)
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @MoveDate --WarehousingDate
                   ,DW.SoukoCD
                   ,DW.RackNO
                   ,DW.StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 14   --WarehousingKBN
                                            ELSE 0 END)
                   ,1  --DeleteFlg
                   ,@MoveNO  --Number
                   ,tbl.MoveRows --NumberRow
                   ,NULL    --VendorCD
	               ,DW.ToStoreCD
	               ,DW.ToSoukoCD
	               ,DW.ToRackNO
	               ,DW.ToStockNO
	               ,DW.FromStoreCD
	               ,DW.FromSoukoCD
	               ,DW.FromRackNO
                   ,NULL    --CustomerCD
                   ,DW.Quantity * (-1)  --Quantity �ړ��� * -1(�}�C�i�X�l�Ƃ���)
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN =  (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13
                                                            ELSE 0 END)
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.NumberRow = tbl.MoveRows
                  ;
            END
        END          
    END
    
    --�V�K--************************************************************************************�V�K
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '�V�K';

        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            18,        --in�`�[��� 18
            @MoveDate, --in���
            @StoreCD,  --in�X��CD
            @Operator,
            @NewMoveNO OUTPUT
            ;
        
        IF ISNULL(@NewMoveNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --�ړ��敪=�X�܊Ԉړ�,�ԕi
        IF @MovePurposeType IN (@KBN_TENPOKAN ,@KBN_HENPIN) 
        BEGIN
            --�yD_DeliveryPlan�z�z���\����@�e�[�u���]���d�lJ
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                19,             --in�`�[��� 19
                @MoveDate, --in���
                @StoreCD,       --in�X��CD
                @Operator,
                @DeliveryPlanNO OUTPUT
                ;
            
            IF ISNULL(@DeliveryPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END

            INSERT INTO [D_DeliveryPlan]
                   ([DeliveryPlanNO]
                   ,[DeliveryKBN]
                   ,[Number]
                   ,[DeliveryName]
                   ,[DeliverySoukoCD]
                   ,[DeliveryZip1CD]
                   ,[DeliveryZip2CD]
                   ,[DeliveryAddress1]
                   ,[DeliveryAddress2]
                   ,[DeliveryMailAddress]
                   ,[DeliveryTelphoneNO]
                   ,[DeliveryFaxNO]
                   ,[DecidedDeliveryDate]
                   ,[DecidedDeliveryTime]
                   ,[CarrierCD]
                   ,[PaymentMethodCD]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[InvoiceNO]
                   ,[DeliveryPlanDate]
                   ,[HikiateFLG]
                   ,[IncludeFLG]
                   ,[OntheDayFLG]
                   ,[ExpressFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
            SELECT
                    @DeliveryPlanNO
                   ,2 AS DeliveryKBN	--(1:�̔��A2:�q�Ɉړ�)
                   ,@NewMoveNO	AS Number
                   ,NULL	--[DeliveryName]
                   ,@ToSoukoCD	--[DeliverySoukoCD]
                   ,NULL	--[DeliveryZip1CD]
                   ,NULL	--[DeliveryZip2CD]
                   ,NULL	--[DeliveryAddress1]
                   ,NULL	--[DeliveryAddress2]
                   ,NULL	--[DeliveryMailAddress]
                   ,NULL	--[DeliveryTelphoneNO]
                   ,NULL	--[DeliveryFaxNO]
                   ,NULL	--[DecidedDeliveryDate]
                   ,NULL	--[DecidedDeliveryTime]
                   ,NULL	--[CarrierCD]
                   ,NULL	--[PaymentMethodCD]
                   ,NULL	--[CommentInStore]
                   ,NULL	--[CommentOutStore]
                   ,NULL	--[InvoiceNO]
                   ,NULL	--[DeliveryPlanDate]
                   ,1	--[HikiateFLG]
                   ,0	--[IncludeFLG]
                   ,0	--[OntheDayFLG]
                   ,0	--[ExpressFLG]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
              ;            
        END  
        
        --�yD_Move�z�݌Ɉړ��@Table�]���d�l�`
        INSERT INTO [D_Move]
           ([MoveNO]
           ,[StoreCD]
           ,[MoveDate]
           ,[RequestNO]
           ,[MovePurposeKBN]
           ,[FromSoukoCD]
           ,[ToStoreCD]
           ,[ToSoukoCD]
           ,[MoveInputDateTime]
           ,[StaffCD]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     	VALUES
           (@NewMoveNO
           ,@StoreCD
           ,convert(date,@MoveDate)
           ,@RequestNO
           ,@MovePurposeKBN
           ,@FromSoukoCD
           ,@ToStoreCD
           ,@ToSoukoCD
           ,SYSDATETIME()	--MoveInputDateTime
           ,@StaffCD

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

        --�yD_MoveDetails�z�݌Ɉړ����ׁ@Table�]���d�l�a
        INSERT INTO [D_MoveDetails]
                   ([MoveNO]
                   ,[MoveRows]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[MoveSu]
                   ,[EvaluationPrice]
                   ,[FromRackNO]
                   ,[ToRackNO]
                   ,[NewJanCD]
                   ,[NewAdminNO]
                   ,[NewSKUCD]
                   ,[DeliveryPlanNO]
                   ,[ExpectReturnDate]
                   ,[VendorCD]
                   ,[CommentInStore]
                   ,[RequestRows]
                   ,[TotalArrivalSu]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @NewMoveNO                         
                   ,tbl.MoveRows                       
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.MoveSu
                   ,tbl.EvaluationPrice
                   ,tbl.FromRackNO
                   ,tbl.ToRackNO
                   ,tbl.NewJanCD
                   ,tbl.NewAdminNO
                   ,tbl.NewSKUCD
                   ,@DeliveryPlanNO
                   ,tbl.ExpectReturnDate
                   ,tbl.VendorCD
                   ,tbl.CommentInStore
                   ,tbl.RequestRows
                   ,0 AS TotalArrivalSu
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
              
        --�ړ��敪=�X�܊Ԉړ�,�ԕi
        IF @MovePurposeType IN (@KBN_TENPOKAN, @KBN_HENPIN)
        BEGIN
            --�yD_DeliveryPlanDetails�z�z���\�薾�ׁ@�e�[�u���]���d�lK
            INSERT INTO [D_DeliveryPlanDetails]
               ([DeliveryPlanNO]
               ,[DeliveryPlanRows]
               ,[Number]
               ,[NumberRows]
               ,[CommentInStore]
               ,[CommentOutStore]
               ,[HikiateFLG]
               ,[UpdateCancelKBN]
               ,[DeliveryOrderComIn]
               ,[DeliveryOrderComOut]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
             SELECT  
                @DeliveryPlanNO
               ,tbl.MoveRows AS DeliveryPlanRows
               ,@NewMoveNO AS Number
               ,tbl.MoveRows  As NumberRows
               ,NULL	--CommentInStore]
               ,NULL	--CommentOutStore]
               ,1	--HikiateFLG]
               ,0	--UpdateCancelKBN]
               ,NULL	--DeliveryOrderComIn]
               ,NULL	--DeliveryOrderComOut]                        
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
        END

        DECLARE @StockSu int;
        DECLARE @AllowableSu int;
        DECLARE @WIdoSu int;
        DECLARE @WUpdSu int;
        DECLARE @ToStockNO varchar(11);

        --���א���Insert��
        --�J�[�\���I�[�v��
        OPEN CUR_TABLE;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ���[�v���̎��ۂ̏��� ��������===
        	SET @ArrivalPlanNO = NULL;
        	
            IF @MovePurposeType = @KBN_TENPOKAN
            BEGIN
               --�`�[�ԍ��̔�
                EXEC Fnc_GetNumber
                    22,             --in�`�[��� 5
                    @MoveDate,      --in���
                    @StoreCD,       --in�X��CD
                    @Operator,
                    @ArrivalPlanNO OUTPUT
                    ;
                
                IF ISNULL(@ArrivalPlanNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
            
                --�yD_ArrivalPlan�z���ח\����@Table�]���d�l�k
                INSERT INTO [D_ArrivalPlan]
                       ([ArrivalPlanNO]
                       ,[ArrivalPlanKBN]
                       ,[Number]
                       ,[NumberRows]
                       ,[NumberSEQ]
                       ,[ArrivalPlanDate]
                       ,[ArrivalPlanMonth]
                       ,[ArrivalPlanCD]
                       ,[CalcuArrivalPlanDate]
                       ,[ArrivalPlanUpdateDateTime]
                       ,[StaffCD]
                       ,[LastestFLG]
                       ,[EDIImportNO]
                       ,[SoukoCD]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[ArrivalPlanSu]
                       ,[ArrivalSu]
                       ,[OriginalArrivalPlanNO]
                       ,[OrderCD]
                       ,[FromSoukoCD]
                       ,[ToStoreCD]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ArrivalPlanNO
                       ,2 AS ArrivalPlanKBN
                       ,@NewMoveNO	AS Number
                       ,tbl.MoveRows AS NumberRows
                       ,tbl.MoveRows AS NumberSEQ
                       ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                     FROM M_Souko A 
                                     WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                     ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) AS ArrivalPlanDate
                       ,0 AS ArrivalPlanMonth
                       ,NULL AS ArrivalPlanCD
                       ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                     FROM M_Souko A 
                                     WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                     ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) AS CalcuArrivalPlanDate
                       ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                       ,@StaffCD
                       ,1 AS LastestFLG
                       ,NULL AS EDIImportNO
                       ,@ToSoukoCD
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,tbl.MoveSu AS ArrivalPlanSu
                       ,0   --ArrivalSu
                       ,NULL AS ArrivalPlanNO    
                       ,NULL AS OrderCD
                       ,@FromSoukoCD
                       ,@ToStoreCD
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
            END
            
            SET @ToStockNO = NULL;
            --�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g/�g�B�X�܈ړ�/H�C�ԕi
            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
            BEGIN
                --�`�[�ԍ��̔ԁ�ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in�`�[��� 21
                    @MoveDate, --in���
                    @StoreCD,  --in�X��CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --�yD_Stock�zInsert    
                INSERT INTO [D_Stock]
                       ([StockNO]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[ArrivalPlanNO]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[ArrivalYetFLG]
                       ,[ArrivalPlanKBN]
                       ,[ArrivalPlanDate]
                       ,[ArrivalDate]
                       ,[StockSu]
                       ,[PlanSu]
                       ,[AllowableSu]
                       ,[AnotherStoreAllowableSu]
                       ,[ReserveSu]
                       ,[InstructionSu]
                       ,[ShippingSu]
                       ,[OriginalStockNO]
                       ,[ExpectReturnDate]
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN @FromSoukoCD
                                               ELSE ISNULL(@ToSoukoCD,@FromSoukoCD) END)    --SoukoCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN NULL
                                               WHEN @KBN_SYOCD THEN tbl.FromRackNO  
                                               ELSE tbl.ToRackNO END)   --RackNO
                       ,@ArrivalPlanNO    --ArrivalPlanNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewSKUCD
                                               ELSE tbl.SKUCD END)
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewAdminNO
                                               ELSE tbl.AdminNO END)
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN tbl.NewJanCD
                                               ELSE tbl.JanCD END)
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 1   
                                               ELSE 0 END)          --  ArrivalYetFLG(0:���׍ρA1:������)
                       ,3   --ArrivalPlanKBN(1:�󔭒����A2:�������A3:�ړ���)
                       ,(SELECT A.ArrivalPlanDate FROM D_ArrivalPlan AS A
                          WHERE A.ArrivalPlanNO = @ArrivalPlanNO)    --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 0
                                               WHEN @KBN_HENPIN   THEN 0
                                               ELSE tbl.MoveSu    END)  --StockSu
                       ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN tbl.MoveSu
                                               ELSE 0 END)   --PlanSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                               ELSE tbl.MoveSu END)   --AllowableSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                               ELSE tbl.MoveSu END)  --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL    --OriginalStockNO
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.ExpectReturnDate
                                               ELSE NULL END)    --ExpectReturnDate
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.MoveSu
                                               ELSE 0 END)      --ReturnPlanSu
                       ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.VendorCD
                                               ELSE NULL END) --[VendorCD]
                       ,NULL   --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
            END      
        
            SET @WIdoSu = @tblMoveSu;

            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD)
            BEGIN
                --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�lG�@
                --�ړ������݌ɂň���
                --�J�[�\����`(D_Stock_SelectSuryo�Q��)
                DECLARE CUR_Stock CURSOR FOR
                    SELECT DS.StockSu
                        ,DS.AllowableSu
                        ,DS.StockNO
                    from D_Stock DS
                    WHERE DS.SoukoCD = @FromSoukoCD
                    AND DS.RackNO = @tblFromRackNO
                    AND DS.AdminNO = @tblAdminNO
                    AND DS.DeleteDateTime is null 
                    AND DS.AllowableSu > 0 
                    AND DS.ArrivalYetFlg = 0 
                    ;
                
                --�J�[�\���I�[�v��
                OPEN CUR_Stock;

                --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_Stock
                INTO @StockSu, @AllowableSu, @StockNO;
                
                --�f�[�^�̍s�������[�v���������s����
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ���[�v���̎��ۂ̏��� ��������===*************************CUR_Stock
                    IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                    BEGIN
                        SET @WUpdSu = @WIdoSu;
                    END
                    ELSE IF @StockSu < @AllowableSu
                    BEGIN
                        SET @WUpdSu = @StockSu;
                    END
                    ELSE
                    BEGIN
                        SET @WUpdSu = @AllowableSu;
                    END
                    
                    UPDATE [D_Stock] SET
                           [StockSu] = [StockSu] - @WUpdSu
                          ,[AllowableSu] = [AllowableSu] - @WUpdSu
                          ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                          --,[ExpectReturnDate]   = tbl.ExpectReturnDate    2020.09.18 del
                          --,[VendorCD]           = tbl.VendorCD
                          --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                          ,[UpdateOperator]     =  @Operator  
                          ,[UpdateDateTime]     =  @SYSDATETIME
                          
                     FROM D_Stock AS DS
                     INNER JOIN @Table tbl
                     ON tbl.MoveRows = @tblMoveRows
                     WHERE DS.DeleteDateTime IS NULL
                     AND DS.StockNO = @StockNO
                    ;
                
                    --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b
                    --C(11),C(15),C(19),C(20),C(22),C(16),C(90)
                    INSERT INTO [D_Warehousing]
                       ([WarehousingDate]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[StockNO]
                       ,[JanCD]
                       ,[AdminNO]
                       ,[SKUCD]
                       ,[WarehousingKBN]
                       ,[DeleteFlg]
                       ,[Number]
                       ,[NumberRow]
                       ,[VendorCD]
                       ,[ToStoreCD]
                       ,[ToSoukoCD]
                       ,[ToRackNO]
                       ,[ToStockNO]
                       ,[FromStoreCD]
                       ,[FromSoukoCD]
                       ,[FromRackNO]
                       ,[CustomerCD]
                       ,[Quantity]
                       ,[Program]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT @MoveDate --WarehousingDate
                       ,@FromSoukoCD AS SoukoCD
                       ,tbl.FromRackNO    --RackNO
                       ,@StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                       ,tbl.JanCD
                       ,tbl.AdminNO
                       ,tbl.SKUCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                               WHEN @KBN_SYOCD      THEN 31 --15
                                               WHEN @KBN_CHOSEI_ADD THEN 19
                                               WHEN @KBN_CHOSEI_DEL THEN 20
                                               WHEN @KBN_LOCATION   THEN 22
                                               WHEN @KBN_HENPIN     THEN 16 --
                                               WHEN @KBN_TENPOKAN   THEN 90
                                               ELSE 0 END)   --WarehousingKBN
                       ,0  --DeleteFlg
                       ,@NewMoveNO  --Number
                       ,tbl.MoveRows --NumberRow
                       ,NULL    --VendorCD
                       
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                               WHEN @KBN_SYOCD      THEN @FromStoreCD
                                               WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                               WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                               WHEN @KBN_LOCATION   THEN @FromStoreCD
                                               WHEN @KBN_HENPIN     THEN @ToStoreCD     --2020.10.6
                                               WHEN @KBN_TENPOKAN   THEN @ToStoreCD
                                               ELSE NULL END)   --ToStoreCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                               WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                               WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                               WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                               WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                               WHEN @KBN_HENPIN     THEN @ToSoukoCD     --2020.10.6
                                               WHEN @KBN_TENPOKAN   THEN @ToSoukoCD
                                               ELSE NULL END)   --ToSoukoCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                               WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                               WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                               WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                               WHEN @KBN_LOCATION   THEN tbl.ToRackNO       --2020.10.29 chg
                                               WHEN @KBN_HENPIN     THEN tbl.ToRackNO       --2020.09.18 add
                                               WHEN @KBN_TENPOKAN   THEN NULL
                                               ELSE NULL END)   --ToRackNO
                       
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_SYOCD      THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_CHOSEI_ADD THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_CHOSEI_DEL THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_LOCATION   THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_HENPIN     THEN @ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l
                                               WHEN @KBN_TENPOKAN   THEN @ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l
                                               ELSE NULL END)  --ToStockNO
                       ,@FromStoreCD    --FromStoreCD
                       ,@FromSoukoCD    --FromSoukoCD
                       ,tbl.FromRackNO  --FromRackNO
                       ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                               ELSE NULL END)    --CustomerCD
                       ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                               WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                               WHEN @KBN_CHOSEI_ADD THEN @WUpdSu   --�ړ���(�v���X�l�Ƃ���)
                                               WHEN @KBN_CHOSEI_DEL THEN @WUpdSu   --�ړ���(�v���X�l�Ƃ���)
                                               WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                               WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                               WHEN @KBN_TENPOKAN   THEN @WUpdSu * (-1) --Quantity
                                               ELSE @WUpdSu END) 
                       
                       ,@Program  --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL

                      FROM @Table tbl
                      WHERE tbl.MoveRows = @tblMoveRows
                      ;
                          
                    IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL, @KBN_TENPOKAN)
                    BEGIN
                        --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�c
                        --D(13),D(15),D(22),D(16)
                        INSERT INTO [D_Warehousing]
                           ([WarehousingDate]
                           ,[SoukoCD]
                           ,[RackNO]
                           ,[StockNO]
                           ,[JanCD]
                           ,[AdminNO]
                           ,[SKUCD]
                           ,[WarehousingKBN]
                           ,[DeleteFlg]
                           ,[Number]
                           ,[NumberRow]
                           ,[VendorCD]
                           ,[ToStoreCD]
                           ,[ToSoukoCD]
                           ,[ToRackNO]
                           ,[ToStockNO]
                           ,[FromStoreCD]
                           ,[FromSoukoCD]
                           ,[FromRackNO]
                           ,[CustomerCD]
                           ,[Quantity]
                           ,[Program]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                        SELECT @MoveDate --WarehousingDate
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN ISNULL(@ToSoukoCD,@FromSoukoCD)
                                                   WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                   WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                   WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                   ELSE NULL END) AS SoukoCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                   WHEN @KBN_SYOCD    THEN tbl.FromRackNO 
                                                   WHEN @KBN_LOCATION THEN tbl.ToRackNO         --2020.10.29 chg
                                                   WHEN @KBN_HENPIN   THEN tbl.ToRackNO
                                                   ELSE tbl.ToRackNO  END)   --RackNO�@���iCD�t�֎��݈̂ړ����I��
                           ,@ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                                   ELSE tbl.JanCD END)  --JanCD
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewAdminNO
                                                   ELSE tbl.AdminNO END)    --AdminNO
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewSKUCD
                                                   ELSE tbl.SKUCD END)      --SKUCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13 
                                                   WHEN @KBN_SYOCD    THEN 32   --15
                                                   WHEN @KBN_LOCATION THEN 22
                                                   WHEN @KBN_HENPIN   THEN 26 --16 2020/10/01 Fukuda
                                                   ELSE 0 END)   --WarehousingKBN
                           ,0  --DeleteFlg
                           ,@NewMoveNO  --Number
                           ,tbl.MoveRows --NumberRow
                           ,NULL     --VendorCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToStoreCD
                                                   WHEN @KBN_SYOCD    THEN @FromStoreCD
                                                   WHEN @KBN_LOCATION THEN @FromStoreCD
                                                   WHEN @KBN_HENPIN   THEN @ToStoreCD
                                                   ELSE NULL END)   --ToStoreCD
                           
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToSoukoCD
                                                   WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                   WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                   WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                   ELSE NULL END)   --ToSoukoCD
                                                
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                   WHEN @KBN_SYOCD    THEN tbl.FromRackNO
                                                   WHEN @KBN_LOCATION THEN tbl.ToRackNO     --2020.10.29 chg
                                                   WHEN @KBN_HENPIN   THEN tbl.ToRackNO     --2020.09.18 add
                                                   ELSE NULL END)   --ToRackNO
                                                
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_SYOCD    THEN @ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_LOCATION THEN @ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_HENPIN   THEN @StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                                                   ELSE NULL END)   --ToStockNO
                           ,@FromStoreCD    --FromStoreCD
                           ,@FromSoukoCD    --FromSoukoCD
                           ,tbl.FromRackNO  --FromRackNO
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.JanCD
                                                   ELSE NULL END)    --CustomerCD
                           ,@WUpdSu  --Quantity
                           ,@Program  --Program
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL
                           ,NULL

                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;
                    END
                    
                    SET @ReserveNO = '';
                    --�ړ��敪=�X�܊Ԉړ�
                    IF @MovePurposeType = @KBN_TENPOKAN
                    BEGIN
                        --�yD_Reserve�z�����@�e�[�u���]���d�lI�@
                        --�`�[�ԍ��̔�
                        EXEC Fnc_GetNumber
                            12,        --in�`�[��� 12
                            @MoveDate, --in���
                            @StoreCD,  --in�X��CD
                            @Operator,
                            @ReserveNO OUTPUT
                            ;
                        
                        IF ISNULL(@ReserveNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                        
                        --�yD_Reserve�z�iInsert�j
                        INSERT INTO [D_Reserve]
                               ([ReserveNO]
                               ,[ReserveKBN]
                               ,[Number]
                               ,[NumberRows]
                               ,[StockNO]
                               ,[SoukoCD]
                               ,[JanCD]
                               ,[SKUCD]
                               ,[AdminNO]
                               ,[ReserveSu]
                               ,[ShippingPossibleDate]
                               ,[ShippingPossibleSU]
                               ,[ShippingOrderNO]
                               ,[ShippingOrderRows]
                               ,[CompletedPickingNO]
                               ,[CompletedPickingRow]
                               ,[CompletedPickingDate]
                               ,[ShippingSu]
                               ,[ReturnKBN]
                               ,[OriginalReserveNO]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                         SELECT
                               @ReserveNO
                               ,2 AS ReserveKBN     --2 (1:�󒍁A2:�ړ�)
                               ,@NewMoveNO AS Number
                               ,@tblMoveRows AS NumberRows
                               ,@StockNO
                               ,@FromSoukoCD
                               ,tbl.JanCD
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,0 AS ReserveSu --���ד��א�
                               ,NULL    --ShippingPossibleDate
                               ,0       --ShippingPossibleSU
                               ,NULL    --ShippingOrderNO
                               ,0       --ShippingOrderRows
                               ,NULL    --CompletedPickingNO
                               ,0       --CompletedPickingRow
                               ,NULL    --CompletedPickingDate
                               ,0       --ShippingSu
                               ,0       --ReturnKBN
                               ,NULL    --OriginalReserveNO
                         
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                         FROM @Table tbl
                         WHERE tbl.MoveRows = @tblMoveRows
                        ;
                    END
                    
                    SET @WIdoSu = @WIdoSu - @WUpdSu;
                    
                    IF @WIdoSu = 0
                    BEGIN
                        --���̖��׃��R�[�h��
                        BREAK;
                    END
                    
                    --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                    FETCH NEXT FROM CUR_Stock
                    INTO @StockSu, @AllowableSu, @StockNO;
                END     --LOOP�̏I���***************************************CUR_Stock
                
                --�J�[�\�������
                CLOSE CUR_Stock;
                DEALLOCATE CUR_Stock;
            END
            
            IF @MovePurposeType = @KBN_CHOSEI_ADD AND @WIdoSu > 0
            BEGIN
                --Insert
                --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�lH�H
                --�`�[�ԍ��̔ԁ�ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in�`�[��� 21
                    @MoveDate, --in���
                    @StoreCD,  --in�X��CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --�yD_Stock�zInsert    
                INSERT INTO [D_Stock]
                       ([StockNO]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[ArrivalPlanNO]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[ArrivalYetFLG]
                       ,[ArrivalPlanKBN]
                       ,[ArrivalPlanDate]
                       ,[ArrivalDate]
                       ,[StockSu]
                       ,[PlanSu]
                       ,[AllowableSu]
                       ,[AnotherStoreAllowableSu]
                       ,[ReserveSu]
                       ,[InstructionSu]
                       ,[ShippingSu]
                       ,[OriginalStockNO]
                       ,[ExpectReturnDate]
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,@FromSoukoCD	--SoukoCD
                       ,tbl.FromRackNO	--RackNO
                       ,NULL    --ArrivalPlanNO
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,0   --  ArrivalYetFLG(0:���׍ρA1:������)
                       ,3   --ArrivalPlanKBN(1:�󔭒����A2:�������A3:�ړ���)	2020.09.18
                       ,NULL   --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,@WIdoSu		--tbl.MoveSu  --StockSu
                       ,0   --PlanSu
                       ,@WIdoSu		--tbl.MoveSu   --AllowableSu
                       ,@WIdoSu		--tbl.MoveSu   --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL  --OriginalStockNO
                       ,NULL  --ExpectReturnDate
                       ,0 	--ReturnPlanSu
                       ,NULL  --[VendorCD]
                       ,NULL   --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
                  
                --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b
                --C(19)
                INSERT INTO [D_Warehousing]
                   ([WarehousingDate]
                   ,[SoukoCD]
                   ,[RackNO]
                   ,[StockNO]
                   ,[JanCD]
                   ,[AdminNO]
                   ,[SKUCD]
                   ,[WarehousingKBN]
                   ,[DeleteFlg]
                   ,[Number]
                   ,[NumberRow]
                   ,[VendorCD]
                   ,[ToStoreCD]
                   ,[ToSoukoCD]
                   ,[ToRackNO]
                   ,[ToStockNO]
                   ,[FromStoreCD]
                   ,[FromSoukoCD]
                   ,[FromRackNO]
                   ,[CustomerCD]
                   ,[Quantity]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @MoveDate --WarehousingDate
                   ,@FromSoukoCD AS SoukoCD
                   ,tbl.FromRackNO    --RackNO
                   ,@ToStockNO	--(D_Stock)��(�ړ���)�Ɠ����l
                   ,tbl.JanCD
                   ,tbl.AdminNO
                   ,tbl.SKUCD
                   ,19 --WarehousingKBN
                   ,0  --DeleteFlg
                   ,@NewMoveNO  --Number
                   ,tbl.MoveRows --NumberRow
                   ,NULL    --VendorCD
                   ,@FromStoreCD
                   ,@FromSoukoCD
                   ,tbl.FromRackNO
                   ,@ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l--ToStockNO
                   ,@FromStoreCD
                   ,@FromSoukoCD
                   ,tbl.FromRackNO
                   ,NULL    --CustomerCD
                   ,@WIdoSu   --�ړ���(�v���X�l�Ƃ���)                   
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;
                   
            END
            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_TABLE
        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
        END            --LOOP�̏I���*******************************************************CUR_TABLE
        
        --�J�[�\�������
        CLOSE CUR_TABLE;
--        DEALLOCATE CUR_TABLE;
    END
    
    ELSE IF @OperateMode = 3 --�폜***************************************************************�폜
    BEGIN
        SET @OperateModeNm = '�폜';
        
        EXEC PRC_ZaikoIdou_A2B2
            @MoveNO,
            @Operator,
            @SYSDATETIME
            ;
            
        --�ړ��敪=�X�܊Ԉړ�
        IF @MovePurposeType = @KBN_TENPOKAN
        BEGIN
            --�yD_Reserve�z����  Table�]���d�lI�A
            UPDATE [D_Reserve] SET
                   [UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  ,[DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
                  
             FROM @Table AS tbl
             WHERE @MoveNO = D_Reserve.Number
             --AND tbl.MoveRows = D_Reserve.NumberRows
            ;
        END
        --�ړ��敪=�X�܊Ԉړ�,�ԕi
        IF @MovePurposeType IN (@KBN_TENPOKAN, @KBN_HENPIN)
        BEGIN
            --�yD_DeliveryPlan�z�z���\����  Table�]���d�lJ�A
            UPDATE [D_DeliveryPlan] SET
                   [Number] = NULL
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
             WHERE @MoveNO = D_DeliveryPlan.Number
            ;
            
            --�yD_DeliveryPlanDetails�z�z���\����@Table�]���d�lK�A
            UPDATE [D_DeliveryPlanDetails] SET
                   [Number] = NULL
                  ,[NumberRows] = 0
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM @Table AS tbl
             WHERE @MoveNO = D_DeliveryPlanDetails.Number
             --AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
            ;
            
            IF @MovePurposeType = @KBN_TENPOKAN
            BEGIN
                UPDATE [D_ArrivalPlan] SET
                   [DeleteOperator]     =  @Operator  
                  ,[DeleteDateTime]     =  @SYSDATETIME
                WHERE @MoveNO = D_ArrivalPlan.Number
                ;
            END
        END
    END
    
    
    IF @OperateMode >= 2    --�폜�E�C����**************************************************�폜�E�C����
    BEGIN
        --�ړ��敪=�X�܊Ԉړ�
        IF @MovePurposeType = @KBN_TENPOKAN  
        BEGIN
            --�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g�A��
            UPDATE [D_Stock] SET
                   [PlanSu] = [PlanSu] - tbl.OldMoveSu
                  ,[AllowableSu] = [AllowableSu] - tbl.OldMoveSu
                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - tbl.OldMoveSu
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM D_Stock AS DS
             INNER JOIN D_ArrivalPlan AS DA
             ON DA.ArrivalPlanNO = DS.ArrivalPlanNO
             INNER JOIN @Table tbl
             ON DA.ArrivalPlanNO = tbl.ArrivalPlanNO
             WHERE DS.DeleteDateTime IS NULL
             AND DS.PlanSu > 0		--����ɒǉ�
            ;

            IF @OperateMode = 2--*******************************************************�C��
            BEGIN 
                --�yD_DeliveryPlan�z�z���\����  Table�]���d�lJ�@
                UPDATE [D_DeliveryPlan] SET
                       [DeliveryKBN]     = 2    --(1:�̔��A2:�q�Ɉړ�)
                      ,[Number]          = @MoveNO
                      ,[DeliverySoukoCD] = @ToSoukoCD
                      ,[UpdateOperator]  = @Operator  
                      ,[UpdateDateTime]  = @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlan.Number
                ;
	            
                --�yD_DeliveryPlanDetails�z�z���\����@Table�]���d�lK�@
                UPDATE [D_DeliveryPlanDetails] SET
                       [UpdateOperator]  =  @Operator  
                      ,[UpdateDateTime]  =  @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlanDetails.Number
                 AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
                ; 

                --�yD_ArrivalPlan�z     Update   Table�]���d�l�k
                UPDATE [D_ArrivalPlan] SET
                   [ArrivalPlanDate] = DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                    FROM M_Souko A 
                                                    WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                    ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) --AS ArrivalPlanDate
                  ,[CalcuArrivalPlanDate] = DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                                         FROM M_Souko A 
                                                         WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                                         ORDER BY A.ChangeDate desc),convert(date,@MoveDate)) --AS ArrivalPlanDate
                  ,[SoukoCD]        = @ToSoukoCD
                  ,[SKUCD]          = tbl.SKUCD
                  ,[AdminNO]        = tbl.AdminNO
                  ,[JanCD]          = tbl.JanCD
                  ,[ArrivalPlanSu]  = tbl.MoveSu
                  ,[FromSoukoCD   ] = @FromSoukoCD
                  ,[ToStoreCD]      = @ToStoreCD
                  ,[UpdateOperator] =  @Operator  
                  ,[UpdateDateTime] =  @SYSDATETIME
                
                  FROM @Table AS tbl
                 WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
                   AND tbl.MoveRows      = D_ArrivalPlan.NumberRows
                   AND tbl.UpdateFlg     = 1
                ;
                
                --�s�폜
                UPDATE [D_ArrivalPlan] SET
                   [DeleteOperator] = @Operator  
                  ,[DeleteDateTime] = @SYSDATETIME
                
                 FROM @Table AS tbl
                 WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
                   AND tbl.MoveRows      = D_ArrivalPlan.NumberRows
                   AND tbl.UpdateFlg     = 2
                ;
                
                --���א���Insert��
                --�J�[�\���I�[�v��
                OPEN CUR_TABLE;

                --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_TABLE
                INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
                
                --�f�[�^�̍s�������[�v���������s����
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ���[�v���̎��ۂ̏��� ��������===
                    IF @tblUpdateFlg = 0    --�ǉ��s�̂�
                    BEGIN
                      --�`�[�ԍ��̔�
                        EXEC Fnc_GetNumber
                            22,             --in�`�[��� 5
                            @MoveDate,      --in���
                            @StoreCD,       --in�X��CD
                            @Operator,
                            @ArrivalPlanNO OUTPUT
                            ;
                        
                        IF ISNULL(@ArrivalPlanNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                    
                        --�yD_ArrivalPlan�z���ח\����@Table�]���d�l�k
                        INSERT INTO [D_ArrivalPlan]
                               ([ArrivalPlanNO]
                               ,[ArrivalPlanKBN]
                               ,[Number]
                               ,[NumberRows]
                               ,[NumberSEQ]
                               ,[ArrivalPlanDate]
                               ,[ArrivalPlanMonth]
                               ,[ArrivalPlanCD]
                               ,[CalcuArrivalPlanDate]
                               ,[ArrivalPlanUpdateDateTime]
                               ,[StaffCD]
                               ,[LastestFLG]
                               ,[EDIImportNO]
                               ,[SoukoCD]
                               ,[SKUCD]
                               ,[AdminNO]
                               ,[JanCD]
                               ,[ArrivalPlanSu]
                               ,[ArrivalSu]
                               ,[OriginalArrivalPlanNO]
                               ,[OrderCD]
                               ,[FromSoukoCD]
                               ,[ToStoreCD]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                         SELECT
                                @ArrivalPlanNO
                               ,2 AS ArrivalPlanKBN
                               ,@MoveNO  AS Number
                               ,tbl.MoveRows AS NumberRows
                               ,tbl.MoveRows AS NumberSEQ
                               ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                             FROM M_Souko A 
                                             WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                             ORDER BY A.ChangeDate desc),convert(date,@MoveDate))  AS ArrivalPlanDate
                               ,0 AS ArrivalPlanMonth
                               ,NULL AS ArrivalPlanCD
                               ,DATEADD(DAY,(SELECT top 1 convert(int,A.StoreIdouCount) 
                                             FROM M_Souko A 
                                             WHERE A.SoukoCD = @ToSoukoCD AND A.DeleteFlg = 0 AND A.ChangeDate <= convert(date,@MoveDate)
                                             ORDER BY A.ChangeDate desc),convert(date,@MoveDate))  AS CalcuArrivalPlanDate
                               ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                               ,@StaffCD
                               ,1 AS LastestFLG
                               ,NULL AS EDIImportNO
                               ,@ToSoukoCD
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,tbl.MoveSu AS ArrivalPlanSu
                               ,0   --ArrivalSu
                               ,NULL AS ArrivalPlanNO    
                               ,NULL AS OrderCD
                               ,@FromSoukoCD
                               ,@ToStoreCD
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                      	WHERE tbl.MoveRows = @tblMoveRows
                          ;
                	END		--�ǉ��s�̂�

                    IF @tblUpdateFlg <> 2    --�폜�s�ȊO
                    BEGIN
                        --�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g�B�X�܈ړ�
                        --�`�[�ԍ��̔ԁ�ToStockNO
                        EXEC Fnc_GetNumber
                            21,        --in�`�[��� 21
                            @MoveDate, --in���
                            @StoreCD,  --in�X��CD
                            @Operator,
                            @ToStockNO OUTPUT
                            ;
                        
                        IF ISNULL(@ToStockNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                        
                        --�yD_Stock�zInsert    
                        INSERT INTO [D_Stock]
                               ([StockNO]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[ArrivalPlanNO]
                               ,[SKUCD]
                               ,[AdminNO]
                               ,[JanCD]
                               ,[ArrivalYetFLG]
                               ,[ArrivalPlanKBN]
                               ,[ArrivalPlanDate]
                               ,[ArrivalDate]
                               ,[StockSu]
                               ,[PlanSu]
                               ,[AllowableSu]
                               ,[AnotherStoreAllowableSu]
                               ,[ReserveSu]
                               ,[InstructionSu]
                               ,[ShippingSu]
                               ,[OriginalStockNO]
                               ,[ExpectReturnDate]
                               ,[ReturnDate]
                               ,[ReturnSu]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                         SELECT
                                @ToStockNO
                               ,@ToSoukoCD
                               ,NULL   --RackNO
                               ,(CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                                                    ELSE tbl.ArrivalPlanNO END)    --ArrivalPlanNO
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,1   --  ArrivalYetFLG(0:���׍ρA1:������)
                               ,3   --ArrivalPlanKBN(1:�󔭒����A2:�������A3:�ړ���)
                               ,(SELECT A.ArrivalPlanDate FROM D_ArrivalPlan AS A
                                  WHERE A.ArrivalPlanNO = (CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                                                                              ELSE tbl.ArrivalPlanNO END))    --ArrivalPlanDate
                               ,NULL    --ArrivalDate
                               ,0  --StockSu
                               ,tbl.MoveSu   --PlanSu
                               ,tbl.MoveSu   --AllowableSu
                               ,tbl.MoveSu   --AnotherStoreAllowableSu
                               ,0    --ReserveSu
                               ,0   --InstructionSu
                               ,0   --ShippingSu
                               ,NULL    --OriginalStockNO
                               ,NULL    --ExpectReturnDate
                               ,NULL    --ReturnDate
                               ,0   --ReturnSu
                         
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;  
                      

                        SET @WIdoSu = @tblMoveSu;

                        --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�lG�@
                        --�ړ������݌ɂň���
                        --�J�[�\����`(D_Stock_SelectSuryo�Q��)
                        DECLARE CUR_Stock CURSOR FOR
                            SELECT DS.StockSu
                                ,DS.AllowableSu
                                ,DS.StockNO
                            from D_Stock DS
                            WHERE DS.SoukoCD = @FromSoukoCD
                            AND DS.RackNO = @tblFromRackNO
                            AND DS.AdminNO = @tblAdminNO
                            AND DS.DeleteDateTime is null 
                            AND DS.AllowableSu > 0 
                            AND DS.ArrivalYetFlg = 0 
                            ;
                            
                        --�J�[�\���I�[�v��
                        OPEN CUR_Stock;

                        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
                        FETCH NEXT FROM CUR_Stock
                        INTO @StockSu, @AllowableSu, @StockNO;
                        
                        --�f�[�^�̍s�������[�v���������s����
                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                        -- ========= ���[�v���̎��ۂ̏��� ��������===*************************CUR_Stock
                            IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                            BEGIN
                                SET @WUpdSu = @WIdoSu;
                            END
                            ELSE IF @StockSu < @AllowableSu
                            BEGIN
                                SET @WUpdSu = @StockSu;
                            END
                            ELSE
                            BEGIN
                                SET @WUpdSu = @AllowableSu;
                            END
                            
                            UPDATE [D_Stock] SET
                                   [StockSu] = [StockSu] - @WUpdSu
                                  ,[AllowableSu] = [AllowableSu] - @WUpdSu
                                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                                  --,[ExpectReturnDate]   = tbl.ExpectReturnDate	2020.09.18 del
                                  --,[VendorCD]           = tbl.VendorCD
                                  --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                                  ,[UpdateOperator]     =  @Operator  
                                  ,[UpdateDateTime]     =  @SYSDATETIME
                                  
                             FROM D_Stock AS DS
                             INNER JOIN @Table tbl
                             ON tbl.MoveRows = @tblMoveRows
                             WHERE DS.DeleteDateTime IS NULL
                             AND DS.StockNO = @StockNO
                            ;

                            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b
                            --C(90)
                            INSERT INTO [D_Warehousing]
                               ([WarehousingDate]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[StockNO]
                               ,[JanCD]
                               ,[AdminNO]
                               ,[SKUCD]
                               ,[WarehousingKBN]
                               ,[DeleteFlg]
                               ,[Number]
                               ,[NumberRow]
                               ,[VendorCD]
                               ,[ToStoreCD]
                               ,[ToSoukoCD]
                               ,[ToRackNO]
                               ,[ToStockNO]
                               ,[FromStoreCD]
                               ,[FromSoukoCD]
                               ,[FromRackNO]
                               ,[CustomerCD]
                               ,[Quantity]
                               ,[Program]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                            SELECT @MoveDate --WarehousingDate
                               ,@FromSoukoCD AS SoukoCD
                               ,tbl.FromRackNO    --RackNO
                               ,@StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                               ,tbl.JanCD
                               ,tbl.AdminNO
                               ,tbl.SKUCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                                       WHEN @KBN_SYOCD      THEN 31		--15
                                                       WHEN @KBN_CHOSEI_ADD THEN 19
                                                       WHEN @KBN_CHOSEI_DEL THEN 20
                                                       WHEN @KBN_LOCATION   THEN 22
                                                       WHEN @KBN_HENPIN     THEN 16 --
                                                       WHEN @KBN_TENPOKAN   THEN 90
                                                       ELSE 0 END)   --WarehousingKBN
                               ,0  --DeleteFlg
                               ,@MoveNO  --Number	�X�܈ړ��̏ꍇ�͍̔Ԃ��Ȃ����Ȃ�
                               ,tbl.MoveRows --NumberRow
                               ,NULL    --VendorCD
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                                       WHEN @KBN_SYOCD      THEN @FromStoreCD
                                                       WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                                       WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                                       WHEN @KBN_LOCATION   THEN @FromStoreCD
                                                       WHEN @KBN_HENPIN     THEN @ToStoreCD		--2020.10.6
                                                       WHEN @KBN_TENPOKAN   THEN @ToStoreCD
                                                       ELSE NULL END)
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                                       WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                                       WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                                       WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN     THEN @ToSoukoCD		--2020.10.6
                                                       WHEN @KBN_TENPOKAN   THEN @ToSoukoCD
                                                       ELSE NULL END)
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                                       WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                                       WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                                       WHEN @KBN_LOCATION   THEN tbl.FromRackNO
                                                       WHEN @KBN_HENPIN     THEN tbl.ToRackNO		--2020.09.18 add
                                                       WHEN @KBN_TENPOKAN   THEN NULL
                                                       ELSE NULL END)
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO   -- (D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_SYOCD      THEN @StockNO     --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_CHOSEI_ADD THEN @StockNO     --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_CHOSEI_DEL THEN @StockNO     --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_LOCATION   THEN @StockNO     --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_HENPIN     THEN @ToStockNO   -- (D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_TENPOKAN   THEN @ToStockNO   -- (D_Stock)��(�ړ���)�Ɠ����l
                                                       ELSE NULL END)  --ToStockNO
                               ,@FromStoreCD
                               ,@FromSoukoCD
                               ,tbl.FromRackNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                                       ELSE NULL END)    --CustomerCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_CHOSEI_ADD THEN @WUpdSu          --�ړ���(�v���X�l�Ƃ���)
                                                       WHEN @KBN_CHOSEI_DEL THEN @WUpdSu          --�ړ���(�v���X�l�Ƃ���)
                                                       WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                                       WHEN @KBN_TENPOKAN   THEN @WUpdSu * (-1)   --Quantity
                                                       ELSE @WUpdSu END) 
                               
                               ,@Program  --Program
                               
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL
                               ,NULL

                              FROM @Table tbl
                              WHERE tbl.MoveRows = @tblMoveRows
                              ;

                            --�yD_Reserve�z�����@�e�[�u���]���d�lI�@
                            --�`�[�ԍ��̔�
                            EXEC Fnc_GetNumber
                                12,        --in�`�[��� 12
                                @MoveDate, --in���
                                @StoreCD,  --in�X��CD
                                @Operator,
                                @ReserveNO OUTPUT
                                ;
                            
                            IF ISNULL(@ReserveNO,'') = ''
                            BEGIN
                                SET @W_ERR = 1;
                                RETURN @W_ERR;
                            END
                            
                            --�yD_Reserve�z�iInsert�j
                            INSERT INTO [D_Reserve]
                                   ([ReserveNO]
                                   ,[ReserveKBN]
                                   ,[Number]
                                   ,[NumberRows]
                                   ,[StockNO]
                                   ,[SoukoCD]
                                   ,[JanCD]
                                   ,[SKUCD]
                                   ,[AdminNO]
                                   ,[ReserveSu]
                                   ,[ShippingPossibleDate]
                                   ,[ShippingPossibleSU]
                                   ,[ShippingOrderNO]
                                   ,[ShippingOrderRows]
                                   ,[CompletedPickingNO]
                                   ,[CompletedPickingRow]
                                   ,[CompletedPickingDate]
                                   ,[ShippingSu]
                                   ,[ReturnKBN]
                                   ,[OriginalReserveNO]
                                   ,[InsertOperator]
                                   ,[InsertDateTime]
                                   ,[UpdateOperator]
                                   ,[UpdateDateTime]
                                   ,[DeleteOperator]
                                   ,[DeleteDateTime])
                             SELECT
                                   @ReserveNO
                                   ,2 AS ReserveKBN     --2 (1:�󒍁A2:�ړ�)
                                   ,@MoveNO AS Number
                                   ,@tblMoveRows AS NumberRows
                                   ,@StockNO
                                   ,@FromSoukoCD
                                   ,tbl.JanCD
                                   ,tbl.SKUCD
                                   ,tbl.AdminNO
                                   ,0 AS ReserveSu --���ד��א�
                                   ,NULL    --ShippingPossibleDate
                                   ,0       --ShippingPossibleSU
                                   ,NULL    --ShippingOrderNO
                                   ,0       --ShippingOrderRows
                                   ,NULL    --CompletedPickingNO
                                   ,0       --CompletedPickingRow
                                   ,NULL    --CompletedPickingDate
                                   ,0       --ShippingSu
                                   ,0       --ReturnKBN
                                   ,NULL    --OriginalReserveNO
                             
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,@Operator  
                                   ,@SYSDATETIME
                                   ,NULL                  
                                   ,NULL
                             FROM @Table tbl
                             WHERE @tblMoveRows = tbl.MoveRows
                            ;

                            SET @WIdoSu = @WIdoSu - @WUpdSu;
                            
                            IF @WIdoSu = 0
                            BEGIN
                                --���̖��׃��R�[�h��
                                BREAK;
                            END
                            
                            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                            FETCH NEXT FROM CUR_Stock
                            INTO @StockSu, @AllowableSu, @StockNO;
                        END     --LOOP�̏I���***************************************CUR_Stock
                        
                        --�J�[�\�������
                        CLOSE CUR_Stock;
                        DEALLOCATE CUR_Stock;
                                                                
                        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                        FETCH NEXT FROM CUR_TABLE
                        INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
                    END            --LOOP�̏I���
                END		--�폜�s�ȊO
                
                --�J�[�\�������
                CLOSE CUR_TABLE;
                --DEALLOCATE CUR_TABLE;
        	END        	
        END
        
    	--�ԃf�[�^�쐬
        ELSE --�ړ��敪<>�X�܊Ԉړ�
        BEGIN
            --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�l�f�A(�g�A�̃f�[�^���܂܂�Ȃ��悤�ɍl��)
            UPDATE [D_Stock] SET
                   [StockSu] = [StockSu] + tbl.OldMoveSu
                  ,[AllowableSu] = [AllowableSu] + tbl.OldMoveSu
                  ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] + tbl.OldMoveSu
                  --,[ReturnSu]           = [ReturnSu] + tbl.OldMoveSu	2020.09.18 del
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                 FROM D_Stock AS DS
                  INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 11 
                                          WHEN @KBN_SYOCD       THEN 31		--15
                                          WHEN @KBN_CHOSEI_ADD  THEN 19
                                          WHEN @KBN_CHOSEI_DEL  THEN 20
                                          WHEN @KBN_LOCATION    THEN 22
                                          WHEN @KBN_HENPIN      THEN 16
                                          ELSE 0 END)   --WarehousingKBN
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.StockNO = DS.StockNO
                  AND (@MovePurposeType IN (@KBN_TENPONAI, @KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
                  	OR (@MovePurposeType IN (@KBN_SYOCD, @KBN_LOCATION, @KBN_HENPIN) AND DW.Quantity < 0)	--G�̃f�[�^�̂�
                  	)
                  
                  INNER JOIN @Table tbl 
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	�s�폜�f�[�^���ԃf�[�^�͍쐬���Ȃ���΂Ȃ�Ȃ�
                 WHERE DS.DeleteDateTime IS NULL
                  ;

            IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)	--, @KBN_HENPIN
            BEGIN
                --�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g�A��
                UPDATE [D_Stock] SET
                       [StockSu] = [StockSu] - tbl.OldMoveSu
                      ,[AllowableSu] = [AllowableSu] - tbl.OldMoveSu
                      ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - tbl.OldMoveSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Stock AS DS
                 INNER JOIN D_Warehousing AS DW
                  ON DW.WarehousingKBN = (CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13
                                                                WHEN @KBN_SYOCD THEN 32		--15
                                                                WHEN @KBN_LOCATION THEN 22
                                                                WHEN @KBN_HENPIN THEN 26 --16 2020/10/01 Fukuda
                                                                ELSE 0 END)
                  AND DW.DeleteFlg = 0
                  AND DW.DeleteDateTime IS NULL
                  AND DW.[Number] = @MoveNO
                  AND DW.StockNO = DS.StockNO
                  AND (@MovePurposeType IN (@KBN_TENPONAI)
                  	OR (@MovePurposeType IN (@KBN_SYOCD, @KBN_LOCATION, @KBN_HENPIN) AND DW.Quantity > 0)	--H�̃f�[�^�̂�
                  	)
                  	
                 INNER JOIN @Table tbl
                  ON tbl.MoveRows = DW.NumberRow
                  --AND tbl.UpdateFlg > 0	�s�폜�f�[�^���ԃf�[�^�͍쐬���Ȃ���΂Ȃ�Ȃ�
                 WHERE DS.DeleteDateTime IS NULL
                ;
            END
            
           
            --�ړ��敪=�ԕi
            IF @MovePurposeType = @KBN_HENPIN
            BEGIN
             /*�ԕi�̐Ԃ�G�A�@2020/02/18
            	--�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g�C�ԕi
                --�`�[�ԍ��̔ԁ�ToStockNO
                EXEC Fnc_GetNumber
                    21,        --in�`�[��� 21
                    @MoveDate, --in���
                    @StoreCD,  --in�X��CD
                    @Operator,
                    @ToStockNO OUTPUT
                    ;
                
                IF ISNULL(@ToStockNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                --�yD_Stock�zInsert    
                INSERT INTO [D_Stock]
                       ([StockNO]
                       ,[SoukoCD]
                       ,[RackNO]
                       ,[ArrivalPlanNO]
                       ,[SKUCD]
                       ,[AdminNO]
                       ,[JanCD]
                       ,[ArrivalYetFLG]
                       ,[ArrivalPlanKBN]
                       ,[ArrivalPlanDate]
                       ,[ArrivalDate]
                       ,[StockSu]
                       ,[PlanSu]
                       ,[AllowableSu]
                       ,[AnotherStoreAllowableSu]
                       ,[ReserveSu]
                       ,[InstructionSu]
                       ,[ShippingSu]
                       ,[OriginalStockNO]
                       ,[ExpectReturnDate]
                       ,[ReturnPlanSu]
                       ,[VendorCD]
                       ,[ReturnDate]
                       ,[ReturnSu]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 SELECT
                        @ToStockNO
                       ,@ToSoukoCD
                       ,tbl.ToRackNO   --RackNO
                       ,(CASE @tblUpdateFlg WHEN 0 THEN @ArrivalPlanNO
                            ELSE tbl.ArrivalPlanNO END)    --ArrivalPlanNO
                       ,tbl.SKUCD
                       ,tbl.AdminNO
                       ,tbl.JanCD
                       ,1   --  ArrivalYetFLG(0:���׍ρA1:������)
                       ,3   --ArrivalPlanKBN(1:�󔭒����A2:�������A3:�ړ���)
                       ,NULL    --ArrivalPlanDate
                       ,NULL    --ArrivalDate
                       ,0  --StockSu
                       ,0   --PlanSu
                       ,0   --AllowableSu
                       ,0   --AnotherStoreAllowableSu
                       ,0    --ReserveSu
                       ,0   --InstructionSu
                       ,0   --ShippingSu
                       ,NULL    --OriginalStockNO
                       ,tbl.ExpectReturnDate    --ExpectReturnDate
                       ,tbl.MoveSu  --ReturnPlanSu
                       ,tbl.VendorCD
                       ,NULL    --ReturnDate
                       ,0   --ReturnSu
                 
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL                  
                       ,NULL
                  FROM @Table tbl
                  WHERE tbl.MoveRows = @tblMoveRows
                  ;  
*/
                --�yD_DeliveryPlan�z�z���\����  Table�]���d�lJ�A
                UPDATE [D_DeliveryPlan] SET
                       [Number]         = NULL
                      ,[UpdateOperator] = @Operator  
                      ,[UpdateDateTime] = @SYSDATETIME
                 WHERE @MoveNO = D_DeliveryPlan.Number
                ;
                
                --�yD_DeliveryPlanDetails�z�z���\����@Table�]���d�lK�A
                UPDATE [D_DeliveryPlanDetails] SET
                       [Number]         = NULL
                      ,[NumberRows]     = 0
                      ,[UpdateOperator] = @Operator  
                      ,[UpdateDateTime] = @SYSDATETIME
                      
                 FROM @Table AS tbl
                 WHERE @MoveNO = D_DeliveryPlanDetails.Number
                 AND tbl.MoveRows = D_DeliveryPlanDetails.NumberRows
                ;
            END
        END
    END
    
    --�C�����̍��@�f�A�g�A�i�A�j***************************************************************�C����
    IF @OperateMode = 2
    BEGIN
        IF @MovePurposeType <> @KBN_TENPOKAN  --�ړ��敪<>�X�܊Ԉړ�
        BEGIN
            --�ړ��敪=�ԕi
            IF @MovePurposeType = @KBN_HENPIN
            BEGIN
                --�yD_DeliveryPlan�z�z���\����@�e�[�u���]���d�lJ
                --�`�[�ԍ��̔�
                EXEC Fnc_GetNumber
                    19,        --in�`�[��� 19
                    @MoveDate, --in���
                    @StoreCD,       --in�X��CD
                    @Operator,
                    @DeliveryPlanNO OUTPUT
                    ;
                
                IF ISNULL(@DeliveryPlanNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
                
                INSERT INTO [D_DeliveryPlan]
                       ([DeliveryPlanNO]
                       ,[DeliveryKBN]
                       ,[Number]
                       ,[DeliveryName]
                       ,[DeliverySoukoCD]
                       ,[DeliveryZip1CD]
                       ,[DeliveryZip2CD]
                       ,[DeliveryAddress1]
                       ,[DeliveryAddress2]
                       ,[DeliveryMailAddress]
                       ,[DeliveryTelphoneNO]
                       ,[DeliveryFaxNO]
                       ,[DecidedDeliveryDate]
                       ,[DecidedDeliveryTime]
                       ,[CarrierCD]
                       ,[PaymentMethodCD]
                       ,[CommentInStore]
                       ,[CommentOutStore]
                       ,[InvoiceNO]
                       ,[DeliveryPlanDate]
                       ,[HikiateFLG]
                       ,[IncludeFLG]
                       ,[OntheDayFLG]
                       ,[ExpressFLG]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                SELECT
                        @DeliveryPlanNO
                       ,2 AS DeliveryKBN    --(1:�̔��A2:�q�Ɉړ�)
                       ,@MoveNO AS Number
                       ,NULL    --[DeliveryName]
                       ,@FromSoukoCD    --[DeliverySoukoCD]
                       ,NULL    --[DeliveryZip1CD]
                       ,NULL    --[DeliveryZip2CD]
                       ,NULL    --[DeliveryAddress1]
                       ,NULL    --[DeliveryAddress2]
                       ,NULL    --[DeliveryMailAddress]
                       ,NULL    --[DeliveryTelphoneNO]
                       ,NULL    --[DeliveryFaxNO]
                       ,NULL    --[DecidedDeliveryDate]
                       ,NULL    --[DecidedDeliveryTime]
                       ,NULL    --[CarrierCD]
                       ,NULL    --[PaymentMethodCD]
                       ,NULL    --[CommentInStore]
                       ,NULL    --[CommentOutStore]
                       ,NULL    --[InvoiceNO]
                       ,NULL    --[DeliveryPlanDate]
                       ,1   --[HikiateFLG]
                       ,0   --[IncludeFLG]
                       ,0   --[OntheDayFLG]
                       ,0   --[ExpressFLG]
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                  ;
                  
                --�yD_DeliveryPlanDetails�z�z���\�薾�ׁ@�e�[�u���]���d�lK
                INSERT INTO [D_DeliveryPlanDetails]
                   ([DeliveryPlanNO]
                   ,[DeliveryPlanRows]
                   ,[Number]
                   ,[NumberRows]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[HikiateFLG]
                   ,[UpdateCancelKBN]
                   ,[DeliveryOrderComIn]
                   ,[DeliveryOrderComOut]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
                 SELECT  
                    @DeliveryPlanNO
                   ,tbl.MoveRows AS DeliveryPlanRows
                   ,@MoveNO AS Number
                   ,tbl.MoveRows  As NumberRows
                   ,NULL    --CommentInStore
                   ,NULL    --CommentOutStore
                   ,1       --HikiateFLG
                   ,0       --UpdateCancelKBN
                   ,NULL    --DeliveryOrderComIn
                   ,NULL    --DeliveryOrderComOut                        
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                  FROM @Table tbl
                  WHERE tbl.UpdateFlg <> 2
                  ;
            END

            --���א���Insert��
            --�J�[�\���I�[�v��
            OPEN CUR_TABLE;

            --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_TABLE
            INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
            
            --�f�[�^�̍s�������[�v���������s����
            WHILE @@FETCH_STATUS = 0
            BEGIN
            -- ========= ���[�v���̎��ۂ̏��� ��������===
                IF @tblUpdateFlg <> 2    --�폜�s�ȊO
                BEGIN
                    IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)   --�X�܊Ԉړ����Ȃ��i��ŏ����ς݁j
                    BEGIN
                        SET @ToStockNO = '';
                        --�yD_Stock�z�݌Ɂ@�ړ���@�e�[�u���]���d�l�g

                        --�`�[�ԍ��̔ԁ�StockNO
                        EXEC Fnc_GetNumber
                            21,        --in�`�[��� 21
                            @MoveDate, --in���
                            @StoreCD,       --in�X��CD
                            @Operator,
                            @ToStockNO OUTPUT
                            ;
                            
                        IF ISNULL(@ToStockNO,'') = ''
                        BEGIN
                            SET @W_ERR = 1;
                            RETURN @W_ERR;
                        END
                            
                        --�yD_Stock�zInsert    
                        INSERT INTO [D_Stock]
                               ([StockNO]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[ArrivalPlanNO]
                               ,[SKUCD]
                               ,[AdminNO]
                               ,[JanCD]
                               ,[ArrivalYetFLG]
                               ,[ArrivalPlanKBN]
                               ,[ArrivalPlanDate]
                               ,[ArrivalDate]
                               ,[StockSu]
                               ,[PlanSu]
                               ,[AllowableSu]
                               ,[AnotherStoreAllowableSu]
                               ,[ReserveSu]
                               ,[InstructionSu]
                               ,[ShippingSu]
                               ,[OriginalStockNO]
                               ,[ExpectReturnDate]
                               ,[ReturnPlanSu]
                               ,[VendorCD]
                               ,[ReturnDate]
                               ,[ReturnSu]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                         SELECT
                                @ToStockNO      --��
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD THEN @FromSoukoCD
                                                       ELSE ISNULL(@ToSoukoCD,@FromSoukoCD) END)   --SoukoCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN NULL
                                                       WHEN @KBN_SYOCD THEN tbl.FromRackNO 
                                                       ELSE tbl.ToRackNO END)    --RackNO
                               ,NULL    --ArrivalPlanNO
                               ,tbl.SKUCD
                               ,tbl.AdminNO
                               ,tbl.JanCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 1   --�X�܊Ԉړ��̏ꍇ
                                                       ELSE 0 END)                 --  ArrivalYetFLG(0:���׍ρA1:������)
                               ,3   --ArrivalPlanKBN(1:�󔭒����A2:�������A3:�ړ���)
                               ,NULL    --ArrivalPlanDate
                               ,NULL    --ArrivalDate
                               ,(CASE @MovePurposeType WHEN @KBN_TENPOKAN THEN 0
                                                       WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)   --StockSu
                               ,0  --PlanSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)    --AllowableSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN 0
                                                       ELSE tbl.MoveSu END)    --AnotherStoreAllowableSu
                               ,0    --ReserveSu
                               ,0   --InstructionSu
                               ,0   --ShippingSu
                               ,NULL    --OriginalStockNO
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.ExpectReturnDate
                                                       ELSE NULL END)    --ExpectReturnDate
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.MoveSu
                                                       ELSE 0 END)     --ReturnPlanSu
                               ,(CASE @MovePurposeType WHEN @KBN_HENPIN THEN tbl.VendorCD
                                                       ELSE NULL END) --[VendorCD]
                               ,NULL    --ReturnDate
                               ,0   --ReturnSu
                         
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL                  
                               ,NULL
                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;
                    END

                    SET @WIdoSu = @tblMoveSu;
                    --�yD_Stock�z�݌Ɂ@�ړ����@�e�[�u���]���d�l�f
                    --�ړ������݌ɂň���                    --�J�[�\����`(D_Stock_SelectSuryo�Q��)
                    DECLARE CUR_Stock CURSOR FOR
                        SELECT DS.StockSu
                            ,DS.AllowableSu
                            ,DS.StockNO
                        from D_Stock DS
                        WHERE DS.SoukoCD = @FromSoukoCD
                        AND DS.RackNO = @tblFromRackNO
                        AND DS.AdminNO = @tblAdminNO
                        AND DS.DeleteDateTime is null 
                        AND DS.AllowableSu > 0 
                        AND DS.ArrivalYetFlg = 0 
                        ;
                        
                    --�J�[�\���I�[�v��
                    OPEN CUR_Stock;

                    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
                    FETCH NEXT FROM CUR_Stock
                    INTO @StockSu, @AllowableSu, @StockNO;
                    
                    --�f�[�^�̍s�������[�v���������s����
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                    -- ========= ���[�v���̎��ۂ̏��� ��������===*************************CUR_Stock
                        IF @WIdoSu <= @StockSu AND @WIdoSu <= @AllowableSu
                        BEGIN
                            SET @WUpdSu = @WIdoSu;
                        END
                        ELSE IF @StockSu < @AllowableSu
                        BEGIN
                            SET @WUpdSu = @StockSu;
                        END
                        ELSE
                        BEGIN
                            SET @WUpdSu = @AllowableSu;
                        END
                        
                        UPDATE [D_Stock] SET
                               [StockSu] = [StockSu] - @WUpdSu
                              ,[AllowableSu] = [AllowableSu] - @WUpdSu
                              ,[AnotherStoreAllowableSu] = [AnotherStoreAllowableSu] - @WUpdSu
                              --,[ExpectReturnDate]   = tbl.ExpectReturnDate    2020.09.18 del
                              --,[VendorCD]           = tbl.VendorCD
                              --,[ReturnSu]           = [ReturnSu] - @WUpdSu
                              ,[UpdateOperator]     =  @Operator  
                              ,[UpdateDateTime]     =  @SYSDATETIME
                              
                         FROM D_Stock AS DS
                         INNER JOIN @Table tbl
                         ON tbl.MoveRows = @tblMoveRows
                         WHERE DS.DeleteDateTime IS NULL
                         AND DS.StockNO = @StockNO
                        ;
                        
                        --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�b   @KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL�̂Ƃ���StockNO�́H
                        --C(11),C(15),C(19),C(20),C(22),C(16)
                        INSERT INTO [D_Warehousing]
                           ([WarehousingDate]
                           ,[SoukoCD]
                           ,[RackNO]
                           ,[StockNO]
                           ,[JanCD]
                           ,[AdminNO]
                           ,[SKUCD]
                           ,[WarehousingKBN]
                           ,[DeleteFlg]
                           ,[Number]
                           ,[NumberRow]
                           ,[VendorCD]
                           ,[ToStoreCD]
                           ,[ToSoukoCD]
                           ,[ToRackNO]
                           ,[ToStockNO]
                           ,[FromStoreCD]
                           ,[FromSoukoCD]
                           ,[FromRackNO]
                           ,[CustomerCD]
                           ,[Quantity]
                           ,[Program]
                           ,[InsertOperator]
                           ,[InsertDateTime]
                           ,[UpdateOperator]
                           ,[UpdateDateTime]
                           ,[DeleteOperator]
                           ,[DeleteDateTime])
                        SELECT @MoveDate --WarehousingDate
                           ,@FromSoukoCD AS SoukoCD
                           ,tbl.FromRackNO    --RackNO
                           ,@StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                           ,tbl.JanCD
                           ,tbl.AdminNO
                           ,tbl.SKUCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN 11 
                                                   WHEN @KBN_SYOCD      THEN 31		--15
                                                   WHEN @KBN_CHOSEI_ADD THEN 19
                                                   WHEN @KBN_CHOSEI_DEL THEN 20
                                                   WHEN @KBN_LOCATION   THEN 22
                                                   WHEN @KBN_HENPIN     THEN 16 --
                                                   ELSE 0 END)   --WarehousingKBN
                           ,0  --DeleteFlg
                           ,@NewMoveNO  --Number
                           ,tbl.MoveRows --NumberRow
                           ,NULL   --VendorCD
                           
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStoreCD
                                                   WHEN @KBN_SYOCD      THEN @FromStoreCD
                                                   WHEN @KBN_CHOSEI_ADD THEN @FromStoreCD
                                                   WHEN @KBN_CHOSEI_DEL THEN @FromStoreCD
                                                   WHEN @KBN_LOCATION   THEN @FromStoreCD
                                                   WHEN @KBN_HENPIN     THEN @ToStoreCD		--2020.10.6
                                                   ELSE NULL END)	--ToStoreCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToSoukoCD
                                                   WHEN @KBN_SYOCD      THEN @FromSoukoCD
                                                   WHEN @KBN_CHOSEI_ADD THEN @FromSoukoCD
                                                   WHEN @KBN_CHOSEI_DEL THEN @FromSoukoCD
                                                   WHEN @KBN_LOCATION   THEN @FromSoukoCD
                                                   WHEN @KBN_HENPIN     THEN @ToSoukoCD		--2020.10.6
                                                   ELSE NULL END)	--ToSoukoCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN tbl.ToRackNO
                                                   WHEN @KBN_SYOCD      THEN tbl.FromRackNO
                                                   WHEN @KBN_CHOSEI_ADD THEN tbl.FromRackNO
                                                   WHEN @KBN_CHOSEI_DEL THEN tbl.FromRackNO
                                                   WHEN @KBN_LOCATION   THEN tbl.ToRackNO   --2020.10.29 chg
                                                   WHEN @KBN_HENPIN     THEN tbl.ToRackNO   --2020.09.18 add
                                                   ELSE NULL END)	--ToRackNO
                           
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_SYOCD      THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_CHOSEI_ADD THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_CHOSEI_DEL THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_LOCATION   THEN @StockNO      --(D_Stock)��(�ړ���)�Ɠ����l
                                                   WHEN @KBN_HENPIN     THEN @ToStockNO    -- (D_Stock)��(�ړ���)�Ɠ����l
                                                   ELSE NULL END)  --ToStockNO
                           ,@FromStoreCD	--FromStoreCD
                           ,@FromSoukoCD	--FromSoukoCD
                           ,tbl.FromRackNO	--FromRackNO
                           ,(CASE @MovePurposeType WHEN @KBN_SYOCD      THEN tbl.NewJanCD
                                                   ELSE NULL END)    --CustomerCD
                           ,(CASE @MovePurposeType WHEN @KBN_TENPONAI   THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_SYOCD      THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_CHOSEI_ADD THEN @WUpdSu          --�ړ���(�v���X�l�Ƃ���)
                                                   WHEN @KBN_CHOSEI_DEL THEN @WUpdSu          --�ړ���(�v���X�l�Ƃ���)
                                                   WHEN @KBN_LOCATION   THEN @WUpdSu * (-1)   --Quantity
                                                   WHEN @KBN_HENPIN     THEN @WUpdSu * (-1)   --Quantity
                                                   ELSE @WUpdSu END) 
                           
                           ,@Program  --Program
                           
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL
                           ,NULL

                          FROM @Table tbl
                          WHERE tbl.MoveRows = @tblMoveRows
                          ;
                          
                        IF @MovePurposeType NOT IN (@KBN_CHOSEI_ADD, @KBN_CHOSEI_DEL)
                        BEGIN
                            --�yD_Warehousing�z���o�ɗ����@�e�[�u���]���d�l�c
                            --D(13),D(15),D(22),D(16)
                            INSERT INTO [D_Warehousing]
                               ([WarehousingDate]
                               ,[SoukoCD]
                               ,[RackNO]
                               ,[StockNO]
                               ,[JanCD]
                               ,[AdminNO]
                               ,[SKUCD]
                               ,[WarehousingKBN]
                               ,[DeleteFlg]
                               ,[Number]
                               ,[NumberRow]
                               ,[VendorCD]
                               ,[ToStoreCD]
                               ,[ToSoukoCD]
                               ,[ToRackNO]
                               ,[ToStockNO]
                               ,[FromStoreCD]
                               ,[FromSoukoCD]
                               ,[FromRackNO]
                               ,[CustomerCD]
                               ,[Quantity]
                               ,[Program]
                               ,[InsertOperator]
                               ,[InsertDateTime]
                               ,[UpdateOperator]
                               ,[UpdateDateTime]
                               ,[DeleteOperator]
                               ,[DeleteDateTime])
                            SELECT @MoveDate --WarehousingDate
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN ISNULL(@ToSoukoCD,@FromSoukoCD)
                                                       WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                       ELSE NULL END) AS SoukoCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD    THEN tbl.FromRackNO 
                                                       WHEN @KBN_LOCATION THEN tbl.ToRackNO   --2020.10.29 chg 
                                                       WHEN @KBN_HENPIN   THEN tbl.ToRackNO
                                                       ELSE tbl.ToRackNO END)   --RackNO�@���iCD�t�֎��݈̂ړ����I��
                               ,@ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewJanCD
                                                       ELSE tbl.JanCD END)  --JanCD
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewAdminNO
                                                       ELSE tbl.AdminNO END)    --AdminNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.NewSKUCD
                                                       ELSE tbl.SKUCD END)      --SKUCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN 13 
                                                       WHEN @KBN_SYOCD    THEN 32		--15
                                                       WHEN @KBN_LOCATION THEN 22
                                                       WHEN @KBN_HENPIN   THEN 26 --16 2020/10/01 Fukuda 
                                                       ELSE 0 END)   --WarehousingKBN
                               ,0  --DeleteFlg
                               ,@NewMoveNO  --Number
                               ,tbl.MoveRows --NumberRow
                               ,NULL    --VendorCD
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToStoreCD
                                                       WHEN @KBN_SYOCD    THEN @FromStoreCD
                                                       WHEN @KBN_LOCATION THEN @FromStoreCD
                                                       WHEN @KBN_HENPIN   THEN @ToStoreCD
                                                       ELSE NULL END)
                               
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @ToSoukoCD
                                                       WHEN @KBN_SYOCD    THEN @FromSoukoCD
                                                       WHEN @KBN_LOCATION THEN @FromSoukoCD
                                                       WHEN @KBN_HENPIN   THEN @ToSoukoCD
                                                       ELSE NULL END)
                                                    
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN tbl.ToRackNO
                                                       WHEN @KBN_SYOCD    THEN tbl.FromRackNO
                                                       WHEN @KBN_LOCATION THEN tbl.ToRackNO     --2020.10.29 chg
                                                       WHEN @KBN_HENPIN   THEN tbl.ToRackNO     --2020.09.18 add
                                                       ELSE NULL END)
                                                    
                               ,(CASE @MovePurposeType WHEN @KBN_TENPONAI THEN @StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_SYOCD    THEN @ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_LOCATION THEN @ToStockNO  --(D_Stock)��(�ړ���)�Ɠ����l
                                                       WHEN @KBN_HENPIN   THEN @StockNO    --(D_Stock)��(�ړ���)�Ɠ����l
                                                       ELSE NULL END)
                               ,@FromStoreCD
                               ,@FromSoukoCD
                               ,tbl.FromRackNO
                               ,(CASE @MovePurposeType WHEN @KBN_SYOCD    THEN tbl.JanCD
                                                       ELSE NULL END)    --CustomerCD
                               ,@WUpdSu  --Quantity
                               ,@Program  --Program
                               
                               ,@Operator  
                               ,@SYSDATETIME
                               ,@Operator  
                               ,@SYSDATETIME
                               ,NULL
                               ,NULL

                              FROM @Table tbl
                              WHERE tbl.MoveRows = @tblMoveRows
                              ;
                        END                                        

                        SET @WIdoSu = @WIdoSu - @WUpdSu;
                        
                        IF @WIdoSu = 0
                        BEGIN
                            --���̖��׃��R�[�h��
                            BREAK;
                        END
                        
                        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                        FETCH NEXT FROM CUR_Stock
                        INTO @StockSu, @AllowableSu, @StockNO;
                    END     --LOOP�̏I���***************************************CUR_Stock
                    
                    --�J�[�\�������
                    CLOSE CUR_Stock;
                    DEALLOCATE CUR_Stock;
                        
                END     --�폜�s�ȊO
                
                --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_TABLE
                INTO @tblMoveRows, @tblFromRackNO, @tblAdminNO, @tblMoveSu, @tblUpdateFlg;
            END            --LOOP�̏I���
            
            --�J�[�\�������
            CLOSE CUR_TABLE;
            DEALLOCATE CUR_TABLE;
            
        END		--�ړ��敪<>�X�܊Ԉړ�
    END
	
    --���������f�[�^�֍X�V
    SET @KeyItem = ISNULL(@NewMoveNO,@MoveNO);
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutMoveNO = @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


GO


