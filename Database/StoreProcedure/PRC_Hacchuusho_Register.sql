IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_Register')
begin
    DROP PROCEDURE PRC_Hacchuusho_Register
end
GO

CREATE PROCEDURE PRC_Hacchuusho_Register(
     @p_Operator        varchar(10)
    ,@p_StoreCD         varchar(4)
    ,@p_OrderCD         varchar(13)
)AS
BEGIN

    DECLARE @SYSDATETIME datetime
    DECLARE @SYSDATE date
    SET @SYSDATETIME = SYSDATETIME()
    SET @SYSDATE = CONVERT(datetime, @SYSDATETIME)

    DECLARE @OrderCD varchar(13)
    DECLARE @CancelOrderNO varchar(11)
    DECLARE @CancelOrderProcessNO varchar(11)

	--�Ώۃf�[�^�p�ꎞ�e�[�u��
	CREATE TABLE #tmpTargetKey
    (
	     JuchuuNO	varchar(11) COLLATE database_default NOT NULL
	    ,JuchuuRows	int NOT NULL
	    ,OrderSEQ	int NOT NULL
	    ,OrderNO	varchar(11) COLLATE database_default NULL
	    ,OrderRows	int
	    ,OrderCD	varchar(13) COLLATE database_default NULL
	)
    ALTER TABLE #tmpTargetKey ADD PRIMARY KEY CLUSTERED 	
    (	
	 JuchuuNO
	,JuchuuRows
	,OrderSEQ
    )	

	--�Ώۃf�[�^���ꎞ�e�[�u���Ɋi�[����
	INSERT INTO #tmpTargetKey
	SELECT    
	     DLOR.JuchuuNO
	    ,DLOR.JuchuuRows
	    ,DLOR.OrderSEQ
	    ,DLOR.OrderNO
	    ,DLOR.OrderRows
	    ,DLOR.OrderCD
    FROM D_LastOrder DLOR
	INNER JOIN D_Order DODH
    ON DODH.OrderNO = DLOR.OrderNO
    WHERE DODH.StoreCD = @p_StoreCD
	AND DLOR.CancelOrderFLG = 1
    AND DLOR.CancelOrderDate IS NULL
    AND (@p_OrderCD IS NULL OR DLOR.OrderCD = @p_OrderCD)

    --�J�[�\����`�i�d���悲�Ɓj
    DECLARE curOrderCD CURSOR FOR
	SELECT OrderCD
    FROM #tmpTargetKey
	GROUP BY OrderCD

    --�J�[�\���I�[�v��
    OPEN curOrderCD;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM curOrderCD
    INTO @OrderCD
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN

	    --�`�[�ԍ��̔ԁi���������ԍ��j        
		--EXEC    Fnc_GetNumber    
		-- 20,              --in�`�[��� 11    
		-- @SYSDATE,        --in���    
        -- @p_StoreCD,      --in�X��CD    
        -- @p_Operator,    
        -- @CancelOrderProcessNO OUTPUT        

        --�`�[�ԍ��̔ԁi�����ԍ��j            
        EXEC Fnc_GetNumber    
            11,             --in�`�[��� 11    
            @SYSDATE,       --in���    
            @p_StoreCD,     --in�X��CD    
            @p_Operator,
            @CancelOrderNO OUTPUT

		--�������׍X�V    
        INSERT INTO D_OrderDetails(
             OrderNO
            ,OrderRows
            ,DisplayRows
            ,JuchuuNO
            ,JuchuuRows
            ,SKUCD
            ,AdminNO
            ,JanCD
            --,MakerItem
            ,ItemName
            ,ColorName
            ,SizeName
            ,Remarks
            ,OrderSu
            ,TaniCD
            ,PriceOutTax
            ,Rate
            ,OrderUnitPrice
            ,OrderHontaiGaku
            ,OrderTax
            ,OrderTaxRitsu
            ,OrderGaku
            ,SoukoCD
            ,DirectFLG
            ,NotNetFLG
            ,EDIFLG
            ,DesiredDeliveryDate
            ,ArrivePlanDate
            ,TotalArrivalSu
            ,CommentOutStore
            ,CommentInStore
            ,FirstOrderNO
            ,FirstOrderRows
            ,CancelOrderNO
            ,AnswerFLG
            ,EDIOutputDatetime
            ,InsertOperator
            ,InsertDateTime
            ,UpdateOperator
            ,UpdateDateTime
            ,DeleteOperator
            ,DeleteDateTime
				)        
		SELECT  
             OrderNO                = @CancelOrderNO
            ,OrderRows              = ROW_NUMBER() OVER(PARTITION BY DLOR.OrderCD ORDER BY DLOR.OrderNO, DLOR.OrderRows)        
            ,DisplayRows            = ROW_NUMBER() OVER(PARTITION BY DLOR.OrderCD ORDER BY DLOR.OrderNO, DLOR.OrderRows)
            ,JuchuuNO               = DLOR.JuchuuNO
            ,JuchuuRows             = DLOR.JuchuuRows
            ,SKUCD                  = DODD.SKUCD
            ,AdminNO                = DODD.AdminNO
            ,JanCD                  = DODD.JanCD
            --,MakerItem
            ,ItemName               = DODD.ItemName
            ,ColorName              = DODD.ColorName
            ,SizeName               = DODD.SizeName
            ,Remarks                = DODD.Remarks
            ,OrderSu                = DODD.OrderSu
            ,TaniCD                 = DODD.TaniCD
            ,PriceOutTax            = DODD.PriceOutTax
            ,Rate                   = DODD.Rate
            ,OrderUnitPrice         = DODD.OrderUnitPrice
            ,OrderHontaiGaku        = DODD.OrderHontaiGaku
            ,OrderTax               = DODD.OrderTax
            ,OrderTaxRitsu          = DODD.OrderTaxRitsu
            ,OrderGaku              = DODD.OrderGaku
            ,SoukoCD                = DODD.SoukoCD
            ,DirectFLG              = 0
            ,NotNetFLG              = 0
            ,EDIFLG                 = 0
            ,DesiredDeliveryDate    = DODD.DesiredDeliveryDate        
            ,ArrivePlanDate         = null
            ,TotalArrivalSu         = 0
            ,CommentOutStore        = null
            ,CommentInStore         = null
            ,FirstOrderNO           = DLOR.OrderNO
            ,FirstOrderRows         = DLOR.OrderRows
            ,CancelOrderNO          = null
            ,AnswerFLG              = 0
            ,EDIOutputDatetime      = null
            ,InsertOperator         = @p_Operator
            ,InsertDateTime         = @SYSDATETIME
            ,UpdateOperator         = @p_Operator
            ,UpdateDateTime         = @SYSDATETIME
            ,DeleteOperator         = null
            ,DeleteDateTime         = null
		FROM D_LastOrder DLOR
		
		INNER JOIN #tmpTargetKey tmp
		ON  tmp.JuchuuNO    = DLOR.JuchuuNO
		AND tmp.JuchuuRows  = DLOR.JuchuuRows
		AND tmp.OrderSEQ    = DLOR.OrderSEQ

		INNER JOIN D_OrderDetails DODD
		ON  DODD.OrderNO    = DLOR.OrderNO
		AND DODD.OrderRows  = DLOR.OrderRows

		WHERE tmp.OrderCD   = @OrderCD

		--�����w�b�_�X�V        
		INSERT INTO D_Order(        
             OrderNO
            ,OrderProcessNO
            ,StoreCD
            ,OrderDate
            ,ReturnFLG
            ,OrderDataKBN
            ,OrderWayKBN
            ,OrderCD
            ,OrderPerson
            ,AliasKBN
            ,DestinationKBN
            ,DestinationName
            ,DestinationZip1CD
            ,DestinationZip2CD
            ,DestinationAddress1
            ,DestinationAddress2
            ,DestinationTelphoneNO
            ,DestinationFaxNO
            ,DestinationSoukoCD
            ,CurrencyCD
            ,OrderHontaiGaku
            ,OrderTax8
            ,OrderTax10
            ,OrderGaku
            ,CommentOutStore
            ,CommentInStore
            ,StaffCD
            ,FirstArriveDate
            ,LastArriveDate
            ,ApprovalDate
            ,LastApprovalDate
            ,LastApprovalStaffCD
            ,ApprovalStageFLG
            ,FirstPrintDate
            ,LastPrintDate
            ,InsertOperator
            ,InsertDateTime
            ,UpdateOperator
            ,UpdateDateTime
            ,DeleteOperator
            ,DeleteDateTime             
				)        
		SELECT
             OrderNO                = @CancelOrderNO        
            ,OrderProcessNO         = null --@CancelOrderProcessNO        
            ,StoreCD                = @p_StoreCD        
            ,OrderDate              = @SYSDATE    
            ,ReturnFLG              = 1
            ,OrderDataKBN           = 3
            ,OrderWayKBN            = 3
            ,OrderCD                = @OrderCD    
            ,OrderPerson            = NULL        
            ,AliasKBN               = 0
            ,DestinationKBN         = 1
            ,DestinationName        = null
            ,DestinationZip1CD      = null
            ,DestinationZip2CD      = null
            ,DestinationAddress1    = null
            ,DestinationAddress2    = null
            ,DestinationTelphoneNO  = null        
            ,DestinationFaxNO       = null
            ,DestinationSoukoCD     = null
            ,CurrencyCD             = MCON.CurrencyCD        
            ,OrderHontaiGaku        = SUB_DODD.SumOrderHontaiGaku
            ,OrderTax8              = SUB_DODD.SumOrderTax8        
            ,OrderTax10             = SUB_DODD.SumOrderTax10        
            ,OrderGaku              = SUB_DODD.SumOrderGaku        
            ,CommentOutStore        = null
            ,CommentInStore         = null
            ,StaffCD                = null
            ,FirstArriveDate        = null
            ,LastArriveDate         = null
            ,ApprovalDate           = @SYSDATE
            ,LastApprovalDate       = @SYSDATE
            ,LastApprovalStaffCD    = @p_Operator        
            ,ApprovalStageFLG       = 10
            ,FirstPrintDate         = null
            ,LastPrintDate          = null
            ,InsertOperator         = @p_Operator
            ,InsertDateTime         = @SYSDATETIME    
            ,UpdateOperator         = @p_Operator
            ,UpdateDateTime         = @SYSDATETIME    
            ,DeleteOperator         = null
            ,DeleteDateTime         = null
		FROM (SELECT SUM(OrderHontaiGaku) SumOrderHontaiGaku
					,SUM(OrderGaku) SumOrderGaku
					,SUM(CASE WHEN DODD.OrderTaxRitsu = 2 THEN OrderTax ELSE 0 END) SumOrderTax8
					,SUM(CASE WHEN DODD.OrderTaxRitsu = 1 THEN OrderTax ELSE 0 END) SumOrderTax10
				FROM D_OrderDetails DODD
				INNER JOIN #tmpTargetKey tmp
				ON  tmp.OrderNO = DODD.OrderNO
				AND tmp.OrderRows = DODD.OrderRows
				AND tmp.OrderCD = @OrderCD) SUB_DODD

		INNER JOIN M_Control MCON
		ON MCON.MainKey = 1

		--���̔������ׂɃL�����Z�������ԍ����X�V
		UPDATE DODD
		SET CancelOrderNO   = @CancelOrderNO
            ,UpdateOperator = @p_Operator
            ,UpdateDateTime = @SYSDATETIME
		FROM D_OrderDetails DODD		
		CROSS APPLY(
            SELECT TOP 1 *    
            FROM #tmpTargetKey tmp
	        WHERE tmp.OrderNO = DODD.OrderNO
            AND tmp.OrderRows = DODD.OrderRows
            AND tmp.OrderCD = @OrderCD) SUB_tmp

		--�L�����Z���f�[�^�쐬�����X�V
		UPDATE DLOR
		SET CancelOrderDate = @SYSDATE
			,UpdateOperator = @p_Operator
			,UpdateDateTime = @SYSDATETIME
		FROM D_LastOrder DLOR
		INNER JOIN #tmpTargetKey tmp
		ON tmp.JuchuuNO = DLOR.JuchuuNO
		AND tmp.JuchuuRows = DLOR.JuchuuRows
		AND tmp.OrderSEQ = DLOR.OrderSEQ

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM curOrderCD
    	INTO @OrderCD

    END
    
    --�J�[�\�������
    CLOSE curOrderCD;
	DEALLOCATE curOrderCD;

	--�ꎞ�e�[�u���폜
	DROP TABLE #tmpTargetKey

END
GO

