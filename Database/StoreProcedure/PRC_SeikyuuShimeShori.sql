/****** Object:  StoredProcedure [dbo].[PRC_SeikyuuShimeShori]    Script Date: 2021/05/31 13:35:55 ******/
IF EXISTS (SELECT * FROM sys.procedures WHERE name like '%PRC_SeikyuuShimeShori%' and type like '%P%')
DROP PROCEDURE [dbo].[PRC_SeikyuuShimeShori]
GO

/****** Object:  StoredProcedure [dbo].[PRC_SeikyuuShimeShori]    Script Date: 2021/05/31 13:35:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [PRC_SeikyuuShimeShori]    */
CREATE PROCEDURE [dbo].[PRC_SeikyuuShimeShori](
    -- Add the parameters for the stored procedure here
    @Syori    tinyint,        -- �����敪�i1:������,2:�������L�����Z��,3:�����m��j
    @StoreCD  varchar(4),
    @CustomerCD  varchar(13),
    @ChangeDate  varchar(10),
    @BillingCloseDate tinyint,
    @Operator  varchar(10),
    @PC  varchar(30)
)AS
BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @ProcessingNO  varchar(11);
    DECLARE @BillingNO varchar(11);
    DECLARE @KeyItem varchar(100);

    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    DECLARE @Yoteibi  varchar(10);
    
    --�J�[�\���̒l���擾����ϐ��錾
    DECLARE @W_COL1 varchar(13);    --CustomerCD
    DECLARE @W_COL2 date;    		--NextCollectPlanDate
    DECLARE @HontaiGaku        money;
    DECLARE @HontaiGaku0       money;
    DECLARE @HontaiGaku8       money;
    DECLARE @HontaiGaku10      money;
    DECLARE @Tax               money;
    DECLARE @Tax8              money;
    DECLARE @Tax10             money;
    DECLARE @CollectPlanGaku   money;
    DECLARE @AdjustTax8        money;
    DECLARE @AdjustTax10       money;
    
    IF ISNULL(@CustomerCD,'') = ''
    BEGIN
        SET @CustomerCD = NULL;
    END
    
    --������--
    IF @Syori = 1
    BEGIN
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            27,             --in�`�[��� 27
            @ChangeDate,    --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @ProcessingNO OUTPUT
            ;
            
        --�yD_BillingProcessing�zINSERT Table�]���d�l�`
        INSERT INTO [D_BillingProcessing]
                   ([ProcessingNO]
                   ,[StoreCD]
                   ,[BillingDate]
                   ,[CustomerCD]
                   ,[ProcessingKBN]
                   ,[ProcessingDateTime]
                   ,[StaffCD]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             VALUES(
                    @ProcessingNO
                   ,@StoreCD
                   ,CONVERT(date,@ChangeDate)
                   ,@CustomerCD     --@W_COL1(CustomerCD)
                   ,1               -- ProcessingKBN
                   ,@SYSDATETIME    --ProcessingDateTime
                   ,@Operator
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL  
                   ,NULL
                   ,NULL                  
                   ,NULL

            );
            
        --�J�[�\����`
        DECLARE CUR_AAA CURSOR FOR
            SELECT DC.CustomerCD
                --,MIN(DC.NextCollectPlanDate) AS NextCollectPlanDate
                ,CONVERT(date, @ChangeDate) AS NextCollectPlanDate
                ,SUM(DC.HontaiGaku) AS   HontaiGaku
                ,SUM(DC.HontaiGaku0) AS  HontaiGaku0
                ,SUM(DC.HontaiGaku8) AS  HontaiGaku8
                ,SUM(DC.HontaiGaku10) AS HontaiGaku10
                ,SUM(DC.Tax) AS          Tax
                ,SUM(DC.Tax8) AS         Tax8
                ,SUM(DC.Tax10) AS        Tax10
                ,SUM(DC.CollectPlanGaku) AS CollectPlanGaku
                
                ,(CASE W.TaxTiming WHEN 3 THEN (CASE W.TaxFractionKBN WHEN 1 THEN FLOOR(SUM(ISNULL(W.AdjustTax8,0))*0.08)   --�؂�̂� 
                                                WHEN 2 THEN ROUND(SUM(ISNULL(W.AdjustTax8,0))*0.08,0)       --�l�̌ܓ�
                                                WHEN 3 THEN CEILING(SUM(ISNULL(W.AdjustTax8,0))*0.08)       --�؂�グ
                                                ELSE 0 END)
                    ELSE SUM(ISNULL(W.AdjustTax8,0)) END)
                    - (CASE W.TaxPrintKBN WHEN 1 THEN 0 ELSE SUM(DC.Tax8) END) AS AdjustTax8
                ,(CASE W.TaxTiming WHEN 3 THEN (CASE W.TaxFractionKBN WHEN 1 THEN FLOOR(SUM(ISNULL(W.AdjustTax10,0))*0.08)   --�؂�̂� 
                                                WHEN 2 THEN ROUND(SUM(ISNULL(W.AdjustTax10,0))*0.1,0)       --�l�̌ܓ�
                                                WHEN 3 THEN CEILING(SUM(ISNULL(W.AdjustTax10,0))*0.1)       --�؂�グ
                                                ELSE 0 END)
                    ELSE SUM(ISNULL(W.AdjustTax10,0)) END)
                    - (CASE W.TaxPrintKBN WHEN 1 THEN 0 ELSE SUM(DC.Tax10) END) AS AdjustTax10
            FROM D_CollectPlan AS DC
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DC.CustomerCD
            
            LEFT OUTER JOIN (
                SELECT DM.CollectPlanNO
                    ,FC.TaxTiming
                    ,FC.TaxPrintKBN
                    ,FC.TaxFractionKBN
                    ,(CASE FC.TaxPrintKBN WHEN 1 THEN 0     --�P���ł̂Ƃ�
                            WHEN 2 THEN (CASE FC.TaxTiming WHEN 1 THEN (CASE FC.TaxFractionKBN WHEN 1 THEN SUM(FLOOR(DM.HontaiGaku * DM.TaxRitsu * 0.01))
                                                                WHEN 2 THEN SUM(ROUND(DM.HontaiGaku * DM.TaxRitsu * 0.01,0))
                                                                WHEN 3 THEN SUM(CEILING(DM.HontaiGaku * DM.TaxRitsu * 0.01))
                                                                ELSE 0 END) --1:���ז��̂Ƃ�
                                        WHEN 2 THEN (CASE FC.TaxFractionKBN WHEN 1 THEN MAX(FLOOR(DC.HontaiGaku8 * 0.08))
                                                            WHEN 2 THEN MAX(ROUND(DC.HontaiGaku8 * 0.08,0))
                                                            WHEN 3 THEN MAX(CEILING(DC.HontaiGaku8 * 0.08))
                                                            ELSE 0 END) --2:�`�[���̂Ƃ�
                                        WHEN 3 THEN MAX(DC.HontaiGaku8)     -- * 0.08��� 3:�����̂Ƃ�
                                        ELSE 0 END)     --�Q�O�ł̂Ƃ�
                            ELSE 0 END) AS AdjustTax8
                    ,(CASE FC.TaxPrintKBN WHEN 1 THEN 0     --�P���ł̂Ƃ�
                    		WHEN 2 THEN (CASE FC.TaxTiming WHEN 1 THEN (CASE FC.TaxFractionKBN WHEN 1 THEN SUM(FLOOR(DM.HontaiGaku * DM.TaxRitsu * 0.01)) 
                                                                WHEN 2 THEN SUM(ROUND(DM.HontaiGaku * DM.TaxRitsu * 0.01,0))
                                                                WHEN 3 THEN SUM(CEILING(DM.HontaiGaku * DM.TaxRitsu * 0.01))
                                                                ELSE 0 END) --1:���ז��̂Ƃ�
                                        WHEN 2 THEN (CASE FC.TaxFractionKBN WHEN 1 THEN MAX(FLOOR(DC.HontaiGaku10 * 0.1))
                                                            WHEN 2 THEN MAX(ROUND(DC.HontaiGaku10 * 0.1,0))
                                                            WHEN 3 THEN MAX(CEILING(DC.HontaiGaku10 * 0.1))
                                                            ELSE 0 END)	--2:�`�[���̂Ƃ�
                                        WHEN 3 THEN MAX(DC.HontaiGaku10)     -- * 0.08��� 3:�����̂Ƃ�
                                        ELSE 0 END)     --�Q�O�ł̂Ƃ�
                            ELSE 0 END) AS AdjustTax10
                FROM D_CollectPlanDetails AS DM
                INNER JOIN D_CollectPlan AS DC
                ON DC.CollectPlanNO = DM.CollectPlanNO
                AND DC.DeleteDateTime IS NULL
                LEFT OUTER JOIN F_Customer(@ChangeDate) As FC
                ON FC.CustomerCD = DC.CustomerCD
                AND FC.DeleteFlg = 0
                WHERE DM.DeleteDateTime IS NULL
                GROUP BY DM.CollectPlanNO, FC.TaxTiming, FC.TaxPrintKBN, FC.TaxFractionKBN
                ) AS W
            ON W.CollectPlanNO = DC.CollectPlanNO            
            
            WHERE --DC.BillingNO IS Null       2019.10.23 chg
            DC.MonthlyBillingNO IS Null       
            AND DC.DeleteOperator IS Null       
            AND DC.DeleteDateTime IS Null       
            AND DC.StoreCD = @StoreCD
            AND DC.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DC.CustomerCD END)
			--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			--AND DC.BillingDate <= CONVERT(date, @ChangeDate)   
			AND (DC.BillingDate IS NULL OR (DC.BillingDate IS NOT NULL AND DC.BillingDate <= CONVERT(date, @ChangeDate)))
			--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
            AND DC.InvalidFLG = 0
            AND DC.BillingConfirmFlg = 0
            AND DC.BillingType = 2
            GROUP BY DC.CustomerCD,W.TaxPrintKBN,W.TaxTiming, W.TaxFractionKBN
            ORDER BY DC.CustomerCD
            ;

        --�J�[�\���I�[�v��
        OPEN CUR_AAA;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA
        INTO @W_COL1
            ,@W_COL2
            ,@HontaiGaku        
            ,@HontaiGaku0
            ,@HontaiGaku8
            ,@HontaiGaku10
            ,@Tax
            ,@Tax8
            ,@Tax10
            ,@CollectPlanGaku
            ,@AdjustTax8
            ,@AdjustTax10
            ;

        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN

        -- ========= ���[�v���̎��ۂ̏��� ��������===
            --������Čv�Z
            EXEC Fnc_PlanDate_SP
                0,             --0:���,1:�x��
                @W_COL1,       --in�ڋqCD
                @ChangeDate,    --in���
                0,              --���[�敪
                @Yoteibi OUTPUT
                ;
            SET @W_COL2 = CONVERT(date, @Yoteibi);

            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                15,             --in�`�[��� 15
                @ChangeDate,    --in���
                @StoreCD,       --in�X��CD
                @Operator,
                @BillingNO OUTPUT
                ;
                
            --�yD_Billing�zINSERT Table�]���d�l�a
            INSERT INTO [D_Billing]
                   ([BillingNO]
                   ,[BillingType]	--2019.10.23 add
                   ,[StoreCD]
                   ,[BillingCloseDate]
                   ,[CollectPlanDate]
                   ,[BillingCustomerCD]
                   ,[ProcessingNO]
                   ,[SumBillingHontaiGaku]
                   ,[SumBillingHontaiGaku0]
                   ,[SumBillingHontaiGaku8]
                   ,[SumBillingHontaiGaku10]
                   ,[SumBillingTax]
                   ,[SumBillingTax8]
                   ,[SumBillingTax10]
                   ,[SumBillingGaku]
                   ,[AdjustHontaiGaku0]
                   ,[AdjustHontaiGaku8]
                   ,[AdjustHontaiGaku10]
                   ,[AdjustTax8]
                   ,[AdjustTax10]
                   ,[TotalBillingHontaiGaku]
                   ,[TotalBillingHontaiGaku0]
                   ,[TotalBillingHontaiGaku8]
                   ,[TotalBillingHontaiGaku10]
                   ,[TotalBillingTax]
                   ,[TotalBillingTax8]
                   ,[TotalBillingTax10]
                   ,[BillingGaku]
                   ,[PrintDateTime]
                   ,[PrintStaffCD]
                   ,[CollectDate]
                   ,[LastCollectDate]
                   ,[CollectStaffCD]
                   ,[CollectGaku]
                   ,[LastBillingGaku]
                   ,[LastCollectGaku]
                   ,[BillingConfirmFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             VALUES
                   (@BillingNO
                   ,2	--BillingType 2019.10.23 add
                   ,@StoreCD	
                   ,CONVERT(date, @ChangeDate)	--BillingCloseDate
                   ,@W_COL2		--CollectPlanDate
                   ,@W_COL1		--BillingCustomerCD
                   ,@ProcessingNO
                   ,@HontaiGaku		--SumBillingHontaiGaku, money,>
                   ,@HontaiGaku0	--SumBillingHontaiGaku0, money,>
                   ,@HontaiGaku8	--SumBillingHontaiGaku8, money,>
                   ,@HontaiGaku10	--SumBillingHontaiGaku10, money,>
                   ,@Tax	--SumBillingTax �W�v����Ŋz
                   ,@Tax8	--SumBillingTax8,�W�v����Ŋz8
                   ,@Tax10	--SumBillingTax10,�W�v����Ŋz10
                   ,@HontaiGaku + @Tax	--SumBillingGaku,�W�v�����z
                   ,0	--AdjustHontaiGaku0
                   ,0	--AdjustHontaiGaku8, money,>
                   ,0	--AdjustHontaiGaku10, money,>
                   ,@AdjustTax8	--AdjustTax8, money,>
                   ,@AdjustTax10	--AdjustTax10, money,>
                   ,@HontaiGaku        --[TotalBillingHontaiGaku]
                   ,@HontaiGaku0       --[TotalBillingHontaiGaku0]
                   ,@HontaiGaku8       --[TotalBillingHontaiGaku8]
                   ,@HontaiGaku10      --[TotalBillingHontaiGaku10]
                   ,@Tax + @AdjustTax8 + @AdjustTax10   --[TotalBillingTax]
                   ,@Tax8 + @AdjustTax8                 --[TotalBillingTax8]
                   ,@Tax10 + @AdjustTax10               --[TotalBillingTax10]
                   ,@HontaiGaku + @Tax + @AdjustTax8 + @AdjustTax10   --[BillingGaku]
                   ,NULL    --PrintDateTime
                   ,NULL    --PrintStaffCD
                   ,NULL    --CollectDate
                   ,NULL    --LastCollectDate
                   ,NULL    --CollectStaffCD>
                   ,NULL    --CollectGaku
                   ,ISNULL((SELECT top 1 A.LastBillingGaku-A.LastCollectGaku+A.BillingGaku
                            FROM D_Billing AS A
                            WHERE A.StoreCD = @StoreCD
                            AND A.BillingCustomerCD = @W_COL1
                            AND A.BillingCloseDate < CONVERT(date, @ChangeDate)    
                            AND A.DeleteDateTime IS Null
                            AND A.BillingConfirmFlg = 1 --�m��ς�
                            ORDER BY A.BillingCloseDate desc
                            ),0)
                   ,ISNULL((SELECT SUM(A.CollectAmount)
                            FROM D_CollectBilling AS A
                            INNER JOIN D_PaymentConfirm AS B
                            ON B.ConfirmNO = A.ConfirmNO
                            AND B.CollectClearDate <= CONVERT(date, @ChangeDate)
                            AND B.DeleteDateTime IS Null
                            INNER JOIN D_CollectPlan AS C
                            ON C.CollectPlanNO = A.CollectPlanNO
                            AND C.DeleteDateTime IS Null
                            AND C.CustomerCD = @W_COL1
                            WHERE A.DeleteDateTime IS Null
                            AND B.CollectClearDate > ISNULL((SELECT MAX(DB.BillingCloseDate) FROM D_Billing AS DB 
                                                        WHERE DB.StoreCD = @StoreCD
                                                        AND DB.BillingCustomerCD = @W_COL1),DATEADD(DAY,-1,B.CollectClearDate))
                           ),0)    --LastCollectGaku, money,>
                       ,0   -- BillingConfirmFlg
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL  
                       ,NULL
                       ,NULL                  
                       ,NULL
              );

            --�yD_BillingDetails�zINSERT Table�]���d�l�b
            INSERT INTO [D_BillingDetails]
                   ([BillingNO]
                   ,[BillingType]	--2019.10.23 add
                   ,[BillingRows]
                   ,[StoreCD]
                   ,[BillingCloseDate]
                   ,[CustomerCD]
                   ,[SalesNO]
                   ,[SalesRows]
                   ,[CollectPlanNO]
                   ,[CollectPlanRows]
                   ,[BillingHontaiGaku]
                   ,[BillingTax]
                   ,[BillingGaku]
                   ,[TaxRitsu]
                   ,[InvoiceFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
             SELECT
                   @BillingNO
                   ,2	--[BillingType]	2019.10.23 add
                   ,ROW_NUMBER() OVER(ORDER BY DS.SalesDate, DS.CustomerCD, DM.SalesNO, DM.SalesRows) AS BillingRows
                   ,@StoreCD
                   ,CONVERT(date, @ChangeDate) AS BillingCloseDate
                   ,DC.CustomerCD
                   ,DM.SalesNO
                   ,ISNULL(DM.SalesRows,0)
                   ,ISNULL(DM.CollectPlanNO,0)
                   ,ISNULL(DM.CollectPlanRows,0)
                   ,ISNULL(DM.HontaiGaku,0)
                   ,ISNULL(DM.Tax,0)
                   ,ISNULL(DM.CollectPlanGaku,0)
                   ,ISNULL(DM.TaxRitsu,0)
                   ,ISNULL(DM.BillingPrintFLG,0)
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL  
                   ,NULL
                   ,NULL                  
                   ,NULL
                   
                --�J�[�\��AAA�̏����Ɠ���
                FROM D_CollectPlan AS DC
                INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                    FROM M_Customer AS MC 
                    WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                    AND MC.BillingCloseDate = @BillingCloseDate
                    AND MC.DeleteFlg = 0
                    GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DC.CustomerCD
                
                LEFT OUTER JOIN D_CollectPlanDetails AS DM
                ON DM.CollectPlanNO = DC.CollectPlanNO
                AND DM.DeleteDateTime IS Null
                
                LEFT OUTER JOIN D_Sales AS DS
                ON DS.SalesNO = DM.SalesNO
                AND DS.DeleteDateTime IS Null

                WHERE --DC.BillingNO IS Null       
                DC.MonthlyBillingNO IS Null       
                AND DC.DeleteOperator IS Null       
                AND DC.DeleteDateTime IS Null       
                AND DC.StoreCD = @StoreCD
                AND DC.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DC.CustomerCD END)
                --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			    --AND DC.BillingDate <= CONVERT(date, @ChangeDate)   
			    AND (DC.BillingDate IS NULL OR (DC.BillingDate IS NOT NULL AND DC.BillingDate <= CONVERT(date, @ChangeDate)))
			    --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���   
                AND DC.InvalidFLG = 0
                AND DC.BillingConfirmFlg = 0
                AND DC.BillingType = 2
                
                AND DC.CustomerCD = @W_COL1
                ORDER BY DS.SalesDate, DS.CustomerCD, DM.SalesNO, DM.SalesRows                
                ;
                
            --�yD_CollectPlan�zUPDATE Table�]���d�l�c
            UPDATE [D_CollectPlan]
               SET [BillingNO] = ISNULL([BillingNO], @BillingNO)
               	  ,[MonthlyBillingNO] = @BillingNO	--2019.10.23 add
                  ,[BillingCloseDate] = CONVERT(date, @ChangeDate)
                  ,[FirstCollectPlanDate] = @W_COL2	--2020.01.05 add
                  ,[NextCollectPlanDate] = @W_COL2	--2020.01.05 add
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                
                FROM D_CollectPlan
                INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                    FROM M_Customer AS MC 
                    WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                    AND MC.BillingCloseDate = @BillingCloseDate
                    AND MC.DeleteFlg = 0
                    GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = D_CollectPlan.CustomerCD
                
                WHERE --D_CollectPlan.BillingNO IS Null		2019.10.23 chg
                D_CollectPlan.MonthlyBillingNO IS NULL
                AND D_CollectPlan.DeleteOperator IS Null
                AND D_CollectPlan.DeleteDateTime IS Null
                AND D_CollectPlan.StoreCD = @StoreCD
                AND D_CollectPlan.CustomerCD = @W_COL1       --�� 
				--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			    --AND D_CollectPlan.BillingDate <= CONVERT(date, @ChangeDate)   
			    AND (D_CollectPlan.BillingDate IS NULL OR (D_CollectPlan.BillingDate IS NOT NULL AND D_CollectPlan.BillingDate <= CONVERT(date, @ChangeDate)))
			    --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���   

                AND D_CollectPlan.InvalidFLG = 0
                AND D_CollectPlan.BillingConfirmFlg = 0
                AND D_CollectPlan.BillingType = 2
                ;

            --���̂Ƃ��A����Œ����z8��0�̏ꍇ
            --���̂Ƃ��A����Œ����z10��0�̏ꍇ
            IF @AdjustTax8 <> 0 OR @AdjustTax10 <> 0
            BEGIN
            	DECLARE @CollectPlanNO varchar(10);
            	SET @CollectPlanNO = (SeleCT top 1 DC.CollectPlanNO
                                        FROM D_CollectPlan AS DC
                                        WHERE MonthlyBillingNO = @BillingNO
                                        AND (@AdjustTax8 = 0 OR (@AdjustTax8 <> 0 AND DC.Tax8 + @AdjustTax8 >= 0))
                                        AND (@AdjustTax10 = 0 OR (@AdjustTax10 <> 0 AND DC.Tax10 + @AdjustTax10 >= 0))
                                        );
                    
                --�e�[�u���]���d�l�d    �����z�̏���Ŋz������\��f�[�^�Œ�������
                UPDATE D_CollectPlan SET
                   [Tax]            = [Tax] + @AdjustTax8 + @AdjustTax10
                  ,[Tax8]           = [Tax8] + @AdjustTax8
                  ,[Tax10]          = [Tax10] + @AdjustTax10
                  ,[CollectPlanGaku]= [CollectPlanGaku] + @AdjustTax8 + @AdjustTax10
                  ,[AdjustTax8]     = @AdjustTax8
                  ,[AdjustTax10]    = @AdjustTax10
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                WHERE MonthlyBillingNO = @BillingNO
                AND CollectPlanNO = @CollectPlanNO
                ;
                
                DECLARE @CollectPlanRows int;
                SET @CollectPlanRows = (SELECT MIN(DM.CollectPlanRows) 
                			FROM D_CollectPlanDetails AS DM
                            WHERE DM.CollectPlanNO = @CollectPlanNO
                            AND DM.Tax + @AdjustTax8 + @AdjustTax10 >= 0
                            AND DM.DeleteDateTime IS NULL
                            );
                            
                --�e�[�u���]���d�l�e    ������������\�薾�ׂ𒲐�����
                UPDATE D_CollectPlanDetails SET
                   [Tax]            = [Tax] + @AdjustTax8 + @AdjustTax10
                  ,[CollectPlanGaku]= [CollectPlanGaku] + @AdjustTax8 + @AdjustTax10
                  ,[AdjustTax]      = @AdjustTax8 + @AdjustTax10
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                WHERE CollectPlanNO = @CollectPlanNO
                AND CollectPlanRows = @CollectPlanRows
                ;
                
                --�e�[�u���]���d�l�f    ���������������ׂ𒲐�����
                UPDATE D_BillingDetails SET
                   [BillingTax] = [BillingTax] + @AdjustTax8 + @AdjustTax10
                  ,[BillingGaku] = [BillingGaku] + @AdjustTax8 + @AdjustTax10
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                WHERE BillingNO = @BillingNO
                AND DeleteDateTime IS NULL
                AND CollectPlanNO = @CollectPlanNO
                AND CollectPlanRows = @CollectPlanRows
                ;

            END
            
            -- ========= ���[�v���̎��ۂ̏��� �����܂�===

            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_AAA
            INTO @W_COL1
                ,@W_COL2
                ,@HontaiGaku        
                ,@HontaiGaku0
                ,@HontaiGaku8
                ,@HontaiGaku10
                ,@Tax
                ,@Tax8
                ,@Tax10
                ,@CollectPlanGaku
                ,@AdjustTax8
                ,@AdjustTax10
                ;

        END

        --�J�[�\�������
        CLOSE CUR_AAA;
        DEALLOCATE CUR_AAA;
        
    END
    
    --��������ݾ�--
    ELSE 
    BEGIN
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            27,             --in�`�[��� 27
            @ChangeDate,    --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @ProcessingNO OUTPUT
            ;
        
        IF @Syori = 2
        BEGIN
            --�yD_BillingProcessing�zInsert Table�]���d�l�`�A
            INSERT INTO [D_BillingProcessing]
                       ([ProcessingNO]
                       ,[StoreCD]
                       ,[BillingDate]
                       ,[CustomerCD]
                       ,[ProcessingKBN]
                       ,[ProcessingDateTime]
                       ,[StaffCD]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 VALUES( 
                        @ProcessingNO
                       ,@StoreCD
                       ,CONVERT(date, @ChangeDate)
                       ,@CustomerCD     --@W_COL1
                       ,2               -- ProcessingKBN
                       ,@SYSDATETIME    --ProcessingDateTime
                       ,@Operator
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL 
                       ,NULL
                       ,NULL                  
                       ,NULL
                );
        END
        --�����m��--
        ELSE IF @Syori = 3
        BEGIN
            --�yD_BillingProcessing�zInsert	Table�]���d�l�`�B
            INSERT INTO [D_BillingProcessing]
                       ([ProcessingNO]
                       ,[StoreCD]
                       ,[BillingDate]
                       ,[CustomerCD]
                       ,[ProcessingKBN]
                       ,[ProcessingDateTime]
                       ,[StaffCD]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime]
                       ,[DeleteOperator]
                       ,[DeleteDateTime])
                 VALUES( 
                        @ProcessingNO
                       ,@StoreCD
                       ,CONVERT(date, @ChangeDate)
                       ,@CustomerCD     --@W_COL1
                       ,3               -- ProcessingKBN
                       ,@SYSDATETIME    --ProcessingDateTime
                       ,@Operator
                       ,@Operator  
                       ,@SYSDATETIME
                       ,NULL 
                       ,NULL
                       ,NULL                  
                       ,NULL
                );
        
        END
        
/*
        --�J�[�\����`
        DECLARE CUR_AAA2 CURSOR FOR
        	SELECT DB.BillingCustomerCD
                FROM D_Billing AS DB
                INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                    FROM M_Customer AS MC 
                    WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                    AND MC.BillingCloseDate = @BillingCloseDate
                    AND MC.DeleteFlg = 0
                    GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
              WHERE DB.StoreCD = @StoreCD
                AND DB.BillingCustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
                AND DB.BillingCloseDate = CONVERT(date, @ChangeDate)
                AND DB.DeleteDateTime IS Null
                GROUP BY DB.BillingCustomerCD
                ORDER BY DB.BillingCustomerCD
                ;
                
        --�J�[�\���I�[�v��
        OPEN CUR_AAA2;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_AAA2
        INTO @W_COL1
            ;

        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN

    -- ========= ���[�v���̎��ۂ̏��� ��������===
        
    
            -- ========= ���[�v���̎��ۂ̏��� �����܂�===

            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_AAA2
            INTO @W_COL1
                ;
        END
        
        --�J�[�\�������
        CLOSE CUR_AAA2;
        DEALLOCATE CUR_AAA2;
*/
        IF @Syori = 2
        BEGIN
            --�yD_BillingDetails�zUpdate Table�]���d�l�b�A
            UPDATE [D_BillingDetails]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            FROM D_BillingDetails
            INNER JOIN D_Billing AS DB ON D_BillingDetails.BillingNO = DB.BillingNO
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            --AND DB.BillingNO IS NOT NULL		--��
            AND DB.BillingCustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingCloseDate = CONVERT(date, @ChangeDate)
            AND DB.DeleteDateTime IS Null
            AND DB.BillingNO IS NOT NULL
            AND DB.BillingConfirmFlg = 0
            ;
             
            --�yD_Billing�zUpdate Table�]���d�l�a�A
            UPDATE [D_Billing]
               SET [DeleteOperator] = @Operator
                  ,[DeleteDateTime] = @SYSDATETIME
            FROM D_Billing AS DB
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            AND DB.BillingCustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingCloseDate = CONVERT(date, @ChangeDate)
            AND DB.DeleteDateTime IS Null
            AND DB.BillingNO IS NOT NULL
            AND DB.BillingConfirmFlg = 0
            ;
            
            --�yD_CollectPlanDetails�zUpdate�@Table�]���d�l�e�A
            UPDATE D_CollectPlanDetails SET
               [Tax]            = D_CollectPlanDetails.Tax - D_CollectPlanDetails.AdjustTax
              ,[CollectPlanGaku]= D_CollectPlanDetails.CollectPlanGaku - D_CollectPlanDetails.AdjustTax
              ,[AdjustTax]      = 0
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
            FROM D_CollectPlan
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = D_CollectPlan.CustomerCD
            
            WHERE D_CollectPlan.MonthlyBillingNO IS NOT NULL
            AND D_CollectPlan.DeleteOperator IS Null
            AND D_CollectPlan.DeleteDateTime IS Null
            AND D_CollectPlan.StoreCD = @StoreCD
            AND D_CollectPlan.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE D_CollectPlan.CustomerCD END) 
            and D_CollectPlan.BillingCloseDate = CONVERT(date, @ChangeDate)   
            AND D_CollectPlan.InvalidFLG = 0
            AND D_CollectPlan.BillingConfirmFlg = 0
            AND D_CollectPlan.BillingType = 2
            AND D_CollectPlan.CollectPlanNO = D_CollectPlanDetails.CollectPlanNO
            AND D_CollectPlanDetails.DeleteDateTime IS Null
            ;
            
            --�yD_CollectPlan�zUpdate Table�]���d�l�c�A
            --�yD_CollectPlan�zUpdate�@Table�]���d�l�d�A
            UPDATE [D_CollectPlan]
               SET [BillingNO] = (CASE WHEN BillingNO = MonthlyBillingNO THEN NULL
               						ELSE BillingNO END)
                  ,[MonthlyBillingNO] = NULL
                  ,[BillingCloseDate] = NULL
	              ,[Tax]            = [Tax] - [AdjustTax8] - [AdjustTax10]
	              ,[Tax8]           = [Tax8] - [AdjustTax8]
	              ,[Tax10]          = [Tax10] - [AdjustTax10]
	              ,[CollectPlanGaku]= [CollectPlanGaku] - [AdjustTax8] - [AdjustTax10]
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                
            FROM D_CollectPlan
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = D_CollectPlan.CustomerCD
            
            WHERE --D_CollectPlan.BillingNO IS NOT Null		2019.10.23 chg
            D_CollectPlan.MonthlyBillingNO IS NOT NULL
            AND D_CollectPlan.DeleteOperator IS Null
            AND D_CollectPlan.DeleteDateTime IS Null
            AND D_CollectPlan.StoreCD = @StoreCD
            AND D_CollectPlan.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE D_CollectPlan.CustomerCD END)
--            AND BillingDate <= CONVERT(date, @ChangeDate)   
            and D_CollectPlan.BillingCloseDate = CONVERT(date, @ChangeDate)   
            AND D_CollectPlan.InvalidFLG = 0
            AND D_CollectPlan.BillingConfirmFlg = 0
            AND D_CollectPlan.BillingType = 2
            ;

        END
        
        --�����m��--
        ELSE IF @Syori = 3
        BEGIN
	    	
            --�yD_Billing�zUpdate   Table�]���d�l�a�B
            UPDATE [D_Billing]
               SET [BillingConfirmFlg] = 1  --�����m��flg
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
            FROM D_Billing AS DB
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            AND DB.BillingCustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingCloseDate = CONVERT(date, @ChangeDate)
            AND DB.DeleteDateTime IS Null
            AND DB.BillingNO IS NOT NULL
            AND DB.BillingConfirmFlg = 0
            
            --�yD_CollectPlan�zUpdate   Table�]���d�l�c�B   
            UPDATE [D_CollectPlan]
               SET  [BillingConfirmFlg] = 1  --�����m��flg
                  ,[UpdateOperator] = @Operator
                  ,[UpdateDateTime] = @SYSDATETIME
                
                FROM D_CollectPlan
                INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                    FROM M_Customer AS MC 
                    WHERE MC.ChangeDate <= CONVERT(date, @ChangeDate)
                    AND MC.BillingCloseDate = @BillingCloseDate
                    AND MC.DeleteFlg = 0
                    GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = D_CollectPlan.CustomerCD
                
                WHERE --D_CollectPlan.BillingNO IS NOT Null		2019.10.23 chg
                D_CollectPlan.MonthlyBillingNO IS NULL
                AND D_CollectPlan.DeleteOperator IS Null
                AND D_CollectPlan.DeleteDateTime IS Null
                AND D_CollectPlan.StoreCD = @StoreCD
                AND D_CollectPlan.CustomerCD = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE D_CollectPlan.CustomerCD END)
    --            AND BillingDate <= CONVERT(date, @ChangeDate)   
                and D_CollectPlan.BillingCloseDate = CONVERT(date, @ChangeDate)   
                AND D_CollectPlan.InvalidFLG = 0
                AND D_CollectPlan.BillingConfirmFlg = 0
                AND D_CollectPlan.BillingType = 2
                ;
        END
    END
	
	
    --�yL_Log�zINSERT
    --���������f�[�^�֍X�V
    SET @KeyItem = CONVERT(varchar,@Syori) + ',' + @ChangeDate + ',' + CONVERT(varchar,@BillingCloseDate) + ',' + ISNULL(@CustomerCD,'');
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'SeikyuuShimeShori',
        @PC,
        @Syori,
        @KeyItem;

END

GO


