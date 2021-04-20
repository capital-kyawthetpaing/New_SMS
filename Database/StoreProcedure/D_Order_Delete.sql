/****** Object:  StoredProcedure [dbo].[D_Order_Delete]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Order_Delete]
GO

/****** Object:  StoredProcedure [dbo].[D_ArrivalPlan_SelectData]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE D_Order_Delete
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）

    @Table  T_NyuukaN READONLY,
    @Operator  varchar(10),
    @PC  varchar(30)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
	
    --新規--
    IF @OperateMode = 1
    BEGIN
        SET @OperateModeNm = '新規';
    END
    
    --変更--
    ELSE IF @OperateMode = 2
    BEGIN
        SET @OperateModeNm = '変更';
    END
            
    --発注 D_Order
    DELETE FROM D_Order
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_Order.OrderNO)
    ;
    
    --発注 D_OrderDetails
    DELETE FROM D_OrderDetails
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_OrderDetails.OrderNO)
    ;
    
    --入荷予定情報　D_ArrivalPlan
    DELETE FROM D_ArrivalPlan
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.OrderNO = D_ArrivalPlan.Number)
    ;

	DELETE FROM D_Stock
    WHERE EXISTS(SELECT 1 FROM @Table AS tbl
                 WHERE tbl.StockNO = D_Stock.StockNO)
    ;
    
    --処理履歴データへ更新
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
