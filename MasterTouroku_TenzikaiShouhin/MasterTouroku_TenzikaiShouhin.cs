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
            Vendor
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
            StartProgram();
            this.InitialControlArray();
            SC_Segment.Value1 = "226";
            string ymd = bl.GetDate();

            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_copyseason.Bind(ymd);
            CB_Copyyear.Bind(ymd);
            BindCombo_Details();
            this.S_SetInit_Grid();
            Scr_Clr(0);
        }

        private void BindCombo_Details()
        {
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                //CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ShuukaSou, W_CtlRow].CellCtl;
                //sctl.Cbo_Type = CKM_ComboBox.CboType.出荷倉庫;
                //sctl.Bind(bbl.GetDate());
            }
        }
        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);

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
           // S_Clear_Grid();   //画面クリア（明細部）


            //   mOrderTaxTiming = 0;
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
                if (c is CKM_ComboBox cb && cb.Name == "cbo_Shuuka")
                {
                    // cb.SelectedIndexChanged += ShuukaSouko_SelectedIndexChanged;
                }
            }

           
        }
        //private void S_SetInit_Grid()
        //{
        //    int W_CtlRow;

        //    if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
        //    {
        //        mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;
        //        m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
        //    }


        //    mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_P_GYO;
        //    mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;

        //    mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;



        //    S_SetControlArray();

        //    for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
        //    {
        //        for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
        //        {
        //            if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
        //            {
        //                if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
        //                {
        //                    Search.CKM_SearchControl sctl = (Search.CKM_SearchControl)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
        //                    sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
        //                    sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
        //                    sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
        //                    sctl.TxtCode.Tag = W_CtlRow.ToString();
        //                    sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
        //                }
        //                else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
        //                {
        //                    CKM_Controls.CKM_TextBox sctl = (CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
        //                    sctl.Tag = W_CtlRow.ToString();
        //                    mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
        //                    mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
        //                    mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
        //                    mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.EnabledChanged += new EventHandler(GridControl_EnableChnaged);
        //                }
        //                else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
        //                {
        //                    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
        //                    sctl.Enter += new System.EventHandler(GridControl_Enter);
        //                    sctl.Leave += new System.EventHandler(GridControl_Leave);
        //                    sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
        //                    sctl.Tag = W_CtlRow.ToString();
        //                }
        //                else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
        //                {
        //                    Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

        //                    //if (w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
        //                    //    btn.Click += new System.EventHandler(BTN_Site_Click);
        //                    //else
        //                    //    btn.Click += new System.EventHandler(BTN_Zaiko_Click);
        //                }
        //                else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
        //                {
        //                    GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
        //                    check.Tag = W_CtlRow.ToString();
        //                    check.Enter += new System.EventHandler(GridControl_Enter);
        //                    check.Leave += new System.EventHandler(GridControl_Leave);
        //                    check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
        //                    check.Click += new System.EventHandler(CHK_Print_Click);
        //                }
        //            }
        //        }
        //    }  // handler Created

        //    // 明細部の状態(初期状態) をセット 
        //    mGrid.g_MK_State = new ClsGridMasterTanzi.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row]; // Cell Format


        //    // データ保持用配列の宣言
        //    mGrid.g_DArray = new ClsGridMasterTanzi.ST_DArray_Grid[mGrid.g_MK_Max_Row];  // Stored Array
        //    SetMultiColNo();
        //    // 行の色
        //    // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
        //    mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.WHColor);//, "0");        // 1行目(奇数行) 白
        //    mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.GridColor);//, "1");      // 2行目(偶数行) 水色

        //    // フォーカス移動順(表示列も含めて、すべての列を設定する)
        //    mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

        //    for (int i = (int)ClsGridMasterTanzi.ColNO.GYONO; i <= (int)ClsGridMasterTanzi.ColNO.Count - 1; i++)
        //    {
        //        mGrid.g_MK_FocusOrder[i] = i;
        //    }

        //    int tabindex = 1;

        //    // 項目の形式セット
        //    for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
        //    {
        //        for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridMasterTanzi.ColNO.Count; W_CtlCol++)
        //        {
        //            switch (W_CtlCol)
        //            {
        //                //case (int)ClsGridMasterTanzi.ColNO.JuchuuSuu:
        //                //case (int)ClsGridMasterTanzi.ColNO.HanbaiTanka:
        //                //case (int)ClsGridMasterTanzi.ColNO.HacchuTanka:
        //                //case (int)ClsGridMasterTanzi.ColNO.ZeinuJuchuu:
        //                //case (int)ClsGridMasterTanzi.ColNO.zeikomijuchuu:
        //                //case (int)ClsGridMasterTanzi.ColNO.ArariGaku:

        //                ////  mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
        //                //((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;//ok
        //                //((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Price; //Ok
        //                //((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).IntegerPart = 8; // ok
        //                //((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).DecimalPlace = 0;
        //                //break;

        //                //case (int)ClsGridMasterTanzi.ColNO.ShuukaYo:
        //                //case (int)ClsGridMasterTanzi.ColNO.NyuuKayo:
        //                //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
        //                //    break;
        //                //case (int)ClsGridMasterTanzi.ColNO.SCJAN:
        //                //    // mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 JANCD_Detail
        //                //    ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
        //                //    ((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.JANCD_Detail;
        //                //    break;
        //            }
        //            //switch (W_CtlCol)
        //            //{
        //            //    case (int)ClsGridMasterTanzi.ColNO.ZeinuJuchuu:
        //            //    case (int)ClsGridMasterTanzi.ColNO.zeikomijuchuu:
        //            //    case (int)ClsGridMasterTanzi.ColNO.ArariGaku:
        //            //    case (int)ClsGridMasterTanzi.ColNO.ZeiNu:
        //            //    case (int)ClsGridMasterTanzi.ColNO.ZeinuTanku:
        //            //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Back_Color = CKM_TextBox.CKM_Color.DarkGrey;
        //            //        break;
        //            //}

        //            // mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
        //            tabindex++;
        //        }
        //    }


        //    Set_GridTabStop(false);

        //}

        private void S_SetInit_Grid()
        {
            int W_CtlRow;  // Grid Properties

            if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
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
            }  // handler Created

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridMasterTanzi.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row]; // Cell Format


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridMasterTanzi.ST_DArray_Grid[mGrid.g_MK_Max_Row];  // Stored Array
            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
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
                        case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.JANCD:
                        case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                        case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                        case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            // mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 JANCD_Detail
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
                            ((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.JANCD_Detail;
                            break;
                    }
                    //switch (W_CtlCol)
                    //{
                    //    //case (int)ClsGridMasterTanzi.ColNO.ZeinuJuchuu:
                    //    //case (int)ClsGridMasterTanzi.ColNO.zeikomijuchuu:
                    //    //case (int)ClsGridMasterTanzi.ColNO.ArariGaku:
                    //    //case (int)ClsGridMasterTanzi.ColNO.ZeiNu:
                    //    //case (int)ClsGridMasterTanzi.ColNO.ZeinuTanku:
                    //    //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Back_Color = CKM_TextBox.CKM_Color.DarkGrey;
                    //        break;
                    //}

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            // TabStopﾌﾟﾛﾊﾟﾃｨの初期状態をセット
            Set_GridTabStop(false);
            // 明細部クリア
            //S_Clear_Grid();   Scr_Clr処理で
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
                            CL = (int)ClsGridMasterTanzi.ColNO.JANCD;
                            break;
                        case "sku":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUName;
                            break;
                        case "shouhin":

                            CL = (int)ClsGridMasterTanzi.ColNO.ShouhinName;
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
                        //case "IMN_ORGAK":
                        //    CL = (int)ClsGridJuchuu.ColNO.OrderGaku;
                        //    break;
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


                    //チェック処理

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
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUName, d].CellCtl = this.Controls.Find("sku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ShouhinName, d].CellCtl = this.Controls.Find("shouhin_" + id, true)[0];
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
        private void BT_Display_Click(object sender, EventArgs e)
        {

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

            DataTable dtmain = tbl.Mastertoroku_Tenzikaishouhin_Select(mt, "2");

            MesaiHyouJi(dtmain);
        }

        private void SearchData(EsearchKbn kbn, Control setCtl)
        {

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

                //case (int)Eindex.SCTenzikai:    // ShiireSaki
                //    //Entry required
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        //顧客情報ALLクリア
                //       // ClearCustomerInfo(2);
                //        detailControls[index].Focus();
                //        return false;
                //    }
                //    M_TenzikaiShouhin_Entity mte = new M_TenzikaiShouhin_Entity
                //    {
                //        ChangeDate = bbl.GetDate(),
                //      //  VendorFlg = "1",
                //        JANCD = detailControls[index].Text,
                //        DeleteFlg = "0"
                //    };

                //    SC_Tenzikai.ChangeDate = bbl.GetDate();
                //    var tezires = SC_Tenzikai.SelectData();
                //    if (tezires )
                //    {
                //        bbl.ShowMessage("E101");
                //        // ClearCustomerInfo(2);
                //        return false;
                //    }
                //    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mte.TenzikaiName;
                //   //if (!CheckDe*/pendsOnDate(index))
                //       // return false;

                ////    break;
                case (int)Eindex.SCShiiresaki:    // ShiireSaki
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
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        VendorFlg = "1",
                        VendorCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();

                    // var  resul1 = vbl.M_Vendor_Select_Tenji(mve);
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
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.Season:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCBrand:    // ShiireSaki
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
                    //    return false;

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
                    ((CKM_TextBox)detailControls[(int)Eindex.SCShiiresaki]),
                    (CKM_ComboBox)detailControls[(int)Eindex.Nendo],
                    (CKM_ComboBox)detailControls[(int)Eindex.Season],
                    (CKM_TextBox)detailControls[(int)Eindex.SCBrand],
                    (CKM_TextBox)detailControls[(int)Eindex.SCSegment],
                    (CKM_TextBox)detailControls[(int)Eindex.SCCShiiresaki],
                    (CKM_TextBox)detailControls[(int)Eindex.CNendo],
                    (CKM_ComboBox)detailControls[(int)Eindex.CSeason],
                    (CKM_ComboBox)detailControls[(int)Eindex.SCCBrand],
                     (CKM_ComboBox)detailControls[(int)Eindex.SCCSegment],
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
            if (index == (int)Eindex.SCTenzikai)
            {


                tbl = new MasterTouroku_TenzikaiShouhin_BL();
                //入力必須(Entry required)
                if (string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCTenzikai].Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    return false;
                }
                //var res = tbl.Check_DTenjikaiJuuchuu((detailControls[(int)Eindex.SCTenzikai]).Text);
                //if (res.Rows.Count == 0)
                //{

                //    //Check Tenji
                //    bbl.ShowMessage("E142");
                //    return false;
                //}
                //if (res.Rows[0]["DeleteDateTime"].ToString() != "")
                //{

                //    //Check deleteDateTime
                //    bbl.ShowMessage("E144");
                //    return false;
                //}

                //// E143 // Already Checked in Main M_StoreAuthor:

                //if (res.Select("JuchuuHurikaeZumiFLG = 1") == null)
                {
                    //Check deleteDateTime
                    bbl.ShowMessage("E256");
                    return false;
                }

                //排他処理
                //bool ret = SelectAndInsertExclusive();
                //if (!ret)
                //    return false;

                //return CheckData(set);
            }
            return true;

        }

        protected void C_Enter(object sender, EventArgs e)
        {
            if ((sender as Control) is Panel p)
            {

                if (p.Name == "pnl_kokyakuu")
                {
                    //kr_1.Focus();
                }
                else if (p.Name == "pnl_haisou")
                {
                    // hr_3.Focus();

                }
            }
            //if (ActiveControl is CKM_SearchControl cs && cs.Name == "sc_Tenji")
            //{
            //    previousCtrl = this.ActiveControl;
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
            DisablePanel(PanelHeader);
            chk_1.Focus();
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
                    mGrid.g_DArray[c].JANCD = dr["JANCD"].ToString();
                    mGrid.g_DArray[c].SKUName = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].ShouhinName = dr["SKUName"].ToString();
                    mGrid.g_DArray[c].ColorCD = dr["ColorNo"].ToString();
                    mGrid.g_DArray[c].ColorName = dr["ColorName"].ToString();
                    mGrid.g_DArray[c].SizeCD = dr["SizeNO"].ToString();
                    mGrid.g_DArray[c].SizeName = dr["SizeName"].ToString();
                    mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["HanbaiYoteiDateMonth"].ToString();
                    (mGrid.g_DArray[c].HanbaiYoteiBi) = dr["HanbaiYoteiDate"].ToString();
                    // mGrid.g_DArray[c].Empty = dr["_3SKUName"].ToString();

                    mGrid.g_DArray[c].Shiiretanka = dr["SiireTanka"].ToString();//
                    mGrid.g_DArray[c].JoutaiTanka = dr["JoudaiTanka"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax = dr["SalePriceOutTax"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax1 = dr["TankKa1"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax2 = dr["TankKa2"].ToString();//

                    mGrid.g_DArray[c].SalePriceOutTax3 = dr["TankKa3"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax4 = dr["TankKa4"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax5 = dr["TankKa5"].ToString();//
                    mGrid.g_DArray[c].BrandCD = dr["BrandCD"].ToString();//
                    mGrid.g_DArray[c].SegmentCD = dr["SegmentCD"].ToString();//

                    // mGrid.g_DArray[c].Chk = dr["_3SKUName"].ToString();   
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
                            // ｷｰ部
                            //foreach (Control ctl in keyControls)
                            //{
                            //    ctl.Enabled = Kbn == 0 ? true : false;
                            //}

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
                            // 明細部
                            //for (int idx = 0; idx < (int)Eindex.Count; idx++)
                            //{
                            //    switch (idx)
                            //    {
                            //        case (int)Eindex.KJuShou1:
                            //        case (int)Eindex.KJuShou2:
                            //        case (int)Eindex.HJuShou1:
                            //        case (int)Eindex.HJuShou2:
                            //            break;
                            //        default:
                            //            detailControls[idx].Enabled = Kbn == 0 ? true : false;
                            //            break;
                            //    }
                            //}
                            //for (int index = 0; index < searchButtons.Length; index++)
                            //    searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            //btn_Customer.Enabled = Kbn == 0 ? true : false;
                            //btn_Shipping.Enabled = Kbn == 0 ? true : false;
                            //panel1.Enabled = Kbn == 0 ? true : false;
                            //if (OperationMode == EOperationMode.UPDATE)
                            //    if (Kbn == 0)
                            //    {
                            //        detailControls[(int)Eindex.SCTenjiKai].Enabled = searchButtons[0].Enabled = btn_Meisai.Enabled = Kbn != 0;
                            //        DisablePanel(PanelHeader);
                            //    }
                            //= false;
                            //Btn_SelectAll.Enabled = Kbn == 0 ? true : false;
                            //Btn_NoSelect.Enabled = Kbn == 0 ? true : false;
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

                                // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                if (OperationMode != EOperationMode.SHOW && OperationMode != EOperationMode.DELETE)
                                    mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                            }

                        }
                        else
                        {
                            //IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                               // detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
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
                                            case (int)ClsGridMasterTanzi.ColNO.SKUName:
                                            case (int)ClsGridMasterTanzi.ColNO.ShouhinName:
                                            //case (int)ClsGridTenjikai.ColNO.Color:
                                            //case (int)ClsGridTenjikai.ColNO.ColorName:
                                            //case (int)ClsGridTenjikai.ColNO.Size:
                                            //case (int)ClsGridTenjikai.ColNO.SizeName:
                                            //case (int)ClsGridMasterTanzi.ColNO.ShuukaYo:
                                            //case (int)ClsGridMasterTanzi.ColNO.ChoukuSou:
                                            //case (int)ClsGridMasterTanzi.ColNO.ShuukaSou:
                                            //case (int)ClsGridMasterTanzi.ColNO.HacchuTanka:
                                            //case (int)ClsGridMasterTanzi.ColNO.NyuuKayo:
                                            //case (int)ClsGridMasterTanzi.ColNO.JuchuuSuu:
                                            //case (int)ClsGridMasterTanzi.ColNO.TenI:
                                            case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                                            case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                                            //case (int)ClsGridTenjikai.ColNO.ZeinuJuchuu:
                                            //case (int)ClsGridTenjikai.ColNO.zeikomijuchuu:
                                            //case (int)ClsGridTenjikai.ColNO.ArariGaku:
                                            //case (int)ClsGridTenjikai.ColNO.ZeiNu:
                                            //case (int)ClsGridTenjikai.ColNO.ZeinuTanku:
                                            case (int)ClsGridMasterTanzi.ColNO.Chk:
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
    }    
}
