using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Base.Client;
using BL;
using Entity;
using Search;

namespace JANCDHenkou
{
    public partial class JANCDHenkou : FrmMainForm
    {
        JANCDHenkou_BL jhbl;
        public JANCDHenkou()
        {
            InitializeComponent();
            jhbl = new JANCDHenkou_BL();
        }

        private void JANCDHenkou_Load(object sender, EventArgs e)
        {
            InProgramID = Application.ProductName;
            StartProgram();
            SetFunctionLabel(EProMode.KehiNyuuryoku);
            Btn_F2.Text = string.Empty;
            Btn_F3.Text = string.Empty;
            Btn_F4.Text = string.Empty;
            Btn_F5.Text = string.Empty;
            Btn_F7.Text = string.Empty;
            Btn_F8.Text = string.Empty;
            Btn_F10.Text = string.Empty;
            Btn_F11.Text = "取込(F11)";
        }
        private void JANCDHenkou_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
        public override void FunctionProcess(int index)
        {
            base.FunctionProcess(index);
            switch (index + 1)
            {
                case 6: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                        {
                            Clear();
                        }
                        break;
                    }
                case 11:
                  //  F11();
                    break;
                case 12:
                    F12();
                    break;
            }
        }
        protected override void EndSec()
        {
            this.Close();
        }

        /// <summary>
        /// Clear data of Panel Detail 
        /// </summary>
        public void Clear()
        {
            Clear(panelDetail);
            //txtTargetPeriodF.Focus();
        }

        /// <summary>
        /// Show select_SKU form on gridview JanCD button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvJANCDHenkou_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                Select_SKU frmSku = new Select_SKU();
                frmSku.parJANCD = dgvJANCDHenkou.Rows[e.RowIndex].Cells["colGenJanCD"].Value.ToString();
                frmSku.parChangeDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                frmSku.ShowDialog();
            }
        }

        private bool ErrorCheck()
        {
            foreach (DataGridViewRow row in dgvJANCDHenkou.Rows)
            {
                if (!(row.Cells["colGenJanCD"].Value == null))
                {
                    if (jhbl.SimpleSelect1("58", System.DateTime.Now.ToString("yyyy-MM-dd"),row.Cells["colGenJanCD"].Value.ToString()).Rows.Count < 1)
                    {
                        jhbl.ShowMessage("E101");
                        return false;
                    }
                }

               if(!(row.Cells["colNewJANCD"].Value == null))
                {
                    if (jhbl.SimpleSelect1("59", System.DateTime.Now.ToString("yyyy-MM-dd"), row.Cells["colNewJanCD"].Value.ToString()).Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        {
                            //if(dr ==  DialogResult.No)

                        }
                    }
                }
            }
            return true;
        }

        private void F11()
        {
            ErrorCheck();
        }

        private void F12()
        {
            ErrorCheck();
        }

        private void BtnF11Show_Click(object sender, EventArgs e)
        {
            F11();
        }
    }
}
