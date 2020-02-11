using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;
using Search;
using Microsoft.VisualBasic;
using static Base.Client.FrmMainForm;

namespace TempoRegiHanbaiTouroku
{
    public partial class FrmOther : Form
    {
        Base_BL bbl = new Base_BL();
        public int btnSelect = 0;
        public int Result = 0;

        private string mJuchuuHontaiGaku;
        private int mTaxRateFLG;

        private string mUriageNo;
        private string mSaleDate;

        public FrmOther()
        {
            InitializeComponent();
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                ckmShop_RadioButton1.Checked = true;

                if (Result.Equals(1))
                    ckmShop_RadioButton1.Checked= true;
                if (Result.Equals(2))
                    ckmShop_RadioButton2.Checked = true;
                if (Result.Equals(3))
                    ckmShop_RadioButton3.Checked = true;
                if (Result.Equals(4))
                    ckmShop_RadioButton4.Checked = true;
                if (Result.Equals(5))
                    ckmShop_RadioButton5.Checked = true;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }     

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnSelect = 1;
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            btnSelect = 2;

            if (ckmShop_RadioButton1.Checked)
                Result = 1;
            if (ckmShop_RadioButton2.Checked)
                Result = 2;
            if (ckmShop_RadioButton3.Checked)
                Result = 3;
            if (ckmShop_RadioButton4.Checked)
                Result = 4;
            if (ckmShop_RadioButton5.Checked)
                Result = 5;

            this.Close();
        }
    }
}
