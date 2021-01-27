﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using ExcelDataReader;

namespace MasterTorikomi_Item
{
    public partial class MasterTorikomi_Item : Base.Client.FrmMainForm
    {
        Base_BL bbl;
        public MasterTorikomi_Item()
        {
            InitializeComponent();
        }
        private void MasterTorikomi_Item_Load(object sender, EventArgs e)
        {
            bbl = new Base_BL();
            InProgramID = "MasterTorikomi_Item";
            StartProgram();
            FalseKey();
            this.KeyUp += MasterTorikomi_Item_KeyUp;
            RB_all.Focus();

        }

        private void MasterTorikomi_Item_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void FalseKey()
        {
            F2Visible = F3Visible=  F4Visible= F5Visible=F7Visible=F8Visible=F9Visible=F10Visible= F11Visible= false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (bbl.ShowMessage("Q001") == DialogResult.Yes)
            //{
            string filePath = string.Empty;
            string fileExt = string.Empty;
            if (!System.IO.Directory.Exists("C:\\SMS\\MasterShutsuryoku_ITEM\\"))
            {
                System.IO.Directory.CreateDirectory("C:\\SMS\\MasterShutsuryoku_ITEM\\");
            }
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                                                        // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
            file.InitialDirectory = "C:\\SMS\\MasterShutsuryoku_ITEM\\";                             // file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; ; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                {
                    bbl.ShowMessage("E137");
                    return;
                }
                inputPath.Text = filePath;
                DataTable dt = new DataTable();
                dt = ExcelToDatatable(filePath);
                //string[] colname = { "SKUCD", "JANCD", "商品名", "カラーNO", "カラー名", "サイズNO", "サイズ名"};
                //if (ColumnCheck(colname, dtExcel))
                //{

                //}
                if (dt != null)
                {
                    if (ErrorCheck(dt))
                    {
                        //    ExcelErrorCheck(dt);
                        //    if (checkerr)
                        //    {
                        //        var xml = bbl.DataTableToXml(dt);
                        // }
                        gvItem.DataSource = null;
                        gvItem.DataSource = dt;
                        bbl.ShowMessage("I101");
                    }
                }
                else
                {
                    MessageBox.Show("No data was set to datatable");
                    return;
                }
            }
            //}

        }

        private bool ErrorCheck(DataTable dt)
        {
            string kibun = dt.Rows[0]["データ区分"].ToString();
            if (RB_all.Checked)
            {
                if (dt.Columns.Count != 116)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "1")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            else if (RB_BaseInfo.Checked)
            {
                if (dt.Columns.Count != 39)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "2")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            else if (RB_attributeinfo.Checked)
            {
                if (dt.Columns.Count != 66)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "3")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            else if (RB_priceinfo.Checked)
            {
                if (dt.Columns.Count != 20)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "4")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            else if (RB_Catloginfo.Checked)
            {
                if (dt.Columns.Count != 13)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "5")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            else if (RB_tagInfo.Checked)
            {
                if (dt.Columns.Count != 16)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "6")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            //else if (RB_JanCD.Checked)
            //{
            //    if (dt.Columns.Count != 16)
            //    {
            //        bbl.ShowMessage("E137");
            //        return false;
            //    }

            //    if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
            //    {
            //        if (kibun != "7")
            //        {
            //            bbl.ShowMessage("E137");
            //            return false;
            //        }
            //    }

            //}
            else if (RB_SizeURL.Checked)
            {
                if (dt.Columns.Count != 7)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "8")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }

            }
            return true;
        }
        
        private void ExcelErrorCheck(DataTable dt)
        {
            dt.Columns.Add("EItem");
            dt.Columns.Add("Error");
            string kibun = dt.Rows[1]["データ区分"].ToString();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Is102(dt.Rows[i]["データ区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "データ区分";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }

                if (!Is102(dt.Rows[i]["ITEMCD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "ITEMCD";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }

                if (!Is102(dt.Rows[i]["改定日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "改定日";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                if (!Is103(dt.Rows[i]["改定日"].ToString()))
                {
                   
                    dt.Rows[i]["EItem"] = "改定日";
                    dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }

                if (!Is103(dt.Rows[i]["承認日"].ToString()))
                {
                        dt.Rows[i]["EItem"] = "承認日";
                        dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }

                if (!Is190(dt.Rows[i]["削除"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "削除";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "削除");

                if (!Is102(dt.Rows[i]["諸口区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "諸口区分";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                if (!Is190(dt.Rows[i]["諸口区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "諸口区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "諸口区分");

                if (!Is102(dt.Rows[i]["商品名"].ToString()))
                {
                        dt.Rows[i]["EItem"] = "商品名";
                        dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }

                if (!Is102(dt.Rows[i]["略名"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "略名";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }

                if (!Is101("M_Vendor", dt.Rows[i]["主要仕入先CD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "主要仕入先CD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }

                if (!Is101("M_Brand", dt.Rows[i]["主要仕入先CD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "主要仕入先CD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }
                if (!Is102(dt.Rows[i]["メーカー商品CD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "メーカー商品CD";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                if (!Is102(dt.Rows[i]["展開サイズ数"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "展開サイズ数";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                IsNoB(dt,i, "展開サイズ数", "1");

                if (!Is102(dt.Rows[i]["展開カラー数"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "展開カラー数";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "展開カラー数", "1");

                if (!Is101("M_MultiPorpose", dt.Rows[i]["単位CD"].ToString(),"201"))
                {
                    dt.Rows[i]["EItem"] = "単位CD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }

                if (!Is101("M_MultiPorpose", dt.Rows[i]["競技CD"].ToString(), "202"))
                {
                    dt.Rows[i]["EItem"] = "競技CD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }

                if (!Is101("M_MultiPorpose", dt.Rows[i]["商品分類CD"].ToString(), "203"))
                {
                    dt.Rows[i]["EItem"] = "商品分類CD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }

                if (!Is101("M_MultiPorpose", dt.Rows[i]["セグメントCD"].ToString(), "206"))
                {
                    dt.Rows[i]["EItem"] = "セグメントCD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }

                if (!Is190(dt.Rows[i]["セット品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "セット品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt,i, "セット品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["プレゼント品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "プレゼント品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "プレゼント品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["サンプル品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "サンプル品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "サンプル品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["値引商品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "値引商品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "値引商品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["実店舗取扱区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "実店舗取扱区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "実店舗取扱区分", "M_SKUInitial");


                if (!Is190(dt.Rows[i]["在庫管理対象区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "在庫管理対象区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "在庫管理対象区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["架空商品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "架空商品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "架空商品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["直送品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "直送品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "直送品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["予約品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "予約品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "予約品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["特記区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "特記区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "特記区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["送料区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "送料区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "送料区分", "M_SKUInitial");


                if (!Is190(dt.Rows[i]["要加工品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "要加工品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "要加工品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["要確認品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "要確認品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "要確認品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["Web在庫連携区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Web在庫連携区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "Web在庫連携区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["販売停止品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "販売停止品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "販売停止品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["廃番品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "廃番品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "廃番品区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["完売品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "完売品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "完売品区分", "M_SKUInitial");


                if (!Is190(dt.Rows[i]["完売品区分名"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "完売品区分名";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "完売品区分名", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["自社在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "自社在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "自社在庫連携対象", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["メーカー在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "メーカー在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "メーカー在庫連携対象", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["店舗在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "店舗在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "店舗在庫連携対象", "M_SKUInitial");


                if (!Is190(dt.Rows[i]["Net発注不可区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Net発注不可区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "Net発注不可区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["EDI発注可能区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "EDI発注可能区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "EDI発注可能区分", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["自動発注対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "自動発注対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "自動発注対象", "M_SKUInitial");
                
                if (!Is190(dt.Rows[i]["カタログ掲載有無"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "カタログ掲載有無";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "カタログ掲載有無", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["小包梱包可能"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "小包梱包可能";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "小包梱包可能", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["小包梱包可能"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "小包梱包可能";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "小包梱包可能", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["税率区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "税率区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "税率区分", "M_SKUInitial");


                if (!Is190(dt.Rows[i]["原価計算方法"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "原価計算方法";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "原価計算方法", "M_SKUInitial");

                if (!Is190(dt.Rows[i]["Sale対象外区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Sale対象外区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "Sale対象外区分", "M_SKUInitial");

                IsNoB(dt,i, "標準原価");

                IsNoB(dt, i, "税込定価");

                IsNoB(dt, i, "税抜定価");

                IsNoB(dt, i, "発注税込価格");

                IsNoB(dt, i, "発注税抜価格");

                IsNoB(dt, i, "掛率");

                if (!Is103(dt.Rows[i]["発売開始日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "発売開始日";
                    dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }
                if (!Is103(dt.Rows[i]["Web掲載開始日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Web掲載開始日";
                    dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }

                if (!Is101("M_MultiPorpose", dt.Rows[i]["発注注意区分"].ToString(),"316"))
                {
                    dt.Rows[i]["EItem"] = "発注注意区分";
                    dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }

                if (!Is103(dt.Rows[i]["指示書発行日"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "指示書発行日";
                    dt.Rows[i]["Error"] = "E103";
                    goto SkippedLine;
                }

                IsNoB(dt, i, "掛率", dt.Rows[i]["ITEMCD"].ToString());

                IsNoB(dt, i, "発注ロット", "1");

                IsNoB(dt, i, "ITEMタグ2","1");

            SkippedLine:
                int g = 0;
            }
        }
        private void IsNoB(DataTable dt, int i ,string col,string Value =null)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][col].ToString()))
            {
                dt.Rows[i][col] =(Value ==null) ? "0" : Value;
            }
        }
        private bool Is103(string date)
        {
            return bbl.CheckDate(bbl.FormatDate(date));
        }
        private bool Is101(string tableName, string param, string paramID=null)
        {
            //if (value.ToString() == "0" || value.ToString() == "1" || string.IsNullOrEmpty(value))
            //{
            //    return true;
            //}
            return false;
        }
        private bool Is190(string value)
        {
            if (value.ToString() == "0" || value.ToString() == "1" || string.IsNullOrEmpty(value))
            {
                return true;
            }
            return false;
        }

        private bool Is102(string value)
        {
            if ( string.IsNullOrEmpty(value))
            {
                return true;
            }
            return false;
        }
        private DataTable ExcelToDatatable(string filePath)
        {
            try
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                string ext = Path.GetExtension(filePath);
                IExcelDataReader excelReader;
                if (ext.Equals(".xls"))
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (ext.Equals(".xlsx"))
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                    excelReader = ExcelReaderFactory.CreateCsvReader(stream, null);

                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                bool useHeaderRow = true;

                DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = useHeaderRow,
                    }
                });
                excelReader.Close();
                return result.Tables[0];
            }
            catch
            {
                bbl.ShowMessage("E137");
                return null;
            }

        }

        private void BT_Torikomi_Click(object sender, EventArgs e)
        {

        }
    }
}
