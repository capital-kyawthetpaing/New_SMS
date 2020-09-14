using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace HacchuuShoukai
{
    /// <summary>
    /// HacchuuShoukai 発注照会
    /// </summary>
    internal partial class HacchuuShoukai : FrmMainForm
    {
        private const string ProID = "HacchuuShoukai";
        private const string ProNm = "発注照会";

        private enum EIndex : int
        {
            DayStart,
            DayEnd,
            ArrivalPlanDateFrom ,
            ArrivalPlanDateTo,
            ChkMikakutei,
            CboMikakutei,
            ChkKanbai,
            ChkFuyo,
            ChkNyukaZumi,
            ChkMiNyuka,
            ArrivalDateFrom,
            ArrivalDateTo,
            PurchaseDateFrom,
            PurchaseDateTo,
            JuchuNo,

            VendorCD,
            MakerItem,
            ITemCD,
            SKUCD,
            JanCD,
            SKUName,
            ChkJuchuAri,
            ChkZaiko,
            ChkMisyonin,
            ChkSyoninzumi,

            ChkChokuso,
            ChkSouko,
            CboSouko,
            ChkNet,
            ChkFax,
            ChkEdi,

            StoreCD,
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
        private Control[] detailControls;
        private D_Order_Entity doe;
        private M_SKU_Entity mse;
        private HacchuuShoukai_BL ssbl;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public HacchuuShoukai()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.SHOW);
                this.InitialControlArray();

                Btn_F10.Text = "出力(F10)";

                //起動時共通処理
                base.StartProgram();

                //初期値セット
                ssbl = new HacchuuShoukai_BL();
                string ymd = ssbl.GetDate();
                CboStoreCD.Bind(ymd);
                CboSoukoName.Bind(ymd);
                CboArrivalPlan.Bind(ymd);
                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScJuchuuNO.Value1 = InOperatorCD;
                ScJuchuuNO.Value2 = stores;
                ScVendor.Value1 = "1";

                SetFuncKeyAll(this, "100001000010");
                Scr_Clr(0);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private D_Order_Entity GetSearchInfo()
        {
            doe = new D_Order_Entity
            {
                OrderDateFrom = detailControls[(int)EIndex.DayStart].Text,
                OrderDateTo = detailControls[(int)EIndex.DayEnd].Text,
                ArrivalPlanDateFrom = detailControls[(int)EIndex.ArrivalPlanDateFrom].Text,
                ArrivalPlanDateTo = detailControls[(int)EIndex.ArrivalPlanDateTo].Text,
                ArrivalDateFrom = detailControls[(int)EIndex.ArrivalDateFrom].Text,
                ArrivalDateTo = detailControls[(int)EIndex.ArrivalDateTo].Text,
                PurchaseDateFrom = detailControls[(int)EIndex.PurchaseDateFrom].Text,
                PurchaseDateTo = detailControls[(int)EIndex.PurchaseDateTo].Text,

                OrderCD = ScVendor.TxtCode.Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
                JuchuuNO = ScJuchuuNO.TxtCode.Text,
            };

            if (CboArrivalPlan.SelectedIndex > 0)
                doe.ArrivalPlanCD = CboArrivalPlan.SelectedValue.ToString();

            if (CboSoukoName.SelectedIndex > 0)
                doe.DestinationSoukoCD = CboSoukoName.SelectedValue.ToString();

            if (((CheckBox)detailControls[(int)EIndex.ChkMikakutei]).Checked)
            {
                doe.ChkMikakutei = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkKanbai]).Checked)
            {
                doe.ChkKanbai = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkFuyo]).Checked)
            {
                doe.ChkFuyo = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkNyukaZumi]).Checked)
            {
                doe.ChkNyukaZumi = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMiNyuka]).Checked)
            {
                doe.ChkMiNyuka = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkJuchuAri]).Checked)
            {
                doe.ChkJuchuAri = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkZaiko]).Checked)
            {
                doe.ChkZaiko = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMisyonin]).Checked)
            {
                doe.ChkMisyonin = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkSyoninzumi]).Checked)
            {
                doe.ChkSyoninzumi = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkChokuso]).Checked)
            {
                doe.ChkChokuso = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkSouko]).Checked)
            {
                doe.ChkSouko = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkNet]).Checked)
            {
                doe.ChkNet = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkFax]).Checked)
            {
                doe.ChkFax = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkEdi]).Checked)
            {
                doe.ChkEdi = 1;
            }

            mse = new M_SKU_Entity
            {
                SKUName = detailControls[(int)EIndex.SKUName].Text,
                ITemCD = detailControls[(int)EIndex.ITemCD].Text,           //カンマ区切り
                SKUCD = detailControls[(int)EIndex.SKUCD].Text,//カンマ区切り
                JanCD = detailControls[(int)EIndex.JanCD].Text,//カンマ区切り
                MakerItem = detailControls[(int)EIndex.MakerItem].Text,     //カンマ区切り
            };

            return doe;
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //受注あり
            if (!((CheckBox)detailControls[(int)EIndex.ChkJuchuAri]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkZaiko]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkJuchuAri].Focus();
                return;
            }
            //承認状況
            if (!((CheckBox)detailControls[(int)EIndex.ChkSyoninzumi]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkMisyonin]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkMisyonin].Focus();
                return;
            }
            //納入先
            if (!((CheckBox)detailControls[(int)EIndex.ChkChokuso]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkSouko]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkChokuso].Focus();
                return;
            }
            //発注方法
            if (!((CheckBox)detailControls[(int)EIndex.ChkNet]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkFax]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkEdi]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkNet].Focus();
                return;
            }

            doe = GetSearchInfo();
            DataTable dt = ssbl.D_Order_SelectAllForShoukai(doe, mse, InOperatorCD, InPcID);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
                Btn_F10.Enabled = true;
            }
            else
            {
                ssbl.ShowMessage("E128");
            }
        }

        private void ExecOutput()
        {

            if (GvDetail.Rows.Count>0)
            {
                string filePath = "";
                if (!ShowSaveFileDialog(InProgramNM, out filePath,1))
                {
                    return;
                }

                //Excel出力
                OutputExecel(this.GvDetail, filePath);

                //ファイル出力が完了しました。
                bbl.ShowMessage("I203");
            }
        }
        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2, ckM_TextBox10, ckM_TextBox9
                ,ckM_CheckBox7,CboArrivalPlan,ckM_CheckBox5,ckM_CheckBox6,ckM_CheckBox1,ckM_CheckBox2
                ,ckM_TextBox14, ckM_TextBox13,ckM_TextBox12, ckM_TextBox11,ScJuchuuNO.TxtCode
                ,ScVendor.TxtCode, ckM_TextBox4,ckM_TextBox8, ckM_TextBox6, ckM_TextBox7, ckM_TextBox5
                ,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11
                ,ckM_CheckBox4,ckM_CheckBox3,CboSoukoName,ckM_CheckBox12,ckM_CheckBox13,ckM_CheckBox14
                , CboStoreCD
                 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            btnSearchItem.Click += new System.EventHandler(BtnSearch_Click);
            btnSearchSKUCD.Click += new System.EventHandler(BtnSearch_Click);
            btnSearchJANCD.Click += new System.EventHandler(BtnSearch_Click);
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.ArrivalPlanDateFrom:
                case (int)EIndex.ArrivalPlanDateTo:
                case (int)EIndex.ArrivalDateFrom:
                case (int)EIndex.ArrivalDateTo:
                case (int)EIndex.PurchaseDateFrom:
                case (int)EIndex.PurchaseDateTo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.ArrivalPlanDateTo || index== (int)EIndex.ArrivalDateTo || index== (int)EIndex.PurchaseDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedIndex == -1)
                    {
                        bbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    else
                    {
                        //店舗権限のチェック
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
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

                    //string ymd = detailControls[(int)EIndex.]
                    //[M_VendorCD_Select]
                    M_Vendor_Entity mce = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    bool ret = sbl.M_Vendor_SelectTop1(mce);
                    if (ret)
                    {
                        ScVendor.LabelText = mce.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScVendor.LabelText = "";
                        return false;
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else
                {
                    ctl.Text = "";
                }
            }

            ScVendor.LabelText = "";
            //foreach (Control ctl in detailLabels)
            //{
            //    ((CKM_SearchControl)ctl).LabelText = "";
            //}


            //初期値セット
            string ymd = ssbl.GetDate();
            detailControls[(int)EIndex.DayEnd].Text = ymd;

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = ymd
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
            }

            //[M_Store]
            M_Store_Entity mse2 = new M_Store_Entity
            {
                StoreCD = mse.StoreCD,
                ChangeDate = ymd
            };
            Store_BL sbl = new Store_BL();
            DataTable dt = sbl.M_Store_Select(mse2);
            if (dt.Rows.Count > 0)
            {
            }
            else
            {
                bbl.ShowMessage("E133");
                EndSec();
            }

            ((CheckBox)detailControls[(int)EIndex.ChkMiNyuka]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkNyukaZumi]).Checked = true;

            for (int i = (int)EIndex.ChkJuchuAri; i <= (int)EIndex.ChkEdi; i++)
            {
                if (detailControls[i].GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)detailControls[i]).Checked = true;
                }
            }

            GvDetail.DataSource = null;
            GvDetail.Enabled = false;
            Btn_F10.Enabled = false;

            detailControls[0].Focus();
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScVendor.LabelText = "";
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
                    {
                        break;
                    }
                case 1:     //F2:新規
                case 2:     //F3:変更
                case 3:     //F4:削除
                case 4:     //F5:照会
                    {
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        Scr_Clr(0);

                        break;
                    }
                case 8:
                    EsearchKbn kbn = EsearchKbn.Null;

                    if (Array.IndexOf(detailControls, PreviousCtrl) == (int)EIndex.ITemCD)
                    {
                        //商品検索
                        kbn = EsearchKbn.Product;
                    }
                    else if (Array.IndexOf(detailControls, PreviousCtrl) == (int)EIndex.SKUCD)
                    {
                        //商品検索
                        kbn = EsearchKbn.Product;
                    }
                    else if (Array.IndexOf(detailControls, PreviousCtrl) == (int)EIndex.JanCD)
                    {
                        //商品検索
                        kbn = EsearchKbn.Product;
                    }

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

                    break;
                case 9://F10:出力
                       //Ｑ２０５				
                    if (bbl.ShowMessage("Q205") != DialogResult.Yes)
                        return;

                    ExecOutput();
                    break;

                case 11:   
                        break;
                    
            }   //switch end

        }
        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
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
                    string ymd = bbl.GetDate();
                    int index = Array.IndexOf(detailControls, setCtl);

                    using (Search_Product frmProduct = new Search_Product(ymd))
                    {
                        if(index.Equals((int)EIndex.JanCD))
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

                                case (int)EIndex.ITemCD:
                                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ITemCD].Text))
                                        detailControls[(int)EIndex.ITemCD].Text = frmProduct.ITEM;
                                    else
                                        detailControls[(int)EIndex.ITemCD].Text = detailControls[(int)EIndex.ITemCD].Text + "," + frmProduct.ITEM;

                                    break;
                            }

                        }
                        setCtl.Focus();
                    }
                    break;
            }

        }

        #region "内部イベント"
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
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        } else
                        {
                            ExecDisp();
                        }
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
                switch (index)
                {
                    case (int)EIndex.VendorCD:
                        case (int)EIndex.JuchuNo:
                        case (int)EIndex.SKUCD:
                        case (int)EIndex.JanCD:
                        case (int)EIndex.ITemCD:
                        F9Visible = true;
                        break;

                    default:
                        F9Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;

                if (((Control)sender).Name.Equals(btnSearchItem.Name))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                    setCtl = detailControls[(int)EIndex.ITemCD];
                }
                else if (((Control)sender).Name.Equals(btnSearchSKUCD.Name))
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
        private void BtnSubF11_Click(object sender, EventArgs e)
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
        private void ChkSouko_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (((CheckBox)sender).Checked)
                    CboSoukoName.Enabled = true;
                else
                {
                    CboSoukoName.SelectedIndex = -1;
                    CboSoukoName.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        #endregion
    }
}








