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

namespace TempoRegiHanbaiTouroku
{
    public partial class FrmSelect : Form
    {
        Base_BL bbl = new Base_BL();
        public int btnSelect = 0;
        public string keijobi = "";

        public FrmSelect()
        {
            InitializeComponent();

            SetRequireField();
            //初期値：Today
            txtSalesDate.Text = bbl.GetDate();
        }
        private void SetRequireField()
        {
            txtSalesDate.Require(true);
        }
        private bool ErrorCheck()
        {
            ////必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
            //if (string.IsNullOrWhiteSpace(txtSalesDate.Text))
            //{
            //    //Ｅ１０２
            //    bbl.ShowMessage("E102");
            //    txtSalesDate.Focus();
            //    return false;
            //}

            //txtSalesDate.Text = bbl.FormatDate(txtSalesDate.Text);

            ////日付として正しいこと(Be on the correct date)Ｅ１０３
            //if (!bbl.CheckDate(txtSalesDate.Text))
            //{
            //    //Ｅ１０３
            //    bbl.ShowMessage("E103");
            //    txtSalesDate.Focus();
            //    return false;
            //}
            ////入力できる範囲内の日付であること
            //if (!bbl.CheckInputPossibleDate(txtSalesDate.Text))
            //{
            //    //Ｅ１１５
            //    bbl.ShowMessage("E115");
            //    txtSalesDate.Focus();
            //    return false;
            //}
            ////過去日付でないこと
            ////共通処理－日付チェック－会計チェック
            //if (!bbl.CheckInputPossibleDateWithFisicalMonth(txtSalesDate.Text))
            //{
            //    //Ｅ１１５
            //    bbl.ShowMessage("E115");
            //    txtSalesDate.Focus();
            //    return false;
            //}

            return true;
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                btnSelect = 1;
                keijobi = txtSalesDate.Text;
                this.Close();
            }
        }

        private void ckM_Button2_Click(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                btnSelect = 2;
                keijobi = txtSalesDate.Text;
                this.Close();
            }
        }

        private void txtSalesDate_KeyDown(object sender, KeyEventArgs e)
        {
            //Enterキー押下時処理
            //Returnキーが押されているか調べる
            //AltかCtrlキーが押されている時は、本来の動作をさせる
            if ((e.KeyCode == Keys.Return) &&
                ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
            {

                if (ErrorCheck())
                {
                    ckM_Button1.Focus();
                }
            }
        }
    }
}
