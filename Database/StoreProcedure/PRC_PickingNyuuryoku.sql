 BEGIN TRY 
 Drop Procedure dbo.[PRC_PickingNyuuryoku]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  ======================================================================
--       Program Call    ピッキング入力
--       Program ID      PickingNyuuryoku
--       Create date:    2020.01.09
--    ======================================================================
--CREATE TYPE T_Picking AS TABLE
--    (
--    [PickingNO] [varchar](11) ,
--    [PickingRows] [int],
--    [Chk][tinyint],		--0:OFF,1:ON
--    [ChkModori][tinyint],		--0:OFF,1:ON
--    [ReserveNO] [varchar](11) ,
--    [UpdateFlg][tinyint]
--    )
--GO

CREATE PROCEDURE [dbo].[PRC_PickingNyuuryoku]
    (
    @PickingDate  varchar(10),

    @Table  T_Picking READONLY,
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
    
    SET @W_ERR = 0;
    SET @SYSDATETIME = SYSDATETIME();;
	
	--ピッキング明細【D_PickingDetails】テーブル転送仕様Ａ
    UPDATE D_PickingDetails SET
           [PickingDate] = (CASE tbl.Chk WHEN 1 THEN @PickingDate ELSE NULL END)
          ,[StaffCD] = (CASE tbl.Chk WHEN 1 THEN @Operator   ELSE NULL END)
          ,[PickingDoneDateTime] = (CASE tbl.Chk WHEN 1 THEN @SYSDATETIME ELSE NULL END)
          ,[UpdateOperator] = @Operator   
          ,[UpdateDateTime] = @SYSDATETIME
     FROM @Table AS tbl
     WHERE tbl.PickingNO = D_PickingDetails.PickingNO
     AND tbl.PickingRows = D_PickingDetails.PickingRows
     AND tbl.UpdateFlg >= 1
     AND DeleteDateTime IS NULL
     ;

	--引当　(戻りは更新不要）【D_Reserve】テーブル転送仕様Ｂ
    UPDATE D_Reserve SET
           [CompletedPickingNO] = (CASE tbl.Chk WHEN 1 THEN tbl.PickingNO ELSE NULL END)
          ,[CompletedPickingRow] = (CASE tbl.Chk WHEN 1 THEN tbl.PickingRows  ELSE 0 END)
          ,[CompletedPickingDate] = (CASE tbl.Chk WHEN 1 THEN @PickingDate ELSE NULL END)
          ,[UpdateOperator] = @Operator   
          ,[UpdateDateTime] = @SYSDATETIME
     FROM @Table AS tbl
     WHERE tbl.ReserveNO = D_Reserve.ReserveNO
     AND tbl.UpdateFlg = 2
     AND DeleteDateTime IS NULL
     ;

--<<OWARI>>
  return @W_ERR;

END


