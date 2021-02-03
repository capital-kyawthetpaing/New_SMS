using System;
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
using Entity;

namespace MasterTorikomi_Item
{
    public partial class MasterTorikomi_Item : Base.Client.FrmMainForm
    {
        ITEM_BL ibl;
        Base_BL bbl;
        MasterTorikomi_SKU_BL mtbl;
        M_SKUInitial_BL msIbl;

        DataTable dtBrand = new DataTable();
        DataTable dtMultiP = new DataTable();
        DataTable dtVendor = new DataTable();
        DataTable dtskuintial = new DataTable();

        public MasterTorikomi_Item()
        {
            InitializeComponent();
        }
        private void MasterTorikomi_Item_Load(object sender, EventArgs e)
        {
            ibl = new ITEM_BL();
            bbl = new Base_BL();
            mtbl = new MasterTorikomi_SKU_BL();
            msIbl = new M_SKUInitial_BL();

            InProgramID = "MasterTorikomi_Item";
            StartProgram();
            FalseKey();
            this.KeyUp += MasterTorikomi_Item_KeyUp;
            RB_all.Focus();

            dtBrand = mtbl.M_Brand_SelectAll_NoPara();
            dtMultiP = mtbl.M_Multipurpose_SelectAll();
            dtVendor = mtbl.M_Vendor_SelectAll();
            dtskuintial = msIbl.M_SKUInitial_SelectAll();

        }

        private void MasterTorikomi_Item_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void FalseKey()
        {
            F2Visible = F3Visible = F4Visible = F5Visible = F7Visible = F8Visible = F9Visible = F10Visible = F11Visible = false;
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
                if (dt != null)
                {
                    if (ErrorCheck(dt))
                    {
                        ExcelErrorCheck(dt);
                        var dtres = dt.Select("ItemCDShow <> ''");
                        if (dtres != null)
                        {
                            gvItem.DataSource = null;
                            gvItem.DataSource = dtres.CopyToDataTable();
                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No row data was found or import excel is opening in different location");
                    return;
                }
            }
            //}

        }
        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck(DataTable dt)
        {
            if (String.IsNullOrEmpty(inputPath.Text))
            {
                MessageBox.Show("E121");
                return false;
            }
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
            dt.Columns.Add("ItemCDShow");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemMakerCD");
            dt.Columns.Add("ItemDate");

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

                if (!Is101("M_Brand", dt.Rows[i]["ブランドCD"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "ブランドCD";
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
                IsNoB(dt, i, "展開サイズ数", "1");

                if (!Is102(dt.Rows[i]["展開カラー数"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "展開カラー数";
                    dt.Rows[i]["Error"] = "E102";
                    goto SkippedLine;
                }
                IsNoB(dt, i, "展開カラー数", "1");

                if (!Is101("M_MultiPorpose", dt.Rows[i]["単位CD"].ToString(), "201"))
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

                if (!Is101("M_MultiPorpose", dt.Rows[i]["セグメントCD"].ToString(), "226"))
                {
                    dt.Rows[i]["EItem"] = "セグメントCD";
                    dt.Rows[i]["Error"] = "E101";
                    goto SkippedLine;
                }
                if (string.IsNullOrWhiteSpace(dt.Rows[i]["セット品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["セット品区分"] = dtskuintial.Rows[0]["SetKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["セット品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "セット品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "セット品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["プレゼント品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["プレゼント品区分"] = dtskuintial.Rows[0]["PresentKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["プレゼント品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "プレゼント品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "プレゼント品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["サンプル品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["サンプル品区分"] = dtskuintial.Rows[0]["SampleKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["サンプル品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "サンプル品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "サンプル品区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["値引商品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["値引商品区分"] = dtskuintial.Rows[0]["DiscountKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["値引商品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "値引商品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "値引商品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["実店舗取扱区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["実店舗取扱区分"] = dtskuintial.Rows[0]["RealStoreFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["実店舗取扱区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "実店舗取扱区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "実店舗取扱区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["在庫管理対象区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["在庫管理対象区分"] = dtskuintial.Rows[0]["ZaikoKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["在庫管理対象区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "在庫管理対象区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "在庫管理対象区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["架空商品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["架空商品区分"] = dtskuintial.Rows[0]["VirtualFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["架空商品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "架空商品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "架空商品区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["直送品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["直送品区分"] = dtskuintial.Rows[0]["DirectFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["直送品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "直送品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "直送品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["予約品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["予約品区分"] = dtskuintial.Rows[0]["ReserveCD"];
                    }
                }
                else if (!Is190(dt.Rows[i]["予約品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "予約品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "予約品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["特記区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["特記区分"] = dtskuintial.Rows[0]["NoticesCD"];
                    }
                }
                else if (!Is190(dt.Rows[i]["特記区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "特記区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "特記区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["送料区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["送料区分"] = dtskuintial.Rows[0]["PostageCD"];
                    }
                }
                else if (!Is190(dt.Rows[i]["送料区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "送料区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "送料区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["要加工品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["要加工品区分"] = dtskuintial.Rows[0]["ManufactCD"];
                    }
                }
                else if (!Is190(dt.Rows[i]["要加工品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "要加工品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "要加工品区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["要確認品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["要確認品区分"] = dtskuintial.Rows[0]["ConfirmCD"];
                    }
                }
                else if (!Is190(dt.Rows[i]["要確認品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "要確認品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                // IsNoB(dt, i, "要確認品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["Web在庫連携区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["Web在庫連携区分"] = dtskuintial.Rows[0]["WebStockFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["Web在庫連携区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Web在庫連携区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "Web在庫連携区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["販売停止品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["販売停止品区分"] = dtskuintial.Rows[0]["StopFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["販売停止品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "販売停止品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "販売停止品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["廃番品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["廃番品区分"] = dtskuintial.Rows[0]["DiscontinueFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["廃番品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "廃番品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "廃番品区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["完売品区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["完売品区分"] = dtskuintial.Rows[0]["SoldoutFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["完売品区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "完売品区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "完売品区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["自社在庫連携対象"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["自社在庫連携対象"] = dtskuintial.Rows[0]["InventoryAddFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["自社在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "自社在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                // IsNoB(dt, i, "自社在庫連携対象", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["メーカー在庫連携対象"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["メーカー在庫連携対象"] = dtskuintial.Rows[0]["MakerAddFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["メーカー在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "メーカー在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "メーカー在庫連携対象", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["店舗在庫連携対象"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["店舗在庫連携対象"] = dtskuintial.Rows[0]["StoreAddFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["店舗在庫連携対象"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "店舗在庫連携対象";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                // IsNoB(dt, i, "店舗在庫連携対象", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["Net発注不可区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["Net発注不可区分"] = dtskuintial.Rows[0]["NoNetOrderFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["Net発注不可区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Net発注不可区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "Net発注不可区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["EDI発注可能区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["EDI発注可能区分"] = dtskuintial.Rows[0]["EDIorderFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["EDI発注可能区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "EDI発注可能区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "EDI発注可能区分", "M_SKUInitial");


                if (string.IsNullOrWhiteSpace(dt.Rows[i]["自動発注対象区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["自動発注対象区分"] = dtskuintial.Rows[0]["AutoOrderFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["自動発注対象区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "自動発注対象区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "自動発注対象区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["カタログ掲載有無"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["カタログ掲載有無"] = dtskuintial.Rows[0]["CatalogFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["カタログ掲載有無"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "カタログ掲載有無";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "カタログ掲載有無", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["小包梱包可能区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["小包梱包可能区分"] = dtskuintial.Rows[0]["ParcelFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["小包梱包可能区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "小包梱包可能区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "小包梱包可能区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["税率区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["税率区分"] = dtskuintial.Rows[0]["TaxRateFLG"];
                    }
                }
                else if (!Is190(dt.Rows[i]["税率区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "税率区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "税率区分", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["原価計算方法"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["原価計算方法"] = dtskuintial.Rows[0]["CostingKBN"];
                    }
                }
                else if (!Is190(dt.Rows[i]["原価計算方法"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "原価計算方法";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "原価計算方法", "M_SKUInitial");

                if (string.IsNullOrWhiteSpace(dt.Rows[i]["Sale対象外区分"].ToString()))
                {
                    if (dtskuintial.Rows.Count > 0)
                    {
                        dt.Rows[i]["Sale対象外区分"] = dtskuintial.Rows[0]["SaleExcludedFlg"];
                    }
                }
                else if (!Is190(dt.Rows[i]["Sale対象外区分"].ToString()))
                {
                    dt.Rows[i]["EItem"] = "Sale対象外区分";
                    dt.Rows[i]["Error"] = "E190";
                    goto SkippedLine;
                }
                //IsNoB(dt, i, "Sale対象外区分", "M_SKUInitial");

                IsNoB(dt, i, "標準原価");

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

                if (!Is101("M_MultiPorpose", dt.Rows[i]["発注注意区分"].ToString(), "316"))
                {
                    dt.Rows[i]["EItem"] = "発注注意区分";
                    dt.Rows[i]["Error"] = "E101";
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

                IsNoB(dt, i, "ITEMタグ2", "1");

            SkippedLine:
                dt.Rows[i]["ItemCDShow"] = dt.Rows[i]["ITEMCD"].ToString();
                dt.Rows[i]["ItemName"] = dt.Rows[i]["商品名"].ToString();
                dt.Rows[i]["ItemDate"] = dt.Rows[i]["改定日"].ToString();
                dt.Rows[i]["ItemMakerCD"] = dt.Rows[i]["メーカー商品CD"].ToString();

                int g = 0;
            }
        }
        private void IsNoB(DataTable dt, int i, string col, string Value = null)
        {
            if (string.IsNullOrEmpty(dt.Rows[i][col].ToString()))
            {
                dt.Rows[i][col] = (Value == null) ? "0" : Value;
            }
        }
        private bool Is103(string date)  // date
        {
            return bbl.CheckDate(bbl.FormatDate(date.Contains(" ") ? date.Split(' ').First() : date));
        }
        private bool Is101(string tableName, string param, string paramID = null)  // Master
        {
            var data = new DataTable();

            if (paramID == null)
            {
                if (tableName == "M_Vendor")
                {
                    string query = " VendorCD = '" + param + "'";
                    var result = dtVendor.Select(query);
                    return (result.Count() > 0);
                    //data = bbl.SimpleSelect1("75", bbl.GetDate(), param);
                }
                if (tableName == "M_Brand")
                {
                    string query = " BrandCD = '" + param + "'";
                    var result = dtBrand.Select(query);
                    return (result.Count() > 0);

                    //data = bbl.SimpleSelect1("56", bbl.GetDate(), param);
                }

                //if (tableName == "M_MultiPorpose")
                //{
                //     data = bbl.SimpleSelect1("14", bbl.GetDate(), param);
                //}

            }
            else if (paramID != null)
            {
                if (tableName == "M_MultiPorpose")
                {
                    string str = " [Key] ='" + param + "'" +
                           "and ID='" + paramID + "'";
                    var result = dtMultiP.Select(str);
                    return (result.Count() > 0);
                    //data = bbl.SimpleSelect1("42", bbl.GetDate(), paramID, param);
                }

                // return (data.Rows.Count > 0);
            }

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
            if (!string.IsNullOrEmpty(value))
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
            F12();
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Cancel();
                    }
                    break;
                case 11:
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        private void F12()
        {
            ibl = new ITEM_BL();
            if (bbl.ShowMessage("Q101") == DialogResult.Yes)
            {
                if (String.IsNullOrEmpty(inputPath.Text))
                {
                    bbl.ShowMessage("E121");
                    return ;
                }
                var dt = gvItem.DataSource as DataTable;
                if (dt == null)
                {
                    MessageBox.Show("Please import first");
                    return;
                }
                //if (ErrorCheck(dt))
                //{
                    if (CheckPartial(dt))
                    {
                    M_ITEM_Entity mie = new M_ITEM_Entity();
                    
                    mie.PC = Environment.MachineName;
                    mie.ProgramID = "MasterTorikomi_Item";
                    mie.ProcessMode = null;
                    mie.Key = inputPath.Text;
                    mie.MainFlg = RB_all.Checked ? "1": RB_BaseInfo.Checked ? "2" : RB_attributeinfo.Checked?"3" :RB_priceinfo.Checked ? "4": RB_Catloginfo.Checked ?"5": RB_tagInfo.Checked ? "6" :"8";
                        var xml = bbl.DataTableToXml(dt);
                    mie.xml1 = xml;

                        var res = ibl.ImportItem(mie);
                        if (res)
                        {
                            bbl.ShowMessage("I101");
                        }
                        else
                        {
                            bbl.ShowMessage("E101");   // Changed please
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fix the error from the imported file.");
                    }
                //}
            }
        }
        private bool CheckPartial(DataTable dt)
        {
            var query = "Error <> ''";
            if (dt.Select(query).Count() > 0)
                return false;
            return true;
        }
        private void Cancel()
        {
            RB_attributeinfo.Checked = RB_BaseInfo.Checked = RB_Catloginfo.Checked = RB_priceinfo.Checked = RB_SizeURL.Checked = RB_tagInfo.Checked = false;
            RB_all.Checked = true;
            inputPath.Clear();
            gvItem.DataSource = null;
            gvItem.Refresh();
        }
    }
}
