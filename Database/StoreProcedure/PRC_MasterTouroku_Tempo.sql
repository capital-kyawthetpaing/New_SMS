 BEGIN TRY 
 Drop Procedure dbo.[PRC_MasterTouroku_Tempo]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    店舗ストアマスタ
--       Program ID      MasterTouroku_Tempo
--       Create date:    2019.5.10
--    ======================================================================
CREATE PROCEDURE [dbo].[PRC_MasterTouroku_Tempo]
    (@OperateMode    int,                 -- 処理区分（1:新規 2:修正 3:削除）
    @StoreCD  varchar(4),
    @ChangeDate date,
    @StoreName  varchar(40),
    @StoreKBN tinyint,
    @StorePlaceKBN tinyint,
    @MallCD  varchar(4),
    @APIKey tinyint,
    @ZipCD1  varchar(3),
    @ZipCD2  varchar(4),
    @Address1  varchar(100),
    @Address2  varchar(100),
    @TelphoneNO  varchar(15),
    @FaxNO  varchar(15),
    @MailAddress1  varchar(50),
    @ApprovalStaffCD11  varchar(10),
    @ApprovalStaffCD12  varchar(10),
    @ApprovalStaffCD21  varchar(10),
    @ApprovalStaffCD22  varchar(10),
    @ApprovalStaffCD31  varchar(10),
    @ApprovalStaffCD32  varchar(10),
    @DeliveryDate  varchar(50),
    @PaymentTerms  varchar(50),
    @DeliveryPlace  varchar(50),
    @ValidityPeriod  varchar(50),
    @Print1  varchar(40),
    @Print2  varchar(40),
    @Print3  varchar(40),
    @Print4  varchar(40),
    @Print5  varchar(40),
    @Print6  varchar(40),
    @KouzaCD  varchar(3),
    @ReceiptPrint  varchar(3),
    @MoveMailPatternCD  varchar(5),
    @Remarks  varchar(500),
    @DeleteFlg tinyint,
    @UsedFlg tinyint,
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
	SET @KeyItem = @StoreCD + ' ' + CONVERT(varchar, @ChangeDate,111);
	
    --新規--
    IF @OperateMode = 1
    BEGIN
		SET @OperateModeNm = '新規';
		
        INSERT INTO [M_Store]
                   ([StoreCD]
                   ,[ChangeDate]
                   ,[StoreName]
                   ,[StoreKBN]
                   ,[StorePlaceKBN]
                   ,[MallCD]
                   ,[APIKey]
                   ,[ZipCD1]
                   ,[ZipCD2]
                   ,[Address1]
                   ,[Address2]
                   ,[TelephoneNO]
                   ,[FaxNO]
                   ,[MailAddress1]
                   ,[MailAddress2]
                   ,[MailAddress3]
                   ,[ApprovalStaffCD11]
                   ,[ApprovalStaffCD12]
                   ,[ApprovalStaffCD21]
                   ,[ApprovalStaffCD22]
                   ,[ApprovalStaffCD31]
                   ,[ApprovalStaffCD32]
                   ,[DeliveryDate]
                   ,[PaymentTerms]
                   ,[DeliveryPlace]
                   ,[ValidityPeriod]
                   ,[Print1]
                   ,[Print2]
                   ,[Print3]
                   ,[Print4]
                   ,[Print5]
                   ,[Print6]
                   ,[KouzaCD]
                   ,[ReceiptPrint]
                   ,[MoveMailPatternCD]
                   ,[Remarks]
                   ,[DeleteFlg]
                   ,[UsedFlg]
                   ,[InsertOperator]
                   ,[InsertDateTime]
                   ,[UpdateOperator]
                   ,[UpdateDateTime])
             VALUES
                   (@StoreCD  
                   ,@ChangeDate
                   ,@StoreName 
                   ,@StoreKBN
                   ,@StorePlaceKBN
                   ,@MallCD 
                   ,@APIKey
                   ,@ZipCD1 
                   ,@ZipCD2 
                   ,@Address1  
                   ,@Address2  
                   ,@TelphoneNO 
                   ,@FaxNO 
                   ,@MailAddress1
                   ,NULL
                   ,NULL
                   ,@ApprovalStaffCD11  
                   ,@ApprovalStaffCD12  
                   ,@ApprovalStaffCD21  
                   ,@ApprovalStaffCD22  
                   ,@ApprovalStaffCD31  
                   ,@ApprovalStaffCD32  
                   ,@DeliveryDate  
                   ,@PaymentTerms  
                   ,@DeliveryPlace 
                   ,@ValidityPeriod
                   ,@Print1  
                   ,@Print2  
                   ,@Print3  
                   ,@Print4  
                   ,@Print5  
                   ,@Print6  
                   ,@KouzaCD 
                   ,@ReceiptPrint
                   ,@MoveMailPatternCD
                   ,@Remarks 
                   ,@DeleteFlg
                   ,@UsedFlg
                   ,@Operator  
                   ,@SYSDATETIME
                   ,@Operator  
                   ,@SYSDATETIME
                   );
    END;
    
    ELSE IF @OperateMode = 2 --変更--
    BEGIN
    	SET @OperateModeNm = '変更';
		
        UPDATE [M_Store]
           SET [StoreCD] = @StoreCD  
              ,[ChangeDate] = @ChangeDate
              ,[StoreName] = @StoreName 
              ,[StoreKBN] = @StoreKBN
              ,[StorePlaceKBN] = @StorePlaceKBN
              ,[MallCD] = @MallCD 
              ,[APIKey] = @APIKey 
              ,[ZipCD1] = @ZipCD1 
              ,[ZipCD2] = @ZipCD2 
              ,[Address1] = @Address1 
              ,[Address2] = @Address2 
              ,[TelephoneNO] = @TelphoneNO 
              ,[FaxNO] = @FaxNO  
              ,[MailAddress1] = @MailAddress1
              ,[ApprovalStaffCD11] = @ApprovalStaffCD11 
              ,[ApprovalStaffCD12] = @ApprovalStaffCD12 
              ,[ApprovalStaffCD21] = @ApprovalStaffCD21 
              ,[ApprovalStaffCD22] = @ApprovalStaffCD22 
              ,[ApprovalStaffCD31] = @ApprovalStaffCD31 
              ,[ApprovalStaffCD32] = @ApprovalStaffCD32 
              ,[DeliveryDate] = @DeliveryDate  
              ,[PaymentTerms] = @PaymentTerms  
              ,[DeliveryPlace] = @DeliveryPlace
              ,[ValidityPeriod] = @ValidityPeriod  
              ,[Print1] = @Print1  
              ,[Print2] = @Print2  
              ,[Print3] = @Print3  
              ,[Print4] = @Print4  
              ,[Print5] = @Print5  
              ,[Print6] = @Print6  
              ,[KouzaCD] = @KouzaCD  
              ,[ReceiptPrint] = @ReceiptPrint  
              ,[MoveMailPatternCD] = @MoveMailPatternCD  
              ,[Remarks] = @Remarks  
              ,[DeleteFlg] = @DeleteFlg
              ,[UpdateOperator] = @Operator  
              ,[UpdateDateTime] = @SYSDATETIME
         WHERE [StoreCD] = @StoreCD  
           AND [ChangeDate] = @ChangeDate
           ;

    END

    ELSE IF @OperateMode = 3 --削除--
    BEGIN
    	SET @OperateModeNm = '削除';
		
        DELETE FROM [M_Store]
         WHERE [StoreCD] = @StoreCD  
           AND [ChangeDate] = @ChangeDate
           ;

    END

    --処理履歴データへ更新
    EXEC L_Log_Insert_SP
        @SYSDATETIME ,
        @Operator,
        'MasterTouroku_Tempo',
        @PC,
        @OperateModeNm,
        @KeyItem;
    
--<<OWARI>>
  return @W_ERR;

END


