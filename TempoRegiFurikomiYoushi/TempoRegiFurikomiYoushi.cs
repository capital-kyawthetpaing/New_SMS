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
using DL;

namespace TempoRegiFurikomiYoushi
{
    public partial class TempoRegiFurikomiYoushi : ShopBaseForm
    {
        DataTable dtreporttemp = new DataTable();
        protected Base_BL bbl = new Base_BL();
        TempoRegiFurikomiYoushi_BL tbl;

        public TempoRegiFurikomiYoushi()
        {

            InitializeComponent();
            tbl = new TempoRegiFurikomiYoushi_BL();
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
        private void SetRequireField()
        {
            txtprintprogress.Require(true);
        }
        public bool ErrorCheck()
        {
           
             if (!RequireCheck(new Control[] { txtprintprogress }))
            {
                return false;
            }
            else if (CheckExistPrintsdata())
            {
                return false;
            }
            return true;
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
            // return "REKUTEN RACKET";
            return tbx.Text;

        }
        public void Save()
        {
            if (!String.IsNullOrWhiteSpace(txtprintprogress.Text))
            {
                if (ErrorCheck())
                {
                    if (InputErrorCheck())
                    {
                        if (bbl.ShowMessage("Q201") == DialogResult.Yes)
                        {
                            var keys = string.Join("-", Array.ConvertAll<object, string>(dtreporttemp.Rows[0].ItemArray, Convert.ToString));
                            try
                            {
                                CR_regi.Youshi youshi = new CR_regi.Youshi(dtreporttemp, new string[] { InOperatorCD, InProgramID, InPcID, "PRINT", txtprintprogress.Text });
                                youshi.ShowDialog();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.InnerException.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Message);
                            }
                          
                        }
                    }
                }
                else
                {
                    bbl.ShowMessage("E138");
                }
                txtprintprogress.Focus();
            }
            else
            {

                bbl.ShowMessage("E102");
                txtprintprogress.Focus();

            }
        }

        private void TempoRegiFurikomiYoushi_Load(object sender, EventArgs e)
        {
            //InProgramID = "店舗振込用紙";
           // InProgramID = "店舗レジ振込用紙印刷";
            InProgramID = "TempoRegiFurikomiYoushi";
            string data = InOperatorCD;
            StartProgram();
            this.Text = "店舗レジ 振込用紙印刷";
            SetRequireField();
            btnProcess.Text = "印  刷";

            txtprintprogress.Focus();
            this.KeyUp += this.TempoRegiFurikomiYoushi_KeyUp;
        }
        private void TempoRegiFurikomiYoushi_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        protected override void EndSec()
        {
            this.Close();
        }
        protected void txtprintprogress_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrWhiteSpace(txtprintprogress.Text))
                    InputErrorCheck();
                else
                {
                    bbl.ShowMessage("E102");
                    txtprintprogress.Focus();
                    return ;
                }


            }

        }

        protected bool  InputErrorCheck()
        {
            //  SqlConnection con = new SqlConnection("Data Source = devserver\\sql2014 ;Initial Catalog=SMS;Persist Security Info=True;User ID=sa;Password=admin12345!");
            Base_DL bdl = new Base_DL();
            var con = bdl.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand(" select * from D_Sales where SalesNo = '" + txtprintprogress.Text + "'  AND BillingType = '1'", con);
            cmd.CommandTimeout = 0;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            var furikomi = new DataTable();
            adp.Fill(furikomi);
            con.Close();

            if (furikomi.Rows.Count == 0)
            {
                bbl.ShowMessage("E138"); txtprintprogress.Focus();
                return false;
            }
            else if (furikomi.Rows.Count == 1)
            {
                if (!String.IsNullOrEmpty(furikomi.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140"); txtprintprogress.Focus();
                    return false;
                }
            }
            
            return true;
        }

        private void ckmShop_CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
