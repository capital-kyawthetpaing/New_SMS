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
                    //case 11:
                    //    PrintSec();
                    //    break;
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
    }
}
