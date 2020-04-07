 BEGIN TRY 
 Drop Procedure dbo.[Amazon_Allow_Check]
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
CREATE PROCEDURE   [dbo].[Amazon_Allow_Check]
      
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	


--select * from M_MultiPorpose where ID='302' and [KEY] =1

if (select Count(*) from M_MultiPorpose where ID='302' and [KEY] ='1' ) = 0
Begin
 select '0' as Status

End
else if (select Count(*) from M_MultiPorpose where ID='302' and [KEY] ='1' ) = 1
Begin
if ( select Num1   from M_MultiPorpose where ID='302' and [KEY] ='1'  ) =1
begin
     select '1' as Status
End
else
begin
     select '0' as Status
End
End

if (select Count(*) from D_APIControl where APIKEY ='11' ) =0
Begin
select '0' as Status
End
else if (select Count(*) from D_APIControl where APIKEY ='11' ) = 1
Begin
if ( select [State] from D_APIControl where APIKEY ='11'  ) =1
begin
     select '1' as Status
End
else
begin
     select '0' as Status
End
End


END

