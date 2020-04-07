 BEGIN TRY 
 Drop Procedure dbo.[Function_GetName_ReturnValue]
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
CREATE  PROCEDURE [dbo].[Function_GetName_ReturnValue]
	-- Add the parameters for the stored procedure here
	@Type as int,
	@SlipType as int,
	@ReferenceDate as date,
	@StoreCD as  varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @StoreKBN int,@data int;
	IF @Type=1
		BEGIN
			RETURN NULL;
		END
	ELSE
		BEGIN 
			IF  (SELECT 1 FROM  M_Calendar WHERE  CalendarDate=@ReferenceDate) is null
			 BEGIN
				RETURN NULL;
			 END
			ELSE
				BEGIN
					SELECT @StoreKBN = StoreKBN from M_Store where StoreCD = @StoreCD
				IF @StoreKBN is null
						BEGIN
							RETURN null;
						END 
				ELSE IF @StoreKBN = 2
						BEGIN
							return (select StoreCD from M_Store where StoreKBN=3 and DeleteFlg=0 and ChangeDate<=@ReferenceDate);
						END			
				ELSE 
					BEGIN
						
						SELECT @data = (CAST(mpn.Prefix as int) + CAST(mpn.Prefix2 as int) + (CAST(mpn.SeqNumber as int)+1))
						FROM M_PrefixNumber mpn
						INNER JOIN M_Prefix mp on mpn.Prefix=mp.Prefix
						INNER JOIN M_Control mc on(CASE mc.SeqUnit WHEN 1 THEN '0000' 
																   WHEN 2 THEN DATEPART(YYYY,@ReferenceDate)
																   ELSE  CONVERT(char(4),@ReferenceDate, 12) END ) = mpn.Prefix2
						WHERE  mp.SeqKBN=@SlipType
						AND mp.StoreCD=@StoreCD
						AND mc.MainKey=1
						
					UPDATE M
						SET  M.SeqNumber=(M.SeqNumber+1)
						FROM M_PrefixNumber AS M
						INNER JOIN M_Prefix AS MP 
						ON MP.Prefix=MP.Prefix
						INNER JOIN M_Control AS MC
						ON (CASE MC.SeqUnit WHEN 1 THEN '0000' WHEN 2 THEN DATEPART(YYYY,@ReferenceDate) ELSE CONVERT(CHAR(4),@ReferenceDate,12) END)=M.Prefix2
						
							RETURN @data;
						END
					END
		END

END
