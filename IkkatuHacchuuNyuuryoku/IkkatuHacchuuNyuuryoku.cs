using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using BL;
using DL;
using Entity;
using Base.Client;
using Search;
using GridBase;
using System.Linq;

namespace IkkatuHacchuuNyuuryoku
{
    /// <summary>
    /// IkkatuHacchuuNyuuryoku 一括発注入力
    /// </summary>
    internal partial class IkkatuHacchuuNyuuryoku : FrmMainForm
    {
        #region const

        /// <summary>
        /// 
        /// </summary>
        private const string ProID = "IkkatuHacchuuNyuuryoku";
        /// <summary>
        /// 
        /// </summary>
        private const string ProNm = "一括発注入力";
        /// <summary>
        /// 
        /// </summary>
        private const short mc_L_END = 3; // ロック用

        #endregion

        #region enum

        /// <summary>
        /// 
        /// </summary>
        private enum EIndex : int
        {
            StoreCD,

            HacchuuDate = 0,
            SiiresakiCD,
            JuchuuStaffCD,
            HacchuuNO,
            HacchuuShoriNO,
            StaffCD,

            SiiresakiName = 0,
            JuchuStaffName,
            StaffName,

            Siiresaki_Search = 0,
            HacchuuNO_Search,
            HacchuuShoriNO_Search,
        }
        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Vendor,
            HacchuuShoriNO,
        }
        /// <summary>
        /// 一括発注モード
        /// </summary>
        private enum IkkatuHacchuuMode : byte
        {
            NetHacchuu,
            FAXHacchuu
        }

        #endregion

        #region 変数

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        private IkkatuHacchuuNyuuryoku_BL ikkatuHacchuuBL;
        private D_Order_Entity dOrderEntity;
        private D_OrderDetails_Entity dOrderDetailsEntity;
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        private string mOldIkkatuHacchuuNo = string.Empty;    //排他処理のため使用
        private string mOldIkkatuHacchuuShoriNo = string.Empty;    //排他処理のため使用
        private List<string> mOldJuchuuNOList = new List<string>();    //排他処理のため使用
        private string mOldIkkatuHacchuuDate = string.Empty;
        private string mOldVendorCD = string.Empty;
        private decimal mZei10;//通常税額(Hidden)
        private decimal mZei8;//軽減税額(Hidden)
        // -- 明細部をグリッドのように扱うための宣言 ↓--------------------
        ClsGridIkkatuHacchuu mGrid = new ClsGridIkkatuHacchuu();
        private int m_EnableCnt;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private IkkatuHacchuuMode ikkatuHacchuuMode;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IkkatuHacchuuNyuuryoku()
        {
            InitializeComponent();

            //ホイールイベントの追加  
            this.Vsb_Mei_0.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);
        }

        #endregion

        #region イベント - Load

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;

                this.SetFunctionLabel(EProMode.INPUT);

                this.Btn_F7.Text = "";
                this.Btn_F8.Text = "";
                this.Btn_F10.Text = "";

                this.InitialControlArray();
                // 明細部初期化
                this.S_SetInit_Grid();

                //起動時共通処理
                base.StartProgram();

                string ymd = bbl.GetDate();
                ikkatuHacchuuBL = new IkkatuHacchuuNyuuryoku_BL();
                CboStoreCD.Bind(ymd,"2");
                //CboJuchuuChanceKBN.Bind(string.Empty);

                string stores = GetAllAvailableStores();
                ScHacchuuNO.Value1 = InOperatorCD;
                ScHacchuuNO.Value2 = stores;

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
                    CboStoreCD.SelectedValue = mse.StoreCD;
                    ScHacchuuShoriNO.Value3 = CboStoreCD.SelectedValue.ToString();
                    ScStaff.LabelText = mse.StaffName;
                }

                //[M_Store]
                M_Store_Entity mse2 = new M_Store_Entity
                {
                    StoreCD = mse.StoreCD,
                    ChangeDate = ymd
                };
                Store_BL sbl = new Store_BL();
                DataTable dt = sbl.M_Store_Select(mse2);
                if (dt.Rows.Count > 0)
                {
                    //detailControls[(int)EIndex.DeliveryDate].Text = dt.Rows[0]["DeliveryDate"].ToString();
                    //detailControls[(int)EIndex.PaymentTerms].Text = dt.Rows[0]["PaymentTerms"].ToString();
                    //detailControls[(int)EIndex.DeliveryPlace].Text = dt.Rows[0]["DeliveryPlace"].ToString();
                    //detailControls[(int)EIndex.ValidityPeriod].Text = dt.Rows[0]["ValidityPeriod"].ToString();
                }
                else
                {
                    bbl.ShowMessage("E133");
                    EndSec();
                }

                this.txtHacchuuDate.Text = bbl.GetDate().ToString();
                this.lblIkkatuHacchuuMode.Visible = true;
                this.btnChangeIkkatuHacchuuMode.Visible = true;
                this.ScSiiresakiCD.Focus();
                this.ikkatuHacchuuMode = IkkatuHacchuuMode.NetHacchuu;
                this.ControlByMode();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        #endregion

        #region イベント - ヘッダ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                            detailControls[(int)EIndex.HacchuuDate].Focus();
                        }
                        else
                        {
                            keyControls[index + 1].Focus();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        if ((index == (int)EIndex.JuchuuStaffCD
                             && this.ScHacchuuNO.Enabled == false
                            )
                            || index == (int)EIndex.HacchuuShoriNO
                           )
                        {
                            this.BtnSubF11.Focus();
                        }
                        else if (detailControls.Length - 1 > index)
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
                            this.txtHacchuuDate.Focus();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                SetFuncKeyAll(this, "111111001011");
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion

        #region イベント - 明細グリッド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                if (sender is GridControl.clsGridCheckBox)
                {
                    int w_Row;
                    GridControl.clsGridCheckBox w_ActCtl;

                    w_ActCtl = (GridControl.clsGridCheckBox)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    //どの項目か判別
                    int CL = -1;
                    string ctlName = string.Empty;
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    }
                    else
                    {
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));
                    }

                    switch (ctlName)
                    {
                        case "chkEDIFLG":
                            CL = (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG;
                            break;
                        case "chkTaishouFLG":
                            CL = (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG;
                            break;
                    }
                    // 背景色
                    w_ActCtl.BackColor = ClsGridBase.BKColor;
                    Grid_Gotfocus(CL, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));
                }
                else
                {
                    int w_Row;
                    CKM_Controls.CKM_TextBox w_ActCtl;

                    w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    // 背景色
                    w_ActCtl.BackColor = ClsGridBase.BKColor;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }
                    Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;

                if (sender is GridControl.clsGridCheckBox)
                {
                    GridControl.clsGridCheckBox w_ActCtl;

                    w_ActCtl = (GridControl.clsGridCheckBox)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    //どの項目か判別
                    int CL = -1;
                    string ctlName = string.Empty;
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                    {
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    }
                    else
                    {
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));
                    }

                    switch (ctlName)
                    {
                        case "chkEDIFLG":
                            CL = (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG;
                            break;
                        case "chkTaishouFLG":
                            CL = (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG;
                            break;
                    }
                    // 背景色
                    w_ActCtl.BackColor = System.Drawing.Color.White; // mGrid.F_GetBackColor_MK(CL, w_Row);
                }
                else
                {
                    CKM_Controls.CKM_TextBox w_ActCtl;

                    w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                    w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }
                    // 背景色
                    w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    if (sender is GridControl.clsGridCheckBox)
                    {
                        GridControl.clsGridCheckBox w_ActCtl;
                        w_ActCtl = (GridControl.clsGridCheckBox)sender;
                        w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                        //どの項目か判別
                        int CL = -1;
                        string ctlName = string.Empty;
                        if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        {
                            ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                        }
                        else
                        {
                            ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));
                        }

                        switch (ctlName)
                        {
                            case "chkEDIFLG":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG;
                                break;
                            case "chkTaishouFLG":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG;
                                break;
                        }

                        //画面の内容を配列へセット
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);

                        if (CL == -1)
                            return;

                        //チェック処理
                        if (CheckGrid(CL, w_Row) == false)
                        {
                            //Focusセット処理
                            w_ActCtl.Focus();
                            return;
                        }
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        //this.ProcessTabKey(!e.Shift);
                        //行き先がなかったら移動しない
                        S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                    }
                    else
                    {
                        CKM_Controls.CKM_TextBox w_ActCtl;
                        w_ActCtl = (CKM_Controls.CKM_TextBox)sender;

                        //どの項目か判別
                        int CL = -1;
                        string ctlName = string.Empty;
                        if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        {
                            ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                            w_Row = System.Convert.ToInt32(w_ActCtl.Parent.Tag) + Vsb_Mei_0.Value;
                        }
                        else
                        {
                            ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));
                            w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                        }

                        bool changeFlg = w_ActCtl.Modified;
                        bool lastCell = false;

                        //if (changeFlg)
                        //{
                        switch (ctlName)
                        {
                            case "scSiiresakiCD":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD;
                                break;
                            case "chkEDIFLG":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG;
                                break;
                            case "txtKibouNouki":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.KibouNouki;
                                break;
                            case "txtShanaiBikou":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou;
                                break;
                            case "txtShagaiBikou":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou;
                                break;
                            case "txtHacchuuSuu":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu;
                                break;
                            case "txtHacchuuTanka":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka;
                                break;
                            case "txtHacchuugaku":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku;
                                if (w_Row == mGrid.g_MK_Max_Row - 1)
                                    lastCell = true;
                                break;
                            case "chkTaishouFLG":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG;
                                break;
                            case "txtShouhinName":
                                CL = (int)ClsGridIkkatuHacchuu.ColNO.ShouhinName;
                                break;
                        }

                        //画面の内容を配列へセット
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);

                        //手入力時金額を再計算
                        decimal wSuu = bbl.Z_Set(mGrid.g_DArray[w_Row].HacchuuSuu);
                        decimal wTanka = bbl.Z_Set(mGrid.g_DArray[w_Row].HacchuuTanka);
                        string ymd = detailControls[(int)EIndex.HacchuuDate].Text;

                        switch (CL)
                        {
                            case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD:
                                if (mGrid.g_DArray[w_Row].SiiresakiCD != mGrid.g_DArray[w_Row].preSiiresakiCD)
                                {
                                    //[M_JANOrderPrice_Select]
                                    M_JANOrderPrice_Entity mjop_StoreSiteiAri = new M_JANOrderPrice_Entity
                                    {
                                        AdminNO = mGrid.g_DArray[w_Row].AdminNO,
                                        VendorCD = mGrid.g_DArray[w_Row].SiiresakiCD,
                                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                                        StoreCD = CboStoreCD.SelectedValue.ToString()
                                    };
                                    M_JANOrderPrice_DL mjdl = new M_JANOrderPrice_DL();
                                    var dt_MJOP_StoreSiteiAri = mjdl.M_JANOrderPrice_Select(mjop_StoreSiteiAri);

                                    //[M_JANOrderPrice_Select]
                                    M_JANOrderPrice_Entity mjop_StoreSiteiNasi = new M_JANOrderPrice_Entity
                                    {
                                        AdminNO = mGrid.g_DArray[w_Row].AdminNO,
                                        VendorCD = mGrid.g_DArray[w_Row].SiiresakiCD,
                                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                                        StoreCD = "0000"
                                    };
                                    M_JANOrderPrice_DL mjdl_StoreSiteiNasi = new M_JANOrderPrice_DL();
                                    var dt_MJOP_StoreSiteiNasi = mjdl.M_JANOrderPrice_Select(mjop_StoreSiteiNasi);
                                    //[M_ItemOrderPrice_Select]
                                    M_ItemOrderPrice_Entity miop_StoreSiteiAri = new M_ItemOrderPrice_Entity
                                    {
                                        MakerItem = mGrid.g_DArray[w_Row].MakerShouhinCD,
                                        VendorCD = mGrid.g_DArray[w_Row].SiiresakiCD,
                                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                                        StoreCD = CboStoreCD.SelectedValue.ToString()
                                    };
                                    M_ItemOrderPrice_DL midl_StoreSiteiAri = new M_ItemOrderPrice_DL();
                                    var dt_MIOP_StoreSiteiAri = midl_StoreSiteiAri.M_ItemOrderPrice_Select(miop_StoreSiteiAri);

                                    //[M_ItemOrderPrice_Select]
                                    M_ItemOrderPrice_Entity miop_StoreSiteiNasi = new M_ItemOrderPrice_Entity
                                    {
                                        MakerItem = mGrid.g_DArray[w_Row].MakerShouhinCD,
                                        VendorCD = mGrid.g_DArray[w_Row].SiiresakiCD,
                                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                                        StoreCD = "0000"
                                    };
                                    M_ItemOrderPrice_DL midl_StoreSiteiNasi = new M_ItemOrderPrice_DL();
                                    var dt_MIOP_StoreSiteiNasi = midl_StoreSiteiNasi.M_ItemOrderPrice_Select(miop_StoreSiteiNasi);

                                    if (dt_MJOP_StoreSiteiAri.Rows.Count != 0)
                                    {
                                        mGrid.g_DArray[w_Row].HacchuuTanka = string.Format("{0:#,##0}", dt_MJOP_StoreSiteiAri.Rows[0].Field<decimal>("PriceWithoutTax"));
                                    }
                                    else if (dt_MJOP_StoreSiteiNasi.Rows.Count != 0)
                                    {
                                        mGrid.g_DArray[w_Row].HacchuuTanka = string.Format("{0:#,##0}", dt_MJOP_StoreSiteiNasi.Rows[0].Field<decimal>("PriceWithoutTax"));
                                    }
                                    else if (dt_MIOP_StoreSiteiAri.Rows.Count != 0)
                                    {
                                        mGrid.g_DArray[w_Row].HacchuuTanka = string.Format("{0:#,##0}", dt_MIOP_StoreSiteiAri.Rows[0].Field<decimal>("PriceWithoutTax"));
                                    }
                                    else if (dt_MIOP_StoreSiteiNasi.Rows.Count != 0)
                                    {
                                        mGrid.g_DArray[w_Row].HacchuuTanka = string.Format("{0:#,##0}", dt_MIOP_StoreSiteiNasi.Rows[0].Field<decimal>("PriceWithoutTax"));
                                    }
                                    mGrid.g_DArray[w_Row].Hacchuugaku = string.Format("{0:#,##0}", decimal.Parse(mGrid.g_DArray[w_Row].HacchuuTanka) * wSuu);
                                    //mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, w_Row].CellCtl.Text = mGrid.g_DArray[w_Row].HacchuuTanka.ToString();
                                    //mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, w_Row].CellCtl.Text = mGrid.g_DArray[w_Row].Hacchuugaku.ToString();

                                    this.CalcKin();

                                    mGrid.g_DArray[w_Row].preSiiresakiCD = mGrid.g_DArray[w_Row].SiiresakiCD;
                                }
                                break;

                            case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu:
                                //数量が整数値かどうかチェック
                                int intSu;
                                if (!int.TryParse(wSuu.ToString(), out intSu))
                                {
                                    bbl.ShowMessage("E102");
                                    return;
                                }
                                //0の場合、メッセージ表示
                                if (intSu == 0)
                                {
                                    if (bbl.ShowMessage("Q305") != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                }
                                //発注数が発注ロット単位でない場合、メッセージ表示
                                var mbl = new SKU_BL();
                                var mse = new M_SKU_Entity();
                                mse.AdminNO = mGrid.g_DArray[w_Row].AdminNO;
                                mse.ChangeDate = this.txtHacchuuDate.Text;
                                DataTable dt = mbl.M_SKU_SelectAll(mse);
                                if (dt.Rows.Count > 0)
                                {
                                    int lot = int.Parse(dt.Rows[0]["OrderLot"].ToString());
                                    if (lot > 0 && intSu % int.Parse(dt.Rows[0]["OrderLot"].ToString()) > 0)
                                    {
                                        if (bbl.ShowMessage("Q319") != DialogResult.Yes)
                                        {
                                            return;
                                        }
                                    }
                                }
                                if (mGrid.g_DArray[w_Row].HacchuuSuu != mGrid.g_DArray[w_Row].preHacchuuSuu)
                                {
                                    //入力された場合、金額再計算
                                    mGrid.g_DArray[w_Row].Hacchuugaku = string.Format("{0:#,##0}", wTanka * wSuu);
                                    //mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, w_Row].CellCtl.Text = mGrid.g_DArray[w_Row].Hacchuugaku.ToString();

                                    this.CalcKin();
                                    mGrid.g_DArray[w_Row].preHacchuuSuu = mGrid.g_DArray[w_Row].HacchuuSuu;
                                }
                                break;

                            case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka: //発注単価 
                                                                               //0の場合、メッセージ表示
                                if (wTanka == 0)
                                {
                                    if (bbl.ShowMessage("Q306") != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                }
                                if (mGrid.g_DArray[w_Row].HacchuuTanka != mGrid.g_DArray[w_Row].preHacchuuTanka)
                                {
                                    mGrid.g_DArray[w_Row].Hacchuugaku = string.Format("{0:#,##0}", wTanka * wSuu);
                                    //mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, w_Row].CellCtl.Text = mGrid.g_DArray[w_Row].Hacchuugaku.ToString();

                                    this.CalcKin();
                                    mGrid.g_DArray[w_Row].preHacchuuTanka = mGrid.g_DArray[w_Row].HacchuuTanka;
                                }
                                break;

                            case (int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku: //発注額 
                                this.CalcKin();
                                break;
                        }

                        if (CL == -1)
                            return;
                        //}

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
        private void GridControl_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                GridControl.clsGridCheckBox w_ActCtl;

                w_ActCtl = (GridControl.clsGridCheckBox)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                //どの項目か判別
                int CL = -1;
                string ctlName = string.Empty;
                if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                {
                    ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                }
                else
                {
                    ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));
                }

                switch (ctlName)
                {
                    case "chkEDIFLG":
                        CL = (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG;
                        break;
                    case "chkTaishouFLG":
                        CL = (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG;
                        // 画面の対象フラグを配列にセット
                        mGrid.S_DispToArray(Vsb_Mei_0.Value);
                        this.CalcKin();
                        break;
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region イベント - ファンクションボタン

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            if (this.ActiveControl == this.btnChangeIkkatuHacchuuMode
                || this.ActiveControl == this.CboStoreCD)
            {
                this.BtnChangeIkkatuHacchuuMode_Click(null,null);
                return;
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
                    EsearchKbn kbn = EsearchKbn.Null;
                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }
                    if (w_Col == (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD)
                        kbn = EsearchKbn.Vendor;
                    if (kbn != EsearchKbn.Null)
                        SearchData(kbn, previousCtrl);

                    break;
                case 10:    //F11:表示
                    {
                        //this.ExecDisp();
                        break;
                    }
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
        /// <summary>
        /// F1：終了
        /// </summary>
        protected override void EndSec()
        {
            DeleteExclusive();
            DeleteExclusive_Juchuu();
            this.Close();
        }
        /// <summary>
        /// F9：検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;

                //検索ボタンClick時
                //商品検索
                kbn = EsearchKbn.Vendor;
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
        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Vendor:
                    int w_Row;

                    this.ScSiiresakiCD.ChangeDate = this.txtHacchuuDate.Text;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    //画面より配列セット 
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);
                    break;
                //case EsearchKbn.HacchuuShoriNO:
                    //int w_Row;

                    //this.ScSiiresakiCD.ChangeDate = this.txtHacchuuDate.Text;

                    //if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    //{
                    //    return;
                    //}

                    //w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    ////画面より配列セット 
                    //mGrid.S_DispToArray(Vsb_Mei_0.Value);
                    //break;
            }
        }
        /// <summary>
        /// F11：表示
        /// </summary>
        protected override void ExecDisp()
        {
            if (this.OperationMode == EOperationMode.UPDATE
                || this.OperationMode == EOperationMode.DELETE
                || this.OperationMode == EOperationMode.SHOW)
            {
                if (string.IsNullOrWhiteSpace(this.ScHacchuuNO.TxtCode.Text)
                    && string.IsNullOrWhiteSpace(this.ScHacchuuShoriNO.TxtCode.Text))
                {
                    bbl.ShowMessage("E169");
                    ((Control)(this.ScHacchuuNO.TxtCode)).Focus();
                    return;
                }
            }
            else
            {
                for (int i = 0; i < detailControls.Length; i++)
                {
                    if (CheckDetail(i, false) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }
                }
            }
            CheckData(true);
            CheckGrid(0, 0);

            this.CalcKin();
         }
        /// <summary>
        /// F12：登録
        /// </summary>
        protected override void ExecSec()
        {
            if (OperationMode != EOperationMode.DELETE)
            {
                if (CheckKey((int)EIndex.StoreCD, false) == false)
                {
                    return;
                }

                if (CheckDetail((int)EIndex.HacchuuDate, false) == false)
                {
                    detailControls[(int)EIndex.HacchuuDate].Focus();
                    return;
                }
            }

            if (OperationMode != EOperationMode.DELETE)
            {
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    //m_dataCntが更新有効行数
                    if (mGrid.g_DArray[RW].TaishouFLG == "True")
                    {
                        for (int CL = (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD; CL < (int)ClsGridIkkatuHacchuu.ColNO.COUNT; CL++)
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

                ////各金額項目の再計算必要
                CalcKin();
            }

            var meisaiList = GetMeisaiList();
            //更新処理
            ikkatuHacchuuBL.PRC_IkkatuHacchuuNyuuryoku_Register((short)OperationMode
                                                               , InOperatorCD
                                                               , InPcID
                                                               , CboStoreCD.SelectedValue.ToStringOrNull()
                                                               , ScStaff.TxtCode.Text.ToStringOrNull()
                                                               , txtHacchuuDate.Text.ToStringOrNull()
                                                               , ScHacchuuNO.TxtCode.Text.ToStringOrNull()
                                                               , ScHacchuuShoriNO.TxtCode.Text.ToStringOrNull()
                                                               , this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu ? "0" : "1"
                                                               , meisaiList.ToDataTable());

            if (OperationMode == EOperationMode.DELETE)
            {
                bbl.ShowMessage("I102");
            }
            else
            {
                bbl.ShowMessage("I101");
            }

            //更新後画面クリア
            ChangeOperationMode(base.OperationMode);
        }

        #endregion

        #region イベント - ボタン_Click

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubF11_Click_1(object sender, EventArgs e)
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
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZenSentaku_Click(object sender, EventArgs e)
        {
            if (OperationMode != EOperationMode.INSERT) return;

            //最終行取得(項目がすべて空の行の1つ上の行)
            int maxrow = mGrid.g_MK_Max_Row - 1;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuNO)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SiiresakiCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SiiresakiName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ChokusouFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NetFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NounyuusakiName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NounyuusakiJuusho)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JuchuuNO)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JANCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShouhinName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].BrandName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SizeName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ColorName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuChuuiZikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].EDIFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].MakerShouhinCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].KibouNouki)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShanaiBikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShagaiBikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaniName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuSuu)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuTanka)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].Hacchuugaku)
                    )
                {
                    maxrow = RW - 1;
                    break;
                }
            }

            for (int RW = 0; RW <= maxrow; RW++)
            {
                if (mGrid.g_DArray[RW].IsYuukouTaishouFLG != "True") continue;
                mGrid.g_DArray[RW].TaishouFLG = "True";
            }
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            this.CalcKin();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZenKaijo_Click(object sender, EventArgs e)
        {
            if (OperationMode != EOperationMode.INSERT) return;

            //最終行取得(項目がすべて空の行の1つ上の行)
            int maxrow = mGrid.g_MK_Max_Row - 1;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuNO)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SiiresakiCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SiiresakiName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ChokusouFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NetFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NounyuusakiName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].NounyuusakiJuusho)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JuchuuNO)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JANCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShouhinName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].BrandName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SizeName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ColorName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuChuuiZikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].EDIFLG)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].MakerShouhinCD)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].KibouNouki)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShanaiBikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ShagaiBikou)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TaniName)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuSuu)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].HacchuuTanka)
                    && string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].Hacchuugaku)
                    )
                {
                    maxrow = RW;
                    break;
                }
            }

            for (int RW = 0; RW <= maxrow; RW++)
            {
                if (mGrid.g_DArray[RW].IsYuukouTaishouFLG != "True") continue;
                mGrid.g_DArray[RW].TaishouFLG = "False";
            }
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            this.CalcKin();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeIkkatuHacchuuMode_Click(object sender, EventArgs e)
        {
            if (bbl.ShowMessage("Q005") != DialogResult.Yes) return;

            if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.FAXHacchuu)
            {
                this.lblIkkatuHacchuuMode.Text = "Net発注";
                this.lblIkkatuHacchuuMode.BackColor = System.Drawing.Color.Lime;
                this.ikkatuHacchuuMode = IkkatuHacchuuMode.NetHacchuu;
                this.btnChangeIkkatuHacchuuMode.Text = "FAX発注";
            }
            else
            {
                this.lblIkkatuHacchuuMode.Text = "FAX発注";
                this.lblIkkatuHacchuuMode.BackColor = System.Drawing.Color.Cyan;
                this.ikkatuHacchuuMode = IkkatuHacchuuMode.FAXHacchuu;
                this.btnChangeIkkatuHacchuuMode.Text = "Net発注";
            }

            this.ChangeOperationMode(this.OperationMode);
        }

        #endregion

        #region 初期処理

        /// <summary>
        /// コントロールの配列を作成 ※EIndexと対応させる
        /// </summary>
        private void InitialControlArray()
        {
            keyControls = new Control[] { CboStoreCD };
            keyLabels = new Control[] { };

            detailControls = new Control[] { txtHacchuuDate, ScSiiresakiCD.TxtCode,ScJuchuuStaff.TxtCode, ScHacchuuNO.TxtCode, ScHacchuuShoriNO.TxtCode, ScStaff.TxtCode };
            detailLabels = new Control[] { ScSiiresakiCD,ScJuchuuStaff, ScStaff };
            searchButtons = new Control[] { ScSiiresakiCD.BtnSearch, ScHacchuuNO.BtnSearch, ScHacchuuShoriNO.BtnSearch };

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
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearVendorInfo()
        {
            mOldVendorCD = string.Empty;
            ScSiiresakiCD.LabelText = string.Empty;
            ScHacchuuNO.TxtCode.Text = string.Empty;
            ScHacchuuShoriNO.TxtCode.Text = string.Empty;
        }

        #endregion

        #region チェック処理

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
                case (int)EIndex.StoreCD:
                    if (string.IsNullOrWhiteSpace(CboStoreCD.SelectedValue.ToString()))
                    {
                        bbl.ShowMessage("E102");
                        CboStoreCD.Focus();
                        return false;
                    }
                    if (CboStoreCD.SelectedIndex == -1)
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

            return true;

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
                case (int)EIndex.HacchuuDate:
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
                    if (this.OperationMode == EOperationMode.INSERT)
                    {
                        //入力できる範囲内の日付であること
                        if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                        {
                            //Ｅ１１５
                            bbl.ShowMessage("E115");
                            return false;
                        }

                        //発注日が変更された場合のチェック処理
                        if (mOldIkkatuHacchuuDate != detailControls[index].Text)
                        {
                            for (int i = (int)EIndex.StaffCD; i < (int)EIndex.SiiresakiCD; i++)
                                if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                                    if (!CheckDependsOnHacchuuDate(i, true))
                                        return false;

                            ////明細部JANCDの再チェック
                            //for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                            //{
                            //    if (CheckGrid((int)ClsGridIkkatuHacchuu.ColNO.JanCD, RW, false, true) == false)
                            //    {
                            //        //Focusセット処理
                            //        ERR_FOCUS_GRID_SUB((int)ClsGridIkkatuHacchuu.ColNO.JanCD, RW);
                            //        return false;
                            //    }
                            //}
                            mOldIkkatuHacchuuDate = detailControls[index].Text;
                        }
                    }
                    break;

                case (int)EIndex.StaffCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    if (!CheckDependsOnHacchuuDate(index))
                        return false;

                    break;

                case (int)EIndex.JuchuuStaffCD:

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    if (!CheckDependsOnHacchuuDate(index))
                        return false;

                    CheckData(set);
                    CheckGrid(0, 0);
                    this.CalcKin();

                    break;

                case (int)EIndex.SiiresakiCD:
                    if (detailControls[index].Enabled == false) break;

                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ////Ｅ１０２
                        //bbl.ShowMessage("E102");
                        ////顧客情報ALLクリア
                        //ClearVendorInfo();
                        return true;
                    }
                    if (!CheckDependsOnHacchuuDate(index))
                        return false;

                    break;

                case (int)EIndex.HacchuuNO:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text)) return true;

                    if (OperationMode != EOperationMode.SHOW)
                    {
                        //排他処理
                        bool ret = SelectAndInsertExclusive();
                        if (!ret) return false;
                    }

                    return CheckData(set);

                case (int)EIndex.HacchuuShoriNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text) && string.IsNullOrWhiteSpace(this.ScHacchuuNO.TxtCode.Text))
                    {
                        if (OperationMode == EOperationMode.INSERT)
                        {
                            return true;
                        }
                        else
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text)) return true;
                    if (OperationMode != EOperationMode.SHOW)
                    {
                        //排他処理
                        bool ret2 = SelectAndInsertExclusive();
                        if (!ret2) return false;
                    }
                    return CheckData(set);

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

            w_Ctrl = detailControls[(int)EIndex.SiiresakiCD];

            IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        /// <summary>
        /// 見積日が変更されたときに必要なチェック処理
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDependsOnHacchuuDate(int index, bool ChangeIkkatuHacchuuDate = false)
        {
            switch (index)
            {
                case (int)EIndex.StaffCD:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text
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
                        ScStaff.LabelText = string.Empty;
                        return false;
                    }
                    break;

                case (int)EIndex.JuchuuStaffCD:
                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text
                    };
                    bl = new Staff_BL();
                    ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScJuchuuStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScJuchuuStaff.LabelText = string.Empty;
                        return false;
                    }
                    break;

                case (int)EIndex.SiiresakiCD:
                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        ////仕入先CDが変更されている場合のみ再セット
                        //if (mOldVendorCD != detailControls[index].Text || ChangeIkkatuHacchuuDate)
                        //{
                            if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu)
                            {
                                if (mve.NetFlg != "1")
                                {
                                    bbl.ShowMessage("E171");
                                    return false;
                                }
                            }
                            if (mve.DeleteFlg == "1")
                            {
                                bbl.ShowMessage("E119");
                                return false;
                            }
                        //}
                        this.ScSiiresakiCD.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearVendorInfo();
                        return false;
                    }
                    mOldVendorCD = detailControls[index].Text;    //位置確認
                    break;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="chkAll"></param>
        /// <param name="changeYmd"></param>
        /// <returns></returns>
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            switch (col)
            {
                case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SiiresakiCD))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = mGrid.g_DArray[row].SiiresakiCD,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    bool ret = sbl.M_Vendor_SelectTop1(mve);
                    if (!ret)
                    {
                        //Ｅ101
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    if (ret)
                    {
                        if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu
                            && mve.NetFlg != "1"
                            )
                        {
                            bbl.ShowMessage("E171");
                            return false;
                        }
                    }
                    mGrid.g_DArray[row].SiiresakiName = mve.VendorName;

                    //[M_JANOrderPrice_Select]
                    M_JANOrderPrice_Entity mjop_StoreSiteiAri = new M_JANOrderPrice_Entity
                    {
                        AdminNO = mGrid.g_DArray[row].AdminNO,
                        VendorCD = mGrid.g_DArray[row].SiiresakiCD,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                        StoreCD = CboStoreCD.SelectedValue.ToString()
                    };
                    M_JANOrderPrice_DL mjdl = new M_JANOrderPrice_DL();
                    var dt_MJOP_StoreSiteiAri = mjdl.M_JANOrderPrice_Select(mjop_StoreSiteiAri);

                    //[M_JANOrderPrice_Select]
                    M_JANOrderPrice_Entity mjop_StoreSiteiNasi = new M_JANOrderPrice_Entity
                    {
                        AdminNO = mGrid.g_DArray[row].AdminNO,
                        VendorCD = mGrid.g_DArray[row].SiiresakiCD,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                        StoreCD = "0000"
                    };
                    M_JANOrderPrice_DL mjdl_StoreSiteiNasi = new M_JANOrderPrice_DL();
                    var dt_MJOP_StoreSiteiNasi = mjdl.M_JANOrderPrice_Select(mjop_StoreSiteiNasi);

                    //[M_ItemOrderPrice_Select]
                    M_ItemOrderPrice_Entity miop_StoreSiteiAri = new M_ItemOrderPrice_Entity
                    {
                        MakerItem = mGrid.g_DArray[row].MakerShouhinCD,
                        VendorCD = mGrid.g_DArray[row].SiiresakiCD,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                        StoreCD = CboStoreCD.SelectedValue.ToString()
                    };
                    M_ItemOrderPrice_DL midl_StoreSiteiAri = new M_ItemOrderPrice_DL();
                    var dt_MIOP_StoreSiteiAri = midl_StoreSiteiAri.M_ItemOrderPrice_Select(miop_StoreSiteiAri);

                    //[M_ItemOrderPrice_Select]
                    M_ItemOrderPrice_Entity miop_StoreSiteiNasi = new M_ItemOrderPrice_Entity
                    {
                        MakerItem = mGrid.g_DArray[row].MakerShouhinCD,
                        VendorCD = mGrid.g_DArray[row].SiiresakiCD,
                        ChangeDate = detailControls[(int)EIndex.HacchuuDate].Text,
                        StoreCD = "0000"
                    };
                    M_ItemOrderPrice_DL midl_StoreSiteiNasi = new M_ItemOrderPrice_DL();
                    var dt_MIOP_StoreSiteiNasi = midl_StoreSiteiNasi.M_ItemOrderPrice_Select(miop_StoreSiteiNasi);

                    if (dt_MIOP_StoreSiteiAri.Rows.Count == 0
                        && dt_MJOP_StoreSiteiNasi.Rows.Count == 0
                        && dt_MIOP_StoreSiteiAri.Rows.Count == 0
                        && dt_MJOP_StoreSiteiNasi.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E170");
                        return false;
                    }

                    break;
                case (int)ClsGridIkkatuHacchuu.ColNO.KibouNouki:
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].KibouNouki))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    if (!bbl.CheckDate(mGrid.g_DArray[row].KibouNouki))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    DateTime kibounouki;
                    DateTime.TryParse(mGrid.g_DArray[row].KibouNouki, out kibounouki);
                    DateTime hacchuudate;
                    DateTime.TryParse(txtHacchuuDate.Text, out hacchuudate);
                    if (kibounouki < hacchuudate)
                    {
                        bbl.ShowMessage("E104");
                        return false;
                    }
                    break;
                case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu:
                case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka:
                case (int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku:

                    //各金額項目の再計算必要
                    if (chkAll == false)
                    {
                        CalcKin();
                    }
                    break;
            }

            //配列の内容を画面へセット    
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        #endregion

        #region 排他処理

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SelectAndInsertExclusive()
        {
            if (OperationMode == EOperationMode.SHOW || OperationMode == EOperationMode.INSERT)
                return true;

            DeleteExclusive();

            if (string.IsNullOrWhiteSpace(detailControls[(int)EIndex.HacchuuNO].Text)
                && string.IsNullOrWhiteSpace(detailControls[(int)EIndex.HacchuuShoriNO].Text)
                )
                return true;
            if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.HacchuuNO].Text))
            {
                //排他Tableに該当番号が存在するとError
                //[D_Exclusive]
                var ebl = new Exclusive_BL();
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                    Number = detailControls[(int)EIndex.HacchuuNO].Text,
                    Program = this.InProgramID,
                    Operator = this.InOperatorCD,
                    PC = this.InPcID
                };

                DataTable dt = ebl.D_Exclusive_Select(dee);

                if (dt.Rows.Count > 0)
                {
                    bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                    detailControls[(int)EIndex.HacchuuNO].Focus();
                    return false;
                }
                else
                {
                    bool ret = ebl.D_Exclusive_Insert(dee);
                    mOldIkkatuHacchuuNo = detailControls[(int)EIndex.HacchuuNO].Text;
                    return ret;
                }
            }
            else if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.HacchuuShoriNO].Text))
            {
                var odl = new D_Order_DL();
                var dtOrder = odl.D_Order_SelectByOrderProcessNO(detailControls[(int)EIndex.HacchuuShoriNO].Text);

                List<string> hacchuuNOList = new List<string>();
                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    //排他Tableに該当番号が存在するとError
                    //[D_Exclusive]
                    var ebl = new Exclusive_BL();
                    D_Exclusive_Entity dee = new D_Exclusive_Entity
                    {
                        DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                        Number = dtOrder.Rows[i]["OrderNO"].ToString(),
                        Program = this.InProgramID,
                        Operator = this.InOperatorCD,
                        PC = this.InPcID
                    };

                    DataTable dt = ebl.D_Exclusive_Select(dee);

                    if (dt.Rows.Count > 0)
                    {
                        bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                        detailControls[(int)EIndex.HacchuuShoriNO].Focus();
                        foreach (var item in hacchuuNOList)
                        {
                            mOldIkkatuHacchuuNo = item;
                            this.DeleteExclusive();
                        }
                        return false;
                    }
                    else
                    {
                        bool ret = ebl.D_Exclusive_Insert(dee);
                        mOldIkkatuHacchuuShoriNo = detailControls[(int)EIndex.HacchuuShoriNO].Text;
                        hacchuuNOList.Add(dtOrder.Rows[i]["OrderNO"].ToString());
                        if (!ret)
                        {
                            return ret;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        private bool SelectAndInsertExclusive_Juchuu(DataTable dt)
        {
            DeleteExclusive_Juchuu();

            DataView dtview = new DataView(dt);
            dt = dtview.ToTable(true, "JuchuuNO");

            foreach (DataRow item in dt.Rows)
            {
                //排他Tableに該当番号が存在するとError
                //[D_Exclusive]
                var ebl = new Exclusive_BL();
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Jyuchu,
                    Number = item["JuchuuNO"].ToString(),
                    Program = this.InProgramID,
                    Operator = this.InOperatorCD,
                    PC = this.InPcID
                };

                DataTable dt2 = ebl.D_Exclusive_Select(dee);
                if (dt2.Rows.Count > 0)
                {
                    bbl.ShowMessage("S004", dt2.Rows[0]["Program"].ToString(), dt2.Rows[0]["Operator"].ToString());
                    DeleteExclusive_Juchuu();
                    return false;
                }
                else
                {
                    ebl.D_Exclusive_Insert(dee);
                    mOldJuchuuNOList.Add(dee.Number);
                }
            }

            return true;
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive()
        {
            if (mOldIkkatuHacchuuNo == string.Empty
                && mOldIkkatuHacchuuShoriNo == string.Empty
                )
                return;

            if (mOldIkkatuHacchuuNo != string.Empty)
            {
                Exclusive_BL ebl = new Exclusive_BL();
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                    Number = mOldIkkatuHacchuuNo,
                };

                bool ret = ebl.D_Exclusive_Delete(dee);
            }
            else if (mOldIkkatuHacchuuShoriNo != string.Empty)
            {
                var odl = new D_Order_DL();
                var dtOrder = odl.D_Order_SelectByOrderProcessNO(mOldIkkatuHacchuuShoriNo);

                for (int i = 0; i < dtOrder.Rows.Count; i++)
                {
                    Exclusive_BL ebl = new Exclusive_BL();
                    D_Exclusive_Entity dee = new D_Exclusive_Entity
                    {
                        DataKBN = (int)Exclusive_BL.DataKbn.Hacchu,
                        Number = dtOrder.Rows[i]["OrderNO"].ToString(),
                    };
                    bool ret = ebl.D_Exclusive_Delete(dee);
                }
            }

            mOldIkkatuHacchuuNo = string.Empty;
            mOldIkkatuHacchuuShoriNo = string.Empty;
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive_Juchuu()
        {
            foreach (var item in mOldJuchuuNOList)
            {
                Exclusive_BL ebl = new Exclusive_BL();
                D_Exclusive_Entity dee = new D_Exclusive_Entity
                {
                    DataKBN = (int)Exclusive_BL.DataKbn.Jyuchu,
                    Number = item,
                };

                bool ret = ebl.D_Exclusive_Delete(dee);
            }

            mOldJuchuuNOList = new List<string>();
        }

        #endregion

        #region 計算処理

        /// <summary>
        /// 金額計算
        /// </summary>
        private void CalcKin()
        {
            var l = GetMeisaiList();
            this.lblHacchuugakuSum.Text = string.Format("{0:#,0}", l.Sum(r => r.Hacchuugaku == null ? 0 : decimal.Parse(r.Hacchuugaku)));
        }

        #endregion

        #region クリア処理

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
                    if (ctl == detailControls[(int)EIndex.StaffCD]) continue;
                    ctl.Text = string.Empty;
                }

                foreach (Control ctl in detailLabels)
                {
                    if (ctl == detailLabels[(int)EIndex.StaffName]) continue;
                    ((CKM_SearchControl)ctl).LabelText = string.Empty;
                }
            }

            foreach (Control ctl in detailControls)
            {
                if (ctl == detailControls[(int)EIndex.HacchuuDate])
                {
                    ctl.Text = bbl.GetDate().ToString();
                }
                else if (ctl == detailControls[(int)EIndex.StaffCD])
                {
                }
                else if (ctl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
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
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                {
                    ClearVendorInfo();
                }
                else
                {
                    ctl.Text = string.Empty;
                }
            }

            foreach (Control ctl in detailLabels)
            {
                if (ctl == detailLabels[(int)EIndex.StaffName]) continue;
                ((CKM_SearchControl)ctl).LabelText = string.Empty;
            }

            mOldIkkatuHacchuuDate = string.Empty;
            S_Clear_Grid();   //画面クリア（明細部）

            lblHacchuugakuSum.Text = string.Empty;
            this.chkSaiHacchuu.Checked = false;
        }

        #endregion

        #region 画面制御

        /// <summary>
        /// モード変更時処理
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            //排他処理を解除
            DeleteExclusive();
            DeleteExclusive_Juchuu();

            Scr_Clr(0);

            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            S_BodySeigyo(0, 0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    detailControls[(int)EIndex.HacchuuNO].Focus();
                    break;
            }

        }
        /// <summary>
        /// モードによる入力制御
        /// </summary>
        private void ControlByMode()
        {
            if (OperationMode == EOperationMode.INSERT)
            {
                if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu)
                {
                    this.txtHacchuuDate.ReadOnly = false;
                    this.ScSiiresakiCD.Enabled = true;
                    this.ScJuchuuStaff.Enabled = true;
                    this.ScHacchuuNO.Enabled = false;
                    this.ScHacchuuShoriNO.Enabled = false;
                    this.chkSaiHacchuu.Enabled = false;
                }
                else
                {
                    this.txtHacchuuDate.ReadOnly = false;
                    this.ScSiiresakiCD.Enabled = true;
                    this.ScJuchuuStaff.Enabled = true;
                    this.ScHacchuuNO.Enabled = false;
                    this.ScHacchuuShoriNO.Enabled = false;
                    this.chkSaiHacchuu.Enabled = true;
                }
            }
            else if (OperationMode == EOperationMode.UPDATE)
            {
                if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu)
                {
                    this.txtHacchuuDate.ReadOnly = false;
                    this.ScSiiresakiCD.Enabled = false;
                    this.ScJuchuuStaff.Enabled = false;
                    this.ScHacchuuNO.Enabled = true;
                    this.ScHacchuuShoriNO.Enabled = true;
                    this.ScSiiresakiCD.Clear();
                    this.chkSaiHacchuu.Enabled = false;
                }
                else
                {
                    this.txtHacchuuDate.ReadOnly = false;
                    this.ScSiiresakiCD.Enabled = false;
                    this.ScJuchuuStaff.Enabled = false;
                    this.ScHacchuuNO.Enabled = true;
                    this.ScHacchuuShoriNO.Enabled = true;
                    this.ScSiiresakiCD.Clear();
                    this.chkSaiHacchuu.Enabled = false;
                }
            }
            else if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
            {
                if (this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu)
                {
                    this.txtHacchuuDate.ReadOnly = true;
                    this.ScSiiresakiCD.Enabled = false;
                    this.ScJuchuuStaff.Enabled = false;
                    this.ScHacchuuNO.Enabled = true;
                    this.ScHacchuuShoriNO.Enabled = true;
                    this.ScSiiresakiCD.Clear();
                    this.chkSaiHacchuu.Enabled = false;
                }
                else
                {
                    this.txtHacchuuDate.ReadOnly = true;
                    this.ScSiiresakiCD.Enabled = false;
                    this.ScJuchuuStaff.Enabled = false;
                    this.ScHacchuuNO.Enabled = true;
                    this.ScHacchuuShoriNO.Enabled = true;
                    this.ScSiiresakiCD.Clear();
                    this.chkSaiHacchuu.Enabled = false;
                }
            }
        }

        #endregion

        #region データ取得

        /// <summary>
        /// 明細情報をリストにする
        /// </summary>
        /// <returns></returns>
        private List<MeisaiRecord> GetMeisaiList()
        {
            List<MeisaiRecord> l = new List<MeisaiRecord>();

            for (int i = 0; i <= mGrid.g_MK_Max_Row - 1; i++)
            {
                if (mGrid.g_DArray[i].TaishouFLG == null || mGrid.g_DArray[i].TaishouFLG == "False") continue;
                MeisaiRecord r = GetMeisaiRecord(i);
                l.Add(r);
            }
            return l;
        }
        /// <summary>
        /// 明細レコード作成
        /// </summary>
        /// <param name="i"></param>
        /// <returns>行番号</returns>
        private MeisaiRecord GetMeisaiRecord(int i)
        {
            var r = new MeisaiRecord();
            r.GyouNO = mGrid.g_DArray[i].GyouNO.ToStringOrNull();
            r.HacchuuNO = mGrid.g_DArray[i].HacchuuNO.ToStringOrNull();
            r.SiiresakiCD = mGrid.g_DArray[i].SiiresakiCD.ToStringOrNull();
            r.SiiresakiName = mGrid.g_DArray[i].SiiresakiName.ToStringOrNull();
            r.ChokusouFLG = mGrid.g_DArray[i].ChokusouFLG.ToStringOrNull();
            r.NetFLG = mGrid.g_DArray[i].NetFLG.ToStringOrNull();
            r.NounyuusakiName = mGrid.g_DArray[i].NounyuusakiName.ToStringOrNull();
            r.NounyuusakiJuusho = mGrid.g_DArray[i].NounyuusakiJuusho.ToStringOrNull();
            r.JuchuuNO = mGrid.g_DArray[i].JuchuuNO.ToStringOrNull();
            r.SKUCD = mGrid.g_DArray[i].SKUCD.ToStringOrNull();
            r.JANCD = mGrid.g_DArray[i].JANCD.ToStringOrNull();
            r.ShouhinName = mGrid.g_DArray[i].ShouhinName.ToStringOrNull();
            r.BrandName = mGrid.g_DArray[i].BrandName.ToStringOrNull();
            r.SizeName = mGrid.g_DArray[i].SizeName.ToStringOrNull();
            r.ColorName = mGrid.g_DArray[i].ColorName.ToStringOrNull();
            r.HacchuuChuuiZikou = mGrid.g_DArray[i].HacchuuChuuiZikou.ToStringOrNull();
            r.EDIFLG = mGrid.g_DArray[i].EDIFLG.ToStringOrNull();
            r.MakerShouhinCD = mGrid.g_DArray[i].MakerShouhinCD.ToStringOrNull();
            r.KibouNouki = mGrid.g_DArray[i].KibouNouki.ToStringOrNull();
            r.ShanaiBikou = mGrid.g_DArray[i].ShanaiBikou.ToStringOrNull();
            r.ShagaiBikou = mGrid.g_DArray[i].ShagaiBikou.ToStringOrNull();
            r.TaniName = mGrid.g_DArray[i].TaniName.ToStringOrNull();
            r.HacchuuSuu = mGrid.g_DArray[i].HacchuuSuu.ToStringOrNull();
            r.HacchuuTanka = mGrid.g_DArray[i].HacchuuTanka.ToStringOrNull();
            r.Hacchuugaku = mGrid.g_DArray[i].Hacchuugaku.ToStringOrNull();
            r.TaishouFLG = mGrid.g_DArray[i].TaishouFLG.ToStringOrNull();
            r.NounyuusakiYuubinNO1 = mGrid.g_DArray[i].NounyuusakiYuubinNO1.ToStringOrNull();
            r.NounyuusakiYuubinNO2 = mGrid.g_DArray[i].NounyuusakiYuubinNO2.ToStringOrNull();
            r.NounyuusakiJuusho1 = mGrid.g_DArray[i].NounyuusakiJuusho1.ToStringOrNull();
            r.NounyuusakiJuusho2 = mGrid.g_DArray[i].NounyuusakiJuusho2.ToStringOrNull();
            r.NounyuusakiMailAddress = mGrid.g_DArray[i].NounyuusakiMailAddress.ToStringOrNull();
            r.NounyuusakiTELNO = mGrid.g_DArray[i].NounyuusakiTELNO.ToStringOrNull();
            r.NounyuusakiFAXNO = mGrid.g_DArray[i].NounyuusakiFAXNO.ToStringOrNull();
            r.SoukoCD = mGrid.g_DArray[i].SoukoCD.ToStringOrNull();
            r.TaxRate = mGrid.g_DArray[i].TaxRate.ToStringOrNull();
            r.JuchuuRows = mGrid.g_DArray[i].JuchuuRows.ToStringOrNull();
            r.VariousFLG = mGrid.g_DArray[i].VariousFLG.ToStringOrNull();
            r.AdminNO = mGrid.g_DArray[i].AdminNO.ToStringOrNull();
            r.SKUName = mGrid.g_DArray[i].SKUName.ToStringOrNull();
            r.Teika = mGrid.g_DArray[i].Teika.ToStringOrNull();
            r.Kakeritu = mGrid.g_DArray[i].Kakeritu.ToStringOrNull();
            r.HacchuuShouhizeigaku = mGrid.g_DArray[i].HacchuuShouhizeigaku.ToStringOrNull();
            r.TaniCD = mGrid.g_DArray[i].TaniCD.ToStringOrNull();
            r.OrderRows = mGrid.g_DArray[i].OrderRows.ToStringOrNull();
            r.IsYuukouTaishouFLG = mGrid.g_DArray[i].IsYuukouTaishouFLG.ToStringOrNull();

            return r;
        }
        /// <summary>
        /// パラメータ設定
        /// </summary>
        /// <param name="dt"></param>
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("IkkatuHacchuuRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("SKUNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("ColorName", typeof(string));
            dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("SetKBN", typeof(int));
            dt.Columns.Add("IkkatuHacchuuSuu", typeof(int));
            dt.Columns.Add("IkkatuHacchuuUnitPrice", typeof(decimal));
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("IkkatuHacchuuGaku", typeof(decimal));
            dt.Columns.Add("IkkatuHacchuuHontaiGaku", typeof(decimal));
            dt.Columns.Add("IkkatuHacchuuTax", typeof(decimal));
            dt.Columns.Add("IkkatuHacchuuTaxRitsu", typeof(decimal));
            dt.Columns.Add("CostUnitPrice", typeof(decimal));
            dt.Columns.Add("CostGaku", typeof(decimal));
            dt.Columns.Add("ProfitGaku", typeof(decimal));
            dt.Columns.Add("CommentInStore", typeof(string));
            dt.Columns.Add("CommentOutStore", typeof(string));
            dt.Columns.Add("IndividualClientName", typeof(string));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                ////m_dataCntが更新有効行数
                //if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JanCD) == false
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUName) == false
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].ColorName) == false
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SizeName) == false
                //    || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriSuu) != 0
                //    || bbl.Z_Set(mGrid.g_DArray[RW].MitsumoriUnitPrice) != 0
                //    || bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice) != 0
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentOutStore) == false
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].IndividualClientName) == false
                //    || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CommentInStore) == false
                //    )
                //{
                //    string setkbn = "1";
                //    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SetKBN))
                //        setkbn = "0";

                //    dt.Rows.Add(rowNo
                //        , rowNo
                //        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                //        , mGrid.g_DArray[RW].SKUCD == string.Empty ? null : mGrid.g_DArray[RW].SKUCD
                //        , mGrid.g_DArray[RW].JanCD == string.Empty ? null : mGrid.g_DArray[RW].JanCD
                //        , mGrid.g_DArray[RW].SKUName == string.Empty ? null : mGrid.g_DArray[RW].SKUName
                //        , mGrid.g_DArray[RW].ColorName == string.Empty ? null : mGrid.g_DArray[RW].ColorName
                //        , mGrid.g_DArray[RW].SizeName == string.Empty ? null : mGrid.g_DArray[RW].SizeName
                //        , setkbn
                //        , bbl.Z_Set(mGrid.g_DArray[RW].MiturmoriSuu)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].MiturmoriUnitPrice)
                //        , mGrid.g_DArray[RW].TaniCD == string.Empty ? null : mGrid.g_DArray[RW].TaniCD
                //        , bbl.Z_Set(mGrid.g_DArray[RW].MiturmoriGaku)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].MiturmoriHontaiGaku)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].MiturmoriTax) + bbl.Z_Set(mGrid.g_DArray[RW].KeigenTax)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].TaxRate)     //税率
                //        , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].CostGaku)
                //        , bbl.Z_Set(mGrid.g_DArray[RW].ProfitGaku)
                //        , mGrid.g_DArray[RW].CommentInStore == string.Empty ? null : mGrid.g_DArray[RW].CommentInStore
                //        , mGrid.g_DArray[RW].CommentOutStore == string.Empty ? null : mGrid.g_DArray[RW].CommentOutStore
                //        , mGrid.g_DArray[RW].IndividualClientName == string.Empty ? null : mGrid.g_DArray[RW].IndividualClientName
                //        //, mGrid.g_DArray[RW].Update
                //        );

                //    rowNo++;
                //}
            }

            return dt;
        }
        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckData(bool set)
        {
            if (!set) return true;

            //[D_IkkatuHacchuu_SelectData]
            dOrderEntity = new D_Order_Entity();
            DataTable dt = new DataTable();
            if (OperationMode == EOperationMode.INSERT)
            {
                dt = ikkatuHacchuuBL.PRC_IkkatuHacchuuNyuuryoku_SelectData(this.ikkatuHacchuuMode == IkkatuHacchuuMode.NetHacchuu ? "0" : "1"
                                                                         , txtHacchuuDate.Text
                                                                         , ScSiiresakiCD.TxtCode.Text.ToStringOrNull()
                                                                         , ScJuchuuStaff.TxtCode.Text.ToStringOrNull()
                                                                         , CboStoreCD.SelectedValue.ToStringOrNull()
                                                                         , chkSaiHacchuu.Checked == true ? "1" : "0"
                                                                         );
            }
            else if (OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
            {
                dt = ikkatuHacchuuBL.PRC_IkkatuHacchuuNyuuryoku_SelectByOrderNO(ScHacchuuNO.TxtCode.Text
                                                                              , ScHacchuuShoriNO.TxtCode.Text);
            }

            //以下の条件で見積入力ーが存在しなければエラー (Error if record does not exist)「登録されていない見積番号」
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E128");
                Scr_Clr(1);
                if (OperationMode == EOperationMode.INSERT)
                {
                    this.ScSiiresakiCD.Focus();
                }
                else
                {
                    this.ScHacchuuNO.Focus();
                }
                return false;
            }
            else
            {
                if (OperationMode == EOperationMode.INSERT)
                {
                    //排他チェック
                    if (!this.SelectAndInsertExclusive_Juchuu(dt))
                    {
                        this.previousCtrl.Focus();
                        return false;
                    }
                }

                if (dt.Rows[0]["OrderWayKBN"].ToString() == "1")
                {
                    this.lblIkkatuHacchuuMode.Text = "Net発注";
                    this.lblIkkatuHacchuuMode.BackColor = System.Drawing.Color.Lime;
                    this.ikkatuHacchuuMode = IkkatuHacchuuMode.NetHacchuu;
                    this.btnChangeIkkatuHacchuuMode.Text = "FAX発注";
                }
                else
                {
                    this.lblIkkatuHacchuuMode.Text = "FAX発注";
                    this.lblIkkatuHacchuuMode.BackColor = System.Drawing.Color.Cyan;
                    this.ikkatuHacchuuMode = IkkatuHacchuuMode.FAXHacchuu;
                    this.btnChangeIkkatuHacchuuMode.Text = "Net発注";
                }

                ////DeleteDateTime 「削除された見積番号」
                //if (!string.IsNullOrWhiteSpace(dt.Rows[0]["DeleteDateTime"].ToString()))
                //{
                //    bbl.ShowMessage("E140");
                //    Scr_Clr(1);
                //    previousCtrl.Focus();
                //    return false;
                //}

                ////権限がない場合（以下のSelectができない場合）Error　「権限のない見積番号」
                //if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                //{
                //    bbl.ShowMessage("E139");
                //    Scr_Clr(1);
                //    previousCtrl.Focus();
                //    return false;
                //}

                //画面セットなしの場合、処理正常終了
                if (set == false)
                {
                    return true;
                }

                S_Clear_Grid();   //画面クリア（明細部）

                //明細にデータをセット
                int i = 0;

                foreach (DataRow row in dt.Rows)
                {
                    //使用可能行数を超えた場合エラー
                    if (i > m_EnableCnt - 1)
                    {
                        MessageBox.Show("表示可能行数を超えています", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        return false;
                    }

                    mGrid.g_DArray[i].HacchuuNO = row["HacchuuNO"].ToString();
                    mGrid.g_DArray[i].SiiresakiCD = row["SiiresakiCD"].ToString();
                    mGrid.g_DArray[i].SiiresakiName = row["SiiresakiName"].ToString();
                    mGrid.g_DArray[i].ChokusouFLG = row["ChokusouFLG"].ToString();
                    mGrid.g_DArray[i].NetFLG = row["NetFLG"].ToString();
                    mGrid.g_DArray[i].NounyuusakiName = row["NounyuusakiName"].ToString();
                    mGrid.g_DArray[i].NounyuusakiJuusho = row["NounyuusakiJuusho"].ToString();
                    mGrid.g_DArray[i].JuchuuNO = row["JuchuuNO"].ToString();
                    mGrid.g_DArray[i].SKUCD = row["SKUCD"].ToString();
                    mGrid.g_DArray[i].JANCD = row["JANCD"].ToString();
                    mGrid.g_DArray[i].ShouhinName = row["ShouhinName"].ToString();
                    mGrid.g_DArray[i].BrandName = row["BrandName"].ToString();
                    mGrid.g_DArray[i].SizeName = row["SizeName"].ToString();
                    mGrid.g_DArray[i].ColorName = row["ColorName"].ToString();
                    mGrid.g_DArray[i].HacchuuChuuiZikou = row["HacchuuChuuiZikou"].ToString();
                    mGrid.g_DArray[i].EDIFLG = row["EDIFLG"].ToString();
                    mGrid.g_DArray[i].MakerShouhinCD = row["MakerShouhinCD"].ToString();
                    mGrid.g_DArray[i].KibouNouki = row["KibouNouki"].ToString();
                    mGrid.g_DArray[i].ShanaiBikou = row["ShanaiBikou"].ToString();
                    mGrid.g_DArray[i].ShagaiBikou = row["ShagaiBikou"].ToString();
                    mGrid.g_DArray[i].TaniName = row["TaniName"].ToString();
                    mGrid.g_DArray[i].HacchuuSuu = row["HacchuuSuu"].ToString();
                    mGrid.g_DArray[i].HacchuuTanka = row["HacchuuTanka"].ToString();
                    mGrid.g_DArray[i].Hacchuugaku = row["Hacchuugaku"].ToString();
                    mGrid.g_DArray[i].TaishouFLG = row["TaishouFLG"].ToString();
                    mGrid.g_DArray[i].NounyuusakiYuubinNO1 = row["NounyuusakiYuubinNO1"].ToString();
                    mGrid.g_DArray[i].NounyuusakiYuubinNO2 = row["NounyuusakiYuubinNO2"].ToString();
                    mGrid.g_DArray[i].NounyuusakiJuusho1 = row["NounyuusakiJuusho1"].ToString();
                    mGrid.g_DArray[i].NounyuusakiJuusho2 = row["NounyuusakiJuusho2"].ToString();
                    mGrid.g_DArray[i].NounyuusakiMailAddress = row["NounyuusakiMailAddress"].ToString();
                    mGrid.g_DArray[i].NounyuusakiTELNO = row["NounyuusakiTELNO"].ToString();
                    mGrid.g_DArray[i].NounyuusakiFAXNO = row["NounyuusakiFAXNO"].ToString();
                    mGrid.g_DArray[i].SoukoCD = row["SoukoCD"].ToString();
                    mGrid.g_DArray[i].TaxRate = row["TaxRate"].ToString();
                    mGrid.g_DArray[i].JuchuuRows = row["JuchuuRows"].ToString();
                    mGrid.g_DArray[i].VariousFLG = row["VariousFLG"].ToString();
                    mGrid.g_DArray[i].AdminNO = row["AdminNO"].ToString();
                    mGrid.g_DArray[i].SKUName = row["SKUName"].ToString();
                    mGrid.g_DArray[i].Teika = row["Teika"].ToString();
                    mGrid.g_DArray[i].Kakeritu = row["Kakeritu"].ToString();
                    mGrid.g_DArray[i].HacchuuShouhizeigaku = row["HacchuuShouhizeigaku"].ToString();
                    mGrid.g_DArray[i].TaniCD = row["TaniCD"].ToString();
                    mGrid.g_DArray[i].OrderRows = row["OrderRows"].ToString();
                    mGrid.g_DArray[i].IsYuukouTaishouFLG = row["IsYuukouTaishouFLG"].ToString();
                    mGrid.g_DArray[i].preSiiresakiCD = mGrid.g_DArray[i].SiiresakiCD;
                    mGrid.g_DArray[i].preHacchuuSuu = mGrid.g_DArray[i].HacchuuSuu;
                    mGrid.g_DArray[i].preHacchuuTanka = mGrid.g_DArray[i].HacchuuTanka;

                    m_dataCnt = i + 1;
                    Grid_NotFocus((int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, i);
                    i++;
                }
                 
                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
            }

            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                S_BodySeigyo(1, 0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG);
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG);
            }
            else
            {
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                S_BodySeigyo(2, 0);

                previousCtrl.Focus();
            }

            this.CalcKin();

            return true;
        }

        #endregion

        #region データ更新



        #endregion

        #region -- 明細部をグリッドのように扱うための関数 ↓--------------------

        /// <summary>
        /// 明細部初期化
        /// 画面のコントロールを配列にセット
        /// 固定色/フォーカスセット不可(クリックでのみフォーカスセット)/使用可 等の初期設定
        /// Tab等によるフォーカス移動順の設定
        /// 行の色の繰り返し単位と色を設定
        /// </summary>
        private void S_SetInit_Grid()
        {
            if (ClsGridIkkatuHacchuu.gc_P_GYO <= ClsGridIkkatuHacchuu.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridIkkatuHacchuu.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridIkkatuHacchuu.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridIkkatuHacchuu.gc_P_GYO;
            //    m_EnableCnt = ClsGridIkkatuHacchuu.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridIkkatuHacchuu.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridIkkatuHacchuu.gc_MaxCL;

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

            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
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
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            ((GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).Click += new System.EventHandler(GridControl_Click);
                        }
                    }

                    switch (w_CtlCol)
                    {
                        case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD:
                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu:
                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka:
                        case (int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku:
                            //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                            //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).CKM_Reqired = true;

                            break;
                    }
                }
            }

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];

            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridIkkatuHacchuu.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridIkkatuHacchuu.ColNO.GyouNO; i <= (int)ClsGridIkkatuHacchuu.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridIkkatuHacchuu.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        break;
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.IkkatuHacchuuHontaiGaku:
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.IkkatuHacchuuUnitPrice:
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.IkkatuHacchuuGaku:
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.CostUnitPrice:
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.CostGaku:
                    //    case (int)ClsGridIkkatuHacchuu.ColNO.ProfitGaku:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        break;
                    //}

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }

            // TabStopﾌﾟﾛﾊﾟﾃｨの初期状態をセット
            Set_GridTabStop(false);
        }
        /// <summary>
        /// 明細部のコントロールを配列にセット
        /// </summary>
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 0].CellCtl = txtGyouNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 0].CellCtl = txtHacchuuNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 0].CellCtl = scSiiresakiCD_0.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 0].CellCtl = txtSiiresakiName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 0].CellCtl = txtChokusouFLG_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 0].CellCtl = txtNetFLG_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 0].CellCtl = txtNounyuusakiName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 0].CellCtl = txtNounyuusakiJuusho_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 0].CellCtl = txtJuchuuNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 0].CellCtl = txtSKUCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 0].CellCtl = txtJANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 0].CellCtl = txtShouhinName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 0].CellCtl = txtBrandName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 0].CellCtl = txtSizeName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 0].CellCtl = txtColorName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 0].CellCtl = txtHacchuuChuuiZikou_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 0].CellCtl = chkEDIFLG_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 0].CellCtl = txtMakerShouhinCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 0].CellCtl = txtKibouNouki_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 0].CellCtl = txtShanaiBikou_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 0].CellCtl = txtShagaiBikou_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 0].CellCtl = txtTaniName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 0].CellCtl = txtHacchuuSuu_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 0].CellCtl = txtHacchuuTanka_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 0].CellCtl = txtHacchuugaku_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 0].CellCtl = chkTaishouFLG_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 0].CellCtl = txtNounyuusakiYuubinNO1_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 0].CellCtl = txtNounyuusakiYuubinNO2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 0].CellCtl = txtNounyuusakiJuusho1_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 0].CellCtl = txtNounyuusakiJuusho2_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 0].CellCtl = txtNounyuusakiMailAddress_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 0].CellCtl = txtNounyuusakiTELNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 0].CellCtl = txtNounyuusakiFAXNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 0].CellCtl = txtSoukoCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 0].CellCtl = txtTaxRate_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 0].CellCtl = txtJuchuuRows_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 0].CellCtl = txtVariousFLG_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 0].CellCtl = txtAdminNO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 0].CellCtl = txtSKUName_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 0].CellCtl = txtTeika_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 0].CellCtl = txtKakeritu_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 0].CellCtl = txtHacchuuShouhizeigaku_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 0].CellCtl = txtTaniCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 0].CellCtl = txtOrderRows_0;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 0].CellCtl = txtIsYuukouTaishouFLG_0;

            // 2行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 1].CellCtl = txtGyouNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 1].CellCtl = txtHacchuuNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 1].CellCtl = scSiiresakiCD_1.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 1].CellCtl = txtSiiresakiName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 1].CellCtl = txtChokusouFLG_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 1].CellCtl = txtNetFLG_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 1].CellCtl = txtNounyuusakiName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 1].CellCtl = txtNounyuusakiJuusho_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 1].CellCtl = txtJuchuuNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 1].CellCtl = txtSKUCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 1].CellCtl = txtJANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 1].CellCtl = txtShouhinName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 1].CellCtl = txtBrandName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 1].CellCtl = txtSizeName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 1].CellCtl = txtColorName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 1].CellCtl = txtHacchuuChuuiZikou_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 1].CellCtl = chkEDIFLG_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 1].CellCtl = txtMakerShouhinCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 1].CellCtl = txtKibouNouki_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 1].CellCtl = txtShanaiBikou_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 1].CellCtl = txtShagaiBikou_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 1].CellCtl = txtTaniName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 1].CellCtl = txtHacchuuSuu_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 1].CellCtl = txtHacchuuTanka_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 1].CellCtl = txtHacchuugaku_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 1].CellCtl = chkTaishouFLG_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 1].CellCtl = txtNounyuusakiYuubinNO1_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 1].CellCtl = txtNounyuusakiYuubinNO2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 1].CellCtl = txtNounyuusakiJuusho1_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 1].CellCtl = txtNounyuusakiJuusho2_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 1].CellCtl = txtNounyuusakiMailAddress_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 1].CellCtl = txtNounyuusakiTELNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 1].CellCtl = txtNounyuusakiFAXNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 1].CellCtl = txtSoukoCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 1].CellCtl = txtTaxRate_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 1].CellCtl = txtJuchuuRows_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 1].CellCtl = txtVariousFLG_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 1].CellCtl = txtAdminNO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 1].CellCtl = txtSKUName_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 1].CellCtl = txtTeika_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 1].CellCtl = txtKakeritu_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 1].CellCtl = txtHacchuuShouhizeigaku_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 1].CellCtl = txtTaniCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 1].CellCtl = txtOrderRows_1;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 1].CellCtl = txtIsYuukouTaishouFLG_1;

            // 3行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 2].CellCtl = txtGyouNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 2].CellCtl = txtHacchuuNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 2].CellCtl = scSiiresakiCD_2.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 2].CellCtl = txtSiiresakiName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 2].CellCtl = txtChokusouFLG_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 2].CellCtl = txtNetFLG_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 2].CellCtl = txtNounyuusakiName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 2].CellCtl = txtNounyuusakiJuusho_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 2].CellCtl = txtJuchuuNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 2].CellCtl = txtSKUCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 2].CellCtl = txtJANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 2].CellCtl = txtShouhinName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 2].CellCtl = txtBrandName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 2].CellCtl = txtSizeName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 2].CellCtl = txtColorName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 2].CellCtl = txtHacchuuChuuiZikou_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 2].CellCtl = chkEDIFLG_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 2].CellCtl = txtMakerShouhinCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 2].CellCtl = txtKibouNouki_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 2].CellCtl = txtShanaiBikou_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 2].CellCtl = txtShagaiBikou_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 2].CellCtl = txtTaniName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 2].CellCtl = txtHacchuuSuu_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 2].CellCtl = txtHacchuuTanka_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 2].CellCtl = txtHacchuugaku_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 2].CellCtl = chkTaishouFLG_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 2].CellCtl = txtNounyuusakiYuubinNO1_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 2].CellCtl = txtNounyuusakiYuubinNO2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 2].CellCtl = txtNounyuusakiJuusho1_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 2].CellCtl = txtNounyuusakiJuusho2_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 2].CellCtl = txtNounyuusakiMailAddress_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 2].CellCtl = txtNounyuusakiTELNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 2].CellCtl = txtNounyuusakiFAXNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 2].CellCtl = txtSoukoCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 2].CellCtl = txtTaxRate_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 2].CellCtl = txtJuchuuRows_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 2].CellCtl = txtVariousFLG_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 2].CellCtl = txtAdminNO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 2].CellCtl = txtSKUName_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 2].CellCtl = txtTeika_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 2].CellCtl = txtKakeritu_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 2].CellCtl = txtHacchuuShouhizeigaku_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 2].CellCtl = txtTaniCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 2].CellCtl = txtOrderRows_2;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 2].CellCtl = txtIsYuukouTaishouFLG_2;

            // 4行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 3].CellCtl = txtGyouNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 3].CellCtl = txtHacchuuNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 3].CellCtl = scSiiresakiCD_3.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 3].CellCtl = txtSiiresakiName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 3].CellCtl = txtChokusouFLG_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 3].CellCtl = txtNetFLG_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 3].CellCtl = txtNounyuusakiName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 3].CellCtl = txtNounyuusakiJuusho_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 3].CellCtl = txtJuchuuNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 3].CellCtl = txtSKUCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 3].CellCtl = txtJANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 3].CellCtl = txtShouhinName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 3].CellCtl = txtBrandName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 3].CellCtl = txtSizeName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 3].CellCtl = txtColorName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 3].CellCtl = txtHacchuuChuuiZikou_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 3].CellCtl = chkEDIFLG_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 3].CellCtl = txtMakerShouhinCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 3].CellCtl = txtKibouNouki_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 3].CellCtl = txtShanaiBikou_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 3].CellCtl = txtShagaiBikou_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 3].CellCtl = txtTaniName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 3].CellCtl = txtHacchuuSuu_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 3].CellCtl = txtHacchuuTanka_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 3].CellCtl = txtHacchuugaku_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 3].CellCtl = chkTaishouFLG_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 3].CellCtl = txtNounyuusakiYuubinNO1_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 3].CellCtl = txtNounyuusakiYuubinNO2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 3].CellCtl = txtNounyuusakiJuusho1_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 3].CellCtl = txtNounyuusakiJuusho2_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 3].CellCtl = txtNounyuusakiMailAddress_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 3].CellCtl = txtNounyuusakiTELNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 3].CellCtl = txtNounyuusakiFAXNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 3].CellCtl = txtSoukoCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 3].CellCtl = txtTaxRate_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 3].CellCtl = txtJuchuuRows_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 3].CellCtl = txtVariousFLG_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 3].CellCtl = txtAdminNO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 3].CellCtl = txtSKUName_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 3].CellCtl = txtTeika_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 3].CellCtl = txtKakeritu_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 3].CellCtl = txtHacchuuShouhizeigaku_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 3].CellCtl = txtTaniCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 3].CellCtl = txtOrderRows_3;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 3].CellCtl = txtIsYuukouTaishouFLG_3;

            // 5行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 4].CellCtl = txtGyouNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 4].CellCtl = txtHacchuuNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 4].CellCtl = scSiiresakiCD_4.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 4].CellCtl = txtSiiresakiName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 4].CellCtl = txtChokusouFLG_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 4].CellCtl = txtNetFLG_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 4].CellCtl = txtNounyuusakiName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 4].CellCtl = txtNounyuusakiJuusho_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 4].CellCtl = txtJuchuuNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 4].CellCtl = txtSKUCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 4].CellCtl = txtJANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 4].CellCtl = txtShouhinName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 4].CellCtl = txtBrandName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 4].CellCtl = txtSizeName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 4].CellCtl = txtColorName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 4].CellCtl = txtHacchuuChuuiZikou_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 4].CellCtl = chkEDIFLG_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 4].CellCtl = txtMakerShouhinCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 4].CellCtl = txtKibouNouki_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 4].CellCtl = txtShanaiBikou_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 4].CellCtl = txtShagaiBikou_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 4].CellCtl = txtTaniName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 4].CellCtl = txtHacchuuSuu_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 4].CellCtl = txtHacchuuTanka_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 4].CellCtl = txtHacchuugaku_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 4].CellCtl = chkTaishouFLG_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 4].CellCtl = txtNounyuusakiYuubinNO1_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 4].CellCtl = txtNounyuusakiYuubinNO2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 4].CellCtl = txtNounyuusakiJuusho1_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 4].CellCtl = txtNounyuusakiJuusho2_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 4].CellCtl = txtNounyuusakiMailAddress_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 4].CellCtl = txtNounyuusakiTELNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 4].CellCtl = txtNounyuusakiFAXNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 4].CellCtl = txtSoukoCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 4].CellCtl = txtTaxRate_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 4].CellCtl = txtJuchuuRows_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 4].CellCtl = txtVariousFLG_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 4].CellCtl = txtAdminNO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 4].CellCtl = txtSKUName_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 4].CellCtl = txtTeika_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 4].CellCtl = txtKakeritu_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 4].CellCtl = txtHacchuuShouhizeigaku_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 4].CellCtl = txtTaniCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 4].CellCtl = txtOrderRows_4;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 4].CellCtl = txtIsYuukouTaishouFLG_4;

            // 6行目
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.GyouNO, 5].CellCtl = txtGyouNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO, 5].CellCtl = txtHacchuuNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD, 5].CellCtl = scSiiresakiCD_5.TxtCode;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName, 5].CellCtl = txtSiiresakiName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG, 5].CellCtl = txtChokusouFLG_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NetFLG, 5].CellCtl = txtNetFLG_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName, 5].CellCtl = txtNounyuusakiName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho, 5].CellCtl = txtNounyuusakiJuusho_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO, 5].CellCtl = txtJuchuuNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUCD, 5].CellCtl = txtSKUCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JANCD, 5].CellCtl = txtJANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShouhinName, 5].CellCtl = txtShouhinName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.BrandName, 5].CellCtl = txtBrandName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SizeName, 5].CellCtl = txtSizeName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ColorName, 5].CellCtl = txtColorName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou, 5].CellCtl = txtHacchuuChuuiZikou_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.EDIFLG, 5].CellCtl = chkEDIFLG_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD, 5].CellCtl = txtMakerShouhinCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.KibouNouki, 5].CellCtl = txtKibouNouki_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou, 5].CellCtl = txtShanaiBikou_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou, 5].CellCtl = txtShagaiBikou_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniName, 5].CellCtl = txtTaniName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu, 5].CellCtl = txtHacchuuSuu_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka, 5].CellCtl = txtHacchuuTanka_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku, 5].CellCtl = txtHacchuugaku_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG, 5].CellCtl = chkTaishouFLG_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1, 5].CellCtl = txtNounyuusakiYuubinNO1_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2, 5].CellCtl = txtNounyuusakiYuubinNO2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1, 5].CellCtl = txtNounyuusakiJuusho1_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2, 5].CellCtl = txtNounyuusakiJuusho2_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress, 5].CellCtl = txtNounyuusakiMailAddress_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO, 5].CellCtl = txtNounyuusakiTELNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO, 5].CellCtl = txtNounyuusakiFAXNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SoukoCD, 5].CellCtl = txtSoukoCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaxRate, 5].CellCtl = txtTaxRate_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows, 5].CellCtl = txtJuchuuRows_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.VariousFLG, 5].CellCtl = txtVariousFLG_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.AdminNO, 5].CellCtl = txtAdminNO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.SKUName, 5].CellCtl = txtSKUName_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Teika, 5].CellCtl = txtTeika_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.Kakeritu, 5].CellCtl = txtKakeritu_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku, 5].CellCtl = txtHacchuuShouhizeigaku_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.TaniCD, 5].CellCtl = txtTaniCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.OrderRows, 5].CellCtl = txtOrderRows_5;
            mGrid.g_MK_Ctrl[(int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG, 5].CellCtl = txtIsYuukouTaishouFLG_5;
        }
        /// <summary>
        /// 明細部 Tab の処理
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pErrSet"></param>
        /// <param name="pMotoControl"></param>
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        /// <summary>
        /// 明細部 Shift+Tab の処理
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pErrSet"></param>
        /// <param name="pMotoControl"></param>
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvPrv, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        /// <summary>
        /// 明細部 Enter の処理
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pErrSet"></param>
        /// <param name="pMotoControl"></param>
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvNxt, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        /// <summary>
        /// 明細部 PageDown の処理
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pErrSet"></param>
        /// <param name="pMotoControl"></param>
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
            {
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            }
            else
            {
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);
            }

            mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }
        /// <summary>
        /// 明細部 PageUp の処理
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pErrSet"></param>
        /// <param name="pMotoControl"></param>
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
            {
                w_GoRow = 0;
            }
            else
            {
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);
            }

            mGrid.F_MoveFocus((int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, (int)ClsGridIkkatuHacchuu.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }
        /// <summary>
        /// 明細部 MouseWheel の処理
        /// </summary>
        /// <param name="pDelta"></param>
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
        /// <summary>
        /// 
        /// </summary>
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
                            case (int)ClsGridIkkatuHacchuu.ColNO.GyouNO:
                            case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO:
                            case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName:
                            case (int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG:
                            case (int)ClsGridIkkatuHacchuu.ColNO.NetFLG:
                            case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName:
                            case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho:
                            case (int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO:
                            case (int)ClsGridIkkatuHacchuu.ColNO.SKUCD:
                            case (int)ClsGridIkkatuHacchuu.ColNO.JANCD:
                            case (int)ClsGridIkkatuHacchuu.ColNO.BrandName:
                            case (int)ClsGridIkkatuHacchuu.ColNO.SizeName:
                            case (int)ClsGridIkkatuHacchuu.ColNO.ColorName:
                            case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou:
                            case (int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD:
                            case (int)ClsGridIkkatuHacchuu.ColNO.TaniName:
                                //mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.GrayColor;
                                mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = true;
                                break;
                            case (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG:
                            case (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG:
                            case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD:
                            case (int)ClsGridIkkatuHacchuu.ColNO.KibouNouki:
                            case (int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou:
                            case (int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou:
                            case (int)ClsGridIkkatuHacchuu.ColNO.ShouhinName:
                                break;
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
                    //    case object _ when ClsGridIkkatuHacchuu.ColNO.DELCK:
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
                mGrid.g_DArray[w_Row].GyouNO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);
        }
        /// <summary>
        /// 明細部TabStopを一括で切り替える(TabStopがTrueのコントロールがない時、KeyExitが発生しなくなるため)
        /// </summary>
        /// <param name="pTabStop"></param>
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
        /// <summary>
        /// 明細部Selectableを切り替え、TabStopの同期も取る
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pSelectable"></param>
        private void S_Set_Cell_Selectable(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_0.Value;

            mGrid.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(pCol, pRow);
        }
        /// <summary>
        /// Tab移動可/不可の制御
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        private void S_Selectable(int pCol, int pRow)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Vsb_Mei_0_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_0_Event_MouseWheel(e.Delta);
        }
        /// <summary>
        /// ボディ部の状態制御
        /// </summary>
        /// <param name="pKBN">0...  新規/修正/削除時(モード選択時),1...  新規/修正時(画面展開後),2...  照会/削除時(画面展開後)明細入力不可、スクロールのみ</param>
        /// <param name="pGrid">0...  明細の各プロパティ以外,1...  明細部の各プロパティ(先に設定しておいてから、実際にpGrid=0で PanelのEnable制御等を行う)</param>
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
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;
                            }
                        }
                        else
                        {
                            IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                //Scr_Lock(0, 0, 0);
                                ////keyControls[(int)EIndex.SiiresakiCD].Enabled = true;
                                ////ScSiiresakiCD.BtnSearch.Enabled = true;

                                //Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                detailControls[(int)EIndex.HacchuuDate].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001011");
                            }
                            else
                            {
                                //keyControls[(int)EIndex.SiiresakiCD].Enabled = true;
                                //ScSiiresakiCD.BtnSearch.Enabled = true;
                                //keyControls[(int)EIndex.CopyIkkatuHacchuuNO].Text = string.Empty;
                                //keyControls[(int)EIndex.CopyIkkatuHacchuuNO].Enabled = false;
                                //ScCopyIkkatuHacchuuNO.BtnSearch.Enabled = false;

                                //Scr_Lock(1, mc_L_END, 1);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }
                            this.ControlByMode();
                        }
                        break;
                    }

                case 1:
                    {
                        if (OperationMode != EOperationMode.INSERT)
                        {
                            this.ScHacchuuNO.Enabled = false;
                            this.ScHacchuuShoriNO.Enabled = false;
                        }
                        if (pGrid == 1)
                        {
                            // 入力可の列の設定
                            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                            {
                                if (m_EnableCnt - 1 < w_Row)
                                    break;
                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridIkkatuHacchuu.ColNO.TaishouFLG:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = mGrid.g_DArray[w_Row].IsYuukouTaishouFLG == "True" ? true : false;
                                            break;
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiCD:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = OperationMode == EOperationMode.INSERT ? true : false;
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = OperationMode == EOperationMode.INSERT ? false : true;
                                            break;
                                        case (int)ClsGridIkkatuHacchuu.ColNO.EDIFLG:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = mGrid.g_DArray[w_Row].IsYuukouTaishouFLG == "True" ? true : false;
                                            break;
                                        case (int)ClsGridIkkatuHacchuu.ColNO.KibouNouki:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.ShanaiBikou:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.ShagaiBikou:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuSuu:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuTanka:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.Hacchuugaku:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            break;
                                        case (int)ClsGridIkkatuHacchuu.ColNO.GyouNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SiiresakiName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.ChokusouFLG:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NetFLG:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.JuchuuNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SKUCD:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.JANCD:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.BrandName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SizeName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.ColorName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuChuuiZikou:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.MakerShouhinCD:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.TaniName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO1:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiYuubinNO2:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho1:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiJuusho2:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiMailAddress:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiTELNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.NounyuusakiFAXNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SoukoCD:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.TaxRate:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.JuchuuRows:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.VariousFLG:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.AdminNO:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.SKUName:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.Teika:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.Kakeritu:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.HacchuuShouhizeigaku:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.TaniCD:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.OrderRows:
                                        case (int)ClsGridIkkatuHacchuu.ColNO.IsYuukouTaishouFLG:
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                            mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                            if (w_Row <= 5)
                                            {
                                                mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font = new System.Drawing.Font(mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font, FontStyle.Bold);
                                            }
                                            break;
                                        case (int)ClsGridIkkatuHacchuu.ColNO.ShouhinName:
                                            if (mGrid.g_DArray[w_Row].VariousFLG == "1")
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                                if (w_Row <= 5)
                                                {
                                                    mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font = new System.Drawing.Font(mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font, FontStyle.Regular);
                                                }
                                            }
                                            else
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = true;
                                                if (w_Row <= 5)
                                                {
                                                    mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font = new System.Drawing.Font(mGrid.g_MK_Ctrl[w_Col, w_Row].CellCtl.Font, FontStyle.Bold);
                                                }
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
                            //Scr_Lock(2, 3, 0);
                            //Scr_Lock(0, 1, 1);
                            if (OperationMode == EOperationMode.SHOW)
                            {
                                SetFuncKeyAll(this, "111111000000");
                            }
                            else
                            {
                                SetFuncKeyAll(this, "111111000001");
                            }
                        }
                        break;
                    }

                case 2:
                    {
                        if (OperationMode != EOperationMode.INSERT)
                        {
                            this.ScHacchuuNO.Enabled = false;
                            this.ScHacchuuShoriNO.Enabled = false;
                        }
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
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000011");
                                IMT_DMY_0.Focus();
                                //Scr_Lock(0, 0, 1);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        /// <param name="pCtlRow"></param>
        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridIkkatuHacchuu.ColNO.DELCK, pCtlRow).GVal(W_Del);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCol"></param>
        /// <param name="pRow"></param>
        private void Grid_NotFocus(int pCol, int pRow)
        {
            // ﾌｫｰｶｽをはじく
            int w_Col;
            //bool w_AllFlg = false;
            int w_CtlRow;

            if (OperationMode >= EOperationMode.DELETE)
                return;

            if (m_EnableCnt - 1 < pRow)
                return;

            w_CtlRow = pRow - Vsb_Mei_0.Value;
            if (w_CtlRow >= 0 && w_CtlRow <= mGrid.g_MK_Ctl_Row - 1)
            {
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);
            }

            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                switch (w_Col)
                {
                    case (int)ClsGridIkkatuHacchuu.ColNO.ShouhinName:
                        if (mGrid.g_DArray[pRow].VariousFLG == "1")
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                            mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                        }
                        else
                        {
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                            mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                            mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboStoreCD_SelectedValueChanged(object sender, EventArgs e)
        {
            ScHacchuuShoriNO.Value3 = CboStoreCD.SelectedValue.ToString();
        }

        #endregion
    }
}