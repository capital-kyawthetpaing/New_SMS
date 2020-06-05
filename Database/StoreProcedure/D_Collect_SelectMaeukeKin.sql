
/****** Object:  StoredProcedure [dbo].[D_Collect_SelectData]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP PROCEDURE [D_Collect_SelectMaeukeKin]
GO

/****** Object:  StoredProcedure [D_Collect_SelectMaeukeKin]    */
CREATE PROCEDURE D_Collect_SelectMaeukeKin(
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(6),
    @CustomerCD  varchar(13),
    @AdvanceFLG  tinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT SUM(DH.ConfirmSource - DH.ConfirmAmount) AS MaeukeKin
    FROM [D_Collect] AS DH

    WHERE DH.StoreCD = @StoreCD
    AND DH.CollectCustomerCD = @CustomerCD
    AND DH.AdvanceFLG = @AdvanceFLG
    AND DH.ConfirmSource - DH.ConfirmAmount > 0
    AND DH.DeleteDateTime IS NULL
    ;

END

GO
