 BEGIN TRY 
 Drop Procedure dbo.[D_Inventory_Update]
END try
BEGIN CATCH END CATCH 
DROP TYPE [dbo].[T_Tanaoroshi]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

--  ======================================================================
--       Program Call    棚卸入力
--       Program ID      TanaoroshiNyuuryoku
--       Create date:    2020.2.13
--    ======================================================================

CREATE TYPE T_Tanaoroshi AS TABLE
    (
    [GyoNO] [int],
    [RackNO] [varchar](11) ,
    [AdminNO] [int] ,
    [ActualQuantity] [int],

    [UpdateFlg][tinyint]
    )
GO

CREATE PROCEDURE D_Inventory_Update
   (@SoukoCD       varchar(6),
    @InventoryDate varchar(10),

    @Table         T_Tanaoroshi READONLY,
    @Operator      varchar(10),
    @PC            varchar(30)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR         tinyint;
    DECLARE @SYSDATETIME   datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem       varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '変更';
    SET @KeyItem = @SoukoCD + ' ' + @InventoryDate;
    
    UPDATE [D_Inventory]
    SET [ActualQuantity]     = tbl.ActualQuantity
       ,[DifferenceQuantity] = tbl.ActualQuantity - D_Inventory.TheoreticalQuantity
       ,[UpdateOperator]     =  @Operator  
       ,[UpdateDateTime]     =  @SYSDATETIME
    FROM @Table tbl
    WHERE tbl.RackNO = D_Inventory.RackNO
      AND tbl.AdminNO = D_Inventory.AdminNO
      AND D_Inventory.SoukoCD = @SoukoCD
      AND D_Inventory.InventoryDate = CONVERT(date,@InventoryDate)
    ;
    
    INSERT INTO [D_Inventory]
           ([SoukoCD]
           ,[RackNO]
           ,[InventoryDate]
           ,[SKUCD]
           ,[AdminNO]
           ,[JanCD]
           ,[TheoreticalQuantity]
           ,[ActualQuantity]
           ,[DifferenceQuantity]
           ,[InventoryNO]
           ,[ADDFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime]
           ,[DeleteOperator]
           ,[DeleteDateTime])
     SELECT
            @SoukoCD
           ,tbl.RackNO
           ,CONVERT(date,@InventoryDate)
           ,FS.SKUCD
           ,tbl.AdminNO
           ,FS.JanCD
           ,0 AS TheoreticalQuantity
           ,tbl.ActualQuantity
           ,tbl.ActualQuantity AS DifferenceQuantity
           ,NULL AS InventoryNO
           ,1
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           ,NULL                  
           ,NULL
    FROM @Table tbl
    INNER JOIN F_SKU(CONVERT(date,@InventoryDate)) AS FS
    ON FS.AdminNO = tbl.AdminNO
    WHERE NOT EXISTS(SELECT A.RackNO FROM D_Inventory AS A
                     WHERE A.SoukoCD = @SoukoCD
                       AND A.RackNO = tbl.RackNO
                       AND A.InventoryDate = CONVERT(date,@InventoryDate)
                       AND A.AdminNO = tbl.AdminNO )
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

GO
