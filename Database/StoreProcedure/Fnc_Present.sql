
/****** Object:  UserDefinedFunction [dbo].[Fnc_Present]    Script Date: 6/11/2019 2:30:20 PM ******/
DROP PROCEDURE [dbo].[Fnc_Present]
GO
DROP PROCEDURE [dbo].[Fnc_Present_SP]
GO

/****** Object:  UserDefinedFunction [dbo].[Fnc_Present]    Script Date: 6/11/2019 2:30:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE Fnc_Present
(   
    -- Add the parameters for the function here
    @AdminNO     int,    
    @ChangeDate  varchar(10),
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    DECLARE @Error  tinyint;
    DECLARE @outPresentCD1 varchar(5);
    DECLARE @outPresentCD2 varchar(5);
    DECLARE @outPresentCD3 varchar(5);
    DECLARE @outPresentCD4 varchar(5);
    DECLARE @outPresentCD5 varchar(5);

    IF ISNULL(@ChangeDate,'') = ''
        SET @ChangeDate = CONVERT(varchar, GETDATE(),111);
    
    EXEC Fnc_Present_SP
        @AdminNO     ,    
        @ChangeDate ,
        @StoreCD,
        @outPresentCD1   OUTPUT,
        @outPresentCD2   OUTPUT,
        @outPresentCD3   OUTPUT,
        @outPresentCD4   OUTPUT,
        @outPresentCD5   OUTPUT,
        @Error  OUTPUT
        ;
    
    -- Insert statements for procedure here
    SELECT @outPresentCD1 AS PresentCD1,
	       @outPresentCD2 AS PresentCD2,
	       @outPresentCD3 AS PresentCD3,
	       @outPresentCD4 AS PresentCD4,
	       @outPresentCD5 AS PresentCD5,
	       @Error  AS Error
	       ;   
END

GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date,,>
-- Description: <Description,,>
-- =============================================
CREATE PROCEDURE Fnc_Present_SP
(   
    -- Add the parameters for the function here
    @AdminNO     int,    
    @ChangeDate  varchar(10),
    @StoreCD  varchar(4),
    
    @outPresentCD1 varchar(5) OUTPUT,
    @outPresentCD2 varchar(5) OUTPUT,
    @outPresentCD3 varchar(5) OUTPUT,
    @outPresentCD4 varchar(5) OUTPUT,
    @outPresentCD5 varchar(5) OUTPUT,
    @Error  tinyint OUTPUT
)AS

--********************************************--
--                                            --
--                 �����J�n                   --
--                                            --
--********************************************--
BEGIN

    --�ϐ��錾
    DECLARE @iCnt int;
    DECLARE @StorePlaceKBN tinyint;
    DECLARE @StoreKBN tinyint;
    DECLARE @ItemCD varchar(30);
    DECLARE @PresentCD varchar(5);
    
    SET @outPresentCD1 = NULL;
    SET @outPresentCD2 = NULL;
    SET @outPresentCD3 = NULL;
    SET @outPresentCD4 = NULL;
    SET @outPresentCD5 = NULL;
    
    SET @Error = 0;

--BEGIN TRY

    --�y�`�F�b�N�z
    --�@in������ȉ��̏����ŃJ�����_�[�}�X�^�[(M_Calendar)�ɑ��݂��Ȃ���΁A
    SET @iCnt = (SELECT COUNT(*) FROM M_Calendar M
                WHERE M.CalendarDate = convert(date,@ChangeDate)
                );
 
    IF @iCnt = 0
    BEGIN
        SET @Error = 1;
        RETURN;
    END
    
    --�AinAdminNO��SKU�}�X�^�[(M_SKU)�ɑ��݂��Ȃ����
    SET @iCnt = (SELECT COUNT(*) FROM M_SKU M
                WHERE M.ChangeDate <= convert(date,@ChangeDate)
                AND M.AdminNO = @AdminNO
                AND M.DeleteFLG = 0
                );
                
    IF @iCnt = 0
    BEGIN
        SET @Error = 1;
        RETURN;
    END
  
    --�Bin�X��CD��Null�̏ꍇ�A�ȉ��̏����œX�܃}�X�^�[(M_Store)�ɑ��݂��Ȃ���΁A   
    IF ISNULL(@StoreCD,'') <> ''
    BEGIN
        SET @iCnt = (SELECT COUNT(*) FROM M_Store M
                    WHERE M.ChangeDate <= convert(date,@ChangeDate)
                    AND M.StoreCD = @StoreCD
                    AND M.DeleteFLG = 0
                    );
                    
        IF @iCnt = 0
        BEGIN
            SET @Error = 1;
            RETURN;
        END
        
        SELECT top 1 @StorePlaceKBN=StorePlaceKBN, @StoreKBN=StoreKBN FROM M_Store M
        WHERE M.ChangeDate <= convert(date,@ChangeDate)
        AND M.StoreCD = @StoreCD
        AND M.DeleteFLG = 0
        ORDER BY M.ChangeDate desc
        ;
        
        IF @StoreKBN = 2    --2�@:WEB�X
        BEGIN
            SELECT top 1 @StoreCD=StoreCD,@StorePlaceKBN=StorePlaceKBN FROM M_Store M
            WHERE M.ChangeDate <= convert(date,@ChangeDate)
            AND M.DeleteFLG = 0
            AND M.StoreKBN = 3  --3�@:WEB�܂ƂߓX��
            ORDER BY M.ChangeDate desc
            ;
        END
    END

    --�ySKU����ITEM�����߂�z
    SELECT top 1 @ItemCD = M.ItemCD FROM M_SKU M
    WHERE M.ChangeDate <= convert(date,@ChangeDate)
    AND M.AdminNO = @AdminNO
    AND M.DeleteFLG = 0
    ORDER BY M.ChangeDate desc
    ;

    --�y����ITEM���v���[���g�����ITEM���𒲂ׂ�z
    --�y����PresentCD���A�����Ƃ��Ė������Ă��邩���ׂ�z
    --�J�[�\����`
    DECLARE CUR1 CURSOR FOR
        SELECT A.PresentCD
        FROM M_Present AS A
        WHERE A.PresentCD IN (SELECT M.PresentCD
                            FROM M_PresentApplied AS M
                            WHERE M.AppliedITemCD = @ItemCD
                            AND M.StartDate <= convert(date,@ChangeDate)
                            AND M.EndDate >= convert(date,@ChangeDate))
        AND A.StoreKBN IN (@StoreKBN, 3)
        AND A.StartDate <= convert(date,@ChangeDate)
        AND A.EndDate >= convert(date,@ChangeDate)
        ;


    --�J�[�\���I�[�v��
    OPEN CUR1;
    
    --�ŏ���1�s�ڂ��擾���ĕϐ��֒l���Z�b�g
    FETCH NEXT FROM CUR1
    INTO @PresentCD;

    --�f�[�^�̍s�������[�v���������s����
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @iCnt = @iCnt + 1;
        
        IF @iCnt = 1
        BEGIN
            SET @outPresentCD1 = @PresentCD;
        END
        ELSE IF @iCnt = 2
        BEGIN
            SET @outPresentCD2 = @PresentCD;
        END
        ELSE IF @iCnt = 3
        BEGIN
            SET @outPresentCD3 = @PresentCD;
        END
        ELSE IF @iCnt = 4
        BEGIN
            SET @outPresentCD4 = @PresentCD;
        END
        ELSE IF @iCnt = 5
        BEGIN
            SET @outPresentCD5 = @PresentCD;
            BREAK;
        END
    	
        --���̍s�̃f�[�^���擾���ĕϐ��֒l���Z�b�g
        FETCH NEXT FROM CUR1
        INTO @PresentCD;
    END
    
    --�J�[�\�������
    CLOSE CUR1;
    DEALLOCATE CUR1;
    
--END TRY

--BEGIN CATCH
	--PRINT 'In catch block.';  
--    THROW; 
--    RETURN;
--END CATCH

END
GO


