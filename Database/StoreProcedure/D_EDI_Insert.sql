BEGIN TRY 
 Drop Procedure D_EDI_Insert
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    EDI�񓚔[���o�^
--       Program ID      EDIKaitouNoukiTouroku
--       Create date:    2019.10.22
--    ======================================================================

CREATE TYPE T_Edi AS TABLE
    (
   [EDIImportRows]           [int]  --�捞�s�ԍ� [2]
  ,[OrderNO]                 [varchar] (11) --�����ԍ�
  ,[OrderRows]               [int]          --�������טA��
  ,[ArrivalPlanDate]         [date]   --���ח\���
  ,[ArrivalPlanMonth]        [int]    --���ח\�茎
  ,[ArrivalPlanCD]           [varchar] (4)   --���ח\���CD
  ,[ArrivalPlanSu]           [int]   --���ח\�萔
  ,[VendorComment]           [varchar] (30)  --�d����R�����g
  ,[ErrorKBN]                [tinyint]   --�G���[�敪
  ,[ErrorText]               [varchar] (60)  --�G���[���e
  ,[EDIOrderNO]              [varchar] (11)  --EDI�����ԍ�
  ,[EDIOrderRows]            [int]           --�s�ԍ�
  ,[CSVRecordKBN]            [varchar] (1)   --���R�[�h�敪
  ,[CSVDataKBN]              [varchar] (2)   --�f�[�^�敪
  ,[CSVCapitalCD]            [varchar] (13)  --�����҉�Е���CD
  ,[CSVCapitalName]          [varchar] (20)  --�����Ҋ�ƕ�����
  ,[CSVOrderCD]              [varchar] (13)  --�󒍎҉�Е���CD
  ,[CSVOrderName]            [varchar] (20)  --�󒍎Ҋ�ƕ�����
  ,[CSVSalesCD]              [varchar] (13)  --�̔��X��Е���CD
  ,[CSVSalesName]            [varchar] (20)  --�̔��X��Е�����
  ,[CSVDestinationCD]        [varchar] (13)  --�o�א��Е���CD
  ,[CSVDestinationName]      [varchar] (20)  --�o�א��Е�����
  ,[CSVOrderNO]              [varchar] (11)  --����NO
  ,[CSVOrderRows]            [varchar] (5)   --����NO�s
  ,[CSVOrderLines]           [varchar] (5)   --����NO��
  ,[CSVOrderDate]            [varchar] (15)  --������
  ,[CSVArriveDate]           [varchar] (15)  --�[�i��
  ,[CSVOrderKBN]             [varchar] (4)   --�����敪
  ,[CSVMakerItemKBN]         [varchar] (8)   --�����ҏ��i����
  ,[CSVMakerItem]            [varchar] (50)  --�����ҏ��iCD
  ,[CSVSKUCD]                [varchar] (30)  --�󒍎ҕi��
  ,[CSVSizeName]             [varchar] (20)  --���[�J�[�K�i1
  ,[CSVColorName]            [varchar] (20)  --���[�J�[�K�i2
  ,[CSVTaniCD]               [varchar] (3)   --�P��
  ,[CSVOrderUnitPrice]       [varchar] (15)  --����P��
  ,[CSVOrderPriceWithoutTax] [varchar] (15)  --�W�����
  ,[CSVBrandName]            [varchar] (20)  --�u�����h����
  ,[CSVSKUName]              [varchar] (15)  --���i����
  ,[CSVJanCD]                [varchar] (13)  --JANCD
  ,[CSVOrderSu]              [varchar] (10)  --������
  ,[CSVOrderGroupNO]         [varchar] (11)  --�\���P�i�����O���[�v�ԍ��j
  ,[CSVAnswerSu]             [varchar] (10)  --�\���Q�i�������j
  ,[CSVNextDate]             [varchar] (10)  --�\���R�i����\����j
  ,[CSVOrderGroupRows]       [varchar] (5)   --�\���S�i�����O���[�v�s�j
  ,[CSVErrorMessage]         [varchar] (10)  --�\���T�i�G���[���b�Z�[�W�j
    )
GO

CREATE PROCEDURE D_EDI_Insert
    (@VendorCD   varchar(13),
    @ImportFile   varchar(200),
    @OrderDetailsSu  int,
    @ImportDetailsSu  int,
    @ErrorSu  int,

    @Table  T_Edi READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    
    @OutEDIImportNO varchar(11) OUTPUT
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
    DECLARE @SYSDATE varchar(10);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(varchar,@SYSDATETIME,111);
    
    INSERT INTO [D_EDI]
           ([ImportDateTime]
           ,[StaffCD]
           ,[VendorCD]
           ,[ImportFile]
           ,[OrderDetailsSu]
           ,[ImportDetailsSu]
           ,[ErrorSu]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@SYSDATETIME    --ImportDateTime, datetime,>
           ,NULL    --StaffCD, varchar(10),>
           ,@VendorCD
           ,@ImportFile
           ,@OrderDetailsSu
           ,@ImportDetailsSu
           ,@ErrorSu
           ,NULL    --InsertOperator, varchar(10),>
           ,@SYSDATETIME    --InsertDateTime, datetime,>
           ,NULL    --UpdateOperator, varchar(10),>
           ,@SYSDATETIME    --UpdateDateTime, datetime,>
           ,NULL    --DeleteOperator, varchar(10),>
           ,NULL    --DeleteDateTime, datetime,>
           );

    SELECT @OutEDIImportNO = MAX(EDIImportNO)
    FROM D_EDI
    ;
    
    INSERT INTO [D_EDIDetail]
           ([EDIImportNO]
           ,[EDIImportRows]
           ,[OrderNO]
           ,[OrderRows]
           ,[ArrivalPlanDate]
           ,[ArrivalPlanMonth]
           ,[ArrivalPlanCD]
           ,[ArrivalPlanSu]
           ,[VendorComment]
           ,[ErrorKBN]
           ,[ErrorText]
           ,[EDIOrderNO]
           ,[EDIOrderRows]
           ,[CSVRecordKBN]
           ,[CSVDataKBN]
           ,[CSVCapitalCD]
           ,[CSVCapitalName]
           ,[CSVOrderCD]
           ,[CSVOrderName]
           ,[CSVSalesCD]
           ,[CSVSalesName]
           ,[CSVDestinationCD]
           ,[CSVDestinationName]
           ,[CSVOrderNO]
           ,[CSVOrderRows]
           ,[CSVOrderLines]
           ,[CSVOrderDate]
           ,[CSVArriveDate]
           ,[CSVOrderKBN]
           ,[CSVMakerItemKBN]
           ,[CSVMakerItem]
           ,[CSVSKUCD]
           ,[CSVSizeName]
           ,[CSVColorName]
           ,[CSVTaniCD]
           ,[CSVOrderUnitPrice]
           ,[CSVOrderPriceWithoutTax]
           ,[CSVBrandName]
           ,[CSVSKUName]
           ,[CSVJanCD]
           ,[CSVOrderSu]
           ,[CSVOrderGroupNO]
           ,[CSVAnswerSu]
           ,[CSVNextDate]
           ,[CSVOrderGroupRows]
           ,[CSVErrorMessage]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            @OutEDIImportNO     --<EDIImportNO, int,>
           ,tbl.EDIImportRows
           ,tbl.OrderNO
           ,tbl.OrderRows
           ,tbl.ArrivalPlanDate
           ,tbl.ArrivalPlanMonth
           ,tbl.ArrivalPlanCD
           ,tbl.ArrivalPlanSu
           ,tbl.VendorComment
           ,tbl.ErrorKBN
           ,tbl.ErrorText
           ,tbl.EDIOrderNO
           ,tbl.EDIOrderRows
           ,tbl.CSVRecordKBN
           ,tbl.CSVDataKBN
           ,tbl.CSVCapitalCD
           ,tbl.CSVCapitalName
           ,tbl.CSVOrderCD
           ,tbl.CSVOrderName
           ,tbl.CSVSalesCD
           ,tbl.CSVSalesName
           ,tbl.CSVDestinationCD
           ,tbl.CSVDestinationName
           ,tbl.CSVOrderNO
           ,tbl.CSVOrderRows
           ,tbl.CSVOrderLines
           ,tbl.CSVOrderDate
           ,tbl.CSVArriveDate
           ,tbl.CSVOrderKBN
           ,tbl.CSVMakerItemKBN
           ,tbl.CSVMakerItem
           ,tbl.CSVSKUCD
           ,tbl.CSVSizeName
           ,tbl.CSVColorName
           ,tbl.CSVTaniCD
           ,tbl.CSVOrderUnitPrice
           ,tbl.CSVOrderPriceWithoutTax
           ,tbl.CSVBrandName
           ,tbl.CSVSKUName
           ,tbl.CSVJanCD
           ,tbl.CSVOrderSu
           ,tbl.CSVOrderGroupNO
           ,tbl.CSVAnswerSu
           ,tbl.CSVNextDate
           ,tbl.CSVOrderGroupRows
           ,tbl.CSVErrorMessage
           ,NULL    --InsertOperator, varchar(10),>
           ,@SYSDATETIME    --InsertDateTime, datetime,>
           ,NULL    --UpdateOperator, varchar(10),>
           ,@SYSDATETIME    --UpdateDateTime, datetime,>
           ,NULL    --DeleteOperator, varchar(10),>
           ,NULL    --DeleteDateTime, datetime,>
      FROM @Table tbl
      ;

    --�G���[�ԍ����O�̏ꍇ�i�G���[�łȂ��ꍇ�j���ACSV.�[�i����Null�̏ꍇ
    --�e�[�u���]���d�l�b(�����t�@�C���G���[����̂��߃w�b�_�����X�V)
    UPDATE D_Order SET
        --,UpdateOperator = 
        UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_Order.OrderNO
    ;
    
    UPDATE D_OrderDetails SET
        ArrivePlanDate = tbl.CSVArriveDate
        --,UpdateOperator = 
        ,UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_OrderDetails.OrderNO
    AND tbl.OrderRows = D_OrderDetails.OrderRows
    ;
    
    --�e�[�u���]���d�l�c
    UPDATE D_ArrivalPlan SET
        LastestFLG = 9
        --,UpdateOperator = 
        ,UpdateDateTime = @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.ErrorKBN = 0
    AND ISNULL(tbl.CSVArriveDate,'') <> ''
    AND tbl.OrderNO = D_ArrivalPlan.Number
    AND tbl.OrderRows = D_ArrivalPlan.NumberRows
    ;
    
    --�e�[�u���]���d�l�d
    DECLARE @EDIImportRows int;
    DECLARE @StoreCD varchar(4);
    DECLARE @CSVOrderDate varchar(15);
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO varchar(11);
    DECLARE @ReserveNO varchar(11);
    DECLARE @JuchuuNO varchar(11);
    
   --�J�[�\����`
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.EDIImportRows, DH.StoreCD
        ,CONVERT(varchar,CONVERT(date,tbl.CSVOrderDate),111) AS CSVOrderDate
        ,(SELECT DM.JuchuuNO FROM D_OrderDetails AS DM 
            WHERE tbl.OrderNO = DM.OrderNO
            AND tbl.OrderRows = DM.OrderRows) AS JuchuuNO
        FROM @Table AS tbl
        INNER JOIN D_Order AS DH
        ON tbl.OrderNO = DH.OrderNO
        WHERE tbl.ErrorKBN = 0
        AND ISNULL(tbl.CSVArriveDate,'') <> ''
        ORDER BY tbl.EDIImportRows;
    
    --�J�[�\���I�[�v��
    OPEN CUR_AAA;
    
    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_AAA
    INTO  @EDIImportRows,@StoreCD,@CSVOrderDate,@JuchuuNO;
     
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            22,         --in�`�[��� 22
            @CSVOrderDate, --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @ArrivalPlanNO OUTPUT
            ;
            
        IF ISNULL(@ArrivalPlanNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
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
               ,1   --ArrivalPlanKBN
               ,tbl.OrderNo     --Number
               ,tbl.OrderRows   --NumberRows
               ,1   --NumberSEQ
               ,tbl.CSVArriveDate
               ,0   --ArrivalPlanMonth
               ,NULL    --ArrivalPlanCD
               ,(CASE WHEN ISNULL(tbl.CSVArriveDate,'') = '' THEN NULL
                ELSE tbl.CSVArriveDate END)
               ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
               ,NULL   --StaffCD
               ,1   --LastestFLG
               ,@OutEDIImportNO
               ,DH.DestinationSoukoCD --SoukoCD
               ,(SELECT top 1 M.SKUCD
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,(SELECT top 1 M.AdminNO
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,tbl.CSVJanCD
               ,tbl.CSVAnswerSu
               ,0   --ArrivalSu
               ,NULL    --OriginalArrivalPlanNO
               ,@VendorCD
               ,NULL    --FromSoukoCD
               ,NULL    --ToStoreCD
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           INNER JOIN D_Order AS DH
            ON tbl.OrderNO = DH.OrderNO
           WHERE tbl.EDIImportRows = @EDIImportRows
           ;
           
        --�e�[�u���]���d�l�e
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            21,         --in�`�[��� 21:�݌�
            @SYSDATE, --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @StockNO OUTPUT
            ;

        IF ISNULL(@StockNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
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
               @StockNO
               ,DH.DestinationSoukoCD
               ,NULL    --RackNO
               ,@ArrivalPlanNO
               ,(SELECT top 1 M.SKUCD
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,(SELECT top 1 M.AdminNO
                    FROM M_SKU AS M
                    WHERE M.SetKBN = 0
                    AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                    AND M.DeleteFlg = 0
                    AND M.JANCD = tbl.CSVJanCD
                ORDER BY M.ChangeDate desc)
               ,tbl.CSVJanCD
               ,1   --ArrivalYetFLG
               ,1   --ArrivalPlanKBN
               ,tbl.CSVArriveDate
               ,NULL    --ArrivalDate
               ,0   --StockSu
               ,tbl.CSVAnswerSu
               ,0   --AllowableSu
               ,0   --AnotherStoreAllowableSu
               ,tbl.CSVAnswerSu --ReserveSu
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
               ,NULL    --DeleteOperator
               ,NULL    --DeleteDateTime
           FROM  @Table tbl
           INNER JOIN D_Order AS DH
            ON tbl.OrderNO = DH.OrderNO
           WHERE tbl.EDIImportRows = @EDIImportRows
           ;
        
        --�ȉ��̏ꍇ�ɒǉ��E�X�V�i�󔭒��ł��A�\�����������x�͂����肵�Ă���ꍇ�ɒǉ��E�X�V�j
        --D_OrderDetails.JuchuuNO��Null ��
        --����
        --�b�r�u�[�i����Null
        IF ISNULL(@JuchuuNO,'') <> ''
        BEGIN
               --�e�[�u���]���d�l�f
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                12,         --in�`�[��� 12:����
                @SYSDATE    , --in���
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
                   ,1   --ReserveKBN
                   ,DM.JuchuuNO
                   ,DM.JuchuuRows
                   ,@StockNO
                   ,DH.DestinationSoukoCD
                   ,tbl.CSVJanCD
                   ,(SELECT top 1 M.SKUCD
                        FROM M_SKU AS M
                        WHERE M.SetKBN = 0
                        AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                        AND M.DeleteFlg = 0
                        AND M.JANCD = tbl.CSVJanCD
                    ORDER BY M.ChangeDate desc)
                   ,(SELECT top 1 M.AdminNO
                        FROM M_SKU AS M
                        WHERE M.SetKBN = 0
                        AND M.ChangeDate <= CONVERT(date,@SYSDATETIME)
                        AND M.DeleteFlg = 0
                        AND M.JANCD = tbl.CSVJanCD
                    ORDER BY M.ChangeDate desc)
                   ,tbl.CSVAnswerSu   --ReserveSu
                   ,NULL    --ShippingPossibleDate
                   ,0   --ShippingPossibleSU
                   ,NULL    --ShippingOrderNO
                   ,0    --ShippingOrderRows
                   ,0   --CompletedPickingNO
                   ,0   --CompletedPickingRow
                   ,NULL    --CompletedPickingDate
                   ,0   --ShippingSu
                   ,0   --ReturnKBN
                   ,NULL    --OriginalReserveNO
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL    --DeleteOperator
                   ,NULL    --DeleteDateTime
               FROM  @Table tbl
               INNER JOIN D_Order AS DH
                ON tbl.OrderNO = DH.OrderNO
               INNER JOIN D_OrderDetails AS DM
                ON tbl.OrderNO = DM.OrderNO
                AND tbl.OrderRows = DM.OrderRows
               WHERE tbl.EDIImportRows = @EDIImportRows
               ;
        END           
           
        -- ========= ���[�v���̎��ۂ̏��� �����܂�===

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO  @EDIImportRows,@StoreCD,@CSVOrderDate,@JuchuuNO;

    END
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'EDIKaitouNoukiTouroku',
        @PC,
        NULL,
        NULL;
    
--<<OWARI>>
  return @W_ERR;

END

GO
