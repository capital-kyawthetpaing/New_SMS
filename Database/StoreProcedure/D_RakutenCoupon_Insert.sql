 BEGIN TRY 
 Drop Procedure dbo.[D_RakutenCoupon_Insert]
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
CREATE PROCEDURE [dbo].[D_RakutenCoupon_Insert](
	-- Add the parameters for the stored procedure here
	@CouponXml as xml,
	@Operator as varchar(10),
	@InsertDateTime as datetime)
AS
BEGIN
	CREATE TABLE [dbo].[#tempCouple]
	(
				InportSEQ int,
				StoreCD varchar(4) collate Japanese_CI_AS,
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50) collate Japanese_CI_AS,
				couponRows int,
				couponCode  varchar(28) collate Japanese_CI_AS,
				itemId int,
				couponName varchar(382) collate Japanese_CI_AS,
				couponSummary varchar(382) collate Japanese_CI_AS,
				couponCapital varchar(64) collate Japanese_CI_AS,
				expiryDate date,
				couponPrice money,
				couponUnit  int,
				couponTotalPrice money
	)
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @CouponXml
	insert into #tempCouple
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				InportSEQ int,
				StoreCD varchar(4),
				APIKey tinyint,
				InportSEQRows int,
				orderNumber varchar(50),
				couponRows int,
				couponCode  varchar(28),
				itemId int,
				couponName varchar(382),
				couponSummary varchar(382),
				couponCapital varchar(64),
				expiryDate date,
				couponPrice money,
				couponUnit  int,
				couponTotalPrice money
			)
			exec sp_xml_removedocument @DocHandle;

			INSERT INTO D_RakutenCoupon(InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,couponRows,couponCode,itemId,couponName,couponSummary,couponCapital,expiryDate,couponPrice,couponUnit,couponTotalPrice,
				InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime
)
			select InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,couponRows,couponCode,itemId,couponName,couponSummary,couponCapital,expiryDate,couponPrice,couponUnit,couponTotalPrice,
			@Operator,@InsertDateTime,@Operator,@InsertDateTime
			from #tempCouple


			drop table #tempCouple

	
END

