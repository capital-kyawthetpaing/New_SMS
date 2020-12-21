 BEGIN TRY 
 Drop Procedure dbo.[D_FBControl_Insert]
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
CREATE PROCEDURE [dbo].[D_FBControl_Insert] 
	-- Add the parameters for the stored procedure here
	@PayDate as date,
	@ActualPayDate as date,
	@MotoKouzaCD as varchar(3),
	@StoreCD as varchar(6),
	@Operator as varchar(10),

	@PayeeCD varchar(13),
	@PayeeName varchar(80),
	@BankCD varchar(4),
	@BranchCD varchar(3),
	@KouzaKBN tinyint,
	@KouzaNO varchar(7),
	@KouzaMeigi varchar(40),
	@PayGaku money,
	@TransferGaku money,
	@TransferFee money,							
	@TransferFeeKBN tinyint,

	@Flg as tinyint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	declare @FBPayNO as varchar(11),
	@DateTime datetime=getdate()
	
		EXEC Fnc_GetNumber
            25,-------------in伝票種別			
            @ActualPayDate,----in基準日		
            @StoreCD,
            @Operator,
            @FBPayNO OUTPUT
            ;

	 IF ISNULL(@FBPayNO,'') = ''
            BEGIN
                RETURN '1';
            END
			
	Insert    ---D_FBControlInsert
	Into D_FBControl
	(FBPayNO,
	PayDate,
	ProcessKBN,
	ActualPayDate,
	MotoKouzaCD,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime,
	DeleteOperator,
	DeleteDateTime)				
	Values(@FBPayNO,
	@PayDate ,
	3 ,
	@ActualPayDate,
	@MotoKouzaCD,
	@Operator,
	@DateTime,
	@Operator,
	@DateTime,
	NULL,
	NULL)

	Insert ---D_FBDataInsert
	Into D_FBData
	(FBPayNO,
	FBPayRows,
	PayeeCD,
	PayeeName,
	BankCD,
	BranchCD,
	KouzaKBN,
	KouzaNO,
	KouzaMeigi,
	PayGaku,
	TransferGaku,
	TransferFee,
	TransferFeeKBN,
	InsertOperator,
	InsertDateTime,
	UpdateOperator,
	UpdateDateTime,
	DeleteOperator,
	DeleteDateTime)
	Values
	(@FBPayNO,	
	--ROW_NUMBER() OVER (ORDER BY @FBPayNO),
	'',
	@PayeeCD,
	@PayeeName,
	@BankCD,
	@BranchCD,
	@KouzaKBN,
	@KouzaNO,
	@KouzaMeigi,
	@PayGaku,
	@TransferGaku,
	@TransferFee,							
	@TransferFeeKBN,
	@Operator,
	@DateTime,
	@Operator,
    @DateTime,
	NULL,
	NULL)

	Update D_Pay  ---D_PayUpdate
	Set 
	FBCreateDate = @DateTime,				
	FBCreateNO	= @FBPayNO,				
	UpdateOperator =@Operator,				
	UpdateDateTime = @DateTime	
	where DeleteDateTime is null					
	and MotoKouzaCD	= @MotoKouzaCD					
	and PayDate	=	@PayDate			
	and TransferGaku > 0					
	and ((@Flg = 0 and FBCreateDate is null) 
	or ((@Flg = 1  or @Flg = 2 )and FBCreateDate is not null))		

END