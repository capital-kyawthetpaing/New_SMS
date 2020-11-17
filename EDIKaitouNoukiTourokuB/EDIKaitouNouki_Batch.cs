using System;
using System.Linq;
using System.Text;
using Entity;
using System.Data;
using System.IO;
using BL;
using System.Collections;

using Microsoft.VisualBasic.FileIO;

namespace EDIKaitouNoukiTouroku
{
    public class EDIKaitouNouki_Batch
    {
        static EDIKaitouNoukiBatch_BL ediAPI_bl = new EDIKaitouNoukiBatch_BL();
        string folderPath = string.Empty;
        const int COL_COUNT = 29;
        D_EDI_Entity dee = new D_EDI_Entity();
        //string OperatorCD = string.Empty;

        //public async void Import(M_MultiPorpose_Entity mmpe)
        public  void Import(M_MultiPorpose_Entity mmpe)
        {
            folderPath = mmpe.Char1;

            ArrayList FilePath = getFilePath();

            //int i = 0;
            foreach (string s in FilePath)
            {
                dee = new D_EDI_Entity();
                dee.UpdateOperator = "Batch";
                dee.ImportFile = System.IO.Path.GetFileName(s);
                dee.VendorCD = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(s));
                //await Insert(s);
                InsertData(s);
                //i++;
            }
        }

        //public async Task<bool> Insert(string filepathName)
        //{
        //    Func<bool> function = new Func<bool>(() => InsertData(filepathName));
        //    return await Task.Factory.StartNew<bool>(function);
        //}

        public bool InsertData(string filePath)
        {
            try
            {
                //string filePath = dgv1.Rows[i].Cells["colFileName"].Value.ToString();
                string ext = Path.GetExtension(filePath);

                DataTable dtImport = new DataTable();


                if (ext.Equals(".csv"))
                {
                    dtImport = CSVToTable(filePath, dee);
                }

                DataTable dt = new DataTable();
                Para_Add(dt);

                SetData(dee, dtImport, dt);

                // CSVファイル１つごとに、
                //テーブル転送仕様Ａ

                //エラーの有無に関わらず
                //テーブル転送仕様Ｂ
                ediAPI_bl.D_EDI_Insert(dee, dt);

                //tmze.dt1 = dtImport;
                //tmze.ImportFileName = Path.GetFileName(filePath);

                M_MultiPorpose_Entity mmpe = new M_MultiPorpose_Entity();
                mmpe.ID = MultiPorpose_BL.ID_EDI;
                mmpe.Key = "1";
                dt = ediAPI_bl.M_MultiPorpose_SelectID(mmpe);
                
                string destination =  dt.Rows[0]["Char2"].ToString() + @"\";
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

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
        /// <summary>get file path
        /// 
        /// </summary>
        /// <returns></returns>
        private ArrayList getFilePath()
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

    public DataTable CSVToTable(string filePath, D_EDI_Entity dee)
        {
            DataTable csvData = new DataTable();
            int count = 1;

            string firstColHeader = "2";   //Todo:何を設定するかは後で
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath, Encoding.GetEncoding(932), true))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    char c = 'A';

                    int errNo = 0;

                    //CSVファイルの項目数チェック
                    if (colFields.Length != COL_COUNT)
                    {
                        errNo = 1;

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

                    ErrCheck(colFields, ref errNo);

                    //最終項目にエラー番号をセット
                    Array.Resize(ref colFields, colFields.Length + 1);
                    colFields[colFields.Length - 1] = errNo.ToString();

                    //１行目の一番左の項目が”B”でなければ１行目は無視する
                    if (!colFields[0].Equals("B"))
                    {
                        firstColHeader = "1";
                    }
                    else
                    {
                        dee.OrderDetailsSu++;
                    }

                    foreach (string column in colFields)
                    {
                        if (firstColHeader.Equals("1")) //first row is column name
                        {
                            if (!csvData.Columns.Contains(column))
                            {
                                DataColumn datacolumn = new DataColumn(column);
                                datacolumn.AllowDBNull = true;
                                csvData.Columns.Add(datacolumn);
                            }
                            else
                            {
                                DataColumn datacolumn = new DataColumn(column + "_" + count++);
                                datacolumn.AllowDBNull = true;
                                csvData.Columns.Add(datacolumn);
                            }
                        }
                        else//2
                        {
                            DataColumn datacolumn = new DataColumn(c++.ToString());
                            datacolumn.AllowDBNull = true;
                            csvData.Columns.Add(datacolumn);
                        }
                    }

                    if (firstColHeader.Equals("2")) // first row is data
                    {
                        csvData.Rows.Add(colFields);//add first row as data row

                        if (errNo == 0)
                        {
                            dee.ImportDetailsSu++;
                        }
                        else
                        {
                            dee.ErrorSu++;
                        }
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        dee.OrderDetailsSu = dee.OrderDetailsSu + 1;

                        //CSVファイルの項目数チェック
                        if (fieldData.Length != COL_COUNT)
                            errNo = 1;


                        while (fieldData.Count() > COL_COUNT)
                        {
                            fieldData = fieldData.Take(fieldData.Count() - 1).ToArray();
                        }

                        while (fieldData.Count() < COL_COUNT)
                        {
                            Array.Resize(ref fieldData, fieldData.Length + 1);
                            fieldData[fieldData.Length - 1] = null;
                        }

                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }

                        ErrCheck(fieldData, ref errNo);

                        //最終項目にエラー番号をセット
                        Array.Resize(ref fieldData, fieldData.Length + 1);
                        fieldData[fieldData.Length - 1] = errNo.ToString();

                        csvData.Rows.Add(fieldData);

                        if(errNo == 0)
                        {
                            dee.ImportDetailsSu++;
                        }
                        else
                        {
                            dee.ErrorSu++; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
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

            D_Order_Entity doe = new D_Order_Entity();

            //・発注先の確認
            // 以下のSelectで発注番号の存在を確認する。
            //Selectできない場合、エラー番号＝２とする
            ret = CheckOrder(doe, colFields);
            if (!ret)
            {
                errNo = 2;
                return;
            }
            //・発注番号の有無
            //以下のSelectで発注番号の存在を確認する。
            //Selectできない場合、エラー番号＝３とする
            ret = CheckOrder(doe, colFields, 1);
            if (!ret)
            {  errNo = 3;
                return;
            }
            //・発注時の情報との差異
            //発注データとEDIデータのJANCDが等しいか？
            //D_OrderDetails.JanCD≠	ＣＳＶ JANCD
            //or D_OrderDetails.OrderSu≠ＣＳＶ 発注数
            //の場合、エラー番号＝４とする
            ret = CheckOrder(doe, colFields, 2);
            if (!ret)
            {   errNo = 4;
                return;
            }
            //・日付チェック
            //ＣＳＶ．納品日≠Null の場合、ＣＳＶ．納品日 が日付として正しくなければエラー
            //エラー番号＝５とする
            if (!string.IsNullOrEmpty(colFields[14]))
            {
                ret = ediAPI_bl.CheckDate(ediAPI_bl.FormatDate(colFields[14]));
                if (!ret)
                {       errNo = 5;
                    return;
                }
            }

            //・数量チェック
            //D_OrderDetails.OrderSu ＜	ＣＳＶ 予備２（引当数）							であればエラー
            //エラー番号＝６とする
            if (ediAPI_bl.Z_Set(doe.OrderSu) < ediAPI_bl.Z_Set(colFields[29]))
            {
                errNo = 6;
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
                if (colFields[0].Equals("B") && colFields[1].Equals("01"))
                    return true;

                else if (colFields[0].Equals("B") && colFields[1].Equals("1"))
                    return true;

                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 発注先の確認
        /// </summary>
        /// <param name="colFields"></param>
        /// <returns></returns>
        private bool CheckOrder(D_Order_Entity doe, string[] colFields, int kbn = 0)
        {
            try
            {
                
                //ＣＳＶ商品略名の左11byte							
                doe.OrderNO = colFields[25].Substring(0,11);
                doe.OrderCD = dee.VendorCD;

                if(kbn == 1 || kbn == 2)
                {
                    //ＣＳＶ商品略名の左13byte～15byte（文字型から数字型に変更必要）	
                    doe.OrderRows = colFields[25].Substring(12, 3);
                    if(ediAPI_bl.Z_Set( doe.OrderRows)==0)
                    {
                        return false;
                    }
                }

                //【D_Order_Select】
                DataTable dt = ediAPI_bl.D_Order_Select(doe);

                if (dt.Rows.Count == 0)
                {
                    return false;
                }

                if (kbn == 2)
                {
                    doe.OrderSu = dt.Rows[0]["OrderSu"].ToString();

                    //D_OrderDetails.JanCD≠	ＣＳＶ JANCD
                    //or D_OrderDetails.OrderSu≠ＣＳＶ 発注数
                    if (!dt.Rows[0]["JanCD"].ToString().Equals(colFields[26]))
                    {
                        return false;
                    }
                    if (ediAPI_bl.Z_Set(dt.Rows[0]["OrderSu"]) != ediAPI_bl.Z_Set(colFields[27]))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("EDIImportRows", typeof(int));  //取込行番号 
            dt.Columns.Add("OrderNO", typeof(string)); //発注番号
            dt.Columns.Add("OrderRows", typeof(int));    //発注明細連番
            dt.Columns.Add("ArrivalPlanDate", typeof(DateTime)); //入荷予定日
            dt.Columns.Add("ArrivalPlanMonth", typeof(int));     //入荷予定月
            dt.Columns.Add("ArrivalPlanCD", typeof(string)); // (4)   //入荷予定状況CD
            dt.Columns.Add("ArrivalPlanSu", typeof(int));   //入荷予定数
            dt.Columns.Add("VendorComment", typeof(string)); // (30)  //仕入先コメント
            dt.Columns.Add("ErrorKBN", typeof(int));   //エラー区分
            dt.Columns.Add("ErrorText", typeof(string)); //エラー内容
            dt.Columns.Add("EDIOrderNO", typeof(string));  //EDI発注番号
            dt.Columns.Add("EDIOrderRows", typeof(int));   //行番号
            dt.Columns.Add("CSVRecordKBN", typeof(string)); // (1)   //レコード区分
            dt.Columns.Add("CSVDataKBN", typeof(string));  //データ区分
            dt.Columns.Add("CSVCapitalCD", typeof(string)); // (13)  //発注者会社部署CD
            dt.Columns.Add("CSVCapitalName", typeof(string));  //発注者企業部署名
            dt.Columns.Add("CSVOrderCD", typeof(string));  //受注者会社部署CD
            dt.Columns.Add("CSVOrderName", typeof(string)); // (20)  //受注者企業部署名
            dt.Columns.Add("CSVSalesCD", typeof(string)); //販売店会社部署CD
            dt.Columns.Add("CSVSalesName", typeof(string)); // (20)  //販売店会社部署名
            dt.Columns.Add("CSVDestinationCD", typeof(string));  //出荷先会社部署CD
            dt.Columns.Add("CSVDestinationName", typeof(string));  //出荷先会社部署名
            dt.Columns.Add("CSVOrderNO", typeof(string));  //発注NO
            dt.Columns.Add("CSVOrderRows", typeof(string)); // (5)   //発注NO行
            dt.Columns.Add("CSVOrderLines", typeof(string)); // (5)   //発注NO列
            dt.Columns.Add("CSVOrderDate", typeof(string)); // (15)  //発注日
            dt.Columns.Add("CSVArriveDate", typeof(string)); // (15)  //納品日
            dt.Columns.Add("CSVOrderKBN", typeof(string)); //発注区分
            dt.Columns.Add("CSVMakerItemKBN", typeof(string));  //発注者商品分類
            dt.Columns.Add("CSVMakerItem", typeof(string)); // (50)  //発注者商品CD
            dt.Columns.Add("CSVSKUCD", typeof(string));  //受注者品番
            dt.Columns.Add("CSVSizeName", typeof(string)); //メーカー規格1
            dt.Columns.Add("CSVColorName", typeof(string)); // (20)  //メーカー規格2
            dt.Columns.Add("CSVTaniCD", typeof(string));   //単位
            dt.Columns.Add("CSVOrderUnitPrice", typeof(string));  //取引単価
            dt.Columns.Add("CSVOrderPriceWithoutTax", typeof(string)); //標準上代
            dt.Columns.Add("CSVBrandName", typeof(string));   //ブランド略名
            dt.Columns.Add("CSVSKUName", typeof(string)); //商品略名
            dt.Columns.Add("CSVJanCD", typeof(string)); //JANCD
            dt.Columns.Add("CSVOrderSu", typeof(string)); //発注数
            dt.Columns.Add("CSVOrderGroupNO", typeof(string));  //予備１（発注グループ番号）
            dt.Columns.Add("CSVAnswerSu", typeof(string));  //予備２（引当数）
            dt.Columns.Add("CSVNextDate", typeof(string));  //予備３（次回予定日）
            dt.Columns.Add("CSVOrderGroupRows", typeof(string));   //予備４（発注グループ行）
            dt.Columns.Add("CSVErrorMessage", typeof(string));  //予備５（エラーメッセージ）
        }

        private void SetData(D_EDI_Entity de, DataTable import, DataTable dt)
        {
            int rowNo = 1;

            foreach (DataRow row in import.Rows)
            {
                int errKbn = Convert.ToInt16(row[33]);
                string errText = GetErrText(errKbn);
                string arrivalPlanDate = ediAPI_bl.FormatDate( row[14].ToString());
                bool ret = ediAPI_bl.CheckDate(arrivalPlanDate);
                if (!ret)
                    arrivalPlanDate = "";

                dt.Rows.Add(rowNo      //EDIImportRows", typeof(int));  //取込行番号 
                , row[25].ToString().Substring(0, 11)    //OrderNO", typeof(string)); //発注番号
                , ediAPI_bl.Z_Set(row[25].ToString().Substring(12, 3))   //OrderRows", typeof(int));    //発注明細連番
                , arrivalPlanDate == "" ? null : arrivalPlanDate   //ArrivalPlanDate", typeof(DateTime)); //入荷予定日
                , null   //ArrivalPlanMonth", typeof(int));     //入荷予定月
                , null   //ArrivalPlanCD", typeof(string)); // (4)   //入荷予定状況CD
                , ediAPI_bl.Z_Set(row[29])   //ArrivalPlanSu", typeof(int));   //入荷予定数
                , row[32].ToString() == "" ? null : row[32].ToString()   //VendorComment", typeof(string)); // (30)  //仕入先コメント
                , errKbn　   //ErrorKBN", typeof(int));   //エラー区分
                , errText == "" ? null : errText  //ErrorText", typeof(string)); //エラー内容
                , row[28].ToString()   //EDIOrderNO", typeof(string));  //EDI発注番号
                , ediAPI_bl.Z_Set(row[31])   //EDIOrderRows", typeof(int));   //行番号
                , row[0].ToString() == "" ? null : row[0].ToString()   //CSVRecordKBN", typeof(string)); // (1)   //レコード区分
                , row[1].ToString() == "" ? null : row[1].ToString()   //CSVDataKBN", typeof(string));  //データ区分
                , row[2].ToString() == "" ? null : row[2].ToString()   //CSVCapitalCD", typeof(string)); // (13)  //発注者会社部署CD
                , row[3].ToString() == "" ? null : row[3].ToString()   //CSVCapitalName", typeof(string));  //発注者企業部署名
                , row[4].ToString() == "" ? null : row[4].ToString()   //CSVOrderCD", typeof(string));  //受注者会社部署CD
                , row[5].ToString() == "" ? null : row[5].ToString()   //CSVOrderName", typeof(string)); // (20)  //受注者企業部署名
                , row[6].ToString() == "" ? null : row[6].ToString()    //CSVSalesCD", typeof(string)); //販売店会社部署CD
                , row[7].ToString() == "" ? null : row[7].ToString()   //CSVSalesName", typeof(string)); // (20)  //販売店会社部署名
                , row[8].ToString() == "" ? null : row[8].ToString()   //CSVDestinationCD", typeof(string));  //出荷先会社部署CD
                , row[9].ToString() == "" ? null : row[9].ToString()   //CSVDestinationName", typeof(string));  //出荷先会社部署名
                , row[10].ToString() == "" ? null : row[10].ToString()   //CSVOrderNO", typeof(string));  //発注NO
                , row[11].ToString() == "" ? null : row[11].ToString()   //CSVOrderRows", typeof(string)); // (5)   //発注NO行
                , row[12].ToString() == "" ? null : row[12].ToString()   //CSVOrderLines", typeof(string)); // (5)   //発注NO列
                , row[13].ToString() == "" ? null : row[13].ToString()   //CSVOrderDate", typeof(string)); // (15)  //発注日
                , row[14].ToString() == "" ? null : row[14].ToString()   //CSVArriveDate", typeof(string)); // (15)  //納品日
                , row[15].ToString() == "" ? null : row[15].ToString()   //CSVOrderKBN", typeof(string)); //発注区分
                , row[16].ToString() == "" ? null : row[16].ToString()    //CSVMakerItemKBN", typeof(string));  //発注者商品分類
                , row[17].ToString() == "" ? null : row[17].ToString()   //CSVMakerItem", typeof(string)); // (50)  //発注者商品CD
                , row[18].ToString() == "" ? null : row[18].ToString()   //CSVSKUCD", typeof(string));  //受注者品番
                , row[19].ToString() == "" ? null : row[19].ToString()   //CSVSizeName", typeof(string)); //メーカー規格1
                , row[20].ToString() == "" ? null : row[20].ToString()   //CSVColorName", typeof(string)); // (20)  //メーカー規格2
                , row[21].ToString() == "" ? null : row[21].ToString()   //CSVTaniCD", typeof(string));   //単位
                , row[22].ToString() == "" ? null : row[22].ToString()   //CSVOrderUnitPrice", typeof(string));  //取引単価
                , row[23].ToString() == "" ? null : row[23].ToString()   //CSVOrderPriceWithoutTax", typeof(string)); //標準上代
                , row[24].ToString() == "" ? null : row[24].ToString()   //CSVBrandName", typeof(string));   //ブランド略名
                , row[25].ToString() == "" ? null : row[25].ToString()   //CSVSKUName", typeof(string)); //商品略名
                , row[26].ToString() == "" ? null : row[26].ToString()   //CSVJanCD", typeof(string)); //JANCD
                , row[27].ToString() == "" ? null : row[27].ToString()   //CSVOrderSu", typeof(string)); //発注数
                , row[28].ToString() == "" ? null : row[28].ToString()   //CSVOrderGroupNO", typeof(string));  //予備１（発注グループ番号）
                , row[29].ToString() == "" ? null : row[29].ToString()   //CSVAnswerSu", typeof(string));  //予備２（引当数）
                , row[30].ToString() == "" ? null : row[30].ToString()   //CSVNextDate", typeof(string));  //予備３（次回予定日）
                , row[31].ToString() == "" ? null : row[31].ToString()   //CSVOrderGroupRows", typeof(string));   //予備４（発注グループ行）
                , row[32].ToString() == "" ? null : row[32].ToString()   //CSVErrorMessage", typeof(string));  //予備５（エラーメッセージ）
                );
                rowNo++;
            }
        }

        private string GetErrText(int errKbn)
        {
            string errText = "";

            switch (errKbn)
            {
                case 1:
                    errText = "データ区分が正しくない";
                    break;
                case 2:
                    errText = "発注先が正しくない";
                    break;
                case 3:
                    errText = "発注番号・明細が存在しない";
                    break;
                case 4:
                    errText = "商品・数量が発注情報と異なっている";
                    break;
                case 5:
                    errText = "入荷予定日が正しくない";
                    break;
                case 6:
                    errText = "発注数より入荷予定数が多い";
                    break;

            }

            return errText;
        }
    }
}

