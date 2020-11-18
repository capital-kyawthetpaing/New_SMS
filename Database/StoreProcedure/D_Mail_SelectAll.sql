 BEGIN TRY 
 Drop Procedure dbo.[D_Mail_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [D_Mail_SelectAll]    */
CREATE PROCEDURE D_Mail_SelectAll(
    -- ADM the parameters for the stored procedure here
    @MailDateFrom  varchar(10),
    @MailDateTo  varchar(10),
    @MailTimeFrom  varchar(20),
    @MailTimeTo  varchar(20),
    @MailType tinyint,
    @MailKBN tinyint,
    @CustomerCD varchar(13),
    @VendorCD varchar(13)    
)AS
BEGIN
    -- SET NOCOUNT ON aDMed to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    -- Insert statements for procedure here    
    SELECT  DM.MailCounter
            ,DM.MailType
            ,DM.MailKBN
            ,DM.Number
            ,DM.MailNORows
            ,CONVERT(varchar,DM.SendedDateTime,111)+' '+SUBSTRING(CONVERT(varchar,DM.SendedDateTime,108),1,5) AS MailDateTime
            ,DM.StaffCD
            ,DM.ContactKBN
            ,DM.MailPatternCD
            ,DM.MailSubject
            ,DM.MailPriority
            ,DM.ReMailFlg
            ,DM.UnitKBN
            ,DM.SendedDateTime
            ,DM.SenderKBN
            ,DM.SenderCD
            ,DM.SenderAddress
            ,DM.MailContent
            ,(CASE WHEN DM.MailType IN (1,3) THEN DJ.CustomerCD + ' '
                    + (SELECT top 1 M.CustomerName FROM M_Customer AS M
                        WHERE M.CustomerCD = DJ.CustomerCD
                        AND M.DeleteFlg = 0
                        AND M.ChangeDate <= CONVERT(date,SYSDATETIME())
                        ORDER BY M.ChangeDate desc)
                WHEN DM.MailType IN (7) THEN DO.OrderCD + ' '
                    + (SELECT top 1 M.VendorName FROM M_Vendor AS M
                        WHERE M.VendorCD = DO.OrderCD
                        AND M.DeleteFlg = 0
                        AND M.ChangeDate <= CONVERT(date,SYSDATETIME())
                        ORDER BY M.ChangeDate desc)
                ELSE '' END) AS Customer

    FROM D_Mail AS DM
    LEFT OUTER JOIN D_Juchuu AS DJ
    ON DJ.JuchuuNo = DM.Number
    AND DJ.DeleteDateTime IS NULL
    LEFT OUTER JOIN D_Order AS DO
    ON DO.OrderNo = DM.Number
    AND DO.DeleteDateTime IS NULL    
    
    WHERE CONVERT(varchar,DM.SendedDateTime,111) >= @MailDateFrom
    AND CONVERT(varchar,DM.SendedDateTime,111) <= @MailDateTo
    AND DM.SendedDateTime >= (CASE WHEN ISNULL(@MailTimeFrom,'') <> '' THEN CONVERT(datetime,@MailTimeFrom) ELSE ISNULL(DM.SendedDateTime,'') END)
    AND DM.SendedDateTime <= (CASE WHEN ISNULL(@MailTimeTo,'') <> '' THEN CONVERT(datetime,@MailTimeTo) ELSE ISNULL(DM.SendedDateTime,'') END)
    AND DM.MailType = (CASE WHEN @MailType <>0 THEN @MailType ELSE DM.MailType END)
    AND DM.MailKBN = (CASE WHEN @MailKBN <>0 THEN @MailKBN ELSE DM.MailKBN END)
    AND ISNULL(DJ.CustomerCD,'') = (CASE WHEN ISNULL(@CustomerCD,'') <> '' THEN @CustomerCD ELSE ISNULL(DJ.CustomerCD,'') END)
    AND ISNULL(DO.OrderCD,'') = (CASE WHEN ISNULL(@VendorCD,'') <> '' THEN @VendorCD ELSE ISNULL(DO.OrderCD,'') END)
    ORDER BY DM.SendedDateTime
    ;

END

GO
