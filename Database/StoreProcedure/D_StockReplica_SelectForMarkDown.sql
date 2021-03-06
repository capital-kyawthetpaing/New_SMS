BEGIN TRY 
 Drop Procedure dbo.[D_StockReplica_SelectForMarkDown]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE D_StockReplica_SelectForMarkDown(
    @ReplicaNO  int,
    @AdminNO    int,
    @SoukoCD    varchar(6)
)AS
BEGIN
    SET NOCOUNT ON;

    WITH Stock AS (
            SELECT DS.LastCost, DS.AdminNO, DS.Stockno
                  ,CASE WHEN ArrivalYetFLG = 0 THEN AllowableSu ELSE 0 END AS AllowableSu          
             FROM D_StockReplica DS    
            WHERE DS.ReplicaNO = @ReplicaNO
              AND DS.AdminNO = @AdminNO
              AND DS.SoukoCD = @SoukoCD
              AND DS.DeleteDateTime IS NULL
    )
    SELECT TOP 1 ST1.LastCost, ISNULL(ST2.AllowableSu,0) AS AllowableSu
      FROM Stock as ST1
      LEFT JOIN ( SELECT AdminNO, SUM(AllowableSu) AS AllowableSu
                    FROM STOCK
                   GROUP BY AdminNO
                 ) ST2 ON ST1.AdminNO = ST2.AdminNO
    ORDER BY ST1.StockNO desc
    ;
END

