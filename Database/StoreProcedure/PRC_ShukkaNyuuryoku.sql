
/****** Object:  StoredProcedure [dbo].[PRC_ShukkaNyuuryoku]    Script Date: 2020/10/01 19:38:03 ******/
DROP PROCEDURE [dbo].[PRC_ShukkaNyuuryoku]
GO

DROP TYPE [dbo].[T_ShukkaF]

/****** Object:  StoredProcedure [dbo].[PRC_ShukkaNyuuryoku]    Script Date: 2020/10/01 19:38:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE TYPE T_ShukkaF AS TABLE
    (
    [ShippingRows] [int],    
    [InstructionRows] [int],
    [Number] [varchar](11) ,
    [NumberRows] [int],
    [ReserveNO] [varchar](11) ,
    [ReserveKBN] [tinyint] ,
    [SKUCD] [varchar](30),
    [AdminNO] [int],
    [JanCD] [varchar](13),
    [SKUName] [varchar](80),
    [ColorName] [varchar](20),
    [SizeName] [varchar](20),
    [ShippingSu] [int] ,
    [StockNO] [varchar](11) ,
    [ToStoreCD] [varchar](4),
    [ToSoukoCD] [varchar](6),
    [ToRackNO] [varchar](11),
    [ToStockNO] [varchar](11),
    [FromStoreCD] [varchar](4),
    [FromSoukoCD] [varchar](6),
    [FromRackNO] [varchar](11),
    [CustomerCD] [varchar](13)
    )
GO

--********************************************--
--                                            --
--        �X�V                                --
--                                            --
--********************************************--
CREATE PROCEDURE [dbo].[PRC_ShukkaNyuuryoku]
    (@OperateMode    int,                 -- �����敪�i1:�V�K 3:�폜�j
     @ShippingNO   varchar(11),
     @ShippingDate  varchar(10),     
     @ShippingKBN   tinyint,    
     @StoreCD   varchar(4),
     @InstructionNO   varchar(11),
     @SoukoCD   varchar(6),
     @CarrierCD   varchar(3),
     @StaffCD   varchar(10),
     @UnitsCount   tinyint,
     @BoxSize   varchar(6),
     @DecidedDeliveryDate   varchar(10),
     @DecidedDeliveryTime   varchar(4),
    
     @Table  T_ShukkaF READONLY,
     @Operator  varchar(10),
     @PC  varchar(30),
     @OutShippingNO varchar(11) OUTPUT
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @SYSDATE = CONVERT(date, @SYSDATETIME);
    
    --�V�K--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '�V�K';
        
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            6,             --in�`�[��� 6
            @ShippingDate, --in���
            @StoreCD,      --in�X��CD
            @Operator,
            @ShippingNO OUTPUT
            ;
        
        IF ISNULL(@ShippingNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --�yD_Shipping�zTable�]���d�l�`
        INSERT INTO [dbo].[D_Shipping]
           ([ShippingNO]
           ,[SoukoCD]
           ,[ShippingKBN]
           ,[InstructionNO]
           ,[CarrierCD]
           ,[DecidedDeliveryDate]
           ,[DecidedDeliveryTime]
           ,[ShippingDate]
           ,[InputDateTime]
           ,[StaffCD]
           ,[UnitsCount]
           ,[BoxSize]
           ,[PrintDate]
           ,[PrintStaffCD]
           ,[LinkageDateTime]
           ,[LinkageStaffCD]
           ,[InvoiceNO]
           ,[InvNOLinkDateTime]
           ,[ReceiveStaffCD]
           ,[SalesDateTime]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime]
           )
     VALUES
           (@ShippingNO
           ,@SoukoCD
           ,@ShippingKBN
           ,@InstructionNO
           ,@CarrierCD
           ,@DecidedDeliveryDate
           ,@DecidedDeliveryTime
           ,@ShippingDate
           ,@SYSDATETIME           
           ,@StaffCD           
           ,@UnitsCount           
           ,@BoxSize
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );
                    

    END
    ELSE IF @OperateMode = 3 --�폜--
    BEGIN
        SET @OperateModeNm = '�폜';
        
        UPDATE [D_Shipping]
           SET [DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [ShippingNO] = @ShippingNO
         ;
         
        UPDATE [D_ShippingDetails]
           SET [DeleteOperator]     =  @Operator  
              ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [ShippingNO] = @ShippingNO
         ;
    END
    
    --�yD_ShippingDetails�zTable�]���d�l�a
    IF @OperateMode = 1    --�V�K��
    BEGIN
        INSERT INTO [D_ShippingDetails]
           ([ShippingNO]
           ,[ShippingRows]
           ,[ShippingKBN]
           ,[Number]
           ,[NumberRows]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[SKUName]
           ,[ColorName]
           ,[SizeName]
           ,[ShippingSu]
           ,[InstructionNO]
           ,[InstructionRows]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime]
           )
        SELECT
            @ShippingNO
           ,tbl.ShippingRows
           ,@ShippingKBN
           ,tbl.Number
           ,tbl.NumberRows
           ,tbl.SKUCD
           ,tbl.AdminNO
           ,tbl.JanCD
           ,tbl.SKUName
           ,tbl.ColorName
           ,tbl.SizeName
           ,tbl.ShippingSu
           ,@InstructionNO
           ,tbl.InstructionRows
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL        
        FROM @Table tbl
        ORDER BY tbl.ShippingRows
        ;
    END
    
    --�yD_Reserve�zUpdate   Table�]���d�l�b
    UPDATE [D_Reserve] SET
       [ShippingPossibleSu] = DR.ShippingPossibleSu - (CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 ELSE tbl.ShippingSu END)
      ,[ShippingSu] = DR.ShippingSu + (CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 ELSE tbl.ShippingSu END)
      ,[UpdateOperator]     =  @Operator  
      ,[UpdateDateTime]     =  @SYSDATETIME
    
     FROM D_Reserve AS DR
     INNER JOIN @Table AS tbl
     ON tbl.ReserveNO = DR.ReserveNO
    ;
    
    
    --�J�[�\����`
    DECLARE CUR_TABLE CURSOR FOR
        SELECT tbl.ShippingRows
             ,(SELECT top 1 (CASE WHEN A.SetKbn = 1 THEN A.SetSU ELSE 1 END)
               FROM M_SKU A 
              WHERE A.AdminNO = tbl.AdminNO 
                AND A.ChangeDate <= @SYSDATE 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SetSu
        FROM @Table AS tbl
        ;
        
    DECLARE @ShippingRows int;
    DECLARE @SetSu int;
    
    --�J�[�\���I�[�v��
    OPEN CUR_TABLE;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_TABLE
    INTO @ShippingRows, @SetSu;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===       
        
       --�yD_Stock�zUpdate   Table�]���d�l�c
        UPDATE [D_Stock] SET
           [StockSu] = DS.StockSu - (CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 * @Setsu ELSE tbl.ShippingSu * @Setsu END)
          ,[InstructionSu] = DS.InstructionSu - (CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 * @Setsu ELSE tbl.ShippingSu * @Setsu END)
          ,[ShippingSu] = DS.ShippingSu + (CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 * @Setsu ELSE tbl.ShippingSu * @Setsu END)
          ,[UpdateOperator]     =  @Operator  
          ,[UpdateDateTime]     =  @SYSDATETIME
        
         FROM D_Stock AS DS
         INNER JOIN @Table AS tbl
         ON tbl.StockNO = DS.StockNO
         AND tbl.ShippingRows = @ShippingRows
        ;
		
		--�yD_Warehousing�zInsert   Table�]���d�l�d
	    INSERT INTO [dbo].[D_Warehousing]
	       ([WarehousingDate]
	       ,[SoukoCD]
	       ,[RackNO]
	       ,[StockNO]
	       ,[SKUCD]
	       ,[AdminNO]
	       ,[JanCD]
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
	    SELECT
	        @ShippingDate
	       ,@SoukoCD
	       ,NULL
	       ,tbl.StockNO
	       ,tbl.SKUCD
	       ,tbl.AdminNO
	       ,tbl.JanCD
	       ,(CASE tbl.ReserveKBN WHEN 1 THEN 3 WHEN 2 THEN 43 ELSE 0 END)
	       ,(CASE WHEN @OperateMode = 3 THEN 1 ELSE 0 END)
	       ,tbl.Number
	       ,tbl.NumberRows
	       ,NULL
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.ToStoreCD ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.ToSoukoCD ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.ToRackNO ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.ToStockNO ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.FromStoreCD ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.FromSoukoCD ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 2 THEN tbl.FromRackNO ELSE NULL END) 
	       ,(CASE tbl.ReserveKBN WHEN 1 THEN tbl.CustomerCD ELSE NULL END) 
	       ,(CASE WHEN @OperateMode = 3 THEN tbl.ShippingSu * -1 * @SetSU ELSE tbl.ShippingSu * @Setsu END)
	       ,0
	       ,0
	       ,'ShukkaNyuuryoku'
	       ,@Operator
	       ,@SYSDATETIME
	       ,@Operator
	       ,@SYSDATETIME
	       ,NULL
	       ,NULL
	    FROM @Table tbl
	   WHERE tbl.AdminNO <> 0
	     AND tbl.ShippingRows = @ShippingRows
		
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @ShippingRows, @SetSU;

    END     --LOOP�̏I���
    
    --�J�[�\�������
    CLOSE CUR_TABLE;
    DEALLOCATE CUR_TABLE;
    
    
    --���������f�[�^�֍X�V
    SET @KeyItem = @ShippingNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'ShukkaNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutShippingNO = @ShippingNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO


