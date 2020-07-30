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
    public partial class Search_HacchuuShoriNO : FrmSubForm
    {
        private const string ProNm = "発注処理番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            StoreCD,
            StaffCD,
            COUNT
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        public string OperatorCD = string.Empty;
        public string HacchuuShoriNO = string.Empty;
        public string ChangeDate = string.Empty;
        public string storeCD = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        D_Order_Entity doe;
        M_SKU_Entity mse;
        IkkatuHacchuuNyuuryoku_BL tjbl;

        public Search_HacchuuShoriNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);

            tjbl = new IkkatuHacchuuNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD, ScStaff.TxtCode};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
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

        private D_Order_Entity GetSearchInfo()
        {
            doe = new D_Order_Entity
            {
                OrderDateFrom = detailControls[(int)EIndex.DayStart].Text,
                OrderDateTo = detailControls[(int)EIndex.DayEnd].Text,
                StaffCD =ScStaff.TxtCode.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };

            return doe;
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

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);

            switch (Index)
            {
                case 0: // F1:終了
                case 1:     //F2:
                case 2:     //F3:
                case 3:     //F4:
                case 4:     //F5:
                case 5: //F6:
                case 6://F7:
                case 7://F8:
                case 11:    //F12:
                    {
                        break;
                    }

                case 8: //F9:検索
                    break;

            }   //switch end

        }
        protected override void ExecSec()
        {
            GetData();
            EndSec();
        }

        protected override void ExecDisp()
        {
            bool exists = false;
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (i != (int)EIndex.StoreCD &&  !string.IsNullOrWhiteSpace(detailControls[i].Text))
                    exists = true;

                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            if(!exists)
            {
                tjbl.ShowMessage("E111");
                detailControls[0].Focus();
                return;
            }

            doe = GetSearchInfo();
            DataTable dt = tjbl.D_Order_SelectData_SeachHacchuuShoriNO(doe.StoreCD,doe.OrderDateFrom,doe.OrderDateTo,doe.StaffCD);
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
               tjbl.ShowMessage("E128");
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
        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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

                    detailControls[index].Text = tjbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!tjbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        tjbl.ShowMessage("E103");
                        return false;
                    }
                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.DayEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                tjbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedValue.Equals("-1"))
                    {
                        tjbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    //else
                    //{
                    //    //店舗権限のチェック、引数で処理可能店舗の配列をセットしたい
                    //    if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                    //    {
                    //        tjbl.ShowMessage("E141");
                    //        CboStoreCD.Focus();
                    //        return false;
                    //    }
                    //}
                    break;

                case (int)EIndex.StaffCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScStaff.LabelText = "";
                        return true;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = tjbl.GetDate() // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Staff_BL bl = new Staff_BL();
                    bool ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        tjbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        return false;
                    }
                    break;

                //case (int)EIndex.MitsumoriName:

                //    break;
            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            {
                HacchuuShoriNO = GvDetail.CurrentRow.Cells["colOrderProcessNO"].Value.ToString();
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
                    StaffCD = OperatorCD,  //パラメータでオペレータCDをセット
                    ChangeDate = tjbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                }
                CboStoreCD.SelectedValue = storeCD;

                detailControls[(int)EIndex.DayEnd].Text = tjbl.GetDate();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        ///// <summary>
        ///// 検索フォーム起動処理
        ///// </summary>
        ///// <param name="kbn"></param>
        ///// <param name="setCtl"></param>
        //private void SearchData(EsearchKbn kbn, Control setCtl)
        //{
        //    switch (kbn)
        //    {
        //        case EsearchKbn.Product:
        //            string ymd = tjbl.GetDate();
        //            using (Search_Product frmProduct = new Search_Product(ymd))
        //            {
        //                int index = Array.IndexOf(detailControls, setCtl);

        //                if (index.Equals((int)EIndex.JanCD))
        //                    frmProduct.Mode = "5";

        //                frmProduct.ShowDialog();

        //                if (!frmProduct.flgCancel)
        //                {

        //                    switch (index)
        //                    {
        //                        case (int)EIndex.JanCD:
        //                            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.JanCD].Text))
        //                                detailControls[(int)EIndex.JanCD].Text = frmProduct.JANCD;
        //                            else
        //                                detailControls[(int)EIndex.JanCD].Text = detailControls[(int)EIndex.JanCD].Text + "," + frmProduct.JANCD;

        //                            break;

        //                        case (int)EIndex.SKUCD:
        //                            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SKUCD].Text))
        //                                detailControls[(int)EIndex.SKUCD].Text = frmProduct.SKUCD;
        //                            else
        //                                detailControls[(int)EIndex.SKUCD].Text = detailControls[(int)EIndex.SKUCD].Text + "," + frmProduct.SKUCD;

        //                            break;

        //                        case (int)EIndex.ITemCD:
        //                            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ITemCD].Text))
        //                                detailControls[(int)EIndex.ITemCD].Text = frmProduct.ITEM;
        //                            else
        //                                detailControls[(int)EIndex.ITemCD].Text = detailControls[(int)EIndex.ITemCD].Text + "," + frmProduct.ITEM;

        //                            break;
        //                    }
                            
        //                }
        //                setCtl.Focus();
        //            }
        //    break;
        //    }

        //}
    }
}
