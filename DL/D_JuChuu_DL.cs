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
    public class D_Juchuu_DL : Base_DL
    {
        public DataTable D_Juchu_SelectAll(D_Juchuu_Entity dme)
        {
            string sp = "D_Juchuu_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.JuchuDateTo } },
                //{ "@MitsumoriInputDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateFrom } },
                //{ "@MitsumoriInputDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriInputDateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.CustomerName} },
                //{ "@MitsumoriName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.MitsumoriName } },
                //{ "@JuchuuChanceKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = dme.JuchuuChanceKBN } },
                //{ "@JuchuuFLG1", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dme.JuchuuFLG1 } },
                //{ "@JuchuuFLG2", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dme.JuchuuFLG2 } },
            };


            if (!string.IsNullOrWhiteSpace(dme.CustomerName))
            {
                DataTable dt = SelectData(dic, sp);
                DataRow[] drs = dt.Select(" CustomerName LIKE '%" + dme.CustomerName + "%'");
                DataTable newdt = dt.Clone();

                foreach (var dr in drs)
                {
                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow = newdt.NewRow();
                    newrow.ItemArray = dr.ItemArray;
                    newdt.Rows.Add(newrow);
                }
                return newdt;
            }
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 受注入力更新処理
        /// TempoJuchuuNyuuryokuより更新時に使用
        /// </summary>
        /// <param name="dme"></param>
        /// <param name="operationMode"></param>
        /// <param name="operatorNm"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public bool D_Juchu_Exec(D_Juchuu_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            string sp = "PRC_TempoJuchuuNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int, operationMode.ToString());
            AddParam(command, "@JuchuuNO", SqlDbType.VarChar, dme.JuchuuNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command, "@JuchuuDate", SqlDbType.VarChar, dme.JuchuuDate);
            AddParam(command, "@ReturnFLG", SqlDbType.TinyInt, dme.ReturnFLG);
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, dme.SoukoCD);
            AddParam(command, "@StaffCD", SqlDbType.VarChar, dme.StaffCD);
            AddParam(command, "@CustomerCD", SqlDbType.VarChar, dme.CustomerCD);
            AddParam(command, "@CustomerName", SqlDbType.VarChar, dme.CustomerName);
            AddParam(command, "@CustomerName2", SqlDbType.VarChar, dme.CustomerName2);
            AddParam(command, "@AliasKBN", SqlDbType.TinyInt, dme.AliasKBN);
            AddParam(command, "@ZipCD1", SqlDbType.VarChar, dme.ZipCD1);
            AddParam(command, "@ZipCD2", SqlDbType.VarChar, dme.ZipCD2);
            AddParam(command, "@Address1", SqlDbType.VarChar, dme.Address1);
            AddParam(command, "@Address2", SqlDbType.VarChar, dme.Address2);
            AddParam(command, "@Tel11", SqlDbType.VarChar, dme.Tel11);
            AddParam(command, "@Tel12", SqlDbType.VarChar, dme.Tel12);
            AddParam(command, "@Tel13", SqlDbType.VarChar, dme.Tel13);
            AddParam(command, "@Tel21", SqlDbType.VarChar, dme.Tel21);
            AddParam(command, "@Tel22", SqlDbType.VarChar, dme.Tel22);
            AddParam(command, "@Tel23", SqlDbType.VarChar, dme.Tel23);
            AddParam(command, "@JuchuuGaku", SqlDbType.Money, dme.JuchuuGaku);
            AddParam(command, "@Discount", SqlDbType.Money, dme.Discount);
            AddParam(command, "@HanbaiHontaiGaku", SqlDbType.Money, dme.HanbaiHontaiGaku);
            AddParam(command, "@HanbaiTax8", SqlDbType.Money, dme.HanbaiTax8);
            AddParam(command, "@HanbaiTax10", SqlDbType.Money, dme.HanbaiTax10);
            AddParam(command, "@HanbaiGaku", SqlDbType.Money, dme.HanbaiGaku);
            AddParam(command, "@CostGaku", SqlDbType.Money, dme.CostGaku);
            AddParam(command, "@ProfitGaku", SqlDbType.Money, dme.ProfitGaku);
            AddParam(command, "@Point", SqlDbType.Money, dme.Point);
            AddParam(command, "@InvoiceGaku", SqlDbType.Money, dme.InvoiceGaku);
            AddParam(command, "@PaymentMethodCD", SqlDbType.VarChar, dme.PaymentMethodCD);
            AddParam(command, "@PaymentPlanNO", SqlDbType.TinyInt, dme.PaymentPlanNO);
            AddParam(command, "@SalesPlanDate", SqlDbType.Date, dme.SalesPlanDate);
            AddParam(command, "@FirstPaypentPlanDate", SqlDbType.Date, dme.FirstPaypentPlanDate);
            AddParam(command, "@LastPaymentPlanDate", SqlDbType.Date, dme.LastPaymentPlanDate);
            AddParam(command, "@CommentOutStore", SqlDbType.VarChar, dme.CommentOutStore);
            AddParam(command, "@CommentInStore", SqlDbType.VarChar, dme.CommentInStore);
            AddParam(command, "@MitsumoriNO", SqlDbType.VarChar, dme.MitsumoriNO);
            AddParam(command, "@NouhinsyoComment", SqlDbType.VarChar, dme.NouhinsyoComment);

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command, "@PC", SqlDbType.VarChar, pc);

            //OUTパラメータの追加
            string outPutParam = "@OutJuchuuNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.JuchuuNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 受注入力データ取得処理
        /// TempoJuchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectData(D_Juchuu_Entity de, short operationMode)
        {
            string sp = "D_Juchuu_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 進捗チェック　
        /// 既に売上済み,出荷済み,出荷指示済み,ピッキングリスト完了済み,仕入済み,入荷済み,発注済み警告
        /// </summary>
        /// <param name="juchuNo"></param>
        /// <returns></returns>
        public DataTable CheckJuchuData(string juchuNo)
        {
            string sp = "CheckJuchuData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = juchuNo } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 入荷進捗
        /// </summary>
        /// <param name="juchuNo"></param>
        /// <param name="juchuGyoNo"></param>
        /// <returns></returns>
        public DataTable CheckJuchuDetailsData(string juchuNo, string juchuGyoNo)
        {
            string sp = "CheckJuchuDetailsData";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = juchuNo } },
                { "@JuchuuRows", new ValuePair { value1 = SqlDbType.Int, value2 = juchuGyoNo } },
            };

            return SelectData(dic, sp);
        }
        public bool D_Juchu_Update(D_Mitsumori_Entity dme, DataTable dt, string operatorNm, string pc)
        {
            string sp = "D_Juchu_Update";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, operatorNm);
            AddParam(command, "@PC", SqlDbType.VarChar, pc);

            UseTransaction = true;

            string outPutParam = "";    //未使用
            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
        public DataTable D_JuChuu_DataSelect(D_Juchuu_Entity dje)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
               {
                   { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.StoreCD } },
               };
            return SelectData(dic, "D_JuChuu_DataSelect");
        }

        /// <summary>
        /// 店舗レジ出荷売上入力データ取得処理
        /// TempoShukkaNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchuu_SelectData_ForTempoShukkaNyuuryoku(D_Juchuu_Entity de, short operationMode, string salesNO = "")
        {
            string sp = "D_Juchuu_SelectData_ForTempoShukkaNyuuryoku";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
                { "@SalesNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = salesNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 店舗レジ販売履歴照会データ取得処理
        /// TempoRegiHanbaiRirekiよりデータ抽出時に使用
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_SelectData_ForTempoRegiHanbaiRireki(D_Juchuu_Entity de)
        {
            string sp = "D_Juchuu_SelectData_ForTempoRegiHanbaiRireki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入荷入力（受注照会画面）
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_SelectData_ForNyuuka(D_Juchuu_Entity de)
        {
            string sp = "D_Juchuu_SelectData_ForNyuuka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_Juchuu_DataSelect_ForShukkaShoukai(D_Juchuu_Entity dje)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuuNO } },
            };

            return SelectData(dic, "D_Juchuu_DataSelect_ForShukkaShoukai");
        }

        /// <summary>
        /// 店舗受注照会にて使用
        /// </summary>
        /// <param name="doe"></param>
        /// <param name="mse"></param>
        /// <returns></returns>
        public DataTable D_Juchu_SelectAllForShoukai(D_Juchuu_Entity doe, M_SKU_Entity mse, string operatorNm, string pc)
        {
            string sp = "D_Juchu_SelectAllForShoukai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.JuchuDateTo } },
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.SalesDateTo } },
                { "@BillingCloseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.BillingCloseDateFrom } },
                { "@BillingCloseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.BillingCloseDateTo } },
                { "@CollectClearDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.CollectClearDateFrom } },
                { "@CollectClearDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.CollectClearDateTo } },

                { "@ChkMihikiate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMihikiate.ToString() } },
                { "@ChkMiuriage", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMiuriage.ToString() } },
                { "@ChkMiseikyu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMiseikyu.ToString() } },
                { "@ChkMinyukin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMinyukin.ToString() } },
                { "@ChkAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkAll.ToString() } },

                { "@ChkTujo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkTujo.ToString() } },
                { "@ChkHenpin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkHenpin.ToString() } },

                { "@ChkMihachu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMihachu.ToString() } },
                { "@ChkNokiKaito", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkNokiKaito.ToString() } },
                { "@ChkMinyuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMinyuka.ToString() } },
                { "@ChkMisiire", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkMisiire.ToString() } },
                { "@ChkHachuAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.ChkHachuAll.ToString() } },

                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.CustomerCD } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.KanaName } },
                { "@Tel1", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.Tel11 } },
                { "@Tel2", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.Tel12 } },
                { "@Tel3", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.Tel13 } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StaffCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                //{ "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },

                { "@JuchuuNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.JuchuuNOFrom } },
                { "@JuchuuNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.JuchuuNOTo } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorNm} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 出荷入力（JANCDチェック）
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public DataTable D_Juchuu_SelectData_ForShukka(D_Juchuu_Entity de)
        {
            string sp = "D_Juchuu_SelectData_ForShukka";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
            };

            return SelectData(dic, sp);
        }


    }
}
