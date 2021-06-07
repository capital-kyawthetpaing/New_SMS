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

namespace PickingNyuuryoku
{
    /// <summary>
    /// PickingNyuuryoku ピッキング結果入力
    /// </summary>
    internal partial class PickingNyuuryoku : FrmMainForm
    {
        private const string ProID = "PickingNyuuryoku";
        private const string ProNm = "ピッキング結果入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            PickingNO,
            ShippingPlanDateFrom,
            ShippingPlanDateTo,
            JuchuuNO,

            SKUCD,
            JanCD,

            PickingDate = 0
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
        
        private PickingNyuuryoku_BL snbl;
        private D_Picking_Entity dpe;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOlPickingNO = "";    //排他処理のため使用
        private string mOldPickingDate = "";

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridPicking mGrid = new ClsGridPicking();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxPurchaseGyoNo;

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

            if (ClsGridPicking.gc_P_GYO <= ClsGridPicking.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridPicking.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridPicking.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridPicking.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridPicking.gc_MaxCL;

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
                            check.Click += new System.EventHandler(CHK_Del_Click);
                        }
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridPicking.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridPicking.ColNO.GYONO; i <= (int)ClsGridPicking.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridPicking.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridPicking.ColNO.ShippingPossibleSu:
                            mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
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
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 0].CellCtl = IMT_PAYDT_0;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 0].CellCtl = IMT_NYKNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 0].CellCtl = CHK_EDICK_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 1].CellCtl = IMT_REMAK_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 1].CellCtl = IMT_PAYDT_1;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 1].CellCtl = IMT_NYKNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 1].CellCtl = CHK_EDICK_1;
            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 2].CellCtl = IMT_PAYDT_2;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 2].CellCtl = IMT_NYKNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 2].CellCtl = CHK_EDICK_2;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 3].CellCtl = IMT_PAYDT_3;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 3].CellCtl = IMT_NYKNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 3].CellCtl = CHK_EDICK_3;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 4].CellCtl = IMT_PAYDT_4;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 4].CellCtl = IMT_NYKNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 4].CellCtl = CHK_EDICK_4;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 5].CellCtl = IMT_PAYDT_5;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 5].CellCtl = IMT_NYKNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 5].CellCtl = CHK_EDICK_5;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 6].CellCtl = CHK_DELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 6].CellCtl = CHK_DELCK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 6].CellCtl = IMT_KAIDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 6].CellCtl = IMN_TEIKA2_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 6].CellCtl = IMN_CLINT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 6].CellCtl = IMN_WEBPR_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 6].CellCtl = IMT_REMAK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 6].CellCtl = IMT_JUONO_6;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 6].CellCtl = IMT_ARIDT_6;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 6].CellCtl = IMT_PAYDT_6;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 6].CellCtl = IMT_NYKNO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 6].CellCtl = CHK_EDICK_6;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 7].CellCtl = CHK_DELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 7].CellCtl = CHK_DELCK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 7].CellCtl = IMT_KAIDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 7].CellCtl = IMN_TEIKA2_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 7].CellCtl = IMN_CLINT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 7].CellCtl = IMN_WEBPR_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 7].CellCtl = IMT_REMAK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 7].CellCtl = IMT_JUONO_7;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 7].CellCtl = IMT_ARIDT_7;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 7].CellCtl = IMT_PAYDT_7;    //
            
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 7].CellCtl = IMT_NYKNO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 7].CellCtl = CHK_EDICK_7;

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 8].CellCtl = CHK_DELCK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 8].CellCtl = CHK_DELCK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 8].CellCtl = IMT_KAIDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 8].CellCtl = IMN_TEIKA2_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 8].CellCtl = IMN_CLINT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 8].CellCtl = IMN_WEBPR_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 8].CellCtl = IMT_REMAK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 8].CellCtl = IMT_JUONO_8;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 8].CellCtl = IMT_ARIDT_8;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 8].CellCtl = IMT_PAYDT_8;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 8].CellCtl = IMT_NYKNO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 8].CellCtl = CHK_EDICK_8;

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 9].CellCtl = CHK_DELCK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Chk, 9].CellCtl = CHK_DELCK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.JanCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SizeName, 9].CellCtl = IMT_KAIDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPossibleSu, 9].CellCtl = IMN_TEIKA2_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ColorName, 9].CellCtl = IMN_CLINT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space2, 9].CellCtl = IMN_WEBPR_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.DeliveryName, 9].CellCtl = IMT_REMAK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.SKUCD, 9].CellCtl = IMT_JUONO_9;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ShippingPlanDate, 9].CellCtl = IMT_ARIDT_9;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.Space, 9].CellCtl = IMT_PAYDT_9;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ArrivalNO, 9].CellCtl = IMT_NYKNO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridPicking.ColNO.ChkModori, 9].CellCtl = CHK_EDICK_9;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridPicking.Gen_MK_FocusMove.MvNxt, (int)ClsGridPicking.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridPicking.Gen_MK_FocusMove.MvPrv, (int)ClsGridPicking.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridPicking.Gen_MK_FocusMove.MvNxt, (int)ClsGridPicking.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridPicking.Gen_MK_FocusMove.MvSet, (int)ClsGridPicking.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridPicking.Gen_MK_FocusMove.MvSet, (int)ClsGridPicking.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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

                    //// クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case (int)ClsGridShiire.ColNO.Chk:
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
        // 1...  (画面展開後)
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
                        }
                        else
                        {
                            IMT_DMY_0.Focus();
                            
                            Scr_Lock(0, 0, 0);  // フレームのロック解除
                            Scr_Lock(1, mc_L_END, 1);  // フレームのロック
                            btnSubF11.Enabled = true;

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

                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].PickingNO))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridPicking.ColNO.Chk:
                                            //D_Picking.PickingKBN＝1の時、入力可能				
                                            if (mGrid.g_DArray[w_Row].PickingKBN == 1)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            }
                                            break;
                                        case (int)ClsGridPicking.ColNO.ChkModori:    // 
                                            //D_Picking.PickingKBN＝2の時、入力可能				
                                            if (mGrid.g_DArray[w_Row].PickingKBN == 2)
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            }
                                                break;
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
                            btnSubF11.Enabled = false;
                            SetFuncKeyAll(this, "100001000001");
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

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);
        }

        #endregion

        public PickingNyuuryoku()
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
                this.Scr_Clr(0);

                //起動時共通処理
                base.StartProgram();

                ModeVisible = false;
                Btn_F2.Text = "";
                Btn_F3.Text = "";
                Btn_F3.Text = "";
                Btn_F4.Text = "";
                Btn_F5.Text = "";
                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "";
                Btn_F11.Text = "表示(F11)";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                snbl = new PickingNyuuryoku_BL();

                //検索用のパラメータ設定
                string stores= GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScJuchuNO.Value1 = InOperatorCD;
                ScJuchuNO.Value2 = stores;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = snbl.GetDate()
                };

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
            keyControls = new Control[] {  ScOrderNO.TxtCode,ckM_TextBox4, ckM_TextBox3,ScJuchuNO.TxtCode, ckM_TextBox6,  ckM_TextBox7  };
            keyLabels = new Control[] { ScJuchuNO };
            detailControls = new Control[] {  ckM_TextBox1 };
            detailLabels = new Control[] {  };
            searchButtons = new Control[] { ScOrderNO.BtnSearch,ScJuchuNO.BtnSearch, btnSearchSKUCD, btnSearchJANCD };

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

            btnSearchSKUCD.Click += new System.EventHandler(BtnSearch_Click);
            btnSearchJANCD.Click += new System.EventHandler(BtnSearch_Click);
        }
        
        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set=true)
        {
            if (keyControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)keyControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.PickingNO:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //入力無しの時
                        //出荷予定日～JANCD 入力可
                        Scr_Lock(0, 0, 0);
                        return true;
                    }
                    else
                    {
                        //Errorでない場合、画面転送表01に従って画面表示
                        //	出荷予定日～JANCD入力不可	ピッキング日へ	
                        for (int i = (int)EIndex.ShippingPlanDateFrom; i <= (int)EIndex.JanCD; i++)
                        {
                            keyControls[i].Enabled = false;
                        }
                        ScJuchuNO.BtnSearch.Enabled = false;
                        btnSearchJANCD.Enabled = false;
                        btnSearchSKUCD.Enabled = false;

                    }

                    if (set)
                        return CheckData(set);
                    break;

                case (int)EIndex.JuchuuNO:
                    //入力された場合
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //受注(D_Juchuu)の存在しない場合エラー
                        D_Juchuu_Entity dje = new D_Juchuu_Entity();
                        dje.JuchuuNO = keyControls[index].Text;

                        TempoJuchuuNyuuryoku_BL mibl = new TempoJuchuuNyuuryoku_BL();
                        DataTable dt = mibl.D_Juchu_SelectData(dje, (short)OperationMode);

                        //受注(D_Juchuu)に存在しない場合、Error 「登録されていない受注番号」
                        if (dt.Rows.Count == 0)
                        {
                            bbl.ShowMessage("E138", "受注番号");
                            return false;
                        }
                        else
                        {
                            //DeleteDateTime 「削除された受注番号」
                            if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                            {
                                bbl.ShowMessage("E140", "受注番号");
                                return false;
                            }
                        }
                    }
                        break;

                case (int)EIndex.ShippingPlanDateFrom:
                case (int)EIndex.ShippingPlanDateTo:
                    //出荷予定日(To) 入力必須(Entry required)
                    if(index == (int)EIndex.ShippingPlanDateTo && keyControls[index].Enabled)
                    {
                        if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        {
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return true;

                    keyControls[index].Text = bbl.FormatDate(keyControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(keyControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.ShippingPlanDateTo && !string.IsNullOrWhiteSpace(keyControls[index - 1].Text) && !string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        int result = keyControls[index].Text.CompareTo(keyControls[index - 1].Text);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E104");
                            return false;
                        }
                    }

                    break;
            }

            return true;

        }
        
        /// <summary>
        /// ピッキングデータ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.PickingNO)
        {
            //[D_Picking_SelectData]
            dpe = GetSearchInfo();

            DataTable dt = snbl.D_Picking_SelectData(dpe, (short)OperationMode);

            //ピッキング(D_Picking)に存在しない場合、Error 「登録されていないピッキング番号」
            if (dt.Rows.Count == 0)
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
                m_MaxPurchaseGyoNo = 0;

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
                        //明細にデータをセット
                        detailControls[(int)EIndex.PickingDate].Text = bbl.GetDate();
                        mOldPickingDate = detailControls[(int)EIndex.PickingDate].Text;
                    }

                    mGrid.g_DArray[i].ShippingPlanDate = row["ShippingPlanDate"].ToString();
                    mGrid.g_DArray[i].ArrivalNO = row["Number"].ToString();
                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();   
                    
                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                    mGrid.g_DArray[i].ShippingPossibleSu = bbl.Z_SetStr(row["ShippingPossibleSu"]);   // 

                    mGrid.g_DArray[i].DeliveryName = row["DeliveryName"].ToString();   // 

                    mGrid.g_DArray[i].Chk = false;
                    mGrid.g_DArray[i].ChkModori = false;
                    mGrid.g_DArray[i].OldChk = false;
                    mGrid.g_DArray[i].OldChkModori = false;

                    if (!string.IsNullOrWhiteSpace(row["PickingDate"].ToString()))
                    {
                        if (row["PickingKBN"].ToString() == "1")
                        {         
                            //D_Picking.PickingKBN＝1かつD_PickingDetails.PickingDate≠NULLの時、CheckBox＝ON
                            mGrid.g_DArray[i].Chk = true;
                            mGrid.g_DArray[i].OldChk = true;
                        }
                        else if (row["PickingKBN"].ToString() == "2")
                        {
                            //D_Picking.PickingKBN＝2かつD_PickingDetails.PickingDate≠NULLの時、CheckBox＝ON
                            mGrid.g_DArray[i].ChkModori = true;
                            mGrid.g_DArray[i].OldChkModori = true;
                        }
                    }

                    //税額(Hidden)
                    mGrid.g_DArray[i].PickingNO = row["PickingNO"].ToString();
                    mGrid.g_DArray[i].PickingRows = Convert.ToInt16(row["PickingRows"]);
                    mGrid.g_DArray[i].PickingKBN = Convert.ToInt16(row["PickingKBN"]);
                    mGrid.g_DArray[i].ReserveNO = row["ReserveNO"].ToString();

                    if (m_MaxPurchaseGyoNo < mGrid.g_DArray[i].PickingRows)
                        m_MaxPurchaseGyoNo = mGrid.g_DArray[i].PickingRows;

                    m_dataCnt = i + 1;
                    i++;
                }

                mOldPickingDate = detailControls[(int)EIndex.PickingDate].Text;

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            S_BodySeigyo(1, 0);
            S_BodySeigyo(1, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            detailControls[0].Focus();

            return true;
        }
        protected override void ExecDisp()
        {
            //抽出条件エリアのエラーチェック
            for (int i = (int)EIndex.PickingNO; i <= (int)EIndex.JanCD; i++)
                if (CheckKey(i) == false)
                {
                    keyControls[i].Focus();
                    return;
                }


            bool ret = CheckData(true);                
        }
        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {       

            switch (index)
            {
                case (int)EIndex.PickingDate:
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
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }

                    break;
            }

            return true;
        }        

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)
        {
            string ymd = detailControls[(int)EIndex.PickingDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            //switch (index)
            //{
            //    case (int)EIndex.JuchuuNO:
            //        //[M_Vendor_Select]
            //        M_Vendor_Entity mve = new M_Vendor_Entity
            //        {
            //            VendorCD = detailControls[index].Text,
            //            ChangeDate = ymd
            //        };
            //        if (index == (int)EIndex.VendorCD)
            //        {
            //            mve.VendorFlg = "1";
            //        }
            //        Vendor_BL sbl = new Vendor_BL();
            //        ret = sbl.M_Vendor_SelectTop1(mve);

            //        if (ret)
            //        {
            //            if (mve.DeleteFlg == "1")
            //            {
            //                bbl.ShowMessage("E119");
            //                //顧客情報ALLクリア
            //                ClearCustomerInfo((EIndex)index);
            //                return false;
            //            }
            //            if (index == (int)EIndex.JuchuuNO)
            //            {
            //                ScCalledVendorCD.LabelText = mve.VendorName;
            //            }
            //            else
            //            {
            //                ScVendorCD.LabelText = mve.VendorName;
            //                mPayeeCD = mve.PayeeCD;
            //                mTaxTiming = mve.TaxTiming;
            //                //mAmountFractionKBN = Convert.ToInt16(mve.AmountFractionKBN);
            //                //mTaxFractionKBN = Convert.ToInt16(mve.TaxFractionKBN);

            //                //支払先CD(Hidden)が、仕入先マスター(M_Vendor)に存在することSelectできなければError
            //                mve.VendorCD = mPayeeCD;
            //                ret = sbl.M_Vendor_SelectForPayeeCD(mve);

            //                if (ret)
            //                {
            //                    //(1:明細ごと 2:伝票ごと 3:締ごと)
            //                    if (mve.TaxTiming == "1")
            //                    {
            //                        lblZei.Text = "明細単位";
            //                    }
            //                    else if (mve.TaxTiming == "2")
            //                    {
            //                        lblZei.Text = "伝票単位";
            //                    }
            //                    else if (mve.TaxTiming == "3")
            //                    {
            //                        lblZei.Text = "締単位";
            //                    }
            //                    else
            //                    {
            //                        lblZei.Text = "";
            //                    }
            //                    //支払予定日←Fnc_PlanDateよりout予定日をSet
            //                    if (detailControls[index].Text != mOldVendorCD || ChangeDate)
            //                    {
            //                        Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
            //                        fpe.KaisyuShiharaiKbn = "1";    // "1";1：支払		
            //                        fpe.CustomerCD = mPayeeCD;    //支払先CD(Hidden)
            //                        fpe.ChangeDate = ymd;
            //                        fpe.TyohaKbn = "0";

            //                        detailControls[(int)EIndex.PickingDate].Text = bbl.Fnc_PlanDate(fpe);

            //                        mOldVendorCD = detailControls[index].Text;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            bbl.ShowMessage("E101");
            //            //顧客情報ALLクリア
            //            ClearCustomerInfo((EIndex)index);
            //            return false;
            //        }
            //        break;
            //}

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll=false, bool changeYmd=false)
        {

          
            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

      
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Picking_Entity GetEntity()
        {
            dpe = new D_Picking_Entity
            {
                PickingDate = detailControls[(int)EIndex.PickingDate].Text,
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dpe;
        }
        private D_Picking_Entity GetSearchInfo()
        {
            dpe = new D_Picking_Entity
            {
                PickingNO = keyControls[(int)EIndex.PickingNO].Text,
                ShippingPlanDateFrom = keyControls[(int)EIndex.ShippingPlanDateFrom].Text,
                ShippingPlanDateTo = keyControls[(int)EIndex.ShippingPlanDateTo].Text,
                JuchuuNO = keyControls[(int)EIndex.JuchuuNO].Text,
                JanCD = keyControls[(int)EIndex.JanCD].Text,
                SKUCD = keyControls[(int)EIndex.SKUCD].Text,
            };
            return dpe;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("PickingNO", typeof(string));
            dt.Columns.Add("PickingRows", typeof(int));
            dt.Columns.Add("Chk", typeof(int));
            dt.Columns.Add("ChkModori", typeof(int));
            dt.Columns.Add("ReserveNO", typeof(string));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            if(OperationMode== EOperationMode.UPDATE)
            {
                rowNo = m_MaxPurchaseGyoNo + 1;
            }

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].PickingNO))
                    break;

                short updFlg = 0;
                //変更がある場合1
                if(!mGrid.g_DArray[RW].Chk.Equals(mGrid.g_DArray[RW].OldChk)) 
                {
                    updFlg = 2;
                }
                else if (!mGrid.g_DArray[RW].ChkModori.Equals(mGrid.g_DArray[RW].OldChkModori))
                {
                    updFlg = 1;
                }
                    short Chk = 0;
                short ChkModori = 0;
                if (mGrid.g_DArray[RW].Chk)
                {
                    Chk = 1;
                }
                if (mGrid.g_DArray[RW].ChkModori)
                    ChkModori = 1;

                dt.Rows.Add(mGrid.g_DArray[RW].PickingNO
                    , bbl.Z_Set(mGrid.g_DArray[RW].PickingRows)
                    , Chk
                    , ChkModori
                    , mGrid.g_DArray[RW].ReserveNO
                    , updFlg
                    );

                if(mGrid.g_DArray[RW].PickingRows == 0)
                    rowNo++;
            }

            return dt;
        }
        protected override void ExecSec()
        {
            for (int i = (int)EIndex.PickingDate; i <= (int)EIndex.PickingDate; i++)
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);            

            DataTable dt = GetGridEntity();

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                return;
            }

            //更新処理
            dpe = GetEntity();
            snbl.Picking_Exec(dpe,dt, (short)OperationMode);

            //ログファイルへの更新
            bbl.L_Log_Insert(Get_L_Log_Entity());

                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            //画面指定項目をカンマ編集で羅列（ex."2019/07/01,2019/7/31,ABCDEFG,未出力"）
            string item = keyControls[0].Text;
            for (int i = 1; i <= (int)EIndex.JanCD; i++)
            {
                item += "," + keyControls[i].Text;
            }

            L_Log_Entity lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "変更",
                KeyItem = item
            };

            return lle;
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            keyControls[0].Focus();

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
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldPickingDate = "";
            S_Clear_Grid();   //画面クリア（明細部）

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
                        break;
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        ChangeOperationMode(base.OperationMode);

                        break;
                    }

                case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;
                    if (Array.IndexOf(keyControls, PreviousCtrl) == (int)EIndex.SKUCD)
                    {
                        //商品検索
                        kbn = EsearchKbn.Product;
                    }
                    else if (Array.IndexOf(keyControls, PreviousCtrl) == (int)EIndex.JanCD)
                    {
                        //商品検索
                        kbn = EsearchKbn.Product;
                    }

                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

                    break;
                case 9://(F10)
                    ExecDisp();
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
                        if (keyControls.Length - 1 > index)
                        {
                            if (keyControls[index + 1].CanFocus)
                                keyControls[index + 1].Focus();
                            else if (!btnSubF11.Enabled)
                            {

                            }
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                        }
                        else 
                        {
                            btnSubF11.Focus();
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
                        if (index == (int)EIndex.PickingDate)
                            //明細の先頭項目へ
                            mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridPicking.ColNO.Chk);

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

                int index = Array.IndexOf(keyControls, sender);
                switch (index)
                {
                    case (int)EIndex.SKUCD:
                    case (int)EIndex.JanCD:
                        F9Visible = true;
                        break;

                    default:
                        //F9Visible = false;
                        break;
                }
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

                    if (CL == (int)ClsGridPicking.ColNO.ChkModori)
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
                    setCtl = keyControls[(int)EIndex.SKUCD];
                }
                else if (((Control)sender).Name.Equals(btnSearchJANCD.Name))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                    setCtl = keyControls[(int)EIndex.JanCD];
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
        /// 明細部チェックボックスクリック時処理
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
               
                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

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

        #endregion

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
                    string ymd = snbl.GetDate();
                    int index = Array.IndexOf(keyControls, setCtl);

                    using (Search_Product frmProduct = new Search_Product(ymd))
                    {
                        if (index.Equals((int)EIndex.JanCD))
                            frmProduct.Mode = "5";

                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {

                            switch (index)
                            {
                                case (int)EIndex.JanCD:
                                    if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.JanCD].Text))
                                        keyControls[(int)EIndex.JanCD].Text = frmProduct.JANCD;
                                    else
                                        keyControls[(int)EIndex.JanCD].Text = keyControls[(int)EIndex.JanCD].Text + "," + frmProduct.JANCD;

                                    break;

                                case (int)EIndex.SKUCD:
                                    if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.SKUCD].Text))
                                        keyControls[(int)EIndex.SKUCD].Text = frmProduct.SKUCD;
                                    else
                                        keyControls[(int)EIndex.SKUCD].Text = keyControls[(int)EIndex.SKUCD].Text + "," + frmProduct.SKUCD;

                                    break;
                            }
                            setCtl.Focus();

                            //SendKeys.Send("{ENTER}");
                        }

                    }
                    break;
            }

        }

        private void ChangeBackColor(int w_Row)
        {
            Color backCL = GridBase.ClsGridBase.GrayColor;

            for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridPicking.ColNO.Chk:
                    case (int)ClsGridPicking.ColNO.ChkModori:
                        break;
                    case (int)ClsGridPicking.ColNO.GYONO:
                    case (int)ClsGridPicking.ColNO.Space:
                    case (int)ClsGridPicking.ColNO.Space2:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            break;
                        }
                    default:
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                        break;
                }
            }
        }

        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                //明細チェックボックスONかつ今回入金額に未入金額をセット。
                ChangeCheck(true);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Btn_NoSelect_Click(object sender, EventArgs e)
        {
            try
            {
                //明細チェックボックスOFFかつ今回入金額にゼロをセット。
                ChangeCheck(false);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void ChangeCheck(bool check)
        {
            // 明細部  画面の範囲の内容を配列にセット
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //明細部チェック
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].PickingNO) == false)
                {
                    mGrid.g_DArray[RW].Chk = check;
                }
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
    }
}








