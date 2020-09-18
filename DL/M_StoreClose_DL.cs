using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_StoreClose_DL : Base_DL
    {
        public DataTable M_StoreClose_Select(M_StoreClose_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@FiscalYYYYMM",new ValuePair {value1=SqlDbType.Int,value2=mse.FiscalYYYYMM} }

            };

            return SelectData(dic, "M_StoreClose_Select");
        }

        public DataTable M_StoreClose_Check(M_StoreClose_Entity mse, string Mode)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@Mode",new ValuePair {value1=SqlDbType.TinyInt,value2 = Mode} },
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@YYYYMM",new ValuePair {value1=SqlDbType.Int,value2=mse.FiscalYYYYMM.Replace("-","")} }
            };

            return SelectData(dic, "M_StoreClose_Check");
        }

        /// <summary>
        /// 月次締処理よりデータ取得時
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_StoreClose_SelectAll(M_StoreClose_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>()
            {
                 { "@StoreCD",new ValuePair {value1=SqlDbType.VarChar,value2=mse.StoreCD} },
                 { "@FiscalYYYYMM",new ValuePair {value1=SqlDbType.Int,value2=mse.FiscalYYYYMM} }
            };
            return SelectData(dic, "M_StoreClose_SelectAll");
        }
        /// <summary>
        /// 月次締処理よりデータ追加
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public bool M_StoreClose_Insert(M_StoreClose_Entity me)
        {
            string sp = "M_StoreClose_Insert";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.StoreCD } },
                { "@FiscalYYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 = me.FiscalYYYYMM} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Operator} },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.Mode.ToString()} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
        /// <summary>
        /// 月次締処理よりデータ更新
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public bool M_StoreClose_Update(M_StoreClose_Entity me)
        {
            string sp = "M_StoreClose_Update";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.StoreCD } },
                { "@FiscalYYYYMM", new ValuePair { value1 = SqlDbType.Int, value2 = me.FiscalYYYYMM} },
                { "@Mode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.Mode.ToString()} },
                { "@KBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.Kbn.ToString()} },
                { "@ClosePosition1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ClosePosition1} },
                { "@ClosePosition2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ClosePosition2} },
                { "@ClosePosition3", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ClosePosition3} },
                { "@ClosePosition4", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ClosePosition4} },
                { "@ClosePosition5", new ValuePair { value1 = SqlDbType.TinyInt, value2 = me.ClosePosition5} },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.Operator} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.PC} },
                { "@OperateModeNm", new ValuePair { value1 = SqlDbType.VarChar, value2 = me.OperateModeNm} },
            };
            UseTransaction = true;
            return InsertUpdateDeleteData(dic, sp);
        }
    }
}
