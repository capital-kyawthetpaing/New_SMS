IF OBJECT_ID ( 'PRC_EDIOrder_Insert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_EDIOrder_Insert]
GO

--  ======================================================================
--       Program Call    EDI��������
--       Program ID      D_EDIOrder_Insert
--       Create date:    2019.11.16
--    ======================================================================

--********************************************--
--                                            --
--              ���o�͕�����                  --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_EDIOrder_Insert
    (@StoreCD  varchar(4),
     @OrderDateFrom  varchar(10),
     @OrderDateTo  varchar(10),    
     @StaffCD  varchar(10),
     @OrderCD  varchar(13),
     @OrderNO varchar(11),
     @ChkMisyonin varchar(1),
     @Operator  varchar(10),
     @PC        varchar(30),     
     @SYSDATETIME  datetime
)AS

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATE date;    
    DECLARE @KeyItem varchar(100);

    SET @W_ERR = 0;
    SET @SYSDATE = CONVERT(date, @SYSDATETIME); 
    SET @KeyItem = '';
    
    -- �f�[�^�擾
    DECLARE @OrderTbl table
    (
        [StoreCD] [varchar](4) ,
        [VendorCD] [varchar](13) ,
        [SoukoCD] [varchar](6) ,
        [OrderNO] [varchar](11) ,
        [OrderRows] [int],
        [MakerItem] [varchar](50) ,
        [SizeName] [varchar](20) ,
        [ColorName] [varchar](20) ,
        [TaniCD] [varchar](2) ,
        [BrandKana] [varchar](20) ,
        [AdminNO] [int] ,
        [JanCD] [varchar](13) ,
        [OrderSu] [int] 
    )
    
    INSERT INTO @OrderTbl EXEC D_Order_SelectAllForEDIHacchuu 
                                      @StoreCD
                                     ,@OrderDateFrom
                                     ,@OrderDateTo
                                     ,@StaffCD
                                     ,@OrderCD
                                     ,@OrderNO
                                     ,@ChkMisyonin
    
    DECLARE @VendorCD varchar(10);
    DECLARE @SoukoCD varchar(6);
    DECLARE @EDIOrderNO varchar(11);
    DECLARE @VCapitalCD varchar(13);
    DECLARE @VCapitalName varchar(20);
    DECLARE @VOrderCD varchar(13);
    DECLARE @VOrderName varchar(20);
    DECLARE @VSalesCD varchar(13);
    DECLARE @VSalesName varchar(20);
    DECLARE @VDestinationCD varchar(13);
    DECLARE @VDestinationName varchar(20);
    
    --�J�[�\����`�@
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.VendorCD
        FROM @OrderTbl AS tbl
        GROUP BY tbl.VendorCD
        ORDER BY tbl.VendorCD;
    
    --�J�[�\���I�[�v���@
    OPEN CUR_AAA;
    
    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_AAA
    INTO  @VendorCD;
     
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
            
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            23,             --in�`�[��� 23
            @SYSDATE,       --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @EDIOrderNO OUTPUT
            ;
        
        IF ISNULL(@EDIOrderNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --M_VendorStoreFTP�擾
        SELECT TOP 1 
               @VCapitalCD = SkenCapitalCD 
              ,@VCapitalName = SkenCapitalName
              ,@VOrderCD = SkenOrderCD
              ,@VOrderName = SkenOrderName
              ,@VSalesCD = SkenSalesCD
              ,@VSalesName = SkenSalesName
              --,@VDestinationCD = SkenDestinationCD
              --,@VDestinationName = SkenDestinationName
          FROM M_VendorStoreFTP 
         WHERE VendorCD = @VendorCD
           AND StoreCD = @StoreCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
         
        --M_VendorSoukoFTP�擾
        /*
        SET @VDestinationCD = '';
        SET @VDestinationName = '';
        
        SELECT TOP 1
               @VDestinationCD = SkenDestinationCD
              ,@VDestinationName = SkenDestinationName
          FROM M_VendorSoukoFTP 
         WHERE VendorCD = @VendorCD
           AND StoreCD = @StoreCD
           AND SoukoCD = @SoukoCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
        */
        
        --�yD_EDIOrder�zTable�]���d�l�`
        INSERT INTO [D_EDIOrder]
            ([EDIOrderNO]
            ,[StoreCD]
            ,[OrderDate]
            ,[VendorCD]
            ,[RecordKBN]
            ,[DataKBN]
            ,[CapitalCD]
            ,[CapitalName]
            ,[OrderCD]
            ,[OrderName]
            ,[SalesCD]
            ,[SalesName]
            ,[DestinationCD]
            ,[DestinationName]
            ,[OutputDatetime]
            ,[InsertOperator]
            ,[InsertDateTime]
            ,[UpdateOperator]
            ,[UpdateDateTime]
            )
        VALUES
           (@EDIOrderNO     
           ,@StoreCD                          
           ,@SYSDATE
           ,LEFT(@VendorCD,10)
           ,'B'
           ,'01'
           ,@VCapitalCD
           ,@VCapitalName
           ,@VOrderCD
           ,@VOrderName
           ,@VSalesCD
           ,@VSalesName
           ,NULL
           ,NULL
           ,NULL   
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           );               

        
        --�yD_EDIOrderDetails�zTable�]���d�l�a
        INSERT INTO [dbo].[D_EDIOrderDetails]
           ([EDIOrderNO]
           ,[EDIOrderRows]
           ,[OrderNO]
           ,[OrderRows]
           ,[OrderLines]
           ,[OrderDate]
           ,[ArriveDate]
           ,[OrderKBN]
           ,[MakerItemKBN]
           ,[MakerItem]
           ,[SKUCD]
           ,[SizeName]
           ,[ColorName]
           ,[TaniCD]
           ,[OrderUnitPrice]
           ,[OrderPriceWithoutTax]
           ,[BrandName]
           ,[SKUName]
           ,[AdminNO]
           ,[JanCD]
           ,[OrderSu]
           ,[OrderGroupNO]
           ,[AnswerSu]
           ,[NextDate]
           ,[OrderGroupRows]
           ,[ErrorMessage]
           ,[DestinationCD]
           ,[DestinationName]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT
            @EDIOrderNO
           ,Row
           ,OrderNO
           ,OrderRows
           ,1
           ,@SYSDATE
           ,NULL
           ,'2'
           ,NULL
           ,OrderNO
           ,MakerItem
           ,LEFT(SizeName,5)
           ,LEFT(ColorName,10)
           ,TaniCD
           ,0
           ,0
           ,LEFT(BrandKana,10)
           ,LEFT(OrderNO + SPACE(11),11) + '-' + RIGHT('000' + CAST(OrderRows AS VARCHAr(3)),3)
           ,AdminNO
           ,JanCD
           ,OrderSu
           ,@EDIOrderNO
           ,0
           ,NULL
           ,Row
           ,NULL
           ,(SELECT TOP 1 SkenDestinationCD
               FROM M_VendorSoukoFTP 
              WHERE VendorCD = tbl.VendorCD
                AND StoreCD = tbl.StoreCD
                AND SoukoCD = tbl.SoukoCD
                AND ChangeDate <= @SYSDATE
              ORDER BY ChangeDate DESC
            )
           ,(SELECT TOP 1 SkenDestinationName
               FROM M_VendorSoukoFTP 
              WHERE VendorCD = tbl.VendorCD
                AND StoreCD = tbl.StoreCD
                AND SoukoCD = tbl.SoukoCD
                AND ChangeDate <= @SYSDATE
              ORDER BY ChangeDate DESC
            )
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
        FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY OrderNO) AS Row
                FROM @OrderTbl
              WHERE StoreCD = @StoreCD
                AND VendorCD = @VendorCD
             ) AS tbl
        ORDER BY tbl.OrderNO;
        
        
        --�yD_OrderDetails�z�e�[�u���]���d�lC
        UPDATE D_OrderDetails
           SET EDIOutputDatetime = @SYSDATETIME
          FROM D_OrderDetails DM
         INNER JOIN @OrderTbl tbl ON tbl.OrderNO = DM.OrderNO
                                 AND tbl.OrderRows = DM.OrderRows
         WHERE tbl.StoreCD = @StoreCD
           AND tbl.VendorCD = @VendorCD;
        
        --M_Vendor�擾
        DECLARE @EDIFlg tinyint;
        
        SELECT TOP 1 
               @EDIFlg = EDIFlg 
          FROM M_Vendor 
         WHERE VendorCD = @VendorCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
           
       --���[���֌W�e�[�u���ǉ�
        IF @EDIFlg = 2 
        BEGIN
            EXEC D_Mail_Insert
                @EDIOrderNo,
                @SYSDATETIME,
                @Operator;
        END
       
        --���������e�[�u���ɂ͍ŏ���EDI�����ԍ����Z�b�g
        IF @KeyItem = ''
            SET @KeyItem = @EDIOrderNO;
    -- ========= ���[�v���̎��ۂ̏��� �����܂�===

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO  @VendorCD;

    END
    
    --���������f�[�^�֍X�V
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'EDIHacchuu',
        @PC,
        NULL,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END

