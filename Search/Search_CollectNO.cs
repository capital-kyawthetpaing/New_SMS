using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;

namespace Search
{
    public partial class Search_CollectNO : FrmSubForm
    {
        private const string ProNm = "入金番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            StoreCD,
            InputStart,
            InputEnd,
            CustomerCD,
            //CustomerName
        }
        public string OperatorCD = string.Empty;
        public string CollectNO = string.Empty;
        public string ChangeDate = string.Empty;

        private Control[] detailControls;
        D_Collect_Entity dce;
        NyuukinNyuuryoku_BL nnbl;

        public Search_CollectNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);

            nnbl = new NyuukinNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD, ckM_TextBox3, ckM_TextBox4,ScCustomer.TxtCode };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        private void BtnF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                base.FunctionProcess(FuncDisp - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private D_Collect_Entity GetSearchInfo()
        {
            dce = new D_Collect_Entity
            {
                CollectDateFrom = detailControls[(int)EIndex.DayStart].Text,
                CollectDateTo = detailControls[(int)EIndex.DayEnd].Text,
                InputDateFrom = detailControls[(int)EIndex.InputStart].Text,
                InputDateTo = detailControls[(int)EIndex.InputEnd].Text,
                CollectCustomerCD=ScCustomer.TxtCode.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };

            return dce;
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        this.ExecSec();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        protected override void ExecSec()
        {
            GetData();
            EndSec();
        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            dce = GetSearchInfo();
            DataTable dt = nnbl.D_Collect_SelectForSearch(dce);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               nnbl.ShowMessage("E128");
                GvDetail.DataSource = null;
                detailControls[0].Focus();
            }
        }
        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
                    if (ret)
                    {
                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();
                        else
                            btnSubF11.Focus();
                    }
                    else
                    {
                        ((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    btnSubF11.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }
        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.InputStart:
                case (int)EIndex.InputEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = nnbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!nnbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        nnbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.InputEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                nnbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedIndex == -1)
                    {
                        nnbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            nnbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }
                    break;

                case (int)EIndex.CustomerCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScCustomer.LabelText = "";
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = nnbl.GetDate()     // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sbl = new Customer_BL();
                  bool  ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        ScCustomer.LabelText = mce.CustomerName;
                    }
                    else
                    {
                        nnbl.ShowMessage("E101");
                        ScCustomer.LabelText = "";
                        return false;
                    }

                    break;
            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                CollectNO = GvDetail.CurrentRow.Cells["colCollectNO"].Value.ToString();
                //ChangeDate = GvDetail.CurrentRow.Cells["ColChangeDate"].Value.ToString();
            }
        }

        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                Scr_Clr();

                ScCustomer.Value1 = "3";

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = OperatorCD,       //パラメータでオペレータCDをセット
                    ChangeDate = nnbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
    }
}
