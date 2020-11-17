
 BEGIN TRY 
 Drop Procedure dbo.D_EDIOrderDetails_SelectAll
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_EDIOrderDetails_SelectAll]    */
CREATE PROCEDURE D_EDIOrderDetails_SelectAll(
    -- Add the parameters for the stored procedure here
    @EDIImportNO  int,
    @ErrorKBN     TinyInt,
    @ChkAnswer    TinyInt,
    @ChkNoAnswer  TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    -- Insert statements for procedure here
    IF OBJECT_ID( N'[dbo]..[#TableForEDIKaitouNouki]', N'U' ) IS NOT NULL
      BEGIN
        DROP TABLE [#TableForEDIKaitouNouki];
      END

    --画面項目転送表02（詳細表示）
    --印刷項目転送表01
    SELECT DE.EDIOrderNO
          ,DE.EDIOrderRows
          ,DE.OrderNO
          ,DE.OrderRows
          ,DE.OrderLines
          ,DE.ArriveDate
          ,DE.OrderKBN
          ,DE.MakerItemKBN
          ,(SELECT top 1 M.MakerItem
            FROM M_SKU AS M
            WHERE M.AdminNO = DE.AdminNO
            AND M.ChangeDate <= DE.OrderDate
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc) AS MakerItem
          ,DE.SKUCD
          ,DE.SizeName
          ,DE.ColorName
          ,DE.TaniCD
          ,DE.OrderUnitPrice
          ,DE.OrderPriceWithoutTax
          ,DE.BrandName
          ,(SELECT top 1 M.SKUName
            FROM M_SKU AS M
            WHERE M.AdminNO = DE.AdminNO
            AND M.ChangeDate <= DE.OrderDate
            AND M.DeleteFlg = 0
            ORDER BY M.ChangeDate desc) AS SKUName
          ,DE.AdminNO
          ,DE.JanCD
          ,DE.OrderSu
          ,DE.OrderGroupNO
          ,DE.AnswerSu
          ,DE.NextDate
          ,DE.OrderGroupRows
          ,DE.ErrorMessage
          ,DE.InsertOperator
          ,DE.InsertDateTime
          ,DE.UpdateOperator
          ,DE.UpdateDateTime
          ,(CASE WHEN DD.ArrivalPlanDate IS NULL THEN '' ELSE CONVERT(varchar,DD.ArrivalPlanDate,111) END) AS ArrivalPlanDate
          ,DD.ArrivalPlanSu
          ,DD.ErrorKBN
          ,DD.ErrorText
          ,DD.EDIImportNO
          ,(CASE WHEN DH.OrderDate IS NULL THEN '' ELSE CONVERT(varchar,DH.OrderDate,111) END)AS OrderDate
          
          ,1 AS DelFlg
          
    INTO #TableForEDIKaitouNouki
    FROM D_EDIDetail AS DD
    LEFT OUTER JOIN D_EDIOrderDetails AS DE
    ON DE.OrderNO = DD.OrderNO
    AND DE.OrderRows = DD.OrderRows
    LEFT OUTER JOIN D_OrderDetails AS DM
    ON DM.OrderNO = DE.OrderNO
    AND DM.OrderRows = DE.OrderRows
    AND DM.DeleteDatetime IS NULL
    LEFT OUTER JOIN D_Order AS DH
    ON DH.OrderNO = DM.OrderNO
    AND DH.DeleteDatetime IS NULL
--    INNER JOIN D_EDI AS D
--    ON D.EDIImportNO = DD.EDIImportNO
    
    WHERE DD.EDIImportNO = @EDIImportNO
    ;
--    ALTER TABLE [#TableForEDIKaitouNouki] ADD PRIMARY KEY CLUSTERED ([EDIImportNO], [EDIOrderNO], [EDIOrderRows])
--    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
--    ;

    IF @ErrorKBN = 1 OR @ChkAnswer = 1 OR @ChkNoAnswer = 1
    BEGIN
        UPDATE #TableForEDIKaitouNouki
        SET DelFlg = 1
        ;
        
        IF @ErrorKBN = 1
        BEGIN
            UPDATE #TableForEDIKaitouNouki
            SET DelFlg = 0
            WHERE ErrorKBN <> 0
        END
        
        IF @ChkAnswer = 1
        BEGIN
            UPDATE #TableForEDIKaitouNouki
            SET DelFlg = 0
            WHERE ArrivalPlanDate <> ''
        END
    
        IF @ChkNoAnswer = 1
        BEGIN
            UPDATE #TableForEDIKaitouNouki
            SET DelFlg = 0
            WHERE ArrivalPlanDate = ''
        END
        
        DELETE FROM #TableForEDIKaitouNouki
        WHERE DelFlg = 1
        ;
    END

    SELECT *
    FROM #TableForEDIKaitouNouki
    ORDER BY EDIImportNO
    ;	
END

GO
