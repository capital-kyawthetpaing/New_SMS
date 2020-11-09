using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.IO;

using System.Data.SqlClient;

namespace DL
{
    public class D_EdiHacchuu_DL : Base_DL
    {
        public DataTable D_Order_SelectForEDIHacchuu(string orderNo)
        {
            string sp = "D_Order_SelectForEDIHacchuu";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = orderNo } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_EDIOrder_Select(D_EDIOrder_Entity de)
        {
            string sp = "D_EDIOrder_Select";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@EDIOrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.EDIOrderNO } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_EDIOrder_SelectAll(D_EDIOrder_Entity de)
        {
            string sp = "D_EDIOrder_SelectAll";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.StoreCD } },
                { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.OrderDateFrom } },
                { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.OrderDateTo } },
                { "@VendorCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.VendorCD } },
            };

            return SelectData(dic, sp);
        }

        public DataTable D_EDIOrder_SelectForCSV(D_EDIOrder_Entity de)
        {
            string sp = "D_EDIOrder_SelectForCSV";

            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@EDIOrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = de.EDIOrderNO } },
            };

            return SelectData(dic, sp);
        }

        /// <summary>
        /// 未出力分の処理
        /// </summary>
        /// <param name="doe"></param>
        /// <param name="dee"></param>
        /// <returns></returns>
         public bool PRC_EDIOrder_Insert(D_Order_Entity doe, D_EDIOrder_Entity dee)
        {
            DateTime dtNow = DateTime.Now;
           
            try
            {

                StartTransaction();

                //EDI発注追加
                Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@StoreCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StoreCD } },
                    { "@OrderDateFrom", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateFrom } },
                    { "@OrderDateTo", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderDateTo } },
                    { "@StaffCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.StaffCD } },
                    { "@OrderCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderCD } },
                    { "@OrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.OrderNO } },
                    { "@ChkMisyonin", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.ChkMisyonin.ToString() } },
                    { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.Operator } },
                    { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = doe.PC } },
                    { "@SYSDATETIME", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtNow.ToString("yyyy/MM/dd HH:mm:ss") } },
                };

                command = new SqlCommand("PRC_EDIOrder_Insert", GetConnection(), transaction);
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                command.ExecuteNonQuery();

                //CSV出力
                dic = new Dictionary<string, ValuePair>
                {
                    { "@EDIOrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.EDIOrderNO } },
                };

                DataTable dt = new DataTable();
                command = new SqlCommand("D_EDIOrder_SelectForCSV", GetConnection(), transaction);
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                ExportCsv(dt,dtNow);

                //CSV出力日時更新
                if (dee.EDIOrderNO == null)
                {
                    dic = new Dictionary<string, ValuePair>
                    {
                        { "@SYSDATETIME", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtNow.ToString("yyyy/MM/dd HH:mm:ss") } },
                    };

                    command = new SqlCommand("D_EDIOrder_UpdateOutputDateTime", GetConnection(), transaction);
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, ValuePair> pair in dic)
                    {
                        ValuePair vp = pair.Value;
                        AddParam(command, pair.Key, vp.value1, vp.value2);
                    }
                    command.ExecuteNonQuery();
                }

                CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                RollBackTransaction();
                throw e;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        /// <summary>
        /// 再出力分の処理
        /// </summary>
        /// <param name="dee"></param>
        /// <returns></returns>
        public bool PRC_EDIOrder_MailInsert(D_EDIOrder_Entity dee)
        {
            DateTime dtNow = DateTime.Now;

            try
            {

                StartTransaction();

                Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
                {
                    { "@EDIOrderNo", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.EDIOrderNO } },                    
                    { "@Operator", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.Operator } },
                    { "@PC", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.PC } },
                    { "@SYSDATETIME", new ValuePair { value1 = SqlDbType.VarChar, value2 = dtNow.ToString("yyyy/MM/dd HH:mm:ss") } },
                };

                command = new SqlCommand("PRC_EDIOrder_MailInsert", GetConnection(), transaction);
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                command.ExecuteNonQuery();

                //CSV出力
                dic = new Dictionary<string, ValuePair>
                {
                    { "@EDIOrderNO", new ValuePair { value1 = SqlDbType.VarChar, value2 = dee.EDIOrderNO } },
                };

                DataTable dt = new DataTable();
                command = new SqlCommand("D_EDIOrder_SelectForCSV", GetConnection(), transaction);
                command.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, ValuePair> pair in dic)
                {
                    ValuePair vp = pair.Value;
                    AddParam(command, pair.Key, vp.value1, vp.value2);
                }
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);

                ExportCsv(dt, dtNow);

                CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                RollBackTransaction();
                throw e;
            }
            finally
            {
                command.Connection.Close();
            }

        }

        /// <summary>
        /// CSV出力
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtNow"></param>
        /// <returns></returns>
        private bool ExportCsv(DataTable dt, DateTime dtNow)
        {
            try
            {
                //CSV出力データ取得
                if (dt.Rows.Count == 0)
                {
                    return true;
                }

                var tables = dt.Rows.Cast<DataRow>()
                            .GroupBy(a => a.Field<string>("VendorCD"))
                            .Select(a => a.CopyToDataTable())
                            .ToArray();

                for (int i = 0; i < tables.Length; i++)
                {

                    //CSV出力処理
                    //VendorFTP_BL ble = new VendorFTP_BL();
                    string strFullPath = string.Empty;
                    string strPath = string.Empty;
                    string titleFlg = "0";
                    DataTable dtFTP;

                    //出力先取得
                    M_VendorFTP_Entity mse = new M_VendorFTP_Entity
                    {
                        VendorCD = tables[i].Rows[0]["VendorCD"].ToString(),
                        ChangeDate = dtNow.Date.ToString()
                    };

                    M_VendorFTP_DL vl = new M_VendorFTP_DL();
                    dtFTP = vl.M_VendorFTP_Select(mse);                    
                    if (dtFTP.Rows.Count == 0)
                    {
                        continue;
                    }

                    //サーバ名
                    strPath = dtFTP.Rows[0]["CreateServer"].ToString();
                    if (!strPath.EndsWith(@"\"))
                    {
                        strPath += @"\";
                    }

                    //フォルダ名
                    strPath += dtFTP.Rows[0]["CreateFolder"].ToString();
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    if (!strPath.EndsWith(@"\"))
                    {
                        strPath += @"\";
                    }
                    strFullPath = strPath + Path.GetFileNameWithoutExtension(dtFTP.Rows[0]["FileName"].ToString()) + "_" + dtNow.ToString("yyyyMMddHHmmss") + ".csv";
                    titleFlg = dtFTP.Rows[0]["TitleFLG"].ToString();

                    //CSV出力
                    //CSVファイルに書き込むときに使うEncoding
                    System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                    using (StreamWriter sw = new StreamWriter(strFullPath, false, enc))
                    {

                        int colCount = tables[i].Columns.Count;
                        int lastColIndex = colCount - 2;

                        //ヘッダを書き込む
                        if (titleFlg == "1")
                        {

                            string field = "レコード区分,データ区分,発注者会社部署CD,発注者企業部署名" +
                                ",受注者会社部署CD,受注者企業部署名,販売店会社部署CD,販売店会社部署名" +
                                ",出荷先会社部署CD,出荷先会社部署名,発注NO,発注NO行,発注NO列,発注日" +
                                ",納品日,発注区分,発注者商品分類,発注者商品CD,受注者品番" +
                                ",メーカー規格1,メーカー規格2,単位,取引単価,標準上代,ブランド略名,商品略名,JANCD" +
                                ",発注数,発注グループ番号,引当数,次回予定日,発注グループ行,エラーメッセージ";

                            sw.Write(field);
                            sw.Write("\r\n");
                        }

                        //レコードを書き込む
                        foreach (DataRow row in tables[i].Rows)
                        {
                            //最後の項目は書き込まない
                            for (int count = 0; count < colCount - 1; count++)
                            {
                                //書き込み
                                switch (count)
                                {
                                    case 10:
                                        string field = EncloseDoubleQuotesIfNeed(row[count].ToString().PadRight(11,' ').Substring(1,10));
                                        sw.Write(field);
                                        break;

                                    case 11:
                                    case 12:
                                    case 22:
                                    case 23:
                                    case 27:
                                    case 29:
                                    case 31:
                                        sw.Write(row[count]);
                                        break;

                                    case 13:
                                        field = EncloseDoubleQuotesIfNeed(row[count].ToString().Replace("/",""));
                                        sw.Write(field);
                                        break;

                                    default:
                                        field = EncloseDoubleQuotesIfNeed(row[count].ToString());
                                        sw.Write(field);
                                        break;
                                }                               
                                
                                //カンマを書き込む
                                if (lastColIndex > count)
                                {
                                    sw.Write(',');
                                }
                            }
                            //改行する
                            sw.Write("\r\n");
                        }
                    }
                }

                return true;
            }

            catch (Exception ex)
            {               
                throw;
            }

        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

    }
}
