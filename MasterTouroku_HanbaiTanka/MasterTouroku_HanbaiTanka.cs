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

namespace MasterTouroku_HanbaiTanka
{
    /// <summary>
    /// MasterTouroku_HanbaiTanka 販売単価マスタ
    /// </summary>
    internal partial class MasterTouroku_HanbaiTanka : FrmMainForm
    {
        private const string ProID = "MasterTouroku_HanbaiTanka";
        private const string ProNm = "販売単価マスタ";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            SyoKBN,
            DisplayKbn,
            StoreCD,
            BrandCD,
            ItemCDFrom,
            ItemCDTo,
            ITemName,

            TankaCD = 0,
            GeneralRate,
            MemberRate,
            ClientRate,
            SaleRate,
            WebRate
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ItemPrice_BL mibl;
        private M_ItemPrice_Entity mie;
        private SKUPrice_BL msbl;
        private M_SKUPrice_Entity mse;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private int mTankaCDRoundKBN;

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridHanbaiTanka mGrid = new ClsGridHanbaiTanka();
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

            if (ClsGridHanbaiTanka.gc_P_GYO <= ClsGridHanbaiTanka.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridHanbaiTanka.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridHanbaiTanka.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridHanbaiTanka.gc_P_GYO;
            //    m_EnableCnt = ClsGridHanbaiTanka.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridHanbaiTanka.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridHanbaiTanka.gc_MaxCL;

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
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                        }
                    }
                    
                     switch(w_CtlCol)
                    {
                        case (int)ClsGridHanbaiTanka.ColNO.ChangeDate:
                        case (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax:
                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax:    // 
                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax:    //  
                            //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                            //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).CKM_Reqired = true;

                            break;
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridHanbaiTanka.ST_DArray_HanbaiTanka[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridHanbaiTanka.ColNO.GYONO; i <= (int)ClsGridHanbaiTanka.ColNO.COUNT - 1; i++)
                mGrid.g_MK_FocusOrder[i] = i;

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHanbaiTanka.ColNO.COUNT; W_CtlCol++)
                {
                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }
            
            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = (int)ClsGridHanbaiTanka.ColNO.PriceWithTax; W_CtlCol <= (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax; W_CtlCol++)
                    mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
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
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 0].CellCtl = IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 0].CellCtl = IMN_GENKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 0].CellCtl = IMN_TEIKA_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 0].CellCtl = IMN_GENER_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 0].CellCtl = IMN_GENER2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 0].CellCtl = IMN_MEMBR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 0].CellCtl = IMN_CLINT2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 0].CellCtl = IMN_SALEP_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 0].CellCtl = IMN_SALEP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 0].CellCtl = IMN_WEBPR2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 0].CellCtl = IMT_REMAK_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 1].CellCtl = IMN_GENKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 1].CellCtl = IMN_TEIKA_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 1].CellCtl = IMN_GENER_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 1].CellCtl = IMN_GENER2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 1].CellCtl = IMN_MEMBR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 1].CellCtl = IMN_CLINT2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 1].CellCtl = IMN_SALEP_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 1].CellCtl = IMN_SALEP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 1].CellCtl = IMN_WEBPR2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 1].CellCtl = IMT_REMAK_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 2].CellCtl = IMN_GENKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 2].CellCtl = IMN_TEIKA_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 2].CellCtl = IMN_GENER_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 2].CellCtl = IMN_GENER2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 2].CellCtl = IMN_MEMBR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 2].CellCtl = IMN_CLINT2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 2].CellCtl = IMN_SALEP_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 2].CellCtl = IMN_SALEP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 2].CellCtl = IMN_WEBPR2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 2].CellCtl = IMT_REMAK_2;

            // 4行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 3].CellCtl = IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 3].CellCtl = IMN_GENKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 3].CellCtl = IMN_TEIKA_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 3].CellCtl = IMN_GENER_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 3].CellCtl = IMN_GENER2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 3].CellCtl = IMN_MEMBR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 3].CellCtl = IMN_CLINT2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 3].CellCtl = IMN_SALEP_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 3].CellCtl = IMN_SALEP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 3].CellCtl = IMN_WEBPR2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 3].CellCtl = IMT_REMAK_3;

            // 5行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 4].CellCtl = IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 4].CellCtl = IMN_GENKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 4].CellCtl = IMN_TEIKA_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 4].CellCtl = IMN_GENER_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 4].CellCtl = IMN_GENER2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 4].CellCtl = IMN_MEMBR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 4].CellCtl = IMN_CLINT2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 4].CellCtl = IMN_SALEP_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 4].CellCtl = IMN_SALEP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 4].CellCtl = IMN_WEBPR2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 4].CellCtl = IMT_REMAK_4;

            // 6行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 5].CellCtl = IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 5].CellCtl = IMN_GENKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 5].CellCtl = IMN_TEIKA_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 5].CellCtl = IMN_GENER_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 5].CellCtl = IMN_GENER2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 5].CellCtl = IMN_MEMBR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 5].CellCtl = IMN_CLINT2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 5].CellCtl = IMN_SALEP_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 5].CellCtl = IMN_SALEP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 5].CellCtl = IMN_WEBPR2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 5].CellCtl = IMT_REMAK_5;

            // 7行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 6].CellCtl = IMN_GENKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 6].CellCtl = IMN_TEIKA_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 6].CellCtl = IMN_TEIKA2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 6].CellCtl = IMN_GENER_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 6].CellCtl = IMN_GENER2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 6].CellCtl = IMN_MEMBR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 6].CellCtl = IMN_MEMBR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 6].CellCtl = IMN_CLINT2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 6].CellCtl = IMN_SALEP_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 6].CellCtl = IMN_SALEP2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 6].CellCtl = IMN_WEBPR2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 6].CellCtl = IMT_REMAK_6;

            // 8行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 7].CellCtl = IMN_GENKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 7].CellCtl = IMN_TEIKA_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 7].CellCtl = IMN_TEIKA2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 7].CellCtl = IMN_GENER_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 7].CellCtl = IMN_GENER2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 7].CellCtl = IMN_MEMBR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 7].CellCtl = IMN_MEMBR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 7].CellCtl = IMN_CLINT2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 7].CellCtl = IMN_SALEP_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 7].CellCtl = IMN_SALEP2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 7].CellCtl = IMN_WEBPR2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 7].CellCtl = IMT_REMAK_7;

            // 9行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 8].CellCtl = IMN_GENKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 8].CellCtl = IMN_TEIKA_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 8].CellCtl = IMN_TEIKA2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 8].CellCtl = IMN_GENER_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 8].CellCtl = IMN_GENER2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 8].CellCtl = IMN_MEMBR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 8].CellCtl = IMN_MEMBR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 8].CellCtl = IMN_CLINT2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 8].CellCtl = IMN_SALEP_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 8].CellCtl = IMN_SALEP2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 8].CellCtl = IMN_WEBPR2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 8].CellCtl = IMT_REMAK_8;

            // 10行目
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.JanCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ChangeDate, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax, 9].CellCtl = IMN_GENKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ITemName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithTax, 9].CellCtl = IMN_TEIKA_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax, 9].CellCtl = IMN_TEIKA2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax, 9].CellCtl = IMN_GENER_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax, 9].CellCtl = IMN_GENER2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax, 9].CellCtl = IMN_MEMBR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax, 9].CellCtl = IMN_MEMBR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax, 9].CellCtl = IMN_CLINT2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax, 9].CellCtl = IMN_SALEP_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax, 9].CellCtl = IMN_SALEP2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax, 9].CellCtl = IMN_WEBPR2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTanka.ColNO.Remarks, 9].CellCtl = IMT_REMAK_9;

        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvPrv, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 MouseWheel の処理
        private void S_Grid_0_Event_MouseWheel(int pDelta)
        {
            int w_ToMove = pDelta * (-1) / (int)mGrid.g_WHEEL_DELTA;
            int w_Value;
            int w_MaxValue;

            mGrid.g_WheelFLG = true;

            if (mGrid.g_MK_MaxValue > m_dataCnt - 1)
                w_MaxValue = m_dataCnt - 1;
            else
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
                            case (int)ClsGridHanbaiTanka.ColNO.GYONO:
                                mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                break;
                            case (int)ClsGridHanbaiTanka.ColNO.ITemCD:
                            case (int)ClsGridHanbaiTanka.ColNO.JanCD:
                            case (int)ClsGridHanbaiTanka.ColNO.ITemName:
                            case (int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax:
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

                    //// クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case object _ when ClsGridHanbaiTanka.ColNO.DELCK:
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

        // ---------------------------------------------
        // ボディ部の状態制御
        // 
        // 引数    pKBN    0...  新規/修正/削除時(モード選択時)
        // 1...  新規/修正時(画面展開後)
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

                        IMT_DMY_0.Focus();
                        Scr_Lock(0, 0, 0);   // フレームのロック解除
                        Scr_Lock(1, mc_L_END, 1);  // フレームのロック
                        SetEnabled();
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
                                if (String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ITemCD) == false)
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        {
                                            case (int)ClsGridHanbaiTanka.ColNO.ChangeDate:
                                            case (int)ClsGridHanbaiTanka.ColNO.PriceWithTax:    //
                                            case (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax:    // 
                                            case (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax:    //  
                                            case (int)ClsGridHanbaiTanka.ColNO.Remarks:    //
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
                            IMT_DMY_0.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
                            Scr_Lock(2, 3, 0);
                            Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "111111000001");
                            btnSubF11.Enabled = false;

                            if (radioButton2.Checked)
                            {
                                //単価CD変更不可にする
                                detailControls[(int)EIndex.TankaCD].Enabled = false;
                                ScTanka.BtnSearch.Enabled = false;
                            }
                        }
                        break;
                    }

                case 2:
                    {
                        if (pGrid == 1)
                        {
                            // 使用可項目無  明細部スクロールのみ可
                            // IMT_DMY_0.Focus()
                            //SetFuncKeyAll(this, "000010000101", "11100000");
                            Pnl_Body.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
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

            //mGrid.g_MK_Ctrl(ClsGridHanbaiTanka.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            //SetFuncKeyAll(this, "111111000001");  おそらく再度設定不要

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

        #endregion

        public MasterTouroku_HanbaiTanka()
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
                base.InProgramID = ProID;

                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                // 明細部初期化
                this.S_SetInit_Grid();
                mibl = new ItemPrice_BL();
                msbl = new SKUPrice_BL();

                Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

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
            keyControls = new Control[] { panel1, panel2, ScStore.TxtCode, ScBrand.TxtCode, ScSkuFrom.TxtCode, ScSkuTo.TxtCode, ckM_Text_4 };
            keyLabels = new Control[] { ScStore, ScBrand};
            detailControls = new Control[] { ScTanka.TxtCode, ckM_Text_6, ckM_Text_7, ckM_Text_8, ckM_Text_9, ckM_Text_10 };
            detailLabels = new Control[] { ScTanka };
            searchButtons = new Control[] { ScStore.BtnSearch, ScBrand.BtnSearch, ScSkuFrom.BtnSearch, ScSkuFrom.BtnSearch};

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

                int index = Array.IndexOf(detailControls, ctl);
                if (index != (int)EIndex.TankaCD)
                {
                    ((CKM_Controls.CKM_TextBox)ctl).IntegerPart = 3;
                   ((CKM_Controls.CKM_TextBox)ctl).DecimalPlace = 2;
                     ((CKM_Controls.CKM_TextBox)ctl).MaxLength = 6;
                }
            }

            radioButton1.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            radioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            radioButton4.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
        }

        private bool CheckKey(int index)
        {
            return this.CheckKey(index, true);
        }

        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set)
        {

            switch (index)
            {
                case (int)EIndex.StoreCD:
                    //商品分類＝SKUの場合必須入力、入力なければエラー
                    if (radioButton2.Checked)
                    {
                        if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            ScStore.LabelText = "";
                            return false;
                        }
                        else
                        {
                            //[M_Store_SelectData]
                            M_Store_Entity mse = new M_Store_Entity
                            {
                                StoreCD = keyControls[index].Text,
                                ChangeDate = mibl.GetDate()
                            };
                            Store_BL sbl = new Store_BL();
                            DataTable dt = sbl.M_Store_Select(mse);
                            if (dt.Rows.Count > 0)
                            {
                                ScStore.LabelText = dt.Rows[0]["StoreName"].ToString();
                            }
                            else
                            {
                                bbl.ShowMessage("E133");
                                ScStore.LabelText  = "";
                                return false;
                            }
                        }
                    }
                    break;

                case (int)EIndex.BrandCD:
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        ScBrand.LabelText = "";
                    }
                    else
                    {
                        //[M_Brand]
                        M_Brand_Entity mme = new M_Brand_Entity
                        {
                            ChangeDate = mibl.GetDate(),
                            BrandCD = keyControls[index].Text
                        };
                        Brand_BL mbl = new Brand_BL();
                        bool ret = mbl.M_Brand_Select(mme);
                        if (ret)
                        {
                            ScBrand.LabelText = mme.BrandName;
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            ScBrand.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.ItemCDTo:
                    if (keyControls[index].Text != "")
                    {
                        int result = keyControls[index].Text.CompareTo(keyControls[index - 1].Text);
                        if (result < 0)
                        {
                            //Ｅ１０６
                            bbl.ShowMessage("E106");
                            keyControls[index].Focus();
                            return false;
                        }
                    }
                    break;
            }

            if(radioButton1.Checked && index==(int)EIndex.ITemName)
                return CheckData(set);

            return true;

        }

        /// <summary>
        /// 単価データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set)
        {
            DataTable dt;

            if (radioButton1.Checked)
            {
                //[M_ItemPrice_SelectData]
                mie = GetEntity();
                dt = mibl.ItemPrice_SelectData(mie, (short)OperationMode);
            }
            else
            {
                //[M_SKUPrice_SelectData]
                mse = GetEntity_SKU();
                dt = msbl.SKUPrice_SelectData(mse, (short)OperationMode);
            }

            //以下の条件で販売単価マスターが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
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

                DataRow[] rows;

                //画面.表示内容＝改定日直近の場合
                if (radioButton3.Checked)
                {
                    rows = dt.Select("row=1");
                    dt = rows.CopyToDataTable();
                }

                //明細にデータをセット
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    //明細にデータをセット

                    if (radioButton1.Checked)
                    {
                        mGrid.g_DArray[i].ITemCD = row["ITemCD"].ToString();
                        detailControls[(int)EIndex.TankaCD].Text = "0000000000000";
                        mGrid.g_DArray[i].ITemName = row["ITemName"].ToString();   // 
                    }
                    else
                    {
                        detailControls[(int)EIndex.TankaCD].Text = row["TankaCD"].ToString();
                        mGrid.g_DArray[i].ITemCD = row["SKUCD"].ToString();
                        mGrid.g_DArray[i].AdminNo = row["AdminNo"].ToString();       //2019.10.16 add
                        mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                        mGrid.g_DArray[i].ITemName = row["SKUName"].ToString();   // 
                    }

                    if (i == 0)     
                    {
                        detailControls[(int)EIndex.GeneralRate].Text = bbl.Z_SetStr(row["GeneralRate"]);
                        detailControls[(int)EIndex.MemberRate].Text = bbl.Z_SetStr(row["MemberRate"]);
                        detailControls[(int)EIndex.ClientRate].Text = bbl.Z_SetStr(row["ClientRate"]);
                        detailControls[(int)EIndex.SaleRate].Text = bbl.Z_SetStr(row["SaleRate"]);
                        detailControls[(int)EIndex.WebRate].Text = bbl.Z_SetStr(row["WebRate"]);
                    }

                    if (OperationMode == EOperationMode.INSERT  && radioButton1.Checked)
                    {
                        CheckDetail((int)EIndex.TankaCD);
                    }

                    mGrid.g_DArray[i].ChangeDate = row["ChangeDate"].ToString();   // 
                    mGrid.g_DArray[i].OldChangeDate = row["ChangeDate"].ToString();   // 
                    mGrid.g_DArray[i].GenkaWithoutTax = bbl.Z_SetStr(row["GenkaWithoutTax"]);   // 
                    mGrid.g_DArray[i].PriceWithTax = bbl.Z_SetStr(row["PriceWithTax"]);   // 
                    mGrid.g_DArray[i].PriceWithoutTax = bbl.Z_SetStr(row["PriceWithoutTax"]);   // 
                    mGrid.g_DArray[i].GeneralPriceWithTax = bbl.Z_SetStr(row["GeneralPriceWithTax"]);   // 
                    mGrid.g_DArray[i].GeneralPriceOutTax = bbl.Z_SetStr(row["GeneralPriceOutTax"]);   // 
                    mGrid.g_DArray[i].MemberPriceWithTax = bbl.Z_SetStr(row["MemberPriceWithTax"]);   // 
                    mGrid.g_DArray[i].MemberPriceOutTax = bbl.Z_SetStr(row["MemberPriceOutTax"]);   // 
                    mGrid.g_DArray[i].ClientPriceWithTax = bbl.Z_SetStr(row["ClientPriceWithTax"]);   // 
                    mGrid.g_DArray[i].ClientPriceOutTax = bbl.Z_SetStr(row["ClientPriceOutTax"]);   // 
                    mGrid.g_DArray[i].SalePriceWithTax = bbl.Z_SetStr(row["SalePriceWithTax"]);   // 
                    mGrid.g_DArray[i].SalePriceOutTax = bbl.Z_SetStr(row["SalePriceOutTax"]);   // 
                    mGrid.g_DArray[i].WebPriceWithTax = bbl.Z_SetStr(row["WebPriceWithTax"]);   // 
                    mGrid.g_DArray[i].WebPriceOutTax = bbl.Z_SetStr(row["WebPriceOutTax"]);   //  
                    mGrid.g_DArray[i].Remarks = row["Remarks"].ToString();   // 
                    if (string.IsNullOrWhiteSpace(row["UpdateOperator"].ToString()))
                    {
                        mGrid.g_DArray[i].Update = 0;
                        mGrid.g_DArray[i].OldUpdate = 0;
                    }
                    else
                    {
                        mGrid.g_DArray[i].Update = 1;
                        mGrid.g_DArray[i].OldUpdate = 1;
                    }
                    mGrid.g_DArray[i].TaxRateFLG = Convert.ToInt16(row["TaxRateFLG"]);

                    m_dataCnt = i + 1;
                    //Grid_NotFocus(CTII0011.ColNO.HAKKU, i)
                    i++;
                }

                //mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHanbaiTanka.ColNO.ChangeDate);
            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                previousCtrl.Focus();
            }

            return true;
        }

        protected override void ExecDisp()
        {
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i, false) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

            CheckData(true);
        }

        private bool CheckDetail(int index)
        {
            return CheckDetail(index, true);
        }
        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.TankaCD:
                    mTankaCDRoundKBN = (int)HASU_KBN.SISYAGONYU;

                    //商品分類＝SKUの場合必須入力、入力なければエラー
                    if (radioButton2.Checked)
                    {
                        //単価設定CD 入力必須(Entry required)
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            ScTanka.LabelText = "";
                            return false;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text) == false)
                    {
                        //以下の条件でM_TankaCDが存在しない場合、エラー

                        //[M_TankaCD]
                        M_TankaCD_Entity mte = new M_TankaCD_Entity
                        {
                            TankaCD = detailControls[index].Text,
                            ChangeDate = mibl.GetDate()
                        };
                        TankaCD_BL mbl = new TankaCD_BL();
                        DataTable dt = mbl.M_TankaCD_Select(mte);
                        if (dt.Rows.Count > 0)
                        {
                            ScTanka.LabelText = dt.Rows[0]["TankaName"].ToString();
                            mTankaCDRoundKBN = Convert.ToInt16(dt.Rows[0]["RoundKBN"]);
                            if (set)
                            {
                                detailControls[(int)EIndex.GeneralRate].Text = bbl.Z_SetStr(dt.Rows[0]["GeneralRate"]);
                                detailControls[(int)EIndex.MemberRate].Text = bbl.Z_SetStr(dt.Rows[0]["MemberRate"]);
                                detailControls[(int)EIndex.ClientRate].Text = bbl.Z_SetStr(dt.Rows[0]["ClientRate"]);
                                detailControls[(int)EIndex.SaleRate].Text = bbl.Z_SetStr(dt.Rows[0]["SaleRate"]);
                                detailControls[(int)EIndex.WebRate].Text = bbl.Z_SetStr(dt.Rows[0]["WebRate"]);
                            }
                        }
                        else
                        {
                            //Ｅ１０１
                            bbl.ShowMessage("E101");
                            ScTanka.LabelText = "";
                            return false;
                        }
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

            w_Ctrl = detailControls[(int)EIndex.ITemName];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        private bool CheckGrid(int col, int row)
        {

            switch (col)
            {
                case (int)ClsGridHanbaiTanka.ColNO.ChangeDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ChangeDate))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    mGrid.g_DArray[row].ChangeDate = bbl.FormatDate(mGrid.g_DArray[row].ChangeDate);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(mGrid.g_DArray[row].ChangeDate))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    //改定日変更時、ITEM販売単価マスタまたはSKU販売単価マスタに存在するかチェック
                    if (mGrid.g_DArray[row].ChangeDate != mGrid.g_DArray[row].OldChangeDate)
                    {
                        if (radioButton1.Checked)
                        {
                            //商品分類：ITEM選択時
                            //[M_ItemPrice]
                            M_ItemPrice_Entity mie = new M_ItemPrice_Entity
                            {
                                ITemCD = mGrid.g_DArray[row].ITemCD,
                                ChangeDate = mGrid.g_DArray[row].ChangeDate
                            };
                            ItemPrice_BL mbl = new ItemPrice_BL();
                            DataTable dt = mbl.M_ItemPrice_Select(mie);
                            if (dt.Rows.Count > 0)
                            {
                                //Ｅ１０５
                                bbl.ShowMessage("E105");
                                return false;
                            }
                            else
                            {
                                mGrid.g_DArray[row].Update = 0;
                            }
                        }
                        else
                        {
                            //商品分類：SKU選択時
                            //[M_SKUPrice]
                            M_SKUPrice_Entity mie = new M_SKUPrice_Entity
                            {
                                StoreCD = keyControls[(int)EIndex.StoreCD].Text,
                                TankaCD = detailControls[(int)EIndex.TankaCD].Text,
                                SKUCD = mGrid.g_DArray[row].ITemCD,
                                AdminNO = mGrid.g_DArray[row].AdminNo,
                                ChangeDate = mGrid.g_DArray[row].ChangeDate
                            };
                            SKUPrice_BL mbl = new SKUPrice_BL();
                            DataTable dt = mbl.M_SKUPrice_Select(mie);
                            if (dt.Rows.Count > 0)
                            {
                                //Ｅ１０５
                                bbl.ShowMessage("E105");
                                return false;
                            }
                            else
                            {
                                mGrid.g_DArray[row].Update = 0;
                            }
                        }
                    }
                    else
                    {
                        //Updateフラグを元に戻す
                        mGrid.g_DArray[row].Update = mGrid.g_DArray[row].OldUpdate;
                    }
                    break;

                case (int)ClsGridHanbaiTanka.ColNO.Remarks:
                    break;

                case (int)ClsGridHanbaiTanka.ColNO.GenkaWithoutTax:
                    break;

                case (int)ClsGridHanbaiTanka.ColNO.ITemName:
                    break;

                default:
                    string val = "";

                    switch (col)
                    {
                        case (int)ClsGridHanbaiTanka.ColNO.PriceWithTax:
                            val=mGrid.g_DArray[row].PriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax:
                            val = mGrid.g_DArray[row].PriceWithoutTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax:    // 
                            val = mGrid.g_DArray[row].GeneralPriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax:    // 
                            val = mGrid.g_DArray[row].GeneralPriceOutTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax:    // 
                            val = mGrid.g_DArray[row].MemberPriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax:    // 
                            val = mGrid.g_DArray[row].MemberPriceOutTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax:    // 
                            val = mGrid.g_DArray[row].ClientPriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax:    // 
                            val = mGrid.g_DArray[row].ClientPriceOutTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax:    // 
                            val = mGrid.g_DArray[row].SalePriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax:    // 
                            val = mGrid.g_DArray[row].SalePriceOutTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax:    // 
                            val = mGrid.g_DArray[row].WebPriceWithTax;
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax:    //  
                            val = mGrid.g_DArray[row].WebPriceOutTax;
                            break;
                    }

                    //必須入力、入力なければエラー
                    if (string.IsNullOrWhiteSpace(val))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private M_ItemPrice_Entity GetEntity()
        {

            mie = new M_ItemPrice_Entity();
            //StoreCD = keyControls[(int)EIndex.StoreCD].Text,
            mie.BrandCD = keyControls[(int)EIndex.BrandCD].Text;
            mie.ItemFrom = keyControls[(int)EIndex.ItemCDFrom].Text;
            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.ItemCDTo].Text))
                mie.ItemTo = new string('Z', ScSkuTo.TxtCode.MaxLength);
            else
                mie.ItemTo = keyControls[(int)EIndex.ItemCDTo].Text;

            mie.ITemName = keyControls[(int)EIndex.ITemName].Text;
            mie.SyoKBN = "0";
            
            mie.GeneralRate = bbl.Z_SetStr(detailControls[(int)EIndex.GeneralRate].Text);
            mie.MemberRate = bbl.Z_SetStr(detailControls[(int)EIndex.MemberRate].Text);
            mie.ClientRate = bbl.Z_SetStr(detailControls[(int)EIndex.ClientRate].Text);
            mie.SaleRate = bbl.Z_SetStr(detailControls[(int)EIndex.SaleRate].Text);
            mie.WebRate = bbl.Z_SetStr(detailControls[(int)EIndex.WebRate].Text);
            mie.DeleteFlg = "0";
            mie.UsedFlg = "0";
            //mse.InsertOperator = this.gOpeCD;
            //mse.UpdateOperator = this.gOpeCD;

            return mie;
        }
        private M_SKUPrice_Entity GetEntity_SKU()
        {
            mse = new M_SKUPrice_Entity();
            mse.StoreCD = keyControls[(int)EIndex.StoreCD].Text;
            mse.TankaCD = detailControls[(int)EIndex.TankaCD].Text;
            mse.BrandCD = keyControls[(int)EIndex.BrandCD].Text;
            mse.ItemFrom = keyControls[(int)EIndex.ItemCDFrom].Text;
            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.ItemCDTo].Text))
                mse.ItemTo = new string('Z', ScSkuTo.TxtCode.MaxLength);
            else
                mse.ItemTo = keyControls[(int)EIndex.ItemCDTo].Text;
            mse.ITemName = keyControls[(int)EIndex.ITemName].Text;
            mse.SyoKBN = "1";

            mse.GeneralRate = bbl.Z_SetStr(detailControls[(int)EIndex.GeneralRate].Text);
            mse.MemberRate = bbl.Z_SetStr(detailControls[(int)EIndex.MemberRate].Text);
            mse.ClientRate = bbl.Z_SetStr(detailControls[(int)EIndex.ClientRate].Text);
            mse.SaleRate = bbl.Z_SetStr(detailControls[(int)EIndex.SaleRate].Text);
            mse.WebRate = bbl.Z_SetStr(detailControls[(int)EIndex.WebRate].Text);
            mse.DeleteFlg = "0";
            mse.UsedFlg = "0";

            return mse;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("ITemCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("ChangeDate", typeof(string));
            dt.Columns.Add("PriceWithTax", typeof(decimal));
            dt.Columns.Add("PriceWithoutTax", typeof(decimal));
            dt.Columns.Add("GeneralPriceWithTax", typeof(decimal));
            dt.Columns.Add("GeneralPriceOutTax", typeof(decimal));
            dt.Columns.Add("MemberPriceWithTax", typeof(decimal));
            dt.Columns.Add("MemberPriceOutTax", typeof(decimal));
            dt.Columns.Add("ClientPriceWithTax", typeof(decimal));
            dt.Columns.Add("ClientPriceOutTax", typeof(decimal));
            dt.Columns.Add("SalePriceWithTax", typeof(decimal));
            dt.Columns.Add("SalePriceOutTax", typeof(decimal));
            dt.Columns.Add("WebPriceWithTax", typeof(decimal));
            dt.Columns.Add("WebPriceOutTax", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);
            
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //m_dataCntが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ITemCD) == false)
                {
                    dt.Rows.Add(mGrid.g_DArray[RW].ITemCD
                        , mGrid.g_DArray[RW].AdminNo
                        , mGrid.g_DArray[RW].ChangeDate
                        , bbl.Z_Set( mGrid.g_DArray[RW].PriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].PriceWithoutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].GeneralPriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].GeneralPriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MemberPriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].MemberPriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ClientPriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ClientPriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalePriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalePriceOutTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].WebPriceWithTax)
                        , bbl.Z_Set(mGrid.g_DArray[RW].WebPriceOutTax)
                        , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
                        , mGrid.g_DArray[RW].Update
                        );

                    
                }
            }

            return dt;
        }
        protected override void ExecSec()
        {
            for (int i = 0; i < keyControls.Length; i++)
                if (CheckKey(i, false) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

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
                //m_dataCntが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ITemCD) == false)
                {
                    for (int CL = (int)ClsGridHanbaiTanka.ColNO.ChangeDate; CL < (int)ClsGridHanbaiTanka.ColNO.COUNT; CL++)
                    {
                        if (CheckGrid(CL, RW) == false)
                        {
                            //Focusセット処理
                            ERR_FOCUS_GRID_SUB(CL, RW);
                            return;
                        }
                    }
                }
            }

            DataTable dt = GetGridEntity();

            //更新処理
            if (radioButton1.Checked)
            {
                mie = GetEntity();
                mibl.ItemPrice_Exec(mie,dt, (short)OperationMode, InOperatorCD, InPcID);
            }
            else
            {
                mse = GetEntity_SKU();
                msbl.SKUPrice_Exec(mse, dt, (short)OperationMode, InOperatorCD, InPcID);
            }

            //更新後画面クリア
            InitScr();

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");
        }

        /// <summary>
        /// 画面の初期化
        /// </summary>
        private void InitScr()
        {
            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            ChangeOperationMode(base.OperationMode);
        }

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //if (OldOperationMode != OperationMode)
            //{
                Scr_Clr(0);
            //}

            switch (mode)
            {
                case EOperationMode.INSERT:
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    S_BodySeigyo(0, 0);
                    break;

            }

            btnSubF11.Enabled = true;
            
            radioButton1.Focus();
            radioButton1.Select();
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
                    if (ctl.GetType().Equals(typeof(Panel)))
                    {
                        radioButton1.Checked = true;
                        radioButton3.Checked = true;
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
                if (ctl.GetType().Equals(typeof(CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(RadioButton)))
                {
                    ((RadioButton)ctl).Checked = true;
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

            S_Clear_Grid();   //画面クリア（明細部）

            if (Kbn == 0)
                SetEnabled();
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
                            for (int index = 0; index < searchButtons.Length; index++)
                                searchButtons[index].Enabled = Kbn == 0 ? true : false;

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
                            ScTanka.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            BtnCalcAll.Enabled = Kbn == 0 ? true : false;
                            BtnCalcZero.Enabled = Kbn == 0 ? true : false;

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

                        InitScr();

                        break;
                    }

                //case 8: //F9:検索
                //    EsearchKbn kbn = EsearchKbn.Null;

                //    if (Array.IndexOf(keyControls, previousCtrl) == (int)EIndex.StoreCD ||
                //        Array.IndexOf(keyLabels, previousCtrl) == (int)EIndex.StoreCD)
                //    {
                //        //店舗検索
                //        kbn = EsearchKbn.Store;
                //    }
                //    else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.BrandCD)
                //    {
                //        //汎用検索
                //        kbn = EsearchKbn.Brand;
                //    }
                //    else if (Array.IndexOf(detailControls, previousCtrl) == (int)EIndex.TankaCD)
                //    {
                //        //単価設定検索
                //        kbn = EsearchKbn.Tanka;
                //    }

                //    if (kbn != EsearchKbn.Null)
                //        SearchData(kbn, previousCtrl);

                //    break;

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
            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
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
                        if (index == (int)EIndex.ITemName)
                        {
                            detailControls[(int)EIndex.TankaCD].Focus();
                        }
                        else
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

                        if (radioButton2.Checked && index==(int)EIndex.TankaCD)
                        {
                            ExecDisp();
                            return;
                        }

                        if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
                        else
                        {
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTanka.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHanbaiTanka.ColNO.ChangeDate);
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
                CKM_Controls.CKM_TextBox w_ActCtl;

                w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色
                w_ActCtl.BackColor = ClsGridBase.BKColor;

                Grid_Gotfocus((int)ClsGridHanbaiTanka.ColNO.ChangeDate, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

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
                CKM_Controls.CKM_TextBox w_ActCtl;

                w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色
                w_ActCtl.BackColor = mGrid.F_GetBackColor_MK((int)ClsGridHanbaiTanka.ColNO.ChangeDate, w_Row);

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
                    CKM_Controls.CKM_TextBox w_ActCtl;

                    w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    //どの項目か判別
                    int CL=(int)ClsGridHanbaiTanka.ColNO.ChangeDate;
                    string ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;

                    switch (ctlName)
                    {
                        case "IMN_TEIKA":
                            CL = (int)ClsGridHanbaiTanka.ColNO.PriceWithTax;
                            break;
                        case "IMN_TEIKA2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax;
                            break;
                        case "IMN_GENER":
                            CL = (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax;
                            break;
                        case "IMN_GENER2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax;
                            break;
                        case "IMN_MEMBR":
                            CL = (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax;
                            break;
                        case "IMN_MEMBR2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax;
                            break;
                        case "IMN_CLINT":
                            CL = (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax;
                            break;
                        case "IMN_CLINT2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax;
                            break;
                        case "IMN_SALEP":
                            CL = (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax;
                            break;
                        case "IMN_SALEP2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax;
                            break;
                        case "IMN_WEBPR":
                            CL = (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax;
                            break;
                        case "IMN_WEBPR2":
                            CL = (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax;
                            break;
                        case "IMT_REMAK":
                            CL = (int)ClsGridHanbaiTanka.ColNO.Remarks;
                            if (w_Row == m_dataCnt - 1)
                                lastCell = true;
                            break;

                    }

                    int w_CtlRow = w_Row - Vsb_Mei_0.Value;
                    if (mGrid.g_MK_Ctrl[CL, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[CL, w_CtlRow].CellCtl).isMaxLengthErr)
                            return ;
                    }

                    bool changeFlg = false;
                    switch (CL)
                    {
                        case (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax:
                            if (!mGrid.g_DArray[w_Row].PriceWithoutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax:
                            if (!mGrid.g_DArray[w_Row].GeneralPriceOutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;

                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax:
                            if (!mGrid.g_DArray[w_Row].MemberPriceOutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax:
                            if (!mGrid.g_DArray[w_Row].ClientPriceOutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax:
                            if (!mGrid.g_DArray[w_Row].SalePriceOutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax:
                            if (!mGrid.g_DArray[w_Row].WebPriceOutTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.PriceWithTax:
                            if (!mGrid.g_DArray[w_Row].PriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax:
                            if (!mGrid.g_DArray[w_Row].GeneralPriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax:
                            if (!mGrid.g_DArray[w_Row].MemberPriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax:
                            if (!mGrid.g_DArray[w_Row].ClientPriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax:
                            if (!mGrid.g_DArray[w_Row].SalePriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                        case (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax:
                            if (!mGrid.g_DArray[w_Row].WebPriceWithTax.Equals(w_ActCtl.Text))
                            {
                                changeFlg = true;
                            }
                            break;
                    }


                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    //手入力時、(税抜)⇒(税込)or(税込)⇒(税抜)金額を再計算
                    if(changeFlg)
                    {
                        decimal d;
                        decimal zei;
                        string ymd = mGrid.g_DArray[w_Row].ChangeDate;
                        int taxRateFlg = mGrid.g_DArray[w_Row].TaxRateFLG;
                        switch (CL)
                        {
                            //(税抜)⇒(税込)
                            case (int)ClsGridHanbaiTanka.ColNO.PriceWithoutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].PriceWithoutTax, out d))
                                    mGrid.g_DArray[w_Row].PriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].PriceWithTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceOutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].GeneralPriceOutTax, out d))
                                    mGrid.g_DArray[w_Row].GeneralPriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].GeneralPriceWithTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.MemberPriceOutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].MemberPriceOutTax, out d))
                                    mGrid.g_DArray[w_Row].MemberPriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].MemberPriceWithTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.ClientPriceOutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].ClientPriceOutTax, out d))
                                    mGrid.g_DArray[w_Row].ClientPriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].ClientPriceWithTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.SalePriceOutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].SalePriceOutTax, out d))
                                    mGrid.g_DArray[w_Row].SalePriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].SalePriceWithTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.WebPriceOutTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].WebPriceOutTax, out d))
                                    mGrid.g_DArray[w_Row].WebPriceWithTax = string.Format("{0:#,##0}", bbl.GetZeikomiKingaku(d, taxRateFlg, out zei, ymd));
                                else
                                    mGrid.g_DArray[w_Row].WebPriceWithTax = "0";
                                break;

                            //(税込)⇒(税抜)
                            case (int)ClsGridHanbaiTanka.ColNO.PriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].PriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].PriceWithoutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].PriceWithoutTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.GeneralPriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].GeneralPriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].GeneralPriceOutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].GeneralPriceOutTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.MemberPriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].MemberPriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].MemberPriceOutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].MemberPriceOutTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.ClientPriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].ClientPriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].ClientPriceOutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].ClientPriceOutTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.SalePriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].SalePriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].SalePriceOutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].SalePriceOutTax = "0";
                                break;

                            case (int)ClsGridHanbaiTanka.ColNO.WebPriceWithTax:
                                if (Decimal.TryParse(mGrid.g_DArray[w_Row].WebPriceWithTax, out d))
                                    mGrid.g_DArray[w_Row].WebPriceOutTax = string.Format("{0:#,##0}", bbl.GetZeinukiKingaku(d, taxRateFlg, ymd));
                                else
                                    mGrid.g_DArray[w_Row].WebPriceOutTax = "0";
                                break;
                        }
                    }

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
                    this.ProcessTabKey(!e.Shift);
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
                SetEnabled();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void SetEnabled()
        {
            if (radioButton1.Checked)
            {
                lblSkuCD.Text = "ITEMCD";
                keyControls[(int)EIndex.StoreCD].Enabled = false;
                keyControls[(int)EIndex.StoreCD].Text = "";
                ScStore.LabelText = "";
                ScStore.BtnSearch.Enabled = false;
                lblGridSkuCD.Text = "ITEMCD";
                lblGridJanCD.Text = "";
                //単価設定CDも入力？？
                detailControls[(int)EIndex.TankaCD].Enabled = false;
                detailControls[(int)EIndex.TankaCD].Text = "";
                ScTanka.LabelText = "";
                ScTanka.BtnSearch.Enabled = false;
            }
            else
            {
                lblSkuCD.Text = " SKUCD";
                keyControls[(int)EIndex.StoreCD].Enabled = true;
                keyControls[(int)EIndex.StoreCD].Text = "0000";
                CheckKey((int)EIndex.StoreCD);
                ScStore.BtnSearch.Enabled = true;
                lblGridSkuCD.Text = "SKUCD";
                lblGridJanCD.Text = "JANCD";
                //単価設定CDも入力？？
                detailControls[(int)EIndex.TankaCD].Enabled = true;
                detailControls[(int)EIndex.TankaCD].Text = "0000000000000";
                CheckDetail((int)EIndex.TankaCD);
                ScTanka.BtnSearch.Enabled = true;
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
                    //ITEM
                    if (sender.Equals(radioButton1) || sender.Equals(radioButton2))
                    {
                        if (radioButton3.Checked)
                            radioButton3.Focus();
                        else if (radioButton4.Checked)
                            radioButton4.Focus();

                    }
                    else
                    {
                        if (keyControls[(int)EIndex.StoreCD].Enabled)
                            keyControls[(int)EIndex.StoreCD].Focus();
                        else
                            keyControls[(int)EIndex.StoreCD + 1].Focus();
                    }

                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }
        #endregion


        private void BtnCalcZero_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //セルの金額が0円か空白の場合に定価×掛率の結果を反映
                this.CalcMoney(0);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnCalcAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //セルの金額に関わらず全ての金額に定価×掛率の結果を反映
                this.CalcMoney(1);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 金額に定価×掛率の結果を反映
        /// </summary>
        /// <param name="allKbn">全項目計算のときは1</param>
        private void CalcMoney(int allKbn)
        {
            //if(detailControls[(int)EIndex.TankaCD].Enabled)
            //    //単価設定CDの端数処理区分を使用
            //    if (CheckDetail((int)EIndex.TankaCD) == false)
            //        return;

            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ITemCD) == false)
                {
                    int teikaWithoutTax = Convert.ToInt32(bbl.Z_Set(mGrid.g_DArray[RW].PriceWithoutTax));
                    int taxRateFlg = mGrid.g_DArray[RW].TaxRateFLG;
                    decimal tax = 0;

                    decimal result = GetResultWithHasuKbn(mTankaCDRoundKBN, teikaWithoutTax * Convert.ToDecimal(detailControls[(int)EIndex.GeneralRate].Text) / 100);
                    decimal zeikomi = bbl.GetZeikomiKingaku(result, mGrid.g_DArray[RW].TaxRateFLG, out tax);
                    // 消費税計算with単価設定CDの端数処理区分(mTankaCDRoundKBN)


                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].GeneralPriceWithTax) == 0)
                        mGrid.g_DArray[RW].GeneralPriceWithTax = (result + tax).ToString("#,##0");

                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].GeneralPriceOutTax) == 0)
                        mGrid.g_DArray[RW].GeneralPriceOutTax = result.ToString("#,##0");   // 

                    result = GetResultWithHasuKbn(mTankaCDRoundKBN, teikaWithoutTax * Convert.ToDecimal(detailControls[(int)EIndex.MemberRate].Text) / 100);
                    zeikomi = bbl.GetZeikomiKingaku(result, taxRateFlg, out tax);

                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].MemberPriceWithTax) == 0)
                        mGrid.g_DArray[RW].MemberPriceWithTax = (result + tax).ToString("#,##0");
                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].MemberPriceOutTax) == 0)
                        mGrid.g_DArray[RW].MemberPriceOutTax = result.ToString("#,##0");

                    result = GetResultWithHasuKbn(mTankaCDRoundKBN, teikaWithoutTax * Convert.ToDecimal(detailControls[(int)EIndex.ClientRate].Text) / 100);
                    zeikomi = bbl.GetZeikomiKingaku(result, taxRateFlg, out tax);

                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].ClientPriceWithTax) == 0)
                        mGrid.g_DArray[RW].ClientPriceWithTax = (result + tax).ToString("#,##0");
                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].ClientPriceOutTax) == 0)
                        mGrid.g_DArray[RW].ClientPriceOutTax = result.ToString("#,##0");

                    result = GetResultWithHasuKbn(mTankaCDRoundKBN, teikaWithoutTax * Convert.ToDecimal(detailControls[(int)EIndex.SaleRate].Text) / 100);
                    zeikomi = bbl.GetZeikomiKingaku(result, taxRateFlg, out tax);

                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].SalePriceWithTax) == 0)
                        mGrid.g_DArray[RW].SalePriceWithTax = (result + tax).ToString("#,##0");
                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].SalePriceOutTax) == 0)
                        mGrid.g_DArray[RW].SalePriceOutTax = result.ToString("#,##0");

                    result = GetResultWithHasuKbn(mTankaCDRoundKBN, teikaWithoutTax * Convert.ToDecimal(detailControls[(int)EIndex.WebRate].Text) / 100);
                    zeikomi = bbl.GetZeikomiKingaku(result, taxRateFlg, out tax);

                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].WebPriceWithTax) == 0)
                        mGrid.g_DArray[RW].WebPriceWithTax = (result + tax).ToString("#,##0");
                    if (allKbn == 1 || bbl.Z_Set(mGrid.g_DArray[RW].WebPriceOutTax) == 0)
                        mGrid.g_DArray[RW].WebPriceOutTax = result.ToString("#,##0");

                }
            }

            //配列の内容を画面にセット
            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

        }

    }
}








