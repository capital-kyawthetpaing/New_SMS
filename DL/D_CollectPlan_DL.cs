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
    public class D_CollectPlan_DL : Base_DL
    {
        /// <summary>
        /// 請求締処理よりチェック処理時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_Check(D_CollectPlan_Entity dce)
        {
            string sp = "D_CollectPlan_Check";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Syori", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.Syori } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CustomerCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.BillingDate } },
                { "@BillingCloseDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.BillingCloseDate } }
            };
            return SelectData(dic, sp);
        }

        /// <summary>
        /// 入金予定表よりデータ抽出時に使用
        /// </summary>
        /// <param name="dce"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_SelectForPrint(D_CollectPlan_Entity dce, M_Customer_Entity mce)
        {
            string sp = "D_CollectPlan_SelectForPrint";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.DateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.DateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.StaffCD } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dce.CustomerCD } },
                { "@PrintFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.PrintFLG } },
                { "@PaymentProgressKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.PaymentProgressKBN } },
                { "@DetailOn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dce.DetailOn } },
            };

            if (dce.PrintFLG == "1")
            {
                DataTable dt1 = SelectData(dic, sp);
                DataRow[] drs1 = dt1.Select(" BillingNO IS NOT NULL");
                DataTable newdt1 = dt1.Clone();

                foreach (var dr in drs1)
                {
                    //消込済分印刷=OFFの場合　PaymentProgressKBN＜＝１
                    if (dce.PaymentProgressKBN == "0")
                        if (Convert.ToInt16( dr["PaymentProgressKBN"]) > 1)
                            continue;

                    //newdt.Rows.Add(dr)ではダメ。
                    //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                    //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                    DataRow newrow1 = newdt1.NewRow();
                    newrow1.ItemArray = dr.ItemArray;
                    newdt1.Rows.Add(newrow1);
                }
                return newdt1;
            }
            else
            {
                //消込済分印刷=OFFの場合　PaymentProgressKBN＜＝１
                if (dce.PaymentProgressKBN == "0")
                {
                    DataTable dt1 = SelectData(dic, sp);
                    DataRow[] drs1 = dt1.Select(" PaymentProgressKBN <= 1");
                    DataTable newdt1 = dt1.Clone();

                    foreach (var dr in drs1)
                    {
                        //newdt.Rows.Add(dr)ではダメ。
                        //drはdtに所属している行なので、別のDataTableであるnewdtにはAddできない。
                        //よって、newdtの新しい行を作成し、その各列の値をdrと全く同じにし、それをnewdtに追加すれば良い。
                        DataRow newrow1 = newdt1.NewRow();
                        newrow1.ItemArray = dr.ItemArray;
                        newdt1.Rows.Add(newrow1);
                    }
                    return newdt1;
                }
                else
                    return SelectData(dic, sp);
            }

        
        }

        /// <summary>
        /// 未入金確認照会にて使用
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_SelectAllForSyokai(D_CollectPlan_Entity doe)
        {
            string sp = "D_CollectPlan_SelectAllForSyokai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@ShippingDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.DateFrom } },
                { "@ShippingDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.DateTo } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                { "@PaymentMethodCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PaymentMethodCD } },
                { "@Kbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = doe.Kbn.ToString() } },
            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 未入金確認照会にて使用
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_CollectPlan_SelectAllForSyosai(D_CollectPlan_Entity doe)
        {
            string sp = "D_CollectPlan_SelectAllForSyosai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@CollectPlanNO", new ValuePair { value1 = SqlDbType.Int, value2 = doe.CollectPlanNO } },
            };

            return SelectData(dic, sp);
        }

        public DataTable NyuukinKesikomiItiranHyou_Report(D_Collect_Entity collect_data)
        {
            string sp = "NyuukinKesikomiItiranHyou_Report";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                 { "@CollectDateF", new ValuePair { value1 = SqlDbType.Date, value2 = collect_data.CollectDateFrom } },
                 { "@CollectDateT", new ValuePair { value1 = SqlDbType.Date, value2 = collect_data.CollectDateTo } },
                 { "@InputDateTimeF", new ValuePair { value1 = SqlDbType.Date, value2 = collect_data.InputDateFrom } },
                 { "@InputDateTimeT", new ValuePair { value1 = SqlDbType.Date, value2 = collect_data.InputDateTo } },
                 { "@WebCollectType", new ValuePair { value1 = SqlDbType.VarChar, value2 = collect_data.WebCollectType } },
                 { "@CollectCustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = collect_data.CollectCustomerCD } }
            };
            return SelectData(dic, sp);
        }
    }
}
