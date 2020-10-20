using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using CKM_Controls;
using Entity;
using GridBase;
using Search;

namespace MasterTouroku_TenzikaiShouhin
{
    public partial class MasterTouroku_TenzikaiShouhin : FrmMainForm
    {
        MasterTouroku_TenzikaiShouhin_BL tbl;
        private const string ProID = "MasterTouroku_TenzikaiShouhin";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        Base_BL bl;
        M_TenzikaiShouhin_Entity mt;
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        ClsGridMasterTanzi mGrid = new ClsGridMasterTanzi();
        private int m_EnableCnt = 0;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxJyuchuGyoNo;
        private System.Windows.Forms.Control previousCtrl;
        public MasterTouroku_TenzikaiShouhin()
        {

            InitializeComponent();
            bl = new Base_BL();
            tbl = new MasterTouroku_TenzikaiShouhin_BL();
            mt = new M_TenzikaiShouhin_Entity();
        }
        private enum EsearchKbn : short
        {
            Null,
            Product,
            Vendor,
            Brand,
            Segment,
            Tani,
        }

        private enum Eindex : int
        {
            SCTenzikai,
            SCShiiresaki,
            Nendo,
            Season,
            SCBrand,
            SCSegment,
            SCCTenzikai,
            SCCShiiresaki,
            CNendo,
            CSeason,
            SCCBrand,
            SCCSegment,
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
        private void MasterTouroku_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_TenzikaiShouhin";
            this.SetFunctionLabel(EProMode.INPUT);
            this.InitialControlArray();
            this.S_SetInit_Grid();
            SC_CopyTenzikai.TxtCode.MaxLength = 500;
            Btn_F7.Text = "行削除（F7）";
            Btn_F8.Text = "行追加（F8）";
            Scr_Clr(0);

            BindCombo_Details();
            StartProgram();

            SC_Segment.Value1 = "226";
            string ymd = bl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_copyseason.Bind(ymd);
            CB_Copyyear.Bind(ymd);

          
        }

        class Person
        {
            public string Name
            {
                get;
                set;
            }
        }
       
        private void BindCombo_Details()
            {
           
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                
                CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, W_CtlRow].CellCtl;
                sctl.Items.Add("上旬");
                sctl.Items.Add("中旬");
                sctl.Items.Add("下旬");

                CKM_Controls.CKM_ComboBox sctl1 = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, W_CtlRow].CellCtl;
                sctl1.Items.Add("非課税");
                sctl1.Items.Add("通常課税");
                sctl1.Items.Add("軽減課税");
                
            }
        }
        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
           // MoveNextControl(e);

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
            int a = 0;
            try
            {

                foreach (Control ctl in detailControls)
                {
                    a++;
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox))) // Check all > Uncheck
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else if (ctl.GetType().Equals(typeof(Panel)))   // Check only First
                    {
                        //kr_1.Checked = true;
                        //  kr_2.Checked = false;
                        // hr_3.Checked = true;
                        // hr_4.Checked = false;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                        try
                        {
                            ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                        }
                        catch
                        {
                            ((CKM_Controls.CKM_ComboBox)ctl).SelectedValue = -1;
                        }
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                    {
                        //顧客情報ALLクリア
                        // ClearCustomerInfo(0);
                        // ClearCustomerInfo(1);
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }
            }
            catch
            {
                var d = a;
            }

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }
            //btn_Customer.Text = btn_Shipping.Text = "住所";
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

        private void InitialControlArray()
        {

            detailControls = new Control[] { SC_Tenzikai.TxtCode,SC_Vendor.TxtCode,CB_Year, CB_Season,SC_Brand.TxtCode,SC_Segment.TxtCode,SC_CopyTenzikai.TxtCode,SC_CopyVendor.TxtCode,
                                             CB_Copyyear,CB_copyseason, SC_copybrand.TxtCode,SC_copysegmet.TxtCode,TB_InsertDateTimeF};
            detailLabels = new Control[] { };
            searchButtons = new Control[] { SC_Tenzikai.BtnSearch, SC_Vendor.BtnSearch, SC_Brand.BtnSearch, SC_Segment.BtnSearch, SC_CopyTenzikai.BtnSearch ,
                                            SC_CopyVendor.BtnSearch,SC_copybrand.BtnSearch,SC_copysegmet.BtnSearch };
            foreach (var c in detailControls)
            {
                c.KeyDown += C_KeyDown;
                c.Enter += C_Enter;
                mGrid.S_DispToArray(Vsb_Mei_0.Value);
                if (c is CKM_ComboBox cb && cb.Name == "")
                {
                    // cb.SelectedIndexChanged += ShuukaSouko_SelectedIndexChanged;
                }
            }


        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)
            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            ////配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    //   detailControls[(int)Eindex.JuuChuuBi].Focus();
                    detailControls[(int)Eindex.SCTenzikai].Focus();
                    BT_SKUCheck.Enabled = false;
                    //F9Visible = false;
                    Clear(panel1);
                    Clear(panel3);
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    EnablePanel(PanelHeader);
                    BT_SKUCheck.Enabled = true;
                    
                    break;

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
                       // detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
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
                   // ADD_SUB();
                    break;

                case 7://F8:行追加
                   // DEL_SUB();
                    break;

                case 9://F10複写
                    //CPY_SUB();
                    break;

                case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    //if (w_Col == (int)ClsGridTenjikai.ColNO.SCJAN)
                    //    //商品検索
                    //    kbn = EsearchKbn.Product;
                    ////else if (w_Col == (int)ClsGridTenjikai.ColNO.VendorCD)
                    ////    //仕入先検索
                    ////    kbn = EsearchKbn.Vendor;

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
            }
        }
       
        private void S_SetInit_Grid()
        {
            int W_CtlRow;  // Grid Properties

            if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
            }
          

            mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
           
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray(); // g_mk_ctrl // Control Assigned

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
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            //if (w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
                            //    btn.Click += new System.EventHandler(BTN_Site_Click);
                            //else
                            //    btn.Click += new System.EventHandler(BTN_Zaiko_Click);
                        }
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
            mGrid.g_MK_State = new ClsGridMasterTanzi.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row]; 
            mGrid.g_DArray = new ClsGridMasterTanzi.ST_DArray_Grid[mGrid.g_MK_Max_Row];  
            SetMultiColNo();
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.GridColor);//, "1");      // 2行目(偶数行) 水色

          
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridMasterTanzi.ColNO.GYONO; i <= (int)ClsGridMasterTanzi.ColNO.Count - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridMasterTanzi.ColNO.Count; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                        case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:

                            //  mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;//ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Price; //Ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).IntegerPart = 8; // ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).DecimalPlace = 0;
                            break;

                        case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
                            break;
                        //case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                        //    ((CKM_Controls.CKMShop_ComboBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Byte = CKMShop_ComboBox.CboType.Default;
                        //    break;
                        case (int)ClsGridMasterTanzi.ColNO.JANCD:
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 JANCD_Detail
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
                            //((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.JANCD;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                          //  ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 6;
                           // ((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.ブランド;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                           // ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 6;
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.Width = 50;
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).Value1 = "226";
                            //((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.商品分類;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.Width = 30;
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).Value1 = "202";
                           ///((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.単位;
                            break;
                    }
                  

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            Set_GridTabStop(false);
            S_Clear_Grid();   //Scr_Clr処理で
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
        private void GridControl_EnableChnaged(object sender, EventArgs e)
        {
            if (!(sender as CKM_TextBox).Enabled)
            {
                (sender as CKM_TextBox).Back_Color = CKM_TextBox.CKM_Color.DarkGrey;
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;
                Control sc = ((Control)sender).Parent;

               
                if (((Search.CKM_SearchControl)sc).Name.Contains("scjan"))
                {
                    kbn = EsearchKbn.Product;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("brand"))
                {

                    kbn = EsearchKbn.Brand;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("segment"))
                {

                    kbn = EsearchKbn.Segment;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("taniCD"))
                {

                    kbn = EsearchKbn.Tani;
                }

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
        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;


                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
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
                            CL = (int)ClsGridMasterTanzi.ColNO.JANCD;
                            break;
                        case "sku":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUCD;
                            break;
                        case "shouhin":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUName;
                            break;
                        case "color":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorCD;
                            break;
                        case "colorname":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorName;
                            break;
                        case "size":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeCD;
                            break;
                        case "sizename":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeName;
                            break;
                        case "hanbaidatemonth":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth;
                            break;
                        case "hanbaibi":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi;
                            break;
                        case "shiiretanka":
                            CL = (int)ClsGridMasterTanzi.ColNO.Shiiretanka;
                            break;
                        case "joutaitanka":
                            CL = (int)ClsGridMasterTanzi.ColNO.JoutaiTanka;
                            break;
                        case "salepriceout":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax;
                            break;
                        case "salepriceout1":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1;
                            break;
                        case "salepriceout2":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2;
                            break;
                        case "salepriceout3":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3;
                            break;
                        case "salepriceout4":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4;
                            break;
                        case "salepriceout5":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5;
                            break;
                        case "joutaitank":
                            CL = (int)ClsGridMasterTanzi.ColNO.SegmentCD;
                            break;
                        case "joutaitanka1":
                            CL = (int)ClsGridMasterTanzi.ColNO.BrandCD;
                            break;
                        case "tanicd":
                            CL = (int)ClsGridMasterTanzi.ColNO.TaniCD;
                            break;
                        case "taxrate":
                            CL = (int)ClsGridMasterTanzi.ColNO.TaxRateFlg;
                            break;
                        case "remark":
                            CL = (int)ClsGridMasterTanzi.ColNO.Remarks;
                            break;

                        case "chk":
                            CL = (int)ClsGridMasterTanzi.ColNO.Chk;
                            break;

                    }

                    bool changeFlg = false;
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    if (CheckGrid(CL, w_Row) == false)
                    {
                        if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            ((CKM_Controls.CKM_ComboBox)w_ActCtl).MoveNext = false;
                        }
                        
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        w_ActCtl.Focus();
                        return;
                    }

                    if (lastCell)
                    {
                        w_ActCtl.Focus();
                        return;
                    }

                    S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                // w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                if (w_ActCtl is GridControl.clsGridCheckBox chk && chk.Name.Contains("chk_"))
                {
                    if (chk.Parent is Panel pnl)
                    {
                        pnl.BackColor = Color.White; ;
                    }
                }
                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                // 背景色
                // w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);

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
                w_ActCtl.BackColor = ClsGridMasterTanzi.BKColor;

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

                if (ActiveControl is GridControl.clsGridCheckBox chk && chk.Name.Contains("chk_"))
                {
                    if (chk.Focus())
                    {
                        if (chk.Parent is Panel pnl)
                        {
                            pnl.BackColor = ClsGridMasterTanzi.BKColor; ;
                        }
                    }
                }
                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                // Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

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
            // Set_GridTabStop(true);

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
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.GYONO, d].CellCtl = this.Controls.Find("LB_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Chk, d].CellCtl = this.Controls.Find("chk_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JANCD, d].CellCtl = this.Controls.Find("scjan_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUCD, d].CellCtl = this.Controls.Find("sku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUName, d].CellCtl = this.Controls.Find("shouhin_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorCD, d].CellCtl = this.Controls.Find("colcd_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorName, d].CellCtl = this.Controls.Find("colorName_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeCD, d].CellCtl = this.Controls.Find("sizecd_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeName, d].CellCtl = this.Controls.Find("sizeName_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth, d].CellCtl = this.Controls.Find("hyoteidatem_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, d].CellCtl = this.Controls.Find("hyoteidate_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Shiiretanka, d].CellCtl = this.Controls.Find("shiire_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JoutaiTanka, d].CellCtl = this.Controls.Find("joutai_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax, d].CellCtl = this.Controls.Find("salepriceout_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1, d].CellCtl = this.Controls.Find("salepriceout1_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2, d].CellCtl = this.Controls.Find("salepriceout2_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3, d].CellCtl = this.Controls.Find("salepriceout3_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4, d].CellCtl = this.Controls.Find("salepriceout4_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5, d].CellCtl = this.Controls.Find("salepriceout5_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.BrandCD, d].CellCtl = this.Controls.Find("brand_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SegmentCD, d].CellCtl = this.Controls.Find("segment_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaniCD, d].CellCtl = this.Controls.Find("taniCD_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, d].CellCtl = this.Controls.Find("taxrate_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remarks, d].CellCtl = this.Controls.Find("remark_" + id, true)[0];
                    // mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remarks, d].sCellCtl = this.Controls.Find("taxrate_" + id, true)[0];

                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message + ex.StackTrace;

            }

        }
        private void DisplayData(int type)
        {
            //if(type == 1)
            //mt = new M_TenzikaiShouhin_Entity
            //{
            //    // TanKaCD=
            //    TenzikaiName =sc.Text,
            //    VendorCD = sc.Text,
            //    LastYearTerm = sc.Text,
            //    LastSeason = sc.Text,
            //    BranCDFrom = sc.Text,
            //    SegmentCDFrom = sc.Text,

            //};
            //DataTable dtshow = tbl.Mastertoroku_Tenzikaishouhin_Select(mt, "2");
            //if(dtshow !=null)
            //{
            //    MesaiHyouJi(dtshow);
            //    //return true;
            //}
            //else
            //{
            //    bl.ShowMessage("E128");
            //  //  return false;
            //}
        }
        private void BT_Display_Click(object sender, EventArgs e)
        {
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    if (!CheckKey(-1) || !CheckKey(1))
                    {
                        return;
                    }
                    if (!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
                    {
                        mt = new M_TenzikaiShouhin_Entity
                        {
                            TenzikaiName = detailControls[(int)Eindex.SCCTenzikai].Text,
                            VendorCD = detailControls[(int)Eindex.SCCShiiresaki].Text,
                            LastYearTerm = detailControls[(int)Eindex.CNendo].Text,
                            LastSeason = detailControls[(int)Eindex.CSeason].Text,
                            BranCDFrom = detailControls[(int)Eindex.SCCBrand].Text,
                            SegmentCDFrom = detailControls[(int)Eindex.SCCSegment].Text,

                        };
                        DataTable dtshow = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                        if (dtshow != null)
                        {
                            MesaiHyouJi(dtshow);
                            S_BodySeigyo(4,0);
                        }
                        else
                        {
                            bl.ShowMessage("E128");
                        }
                    }
                    else
                    {
                        scjan_1.TxtCode.Focus();
                    }
                    break;
                case EOperationMode.UPDATE:
                    mt = new M_TenzikaiShouhin_Entity
                    {
                        TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                        VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                        LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                        LastSeason = detailControls[(int)Eindex.Season].Text,
                        BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                        SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                    };
                    DataTable dtmain = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                    if (dtmain != null)
                    {
                        MesaiHyouJi(dtmain);
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
                case EOperationMode.DELETE:
                    break;
                case EOperationMode.SHOW:
                    break;
            }
        }
        private void DataDisplay()
        {
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    if (!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
                    {
                        mt = new M_TenzikaiShouhin_Entity
                        {
                            // TanKaCD=
                            TenzikaiName = detailControls[(int)Eindex.SCCTenzikai].Text,
                            VendorCD = detailControls[(int)Eindex.SCCShiiresaki].Text,
                            LastYearTerm = detailControls[(int)Eindex.CNendo].Text,
                            LastSeason = detailControls[(int)Eindex.CSeason].Text,
                            BranCDFrom = detailControls[(int)Eindex.SCCBrand].Text,
                            SegmentCDFrom = detailControls[(int)Eindex.SCCSegment].Text,

                        };
                        DataTable dtshow = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                        if (dtshow != null)
                        {
                            MesaiHyouJi(dtshow);
                            //return true;
                        }
                        else
                        {
                            bl.ShowMessage("E128");
                        }
                    }
                    else
                    {
                        scjan_1.TxtCode.Focus();
                    }
                    break;
                case EOperationMode.UPDATE:
                    mt = new M_TenzikaiShouhin_Entity
                    {
                        // TanKaCD=
                        TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                        VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                        LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                        LastSeason = detailControls[(int)Eindex.Season].Text,
                        BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                        SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                    };
                    DataTable dtmain = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                    if (dtmain != null)
                    {
                        MesaiHyouJi(dtmain);
                        //return true;
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
                case EOperationMode.DELETE:

                    break;
                case EOperationMode.SHOW:

                    break;
            }
        }
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            int w_Row = w_CtlRow + Vsb_Mei_0.Value;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);
            string date = bl.GetDate();
            switch (kbn)
            {
                case EsearchKbn.Product:
                    using (Search_Product frmProduct = new Search_Product(date))
                    {
                        frmProduct.JANCD = mGrid.g_DArray[w_Row].JANCD;
                        frmProduct.ShowDialog();
                        if (!frmProduct.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].JANCD = frmProduct.JANCD;
                            CheckGrid((int)ClsGridMasterTanzi.ColNO.JANCD, w_Row, false, true);
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
                
                case EsearchKbn.Brand:
                    using (Search_Brand frmBrand = new Search_Brand())
                    {
                        frmBrand.parBrandCD = mGrid.g_DArray[w_Row].BrandCD;
                        frmBrand.ShowDialog();
                        if (!frmBrand.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].BrandCD = frmBrand.parBrandCD;
                            CheckGrid((int)ClsGridMasterTanzi.ColNO.BrandCD, w_Row, false, true);
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
                case EsearchKbn.Segment:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {
                        frmMulti.parID = "226";
                        frmMulti.parKey = mGrid.g_DArray[w_Row].SegmentCD; 
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].SegmentCD = frmMulti.parKey;
                            CheckGrid((int)ClsGridMasterTanzi.ColNO.SegmentCD, w_Row, false, true);
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;

                case EsearchKbn.Tani:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {

                        frmMulti.parID = "202";
                        frmMulti.parKey= mGrid.g_DArray[w_Row].TaniCD;
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].TaniCD = frmMulti.parKey;
                            CheckGrid((int)ClsGridMasterTanzi.ColNO.TaniCD, w_Row, false, true);
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
            }
        }

        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false, bool IsExec = false)
        {
            try
            {
                bool ret = false;
                string ymd = bl.GetDate();
                if (string.IsNullOrWhiteSpace(ymd))
                    ymd = bbl.GetDate();
                if (!chkAll && !changeYmd) // check length
                {
                    int w_CtlRow = row - Vsb_Mei_0.Value;

                    if (w_CtlRow < ClsGridMasterTanzi.gc_P_GYO)
                        if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                                return false;
                        }
                }
                //if (IsExec ? !mGrid.g_DArray[row].Chk : false || string.IsNullOrEmpty(mGrid.g_DArray[row].JANCD))
                //{
                //    return true;
                //}
                switch (col)
                {
                    case (int)ClsGridMasterTanzi.ColNO.JANCD:

                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JANCD))
                        {
                            Grid_Gyo_Clr(row);//1014
                                              // return true;
                        }
                        M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity
                        {
                            TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                            VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                            LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                            LastSeason = detailControls[(int)Eindex.Season].Text,
                            BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                            SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                            JANCD = mGrid.g_DArray[row].JANCD,
                        };

                        DataTable dt = tbl.M_Tenzikaishouhin_SelectForJancd(mt);
                        DataTable dtsku = null;
                        if (dt.Rows.Count > 0)
                        {
                            tbl.ShowMessage("E107");
                            return false;
                        }
                        else
                        {
                            M_SKU_Entity msku = new M_SKU_Entity
                            {
                                JanCD = mGrid.g_DArray[row].JANCD,
                                MainVendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                            };
                            SKU_BL mbl = new SKU_BL();
                            dtsku = mbl.M_SKU_SelectByJanCD_ForTenzikaishouhin(msku);
                        }
                        DataRow selectRow = null;

                        if (dtsku != null)
                        {
                            selectRow = dtsku.Rows[0];
                        }
                        if (selectRow != null)
                        {

                            mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                            mGrid.g_DArray[row].SKUName = selectRow["SKUName"].ToString();
                            mGrid.g_DArray[row].ColorCD = selectRow["ColorNO"].ToString();
                            mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                            mGrid.g_DArray[row].SizeCD = selectRow["SizeNo"].ToString();
                            mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                            // mGrid.g_DArray[row].HanbaiYoteiDateMonth = selectRow["HanbaiYoteiDateMonth"].ToString();
                            // mGrid.g_DArray[row].HanbaiYoteiBi = selectRow["HanbaiYoteiDate"].ToString();
                            mGrid.g_DArray[row].Shiiretanka = selectRow["SiireTanka"].ToString();
                            mGrid.g_DArray[row].JoutaiTanka = selectRow["JoudaiTanka"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax = selectRow["SalePriceOutTax"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax1 = selectRow["TankKa1"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax2 = selectRow["TankKa2"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax3 = selectRow["TankKa3"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax4 = selectRow["TankKa4"].ToString();
                            //mGrid.g_DArray[row].SalePriceOutTax5 = selectRow["TankKa5"].ToString();
                            //mGrid.g_DArray[row].BrandCD = selectRow["BrandCD"].ToString();
                            //mGrid.g_DArray[row].SegmentCD = selectRow["SegmentCD"].ToString();
                            //mGrid.g_DArray[row].TaniCD = selectRow["TaniCD"].ToString();
                            //mGrid.g_DArray[row].TaxRateFlg = selectRow["TaxRateFlg"].ToString();
                            //mGrid.g_DArray[row].Remarks = selectRow["Remarks"].ToString();
                            //mGrid.g_DArray[row]. = Convert.ToInt16(selectRow["ExhibitionCommonCD"].ToString()).ToString();
                        }
                        Grid_NotFocus(col, row);
                        break;

                    case (int)ClsGridMasterTanzi.ColNO.SKUCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SKUCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SKUCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SKUName:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SKUName))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SKUName = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.ColorCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ColorCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].ColorCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.ColorName:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ColorName))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].ColorName = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SizeCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SizeCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SizeCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SizeName:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SizeName))
                            {
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SizeName = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].Shiiretanka))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].Shiiretanka = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JoutaiTanka))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].JoutaiTanka = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SalePriceOutTax = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax1))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            mGrid.g_DArray[row].SalePriceOutTax1 = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].BrandCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            // mGrid.g_DArray[row].BrandCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SegmentCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            // mGrid.g_DArray[row].SegmentCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TaniCD))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            /// mGrid.g_DArray[row].TaniCD = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                    case (int)ClsGridMasterTanzi.ColNO.TaxRateFlg:
                        if (mGrid.g_MK_State[col, row].Cell_Enabled)
                        {
                            //Se(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TaxRateFlg))
                            {
                                //Ｅ１０２
                                bbl.ShowMessage("E102");
                                return false;
                            }
                            // mGrid.g_DArray[row].TaxRateFlg = ActiveControl.Text;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        }
                        break;
                }
                if (chkAll == false)
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                return true;
            }
            catch (Exception ec){ MessageBox.Show(ec.StackTrace.ToString() + col + row); return true; }
        }
        //private void CalcKin()
        //{
        //    decimal ExcAmt = 0;
        //    decimal IncAmt = 0;
        //    decimal CostAmt = 0;
        //    decimal GrossAmt = 0;
        //    decimal CsumAmt = 0;
        //    decimal NmalAmt = 0;
        //    decimal RdueAmt = 0;

        //    decimal kin1 = 0;
        //    decimal kin2 = 0;
        //    decimal kin3 = 0;
        //    decimal kin4 = 0;
        //    decimal kin5 = 0;
        //    decimal zei10 = 0;  //tsuujou
        //    decimal zei8 = 0;  // keijen

        //    decimal kin10 = 0;
        //    decimal kin8 = 0;
        //    int zeiritsu10 = 0;
        //    int zeiritsu8 = 0;
        //    int maxKinRowNo = 0;
        //    decimal maxKin = 0;

        //    decimal kinOrder10 = 0;      //発注通常税額(Hidden)
        //    decimal kinOrder8 = 0;      //発注軽減税額(Hidden)

        //    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
        //    {
        //        ///if (mGrid.g_DArray[RW].Chk)
        //        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SCJAN) && mGrid.g_DArray[RW].Chk)
        //        {
        //            ExcAmt += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu); // 1
        //            IncAmt += bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu); //2
        //            CostAmt += bbl.Z_Set(mGrid.g_DArray[RW].HacchuTanka) * bbl.Z_Set(mGrid.g_DArray[RW].JuchuuSuu);

        //            zei10 += bbl.Z_Set(mGrid.g_DArray[RW].Tsuujou);
        //            zei8 += bbl.Z_Set(mGrid.g_DArray[RW].Keigen);

        //            if (mTaxTiming.Equals("2"))
        //            {
        //                if (mGrid.g_DArray[RW].TaxRateFlg.Equals("1"))
        //                {
        //                    kin10 += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu);
        //                    if (zeiritsu10 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ZeinuTanku))
        //                        zeiritsu10 = Convert.ToInt16(mGrid.g_DArray[RW].ZeinuTanku.Replace("%", ""));
        //                }
        //                else if (mGrid.g_DArray[RW].TaxRateFlg.Equals("2"))
        //                {
        //                    kin8 += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu);
        //                    if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ZeinuTanku))
        //                        zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].ZeinuTanku.Replace("%", ""));
        //                }

        //                if (maxKin < bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu))
        //                {
        //                    maxKin = bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu);
        //                    maxKinRowNo = RW;
        //                }
        //            }
        //        }
        //    }

        //    GrossAmt = ExcAmt - CostAmt;

        //    if (Convert.ToInt32(mTaxTiming) == 1 || Convert.ToInt32(mTaxTiming) == 3)
        //    {
        //        CsumAmt = zei10 + zei8;
        //        NmalAmt = bbl.Z_Set(zei10);
        //        RdueAmt = bbl.Z_Set(zei8);
        //    }
        //    else if (Convert.ToInt32(mTaxTiming) == 2)
        //    {
        //        kin10 = GetResultWithHasuKbn(mTaxFractionKBN, kin10 * zeiritsu10 / 100);
        //        kin8 = GetResultWithHasuKbn(mTaxFractionKBN, kin8 * zeiritsu8 / 100);

        //        decimal sagaku = (kin10 + kin8) - (zei10 + zei8);
        //        CsumAmt = kin10 + kin8;
        //        if (sagaku != 0)
        //        {
        //            mGrid.g_DArray[maxKinRowNo].zeikomijuchuu = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxKinRowNo].zeikomijuchuu) + sagaku);  // Add percent Error Amount in the Max row or first line if same 
        //            IncAmt += sagaku;  //Add percent Error Amount
        //            if (mGrid.g_DArray[maxKinRowNo].TaxRateFlg.Equals("1"))
        //            {
        //                mGrid.g_DArray[maxKinRowNo].Tsuujou = mGrid.g_DArray[maxKinRowNo].Tsuujou + sagaku;
        //            }
        //            else if (mGrid.g_DArray[maxKinRowNo].TaxRateFlg.Equals("2"))
        //            {
        //                mGrid.g_DArray[maxKinRowNo].Keigen = mGrid.g_DArray[maxKinRowNo].Keigen + sagaku;
        //            }
        //            CsumAmt += sagaku;
        //        }
        //        NmalAmt = kin10;
        //        RdueAmt = kin8;
        //    }

        //    hdn_CostAmt.Text = CostAmt.ToString();
        //    hdn_CsumAmt.Text = CsumAmt.ToString();
        //    hdn_ExcAmt.Text = ExcAmt.ToString();
        //    hdn_GrossAmt.Text = GrossAmt.ToString();
        //    hdn_IncAmt.Text = IncAmt.ToString();
        //    hdn_NmalAmt.Text = NmalAmt.ToString();
        //    hdn_RduAmt.Text = RdueAmt.ToString();
        //}
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
            //    Grid_NotFocus((int)ClsGridMasterTanzi.ColNO.SCJAN, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }
        protected override void EndSec()
        {
            try
            {

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();

        }
        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            // Processing when the Enter key is pressed
            // Check if the Return key is pressed
            // Alt or Ctrl key is pressed, do the original operation
            try
            {
                if ((e.KeyCode == Keys.Return) && ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
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
                //EndSec();
            }
            //  throw new NotImplementedException();
        }

        private void ChangeFunKeys()
        {
            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridMasterTanzi.ColNO.Chk);
                detailControls[(int)Eindex.SCTenzikai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                detailControls[(int)Eindex.SCTenzikai].Focus();
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                //画面へデータセット後、明細部入力可
                Scr_Lock(2, 3, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                detailControls[(int)Eindex.SCTenzikai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = false;

            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                previousCtrl.Focus();
            }

        }
        private bool CheckDetail(int index, bool set = true)
        {

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }
            MasterTouroku_TenzikaiShouhin_BL tbl = new MasterTouroku_TenzikaiShouhin_BL();
            Base_BL bl = new Base_BL();
            switch (index)
            {
                case (int)Eindex.SCTenzikai:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");

                        detailControls[index].Focus();
                        return false;
                    }
                    //if (!CheckDependsOnDate(index))
                    //    return false;


                    break;


                case (int)Eindex.SCShiiresaki:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        // ClearCustomerInfo(2);
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
                    SC_Vendor.ChangeDate = bbl.GetDate();

                    var resul1 = SC_Vendor.SelectData();
                    if (!resul1)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    //(detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mve.VendorName;
                    //if (!CheckDependsOnDate(index))
                    //return false;

                    break;
                case (int)Eindex.Nendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.Season:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCBrand:    //
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Brand_Entity mbe = new M_Brand_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        BrandCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    SC_Brand.ChangeDate = bbl.GetDate();

                    var bres = SC_Brand.SelectData();
                    if (!bres)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    // (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mbe.BrandName;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;
                case (int)Eindex.SCSegment:    // ShiireSaki
                    //Entry required
                    Base_BL blb = new Base_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_MultiPorpose_Entity mpe = new M_MultiPorpose_Entity()
                    {
                        ChangeDate = blb.GetDate(),
                        ID = "226",
                        Key = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Base_BL bl1 = new Base_BL();
                    SC_Segment.ChangeDate = blb.GetDate();
                    SC_Segment.Value1 = "226";
                    var seres = SC_Segment.SelectData();
                    if (!seres)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    // (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mpe.Char1;
                    //if (!CheckDependsOnDate(index))
                    //    return false;C

                    break;
                case (int)Eindex.SCCShiiresaki:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.CNendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.CSeason:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)Eindex.SCCBrand:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCCSegment:    // ShiireSaki
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Brand_Entity mcb = new M_Brand_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        BrandCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    SC_Brand.ChangeDate = bbl.GetDate();

                    var cbr = SC_Brand.SelectData();
                    if (!cbr)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    // (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mbe.BrandName;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;

            }

            return true;
        }
        private bool CheckKey(int index, bool set = true)
        {
            if (index == -1)
            {
                var Con = new Control[] {
                    ((CKM_TextBox)detailControls[(int)Eindex.SCTenzikai]),
                    ((CKM_TextBox)detailControls[(int)Eindex.SCShiiresaki]),
                    (CKM_ComboBox)detailControls[(int)Eindex.Nendo],
                    (CKM_ComboBox)detailControls[(int)Eindex.Season],
                    //(CKM_TextBox)detailControls[(int)Eindex.SCCShiiresaki],
                    //(CKM_ComboBox)detailControls[(int)Eindex.CNendo],
                    //(CKM_ComboBox)detailControls[(int)Eindex.CSeason],
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
            if (index == 1)
            {

                if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                {
                    if (!RequireCheck(new Control[] { SC_CopyVendor.TxtCode, CB_Copyyear, CB_copyseason }))  // go that focus
                        return false;
                }
            }

            return true;
        }
        private bool ErrorCheck()
        {

            return true;
        }
        protected void C_Enter(object sender, EventArgs e)
        {
            //if ((sender as Control) is Panel p)
            //{

            //    if (p.Name == "pnl_kokyakuu")
            //    {
            //        if (kr_1.Checked)
            //            kr_1.Focus();
            //        else
            //            kr_2.Focus();
            //    }
            //    else if (p.Name == "pnl_haisou")
            //    {
            //        if (hr_3.Checked)
            //            hr_3.Focus();
            //        else
            //            hr_4.Focus();

            //    }
            //}
            //previousCtrl = this.ActiveControl;

            //SetFuncKeyAll(this, "111111001001");
            //if (ActiveControl.Name == "sc_Tenji")
            //{
            //    previousCtrl = this.ActiveControl;
            //    if (OperationMode == EOperationMode.SHOW)
            //        SetFuncKeyAll(this, "111111001000");

            //}
            //else
            //{
            //    previousCtrl = null;
            //}
        }

        private void MesaiHyouJi(DataTable dt)
        {
            SetTenjiGrid(dt, true);
        }
        private void SetTenjiGrid(DataTable dt, bool IsShow = false)
        {
            SetMultiColNo(dt);
            if (IsShow)
                S_BodySeigyo(1, 1);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
            //DisablePanel(PanelHeader);
           // chk_1.Focus();
        }
        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                }
            }
            else  // set Data from db
            {
                int c = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    mGrid.g_DArray[c].JANCD = dr["JANCD"].ToString();
                    mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].SKUName = dr["SKUName"].ToString();
                    mGrid.g_DArray[c].ColorCD = dr["ColorNo"].ToString();
                    mGrid.g_DArray[c].ColorName = dr["ColorName"].ToString();
                    mGrid.g_DArray[c].SizeCD = dr["SizeNO"].ToString();
                    mGrid.g_DArray[c].SizeName = dr["SizeName"].ToString();
                    mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["HanbaiYoteiDateMonth"].ToString();
                    (mGrid.g_DArray[c].HanbaiYoteiBi) = dr["HanbaiYoteiDate"].ToString();
                    //CheckGrid((int)ClsGridMasterTanzi.ColNO.JANCD, c, true);
                    mGrid.g_DArray[c].Shiiretanka = dr["SiireTanka"].ToString();//
                    mGrid.g_DArray[c].JoutaiTanka = dr["JoudaiTanka"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax = dr["SalePriceOutTax"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax1 = dr["TankKa1"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax2 = dr["TankKa2"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax3 = dr["TankKa3"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax4 = dr["TankKa4"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax5 = dr["TankKa5"].ToString();//
                    mGrid.g_DArray[c].BrandCD = dr["BrandCD"].ToString();//
                    mGrid.g_DArray[c].SegmentCD = dr["SegmentCD"].ToString();
                    mGrid.g_DArray[c].TaniCD = dr["TaniCD"].ToString();//
                    mGrid.g_DArray[c].TaxRateFlg = dr["TaxRateFlg"].ToString();//
                    mGrid.g_DArray[c].Remarks = dr["Remarks"].ToString();//
                    c++;
                }
            }
        }
        private void Scr_Lock(short no1, short no2, short Kbn)
        {
            short i;
            for (i = no1; i <= no2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                           

                            ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = Kbn == 0 ? true : false;
                            break;
                        }
                    case 1:
                        {
                            // ｷｰ部(複写)
                            break;
                        }

                    case 2:
                        {
                            break;
                        }

                    case 3:
                        {
                            
                            break;
                        }
                }
            }
        }
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;
                                if (OperationMode != EOperationMode.SHOW && OperationMode != EOperationMode.DELETE)
                                    mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;

                                if (OperationMode == EOperationMode.UPDATE)
                                    mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Chk, w_Row].Cell_Enabled = true;
                            }

                        }
                        else
                        {
                            if (OperationMode == EOperationMode.INSERT || OperationMode== EOperationMode.DELETE || OperationMode ==EOperationMode.SHOW
                                )
                            {
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                SetFuncKeyAll(this, "111111001001");
                            }
                            else if(OperationMode  == EOperationMode.UPDATE)
                            {
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                                Scr_Lock(1, mc_L_END, 1);   
                                this.Vsb_Mei_0.TabStop = false;
                                SetFuncKeyAll(this, "111111111110");
                            }
                            else 
                            {
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                this.Vsb_Mei_0.TabStop = false;
                                this.Vsb_Mei_0.TabStop = false;
                                SetFuncKeyAll(this, "111111001010");
                            }

                            SetEnabled(false);

                        }
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
                                if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JANCD))
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        {
                                            case (int)ClsGridMasterTanzi.ColNO.JANCD:
                                            case (int)ClsGridMasterTanzi.ColNO.SKUCD:
                                            case (int)ClsGridMasterTanzi.ColNO.SKUName:
                                            //case (int)ClsGridTenjikai.ColNO.Color:
                                            //case (int)ClsGridTenjikai.ColNO.ColorName:
                                            //case (int)ClsGridTenjikai.ColNO.Size:
                                            //case (int)ClsGridTenjikai.ColNO.SizeName:
                                           
                                            case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                                            case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                                            case (int)ClsGridMasterTanzi.ColNO.Chk:
                                              
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
                            SetFuncKeyAll(this, "111111000001");
                        }
                        break;
                    }

                case 2:
                    {
                        if (pGrid == 1)
                        {
                            //panel2.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //   Scr_Lock(0, 0, 0);


                            if (OperationMode == EOperationMode.DELETE)
                            {
                                // Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
                                if (OperationMode != EOperationMode.DELETE)
                                    Scr_Lock(0, 3, 1);
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
                case 4:
                    if (pGrid == 1)
                    {
                        // 入力可の列の設定
                        for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = false;
                        }

                    }
                    else if (pGrid == 0)
                    {
                        for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            if (OperationMode != EOperationMode.UPDATE) mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                        }
                    }
                    Vsb_Mei_0.TabStop = true;
                    break;
                default:
                    {
                        break;
                    }
            }

        }
        private void SetEnabled(bool enabled)
        {
            //detailControls[(int)Eindex.KJuShou1].Enabled = enabled;
            //detailControls[(int)Eindex.KJuShou2].Enabled = enabled;

            //detailControls[(int)Eindex.HJuShou1].Enabled = enabled;
            //detailControls[(int)Eindex.HJuShou2].Enabled = enabled;
            //if (enabled)
            //{

            //}
            //else
            //{
            //    detailControls[(int)Eindex.KJuShou1].Text = "";
            //    detailControls[(int)Eindex.KJuShou2].Text = "";
            //    detailControls[(int)Eindex.HJuShou1].Text = "";
            //    detailControls[(int)Eindex.HJuShou2].Text = "";
            //}
        }
        private void sizecd_5_TextChanged(object sender, EventArgs e)
        {

        }
        private void SC_Tenzikai_Load(object sender, EventArgs e)
        {

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

                //if (w_Col == (int)ClsGridMasterTanzi.ColNO.ChoukuSou)
                //{
                //    bool Check = mGrid.g_DArray[w_Row].ChoukuSou;

                //    if (w_Row == 0 && Check)
                //        mGrid.g_DArray[w_Row].ChoukuSou = false;

                //    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SCJAN))
                //        mGrid.g_DArray[w_Row].ChoukuSou = false;
                //}
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


            if (pCol == (int)ClsGridMasterTanzi.ColNO.JANCD)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JANCD))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridMasterTanzi.ColNO.JANCD)
                            //JANCD使用可
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                        else
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                    }

                    w_AllFlg = false;
                }
                else
                {
                    //JANCD入力時
                    w_AllFlg = true;

                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        switch (w_Col)
                        {
                            case (int)ClsGridMasterTanzi.ColNO.JANCD:
                                if (mGrid.g_DArray[pRow].GYONO == "0")
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;

                            //case (int)ClsGridMasterTanzi.ColNO.SKUCD:
                            //case (int)ClsGridMasterTanzi.ColNO.SKUName:
                            //case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                            //case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                            //case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:
                            //    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                            //    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                            //    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                            //    break;

                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth:
                            case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                            case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                            case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            case (int)ClsGridMasterTanzi.ColNO.TaxRateFlg:
                            case (int)ClsGridMasterTanzi.ColNO.Remarks:

                                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;

                                break;


                        }

                    }
                    w_AllFlg = false;

                }
            }

        }
        private void BT_meisai_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                  DataTable  dtExcel = new DataTable();
                    dtExcel = ReadExcel(filePath, fileExt);
                   if(dtExcel != null)
                   {
                        MesaiHyouJi(dtExcel);
                        S_BodySeigyo(4, 0);
                   }
                    else
                    {
                        tbl.ShowMessage("E128");
                    }
                }
            }
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
                    w_Dest = (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt;

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
                if (w_Dest == (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                {
                    if (mGrid.g_WheelFLG == true)
                    {
                        // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
                        // 最後のフォーカス可能セルに移動
                        if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
                            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                    }
                    else
                        // その行の先頭から探す
                        mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
                }
            }

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            // panel2.Refresh();
        }
        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            //Provider = Microsoft.ACE.OLEDB.12.0; Data Source = c:\myFolder\myExcel2007file.xlsx;
            //Extended Properties = "Excel 12.0 Xml;HDR=YES;IMEX=1";
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=No;IMEX=1';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    //sheet.Cells.NumberFormat = "@";
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dtexcel;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ckM_TextBox43_TextChanged(object sender, EventArgs e)
        {

        }

        private void BT_SKUCheck_Click(object sender, EventArgs e)
        {
            //int W_CtlRow;  // Grid Properties
            //if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;               // プログラムＭ内最大行数
            //    m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
            //}
            //mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_MaxR;
            //mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;
            //mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            //for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Col - 1; W_CtlRow++)
            //{
            //    //for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
            //    //{
            //        //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
            //        //{
            //            M_SKU_Entity msku = new M_SKU_Entity
            //            {
            //                JanCD = mGrid.g_DArray[W_CtlRow].JANCD,
            //                SKUName = mGrid.g_DArray[W_CtlRow].SKUName,
            //                ColorName = mGrid.g_DArray[W_CtlRow].ColorName,
            //                SizeName = mGrid.g_DArray[W_CtlRow].SizeName,
            //               // ExhibitionCommonCD = mGrid.g_DArray[W_CtlRow].
            //            };
            //            DataTable dt = tbl.M_SKU_SelectForSKUCheck(msku);
            //            if(dt == null)
            //            {
            //                mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.GridColor);
            //            }
            //       // }
            //   // }
            //}


            //for (int w_Row = 0; w_Row < 999; w_Row++)
            //{

            //    if(String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD))
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        M_SKU_Entity msku = new M_SKU_Entity
            //        {
            //            JanCD = mGrid.g_DArray[w_Row].JANCD,
            //            SKUName = mGrid.g_DArray[w_Row].SKUName,
            //            ColorName = mGrid.g_DArray[w_Row].ColorName,
            //            SizeName = mGrid.g_DArray[w_Row].SizeName,
            //            // ExhibitionCommonCD = mGrid.g_DArray[W_CtlRow].
            //        };
            //        DataTable dt = tbl.M_SKU_SelectForSKUCheck(msku);
            //        if (dt != null)
            //        {
            //            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Chk, w_Row].Cell_Enabled = false;
            //            S_Clear_Grid();
            //            //mGrid.g_DArray[w_Row].Chk.;
            //        }
            //    }
            //}
        }
    }
}
