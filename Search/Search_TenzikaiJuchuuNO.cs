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
        public Search_TenzikaiJuchuuNO()
        {
            InitializeComponent();
        }

        private void Search_TenzikaiJuchuuNO_Load(object sender, EventArgs e)
        {
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
                    //ssbl.ShowMessage("E104");
                    txtOrderDateTo.Focus();
                    return false;
                }
            }

            return true;

        }
    }
}
