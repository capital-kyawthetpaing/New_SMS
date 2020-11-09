/****** Object:  StoredProcedure [dbo].[D_MailAddress_SelectAll]    Script Date: 6/11/2019 2:17:53 PM ******/
DROP PROCEDURE [dbo].[D_MailAddress_SelectAll]
GO

/****** Object:  StoredProcedure [D_MailAddress_SelectAll]    */
CREATE PROCEDURE D_MailAddress_SelectAll(
    -- ADM the parameters for the stored procedure here
    @MailCounter int
)AS
BEGIN
    -- SET NOCOUNT ON aDMed to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
        -- Insert statements for procedure here    
        SELECT  DM.MailCounter
              ,DM.AddressRows
              ,DM.AddressKBN
              ,DM.Address

        FROM D_MailAddress AS DM  
        
        WHERE DM.MailCounter = @MailCounter

        ORDER BY DM.AddressRows, DM.AddressKBN
        ;

END

GO
