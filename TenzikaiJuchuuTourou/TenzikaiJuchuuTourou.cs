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
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        private string KibouBi1 = "";
        private string KibouBi2 = "";
        CKM_SearchControl mOldCustomerCD;
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
                base.StartProgram();

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

                //for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
                //{
                //    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridJuchuu.ColNO.SoukoName, W_CtlRow].CellCtl;
                //    sctl.Bind(ymd);
                //}
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    string juchuNO = cmds[(int)ECmdLine.PcID + 1];   //
                    ChangeOperationMode(EOperationMode.SHOW);
                    //  keyControls[(int)Eindex.JuchuuNO].Text = juchuNO;
                    //   CheckKey((int)EIndex.JuchuuNO, true);
                }
                Btn_F7.Text = "行削除(F7)";
                Btn_F8.Text = "行追加(F8)";
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
            }
            kr_1.KeyDown += Kr_2_KeyDown;
            kr_2.KeyDown += Kr_2_KeyDown;
            hr_3.KeyDown += Hr_3_KeyDown;
            hr_4.KeyDown += Hr_3_KeyDown;
            //  sc_Tenji.KeyDown += TenzikaiJuchuuTourou_KeyDown;
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
                            //sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
                            //sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
                            //sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            //sctl.TxtCode.Tag = W_CtlRow.ToString();
                            //sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            //mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            //mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            //mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            //CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            //sctl.Enter += new System.EventHandler(GridControl_Enter);
                            //sctl.Leave += new System.EventHandler(GridControl_Leave);
                            //sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            //    sctl.Tag = W_CtlRow.ToString();
                        }
                        //else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        //{
                        //    Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                        //    if (w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
                        //        btn.Click += new System.EventHandler(BTN_Site_Click);
                        //    else
                        //        btn.Click += new System.EventHandler(BTN_Zaiko_Click);
                        //}
                        //else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        //{
                        //    GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                        //    check.Enter += new System.EventHandler(GridControl_Enter);
                        //    check.Leave += new System.EventHandler(GridControl_Leave);
                        //    check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                        //    check.Click += new System.EventHandler(CHK_Print_Click);
                        //}
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
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //    case (int)ClsGridTenjikai.ColNO.JuchuuHontaiGaku:
                    //    case (int)ClsGridTenjikai.ColNO.JuchuuUnitPrice:
                    //    case (int)ClsGridTenjikai.ColNO.JuchuuGaku:
                    //    case (int)ClsGridTenjikai.ColNO.CostUnitPrice:
                    //    case (int)ClsGridTenjikai.ColNO.CostGaku:
                    //    case (int)ClsGridTenjikai.ColNO.ProfitGaku:
                    //    case (int)ClsGridTenjikai.ColNO.OrderUnitPrice:
                    //        //case (int)ClsGridJuchuu.ColNO.OrderGaku:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
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
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TorokuFlg, d].CellCtl = new Control();  //torokuflg  hidden
                    mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TaxRateFlg, d].CellCtl = new Control(); //taxrateflg  hidden
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

                //case (int)Eindex.JuchuuDate:
                //    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                //    //日付として正しいこと(Be on the correct date)Ｅ１０３
                //    if (!bbl.CheckDate(detailControls[index].Text))
                //    {
                //        //Ｅ１０３
                //        bbl.ShowMessage("E103");
                //        detailControls[index].Focus();
                //        return false;
                //    }
                //    //入力できる範囲内の日付であること
                //    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                //    {
                //        //Ｅ１１５
                //        bbl.ShowMessage("E115");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    //受注日が変更された場合のチェック処理
                //    if (mOldJyuchuDate != detailControls[index].Text)
                //    {
                //        for (int i = (int)EIndex.StaffCD; i <= (int)EIndex.DeliveryCD; i++)
                //            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                //                if (!CheckDependsOnDate(i, true))
                //                    return false;

                //        //明細部JANCDの再チェック
                //        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                //        {
                //            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                //            {
                //                if (CheckGrid((int)ClsGridJuchuu.ColNO.JanCD, RW, true, true) == false)
                //                {
                //                    //Focusセット処理
                //                    ERR_FOCUS_GRID_SUB((int)ClsGridJuchuu.ColNO.JanCD, RW);
                //                    return false;
                //                }
                //            }
                //        }
                //        mOldJyuchuDate = detailControls[index].Text;
                //        ScCustomer.ChangeDate = mOldJyuchuDate;
                //        ScDeliveryCD.ChangeDate = mOldJyuchuDate;
                //    }

                //    break;

                //case (int)EIndex.SoukoName:
                //    //選択必須(Entry required)
                //    if (!RequireCheck(new Control[] { detailControls[index] }))
                //    {
                //        CboSoukoName.MoveNext = false;
                //        return false;
                //    }
                //    if (!CheckDependsOnDate(index))
                //    {
                //        CboSoukoName.MoveNext = false;
                //        return false;
                //    }
                //    break;

                //case (int)EIndex.StaffCD:
                //    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    if (!CheckDependsOnDate(index))
                //        return false;

                //    break;


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

                    //////case (int)EIndex.Point:
                    //////    //入力された場合 ポイント＞	Form.Header.残ポイントの場合、Error				
                    //////    if(bbl.Z_Set(detailControls[index].Text)>0)
                    //////        if(bbl.Z_Set(detailControls[index].Text) > bbl.Z_Set(lblPoint.Text))
                    //////        {
                    //////            //Ｅ１４６
                    //////            bbl.ShowMessage("E146");
                    //////            detailControls[index].Focus();
                    //////            return false;
                    //////        }

                    //////    break;

                    ////case (int)EIndex.SalesDate:
                    ////    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    ////    {
                    ////        return true;
                    ////    }
                    ////    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    ////    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    ////    if (!bbl.CheckDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１０３
                    ////        bbl.ShowMessage("E103");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    //入力できる範囲内の日付であること
                    ////    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１１５
                    ////        bbl.ShowMessage("E115");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    //Form.Detail.入荷予定日＞form.売上予定日の場合、
                    ////    bool errFlg = false;
                    ////    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    ////    {
                    ////        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ArrivePlanDate))
                    ////        {
                    ////            int result = detailControls[index].Text.CompareTo(mGrid.g_DArray[RW].ArrivePlanDate);
                    ////            if (result < 0)
                    ////            {
                    ////                errFlg = true;
                    ////                break;
                    ////            }
                    ////        }
                    ////    }
                    ////    if (errFlg)
                    ////    {
                    ////        //「入荷予定日が売上予定日より後になっている商品があります。このまま処理を進めますか？」
                    ////        if (bbl.ShowMessage("Q304") != DialogResult.Yes)
                    ////        {
                    ////            detailControls[index].Focus();
                    ////            //売上予定日にCursorを移動
                    ////            return false;
                    ////        }
                    ////    }
                    ////    break;

                    ////case (int)EIndex.FirstPaypentPlanDate:
                    ////    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    ////    {
                    ////        return true;
                    ////    }

                    ////    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    ////    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    ////    if (!bbl.CheckDate(detailControls[index].Text))
                    ////    {
                    ////        //Ｅ１０３
                    ////        bbl.ShowMessage("E103");
                    ////        detailControls[index].Focus();
                    ////        return false;
                    ////    }

                    ////    break;

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
                //case (int)Eindex.SoukoName:
                //    //[M_Souko_Select]
                //    M_Souko_Entity me = new M_Souko_Entity
                //    {
                //        SoukoCD = CboSoukoName.SelectedValue.ToString(),
                //        ChangeDate = ymd,
                //        DeleteFlg = "0"
                //    };

                //    DataTable mdt = mibl.M_Souko_IsExists(me);
                //    if (mdt.Rows.Count > 0)
                //    {
                //        if (!base.CheckAvailableStores(mdt.Rows[0]["StoreCD"].ToString()))
                //        {
                //            bbl.ShowMessage("E141");
                //            detailControls[index].Focus();
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        bbl.ShowMessage("E101");
                //        detailControls[index].Focus();
                //        return false;
                //    }

                //    break;

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

                //mOldCustomerCD = "";
                //mTaxFractionKBN = 0;
                //mTaxTiming = 0;

                ////addInfo = new FrmAddress();

                //ScCustomer.LabelText = "";
                //detailControls[(int)Eindex.CustomerName].Text = "";
                //detailControls[(int)EIndex.CustomerName2].Text = "";
                //detailControls[(int)EIndex.CustomerName].Enabled = false;
                //textBox1.Text = "";
                //textBox2.Text = "";
                //lblLastSalesDate.Text = "";
                //lblStoreName.Text = "";
                //lblTankaName.Text = "";
                //lblPoint.Text = "";
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
                //mOldDeliveryCD = "";

                //ScDeliveryCD.LabelText = "";
                //detailControls[(int)EIndex.DeliveryName].Text = "";
                //detailControls[(int)EIndex.DeliveryName2].Text = "";
                //detailControls[(int)EIndex.DeliveryName].Enabled = false;
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
            //  S_Clear_Grid();   //画面クリア（明細部）


            //   mOrderTaxTiming = 0;
        }

        protected void BindCombo()
        {
            cbo_nendo.Bind(C_dt);
            cbo_Shuuka.Bind(C_dt);
            cbo_season.Bind(C_dt);
            cbo_yotei.Bind(C_dt);
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
            SetMultiColNo(dt);
           // S_BodySeigyo();
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

            //w_IsGrid = mGrid.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow);
            //if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            //{

            //    // 明細部にフォーカスがあるとき
            //    w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //    if (Math.Sign(this.Vsb_Mei_0.Value - mGrid.g_MK_DataValue) == -1)
            //        // 上へスクロール
            //        w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvPrv;
            //    else
            //        // 下へスクロール
            //        w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvNxt;

            //    // 一旦 フォーカスを退避
            //    //IMT_DMY_0.Focus();
            //}

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid.S_DispToArray(mGrid.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

            //if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            //{

            //    // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
            //    if (w_Dest == (int)ClsGridBase.Gen_MK_FocusMove.MvPrv)
            //    {
            //        if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
            //            // その行の最後から探す
            //            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
            //    }
            //    else if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
            //    {
            //        if (mGrid.g_WheelFLG == true)
            //        {
            //            // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
            //            // 最後のフォーカス可能セルに移動
            //            if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
            //                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
            //        }
            //        else
            //            // その行の先頭から探す
            //            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
            //    }
            //}

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
                                            case (int)ClsGridTenjikai.ColNO.Color:
                                            case (int)ClsGridTenjikai.ColNO.ColorName:
                                            case (int)ClsGridTenjikai.ColNO.Size:
                                            case (int)ClsGridTenjikai.ColNO.SizeName:
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
                                            case (int)ClsGridTenjikai.ColNO.ShanaiBi:
                                            case (int)ClsGridTenjikai.ColNO.ShagaiBi:
                                            case (int)ClsGridTenjikai.ColNO.KobeTsu:
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

                if (i.Equals(w_Row))
                {
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

            //string ymd = detailControls[(int)EIndex.JuchuuDate].Text;

            //if (string.IsNullOrWhiteSpace(ymd))
            //    ymd = bbl.GetDate();

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


            //if (pCol == (int)ClsGridTenjikai.ColNO.SCJAN || w_AllFlg)
            //{
            //    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].SCJAN))
            //    {
            //        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            //        {
            //            if (w_Col == (int)ClsGridTenjikai.ColNO.SCJAN)
            //                //JANCD使用可
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //            else
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //        }

            //        w_AllFlg = false;
            //    }
            //    else
            //    {
            //        //JANCD入力時
            //        w_AllFlg = true;

            //        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            //        {
            //            switch (w_Col)
            //            {
            //                case (int)ClsGridTenjikai.ColNO.SCJAN:
            //                    if (mGrid.g_DArray[pRow].GYONO == 0)
            //                    {
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
            //                    }
            //                    else
            //                    {
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
            //                    }
            //                    break;

            //                case (int)ClsGridJuchuu.ColNO.JuchuuSuu:
                              
            //                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                    mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                             
            //                    break;

            //                case (int)ClsGridJuchuu.ColNO.SKUName:
            //                case (int)ClsGridJuchuu.ColNO.ColorName:
            //                case (int)ClsGridJuchuu.ColNO.SizeName:
            //                case (int)ClsGridJuchuu.ColNO.CostUnitPrice:
            //                    if (mGrid.g_DArray[pRow].VariousFLG == 1)
            //                    {
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
            //                    }
            //                    else
            //                    {
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
            //                    }
            //                    break;


            //                case (int)ClsGridJuchuu.ColNO.JuchuuUnitPrice:    // 
            //                case (int)ClsGridJuchuu.ColNO.CommentOutStore:    // 
            //                case (int)ClsGridJuchuu.ColNO.IndividualClientName:    //  
            //                case (int)ClsGridJuchuu.ColNO.CommentInStore:    //
            //                case (int)ClsGridJuchuu.ColNO.NotPrintFLG:    //
            //                case (int)ClsGridJuchuu.ColNO.ChkTyokuso:    //
            //                case (int)ClsGridJuchuu.ColNO.ChkFuyo:    //
            //                case (int)ClsGridJuchuu.ColNO.Site:    //
            //                case (int)ClsGridJuchuu.ColNO.Zaiko:    // 
            //                case (int)ClsGridJuchuu.ColNO.ChkExpress:    // 
            //                case (int)ClsGridJuchuu.ColNO.ShippingPlanDate:    //
            //                case (int)ClsGridJuchuu.ColNO.OrderUnitPrice:    //  
            //                                                                 //case (int)ClsGridJuchuu.ColNO.OrderGaku:    //  
            //                    {
            //                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                    }
            //                    break;

            //                case (int)ClsGridJuchuu.ColNO.VendorCD:           //発注先
            //                case (int)ClsGridJuchuu.ColNO.ArrivePlanDate:    //入荷予定日
            //                    {
            //                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].Nyuka))
            //                        {
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
            //                        }
            //                        else
            //                        {
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
            //                        }
            //                    }
            //                    break;

            //                case (int)ClsGridJuchuu.ColNO.SoukoName:          //出荷倉庫
            //                    {
            //                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].Syukka))
            //                        {
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
            //                        }
            //                        else
            //                        {
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
            //                        }
            //                    }
            //                    break;

            //            }

            //        }
            //        w_AllFlg = false;

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
            //else if (pCol == (int)ClsGridJuchuu.ColNO.CostUnitPrice)
            //{
            //    if (mGrid.g_DArray[pRow].VariousFLG == 1)
            //    {
            //        mGrid.g_MK_State[pCol, pRow].Cell_Enabled = true;
            //        mGrid.g_MK_State[pCol, pRow].Cell_ReadOnly = false;
            //        mGrid.g_MK_State[pCol, pRow].Cell_Bold = false;
            //    }
            //    else
            //    {
            //        mGrid.g_MK_State[pCol, pRow].Cell_Enabled = false;
            //        mGrid.g_MK_State[pCol, pRow].Cell_ReadOnly = true;
            //        mGrid.g_MK_State[pCol, pRow].Cell_Bold = true;
            //    }
            //}


        }
    }
}
