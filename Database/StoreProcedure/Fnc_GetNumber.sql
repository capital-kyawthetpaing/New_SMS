 BEGIN TRY 
 Drop Procedure dbo.[Fnc_GetNumber]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Fnc_GetNumber]
(   
    -- Add the parameters for the function here
    @SeqKBN      tinyint,
    @ChangeDate  varchar(10),
    @StoreCD     varchar(4),
    @Operator  varchar(10),
    @OutNO      varchar(11) OUTPUT  
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--
BEGIN

    --変数宣言
    DECLARE @iCnt int;
    SET @OutNO = null;
    
    DECLARE @StoreKBN tinyint;

--BEGIN TRY
    --in伝票種別＝13の場合
    IF @SeqKBN = 13
    BEGIN
        SET @OutNO = (SELECT RIGHT('0000000000' + CONVERT(varchar, M.TemporaryReserveCounter + 1), 11)
        FROM M_TemporaryReserve AS M
        WHERE M.TemporaryReserveKey = 1
        );
        
        UPDATE M_TemporaryReserve
        SET TemporaryReserveCounter = TemporaryReserveCounter + 1
        WHERE TemporaryReserveKey = 1
        ;
        
	END
	--in伝票種別≠13の場合
	ELSE
	BEGIN
	    --【チェック】
	    --①in基準日が以下の条件でカレンダーマスター(M_Calendar)に存在しなければ、Error★とする
	    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
	                WHERE M.CalendarDate = convert(date,@ChangeDate)
	                );
	 
	    IF @iCnt = 0
	    BEGIN
	        RETURN @OutNO;
	    END
	    
	    --②in店舗CD≠Nullの場合、以下の条件で店舗マスター(M_Store①)に存在しなければ、Error★とする
	    IF ISNULL(@StoreCD,'') = ''
	    BEGIN
	        SET @iCnt = (SELECT COUNT(*) FROM M_Store M
	                    WHERE M.StoreCD = @StoreCD
	                    AND M.DeleteFlg = 0
	                    AND M.ChangeDate < = convert(date,@ChangeDate)
	                    );
	                
	        IF @iCnt = 0
	        BEGIN
	            RETURN @OutNO;
	        END
	        ELSE
	        BEGIN
	            SET @StoreKBN = (SELECT top 1 M.StoreKBN FROM M_Store M
	                    WHERE M.StoreCD = @StoreCD
	                    AND M.DeleteFlg = 0
	                    AND M.ChangeDate < = convert(date,@ChangeDate)
	                    ORDER BY M.ChangeDate desc
	                    );
	                    
	            IF @StoreKBN = 2    --2　:WEB店
	            BEGIN
	                SET @StoreCD = (SELECT top 1 M.StoreCD FROM M_Store M
	                                WHERE M.StoreKBN = 3    --3　:WEBまとめ店舗
	                                AND M.DeleteFlg = 0
	                                AND M.ChangeDate < = convert(date,@ChangeDate)
	                                ORDER BY M.ChangeDate desc
	                                );
	            END
	        END
	    END

	    --【採番する】
	    DECLARE @SeqUnit tinyint;
	    DECLARE @Prefix varchar(3);
	    DECLARE @Prefix2 varchar(4);
	    DECLARE @SeqNumber int;
	    
	    SET @SeqUnit = (SELECT M.SeqUnit FROM M_Control M WHERE MainKey = 1);

	    IF @SeqUnit = 1
	        SET @Prefix2 = '0000';
	    ELSE IF @SeqUnit = 2    --YYYY
	        SET @Prefix2 = SUBSTRING(@ChangeDate,1,4);
	    ELSE    --YYMM
	        SET @Prefix2 = SUBSTRING(@ChangeDate,3,2) + SUBSTRING(@ChangeDate,6,2);
	    
	    SET @Prefix = (SELECT M.Prefix FROM M_Prefix M
	                    WHERE M.StoreCD = @StoreCD
	                    AND M.SeqKBN = @SeqKBN);
	                    
	    SET @SeqNumber = (SELECT M.SeqNumber+1 FROM M_PrefixNumber M
	                        WHERE M.Prefix = @Prefix
	                        AND M.Prefix2 = @Prefix2
	                        );  
	    

	    SET @OutNO = @Prefix + @Prefix2 + RIGHT('000' + convert(varchar,@SeqNumber),4);
	    
	    --採番後(After counting up the number)、M_PrefixNumber　を Update
	    UPDATE M_PrefixNumber 
	    SET SeqNumber = @SeqNumber
	    ,UpdateOperator = @Operator
	    ,UpdateDateTime = SYSDATETIME()
	    WHERE Prefix = @Prefix
	    AND Prefix2 = @Prefix2
	    ;
	END
--END TRY

--BEGIN CATCH
--    SET @OutNO = null;
--END CATCH

END

