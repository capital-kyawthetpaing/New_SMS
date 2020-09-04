﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;
using CKM_Controls;
namespace WMasterTouroku_HanbaiTankaTennic
{
    public partial class MasterTouroku_HanbaiTankaTennic : FrmMainForm
    {
        private const string ProID = "MasterTouroku_HanbaiTankaTennic";
        private const string ProNm = "販売単価マスタ(テニック)";
        private const short mc_L_END = 3;
        SKUPrice_BL spb;
        DataTable dt;
        M_SKUPrice_Entity mse;
        M_SKU_Entity ms;
        private enum EIndex : int
        {
            SKUCD,
            JANCD,
            ChangeDate,
            TekiyouShuuryouDate,
            SKUName,
            UnitPrice,
            StandardSalesUnitPrice,
            Rank1UnitPrice,
            Rank2UnitPrice,
            Rank3UnitPrice,
            Rank4UnitPrice,
            Rank5UnitPrice,
            Remarks
        }
        private Control[] keyControls;
        private Control[] detailControls;
        private System.Windows.Forms.Control previousCtrl;
        public MasterTouroku_HanbaiTankaTennic()
        {
            InitializeComponent();
            // this.Vsb_Mei_0.MouseWheel+= new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);
            spb = new SKUPrice_BL();
            dt = new DataTable();
            mse = new M_SKUPrice_Entity();
            ms = new M_SKU_Entity();
        }
        ClsGridHanbaiTankaTennic mGrid = new ClsGridHanbaiTankaTennic();
        private int m_EnableCnt;
        private int m_dataCnt = 0;
        public string[] DuplicateCheckCol = null;
        private void MasterTouroku_HanbaiTankaTennic_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                SetFunctionLabel(EProMode.MENTE);
                base.StartProgram();
                S_SetInit_Grid();
                txtStartDateFrom.Focus();
                SKUCDFrom.NameWidth = 0;
                SKUCDTo.NameWidth = 0;
                Clear(pnl_Body);
                Scr_Clr(0);
                TextLeave();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }
        private void Scr_Clr(short Kbn)  /// 0 is initial state
        {
            //カスタムコントロールのLeave処理を先に走らせるため pnl_Header
            //   IMT_DMY_0.Focus();
            var ctrl = GetAllControls(pnl_Header);
            if (Kbn == 0)
            {
                foreach (Control ctl in ctrl)
                {
                    if (ctrl is CKM_TextBox ct)
                    {
                        ct.Text = "";
                    }
                    else if (ctrl is CKM_SearchControl c)
                    {
                        c.TxtChangeDate.Text = "";
                        c.TxtCode.Text = "";
                        c.LabelText = "";
                    }
                    else if (ctrl is CKM_RadioButton cr && cr.Name == "ckM_RadioButton1")
                    {
                        cr.Checked = true;
                    }
                }
            }

            //    foreach (Control ctl in keyLabels)
            //    {
            //        ((CKM_SearchControl)ctl).LabelText = "";
            //    }

            //}

            //foreach (Control ctl in detailControls)
            //{
            //    if (ctl.GetType().Equals(typeof(CheckBox)))
            //    {
            //        ((CheckBox)ctl).Checked = false;
            //    }
            //    else if (ctl.GetType().Equals(typeof(RadioButton)))
            //    {
            //        ((RadioButton)ctl).Checked = true;
            //    }
            //    else
            //    {
            //        ctl.Text = "";
            //    }
            //}

            //foreach (Control ctl in detailLabels)
            //{
            //    ((CKM_SearchControl)ctl).LabelText = "";
            //}

            S_Clear_Grid();   //画面クリア（明細部）

            //if (Kbn == 0)
            //    SetEnabled();
        }
        private void InitialControlArray()
        {
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
                        if (index == (int)EIndex.JANCD)
                        {
                            detailControls[(int)EIndex.SKUCD].Focus();
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
                            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate);
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
                MessageBox.Show(ex.Message);
            }
        }
        private bool CheckKey(int index, bool set = true)
        {
            return true;
        }
        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridHanbaiTankaTennic.gc_P_GYO <= ClsGridHanbaiTankaTennic.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridHanbaiTankaTennic.gMxGyo;
                m_EnableCnt = ClsGridHanbaiTankaTennic.gMxGyo;
            }
            mGrid.g_MK_Ctl_Row = ClsGridHanbaiTankaTennic.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridHanbaiTankaTennic.gc_MaxCL;

            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;
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
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);//GridControl_Validated
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Validated += new EventHandler(GridControl_Validated);

                        }
                    }

                    switch (w_CtlCol)
                    {
                        case (int)ClsGridHanbaiTankaTennic.ColNO.SKUCD:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.JANCD:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:    // 
                        case (int)ClsGridHanbaiTankaTennic.ColNO.ItemName:    //  
                        case (int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
                        case (int)ClsGridHanbaiTankaTennic.ColNO.Space1:
                            //if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                            //    ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl).CKM_Reqired = true;

                            break;
                    }
                }
            }
            // 明細部の状態(初期状態) をセット 
            //To be Proceed. . . . 
            //
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];
            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridHanbaiTankaTennic.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色
            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];
            for (int i = (int)ClsGridHanbaiTankaTennic.ColNO.GYONO; i <= (int)ClsGridHanbaiTankaTennic.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }
            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridHanbaiTankaTennic.ColNO.COUNT; W_CtlCol++)
                {
                    //switch (W_CtlCol)
                    //{
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriSuu:
                    //        mGrid.SetProp_SU(5, ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriUnitPrice:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        break;
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriHontaiGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.MitsumoriGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.CostGaku:
                    //    case (int)ClsGridHanbaiTankaTennic.ColNO.ProfitGaku:
                    //        mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                    //        ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                    //        break;
                    //}

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }
            Set_GridTabStop(false);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

        }
        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                    //mGrid.g_DArray[w_Row].Rank1UnitPrice = "0";
                    //mGrid.g_DArray[w_Row].Rank2UnitPrice = "0";
                    //mGrid.g_DArray[w_Row].Rank3UnitPrice = "0";
                    //mGrid.g_DArray[w_Row].Rank4UnitPrice = "0";
                    //mGrid.g_DArray[w_Row].Rank5UnitPrice = "0";
                }
            }
            else  // set Data from db
            {
                int c = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].JANCD = dr["JanCD"].ToString();
                    mGrid.g_DArray[c].AdminNo = dr["AdminNo"].ToString();
                    mGrid.g_DArray[c].StartChangeDate = dr["StartDate"].ToString();
                    mGrid.g_DArray[c].EndChangeDate = dr["EndDate"].ToString();
                    mGrid.g_DArray[c].UnitPrice = dr["UnitPrice"].ToString();
                    mGrid.g_DArray[c].StandardSalesUnitPrice = dr["StandardSalesUnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank1UnitPrice = dr["Rank1UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank2UnitPrice = dr["Rank2UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank3UnitPrice = dr["Rank3UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank4UnitPrice = dr["Rank4UnitPrice"].ToString();
                    mGrid.g_DArray[c].Rank5UnitPrice = dr["Rank5UnitPrice"].ToString();
                    mGrid.g_DArray[c].ItemName = dr["SKUName"].ToString();
                    mGrid.g_DArray[c].CostUnitPrice = dr["CostUnitPrice"].ToString();
                    mGrid.g_DArray[c].Remarks = dr["Remarks"].ToString();
                    c++;
                }
            }
        }
        //protected override void ExecDisp()
        //{
        //    for (int i = 0; i < keyControls.Length; i++)
        //        if (CheckKey(i, false) == false)
        //        {
        //            keyControls[i].Focus();
        //            return;
        //        }
        //    CheckData(true);
        //}

        private bool CheckDetail(int index)
        {
            return CheckDetail(index, true);
        }
        //private bool CheckGrid(int col, int row)
        //{
        //    if (!bbl.CheckDate(mGrid.g_DArray[row].StartChangeDate))
        //    {
        //        bbl.ShowMessage("E103");
        //        return false;
        //    }
        //    if (!bbl.CheckDate(mGrid.g_DArray[row].EndChangeDate))
        //    {
        //        bbl.ShowMessage("E103");
        //        return false;
        //    }
        //    if (!string.IsNullOrEmpty(mGrid.g_DArray[row].StartChangeDate) && !string.IsNullOrEmpty(mGrid.g_DArray[row].EndChangeDate))
        //    {
        //        if (string.Compare(mGrid.g_DArray[row].StartChangeDate, mGrid.g_DArray[row].EndChangeDate) == 1)
        //        {
        //            bbl.ShowMessage("E104");
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        private bool ErrorCheck(int row)
        {
            if (!bbl.CheckDate(mGrid.g_DArray[row].StartChangeDate))
            {
                bbl.ShowMessage("E103");
                return false;
            }
            if (!bbl.CheckDate(mGrid.g_DArray[row].EndChangeDate))
            {
                bbl.ShowMessage("E103");
                return false;
            }
            if (!string.IsNullOrEmpty(mGrid.g_DArray[row].StartChangeDate) && !string.IsNullOrEmpty(mGrid.g_DArray[row].EndChangeDate))
            {
                if (string.Compare(mGrid.g_DArray[row].StartChangeDate, mGrid.g_DArray[row].EndChangeDate) == 1)
                {
                    bbl.ShowMessage("E104");
                    return false;
                }
            }

            return true;
        }
        private bool CheckDetail(int index, bool set)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }
            if (!string.IsNullOrEmpty(txtStartDateFrom.Text) && !string.IsNullOrEmpty(txtStartDateTo.Text))
            {
                if (string.Compare(txtStartDateFrom.Text, txtStartDateTo.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    txtStartDateTo.Focus();
                }
            }
            scBrandCD.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(scBrandCD.TxtCode.Text))
            {
                if (scBrandCD.SelectData())
                {
                    scBrandCD.SetFocus(1);
                }
                else
                {
                    bbl.ShowMessage("E101");
                    scBrandCD.SetFocus(1);
                }
            }
            scSegmentCD.ChangeDate = bbl.GetDate();
            if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
            {
                if (scSegmentCD.SelectData())
                {
                    scSegmentCD.SetFocus(1);
                }
                else
                {
                    bbl.ShowMessage("E101");
                    scSegmentCD.SetFocus(1);
                }
            }

            if (!string.IsNullOrEmpty(SKUCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(SKUCDTo.TxtCode.Text))
            {
                if (string.Compare(SKUCDFrom.TxtCode.Text, SKUCDTo.TxtCode.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    SKUCDTo.Focus();
                }
            }
            if (!string.IsNullOrEmpty(txtStartDateFrom.Text) && !string.IsNullOrEmpty(txtStartDateTo.Text))
            {
                if (string.Compare(txtStartDateFrom.Text, txtStartDateTo.Text) == 1)
                {
                    bbl.ShowMessage("E104");
                    txtStartDateTo.Focus();
                }
            }
            return true;
        }
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = detailControls[(int)EIndex.SKUName];

            IMT_ITMCD_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

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
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD) == false)
                {
                    for (int CL = (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate; CL < (int)ClsGridHanbaiTankaTennic.ColNO.COUNT; CL++)
                    {
                        //if (CheckGrid(CL, RW) == false)
                        //{
                        //Focusセット処理
                        ERR_FOCUS_GRID_SUB(CL, RW);
                        return;
                        //}
                    }
                }
            }
            DataTable dt = GetGridEntity();
        }
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
            OperationMode = mode;
            switch (mode)
            {
                case EOperationMode.INSERT:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    break;
                case EOperationMode.UPDATE:
                    //S_BodySeigyo(1, 1);
                    S_BodySeigyo(0, 0);
                    break;
            }
            S_SetInit_Grid();
            Clear(pnl_Body);
            btnDisplay.Enabled = true;
            F12Enable = true;
            txtStartDateFrom.Focus();
        }
        //private void S_BodySeigyo()
        //{

        //for (int row = 0; row <= 9; row++)
        //{

        //    (Controls.Find("IMT_ITMCD_" + row, true).FirstOrDefault() as CKM_Controls.CKM_TextBox).Enabled = false;
        //    (Controls.Find("IMT_JANCD_" + row, true).FirstOrDefault() as CKM_Controls.CKM_TextBox).Enabled = false;
        //    (Controls.Find("IMT_ITMNM_" + row, true).FirstOrDefault() as CKM_Controls.CKM_TextBox).Enabled = false;
        //    (Controls.Find("IMN_COSTUNPRICE_" + row, true).FirstOrDefault() as CKM_Controls.CKM_TextBox).Enabled = false;
        //}

        //    var txtckm =  this.Controls["IMT_ITMCD_0"];
        //foreach (Control ctl in detailControls)
        //{

        //   // ctl.Enabled = Kbn == 0 ? true : false;
        //}


        //}
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            switch (pKBN)
            {
                case 0:
                    {
                        txtStartDateFrom.Focus();
                        //Scr_Lock(0, 0, 0);   // フレームのロック解除
                        //Scr_Lock(1, mc_L_END, 1);  // フレームのロック
                        //SetEnabled(); //radio button
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
                                            //case (int)ClsGridHanbaiTankaTennic.ColNO.SKUCD:  
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice:
                                            case (int)ClsGridHanbaiTankaTennic.ColNO.Remarks:
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
                            txtStartDateFrom.Focus();

                            //画面へデータセット後、明細部入力可、キー部入力不可
                            //Scr_Lock(2, 3, 0);
                            //Scr_Lock(0, 1, 1);
                            SetFuncKeyAll(this, "111111000001");
                            btnDisplay.Enabled = false;

                            //if (radioButton2.Checked)
                            //{
                            //    //単価CD変更不可にする
                            //    detailControls[(int)EIndex.TankaCD].Enabled = false;
                            //    ScTanka.BtnSearch.Enabled = false;
                            //}
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
                            pnl_Body.Enabled = true;                  // ボディ部使用可
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

            switch (Index + 1)
            {
                case 2:
                    ChangeOperationMode(EOperationMode.INSERT);
                    ChangeOperationMode(OperationMode);
                    Clear(pnl_Header);
                    Clear(pnl_Body);
                    RadioButton1.Checked = true;
                    break;
                case 3:
                    ChangeOperationMode(EOperationMode.UPDATE);
                    //IMT_STADT_0.Focus();
                    ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 4:
                    ChangeOperationMode(EOperationMode.DELETE);
                    ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 5:
                    ChangeOperationMode(EOperationMode.SHOW);
                    ChangeOperationMode(OperationMode);//
                    Clear(pnl_Header);
                    RadioButton1.Checked = true;
                    InitScr();
                    break;
                case 6:
                    if (bbl.ShowMessage("Q004") == DialogResult.Yes)
                    {
                        ChangeOperationMode(OperationMode);
                        Clear(pnl_Header);
                        RadioButton1.Checked = true;
                        InitScr();
                        F12Enable = true;
                    }
                    else
                        PreviousCtrl.Focus();
                    break;
                case 11:
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
                case 12:
                    F12();
                    break;
            }
        }
        private DataTable GetGridEntity()
        {
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //m_dataCntが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].SKUCD) == false)
                {
                    dt.Rows.Add(mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].AdminNo
                        , mGrid.g_DArray[RW].JANCD
                        , bbl.Z_Set(mGrid.g_DArray[RW].StartChangeDate)
                        , bbl.Z_Set(mGrid.g_DArray[RW].EndChangeDate)
                        , bbl.Z_Set(mGrid.g_DArray[RW].UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].StandardSalesUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rank1UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rank2UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rank3UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rank4UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Rank5UnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].ItemName)
                        , bbl.Z_Set(mGrid.g_DArray[RW].CostUnitPrice)
                        , bbl.Z_Set(mGrid.g_DArray[RW].Space1)
                        , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
                        //, mGrid.g_DArray[RW].Update
                        );
                }
            }
            return dt;
        }
        private DataTable GetdatafromArray()
        {
            var result = new DataTable();
            var dt = new DataTable();
            var colnames = new string[] { "TanKaCD", "StoreCD", "AdminNO", "SKUCD", "StartChangeDate", "EndChangeDate", "PriceWithoutTax", "SalePriceOutTax", "Remarks", "DeleteFlg", "UsedFlg", "InsertOperartor", "InsertDateTime", "UpdateOperator", "UpdateDateTime" };
            var ColumnNames = new string[] { "SKUCD", "AdminNo", "JANCD", "StartChangeDate", "EndChangeDate", "UnitPrice", "StandardSalesUnitPrice", "Rank1", "Rank2", "Rank3", "Rank4", "Rank5", "ItemName", "CostUnitPrice", "Remarks" };
            foreach (var col in ColumnNames)
            {
                dt.Columns.Add(col);
            }
            foreach (var col in colnames)
            {
                result.Columns.Add(col);
            }
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //DataRow dr = dt.NewRow();

                // var sd= 
                if (!string.IsNullOrEmpty(mGrid.g_DArray[RW].SKUCD))
                {
                    dt.Rows.Add(
                        mGrid.g_DArray[RW].SKUCD
                          , mGrid.g_DArray[RW].AdminNo
                         , mGrid.g_DArray[RW].JANCD
                         , mGrid.g_DArray[RW].StartChangeDate
                         , mGrid.g_DArray[RW].EndChangeDate
                         , mGrid.g_DArray[RW].UnitPrice
                         , mGrid.g_DArray[RW].StandardSalesUnitPrice
                         , mGrid.g_DArray[RW].Rank1UnitPrice
                         , mGrid.g_DArray[RW].Rank2UnitPrice
                         , mGrid.g_DArray[RW].Rank3UnitPrice
                         , mGrid.g_DArray[RW].Rank4UnitPrice
                         , mGrid.g_DArray[RW].Rank5UnitPrice
                         , mGrid.g_DArray[RW].ItemName
                         , mGrid.g_DArray[RW].CostUnitPrice
                         // , bbl.Z_Set(mGrid.g_DArray[RW].Space1)
                         , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
                         //, mGrid.g_DArray[RW].Update
                         );
                }
                else
                {
                    dt.Rows.Add();
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                if ( !string.IsNullOrEmpty(dr["SKUCD"].ToString()) )
                {
                    if (string.IsNullOrEmpty(dr["StartChangeDate"].ToString()))
                    {
                        //Show Error Empty
                    }
                    else
                    {
                        M_SKUPrice_Entity mse = new M_SKUPrice_Entity
                        {
                            SKUCD = dr["SKUCD"].ToString(),
                        };
                        var dt_Exist = spb.M_SKUPrice_SelectData(mse);
                        if (dt_Exist.Rows.Count > 0)
                        {
                            return null;
                        }
                    }
                }
            }

            var dtnow = DateTime.Now.ToString();
           
            try
            {
                foreach (DataRow r in dt.Rows)
                {

                    var row = r;
                    if (!row.IsNull(1))
                    {
                        if (Getint(row["StandardSalesUnitPrice"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "0",
                                "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["StandardSalesUnitPrice"].ToString(),
                                row["Remarks"].ToString(),
                                "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (Getint(row["Rank1"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "1",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank1"].ToString(),
                                row["Remarks"].ToString(),
                                 "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (Getint(row["Rank2"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "2",
                                "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank2"].ToString(),
                                row["Remarks"].ToString(),
                                  "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (Getint(row["Rank3"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "3",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank3"].ToString(),
                                row["Remarks"].ToString(),
                                   "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (Getint(row["Rank4"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "4",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank4"].ToString(),
                                row["Remarks"].ToString(),
                                   "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                        if (Getint(row["Rank5"].ToString()) != "0")
                        {
                            result.Rows.Add(
                                "5",
                                 "0000",
                                row["AdminNO"].ToString(),
                                row["SKUCD"].ToString(),
                                row["StartChangeDate"].ToString(),
                                row["EndChangeDate"].ToString(),
                                row["UnitPrice"].ToString(),
                                row["Rank5"].ToString(),
                                row["Remarks"].ToString(),
                                "0",
                                "0",
                                 InOperatorCD,
                                dtnow,
                                "",
                                ""
                                );
                        }
                    }
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }
            
            return result;
        }
        private string Getint(string val)
        {
            try
            {
                return Convert.ToInt32(val.Split('.')[0]).ToString();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return "0";
        }
        private M_SKUPrice_Entity GetSearchInfo()
        {
            mse = new M_SKUPrice_Entity();
            mse.DisplayKBN = RadioButton1.Checked ? "0" : "1";
            mse.StartChangeDate = txtStartDateFrom.Text;
            mse.EndChangeDate = txtStartDateTo.Text;

            return mse;
        }
        private M_SKU_Entity GetInfo()
        {
            ms = new M_SKU_Entity()
            {
                SKUCDFrom = SKUCDFrom.Text,
                SKUCDTo = SKUCDTo.Text,
                BrandCD = scBrandCD.TxtCode.Text,
                SKUName = txtSKUName.Text,
                ExhibitionSegmentCD=scSegmentCD.TxtCode.Text,
                DeleteFlg="0"
            };
            return ms;
        }
        private void F11()
        {
            mse = GetSearchInfo();
            ms = GetInfo();
            dt = spb.M_SKUPrice_HanbaiTankaTennic_Select(mse, ms, (short)OperationMode);
            if (dt.Rows.Count > 0)
            {
                if ((OperationMode == EOperationMode.DELETE) || (OperationMode == EOperationMode.SHOW))
                {
                    SetMultiColNo(dt);
                    S_BodySeigyo(2, 2);
                    mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);

                    return;
                }
                else
                {
                    S_BodySeigyo(3, 0);
                    SetMultiColNo(dt);
                    S_BodySeigyo(1, 1);
                    mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                }
            }
            else
            {
                bbl.ShowMessage("E128");
                txtStartDateFrom.Focus();
            }
        }
        private void TextLeave()
        {
            Add_Leave(new Control[] { panel10_1, panel10_2, panel8, panel3, panel17, panel4, panel65, panel89, panel113, panel137 });
        }
        private void Add_Leave(Control[] cont)
        {
            foreach (var ctr in cont)
            {
                var Con = GetAllControls(ctr);
                foreach (var c in Con)
                {
                    if (c is CKM_TextBox ct)
                    {
                        ct.Leave += Ct_Leave;
                        //ct.Enter += keyD;
                        //ct.GotFocus += Ct_GotFocus;
                    }
                }
            }
        }
        private void Ct_GotFocus(object sender, EventArgs e)
        {
            L_Control = (Control)sender;
        }
        private void Ct_Enter(object sender, EventArgs e)
        {
            L_Control = (Control)sender;
            L_Control = ActiveControl;
        }
        private void Ct_Leave(object sender, EventArgs e)
        {//IMT_GYONO_0

            if ((sender as CKM_TextBox).Parent is Panel cp)
            {
                var Con = GetAllControls(cp);
                foreach (var ctr in Con)
                {
                    if (ctr is Label cl)
                    {
                        if (Convert.ToInt32(cl.Text) % 2 == 0)
                        {
                            ((CKM_TextBox)sender).BackColor = ClsGridBase.GridColor;
                        }
                        else {
                            ((CKM_TextBox)sender).Back_Color = CKM_TextBox.CKM_Color.White;
                        }
                        break;
                    }
                }
                mGrid.S_DispToArray(Vsb_Mei_0.Value);
                //   ((CKM_TextBox)sender).BackColor =ClsGridBase.GridColor;
                //if (ct is "")
                //((CKM_TextBox)sender).BackColor = ClsGridBase.GridColor;
                //else
                //    ((CKM_TextBox)sender).BackColor =  Color.White;
            }
        }
        Control L_Control = null;
        bool IsEntered = false;
        private void GridControl_Validated(object sender, EventArgs e)
        {
            if (IsEntered)
                return;
            Control c = sender as Control;
            if (c is CKM_TextBox ct)
            {
                if (ct.Name.Contains("IMT_STADT_"))
                {
                    if(!bbl.CheckDate(ct.Text))
                    {
                        bbl.ShowMessage("E103");
                        ct.Focus();
                    }
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var SKUCD = this.Controls.Find("IMT_ITMCD_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                    M_SKUPrice_Entity mse = new M_SKUPrice_Entity
                    {
                        SKUCD=SKUCD.Text,
                        StartChangeDate =ct.Text
                    };
                    DataTable dt = spb.M_SKUPrice_SelectData(mse);
                    if(dt.Rows.Count>0)
                    {
                        bbl.ShowMessage("E105");
                        IsEntered = true;
                        //// ct.Focus();
                        return;
                    }
                    //IsEntered = false;
                }
                if (ct.Name.Contains("IMT_ENDDT_"))
                {
                    if(!bbl.CheckDate(ct.Text))
                    {
                        bbl.ShowMessage("E103");
                        ct.Focus();
                    }
                    var StartDate = this.Controls.Find("IMT_STADT_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                    if (string.Compare(StartDate.Text, ct.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        ct.Focus();
                    }
                    //DataTable dtInsert = new DataTable();
                    //dtInsert = spb.SimpleSelect1("73", null, ct.Text);
                    //if(dtInsert.Rows.Count>0)
                    //{
                    //    MessageBox.Show("Date Exit!!!");
                    //    ct.Focus();
                    //}
                }
                if (ct.Name.Contains("IMN_UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_SSUNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var ssUnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R1UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r1UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R2UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r2UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R3UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r3UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R4UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r4UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                if (ct.Name.Contains("IMN_R5UNITPRICE_"))
                {
                    if (ActiveControl is CKM_Button cb && cb.Name == "BtnF1")
                    {
                        return;
                    }
                    var r5UnitPrice = (ct.Text);
                    if (string.IsNullOrWhiteSpace(ct.Text))
                    {
                        bbl.ShowMessage("E102");
                        ct.Focus();
                    }
                }
                //if (ct.Name.Contains("IMN_R5UNITPRICE_"))
                //{
                //    var UnitPrice = ct.Text;
                //    if (string.IsNullOrEmpty(ct.Text))
                //    {
                //        bbl.ShowMessage("E102");
                //        ct.Focus();
                //    }
                //}
            }
            mGrid.S_DispToArray(Vsb_Mei_0.Value);
        }
        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
             //   PreViousColor = ActiveControl.BackColor;
                int w_Row;
                CKM_Controls.CKM_TextBox w_ActCtl;

                w_ActCtl = (CKM_Controls.CKM_TextBox)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                // 背景色
               // w_ActCtl.BackColor = ClsGridBase.BKColor;  ---PTK

                Grid_Gotfocus((int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

              
                //w_ActCtl.Name
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
             //   w_ActCtl.BackColor = mGrid.F_GetBackColor_MK((int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, w_Row);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
            Control c = sender as Control;
            if (c is CKM_TextBox ct && e.KeyCode == (Keys.Enter | Keys.Tab))
            {
                if (ct.Name.Contains("IMT_STADT_"))
                {
                    SetMultiColNo(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var StartDate1 = dt.Rows[i]["StartDate"].ToString();
                        var StartDate = this.Controls.Find("IMT_STADT_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                        if (StartDate.Text == StartDate1.ToString())
                        {
                            bbl.ShowMessage("Date exit!!!");
                        }
                    }
                    //var IsExist = true; ///(ct.Text) // bll.bdfbedf 
                }
            }
                //    else if (ct.Name.Contains("IMT_ENDDT_"))
                //    {
                //        // Button btn = PanelFooter.;
                //        var StartDate = this.Controls.Find("IMT_STADT_" + ct.Name.Split('_').Last(), true)[0] as CKM_TextBox;
                //        if (string.Compare(StartDate.Text,  ct.Text) == 1)
                //        {
                //            bbl.ShowMessage("E104");
                //        }
                //    }
                //}
                //mGrid.S_DispToArray(Vsb_Mei_0.Value);
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
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);
            //Control1
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 0].CellCtl = IMT_JANCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 0].CellCtl = IMT_STADT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 0].CellCtl = IMT_ENDDT_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 0].CellCtl = IMN_UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 0].CellCtl = IMN_SSUNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 0].CellCtl = IMN_R1UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 0].CellCtl = IMN_R2UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 0].CellCtl = IMN_R3UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 0].CellCtl = IMN_R4UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 0].CellCtl = IMN_R5UNITPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 0].CellCtl = IMN_COSTUNPRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 0].CellCtl = IMT_REMARK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 0].CellCtl = Space0;
            //Control2
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 1].CellCtl = IMT_JANCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 1].CellCtl = IMT_STADT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 1].CellCtl = IMT_ENDDT_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 1].CellCtl = IMN_UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 1].CellCtl = IMN_SSUNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 1].CellCtl = IMN_R1UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 1].CellCtl = IMN_R2UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 1].CellCtl = IMN_R3UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 1].CellCtl = IMN_R4UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 1].CellCtl = IMN_R5UNITPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 1].CellCtl = IMN_COSTUNPRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 1].CellCtl = IMT_REMARK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 1].CellCtl = Space1;
            //Control3
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 2].CellCtl = IMT_JANCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 2].CellCtl = IMT_STADT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 2].CellCtl = IMT_ENDDT_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 2].CellCtl = IMN_UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 2].CellCtl = IMN_SSUNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 2].CellCtl = IMN_R1UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 2].CellCtl = IMN_R2UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 2].CellCtl = IMN_R3UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 2].CellCtl = IMN_R4UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 2].CellCtl = IMN_R5UNITPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 2].CellCtl = IMN_COSTUNPRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 2].CellCtl = IMT_REMARK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 2].CellCtl = Space2;
           
            //Control4
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 3].CellCtl = IMT_JANCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 3].CellCtl = IMT_STADT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 3].CellCtl = IMT_ENDDT_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 3].CellCtl = IMN_UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 3].CellCtl = IMN_SSUNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 3].CellCtl = IMN_R1UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 3].CellCtl = IMN_R2UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 3].CellCtl = IMN_R3UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 3].CellCtl = IMN_R4UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 3].CellCtl = IMN_R5UNITPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 3].CellCtl = IMN_COSTUNPRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 3].CellCtl = Space3;
            //Control5
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 4].CellCtl = IMT_JANCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 4].CellCtl = IMT_STADT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 4].CellCtl = IMT_ENDDT_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 4].CellCtl = IMN_UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 4].CellCtl = IMN_SSUNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 4].CellCtl = IMN_R1UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 4].CellCtl = IMN_R2UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 4].CellCtl = IMN_R3UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 4].CellCtl = IMN_R4UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice,4].CellCtl = IMN_R5UNITPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 4].CellCtl = IMN_COSTUNPRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 4].CellCtl = IMT_REMARK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 4].CellCtl = Space4;
            //Control6
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 5].CellCtl = IMT_JANCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 5].CellCtl = IMT_STADT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 5].CellCtl = IMT_ENDDT_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 5].CellCtl = IMN_UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 5].CellCtl = IMN_SSUNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 5].CellCtl = IMN_R1UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 5].CellCtl = IMN_R2UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 5].CellCtl = IMN_R3UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 5].CellCtl = IMN_R4UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 5].CellCtl = IMN_R5UNITPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 5].CellCtl = IMN_COSTUNPRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 5].CellCtl = IMT_REMARK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 5].CellCtl = Space5;
            //Control7
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 6].CellCtl = IMT_JANCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 6].CellCtl = IMT_STADT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 6].CellCtl = IMT_ENDDT_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 6].CellCtl = IMN_UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 6].CellCtl = IMN_SSUNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 6].CellCtl = IMN_R1UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 6].CellCtl = IMN_R2UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 6].CellCtl = IMN_R3UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 6].CellCtl = IMN_R4UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 6].CellCtl = IMN_R5UNITPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 6].CellCtl = IMN_COSTUNPRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 6].CellCtl = IMT_REMARK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 6].CellCtl = Space6;
            //Control8
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 7].CellCtl = IMT_JANCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 7].CellCtl = IMT_STADT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 7].CellCtl = IMT_ENDDT_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 7].CellCtl = IMN_UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 7].CellCtl = IMN_SSUNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 7].CellCtl = IMN_R1UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 7].CellCtl = IMN_R2UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 7].CellCtl = IMN_R3UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 7].CellCtl = IMN_R4UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 7].CellCtl = IMN_R5UNITPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 7].CellCtl = IMN_COSTUNPRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 7].CellCtl = IMT_REMARK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 7].CellCtl = Space7;
            //Control9
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 8].CellCtl = IMT_JANCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 8].CellCtl = IMT_STADT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 8].CellCtl = IMT_ENDDT_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 8].CellCtl = IMN_UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 8].CellCtl = IMN_SSUNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 8].CellCtl = IMN_R1UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 8].CellCtl = IMN_R2UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 8].CellCtl = IMN_R3UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 8].CellCtl = IMN_R4UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 8].CellCtl = IMN_R5UNITPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 8].CellCtl = IMN_COSTUNPRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 8].CellCtl = IMT_REMARK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 8].CellCtl = Space8;
            //Control10
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.SKUCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.JANCD, 9].CellCtl = IMT_JANCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StartChangeDate, 9].CellCtl = IMT_STADT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.EndChangeDate, 9].CellCtl = IMT_ENDDT_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.UnitPrice, 9].CellCtl = IMN_UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.StandardSalesUnitPrice, 9].CellCtl = IMN_SSUNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank1UnitPrice, 9].CellCtl = IMN_R1UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank2UnitPrice, 9].CellCtl = IMN_R2UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank3UnitPrice, 9].CellCtl = IMN_R3UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank4UnitPrice, 9].CellCtl = IMN_R4UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Rank5UnitPrice, 9].CellCtl = IMN_R5UNITPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.ItemName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.CostUnitPrice, 9].CellCtl = IMN_COSTUNPRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Remarks, 9].CellCtl = IMT_REMARK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridHanbaiTankaTennic.ColNO.Space1, 9].CellCtl = Space9;
        }
        private void S_Grid_0_Event_Tab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void S_Grid_0_Event_ShiftTab(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvPrv, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvPrv, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void S_Grid_0_Event_PageDown(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow + (mGrid.g_MK_Ctl_Row - 1) > mGrid.g_MK_Max_Row - 1)
                w_GoRow = mGrid.g_MK_Max_Row - 1;
            else
                w_GoRow = pRow + (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }
        private void S_Grid_0_Event_PageUp(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            int w_GoRow;

            if (pRow - (mGrid.g_MK_Ctl_Row - 1) < 0)
                w_GoRow = 0;
            else
                w_GoRow = pRow - (mGrid.g_MK_Ctl_Row - 1);

            mGrid.F_MoveFocus((int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, (int)ClsGridHanbaiTankaTennic.Gen_MK_FocusMove.MvSet, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0, w_GoRow, pCol);
        }
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
        private void S_Set_Cell_Selectable(int pCol, int pRow, bool pSelectable)
        {
            int w_CtlRow;
            w_CtlRow = pRow - Vsb_Mei_0.Value;

            mGrid.g_MK_State[pCol, pRow].Cell_Selectable = pSelectable;

            // 全行クリアなどのときは、画面範囲のみTABINDEX制御
            if (w_CtlRow < mGrid.g_MK_Ctl_Row & w_CtlRow > 0)
                mGrid.g_MK_Ctrl[pCol, w_CtlRow].CellCtl.TabStop = mGrid.F_GetTabStop(pCol, pRow);
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
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }
                }
            }
           Array.Clear(mGrid.g_DArray, mGrid.g_DArray.GetLowerBound(0), mGrid.g_DArray.GetLength(0));

            // 行番号の初期値セット
            for (w_Row = 0; w_Row <= mGrid.g_MK_Max_Row - 1; w_Row++)
                mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();

            // 画面クリア
            mGrid.S_DispFromArray(0, ref this.Vsb_Mei_0);
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
                    txtStartDateFrom.TabStop = pTabStop;
                else
                    txtStartDateFrom.TabStop = false;

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
                return;
            }
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
            pnl_Body.Refresh();
        }
        private void Vsb_Mei_0_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_0_Event_MouseWheel(e.Delta);
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
        private M_SKUPrice_Entity SKUPriceEntity()
        {
            mse = new M_SKUPrice_Entity()
            {
                //TankaCD = "0000000000000",
                StoreCD = "0000",
                //AdminNO="111",
                StartChangeDate = IMT_STADT_0.Text,
                EndChangeDate = IMT_ENDDT_0.Text,
                PriceWithTax = "0",
                PriceWithoutTax = "0",
                GeneralRate = "0",
                GeneralPriceWithTax = "0",
                GeneralPriceOutTax="0",
                MemberRate="0",
                MemberPriceWithTax="0",
                MemberPriceOutTax="0",
                ClientRate="0",
                ClientPriceWithTax="0",
                ClientPriceOutTax="0",
                SaleRate="0",
                SalePriceWithTax="0",
                SalePriceOutTax="0",
                WebRate="0",
                WebPriceWithTax="0",
                WebPriceOutTax="0",
                UnitPrice= IMN_UNITPRICE_0.Text,
                StandardSalesUnitPrice= IMN_SSUNITPRICE_0.Text,
                //Rank1UnitPrice= IMN_R1UNITPRICE_0.Text,
                //Rank2UnitPrice= IMN_R2UNITPRICE_0.Text,
                //Rank3UnitPrice= IMN_R3UNITPRICE_0.Text,
                //Rank4UnitPrice= IMN_R4UNITPRICE_0.Text,
                //Rank5UnitPrice= IMN_R5UNITPRICE_0.Text,
                Remarks=IMT_REMARK_0.Text,
                DeleteFlg ="0",
                UsedFlg="0",
                Operator = InOperatorCD,
                ProcessMode =ModeText,
                ProgramID = InProgramID,
                PC = InPcID,
                Key =txtStartDateFrom.Text
            };
            return mse;
        }
        private void F12()
        {
            if (spb.ShowMessage(OperationMode == EOperationMode.DELETE ? "Q102" : "Q101") == DialogResult.Yes)
            {
               mse = SKUPriceEntity();
                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                        //SetMultiColNo(dt);
                        //var EndDate = dt.Rows[0]["EndDate"].ToString();
                        //if (EndDate.Length==0)
                        //{
                        //    bbl.ShowMessage("Date Exist!!");
                        //    IMT_ENDDT_0.Focus();
                        //}
                        //else
                        //{
                            InsertUpdate(1);
                            InitScr();
                        //}
                        break;
                    case EOperationMode.UPDATE:
                        InsertUpdate(2);
                        InitScr();
                        break;
                    case EOperationMode.DELETE: 
                        InsertUpdate(3);
                        InitScr();
                        break;
                }
            }
        }
        private void InsertUpdate(int mode)
        {
            var dt = GetdatafromArray();
            if (dt == null)
            {
                bbl.ShowMessage("E105");  // Start date exist check
                PreviousCtrl.Focus();
                return;
            }
            string Xml = spb.DataTableToXml(dt);
           // string StartDate = dt.Rows[0]["StartChangeDate"].ToString();
            //if (dt.Rows.Count >= StartDate.Length)
            //{
            //    bbl.ShowMessage("Date Exist!!");
            //    IMT_ENDDT_0.Focus();
            //}
            if (spb.M_SKUPrice_Insert_Update(mse, Xml, mode))
            {
                 spb.ShowMessage("I101");
                 Clear(pnl_Header);
                 Clear(pnl_Body);
                 ChangeOperationMode(OperationMode);
            }
            else
            {
                spb.ShowMessage("S001");
            }
        }
        //protected bool RequireCheck(Control[] ctrl, TextBox txt = null)
        //{
        //    this.txt = txt;
        //    foreach (Control c in ctrl)
        //    {
        //        if (c is CKM_TextBox)
        //        {
        //            if (txt == null)
        //            {
        //                var StartDate = dt.Rows[0]["StartChangeDate"].ToString();
        //                if(c.Text==StartDate.ToString())
        //                {
        //                    bbl.ShowMessage("Date Exist!!");
        //                    c.Focus();
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}
        //protected void Check_Textbox()
        //{
        //    try
        //    {
        //        previousCtrl = this.ActiveControl;
        //        if (previousCtrl is CKM_TextBox ct)
        //        {
        //            if (ct.Name.Contains("IMT_STADT_"))
        //            {
        //                var StartDate = dt.Rows[0]["StartChangeDate"].ToString();
        //                if (previousCtrl.GetType().BaseType.Name.Contains("StartDate"))
        //                {
        //                    bbl.ShowMessage("Date Exist!!");
        //                    previousCtrl.Focus();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
       
        private void ckM_TextBox68_TextChanged(object sender, EventArgs e)
        {

        }
        
        protected override void EndSec()
        {
            this.Close();
        }

        private void MasterTouroku_HanbaiTankaTennic_KeyUp(object sender, KeyEventArgs e)
        {
            Control w_ActCtl= ActiveControl;
            if (w_ActCtl is CKM_TextBox c )
            {

                if ((w_ActCtl as CKM_TextBox).Name == "IMT_REMARK_0")
                {
                    c.MoveNext = false;

                    if (e.KeyCode == (Keys.Enter | Keys.Tab))
                        IMT_STADT_1.Focus();
                }
                else if ((w_ActCtl as CKM_TextBox).Name == "IMT_REMARK_1")
                {
                    if (e.KeyCode == (Keys.Enter | Keys.Tab))
                        IMT_STADT_2.Focus();
                }
                else
                {
                    MoveNextControl(e);
                }
            }
            else
                 MoveNextControl(e);
                //else
                //{
                //  //  var w_ActCtl = (CKM_TextBox)sender;
                //    if (w_ActCtl.Name == "IMT_REMARK_0")
                //    {


                //    }
                //}

        }

        private void txtStartDateTo_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtStartDateFrom.Text) && !string.IsNullOrEmpty(txtStartDateTo.Text))
                {
                    if (string.Compare(txtStartDateFrom.Text, txtStartDateTo.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        txtStartDateTo.Focus();
                    }
                }
            }
        }

        private void SKUCDTo_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(SKUCDFrom.TxtCode.Text) && !string.IsNullOrEmpty(SKUCDTo.TxtCode.Text))
                {
                    if (string.Compare(SKUCDFrom.TxtCode.Text, SKUCDTo.TxtCode.Text) == 1)
                    {
                        bbl.ShowMessage("E104");
                        SKUCDTo.Focus();
                    }
                }
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            F11();
        }

        private void scSegmentCD_CodeKeyDownEvent_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scSegmentCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scSegmentCD.TxtCode.Text))
                {
                    if (!scSegmentCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scSegmentCD.SetFocus(1);
                    }
                }
            }
        }

        private void scSegmentCD_Enter_1(object sender, EventArgs e)
        {
            scSegmentCD.Value1 = "226";
        }

        private void scBrandCD_CodeKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                scBrandCD.ChangeDate = bbl.GetDate();
                if (!string.IsNullOrEmpty(scBrandCD.TxtCode.Text))
                {
                    if (!scBrandCD.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        scBrandCD.SetFocus(1);
                    }
                }
            }
        }
    }
}
