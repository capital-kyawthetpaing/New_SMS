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
    public class M_ItemPrice_DL : Base_DL
    {
        public DataTable M_ItemPrice_Select(M_ItemPrice_Entity mie)
        {
            string sp = "M_ItemPrice_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ITemCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ChangeDate } },
            };
            return SelectData(dic, sp);
        }

        /*
        public DataTable M_Store_SelectAll(M_Store_Entity mbe)
        {
            string sp = "M_Store_SelectAll";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            command.Parameters.Add("@DisplayKbn", SqlDbType.TinyInt).Value = mbe.DisplayKbn;
            command.Parameters.Add("@ChangeDate", SqlDbType.VarChar).Value = mbe.ChangeDate;
            command.Parameters.Add("@StoreCDFrom", SqlDbType.VarChar).Value = mbe.StoreCDFrom;
            command.Parameters.Add("@StoreCDTo", SqlDbType.VarChar).Value = mbe.StoreCDTo;
            command.Parameters.Add("@StoreName", SqlDbType.VarChar).Value = mbe.StoreName;
            command.Parameters.Add("@StoreKBN1", SqlDbType.TinyInt).Value = mbe.StoreKBN1;
            command.Parameters.Add("@StoreKBN2", SqlDbType.TinyInt).Value = mbe.StoreKBN2;
            command.Parameters.Add("@StoreKBN3", SqlDbType.TinyInt).Value = mbe.StoreKBN3;

            return SelectData(sp);
        }
        */
        /// <summary>
        /// ITEM販売単価マスタ更新処理
        /// MasterTouroku_HanbaiTankaより更新時に使用
        /// </summary>
        /// <param name="mie"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool M_ItemPrice_Exec(M_ItemPrice_Entity mie,DataTable dt, short operationMode, string operatorNm, string pc )
        {
            string sp = "PRC_MasterTouroku_HanbaiTanka";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            this.UseTransaction = true;

            AddParam(command,"@OperateMode", SqlDbType.TinyInt, operationMode.ToString());
            AddParam(command,"@GeneralRate", SqlDbType.Decimal, mie.GeneralRate);
            AddParam(command,"@MemberRate", SqlDbType.Decimal, mie.MemberRate);
            AddParam(command,"@ClientRate", SqlDbType.Decimal, mie.ClientRate);
            AddParam(command,"@SaleRate", SqlDbType.Decimal, mie.SaleRate);
            AddParam(command,"@WebRate", SqlDbType.Decimal, mie.WebRate);
            AddParamForDataTable(command,"@Table", SqlDbType.Structured, dt);

            AddParam(command,"@DeleteFlg", SqlDbType.TinyInt, mie.DeleteFlg);
            AddParam(command,"@UsedFlg", SqlDbType.TinyInt, mie.UsedFlg);
            AddParam(command,"@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command,"@PC", SqlDbType.VarChar, pc);

            string outPutParam = "";
            return InsertUpdateDeleteData(sp, ref outPutParam);
        }

        /// <summary>
        /// ITEM販売単価マスタ取得処理
        /// MasterTouroku_HanbaiTankaよりデータ抽出時に使用
        /// </summary>
        public DataTable M_ItemPrice_SelectData(M_ItemPrice_Entity mie, short operationMode)
        {
            string sp = "M_ItemPrice_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@ItemFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ItemFrom } },
                { "@ItemTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ItemTo } },
                { "@BrandCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.BrandCD } },
                { "@ITemName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mie.ITemName } },
            };
            
            return SelectData(dic, sp);

        }
    }

}
