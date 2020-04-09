 BEGIN TRY 
 Drop Procedure dbo.[D_Mitsumori_SelectAll]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [D_Mitsumori_SelectAll]    */
CREATE PROCEDURE [dbo].[D_Mitsumori_SelectAll](
    -- Add the parameters for the stored procedure here
    @MitsumoriDateFrom  varchar(10),
    @MitsumoriDateTo  varchar(10),
    @MitsumoriInputDateFrom varchar(10),
    @MitsumoriInputDateTo varchar(10),
    @StoreCD  varchar(4),
    @StaffCD  varchar(10),
    @CustomerCD  varchar(13),
    @CustomerName  varchar(80),
    @MitsumoriName  varchar(100),
    @JuchuuChanceKBN varchar(3),
    @JuchuuFLG1 TinyInt,
    @JuchuuFLG2 TinyInt
)AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here

    SELECT DH.MitsumoriNO
          ,CONVERT(varchar,DH.MitsumoriDate,111) AS MitsumoriDate
          ,CONVERT(varchar,DH.InsertDateTime,111) AS InsertDateTime
          ,DH.CustomerCD
--          ,DH.CustomerName
          ,(SELECT top 1 (CASE A.VariousFLG WHEN 1 THEN DH.CustomerName ELSE A.CustomerName END)
          FROM M_Customer A 
          WHERE A.CustomerCD = DH.CustomerCD AND A.DeleteFlg = 0 AND A.ChangeDate <= DH.MitsumoriDate
          ORDER BY A.ChangeDate desc) AS CustomerName 
          ,DH.CustomerName2
          ,DH.MitsumoriName
          ,DH.StaffCD
          ,(SELECT top 1 A.StaffName 
          FROM M_Staff A 
          WHERE A.StaffCD = DH.StaffCD AND A.ChangeDate <= DH.MitsumoriDate
          ORDER BY A.ChangeDate desc) AS StaffName
          ,(SELECT top 1 A.StoreName 
          FROM M_Store A 
          WHERE A.StoreCD = DH.StoreCD AND A.ChangeDate <= DH.MitsumoriDate
          ORDER BY A.ChangeDate desc) AS StoreName
          ,DH.MitsumoriGaku 
          ,(SELECT A.Char1 FROM M_MultiPorpose A WHERE A.ID='216' AND A.[Key] = DH.JuchuuChanceKBN) AS JuchuuChanceKBN
          ,(CASE DH.JuchuuFLG WHEN 0 THEN '未受注' ELSE '受注済' END) AS JuchuuFLG
                  
    from D_Mitsumori DH
        WHERE DH.MitsumoriDate >= (CASE WHEN @MitsumoriDateFrom <> '' THEN CONVERT(DATE, @MitsumoriDateFrom) ELSE DH.MitsumoriDate END)
        AND DH.MitsumoriDate <= (CASE WHEN @MitsumoriDateTo <> '' THEN CONVERT(DATE, @MitsumoriDateTo) ELSE DH.MitsumoriDate END)
        AND DH.InsertDateTime >= (CASE WHEN @MitsumoriInputDateFrom <> '' THEN CONVERT(DATE, @MitsumoriInputDateFrom) ELSE DH.InsertDateTime END)
        AND DH.InsertDateTime <= (CASE WHEN @MitsumoriInputDateTo <> '' THEN CONVERT(DATE, @MitsumoriInputDateTo) ELSE DH.InsertDateTime END)
        AND DH.StoreCD = @StoreCD
        AND DH.StaffCD = (CASE WHEN @StaffCD <> '' THEN @StaffCD ELSE DH.StaffCD END)
        AND DH.CustomerCD = (CASE WHEN @CustomerCD <> '' THEN @CustomerCD ELSE DH.CustomerCD END)
        AND DH.MitsumoriName LIKE '%' + CASE WHEN @MitsumoriName <> '' THEN @MitsumoriName ELSE DH.MitsumoriName END + '%'
        AND DH.JuchuuChanceKBN = (CASE WHEN @JuchuuChanceKBN <> '' THEN @JuchuuChanceKBN ELSE DH.JuchuuChanceKBN END)
        AND DH.JuchuuFLG IN (@JuchuuFLG1, @JuchuuFLG2)
        
        AND DH.DeleteDateTime IS NULL

    ORDER BY DH.MitsumoriNO
    ;

  
END


