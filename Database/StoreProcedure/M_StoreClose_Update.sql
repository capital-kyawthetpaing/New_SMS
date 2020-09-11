
 BEGIN TRY 
 Drop Procedure dbo.[M_StoreClose_Update]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [M_StoreClose_Update]    */
CREATE PROCEDURE M_StoreClose_Update(
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4),
    @FiscalYYYYMM int,
    @Mode tinyint,	--1:全社
    @KBN tinyint,	--1:売上,3:仕入,2:入金,4:支払,5:在庫
    @ClosePosition1 tinyint,
    @ClosePosition2 tinyint,
    @ClosePosition3 tinyint,
    @ClosePosition4 tinyint,
    @ClosePosition5 tinyint,
    @Operator varchar(10),
    @PC varchar(30),
    @OperateModeNm varchar(30)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    DECLARE @KeyItem varchar(100);
    DECLARE @SYSDATETIME datetime;

    SET @SYSDATETIME = SYSDATETIME();
    
    IF @KBN >= 10
    BEGIN
        Update M_StoreClose SET
               [ClosePosition1] = 0
              ,[ClosePosition2] = 0
              ,[ClosePosition3] = 0
              ,[ClosePosition4] = 0
              ,[ClosePosition5] = 0
              ,[MonthlyClaimsFLG]      = 0
              ,[MonthlyClaimsDateTime] = NULL
              ,[MonthlyDebtFLG]        = 0
              ,[MonthlyDebtDateTime]   = NULL
              ,[MonthlyStockFLG]       = 0
              ,[MonthlyStockDateTime]  = NULL
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
        WHERE StoreCD = (CASE @Mode WHEN 1 THEN StoreCD ELSE @StoreCD END)
        AND FiscalYYYYMM = @FiscalYYYYMM
        ;
        
        IF @@ROWCOUNT = 0 OR @Mode = 1
        BEGIN
            EXEC M_StoreClose_Insert
                  @StoreCD
                  ,@FiscalYYYYMM
                  ,@Operator
                  ,@Mode
            ;
        END
    END
    ELSE
    BEGIN
        -- Update statements for procedure here
        Update M_StoreClose SET
               [ClosePosition1] = (CASE @KBN WHEN 1 THEN @ClosePosition1 ELSE ClosePosition1 END)
              ,[ClosePosition2] = (CASE @KBN WHEN 3 THEN @ClosePosition2 ELSE ClosePosition2 END)
              ,[ClosePosition3] = (CASE @KBN WHEN 2 THEN @ClosePosition3 ELSE ClosePosition3 END)
              ,[ClosePosition4] = (CASE @KBN WHEN 4 THEN @ClosePosition4 ELSE ClosePosition4 END)
              ,[ClosePosition5] = (CASE @KBN WHEN 5 THEN @ClosePosition5 ELSE ClosePosition5 END)
              ,[MonthlyClaimsFLG]      = (CASE @KBN WHEN 1 THEN 0    WHEN 2 THEN 0    WHEN 6 THEN 1            ELSE MonthlyClaimsFLG END)
              ,[MonthlyClaimsDateTime] = (CASE @KBN WHEN 1 THEN NULL WHEN 2 THEN NULL WHEN 6 THEN @SYSDATETIME ELSE MonthlyClaimsDateTime END)
              ,[MonthlyDebtFLG]        = (CASE @KBN WHEN 3 THEN 0    WHEN 4 THEN 0    WHEN 7 THEN 1            ELSE MonthlyDebtFLG END)
              ,[MonthlyDebtDateTime]   = (CASE @KBN WHEN 3 THEN NULL WHEN 4 THEN NULL WHEN 7 THEN @SYSDATETIME ELSE MonthlyDebtDateTime END)
              ,[MonthlyStockFLG]       = (CASE @KBN WHEN 5 THEN 0    WHEN 8 THEN 1            ELSE MonthlyStockFLG END)
              ,[MonthlyStockDateTime]  = (CASE @KBN WHEN 5 THEN NULL WHEN 8 THEN @SYSDATETIME ELSE MonthlyStockDateTime END)
              ,[UpdateOperator] = @Operator
              ,[UpdateDateTime] = @SYSDATETIME
        WHERE StoreCD = (CASE @Mode WHEN 1 THEN StoreCD ELSE @StoreCD END)
        AND FiscalYYYYMM = @FiscalYYYYMM
        ;
    END
        
    --処理履歴データへ更新
    SET @KeyItem = CONVERT(varchar,@FiscalYYYYMM) + (CASE @Mode WHEN 1 THEN ',ALL' ELSE ',' + @StoreCD END);
        
    EXEC L_Log_Insert_SP
        @SYSDATETIME,
        @Operator,
        'GetsujiShimeShori',
        @PC,
        @OperateModeNm,		--仮締 or キャンセル or 集計 or 確定 or 解除
        @KeyItem;
    
END

GO
