--  ======================================================================
--       Program Call    ヤマト送り状
--       Program ID      YamatoOkurijou
--       Create date:    2020.1.29
--    ======================================================================
 BEGIN TRY 
 Drop Procedure dbo.[D_Shipping_Update]
END try
BEGIN CATCH END CATCH 

/****** Object:  StoredProcedure [dbo].[D_Shipping_Update]    Script Date: 2019/09/15 19:54:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE D_Shipping_Update
    (@Operator    varchar(10)
    ,@ShippingNO  varchar(11)
)AS

--********************************************--
--                                            --
--                 処理開始                   --
--                                            --
--********************************************--

BEGIN
    DECLARE @W_ERR  tinyint;
    DECLARE @SYSDATETIME datetime;

    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();

    UPDATE D_Shipping
       SET [LinkageDateTime] = @SYSDATETIME 
          ,[LinkageStaffCD]  = @Operator 
          ,[UpdateOperator]  = @Operator  
          ,[UpdateDateTime]  = @SYSDATETIME
    WHERE ShippingNO = @ShippingNO
    ;

--<<OWARI>>
  return @W_ERR;

END

GO
