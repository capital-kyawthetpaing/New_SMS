/****** Object:  StoredProcedure [dbo].[D_Shipping_SelectForYamato]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Shipping_SelectForYamato]
GO

/****** Object:  StoredProcedure [D_Shipping_SelectForYamato]    */
CREATE PROCEDURE D_Shipping_SelectForYamato(
    -- Add the parameters for the stored procedure here
    @ShippingNO  varchar(11)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DS.LinkageDateTime
          ,(SELECT top 1 M.StoreCD FROM M_Souko AS M 
            WHERE M.SoukoCD = DS.SoukoCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    StoreCD
          
          --CSV€–Ú‡
          ,DS.ShippingNO                         AS    col1    --o‰×ŽwŽ¦”Ô†	
          ,1                                     AS    col2    --o‰×ŽwŽ¦–¾×”Ô†  
          ,(SELECT top 1 M.SCatKBN1 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col3    --‘—‚èóŽí•Ê    
          ,(SELECT top 1 ISNULL(M.SCatKBN1 ,DS.BoxSize)
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col4    --ƒTƒCƒY•i–ÚƒR[ƒh  
          ,0                                     AS    col5    --ƒN[ƒ‹‹æ•ª    
          ,Null                                  AS    col6    --ƒMƒtƒgƒtƒ‰ƒO  
          ,Null                                  AS    col7    --Žó’‹æ•ª  
          ,Null                                  AS    col8    --‘—‚èó”Ô†    
          ,CONVERT(varchar,DS.ShippingDate,111)  AS    col9    --o‰×“ú    
          ,CONVERT(varchar,DS.DecidedDeliveryDate,111) 
                                                 AS    col10   --‚¨“Í‚¯—\’è“ú  
          ,DS.DecidedDeliveryTime                AS    col11   --”[•iŽžŠÔŽw’è‹æ•ª  
          ,Null                                  AS    col12   --’•¶ŽÒƒR[ƒh  
          ,Null                                  AS    col13   --’•¶ŽÒ“d˜b”Ô†    
          ,Null                                  AS    col14   --’•¶ŽÒ—X•Ö”Ô†    
          ,Null                                  AS    col15   --’•¶ŽÒZŠ    
          ,Null                                  AS    col16   --’•¶ŽÒZŠ‚P  
          ,Null                                  AS    col17   --’•¶ŽÒZŠ‚Q  
          ,Null                                  AS    col18   --’•¶ŽÒZŠ‚R  
          ,Null                                  AS    col19   --’•¶ŽÒŽ–¼    
          ,Null                                  AS    col20   --’•¶ŽÒŒhÌ    
          ,Null                                  AS    col21   --“X•ÜƒR[ƒh    
          ,Null                                  AS    col22   --“X•Ü“d˜b”Ô†  
          ,Null                                  AS    col23   --“X•ÜFAX”Ô†   
          ,Null                                  AS    col24   --“X•Ü—X•Ö”Ô†  
          ,Null                                  AS    col25   --“X•ÜZŠ  
          ,Null                                  AS    col26   --“X•ÜZŠ‚P    
          ,Null                                  AS    col27   --“X•ÜZŠ‚Q    
          ,Null                                  AS    col28   --“X•ÜZŠ‚R    
          ,Null                                  AS    col29   --“X•Ü–¼    
          ,Null                                  AS    col30   --“X•ÜURL   
          ,Null                                  AS    col31   --“X•Üƒ[ƒ‹ƒAƒhƒŒƒX    
          ,Null                                  AS    col32   --’•¶“ú    
          ,Null                                  AS    col33   --ŒˆÏ•û–@•\Ž¦  
          ,Null                                  AS    col34   --¬Œv  
          ,Null                                  AS    col35   --‘——¿  
          ,Null                                  AS    col36   --ŒˆÏ—¿    
          ,Null                                  AS    col37   --Žè”—¿‚P  
          ,Null                                  AS    col38   --Žè”—¿‚Q  
          ,Null                                  AS    col39   --Žè”—¿‚R  
          ,Null                                  AS    col40   --Š„ˆø‹àŠz  
          ,Null                                  AS    col41   --¿‹‹àŠz  
          ,Null                                  AS    col42   --‚¨“Í‚¯æƒR[ƒh    
          ,DI.DeliveryTelphoneNO                 AS    col43   --‚¨“Í‚¯æ“d˜b”Ô†  
          ,DI.DeliveryZip1CD + '-' + DI.DeliveryZip2CD
                                                 AS    col44   --‚¨“Í‚¯æ—X•Ö”Ô†  
          ,LTRIM(RTRIM(DI.DeliveryAddress1)) + '@' + LTRIM(RTRIM(DI.DeliveryAddress2))
                                                 AS    col45   --‚¨“Í‚¯æZŠ  
          ,Null                                  AS    col46   --‚¨“Í‚¯æZŠ‚P    
          ,Null                                  AS    col47   --‚¨“Í‚¯æZŠ‚Q    
          ,Null                                  AS    col48   --‚¨“Í‚¯æZŠ‚R    
          ,Null                                  AS    col49   --‚¨“Í‚¯æ•”–å–¼‚P  
          ,Null                                  AS    col50   --‚¨“Í‚¯æ•”–å–¼‚Q  
          ,DI.DeliveryName                       AS    col51   --‚¨“Í‚¯æ–¼    
          ,Null                                  AS    col52   --‚¨“Í‚¯æ–¼—ªÌƒJƒi    
          ,Null                                  AS    col53   --‚¨“Í‚¯æŒhÌ  
          ,Null                                  AS    col54   --ˆË—ŠŽåƒR[ƒh  
          ,(SELECT top 1 W.TelephoneNO 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col55   --ˆË—ŠŽå“d˜b”Ô†    
          ,(SELECT top 1 W.ZipCD1 + '-' + W.ZipCD2
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col56   --ˆË—ŠŽå—X•Ö”Ô†    
          ,(SELECT top 1 LTRIM(RTRIM(W.Address1)) + '@' + LTRIM(RTRIM(W.Address2))
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col57      --ˆË—ŠŽåZŠ    
          ,Null                                  AS    col58      --ˆË—ŠŽåZŠ‚P   
          ,Null                                  AS    col59      --ˆË—ŠŽåZŠ‚Q   
          ,Null                                  AS    col60      --ˆË—ŠŽåZŠ‚R   
          ,(SELECT top 1 W.InvoiceNotation 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col61      --ˆË—ŠŽå–¼   
          ,Null                                  AS    col62      --ˆË—ŠŽå–¼—ªÌƒJƒi   
          ,(SELECT M.Char1
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col63      --‰×ˆµ‚¢‚P uƒƒŒƒ‚ƒm’ˆÓv‚ð—\’è  
          ,(SELECT M.Char2
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col64      --‰×ˆµ‚¢‚Q   
          ,Null                                  AS    col65      --‹LŽ–   
          ,Null                                  AS    col66      --•i–¼ƒR[ƒh‚P   
          ,(SELECT M.Char3
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col67      --•i–¼‚P u’Ê”Ì¤•iiƒXƒ|[ƒc—p•iEH•ijv‚ð—\’è
          ,Null                                  AS    col68      --•i–¼ƒR[ƒh‚Q   
          ,(SELECT M.Char4
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col69      --•i–¼‚Q 
          ,DI.CashOnAmount                       AS    col70      --ƒRƒŒƒNƒg‘ã‹àˆøŠ·Šz 
          ,DI.CashOnIncludeTax                   AS    col71      --ƒRƒŒƒNƒg“àÁ”ïÅŠz“™   
          ,0                                     AS    col72      --‰c‹ÆŠŽ~’u‚«   
          ,0                                     AS    col73      --‰c‹ÆŠƒR[ƒh   
          ,DS.UnitsCount                         AS    col74      --ŒÂŒû” 
          ,0                                     AS    col75      --ŒÂŒû”˜g‚ÌˆóŽš 
          ,Null                                  AS    col76      --‚Ì‚µ–¼“ü‚ê 
          ,Null                                  AS    col77      --o‰×ŽwŽ¦”õl‚P 
          ,Null                                  AS    col78      --o‰×ŽwŽ¦”õl‚Q 
          ,Null                                  AS    col79      --o‰×ŽwŽ¦”õl‚R 
          ,Null                                  AS    col80      --o‰×ŽwŽ¦”õl‚S 
          ,Null                                  AS    col81      --‰^’À¿‹æƒR[ƒh   
          ,Null                                  AS    col82      --‰^’À¿‹æƒR[ƒhŽ}”Ô   
          ,Null                                  AS    col83      --‰^’ÀŠÇ—”Ô†   
          ,0                                     AS    col84      --o‰×‹æ•ª   
          ,Null                                  AS    col85      --¤•i”Ô†   
          ,Null                                  AS    col86      --¤•i–¼•\Ž¦ 
          ,Null                                  AS    col87      --ƒoƒŠƒG[ƒVƒ‡ƒ“ 
          ,Null                                  AS    col88      --¤•iŒÂ”   
          ,Null                                  AS    col89      --’PˆÊ   
          ,Null                                  AS    col90      --¤•iÅž’P‰¿   
          ,Null                                  AS    col91      --¤•i‹àŠz   
          ,Null                                  AS    col92      --–¾×—\”õ€–Ú‚P 
          ,Null                                  AS    col93      --–¾×—\”õ€–Ú‚Q 
          ,0                                     AS    col94      --‚¨“Í‚¯—\’èeƒ[ƒ‹@—˜—p‹æ•ª    
          ,Null                                  AS    col95      --‚¨“Í‚¯—\’èeƒ[ƒ‹@e-mailƒAƒhƒŒƒX  
          ,Null                                  AS    col96      --“ü—Í‹@Ží   
          ,Null                                  AS    col97      --‚¨“Í‚¯—\’èeƒ[ƒ‹@ƒƒbƒZ[ƒW  
          ,0                                     AS    col98      --‚¨“Í‚¯Š®—¹eƒ[ƒ‹@—˜—p‹æ•ª    
          ,Null                                  AS    col99      --‚¨“Í‚¯Š®—¹eƒ[ƒ‹@e-mailƒAƒhƒŒƒX  
          ,Null                                  AS    col100     --‚¨“Í‚¯Š®—¹eƒ[ƒ‹@ƒƒbƒZ[ƒW  
          ,Null                                  AS    col101     --o‰×‹’“_ƒR[ƒh 
          ,0                                     AS    col102     --‚ ‚ñ‚µ‚ñŒˆÏ@—˜—p‹æ•ª 
          ,Null                                  AS    col103     --‚ ‚ñ‚µ‚ñŒˆÏ@Žó•t”Ô† 
          ,Null                                  AS    col104     --‚ ‚ñ‚µ‚ñŒˆÏ@‰Á–¿“XƒR[ƒh 
          ,Null                                  AS    col105     --‚ ‚ñ‚µ‚ñŒˆÏ@’•¶“ú   
          ,Null                                  AS    col106     --‚ ‚ñ‚µ‚ñŒˆÏ@Žæˆøæ–¼ 
          ,Null                                  AS    col107     --‚ ‚ñ‚µ‚ñŒˆÏ@ŒˆÏ‹àŠziÅžj 
          ,Null                                  AS    col108     --‚ ‚ñ‚µ‚ñŒˆÏ@‰ïˆõID   
          ,Null                                  AS    col109     --—\”õ€–Ú‚P 
          ,Null                                  AS    col110     --—\”õ€–Ú‚Q 
          ,0                                     AS    col111     --“Š”Ÿ—\’èƒ[ƒ‹—˜—p‹æ•ª 
          ,Null                                  AS    col112     --“Š”Ÿ—\’èƒ[ƒ‹ƒAƒhƒŒƒX 
          ,Null                                  AS    col113     --“Š”Ÿ—\’èƒ[ƒ‹ƒƒbƒZ[ƒW   
          ,0                                     AS    col114     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚¨“Í‚¯æˆ¶)—˜—p‹æ•ª 
          ,Null                                  AS    col115     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚¨“Í‚¯æˆ¶)ƒAƒhƒŒƒX 
          ,Null                                  AS    col116     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚¨“Í‚¯æˆ¶)ƒƒbƒZ[ƒW   
          ,0                                     AS    col117     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚²ˆË—ŠŽåˆ¶)—˜—p‹æ•ª 
          ,Null                                  AS    col118     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚²ˆË—ŠŽåˆ¶)ƒAƒhƒŒƒX 
          ,Null                                  AS    col119     --“Š”ŸŠ®—¹ƒ[ƒ‹(‚²ˆË—ŠŽåˆ¶)ƒƒbƒZ[ƒW   
          ,(SELECT top 1 M.SCatKBN2 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col120     --‰^‘—‰ïŽÐ‹æ•ª   
          ,0                                     AS    col121     --¼”ZŒ´•[‹æ•ª   
          ,Null                                  AS    col122     --d—Ê   
          ,0                                     AS    col123     --•ÛŒ¯‹àŠz   
          ,Null                                  AS    col124     --ƒf[ƒ^Ž¯•Ê‹æ•ª 
          ,Null                                  AS    col125     --ŽóŽæ“XƒƒS•ª—Þ 
          ,Null                                  AS    col126     --ŽóŽæ‘I‘ð˜AŒgŠÇ—”Ô†   
          ,Null                                  AS    col127     --ŽóŽæ‘I‘ð’¼‰c“XƒR[ƒh   
          ,Null                                  AS    col128     --ŽóŽæ‘I‘ð‰×ŽólƒR[ƒh   
          ,Null                                  AS    col129     --^‰×ŽåƒR[ƒh   
          ,Null                                  AS    col130     --JPƒZƒLƒ…ƒŠƒeƒBƒtƒ‰ƒO   

    FROM D_Shipping AS DS
    LEFT OUTER JOIN D_ShippingDetails AS DM
    ON DM.ShippingNO = DS.ShippingNO
    AND DM.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Instruction AS DI
    ON DI.InstructionNO = DS.InstructionNO
    AND DI.DeleteDateTime IS NULL
    
    WHERE DS.DeleteDateTime IS NULL
    AND DS.ShippingNO = @ShippingNO
    ORDER BY DM.ShippingRows
    ;

END

GO
