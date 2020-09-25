﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using CKM_Controls;
using Entity;
using BL;
using Search;
using System.Data.OleDb;
//using TempoJuchuuNyuuryoku;

namespace TenzikaiJuchuuTourou
{
    internal partial class TenzikaiJuchuuTourou : FrmMainForm
    {
        private const string ProID = "TenzikaiJuchuuTourou";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        private string C_dt = "";
        private int mTennic;
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        private string KibouBi1 = "";
        private string KibouBi2 = "";
        CKM_SearchControl mOldCustomerCD;
        private TempoJuchuuNyuuryoku_BL mibl;
        TenjikaiJuuChuu_BL tkb;
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private enum Eindex : int
        {
            SCTenjiKai,
            SCShiiresaki,
            Nendo,
            ShiSon,
            JuuChuuBi,
            Uriageyotei,
            ShuukaSouko,
            SCTentouStaffu,
            SCKokyakuu,
            KJuShou1,
            btnkokyaku,
            KJuShou2,
            pnlKokyakuu,
            KDenwa1,
            KDenwa2,
            KDenwa3,
            SCHaiSoSaki,
            HJuShou1,
            btnHaisou,
            HJuShou2,
            pnlHaisou,
            HDenwa1,
            HDenwa2,
            HDenwa3,
            YoteiKinShuu,
            Count
        }
        FrmAddress addInfo;
        ClsGridTenjikai mGrid = new ClsGridTenjikai();
        private int m_EnableCnt = 0;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxJyuchuGyoNo;
        private Tenjikai_Entity tje;
        public TenzikaiJuchuuTourou()
        {
            InitializeComponent();
            C_dt = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private enum EsearchKbn : short
        {
            Null,
            Product,
            Vendor
        }
        private void TenzikaiJuchuuTourou_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();
                addInfo = new FrmAddress();
                // 明細部初期化
                this.S_SetInit_Grid();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();
                BindCombo();
                //コンボボックス初期化
                string ymd = bbl.GetDate();
                // tubl = new TempoUriageNyuuryoku_BL();
                //  CboStoreCD.Bind(ymd);
               // base.StartProgram();

                // mTennic = bbl.GetTennic();
                //検索用のパラメータ設定
                //    ScCustomerCD.Value1 = "1";
                //   ScCustomerCD.Value2 = "";

                Btn_F11.Text = "";
                sc_shiiresaki.TxtCode.Focus();

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                //if (ret)
                //{
                //    CboStoreCD.SelectedValue = mse.StoreCD;
                //    ScStaff.LabelText = mse.StaffName;
                //}

                detailControls[(int)Eindex.JuuChuuBi].Text = ymd;

               
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                    ChangeOperationMode(EOperationMode.SHOW);
                    //  keyControls[(int)Eindex.JuchuuNO].Text = juchuNO;
                    //   CheckKey((int)EIndex.JuchuuNO, true);
                }
                Btn_F7.Text =  "行追加(F7)";
                Btn_F8.Text = "行削除(F8)";
                Btn_F10.Text = "行複写(F10)";
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            //DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            ////配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:

                    //ScStaff.TxtCode.Text = InOperatorCD;
                    //ScStaff.LabelText = InOperatorName;
                    //CboStoreCD.SelectedValue = StoreCD;

                    detailControls[1].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    detailControls[0].Focus();
                    break;

            }

        }
        private void InitialControlArray()
        {

            detailControls = new Control[] { sc_Tenji.TxtCode, sc_shiiresaki.TxtCode, cbo_nendo,cbo_season,txt_JuchuuBi, txt_UriageYoteiBi, cbo_Shuuka,sc_TentouStaff.TxtCode,
                sc_kokyakuu.TxtCode, txt_kokyaJuusho1, btn_Customer, txt_KokyaJuusho2,  pnl_kokyakuu, txt_KDenwa1, txt_KDenwa2, txt_KDenwa3,
                sc_haisosaki.TxtCode, txt_HaisoJuusho1, btn_Shipping,  txt_HaisoJuusho2, pnl_haisou, txt_HDenwa1, txt_HDenwa2,txt_HDenwa3,
                cbo_yotei };
            detailLabels = new Control[] { };
            searchButtons = new Control[] { sc_Tenji.BtnSearch, sc_shiiresaki.BtnSearch, sc_TentouStaff.BtnSearch, sc_kokyakuu.BtnSearch, sc_haisosaki.BtnSearch };
            detailControls[(int)(Eindex.KJuShou1)].Enabled = false;
            detailControls[(int)(Eindex.KJuShou2)].Enabled = false;
            detailControls[(int)(Eindex.HJuShou1)].Enabled = false;
            detailControls[(int)(Eindex.HJuShou2)].Enabled = false;

            (detailControls[(int)(Eindex.JuuChuuBi)] as CKM_TextBox).Require(false);

            foreach (var c in detailControls)
            {
                c.KeyDown += C_KeyDown;
                c.Enter += C_Enter;
                if (c is CKM_ComboBox cb && cb.Name == "cbo_Shuuka")
                {
                    cb.SelectedIndexChanged += ShuukaSouko_SelectedIndexChanged;
                }
            }
            kr_1.KeyDown += Kr_2_KeyDown;
            kr_2.KeyDown += Kr_2_KeyDown;
            hr_3.KeyDown += Hr_3_KeyDown;
            hr_4.KeyDown += Hr_3_KeyDown;
            //  sc_Tenji.KeyDown += TenzikaiJuchuuTourou_KeyDown;
        }

        private void ShuukaSouko_SelectedIndexChanged(object sender, EventArgs e)
        {
            //for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            //{
            //CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShuukaSou, W_CtlRow].CellCtl;
            //CKM_Controls.CKM_ComboBox sctl; 
            //    if (sctl.DataSource != null)
            //    {
            //        sctl.SelectedIndex = (sender as CKM_ComboBox).SelectedIndex;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrEmpty(mGrid.g_DArray[RW].SKUCD) )
                {
                    mGrid.g_DArray[RW].ShuukaSou = (sender as CKM_ComboBox).SelectedValue.ToString();
                }
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            //}

            //}
            //foreach (var c in mGrid.g_DArray )
            //{

            //}
        }

        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridTenjikai.gc_P_GYO <= ClsGridTenjikai.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridTenjikai.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridTenjikai.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridTenjikai.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridTenjikai.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray(); // g_mk_ctrl

            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
                {
                    if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
                    {
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        {
                            Search.CKM_SearchControl sctl = (Search.CKM_SearchControl)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.TxtCode.Tag = W_CtlRow.ToString();
                            sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            CKM_Controls.CKM_TextBox sctl = (CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Tag = W_CtlRow.ToString();
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.EnabledChanged += new EventHandler(GridControl_EnableChnaged);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.Tag = W_CtlRow.ToString();
                        }
                        //else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        //{
                        //    Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                        //    if (w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
                        //        btn.Click += new System.EventHandler(BTN_Site_Click);
                        //    else
                        //        btn.Click += new System.EventHandler(BTN_Zaiko_Click);
                        //}
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Tag = W_CtlRow.ToString();
                            check.Enter += new System.EventHandler(GridControl_Enter);
                            check.Leave += new System.EventHandler(GridControl_Leave);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            check.Click += new System.EventHandler(CHK_Print_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridTenjikai.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridTenjikai.ST_DArray_Grid[mGrid.g_MK_Max_Row];
            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridTenjikai.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridTenjikai.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridTenjikai.ColNO.GYONO; i <= (int)ClsGridTenjikai.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridTenjikai.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                        case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
                        case (int)ClsGridTenjikai.ColNO.HacchuTanka:
                        case (int)ClsGridTenjikai.ColNO.ZeinuJuchuu:
                        case (int)ClsGridTenjikai.ColNO.zeikomijuchuu:
                        case (int)ClsGridTenjikai.ColNO.ArariGaku:

                            //  mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;//ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Price; //Ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).IntegerPart = 8; // ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).DecimalPlace = 0;
                            break;

                        case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                        case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
                            break;
                        case (int)ClsGridTenjikai.ColNO.SCJAN:
                           // mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
                            break;
                    }
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridTenjikai.ColNO.ZeinuJuchuu:
                        case (int)ClsGridTenjikai.ColNO.zeikomijuchuu:
                        case (int)ClsGridTenjikai.ColNO.ArariGaku:
                        case (int)ClsGridTenjikai.ColNO.ZeiNu:
                        case (int)ClsGridTenjikai.ColNO.ZeinuTanku:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Back_Color =  CKM_TextBox.CKM_Color.DarkGrey ;
                            break;
                    }

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            // TabStopﾌﾟﾛﾊﾟﾃｨの初期状態をセット
            Set_GridTabStop(false);
            // 明細部クリア
            //S_Clear_Grid();   Scr_Clr処理で
        }
        private  void GridControl_EnableChnaged(object sender, EventArgs e)
        {
            if (!(sender as CKM_TextBox).Enabled)
            {
                (sender as CKM_TextBox).Back_Color = CKM_TextBox.CKM_Color.DarkGrey;
            }
        }
        private void CHK_Print_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                if (w_Col == (int)ClsGridTenjikai.ColNO.ChoukuSou)
                {
                    bool Check = mGrid.g_DArray[w_Row].ChoukuSou;

                    if (w_Row == 0 && Check)
                        mGrid.g_DArray[w_Row].ChoukuSou = false;

                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SCJAN))
                        mGrid.g_DArray[w_Row].ChoukuSou = false;
                }
                //else if (w_Col == (int)ClsGridTenjikai.ColNO.ChkTyokuso)
                //{
                //    //画面明細.直送checkbox＝onの場合
                //    bool Check = mGrid.g_DArray[w_Row].ChkTyokuso;
                //    if (Check)
                //    {
                //        mGrid.g_DArray[w_Row].ArrivePlanDate = "";
                //        mGrid.g_DArray[w_Row].KariHikiateNO = "";
                //    }

                //}

                //配列の内容を画面へセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
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
                Control sc = ((Control)sender).Parent;

                //検索ボタンClick時
                if (((Search.CKM_SearchControl)sc).Name.Contains("scjan"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                }
                //else if (((Search.CKM_SearchControl)sc).Name.Substring(0, 9).Equals("IMT_VENCD"))
                //{
                //    //仕入先検索
                //    kbn = EsearchKbn.Vendor;
                //}
                setCtl = previousCtrl;


                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            int w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            switch (kbn)
            {
                case EsearchKbn.Product:


                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)Eindex.JuuChuuBi].Text))
                    {
                        frmProduct.SKUCD = mGrid.g_DArray[w_Row].SKUCD;
                        //frmProduct.ITEM = mGrid.g_DArray[w_Row].item;
                        //frmProduct.MakerItem = mGrid.g_DArray[w_Row].SKUCD;
                        frmProduct.JANCD = mGrid.g_DArray[w_Row].SCJAN;
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].SCJAN = frmProduct.JANCD;
                         //   mGrid.g_DArray[w_Row].OldJanCD = frmProduct.JANCD;
                            mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
                           // mGrid.g_DArray[w_Row].AdminNO = frmProduct.AdminNO;

                            CheckGrid((int)ClsGridTenjikai.ColNO.SCJAN, w_Row, false, true);

                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;

                case EsearchKbn.Vendor:
                    //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].VendorCD))
                    //    CheckGrid((int)ClsGridJuchuu.ColNO.VendorCD, w_Row, false, true);

                    ////配列の内容を画面へセット
                    //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    break;
            }

        }
        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    //どの項目か判別
                    int CL = -1;
                    string ctlName = "";
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    else
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;

                    switch (ctlName)
                    {
                        case "scjan":
                            CL = (int)ClsGridTenjikai.ColNO.SCJAN;
                            break;
                        case "shouhin":

                            CL = (int)ClsGridTenjikai.ColNO.ShouName;
                            break;
                        case "shuukayotei":
                            CL = (int)ClsGridTenjikai.ColNO.ShuukaYo;
                            break;
                        case "choukusou":
                            CL = (int)ClsGridTenjikai.ColNO.ChoukuSou;
                            break;
                        case "shuukasouko":
                            CL = (int)ClsGridTenjikai.ColNO.ShuukaSou;
                            break;
                        case "hacchutanka":
                            CL = (int)ClsGridTenjikai.ColNO.HacchuTanka;
                            break;
                        case "nyuukayotei":
                            CL = (int)ClsGridTenjikai.ColNO.NyuuKayo;
                            break;
                        case "juchuusuu":
                            CL = (int)ClsGridTenjikai.ColNO.JuchuuSuu;
                            break;
                        //case "IMN_ORGAK":
                        //    CL = (int)ClsGridJuchuu.ColNO.OrderGaku;
                        //    break;
                        case "hanbaitanka":
                            CL = (int)ClsGridTenjikai.ColNO.HanbaiTanka;
                            break;
                        case "chk":
                            CL = (int)ClsGridTenjikai.ColNO.Chk;
                            break;
                            //if (w_Row == m_dataCnt - 1)
                            //if (w_Row == mGrid.g_MK_Max_Row - 1)
                            //    lastCell = true;
                            //break;

                    }

                    bool changeFlg = false;
                    //switch (CL)
                    //{
                        //case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                        //    if (!mGrid.g_DArray[w_Row].JuchuuSuu.Equals(w_ActCtl.Text))
                        //    {
                        //        changeFlg = true;
                        //    }
                        //    break;

                        //case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice: //販売単価
                        //    if (!mGrid.g_DArray[w_Row].JuchuuUnitPrice.Equals(w_ActCtl.Text))
                        //    {
                        //        changeFlg = true;
                        //    }
                        //    break;

                        //case (int)ClsGridJuchuu.ColNO.CostUnitPrice: //原価単価
                        //    if (!mGrid.g_DArray[w_Row].CostUnitPrice.Equals(w_ActCtl.Text))
                        //    {
                        //        changeFlg = true;
                        //    }
                        //    break;
                    //}

                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //手入力時金額を再計算
                    if (changeFlg)
                    {
                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuSuu);
                        string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;
                        decimal wTanka;

                        switch (CL)
                        {
                            case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                                //数量が整数値かどうかチェック
                                int intSu;
                                if (!int.TryParse(wSuu.ToString(), out intSu))
                                {
                                    bbl.ShowMessage("E102");
                                    return;
                                }

                                //入力された場合、金額再計算
                                if (wSuu != 0)
                                {
                                    ////Function_単価取得.
                                    //Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                                    //{
                                    //    AdminNo = mGrid.g_DArray[w_Row].AdminNO,
                                    //    ChangeDate = ymd,
                                    //    CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                                    //    StoreCD = CboStoreCD.SelectedIndex > 0 ? CboStoreCD.SelectedValue.ToString() : "",
                                    //    SaleKbn = "0",
                                    //    Suryo = wSuu.ToString()
                                    //};

                                    //bool ret = bbl.Fnc_UnitPrice(fue);
                                    //if (ret)
                                    //{
                                    //    //数量変更時も単価再計算
                                    //    switch (mTennic)
                                    //    {
                                    //        case 0:
                                    //            //販売単価=Function_単価取得.out税込単価		
                                    //            mGrid.g_DArray[w_Row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeikomiTanka));
                                    //            break;
                                    //        case 1:
                                    //            //販売単価=Function_単価取得.out税抜単価
                                    //            mGrid.g_DArray[w_Row].JuchuuUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.ZeinukiTanka));
                                    //            break;
                                    //    }

                                    //    //原価単価=Function_単価取得.out原価単価	
                                    //    mGrid.g_DArray[w_Row].CostUnitPrice = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka));
                                    //}
                                    //else
                                    //{
                                    //    //販売単価		
                                    //    mGrid.g_DArray[w_Row].JuchuuUnitPrice = "0";
                                    //    //原価単価
                                    //    mGrid.g_DArray[w_Row].CostUnitPrice = "0";
                                    //}

                                    //SetJuchuuGaku(w_Row, wSuu, ymd, fue.ZeinukiTanka);

                                    ////原価額←Function_単価取得.out原価単価×Form.Detail.見積数
                                    //mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(fue.GenkaTanka) * wSuu);
                                    //mGrid.g_DArray[w_Row].OrderGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].OrderUnitPrice) * wSuu);

                                   //.//CalcZei(w_Row);
                                }
                                break;

                            //case (int)ClsGridTenjikai.ColNO.JuchuuUnitPrice: //販売単価 
                            //    {
                            //        string tanka = mGrid.g_DArray[w_Row].JuchuuUnitPrice;
                            //        if (mTennic.Equals(0))
                            //            tanka = bbl.GetZeinukiKingaku(bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuUnitPrice), mGrid.g_DArray[w_Row].TaxRateFLG, ymd).ToString();

                            //        SetJuchuuGaku(w_Row, wSuu, ymd, tanka);

                            //        CalcZei(w_Row, true);
                            //    }
                            //    break;

                            //case (int)ClsGridJuchuu.ColNO.CostUnitPrice: //原価単価
                            //    //原価額=Form.Detail.原価単価×	Form.Detail.受注数
                            //    mGrid.g_DArray[w_Row].CostGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].CostUnitPrice) * bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuSuu));

                            //    //粗利額
                            //    mGrid.g_DArray[w_Row].ProfitGaku = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].JuchuuHontaiGaku) - bbl.Z_Set(mGrid.g_DArray[w_Row].CostGaku)); // 

                               // break;
                        }
                    }

                    //if (CL == -1)
                    //return;

                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            ((CKM_Controls.CKM_ComboBox)w_ActCtl).MoveNext = false;
                        }

                        //配列の内容を画面へセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                       // IMT_DMY_0.Focus();
                        //Focusセット処理
                        w_ActCtl.Focus();
                        return;
                    }

                    if (lastCell)
                    {
                        w_ActCtl.Focus();
                        return;
                    }

                    //あたかもTabキーが押されたかのようにする
                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    //this.ProcessTabKey(!e.Shift);
                    //行き先がなかったら移動しない
                    S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    //switch (CL)
                    //{
                    //    //case (int)ClsGridJuchuu.ColNO.NotPrintFLG:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkExpress:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkFuyo:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkTyokuso:
                    //        if (e.Shift)
                    //            S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                    //        else
                    //            S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);

                    //        break;
                    //}
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private bool GetTenji(int row)
        {
            string Jan = mGrid.g_DArray[row].SCJAN;
            tje = new Tenjikai_Entity()
            {
                Kokyaku = detailControls[(int)Eindex.SCKokyakuu].Text,
                JanCD = Jan,
                Nendo = detailControls[(int)Eindex.Nendo].Text,
                ShiZun = detailControls[(int)Eindex.ShiSon].Text,
                Shiiresaki = detailControls[(int)Eindex.SCShiiresaki].Text,
                ChangeDate = detailControls[(int)Eindex.JuuChuuBi].Text
            };
            // M_TeniKaiSelectbyJAN
            tkb = new TenjikaiJuuChuu_BL();
            var dt = tkb.M_TeniKaiSelectbyJAN(tje);
            if (dt.Rows.Count == 1)
            {
                var selectRow = dt.Rows[0];
                mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                mGrid.g_DArray[row].ShouName = selectRow["SKUName"].ToString();
                mGrid.g_DArray[row].Color = selectRow["ColorNO"].ToString();
                mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString() == "1" ? "〇" : "";
                mGrid.g_DArray[row].Size = selectRow["SizeNo"].ToString();
                mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                mGrid.g_DArray[row].AdminNo = selectRow["AdminNO"].ToString();
                mGrid.g_DArray[row].HacchuTanka = bbl.Z_SetStr(selectRow["ShiireTanka"].ToString());
                mGrid.g_DArray[row].HanbaiTanka = bbl.Z_SetStr(selectRow["SalePriceOutTax"].ToString());
                mGrid.g_DArray[row].TenI = selectRow["TaniCD"].ToString(); // Name
                mGrid.g_DArray[row].TeniName = selectRow["TaniName"].ToString(); 
                mGrid.g_DArray[row].TaxRateFlg = Convert.ToInt16(selectRow["TaxRateFlg"].ToString()).ToString();
        }
            else
                return false;
            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            bool ret = false;

            string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridTenjikai.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }
            }

            switch (col)
            {
                case (int)ClsGridTenjikai.ColNO.SCJAN:
                    //販売単価 複写元受注番号が入力されている場合は、以下のメッセージを表示後、その回答によって扱いを変える
                   

                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SCJAN))
                    {
                        //入力が無い場合(If there is no input)その明細で、JANCDを除く他の項目は全て入力不可とする
                        Grid_Gyo_Clr(row);
                        return true;
                    }
                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].SCJAN,
                        ChangeDate = ymd
                    };
                
                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;
                   
                    if (dt.Rows.Count == 0)
                    {
                        var val = GetTenji(row);
                        if (!val)
                        {
                            //Ｅ１０7
                            bbl.ShowMessage("E107");
                            return false;
                        }
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        selectRow = dt.Rows[0];
                    }
                    else
                    {
                        //JANCDでSKUCDが複数存在する場合（If there is more than one）
                        using (Select_SKU frmSKU = new Select_SKU())
                        {
                            frmSKU.parJANCD = dt.Rows[0]["JanCD"].ToString();
                            frmSKU.parChangeDate = ymd;
                            frmSKU.ShowDialog();

                            if (!frmSKU.flgCancel)
                            {
                                selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                            }
                        }

                    }

                    if (selectRow != null)
                    {
                        //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                        // mGrid.g_DArray[row].SCJAN = selectRow["JanCD"].ToString();
                        mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                        mGrid.g_DArray[row].ShouName = selectRow["SKUName"].ToString();
                        mGrid.g_DArray[row].Color = selectRow["ColorNO"].ToString();
                        mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString() == "1" ? "〇" : "";
                        mGrid.g_DArray[row].Size = selectRow["SizeNo"].ToString();
                        mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();

                        //AdminNo
                        mGrid.g_DArray[row].AdminNo = selectRow["AdminNO"].ToString();

                        //[M_JANOrderPrice]
                        M_JANOrderPrice_Entity mje = new M_JANOrderPrice_Entity
                        {

                            //①JAN発注単価マスタ（店舗指定なし）
                            AdminNO = mGrid.g_DArray[row].AdminNo,
                            VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                            StoreCD = "0000",
                            ChangeDate = !String.IsNullOrWhiteSpace(detailControls[(int)Eindex.JuuChuuBi].Text) ? detailControls[(int)Eindex.JuuChuuBi].Text : ymd,
                        };

                        JANOrderPrice_BL jbl = new JANOrderPrice_BL();
                        ret = jbl.M_JANOrderPrice_Select(mje);
                        if (ret)
                        {
                            mGrid.g_DArray[row].HacchuTanka = bbl.Z_SetStr(mje.PriceWithoutTax);
                        }
                        else
                        {
                            //[M_ItemOrderPrice]
                            M_ItemOrderPrice_Entity mje2 = new M_ItemOrderPrice_Entity
                            {

                                //③	ITEM発注単価マスター（店舗指定あり）	
                                MakerItem = selectRow["MakerItem"].ToString(),
                                VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                                ChangeDate = !String.IsNullOrWhiteSpace(detailControls[(int)Eindex.JuuChuuBi].Text) ? detailControls[(int)Eindex.JuuChuuBi].Text : ymd,
                                StoreCD = "0000"
                            };

                            ItemOrderPrice_BL ibl = new ItemOrderPrice_BL();
                            ret = ibl.M_ItemOrderPrice_Select(mje2);
                            if (ret)
                            {
                                mGrid.g_DArray[row].HacchuTanka = bbl.Z_SetStr(mje2.PriceWithoutTax);
                            }
                            else
                                mGrid.g_DArray[row].HacchuTanka = "0";

                        }
                        //Function_単価取得.
                        Fnc_UnitPrice_Entity fue = new Fnc_UnitPrice_Entity
                        {
                            AdminNo = mGrid.g_DArray[row].AdminNo,
                            ChangeDate = !String.IsNullOrWhiteSpace(detailControls[(int)Eindex.JuuChuuBi].Text) ? detailControls[(int)Eindex.JuuChuuBi].Text : ymd,
                            CustomerCD = detailControls[(int)Eindex.SCKokyakuu].Text,
                            StoreCD = "0000",
                            SaleKbn = "0",
                            Suryo = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu).ToString(),
                        };

                        ret = bbl.Fnc_UnitPrice(fue);
                        if (ret)
                        {
                            mGrid.g_DArray[row].HanbaiTanka = bbl.Z_SetStr(fue.ZeinukiTanka);
                        }
                        //Tani
                        mGrid.g_DArray[row].TenI = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TeniName = selectRow["TaniName"].ToString();
                        //TaxRate
                        mGrid.g_DArray[row].TaxRateFlg = Convert.ToInt16(selectRow["TaxRateFLG"].ToString()).ToString();
                    }
                    
                            Grid_NotFocus(col, row);

                        //}

                        break;

                case (int)ClsGridTenjikai.ColNO.ShouName:
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ShouName))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }

                    }
                    break;

                case (int)ClsGridTenjikai.ColNO.ShuukaSou:
                    if (mGrid.g_MK_State[col, row].Cell_Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ShuukaSou))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    M_Souko_Entity msoe = new M_Souko_Entity
                    {
                        SoukoCD = mGrid.g_DArray[row].ShuukaSou,
                        ChangeDate = ymd,
                        DeleteFlg = "0"
                    };

                    DataTable msdt = mibl.M_Souko_IsExists(msoe);
                    if (msdt.Rows.Count > 0)
                        if (!base.CheckAvailableStores(msdt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E145");
                            return false;
                        }

                    break;

                case (int)ClsGridTenjikai.ColNO.ChoukuSou:
                    bool Check = mGrid.g_DArray[row].ChoukuSou;
                    if (Check)
                    {
                     //
                    }
                    break;

                case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ShuukaYo))
                    {
                    }
                    else
                    {
                        mGrid.g_DArray[row].ShuukaYo = bbl.FormatDate(mGrid.g_DArray[row].ShuukaYo);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(mGrid.g_DArray[row].ShuukaYo))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(mGrid.g_DArray[row].ShuukaYo))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }
                    }
                    break;
                case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].NyuuKayo))
                    {
                    }
                    else
                    {
                        mGrid.g_DArray[row].NyuuKayo = bbl.FormatDate(mGrid.g_DArray[row].NyuuKayo);

                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(mGrid.g_DArray[row].NyuuKayo))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(mGrid.g_DArray[row].NyuuKayo))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }

                    }
                    break;

                case (int)ClsGridTenjikai.ColNO.HacchuTanka://発注単価
                                                             //入力無くても良い(It is not necessary to input)
                                                             //入力無い場合、0とする（When there is no input, it is set to 0）
                    decimal orderUnitPrice = bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka);
                    mGrid.g_DArray[row].HacchuTanka = bbl.Z_SetStr(orderUnitPrice);

                    //０の場合				メッセージ表示
                    if (orderUnitPrice.Equals(0))
                    {
                        if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                            return false;
                    }
                    //０で無いかつ原価単価＝０の場合場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                    var val1 = (bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu) - bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka)) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
                    mGrid.g_DArray[row].ArariGaku = bbl.Z_SetStr(val1);
                    break;

            }

            switch (col)
            {
                case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                    decimal JuchuuSuu = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
                    mGrid.g_DArray[row].JuchuuSuu = bbl.Z_SetStr(JuchuuSuu);

                    //０の場合				メッセージ表示
                    if (JuchuuSuu.Equals(0))
                    {
                        if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                            return false;
                    }
                    //各金額項目の再計算必要
                    //if (chkAll == false)
                    CalcKin(row,col);

                    break;

                case (int)ClsGridTenjikai.ColNO.SCJAN:
                case (int)ClsGridTenjikai.ColNO.HacchuTanka: // 発注単価
                case (int)ClsGridTenjikai.ColNO.HanbaiTanka: //販売単価

                    //各金額項目の再計算必要
                 //   if (chkAll == false)
                        CalcKin(row,col);

                    break;

            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        private void CalcKin(int row, int col, bool IsHanBai = false)
        {
           
            if (col == (int)ClsGridTenjikai.ColNO.HanbaiTanka)
            {
                IsHanBai = true;
            }
            var hanbai = bbl.Z_Set(mGrid.g_DArray[row].HanbaiTanka);
            var juchuu = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
            mGrid.g_DArray[row].ZeinuJuchuu = bbl.Z_SetStr(hanbai * juchuu); // excTax 1
            var taxflg = bbl.Z_SetStr(mGrid.g_DArray[row].TaxRateFlg);
            tkb = new TenjikaiJuuChuu_BL();
            var res = tkb.GetTaxRate(taxflg, detailControls[(int)Eindex.JuuChuuBi].Text.Replace("/", "-"));
            if (taxflg == "0")     // incTax 2
            {
                //if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaxRate))
                //    zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                mGrid.g_DArray[row].zeikomijuchuu = mGrid.g_DArray[row].ZeinuJuchuu;
            }
            else // 1 or 2 
            {
                mGrid.g_DArray[row].zeikomijuchuu = bbl.Z_SetStr(GetResultWithHasuKbn(Convert.ToInt32(res.Rows[0]["KBN"].ToString()), Convert.ToDecimal(mGrid.g_DArray[row].ZeinuJuchuu) * (bbl.Z_Set(res.Rows[0]["TaxRate"].ToString()) + 100) / 100).ToString());
            }
            if (!IsHanBai)  // inside hanbaitanka
            {
                if (taxflg == "0") //  Zeinu TaxNotation 3
                {
                    mGrid.g_DArray[row].ZeiNu = "非税";
                }
                else
                {
                    mGrid.g_DArray[row].ZeiNu = "税抜";
                }



                if (taxflg == "0") // taxRates 4 
                {
                    mGrid.g_DArray[row].ZeinuTanku = "0%";
                }
                else
                {
                    mGrid.g_DArray[row].ZeinuTanku = bbl.Z_SetStr(res.Rows[0]["TaxRate"].ToString()) + "%";
                }
            }
            else  // arariGaku 3
            {
                var val1 = (bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu) - bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka)) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
                mGrid.g_DArray[row].ArariGaku = bbl.Z_SetStr(val1);
            }
            //same with hanbai Tanka
            mGrid.g_DArray[row].Tsuujou = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) -bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu));  // normalTax 5
            mGrid.g_DArray[row].Keigen = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) - bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu)); // ReduceTax 6


        }
        private void Grid_Gyo_Clr(int RW)  // 明細部１行クリア
        {
            string w_Gyo;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            w_Gyo = mGrid.g_DArray[RW].GYONO;

            // 一行クリア
            Array.Clear(mGrid.g_DArray, RW, 1);

            mGrid.g_DArray[RW].GYONO = w_Gyo;

            //ZEI_SUB(); // 消費税計算
            //Kin_Kei(); // 再計算

            // JanCD列以外入力不可 (JanCDを入力した時点で他の列が入力可になるため)
            Grid_NotFocus((int)ClsGridTenjikai.ColNO.SCJAN, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }
        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                // 背景色
                w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色
                w_ActCtl.BackColor = ClsGridTenjikai.BKColor;

                //明細が出荷済の受注明細、または、売上済の受注明細である場合、削除できない（F7ボタンを使えないようにする）
                //出荷済・売上済の場合は行削除不可
                //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ShuukaSou))
                //{
                //    if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
                //        Btn_F8.Text = "";
                //    else
                //        Btn_F8.Text = "行削除(F8)";
                //}
                //else
                //    Btn_F8.Text = "行削除(F8)";

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridMitsumori.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            SetFuncKeyAll(this, "111111111111");

            // 検索ﾎﾞﾀﾝ使用不可.解除
            if (W_Del == true)
                W_Ret = false;
            else
                switch (pCol)
                {
                    default:
                        {
                            W_Ret = false;
                            break;
                        }
                }
            SetFuncKey(this, 8, W_Ret);
        }
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1>5行目
            try
            {
                for (int d = 0; d < 5; d++)
                {
                    int id = d + 1;
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.GYONO, d].CellCtl = this.Controls.Find("lbl_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.SCJAN, d].CellCtl = this.Controls.Find("scjan_" + id, true)[0];
                    //mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.AdminNo, d].CellCtl = new Control(); // Adminno hidden
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.SKUCD, d].CellCtl = this.Controls.Find("sku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShouName, d].CellCtl = this.Controls.Find("shouhin_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.Color, d].CellCtl = this.Controls.Find("color_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ColorName, d].CellCtl = this.Controls.Find("colorname_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.Size, d].CellCtl = this.Controls.Find("size_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.SizeName, d].CellCtl = this.Controls.Find("sizename_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShuukaYo, d].CellCtl = this.Controls.Find("shuukayotei_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ChoukuSou, d].CellCtl = this.Controls.Find("choukusou_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShuukaSou, d].CellCtl = this.Controls.Find("shuukasouko_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.Empty, d].CellCtl = this.Controls.Find("empty_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.HacchuTanka, d].CellCtl = this.Controls.Find("hacchutanka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.NyuuKayo, d].CellCtl = this.Controls.Find("nyuukayotei_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.JuchuuSuu, d].CellCtl = this.Controls.Find("juchuusuu_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TenI, d].CellCtl = this.Controls.Find("teni_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.HanbaiTanka, d].CellCtl = this.Controls.Find("hanbaitanka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ZeinuJuchuu, d].CellCtl = this.Controls.Find("zeinuJuchuugaka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.zeikomijuchuu, d].CellCtl = this.Controls.Find("zeikomijuchuugaku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ArariGaku, d].CellCtl = this.Controls.Find("ararigaku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ZeiNu, d].CellCtl = this.Controls.Find("zeinu_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ZeinuTanku, d].CellCtl = this.Controls.Find("zeinutanku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.Chk, d].CellCtl = this.Controls.Find("chk_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShanaiBi, d].CellCtl = this.Controls.Find("shanaibikou_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShagaiBi, d].CellCtl = this.Controls.Find("shagaibikou_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.KobeTsu, d].CellCtl = this.Controls.Find("kobetsuhanbai_" + id, true)[0];
                    //mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TorokuFlg, d].CellCtl = new Control();  //torokuflg  hidden
                   // mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TaxRateFlg, d].CellCtl = new Control(); //taxrateflg  hidden
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message + ex.StackTrace;

            }

        }
        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                    //mGrid.g_DArray[w_Row].Rank1UnitPrice = "0";
                }
            }
            else  // set Data from db
            {
                int c = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    mGrid.g_DArray[c].SCJAN = dr["_1JanCD"].ToString();
                    mGrid.g_DArray[c].SKUCD = dr["_2SKUCD"].ToString();
                    mGrid.g_DArray[c].ShouName = dr["_3SKUName"].ToString();
                    mGrid.g_DArray[c].Color = dr["_4ColorNo"].ToString();
                    mGrid.g_DArray[c].ColorName = dr["_5ColorName"].ToString();
                    mGrid.g_DArray[c].Size = dr["_6SizeNo"].ToString();
                    mGrid.g_DArray[c].SizeName = dr["_7SizeName"].ToString();
                    mGrid.g_DArray[c].ShuukaYo = dr["_8ShuukaYoteiBi"].ToString();
                    mGrid.g_DArray[c].ShuukaSou = dr["_9SoukoName"].ToString();
                   // mGrid.g_DArray[c].Empty = dr["_3SKUName"].ToString();

                    mGrid.g_DArray[c].HacchuTanka = dr["_10SiireTanka"].ToString();//
                    mGrid.g_DArray[c].NyuuKayo = dr["_11NyuukaYoteiHyou"].ToString();//
                    mGrid.g_DArray[c].JuchuuSuu = dr["_12SoukunoSu"].ToString();//
                    mGrid.g_DArray[c].TenI = dr["_13TaniCD"].ToString();//
                    mGrid.g_DArray[c].HanbaiTanka = dr["_14SalePriceOutTax"].ToString();//

                    mGrid.g_DArray[c].ZeinuJuchuu = dr["_15OrderExcTax"].ToString();//
                    mGrid.g_DArray[c].zeikomijuchuu = dr["_16OrderIncTax"].ToString();//
                    mGrid.g_DArray[c].ArariGaku = dr["_17TotalProfit"].ToString();//
                    mGrid.g_DArray[c].ZeiNu = dr["_18Taxnotation"].ToString();//
                    mGrid.g_DArray[c].ZeinuTanku = dr["_19TaxRate"].ToString();//

                    // mGrid.g_DArray[c].Chk = dr["_3SKUName"].ToString();   
                    mGrid.g_DArray[c].ShanaiBi =  dr["_20ExternaRemarks"].ToString();//
                    mGrid.g_DArray[c].ShagaiBi = dr["_21InternalRemarks"].ToString();//
                    mGrid.g_DArray[c].KobeTsu = dr["_22kobetsuHanbai"].ToString();//

                    mGrid.g_DArray[c].TorokuFlg = dr["_23TorokuFlag"].ToString();//
                    mGrid.g_DArray[c].TaxRateFlg = dr["_25TaxRateFlg"].ToString();//
                    c++;
                }
            }
        }
        private void Set_GridTabStop(bool pTabStop)
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;

            try
            {
                // 画面上にEnable=Trueのコントロールがひとつしかない時はTab,Shift+Tab,↑,↓押下時にKeyExitが発生しないため、
                // 明細がスクロールしなくなる。回避策として、そのパターンが起こりうる時のみ、IMT_DMY_0.TabStopをTrueにする。
                if (OperationMode == EOperationMode.UPDATE)
                    scjan_1.TabStop = pTabStop;
                else
                    scjan_1.TabStop = false;

                // 状態が変わらない時は処理を抜ける
                if (mGrid.g_GridTabStop == pTabStop)
                    return;

                // TabStopの状態を退避
                mGrid.g_GridTabStop = pTabStop;

                // TabStopﾌﾟﾛﾊﾟﾃｨをセット
                for (w_CtlRow = 0; w_CtlRow <= mGrid.g_MK_Ctl_Row - 1; w_CtlRow++)
                {
                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    for (w_Col = 0; w_Col <= mGrid.g_MK_Ctl_Col - 1; w_Col++)
                        mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(w_Col, w_Row);
                }
            }

            catch
            {
                // 例外が発生した時は抜ける
                return;
            }
        }


        private void TenzikaiJuchuuTourou_KeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveControl is CKM_SearchControl cs && cs.Name == "sc_Tenji")
            {
                if (e.KeyCode == Keys.Tab)
                {
                    detailControls[(int)(Eindex.SCShiiresaki)].Focus();
                }
            }
        }

        private void Hr_3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    MoveNextControl(e);
                    //kr_1.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void Kr_2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    MoveNextControl(e);
                    //kr_1.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }


        protected void C_Enter(object sender, EventArgs e)
        {
            if ((sender as Control) is Panel p)
            {

                if (p.Name == "pnl_kokyakuu")
                {
                    kr_1.Focus();
                }
                else if (p.Name == "pnl_haisou")
                {
                    hr_3.Focus();

                }
            }
            if (ActiveControl is  CKM_SearchControl cs && cs.Name == "sc_Tenji")
            {
                previousCtrl = this.ActiveControl;
            }
            else
            {
                previousCtrl = null;
            }
        }

        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            // Processing when the Enter key is pressed
            // Check if the Return key is pressed
            // Alt or Ctrl key is pressed, do the original operation
            try
            {
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if (index == (int)Eindex.KDenwa3 || index == (int)Eindex.HDenwa3 || index == (int)(Eindex.YoteiKinShuu))
                        //明細の先頭項目へ
                        {
                            MoveNextControl(e);
                            //   mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridJuchuu.ColNO.JanCD);
                        }
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                        }
                    }
                    else
                    {
                        //((Control)sender).Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
            //  throw new NotImplementedException();
        }
        private bool CheckDetail(int index, bool set = true)
        {

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }
            TenjikaiJuuChuu_BL tbl = new TenjikaiJuuChuu_BL();
            switch (index)
            {
                case (int)Eindex.Nendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.ShiSon:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.JuuChuuBi:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        detailControls[index].Focus();
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.Uriageyotei:
                    // tbl = new TenjikaiJuuChuu_BL();
                    // TenjikaiJuuChuu_BL tbl = new TenjikaiJuuChuu_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        break;
                    }
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        detailControls[index].Focus();
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.ShuukaSouko:

                    tbl = new TenjikaiJuuChuu_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    var souko = tbl.ShuukaSouko((detailControls[index] as CKM_ComboBox).SelectedValue.ToString(), bbl.GetDate());

                    if (souko.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E145");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCTentouStaffu:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;
                case (int)Eindex.SCKokyakuu:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(0);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;


                    break;
                case (int)Eindex.SCHaiSoSaki:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(1);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;
                    break;
                case (int)Eindex.SCShiiresaki:    // ShiireSaki
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        VendorFlg = "1",
                        VendorCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();
                    var resul = vbl.M_Vendor_Select_Tenji(mve);
                    if (!resul)
                    {
                        bbl.ShowMessage("E101");
                        ClearCustomerInfo(2);
                        return false;
                    }
                    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mve.VendorName;
                    if (!CheckDependsOnDate(index))
                        return false;

                    break;
                    


                case (int)Eindex.KJuShou1:
                case (int)Eindex.HJuShou1:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                        //顧客名2に入力無い場合、先頭20Byteを顧客名2へセット   delete 4/24
                        if (string.IsNullOrWhiteSpace(detailControls[index + 2].Text))
                        {
                            detailControls[index + 2].Text = bbl.LeftB(detailControls[index].Text, 20);
                        }
                    }
                    break;

                case (int)Eindex.KJuShou2:
                case (int)Eindex.HJuShou2:
                    //入力可能の場合 入力必須(Entry required)
                    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;



                case (int)Eindex.YoteiKinShuu:
                    //選択必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                    

            }

            return true;
        }
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)

        {
            string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {

                case (int)Eindex.SCTentouStaffu:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Staff_BL bl = new Staff_BL();
                    bool staff = bl.M_Staff_Select(mse);
                    if (staff)
                    {
                        (detailControls[(int)Eindex.SCTentouStaffu].Parent as CKM_SearchControl).LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        (detailControls[(int)Eindex.SCTentouStaffu].Parent as CKM_SearchControl).LabelText = "";
                        detailControls[(int)Eindex.SCTentouStaffu].Text = "";
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)Eindex.SCShiiresaki:
                    //[Shiiresaki]

                    break;
                case (int)Eindex.SCKokyakuu:
                case (int)Eindex.SCHaiSoSaki:
                    short kbn = 0;
                    if (index.Equals((int)Eindex.SCHaiSoSaki))
                        kbn = 1;

                    //[M_Customer_Select]
                    Entity.M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    BL.Customer_BL sbl = new Customer_BL();
                    //ret = sbl.M_Customer_Select(mce, 1);
                    bool ret = sbl.M_Customer_Select(mce, 1);

                    if (ret)
                    {
                        if (mce.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo(kbn);
                            detailControls[index].Focus();
                            return false;
                        }
                        if (kbn.Equals(0))
                        {                                //住所情報セット
                            addInfo.ade.VariousFLG = mce.VariousFLG;
                            addInfo.ade.ZipCD1 = mce.ZipCD1;
                            addInfo.ade.ZipCD2 = mce.ZipCD2;
                            addInfo.ade.Address1 = mce.Address1;
                            addInfo.ade.Address2 = mce.Address2;
                            addInfo.ade.Tel11 = mce.Tel11;
                            addInfo.ade.Tel12 = mce.Tel12;
                            addInfo.ade.Tel13 = mce.Tel13;
                            detailControls[(int)(Eindex.KDenwa1)].Text = mce.Tel11;
                            detailControls[(int)(Eindex.KDenwa2)].Text = mce.Tel12;
                            detailControls[(int)(Eindex.KDenwa3)].Text = mce.Tel13;
                            detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
                            // detailControls[(int)(Eindex.KJuShou2)].Text = ;
                            //radio
                            if (mce.AliasKBN == "1")
                            {
                                kr_1.Checked = true;
                                kr_2.Checked = false;
                            }
                            else
                            {
                                kr_1.Checked = false;
                                kr_2.Checked = true;
                            }
                            if (mce.VariousFLG == "1")
                            {
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[index + 1].Enabled = true;
                                    detailControls[index + 3].Enabled = true;
                                }
                                detailControls[index + 3].Text = "";

                            }
                            else
                            {
                                detailControls[index + 1].Enabled = false;
                                detailControls[index + 3].Text = "";
                                detailControls[index + 3].Enabled = false;
                            }

                            if (string.IsNullOrWhiteSpace(sc_haisosaki.TxtCode.Text))
                            {
                                detailControls[(int)(Eindex.SCHaiSoSaki)].Text = detailControls[(int)Eindex.SCKokyakuu].Text;
                                CheckDetail((int)Eindex.SCHaiSoSaki);
                                detailControls[(int)Eindex.HJuShou1].Text = detailControls[(int)Eindex.KJuShou1].Text;
                                hr_3.Checked = kr_1.Checked;
                                hr_4.Checked = kr_2.Checked;
                                detailControls[(int)Eindex.HDenwa1].Text = detailControls[(int)Eindex.KDenwa1].Text;
                                detailControls[(int)Eindex.HDenwa2].Text = detailControls[(int)Eindex.KDenwa2].Text;
                                detailControls[(int)Eindex.HDenwa3].Text = detailControls[(int)Eindex.KDenwa3].Text;
                                addInfo.adeD.ZipCD1 = addInfo.ade.ZipCD1;
                                addInfo.adeD.ZipCD2 = addInfo.ade.ZipCD2;
                                addInfo.adeD.Address1 = addInfo.ade.Address1;
                                addInfo.adeD.Address2 = addInfo.ade.Address2;
                            }
                        }

                        else
                        {
                            addInfo.adeD.VariousFLG = mce.VariousFLG;
                            addInfo.adeD.ZipCD1 = mce.ZipCD1;
                            addInfo.adeD.ZipCD2 = mce.ZipCD2;
                            addInfo.adeD.Address1 = mce.Address1;
                            addInfo.adeD.Address2 = mce.Address2;
                            addInfo.adeD.Tel11 = mce.Tel11;
                            addInfo.adeD.Tel12 = mce.Tel12;
                            addInfo.adeD.Tel13 = mce.Tel13;
                            detailControls[(int)Eindex.HDenwa1].Text = mce.Tel11;
                            detailControls[(int)Eindex.HDenwa2].Text = mce.Tel12;
                            detailControls[(int)Eindex.HDenwa3].Text = mce.Tel13;

                            if (mce.VariousFLG == "1")
                            {
                                detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                                {
                                    detailControls[index + 1].Enabled = true;
                                    detailControls[index + 3].Enabled = true;
                                }
                                detailControls[index + 3].Text = "";
                            }
                            else
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                detailControls[index + 1].Enabled = false;
                                detailControls[index + 3].Text = "";
                                detailControls[index + 3].Enabled = false;
                            }
                            //敬称
                            if (mce.AliasKBN == "1")
                            {
                                hr_3.Checked = true;
                                hr_4.Checked = false;
                            }
                            else
                            {
                                hr_3.Checked = false;
                                hr_4.Checked = true;
                            }
                            //    }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(kbn);
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
            }
            return true;

        }
        private void ClearCustomerInfo(short kbn)
        {
            addInfo.ClearAddressInfo(kbn);  //Clear to Address Screen that still idle

            if (kbn.Equals(0))   // Customer
            {

                detailControls[(int)(Eindex.SCKokyakuu)].Text = "";
                detailControls[(int)(Eindex.KJuShou1)].Text = "";
                detailControls[(int)(Eindex.KJuShou2)].Text = "";
                detailControls[(int)(Eindex.KDenwa1)].Text = "";
                detailControls[(int)(Eindex.KDenwa2)].Text = "";
                detailControls[(int)(Eindex.KDenwa3)].Text = "";
                kr_1.Checked = true;
                kr_2.Checked = false;
                
            }
            else if (kbn.Equals(1))  // delivery / Shipping
            {
                detailControls[(int)(Eindex.SCHaiSoSaki)].Text = "";
                detailControls[(int)(Eindex.HJuShou1)].Text = "";
                detailControls[(int)(Eindex.HJuShou2)].Text = "";
                detailControls[(int)(Eindex.HDenwa1)].Text = "";
                detailControls[(int)(Eindex.HDenwa2)].Text = "";
                detailControls[(int)(Eindex.HDenwa3)].Text = "";
                hr_3.Checked = true;
                hr_4.Checked = false;

            }
            else if (kbn.Equals(2))  // Shiiresaki
            {
                detailControls[(int)(Eindex.SCShiiresaki)].Text = "";
                sc_shiiresaki.LabelText = "";

            }
        }

        private void Scr_Clr(short Kbn)
        {
            //カスタムコントロールのLeave処理を先に走らせるため
            //  IMT_DMY_0.Focus();

            if (Kbn == 0)
            {
                foreach (Control ctl in detailControls)
                {

                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else
                    {
                        ctl.Text = "";

                    }
                }

                //foreach (Control ctl in keyLabels)
                //{
                //    ((CKM_SearchControl)ctl).LabelText = "";
                //}

                lblDisp.Text = "未売上";


                //  mTemporaryReserveNO = "";
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox))) // Check all > Uncheck
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(Panel)))   // Check only First
                {
                    kr_1.Checked = true;
                    //  kr_2.Checked = false;
                    hr_3.Checked = true;
                    // hr_4.Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                {
                    //顧客情報ALLクリア
                    ClearCustomerInfo(0);
                    ClearCustomerInfo(1);
                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }
            btn_Customer.Text = btn_Shipping.Text = "住所";
            // mOldJyuchuDate = "";
              S_Clear_Grid();   //画面クリア（明細部）


            //   mOrderTaxTiming = 0;
        }
        private void S_Clear_Grid()
        {
            int w_Row;
            int w_Col;

            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {
                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                {

                    // 配列を初期化
                    mGrid.g_MK_State[w_Col, w_Row].SetDefault();
                    // Selectable=Trueに戻ったので、下記S_DispFromArrayで TabStopがTRUEになる。そのため同期を取るためFLG更新
                    mGrid.g_GridTabStop = true;

                    if (w_Row <= m_EnableCnt - 1)
                    {
                        
                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    // クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case (int)ClsGridTenjikai.ColNO.Site:
                    //    case (int)ClsGridTenjikai.ColNO.Zaiko:
                    //        {
                    //            // ボタン
                    //            S_Set_Cell_Selectable(w_Col, w_Row, false);
                    //            break;
                    //        }
                    //}
                }
            }

            // データ用配列初期化
            Array.Clear(mGrid.g_DArray, mGrid.g_DArray.GetLowerBound(0), mGrid.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid.g_MK_Max_Row - 1; w_Row++)
                mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);

            //Btn_SelectAll.Enabled = false;
            //Btn_NoSelect.Enabled = false;
        }
        private void S_Set_Cell_Selectable(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_0.Value;

            mGrid.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(pCol, pRow);
        }
        protected void BindCombo()
        {
            cbo_nendo.Bind(C_dt);
            cbo_Shuuka.Bind(C_dt);
            cbo_season.Bind(C_dt);
            cbo_yotei.Bind(C_dt);
            BindCombo_Details();
        }
        private void BindCombo_Details()
        {
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.ShuukaSou, W_CtlRow].CellCtl;
                sctl.Cbo_Type = CKM_ComboBox.CboType.出荷倉庫;
                sctl.Bind(bbl.GetDate());
            }
        }
        //protected void EndSec()
        //{
        //    this.Close();
        //}

        private void btn_Customer_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = detailControls[(int)(Eindex.SCKokyakuu)].Text;
                addInfo.ade.CustomerName = detailControls[(int)Eindex.KJuShou1].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)Eindex.KJuShou2].Text;
                addInfo.ade.Tel11 = detailControls[(int)Eindex.KDenwa1].Text;
                addInfo.ade.Tel12 = detailControls[(int)Eindex.KDenwa2].Text;
                addInfo.ade.Tel13 = detailControls[(int)Eindex.KDenwa3].Text;
                addInfo.kbn = 0;
                addInfo.ShowDialog();

                detailControls[(int)(Eindex.pnlKokyakuu)].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_Shipping_Click(object sender, EventArgs e)
        {
            try
            {
                addInfo.ade.CustomerCD = detailControls[(int)(Eindex.SCHaiSoSaki)].Text;
                addInfo.ade.CustomerName = detailControls[(int)Eindex.HJuShou1].Text;
                addInfo.ade.CustomerName2 = detailControls[(int)Eindex.HJuShou2].Text;
                addInfo.ade.Tel11 = detailControls[(int)Eindex.HDenwa1].Text;
                addInfo.ade.Tel12 = detailControls[(int)Eindex.HDenwa2].Text;
                addInfo.ade.Tel13 = detailControls[(int)Eindex.HDenwa3].Text;
                addInfo.kbn = 1;
                addInfo.ShowDialog();

                detailControls[(int)(Eindex.pnlHaisou)].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        protected override void EndSec()
        {
            try
            {
                //DeleteExclusive();
            }
            catch (Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        public void btn_Meisai_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\ses\\";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Excel Worksheets|*.xlsx";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tkb = new TenjikaiJuuChuu_BL();
                var dt = ConvertToDataTable(openFileDialog1.FileName);

                tje = new Tenjikai_Entity
                {
                    xml = bbl.DataTableToXml(dt),
                    Kokyaku = detailControls[(int)Eindex.SCKokyakuu].Text,
                    JuchuuBi = detailControls[(int)Eindex.JuuChuuBi].Text,
                    Nendo = detailControls[(int)Eindex.Nendo].Text,
                    ShiZun = detailControls[(int)Eindex.ShiSon].Text,
                    Shiiresaki = detailControls[(int)Eindex.SCShiiresaki].Text,
                    ShuuKaSouKo = detailControls[(int)Eindex.ShuukaSouko].Text,
                    KibouBi1 = this.KibouBi1,
                    KibouBi2 = this.KibouBi2
                };
                var resTable = tkb.M_TenjiKaiJuuChuu_Select(tje);
                MesaiHyouJi(resTable);
            }
        }
        private void MesaiHyouJi(DataTable dt)
        {
            //  
            //  S_BodySeigyo(1,1);
          //  S_BodySeigyo(3, 0);
            SetMultiColNo(dt);
            S_BodySeigyo(1, 1);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
            // DisablePanel(panel1);
            ///BindCombo_Details();
            DisablePanel(PanelHeader);
        }
        private DataTable ConvertToDataTable(string FileName)
        {
            OleDbConnection oledbConn = null;
            DataTable res = null;

            try

            {

                string path = System.IO.Path.GetFullPath(FileName);

                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

                oledbConn.Open();

                OleDbCommand cmd = new OleDbCommand(); ;

                OleDbDataAdapter oleda = new OleDbDataAdapter();

                DataSet ds = new DataSet();

                DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string sheetName = string.Empty;

                if (dt != null)

                {

                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();

                }

                cmd.Connection = oledbConn;

                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                oleda = new OleDbDataAdapter(cmd);

                oleda.Fill(ds, "excelData");

                res = ds.Tables["excelData"];

            }

            catch (Exception ex)
            {
                // oledbConn.Close();
            }

            oledbConn.Close();


            KibouBi1 = res.Columns[res.Columns.Count - 2].ColumnName;
            KibouBi2 = res.Columns[res.Columns.Count - 1].ColumnName;
            res.Columns[res.Columns.Count - 2].ColumnName = "希望日1";
            res.Columns[res.Columns.Count - 1].ColumnName = "希望日2";
            foreach (DataColumn c in res.Columns)
            {
                c.ColumnName = c.ColumnName.Trim();
            }
            return res;

        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Vsb_Mei_0_ValueChanged(object sender, System.EventArgs e)
        {
            int w_Dest = 0;
            int w_Row = 0;
            bool w_IsGrid;

            if (mGrid.g_VSB_Flg == 1)
                return;

            w_IsGrid = mGrid.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow);
            if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            {

                // 明細部にフォーカスがあるとき
                w_Row = w_CtlRow + Vsb_Mei_0.Value;

                if (Math.Sign(this.Vsb_Mei_0.Value - mGrid.g_MK_DataValue) == -1)
                    // 上へスクロール
                    w_Dest = (int)ClsGridTenjikai.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt;

                // 一旦 フォーカスを退避
                //IMT_DMY_0.Focus();
            }

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid.S_DispToArray(mGrid.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

            if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            {

                // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
                if (w_Dest == (int)ClsGridTenjikai.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                {
                    if (mGrid.g_WheelFLG == true)
                    {
                        // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
                        // 最後のフォーカス可能セルに移動
                        if (mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
                            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                    }
                    else
                        // その行の先頭から探す
                        mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
                }
            }

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            panel2.Refresh();
        }
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        detailControls[(int)Eindex.SCShiiresaki].Focus();
                        this.Vsb_Mei_0.TabStop = false;
                        SetFuncKeyAll(this, "111111001010");
                        break;
                    }

                case 1:
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;
                                if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SKUCD))
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        {
                                            case (int)ClsGridTenjikai.ColNO.SCJAN:
                                            case (int)ClsGridTenjikai.ColNO.ShouName:
                                            //case (int)ClsGridTenjikai.ColNO.Color:
                                            //case (int)ClsGridTenjikai.ColNO.ColorName:
                                            //case (int)ClsGridTenjikai.ColNO.Size:
                                            //case (int)ClsGridTenjikai.ColNO.SizeName:
                                            case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                                            case (int)ClsGridTenjikai.ColNO.ChoukuSou:
                                            case (int)ClsGridTenjikai.ColNO.ShuukaSou:
                                            case (int)ClsGridTenjikai.ColNO.HacchuTanka:
                                            case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                                            case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                                            //case (int)ClsGridTenjikai.ColNO.TenI:
                                            case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
                                            //case (int)ClsGridTenjikai.ColNO.ZeinuJuchuu:
                                            //case (int)ClsGridTenjikai.ColNO.zeikomijuchuu:
                                            //case (int)ClsGridTenjikai.ColNO.ArariGaku:
                                            //case (int)ClsGridTenjikai.ColNO.ZeiNu:
                                            //case (int)ClsGridTenjikai.ColNO.ZeinuTanku:
                                            case (int)ClsGridTenjikai.ColNO.Chk:
                                            //case (int)ClsGridTenjikai.ColNO.ShanaiBi:
                                            //case (int)ClsGridTenjikai.ColNO.ShagaiBi:
                                            //case (int)ClsGridTenjikai.ColNO.KobeTsu:
                                                {
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                    break;
                                                }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // txtStartDateFrom.Focus();
                            SetFuncKeyAll(this, "111111000001");
                            // btnDisplay.Enabled = false;
                        }
                        break;
                    }

                case 2:
                    {
                        if (pGrid == 1)
                        {
                            panel2.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //  Scr_Lock(0, 0, 0);


                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
                                // Scr_Lock(0, 3, 1);
                            }
                            else
                            {
                                SetFuncKeyAll(this, "111111000010");
                            }

                            // 削除時のみ、明細を参照できるように、スクロールバーのTabStopをTrueにする
                            this.Vsb_Mei_0.TabStop = true;
                        }

                        break;
                    }

                case 3:
                    Scr_Clr(1);
                    break;
                default:
                    {
                        break;
                    }
            }
            
        }
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
                        ChangeOperationMode((EOperationMode)Index);

                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        ChangeOperationMode(base.OperationMode);

                        break;
                    }
                case 6://F7:行削除
                    ADD_SUB();
                    break;

                case 7://F8:行追加
                    DEL_SUB();
                    break;

                case 9://F10複写
                    CPY_SUB();
                    break;

                case 8: //F9:検索
                   // EsearchKbn kbn = EsearchKbn.Null;
                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    //if (w_Col == (int)ClsGridJuchuu.ColNO.JanCD)
                    //    //商品検索
                    //    kbn = EsearchKbn.Product;
                    //else if (w_Col == (int)ClsGridJuchuu.ColNO.VendorCD)
                    //    //仕入先検索
                    //    kbn = EsearchKbn.Vendor;

                    //if (kbn != EsearchKbn.Null)
                    //    SearchData(kbn, previousCtrl);

                    break;

                case 11:    //F12:登録
                    {
                        if (OperationMode == EOperationMode.DELETE)
                        { //Ｑ１０２		
                            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                                return;
                        }
                        else
                        {
                            //Ｑ１０１		
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                                return;
                        }


                        this.ExecSec();
                        break;
                    }
            }   //switch end

        }
        private bool CheckKey(int index, bool set = true)
        {
            if (index == -1)
            {
                var Con = new Control[] {
                    ((CKM_TextBox)detailControls[(int)Eindex.SCShiiresaki]),
                    (CKM_ComboBox)detailControls[(int)Eindex.Nendo],
                    (CKM_ComboBox)detailControls[(int)Eindex.ShiSon],
                    (CKM_TextBox)detailControls[(int)Eindex.JuuChuuBi],
                    (CKM_TextBox)detailControls[(int)Eindex.SCTentouStaffu],
                    (CKM_TextBox)detailControls[(int)Eindex.SCKokyakuu],
                    (CKM_TextBox)detailControls[(int)Eindex.SCHaiSoSaki],
                    (CKM_ComboBox)detailControls[(int)Eindex.ShuukaSouko],
                    (CKM_ComboBox)detailControls[(int)Eindex.YoteiKinShuu],
                    };
                foreach (var c in Con)
                {
                    if (c is CKM_ComboBox co)
                    {
                        co.Require(true);
                    }
                    else if (c is CKM_TextBox ct)
                    {
                        ct.Require(true);
                    }
                }
                return RequireCheck(Con);
                
            }

            return true;

        }
        protected override void ExecSec()
        {

            if (OperationMode == EOperationMode.INSERT) // Keys
            {
                if (CheckKey(-1, false) == false)
                {
                    return;
                }
            }
            else
            {
                for (int i = 0; i < keyControls.Length; i++)
                    if (CheckKey(i, false) == false)
                    {
                        keyControls[i].Focus();
                        return;
                    }
            }

            if (OperationMode != EOperationMode.DELETE)  // Details
            {
                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                //dje = new D_Juchuu_Entity();
                //dje.JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text == "" ? mTemporaryReserveNO : keyControls[(int)EIndex.JuchuuNO].Text;

                ////D_TemporaryReserveをDelete
                //mibl.DeleteTemporaryReserve(dje);

                //// 明細部  画面の範囲の内容を配列にセット
                //mGrid.S_DispToArray(Vsb_Mei_0.Value);

                ////明細部チェック
                //for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                //{
                //    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                //    {
                //        for (int CL = (int)ClsGridJuchuu.ColNO.JanCD; CL < (int)ClsGridJuchuu.ColNO.COUNT; CL++)
                //        {
                //            if (CheckGrid(CL, RW, true) == false)
                //            {
                //                //Focusセット処理
                //                ERR_FOCUS_GRID_SUB(CL, RW);
                //                return;
                //            }
                //        }
                //    }
                //}

                ////各金額項目の再計算必要
                //CalcKin();

            }

            //M_Control.Tennic＝1 の場合、
            if (mTennic.Equals(1))
            {
                //与信チェックを行う
                //Function_与信チェックより以下情報を取得する
                Fnc_Credit_Entity fce = new Fnc_Credit_Entity
                {
                    Operator = InOperatorCD,
                    PC = InPcID,
                    //ChangeDate = detailControls[(int)EIndex.JuchuuDate].Text,
                    //CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                };

                bool ret = bbl.Fnc_Credit(fce);
                if (ret)
                {
                    //out与信チェック区分 0:なし、1:警告、2:エラー
                    if (fce.CreditCheckKBN.Equals("0"))
                    {
                        //●与信チェック区分＝0の場合、処理なし 次工程へ

                    }
                    //else if (fce.CreditCheckKBN.Equals("1"))
                    //{
                    //    //●与信チェック区分＝1の場合、
                    //    //●与信限度額<●総債権額＋Footer.税込売上額の場合、警告表示			
                    //    if (bbl.Z_Set(fce.CreditAmount) < bbl.Z_Set(fce.SaikenGaku) + bbl.Z_Set(lblKin6.Text))
                    //    {
                    //        if (bbl.ShowMessage("Q324", bbl.Z_SetStr(fce.CreditAmount), bbl.Z_SetStr(fce.SaikenGaku), bbl.Z_SetStr(lblKin6.Text), fce.CreditMessage) != DialogResult.Yes)
                    //            return;
                    //    }
                    //}
                    //else if (fce.CreditCheckKBN.Equals("2"))
                    //{
                    //    //●与信チェック区分＝2の場合、
                    //    //●与信限度額<●総債権額＋Footer.税込売上額の場合、警告表示			
                    //    if (bbl.Z_Set(fce.CreditAmount) < bbl.Z_Set(fce.SaikenGaku) + bbl.Z_Set(lblKin6.Text))
                    //    {
                    //        bbl.ShowMessage("E261", bbl.Z_SetStr(fce.CreditAmount), bbl.Z_SetStr(fce.SaikenGaku), bbl.Z_SetStr(lblKin6.Text), fce.CreditMessage);
                    //        return;
                    //    }
                    //}

                }
            }

            //DataTable dt = GetGridEntity();

            ////更新処理
            //dje = GetEntity();
            //mibl.Juchu_Exec(dje, dt, (short)OperationMode);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void CPY_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            w_Act = this.ActiveControl;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //先頭行のとき、複写不可
            if (w_Row == 0)
                return;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            int col = (int)ClsGridTenjikai.ColNO.SCJAN;

            //コピー行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i >= w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号

                //if (i.Equals(w_Row))
                //{
                //    //前の行をコピーしてできた新しい行
                //    //mGrid.g_DArray[i].JuchuuNO = "";
                //    //mGrid.g_DArray[i].juchuGyoNO = 0;
                //    //mGrid.g_DArray[i].copyJuchuGyoNO = 0;
                //    //mGrid.g_DArray[i].KariHikiateNO = "";
                //}

                Grid_NotFocus(col, i);
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }

            string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            // CheckHikiate(w_Row, ymd);
            Grid_NotFocus(col, w_Row);
           // CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet,sc_shiiresaki, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
        }
        private void DEL_SUB()
        {
            int w_Row;

            Control w_Act = ActiveControl;
            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細が出荷済の受注明細、または、売上済の受注明細である場合、削除できない（F7ボタンを使えないようにする）
            //出荷済・売上済の場合は行削除不可
            //if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
            //    return;

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

           // CalcKin();

            int col = (int)ClsGridTenjikai.ColNO.SCJAN;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
              scjan_1.TxtCode.Focus();

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }
        private void ADD_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }
            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //追加行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i > w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //前行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            w_Gyo = Convert.ToInt16(mGrid.g_DArray[w_Row].GYONO);
            // 一行クリア
            Array.Clear(mGrid.g_DArray, w_Row, 1);
            //退避内容を戻す
            mGrid.g_DArray[w_Row].GYONO = w_Gyo.ToString();          //行番号

           // CalcKin();

            int col = (int)ClsGridTenjikai.ColNO.SCJAN;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }
        private void Grid_NotFocus(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            bool w_AllFlg = false;
            int w_CtlRow;

            if (OperationMode >= EOperationMode.DELETE)
                return;

            if (m_EnableCnt - 1 < pRow)
                return;

            w_CtlRow = pRow - Vsb_Mei_0.Value;
            if (w_CtlRow >= 0 && w_CtlRow <= mGrid.g_MK_Ctl_Row - 1)
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);

            

        }
    }
}
