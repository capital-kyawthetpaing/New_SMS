using Base.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using System.IO;
using ExcelDataReader;
using System.Web.UI.WebControls;

namespace MasterTorikomi_SKU
{
    public partial class MasterTorikomi_SKU : FrmMainForm
    {
        Base_BL bbl;
        DataTable dtSKU = new DataTable();
        DataTable dtAdmin = new DataTable();
        DataTable dtBrand = new DataTable();
        DataTable dtMultiP = new DataTable();
        DataTable dtVendor = new DataTable();
        DataTable dtskuintial = new DataTable();
        DataTable dtAPI = new DataTable();
        SKU_BL sbl;
        MasterTorikomi_SKU_BL mtbl;
        M_SKUInitial_BL msIbl;
        API_BL apbl;
        bool checkerr = false;
        int type = 0;
        M_SKU_Entity mE;
        string filePath = string.Empty;
        string fileExt = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtmain = new DataTable();
        public MasterTorikomi_SKU()
        {
            InitializeComponent();
            bbl = new Base_BL();
            sbl = new SKU_BL();
            mtbl = new MasterTorikomi_SKU_BL();
            msIbl = new M_SKUInitial_BL();
            apbl = new API_BL();
        }
        private void MasterTorikomi_SKU_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTorikomi_SKU";
            StartProgram();
            RB_all.Checked = true;
           
            dtBrand = mtbl.M_Brand_SelectAll_NoPara();
            dtMultiP = mtbl.M_Multipurpose_SelectAll();
            dtVendor = mtbl.M_Vendor_SelectAll();
            dtskuintial = msIbl.M_SKUInitial_SelectAll();
            dtSKU = sbl.M_SKU_SelectAll_NOPara();
            //dtAPI = apbl.M_API_Select();
        }
        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {

                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        CleanData();
                    }
                    break;

                case 12:
                    InputExcel();
                    break;
            }
        }

        private void InputExcel()
        {
            if(String.IsNullOrEmpty(TB_FileName.Text))
            {
                bbl.ShowMessage("E121");
            }
            else
            {
                if (bbl.ShowMessage("Q001", "取込処理") == DialogResult.Yes)
                {
                    //string filePath = string.Empty;
                    //string fileExt = string.Empty;
                    ////if (!System.IO.Directory.Exists("C:\\SMS\\TenzikaiShouhin\\"))
                    ////{
                    ////    System.IO.Directory.CreateDirectory("C:\\SMS\\TenzikaiShouhin\\");
                    ////}
                    //OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                    //                                            // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
                    //                                            // file.RestoreDirectory = true;
                    //if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
                    //{
                    //   filePath = file.FileName; //get the path of the file  
                    //fileExt = Path.GetExtension(filePath); //get the file extension  
                    //if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                    //{
                    //    bbl.ShowMessage("E137");
                    //    return;
                    //}

                    dt = ExcelToDatatable(filePath);
                    if (dt != null)
                    {
                        if (ErrorCheck(dt))
                        {
                            ExcelErrorCheck(dt);
                            if (CheckPartial(dt))
                            {
                                type = RB_all.Checked ? 1 : RB_BaseInfo.Checked ? 2 : RB_attributeinfo.Checked ? 3 : RB_priceinfo.Checked ? 4 : RB_Catloginfo.Checked ? 5 : RB_tagInfo.Checked ? 6 : RB_JanCD.Checked ? 7 : RB_SizeURL.Checked ? 8 : 0;

                                mE = GetEntity(dt);
                                if (mtbl.MasterTorikomi_SKU_Insert_Update(type, mE))
                                {
                                    bbl.ShowMessage("I101");
                                }
                            }
                            GV_SKU.DataSource = null;
                            GV_SKU.DataSource = dt;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No row data was found or import excel is opening in different location");
                    }
                }
            }
        }
        private void BT_Torikomi_Click(object sender, EventArgs e)
        {
            InputExcel();
        }
        protected Boolean ColumnCheck(String[] colName, DataTable dtMain)
        {
            DataColumnCollection cols = dtMain.Columns;
            for (int i = 0; i < colName.Length; i++)
            {
                if (!cols.Contains(colName[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private bool ErrorCheck(DataTable dt)
        {
            if (dt.Columns.Contains("EItem") && dt.Columns.Contains("Error"))
            {
                dt.Columns.Remove("EItem");
                dt.Columns.Remove("Error");
            }
            string kibun = dt.Rows[0]["データ区分"].ToString();
            if (RB_all.Checked)
            {
                if (dt.Columns.Count != 124)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "1")
                    {
                        bbl.ShowMessage("E190");
                        return false;
                    }
                }
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                    "JANCD", "削除","諸口区分", "商品名", "カナ名",
                    "略名", "英語名", "ITEMCD", "サイズ枝番","カラー枝番",
                     "サイズ名", "カラー名", "主要仕入先CD", "主要仕入先名", 
                   "ブランドCD", "ブランド名", "メーカー商品CD", "単位CD",
                    "単位名","競技CD", "競技名", "商品分類CD","分類名",
                       "セグメントCD", "セグメント名", "セット品区分", "セット品区分名", "プレゼント品区分",
                    "プレゼント品区分名", "サンプル品区分", "サンプル品区分名", "値引商品区分", "値引商品区分名",
                    "Webストア取扱区分", "Webストア取扱区分名", "実店舗取扱区分","実店舗取扱区分名", "在庫管理対象区分",
                    "在庫管理対象区分名", "架空商品区分", "架空商品区分名", "直送品区分", "直送品区分名",
                                   "予約品区分", "予約品区分名", "特記区分", "特記区分名", "送料区分",
                    "送料区分名", "要加工品区分", "要加工品区分名", "要確認品区分", "要確認品区分名",
                                  "Web在庫連携区分", "Web在庫連携区分名", "販売停止品区分", "販売停止品区分名","廃番品区分",
                    "廃番品区分名", "完売品区分", "完売品区分名", "自社在庫連携対象", "自社在庫連携対象名",
                    "メーカー在庫連携対象",  "メーカー在庫連携対象名", "店舗在庫連携対象", "店舗在庫連携対象名", "Net発注不可区分",
                    "Net発注不可区分名", "EDI発注可能区分", "EDI発注可能区分名",  "自動発注対象区分", "自動発注対象",
                    "カタログ掲載有無区分", "カタログ掲載有無", "小包梱包可能区分", "小包梱包可能", "税率区分",
                                   "税率区分名", "原価計算方法", "原価計算方法名", "Sale対象外区分", "Sale対象外区分名",
                    "標準原価", "税抜定価", "税込定価", "発注税込価格", "発注税抜価格",
                                   "掛率", "発売開始日", "Web掲載開始日", "発注注意区分","発注注意区分名",
                    "発注注意事項", "管理用備考","表示用備考", "棚番", "年度", "シーズン",
                    "カタログ番号","カタログページ", "カタログ番号Long", "カタログページLong", "カタログ情報",
                    "指示書番号", "指示書発行日", "商品情報アドレス","構成数", "発注ロット",
                    "タグ1", "タグ2", "タグ3", "タグ4", "タグ5",
                      "タグ6", "タグ7", "タグ8", "タグ9", "タグ10"};
                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
                
            }
            else if (RB_BaseInfo.Checked)
            {
                if (dt.Columns.Count != 46)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                    "JANCD", "削除","諸口区分", "商品名", "カナ名",
                    "略名", "英語名", "ITEMCD", "サイズ枝番","カラー枝番",
                     "サイズ名", "カラー名", "主要仕入先CD", "主要仕入先名",
                    "ブランドCD", "ブランド名", "メーカー商品CD", "単位CD",
                    "単位名","競技CD", "競技名", "商品分類CD","分類名",
                       "セグメントCD", "セグメント名",  "標準原価", "税抜定価", "税込定価",
                  "発注税込価格", "発注税抜価格",  "掛率", "発売開始日", "Web掲載開始日",
                                 "発注注意区分","発注注意区分名","発注注意事項", "管理用備考","表示用備考",
                     "棚番","構成数", "発注ロット"
                    };
                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_attributeinfo.Checked)
            {

                if (dt.Columns.Count != 75)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                    "JANCD", "削除", "商品名", "ITEMCD", "サイズ枝番","カラー枝番",
                     "サイズ名", "カラー名", "主要仕入先CD", "主要仕入先名", "セット品区分",
                      "セット品区分名", "プレゼント品区分","プレゼント品区分名", "サンプル品区分", "サンプル品区分名",
                    "値引商品区分", "値引商品区分名",  "Webストア取扱区分", "Webストア取扱区分名", "実店舗取扱区分",
                    "実店舗取扱区分名", "在庫管理対象区分",  "在庫管理対象区分名", "架空商品区分", "架空商品区分名",
                   "直送品区分", "直送品区分名","予約品区分", "予約品区分名", "特記区分",
                     "特記区分名", "送料区分", "送料区分名", "要加工品区分", "要加工品区分名",
                    "要確認品区分", "要確認品区分名", "Web在庫連携区分", "Web在庫連携区分名", "販売停止品区分",
                     "販売停止品区分名","廃番品区分", "廃番品区分名", "完売品区分", "完売品区分名",
                    "自社在庫連携対象", "自社在庫連携対象名", "メーカー在庫連携対象",  "メーカー在庫連携対象名", "店舗在庫連携対象",
                   "店舗在庫連携対象名", "Net発注不可区分","Net発注不可区分名", "EDI発注可能区分", "EDI発注可能区分名",
                     "自動発注対象区分", "自動発注対象","カタログ掲載有無区分", "カタログ掲載有無", "小包梱包可能区分",
                    "小包梱包可能",  "Sale対象外区分", "Sale対象外区分名","標準原価", "税抜定価",
                     "税込定価", "発注税込価格", "発注税抜価格","掛率", };
                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_priceinfo.Checked)
            {
                if (dt.Columns.Count != 27)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                    "JANCD", "削除", "商品名", "ITEMCD", "サイズ枝番",
                    "カラー枝番", "サイズ名", "カラー名", "主要仕入先CD", "主要仕入先名",
                      "税率区分", "税率区分名", "原価計算方法", "原価計算方法名", "Sale対象外区分",
                                  "Sale対象外区分名", "標準原価", "税抜定価", "税込定価", "発注税込価格",
                    "発注税抜価格",
                                   "掛率"};
                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_Catloginfo.Checked)
            {
                if (dt.Columns.Count != 22)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                                     "JANCD", "削除", "商品名", "ITEMCD", "サイズ枝番",
                                     "カラー枝番", "サイズ名", "カラー名", "年度", "シーズン",
                                     "カタログ番号","カタログページ", "カタログ番号Long", "カタログページLong", "カタログ情報",
                                     "指示書番号", "指示書発行日"};
                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_tagInfo.Checked)
            {
                if (dt.Columns.Count != 23)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD", "JANCD", "削除", "商品名",  "ITEMCD", "サイズ枝番",
                    "カラー枝番", "サイズ名", "カラー名","タグ1", "タグ2", "タグ3", "タグ4", "タグ5",     "タグ6", "タグ7", "タグ8", "タグ9", "タグ10"};


                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_JanCD.Checked)
            {
                if (dt.Columns.Count != 16)
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "7")
                    {
                        bbl.ShowMessage("E137");
                        return false;
                    }
                }
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD","JANCD", "削除", "商品名", "カナ名",
                                 "略名", "英語名", "ITEMCD", "サイズ枝番","カラー枝番", "サイズ名", "カラー名"};

                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            else if (RB_SizeURL.Checked)
            {
                if (dt.Columns.Count != 15)
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
                string[] colname = {"データ区分", "AdminNO", "改定日", "承認日", "SKUCD",
                                    "JANCD", "削除", "商品名",  "ITEMCD", "サイズ枝番","カラー枝番",  "サイズ名", "カラー名","APIKey","サイト商品CD"
                                    };

                if (!ColumnCheck(colname, dt))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
            }
            return true;
        }
        private bool Check(DataTable dt)
        {
            if (dt.Columns.Contains("EItem") && dt.Columns.Contains("Error"))
            {
                dt.Columns.Remove("EItem");
                dt.Columns.Remove("Error");
            }
            string kibun = dt.Rows[0]["データ区分"].ToString();
            if (RB_all.Checked)
            {
                if (dt.Columns.Count != 126)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "1")
                    {
                        return false;
                    }
                }
            }
            else if (RB_BaseInfo.Checked)
            {
                if (dt.Columns.Count != 48)
                {
                    return false;
                }
                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "2")
                    {
                        return false;
                    }
                }
            }
            else if (RB_attributeinfo.Checked)
            {

                if (dt.Columns.Count != 75)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "3")
                    {
                        return false;
                    }
                }
            }
            else if (RB_priceinfo.Checked)
            {
                if (dt.Columns.Count != 27)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "4")
                    {
                        return false;
                    }
                }
            }
            else if (RB_Catloginfo.Checked)
            {
                if (dt.Columns.Count != 22)
                {
                    return false;
                }
                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "5")
                    {
                        return false;
                    }
                }
            }
            else if (RB_tagInfo.Checked)
            {
                if (dt.Columns.Count != 23)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "6")
                    {
                        return false;
                    }
                }

            }
            else if (RB_JanCD.Checked)
            {
                if (dt.Columns.Count != 16)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "7")
                    {
                        return false;
                    }
                }

            }
            else if (RB_SizeURL.Checked)
            {
                if (dt.Columns.Count != 15)
                {
                    return false;
                }

                if (!String.IsNullOrEmpty(dt.Rows[1]["データ区分"].ToString()))
                {
                    if (kibun != "8")
                    {
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
            
            if (RB_SizeURL.Checked)
            {
                dtAPI = apbl.M_API_Select();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (String.IsNullOrEmpty(dt.Rows[i]["データ区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "データ区分";
                    dt.Rows[i]["Error"] = "E102";
                }

                if (String.IsNullOrEmpty(dt.Rows[i]["AdminNO"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "AdminNO";
                    dt.Rows[i]["Error"] = "E102";
                }
                else
                {
                    if (dt.Rows[i]["AdminNo"].ToString() != "New")
                    {
                        String query = "AdminNO = " + dt.Rows[i]["AdminNO"].ToString() + "" +
                            " and SKUCD = '" + dt.Rows[i]["SKUCD"].ToString() + "'";

                        var result = dtSKU.Select(query);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "JANCD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    //else
                    //{
                    //    var dtAdmin = mtbl.M_SKUCounter_Update();
                    //    if (dtAdmin.Rows.Count > 0)
                    //    {
                    //        dt.Rows[i]["AdminNo"] = dtAdmin.Rows[0]["AdminNO"].ToString();
                    //    }
                    //}
                }
                if (!String.IsNullOrEmpty(dt.Rows[i]["改定日"].ToString()))
                {
                    string date = bbl.FormatDate(dt.Rows[i]["改定日"].ToString());
                    if (!bbl.CheckDate(date))
                    {
                        dt.Rows[i]["EItem"] = "改定日";
                        dt.Rows[i]["Error"] = "E103";
                    }
                }
                else
                {
                    dt.Rows[i]["EItem"] = "改定日";
                    dt.Rows[i]["Error"] = "E102";
                }
                if (!String.IsNullOrEmpty(dt.Rows[i]["承認日"].ToString()))
                {
                    string date = bbl.FormatDate(dt.Rows[i]["承認日"].ToString());
                    if (!bbl.CheckDate(date))
                    {
                        dt.Rows[i]["EItem"] = "承認日";
                        dt.Rows[i]["Error"] = "E103";
                    }
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["SKUCD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "SKUCD";
                    dt.Rows[i]["Error"] = "E102";
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["JANCD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "JANCD";
                    dt.Rows[i]["Error"] = "E102";
                }
                else
                {
                    if (dt.Rows[i]["JANCD"].ToString() != "Auto")
                    {
                        //String query = " JanCD = '" + dt.Rows[i]["JANCD"].ToString() + "'" +
                        //    " and SKUCD = '" + dt.Rows[i]["SKUCD"].ToString() + "'";
                        //var result = dtSKU.Select(query);
                        //if (result.Count() == 0)
                        //{
                        //    dt.Rows[i]["EItem"] = "JANCD";
                        //    dt.Rows[i]["Error"] = "E101";
                        //}
                    }
                    else
                    {
                        var dtJan = mtbl.M_JANCounter_JanContUpdate();
                        if (dtJan.Rows.Count > 0)
                        {
                            dt.Rows[i]["JanCount"] = dtJan.Rows[0]["JanCount"].ToString();
                        }
                    }
                }
                if (!String.IsNullOrEmpty(dt.Rows[i]["削除"].ToString()))
                {
                    string d = dt.Rows[i]["削除"].ToString();
                    if (dt.Rows[i]["削除"].ToString() != "0" && dt.Rows[i]["削除"].ToString() != "1")
                    {
                        dt.Rows[i]["EItem"] = "削除";
                        dt.Rows[i]["Error"] = "E190";
                    }
                }
                //if (String.IsNullOrEmpty(dt.Rows[i]["商品名"].ToString()))
                //{
                //    dt.Rows[i]["EItem"] = "商品名";
                //    dt.Rows[i]["Error"] = "E102";
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["ITEMCD"].ToString()))
                //{

                //    dt.Rows[i]["EItem"] = "ITEMCD";
                //    dt.Rows[i]["Error"] = "E102";
                //}
                if (String.IsNullOrEmpty(dt.Rows[i]["サイズ枝番"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "サイズ枝番";
                    dt.Rows[i]["Error"] = "E102";
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["カラー枝番"].ToString()))
                {

                    dt.Rows[i]["EItem"] = "カラー枝番";
                    dt.Rows[i]["Error"] = "E102";
                }
                if (String.IsNullOrEmpty(dt.Rows[i]["サイズ名"].ToString()))
                {

                    dt.Rows[i]["EItem"] = "サイズ名";
                    dt.Rows[i]["Error"] = "E102";
                }
                if (RB_all.Checked || RB_BaseInfo.Checked )
                {
                    //if (!String.IsNullOrEmpty(dt.Rows[i]["主要仕入先CD"].ToString()))
                    //{
                    //    string query = " VendorCD = '" + dt.Rows[i]["主要仕入先CD"].ToString() + "'";
                    //    var result = dtVendor.Select(query);
                    //    if (result.Count() == 0)
                    //    {
                    //        dt.Rows[i]["EItem"] = "主要仕入先CD";
                    //        dt.Rows[i]["Error"] = "E101";
                    //    }
                    //}

                    if (!String.IsNullOrEmpty(dt.Rows[i]["ブランドCD"].ToString()))
                    {
                        String Bq = " BrandCD = '" + dt.Rows[i]["ブランドCD"].ToString() + "'";
                        var result = dtBrand.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "ブランドCD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["単位CD"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["単位CD"].ToString() + "'" +
                            "and ID= 201";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "単位CD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["競技CD"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["競技CD"].ToString() + "'" +
                            "and ID= 202";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "競技CD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["発売開始日"].ToString()))
                    {
                        string date = bbl.FormatDate(dt.Rows[i]["発売開始日"].ToString());
                        if (!bbl.CheckDate(date))
                        {
                            dt.Rows[i]["EItem"] = "発売開始日";
                            dt.Rows[i]["Error"] = "E103";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Web掲載開始日"].ToString()))
                    {
                        string date = bbl.FormatDate(dt.Rows[i]["Web掲載開始日"].ToString());
                        if (!bbl.CheckDate(date))
                        {
                            dt.Rows[i]["EItem"] = "Web掲載開始日";
                            dt.Rows[i]["Error"] = "E103";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["セグメントCD"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["セグメントCD"].ToString() + "'" +
                            "and ID= 226";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "セグメントCD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["発注注意区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["発注注意区分"].ToString() + "'" +
                            "and ID= 316";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "発注注意区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                }

                else if (RB_all.Checked || RB_attributeinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["セット品区分"].ToString()))
                    {
                        if (dt.Rows[i]["セット品区分"].ToString() != "0" && dt.Rows[i]["セット品区分"].ToString() != "1")
                        {
                            dt.Rows[i]["EItem"] = "セット品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if(dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["セット品区分"] = dtskuintial.Rows[0]["SetKBN"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["プレゼント品区分"].ToString()))
                    {
                        if (dt.Rows[i]["プレゼント品区分"].ToString() != "0" && dt.Rows[i]["プレゼント品区分"].ToString() != "1")
                        {
                            dt.Rows[i]["EItem"] = "プレゼント品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["プレゼント品区分"] = dtskuintial.Rows[0]["PresentKBN"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["サンプル品区分"].ToString()))
                    {
                        if (dt.Rows[i]["サンプル品区分"].ToString() != "1" && dt.Rows[i]["サンプル品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "サンプル品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["サンプル品区分"] = dtskuintial.Rows[0]["SampleKBN"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["値引商品区分"].ToString()))
                    {
                        if (dt.Rows[i]["値引商品区分"].ToString() != "1" && dt.Rows[i]["値引商品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "値引商品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["値引商品区分"] = dtskuintial.Rows[0]["DiscountKBN"];
                        }
                    }


                    if (!String.IsNullOrEmpty(dt.Rows[i]["Webストア取扱区分"].ToString()))
                    {

                        if (dt.Rows[i]["Webストア取扱区分"].ToString() != "1" && dt.Rows[i]["Webストア取扱区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "Webストア取扱区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["Webストア取扱区分"] = dtskuintial.Rows[0]["WebFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["実店舗取扱区分"].ToString()))
                    {
                        if (dt.Rows[i]["実店舗取扱区分"].ToString() != "1" && dt.Rows[i]["実店舗取扱区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "実店舗取扱区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["実店舗取扱区分"] = dtskuintial.Rows[0]["RealStoreFlg"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["在庫管理対象区分"].ToString()))
                    {
                        if (dt.Rows[i]["在庫管理対象区分"].ToString() != "1" && dt.Rows[i]["在庫管理対象区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "在庫管理対象区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["在庫管理対象区分"] = dtskuintial.Rows[0]["ZaikoKBN"];
                        }
                    }
                   
                    if (!String.IsNullOrEmpty(dt.Rows[i]["架空商品区分"].ToString()))
                    {
                        if (dt.Rows[i]["架空商品区分"].ToString() != "1" && dt.Rows[i]["架空商品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "架空商品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["架空商品区分"] = dtskuintial.Rows[0]["VirtualFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["直送品区分"].ToString()))
                    {
                        if (dt.Rows[i]["直送品区分"].ToString() != "1" && dt.Rows[i]["直送品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "直送品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["直送品区分"] = dtskuintial.Rows[0]["DirectFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["予約品区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["予約品区分"].ToString() + "'" +
                              "and ID= 311";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "予約品区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["予約品区分"] = dtskuintial.Rows[0]["ReserveCD"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["特記区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["特記区分"].ToString() + "'" +
                                 "and ID= 310";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "特記区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["特記区分"] = dtskuintial.Rows[0]["NoticesCD"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["送料区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["送料区分"].ToString() + "'" +
                                  "and ID= 309";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "送料区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["送料区分"] = dtskuintial.Rows[0]["PostageCD"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["要加工品区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["要加工品区分"].ToString() + "'" +
                                 "and ID= 312";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "要加工品区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["要加工品区分"] = dtskuintial.Rows[0]["ManufactCD"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["要確認品区分"].ToString()))
                    {
                        String Bq = " [Key] ='" + dt.Rows[i]["要確認品区分"].ToString() + "'" +
                                "and ID= 313";
                        var result = dtMultiP.Select(Bq);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "要確認品区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["要確認品区分"] = dtskuintial.Rows[0]["ConfirmCD"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["廃番品区分"].ToString()))
                    {
                        if (dt.Rows[i]["廃番品区分"].ToString() != "1" && dt.Rows[i]["廃番品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "廃番品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["廃番品区分"] = dtskuintial.Rows[0]["DiscontinueFlg"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["完売品区分"].ToString()))
                    {
                        if (dt.Rows[i]["完売品区分"].ToString() != "1" && dt.Rows[i]["完売品区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "完売品区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["完売品区分"] = dtskuintial.Rows[0]["SoldoutFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自社在庫連携対象"].ToString()))
                    {
                        if (dt.Rows[i]["自社在庫連携対象"].ToString() != "1" && dt.Rows[i]["自社在庫連携対象"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "自社在庫連携対象";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["自社在庫連携対象"] = dtskuintial.Rows[0]["InventoryAddFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["メーカー在庫連携対象"].ToString()))
                    {
                        if (dt.Rows[i]["メーカー在庫連携対象"].ToString() != "1" && dt.Rows[i]["メーカー在庫連携対象"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "メーカー在庫連携対象";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["メーカー在庫連携対象"] = dtskuintial.Rows[0]["MakerAddFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Net発注不可区分"].ToString()))
                    {
                        if (dt.Rows[i]["Net発注不可区分"].ToString() != "1" && dt.Rows[i]["Net発注不可区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "Net発注不可区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["Net発注不可区分"] = dtskuintial.Rows[0]["NoNetOrderFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["EDI発注可能区分"].ToString()))
                    {
                        if (dt.Rows[i]["EDI発注可能区分"].ToString() != "1" && dt.Rows[i]["EDI発注可能区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "EDI発注可能区分";
                            dt.Rows[i]["Error"] = "E190";
                        }

                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["EDI発注可能区分"] = dtskuintial.Rows[0]["EDIorderFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自動発注対象区分"].ToString()))
                    {
                        if (dt.Rows[i]["自動発注対象区分"].ToString() != "1" && dt.Rows[i]["自動発注対象区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "自動発注対象区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["自動発注対象区分"] = dtskuintial.Rows[0]["AutoOrderFlg"];
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ掲載有無区分"].ToString()))
                    {
                        if (dt.Rows[i]["カタログ掲載有無区分"].ToString() != "1" && dt.Rows[i]["カタログ掲載有無区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "カタログ掲載有無区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["カタログ掲載有無区分"] = dtskuintial.Rows[0]["CatalogFlg"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["小包梱包可能区分"].ToString()))
                    {
                        if (dt.Rows[i]["小包梱包可能区分"].ToString() != "1" && dt.Rows[i]["小包梱包可能区分"].ToString() != "0")
                        {
                            dt.Rows[i]["EItem"] = "小包梱包可能区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["小包梱包可能区分"] = dtskuintial.Rows[0]["ParcelFlg"];
                        }
                    }

                }

                else if (RB_all.Checked || RB_priceinfo.Checked)
                {

                    if (!String.IsNullOrEmpty(dt.Rows[i]["税率区分"].ToString()))
                    {
                        if (dt.Rows[i]["税率区分"].ToString() != "1" && dt.Rows[i]["税率区分"].ToString() != "2")
                        {
                            dt.Rows[i]["EItem"] = "税率区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["税率区分"] = dtskuintial.Rows[0]["TaxRateFLG"];
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["原価計算方法"].ToString()))
                    {
                        if (dt.Rows[i]["原価計算方法"].ToString() != "1" && dt.Rows[i]["原価計算方法"].ToString() != "2")
                        {
                            dt.Rows[i]["EItem"] = "原価計算方法";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["原価計算方法"] = dtskuintial.Rows[0]["CostingKBN"];
                        }
                    }
                }


                else if (RB_all.Checked || RB_Catloginfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["指示書発行日"].ToString()))
                    {
                        string date = bbl.FormatDate(dt.Rows[i]["指示書発行日"].ToString());
                        if (!bbl.CheckDate(dt.Rows[i]["指示書発行日"].ToString()))
                        {

                            dt.Rows[i]["EItem"] = "指示書発行日";
                            dt.Rows[i]["Error"] = "E103";
                        }
                    }
                }

                else if (RB_all.Checked || RB_attributeinfo.Checked || RB_priceinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Sale対象外区分"].ToString()))
                    {
                        if (dt.Rows[i]["Sale対象外区分"].ToString() != "0" && dt.Rows[i]["Sale対象外区分"].ToString() != "1")
                        {
                            dt.Rows[i]["EItem"] = "Sale対象外区分";
                            dt.Rows[i]["Error"] = "E190";
                        }
                    }
                    else
                    {
                        if (dtskuintial.Rows.Count > 0)
                        {
                            dt.Rows[i]["Sale対象外区分"] = dtskuintial.Rows[0]["SaleExcludedFlg"];
                        }
                    }
                }  

                else if(RB_all.Checked || RB_attributeinfo.Checked || RB_BaseInfo.Checked || RB_priceinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["主要仕入先CD"].ToString()))
                    {
                        string query = " VendorCD = '" + dt.Rows[i]["主要仕入先CD"].ToString() + "'";
                        var result = dtVendor.Select(query);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "主要仕入先CD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }

                }
                else if (RB_SizeURL.Checked)
                {
                    if (String.IsNullOrEmpty(dt.Rows[i]["APIKey"].ToString()))
                    {
                        dt.Rows[i]["EItem"] = "APIKey";
                        dt.Rows[i]["Error"] = "E102";
                    }
                    else
                    {
                        String query = " APIKey = '" + dt.Rows[i]["APIKey"].ToString() + "'";

                        var result = dtAPI.Select(query);
                        if (result.Count() == 0)
                        {
                            dt.Rows[i]["EItem"] = "APIKey";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["サイト商品CD"].ToString()))
                    {
                    }
                }
            }

        }
        private DataTable ChangeColName(DataTable dt, int type)
        {

            if (type == 1)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["諸口区分"].ColumnName = "VariousFLG";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["カナ名"].ColumnName = "KanaName";
                dt.Columns["略名"].ColumnName = "SKUShortName";
                dt.Columns["英語名"].ColumnName = "EnglishName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["セット品区分"].ColumnName = "SetKBN";
                dt.Columns["プレゼント品区分"].ColumnName = "PresentKBN";
                dt.Columns["サンプル品区分"].ColumnName = "SampleKBN";
                dt.Columns["値引商品区分"].ColumnName = "DiscountKBN";
                dt.Columns["主要仕入先CD"].ColumnName = "MainVendorCD";
                dt.Columns["主要仕入先名"].ColumnName = "VendorName";
                //dt.Columns["メーカー仕入先CD"].ColumnName = "MakerVendorCD";
                //dt.Columns["メーカー仕入先名"].ColumnName = "MakerVendorName";
                dt.Columns["ブランドCD"].ColumnName = "BrandCD";
                dt.Columns["ブランド名"].ColumnName = "BrandName";
                dt.Columns["メーカー商品CD"].ColumnName = "MakerItem";
                dt.Columns["単位CD"].ColumnName = "TaniCD";
                dt.Columns["単位名"].ColumnName = "TaniName";
                dt.Columns["競技CD"].ColumnName = "SportsCD";
                dt.Columns["競技名"].ColumnName = "SportsName";
                dt.Columns["商品分類CD"].ColumnName = "SegmentCD";
                dt.Columns["分類名"].ColumnName = "SegmentName";
                dt.Columns["セグメントCD"].ColumnName = "ExhibitionSegmentCD";
                dt.Columns["セグメント名"].ColumnName = "ExhibitionSegmentName";
                dt.Columns["Webストア取扱区分"].ColumnName = "WebFlg";
                dt.Columns["実店舗取扱区分"].ColumnName = "RealStoreFlg";
                dt.Columns["在庫管理対象区分"].ColumnName = "ZaikoKBN";
                dt.Columns["棚番"].ColumnName = "Rack";
                dt.Columns["架空商品区分"].ColumnName = "VirtualFlg";
                dt.Columns["直送品区分"].ColumnName = "DirectFlg";
                dt.Columns["予約品区分"].ColumnName = "ReserveCD";
                dt.Columns["特記区分"].ColumnName = "NoticesCD";
                dt.Columns["送料区分"].ColumnName = "PostageCD";
                dt.Columns["要加工品区分"].ColumnName = "ManufactCD";
                dt.Columns["要確認品区分"].ColumnName = "ConfirmCD";
                dt.Columns["Web在庫連携区分"].ColumnName = "WebStockFlg";
                dt.Columns["販売停止品区分"].ColumnName = "StopFlg";
                dt.Columns["廃番品区分"].ColumnName = "DiscontinueFlg";
                dt.Columns["完売品区分"].ColumnName = "SoldoutFlg";
                dt.Columns["自社在庫連携対象"].ColumnName = "InventoryAddFlg";
                dt.Columns["メーカー在庫連携対象"].ColumnName = "MakerAddFlg";
                dt.Columns["店舗在庫連携対象"].ColumnName = "StoreAddFlg";
                dt.Columns["Net発注不可区分"].ColumnName = "NoNetOrderFlg";
                dt.Columns["EDI発注可能区分"].ColumnName = "EDIorderFlg";
                dt.Columns["自動発注対象区分"].ColumnName = "AutoOrderFlg";
                dt.Columns["カタログ掲載有無区分"].ColumnName = "CatalogFlg";
                dt.Columns["小包梱包可能区分"].ColumnName = "ParcelFlg";
                dt.Columns["標準原価"].ColumnName = "NormalCost";
                dt.Columns["Sale対象外区分"].ColumnName = "SaleExcludedFlg";
                dt.Columns["税率区分"].ColumnName = "TaxRateFlg";
                dt.Columns["原価計算方法"].ColumnName = "CostingKBN";
                dt.Columns["税抜定価"].ColumnName = "PriceWithTax";
                dt.Columns["税込定価"].ColumnName = "PriceOutTax";
                dt.Columns["発注税込価格"].ColumnName = "OrderPriceWithTax";
                dt.Columns["発注税抜価格"].ColumnName = "OrderPriceWithoutTax";
                dt.Columns["掛率"].ColumnName = "Rate";
                dt.Columns["年度"].ColumnName = "LastYeaar";
                dt.Columns["シーズン"].ColumnName = "LastSeason";
                dt.Columns["カタログ番号"].ColumnName = "LastCatalogNo";
                dt.Columns["カタログページ"].ColumnName = "LastCatalogPage";
                dt.Columns["カタログ番号Long"].ColumnName = "LastCatalogNoLong";
                dt.Columns["カタログページLong"].ColumnName = "LastCatalogPageLong";
                dt.Columns["カタログ情報"].ColumnName = "LastCatalogText";
                dt.Columns["指示書番号"].ColumnName = "LastInstructionNo";
                dt.Columns["指示書発行日"].ColumnName = "LastInstructionDate";
                dt.Columns["商品情報アドレス"].ColumnName = "WebAddress";
                dt.Columns["発売開始日"].ColumnName = "SaleStartDate";
                dt.Columns["Web掲載開始日"].ColumnName = "WebStartDate";
                dt.Columns["発注注意区分"].ColumnName = "OrderAttentionCD";
                dt.Columns["発注注意区分名"].ColumnName = "OrderAttentionName";
                dt.Columns["発注注意事項"].ColumnName = "OrderAttentionNote";
                dt.Columns["管理用備考"].ColumnName = "CommentInStore";
                dt.Columns["表示用備考"].ColumnName = "CommentOutStore";
                dt.Columns["発注ロット"].ColumnName = "OrderLot";
                dt.Columns["タグ1"].ColumnName = "TageName1";
                dt.Columns["タグ2"].ColumnName = "TageName2";
                dt.Columns["タグ3"].ColumnName = "TageName3";
                dt.Columns["タグ4"].ColumnName = "TageNam e4";
                dt.Columns["タグ5"].ColumnName = "TageName5";
                dt.Columns["タグ6"].ColumnName = "TageName6";
                dt.Columns["タグ7"].ColumnName = "TageName7";
                dt.Columns["タグ8"].ColumnName = "TageName8";
                dt.Columns["タグ9"].ColumnName = "TageName9";
                dt.Columns["タグ10"].ColumnName = "TageName10";


            }
            else if (type == 2)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["諸口区分"].ColumnName = "VariousFLG";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["カナ名"].ColumnName = "KanaName";
                dt.Columns["略名"].ColumnName = "SKUShortName";
                dt.Columns["英語名"].ColumnName = "EnglishName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["主要仕入先CD"].ColumnName = "MainVendorCD";
                dt.Columns["主要仕入先名"].ColumnName = "VendorName";
                //dt.Columns["メーカー仕入先CD"].ColumnName = "MakerVendorCD";
                //dt.Columns["メーカー仕入先名"].ColumnName = "MakerVendorName";
                dt.Columns["ブランドCD"].ColumnName = "BrandCD";
                dt.Columns["ブランド名"].ColumnName = "BrandName";
                dt.Columns["メーカー商品CD"].ColumnName = "MakerItem";
                dt.Columns["単位CD"].ColumnName = "TaniCD";
                dt.Columns["単位名"].ColumnName = "TaniName";
                dt.Columns["競技CD"].ColumnName = "SportsCD";
                dt.Columns["競技名"].ColumnName = "SportsName";
                dt.Columns["商品分類CD"].ColumnName = "SegmentCD";
                dt.Columns["分類名"].ColumnName = "SegmentName";
                dt.Columns["セグメントCD"].ColumnName = "ExhibitionSegmentCD";
                dt.Columns["セグメント名"].ColumnName = "ExhibitionSegmentName";
                dt.Columns["標準原価"].ColumnName = "NormalCost";
                dt.Columns["税抜定価"].ColumnName = "PriceWithTax";
                dt.Columns["税込定価"].ColumnName = "PriceOutTax";
                dt.Columns["発注税込価格"].ColumnName = "OrderPriceWithTax";
                dt.Columns["発注税抜価格"].ColumnName = "OrderPriceWithoutTax";
                dt.Columns["掛率"].ColumnName = "Rate";
                dt.Columns["発売開始日"].ColumnName = "SaleStartDate";
                dt.Columns["Web掲載開始日"].ColumnName = "WebStartDate";
                dt.Columns["発注注意区分"].ColumnName = "OrderAttentionCD";
                dt.Columns["発注注意区分名"].ColumnName = "OrderAttentionName";
                dt.Columns["発注注意事項"].ColumnName = "OrderAttentionNote";
                dt.Columns["管理用備考"].ColumnName = "CommentInStore";
                dt.Columns["表示用備考"].ColumnName = "CommentOutStore";
                dt.Columns["棚番"].ColumnName = "Rank";
                dt.Columns["構成数"].ColumnName = "SetSU";
                dt.Columns["発注ロット"].ColumnName = "OrderLot";
            }
            else if (type == 3)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["セット品区分"].ColumnName = "SetKBN";
                dt.Columns["プレゼント品区分"].ColumnName = "PresentKBN";
                dt.Columns["サンプル品区分"].ColumnName = "SampleKBN";
                dt.Columns["値引商品区分"].ColumnName = "DiscountKBN";
                dt.Columns["主要仕入先CD"].ColumnName = "MainVendorCD";
                dt.Columns["主要仕入先名"].ColumnName = "VendorName";
                dt.Columns["Webストア取扱区分"].ColumnName = "WebFlg";
                dt.Columns["実店舗取扱区分"].ColumnName = "RealStoreFlg";
                dt.Columns["在庫管理対象区分"].ColumnName = "ZaikoKBN";
                // dt.Columns["棚番"].ColumnName = "Rack";  
                dt.Columns["架空商品区分"].ColumnName = "VirtualFlg";
                dt.Columns["直送品区分"].ColumnName = "DirectFlg";
                dt.Columns["予約品区分"].ColumnName = "ReserveCD";
                dt.Columns["特記区分"].ColumnName = "NoticesCD";
                dt.Columns["送料区分"].ColumnName = "PostageCD";
                dt.Columns["要加工品区分"].ColumnName = "ManufactCD";
                dt.Columns["要確認品区分"].ColumnName = "ConfirmCD";
                dt.Columns["Web在庫連携区分"].ColumnName = "WebStockFlg";
                dt.Columns["販売停止品区分"].ColumnName = "StopFlg";
                dt.Columns["廃番品区分"].ColumnName = "DiscontinueFlg";
                dt.Columns["完売品区分"].ColumnName = "SoldoutFlg";
                dt.Columns["自社在庫連携対象"].ColumnName = "InventoryAddFlg";
                dt.Columns["メーカー在庫連携対象"].ColumnName = "MakerAddFlg";
                dt.Columns["店舗在庫連携対象"].ColumnName = "StoreAddFlg";
                dt.Columns["Net発注不可区分"].ColumnName = "NoNetOrderFlg";
                dt.Columns["EDI発注可能区分"].ColumnName = "EDIorderFlg";
                dt.Columns["自動発注対象区分"].ColumnName = "AutoOrderFlg";
                dt.Columns["カタログ掲載有無区分"].ColumnName = "CatalogFlg";
                dt.Columns["小包梱包可能区分"].ColumnName = "ParcelFlg";
                dt.Columns["Sale対象外区分"].ColumnName = "SaleExcludedFlg";
                dt.Columns["標準原価"].ColumnName = "NormalCost";
                dt.Columns["税抜定価"].ColumnName = "PriceWithTax";
                dt.Columns["税込定価"].ColumnName = "PriceOutTax";
                dt.Columns["発注税込価格"].ColumnName = "OrderPriceWithTax";
                dt.Columns["発注税抜価格"].ColumnName = "OrderPriceWithoutTax";
                dt.Columns["掛率"].ColumnName = "Rate";
            }
            else if (type == 4)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["主要仕入先CD"].ColumnName = "MainVendorCD";
                dt.Columns["主要仕入先名"].ColumnName = "VendorName";
                dt.Columns["税率区分"].ColumnName = "TaxRateFlg";
                dt.Columns["Sale対象外区分"].ColumnName = "SaleExcludedFlg";
                dt.Columns["原価計算方法"].ColumnName = "CostingKBN";
                dt.Columns["標準原価"].ColumnName = "NormalCost";
                dt.Columns["税抜定価"].ColumnName = "PriceWithTax";
                dt.Columns["税込定価"].ColumnName = "PriceOutTax";
                dt.Columns["発注税込価格"].ColumnName = "OrderPriceWithTax";
                dt.Columns["発注税抜価格"].ColumnName = "OrderPriceWithoutTax";
                dt.Columns["掛率"].ColumnName = "Rate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
            }
            else if (type == 5)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["年度"].ColumnName = "LastYeaar";
                dt.Columns["シーズン"].ColumnName = "LastSeason";
                dt.Columns["カタログ番号"].ColumnName = "LastCatalogNo";
                dt.Columns["カタログページ"].ColumnName = "LastCatalogPage";
                dt.Columns["カタログ番号Long"].ColumnName = "LastCatalogNoLong";
                dt.Columns["カタログページLong"].ColumnName = "LastCatalogPageLong";
                dt.Columns["カタログ情報"].ColumnName = "LastCatalogText";
                dt.Columns["指示書番号"].ColumnName = "LastInstructionNo";
                dt.Columns["指示書発行日"].ColumnName = "LastInstructionDate";

            }
            else if (type == 6)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["タグ1"].ColumnName = "TageName1";
                dt.Columns["タグ2"].ColumnName = "TageName2";
                dt.Columns["タグ3"].ColumnName = "TageName3";
                dt.Columns["タグ4"].ColumnName = "TageNam e4";
                dt.Columns["タグ5"].ColumnName = "TageName5";
                dt.Columns["タグ6"].ColumnName = "TageName6";
                dt.Columns["タグ7"].ColumnName = "TageName7";
                dt.Columns["タグ8"].ColumnName = "TageName8";
                dt.Columns["タグ9"].ColumnName = "TageName9";
                dt.Columns["タグ10"].ColumnName = "TageName10";
            }
            else if (type == 7)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["カナ名"].ColumnName = "KanaName";
                dt.Columns["略名"].ColumnName = "SKUShortName";
                dt.Columns["英語名"].ColumnName = "EnglishName";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
            }
            else if (type == 8)
            {
                dt.Columns["改定日"].ColumnName = "ChangeDate";
                dt.Columns["承認日"].ColumnName = "ApprovalDate";
                dt.Columns["削除"].ColumnName = "DeleteFlg";
                dt.Columns["商品名"].ColumnName = "SKUName";
                dt.Columns["サイズ枝番"].ColumnName = "SizeNO";
                dt.Columns["カラー枝番"].ColumnName = "ColorNO";
                dt.Columns["サイズ名"].ColumnName = "SizeName";
                dt.Columns["カラー名"].ColumnName = "ColorName";
                dt.Columns["サイト商品CD"].ColumnName = "ShouhinCD";

            }
            return dt;
        }
        private void CleanData()
        {
            RB_all.Checked = true;
            GV_SKU.DataSource = null;
            TB_FileName.Text = "";
            
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
               // bbl.ShowMessage("E137");
                return null;
            }
        }
        private void GV_SKU_Paint(object sender, PaintEventArgs e)
        {
            for (int j = 6; j < 8;)
            {
                Rectangle r1 = this.GV_SKU.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.GV_SKU.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;
                j += 7;
            }
        }

        private bool CheckPartial(DataTable dt)
        {
            var query = "Error <> ''";
            if (dt.Select(query).Count() > 0)
                return false;
            return true;
        }
        private M_SKU_Entity GetEntity(DataTable dtT)
        {
            mE = new M_SKU_Entity
            {
                dt1 = dtT,
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                Key = filePath,
                PC = InPcID,
            };
            return mE;
        }

        private void BT_FileName_Click(object sender, EventArgs e)
        {
            //if (!System.IO.Directory.Exists("C:\\SMS\\TenzikaiShouhin\\"))
            //{
            //    System.IO.Directory.CreateDirectory("C:\\SMS\\TenzikaiShouhin\\");
            //}
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
                                                        // file.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
                                                        // file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                TB_FileName.Text = file.FileName;
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                {
                    // bbl.ShowMessage("E137");
                    MessageBox.Show("No row data was found or import excel is opening in different location");
                    return;
                }

               // type = RB_all.Checked ? 1 : RB_BaseInfo.Checked ? 2 : RB_attributeinfo.Checked ? 3 : RB_priceinfo.Checked ? 4 : RB_Catloginfo.Checked ? 5 : RB_tagInfo.Checked ? 6 : RB_JanCD.Checked ? 7 : RB_SizeURL.Checked ? 8 : 0;
                //dt = ExcelToDatatable(filePath);
                //if (dt != null)
                //{
                //    if (Check(dt))
                //    {
                //        dtmain = dt.Copy();
                //        dtmain = ChangeColName(dtmain, type);
                //    }
                //}
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private void MasterTorikomi_SKU_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
