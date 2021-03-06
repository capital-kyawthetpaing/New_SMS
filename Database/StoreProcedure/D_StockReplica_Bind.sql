 BEGIN TRY 
 Drop Procedure dbo.[D_StockReplica_Bind] 
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
CREATE PROCEDURE [dbo].[D_StockReplica_Bind] 
AS
BEGIN
    SET NOCOUNT ON;
 
    SELECT MAX(ReplicaNO) AS ReplicaNO
         , CONVERT(NVARCHAR, ReplicaDate, 111) + ' ' + CONVERT(NVARCHAR, ReplicaTime, 108) + ' ' + FORMAT(MAX(ReplicaNO),'0000') AS DateTime
      FROM D_StockReplica 
    GROUP BY ReplicaDate, ReplicaTime
    ORDER BY ReplicaDate, ReplicaTime

END

GO
