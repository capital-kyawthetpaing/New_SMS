DROP  PROCEDURE [dbo].[PRC_NayoseSyoriAll]
GO

--  ======================================================================
--       Program Call    名寄せ結果登録
--       Program ID      NayoseSyoriAll
--       Create date:    2021.5.31
--    ======================================================================

CREATE PROCEDURE PRC_NayoseSyoriAll
    (@Operator    varchar(10),
    @PC          varchar(30),
    @OutJuchuuNo varchar(11) OUTPUT
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;
    DECLARE @OperateModeNm varchar(20);
    DECLARE @KeyItem varchar(100);
        
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();
    SET @OperateModeNm = '名寄せ処理(全顧客)';
    
        SELECT DH.JuchuuNO
               ,(SELECT M.Char1 FROM M_Multiporpose AS M
                    WHERE M.ID = 232 AND M.[KEY} = DH.SiteKBN) AS SiteName
              ,DH.CustomerCD AS CustomerCD
              ,DH.CustomerName
              ,ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'') AS TEL
              ,DH.MailAddress
              ,ISNULL(DH.ZipCD1,'') + '-' + ISNULL(DH.ZipCD2,'') AS ZIP
              ,DH.Address1
              ,DH.Address2
              
              ,FC.CustomerCD AS M_CustomerCD
              ,FC.CustomerName AS M_CustomerName
              ,ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') AS M_TEL
              ,FC.MailAddress AS M_MailAddress
              ,ISNULL(FC.ZipCD1,'') + '-' + ISNULL(FC.ZipCD2,'') AS M_ZIP
              ,FC.Address1 AS M_Address1
              ,FC.Address2 AS M_Address2
              ,FC.AttentionFLG
        
        from D_Juchuu AS DH
        INNER JOIN F_Customer(GETDATE()) FC
        ON FC.CustomerName = DH.CustomerName
        AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
                         
        WHERE (@NayoseKekkaTourokuDate IS NULL AND DH.IdentificationFLG = 1) OR 
              (DH.NayoseKekkaTourokuDate = @NayoseKekkaTourokuDate))
            AND DH.DeleteDateTime IS NULL
            ;
            
	--テーブル転送仕様Ａ
    UPDATE D_Juchuu SET
        [CustomerCD]             = tbl.CustomerCD
       ,[IdentificationFLG]      = 0
       ,[NayoseKekkaTourokuDate] = CONVERT(date,@SYSDATETIME)
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_Juchuu.JuchuuNO = tbl.JuchuuNo
    ;
    
    --テーブル転送仕様Ｂ
    UPDATE D_JuchuuOnHold SET
        [DisappeareDateTime]     = @SYSDATETIME
       ,[UpdateOperator]         = @Operator  
       ,[UpdateDateTime]         = @SYSDATETIME
    FROM @Table tbl
    WHERE D_JuchuuOnHold.JuchuuNO = tbl.JuchuuNo
    AND D_JuchuuOnHold.OnHoldCD = '001'
    ;    
    
    --処理履歴データへ更新
    SET @KeyItem = '';
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'NayoseKekkaTouroku',
        @PC,
        @OperateModeNm,
        @KeyItem;

    SET @OutJuchuuNo = '';
    
--<<OWARI>>
  return @W_ERR;

END

GO
