IF OBJECT_ID ( 'PRC_EDIOrder_MailInsert', 'P' ) IS NOT NULL
    Drop Procedure dbo.[PRC_EDIOrder_MailInsert]
GO


--  ======================================================================
--       Program Call    EDI��������
--       Program ID      D_EDIOrder_Insert
--       Create date:    2019.11.16
--    ======================================================================

--********************************************--
--                                            --
--              �ďo�͕�����                  --
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
    
    --M_Vendor�擾
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
    
    --���[���֌W�e�[�u���ǉ�
    IF @EDIFlg = 2 
    BEGIN
        EXEC D_Mail_Insert
            @EDIOrderNo,
            @SYSDATETIME,
            @Operator;
    END
    
    --���������f�[�^�֍X�V
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

