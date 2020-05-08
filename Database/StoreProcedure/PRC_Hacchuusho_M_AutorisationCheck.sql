--  ======================================================================  
--       Program Call    î≠íçèë  
--       Program ID      IkkatuHacchuuNyuuryoku
--       Create date:    2020.01.09
--    ======================================================================  
  
IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_M_AutorisationCheck')
begin
    DROP PROCEDURE PRC_Hacchuusho_M_AutorisationCheck
end
GO

CREATE PROCEDURE PRC_Hacchuusho_M_AutorisationCheck(
 @p_AuthorizationsCD             varchar(4)
,@p_ChangeDate                   varchar(10)
,@p_ProgramID                    varchar(50)
,@p_StoreCD                      varchar(6)
)
AS
BEGIN

    SELECT *
    FROM M_AuthorizationsDetails
    WHERE AuthorizationsCD = @p_AuthorizationsCD
      AND ChangeDate       = @p_ChangeDate      
      AND ProgramID        = @p_ProgramID       
      --AND StoreCD          = @p_StoreCD     

END
