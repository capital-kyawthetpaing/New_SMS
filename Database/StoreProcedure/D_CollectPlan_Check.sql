/****** Object:  StoredProcedure [dbo].[D_CollectPlan_Check]    Script Date: 2021/05/31 11:35:45 ******/
IF EXISTS (SELECT * FROM sys.procedures WHERE name like '%D_CollectPlan_Check%' and type like '%P%')
DROP PROCEDURE [dbo].[D_CollectPlan_Check]
GO

/****** Object:  StoredProcedure [dbo].[D_CollectPlan_Check]    Script Date: 2021/05/31 11:35:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_CollectPlan_Check]    */
CREATE PROCEDURE [dbo].[D_CollectPlan_Check](
    -- Add the parameters for the stored procedure here
    @Syori    tinyint,        -- �����敪�i1:������,2:�������L�����Z��,3:�����m��j
    @StoreCD  varchar(4),
    @CustomerCD  varchar(13),
    @ChangeDate  varchar(10),
    @BillingCloseDate tinyint
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    --������--
    IF @Syori = 1
    BEGIN
        --�����C�\��̑��݃`�F�b�N
        if not exists(SELECT DC.CollectPlanNO
            FROM D_CollectPlan AS DC
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DC.CustomerCD
            
            WHERE --DC.BillingNO IS Null	2019.10.23 chg
            DC.MonthlyBillingNO IS Null 
            AND DC.DeleteOperator IS Null       
            AND DC.DeleteDateTime IS Null       
            AND DC.StoreCD = @StoreCD
            AND DC.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DC.CustomerCD END)
            --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			--AND DC.BillingDate <= @ChangeDate   
			AND (DC.BillingDate IS NULL OR (DC.BillingDate IS NOT NULL AND DC.BillingDate <= @ChangeDate))
			--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
            AND DC.InvalidFLG = 0
            AND DC.BillingConfirmFlg = 0
            AND DC.BillingType = 2
            )
        begin
            --Select�ł��Ȃ����Error
            select 'S013' as MessageID
            return;
        end

        --������t�̐������`�F�b�N
        if exists(SELECT DC.CollectPlanNO
            FROM D_CollectPlan AS DC
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DC.CustomerCD
            INNER JOIN D_Billing AS DB ON DB.StoreCD = DC.StoreCD
            AND DB.BillingCustomerCD = DC.CustomerCD
            AND DB.BillingCloseDate >= @ChangeDate    
            AND DB.DeleteDateTime IS Null   
            WHERE DC.BillingNO IS Null       
            AND DC.DeleteOperator IS Null       
            AND DC.DeleteDateTime IS Null       
            AND DC.StoreCD = @StoreCD
            AND DC.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DC.CustomerCD END)
            --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			--AND DC.BillingDate <= @ChangeDate   
			AND (DC.BillingDate IS NULL OR (DC.BillingDate IS NOT NULL AND DC.BillingDate <= @ChangeDate))
			--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���   
            AND DC.InvalidFLG = 0
            AND DC.BillingConfirmFlg = 0
            AND DC.BillingType = 2
        )
        begin
            --Select�ł����Error
            select 'S016' as MessageID
            return;
        end
        
        --�������ߒ��̃`�F�b�N
        if exists(SELECT DC.CollectPlanNO
            FROM D_CollectPlan AS DC
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DC.CustomerCD
            INNER JOIN D_Billing AS DB ON DB.StoreCD = DC.StoreCD
            AND DB.BillingCustomerCD = DC.CustomerCD
            AND DB.BillingConfirmFlg = 0
            AND DB.DeleteDateTime IS Null   
            
            WHERE DC.BillingNO IS Null       
            AND DC.DeleteOperator IS Null       
            AND DC.DeleteDateTime IS Null       
            AND DC.StoreCD = @StoreCD
            AND DC.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DC.CustomerCD END)
            --2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���
			--AND DC.BillingDate <= @ChangeDate   
			AND (DC.BillingDate IS NULL OR (DC.BillingDate IS NOT NULL AND DC.BillingDate <= @ChangeDate))
			--2021/05/31 Y.Nishikawa CHG ��������BillingDate��NULL��ԁ���   
            AND DC.InvalidFLG = 0
            AND DC.BillingConfirmFlg = 0
            AND DC.BillingType = 2
        )
        begin
            --Select�ł����Error
            select 'S017' as MessageID
            return;
        end
    END

    --��������ݾ�--
    ELSE IF @Syori = 2
    BEGIN
        if NOT exists(SELECT DB.BillingNO
            FROM D_Billing AS DB
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingConfirmFlg = 0
            AND DB.DeleteDateTime IS Null   
        )
        begin
            --Select�ł��Ȃ����Error
            select 'S013' as MessageID
            return;
        end
        
        --���̂Ƃ��AD_Billing.CollectGaku ��0�̃��R�\�h�����݂���΃G���[
        --�i���ɓ������s���Ă��邽�ߎ���ł��܂���j
        if exists(SELECT DB.BillingNO
            FROM D_Billing AS DB
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingConfirmFlg = 0
            AND DB.DeleteDateTime IS Null
            AND DB.CollectGaku <> 0   
        )
        begin
            --Select�ł��Ȃ����Error
            select 'E152' as MessageID
            return;
        end
    END
    
    --�����m��--
    ELSE IF @Syori = 3
    BEGIN
        if NOT exists(SELECT DB.BillingNO
            FROM D_Billing AS DB
            INNER JOIN (SELECT MC.CustomerCD, MAX(MC.ChangeDate) AS ChangeDate
                FROM M_Customer AS MC 
                WHERE MC.ChangeDate <= @ChangeDate
                AND MC.BillingCloseDate = @BillingCloseDate
                AND MC.DeleteFlg = 0
                GROUP BY MC.CustomerCD) AS MMC ON MMC.CustomerCD = DB.BillingCustomerCD
          WHERE DB.StoreCD = @StoreCD
            AND DB.BillingCustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DB.BillingCustomerCD END)
            AND DB.BillingConfirmFlg = 0
            AND DB.DeleteDateTime IS Null   
        )
        begin
            --Select�ł��Ȃ����Error
            select 'S013' as MessageID
            return;
        end

    END
    
    
	--Check OK
    select '' as MessageID
    return;

END


GO


