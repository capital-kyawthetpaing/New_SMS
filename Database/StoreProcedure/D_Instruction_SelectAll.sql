 BEGIN TRY 
 Drop Procedure dbo.[D_Instruction_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Instruction_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Instruction_SelectAll](
    -- Add the parameters for the stored procedure here
    @InstructionDateFrom  varchar(10),
    @InstructionDateTo  varchar(10),
    @DeliveryPlanDateFrom  varchar(10),
    @DeliveryPlanDateTo  varchar(10),
    @SoukoCD  varchar(6),
    @Chk1 tinyint,
    @Chk2 tinyint,
    @Chk3 tinyint,
    @Chk4 tinyint,
    @Chk5 tinyint,
    @DeliveryName  varchar(80),
    @CarrierCD  varchar(3),
    @JuchuuNO varchar(11), 
    @StaffCD  varchar(10)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT  CONVERT(varchar,DI.InstructionDate,111) AS InstructionDate          
            ,CONVERT(varchar,DI.DeliveryPlanDate,111) AS DeliveryPlanDate   
            ,DI.InstructionNO                                               
            ,DD.Number                                              
            ,DI.DeliveryName                                                
            ,DI.DeliveryAddress1                                                
            ,(CASE WHEN DI.DecidedDeliveryDate IS NULL THEN '' ELSE '○' END) AS DecidedDeliveryDate    
            ,(CASE DI.OntheDayFLG WHEN 1 THEN '○' else '' END) AS OntheDayFLG
            ,(CASE DI.InstructionKBN WHEN 2 THEN '○' else '' END) AS InstructionKBN
            ,(SELECT top 1 A.StaffName
                  FROM M_Staff AS A 
                  WHERE A.StaffCD = DI.InsertOperator 
                  AND A.DeleteFlg = 0 
                  AND A.ChangeDate <= DI.InstructionDate
                  ORDER BY A.ChangeDate desc) AS StaffName                                               
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A 
                  WHERE A.CarrierCD = DI.CarrierCD 
                  AND A.DeleteFlg = 0 
                  AND A.ChangeDate <= DI.InstructionDate
                  ORDER BY A.ChangeDate desc) AS CarrierName                                             

          ,(SELECT top 1 M.SKUName
            FROM M_SKU AS M 
            WHERE M.ChangeDate <= DI.InstructionDate
             AND M.AdminNO = DM.AdminNO
              AND M.DeleteFlg = 0
             ORDER BY M.ChangeDate desc) AS SKUName
             
        FROM D_Instruction AS DI
        LEFT OUTER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL

        LEFT OUTER JOIN D_InstructionDetails AS DM 
        ON DM.InstructionNO = DI.InstructionNO 
        AND DM.DeleteDateTime IS NULL
        AND DM.InstructionRows = 1
        
        WHERE DI.InstructionDate >= (CASE WHEN @InstructionDateFrom <> '' THEN CONVERT(DATE, @InstructionDateFrom) ELSE DI.InstructionDate END)
        AND DI.InstructionDate <= (CASE WHEN @InstructionDateTo <> '' THEN CONVERT(DATE, @InstructionDateTo) ELSE DI.InstructionDate END)
        AND DI.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DI.DeliveryPlanDate END)
        AND DI.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DI.DeliveryPlanDate END)
        AND DI.CarrierCD = (CASE WHEN @CarrierCD <> '' THEN @CarrierCD ELSE DI.CarrierCD END)
        AND DI.DeliverySoukoCD = (CASE WHEN @SoukoCD <> '' THEN @SoukoCD ELSE DI.DeliverySoukoCD END)
        AND DI.InsertOperator = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DI.InsertOperator END)
        AND DI.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND DI.DeleteDateTime IS NULL
        AND ISNULL(DD.Number,'') = (CASE WHEN @JuchuuNO <> '' THEN @JuchuuNO ELSE ISNULL(DD.Number,'') END)
        AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.DecidedDeliveryDate IS NULL AND DI.OntheDayFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk3 = 1 AND DI.DecidedDeliveryDate IS NOT NULL)
            OR (@Chk4 = 1 AND DI.OntheDayFLG = 1)
            OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
            )
    ORDER BY DI.InstructionDate desc, DI.InstructionNO
    ;

END


