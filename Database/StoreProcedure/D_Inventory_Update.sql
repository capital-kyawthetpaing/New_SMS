 BEGIN TRY 
 Drop Procedure dbo.[D_Inventory_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[D_Inventory_Update]
    (@SoukoCD varchar(6),
    @InventoryDate varchar(10),

    @Table  T_Tanaoroshi READONLY,
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
	SET @OperateModeNm = '変更';
    SET @KeyItem = @SoukoCD + ' ' + @InventoryDate;
    
    UPDATE [D_Inventory]
    SET [ActualQuantity] = tbl.ActualQuantity
      ,[DifferenceQuantity] = tbl.ActualQuantity - D_Inventory.TheoreticalQuantity
      ,[UpdateOperator] =  @Operator  
      ,[UpdateDateTime] =  @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.RackNO = D_Inventory.RackNO
       AND tbl.AdminNO = D_Inventory.AdminNO
       AND D_Inventory.SoukoCD = @SoukoCD
       AND D_Inventory.InventoryDate = CONVERT(date,@InventoryDate)
    ;
	
	--L_LogInsert	Table転送仕様Ｚ
    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'TanaoroshiNyuuryoku',
        @PC,
        @OperateModeNm,
        @KeyItem;	--;

    
--<<OWARI>>
  return @W_ERR;

END


