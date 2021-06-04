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
	--①	受注保留リストボックス　内容セット
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
        Where       M_OnHold.JudgmentTiming = 1 --（受注保留）              
          and       D_JuchuuOnHold.DisappeareDateTime is NULL    --:解消日がNULL
          and       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
        Group by    M_OnHold.OnHoldCD,M_OnHold.DisplayRows,M_OnHold.OnHoldShortName  
        Order by    M_OnHold.DisplayRows
        ;

    END
    
    --②    発注状況リストボックス　内容セット
    ELSE IF @Kbn = 2
    BEGIN
        --101　未発注　の件数取得
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '101'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:直送
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is NULL --:発注番号がNULL
          and not exists (Select 1 From D_JuchuuOnHold  --:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1    --:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:受注保留が解消されていない
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --102　納期回答待ち　の件数取得				
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '102'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:直送
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is NULL --:入荷予定未入力
          and not exists (Select 1 From D_JuchuuOnHold  --:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1    --:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:受注保留が解消されていない
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --103　入荷待ち　の件数取得
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '103'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
          and       (D_JuchuuDetails.DirectFLG = 1  --:直送
            or      D_JuchuuDetails.HikiateFLG <> 1)    --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is not NULL --:入荷予定済み
          and       D_JuchuuDetails.ArriveNO is NULL --:入荷未入力
          and not exists (Select 1 From D_JuchuuOnHold  --:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1    --:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:受注保留が解消されていない
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        Order by    DisplayRows
        ;

	END
	
	--③	出荷保留リストボックス　内容セット
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
        Where       M_OnHold.JudgmentTiming = 3                 --（出荷保留）
          and       D_JuchuuOnHold.DisappeareDateTime is NULL    --:解消日がNULL
          and       D_Juchuu.JuchuuKBN = 1                      --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
          ----在庫出し以外と直送の時、発注～入荷が終わってるか
          and (     D_JuchuuDetails.HikiateFLG = 1              --:在庫出し
             or ((  D_JuchuuDetails.DirectFLG = 1               --:直送
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:入荷予定済み
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:入荷済み
          and not exists(Select 1 From D_JuchuuOnHold AS D_JOH  --:受注保留が無い   
                          Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JOH.OnHoldCD
                          Where D_JOH.JuchuuNO = D_JuchuuDetails.JuchuuNO   --:受注番号が同じ
                            and M_OH.JudgmentTiming = 1                     --:判断タイミングが受注保留         
                            and D_JOH.DisappeareDateTime is NULL)           --:受注保留が解消されていない           

        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName  
        Order by    M_OnHold.DisplayRows
        ;

    END
    
    --④	出荷状態リストボックス　内容セット
    ELSE IF @Kbn = 4
    BEGIN
        --104　未引当　の件数取得
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '104'
        Where       D_Juchuu.JuchuuKBN = 1  --:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  --:キャンセル日がNULL
          ----在庫出し以外と直送の時、発注～入荷が終わってるか
          and (     D_JuchuuDetails.HikiateFLG = 1              --:在庫出し
             or ((  D_JuchuuDetails.DirectFLG = 1               --:直送
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:入荷予定済み
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:入荷済み
          and not exists (Select 1 From D_JuchuuOnHold  		--:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1    		--:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:受注保留が解消されていない
          and not exists (Select 1 From D_JuchuuOnHold  		--:出荷保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 3    		--:判断タイミングが出荷保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:出荷保留が解消されていない
          and       D_JuchuuDetails.DirectFLG <> 1							--:直送ではない
          and       D_JuchuuDetails.HikiateSu <> D_JuchuuDetails.JuchuuSuu	--:引当数<>受注数
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --105　出荷指示待ち　の件数取得
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '105'
        Where       D_Juchuu.JuchuuKBN = 1  								--:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数  
          ----在庫出し以外と直送の時、発注～入荷が終わってるか
          and (     D_JuchuuDetails.HikiateFLG = 1              --:在庫出し
             or ((  D_JuchuuDetails.DirectFLG = 1               --:直送
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:入荷予定済み
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:入荷済み
          and not exists (Select 1 From D_JuchuuOnHold  --:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1    --:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL) --:受注保留が解消されていない
          and not exists (Select 1 From D_JuchuuOnHold          --:出荷保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 3                            --:判断タイミングが出荷保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:出荷保留が解消されていない
          and       D_JuchuuDetails.DirectFLG <> 1                                  --:直送ではない
          and       D_JuchuuDetails.HikiateSu = D_JuchuuDetails.JuchuuSuu           --:引当済み
          and       D_JuchuuDetails.DeliveryOrderSu <> D_JuchuuDetails.JuchuuSuu    --:出荷指示数<>受注数
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        UNION ALL
        
        --106　出荷待ち　の件数取得
        Select      M_OnHold.OnHoldCD, M_OnHold.DisplayRows
                  , M_OnHold.OnHoldShortName + '(' + convert(varchar, count(distinct(D_JuchuuDetails.JuchuuNO))) + ')' AS OnHoldShortName
        From        D_JuchuuDetails
        Left Join   D_Juchuu on D_Juchuu.JuchuuNO = D_JuchuuDetails.JuchuuNO     
          and       D_Juchuu.DeleteDateTime is NULL
        Left Join   M_OnHold on M_OnHold.OnHoldCD = '106'
        Where       D_Juchuu.JuchuuKBN = 1  								--:WEB                  
          and       D_JuchuuDetails.DeliverySu <> D_JuchuuDetails.JuchuuSuu --:出荷数<>受注数                   
--          and       D_JuchuuDetails.CancelDate is NULL  					--:キャンセル日がNULL
          ----在庫出し以外と直送の時、発注～入荷が終わってるか
          and (     D_JuchuuDetails.HikiateFLG = 1              --:在庫出し
             or ((  D_JuchuuDetails.DirectFLG = 1               --:直送
                or  D_JuchuuDetails.HikiateFLG <> 1)            --:在庫以外
          and       D_JuchuuDetails.LastOrderNO is not NULL     --:発注済み
          and       D_JuchuuDetails.ArrivePlanNO is not NULL    --:入荷予定済み
          and       D_JuchuuDetails.ArriveNO is not NULL))      --:入荷済み
          and not exists (Select 1 From D_JuchuuOnHold          --:受注保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 1                            --:判断タイミングが受注保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:受注保留が解消されていない
          and not exists (Select 1 From D_JuchuuOnHold                              --:出荷保留が無い
                           Inner join M_OnHold as M_OH on M_OH.OnHoldCD = D_JuchuuOnHold.OnHoldCD   
                           Where D_JuchuuOnHold.JuchuuNO = D_JuchuuDetails.JuchuuNO --:受注番号が同じ
                             and M_OH.JudgmentTiming = 3                            --:判断タイミングが出荷保留
                             and D_JuchuuOnHold.DisappeareDateTime is NULL)         --:出荷保留が解消されていない
          and       D_JuchuuDetails.DirectFLG <> 1                                  --:直送ではない
          and       D_JuchuuDetails.HikiateSu = D_JuchuuDetails.JuchuuSuu           --:引当済み
          and       D_JuchuuDetails.DeliveryOrderSu = D_JuchuuDetails.JuchuuSuu    --:出荷指示数済み
          and       D_JuchuuDetails.DeleteDateTime is NULL
        Group by    M_OnHold.OnHoldCD, M_OnHold.DisplayRows,M_OnHold.OnHoldShortName
        
        Order by    DisplayRows
        ;

	END
END

GO


