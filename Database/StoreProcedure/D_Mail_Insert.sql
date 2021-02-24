IF OBJECT_ID ( 'D_Mail_Insert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_Mail_Insert]
GO

--  ======================================================================
--       Program Call    EDIî≠íçì¸óÕ
--       Program ID      D_EDIOrder_Insert
--       Create date:    2019.11.16
--    ======================================================================

--********************************************--
--                                            --
--             ÉÅÅ[Éãä÷åWÉeÅ[ÉuÉãí«â¡         --
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
    
    --ÉÅÅ[ÉãÉJÉEÉìÉ^Å[çXêV
    SELECT @Counter = MailCounter + 1 
      FROM M_MailCounter
     WHERE MailCounterKey = 1;
     
    UPDATE M_MailCounter
       SET MailCounter = @Counter
     WHERE MailCounterKey = 1;
            
    --M_VendorFTPéÊìæ
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
     
    --ägí£éqÇ»ÇµÉtÉ@ÉCÉãñº
    SET @ExtIndex = CHARINDEX('.',REVERSE(@FileName));
    SET @FileNameNoExt = REVERSE(SUBSTRING(REVERSE(@FileName),@ExtIndex + 1, LEN(@FileName) - @ExtIndex));
    SET @Extention = REVERSE(SUBSTRING(REVERSE(@FileName),1, @ExtIndex));
    
    --M_VendorMailéÊìæ
    /*
    SELECT TOP 1 
           @AddressRows = ISNULL(MV.AddressRows,0)
          ,@AddressKBN = ISNULL(MV.AddressKBN,0)
          ,@Address = MV.Address
      FROM M_VendorMail MV
     WHERE MV.VendorCD = @VendorCD
       AND MV.ChangeDate <= @SYSDATE
     ORDER BY MV.ChangeDate DESC;
     */
     
     --M_StoreéÊìæ
     SELECT TOP 1
            @StorePlaceKBN = ST.StorePlaceKBN
       FROM D_EDIOrder DH
       LEFT JOIN M_Store ST ON DH.StoreCD = ST.StoreCD
                           AND ST.ChangeDate <= @SYSDATE
      WHERE DH.EDIOrderNo = @EDIOrderNo
      ORDER BY ST.ChangeDate DESC;     
            
    --ÅyD_MailÅzTableì]ëóédólÇc
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
      
    --ÅyD_MailAddressÅzTableì]ëóédólÇd
    INSERT INTO [dbo].[D_MailAddress]
       ([MailCounter]
       ,[AddressRows]
       ,[AddressKBN]
       ,[Address])
    SELECT
        @Counter
       ,ISNULL(MV1.AddressRows,0)
       ,ISNULL(MV1.AddressKBN,0)
       ,MV1.Address
    FROM M_VendorMail MV1
   INNER JOIN ( SELECT VendorCD , MAX(ChangeDate) AS ChangeDate
                  FROM M_VendorMail
                 WHERE VendorCD = @VendorCD
                   AND ChangeDate <= @SYSDATE
                 GROUP BY VendorCD
               ) MV2 ON MV1.VendorCD = MV2.VendorCD
                    AND MV1.ChangeDate = MV2.ChangeDate
   ;       
    
    --ÅyD_MailFileÅzTableì]ëóédólÇe
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
       ,@FileNameNoExt + '_' + FORMAT(@SYSDATETIME, 'yyyyMMddHHmmss') + @Extention
      );  
    
--<<OWARI>>
  return @W_ERR;

END
