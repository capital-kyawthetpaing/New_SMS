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

namespace Search
{
    public partial class Search_TenzikaiJuchuuNO :  FrmSubForm
    {
        TenzikaiJuchuuNo_BL tzkjbl;
        M_Vendor_Entity mve;
        public Search_TenzikaiJuchuuNO()
        {
            
            InitializeComponent();
        }

        public void Search_TenzikaiJuchuuNO_Load(object sender, EventArgs e)
        {
            tzkjbl = new TenzikaiJuchuuNo_BL();
            mve = new M_Vendor_Entity();
            txtOrderDateTo.Text = DateTime.Now.ToString();
            BindCombo();
        }

        private void BindCombo()
        {
            string ymd = bbl.GetDate();
            cboYear.Bind(ymd);
            cboSeason.Bind(ymd);
        }

        private void F11()
        {
            if (ErrorCheck())
            {
                //mse = GetSearchInfo();
                //DataTable dtSouko = ssbl.M_Souko_Search(mse);
                //GvSouko.DataSource = dtSouko;
            }
        }

        private bool ErrorCheck()
        {
            if (!txtOrderDateFrom.DateCheck())
                return false;

            if (!txtOrderDateTo.DateCheck())
                return false;
            else
            {
                if (string.Compare(txtOrderDateFrom.Text, txtOrderDateTo.Text) == 1)
                {
                    tzkjbl.ShowMessage("E104");
                    txtOrderDateTo.Focus();
                    return false;
                }
            }

            if(string.IsNullOrWhiteSpace(txtOrderDateTo.Text))
            {
                ScSupplier.ChangeDate = bbl.GetDate();
                scStaff.ChangeDate = bbl.GetDate();
            }
            else
            {
                ScSupplier.ChangeDate = txtOrderDateTo.Text;
                scStaff.ChangeDate = txtOrderDateTo.Text;
            }
            if(string.IsNullOrWhiteSpace(ScSupplier.TxtCode.Text))
            {   
                if(ScSupplier.SelectData())
                {
                    bbl.ShowMessage("E101");
                    ScSupplier.SetFocus(1);
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(cboYear.Text.ToString())) 
            {
                tzkjbl.ShowMessage("E102");
                cboYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cboSeason.Text.ToString()))
            {
                tzkjbl.ShowMessage("E102");
                cboSeason.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(scStaff.TxtCode.Text))
            {
                if (scStaff.SelectData())
                {
                    bbl.ShowMessage("E101");
                    scStaff.SetFocus(1);
                    return false;
                }              
            }


            return true;

        }
    }
}
