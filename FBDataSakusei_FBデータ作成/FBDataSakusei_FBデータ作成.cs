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

namespace FBDataSakusei_FBデータ作成
{
    public partial class FrmFBDataSakusei_FBデータ作成 : FrmMainForm
    {
        FBDataSakusei_BL fbbl = new FBDataSakusei_BL();
        M_Calendar_Entity mce = new M_Calendar_Entity();
        public FrmFBDataSakusei_FBデータ作成()
        {
            InitializeComponent();
        }

        private void FrmFBDataSakusei_FBデータ作成_Load(object sender, EventArgs e)
        {
            InProgramID = "FBDataSakusei_FBデータ作成";
            F2Visible = false;
            F3Visible = false;
            F4Visible = false;
            F5Visible = false;
            F6Visible = false;
            F7Visible = false;
            F8Visible = false;
            F9Visible = false;

            StartProgram();

            Btn_F12.Text = "出力(F12)";
            ModeVisible = false;

            BindCombo();
            SetRequireField();

            cboProcess.Focus();
        }

        public void BindCombo()
        {
            cboProcess.Items.Insert(-1,"");
            cboProcess.Items.Insert(0,"振込データ作成");
            cboProcess.Items.Insert(1,"振込データ削除");
            cboProcess.Items.Insert(2,"振込データ印刷");
            cboProcess.SelectedValue = 0;

            string ymd = bbl.GetDate();
            cboPayment.Bind(ymd);
        }

       public void SetRequireField()
       {
            cboProcess.Require(true);
            cboPayment.Require(true);
            txtPaymentDate.Require(true);
            txtTransferDate.Require(true);
       }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int Index)
        {
            switch (Index + 1)
            {
                case 2:
                case 3:
                case 4:                  
                case 5:
                case 6:                 
                case 10:
                    F10();
                    break;
                case 11:
                    F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }

        public void F10()
        {

        }

        public void F11()
        {

        }

        public void F12()
        {

        }

        public bool ErrorCheck(int index)
        {
            if (cboProcess.SelectedValue.ToString() == "-1")
            {
                bbl.ShowMessage("E102");
                cboProcess.Focus();
                return false;
            }
            else if(cboProcess.SelectedValue.ToString() == "0")
            {
                Btn_F11.Text = "印刷(F11)";
                Btn_F12.Text = "出力(F12)";               
            }
            else if(cboProcess.SelectedValue.ToString() == "1")
            {
                Btn_F12.Text = "削除(F12)";
            }
            else
            {
                Btn_F11.Text = "印刷(F11)";
            }

            if (cboPayment.SelectedValue.ToString() == "-1")
            {
                bbl.ShowMessage("E102");
                cboPayment.Focus();
                return false;
            }

            if (!RequireCheck(new Control[] { txtPaymentDate }))
                return false;

            if (!RequireCheck(new Control[] { txtTransferDate }))
                return false;

            return true;
        }

        private void txtTransferDate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(string.IsNullOrWhiteSpace(txtTransferDate.Text))
                {
                    bbl.ShowMessage("E102");
                    txtTransferDate.Focus();                   
                }
                else
                {
                    mce.CalendarDate = txtTransferDate.Text;
                    DataTable dtC = new DataTable();
                    //dtC = fbbl.
                }
            }
        }
    }
}
