
/****** Object:  StoredProcedure [dbo].[PRC_NyuukaNyuuryoku]    Script Date: 2020/10/01 19:36:58 ******/
DROP PROCEDURE [dbo].[PRC_NyuukaNyuuryoku]
GO

/****** Object:  StoredProcedure [dbo].[PRC_NyuukaNyuuryoku]    Script Date: 2020/10/01 19:36:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--CREATE TYPE T_Nyuuka AS TABLE
--    (
--    [DataKbn][tinyint],		--1:�y�����z,2:�y�����z,3:�y�ړ��z
--    [ArrivalRows] [int],
--    [ArrivalKBN] [tinyint] ,
--    [OrderNO] [varchar](11) ,
--    [OrderRows] [int],
----    [MoveNO] [varchar](11) ,
----    [MoveRows] [int],
--    [ArrivalPlanNO] [varchar](11) ,
--    [StockNO] [varchar](11) ,
--    [ReserveNO] [varchar](11) ,
--    [CustomerCD] [varchar](13) ,
--    [ArrivalSu] [int] ,
--    [ArrivalPlanKBN][tinyint],
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_NyuukaNyuuryoku]
    (@OperateMode    int,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    @ArrivalNO   varchar(11),
    @VendorDeliveryNo  varchar(15),
    @StoreCD   varchar(4),
    @VendorCD   varchar(13),
    @ArrivalDate  varchar(10),
    @SoukoCD varchar(6) ,
    @StaffCD   varchar(10),
    @JANCD   varchar(13),
    @AdminNO int ,
    @SKUCD   varchar(30),
    @MakerItem   varchar(50),
    @ArrivalSu   int,

    @Table  T_Nyuuka READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutArrivalNO varchar(11) OUTPUT
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
    DECLARE @ArrivalPlanNO varchar(11);
    DECLARE @StockNO  varchar(11);
    DECLARE @ReserveNO varchar(11);
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
            5,             --in�`�[��� 5
            @ArrivalDate, --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @ArrivalNO OUTPUT
            ;
        
        IF ISNULL(@ArrivalNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --�yD_Arrival�zTable�]���d�l�`
        INSERT INTO [D_Arrival]
           ([ArrivalNO]
           ,[VendorDeliveryNo]
           ,[StoreCD]
           ,[VendorCD]
           ,[ArrivalDate]
           ,[InputDate]
           ,[ArrivalKBN]
           ,[StaffCD]
           ,[SoukoCD]
           ,[RackNO]
           ,[JanCD]
           ,[AdminNO]
           ,[SKUCD]
           ,[MakerItem]
           ,[ArrivalSu]
           ,[PurchaseSu]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@ArrivalNO     
           ,@VendorDeliveryNo        
           ,(SELECT top 1 M.StoreCD
                FROM M_Souko AS M
                WHERE M.SoukoCD = @SoukoCD
                AND M.ChangeDate <= @ArrivalDate
                ORDER BY M.ChangeDate desc)
           ,@VendorCD                                             
           ,convert(date,@ArrivalDate)  
           ,@SYSDATE    --InputDate
           ,(SELECT A.ArrivalPlanKBN
            FROM D_ArrivalPlan AS A
            WHERE A.ArrivalPlanNO = (SELECT top 1 tbl.ArrivalPlanNO FROM @Table tbl))	--ArrivalKBN
           ,@StaffCD
           ,@SoukoCD
           ,NULL    --RackNO
           ,@JANCD
           ,@AdminNO
           ,@SKUCD
           ,@MakerItem
           ,@ArrivalSu  --ArrivalSu
           ,0 --PurchaseSu
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

    END
        
    --�ύX--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '�ύX';
        
        UPDATE D_Arrival SET
               [VendorDeliveryNo] = @VendorDeliveryNo
              ,[StoreCD] = (SELECT top 1 M.StoreCD
                            FROM M_Souko AS M
                            WHERE M.SoukoCD = @SoukoCD
                            AND M.ChangeDate <= @ArrivalDate
                            ORDER BY M.ChangeDate desc)  
              ,[VendorCD] = @VendorCD
              ,[ArrivalDate] = @ArrivalDate
              ,[InputDate] = @SYSDATE
              ,[SoukoCD] = @SoukoCD
              ,[JanCD] = @JanCD
              ,[AdminNO] = @AdminNO
              ,[SKUCD] = @SKUCD
              ,[MakerItem] = @MakerItem
              ,[ArrivalSu] = @ArrivalSu      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE ArrivalNO = @ArrivalNO
           ;
    END
    
    ELSE IF @OperateMode = 3 --�폜--
    BEGIN
        SET @OperateModeNm = '�폜';
        
        DELETE FROM [D_Arrival]
         WHERE [ArrivalNO] = @ArrivalNO
         ;

    END
    
    --�yD_ArrivalDetails�zTable�]���d�l�a
    --�s�폜���ꂽ�f�[�^��DELETE����
    DELETE FROM [D_ArrivalDetails]
     WHERE [ArrivalNO] = @ArrivalNO
     ;
         
    IF @OperateMode <= 2    --�V�K�E�C����
    BEGIN
        INSERT INTO [D_ArrivalDetails]
                   ([ArrivalNO]
                   ,[ArrivalRows]
                   ,[ArrivalKBN]
                   ,[Number]
                   ,[ArrivalPlanNO]
                   ,[ArrivalSu]
                   ,[PurchaseSu]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @ArrivalNO                         
                   ,tbl.ArrivalRows                       
                   ,tbl.ArrivalKBN
                   ,tbl.OrderNO	--�y�����z�Ȃ�󒍔ԍ��A�y�݌Ɂz�Ȃ甭���E�ړ��ԍ�
                   ,tbl.ArrivalPlanNO  --ArrivalPlanNO
                   ,tbl.ArrivalSu
                   ,0

                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg >= 0
              ;
        
        --�J�[�\����`
        DECLARE CUR_TABLE CURSOR FOR
            SELECT tbl.ArrivalPlanNO, tbl.StockNO, tbl.ReserveNO, tbl.ArrivalSu
            ,DS.PlanSu-tbl.ArrivalSu AS ArrivalPlanSu	--�i���̃��R�[�h��PlanSu - ��ʖ���.���א��j>�O�Ȃ�INSERT
            ,tbl.DataKbn
            ,(CASE WHEN DP.InsertOperator = 'Nyuuka' THEN 0 ELSE 1 END) AS SakuseiFlg
            FROM @Table AS tbl
            LEFT OUTER JOIN D_ArrivalPlan AS DP
            ON DP.ArrivalPlanNO = tbl.ArrivalPlanNO
            AND DP.DeleteDateTime IS NULL
            LEFT OUTER JOIN D_Stock As DS
            ON DS.StockNO = tbl.StockNO
            AND DS.DeleteDateTime IS NULL
            ORDER BY tbl.ArrivalPlanNO
            ;
        
        DECLARE @tblArrivalPlanNO varchar(11);
        DECLARE @oldArrivalPlanNO varchar(11);
        DECLARE @tblStockNO varchar(11);
        DECLARE @tblReserveNO varchar(11);
        DECLARE @tblArrivalSu int;
        DECLARE @tblArrivalPlanSu int;
        DECLARE @tblDataKbn tinyint;	--1:�y�����z,2:�y�����z,3:�y�ړ��z
        DECLARE @SakuseiFlg tinyint;    --0:�쐬���Ȃ��@1:�쐬����

        --�J�[�\���I�[�v��
        OPEN CUR_TABLE;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @tblArrivalPlanNO, @tblStockNO, @tblReserveNO, @tblArrivalSu, @tblArrivalPlanSu, @tblDataKbn, @SakuseiFlg;
        
        SET @oldArrivalPlanNO = '';
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ���[�v���̎��ۂ̏��� ��������===
            IF @tblArrivalPlanSu > 0 AND @SakuseiFlg = 1
            BEGIN
                IF @oldArrivalPlanNO <> @tblArrivalPlanNO 
                BEGIN
                
                    --�`�[�ԍ��̔�
                    EXEC Fnc_GetNumber
                        22,             --in�`�[��� 5
                        @ArrivalDate, --in���
                        @StoreCD,       --in�X��CD
                        @Operator,
                        @ArrivalPlanNO OUTPUT
                        ;
                    
                    IF ISNULL(@ArrivalPlanNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                
                    --�yD_ArrivalPlan�z�������X�V�iInsert�j Table�]���d�l�b     Table�̃f�[�^��Loop����K�v����?
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
                           ,DP.ArrivalPlanKBN
                           ,DP.Number
                           ,DP.NumberRows
                           ,DP.NumberSEQ
                           ,DP.ArrivalPlanDate
                           ,DP.ArrivalPlanMonth
                           ,DP.ArrivalPlanCD
                           ,DP.CalcuArrivalPlanDate
                           ,@SYSDATETIME    --ArrivalPlanUpdateDateTime
                           ,@StaffCD
                           ,1 AS LastestFLG
                           ,DP.EDIImportNO
                           ,DP.SoukoCD
                           ,DP.SKUCD
                           ,DP.AdminNO
                           ,DP.JanCD
                           ,DP.ArrivalPlanSu-@tblArrivalSu  --���̃��R�[�h��ArrivalPlanSu - ���ד��א�
                           ,0   --ArrivalSu
                           ,DP.ArrivalPlanNO    --���̃��R�[�h��ArrivalPlanNO
                           ,DP.OrderCD
                           ,DP.FromSoukoCD
                           ,DP.ToStoreCD
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                     FROM D_ArrivalPlan AS DP
                     WHERE DP.ArrivalPlanNO = @tblArrivalPlanNO
                     AND DP.DeleteDateTime IS NULL
                     AND DP.ArrivalPlanSu-@tblArrivalSu > 0
                     ;
 
                    --�`�[�ԍ��̔�
                    EXEC Fnc_GetNumber
                        21,             --in�`�[��� 21
                        @ArrivalDate, --in���
                        @StoreCD,       --in�X��CD
                        @Operator,
                        @StockNO OUTPUT
                        ;
                    
                    IF ISNULL(@StockNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --�yD_Stock�z�������X�V�iInsert�j
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
                           ,DS.SoukoCD
                           ,NULL    --RackNO
                           ,@ArrivalPlanNO
                           ,DS.SKUCD
                           ,DS.AdminNO
                           ,DS.JanCD
                           ,1   --  ArrivalYetFLG
                           ,DS.ArrivalPlanKBN
                           ,DS.ArrivalPlanDate
                           ,NULL    --ArrivalDate
                           ,0   --StockSu
                           ,@tblArrivalPlanSu   --PlanSu
                           --���̃��R�[�h��AllowableSu-���L ���̃��R�[�h�֍X�V�����l
                           ,DS.AllowableSu-(CASE WHEN @tblArrivalSu - ReserveSu > 0 THEN @tblArrivalSu - ReserveSu ELSE 0 END)--AllowableSu
                           --���̃��R�[�h��AnotherStoreAllowableSu-���L ���̃��R�[�h�֍X�V�����l
                           ,DS.AnotherStoreAllowableSu - (CASE WHEN @tblArrivalSu - ReserveSu > 0 THEN @tblArrivalSu - ReserveSu ELSE 0 END)  --AnotherStoreAllowableSu
                           --���̃��R�[�h��ReserveSu-���L ���̃��R�[�h�֍X�V�����l
                           ,DS.ReserveSu - (CASE WHEN @tblArrivalSu - DS.ReserveSu > 0 THEN DS.ReserveSu ELSE @tblArrivalSu END)  --ReserveSu
                           ,DS.InstructionSu
                           ,DS.ShippingSu
                           ,DS.StockNO  --OriginalStockNO
                           ,DS.ExpectReturnDate
                           ,DS.ReturnDate
                           ,DS.ReturnSu
                     
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                     FROM D_Stock AS DS
                     WHERE DS.StockNO = @tblStockNO
                    ;
                                        
                END     --@tblArrivalPlanNO���قȂ�ꍇ
                ELSE
                BEGIN
                    --���ד��œ���ArrivalPlanNO�AStockNO�̏ꍇ�͐V�K��INSERT�������ʂ�UPDATE�iINSERT����ƍ݌ɂ��{�����Ă��܂��j
                    UPDATE D_ArrivalPlan SET
                       [ArrivalPlanSu] = ArrivalPlanSu - @tblArrivalSu
                     WHERE ArrivalPlanNO = @ArrivalPlanNO
                     AND ArrivalPlanSu-@tblArrivalSu > 0
                     ;
                     
                    --�yD_Stock�z           Update  Table�]���d�l�c
                    UPDATE [D_Stock] SET
                        StockSu = StockSu + @tblArrivalSu
                       ,PlanSu = PlanSu + @tblArrivalPlanSu	--(CASE WHEN PlanSu - @tblArrivalSu > 0 THEN PlanSu - @tblArrivalSu ELSE 0 END)   --PlanSu
                       ,AllowableSu = AllowableSu - @tblArrivalSu   --AllowableSu
                       ,AnotherStoreAllowableSu = AnotherStoreAllowableSu - @tblArrivalSu   --AnotherStoreAllowableSu
                       ,ReserveSu = ReserveSu - @tblArrivalSu   --ReserveSu
                    WHERE StockNO = @StockNO
                    ;
                END

                IF @tblDataKbn = 1
                BEGIN
                    --�`�[�ԍ��̔�
                    EXEC Fnc_GetNumber
                        12,             --in�`�[��� 12
                        @ArrivalDate, --in���
                        @StoreCD,       --in�X��CD
                        @Operator,
                        @ReserveNO OUTPUT
                        ;
                    
                    IF ISNULL(@ReserveNO,'') = ''
                    BEGIN
                        SET @W_ERR = 1;
                        RETURN @W_ERR;
                    END
                    
                    --�yD_Reserve�z�������X�V�iInsert�j
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
                           ,DR.ReserveKBN
                           ,DR.Number
                           ,DR.NumberRows
                           ,@StockNO
                           ,DR.SoukoCD
                           ,DR.JanCD
                           ,DR.SKUCD
                           ,DR.AdminNO
                           ,DR.ReserveSu - @tblArrivalSu    --���̃��R�[�h��ReserveSu - ���ד��א�
                           ,NULL    --ShippingPossibleDate
                           ,0       --ShippingPossibleSU
                           ,NULL    --ShippingOrderNO
                           ,0       --ShippingOrderRows
                           ,NULL    --CompletedPickingNO
                           ,0       --CompletedPickingRow
                           ,NULL    --CompletedPickingDate
                           ,0       --ShippingSu
                           ,0       --ReturnKBN
                           ,DR.OriginalReserveNO
                     
                           ,@Operator  
                           ,@SYSDATETIME
                           ,@Operator  
                           ,@SYSDATETIME
                           ,NULL                  
                           ,NULL
                     FROM D_Reserve AS DR
                     WHERE @tblReserveNO = DR.ReserveNO
                    ;
                END
            END
            
            --�yD_Stock�z           Update  Table�]���d�l�c
            UPDATE [D_Stock] SET
                   [ArrivalYetFLG]           = 0
                  ,[ArrivalDate]             = @SYSDATE
                  ,[StockSu]                 = [StockSu] + @tblArrivalSu
                  ,[PlanSu]                  = 0
                  ,[AllowableSu]             = (CASE WHEN @tblArrivalSu - ReserveSu > 0 THEN @tblArrivalSu - ReserveSu ELSE 0 END)   --AllowableSu
                  ,[AnotherStoreAllowableSu] = (CASE WHEN @tblArrivalSu - ReserveSu > 0 THEN @tblArrivalSu - ReserveSu ELSE 0 END)   --AnotherStoreAllowableSu
                  ,[ReserveSu]               = (CASE WHEN @tblArrivalSu - ReserveSu > 0 THEN ReserveSu ELSE @tblArrivalSu END)   --ReserveSu
                  ,[UpdateOperator]          = @Operator  
                  ,[UpdateDateTime]          = @SYSDATETIME
                  
             FROM D_Stock AS DS
             WHERE DS.StockNO = @tblStockNO
             AND DS.DeleteDateTime IS NULL
            ;
            
            IF @tblDataKbn = 1
            BEGIN
                --�yD_Reserve�z         Update  Table�]���d�l�d
                UPDATE [D_Reserve] SET
                       [ShippingPossibleDate] = @SYSDATE
                      ,[ShippingPossibleSU] = [ShippingPossibleSU] + @tblArrivalSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Reserve AS DR
                 WHERE DR.ReserveNO  = @tblReserveNO
                 AND DR.DeleteDateTime IS NULL
                ;
            END
            
            SET @oldArrivalPlanNO = @tblArrivalPlanNO;
            
            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_TABLE
            INTO @tblArrivalPlanNO, @tblStockNO, @tblReserveNO, @tblArrivalSu, @tblArrivalPlanSu, @tblDataKbn, @SakuseiFlg;

        END
        
        --�J�[�\�������
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;
        

        --�yD_ArrivalPlan�zUpdate   Table�]���d�l�b	����
        UPDATE [D_ArrivalPlan] SET
           [ArrivalPlanSu] = tbl.ArrivalSu
          ,[ArrivalSu] = tbl.ArrivalSu
          ,[UpdateOperator]     =  @Operator  
          ,[UpdateDateTime]     =  @SYSDATETIME
        
         FROM (SELECT tbl.ArrivalPlanNO, SUM(tbl.ArrivalSu) AS ArrivalSu
                FROM @Table AS tbl
                WHERE tbl.UpdateFlg >= 0
                GROUP BY tbl.ArrivalPlanNO
         ) AS tbl
         WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
        ;
        
    END
	ELSE	--�폜��
	BEGIN
        --�yD_ArrivalPlan�z     Update/Delete   Table�]���d�l�b�A
        UPDATE [D_ArrivalPlan] SET
           [ArrivalPlanSu]  = D_ArrivalPlan.[ArrivalPlanSu] + ISNULL(DS2.ArrivalPlanSu,0)		--��
          ,[ArrivalSu]      = D_ArrivalPlan.[ArrivalSu] - tbl.ArrivalSu
          ,[UpdateOperator] = @Operator  
          ,[UpdateDateTime] = @SYSDATETIME
        
        FROM (SELECT tbl.ArrivalPlanNO, SUM(tbl.ArrivalSu) AS ArrivalSu
              FROM @Table AS tbl
              WHERE tbl.UpdateFlg >= 0
              GROUP BY tbl.ArrivalPlanNO
        ) AS tbl
        LEFT OUTER JOIN (SELECT D.OriginalArrivalPlanNO
                              , SUM(D.ArrivalPlanSu) AS ArrivalPlanSu
                         FROM D_ArrivalPlan D
                         GROUP BY D.OriginalArrivalPlanNO
        )AS DS2
        ON DS2.OriginalArrivalPlanNO = tbl.ArrivalPlanNO
        WHERE tbl.ArrivalPlanNO = D_ArrivalPlan.ArrivalPlanNO
        ;
        
        --�yD_ArrivalPlan�z  �������폜�iDelete�j
        DELETE FROM D_ArrivalPlan
        WHERE EXISTS (SELECT 1 FROM D_ArrivalPlan AS DP 
                      INNER JOIN @Table AS tbl
                      ON tbl.ArrivalPlanNO = DP.ArrivalPlanNO
                      AND tbl.UpdateFlg >= 0
                      WHERE DP.ArrivalPlanNO = D_ArrivalPlan.OriginalArrivalPlanNO)
        ;

        --�yD_Stock�z           Update/Delete   Table�]���d�l�c�A
        UPDATE [D_Stock] SET
               [ArrivalYetFLG]  = 1
              ,[ArrivalDate]    = NULL
              ,[StockSu]        = [D_Stock].[StockSu] - tbl.ArrivalSu
              ,[PlanSu]         = tbl.ArrivalSu + ISNULL(DS2.PlanSu,0)
              ,[AllowableSu]    = [D_Stock].[AllowableSu] + ISNULL(DS2.PlanSu,0)
              ,[AnotherStoreAllowableSu] = [D_Stock].[AnotherStoreAllowableSu] + ISNULL(DS2.AnotherStoreAllowableSu,0)
              ,[ReserveSu]      = [D_Stock].[ReserveSu] - tbl.ArrivalSu + ISNULL(DS2.ReserveSu,0)
              ,[UpdateOperator] = @Operator  
              ,[UpdateDateTime] = @SYSDATETIME
              
         FROM (SELECT tbl.StockNO, SUM(tbl.ArrivalSu) AS ArrivalSu
                FROM @Table AS tbl
                WHERE tbl.UpdateFlg >= 0
                GROUP BY tbl.StockNO
         ) AS tbl
         LEFT OUTER JOIN (SELECT D.OriginalStockNO
                               , SUM(D.PlanSu) AS PlanSu
                               , SUM(D.AnotherStoreAllowableSu) AS AnotherStoreAllowableSu
                               , SUM(D.ReserveSu) AS ReserveSu
                          FROM D_Stock D
                          GROUP BY D.OriginalStockNO
         )AS DS2
         ON DS2.OriginalStockNO = tbl.StockNO
         WHERE tbl.StockNO = D_Stock.StockNO
        ;
        
        --�yD_Stock�z  �������폜�iDelete�j
        DELETE FROM D_Stock
        WHERE EXISTS (SELECT 1 FROM D_Stock AS DS
                      INNER JOIN @Table AS tbl
                      ON tbl.StockNO = DS.StockNO
                      AND tbl.UpdateFlg = 0
                      WHERE DS.StockNO = D_Stock.OriginalStockNO)
        ;
        
        --�yD_Reserve�z         Update/Delete   Table�]���d�l�d�A
        UPDATE [D_Reserve] SET
               [ShippingPossibleDate] = NULL
              ,[ShippingPossibleSU]   = [ShippingPossibleSU] - tbl.ArrivalSu
              ,[UpdateOperator]       =  @Operator  
              ,[UpdateDateTime]       =  @SYSDATETIME
              
        FROM @Table AS tbl
        WHERE tbl.ReserveNO = D_Reserve.ReserveNO
        AND tbl.UpdateFlg = 0
        AND tbl.DataKbn = 1
        ;
        
        --�yD_Reserve�z  �������폜�iDelete�j
        DELETE FROM D_Reserve
        WHERE EXISTS (SELECT 1 FROM D_Reserve AS DR
                      INNER JOIN @Table AS tbl
                      ON tbl.ReserveNO = DR.ReserveNO
                      AND tbl.UpdateFlg = 0
                      AND tbl.DataKbn = 1
                      WHERE DR.OriginalReserveNO = D_Reserve.ReserveNO)
        ;

	END

    --�yD_Warehousing�z�ǉ��X�V�iInsert)  Table�]���d�l�e ����
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
    SELECT (CASE WHEN @OperateMode = 3 THEN (CASE WHEN @ArrivalDate > @SYSDATE THEN @ArrivalDate ELSE @SYSDATE END)
                 ELSE @ArrivalDate END)	--WarehousingDate
       ,@SoukoCD
       ,NULL	--RackNO
       ,@StockNO
       ,@JanCD
       ,@AdminNO
       ,@SKUCD
       ,1	--WarehousingKBN
       ,(CASE WHEN @OperateMode = 3 THEN 1 ELSE 0 END)	--DeleteFlg
       ,@ArrivalNO	--Number
       ,tbl.ArrivalRows	--NumberRow
       ,@VendorCD
       ,NULL	--ToStoreCD
       ,NULL	--ToSoukoCD
       ,NULL	--ToRackNO
       ,NULL	--ToStockNO
       ,NULL    --FromStoreCD
       ,NULL	--FromSoukoCD]
       ,NULL	--FromRackNO
       ,tbl.CustomerCD
       ,(CASE @OperateMode WHEN 3 THEN -1 ELSE 1 END) * tbl.ArrivalSu	--Quantity
       ,'NyuukaNyuuryoku'	--Program
       
       ,@Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
       ,NULL
       ,NULL

      FROM @Table tbl
      WHERE tbl.UpdateFlg >= 0
      AND tbl.ArrivalPlanKBN = 1	--����
      ;

    --�yD_Warehousing�z�ǉ��X�V�iInsert)  Table�]���d�l�e �ړ�
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
    SELECT @ArrivalDate	--WarehousingDate
       ,@SoukoCD
       ,NULL	--RackNO
       ,@StockNO
       ,@JanCD
       ,@AdminNO
       ,@SKUCD
 --      ,14    --WarehousingKBN
        ,41     --2020/10/01 Fukuda 
       ,(CASE WHEN @OperateMode = 3 THEN 1 ELSE 0 END)	--DeleteFlg
       ,@ArrivalNO	--Number
       ,tbl.ArrivalRows --NumberRow
       ,NULL    --VendorCD
       ,NULL    --ToStoreCD
       ,NULL    --ToSoukoCD
       ,NULL    --ToRackNO
       ,NULL    --ToStockNO
       ,(SELECT top 1 M.StoreCD
         FROM M_Souko AS M
         WHERE M.SoukoCD = @SoukoCD
         AND M.ChangeDate <= @ArrivalDate
         ORDER BY M.ChangeDate desc)  --FromStoreCD
       ,@SoukoCD	--FromSoukoCD]
       ,NULL	--FromRackNO
       ,tbl.CustomerCD
       ,(CASE @OperateMode WHEN 3 THEN -1 ELSE 1 END) * tbl.ArrivalSu	--Quantity
       ,'NyuukaNyuuryoku'	--Program
       
       ,@Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
       ,NULL
       ,NULL

      FROM @Table tbl
      WHERE tbl.UpdateFlg >= 0
      AND tbl.ArrivalPlanKBN = 2	--�ړ�
      ;
    
    --�J�[�\����`
    DECLARE CUR_AAA CURSOR FOR
            SELECT tbl.OrderNO, tbl.OrderRows, tbl.ArrivalSu
            FROM @Table AS tbl
            WHERE tbl.DataKbn > 1
        UNION ALL
            SELECT DO.OrderNO, DO.OrderRows, tbl.ArrivalSu
            FROM @Table AS tbl
            INNER JOIN D_OrderDetails AS DO
            ON DO.JuchuuNO = tbl.OrderNO
            AND DO.JuchuuRows = tbl.OrderRows
            WHERE tbl.DataKbn = 1
        ORDER BY OrderNO, OrderRows
        ;
    
    DECLARE @OrderNO varchar(11);
    DECLARE @OrderRows int;
	
    --�J�[�\���I�[�v��
    OPEN CUR_AAA;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_AAA
    INTO @OrderNO, @OrderRows, @tblArrivalSu;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===
        --�����t�@�C����SEQ���d�����Ă��܂����߃w�b�_���X�V
        UPDATE D_Order SET
            [UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE OrderNO = @OrderNO
         ;
         
        --�yD_OrderDetails�z    Update  Table�]���d�l�f     Table�̃f�[�^��Loop����K�v����
        UPDATE D_OrderDetails
            SET [TotalArrivalSu] = [TotalArrivalSu] + (CASE WHEN @OperateMode = 3 THEN -1 * @tblArrivalSu ELSE @tblArrivalSu END)
               ,[UpdateOperator] = @Operator  
               ,[UpdateDateTime] = @SYSDATETIME
         WHERE OrderNO = @OrderNO
         AND OrderRows = @OrderRows
         ;
            
        --�yD_MoveDetails�z     Update  Table�]���d�l�g
        UPDATE D_MoveDetails
            SET [TotalArrivalSu] =  [TotalArrivalSu] + (CASE WHEN @OperateMode = 3 THEN -1 * @tblArrivalSu ELSE @tblArrivalSu END)
               ,[UpdateOperator] =  @Operator  
               ,[UpdateDateTime] =  @SYSDATETIME
         WHERE MoveNO = @OrderNO
         AND MoveRows = @OrderRows
         ;
        -- ========= ���[�v���̎��ۂ̏��� �����܂�===

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO @OrderNO, @OrderRows, @tblArrivalSu;

    END
    
    --�J�[�\�������
    CLOSE CUR_AAA;
    DEALLOCATE CUR_AAA;
    
    --���������f�[�^�֍X�V
    SET @KeyItem = @ArrivalNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NyuukaNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutArrivalNO = @ArrivalNO;
    
--<<OWARI>>
  return @W_ERR;

END


GO


