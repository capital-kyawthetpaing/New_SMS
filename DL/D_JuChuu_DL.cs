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
    {        /// <summary>
             /// 受注番号検索にて使用
             /// </summary>
             /// <param name="de"></param>
             /// <returns></returns>
        public DataTable D_Juchu_SelectAll(D_Juchuu_Entity de, M_SKU_Entity mse)
        {
            string sp = "D_Juchuu_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateTo } },
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateTo } },
                { "@BillingCloseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateFrom } },
                { "@BillingCloseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateTo } },
                { "@CollectClearDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateFrom } },
                { "@CollectClearDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateTo } },

                { "@ChkMihikiate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihikiate.ToString() } },
                { "@ChkMiuriage", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiuriage.ToString() } },
                { "@ChkMiseikyu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiseikyu.ToString() } },
                { "@ChkMinyukin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyukin.ToString() } },
                { "@ChkAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkAll.ToString() } },

                { "@ChkTujo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkTujo.ToString() } },
                { "@ChkHenpin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHenpin.ToString() } },
                { "@ChkGaisho", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValGaisho.ToString() } },
                { "@ChkTento", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValTento.ToString() } },
                { "@ChkWeb", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValWeb.ToString() } },

                { "@ChkMihachu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihachu.ToString() } },
                { "@ChkNokiKaito", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkNokiKaito.ToString() } },
                { "@ChkMinyuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyuka.ToString() } },
                { "@ChkMisiire", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMisiire.ToString() } },
                { "@ChkHachuAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHachuAll.ToString() } },

                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerName } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@OrderName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorName } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.KanaName } },
                { "@Tel1", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel11 } },
                { "@Tel2", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel12 } },
                { "@Tel3", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel13 } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                //{ "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },

                { "@JuchuuNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOFrom } },
                { "@JuchuuNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOTo } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator} },
                //{ "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }
        public DataTable D_Juchu_SelectAllForSearch_JuchuuProcessNO(D_Juchuu_Entity de, M_SKU_Entity mse)
        {
            string sp = "D_Juchu_SelectAllForSearch_JuchuuProcessNO";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateTo } },
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateTo } },
                { "@BillingCloseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateFrom } },
                { "@BillingCloseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateTo } },
                { "@CollectClearDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateFrom } },
                { "@CollectClearDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateTo } },

                { "@ChkMihikiate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihikiate.ToString() } },
                { "@ChkMiuriage", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiuriage.ToString() } },
                { "@ChkMiseikyu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiseikyu.ToString() } },
                { "@ChkMinyukin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyukin.ToString() } },
                { "@ChkAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkAll.ToString() } },

                { "@ChkTujo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkTujo.ToString() } },
                { "@ChkHenpin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHenpin.ToString() } },
                { "@ChkGaisho", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValGaisho.ToString() } },
                { "@ChkTento", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValTento.ToString() } },
                { "@ChkWeb", new ValuePair { value1 = SqlDbType.Int, value2 = de.ValWeb.ToString() } },

                { "@ChkMihachu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihachu.ToString() } },
                { "@ChkNokiKaito", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkNokiKaito.ToString() } },
                { "@ChkMinyuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyuka.ToString() } },
                { "@ChkMisiire", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMisiire.ToString() } },
                { "@ChkHachuAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHachuAll.ToString() } },

                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerName } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@OrderName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorName } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.KanaName } },
                { "@Tel1", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel11 } },
                { "@Tel2", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel12 } },
                { "@Tel3", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel13 } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                //{ "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },

                { "@JuchuuProcessNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuProcessNOFrom } },
                { "@JuchuuProcessNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuProcessNOTo } },
                { "@JuchuuNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOFrom } },
                { "@JuchuuNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOTo } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Operator} },
                //{ "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }
    /// <summary>
    /// 店舗受注照会にて使用
    /// </summary>
    /// <param name="de"></param>
    /// <param name="mse"></param>
    /// <returns></returns>
    public DataTable D_Juchu_SelectAllForShoukai(D_Juchuu_Entity de, M_SKU_Entity mse, string operatorNm, string pc)
        {
            string sp = "D_Juchu_SelectAllForShoukai";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateTo } },
                { "@SalesDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateFrom } },
                { "@SalesDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SalesDateTo } },
                { "@BillingCloseDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateFrom } },
                { "@BillingCloseDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.BillingCloseDateTo } },
                { "@CollectClearDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateFrom } },
                { "@CollectClearDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CollectClearDateTo } },

                { "@ChkMihikiate", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihikiate.ToString() } },
                { "@ChkMiuriage", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiuriage.ToString() } },
                { "@ChkMiseikyu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMiseikyu.ToString() } },
                { "@ChkMinyukin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyukin.ToString() } },
                { "@ChkAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkAll.ToString() } },

                { "@ChkReji", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkReji.ToString() } },
                { "@ChkGaisho", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkGaisho.ToString() } },
                { "@ChkTujo", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkTujo.ToString() } },
                { "@ChkHenpin", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHenpin.ToString() } },

                { "@ChkMihachu", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMihachu.ToString() } },
                { "@ChkNokiKaito", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkNokiKaito.ToString() } },
                { "@ChkMinyuka", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMinyuka.ToString() } },
                { "@ChkMisiire", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkMisiire.ToString() } },
                { "@ChkHachuAll", new ValuePair { value1 = SqlDbType.TinyInt, value2 = de.ChkHachuAll.ToString() } },

                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
                { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.KanaName } },
                { "@Tel1", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel11 } },
                { "@Tel2", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel12 } },
                { "@Tel3", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.Tel13 } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
                { "@SKUName", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUName } },
                //{ "@ITemCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.ITemCD } },
                { "@SKUCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.SKUCD } },
                { "@JanCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse.JanCD } },

                { "@JuchuuNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOFrom } },
                { "@JuchuuNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNOTo } },
                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = operatorNm} },
                { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }
        public DataTable D_Juchuu_SelectForWeb(D_Juchuu_Entity dje, D_JuchuuStatus_Entity djse)
        {
            string sp = "D_Juchuu_SelectForWeb";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.StoreCD } },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.KanaName } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.CustomerCD } },
                { "@Tel1", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.Tel11 } },
                { "@JuchuuNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuuNOFrom } },
                { "@JuchuuNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuuNOTo } },

                { "@SiteJuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.SiteJuchuuDateFrom } },
                { "@SiteJuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.SiteJuchuuDateTo } },
                { "@JuchuuDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuDateFrom } },
                { "@JuchuuDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuDateTo } },
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.OrderDateTo } },
                { "@NyukinDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.NyukinDateFrom } },
                { "@NyukinDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.NyukinDateTo } },

                { "@DecidedDeliveryDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DecidedDeliveryDateFrom } },
                { "@DecidedDeliveryDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DecidedDeliveryDateTo } },
                { "@DeliveryPlanDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DeliveryPlanDateFrom } },
                { "@DeliveryPlanDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DeliveryPlanDateTo } },
                { "@DeliveryDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DeliveryDateFrom } },
                { "@DeliveryDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.DeliveryDateTo } },
                { "@InvoiceNOFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.InvoiceNOFrom } },
                { "@InvoiceNOTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.InvoiceNOTo } },
            
                { "@SiteJuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.SiteJuchuuNO } },


                { "@IncludeFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.IncludeFLG } },
                { "@GiftFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.GiftFLG } },
                { "@NoshiFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.NoshiFLG } },
                { "@NouhinsyoFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.NouhinsyoFLG } },
                { "@RyousyusyoFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.RyousyusyoFLG } },
                { "@SonotoFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.SonotoFLG } },
                { "@TelephoneContactKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.TelephoneContactKBN } },
                { "@IndividualContactKBN", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.IndividualContactKBN } },
                { "@NoMailFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.NoMailFLG } },
                { "@CancelFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.CancelFLG } },
                { "@ReturnFLG", new ValuePair { value1 = SqlDbType.TinyInt, value2 = dje.ReturnFLG } },

                { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.Operator} },
                //{ "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = pc} },

            };

            return SelectData(dic, sp);
        }
        public DataTable D_Juchuu_SelectForWebHikiate(D_Juchuu_Entity dje)
        {
            string sp = "D_Juchuu_SelectForWebHikiate";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.JuchuuNO } },
            };

            return SelectData(dic, sp);
        }
        public DataTable BindForWebJuchuuKakunin(D_Juchuu_Entity de, int kbn)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@Kbn", new ValuePair { value1 = SqlDbType.TinyInt, value2 = kbn.ToString() } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.Date, value2 = de.ChangeDate } }

            };
            return SelectData(dic, "BindForWebJuchuuKakunin");
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
        public bool D_Juchu_Exec(D_Juchuu_Entity dme, DataTable dt, short operationMode)
        {
            string sp = "PRC_TempoJuchuuNyuuryoku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@OperateMode", SqlDbType.Int,operationMode.ToString());
            AddParam(command, "@JuchuuNO", SqlDbType.VarChar, dme.JuchuuNO);
            AddParam(command, "@JuchuuProcessNO", SqlDbType.VarChar, dme.JuchuuProcessNO);
            AddParam(command, "@StoreCD", SqlDbType.VarChar, dme.StoreCD);
            AddParam(command, "@JuchuuDate", SqlDbType.VarChar, dme.JuchuuDate);
            AddParam(command, "@ReturnFLG", SqlDbType.TinyInt, dme.ReturnFLG);
            AddParam(command, "@SoukoCD", SqlDbType.VarChar, dme.SoukoCD);
            AddParam(command,"@StaffCD", SqlDbType.VarChar, dme.StaffCD);
            AddParam(command,"@CustomerCD", SqlDbType.VarChar, dme.CustomerCD);
            AddParam(command,"@CustomerName", SqlDbType.VarChar, dme.CustomerName);
            AddParam(command,"@CustomerName2", SqlDbType.VarChar, dme.CustomerName2 );
            AddParam(command,"@AliasKBN", SqlDbType.TinyInt, dme.AliasKBN);
            AddParam(command,"@ZipCD1", SqlDbType.VarChar, dme.ZipCD1 );
            AddParam(command,"@ZipCD2", SqlDbType.VarChar, dme.ZipCD2 );
            AddParam(command,"@Address1", SqlDbType.VarChar, dme.Address1);
            AddParam(command,"@Address2", SqlDbType.VarChar, dme.Address2);
            AddParam(command,"@Tel11", SqlDbType.VarChar, dme.Tel11);
            AddParam(command,"@Tel12", SqlDbType.VarChar, dme.Tel12);
            AddParam(command,"@Tel13", SqlDbType.VarChar, dme.Tel13);
            AddParam(command,"@Tel21", SqlDbType.VarChar, dme.Tel21);
            AddParam(command,"@Tel22", SqlDbType.VarChar, dme.Tel22);
            AddParam(command,"@Tel23", SqlDbType.VarChar, dme.Tel23);

            AddParam(command, "@DeliveryCD", SqlDbType.VarChar, dme.DeliveryCD);
            AddParam(command, "@DeliveryName", SqlDbType.VarChar, dme.DeliveryName);
            AddParam(command, "@DeliveryName2", SqlDbType.VarChar, dme.DeliveryName2);
            AddParam(command, "@DeliveryAliasKBN", SqlDbType.TinyInt, dme.DeliveryAliasKBN);
            AddParam(command, "@DeliveryZipCD1", SqlDbType.VarChar, dme.DeliveryZipCD1);
            AddParam(command, "@DeliveryZipCD2", SqlDbType.VarChar, dme.DeliveryZipCD2);
            AddParam(command, "@DeliveryAddress1", SqlDbType.VarChar, dme.DeliveryAddress1);
            AddParam(command, "@DeliveryAddress2", SqlDbType.VarChar, dme.DeliveryAddress2);
            AddParam(command, "@DeliveryTel11", SqlDbType.VarChar, dme.DeliveryTel11);
            AddParam(command, "@DeliveryTel12", SqlDbType.VarChar, dme.DeliveryTel12);
            AddParam(command, "@DeliveryTel13", SqlDbType.VarChar, dme.DeliveryTel13);

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
            //AddParam(command, "@OrderHontaiGaku", SqlDbType.Money, dme.OrderHontaiGaku);
            //AddParam(command, "@OrderTax8", SqlDbType.Money, dme.OrderTax8);
            //AddParam(command, "@OrderTax10", SqlDbType.Money, dme.OrderTax10);
            //AddParam(command, "@OrderGaku", SqlDbType.Money, dme.OrderGaku);
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
            AddParam(command,"@Operator", SqlDbType.VarChar, dme.InsertOperator);
            AddParam(command,"@PC", SqlDbType.VarChar, dme.PC);

            //OUTパラメータの追加
            string outPutParam = "@OutJuchuuNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret= InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dme.JuchuuNO = outPutParam;

            return ret;
        }

        /// <summary>
        /// 受注入力データ取得処理
        /// TempoJuchuuNyuuryokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectData(D_Juchuu_Entity de, short operationMode, short tennic=0)
        {
            string sp = "D_Juchuu_SelectData";

            //command.Parameters.Add("@SyoKBN", SqlDbType.TinyInt).Value = mie.SyoKBN;
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
                { "@Tennic", new ValuePair { value1 = SqlDbType.TinyInt, value2 = tennic.ToString() } },
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

        public DataTable D_Juchu_SelectDataForTempoUriage(D_Juchuu_Entity de, short operationMode)
        {
            string sp = "D_Juchu_SelectDataForTempoUriage";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OperateMode", new ValuePair { value1 = SqlDbType.TinyInt, value2 = operationMode.ToString() } },
                { "@DateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateFrom } },
                { "@DateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuDateTo } },
                { "@CustomerCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerCD } },
                { "@CustomerName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.CustomerName} },
                { "@KanaName", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.KanaName} },
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StaffCD } },
            };

            return SelectData(dic, sp);
        }
        public DataTable D_Juchuu_SelectForSeisan(D_Juchuu_Entity dje)
        {
            string sp = "D_Juchuu_SelectForSeisan";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = dje.StoreCD } },
                { "@Date", new ValuePair { value1 = SqlDbType.Date, value2 = dje.ChangeDate } }

            };

            return SelectData(dic, sp);
        }
        /// <summary>
        /// 名寄せ結果登録データ取得処理
        /// NayoseKekkaTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Juchu_SelectForNayose(D_Juchuu_Entity de)
        {
            string sp = "D_Juchu_SelectForNayose";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@SiteKBN", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.SiteKBN } },
                { "@NayoseKekkaTourokuDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.NayoseKekkaTourokuDate } },
            };

            return SelectData(dic, sp);
        }
        public bool NayoseKekkaTouroku_Exec(D_Juchuu_Entity dje, DataTable dt)
        {
            string sp = "PRC_NayoseKekkaTouroku";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@NayoseKekkaTourokuDate", SqlDbType.VarChar, dje.NayoseKekkaTourokuDate);
            AddParamForDataTable(command, "@Table", SqlDbType.Structured, dt);
            AddParam(command, "@Operator", SqlDbType.VarChar, dje.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dje.PC);

            UseTransaction = true;

            string outPutParam = "";    //未使用

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);

            return ret;
        }
        public bool NayoseSyoriAll_Exec(D_Juchuu_Entity dje)
        {
            string sp = "PRC_NayoseSyoriAll";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@Operator", SqlDbType.VarChar, dje.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dje.PC);

            UseTransaction = true;

            //OUTパラメータの追加
            string outPutParam = "@OutErrNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dje.ReturnFLG = outPutParam;

            return ret;
        }
        public bool JuchuuDataCheck_Exec(D_Juchuu_Entity dje)
        {
            string sp = "PRC_JuchuuDataCheck";

            command = new SqlCommand(sp, GetConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            AddParam(command, "@Operator", SqlDbType.VarChar, dje.InsertOperator);
            AddParam(command, "@PC", SqlDbType.VarChar, dje.PC);

            UseTransaction = true;

            //OUTパラメータの追加
            string outPutParam = "@OutErrNo";
            command.Parameters.Add(outPutParam, SqlDbType.VarChar, 11);
            command.Parameters[outPutParam].Direction = ParameterDirection.Output;

            UseTransaction = true;

            bool ret = InsertUpdateDeleteData(sp, ref outPutParam);
            if (ret)
                dje.ReturnFLG = outPutParam;

            return ret;
        }
        public bool DeleteTemporaryReserve(D_Juchuu_Entity de)
        {
            string sp = "DeleteTemporaryReserve";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.JuchuuNO } },
            };

            return InsertUpdateDeleteData(dic, sp);
        }
        public DataTable GetTemporaryReserveNO(string Denno)
        {
            string sp = "GetTemporaryReserveNO";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = Denno } },
            };

            return SelectData(dic, sp);
        }
        public DataTable GetJuchuuNO(string Processno)
        {
            string sp = "GetJuchuuNO";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@JuchuuProcessNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = Processno } },
            };

            return SelectData(dic, sp);
        }
        public DataTable GetNouki(string date, string storeCD)
        {
            string sp = "GetNouki";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = storeCD } },
                { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = date } },
            };

            return SelectData(dic, sp);
        }

    }
}
