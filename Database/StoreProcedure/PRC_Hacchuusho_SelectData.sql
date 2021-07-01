
IF EXISTS (select * from sys.objects where name = 'PRC_Hacchuusho_SelectData')
begin
    DROP PROCEDURE PRC_Hacchuusho_SelectData
end
GO
/****** Object:  StoredProcedure [dbo].[PRC_Hacchuusho_SelectData]    Script Date: 2020/09/23 16:53:51 ******/


/****** Object:  StoredProcedure [dbo].[PRC_Hacchuusho_SelectData]    Script Date: 2020/09/23 16:53:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[PRC_Hacchuusho_SelectData](
 @p_StoreCD                             varchar(4)
,@p_InsatuTaishou_Mihakkou              bit
,@p_InsatuTaishou_Saihakkou             bit
,@p_HacchuuDateFrom                     varchar(10)
,@p_HacchuuDateTo                       varchar(10)
,@p_Vendor                              varchar(13)
,@p_Staff                               varchar(10)
,@p_HacchuuNO                           varchar(11)
,@p_IsPrintMisshounin                   bit
,@p_IsPrintEDIHacchuu                   bit
,@p_InsatuShurui_Hacchhusho             bit
,@p_InsatuShurui_NetHacchuu             bit
,@p_InsatuShurui_Chokusou               bit
,@p_InsatuShurui_Cancel                 bit
)
AS
BEGIN

    SELECT ShouninJoutai                    = MAIN.ShouninJoutai  
          ,OrderNO                          = MAIN.OrderNO        
          ,HakkouDate                       = MAIN.HakkouDate     
          ,PageNO                           = DENSE_RANK()OVER(ORDER BY MAIN.OrderNO,MAIN.InsatuShuruiKBN,MAIN.PageNO)
          ,SubPageNO                        = CAST(MAIN.PageNO as varchar)
          ,MaxPageNO                        = CAST((MAX(MAIN.PageNO)OVER(PARTITION BY MAIN.OrderNO)) as varchar)
          ,OrderCD                          = MAIN.OrderCD           
          ,VendorName                       = MAIN.VendorName        
          ,OrderPerson                      = MAIN.OrderPerson       
          ,VendorKeishou_Sama               = MAIN.VendorKeishou_Sama      
          ,VendorKeishou_Onchuu             = MAIN.VendorKeishou_Onchuu    
          ,NouhinsakiZipCD                  = MAIN.NouhinsakiZipCD
          ,NouhinsakiName                   = MAIN.NouhinsakiName    
          ,NouhinsakiJuusho1                = MAIN.NouhinsakiJuusho1 
          ,NouhinsakiJuusho2                = MAIN.NouhinsakiJuusho2 
          ,NouhinsakiTELNO                  = MAIN.NouhinsakiTELNO   
          ,TyakuYoteiDate                   = MAIN.TyakuYoteiDate
          ,NouhinsakiChar1                  = MAIN.NouhinsakiChar1   
          ,NouhinsakiChar2                  = MAIN.NouhinsakiChar2   
          ,DetailGyouNO                     = MAIN.DetailGyouNO      
          ,MakerItem                        = MAIN.MakerItem         
          ,JANCD                            = MAIN.JANCD             
          ,SKUName                          = MAIN.SKUName           
          ,ColorSizeName                    = MAIN.ColorSizeName     
          ,CommentOutStore                  = MAIN.CommentOutStore   
          ,OrderUnitPrice                   = MAIN.OrderUnitPrice    
          ,OrderGaku                        = MAIN.OrderGaku           
          ,OrderSu                          = MAIN.OrderSu           
          ,TaniName                         = MAIN.TaniName          
          ,JuchuuNO                         = MAIN.JuchuuNO          
          ,OrderHontaiGaku                  = MAIN.OrderHontaiGaku   
          ,RemarksOutStore                  = MAIN.RemarksOutStore   
          ,Print1                           = MAIN.Print1            
          ,Print2                           = MAIN.Print2            
          ,Print3                           = MAIN.Print3            
          ,Print4                           = MAIN.Print4            
          ,Print5                           = MAIN.Print5            
          ,Print6                           = MAIN.Print6      
          ,InsatuShuruiKBN                  = MAIN.InsatuShuruiKBN  --1:通常発注、2:Net発注、3:直送発注、4:キャンセル発注
          ,FirstOrderNO                     = MAIN.FirstOrderNO

    FROM (SELECT ShouninJoutai                    = CASE WHEN DODH.ApprovalStageFLG >= 9 THEN '' ELSE '※未承認※' END
                ,OrderNO                          = DODH.OrderNO
                ,HakkouDate                       = FORMAT(GETDATE(),'yyyy/MM/dd HH:mm')
                ,PageNO                           = ((ROW_NUMBER()OVER(PARTITION BY DODH.OrderNO ORDER BY DODD.DisplayRows) - 1) / 5) + 1
                ,OrderCD                          = DODH.OrderCD
                ,VendorName                       = MVEN.VendorName
                ,OrderPerson                      = DODH.OrderPerson
                ,VendorKeishou_Sama               = CASE WHEN DODH.AliasKBN = 1 THEN '様' ELSE NULL END
                ,VendorKeishou_Onchuu             = CASE WHEN DODH.AliasKBN = 2 THEN '御中' ELSE NULL END
                ,NouhinsakiZipCD                  = CASE WHEN SUB_InsatuShurui.Value = 3 THEN ISNULL(DODH.DestinationZip1CD,'') + '-' + ISNULL(DODH.DestinationZip2CD,'') ELSE ISNULL(MSOU.ZipCD1,'') + '-' + ISNULL(MSOU.ZipCD2,'') END
                ,NouhinsakiName                   = CASE WHEN SUB_InsatuShurui.Value = 3 THEN DODH.DestinationName ELSE MSOU.SoukoName END 
                ,NouhinsakiJuusho1                = CASE WHEN SUB_InsatuShurui.Value = 3 THEN DODH.DestinationAddress1 ELSE MSOU.Address1 END 
                ,NouhinsakiJuusho2                = CASE WHEN SUB_InsatuShurui.Value = 3 THEN DODH.DestinationAddress2 ELSE MSOU.Address2 END 
                ,NouhinsakiTELNO                  = CASE WHEN SUB_InsatuShurui.Value = 3 THEN DODH.DestinationTelphoneNO ELSE MSOU.TelephoneNO END
                ,TyakuYoteiDate                   = FORMAT(MIN(DODD.DesiredDeliveryDate)OVER(PARTITION BY DODH.OrderNO),'yyyy/MM/dd')
                ,NouhinsakiChar1                  = MMPP_Nouhinsaki.Char1
                ,NouhinsakiChar2                  = MMPP_Nouhinsaki.Char2
                ,DetailGyouNO                     = ROW_NUMBER()OVER(PARTITION BY DODH.OrderNO ORDER BY DODD.DisplayRows)
                ,MakerItem                        = CASE WHEN MSKU.VariousFLG = 0 THEN MSKU.MakerItem ELSE DODD.MakerItem END
                ,JANCD                            = MSKU.JanCD
                ,SKUName                          = CASE WHEN MSKU.VariousFLG = 0 THEN MSKU.SKUName ELSE DODD.ItemName END
                ,ColorSizeName                    = CASE WHEN MSKU.VariousFLG = 0 THEN ISNULL(MSKU.ColorName,'') + '  ' + ISNULL(MSKU.SizeName,'')
                                                                                  ELSE ISNULL(DODD.ColorName,'') + '  ' + ISNULL(DODD.SizeName,'')
                                                    END
                ,CommentOutStore                  = DODD.CommentOutStore
                ,OrderUnitPrice                   = DODD.OrderUnitPrice
                ,OrderGaku                        = DODD.OrderHontaiGaku
                ,OrderSu                          = DODD.OrderSu
                ,TaniName                         = MMPP_Tani.Char1
                ,JuchuuNO                         = DODD.JuchuuNO
                ,OrderHontaiGaku                  = DODH.OrderHontaiGaku
                ,RemarksOutStore                  = DODH.CommentOutStore
                ,Print1                           = MSTR.Print1 
                ,Print2                           = MSTR.Print2 
                ,Print3                           = MSTR.Print3 
                ,Print4                           = MSTR.Print4 
                ,Print5                           = MSTR.Print5 
                ,Print6                           = MSTR.Print6 
                ,InsatuShuruiKBN                  = SUB_InsatuShurui.Value
                ,FirstOrderNO                     = CASE WHEN SUB_InsatuShurui.Value = 4 THEN DODD.FirstOrderNO ELSE '' END
          
          FROM D_Order DODH
          LEFT JOIN D_OrderDetails DODD
          ON  DODH.OrderNO = DODD.OrderNO
          AND DODD.DeleteDateTime IS NULL
          OUTER APPLY (SELECT *
                         FROM Fnc_M_SKU_SelectLatest(DODH.OrderDate)MSKU  
                        WHERE MSKU.AdminNO = DODD.AdminNO
                      )MSKU
          OUTER APPLY (SELECT *
                         FROM Fnc_M_Vendor_SelectLatest(DODH.OrderDate)MVEN  
                        WHERE MVEN.VendorCD = DODH.OrderCD
                      )MVEN
          OUTER APPLY (SELECT *
                         FROM Fnc_M_Souko_SelectLatest(DODH.OrderDate)MSOU
                        WHERE MSOU.SoukoCD = DODH.DestinationSoukoCD
                      )MSOU
          LEFT JOIN M_MultiPorpose MMPP_Nouhinsaki
          ON  MMPP_Nouhinsaki.ID  = 221
          AND MMPP_Nouhinsaki.[Key] = 1
          LEFT JOIN M_MultiPorpose MMPP_Tani
          ON  MMPP_Tani.ID  = 201
          AND MMPP_Tani.[Key] = DODD.TaniCD
          OUTER APPLY (SELECT *
                         FROM Fnc_M_Store_SelectLatest(DODH.OrderDate)MSTR
                        WHERE MSTR.StoreCD = DODH.StoreCD
                      )MSTR
          --印刷種類 1:通常発注、2:Net発注、3:直送発注、4:キャンセル発注 ※条件でも使用
          OUTER APPLY(SELECT CASE WHEN DODH.DestinationKBN = 2
                                       AND DODH.OrderWayKBN = 2
                                       AND DODH.ReturnFLG = 0
                                    THEN 1
                                  WHEN DODH.DestinationKBN = 2
                                       AND DODH.OrderWayKBN = 1
                                       AND DODH.ReturnFLG = 0
                                    THEN 2
                                  WHEN DODH.DestinationKBN = 1
                                   --    AND DODH.OrderWayKBN = 2
                                       AND DODH.ReturnFLG = 0
                                    THEN 3
                                  WHEN DODH.ReturnFLG = 1
                                    THEN 4
                                    ELSE NULL
                             END AS Value
                     )SUB_InsatuShurui

          WHERE (@p_HacchuuNO IS NULL OR (@p_HacchuuNO IS NOT NULL AND DODH.OrderNO = @p_HacchuuNO))
            AND DODH.DeleteDateTime IS NULL
            AND DODH.StoreCD = @p_StoreCD
            AND ((@p_InsatuShurui_Hacchhusho = 1 AND SUB_InsatuShurui.Value = 1)
                  OR
                 (@p_InsatuShurui_NetHacchuu = 1 AND SUB_InsatuShurui.Value = 2)
                  OR
                 (@p_InsatuShurui_Chokusou   = 1 AND SUB_InsatuShurui.Value = 3)
                  OR
                 (@p_InsatuShurui_Cancel     = 1 AND SUB_InsatuShurui.Value = 4)
                )
            AND (   (@p_InsatuTaishou_Mihakkou  = 1 AND DODH.LastPrintDate IS NULL)  
                 OR (@p_InsatuTaishou_Saihakkou = 1 AND DODH.LastPrintDate IS NOT NULL))  
            AND (@p_HacchuuDateFrom IS NULL OR DODH.OrderDate >= @p_HacchuuDateFrom)
            AND (@p_HacchuuDateTo IS NULL OR DODH.OrderDate <= @p_HacchuuDateTo)
            AND (@p_Staff IS NULL OR (SUB_InsatuShurui.Value = 4 OR DODH.StaffCD = @p_Staff))
            AND (@p_Vendor IS NULL OR DODH.OrderCD = @p_Vendor)
            AND ((@p_IsPrintMisshounin = 1 AND DODH.ApprovalStageFLG > 0)
                 OR 
                 (@p_IsPrintMisshounin = 0 AND DODH.ApprovalStageFLG >= 9)
                )
            AND (@p_IsPrintEDIHacchuu = 1
                 OR 
                 (@p_IsPrintEDIHacchuu = 0 AND DODD.EDIFLG = 0)
                )
          )MAIN
    ORDER BY MAIN.OrderNO
            ,MAIN.DetailGyouNO

END
GO


