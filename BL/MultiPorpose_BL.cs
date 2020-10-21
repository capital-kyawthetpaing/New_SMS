using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;
using System.Collections;

namespace BL
{
    public class MultiPorpose_BL : Base_BL
    {
        public static string ID_TANI = "201";
        public static string ID_SPORTS = "202";
        public static string ID_SegmentCD = "203";
        public static string ID_ArrivalPlanCD = "206";
        public static string ID_PaymentMethod = "207";
        public static string ID_BRANDCD = "210";
        public static string ID_MALL = "212";
        public static string ID_JyuchuChance = "216";
        public static string ID_TagName = "219";
        public static string ID_Identification = "220";
        public static string ID_HaspoSaleDate = "224";
        public static string ID_ExhibitionSegmentCD = "226";      //セグメント
        public string ID_Store = "302";                 //受注API起動
        public static string ID_YearTerm = "307";       //年度(20)
        public static string ID_Season = "308";         //シーズン(10)
        public static string ID_PostageCD = "309";      //送料扱いCD(3)
        public static string ID_NoticesCD = "310";      //特記事項CD(3)
        public static string ID_ReserveCD = "311";      //予約フラグCD(3)
        public static string ID_ManufactCD = "312";     //要加工品区分 	
        public static string ID_ConfirmCD = "313";     //要確認品区分
        public string PaymentType = "314";              //支払方法(1)
        public string PaymentMonth = "315";             //支払月
        public static string ID_OrderAttentionCD = "316";//発注区分 
        public string ID_Money = "317";                 //金種区分        
        public static string ID_TempoGenkin = "320";    //店舗現金会員
        public static string ID_ShukkaUriageUpdate = "321";    //出荷売上更新
        public static string ID_Mail = "325";            //Mail履歴
        public static string ID_EDI = "325";            //EDI受信
        public static string ID_TanaFile = "327";            //棚卸読取ファイル

        M_MultiPorpose_DL mmdl;
        public MultiPorpose_BL()
        {
            mmdl = new M_MultiPorpose_DL();
        }
        public DataTable M_MultiPorpose_Select(M_MultiPorpose_Entity mse)
        {
            return mmdl.M_MultiPorpose_Select(mse);
        }

        public DataTable M_MultiPorpose_SelectAll(M_MultiPorpose_Entity mme)
        {
            return mmdl.M_MultiPorpose_SelectAll(mme);
        }

        /// <summary>
        /// Select Key + '：' + Char1										
        /// </summary>
        /// <param name="mme"></param>
        /// <returns></returns>
        public DataTable M_MultiPorpose_SelectForCombo(M_MultiPorpose_Entity mme, string DisplayMember = "")
        {
            DataTable dt = mmdl.M_MultiPorpose_SelectAll(mme);
            int count = dt.Columns.Count;

            if (mme.ID == MultiPorpose_BL.ID_ArrivalPlanCD)
            {
                DataRow[] rows = dt.Select("Num2 NOT IN (1,2,4,6)");

                for (int i = 0; i < rows.Length; i++)
                {
                    if (rows[i].RowState != DataRowState.Deleted)
                    {
                        rows[i].Delete();
                    }
                }
            }

            ArrayList arrlst = new ArrayList();
            arrlst.Add("Key");
            if (DisplayMember.Equals("") || DisplayMember.Equals("KeyAndChar1"))
                arrlst.Add("KeyAndChar1");
            else if (DisplayMember.Equals("Char1"))
                arrlst.Add("Char1");
            else if (DisplayMember.Equals("Key2"))
                arrlst.Add("Key2");

            for (int i = 0; i < count; i++)
            {
                if (!arrlst.Contains(dt.Columns[i].ColumnName))
                {
                    dt.Columns.RemoveAt(i);
                    i--;
                    count--;
                    dt.AcceptChanges();
                }
            }

            return dt;
        }


        public DataTable M_MultiPorpose_SoukoTypeSelect(M_MultiPorpose_Entity mme)
        {
            DataTable dt = mmdl.M_MultiPorpose_SelectAll(mme);
            int count = dt.Columns.Count;

            ArrayList arrlst = new ArrayList();
            arrlst.Add("Key");
            arrlst.Add("IDName");

            for(int i =0;i < count;i++)
            {
                if(!arrlst.Contains(dt.Columns[i].ColumnName))
                {
                    dt.Columns.RemoveAt(i);
                    i--;
                    count--;
                    dt.AcceptChanges();
                }
            }

            return dt;
        }

        public DataTable BindSeqDepo()//Bind data for comboBox
        {
            return SimpleSelect1("14",string.Empty, ID_Money);  
        }

        public DataTable BindSeqKBN()//Bind data for comboBox
        {
            return SimpleSelect1("15",string.Empty, "304");
        }

        public DataTable M_MultiPorpose_SupplierSelect(M_MultiPorpose_Entity mme)
        {
            DataTable dt = mmdl.M_MultiPorpose_SupplierSelect(mme);
            return dt;
        }


        public DataTable M_MultiPorpose_SelectByChar1(M_MultiPorpose_Entity mse)
        {
            return mmdl.M_MultiPorpose_SelectByChar1(mse);
        }

        public bool M_MultiPorpose_SaleModeSelect(M_MultiPorpose_Entity mme)
        {
            mme.ID = MultiPorpose_BL.ID_HaspoSaleDate;
            DataTable dt = mmdl.M_MultiPorpose_SaleModeSelect(mme);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
