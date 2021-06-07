BEGIN TRY 
 DROP PROCEDURE [dbo].[BindForWebJuchuuKakunin]
END TRY

BEGIN CATCH END CATCH 
GO

/****** Object:  StoredProcedure [dbo].[BindForWebJuchuuKakunin]    Script Date: 6/11/2019 2:21:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BindForWebJuchuuKakunin]
	-- Add the parameters for the stored procedure here
	@Kbn as tinyint,
	@ChangeDate as date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--�@	�󒍕ۗ����X�g�{�b�N�X�@���e�Z�b�g
    IF @Kbn = 1
    BEGIN
        Select      M_OnHold.OnHoldCD
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuOnHold.JuchuuNO))) + ')' AS OnHoldShortName
        From        M_OnHold
        Left Join   D_JuchuuOnHold on D_JuchuuOnHold.OnHoldCD = M_OnHold.OnHoldCD
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuOnHold.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   D_JuchuuDetails on D_JuchuuDetails.JuchuuNO = D_Juchuu.JuchuuNO
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Where       M_OnHold.JudgmentTiming = 1 --�i�󒍕ۗ��j              
          and       D_JuchuuOnHold.DisappeareDateTime is NULL    --:��������NULL
          and       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
        Group by    M_OnHold.OnHoldCD,M_OnHold.DisplayRows,M_OnHold.OnHoldShortName  
        Order by    M_OnHold.DisplayRows
        ;

    END
    
    --�A    �����󋵃��X�g�{�b�N�X�@���e�Z�b�g
    ELSE IF @Kbn = 2
    BEGIN
        --101�@�������@�̌����擾
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '101'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:����
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is NULL --:�����ԍ���NULL
          and not exists (Select 1 From D_JuchuuOnHold  --:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1    --:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�󒍕ۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --102�@�[���񓚑҂��@�̌����擾				
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '102'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:����
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is NULL --:���ח\�薢����
          and not exists (Select 1 From D_JuchuuOnHold  --:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1    --:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�󒍕ۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --103�@���ב҂��@�̌����擾
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '103'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:����
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is not NULL --:���ח\��ς�
          and       D_JuchuuDetails.ArriveNO is NULL --:���ז�����
          and not exists (Select 1 From D_JuchuuOnHold  --:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1    --:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�󒍕ۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        Order by    DisplayRows
        ;

	END
	
	--�B	�o�וۗ����X�g�{�b�N�X�@���e�Z�b�g
    ELSE IF @Kbn = 3
    BEGIN
        Select      M_OnHold.OnHoldCD
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuOnHold.JuchuuNO))) + ')' AS OnHoldShortName
        From        M_OnHold
        Left Join   D_JuchuuOnHold on D_JuchuuOnHold.OnHoldCD = M_OnHold.OnHoldCD
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuOnHold.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   D_JuchuuDetails on D_JuchuuDetails.JuchuuNO = D_Juchuu.JuchuuNO
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Where       M_OnHold.JudgmentTiming = 3                 --�i�o�וۗ��j
          and       D_JuchuuOnHold.DisappeareDateTime is NULL    --:��������NULL
          and       D_Juchuu.JuchuuKBN = 1                      --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
          ----�݌ɏo���ȊO�ƒ����̎��A�����`���ׂ��I����Ă邩
          and (     D_JuchuuDetails.HikiateFLG = 1              --:�݌ɏo��
             or ((  D_JuchuuDetails.DirectFLG = 1               --:����
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:���ח\��ς�
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:���׍ς�
          and not exists(Select 1 From D_JuchuuOnHold AS D_JOH  --:�󒍕ۗ�������   
                          Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JOH.OnHoldCD
                          Where D_JOH.JuchuuNO = D_JuchuuDetails.JuchuuNO   --:�󒍔ԍ�������
                            and M_OH.JudgmentTiming = 1                     --:���f�^�C�~���O���󒍕ۗ�         
                            and D_JOH.DisappeareDateTime is NULL)           --:�󒍕ۗ�����������Ă��Ȃ�           

        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName  
        Order by    M_OnHold.DisplayRows
        ;

    END
    
    --�C	�o�׏�ԃ��X�g�{�b�N�X�@���e�Z�b�g
    ELSE IF @Kbn = 4
    BEGIN
        --104�@�������@�̌����擾
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '104'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:�L�����Z������NULL
          ----�݌ɏo���ȊO�ƒ����̎��A�����`���ׂ��I����Ă邩
          and (     D_JuchuuDetails.HikiateFLG = 1              --:�݌ɏo��
             or ((  D_JuchuuDetails.DirectFLG = 1               --:����
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:���ח\��ς�
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:���׍ς�
          and not exists (Select 1 From D_JuchuuOnHold  		--:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1    		--:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�󒍕ۗ�����������Ă��Ȃ�
          and not exists (Select 1 From D_JuchuuOnHold  		--:�o�וۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 3    		--:���f�^�C�~���O���o�וۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�o�וۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DirectFLG <> 1							--:�����ł͂Ȃ�
          and       D_JuchuuDetails.HikiateSu <> D_JuchuuDetails.JuchuuSuu	--:������<>�󒍐�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --105�@�o�׎w���҂��@�̌����擾
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '105'
        Where       D_Juchuu.JuchuuKBN = 1  								--:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�  
          ----�݌ɏo���ȊO�ƒ����̎��A�����`���ׂ��I����Ă邩
          and (     D_JuchuuDetails.HikiateFLG = 1              --:�݌ɏo��
             or ((  D_JuchuuDetails.DirectFLG = 1               --:����
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:���ח\��ς�
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:���׍ς�
          and not exists (Select 1 From D_JuchuuOnHold  --:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1    --:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:�󒍕ۗ�����������Ă��Ȃ�
          and not exists (Select 1 From D_JuchuuOnHold          --:�o�וۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 3                            --:���f�^�C�~���O���o�וۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:�o�וۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DirectFLG <> 1                                  --:�����ł͂Ȃ�
          and       D_JuchuuDetails.HikiateSu = D_JuchuuDetails.JuchuuSuu           --:�����ς�
          and       D_JuchuuDetails.DeliveryOrderSu <> D_JuchuuDetails.JuchuuSuu    --:�o�׎w����<>�󒍐�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --106�@�o�ב҂��@�̌����擾
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '106'
        Where       D_Juchuu.JuchuuKBN = 1  								--:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:�o�א�<>�󒍐�                   
--          and       D_JuchuuDetails.CancelDate is NULL  					--:�L�����Z������NULL
          ----�݌ɏo���ȊO�ƒ����̎��A�����`���ׂ��I����Ă邩
          and (     D_JuchuuDetails.HikiateFLG = 1              --:�݌ɏo��
             or ((  D_JuchuuDetails.DirectFLG = 1               --:����
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:�݌ɈȊO
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:�����ς�
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:���ח\��ς�
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:���׍ς�
          and not exists (Select 1 From D_JuchuuOnHold          --:�󒍕ۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 1                            --:���f�^�C�~���O���󒍕ۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:�󒍕ۗ�����������Ă��Ȃ�
          and not exists (Select 1 From D_JuchuuOnHold                              --:�o�וۗ�������
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:�󒍔ԍ�������
                             and M_OH.JudgmentTiming = 3                            --:���f�^�C�~���O���o�וۗ�
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:�o�וۗ�����������Ă��Ȃ�
          and       D_JuchuuDetails.DirectFLG <> 1                                  --:�����ł͂Ȃ�
          and       D_JuchuuDetails.HikiateSu = D_JuchuuDetails.JuchuuSuu           --:�����ς�
          and       D_JuchuuDetails.DeliveryOrderSu = D_JuchuuDetails.JuchuuSuu    --:�o�׎w�����ς�
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        Order by    DisplayRows
        ;

	END
END

GO


