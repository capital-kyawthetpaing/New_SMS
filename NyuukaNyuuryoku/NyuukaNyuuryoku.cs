using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace NyuukaNyuuryoku
{
    /// <summary>
    /// NyuukaNyuuryoku 入荷入力
    /// </summary>
    internal partial class NyuukaNyuuryoku : FrmMainForm
    {
        private const string ProID = "NyuukaNyuuryoku";
        private const string ProNm = "入荷入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            ArrivalNO,
            ArrivalDate = 0,
            //StoreCD,

            VendorDeliveryNo,
            SoukoName,
            JANCD,
            txtMaker,
            Nyukasu,

            Vendor = 0,
            SKUCD,
            Maker,
            SKUName,
            Brand,
            Color,
            Size
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private NyuukaNyuuryoku_BL nnbl;
        private D_Arrival_Entity dae;
        private DataTable dtForUpdate;  //排他用

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private string mJANCD;
        private string mAdminNO;
        private string mSoukoCD = "";   //初期設定値の倉庫CD

        private string mOldArrivalNO = "";    //排他処理のため使用
        private string mOldArrivalDate = "";       

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridHikiate mGrid = new ClsGridHikiate();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数

        ClsGridZaiko mGrid2 = new ClsGridZaiko();
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

            if (ClsGridHikiate.gc_P_GYO <= ClsGridHikiate.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridHikiate.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridHikiate.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridHikiate.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridHikiate.gc_MaxCL;

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
                            check.Click += new System.EventHandler(CHK_Del_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            btn.Click += new System.EventHandler(BTN_Detail_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridHikiate.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridHikiate.ColNO.GYONO; i <= (int)ClsGridHikiate.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHikiate.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridHikiate.ColNO.SURYO:
                        case (int)ClsGridHikiate.ColNO.Space:
                        case (int)ClsGridHikiate.ColNO.ReserveSu:
                            mGrid.SetProp_SU(3, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 0].CellCtl = BTN_Detail_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 0].CellCtl = IMN_SURYO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 0].CellCtl = IMN_SALEP_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 0].CellCtl = IMT_PAYDT_0;    //支払予定日
            
            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 1].CellCtl = BTN_Detail_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 1].CellCtl = IMN_SURYO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 1].CellCtl = IMN_SALEP_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 1].CellCtl = IMN_WEBPR_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 1].CellCtl = IMT_PAYDT_1;    //支払予定日
            
            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 2].CellCtl = BTN_Detail_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 2].CellCtl = IMN_SURYO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 2].CellCtl = IMN_SALEP_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 2].CellCtl = IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 3].CellCtl = BTN_Detail_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 3].CellCtl = IMN_SURYO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 3].CellCtl = IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 4].CellCtl = BTN_Detail_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 4].CellCtl = IMN_SURYO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 4].CellCtl = IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Btn, 5].CellCtl = BTN_Detail_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Check, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYUNO, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.SURYO, 5].CellCtl = IMN_SURYO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Space, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DirectFlg, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.ReserveSu, 5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.Customer, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.DeliveryPlanDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridHikiate.ColNO.JYGNO, 5].CellCtl = IMT_PAYDT_5;    //支払予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, (int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvPrv, (int)ClsGridHikiate.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, (int)ClsGridHikiate.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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

                        // 固定色の列はその色を設定
                        switch (w_Col)
                        {
                            case (int)ClsGridHikiate.ColNO.GYONO:
                            case (int)ClsGridHikiate.ColNO.Btn:
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridHikiate.ColNO.JYUNO:
                            case (int)ClsGridHikiate.ColNO.JYGNO:
                            case (int)ClsGridHikiate.ColNO.Customer:
                            case (int)ClsGridHikiate.ColNO.Space:
                            case (int)ClsGridHikiate.ColNO.ReserveSu:
                            case (int)ClsGridHikiate.ColNO.DirectFlg:
                            case (int)ClsGridHikiate.ColNO.DeliveryPlanDate:
                                {
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

                    //クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    switch (w_Col)
                    {
                        case (int)ClsGridHikiate.ColNO.Btn:
                            {
                                S_Set_Cell_Selectable(w_Col, w_Row, false);
                                break;
                            }
                    }
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

        // ---------------------------------------------
        // ボディ部の状態制御
        // 
        // 引数    pKBN    0...  新規/修正/削除時(モード選択時)
        // 1...  修正時(画面展開後)
        // 2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ
        // pGrid 0...  明細の各プロパティ以外    
        // 1...  明細部の各プロパティ(先に設定しておいてから、実際にpGrid=0で PanelのEnable制御等を行う)
        // ---------------------------------------------
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
                            //for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            //{
                            //    if (m_EnableCnt - 1 < w_Row)
                            //        break;
                            //    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                            //    {
                            //        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                            //    }
                            //}
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.ArrivalNO].Text = "";
                                keyControls[(int)EIndex.ArrivalNO].Enabled = false;
                                ScOrderNO.BtnSearch.Enabled = false;
                                InitScr();
                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                F9Visible = false;
                                SetFuncKeyAll(this, "111111000001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.ArrivalNO].Enabled = true;
                                ScOrderNO.BtnSearch.Enabled = true;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001000");
                            }
                                                    }
                        break;
                    }

                case 1: //修正時
                    {
                        if (pGrid == 1)
                        {
                            //倉庫とJANCDは変更不可
                            CboSoukoName.Enabled = false;
                            txtJANCD.Enabled = false;
                            BtnJANCD.Enabled = false;

                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JYUNO))
                                {
                                    mGrid.g_MK_State[(int)ClsGridHikiate.ColNO.Check, w_Row].Cell_Enabled = true;
                                    mGrid.g_MK_State[(int)ClsGridHikiate.ColNO.SURYO, w_Row].Cell_Enabled = true;
                                    mGrid.g_MK_State[(int)ClsGridHikiate.ColNO.Btn, w_Row].Cell_Enabled = true;
                                    continue;
                                }
                            }
                            for (w_Row = mGrid2.g_MK_State.GetLowerBound(1); w_Row <= mGrid2.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                if (!string.IsNullOrWhiteSpace(mGrid2.g_DArray[w_Row].Number))
                                {
                                    mGrid2.g_MK_State[(int)ClsGridHikiate.ColNO.Check, w_Row].Cell_Enabled = true;
                                    mGrid2.g_MK_State[(int)ClsGridHikiate.ColNO.SURYO, w_Row].Cell_Enabled = true;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
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
                            // 使用可項目無  明細部スクロールのみ可
                            // IMT_DMY_0.Focus()
                            Pnl_Body.Enabled = true;                  // ボディ部使用可
                            Pnl_Body2.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000001");
                                IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                SetFuncKeyAll(this, "111111000000");
                            }

                            // 削除時のみ、明細を参照できるように、スクロールバーのTabStopをTrueにする
                            this.Vsb_Mei_0.TabStop = true;
                            this.Vsb_Mei_1.TabStop = true;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //【引当】か【在庫】							
            if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JYUNO))
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

            if (OperationMode >= EOperationMode.DELETE)
                return;

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

            if (pCol == (int)ClsGridHikiate.ColNO.JYUNO || w_AllFlg == true)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JYUNO))
                { 
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
                            case (int)ClsGridHikiate.ColNO.SURYO:
                            case (int)ClsGridHikiate.ColNO.Check:    // 
                            case (int)ClsGridHikiate.ColNO.Btn:    // 
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                }
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

            if (ClsGridZaiko.gc_P_GYO <= ClsGridZaiko.gMxGyo)
            {
                mGrid2.g_MK_Max_Row = ClsGridZaiko.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridZaiko.gMxGyo;
            }
            //else
            //{
            //    mGrid2.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid2.g_MK_Ctl_Row = ClsGridZaiko.gc_P_GYO;
            mGrid2.g_MK_Ctl_Col = ClsGridZaiko.gc_MaxCL;

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
                        }
                        else if (mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid2.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Enter += new System.EventHandler(GridControl_Enter2);
                            check.Leave += new System.EventHandler(GridControl_Leave2);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown2);
                            check.Click += new System.EventHandler(CHK_Del_Click2);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid2.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid2.g_MK_Ctl_Col, mGrid2.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid2.g_DArray = new ClsGridZaiko.ST_DArray_Grid[mGrid2.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid2.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid2.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid2.g_MK_FocusOrder = new int[mGrid2.g_MK_Ctl_Col];

            for (int i = (int)ClsGridZaiko.ColNO.GYONO; i <= (int)ClsGridZaiko.ColNO.COUNT - 1; i++)
            {
                mGrid2.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid2.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridZaiko.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridZaiko.ColNO.SURYO:
                        case (int)ClsGridZaiko.ColNO.OrderSu:
                        case (int)ClsGridZaiko.ColNO.ReserveSu:
                            mGrid2.SetProp_SU(3, ref mGrid2.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 0].CellCtl = IMT_GYONO2_0;
            //mGrid2.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Check, 0].CellCtl = CHK_EDICK2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Number, 0].CellCtl = IMT_ITMCD2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.SURYO, 0].CellCtl = IMN_SURYO2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderSu, 0].CellCtl = IMN_GENER22_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DirectFlg, 0].CellCtl = IMN_MEMBR2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 0].CellCtl = IMN_SALEP2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Customer, 0].CellCtl = IMN_WEBPR2_0;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DeliveryPlanDate, 0].CellCtl = IMT_ARIDT2_0;     //
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.RowNo, 0].CellCtl = IMT_PAYDT2_0;    //

            // 2行目
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 1].CellCtl = IMT_GYONO2_1;
            //mGrid2.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Check, 1].CellCtl = CHK_EDICK2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Number, 1].CellCtl = IMT_ITMCD2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.SURYO, 1].CellCtl = IMN_SURYO2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderSu, 1].CellCtl = IMN_GENER22_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DirectFlg, 1].CellCtl = IMN_MEMBR2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 1].CellCtl = IMN_SALEP2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Customer, 1].CellCtl = IMN_WEBPR2_1;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DeliveryPlanDate, 1].CellCtl = IMT_ARIDT2_1;     //
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.RowNo, 1].CellCtl = IMT_PAYDT2_1;    //

            // 3行目
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.GYONO, 2].CellCtl = IMT_GYONO2_2;
            //mGrid2.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Check, 2].CellCtl = CHK_EDICK2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Number, 2].CellCtl = IMT_ITMCD2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.SURYO, 2].CellCtl = IMN_SURYO2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.OrderSu, 2].CellCtl = IMN_GENER22_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DirectFlg, 2].CellCtl = IMN_MEMBR2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.ReserveSu, 2].CellCtl = IMN_SALEP2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.Customer, 2].CellCtl = IMN_WEBPR2_2;
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.DeliveryPlanDate, 2].CellCtl = IMT_ARIDT2_2;     //
            mGrid2.g_MK_Ctrl[(int)ClsGridZaiko.ColNO.RowNo, 2].CellCtl = IMT_PAYDT2_2;    //

        }

        // 明細部 Tab の処理
        private void S_Grid_1_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, (int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_1_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvPrv, (int)ClsGridZaiko.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 Enter の処理
        private void S_Grid_1_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, (int)ClsGridZaiko.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1);
        }

        // 明細部 PageDown の処理
        private void S_Grid_1_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid2.g_MK_Ctl_Row - 1) > mGrid2.g_MK_Max_Row - 1)
                w_GoRow = mGrid2.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid2.g_MK_Ctl_Row - 1);

            mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_1_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid2.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid2.g_MK_Ctl_Row - 1);

            mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_1, w_GoRow, pCol);
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

                        // 固定色の列はその色を設定
                        switch (w_Col)
                        {
                            case (int)ClsGridZaiko.ColNO.GYONO:
                                {
                                    mGrid2.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                    break;
                                }
                            case (int)ClsGridZaiko.ColNO.Number:
                            case (int)ClsGridZaiko.ColNO.RowNo:
                            case (int)ClsGridZaiko.ColNO.Customer:
                            case (int)ClsGridZaiko.ColNO.OrderSu:
                            case (int)ClsGridZaiko.ColNO.ReserveSu:
                            case (int)ClsGridZaiko.ColNO.DirectFlg:
                            case (int)ClsGridZaiko.ColNO.DeliveryPlanDate:
                                {
                                    mGrid2.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                                    break;
                                }
                        }
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

        // ---------------------------------------------
        // ボディ部の状態制御
        // 
        // 引数    pKBN    0...  新規/修正/削除時(モード選択時)
        // 1...  修正時(画面展開後)
        // 2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ
        // pGrid 0...  明細の各プロパティ以外    
        // 1...  明細部の各プロパティ(先に設定しておいてから、実際にpGrid=0で PanelのEnable制御等を行う)
        // ---------------------------------------------
        private void S_BodySeigyo2(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid2.g_MK_State.GetLowerBound(1); w_Row <= mGrid2.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                //Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                mGrid2.g_MK_State[(int)ClsGridZaiko.ColNO.SURYO, w_Row].Cell_Enabled = true;

                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.ArrivalNO].Text = "";
                                keyControls[(int)EIndex.ArrivalNO].Enabled = false;
                                ScOrderNO.BtnSearch.Enabled = false;
                                InitScr();
                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                F9Visible = false;
                                SetFuncKeyAll(this, "111111000001");
                            }
                            else
                            {
                                keyControls[(int)EIndex.ArrivalNO].Enabled = true;
                                ScOrderNO.BtnSearch.Enabled = true;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_1.TabStop = false;

                                SetFuncKeyAll(this, "111111001000");
                            }
                        }
                        break;
                    }

                case 1: //修正時
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid2.g_MK_State.GetLowerBound(1); w_Row <= mGrid2.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                if (string.IsNullOrWhiteSpace(mGrid2.g_DArray[w_Row].Number))
                                {
                                    mGrid2.g_MK_State[(int)ClsGridZaiko.ColNO.SURYO, w_Row].Cell_Enabled = true;
                                    continue;
                                }

                                for (int w_Col = mGrid2.g_MK_State.GetLowerBound(0); w_Col <= mGrid2.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridZaiko.ColNO.Check:    // 
                                        case (int)ClsGridZaiko.ColNO.SURYO:    // 
                                            {
                                                mGrid2.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                            //case (int)ClsGridNyuuka.ColNO.JanCD:
                                            //case (int)ClsGridNyuuka.ColNO.SKUName:    // 
                                            //case (int)ClsGridNyuuka.ColNO.ColorName:    // 
                                            //case (int)ClsGridNyuuka.ColNO.SizeName:
                                            //    {
                                            //        mGrid2.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                            //        break;
                                            //    }
                                    }
                                }
                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
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
                            // 使用可項目無  明細部スクロールのみ可
                            // IMT_DMY_0.Focus()
                            Pnl_Body2.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000001");
                                IMT_DMY_0.Focus();
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                SetFuncKeyAll(this, "111111000000");
                            }

                            // 削除時のみ、明細を参照できるように、スクロールバーのTabStopをTrueにする
                            this.Vsb_Mei_1.TabStop = true;
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void Grid_Gotfocus2(int pCol, int pRow, int pCtlRow)
        {
            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop2(true);

            if (mGrid2.g_DArray[pRow].ArrivalPlanKBN == "2")
                SetFuncKey(this, 9, false);
            else
                SetFuncKey(this, 9, true);
        }

        private void Grid_NotFocus2(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            bool w_AllFlg = false;
            int w_CtlRow;

            if (OperationMode >= EOperationMode.DELETE)
                return;

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


            if (pCol == (int)ClsGridZaiko.ColNO.Number || w_AllFlg == true)
            {
                if (string.IsNullOrWhiteSpace(mGrid2.g_DArray[pRow].Number))
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
                            case (int)ClsGridZaiko.ColNO.SURYO:    
                            case (int)ClsGridZaiko.ColNO.Check:    // 
                                {
                                    mGrid2.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                }
                                break;
                        }
                    }
                    w_AllFlg = false;
                }
            }
        }
        #endregion

        public NyuukaNyuuryoku()
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

                this.SetFunctionLabel(EProMode.INPUT);
                this.InitialControlArray();

                // 明細部初期化
                this.S_SetInit_Grid();
                this.S_SetInit_Grid2();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "入荷予定(F10)";
                Btn_F11.Text = "";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                nnbl = new NyuukaNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                CboSoukoName.Bind(ymd);

                InitScr();

                //検索用のパラメータ設定
                string stores= GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScOrderNO.Value3 = mSoukoCD;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        private void InitScr()
        {
            if (nnbl == null)
                return;

            string ymd = bbl.GetDate();
            detailControls[(int)EIndex.ArrivalDate].Text = ymd;

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
            //[M_Souko_Select]
            M_Souko_Entity me = new M_Souko_Entity
            {
                StoreCD = mse.StoreCD,
                SoukoType = "3", //店舗Main倉庫
                ChangeDate = bbl.GetDate(),
                DeleteFlg = "0"
            };
            DataTable sdt = nnbl.M_Souko_SelectForNyuuka(me);
            if (sdt.Rows.Count > 0)
            {
                CboSoukoName.SelectedValue = sdt.Rows[0]["SoukoCD"];
                mSoukoCD = sdt.Rows[0]["SoukoCD"].ToString();
            }
            else
            {
                CboSoukoName.SelectedValue = "";
            }
        }
        private void InitialControlArray()
        {
            keyControls = new Control[] {  ScOrderNO.TxtCode, CboStoreCD};
            keyLabels = new Control[] {  };
            detailControls = new Control[] { ckM_TextBox1, txtVendorDeliveryNo, CboSoukoName, txtJANCD, txtMakerItem, txtSu };
            detailLabels = new Control[] { lblVendor,lblSKUCD,lblMaker,lblSKUName,lblBrand,lblColorName,lblSizeName,lblTani,lblHikiate,lblZaiko };
            searchButtons = new Control[] { BtnJANCD};

            
            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set=true)
        {

            switch (index)
            {
                case (int)EIndex.ArrivalNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //排他処理
                    bool ret = SelectAndInsertExclusive(keyControls[index].Text, Exclusive_BL.DataKbn.Nyuka);
                    if (!ret)
                        return false;

                    return CheckData(set);

                //case (int)EIndex.StoreCD:
                //    //選択必須(Entry required)
                //    if (!RequireCheck(new Control[] { keyControls[index] }))
                //    {
                //        return false;
                //    }
                //    else
                //    {
                //        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                //        {
                //            bbl.ShowMessage("E141");
                //            CboStoreCD.Focus();
                //            return false;
                //        }
                //    }

                //    break;

            }

            return true;

        }

        private bool SelectAndInsertExclusive(string no, Exclusive_BL.DataKbn dataKbn)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;
            
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();

            if (OperationMode == EOperationMode.INSERT)
            {
                
            }
            else
            {
                DeleteExclusive();
            }

            if (string.IsNullOrWhiteSpace(no))
                return true;

            //排他Tableに該当番号が存在するとError
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)dataKbn,
                Number = no,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };


            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.ArrivalNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldArrivalNO = keyControls[(int)EIndex.ArrivalNO].Text;
                return ret;
            }
            
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive()
        {

            if (mOldArrivalNO == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                dtForUpdate = new DataTable();
            }

            if (mOldArrivalNO != "")
            {
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Nyuka,
                    Number = mOldArrivalNO,
                };

                bool ret = ebl.D_Exclusive_Delete(dee);

                mOldArrivalNO = "";
            }
            
        }
        private void DeleteOrder()
        {
            //F10：入荷予定ボタンで追加されたデータについて画面上の入荷数＝０であれば（追加したにもかかわらず、入荷数が指定されなかったら）そのRecordをDeleteする
            DataTable dtCopy = GetGridCopyEntity();
            if (dtCopy != null && dtCopy.Rows.Count > 0)
            {
                dae = GetEntity();
                bool ret = nnbl.D_Order_Delete(dae, dtCopy, (short)OperationMode);
            }

        }

        /// <summary>
        /// 入荷データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.ArrivalNO)
        {
            //[D_Arrival_SelectData]
            dae = new D_Arrival_Entity
            {
                ArrivalNO = keyControls[(int)EIndex.ArrivalNO].Text
            };

            DataTable dt = nnbl.D_Arrival_SelectData(dae, (short)OperationMode);

            //入荷(D_Order)に存在しない場合、Error 「登録されていない入荷番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "入荷番号");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された入荷番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "入荷番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //取得した入荷データの入荷日が、入力できる範囲内の日付であること
                if (!bbl.CheckInputPossibleDate(dt.Rows[0]["ArrivalDate"].ToString()))
                {
                    //Ｅ１１５
                    bbl.ShowMessage("E115");
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない入荷番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", "入荷番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                    //進捗チェック　既に出荷済み,出荷指示済み,ピッキングリスト完了済み警告
                    bool ret = nnbl.CheckNyuukaData(dae, out string errno);
                    if (ret)
                    {
                        if (!string.IsNullOrWhiteSpace(errno))
                        {
                            if (errno.Substring(0).Equals("Q"))
                            {
                                if (bbl.ShowMessage(errno) != DialogResult.Yes)
                                    return false;
                            }
                            else
                            {
                                //Errorメッセージを表示する
                                bbl.ShowMessage(errno);
                                return false;
                            }
                        }
                        // Errorでない場合、画面転送表01に従ってデータ取得 / 画面表示
                    }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                S_Clear_Grid();   //画面クリア（明細部）
                S_Clear_Grid2();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                int i2 = 0;
                m_dataCnt = 0;
                m_dataCnt2 = 0;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    if (i == 0)
                    {
                        CboStoreCD.SelectedValue = row["StoreCD"];

                        //明細にデータをセット
                        if (index == (int)EIndex.ArrivalNO)
                        {
                            detailControls[(int)EIndex.ArrivalDate].Text = row["ArrivalDate"].ToString();
                        }
                        else
                        {
                            detailControls[(int)EIndex.ArrivalDate].Text = bbl.GetDate();
                        }
                        mOldArrivalDate = detailControls[(int)EIndex.ArrivalDate].Text;

                        detailControls[(int)EIndex.VendorDeliveryNo].Text = row["VendorDeliveryNo"].ToString();
                        CboSoukoName.SelectedValue = row["SoukoCD"];
                        detailControls[(int)EIndex.JANCD].Text = row["JANCD"].ToString();
                        mJANCD = detailControls[(int)EIndex.JANCD].Text;
                        mAdminNO = row["AdminNO"].ToString();
                        lblVendor.Text= row["VendorName"].ToString();
                        lblSKUCD.Text = row["SKUCD"].ToString();
                        SetMakerText( row["MakerItem"].ToString());
                        lblSKUName.Text = row["SKUName"].ToString();
                        lblBrand.Text = row["BrandName"].ToString();
                        lblColorName.Text = row["ColorName"].ToString();
                        lblSizeName.Text = row["SizeName"].ToString();
                        txtSu.Text = bbl.Z_SetStr(row["ArrivalSu"]);
                        lblTani.Text = row["TaniName"].ToString();
                    }
                    if (bbl.Z_Set(row["KBN"]) == 1)
                    {
                        mGrid.g_DArray[i].Check = true;
                        mGrid.g_DArray[i].JYUNO = row["JuchuuNO"].ToString();
                        mGrid.g_DArray[i].JYGNO = row["JuchuuRows"].ToString();
                        mGrid.g_DArray[i].CustomerCD = row["CustomerCD"].ToString();
                        mGrid.g_DArray[i].Customer = row["CustomerName2"].ToString();
                        mGrid.g_DArray[i].Space = "";   // 
                        mGrid.g_DArray[i].ReserveSu = bbl.Z_SetStr(row["ReserveSu"]);   //
                        mGrid.g_DArray[i].SURYO = bbl.Z_SetStr(row["DR_ArrivalSu"]);
                        mGrid.g_DArray[i].DirectFlg = row["DirectFlg"].ToString();
                        mGrid.g_DArray[i].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();
                        mGrid.g_DArray[i].VendorCD = row["VendorCD"].ToString();
                        mGrid.g_DArray[i].VendorName = row["VendorName"].ToString();
                        mGrid.g_DArray[i].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                        mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                        mGrid.g_DArray[i].ReserveNO = row["ReserveNO"].ToString();
                        mGrid.g_DArray[i].ArrivalPlanKBN = row["ArrivalPlanKBN"].ToString();

                        mGrid.g_DArray[i].OrderUnitPrice = row["OrderUnitPrice"].ToString();
                        mGrid.g_DArray[i].PriceOutTax = row["PriceOutTax"].ToString();
                        mGrid.g_DArray[i].Rate = row["Rate"].ToString();
                        mGrid.g_DArray[i].TaniCD = row["TaniCD"].ToString();
                        mGrid.g_DArray[i].OrderTaxRitsu = row["OrderTaxRitsu"].ToString();
                        mGrid.g_DArray[i].OrderWayKBN = row["OrderWayKBN"].ToString();
                        mGrid.g_DArray[i].AliasKBN = row["AliasKBN"].ToString();

                        m_dataCnt = i + 1;
                        Grid_NotFocus((int)ClsGridHikiate.ColNO.JYUNO, i);
                        i++;
                    }
                    else
                    {
                        //使用可能行数を超えた場合エラー
                        if (i2 > m_EnableCnt - 1)
                        {
                            bbl.ShowMessage("E178", m_EnableCnt.ToString());
                            mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);
                            return false;
                        }
                        mGrid2.g_DArray[i2].Check = true;
                        mGrid2.g_DArray[i2].Number = row["JuchuuNO"].ToString();     //OrderNo
                        mGrid2.g_DArray[i2].RowNo = row["JuchuuRows"].ToString();    //OrderRows
                        mGrid2.g_DArray[i2].CustomerCD = row["CustomerCD"].ToString();
                        mGrid2.g_DArray[i2].Customer = row["CustomerName2"].ToString();
                        //mGrid2.g_DArray[i2].OrderSu = bbl.Z_SetStr(row["HachuSu"]);   // 
                        //mGrid2.g_DArray[i2].ReserveSu = bbl.Z_SetStr(row["ReserveSu"]);   //
                        mGrid2.g_DArray[i2].ReserveSu = bbl.Z_SetStr(bbl.Z_Set(row["HachuSu"]) - bbl.Z_Set(row["ReserveSu"]));   //
                        mGrid2.g_DArray[i2].SURYO = bbl.Z_SetStr(row["DR_ArrivalSu"]);
                        mGrid2.g_DArray[i2].DirectFlg = row["DirectFlg"].ToString();
                        mGrid2.g_DArray[i2].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();
                        mGrid2.g_DArray[i2].VendorCD = row["VendorCD"].ToString();
                        mGrid2.g_DArray[i2].VendorName = row["VendorName"].ToString();
                        mGrid2.g_DArray[i2].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                        mGrid2.g_DArray[i2].StockNO = row["StockNO"].ToString();
                        mGrid2.g_DArray[i2].ReserveNO = row["ReserveNO"].ToString();
                        mGrid2.g_DArray[i2].ArrivalPlanKBN = row["ArrivalPlanKBN"].ToString();

                        mGrid2.g_DArray[i2].OrderUnitPrice = row["OrderUnitPrice"].ToString();
                        mGrid2.g_DArray[i2].PriceOutTax = row["PriceOutTax"].ToString();
                        mGrid2.g_DArray[i2].Rate = row["Rate"].ToString();
                        mGrid2.g_DArray[i2].TaniCD = row["TaniCD"].ToString();
                        mGrid2.g_DArray[i2].OrderTaxRitsu = row["OrderTaxRitsu"].ToString();
                        mGrid2.g_DArray[i2].OrderWayKBN = row["OrderWayKBN"].ToString();
                        mGrid2.g_DArray[i2].AliasKBN = row["AliasKBN"].ToString();
                        mGrid2.g_DArray[i2].isCopy = false;

                         m_dataCnt2 = i2 + 1;
                        Grid_NotFocus2((int)ClsGridZaiko.ColNO.Number, i2);
                        i2++;
                    }
                }


                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);

            }
            CalcKin();

            if (OperationMode == EOperationMode.UPDATE )
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);
            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

                previousCtrl.Focus();
            }

            return true;
        }
        protected override void ExecDisp()
        {

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

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.ArrivalDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if(!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity msce = new M_StoreClose_Entity
                    {
                        StoreCD = CboStoreCD.SelectedValue.ToString(),
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    ret = bbl.CheckStoreClose(msce,false,false,false,false,true);
                    if (!ret)
                    {
                        return false;
                    }

                    break;

                case (int)EIndex.SoukoName:
                    //入力必須(Entry required)
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboSoukoName.MoveNext = false;
                        return false;
                    }
                    ////[M_Souko_Select]
                    //M_Souko_Entity me = new M_Souko_Entity
                    //{
                    //    SoukoCD = CboSoukoName.SelectedValue.ToString(),
                    //    ChangeDate = bbl.GetDate(),
                    //    DeleteFlg = "0"
                    //};
                    
                    //DataTable mdt = nnbl.M_Souko_IsExists(me);
                    //if (mdt.Rows.Count > 0)
                    //{
                    //    if (!base.CheckAvailableStores(mdt.Rows[0]["StoreCD"].ToString()))
                    //    {
                    //        bbl.ShowMessage("E141");
                    //        detailControls[index].Focus();
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    bbl.ShowMessage("E101");
                    //    detailControls[index].Focus();
                    //    return false;
                    //}

                    break;

                case (int)EIndex.JANCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    if(!detailControls[index].Text.Equals(mJANCD))
                    {
                        mAdminNO = "";
                    }

                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = detailControls[index].Text,
                        AdminNO = mAdminNO,
                        ChangeDate = detailControls[(int)EIndex.ArrivalDate].Text
                    };

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
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
                            frmSKU.parChangeDate = detailControls[(int)EIndex.ArrivalDate].Text;
                            frmSKU.ShowDialog();

                            if (!frmSKU.flgCancel)
                            {
                                selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                            }
                        }
                    }

                    if (selectRow != null)
                    {
                        //変更なしの場合は再セットしない
                        if (mAdminNO.Equals(selectRow["AdminNO"].ToString()) && set == false)
                            return true;

                        //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                        mAdminNO = selectRow["AdminNO"].ToString();
                        mJANCD = selectRow["JANCD"].ToString();
                        lblSKUCD.Text = selectRow["SKUCD"].ToString();
                        SetMakerText(selectRow["MakerItem"].ToString());
                        SetMakerVisible(selectRow["VariousFLG"].ToString().Equals("1"));
                        lblSKUName.Text = selectRow["SKUName"].ToString();
                        lblColorName.Text = selectRow["ColorName"].ToString();
                        lblSizeName.Text = selectRow["SizeName"].ToString();
                        lblBrand.Text = selectRow["BrandName"].ToString();
                        lblTani.Text = selectRow["TaniName"].ToString();

                        if (OperationMode == EOperationMode.INSERT)
                        {
                            //Errorでない場合、画面転送表02に従ってデータ取得/画面表示  
                            D_ArrivalPlan_Entity de = new D_ArrivalPlan_Entity
                            {
                                AdminNO = mAdminNO
                            };
                            if (CboSoukoName.SelectedIndex != -1)
                                de.SoukoCD = CboSoukoName.SelectedValue.ToString();
                            DataTable dtAr = nnbl.D_ArrivalPlan_SelectData(de, (short)OperationMode);
                            if (dtAr.Rows.Count == 0)
                            {
                                bbl.ShowMessage("E128");
                                S_Clear_Grid();   //画面クリア（明細部）
                                S_Clear_Grid2();   //画面クリア（明細部）
                                mAdminNO = "";
                                return false;
                            }
                            else
                            {
                                //明細にデータをセット 排他データの削除がうまくいっているか確認
                                DeleteExclusive();

                                int gyono = 0;
                                string breakKey = "";
                                dtForUpdate = new DataTable();
                                dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
                                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

                                Exclusive_BL.DataKbn dataKbn = Exclusive_BL.DataKbn.Hacchu;
                                //排他処理
                                foreach (DataRow row in dtAr.Rows)
                                {
                                    if (row["KBN"].ToString() == "2")
                                    {
                                        //OrderNO
                                        dataKbn = Exclusive_BL.DataKbn.Hacchu;
                                    }
                                    else if (row["KBN"].ToString() == "3")
                                    {
                                        //MoveNO
                                        dataKbn = Exclusive_BL.DataKbn.Zaikoido;
                                    }

                                    if(breakKey.Equals(row["JuchuuNO"].ToString() + dataKbn.ToString()))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        breakKey = row["JuchuuNO"].ToString() + dataKbn.ToString();
                                    }

                                    ret = SelectAndInsertExclusive(row["JuchuuNO"].ToString(), dataKbn);
                                    if (!ret)
                                        return false;

                                    gyono++;
                                    // データを追加
                                    DataRow rowForUpdate;
                                    rowForUpdate = dtForUpdate.NewRow();
                                    rowForUpdate["kbn"] = (int)dataKbn;
                                    rowForUpdate["no"] = row["JuchuuNO"].ToString();
                                    dtForUpdate.Rows.Add(rowForUpdate);
                                }
                            }                            

                            S_Clear_Grid();   //画面クリア（明細部）
                            S_Clear_Grid2();   //画面クリア（明細部）

                            //明細にデータをセット
                            int i = 0;
                            int i2 = 0;
                            m_dataCnt = 0;
                            m_dataCnt2 = 0;

                            foreach (DataRow row in dtAr.Rows)
                            {
                                if (bbl.Z_Set(row["KBN"]) == 1)
                                {
                                    //使用可能行数を超えた場合エラー
                                    if (i > m_EnableCnt - 1)
                                    {
                                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                                        return false;
                                    }

                                    mGrid.g_DArray[i].Check = false;
                                    mGrid.g_DArray[i].JYUNO = row["JuchuuNO"].ToString();
                                    mGrid.g_DArray[i].JYGNO = row["JuchuuRows"].ToString();
                                    mGrid.g_DArray[i].CustomerCD = row["CustomerCD"].ToString();
                                    mGrid.g_DArray[i].Customer = row["CustomerName2"].ToString();
                                    mGrid.g_DArray[i].Space = "";   // 
                                    mGrid.g_DArray[i].ReserveSu = bbl.Z_SetStr(row["ReserveSu"]);   //
                                    mGrid.g_DArray[i].SURYO = bbl.Z_SetStr(row["DR_ArrivalSu"]);
                                    mGrid.g_DArray[i].DirectFlg = row["DirectFlg"].ToString();
                                    mGrid.g_DArray[i].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();
                                    mGrid.g_DArray[i].VendorCD = row["VendorCD"].ToString();
                                    mGrid.g_DArray[i].VendorName = row["VendorName"].ToString();
                                    mGrid.g_DArray[i].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                                    mGrid.g_DArray[i].StockNO = row["StockNO"].ToString();
                                    mGrid.g_DArray[i].ReserveNO = row["ReserveNO"].ToString();
                                    mGrid.g_DArray[i].ArrivalPlanKBN = row["ArrivalPlanKBN"].ToString();

                                    mGrid.g_DArray[i].OrderUnitPrice = row["OrderUnitPrice"].ToString();
                                    mGrid.g_DArray[i].PriceOutTax = row["PriceOutTax"].ToString();
                                    mGrid.g_DArray[i].Rate = row["Rate"].ToString();
                                    mGrid.g_DArray[i].TaniCD = row["TaniCD"].ToString();
                                    mGrid.g_DArray[i].OrderTaxRitsu = row["OrderTaxRitsu"].ToString();
                                    mGrid.g_DArray[i].OrderWayKBN = row["OrderWayKBN"].ToString();
                                    mGrid.g_DArray[i].AliasKBN = row["AliasKBN"].ToString();


                                    m_dataCnt = i + 1;
                                    Grid_NotFocus((int)ClsGridHikiate.ColNO.JYUNO, i);
                                    i++;
                                }
                                else
                                {
                                    //使用可能行数を超えた場合エラー
                                    if (i2 > m_EnableCnt - 1)
                                    {
                                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                                        mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);
                                        return false;
                                    }
                                    mGrid2.g_DArray[i2].Check = false;
                                    mGrid2.g_DArray[i2].Number = row["JuchuuNO"].ToString();     //OrderNo
                                    mGrid2.g_DArray[i2].RowNo = row["JuchuuRows"].ToString();    //OrderRows
                                    mGrid2.g_DArray[i2].CustomerCD = row["CustomerCD"].ToString();
                                    mGrid2.g_DArray[i2].Customer = row["CustomerName2"].ToString();
                                    //mGrid2.g_DArray[i2].OrderSu = bbl.Z_SetStr(row["HachuSu"]);   // 
                                    mGrid2.g_DArray[i2].ReserveSu =  bbl.Z_SetStr(row["ReserveSu"]);   //
                                    mGrid2.g_DArray[i2].SURYO = bbl.Z_SetStr(row["DR_ArrivalSu"]);
                                    mGrid2.g_DArray[i2].DirectFlg = row["DirectFlg"].ToString();
                                    mGrid2.g_DArray[i2].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();
                                    mGrid2.g_DArray[i2].VendorCD = row["VendorCD"].ToString();
                                    mGrid2.g_DArray[i2].VendorName = row["VendorName"].ToString();
                                    mGrid2.g_DArray[i2].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                                    mGrid2.g_DArray[i2].StockNO = row["StockNO"].ToString();
                                    mGrid2.g_DArray[i2].ReserveNO = row["ReserveNO"].ToString();
                                    mGrid2.g_DArray[i2].ArrivalPlanKBN = row["ArrivalPlanKBN"].ToString();

                                    mGrid2.g_DArray[i2].OrderUnitPrice = row["OrderUnitPrice"].ToString();
                                    mGrid2.g_DArray[i2].PriceOutTax = row["PriceOutTax"].ToString();
                                    mGrid2.g_DArray[i2].Rate = row["Rate"].ToString();
                                    mGrid2.g_DArray[i2].TaniCD = row["TaniCD"].ToString();
                                    mGrid2.g_DArray[i2].OrderTaxRitsu = row["OrderTaxRitsu"].ToString();
                                    mGrid2.g_DArray[i2].OrderWayKBN = row["OrderWayKBN"].ToString();
                                    mGrid2.g_DArray[i2].AliasKBN = row["AliasKBN"].ToString();
                                    mGrid2.g_DArray[i2].isCopy = false;

                                    m_dataCnt2 = i2 + 1;
                                    Grid_NotFocus2((int)ClsGridZaiko.ColNO.Number, i2);
                                    i2++;
                                }
                            }

                            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                            mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);
                        }
                    }
                    break;

                case (int)EIndex.Nyukasu:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    //入荷総数≦0の場合、エラー
                    if(bbl.Z_Set(detailControls[index].Text) <= 0)
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143","1","小さい");
                        return false;
                    }

                    break;
            }

            return true;
        }

        // ********************************************
        // ERR時のSETFOCUS  ERR_FOCUS_GRID_SUB
        // 
        // *******************************************
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = detailControls[(int)EIndex.Nyukasu];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, (int)ClsGridHikiate.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        private void ERR_FOCUS_GRID_SUB2(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_1.Value;

            //配列の内容を画面へセット
            mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

            w_Ctrl = detailControls[(int)EIndex.Nyukasu];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid2.F_MoveFocus((int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, (int)ClsGridZaiko.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_1, pRow, pCol);

        }

        private bool CheckGrid(int col, int row, ref bool focus, bool chkAll = false, bool changeYmd = false)
        {

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridHikiate.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }
            }

            switch (col)
            {
                case (int)ClsGridHikiate.ColNO.SURYO:
                    //ONにした明細に対して、入荷数≦0の場合、エラー
                    if (mGrid.g_DArray[row].Check && bbl.Z_Set(mGrid.g_DArray[row].SURYO) <= 0)
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143", "1", "小さい");
                        return false;
                    }
                    //【引当】
                    //入荷数＞引当数の場合、エラー
                    if (bbl.Z_Set(mGrid.g_DArray[row].SURYO) > bbl.Z_Set(mGrid.g_DArray[row].ReserveSu))
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143", "引当数", "大きい");
                        return false;
                    }
                    if (bbl.Z_Set(mGrid.g_DArray[row].SURYO) > 0)
                    {
                        mGrid.g_DArray[row].Check = true;
                    }
                    //【引当】画面明細.引当数≠画面明細.入荷数（＝０を含む）のとき、その画面明細,StockNOと同じ【在庫】画面明細.StockNOで
                    //【在庫】画面明細.予定数≠画面明細.入荷数(≠０）となる明細があれば、【在庫】画面明細.入荷数をエラーとする
                    if (bbl.Z_Set(mGrid.g_DArray[row].SURYO) != bbl.Z_Set(mGrid.g_DArray[row].ReserveSu))
                    {
                        string stockNO = mGrid.g_DArray[row].StockNO;
                        for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
                        {
                            if (mGrid2.g_DArray[RW].StockNO == stockNO)
                            {
                                if (bbl.Z_Set(mGrid2.g_DArray[RW].SURYO) != 0 && bbl.Z_Set(mGrid2.g_DArray[RW].SURYO) != bbl.Z_Set(mGrid2.g_DArray[RW].ReserveSu))
                                {
                                    //Ｅ２５１
                                    bbl.ShowMessage("E251");
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB2((int)ClsGridZaiko.ColNO.SURYO, RW);
                                    focus = false;
                                    return false;
                                }
                            }
                        }
                    }
                    break;
            }

            //各金額項目の再計算必要
            if (chkAll == false)
            {
                if (!CalcKin())
                    return false;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        private bool CheckGrid2(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_1.Value;
                if (mGrid2.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                {
                    if (((CKM_Controls.CKM_TextBox)mGrid2.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                        return false;
                }
            }

            switch (col)
            {
                case (int)ClsGridHikiate.ColNO.SURYO:
                    //ONにした明細に対して、入荷数≦0の場合、エラー
                    if (mGrid2.g_DArray[row].Check && bbl.Z_Set(mGrid2.g_DArray[row].SURYO) <= 0)
                    {
                        //Ｅ１４３
                        bbl.ShowMessage("E143", "1", "小さい");
                        return false;
                    }
                    //【在庫】
                    //入荷数＞引当数の場合、	エラーでない
                    ////入荷数＞引当数の場合、エラー
                    //if (bbl.Z_Set(mGrid2.g_DArray[row].SURYO) > bbl.Z_Set(mGrid2.g_DArray[row].ReserveSu))
                    //{
                    //    //Ｅ１４３
                    //    bbl.ShowMessage("E143", "引当数", "大きい");
                    //    return false;
                    //}
                    if (bbl.Z_Set(mGrid2.g_DArray[row].SURYO) > 0)
                    {
                        mGrid2.g_DArray[row].Check = true;
                    }
                    //【在庫】画面明細.予定数≠画面明細.入荷数（≠０）のとき、その画面明細,StockNOと同じ【引当】画面明細.StockNOで
                    //【引当】画面明細.引当数≠画面明細.入荷数(≠０）となる明細があれば、【在庫】画面明細.入荷数をエラーとする
                    if (bbl.Z_Set(mGrid2.g_DArray[row].SURYO) != 0 && bbl.Z_Set(mGrid2.g_DArray[row].SURYO) != bbl.Z_Set(mGrid2.g_DArray[row].ReserveSu))
                    {
                        string stockNO = mGrid2.g_DArray[row].StockNO;
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (mGrid.g_DArray[RW].StockNO == stockNO)
                            {
                                if (bbl.Z_Set(mGrid.g_DArray[RW].SURYO) != 0 && bbl.Z_Set(mGrid.g_DArray[RW].ReserveSu) != bbl.Z_Set(mGrid.g_DArray[RW].SURYO))
                                {
                                    //Ｅ２５１
                                    bbl.ShowMessage("E251");
                                    return false;
                                }
                            }
                        }
                    }
                    break;
            }

            //各金額項目の再計算必要
            if (chkAll == false)
                CalcKin();

            //配列の内容を画面へセット
            mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

            return true;
        }
        private void ClearVendor()
        {
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].Check)
                {
                    return;
                }
            }
            for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid2.g_DArray[RW].Check)
                {
                    return;
                }
            }
            lblVendor.Text = "";
        }
        /// <summary>
        /// Footer部 金額計算処理
        /// </summary>
        private bool CalcKin(bool f12 = false)
        {
            //【引当】でチェックが入っている明細の入荷数を集計し、引当総数（【引当】直下）に表示
            decimal sumHikSu = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid.g_DArray[RW].Check)
                {
                    sumHikSu += bbl.Z_Set(mGrid.g_DArray[RW].SURYO);
                }
            }
            lblHikiate.Text = bbl.Z_SetStr(sumHikSu);

            decimal sumZaikoSu = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (mGrid2.g_DArray[RW].Check)
                {
                    sumZaikoSu += bbl.Z_Set(mGrid2.g_DArray[RW].SURYO);
                }
            }
            lblZaiko.Text = bbl.Z_SetStr(sumZaikoSu);

            //引当総数＋在庫総数＞入荷総数の場合、エラー
            if (sumHikSu + sumZaikoSu > bbl.Z_Set(detailControls[(int)EIndex.Nyukasu].Text))
            {
                //Ｅ１４３
                bbl.ShowMessage("E143", "入荷総数", "大きい");
                return false;
            }
            if(f12)
            {
                //入荷総数＞（引当での入荷数計＋在庫での入荷数計）の場合、 Ｅ２４８
                //エラーの場合、カーソルは入荷総数へ
                if (sumHikSu + sumZaikoSu < bbl.Z_Set(detailControls[(int)EIndex.Nyukasu].Text))
                {
                    //Ｅ１４３
                    bbl.ShowMessage("E248");
                    txtSu.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Arrival_Entity GetEntity()
        {
            dae = new D_Arrival_Entity
            {
                ArrivalNO = keyControls[(int)EIndex.ArrivalNO].Text,
                StoreCD = CboStoreCD.SelectedIndex>0 ? CboStoreCD.SelectedValue.ToString() :"",
                ArrivalDate = detailControls[(int)EIndex.ArrivalDate].Text,
                VendorDeliveryNo = detailControls[(int)EIndex.VendorDeliveryNo].Text,
                SoukoCD = CboSoukoName.SelectedIndex > 0 ? CboSoukoName.SelectedValue.ToString():"",
                ArrivalSu = txtSu.Text,
                JanCD = txtJANCD.Text,
                AdminNO = mAdminNO,
                SKUCD = lblSKUCD.Text,
                MakerItem = txtMakerItem.Text,
                StaffCD = InOperatorCD,
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dae;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("DataKbn", typeof(int));
            dt.Columns.Add("ArrivalRows", typeof(int));
            dt.Columns.Add("ArrivalKBN", typeof(int));
            dt.Columns.Add("OrderNO", typeof(string));
            dt.Columns.Add("OrderRows", typeof(int));
            //dt.Columns.Add("MoveNO", typeof(string));
            //dt.Columns.Add("MoveRows", typeof(int));
            dt.Columns.Add("ArrivalPlanNO", typeof(string));
            dt.Columns.Add("StockNO", typeof(string));
            dt.Columns.Add("ReserveNO", typeof(string));   
            dt.Columns.Add("CustomerCD", typeof(string));
            dt.Columns.Add("ArrivalSu", typeof(int));
            dt.Columns.Add("ArrivalPlanKBN", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity(out string vendorCD)
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;
            vendorCD = "";

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (mGrid.g_DArray[RW].Check || OperationMode == EOperationMode.DELETE)
                {
                    dt.Rows.Add(1
                        , rowNo
                        , 1
                        , mGrid.g_DArray[RW].JYUNO
                        , mGrid.g_DArray[RW].JYGNO
                        //, null  //MoveNO
                        //, null  //MoveRows
                        , mGrid.g_DArray[RW].ArrivalPlanNO
                        , mGrid.g_DArray[RW].StockNO
                        , mGrid.g_DArray[RW].ReserveNO
                        , mGrid.g_DArray[RW].CustomerCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].SURYO)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ArrivalPlanKBN)
                        , 0 //bbl.Z_Set(mGrid.g_DArray[RW].SURYO) != 0 && bbl.Z_Set(mGrid.g_DArray[RW].SURYO) != bbl.Z_Set(mGrid.g_DArray[RW].ReserveSu) ? 1 : 0
                        );
                    vendorCD = mGrid.g_DArray[RW].VendorCD;
                    rowNo++;
                }
            }
            for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (mGrid2.g_DArray[RW].Check || OperationMode == EOperationMode.DELETE)
                {
                    dt.Rows.Add(2
                        , rowNo
                        , 2
                        , mGrid2.g_DArray[RW].Number
                        , mGrid2.g_DArray[RW].RowNo
                        //, mGrid2.g_DArray[RW].MoveNO
                        //, mGrid2.g_DArray[RW].MoveRows
                        , mGrid2.g_DArray[RW].ArrivalPlanNO
                        , mGrid2.g_DArray[RW].StockNO
                        , mGrid2.g_DArray[RW].ReserveNO
                        , mGrid2.g_DArray[RW].CustomerCD
                        , bbl.Z_Set(mGrid2.g_DArray[RW].SURYO)
                        , bbl.Z_Set(mGrid2.g_DArray[RW].ArrivalPlanKBN)
                        , 0 //bbl.Z_Set(mGrid2.g_DArray[RW].SURYO) != 0 && bbl.Z_Set(mGrid2.g_DArray[RW].SURYO) != bbl.Z_Set(mGrid2.g_DArray[RW].OrderSu) - bbl.Z_Set(mGrid2.g_DArray[RW].ReserveSu) ? 1 : 0
                        );
                    vendorCD = mGrid2.g_DArray[RW].VendorCD;
                    rowNo++;
                }
            }
            return dt;
        }
        private DataTable GetGridCopyEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            for (int RW = 0; RW <= mGrid2.g_MK_Max_Row - 1; RW++)
            {
                //zが更新有効行数
                if (!string.IsNullOrWhiteSpace( mGrid2.g_DArray[RW].Number) && bbl.Z_Set(mGrid2.g_DArray[RW].SURYO) == 0 && mGrid2.g_DArray[RW].isCopy )
                {
                    dt.Rows.Add(2
                        , rowNo
                        , 2
                        , mGrid2.g_DArray[RW].Number
                        , mGrid2.g_DArray[RW].RowNo
                        , mGrid2.g_DArray[RW].ArrivalPlanNO
                        , mGrid2.g_DArray[RW].StockNO
                        , mGrid2.g_DArray[RW].ReserveNO
                        , mGrid2.g_DArray[RW].CustomerCD
                        , bbl.Z_Set(mGrid2.g_DArray[RW].SURYO)
                        , bbl.Z_Set(mGrid2.g_DArray[RW].ArrivalPlanKBN)
                        , 0
                        );
                    rowNo++;
                }
            }
            return dt;
        }
        protected override void ExecSec()
        {

            if (OperationMode == EOperationMode.INSERT)
            {
                //if (CheckKey((int)EIndex.StoreCD, false) == false)
                //{
                //    return;
                //}
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

            if (OperationMode != EOperationMode.DELETE)
            {
                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (mGrid.g_DArray[RW].Check)
                    {
                        for (int CL = (int)ClsGridHikiate.ColNO.SURYO; CL < (int)ClsGridHikiate.ColNO.COUNT; CL++)
                        {
                            bool focus = true;
                            if (CheckGrid(CL, RW, ref focus, true) == false)
                            {
                                if(focus)
                                    //Focusセット処理
                                    ERR_FOCUS_GRID_SUB(CL, RW);
                                return;
                            }
                        }
                    }
                }
                mGrid2.S_DispToArray(Vsb_Mei_1.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (mGrid2.g_DArray[RW].Check)
                    {
                        for (int CL = (int)ClsGridZaiko.ColNO.SURYO; CL < (int)ClsGridZaiko.ColNO.COUNT; CL++)
                        {
                            if (CheckGrid2(CL, RW, true) == false)
                            {
                                //Focusセット処理
                                ERR_FOCUS_GRID_SUB2(CL, RW);
                                return;
                            }
                        }
                    }
                }

                //各金額項目の再計算必要
                if (!CalcKin(true))
                    return;
            }

            string vendorCD = "";
            DataTable dt = GetGridEntity(out vendorCD);

            if(dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E111");
                return;
            }

            //更新処理
            dae = GetEntity();
            dae.VendorCD = vendorCD;

            bool ret = nnbl.Arrival_Exec(dae, dt, (short)OperationMode);

            if(!ret)
            {
                bbl.ShowMessage("E000");
                return;
            }

            DeleteOrder();

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        /// <summary>
        ///F10:入荷予定　押下時処理 
        /// </summary>
        private void ExecCopy()
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;
            int kbn = 0;

            if (mGrid.F_Search_Ctrl_MK(previousCtrl, out w_Col, out w_CtlRow))
            {
                kbn = 1;

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);
            }
            else if (mGrid2.F_Search_Ctrl_MK(previousCtrl, out w_Col, out w_CtlRow))
            {
                kbn = 2;

                //画面より配列セット 
                mGrid2.S_DispToArray(Vsb_Mei_1.Value);
            }

            string nyukaSu = "";
            string VendorCD = "";
            string VendorName = "";
            string OrderUnitPrice = "";
            string TaniCD = "";
            string PriceOutTax = "";
            string Rate = "";
            string OrderTaxRitsu = "";
            string OrderWayKBN = "";
            string AliasKBN = "";

            if (!CalcKin())
                return;

            switch (kbn)
            {
                case 0:
                    return;
                case 1:
                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    nyukaSu = bbl.Z_SetStr(bbl.Z_Set(lblHikiate.Text) + bbl.Z_Set(lblZaiko.Text));// + bbl.Z_Set( mGrid.g_DArray[w_Row].ReserveSu));  
                    VendorCD = mGrid.g_DArray[w_Row].VendorCD;
                    VendorName = mGrid.g_DArray[w_Row].VendorName;
                    OrderUnitPrice = mGrid.g_DArray[w_Row].OrderUnitPrice;
                    TaniCD = mGrid.g_DArray[w_Row].TaniCD;
                    PriceOutTax = mGrid.g_DArray[w_Row].PriceOutTax;
                    Rate = mGrid.g_DArray[w_Row].Rate;
                    OrderTaxRitsu = mGrid.g_DArray[w_Row].OrderTaxRitsu;
                    OrderWayKBN = mGrid.g_DArray[w_Row].OrderWayKBN;
                    AliasKBN = mGrid.g_DArray[w_Row].AliasKBN;
                    break;

                case 2:
                    w_Row = w_CtlRow + Vsb_Mei_1.Value;

                    nyukaSu = bbl.Z_SetStr(bbl.Z_Set(lblHikiate.Text) + bbl.Z_Set(lblZaiko.Text) + bbl.Z_Set(mGrid2.g_DArray[w_Row].ReserveSu));
                    VendorCD = mGrid2.g_DArray[w_Row].VendorCD;
                    VendorName = mGrid2.g_DArray[w_Row].VendorName;
                    OrderUnitPrice = mGrid2.g_DArray[w_Row].OrderUnitPrice;
                    TaniCD = mGrid2.g_DArray[w_Row].TaniCD;
                    PriceOutTax = mGrid2.g_DArray[w_Row].PriceOutTax;
                    Rate = mGrid2.g_DArray[w_Row].Rate;
                    OrderTaxRitsu = mGrid2.g_DArray[w_Row].OrderTaxRitsu;
                    OrderWayKBN = mGrid2.g_DArray[w_Row].OrderWayKBN;
                    AliasKBN = mGrid2.g_DArray[w_Row].AliasKBN;
                    break;
            }

            //追加入荷予定数＝画面.入荷総数－（ΣF10で追加した明細以外.入力した入荷数＋ΣF10で追加した明細の.予定数）							
            //この結果の追加入荷予定数＞０の場合だけ追加する
            decimal OrderSuu = bbl.Z_Set(detailControls[(int)EIndex.Nyukasu].Text) - bbl.Z_Set(nyukaSu) ;
            if (OrderSuu <= 0)
                return;

            D_Order_Entity de = new D_Order_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                ChangeDate = detailControls[(int)EIndex.ArrivalDate].Text,
                DestinationSoukoCD = CboSoukoName.SelectedValue.ToString(),
                OrderWayKBN = OrderWayKBN ,
                OrderCD = VendorCD,
                OrderPerson = VendorName,
                AliasKBN = AliasKBN,
                StaffCD = InOperatorCD,

                JANCD = txtJANCD.Text,
                AdminNO = mAdminNO,
                MakerItem = lblMaker.Text,
                SKUCD = lblSKUCD.Text,
                SKUName = lblSKUName.Text,
                ColorName = lblColorName.Text,
                SizeName = lblSizeName.Text,
            };
            de.OrderSuu = bbl.Z_SetStr(OrderSuu);
            de.OrderUnitPrice = bbl.Z_SetStr(OrderUnitPrice);
            de.TaniCD = TaniCD;
            de.PriceOutTax = PriceOutTax;
            de.Rate = Rate;
            de.OrderHontaiGaku = bbl.Z_SetStr(bbl.Z_Set(OrderSuu) * bbl.Z_Set(OrderUnitPrice));

            int TaxFractionKBN = GetTaxFractionKBN(VendorCD, de.ChangeDate);
            de.OrderTax = GetResultWithHasuKbn(TaxFractionKBN, bbl.Z_Set(OrderSuu) * bbl.Z_Set(OrderUnitPrice) * bbl.Z_Set(OrderTaxRitsu) / 100).ToString();
            de.OrderTaxRitsu = OrderTaxRitsu;
            de.Operator = InOperatorCD;

            DataTable dt = nnbl.Order_Exec(de);

            foreach (DataRow row in dt.Rows)
            {
                //追加した行を在庫データに表示
                int i2 = m_dataCnt2;
                mGrid2.g_DArray[i2].Check = false;
                mGrid2.g_DArray[i2].Number = row["JuchuuNO"].ToString();     //OrderNo
                mGrid2.g_DArray[i2].RowNo = row["JuchuuRows"].ToString();    //OrderRows
                mGrid2.g_DArray[i2].CustomerCD = row["CustomerCD"].ToString();
                mGrid2.g_DArray[i2].Customer = row["CustomerName2"].ToString();
                //mGrid2.g_DArray[i2].OrderSu = bbl.Z_SetStr(row["HachuSu"]);   // 
                mGrid2.g_DArray[i2].ReserveSu =bbl.Z_SetStr(row["ReserveSu"]);   //
                mGrid2.g_DArray[i2].SURYO = bbl.Z_SetStr(row["DR_ArrivalSu"]);
                mGrid2.g_DArray[i2].DirectFlg = row["DirectFlg"].ToString();
                mGrid2.g_DArray[i2].DeliveryPlanDate = row["DeliveryPlanDate"].ToString();
                mGrid2.g_DArray[i2].VendorCD = row["VendorCD"].ToString();
                mGrid2.g_DArray[i2].VendorName = row["VendorName"].ToString();
                mGrid2.g_DArray[i2].ArrivalPlanNO = row["ArrivalPlanNO"].ToString();
                mGrid2.g_DArray[i2].StockNO = row["StockNO"].ToString();
                mGrid2.g_DArray[i2].ReserveNO = row["ReserveNO"].ToString();
                mGrid2.g_DArray[i2].ArrivalPlanKBN = row["ArrivalPlanKBN"].ToString();

                mGrid2.g_DArray[i2].OrderUnitPrice = row["OrderUnitPrice"].ToString();
                mGrid2.g_DArray[i2].PriceOutTax = row["PriceOutTax"].ToString();
                mGrid2.g_DArray[i2].Rate = row["Rate"].ToString();
                mGrid2.g_DArray[i2].TaniCD = row["TaniCD"].ToString();
                mGrid2.g_DArray[i2].OrderTaxRitsu = row["OrderTaxRitsu"].ToString();
                mGrid2.g_DArray[i2].OrderWayKBN = row["OrderWayKBN"].ToString();
                mGrid2.g_DArray[i2].AliasKBN = row["AliasKBN"].ToString();
                mGrid2.g_DArray[i2].isCopy = true;

                m_dataCnt2 = i2 + 1;
                Grid_NotFocus2((int)ClsGridZaiko.ColNO.Number, i2);
                mGrid2.S_DispFromArray(0, ref Vsb_Mei_1);

            }
        }

        private int GetTaxFractionKBN(string VendorCD, string ChangeDate)
        {
            M_Vendor_Entity me = new M_Vendor_Entity
            {
                VendorCD = VendorCD,
                ChangeDate = ChangeDate,
                VendorFlg = "1"
            };

            Vendor_BL bl = new Vendor_BL();
            bool retV = bl.M_Vendor_SelectTop1(me);

            return Convert.ToInt16(me.TaxFractionKBN);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            DeleteOrder();

            //排他処理を解除
            DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    detailControls[(int)EIndex.VendorDeliveryNo].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    keyControls[0].Focus();
                    break;

            }

        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            //カスタムコントロールのLeave処理を先に走らせるため
            IMT_DMY_0.Focus();

            if (Kbn == 0)
            {
                foreach (Control ctl in keyControls)
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

                foreach (Control ctl in keyLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else
                {
                    ctl.Text = "";
                }
            }

            foreach (Control ctl in detailLabels)
            {
                ctl.Text = "";
            }

            mAdminNO = "";
            mJANCD = "";
            mOldArrivalDate = "";
            SetMakerVisible(false);
            S_Clear_Grid();   //画面クリア（明細部）
            S_Clear_Grid2();   //画面クリア（明細部）
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
                            foreach (Control ctl in keyControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            ScOrderNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;
                            
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;
                            Pnl_Body2.Enabled = Kbn == 0 ? true : false;
                            break;
                        }
                }
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
                case 7://F8:行追加
                    break;
                case 8: //F9:検索
                    if (previousCtrl.Name.Equals(txtJANCD.Name))
                    {
                        //商品検索
                            SearchData(EsearchKbn.Product, previousCtrl);
                    }
                    break;
                case 9://F10:入荷予定
                    //その行の内容をコピーして【在庫】に明細を追加したい
                    ExecCopy();
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

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            try
            {           
                DeleteOrder();
                DeleteExclusive();
            }
            catch(Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

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
                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.ArrivalDate].Text))
                    {
                        frmProduct.SKUCD = lblSKUCD.Text;
                        frmProduct.JANCD = detailControls[(int)EIndex.JANCD].Text;
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            detailControls[(int)EIndex.JANCD].Text = frmProduct.JANCD;
                            mJANCD = frmProduct.JANCD;
                            lblSKUCD.Text = frmProduct.SKUCD;
                            mAdminNO = frmProduct.AdminNO;

                            setCtl.Focus();

                            //CheckDetail((int)EIndex.JANCD, true);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
            }

        }
        private void SetMakerText(string text)
        {
            txtMakerItem.Text = text;
            lblMaker.Text = text;
        }
        private void SetMakerVisible(bool visible)
        {
            txtMakerItem.Visible = visible;
            lblMaker.Visible = !visible;
        }

        #region "内部イベント"
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
                        if(index == (int)EIndex.ArrivalNO)
                                detailControls[(int)EIndex.ArrivalDate].Focus();

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
                        if (index == (int)EIndex.Nyukasu)
                        {
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHikiate.ColNO.SURYO);

                            if (mGrid.F_Search_Ctrl_MK(this.ActiveControl, out int w_Col, out int w_CtlRow) == false)
                            {
                                //明細の先頭項目へ
                                mGrid2.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_1, Vsb_Mei_1.Value, (int)ClsGridZaiko.ColNO.SURYO);
                            }
                        }
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else if(index.Equals((int)EIndex.VendorDeliveryNo))
                                detailControls[(int)EIndex.Nyukasu].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
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

        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);
                if(index == (int)EIndex.JANCD)
                {
                    F9Visible = true;
                }
                //SetFuncKeyAll(this, "111111001011");
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
                Control w_ActCtl;
                int w_Row;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridHikiate.ColNO.SURYO)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;


                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    bool focus = true;

                    //チェック処理
                    if (CheckGrid(CL, w_Row, ref focus) == false)
                    {
                        //配列の内容を画面へセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        if (focus)
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

                    if (CL == (int)ClsGridHikiate.ColNO.Check)
                    {
                        if (e.Shift)
                            S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                        else
                            S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
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
        private void BtnSearch_Click(object sender, EventArgs e)
        {

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
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    bool lastCell = false;

                    if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridZaiko.ColNO.SURYO)
                        if (w_Row == mGrid2.g_MK_Max_Row - 1)
                            lastCell = true;


                    //画面の内容を配列へセット
                    mGrid2.S_DispToArray(Vsb_Mei_1.Value);

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
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid2.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridZaiko.ColNO.Check)
                    {
                        if (e.Shift)
                            S_Grid_1_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                        else
                            S_Grid_1_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
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
        private void CHK_Del_Click2(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_1.Value;

                //新規モードの場合、ONにした明細に紐づく会員名または発注先を取得し、画面項目の会員名または発注先にセット
                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    // 明細部  画面の範囲の内容を配列にセット
                    mGrid2.S_DispToArray(Vsb_Mei_1.Value);

                    //【在庫】の明細をONにした場合、発注先←	画面転送表02のD_Order②.発注先(移動元倉庫)またはD_Move②.発注先(移動元倉庫)									
                    if (mGrid2.g_DArray[w_Row].Check)
                    {
                        //既に選択済みの明細の入荷予定が入荷予定区分=1:発注分かつONにした明細の入荷予定が入荷予定区分=2:移動分の場合、エラー
                        for(int row=0; row<=mGrid2.g_MK_Max_Row - 1; row++)
                        {
                            if(row != w_Row && mGrid2.g_DArray[row].Check)
                            {
                                if (mGrid2.g_DArray[w_Row].ArrivalPlanKBN != mGrid2.g_DArray[row].ArrivalPlanKBN)
                                {
                                    //Ｅ２１２	メッセージ第一引数：「発注分」、メッセージ第二引数：「移動分」	 		
                                    bbl.ShowMessage("E212", "発注分", "移動分");
                                    return;
                                }
                            }
                        }

                        //mGrid2.g_DArray[w_Row].SURYO = bbl.Z_SetStr(bbl.Z_Set(mGrid2.g_DArray[w_Row].OrderSu) - bbl.Z_Set(mGrid2.g_DArray[w_Row].ReserveSu));
                        mGrid2.g_DArray[w_Row].SURYO = bbl.Z_SetStr(mGrid2.g_DArray[w_Row].ReserveSu);

                        if (!string.IsNullOrWhiteSpace(lblVendor.Text) && !lblVendor.Text.Equals(mGrid2.g_DArray[w_Row].Customer))
                        {
                            //Ｅ１９９				
                            bbl.ShowMessage("E199");
                            mGrid2.g_DArray[w_Row].Check = false;

                            //配列の内容を画面にセット
                            mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);
                            return;
                        }
                        lblVendor.Text = mGrid2.g_DArray[w_Row].Customer;

                        CalcKin();

                        //配列の内容を画面にセット
                        mGrid2.S_DispFromArray(Vsb_Mei_1.Value, ref Vsb_Mei_1);
                    }
                    else
                    {
                        ClearVendor();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF12_Click(object sender, EventArgs e)
        {
            try
            {
                FunctionProcess(FuncExec - 1);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        
        /// <summary>
        /// 明細部削除チェックボックスクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CHK_Del_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //新規モードの場合、ONにした明細に紐づく会員名または発注先を取得し、画面項目の会員名または発注先にセット
                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    // 明細部  画面の範囲の内容を配列にセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //【引当】の明細をONにした場合、会員名←	画面転送表02のD_Reserve②.発注先								
                    if (mGrid.g_DArray[w_Row].Check)
                    {
                        mGrid.g_DArray[w_Row].SURYO = mGrid.g_DArray[w_Row].ReserveSu;

                        if (!string.IsNullOrWhiteSpace(lblVendor.Text) && !lblVendor.Text.Equals(mGrid.g_DArray[w_Row].VendorName))
                        {
                            //Ｅ１９９				
                            bbl.ShowMessage("E199");
                            mGrid.g_DArray[w_Row].Check = false;

                            //配列の内容を画面にセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            CalcKin();
                            return;
                        }
                        lblVendor.Text = mGrid.g_DArray[w_Row].VendorName;

                        CalcKin();

                        //配列の内容を画面にセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    }
                    else
                    {
                        ClearVendor();
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
        /// <summary>
        /// 明細部在庫ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Detail_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                FrmJuchuu frm = new FrmJuchuu();
                frm.JuchuuNO = mGrid.g_DArray[w_Row].JYUNO;
                frm.CustomerName = mGrid.g_DArray[w_Row].Customer;

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnJANCD_Click(object sender, EventArgs e)
        {
            try
            {
                SearchData(EsearchKbn.Product, txtJANCD);
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








