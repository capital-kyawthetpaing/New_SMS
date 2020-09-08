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
using System.IO;
//using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace MarkDownNyuuryoku
{
    /// <summary>
    /// MarkDownNyuuryoku マークダウン入力
    /// </summary>
    internal partial class MarkDownNyuuryoku : FrmMainForm
    {
        private const string ProID = "MarkDownNyuuryoku";
        private const string ProNm = "マークダウン入力";
        private const short mc_L_END = 3; // ロック用

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }

        /// <summary>
        /// 月次締状況
        /// </summary>
        private enum EClose : short
        {
            NotClose,
            Closed
        }

        private enum EIndex : int
        {
            MarkDownNO = 0,

            VendorCD = 0,
            SoukoCD,
            StockInfo,
            CostingDate,
            UnitPriceDate,
            ExpectedPurchaseDate,
            PurchaseDate,
            StaffCD,
            Remark,
            Rate,
            UnitPrice,
            Su
        }

        /// <summary>
        /// 予定/結果
        /// </summary>
        private enum EKbn : short
        {
            Plan,
            Result
        }

        private enum EColNo : int
        {
            JanCD,
            Rate,
            MarkDownUnitPrice,
            CalculationSu,

            COUNT
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        private Control[] radioButtons;

        private MarkDownNyuuryoku_BL mdbl;
        private D_MarkDown_Entity dme;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldPurchaseDate = "";
        private string mStoreCD = "";
        private string mPayeeCD = "";
        private string mMDPurchaseNO;
        private string mPurchaseNO;
        private decimal mTaxGaku8;
        private decimal mTaxGaku10;
        private decimal mHontaiGaku8;
        private decimal mHontaiGaku10;
        private decimal mTaxCalcGaku;
        private EClose mCloseState;

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridMarkDown mGrid = new ClsGridMarkDown();
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

            if (ClsGridMarkDown.gc_P_GYO <= ClsGridMarkDown.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMarkDown.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridMarkDown.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridMarkDown.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMarkDown.gc_MaxCL;

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
                            //sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
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
                            check.Click += new System.EventHandler(CHK_Sel_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridMarkDown.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridMarkDown.ColNO.GYONO; i <= (int)ClsGridMarkDown.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridMarkDown.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridMarkDown.ColNO.StockSu:
                        case (int)ClsGridMarkDown.ColNO.CalculationSu:
                            // 数量
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridMarkDown.ColNO.PriceOutTax:
                        case (int)ClsGridMarkDown.ColNO.EvaluationPrice:
                        case (int)ClsGridMarkDown.ColNO.MarkDownUnitPrice:
                        case (int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice:
                        case (int)ClsGridMarkDown.ColNO.MarkDownGaku:
                            // 単価・金額
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                        case (int)ClsGridMarkDown.ColNO.Rate:
                            // 率
                            mGrid.SetProp_Ritu(3, 2, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = false;
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
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 0].CellCtl = CHK_SELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 0].CellCtl = IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 0].CellCtl = IMN_COLNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 0].CellCtl = IMT_SIZNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 0].CellCtl = IMT_MAKER_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 0].CellCtl = IMT_SKUCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 0].CellCtl = IMN_COST_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 0].CellCtl = IMN_RATE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 0].CellCtl = IMN_MDTAN_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 0].CellCtl = IMN_MDSGK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 0].CellCtl = IMN_ZAISU_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 0].CellCtl = IMN_SURYO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 0].CellCtl = IMN_PURKN_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 1].CellCtl = CHK_SELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 1].CellCtl = IMN_COLNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 1].CellCtl = IMT_SIZNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 1].CellCtl = IMT_MAKER_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 1].CellCtl = IMT_SKUCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 1].CellCtl = IMN_COST_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 1].CellCtl = IMN_RATE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 1].CellCtl = IMN_MDTAN_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 1].CellCtl = IMN_MDSGK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 1].CellCtl = IMN_ZAISU_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 1].CellCtl = IMN_SURYO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 1].CellCtl = IMN_PURKN_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 2].CellCtl = CHK_SELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 2].CellCtl = IMN_COLNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 2].CellCtl = IMT_SIZNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 2].CellCtl = IMT_MAKER_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 2].CellCtl = IMT_SKUCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 2].CellCtl = IMN_COST_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 2].CellCtl = IMN_RATE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 2].CellCtl = IMN_MDTAN_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 2].CellCtl = IMN_MDSGK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 2].CellCtl = IMN_ZAISU_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 2].CellCtl = IMN_SURYO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 2].CellCtl = IMN_PURKN_2;

            // 4行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 3].CellCtl = CHK_SELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 3].CellCtl = IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 3].CellCtl = IMN_COLNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 3].CellCtl = IMT_SIZNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 3].CellCtl = IMT_MAKER_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 3].CellCtl = IMT_SKUCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 3].CellCtl = IMN_COST_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 3].CellCtl = IMN_RATE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 3].CellCtl = IMN_MDTAN_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 3].CellCtl = IMN_MDSGK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 3].CellCtl = IMN_ZAISU_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 3].CellCtl = IMN_SURYO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 3].CellCtl = IMN_PURKN_3;

            // 5行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 4].CellCtl = CHK_SELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 4].CellCtl = IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 4].CellCtl = IMN_COLNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 4].CellCtl = IMT_SIZNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 4].CellCtl = IMT_MAKER_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 4].CellCtl = IMT_SKUCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 4].CellCtl = IMN_COST_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 4].CellCtl = IMN_RATE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 4].CellCtl = IMN_MDTAN_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 4].CellCtl = IMN_MDSGK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 4].CellCtl = IMN_ZAISU_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 4].CellCtl = IMN_SURYO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 4].CellCtl = IMN_PURKN_4;

            // 6行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 5].CellCtl = CHK_SELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 5].CellCtl = IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 5].CellCtl = IMN_COLNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 5].CellCtl = IMT_SIZNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 5].CellCtl = IMT_MAKER_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 5].CellCtl = IMT_SKUCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 5].CellCtl = IMN_COST_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 5].CellCtl = IMN_RATE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 5].CellCtl = IMN_MDTAN_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 5].CellCtl = IMN_MDSGK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 5].CellCtl = IMN_ZAISU_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 5].CellCtl = IMN_SURYO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 5].CellCtl = IMN_PURKN_5;

            // 7行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 6].CellCtl = CHK_SELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 6].CellCtl = IMN_COLNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 6].CellCtl = IMT_SIZNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 6].CellCtl = IMT_MAKER_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 6].CellCtl = IMT_SKUCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 6].CellCtl = IMN_COST_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 6].CellCtl = IMN_RATE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 6].CellCtl = IMN_MDTAN_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 6].CellCtl = IMN_MDSGK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 6].CellCtl = IMN_ZAISU_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 6].CellCtl = IMN_SURYO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 6].CellCtl = IMN_PURKN_6;

            // 8行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 7].CellCtl = CHK_SELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 7].CellCtl = IMN_COLNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 7].CellCtl = IMT_SIZNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 7].CellCtl = IMT_MAKER_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 7].CellCtl = IMT_SKUCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 7].CellCtl = IMN_COST_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 7].CellCtl = IMN_RATE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 7].CellCtl = IMN_MDTAN_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 7].CellCtl = IMN_MDSGK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 7].CellCtl = IMN_ZAISU_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 7].CellCtl = IMN_SURYO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 7].CellCtl = IMN_PURKN_7;

            // 9行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 8].CellCtl = CHK_SELCK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 8].CellCtl = IMN_COLNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 8].CellCtl = IMT_SIZNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 8].CellCtl = IMT_MAKER_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 8].CellCtl = IMT_SKUCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 8].CellCtl = IMN_COST_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 8].CellCtl = IMN_RATE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 8].CellCtl = IMN_MDTAN_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 8].CellCtl = IMN_MDSGK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 8].CellCtl = IMN_ZAISU_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 8].CellCtl = IMN_SURYO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 8].CellCtl = IMN_PURKN_8;

            // 10行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 9].CellCtl = CHK_SELCK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 9].CellCtl = IMN_COLNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 9].CellCtl = IMT_SIZNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 9].CellCtl = IMT_MAKER_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 9].CellCtl = IMT_SKUCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 9].CellCtl = IMN_COST_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 9].CellCtl = IMN_RATE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 9].CellCtl = IMN_MDTAN_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 9].CellCtl = IMN_MDSGK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 9].CellCtl = IMN_ZAISU_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 9].CellCtl = IMN_SURYO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 9].CellCtl = IMN_PURKN_9;

            // 11行目
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.GYONO, 10].CellCtl = IMT_GYONO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Chk, 10].CellCtl = CHK_SELCK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.JanCD, 10].CellCtl = IMT_JANCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ITEMCD, 10].CellCtl = IMT_ITMCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUName, 10].CellCtl = IMT_ITMNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.ColorName, 10].CellCtl = IMN_COLNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SizeName, 10].CellCtl = IMT_SIZNM_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MakerItem, 10].CellCtl = IMT_MAKER_10;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.SKUCD, 10].CellCtl = IMT_SKUCD_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.PriceOutTax, 10].CellCtl = IMN_TEIKA_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.EvaluationPrice, 10].CellCtl = IMN_COST_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.Rate, 10].CellCtl = IMN_RATE_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownUnitPrice, 10].CellCtl = IMN_MDTAN_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice, 10].CellCtl = IMN_MDSGK_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.StockSu, 10].CellCtl = IMN_ZAISU_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.CalculationSu, 10].CellCtl = IMN_SURYO_10;
            mGrid.g_MK_Ctrl[(int)ClsGridMarkDown.ColNO.MarkDownGaku, 10].CellCtl = IMN_PURKN_10;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvNxt, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvPrv, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvNxt, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                        ChangeBackColor(w_Row);
                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    // クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    switch (w_Col)
                    {
                        case (int)ClsGridMarkDown.ColNO.Chk:
                            {
                                // チェック
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
        // 引数    pKBN    0...  新規
        //                 1...  修正時(画面展開後)
        //                 2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ
        // pGrid 0...  明細の各プロパティ以外    
        //       1...  明細部の各プロパティ(先に設定しておいてから、実際にpGrid=0で PanelのEnable制御等を行う)
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
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            Scr_Lock(0, 0, 0);
                            Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                            rdoPlan.Checked = true;
                            RadioButton_CheckedChanged(rdoPlan, EventArgs.Empty);
                            detailControls[(int)EIndex.CostingDate].Text = bbl.GetDate();
                            ScStaff.TxtCode.Text = InOperatorCD;
                            CheckDetail((int)EIndex.StaffCD);

                            detailControls[(int)EIndex.PurchaseDate].Enabled = false; 
                            btnInput.Enabled = true;
                            btnSearch.Enabled = true;
                            btnSelectAll.Enabled = false;
                            btnNoSelect.Enabled = false;
                            btnReflect.Enabled = false;

                            SetFuncKeyAll(this, "100001001011");
                            
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

                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JanCD))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridMarkDown.ColNO.Chk:
                                        case (int)ClsGridMarkDown.ColNO.Rate:
                                        case (int)ClsGridMarkDown.ColNO.MarkDownUnitPrice:
                                        case (int)ClsGridMarkDown.ColNO.CalculationSu:
                                        case (int)ClsGridMarkDown.ColNO.MarkDownGaku:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
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
                            Scr_Lock(0, 1, 1);
                            RadioButton_CheckedChanged(rdoPlan, EventArgs.Empty);
                            CboSouko.Enabled = false;
                            SetFuncKeyAll(this, "101100001011");
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

                            int w_Col;
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;

                                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;

                                }
                            }
                            
                            break;
                        }
                        else
                        {
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                Scr_Lock(1, 3, 1);
                                IMT_DMY_0.Focus();
                                SetFuncKeyAll(this, "101100000001");                                
                            }
                            else
                            {
                                Scr_Lock(1, 3, 1);
                                IMT_DMY_0.Focus();
                                SetFuncKeyAll(this, "100000000010");
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
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            // 行削除ﾎﾞﾀﾝ使用可否
            if (mCloseState == EClose.Closed)
            {
                SetFuncKey(this, 7, false);
            }
            else
            {
                SetFuncKey(this, 7, true);
            }

            // 検索ﾎﾞﾀﾝ使用使用可否
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

        private void Grid_NotFocus(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
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


            if (pCol == (int)ClsGridMarkDown.ColNO.Chk)
            {
                
                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                {
                    //JanCD空白時、入力不可
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JanCD))
                    {
                        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                            
                        }
                    }
                    else
                    {
                        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                        {

                            switch (w_Col)
                            {
                                case (int)ClsGridMarkDown.ColNO.Chk:
                                case (int)ClsGridMarkDown.ColNO.Rate:
                                case (int)ClsGridMarkDown.ColNO.MarkDownUnitPrice:
                                case (int)ClsGridMarkDown.ColNO.CalculationSu:
                                case (int)ClsGridMarkDown.ColNO.MarkDownGaku:
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    break;

                                default:
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                    break;

                                    break;

                            }

                        }
                    }

                    //if (!mGrid.g_DArray[pRow].Chk)
                    //{
                    //    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    //    {
                    //        switch (w_Col)
                    //        {
                    //            case (int)ClsGridMarkDown.ColNO.Rate:
                    //            case (int)ClsGridMarkDown.ColNO.MarkDownUnitPrice:
                    //            case (int)ClsGridMarkDown.ColNO.CalculationSu:
                    //            case (int)ClsGridMarkDown.ColNO.MarkDownGaku:
                    //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                    //                break;

                    //            default:
                    //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                    //                break;

                    //        }
                    //    }

                    //    w_AllFlg = false;
                    //}
                    //else
                    //{
                    //    //Chk入力時
                    //    w_AllFlg = true;
                    //    if (mGrid.g_DArray[pRow].Chk)
                    //    {
                    //        switch (w_Col)
                    //        {
                    //            case (int)ClsGridMarkDown.ColNO.Chk:
                    //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                    //                break;

                    //            default:
                    //                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                    //                break;
                    //        }
                    //    }
                    //}
                }

                bool isEnabled = false;
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[0].JanCD))
                {
                    isEnabled = true;                    
                }

                btnSelectAll.Enabled = isEnabled;
                btnNoSelect.Enabled = isEnabled;
                btnReflect.Enabled = isEnabled;

                return;
            }
        }

        // -----------------------------------------------
        // <明細部>行削除処理 Ｆ７
        // -----------------------------------------------
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

            //ただし、その明細が入荷予定日ありの明細である場合、削除できない（F7ボタンを使えないようにする）
            //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ArrivePlanDate))
            //    return;

            int col = (int)ClsGridMarkDown.ColNO.Chk;

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
                
                Grid_NotFocus(col, i);
            }

            this.CalcKin();

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            IMT_DMY_0.Focus();

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            // 明細がすべて削除されたら、行削除ボタンをOFFする
            SetFuncKey(this, 7, btnSelectAll.Enabled);
        }

        private void Grid_Gyo_Clr(int RW)  // 明細部１行クリア
        {
            string w_Gyo;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            w_Gyo = mGrid.g_DArray[RW].GYONO;

            // 一行クリア
            Array.Clear(mGrid.g_DArray, RW, 1);

            mGrid.g_DArray[RW].GYONO = w_Gyo;


            // JanCD列以外入力不可 (JanCDを入力した時点で他の列が入力可になるため)
            Grid_NotFocus((int)ClsGridMarkDown.ColNO.JanCD, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }

        #endregion

        public MarkDownNyuuryoku()
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

                //起動時共通処理
                base.StartProgram();

                Btn_F2.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "出力(F11)";
                F9Visible = false;

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                mdbl = new MarkDownNyuuryoku_BL();
                CboSouko.Bind(ymd, StoreAuthorizationsCD);
                CboStockInfo.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScVendorCD.Value1 = "1";

                ScStaff.TxtCode.Text = InOperatorCD;

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
                    ScStaff.LabelText = mse.StaffName;
                }

                //マークダウンNO＝NULL時、新規モードで起動
                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 == (int)ECmdLine.PcID || string.IsNullOrWhiteSpace(cmds[(int)ECmdLine.PcID + 1]))
                {
                    Btn_F3.Text = "";
                    Btn_F4.Text = "";
                    ChangeOperationMode(EOperationMode.INSERT);
                }
                else
                {
                    Btn_F6.Text = "";
                    string markDownNO = cmds[(int)ECmdLine.PcID + 1];
                    keyLabels[(int)EIndex.MarkDownNO].Text = markDownNO;
                    this.CheckData();            
                }

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
            keyControls = new Control[] { };
            keyLabels = new Control[] { lblMDNo };
            detailControls = new Control[] { ScVendorCD.TxtCode, CboSouko, CboStockInfo
                                ,txtCostingDate, txtUnitPriceDate, txtExpectedPurchaseDate, txtPurchaseDate
                                , ScStaff.TxtCode ,txtRemark, txtRate, txtUnitPrice , txtSu };
            detailLabels = new Control[] { ScVendorCD, ScStaff };
            searchButtons = new Control[] { ScVendorCD.BtnSearch, ScStaff.BtnSearch };
            radioButtons = new Control[] { rdoPlan, rdoResult };

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
            foreach (CKM_Controls.CKM_RadioButton ctl in radioButtons)
            {
                ctl.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            }

        }

        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set = true)
        {
            //switch (index)
            //{
            //}
            return true;

        }

        /// <summary>
        /// マークダウンデータ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData()
        {
            //[D_Purchase_SelectDataF]
            dme = new D_MarkDown_Entity
            {
                MarkDownNO = keyLabels[(int)EIndex.MarkDownNO].Text
            };

            DataTable dt = mdbl.D_MarkDown_SelectData(dme);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                //　締処理チェック
                bool ret1 = false;
                bool ret2 = false;
                bool ret3 = false;
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["CostingDate"].ToString()))
                {
                    //予定月
                    //月次締チェック
                    M_StoreClose_Entity mste = new M_StoreClose_Entity
                    {
                        StoreCD = dt.Rows[0]["StoreCD"].ToString(),
                        FiscalYYYYMM = dt.Rows[0]["CostingDate"].ToString().Replace("/", "").Substring(0, 6)
                    };
                    ret1 = mdbl.CheckStoreCloseForMarkDown(mste, true, false);
                }

                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["PurchaseDate"].ToString()))
                {
                    //仕入月
                    //月次締チェック
                    M_StoreClose_Entity mste = new M_StoreClose_Entity
                    {
                        StoreCD = dt.Rows[0]["StoreCD"].ToString(),
                        FiscalYYYYMM = dt.Rows[0]["PurchaseDate"].ToString().Replace("/", "").Substring(0, 6)
                    };
                    ret2 = mdbl.CheckStoreCloseForMarkDown(mste, true, true);

                    //支払締チェック
                    D_PayCloseHistory_Entity dpe = new D_PayCloseHistory_Entity
                    {
                        PayeeCD = dt.Rows[0]["PayeeCD"].ToString(),
                        PayCloseDate = dt.Rows[0]["PurchaseDate"].ToString()
                    };
                    ret3 = mdbl.CheckPayCloseHistory(dpe);
                }

                //処理モードセット
                EOperationMode mode;
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["PurchaseDate"].ToString()))
                {
                    //仕入月で仮締め以降がされている
                    if (!ret2 || ret3)
                    {
                        mode = EOperationMode.SHOW;
                        Btn_F3.Text = "";
                        Btn_F4.Text = "";
                        Btn_F8.Text = "";
                    }
                    //仕入月で仮締めがされていない
                    else
                    {
                        mode = EOperationMode.UPDATE;
                        if (!ret1)
                        {
                            Btn_F3.Text = "";
                            Btn_F4.Text = "";
                            Btn_F8.Text = "";
                            mCloseState = EClose.Closed;
                        }
                        else
                        {
                            mCloseState = EClose.NotClose;
                        }
                    }

                    rdoResult.Checked = true;
                }
                else
                {
                    mode = EOperationMode.UPDATE;                    
                    if (!ret1)
                    {                        
                        Btn_F3.Text = "";
                        Btn_F4.Text = "";
                        Btn_F8.Text = "";
                        mCloseState = EClose.Closed;                        
                    }
                    else
                    {
                        mCloseState = EClose.NotClose;
                    }

                    rdoPlan.Checked = true;
                }

                // 明細表示                
                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;

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
                        // 担当スタッフ
                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDependsOnDate((int)EIndex.StaffCD, false, true);
                        // 仕入日
                        if (!string.IsNullOrWhiteSpace(row["PurchaseDate"].ToString()))
                        {
                            detailControls[(int)EIndex.PurchaseDate].Text = row["PurchaseDate"].ToString();
                            mOldPurchaseDate = detailControls[(int)EIndex.PurchaseDate].Text;
                        }                        
                        // 仕入先
                        detailControls[(int)EIndex.VendorCD].Text = row["VendorCD"].ToString();
                        CheckDependsOnDate((int)EIndex.VendorCD, false, true);
                        // 倉庫
                        CboSouko.SelectedValue = row["SoukoCD"].ToString();
                        mStoreCD = row["StoreCD"].ToString();
                        // 在庫情報
                        CboStockInfo.SelectedValue = row["ReplicaNO"].ToString();
                        // MD計上日
                        detailControls[(int)EIndex.CostingDate].Text = row["CostingDate"].ToString();
                        // 単価改定日
                        detailControls[(int)EIndex.UnitPriceDate].Text = row["UnitPriceDate"].ToString();
                        // 仕入予定日
                        detailControls[(int)EIndex.ExpectedPurchaseDate].Text = row["ExpectedPurchaseDate"].ToString();
                        // 備考
                        detailControls[(int)EIndex.Remark].Text = row["Comment"].ToString();

                        // マークダウン仕入番号
                        mMDPurchaseNO = row["MDPurchaseNO"].ToString();
                        // 仕入番号
                        mPurchaseNO = row["PurchaseNO"].ToString();

                    }

                    mGrid.g_DArray[i].Chk = false;
                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    CheckGrid((int)ClsGridMarkDown.ColNO.JanCD, i);
                    mGrid.g_DArray[i].EvaluationPrice = bbl.Z_SetStr(row["EvaluationPrice"]);
                    mGrid.g_DArray[i].Rate = string.Format("{0:#,##0.00}", bbl.Z_Set(row["Rate"]));
                    mGrid.g_DArray[i].StockSu = bbl.Z_SetStr(row["StockSu"]);
                    if (!string.IsNullOrWhiteSpace(row["PurchaseDate"].ToString()))
                    {
                        mGrid.g_DArray[i].MarkDownUnitPrice = bbl.Z_SetStr(row["PurchaserUnitPrice"]);
                        mGrid.g_DArray[i].MarkDownSagakuPrice = bbl.Z_SetStr(row["PurchaserSagakuPrice"]);
                        mGrid.g_DArray[i].CalculationSu = bbl.Z_SetStr(row["PurchaseSu"]);
                        mGrid.g_DArray[i].MarkDownGaku = bbl.Z_SetStr(row["PurchaseGaku"]);
                        mGrid.g_DArray[i].Tax = bbl.Z_Set(row["PurchaseTax"]);
                    }
                    else
                    {
                        mGrid.g_DArray[i].MarkDownUnitPrice = bbl.Z_SetStr(row["MarkDownUnitPrice"]);
                        mGrid.g_DArray[i].MarkDownSagakuPrice = bbl.Z_SetStr(row["MarkDownSagakuPrice"]);
                        mGrid.g_DArray[i].CalculationSu = bbl.Z_SetStr(row["CalculationSu"]);
                        mGrid.g_DArray[i].MarkDownGaku = bbl.Z_SetStr(row["MarkDownGaku"]);
                        mGrid.g_DArray[i].Tax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[i].MarkDownGaku) * bbl.Z_Set(mGrid.g_DArray[i].TaxRate) / 100);
                    }

                    mGrid.g_DArray[i].MDPurchaseRows = (int)bbl.Z_Set(row["MDPurchaseRows"]);
                    mGrid.g_DArray[i].PurchaseRows = (int)bbl.Z_Set(row["PurchaseRows"]);
                    mGrid.g_DArray[i].InsertOperator = row["InsertOperator"].ToString();
                    mGrid.g_DArray[i].InsertDateTime = row["InsertDateTime"].ToString();
                    mGrid.g_DArray[i].OldEvaluationPrice = mGrid.g_DArray[i].EvaluationPrice;
                    mGrid.g_DArray[i].OldRate = mGrid.g_DArray[i].Rate;
                    mGrid.g_DArray[i].OldMarkDownUnitPrice = mGrid.g_DArray[i].MarkDownUnitPrice;
                    mGrid.g_DArray[i].OldCalculationSu = mGrid.g_DArray[i].CalculationSu;

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridMarkDown.ColNO.Chk, i);
                    i++;                    
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

                this.CalcKin();

                ChangeOperationMode(mode);

                return true;
            }
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
            if (detailControls[index].Enabled == false)
            {
                return true;
            }

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.VendorCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        ScVendorCD.LabelText = "";
                        return false;
                    }
                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.StaffCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        ScStaff.LabelText = "";
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    M_Souko_Entity mse = new M_Souko_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        SoukoCD = CboSouko.SelectedValue.ToString()

                    };
                    DataTable dt = mdbl.M_Souko_SelectData(mse);

                    if (dt.Rows.Count > 0)
                    {
                        mStoreCD = dt.Rows[0]["StoreCD"].ToString();
                    }
                    break;
                case (int)EIndex.StockInfo:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.CostingDate:
                    //選択必須(Entry required)
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
                    M_StoreClose_Entity mste = new M_StoreClose_Entity
                    {
                        StoreCD = mStoreCD,
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    bool ret = bbl.CheckStoreClose(mste, false, true, false, false, false);
                    if (!ret)
                    {
                        return false;
                    }

                    if (detailControls[(int)EIndex.UnitPriceDate].Enabled && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.UnitPriceDate].Text))
                    {
                        detailControls[(int)EIndex.UnitPriceDate].Text = detailControls[index].Text;
                    }
                    break;

                case (int)EIndex.UnitPriceDate:
                case (int)EIndex.ExpectedPurchaseDate:
                    //選択必須(Entry required)
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
                    //MD計上日以降であること
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CostingDate].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[(int)EIndex.CostingDate].Text);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E104");
                            detailControls[index].Focus();
                            return false;
                        }
                    }

                    break;

                case (int)EIndex.PurchaseDate:
                    if (detailControls[index].Enabled == false)
                    {
                        return true;
                    }

                    //選択必須(Entry required)
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

                    //締処理済の場合（以下のSelectができる場合）Error
                    //【D_PayCloseHistory】
                    D_PayCloseHistory_Entity dpe = new D_PayCloseHistory_Entity
                    {
                        PayeeCD = mPayeeCD,
                        PayCloseDate = detailControls[index].Text
                    };
                    ret = mdbl.CheckPayCloseHistory(dpe);
                    if (ret)
                    {
                        //Ｅ１７７
                        bbl.ShowMessage("E177");
                        return false;
                    }

                    //店舗の締日チェック
                    //店舗締マスターで判断
                    mste = new M_StoreClose_Entity
                    {
                        StoreCD = mStoreCD,
                        FiscalYYYYMM = detailControls[index].Text.Replace("/", "").Substring(0, 6)
                    };
                    ret = bbl.CheckStoreClose(mste,false,true,false,false,false);
                    if (!ret)
                    {
                        return false;
                    }

                    //仕入日が変更された場合のチェック処理
                    if (mOldPurchaseDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.VendorCD; i < (int)EIndex.StaffCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnDate(i, true))
                                    return false;

                        mOldPurchaseDate = detailControls[index].Text;
                        ScVendorCD.ChangeDate = mOldPurchaseDate;
                    }

                    break;

                case (int)EIndex.Remark:
                    string str = Encoding.GetEncoding(932).GetByteCount(detailControls[index].Text).ToString();
                    if (Convert.ToInt32(str) > txtRemark.MaxLength)
                    {
                        MessageBox.Show("入力された文字が長すぎます", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            w_Ctrl = detailControls[(int)EIndex.Remark];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, (int)ClsGridMarkDown.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false, bool dispKbn = false)
        {
            string ymd = detailControls[(int)EIndex.PurchaseDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
                case (int)EIndex.VendorCD:
                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = ymd
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    bool ret = sbl.M_Vendor_SelectTop1(mve);
                    if (ret)
                    {
                        if (mve.DeleteFlg == "1")
                        {                           
                            ScVendorCD.LabelText = "";
                            mPayeeCD = "";
                            if (!dispKbn)
                            { 
                                bbl.ShowMessage("E119");
                                ScVendorCD.TxtCode.SelectAll();
                            }                            
                            return false;
                        }
                        ScVendorCD.LabelText = mve.VendorName;
                        mPayeeCD = mve.PayeeCD;
                    }
                    else
                    {
                        ScVendorCD.LabelText = "";
                        mPayeeCD = "";
                        if (!dispKbn)
                        {
                            bbl.ShowMessage("E101");
                            ScVendorCD.TxtCode.SelectAll();
                        }
                        return false;
                    }
                    break;

                case (int)EIndex.StaffCD:
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
                        ScStaff.LabelText = "";
                        if (!dispKbn)
                        {
                            bbl.ShowMessage("E101");
                            ScStaff.TxtCode.SelectAll();
                        }
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// 明細部チェック
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="chkAll"></param>
        /// <param name="changeYmd"></param>
        /// <returns></returns>
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {
            string ymd = detailControls[(int)EIndex.CostingDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridMarkDown.gc_P_GYO)
                {
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }
                }                    
            }

            switch (col)
            {
                case (int)ClsGridMarkDown.ColNO.JanCD:
                    if (!chkAll)
                    {
                        //[M_SKU]
                        M_SKU_Entity mse = new M_SKU_Entity
                        {
                            JanCD = mGrid.g_DArray[row].JanCD,
                            AdminNO = bbl.Z_Set(mGrid.g_DArray[row].AdminNO).ToString(),
                            ChangeDate = ymd
                        };

                        mdbl = new MarkDownNyuuryoku_BL();
                        DataTable dt = mdbl.M_SKU_SelectForMarkDown(mse);
                        if (dt.Rows.Count > 0)
                        {
                            mGrid.g_DArray[row].ITEMCD = dt.Rows[0]["ITemCD"].ToString();
                            mGrid.g_DArray[row].SKUName = dt.Rows[0]["SKUName"].ToString();
                            mGrid.g_DArray[row].ColorName = dt.Rows[0]["ColorName"].ToString();
                            mGrid.g_DArray[row].SizeName = dt.Rows[0]["SizeName"].ToString();
                            mGrid.g_DArray[row].MakerItem = dt.Rows[0]["MakerItem"].ToString();
                            mGrid.g_DArray[row].SKUCD = dt.Rows[0]["SKUCD"].ToString();
                            mGrid.g_DArray[row].PriceOutTax = bbl.Z_SetStr(dt.Rows[0]["PriceOutTax"]);
                            mGrid.g_DArray[row].AdminNO = bbl.Z_Set(dt.Rows[0]["AdminNO"]).ToString();
                            mGrid.g_DArray[row].TaniCD = dt.Rows[0]["TaniCD"].ToString();
                            mGrid.g_DArray[row].TaniName = dt.Rows[0]["TaniName"].ToString();
                            mGrid.g_DArray[row].TaxRate = bbl.Z_Set(dt.Rows[0]["TaxRate"]);
                            mGrid.g_DArray[row].NormalCost = bbl.Z_Set(dt.Rows[0]["NormalCost"]);
                            mGrid.g_DArray[row].EvaluationPrice = bbl.Z_SetStr(dt.Rows[0]["NormalCost"]);
                            mGrid.g_DArray[row].OldEvaluationPrice = bbl.Z_SetStr(dt.Rows[0]["NormalCost"]);


                            m_dataCnt = row + 1;
                            Grid_NotFocus((int)ClsGridMarkDown.ColNO.Chk, row);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    
                    break;

                case (int)ClsGridMarkDown.ColNO.EvaluationPrice:
                case (int)ClsGridMarkDown.ColNO.Rate:
                case (int)ClsGridMarkDown.ColNO.MarkDownUnitPrice:
                case (int)ClsGridMarkDown.ColNO.CalculationSu:

                    // MD変更単価
                    if (mGrid.g_DArray[row].OldRate != mGrid.g_DArray[row].Rate)
                    {
                        mGrid.g_DArray[row].MarkDownUnitPrice = bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[row].PriceOutTax) * bbl.Z_Set(mGrid.g_DArray[row].Rate) / 100));
                    }

                    // MD差額
                    if (mGrid.g_DArray[row].OldRate != mGrid.g_DArray[row].Rate 
                        || mGrid.g_DArray[row].OldEvaluationPrice != mGrid.g_DArray[row].EvaluationPrice
                        || mGrid.g_DArray[row].OldMarkDownUnitPrice != mGrid.g_DArray[row].MarkDownUnitPrice)
                    {                        
                        mGrid.g_DArray[row].MarkDownSagakuPrice = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].MarkDownUnitPrice) - bbl.Z_Set(mGrid.g_DArray[row].EvaluationPrice));
                    }

                    // 仕入予定額・税額
                    if (mGrid.g_DArray[row].OldRate != mGrid.g_DArray[row].Rate
                        || mGrid.g_DArray[row].OldEvaluationPrice != mGrid.g_DArray[row].EvaluationPrice
                        || mGrid.g_DArray[row].OldMarkDownUnitPrice != mGrid.g_DArray[row].MarkDownUnitPrice
                        || mGrid.g_DArray[row].OldCalculationSu != mGrid.g_DArray[row].CalculationSu)
                    {
                        mGrid.g_DArray[row].MarkDownGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].MarkDownSagakuPrice) * bbl.Z_Set(mGrid.g_DArray[row].CalculationSu));
                        mGrid.g_DArray[row].Tax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[row].MarkDownGaku) * bbl.Z_Set(mGrid.g_DArray[row].TaxRate) / 100);
                    }

                    if (chkAll == false)
                        CalcKin();

                    // 入力内容変更
                    mGrid.g_DArray[row].OldRate = mGrid.g_DArray[row].Rate;
                    mGrid.g_DArray[row].OldMarkDownUnitPrice = mGrid.g_DArray[row].MarkDownUnitPrice;
                    mGrid.g_DArray[row].OldCalculationSu = mGrid.g_DArray[row].CalculationSu;
                    mGrid.g_DArray[row].OldMarkDownGaku = mGrid.g_DArray[row].MarkDownGaku;

                    break;

                case (int)ClsGridMarkDown.ColNO.MarkDownGaku:
                    if (mGrid.g_DArray[row].OldMarkDownGaku != mGrid.g_DArray[row].MarkDownGaku)
                    {
                        mGrid.g_DArray[row].Tax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[row].MarkDownGaku) * bbl.Z_Set(mGrid.g_DArray[row].TaxRate) / 100);
                    }

                    if (chkAll == false)
                        CalcKin();

                    // 入力内容変更
                    mGrid.g_DArray[row].OldMarkDownGaku = mGrid.g_DArray[row].MarkDownGaku;

                    break;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        /// <summary>
        /// 明細原価・在庫数再取得（倉庫・在庫情報変更時）
        /// </summary>
        private void ReCalcDetail()
        {
            if (CboSouko.SelectedIndex <= 0 || CboStockInfo.SelectedIndex <= 0)
                return;
            
            //[D_StockReplica]
            D_StockReplica_Entity dse = new D_StockReplica_Entity
            {
                ReplicaNO = CboStockInfo.SelectedValue.ToString(),                
                SoukoCD = CboSouko.SelectedValue.ToString()
            };

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                {
                    dse.AdminNO = mGrid.g_DArray[RW].AdminNO;

                    MarkDownNyuuryoku_BL mbl = new MarkDownNyuuryoku_BL();
                    DataTable sdt = mbl.D_StockReplica_SelectForMarkDown(dse);
                    if (sdt.Rows.Count > 0)
                    {
                        mGrid.g_DArray[RW].EvaluationPrice = bbl.Z_SetStr(sdt.Rows[0]["LastCost"]);
                        mGrid.g_DArray[RW].StockSu = bbl.Z_SetStr(sdt.Rows[0]["AllowableSu"]);
                    }
                    else
                    {
                        mGrid.g_DArray[RW].EvaluationPrice = bbl.Z_SetStr(mGrid.g_DArray[RW].NormalCost);
                        mGrid.g_DArray[RW].StockSu = "0";
                    }
                    this.CheckGrid((int)ClsGridMarkDown.ColNO.EvaluationPrice, RW);
                    mGrid.g_DArray[RW].OldEvaluationPrice = mGrid.g_DArray[RW].EvaluationPrice;
                }
            }           

        }

        /// <summary>
        /// 金額計算処理
        /// </summary>
        private void CalcKin()
        {

            decimal kin = 0;
            decimal zei = 0;
            mHontaiGaku8 = 0;
            mHontaiGaku10 = 0;
            mTaxGaku8 = 0;
            mTaxGaku10 = 0;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                {
                    kin += bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku);
                    zei += bbl.Z_Set(mGrid.g_DArray[RW].Tax);

                    if (bbl.Z_Set(mGrid.g_DArray[RW].TaxRate) == 10)
                    {
                        mHontaiGaku10 += bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku); 
                        mTaxGaku10 += bbl.Z_Set(mGrid.g_DArray[RW].Tax);
                    }
                    else
                    {
                        mHontaiGaku8 += bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku);
                        mTaxGaku8 += bbl.Z_Set(mGrid.g_DArray[RW].Tax);
                    }
                }              
            }

            //Footer部
            lblPurchaseGaku.Text = string.Format("{0:#,##0}", kin);
            mTaxCalcGaku = zei;
            if (rdoResult.Checked)
            {                
                txtPurchaseTax.Text = string.Format("{0:#,##0}", zei);
                lblTotalPurchaseGaku.Text = string.Format("{0:#,##0}", kin + zei);
            }           

        }

        /// <summary>
        /// 予定税額入力チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckTax()
        {
            // チェックされた明細がない場合、予定税額入力されたらエラーとする
            bool blnCheck = false;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) && mGrid.g_DArray[RW].Chk == true)
                {
                    blnCheck = true;
                    break;
                }
            }

            if (blnCheck == false && bbl.Z_Set(txtPurchaseTax.Text) != 0)
            {
                bbl.ShowMessage("E255");
                return false;
            }

            // 明細税額調整
            if (bbl.Z_Set(txtPurchaseTax.Text) != mTaxCalcGaku)
            {
                lblTotalPurchaseGaku.Text = bbl.Z_SetStr(bbl.Z_Set(lblPurchaseGaku.Text) + bbl.Z_Set(txtPurchaseTax.Text));
                this.AdjustTax();

                mTaxCalcGaku = bbl.Z_Set(txtPurchaseTax.Text);
            }
            
            return true;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        private void ExecInput()
        {
            string fileName = "";

            //　ファイルを選択
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "EXCELブック(*.xlsx;*.xls)|*.xlsx;*.xls|すべてのファイル(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = fileDialog.FileName;
            }
            else
            {
                return;
            }

            // 拡張子がExcelではない時
            if (Path.GetExtension(fileName).ToLower() != ".xlsx" && Path.GetExtension(fileName).ToLower() != ".xls")
            {
                bbl.ShowMessage("E137");
                btnInput.Focus();
                return;
            }

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            Microsoft.Office.Interop.Excel.Worksheet sheet = null;

            try
            {
                ExcelApp.Visible = false;

                // ファイルOPEN
                wb = ExcelApp.Workbooks.Open(fileName);

                sheet = wb.Sheets[1];
                sheet.Select();

                // 読み込む範囲を指定する
                Microsoft.Office.Interop.Excel.Range InputRange = sheet.Range[sheet.Cells[1, 1], sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell)];

                var yCount = sheet.UsedRange.Rows.Count;
                var xCount = sheet.UsedRange.Columns.Count;

                // 指定された範囲のセルの値をオブジェクト型の配列に読み込む
                object[,] InputData = (System.Object[,])InputRange.Value;

                // フォーマットチェック
                if (InputData[1, (int)EColNo.JanCD + 1].ToString() != "JANCD"
                    || InputData[1, (int)EColNo.Rate + 1].ToString() != "掛率"
                    || InputData[1, (int)EColNo.MarkDownUnitPrice + 1].ToString() != "MD単価"
                    || InputData[1, (int)EColNo.CalculationSu + 1].ToString() != "対象数")
                {
                    bbl.ShowMessage("E137");
                    btnInput.Focus();
                    return;
                }

                //  空行の検索
                int i = 0;
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                    {
                        i = RW;
                        break;
                    }
                }

                for (int row = 2; row <= InputData.GetLength(0); row++)
                {
                    // 各行の値をListに変換（すべてnullは読み飛ばし）
                    List<string> data = new List<string>();
                    bool isEmpty = true;
                    for (int col = 1; col <= (int)EColNo.COUNT; col++)
                    {
                        if (InputData[row, col] != null)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty)
                        continue;

                    for (int col = 1; col <= (int)EColNo.COUNT; col++)
                    {
                        if (InputData[row, col] != null)
                            data.Add(InputData[row, col].ToString());
                        else
                            data.Add("");
                    }

                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return;
                    }

                    //データチェック(1行目はヘッダ）
                    if (!Check_ExcelData(data, i))
                        continue;

                    mGrid.g_DArray[i].JanCD = data[(int)EColNo.JanCD].ToString();
                    this.DispNewDetail(i, data[(int)EColNo.Rate].ToString(), data[(int)EColNo.MarkDownUnitPrice].ToString(), data[(int)EColNo.CalculationSu].ToString());

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridMarkDown.ColNO.JanCD, i);
                    i += 1;

                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (wb != null)
                {
                    wb.Close(false);
                }
                Marshal.ReleaseComObject(sheet);
                Marshal.ReleaseComObject(wb);
                ExcelApp.Quit();
            }


        }

        /// <summary>
        /// 取込データチェック
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Check_ExcelData(List<string> data, int RW)
        {
            string ymd = detailControls[(int)EIndex.CostingDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            // JANCDチェック
            //[M_SKU]
            M_SKU_Entity mse = new M_SKU_Entity
            {
                JanCD = data[(int)EColNo.JanCD].ToString(),
                SetKBN = "0",
                ChangeDate = ymd
            };

            SKU_BL sbl = new SKU_BL();
            DataTable dt = sbl.M_SKU_SelectAll(mse);
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E101");
                return false;
            }
            
            // 掛率数値チェック
            decimal rate;
            if (!String.IsNullOrEmpty(data[(int)EColNo.Rate]))
            {
                if (!Decimal.TryParse(data[(int)EColNo.Rate], out rate))
                {
                    //Ｅ１９０
                    bbl.ShowMessage("E190");
                    return false;
                }
                else if(bbl.Z_Set(data[(int)EColNo.Rate]) < 0 || bbl.Z_Set(data[(int)EColNo.Rate]) > 100)
                {
                    //Ｅ２３１
                    bbl.ShowMessage("E231");
                    return false;
                }
            }

            // MD単価チェック
            decimal tanka;
            if (!String.IsNullOrEmpty(data[(int)EColNo.MarkDownUnitPrice]))
            {
                if (!Decimal.TryParse(data[(int)EColNo.MarkDownUnitPrice], out tanka))
                {
                    //Ｅ１９０
                    bbl.ShowMessage("E190");
                    return false;
                }
                else if (bbl.Z_Set(data[(int)EColNo.MarkDownUnitPrice]) < 0 || bbl.Z_Set(data[(int)EColNo.MarkDownUnitPrice]) > 999999999)
                {
                    //Ｅ２３１
                    bbl.ShowMessage("E231");
                    return false;
                }
            }

            // 対象数チェック
            decimal suryo;
            if (!String.IsNullOrEmpty(data[(int)EColNo.CalculationSu]))
            {
                if (!Decimal.TryParse(data[(int)EColNo.CalculationSu], out suryo))
                {
                    //Ｅ１９０
                    bbl.ShowMessage("E190");
                    return false;
                }
                else if (bbl.Z_Set(data[(int)EColNo.CalculationSu]) < -99999 || bbl.Z_Set(data[(int)EColNo.CalculationSu]) > 99999)
                {
                    //Ｅ２３１
                    bbl.ShowMessage("E231");
                    return false;
                }
            }

            //重複チェック
            if (!this.CheckDoubleForAdd(data[(int)EColNo.JanCD].ToString()))
            {
                bbl.ShowMessage("E105");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 追加データ重複チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckDoubleForAdd(string jancd)
        {
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (String.Compare(mGrid.g_DArray[RW].JanCD, jancd, true) == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// CSV出力処理
        /// </summary>
        private void OutputCsv()
        {
            try
            {
                string strFullPath;

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // ファイルの種類リストを設定
                    saveFileDialog.FileName = ProNm + " " + DateTime.Now.ToString("yyyyMMdd_HHmmss") + " " + InOperatorCD;
                    saveFileDialog.Filter = "CSVファイル (*.CSV)|*.CSV";
                    saveFileDialog.RestoreDirectory = true;

                    // ダイアログを表示
                    DialogResult dialogResult = saveFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        // キャンセルされたので終了
                        return;
                    }

                    // 選択されたファイル名 (ファイルパス) をテキストボックスに設定
                    strFullPath = saveFileDialog.FileName;
                }

                

                //CSV出力処理                        
                bool titleFlg = false;
                string field = string.Empty;

                //CSVファイルに書き込むときに使うEncoding
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");                        
                using (StreamWriter sw = new StreamWriter(strFullPath, false, enc))
                {
                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    {
                        if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                        {
                            //ヘッダを書き込む
                            if (titleFlg == false)
                            {
                                field = "JANCD,ITEM,商品名,カラー,サイズ,メーカー商品CD,SKUCD" +
                                        ",定価(税抜),最新原価＠,掛率,ＭＤ変更＠,ＭＤ差額,在庫数,対象数,仕入予定額";

                                sw.Write(field);
                                sw.Write("\r\n");

                                titleFlg = true;
                            }

                            //レコードを書き込む
                            field = EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].JanCD.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].ITEMCD.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].SKUName.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].ColorName.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].SizeName.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].MakerItem.ToString()) + ',' +
                                    EncloseDoubleQuotesIfNeed(mGrid.g_DArray[RW].SKUCD.ToString()) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].PriceOutTax) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].EvaluationPrice) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].Rate) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].MarkDownUnitPrice) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].MarkDownSagakuPrice) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].StockSu) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].CalculationSu) + ',' +
                                    bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku);

                            sw.Write(field);
                            sw.Write("\r\n");
                           
                        }
                    }
                }

                bbl.ShowMessage("I201");

                return;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

        /// <summary>
        /// 反映処理
        /// </summary>
        private void ExecReflect()
        {
            // 3つの値がすべて空白時は、処理しない
            if (string.IsNullOrWhiteSpace(txtRate.Text) && string.IsNullOrWhiteSpace(txtUnitPrice.Text) && string.IsNullOrWhiteSpace(txtSu.Text))
            {
                return;
            }

            // チェックされていなければ、エラー
            bool blnCheck = false;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) && mGrid.g_DArray[RW].Chk == true)
                {
                    blnCheck = true;
                    break;
                }
            }

            if (!blnCheck)
            {
                bbl.ShowMessage("E216");
                return;
            }

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) && mGrid.g_DArray[RW].Chk == true)
                {                   
                    // 掛率
                    if (!string.IsNullOrWhiteSpace(txtRate.Text))
                    {
                        mGrid.g_DArray[RW].Rate = string.Format("{0:#,##0.00}", bbl.Z_Set(txtRate.Text)); ;
                        mGrid.g_DArray[RW].OldRate = mGrid.g_DArray[RW].Rate;

                        if (string.IsNullOrWhiteSpace(txtUnitPrice.Text))
                        {
                            mGrid.g_DArray[RW].MarkDownUnitPrice = bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[RW].PriceOutTax) * bbl.Z_Set(mGrid.g_DArray[RW].Rate) / 100));
                        }
                    }

                    //　MD変更＠
                    if (!string.IsNullOrWhiteSpace(txtUnitPrice.Text))
                    {
                        mGrid.g_DArray[RW].MarkDownUnitPrice = bbl.Z_SetStr(txtUnitPrice.Text);
                        mGrid.g_DArray[RW].OldMarkDownUnitPrice = mGrid.g_DArray[RW].MarkDownUnitPrice;
                    }

                    //　対象数
                    if (!string.IsNullOrWhiteSpace(txtSu.Text))
                    {
                        mGrid.g_DArray[RW].CalculationSu = bbl.Z_SetStr(txtSu.Text);
                        mGrid.g_DArray[RW].OldCalculationSu = mGrid.g_DArray[RW].CalculationSu;
                    }

                    //MD変更＠が変更された場合のみ、MD差額を計算
                    if ((!string.IsNullOrWhiteSpace(txtRate.Text) && string.IsNullOrWhiteSpace(txtUnitPrice.Text))
                        || !string.IsNullOrWhiteSpace(txtUnitPrice.Text))
                    {
                        mGrid.g_DArray[RW].MarkDownSagakuPrice = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[RW].MarkDownUnitPrice) - bbl.Z_Set(mGrid.g_DArray[RW].EvaluationPrice));
                    }

                    mGrid.g_DArray[RW].MarkDownGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[RW].MarkDownSagakuPrice) * bbl.Z_Set(mGrid.g_DArray[RW].CalculationSu));
                    mGrid.g_DArray[RW].OldMarkDownGaku = mGrid.g_DArray[RW].MarkDownGaku;

                    mGrid.g_DArray[RW].Tax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku) * bbl.Z_Set(mGrid.g_DArray[RW].TaxRate) / 100);
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

                CalcKin();

            }


        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_MarkDown_Entity GetEntity()
        {
            dme = new D_MarkDown_Entity
            {
                ChkResult = rdoResult.Checked ? "1" : "0",
                ChangeDate = rdoResult.Checked ? detailControls[(int)EIndex.PurchaseDate].Text : detailControls[(int)EIndex.CostingDate].Text,
                MarkDownNO = keyLabels[(int)EIndex.MarkDownNO].Text,
                StoreCD = mStoreCD,
                SoukoCD = CboSouko.SelectedValue.ToString(),
                ReplicaNO = CboStockInfo.SelectedValue.ToString(),
                ReplicaDate = CboStockInfo.Text.Substring(0, 10),
                ReplicaTime = CboStockInfo.Text.Substring(11, 8),
                StaffCD = detailControls[(int)EIndex.StaffCD].Text,
                VendorCD = detailControls[(int)EIndex.VendorCD].Text,
                CostingDate = detailControls[(int)EIndex.CostingDate].Text,
                UnitPriceDate = detailControls[(int)EIndex.UnitPriceDate].Text,
                ExpectedPurchaseDate = detailControls[(int)EIndex.ExpectedPurchaseDate].Text,
                PurchaseDate = detailControls[(int)EIndex.PurchaseDate].Text,
                Comment = detailControls[(int)EIndex.Remark].Text,
                MDPurchaseNO = mMDPurchaseNO,
                PurchaseNO = mPurchaseNO,
                PayeeCD = mPayeeCD,
                PurchaseGaku = bbl.Z_Set(lblPurchaseGaku.Text).ToString(),
                PurchaseTax = bbl.Z_Set(txtPurchaseTax.Text).ToString(),
                TotalPurchaseGaku = rdoResult.Checked ? bbl.Z_Set(lblTotalPurchaseGaku.Text).ToString() : bbl.Z_Set(lblPurchaseGaku.Text).ToString(),
                TaxGaku8 = mTaxGaku8.ToString(),
                TaxGaku10 = mTaxGaku10.ToString(),
                HontaiGaku8 = mHontaiGaku8.ToString(),
                HontaiGaku10 = mHontaiGaku10.ToString(),
                InsertOperator = InOperatorCD,
                PC = InPcID
            };
            
            // 計算税額≠入力税額の場合、差異を足しこむ
            //if (rdoResult.Checked && bbl.Z_Set(txtPurchaseTax.Text) != mTaxCalcGaku) 
            //{
            //    if (mTaxGaku10 != 0)
            //    {
            //        dme.TaxGaku10 = (mTaxGaku10 + mTaxCalcGaku).ToString();
            //    }
            //    else
            //    {
            //        dme.TaxGaku8 = (mTaxGaku8 + mTaxCalcGaku).ToString();
            //    }
            //}

            return dme;
        }
       
        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("MarkDownRows", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("MakerItem", typeof(string));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("PriceOutTax", typeof(decimal));
            dt.Columns.Add("EvaluationPrice", typeof(decimal));
            dt.Columns.Add("StockSu", typeof(int));
            dt.Columns.Add("CalculationSu", typeof(int));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("MarkDownUnitPrice", typeof(decimal));
            dt.Columns.Add("MarkDownGaku", typeof(decimal));
            dt.Columns.Add("PurchaseTax", typeof(decimal));
            dt.Columns.Add("TaxRate", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("TaniName", typeof(string));
            dt.Columns.Add("MDPurchaseRows", typeof(int));
            dt.Columns.Add("PurchaseRows", typeof(int));
            dt.Columns.Add("InsertOperator", typeof(string));
            dt.Columns.Add("InsertDateTime", typeof(DateTime));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                    break;

                    dt.Rows.Add(
                            rowNo
                        , mGrid.g_DArray[RW].SKUCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].JanCD
                        , mGrid.g_DArray[RW].MakerItem
                        , bbl.LeftB(mGrid.g_DArray[RW].SKUName,80)
                        , bbl.LeftB(mGrid.g_DArray[RW].ColorName,20)
                        , bbl.LeftB(mGrid.g_DArray[RW].SizeName,20)
                        , bbl.Z_Set(mGrid.g_DArray[RW].PriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].EvaluationPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].StockSu)    
                        , bbl.Z_Set(mGrid.g_DArray[RW].CalculationSu) 
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rate)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MarkDownUnitPrice) 
                        , bbl.Z_Set(mGrid.g_DArray[RW].MarkDownGaku) 
                        , rdoResult.Checked ? bbl.Z_Set(mGrid.g_DArray[RW].Tax) : 0
                        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate)
                        , mGrid.g_DArray[RW].TaniCD
                        , mGrid.g_DArray[RW].TaniName
                        , rdoResult.Checked ? bbl.Z_Set(mGrid.g_DArray[RW].MDPurchaseRows) : rowNo
                        , rdoResult.Checked ? rowNo : 0
                        , mGrid.g_DArray[RW].InsertOperator
                        , mGrid.g_DArray[RW].InsertDateTime
                    );

                    rowNo++;
            }

            return dt;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        protected void ExecSec(bool isCsv = false)
        {

            if (OperationMode != EOperationMode.DELETE)
            {
                // 仕入先のチェックにて仕入日が必要なため、先に仕入日のチェックを行う
                if (CheckDetail((int)EIndex.PurchaseDate, false) == false)
                {
                    detailControls[(int)EIndex.PurchaseDate].Focus();
                    return;
                }

                for (int i = 0; i <= (int)EIndex.Remark ; i++)
                {
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }
                }                    

                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                    {
                        for (int CL = (int)ClsGridMarkDown.ColNO.Chk; CL < (int)ClsGridMarkDown.ColNO.COUNT; CL++)
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

                //各金額項目の再計算（結果時は手入力可能なため、再計算しない）
                if (rdoPlan.Checked)
                {
                    CalcKin();
                }
            }

            DataTable dt = GetGridEntity();

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (OperationMode != EOperationMode.DELETE)
            {
                if (dt.Rows.Count == 0)
                {
                    //更新対象なし
                    bbl.ShowMessage("E189");
                    return;
                }

            }

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

            //更新処理
            dme = GetEntity();
            mdbl.PRC_MarkDownNyuuryoku(dme, dt, (short)OperationMode);

            if (isCsv)
            {
                this.OutputCsv();
            }

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");


            this.EndSec();

            ////更新後画面クリア
            //ChangeOperationMode(OperationMode);
        }

        /// <summary>
        /// 処理モード変更時処理
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            switch (mode)
            {
                case EOperationMode.INSERT:
                    Scr_Clr(0);
                    S_BodySeigyo(0, 0);
                    S_BodySeigyo(0, 1);
                    //配列の内容を画面にセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    rdoPlan.Focus();
                    break;

                case EOperationMode.UPDATE:
                    S_BodySeigyo(1, 0);
                    S_BodySeigyo(1, 1);
                    //配列の内容を画面にセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                    for (int idx = 0; idx < (int)EIndex.Rate; idx++)
                    {
                        if (detailControls[idx].Enabled)
                        {
                            detailControls[idx].Focus();
                            break;
                        }
                    }
                    break;

                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    S_BodySeigyo(2, 0);
                    S_BodySeigyo(2, 1);
                    //配列の内容を画面にセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
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
                    ((Label)ctl).Text = "";
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
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldPurchaseDate = "";
            mPayeeCD = "";
            mStoreCD = "";
            S_Clear_Grid();   //画面クリア（明細部）

            // フッター部
            lblPurchaseGaku.Text = "";
            txtPurchaseTax.Text = "";
            lblTotalPurchaseGaku.Text = "";
            mTaxGaku8 = 0;
            mTaxGaku10 = 0;
            mHontaiGaku8 = 0;
            mHontaiGaku10 = 0;
            mTaxCalcGaku = 0;
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
                            
                            ScVendorCD.BtnSearch.Enabled = ScVendorCD.TxtCode.Enabled;
                            ScStaff.BtnSearch.Enabled = ScStaff.TxtCode.Enabled;

                            btnInput.Enabled = Kbn == 0 ? true : false;
                            btnSearch.Enabled = Kbn == 0 ? true : false;
                            btnSelectAll.Enabled = Kbn == 0 ? true : false;
                            btnNoSelect.Enabled = Kbn == 0 ? true : false;
                            btnReflect.Enabled = Kbn == 0 ? true : false;

                            //　月次締処理後の時、一部項目を入力不可とする
                            if (Kbn.Equals(0) && mCloseState.Equals(EClose.Closed))
                            {
                                for (int idx = 0; idx < (int)EIndex.UnitPriceDate; idx++)
                                {
                                    detailControls[idx].Enabled = false;
                                }
                                btnInput.Enabled = false;
                                btnSearch.Enabled = false;
                            }

                            pnlKubun.Enabled = Kbn == 0 ? true : false;
                            Pnl_Body.Enabled = Kbn == 0 ? true : false;     
                            
                            txtPurchaseTax.Enabled = Kbn == 0 ? true : false;
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
                case 7: //F8:行削除
                    {
                        DEL_SUB();
                        break;
                    }
                case 10:    //F11:出力
                    {
                        if (OperationMode != EOperationMode.SHOW)
                        {
                            this.ExecSec(true);
                        }
                        else
                        {
                            if (bbl.ShowMessage("Q203") != DialogResult.Yes)
                                return;

                            this.OutputCsv();

                            
                        }
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
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.CostingDate].Text))
                    {
                        ymd = detailControls[(int)EIndex.CostingDate].Text;
                    }

                    using (Search_Product frmProduct = new Search_Product(ymd))
                    {
                        frmProduct.Mode = "5";
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            //JanCdの重複削除
                            List<string> list = new List<string>();
                            foreach (string janCd in frmProduct.list)
                            {
                                if (!list.Contains(janCd))
                                {
                                    list.Add(janCd);
                                }
                            }

                            //  空行の検索
                            int i = 0;
                            List<string> dtlJanCd = new List<string>();
                            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                            {
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                                {
                                    i = RW;
                                    break;
                                }
                                else
                                {
                                    dtlJanCd.Add(mGrid.g_DArray[RW].JanCD);
                                }
                            }

                            //  商品表示
                            foreach (string jancd in list)
                            {
                                //画面に同一のJANCDの明細が存在する場合、表示なし
                                if (dtlJanCd.Contains(jancd))
                                {
                                    bbl.ShowMessage("E226");
                                    continue;
                                }

                                //使用可能行数を超えた場合エラー
                                if (i > m_EnableCnt - 1)
                                {
                                    bbl.ShowMessage("E178", m_EnableCnt.ToString());
                                    mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                                    return;
                                }

                                mGrid.g_DArray[i].JanCD = jancd;
                                this.DispNewDetail(i);

                                m_dataCnt = i + 1;
                                Grid_NotFocus((int)ClsGridMarkDown.ColNO.JanCD, i);
                                i += 1;

                                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }

                            
                        }
                    } 
                    break;

            }

        }

        /// <summary>
        /// 新規明細表示処理
        /// </summary>
        /// <param name="row"></param>
        private void DispNewDetail(int row, string rate = "", string markDownUnitPrice = "" , string calculationSu = "")
        {
            // M_SKUよりセット
            CheckGrid((int)ClsGridMarkDown.ColNO.JanCD, row);

            mGrid.g_DArray[row].Chk = false;
            mGrid.g_DArray[row].Rate = "";
            mGrid.g_DArray[row].MarkDownUnitPrice = "";
            if (!string.IsNullOrWhiteSpace(rate))
            {
                mGrid.g_DArray[row].Rate = bbl.Z_SetStr(rate);

                if (string.IsNullOrWhiteSpace(markDownUnitPrice))
                {
                    mGrid.g_DArray[row].MarkDownUnitPrice = bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[row].PriceOutTax) * bbl.Z_Set(mGrid.g_DArray[row].Rate) / 100));
                }
            }

            if (!string.IsNullOrWhiteSpace(markDownUnitPrice))
            {
                mGrid.g_DArray[row].MarkDownUnitPrice = bbl.Z_SetStr(markDownUnitPrice);
            }

            //[D_StockReplica]
            D_StockReplica_Entity dse = new D_StockReplica_Entity
            {
                ReplicaNO = CboStockInfo.SelectedValue.ToString(),
                AdminNO = mGrid.g_DArray[row].AdminNO,
                SoukoCD = CboSouko.SelectedValue.ToString()
            };

            MarkDownNyuuryoku_BL mbl = new MarkDownNyuuryoku_BL();
            DataTable sdt = mbl.D_StockReplica_SelectForMarkDown(dse);
            if (sdt.Rows.Count > 0)
            {
                mGrid.g_DArray[row].EvaluationPrice = bbl.Z_SetStr(sdt.Rows[0]["LastCost"]);
                mGrid.g_DArray[row].StockSu = bbl.Z_SetStr(sdt.Rows[0]["AllowableSu"]);
            }
            else
            {                
                mGrid.g_DArray[row].StockSu = "0";
            }
            mGrid.g_DArray[row].MarkDownSagakuPrice = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].MarkDownUnitPrice) - bbl.Z_Set(mGrid.g_DArray[row].EvaluationPrice));
            if (!string.IsNullOrWhiteSpace(calculationSu))
            {
                mGrid.g_DArray[row].CalculationSu = bbl.Z_SetStr(calculationSu);
            }
            else
            {
                mGrid.g_DArray[row].CalculationSu = mGrid.g_DArray[row].StockSu;
            }            
            mGrid.g_DArray[row].MarkDownGaku = (bbl.Z_Set(mGrid.g_DArray[row].MarkDownSagakuPrice) * bbl.Z_Set(mGrid.g_DArray[row].CalculationSu)).ToString();
            mGrid.g_DArray[row].Tax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mGrid.g_DArray[row].MarkDownGaku) * mGrid.g_DArray[row].TaxRate / 100);

            mGrid.g_DArray[row].OldEvaluationPrice = mGrid.g_DArray[row].EvaluationPrice;
            mGrid.g_DArray[row].OldRate = mGrid.g_DArray[row].Rate;
            mGrid.g_DArray[row].OldMarkDownUnitPrice = mGrid.g_DArray[row].MarkDownUnitPrice;
            mGrid.g_DArray[row].OldCalculationSu = mGrid.g_DArray[row].CalculationSu;
            mGrid.g_DArray[row].OldMarkDownGaku = mGrid.g_DArray[row].MarkDownGaku;
        }

        /// <summary>
        /// 税額調整
        /// </summary>
        private void AdjustTax()
        {
            // 入力値との税差額
            decimal Sagaku = (mTaxGaku8 + mTaxGaku10) - bbl.Z_Set(txtPurchaseTax.Text);

            if (Sagaku == 0)
                return;

            int LastRow = 0;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) && mGrid.g_DArray[RW].Chk == true)
                {
                    if (Math.Abs(mGrid.g_DArray[RW].Tax) > Math.Abs(Sagaku))
                    {
                        mGrid.g_DArray[RW].Tax -= Sagaku;
                        
                        if (mGrid.g_DArray[RW].TaxRate == 10)
                        {
                            mTaxGaku10 -= Sagaku;
                        }
                        else
                        {
                            mTaxGaku8 -= Sagaku;
                        }
                        Sagaku = 0;

                        break;
                    }
                    else
                    {
                        if (mGrid.g_DArray[RW].TaxRate == 10)
                        {
                            mTaxGaku10 -= bbl.Z_Set(mGrid.g_DArray[RW].Tax);
                        }
                        else
                        {
                            mTaxGaku8 -= bbl.Z_Set(mGrid.g_DArray[RW].Tax);
                        }                        
                        Sagaku -= mGrid.g_DArray[RW].Tax;

                        mGrid.g_DArray[RW].Tax = 0;
                        LastRow = RW;
                    }
                }
            }

            // 差額が残った場合、最終行で調整
            if (Sagaku != 0)
            {
                mGrid.g_DArray[LastRow].Tax -= Sagaku;
                if (mGrid.g_DArray[LastRow].TaxRate == 10)
                {
                    mTaxGaku10 -= Sagaku;
                }
                else
                {
                    mTaxGaku8 -= Sagaku;
                }
                
            }
        }

        #region "内部イベント"
        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
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
                        keyControls[index + 1].Focus();

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
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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
                        if (index == (int)EIndex.Su)
                            ((Control)sender).Focus();
                        //else if (OperationMode != EOperationMode.INSERT &&  index == (int)EIndex.StaffCD)
                        //    //明細の先頭項目へ
                        //    mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridMarkDown.ColNO.PurchaseSu);
                        else if (detailControls.Length - 1 > index)
                        {                            
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

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox ctrl = (ComboBox)sender;
                if (ctrl.SelectedIndex > 0)
                {
                    int index = Array.IndexOf(detailControls, sender);
                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        // D_StockReplicaより最新原価＠、在庫数を再取得
                        this.ReCalcDetail();

                        //あたかもTabキーが押されたかのようにする
                        ProcessTabKey(true);
                    }
                    else
                    {
                        ctrl.Focus();
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

        private void txtPurchaseTax_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }


        private void txtPurchaseTax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool ret = CheckTax();                   
                    
                    ((Control)sender).Focus();
                    
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtPurchaseDate.Enabled = rdoResult.Checked;
                txtPurchaseTax.Visible = rdoResult.Checked;
                lblTitlePurchaseTax.Visible = rdoResult.Checked;
                lblTotalPurchaseGaku.Visible = rdoResult.Checked;
                lblTitleTotalPurchaseGaku.Visible = rdoResult.Checked;
                if (!rdoResult.Checked)
                {
                    txtPurchaseDate.Text = "";
                    txtPurchaseTax.Text = "";
                    lblTotalPurchaseGaku.Text = "";
                }
                else
                {
                    txtPurchaseDate.Text = mOldPurchaseDate;
                    txtPurchaseTax.Text = string.Format("{0:#,##0}", mTaxCalcGaku);
                    lblTotalPurchaseGaku.Text = string.Format("{0:#,##0}", bbl.Z_Set(lblPurchaseGaku.Text) + mTaxCalcGaku);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void RadioButton_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //あたかもTabキーが押されたかのようにする
                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    ProcessTabKey(!e.Shift);
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

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridMarkDown.ColNO.MarkDownGaku)
                        if (w_Row == mGrid.g_MK_Max_Row - 1)
                            lastCell = true;

                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //チェック処理
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
        /// 明細部チェックボックスクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CHK_Sel_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                bool Check = mGrid.g_DArray[w_Row].Chk;

                Grid_NotFocus((int)ClsGridMarkDown.ColNO.Chk, w_Row);

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

        /// <summary>
        /// 取込ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= (int)EIndex.Remark; i++)
                {
                    if (!CheckDetail(i))
                    {
                        detailControls[i].Focus();
                        return;
                    }
                }
                this.ExecInput();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }            
        }

        /// <summary>
        /// 商品検索ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= (int)EIndex.Remark ; i++)
                {
                    if (!CheckDetail(i))
                    {
                        detailControls[i].Focus();
                        return;
                    }
                }
                this.SearchData(EsearchKbn.Product, btnSearch);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }        

        /// <summary>
        /// 全選択ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                //新規登録時のみ、使用可
                this.ChangeCheck(true);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// 全解除ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_NoSelect_Click(object sender, EventArgs e)
        {
            try
            {
                //新規登録時のみ、使用可
                this.ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// 反映ボタン押下時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReflect_Click(object sender, EventArgs e)
        {
            try
            {
                this.ExecReflect();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion
        private void ChangeCheck(bool check)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                {
                    mGrid.g_DArray[RW].Chk = check;

                    Grid_NotFocus((int)ClsGridMarkDown.ColNO.Chk, RW);

                    //配列の内容を画面へセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }
            }
            CalcKin();
        }

        private void ChangeBackColor(int w_Row, short kbn = 0)
        {
            Color backCL = GridBase.ClsGridBase.GrayColor;
            
            for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridMarkDown.ColNO.GYONO:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            break;
                        }
                    case (int)ClsGridMarkDown.ColNO.JanCD:
                    case (int)ClsGridMarkDown.ColNO.ITEMCD:
                    case (int)ClsGridMarkDown.ColNO.SKUName:
                    case (int)ClsGridMarkDown.ColNO.ColorName:
                    case (int)ClsGridMarkDown.ColNO.SizeName:
                    case (int)ClsGridMarkDown.ColNO.MakerItem: 
                    case (int)ClsGridMarkDown.ColNO.SKUCD:
                    case (int)ClsGridMarkDown.ColNO.PriceOutTax:
                    case (int)ClsGridMarkDown.ColNO.EvaluationPrice:
                    case (int)ClsGridMarkDown.ColNO.MarkDownSagakuPrice:
                    case (int)ClsGridMarkDown.ColNO.StockSu:
                        {
                            //mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                            break;
                        }
                }
            }
        }        
    }
}








