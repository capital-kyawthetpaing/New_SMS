using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
   public class MasterTouroku_TenzikaiShouhin_DL:Base_DL
    {

        public DataTable Mastertoroku_Tenzikaishouhin_Select(M_TenzikaiShouhin_Entity mt, int mode)
        {
            string sp = "Mastertoroku_Tenzikaishouhin_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@lastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mode.ToString() } },
            };
            return SelectData(dic, sp);
        }
        public DataTable M_TenzikaiShouhinName_Select(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "M_TenzikaiShouhinName_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                
            };
            return SelectData(dic, sp);
        }
        

        public DataTable M_Tenzikaishouhin_SelectForJancd(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "M_Tenzikaishouhin_SelectForJancd";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName } },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD } },
                { "@lastyear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm } },
                { "@lastseason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason } },
                { "@brandcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BranCDFrom } },
                { "@segment", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCDFrom } },
                { "@jancd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.JANCD } },
            };
            return SelectData(dic, sp);
        }

        public DataTable M_SKU_SelectForSKUCheck(M_SKU_Entity msku)
        {
            string sp = "M_SKU_SelectForSKUCheck";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                //{ "@skuname", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SKUName} },
                //{ "@colorName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ColorName } },
                //{ "@sizeName", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.SizeName } 
                { "@ExhibitionCommomCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.ExhibitionCommonCD } },
                //{ "@Jancd", new ValuePair { value1 = SqlDbType.VarChar, value2 = msku.JanCD } },
            };
            return SelectData(dic, sp);
        }
        

        public bool M_Tenzikaishouhin_DeleteUpdate(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "M_Tenzikaishouhin_DeleteUpdate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = mt.xml} },
                { "@year", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm} },
                { "@season", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason} },
                { "@startDate" , new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.ChangeDate} },
                { "@OperatorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.InsertOperator} },
             
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        public bool M_Tenzikaishouhin_InsertUpdate(M_TenzikaiShouhin_Entity mt,int type)
        {
            string sp = "M_Tenzikaishouhin_InsertUpdate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@xml", new ValuePair { value1 = SqlDbType.Xml, value2 = mt.xml} },
                { "@year", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm} },
                { "@season", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason} },
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName} },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD} },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.BrandCD} },
                { "@SegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.SegmentCD} },
                { "@OperatorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.InsertOperator} },
                { "@Pc", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.PC} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.ProgramID} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.ProcessMode} },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.Key } },
                { "@type", new ValuePair { value1 = SqlDbType.TinyInt, value2 = type.ToString() } }
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }

        public bool M_Tenzikaishouhin_Delete(M_TenzikaiShouhin_Entity mtzke)
        {
            string sp = "M_Tenzikaishouhin_Delete";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@year", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.LastYearTerm} },
                { "@season", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.LastSeason} },
                { "@tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.TenzikaiName} },
                { "@vendorcd", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.VendorCD} },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.BrandCD} },
                { "@SegmentCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.SegmentCD} },
                { "@OperatorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.InsertOperator} },
                { "@Pc", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.PC} },
                { "@Program", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.ProgramID} },
                { "@OperateMode", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.ProcessMode} },
                { "@KeyItem", new ValuePair { value1 = SqlDbType.VarChar, value2 = mtzke.Key } },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }


        public DataTable M_TenzikaiShouhin_Check(M_TenzikaiShouhin_Entity mt)
        {
            string sp = "M_TenzikaiShouhin_Check";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@LastYear", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastYearTerm} },
                { "@LastSeason", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.LastSeason} },
                { "@Tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.TenzikaiName} },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mt.VendorCD} },
              
            };
           // UseTransaction = true;
            return SelectData(dic, sp);
        }

        public DataTable M_TenzikaiShouhin_SelectForHachuu(M_TenzikaiShouhin_Entity mte)
        {
            string sp = "M_TenzikaiShouhin_SelectForHachuu";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Tenzikainame", new ValuePair { value1 = SqlDbType.VarChar, value2 = mte.TenzikaiName} },              
            };
            return SelectData(dic, sp);
        }
    }
}
