IF OBJECT_ID ( 'CheckInstruction', 'P' ) IS NOT NULL
    Drop Procedure dbo.[CheckInstruction]
GO
IF OBJECT_ID ( 'D_Instruction_SelectDataForShukka', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Instruction_SelectDataForShukka]
GO
IF OBJECT_ID ( 'CheckShipping', 'P' ) IS NOT NULL
    Drop Procedure dbo.[CheckShipping]
GO
IF OBJECT_ID ( 'D_Shipping_SelectData', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Shipping_SelectData]
GO
IF OBJECT_ID ( 'CheckShukkaData', 'P' ) IS NOT NULL
    Drop Procedure dbo.[CheckShukkaData]
GO
IF OBJECT_ID ( 'PRC_ShukkaNyuuryoku', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_ShukkaNyuuryoku]
GO
IF EXISTS (select * from sys.table_types where name = 'T_Shukka')
    Drop TYPE dbo.[T_Shukka]
GO

--  ======================================================================
--       Program Call    �o�ד���
--       Program ID      ShukkaNyuuryoku
--       Create date:    2019.12.09
--    ======================================================================

--********************************************--
--                                            --
--             �o�׎w���`�F�b�N               --
--                                            --
--********************************************--
CREATE PROCEDURE CheckInstruction
    (@InstructionNo varchar(11)
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT DH.InstructionNO
          ,DH.DeleteDateTime
      FROM D_Instruction DH
     WHERE DH.InstructionNO = @InstructionNo
     ;

END

GO

--********************************************--
--                                            --
--            �o�׎w���f�[�^���o              --
--                                            --
--********************************************--
CREATE PROCEDURE D_Instruction_SelectDataForShukka
    (@InstructionNo varchar(11),
     @ShippingDate  varchar(10)    
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT (CASE WHEN DJM.UpdateCancelKBN = 1 THEN 1 ELSE 0 END) AS UpdateKBN
           ,DI.OntheDayFLG
           ,DI.DeliveryName
           ,(CASE WHEN DI.DecidedDeliveryDate IS NOT NULL AND DI.DecidedDeliveryTime IS NOT NULL THEN 1 ELSE 0 END) AS DecidedDeliveryKbn
           ,(SELECT top 1 A.CarrierCD
               FROM M_Carrier A 
              WHERE A.CarrierCD = DI.CarrierCD 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CarrierCD
           ,(CASE WHEN DI.CashOnDelivery = 1 THEN 1 ELSE 0 END) AS CashOnDelivery
           ,1 AS UnitsCount
           ,(CASE WHEN DJS.NouhinsyoFLG = 1 THEN 1 ELSE 0 END) AS NouhinsyoFLG
           ,DI.CommentInStore
           ,(SELECT top 1 B.Char1
               FROM M_SKU A 
               LEFT JOIN M_MultiPorpose B ON A.TaniCD = B.[Key]
                                         AND 201 = B.ID
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS TANI
           ,DIM.JanCD
           ,(SELECT top 1 A.SKUName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
           ,(SELECT top 1 A.ColorName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
           ,(SELECT top 1 A.SizeName
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SizeName
           ,(SELECT top 1 A.CommentOutStore
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= @ShippingDate
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CommentOutStore
           ,DIM.InstructionSu
           ,0 AS ShippingSu
           ,DJM.JuchuuNO
           ,DJM.UpdateCancelKBN
           ,CASE WHEN DJ.CancelDate IS NULL THEN '0' ELSE '1' END AS CancelKbn
           ,DIM.SKUCD
           ,DIM.AdminNO
           ,DI.FromSoukoCD AS SoukoCD
           ,DI.InstructionKBN
           ,DIM.InstructionNO
           ,DIM.InstructionRows
           ,DRM.Number
           ,DRM.NumberRows
           ,DRM.ReserveNO
           ,DRM.ReserveKBN
           ,DRM.StockNO
           ,DM.ToStoreCD
           ,DM.ToSoukoCD
           ,NULL AS ToRackNO
           ,NULL AS ToStockNO
           ,DM.StoreCD AS FromStoreCD
           ,DM.FromSoukoCD AS FromSoukoCD
           ,DMM.FromRackNO
           ,DJ.CustomerCD
    FROM D_Instruction AS DI
    INNER JOIN D_InstructionDetails AS DIM ON DI.InstructionNO = DIM.InstructionNO
                                          AND DIM.InstructionKBN IN (1,2)
                                          AND DIM.DeleteDateTime IS NULL   
    LEFT OUTER JOIN D_Reserve AS DRM ON DIM.InstructionNO = DRM.ShippingOrderNO
                                    AND DIM.InstructionRows = DRM.ShippingOrderRows
                                    AND DRM.DeleteDateTime IS NULL                                                     
    LEFT OUTER JOIN D_JuchuuDetails AS DJM ON DRM.Number = DJM.JuchuuNO
                                          AND DRM.NumberRows = DJM.JuchuuRows
                                          AND DRM.ReserveKBN = 1
                                          AND DJM.DeleteDateTime IS NULL                                                          
    LEFT OUTER JOIN D_Juchuu AS DJ ON DJM.JuchuuNO = DJ.JuchuuNO
                                  AND DJ.DeleteDateTime IS NULL  
    LEFT OUTER JOIN D_JuchuuStatus AS DJS ON DJ.JuchuuNO = DJS.JuchuuNO   
    LEFT OUTER JOIN D_MoveDetails AS DMM ON DRM.[Number] = DMM.MoveNO
                                        AND DRM.NumberRows = DMM.MoveRows
                                        AND DRM.ReserveKBN = 2
                                        AND DMM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Move AS DM ON DMM.MoveNO = DM.MoveNO
                                AND DM.DeleteDateTime IS NULL     
    WHERE DI.DeleteDateTime IS NULL
      AND DI.InstructionNO = @InstructionNo
    ORDER BY DIM.InstructionRows
    ;

END

GO

--********************************************--
--                                            --
--             �o�׃`�F�b�N               --
--                                            --
--********************************************--
CREATE PROCEDURE CheckShipping
    (@ShippingNO varchar(11)
    )AS
BEGIN

    SET NOCOUNT ON;

    SELECT DS.ShippingNO
          ,DS.ShippingDate
          ,DS.DeleteDateTime
      FROM D_Shipping DS
     WHERE DS.ShippingNO = @ShippingNO
     ;

END

GO

--********************************************--
--                                            --
--             �o�׃f�[�^���o                 --
--                                            --
--********************************************--
CREATE PROCEDURE D_Shipping_SelectData
    (@ShippingNO varchar(11)
    )AS
    
BEGIN
    
    SET NOCOUNT ON;
    
    SELECT  CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
           ,(CASE WHEN DJM.UpdateCancelKBN = 1 THEN 1 ELSE 0 END) AS UpdateKBN
           ,DI.OntheDayFLG
           ,DI.DeliveryName
           ,(CASE WHEN DI.DecidedDeliveryDate IS NOT NULL AND DI.DecidedDeliveryTime IS NOT NULL THEN 1 ELSE 0 END) AS DecidedDeliveryKbn
           ,(SELECT top 1 A.CarrierCD
               FROM M_Carrier A 
              WHERE A.CarrierCD = DS.CarrierCD 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CarrierCD
           ,(CASE WHEN DI.CashOnDelivery = 1 THEN 1 ELSE 0 END) AS CashOnDelivery
           ,DS.UnitsCount
           ,(CASE WHEN DJS.NouhinsyoFLG = 1 THEN 1 ELSE 0 END) AS NouhinsyoFLG
           ,DI.CommentInStore
           ,(SELECT top 1 B.Char1
               FROM M_SKU A 
               LEFT JOIN M_MultiPorpose B ON A.TaniCD = B.[Key]
                                         AND 201 = B.ID
              WHERE A.AdminNO = DSM.AdminNO 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS TANI
           ,DSM.JanCD
           ,(SELECT top 1 A.SKUName
               FROM M_SKU A 
              WHERE A.AdminNO = DSM.AdminNO 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SKUName
           ,(SELECT top 1 A.ColorName
               FROM M_SKU A 
              WHERE A.AdminNO = DSM.AdminNO 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS ColorName
           ,(SELECT top 1 A.SizeName
               FROM M_SKU A 
              WHERE A.AdminNO = DSM.AdminNO 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS SizeName
           ,(SELECT top 1 A.CommentOutStore
               FROM M_SKU A 
              WHERE A.AdminNO = DIM.AdminNO 
                AND A.ChangeDate <= DS.ShippingDate 
                AND A.DeleteFlg = 0
              ORDER BY A.ChangeDate desc) AS CommentOutStore
           ,DIM.InstructionSu
           ,DSM.ShippingSu
           ,DJM.JuchuuNO
           ,DJM.UpdateCancelKBN
           ,CASE WHEN DJ.CancelDate IS NULL THEN '0' ELSE '1' END AS CancelKbn
           ,DSM.SKUCD
           ,DSM.AdminNO
           ,DI.FromSoukoCD AS SoukoCD
           ,DI.InstructionKBN
           ,DSM.InstructionNO
           ,DSM.InstructionRows
           ,DSM.Number
           ,DSM.NumberRows
           ,DRM.ReserveNO
           ,DRM.ReserveKBN
           ,DRM.StockNO
           ,DM.ToStoreCD
           ,DM.ToSoukoCD
           ,NULL AS ToRackNO
           ,NULL AS ToStockNO
           ,DM.StoreCD AS FromStoreCD
           ,DM.FromSoukoCD AS FromSoukoCD
           ,DMM.FromRackNO
           ,DJ.CustomerCD
    FROM D_Shipping AS DS
    INNER JOIN D_ShippingDetails AS DSM ON DS.ShippingNO = DSM.ShippingNO
                                       AND DSM.ShippingKBN IN (1,2)
                                       AND DSM.DeleteDateTime IS NULL   
    LEFT OUTER JOIN D_InstructionDetails AS DIM ON DSM.InstructionNO = DIM.InstructionNO
                                               AND DSM.InstructionRows = DIM.InstructionRows
                                               AND DIM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Instruction AS DI ON DSM.InstructionNO = DI.InstructionNO
                                       AND DI.DeleteDateTime IS NULL                                                                                      
    LEFT OUTER JOIN D_Reserve AS DRM ON DIM.InstructionNO = DRM.ShippingOrderNO
                                    AND DIM.InstructionRows = DRM.ShippingOrderRows
                                    AND DRM.DeleteDateTime IS NULL                                                     
    LEFT OUTER JOIN D_JuchuuDetails AS DJM ON DRM.Number = DJM.JuchuuNO
                                          AND DRM.NumberRows = DJM.JuchuuRows
                                          AND DRM.ReserveKBN = 1
                                          AND DJM.DeleteDateTime IS NULL                                                          
    LEFT OUTER JOIN D_Juchuu AS DJ ON DJM.JuchuuNO = DJ.JuchuuNO
                                  AND DJ.DeleteDateTime IS NULL  
    LEFT OUTER JOIN D_JuchuuStatus AS DJS ON DJ.JuchuuNO = DJS.JuchuuNO   
    LEFT OUTER JOIN D_MoveDetails AS DMM ON DRM.[Number] = DMM.MoveNO
                                        AND DRM.NumberRows = DMM.MoveRows
                                        AND DRM.ReserveKBN = 2
                                        AND DMM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Move AS DM ON DMM.MoveNO = DM.MoveNO
                                AND DM.DeleteDateTime IS NULL     
    WHERE DS.DeleteDateTime IS NULL
      AND DS.ShippingNO = @ShippingNO
      AND DS.InvoiceNO IS NULL
    ORDER BY DSM.ShippingRows
    ;
    


END

GO

--********************************************--
--                                            --
--        �o�׍ς݁E����ς݃`�F�b�N          --
--                                            --
--********************************************--
CREATE PROCEDURE CheckShukkaData
    (@ShippingNO varchar(11),
     @InstructionNO varchar(11)
    )AS
    

BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    
    SET @ERRNO = '';
    
    
    --����ς�
    --�ȉ��̏����Ń��R�[�h������Δ���ςƂ��ăG���[���b�Z�[�W��\������
    IF @ShippingNO IS NOT NULL
    BEGIN
        SELECT @CNT = COUNT(DA.ShippingNO)
        FROM D_Shipping AS DA    
        WHERE DA.ShippingNO = @ShippingNO
          AND DA.DeleteDateTime IS NULL
          AND DA.InvoiceNO IS NOT NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E201';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
    END
    --�o�׍ς�
    --�ȉ��̏����Ń��R�[�h������Ώo�׍ςƂ��ăG���[���b�Z�[�W��\������
    ELSE
    BEGIN
        SELECT @CNT = COUNT(DA.ShippingNO)
        FROM D_Shipping AS DA    
        WHERE DA.InstructionNO = @InstructionNO
          AND DA.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E214';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

    END
    
    SELECT @ERRNO AS errno;

END

GO

CREATE TYPE T_Shukka AS TABLE
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
CREATE PROCEDURE PRC_ShukkaNyuuryoku
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
    
     @Table  T_Shukka READONLY,
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
           ,[ShippingDate]
           ,[InputDateTime]
           ,[StaffCD]
           ,[UnitsCount]
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
           ,@ShippingDate
           ,@SYSDATETIME           
           ,@StaffCD           
           ,@UnitsCount           
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
	       ,(CASE tbl.ReserveKBN WHEN 1 THEN 3 WHEN 2 THEN 13 ELSE 0 END)
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
