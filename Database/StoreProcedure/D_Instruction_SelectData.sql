 BEGIN TRY 
 Drop Procedure dbo.[D_Instruction_SelectData]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Instruction_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Instruction_SelectData](
    -- Add the parameters for the stored procedure here
    @DeliveryPlanDateFrom  varchar(10),
    @DeliveryPlanDateTo  varchar(10),
    @StoreCD  varchar(4),
    @Chk1 tinyint,
    @Chk2 tinyint,
    @Chk3 tinyint,
    @Chk4 tinyint,
    @Chk5 tinyint,
    @DeliveryName  varchar(80),
    @ChkHakkozumi tinyint,
    @ChkSyukkazumi tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    --画面項目転送表01　(出荷指示未作成分)・・・配送予定情報から表示
    SELECT  (CASE WHEN DD.DecidedDeliveryDate IS NOT NULL THEN 
                    (CASE WHEN DATEADD(DAY,-1*ISNULL(MZ.CarrierLeadDay,0),DD.DecidedDeliveryDate) < SYSDATETIME() THEN
                        CONVERT(varchar,SYSDATETIME(),111)
                        ELSE CONVERT(varchar,DD.DecidedDeliveryDate,111) END)
                ELSE CONVERT(varchar,DD.DeliveryPlanDate,111) END) AS DeliveryPlanDate   --出荷予定日
            ,NULL AS InstructionNO	--出荷指示番号
            ,DD.Number
            ,DD.DeliveryName
            ,DD.DeliveryAddress1
            ,DD.CarrierCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DD.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DD.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS CarrierName
            ,NULL AS PrintDate		--出荷指示書
            ,NULL AS ShippingDate	--出荷
            ,DD.CommentOutStore
            ,DD.CommentInStore
            ,DD.DeliveryPlanNO
            ,DD.DeliveryKBN As InstructionKBN
            ,DD.OntheDayFLG
            ,DD.ExpressFLG
            ,CONVERT(varchar,DD.DecidedDeliveryDate,111) + ' ' + ISNULL(DD.DecidedDeliveryTime,'') AS DecidedDeliveryDate	--着指定日時
            ,1 AS KBN	--出荷指示未作成分
        FROM D_DeliveryPlan AS DD
        LEFT OUTER JOIN M_ZipCode MZ
        ON MZ.ZipCD1 = DD.DeliveryZip1CD
        AND MZ.ZipCD2 = DD.DeliveryZip2CD

        WHERE DD.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DD.DeliveryPlanDate END)
        AND DD.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DD.DeliveryPlanDate END)
        AND DD.HikiateFLG = 1
        AND DD.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        --AND DD.DeleteDateTime IS NULL
        
        AND ((@Chk1 = 1 AND DD.ExpressFLG <> 1 AND DD.DecidedDeliveryDate IS NULL AND DD.OntheDayFLG <> 1 AND DD.DeliveryKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DD.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk3 = 1 AND DD.DecidedDeliveryDate IS NOT NULL)
            OR (@Chk4 = 1 AND DD.OntheDayFLG = 1)
            OR (@Chk5 = 1 AND DD.DeliveryKBN = 2)
            )
        AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
                WHERE M.SoukoCD = DD.DeliverySoukoCD
                AND M.DeleteFlg = 0
                AND M.StoreCD = @StoreCD
            )
        AND NOT EXISTS(SELECT 1 FROM D_DeliveryPlanDetails AS B
                    WHERE B.DeliveryPlanNO = DD.DeliveryPlanNO
                    --AND B.DeleteDateTime IS NULL
                    AND B.HikiateFLG = 0
                    AND B.UpdateCancelKBN <> 9)
        AND NOT EXISTS(SELECT 1 FROM D_Instruction AS DI
                    WHERE DI.DeliveryPlanNO = DD.DeliveryPlanNO
                    AND DI.DeleteDateTime IS NULL)
	UNION ALL
	
    --画面項目転送表02　(出荷指示作成済分)・・・出荷指示データから表示
    SELECT  CONVERT(varchar,DI.DeliveryPlanDate,111) AS DeliveryPlanDate
            ,DI.InstructionNO
            ,DD.Number
            ,DI.DeliveryName
            ,DI.DeliveryAddress1
            ,DI.CarrierCD
            ,(SELECT top 1 A.CarrierName
                  FROM M_Carrier AS A
                  WHERE A.CarrierCD = DI.CarrierCD
                  AND A.DeleteFlg = 0
                  AND A.ChangeDate <= DI.DeliveryPlanDate
                  ORDER BY A.ChangeDate desc) AS CarrierName
            ,CONVERT(varchar,DI.PrintDate,111) AS PrintDate
            ,CONVERT(varchar,DS.ShippingDate,111) AS ShippingDate
            ,DI.CommentOutStore
            ,DI.CommentInStore
            ,DI.DeliveryPlanNO
            ,DI.InstructionKBN
            ,DI.OntheDayFLG
            ,DI.ExpressFLG
            ,CONVERT(varchar,DI.DecidedDeliveryDate,111) + ' ' + ISNULL(DI.DecidedDeliveryTime,'') AS DecidedDeliveryDate
            ,(CASE WHEN DS.InstructionNO IS NULL THEN 1 ELSE 2 END) AS KBN	--出荷指示作成済分
            
        FROM D_Instruction AS DI
        LEFT OUTER JOIN D_DeliveryPlan AS DD
        ON DD.DeliveryPlanNO = DI.DeliveryPlanNO
        --AND DD.DeleteDateTime IS NULL
        LEFT OUTER JOIN D_Shipping AS DS
        ON DS.InstructionNO = DI.InstructionNO
        AND DS.DeleteDateTime IS NULL
        
        WHERE DI.DeliveryPlanDate >= (CASE WHEN @DeliveryPlanDateFrom <> '' THEN CONVERT(DATE, @DeliveryPlanDateFrom) ELSE DI.DeliveryPlanDate END)
        AND DI.DeliveryPlanDate <= (CASE WHEN @DeliveryPlanDateTo <> '' THEN CONVERT(DATE, @DeliveryPlanDateTo) ELSE DI.DeliveryPlanDate END)

        AND DI.DeliveryName LIKE '%' + ISNULL(@DeliveryName,'') + '%'
        AND DI.DeleteDateTime IS NULL

        AND ((@Chk1 = 1 AND DI.ExpressFLG <> 1 AND DI.DecidedDeliveryDate IS NULL AND DI.OntheDayFLG <> 1 AND DI.InstructionKBN <> 2)    --通常
            OR (@Chk2 = 1 AND DI.ExpressFLG = 1)    --至急CheckBox=ONの時
            OR (@Chk3 = 1 AND DI.DecidedDeliveryDate IS NOT NULL)
            OR (@Chk4 = 1 AND DI.OntheDayFLG = 1)
            OR (@Chk5 = 1 AND DI.InstructionKBN = 2)
            )
        AND EXISTS(SELECT M.SoukoCD FROM M_Souko AS M
                WHERE M.SoukoCD = DI.DeliverySoukoCD
                AND M.DeleteFlg = 0
                AND M.StoreCD = @StoreCD
            )
        AND (@ChkHakkozumi = 1 OR (@ChkHakkozumi = 0 AND DI.PrintDate IS NULL))
        AND (@ChkSyukkazumi = 1 OR (@ChkSyukkazumi = 0 AND NOT EXISTS(SELECT 1 FROM D_Shipping AS A
                                            WHERE A.InstructionNO = DI.InstructionNO
                                            AND A.DeleteDateTime IS NULL)))        
    --※画面項目転送表01のデータも含めての並び順
    ORDER BY DeliveryPlanDate, DeliveryPlanNO	
    ;

END


