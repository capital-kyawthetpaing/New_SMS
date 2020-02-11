using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DL
{
    public class M_MovePurpose_DL : Base_DL
    {
        /// <summary>
        /// 移動区分コンボボックスBind処理用
        /// </summary>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable M_MovePurpose_Bind(M_MovePurpose_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MoveFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.MoveFLG } },
                { "@MoveRequestFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.MoveRequestFLG } }
            };
            return SelectData(dic, "M_MovePurpose_Bind");
        }

        public DataTable M_MovePurpose_Select(M_MovePurpose_Entity mse)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MovePurposeKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.MovePurposeKBN } },
                { "@MoveFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.MoveFLG } },
                { "@MoveRequestFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = mse.MoveRequestFLG } }
            };
            return SelectData(dic, "M_MovePurpose_Select");
        }
        }
}
