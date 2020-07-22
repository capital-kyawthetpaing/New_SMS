

/****** Object:  StoredProcedure [dbo].[M_Store_Bind]    Script Date: 2020/07/22 10:32:10 ******/
DROP PROCEDURE [dbo].[M_Store_Bind]
GO

/****** Object:  StoredProcedure [dbo].[M_Store_Bind]    Script Date: 2020/07/22 10:32:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[M_Store_Bind]  
 -- Add the parameters for the stored procedure here  
 @StoreCD as varchar(4),  
 @ChangeDate as date,  
 @DeleteFlg as tinyint , 
/* J.Okada 2019/07/29 ADD  Å´Å´ */
 @Type as tinyint  
/* J.Okada 2019/07/29 ADD  Å™Å™ */
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select ms.StoreCD,ms.StoreName from M_Store ms  
 inner join F_Store(cast(@ChangeDate as varchar(10))) fs on ms.StoreCD = fs.StoreCD  
  and ms.ChangeDate = fs.ChangeDate  
 where (@StoreCD is null or (fs.StoreCD = @StoreCD ))  
END  
  
GO


