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
using System.IO;
using ExcelDataReader;

namespace MasterTorikomi_SKU
{
    public partial class MasterTorikomi_SKU : FrmMainForm
    {
        Base_BL bbl;
        DataTable dtSKU = new DataTable();
        DataTable dtAdmin = new DataTable();
        SKU_BL sbl;

        public MasterTorikomi_SKU()
        {
            InitializeComponent();
            bbl = new Base_BL();
            sbl = new SKU_BL();
        }

       

        private void MasterTorikomi_SKU_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTorikomi_SKU";
            StartProgram();
            RB_all.Checked = true;
            dtSKU = sbl.M_SKU_SelectAll_NOPara();

        }

        private void BT_Torikomi_Click(object sender, EventArgs e)
        {
            if (bbl.ShowMessage("Q001") == DialogResult.Yes)
            {
                string filePath = string.Empty;
                string fileExt = string.Empty;
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
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                    {
                        bbl.ShowMessage("E137");
                        return;
                    }
                    DataTable dt = new DataTable();
                    dt = ExcelToDatatable(filePath);
                    //string[] colname = { "SKUCD", "JANCD", "商品名", "カラーNO", "カラー名", "サイズNO", "サイズ名"};
                    //if (ColumnCheck(colname, dtExcel))
                    //{

                    //}
                    if(dt != null)
                    {
                        if (ErrorCheck(dt))
                        {
                            ExcelErrorCheck(dt);
                            GV_SKU.DataSource = null;
                            GV_SKU.DataSource = dt;
                            bbl.ShowMessage("I101");
                        }

                    }
                }
            }
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
            string kibun = dt.Rows[0]["データ区分"].ToString();
            if (RB_all.Checked)
            {
                if (dt.Columns.Count != 126)
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

            }
            else if (RB_attributeinfo.Checked)
            {
                if (dt.Columns.Count != 73)
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
                
            }
            return true;
        }
        private void ExcelErrorCheck(DataTable dt)
        {
           
            dt.Columns.Add("EItem");
            dt.Columns.Add("Error");
            DataRow dr;
            string kibun = dt.Rows[1]["データ区分"].ToString();
            
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
                    if(dt.Rows[i]["JANCD"].ToString() !="Auto")
                    {
                        String query = " JanCD = '" + dt.Rows[i]["JANCD"].ToString() + "'" +
                            " and SKUCD = '" + dt.Rows[i]["SKUCD"].ToString() + "'";
                      
                        var result = dtSKU.Select(query);
                        if(result.Count() == 0)
                        {
                            
                            dt.Rows[i]["EItem"] = "JANCD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }
                }

                if (!String.IsNullOrEmpty(dt.Rows[i]["削除"].ToString()))
                {

                }

                if (String.IsNullOrEmpty(dt.Rows[i]["商品名"].ToString()))
                {
                    
                    dt.Rows[i]["EItem"] = "商品名";
                    dt.Rows[i]["Error"] = "E102";
                }


              
                if (String.IsNullOrEmpty(dt.Rows[i]["ITEMCD"].ToString()))
                {
                   
                    dt.Rows[i]["EItem"] = "ITEMCD";
                    dt.Rows[i]["Error"] = "E102";
                }
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
                //if (String.IsNullOrEmpty(dt.Rows[i]["カナ名"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                if (RB_all.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["商品情報アドレス"].ToString()))
                    {
                        
                    }
                }
                // if (String.IsNullOrEmpty(dt.Rows[i]["年度"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["シーズン"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["カタログページ"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["カタログページLong"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["指示書番号"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}
                //if (String.IsNullOrEmpty(dt.Rows[i]["指示書発行日"].ToString()))
                //{
                //    bbl.ShowMessage("E102");
                //}

                if (RB_all.Checked || RB_attributeinfo.Checked || RB_BaseInfo.Checked || RB_priceinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["標準原価"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["税込定価"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["税抜定価"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["発注税込価格"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["掛率"].ToString()))
                    {
                        
                    }
                    
                }



                if (RB_all.Checked || RB_BaseInfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["構成数"].ToString()))
                    {
                       
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["発注ロット"].ToString()))
                    {
                      
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["諸口区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["主要仕入先CD"].ToString()))
                    {

                        var dtResult = bbl.SimpleSelect1("28", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["主要仕入先CD"].ToString());
                        if (dtResult.Rows.Count == 0)
                        {
                            
                            dt.Rows[i]["EItem"] = "主要仕入先CD";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["主要仕入先名"].ToString()))
                    {
                       
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["メーカー仕入先CD"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["ブランドCD"].ToString()))
                    {
                       
                    }
                    else
                    {
                        var drB = bbl.SimpleSelect1("56", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["ブランドCD"].ToString());
                        if (drB.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "ブランドCD";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["メーカー商品CD"].ToString()))
                    {
                      
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["メーカー仕入先名"].ToString()))
                    {
                       
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["ブランド名"].ToString()))
                    {
                        
                    }
                   


                    if (!String.IsNullOrEmpty(dt.Rows[i]["単位CD"].ToString()))
                    {
                        var dtTani = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["単位CD"].ToString(), "201");
                        if (dtTani.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "単位CD";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["競技CD"].ToString()))
                    {
                        var dtSp = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["競技CD"].ToString(), "202");
                        if (dtSp.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "競技CD";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }


                    if (!String.IsNullOrEmpty(dt.Rows[i]["分類名"].ToString()))
                    {
                        
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
                        var dtSeg = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["セグメントCD"].ToString(), "226");
                        if (dtSeg.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "セグメントCD";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["発注注意区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["発注注意区分"].ToString(), "316");
                        if (dt1.Rows.Count == 0)
                        {
                            
                            dt.Rows[i]["EItem"] = "発注注意区分";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["発注注意事項"].ToString()))
                    {
                       
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["表示用備考"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["棚番"].ToString()))
                    {
                       
                    }
                }

                if (RB_all.Checked || RB_attributeinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["セット品区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["プレゼント品区分"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["サンプル品区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["値引商品区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Webストア取扱区分"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["実店舗取扱区分"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["実店舗取扱区分名"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["在庫管理対象区分"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["在庫管理対象区分名"].ToString()))
                    {

                    }


                    if (!String.IsNullOrEmpty(dt.Rows[i]["架空商品区分名"].ToString()))
                    {


                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["直送品区分"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["直送品区分名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["予約品区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["予約品区分"].ToString(), "311");
                        if (dt1.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "予約品区分";
                            dt.Rows[i]["Error"] = "E101";
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["特記区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["特記区分"].ToString(), "310");
                        if (dt1.Rows.Count == 0)
                        {
                            
                            dt.Rows[i]["EItem"] = "特記区分";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["特記区分名"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["送料区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["送料区分"].ToString(), "309");
                        if (dt1.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "送料区分";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["送料区分名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["要加工品区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["要加工品区分"].ToString(), "312");
                        if (dt1.Rows.Count == 0)
                        {
                            
                            dt.Rows[i]["EItem"] = "要加工品区分";
                            dt.Rows[i]["Error"] = "E101";


                        }
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["要確認品区分"].ToString()))
                    {
                        var dt1 = bbl.SimpleSelect1("57", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), dt.Rows[i]["要確認品区分"].ToString(), "313");
                        if (dt1.Rows.Count == 0)
                        {
                           
                            dt.Rows[i]["EItem"] = "要確認品区分";
                            dt.Rows[i]["Error"] = "E101";

                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Web在庫連携区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["販売停止品区分名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["廃番品区分"].ToString()))
                    {
                        
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["完売品区分"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["完売品区分名"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自社在庫連携対象"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自社在庫連携対象名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["メーカー在庫連携対象名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["店舗在庫連携対象名"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Net発注不可区分"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Net発注不可区分名"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["EDI発注可能区分"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["EDI発注可能区分名"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自動発注対象区分"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["自動発注対象"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ掲載有無区分"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ掲載有無"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["小包梱包可能区分"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["小包梱包可能"].ToString()))
                    {

                    }

                }


                if (RB_all.Checked || RB_priceinfo.Checked)
                {

                    if (!String.IsNullOrEmpty(dt.Rows[i]["税率区分"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["税率区分名"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["原価計算方法"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["原価計算方法名"].ToString()))
                    {

                    }


                }


                if (RB_all.Checked || RB_Catloginfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["年度"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["シーズン"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ番号"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログページ"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ番号Long"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログページLong"].ToString()))
                    {

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["カタログ情報"].ToString()))
                    {

                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["指示書番号"].ToString()))
                    {

                    }
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


                if (RB_all.Checked || RB_tagInfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ1"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ2"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ3"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ4"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ5"].ToString()))
                    {
                      
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ6"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ7"].ToString()))
                    {
                        
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ8"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ9"].ToString()))
                    {
                       
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["タグ10"].ToString()))
                    {
                        
                    }
                }

               
               

                if (RB_all.Checked || RB_BaseInfo.Checked || RB_JanCD.Checked)
                {

                    if (!String.IsNullOrEmpty(dt.Rows[i]["カナ名"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["略名"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["英語名"].ToString()))
                    {
                    }
                }
                if (RB_all.Checked || RB_attributeinfo.Checked || RB_priceinfo.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Sale対象外区分"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Sale対象外区分名"].ToString()))
                    {
                    }
                }
                if (RB_SizeURL.Checked)
                {
                    if (!String.IsNullOrEmpty(dt.Rows[i]["APIKey"].ToString()))
                    {
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["サイト商品CD"].ToString()))
                    {
                    }
                }


            }
           
        }


      private  DataTable ExcelToDatatable(string filePath)
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

        private void RB_attributeinfo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RB_priceinfo_CheckedChanged(object sender, EventArgs e)
        {

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
    }
}
