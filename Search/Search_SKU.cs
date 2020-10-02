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

namespace Search
{
    public partial class frmSearch_SKU : Search_Base
    {
        //public frmSearch_SKU()
        //{
        //    InitializeComponent();
        //}
        private const string ProNm = "SKU選択";
        public string SKUCD = string.Empty;
        public string SKUName = string.Empty;
        public string AdminNO = string.Empty;
        public string SizeColorName = string.Empty;

        
        public frmSearch_SKU(string janCD,DataTable dtSKU)
        {
            InitializeComponent();
            lblJanCD.Text = janCD;
            //this.Text = ProNm;
            ProgramName = "SKU選択";
            GvMultiSKU.DataSource = dtSKU;
            GvMultiSKU.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            GvMultiSKU.CurrentRow.Selected = true;
            GvMultiSKU.Enabled = true;
            GvMultiSKU.Focus();

        }

        private void GvMultiSKU_DoubleClick(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (GvMultiSKU.CurrentRow != null && GvMultiSKU.CurrentRow.Index >= 0)
            {
                SKUCD = GvMultiSKU.CurrentRow.Cells["colSKUCD"].Value.ToString();
                SKUName = GvMultiSKU.CurrentRow.Cells["colSKUName"].Value.ToString();
                AdminNO = GvMultiSKU.CurrentRow.Cells["colAdminNO"].Value.ToString();
                SizeColorName = GvMultiSKU.CurrentRow.Cells["colColorName"].Value.ToString()+" . " + GvMultiSKU.CurrentRow.Cells["colSizeName"].Value.ToString();
                this.Close();
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }

        public override void FunctionProcess(int Index)
        {
            if (Index + 11 == 12)
            {
                GetData();
            }
        }
    }
}
