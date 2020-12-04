using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using Entity;
using DL;
using System.Threading;
using System.Diagnostics;

namespace TempoRegiRyougaeNyuuryoku
{
    
    public partial class frmTempoRegiRyougaeNyuuryoku : ShopBaseForm
    {
        int countmoney = 0;
        int moneytype = 0;
        string combovalue = "";
        bool valid = false;
        TempoRegiRyougaeNyuuryoku_BL trrnbl;
        D_DepositHistory_Entity mre;
        DataTable dtDepositNO;
        string storeCD;
        public frmTempoRegiRyougaeNyuuryoku()
        {
            Start_Display();
            InitializeComponent();
           
            dtDepositNO = new DataTable();
            trrnbl = new TempoRegiRyougaeNyuuryoku_BL();
            mre = new D_DepositHistory_Entity();
        }
        private void frmTempoRegiRyougaeNyuuryoku_Load(object sender, EventArgs e)
        {
            
            trrnbl = new TempoRegiRyougaeNyuuryoku_BL();
            InProgramID = "TempoRegiRyougaeNyuuryoku";
            
            StartProgram();
            this.Text = "両替入力";
            SetRequireField();
            BindCombo();
            storeCD = StoreCD;
            Stop_DisplayService();
            //displayData();
        }
        public void BindCombo()
        {
            ExchangeDenomination.Bind(string.Empty);
        }
        /// <summary>
        /// お金が正しいかどうかををチェックする
        /// </summary>
        public void displayData()
        {
            int moneyammount = countmoney * moneytype;
            //string aa = countmoney.ToString("#,##0");
            //ExchangeCount.Text = aa.Trim();
           // countmoney = Convert.ToInt32(ExchangeCount.Text.Replace(",",""));
            ExchangeCount.Text = countmoney.ToString("#,##0");
            string moneysperate = moneyammount.ToString("#,##0");
            ExchangeLabel.Text = moneysperate;
            if (ExchangeLabel.Text != ExchangeMoney.Text)
            {
                trrnbl.ShowMessage("E181");
                ExchangeCount.Focus();
            }
        }
        private void SetRequireField()
        {
            ExchangeMoney.Require(true);
            ExchangeDenomination.Require(true);
            ExchangeCount.Require(true);
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
                DepositKBN = "4",
                DepositKBN1 = "5",
                CancelKBN = "0",
                RecoredKBN = "0",
                DenominationCD = "1",
                Rows = "0",
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                TotalGaku = "0",
                SalesTaxRate = "0",
                Refund = "0",
                ProperGaku="0",//update
                DiscountGaku="0",//update
                CustomerCD="",//update
                IsIssued = "0",
                ExchangeMoney = ExchangeMoney.Text,
                ExchangeDenomination = ExchangeDenomination.SelectedValue.ToString(),
                ExchangeCount = ExchangeCount.Text.Replace(",", ""),
                Remark = Remark.Text,
                StoreCD = storeCD,
                Operator = InOperatorCD,
                ProcessMode = "登録",
                ProgramID = InProgramID,
                PC = InPcID,
                Key = storeCD+" "+ ExchangeMoney.Text

            };
            return mre;
        }
        /// 
        /// <summary>
        /// 登録ボタンを押下時データベースにInsertする
        /// </summary>
        public void Save()
        {
            if (ErrorCheck())
            {
                if (ExchangeLabel.Text != ExchangeMoney.Text)
                {
                    trrnbl.ShowMessage("E181");
                    ExchangeCount.Focus();
                }
                else
                {

                    if (trrnbl.ShowMessage("Q101") == DialogResult.Yes)
                    {
                        DataTable dt = new DataTable();
                        dt = trrnbl.SimpleSelect1("70", ChangeDate.Replace("/", "-"), storeCD);
                        if(dt.Rows.Count >0)
                        {
                            trrnbl.ShowMessage("E252");
                        }
                        valid = false;
                        mre = DepositHistoryEnity();
                        if (trrnbl.TempoRegiRyougaeNyuuryoku_Insert_Update(mre))
                        {
                            //trrnbl.ShowMessage("I101");

                            //RunConsole();
                            //exeRun
                            if (Base_DL.iniEntity.IsDM_D30Used)
                            {
                                RunConsole();
                            }
                            else
                            {
                                trrnbl.ShowMessage("I101");
                            }
                            ExchangeDenomination.SelectedValue = "-1";
                            ExchangeMoney.Clear();
                            ExchangeCount.Clear();
                            ExchangeLabel.Text = "";
                            Remark.Clear();
                            ExchangeMoney.Focus();
                        }
                        else
                        {
                            trrnbl.ShowMessage("S001");
                        }
                    }
                    else
                    {
                        PreviousCtrl.Focus();
                        //Remark.Focus();
                    }
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
                //try
                //{
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //}
                //catch { }
                // System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                //pro.WaitForExit();
                //try
                //{
                //    cdo.SetDisplay(true, true, "");
                //    cdo.RemoveDisplay(true);
                //    cdo.RemoveDisplay(true);
                //    // cdo.SetDisplay(false, false, "", Up, Lp);
                //}
                //catch
                //{
                //    MessageBox.Show("P0. .  .");
                //}
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
            string Mode = "5";
            dtDepositNO = bbl.SimpleSelect1("52", "", Application.ProductName, "", "");
            string DepositeNO = dtDepositNO.Rows[0]["DepositNO"].ToString();
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + Mode + " " + DepositeNO;

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
                        //  Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                    }
                    catch (Exception ex) { MessageBox.Show("Parallel function worked and cant dispose instance. . . " + ex.Message); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                /////// Stop_DisplayService();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 入力必須エラーをチェックする
        /// </summary>
        private bool ErrorCheck()
        {

            if (!RequireCheck(new Control[] { ExchangeMoney, ExchangeDenomination }))   // go that focus
                return false;

            if (ExchangeDenomination.SelectedValue.ToString() == "-1")
            {
                trrnbl.ShowMessage("E102");
                ExchangeDenomination.Focus();
                ExchangeDenomination.MoveNext = false;
                ExchangeCount.MoveNext = false;
                return false;
            }
            DataTable dt = new DataTable();
            //dt = trrnbl.SimpleSelect1("70",null, storeCD,null,null);
            dt = trrnbl.SimpleSelect1("70", ChangeDate.Replace("/", "-"), storeCD);
            if (dt.Rows.Count >0)
            {
                trrnbl.ShowMessage("E252");
                return false;
            }
            if (!RequireCheck(new Control[] { ExchangeCount }))   // go that focus
                return false;

            return true;
        }
        /// <summary>
        /// 戻るボタンを押下時formを閉じる
        /// </summary>
        protected override void EndSec()
        {
            RunDisplay_Service();
            this.Close();
        }
        private void ExchangeCount_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(ExchangeCount.Text))
                {
                    trrnbl.ShowMessage("E102");
                    ExchangeCount.Focus();
                    ExchangeCount.MoveNext = false;
                }
                else
                {
                    //ExchangeCount.Text = countmoney.ToString("#,##0");
                    countmoney = Convert.ToInt32(ExchangeCount.Text.Replace(",", "").ToString());
                }
                if (ExchangeDenomination.SelectedValue.ToString()=="-1")
                {

                    trrnbl.ShowMessage("E102");
                    ExchangeDenomination.Select();
                    ExchangeDenomination.MoveNext = false;
                    ExchangeCount.MoveNext = false;
                }
                else
                {
                   
                    combovalue = ExchangeDenomination.SelectedValue.ToString();
                    moneytype = Convert.ToInt32(combovalue);
                    
                }
                valid = true;
                displayData();
            }
        }
        private void ExchangeDenomination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (valid)
            {
                if (ExchangeDenomination.SelectedValue.ToString() == "-1")
                {

                    trrnbl.ShowMessage("E102");
                    ExchangeDenomination.Focus();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ExchangeCount.Text))
                    {
                        trrnbl.ShowMessage("E102");
                        ExchangeCount.Focus();

                    }
                    else
                    {
                        countmoney = Convert.ToInt32(ExchangeCount.Text.Replace(",", "").ToString());
                        //countmoney = Convert.ToInt32(ExchangeCount.Text);
                    }
                    combovalue = ExchangeDenomination.SelectedValue.ToString();
                    moneytype = Convert.ToInt32(combovalue);

                }
                displayData();
            }
           
        }
        private void frmTempoRegiRyougaeNyuuryoku_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
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
            ////////   /// //Base_DL.iniEntity.CDO_DISPLAY.SetDisplay(true, true,Base_DL.iniEntity.DefaultMessage);
            ////////}
        }
        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            //////////try
            //////////{
            //////////    if (Base_DL.iniEntity.IsDM_D30Used)
            //////////    {
            //////////        try
            //////////        {
            //////////            cdo.RemoveDisplay(true);
            //////////        }
            //////////        catch { }
            //////////        Login_BL bbl_1 = new Login_BL();
            //////////        bbl_1.Display_Service_Update(true);
            //////////        bbl_1.Display_Service_Enabled(true);
            //////////    }
            //////////}
            //////////catch (Exception ex)
            //////////{
            //////////    MessageBox.Show("Error in removing display. . .");
            //////////}
        }
        private void Start_Display()
        {
            //////////try
            //////////{
            //////////    if (Base_DL.iniEntity.IsDM_D30Used)
            //////////    {
            //////////        Login_BL bbl_1 = new Login_BL();
            //////////        if (bbl_1.ReadConfig())
            //////////        {
            //////////            bbl_1.Display_Service_Update(false);
            //////////            Thread.Sleep(2 * 1000);
            //////////            bbl_1.Display_Service_Enabled(false);
            //////////        }
            //////////        else
            //////////        {
            //////////            bbl_1.Display_Service_Update(false);
            //////////            Thread.Sleep(2 * 1000);
            //////////            bbl_1.Display_Service_Enabled(false);
            //////////        }
            //////////        Kill("Display_Service");
            //////////    }
            //////////}
            //////////catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}
