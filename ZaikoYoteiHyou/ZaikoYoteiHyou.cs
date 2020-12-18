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
using CrystalDecisions.Shared;
using System.IO;

namespace ZaikoYoteiHyou
{
    public partial class FrmZaikoYoteiHyou : FrmMainForm
    {
        ZaikoYoteiHyou_BL zkybl = new ZaikoYoteiHyou_BL();
        //Staff_BL sbl = new Staff_BL();
        //Search_Souko_BL ssbl = new Search_Souko_BL();
        //M_Souko_Entity mske = new M_Souko_Entity();
        M_Staff_Entity mse = new M_Staff_Entity();
        M_StoreAuthorizations_Entity msae = new M_StoreAuthorizations_Entity();
        //MasterTouroku_Souko_BL msbl = new MasterTouroku_Souko_BL();
        D_Purchase_Entity dpe = new D_Purchase_Entity();
        D_Order_Entity doe = new D_Order_Entity();
        Viewer previewForm = new Viewer();
 
        public FrmZaikoYoteiHyou()
        {
            InitializeComponent();
        }
 
        private void FrmZaikoYoteiHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "ZaikoYoteiHyou";
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();
            base.Btn_F9.Text = "";
            base.Btn_F10.Text = "";
            base.Btn_F11.Text = "";
            BindCombo();

            //ComboDispay();
            SetRequireField();
            txtTargetDateTo.Focus();                     
           string m = DateTime.Now.Month.ToString();
            if(m.Length ==1)
            {
                m = 0 + DateTime.Now.Month.ToString();
            }
            string y = DateTime.Now.Year.ToString();
            //txtTargetDateFrom.Text = a.ToString().Substring(0, 7);
            txtTargetDateFrom.Text = y + "/" + m;
            txtTargetDateFrom.Enabled = false;
        }

        //public void ComboDispay()
        //{
        //    DataTable dtStoreAuthor = sbl.BindStoreAuthorization();
        //    mse.StaffCD = InOperatorCD;
        //    mse.ChangeDate = DateTime.Today.ToShortDateString();
        //    DataTable dtstaff = new DataTable();
        //    dtstaff = zkybl.M_Staff_Select(mse);
        //    if(dtstaff.Rows.Count >0 )
        //    {
        //        mse.StoreCD = dtstaff.Rows[0]["StoreCD"].ToString();
        //        for(int i = 0; i< dtStoreAuthor.Rows.Count; i++)
        //        {
        //           if(dtStoreAuthor.Rows[i]["StoreCD"].Equals(mse.StoreCD))
        //            {
        //                cboStore.SelectedValue = dtStoreAuthor.Rows[i]["StoreAuthorizationsCD"].ToString();
        //            }
        //        }
        //    }

        //    mske.StoreCD = dtstaff.Rows[0]["StoreCD"].ToString();
        //    mske.ChangeDate = DateTime.Today.ToShortDateString();
        //    DataTable dtSouko1 = ssbl.M_Souko_Bind(mske);
        //    DataTable dtSouko = ssbl.M_Souko_BindAll(mske);
        //    for(int i = 0; i< dtSouko.Rows.Count;i++)
        //    {
        //        for(int j= 0; j < dtSouko1.Rows.Count ; j++)
        //        {
        //            if (dtSouko.Rows[i]["SoukoCD"].ToString().Contains(dtSouko1.Rows[j]["SoukoCD"].ToString()))
        //            {
        //                cboWareHouse.SelectedValue = dtSouko1.Rows[j]["SoukoCD"].ToString();
        //                if (cboWareHouse.SelectedValue.ToString() != "-1")
        //                {
        //                    j = dtSouko1.Rows.Count;
        //                    i = dtSouko.Rows.Count;
        //                }
        //            }
        //        }             
        //    }

        //    //msae.StoreAuthorizationsCD = StoreAuthorizationsCD;
        //    //msae.ChangeDate = StoreAuthorizationsChangeDate;
        //    //msae.StoreCD = cboStore.SelectedValue.ToString();
        //    //DataTable dtAuthorization = new DataTable();
        //    //dtAuthorization = zkybl.M_StoreAuthorizations_Select(msae);
        //    //if (dtAuthorization.Rows.Count == 0)
        //    //{
        //    //    zkybl.ShowMessage("E139");
        //    //    cboStore.Focus();
        //    //}

        //}

        public void SetRequireField()
        {
            cboStore.Require(true);
            cboWareHouse.Require(true);
        }

        public void BindCombo()
        {
            cboStore.Bind(string.Empty,"2");
            cboStore.SelectedValue = StoreCD;
            cboWareHouse.Bind(string.Empty,"");
            cboWareHouse.SelectedValue = SoukoCD;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {              
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        Clear(panelDetail);
                        string m = DateTime.Now.Month.ToString();
                        if (m.Length == 1)
                        {
                            m = 0 + DateTime.Now.Month.ToString();
                        }
                        string y = DateTime.Now.Year.ToString();
                        txtTargetDateFrom.Text = y + "/" + m;
                        txtTargetDateTo.Focus();
                        BindCombo();
                    }
                    break;              
            }
        }
        
        /// <summary>
        /// Error Check for F12
        /// </summary>
        /// <returns></returns>
        public bool ErrorCheck()
        {
            if(!string.IsNullOrWhiteSpace(txtTargetDateTo.Text))
            {
                if (!txtTargetDateTo.YearMonthCheck())
                    return false;
                int result = txtTargetDateFrom.Text.CompareTo(txtTargetDateTo.Text);
                if(result > 0)
                {
                    zkybl.ShowMessage("E104");
                    txtTargetDateTo.Focus();                   
                    return false;
                }
            }

            //mse.StaffCD = InOperatorCD;
            //mse.ChangeDate = DateTime.Today.ToShortDateString();
            //DataTable dtstaff = new DataTable();
            //dtstaff = zkybl.M_Staff_Select(mse);
            //if(dtstaff.Rows.Count == 0)
            //{
            //    zkybl.ShowMessage("E139");
            //    cboStore.Focus();
            //    return false;
            //}
            //else
            //{
            //msae.StoreAuthorizationsCD = StoreAuthorizationsCD;
            //msae.ChangeDate = StoreAuthorizationsChangeDate;
            //msae.StoreCD = cboStore.SelectedValue.ToString();
            //DataTable dtAuthorization = new DataTable();
            //dtAuthorization = zkybl.M_StoreAuthorizations_Select(msae);
            //if (dtAuthorization.Rows.Count == 0)
            //{
            //    zkybl.ShowMessage("E139");
            //    cboStore.Focus();
            //    return false;
            //}
            ////}

            if (!RequireCheck(new Control[] { cboStore, cboWareHouse }))
                return false;

            if (!base.CheckAvailableStores(cboStore.SelectedValue.ToString()))
            {
                zkybl.ShowMessage("E139");
                cboStore.Focus();
                return false;
            }
            if (!RequireCheck(new Control[] { cboWareHouse }))
                return false;

           return true;
        }

        #region To Print Report
        protected override void PrintSec()
        {
            if (PrintMode != EPrintMode.DIRECT)
                return;

            F12();
        }

        public void F12()
        {
            if(ErrorCheck())
            {
                doe = new D_Order_Entity
                {
                    StoreCD = cboStore.SelectedValue.ToString(),
                    DestinationSoukoCD = cboWareHouse.SelectedValue.ToString(),

                };
                DateTime firstday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                firstday.AddDays(-1).ToString("dd/MM/yyyy");

                string Text = txtTargetDateTo.Text.ToString();
                if(!string.IsNullOrWhiteSpace(Text))
                {
                    string[] p = Text.Split('/');
                    string y = p[0].ToString();
                    string m = p[1].ToString();
                    int yy = Convert.ToInt32(y);
                    int mm = Convert.ToInt32(m);
                    DateTime lastday = new DateTime(yy, mm,
                                            DateTime.DaysInMonth(yy, mm));

                    dpe = new D_Purchase_Entity
                    {
                        PurchaseDateFrom = firstday.ToShortDateString(),
                        PurchaseDateTo = lastday.ToShortDateString(),
                    };


                }
                else
                {
                    dpe = new D_Purchase_Entity
                    {
                        PurchaseDateFrom = firstday.ToShortDateString(),
                        PurchaseDateTo = null,
                    };
                }

                
                DataTable dt = zkybl.D_Order_Select(doe, dpe);
                //if (dt == null)
                if(dt.Rows.Count == 0)
                {                   
                    //if (dt == null) return;                  
                    zkybl.ShowMessage("E128");
                    txtTargetDateTo.Focus();
                }
                else
                {
                    dt.Columns.Add("Total");
                    dt.Rows[0]["Total"] = dt.Rows[0]["Gaku"].ToString();
                    decimal t = Convert.ToDecimal(dt.Rows[1]["Gaku"]) + Convert.ToDecimal(dt.Rows[0]["Total"]);
                    dt.Rows[1]["Total"] = t.ToString();

                    for (int i = 2; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Total"] = Convert.ToDecimal(dt.Rows[i]["Gaku"]) + Convert.ToDecimal(dt.Rows[i - 1]["Total"]);
                    }

                    try
                    {
                        ZaikoYoteiHyouReport Report = new ZaikoYoteiHyouReport();
                        DialogResult ret;
                        switch (PrintMode)
                        {
                            case EPrintMode.DIRECT:
                                ret = bbl.ShowMessage("Q202");
                                if (ret == DialogResult.Cancel)
                                {
                                    return;
                                }
                                // 印字データをセット
                                Report.SetDataSource(dt);
                                Report.Refresh();
                                Report.SetParameterValue("PrintDate", System.DateTime.Now.ToString("yyyy/MM/dd") + " " + System.DateTime.Now.ToString("HH:mm"));
                                Report.SetParameterValue("TargetDate", txtTargetDateFrom.Text + " ～ " + txtTargetDateTo.Text);
                                Report.SetParameterValue("txtSouko", cboWareHouse.SelectedValue.ToString() + "  " + cboWareHouse.Text);

                                if (ret == DialogResult.Yes)
                                {
                                    var previewForm = new Viewer();
                                    previewForm.CrystalReportViewer1.ShowPrintButton = true;
                                    previewForm.CrystalReportViewer1.ReportSource = Report;
                                    previewForm.ShowDialog();
                                }
                                else
                                {
                                    //int marginLeft = 360;
                                    CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                                    margin.leftMargin = DefaultMargin.Left; // mmの指定をtwip単位に変換する
                                    margin.topMargin = DefaultMargin.Top;
                                    margin.bottomMargin = DefaultMargin.Bottom;//mmToTwip(marginLeft);
                                    margin.rightMargin = DefaultMargin.Right;
                                    Report.PrintOptions.ApplyPageMargins(margin);     /// Error Now
                                    // プリンタに印刷
                                    System.Drawing.Printing.PageSettings ps;
                                    try
                                    {
                                        System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();

                                        CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();

                                        System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();



                                        Report.PrintOptions.PrinterName = "\\\\dataserver\\Canon LBP2900";
                                        System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                                        Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                                        Report.PrintOptions.PrinterDuplex = PrinterDuplex.Simplex;

                                        Report.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                                        // Print the report. Set the startPageN and endPageN 
                                        // parameters to 0 to print all pages. 
                                        //Report.PrintToPrinter(1, false, 0, 0);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                break;
                        }
                    }
                    catch(Exception e)
                    {
                        var mse = e.Message;
                    }
                    finally
                    {
                        txtTargetDateTo.Focus();
                    }
                }
              
            }
        }
        #endregion

        #region Key Event
        private void FrmZaikoYoteiHyou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }

        private void txtTargetDateTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(txtTargetDateTo.Text))
                {
                    int result = txtTargetDateFrom.Text.CompareTo(txtTargetDateTo.Text);
                    if (result > 0)
                    {
                        zkybl.ShowMessage("E104");
                        txtTargetDateTo.Focus();
                    }
                }
            }
        }
        #endregion
    }
}
