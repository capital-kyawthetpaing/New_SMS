IF OBJECT_ID ( 'PRC_HikiateHenkouNyuuryoku', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_HikiateHenkouNyuuryoku]
GO
IF EXISTS (select * from sys.table_types where name = 'T_Zaiko')
    Drop TYPE dbo.[T_Zaiko]
GO

IF EXISTS (select * from sys.table_types where name = 'T_Reserve')
    Drop TYPE dbo.[T_Reserve]
GO

--  ======================================================================
--       Program Call    �����ύX����
--       Program ID      HikiateHenkouNyuuryoku
--       Create date:    2020.04.05
--    ======================================================================

CREATE TYPE T_Zaiko AS TABLE
    (
    [StockNO] [varchar](11) ,
    [SoukoCD] [varchar](6),
    [SoukoName] [varchar](40),
    [ArrivalPlanNO] [varchar](11) ,
    [SKUCD] [varchar](30),
    [AdminNO] [int],
    [JanCD] [varchar](13),
    [ArrivalYetFLG] [tinyint],
    [OrderDate] [date],
    [ArrivalPlanDate] [date],
    [ArrivalDate] [date],
    [StockSu] [int] ,
    [PlanSu] [int] ,
    [AllowableSu] [int] ,
    [SelectAllowableSu] [int] ,
    [AnotherStoreAllowableSu] [int] ,
    [ReserveSu] [int] ,
    [SelectReserveSu] [int] ,
    [InstructionSu] [int] ,
    [ArrivalPlanKBN] [tinyint],
    [OrderNO] [varchar](11) ,
    [OrderCD] [varchar](13) ,
    [VendorName] [varchar](50)
    )
GO


CREATE TYPE T_Reserve AS TABLE
    (
    [Seq] [int],
    [StockNO] [varchar](11) ,
    [ReserveNO] [varchar](11) ,
    [ReserveKBN] [tinyint],
    [Number] [varchar](11) ,
    [NumberRows] [int],
    [SoukoCD] [varchar](6),
    [SKUCD] [varchar](30),
    [AdminNO] [int],
    [JanCD] [varchar](13),
    [ReserveSu] [int] ,
    [SelectReserveSu] [int] ,
    [ShippingOrderNO] [varchar](11) ,
    [ShippingOrderRows] [int],
    [InstructionSu] [int] ,
    [ShippingSu] [int] ,
    [ReturnKBN] [tinyint],
    [OriginalReserveNO] [varchar](11) ,
    [JuchuuDate] [date],
    [CustomerCD] [varchar](13),
    [CustomerName] [varchar](80),
    [JuchuuSuu] [int] ,
    [AllReserveSu] [int] ,
    [AllShippingSu] [int] ,
    [JuchuuKBN] [tinyint]
    )
GO



--********************************************--
--                                            --
--        �X�V                                --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_HikiateHenkouNyuuryoku
    (@StoreCD   varchar(4),
     @SKUCD     varchar(30),
     @ZTable    T_Zaiko READONLY,
     @RTable    T_Reserve READONLY,
     @Operator  varchar(10),
     @PC  varchar(30),
     
     @OutSKUCD varchar(30) OUTPUT

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
    DECLARE @SYSDATE date;
    DECLARE @ReserveNO varchar(11);
    DECLARE @CNT int;

    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(date, @SYSDATETIME);
    
    -- �݌Ƀe�[�u���i�e�[�u���]���d�lA�j
    UPDATE D_Stock
       SET ReserveSu = tbl.ReserveSu
          ,AllowableSu = tbl.AllowableSu
          --,AllowableSu = tbl.AllowableSu + tbl.SelectReserveSu - tbl.ReserveSu
          ,AnotherStoreAllowableSu = tbl.AnotherStoreAllowableSu  + tbl.SelectReserveSu - tbl.ReserveSu
          ,UpdateOperator  =  @Operator  
          ,UpdateDateTime  =  @SYSDATETIME
      FROM @ZTable AS tbl
     INNER JOIN D_Stock AS DS ON tbl.StockNO = DS.StockNO
     WHERE tbl.ReserveSu <> tbl.SelectReserveSu
    ;
    
    --�J�[�\����`
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.Seq
              ,tbl.Number
              ,tbl.NumberRows
              ,tbl.StockNO
              ,tbl.ReserveKBN
          FROM @RTable AS tbl
         WHERE tbl.SelectReserveSu <> tbl.ReserveSu
        ;
    
    DECLARE @Seq int;
    DECLARE @Number varchar(11);
    DECLARE @NumberRows int;
    DECLARE @StockNO varchar(11);
    DECLARE @ReserveKBN tinyint;
    
    --�J�[�\���I�[�v��
    OPEN CUR_TABLE;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_TABLE
    INTO @Seq, @Number, @NumberRows, @StockNO, @ReserveKBN;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===       
        
        -- �����e�[�u���i�e�[�u���]���d�lB�j
        SELECT @CNT = COUNT(A.Number)
          FROM D_Reserve A
         WHERE A.Number = @Number
           AND A.NumberRows = @NumberRows
           AND A.StockNO = @StockNO
           AND A.ReserveKBN = @ReserveKBN
         ;

        IF @CNT > 0 
        BEGIN
            UPDATE D_Reserve SET
               ReserveSu = tbl.ReserveSu
              ,ShippingPossibleSU = CASE WHEN ISNULL(DS.ArrivalYetFLG,0) = 0 THEN tbl.ReserveSu ELSE 0 END
              ,UpdateOperator  =  @Operator  
              ,UpdateDateTime  =  @SYSDATETIME          
              FROM @RTable AS tbl
             INNER JOIN D_Reserve AS DR ON tbl.Number = DR.Number
                                       AND tbl.NumberRows = DR.NumberRows
                                       AND tbl.StockNO = DR.StockNO
                                       AND tbl.ReserveKBN = DR.ReserveKBN
             LEFT JOIN D_Stock DS ON tbl.StockNO = DS.StockNO
             WHERE tbl.Seq = @Seq
            ;
        END
        ELSE
        BEGIN
        
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                12,             --in�`�[��� 6
                @SYSDATE,       --in���
                @StoreCD,       --in�X��CD
                @Operator,
                @ReserveNO OUTPUT
                ;
            
            IF ISNULL(@ReserveNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
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
               ,[ShippingPlanDate]
               ,[ShippingPossibleSU]
               ,[ShippingOrderNO]
               ,[ShippingOrderRows]
               ,[PickingListDateTime]               
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
               ,tbl.ReserveKBN
               ,tbl.Number
               ,tbl.NumberRows
               ,tbl.StockNO
               ,tbl.SoukoCD
               ,tbl.JanCD
               ,tbl.SKUCD
               ,tbl.AdminNO
               ,tbl.ReserveSu
               ,CASE WHEN ISNULL(DS.ArrivalYetFLG,0) = 0 THEN @SYSDATE ELSE NULL END    --ShippingPossibleDate
               ,NULL
               ,CASE WHEN ISNULL(DS.ArrivalYetFLG,0) = 0 THEN tbl.ReserveSu ELSE 0 END   --ShippingPossibleSU
               ,NULL    --ShippingOrderNO
               ,0       --ShippingOrderRows
               ,NULL    --PickingListDateTime
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
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @RTable tbl
           LEFT JOIN D_Stock DS ON tbl.StockNO = DS.StockNO
          WHERE tbl.Seq = @SEQ
          ;        
        END
        
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @Seq, @Number, @NumberRows, @StockNO, @ReserveKBN;

    END     --LOOP�̏I���
    
    --�J�[�\�������
    CLOSE CUR_TABLE;
    DEALLOCATE CUR_TABLE;
    
    
    --�J�[�\����`
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.Number
              ,tbl.NumberRows
              ,MAX(DJM.HikiateSu) - SUM(tbl.SelectReserveSu) + SUM(tbl.ReserveSu) AS ReserveSu
              ,MAX(DJM.JuchuuSuu) AS JuchuuSuu
          FROM @RTable AS tbl
         INNER JOIN D_JuchuuDetails DJM ON tbl.Number = DJM.JuchuuNO
                                       AND tbl.NumberRows = DJM.JuchuuRows
         WHERE tbl.ReserveKBN = 1
           AND tbl.ReserveSu <> tbl.SelectReserveSu
         GROUP BY tbl.Number
                 ,tbl.NumberRows
        ;

    DECLARE @ReserveSu int;
    DECLARE @JuchuuSuu int;
    
    --�J�[�\���I�[�v��
    OPEN CUR_TABLE;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_TABLE
    INTO @Number, @NumberRows, @ReserveSu, @JuchuuSuu;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
        -- �󒍖��׃e�[�u���i�e�[�u���]���d�lC�j
        UPDATE D_JuchuuDetails SET
               HikiateSu = @ReserveSu
              ,HikiateFlg = (CASE WHEN @ReserveSu = 0 THEN 3 WHEN @ReserveSu >= @JuchuuSuu THEN 1 ELSE 2 END)
              ,UpdateOperator  =  @Operator  
              ,UpdateDateTime  =  @SYSDATETIME
        WHERE JuchuuNO = @Number
          AND JuchuuRows = @NumberRows
        ;
        
        -- �z���\�薾�׃e�[�u���i�e�[�u���]���d�lD�j
        UPDATE D_DeliveryPlanDetails SET
               HikiateFlg = (CASE WHEN @ReserveSu >= @JuchuuSuu THEN 1 ELSE 0 END)
              ,UpdateOperator  =  @Operator  
              ,UpdateDateTime  =  @SYSDATETIME
        WHERE Number = @Number
          AND NumberRows = @NumberRows
        ;
        
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @Number, @NumberRows, @ReserveSu, @JuchuuSuu;

    END     --LOOP�̏I���
    
    --�J�[�\�������
    CLOSE CUR_TABLE;
    DEALLOCATE CUR_TABLE;
    
    --���������f�[�^�֍X�V
    SET @KeyItem = @SKUCD;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'HikiateHenkouNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;
    
    SET @OutSKUCD = @SKUCD;

--<<OWARI>>
  return @W_ERR;

END

GO
