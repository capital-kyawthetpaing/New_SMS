using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data;

namespace DL
{
   public class D_Stock_DL:Base_DL
    {
        public DataTable D_Stock_DataSelect(TempoRegiZaikoKakunin_Entity kne)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@JanCD",new ValuePair { value1=SqlDbType.VarChar,value2=kne.JanCD} } ,
                   {"@DataCheck",new ValuePair { value1=SqlDbType.TinyInt,value2=kne.dataCheck} } ,
                   { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = kne.Operator } },
               };
            return SelectData(dic, "D_Stock_DataSelect");
        }

        public DataTable SelectDataForPrint(D_Stock_Entity dse,string option)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=dse.SoukoCD} } ,
                   {"@SKUCD",new ValuePair { value1=SqlDbType.VarChar,value2=dse.SKUCD} } ,
                   {"@ArrivalStartDate",new ValuePair { value1=SqlDbType.VarChar,value2=dse.ArrivalStartDate.Replace('/','-')} } ,
                   {"@ArrivalEndDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.ArrivalEndDate.Replace('/','-')} },
                   {"@RegisterFlg",new ValuePair { value1=SqlDbType.VarChar,value2=dse.RegisterFlg} } ,
                   {"@LocationFlg", new ValuePair { value1 = SqlDbType.VarChar, value2 = dse.LocationFlg} },
                   {"@Option", new ValuePair { value1 = SqlDbType.VarChar, value2 = option} },
               };
            return SelectData(dic, "TanaireList_DataSelect");
        }
        public DataTable D_Stock_SelectAllForShiireH(D_Stock_Entity de)
        {
            string sp = "D_Stock_SelectAllForShiireH";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ExpectReturnDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ExpectReturnDateFrom } },
                { "@ExpectReturnDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.ExpectReturnDateTo } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@F10", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.F10.ToString() } }
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Stock_Select(D_Stock_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=de.SoukoCD} },
                   {"@RackNO",new ValuePair { value1=SqlDbType.VarChar,value2=de.RackNO} },
                   {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=de.AdminNO} }
               };
            return SelectData(dic, "D_Stock_Select");
        }
        public DataTable D_Stock_SelectZaiko(D_Stock_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=de.SoukoCD} },
                   {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=de.AdminNO} }
               };
            return SelectData(dic, "D_Stock_SelectZaiko");
        }
        public DataTable D_Stock_SelectSuryo(D_Stock_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=de.SoukoCD} },
                   {"@RackNO",new ValuePair { value1=SqlDbType.VarChar,value2=de.RackNO} },
                   {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=de.AdminNO} },
                   {"@Suryo",new ValuePair { value1=SqlDbType.Int,value2=de.StockSu} }
               };
            return SelectData(dic, "D_Stock_SelectSuryo");
        }

        public DataTable D_Stock_SelectRackNO(D_Stock_Entity de)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=de.SoukoCD} },
                   {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=de.AdminNO} },
               };
            return SelectData(dic, "D_Stock_SelectRackNO");
        }

        public DataTable D_StockSelectForTairyuzaikohyo(D_Stock_Entity dse,M_SKU_Entity skue,M_SKUInfo_Entity info,M_SKUTag_Entity tage)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   {"@SoukoCD",new ValuePair { value1=SqlDbType.VarChar,value2=dse.SoukoCD} },
                   {"@RackNOFrom",new ValuePair { value1=SqlDbType.VarChar,value2=dse.RackNOFrom} },
                   {"@RackNOTo",new ValuePair { value1=SqlDbType.VarChar,value2=dse.RackNOTo} },
                   {"@AdminNO",new ValuePair { value1=SqlDbType.Int,value2=dse.AdminNO} },
                   {"@keyword1",new ValuePair { value1=SqlDbType.VarChar,value2=dse.Keyword1} },
                   {"@keyword2",new ValuePair { value1=SqlDbType.VarChar,value2=dse.Keyword2} },
                   {"@keyword3",new ValuePair { value1=SqlDbType.Int,value2=dse.Keyword3} },
                   {"@MainVendorCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.MainVendorCD} },
                   {"@BrandCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.BrandCD} },
                   {"@SKUName",new ValuePair { value1=SqlDbType.VarChar,value2=skue.SKUName} },
                   {"@JanCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.JanCD} },
                   {"@SKUCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.SKUCD} },
                   {"@ItemCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.ITemCD} },
                   {"@MakerItem",new ValuePair { value1=SqlDbType.VarChar,value2=skue.MakerItem} },
                   {"@SportsCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.SportsCD} },
                   {"@ReserveCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.ReserveCD} },
                   {"@NoticesCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.NoticesCD} },
                   {"@PostageCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.PostageCD} },
                   {"@OrderAttentionCD",new ValuePair { value1=SqlDbType.VarChar,value2=skue.OrderAttentionCD} },
                   {"@YearTerm",new ValuePair { value1=SqlDbType.VarChar,value2=info.YearTerm} },
                   {"@Season",new ValuePair { value1=SqlDbType.VarChar,value2=info.Season} },
                   {"@CatalogNO",new ValuePair { value1=SqlDbType.VarChar,value2=info.CatalogNO} },
                   {"@InsturctionsNO",new ValuePair { value1=SqlDbType.VarChar,value2=info.InstructionsNO} },
                   {"@TagName1",new ValuePair { value1=SqlDbType.VarChar,value2=tage.TagName1} },
                   {"@TagName2",new ValuePair { value1=SqlDbType.VarChar,value2=tage.TagName2} },
                   {"@TagName3",new ValuePair { value1=SqlDbType.VarChar,value2=tage.TagName3} },
                   {"@TagName4",new ValuePair { value1=SqlDbType.VarChar,value2=tage.TagName4} },
                   {"@TagName5",new ValuePair { value1=SqlDbType.Int,value2=tage.TagName5} },
                   {"@Type",new ValuePair { value1=SqlDbType.TinyInt,value2=dse.type} },
                   {"@Rdotype",new ValuePair { value1=SqlDbType.TinyInt,value2=dse.Rdotype} },
               };
            return SelectData(dic, "D_StockSelectForTairyuzaikohyo");
        }
    }
}
