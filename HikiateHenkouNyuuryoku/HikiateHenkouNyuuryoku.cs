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
using GridBase;

namespace HikiateHenkouNyuuryoku
{
    /// <summary>
    /// HikiateHenkouNyuuryoku 引当変更入力
    /// </summary>
    internal partial class HikiateHenkouNyuuryoku : FrmMainForm
    {
        private const string ProID = "HikiateHenkouNyuuryoku";
        private const string ProNm = "引当変更入力";
        private const short mc_L_END = 3; // ロック用
        private const string TempoJuchuuNyuuryoku = "TempoJuchuuNyuuryoku.exe";


        private enum EOpeMode : short
        {
            ZAIKO,
            JUCHUU
        }

        private enum HIndex : int
        {
            StoreCD,
            SKUCD,
            JANCD
        }

        private enum ZIndex : int
        {
            OrderDateFrom,
            OrderDateTo,
            ArrivalPlanDateFrom,
            ArrivalPlanDateTo,
            ArrivalDateFrom,
            ArrivalDateTo,
            OrderCD,
            OrderNO
        }

        private enum JIndex : int
        {
            JuchuuDateFrom,
            JuchuuDateTo,
            Customer,
            JuchuuNo,
            ChkNotReserve,
            ChkNotSale,
            ChkJuchuuKBN3,
            ChkJuchuuKBN2,
            ChkJuchuuKBN1,
            ChkJuchuuKBN4
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        private Control[] headControls;
        private Control[] zaikoControls;
        private Control[] juchuuControls;

        private HikiateHenkouNyuuryoku_BL hhbl;
        private HikiateHenkouNyuuryoku_Entity hhe;

        private DataTable zaikoTbl;
        private DataTable juchuuTbl;

        private DataTable zaikoDispTbl;
        private DataTable juchuuDispTbl;

        private int mZaikoCurrentRow;
        private int mHikiateCurrentRow;

        private string mAdminNO;
        private EOpeMode mOpeMode;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridZaiko mGrid = new ClsGridZaiko();
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_EnableCnt;

        ClsGridHikiate mGrid2 = new ClsGridHikiate();
        private int m_dataCnt2 = 0;        // 修正削除時に画面に展開された行数
        #region -- 明細部をグリッドのように扱うための関数 ↓--------------------

        // ----------------------------------------------------------------
        // 明細部初期化
        // 画面のコントロールを配列にセット
        // 固定色/フォーカスセット不可(クリックでのみフォーカスセット)/使用可 等の初期設定
        // Tab等によるフォーカス移動順の設定
        // 行の色の繰り返し単位と色を設定
        // ----------------------------------------------------------------
        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridZaiko.gc_P_GYO <= ClsGridZaiko.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridZaiko.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridZaiko.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridZaiko.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridZaiko.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray();

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
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(GridControl_PreviewKeyDown);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Validating += new System.ComponentModel.CancelEventHandler(GridControl_Validating);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.Tag = W_CtlRow.ToString();
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Enter += new System.EventHandler(GridControl_Enter);
                            check.Leave += new System.EventHandler(GridControl_Leave);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            //check.Click += new System.EventHandler(CHK_Del_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            //Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            //btn.Click += new System.EventHandler(BTN_Detail_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridZaiko.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridZaiko.ColNO.GYONO; i <= (int)ClsGridZaiko.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridZaiko.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridZaiko.ColNO.StockSu:
                        case (int)ClsGridZaiko.ColNO.PlanSu:
                        case (int)ClsGridZaiko.ColNO.AllowableSu:
                        case (int)ClsGridZaiko.ColNO.ReserveSu:
                        case (int)ClsGridZaiko.ColNO.InstructionSu:
                            mGrid.SetProp_SU(6, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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

        // 明細部のコントロールを配列にセット
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 0].CellCtl = IMT_ZAGYO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 0].CellCtl = IMT_NYUDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 0].CellCtl = IMT_ODRNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 0].CellCtl = IMT_SIRNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 0].CellCtl = IMN_STKSU_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 0].CellCtl = IMN_PLNSU_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 0].CellCtl = IMN_ARWSU_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 0].CellCtl = IMN_ZRVSU_0;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 0].CellCtl = IMN_ZSJSU_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 1].CellCtl = IMT_ZAGYO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 1].CellCtl = IMT_NYUDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 1].CellCtl = IMT_ODRNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 1].CellCtl = IMT_SIRNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 1].CellCtl = IMN_STKSU_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 1].CellCtl = IMN_PLNSU_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 1].CellCtl = IMN_ARWSU_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 1].CellCtl = IMN_ZRVSU_1;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 1].CellCtl = IMN_ZSJSU_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 2].CellCtl = IMT_ZAGYO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 2].CellCtl = IMT_NYUDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 2].CellCtl = IMT_ODRNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 2].CellCtl = IMT_SIRNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 2].CellCtl = IMN_STKSU_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 2].CellCtl = IMN_PLNSU_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 2].CellCtl = IMN_ARWSU_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 2].CellCtl = IMN_ZRVSU_2;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 2].CellCtl = IMN_ZSJSU_2;

            // 4行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 3].CellCtl = IMT_ZAGYO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 3].CellCtl = IMT_NYUDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 3].CellCtl = IMT_ODRNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 3].CellCtl = IMT_SIRNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 3].CellCtl = IMN_STKSU_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 3].CellCtl = IMN_PLNSU_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 3].CellCtl = IMN_ARWSU_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 3].CellCtl = IMN_ZRVSU_3;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 3].CellCtl = IMN_ZSJSU_3;

            // 5行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 4].CellCtl = IMT_ZAGYO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 4].CellCtl = IMT_NYUDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 4].CellCtl = IMT_ODRNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 4].CellCtl = IMT_SIRNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 4].CellCtl = IMN_STKSU_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 4].CellCtl = IMN_PLNSU_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 4].CellCtl = IMN_ARWSU_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 4].CellCtl = IMN_ZRVSU_4;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 4].CellCtl = IMN_ZSJSU_4;

            // 6行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 5].CellCtl = IMT_ZAGYO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 5].CellCtl = IMT_NYUDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 5].CellCtl = IMT_ODRNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 5].CellCtl = IMT_SIRNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 5].CellCtl = IMN_STKSU_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 5].CellCtl = IMN_PLNSU_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 5].CellCtl = IMN_ARWSU_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 5].CellCtl = IMN_ZRVSU_5;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 5].CellCtl = IMN_ZSJSU_5;

            // 7行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 6].CellCtl = IMT_ZAGYO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 6].CellCtl = IMT_NYUDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 6].CellCtl = IMT_ODRNO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 6].CellCtl = IMT_SIRNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 6].CellCtl = IMN_STKSU_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 6].CellCtl = IMN_PLNSU_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 6].CellCtl = IMN_ARWSU_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 6].CellCtl = IMN_ZRVSU_6;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 6].CellCtl = IMN_ZSJSU_6;

            // 8行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 7].CellCtl = IMT_ZAGYO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 7].CellCtl = IMT_NYUDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 7].CellCtl = IMT_ODRNO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 7].CellCtl = IMT_SIRNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 7].CellCtl = IMN_STKSU_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 7].CellCtl = IMN_PLNSU_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 7].CellCtl = IMN_ARWSU_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 7].CellCtl = IMN_ZRVSU_7;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 7].CellCtl = IMN_ZSJSU_7;

            // 9行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 8].CellCtl = IMT_ZAGYO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 8].CellCtl = IMT_NYUDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 8].CellCtl = IMT_ODRNO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 8].CellCtl = IMT_SIRNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 8].CellCtl = IMN_STKSU_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 8].CellCtl = IMN_PLNSU_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 8].CellCtl = IMN_ARWSU_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 8].CellCtl = IMN_ZRVSU_8;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 8].CellCtl = IMN_ZSJSU_8;

            // 10行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 9].CellCtl = IMT_ZAGYO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 9].CellCtl = IMT_NYUDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 9].CellCtl = IMT_ODRNO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 9].CellCtl = IMT_SIRNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 9].CellCtl = IMN_STKSU_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 9].CellCtl = IMN_PLNSU_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 9].CellCtl = IMN_ARWSU_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 9].CellCtl = IMN_ZRVSU_9;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 9].CellCtl = IMN_ZSJSU_9;

            // 11行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 10].CellCtl = IMT_ZAGYO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 10].CellCtl = IMT_NYUDT_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 10].CellCtl = IMT_ODRNO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 10].CellCtl = IMT_SIRNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 10].CellCtl = IMN_STKSU_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 10].CellCtl = IMN_PLNSU_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 10].CellCtl = IMN_ARWSU_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 10].CellCtl = IMN_ZRVSU_10;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 10].CellCtl = IMN_ZSJSU_10;

            // 12行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 11].CellCtl = IMT_ZAGYO_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 11].CellCtl = IMT_NYUDT_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 11].CellCtl = IMT_ODRNO_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 11].CellCtl = IMT_SIRNM_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 11].CellCtl = IMN_STKSU_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 11].CellCtl = IMN_PLNSU_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 11].CellCtl = IMN_ARWSU_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 11].CellCtl = IMN_ZRVSU_11;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 11].CellCtl = IMN_ZSJSU_11;

            // 13行目
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 12].CellCtl = IMT_ZAGYO_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DispArrivalPlanDate, 12].CellCtl = IMT_NYUDT_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderNO, 12].CellCtl = IMT_ODRNO_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.VendorName, 12].CellCtl = IMT_SIRNM_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.StockSu, 12].CellCtl = IMN_STKSU_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.PlanSu, 12].CellCtl = IMN_PLNSU_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.AllowableSu, 12].CellCtl = IMN_ARWSU_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 12].CellCtl = IMN_ZRVSU_12;
            mGrid.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.InstructionSu, 12].CellCtl = IMN_ZSJSU_12;

        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, (int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvPrv, (int)ClsGridZaiko.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, (int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 MouseWheel の処理
        private void S_Grid_0_Event_MouseWheel(int pDelta)
        {
            int w_ToMove = pDelta * (-1) / (int)mGrid.g_WHEEL_DELTA;
            int w_Value;
            int w_MaxValue;

            mGrid.g_WheelFLG = true;

            //if (mGrid.g_MK_MaxValue > m_dataCnt - 1)
            //    w_MaxValue = m_dataCnt - 1;
            //else
                w_MaxValue = mGrid.g_MK_MaxValue;

            w_Value = Vsb_Mei_0.Value + w_ToMove;

            switch (w_Value)
            {
                case object _ when w_Value > w_MaxValue:
                    {
                        Vsb_Mei_0.Value = w_MaxValue;
                        break;
                    }

                case object _ when w_Value < Vsb_Mei_0.Minimum:
                    {
                        Vsb_Mei_0.Value = Vsb_Mei_0.Minimum;
                        break;
                    }

                default:
                    {
                        Vsb_Mei_0.Value = w_Value;
                        break;
                    }
            }

            mGrid.g_WheelFLG = false;
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

                        //固定色の列はその色を設定 2020/07/29通常と同じ色にする
                        switch (w_Col)
                        {
                            case (int)ClsGridZaiko.ColNO.GYONO:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridZaiko.ColNO.DispArrivalPlanDate:
                            case (int)ClsGridZaiko.ColNO.OrderNO:
                            case (int)ClsGridZaiko.ColNO.VendorName:
                            case (int)ClsGridZaiko.ColNO.StockSu:
                            case (int)ClsGridZaiko.ColNO.PlanSu:
                            case (int)ClsGridZaiko.ColNO.AllowableSu:
                            case (int)ClsGridZaiko.ColNO.InstructionSu:
                                {
                                    //mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                                    break;
                                }
                        }
                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    //// クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case object _ when ClsGridMitsumori.ColNO.DELCK:
                    //        {
                    //            // 削除チェック
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
        }

        // -------------------------------------------------------------
        // 明細部TabStopを一括で切り替える(TabStopがTrueのコントロールがない時、KeyExitが発生しなくなるため)
        // -------------------------------------------------------------
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
                    IMT_DMY_0.TabStop = pTabStop;
                else
                    IMT_DMY_0.TabStop = false;

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

        // -------------------------------------------------------------
        // 明細部Selectableを切り替え、TabStopの同期も取る
        // -------------------------------------------------------------
        private void S_Set_Cell_Selectable(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_0.Value;

            mGrid.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(pCol, pRow);
        }

        // -------------------------------------------------------------
        // Tab移動可/不可の制御
        // -------------------------------------------------------------
        private void S_Selectable(int pCol, int pRow)
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
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvNxt;

                // 一旦 フォーカスを退避
                IMT_DMY_0.Focus();
            }

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid.S_DispToArray(mGrid.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

            if (w_IsGrid & mGrid.g_InMoveFocus_Flg == 0)
            {

                // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
                if (w_Dest == (int)ClsGridBase.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                {
                    if (mGrid.g_WheelFLG == true)
                    {
                        // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
                        // 最後のフォーカス可能セルに移動
                        if (mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                    }
                    else
                        // その行の先頭から探す
                        mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
                }
            }

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            Pnl_Body.Refresh();
        }

        private void Vsb_Mei_0_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_0_Event_MouseWheel(e.Delta);
        }

        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //受注ボタン
            SetFuncKey(this, 1, false);

            //検索ボタン
            SetFuncKey(this, 8, false);

            //展開ボタン
            if (mOpeMode == EOpeMode.ZAIKO)
                SetFuncKey(this, 9, true);
            else
                SetFuncKey(this, 9, false);

        }

        private void Grid_NotFocus(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            bool w_AllFlg = false;
            int w_CtlRow;

            //if (OperationMode >= EOperationMode.DELETE)
            //    return;

            if (m_EnableCnt - 1 < pRow)
                return;

            w_CtlRow = pRow - Vsb_Mei_0.Value;
            if (w_CtlRow >= 0 && w_CtlRow <= mGrid.g_MK_Ctl_Row - 1)
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);

            //if (pCol == (int)ClsGridHacchuu.ColNO.JanCD || pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //{
            //    if (mGrid.g_DArray[pRow].DELCK == true)
            //    {
            //        // 削除チェック時は、JanCDは フォーカスセット可 変更不可
            //        mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = true;
            //        //削除チェック時、ReadOnlyの列以外 使用不可
            //        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            //        {
            //            if (mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly)
            //            {
            //                //READONLYの列(JanCD または 取消区分)は使用可
            //            }
            //            else if (pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //            {
            //                //削除チェックは使用可
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //            }
            //            else
            //            {
            //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //            }
            //        }
            //        return;
            //    }
            //    else {
            //        w_AllFlg = true;
            //        //削除チェックOFFは、JanCDは 元に戻す
            //        mGrid.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = false;
            //    }
            //}

            if (pCol == (int)ClsGridZaiko.ColNO.DispArrivalPlanDate || w_AllFlg == true)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].DispArrivalPlanDate))
                {
                    w_AllFlg = false;
                }

                else
                {
                    w_AllFlg = true;

                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        switch (w_Col)
                        {
                            case (int)ClsGridZaiko.ColNO.ReserveSu:
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                }
                                break;

                            default:
                                mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                break;

                        }
                    }
                    w_AllFlg = false;
                }
            }
        }
        #endregion

        #region -- 明細部をグリッドのように扱うための関数 ↓--------------------

        // ----------------------------------------------------------------
        // 明細部初期化
        // 画面のコントロールを配列にセット
        // 固定色/フォーカスセット不可(クリックでのみフォーカスセット)/使用可 等の初期設定
        // Tab等によるフォーカス移動順の設定
        // 行の色の繰り返し単位と色を設定
        // ----------------------------------------------------------------
        private void S_SetInit_Grid2()
        {
            int W_CtlRow;

            if (ClsGridHikiate.gc_P_GYO <= ClsGridHikiate.gMxGyo)
            {
                mGrid2.g_MK_Max_Row = ClsGridHikiate.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridHikiate.gMxGyo;
            }
            //else
            //{
            //    mGrid2.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid2.g_MK_Ctl_Row = ClsGridHikiate.gc_P_GYO;
            mGrid2.g_MK_Ctl_Col = ClsGridHikiate.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid2.g_MK_MaxValue = mGrid2.g_MK_Max_Row - mGrid2.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_1.LargeChange = mGrid2.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_1.SmallChange = 1;
            this.Vsb_Mei_1.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
            this.Vsb_Mei_1.Maximum = mGrid2.g_MK_MaxValue + this.Vsb_Mei_1.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray2();

            for (W_CtlRow = 0; W_CtlRow <= mGrid2.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int w_CtlCol = 0; w_CtlCol <= mGrid2.g_MK_Ctl_Col - 1; w_CtlCol++)
                {
                    if (mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
                    {
                        if (mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter2);
                            mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave2);
                            mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown2);
                            mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(GridControl_PreviewKeyDown2);
                            mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Validating += new System.ComponentModel.CancelEventHandler(GridControl_Validating2);

                        }
                        else if (mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Enter += new System.EventHandler(GridControl_Enter2);
                            check.Leave += new System.EventHandler(GridControl_Leave2);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown2);
                            //check.Click += new System.EventHandler(CHK_Del_Click2);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid2.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid2.g_MK_Ctl_Col, mGrid2.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid2.g_DArray = new ClsGridHikiate.ST_DArray_Grid[mGrid2.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid2.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid2.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid2.g_MK_FocusOrder = new int[mGrid2.g_MK_Ctl_Col];

            for (int i = (int)ClsGridHikiate.ColNO.GYONO; i <= (int)ClsGridHikiate.ColNO.COUNT - 1; i++)
            {
                mGrid2.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid2.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHikiate.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridHikiate.ColNO.JuchuuSuu:
                        case (int)ClsGridHikiate.ColNO.ReserveSu:
                        case (int)ClsGridHikiate.ColNO.InstructionSu:
                            mGrid2.SetProp_SU(6, ref mGrid2.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            break;
                    }

                    mGrid2.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            // TabStopﾌﾟﾛﾊﾟﾃｨの初期状態をセット
            Set_GridTabStop2(false);

            // 明細部クリア
            //S_Clear_Grid2();   Scr_Clr処理で
        }

        // 明細部のコントロールを配列にセット
        private void S_SetControlArray2()
        {
            mGrid2.F_CtrlArray_MK(mGrid2.g_MK_Ctl_Col, mGrid2.g_MK_Ctl_Row);

            // 1行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 0].CellCtl = IMT_JUGYO_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 0].CellCtl = IMT_JYUDT_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 0].CellCtl = IMT_CUSCD_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 0].CellCtl = IMT_CUSNM_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 0].CellCtl = IMT_JYUNO_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 0].CellCtl = IMN_JYUSU_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 0].CellCtl = IMN_JRVSU_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 0].CellCtl = IMN_JSJSU_0;

            // 2行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 1].CellCtl = IMT_JUGYO_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 1].CellCtl = IMT_JYUDT_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 1].CellCtl = IMT_CUSCD_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 1].CellCtl = IMT_CUSNM_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 1].CellCtl = IMT_JYUNO_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 1].CellCtl = IMN_JYUSU_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 1].CellCtl = IMN_JRVSU_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 1].CellCtl = IMN_JSJSU_1;

            // 3行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 2].CellCtl = IMT_JUGYO_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 2].CellCtl = IMT_JYUDT_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 2].CellCtl = IMT_CUSCD_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 2].CellCtl = IMT_CUSNM_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 2].CellCtl = IMT_JYUNO_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 2].CellCtl = IMN_JYUSU_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 2].CellCtl = IMN_JRVSU_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 2].CellCtl = IMN_JSJSU_2;

            // 4行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 3].CellCtl = IMT_JUGYO_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 3].CellCtl = IMT_JYUDT_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 3].CellCtl = IMT_CUSCD_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 3].CellCtl = IMT_CUSNM_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 3].CellCtl = IMT_JYUNO_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 3].CellCtl = IMN_JYUSU_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 3].CellCtl = IMN_JRVSU_3;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 3].CellCtl = IMN_JSJSU_3;

            // 5行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 4].CellCtl = IMT_JUGYO_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 4].CellCtl = IMT_JYUDT_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 4].CellCtl = IMT_CUSCD_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 4].CellCtl = IMT_CUSNM_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 4].CellCtl = IMT_JYUNO_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 4].CellCtl = IMN_JYUSU_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 4].CellCtl = IMN_JRVSU_4;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 4].CellCtl = IMN_JSJSU_4;

            // 6行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 5].CellCtl = IMT_JUGYO_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 5].CellCtl = IMT_JYUDT_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 5].CellCtl = IMT_CUSCD_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 5].CellCtl = IMT_CUSNM_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 5].CellCtl = IMT_JYUNO_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 5].CellCtl = IMN_JYUSU_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 5].CellCtl = IMN_JRVSU_5;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 5].CellCtl = IMN_JSJSU_5;

            // 7行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 6].CellCtl = IMT_JUGYO_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 6].CellCtl = IMT_JYUDT_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 6].CellCtl = IMT_CUSCD_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 6].CellCtl = IMT_CUSNM_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 6].CellCtl = IMT_JYUNO_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 6].CellCtl = IMN_JYUSU_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 6].CellCtl = IMN_JRVSU_6;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 6].CellCtl = IMN_JSJSU_6;

            // 8行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 7].CellCtl = IMT_JUGYO_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 7].CellCtl = IMT_JYUDT_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 7].CellCtl = IMT_CUSCD_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 7].CellCtl = IMT_CUSNM_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 7].CellCtl = IMT_JYUNO_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 7].CellCtl = IMN_JYUSU_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 7].CellCtl = IMN_JRVSU_7;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 7].CellCtl = IMN_JSJSU_7;

            // 9行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 8].CellCtl = IMT_JUGYO_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 8].CellCtl = IMT_JYUDT_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 8].CellCtl = IMT_CUSCD_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 8].CellCtl = IMT_CUSNM_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 8].CellCtl = IMT_JYUNO_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 8].CellCtl = IMN_JYUSU_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 8].CellCtl = IMN_JRVSU_8;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 8].CellCtl = IMN_JSJSU_8;

            // 10行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 9].CellCtl = IMT_JUGYO_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 9].CellCtl = IMT_JYUDT_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 9].CellCtl = IMT_CUSCD_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 9].CellCtl = IMT_CUSNM_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 9].CellCtl = IMT_JYUNO_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 9].CellCtl = IMN_JYUSU_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 9].CellCtl = IMN_JRVSU_9;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 9].CellCtl = IMN_JSJSU_9;

            // 11行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 10].CellCtl = IMT_JUGYO_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 10].CellCtl = IMT_JYUDT_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 10].CellCtl = IMT_CUSCD_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 10].CellCtl = IMT_CUSNM_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 10].CellCtl = IMT_JYUNO_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 10].CellCtl = IMN_JYUSU_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 10].CellCtl = IMN_JRVSU_10;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 10].CellCtl = IMN_JSJSU_10;

            // 12行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 11].CellCtl = IMT_JUGYO_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 11].CellCtl = IMT_JYUDT_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 11].CellCtl = IMT_CUSCD_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 11].CellCtl = IMT_CUSNM_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 11].CellCtl = IMT_JYUNO_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 11].CellCtl = IMN_JYUSU_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 11].CellCtl = IMN_JRVSU_11;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 11].CellCtl = IMN_JSJSU_11;

            // 13行目
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 12].CellCtl = IMT_JUGYO_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuDate, 12].CellCtl = IMT_JYUDT_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerCD, 12].CellCtl = IMT_CUSCD_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.CustomerName, 12].CellCtl = IMT_CUSNM_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuNO, 12].CellCtl = IMT_JYUNO_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JuchuuSuu, 12].CellCtl = IMN_JYUSU_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 12].CellCtl = IMN_JRVSU_12;
            mGrid2.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.InstructionSu, 12].CellCtl = IMN_JSJSU_12;

        }

        // 明細部 Tab の処理
        private void S_Grid_1_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, (int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_1_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvPrv, (int)ClsGridHikiate.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 Enter の処理
        private void S_Grid_1_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, (int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 PageDown の処理
        private void S_Grid_1_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid2.g_MK_Ctl_Row - 1) > mGrid2.g_MK_Max_Row - 1)
                w_GoRow = mGrid2.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid2.g_MK_Ctl_Row - 1);

            mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_1_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid2.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid2.g_MK_Ctl_Row - 1);

            mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1, w_GoRow, pCol);
        }

        // 明細部 MouseWheel の処理
        private void S_Grid_1_Event_MouseWheel(int pDelta)
        {
            int w_ToMove = pDelta * (-1) / (int)mGrid2.g_WHEEL_DELTA;
            int w_Value;
            int w_MaxValue;

            mGrid2.g_WheelFLG = true;

            //if (mGrid2.g_MK_MaxValue > m_dataCnt2 - 1)
            //    w_MaxValue = m_dataCnt2 - 1;
            //else
                w_MaxValue = mGrid2.g_MK_MaxValue;

            w_Value = Vsb_Mei_1.Value + w_ToMove;

            switch (w_Value)
            {
                case object _ when w_Value > w_MaxValue:
                    {
                        Vsb_Mei_1.Value = w_MaxValue;
                        break;
                    }

                case object _ when w_Value < Vsb_Mei_1.Minimum:
                    {
                        Vsb_Mei_1.Value = Vsb_Mei_1.Minimum;
                        break;
                    }

                default:
                    {
                        Vsb_Mei_1.Value = w_Value;
                        break;
                    }
            }

            mGrid2.g_WheelFLG = false;
        }
        private void S_Clear_Grid2()
        {
            int w_Row;
            int w_Col;

            for (w_Row = mGrid2.g_MK_State.GetLowerBound(1); w_Row <= mGrid2.g_MK_State.GetUpperBound(1); w_Row++)
            {
                for (w_Col = mGrid2.g_MK_State.GetLowerBound(0); w_Col <= mGrid2.g_MK_State.GetUpperBound(0); w_Col++)
                {

                    // 配列を初期化
                    mGrid2.g_MK_State[w_Col, w_Row].SetDefault();
                    // Selectable=Trueに戻ったので、下記S_DispFromArrayで TabStopがTRUEになる。そのため同期を取るためFLG更新
                    mGrid2.g_GridTabStop = true;

                    if (w_Row <= m_EnableCnt - 1)
                    {

                        // 固定色の列はその色を設定　2020/07/29　通常と同じ色にする
                        //switch (w_Col)
                        //{
                        //    case (int)ClsGridHikiate.ColNO.GYONO:
                        //    case (int)ClsGridHikiate.ColNO.JuchuuDate:
                        //    case (int)ClsGridHikiate.ColNO.CustomerCD:
                        //    case (int)ClsGridHikiate.ColNO.CustomerName:
                        //    case (int)ClsGridHikiate.ColNO.JuchuuNO:
                        //    case (int)ClsGridHikiate.ColNO.JuchuuSuu:
                        //    case (int)ClsGridHikiate.ColNO.InstructionSu:
                        //        {
                        //            mGrid2.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                        //            break;
                        //        }
                        //}
                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid2.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid2.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    //// クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case object _ when ClsGridMitsumori.ColNO.DELCK:
                    //        {
                    //            // 削除チェック
                    //            S_Set_Cell_Selectable(w_Col, w_Row, false);
                    //            break;
                    //        }
                    //}
                }
            }

            // データ用配列初期化
            Array.Clear(mGrid2.g_DArray, mGrid2.g_DArray.GetLowerBound(0), mGrid2.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid2.g_MK_Max_Row - 1; w_Row++)
                mGrid2.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid2.S_DispFromArray(0, ref this.Vsb_Mei_1);
        }

        // -------------------------------------------------------------
        // 明細部TabStopを一括で切り替える(TabStopがTrueのコントロールがない時、KeyExitが発生しなくなるため)
        // -------------------------------------------------------------
        private void Set_GridTabStop2(bool pTabStop)
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;

            try
            {
                // 画面上にEnable=Trueのコントロールがひとつしかない時はTab,Shift+Tab,↑,↓押下時にKeyExitが発生しないため、
                // 明細がスクロールしなくなる。回避策として、そのパターンが起こりうる時のみ、IMT_DMY_0.TabStopをTrueにする。
                if (OperationMode == EOperationMode.UPDATE)
                    IMT_DMY_0.TabStop = pTabStop;
                else
                    IMT_DMY_0.TabStop = false;

                // 状態が変わらない時は処理を抜ける
                if (mGrid2.g_GridTabStop == pTabStop)
                    return;

                // TabStopの状態を退避
                mGrid2.g_GridTabStop = pTabStop;

                // TabStopﾌﾟﾛﾊﾟﾃｨをセット
                for (w_CtlRow = 0; w_CtlRow <= mGrid2.g_MK_Ctl_Row - 1; w_CtlRow++)
                {
                    w_Row = w_CtlRow + Vsb_Mei_1.Value;

                    for (w_Col = 0; w_Col <= mGrid2.g_MK_Ctl_Col - 1; w_Col++)
                        mGrid2.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl.TabStop = mGrid2.F_GetTabStop(w_Col, w_Row);
                }
            }

            catch
            {
                // 例外が発生した時は抜ける
                return;
            }
        }

        // -------------------------------------------------------------
        // 明細部Selectableを切り替え、TabStopの同期も取る
        // -------------------------------------------------------------
        private void S_Set_Cell_Selectable2(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_1.Value;

            mGrid2.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid2.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid2.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid2.F_GetTabStop(pCol, pRow);
        }

        // -------------------------------------------------------------
        // Tab移動可/不可の制御
        // -------------------------------------------------------------
        private void S_Selectable2(int pCol, int pRow)
        {
        }

        private void Vsb_Mei_1_ValueChanged(object sender, System.EventArgs e)
        {
            int w_Dest = 0;
            int w_Row = 0;
            bool w_IsGrid;

            if (mGrid2.g_VSB_Flg == 1)
                return;

            w_IsGrid = mGrid2.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow);
            if (w_IsGrid & mGrid2.g_InMoveFocus_Flg == 0)
            {

                // 明細部にフォーカスがあるとき
                w_Row = w_CtlRow + Vsb_Mei_1.Value;

                if (Math.Sign(this.Vsb_Mei_1.Value - mGrid2.g_MK_DataValue) == -1)
                    // 上へスクロール
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridBase.Gen_MK_FocusMove.MvNxt;

                // 一旦 フォーカスを退避
                IMT_DMY_0.Focus();
            }

            // 画面の内容を、配列にセット(スクロール前の行に)
            mGrid2.S_DispToArray(mGrid2.g_MK_DataValue);

            // 配列より画面セット (スクロール後の行)
            mGrid2.S_DispFromArray(this.Vsb_Mei_1.Value, ref this.Vsb_Mei_1);

            if (w_IsGrid & mGrid2.g_InMoveFocus_Flg == 0)
            {

                // 元いた位置にフォーカスをセット(場合によってはロックがかかっているかもしれないのでセットしなおす)
                if (w_Dest == (int)ClsGridBase.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid2.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_1, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid2.g_MK_FocusOrder[mGrid2.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_1, w_Row, mGrid2.g_MK_FocusOrder[mGrid2.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, mGrid2.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_1, w_Row, w_Col) == false)
                {
                    if (mGrid2.g_WheelFLG == true)
                    {
                        // まず対象行の先頭からさがし、まったくフォーカス移動先が無ければ
                        // 最後のフォーカス可能セルに移動
                        if (mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, mGrid2.g_MK_Ctrl[mGrid2.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid2.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_1, w_Row, mGrid2.g_MK_FocusOrder[0]) == false)
                            mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_1, mGrid2.g_MK_Max_Row - 1, mGrid2.g_MK_FocusOrder[mGrid2.g_MK_Ctl_Col - 1]);
                    }
                    else
                        // その行の先頭から探す
                        mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid2.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_1, w_Row, mGrid2.g_MK_FocusOrder[0]);
                }
            }

            // 連続スクロールの途中に、画面の表示がおかしくなる現象への対策
            Pnl_Body2.Refresh();
        }

        private void Vsb_Mei_1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_1_Event_MouseWheel(e.Delta);
        }
        
        private void Grid_Gotfocus2(int pCol, int pRow, int pCtlRow)
        {
            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop2(true);

            //受注ボタン
            SetFuncKey(this, 1, false);
            if (mOpeMode == EOpeMode.JUCHUU)
            {
                string juchuuKbn = mGrid2.g_DArray[pRow].JuchuuKBN.ToString();
                if (juchuuKbn == "2" || juchuuKbn == "3")
                    SetFuncKey(this, 1, true);
            }

            //検索ボタン
            SetFuncKey(this, 8, false);

            //展開ボタン
            if (mOpeMode == EOpeMode.JUCHUU)
                SetFuncKey(this, 9, true);
            else
                SetFuncKey(this, 9, false);
        }

        private void Grid_NotFocus2(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            bool w_AllFlg = false;
            int w_CtlRow;

            //if (OperationMode >= EOperationMode.DELETE)
            //    return;

            if (m_EnableCnt - 1 < pRow)
                return;

            w_CtlRow = pRow - Vsb_Mei_1.Value;
            if (w_CtlRow >= 0 && w_CtlRow <= mGrid2.g_MK_Ctl_Row - 1)
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1, w_CtlRow, w_CtlRow);


            //if (pCol == (int)ClsGridHacchuu.ColNO.JanCD || pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //{
            //    if (mGrid2.g_DArray[pRow].DELCK == true)
            //    {
            //        // 削除チェック時は、JanCDは フォーカスセット可 変更不可
            //        mGrid2.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = true;


            //        //削除チェック時、ReadOnlyの列以外 使用不可
            //        for (w_Col = mGrid2.g_MK_State.GetLowerBound(0); w_Col <= mGrid2.g_MK_State.GetUpperBound(0); w_Col++)
            //        {
            //            if (mGrid2.g_MK_State[w_Col, pRow].Cell_ReadOnly)
            //            {
            //                //READONLYの列(JanCD または 取消区分)は使用可
            //            }
            //            else if (pCol == (int)ClsGridHacchuu.ColNO.ChkDel)
            //            {
            //                //削除チェックは使用可
            //                mGrid2.g_MK_State[w_Col, pRow].Cell_Enabled = true;
            //            }
            //            else
            //            {
            //                mGrid2.g_MK_State[w_Col, pRow].Cell_Enabled = false;
            //            }
            //        }
            //        return;
            //    }
            //    else {
            //        w_AllFlg = true;
            //        //削除チェックOFFは、JanCDは 元に戻す
            //        mGrid2.g_MK_State[(int)ClsGridHacchuu.ColNO.JanCD, pRow].Cell_ReadOnly = false;
            //    }
            //}


            if (pCol == (int)ClsGridHikiate.ColNO.JuchuuNO || w_AllFlg == true)
            {
                if (string.IsNullOrWhiteSpace(mGrid2.g_DArray[pRow].JuchuuNO))
                {
                    w_AllFlg = false;
                }

                else
                {
                    //入力時
                    w_AllFlg = true;

                    for (w_Col = mGrid2.g_MK_State.GetLowerBound(0); w_Col <= mGrid2.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        switch (w_Col)
                        {
                            case (int)ClsGridHikiate.ColNO.ReserveSu:
                                {
                                    mGrid2.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                }
                                break;
                            default:
                                mGrid2.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                break;
                        }
                    }
                    w_AllFlg = false;
                }
            }
        }
        #endregion

        public HikiateHenkouNyuuryoku()
        {
            InitializeComponent();

            //ホイールイベントの追加  
            this.Vsb_Mei_0.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);
            this.Vsb_Mei_1.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_1_MouseWheel);

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動      
                this.InitialControlArray();
                mOpeMode = EOpeMode.ZAIKO;
                ModeVisible = true;
                ModeText = "在庫";
                ModeColor = Color.FromArgb(0, 176, 240);

                // 明細部初期化
                this.S_SetInit_Grid();
                this.S_SetInit_Grid2();

                //起動時共通処理
                base.StartProgram();
                Btn_F2.Text = "受注(F2)";
                Btn_F3.Text = "引当(F3)";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "展開(F10)";
                Btn_F12.Text = "確定(F12)";
                SetFuncKeyAll(this, "100001001010");

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd);

                hhbl = new HikiateHenkouNyuuryoku_BL();

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

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScVendor.Value1 = "1";
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScCustomer.SetLabelWidth(300);
                ScCustomer.Value1 = "1";
                ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
                ScJuchuuNo.Value1 = InOperatorCD;
                ScJuchuuNo.Value2 = stores;

                //表示用ワークテーブル作成
                this.CreateTable();

                //画面クリア
                this.Scr_Clr(0);
                this.Scr_Lock(1);

                headControls[(int)HIndex.SKUCD].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void InitialControlArray()
        {
            headControls = new Control[] { CboStoreCD, ckM_SKUCD, ckM_JANCD };
            zaikoControls = new Control[] { ckM_TextBox1, ckM_TextBox2, ckM_TextBox3, ckM_TextBox4, ckM_TextBox5, ckM_TextBox6, ScVendor.TxtCode, ScOrderNO.TxtCode };
            juchuuControls = new Control[] { ckM_TextBox7, ckM_TextBox8, ScCustomer.TxtCode, ScJuchuuNo.TxtCode, ChkNotReserve, ChkReserved, ChkJuchuuKBN3, ChkJuchuuKBN2, ChkJuchuuKBN1,ChkJuchuuKBN4 };

            //イベント付与
            foreach (Control ctl in headControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(HeadControl_KeyDown);
                ctl.Enter += new System.EventHandler(HeadControl_Enter);
            }

            foreach (Control ctl in zaikoControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(ZaikoControl_KeyDown);
                ctl.Enter += new System.EventHandler(ZaikoControl_Enter);

            }

            foreach (Control ctl in juchuuControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(JuchuuControl_KeyDown);
                ctl.Enter += new System.EventHandler(JuchuuControl_Enter);
            }
        }
        
        /// <summary>
        /// コードチェック（ヘッダ）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckHead(int index)
        {
            DataTable dt;

            switch (index)
            {
                case (int)HIndex.StoreCD:
                    {
                        if (string.IsNullOrWhiteSpace(headControls[index].Text))
                        {
                            bbl.ShowMessage("E102");
                            CboStoreCD.Focus();
                            return false;
                        }
                        else
                        {
                            if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                            {
                                bbl.ShowMessage("E141");
                                CboStoreCD.Focus();
                                return false;
                            }

                        }

                        break;
                    }
                case (int)HIndex.SKUCD:
                    { 
                        if (string.IsNullOrWhiteSpace(headControls[index].Text))
                        {
                            if (string.IsNullOrWhiteSpace(headControls[index + 1].Text))
                                this.ClearSkuInfo();
                            return true;
                        }

                        //入力がある場合、SKUマスターに存在すること
                        //[M_SKU]
                        SKU_BL sbl = new SKU_BL();
                        M_SKU_Entity mse = new M_SKU_Entity
                        {
                            SKUCD = headControls[index].Text,
                            ChangeDate = bbl.GetDate()
                        };
                        dt = sbl.M_SKU_SelectAll(mse);

                        if (dt.Rows.Count == 0)
                        {
                            //Ｅ１０１
                            this.ClearSkuInfo();
                            bbl.ShowMessage("E101");
                            ((TextBox)headControls[index]).SelectAll();
                            return false;
                        }
                        else
                        {
                            mAdminNO = dt.Rows[0]["AdminNO"].ToString();
                            ckM_JANCD.Text = dt.Rows[0]["JanCD"].ToString();
                            lblJANCD.Text = dt.Rows[0]["JanCD"].ToString();
                            lblSKUCD.Text = dt.Rows[0]["SKUCD"].ToString();
                            lblSKUName.Text = dt.Rows[0]["SKUName"].ToString();
                            lblSizeName.Text = dt.Rows[0]["SizeName"].ToString();
                            lblColorName.Text = dt.Rows[0]["ColorName"].ToString();
                        }
                        break;
                    }
                case (int)HIndex.JANCD:
                    {
                        if (string.IsNullOrWhiteSpace(headControls[index].Text))
                        {
                            if (string.IsNullOrWhiteSpace(headControls[index - 1].Text))
                                this.ClearSkuInfo();
                            return true;
                        }


                        //入力がある場合、SKUマスターに存在すること
                        //[M_SKU]
                        M_SKU_Entity mse = new M_SKU_Entity
                        {
                            JanCD = headControls[index].Text,
                            SKUCD = null,
                            AdminNO = mAdminNO == "" ? null : mAdminNO,
                            SetKBN = null,
                            ChangeDate = bbl.GetDate()
                        };

                        SKU_BL mbl = new SKU_BL();
                        dt = mbl.M_SKU_SelectAll(mse);
                        DataRow selectRow = null;

                        if (dt.Rows.Count == 0)
                        {
                            //Ｅ１０１
                            this.ClearSkuInfo();
                            bbl.ShowMessage("E101");                            
                            ((TextBox)headControls[index]).SelectAll();
                            return false;
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
                                frmSKU.parChangeDate = bbl.GetDate();
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
                            mAdminNO = selectRow["AdminNO"].ToString();
                            ckM_SKUCD.Text = selectRow["SKUCD"].ToString();
                            lblJANCD.Text = selectRow["JanCD"].ToString();
                            lblSKUCD.Text = selectRow["SKUCD"].ToString();
                            lblSKUName.Text = selectRow["SKUName"].ToString();
                            lblSizeName.Text = selectRow["SizeName"].ToString();
                            lblColorName.Text = selectRow["ColorName"].ToString();                        

                        }
                        break;
                    }
            }

            return true;

        }

        /// <summary>
        /// 在庫状況コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckZaiko(int index)
        {
            bool ret;

            switch (index)
            {
                case (int)ZIndex.OrderDateFrom:
                case (int)ZIndex.OrderDateTo:
                case (int)ZIndex.ArrivalPlanDateFrom:
                case (int)ZIndex.ArrivalPlanDateTo:
                case (int)ZIndex.ArrivalDateFrom:
                case (int)ZIndex.ArrivalDateTo:
                    if (string.IsNullOrWhiteSpace(zaikoControls[index].Text))
                        return true;

                    string strYmd = "";
                    switch (index)
                    {
                        default:
                            strYmd = bbl.FormatDate(zaikoControls[index].Text);
                            break;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    switch (index)
                    {
                        default:
                            zaikoControls[index].Text = strYmd;
                            break;
                    }

                    //開始日 ≧ 終了日である場合Error
                    if (index == (int)ZIndex.OrderDateTo || index == (int)ZIndex.ArrivalPlanDateTo || index == (int)ZIndex.ArrivalDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(zaikoControls[index - 1].Text) && !string.IsNullOrWhiteSpace(zaikoControls[index].Text))
                        {
                            int result = zaikoControls[index].Text.CompareTo(zaikoControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                zaikoControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)ZIndex.OrderCD:
                    if (string.IsNullOrWhiteSpace(zaikoControls[index].Text))
                    {
                        //情報ALLクリア
                        ScVendor.LabelText = "";
                        return true;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorFlg = "1",
                        VendorCD = zaikoControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        ScVendor.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ScVendor.LabelText = "";
                        ScVendor.TxtCode.SelectAll();
                        return false;
                    }

                    break;

                case (int)ZIndex.OrderNO:
                    if (string.IsNullOrWhiteSpace(zaikoControls[index].Text))
                    {
                        return true;
                    }

                    //発注(D_Order)に存在すること
                    //[D_Order]
                    string orderNo = zaikoControls[index].Text;
                    DataTable dt = hhbl.D_Order_Select(orderNo);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138", "発注番号");
                        ScOrderNO.TxtCode.SelectAll();
                        return false;
                    }
                    else
                    {
                        //DeleteDateTime 「削除された発注番号」
                        if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            bbl.ShowMessage("E140", "発注番号");
                            ScOrderNO.TxtCode.SelectAll();
                            return false;
                        }

                        //権限がない場合（以下のSelectができない場合）Error　「権限のない発注番号」
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", "発注番号");
                            ScOrderNO.TxtCode.SelectAll();
                            return false;
                        }

                        break;
                    }

            }

            return true;

        }

        /// <summary>
        /// 受注状況コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckJuchuu(int index)
        {
            bool ret;

            switch (index)
            {
                case (int)JIndex.JuchuuDateFrom:
                case (int)JIndex.JuchuuDateTo:
                    if (string.IsNullOrWhiteSpace(juchuuControls[index].Text))
                        return true;

                    string strYmd = "";
                    switch (index)
                    {
                        default:
                            strYmd = bbl.FormatDate(juchuuControls[index].Text);
                            break;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    switch (index)
                    {
                        default:
                            juchuuControls[index].Text = strYmd;
                            break;
                    }

                    //開始日 ≧ 終了日である場合Error
                    if (index == (int)JIndex.JuchuuDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(juchuuControls[index - 1].Text) && !string.IsNullOrWhiteSpace(juchuuControls[index].Text))
                        {
                            int result = juchuuControls[index].Text.CompareTo(juchuuControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                juchuuControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;

                case (int)JIndex.Customer:
                    if (string.IsNullOrWhiteSpace(juchuuControls[index].Text))
                    {
                        //情報ALLクリア
                        ScCustomer.LabelText = "";
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mve = new M_Customer_Entity
                    {
                        CustomerCD = juchuuControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Customer_BL sbl = new Customer_BL();
                    ret = sbl.M_Customer_Select(mve);

                    if (ret)
                    {
                        ScCustomer.LabelText = mve.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ScCustomer.LabelText = "";
                        ScCustomer.TxtCode.SelectAll();
                        return false;
                    }

                    break;

                case (int)JIndex.JuchuuNo:
                    if (string.IsNullOrWhiteSpace(juchuuControls[index].Text))
                    {
                        return true;
                    }

                    //受注(D_juchuu)に存在すること
                    //[D_Juchuu]
                    D_Juchuu_Entity de = new D_Juchuu_Entity
                    {
                        JuchuuNO = juchuuControls[index].Text,
                    };
                    DataTable dt = hhbl.D_Juchuu_DataSelect_ForShukkaShoukai(de);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138", "受注番号");
                        ScJuchuuNo.TxtCode.SelectAll();
                        return false;
                    }
                    else
                    {
                        //DeleteDateTime 「削除された受注番号」
                        if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                        {
                            bbl.ShowMessage("E140", "受注番号");
                            ScJuchuuNo.TxtCode.SelectAll();
                            return false;
                        }

                        //権限がない場合（以下のSelectができない場合）Error　「権限のない受注番号」
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E139", "受注番号");
                            ScJuchuuNo.TxtCode.SelectAll();
                            return false;
                        }

                        break;
                    }

            }

            return true;

        }

        /// <summary>
        /// 在庫状況明細入力チェック
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="chkAll"></param>
        /// <returns></returns>
        private bool CheckGrid(int col, int row, bool chkAll = false)
        {

            switch (col)
            {                
                case (int)ClsGridZaiko.ColNO.ReserveSu:

                    //入力できる場合(When input is possible)
                    if (mGrid.g_MK_State[col, row].Cell_Enabled　&& !mGrid.g_MK_State[col, row].Cell_ReadOnly)
                    {
                        //画面の内容を配列へセット
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);


                        //出荷指示数＞引当数
                        if (bbl.Z_Set(mGrid.g_DArray[row].InstructionSu) > bbl.Z_Set(mGrid.g_DArray[row].ReserveSu))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E222");
                            return false;
                        }

                        //在庫情報引当数合計
                        decimal reserveSu = 0;
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO))
                            {
                                reserveSu += bbl.Z_Set(mGrid.g_DArray[RW].ReserveSu);
                            }
                        }

                        //引当数＞受注数
                        if (reserveSu > bbl.Z_Set(mGrid2.g_DArray[mHikiateCurrentRow].JuchuuSuu))
                        {
                            bbl.ShowMessage("E223", bbl.Z_SetStr(reserveSu), bbl.Z_SetStr(mGrid2.g_DArray[mHikiateCurrentRow].JuchuuSuu));
                            return false;
                        }

                        decimal updateSu = bbl.Z_Set(mGrid.g_DArray[row].ReserveSu) - bbl.Z_Set(mGrid.g_DArray[row].SelectReserveSu);

                        //受注情報.出荷指示数＞受注情報.引当数
                        if (bbl.Z_Set(mGrid2.g_DArray[mHikiateCurrentRow].InstructionSu) > (bbl.Z_Set(mGrid2.g_DArray[mHikiateCurrentRow].ReserveSu) + bbl.Z_Set(updateSu)))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E222");
                            return false;
                        }
                        mGrid2.g_DArray[mHikiateCurrentRow].ReserveSu = (bbl.Z_Set(mGrid2.g_DArray[mHikiateCurrentRow].ReserveSu) + bbl.Z_Set(updateSu)).ToString();
                        mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

                        // 引当可能数を表示
                        mGrid.g_DArray[row].AllowableSu = (bbl.Z_Set(mGrid.g_DArray[row].SelectAllowableSu) - updateSu).ToString();
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        //表示用ワークテーブルに反映
                        this.UpdateWorkTable(row, updateSu);

                        //前回との差数をワークテーブルに更新するため、今回入力数を退避
                        mGrid.g_DArray[row].SelectReserveSu = mGrid.g_DArray[row].ReserveSu;
                        mGrid.g_DArray[row].SelectAllowableSu = mGrid.g_DArray[row].AllowableSu;

                        //フッタ部引当数
                        this.CalcKei();

                    }
                    break;

            }
           
            return true;
        }

        /// <summary>
        /// 受注状況明細入力チェック
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="chkAll"></param>
        /// <returns></returns>
        private bool CheckGrid2(int col, int row, bool chkAll = false)
        {

            switch (col)
            {
                case (int)ClsGridHikiate.ColNO.ReserveSu:
                    //入力できる場合(When input is possible)
                    if (mGrid2.g_MK_State[col, row].Cell_Enabled && !mGrid2.g_MK_State[col, row].Cell_ReadOnly)
                    {
                        //画面の内容を配列へセット
                        mGrid2.S_DispToArray(Vsb_Mei_1.Value);

                        //出荷指示数＞引当数
                        if (bbl.Z_Set(mGrid2.g_DArray[row].InstructionSu) > bbl.Z_Set(mGrid2.g_DArray[row].ReserveSu))
                        {                        
                            bbl.ShowMessage("E222");
                            return false;
                        }

                        //引当数＞受注数

                        //同一受注への引当数を算出
                        decimal allReserveSu = 0;
                        foreach (DataRow dr in juchuuTbl.Rows)
                        {
                            if (dr["JuchuuNO"].ToString() == mGrid2.g_DArray[row].JuchuuNO && bbl.Z_Set(dr["JuchuuRows"]) == bbl.Z_Set(mGrid2.g_DArray[row].JuchuuRows))
                            {
                                if (dr["StockNO"].ToString() != mGrid.g_DArray[mZaikoCurrentRow].StockNO)
                                {
                                    allReserveSu += bbl.Z_Set(dr["ReserveSu"]);
                                }
                            }
                        }

                        if (allReserveSu  + bbl.Z_Set(mGrid2.g_DArray[row].ReserveSu) > bbl.Z_Set(mGrid2.g_DArray[row].JuchuuSuu))
                        {
                            bbl.ShowMessage("E223", bbl.Z_SetStr(allReserveSu + bbl.Z_Set(mGrid2.g_DArray[row].ReserveSu)), bbl.Z_SetStr(mGrid2.g_DArray[row].JuchuuSuu));
                            return false;
                        }

                        //在庫情報引当数
                        decimal updateSu = 0;
                        for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
                        {
                            if (!string.IsNullOrWhiteSpace(mGrid2.g_DArray[RW].JuchuuNO))
                            {
                                updateSu += bbl.Z_Set(mGrid2.g_DArray[RW].ReserveSu) - bbl.Z_Set(mGrid2.g_DArray[RW].SelectReserveSu);
                            }
                        }

                        //在庫情報.出荷指示数＞受注情報.引当数
                        if (bbl.Z_Set(mGrid.g_DArray[mZaikoCurrentRow].InstructionSu) > (bbl.Z_Set(mGrid.g_DArray[mZaikoCurrentRow].SelectReserveSu) + bbl.Z_Set(updateSu)))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E222");
                            return false;
                        }

                        mGrid.g_DArray[mZaikoCurrentRow].ReserveSu = (bbl.Z_Set(mGrid.g_DArray[mZaikoCurrentRow].SelectReserveSu) + bbl.Z_Set(updateSu)).ToString();
                        mGrid.g_DArray[mZaikoCurrentRow].AllowableSu = (bbl.Z_Set(mGrid.g_DArray[mZaikoCurrentRow].SelectAllowableSu) - bbl.Z_Set(updateSu)).ToString();
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        //ワークテーブルに反映
                        this.UpdateWorkTable(row,updateSu);

                        //フッタ部引当数
                        this.CalcKei();
                    }
                    break;

            }

            return true;
        }

        /// <summary>
        /// エラー時フォーカスセット
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            w_Ctrl = juchuuControls[(int)JIndex.JuchuuDateFrom];

            //IMT_DMY_0.Focus();       // エラー内容をハイライトにするため　→エラーメッセージが何度も出るため、削除
            w_Ret = mGrid.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        private void ERR_FOCUS_GRID2_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_1.Value;

            w_Ctrl = zaikoControls[(int)ZIndex.OrderDateFrom];

            //IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid2.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_1, pRow, pCol);

        }

        private void CalcKei()
        {
            decimal reserveSu;

            //在庫情報引当数
            if (zaikoDispTbl.Rows.Count > 0)
            {
                reserveSu = 0;
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO))
                    {
                        reserveSu += bbl.Z_Set(mGrid.g_DArray[RW].ReserveSu);
                    }
                }
                lblZReserveSu.Text = string.Format("{0:#,##0}", reserveSu);

            }

            //受注情報引当数
            if (juchuuDispTbl.Rows.Count > 0)
            {
                reserveSu = 0;
                for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
                {
                    if (!string.IsNullOrWhiteSpace(mGrid2.g_DArray[RW].JuchuuNO))
                    {
                        reserveSu += bbl.Z_Set(mGrid2.g_DArray[RW].ReserveSu);
                    }
                }
                lblJReserveSu.Text = string.Format("{0:#,##0}", reserveSu);

            }

        }

        /// <summary>
        /// 商品情報クリア処理
        /// </summary>
        private void ClearSkuInfo()
        {
            mAdminNO = "";
            lblJANCD.Text = "";
            lblSKUCD.Text = "";
            lblSKUName.Text = "";
            lblSizeName.Text = "";
            lblColorName.Text = "";
        }

        /// <summary>
        /// 在庫状況ヘッダ・フッタクリア処理
        /// </summary>
        private void ClearZaikoInfo()
        {
            lblJuchuuNO.Text = "";
            lblCustomerCD.Text = "";
            lblCustomerName.Text = "";
            lblJuchuuSuu.Text = "";
            lblZReserveSu.Text = "";
        }

        /// <summary>
        /// 受注状況ヘッダ・フッタクリア処理
        /// </summary>
        private void ClearJuchuuInfo()
        {
            lblSoukoName.Text = "";
            lblArrivalPlanDate.Text = "";
            lblArrivalDate.Text = "";
            lblAllowableSu.Text = "";
            lblJReserveSu.Text = "";
        }

        /// <summary>
        /// ワークテーブル作成
        /// </summary>
        private void CreateTable()
        {
            zaikoDispTbl = new DataTable();
            Para_Add_Zaiko(zaikoDispTbl);

            juchuuDispTbl = new DataTable();
            Para_Add_Juchuu(juchuuDispTbl);

        }

        /// <summary>
        /// 在庫ワークテーブル項目追加
        /// </summary>
        /// <param name="dt"></param>
        private void Para_Add_Zaiko(DataTable dt)
        {
            dt.Columns.Add("DispArrivalPlanDate", typeof(string));
            dt.Columns.Add("OrderNO", typeof(string));
            dt.Columns.Add("VendorName", typeof(string));
            dt.Columns.Add("StockSu", typeof(Int32));
            dt.Columns.Add("PlanSu", typeof(Int32));
            dt.Columns.Add("AllowableSu", typeof(Int32));
            dt.Columns.Add("ReserveSu", typeof(Int32));
            dt.Columns.Add("InstructionSu", typeof(Int32));
            //隠し
            dt.Columns.Add("SoukoCD", typeof(string));
            dt.Columns.Add("SoukoName", typeof(string));
            dt.Columns.Add("ArrivalPlanDate", typeof(string));
            dt.Columns.Add("ArrivalDate", typeof(string));
            dt.Columns.Add("StockNO", typeof(string));
            dt.Columns.Add("SelectReserveSu", typeof(Int32));
            dt.Columns.Add("SelectAllowableSu", typeof(Int32));
            dt.Columns.Add("AnotherStoreAllowableSu", typeof(Int32));

            //dt.Columns.Add("SoukoCD", typeof(string));
            //dt.Columns.Add("ArrivalPlanNO", typeof(string));
            //dt.Columns.Add("SKUCD", typeof(string));
            //dt.Columns.Add("AdminNO", typeof(Int32));
            //dt.Columns.Add("JanCD", typeof(string));
            //dt.Columns.Add("ArrivalYetFLG", typeof(Byte));
            //dt.Columns.Add("OrderDate", typeof(DateTime));
            //dt.Columns.Add("ArrivalPlanKBN", typeof(Byte));
            //dt.Columns.Add("OrderCD", typeof(string));
        }

        /// <summary>
        /// 受注ワークテーブル項目追加
        /// </summary>
        /// <param name="dt"></param>
        private void Para_Add_Juchuu(DataTable dt)
        {
            //dt.Columns.Add("Seq", typeof(Int32));
            //dt.Columns.Add("StockNO", typeof(string));
            //dt.Columns.Add("ReserveNO", typeof(string));
            //dt.Columns.Add("ReserveKBN", typeof(Byte));
            dt.Columns.Add("JuchuuDate", typeof(string));
            dt.Columns.Add("CustomerCD", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("JuchuuNO", typeof(string));
            dt.Columns.Add("JuchuuSuu", typeof(Int32));
            dt.Columns.Add("ReserveSu", typeof(Int32));
            dt.Columns.Add("InstructionSu", typeof(Int32));
            //隠し
            dt.Columns.Add("JuchuuRows", typeof(Int32));
            dt.Columns.Add("SelectReserveSu", typeof(Int32));
            dt.Columns.Add("AllReserveSu", typeof(Int32));
            dt.Columns.Add("AllShippingSu", typeof(Int32));
            dt.Columns.Add("JuchuuKBN", typeof(Byte));

            //dt.Columns.Add("SoukoCD", typeof(string));
            //dt.Columns.Add("SKUCD", typeof(string));
            //dt.Columns.Add("AdminNO", typeof(Int32));
            //dt.Columns.Add("JanCD", typeof(string));       
            //dt.Columns.Add("ShippingOrderNO", typeof(string));
            //dt.Columns.Add("ShippingOrderRows", typeof(Int32));
            //dt.Columns.Add("ShippingSu", typeof(Int32));
            //dt.Columns.Add("ReturnKBN", typeof(Byte));
            //dt.Columns.Add("OriginalReserveNO", typeof(string));
        }

        /// <summary>
        /// 表示用ワークテーブル追加処理
        /// </summary>
        /// <param name="isFirst">初期表示フラグ</param>
        private void InsertDispTable(bool isFirst = false)
        {
            //表示用テーブルクリア
            zaikoDispTbl.Clear();
            juchuuDispTbl.Clear();

            mZaikoCurrentRow = -1;
            mHikiateCurrentRow = -1;

            Btn_F10.Enabled = false;
            
            if (isFirst)
            {
                //データ抽出
                this.GetEntity(); 
                zaikoTbl = hhbl.D_Stock_SelectForHikiateZaiko(hhe);
                juchuuTbl = hhbl.D_Stock_SelectForHikiateJuchuu(hhe);

                // 初期表示処理にて在庫テーブルがない場合、受注モードに切り替える
                if (zaikoTbl.Rows.Count == 0)
                {
                    //受注から引当モード
                    this.ChangeOperationMode(EOpeMode.JUCHUU);
                }
            }

            //ワークテーブルより表示用テーブルにセット
            if (mOpeMode == EOpeMode.ZAIKO)
            {
                int rowNo = 0;
                foreach (DataRow row in zaikoTbl.Rows)
                {
                    rowNo++;

                    zaikoDispTbl.Rows.Add(
                         row["ArrivalPlanDate"].ToString() != "" ? row["ArrivalPlanDate"].ToString() : row["ArrivalDate"].ToString()
                       , row["OrderNO"].ToString()
                       , row["VendorName"].ToString()
                        , bbl.Z_Set(row["StockSu"])
                        , bbl.Z_Set(row["PlanSu"])
                        , bbl.Z_Set(row["AllowableSu"])
                        , bbl.Z_Set(row["ReserveSu"])
                        , bbl.Z_Set(row["InstructionSu"])
                        , row["SoukoCD"].ToString()
                        , row["SoukoName"].ToString()
                        , row["ArrivalPlanDate"].ToString() == "" ? null : row["ArrivalPlanDate"].ToString()
                        , row["ArrivalDate"].ToString() == "" ? null : row["ArrivalDate"].ToString()
                        , row["StockNO"].ToString()
                        , bbl.Z_Set(row["SelectReserveSu"])
                        , bbl.Z_Set(row["SelectAllowableSu"])
                        , bbl.Z_Set(row["AnotherStoreAllowableSu"])                        

                        //, row["SoukoCD"].ToString()
                        //, row["ArrivalPlanNO"].ToString()
                        //, row["SKUCD"].ToString()
                        //, bbl.Z_Set(row["AdminNO"])
                        //, row["JanCD"].ToString()
                        //, bbl.Z_Set(row["ArrivalYetFLG"])
                        //, row["OrderDate"].ToString() == "" ? null : row["OrderDate"].ToString()
                        //, bbl.Z_Set(row["ArrivalPlanKBN"])
                        //, row["OrderCD"].ToString()
                        );
                }
            }
            else
            {
                int rowNo = 0;
                string juchuuNo = string.Empty;
                int juchuuRows = 0;
                foreach (DataRow row in juchuuTbl.Rows)
                {
                    if (juchuuNo != row["JuchuuNO"].ToString() || juchuuRows != bbl.Z_Set(row["JuchuuRows"]))
                    {
                        rowNo++;

                        decimal reserveSu = bbl.Z_Set(juchuuTbl.Compute("SUM(ReserveSu)", "JuchuuNO = '" + row["JuchuuNO"].ToString() + "' and JuchuuRows = " + bbl.Z_Set(row["JuchuuRows"])));
                        decimal instructionSu = bbl.Z_Set(juchuuTbl.Compute("SUM(InstructionSu)", "JuchuuNO = '" + row["JuchuuNO"].ToString() + "' and JuchuuRows = " + bbl.Z_Set(row["JuchuuRows"])));

                        juchuuDispTbl.Rows.Add(
                           row["JuchuuDate"].ToString() == "" ? null : row["JuchuuDate"].ToString()
                         , row["CustomerCD"].ToString()
                         , row["CustomerName"].ToString()
                         , row["JuchuuNO"].ToString()
                         , bbl.Z_Set(row["JuchuuSuu"])
                         , reserveSu
                         , instructionSu
                         , bbl.Z_Set(row["JuchuuRows"])
                         , reserveSu
                         , bbl.Z_Set(row["AllReserveSu"])
                         , bbl.Z_Set(row["AllShippingSu"])
                         , bbl.Z_Set(row["JuchuuKBN"])

                         // , row["StockNO"].ToString()
                         // , row["ReserveNO"].ToString()
                         // , bbl.Z_Set(row["ReserveKBN"])
                         //, row["SoukoCD"].ToString()
                         //, row["SKUCD"].ToString()
                         //, bbl.Z_Set(row["AdminNO"])
                         //, row["JanCD"].ToString()
                         //, row["ShippingOrderNO"].ToString()
                         //, bbl.Z_Set(row["ShippingOrderRows"])
                         //, bbl.Z_Set(row["ShippingSu"])
                         //, bbl.Z_Set(row["ReturnKBN"])
                         //, row["OriginalReserveNO"].ToString() 

                         );
                    }
                    juchuuNo = row["JuchuuNO"].ToString();
                    juchuuRows = (int)bbl.Z_Set(row["JuchuuRows"]);
                }
            }
        }


        /// <summary>
        /// ワークテーブル更新
        /// </summary>
        /// <param name="currentRow"></param>
        /// <param name="updateSu"></param>
        private void UpdateWorkTable(int currentRow, decimal updateSu)
        {
            if (mOpeMode == EOpeMode.ZAIKO)
            {
                //在庫ワークテーブル更新
                string stockNO = mGrid.g_DArray[mZaikoCurrentRow].StockNO;
                string juchuuNO = mGrid2.g_DArray[currentRow].JuchuuNO;
                int juchuuRows = (int)bbl.Z_Set(mGrid2.g_DArray[currentRow].JuchuuRows);

                foreach (DataRow row in zaikoTbl.Rows)
                {
                    if (row["StockNO"].ToString() == stockNO)
                    {                       
                        //受注状況の読込時引当数と引当数との差の合計を更新
                        row["ReserveSu"] = bbl.Z_Set(row["SelectReserveSu"]) + updateSu;
                        row["AllowableSu"] = bbl.Z_Set(row["SelectAllowableSu"]) - updateSu;
                        //row["AnotherStoreAllowableSu"] = bbl.Z_Set(row["AnotherStoreAllowableSu"]) - updateSu;
                    }
                }

                //受注ワークテーブル
                //if (bbl.Z_Set(mGrid2.g_DArray[currentRow].ReserveSu) != bbl.Z_Set(mGrid2.g_DArray[currentRow].SelectReserveSu))
                //{
                    bool blnExist = false;
                    short reserveKbn = 0;

                    foreach (DataRow row in juchuuTbl.Rows)
                    {
                        if (row["JuchuuNO"].ToString() == juchuuNO && bbl.Z_Set(row["JuchuuRows"]) == juchuuRows)
                        {
                            if (row["StockNO"].ToString() == stockNO)
                            {
                                row["ReserveSu"] = bbl.Z_Set(mGrid2.g_DArray[currentRow].ReserveSu);
                                blnExist = true;
                            }

                            reserveKbn = (short)bbl.Z_Set(row["ReserveKBN"]);

                        }
                    }

                    if (blnExist == false)
                    {
                        //行追加
                        DataRow dr = juchuuTbl.NewRow();
                        dr["SEQ"] = bbl.Z_Set(juchuuTbl.Rows.Count) + 1;
                        dr["StockNO"] = stockNO;
                        dr["ReserveKBN"] = reserveKbn;
                        dr["JuchuuNO"] = juchuuNO;
                        dr["JuchuuRows"] = juchuuRows;
                        dr["SoukoCD"] = mGrid.g_DArray[mZaikoCurrentRow].SoukoCD;
                        dr["SKUCD"] = headControls[(int)HIndex.SKUCD].Text;
                        dr["AdminNO"] = mAdminNO;
                        dr["JanCD"] = headControls[(int)HIndex.JANCD].Text; ;
                        dr["ReserveSu"] = bbl.Z_Set(mGrid2.g_DArray[currentRow].ReserveSu);
                        dr["SelectReserveSu"] = 0;

                        juchuuTbl.Rows.Add(dr);


                    }
                //}
            }
            else
            {
                
                string stockNO = mGrid.g_DArray[currentRow].StockNO;
                string juchuuNO = mGrid2.g_DArray[mHikiateCurrentRow].JuchuuNO;
                int juchuuRows = (int)bbl.Z_Set(mGrid2.g_DArray[mHikiateCurrentRow].JuchuuRows);

                if (updateSu != 0)
                {
                    //在庫ワークテーブル更新
                    foreach (DataRow row in zaikoTbl.Rows)
                    {
                        if (row["StockNO"].ToString() == stockNO)
                        {  
                            //各行の前回入力時との差数を更新
                            row["ReserveSu"] = bbl.Z_Set(row["ReserveSu"]) + updateSu;
                            row["AllowableSu"] = bbl.Z_Set(row["AllowableSu"]) - updateSu;
                            //row["AnotherStoreAllowableSu"] = bbl.Z_Set(row["AnotherStoreAllowableSu"]) - updateSu;
                        }
                    }

                    //受注ワークテーブル更新                
                    bool blnExist = false;
                    short reserveKbn = 0;

                    foreach (DataRow row in juchuuTbl.Rows)
                    {
                        if (row["JuchuuNO"].ToString() == juchuuNO && bbl.Z_Set(row["JuchuuRows"]) == juchuuRows)
                        {
                            if (row["StockNO"].ToString() == stockNO)
                            {
                                row["ReserveSu"] = bbl.Z_Set(row["ReserveSu"]) + updateSu;
                                blnExist = true;
                            }

                            reserveKbn = (short)bbl.Z_Set(row["ReserveKBN"]);
                        }
                    }

                    if (blnExist == false)
                    {
                        //行追加
                        DataRow dr = juchuuTbl.NewRow();
                        dr["SEQ"] = bbl.Z_Set(juchuuTbl.Rows.Count) + 1;
                        dr["StockNO"] = stockNO;
                        dr["ReserveKBN"] = reserveKbn;
                        dr["JuchuuNO"] = juchuuNO;
                        dr["JuchuuRows"] = juchuuRows;
                        dr["SoukoCD"] = mGrid.g_DArray[currentRow].SoukoCD;
                        dr["SKUCD"] = headControls[(int)HIndex.SKUCD].Text;
                        dr["AdminNO"] = mAdminNO;
                        dr["JanCD"] = headControls[(int)HIndex.JANCD].Text; ;
                        dr["ReserveSu"] = bbl.Z_Set(mGrid.g_DArray[currentRow].ReserveSu);
                        dr["SelectReserveSu"] = 0;

                        juchuuTbl.Rows.Add(dr);
                    }
                }
            }
        }


        /// <summary>
        /// 明細表示処理（ヘッダ部表示ボタン押下）
        /// </summary>
        protected new void ExecDisp()
        {

            for (int i = 0; i < headControls.Length; i++)
                if (CheckHead(i) == false)
                {
                    headControls[i].Select();
                    headControls[i].Focus();
                    return;
                }

            if (mAdminNO == "")
            {
                bbl.ShowMessage("E102");
                headControls[(int)HIndex.SKUCD].Focus();
                return;
            }

            //在庫から引当モード
            this.ChangeOperationMode(EOpeMode.ZAIKO);

            //表示用ワークテーブル追加
            this.InsertDispTable(true);

            if (zaikoTbl.Rows.Count == 0 && juchuuTbl.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                headControls[(int)HIndex.SKUCD].Focus();
                return;
            }

            //ヘッダ部ロック
            this.Scr_Lock(0);

            //在庫明細表示     
            this.DispDetail((short)mOpeMode, true);

            //引当(F3)を可能にする
            this.SetFuncKey(this, 2, true);
            //zaikoControls[(int)ZIndex.OrderDateFrom].Focus();


        }

        /// <summary>
        /// 明細表示処理（データ部表示ボタン押下）
        /// </summary>
        private void ExecDispDetail()
        {
            if (mOpeMode == EOpeMode.ZAIKO)
            {
                for (int i = 0; i < zaikoControls.Length; i++)
                {
                    if (CheckZaiko(i) == false)
                    {
                        ((TextBox)zaikoControls[i]).SelectAll();
                        zaikoControls[i].Focus();
                        return;
                    }
                }
            }
            else
            {
                for (int i = 0; i < juchuuControls.Length; i++)
                { 
                    if (CheckJuchuu(i) == false)
                    {
                        ((TextBox)juchuuControls[i]).SelectAll();
                        juchuuControls[i].Focus();
                        return;
                    }
                }

                if (ChkNotReserve.Checked == false && ChkReserved.Checked == false)
                {
                    bbl.ShowMessage("E111");
                    ChkNotReserve.Focus();
                    return;
                }

                if (ChkJuchuuKBN1.Checked == false && ChkJuchuuKBN2.Checked == false && ChkJuchuuKBN3.Checked == false && ChkJuchuuKBN4.Checked == false)
                {
                    bbl.ShowMessage("E111");
                    ChkJuchuuKBN3.Focus();
                    return;
                }
            }

            //表示用ワークテーブル作成
            this.InsertDispTable();

            this.ClearZaikoInfo();
            this.ClearJuchuuInfo();

            S_Clear_Grid();   //画面クリア（明細部）
            S_Clear_Grid2();   //画面クリア（明細部）

            if (mOpeMode == EOpeMode.ZAIKO)
            {
                if (zaikoTbl.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");                    
                    zaikoControls[(int)ZIndex.OrderDateFrom].Focus();
                    return;
                }

            }
            else
            {
                if (juchuuTbl.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");                    
                    juchuuControls[(int)JIndex.JuchuuDateFrom].Focus();
                    return;
                }
            }
            
            //在庫明細表示     
            this.DispDetail((short)mOpeMode, true);
        }

        /// <summary>
        /// 展開処理
        /// </summary>
        private void ExecExpand()
        {
            //Control w_Act = this.ActiveControl;
            Control w_Act = this.PreviousCtrl;
            int w_Row;

            if (mOpeMode == EOpeMode.ZAIKO)
            {
                if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                w_Row = w_CtlRow + Vsb_Mei_0.Value;
                mZaikoCurrentRow = w_Row;

                lblSoukoName.Text = mGrid.g_DArray[w_Row].SoukoName;
                lblArrivalPlanDate.Text = mGrid.g_DArray[w_Row].ArrivalPlanDate;
                lblArrivalDate.Text = mGrid.g_DArray[w_Row].ArrivalDate;
                lblAllowableSu.Text = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[w_Row].AllowableSu)); 

                //ワークテーブルより受注明細表示用テーブルセット
                int rowNo = 0;
                string juchuuNo = string.Empty;
                int juchuuRows = 0;

                string stockNO = mGrid.g_DArray[w_Row].StockNO;

                juchuuDispTbl.Clear();

                foreach (DataRow row in juchuuTbl.Select("", "JuchuuDate, JuchuuNO, JuchuuRows"))
                {
                    if (juchuuNo != row["JuchuuNO"].ToString() || juchuuRows != bbl.Z_Set(row["JuchuuRows"]))
                    {
                        rowNo++;

                        //引当数、出荷指示数は同じ在庫番号のもののみ
                        //読込時引当数はデータ取得時のもの
                        decimal reserveSu = bbl.Z_Set(juchuuTbl.Compute("SUM(ReserveSu)", "JuchuuNO = '" + row["JuchuuNO"].ToString() + "' and JuchuuRows = " + bbl.Z_Set(row["JuchuuRows"]) + " and StockNO = '" + stockNO + "'"));
                        decimal instructionSu = bbl.Z_Set(juchuuTbl.Compute("SUM(InstructionSu)", "JuchuuNO = '" + row["JuchuuNO"].ToString() + "' and JuchuuRows = " + bbl.Z_Set(row["JuchuuRows"]) + " and StockNO = '" + stockNO + "'"));
                        decimal selectReserveSu = bbl.Z_Set(juchuuTbl.Compute("SUM(SelectReserveSu)", "JuchuuNO = '" + row["JuchuuNO"].ToString() + "' and JuchuuRows = " + bbl.Z_Set(row["JuchuuRows"]) + " and StockNO = '" + stockNO + "'"));

                        juchuuDispTbl.Rows.Add(
                           row["JuchuuDate"].ToString() == "" ? null : row["JuchuuDate"].ToString()
                         , row["CustomerCD"].ToString()
                         , row["CustomerName"].ToString()
                         , row["JuchuuNO"].ToString()
                         , bbl.Z_Set(row["JuchuuSuu"])
                         , reserveSu
                         , instructionSu
                         , bbl.Z_Set(row["JuchuuRows"])
                         , selectReserveSu
                         , bbl.Z_Set(row["AllReserveSu"])
                         , bbl.Z_Set(row["AllShippingSu"])
                         , bbl.Z_Set(row["JuchuuKBN"])
                         );
                    }

                    juchuuNo = row["JuchuuNO"].ToString();
                    juchuuRows = (int)bbl.Z_Set(row["JuchuuRows"]);
                }

                //受注明細表示
                this.DispDetail((short)EOpeMode.JUCHUU, false);

            }
            else
            {
                if (mGrid2.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                w_Row = w_CtlRow + Vsb_Mei_1.Value;
                mHikiateCurrentRow = w_Row;

                lblJuchuuNO.Text = mGrid2.g_DArray[w_Row].JuchuuNO;
                lblCustomerCD.Text = mGrid2.g_DArray[w_Row].CustomerCD;
                lblCustomerName.Text = mGrid2.g_DArray[w_Row].CustomerName;
                lblJuchuuSuu.Text = string.Format("{0:#,##0}", bbl.Z_Set(mGrid2.g_DArray[w_Row].JuchuuSuu)); 

                //ワークテーブルより在庫明細表示用テーブルセット
                int rowNo = 0;

                string juchuuNO = mGrid2.g_DArray[w_Row].JuchuuNO;
                int juchuuRows = (int)bbl.Z_Set(mGrid2.g_DArray[w_Row].JuchuuRows);

                zaikoDispTbl.Clear();

                foreach (DataRow row in zaikoTbl.Rows)
                {
                    
                    rowNo++;

                    //引当数、出荷指示数は同じ受注番号のもののみ
                    //読込時引当数は前回内容をセット
                    decimal reserveSu = bbl.Z_Set(juchuuTbl.Compute("SUM(ReserveSu)", "JuchuuNO = '" + juchuuNO + "' and JuchuuRows = " + juchuuRows + " and StockNO = '" + row["StockNO"].ToString() + "'"));
                    decimal instructionSu = bbl.Z_Set(juchuuTbl.Compute("SUM(InstructionSu)", "JuchuuNO = '" + juchuuNO + "' and JuchuuRows = " + juchuuRows + " and StockNO = '" + row["StockNO"].ToString() + "'"));
                    //decimal selectReserveSu = bbl.Z_Set(juchuuTbl.Compute("SUM(SelectReserveSu)", "JuchuuNO = '" + juchuuNO + "' and JuchuuRows = " + juchuuRows + " and StockNO = '" + row["StockNO"].ToString() + "'"));

                    zaikoDispTbl.Rows.Add(
                          row["ArrivalPlanDate"].ToString() != "" ? row["ArrivalPlanDate"].ToString() : row["ArrivalDate"].ToString()
                        , row["OrderNO"].ToString()
                        , row["VendorName"].ToString()
                        , bbl.Z_Set(row["StockSu"])
                        , bbl.Z_Set(row["PlanSu"])
                        , bbl.Z_Set(row["AllowableSu"])
                        , reserveSu
                        , instructionSu
                        , row["SoukoCD"].ToString()
                        , row["SoukoName"].ToString()
                        , row["ArrivalPlanDate"].ToString() == "" ? null : row["ArrivalPlanDate"].ToString()
                        , row["ArrivalDate"].ToString() == "" ? null : row["ArrivalDate"].ToString()
                        , row["StockNO"].ToString()
                        , reserveSu
                        , bbl.Z_Set(row["AllowableSu"])
                        , bbl.Z_Set(row["AnotherStoreAllowableSu"])
                       );
                }
                
                //在庫明細表示
                this.DispDetail((short)EOpeMode.ZAIKO, false);

            }
        }

        protected override void ExecSec()
        {
            try
            {
                if (mOpeMode == EOpeMode.JUCHUU)
                {
                    // 明細部  画面の範囲の内容を配列にセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //明細部チェック
                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    {
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].StockNO) == false)
                        {
                            for (int CL = (int)ClsGridZaiko.ColNO.ReserveSu; CL < (int)ClsGridZaiko.ColNO.COUNT; CL++)
                            {
                                if (CheckGrid(CL, RW, true) == false)
                                {
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB(CL, RW);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 明細部  画面の範囲の内容を配列にセット
                    mGrid2.S_DispToArray(Vsb_Mei_1.Value);

                    //明細部チェック
                    for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
                    {
                        if (string.IsNullOrWhiteSpace(mGrid2.g_DArray[RW].JuchuuNO) == false)
                        {
                            for (int CL = (int)ClsGridHikiate.ColNO.ReserveSu; CL < (int)ClsGridHikiate.ColNO.COUNT; CL++)
                            {
                                if (CheckGrid2(CL, RW, true) == false)
                                {
                                    //Focusセット処理
                                    ERR_FOCUS_GRID2_SUB(CL, RW);
                                    return;
                                }
                            }
                        }
                    }
                }

                //Ｑ１０１		
                if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                    return;

                hhe = new HikiateHenkouNyuuryoku_Entity
                {
                    SKUCD = headControls[(int)HIndex.SKUCD].Text,
                    StoreCD = CboStoreCD.SelectedValue.ToString(),
                    Operator = InOperatorCD,
                    PC = InPcID

                };

                //更新処理
                bool ret = hhbl.PRC_HikiateHenkouNyuuryoku(hhe, zaikoTbl, juchuuTbl);

                if (ret)
                    bbl.ShowMessage("I101");
                else
                    bbl.ShowMessage("S002");

                //在庫モードに
                this.ChangeOperationMode(EOpeMode.ZAIKO);

                this.Scr_Clr(0);
                this.Scr_Lock(1);
                headControls[(int)HIndex.SKUCD].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// 店舗受注入力起動
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        private void ExecTempoJuchuu()
        {
            try
            {
                int w_Row;
                //Control w_Act = this.ActiveControl;
                Control w_Act = this.PreviousCtrl;
                if (w_Act == null)
                    return;

                if (mGrid2.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
                    return;

                w_Row = w_CtlRow + Vsb_Mei_1.Value;

                //「受注入力」を照会モードで起動（引数：該当行の受注番号）
                string juchuuNo = mGrid2.g_DArray[w_Row].JuchuuNO;

                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoJuchuuNyuuryoku;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + juchuuNo;
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


        /// <summary>
        /// 引当処理
        /// </summary>
        protected void ExecHikiate()
        {
            try
            {
                //Ｑ３１７		
                if (bbl.ShowMessage("Q317") != DialogResult.Yes)
                    return;

                hhe = new HikiateHenkouNyuuryoku_Entity
                {
                    StoreCD = CboStoreCD.SelectedValue.ToString(),
                    AdminNO = mAdminNO,
                    Operator = InOperatorCD,
                    PC = InPcID
                };

                //引当更新処理
                bool ret = hhbl.ALL_Hikiate(hhe);
                if (!ret)
                {
                    bbl.ShowMessage("S002");
                    return;
                }

                //在庫から引当モード
                this.ChangeOperationMode(EOpeMode.ZAIKO);

                //表示用ワークテーブル追加
                this.InsertDispTable(true);

                if (zaikoTbl.Rows.Count == 0)
                {
                    bbl.ShowMessage("E128");
                    headControls[(int)HIndex.SKUCD].Focus();
                    return;
                }

                //在庫明細表示     
                this.DispDetail((short)EOpeMode.ZAIKO, true);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("GyoNO", typeof(int));
            dt.Columns.Add("RackNO", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("ActualQuantity", typeof(int));
          
            dt.Columns.Add("UpdateFlg", typeof(int));
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private HikiateHenkouNyuuryoku_Entity GetEntity()
        {
            hhe = new HikiateHenkouNyuuryoku_Entity
            {
                AdminNO = mAdminNO,
                StoreCD = CboStoreCD.SelectedValue.ToString(),

                OrderDateFrom = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.OrderDateFrom].Text : "",
                OrderDateTo = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.OrderDateTo].Text : "",
                ArrivalPlanDateFrom = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.ArrivalPlanDateFrom].Text : "",
                ArrivalPlanDateTo = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.ArrivalPlanDateTo].Text : "",
                ArrivalDateFrom = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.ArrivalDateFrom].Text : "",
                ArrivalDateTo = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.ArrivalDateTo].Text: "",
                OrderCD = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.OrderCD].Text : "",
                OrderNO = mOpeMode == EOpeMode.ZAIKO ? zaikoControls[(int)ZIndex.OrderNO].Text : "",

                JuchuuDateFrom = mOpeMode == EOpeMode.JUCHUU ? juchuuControls[(int)JIndex.JuchuuDateFrom].Text : "",
                JuchuuDateTo = mOpeMode == EOpeMode.JUCHUU ? juchuuControls[(int)JIndex.JuchuuDateTo].Text : "",
                CustomerCD = mOpeMode == EOpeMode.JUCHUU ? juchuuControls[(int)JIndex.Customer].Text : "",
                JuchuuNO = mOpeMode == EOpeMode.JUCHUU ? juchuuControls[(int)JIndex.JuchuuNo].Text : "",

            };

            if (ChkNotReserve.Checked)
                hhe.ChkNotReserve = "1";
            else
                hhe.ChkNotReserve = "0";

            if (ChkReserved.Checked)
                hhe.ChkReserved = "1";
            else
                hhe.ChkReserved = "0";

            if (ChkJuchuuKBN1.Checked)
                hhe.JuchuuKBN1 = "1";
            else
                hhe.JuchuuKBN1 = "0";

            if (ChkJuchuuKBN2.Checked)
                hhe.JuchuuKBN2 = "2";
            else
                hhe.JuchuuKBN2 = "0";

            if (ChkJuchuuKBN3.Checked)
                hhe.JuchuuKBN3 = "3";
            else
                hhe.JuchuuKBN3 = "0";

            if (ChkJuchuuKBN4.Checked)
                hhe.JuchuuKBN4 = "4";
            else
                hhe.JuchuuKBN4 = "0";

            return hhe;
        }
        
        /// <summary>
        /// 処理モード変更
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeOperationMode(EOpeMode mode)
        {
            mOpeMode = mode;
            switch (mOpeMode)
            {
                case EOpeMode.ZAIKO:
                    //OperationMode = EOperationMode.SHOW;  //モードラベルの色を青にする
                    ModeColor = Color.FromArgb(0, 176, 240);
                    ModeText = "在庫";
                    Scr_Clr(1);
                    lblArrow.Text = "▶";
                    lblArrow.ForeColor = Color.FromArgb(0, 176, 240);
                    break;

                case EOpeMode.JUCHUU:
                    //OperationMode = EOperationMode.UPDATE;  //モードラベルの色を黄色にする
                    ModeColor = Color.FromArgb(255, 255, 0);
                    ModeText = "受注";
                    Scr_Clr(2);
                    lblArrow.Text = "◀";
                    lblArrow.ForeColor = Color.FromArgb(255, 255, 0);
                    break;
            }

            lblZaikoMode.Visible = mOpeMode.Equals(EOpeMode.ZAIKO);
            lblJuchuuMode.Visible = mOpeMode.Equals(EOpeMode.JUCHUU);

        }

        /// <summary>
        /// 明細表示
        /// </summary>
        /// <param name="Kbn">0:在庫　1:受注</param>
        /// <param name="readOnly">true:読み取り専用</param>
        private void DispDetail(short Kbn, bool readOnly)
        {
                        
            //明細にデータをセット
            if (Kbn == (int)EOpeMode.ZAIKO)
            {
                int i = 0;
                m_dataCnt = 0;

                foreach (DataRow row in zaikoDispTbl.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return;
                    }

                    mGrid.g_DArray[i].DispArrivalPlanDate = row["ArrivalDate"].ToString() != "" ? row["ArrivalDate"].ToString() : row["ArrivalPlanDate"].ToString();
                    mGrid.g_DArray[i].OrderNO = row["OrderNO"].ToString();
                    mGrid.g_DArray[i].VendorName = row["VendorName"].ToString();
                    mGrid.g_DArray[i].StockSu = bbl.Z_SetStr(row["StockSu"]);
                    mGrid.g_DArray[i].PlanSu = bbl.Z_SetStr(row["PlanSu"]);
                    mGrid.g_DArray[i].AllowableSu = bbl.Z_SetStr(row["AllowableSu"]);
                    mGrid.g_DArray[i].ReserveSu = bbl.Z_SetStr(row["ReserveSu"]);
                    mGrid.g_DArray[i].InstructionSu = bbl.Z_SetStr(row["InstructionSu"]);

                    mGrid.g_DArray[i].SoukoCD = row["SoukoCD"].ToString();
                    mGrid.g_DArray[i].SoukoName = row["SoukoName"].ToString();
                    mGrid.g_DArray[i].ArrivalPlanDate = row["ArrivalPlanDate"].ToString();
                    mGrid.g_DArray[i].ArrivalDate = row["ArrivalDate"].ToString();
                    mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                    mGrid.g_DArray[i].SelectReserveSu = bbl.Z_SetStr(row["SelectReserveSu"]);
                    mGrid.g_DArray[i].SelectAllowableSu = bbl.Z_SetStr(row["SelectAllowableSu"]);
                    mGrid.g_DArray[i].AnotherStoreAllowableSu = bbl.Z_SetStr(row["AnotherStoreAllowableSu"]);

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridZaiko.ColNO.DispArrivalPlanDate, i);
                    i++;

                };

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

                //合計引当数
                this.CalcKei();

                // 入力可の列の設定
                int w_Row;
                for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                {
                    if (m_EnableCnt - 1 < w_Row)
                        break;

                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].StockNO))
                    {
                        if (readOnly)
                            mGrid.g_MK_State[(int)ClsGridZaiko.ColNO.ReserveSu, w_Row].Cell_ReadOnly = true;
                        else
                            mGrid.g_MK_State[(int)ClsGridZaiko.ColNO.ReserveSu, w_Row].Cell_Enabled = true;
                        continue;
                    }
                }

                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridZaiko.ColNO.ReserveSu);
            }
            else
            {
                int i2 = 0;
                m_dataCnt2 = 0;

                foreach (DataRow row in juchuuDispTbl.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i2 > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);
                        return;
                    }

                    mGrid2.g_DArray[i2].JuchuuDate = row["JuchuuDate"].ToString();
                    mGrid2.g_DArray[i2].CustomerCD = row["CustomerCD"].ToString();
                    mGrid2.g_DArray[i2].CustomerName = row["CustomerName"].ToString();
                    mGrid2.g_DArray[i2].JuchuuNO = row["JuchuuNO"].ToString();
                    mGrid2.g_DArray[i2].JuchuuSuu = bbl.Z_SetStr(row["JuchuuSuu"]);
                    mGrid2.g_DArray[i2].ReserveSu = bbl.Z_SetStr(row["ReserveSu"]);
                    mGrid2.g_DArray[i2].InstructionSu = bbl.Z_SetStr(row["InstructionSu"]);

                    mGrid2.g_DArray[i2].JuchuuRows = bbl.Z_SetStr(row["JuchuuRows"]);
                    mGrid2.g_DArray[i2].SelectReserveSu = bbl.Z_SetStr(row["SelectReserveSu"]);
                    mGrid2.g_DArray[i2].AllReserveSu = bbl.Z_SetStr(row["AllReserveSu"]);
                    mGrid2.g_DArray[i2].AllShippingSu = bbl.Z_SetStr(row["AllShippingSu"]);
                    mGrid2.g_DArray[i2].JuchuuKBN = row["JuchuuKBN"].ToString();

                    m_dataCnt2 = i2 + 1;
                    Grid_NotFocus2((int)ClsGridHikiate.ColNO.JuchuuNO, i2);
                    i2++;
                }
                mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);

                //合計引当数
                this.CalcKei();

                // 入力可の列の設定
                int w_Row;
                for (w_Row = mGrid2.g_MK_State.GetLowerBound(1); w_Row <= mGrid2.g_MK_State.GetUpperBound(1); w_Row++)
                {
                    if (m_EnableCnt - 1 < w_Row)
                        break;

                    if (!string.IsNullOrWhiteSpace(mGrid2.g_DArray[w_Row].JuchuuNO))
                    {
                        if (readOnly)
                            mGrid2.g_MK_State[(int)ClsGridHikiate.ColNO.ReserveSu, w_Row].Cell_ReadOnly = true;
                        else
                            mGrid2.g_MK_State[(int)ClsGridHikiate.ColNO.ReserveSu, w_Row].Cell_Enabled = true;
                        continue;
                    }
                }

                //配列の内容を画面にセット
                mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

                //明細の先頭項目へ
                mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_1, Vsb_Mei_1.Value, (int)ClsGridHikiate.ColNO.ReserveSu);

            }
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:在庫　2：受注)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            if (Kbn == 0)
            {
                foreach (Control ctl in headControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

                this.ClearSkuInfo();                
            }

            //在庫状況
            if (Kbn != 2)
            {
                foreach (Control ctl in zaikoControls)
                {                    
                    ctl.Text = "";                    
                }             

            }

            //受注状況
            if (Kbn != 1)
            {
                foreach (Control ctl in juchuuControls)
                {
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = true;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }                
            }

            this.ClearZaikoInfo();
            this.ClearJuchuuInfo();

            S_Clear_Grid();   //画面クリア（明細部）
            S_Clear_Grid2();   //画面クリア（明細部）         

       }

        /// <summary>
        /// 画面入力制御
        /// </summary>
        /// <param name="kbn">0：ヘッダ 1:明細</param>
        private void Scr_Lock(short kbn)
        {
            // ヘッダ部
            foreach (Control ctl in headControls)
            {
                ctl.Enabled = kbn == 0 ? false : true;
            }
            BtnSubF11.Enabled = kbn == 0 ? false : true;
            btnSearchSKUCD.Enabled = kbn == 0 ? false : true;
            btnSearchJANCD.Enabled = kbn == 0 ? false : true;
            
            // 明細部
            //pnl_Zaiko.Enabled = kbn == 0 && mOpeMode == EOpeMode.ZAIKO ? true : false;
            foreach (Control ctl in zaikoControls)
            {
                ctl.Enabled = kbn == 0 && mOpeMode == EOpeMode.ZAIKO ? true : false;
            }
            ScVendor.BtnSearch.Enabled = kbn == 0 && mOpeMode == EOpeMode.ZAIKO ? true : false;
            ScOrderNO.BtnSearch.Enabled = kbn == 0 && mOpeMode == EOpeMode.ZAIKO ? true : false;

            //pnl_Juchuu.Enabled = kbn == 0 && mOpeMode == EOpeMode.JUCHUU ? true : false;
            foreach (Control ctl in juchuuControls)
            {
                ctl.Enabled = kbn == 0 && mOpeMode == EOpeMode.JUCHUU ? true : false;
            }
            ScCustomer.BtnSearch.Enabled = kbn == 0 && mOpeMode == EOpeMode.JUCHUU ? true : false;
            ScJuchuuNo.BtnSearch.Enabled = kbn == 0 && mOpeMode == EOpeMode.JUCHUU ? true : false;

            btn_Zaiko.Enabled = kbn == 0 ? true : false; 
            btn_Juchuu.Enabled = kbn == 0 ? true : false;
            BtnDisp.Enabled = kbn == 0 ? true : false;

            Pnl_Body.Enabled = kbn == 0 ? true : false;
            Pnl_Body2.Enabled = kbn == 0 ? true : false;

            Btn_F10.Enabled = false;
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
                case 0:     //F1:終了
                    {
                        break;
                    }
                case 1:     //F2:受注
                    {
                        this.ExecTempoJuchuu();
                        break;
                    }
                case 2:     //F3:引当
                    {
                        this.ExecHikiate();
                        break;
                    }
                case 3:     //F4
                case 4:     //F5
                    {
                        break;
                    }

                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        //在庫モードに
                        this.ChangeOperationMode(EOpeMode.ZAIKO);

                        this.Scr_Clr(0);
                        this.Scr_Lock(1);
                        headControls[(int)HIndex.SKUCD].Focus();

                        break;
                    }
                case 8: //F9:検索
                    if (previousCtrl.Name.Equals(ckM_SKUCD.Name) || previousCtrl.Name.Equals(ckM_JANCD.Name))
                    {
                        //商品検索
                        SearchData(EsearchKbn.Product, previousCtrl);
                    }
                    break;

                case 9:     //F10:展開
                    {
                        this.ExecExpand();
                        SetFuncKey(this, 11, true);
                        break;
                    }
                case 10:    //F11:表示
                    {
                        if (BtnSubF11.Enabled)
                        {
                            this.ExecDisp();                     
                        }
                        else
                        {
                            this.ExecDispDetail();
                        }
                        SetFuncKey(this, 11, false);
                        break;
                    }

                case 11:    //F12:登録
                    {
                        this.ExecSec();
                        break;
                    }
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
                            int index = Array.IndexOf(headControls, setCtl);

                            switch (index)
                            {
                                case (int)HIndex.JANCD:
                                    headControls[(int)HIndex.JANCD].Text = frmProduct.JANCD;
                                    
                                    break;

                                case (int)HIndex.SKUCD:
                                    headControls[(int)HIndex.SKUCD].Text = frmProduct.SKUCD;
                                    
                                    break;
                            }

                        }
                        setCtl.Focus();
                    }
                    break;
            }

        }

        #region "内部イベント"
        private void HeadControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(headControls, sender);
                    if (index == (int)HIndex.JANCD && string.IsNullOrWhiteSpace(headControls[(int)HIndex.SKUCD].Text))
                        mAdminNO = "";

                    bool ret = CheckHead(index);
                    if (ret)
                    {
                        if (index == (int)HIndex.JANCD)
                        {
                            //BtnSubF11.Focus();
                            this.ExecDisp();
                        }
                        else
                            headControls[index + 1].Focus();

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

        private void HeadControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                SetFuncKeyAll(this, "100001001010");

                int index = Array.IndexOf(headControls, sender);
                switch (index)
                {
                    case (int)HIndex.JANCD:
                    case (int)HIndex.SKUCD:
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

        private void ZaikoControl_Leave(object sender, EventArgs e)
        {
            //Control w_ActCtl;
            //w_ActCtl = (Control)sender;
            //// 背景色
            //w_ActCtl.BackColor = SystemColors.Window;
        }

        private void ZaikoControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(zaikoControls, sender);
                    bool ret = CheckZaiko(index);

                    if (ret)
                    {
                        if (index == (int)ZIndex.OrderNO)
                            BtnDisp.Focus();
                        else
                            zaikoControls[index + 1].Focus();

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

        private void ZaikoControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                SetFuncKeyAll(this, "100001001010");

                int index = Array.IndexOf(zaikoControls, sender);
                switch (index)
                {
                    case (int)ZIndex.OrderCD:
                    case (int)ZIndex.OrderNO:
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

        private void JuchuuControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(juchuuControls, sender);
                    bool ret = CheckJuchuu(index);

                    if (ret)
                    {
                        if (index == (int)JIndex.ChkJuchuuKBN4)
                        {
                            BtnDisp.Focus();
                        }
                        else
                            juchuuControls[index + 1].Focus();

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

        private void JuchuuControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                SetFuncKeyAll(this, "100001001010");

                int index = Array.IndexOf(juchuuControls, sender);
                switch (index)
                {
                    case (int)JIndex.Customer:
                    case (int)JIndex.JuchuuNo:
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
                w_ActCtl.BackColor = ClsGridBase.BKColor;

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

        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int w_Row;
                    Control w_ActCtl;

                    w_ActCtl = (Control)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    bool lastCell = false;

                    if (CL == (int)ClsGridZaiko.ColNO.ReserveSu)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;

                    //チェック処理
                    //最終行を変更時、Validatingが発生しないため、KEYDOWNでもチェックを行う
                    if (CheckGrid(CL, w_Row) == false)
                    {
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

                    //validatingでエラーとなった場合に次の行の背景色が変わってしまうため、以下に変更
                    //this.ProcessTabKey(!e.Shift);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GridControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                {
                    return;
                }

                switch (e.KeyCode)
                {
                    case Keys.Tab:
                        if (!e.Shift)
                            S_Grid_0_Event_Tab(CL, w_Row, w_ActCtl, w_ActCtl);
                        else
                            S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                        e.IsInputKey = true;
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GridControl_Enter2(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                // 背景色
                w_ActCtl.BackColor = ClsGridBase.BKColor;

                if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }

                Grid_Gotfocus2(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void GridControl_Leave2(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                // 背景色
                w_ActCtl.BackColor = mGrid2.F_GetBackColor_MK(w_Col, w_Row);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void GridControl_KeyDown2(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int w_Row;
                    Control w_ActCtl;

                    w_ActCtl = (Control)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                    bool lastCell = false;

                    if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridHikiate.ColNO.ReserveSu)
                        if (w_Row == mGrid2.g_MK_Max_Row - 1)
                            lastCell = true;

                    //チェック処理
                    if (CheckGrid2(CL, w_Row) == false)
                    {
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
                    S_Grid_1_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);

                    //validatingでエラーとなった場合に次の行の背景色が変わってしまうため、以下に変更
                    // →　スクロールがきかなくなるのでやめる
                    //this.ProcessTabKey(!e.Shift);

                    e.Handled = true;
                    e.SuppressKeyPress = true;

                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void GridControl_PreviewKeyDown2(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                {
                    return;
                }

                switch (e.KeyCode)
                {
                    case Keys.Tab:
                        if (!e.Shift)
                            S_Grid_1_Event_Tab(CL, w_Row, w_ActCtl, w_ActCtl);
                        else
                            this.S_Grid_1_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                        e.IsInputKey = true;
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        
        private void GridControl_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;


                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                {
                    return;
                }

                if (CL == (int)ClsGridZaiko.ColNO.ReserveSu)
                { 
                    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        //Focusセット処理
                        e.Cancel = true;
                        previousCtrl.BackColor = mGrid.F_GetBackColor_MK(CL, w_Row);
                        return;
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

        private void GridControl_Validating2(object sender, CancelEventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;


                if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                {
                    return;
                }

                if (CL == (int)ClsGridHikiate.ColNO.ReserveSu)
                {
                    //チェック処理
                    if (CheckGrid2(CL, w_Row) == false)
                    {
                        //Focusセット処理
                        e.Cancel = true;
                        previousCtrl.BackColor = mGrid2.F_GetBackColor_MK(CL, w_Row);
                        return;
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
                     
        #endregion

        /// <summary>
        /// 商品検索ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    setCtl = headControls[(int)HIndex.SKUCD];
                }
                else if (((Control)sender).Name.Equals(btnSearchJANCD.Name))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                    setCtl = headControls[(int)HIndex.JANCD];
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
        /// 在庫から引当ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Zaiko_Click(object sender, EventArgs e)
        {
            this.ChangeOperationMode(EOpeMode.ZAIKO);
            this.Scr_Lock(0);

            zaikoControls[0].Focus(); 
        }

        /// <summary>
        /// 受注から引当ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Juchuu_Click(object sender, EventArgs e)
        {
            this.ChangeOperationMode(EOpeMode.JUCHUU);
            this.Scr_Lock(0);

            juchuuControls[0].Focus();
        }

        /// <summary>
        /// 表示（ヘッダ部）クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF11_Click(object sender, EventArgs e)
        {
            this.ExecDisp();
        }

        /// <summary>
        /// 表示（明細部）クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDisp_Click(object sender, EventArgs e)
        {
            this.ExecDispDetail();
        }

        
    }
}








