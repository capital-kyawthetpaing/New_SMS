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
        private int mTaxTiming;
        private int mTaxFractionKBN;
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private string mOldJyuchuNo = "";
        private Control[] detailLabels;
        private Control[] searchButtons;
        private string KibouBi1 = "";
        private string KibouBi2 = "";
        CKM_SearchControl mOldCustomerCD;
        private string KZip1 = "";
        private string KZip2 = "";
        private string KAddress1 = "";
        private string KAddress2 = "";
        private string HZip1 = "";
        private string HZip2 = "";
        private string HAddress1 = "";
        private string HAddress2 = "";
        private TempoJuchuuNyuuryoku_BL mibl;
        TenjikaiJuuChuu_BL tkb;
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private string InOperatorName = "";
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
        private string StoreCD = "";
        public TenzikaiJuchuuTourou()
        {
            InitializeComponent();
            C_dt = DateTime.Now.ToString("yyyy-MM-dd");
            PanelSearch.SendToBack();
            PanelSearch.Visible = true;
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

                Btn_F11.Text = "";
                sc_shiiresaki.TxtCode.Focus();

                SetInitialState();
                //M_Souko_SelectForMitsumori

                //  detailControls[(int)Eindex.JuuChuuBi].Text = ymd;


                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                    ChangeOperationMode(EOperationMode.SHOW);
                }
                Btn_F7.Text =  "行追加(F7)";
                Btn_F8.Text = "行削除(F8)";
                Btn_F10.Text = "行複写(F10)";
                Btn_F1.Enter += Btn_F1_Enter;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void SetInitialState()
        {
            mibl = new TempoJuchuuNyuuryoku_BL();
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = mibl.GetDate()
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                //  CboStoreCD.SelectedValue = mse.StoreCD;
                sc_TentouStaff.LabelText = mse.StaffName;
                sc_TentouStaff.TxtCode.Text = mse.StaffCD;
            }
            InOperatorName = mse.StaffName;
            StoreCD = mse.StoreCD;  //初期値を退避
            M_Souko_Entity msoe = new M_Souko_Entity
            {
                StoreCD = StoreCD,
                ChangeDate = mibl.GetDate(),
                DeleteFlg = "0"
            }
            ;
            var dt = mibl.M_Souko_SelectForMitsumori(msoe);
            if (dt.Rows.Count > 0)
                cbo_Shuuka.SelectedValue = dt.Rows[0]["SoukoCD"].ToString();
        }
        private void Btn_F1_Enter(object sender, EventArgs e)
        {
           // previousCtrl = this.ActiveControl;
            //if (OperationMode == EOperationMode.SHOW)
            //    SetFuncKeyAll(this, "111111000000");
            //else
            //SetFuncKeyAll(this, "11111100001");
        }

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            ////配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:

                    //   detailControls[(int)Eindex.JuuChuuBi].Focus();
                    sc_Tenji.Enabled = false;
                    sc_Tenji.BtnSearch.Enabled = false;
                    detailControls[(int)Eindex.SCShiiresaki].Focus();
                    btn_Meisai.Enabled = true;
                   
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    EnablePanel(PanelHeader);

                    sc_Tenji.Enabled =sc_Tenji.TxtCode.Enabled= true;
                    sc_Tenji.BtnSearch.Enabled = true;
                    sc_Tenji.TxtCode.Focus();
                    //if (mode == EOperationMode.SHOW)
                    //{
                    //    F12Enable = false;
                    //}
                    //else
                    //    F12Enable = true;
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
            int W_CtlRow;  // Grid Properties

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
            mGrid.g_MK_State = new ClsGridTenjikai.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row]; // Cell Format


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridTenjikai.ST_DArray_Grid[mGrid.g_MK_Max_Row];  // Stored Array
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
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).IntegerPart = 9; // ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).DecimalPlace = 0;
                            break;

                        case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                        case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Date;
                            break;
                        case (int)ClsGridTenjikai.ColNO.SCJAN:
                            // mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 JANCD_Detail
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
                            ((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.JANCD_Detail;
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
                        case (int)ClsGridTenjikai.ColNO.ShagaiBi:
                        case (int)ClsGridTenjikai.ColNO.ShanaiBi:
                        case (int)ClsGridTenjikai.ColNO.KobeTsu:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).MaxLength = 80;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type =  CKM_TextBox.Type.Normal;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Byte =  CKM_TextBox.Bytes.半全角;
                            break;
                        case (int)ClsGridTenjikai.ColNO.ShouName:
                           // ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).MaxLength = 80;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).TextAlign = HorizontalAlignment.Left;
                            break;
                        case (int)ClsGridTenjikai.ColNO.SKUCD:
                            //   (Label)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl). = 30;
                            ((Label)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).TextAlign = ContentAlignment.MiddleLeft;
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

                    //if (w_Row == 0 && Check)
                    //    mGrid.g_DArray[w_Row].ChoukuSou = false;

                    //if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SCJAN))
                    //    mGrid.g_DArray[w_Row].ChoukuSou = false;
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
                           mGrid.g_DArray[w_Row].OldJanCD = frmProduct.JANCD;
                           mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
                           mGrid.g_DArray[w_Row].AdminNo = frmProduct.AdminNO;

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
                        case "shanaibikou":
                            CL = (int)ClsGridTenjikai.ColNO.ShanaiBi;
                            break;

                        case "shagaibikou":
                            CL = (int)ClsGridTenjikai.ColNO.ShagaiBi;
                            break;
                        case "kobetsuhanbai":
                            CL = (int)ClsGridTenjikai.ColNO.KobeTsu;
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
                mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                mGrid.g_DArray[row].Size = selectRow["SizeNo"].ToString();
                mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                mGrid.g_DArray[row].AdminNo = selectRow["AdminNO"].ToString();
                mGrid.g_DArray[row].HacchuTanka = bbl.Z_SetStr(selectRow["ShiireTanka"].ToString());
                mGrid.g_DArray[row].HanbaiTanka = bbl.Z_SetStr(selectRow["SalePriceOutTax"].ToString());
                // mGrid.g_DArray[row].TenI = selectRow["TaniCD"].ToString(); // Name
                mGrid.g_DArray[row].TaniHDN = selectRow["TaniCD"].ToString();
                mGrid.g_DArray[row].TenI = tkb.D_TeniSelectbyTaniCD(mGrid.g_DArray[row].TaniHDN);
                mGrid.g_DArray[row].TeniName = selectRow["TaniName"].ToString();
                mGrid.g_DArray[row].TaxRateFlg = Convert.ToInt16(selectRow["TaxRateFlg"].ToString()).ToString();
            }
            else
                return false;
            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false,bool IsExec =false)
        {
            bool ret = false;

            string ymd = detailControls[(int)Eindex.JuuChuuBi].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)  // check length
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridTenjikai.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }

            }
            //if (chkAll)

            if (IsExec && !mGrid.g_DArray[row].Chk)
            {
                IsExec = false;
                return true;
            }
            switch (col)
            {
                case (int)ClsGridTenjikai.ColNO.SCJAN:
                    if (!changeYmd)
                    {
                        if (mGrid.g_DArray[row].SCJAN == mGrid.g_DArray[row].OldJanCD)      //chkAll &&  change/ no need to check if unchanged
                            return true;
                    }
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SCJAN))
                    {
                        Grid_Gyo_Clr(row);
                       // return true;
                    }
                  
                    if (IsExec ? !mGrid.g_DArray[row].Chk : false || string.IsNullOrEmpty(mGrid.g_DArray[row].SCJAN))    // Neglect uncheck / Null or string JanCd
                    {
                        IsExec = false;
                        return true;
                    }
                    //if (!chkAll)
                    //{
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].SCJAN,
                        ChangeDate = ymd
                    };
                    if (mGrid.g_DArray[row].SCJAN == mGrid.g_DArray[row].OldJanCD || string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OldJanCD))
                    {
                        mse.SKUCD = mGrid.g_DArray[row].SKUCD;
                        mse.AdminNO = mGrid.g_DArray[row].AdminNo;
                    }

                    if (mGrid.g_DArray[row].SCJAN != mGrid.g_DArray[row].OldJanCD)
                    {
                        //JANCD変更時は単価再計算するように
                        mGrid.g_DArray[row].NotReCalc = false;
                    }
                    //else
                    //    mGrid.g_DArray[row].NotReCalc = false;
                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        var val = GetTenji(row);
                        if (!val)
                        {
                            //Ｅ１０7
                            bbl.ShowMessage("E101");
                            return false;
                        }
                        else
                             if (mGrid.g_DArray[row].NotReCalc != true)
                            CalcZei(row, col);
                    }
                    else if (dt.Rows.Count == 1)
                    {
                        selectRow = dt.Rows[0];
                    }
                    else
                    {
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

                        ((CKM_SearchControl)sc_shiiresaki).Refresh();
                        panel1.Refresh();
                    }

                    if (selectRow != null)
                    {
                        mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                        mGrid.g_DArray[row].ShouName = selectRow["SKUName"].ToString();
                        mGrid.g_DArray[row].Color = selectRow["ColorNO"].ToString();
                        mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
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
                        if (mGrid.g_DArray[row].NotReCalc != true) // false > to recalculate on JanCD Change
                        {
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
                            CalcZei(row, col);
                        }
                        mGrid.g_DArray[row].TaniHDN = selectRow["TaniCD"].ToString();
                        mGrid.g_DArray[row].TenI = tkb.D_TeniSelectbyTaniCD(mGrid.g_DArray[row].TaniHDN);
                        mGrid.g_DArray[row].TeniName = selectRow["TaniName"].ToString();
                        //TaxRate
                        mGrid.g_DArray[row].TaxRateFlg = Convert.ToInt16(selectRow["TaxRateFLG"].ToString()).ToString();

                    }
                    mGrid.g_DArray[row].OldJanCD = mGrid.g_DArray[row].SCJAN;    // May be in M_Tenji Table So Fetched from selectRow

                    //  CalcZei(row, (int)ClsGridTenjikai.ColNO.SCJAN);
                    Grid_NotFocus(col, row);

                    //}
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
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
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
                    mibl = new TempoJuchuuNyuuryoku_BL();
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
                       //mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ChoukuSou, row].Cell_ReadOnly = false;
                       // mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ChoukuSou, row].Cell_Enabled = true;
                       // mGrid.g_DArray[row].ChoukuSou = false;
                        //
                    }
                    else
                    {
                        //mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ChoukuSou, row].Cell_ReadOnly = false;
                        //mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ChoukuSou, row].Cell_Enabled = true;
                        //mGrid.g_DArray[row].ChoukuSou = true;

                    }
                  //  mGrid.S_DispFromArray(Vsb_Mei_0.Value,ref Vsb_Mei_0);

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
                    if (!chkAll)
                        if (orderUnitPrice.Equals(0))
                        {
                            if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                return false;
                        }
                    //０で無いかつ原価単価＝０の場合場合、入力された発注単価を原価単価にセットし、原価金額、粗利金額を再計算。
                    var val1 = bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu) - ((bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka)) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu));
                    mGrid.g_DArray[row].ArariGaku = bbl.Z_SetStr(val1);
                    break;

            }

            switch (col)
            {
                case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                    decimal JuchuuSuu = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
                    mGrid.g_DArray[row].JuchuuSuu = bbl.Z_SetStr(JuchuuSuu);

                    //０の場合				メッセージ表示
                    if (!chkAll)
                        if (JuchuuSuu.Equals(0))
                        {
                            if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                return false;
                        }
                    CalcZei(row, col);
                    break;
                case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
                    decimal HanbaiSuu = bbl.Z_Set(mGrid.g_DArray[row].HanbaiTanka);
                    mGrid.g_DArray[row].HanbaiTanka = bbl.Z_SetStr(HanbaiSuu);

                    //０の場合				メッセージ表示
                    if (!chkAll)
                        if (HanbaiSuu.Equals(0))
                        {
                            if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                return false;
                        }
                    CalcZei(row, col);
                    break;
                //case (int)ClsGridTenjikai.ColNO.SCJAN:
                case (int)ClsGridTenjikai.ColNO.HacchuTanka:
                    //CalcZei(row, col);
                    break;

            }

            if (chkAll == false)
                CalcKin();
            // CalcKin(row, col);
            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        private void CalcKin()
        {
            decimal ExcAmt = 0;
            decimal IncAmt = 0;
            decimal CostAmt = 0;
            decimal GrossAmt = 0;
            decimal CsumAmt = 0;
            decimal NmalAmt = 0;
            decimal RdueAmt = 0;

            decimal kin1 = 0;
            decimal kin2 = 0;
            decimal kin3 = 0;
            decimal kin4 = 0;
            decimal kin5 = 0;
            decimal zei10 = 0;  //tsuujou
            decimal zei8 = 0;  // keijen

            decimal kin10 = 0;
            decimal kin8 = 0;
            int zeiritsu10 = 0;
            int zeiritsu8 = 0;
            int maxKinRowNo = 0;
            decimal maxKin = 0;

            decimal kinOrder10 = 0;      //発注通常税額(Hidden)
            decimal kinOrder8 = 0;      //発注軽減税額(Hidden)

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                ///if (mGrid.g_DArray[RW].Chk)
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SCJAN) && mGrid.g_DArray[RW].Chk)
                {
                    ExcAmt += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu); // 1
                    IncAmt += bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu); //2
                    CostAmt += bbl.Z_Set(mGrid.g_DArray[RW].HacchuTanka) * bbl.Z_Set(mGrid.g_DArray[RW].JuchuuSuu);

                    zei10 += bbl.Z_Set(mGrid.g_DArray[RW].Tsuujou);
                    zei8 += bbl.Z_Set(mGrid.g_DArray[RW].Keigen);

                    if (mTaxTiming.Equals(2))
                    {
                        if (mGrid.g_DArray[RW].TaxRateFlg.Equals("1"))
                        {
                            kin10 += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu);
                            if (zeiritsu10 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ZeinuTanku))
                                zeiritsu10 = Convert.ToInt16(mGrid.g_DArray[RW].ZeinuTanku.Trim().Replace("%", "").Replace("％","").Replace(".0", ""));
                        }
                        else if (mGrid.g_DArray[RW].TaxRateFlg.Equals("2"))
                        {
                            kin8 += bbl.Z_Set(mGrid.g_DArray[RW].ZeinuJuchuu);
                            if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ZeinuTanku))
                                zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].ZeinuTanku.Trim().Replace("%", "").Replace("％", "").Replace(".0", ""));
                        }

                        if (maxKin < bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu))
                        {
                            maxKin = bbl.Z_Set(mGrid.g_DArray[RW].zeikomijuchuu);
                            maxKinRowNo = RW;
                        }
                    }
                }
            }

            GrossAmt = ExcAmt - CostAmt;

            if (Convert.ToInt32(mTaxTiming) == 1 || Convert.ToInt32(mTaxTiming) == 3)
            {
                CsumAmt = zei10 + zei8;
                NmalAmt = bbl.Z_Set(zei10);
                RdueAmt = bbl.Z_Set(zei8);
            }
            else if (Convert.ToInt32(mTaxTiming) == 2)
            {
                kin10 = GetResultWithHasuKbn(mTaxFractionKBN, kin10 * zeiritsu10 / 100);
                kin8 = GetResultWithHasuKbn(mTaxFractionKBN, kin8 * zeiritsu8 / 100);

                decimal sagaku = (kin10 + kin8) - (zei10 + zei8);
                CsumAmt = kin10 + kin8;
                if (sagaku != 0)
                {
                    mGrid.g_DArray[maxKinRowNo].zeikomijuchuu = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[maxKinRowNo].zeikomijuchuu) + sagaku);  // Add percent Error Amount in the Max row or first line if same 
                    IncAmt += sagaku;  //Add percent Error Amount
                    if (mGrid.g_DArray[maxKinRowNo].TaxRateFlg.Equals("1"))
                    {
                        mGrid.g_DArray[maxKinRowNo].Tsuujou = mGrid.g_DArray[maxKinRowNo].Tsuujou + sagaku; // Add percent Error In most biggest
                    }
                    else if (mGrid.g_DArray[maxKinRowNo].TaxRateFlg.Equals("2"))
                    {
                        mGrid.g_DArray[maxKinRowNo].Keigen = mGrid.g_DArray[maxKinRowNo].Keigen + sagaku;// Add percent Error In most biggest
                    }
                    CsumAmt += sagaku;
                }
                NmalAmt = kin10;
                RdueAmt = kin8;
            }

            hdn_CostAmt.Text = CostAmt.ToString();
            hdn_CsumAmt.Text = CsumAmt.ToString();
            hdn_ExcAmt.Text = ExcAmt.ToString();
            hdn_GrossAmt.Text = GrossAmt.ToString();
            hdn_IncAmt.Text = IncAmt.ToString();
            hdn_NmalAmt.Text = NmalAmt.ToString();
            hdn_RduAmt.Text = RdueAmt.ToString();
        }
        private void CalcZei(int row, int col, bool IsHanBai = false)
        {
           
            if (col == (int)ClsGridTenjikai.ColNO.HanbaiTanka)
            {
                IsHanBai = true;
            }
            var hanbai = bbl.Z_Set(mGrid.g_DArray[row].HanbaiTanka);
            var juchuu = bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
           
            var taxflg = bbl.Z_SetStr(mGrid.g_DArray[row].TaxRateFlg);
            tkb = new TenjikaiJuuChuu_BL();
            var res = tkb.GetTaxRate(taxflg, detailControls[(int)Eindex.JuuChuuBi].Text.Replace("/", "-"));
            if (taxflg == "1")
            {
                mGrid.g_DArray[row].ZeiNu = "税抜";
                mGrid.g_DArray[row].ZeinuJuchuu = bbl.Z_SetStr(hanbai * juchuu); 
                mGrid.g_DArray[row].zeikomijuchuu = bbl.Z_SetStr(GetResultWithHasuKbn(Convert.ToInt32(res.Rows[0]["KBN"].ToString()), Convert.ToDecimal(mGrid.g_DArray[row].ZeinuJuchuu) * (bbl.Z_Set(res.Rows[0]["TaxRate"].ToString()) + 100) / 100).ToString());
                mGrid.g_DArray[row].ZeinuTanku = bbl.Z_SetStr(res.Rows[0]["TaxRate"].ToString()) + "%";

                mGrid.g_DArray[row].Tsuujou = bbl.Z_Set(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) - bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu));  
                mGrid.g_DArray[row].Keigen = 0; 
            }
            else if (taxflg == "2")
            {
                mGrid.g_DArray[row].ZeiNu = "税抜";
                mGrid.g_DArray[row].ZeinuJuchuu = bbl.Z_SetStr(hanbai * juchuu); 
                mGrid.g_DArray[row].zeikomijuchuu = bbl.Z_SetStr(GetResultWithHasuKbn(Convert.ToInt32(res.Rows[0]["KBN"].ToString()), Convert.ToDecimal(mGrid.g_DArray[row].ZeinuJuchuu) * (bbl.Z_Set(res.Rows[0]["TaxRate"].ToString()) + 100) / 100).ToString());
                mGrid.g_DArray[row].ZeinuTanku = bbl.Z_SetStr(res.Rows[0]["TaxRate"].ToString()) + "%";
                mGrid.g_DArray[row].Tsuujou =0;  
                mGrid.g_DArray[row].Keigen = bbl.Z_Set(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) - bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu)); 
                
            }
            else
            {
                mGrid.g_DArray[row].ZeiNu = "非税";
                mGrid.g_DArray[row].ZeinuJuchuu = bbl.Z_SetStr(hanbai * juchuu); 
                mGrid.g_DArray[row].zeikomijuchuu = mGrid.g_DArray[row].ZeinuJuchuu;
                mGrid.g_DArray[row].ZeinuTanku = "0%";
                mGrid.g_DArray[row].Tsuujou = 0;
                mGrid.g_DArray[row].Keigen = 0;
            }
            if (IsHanBai)
            {
                var val1 = bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu) - (bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu));
                mGrid.g_DArray[row].ArariGaku = bbl.Z_SetStr(val1);
            }

            {
                //if (taxflg == "0")     // incTax 2
                //{
                //    //if (zeiritsu8 == 0 && !string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaxRate))
                //    //    zeiritsu8 = Convert.ToInt16(mGrid.g_DArray[RW].TaxRate.Replace("%", ""));
                //    mGrid.g_DArray[row].zeikomijuchuu = mGrid.g_DArray[row].ZeinuJuchuu;
                //}
                //else // 1 or 2 
                //{
                //    mGrid.g_DArray[row].zeikomijuchuu = bbl.Z_SetStr(GetResultWithHasuKbn(Convert.ToInt32(res.Rows[0]["KBN"].ToString()), Convert.ToDecimal(mGrid.g_DArray[row].ZeinuJuchuu) * (bbl.Z_Set(res.Rows[0]["TaxRate"].ToString()) + 100) / 100).ToString());
                //}
                //if (!IsHanBai)  // inside hanbaitanka
                //{
                //    if (taxflg == "0") //  Zeinu TaxNotation 3
                //    {
                //        mGrid.g_DArray[row].ZeiNu = "非税";
                //    }
                //    else
                //    {
                //        mGrid.g_DArray[row].ZeiNu = "税抜";
                //    }



                //    if (taxflg == "0") // taxRates 4 
                //    {
                //        mGrid.g_DArray[row].ZeinuTanku = "0%";
                //    }
                //    else
                //    {
                //        mGrid.g_DArray[row].ZeinuTanku = bbl.Z_SetStr(res.Rows[0]["TaxRate"].ToString()) + "%";
                //    }
                //}
                //else  // arariGaku 3
                //{
                //    var val1 = (bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu) - bbl.Z_Set(mGrid.g_DArray[row].HacchuTanka)) * bbl.Z_Set(mGrid.g_DArray[row].JuchuuSuu);
                //    mGrid.g_DArray[row].ArariGaku = bbl.Z_SetStr(val1);
                //}
                ////same with hanbai Tanka
                //mGrid.g_DArray[row].Tsuujou = bbl.Z_Set(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) -bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu));  // normalTax 5
                //mGrid.g_DArray[row].Keigen = bbl.Z_Set(bbl.Z_Set(mGrid.g_DArray[row].zeikomijuchuu) - bbl.Z_Set(mGrid.g_DArray[row].ZeinuJuchuu)); // ReduceTax 6
            }

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
                w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);
                string[] pnel = new string[] { "panel_1", "panel_2", "panel_3", "panel_4","panel_5"};
                if (w_ActCtl is GridControl.clsGridCheckBox)
                {
                    if (pnel.Contains(w_ActCtl.Parent.Parent.Name))
                    {
                        //SetFuncKeyAll(this, "111111110111");
                    }
                }
                else
                {
                    if (pnel.Contains(w_ActCtl.Parent.Name) || pnel.Contains(w_ActCtl.Parent.Parent.Name))
                    {
                       // SetFuncKeyAll(this, "111111111111");
                    }
                }
                if (OperationMode == EOperationMode.SHOW)
                    SetFuncKeyAll(this, "111111000000");
                else
                {
                    //SetFuncKeyAll(this, "111111000001");
                }
                    
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
                SetFuncKeyAll(this, "111111110101");
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

                if ( ActiveControl is GridControl.clsGridCheckBox chk  && chk.Name.Contains("chk_"))
                {
                    if (chk.Focus())
                    {
                        if (chk.Parent is Panel pnl)
                        {
                            pnl.BackColor = ClsGridTenjikai.BKColor; ;
                        }
                    }
                }
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
                for (int d = 0; d < 8; d++)
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
            tkb = new TenjikaiJuuChuu_BL();

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
                if (dt.TableName == "Table1")  // Update Dt
                {
                    S_Clear_Grid();

                    int c = 0;
                   
                    foreach (DataRow dr in dt.Rows)   // 
                    {
                        mGrid.g_DArray[c].TenjiRow = dr["TenjiRow"].ToString();
                        mGrid.g_DArray[c].SCJAN = mGrid.g_DArray[c].OldJanCD = dr["JANCD"].ToString();
                        mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                        mGrid.g_DArray[c].ShouName = dr["SKUName"].ToString();
                        CheckGrid((int)ClsGridTenjikai.ColNO.SCJAN, c, true);
                        mGrid.g_DArray[c].Color = dr["ColorNO"].ToString();
                        mGrid.g_DArray[c].ColorName = dr["ColorName"].ToString();
                        mGrid.g_DArray[c].Size = dr["SizeNO"].ToString();
                        mGrid.g_DArray[c].SizeName = dr["SizeName"].ToString();
                        mGrid.g_DArray[c].ShuukaYo = dr["ShippingPlanDate"].ToString();
                        (mGrid.g_DArray[c].ShuukaSou) = dr["SoukoCD"].ToString();
                        mGrid.g_DArray[c].AdminNo = dr["AdminNo"].ToString();
                        mGrid.g_DArray[c].Chk = true;
                        mGrid.g_DArray[c].HacchuTanka = bbl.Z_SetStr(dr["OrderUnitPrice"].ToString());//
                        mGrid.g_DArray[c].NyuuKayo = dr["ArrivePlanDate"].ToString();//
                        mGrid.g_DArray[c].JuchuuSuu = bbl.Z_SetStr(dr["JuchuuSuu"].ToString());//
                        // mGrid.g_DArray[c].TenI = dr["Tani"].ToString();//
                        mGrid.g_DArray[c].TaniHDN = dr["Tani"].ToString();//
                        mGrid.g_DArray[c].TenI = tkb.D_TeniSelectbyTaniCD(mGrid.g_DArray[c].TaniHDN);
                        mGrid.g_DArray[c].HanbaiTanka = bbl.Z_SetStr(dr["JuchuuUnitPrice"].ToString());//
                        //CheckGrid((int)ClsGridTenjikai.ColNO.HanbaiTanka, c, true);

                        mGrid.g_DArray[c].ZeinuJuchuu = bbl.Z_SetStr(dr["JuchuuHontaiGaku"].ToString());//
                        mGrid.g_DArray[c].zeikomijuchuu = bbl.Z_SetStr(dr["JuchuuGaku"].ToString());//
                        mGrid.g_DArray[c].ArariGaku = bbl.Z_SetStr(dr["ProfitGaku"].ToString());//
                        mGrid.g_DArray[c].ZeiNu = dr["ZeiHyouki"].ToString();//
                        mGrid.g_DArray[c].ZeinuTanku = dr["JuchuuTaxRitsu"].ToString() + "%";//

                        // mGrid.g_DArray[c].Chk = dr["_3SKUName"].ToString();   
                        mGrid.g_DArray[c].ShanaiBi = dr["CommentOutStore"].ToString();//
                        mGrid.g_DArray[c].ShagaiBi = dr["CommentInStore"].ToString();//
                        mGrid.g_DArray[c].KobeTsu = dr["IndividualClientName"].ToString();//

                        mGrid.g_DArray[c].ChoukuSou = dr["DirectFLG"].ToString().Equals("0") ? false : true;//
                        mGrid.g_DArray[c].TorokuFlg = dr["TorokuFlg"].ToString();//
                        mGrid.g_DArray[c].TaxRateFlg = dr["TaxRateFLG"].ToString();//
                        c++;
                    }
                }
                else
                {
                    int Res_Gyo = 0;
                    if (OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.INSERT)
                    {
                        
                        int w_Gyo = 0;

                        for (int i = mGrid.g_MK_Max_Row - 1; i >= 0; i--)
                        {
                            if (i == 4)
                            {

                            }
                            if (!string.IsNullOrEmpty(mGrid.g_DArray[i].SCJAN) )
                            {
                                Res_Gyo = i;
                                break;
                            }
                            w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);
                        }
                    }
                    else
                        S_Clear_Grid();
                    int c = 0;
                    if (Res_Gyo != 0)
                    {
                        c += (Res_Gyo+1);
                    }
                    foreach (DataRow dr in dt.Rows)   // Meisai Dt
                    {
                        mGrid.g_DArray[c].SCJAN = mGrid.g_DArray[c].OldJanCD = dr["_1JanCD"].ToString();
                        mGrid.g_DArray[c].SKUCD = dr["_2SKUCD"].ToString();
                        mGrid.g_DArray[c].ShouName = dr["_3SKUName"].ToString();
                        CheckGrid((int)ClsGridTenjikai.ColNO.SCJAN, c,true);
                        mGrid.g_DArray[c].Color = dr["_4ColorNo"].ToString();
                        mGrid.g_DArray[c].ColorName = dr["_5ColorName"].ToString();
                        mGrid.g_DArray[c].Size = dr["_6SizeNo"].ToString();
                        mGrid.g_DArray[c].SizeName = dr["_7SizeName"].ToString();
                        mGrid.g_DArray[c].ShuukaYo = dr["_8ShuukaYoteiBi"].ToString();
                        (mGrid.g_DArray[c].ShuukaSou) = dr["_9SoukoName"].ToString();
                        // mGrid.g_DArray[c].Empty = dr["_3SKUName"].ToString();

                        mGrid.g_DArray[c].HacchuTanka = dr["_10SiireTanka"].ToString();//
                        mGrid.g_DArray[c].NyuuKayo = dr["_11NyuukaYoteiHyou"].ToString();//
                        mGrid.g_DArray[c].JuchuuSuu = dr["_12SoukunoSu"].ToString();//
                        mGrid.g_DArray[c].TaniHDN = dr["_13TaniCD"].ToString();//
                        mGrid.g_DArray[c].TenI = tkb.D_TeniSelectbyTaniCD(mGrid.g_DArray[c].TaniHDN);
                        mGrid.g_DArray[c].HanbaiTanka = dr["_14SalePriceOutTax"].ToString();//
                       // CalcZei(c,(int)ClsGridTenjikai.ColNO.HanbaiTanka, true);
                        mGrid.g_DArray[c].ZeinuJuchuu = dr["_15OrderExcTax"].ToString();//
                        mGrid.g_DArray[c].zeikomijuchuu = dr["_16OrderIncTax"].ToString();//
                        mGrid.g_DArray[c].ArariGaku = dr["_17TotalProfit"].ToString();//
                        mGrid.g_DArray[c].ZeiNu = dr["_18Taxnotation"].ToString();//
                        mGrid.g_DArray[c].ZeinuTanku = dr["_19TaxRate"].ToString();//

                        mGrid.g_DArray[c].Chk =true;   
                        mGrid.g_DArray[c].ShanaiBi = dr["_20ExternaRemarks"].ToString();//
                        mGrid.g_DArray[c].ShagaiBi = dr["_21InternalRemarks"].ToString();//
                        mGrid.g_DArray[c].KobeTsu = dr["_22kobetsuHanbai"].ToString();//

                        mGrid.g_DArray[c].TorokuFlg = dr["_23TorokuFlag"].ToString();//
                        mGrid.g_DArray[c].TaxRateFlg = dr["_25TaxRateFlg"].ToString();//
                        c++;
                    }
                }
            }
            CalcKin();
      
          //  mGrid.S_DispToArray
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
                    if (kr_1.Checked)
                        kr_1.Focus();
                    else
                        kr_2.Focus();
                }
                else if (p.Name == "pnl_haisou")
                {
                    if (hr_3.Checked)
                        hr_3.Focus();
                    else
                        hr_4.Focus();

                }
            }
            previousCtrl = this.ActiveControl;
           
            SetFuncKeyAll(this, "111111001001");
            if (ActiveControl.Name == "sc_Tenji")
            {
                previousCtrl = this.ActiveControl;
                if (OperationMode == EOperationMode.SHOW)
                    SetFuncKeyAll(this, "111111001000");
                
            }
            else
            {
                previousCtrl = null;
            }
        }
        private void EnableDetail()
        {
            DisablePanel(PanelHeader);
            panel1.Enabled = true;
            pnl_haisou.Enabled = true;
            pnl_kokyakuu.Enabled = true;
            EnablePanel(panel1);
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
                    if (index == (int)Eindex.SCTenjiKai)
                    {
                        bool ret = CheckKey(index);
                        if (ret)
                        {
                            ChangeFunKeys();
                            tkb = new TenjikaiJuuChuu_BL();
                            tje = new Tenjikai_Entity() {
                                TenjiKaiOrderNo = detailControls[(int)Eindex.SCTenjiKai].Text
                            };

                            var dr = tkb.Select_TenjiData(tje, out DataTable GridDt);
                            if (dr)
                            {
                                SetTenJiDetails(index, tje, GridDt);  // DetailEnable
                                
                                 Scr_Lock(1,3,0); // unlocked details , TenjiNo, Mesaidisable
                                if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
                                {
                                    SetTenjiGrid(GridDt);
                                    panel1.Enabled = false;
                                   // S_BodySeigyo(1, 1);
                                    S_BodySeigyo(4, 1);
                                    sc_Tenji.Enabled = sc_Tenji.BtnSearch.Enabled = true;

                                }
                                else if ( OperationMode == EOperationMode.UPDATE)
                                {
                                    SetTenjiGrid(GridDt,true);
                                    S_BodySeigyo(1,0);
                                    S_BodySeigyo(1, 1);
                                    sc_Tenji.Enabled = sc_Tenji.BtnSearch.Enabled = false;
                                    btn_Meisai.Enabled = true;

                                    // Count from --last row.
                                    // and rebind grid
                                }
                                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)  S_BodySeigyo(4, 0);
                                scjan_1.Focus();
                            }
                        }

                    }
                    else
                    {
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
                mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridTenjikai.ColNO.Chk);
                detailControls[(int)Eindex.SCTenjiKai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenjiKai].Parent).BtnSearch.Enabled = true;
                detailControls[(int)Eindex.SCTenjiKai].Focus();
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                //画面へデータセット後、明細部入力可
                Scr_Lock(2, 3, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                detailControls[(int)Eindex.SCTenjiKai].Enabled= ((CKM_SearchControl)detailControls[(int)Eindex.SCTenjiKai].Parent).BtnSearch.Enabled = false;
                
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
        private void CustomerDeliveryInfo(bool ret, Tenjikai_Entity tje)
        {
            //if (ret)
            //{
            //    //if (mce.DeleteFlg == "1")
            //    //{
            //    //    bbl.ShowMessage("E119");
            //    //    //顧客情報ALLクリア
            //    //    ClearCustomerInfo(kbn);
            //    //    detailControls[index].Focus();
            //    //    return false;
            //    //}
            //    //mTaxTiming = Convert.ToInt16(mce.TaxTiming);
            //    //mTaxFractionKBN = Convert.ToInt16(mce.TaxFractionKBN);
            //    if (tje.k.Equals(0))
            //    {                                //住所情報セット
            //        addInfo.ade.VariousFLG = mce.VariousFLG;
            //        KZip1 = addInfo.ade.ZipCD1 = mce.ZipCD1;
            //        KZip2 = addInfo.ade.ZipCD2 = mce.ZipCD2;
            //        KAddress1 = addInfo.ade.Address1 = mce.Address1;
            //        KAddress2 = addInfo.ade.Address2 = mce.Address2;
            //        addInfo.ade.Tel11 = mce.Tel11;
            //        addInfo.ade.Tel12 = mce.Tel12;
            //        addInfo.ade.Tel13 = mce.Tel13;
            //        detailControls[(int)(Eindex.KDenwa1)].Text = mce.Tel11;
            //        detailControls[(int)(Eindex.KDenwa2)].Text = mce.Tel12;
            //        detailControls[(int)(Eindex.KDenwa3)].Text = mce.Tel13;
            //        detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
            //        // detailControls[(int)(Eindex.KJuShou2)].Text = ;
            //        //radio
            //        if (mce.AliasKBN == "1")
            //        {
            //            kr_1.Checked = true;
            //            kr_2.Checked = false;
            //        }
            //        else
            //        {
            //            kr_1.Checked = false;
            //            kr_2.Checked = true;
            //        }
            //        if (mce.VariousFLG == "1")
            //        {
            //            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            //            {
            //                detailControls[index + 1].Enabled = true;
            //                detailControls[index + 3].Enabled = true;
            //            }
            //            detailControls[index + 3].Text = "";

            //        }
            //        else
            //        {
            //            detailControls[index + 1].Enabled = false;
            //            detailControls[index + 3].Text = "";
            //            detailControls[index + 3].Enabled = false;
            //        }

            //        if (string.IsNullOrWhiteSpace(sc_haisosaki.TxtCode.Text))
            //        {
            //            detailControls[(int)(Eindex.SCHaiSoSaki)].Text = detailControls[(int)Eindex.SCKokyakuu].Text;
            //            CheckDetail((int)Eindex.SCHaiSoSaki);
            //            detailControls[(int)Eindex.HJuShou1].Text = detailControls[(int)Eindex.KJuShou1].Text;
            //            hr_3.Checked = kr_1.Checked;
            //            hr_4.Checked = kr_2.Checked;
            //            detailControls[(int)Eindex.HDenwa1].Text = detailControls[(int)Eindex.KDenwa1].Text;
            //            detailControls[(int)Eindex.HDenwa2].Text = detailControls[(int)Eindex.KDenwa2].Text;
            //            detailControls[(int)Eindex.HDenwa3].Text = detailControls[(int)Eindex.KDenwa3].Text;
            //            KZip1 = addInfo.adeD.ZipCD1 = addInfo.ade.ZipCD1;
            //            KZip2 = addInfo.adeD.ZipCD2 = addInfo.ade.ZipCD2;
            //            KAddress1 = addInfo.adeD.Address1 = addInfo.ade.Address1;
            //            KAddress2 = addInfo.adeD.Address2 = addInfo.ade.Address2;
            //        }
            //    }

            //    else
            //    {
            //        addInfo.adeD.VariousFLG = mce.VariousFLG;
            //        HZip1 = addInfo.adeD.ZipCD1 = mce.ZipCD1;
            //        HZip2 = addInfo.adeD.ZipCD2 = mce.ZipCD2;
            //        HAddress1 = addInfo.adeD.Address1 = mce.Address1;
            //        HAddress2 = addInfo.adeD.Address2 = mce.Address2;
            //        addInfo.adeD.Tel11 = mce.Tel11;
            //        addInfo.adeD.Tel12 = mce.Tel12;
            //        addInfo.adeD.Tel13 = mce.Tel13;
            //        detailControls[(int)Eindex.HDenwa1].Text = mce.Tel11;
            //        detailControls[(int)Eindex.HDenwa2].Text = mce.Tel12;
            //        detailControls[(int)Eindex.HDenwa3].Text = mce.Tel13;

            //        if (mce.VariousFLG == "1")
            //        {
            //            detailControls[(int)(Eindex.KJuShou1)].Text = mce.CustomerName;
            //            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            //            {
            //                detailControls[index + 1].Enabled = true;
            //                detailControls[index + 3].Enabled = true;
            //            }
            //            detailControls[index + 3].Text = "";
            //        }
            //        else
            //        {
            //            detailControls[index + 1].Text = mce.CustomerName;
            //            detailControls[index + 1].Enabled = false;
            //            detailControls[index + 3].Text = "";
            //            detailControls[index + 3].Enabled = false;
            //        }
            //        //敬称
            //        if (mce.AliasKBN == "1")
            //        {
            //            hr_3.Checked = true;
            //            hr_4.Checked = false;
            //        }
            //        else
            //        {
            //            hr_3.Checked = false;
            //            hr_4.Checked = true;
            //        }
            //        //    }
            //    }
            //}
            //else
            //{
            //    HZip1 = "";
            //    HZip2 = "";
            //    HAddress1 = "";
            //    HAddress2 = "";
            //    KZip1 = "";
            //    KZip2 = "";
            //    KAddress1 = "";
            //    KAddress2 = "";
            //    bbl.ShowMessage("E101");
            //    //顧客情報ALLクリア
            //    ClearCustomerInfo(kbn);
            //    detailControls[index].Focus();
            //    return false;
            //}
        }
        private  string FMD(string FM)
        {
            return FM.Replace("-", "/").Split(' ')[0].ToString();
        }
        private void SetTenJiDetails(int index, Tenjikai_Entity tje, DataTable Detail)
        {
            (detailControls[(int)Eindex.SCKokyakuu]).Text = tje.Kokyaku;
            CheckDetail((int)Eindex.SCKokyakuu);
            (detailControls[(int)Eindex.KJuShou1]).Text = tje.K_Name1;
            (detailControls[(int)Eindex.KJuShou2]).Text = tje.K_name2;
            (detailControls[(int)Eindex.KDenwa1]).Text = tje.K_Denwa1;
            (detailControls[(int)Eindex.KDenwa2]).Text = tje.K_Denwa2;
            (detailControls[(int)Eindex.KDenwa3]).Text = tje.K_Denwa3;


            detailControls[(int)Eindex.SCShiiresaki].Text = tje.Shiiresaki;
            CheckDetail((int)Eindex.SCShiiresaki);
           
            ((CKM_ComboBox)detailControls[(int)Eindex.Nendo]).SelectedValue = tje.Nendo;
            ((CKM_ComboBox)detailControls[(int)Eindex.ShiSon]).SelectedValue = tje.ShiZun;
            ((CKM_ComboBox)detailControls[(int)Eindex.ShuukaSouko]).SelectedValue = tje.SouKoCD;
            (detailControls[(int)Eindex.JuuChuuBi]).Text= FMD(tje.JuchuuBi);
            (detailControls[(int)Eindex.Uriageyotei]).Text = FMD(tje.UriageYoteiBi);
            (detailControls[(int)Eindex.SCTentouStaffu]).Text = tje.TantouStaffu;
            ((CKM_SearchControl)(detailControls[(int)Eindex.SCTentouStaffu].Parent)).LabelText = tje.StaffName;


          

            if (tje.CKBN == "様")
            {
                kr_1.Checked = true;
                kr_2.Checked = false;
            }
            else
            {
                kr_1.Checked = false;
                kr_2.Checked = true;
            }
            (detailControls[(int)Eindex.SCHaiSoSaki]).Text = tje.HaisoSaki;
            (detailControls[(int)Eindex.HJuShou1]).Text = tje.H_Name1;
            (detailControls[(int)Eindex.HJuShou2]).Text = tje.H_name2;
            (detailControls[(int)Eindex.HDenwa1]).Text = tje.H_Denwa1;
            (detailControls[(int)Eindex.HDenwa2]).Text = tje.H_Denwa2;
            (detailControls[(int)Eindex.HDenwa3]).Text = tje.H_Denwa3;

            if (tje.HKBN == "様")
            {
                hr_3.Checked = true;
                hr_4.Checked = false;
            }
            else
            {
                hr_3.Checked = false;
                hr_4.Checked = true;
            }

            ((CKM_ComboBox)detailControls[(int)Eindex.YoteiKinShuu]).SelectedValue = tje.YoteiKinShu;


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
                    //////////////受注日が変更された場合のチェック処理
                    ////////////if (mOldJyuchuDate != detailControls[index].Text)
                    ////////////{
                    ////////////    for (int i = (int)EIndex.StaffCD; i <= (int)EIndex.DeliveryCD; i++)
                    ////////////        if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                    ////////////            if (!CheckDependsOnDate(i, true))
                    ////////////                return false;

                    ////////////    //明細部JANCDの再チェック
                    ////////////    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    ////////////    {
                    ////////////        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                    ////////////        {
                    ////////////            if (CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, RW, true, true) == false)
                    ////////////            {
                    ////////////                //Focusセット処理
                    ////////////                ERR_FOCUS_GRID_SUB((int)ClsGridJuchuu.ColNO.JanCD, RW);
                    ////////////                return false;
                    ////////////            }
                    ////////////        }
                    ////////////    }
                    ////////////    mOldJyuchuDate = detailControls[index].Text;
                    ////////////    ScCustomer.ChangeDate = mOldJyuchuDate;
                    ////////////    ScDeliveryCD.ChangeDate = mOldJyuchuDate;
                    ////////////}
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
                       // ClearCustomerInfo(0);
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
                        //ClearCustomerInfo(1);
                        detailControls[index].Focus();
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;
                    break;
                case (int)Eindex.SCShiiresaki:    // ShiireSaki>>>>>
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        ///ClearCustomerInfo(2);
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
                        (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = "";
                       // detailControls[(int)Eindex.SCShiiresaki].Text = "";
                        detailControls[index].Focus();
                        // ClearCustomerInfo(2);
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
                      // detailControls[(int)Eindex.SCTentouStaffu].Text = "";
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)Eindex.SCShiiresaki:
                    //[Shiiresaki]

                    break;
                case (int)Eindex.SCKokyakuu:
                case (int)Eindex.SCHaiSoSaki:
                    var oldVal = "";
                    short kbn = 0;
                    if (index.Equals((int)Eindex.SCHaiSoSaki))
                        kbn = 1;

                    if (IsExecutedTriggered)
                    {
                        IsExecutedTriggered = false;
                        return true;
                    }
                    //[M_Customer_Select]
                    Entity.M_Customer_Entity mce = new M_Customer_Entity
                    {
                        
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                   var mce1 = mce;
                    BL.Customer_BL sbl = new Customer_BL();
                    bool ret = false;
                    if (OperationMode == EOperationMode.UPDATE)
                    {
                        ret = sbl.M_Customer_Select_Update( detailControls[(int)Eindex.SCTenjiKai].Text, mce, kbn,true );
                    }
                    else
                    {
                        ret = sbl.M_Customer_Select(mce, 1);
                    }
                   
                    if (kbn == 1)
                    oldVal = detailControls[(int)Eindex.SCHaiSoSaki].Text;
                    else
                        oldVal = detailControls[(int)Eindex.SCKokyakuu].Text;
                    if (ret)
                    {
                        if (mce.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //ClearCustomerInfo(kbn);
                            detailControls[index].Focus();
                            return false;
                        }
                        mTaxTiming = Convert.ToInt16(mce.TaxTiming);
                        mTaxFractionKBN = Convert.ToInt16(mce.TaxFractionKBN);
                        if (kbn.Equals(0))
                        {                                //住所情報セット
                            addInfo.ade.VariousFLG = mce.VariousFLG;
                            KZip1 = addInfo.ade.ZipCD1 = mce.ZipCD1;
                            KZip2 = addInfo.ade.ZipCD2 = mce.ZipCD2;
                            KAddress1 = addInfo.ade.Address1 = mce.Address1;
                            KAddress2 = addInfo.ade.Address2 = mce.Address2;
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
                                KZip1 = addInfo.adeD.ZipCD1 = addInfo.ade.ZipCD1;
                                KZip2 = addInfo.adeD.ZipCD2 = addInfo.ade.ZipCD2;
                                KAddress1 = addInfo.adeD.Address1 = addInfo.ade.Address1;
                                KAddress2 = addInfo.adeD.Address2 = addInfo.ade.Address2;
                            }
                        }

                        else
                        {
                            addInfo.adeD.VariousFLG = mce.VariousFLG;
                            HZip1 = addInfo.adeD.ZipCD1 = mce.ZipCD1;
                            HZip2 = addInfo.adeD.ZipCD2 = mce.ZipCD2;
                            HAddress1 = addInfo.adeD.Address1 = mce.Address1;
                            HAddress2 = addInfo.adeD.Address2 = mce.Address2;
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
                       
                        HZip1 = "";
                        HZip2 = "";
                        HAddress1 = "";
                        HAddress2 = "";
                        KZip1 = "";
                        KZip2 = "";
                        KAddress1 = "";
                        KAddress2 = "";
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo(kbn);
                        detailControls[index].Text = oldVal;
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
                    else if (ctl.Parent is CKM_SearchControl cs)
                    {
                        cs.LabelText = "";
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
                        ClearCustomerInfo(0);
                        ClearCustomerInfo(1);
                    }

                    else if (ctl is Button)
                    {
                        ClearCustomerInfo(0);
                        ClearCustomerInfo(1);
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }
            }
            catch{
                var d = a;
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
        private DataTable GetTable(out DataTable delete)
        {
            var dt = Cols(out DataTable dout);
            delete = dout;
            return dt;
        }
        private DataTable Cols(out DataTable dout)
        {
            DataTable dt = new DataTable();
            var c = new string[] {
                "TenjiRow"
                ,"TenjiCD"
                ,"No"
                ,"GYONO"
                ,"Chk"
                ,"SCJAN"
                ,"AdminNo"
                ,"SKUCD"
                ,"ShouName"
                ,"Color"
                ,"ColorName"
                ,"Size" 
                ,"SizeName"
                ,"ShuukaYo"
                ,"ChoukuSou"
                ,"ShuukaSou"
                ,"HacchuTanka"
                ,"NyuuKayo"
                ,"JuchuuSuu" 
                ,"TenI"
                ,"TeniName"
                ,"HanbaiTanka"
                ,"ZeinuJuchuu"
                ,"zeikomijuchuu"
                ,"ArariGaku"
                ,"ZeiNu"
                ,"ZeinuTanku"
                ,"ShanaiBi"
                ,"ShagaiBi"
                ,"KobeTsu"
                ,"TorokuFlg"
                ,"TaxRateFlg"
                ,"Tsuujou"
                ,"Keigen"
            };
            foreach (var colname in c)
            {
                dt.Columns.Add(colname);
            }
            var dtsakujou = new DataTable();
            dtsakujou.Columns.Add("TenjiRow");

            int h = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //DataRow dr = dt.NewRow();

                // var sd= 
                if (!string.IsNullOrEmpty(mGrid.g_DArray[RW].SKUCD))
                {
                    if ((mGrid.g_DArray[RW].Chk) && !String.IsNullOrEmpty(mGrid.g_DArray[RW].SCJAN))

                    {
                        h++;
                        dt.Rows.Add(
                             String.IsNullOrEmpty(mGrid.g_DArray[RW].TenjiRow) ? null : mGrid.g_DArray[RW].TenjiRow
                           , detailControls[(int)Eindex.SCTenjiKai].Text
                           , h.ToString()
                           , mGrid.g_DArray[RW].GYONO
                           , mGrid.g_DArray[RW].Chk ? "1" : "0"
                           , mGrid.g_DArray[RW].SCJAN
                           , mGrid.g_DArray[RW].AdminNo == "" ? "0" : mGrid.g_DArray[RW].AdminNo == null ? "0" : mGrid.g_DArray[RW].AdminNo   // Changed value as from null to 0 default as doc mentioned 
                           , mGrid.g_DArray[RW].SKUCD
                           , mGrid.g_DArray[RW].ShouName == "" ? null : mGrid.g_DArray[RW].ShouName
                           , mGrid.g_DArray[RW].Color == "" ? null : mGrid.g_DArray[RW].Color
                           , mGrid.g_DArray[RW].ColorName == "" ? null : mGrid.g_DArray[RW].ColorName
                           , mGrid.g_DArray[RW].Size == "" ? null : mGrid.g_DArray[RW].Size
                           , mGrid.g_DArray[RW].SizeName == "" ? null : mGrid.g_DArray[RW].SizeName
                           , mGrid.g_DArray[RW].ShuukaYo == "" ? null : mGrid.g_DArray[RW].ShuukaYo
                           , mGrid.g_DArray[RW].ChoukuSou ? "1" : "0"
                           , mGrid.g_DArray[RW].ShuukaSou
                           , mGrid.g_DArray[RW].HacchuTanka.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].NyuuKayo == "" ? null : mGrid.g_DArray[RW].NyuuKayo
                           , mGrid.g_DArray[RW].JuchuuSuu.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].TaniHDN == "" ? null : mGrid.g_DArray[RW].TaniHDN
                           , mGrid.g_DArray[RW].TeniName == "" ? null : mGrid.g_DArray[RW].TeniName
                           , mGrid.g_DArray[RW].HanbaiTanka.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].ZeinuJuchuu.ToString().Replace(",", "") == "" ? null : mGrid.g_DArray[RW].ZeinuJuchuu.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].zeikomijuchuu.ToString().Replace(",", "") == "" ? null : mGrid.g_DArray[RW].zeikomijuchuu.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].ArariGaku.ToString().Replace(",", "") == "" ? null : mGrid.g_DArray[RW].ArariGaku.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].ZeiNu.ToString().Replace(",", "") == "" ? null : mGrid.g_DArray[RW].ZeiNu.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].ZeinuTanku.ToString().Replace(",", "").Replace("%", "").Replace("％", "") == "" ? null : mGrid.g_DArray[RW].ZeinuTanku.ToString().Replace(",", "").Replace("%", "").Replace("％", "")
                           , mGrid.g_DArray[RW].ShanaiBi == "" ? null : mGrid.g_DArray[RW].ShanaiBi
                           , mGrid.g_DArray[RW].ShagaiBi == "" ? null : mGrid.g_DArray[RW].ShagaiBi
                           , mGrid.g_DArray[RW].KobeTsu == "" ? null : mGrid.g_DArray[RW].KobeTsu
                           , mGrid.g_DArray[RW].Chk ? "1" : "0"   // , mGrid.g_DArray[RW].TorokuFlg
                           , mGrid.g_DArray[RW].TaxRateFlg
                           , mGrid.g_DArray[RW].Tsuujou.ToString().Replace(",", "")
                           , mGrid.g_DArray[RW].Keigen.ToString().Replace(",", "")
                           );
                    }
                    else if (!(mGrid.g_DArray[RW].Chk) && mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.Chk, RW].Cell_Enabled && !String.IsNullOrEmpty(mGrid.g_DArray[RW].TenjiRow))
                    {
                        dtsakujou.Rows.Add(mGrid.g_DArray[RW].TenjiRow);
                    }
                }
                //else
                //{
                //    dt.Rows.Add();
                //}
            }
            dout = dtsakujou;
            return dt;
            ////    //dt.Columns.Add();
            ////     internal string GYONO;
            ////internal bool Chk;
            ////internal string SCJAN;
            ////internal string AdminNo; // Hidden
            ////internal string SKUCD;
            ////internal string ShouName;
            ////internal string Color;
            ////internal string ColorName;
            ////internal string Size;
            ////internal string SizeName;
            ////internal string ShuukaYo;
            ////internal bool ChoukuSou;
            ////internal string ShuukaSou;
            ////internal string Empty;
            ////internal string HacchuTanka;
            ////internal string NyuuKayo;
            ////internal string JuchuuSuu;
            ////internal string TenI;
            ////internal string TeniName;
            ////internal string HanbaiTanka;
            ////internal string ZeinuJuchuu;
            ////internal string zeikomijuchuu;
            ////internal string ArariGaku;
            ////internal string ZeiNu;
            ////internal string ZeinuTanku;
            //////internal bool Chk;
            ////internal string ShanaiBi;
            ////internal string ShagaiBi;
            ////internal string KobeTsu;
            ////internal string TorokuFlg; // hidden
            ////internal string TaxRateFlg; //hidden
            ////internal decimal Tsuujou; // hidden
            ////internal decimal Keigen;// hidden
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
                addInfo.adeD.CustomerCD = detailControls[(int)(Eindex.SCHaiSoSaki)].Text;
                addInfo.adeD.CustomerName = detailControls[(int)Eindex.HJuShou1].Text;
                addInfo.adeD.CustomerName2 = detailControls[(int)Eindex.HJuShou2].Text;
                addInfo.adeD.Tel11 = detailControls[(int)Eindex.HDenwa1].Text;
                addInfo.adeD.Tel12 = detailControls[(int)Eindex.HDenwa2].Text;
                addInfo.adeD.Tel13 = detailControls[(int)Eindex.HDenwa3].Text;
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
                DeleteExclusive();
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
            if (!CheckKey(-1))
            {
                return;
            }
            if (!System.IO.Directory.Exists("C:\\SMS\\TenziKaiJuchuu\\"))
            {
                System.IO.Directory.CreateDirectory("C:\\SMS\\TenziKaiJuchuu\\");
            }
            openFileDialog1.InitialDirectory = "C:\\SMS\\TenziKaiJuchuu\\";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Excel Worksheets|*.xlsx";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tkb = new TenjikaiJuuChuu_BL();
                var dt = ConvertToDataTable(openFileDialog1.FileName);
                if (dt == null)
                {
                    return;
                }
                tje = new Tenjikai_Entity
                {
                    xml = bbl.DataTableToXml(dt),
                    Kokyaku = detailControls[(int)Eindex.SCKokyakuu].Text,
                    JuchuuBi = detailControls[(int)Eindex.JuuChuuBi].Text,
                    Nendo = detailControls[(int)Eindex.Nendo].Text,
                    ShiZun = detailControls[(int)Eindex.ShiSon].Text,
                    Shiiresaki = detailControls[(int)Eindex.SCShiiresaki].Text,
                    ShuuKaSouKo = ((CKM_ComboBox)detailControls[(int)Eindex.ShuukaSouko]).SelectedValue.ToString(),
                    KibouBi1 = this.KibouBi1,
                    KibouBi2 = this.KibouBi2
                };
                var resTable = tkb.M_TenjiKaiJuuChuu_Select(tje);
                MesaiHyouJi(resTable);
                S_BodySeigyo(0, 1);
                mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                S_BodySeigyo(4, 0);
                scjan_1.Focus();
                //SelectNextControl(ActiveControl, true, true, true, true);
            }
        }
        private void MesaiHyouJi(DataTable dt)
        {
            SetTenjiGrid(dt,true);
        }
        private void SetTenjiGrid(DataTable dt, bool IsShow = false)
        {
            SetMultiColNo(dt);
            if(IsShow)
            S_BodySeigyo(1, 1);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
            
           // DisablePanel(PanelHeader);
            ///chk_1.Focus();
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
                bbl.ShowMessage("E137" + ex.StackTrace);
                return null;
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

            if (res.Rows.Count == 0)
            {
                MessageBox.Show("No Data Exist");
                return null;
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
           // panel2.Refresh();
        }
        private void SetEnabled(bool enabled)
        {
            detailControls[(int)Eindex.KJuShou1].Enabled = enabled;
            detailControls[(int)Eindex.KJuShou2].Enabled = enabled;

            detailControls[(int)Eindex.HJuShou1].Enabled = enabled;
            detailControls[(int)Eindex.HJuShou2].Enabled = enabled;
            if (enabled)
            {

            }
            else
            {
                detailControls[(int)Eindex.KJuShou1].Text = "";
                detailControls[(int)Eindex.KJuShou2].Text = "";
                detailControls[(int)Eindex.HJuShou1].Text = "";
                detailControls[(int)Eindex.HJuShou2].Text = "";
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
                          
                             ((CKM_SearchControl)detailControls[(int)Eindex.SCTenjiKai].Parent).BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            for (int idx = 0; idx < (int)Eindex.Count; idx++)
                            {
                                switch (idx)
                                {
                                    case (int)Eindex.KJuShou1:
                                    case (int)Eindex.KJuShou2:
                                    case (int)Eindex.HJuShou1:
                                    case (int)Eindex.HJuShou2:
                                        break;
                                    default:
                                        detailControls[idx].Enabled = Kbn == 0 ? true : false;
                                        break;
                                }
                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

                            btn_Customer.Enabled = Kbn == 0 ? true : false;
                            btn_Shipping.Enabled = Kbn == 0 ? true : false;
                            panel1.Enabled = Kbn == 0 ? true : false;
                            panel2.Enabled = Kbn == 0 ? true : false;
                            if (OperationMode == EOperationMode.UPDATE)
                            if (Kbn == 0)
                            {
                                detailControls[(int)Eindex.SCTenjiKai].Enabled = searchButtons[0].Enabled = btn_Meisai.Enabled = Kbn != 0;
                                DisablePanel(PanelHeader);
                            }
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
            if (pKBN == 4 && pGrid == 2  )
            {
                if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
                    return;
            }
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
                                //  if (OperationMode != EOperationMode.SHOW && OperationMode != EOperationMode.DELETE)
                                mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = true;
                            }

                        }
                        else
                        {
                            //IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                detailControls[(int)Eindex.SCTenjiKai].Enabled = true;
                                detailControls[(int)Eindex.SCTenjiKai].Focus();
                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                detailControls[(int)Eindex.SCTenjiKai].Enabled = true;
                                ((CKM_SearchControl)detailControls[(int)Eindex.SCTenjiKai].Parent).BtnSearch.Enabled = true;
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
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SCJAN))
                                {
                                    mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = true;
                                    continue;
                                }
                                if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SKUCD))
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        {
                                            case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                                            case (int)ClsGridTenjikai.ColNO.ChoukuSou:
                                            case (int)ClsGridTenjikai.ColNO.ShuukaSou:
                                            case (int)ClsGridTenjikai.ColNO.HacchuTanka:
                                            case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                                            case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                                            case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
                                            case (int)ClsGridTenjikai.ColNO.Chk:
                                            case (int)ClsGridTenjikai.ColNO.ShanaiBi:
                                            case (int)ClsGridTenjikai.ColNO.ShagaiBi:
                                            case (int)ClsGridTenjikai.ColNO.KobeTsu:

                                                {
                                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                    break;
                                                }

                                                case (int)ClsGridTenjikai.ColNO.SCJAN:
                                                //case (int)ClsGridTenjikai.ColNO.ShouName:
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
                            Scr_Lock(2, 3, 0);
                            Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "111111000001");
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
                            //   Scr_Lock(0, 0, 0);


                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000001");

                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {

                                SetFuncKeyAll(this, "111111000000");
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
                           if (OperationMode != EOperationMode.UPDATE ) mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = false;
                        }

                    }
                    else if (pGrid == 0)
                    {
                        for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            if (OperationMode != EOperationMode.UPDATE) mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = true;
                        }
                    }
                    else if (pGrid == 2)
                    {
                    
                        for (w_Row = 0; w_Row <= 999; w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            if (!mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled && String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD))
                            {
                                mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = true;
                            }

                            if (String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD) && String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SCJAN))
                            {
                                mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, w_Row].Cell_Enabled = true;
                                FlgChange(w_Row, false);
                            }
                            if (!String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SCJAN) && !String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD))
                            {
                                FlgChange(w_Row, true);
                            }


                        }
                        //try
                        //{
                        //    for (int i = mGrid.g_MK_Max_Row - 1; i >= 0; i--)
                        //    {

                        //        if (!mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, i].Cell_Enabled && !String.IsNullOrEmpty(mGrid.g_DArray[i].SKUCD))
                        //        {
                        //            FlgChange(i, true);
                        //            break;
                        //        }
                        //    }
                        //}
                        //catch (Exception e){
                        //    var x = e.Message;
                        //}
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        panel2.Refresh();
                    }
                    break;
            }
        }
        private void FlgChange(int w_Row,bool Flg)
        {
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ChoukuSou, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ShuukaSou, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.NyuuKayo, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.JuchuuSuu, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.HanbaiTanka, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.Chk, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ShagaiBi, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ShanaiBi, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.KobeTsu, w_Row].Cell_Enabled = Flg;


            //mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ShouName, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.ShuukaYo, w_Row].Cell_Enabled = Flg;
            mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.HacchuTanka, w_Row].Cell_Enabled = Flg;
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
                        if (OperationMode == EOperationMode.INSERT)
                        {
                            SetInitialState();
                        }
                        detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        ChangeOperationMode(base.OperationMode);
                        if (OperationMode == EOperationMode.INSERT)
                        {
                            SetInitialState();
                        }
                        detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
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
                    EsearchKbn kbn = EsearchKbn.Null;
                  
                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (w_Col == (int)ClsGridTenjikai.ColNO.SCJAN)
                        //商品検索
                        kbn = EsearchKbn.Product;
                    //else if (w_Col == (int)ClsGridTenjikai.ColNO.VendorCD)
                    //    //仕入先検索
                    //    kbn = EsearchKbn.Vendor;

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

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
            if (index == (int)Eindex.SCTenjiKai)
            {


                tkb = new TenjikaiJuuChuu_BL();
                //入力必須(Entry required)
                if (string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCTenjiKai].Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    return false;
                }
                var res = tkb.Check_DTenjikaiJuuchuu((detailControls[(int)Eindex.SCTenjiKai]).Text);
                if (res.Rows.Count ==0)
                {

                    //Check Tenji
                    bbl.ShowMessage("E142");
                    return false;
                }
                if (res.Rows[0]["DeleteDateTime"].ToString() != "")
                {

                    //Check deleteDateTime
                    bbl.ShowMessage("E144");
                    return false;
                }

                // E143 // Already Checked in Main M_StoreAuthor:

                if (res.Select("JuchuuHurikaeZumiFLG = 1") == null)
                {
                    //Check deleteDateTime
                    bbl.ShowMessage("E256");
                    return false;
                }
                bool ret = SelectAndInsertExclusive();
                if (!ret)
                    return false;
                //排他処理
                //bool ret = SelectAndInsertExclusive();
                //if (!ret)
                //    return false;

                //return CheckData(set);
            }
            return true;

        }
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = Btn_F1;  /// Confirmed

           // IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        private void KeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(keyControls, sender);
                    bool ret = CheckKey(index);

                    if (ret)
                    {
                        if (index == (int)Eindex.SCTenjiKai)
                        {
                            detailControls[(int)Eindex.SCTenjiKai].Focus();
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
        private bool IsExecutedTriggered = false;
        protected override void ExecSec()
        {

            if (OperationMode == EOperationMode.SHOW)
            {
                return;
            }
            if (OperationMode != EOperationMode.SHOW) // KeysControl
            {
                if (CheckKey(-1, false) == false)
                {
                    return;
                }
            }
            //IsExecutedTriggered = true;
            if (OperationMode != EOperationMode.DELETE && OperationMode != EOperationMode.SHOW )  // DetailsControl
            {
                if (OperationMode != EOperationMode.UPDATE)
                for (int i = 0; i < detailControls.Length; i++)
                {
                    IsExecutedTriggered = true;
                    
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        IsExecutedTriggered = false;
                        return;
                    }
                    IsExecutedTriggered = false;
                }
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                ////明細部チェック
                ///try
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++) // GridControl
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SCJAN) == false)
                    {
                        for (int CL = (int)ClsGridTenjikai.ColNO.SCJAN; CL < (int)ClsGridTenjikai.ColNO.COUNT; CL++)
                        {
                            if (CheckGrid(CL, RW, true,false,true) == false)
                            {
                                //Focusセット処理
                                ERR_FOCUS_GRID_SUB(CL, RW);
                                return;
                            }
                        }
                    }
                }

                ////各金額項目の再計算必要
                CalcKin();

            }
            var resData = GetTable(out DataTable delete);
            if (OperationMode == EOperationMode.INSERT)
            {
               
                if (resData.Rows.Count == 0)
                {
                    bbl.ShowMessage("E257");
                    return;
                }
                var GetData = GetTenjiData();
                var xml = bbl.DataTableToXml(resData);
                tkb = new TenjikaiJuuChuu_BL();
                var TenjiInsert = tkb.D_TenjiInsert(GetData, xml);
                if (TenjiInsert)
                {
                    bbl.ShowMessage("I101");
                    ChangeOperationMode(OperationMode);
                    detailControls[(int)Eindex.SCShiiresaki].Focus();
                    return;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    return;
                }
            }
            else if (OperationMode == EOperationMode.UPDATE)
            {
               
                if (resData.Rows.Count == 0)
                {
                    bbl.ShowMessage("E257");
                    return;
                }
                var GetData = GetTenjiData();
                var xml = bbl.DataTableToXml(resData);
                var dxml = bbl.DataTableToXml(delete);
                tkb = new TenjikaiJuuChuu_BL();
                var TenjiUpdate = tkb.D_TenjiUpdate(GetData, xml,dxml);
                if (TenjiUpdate)
                {
                    bbl.ShowMessage("I101");
                    ChangeOperationMode(OperationMode);
                    return;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    return;
                }
            }
            else if (OperationMode == EOperationMode.DELETE)
            {
               
                if (resData.Rows.Count == 0)
                {
                    bbl.ShowMessage("E257");
                    return;
                }
                var GetData = GetTenjiData();
                var xml = bbl.DataTableToXml(resData);
                var resDelete = tkb.D_TenjiDelete(GetData, xml);
                if (resDelete)
                {
                    bbl.ShowMessage("I101");
                    ChangeOperationMode(OperationMode);
                    return;
                }
                else
                {
                    bbl.ShowMessage("E101");
                    return;
                }
            }
           

            //if (OperationMode == EOperationMode.DELETE)
            //    bbl.ShowMessage("I102");
            //else
            //    bbl.ShowMessage("I101");

            ////更新後画面クリア
            //ChangeOperationMode(OperationMode);
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
                //mGrid.g_DArray[i].TenjiRow = null;
                if (i.Equals(w_Row))
                {
                    mGrid.g_DArray[i].TenjiRow = null;
                    //前の行をコピーしてできた新しい行
                    //mGrid.g_DArray[i].JuchuuNO = "";
                    //mGrid.g_DArray[i].juchuGyoNO = 0;
                    //mGrid.g_DArray[i].copyJuchuGyoNO = 0;
                    //mGrid.g_DArray[i].KariHikiateNO = "";
                }

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
            CalcKin();

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

             CalcKin();

            int col = (int)ClsGridTenjikai.ColNO.SCJAN;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            
            S_BodySeigyo(4, 2); ///
            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridTenjikai.Gen_MK_FocusMove.MvSet, (int)ClsGridTenjikai.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
            w_Act.Focus();
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
              //  mGrid.g_DArray[i].TenjiRow = null;
                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            w_Gyo = Convert.ToInt16(mGrid.g_DArray[w_Row].GYONO);
            // 一行クリア
            Array.Clear(mGrid.g_DArray, w_Row, 1);
            //退避内容を戻す
            mGrid.g_DArray[w_Row].GYONO = w_Gyo.ToString();          //行番号
            mGrid.g_DArray[w_Row].TenjiRow = null;
            CalcKin();

            int col = (int)ClsGridTenjikai.ColNO.SCJAN;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            S_BodySeigyo(4, 2); ///
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


            if (pCol == (int)ClsGridTenjikai.ColNO.SCJAN )
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].SCJAN))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridTenjikai.ColNO.SCJAN)
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
                            case (int)ClsGridTenjikai.ColNO.SCJAN:
                                if (mGrid.g_DArray[pRow].GYONO == "0")
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    //mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    if (OperationMode != EOperationMode.UPDATE && OperationMode != EOperationMode.INSERT)  mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                   // mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;
                            case (int)ClsGridTenjikai.ColNO.Chk:
                            case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                            //case (int)ClsGridTenjikai.ColNO.ShouName:
                            case (int)ClsGridTenjikai.ColNO.ShuukaYo:
                            case (int)ClsGridTenjikai.ColNO.ShuukaSou:
                            case (int)ClsGridTenjikai.ColNO.ChoukuSou:
                            case (int)ClsGridTenjikai.ColNO.HacchuTanka:
                            case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
                            case (int)ClsGridTenjikai.ColNO.NyuuKayo:
                            case (int)ClsGridTenjikai.ColNO.ShagaiBi:
                            case (int)ClsGridTenjikai.ColNO.ShanaiBi:
                            case (int)ClsGridTenjikai.ColNO.KobeTsu:
                                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                                mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                break;

                        }

                    }
                    w_AllFlg = false;

                }
            }
           // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);
            // Only allow For JanCd not Confirmed
            //for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            //{
            //    bool IsEmpty = false;
            //    for (int w_Cola = 0; w_Cola < (int)ClsGridTenjikai.ColNO.COUNT; w_Cola++)
            //    {
            //        if (w_Cola == (int)ClsGridTenjikai.ColNO.SCJAN )
            //        {
            //            if (String.IsNullOrEmpty(mGrid.g_DArray[W_CtlRow].SCJAN))
            //            IsEmpty = true;
            //        }
            //    switch (w_Cola)
            //        {
            //            case (int)ClsGridTenjikai.ColNO.SCJAN:
            //                if (mGrid.g_DArray[pRow].GYONO == "0")
            //                {
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Enabled = true;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Bold = false;
            //                }
            //                else
            //                {
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Enabled = false;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Bold = true;
            //                }
            //                break;

            //            case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
            //            case (int)ClsGridTenjikai.ColNO.ShouName:
            //            case (int)ClsGridTenjikai.ColNO.ShuukaYo:
            //            case (int)ClsGridTenjikai.ColNO.ShuukaSou:
            //            case (int)ClsGridTenjikai.ColNO.ChoukuSou:
            //            case (int)ClsGridTenjikai.ColNO.HacchuTanka:
            //            case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
            //            case (int)ClsGridTenjikai.ColNO.NyuuKayo:
            //                if (IsEmpty)
            //                {
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Enabled = false;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_ReadOnly = true;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Bold = true;
            //                }
            //                else {
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Enabled = true;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_ReadOnly = false;
            //                    mGrid.g_MK_State[w_Cola, W_CtlRow].Cell_Bold = false;
            //                }
            //                break;

            //        }
            //    }

            //}
            //if (pCol == (int)ClsGridJuchuu.ColNO.Nyuka)
            //{
            //    if (mGrid.g_DArray[pRow].Nyuka == "")
            //    {
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Enabled = true;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_ReadOnly = false;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Bold = false;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Enabled = true;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_ReadOnly = false;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Bold = false;
            //    }
            //    else
            //    {
            //        //入荷予定日,発注先 入力不可
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Enabled = false;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_ReadOnly = true;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.ArrivePlanDate, pRow].Cell_Bold = true;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Enabled = false;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_ReadOnly = true;
            //        mGrid.g_MK_State[(int)ClsGridJuchuu.ColNO.VendorCD, pRow].Cell_Bold = true;
            //    }
            //}

        }

        protected  Tenjikai_Entity GetTenjiData()
        {
            //[M_Staff]
            //M_Staff_Entity mse = new M_Staff_Entity
            //{
            //    StaffCD = InOperatorCD
            //};
            //Login_BL lbl = new Login_BL();
            //mse =  lbl.M_Staff_InitSelect(mse);

            //this.lblOperatorName.Text = mse.StaffName;
            //this.lblLoginDate.Text = mse.SysDate;
            //this.StoreCD = lblStoreCD.Text = mse.StoreCD;
            var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Tenjikai_Entity tje = new Tenjikai_Entity() {
                TenjiKaiOrderNo = detailControls[(int)Eindex.SCTenjiKai].Text,
                JuchuuBi = detailControls[(int)Eindex.JuuChuuBi].Text,
                ShuuKaSouKo = (detailControls[(int)Eindex.ShuukaSouko] as CKM_ComboBox).SelectedValue.ToString(),
                TantouStaffu = detailControls[(int)Eindex.SCTentouStaffu].Text,
                Shiiresaki = detailControls[(int)Eindex.SCShiiresaki].Text,
                Nendo = (detailControls[(int)Eindex.Nendo] as CKM_ComboBox).SelectedValue.ToString(),
                ShiZun = (detailControls[(int)Eindex.ShiSon] as CKM_ComboBox).SelectedValue.ToString(),

                Kokyaku = detailControls[(int)Eindex.SCKokyakuu].Text,
                K_Name1 = detailControls[(int)Eindex.KJuShou1].Text,
                K_name2 = detailControls[(int)Eindex.KJuShou2].Text,
                K_radio = kr_1.Checked ? "1" : "2",
                K_Zip1 = KZip1,
                K_Zip2 = KZip2,
                K_Address1 = KAddress1,
                K_Address2 = KAddress2,
                K_Denwa1 = detailControls[(int)Eindex.KDenwa1].Text,
                K_Denwa2 = detailControls[(int)Eindex.KDenwa2].Text,
                K_Denwa3 = detailControls[(int)Eindex.KDenwa3].Text,
                KDenwa21=null,
                KDenwa22=null,
                KDenwa23=null,
                KkanaMei=null,
                HaisoSaki = detailControls[(int)Eindex.SCHaiSoSaki].Text,
                H_Name1 = detailControls[(int)Eindex.HJuShou1].Text,
                H_name2 = detailControls[(int)Eindex.HJuShou2].Text,
                H_radio = hr_3.Checked ? "1" : "2",
                H_Zip1 = HZip1,
                H_Zip2 = HZip2,
                H_Address1 = HAddress1,
                H_Address2 = HAddress2,
                H_Denwa1 = detailControls[(int)Eindex.HDenwa1].Text,
                H_Denwa2 = detailControls[(int)Eindex.HDenwa2].Text,
                H_Denwa3 = detailControls[(int)Eindex.HDenwa3].Text,
                HDenwa21 = null,
                HDenwa22 = null,
                HDenwa23 = null,
                HkanaMei =null,

                ZeiKomi =  hdn_IncAmt.Text,
                Zeinu= hdn_ExcAmt.Text,
                Keijen=  hdn_RduAmt.Text,
                Tsuujou= hdn_NmalAmt.Text,

                GenkaGaku= hdn_CostAmt.Text,
                ArariGaku= hdn_GrossAmt.Text,
                YoteiKinShu=  (detailControls[(int)Eindex.YoteiKinShuu] as CKM_ComboBox).SelectedValue.ToString(),
                UriageYoteiBi= (detailControls[(int)Eindex.Uriageyotei].Text),

                Sumi = "0",
                Nichi=null,
                InsertDt= dt,
                InsertOperator=  InOperatorCD,
                DeleteDt= dt,
                StoreCD=  StoreCD,
                Program= InProgramID
                ,PC= this.InPcID
            };

            return tje;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var val = detailControls[(int)Eindex.SCShiiresaki].Text;
        }

        private void DeleteExclusive()
        {
            if (mOldJyuchuNo == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.TenjiNo,
                Number = mOldJyuchuNo,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldJyuchuNo = "";
        }
        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT) // Will work and hold CD When Delete or Update 
                return true;

            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCTenjiKai].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.TenjiNo,
                Number = detailControls[(int)Eindex.SCTenjiKai].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                detailControls[(int)Eindex.SCTenjiKai].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldJyuchuNo = detailControls[(int)Eindex.SCTenjiKai].Text;
                return ret;
            }
        }
    }
}
