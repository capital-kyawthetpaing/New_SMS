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
    public partial class Search_ShippingNO : FrmSubForm
    {
        private const string ProNm = "出荷番号検索";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            SoukoCD,
            SKUCD,
            JanCD,
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
        public string ShippingNO = string.Empty;
        public string ChangeDate = string.Empty;
        public string SoukoCD = string.Empty;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private Control[] detailControls;
        D_Shipping_Entity dse;
        ShukkaNyuuryoku_BL snbl;

        public Search_ShippingNO(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;

            snbl = new ShukkaNyuuryoku_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2,CboSoukoCD,  ckM_TextBox6, ckM_TextBox7};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
                ctl.Leave += new System.EventHandler(DetailControl_Leave);
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

        private D_Shipping_Entity GetSearchInfo()
        {
            //エラーで落ちるので、スペースは省く
            string skuCD = string.Empty;
            string[] arr = detailControls[(int)EIndex.SKUCD].Text.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(arr[i]))
                {
                    if (skuCD != string.Empty)
                        skuCD += ",";

                    skuCD += arr[i];
                }                
            }

            string janCD = string.Empty;
            arr = detailControls[(int)EIndex.JanCD].Text.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(arr[i]))
                {
                    if (janCD != string.Empty)
                        janCD += ",";
                    janCD += arr[i];
                }                
            }


            dse = new D_Shipping_Entity
            {
                ShippingDateFrom = detailControls[(int)EIndex.DayStart].Text,
                ShippingDateTo = detailControls[(int)EIndex.DayEnd].Text,
                SoukoCD = CboSoukoCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboSoukoCD.SelectedValue.ToString(),
                
                SKUCD = skuCD,//カンマ区切り
                JanCD = janCD,//カンマ区切り
            };

            return dse;
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
                case 5:     //F6:
                case 6:     //F7:
                case 7:     //F8:
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
                snbl.ShowMessage("E111");
                detailControls[0].Focus();
                return;
            }

            dse = GetSearchInfo();
            DataTable dt = snbl.D_Shipping_SelectAll(dse);
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
               snbl.ShowMessage("E128");
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

                int index = Array.IndexOf(detailControls, sender);
                if (index == (int)EIndex.SKUCD || index == (int)EIndex.JanCD)
                {
                    F9Visible = true;
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailControl_Leave(object sender, EventArgs e)
        {
            try
            {
                F9Visible = false;

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

                    detailControls[index].Text = snbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!snbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        snbl.ShowMessage("E103");
                        return false;
                    }
                    //出荷日(From) ≧ 出荷日(To)である場合Error
                    if (index == (int)EIndex.DayEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                snbl.ShowMessage("E104");
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

                case (int)EIndex.SKUCD:
                    string[] arr = detailControls[index].Text.Split(',');
                    if (arr.Length > 6)
                    {
                        //Ｅ１８７
                        snbl.ShowMessage("E187","6個");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)EIndex.JanCD:
                    arr = detailControls[index].Text.Split(',');
                    if (arr.Length > 12)
                    {
                        //Ｅ１８７
                        snbl.ShowMessage("E187", "1個");
                        detailControls[index].Focus();
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
                        snbl.ShowMessage("E102");
                        c.Focus();
                        return false;
                    }
                    if (((ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        snbl.ShowMessage("E102");
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
                ShippingNO = GvDetail.CurrentRow.Cells["colShippingNO"].Value.ToString();
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
                detailControls[(int)EIndex.DayStart].Text = snbl.GetDate();
                detailControls[(int)EIndex.DayEnd].Text = snbl.GetDate();

                string ymd = snbl.GetDate();
                CboSoukoCD.Bind(ymd);

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = OperatorCD,
                    ChangeDate = snbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    //[M_Souko_Select]
                    M_Souko_Entity me = new M_Souko_Entity
                    {
                        StoreCD = mse.StoreCD,
                        SoukoType = "3",
                        ChangeDate = ymd,
                        DeleteFlg = "0"
                    };

                    DataTable mdt = snbl.M_Souko_SelectForNyuuka(me);
                    if (mdt.Rows.Count > 0)
                    {
                        CboSoukoCD.SelectedValue = mdt.Rows[0]["SoukoCD"];
                    }
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
                    string ymd = snbl.GetDate();
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
                                    else if (!string.IsNullOrWhiteSpace(frmProduct.JANCD))
                                        detailControls[(int)EIndex.JanCD].Text = detailControls[(int)EIndex.JanCD].Text + "," + frmProduct.JANCD;

                                    break;

                                case (int)EIndex.SKUCD:
                                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.SKUCD].Text))
                                        detailControls[(int)EIndex.SKUCD].Text = frmProduct.SKUCD;
                                    else if (!string.IsNullOrWhiteSpace(frmProduct.SKUCD))
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
