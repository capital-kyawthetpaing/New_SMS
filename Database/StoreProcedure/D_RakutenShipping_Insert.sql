 BEGIN TRY 
 Drop Procedure dbo.[D_RakutenShipping_Insert]
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
CREATE PROCEDURE [dbo].[D_RakutenShipping_Insert](
	-- Add the parameters for the stored procedure here
	@ShippingXml as xml,
	@Operator as varchar(10),
	@InsertDateTime as datetime)
AS
BEGIN
CREATE TABLE [dbo].[#tempShipping]
(
				InportSEQ int,
				StoreCD	varchar(4) collate Japanese_CI_AS,
				APIKey tinyint,
				InportSEQRows	int,
				orderNumber		varchar(50) collate Japanese_CI_AS,
				basketRows		int,
				basketId		int,
				postagePrice	money,
				deliveryPrice	money,
				goodsTax		money,
				goodsPrice		money,
				totalPrice		money,
				noshi			varchar(382) collate Japanese_CI_AS,
				packageDeleteFlag	tinyint,
				zipCode1	varchar(3) collate Japanese_CI_AS,
				zipCode2	varchar(4) collate Japanese_CI_AS,
				prefecture	 varchar(50) collate Japanese_CI_AS,
				city		 varchar(50) collate Japanese_CI_AS,
				subAddress	varchar(100) collate Japanese_CI_AS,
				familyName		varchar(20) collate Japanese_CI_AS,
				firstName		varchar(20) collate Japanese_CI_AS,
				familyNameKana	varchar(20) collate Japanese_CI_AS,
				firstNameKana	varchar(20) collate Japanese_CI_AS,
				phoneNumber1	 varchar(5) collate Japanese_CI_AS,
				phoneNumber2	 varchar(5) collate Japanese_CI_AS,
				phoneNumber3	 varchar(5) collate Japanese_CI_AS,
				isolatedIslandFlag	tinyint,
				cvsCode				tinyint,
				cvsstoreGenreCode	varchar(10) collate Japanese_CI_AS,
				cvsstoreCode		varchar(10) collate Japanese_CI_AS,
				cvsstoreName		varchar(50) collate Japanese_CI_AS,
				cvsstoreZip			varchar(8) collate Japanese_CI_AS,
				cvsstorePrefecture	varchar(50) collate Japanese_CI_AS,
				cvsstoreAddress	varchar(200) collate Japanese_CI_AS,
				cvsareaCode		varchar(30) collate Japanese_CI_AS,
				cvsdepo			varchar(30) collate Japanese_CI_AS,
				cvsopenTime		varchar(7) collate Japanese_CI_AS,
				cvscloseTime	varchar(7) collate Japanese_CI_AS,
				cvsRemarks		varchar(300)	 collate Japanese_CI_AS	
)
	
	declare @DocHandle int

	exec sp_xml_preparedocument @DocHandle output, @ShippingXml
	insert into #tempShipping
	select *  FROM OPENXML (@DocHandle, '/NewDataSet/test',2)
			with
			(
				InportSEQ int,
				StoreCD	varchar(4),
				APIKey tinyint,
				InportSEQRows	int,
				orderNumber		varchar(50),
				basketRows		int,
				basketId		int,
				postagePrice	money,
				deliveryPrice	money,
				goodsTax		money,
				goodsPrice		money,
				totalPrice		money,
				noshi			varchar(382),
				packageDeleteFlag	tinyint,
				zipCode1	varchar(3),
				zipCode2	varchar(4),
				prefecture	 varchar(50),
				city		 varchar(50),
				subAddress	varchar(100),
				familyName		varchar(20),
				firstName		varchar(20),
				familyNameKana	varchar(20),
				firstNameKana	varchar(20),
				phoneNumber1	 varchar(5),
				phoneNumber2	 varchar(5),
				phoneNumber3	 varchar(5),
				isolatedIslandFlag	tinyint,
				cvsCode				tinyint,
				cvsstoreGenreCode	varchar(10),
				cvsstoreCode		varchar(10),
				cvsstoreName		varchar(50),
				cvsstoreZip			varchar(8),
				cvsstorePrefecture	varchar(50),
				cvsstoreAddress	varchar(200),
				cvsareaCode		varchar(30),
				cvsdepo			varchar(30),
				cvsopenTime		varchar(7),
				cvscloseTime	varchar(7),
				cvsRemarks		varchar(300)		

			)
			exec sp_xml_removedocument @DocHandle;

			INSERT INTO D_RakutenShipping(InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,basketId,postagePrice,deliveryPrice,goodsTax,goodsPrice,totalPrice,noshi,packageDeleteFlag,zipCode1,zipCode2,prefecture,city,subAddress,familyName,
			firstName,familyNameKana,firstNameKana,phoneNumber1,phoneNumber2,phoneNumber3,isolatedIslandFlag,cvsCode,cvsstoreGenreCode,cvsstoreCode,cvsstoreName,cvsstoreZip,cvsstorePrefecture,cvsstoreAddress,cvsareaCode,cvsdepo,cvsopenTime,cvscloseTime,
			cvsRemarks,InsertOperator,InsertDateTime,UpdateOperator,UpdateDateTime)
			select InportSEQ,StoreCD,APIKey,InportSEQRows,orderNumber,basketRows,basketId,postagePrice,deliveryPrice,goodsTax,goodsPrice,totalPrice,noshi,packageDeleteFlag,zipCode1,zipCode2,prefecture,city,subAddress,familyName,
			firstName,familyNameKana,firstNameKana,phoneNumber1,phoneNumber2,phoneNumber3,isolatedIslandFlag,cvsCode,cvsstoreGenreCode,cvsstoreCode,cvsstoreName,cvsstoreZip,cvsstorePrefecture,cvsstoreAddress,cvsareaCode,cvsdepo,cvsopenTime,cvscloseTime,
			cvsRemarks,@Operator,@InsertDateTime,@Operator,@InsertDateTime
			from #tempShipping

			drop table #tempShipping
	
END

