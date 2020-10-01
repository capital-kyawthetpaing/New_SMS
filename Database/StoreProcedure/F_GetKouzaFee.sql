/****** Object:  StoredProcedure [dbo].[[F_GetKouzaFee]]    Script Date: 6/11/2019 2:21:19 PM ******/
DROP FUNCTION [dbo].[F_GetKouzaFee]
GO

/****** Object:  UserDefinedFunction [dbo].[F_GetKouzaFee]    Script Date: 2020/09/28 19:30:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author,,Name>
-- Create date: <Create Date, ,>
-- Description: <Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[F_GetKouzaFee]
(
    -- Add the parameters for the function here
    @BankCD       varchar(6),
    @BranchCD     varchar(6),
    @PaymentTotal money,
    @KouzaCD      varchar(6),
    @ChangeDate   date
)
RETURNS money
AS
BEGIN
    -- Declare the return variable here
    DECLARE 
    @Fee money='0'

    SET @ChangeDate = ISNULL(@ChangeDate, GETDATE());

    -- Add the T-SQL statements to compute the return value here
    select @Fee=
    (case when fz.BankCD=@BankCD and fz.BranchCD=@BranchCD then
                        case when @PaymentTotal<=fz.Amount1 then fz.Fee11
                             when @PaymentTotal>fz.Amount1  then fz.Fee12
                             end
                     
          when fz.BankCD=@BankCD and fz.BranchCD<>@BranchCD then 
                        case when @PaymentTotal<=fz.Amount2 then fz.Fee21
                             when @PaymentTotal>fz.Amount2  then fz.Fee22
                             end

          when fz.BankCD<>@BankCD then 
                        case when @PaymentTotal<=fz.Amount3 then fz.Fee31
                             when @PaymentTotal>fz.Amount3  then fz.Fee32
                             end

        end )
        from F_Kouza(@ChangeDate) fz
        where fz.KouzaCD=@KouzaCD

    -- Return the result of the function
    RETURN @Fee

END


GO


