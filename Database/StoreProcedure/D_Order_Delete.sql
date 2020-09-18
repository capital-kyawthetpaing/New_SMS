/****** Object:  StoredProcedure [dbo].[D_Order_Delete]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Order_Delete]
GO

/****** Object:  StoredProcedure [dbo].[D_ArrivalPlan_SelectData]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE D_Order_Delete
    (@OperateMode    int,                 -- �����敪�i1:�V�K 2:�C�� 3:�폜�j

    @Table  T_Nyuuka READONLY,
    @Operator  varchar(10),
    @PC  varchar(30)
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	
    --�V�K--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '�V�K';
    END
    
    --�ύX--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '�ύX';
    END
            
    --���� D_Order
    DELETE FROM D_Order
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_Order.OrderNO)
    ;
    
    --���� D_OrderDetails
    DELETE FROM D_OrderDetails
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_OrderDetails.OrderNO)
    ;
    
    --���ח\����@D_ArrivalPlan
    DELETE FROM D_ArrivalPlan
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_ArrivalPlan.Number)
    ;

	DELETE FROM D_Stock
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.StockNO = D_Stock.StockNO)
    ;
    
    --���������f�[�^�֍X�V
    SET @KeyItem = NULL;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NyuukaNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END

GO
