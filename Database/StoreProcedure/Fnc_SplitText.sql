  
CREATE FUNCTION [dbo].[Fnc_SplitText]  
    (  
        @Texts as nvarchar(max),  
        @SeparatorChar as char(1) = ',',  
        @NullText as nvarchar(16) = '<NULL>'  
    )  
  
  
RETURNS @Table TABLE([Text] varchar(max))  
AS  
/*  
***************************************************************************************************  
**  ���́F�����p�����[�^�w��p�֐�  
**  �@�\�F�u,�v�ŋ�؂�ꂽ������𕡐��̃p�����[�^�ɕ�������  
**          
**  �p�����[�^          
**      @Texts                    nvarchar  
**      @SeparatorChar            char(1)  
**      @NullText                 nvarchar  
**  
**************************************************************************************************************************
*/  
BEGIN  
    DECLARE @Cnt int;  
    DECLARE @NextChar nvarchar(1);  
    DECLARE @SaveText nvarchar(max);  
    SELECT @Cnt = 1, @NextChar = '', @SaveText = '';  
  
    WHILE @Cnt <= LEN(@Texts)  
    BEGIN  
            SET @NextChar = SUBSTRING(@Texts, @Cnt, 1);  
  
            IF @NextChar = @SeparatorChar  
            BEGIN  
                    IF @SaveText = @NullText SELECT @SaveText = null;  
                    INSERT INTO @Table VALUES(@SaveText);  
                    SELECT @SaveText = '';  
            END  
  
            IF @NextChar <> @SeparatorChar  
                    SELECT @SaveText = @SaveText + SUBSTRING(@Texts, @Cnt, 1);  
                      
            SET @Cnt = @Cnt + 1;  
  
    END  
  
    IF @SaveText = @NullText SELECT @SaveText = null;  
    INSERT INTO @Table VALUES(@SaveText);  
      
    RETURN  
END  