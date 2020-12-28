using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

using System.Data.SqlClient;

namespace DL
{
    public class Function_DL : Base_DL
    {
        /// <summary>
        /// 消費税計算処理
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="kingaku"></param>
        /// <param name="ymd"></param>
        /// <param name="taxRateFLG"></param>
        /// <returns></returns>
        public DataTable Fnc_TAXCalculation(int mode,decimal kingaku, string ymd, int taxRateFLG)
        {
            string sp = "Fnc_TAXCalculation";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 =mode.ToString() } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = ymd } },
                { "@TaxRateFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = taxRateFLG.ToString() } },
                { "@Kingaku", new ValuePair { value1 = SqlDbType.Money, value2 = kingaku.ToString() } }
            };
            
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 単価取得処理
        /// </summary>
        /// <param name="fue"></param>
        /// <returns></returns>
        public DataTable Fnc_UnitPrice(Fnc_UnitPrice_Entity fue)
        {
            string sp = "Fnc_UnitPrice";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AdminNo", new ValuePair { value1 = SqlDbType.Int, value2 = fue.AdminNo } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = fue.ChangeDate } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fue.CustomerCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fue.StoreCD } },
                { "@SaleKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = fue.SaleKbn } },
                { "@Suryo", new ValuePair { value1 = SqlDbType.Int, value2 = fue.Suryo } }
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 商品引当処理
        /// </summary>
        /// <param name="fre"></param>
        /// <returns></returns>
        public DataTable Fnc_Reserve(Fnc_Reserve_Entity fre)
        {
            string sp = "Fnc_Reserve";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = fre.AdminNO } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.ChangeDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.StoreCD } },
                { "@SoukoCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.SoukoCD } },
                { "@Suryo", new ValuePair { value1 = SqlDbType.Int, value2 = fre.Suryo } },
                { "@DenType", new ValuePair { value1 = SqlDbType.TinyInt, value2 = fre.DenType } },
                { "@DenNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.DenNo } },
                { "@DenGyoNo", new ValuePair { value1 = SqlDbType.Int, value2 = fre.DenGyoNo } },
                { "@KariHikiateNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.KariHikiateNo } }
            };

            return SelectData(dic, sp);
        }

        public DataTable Fnc_PlanDate(Fnc_PlanDate_Entity fpe)
        {
            string sp = "Fnc_PlanDate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@KaisyuShiharaiKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = fpe.KaisyuShiharaiKbn } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fpe.CustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = fpe.ChangeDate } },
                { "@TyohaKbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = fpe.TyohaKbn } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// プレゼント判断Function
        /// </summary>
        /// <param name="fre"></param>
        /// <returns></returns>
        public DataTable Fnc_Present(Fnc_Present_Entity fre)
        {
            string sp = "Fnc_Present";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@AdminNO", new ValuePair { value1 = SqlDbType.Int, value2 = fre.AdminNO } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.ChangeDate } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fre.StoreCD } }
            };

            return SelectData(dic, sp);
        }
        public DataTable Fnc_Credit(Fnc_Credit_Entity fce)
        {
            string sp = "Fnc_Credit";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 =fce.Operator } },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = fce.PC }},
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = fce.ChangeDate } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = fce.CustomerCD } },
            };

            return SelectData(dic, sp);
        }
        public DataTable Fnc_SetCheckdigit(string janCount)
        {
            string sp = "Select_SetCheckdigit";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@inJAN12", new ValuePair { value1 = SqlDbType.VarChar, value2 =janCount } },
            };

            return SelectData(dic, sp);
        }
    }
}
