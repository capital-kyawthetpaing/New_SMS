DROP  PROCEDURE [dbo].[D_TenzikaiJuchuu_SelectAll]
GO
DROP  PROCEDURE [dbo].[CheckTenzikaiJuchuu]
GO

DROP  PROCEDURE [dbo].[PRC_TenzikaiJuchuuJouhouHurikaeShori]
GO


--  ======================================================================
--       Program Call    �W����󒍏��U�֏���
--       Program ID      TenzikaiJuchuuJouhouHurikaeShori
--       Create date:    2020.9.10
--    ======================================================================
CREATE PROCEDURE D_TenzikaiJuchuu_SelectAll
    (
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.TenzikaiJuchuuNO
    FROM D_TenzikaiJuchuu AS A
    LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS B
    ON B.TenzikaiJuchuuNO = A.TenzikaiJuchuuNO
    AND B.DeleteDateTime IS NULL
    WHERE A.DeleteDateTime IS NULL
    --�ꎞ���[�N�e�[�u���uM_SKU�@�v(�ꎞ���[�N�e�[�u���uD_TaishougaiJuchuu�@�v�A�e�[�u���]���d�l�a�ŁuM_SKU�@�v�Ƃ��Ďg�p)	
    AND NOT EXISTS(SELECT MT.JanCD AS TenzikaiShouhinJanCD
                         ,F.AdminNO
                         ,F.SKUCD
                         ,F.JanCD
                         ,F.SKUName
                         ,F.ColorName
                         ,F.SizeName
                   FROM F_SKU(convert(date,SYSDATETIME())) AS F
                   --�ꎞ���[�N�e�[�u���uM_TenzikaiShouhin�@�v(�ꎞ���[�N�e�[�u���uM_SKU�@�v�ŁuM_TenzikaiShouhin�@�v�Ƃ��Ďg�p)
                   INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                     ,MAX(MTS.JanCD) AS JanCD 
                               FROM M_TenzikaiShouhin AS MTS
                               WHERE MTS.DeleteFLG = 0
                               AND MTS.VendorCD = @VendorCD
                               AND MTS.LastYearTerm = @LastYearTerm
                               AND MTS.LastSeason = @LastSeason
                               GROUP BY MTS.ExhibitionCommonCD
                               ) AS MT 
                   ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                   WHERE F.DeleteFlg = 0
                   AND MT.JanCD = B.JanCD)
    AND A.JuchuuHurikaeZumiFLG = 0
    AND A.VendorCD = @VendorCD
    AND A.LastYearTerm = @LastYearTerm
    AND A.LastSeason = @LastSeason
    ORDER BY A.TenzikaiJuchuuNO
    ;

END

GO

CREATE PROCEDURE CheckTenzikaiJuchuu
    (    
    @TorokuDate     varchar(10),
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6)
    )AS
    
--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ERRNO varchar(4);
    DECLARE @CNT int;
    DECLARE @JuchuuNO varchar(11);
    
    SET @ERRNO = '';
    
        --�Y���̓W����󒍔ԍ����擾(Obtain the relevant "�W����󒍔ԍ�")
        --(Table_TenzikaiJuchuuNO)
/*        SELECT DT.TenzikaiJuchuuNO                                                                                 
        From D_TenzikaiJuchuu AS DT                                                                                 
        Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
        AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
        AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
        AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
        ;
        */
        
	--�J�[�\����`
    DECLARE CUR_Juchu CURSOR FOR
        --�Y���̓W����󒍔ԍ��ɕR�Â��󒍔ԍ����擾(Obtain "�󒍔ԍ�" associated with the corresponding "�W����󒍔ԍ�")
        --(Table_JuchuuNO)
        SELECT A.JuchuuNO
        From D_Juchuu AS A
        --�Y���̓W����󒍔ԍ����擾(Obtain the relevant "�W����󒍔ԍ�")
        --(Table_TenzikaiJuchuuNO)
        Inner Join (SELECT DT.TenzikaiJuchuuNO                                                                                 
                    From D_TenzikaiJuchuu AS DT                                                                                 
                    Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
                    AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
                    AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
                    AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
        			And DT.DeleteDateTime IS NULL
        )	--Table_TenzikaiJuchuuNO 
        AS B
        On A.TenzikaiJuchuuNO = B.TenzikaiJuchuuNO
        Where A.DeleteDateTime IS NULL
        ;

    --�J�[�\���I�[�v��
    OPEN CUR_Juchu;

    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR_Juchu
    INTO @JuchuuNO;
    
    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
    -- ========= ���[�v���̎��ۂ̏��� ��������===

        --���ɔ���ς݌x��
        --�ȉ��̏����Ń��R�[�h������Δ���ςƂ��Čx�����b�Z�[�W��\������
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_SalesDetails A
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E168';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        --���ɏo�׍ς݌x��
        --�ȉ��̏����Ń��R�[�h������Ώo�׍ςƂ��Čx�����b�Z�[�W��\������
        SELECT @CNT = COUNT(A.Number)
        FROM D_ShippingDetails A
        WHERE A.Number = @JuchuuNO
        AND A.ShippingKBN = 1
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E167';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --���Ƀs�b�L���O���X�g�����ς݌x��
        --�ȉ��̏����Ń��R�[�h������΃s�b�L���O�ςƂ��ăG���[���b�Z�[�W��\������
        SELECT @CNT = COUNT(A.Number)
        FROM D_PickingDetails B
        INNER JOIN D_Reserve A ON A.ReserveNO = B.ReserveNO
        WHERE A.Number = @JuchuuNO
        AND A.ReserveKBN = 1
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND B.PickingDoneDateTime IS NOT NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E161';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        --���Ɏd���ς݌x��
        --�ȉ��̏����Ń��R�[�h������Δ����ςƂ��Čx�����b�Z�[�W��\������
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_PurchaseDetails D
        INNER JOIN D_ArrivalDetails C ON C.ArrivalNO = D.ArrivalNO
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        AND D.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E164';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --���ɓ��׍ς݌x��
        --�ȉ��̏����Ń��R�[�h������Δ����ςƂ��Čx�����b�Z�[�W��\������
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_ArrivalDetails C
        INNER JOIN D_ArrivalPlan B ON B.ArrivalPlanNO = C.ArrivalPlanNO
        INNER JOIN D_OrderDetails A ON A.OrderNO = B.Number AND A.OrderRows = B.NumberRows
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        AND B.DeleteDateTime IS NULL
        AND C.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E163';
            SELECT @ERRNO AS errno;
            RETURN;
        END;

        --���ɔ����ς݌x��
        --�ȉ��̏����Ń��R�[�h������Δ����ςƂ��Čx�����b�Z�[�W��\������
        SELECT @CNT = COUNT(A.JuchuuNO)
        FROM D_OrderDetails A 
        WHERE A.JuchuuNO = @JuchuuNO
        AND A.DeleteDateTime IS NULL
        ;

        IF @CNT > 0 
        BEGIN
            SET @ERRNO = 'E162';
            SELECT @ERRNO AS errno;
            RETURN;
        END;
        
        -- ========= ���[�v���̎��ۂ̏��� �����܂�===
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO;

    END
    
    --�J�[�\�������
    CLOSE CUR_Juchu;
    DEALLOCATE CUR_Juchu;
    
    SELECT @ERRNO AS errno;

END

GO

CREATE PROCEDURE PRC_TenzikaiJuchuuJouhouHurikaeShori
   (@OperateMode    int,         -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    
    @TorokuDate     varchar(10),
    @VendorCD       varchar(13),
    @LastYearTerm   varchar(6),
    @LastSeason     varchar(6),

    @Operator  varchar(10),
    @PC  varchar(30)
--    @OutJuchuuProcessNO varchar(11) OUTPUT
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
    DECLARE @DeliveryPlanNO  varchar(11);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    
    DECLARE @JuchuuProcessNO     varchar(11);
    DECLARE @JuchuuNO            varchar(11);
    DECLARE @StoreCD             varchar(4);
    DECLARE @TenzikaiJuchuuNO    varchar(11);
    DECLARE @JuchuuDate          date;
    DECLARE @varJuchuuDate       varchar(10);
    DECLARE @TenzikaiJuchuuRows  int;
    DECLARE @oldTenzikaiJuchuuNO varchar(11);
    DECLARE @DisplayRows         int;
    
    SET @StoreCD = '0000';
    SET @oldTenzikaiJuchuuNO = '';
    
    --�J�[�\����`
    DECLARE CUR_Tenzi CURSOR FOR
    	--�ꎞ���[�N�e�[�u���uD_TenzikaiJuchuu�@�v(�e�[�u���]���d�l�`�ŁuD_TenzikaiJuchuu�@�v�Ƃ��Ďg�p)
        SELECT
             DT.TenzikaiJuchuuNO    
            ,MAX(DT.JuchuuDate) OVER(PARTITION BY DT.TenzikaiJuchuuNO)       AS JuchuuDate
            ,DM.TenzikaiJuchuuRows
        FROM D_TenzikaiJuchuu AS DT
        LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS DM
        ON DM.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
        AND DM.DeleteDateTime IS NULL
        WHERE DT.DeleteDateTime IS NULL
        AND DT.JuchuuHurikaeZumiFLG = 0
        AND DT.VendorCD = @VendorCD
        AND DT.LastYearTerm = @LastYearTerm
        AND DT.LastSeason = @LastSeason
        --�ꎞ���[�N�e�[�u���uD_TaishougaiJuchuu�@�v(�ꎞ���[�N�e�[�u���uD_TenzikaiJuchuu�@�v�A��ʓ]���\01�ŁuD_TaishougaiJuchuu�@�v�Ƃ��Ďg�p)
        AND NOT EXISTS(SELECT 1 
                FROM D_TenzikaiJuchuu AS A
                LEFT OUTER JOIN D_TenzikaiJuchuuDetails AS B
                ON B.TenzikaiJuchuuNO = A.TenzikaiJuchuuNO
                AND B.DeleteDateTime IS NULL
                WHERE A.DeleteDateTime IS NULL
                --�ꎞ���[�N�e�[�u���uM_SKU�@�v(�ꎞ���[�N�e�[�u���uD_TaishougaiJuchuu�@�v�A�e�[�u���]���d�l�a�ŁuM_SKU�@�v�Ƃ��Ďg�p)	
                AND NOT EXISTS(SELECT MT.JanCD AS TenzikaiShouhinJanCD
                                     ,F.AdminNO
                                     ,F.SKUCD
                                     ,F.JanCD
                                     ,F.SKUName
                                     ,F.ColorName
                                     ,F.SizeName
                               FROM F_SKU(convert(date,@SYSDATETIME)) AS F
                               --�ꎞ���[�N�e�[�u���uM_TenzikaiShouhin�@�v(�ꎞ���[�N�e�[�u���uM_SKU�@�v�ŁuM_TenzikaiShouhin�@�v�Ƃ��Ďg�p)
                               INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                                 ,MAX(MTS.JanCD) AS JanCD 
                                           FROM M_TenzikaiShouhin AS MTS
                                           WHERE MTS.DeleteFLG = 0
                                           AND MTS.VendorCD = @VendorCD
                                           AND MTS.LastYearTerm = @LastYearTerm
                                           AND MTS.LastSeason = @LastSeason
                                           GROUP BY MTS.ExhibitionCommonCD
                                           ) AS MT 
                               ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                               WHERE F.DeleteFlg = 0
                               AND MT.JanCD = B.JanCD)
        		AND A.JuchuuHurikaeZumiFLG = 0
                AND A.VendorCD = @VendorCD
                AND A.LastYearTerm = @LastYearTerm
                AND A.LastSeason = @LastSeason
        		AND A.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
        )
        ORDER BY DT.TenzikaiJuchuuNO, DM.TenzikaiJuchuuRows
        ;

    
    --�V�K--
    IF @OperateMode = 1
    BEGIN
        --�J�[�\���I�[�v��
        OPEN CUR_Tenzi;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_Tenzi
        INTO @TenzikaiJuchuuNO, @JuchuuDate, @TenzikaiJuchuuRows;
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ���[�v���̎��ۂ̏��� ��������===
            --���W����󒍔ԍ��P�ʂō̔�
            SET @varJuchuuDate = convert(varchar, @JuchuuDate,111);
            
            IF @oldTenzikaiJuchuuNO <> @TenzikaiJuchuuNO
            BEGIN
                SET @oldTenzikaiJuchuuNO = @TenzikaiJuchuuNO;
                SET @DisplayRows = 1;
                
                --�`�[�ԍ��̔�
                EXEC Fnc_GetNumber
                    30,             --in�`�[��� 1
                    @varJuchuuDate, --in���
                    @StoreCD,       --in�X��CD
                    @Operator,
                    @JuchuuProcessNO OUTPUT
                    ;
                
                IF ISNULL(@JuchuuProcessNO,'') = ''
                BEGIN
                    SET @W_ERR = 1;
                    RETURN @W_ERR;
                END
            END
            
            --�����גP�ʂō̔�
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                1,             --in�`�[��� 1
                @varJuchuuDate, --in���
                @StoreCD,       --in�X��CD
                @Operator,
                @JuchuuNO OUTPUT
                ;
            
            IF ISNULL(@JuchuuNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
            
            --�yD_DeliveryPlan�z�z���\����@Table�]���d�lG
            --�`�[�ԍ��̔�
            EXEC Fnc_GetNumber
                19,             --in�`�[��� 19
                @varJuchuuDate,      --in���
                @StoreCD,       --in�X��CD
                @Operator,
                @DeliveryPlanNO OUTPUT
                ;
            
            IF ISNULL(@DeliveryPlanNO,'') = ''
            BEGIN
                SET @W_ERR = 1;
                RETURN @W_ERR;
            END
                    
            --�yD_Juchuu�z
            INSERT INTO [D_Juchuu]
               ([JuchuuNO]
               ,[JuchuuProcessNO]
               ,[StoreCD]
               ,[JuchuuDate]
               ,[JuchuuTime]
               ,[ReturnFLG]
               ,[SoukoCD]
               ,[JuchuuKBN]
               ,[SiteKBN]
               ,[SiteJuchuuDateTime]
               ,[SiteJuchuuNO]
               ,[InportErrFLG]
               ,[OnHoldFLG]
               ,[IdentificationFLG]
               ,[TorikomiDateTime]
               ,[StaffCD]
               ,[CustomerCD]
               ,[CustomerName]
               ,[CustomerName2]
               ,[AliasKBN]
               ,[ZipCD1]
               ,[ZipCD2]
               ,[Address1]
               ,[Address2]
               ,[Tel11]
               ,[Tel12]
               ,[Tel13]
               ,[Tel21]
               ,[Tel22]
               ,[Tel23]
               ,[CustomerKanaName]
               ,[DeliveryCD]
               ,[DeliveryName]
               ,[DeliveryName2]
               ,[DeliveryAliasKBN]
               ,[DeliveryZipCD1]
               ,[DeliveryZipCD2]
               ,[DeliveryAddress1]
               ,[DeliveryAddress2]
               ,[DeliveryTel11]
               ,[DeliveryTel12]
               ,[DeliveryTel13]
               ,[DeliveryTel21]
               ,[DeliveryTel22]
               ,[DeliveryTel23]
               ,[DeliveryKanaName]
               ,[JuchuuCarrierCD]
               ,[DecidedCarrierFLG]
               ,[LastCarrierCD]
               ,[NameSortingDateTime]
               ,[NameSortingStaffCD]
               ,[CurrencyCD]
               ,[JuchuuGaku]
               ,[Discount]
               ,[HanbaiHontaiGaku]
               ,[HanbaiTax8]
               ,[HanbaiTax10]
               ,[HanbaiGaku]
               ,[CostGaku]
               ,[ProfitGaku]
               ,[Coupon]
               --,[Point]
               ,[PayCharge]
               ,[Adjustments]
               ,[Postage]
               ,[GiftWrapCharge]
               ,[InvoiceGaku]
               ,[PaymentMethodCD]
               ,[PaymentPlanNO]
               ,[CardProgressKBN]
               ,[CardCompany]
               ,[CardNumber]
               ,[PaymentProgressKBN]
               ,[PresentFLG]
               ,[SalesPlanDate]
               ,[FirstPaypentPlanDate]
               ,[LastPaymentPlanDate]
               ,[DemandProgressKBN]
               ,[CommentDemand]
               ,[CancelDate]
               ,[CancelReasonKBN]
               ,[CancelRemarks]
               ,[NoMailFLG]
               ,[IndividualContactKBN]
               ,[TelephoneContactKBN]
               ,[LastMailKBN]
               ,[LastMailPatternCD]
               ,[LastMailDatetime]
               ,[LastMailName]
               ,[NextMailKBN]
               ,[CommentOutStore]
               ,[CommentInStore]
               ,[LastDepositeDate]
               ,[LastOrderDate]
               ,[LastArriveDate]
               ,[LastSalesDate]
               ,[MitsumoriNO]
               ,[TenzikaiJuchuuNO]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[JuchuuDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT
                @JuchuuNO
               ,@JuchuuProcessNO                      
               ,@StoreCD                          
               ,@JuchuuDate
               ,NULL    --JuchuuTime
               ,0 AS ReturnFLG
               ,DT.SoukoCD
               ,3   --JuchuuKBN
               ,0   --SiteKBN
               ,NULL    --SiteJuchuuDateTime
               ,NULL    --SiteJuchuuNO
               ,0   --InportErrFLG
               ,0   --OnHoldFLG
               ,0   --IdentificationFLG
               ,NULL    --TorikomiDateTime
               ,DT.StaffCD
               ,DT.CustomerCD
               ,DT.CustomerName
               ,DT.CustomerName2
               ,DT.AliasKBN
               ,DT.ZipCD1
               ,DT.ZipCD2
               ,DT.Address1
               ,DT.Address2
               ,DT.Tel11
               ,DT.Tel12
               ,DT.Tel13
               ,NULL    --Tel21
               ,NULL    --Tel22
               ,NULL    --Tel23
               ,NULL    --CustomerKanaName
               ,DT.DeliveryCD
               ,DT.DeliveryName
               ,DT.DeliveryName2
               ,DT.DeliveryAliasKBN
               ,DT.DeliveryZipCD1
               ,DT.DeliveryZipCD2
               ,DT.DeliveryAddress1
               ,DT.DeliveryAddress2
               ,DT.DeliveryTel11
               ,DT.DeliveryTel12
               ,DT.DeliveryTel13
               ,NULL    --Tel21
               ,NULL    --Tel22
               ,NULL    --Tel23
               ,NULL   --,[DeliveryKanaName]
               ,NULL    --JuchuuCarrierCD
               ,0   --DecidedCarrierFLG
               ,NULL    --LastCarrierCD
               ,NULL    --NameSortingDateTime
               ,NULL    --NameSortingStaffCD
               ,DT.CurrencyCD   --CurrencyCD
               ,DT.JuchuuGaku
               ,0 As Discount
               ,DT.HanbaiHontaiGaku
               ,DT.HanbaiTax8
               ,DT.HanbaiTax10
               ,DT.HanbaiGaku
               ,DT.CostGaku
               ,DT.ProfitGaku
               ,0   --Coupon
               --,@Point
               ,0   --PayCharge
               ,0   --Adjustments
               ,0   --Postage
               ,0   --GiftWrapCharge
               ,0   --InvoiceGaku
               ,DT.PaymentMethodCD
               ,NULL    --PaymentPlanNO
               ,0       --CardProgressKBN
               ,NULL    --CardCompany
               ,NULL    --CardNumber
               ,0   --PaymentProgressKBN
               ,0   --PresentFLG
               ,DT.SalesPlanDate
               ,NULL --FirstPaypentPlanDate
               ,NULL --LastPaymentPlanDate
               ,0   --DemandProgressKBN
               ,NULL    --CommentDemand
               ,NULL    --CancelDate
               ,NULL    --CancelReasonKBN
               ,NULL    --CancelRemarks
               ,0   --NoMailFLG
               ,0   --IndividualContactKBN
               ,0   --TelephoneContactKBN
               ,NULL    --LastMailKBN
               ,NULL    --LastMailPatternCD
               ,NULL    --LastMailDatetime
               ,NULL    --LastMailName
               ,NULL    --NextMailKBN
               ,NULL    --CommentOutStore
               ,NULL    --CommentInStore
               ,NULL    --LastDepositeDate
               ,NULL    --LastOrderDate
               ,NULL    --LastArriveDate
               ,NULL    --LastSalesDate
               ,NULL    --MitsumoriNO
               ,@TenzikaiJuchuuNO
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL                  
               ,NULL
           FROM D_TenzikaiJuchuu AS DT
           WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
           ;               

            --�yD_JuchuuDetails�z
            --Table�]���d�l�a
            INSERT INTO [D_JuchuuDetails]
                       ([JuchuuNO]
                       ,[JuchuuRows]
                       ,[DisplayRows]
                       ,[SiteJuchuuRows]
                       ,[NotPrintFLG]
                       ,[AddJuchuuRows]
                       ,[NotOrderFLG]
                       ,[ExpressFLG]
                       ,[AdminNO]
                       ,[SKUCD]
                       ,[JanCD]
                       ,[SKUName]
                       ,[ColorName]
                       ,[SizeName]
                       ,[SetKBN]
                       ,[SetRows]
                       ,[JuchuuSuu]
                       ,[JuchuuUnitPrice]
                       ,[TaniCD]
                       ,[JuchuuGaku]
                       ,[JuchuuHontaiGaku]
                       ,[JuchuuTax]
                       ,[JuchuuTaxRitsu]
                       ,[CostUnitPrice]
                       ,[CostGaku]
                       ,[OrderUnitPrice]
                       ,[ProfitGaku]
                       ,[SoukoCD]
                       ,[HikiateSu]
                       ,[DeliveryOrderSu]
                       ,[DeliverySu]
                       ,[DirectFLG]
                       ,[HikiateFLG]
                       ,[JuchuuOrderNO]
                       ,[VendorCD]
                       ,[LastOrderNO]
                       ,[LastOrderRows]
                       ,[LastOrderDateTime]
                       ,[DesiredDeliveryDate]
                       ,[AnswerFLG]
                       ,[ArrivePlanDate]
                       ,[ArrivePlanNO]
                       ,[ArriveDateTime]
                       ,[ArriveNO]
                       ,[ArribveNORows]
                       ,[DeliveryPlanNO]
                       ,[CommentOutStore]
                       ,[CommentInStore]
                       ,[IndividualClientName]
                       ,[ShippingPlanDate]
                       ,[SalesDate]
                       ,[SalesNO]
                       ,[DepositeDetailNO]
                       ,[InsertOperator]
                       ,[InsertDateTime]
                       ,[UpdateOperator]
                       ,[UpdateDateTime])
                 SELECT @JuchuuNO                         
                       ,1 AS JuchuuRows                     
                       ,@DisplayRows                      
                       ,0   --SiteJuchuuRows
                       ,0 AS NotPrintFLG
                       ,0 AS AddJuchuuRows
                       ,0 AS NotOrderFLG
                       ,0 AS ExpressFLG
                       ,FS.AdminNO
                       ,FS.SKUCD
                       ,FS.JanCD
                       ,FS.SKUName
                       ,FS.ColorName
                       ,FS.SizeName
                       ,0 AS SetKBN
                       ,0 AS SetRows
                       ,DM.JuchuuSuu
                       ,DM.JuchuuUnitPrice
                       ,DM.TaniCD
                       ,DM.JuchuuGaku
                       ,DM.JuchuuHontaiGaku
                       ,DM.JuchuuTax
                       ,DM.JuchuuTaxRitsu
                       ,DM.OrderUnitPrice AS CostUnitPrice
                       ,DM.OrderUnitPrice * DM.JuchuuSuu AS CostGaku
                       ,DM.OrderUnitPrice
                       ,DM.ProfitGaku
                       ,DM.SoukoCD
                       ,0 --HikiateSu
                       ,0 --DeliveryOrderSu
                       ,0 --DeliverySu
                       ,0 --DirectFLG
                       ,0 --HikiateFLG
                       ,NULL --JuchuuOrderNO
                       ,DT.VendorCD
                       ,NULL    --LastOrderNO
                       ,0   --LastOrderRows
                       ,NULL    --LastOrderDateTime
                       ,NULL	--DesiredDeliveryDate
                       ,0   --AnswerFLG
                       ,DM.ArrivePlanDate
                       ,NULL    --ArrivePlanNO
                       ,NULL    --ArriveDateTime
                       ,NULL    --ArriveNO
                       ,0   --ArribveNORows
                       ,@DeliveryPlanNO		--(CASE @OperateMode WHEN 1 THEN @DeliveryPlanNO ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
                       ,DM.CommentOutStore
                       ,DM.CommentInStore
                       ,DM.IndividualClientName
                       ,DM.ShippingPlanDate
                       ,NULL    --SalesDate
                       ,NULL    --SalesNO
                       ,NULL    --DepositeDetailNO
                       ,@Operator  
                       ,@SYSDATETIME
                       ,@Operator  
                       ,@SYSDATETIME

                   FROM D_TenzikaiJuchuu AS DT
                   INNER JOIN D_TenzikaiJuchuuDetails AS DM
                   ON DM.TenzikaiJuchuuNO = DT.TenzikaiJuchuuNO
                   AND DM.DeleteDateTime IS NULL
                   LEFT OUTER JOIN (SELECT MT.JanCD AS TenzikaiShouhinJanCD
                                     ,F.AdminNO
                                     ,F.SKUCD
                                     ,F.JanCD
                                     ,F.SKUName
                                     ,F.ColorName
                                     ,F.SizeName
                               FROM F_SKU(convert(date,@SYSDATETIME)) AS F
                               --�ꎞ���[�N�e�[�u���uM_TenzikaiShouhin�@�v(�ꎞ���[�N�e�[�u���uM_SKU�@�v�ŁuM_TenzikaiShouhin�@�v�Ƃ��Ďg�p)
                               INNER JOIN (SELECT MTS.ExhibitionCommonCD
                                                 ,MAX(MTS.JanCD) AS JanCD 
                                           FROM M_TenzikaiShouhin AS MTS
                                           WHERE MTS.DeleteFLG = 0
                                           AND MTS.VendorCD = @VendorCD
                                           AND MTS.LastYearTerm = @LastYearTerm
                                           AND MTS.LastSeason = @LastSeason
                                           GROUP BY MTS.ExhibitionCommonCD
                                           ) AS MT 
                               ON MT.ExhibitionCommonCD = F.ExhibitionCommonCD
                               WHERE F.DeleteFlg = 0) AS FS
                   ON FS.TenzikaiShouhinJanCD = DM.JanCD
                   WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
                   AND DM.TenzikaiJuchuuRows = @TenzikaiJuchuuRows
                  ;
                  
            SET @DisplayRows = @DisplayRows + 1;
            
            --�yD_StoreJuchuu�zTable�]���d�l�b
            INSERT INTO [D_StoreJuchuu]
                   ([JuchuuNO]
                   ,[NouhinsyoComment])
             VALUES
                   (@JuchuuNO
                   ,NULL    --NouhinsyoComment
                   )
                   ;
            
            --�yD_DeliveryPlan�zTable�]���d�l�f
            INSERT INTO [D_DeliveryPlan]
                   ([DeliveryPlanNO]
                   ,[DeliveryKBN]
                   ,[Number]
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
                   ,[PaymentMethodCD]
                   ,[CommentInStore]
                   ,[CommentOutStore]
                   ,[InvoiceNO]
                   ,[DeliveryPlanDate]
                   ,[HikiateFLG]
                   ,[IncludeFLG]
                   ,[OntheDayFLG]
                   ,[ExpressFLG]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
            SELECT
                    @DeliveryPlanNO
                   ,1 AS DeliveryKBN    --(1:�̔��A2:�q�Ɉړ�)
                   ,@JuchuuNO   AS Number
                   ,DT.DeliveryName
                   ,NULL    --[DeliverySoukoCD]
                   ,DT.DeliveryZipCD1
                   ,DT.DeliveryZipCD2
                   ,DT.DeliveryAddress1
                   ,DT.DeliveryAddress2
                   ,NULL    --[DeliveryMailAddress]
                   ,DT.DeliveryTel11 + '-' + DT.DeliveryTel12 + '-' + DT.DeliveryTel13    --[DeliveryTelphoneNO]
                   ,NULL    --[DeliveryFaxNO]
                   ,NULL    --[DecidedDeliveryDate]
                   ,NULL    --[DecidedDeliveryTime]
                   ,NULL    --[CarrierCD]
                   ,DT.PaymentMethodCD    --[PaymentMethodCD]
                   ,NULL    --[CommentInStore]
                   ,NULL    --[CommentOutStore]
                   ,NULL    --[InvoiceNO]
                   ,NULL    --[DeliveryPlanDate]
                   ,0   --[HikiateFLG]
                   ,0   --[IncludeFLG]
                   ,0   --[OntheDayFLG]
                   ,0   --[ExpressFLG]
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
            FROM D_TenzikaiJuchuu AS DT
            WHERE DT.TenzikaiJuchuuNO = @TenzikaiJuchuuNO
            ;
                       
            --�yD_DeliveryPlanDetails�z�z���\�薾�ׁ@Table�]���d�l�g
            INSERT INTO [D_DeliveryPlanDetails]
               ([DeliveryPlanNO]
               ,[DeliveryPlanRows]
               ,[Number]
               ,[NumberRows]
               ,[CommentInStore]
               ,[CommentOutStore]
               ,[HikiateFLG]
               ,[UpdateCancelKBN]
               ,[DeliveryOrderComIn]
               ,[DeliveryOrderComOut]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime])
             SELECT  
                @DeliveryPlanNO --ELSE (SELECT DD.DeliveryPlanNO FROM D_DeliveryPlan AS DD WHERE @JuchuuNO = DD.[Number])  END)
               ,DM.JuchuuRows AS DeliveryPlanRows
               ,@JuchuuNO AS Number
               ,DM.JuchuuRows  As NumberRows
               ,DM.CommentInStore
               ,DM.CommentOutStore
               ,0   --HikiateFLG
               ,0   --UpdateCancelKBN
               ,NULL    --DeliveryOrderComIn
               ,NULL    --DeliveryOrderComOut                        
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME

              FROM D_JuchuuDetails AS DM
              WHERE DM.JuchuuNO = @JuchuuNO
              ;
              
            -- ========= ���[�v���̎��ۂ̏��� �����܂�===
            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_Tenzi
            INTO @TenzikaiJuchuuNO, @JuchuuDate, @TenzikaiJuchuuRows;

        END
        
        --�J�[�\�������
        CLOSE CUR_Tenzi;
        DEALLOCATE CUR_Tenzi;
            
        --�yD_TenzikaiJuchuu�z Table�]���d�l�h�i�X�V�iUpdate�j�j
        UPDATE D_TenzikaiJuchuu SET
            [JuchuuHurikaeZumiFLG]  = 1
           ,[JuchuuHurikaeDateTime] = @SYSDATETIME
           ,[UpdateOperator]        = @Operator  
           ,[UpdateDateTime]        = @SYSDATETIME
        WHERE DeleteDateTime IS NULL
        AND VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE VendorCD END)
        AND LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE LastYearTerm END)
        AND LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE LastSeason END)
        AND JuchuuHurikaeZumiFLG = 0
        ;

    END
    
    --�폜��I�����Ă���ꍇ�A(If you have selected by "�폜",)
    ELSE IF @OperateMode = 3
    BEGIN
        --�J�[�\����`
        DECLARE CUR_Juchu CURSOR FOR
            --�Y���̓W����󒍔ԍ��ɕR�Â��󒍔ԍ����擾(Obtain "�󒍔ԍ�" associated with the corresponding "�W����󒍔ԍ�")
            --(Table_JuchuuNO)
            SELECT A.JuchuuNO
            From D_Juchuu AS A
            --�Y���̓W����󒍔ԍ����擾(Obtain the relevant "�W����󒍔ԍ�")
            --(Table_TenzikaiJuchuuNO)
            Inner Join (SELECT DT.TenzikaiJuchuuNO                                                                                 
                        From D_TenzikaiJuchuu AS DT                                                                                 
                        Where CAST(DT.InsertDateTime AS DATE) = @TorokuDate
                        AND DT.VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE DT.VendorCD END)
                        AND DT.LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE DT.LastYearTerm END)
                        AND DT.LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE DT.LastSeason END)
                        And DT.DeleteDateTime IS NULL
            )   --Table_TenzikaiJuchuuNO 
            AS B
            On A.TenzikaiJuchuuNO = B.TenzikaiJuchuuNO
            Where A.DeleteDateTime IS NULL            
            ;
            
        --�J�[�\���I�[�v��
        OPEN CUR_Juchu;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_Juchu
        INTO @JuchuuNO;
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- ========= ���[�v���̎��ۂ̏��� ��������===
            --D_Juchuu Delete
            Delete FROM D_Juchuu 
            WHERE JuchuuNO = @JuchuuNO
            AND DeleteDateTime IS NULL
            ;
            
            --D_JuchuuDetails
            DELETE FROM D_JuchuuDetails
            WHERE JuchuuNO = @JuchuuNO
            AND DeleteDateTime IS NULL
            ;
            
            --D_StoreJuchuu 
            Delete FROM D_StoreJuchuu
            WHERE JuchuuNO = @JuchuuNO
            ;
            
            --D_DeliveryPlan
            Delete FROM D_DeliveryPlan
            WHERE Number = @JuchuuNO
            ;
            
            --D_DeliveryPlanDetails
            Delete FROM D_DeliveryPlanDetails
            WHERE Number = @JuchuuNO
            ;

            -- ========= ���[�v���̎��ۂ̏��� �����܂�===
            --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
            FETCH NEXT FROM CUR_Juchu
            INTO @JuchuuNO;

        END
        
        --�J�[�\�������
        CLOSE CUR_Juchu;
        DEALLOCATE CUR_Juchu;

        --�yD_TenzikaiJuchuu�z Table�]���d�l�h�i�폜�iUpdate�j�j
        UPDATE D_TenzikaiJuchuu SET
            [JuchuuHurikaeZumiFLG]  = 0
           ,[JuchuuHurikaeDateTime] = NULL
           ,[UpdateOperator]        = @Operator  
           ,[UpdateDateTime]        = @SYSDATETIME
        WHERE DeleteDateTime IS NULL
        AND VendorCD = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE VendorCD END)
        AND LastYearTerm = (CASE WHEN @LastYearTerm <> '' THEN @LastYearTerm ELSE LastYearTerm END)
        AND LastSeason = (CASE WHEN @LastSeason <> '' THEN @LastSeason ELSE LastSeason END)
        AND JuchuuHurikaeZumiFLG = 1
        AND CAST(InsertDateTime AS DATE) = @TorokuDate
        ;

    END
     
    --���������f�[�^�֍X�V
    --SET @KeyItem = @JuchuuNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TenzikaiJuchuuJouhouHurikaeShori',
        @PC,
        NULL,
        @KeyItem;

    --SET @OutJuchuuProcessNO = @JuchuuProcessNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
