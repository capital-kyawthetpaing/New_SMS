
using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZaikoIdouIraiNyuuryoku
{
    /// <summary>
    /// ZaikoIdouIraiNyuuryoku 在庫移動依頼入力
    /// </summary>
    internal partial class ZaikoIdouIraiNyuuryoku : FrmMainForm
    {
        private const string ProID = "ZaikoIdouIraiNyuuryoku";
        private const string ProNm = "在庫移動依頼入力";
        private const short mc_L_END = 3; // ロック用

        private ListBox DeleteRowNo = new ListBox();
        private EOperationMode mDetailOperationMode = EOperationMode.INSERT;
        private EIdoType mIdoType = EIdoType.NULL;
        private enum EIdoType : int
        {
            NULL,
            店舗間移動,
        }

        private enum EIndex : int
        {
            RequestNO,
            CopyRequestNO,
            StoreCD,

            RequestDate = 0,
            MovePurposeKBN,
            StaffCD,
            FromSoukoCD,
            ToSoukoCD,

            Gyono,    //行番号
            JANCD,
            Suryo,//数量
            ExpectedDate,    //移動希望日付
            RemarksInStore,     //コメント
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

        private ZaikoIdouIraiNyuuryoku_BL zibl;
        private D_MoveRequest_Entity dme;
        private M_MovePurpose_Entity mme;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldRequestNO = "";    //排他処理のため使用
        private string mOldDate = "";
        private string mFromStoreCD = "";
        private string mToStoreCD = "";
        private string InStoreCD = "";      //初期店舗CD

        private string mAdminNO = "";
        private string mJANCD = "";


        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridIdoIrai mGrid = new ClsGridIdoIrai();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxMoveGyoNo;

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

            if (ClsGridIdoIrai.gc_P_GYO <= ClsGridIdoIrai.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridIdoIrai.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridIdoIrai.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridIdoIrai.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridIdoIrai.gc_MaxCL;

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

            //for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            //{
            //    for (int w_CtlCol = 0; w_CtlCol <= mGrid.g_MK_Ctl_Col - 1; w_CtlCol++)
            //    {
            //        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl != null)
            //        {
            //            if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
            //            {
            //                Search.CKM_SearchControl sctl = (Search.CKM_SearchControl)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
            //                sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
            //                sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
            //                sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
            //                sctl.TxtCode.Tag = W_CtlRow.ToString();
            //                //sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
            //            }
            //            else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            //            {
            //                mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
            //                mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
            //                mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
            //            }
            //            else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
            //            {
            //                CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
            //                sctl.Enter += new System.EventHandler(GridControl_Enter);
            //                sctl.Leave += new System.EventHandler(GridControl_Leave);
            //                sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
            //                sctl.Tag = W_CtlRow.ToString();
            //            }
            //            else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
            //            {
            //                GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
            //                check.Enter += new System.EventHandler(GridControl_Enter);
            //                check.Leave += new System.EventHandler(GridControl_Leave);
            //                check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
            //                check.Click += new System.EventHandler(CHK_Del_Click);
            //            }
            //        }
            //    }
            //}

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridIdoIrai.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridIdoIrai.ColNO.GYONO; i <= (int)ClsGridIdoIrai.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridIdoIrai.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridIdoIrai.ColNO.RequestSu:
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
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 0].CellCtl = CHK_DELCK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 0].CellCtl = LBL_SPACE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 0].CellCtl = IMT_KAIDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 0].CellCtl = IMN_TEIKA2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 0].CellCtl = IMN_MEMBR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 0].CellCtl = IMN_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 0].CellCtl = IMT_REMAK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 0].CellCtl = IMT_JUONO_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 1].CellCtl = LBL_SPACE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 1].CellCtl = IMT_KAIDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 1].CellCtl = IMN_TEIKA2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 1].CellCtl = IMN_MEMBR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 1].CellCtl = IMN_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 1].CellCtl = IMT_REMAK_1;

            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 1].CellCtl = IMT_JUONO_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 2].CellCtl = LBL_SPACE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 2].CellCtl = IMT_KAIDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 2].CellCtl = IMN_TEIKA2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 2].CellCtl = IMN_MEMBR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 2].CellCtl = IMN_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 2].CellCtl = IMT_REMAK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 2].CellCtl = IMT_JUONO_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 3].CellCtl = LBL_SPACE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 3].CellCtl = IMT_KAIDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 3].CellCtl = IMN_TEIKA2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 3].CellCtl = IMN_MEMBR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 3].CellCtl = IMN_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 3].CellCtl = IMT_REMAK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 3].CellCtl = IMT_JUONO_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 4].CellCtl = LBL_SPACE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 4].CellCtl = IMT_KAIDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 4].CellCtl = IMN_TEIKA2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 4].CellCtl = IMN_MEMBR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 4].CellCtl = IMN_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 4].CellCtl = IMT_REMAK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 4].CellCtl = IMT_JUONO_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space2, 5].CellCtl = LBL_SPACE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.JanCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SizeName, 5].CellCtl = IMT_KAIDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.RequestSu, 5].CellCtl = IMN_TEIKA2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.Space, 5].CellCtl = IMN_MEMBR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ColorName, 5].CellCtl = IMN_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentOutStore, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.CommentInStore, 5].CellCtl = IMT_REMAK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.SKUCD, 5].CellCtl = IMT_JUONO_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridIdoIrai.ColNO.ExpectedDate, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvNxt, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvPrv, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvNxt, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                keyControls[(int)EIndex.RequestNO].Text = "";
                                keyControls[(int)EIndex.RequestNO].Enabled = false;
                                ScOrderNO.BtnSearch.Enabled = false;
                                keyControls[(int)EIndex.CopyRequestNO].Enabled = true;
                                ScCopyOrderNO.BtnSearch.Enabled = true;

                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.RequestDate].Text = bbl.GetDate();
                                ScStaff.TxtCode.Text = InOperatorCD;
                                CheckDetail((int)EIndex.StaffCD);

                                BtnSubF2.Enabled = true;
                                BtnSubF3.Enabled = true;
                                BtnSubF4.Enabled = true;
                                btnSubF10.Enabled = true;

                                SetFuncKeyAll(this, "111111001111");
                            }
                            else
                            {
                                keyControls[(int)EIndex.RequestNO].Enabled = true;
                                ScOrderNO.BtnSearch.Enabled = true;
                                keyControls[(int)EIndex.CopyRequestNO].Text = "";
                                keyControls[(int)EIndex.CopyRequestNO].Enabled = false;
                                ScCopyOrderNO.BtnSearch.Enabled = false;
                                //keyControls[(int)EIndex.RequestNO].Text = "";
                                //keyControls[(int)EIndex.RequestNO].Enabled = false;
                                //ScRequestNO.BtnSearch.Enabled = false;

                                Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                BtnSubF2.Enabled = false;
                                BtnSubF3.Enabled = false;
                                BtnSubF4.Enabled = false;
                                btnSubF10.Enabled = false;

                                SetFuncKeyAll(this, "111111001000");
                            }
                        }
                        CboStoreCD.SelectedValue = InStoreCD;

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
                                        case (int)ClsGridIdoIrai.ColNO.Space2:    // 
                                            if (OperationMode == EOperationMode.INSERT)
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
                            SetFuncKeyAll(this, "111111000101");
                            btnSubF10.Enabled = true;
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

        //private void Grid_NotFocus(int pCol, int pRow)
        //{
        //    // ﾌｫｰｶｽをはじく
        //    int w_Col;
        //    bool w_AllFlg = false;
        //    int w_CtlRow;

        //    if (OperationMode >= EOperationMode.DELETE)
        //        return;

        //    if (m_EnableCnt - 1 < pRow)
        //        return;

        //    w_CtlRow = pRow - Vsb_Mei_0.Value;
        //    if (w_CtlRow >= 0 && w_CtlRow <= mGrid.g_MK_Ctl_Row - 1)
        //        //画面範囲の時
        //        //配列の内容を画面にセット
        //        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);


        //    if (pCol == (int)ClsGridIdo.ColNO.Chk)
        //    {
        //        //チェック時、ReadOnlyの列以外 使用可
        //        for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
        //        {
        //            if (!mGrid.g_DArray[pRow].Chk)
        //            {
        //                for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
        //                {
        //                    if (OperationMode == EOperationMode.INSERT)
        //                    {
        //                        if (w_Col == (int)ClsGridIdo.ColNO.Chk)
        //                            //使用可
        //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
        //                        else
        //                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
        //                    }
        //                    else
        //                    {

        //                    }
        //                }

        //                w_AllFlg = false;
        //            }
        //            else
        //            {
        //                //Chk入力時
        //                w_AllFlg = true;
        //                if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JanCD))
        //                {

        //                }
        //            }
        //        }
        //        return;
        //    }
        //}

        #endregion

        public ZaikoIdouIraiNyuuryoku()
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

                Btn_F7.Text = "";
                Btn_F8.Text = "";
                Btn_F10.Text = "確定(F10)";
                Btn_F11.Text = "";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                zibl = new ZaikoIdouIraiNyuuryoku_BL();
                CboStoreCD.Bind(ymd, "2");
                CboIdoKbn.Bind(ymd);
                CboFromSoukoCD.Bind(ymd);
                CboToSoukoCD.Bind(ymd);

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScOrderNO.Value1 = InOperatorCD;
                ScOrderNO.Value2 = stores;
                ScCopyOrderNO.Value1 = InOperatorCD;
                ScCopyOrderNO.Value2 = stores;

                ScStaff.TxtCode.Text = InOperatorCD;

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = zibl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    CboStoreCD.SelectedValue = mse.StoreCD;
                    InStoreCD = mse.StoreCD;
                    ScStaff.LabelText = mse.StaffName;
                }

                InitScr();
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
            keyControls = new Control[] { ScOrderNO.TxtCode, ScCopyOrderNO.TxtCode, CboStoreCD };
            keyLabels = new Control[] { };
            detailControls = new Control[] { ckM_TextBox1, CboIdoKbn, ScStaff.TxtCode, CboFromSoukoCD, CboToSoukoCD
                         ,ckM_TextBox4, SC_ITEM_0.TxtCode, ckM_TextBox8,  ckM_TextBox2, TxtRemark1};
            detailLabels = new Control[] { ScStaff };
            searchButtons = new Control[] { ScStaff.BtnSearch };

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

            SC_ITEM_0.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
        }

        /// <summary>
        /// PrimaryKeyのコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckKey(int index, bool set = true)
        {

            switch (index)
            {
                case (int)EIndex.RequestNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //排他処理
                    bool ret = SelectAndInsertExclusive();
                    if (!ret)
                        return false;

                    return CheckData(set);

                case (int)EIndex.CopyRequestNO:
                    if (!string.IsNullOrWhiteSpace(keyControls[index].Text))
                        return CheckData(set, index);

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { keyControls[index] }))
                    {
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

            return true;

        }

        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;

            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(keyControls[(int)EIndex.RequestNO].Text))
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Zaikoido,
                Number = keyControls[(int)EIndex.RequestNO].Text,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                keyControls[(int)EIndex.RequestNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldRequestNO = keyControls[(int)EIndex.RequestNO].Text;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (mOldRequestNO == "")
                return;

            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Zaikoido,
                Number = mOldRequestNO,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldRequestNO = "";
        }

        /// <summary>
        /// 移動データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index = (int)EIndex.RequestNO)
        {
            //[D_Purchase_SelectDataF]
            dme = new D_MoveRequest_Entity();
            dme.RequestNO = keyControls[index].Text;

            DataTable dt;
            dt = zibl.D_MoveRequest_SelectDataForIdouIrai(dme);


            //移動(D_Purchase)に存在しない場合、Error 「登録されていない移動番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E138", "移動依頼番号");
                Scr_Clr(1);
                previousCtrl.Focus();
                return false;
            }
            else
            {
                //DeleteDateTime 「削除された移動番号」
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                {
                    bbl.ShowMessage("E140", "移動依頼番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                //権限がない場合（以下のSelectができない場合）Error　「権限のない移動番号」
                if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                {
                    bbl.ShowMessage("E139", "移動番号");
                    Scr_Clr(1);
                    previousCtrl.Focus();
                    return false;
                }

                if (index == (int)EIndex.RequestNO)
                {
                    //★特例処理★変更モード、削除モードで、移動依頼番号に対し回答済の場合
                    if (!string.IsNullOrWhiteSpace(dt.Rows[0]["AnswerDateTime"].ToString()))
                    {
                        OperationMode = EOperationMode.SHOW;
                        if (set == false)
                        {
                            bbl.ShowMessage("E192", "移動依頼番号");
                            set = true;
                        }
                        else
                        {
                            bbl.ShowMessage("E191", "移動依頼番号");
                        }
                    }
                }

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                if (index == (int)EIndex.CopyRequestNO)
                {
                    //Errorでない場合、画面転送表02に従って画面表示
                    mDetailOperationMode = EOperationMode.INSERT;
                }
                else
                {
                    //Errorでない場合、画面転送表01に従って画面表示

                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;
                m_dataCnt = 0;
                m_MaxMoveGyoNo = 0;

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

                        if (index == (int)EIndex.CopyRequestNO)
                        {
                            detailControls[(int)EIndex.RequestDate].Text = bbl.GetDate();
                        }
                        else
                        {
                            detailControls[(int)EIndex.RequestDate].Text = row["RequestDate"].ToString();
                        }

                        CboIdoKbn.SelectedValue = row["MovePurposeKBN"];
                        CheckDetail((int)EIndex.MovePurposeKBN);

                        detailControls[(int)EIndex.StaffCD].Text = row["StaffCD"].ToString();
                        CheckDetail((int)EIndex.StaffCD);

                        CboToSoukoCD.SelectedValue = row["ToSoukoCD"].ToString();
                        CheckDetail((int)EIndex.ToSoukoCD);
                        CboFromSoukoCD.SelectedValue = row["FromSoukoCD"].ToString();
                        CheckDetail((int)EIndex.FromSoukoCD);

                        //明細なしの場合
                        if (bbl.Z_Set(row["RequestRows"]) == 0)
                            break;
                    }

                    mGrid.g_DArray[i].JanCD = row["JanCD"].ToString();
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].OldRequestSu = Convert.ToInt16(row["RequestSu"]);

                    CheckGrid((int)ClsGridIdoIrai.ColNO.JanCD, i);

                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();   // 
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();   // 
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();   // 

                    mGrid.g_DArray[i].RequestSu = bbl.Z_SetStr(row["RequestSu"]);   // 
                    mGrid.g_DArray[i].ExpectedDate = row["ExpectedDate"].ToString();

                    mGrid.g_DArray[i].CommentInStore = row["CommentInStore"].ToString();   // 

                    if (index == (int)EIndex.RequestNO)
                    {
                        //(Hidden)
                        mGrid.g_DArray[i].RequestRows = Convert.ToInt16(row["RequestRows"]);

                        if (m_MaxMoveGyoNo < mGrid.g_DArray[i].RequestRows)
                            m_MaxMoveGyoNo = mGrid.g_DArray[i].RequestRows;
                    }
                    m_dataCnt = i + 1;
                    i++;
                }

                mOldDate = detailControls[(int)EIndex.RequestDate].Text;

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                S_BodySeigyo(0, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                detailControls[(int)EIndex.RequestDate].Focus();
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
            if (mDetailOperationMode == EOperationMode.DELETE)
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

            //在庫移動明細編集エリアのエラーチェック
            for (int i = (int)EIndex.Gyono; i <= (int)EIndex.RemarksInStore; i++)
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            //在庫移動明細編集エリア(Detail Area)が新規モードの場合
            SetDataToGrid();

        }
        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = true)
        {
            switch (index)
            {
                case (int)EIndex.RequestDate:
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
  
                    //移動日が変更された場合のチェック処理
                    if (mOldDate != detailControls[index].Text)
                    {
                        for (int i = (int)EIndex.FromSoukoCD; i < (int)EIndex.StaffCD; i++)
                            if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                if (!CheckDependsOnDate(i, true))
                                    return false;

                        mOldDate = detailControls[index].Text;
                    }

                    break;

                case (int)EIndex.MovePurposeKBN:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    mme = new M_MovePurpose_Entity();
                    mme.MovePurposeKBN = CboIdoKbn.SelectedValue.ToString();
                    mme.MoveRequestFLG = "1";

                    bool ret = zibl.M_MovePurpose_Select(mme);
                    mIdoType = (EIdoType)Convert.ToInt16(mme.MovePurposeType);                   
                    SetEnabled(mIdoType);
                    break;

                case (int)EIndex.StaffCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    if (!CheckDependsOnDate(index))
                        return false;

                    break;

                case (int)EIndex.FromSoukoCD:
                case (int)EIndex.ToSoukoCD:
                    //入力できる場合(When input is possible)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                        if (!CheckDependsOnDate(index))
                            return false;
                    }
                    break;

                //明細編集エリア項目****************************************************************************************************↓
                case (int)EIndex.Gyono:
                    //入力できる場合(When input is possible)
                    if (detailControls[index].Enabled)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                            return true;

                        //入力がある場合
                        //入力されている変更・削除行№が、画面.Detail Display Area.行№に無い場合 Ｅ１９０
                        for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                        {
                            if (mGrid.g_DArray[RW].GYONO.Equals(detailControls[index].Text))
                            {
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                                {
                                    bbl.ShowMessage("E190");
                                    return false;
                                }
                                break;
                            }
                        }

                        if (set)
                        {
                            //入力されている変更・削除行№が、画面.Detail Display Area.行№にある場合
                            //在庫移動明細表示エリア(Detail Display Area)の行№の画面項目を、在庫移動明細編集エリア(Deta Area Detail)にセットする。
                            SetDataFromGrid(Convert.ToInt16(detailControls[index].Text));
                        }
                    }
                    break;

                case (int)EIndex.JANCD:
                    //入力できる場合(When input is possible)
                    if (detailControls[index].Enabled)
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                        if (!detailControls[index].Text.Equals(mJANCD))
                        {
                            mAdminNO = "";
                        }

                        //入力がある場合、SKUマスターに存在すること
                        //[M_SKU]
                        M_SKU_Entity mse = new M_SKU_Entity
                        {
                            JanCD = detailControls[index].Text,
                            AdminNO = mAdminNO,
                            ChangeDate = detailControls[(int)EIndex.RequestDate].Text
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
                            //Ｅ１９３
                            bbl.ShowMessage("E193");

                            using (Select_SKU frmSKU = new Select_SKU())
                            {
                                frmSKU.parJANCD = dt.Rows[0]["JanCD"].ToString();
                                frmSKU.parChangeDate = detailControls[(int)EIndex.RequestDate].Text;
                                frmSKU.ShowDialog();

                                if (!frmSKU.flgCancel)
                                {
                                    selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                                }
                            }
                        }

                        if (selectRow != null)
                        {
                            //移動依頼明細表示エリア(Display Area Detail).SKUCDと重複する場合Error
                            //明細部チェック
                            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                            {
                                if (bbl.Z_Set(detailControls[(int)EIndex.Gyono].Text) == RW + 1)
                                    continue;

                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false)
                                {
                                    if (mGrid.g_DArray[RW].SKUCD.Equals(selectRow["SKUCD"].ToString()))
                                    {
                                        //Ｅ１９３
                                        bbl.ShowMessage("E193");

                                        using (Select_SKU frmSKU = new Select_SKU())
                                        {
                                            frmSKU.parJANCD = dt.Rows[0]["JanCD"].ToString();
                                            frmSKU.parChangeDate = detailControls[(int)EIndex.RequestDate].Text;
                                            frmSKU.ShowDialog();

                                            if (!frmSKU.flgCancel)
                                            {
                                                selectRow = dt.Select(" AdminNO = " + frmSKU.parAdminNO)[0];
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        RW = -1;
                                    }
                                }
                            }

                            //変更なしの場合は再セットしない
                            if (mAdminNO.Equals(selectRow["AdminNO"].ToString()) && set == false)
                                return true;

                            //JANCDでSKUCDが１つだけ存在する場合（If there is only one）
                            mAdminNO = selectRow["AdminNO"].ToString();
                            mJANCD = selectRow["JANCD"].ToString();
                            lblSKUCD.Text = selectRow["SKUCD"].ToString();
                            lblSKUName.Text = selectRow["SKUName"].ToString();
                            lblColorName.Text = selectRow["ColorName"].ToString();
                            lblSizeName.Text = selectRow["SizeName"].ToString();
                        }
                    }
                    break;

                case (int)EIndex.Suryo:
                    //入力できる場合(When input is possible)
                    if (detailControls[index].Enabled)
                    {
                        //入力必須(Entry required)
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.ExpectedDate:
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
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.RequestDate].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[(int)EIndex.RequestDate].Text);
                        if (result < 0)
                        {
                            //Ｅ１４３「依頼日より小さい値は入力できません」
                            bbl.ShowMessage("E143", "依頼日", "小さい");
                            return false;
                        }
                    }

                    break;

                case (int)EIndex.RemarksInStore:
                    //入力できる場合(When input is possible)
                    //入力無くても良い(It is not necessary to input)
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

            w_Ctrl = detailControls[(int)EIndex.RemarksInStore];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, (int)ClsGridIdoIrai.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        /// <summary>
        /// 日付が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnDate(int index, bool ChangeDate = false)
        {
            string ymd = detailControls[(int)EIndex.RequestDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (index)
            {
                case (int)EIndex.StaffCD:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    Staff_BL bl = new Staff_BL();
                    bool ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        detailControls[index].Focus();
                        return false;
                    }
                    break;

                case (int)EIndex.ToSoukoCD:
                case (int)EIndex.FromSoukoCD:
                    //[M_Souko_Select]
                    M_Souko_Entity msoe = new M_Souko_Entity();
                    msoe.ChangeDate = ymd;
                    if (index == (int)EIndex.ToSoukoCD)
                    {
                        msoe.SoukoCD = CboToSoukoCD.SelectedValue.ToString();
                    }
                    else
                    {
                        msoe.SoukoCD = CboFromSoukoCD.SelectedValue.ToString();
                    }

                    DataTable dt = zibl.M_Souko_SelectData(msoe);

                    if (dt.Rows.Count > 0)
                    {
                        ////Ｅ１４５ 「権限のない店舗の倉庫です。」
                        //if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        //{
                        //    bbl.ShowMessage("E141");
                        //    return false;
                        //}
                        if (index == (int)EIndex.FromSoukoCD)
                        {
                            mFromStoreCD = dt.Rows[0]["StoreCD"].ToString();
                        }
                        else
                        {
                            mToStoreCD = dt.Rows[0]["StoreCD"].ToString();

                            switch (mIdoType)
                            {
                                case EIdoType.店舗間移動:
                                    //重複不可時	重複時Error	
                                    if (CboFromSoukoCD.SelectedIndex > 0 && CboToSoukoCD.SelectedIndex > 0)
                                    {
                                        if (CboFromSoukoCD.SelectedValue.Equals(CboToSoukoCD.SelectedValue))
                                        {
                                            bbl.ShowMessage("E186");
                                            return false;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;
            }

            return true;
        }
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            //bool ret = false;

            string ymd = detailControls[(int)EIndex.RequestDate].Text;

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            switch (col)
            {
                case (int)ClsGridIdoIrai.ColNO.JanCD:
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].JanCD,
                        AdminNO = mGrid.g_DArray[row].AdminNO,
                        ChangeDate = ymd
                    };

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    if (dt.Rows.Count > 0)
                    {
                        //mGrid.g_DArray[row].DiscountKbn = Convert.ToInt16(dt.Rows[0]["DiscountKbn"].ToString());
                        //mGrid.g_DArray[row].TaxRateFLG = Convert.ToInt16(dt.Rows[0]["TaxRateFLG"].ToString());
                        //mGrid.g_DArray[row].VariousFLG = Convert.ToInt16(dt.Rows[0]["VariousFLG"].ToString());
                        //mGrid.g_DArray[row].ZaikoKBN = Convert.ToInt16(dt.Rows[0]["ZaikoKBN"].ToString());
                    }

                    break;
            }

            //switch (col)
            //{
            //    case (int)ClsGridIdo.ColNO.PurchaseSu:
            //    case (int)ClsGridIdo.ColNO.PurchaserUnitPrice: //単価 
            //    case (int)ClsGridIdo.ColNO.AdjustmentGaku: //調整額
            //        //入力された場合、以下を再計算
            //        //計算仕入額←	form.仕入数	×	form.仕入単価
            //        mGrid.g_DArray[row].CalculationGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].PurchaseSu) * bbl.Z_Set(mGrid.g_DArray[row].PurchaserUnitPrice));
            //        //計算仕入額＋調整額＝仕入額
            //        mGrid.g_DArray[row].PurchaseGaku = bbl.Z_SetStr(bbl.Z_Set(mGrid.g_DArray[row].CalculationGaku) + bbl.Z_Set(mGrid.g_DArray[row].AdjustmentGaku)); 
            //         //消費税額(Hidden)←Function_消費税計算.out金額１	
            //         decimal zei;
            //        decimal zeikomi = bbl.GetZeikomiKingaku(bbl.Z_Set(mGrid.g_DArray[row].CalculationGaku), mGrid.g_DArray[row].TaxRateFLG,out zei, ymd);
            //        mGrid.g_DArray[row].PurchaseTax = zei;
            //        mGrid.g_DArray[row].PurchaseGaku10 = 0;
            //        mGrid.g_DArray[row].PurchaseGaku8 = 0;

            //        //通常税率仕入額(Hidden)M_SKU.TaxRateFLG＝1	の時の仕入額
            //        if (mGrid.g_DArray[row].TaxRateFLG.Equals(1))
            //        {
            //            mGrid.g_DArray[row].PurchaseGaku10 =bbl.Z_Set( mGrid.g_DArray[row].CalculationGaku);
            //        }
            //        //軽減税率仕入額(Hidden)M_SKU.TaxRateFLG＝2	の時の仕入額
            //        else if (mGrid.g_DArray[row].TaxRateFLG.Equals(2))
            //        {
            //            mGrid.g_DArray[row].PurchaseGaku8 = bbl.Z_Set(mGrid.g_DArray[row].CalculationGaku);
            //        }

            //        //各金額項目の再計算必要
            //        if (chkAll == false)
            //            CalcKin();

            //            break;

            //}

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_MoveRequest_Entity GetEntity()
        {
            dme = new D_MoveRequest_Entity
            {
                RequestNO = keyControls[(int)EIndex.RequestNO].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                FromStoreCD = mFromStoreCD,
                FromSoukoCD = CboFromSoukoCD.SelectedValue.ToString(),
                ToStoreCD = mToStoreCD,
                ToSoukoCD = CboToSoukoCD.SelectedValue.ToString(),
                RequestDate = detailControls[(int)EIndex.RequestDate].Text,
                MovePurposeKBN = CboIdoKbn.SelectedValue.ToString(),
                //MovePurposeType = ((int)mIdoType).ToString(),
                StaffCD = detailControls[(int)EIndex.StaffCD].Text,
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dme;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("RequestRows", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("RequestSu", typeof(int));
            dt.Columns.Add("OldRequestSu", typeof(int));

            dt.Columns.Add("ExpectedDate", typeof(DateTime));
            dt.Columns.Add("CommentInStore", typeof(string));

            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            if (OperationMode == EOperationMode.UPDATE)
            {
                rowNo = m_MaxMoveGyoNo + 1;
            }

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                    break;

                dt.Rows.Add(mGrid.g_DArray[RW].RequestRows > 0 ? mGrid.g_DArray[RW].RequestRows : rowNo
                    , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                    , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                    , mGrid.g_DArray[RW].JanCD == "" ? null : mGrid.g_DArray[RW].JanCD
                    , bbl.Z_Set(mGrid.g_DArray[RW].RequestSu)
                    , bbl.Z_Set(mGrid.g_DArray[RW].OldRequestSu)
                    
                    , mGrid.g_DArray[RW].ExpectedDate == "" ? null : mGrid.g_DArray[RW].ExpectedDate
                    , mGrid.g_DArray[RW].CommentInStore == "" ? null : mGrid.g_DArray[RW].CommentInStore

                    , mGrid.g_DArray[RW].RequestRows > 0 ? 1 : 0
                    );

                if (mGrid.g_DArray[RW].RequestRows == 0)
                    rowNo++;
            }
            if (OperationMode == EOperationMode.UPDATE)
            {
                //削除した行
                foreach (D_MoveRequest_Entity delNo in DeleteRowNo.Items)
                {
                    dt.Rows.Add(delNo.RequestRows
                        , null
                        , 0
                        , null
                        , 0
                        , delNo.OldRequestSu
                        , null
                        , null
                        , 2
                        );
                }
            }
            return dt;
        }
        protected override void ExecSec()
        {

            if (OperationMode == EOperationMode.INSERT)
            {
                if (CheckKey((int)EIndex.StoreCD, false) == false)
                {
                    return;
                }
            }
            else
            {
                for (int i = 0; i < keyControls.Length; i++)
                    if (CheckKey(i, false) == false)
                    {
                        keyControls[i].Focus();
                        return;
                    }

                if(OperationMode == EOperationMode.SHOW)
                {
                    return;
                }
            }

            if (OperationMode != EOperationMode.DELETE)
            {
                for (int i = 0; i <= (int)EIndex.ToSoukoCD; i++)
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }              
            }

            DataTable dt = GetGridEntity();

            //明細が1件も登録されていなければ、エラー Ｅ１８９
            if (dt.Rows.Count == 0)
            {
                //更新対象なし
                bbl.ShowMessage("E189");
                return;
            }

            //更新処理
            dme = GetEntity();
            zibl.D_MoveRequest_Exec(dme, dt, (short)OperationMode);

            if (OperationMode == EOperationMode.DELETE)
                bbl.ShowMessage("I102");
            else
                bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)
            mDetailOperationMode = EOperationMode.INSERT;

            //排他処理を解除
            DeleteExclusive();

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    keyControls[0].Focus();
                    break;

            }

        }

        private void ChangeDetailOperationMode(EOperationMode mode)
        {
            switch (mode)
            {
                case EOperationMode.INSERT:
                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                    mDetailOperationMode = mode;

                    //「在庫移動明細編集エリア」の全項目をClear
                    ClrDetailEditArea();

                    if (mDetailOperationMode == EOperationMode.INSERT)
                        detailControls[(int)EIndex.JANCD].Focus();
                    else
                        detailControls[(int)EIndex.Gyono].Focus();
                    return;
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
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                    //radioButton1.Checked = true;
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

            foreach (Control ctl in detailLabels)
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }

            mOldDate = "";
            m_MaxMoveGyoNo = 0;
            mFromStoreCD = "";

            S_Clear_Grid();   //画面クリア（明細部）
            DeleteRowNo.Items.Clear();

            ClrDetailEditArea();

            //BtnSubF2.Enabled = true;
            //BtnSubF3.Enabled = true;
            //BtnSubF4.Enabled = true;

            InitScr();
        }

        private void InitScr()
        {

            if (CboIdoKbn.DataSource != null)
            {
                CboIdoKbn.SelectedIndex = 1;
                CheckDetail((int)EIndex.MovePurposeKBN);
            }
            if (CboFromSoukoCD.DataSource != null)
            {
                if (((DataTable)CboFromSoukoCD.DataSource).Rows.Count == 2)
                {
                    CboFromSoukoCD.SelectedIndex = 1;
                }
            }
            if (CboToSoukoCD.DataSource != null)
            {
                if (((DataTable)CboToSoukoCD.DataSource).Rows.Count == 2)
                {
                    CboToSoukoCD.SelectedIndex = 1;
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

                            ////抽出条件エリア	※新規時のみ、入力可能
                            //if (OperationMode != EOperationMode.INSERT)
                            //{
                            //    for(int index = (int)EIndex.Gyono; index<= (int)EIndex.RackNO; index++)
                            //    {
                            //        detailControls[index].Enabled = false;
                            //    }
                            //    ScMaker.BtnSearch.Enabled = false;
                            //}
                            break;
                        }
                }
            }

            CboIdoKbn.Enabled = false;
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            int idx = Array.IndexOf(detailControls, previousCtrl);
            //フォーカスが「在庫移動明細エリア」にある場合のみ利用可能。
            if (idx >= (int)EIndex.Gyono && idx <= (int)EIndex.RemarksInStore && Index <= 3)
            {
                ChangeDetailOperationMode((EOperationMode)Index);
            }

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

                case 8: //F9:検索
                    if (previousCtrl.Name.Equals(SC_ITEM_0.Name))
                    {
                        //商品検索
                        SearchData(EsearchKbn.Product, previousCtrl);
                    }
                    break;

                case 9://確認(F10)
                    ExecDisp();
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

        ///// <summary>
        ///// 検索フォーム起動処理
        ///// </summary>
        ///// <param name="kbn"></param>
        ///// <param name="setCtl"></param>
        //private void SearchData(EsearchKbn kbn, Control setCtl)
        //{
        //    switch (kbn)
        //    {
        //        case EsearchKbn.Product:
        //            int w_Row;

        //            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
        //            {
        //                return;
        //            }

        //            w_Row = w_CtlRow + Vsb_Mei_0.Value;

        //            //画面より配列セット 
        //            mGrid.S_DispToArray(Vsb_Mei_0.Value);

        //            using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.PurchaseDate].Text))
        //            {
        //                frmProduct.SKUCD = mGrid.g_DArray[w_Row].SKUCD;
        //                //frmProduct.ITEM = mGrid.g_DArray[w_Row].item;
        //                frmProduct.JANCD = mGrid.g_DArray[w_Row].JanCD;
        //                frmProduct.ShowDialog();

        //                if (!frmProduct.flgCancel)
        //                {
        //                    mGrid.g_DArray[w_Row].JanCD = frmProduct.JANCD;
        //                    mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
        //                    mGrid.g_DArray[w_Row].AdminNO = frmProduct.AdminNO;

        //                    //配列の内容を画面へセット
        //                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        //                }
        //            }
        //            break;



        //    }

        //}

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
                        if (index == (int)EIndex.StoreCD)
                        {
                            detailControls[(int)EIndex.RequestDate].Focus();
                        }
                        else if (index == (int)EIndex.RequestNO)
                            if (OperationMode == EOperationMode.UPDATE)
                                detailControls[(int)EIndex.RequestDate].Focus();
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
                        if (index == (int)EIndex.RemarksInStore)
                            btnSubF10.Focus();
                        else if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else if (index < (int)EIndex.ToSoukoCD)
                            {
                                bool exists = false;
                                for (int i = index + 2; i <= (int)EIndex.ToSoukoCD; i++)
                                {
                                    if (detailControls[i].CanFocus)
                                    {
                                        exists = true;
                                        detailControls[i].Focus();
                                        break;
                                    }
                                }
                                if (!exists && detailControls[(int)EIndex.Gyono].CanFocus)
                                    detailControls[(int)EIndex.Gyono].Focus();
                                else
                                {
                                    //あたかもTabキーが押されたかのようにする
                                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                                    ProcessTabKey(!e.Shift);
                                }
                            }
                            else
                            {
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                            }
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
        private void BtnSubF10_Click(object sender, EventArgs e)
        {
            //新規登録時のみ使用可
            //納品ボタンClick時   
            try
            {
                FunctionProcess(9);

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

                //BtnSubF2.Enabled = false;
                //BtnSubF3.Enabled = false;
                //BtnSubF4.Enabled = false;
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

                //フォーカスが「在庫移動明細エリア」にある場合のみ利用可能。
                int idx = Array.IndexOf(detailControls, previousCtrl);

                //Key Areaが、新規モードと変更モードの場合のみ利用可能。
                if (OperationMode == EOperationMode.INSERT || OperationMode == EOperationMode.UPDATE)
                {
                    //Key Areaの移動依頼番号が入力済の場合は、移動依頼の回答のみを行うため、新規ボタン利用不可。
                    if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.RemarksInStore].Text))
                        BtnSubF2.Enabled = true;
                }
                BtnSubF3.Enabled = true;
                BtnSubF4.Enabled = true;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        //private void GridControl_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        previousCtrl = this.ActiveControl;

        //        int w_Row;
        //        Control w_ActCtl;

        //        w_ActCtl = (Control)sender;
        //        w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

        //        // 背景色
        //        w_ActCtl.BackColor = ClsGridBase.BKColor;

        //        if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
        //        {
        //            return;
        //        }

        //        Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GridControl_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int w_Row;
        //        Control w_ActCtl;

        //        w_ActCtl = (Control)sender;
        //        w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

        //        if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
        //        {
        //            return;
        //        }
        //        // 背景色
        //        w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void GridControl_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        //Enterキー押下時処理
        //        //Returnキーが押されているか調べる
        //        //AltかCtrlキーが押されている時は、本来の動作をさせる
        //        if ((e.KeyCode == Keys.Return) &&
        //            ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
        //        {
        //            int w_Row;
        //            Control w_ActCtl;

        //            w_ActCtl = (Control)sender;
        //            w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

        //            bool lastCell = false;

        //            if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
        //            {
        //                return;
        //            }

        //            if (CL == (int)ClsGridIdo.ColNO.Chk)
        //                if (w_Row == mGrid.g_MK_Max_Row - 1)
        //                    lastCell = true;


        //            //画面の内容を配列へセット
        //            mGrid.S_DispToArray(Vsb_Mei_0.Value);


        //            //チェック処理
        //            if (CheckGrid(CL, w_Row) == false)
        //            {
        //                //Focusセット処理
        //                w_ActCtl.Focus();
        //                return;
        //            }

        //            if (lastCell)
        //            {
        //                w_ActCtl.Focus();
        //                return;
        //            }

        //            //あたかもTabキーが押されたかのようにする
        //            //Shiftが押されている時は前のコントロールのフォーカスを移動
        //            //this.ProcessTabKey(!e.Shift);
        //            //行き先がなかったら移動しない
        //            S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;
                Control sc = ((Control)sender).Parent;

                //検索ボタンClick時
                if (((Search.CKM_SearchControl)sc).Name.Substring(0, 3).Equals("SC_"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
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

        ///// <summary>
        ///// 明細部削除チェックボックスクリック時処理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void CHK_Del_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int w_Row;
        //        Control w_ActCtl;

        //        w_ActCtl = (Control)sender;
        //        w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

        //        //画面より配列セット 
        //        mGrid.S_DispToArray(Vsb_Mei_0.Value);

        //        bool Check = mGrid.g_DArray[w_Row].Chk;
        //        ChangeCheck(Check, w_Row);

        //        //配列の内容を画面へセット
        //        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}

        /// <summary>
        /// CheckBox　CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkM_CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void BtnSubF2_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeDetailOperationMode(EOperationMode.INSERT);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSubF3_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeDetailOperationMode(EOperationMode.UPDATE);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnSubF4_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeDetailOperationMode(EOperationMode.DELETE);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        #endregion

        private void SetEnabled(EIdoType kbn)
        {
            if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
                return;

                switch (kbn)
            {
                case EIdoType.店舗間移動:
                    for (int i = (int)EIndex.Gyono; i <= (int)EIndex.RemarksInStore; i++)
                    {
                        switch (i)
                        {
                            default:
                                detailControls[i].Enabled = true;
                                break;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// kbn=0【移動依頼明細が全てまたは一部回答済の場合】
        /// kbn=1【移動依頼明細が全て未回答の場合】
        /// </summary>
        private void SetEnabledAfterDisp(short kbn)
        {
            if (kbn == 0)
            {
                BtnSubF2.Enabled = false;
                BtnSubF4.Enabled = false;
                mDetailOperationMode = EOperationMode.UPDATE;
            }
            else if (kbn == 1)
            {
                BtnSubF4.Enabled = false;
                mDetailOperationMode = EOperationMode.INSERT;
            }

            for (int i = 0; i <= (int)EIndex.RemarksInStore; i++)
            {
                switch (i)
                {
                    case (int)EIndex.MovePurposeKBN:
                    case (int)EIndex.FromSoukoCD:
                    case (int)EIndex.ToSoukoCD:
                    case (int)EIndex.JANCD:
                    case (int)EIndex.Suryo:
                        detailControls[i].Enabled = false;
                        break;
                }
            }
        }
        private void SetDataFromGrid(int gyono)
        {
            int row = gyono - 1;

            detailControls[(int)EIndex.Gyono].Text = mGrid.g_DArray[row].GYONO;
            detailControls[(int)EIndex.JANCD].Text = mGrid.g_DArray[row].JanCD;
            mJANCD = mGrid.g_DArray[row].JanCD;
            mAdminNO = mGrid.g_DArray[row].AdminNO;
            lblColorName.Text = mGrid.g_DArray[row].ColorName;
            lblSizeName.Text = mGrid.g_DArray[row].SizeName;
            lblSKUCD.Text = mGrid.g_DArray[row].SKUCD;
            lblSKUName.Text = mGrid.g_DArray[row].SKUName;

            detailControls[(int)EIndex.Suryo].Text = bbl.Z_SetStr(mGrid.g_DArray[row].RequestSu);
            detailControls[(int)EIndex.ExpectedDate].Text = mGrid.g_DArray[row].ExpectedDate;
            detailControls[(int)EIndex.RemarksInStore].Text = mGrid.g_DArray[row].CommentInStore;

            detailControls[(int)EIndex.Gyono].Focus();
        }
        private void SetDataToGrid()
        {
            int gyono = Convert.ToInt16(bbl.Z_Set(detailControls[(int)EIndex.Gyono].Text));
            int row = 0;
            if (gyono == 0)
            {
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD))
                    {
                        row = RW;
                        break;
                    }
                }
            }
            else
            {
                row = gyono - 1;
            }
            //m_MaxMoveGyoNo++; 更新時にカウントすべき？
            if (mDetailOperationMode == EOperationMode.DELETE)
            {
                DEL_SUB(row);
            }
            else
            {
                //mGrid.g_DArray[row].MoveRows = m_MaxMoveGyoNo;
                mGrid.g_DArray[row].JanCD = detailControls[(int)EIndex.JANCD].Text;
                mGrid.g_DArray[row].AdminNO = mAdminNO;
                mGrid.g_DArray[row].ColorName = lblColorName.Text;
                mGrid.g_DArray[row].SizeName = lblSizeName.Text;
                mGrid.g_DArray[row].SKUCD = lblSKUCD.Text;
                mGrid.g_DArray[row].SKUName = lblSKUName.Text;

                mGrid.g_DArray[row].RequestSu = bbl.Z_SetStr(detailControls[(int)EIndex.Suryo].Text);
                mGrid.g_DArray[row].ExpectedDate = detailControls[(int)EIndex.ExpectedDate].Text;
                mGrid.g_DArray[row].CommentInStore = detailControls[(int)EIndex.RemarksInStore].Text;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            ClrDetailEditArea();

            if (mDetailOperationMode == EOperationMode.INSERT)
                detailControls[(int)EIndex.JANCD].Focus();
            else
                detailControls[(int)EIndex.Gyono].Focus();
        }
        // -----------------------------------------------
        // <明細部>行削除処理 Ｆ７
        // -----------------------------------------------
        private void DEL_SUB(int w_Row)
        {
            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //行削除したデータは行番号を退避するよう変更
            if (mGrid.g_DArray[w_Row].RequestRows > 0)
            {
                D_MoveRequest_Entity dmen = new D_MoveRequest_Entity
                {
                    OldRequestSu = mGrid.g_DArray[w_Row].OldRequestSu,
                    RequestRows = mGrid.g_DArray[w_Row].RequestRows
                };
                DeleteRowNo.Items.Add(dmen);
            }

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
        }
        private void ClrDetailEditArea()
        {
            //在庫移動明細編集エリア(Detail Area)の画面項目はClear
            for (int index = (int)EIndex.Gyono; index <= (int)EIndex.RemarksInStore; index++)
            {
                detailControls[index].Text = "";
            }
            lblColorName.Text = "";
            lblSizeName.Text = "";
            lblSKUCD.Text = "";
            lblSKUName.Text = "";
            mAdminNO = "";
            mJANCD = "";
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
                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.RequestDate].Text))
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

                            CheckDetail((int)EIndex.JANCD, true);
                        }
                    }
                    break;
            }

        }
        private void ChangeBackColor(int w_Row, short kbn = 0)
        {
            Color backCL = GridBase.ClsGridBase.GrayColor;

            for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridIdoIrai.ColNO.GYONO:
                    case (int)ClsGridIdoIrai.ColNO.Space:
                    case (int)ClsGridIdoIrai.ColNO.Space2:
                    case (int)ClsGridIdoIrai.ColNO.CommentOutStore:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            break;
                        }
                    default:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = System.Drawing.Color.Empty;
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                            break;
                        }
                }
            }

        }
    }
}








