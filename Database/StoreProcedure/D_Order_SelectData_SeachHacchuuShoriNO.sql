 BEGIN TRY 
 Drop Procedure dbo.[D_Order_SelectData_SeachHacchuuShoriNO]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[D_Order_SelectData_SeachHacchuuShoriNO]
    (@p_StoreCD             varchar(4)
    ,@p_DateFrom            date
    ,@p_DateTo              date
    ,@p_InsertOperator      varchar(10)
    )AS
    
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT DODH.OrderProcessNO
          ,FORMAT(MAX(DODH.InsertDateTime),'yyyy/MM/dd HH:mm')InsertDateTime
          ,MAX(MSTF.StaffName) StaffName
          ,CASE WHEN MAX(DODH.OrderWayKBN) = 1 THEN 'Net' ELSE 'Fax' END OrderWayKBNName
          ,MAX(MVEN.VendorName) VendorName
    FROM D_Order DODH
    OUTER APPLY (SELECT *
                   FROM F_Staff(DODH.OrderDate) MSTF
                  WHERE MSTF.StaffCD = DODH.StaffCD)MSTF
    OUTER APPLY (SELECT *
                   FROM F_Vendor(DODH.OrderDate) MVEN
                  WHERE MVEN.VendorCD = DODH.OrderCD)MVEN
    WHERE DODH.DeleteDateTime IS NULL
    AND   DODH.OrderProcessNO IS NOT NULL
    AND   DODH.StoreCD = @p_StoreCD
    AND   DODH.OrderWayKBN in (1,2)
    AND   (@p_DateFrom IS NULL OR CAST(DODH.InsertDateTime as date) >= @p_DateFrom)
    AND   (@p_DateTo IS NULL OR CAST(DODH.InsertDateTime as date) <= @p_DateTo)
    AND   (@p_InsertOperator IS NULL OR DODH.InsertOperator = @p_InsertOperator)
    GROUP BY DODH.OrderProcessNO

END


