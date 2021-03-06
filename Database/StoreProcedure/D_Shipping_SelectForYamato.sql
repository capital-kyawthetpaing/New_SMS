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
          
          --CSV項目順
          ,DS.ShippingNO                         AS    col1    --出荷指示番号	
          ,1                                     AS    col2    --出荷指示明細番号  
          ,(SELECT top 1 M.SCatKBN1 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col3    --送り状種別    
          ,(SELECT top 1 ISNULL(M.SCatKBN1 ,DS.BoxSize)
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col4    --サイズ品目コード  
          ,0                                     AS    col5    --クール区分    
          ,Null                                  AS    col6    --ギフトフラグ  
          ,Null                                  AS    col7    --受注区分  
          ,Null                                  AS    col8    --送り状番号    
          ,CONVERT(varchar,DS.ShippingDate,111)  AS    col9    --出荷日    
          ,CONVERT(varchar,DS.DecidedDeliveryDate,111) 
                                                 AS    col10   --お届け予定日  
          ,DS.DecidedDeliveryTime                AS    col11   --納品時間指定区分  
          ,Null                                  AS    col12   --注文者コード  
          ,Null                                  AS    col13   --注文者電話番号    
          ,Null                                  AS    col14   --注文者郵便番号    
          ,Null                                  AS    col15   --注文者住所    
          ,Null                                  AS    col16   --注文者住所１  
          ,Null                                  AS    col17   --注文者住所２  
          ,Null                                  AS    col18   --注文者住所３  
          ,Null                                  AS    col19   --注文者氏名    
          ,Null                                  AS    col20   --注文者敬称    
          ,Null                                  AS    col21   --店舗コード    
          ,Null                                  AS    col22   --店舗電話番号  
          ,Null                                  AS    col23   --店舗FAX番号   
          ,Null                                  AS    col24   --店舗郵便番号  
          ,Null                                  AS    col25   --店舗住所  
          ,Null                                  AS    col26   --店舗住所１    
          ,Null                                  AS    col27   --店舗住所２    
          ,Null                                  AS    col28   --店舗住所３    
          ,Null                                  AS    col29   --店舗名    
          ,Null                                  AS    col30   --店舗URL   
          ,Null                                  AS    col31   --店舗メールアドレス    
          ,Null                                  AS    col32   --注文日    
          ,Null                                  AS    col33   --決済方法表示  
          ,Null                                  AS    col34   --小計  
          ,Null                                  AS    col35   --送料  
          ,Null                                  AS    col36   --決済料    
          ,Null                                  AS    col37   --手数料１  
          ,Null                                  AS    col38   --手数料２  
          ,Null                                  AS    col39   --手数料３  
          ,Null                                  AS    col40   --割引金額  
          ,Null                                  AS    col41   --請求金額  
          ,Null                                  AS    col42   --お届け先コード    
          ,DI.DeliveryTelphoneNO                 AS    col43   --お届け先電話番号  
          ,DI.DeliveryZip1CD + '-' + DI.DeliveryZip2CD
                                                 AS    col44   --お届け先郵便番号  
          ,LTRIM(RTRIM(DI.DeliveryAddress1)) + '　' + LTRIM(RTRIM(DI.DeliveryAddress2))
                                                 AS    col45   --お届け先住所  
          ,Null                                  AS    col46   --お届け先住所１    
          ,Null                                  AS    col47   --お届け先住所２    
          ,Null                                  AS    col48   --お届け先住所３    
          ,Null                                  AS    col49   --お届け先部門名１  
          ,Null                                  AS    col50   --お届け先部門名２  
          ,DI.DeliveryName                       AS    col51   --お届け先名    
          ,Null                                  AS    col52   --お届け先名略称カナ    
          ,Null                                  AS    col53   --お届け先敬称  
          ,Null                                  AS    col54   --依頼主コード  
          ,(SELECT top 1 W.TelephoneNO 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col55   --依頼主電話番号    
          ,(SELECT top 1 W.ZipCD1 + '-' + W.ZipCD2
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col56   --依頼主郵便番号    
          ,(SELECT top 1 LTRIM(RTRIM(W.Address1)) + '　' + LTRIM(RTRIM(W.Address2))
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col57      --依頼主住所    
          ,Null                                  AS    col58      --依頼主住所１   
          ,Null                                  AS    col59      --依頼主住所２   
          ,Null                                  AS    col60      --依頼主住所３   
          ,(SELECT top 1 W.InvoiceNotation 
            FROM M_Store AS W
            WHERE W.StoreCD = (SELECT top 1 M.StoreCD FROM M_Souko AS M 
                               WHERE M.SoukoCD = DS.SoukoCD
                               AND M.ChangeDate <= DS.ShippingDate
                               AND M.DeleteFlg = 0 
                               ORDER BY M.ChangeDate desc)
            AND W.ChangeDate <= DS.ShippingDate
            AND W.DeleteFlg = 0 
            ORDER BY W.ChangeDate desc)          AS    col61      --依頼主名   
          ,Null                                  AS    col62      --依頼主名略称カナ   
          ,(SELECT M.Char1
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col63      --荷扱い１ 「ワレモノ注意」を予定  
          ,(SELECT M.Char2
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col64      --荷扱い２   
          ,Null                                  AS    col65      --記事   
          ,Null                                  AS    col66      --品名コード１   
          ,(SELECT M.Char3
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col67      --品名１ 「通販商品（スポーツ用品・食品）」を予定
          ,Null                                  AS    col68      --品名コード２   
          ,(SELECT M.Char4
            FROM M_MultiPorpose AS M
            WHERE M.ID = 329
            AND M.[Key] = 1)                     AS    col69      --品名２ 
          ,DI.CashOnAmount                       AS    col70      --コレクト代金引換額 
          ,DI.CashOnIncludeTax                   AS    col71      --コレクト内消費税額等   
          ,0                                     AS    col72      --営業所止置き   
          ,0                                     AS    col73      --営業所コード   
          ,DS.UnitsCount                         AS    col74      --個口数 
          ,0                                     AS    col75      --個口数枠の印字 
          ,Null                                  AS    col76      --のし名入れ 
          ,Null                                  AS    col77      --出荷指示備考１ 
          ,Null                                  AS    col78      --出荷指示備考２ 
          ,Null                                  AS    col79      --出荷指示備考３ 
          ,Null                                  AS    col80      --出荷指示備考４ 
          ,Null                                  AS    col81      --運賃請求先コード   
          ,Null                                  AS    col82      --運賃請求先コード枝番   
          ,Null                                  AS    col83      --運賃管理番号   
          ,0                                     AS    col84      --出荷区分   
          ,Null                                  AS    col85      --商品番号   
          ,Null                                  AS    col86      --商品名表示 
          ,Null                                  AS    col87      --バリエーション 
          ,Null                                  AS    col88      --商品個数   
          ,Null                                  AS    col89      --単位   
          ,Null                                  AS    col90      --商品税込単価   
          ,Null                                  AS    col91      --商品金額   
          ,Null                                  AS    col92      --明細予備項目１ 
          ,Null                                  AS    col93      --明細予備項目２ 
          ,0                                     AS    col94      --お届け予定eメール　利用区分    
          ,Null                                  AS    col95      --お届け予定eメール　e-mailアドレス  
          ,Null                                  AS    col96      --入力機種   
          ,Null                                  AS    col97      --お届け予定eメール　メッセージ  
          ,0                                     AS    col98      --お届け完了eメール　利用区分    
          ,Null                                  AS    col99      --お届け完了eメール　e-mailアドレス  
          ,Null                                  AS    col100     --お届け完了eメール　メッセージ  
          ,Null                                  AS    col101     --出荷拠点コード 
          ,0                                     AS    col102     --あんしん決済　利用区分 
          ,Null                                  AS    col103     --あんしん決済　受付番号 
          ,Null                                  AS    col104     --あんしん決済　加盟店コード 
          ,Null                                  AS    col105     --あんしん決済　注文日   
          ,Null                                  AS    col106     --あんしん決済　取引先名 
          ,Null                                  AS    col107     --あんしん決済　決済金額（税込） 
          ,Null                                  AS    col108     --あんしん決済　会員ID   
          ,Null                                  AS    col109     --予備項目１ 
          ,Null                                  AS    col110     --予備項目２ 
          ,0                                     AS    col111     --投函予定メール利用区分 
          ,Null                                  AS    col112     --投函予定メールアドレス 
          ,Null                                  AS    col113     --投函予定メールメッセージ   
          ,0                                     AS    col114     --投函完了メール(お届け先宛)利用区分 
          ,Null                                  AS    col115     --投函完了メール(お届け先宛)アドレス 
          ,Null                                  AS    col116     --投函完了メール(お届け先宛)メッセージ   
          ,0                                     AS    col117     --投函完了メール(ご依頼主宛)利用区分 
          ,Null                                  AS    col118     --投函完了メール(ご依頼主宛)アドレス 
          ,Null                                  AS    col119     --投函完了メール(ご依頼主宛)メッセージ   
          ,(SELECT top 1 M.SCatKBN2 
            FROM M_Carrier AS M
            WHERE M.CarrierCD = DS.CarrierCD
            AND M.ChangeDate <= DS.ShippingDate
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc)          AS    col120     --運送会社区分   
          ,0                                     AS    col121     --西濃原票区分   
          ,Null                                  AS    col122     --重量   
          ,0                                     AS    col123     --保険金額   
          ,Null                                  AS    col124     --データ識別区分 
          ,Null                                  AS    col125     --受取店ロゴ分類 
          ,Null                                  AS    col126     --受取選択連携管理番号   
          ,Null                                  AS    col127     --受取選択直営店コード   
          ,Null                                  AS    col128     --受取選択荷受人コード   
          ,Null                                  AS    col129     --真荷主コード   
          ,Null                                  AS    col130     --JPセキュリティフラグ   

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
