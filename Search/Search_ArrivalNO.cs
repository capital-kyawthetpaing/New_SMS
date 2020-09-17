using System;
using System.Data;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;

namespace Search
{
    public partial class Search_ArrivalNO : FrmSubForm
    {
        private const string ProNm = "入荷番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            SoukoCD,
            SKUCD,
            JanCD,
            VendorDeliveryNo,
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
        public string ArrivalNO = string.Empty;
        public string ChangeDate = string.Empty;
        public string SoukoCD = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        D_Arrival_Entity dae;
        NyuukaNyuuryoku_BL nnbl;

        public Search_ArrivalNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            CboStoreCD.Bind(changeDate);

            nnbl = new NyuukaNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboStoreCD,  ckM_TextBox6, ckM_TextBox7, ckM_TextBox4};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
            
            btnSearchSKUCD.Click += new System.EventHandler(BtnSearch_Click);
            btnSearchJANCD.Click += new System.EventHandler(BtnSearch_Click);
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

        private D_Arrival_Entity GetSearchInfo()
        {
            dae = new D_Arrival_Entity
            {
                ArrivalDateFrom = detailControls[(int)EIndex.DayStart].Text,
                ArrivalDateTo = detailControls[(int)EIndex.DayEnd].Text,
                SoukoCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
                SKUCD = detailControls[(int)EIndex.SKUCD].Text,//カンマ区切り
                JanCD = detailControls[(int)EIndex.JanCD].Text,//カンマ区切り
                VendorDeliveryNo = detailControls[(int)EIndex.VendorDeliveryNo].Text,     //カンマ区切り
            };

            return dae;
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
                    EsearchKbn kbn = EsearchKbn.Null;
                    int index = Array.IndexOf(detailControls, previousCtrl);

                    switch(index)
                    {
                        case (int)EIndex.SKUCD:
                        case (int)EIndex.JanCD:
                            //商品検索
                            kbn = EsearchKbn.Product;
                            break;
                    }

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

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
                if (i != (int)EIndex.SoukoCD &&  !string.IsNullOrWhiteSpace(detailControls[i].Text))
                    exists = true;

                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            if(!exists)
            {
                nnbl.ShowMessage("E111");
                detailControls[0].Focus();
                return;
            }

            dae = GetSearchInfo();
            DataTable dt = nnbl.D_Arrival_SelectAll(dae);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count>0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

                    detailControls[index].Text = nnbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!nnbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        nnbl.ShowMessage("E103");
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
                                nnbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireComboCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

               
            }

            return true;
        }
        private bool RequireComboCheck(Control[] ctrl)
        {
            foreach (Control c in ctrl)
            {
                if (c is ComboBox)
                {
                    if (((ComboBox)c).SelectedIndex.Equals(-1))
                    {
                        nnbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                    if (((ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        nnbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                ArrivalNO = GvDetail.CurrentRow.Cells["colArrivalNO"].Value.ToString();
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
                detailControls[(int)EIndex.DayStart].Text = nnbl.GetDate();
                detailControls[(int)EIndex.DayEnd].Text = nnbl.GetDate();
                CboStoreCD.SelectedValue = SoukoCD; 
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;

                if(((Control)sender).Name.Equals(btnSearchSKUCD.Name))
                    {
                    //商品検索
                    kbn = EsearchKbn.Product;
                    setCtl = detailControls[(int)EIndex.SKUCD];
                }
                else if (((Control)sender).Name.Equals(btnSearchJANCD.Name))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                    setCtl = detailControls[(int)EIndex.JanCD];
                }
                
                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Product:
                    string ymd = nnbl.GetDate();
                    using (Search_Product frmProduct = new Search_Product(ymd))
                    {
                        int index = Array.IndexOf(detailControls, setCtl);

                        if (index.Equals((int)EIndex.JanCD))
                            frmProduct.Mode = "5";

                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {

                            switch (index)
                            {
                                case (int)EIndex.JanCD:
                                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.JanCD].Text))
                                        detailControls[(int)EIndex.JanCD].Text = frmProduct.JANCD;
                                    else
                                        detailControls[(int)EIndex.JanCD].Text = detailControls[(int)EIndex.JanCD].Text + "," + frmProduct.JANCD;

                                    break;

                                case (int)EIndex.SKUCD:
                                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SKUCD].Text))
                                        detailControls[(int)EIndex.SKUCD].Text = frmProduct.SKUCD;
                                    else
                                        detailControls[(int)EIndex.SKUCD].Text = detailControls[(int)EIndex.SKUCD].Text + "," + frmProduct.SKUCD;

                                    break;
                            }
                            
                        }
                        setCtl.Focus();
                    }
            break;
            }

        }
    }
}
