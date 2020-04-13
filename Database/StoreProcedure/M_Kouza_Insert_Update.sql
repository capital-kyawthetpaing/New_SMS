 BEGIN TRY 
 Drop Procedure dbo.[M_Kouza_Insert_Update]
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
CREATE PROCEDURE [dbo].[M_Kouza_Insert_Update] 
	-- Add the parameters for the stored procedure here
	@KouzaCD as varchar(6),
	@ChangeDate date,
	@KouzaName varchar(50),
	@BankCD varchar(4),
	@BranchCD varchar(3),
	@KouzaKBN tinyint,
	@KouzaMeigi varchar(40),
	@KouzaNO varchar(7),
	@Print1 varchar(80),
	@Print2 varchar(80),
	@Print3 varchar(80),
	@Print4 varchar(80),
	@Fee11 money,
	@Tax11 money,
	@Amount1 money,
	@Fee12 money,
	@Tax12 money,

	@Fee21 money,
	@Tax21 money,
	@Amount2 money,
	@Fee22 money,
	@Tax22 money,

	@Fee31 money,
	@Tax31 money,
	@Amount3 money,
	@Fee32 money,
	@Tax32 money,

	@CompanyCD varchar(10),
	@CompanyName varchar(40),
	
	@Remarks varchar(500),
	@DeleteFlg tinyint,
	@Operator varchar(10),
	@Program as varchar(30),
	@PC as varchar(30),
	@OperateMode as varchar(10),
	@KeyItem as varchar(100),
	@Mode as tinyint-- 1 - insert, 2 - update
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @currentDate as datetime = getdate();

			if @Mode = 1--insert mode
				begin	
					--souko insert				
					insert into M_Kouza
					(
						KouzaCD,
						ChangeDate,
						KouzaName,
						BankCD,
						BranchCD,

						KouzaKBN,
						KouzaMeigi,
						KouzaNO,

						Print1,
						Print2,
						Print3,
						Print4,

						Fee11,
						Tax11,
						Amount1,
						Fee12,
						Tax12,
						Fee21,
						Tax21,
						Amount2,
						Fee22,
						Tax22,

						Fee31,
						Tax31,
						Amount3,
						Fee32,
						Tax32,

						CompanyCD,
						CompanyName,

						Remarks,
						DeleteFlg,
						UsedFlg,
						InsertOperator,
						InsertDateTime,
						UpdateOperator,
						UpdateDateTime
					)
					values
					(
						@KouzaCD,
						@ChangeDate,
						@KouzaName ,
						@BankCD,
						@BranchCD ,

						@KouzaKBN ,
						@KouzaMeigi ,
						@KouzaNO ,

						@Print1,
						@Print2,
						@Print3,
						@Print4,
						@Fee11 ,
						@Tax11 ,
						@Amount1 ,
						@Fee12 ,
						@Tax12 ,

						@Fee21 ,
						@Tax21 ,
						@Amount2 ,
						@Fee22 ,
						@Tax22 ,

						@Fee31 ,
						@Tax31 ,
						@Amount3 ,
						@Fee32 ,
						@Tax32 ,

						@CompanyCD,
						@CompanyName,
	
						@Remarks,
						@DeleteFlg ,
						0,
						@Operator,
						@currentDate ,
						@Operator ,
						@currentDate
					)
				end
			else if @Mode = 2--update mode
				begin
					update M_Kouza
					set
						KouzaCD=@KouzaCD,
						ChangeDate=@ChangeDate,
						KouzaName=@KouzaName,
						BankCD=@BankCD,
						BranchCD=@BranchCD,
						KouzaKBN=@KouzaKBN,
						KouzaMeigi=@KouzaMeigi,
						KouzaNO=@KouzaNO,
						Print1=@Print1,
						Print2=@Print2,
						Print3=@Print3,
						Print4=@Print4,
						Fee11=@Fee11,
						Tax11=@Tax11,
						Amount1=@Amount1,
						Fee12=@Fee12,
						Tax12=@Tax12,
						Fee21=@Fee21,
						Tax21=@Tax21,
						Amount2=@Amount2,
						Fee22=@Fee22,
						Tax22=@Tax22,

						Fee31=@Fee31,
						Tax31=@Tax31,
						Amount3=@Amount3,
						Fee32=@Fee32,
						Tax32=@Tax32,

						CompanyCD=@CompanyCD,
						CompanyName=@CompanyName,

						Remarks=@Remarks,
						DeleteFlg=@DeleteFlg,

						UpdateOperator=@Operator,
						UpdateDateTime=@currentDate
						
					where KouzaCD=@KouzaCD
						and ChangeDate=@ChangeDate

										
				end

					
			
			--log insert
			exec dbo.L_Log_Insert @Operator,@Program,@PC,@OperateMode,@KeyItem

		
END

