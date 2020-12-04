using Base.Client;
using BL;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DL;
using System.Threading;
using System.Diagnostics;

namespace TempoRegiTsurisenJyunbi
{
    public partial class frmTempoRegiTsurisenJyunbi : ShopBaseForm
    {
        TempoRegiTsurisenJyunbi_BL trtjb;
        D_DepositHistory_Entity mre;
        string storeCD;
        DataTable dtDepositNO;
        int DayNow;
        public frmTempoRegiTsurisenJyunbi()
        {
            Start_Display();
            InitializeComponent();
            dtDepositNO = new DataTable();
            trtjb = new TempoRegiTsurisenJyunbi_BL();
            mre = new D_DepositHistory_Entity();
        }
        //void button1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Down:
        //        case Keys.Up:
        //            if (button1.ContextMenuStrip != null)
        //            {
        //                button1.ContextMenuStrip.Show(button1,
        //                    new Point(0, button1.Height), ToolStripDropDownDirection.BelowRight);
        //            }
        //            break;
        //    }
        //}
        private void button1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
            }
        }
        private void frmTempoRegiTsurisenJyunbi_Load(object sender, EventArgs e)
        {
            
            trtjb = new TempoRegiTsurisenJyunbi_BL();
            InProgramID = "TempoRegiTsurisenJyunbi";

            StartProgram();
            this.Text = "釣銭準備入力";
            SetRequireField();
            storeCD = StoreCD;
            DateTime tomorrow = DateTime.Now.AddDays(1);
            txtDate.Text = tomorrow.Date.ToString("yyyy/MM/dd");
            Stop_DisplayService();
        }
        public bool CheckDate()
        {
            string ymd = bbl.GetDate();
            //txtDate.Text = ymd;
            DateTime target = DateTime.Parse(txtDate.Text);
            //DateTime today = DateTime.Today;
            DateTime yesterday = DateTime.Now.AddDays(-1);
            if(target<=yesterday)
            {
                trtjb.ShowMessage("E103");
                return false;
            }
            return true;
        }
        private void SetRequireField()
        {
            txtDate.Require(true);
            DepositGaku.Require(true);
        }
        public override void FunctionProcess(int index)
        {

            switch (index + 1)
            {
                case 2:
                    Save();
                    break;
            }
        }
        private D_DepositHistory_Entity DepositHistoryEnity()
        {
            mre = new D_DepositHistory_Entity
            {
                DataKBN = "3",
                DepositKBN = "6",
                CancelKBN = "0",
                RecoredKBN = "0",
                Rows = "0",
                AdminNO = "",
                //AccountingDate = System.DateTime.Now.ToShortDateString(),
                AccountingDate=txtDate.Text,
                SalesTaxRate = "0",
                DepositGaku = DepositGaku.Text,
                DenominationCD = "",
                Remark = Remark.Text,
                ExchangeMoney = "0",
                ExchangeDenomination = "",
                ExchangeCount = "0",
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                TotalGaku = "0",
                Refund = "0",
                IsIssued = "0",
                StoreCD = storeCD,
                CustomerCD="",
                DiscountGaku="0",
                ProperGaku="0",
                Operator = InOperatorCD,
                ProcessMode = "登録",
                ProgramID = InProgramID,
                PC = InPcID,
                Key = storeCD + " " + DepositGaku.Text
            };
            return mre;
        }
        /// <summary>
        /// 登録ボタンを押下時データベースにInsertする
        /// </summary>
        public void Save()
        {

            if (ErrorCheck())
            {
                {
                    if (trtjb.ShowMessage("Q101") == DialogResult.Yes)
                    {
                        //DataTable dt = new DataTable();
                        //dt = trtjb.SimpleSelect1("70", ChangeDate.Replace("/", "-"), storeCD, txtDate.Text);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    trtjb.ShowMessage("E252");
                        //}
                        //DataCheck();
                        mre = DepositHistoryEnity();
                        if (trtjb.TempoRegiTsurisenJyunbi_Insert_Update(mre))
                        {
                            if (Base_DL.iniEntity.IsDM_D30Used)
                            {
                                RunConsole();
                            }
                            else
                            {
                                trtjb.ShowMessage("I101");
                            }
                            //trtjb.ShowMessage("I101");
                            //RunConsole();
                            txtDate.Clear();
                            DepositGaku.Clear();
                            Remark.Clear();
                            txtDate.Focus();
                        }
                        else
                        {
                            trtjb.ShowMessage("S001");
                        }
                    }
                    else
                    {
                        DepositGaku.Focus();
                    }
                }
            }
        }
        //public bool DataCheck()
        //{
        //    DataTable dt = new DataTable();
        //    dt = trtjb.SimpleSelect1("71", ChangeDate.Replace("/", "-"), storeCD, txtDate.Text);
        //    if (dt.Rows.Count > 0)
        //    {
        //        trtjb.ShowMessage("E252");
        //        return false;
        //    }
        //    return true;
        //}
        protected override void EndSec()
        {
            RunDisplay_Service();
            this.Close();
        }
        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtDate,DepositGaku}))   // go that focus
                return false;
            DataTable dt = new DataTable();
            dt = trtjb.SimpleSelect1("71", ChangeDate.Replace("/", "-"), storeCD, txtDate.Text);
            if (dt.Rows.Count > 0)
            {
                trtjb.ShowMessage("E252");
                txtDate.Focus();
                return false;
            }
            return true;
        }
        private void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtDate.Text))
                {
                    trtjb.ShowMessage("E102");
                }
                else
                {
                    CheckDate();
                }
            }
        }
        protected void CDO_Open()
        {
            if (Base_DL.iniEntity.IsDM_D30Used)
            {
                CashDrawerOpen op = new CashDrawerOpen();  //2020_06_24 
                op.OpenCashDrawer(); //2020_06_24     << PTK
            }
        }
        protected void Printer_Open(string filePath, string programID, string cmdLine)
        {
            try
            {
                ////////try
                ////////{
                ////////    cdo.RemoveDisplay(true);
                ////////    cdo.RemoveDisplay(true);
                ////////}
                ////////catch { }
                // System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
               /// pro.WaitForExit();
                //////try
                //////{
                //////    cdo.SetDisplay(true, true, "");
                //////    cdo.RemoveDisplay(true);
                //////    cdo.RemoveDisplay(true);
                //////    // cdo.SetDisplay(false, false, "", Up, Lp);
                //////}
                //////catch
                //////{
                //////    MessageBox.Show("P0. .  .");
                //////}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RunConsole()
        {
            string programID = "TempoRegiTorihikiReceipt";
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath);
            string Mode = "6";            
            dtDepositNO = bbl.SimpleSelect1("51", "", Application.ProductName, "", "");
            string DepositeNO = dtDepositNO.Rows[0]["DepositNO"].ToString();//テーブル転送仕様Ａで覚えた入出金番号

            string cmdLine =InCompanyCD+ " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + Mode+" "+DepositeNO;//parameter
            try
            {
                try
                {
                    ///movedBegin
                    try
                    {
                        Printer_Open(filePath, programID, cmdLine);
                        CDO_Open();
                        //  Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                        // Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                    }
                    catch (Exception ex) { MessageBox.Show("Parallel function worked and cant dispose instance. . . " + ex.Message); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ////Stop_DisplayService();
                //try
                //{
                //    cdo.SetDisplay(true, true, "", "");
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //}
                //catch { }
                //if (Base_DL.iniEntity.IsDM_D30Used)
                //{
                //    CashDrawerOpen op = new CashDrawerOpen();  //2020_06_24 
                //    op.OpenCashDrawer(); //2020_06_24     << PTK
                //}
                //try
                //{
                //    cdo.SetDisplay(true, true, "", "");
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //}
                //catch
                //{
                //}
                //var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                //pro.WaitForExit();
                //Stop_DisplayService();
            }
            catch
            {
            }

            //try
            //{
            //    try {
            //        cdo.RemoveDisplay(true);
            //        cdo.RemoveDisplay(true);
            //    }
            //    catch { }

            //    if (Base_DL.iniEntity.IsDM_D30Used)
            //    {
            //        CashDrawerOpen op = new CashDrawerOpen(); //ses   << PTK
            //        op.OpenCashDrawer();
            //    }
            //    var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            //    // cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            //    pro.WaitForExit();
            //    Stop_DisplayService();
            //}
            //catch
            //{

            //}
        }
        protected void Kill(string pth)
        {
            try
            {
                Process[] processCollection = Process.GetProcessesByName(pth.Replace(".exe", ""));
                foreach (Process p in processCollection)
                {
                    p.Kill();
                }

                Process[] processCollections = Process.GetProcessesByName(pth + ".exe");
                foreach (Process p in processCollections)
                {
                    p.Kill();
                }
            }
            catch { }
        }
        private void frmTempoRegiTsurisenJyunbi_KeyUp(object sender, KeyEventArgs e)
        {
           MoveNextControl(e);
        }

        private void Stop_DisplayService(bool isForced = true)
        {
            ////////if (Base_DL.iniEntity.IsDM_D30Used)
            ////////{

            ////////    Login_BL bbl_1 = new Login_BL();
            ////////    if (bbl_1.ReadConfig())
            ////////    {
            ////////        bbl_1.Display_Service_Update(false);
            ////////        Thread.Sleep(2 * 1000);
            ////////        bbl_1.Display_Service_Enabled(false);
            ////////    }
            ////////    else
            ////////    {
            ////////        bbl_1.Display_Service_Update(false);
            ////////        Thread.Sleep(2 * 1000);
            ////////        bbl_1.Display_Service_Enabled(false);
            ////////    }
            ////////    try
            ////////    {
            ////////        Kill("Display_Service");
            ////////    }
            ////////    catch (Exception ex)
            ////////    {
            ////////        MessageBox.Show(ex.StackTrace.ToString());
            ////////    }
            ////////    if (isForced) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            ////////    ///////Base_DL.iniEntity.CDO_DISPLAY.SetDisplay(true, true,Base_DL.iniEntity.DefaultMessage);
            ////////}
        }
        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            ////////try
            ////////{
            ////////    if (Base_DL.iniEntity.IsDM_D30Used)
            ////////    {
            ////////        cdo.RemoveDisplay(true);
            ////////        Login_BL bbl_1 = new Login_BL();
            ////////        bbl_1.Display_Service_Update(true);
            ////////        bbl_1.Display_Service_Enabled(true);
            ////////    }
            ////////}
            ////////catch (Exception ex)
            ////////{
            ////////    MessageBox.Show("Error in removing display. . .");
            ////////}
        }
        private void Start_Display()
        {
            //////try
            //////{
            //////    if (Base_DL.iniEntity.IsDM_D30Used)
            //////    {
            //////        Login_BL bbl_1 = new Login_BL();
            //////        if (bbl_1.ReadConfig())
            //////        {
            //////            bbl_1.Display_Service_Update(false);
            //////            Thread.Sleep(2 * 1000);
            //////            bbl_1.Display_Service_Enabled(false);
            //////        }
            //////        else
            //////        {
            //////            bbl_1.Display_Service_Update(false);
            //////            Thread.Sleep(2 * 1000);
            //////            bbl_1.Display_Service_Enabled(false);
            //////        }
            //////        Kill("Display_Service");
            //////    }
            //////}
            //////catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}
