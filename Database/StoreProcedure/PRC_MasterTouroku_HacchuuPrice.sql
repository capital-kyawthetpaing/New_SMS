IF OBJECT_ID ( 'PRC_MasterTouroku_HacchuuPrice', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_MasterTouroku_HacchuuPrice]
GO
IF EXISTS (select * from sys.table_types where name = 'T_ItmTanka')
    Drop TYPE dbo.[T_ItmTanka]
GO

IF EXISTS (select * from sys.table_types where name = 'T_SkuTanka')
    Drop TYPE dbo.[T_SkuTanka]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    仕入先別発注単価マスタ
--       Program ID      MasterTouroku_HacchuuPrice
--       Create date:    2020.05.11
--    ======================================================================

CREATE TYPE T_ItmTanka AS TABLE
    (
    [Chk] [varchar](1) ,
    [VendorCD] [varchar](13),
    [StoreCD] [varchar](4),
    [ITEMCD] [varchar](30),
    [ITemName] [varchar](100),
    [MakerItem] [varchar](30),
    [BrandCD] [varchar](6),
    [BrandName] [varchar](30),
    [SportsCD] [varchar](6),
    [SportsName] [varchar](100),
    [SegmentCD] [varchar](6),
    [SegmentName] [varchar](100),
    [LastYearTerm] [varchar](6),
    [LastSeason] [varchar](6),
    [ChangeDate] [date],
    [Rate] [decimal](5, 2) ,
    [PriceOutTax] [money] ,
    [PriceWithoutTax] [money] ,
    [InsertOperator] [varchar](10),
    [InsertDateTime] [datetime],
    [UpdateOperator] [varchar](10),
    [UpdateDateTime] [datetime],
    [OldRate] [decimal](5, 2) ,
    [DelFlg] [varchar](1) ,
    [TempKey] [int]
    )
GO

CREATE TYPE T_SkuTanka AS TABLE
    (
    [Chk] [varchar](1) ,
    [VendorCD] [varchar](13),
    [StoreCD] [varchar](4),
    [ITEMCD] [varchar](30),
    [ITemName] [varchar](100),
    [AdminNO] [int],
    [SKUCD] [varchar](30),    
    [SizeName] [varchar](20),
    [ColorName] [varchar](20),
    [MakerItem] [varchar](30),
    [BrandCD] [varchar](6),
    [SportsCD] [varchar](6),
    [SegmentCD] [varchar](6),
    [LastYearTerm] [varchar](6),
    [LastSeason] [varchar](6),
    [ChangeDate] [date],
    [Rate] [decimal](5, 2) ,
    [PriceOutTax] [money] ,
    [PriceWithoutTax] [money] ,
    [InsertOperator] [varchar](10),
    [InsertDateTime] [datetime],
    [UpdateOperator] [varchar](10),
    [UpdateDateTime] [datetime],
    [OldRate] [decimal](5, 2) ,
    [DelFlg] [varchar](1) ,
    [TempKey] [int]
    )
GO

--****************************************--
--                                        --
--            登録処理                    --
--                                        --
--****************************************--
CREATE PROCEDURE [dbo].[PRC_MasterTouroku_HacchuuPrice]
    (@VendorCD    varchar(13),
    @OldITMTable  T_ItmTanka READONLY,
    @OldSKUTable  T_SkuTanka READONLY,
    @ITMTable     T_ItmTanka READONLY,
    @SKUTable     T_SkuTanka READONLY,
    @Operator     varchar(10),
    @PC  varchar(30),
    
    @OutVendorCD varchar(13) OUTPUT
)AS

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(10);
    DECLARE @KeyItem varchar(100);
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

    -- 【テーブル転送仕様C】M_ItemOrderPrice ITEM発注単価マスタ 
    -- 削除
    DELETE IP
    FROM   M_ItemOrderPrice IP
    INNER JOIN @OldITMTable WK ON IP.VendorCD = WK.VendorCD
                              AND IP.StoreCD = WK.StoreCD
                              AND IP.MakerItem = WK.MakerItem
                              AND IP.ChangeDate = WK.ChangeDate
    ;
    
    -- 追加
    INSERT INTO [M_ItemOrderPrice]
           ([VendorCD]
           ,[StoreCD]
           ,[MakerItem]
           ,[ChangeDate]
           ,[Rate]
           ,[PriceWithoutTax]
           ,[DeleteFlg]
           ,[UsedFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     SELECT
            VendorCD
           ,StoreCD
           ,MakerItem
           ,ChangeDate
           ,Rate
           ,PriceWithoutTax
           ,0
           ,0
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
    FROM @ITMTable
    ;

    -- 【テーブル転送仕様D】 M_JANOrderPrice JAN発注単価マスタ
    -- 削除
    DELETE JP
    FROM   M_JANOrderPrice JP 
    INNER JOIN @OldSKUTable WK ON JP.VendorCD = WK.VendorCD
                              AND JP.StoreCD = WK.StoreCD
                              AND JP.AdminNO = WK.AdminNO
                              AND JP.ChangeDate = WK.ChangeDate
    ;
    
    -- 追加
    INSERT INTO [dbo].[M_JANOrderPrice]
           ([VendorCD]
           ,[StoreCD]
           ,[AdminNO]
           ,[ChangeDate]
           ,[SKUCD]
           ,[Rate]
           ,[PriceWithoutTax]
           ,[DeleteFlg]
           ,[UsedFlg]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
     SELECT
            VendorCD
           ,StoreCD
           ,AdminNO
           ,ChangeDate
           ,SKUCD
           ,Rate
           ,PriceWithoutTax
           ,0
           ,0
           ,InsertOperator
           ,InsertDateTime
           ,UpdateOperator
           ,UpdateDateTime
    FROM @SKUTable
    ;
    
    --処理履歴データへ更新
    SET @KeyItem = @VendorCD;
            
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'MasterTouroku_HacchuuPrice',
        @PC,
        @OperateModeNm,
        @KeyItem;
 
--<<OWARI>>
  return @W_ERR;

END


