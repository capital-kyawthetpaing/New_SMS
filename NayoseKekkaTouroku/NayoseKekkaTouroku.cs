using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace NayoseKekkaTouroku
{
    /// <summary>
    /// NayoseKekkaTouroku 名寄せ結果登録
    /// </summary>
    internal partial class NayoseKekkaTouroku : FrmMainForm
    {
        private const string ProID = "NayoseKekkaTouroku";
        private const string ProNm = "名寄せ結果登録";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            CboSite,
            NayoseKekkaTourokuDate,
        }

           private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private NayoseKekkaTouroku_BL nkbl;
        private D_Juchuu_Entity dje;

        private DataTable dtForUpdate;      //排他用   
        private string mOldJuchuNO = "";    //排他処理のため使用

        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridNayose mGrid = new ClsGridNayose();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxGyoNo;

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

            if (ClsGridNayose.gc_P_GYO <= ClsGridNayose.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridNayose.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridNayose.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridNayose.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridNayose.gc_MaxCL;

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
            mGrid.g_DArray = new ClsGridNayose.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridNayose.ColNO.GYONO; i <= (int)ClsGridNayose.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目のTabIndexセット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridNayose.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridPicking.ColNO.TEL2:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
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

        // 明細部のコントロールを配列にセット
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 0].CellCtl = LBL_NAME2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 0].CellCtl = LBL_TEL2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 0].CellCtl = LBL_CLINT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 0].CellCtl = IMN_WEBPR_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 0].CellCtl = IMT_MAIL_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 0].CellCtl = IMT_ZIP_0;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 0].CellCtl = IMT_ARIDT_0;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 0].CellCtl = IMT_TEL_0;    //支払予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 0].CellCtl = IMT_NAME_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 0].CellCtl = CHK_EDICK_0;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 0].CellCtl = LBL_ZIP2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 0].CellCtl = LBL_MAIL2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 0].CellCtl = LBL_ADD21_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 0].CellCtl = LBL_ADD22_0;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 0].CellCtl = IMT_SPACE2_0;


            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 1].CellCtl = CHK_DELCK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 1].CellCtl = LBL_NAME2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 1].CellCtl = LBL_TEL2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 1].CellCtl = LBL_CLINT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 1].CellCtl = IMN_WEBPR_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 1].CellCtl = IMT_MAIL_1;
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 1].CellCtl = IMT_ZIP_1;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 1].CellCtl = IMT_ARIDT_1;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 1].CellCtl = IMT_TEL_1;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 1].CellCtl = IMT_NAME_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 1].CellCtl = CHK_EDICK_1;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 1].CellCtl = LBL_ZIP2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 1].CellCtl = LBL_MAIL2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 1].CellCtl = LBL_ADD21_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 1].CellCtl = LBL_ADD22_1;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 1].CellCtl = IMT_SPACE2_1;
            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 2].CellCtl = CHK_DELCK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 2].CellCtl = LBL_NAME2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 2].CellCtl = LBL_TEL2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 2].CellCtl = LBL_CLINT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 2].CellCtl = IMN_WEBPR_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 2].CellCtl = IMT_MAIL_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 2].CellCtl = IMT_ZIP_2;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 2].CellCtl = IMT_ARIDT_2;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 2].CellCtl = IMT_TEL_2;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 2].CellCtl = IMT_NAME_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 2].CellCtl = CHK_EDICK_2;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 2].CellCtl = LBL_ZIP2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 2].CellCtl = LBL_MAIL2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 2].CellCtl = LBL_ADD21_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 2].CellCtl = LBL_ADD22_2;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 2].CellCtl = IMT_SPACE2_2;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 3].CellCtl = CHK_DELCK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 3].CellCtl = LBL_NAME2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 3].CellCtl = LBL_TEL2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 3].CellCtl = LBL_CLINT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 3].CellCtl = IMN_WEBPR_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 3].CellCtl = IMT_MAIL_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 3].CellCtl = IMT_ZIP_3;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 3].CellCtl = IMT_ARIDT_3;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 3].CellCtl = IMT_TEL_3;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 3].CellCtl = IMT_NAME_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 3].CellCtl = CHK_EDICK_3;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 3].CellCtl = LBL_ZIP2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 3].CellCtl = LBL_MAIL2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 3].CellCtl = LBL_ADD21_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 3].CellCtl = LBL_ADD22_3;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 3].CellCtl = IMT_SPACE2_3;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 4].CellCtl = CHK_DELCK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 4].CellCtl = LBL_NAME2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 4].CellCtl = LBL_TEL2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 4].CellCtl = LBL_CLINT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 4].CellCtl = IMN_WEBPR_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 4].CellCtl = IMT_MAIL_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 4].CellCtl = IMT_ZIP_4;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 4].CellCtl = IMT_ARIDT_4;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 4].CellCtl = IMT_TEL_4;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 4].CellCtl = IMT_NAME_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 4].CellCtl = CHK_EDICK_4;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 4].CellCtl = LBL_ZIP2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 4].CellCtl = LBL_MAIL2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 4].CellCtl = LBL_ADD21_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 4].CellCtl = LBL_ADD22_4;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 4].CellCtl = IMT_SPACE2_4;
            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            //mGrid.g_MK_Ctrl[(int)ClsGridHacchuu.ColNO.ChkDel, 5].CellCtl = CHK_DELCK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD11, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME2, 5].CellCtl = LBL_NAME2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD12, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL2, 5].CellCtl = LBL_TEL2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Client, 5].CellCtl = LBL_CLINT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space1, 5].CellCtl = IMN_WEBPR_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL, 5].CellCtl = IMT_MAIL_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP, 5].CellCtl = IMT_ZIP_5;      //メーカー商品CD
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.SiteNm, 5].CellCtl = IMT_ARIDT_5;     //入荷予定日
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.TEL, 5].CellCtl = IMT_TEL_5;    //支払予定日
            
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.NAME, 5].CellCtl = IMT_NAME_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ChkNayose, 5].CellCtl = CHK_EDICK_5;

            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ZIP2, 5].CellCtl = LBL_ZIP2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.MAIL2, 5].CellCtl = LBL_MAIL2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD21, 5].CellCtl = LBL_ADD21_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.ADD22, 5].CellCtl = LBL_ADD22_5;
            mGrid.g_MK_Ctrl[(int)ClsGridNayose.ColNO.Space2, 5].CellCtl = IMT_SPACE2_5;
        }

        // 明細部 Tab の処理
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNayose.Gen_MK_FocusMove.MvNxt, (int)ClsGridNayose.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Shift+Tab の処理
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNayose.Gen_MK_FocusMove.MvPrv, (int)ClsGridNayose.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 Enter の処理
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridNayose.Gen_MK_FocusMove.MvNxt, (int)ClsGridNayose.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }

        // 明細部 PageDown の処理
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridNayose.Gen_MK_FocusMove.MvSet, (int)ClsGridNayose.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }

        // 明細部 PageUp の処理
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridNayose.Gen_MK_FocusMove.MvSet, (int)ClsGridNayose.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
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

                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JuchuuNO))
                                {
                                    continue;
                                }

                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridNayose.ColNO.ChkNayose:    // 
                                                         mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                        case (int)ClsGridNayose.ColNO.Client:
                                        case (int)ClsGridNayose.ColNO.NAME2:
                                        case (int)ClsGridNayose.ColNO.ZIP2:
                                        case (int)ClsGridNayose.ColNO.ADD21:
                                        case (int)ClsGridNayose.ColNO.ADD22:
                                        case (int)ClsGridNayose.ColNO.TEL2:
                                        case (int)ClsGridNayose.ColNO.MAIL2:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            if (mGrid.g_DArray[w_Row].AttentionFLG == "1")
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ForeColor= Color.Red;
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ForeColor = Color.DimGray;
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

        public NayoseKekkaTouroku()
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
                CboStoreCD.Bind(ymd);
                nkbl = new NayoseKekkaTouroku_BL();

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
            keyControls = new Control[] { CboStoreCD, ckM_TextBox4 };
            keyLabels = new Control[] {  };
            detailControls = new Control[] {  };
            detailLabels = new Control[] {  };
            searchButtons = new Control[] {  };

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
            if (keyControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)keyControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.CboSite:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        return true;
                    }
                    
                    break;

               
                case (int)EIndex.NayoseKekkaTourokuDate:
                
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
                       break;
            }

            return true;

        }
        private bool SelectAndInsertExclusive(Exclusive_BL.DataKbn kbn, string No)
        {
            if (OperationMode == EOperationMode.SHOW)
                return true;

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)kbn,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                PreviousCtrl.Focus();
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
        private void DeleteExclusive()
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                mOldJuchuNO = "";
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity
                    {
                        DataKBN = Convert.ToInt16(dr["kbn"]),
                        Number = dr["no"].ToString()
                    };

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }
        }
        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set, int index= (int)EIndex.CboSite)
        {
            M_Customer_Entity mce = new M_Customer_Entity();
            mce.StoreKBN = "1";//1（WEB)
            DataTable dtCustomer = nkbl.M_Customer_SelectForNayose(mce);

            //[D_Juchu_SelectForNayose]
            dje = GetSearchInfo();

            DataTable dt = nkbl.D_Juchu_SelectForNayose(dje);

            //ヘッダ部.名寄せ実施日＝空白　の時
            //画面転送表01に従って、画面情報を表示

            //ヘッダ部.名寄せ実施日<>空白　の時
            //画面転送表02に従って、画面情報を表示

            //テーブル転送仕様Ｙに従って、排他テーブルにレコード追加
            DeleteExclusive();

            dtForUpdate = new DataTable();
            dtForUpdate.Columns.Add("kbn", Type.GetType("System.String"));
            dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

            bool ret;
            //排他処理
            foreach (DataRow row in dt.Rows)
            {                
                if (mOldJuchuNO != row["JuchuuNO"].ToString() && !string.IsNullOrWhiteSpace(row["JuchuuNO"].ToString()))
                {
                    ret = SelectAndInsertExclusive(Exclusive_BL.DataKbn.Jyuchu, row["JuchuuNO"].ToString());
                    if (!ret)
                        return false;

                    mOldJuchuNO = row["JuchuuNO"].ToString();

                    // データを追加
                    DataRow rowForUpdate;
                    rowForUpdate = dtForUpdate.NewRow();
                    rowForUpdate["kbn"] = (int)Exclusive_BL.DataKbn.Jyuchu;
                    rowForUpdate["no"] = mOldJuchuNO;
                    dtForUpdate.Rows.Add(rowForUpdate);
                }
            }

            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                Scr_Clr(1);
                PreviousCtrl.Focus();
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
                m_MaxGyoNo = 0;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        bbl.ShowMessage("E178", m_EnableCnt.ToString());
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }
                    if (row["ROWNUM"].ToString().Equals("1"))
                    {
                        mGrid.g_DArray[i].SiteNm = row["SiteName"].ToString();
                        mGrid.g_DArray[i].NAME = row["CustomerName"].ToString();
                        mGrid.g_DArray[i].ZIP = row["ZIP"].ToString();
                        mGrid.g_DArray[i].ADD11 = row["Address1"].ToString();
                        mGrid.g_DArray[i].ADD12 = row["Address2"].ToString();   // 
                        mGrid.g_DArray[i].TEL = row["TEL"].ToString();   // 
                        mGrid.g_DArray[i].MAIL = row["MailAddress"].ToString();   //                      
                    }
                    mGrid.g_DArray[i].Client = row["M_CustomerCD"].ToString();   // 
                    mGrid.g_DArray[i].NAME2 = row["M_CustomerName"].ToString();   // 
                    mGrid.g_DArray[i].ZIP2 = row["M_ZIP"].ToString();
                    mGrid.g_DArray[i].ADD21 = row["M_Address1"].ToString();
                    mGrid.g_DArray[i].ADD22 = row["M_Address2"].ToString();   // 
                    mGrid.g_DArray[i].TEL2 = row["M_TEL"].ToString();   // 
                    mGrid.g_DArray[i].MAIL2 = row["M_MailAddress"].ToString();   // 
                    
                    mGrid.g_DArray[i].ChkNayose = false;

                    if (!string.IsNullOrWhiteSpace(keyControls[(int)EIndex.NayoseKekkaTourokuDate].Text))
                    {
                        //D_Juchuu.CustomerCD＝M_Customer.CustomerCD の時、CheckBox＝ON
                        if (row["CustomerCD"].ToString().Equals(row["M_CustomerCD"].ToString()))
                        {
                            mGrid.g_DArray[i].ChkNayose = true;
                            mGrid.g_DArray[i].ChkOldNayose = true;
                        }
                    }

                    //税額(Hidden)
                    mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                    mGrid.g_DArray[i].AttentionFLG = row["AttentionFLG"].ToString();
                    m_dataCnt = i + 1;
                    i++;
                }

                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);

            }

            S_BodySeigyo(1, 0);
            S_BodySeigyo(1, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            
            //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridNayose.ColNO.ChkNayose);

            return true;
        }
        protected override void ExecDisp()
        {
            //抽出条件エリアのエラーチェック
            for (int i = (int)EIndex.CboSite; i <= (int)EIndex.NayoseKekkaTourokuDate; i++)
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
        private D_Juchuu_Entity GetEntity()
        {
            dje = new D_Juchuu_Entity
            {
                NayoseKekkaTourokuDate = keyControls[(int)EIndex.NayoseKekkaTourokuDate].Text,
                InsertOperator = InOperatorCD,
                PC = InPcID
            };

            return dje;
        }
        private D_Juchuu_Entity GetSearchInfo()
        {
            dje = new D_Juchuu_Entity
            {
                SiteKBN = CboStoreCD.SelectedIndex>0 ? CboStoreCD.SelectedValue.ToString():"",
                NayoseKekkaTourokuDate = keyControls[(int)EIndex.NayoseKekkaTourokuDate].Text,
            };
            return dje;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("JuchuuNO", typeof(string));
            dt.Columns.Add("CustomerCD", typeof(string));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JuchuuNO))
                    break;

                if (mGrid.g_DArray[RW].ChkNayose || mGrid.g_DArray[RW].ChkOldNayose)
                {
                    dt.Rows.Add(mGrid.g_DArray[RW].JuchuuNO
                        , mGrid.g_DArray[RW].Client
                        , !mGrid.g_DArray[RW].ChkNayose  && mGrid.g_DArray[RW].ChkOldNayose ? 1:0
                        );
                }
            }

            return dt;
        }
        protected override void ExecSec()
        {
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
            dje = GetEntity();
            nkbl.NayoseKekkaTouroku_Exec(dje, dt);

            bbl.ShowMessage("I101");

            //更新後画面クリア
            ChangeOperationMode(OperationMode);
        }
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive();

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
                        //if (index == (int)EIndex.PickingDate)
                        

                        //else if (detailControls.Length - 1 > index)
                        //{
                        //    if (detailControls[index + 1].CanFocus)
                        //        detailControls[index + 1].Focus();
                        //    else
                        //        //あたかもTabキーが押されたかのようにする
                        //        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        //        ProcessTabKey(!e.Shift);                          
                        //}
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
                PreviousCtrl = this.ActiveControl;

                int index = Array.IndexOf(keyControls, sender);
                switch (index)
                {
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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                PreviousCtrl = this.ActiveControl;
            
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
                PreviousCtrl = this.ActiveControl;

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

                    bool lastCell = false;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridNayose.ColNO.ChkNayose)
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
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    switch (CL)
                    {
                        case (int)ClsGridNayose.ColNO.ChkNayose:
                            if (e.Shift)
                                S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                            else
                                S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);

                            break;
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

                //同一受注番号内で「する」チェックボックスがONの明細があればOFFにする。
                bool Check = mGrid.g_DArray[w_Row].ChkNayose;
                if(Check)
                {
                    string juchuuNo = mGrid.g_DArray[w_Row].JuchuuNO;
                    for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                    {
                        //更新有効行数
                        if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JuchuuNO))
                            break;

                        if (mGrid.g_DArray[RW].JuchuuNO.Equals(juchuuNo) && mGrid.g_DArray[RW].ChkNayose)
                        {
                            if(RW != w_Row)
                            {
                                mGrid.g_DArray[RW].ChkNayose = false;
                            }
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

        #endregion

        private void ChangeBackColor(int w_Row)
        {
            Color backCL = GridBase.ClsGridBase.GrayColor;

            for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridNayose.ColNO.ChkNayose:
                        break;
                    case (int)ClsGridNayose.ColNO.GYONO:
                    case (int)ClsGridNayose.ColNO.Space1:
                    case (int)ClsGridNayose.ColNO.Space2:
                        {
                            mGrid.g_MK_State[w_Col, w_Row].Cell_Color = backCL;
                            break;
                        }

                    default:
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Selectable = false;
                        break;
                }
            }
        }

        private void ckM_Button1_Click(object sender, EventArgs e)
        {
            try
            {
                D_Juchuu_Entity dje = new  D_Juchuu_Entity
                {
                    InsertOperator = InOperatorCD,
                    PC = InPcID
                };
                NayoseSyoriAll_BL nbl = new NayoseSyoriAll_BL();
                nbl.NayoseSyoriAll_Exec(dje);

                bbl.ShowMessage("I001", "名寄せ処理");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}








