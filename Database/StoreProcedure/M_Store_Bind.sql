  
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
/* J.Okada 2019/07/29 ADD  ���� */
 @Type as tinyint  
/* J.Okada 2019/07/29 ADD  ���� */
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 select ms.StoreCD,ms.StoreName from M_Store ms  
 inner join F_Store(cast(@ChangeDate as varchar(10)),@DeleteFlg) fs on ms.StoreCD = fs.StoreCD  
  and ms.ChangeDate = fs.ChangeDate  
 where (@StoreCD is null or (fs.StoreCD = @StoreCD ))  
END  
  