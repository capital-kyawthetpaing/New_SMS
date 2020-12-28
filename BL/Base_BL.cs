using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Windows.Forms;
using System.Globalization;

namespace BL
{
    public class Base_BL
    {
        /// <summary>
        /// change datatable to xml format
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>return xml format string</returns>
        public String DataTableToXml(DataTable dt)
        {
            dt.TableName = "test";
            System.IO.StringWriter writer = new System.IO.StringWriter();
            dt.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            string result = writer.ToString();
            return result;
        }

        /// <summary>
        /// select data from single table only
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="tableName"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        //public DataTable SimpleSelect(string fields, string tableName, string condition)
        //{
        //    Base_DL bdl = new Base_DL();

        //    return bdl.SimpleSelect(fields, tableName, condition);
        //}

        /// <summary>
        /// to show Message
        /// MessageID is require,other params are nullable
        /// eg.ShowMessage("I0001"); ---> OK.No need to add all params if not necessary
        /// </summary>
        /// <param name="MessageID">Message ID eg.I001</param>
        /// <param name="text1">to replace {1}</param>
        /// <param name="text2">to replace {2}</param>
        /// <param name="text3">to replace {3}</param>
        /// <param name="text4">to replace {4}</param>
        /// <param name="text5">to replace {5}</param>
        /// <returns>return DialogResult(Yes,No,OK,Cancel)</returns>
        public DialogResult ShowMessage(string MessageID, string text1 = null, string text2 = null, string text3 = null, string text4 = null, string text5 = null)
        {
            M_Message_Entity mme = new M_Message_Entity
            {
                MessageID = MessageID,
                MessageText1 = string.IsNullOrWhiteSpace(text1) ? string.Empty : text1,
                MessageText2 = string.IsNullOrWhiteSpace(text2) ? string.Empty : text2,
                MessageText3 = string.IsNullOrWhiteSpace(text3) ? string.Empty : text3,
                MessageText4 = string.IsNullOrWhiteSpace(text4) ? string.Empty : text4,
                MessageText5 = string.IsNullOrWhiteSpace(text5) ? string.Empty : text5
            };

            return GetMessage(mme);
        }

        /// <summary>
        /// show message
        /// </summary>
        /// <param name="mme"></param>
        /// <returns>dialogresult(ok,cancel,...)</returns>
        private DialogResult GetMessage(M_Message_Entity mme)
        {
            M_Message_DL mmdl = new M_Message_DL();
            DataTable dtMsg = mmdl.M_Message_Select(mme);
            string message = string.Empty;
            string MessageID;
            if (dtMsg.Rows.Count > 0)
            {
                message = ReplaceMessage(dtMsg.Rows[0]["MessageText1"].ToString(), mme);
                message += !string.IsNullOrWhiteSpace(dtMsg.Rows[0]["MessageText2"].ToString()) ? "\n\n" + ReplaceMessage(dtMsg.Rows[0]["MessageText2"].ToString(), mme) : string.Empty;
                message += !string.IsNullOrWhiteSpace(dtMsg.Rows[0]["MessageText3"].ToString()) ? "\n\n" + ReplaceMessage(dtMsg.Rows[0]["MessageText3"].ToString(), mme) : string.Empty;
                message += !string.IsNullOrWhiteSpace(dtMsg.Rows[0]["MessageText4"].ToString()) ? "\n\n" + ReplaceMessage(dtMsg.Rows[0]["MessageText4"].ToString(), mme) : string.Empty;
                MessageID = !string.IsNullOrWhiteSpace(dtMsg.Rows[0]["MessageID"].ToString()) ? "\n\n" + ReplaceMessage(dtMsg.Rows[0]["MessageID"].ToString(), mme) : string.Empty;
                // MessageID = ReplaceMessage(dtMsg.Rows[0]["MessageID"].ToString(), mme);

                MessageBoxButtons msgbtn = dtMsg.Rows[0]["MessageButton"].ToString().Equals("1") ? MessageBoxButtons.OK :
                                           dtMsg.Rows[0]["MessageButton"].ToString().Equals("2") ? MessageBoxButtons.OKCancel :
                                           dtMsg.Rows[0]["MessageButton"].ToString().Equals("3") ? MessageBoxButtons.RetryCancel :
                                           dtMsg.Rows[0]["MessageButton"].ToString().Equals("4") ? MessageBoxButtons.YesNo :
                                           dtMsg.Rows[0]["MessageButton"].ToString().Equals("5") ? MessageBoxButtons.YesNoCancel :
                                           MessageBoxButtons.AbortRetryIgnore;

                MessageBoxIcon msgicon = dtMsg.Rows[0]["MessageMark"].ToString().Equals("1") ? MessageBoxIcon.Information :
                                         dtMsg.Rows[0]["MessageMark"].ToString().Equals("2") ? MessageBoxIcon.Asterisk :
                                         dtMsg.Rows[0]["MessageMark"].ToString().Equals("3") ? MessageBoxIcon.Question :
                                         dtMsg.Rows[0]["MessageMark"].ToString().Equals("4") ? MessageBoxIcon.Error :
                                         dtMsg.Rows[0]["MessageMark"].ToString().Equals("5") ? MessageBoxIcon.Stop :
                                         dtMsg.Rows[0]["MessageMark"].ToString().Equals("6") ? MessageBoxIcon.Exclamation :
                                         MessageBoxIcon.None;
                if(mme.MessageID=="Q003")
                    return MessageBox.Show(message, mme.MessageID, msgbtn, msgicon, MessageBoxDefaultButton.Button2);
                else
                    return MessageBox.Show(message, mme.MessageID, msgbtn, msgicon, MessageBoxDefaultButton.Button1);
            }
            else
            {
                return MessageBox.Show("システムで予約されたコード（メッセージマスタ未登録）", "エラー(" + mme.MessageID + ")", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// replace message {1}-->param1,{2}-->param2
        /// </summary>
        /// <param name="originalText">select from db</param>
        /// <param name="mme">replace text</param>
        /// <returns></returns>
        private string ReplaceMessage(string originalText, M_Message_Entity mme)
        {
            if (!string.IsNullOrWhiteSpace(originalText))
            {
                if (originalText.Contains("{0}"))
                    originalText = originalText.Replace("{0}", mme.MessageText1);
                if (originalText.Contains("{1}"))
                    originalText = originalText.Replace("{1}", mme.MessageText2);
                if (originalText.Contains("{2}"))
                    originalText = originalText.Replace("{2}", mme.MessageText3);
                if (originalText.Contains("{3}"))
                    originalText = originalText.Replace("{3}", mme.MessageText4);
                if (originalText.Contains("{4}"))
                    originalText = originalText.Replace("{4}", mme.MessageText5);

                //originalText += !string.IsNullOrWhiteSpace(mme.MessageText2) ? originalText.Replace("{2}", mme.MessageText2) : string.Empty;
                //originalText += !string.IsNullOrWhiteSpace(mme.MessageText3) ? originalText.Replace("{3}", mme.MessageText3) : string.Empty;
                //originalText += !string.IsNullOrWhiteSpace(mme.MessageText4) ? originalText.Replace("{4}", mme.MessageText4) : string.Empty;
                //originalText += !string.IsNullOrWhiteSpace(mme.MessageText5) ? originalText.Replace("{5}", mme.MessageText5) : string.Empty;
            }
            return originalText;
        }

        /// <summary>
        /// Access Check
        /// </summary>
        /// <param name="mae">Staff And Program Relation Check</param>
        /// <returns>if no access to open then return null else return all info</returns>
        public M_AuthorizationsDetails_Entity M_Authorizations_AccessCheck(M_Authorizations_Entity mae)
        {
            M_Authorizations_DL madl = new M_Authorizations_DL();
            DataTable dtAuthorization = madl.M_Authorizations_AccessCheck(mae);

            if (dtAuthorization.Rows.Count > 0)
            {
                string messageID = dtAuthorization.Rows[0]["MessageID"].ToString();
                if (messageID.Equals("allow"))
                {
                    M_AuthorizationsDetails_Entity made = new M_AuthorizationsDetails_Entity
                    {
                        Insertable = dtAuthorization.Rows[0]["Insertable"].ToString(),
                        Updatable = dtAuthorization.Rows[0]["Updatable"].ToString(),
                        Deletable = dtAuthorization.Rows[0]["Deletable"].ToString(),
                        Inquirable = dtAuthorization.Rows[0]["Inquirable"].ToString(),
                        Printable = dtAuthorization.Rows[0]["Printable"].ToString(),
                        Outputable = dtAuthorization.Rows[0]["Outputable"].ToString(),
                        Runable = dtAuthorization.Rows[0]["Runable"].ToString(),
                        StoreAuthorizationsCD = dtAuthorization.Rows[0]["StoreAuthorizationsCD"].ToString(),
                        StoreAuthorization_ChangeDate = dtAuthorization.Rows[0]["ChangeDate"].ToString(),

                        //KTP 2019-05-29 select ProgramName and Type
                        ProgramID = dtAuthorization.Rows[0]["ProgramID"].ToString(),
                        ProgramName = dtAuthorization.Rows[0]["ProgramName"].ToString(),
                        ProgramType = dtAuthorization.Rows[0]["Type"].ToString()
                    };
                    return made;
                }
                else
                {
                    mae.MessageID = messageID;
                  //  mae.DeleteFlg = 
                    return null;
                }
            }
            return null;
        }

        public DataTable M_StoreAuthorizations_Select(M_StoreAuthorizations_Entity msa)
        {
            M_Authorizations_DL madl = new M_Authorizations_DL();
            DataTable dt = madl.M_StoreAuthorizations_Select(msa);
            return dt;
        }

        /// <summary>
        /// log insert
        /// </summary>
        /// <param name="lle"></param>
        public void L_Log_Insert(L_Log_Entity lle)
        {
            L_Log_DL lldl = new L_Log_DL();
            lldl.L_Log_Insert(lle);
        }

        /// <summary>
        /// search text.contain funciton by multipel param with commaseparated value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public bool IsContain(string text, string searchText)
        {
            string[] strArr = searchText.Split(',');
            foreach (string str in strArr)
            {
                if (text.Contains(str))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// check string is integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //public bool IsInteger(string value)
        //{
        //    if (System.Text.RegularExpressions.Regex.IsMatch(value, "\\d+"))
        //        return true;
        //    else
        //        return false;
        //}
        public bool IsInteger(string value)
        {
            value = value.Replace("-", "");
            if (Int64.TryParse(value, out Int64 Num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// check date is correct
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckDate(string value)
        {
            return DateTime.TryParseExact(value,
                       "yyyy/MM/dd",
                       System.Globalization.CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime d);
        }

        public string FormatDate(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (this.IsInteger(value.Replace("/", "").Replace("-", "")))
                {
                    string day = string.Empty, month = string.Empty, year = string.Empty;
                    if (value.Contains("/"))
                    {
                        string[] date = value.Split('/');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        //value = value.Replace("/", "");
                        value = year + month + day;
                    }
                    else if (value.Contains("-"))
                    {
                        string[] date = value.Split('-');
                        day = date[date.Length - 1].PadLeft(2, '0');
                        month = date[date.Length - 2].PadLeft(2, '0');

                        if (date.Length > 2)
                            year = date[date.Length - 3];

                        value = year + month + day;
                    }

                    string text = value;
                    text = text.PadLeft(8, '0');
                    day = text.Substring(text.Length - 2);
                    month = text.Substring(text.Length - 4).Substring(0, 2);
                    year = Convert.ToInt32(text.Substring(0, text.Length - 4)).ToString();

                    if (month == "00")
                    {
                        month = string.Empty;
                    }
                    if (year == "0")
                    {
                        year = string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(month))
                        month = DateTime.Now.Month.ToString().PadLeft(2, '0');//if user doesn't input for month,set current month

                    if (string.IsNullOrWhiteSpace(year))
                    {
                        year = DateTime.Now.Year.ToString();//if user doesn't input for year,set current year
                    }
                    else
                    {
                        if (year.Length == 1)
                            year = "200" + year;
                        else if (year.Length == 2)
                            year = "20" + year;
                    }

                    string strdate = year + "/" + month + "/" + day;
                    if (DateTime.TryParseExact(strdate,
                       "yyyy/MM/dd",
                       System.Globalization.CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out DateTime dd))
                    {
                        return strdate;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        public string GetDate()
        {
            return Base_DL.iniEntity.DatabaseDate;
        }
        public DataTable Select_SearchName(string changeDate, int type, string param1 = null, string param2 = null, string param3 = null)
        {
            Base_DL bdl = new Base_DL();
            string sp = "Select_SearchName";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@ChangeDate", new ValuePair { value1 = SqlDbType.VarChar, value2 = changeDate } },
                    { "@CD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = param1==null? DBNull.Value.ToString(): param1 } },
                    { "@CD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = param2==null? DBNull.Value.ToString(): param2 } },
                    { "@CD3", new ValuePair { value1 = SqlDbType.VarChar, value2 =  param3==null? DBNull.Value.ToString(): param3 } },
                    { "@Type", new ValuePair { value1 = SqlDbType.TinyInt, value2 = type.ToString() } },
                };

            return bdl.Select_SearchName(dic, sp);
        }
        public bool CheckInputPossibleDate(string value)
        {
            //[M_Control]
            M_Control_DL cdl = new M_Control_DL();
            M_Control_Entity me = new M_Control_Entity();

            me.MainKey = "1";
            me.ChangeDate = value;

            DataTable dt = cdl.M_Control_CheckDate(me);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool CheckInputPossibleDateWithFisicalMonth(string value)
        {
            //[M_Control]
            M_Control_DL cdl = new M_Control_DL();
            M_Control_Entity me = new M_Control_Entity();

            me.MainKey = "1";
            me.ChangeDate = value;

            DataTable dt = cdl.M_Control_CheckDateWithFisicalMonth(me);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        public int GetTennic()
        {
            //[M_Control]
            M_Control_DL cdl = new M_Control_DL();
            M_Control_Entity me = new M_Control_Entity();

            me.MainKey = "1";

            DataTable dt = cdl.M_Control_Select(me);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt16(dt.Rows[0]["Tennic"]);
            }
            return 0;
        }
        /// <summary>	
        /// 店舗の締日チェック	
        /// 店舗締マスターで判断	
        /// </summary>	
        /// <param name="mse">M_StoreClose_Entity</param>	
        /// <returns></returns>	
        public bool CheckStoreClose(M_StoreClose_Entity mse, bool uri, bool sir, bool nyk, bool sha, bool son)
        {
            M_StoreClose_DL msdl = new M_StoreClose_DL();
            DataTable dt = msdl.M_StoreClose_Select(mse);
            if (dt.Rows.Count > 0)
            {
                //M_StoreClose.ClosePosition2＝	1		
                //M_StoreClose.ClosePosition2＝	2		
                //であればエラー	
                if (uri)
                {
                    if (dt.Rows[0]["ClosePosition1"].ToString().Equals("1"))
                    {
                        ShowMessage("E203");
                        return false;
                    }
                    else if (dt.Rows[0]["ClosePosition1"].ToString().Equals("2"))
                    {
                        ShowMessage("E194");
                        return false;
                    }
                }
                if (sir)
                {
                    if (dt.Rows[0]["ClosePosition2"].ToString().Equals("1"))
                    {
                        ShowMessage("E203");
                        return false;
                    }
                    else if (dt.Rows[0]["ClosePosition2"].ToString().Equals("2"))
                    {
                        ShowMessage("E194");
                        return false;
                    }
                }
                if (nyk)
                {
                    if (dt.Rows[0]["ClosePosition3"].ToString().Equals("1"))
                    {
                        ShowMessage("E203");
                        return false;
                    }
                    else if (dt.Rows[0]["ClosePosition3"].ToString().Equals("2"))
                    {
                        ShowMessage("E194");
                        return false;
                    }
                }
                if (sha)
                {
                    if (dt.Rows[0]["ClosePosition4"].ToString().Equals("1"))
                    {
                        ShowMessage("E203");
                        return false;
                    }
                    else if (dt.Rows[0]["ClosePosition4"].ToString().Equals("2"))
                    {
                        ShowMessage("E194");
                        return false;
                    }
                }
                if (son)
                {
                    if (dt.Rows[0]["ClosePosition5"].ToString().Equals("1"))
                    {
                        ShowMessage("E203");
                        return false;
                    }
                    else if (dt.Rows[0]["ClosePosition5"].ToString().Equals("2"))
                    {
                        ShowMessage("E194");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        public string LeftB(string s, int maxByteCount)
        {
            Encoding encoding = Encoding.GetEncoding("Shift_JIS");
            var bytes = encoding.GetBytes(s);
            if (bytes.Length <= maxByteCount) return s;

            string result = s.Substring(0,
                encoding.GetString(bytes, 0, maxByteCount).Length);

            while (encoding.GetByteCount(result) > maxByteCount)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public decimal Z_Set(object Dmy)
        {
            // ***  項目チェック  （dmy="" OR dmy=NULL の時 0 をかえす)
            //decimalに変換できるか確かめる
            decimal d;
            if (Dmy == null)
                return 0;
            if (String.IsNullOrWhiteSpace(Dmy.ToString()))
                return 0;
            else if ((decimal.TryParse(Dmy.ToString(), out d)))
                return d;
            else
                return 0;
        }
        public string Z_SetStr(object Dmy)
        {
            // ***  項目チェック  （dmy="" OR dmy=NULL の時 0 をかえす)
            //decimalに変換できるか確かめる
            decimal d;
            if (Dmy == null)
                return "0";
            if (String.IsNullOrWhiteSpace(Dmy.ToString()))
                return "0";
            //else if ((int.TryParse(Dmy.ToString(), out int i)))
            //    return string.Format("{0}", i);
            else if ((decimal.TryParse(Dmy.ToString(), out d)))
            {
                decimal decimal_part = d % 1;
                if (decimal_part == 0)
                    return string.Format("{0:#,##0}", Convert.ToInt64(d));
                else
                    return d.ToString();
            }
            else
                return "0";
        }

        /// <summary>
        /// 消費税計算処理
        /// 税抜金額より税込金額を取得する
        /// 計算モード：１
        /// </summary>
        /// <param name="kingaku">税抜金額</param>
        /// <param name="ymd">YYYY/MM/DD形式,未入力時システム日付</param>
        /// <param name="taxRateFLG">0:非課税、1:通常課税、2:軽減課税</param>
        /// <returns></returns>
        public decimal GetZeikomiKingaku(decimal kingaku, int taxRateFLG, out decimal zei, string ymd = "")
        {
            decimal outKingaku = 0;
            zei = 0;
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_TAXCalculation(1, kingaku, ymd, taxRateFLG);
            if (dt.Rows.Count > 0)
            {
                zei = Z_Set(dt.Rows[0]["Kingaku1"]);
                outKingaku = zei + kingaku;
            }
            return outKingaku;
        }

        /// <summary>
        /// 消費税計算処理
        /// 税抜金額より税込金額を取得する
        /// 計算モード：１
        /// </summary>
        /// <param name="kingaku">税抜金額</param>
        /// <param name="taxRateFLG">0:非課税、1:通常課税、2:軽減課税</param>
        /// <param name="zei">OUT:消費税</param>
        /// <param name="zeiritsu">OUT:消費税率</param>
        /// <param name="ymd">YYYY/MM/DD形式,未入力時システム日付</param>
        /// <returns></returns>
        public decimal GetZeikomiKingaku(decimal kingaku, int taxRateFLG, out decimal zei, out decimal zeiritsu, string ymd = "")
        {
            decimal outKingaku = 0;
            zei = 0;
            zeiritsu = 0;
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_TAXCalculation(1, kingaku, ymd, taxRateFLG);
            if (dt.Rows.Count > 0)
            {
                zei = Z_Set(dt.Rows[0]["Kingaku1"]);
                zeiritsu = Z_Set(dt.Rows[0]["Zeiritsu"]);
                outKingaku = zei + kingaku;
            }
            return outKingaku;
        }

        /// <summary>
        /// 消費税計算処理
        /// 税込金額より税抜金額を取得する
        /// 計算モード：２
        /// </summary>
        /// <param name="kingaku"></param>
        /// <param name="ymd">YYYY/MM/DD形式,未入力時システム日付</param>
        /// <param name="taxRateFLG">0:非課税、1:通常課税、2:軽減課税</param>
        /// <returns></returns>
        public decimal GetZeinukiKingaku(decimal kingaku, int taxRateFLG, string ymd = "")
        {
            decimal outKingaku = 0;

            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_TAXCalculation(2, kingaku, ymd, taxRateFLG);

            if (dt.Rows.Count > 0)
            {
                outKingaku = Z_Set(dt.Rows[0]["Kingaku1"]);
            }

            return outKingaku;
        }

        /// <summary>
        /// 単価取得処理
        /// </summary>
        /// <param name="fue"></param>
        /// <returns></returns>
        public bool Fnc_UnitPrice(Fnc_UnitPrice_Entity fue)
        {
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_UnitPrice(fue);

            if (dt.Rows.Count > 0)
            {
                fue.ZeikomiTanka = Z_SetStr(dt.Rows[0]["ZeikomiTanka"]);
                fue.ZeinukiTanka = Z_SetStr(dt.Rows[0]["ZeinukiTanka"]);
                fue.Zeiritsu = Z_SetStr(dt.Rows[0]["Zeiritsu"]);
                fue.Zei = Z_SetStr(dt.Rows[0]["Zei"]);
                fue.GenkaTanka = Z_SetStr(dt.Rows[0]["GenkaTanka"]);
                fue.Bikou = dt.Rows[0]["Bikou"].ToString();

                return true;
            }
            else
            {
                fue.ZeikomiTanka = "0";
                fue.ZeinukiTanka = "0";
                fue.Zeiritsu = "0";
                fue.Zei = "0";
                fue.GenkaTanka = "0";
                fue.Bikou = "";
                return false;
            }
        }

        public bool Fnc_Reserve(Fnc_Reserve_Entity fre)
        {
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_Reserve(fre);

            if (dt.Rows.Count > 0)
            {
                fre.Result = dt.Rows[0]["Result"].ToString();
                fre.Error = dt.Rows[0]["Error"].ToString();
                fre.LastDay = dt.Rows[0]["LastDay"].ToString();
                fre.OutKariHikiateNo = dt.Rows[0]["OutKariHikiateNo"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }
        public string Fnc_PlanDate(Fnc_PlanDate_Entity fpe)
        {
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_PlanDate(fpe);

            if (dt.Rows.Count > 0)
            {
                fpe.Yoteibi = dt.Rows[0]["Yoteibi"].ToString();

                return fpe.Yoteibi;
            }
            else
            {
                return "";
            }
        }

        public bool Fnc_Present(Fnc_Present_Entity fpe)
        {
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_Present(fpe);

            if (dt.Rows.Count > 0)
            {
                fpe.outPresentCD1 = dt.Rows[0]["PresentCD1"].ToString();
                fpe.outPresentCD2 = dt.Rows[0]["PresentCD2"].ToString();
                fpe.outPresentCD3 = dt.Rows[0]["PresentCD3"].ToString();
                fpe.outPresentCD4 = dt.Rows[0]["PresentCD4"].ToString();
                fpe.outPresentCD5 = dt.Rows[0]["PresentCD5"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }
        public string Fnc_SetCheckdigit(string janCount)
        {
            string outJancd ="";

            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_SetCheckdigit(janCount);

            if (dt.Rows.Count > 0)
            {
                outJancd =dt.Rows[0]["JANCD"].ToString();
            }

            return outJancd;
        }
        public DataTable SimpleSelect1(string checkType = null, string changeDate = null, string param1 = null, string param2 = null, string param3 = null)
        {
            Base_DL bdl = new Base_DL();
            string sp = "Simple_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@ChangeDate", new ValuePair {value1 = SqlDbType.VarChar, value2 = changeDate==null? DBNull.Value.ToString(): changeDate  } },
                    { "@CD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = param1==null? DBNull.Value.ToString(): param1 } },
                    { "@CD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = param2==null? DBNull.Value.ToString(): param2 } },
                    { "@CD3", new ValuePair { value1 = SqlDbType.VarChar, value2 =  param3==null? DBNull.Value.ToString(): param3 } },
                    { "@CheckType", new ValuePair { value1 = SqlDbType.VarChar, value2 =  checkType==null? DBNull.Value.ToString(): checkType } },
                };

            return bdl.SelectData(dic, sp);
        }
        public DataTable SimpleSelect2(string checkType = null, string changeDate = null, string param1 = null, string param2 = null, string param3 = null)
        {
            Base_DL bdl = new Base_DL();
            string sp = "Simple_Select";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@ChangeDate", new ValuePair {value1 = SqlDbType.VarChar, value2 = changeDate==null? DBNull.Value.ToString(): changeDate  } },
                    { "@CD1", new ValuePair { value1 = SqlDbType.VarChar, value2 = param1==null? DBNull.Value.ToString(): param1 } },
                    { "@CD2", new ValuePair { value1 = SqlDbType.VarChar, value2 = param2==null? DBNull.Value.ToString(): param2 } },
                    { "@CD3", new ValuePair { value1 = SqlDbType.VarChar, value2 =  param3==null? DBNull.Value.ToString(): param3 } },
                    { "@CheckType", new ValuePair { value1 = SqlDbType.VarChar, value2 =  checkType==null? DBNull.Value.ToString(): checkType } },
                };

            return bdl.SelectData(dic, sp);
        }
        public bool Fnc_Credit(Fnc_Credit_Entity fce)
        {
            Function_DL fdl = new Function_DL();
            DataTable dt = fdl.Fnc_Credit(fce);

            if (dt.Rows.Count > 0)
            {
                fce.CreditCheckKBN = dt.Rows[0]["CreditCheckKBN"].ToString();
                fce.CreditMessage = dt.Rows[0]["CreditMessage"].ToString();
                fce.SaikenGaku = dt.Rows[0]["SaikenGaku"].ToString();
                fce.CreditAmount = dt.Rows[0]["CreditAmount"].ToString();

                return true;
            }
            else
            {
                return false;
            }
        }

        //public string GetConnectionString()
        //{
        //    Base_DL dl = new Base_DL();
        //    return dl.GetConnectionString();
        //}
    }
}

