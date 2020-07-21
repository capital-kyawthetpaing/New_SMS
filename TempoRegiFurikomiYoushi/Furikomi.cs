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
using Base.Client;
using CKM_Controls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace TempoRegiFurikomiYoushi
{
    public partial class Furikomi : ShopBaseForm
    {
        DataTable dtreporttemp = new DataTable();
        protected Base_BL bbl = new Base_BL();
        TempoRegiFurikomiYoushi_BL tbl;
        public Furikomi()
        {
            InitializeComponent();
        }
        public override void FunctionProcess(int index)
        {
            Printtest();
            //switch (index + 1)
            //{
            //    case 2:
            //        Save();
            //        break;
            //}
        }
        private void SetRequireField()
        {
            txtprintprogress.Require(true);
        }
        public bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtprintprogress }) || CheckExistPrintsdata())
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        private bool CheckExistPrintsdata()
        {
            Form parentForm = this.FindForm();
            dtreporttemp = tbl.SelectPrintData(txtprintprogress.Text, GetText(parentForm, "lblStoreName"));
            return (dtreporttemp.Rows.Count != 1) ? true : false;

        }
        public string GetText(Form frm, string storename)
        {
            CKMShop_Label tbx = this.Controls.Find(storename, true).FirstOrDefault() as CKMShop_Label;
            return "REKUTEN RACKET";
            // return tbx.Text;

        }
        public void Save()
        {
            if (ErrorCheck())
            {
                if (bbl.ShowMessage("Q201") == DialogResult.Yes)
                {
                    var keys = string.Join("-", Array.ConvertAll<object, string>(dtreporttemp.Rows[0].ItemArray, Convert.ToString));
                    // var keys = dtreporttemp.Rows[0].ItemArray.ToString(); ;
                    CR_regi.Youshi youshi = new CR_regi.Youshi(dtreporttemp, new string[] { InOperatorCD, InProgramID, InPcID, "Printed Mode", keys });
                    youshi.ShowDialog();
                    // Show Report
                }
            }
        }

        private void TempoRegiFurikomiYoushi_Load(object sender, EventArgs e)
        {
            InProgramID = "TempoRegiFurikomiYoushi";

            string data = InOperatorCD;
            StartProgram();
            SetRequireField();

            txtprintprogress.Focus();
            this.KeyUp += this.TempoRegiFurikomiYoushi_KeyUp;
        }
        private void TempoRegiFurikomiYoushi_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        protected void txtprintprogress_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SqlConnection con = new SqlConnection("Data Source = devserver1\\sql2014 ;Initial Catalog=SMS;Persist Security Info=True;User ID=sa;Password=admin12345!");
                con.Open();
                SqlCommand cmd = new SqlCommand(" select * from D_Sales where SalesNo = '" + txtprintprogress.Text + "'  AND BillingType = '1'", con);
                cmd.CommandTimeout = 0;
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                var furikomi = new DataTable();
                adp.Fill(furikomi);
                con.Close();

                if (furikomi.Rows.Count == 0)
                {
                    bbl.ShowMessage("E138");
                }
                else if (furikomi.Rows.Count == 1)
                {
                    if (!String.IsNullOrEmpty(furikomi.Rows[0]["DeleteDateTime"].ToString()))
                    {
                        bbl.ShowMessage("E140");
                    }
                }
            }
        }

        protected void Printtest()
        {

            try
            {
                A4_Test Report = new A4_Test();
                CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                margin.leftMargin = 0; // mmの指定をtwip単位に変換する
                margin.topMargin = 0;
                margin.bottomMargin = 0;//mmToTwip(marginLeft);
                margin.rightMargin = 0;
                Report.PrintOptions.ApplyPageMargins(margin);
                //Report.PrintOptions.PaperSize=PaperSize.;
                Report.PrintOptions.PrinterName = @"\\dataserver\Canon LBP2900";
                Report.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;

                //Report.PrintToPrinter(0, false, 0, 0);   ///Direct Printing

                var vwr = new Viewer();
                vwr.CrystalReportViewer1.ReportSource = Report;
                vwr.ShowDialog();
            }
            catch (CrystalReportsException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }



    }
}
