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
using Entity;
using BL;

namespace UrikakekinTairyuuHyou
{
    public partial class UrikakekinTairyuuHyou : FrmMainForm
    {
        string todayDate = DateTime.Now.ToString("yyyy/MM/dd");
        M_StoreClose_Entity msce;
        UrikakekinTairyuuHyou_BL ukkthbl;
        public UrikakekinTairyuuHyou()
        {
            InitializeComponent();
        }

        private void UrikakekinTairyuuHyou_Load(object sender, EventArgs e)
        {
            InProgramID = "UrikakekinTairyuuHyou";

            ukkthbl = new UrikakekinTairyuuHyou_BL();

            SetFunctionLabel(EProMode.MENTE);
            SetFunctionLabel(EProMode.PRINT);
            StartProgram();

            ModeVisible = false;
            Btn_F2.Text = string.Empty;
            Btn_F9.Text = string.Empty;
            Btn_F10.Text = string.Empty;

            Btn_F11.Text = "Excel(F11)";

            BindData();
            SetRequireField();
        }

        private void BindData()
        {
            txtDate.Text = todayDate.Substring(0,todayDate.Length-3);
            txtDate.Focus();
            cboStore.Bind(todayDate, "2");
            cboStore.SelectedValue = StoreCD;
        }

        private void SetRequireField()
        {
            txtDate.Require(true);
            cboStore.Require(true);
        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { txtDate, cboStore }))
                return false;

            return true;
        }


        /// <summary>
        /// Handle F1 to F12 Click
        /// </summary>
        /// <param name="index"> button index+1, eg.if index is 0,it means F1 click </param>
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index)
            {
                case 0: // F1:終了
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        Clear();
                        break;
                    }

                case 10: //F11
                    ExcelExport(); break;
                case 11:
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }

        private M_StoreClose_Entity GetStoreClose_Data()
        {
            msce = new M_StoreClose_Entity()
            {
                StoreCD = cboStore.SelectedValue.ToString(),
                FiscalYYYYMM = txtDate.Text.Replace("/", ""),
            };
            return msce;
        }

        public bool CheckBeforeExport()
        {
            msce = new M_StoreClose_Entity();
            msce = GetStoreClose_Data();
            if (ukkthbl.M_StoreClose_Check(msce, "1").Rows.Count > 0)
            {
                //    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                //    {

                //    }
                return true;
            }
            return false;
        }

        public void ExcelExport()
        {
            if(ErrorCheck())
            {
                if(!CheckBeforeExport())
                {
                    if (bbl.ShowMessage("Q205") == DialogResult.Yes)
                    {
                        msce = new M_StoreClose_Entity();
                        msce = GetStoreClose_Data();
                        DataTable dt=ukkthbl.Select_DataToExport(msce);
                    }
                }
            }
        }

        public void Clear()
        {
            txtDate.Text = string.Empty;
            cboStore.SelectedValue = StoreCD;
        }
    }
}
