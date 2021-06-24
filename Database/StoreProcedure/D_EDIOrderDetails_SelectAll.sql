

/****** Object:  StoredProcedure [dbo].[D_EDIOrderDetails_SelectAll]    Script Date: 2021/06/24 19:11:54 ******/
DROP PROCEDURE [dbo].[D_EDIOrderDetails_SelectAll]
GO

/****** Object:  StoredProcedure [dbo].[D_EDIOrderDetails_SelectAll]    Script Date: 2021/06/24 19:11:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_EDIOrderDetails_SelectAll]    */
CREATE PROCEDURE [dbo].[D_EDIOrderDetails_SelectAll](
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

    --��ʍ��ړ]���\02�i�ڍו\���j
    --������ړ]���\01
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
    OUTER APPLY (Select * from D_EDIOrderDetails D1 where D1.EDIOrderNO = (Select max(EDIOrderNO) from D_EDIOrderDetails Where OrderNO = DD.OrderNO AND OrderRows = DD.OrderRows group by OrderNO,OrderRows) and D1.OrderNO = DD.OrderNO and D1.OrderRows = DD.OrderRows) as DE
    LEFT OUTER JOIN D_OrderDetails AS DM
    ON DM.OrderNO = DE.OrderNO
    AND DM.OrderRows = DE.OrderRows
    AND DM.DeleteDatetime IS NULL
    LEFT OUTER JOIN D_Order AS DH
    ON DH.OrderNO = DM.OrderNO
    AND DH.DeleteDatetime IS NULL
    
    WHERE DD.EDIImportNO = @EDIImportNO
	Order by DD.EDIImportNO,DD.EDIImportRows
    ;

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


