
 BEGIN TRY 
 Drop Procedure dbo.D_Edi_SelectAll
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Edi_SelectAll]    */
CREATE PROCEDURE D_Edi_SelectAll(
    -- Add the parameters for the stored procedure here
    @StoreCD  varchar(4)
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT DE.EDIImportNO
          ,CONVERT(varchar,DE.ImportDateTime,111)+' '+SUBSTRING(CONVERT(varchar,DE.ImportDateTime,108),1,5) AS ImportDateTime
          ,DE.StaffCD
          ,DE.VendorCD
          ,(SELECT top 1 M.VendorName FROM M_Vendor AS M 
            WHERE M.VendorCD = DE.VendorCD
            AND M.ChangeDate <= CONVERT(date,DE.ImportDateTime)
            AND M.DeleteFlg = 0 
            ORDER BY M.ChangeDate desc) AS VendorName
          ,DE.ImportFile
          ,DE.OrderDetailsSu
          ,DE.ImportDetailsSu
          ,DE.ErrorSu
          ,DE.InsertOperator
          ,DE.InsertDateTime
          ,DE.UpdateOperator
          ,DE.UpdateDateTime
          ,DE.DeleteOperator
          ,DE.DeleteDateTime
       
    FROM D_Edi AS DE
    
    ORDER BY DE.EDIImportNO desc
    ;

END

GO
