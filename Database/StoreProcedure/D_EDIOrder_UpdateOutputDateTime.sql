IF OBJECT_ID ( 'D_EDIOrder_UpdateOutputDateTime', 'P' ) IS NOT NULL
    Drop Procedure dbo.[D_EDIOrder_UpdateOutputDateTime]
GO


--  ======================================================================
--       Program Call    EDI発注入力
--       Program ID      D_EDIOrder_Insert
--       Create date:    2019.11.16
--    ======================================================================

--********************************************--
--                                            --
--           CSV出力済みフラグ更新            --
--                                            --
--********************************************--
CREATE PROCEDURE D_EDIOrder_UpdateOutputDateTime(
     @SYSDATETIME varchar(19)
)AS
BEGIN

    SET NOCOUNT ON;

    -- CSV出力済みフラグ
    UPDATE D_EDIOrder
       SET OutputDatetime = CONVERT(datetime, @SYSDATETIME)
      FROM D_EDIOrder DH
     WHERE DH.OutputDatetime IS NULL
END

