 BEGIN TRY 
 Drop Procedure dbo.[D_Juchu_SelectForNayose]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[D_Juchu_SelectForNayose]
	-- Add the parameters for the stored procedure here
	@SiteKBN as VarChar(1),
	@NayoseKekkaTourokuDate as VarChar(10)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    DECLARE @SYSDATETIME datetime;
    SET @SYSDATETIME = SYSDATETIME();
    
    --画面転送表01（ヘッダ部.名寄せ実施日＝空白）
    --画面転送表02（ヘッダ部.名寄せ実施日<>空白）
    SELECT MAIN.JuchuuNO
          ,MAIN.M_CustomerCD
          ,ROW_NUMBER() OVER(PARTITION BY MAIN.JuchuuNO ORDER BY MAIN.M_CustomerCD) ROWNUM
          ,(SELECT M.Char1 FROM M_Multiporpose AS M
             WHERE M.ID = 232 AND M.[KEY] = DH.SiteKBN) AS SiteName
          ,DH.CustomerCD AS CustomerCD
          ,DH.CustomerName
          ,ISNULL(DH.Tel11,'') + ISNULL(DH.Tel12,'') + ISNULL(DH.Tel13,'') AS TEL
          ,DH.MailAddress
          ,ISNULL(DH.ZipCD1,'') + ISNULL(DH.ZipCD2,'') AS ZIP
          ,DH.Address1
          ,DH.Address2
          
          ,FC.CustomerName AS M_CustomerName
          ,ISNULL(FC.Tel11,'') + ISNULL(FC.Tel12,'') + ISNULL(FC.Tel13,'') AS M_TEL
          ,FC.MailAddress AS M_MailAddress
          ,ISNULL(FC.ZipCD1,'') + ISNULL(FC.ZipCD2,'') AS M_ZIP
          ,FC.Address1 AS M_Address1
          ,FC.Address2 AS M_Address2
          ,FC.AttentionFLG	--債権要注意顧客
         
    FROM (
        SELECT W.JuchuuNO, W.M_CustomerCD

        FROM (
               SELECT DH.JuchuuNO
                     ,FC.CustomerCD AS M_CustomerCD
                 from D_Juchuu AS DH
                INNER JOIN F_Customer(GETDATE()) FC
                   ON FC.CustomerName = DH.CustomerName
                  AND ISNULL(FC.Tel11,'') + '-' + ISNULL(FC.Tel12,'') + '-' + ISNULL(FC.Tel13,'') = ISNULL(DH.Tel11,'') + '-' + ISNULL(DH.Tel12,'') + '-' + ISNULL(DH.Tel13,'')
                  AND FC.StoreKBN = 1
                  AND FC.DeleteFlg = 0
                                 
                WHERE ((@NayoseKekkaTourokuDate IS NULL AND DH.IdentificationFLG = 1) OR 
                      (DH.NayoseKekkaTourokuDate = @NayoseKekkaTourokuDate))
                  AND DH.SiteKBN = (CASE WHEN ISNULL(@SiteKBN,'') <> '' THEN convert(tinyint,@SiteKBN) ELSE DH.SiteKBN END)
                  AND DH.DeleteDateTime IS NULL

                UNION ALL
               SELECT DH.JuchuuNO
                     ,FC.CustomerCD AS M_CustomerCD
                 from D_Juchuu AS DH
                INNER JOIN F_Customer(GETDATE()) FC
                   ON FC.CustomerName = DH.CustomerName
                  AND dbo.Fnc_MailAdress(FC.MailAddress) = dbo.Fnc_MailAdress(DH.MailAddress)     --※Fnc_MailAdress　で半角に
                  AND FC.StoreKBN = 1
                  AND FC.DeleteFlg = 0
                                 
                WHERE ((@NayoseKekkaTourokuDate IS NULL AND DH.IdentificationFLG = 1) OR 
                      (DH.NayoseKekkaTourokuDate = @NayoseKekkaTourokuDate))
                  AND DH.SiteKBN = (CASE WHEN ISNULL(@SiteKBN,'') <> '' THEN convert(tinyint,@SiteKBN) ELSE DH.SiteKBN END)
                  AND DH.DeleteDateTime IS NULL

                UNION ALL
               SELECT DH.JuchuuNO
                     ,FC.CustomerCD AS M_CustomerCD
                 from D_Juchuu AS DH
                INNER JOIN F_Customer(GETDATE()) FC
                   ON FC.CustomerName = DH.CustomerName
                  AND dbo.Fnc_AdressSpacePullOut(dbo.Fnc_AdressHalfToFull(FC.Address1)) = dbo.Fnc_AdressSpacePullOut(dbo.Fnc_AdressHalfToFull(DH.Address1))   --Fnc_AdressHalfToFullとFnc_AdressSpacePullOutで
                  AND FC.StoreKBN = 1
                  AND FC.DeleteFlg = 0
                                 
                WHERE ((@NayoseKekkaTourokuDate IS NULL AND DH.IdentificationFLG = 1) OR 
                      (DH.NayoseKekkaTourokuDate = @NayoseKekkaTourokuDate))
                  AND DH.SiteKBN = (CASE WHEN ISNULL(@SiteKBN,'') <> '' THEN convert(tinyint,@SiteKBN) ELSE DH.SiteKBN END)
                  AND DH.DeleteDateTime IS NULL
        )W
        GROUP BY W.JuchuuNO, W.M_CustomerCD
    ) AS MAIN
    INNER JOIN D_Juchuu AS DH
       ON DH.JuchuuNO = MAIN.JuchuuNO
    INNER JOIN F_Customer(GETDATE()) FC
       ON FC.CustomerCD = MAIN.M_CustomerCD
    
    ORDER BY MAIN.JuchuuNO, MAIN.M_CustomerCD
    ;

END
