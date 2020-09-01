
/****** Object:  StoredProcedure [dbo].[PRC_ShukkaSiziTourokuFromJuchuu]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [PRC_ShukkaSiziTourokuFromJuchuu]
GO

/****** Object:  StoredProcedure [dbo].[PRC_ShukkaSiziTourokuFromJuchuu]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--  ======================================================================
--       Program Call    �o�׎w���o�^
--       Program ID      ShukkaSiziTourokuFromJuchuu
--       Create date:    2020.8.30
--    ======================================================================

CREATE TYPE [T_ShukkaF] AS TABLE(
    [InstructionNO]    [varchar](11) NULL,
    [InstructionRows]  [int] NULL,
    [InstructionKBN]   [tinyint] NULL,
    [DeliveryPlanNO]   [varchar](11) NULL,
    [GyoNO]            [int] NULL,
    [DeliveryPlanDate] [date] NULL,
    [CarrierCD]        [varchar](3) NULL,
    [CommentOutStore]  [varchar](80) NULL,
    [CommentInStore]   [varchar](80) NULL,
    [ExpressFLG]       [tinyint] NULL,
    [UntinFlg]         [tinyint] NULL,
    [UpdateFlg]        [tinyint] NULL
)
GO

CREATE PROCEDURE [PRC_ShukkaSiziTourokuFromJuchuu]
    (@StoreCD         varchar(4),
    @Table            T_ShukkaF READONLY,
    @Operator         varchar(10),
    @PC               varchar(30),
    @OutInstructionNO varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR         tinyint;
    DECLARE @SYSDATETIME   datetime;
    DECLARE @SYSDATE       date;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem       varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @SYSDATE = CONVERT(date,@SYSDATETIME);

    --�J�[�\����`
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.InstructionNO, tbl.InstructionRows, tbl.InstructionKBN, tbl.GyoNO 
            ,CONVERT(varchar, tbl.DeliveryPlanDate, 111) AS DeliveryPlanDate
            ,tbl.DeliveryPlanNO, tbl.UntinFlg
        FROM @Table tbl
        ORDER BY tbl.DeliveryPlanNO, tbl.GyoNO
        ;
    DECLARE @InstructionNO    varchar(11);
    DECLARE @InstructionRows  int;
    DECLARE @InstructionKBN   tinyint;
    DECLARE @GyoNO            int;
    DECLARE @DeliveryPlanDate varchar(10);
    DECLARE @UntinFlg         tinyint;
    DECLARE @DeliveryPlanNO   varchar(11);
	DECLARE @OldDeliveryPlanNO   varchar(11);
	
    DECLARE @D_InstructionDetailsInserted TABLE (
         InstructionRows int
         ,ReserveNO varchar(11)
         );
         
    --�J�[�\���I�[�v��
    OPEN CUR_AAA;

	SET @OldDeliveryPlanNO = '';
	
    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_AAA
    INTO @InstructionNO, @InstructionRows, @InstructionKBN, @GyoNO, @DeliveryPlanDate, @DeliveryPlanNO, @UntinFlg 
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===
        --Form.Detail.�o�׎w���ԍ���""�̎� �i�o�׎w���f�[�^���쐬�j
        IF ISNULL(@InstructionNO,'') = ''
        BEGIN
        	--�Y���̏o�׎w���`�[���폜���A�֌W����e�[�u�����X�V����B���̌㓯��o�א�/�o�ח\����ŏo�׎w���`�[���쐬����B
            UPDATE [D_Stock]
               SET [ReserveSu] = [D_Stock].[ReserveSu] - (CASE ISNULL(W.SetKBN,0) WHEN 1 THEN W.ReserveSu*W.SetSu
                                ELSE W.ReserveSu END)   
                  ,[InstructionSu] = [D_Stock].[InstructionSu] + (CASE ISNULL(W.SetKBN,0) WHEN 1 THEN W.ReserveSu*W.SetSu
                                ELSE W.ReserveSu END)   
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM (SELECT A.StockNO
                    ,C.SetKBN
                    ,A.ReserveSu
                    ,C.SetSu
                FROM D_Reserve AS A
                LEFT OUTER JOIN D_Stock AS B
                ON B.StockNO = A.StockNO
                AND B.DeleteDateTime IS NULL
                LEFT OUTER JOIN F_SKU(@DeliveryPlanDate) AS C
                ON C.AdminNO = A.AdminNO                   
                AND C.DeleteFlg = 0
                
                WHERE A.ShippingOrderNO = @InstructionNO
                AND A.ShippingOrderRows = @InstructionRows

            ) AS W
            WHERE D_Stock.StockNO = W.StockNO
            AND D_Stock.DeleteDateTime IS NULL
            ;
        	
        	UPDATE [D_Reserve] 
                SET [UpdateOperator]   =  @Operator  
                   ,[UpdateDateTime]   =  @SYSDATETIME
                   ,[ShippingPlanDate] = NULL
                   ,[ShippingOrderNO]  = NULL
                   ,[ShippingOrderRows]= 0
             WHERE D_Reserve.ShippingOrderNO = @InstructionNO
               AND D_Reserve.ShippingOrderRows = @InstructionRows
               AND D_Reserve.DeleteDateTime IS NULL
             ;

            UPDATE [D_JuchuuDetails]
               SET [DeliveryOrderSu] = B.ReserveSu * (-1)
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_Reserve AS B
            WHERE B.Number = D_JuchuuDetails.JuchuuNO
            AND B.NumberRows = D_JuchuuDetails.JuchuuRows
            AND B.ReserveKBN = 1
            AND B.DeleteDateTime IS NULL
            AND B.ShippingOrderNO = @InstructionNO
            AND B.ShippingOrderRows = @InstructionRows
            AND D_JuchuuDetails.DeleteDateTime IS NULL
            ;
            
            DELETE FROM D_Instruction
            WHERE InstructionNO = @InstructionNO
            ;
            
            DELETE FROM D_InstructionDetails
            WHERE InstructionNO = @InstructionNO
            ;
            
        END
        
        --�ȉ���Form.Detail.�o�׎w���ԍ���""�̎����X�V(The following is also updated when Form.Detail.�o�׎w���ԍ� = "")

        --D_Instruction Insert�� "�V�K" D_Instruction Update�� "�ύX"
        SET @OperateModeNm = '�V�K';    

        IF @OldDeliveryPlanNO <> @DeliveryPlanNO
        BEGIN
    
            --�yD_Instruction�zInsert Table�]���d�l�` �o�׎w��
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                14,          --in�`�[��� 14 �o�׎w��
                @DeliveryPlanDate, --in���
                @StoreCD,    --in�X��CD
                @Operator,
                @InstructionNO OUTPUT
                ;
            
            IF ISNULL(@InstructionNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            INSERT INTO [D_Instruction]
               ( [InstructionNO]
              ,[DeliveryPlanNO]
              ,[InstructionKBN]
              ,[InstructionDate]
              ,[DeliveryPlanDate]
              ,[FromSoukoCD]
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
              ,[CashOnDelivery]
              ,[PaymentMethodCD]
              ,[CommentOutStore]
              ,[CommentInStore]
              ,[InvoiceNO]
              ,[OntheDayFLG]
              ,[ExpressFLG]
              ,[PrintDate]
              ,[PrintStaffCD]
              ,[InsertOperator]
              ,[InsertDateTime]
              ,[UpdateOperator]
              ,[UpdateDateTime]
              ,[DeleteOperator]
              ,[DeleteDateTime]
            )SELECT
               @InstructionNO
               ,tbl.DeliveryPlanNO
               ,DD.DeliveryKBN  --InstructionKBN
               ,@SYSDATE    --InstructionDate
               ,tbl.DeliveryPlanDate
               ,(SELECT top 1 DR.SoukoCD    --FromSoukoCD
                           FROM  D_Reserve AS DR
                           INNER JOIN D_DeliveryPlanDetails AS DM
                           ON DM.DeliveryPlanNO = DD.DeliveryPlanNO
                           WHERE DR.Number = DM.Number
                            AND DR.NumberRows = DM.NumberRows
                            AND DR.DeleteDateTime IS NULL
                            ORDER BY DM.DeliveryPlanRows, DR.ReserveNO)
               ,DD.DeliveryName
               ,DD.DeliverySoukoCD
               ,DD.DeliveryZip1CD
               ,DD.DeliveryZip2CD
               ,DD.DeliveryAddress1
               ,DD.DeliveryAddress2
               ,DD.DeliveryMailAddress
               ,DD.DeliveryTelphoneNO
               ,DD.DeliveryFaxNO
               ,DD.DecidedDeliveryDate
               ,DD.DecidedDeliveryTime
               ,tbl.CarrierCD
               ,0   --CashOnDelivery
               ,DD.PaymentMethodCD
               ,tbl.CommentOutStore
               ,tbl.CommentInStore
               ,DD.InvoiceNO
               ,0 AS OntheDayFLG
               ,0 AS ExpressFLG
               ,NULL    --PrintDate
               ,NULL    --PrintStaffCD
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM @Table AS tbl
            INNER JOIN D_DeliveryPlan AS DD
            ON DD.DeliveryPlanNO = tbl.DeliveryPlanNO   --���L�����Z�������쐬
            AND DD.DeliveryKBN = tbl.InstructionKBN

            WHERE tbl.DeliveryPlanNO = @DeliveryPlanNO  
            ;
            
            --�yL_Log�zInsert Table�]���d�l�y�i�������[�h�F�V�K�j�������� 
            --���������f�[�^�֍X�V
            SET @KeyItem = @InstructionNO;

            EXEC L_Log_Insert_SP
                @SYSDATETIME,
                @Operator,
                'ShukkaSiziTourokuFromJuchuu',
                @PC,
                @OperateModeNm,
                @KeyItem;
        END
        
        --�yD_InstructionDetails�zInsert  Table�]���d�l�a�@ �o�׎w������
        INSERT INTO [D_InstructionDetails]
           ([InstructionNO]
           ,[InstructionRows]
           ,[InstructionKBN]
           ,[ReserveNO]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[InstructionSu]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        OUTPUT INSERTED.InstructionRows, INSERTED.ReserveNO INTO @D_InstructionDetailsInserted(InstructionRows,ReserveNO)
        SELECT
            @InstructionNO
           ,ROW_NUMBER() OVER(ORDER BY DD.DeliveryPlanNO, DM.DeliveryPlanRows, DM.Number, DM.NumberRows)
           ,DD.DeliveryKBN   --InstructionKBN
           ,DR.ReserveNO
           ,DR.SKUCD
           ,ISNULL(DR.AdminNO,0)
           ,DR.JanCD
           ,tbl.CommentOutStore
           ,tbl.CommentInStore
           ,ISNULL(DR.ReserveSu,0) --InstructionSu
           ,@Operator
           ,@SYSDATETIME
           ,@Operator
           ,@SYSDATETIME
           ,NULL --DeleteOperator
           ,NULL --DeleteDateTime
        FROM @Table AS tbl
        INNER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = tbl.DeliveryPlanNO	--���L�����Z�������쐬
        AND DD.DeliveryKBN = tbl.InstructionKBN
        INNER JOIN D_DeliveryPlanDetails AS DM
        ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
        LEFT OUTER JOIN D_Reserve AS DR
        ON DR.Number = DM.Number
        AND DR.NumberRows = DM.NumberRows
        AND DR.DeleteDateTime IS NULL
        WHERE tbl.DeliveryPlanNO = @DeliveryPlanNO
        ORDER BY DD.DeliveryPlanNO, DM.DeliveryPlanRows, DM.Number, DM.NumberRows
        ;

        IF @OldDeliveryPlanNO <> @DeliveryPlanNO
        BEGIN
            SET @OldDeliveryPlanNO = @DeliveryPlanNO;
            
            --�yD_Reserve�z      Update Table�]���d�l�b ����
            UPDATE D_Reserve
                SET [UpdateOperator]   =  @Operator  
                   ,[UpdateDateTime]   =  @SYSDATETIME
                   ,[ShippingOrderNO]  =  @InstructionNO
                   ,[ShippingOrderRows]= (SELECT DI.InstructionRows
                                          FROM @D_InstructionDetailsInserted AS DI
                                          WHERE DI.ReserveNO = D_Reserve.ReserveNO
                                          )
             FROM D_DeliveryPlanDetails AS DM
             INNER JOIN @Table AS tbl
             ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
             AND tbl.DeliveryPlanNO = @DeliveryPlanNO
             WHERE D_Reserve.Number = DM.Number
                AND D_Reserve.NumberRows = DM.NumberRows
                AND D_Reserve.DeleteDateTime IS NULL
             ;
                     
            --�yD_Stock�z        Update Table�]���d�l�c �݌�
            UPDATE [D_Stock]
               SET [ReserveSu] = [D_Stock].[ReserveSu] - (CASE ISNULL(F.SetKBN,0) WHEN 1 THEN DR.ReserveSu*F.SetSu
                                                            ELSE DR.ReserveSu END) 
                  ,[InstructionSu] = [D_Stock].[InstructionSu] + (CASE ISNULL(F.SetKBN,0) WHEN 1 THEN DR.ReserveSu*F.SetSu
                                                            ELSE DR.ReserveSu END)    
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_DeliveryPlanDetails AS DM
            INNER JOIN @Table AS tbl
            ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND tbl.DeliveryPlanNO = @DeliveryPlanNO
            INNER JOIN D_Reserve AS DR
            ON DR.Number = DM.Number
            AND DR.NumberRows = DM.NumberRows
            AND DR.DeleteDateTime IS NULL
            LEFT OUTER JOIN F_SKU(@SYSDATE) AS F
            ON F.AdminNO = DR.AdminNO
            AND F.DeleteFlg = 0
            WHERE D_Stock.StockNO = DR.StockNO
            AND D_Stock.DeleteDateTime IS NULL
            ;
        
            --�yD_JuchuuDetails�zUpdate Table�]���d�l�d �󒍖���    
            UPDATE [D_JuchuuDetails]
               SET [DeliveryOrderSu] = [D_JuchuuDetails].[DeliveryOrderSu] + DR.ReserveSu  
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_DeliveryPlanDetails AS DM
            INNER JOIN @Table AS tbl
            ON DM.DeliveryPlanNO = tbl.DeliveryPlanNO
            AND tbl.DeliveryPlanNO = @DeliveryPlanNO
            INNER JOIN D_Reserve AS DR
            ON DR.Number = DM.Number
            AND DR.NumberRows = DM.NumberRows
            AND DR.DeleteDateTime IS NULL
            WHERE D_JuchuuDetails.JuchuuNO = DM.Number
            AND D_JuchuuDetails.JuchuuRows = DM.NumberRows
            AND D_JuchuuDetails.DeleteDateTime IS NULL
            ;

        END
        
        --�ȉ���Form.Detail.�^�����גǉ�CheckBox��ON�̖��ׂɂ��āA����o�א�/�o�ח\����̏o�׎w���`�[�̍ŏI�s�ɉ^�����ׂ�ǉ�
        IF @UntinFlg = 1
        BEGIN
            ---�yD_InstructionDetails�zInsert Table�]���d�l�a�A �o�׎w������(�^������)
            INSERT INTO [D_InstructionDetails]
               ([InstructionNO]
               ,[InstructionRows]
               ,[InstructionKBN]
               ,[ReserveNO]
               ,[SKUCD]
               ,[AdminNO]
               ,[JanCD]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[InstructionSu]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @InstructionNO
               ,(SELECT MAX(InstructionRows)+1 FROM D_InstructionDetails WHERE InstructionNO = @InstructionNO)
               ,1   --InstructionKBN
               ,NULL AS ReserveNO
               ,F.SKUCD
               ,F.AdminNO
               ,F.JanCD
               ,NULl AS CommentOutStore
               ,NULL AS CommentInStore
               ,1 --InstructionSu
               ,@Operator
               ,@SYSDATETIME
               ,@Operator
               ,@SYSDATETIME
               ,NULL --DeleteOperator
               ,NULL --DeleteDateTime
            FROM @Table AS tbl
            INNER JOIN M_MultiPorpose AS MM
            ON MM.ID = 227
            AND MM.[KEY] = 1
            INNER JOIN F_SKU(@SYSDATE) AS F
            ON F.AdminNO = MM.Num1
            AND F.DeleteFlg = 0
            WHERE tbl.GyoNO = @GyoNO
            ;
        END
        

        
        -- ========= ���[�v���̎��ۂ̏��� �����܂�===

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
    	INTO @InstructionNO, @InstructionRows, @InstructionKBN, @GyoNO, @DeliveryPlanDate, @DeliveryPlanNO, @UntinFlg
    

    END
    
    --�J�[�\�������
    CLOSE CUR_AAA;
	DEALLOCATE CUR_AAA;

    SET @OutInstructionNO = @InstructionNO;
    
--<<OWARI>>
  return @W_ERR;

END


GO

