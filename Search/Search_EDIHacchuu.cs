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

namespace Search
{
    public partial class Search_EDIHacchuuNO : FrmSubForm
    {
        private const string ProNm = "EDI処理番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            StoreCD,
            VendorCD,
        }
        public string OperatorCD = string.Empty;
        public string EDIOrderNO = string.Empty;
        public string ChangeDate = string.Empty;

        private Control[] detailControls;
        private D_EDIOrder_Entity dee;
        private EDIHacchuu_BL ehbl;

        public Search_EDIHacchuuNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);

            ehbl = new EDIHacchuu_BL();
            
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD, ScVendor.TxtCode };

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

        private D_EDIOrder_Entity GetSearchInfo()
        {
            dee = new D_EDIOrder_Entity
            {
                OrderDateFrom = detailControls[(int)EIndex.DayStart].Text,
                OrderDateTo = detailControls[(int)EIndex.DayEnd].Text,
                VendorCD=ScVendor.TxtCode.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };

            return dee;
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

            dee = GetSearchInfo();
            DataTable dt = ehbl.D_EDIOrder_SelectAll(dee);
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
                ehbl.ShowMessage("E128");
                GvDetail.DataSource = null;
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
                        if ((int)EIndex.VendorCD == Array.IndexOf(detailControls, sender))
                            return;
                        else
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();
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
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = ehbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!ehbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        ehbl.ShowMessage("E103");
                        return false;
                    }
                    //処理日(From) ≧ 処理日(To)である場合Error
                    if (index == (int)EIndex.DayEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                ehbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ehbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            ehbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }

                    }
                    break;

                case (int)EIndex.VendorCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScVendor.LabelText = "";
                        return true;
                    }

                    //仕入先マスター(M_Vendor)に存在すること
                    //[M_Vendor]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = ehbl.GetDate() ,
                        VendorFlg = "1"
                    };
                    Vendor_BL bl = new Vendor_BL();
                    bool ret = bl.M_Vendor_SelectTop1(mve);
                    if (ret)
                    {
                        ScVendor.LabelText = mve.VendorName;
                    }
                    else
                    {
                        ehbl.ShowMessage("E101");
                        ScVendor.LabelText = "";
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
                EDIOrderNO = GvDetail.CurrentRow.Cells["colEDIOrderNO"].Value.ToString();
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

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = OperatorCD,       //パラメータでオペレータCDをセット
                    ChangeDate = ehbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }

                F9Visible = false;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
