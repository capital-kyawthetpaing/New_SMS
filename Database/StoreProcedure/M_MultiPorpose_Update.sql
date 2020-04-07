 BEGIN TRY 
 Drop Procedure dbo.[M_MultiPorpose_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Object:  StoredProcedure [M_MultiPorpose_Update]    */
CREATE PROCEDURE [dbo].[M_MultiPorpose_Update](
    -- Add the parameters for the stored procedure here
    @ID  int,
    @Key varchar(50),
    @Num1 int,
    @Operator varchar(10),
    @PC  varchar(30)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @SYSDATETIME datetime;
	DECLARE @COUNT int;
	
    SET @SYSDATETIME = SYSDATETIME();
    
    -- Insert statements for procedure here
    UPDATE M_MultiPorpose
    SET [Num1] = @Num1
        ,[UpdateOperator] = @Operator
        ,[UpdateDateTime] = @SYSDATETIME
	FROM M_MultiPorpose

    WHERE [ID] = @ID
    AND [Key] = @Key
    ;
    
    IF @ID = 319	--ID_EDI = "319";            //EDI受信
    BEGIN
        --【L_Log】INSERT
        --処理履歴データへ更新
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            'EDIKaitouNoukiTouroku',
            @PC,
            NULL,
            NULL;
    END
    ELSE IF @ID = 321   --ID_ShukkaUriageUpdate = "321";    //出荷売上更新
    BEGIN
        SET @COUNT = (SELECT COUNT(*) FROM M_MultiPorpose
                        WHERE [ID] = @ID
                        AND [Key] = @Key);
        
        IF @COUNT = 0
        BEGIN
            INSERT INTO M_MultiPorpose
                ([ID]
               ,[Key]
               ,[IDName]
               ,[Char1]
               ,[Char2]
               ,[Char3]
               ,[Char4]
               ,[Char5]
               ,[Num1]
               ,[Num2]
               ,[Num3]
               ,[Num4]
               ,[Num5]
               ,[Date1]
               ,[Date2]
               ,[Date3]
               ,[InsertOperator]
               ,[InsertDateTime]
               ,[UpdateOperator]
               ,[UpdateDateTime]
           )VALUES(
                @ID
               ,@Key
               ,'出荷売上更新'      --[IDName]
               ,NULL    --[Char1]
               ,NULL    --[Char2]
               ,NULL    --[Char3]
               ,NULL    --[Char4]
               ,NULL    --[Char5]
               ,@Num1
               ,0   --[Num2]
               ,0   --[Num3]
               ,0   --[Num4]
               ,0   --[Num5]
               ,NULL    --[Date1]
               ,NULL    --[Date2]
               ,NULL    --[Date3]
               ,@Operator       --[InsertOperator]
               ,@SYSDATETIME    --[InsertDateTime]
               ,@Operator       --[UpdateOperator]
               ,@SYSDATETIME    --[UpdateDateTime]
               );
               
        END
    END
    ELSE IF @ID = 325   --ID_Mail = "325";            //Mail履歴
    BEGIN
        --【L_Log】INSERT
        --処理履歴データへ更新
        EXEC L_Log_Insert_SP
            @SYSDATETIME,
            @Operator,
            'MailHstoryShoukai',
            @PC,
            NULL,
            NULL;
    END
END


