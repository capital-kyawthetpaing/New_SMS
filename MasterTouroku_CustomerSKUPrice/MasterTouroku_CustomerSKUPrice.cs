using System;
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
using System.IO;
using ExcelDataReader;

namespace MasterTouroku_CustomerSKUPrice
{
    public partial class FrmMasterTouroku_CustomerSKUPrice : FrmMainForm
    {

        private const string ProID = "MasterTouroku_CustomerSKUPrice";
        private const string ProNm = "顧客別SKU販売単価マスタ";
        private const short mc_L_END = 3; // ロック用
        MasterTouroku_CustomerSKUPrice_BL mcskupbl;
        M_CustomerSKUPrice_Entity mcskue;

        //private Control[] keyControls;
        //private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        //private Control[] searchButtons;

        private System.Windows.Forms.Control previousCtrl;
        ClsGridCustomerSKUPric mGrid;
        private int m_EnableCnt;
        private int m_dataCnt = 0;
        public string[] DuplicateCheckCol = null;

        private DataTable dtExcel;
        private DataTable dtDelete;

        private enum EIndex : int
        {
            DateStart,
            DateEnd,
            CustomerCD_Start,
            CustomerCD_End,
            SKUCD_Start,
            SKUCD_End,

            CustomerCD,
            CustomerName,

            TekiyouKaisiDate,
            TekiyouShuuryouDate,

            Date = 0,

            AdminNO,
            JANCD,
            SKUCD,
            SKUName,
            SalePriceOutTax,
            Remarks
        }

        private enum EsearchKbn : short
        {
            Null,
            Product,
            Customer
        }

        public FrmMasterTouroku_CustomerSKUPrice()
        {
            InitializeComponent();
            mGrid = new ClsGridCustomerSKUPric();
            mcskupbl = new MasterTouroku_CustomerSKUPrice_BL();
            mcskue = new M_CustomerSKUPrice_Entity();

            dtExcel = new DataTable();

            this.Vsb_Mei_0.MouseWheel
               += new System.Windows.Forms.MouseEventHandler(this.Vsb_Mei_0_MouseWheel);
        }

        private void FrmMasterTouroku_CustomerSKUPrice_Load(object sender, EventArgs e)
        {
            InProgramID = ProID;
            SetFunctionLabel(EProMode.INPUT);

            InitialControlArray();

            Btn_F8.Text = "";
            Btn_F10.Text = "";
            Btn_F11.Text = "";

            S_SetInit_Grid();


            StartProgram();
            txtStartDate.Focus();

            Clear(pnl_Body);
            Scr_Clr(0);
            S_BodySeigyo(0, 1);
        }

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
                    DEL_SUB();
                    break;

                case 7://F8:行追加
                    //DEL_SUB();
                    break;

                case 9://F10複写
                    //CPY_SUB();
                    break;

                case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;
                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (w_Col == (int)ClsGridCustomerSKUPric.ColNO.JANCD)
                        //商品検索
                        kbn = EsearchKbn.Product;
                    if (w_Col == (int)ClsGridCustomerSKUPric.ColNO.CustomerCD)
                        kbn = EsearchKbn.Customer;

                        if (kbn != EsearchKbn.Null)
                            SearchData(kbn, previousCtrl);

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

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; // (1:新規,2:修正,3;削除)

            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            //配列の内容を画面にセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    btnImport.Enabled = true;
                    btnDisplay.Enabled = false;
                    detailControls[0].Focus();
                    break;

                case EOperationMode.UPDATE:
                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    btnImport.Enabled = false;
                    btnDisplay.Enabled = true;
                    detailControls[0].Focus();
                    break;

            }

        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { txtStartDate, txtEndDate, ScCustomer_Start.TxtCode,ScCustomer_End.TxtCode, ScSKUCD_Start.TxtCode,ScSKUCD_End.TxtCode,txtItemName };


            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            rdoRecent.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
            rdoAll.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);

            rdoRecent.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            rdoAll.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
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
                    detailControls[(int)EIndex.DateStart].Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        private void Scr_Clr(short Kbn)  /// 0 is initial state
        {
            var ctrl = GetAllControls(pannel_Header);
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
                    else if (ctrl is CKM_RadioButton cr && cr.Name == "rdoRecent")
                    {
                        cr.Checked = true;
                    }
                }
            }

            S_Clear_Grid();   //画面クリア（明細部）
        }

        /// <summary>
        /// Bind Data
        /// </summary>
        /// <param name="dt"></param>
        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                    //mGrid.g_DArray[w_Row].Rank1UnitPrice = "0";
                }
            }
            else  // set Data from db
            {
                int c = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    //detailControls[(int)EIndex.Date].Text = bbl.GetDate();
                    mGrid.g_DArray[c].CustomerCD = dr["CustomerCD"].ToString();
                    mGrid.g_DArray[c].CustomerName = dr["CustomerName"].ToString();
                    mGrid.g_DArray[c].TekiyouKaisiDate = dr["TekiyouKaisiDate"].ToString();
                    mGrid.g_DArray[c].TekiyouShuuryouDate = dr["TekiyouShuuryouDate"].ToString();
                    mGrid.g_DArray[c].AdminNO = dr["AdminNo"].ToString();
                    mGrid.g_DArray[c].JANCD = dr["JanCD"].ToString();
                    mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].SKUName = dr["SKUName"].ToString(); 
                    mGrid.g_DArray[c].SalePriceOutTax = bbl.Z_SetStr(dr["SalePriceOutTax"]);
                    mGrid.g_DArray[c].Remarks = dr["Remarks"].ToString();
                    c++;
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
                            //// ｷｰ部
                            //foreach (Control ctl in keyControls)
                            //{
                            //    ctl.Enabled = Kbn == 0 ? true : false;
                            //}
                            //ScMitsumoriNO.BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            //ScCustomer.BtnSearch.Enabled = Kbn == 0 ? true : false;
                            //btnAddress.Enabled = Kbn == 0 ? true : false;

                            break;
                        }
                }
            }
        }

        private void ADD_SUB()
        {
            Control w_Act = previousCtrl;
            int w_Row;
            int w_Gyo;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }
            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            //追加行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i > w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //前行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            w_Gyo = Convert.ToInt16(mGrid.g_DArray[w_Row].GYONO);
            // 一行クリア
            Array.Clear(mGrid.g_DArray, w_Row, 1);
            //退避内容を戻す
            mGrid.g_DArray[w_Row].GYONO = w_Gyo.ToString();          //行番号

            //CalcKin();

            int col = (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvSet, (int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }

        private void DEL_SUB()
        {
            int w_Row;

            if (mGrid.F_Search_Ctrl_MK(previousCtrl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            //CalcKin();

            int col = (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット
            //IMT_DMY_0.Focus();

            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvSet, (int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }

        protected override void ExecDisp()
        {
            if (OperationMode != EOperationMode.INSERT)
            {
                CheckData(true);
            }
                
        }

        protected override void ExecSec()
        {

            if (OperationMode != EOperationMode.DELETE)
            {
            
                // 明細部  画面の範囲の内容を配列にセット
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //明細部チェック
                for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
                {
                    //m_dataCntが更新有効行数
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TekiyouKaisiDate) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CustomerCD) == false
                        || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JANCD) == false
                        )
                    {
                        for (int CL = (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate; CL < (int)ClsGridCustomerSKUPric.ColNO.COUNT; CL++)
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
                //CalcKin();
            }

            DataTable dt = GetGridEntity();

            //更新処理
            mcskue = GetEntity(dt);
            //mibl.Mitsumori_Exec(dme, dt, (short)OperationMode, InOperatorCD, InPcID);
            switch(OperationMode)
            {
                case EOperationMode.INSERT:
                    InsertUpdate(1);
                    break;
                case EOperationMode.UPDATE:
                   // Delete();
                    InsertUpdate(2);
                    break;
                case EOperationMode.DELETE:
                    Delete();
                    break;
            }

            //更新後画面クリア
            ChangeOperationMode(base.OperationMode);
        }

        private void InsertUpdate(int mode)
        {
            if (mcskupbl.CustomerSKUPrice_Exec(mcskue,mode))
            {
                bbl.ShowMessage("I101");

                ChangeOperationMode(OperationMode);
                detailControls[0].Focus();
            }
            else
            {
                bbl.ShowMessage("S001");
            }
        }

        private void Delete()
        {
            if (mcskupbl.CustomerSKUPrice_Exec(mcskue, 3))
            {
                ChangeOperationMode(OperationMode);
                detailControls[0].Focus();
            }
            else
            {
                bbl.ShowMessage("S001");
            }

        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

           // int rowNo = 1;
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++)
            {
                //m_dataCntが更新有効行数
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].TekiyouKaisiDate) == false
                       || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].CustomerCD) == false
                       || string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JANCD) == false
                       )
                {

                    dt.Rows.Add(
                         mGrid.g_DArray[RW].CustomerCD == "" ? null : mGrid.g_DArray[RW].CustomerCD
                        , mGrid.g_DArray[RW].CustomerName == "" ? null : mGrid.g_DArray[RW].CustomerName
                        , mGrid.g_DArray[RW].TekiyouKaisiDate == "" ? null : mGrid.g_DArray[RW].TekiyouKaisiDate
                        , mGrid.g_DArray[RW].TekiyouShuuryouDate == "" ? null : mGrid.g_DArray[RW].TekiyouShuuryouDate
                        , bbl.Z_Set(mGrid.g_DArray[RW].AdminNO)
                        , mGrid.g_DArray[RW].JANCD == "" ? null : mGrid.g_DArray[RW].JANCD
                        , mGrid.g_DArray[RW].SKUCD == "" ? null : mGrid.g_DArray[RW].SKUCD
                        , mGrid.g_DArray[RW].SKUName == "" ? null : mGrid.g_DArray[RW].SKUName
                        , bbl.Z_Set(mGrid.g_DArray[RW].SalePriceOutTax)
                        , mGrid.g_DArray[RW].Remarks == "" ? null : mGrid.g_DArray[RW].Remarks
                        //, mGrid.g_DArray[RW].Update
                        );

                  //  rowNo++;
                }
            }

            return dt;
        }

        private M_CustomerSKUPrice_Entity GetEntity(DataTable dtCSKUP)
        {
            mcskue = new M_CustomerSKUPrice_Entity
            {
                dt1 = dtCSKUP,
                dt2 = dtDelete,
                ProcessMode = ModeText,
                InsertOperator = InOperatorCD,
                ProgramID = InProgramID,
                Key = InPcID + bbl.GetDate(),
                PC = InPcID,
            };
            return mcskue;
        }

        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("CustomerCD", typeof(string));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("TekiyouKaisiDate", typeof(string));
            dt.Columns.Add("TekiyouShuuryouDate", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("JanCD", typeof(string));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("SKUName", typeof(string));
            dt.Columns.Add("SalePriceOutTax", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));

        }
      
        public M_CustomerSKUPrice_Entity GetSKUPriceInfo()
        {
            mcskue = new M_CustomerSKUPrice_Entity
            {
                TekiyouKaisiDate_From = txtStartDate.Text,
                TekiyouKaisiDate_To = txtEndDate.Text,
                CustomerCD_From = ScCustomer_Start.TxtCode.Text,
                CustomerCD_To = ScCustomer_End.TxtCode.Text,
                SKUCD_From = ScSKUCD_Start.TxtCode.Text,
                SKUCD_To = ScSKUCD_End.TxtCode.Text,
                SKUName = txtItemName.Text,
                DisplayKBN = rdoRecent.Checked? "0" : "1",
            };
            return mcskue;
        }

        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Product:
                    int w_Row;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    w_Row = w_CtlRow + Vsb_Mei_0.Value;

                    //画面より配列セット 
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);

                    using (Search_Product frmProduct = new Search_Product(bbl.GetDate()))
                    {
                        frmProduct.SKUCD = mGrid.g_DArray[w_Row].SKUCD;
                        frmProduct.JANCD = mGrid.g_DArray[w_Row].JANCD;
                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].JANCD = frmProduct.JANCD;
                            mGrid.g_DArray[w_Row].OldJanCD = frmProduct.JANCD;
                            mGrid.g_DArray[w_Row].SKUCD = frmProduct.SKUCD;
                            mGrid.g_DArray[w_Row].SKUName = frmProduct.ITEM;
                            mGrid.g_DArray[w_Row].AdminNO = frmProduct.AdminNO;

                            CheckGrid((int)ClsGridCustomerSKUPric.ColNO.JANCD, w_Row, false, true);

                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;

                case EsearchKbn.Customer:
                    int wc_Row;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int wc_Col, out int wc_CtlRow) == false)
                    {
                        return;
                    }

                    wc_Row = wc_CtlRow + Vsb_Mei_0.Value;

                    //画面より配列セット 
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);
                    using (FrmSearch_Customer frmCustomer = new FrmSearch_Customer(bbl.GetDate(),"1",StoreCD))
                    {
                        //frmCustomer.CustomerCD=mGrid.g_DArray[wc_Row].CustomerCD;
                        frmCustomer.ShowDialog();

                        if (!frmCustomer.flgCancel)
                        {
                            mGrid.g_DArray[wc_Row].CustomerCD = frmCustomer.CustomerCD;
                            mGrid.g_DArray[wc_Row].CustomerName = frmCustomer.CustName;
                            //mGrid.g_DArray[wc_Row].AdminNO = frmCustomer.AdminNO;

                            CheckGrid((int)ClsGridCustomerSKUPric.ColNO.CustomerCD, wc_Row, false, true);

                            //配列の内容を画面へセット
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                            SendKeys.Send("{ENTER}");
                        }
                    }

                    break;
                    
            }

        }


        #region Button Envent
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (OperationMode == EOperationMode.INSERT)
            {
                dtExcel = null;
                if (ErrorCheckForExcel())
                {
                    S_Clear_Grid();
                    SetMultiColNo(dtExcel);
                    S_BodySeigyo(0, 1);
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;

                if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                if (w_Col == (int)ClsGridCustomerSKUPric.ColNO.JANCD)
                    kbn = EsearchKbn.Product;

                if (w_Col == (int)ClsGridCustomerSKUPric.ColNO.CustomerCD)
                    kbn = EsearchKbn.Customer;

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

        private void btnDisplay_Click(object sender, EventArgs e)
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
        #endregion

        #region error check for Excel Import 
        private bool ErrorCheckForExcel()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true
            };
            if (op.ShowDialog() == DialogResult.OK)
            {
                string str = op.FileName;
                string ext = Path.GetExtension(str);
                if (!(ext.Equals(".xls") || ext.Equals(".xlsx")))
                {
                    bbl.ShowMessage("E137");
                    return false;
                }
                else
                {
                    dtExcel = ExcelToDatatable(str);
                    string[] colname = { "適用開始日", "適用終了日", "顧客CD", "顧客名" , "JANCD" , "SKUCD" , "商品名", "税抜単価" , "備考" };
                    
                    if (ColumnCheck(colname, dtExcel))
                    {
                        dtExcel = ChangeColumnName(dtExcel);
                        dtExcel.Columns.Add("AdminNO", typeof(String));
                        for(int i=0; i < dtExcel.Rows.Count; i++)
                        {
                            //dtExcel.Rows[i]["TekiyouKaisiDate"] = dtExcel.Rows[i]["TekiyouKaisiDate"].ToString("yyyy-mm-dd");
                            M_SKU_Entity mse = new M_SKU_Entity
                            {
                                JanCD = dtExcel.Rows[i]["JANCD"].ToString(),
                                ChangeDate = bbl.GetDate(),
                            };
                            SKU_BL mbl = new SKU_BL();
                            DataTable dt = mbl.M_SKU_SelectAll(mse);
                            if (dt.Rows.Count == 0)
                            {
                                bbl.ShowMessage("E107");
                            }
                            else
                            {
                                dtExcel.Rows[i]["AdminNO"] = dt.Rows[0]["AdminNO"].ToString();
                            }

                        }
                        
                    }
                    else
                    {
                        bbl.ShowMessage("E137");
                    }

                 }

            }
           return true;
        }

        protected DataTable ExcelToDatatable(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            string ext = Path.GetExtension(filePath);
            IExcelDataReader excelReader;
            if (ext.Equals(".xls"))
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else if (ext.Equals(".xlsx"))
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            else
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx) 
                excelReader = ExcelReaderFactory.CreateCsvReader(stream, null);

            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            bool useHeaderRow = true;

            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = useHeaderRow,
                }
            });
            excelReader.Close();
            return result.Tables[0];
        }

        protected DataTable ChangeColumnName(DataTable dt)
        {
            dt.Columns["適用開始日"].ColumnName = "TekiyouKaisiDate";
            dt.Columns["適用終了日"].ColumnName = "TekiyouShuuryouDate";
            dt.Columns["顧客CD"].ColumnName = "CustomerCD";
            dt.Columns["顧客名"].ColumnName = "CustomerName";
            dt.Columns["商品名"].ColumnName = "SKUName";
            dt.Columns["税抜単価"].ColumnName = "SalePriceOutTax";
            dt.Columns["備考"].ColumnName = "Remarks";
            return dt;
        }

        protected Boolean ColumnCheck(String[] colName, DataTable dtMain)
        {
            DataColumnCollection cols = dtMain.Columns;
            for (int i = 0; i < colName.Length; i++)
            {
                if (!cols.Contains(colName[i]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Gridview Function

        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvNxt, (int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
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
                //IMT_DMY_0.Focus();
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
            pnl_Body.Refresh();
        }

        private void Vsb_Mei_0_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            S_Grid_0_Event_MouseWheel(e.Delta);
        }

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

        private void Grid_Gotfocus(int pCol, int pRow, int pCtlRow)
        {
            bool W_Del = false;
            bool W_Ret;

            // TabStopを全てTrueに(KeyExitイベントが発生しなくなることを防ぐ)
            Set_GridTabStop(true);

            //mGrid.g_MK_Ctrl(ClsGridHanbaiTanka.ColNO.DELCK, pCtlRow).GVal(W_Del);

            // ﾌｧﾝｸｼｮﾝﾎﾞﾀﾝ使用可否
            SetFuncKeyAll(this, "111111111101");

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

                w_ActCtl = (Control)sender ;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;

                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //どの項目か判別
                    int CL = -1;
                    string ctlName = "";
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    else
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;

                    switch (ctlName)
                    {
                        case "IMT_SDate":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate;
                            break;
                        case "IMT_EDate":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate;
                            break;
                        case "SC_Customer":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.CustomerCD;
                            break;
                        case "IMT_REMARK":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.Remarks;
                            break;
                        case "SC_ITEM":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.JANCD;
                            break;
                        case "IMT_ITMNM":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.SKUName;
                            break;
                        case "IMT_PRICE":
                            CL = (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax;
                            if (w_Row == mGrid.g_MK_Max_Row - 1)
                                lastCell = true;
                            break;
                    }

                    
                    //画面の内容を配列へセット
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);



                    if (CL == -1)
                        return;

                    //    //チェック処理
                    if (CheckGrid(CL, w_Row) == false)
                    {
                        //配列の内容を画面へセット
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                        //Focusセット処理
                        w_ActCtl.Focus();
                        return;
                    }

                    if (lastCell)
                    {
                        w_ActCtl.Focus();
                        return;
                    }

                    //    //あたかもTabキーが押されたかのようにする
                    //    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    //    //this.ProcessTabKey(!e.Shift);
                    //    //行き先がなかったら移動しない
                    S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    if (CL == (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax)
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

        private void DetailControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
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
                                this.ProcessTabKey(!e.Shift);
                        }
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
                SetFuncKeyAll(this, "111111001001");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

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
                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                                        //case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                                        case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                                        //case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                                        //case (int)ClsGridCustomerSKUPric.ColNO.SKUName:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Space1:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                break;
                                            }
                                    }
                                }
                            }
                            mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                        }
                        else
                        {
                            //IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                //Scr_Lock(0, 0, 0);

                                Scr_Lock(1, mc_L_END, 1);  // フレームのロック解除
                                                           // detailControls[(int)EIndex.Date].Text = bbl.GetDate();
                                F9Visible = false;

                                SetFuncKeyAll(this, "111111001001");
                            }
                            else
                            {
                                //keyControls[(int)EIndex.MitsumoriNO].Enabled = true;
                                //ScMitsumoriNO.BtnSearch.Enabled = true;
                                //keyControls[(int)EIndex.CopyMitsumoriNO].Text = "";
                                //keyControls[(int)EIndex.CopyMitsumoriNO].Enabled = false;
                                //ScCopyMitsumoriNO.BtnSearch.Enabled = false;

                                Scr_Lock(1, mc_L_END, 0);   // フレームのロック
                                this.Vsb_Mei_0.TabStop = false;

                                SetFuncKeyAll(this, "111111001010");
                            }

                            //SetEnabled(false);

                        }
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
                                for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                {
                                    switch (w_Col)
                                    {
                                        case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                                        // case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                                        case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                                        //case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                                        //case (int)ClsGridCustomerSKUPric.ColNO.SKUName:    // 
                                        case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Remarks:
                                        case (int)ClsGridCustomerSKUPric.ColNO.Space1:
                                            {
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;

                                                break;
                                            }
                                    }
                                }
                                mGrid.S_DispFromArray(0, ref Vsb_Mei_0);
                            }
                        }
                        else
                        {
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
                            pnl_Body.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //Scr_Lock(0, 0, 0);
                            if (OperationMode == EOperationMode.DELETE)
                            {
                                SetFuncKeyAll(this, "111111000011");
                                Scr_Lock(0, 0, 1);
                            }
                            else
                            {
                                //CboStoreCD.Enabled = false;
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

        private void Set_GridTabStop(bool pTabStop)
        {
            int w_Row;
            int w_Col;
            int w_CtlRow;
            try
            {
                // 画面上にEnable=Trueのコントロールがひとつしかない時はTab,Shift+Tab,↑,↓押下時にKeyExitが発生しないため、
                // 明細がスクロールしなくなる。回避策として、そのパターンが起こりうる時のみ、IMT_DMY_0.TabStopをTrueにする。
                //if (OperationMode == EOperationMode.UPDATE)
                //    txtStartDateFrom.TabStop = pTabStop;
                //else
                //    txtStartDateFrom.TabStop = false;

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

        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridCustomerSKUPric.gc_P_GYO <= ClsGridCustomerSKUPric.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridCustomerSKUPric.gMxGyo;
                m_EnableCnt = ClsGridCustomerSKUPric.gMxGyo;
            }
            mGrid.g_MK_Ctl_Row = ClsGridCustomerSKUPric.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridCustomerSKUPric.gc_MaxCL;

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
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        {
                            Search.CKM_SearchControl sctl = (Search.CKM_SearchControl)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.TxtCode.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.TxtCode.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.TxtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.TxtCode.Tag = W_CtlRow.ToString();
                            sctl.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
                        }
                        if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                        {
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.TxtCode.Tag = W_CtlRow.ToString();

                        }
                    }

                }
            }
            // 明細部の状態(初期状態) をセット 
            //To be Proceed. . . . 
            //
            mGrid.g_MK_State = new ClsGridBase.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row];
            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridCustomerSKUPric.ST_DArray_Grid[mGrid.g_MK_Max_Row];

            //SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridBase.GridColor);//, "1");      // 2行目(偶数行) 水色
            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];
            for (int i = (int)ClsGridCustomerSKUPric.ColNO.GYONO; i <= (int)ClsGridCustomerSKUPric.ColNO.COUNT - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }
            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridCustomerSKUPric.ColNO.COUNT; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;
                            break;
                    }

                    mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl.TabIndex = tabindex;
                    tabindex++;
                }
            }
            Set_GridTabStop(false);

        }

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
                //画面範囲の時
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0, w_CtlRow, w_CtlRow);


            //w_AllFlg = true;

            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                switch (w_Col)
                {
                    case (int)ClsGridCustomerSKUPric.ColNO.CustomerName:
                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                        break;
                    case (int)ClsGridCustomerSKUPric.ColNO.SKUName:
                    case (int)ClsGridCustomerSKUPric.ColNO.SKUCD:
                        mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = true;
                        mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                        break;
                }


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

        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);
            //Control1
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 0].CellCtl = IMT_GYONO_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 0].CellCtl = SC_Customer_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 0].CellCtl = IMT_CUSTNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 0].CellCtl = IMT_SDate_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 0].CellCtl = IMT_EDate_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 0].CellCtl = SC_ITEM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 0].CellCtl = IMT_ITMCD_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 0].CellCtl = IMT_ITMNM_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 0].CellCtl = IMT_PRICE_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 0].CellCtl = IMT_REMARK_0;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 0].CellCtl = Space_0;
            //Control2
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 1].CellCtl = IMT_GYONO_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 1].CellCtl = SC_Customer_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 1].CellCtl = IMT_CUSTNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 1].CellCtl = IMT_SDate_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 1].CellCtl = IMT_EDate_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 1].CellCtl = SC_ITEM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 1].CellCtl = IMT_ITMCD_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 1].CellCtl = IMT_ITMNM_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 1].CellCtl = IMT_PRICE_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 1].CellCtl = IMT_REMARK_1;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 1].CellCtl = Space_1;
            //Control3
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 2].CellCtl = IMT_GYONO_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 2].CellCtl = SC_Customer_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 2].CellCtl = IMT_CUSTNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 2].CellCtl = IMT_SDate_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 2].CellCtl = IMT_EDate_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 2].CellCtl = SC_ITEM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 2].CellCtl = IMT_ITMCD_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 2].CellCtl = IMT_ITMNM_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 2].CellCtl = IMT_PRICE_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 2].CellCtl = IMT_REMARK_2;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 2].CellCtl = Space_2;

            //Control4
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 3].CellCtl = IMT_GYONO_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 3].CellCtl = SC_Customer_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 3].CellCtl = IMT_CUSTNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 3].CellCtl = IMT_SDate_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 3].CellCtl = IMT_EDate_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 3].CellCtl = SC_ITEM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 3].CellCtl = IMT_ITMCD_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 3].CellCtl = IMT_ITMNM_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 3].CellCtl = IMT_PRICE_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 3].CellCtl = IMT_REMARK_3;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 3].CellCtl = Space_3;

            //Control5
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 4].CellCtl = IMT_GYONO_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 4].CellCtl = SC_Customer_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 4].CellCtl = IMT_CUSTNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 4].CellCtl = IMT_SDate_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 4].CellCtl = IMT_EDate_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 4].CellCtl = SC_ITEM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 4].CellCtl = IMT_ITMCD_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 4].CellCtl = IMT_ITMNM_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 4].CellCtl = IMT_PRICE_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 4].CellCtl = IMT_REMARK_4;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 4].CellCtl = Space_4;

            //Control6
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 5].CellCtl = IMT_GYONO_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 5].CellCtl = SC_Customer_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 5].CellCtl = IMT_CUSTNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 5].CellCtl = IMT_SDate_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 5].CellCtl = IMT_EDate_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 5].CellCtl = SC_ITEM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 5].CellCtl = IMT_ITMCD_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 5].CellCtl = IMT_ITMNM_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 5].CellCtl = IMT_PRICE_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 5].CellCtl = IMT_REMARK_5;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 5].CellCtl = Space_5;

            //Control7
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 6].CellCtl = IMT_GYONO_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 6].CellCtl = SC_Customer_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 6].CellCtl = IMT_CUSTNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 6].CellCtl = IMT_SDate_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 6].CellCtl = IMT_EDate_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 6].CellCtl = SC_ITEM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 6].CellCtl = IMT_ITMCD_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 6].CellCtl = IMT_ITMNM_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 6].CellCtl = IMT_PRICE_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 6].CellCtl = IMT_REMARK_6;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 6].CellCtl = Space_6;

            //Control8
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 7].CellCtl = IMT_GYONO_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 7].CellCtl = SC_Customer_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 7].CellCtl = IMT_CUSTNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 7].CellCtl = IMT_SDate_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 7].CellCtl = IMT_EDate_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 7].CellCtl = SC_ITEM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 7].CellCtl = IMT_ITMCD_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 7].CellCtl = IMT_ITMNM_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 7].CellCtl = IMT_PRICE_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 7].CellCtl = IMT_REMARK_7;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 7].CellCtl = Space_7;

            //Control9
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 8].CellCtl = IMT_GYONO_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 8].CellCtl = SC_Customer_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 8].CellCtl = IMT_CUSTNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 8].CellCtl = IMT_SDate_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 8].CellCtl = IMT_EDate_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 8].CellCtl = SC_ITEM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 8].CellCtl = IMT_ITMCD_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 8].CellCtl = IMT_ITMNM_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 8].CellCtl = IMT_PRICE_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 8].CellCtl = IMT_REMARK_8;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 8].CellCtl = Space_8;
            //Control10
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.GYONO, 9].CellCtl = IMT_GYONO_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerCD, 9].CellCtl = SC_Customer_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.CustomerName, 9].CellCtl = IMT_CUSTNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate, 9].CellCtl = IMT_SDate_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate, 9].CellCtl = IMT_EDate_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.JANCD, 9].CellCtl = SC_ITEM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUCD, 9].CellCtl = IMT_ITMCD_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SKUName, 9].CellCtl = IMT_ITMNM_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax, 9].CellCtl = IMT_PRICE_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Remarks, 9].CellCtl = IMT_REMARK_9;
            mGrid.g_MK_Ctrl[(int)ClsGridCustomerSKUPric.ColNO.Space1, 9].CellCtl = Space_9;
        }

        #endregion

        #region Error Check
        public bool ErrorCheck(int mode)
        {
            if (!txtStartDate.DateCheck())
                return false;
            if (!txtEndDate.DateCheck())
                return false;
            if (!string.IsNullOrWhiteSpace(txtStartDate.Text) && !string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                int result = txtStartDate.Text.CompareTo(txtEndDate.Text);
                if (result > 0)
                {
                    bbl.ShowMessage("E104");
                    txtStartDate.Focus();
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(ScCustomer_Start.TxtCode.Text))
            {
                if (ScCustomer_Start.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScCustomer_Start.SetFocus(1);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(ScCustomer_End.TxtCode.Text))
            {
                if (ScCustomer_End.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScCustomer_End.SetFocus(1);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(ScCustomer_End.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCustomer_End.TxtCode.Text))
            {
                int result1 = ScCustomer_Start.TxtCode.Text.CompareTo(ScCustomer_End.TxtCode.Text);
                if (result1 > 0)
                {
                    bbl.ShowMessage("E104");
                    ScCustomer_Start.SetFocus(1);
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(ScSKUCD_Start.TxtCode.Text))
            {
                if (ScSKUCD_Start.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScSKUCD_Start.SetFocus(1);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(ScSKUCD_End.TxtCode.Text))
            {
                if (ScSKUCD_End.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    ScSKUCD_End.SetFocus(1);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(ScCustomer_End.TxtCode.Text) && !string.IsNullOrWhiteSpace(ScCustomer_End.TxtCode.Text))
            {
                int skuresult = ScSKUCD_Start.TxtCode.Text.CompareTo(ScSKUCD_End.TxtCode.Text);
                if (skuresult > 0)
                {
                    bbl.ShowMessage("E104");
                    ScSKUCD_Start.SetFocus(1);
                    return false;
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

            w_Ctrl = Btn_F1;

            /*IMT_DMY_0.Focus();*/       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvSet, (int)ClsGridCustomerSKUPric.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }

        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.DateStart:
                case (int)EIndex.DateEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;
                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    if (index == (int)EIndex.DateEnd)
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
                case (int)EIndex.CustomerCD_Start:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScCustomer_Start.LabelText = "";
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()     // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sbl = new Customer_BL();
                    bool ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        ScCustomer_Start.LabelText = mce.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScCustomer_Start.LabelText = "";
                        return false;
                    }

                    break;
                case (int)EIndex.CustomerCD_End:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScCustomer_End.LabelText = "";
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mcee = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()     // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sble = new Customer_BL();
                    bool rete = sble.M_Customer_Select(mcee);
                    if (rete)
                    {
                        ScCustomer_End.LabelText = mcee.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScCustomer_End.LabelText = "";
                        return false;
                    }
                    if (index == (int)EIndex.CustomerCD_End)
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
                case (int)EIndex.SKUCD_Start:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScSKUCD_Start.LabelText = "";
                        return true;
                    }
                    if (!ScSKUCD_Start.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSKUCD_Start.SetFocus(1);
                        return false;
                    }

                    break;

                case (int)EIndex.SKUCD_End:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScSKUCD_End.LabelText = "";
                        return true;
                    }
                    if (!ScSKUCD_End.SelectData())
                    {
                        bbl.ShowMessage("E101");
                        ScSKUCD_End.SetFocus(1);
                        return false;
                    }

                    if (index == (int)EIndex.SKUCD_End)
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

            }

            return true;
        }

        public void CheckData(bool set, bool copy = false)
        {
            if (ErrorCheck(11))
            {
                mcskue = GetSKUPriceInfo();
                dtDelete = null;
                DataTable dtSKUPrice = dtDelete = mcskupbl.M_CustomerSKUPriceSelectData(mcskue);


                if (dtSKUPrice.Rows.Count > 0)
                {
                    S_Clear_Grid();
                    SetMultiColNo(dtSKUPrice);

                }
                else
                {
                    bbl.ShowMessage("E128");
                    Scr_Clr(1);
                    txtStartDate.Focus();
                }
                if (OperationMode == EOperationMode.UPDATE)
                {
                    S_BodySeigyo(1, 1);
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    detailControls[(int)EIndex.DateStart].Focus();
                }
                else
                {
                    S_BodySeigyo(2, 1);
                    //配列の内容を画面にセット
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }

            }
        }

        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false)
        {
            if (!chkAll && !changeYmd)
            {
                int w_CtlRow = row - Vsb_Mei_0.Value;
                if (w_CtlRow < ClsGridCustomerSKUPric.gc_P_GYO)
                    if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                    {
                        if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                            return false;
                    }
            }

            switch (col)
            {
                case (int)ClsGridCustomerSKUPric.ColNO.TekiyouKaisiDate:
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TekiyouKaisiDate))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    if (!bbl.CheckDate(mGrid.g_DArray[row].TekiyouKaisiDate))
                    {
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    break;

                case (int)ClsGridCustomerSKUPric.ColNO.TekiyouShuuryouDate:
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TekiyouShuuryouDate))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    if (!bbl.CheckDate(mGrid.g_DArray[row].TekiyouShuuryouDate))
                    {
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    int result = mGrid.g_DArray[row].TekiyouShuuryouDate.CompareTo(mGrid.g_DArray[row].TekiyouKaisiDate);
                    if (result < 0)
                    {
                        bbl.ShowMessage("E104");
                        return false;
                    }
                    break;

                case (int)ClsGridCustomerSKUPric.ColNO.CustomerCD:
                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].CustomerCD))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    M_Customer_Entity mcee = new M_Customer_Entity
                    {
                        CustomerCD = mGrid.g_DArray[row].CustomerCD,
                        ChangeDate = bbl.GetDate()     // detailControls[(int)EIndex.MitsumoriDate].Text
                    };
                    Customer_BL sble = new Customer_BL();
                    bool rete = sble.M_Customer_Select(mcee);
                    if (rete)
                    {
                        mGrid.g_DArray[row].CustomerName = mcee.CustomerName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        mGrid.g_DArray[row].CustomerName = "";
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TekiyouKaisiDate) && !string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JANCD))
                    {
                        DataTable dtSKUPrice = bbl.SimpleSelect1("73", mGrid.g_DArray[row].TekiyouKaisiDate, mGrid.g_DArray[row].CustomerCD, mGrid.g_DArray[row].AdminNO);
                        if (dtSKUPrice.Rows.Count > 0)
                        {
                            bbl.ShowMessage("E132");
                            return false;
                        }
                    }
                    break;
                case (int)ClsGridCustomerSKUPric.ColNO.JANCD:
                    string ymd = detailControls[(int)EIndex.Date].Text;

                    if (!changeYmd)
                    {
                        if (mGrid.g_DArray[row].JANCD == mGrid.g_DArray[row].OldJanCD) //chkAll &&  change
                            return true;
                    }

                    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JANCD))
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }


                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = mGrid.g_DArray[row].JANCD,
                        ChangeDate = ymd
                    };

                    if (mGrid.g_DArray[row].JANCD == mGrid.g_DArray[row].OldJanCD || string.IsNullOrWhiteSpace(mGrid.g_DArray[row].OldJanCD))
                    {
                        mse.SKUCD = mGrid.g_DArray[row].SKUCD;
                        mse.AdminNO = mGrid.g_DArray[row].AdminNO;
                    }

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E107");
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
                            frmSKU.parChangeDate = ymd;
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
                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();
                        mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                        mGrid.g_DArray[row].SKUName = selectRow["SKUName"].ToString();
                        mGrid.g_DArray[row].AdminNO = selectRow["AdminNO"].ToString();


                        mGrid.g_DArray[row].OldJanCD = mGrid.g_DArray[row].JANCD;

                        Grid_NotFocus(0, row);
                    }

                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[row].CustomerCD) && !string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TekiyouKaisiDate))
                    {
                        DataTable dtSKUPrice = bbl.SimpleSelect1("73", mGrid.g_DArray[row].TekiyouKaisiDate, mGrid.g_DArray[row].CustomerCD, mGrid.g_DArray[row].AdminNO);
                        if (dtSKUPrice.Rows.Count > 0)
                        {
                            bbl.ShowMessage("E132");
                            return false;
                        }
                    }

                    break;
                case (int)ClsGridCustomerSKUPric.ColNO.SalePriceOutTax:
                    if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax))
                        mGrid.g_DArray[row].SalePriceOutTax = string.Format("{0:#,##0}", bbl.Z_Set(mGrid.g_DArray[row].SalePriceOutTax));
                    else
                    {
                        bbl.ShowMessage("E102");
                        return false;
                    }


                    break;
            }

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            return true;
        }

        #endregion

        protected override void EndSec()
        {
            this.Close();
        }

        private void FrmMasterTouroku_CustomerSKUPrice_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);
        }
    }
}
