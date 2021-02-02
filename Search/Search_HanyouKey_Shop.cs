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
    public partial class Search_HanyouKey_Shop : Search_Base
    {

        private const string ProNm = "汎用検索";

        #region"公開プロパティ"
        public string parID = "";
        public string parKey = "";
        public string parChar1 = "";
        #endregion

        MultiPorpose_BL mbl;
        M_MultiPorpose_Entity mme;
        public Search_HanyouKey_Shop()
        {
            InitializeComponent();
        }

        private void Search_HanyouKey_Shop_Load(object sender, EventArgs e)
        {
            try
            {

                string title = "";
                if (parID.Equals(MultiPorpose_BL.ID_Mail))
                {
                    title = "モール";
                }
                else if (parID.Equals(MultiPorpose_BL.ID_TANI))
                {
                    title = "単位";
                }
                else if (parID.Equals(MultiPorpose_BL.ID_SPORTS))
                {
                    title = "競技";
                }
                else if (parID.Equals(MultiPorpose_BL.ID_SegmentCD))
                {
                    title = "分類";
                }

                HeaderTitleText = title;
                this.Text = title + "検索";
                this.colChar1.HeaderText = title + "名";

                F11();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        public override void FunctionProcess(int index)
        {
            if (index + 11 == 12)
            {
                GetData();
            }
        }

       private void F11()
        {
            mbl = new MultiPorpose_BL();
            bool ret = BindGrid();

            if (ret)
            {
                dgvHanyou.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvHanyou.CurrentRow.Selected = true;
                dgvHanyou.Enabled = true;
                dgvHanyou.Focus();
                dgvHanyou.Select();
            }
            else
            {
                mbl.ShowMessage("E128");
                dgvHanyou.DataSource = null;
            }
        }

        private bool BindGrid()
        {
            mme = GetEntity();
            DataTable dt = mbl.M_MultiPorpose_SelectAll(mme);

            if (dt.Rows.Count == 0)
                return false;

            dgvHanyou.DataSource = dt;

            return true;
        }

        private M_MultiPorpose_Entity GetEntity()
        {
            mme = new M_MultiPorpose_Entity();
            mme.ID = parID;
            return mme;
        }

        private void GetData()
        {
            if (dgvHanyou.CurrentRow is null)
                return;

            parKey = dgvHanyou.CurrentRow.Cells["colKey"].Value.ToString();
            parChar1 = dgvHanyou.CurrentRow.Cells["colChar1"].Value.ToString();
            this.Close();
        }

        private void dgvHanyou_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GetData();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvHanyou_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        GetData();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        protected override void EndSec()
        {
            this.Close();
        }
    }
}
