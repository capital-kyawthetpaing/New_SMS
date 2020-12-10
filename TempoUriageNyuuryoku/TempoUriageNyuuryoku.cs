using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace TempoUriageNyuuryoku
{
    /// <summary>
    /// TempoUriageNyuuryoku 店舗売上入力
    /// </summary>
    internal partial class TempoUriageNyuuryoku : FrmMainForm
    {
        private const string ProID = "TempoUriageNyuuryoku";
        private const string ProNm = "店舗売上入力";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TempoNouhinsyo.exe";

        private enum EIndex : int
        {
            SalesDate,
            StoreCD,

            JuchuuDateFrom,
            JuchuuDateTo,
            SalesDateFrom,
            SalesDateTo,
            StaffCD,

            ChkSokuSeikyu,
            ChkNouhinsho,
            CustomerCD,
            CustomerName,
            KanaName,
            COUNT
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private TempoUriageNyuuryoku_BL tubl;
        private D_Juchuu_Entity dje;
        private DataTable dtDetail;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldDenNo = "";    //排他処理のため使用


        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridTempoUriage mGrid = new ClsGridTempoUriage();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数

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

            if (ClsGridTempoUriage.gc_P_GYO <= ClsGridTempoUriage.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridTempoUriage.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridTempoUriage.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridTempoUriage.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridTempoUriage.gc_MaxCL;

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
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridTempoUriage.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridTempoUriage.ColNO.GYONO; i <= (int)ClsGridTempoUriage.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridTempoUriage.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridTempoUriage.ColNO.JuchuuSuu:
                        case (int)ClsGridTempoUriage.ColNO.SalesSU:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 0].CellCtl = CHK_EDICK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 0].CellCtl = IMN_SIRGK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 0].CellCtl = IMN_CALGK_0;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 0].CellCtl =  IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 0].CellCtl = IMT_PAYDT_0;    //支払予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 1].CellCtl = CHK_EDICK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 1].CellCtl = IMN_SIRGK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 1].CellCtl = IMN_CALGK_1;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 1].CellCtl = IMT_REMAK_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 1].CellCtl = IMT_PAYDT_1;    //支払予定日

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 2].CellCtl = IMN_SIRGK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 2].CellCtl = IMN_CALGK_2;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 2].CellCtl = CHK_EDICK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 2].CellCtl = IMT_PAYDT_2;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 3].CellCtl = IMN_SIRGK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 3].CellCtl = IMN_CALGK_3;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 3].CellCtl = CHK_EDICK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 3].CellCtl =  IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 3].CellCtl = IMT_PAYDT_3;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 4].CellCtl = IMN_SIRGK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 4].CellCtl = IMN_CALGK_4;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 4].CellCtl = CHK_EDICK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 4].CellCtl =  IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 4].CellCtl = IMT_PAYDT_4;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space,5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 5].CellCtl = IMN_SIRGK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 5].CellCtl = IMN_CALGK_5;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 5].CellCtl = CHK_EDICK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 5].CellCtl =  IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 5].CellCtl = IMT_PAYDT_5;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 6].CellCtl = IMN_SALEP2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 6].CellCtl = IMN_SIRGK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 6].CellCtl = IMN_CALGK_6;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 6].CellCtl = CHK_EDICK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 6].CellCtl = IMT_PAYDT_6;    //支払予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 7].CellCtl = IMN_SALEP2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 7].CellCtl = IMN_SIRGK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice,7].CellCtl = IMN_CALGK_7;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 7].CellCtl = CHK_EDICK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 7].CellCtl = IMT_PAYDT_7;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 8].CellCtl = IMN_SALEP2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 8].CellCtl = IMN_SIRGK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 8].CellCtl = IMN_CALGK_8;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 8].CellCtl = CHK_EDICK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 8].CellCtl = IMN_GENER2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 8].CellCtl = IMN_MEMBR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 8].CellCtl = IMN_WEBPR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 8].CellCtl = IMT_JUONO_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 8].CellCtl = IMT_PAYDT_8;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 9].CellCtl = IMN_SALEP2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 9].CellCtl = IMN_SIRGK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 9].CellCtl = IMN_CALGK_9;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 9].CellCtl = CHK_EDICK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 9].CellCtl = IMN_GENER2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 9].CellCtl = IMN_MEMBR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 9].CellCtl = IMN_WEBPR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 9].CellCtl = IMT_REMAK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 9].CellCtl = IMT_JUONO_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 9].CellCtl = IMT_ARIDT_9;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 9].CellCtl = IMT_PAYDT_9;    //支払予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.GYONO, 10].CellCtl = IMT_GYONO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Space, 10].CellCtl = IMN_SALEP2_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuGaku, 10].CellCtl = IMN_SIRGK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuUnitPrice, 10].CellCtl = IMN_CALGK_10;    //支払予定
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.Chk, 10].CellCtl = CHK_EDICK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuNO, 10].CellCtl = IMT_ITMCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JanCD, 10].CellCtl = IMT_JANCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SizeName, 10].CellCtl = IMT_KAIDT_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SKUName, 10].CellCtl = IMT_ITMNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuSuu, 10].CellCtl = IMN_TEIKA_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesSU, 10].CellCtl = IMN_GENER2_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.TaniCD, 10].CellCtl = IMN_MEMBR_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.ColorName, 10].CellCtl = IMN_CLINT_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName, 10].CellCtl = IMN_WEBPR_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerName2, 10].CellCtl = IMN_WEBPR2_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.CustomerCD, 10].CellCtl = IMT_REMAK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesNO, 10].CellCtl = IMT_JUONO_10;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.SalesDate, 10].CellCtl = IMT_ARIDT_10;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridTempoUriage.ColNO.JuchuuDate, 10].CellCtl = IMT_PAYDT_10;    //支払予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridTempoUriage.Gen_MK_FocusMove.MvNxt, (int)ClsGridTempoUriage.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridTempoUriage.Gen_MK_FocusMove.MvPrv, (int)ClsGridTempoUriage.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridTempoUriage.Gen_MK_FocusMove.MvNxt, (int)ClsGridTempoUriage.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridTempoUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridTempoUriage.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridTempoUriage.Gen_MK_FocusMove.MvSet, (int)ClsGridTempoUriage.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                            case (int)ClsGridTempoUriage.ColNO.Chk:

                                break;

                            default:
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
                    //    case  (int)ClsGridTempoUriage.ColNO.Chk:
                    //        {
                    //            // チェック
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
                            //// 入力可の列の設定
                            //for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            //{
                            //    if (m_EnableCnt - 1 < w_Row)
                            //        break;

                            //    //Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            //    mGrid.g_MK_State[(int)ClsGridTempoUriage.ColNO.Chk, w_Row].Cell_Enabled = true;

                            //}
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                detailControls[(int)EIndex.SalesDateFrom].Text = "";
                                detailControls[(int)EIndex.SalesDateFrom].Enabled = false;
                                detailControls[(int)EIndex.SalesDateTo ].Text = "";
                                detailControls[(int)EIndex.SalesDateTo].Enabled = false;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.SalesDate].Text = bbl.GetDate();
                                F9Visible = false;

                                if (BtnSubF11.Enabled)
                                    SetFuncKeyAll(this, "110101001011");
                                else
                                    SetFuncKeyAll(this, "110101001001");
                            }
                            else
                            {

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック

                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }
                        }
                        break;
                    }

                case 1: //修正時
                    {
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                // 'OrderNoがある場合、入力可（有効行）
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JuchuuNO) && string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SalesNO))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    { 
                                        case (int)ClsGridTempoUriage.ColNO.Chk:    // 
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                        default:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
                            Scr_Lock(2, 3, 0);
                            //Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "100001000011");
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
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridMitsumori.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            //SetFuncKeyAll(this, "111111111111");
            SetFuncKey(this, 6, true);
            SetFuncKey(this, 7, true);
            SetFuncKey(this, 9, true);

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

        // -----------------------------------------------
        // <明細部>行削除処理 Ｆ７
        // -----------------------------------------------
        private void DEL_SUB()
        {
       
        }
      
        private void Grid_Gyo_Clr(int RW)  // 明細部１行クリア
        {

        }

        #endregion

        public TempoUriageNyuuryoku()
        {
            InitializeComponent();

            //ホイールイベントの追加  
            this.Vsb_Mei_0.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);

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

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                tubl = new TempoUriageNyuuryoku_BL();
                CboStoreCD.Bind(ymd);

                //検索用のパラメータ設定
                ScCustomerCD.Value1 = "1";
                ScCustomerCD.Value2 = "";
                
                Btn_F3.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "";
                
                ChangeOperationMode(EOperationMode.INSERT);
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
            detailControls = new Control[] {ckM_TextBox18, CboStoreCD, ckM_TextBox1 ,ckM_TextBox2 ,ckM_TextBox6, ckM_TextBox5, ScStaffCD.TxtCode
                    , ChkSokuSeikyu, ChkNouhinsho   ,ScCustomerCD.TxtCode, txtCustomerName,txtKanaName       };
            detailLabels = new Control[] { ScCustomerCD, ScStaffCD };
            searchButtons = new Control[] { ScCustomerCD.BtnSearch,  ScStaffCD.BtnSearch };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }

        private bool SelectAndInsertExclusive(string denNo, Exclusive_BL.DataKbn kbn)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)kbn,
                Number = denNo,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                //detailControls[(int)EIndex.Edi].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive(DataTable dtForUpdate = null, int kbn=0)
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                mOldDenNo = "";
                if (kbn == 0)
                {
                    DeleteExclusive(dtForUpdate,(int)Exclusive_BL.DataKbn.Jyuchu);
                    DeleteExclusive(dtForUpdate, (int)Exclusive_BL.DataKbn.Uriage);
                    DeleteExclusive(dtForUpdate, (int)Exclusive_BL.DataKbn.Seikyu);
                    return;
                }

                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    switch (kbn)
                    {
                        case (int)Exclusive_BL.DataKbn.Jyuchu:
                            if (mOldDenNo != dr["JuchuuNo"].ToString())
                            {
                                D_Exclusive_Entity de = new D_Exclusive_Entity
                                {
                                    DataKBN = (int)kbn,
                                    Number = dr["JuchuuNo"].ToString()
                                };

                                ebl.D_Exclusive_Delete(de);

                                mOldDenNo = dr["JuchuuNo"].ToString();
                            }
                            break;
                        case (int)Exclusive_BL.DataKbn.Uriage:
                            if (mOldDenNo != dr["SalesNO"].ToString())
                            {
                                D_Exclusive_Entity de = new D_Exclusive_Entity
                                {
                                    DataKBN = (int)kbn,
                                    Number = dr["SalesNO"].ToString()
                                };

                                ebl.D_Exclusive_Delete(de);

                                mOldDenNo = dr["SalesNO"].ToString();
                            }
                            break;
                        case (int)Exclusive_BL.DataKbn.Seikyu:
                            if (mOldDenNo != dr["BillingNO"].ToString())
                            {
                                D_Exclusive_Entity de = new D_Exclusive_Entity
                                {
                                    DataKBN = (int)kbn,
                                    Number = dr["BillingNO"].ToString()
                                };

                                ebl.D_Exclusive_Delete(de);

                                mOldDenNo = dr["BillingNO"].ToString();
                            }
                            break;
                    }
                }
                return;
            }
        }

        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set)
        {
            Exclusive_BL.DataKbn kbn = Exclusive_BL.DataKbn.Null;

            //新規モード時
            if (OperationMode == EOperationMode.INSERT)
            {
                kbn = Exclusive_BL.DataKbn.Jyuchu;
            }
            else
            {
                kbn = Exclusive_BL.DataKbn.Uriage;
            }

            DeleteExclusive(dtDetail);

            dje = GetEntity(kbn);

            //[D_Juchu_SelectAllForUriage]
            dtDetail = tubl.D_Juchu_SelectDataForTempoUriage(dje, (short)OperationMode);

            if (dtDetail.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;

                mOldDenNo = "";
                string oldBillingNo = "";    //排他処理のため使用

                foreach (DataRow row in dtDetail.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }
                    if (OperationMode == EOperationMode.INSERT)
                    {
                        if (mOldDenNo != row["JuchuuNO"].ToString())
                        {
                            bool ret = SelectAndInsertExclusive(row["JuchuuNO"].ToString(), kbn);
                            if (!ret)
                                return false;

                            mOldDenNo = row["JuchuuNO"].ToString();
                        }
                        mGrid.g_DArray[i].JuchuuSuu = bbl.Z_SetStr(row["JuchuuSuu"]);
                        mGrid.g_DArray[i].SalesSU = "";
                    }
                    else
                    {
                        if (mOldDenNo != row["SalesNO"].ToString())
                        {
                            bool ret = SelectAndInsertExclusive(row["SalesNO"].ToString(), Exclusive_BL.DataKbn.Uriage);
                            if (!ret)
                                return false;

                            mOldDenNo = row["SalesNO"].ToString();
                        }
                        if (oldBillingNo != row["BillingNO"].ToString())
                        {
                            bool ret = SelectAndInsertExclusive(row["BillingNO"].ToString(), Exclusive_BL.DataKbn.Seikyu);
                            if (!ret)
                                return false;

                            oldBillingNo = row["BillingNO"].ToString();
                        }
                        mGrid.g_DArray[i].JuchuuSuu = "";
                        mGrid.g_DArray[i].SalesSU = bbl.Z_SetStr(row["SalesSU"]);
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    //mGrid.g_DArray[i].OldJanCD = mGrid.g_DArray[i].JanCD;
                    //mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    //mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                    mGrid.g_DArray[i].JuchuuRows = row["JuchuuRows"].ToString();
                    mGrid.g_DArray[i].JuchuuDate = row["JuchuuDate"].ToString();
                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].SalesNO = row["SalesNO"].ToString();   // 
                    mGrid.g_DArray[i].SalesRows = row["SalesRows"].ToString();   // 
                    mGrid.g_DArray[i].SalesDate = row["SalesDate"].ToString();
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 
                    mGrid.g_DArray[i].JuchuuUnitPrice = bbl.Z_SetStr(row["UnitPrice"]);
                    mGrid.g_DArray[i].JuchuuGaku = bbl.Z_SetStr(row["Kingaku"]);

                    mGrid.g_DArray[i].TaniName = row["TaniName"].ToString();
                    mGrid.g_DArray[i].CustomerCD = row["CustomerCD"].ToString();   // 
                    mGrid.g_DArray[i].CustomerName = row["CustomerName"].ToString();   //  
                    mGrid.g_DArray[i].CustomerName2 = row["CustomerName2"].ToString();   //  

                    //隠し項目
                    //mGrid.g_DArray[i].BillingNO = row["BillingNO"].ToString();
                    mGrid.g_DArray[i].Update = 0;

                    m_dataCnt = i + 1;
                    i++;
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            S_BodySeigyo(1, 0);
            S_BodySeigyo(1, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        protected override void ExecDisp()
        {
            for (int i = 0; i < (int)EIndex.COUNT; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            bool ret = CheckData(true);
            if (ret)
            {
                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridTempoUriage.ColNO.Chk);
            }
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
                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboStoreCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }
                    }

                    break;

                case (int)EIndex.JuchuuDateFrom:
                case (int)EIndex.JuchuuDateTo:
                case (int)EIndex.SalesDateFrom:
                case (int)EIndex.SalesDateTo:
                    //新規モード時、入力必須(Entry required)
                    if (OperationMode == EOperationMode.INSERT)
                    {
                        if (index == (int)EIndex.JuchuuDateFrom || index == (int)EIndex.JuchuuDateTo)
                        {
                            if (!RequireCheck(new Control[] { detailControls[index] }))
                            {
                                return false;
                            }
                        }
                    }
                    //削除モード時、入力必須(Entry required)
                    else if (OperationMode == EOperationMode.DELETE)
                    {
                        if (index == (int)EIndex.SalesDateFrom || index == (int)EIndex.SalesDateTo)
                        {
                            if (!RequireCheck(new Control[] { detailControls[index] }))
                            {
                                return false;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    string strYmd = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                            detailControls[index].Text = strYmd;

                    if (index == (int)EIndex.SalesDateFrom || index == (int)EIndex.SalesDateTo)
                    {
                        //店舗の締日チェック
                        //店舗締マスターで判断
                        M_StoreClose_Entity me = new M_StoreClose_Entity();
                        me.StoreCD = CboStoreCD.SelectedValue == null ? "": CboStoreCD.SelectedValue.ToString();
                        me.FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6);
                        ret = tubl.CheckStoreClose(me, true, false, false, false, true);
                        if (!ret)
                        {
                            detailControls[index].Focus();
                            return false;
                        }
                    }

                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.JuchuuDateTo || index == (int)EIndex.SalesDateTo)
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
                    

                case (int)EIndex.CustomerCD:                    
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate =  detailControls[(int)EIndex.SalesDate].Text
                    };
                    Customer_BL sbl = new Customer_BL();
                    ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        ScCustomerCD.LabelText = mce.CustomerName;
                        txtCustomerName.Text = mce.CustomerName;
                        txtKanaName.Text = mce.KanaName;
                    }
                    else
                    {
                        //情報ALLクリア
                        ClearCustomerInfo();
                        tubl.ShowMessage("E101");
                        return false;
                    }

                    break;

                case (int)EIndex.StaffCD:
                    ScStaffCD.LabelText = "";

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity msf = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate() // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Staff_BL bl = new Staff_BL();
                    ret = bl.M_Staff_Select(msf);
                    if (ret)
                    {
                        ScStaffCD.LabelText = msf.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;

                case (int)EIndex.SalesDate:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
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
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    //店舗の締日チェック
                    //店舗締マスターで判断
                    M_StoreClose_Entity mse = new M_StoreClose_Entity();
                    mse.StoreCD = CboStoreCD.SelectedValue == null ? "" : CboStoreCD.SelectedValue.ToString();
                    mse.FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6);
                    ret = tubl.CheckStoreClose(mse, true, false, false, false, true);
                    if (!ret)
                    {
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

            }

            return true;
        }

        private D_Juchuu_Entity GetEntity(Exclusive_BL.DataKbn kbn)
        {
            dje = new D_Juchuu_Entity
            {
                JuchuDateFrom = detailControls[(int)EIndex.JuchuuDateFrom].Text,
                JuchuDateTo = detailControls[(int)EIndex.JuchuuDateTo].Text,
                //OrderDateFrom = detailControls[(int)EIndex.SalesDateFrom].Text,
                //OrderDateTo = detailControls[(int)EIndex.SalesDateTo].Text,

                StoreCD = CboStoreCD.SelectedValue.ToString(),
                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                CustomerName= detailControls[(int)EIndex.CustomerName].Text,
                KanaName = detailControls[(int)EIndex.KanaName].Text,

                StaffCD = detailControls[(int)EIndex.StaffCD].Text
            };

            if(kbn == Exclusive_BL.DataKbn.Uriage)
            {
                dje.JuchuDateFrom = detailControls[(int)EIndex.SalesDateFrom].Text;
                dje.JuchuDateTo = detailControls[(int)EIndex.SalesDateTo].Text;
            }

            return dje;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("DenNO", typeof(string));
            dt.Columns.Add("DenRows", typeof(int));
            dt.Columns.Add("SalesSU", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);
            
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (mGrid.g_DArray[RW].Chk)
                {
                    int rowNo = 1;
                    if (OperationMode == FrmMainForm.EOperationMode.INSERT)
                    {
                        dt.Rows.Add(mGrid.g_DArray[RW].JuchuuNO
                             , mGrid.g_DArray[RW].JuchuuRows
                             , bbl.Z_Set(mGrid.g_DArray[RW].JuchuuSuu)
                             , mGrid.g_DArray[RW].Update
                             );
                    }
                    else
                    {
                        dt.Rows.Add(mGrid.g_DArray[RW].SalesNO
                             , mGrid.g_DArray[RW].SalesRows
                             , bbl.Z_Set(mGrid.g_DArray[RW].SalesSU)
                             , mGrid.g_DArray[RW].Update
                             );
                    }
                    rowNo++;

                }
            }

            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetCalcuArrivalPlanDate(string arrivalPlanDate , string arrivalPlanMonth,string arrivalPlanCD)
        {
            string calcuArrivalPlanDate = "";

            //入荷予定日≠Nullの場合そのまま、入荷予定日	
            if (!string.IsNullOrWhiteSpace(arrivalPlanDate))
            {
                calcuArrivalPlanDate = arrivalPlanDate;
            }
            else
            {
                //入荷予定年月≠Nullの場合
                if (bbl.Z_Set(arrivalPlanMonth.Replace("/", "")) != 0)
                {
                    //入荷予定区分
                    M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                    {
                        ID = MultiPorpose_BL.ID_ArrivalPlanCD,
                        Key = arrivalPlanCD
                    };

                    MultiPorpose_BL mbl = new MultiPorpose_BL();
                    DataTable  dt = mbl.M_MultiPorpose_Select(me);
                    if (dt.Rows.Count > 0)
                    {
                        //Num2＝1 の場合
                        //Num1	を日として、年月＋日とする。日＝31の時は月末の意味なので、小の月に注意（例 2月や4月）																												
                        if (dt.Rows[0]["Num2"].ToString() == "1")
                        {
                            string day = string.Format("{0:00}", dt.Rows[0]["Num1"].ToString());
                            string ymd;
                            if (day != "31")
                            {
                                calcuArrivalPlanDate = bbl.FormatDate(arrivalPlanMonth + day);
                            }
                            else
                            {
                                ymd = bbl.FormatDate(arrivalPlanMonth + "01");    //月末を後で計算
                                calcuArrivalPlanDate = Convert.ToDateTime(ymd).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                            }
                        }
                        else
                        {
                            //Num2≠1 の場合
                        }
                    }
                }
                else
                {
                    //入荷予定年月＝Nullの場合 Null
                }
            }

            return calcuArrivalPlanDate;
        }
        protected override void ExecSec()
        {

            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            DataTable dt = GetGridEntity();

            //明細が一件も選択されていない場合、Error Ｅ２１６				
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E216");

                previousCtrl.Focus();
                return;
            }

            //お買上日から入金予定日を自動計算
            Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity
            {
                KaisyuShiharaiKbn = "0",    // "1";
                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                ChangeDate = detailControls[(int)EIndex.SalesDate].Text,
                TyohaKbn = "0"
            };

            D_Sales_Entity dse = new D_Sales_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                SalesDate = detailControls[(int)EIndex.SalesDate].Text,
                Operator = InOperatorCD,
                PC = InPcID
            };

            dse.BillingType = ChkSokuSeikyu.Checked ? "1" : "0";

            //更新処理 
            tubl.Uriage_Exec(dse, dt, (short)OperationMode);

            //Form.納品書印刷CheckBox＝onの場合、印刷Program(TempoNouhinsyo)	を起動		
            if (OperationMode == FrmMainForm.EOperationMode.INSERT && ChkNouhinsho.Checked)
            {
                if (bbl.ShowMessage("Q202") == DialogResult.Yes)
                {
                    mOldDenNo = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (mOldDenNo != dr["DenNO"].ToString())
                        {
                            mOldDenNo = dr["DenNO"].ToString();
                            D_Juchuu_Entity de = new D_Juchuu_Entity
                            {
                                JuchuuNO = mOldDenNo
                            };

                            string SalesNO =  tubl.D_Juchu_SelectData(de);

                            //印刷Program(TempoNouhinsyo)を起動
                            ExecPrint(SalesNO);

                        }
                    }
                }
            }

            bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ExecPrint(string no)
        {
            //Form.印刷CheckBox＝onの場合、印刷Program(TempoNouhinsyo)を起動					
            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoNouhinsyo;
            if (System.IO.File.Exists(filePath))
            {
                //売上番号,起動区分＝１					
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + no + " 1";
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive(dtDetail);

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            detailControls[(int)EIndex.SalesDate].Focus();

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
                foreach (Control ctl in detailControls)
                {
                    if (ctl.Equals(CboStoreCD))
                        continue;

                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                        ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                    {

                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

                //顧客情報ALLクリア
                ClearCustomerInfo();

                foreach (Control ctl in detailLabels)
                {
                    ((CKM_SearchControl)ctl).LabelText = "";
                }

            }

            S_Clear_Grid();   //画面クリア（明細部）

        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScCustomerCD.LabelText = "";
            txtCustomerName.Text = "";
            txtKanaName.Text = "";
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
                            foreach (Control ctl in detailControls)
                            {
                                ctl.Enabled = Kbn == 0 ? true : false;
                            }
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;
                            break;
                        }

                    case 1:
                        {
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                // ｷｰ部(削除時)
                                detailControls[(int)EIndex.JuchuuDateFrom].Text = "";
                                detailControls[(int)EIndex.JuchuuDateFrom].Enabled = false;
                                detailControls[(int)EIndex.JuchuuDateTo].Text = "";
                                detailControls[(int)EIndex.JuchuuDateTo].Enabled = false;
                                ChkNouhinsho.Enabled = false;

                                detailControls[(int)EIndex.SalesDateFrom].Enabled = true;
                                detailControls[(int)EIndex.SalesDateTo].Enabled = true;
                            }
                                break;
                        }

                    case 2:
                        {
                            break;
                        }

                    case 3:
                        {
                            // 明細部
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;
                            
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
                case 6://F7:
                    break;

                case 7://F8:
                    break;

                case 9://F10
                    break;

                case 8: //F9:検索
                    break;

                case 11:    //F12:登録
                    {
                        //Ｑ１０１		
                        if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                            return;

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
                DeleteExclusive(dtDetail);
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

                         if (index == (int)EIndex.COUNT-1)
                            BtnSubF11.Focus();
                            
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

                    //bool changeFlg = false;
                    //if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    //    changeFlg = ((CKM_Controls.CKM_TextBox)w_ActCtl).Modified;
                    //else if (w_ActCtl.GetType().Equals(typeof(CKM_SearchControl)))
                    //    changeFlg = ((CKM_SearchControl)w_ActCtl).TxtCode.Modified;

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridTempoUriage.ColNO.CustomerName2)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;


                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    ////手入力時金額を再計算
                    //if (changeFlg)
                    //{
                    //    switch (CL)
                    //    {
                    //        case (int)ClsGridKaitouNouki.ColNO.OrderSu:
                    //        case (int)ClsGridKaitouNouki.ColNO.ArrivalPlanMonth:

                    //            //変更された場合、金額再計算 発注単価×発注数
                    //            mGrid.g_DArray[w_Row].ArrivalPlanCD = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[w_Row].ArrivalPlanMonth) * bbl.Z_Set(mGrid.g_DArray[w_Row].OrderSu));
                    //            break;
                    //    }
                    //}

                    ////チェック処理
                    //if (CheckGrid(CL, w_Row) == false)
                    //{
                    //    //Focusセット処理
                    //    w_ActCtl.Focus();
                    //    return;
                    //}

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

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF11_Click(object sender, EventArgs e)
        {
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

                int kbn = mGrid.g_DArray[w_Row].JuchuuNO == "" ? 2 : 1;
                string denNo = kbn == 2 ? mGrid.g_DArray[w_Row].SalesNO : mGrid.g_DArray[w_Row].JuchuuNO;

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //チェックON時、同一受注番号の明細もONにする。
                //チェックOFF時、同一受注番号の明細もOFFにする。
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (kbn.Equals(1))
                    {
                        if (mGrid.g_DArray[RW].JuchuuNO == denNo)
                        {
                            mGrid.g_DArray[RW].Chk = mGrid.g_DArray[w_Row].Chk;
                        }
                    }
                    else
                    {
                        if (mGrid.g_DArray[RW].SalesNO == denNo)
                        {
                            mGrid.g_DArray[RW].Chk = mGrid.g_DArray[w_Row].Chk;
                        }
                    }
                }

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

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomerCD.Value2 = CboStoreCD.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}








