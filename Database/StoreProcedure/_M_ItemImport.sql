 BEGIN TRY 
 Drop Procedure dbo.[_M_ItemImport]
END try
BEGIN CATCH END CATCH 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[_M_ItemImport]
	-- Add the parameters for the stored procedure here

	@xml as xml 
	,@Opt as varchar(20)
	,@PG as varchar(100)
	,@PC  as varchar(20)
	,@Mode as varchar(20)
	,@KeyItem as varchar(200)
	,@MainFlg as tinyint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @Date as Datetime =SysDateTime() ;
    
	     if @MainFlg = 1			--- all
	 Begin 
			 exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_SKU
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_SKUTag
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_ItemPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			  exec dbo._Item_JanOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_JanOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			  exec dbo._Item_Site
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_SKUInfo
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			  exec dbo._Item_SKUPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;

	 End
	 else if @MainFlg = 2			--- basic
	 Begin
	 	 exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_SKU
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKUTag
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 exec dbo._Item_ItemPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			  exec dbo._Item_JanOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_JanOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 -- exec dbo._Item_Site
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_SKUInfo
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			  exec dbo._Item_SKUPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
	 End
	 else if @MainFlg = 3			---attribute
	 Begin
	  exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKU
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 exec dbo._Item_SKUTag
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_ItemPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 -- exec dbo._Item_JanOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_JanOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_Site
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_SKUInfo
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			  exec dbo._Item_SKUPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
	 End 
	 else if @MainFlg = 4			---Price
	 Begin
	  exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKU
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 exec dbo._Item_SKUTag
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 exec dbo._Item_ItemPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			  exec dbo._Item_JanOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_JanOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			   exec dbo._Item_ItemOrderPrice2
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 -- exec dbo._Item_Site
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_SKUInfo
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			  exec dbo._Item_SKUPrice
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
	 End 
	 else if @MainFlg = 5			---cataroku
	 Begin
	  exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKU
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_SKUTag
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_ItemPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_JanOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_JanOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_Site
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			   exec dbo._Item_SKUInfo
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 -- exec dbo._Item_SKUPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
	 End 
	 else if @MainFlg = 6			--tagged
	 Begin
	  exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKU
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_SKUTag
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_ItemPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_JanOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_JanOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_Site
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_SKUInfo
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_SKUPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
	 End 
	 else if @MainFlg = 8
	 Begin
	  exec dbo._Item_ITem
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --exec dbo._Item_SKU
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_SKUTag
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --exec dbo._Item_ItemPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_JanOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_JanOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 --  exec dbo._Item_ItemOrderPrice2
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			  exec dbo._Item_Site
			 @xml,
			 @Opt,
			 @Date,
			 @MainFlg;
			 --  exec dbo._Item_SKUInfo
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
			 -- exec dbo._Item_SKUPrice
			 --@xml,
			 --@Opt,
			 --@Date,
			 --@MainFlg;
	 End		---site

--------------------------------------------------------------------------------
	EXEC L_Log_Insert
					 @opt  
					,@PG        
					,@PC             
					,@Mode    
					,@KeyItem


END
