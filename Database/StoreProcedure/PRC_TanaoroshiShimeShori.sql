 BEGIN TRY 
 Drop Procedure dbo.[PRC_TanaoroshiShimeShori]
END try
BEGIN CATCH END CATCH 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [PRC_TanaoroshiShimeShori]    */
CREATE PROCEDURE PRC_TanaoroshiShimeShori(
    -- Add the parameters for the stored procedure here
    @Syori    tinyint,        -- �����敪�i1:������,2:�������L�����Z��,3:�����m��j
    @SoukoCD  varchar(6),
    @InventoryDate varchar(10),
    @FromRackNO varchar(13),
    @ToRackNO varchar(13),
    @StoreCD  varchar(4),
    @Operator  varchar(10),
    @PC  varchar(30)
)AS
BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @InventoryNO  varchar(11);
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    DECLARE @Program varchar(100); 

    DECLARE @DifferenceQuantity int;
    DECLARE @RackNO varchar(11);
    DECLARE @AdminNO int;
    DECLARE @StockNO varchar(11);
    DECLARE @StockNO_B varchar(11);
    DECLARE @StockSu int;
    DECLARE @AjustSu int;	--���I��������
    DECLARE @wDifferenceQuantity int;	--�m�莞�Ɏg�p
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @Program = 'TanaoroshiShimeShori';	

    --�`�[�ԍ��̔�
    EXEC Fnc_GetNumber
        28,             --in�`�[��� 28
        @InventoryDate,    --in���
        @StoreCD,       --in�X��CD
        @Operator,
        @InventoryNO OUTPUT
        ;
    
    IF ISNULL(@InventoryNO,'') = ''
    BEGIN
        SET @W_ERR = 1;
        RETURN @W_ERR;
    END
    
    --���P���쐬    �I�������f�[�^(��)
    --D_InventoryProcessing Insert  Table�]���d�l�`
    INSERT INTO D_InventoryProcessing
    ([InventoryNO]
      ,[SoukoCD]
      ,[FromRackNO]
      ,[ToRackNO]
      ,[InventoryDate]
      ,[InventoryKBN]
      ,[ProcessingDateTime]
      ,[StaffCD]
      ,[InsertOperator]
      ,[InsertDateTime]
      ,[UpdateOperator]
      ,[UpdateDateTime]
      ,[DeleteOperator]
      ,[DeleteDateTime])
  	VALUES (
  	   @InventoryNO
      ,@SoukoCD
      ,@FromRackNO
      ,@ToRackNO
      ,@InventoryDate
      ,@Syori 			--InventoryKBN
      ,@SYSDATETIME		--ProcessingDateTime
      ,@Operator	--StaffCD
      ,@Operator  
      ,@SYSDATETIME
      ,@Operator  
      ,@SYSDATETIME
      ,NULL                  
      ,NULL 
    );
    
    --�I����--
    IF @Syori = 1
    BEGIN
        SET @OperateModeNm = '�I����';

        --�I���f�[�^�쐬
        --D_Inventory Insert    Table�]���d�l�a
        INSERT INTO [D_Inventory]
           ([SoukoCD]
           ,[RackNO]
           ,[InventoryDate]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[TheoreticalQuantity]
           ,[ActualQuantity]
           ,[DifferenceQuantity]
           ,[InventoryNO]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        SELECT 
            DS.SoukoCD
           ,DS.RackNO
           ,@InventoryDate
           ,MAX(DS.SKUCD)
           ,DS.AdminNO
           ,MAX(DS.JanCD)
           ,SUM(DS.AllowableSu) AS TheoreticalQuantity
           ,SUM(DS.AllowableSu) AS ActualQuantity
           ,0 AS DifferenceQuantity
           ,@InventoryNO
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL 
        FROM D_Stock AS DS
        WHERE DS.SoukoCD = @SoukoCD
        AND DS.DeleteDateTime IS NULL
        AND ISNULL(DS.RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(DS.RackNO,'') END)
        AND ISNULL(DS.RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(DS.RackNO,'') END)
        AND DS.RackNO IS Not NULL
        AND DS.ArrivalYetFLG = 0
        GROUP BY DS.SoukoCD, DS.RackNO, DS.AdminNO
        ;
        
        --�I������f�[�^�쐬
        --D_InventoryControl Insert Table�]���d�l�b
        INSERT INTO [D_InventoryControl]
            ([SoukoCD]
            ,[RackNO]
            ,[InventoryDate]
            ,[InventoryKBN]
            ,[InventoryNO]
            ,[AdditionDateTime]
            ,[AdditionStaffCD]
            ,[InsertOperator]
            ,[InsertDateTime]
            ,[UpdateOperator]
            ,[UpdateDateTime]
            ,[DeleteOperator]
            ,[DeleteDateTime])
        SELECT
             ML.SoukoCD
            ,ML.TanaCD AS RackNO
            ,@InventoryDate
            ,1 AS InventoryKBN
            ,@InventoryNO
            ,NULL AS AdditionDateTime
            ,NULL AS AdditionStaffCD
            ,@Operator  
            ,@SYSDATETIME
            ,@Operator  
            ,@SYSDATETIME
            ,NULL                  
            ,NULL 
        FROM F_Location(@InventoryDate) AS ML
        WHERE ML.SoukoCD = @SoukoCD
        AND ML.DeleteFlg = 0
        AND ML.TanaCD >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(ML.TanaCD,'') END)
        AND ML.TanaCD <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(ML.TanaCD,'') END)
        ;
		
    END
    
    --�I������ݾ�--
    ELSE IF @Syori = 2
    BEGIN
        SET @OperateModeNm = '�I������ݾ�';

        --�I���f�[�^�폜
        --D_Inventory Delete    Table�]���d�l�a�A
        DELETE FROM [D_Inventory]
        WHERE SoukoCD = @SoukoCD
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        AND InventoryDate = @InventoryDate
        ;
        
        --�I������f�[�^�폜
        DELETE FROM [D_InventoryControl]
        WHERE SoukoCD = @SoukoCD
        AND InventoryDate = @InventoryDate
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        ;
        /*
        --D_InventoryControl UPDATE     Table�]���d�l�b�A
        UPDATE [D_InventoryControl] SET
            InventoryKBN = 3
            ,AdditionDateTime = @SYSDATETIME
            ,AdditionStaffCD = @Operator
            ,UpdateOperator = @Operator
            ,UpdateDateTime = @SYSDATETIME
        WHERE SoukoCD = @SoukoCD
        AND InventoryDate = @InventoryDate
        AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
        AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
        ;
        */
    END
    
    --�I���m��--
    ELSE IF @Syori = 3
    BEGIN
    	--�I���f�[�^�̍��ِ����O�̃f�[�^���擾���A�݌Ƀf�[�^�����ِ����O�ɂȂ�܂ŏ���Update

        --�J�[�\����`
        DECLARE CUR_AAA CURSOR FOR
            SELECT DI.DifferenceQuantity
                  ,DI.RackNO          
                  ,DI.AdminNO     
              FROM D_Inventory AS DI
             WHERE DI.SoukoCD = @SoukoCD
               AND DI.InventoryDate = @InventoryDate
               AND ISNULL(DI.RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND ISNULL(DI.RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(DI.RackNO,'') END)
               AND DI.DifferenceQuantity <> 0
             ORDER BY DI.RackNO
               ;
                
        --�J�[�\���I�[�v��
        OPEN CUR_AAA;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO @DifferenceQuantity, @RackNO, @AdminNO;
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN

    -- ========= ���[�v���̎��ۂ̏��� ��������===
            --��A.DifferenceQuantity��0�̎� ���ِ����v���X�̎�
            IF @DifferenceQuantity > 0
            BEGIN
                --���I������������A.DifferenceQuantity
                --1���ڂɂ̂݁A�X�V
                SET @StockNO = (SELECT top 1 DS.StockNO
                                FROM D_Stock AS DS
                                WHERE DS.SoukoCD = @SoukoCD
                                AND DS.RackNO = @RackNO
                                AND DS.AdminNO = @AdminNO
                                AND DS.ArrivalYetFLG = 0
                                AND DS.AllowableSu > 0
                                ORDER BY DS.AllowableSu desc
                                        ,DS.ArrivalDate
                                        ,DS.StockNO
                                );
                
                --�݌Ƀf�[�^�X�V(1��)                
                --D_Zaiko   Update  Table�]���d�l�c 
                UPDATE D_Stock SET
                     StockSu = StockSu + @DifferenceQuantity
                    ,AllowableSu = AllowableSu + @DifferenceQuantity
                    ,AnotherStoreAllowableSu = AnotherStoreAllowableSu + @DifferenceQuantity
                    ,UpdateOperator = @Operator
                    ,UpdateDateTime = @SYSDATETIME
                WHERE StockNO = @StockNO
                ;
                
                --���o�ɗ����쐬
                --D_Warehousing Insert  Table�]���d�l�d
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
                   ,[UnitPrice]
                   ,[Amount]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
                SELECT @InventoryDate --WarehousingDate
                   ,DS.SoukoCD
                   ,DS.RackNO
                   ,@StockNO
                   ,DS.JanCD
                   ,DS.AdminNO
                   ,DS.SKUCD
                   ,17                    --WarehousingKBN   DifferenceQuantity��0�̎� 17:�I�����Z�A
                   ,0                     --DeleteFlg
                   ,@InventoryNO          --Number
                   ,0                     --NumberRow
                   ,NULL                  --VendorCD
                   ,NULL                  --ToStoreCD
                   ,NULL                  --ToSoukoCD
                   ,NULL                  --ToRackNO
                   ,NULL                  --ToStockNO
                   ,NULL                  --FromStoreCD
                   ,NULL                  --FromSoukoCD]
                   ,NULL                  --FromRackNO
                   ,NULL                  --CustomerCD
                   ,@DifferenceQuantity   --Quantity
                   ,0                     --UnitPrice
                   ,0                     --Amount
                   ,@Program              --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL

                  FROM D_Stock AS DS
                  WHERE StockNO = @StockNO
                  ;
            END
            ELSE    --��A.DifferenceQuantity��0�̎� ���ِ����}�C�i�X�̎�
            BEGIN
            	SET @wDifferenceQuantity = @DifferenceQuantity;

                --�J�[�\����`
                DECLARE CUR_AAA2 CURSOR FOR
                    --�݌ɐ��|��������0�̍݌Ƀf�[�^���擾
                    SELECT top 1 DS.StockNO--, DS.StockSu - DS.ReserveSu AS StockSu
                          ,DS.AllowableSu AS StockSu
                    FROM D_Stock AS DS
                    WHERE DS.SoukoCD = @SoukoCD
                    AND DS.RackNO = @RackNO
                    AND DS.AdminNO = @AdminNO
                    AND DS.ArrivalYetFLG = 0
                    AND DS.AllowableSu > 0
                    --AND DS.StockSu - DS.ReserveSu > 0
                    ORDER BY DS.AllowableSu desc	--(DS.StockSu - DS.ReserveSu) desc
                            ,DS.ArrivalDate
                            ,DS.StockNO
                    ;

                --�J�[�\���I�[�v��
                OPEN CUR_AAA2;

                --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
                FETCH NEXT FROM CUR_AAA2
                INTO @StockNO_B, @StockSu;
                
                --�f�[�^�̍s�������[�v���������s����
                WHILE @@FETCH_STATUS = 0
                BEGIN
                -- ========= ���[�v���̎��ۂ̏��� ��������===
                    --��A.DifferenceQuantity�~(-1)����B.StockSu�|��B.ReserveSu�̎�
                    IF -1*@wDifferenceQuantity > @StockSu
                    BEGIN
                        --���I������������B.StockSu�|��B.ReserveSu
                        SET @AjustSu = @StockSu;
                    END
                    ELSE    --��A.DifferenceQuantity�~(-1)����B.StockSu�|��B.ReserveSu�̎�
                    BEGIN
                        --���I����������(-1)�~��A.DifferenceQuantity
                        SET @AjustSu = -1*@wDifferenceQuantity;
                    END
                    
                    --�݌Ƀf�[�^�X�V
                    --D_Stock Update Table�]���d�l�c�A
                    UPDATE D_Stock SET
                         StockSu = StockSu - @AjustSu        --���I�������������Z
                        ,AllowableSu = AllowableSu - @AjustSu
                        ,AnotherStoreAllowableSu = AnotherStoreAllowableSu - @AjustSu
                        ,UpdateOperator = @Operator
                        ,UpdateDateTime = @SYSDATETIME
                    WHERE StockNO = @StockNO_B
                    ;
                    
                    --���o�ɗ����쐬
                    --D_Warehousing	Insert	Table�]���d�l�d
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
                       ,[UnitPrice]
                       ,[Amount]
                       ,[Program]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                    SELECT @InventoryDate --WarehousingDate
                       ,DS.SoukoCD
                       ,DS.RackNO
                       ,@StockNO
                       ,DS.JanCD
                       ,DS.AdminNO
                       ,DS.SKUCD
                       ,18            --WarehousingKBN   DifferenceQuantity��0�̎� 18:�I�����Z
                       ,0             --DeleteFlg
                       ,@InventoryNO  --Number
                       ,0             --NumberRow
                       ,NULL          --VendorCD
                       ,NULL          --ToStoreCD
                       ,NULL          --ToSoukoCD
                       ,NULL          --ToRackNO
                       ,NULL          --ToStockNO
                       ,NULL          --FromStoreCD
                       ,NULL          --FromSoukoCD]
                       ,NULL          --FromRackNO
                       ,NULL          --CustomerCD
                       ,-1*@AjustSu   --Quantity ���݌ɒ�����
                                      --DifferenceQuantity��0�̎��́A�~(-1)
                       ,0             --UnitPrice
                       ,0             --Amount
                       ,@Program      --Program
                       
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL
                       ,NULL

                      FROM D_Stock AS DS
                      WHERE StockNO = @StockNO_B
                      ;
                      
                    --��A.DifferenceQuantity�@���Ɂ��I�������������Z
                    SET @wDifferenceQuantity = @wDifferenceQuantity + @AjustSu;
                    
                    --��A.DifferenceQuantity�@��0�ɂȂ�܂ōX�V����
                    IF @wDifferenceQuantity >= 0
                    BEGIN
                        BREAK;
                    END
                
                    --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
                    FETCH NEXT FROM CUR_AAA2
                    INTO @StockNO_B, @StockSu;
                -- ========= ���[�v���̎��ۂ̏��� �����܂�===
                END
                --�J�[�\�������
                CLOSE CUR_AAA2;
                DEALLOCATE CUR_AAA2;
                
                --�I������f�[�^�쐬
                --D_InventoryControl UPDATE     Table�]���d�l�b�A
                UPDATE [D_InventoryControl] SET
                    InventoryKBN = 3
                    ,AdditionDateTime = @SYSDATETIME
                    ,AdditionStaffCD = @Operator
                    ,UpdateOperator = @Operator
                    ,UpdateDateTime = @SYSDATETIME
                WHERE SoukoCD = @SoukoCD
                AND InventoryDate = @InventoryDate
                AND ISNULL(RackNO,'') >= (CASE WHEN ISNULL(@FromRackNO,'') <> '' THEN @FromRackNO ELSE ISNULL(RackNO,'') END)
                AND ISNULL(RackNO,'') <= (CASE WHEN ISNULL(@ToRackNO,'') <> '' THEN @ToRackNO ELSE ISNULL(RackNO,'') END)
                ;
            END
            -- ========= ���[�v���̎��ۂ̏��� �����܂�===

            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_AAA
            INTO @DifferenceQuantity, @RackNO, @AdminNO;
        END
        
        --�J�[�\�������
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
    END
	
	
    --�yL_Log�zINSERT
    --���������f�[�^�֍X�V
    SET @KeyItem = CONVERT(varchar,@Syori) + ',' + @SoukoCD 
            + ',' + @InventoryDate + ',' + ISNULL(@FromRackNO,'') + ',' + ISNULL(@ToRackNO,'');
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

END

GO

