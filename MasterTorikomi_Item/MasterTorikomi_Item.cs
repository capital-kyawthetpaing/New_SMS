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
//using System.Threading;
using System.Threading.Tasks;

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
        DataTable dtMessage = new DataTable();
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
            dtMessage = msIbl.M_MessageSelectAll();
            this.ModeVisible = false;
            this.Text = "Itemマスター取込	";
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
                
                Cursor = Cursors.WaitCursor;
                try
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
                            //var dtres = dt.Select("ItemCDShow <> ''");
                            if (dt != null)
                            {
                                gvItem.DataSource = null;
                                gvItem.DataSource = dt;
                                //Cursor = Cursors.WaitCursor;
                            }

                        }
                        else
                        {
                            inputPath.Focus();
                        }
                        Cursor = Cursors.Default; ;
                    }
                    else
                    {
                        MessageBox.Show("No row data was found or import excel is opening in different location");
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message);
                }

                Cursor = Cursors.Default;
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
        private async void TestAsyncAwaitMethods(string val)
        {
            await LongRunningMethod(val);
        }
        private  void Run1(string val)
        {
            label2.Text = "Start";
            label2.Text = val + " of " + maxCount;
           // Task.Delay(1);
            Task.WhenAll();
        }

        private async Task LongRunningMethod(string val)
        {
            //  var tasks = new List<Task>();
            //  var f= new  Task.Run(Run1(val));
            //tasks.Add(DoIoBoundWorkAsync());
            //tasks.Add(DoCpuBoundWorkAsync());
            //tasks.Add(DoSomeOtherWorkAsync());

            //await Task.WhenAll(tasks).ConfigureAwait(false);
            //await  Task.Run(() => Run1(val));
            //  //label2.Text = " Starting ...";
            await Task.Run(() =>
            {
               // label2.Text = val + " of " + maxCount;
            });
            //  //label2.Text = "End...";
            //  return 1;
        }
        public async Task RunAsync()
        {
            var tasks = new List<Task>();

            foreach (var x in new[] { 1, 2, 3 })
            {
                //var task = Run1(x);
                //tasks.Add(task);
            }

            await Task.WhenAll();
        }
        string  maxCount = "";
        private async  void ExcelErrorCheck(DataTable dt)
        {
          //  tick = 0;

            dt.Columns.Add("EItem");
            dt.Columns.Add("Error");
            dt.Columns.Add("ItemCDShow");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemMakerCD");
            dt.Columns.Add("ItemDate");
            string currentType =  RB_all.Checked ? "1" : RB_BaseInfo.Checked ? "2" : RB_attributeinfo.Checked ? "3" : RB_priceinfo.Checked ? "4" : RB_Catloginfo.Checked ? "5" : RB_tagInfo.Checked ? "6" : "8";
            string[] Cols = GetCurrentType(Convert.ToInt32(currentType)); 
            string kibun = dt.Rows[1]["データ区分"].ToString();
            label2.Visible = true;
            var cou = dt.Rows.Count.ToString() + "";
            maxCount = cou;
            int cc = 0;
           // await Task.Run(() =>
           //{
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   cc++;

                   try
                   { //tick = i;
                        if (cc == 100)
                       {
                      // Run1(i.ToString());
                            //await   Run1(i.ToString()).ConfigureAwait(false);
                            //await Task.Run(() =>
                            //{
                            //    label2.Text = i.ToString() + " of " + maxCount;
                            //});

                            //Task task = new Task(TestAsyncAwaitMethods("")));
                            //task.Start();
                            //task.Wait();
                            // await LongRunningMethod(i.ToString());
                        }
                   }

                   catch { }
                   if (cc == 20)
                   {
                       cc = 0;
                   }
                   try
                   {
                       if (Cols.Contains("データ区分"))
                       {
                           if (!Is102(dt.Rows[i]["データ区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "データ区分";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                           if (!Is190(dt.Rows[i]["データ区分"].ToString(), true))
                           {
                               dt.Rows[i]["EItem"] = "データ区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("ITEMCD"))
                       {
                           if (!Is102(dt.Rows[i]["ITEMCD"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "ITEMCD";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("改定日"))
                       {
                           if (!Is102(dt.Rows[i]["改定日"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "改定日";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("改定日"))
                       {
                           if (!Is103(dt.Rows[i]["改定日"].ToString()))
                           {

                               dt.Rows[i]["EItem"] = "改定日";
                               dt.Rows[i]["Error"] = "E103";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("承認日"))
                       {
                           if (!Is103(dt.Rows[i]["承認日"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "承認日";
                               dt.Rows[i]["Error"] = "E103";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("削除"))
                       {
                           if (!Is190(dt.Rows[i]["削除"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "削除";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }

                           IsNoB(dt, i, "削除");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("諸口区分"))
                       {
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
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("商品名"))
                       {
                           if (!Is102(dt.Rows[i]["商品名"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "商品名";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("略名"))
                       {
                           if (!Is102(dt.Rows[i]["略名"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "略名";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("主要仕入先CD"))
                       {
                           if (!Is101("M_Vendor", dt.Rows[i]["主要仕入先CD"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "主要仕入先CD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("ブランドCD"))
                       {
                           if (!Is101("M_Brand", dt.Rows[i]["ブランドCD"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "ブランドCD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("メーカー商品CD"))
                       {
                           if (!Is102(dt.Rows[i]["メーカー商品CD"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "メーカー商品CD";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("展開サイズ数"))
                       {
                           if (!Is102(dt.Rows[i]["展開サイズ数"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "展開サイズ数";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                           IsNoB(dt, i, "展開サイズ数", "1");
                       }
                   }
                   catch { }
                   try
                   {

                       if (Cols.Contains("展開カラー数"))
                       {
                           if (!Is102(dt.Rows[i]["展開カラー数"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "展開カラー数";
                               dt.Rows[i]["Error"] = "E102";
                               goto SkippedLine;
                           }
                           IsNoB(dt, i, "展開カラー数", "1");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("単位CD"))
                       {
                           if (!Is101("M_MultiPorpose", dt.Rows[i]["単位CD"].ToString(), "201"))
                           {
                               dt.Rows[i]["EItem"] = "単位CD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("競技CD"))
                       {
                           if (!Is101("M_MultiPorpose", dt.Rows[i]["競技CD"].ToString(), "202"))
                           {
                               dt.Rows[i]["EItem"] = "競技CD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("商品分類CD"))
                       {
                           if (!Is101("M_MultiPorpose", dt.Rows[i]["商品分類CD"].ToString(), "203"))
                           {
                               dt.Rows[i]["EItem"] = "商品分類CD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("セグメントCD"))
                       {
                           if (!Is101("M_MultiPorpose", dt.Rows[i]["セグメントCD"].ToString(), "226"))
                           {
                               dt.Rows[i]["EItem"] = "セグメントCD";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }



                   try
                   {
                       if (Cols.Contains("セグメントCD"))
                       {
                           if (!Is190(dt.Rows[i]["セット品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "セット品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["セット品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["セット品区分"] = dtskuintial.Rows[0]["SetKBN"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("プレゼント品区分"))
                       {
                           if (!Is190(dt.Rows[i]["プレゼント品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "プレゼント品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "セット品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["プレゼント品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["プレゼント品区分"] = dtskuintial.Rows[0]["PresentKBN"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("サンプル品区分"))
                       {
                           if (!Is190(dt.Rows[i]["サンプル品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "サンプル品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "プレゼント品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["サンプル品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["サンプル品区分"] = dtskuintial.Rows[0]["SampleKBN"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("値引商品区分"))
                       {
                           if (!Is190(dt.Rows[i]["値引商品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "値引商品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "サンプル品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["値引商品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val) || val == "0")
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["値引商品区分"] = dtskuintial.Rows[0]["DiscountKBN"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("実店舗取扱区分"))
                       {
                           if (!Is190(dt.Rows[i]["実店舗取扱区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "実店舗取扱区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "値引商品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["実店舗取扱区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["EItem"] = "実店舗取扱区分";
                                   dt.Rows[i]["Error"] = "E190";
                                   dt.Rows[i]["実店舗取扱区分"] = dtskuintial.Rows[0]["RealStoreFlg"];
                                   goto SkippedLine;
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("在庫管理対象区分"))
                       {
                           if (!Is190(dt.Rows[i]["在庫管理対象区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "在庫管理対象区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "実店舗取扱区分", "M_SKUInitial");
                            var val = dt.Rows[i]["在庫管理対象区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["在庫管理対象区分"] = dtskuintial.Rows[0]["ZaikoKBN"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("架空商品区分"))
                       {
                           if (!Is190(dt.Rows[i]["架空商品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "架空商品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "在庫管理対象区分", "M_SKUInitial");
                            var val = dt.Rows[i]["架空商品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["架空商品区分"] = dtskuintial.Rows[0]["VirtualFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("直送品区分"))
                       {
                            //IsNoB(dt, i, "架空商品区分", "M_SKUInitial");
                            if (!Is190(dt.Rows[i]["直送品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "直送品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["直送品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["直送品区分"] = dtskuintial.Rows[0]["DirectFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("予約品区分"))
                       {
                           if (!Is190(dt.Rows[i]["予約品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "予約品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "直送品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["予約品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["予約品区分"] = dtskuintial.Rows[0]["ReserveCD"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("特記区分"))
                       {
                           if (!Is190(dt.Rows[i]["特記区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "特記区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "予約品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["特記区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["特記区分"] = dtskuintial.Rows[0]["NoticesCD"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("送料区分"))
                       {
                           if (!Is190(dt.Rows[i]["送料区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "送料区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "特記区分", "M_SKUInitial");
                            var val = dt.Rows[i]["送料区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["送料区分"] = dtskuintial.Rows[0]["PostageCD"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("要加工品区分"))
                       {
                           if (!Is190(dt.Rows[i]["要加工品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "要加工品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "送料区分", "M_SKUInitial");
                            var val = dt.Rows[i]["要加工品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["要加工品区分"] = dtskuintial.Rows[0]["ManufactCD"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("要確認品区分"))
                       {
                            //IsNoB(dt, i, "要加工品区分", "M_SKUInitial");
                            if (!Is190(dt.Rows[i]["要確認品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "要確認品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["要確認品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["要確認品区分"] = dtskuintial.Rows[0]["ConfirmCD"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("Web在庫連携区分"))
                       {
                           if (!Is190(dt.Rows[i]["Web在庫連携区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "Web在庫連携区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            // IsNoB(dt, i, "要確認品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["Web在庫連携区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["Web在庫連携区分"] = dtskuintial.Rows[0]["WebStockFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("販売停止品区分"))
                       {
                           if (!Is190(dt.Rows[i]["販売停止品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "販売停止品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "Web在庫連携区分", "M_SKUInitial");
                            var val = dt.Rows[i]["販売停止品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["販売停止品区分"] = dtskuintial.Rows[0]["StopFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("廃番品区分"))
                       {
                           if (!Is190(dt.Rows[i]["廃番品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "廃番品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "販売停止品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["廃番品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["廃番品区分"] = dtskuintial.Rows[0]["DiscontinueFlg"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("完売品区分"))
                       {
                           if (!Is190(dt.Rows[i]["完売品区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "完売品区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "廃番品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["完売品区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["完売品区分"] = dtskuintial.Rows[0]["SoldoutFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("自社在庫連携対象"))
                       {
                           if (!Is190(dt.Rows[i]["自社在庫連携対象"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "自社在庫連携対象";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "完売品区分", "M_SKUInitial");
                            var val = dt.Rows[i]["自社在庫連携対象"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["自社在庫連携対象"] = dtskuintial.Rows[0]["InventoryAddFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("メーカー在庫連携対象"))
                       {
                           if (!Is190(dt.Rows[i]["メーカー在庫連携対象"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "メーカー在庫連携対象";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["メーカー在庫連携対象"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["メーカー在庫連携対象"] = dtskuintial.Rows[0]["MakerAddFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("店舗在庫連携対象"))
                       {
                           if (!Is190(dt.Rows[i]["店舗在庫連携対象"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "店舗在庫連携対象";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "メーカー在庫連携対象", "M_SKUInitial");
                            var val = dt.Rows[i]["店舗在庫連携対象"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["店舗在庫連携対象"] = dtskuintial.Rows[0]["StoreAddFlg"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("Net発注不可区分"))
                       {
                           if (!Is190(dt.Rows[i]["Net発注不可区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "Net発注不可区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            // IsNoB(dt, i, "店舗在庫連携対象", "M_SKUInitial");
                            var val = dt.Rows[i]["Net発注不可区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["Net発注不可区分"] = dtskuintial.Rows[0]["NoNetOrderFlg"];
                               }
                           }

                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("EDI発注可能区分"))
                       {
                           if (!Is190(dt.Rows[i]["EDI発注可能区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "EDI発注可能区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "Net発注不可区分", "M_SKUInitial");
                            var val = dt.Rows[i]["EDI発注可能区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["EDI発注可能区分"] = dtskuintial.Rows[0]["EDIorderFlg"];
                               }
                           }
                       }

                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("自動発注対象区分"))
                       {
                            //IsNoB(dt, i, "EDI発注可能区分", "M_SKUInitial");
                            if (!Is190(dt.Rows[i]["自動発注対象区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "自動発注対象区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["自動発注対象区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["自動発注対象区分"] = dtskuintial.Rows[0]["AutoOrderFlg"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("カタログ掲載有無"))
                       {
                           if (!Is190(dt.Rows[i]["カタログ掲載有無"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "カタログ掲載有無";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "自動発注対象区分", "M_SKUInitial");
                            var val = dt.Rows[i]["カタログ掲載有無"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["カタログ掲載有無"] = dtskuintial.Rows[0]["CatalogFlg"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("小包梱包可能区分"))
                       {
                           if (!Is190(dt.Rows[i]["小包梱包可能区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "小包梱包可能区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "カタログ掲載有無", "M_SKUInitial");
                            var val = dt.Rows[i]["小包梱包可能区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["小包梱包可能区分"] = dtskuintial.Rows[0]["ParcelFlg"];
                               }
                           }

                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("税率区分"))
                       {
                           if (!Is190(dt.Rows[i]["税率区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "税率区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "小包梱包可能区分", "M_SKUInitial");
                            var val = dt.Rows[i]["税率区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["税率区分"] = dtskuintial.Rows[0]["TaxRateFLG"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("原価計算方法"))
                       {
                           if (!Is190(dt.Rows[i]["原価計算方法"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "原価計算方法";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                            //IsNoB(dt, i, "税率区分", "M_SKUInitial");
                            var val = dt.Rows[i]["原価計算方法"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["原価計算方法"] = dtskuintial.Rows[0]["CostingKBN"];
                               }
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("Sale対象外区分"))
                       {
                            //IsNoB(dt, i, "原価計算方法", "M_SKUInitial");
                            if (!Is190(dt.Rows[i]["Sale対象外区分"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "Sale対象外区分";
                               dt.Rows[i]["Error"] = "E190";
                               goto SkippedLine;
                           }
                           var val = dt.Rows[i]["Sale対象外区分"].ToString();
                           if (string.IsNullOrWhiteSpace(val))
                           {
                               if (dtskuintial.Rows.Count > 0)
                               {
                                   dt.Rows[i]["Sale対象外区分"] = dtskuintial.Rows[0]["SaleExcludedFlg"];
                               }
                           }
                       }
                   }
                   catch { }


                   try
                   {
                        //IsNoB(dt, i, "Sale対象外区分", "M_SKUInitial");
                        if (Cols.Contains("標準原価"))
                       {
                           IsNoB(dt, i, "標準原価");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("税込定価"))
                       {
                           IsNoB(dt, i, "税込定価");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("税抜定価"))
                       {
                           IsNoB(dt, i, "税抜定価");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("発注税込価格"))
                       {
                           IsNoB(dt, i, "発注税込価格");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("発注税抜価格"))
                       {
                           IsNoB(dt, i, "発注税抜価格");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("掛率"))
                       {
                           IsNoB(dt, i, "掛率");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("発売開始日"))
                       {
                           if (!Is103(dt.Rows[i]["発売開始日"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "発売開始日";
                               dt.Rows[i]["Error"] = "E103";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("Web掲載開始日"))
                       {
                           if (!Is103(dt.Rows[i]["Web掲載開始日"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "Web掲載開始日";
                               dt.Rows[i]["Error"] = "E103";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("発注注意区分"))
                       {
                           if (!Is101("M_MultiPorpose", dt.Rows[i]["発注注意区分"].ToString(), "316"))
                           {
                               dt.Rows[i]["EItem"] = "発注注意区分";
                               dt.Rows[i]["Error"] = "E101";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("指示書発行日"))
                       {
                           if (!Is103(dt.Rows[i]["指示書発行日"].ToString()))
                           {
                               dt.Rows[i]["EItem"] = "指示書発行日";
                               dt.Rows[i]["Error"] = "E103";
                               goto SkippedLine;
                           }
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("ITEMCD"))
                       {
                           IsNoB(dt, i, "掛率", dt.Rows[i]["ITEMCD"].ToString());
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("発注ロット"))
                       {
                           IsNoB(dt, i, "発注ロット", "1");
                       }
                   }
                   catch { }
                   try
                   {
                       if (Cols.Contains("ITEMタグ2"))
                       {
                           IsNoB(dt, i, "ITEMタグ2", "1");
                       }
                   }
                   catch { }

               SkippedLine:
                   dt.Rows[i]["ItemCDShow"] = dt.Rows[i]["ITEMCD"].ToString();
                   dt.Rows[i]["ItemName"] = dt.Rows[i]["商品名"].ToString();
                   dt.Rows[i]["ItemDate"] = dt.Rows[i]["改定日"].ToString();
                   try
                   {
                       dt.Rows[i]["ItemMakerCD"] = dt.Rows[i]["メーカー商品CD"].ToString();
                   }
                   catch { }
                   int g = 0;
               }
        //   });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
              
                var row = dt.Rows[i]["Error"].ToString();
                if (!string.IsNullOrEmpty(row) && row != "")
                {
                    var msg = "";
                    var query="";
                    try
                    {
                        query = "MessageID = '" + row + "'";
                    }
                    catch { }
                    if (dtMessage.Select(query) != null)
                    {
                        msg = dtMessage.Select(query).CopyToDataTable().Rows[0]["MessageText1"].ToString();
                        dt.Rows[i]["Error"] = msg;
                    }
                }
            }
            try
            {
               // timer1.Stop();
                label2.Visible = false;
                
            }
            catch { }
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
                if (string.IsNullOrEmpty(param.Trim()) || param.Trim() == "" )
                {
                    return true;
                }
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
                if (string.IsNullOrEmpty(param.Trim()) || param.Trim() == "")
                {
                    return true;
                }
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
        private bool Is190(string value, bool IsDataFlag = false)
        {
            if (!IsDataFlag)
            {
                if (value.Trim().ToString() == "0" || value.Trim().ToString() == "1" || string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            else
            {
                var val = RB_all.Checked ? "1" : RB_BaseInfo.Checked ? "2" : RB_attributeinfo.Checked ? "3" : RB_priceinfo.Checked ? "4" : RB_Catloginfo.Checked ? "5" : RB_tagInfo.Checked ? "6" : "8";
                if (value.Trim() == val)
                {
                    return true;
                }
            }
            return false;
        }

        private bool Is102(string value)
        {
            if (!string.IsNullOrEmpty(value.Trim()))
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
            try { 

            ibl = new ITEM_BL();
                if (bbl.ShowMessage("Q101") == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    if (String.IsNullOrEmpty(inputPath.Text))
                    {
                        bbl.ShowMessage("E121");
                        inputPath.Focus();
                        Cursor = Cursors.Default;
                        return;
                    }
                    var dt = gvItem.DataSource as DataTable;
                    if (dt == null)
                    {
                        MessageBox.Show("Please import first");
                        Cursor = Cursors.Default;
                        return;
                    }
                    //if (ErrorCheck(dt))
                    //{
                    if (CheckPartial(dt))
                    {
                        M_ITEM_Entity mie = new M_ITEM_Entity();
                        mie.Operator = InOperatorCD;
                        mie.PC = Environment.MachineName;
                        mie.ProgramID = "MasterTorikomi_Item";
                        mie.ProcessMode = null;
                        mie.Key = inputPath.Text;
                        mie.MainFlg = RB_all.Checked ? "1" : RB_BaseInfo.Checked ? "2" : RB_attributeinfo.Checked ? "3" : RB_priceinfo.Checked ? "4" : RB_Catloginfo.Checked ? "5" : RB_tagInfo.Checked ? "6" : "8";
                        var xml = bbl.DataTableToXml(dt);
                        mie.xml1 = xml;

                        var res = ibl.ImportItem(mie);
                        if (res)
                        {
                            bbl.ShowMessage("I101");
                        }
                        else
                        {
                            MessageBox.Show("Failed to Import");   // Changed please
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fix the error of the imported file shown on screen and then try to import. . .");
                    }
                }
                //}
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Default;
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
            Cursor = Cursors.WaitCursor;

            try
            {
                RB_attributeinfo.Checked = RB_BaseInfo.Checked = RB_Catloginfo.Checked = RB_priceinfo.Checked = RB_SizeURL.Checked = RB_tagInfo.Checked = false;
                RB_all.Checked = true;
                inputPath.Clear();
                gvItem.DataSource = null;
                gvItem.Refresh();
                dtBrand = mtbl.M_Brand_SelectAll_NoPara();
                dtMultiP = mtbl.M_Multipurpose_SelectAll();
                dtVendor = mtbl.M_Vendor_SelectAll();
                dtskuintial = msIbl.M_SKUInitial_SelectAll();
                dtMessage = msIbl.M_MessageSelectAll();
            }
            catch {

            }
            Cursor = Cursors.Default; ;
        }

        private string[] GetCurrentType(int a= 0)
        {
            string[] val = null;
            if (a == 1)
            {
                val = new string[(int)Subete.Count];
                foreach (var attribute in (Subete[])Enum.GetValues(typeof(Subete)))
                {
                    if (attribute != Subete.Count)
                    {
                        val[(int)attribute] =  attribute.ToString();
                    }
                }
            }
            else if (a == 2)
            {
                val = new string[(int)Kihon.Count];
                foreach (var attribute in (Kihon[])Enum.GetValues(typeof(Kihon)))
                {
                    if (attribute != Kihon.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }
            else if (a == 3)
            {
                val = new string[(int)Zokusei.Count];
                foreach (var attribute in (Zokusei[])Enum.GetValues(typeof(Zokusei)))
                {
                    if (attribute != Zokusei.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }
            else if (a == 4)
            {
                val = new string[(int)Kakaku.Count];
                foreach (var attribute in (Kakaku[])Enum.GetValues(typeof(Kakaku)))
                {
                    if (attribute != Kakaku.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }
            else if (a == 5)
            {
                val = new string[(int)Kataroku.Count];
                foreach (var attribute in (Kataroku[])Enum.GetValues(typeof(Kataroku)))
                {
                    if (attribute != Kataroku.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }

            else if (a == 6)
            {
                val = new string[(int)Taggu.Count];
                foreach (var attribute in (Taggu[])Enum.GetValues(typeof(Taggu)))
                {
                    if (attribute != Taggu.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }
            else if (a == 8)
            {
                val = new string[(int)SaitoURL.Count];
                foreach (var attribute in (SaitoURL[])Enum.GetValues(typeof(SaitoURL)))
                {
                    if (attribute != SaitoURL.Count)
                    {
                        val[(int)attribute] = attribute.ToString();
                    }
                }
            }
            return val;
        }

        private enum SaitoURL : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 商品名, 商品情報アドレス, Count

        }
        private enum Taggu : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 商品名, ITEMタグ1, ITEMタグ2, ITEMタグ3, ITEMタグ4, ITEMタグ5, ITEMタグ6, ITEMタグ7, ITEMタグ8, ITEMタグ9, ITEMタグ10, Count

        }
        private enum Kataroku : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 商品名, 年度, シーズン, カタログ番号, カタログページ, カタログ情報, 指示書番号, 指示書発行日, Count
        }
        private enum Kakaku : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 商品名, 税率区分, 税率区分名, 原価計算方法, 原価計算方法名, Sale対象外区分, Sale対象外区分名, 標準原価, 税込定価, 税抜定価, 主要仕入先CD, 主要仕入先名
                , 発注税込価格, 発注税抜価格, 掛率
                , Count
        }
        private enum Zokusei : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 商品名, セット品区分, セット品区分名, プレゼント品区分, プレゼント品区分名, サンプル品区分, サンプル品区分名, 値引商品区分, 値引商品区分名, Webストア取扱区分
                , Webストア取扱区分名, 実店舗取扱区分, 実店舗取扱区分名, 在庫管理対象区分, 在庫管理対象区分名, 架空商品区分, 架空商品区分名, 直送品区分, 直送品区分名, 予約品区分名, 予約品区分, 特記区分, 特記区分名
                , 送料区分, 送料区分名, 要加工品区分, 要加工品区分名, 要確認品区分, 要確認品区分名, Web在庫連携区分, Web在庫連携区分名, 販売停止品区分, 販売停止品区分名, 廃番品区分, 廃番品区分名, 完売品区分, 完売品区分名
                , 自社在庫連携対象, 自社在庫連携対象名, メーカー在庫連携対象, メーカー在庫連携対象名, 店舗在庫連携対象, 店舗在庫連携対象名, Net発注不可区分, Net発注不可区分名, EDI発注可能区分, EDI発注可能区分名, 自動発注対象区分
                , 自動発注対象名, カタログ掲載有無, カタログ掲載有無名, 小包梱包可能区分, 小包梱包可能名, Sale対象外区分, Sale対象外区分名, 標準原価, 税込定価, 税抜定価, 発注税込価格, 発注税抜価格, 掛率
                , Count
        }
        private enum Kihon : int
        {
            データ区分, ITEMCD, 改定日, 承認日, 削除, 諸口区分, 商品名, カナ名, 略名, 英語名, 主要仕入先CD, 主要仕入先名, ブランドCD, ブランド名, メーカー商品CD, 展開サイズ数, 展開カラー数, 単位CD, 単位名, 競技CD
                , 競技名, 商品分類CD, 分類名, セグメントCD, セグメント名, 標準原価, 税込定価, 税抜定価, 発注税込価格, 発注税抜価格, 掛率, 発売開始日, Web掲載開始日, 発注注意区分, 発注注意区分名, 発注注意事項, 管理用備考
                , 表示用備考, 棚番
                //, 発注ロット
                , Count

        }
        private enum Subete : int
        {
            データ区分
                , ITEMCD
                , 改定日
                , 承認日
                , 削除
                , 諸口区分
                , 商品名
                , カナ名
                , 略名
                , 英語名
                , 主要仕入先CD
                , 主要仕入先名
                , ブランドCD
                , ブランド名
                , メーカー商品CD
                , 展開サイズ数
                , 展開カラー数
                , 単位CD
                , 単位名
                , 競技CD
                , 競技名
                , 商品分類CD
                , 分類名
                , セグメントCD
                , セグメント名
                , セット品区分
                , セット品区分名
                , プレゼント品区分
                , プレゼント品区分名
                , サンプル品区分
                , サンプル品区分名
                , 値引商品区分
                , 値引商品区分名
                , Webストア取扱区分
                , Webストア取扱区分名
                , 実店舗取扱区分
                , 実店舗取扱区分名
                , 在庫管理対象区分
                , 在庫管理対象区分名
                , 架空商品区分
                , 架空商品区分名
                , 直送品区分
                , 直送品区分名
                , 予約品区分
                , 予約品区分名
                , 特記区分
                , 特記区分名
                , 送料区分
                , 送料区分名
                , 要加工品区分
                , 要加工品区分名
                , 要確認品区分
                , 要確認品区分名
                , Web在庫連携区分
                , Web在庫連携区分名
                , 販売停止品区分
                , 販売停止品区分名
                , 廃番品区分
                , 廃番品区分名
                , 完売品区分
                , 完売品区分名
                , 自社在庫連携対象
                , 自社在庫連携対象名
                , メーカー在庫連携対象
                , メーカー在庫連携対象名
                , 店舗在庫連携対象
                , 店舗在庫連携対象名
                , Net発注不可区分
                , Net発注不可区分名
                , EDI発注可能区分
                , EDI発注可能区分名
                , 自動発注対象区分
                , 自動発注対象名
                , カタログ掲載有無
                , カタログ掲載有無名
                , 小包梱包可能区分
                , 小包梱包可能名
                , 税率区分
                , 税率区分名
                , 原価計算方法
                , 原価計算方法名
                , Sale対象外区分
                , Sale対象外区分名
                , 標準原価
                , 税込定価
                , 税抜定価
                , 発注税込価格
                , 発注税抜価格
                , 掛率
                , 発売開始日
                , Web掲載開始日
                , 発注注意区分
                , 発注注意区分名
                , 発注注意事項
                , 管理用備考
                , 表示用備考
                , 棚番
                , 年度
                , シーズン
                , カタログ番号
                , カタログページ
                , カタログ情報
                , 指示書番号
                , 指示書発行日
                , 商品情報アドレス
                , 発注ロット
                , ITEMタグ1
                , ITEMタグ2
                , ITEMタグ3
                , ITEMタグ4
                , ITEMタグ5
                , ITEMタグ6
                , ITEMタグ7
                , ITEMタグ8
                , ITEMタグ9
                , ITEMタグ10
          , Count
        }

        private void RB_all_CheckedChanged(object sender, EventArgs e)
        {
            inputPath.Clear();
        }
        int tick = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            // tick++;
           // label2.Text = tick.ToString(); ;
        }

        private void gvItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
