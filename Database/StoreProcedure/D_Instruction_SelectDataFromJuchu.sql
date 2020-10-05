
/****** Object:  StoredProcedure [dbo].[D_Instruction_SelectDataFromJuchu]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Instruction_SelectDataFromJuchu]
GO

/****** Object:  StoredProcedure [D_Instruction_SelectAll]    */
CREATE PROCEDURE D_Instruction_SelectDataFromJuchu(
    -- Add the parameters for the stored procedure here
    @DeliveryPlanDateFrom  varchar(10),
    @DeliveryPlanDateTo  varchar(10),
    @StoreCD  varchar(4),
    @Chk1 tinyint,
    @Chk2 tinyint,
    --@Chk3 tinyint,
    --@Chk4 tinyint,
    @Chk5 tinyint,
    @DeliveryName  varchar(80),
    @ChkHakkozumi tinyint,
    @ChkSyukkazumi tinyint,
    @ChkSyukkaFuka tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --��ʍ��ړ]���\01�@(�o�׎w�����쐬��)�E�E�E�󒍏�񂩂�\��
    SELECT  DJ.ExpressFLG
            ,(CASE WHEN DJ.FareLevel > SUM(DJ.JuchuuHontaiGaku) OVER(PARTITION BY DJ.CustomerCD) THEN 1 ELSE 0 END) AS UntinFlg
            ,DJ.JuchuuNO + '-' + RIGHT('00' + CONVERT(varchar,DJ.JuchuuRows),3) AS JuchuuNO
            ,NULL AS PrintDate		--�o�׎w����
            ,DJ.CustomerCD
            ,DJ.CustomerName
            ,DJ.DeliveryCD
            ,DJ.DeliveryName
            ,DJ.DeliveryAddress1
            ,CONVERT(varchar,DJ.DeliveryPlanDate,111) As DeliveryPlanDate	--�o�ח\���
            ,DJ.Minyukin    --
            ,DJ.HanbaiHontaiGaku
            ,NULL AS InstructionNO--�o�׎w���ԍ�
            ,NULL AS ShippingDate	--�o�ד�
            ,DJ.CommentOutStore
            ,DJ.CommentInStore
            ,DJ.AdminNO
            ,DJ.JANCD
            ,DJ.SKUCD
            ,DJ.SKUName
            ,DJ.ColorName
            ,DJ.SizeName
            ,DJ.JuchuuSuu
            ,DJ.SoukoName
            ,DJ.ShukkaShubetsu
            ,DJ.DeliveryPlanNO--�z���\��ԍ� AS DeliveryPlanNO	--�z���\��ԍ�
            ,DJ.DeliveryPlanRows
            ,0 AS InstructionRows	--�o�׎w�����טA��
            ,DJ.JuchuuNo As JuchuNo
            --��D_Juchuu�A�FD_Juchuu�@���ڋqCD�ŃO���[�v������SUM(D_Juchuu�@.���׎󒍖{�̊z)��SELECT
            ,SUM(DJ.JuchuuHontaiGaku) OVER(PARTITION BY DJ.CustomerCD) AS JuchuuHontaiGaku
            ,0 AS CNT

            ,1 AS KBN	--�o�׎w�����쐬��
        FROM (SELECT DM.ExpressFLG
                    ,DM.JuchuuNO
                    ,DM.JuchuuRows
                    ,DH.CustomerName
                    ,DH.DeliveryCD
                    ,DH.DeliveryName
                    ,DH.DeliveryAddress1
                    --,(CASE WHEN DM.ShippingPlanDate IS NULL THEN NULL 
                    --       ELSE (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DM.ShippingPlanDate) < SYSDATETIME() THEN CONVERT(varchar, SYSDATETIME(),111)
                    --                  ELSE CONVERT(varchar, DM.ShippingPlanDate,111) END)  END) AS DeliveryPlanDate	--�o�ח\���
                    ,(CASE WHEN DM.ShippingPlanDate IS NULL THEN CONVERT(varchar, GETDATE(), 111)
                      ELSE CONVERT(varchar, DM.ShippingPlanDate,111) END) AS DeliveryPlanDate
                    --������
                    ,DH.InvoiceGaku
                    ,DB1.TotalCollectAmount AS TotalCollectAmount_DB1	 --D_CollectBilling�@
                    -- D_CollectPlan�@.TotalCollectPlanGaku�|D_CollectBilling�A.TotalCollectAmount�{D_Juchuu.InvoiceGaku
                    ,DB2.TotalCollectAmount AS TotalCollectAmount_DB2
                    ,MD.DepositLaterFLG

                    ,(CASE @ChkSyukkaFuka WHEN 0 THEN 0 --Form.�o�וs��(�����s��)���܂ށ�ON�̏ꍇ�AOFF
                    	                  ELSE (CASE MD.DepositLaterFLG WHEN 0 THEN 0 ELSE 1 END) END) AS Minyukin --= 0�i������j�̏ꍇ�AOFF
                    
                    ,DH.HanbaiHontaiGaku
                    ,DM.CommentOutStore
                    ,DM.CommentInStore
                    ,DM.AdminNO
                    ,DM.JANCD
                    ,DM.SKUCD
                    ,DM.SKUName
                    ,DM.ColorName
                    ,DM.SizeName
                    ,DM.JuchuuSuu
                    ,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
                       WHERE M.SoukoCD = DM.SoukoCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS SoukoName
                    ,1 As ShukkaShubetsu
                    ,DH.JuchuuDate
                    ,DH.CustomerCD
                    ,DM.JuchuuHontaiGaku
                    ,(SELECT top 1 M.CollectCD FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS CollectCD
                    ,(SELECT top 1 M.BillingCD FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS BillingCD
                    ,(SELECT top 1 M.FareLevel FROM M_Customer AS M 
                       WHERE M.CustomerCD = DH.CustomerCD AND M.ChangeDate <= DH.JuchuuDate
                         AND M.DeleteFlg = 0 
                       ORDER BY M.ChangeDate desc) AS FareLevel 
                    ,DD.DeliveryPlanNO--�z���\��ԍ�
                    ,DD.DeliveryPlanRows--�z���\��ԍ�
            FROM D_JuchuuDetails AS DM
           INNER JOIN D_Juchuu AS DH
              ON DH.JuchuuNo = DM.JuchuuNo
             AND DH.DeleteDateTime IS NULL
           INNER JOIN D_DeliveryPlanDetails AS DD
              ON DD.Number = DM.JuchuuNo
             AND DD.NumberRows = DM.JuchuuRows
           --AND DD.DeleteDateTime IS NULL
            LEFT OUTER JOIN M_ZipCode AS MZ
              ON MZ.ZipCD1 = DH.DeliveryZipCD1
             AND MZ.ZipCD2 = DH.DeliveryZipCD2
            LEFT OUTER JOIN M_DenominationKBN As MD
              ON MD.DenominationCD = DH.PaymentMethodCD
            
            --�@����\�肩������ϊz���W�v�iD_CollectBilling��CollectPlanNO��Groupby �K�v)
            LEFT OUTER JOIN D_CollectPlan AS DC
              ON DC.JuchuuNO = DH.JuchuuNO
             AND DC.DeleteDateTime IS NULL
            
            --D_CollectBilling�@
            LEFT OUTER JOIN (
                SELECT CollectPlanNO AS CollectPlanNO                                  
                      ,SUM(CollectAmount) AS TotalCollectAmount                                    
                 FROM D_CollectBilling                                    
                WHERE DeleteDateTime IS NULL                                  
                GROUP BY CollectPlanNO                                   
            ) AS DB1
            ON DB1.CollectPlanNO = DC.CollectPlanNO

            --��D_CollectBilling�A�FCollectPlanNO�ŃO���[�v���iD_CollectBilling.DeleteDateTime IS NULL�j����SUM(D_CollectBilling.CollectAmount)��SELECT
            LEFT OUTER JOIN (
                SELECT CollectPlanNO AS CollectPlanNO
                      ,SUM(CollectAmount) AS TotalCollectAmount
                 FROM D_CollectBilling
                WHERE DeleteDateTime IS NULL
                GROUP BY CollectPlanNO
            ) AS DB2
            ON DB2.CollectPlanNO = DC.CollectPlanNO
                
            WHERE DM.ShippingPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DM.ShippingPlanDate END)
            AND DM.ShippingPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DM.ShippingPlanDate END)
            AND DD.HikiateFLG = 1
            AND DH.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
            AND DM.DeleteDateTime IS NULL
            AND ((@Chk1 = 1 AND DM.ExpressFLG = 0)    --�ʏ�
                OR (@Chk2 = 1 AND DM.ExpressFLG = 1)    --���}CheckBox=ON�̎�
                )
            AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B     --�������Ă��Ȃ������Ȃ�
                           WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                           --AND B.DeleteDateTime IS NULL
                           AND B.HikiateFLG = 0
                           AND B.UpdateCancelKBN <> 9)
            AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                           WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO     --�o�׎w�����쐬
                           AND DI.DeleteDateTime IS NULL)
                           
            AND EXISTS(SELECT DR.ReserveNO FROM D_Reserve AS DR
                       WHERE DR.Number = DM.JuchuuNo
                       AND DR.NumberRows = DM.JuchuuRows
                       AND DR.DeleteDateTime IS NULL)

        ) AS DJ
        
        --�B�������ł̖����������z���W�v�iD_Collect��CollectCustomerCD��Group by�K�v�j
        --��D_Collect�@�FCollectCustomerCD�ŃO���[�v���iD_Collect.DeleteDateTime IS NULL�j����SUM(D_Collect.ConfirmSource),SUM(D_Collect.CollectAmount)��SELECT
        LEFT OUTER JOIN (
            SELECT CollectCustomerCD AS CollectCustomerCD                              
                  ,SUM(ConfirmSource) AS TotalConfirmSource                                
                  ,SUM(CollectAmount) AS TotalCollectAmount                                
            FROM D_Collect                               
            WHERE DeleteDateTime IS NULL                              
            GROUP BY CollectCustomerCD                               
        ) AS DC1
        ON DC1.CollectCustomerCD = DJ.CollectCD

        --�C�������ɑΉ����鐿����ւ̎󒍂��擾���Ė����������W�v�iGroup by�K�v�j
        --��D_CollectPlan�@�FCustomerCD�ŃO���[�v���iD_CollectPlan.DeleteDateTime IS NULL�j����SUM(D_CollectPlan.CollectPlanGaku)��SELECT
        LEFT OUTER JOIN (
            SELECT DC.CustomerCD AS CustomerCD
            	  ,SUM(DC.CollectPlanGaku) AS TotalCollectPlanGaku
            FROM D_CollectPlan AS DC
            WHERE DC.DeleteDateTime IS NULL
            GROUP BY DC.CustomerCD
        ) AS DCP1
        ON DCP1.CustomerCD = DJ.BillingCD

        --�o�וs��(�����s��)���܂ށ@CheckBox=ON�̎��i�������s�����\�����適�܂�����̏�ԂɊ֌W�Ȃ��\������j
        WHERE ((@ChkSyukkaFuka = 1 AND DJ.DepositLaterFLG IN (0,1))
            OR (@ChkSyukkaFuka = 0 AND ((DJ.DepositLaterFLG = 1 AND (DJ.InvoiceGaku <= DJ.TotalCollectAmount_DB1 OR DCP1.TotalCollectPlanGaku-DJ.TotalCollectAmount_DB2+DJ.InvoiceGaku<=DC1.TotalConfirmSource-DC1.TotalCollectAmount))
                                    OR DJ.DepositLaterFLG = 0)))
    UNION ALL
    
    --��ʍ��ړ]���\02�@(�o�׎w�����쐬��)�E�E�E�z���\����(�󒍈ȊO)����\��
    SELECT  DD.ExpressFLG
    		,0 AS Untin
            ,NULL AS JuchuuNO
            ,NULL AS PrintDate		--�o�׎w����
            ,NULL AS CustomerCD
            ,DD.DeliveryName AS CustomerName
            ,DD.CarrierCD AS DeliveryCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DD.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DD.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS DeliveryName
            ,DD.DeliveryAddress1
            ,(CASE WHEN DD.DecidedDeliveryDate IS NOT NULL THEN 
                    (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DD.DecidedDeliveryDate) < SYSDATETIME() THEN
                               CONVERT(varchar,SYSDATETIME(),111)
                          ELSE CONVERT(varchar,DD.DecidedDeliveryDate,111) END)
                   ELSE CONVERT(varchar,DD.DeliveryPlanDate,111) END) AS DeliveryPlanDate   --�o�ח\���
            ,0 AS Minyukin    --
            ,0 AS HanbaiHontaiGaku
            ,NULL AS InstructionNO  --�o�׎w���ԍ�
            ,NULL AS ShippingDate	--�o�ד�
            ,DD.CommentOutStore
            ,DD.CommentInStore
            ,DR.AdminNO
            ,(SELECT top 1 M.JANCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.SKUCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ColorName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 M.SizeName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DD.DeliveryPlanDate
              AND M.AdminNO = DR.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SizeName
            ,DMD.MoveSu AS JuchuuSuu
            --,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
            --  WHERE M.SoukoCD = DD.DeliverySoukoCD AND M.ChangeDate <= DD.DeliveryPlanDate
            --  AND M.DeleteFlg = 0 
            --  AND M.StoreCD = @StoreCD
            --  ORDER BY M.ChangeDate desc) AS SoukoName
            ,NULL As SoukoName
            ,DD.DeliveryKBN As ShukkaShubetsu
            ,DM.DeliveryPlanNO--�z���\��ԍ�
            ,DM.DeliveryPlanRows
            ,0 AS InstructionRows	--�o�׎w�����טA��
            ,NULL As JuchuNo
            --��D_Juchuu�A�FD_Juchuu�@���ڋqCD�ŃO���[�v������SUM(D_Juchuu�@.���׎󒍖{�̊z)��SELECT
            ,0 AS JuchuuHontaiGaku
            ,0 AS CNT

            ,2 AS KBN	--�z���\����

        FROM D_DeliveryPlan AS DD
        INNER JOIN D_DeliveryPlanDetails AS DM
        ON DM.DeliveryPlanNO = DD.DeliveryPlanNO
        --AND DM.DeleteDateTime IS NULL
        AND DM.UpdateCancelKBN <> 9
        INNER JOIN D_Reserve AS DR
        ON DR.Number = DM.Number
        AND DR.NumberRows = DM.NumberRows
        AND DR.DeleteDateTime IS NULL
        /*LEFT OUTER JIN D_Stock AS DS
        ON DS.StockNO = DR.StockNO
        AND DS.DeleteDateTime IS NULL*/
        INNER JOIN D_MoveDetails AS DMD
        ON DMD.MoveNO = DR.Number
        AND DMD.MoveRows = DR.NumberRows
        AND DMD.DeleteDateTime IS NULL
        
        LEFT OUTER JOIN M_ZipCode MZ
        ON MZ.ZipCD1 = DD.DeliveryZip1CD
        AND MZ.ZipCD2 = DD.DeliveryZip2CD

        WHERE DD.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DD.DeliveryPlanDate END)
        AND DD.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DD.DeliveryPlanDate END)
        AND DD.HikiateFLG = 1
        AND DD.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        --AND DD.DeleteDateTime IS NULL
        
        AND ((@Chk1 = 1 AND DD.ExpressFLG <> 1 AND DD.DeliveryKBN <> 2)    --�ʏ�
            OR (@Chk2 = 1 AND DD.ExpressFLG = 1)    --���}CheckBox=ON�̎�
            OR (@Chk5 = 1 AND DD.DeliveryKBN = 2)
            )
        --AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
        --           WHERE M.SoukoCD = DD.DeliverySoukoCD
        --           AND M.DeleteFlg = 0
        --           AND M.StoreCD = @StoreCD
        --    )
        --���������Ă��Ȃ������Ȃ�
        AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B
                       WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                       --AND B.DeleteDateTime IS NULL
                       AND B.HikiateFLG = 0
                       AND B.UpdateCancelKBN <> 9)
        --���o�׎w�����쐬
        AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                       WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO
                       AND DI.DeleteDateTime IS NULL)
	UNION ALL
	
    --��ʍ��ړ]���\03�@(�o�׎w���쐬�ϕ�)�E�E�E�o�׎w���f�[�^����\��
    SELECT  DI.ExpressFLG
            --���񒊏o�Ώۂ̏o�׎w������(�o�׎w���ԍ��P�ʂ�)�ɉ^�����i���܂܂�Ă���ꍇ�A
            ,(CASE WHEN MM.Num1 = DIM.AdminNO THEN 1 ELSE 0 END) AS Untin
            ,DR.Number + '-' + RIGHT('00' + CONVERT(varchar,DR.NumberRows),3) AS JuchuuNO
            ,CONVERT(varchar,DI.PrintDate,111) AS PrintDate		--�o�׎w����
            ,NULL AS CustomerCD
            ,DI.DeliveryName AS CustomerName
            ,DI.CarrierCD AS DeliveryCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DI.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DI.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS DeliveryName
            ,DI.DeliveryAddress1
            ,CONVERT(varchar,DI.DeliveryPlanDate,111) AS DeliveryPlanDate   --�o�ח\���
            ,0 AS Minyukin    --
            ,NULL AS HanbaiHontaiGaku
            ,DI.InstructionNO		--�o�׎w���ԍ�
            ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate	--�o�ד�
            ,DI.CommentOutStore
            ,DI.CommentInStore
            ,DR.AdminNO
            ,(SELECT top 1 M.JANCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS JANCD
            ,(SELECT top 1 M.SKUCD 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUCD
            ,(SELECT top 1 M.SKUName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SKUName
            ,(SELECT top 1 M.ColorName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS ColorName
            ,(SELECT top 1 M.SizeName 
              FROM M_SKU AS M 
              WHERE M.ChangeDate <= DI.DeliveryPlanDate
              AND M.AdminNO = DIM.AdminNO
              AND M.DeleteFlg = 0
              ORDER BY M.ChangeDate desc) AS SizeName
            ,DIM.InstructionSu AS JuchuuSuu
            --,(SELECT top 1 M.SoukoName FROM M_Souko AS M 
            --  WHERE M.SoukoCD = DI.DeliverySoukoCD 
            --  AND M.ChangeDate <= DI.DeliveryPlanDate
            --  AND M.DeleteFlg = 0
            --  AND M.StoreCD = @StoreCD
            --  ORDER BY M.ChangeDate desc) AS SoukoName
            ,NULL AS SoukoName
            ,DI.InstructionKBN As ShukkaShubetsu
            ,DI.DeliveryPlanNO	--�z���\��ԍ�
            ,DM.DeliveryPlanRows
            ,DIM.InstructionRows	--�o�׎w�����טA��
            ,NULL As JuchuNo
            --��D_Juchuu�A�FD_Juchuu�@���ڋqCD�ŃO���[�v������SUM(D_Juchuu�@.���׎󒍖{�̊z)��SELECT
            ,0 AS JuchuuHontaiGaku
            ,(SELECT COUNT(*)
                FROM D_InstructionDetails AS DISM
                INNER JOIN D_Instruction DISH
                ON DISH.InstructionNO = DISM.InstructionNO
                LEFT OUTER JOIN D_ShippingDetails AS DSPM
                ON DSPM.InstructionNO = DISM.InstructionNO
                AND DSPM.InstructionRows = DISM.InstructionRows
                WHERE DSPM.InstructionNO IS NULL
                AND DISM.InstructionNO <> DI.InstructionNO
                AND DISM.InstructionRows <> DIM.InstructionRows
                AND DISH.DeliveryName = DI.DeliveryName
             ) AS CNT
            ,3 AS KBN	--�z���\����

        FROM D_Instruction AS DI
        INNER JOIN D_InstructionDetails AS DIM
        ON DIM.InstructionNO = DI.InstructionNO
        AND DIM.DeleteDateTime IS NULL
        INNER JOIN D_Reserve AS DR
        ON DR.ReserveNO = DIM.ReserveNO
        AND DR.DeleteDateTime IS NULL
        INNER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL
        INNER JOIN D_DeliveryPlanDetails AS DM
        ON DR.Number = DM.Number
        AND DR.NumberRows = DM.NumberRows
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.InstructionNO = DI.InstructionNO
        AND DS.DeleteDateTime IS NULL
        LEFT OUTER JOIN M_MultiPorpose AS MM
        ON MM.ID = 227
        AND MM.[KEY] = 1
        
        WHERE DI.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DI.DeliveryPlanDate END)
        AND DI.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DI.DeliveryPlanDate END)

        AND DI.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND DI.DeleteDateTime IS NULL
        AND DIM.AdminNO <> MM.Num1	--�^�����ׂ͕\���ΏۊO

        AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.InstructionKBN <> 2)    --�ʏ�
            OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --���}CheckBox=ON�̎�
            OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
            )
        --AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
        --        WHERE M.SoukoCD = DI.DeliverySoukoCD
        --        AND M.DeleteFlg = 0
        --        AND M.StoreCD = @StoreCD
        --    )
        AND (@ChkHakkozumi = 1 OR (@ChkHakkozumi = 0 AND DI.PrintDate IS NULL))
        AND (@ChkSyukkazumi = 1 OR (@ChkSyukkazumi = 0 AND NOT EXISTS(SELECT 1 FROM D_Shipping AS A
                                                                      WHERE A.InstructionNO = DI.InstructionNO
                                                                      AND A.DeleteDateTime IS NULL)))        
    --����ʍ��ړ]���\01�̃f�[�^���܂߂Ă̕��я�
    --�o�ח\���,�o�א於(�ڋq��),�󒍔ԍ�-���טA��,�o�׎w���ԍ�
    ORDER BY DeliveryPlanDate, DeliveryName, JuchuuNO, InstructionNO	
    ;

END

GO
