IF OBJECT_ID ( 'D_Mail_Insert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Mail_Insert]
GO
IF OBJECT_ID ( 'D_EDIOrder_UpdateOutputDateTime', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_EDIOrder_UpdateOutputDateTime]
GO
IF OBJECT_ID ( 'PRC_EDIOrder_Insert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_EDIOrder_Insert]
GO

IF OBJECT_ID ( 'PRC_EDIOrder_MailInsert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_EDIOrder_MailInsert]
GO


--  ======================================================================
--       Program Call    EDI発注入力
--       Program ID      D_EDIOrder_Insert
--       Create date:    2019.11.16
--    ======================================================================

--********************************************--
--                                            --
--             メール関係テーブル追加         --
--                                            --
--********************************************--
CREATE PROCEDURE D_Mail_Insert
    (@EDIOrderNo   varchar(11),
     @SYSDATETIME  datetime,
     @Operator     varchar(10)
)AS

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATE date;  

    SET @W_ERR = 0;
    SET @SYSDATE = CONVERT(date, @SYSDATETIME); 
    
    DECLARE @Counter bigint;
    DECLARE @StoreCD varchar(4);
    DECLARE @VendorCD varchar(13);
    DECLARE @MailPatternCD varchar(5);
    DECLARE @MailTitle varchar(50);
    DECLARE @MailPriority tinyint;    
    DECLARE @SenderMailAddress varchar(200);    
    DECLARE @MailText varchar(5000);    
    DECLARE @StorePlaceKBN tinyint;
    DECLARE @AddressRows int;
    DECLARE @AddressKBN tinyint;
    DECLARE @Address varchar(100);  
    DECLARE @CreateServer varchar(200);  
    DECLARE @CreateFolder varchar(200);  
    DECLARE @FileName varchar(100);    
    DECLARE @Extention varchar(10);
    DECLARE @ExtIndex tinyint;
    DECLARE @FileNameNoExt varchar(100);
    
    SET @MailPriority = 0;
    SET @StorePlaceKBN = 0;
    SET @AddressRows = 0;
    SET @AddressKBN = 0;
    
    --メールカウンター更新
    SELECT @Counter = MailCounter + 1 
      FROM M_MailCounter
     WHERE MailCounterKey = 1;
     
    UPDATE M_MailCounter
       SET MailCounter = @Counter
     WHERE MailCounterKey = 1;
            
    --M_VendorFTP取得
    SELECT TOP 1 
           @VendorCD = DH.VendorCD
          ,@StoreCD = DH.StoreCD
          ,@MailPatternCD = MV.MailPatternCD 
          ,@MailTitle = MV.MailTitle
          ,@MailPriority = ISNULL(MV.MailPriority,0)
          ,@SenderMailAddress = MV.SenderMailAddress
          ,@MailText = MP.MailText
          ,@CreateServer = MV.CreateServer
          ,@CreateFolder = MV.CreateFolder
          ,@FileName = MV.FileName
      FROM D_EDIOrder DH
      LEFT JOIN M_VendorFTP MV ON DH.VendorCD = MV.VendorCD
                              AND MV.ChangeDate <= @SYSDATE
      LEFT JOIN M_MailPattern MP ON MV.MailPatternCD = MP.MailPatternCD
     WHERE DH.EDIOrderNo = @EDIOrderNo
     ORDER BY MV.ChangeDate DESC;
     
    --拡張子なしファイル名
    SET @ExtIndex = CHARINDEX('.',REVERSE(@FileName));
    SET @FileNameNoExt = REVERSE(SUBSTRING(REVERSE(@FileName),@ExtIndex + 1, LEN(@FileName) - @ExtIndex));
    SET @Extention = REVERSE(SUBSTRING(REVERSE(@FileName),1, @ExtIndex));
    
    --M_VendorMail取得
    SELECT TOP 1 
           @AddressRows = ISNULL(MV.AddressRows,0)
          ,@AddressKBN = ISNULL(MV.AddressKBN,0)
          ,@Address = MV.Address
      FROM M_VendorMail MV
     WHERE MV.VendorCD = @VendorCD
       AND MV.ChangeDate <= @SYSDATE
     ORDER BY MV.ChangeDate DESC;
     
     --M_Store取得
     SELECT TOP 1
            @StorePlaceKBN = ST.StorePlaceKBN
       FROM D_EDIOrder DH
       LEFT JOIN M_Store ST ON DH.StoreCD = ST.StoreCD
                           AND ST.ChangeDate <= @SYSDATE
      WHERE DH.EDIOrderNo = @EDIOrderNo
      ORDER BY ST.ChangeDate DESC;     
            
    --【D_Mail】Table転送仕様Ｄ
    INSERT INTO [dbo].[D_Mail]
       ([MailCounter]
       ,[MailType]
       ,[MailKBN]
       ,[Number]
       ,[MailNORows]
       ,[MailDateTime]
       ,[StaffCD]
       ,[ContactKBN]
       ,[MailPatternCD]
       ,[MailSubject]
       ,[MailPriority]
       ,[ReMailFlg]
       ,[UnitKBN]
       ,[SendedDateTime]
       ,[SenderKBN]
       ,[SenderCD]
       ,[SenderAddress]
       ,[MailContent]
       ,[InsertOperator]
       ,[InsertDateTime]
       ,[UpdateOperator]
       ,[UpdateDateTime])
    VALUES
       (@Counter
       ,7
       ,71
       ,@EDIOrderNo
       ,1
       ,@SYSDATETIME
       ,@Operator
       ,1
       ,@MailPatternCD
       ,@MailTitle
       ,@MailPriority
       ,0
       ,2
       ,NULL
       ,CASE WHEN @StorePlaceKBN = 1 THEN 1 ELSE 2 END
       ,@StoreCD
       ,@SenderMailAddress
       ,@MailText
       ,@Operator  
       ,@SYSDATETIME
       ,@Operator  
       ,@SYSDATETIME
      );
      
    --【D_MailAddress】Table転送仕様Ｅ
    INSERT INTO [dbo].[D_MailAddress]
       ([MailCounter]
       ,[AddressRows]
       ,[AddressKBN]
       ,[Address])
    VALUES
       (@Counter
       ,@AddressRows
       ,@AddressKBN
       ,@Address
      );       
    
    --【D_MailFile】Table転送仕様Ｆ
    INSERT INTO [dbo].[D_MailFile]
       ([MailCounter]
       ,[FileRows]
       ,[CreateServer]
       ,[CreateFolder]
       ,[FileName])
    VALUES
       (@Counter
       ,1
       ,@CreateServer
       ,@CreateFolder
       ,@FileNameNoExt + FORMAT(@SYSDATETIME, 'yyyyMMddHHmmss') + @Extention
      );  
    
--<<OWARI>>
  return @W_ERR;

END

GO

--********************************************--
--                                            --
--           CSV出力済みフラグ更新            --
--                                            --
--********************************************--
CREATE PROCEDURE D_EDIOrder_UpdateOutputDateTime(
     @SYSDATETIME varchar(19)
)AS
BEGIN

    SET NOCOUNT ON;

    -- CSV出力済みフラグ
    UPDATE D_EDIOrder
       SET OutputDatetime = CONVERT(datetime, @SYSDATETIME)
      FROM D_EDIOrder DH
     WHERE DH.OutputDatetime IS NULL
END

GO

--********************************************--
--                                            --
--              未出力分処理                  --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_EDIOrder_Insert
    (@StoreCD  varchar(4),
     @OrderDateFrom  varchar(10),
     @OrderDateTo  varchar(10),    
     @StaffCD  varchar(10),
     @OrderCD  varchar(13),
     @OrderNO varchar(11),
     @ChkMisyonin varchar(1),
     @Operator  varchar(10),
     @PC        varchar(30),     
     @SYSDATETIME  datetime
)AS

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATE date;    
    DECLARE @KeyItem varchar(100);

    SET @W_ERR = 0;
    SET @SYSDATE = CONVERT(date, @SYSDATETIME); 
    SET @KeyItem = '';
    
    -- データ取得
    DECLARE @OrderTbl table
    (
        [StoreCD] [varchar](4) ,
        [VendorCD] [varchar](13) ,
        [SoukoCD] [varchar](6) ,
        [OrderNO] [varchar](11) ,
        [OrderRows] [int],
        [MakerItem] [varchar](50) ,
        [SizeName] [varchar](20) ,
        [ColorName] [varchar](20) ,
        [TaniCD] [varchar](2) ,
        [BrandKana] [varchar](20) ,
        [AdminNO] [int] ,
        [JanCD] [varchar](13) ,
        [OrderSu] [int] 
    )
    
    INSERT INTO @OrderTbl EXEC D_Order_SelectAllForEDIHacchuu 
                                      @StoreCD
                                     ,@OrderDateFrom
                                     ,@OrderDateTo
                                     ,@StaffCD
                                     ,@OrderCD
                                     ,@OrderNO
                                     ,@ChkMisyonin
    
    DECLARE @VendorCD varchar(10);
    DECLARE @SoukoCD varchar(6);
    DECLARE @EDIOrderNO varchar(11);
    DECLARE @VCapitalCD varchar(13);
    DECLARE @VCapitalName varchar(20);
    DECLARE @VOrderCD varchar(13);
    DECLARE @VOrderName varchar(20);
    DECLARE @VSalesCD varchar(13);
    DECLARE @VSalesName varchar(20);
    DECLARE @VDestinationCD varchar(13);
    DECLARE @VDestinationName varchar(20);
    
    --カーソル定義①
    DECLARE CUR_AAA CURSOR FOR
        SELECT tbl.VendorCD
        FROM @OrderTbl AS tbl
        GROUP BY tbl.VendorCD
        ORDER BY tbl.VendorCD;
    
    --カーソルオープン①
    OPEN CUR_AAA;
    
    --最初の1行目を取得して変数へ値をセット
    FETCH NEXT FROM CUR_AAA
    INTO  @VendorCD;
     
    --データの行数分ループ処理を実行する
    WHILE @@FETCH_STATUS = 0
    BEGIN
            
        --伝票番号採番
        EXEC Fnc_GetNumber
            23,             --in伝票種別 23
            @SYSDATE,       --in基準日
            @StoreCD,       --in店舗CD
            @Operator,
            @EDIOrderNO OUTPUT
            ;
        
        IF ISNULL(@EDIOrderNO,'') = ''
        BEGIN
            SET @W_ERR = 1;
            RETURN @W_ERR;
        END
        
        --M_VendorStoreFTP取得
        SELECT TOP 1 
               @VCapitalCD = SkenCapitalCD 
              ,@VCapitalName = SkenCapitalName
              ,@VOrderCD = SkenOrderCD
              ,@VOrderName = SkenOrderName
              ,@VSalesCD = SkenSalesCD
              ,@VSalesName = SkenSalesName
              --,@VDestinationCD = SkenDestinationCD
              --,@VDestinationName = SkenDestinationName
          FROM M_VendorStoreFTP 
         WHERE VendorCD = @VendorCD
           AND StoreCD = @StoreCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
         
        --M_VendorSoukoFTP取得
        /*
        SET @VDestinationCD = '';
        SET @VDestinationName = '';
        
        SELECT TOP 1
               @VDestinationCD = SkenDestinationCD
              ,@VDestinationName = SkenDestinationName
          FROM M_VendorSoukoFTP 
         WHERE VendorCD = @VendorCD
           AND StoreCD = @StoreCD
           AND SoukoCD = @SoukoCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
        */
        
        --【D_EDIOrder】Table転送仕様Ａ
        INSERT INTO [D_EDIOrder]
            ([EDIOrderNO]
            ,[StoreCD]
            ,[OrderDate]
            ,[VendorCD]
            ,[RecordKBN]
            ,[DataKBN]
            ,[CapitalCD]
            ,[CapitalName]
            ,[OrderCD]
            ,[OrderName]
            ,[SalesCD]
            ,[SalesName]
            ,[DestinationCD]
            ,[DestinationName]
            ,[OutputDatetime]
            ,[InsertOperator]
            ,[InsertDateTime]
            ,[UpdateOperator]
            ,[UpdateDateTime]
            )
        VALUES
           (@EDIOrderNO     
           ,@StoreCD                          
           ,@SYSDATE
           ,LEFT(@VendorCD,10)
           ,'B'
           ,'01'
           ,@VCapitalCD
           ,@VCapitalName
           ,@VOrderCD
           ,@VOrderName
           ,@VSalesCD
           ,@VSalesName
           ,NULL
           ,NULL
           ,NULL   
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
           );               

        
        --【D_EDIOrderDetails】Table転送仕様Ｂ
        INSERT INTO [dbo].[D_EDIOrderDetails]
           ([EDIOrderNO]
           ,[EDIOrderRows]
           ,[OrderNO]
           ,[OrderRows]
           ,[OrderLines]
           ,[OrderDate]
           ,[ArriveDate]
           ,[OrderKBN]
           ,[MakerItemKBN]
           ,[MakerItem]
           ,[SKUCD]
           ,[SizeName]
           ,[ColorName]
           ,[TaniCD]
           ,[OrderUnitPrice]
           ,[OrderPriceWithoutTax]
           ,[BrandName]
           ,[SKUName]
           ,[AdminNO]
           ,[JanCD]
           ,[OrderSu]
           ,[OrderGroupNO]
           ,[AnswerSu]
           ,[NextDate]
           ,[OrderGroupRows]
           ,[ErrorMessage]
           ,[DestinationCD]
           ,[DestinationName]
           ,[InsertOperator]
           ,[InsertDateTime]
           ,[UpdateOperator]
           ,[UpdateDateTime])
        SELECT
            @EDIOrderNO
           ,Row
           ,RIGHT(OrderNO,10)
           ,OrderRows
           ,1
           ,@SYSDATE
           ,NULL
           ,'2'
           ,NULL
           ,OrderNO
           ,MakerItem
           ,LEFT(SizeName,5)
           ,LEFT(ColorName,10)
           ,TaniCD
           ,0
           ,0
           ,LEFT(BrandKana,10)
           ,LEFT(OrderNO + SPACE(11),11) + '-' + RIGHT('000' + CAST(Row AS VARCHAr(3)),3)
           ,AdminNO
           ,JanCD
           ,OrderSu
           ,@EDIOrderNO
           ,0
           ,NULL
           ,Row
           ,NULL
           ,(SELECT TOP 1 SkenDestinationCD
               FROM M_VendorSoukoFTP 
              WHERE VendorCD = tbl.VendorCD
                AND StoreCD = tbl.StoreCD
                AND SoukoCD = tbl.SoukoCD
                AND ChangeDate <= @SYSDATE
              ORDER BY ChangeDate DESC
            )
           ,(SELECT TOP 1 SkenDestinationName
               FROM M_VendorSoukoFTP 
              WHERE VendorCD = tbl.VendorCD
                AND StoreCD = tbl.StoreCD
                AND SoukoCD = tbl.SoukoCD
                AND ChangeDate <= @SYSDATE
              ORDER BY ChangeDate DESC
            )
           ,@Operator  
           ,@SYSDATETIME
           ,@Operator  
           ,@SYSDATETIME
        FROM (SELECT *, ROW_NUMBER() OVER(ORDER BY OrderNO) AS Row
                FROM @OrderTbl
              WHERE StoreCD = @StoreCD
                AND VendorCD = @VendorCD
             ) AS tbl
        ORDER BY tbl.OrderNO;
        
        
        --【D_OrderDetails】テーブル転送仕様C
        UPDATE D_OrderDetails
           SET EDIOutputDatetime = @SYSDATETIME
          FROM D_OrderDetails DM
         INNER JOIN @OrderTbl tbl ON tbl.OrderNO = DM.OrderNO
                                 AND tbl.OrderRows = DM.OrderRows
         WHERE tbl.StoreCD = @StoreCD
           AND tbl.VendorCD = @VendorCD;
        
        --M_Vendor取得
        DECLARE @EDIFlg tinyint;
        
        SELECT TOP 1 
               @EDIFlg = EDIFlg 
          FROM M_Vendor 
         WHERE VendorCD = @VendorCD
           AND ChangeDate <= @SYSDATE
         ORDER BY ChangeDate DESC;
           
       --メール関係テーブル追加
        IF @EDIFlg = 2 
        BEGIN
            EXEC D_Mail_Insert
                @EDIOrderNo,
                @SYSDATETIME,
                @Operator;
        END
       
        --処理履歴テーブルには最初のEDI発注番号をセット
        IF @KeyItem = ''
            SET @KeyItem = @EDIOrderNO;
    -- ========= ループ内の実際の処理 ここまで===

        --次の行のデータを取得して変数へ値をセット
        FETCH NEXT FROM CUR_AAA
        INTO  @VendorCD;

    END
    
    --処理履歴データへ更新
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'EDIHacchuu',
        @PC,
        NULL,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END

GO

--********************************************--
--                                            --
--              再出力分処理                  --
--                                            --
--********************************************--
CREATE PROCEDURE PRC_EDIOrder_MailInsert
    (@EDIOrderNo  varchar(13),
     @Operator    varchar(10),
     @PC          varchar(30),
     @SYSDATETIME  datetime
)AS

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATE date;    
    DECLARE @KeyItem varchar(100);

    SET @W_ERR = 0;
    SET @SYSDATE = CONVERT(date, @SYSDATETIME); 
    SET @KeyItem = '';
    
    --M_Vendor取得
    DECLARE @EDIFlg tinyint;
    DECLARE @VendorCD varchar(13);
    
    SELECT TOP 1 
           @EDIFlg = MV.EDIFlg 
          ,@VendorCD = DH.VendorCD
      FROM D_EDIOrder DH
     INNER JOIN M_Vendor MV ON DH.VendorCD = MV.VendorCD
                           AND ChangeDate <= @SYSDATE
     WHERE DH.EDIOrderNo = @EDIOrderNo       
     ORDER BY MV.ChangeDate DESC;
    
    --メール関係テーブル追加
    IF @EDIFlg = 2 
    BEGIN
        EXEC D_Mail_Insert
            @EDIOrderNo,
            @SYSDATETIME,
            @Operator;
    END
    
    --処理履歴データへ更新
    SET @KeyItem = @EDIOrderNO;
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'EDIHacchuu',
        @PC,
        NULL,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END

GO

