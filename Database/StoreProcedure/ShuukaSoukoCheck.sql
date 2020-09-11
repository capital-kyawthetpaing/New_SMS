-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL

use [CapitalSMS]
GO 
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Alter PROCEDURE ShuukaSoukoCheck 
	-- Add the parameters for the stored procedure here
	@ChangeDate as Date ,
	@SoukoCD as varchar(13)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		select 1 from M_StoreAuthorizations msa 
							inner join	F_Souko(@ChangeDate )  ms on msa.StoreCD = ms.StoreCD
							where
							 ms.SoukoCD= @SoukoCD
							and 
							ms.ChangeDate <= @ChangeDate
END
GO

