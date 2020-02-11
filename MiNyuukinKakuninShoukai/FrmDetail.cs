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

namespace MiNyuukinKakuninShoukai
{
    public partial class FrmDetail : Form
    {
        #region"公開プロパティ"
        
        public DataTable dt;
        #endregion

        private MiNyuukinKakuninShoukai_BL mibl;

        public FrmDetail()
        {
            InitializeComponent();
            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                mibl = new MiNyuukinKakuninShoukai_BL();
                ExecDisp();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void ExecDisp()
        {
            if (dt.Rows.Count > 0)
            {
                decimal kin = 0;
                foreach(DataRow row in dt.Rows)
                {
                    kin += mibl.Z_Set(row["CollectAmount"]);
                }
                label1.Text = mibl.Z_SetStr(kin);
                dgvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dgvDetail.CurrentRow.Selected = true;
                dgvDetail.Enabled = true;
                dgvDetail.Focus();
                dgvDetail.Select();
            }
            else
            {
                mibl.ShowMessage("E128");
                this.Close();
            }
        }
    }
}
