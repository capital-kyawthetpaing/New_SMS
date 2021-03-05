using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace TempoJuchuuShoukai
{
    /// <summary>
    /// TempoJuchuuShoukai 店舗受注照会
    /// </summary>
    internal partial class TempoJuchuuShoukai : FrmMainForm
    {
        private const string ProID = "TempoJuchuuShoukai";
        private const string ProNm = "店舗受注照会";
        private const string Juchuu = "TempoJuchuuNyuuryoku.exe";

        private enum EIndex : int
        {

            ChkMihikiate,
            ChkMiuriage,
            ChkMiseikyu,
            ChkMinyukin,
            ChkAll,
            ChkReji,
            ChkGaisho,
            ChkTujo,
            ChkHenpin,

            ChkMihachu,
            ChkNokiKaito,
            ChkMinyuka,
            ChkMisiire,
            ChkHachuAll,

            StoreCD,
            CustomerCD,
            KanaName,
            Tel1,
            Tel2,
            Tel3,
            VendorCD,
            StaffCD,

            DayStart,
            DayEnd,
            SalesDateFrom,
            SalesDateTo,
            BillingDateFrom,
            BillingDateTo,
            CollectDateFrom,
            CollectDateTo,
            JuchuNoFrom,
            JuchuNoTo,

            SKUCD,
            JanCD,
            SKUName,
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
        private D_Juchuu_Entity dje;
        private M_SKU_Entity mse;
        private TempoJuchuuShoukai_BL ssbl;
        private int mTennic;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        public TempoJuchuuShoukai()
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

                Btn_F2.Text = "新規受注(F2)";
                Btn_F10.Text = "出力(F10)";

                //起動時共通処理
                base.StartProgram();

                mTennic = bbl.GetTennic();

                //初期値セット
                ssbl = new TempoJuchuuShoukai_BL();
                string ymd = ssbl.GetDate();
                CboStoreCD.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScJuchuuNO.Value1 = InOperatorCD;
                ScJuchuuNO.Value2 = stores;
                ScJuchuuNOTo.Value1 = InOperatorCD;
                ScJuchuuNOTo.Value2 = stores;
                ScCustomer.Value1 = "1";

                SetFuncKeyAll(this, "110001000010");
                Scr_Clr(0);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private D_Juchuu_Entity GetSearchInfo()
        {
            dje = new D_Juchuu_Entity
            {
                JuchuDateFrom = detailControls[(int)EIndex.DayStart].Text,
                JuchuDateTo = detailControls[(int)EIndex.DayEnd].Text,
                SalesDateFrom = detailControls[(int)EIndex.SalesDateFrom].Text,
                SalesDateTo = detailControls[(int)EIndex.SalesDateTo].Text,
                BillingCloseDateFrom = detailControls[(int)EIndex.BillingDateFrom].Text,
                BillingCloseDateTo = detailControls[(int)EIndex.BillingDateTo].Text,
                CollectClearDateFrom = detailControls[(int)EIndex.CollectDateFrom].Text,
                CollectClearDateTo = detailControls[(int)EIndex.CollectDateTo].Text,
                JuchuuNOFrom = ScJuchuuNO.TxtCode.Text,
                JuchuuNOTo = ScJuchuuNOTo.TxtCode.Text,

                CustomerCD = ScCustomer.TxtCode.Text,
                KanaName = detailControls[(int)EIndex.KanaName].Text,
                Tel11 = detailControls[(int)EIndex.Tel1].Text,
                Tel12 = detailControls[(int)EIndex.Tel2].Text,
                Tel13 = detailControls[(int)EIndex.Tel3].Text,
                VendorCD = detailControls[(int)EIndex.VendorCD].Text,
                StaffCD = detailControls[(int)EIndex.StaffCD].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),
            };

            if (((CheckBox)detailControls[(int)EIndex.ChkMihikiate]).Checked)
            {
                dje.ChkMihikiate = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMiuriage]).Checked)
            {
                dje.ChkMiuriage = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMiseikyu]).Checked)
            {
                dje.ChkMiseikyu = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMinyukin]).Checked)
            {
                dje.ChkMinyukin = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked)
            {
                dje.ChkAll = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkReji]).Checked)
            {
                dje.ChkReji = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkGaisho]).Checked)
            {
                dje.ChkGaisho = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkTujo]).Checked)
            {
                dje.ChkTujo = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkHenpin]).Checked)
            {
                dje.ChkHenpin = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMihachu]).Checked)
            {
                dje.ChkMihachu = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkNokiKaito]).Checked)
            {
                dje.ChkNokiKaito = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMinyuka]).Checked)
            {
                dje.ChkMinyuka = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkMisiire]).Checked)
            {
                dje.ChkMisiire = 1;
            }
            if (((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked)
            {
                dje.ChkHachuAll = 1;
            }

            mse = new M_SKU_Entity
            {
                SKUName = detailControls[(int)EIndex.SKUName].Text,
                SKUCD = detailControls[(int)EIndex.SKUCD].Text,//カンマ区切り
                JanCD = detailControls[(int)EIndex.JanCD].Text,//カンマ区切り
                MakerItem = detailControls[(int)EIndex.KanaName].Text,     //カンマ区切り
            };

            return dje;
        }
        private void ExecDetail(int row)
        {
            //受注入力を起動します
            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + Juchuu;
            if (System.IO.File.Exists(filePath))
            {
                string juchuNo =mTennic.Equals(1) ? GvDetail.Rows[row].Cells["colJuchuuProcessNO"].Value.ToString() : GvDetail.Rows[row].Cells["colJuchuuNO"].Value.ToString();
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + juchuNo;
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }

        }
        protected override void ExecSec()
        {
            try
            {
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + Juchuu;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //進捗状況
            if (!((CheckBox)detailControls[(int)EIndex.ChkMihikiate]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkMiuriage]).Checked
                 && !((CheckBox)detailControls[(int)EIndex.ChkMiseikyu]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkMinyukin]).Checked
                  && !((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkMihikiate].Focus();
                return;
            }
            //受注場所
            if (!((CheckBox)detailControls[(int)EIndex.ChkReji]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkGaisho]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkReji].Focus();
                return;
            }
            //受注種別
            if (!((CheckBox)detailControls[(int)EIndex.ChkTujo]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkHenpin]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkTujo].Focus();
                return;
            }
            //発注状況
            if (!((CheckBox)detailControls[(int)EIndex.ChkMinyuka]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkMihachu]).Checked
                 && !((CheckBox)detailControls[(int)EIndex.ChkNokiKaito]).Checked && !((CheckBox)detailControls[(int)EIndex.ChkMisiire]).Checked
                  && !((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked)
            {
                bbl.ShowMessage("E111");
                detailControls[(int)EIndex.ChkMihachu].Focus();
                return;
            }

            dje = GetSearchInfo();
            DataTable dt = ssbl.D_Juchu_SelectAllForShoukai(dje, mse, InOperatorCD, InPcID);
            GvDetail.DataSource = null;
            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                //SetGrid();
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

            if (GvDetail.Rows.Count > 0)
            {
                string filePath = "";
                if (!ShowSaveFileDialog(InProgramNM, out filePath, 1))
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
            detailControls = new Control[] { ckM_CheckBox7,ckM_CheckBox5,ckM_CheckBox6,ckM_CheckBox1,ckM_CheckBox2
                ,ckM_CheckBox3,ckM_CheckBox14
                ,ckM_CheckBox8,ckM_CheckBox9,ckM_CheckBox10,ckM_CheckBox11
                ,ckM_CheckBox4,ckM_CheckBox12,ckM_CheckBox13
                , CboStoreCD
                 ,ScCustomer.TxtCode, ckM_TextBox4,ckM_TextBox8, ckM_TextBox3,ckM_TextBox15
                 ,ScVendor.TxtCode,ScStaff.TxtCode
                  ,ckM_TextBox1, ckM_TextBox2, ckM_TextBox10, ckM_TextBox9
                 ,ckM_TextBox14, ckM_TextBox13,ckM_TextBox12, ckM_TextBox11,ScJuchuuNO.TxtCode,ScJuchuuNOTo.TxtCode
                 , ckM_TextBox6, ckM_TextBox7, ckM_TextBox5
                 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
            
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
            bool ret;

            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.SalesDateFrom:
                case (int)EIndex.SalesDateTo:
                case (int)EIndex.BillingDateFrom:
                case (int)EIndex.BillingDateTo:
                case (int)EIndex.CollectDateFrom:
                case (int)EIndex.CollectDateTo:
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
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.SalesDateTo || index== (int)EIndex.BillingDateTo || index== (int)EIndex.CollectDateTo)
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
                case (int)EIndex.JuchuNoTo:
                    if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E106");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.StoreCD:
                    //選択なくてもよい
                    if (CboStoreCD.SelectedIndex <= 0)
                    {
                        //bbl.ShowMessage("E102");
                        //CboStoreCD.Focus();
                        //return false;
                        return true;
                    }
                    else
                    {
                        //店舗権限のチェック
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            bbl.ShowMessage("E141");
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
                        ChangeDate = bbl.GetDate()
                    };
                    Customer_BL sbl = new Customer_BL();
                     ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        ScCustomer.LabelText = mce.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScCustomer.LabelText = "";
                        return false;
                    }

                    break;

                case (int)EIndex.VendorCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScVendor.LabelText = "";
                        return true;
                    }
                    
                    //[M_VendorCD_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL vbl = new Vendor_BL();
                     ret = vbl.M_Vendor_SelectTop1(mve);
                    if (ret)
                    {
                        ScVendor.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScVendor.LabelText = "";
                        return false;
                    }

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
                        ChangeDate = bbl.GetDate() 
                    };
                    Staff_BL bl = new Staff_BL();
                     ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
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

            //foreach (Control ctl in detailLabels)
            //{
            //    ((CKM_SearchControl)ctl).LabelText = "";
            //}


            //初期値セット
            string ymd = ssbl.GetDate();

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

            ((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkTujo]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkHenpin]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkReji]).Checked = true;
            ((CheckBox)detailControls[(int)EIndex.ChkGaisho]).Checked = true;

            GvDetail.DataSource = null;
            //SetGrid();
            GvDetail.Enabled = false;
            Btn_F10.Enabled = false;
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScCustomer.LabelText = "";
        }

        //private void SetGrid()
        //{

            ////GridViewのプロパティ設定
            ////"Column1"列のヘッダーのテキストの配置を上下左右とも中央にする
            //GvDetail.Columns["colJuchuuDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["coIOrderDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["colArrivalDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["colNyukaBi"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["colSaleDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["colSikyuYmd"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //GvDetail.Columns["colNyukinYmd"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //GvDetail.Columns["colJuchuuDate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["coIOrderDate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["colArrivalDate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["colNyukaBi"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["colSaleDate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["colSikyuYmd"].SortMode = DataGridViewColumnSortMode.NotSortable;
            //GvDetail.Columns["colNyukinYmd"].SortMode = DataGridViewColumnSortMode.NotSortable;
        //}
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
                    if (bbl.ShowMessage("Q210") != DialogResult.Yes)
                        return;

                    ExecSec();
                    break;
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
                    using (Search_Product frmProduct = new Search_Product(ymd))
                    {
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            int index = Array.IndexOf(detailControls, setCtl);

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
                    case (int)EIndex.CustomerCD:
                    case (int)EIndex.StaffCD:
                    case (int)EIndex.JuchuNoFrom:
                    case (int)EIndex.JuchuNoTo:
                    case (int)EIndex.SKUCD:
                        case (int)EIndex.JanCD:
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

                if (((Control)sender).Name.Equals(btnSearchSKUCD.Name))
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

        private void BtnChoseAll_Click(object sender, EventArgs e)
        {
            try
            {
                CheckedChange(true);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnOff_Click(object sender, EventArgs e)
        {
            try
            {
                CheckedChange(false);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void ChkSinchokuAll_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked)
                for (int i = (int)EIndex.ChkMihikiate; i <= (int)EIndex.ChkMinyukin; i++)
                {
                    ((CheckBox)detailControls[i]).Checked = false;
                }
        }
        private void ChkSinchoku_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked = false;
            }
        }
        private void ChkHachuAll_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked)
                for (int i = (int)EIndex.ChkMihachu; i <= (int)EIndex.ChkMisiire; i++)
                {
                    ((CheckBox)detailControls[i]).Checked = false;
                }
        }
        private void ChkHachu_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked = false;
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
        private void GvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    ExecDetail(e.RowIndex);
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

        private void CheckedChange(bool kbn)
        {
            for(int i=(int)EIndex.ChkMihikiate; i<= (int)EIndex.ChkHachuAll; i++)
            {
                if (i != (int)EIndex.ChkAll && i != (int)EIndex.ChkHachuAll)
                    ((CheckBox)detailControls[i]).Checked = kbn;
                else
                {
                    ((CheckBox)detailControls[(int)EIndex.ChkAll]).Checked = false;
                    ((CheckBox)detailControls[(int)EIndex.ChkHachuAll]).Checked = false;
                }
            }
        }

    }
}








