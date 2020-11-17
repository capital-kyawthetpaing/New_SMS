using Entity;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace EDINouhinJouhonTourokuB
{
    public class EDINouhinJouhou_Batch
    {
        D_SKENDelivery_Entity dskend = new D_SKENDelivery_Entity();
        EDINouhinJouhon_Batch_BL edij_bl = new EDINouhinJouhon_Batch_BL();
        string mainfolderPath = string.Empty;
        string bakfolderPath = string.Empty;
        string firstColHeader = string.Empty;
        static DataTable dtImportB, dtImportD;
        static DataTable dtImport = new DataTable();

        const int COL_COUNT = 36;
        public void Import(M_MultiPorpose_Entity mmpe)
        {
            mainfolderPath = mmpe.Char1;
            bakfolderPath = mmpe.Char2;

            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(mainfolderPath);
            DirectoryInfo[] diArr = di.GetDirectories();

            //int i = 0;
            foreach(DirectoryInfo dr in diArr)
            {
                dtImportB = dtImportD = null;
                ArrayList FilePath = getFilePath(mainfolderPath +@"\"+ dr.ToString());
                dskend.VendorCD = dr.ToString();

                foreach (string s in FilePath)
                {
                    dskend.UpdateOperator = "Batch";
                    dskend.ImportFile = System.IO.Path.GetFileName(s);
                    //dskend.VendorCD = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(s));
                    PrepareImportData(s);

                }
                if(dtImportB!=null && dtImportD!=null)
                {
                    dskend.dt1 = dtImportB;
                    dskend.dt2 = dtImportD;
                    if(edij_bl.SKEN_InsertData(dskend))
                    {
                        foreach (string s in FilePath)
                        {
                            MoveBakFile(mainfolderPath + @"\" + dr.ToString() + @"\" + System.IO.Path.GetFileName(s));
                        }
                    }
                }
                   
            }
           
        }

        private ArrayList getFilePath(string folderPath)
        {
            ArrayList arr = new ArrayList();

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            DirectoryInfo d = new DirectoryInfo(folderPath);
            string[] Files;

            Files = Directory.GetFiles(folderPath, "*.*", System.IO.SearchOption.AllDirectories).Where(s => s.EndsWith(".csv")).ToArray();

            foreach (string file in Files)
            {
                //このとき、フォルダー名＝仕入先CDと見なす
                arr.Add(file);
            }

            return arr;
        }

        public void PrepareImportData(string filePath)
        {
            try
            {
                string ext = Path.GetExtension(filePath);
                //dtImportB = new DataTable();
                //dtImportD = new DataTable();

                String[] colNameB = { "レコード区分", "データ区分", "取引先会社部署CD", "取引先企業部署名", "納品元会社部署CD", "納品元企業部署名", "販売店会社部署CD", "販売店会社部署名", "出荷先会社部署CD", "出荷先会社部署名", "納品書NO", "伝票区分", "受注日", "出荷日", "納品/返品伝票日", "発注NO", "発注区分", "伝票表示a", "伝票表示b", "伝票表示c", "伝票表示d", "運送方法", "個数", "運賃区分", "諸掛", "運賃", "品代合計", "消費税", "総合計", "メーカー伝票NO", "元伝NO", "予備1", "予備2", "予備3", "予備4", "予備5" };
                String[] colNameD = { "レコード区分", "データ区分", "取引先会社部署CD", "取引先企業部署名", "納品元会社部署CD", "納品元企業部署名", "販売店会社部署CD", "販売店会社部署名", "出荷先会社部署CD", "出荷先会社部署名", "納品書NO", "納品書NO行", "納品書NO列", "伝票区分", "受注日", "出荷日", "納品/返品伝票日", "発注NO", "発注区分", "発注者商品CD", "納品元品番", "メーカー規格1", "メーカー規格2", "単位", "取引単価", "標準上代", "ブランド略名", "商品略名", "JANCD", "納品数", "メーカー伝票NO", "元伝NO","予備1", "予備2", "予備3", "予備4", "予備5" };
                dtImport = null;


                if (ext.Equals(".csv"))
                {
                    dtImport = CSVToTable(filePath);
                }

                
                if (dtImport.Rows.Count>0)
                {
                    if (dtImport.Rows[0]["A"].ToString() == "B" && dtImport.Rows[0]["B"].ToString() == "21")
                    {
                        //if (CheckColName(colNameB, dtImport))
                        //{
                            Para_Add(dtImportB);
                            dtImportB = ChangeColName(dtImport,"B");
                            
                        //}
                    }
                    else if(dtImport.Rows[0]["A"].ToString() == "D" && dtImport.Rows[0]["B"].ToString() == "21")
                    {
                        //if (CheckColName(colNameD, dtImport))
                        //{
                            dtImportD = ChangeColName(dtImport,"D");
                        //}
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        public void MoveBakFile(string filePath)
        {
            string destination = bakfolderPath + @"\";
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            //読み取ったCSVファイルを汎用マスター.文字型２で設定されたドライブ、フォルダーに保存する。（元のフォルダーからは削除する）		
            if (File.Exists(destination + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath)) + @"\" + Path.GetFileName(filePath)))
                File.Delete(destination + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath)) + @"\" + Path.GetFileName(filePath));

            //仕入先ごとのサブフォルダーの構造は元のフォルダーと同じにする。
            //（サブフォルダーが無い場合   、作成する）
            if (!Directory.Exists(destination + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath))))
            {
                Directory.CreateDirectory(destination + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath)));
            }

            File.Move(filePath, destination + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath)) + @"\" + Path.GetFileName(filePath));

        }
        public DataTable CSVToTable(string filePath)
        {
            DataTable csvData = new DataTable();
            string firstColHeader = "2";
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath, Encoding.GetEncoding(932)))
                {
                    csvReader.SetDelimiters(new string[] { "," });

                    csvReader.HasFieldsEnclosedInQuotes = true;

                    string[] colFields = csvReader.ReadFields();
                    char c = 'A';


                    if (colFields.Length != COL_COUNT)
                    {

                        while (colFields.Count() > COL_COUNT)
                        {
                            colFields = colFields.Take(colFields.Count() - 1).ToArray();
                        }

                        while (colFields.Count() < COL_COUNT)
                        {
                            Array.Resize(ref colFields, colFields.Length + 1);
                            colFields[colFields.Length - 1] = null;
                        }
                    }

                    foreach (string column in colFields)
                    {

                        DataColumn datacolumn = new DataColumn(c++.ToString());
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                    }
                    if (firstColHeader.Equals("2")) // first row is data
                    {
                        csvData.Rows.Add(colFields);//add first row as data row
                    }

                     while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        //Making empty value as null

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }

        protected Boolean CheckColName(String[] colName, DataTable dt)
        {
            DataColumnCollection col = dt.Columns;
            for (int i = 0; i < colName.Length; i++)
            {
                if (!col.Contains(colName[i]))
                    return false;
            }
            return true;

        }

        private void Para_Add(DataTable dt)
        {
            
            dt.Columns.Add("SKENRecordKBN", typeof(string));  //レコード区分 
            dt.Columns.Add("SKENDataKBN", typeof(string)); //データ区分
            dt.Columns.Add("SKENTorihikisakiCD", typeof(string));    //取引先会社部署CD
            dt.Columns.Add("SKENTorihikisakiMei", typeof(string)); //取引先企業部署名
            dt.Columns.Add("SKENNouhinmotoCD", typeof(string));     //納品元会社部署CD

            dt.Columns.Add("SKENNouhinmotoMei", typeof(string)); // //納品元企業部署名
            dt.Columns.Add("SKENHanbaitenCD", typeof(string));   //販売店会社部署CD
            dt.Columns.Add("SKENHanbaitenMei", typeof(string)); // 販売店会社部署名
            dt.Columns.Add("SKENSyukkasakiCD", typeof(string));   //出荷先会社部署CD
            dt.Columns.Add("SKENSyukkasakiMei", typeof(string)); //出荷先会社部署名

            dt.Columns.Add("SKENNouhinshoNO", typeof(string));  //納品書NO
            dt.Columns.Add("SKENDenpyouKBN", typeof(string));   //伝票区分
            dt.Columns.Add("SKENJuchuuDate", typeof(string)); // 受注日
            dt.Columns.Add("SKENSyukkaDate", typeof(string));  //出荷日
            dt.Columns.Add("SKENNouhinDate", typeof(string)); // 納品/返品伝票日

            dt.Columns.Add("SKENNouhinshoNO", typeof(string));  //発注NO
            dt.Columns.Add("SKENHacchuuKBN", typeof(string));  //発注区分
            dt.Columns.Add("SKENDenpyoua", typeof(string)); //伝票表示a
            dt.Columns.Add("SKENDenpyoub", typeof(string)); //伝票表示b
            dt.Columns.Add("SKENDenpyouc", typeof(string)); // 伝票表示c

            dt.Columns.Add("SKENDenpyoud", typeof(string));  //伝票表示d
            dt.Columns.Add("SKENUnsouHouhou", typeof(string));  //運送方法
            dt.Columns.Add("SKENKosuu", typeof(string));  //個数
            dt.Columns.Add("SKENUnchinKBN", typeof(string)); //運賃区分
            dt.Columns.Add("SKENSyogakari", typeof(string)); //諸掛

            dt.Columns.Add("SKENUnchin", typeof(string)); //運賃
            dt.Columns.Add("SKENShinadai", typeof(string)); //品代合計
            dt.Columns.Add("SKENSyouhiZei", typeof(string)); //消費税
            dt.Columns.Add("SKENSougoukei", typeof(string));  //総合計
            dt.Columns.Add("SKENMakerDenpyou", typeof(string)); // メーカー伝票NO

            dt.Columns.Add("SKENMotoDenNO", typeof(string));  //元伝NO
            dt.Columns.Add("SKENYobi1", typeof(string)); //予備1
            dt.Columns.Add("SKENYobi2", typeof(string)); // 予備2
            dt.Columns.Add("SKENYobi3", typeof(string));   //予備3
            dt.Columns.Add("SKENYobi4", typeof(string));  //予備4

            dt.Columns.Add("SKENYobi5", typeof(string)); //予備5

        }
        protected DataTable ChangeColName(DataTable dtImport,string c)
        {
            dtImport.Columns["レコード区分"].ColumnName = "SKENRecordKBN";
            dtImport.Columns["データ区分"].ColumnName = "SKENDataKBN";
            dtImport.Columns["取引先会社部署CD"].ColumnName = "SKENTorihikisakiCD";
            dtImport.Columns["取引先企業部署名"].ColumnName = "SKENTorihikisakiMei";
            dtImport.Columns["納品元会社部署CD"].ColumnName = "SKENNouhinmotoCD";

            dtImport.Columns["納品元企業部署名"].ColumnName = "SKENNouhinmotoMei";
            dtImport.Columns["販売店会社部署CD"].ColumnName = "SKENHanbaitenCD";
            dtImport.Columns["販売店会社部署名"].ColumnName = "SKENHanbaitenMei";
            dtImport.Columns["出荷先会社部署CD"].ColumnName = "SKENSyukkasakiCD";
            dtImport.Columns["出荷先会社部署名"].ColumnName = "SKENSyukkasakiMei";

            dtImport.Columns["納品書NO"].ColumnName = "SKENNouhinshoNO";
            dtImport.Columns["伝票区分"].ColumnName = "SKENDenpyouKBN";
            dtImport.Columns["受注日"].ColumnName = "SKENJuchuuDate";
            dtImport.Columns["出荷日"].ColumnName = "SKENSyukkaDate";
            dtImport.Columns["納品/返品伝票日"].ColumnName = "SKENNouhinDate";

            dtImport.Columns["発注NO"].ColumnName = "SKENHacchuu";
            dtImport.Columns["発注区分"].ColumnName = "SKENHacchuuKBN";
            dtImport.Columns["メーカー伝票NO"].ColumnName = "SKENMakerDenpyou";
            dtImport.Columns["元伝NO"].ColumnName = "SKENMotoDenNO";
            dtImport.Columns["予備1"].ColumnName = "SKENYobi1";

            dtImport.Columns["予備2"].ColumnName = "SKENYobi2";
            dtImport.Columns["予備3"].ColumnName = "SKENYobi3";
            dtImport.Columns["予備4"].ColumnName = "SKENYobi4";
            dtImport.Columns["予備5"].ColumnName = "SKENYobi5";

            if (c=="B")
            {
                dtImport.Columns["伝票表示a"].ColumnName = "SKENDenpyoua";
                dtImport.Columns["伝票表示b"].ColumnName = "SKENDenpyoub";
                dtImport.Columns["伝票表示c"].ColumnName = "SKENDenpyouc";

                dtImport.Columns["伝票表示d"].ColumnName = "SKENDenpyoud";
                dtImport.Columns["運送方法"].ColumnName = "SKENUnsouHouhou";
                dtImport.Columns["個数"].ColumnName = "SKENKosuu";
                dtImport.Columns["運賃区分"].ColumnName = "SKENUnchinKBN";
                dtImport.Columns["諸掛"].ColumnName = "SKENSyogakari";

                dtImport.Columns["運賃"].ColumnName = "SKENUnchin";
                dtImport.Columns["品代合計"].ColumnName = "SKENShinadai";
                dtImport.Columns["消費税"].ColumnName = "SKENSyouhiZei";
                dtImport.Columns["総合計"].ColumnName = "SKENSougoukei";
            }
            else
            {
                dtImport.Columns["納品書NO行"].ColumnName = "SKENNouhinshoNOGyou";
                dtImport.Columns["納品書NO列"].ColumnName = "SKENNouhinshoNORetsu";
                dtImport.Columns["発注者商品CD"].ColumnName = "SKENHacchuuShouhinCD";
                dtImport.Columns["納品元品番"].ColumnName = "SKENNouhinHinban";
                dtImport.Columns["メーカー規格1"].ColumnName = "SKENMakerKikaku1";

                dtImport.Columns["メーカー規格2"].ColumnName = "SKENMakerKikaku2";
                dtImport.Columns["単位"].ColumnName = "SKENTani";
                dtImport.Columns["取引単価"].ColumnName = "SKENTorihikiTanka";
                dtImport.Columns["標準上代"].ColumnName = "SKENHyoujyunJyoudai";
                dtImport.Columns["ブランド略名"].ColumnName = "SKENBrandmei";

                dtImport.Columns["商品略名"].ColumnName = "SKENSyouhinmei";
                dtImport.Columns["JANCD"].ColumnName = "SKENJanCD";
                dtImport.Columns["納品数"].ColumnName = "SKENNouhinSuu";

            }

            

            DataColumn dc = new DataColumn();
            dc.DefaultValue = 0;
            dc.ColumnName = "ImportDetailsSu";  //'エラーなく取り込んだ行数 
            dtImport.Columns.Add(dc);

            DataColumn dc1 = new DataColumn();
            dc1.DefaultValue = 0;
            dc1.ColumnName = "ErrorSu";     //エラーがあった行数
            dtImport.Columns.Add(dc1);

            DataColumn dc2 = new DataColumn();
            dc2.DefaultValue = 0;
            dc2.ColumnName = "ErrorKBN";     //エラー番号
            dtImport.Columns.Add(dc2);

            DataColumn dc3 = new DataColumn();
            dc3.DefaultValue = dskend.ImportFile;
            dc3.ColumnName = "ImportFile";
            dtImport.Columns.Add(dc3);

            dtImport.Columns.Add("ErrorText"); //Error Msg

            return dtImport;
        }

        private void Para_Add(DataTable dt,string ch)
        {
            dt.Columns.Add("ImportDetailsSu");  //'エラーなく取り込んだ行数 
            dt.Columns.Add("ErrorSu"); //エラーがあった行数
            dt.Columns.Add("ErrorKBN");    //エラー番号
            dt.Columns.Add("ErrorText"); //Error Msg

            dt.Columns.Add("SKENRecordKBN");     //'レコード区分
            dt.Columns.Add("SKENDataKBN"); // (4)   //データ区分
            dt.Columns.Add("SKENTorihikisakiCD");     //取引先会社部署CD
            dt.Columns.Add("SKENTorihikisakiMei"); //取引先企業部署名
            dt.Columns.Add("SKENNouhinmotoCD");     //納品元会社部署CD

            dt.Columns.Add("SKENNouhinmotoMei"); //納品元企業部署名
            dt.Columns.Add("SKENHanbaitenCD");   //販売店会社部署CD
            dt.Columns.Add("SKENHanbaitenMei");     //販売店会社部署名
            dt.Columns.Add("SKENSyukkasakiCD");  //出荷先会社部署CD
            dt.Columns.Add("SKENSyukkasakiMei");     //出荷先会社部署名

            dt.Columns.Add("SKENNouhinshoNO");  //納品書NO
            dt.Columns.Add("SKENDenpyouKBN");   //伝票区分
            dt.Columns.Add("SKENJuchuuDate");     //受注日
            dt.Columns.Add("SKENSyukkaDate");  //出荷日
            dt.Columns.Add("SKENNouhinDate");     //納品/返品伝票日

            dt.Columns.Add("SKENHacchuu"); //発注NO
            dt.Columns.Add("SKENHacchuuKBN");   //発注区分
            dt.Columns.Add("SKENDenpyoua");     //伝票表示a
            dt.Columns.Add("SKENDenpyoub");  //伝票表示b
            dt.Columns.Add("SKENDenpyouc");     //伝票表示c

            dt.Columns.Add("SKENDenpyoud"); //伝票表示d
            dt.Columns.Add("SKENUnsouHouhou");  //運送方法
            dt.Columns.Add("SKENKosuu");     //個数
            dt.Columns.Add("SKENSyogakari");  //運賃区分
            dt.Columns.Add("SKENUnchin");     //諸掛

            dt.Columns.Add("SKENShinadai");  //運賃
            dt.Columns.Add("SKENSyouhiZei");  //品代合計
            dt.Columns.Add("SKENSougoukei");    //消費税
            dt.Columns.Add("SKENMakerDenpyou");   //メーカー伝票NO
            dt.Columns.Add("SKENMotoDenNO");     //元伝NO

            dt.Columns.Add("SKENYobi1"); // (4)   //予備1
            dt.Columns.Add("SKENYobi2"); // (4)   //予備2
            dt.Columns.Add("SKENYobi3");     //予備3
            dt.Columns.Add("SKENYobi4"); // (4)   //予備4
            dt.Columns.Add("SKENYobi5");     //予備5

        }
         private void ErrCheck(string[] colFields, ref int errNo)
        {
            //CSVデータの整合性確認
            bool ret = CheckKbn(colFields);
            if (!ret)
            {
                errNo = 1;
                return;
            }
            
        }



        /// <summary>
        /// CSVデータの整合性確認
        /// </summary>
        /// <param name="colFields">１レコード</param>
        /// <returns></returns>
        private bool CheckKbn(string[] colFields)
        {
            try
            {
                //１つ目の項目：レコード区分＝B
                //２つ目の項目：データ区分＝01
                //でなければエラー番号＝１とする
                if (colFields[0].Equals("B") && colFields[1].Equals("21"))
                    return true;

                else if (colFields[0].Equals("D") && colFields[1].Equals("21"))
                    return true;

                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
