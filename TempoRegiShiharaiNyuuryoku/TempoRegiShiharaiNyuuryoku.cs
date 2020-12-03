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
using System.Diagnostics;
using System.Threading;

namespace TempoRegiShiharaiNyuuryoku
{
    public partial class TempoRegiShiharaiNyuuryoku : ShopBaseForm
    {
        TempoRegiShiharaiNyuuryoku_BL trgshbl = new TempoRegiShiharaiNyuuryoku_BL();
        D_DepositHistory_Entity ddpe = new D_DepositHistory_Entity();
        DataTable dtDepositNO;
        public TempoRegiShiharaiNyuuryoku()
        {
            Start_Display();
            InitializeComponent();
            dtDepositNO = new DataTable();
        }

        private void TempoRejiShiharaiNyuuryoku_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiShiharaiNyuuryoku";

            string data = InOperatorCD;
            StartProgram();
            this.Text = "支払入力";
            SetRequireField();
            BindCombo();
            txtPayment.Focus();
            Stop_DisplayService();
        }

        private void SetRequireField()
        {
            txtPayment.Require(true);
            cboDenominationName.Require(true);
        }

        public void BindCombo()
        {
            cboDenominationName.Bind(string.Empty);
        }

        private void DisplayData()
        {
            txtPayment.Focus();
            string data = InOperatorCD;        
            SetRequireField();
            BindCombo();
        }

        public bool ErrorCheck()
        {
            if(string.IsNullOrWhiteSpace(txtPayment.Text))
            {
                trgshbl.ShowMessage("E102");
                txtPayment.Focus();
                return false;
            }
            DataTable dt = new DataTable();
            dt = trgshbl.SimpleSelect1("70", ChangeDate.Replace("/", "-"), InOperatorCD);
            if (dt.Rows.Count > 0)
            {
                trgshbl.ShowMessage("E252");
                return false;
            }
            if (cboDenominationName.SelectedValue.ToString() == "-1")
            {
                trgshbl.ShowMessage("E102");
                cboDenominationName.Focus();
                return false;
            }

          
            return true;
        }
        protected override void EndSec()
        {
            RunDisplay_Service();
            this.Close();
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

        private void Save()
        {
            if(ErrorCheck())
            {

                if (trgshbl.ShowMessage("Q101") == DialogResult.Yes)
                {
                    DataTable dt = new DataTable();
                    dt = trgshbl.SimpleSelect1("70", ChangeDate.Replace("/", "-"),InOperatorCD);
                    if (dt.Rows.Count > 0)
                    {
                        trgshbl.ShowMessage("E252");
                    }

                    ddpe = GetDepositEntity();

                    if (trgshbl.TempoRegiShiNyuuryoku_InsertUpdate(ddpe))
                    {
                        //trgshbl.ShowMessage("I101");
                        //RunConsole();//exeRun    <<<< PTK
                        if (Base_DL.iniEntity.IsDM_D30Used)
                        {
                            RunConsole();
                        }
                        else
                        {
                            trgshbl.ShowMessage("I101");
                        }
                        txtPayment.Clear();
                        txtPayment.Focus();
                        cboDenominationName.SelectedValue = "-1";
                        txtRemarks.Clear();
                        DisplayData();                       
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
                try
                {
                    //cdo.RemoveDisplay(true);
                    //cdo.RemoveDisplay(true);
                }
                catch { }
                var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                //pro.WaitForExit();
                try
                {
                    //cdo.SetDisplay(true, true, "");
                    //cdo.RemoveDisplay(true);
                    //cdo.RemoveDisplay(true);
                //    cdo.SetDisplay(false, false, "", Up, Lp);
                }
                catch
                {
                    MessageBox.Show("P0. .  .");
                }
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
            string Mode = "3";
            dtDepositNO = bbl.SimpleSelect1("52", "", Application.ProductName, "", "");
            string DepositeNO = dtDepositNO.Rows[0]["DepositNO"].ToString();
            string cmdLine = InCompanyCD + " " + InOperatorCD + " " + Login_BL.GetHostName() + " " + Mode + " " + DepositeNO;//parameter

            try
            {
                try
                {
                    ///movedBegin
                    try
                    {
                        //  Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine));
                        Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath, programID, cmdLine+ ""));
                    }
                    catch (Exception ex) { MessageBox.Show("Parallel function worked and cant dispose instance. . . " + ex.Message); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //////////Stop_DisplayService();
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
            //    try
            //    {
            //        cdo.RemoveDisplay(true);
            //        cdo.RemoveDisplay(true);
            //        var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
            //        pro.WaitForExit();
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Error in removing . . . .");
            //    }
            //    if (Base_DL.iniEntity.IsDM_D30Used)
            //    {
            //        CashDrawerOpen op = new CashDrawerOpen();
            //        op.OpenCashDrawer();   // <<<< PTK
            //    }
            //    Stop_DisplayService();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.StackTrace.ToString());
            //}
        }

        private D_DepositHistory_Entity GetDepositEntity()
        {
            ddpe = new D_DepositHistory_Entity
            {
                StoreCD = InOperatorCD,
                DenominationCD = cboDenominationName.SelectedValue.ToString(),
                DataKBN = "3",
                DepositKBN = "3",
                DepositGaku = txtPayment.Text,
                Remark = txtRemarks.Text,
                ExchangeMoney = "0",
                ExchangeDenomination = string.Empty,
                ExchangeCount = "0",
                AdminNO = string.Empty,
                SalesSU = "0",
                SalesUnitPrice = "0",
                SalesGaku = "0",
                SalesTax = "0",
                TotalGaku = "0",
                Refund = "0",
                ProperGaku = "0",
                DiscountGaku = "0",
                CustomerCD = string.Empty,
                IsIssued = "0",
                CancelKBN = "0",
                RecoredKBN = "0",
                Rows = "0",
                SalesTaxRate = "0",
                AccountingDate = DateTime.Today.ToShortDateString(),
                Operator = InOperatorCD,
                ProgramID = InProgramID,
                PC = InPcID,
                ProcessMode = "登 録",
                Key = txtPayment.Text.ToString()
            };
            return ddpe;
        }


        private void TempoRegiShiharaiNyuuryoku_KeyUp(object sender, KeyEventArgs e)
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
            //////if (Base_DL.iniEntity.IsDM_D30Used)
            //////{

            //////    Login_BL bbl_1 = new Login_BL();
            //////    if (bbl_1.ReadConfig())
            //////    {
            //////        bbl_1.Display_Service_Update(false);
            //////        Thread.Sleep(2 * 1000);
            //////        bbl_1.Display_Service_Enabled(false);
            //////    }
            //////    else
            //////    {
            //////        bbl_1.Display_Service_Update(false);
            //////        Thread.Sleep(2 * 1000);
            //////        bbl_1.Display_Service_Enabled(false);
            //////    }
            //////    try
            //////    {
            //////        Kill("Display_Service");
            //////    }
            //////    catch (Exception ex)
            //////    {
            //////        MessageBox.Show(ex.StackTrace.ToString());
            //////    }
            //////    if (isForced) cdo.SetDisplay(true, true, Base_DL.iniEntity.DefaultMessage);
            //////}
        }
        private void RunDisplay_Service()  // Make when we want to run display_service
        {
            ////////try
            ////////{
            ////////    if (Base_DL.iniEntity.IsDM_D30Used)
            ////////    {
            ////////        try
            ////////        {
            ////////            cdo.RemoveDisplay(true);
            ////////        }
            ////////        catch { }
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
            ////////try
            ////////{
            ////////    if (Base_DL.iniEntity.IsDM_D30Used)
            ////////    {
            ////////        Login_BL bbl_1 = new Login_BL();
            ////////        if (bbl_1.ReadConfig())
            ////////        {
            ////////            bbl_1.Display_Service_Update(false);
            ////////            Thread.Sleep(2 * 1000);
            ////////            bbl_1.Display_Service_Enabled(false);
            ////////        }
            ////////        else
            ////////        {
            ////////            bbl_1.Display_Service_Update(false);
            ////////            Thread.Sleep(2 * 1000);
            ////////            bbl_1.Display_Service_Enabled(false);
            ////////        }
            ////////        Kill("Display_Service");
            ////////    }
            ////////}
            ////////catch (Exception ex) { MessageBox.Show("Cant remove on second time" + ex.StackTrace); }
        }
        EPSON_TM30.CashDrawerOpen cdo = new EPSON_TM30.CashDrawerOpen();
    }
}
