IF OBJECT_ID ( 'D_Purchase_SelectDataH', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Purchase_SelectDataH]
GO
IF OBJECT_ID ( 'D_Stock_SelectAllForShiireH', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Stock_SelectAllForShiireH]
GO
IF OBJECT_ID ( 'PRC_HenpinNyuuryoku', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_HenpinNyuuryoku]
GO

--  ======================================================================
--       Program Call    �d������
--       Program ID      HenpinNyuuryoku
--       Create date:    2019.11.20
--    ======================================================================
CREATE PROCEDURE D_Purchase_SelectDataH
    (@OperateMode    tinyint,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    @PurchaseNO varchar(11)
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

--        IF @OperateMode = 2   --�C����
--        BEGIN
            SELECT DH.PurchaseNO
                  ,DH.StoreCD
                  ,CONVERT(varchar,DH.PurchaseDate,111) AS PurchaseDate
                  ,DH.CancelFlg
                  ,DH.ProcessKBN
                  ,DH.ReturnsFlg
                  ,DH.VendorCD
                  ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                  WHERE A.VendorCD = DH.VendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
				  AND A.VendorFlg = 1
                  ORDER BY A.ChangeDate desc) AS VendorName 
                  ,DH.CalledVendorCD
                  ,(SELECT top 1 A.VendorName
                  FROM M_Vendor A 
                  WHERE A.VendorCD = DH.CalledVendorCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.PurchaseDate
                  ORDER BY A.ChangeDate desc) AS CalledVendorName 
                  ,DH.CalculationGaku * (-1) As CalculationGaku
                  ,DH.AdjustmentGaku * (-1) As AdjustmentGaku
                  ,DH.PurchaseGaku * (-1) As PurchaseGaku
                  ,DH.PurchaseTax * (-1) As PurchaseTax
                  ,DH.TotalPurchaseGaku
                  ,DH.CommentOutStore
                  ,DH.CommentInStore
                  ,DH.ExpectedDateFrom
                  ,DH.ExpectedDateTo
                  ,DH.InputDate
                  ,DH.StaffCD
                  ,CONVERT(varchar,DH.PaymentPlanDate,111) AS PaymentPlanDate
                  ,DH.PayPlanNO
                  ,DH.OutputDateTime
                  ,DH.StockAccountFlg
                  ,DH.InsertOperator
                  ,CONVERT(varchar,DH.InsertDateTime) AS InsertDateTime
                  ,DH.UpdateOperator
                  ,CONVERT(varchar,DH.UpdateDateTime) AS UpdateDateTime
                  ,DH.DeleteOperator
                  ,CONVERT(varchar,DH.DeleteDateTime) AS DeleteDateTime
                  
                  ,DM.PurchaseRows
                  ,DM.DisplayRows
                  ,DM.ArrivalNO
                  ,DM.SKUCD
                  ,DM.AdminNO
                  ,DM.JanCD
                  ,DM.ItemName
                  ,DM.ColorName
                  ,DM.SizeName
                  ,DM.Remark
                  ,DM.PurchaseSu * (-1) AS PurchaseSu
                  ,DM.TaniCD
                  ,DM.TaniName
                  ,DM.PurchaserUnitPrice
                  ,DM.CalculationGaku * (-1) AS D_CalculationGaku
                  ,DM.AdjustmentGaku * (-1) As D_AdjustmentGaku
                  ,DM.PurchaseGaku * (-1) AS D_PurchaseGaku
                  ,DM.PurchaseTax AS D_PurchaseTax
                  ,DM.TotalPurchaseGaku AS D_TotalPurchaseGaku
                  ,DM.CurrencyCD
                  ,DM.CommentOutStore AS D_CommentOutStore
                  ,DM.CommentInStore AS D_CommentInStore
                  ,DM.ReturnNO
                  ,DM.ReturnRows
                  ,DM.OrderUnitPrice
                  ,DM.OrderNO
                  ,DM.OrderRows
                  ,DM.DifferenceFlg
                  ,DM.DeliveryNo
                  ,DW.WarehousingNO
                  ,DWS.StockNO
                  
                  ,DS.ReturnPlanSu
                  ,CONVERT(varchar,DS.ExpectReturnDate,111) AS ExpectReturnDate
                  ,(SELECT top 1 M.MakerItem 
                    FROM M_SKU AS M 
                    WHERE M.ChangeDate <= DH.PurchaseDate
                     AND M.AdminNO = DM.AdminNO
                      AND M.DeleteFlg = 0
                     ORDER BY M.ChangeDate desc) AS MakerItem
                     
              FROM D_Purchase DH
              LEFT OUTER JOIN D_PurchaseDetails AS DM 
              ON DH.PurchaseNO = DM.PurchaseNO 
              AND DM.DeleteDateTime IS NULL
              LEFT OUTER JOIN (SELECT MAX(DW.WarehousingNO) AS WarehousingNO
                    ,DW.Number, DW.NumberRow
                   FROM D_Warehousing AS DW
                   WHERE DW.WarehousingKBN = 21
                  AND DW.DeleteDateTime IS NULL
                  AND DW.DeleteFlg = 0
                  GROUP BY DW.Number,DW.NumberRow
                  ) AS DW
              ON DW.Number = DM.PurchaseNO
              AND DW.NumberRow = DM.PurchaseRows

           	  LEFT OUTER JOIN D_Warehousing AS DWS
           	  ON DWS.WarehousingNO = DW.WarehousingNO

              LEFT OUTER JOIN D_Stock AS DS
              ON DS.StockNO = DWS.StockNO
              AND DS.DeleteDateTime IS NULL
              WHERE DH.PurchaseNO = @PurchaseNO 
--              AND DH.ProcessKBN = 1
--              AND DH.DeleteDateTime IS Null
                ORDER BY DH.PurchaseNO, DM.DisplayRows
                ;
--        END

END

GO
CREATE PROCEDURE D_Stock_SelectAllForShiireH
    (
    @ExpectReturnDateFrom  varchar(10),
    @ExpectReturnDateTo  varchar(10),
    @VendorCD  varchar(13),
    @StoreCD  varchar(4),
    @F10 tinyint	--F10=1�̂Ƃ���D_Delivery��INNER JOIN
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
    
    DECLARE @SYSDATETIME datetime;
    SET @SYSDATETIME = SYSDATETIME();

    -- Insert statements for procedure here
    SELECT (SELECT top 1 M.MakerItem 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS MakerItem
          ,DS.JanCD
          ,(SELECT top 1 M.SKUName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ItemName
          ,(SELECT top 1 M.ColorName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS ColorName
          ,(SELECT top 1 M.SizeName 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SizeName
          ,(SELECT top 1 M.TaniCD 
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS TaniCD
          ,(SELECT top 1 A.Char1 
            FROM M_SKU AS M 
            LEFT OUTER JOIN M_MultiPorpose AS A
            ON A.ID = 201
            AND A.[Key] = M.TaniCD 
            WHERE M.ChangeDate <= convert(date, @SYSDATETIME)
             AND M.AdminNO = DS.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS TaniName
          ,DS.AdminNO
          ,DS.ReturnPlanSu	--�\�萔
          ,DS.ReturnPlanSu - DS.ReturnSu AS PurchaseSu	--�ԕi��
          ,NULL AS WarehousingNO
          ,DS.StockNO

          ,'' AS D_CommentOutStore
          ,'' AS D_CommentInStore
          ,CONVERT(varchar,DS.ExpectReturnDate,111) AS ExpectReturnDate
          ,DD.DeliveryNo
          
          ,ISNULL(DD.DeliveryUnitPrice,0) AS PurchaserUnitPrice
          ,(DS.ReturnPlanSu - DS.ReturnSu)*ISNULL(DD.DeliveryUnitPrice,0) AS D_CalculationGaku
          ,0 AS D_AdjustmentGaku
          ,(DS.ReturnPlanSu - DS.ReturnSu)*ISNULL(DD.DeliveryUnitPrice,0) AS D_PurchaseGaku
          
          ,DA.Number AS OrderNO
          ,DA.NumberRows AS OrderRows          
          ,DS.SKUCD
          
      FROM  D_Stock AS DS
      LEFT OUTER JOIN  D_Delivery AS DD
      ON /*DD.VendorDeliveryNo = DS.VendorDeliveryNo
      AND*/ DD.VendorCD = DS.VendorCD
      AND DD.AdminNO = DS.AdminNO
      AND DD.DeliverySu > DD.PurchaseSu
      AND DD.MatchingFlg = 0
      AND DD.DeleteDateTime IS NULL
	  
      LEFT OUTER JOIN D_ArrivalPlan AS DA
      ON DA.ArrivalPlanNO = DS.ArrivalPlanNO
      --AND DA.ArrivalPlanKBN = 1     --1:�������A2:�ړ���
      AND DA.DeleteDateTime IS NULL
	  
      WHERE ISNULL(DS.VendorCD,'') = (CASE WHEN @VendorCD <> '' THEN @VendorCD ELSE ISNULL(DS.VendorCD,'') END)
      AND DS.ExpectReturnDate IS NOT NULL
      AND DS.ReturnPlanSu > DS.ReturnSu

      AND DS.ExpectReturnDate >= (CASE WHEN @ExpectReturnDateFrom <> '' THEN CONVERT(DATE, @ExpectReturnDateFrom) ELSE DS.ExpectReturnDate END)
      AND DS.ExpectReturnDate <= (CASE WHEN @ExpectReturnDateTo <> '' THEN CONVERT(DATE, @ExpectReturnDateTo) ELSE DS.ExpectReturnDate END)
      
      AND EXISTS (SELECT 1 FROM M_Souko AS M WHERE M.SoukoCD = DS.SoukoCD
                    AND M.StoreCD = @StoreCD
                    AND M.ChangeDate <= convert(date, @SYSDATETIME) 	--�ԕi�q��
                    AND M.SoukoType = 8)
      AND (@F10 = 0 OR (@F10 = 1 AND DD.DeliveryNo IS NOT NULL))
      AND DS.DeleteDateTime IS Null
      ORDER BY DS.ExpectReturnDate desc, DS.JanCD
      ;

END

GO

--CREATE TYPE T_ShiireH AS TABLE
--    (
--    [PurchaseRows] [int],
--    [DisplayRows] [int],
--    
--    [SKUCD] [varchar](30) ,
--    [AdminNO] [int] ,
--    [JanCD] [varchar](13) ,
--    [ItemName] [varchar](80) NULL,
--    [ColorName] [varchar](20) ,
--    [SizeName] [varchar](20) ,
--    [Remark] [varchar](200) NULL,
--    
--    [PurchaseSu] [int] ,
--    [OldPurchaseSu] [int] ,
--    [TaniCD] [varchar](2) ,
--    [TaniName] [varchar](10) ,
--    [PurchaserUnitPrice] [money] ,
--    [CalculationGaku] [money] ,
--    [AdjustmentGaku] [money] ,
--    [PurchaseGaku] [money] ,
--    [PurchaseTax] [money] ,
----    [CommentOutStore] [varchar](80) ,
----    [CommentInStore] [varchar](80) ,
--
--    [OrderNO] [varchar](11) ,
--    [OrderRows] [int] ,
--    [DeliveryNo] [int],
--    [StockNO] [varchar](11) ,
--    [WarehousingNO] [int] ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE PRC_HenpinNyuuryoku
    (@OperateMode    int,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j
    @PurchaseNO   varchar(11),
    @StoreCD   varchar(4),
    @PurchaseDate  varchar(10),
    @PaymentPlanDate varchar(10),
    @StaffCD   varchar(10),
    @CalledVendorCD   varchar(13),
    @PayeeCD  varchar(13),
    @CalculationGaku money,
    @AdjustmentGaku money,
    @PurchaseGaku money,
    @PurchaseTax money,
    @CommentInStore varchar(80) ,

    @ExpectedDateFrom  varchar(10),
    @ExpectedDateTo  varchar(10),
    
    @Table  T_ShiireH READONLY,
    @Operator  varchar(10),
    @PC  varchar(30),
    @OutPurchaseNO varchar(11) OUTPUT
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
    DECLARE @Program varchar(100); 
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	SET @Program = 'HenpinNyuuryoku';	
	
    --�V�K--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '�V�K';

		--�yD_PayPlan�zInsert�@Table�]���d�l�d �x���\��
		INSERT INTO [D_PayPlan]
           ([PayPlanKBN]
           ,[Number]
           ,[StoreCD]
           ,[PayeeCD]
           ,[RecordedDate]
           ,[PayPlanDate]
           ,[HontaiGaku8]
           ,[HontaiGaku10]
           ,[TaxGaku8]
           ,[TaxGaku10]
           ,[PayPlanGaku]
           ,[PayConfirmGaku]
           ,[PayConfirmFinishedKBN]
           ,[PayCloseDate]
           ,[PayCloseNO]
           ,[Program]
           ,[PayConfirmFinishedDate]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
        VALUES(        
            1   --PayPlanKBN
           ,@PurchaseNO --Number
           ,@StoreCD
           ,@PayeeCD
           ,@PurchaseDate   --RecordedDate
           ,@PaymentPlanDate    --PayPlanDate
           ,0   --HontaiGaku8
           ,0   --HontaiGaku10
           ,0   --TaxGaku8
           ,0   --TaxGaku10
           ,(@PurchaseGaku + @PurchaseTax)*(-1)    --PayPlanGaku
           ,0   --PayConfirmGaku
           ,0   --PayConfirmFinishedKBN
           ,NULL    --PayCloseDate
           ,NULL    --PayCloseNO
           ,@Program
           ,NULL    --PayConfirmFinishedDate
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
        );
        
        --���O�ɍ̔Ԃ��ꂽ IDENTITY ��̒l���擾����
        DECLARE @PayPlanNO int;
        SET @PayPlanNO = @@IDENTITY;
        
        --�`�[�ԍ��̔�
        EXEC Fnc_GetNumber
            4,             --in�`�[��� 4
            @PurchaseDate, --in���
            @StoreCD,       --in�X��CD
            @Operator,
            @PurchaseNO OUTPUT
            ;
        
        IF ISNULL(@PurchaseNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --�yD_Purchase�zTable�]���d�l�`
        INSERT INTO [D_Purchase]
           ([PurchaseNO]
           ,[StoreCD]
           ,[PurchaseDate]
           ,[CancelFlg]
           ,[ProcessKBN]
           ,[ReturnsFlg]
           ,[VendorCD]
           ,[CalledVendorCD]
           ,[CalculationGaku]
           ,[AdjustmentGaku]
           ,[PurchaseGaku]
           ,[PurchaseTax]
           ,[TotalPurchaseGaku]
           ,[CommentOutStore]
           ,[CommentInStore]
           ,[ExpectedDateFrom]
           ,[ExpectedDateTo]
           ,[InputDate]
           ,[StaffCD]
           ,[PaymentPlanDate]
           ,[PayPlanNO]
           ,[OutputDateTime]
           ,[StockAccountFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     VALUES
           (@PurchaseNO
           ,@StoreCD
           ,convert(date,@PurchaseDate)
           ,1	--CancelFlg��
           ,3	--ProcessKBN��
           ,1	--ReturnsFlg��
           ,@CalledVendorCD		--��
           ,@CalledVendorCD
           ,@CalculationGaku * (-1)
           ,@AdjustmentGaku * (-1)
           ,@PurchaseGaku * (-1)
           ,@PurchaseTax * (-1)
           ,(@PurchaseGaku + @PurchaseTax) * (-1)	--TotalPurchaseGaku
           ,@CommentInStore	--CommentOutStore��
           ,@CommentInStore
           ,@ExpectedDateFrom	--ExpectedDateFrom
           ,@ExpectedDateTo	--ExpectedDateTo
           ,@SYSDATETIME	--InputDate
           ,@StaffCD
           ,@PaymentPlanDate
           ,@PayPlanNO
           ,NULL	--OutputDateTime
           ,1	--StockAccountFlg��

           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
           );               

        INSERT INTO [D_PurchaseDetails]
                   ([PurchaseNO]
                   ,[PurchaseRows]
                   ,[DisplayRows]
                   ,[ArrivalNO]
                   ,[SKUCD]
                   ,[AdminNO]
                   ,[JanCD]
                   ,[ItemName]
                   ,[ColorName]
                   ,[SizeName]
                   ,[Remark]
                   ,[PurchaseSu]
                   ,[TaniCD]
                   ,[TaniName]
                   ,[PurchaserUnitPrice]
                   ,[CalculationGaku]
                   ,[AdjustmentGaku]
                   ,[PurchaseGaku]
                   ,[PurchaseTax]
                   ,[TotalPurchaseGaku]
                   ,[CurrencyCD]
                   ,[CommentOutStore]
                   ,[CommentInStore]
                   ,[ReturnNO]
                   ,[ReturnRows]
                   ,[OrderUnitPrice]
                   ,[OrderNO]
                   ,[OrderRows]
                   ,[DifferenceFlg]
                   ,[DeliveryNo]

                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             SELECT @PurchaseNO                         
                   ,tbl.PurchaseRows                       
                   ,tbl.DisplayRows 

                   ,NULL	--ArrivalNO��
                   ,tbl.SKUCD
                   ,tbl.AdminNO
                   ,tbl.JanCD
                   ,tbl.ItemName
                   ,tbl.ColorName
                   ,tbl.SizeName
                   ,tbl.Remark	--Remark��
                   ,tbl.PurchaseSu * (-1)
                   ,tbl.TaniCD
                   ,tbl.TaniName
                   ,tbl.PurchaserUnitPrice
                   ,tbl.CalculationGaku * (-1)
                   ,tbl.AdjustmentGaku * (-1)
                   ,tbl.PurchaseGaku * (-1)
                   ,tbl.PurchaseTax * (-1)
                   ,(tbl.PurchaseGaku + tbl.PurchaseTax) * (-1) --TotalPurchaseGaku
                   ,(SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)	--CurrencyCD
                   ,tbl.Remark		--CommentOutStore
                   ,tbl.Remark		--CommentInStore
                   ,tbl.OrderNO	--ReturnNO��
                   ,tbl.OrderRows	--ReturnRows��
                   ,0	--OrderUnitPrice��
                   ,NULL	--OrderNO��
                   ,0	--OrderRows��
                   ,0	--DifferenceFlg��
                   ,tbl.DeliveryNo
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME

              FROM @Table tbl
              WHERE tbl.UpdateFlg = 0
              ;
    END
        
    --�ύX--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '�ύX';
        
        --�yD_PurchaseHistory�zInsert�@Table�]���d�l�b�@��
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;
        
        --�yD_Delivery�zUpdate	Table�]���d�l�e	��	�[�i��
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;
        
        UPDATE D_Purchase
           SET [StoreCD] = @StoreCD                         
              ,[PurchaseDate] = convert(date,@PurchaseDate)
              ,[VendorCD] = @CalledVendorCD
              ,[CalledVendorCD] = @CalledVendorCD
              ,[CalculationGaku] = @CalculationGaku * (-1)
              ,[AdjustmentGaku] = @AdjustmentGaku * (-1)
              ,[PurchaseGaku] = @PurchaseGaku * (-1)
              ,[PurchaseTax] = @PurchaseTax * (-1)
              ,[TotalPurchaseGaku] = (@PurchaseGaku + @PurchaseTax) * (-1)
              ,[CommentOutStore]  = @CommentInStore
              ,[CommentInStore]  = @CommentInStore
              ,[InputDate]  = @SYSDATETIME
              ,[StaffCD]         = @StaffCD
              ,[PaymentPlanDate] = @PaymentPlanDate      
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME
         WHERE PurchaseNO = @PurchaseNO
           ;
           
        UPDATE [D_PurchaseDetails]
           SET  [SKUCD]              = tbl.SKUCD
               ,[AdminNO]            = tbl.AdminNO
               ,[JanCD]              = tbl.JanCD
               ,[ItemName]           = tbl.ItemName
               ,[ColorName]          = tbl.ColorName
               ,[SizeName]           = tbl.SizeName
               ,[Remark]             = tbl.Remark
               ,[PurchaseSu]         = tbl.PurchaseSu * (-1)
               ,[TaniCD]             = tbl.TaniCD
               ,[TaniName]           = tbl.TaniName
               ,[PurchaserUnitPrice] = tbl.PurchaserUnitPrice
               ,[CalculationGaku]    = tbl.CalculationGaku * (-1)
               ,[AdjustmentGaku]     = tbl.AdjustmentGaku * (-1)
               ,[PurchaseGaku]       = tbl.PurchaseGaku * (-1)
               ,[PurchaseTax]        = tbl.PurchaseTax * (-1)
               ,[TotalPurchaseGaku]  = (tbl.PurchaseGaku + tbl.PurchaseTax) * (-1) --TotalPurchaseGaku
               ,[CurrencyCD]         = (SELECT M.CurrencyCD FROM M_Control AS M WHERE M.MainKey = 1)	--CurrencyCD
               ,[CommentOutStore]    = tbl.Remark
               ,[CommentInStore]     = tbl.Remark
               ,[DifferenceFlg]      = 1
               ,[DeliveryNo]         = tbl.DeliveryNo          
               ,[UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
        FROM D_PurchaseDetails
        INNER JOIN @Table tbl
         ON @PurchaseNO = D_PurchaseDetails.PurchaseNO
         AND tbl.PurchaseRows = D_PurchaseDetails.PurchaseRows
         AND tbl.UpdateFlg = 1
         ;
        
        --�yD_PayPlan�zUpdate�@Table�]���d�l�d �x���\��
        UPDATE [D_PayPlan]
           SET 
            [PayeeCD]            = @PayeeCD
           ,[RecordedDate]       = @PurchaseDate   --RecordedDate
           ,[PayPlanDate]        = @PaymentPlanDate    --PayPlanDate
           ,[PayPlanGaku]        = (@PurchaseGaku + @PurchaseTax) * (-1)    --PayPlanGaku
           ,[UpdateOperator]     =  @Operator  
           ,[UpdateDateTime]     =  @SYSDATETIME
        WHERE [Number] = @PurchaseNO
        AND [DeleteDateTime] IS NULL           
           ;
    END
    
    ELSE IF @OperateMode = 3 --�폜--
    BEGIN
        SET @OperateModeNm = '�폜';
        
        --�yD_PurchaseHistory�zInsert�@Table�]���d�l�b�@��
        EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,2  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;

        --�yD_Delivery�zUpdate	Table�]���d�l�e	��	�[�i��
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] - tbl.OldPurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;
         
        UPDATE [D_Purchase]
            SET [UpdateOperator]     =  @Operator  
               ,[UpdateDateTime]     =  @SYSDATETIME
               ,[DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [PurchaseNO] = @PurchaseNO
         ;

    END
    
    IF @OperateMode <= 2    --�V�K�E�C����
    BEGIN
    	--�yD_PurchaseHistory�zInsert�@Table�]���d�l�b�@��
    	EXEC INSERT_D_PurchaseHistory
            @PurchaseNO    -- varchar(11),
            ,1  --@RecoredKBN
            ,@SYSDATETIME   --  datetime,
            ,@Operator  --varchar(10),
            ;
        
        --�yD_Delivery�zUpdate	Table�]���d�l�e	��	�[�i��
        UPDATE [D_Delivery]
           SET [PurchaseSu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
              ,[MatchingDatetime] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
                                    THEN @SYSDATETIME ELSE NULL END)    --MatchingDatetime
              ,[MatchingFlg] = (CASE WHEN D_Delivery.[DeliverySu] = D_Delivery.[PurchaseSu] + tbl.PurchaseSu
                                    THEN 1 ELSE 0 END)	--MatchingFlg
              ,[UpdateOperator]     =  @Operator  
              ,[UpdateDateTime]     =  @SYSDATETIME        
        FROM D_Delivery
        INNER JOIN @Table tbl
         ON tbl.DeliveryNo = D_Delivery.DeliveryNo
         WHERE D_Delivery.DeleteDateTime IS NULL
         ;

        --�J�[�\����`
        DECLARE CUR_TABLE CURSOR FOR
            SELECT tbl.PurchaseRows, tbl.UpdateFlg
            FROM @Table AS tbl
            ORDER BY tbl.PurchaseRows
            ;
            
        DECLARE @tblPurchaseRows int;
        DECLARE @tblUpdateFlg int;
        
        --�J�[�\���I�[�v��
        OPEN CUR_TABLE;

        --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;
        
        --�f�[�^�̍s�������[�v���������s����
        WHILE @@FETCH_STATUS = 0
        BEGIN
        -- ========= ���[�v���̎��ۂ̏��� ��������===
            IF @tblUpdateFlg = 1
            BEGIN
            	--�yD_Stock�z           Update  Table�]���d�l�f��
                UPDATE [D_Stock] SET
                       [ReturnSu] = [ReturnSu] - tbl.OldPurchaseSu
                      ,[UpdateOperator]     =  @Operator  
                      ,[UpdateDateTime]     =  @SYSDATETIME
                      
                 FROM D_Stock AS DS
                 INNER JOIN @Table tbl
                 ON tbl.StockNO = DS.StockNO
                 AND tbl.PurchaseRows = @tblPurchaseRows
                 WHERE DS.DeleteDateTime IS NULL
                ;
            END
            
            --�yD_Stock�z           Update  Table�]���d�l�f��
            UPDATE [D_Stock] SET
                   [ReturnDate] = @PurchaseDate
                  ,[ReturnSu] = [ReturnSu] + tbl.PurchaseSu
                  ,[UpdateOperator]     =  @Operator  
                  ,[UpdateDateTime]     =  @SYSDATETIME
                  
             FROM D_Stock AS DS
             INNER JOIN @Table tbl
             ON tbl.StockNO = DS.StockNO
             AND tbl.PurchaseRows = @tblPurchaseRows
             WHERE DS.DeleteDateTime IS NULL
            ;

            IF @tblUpdateFlg = 1
            BEGIN
                --Table�]���d�l�g�A ��
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
	               ,[UnitPrice]
	               ,[Amount]
                   ,[Program]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime]
                   ,[DeleteOperator]
                   ,[DeleteDateTime])
               SELECT (CASE WHEN @PurchaseDate >= CONVERT(date,@SYSDATETIME)
               			THEN @PurchaseDate ELSE CONVERT(date,@SYSDATETIME) END) --WarehousingDate
                   ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                    WHERE M.StoreCD = @StoreCD
                    AND M.SoukoType = 8     --8:�ԕi�q��
                    AND M.ChangeDate <= convert(date,@PurchaseDate)
                    ORDER BY M.ChangeDate desc) AS SoukoCD
                   ,NULL    --RackNO
                   ,DW.StockNO
                   ,DW.JanCD
                   ,DW.AdminNO
                   ,DW.SKUCD
                   ,21   --WarehousingKBN
                   ,1  --DeleteFlg��
                   ,DW.Number  --Number
                   ,DW.NumberRow --NumberRow
                   ,DW.VendorCD
                   ,NULL    --ToStoreCD
                   ,NULL    --ToSoukoCD
                   ,NULL    --ToRackNO
                   ,NULL    --ToStockNO
                   ,NULL    --FromStoreCD
                   ,NULL    --FromSoukoCD]
                   ,NULL    --FromRackNO
                   ,NULL    --CustomerCD
                   ,DW.Quantity * (-1)   --Quantity
	               ,DW.UnitPrice
	               ,DW.Amount * (-1) 
                   ,@Program  --Program
                   
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   ,NULL
                   ,NULL
                 FROM D_Warehousing AS DW
                 INNER JOIN @Table tbl
                 ON tbl.WarehousingNO = DW.WarehousingNO
                 AND tbl.PurchaseRows = @tblPurchaseRows
                 WHERE DW.DeleteDateTime IS NULL
                  ;
             END

         	--Table�]���d�l�g
            --�yD_Warehousing�z
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
               ,[UnitPrice]
               ,[Amount]
               ,[Program]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
               ,[DeleteOperator]
               ,[DeleteDateTime])
            SELECT @PurchaseDate --WarehousingDate
               ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
                WHERE M.StoreCD = @StoreCD
                AND M.SoukoType = 8     --8:�ԕi�q��
                AND M.ChangeDate <= convert(date,@PurchaseDate)
                ORDER BY M.ChangeDate desc) AS SoukoCD
               ,NULL    --RackNO
               ,tbl.StockNO
               ,tbl.JanCD
               ,tbl.AdminNO
               ,tbl.SKUCD
               ,21   --WarehousingKBN
               ,0  --DeleteFlg
               ,@PurchaseNO  --Number
               ,tbl.PurchaseRows --NumberRow
               ,@CalledVendorCD
               ,NULL    --ToStoreCD
               ,NULL    --ToSoukoCD
               ,NULL    --ToRackNO
               ,NULL    --ToStockNO
               ,NULL    --FromStoreCD
               ,NULL    --FromSoukoCD]
               ,NULL    --FromRackNO
               ,NULL    --CustomerCD
               ,tbl.PurchaseSu   --Quantity
               ,tbl.PurchaserUnitPrice	--UnitPrice
               ,tbl.PurchaseGaku	--Amount
               ,@Program  --Program
               
               ,@Operator  
               ,@SYSDATETIME
               ,@Operator  
               ,@SYSDATETIME
               ,NULL
               ,NULL

              FROM @Table tbl
              WHERE tbl.PurchaseRows = @tblPurchaseRows
              ;

        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR_TABLE
        INTO @tblPurchaseRows, @tblUpdateFlg;

        END		--LOOP�̏I���
        
        --�J�[�\�������
        CLOSE CUR_TABLE;
        DEALLOCATE CUR_TABLE;

    END
    ELSE    --�폜
    BEGIN
        UPDATE [D_PurchaseDetails]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [PurchaseNO] = @PurchaseNO
         AND [DeleteDateTime] IS NULL
         ;

        --�yD_PayPlan�zDelete�@Table�]���d�l�d �x���\��
        UPDATE [D_PayPlan]
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         WHERE [Number] = @PurchaseNO
         AND [DeleteDateTime] IS NULL
         ;
         
       --�yD_Stock�z           Update  Table�]���d�l�e�A
        UPDATE D_Stock 
            SET [DeleteOperator]     =  @Operator  
               ,[DeleteDateTime]     =  @SYSDATETIME
         FROM D_Stock AS DS
         INNER JOIN @Table tbl
         ON tbl.StockNO = DS.StockNO
         WHERE DS.DeleteDateTime IS NULL
        ;
        
        --Table�]���d�l�g�A ��
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
           ,[UnitPrice]
           ,[Amount]
           ,[Program]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
       SELECT (CASE WHEN @PurchaseDate >= CONVERT(date,@SYSDATETIME)
       			THEN @PurchaseDate ELSE CONVERT(date,@SYSDATETIME) END) --WarehousingDate
           ,(SELECT top 1 M.SoukoCD FROM M_Souko AS M
            WHERE M.StoreCD = @StoreCD
            AND M.SoukoType = 8     --8:�ԕi�q��
            AND M.ChangeDate <= convert(date,@PurchaseDate)
            ORDER BY M.ChangeDate desc) AS SoukoCD
           ,NULL    --RackNO
           ,DW.StockNO
           ,DW.JanCD
           ,DW.AdminNO
           ,DW.SKUCD
           ,21   --WarehousingKBN
           ,1  --DeleteFlg��
           ,DW.Number  --Number
           ,DW.NumberRow --NumberRow
           ,DW.VendorCD
           ,NULL    --ToStoreCD
           ,NULL    --ToSoukoCD
           ,NULL    --ToRackNO
           ,NULL    --ToStockNO
           ,NULL    --FromStoreCD
           ,NULL    --FromSoukoCD]
           ,NULL    --FromRackNO
           ,NULL    --CustomerCD
           ,DW.Quantity * (-1)   --Quantity
           ,DW.UnitPrice
           ,DW.Amount * (-1)
           ,@Program  --Program
           
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL
           ,NULL
         FROM D_Warehousing AS DW
         INNER JOIN @Table tbl
         ON tbl.WarehousingNO = DW.WarehousingNO
         WHERE DW.DeleteDateTime IS NULL
          ;


    END
	
    --���������f�[�^�֍X�V
    SET @KeyItem = @PurchaseNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        @Program,
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutPurchaseNO = @PurchaseNO;
    
--<<OWARI>>
  return @W_ERR;

END

GO
