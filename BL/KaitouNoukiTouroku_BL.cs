using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class KaitouNoukiTouroku_BL : Base_BL
    {
        D_Hacchu_DL mdl;
        public KaitouNoukiTouroku_BL()
        {
            mdl = new D_Hacchu_DL();
        }

        /// <summary>
        /// 回答納期登録更新処理
        /// KaitouNoukiTourokuより更新時に使用
        /// </summary>
        public bool Order_Exec(D_Order_Entity dme, DataTable dt, short operationMode, string operatorNm, string pc)
        {
            return mdl.D_Order_ExecForKaitouNouki(dme, dt, operationMode, operatorNm, pc);
        }

        /// <summary>
        /// 回答納期登録取得処理
        /// KaitouNoukiTourokuよりデータ抽出時に使用
        /// </summary>
        public DataTable D_Order_SelectDataForKaitouNouki(D_Order_Entity de)
        {
            DataTable dt = mdl.D_Order_SelectDataForKaitouNouki(de);

            return dt;
        }

        /// <summary>
        /// 入荷予定日チェック処理
        /// </summary>
        /// <param name="inYmd">チェック対象日付</param>
        /// <param name="outYmd">YYYY/MM/DDのフォーマット</param>
        /// <returns></returns>
        public bool ChkArrivePlanDate(string inYmd, ref string outYmd)
        {
            //入力無くても良い(It is not necessary to input)
            if (string.IsNullOrWhiteSpace(inYmd))
            {
                outYmd = "";
                return true;
            }
            else
            {
                outYmd = FormatDate(inYmd);

                //日付として正しいこと(Be on the correct date)Ｅ１０３
                if (!CheckDate(outYmd))
                {
                    //Ｅ１０３
                    ShowMessage("E103");
                    return false;
                }
                //今日より前の日付はエラー
                string sysDate = GetDate();
                int result = outYmd.CompareTo(sysDate);
                if (result < 0)
                {
                    ShowMessage("E123");
                    return false;
                }
            }

            return true; ;
        }
        /// <summary>
        /// 入荷予定月のチェック処理
        /// </summary>
        /// <param name="inYm">チェック対象年月</param>
        /// <param name="arrivalPranDate">入荷予定日</param>
        /// <param name="outYm">YYYY/MMのフォーマット</param>
        /// <returns></returns>
        public bool ChkArrivalPlanMonth(string inYm, string arrivalPranDate, ref string outYm)
        {
            //入力無くても良い(It is not necessary to input)
            if (string.IsNullOrWhiteSpace(inYm))
            {
                return true;

            }
            //入荷予定日に入力がある場合、入力されたらエラー 「入荷予定日が入力済みのため、入力不要です。」
            if (!string.IsNullOrWhiteSpace(arrivalPranDate))
            {
                ShowMessage("E232");
                return false;
            }

            string ymd = FormatDate(inYm + "/01");
            if (!CheckDate(ymd))
            {
                //Ｅ１０３
                ShowMessage("E103");
                return false;
            }
            outYm = ymd.Substring(0, 7);

            //今月より前の年月はエラー
            string sysDate = GetDate();
            int result = outYm.CompareTo(sysDate.Substring(0, 7));
            if (result < 0)
            {
                ShowMessage("E123");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 入荷予定区分のチェック処理
        /// </summary>
        /// <param name="kbn">入荷予定区分</param>
        /// <param name="arrivalPlanDate">入荷予定日</param>
        /// <param name="arrivalPlanMonth">入荷予定月</param>
        /// <returns></returns>
        public bool ChkArrivalPlanKbn(string kbn, string arrivalPlanDate, string arrivalPlanMonth, out string num2)
        {
            //選択無くてもよい
            DataTable dt = null;
            num2 = "0";

            if (!string.IsNullOrWhiteSpace(kbn))
            {
                M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                {
                    ID = MultiPorpose_BL.ID_ArrivalPlanCD,
                    Key = kbn
                };

                MultiPorpose_BL mbl = new MultiPorpose_BL();
                dt = mbl.M_MultiPorpose_Select(me);
                if (dt.Rows.Count > 0)
                {
                    num2 = dt.Rows[0]["Num2"].ToString();
                }
                else
                {
                    //Ｅ１０１
                    ShowMessage("E101");
                    return false;
                }

                //入荷予定日に入力がある場合
                //区分に入力されたら、M_MultiPorpose.Num2が2以外はエラー
                if (!string.IsNullOrWhiteSpace(arrivalPlanDate))
                {
                    if (num2 != "2")
                    {
                        ShowMessage("E175");
                        return false;
                    }

                }
            }

            //年月に入力がある場合
            //入力必須
            //Num2が1以外はエラー
            if (!string.IsNullOrWhiteSpace(arrivalPlanMonth))
            {
                if (string.IsNullOrWhiteSpace(kbn))
                {
                    //Ｅ１０２
                    ShowMessage("E102");
                    return false;
                }
                else if (num2 != "1")
                {
                    ShowMessage("E175");
                    return false;
                }

            }
            ////入荷予定日、年月にともに入力がない場合
            //if (string.IsNullOrWhiteSpace(arrivalPlanDate) && string.IsNullOrWhiteSpace(arrivalPlanMonth))
            //{
            //    //入力必須
            //    if (string.IsNullOrWhiteSpace(kbn))
            //    {
            //        //Ｅ１０２
            //        ShowMessage("E102");
            //        return false;
            //    }
            //    //Num2が3,4,5,6以外はエラー
            //    else if (num2 != "3" && num2 != "4" && num2 != "5" && num2 != "6")
            //    {
            //        ShowMessage("E175");
            //        return false;
            //    }
            //}
            return true;
        }
        /// <summary>
        /// 回答納期確認書よりデータ取得
        /// </summary>
        /// <param name="doe"></param>
        /// <returns></returns>
        public DataTable D_ArrivalPlan_SelectForPrint(D_Order_Entity doe)
        {
            return mdl.D_ArrivalPlan_SelectForPrint(doe);
        }
    }
}
