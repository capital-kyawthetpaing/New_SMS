using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using CKM_Controls;
using Search;
using Entity;
namespace MasterTouroku_TenzikaiShouhin
{
    public partial class MasterTouroku_TenzikaiShouhin : FrmMainForm
    {
        MasterTouroku_TenzikaiShouhin_BL tbl;
        private const string ProID = "MasterTouroku_TenzikaiShouhin";
        private const string ProNm = "展示会受注登録";
        private const short mc_L_END = 3; // ロック用
        private const string TempoNouhinsyo = "TenzikaiJuchuuTourou.exe";
        Base_BL bl;
        bool excel = false;
        M_TenzikaiShouhin_Entity mt;
        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        ClsGridMasterTanzi mGrid = new ClsGridMasterTanzi();
        private int m_EnableCnt = 0;
        private int m_dataCnt = 0;        // 修正削除時に画面に展開された行数
        private int m_MaxJyuchuGyoNo;
        bool checkmei = false;
        private System.Windows.Forms.Control previousCtrl;
        public MasterTouroku_TenzikaiShouhin()
        {

            InitializeComponent();
            bl = new Base_BL();
            tbl = new MasterTouroku_TenzikaiShouhin_BL();
            mt = new M_TenzikaiShouhin_Entity();
        }
        private enum EsearchKbn : short
        {
            Null,
            Product,
            Vendor,
            Brand,
            Segment,
            Tani,
        }

        private enum Eindex : int
        {
            SCTenzikai,
            SCShiiresaki,
            Nendo,
            Season,
            SCBrand,
            SCSegment,
            SCCTenzikai,
            SCCShiiresaki,
            CNendo,
            CSeason,
            SCCBrand,
            SCCSegment,
            StartDate,
            KDenwa1,
            KDenwa2,
            KDenwa3,
            HDenwa2,
            HDenwa3,
            YoteiKinShuu,
            Count
        }
        private void MasterTouroku_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_TenzikaiShouhin";
            this.SetFunctionLabel(EProMode.INPUT);
            this.InitialControlArray();
            this.S_SetInit_Grid();
            Btn_F7.Text = "行削除（F7）";
            Btn_F8.Text = "行追加(F8)";
            Btn_F11.Text = "反映処理(F11)";
            Scr_Clr(0);

            BindCombo_Details();
            StartProgram();

            SC_Segment.Value1 = "226";
            string ymd = bl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_copyseason.Bind(ymd);
            CB_Copyyear.Bind(ymd);
            SetRequireField();
        }
        private void BindCombo_Details()
        {
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, W_CtlRow].CellCtl;
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Key");
                dt1.Columns.Add("Value");

                //dt1.Rows.Add("", "-1");
                dt1.Rows.Add("上旬", "1");
                dt1.Rows.Add("中旬", "2");
                dt1.Rows.Add("下旬", "3");
                sctl.BindCombo("Value", "Key", dt1);


                var sctl1 = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, W_CtlRow].CellCtl;
                var dt = new DataTable();
                dt.Columns.Add("Key");
                dt.Columns.Add("Value");

                //dt.Rows.Add("", "-1");
                dt.Rows.Add("非課税", "0");
                dt.Rows.Add("通常課税", "1");
                dt.Rows.Add("軽減課税", "2");

                sctl1.BindCombo("Value", "Key", dt);
            }
        }
        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
           // MoveNextControl(e);

        }


        private void Scr_Clr(short Kbn)
        {
            //カスタムコントロールのLeave処理を先に走らせるため
            //  IMT_DMY_0.Focus();

            if (Kbn == 0)
            {
                foreach (Control ctl in detailControls)
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
               //lblDisp.Text = "未売上";
            }
            int a = 0;
            try
            {

                foreach (Control ctl in detailControls)
                {
                    a++;
                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox))) // Check all > Uncheck
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else if (ctl.Parent is CKM_SearchControl cs)
                    {
                        cs.LabelText = "";
                    }
                    else if (ctl.GetType().Equals(typeof(Panel)))   // Check only First
                    {
                        //kr_1.Checked = true;
                        ////  kr_2.Checked = false;
                        //hr_3.Checked = true;
                        //// hr_4.Checked = false;
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                    {
                        try
                        {
                            ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                        }
                        catch
                        {
                            ((CKM_Controls.CKM_ComboBox)ctl).SelectedValue = -1;
                        }
                    }
                    else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                    {
                        //顧客情報ALLクリア
                       // ClearCustomerInfo(0);
                       // ClearCustomerInfo(1);
                    }

                    else if (ctl is Button)
                    {
                       // ClearCustomerInfo(0);
                        //ClearCustomerInfo(1);
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }
            }
            catch
            {
                var d = a;
            }

           
            S_Clear_Grid();  
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

                    }

                    // 使用不可行は固定色を設定
                    if (w_Row > m_EnableCnt - 1)
                    {
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = false;
                        mGrid.g_MK_State[w_Col, w_Row].Cell_Color = GridBase.ClsGridBase.MaxGyoColor;
                    }

                    // クリック以外ではフォーカス入らない列の設定(Cell_Selectable)
                    //switch (w_Col)
                    //{
                    //    case (int)ClsGridTenjikai.ColNO.Site:
                    //    case (int)ClsGridTenjikai.ColNO.Zaiko:
                    //        {
                    //            // ボタン
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

            //Btn_SelectAll.Enabled = false;
            //Btn_NoSelect.Enabled = false;
        }

        private void InitialControlArray()
        {

            detailControls = new Control[] { SC_Tenzikai.TxtCode,SC_Vendor.TxtCode,CB_Year, CB_Season,SC_Brand.TxtCode,SC_Segment.TxtCode,SC_CopyTenzikai.TxtCode,SC_CopyVendor.TxtCode,
                                             CB_Copyyear,CB_copyseason, SC_copybrand.TxtCode,SC_copysegmet.TxtCode,
                //BT_Display,BT_meisai,BT_SKUCheck,
                TB_StartDate };
            detailLabels = new Control[] { SC_Vendor,SC_Brand,SC_Segment,SC_CopyVendor,SC_copybrand,SC_copysegmet };
            searchButtons = new Control[] { SC_Tenzikai.BtnSearch, SC_Vendor.BtnSearch, SC_Brand.BtnSearch, SC_Segment.BtnSearch, SC_CopyTenzikai.BtnSearch ,
                                            SC_CopyVendor.BtnSearch,SC_copybrand.BtnSearch,SC_copysegmet.BtnSearch };
            
            foreach (var c in detailControls)
            {
                c.KeyDown += C_KeyDown;
                c.Enter += C_Enter;
                
            }
        }

        private void ChangeOperationMode(EOperationMode mode)
        {
            OperationMode = mode; 
            Scr_Clr(0);

            S_BodySeigyo(0, 0);
            S_BodySeigyo(0, 1);
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            switch (mode)
            {
                case EOperationMode.INSERT:
                    detailControls[(int)Eindex.SCTenzikai].Focus();
                    BT_SKUCheck.Enabled = false;
                    detailControls[(int)Eindex.StartDate].Enabled = false;
                    detailControls[(int)Eindex.SCCTenzikai].Enabled = true;
                    detailControls[(int)Eindex.SCCShiiresaki].Enabled = true;
                    detailControls[(int)Eindex.SCCBrand].Enabled = true;
                    detailControls[(int)Eindex.SCCSegment].Enabled = true;
                    detailControls[(int)Eindex.CNendo].Enabled = true;
                    detailControls[(int)Eindex.CSeason].Enabled = true;
                    Clear(panel1);
                    Clear(panel_2);
                    break;

                case EOperationMode.UPDATE:
                     BT_SKUCheck.Enabled = true;
                    detailControls[(int)Eindex.SCTenzikai].Focus();
                    detailControls[(int)Eindex.StartDate].Enabled = true;
                    detailControls[(int)Eindex.SCCTenzikai].Enabled = false;
                    detailControls[(int)Eindex.SCCShiiresaki].Enabled = false;
                    detailControls[(int)Eindex.SCCBrand].Enabled = false;
                    detailControls[(int)Eindex.SCCSegment].Enabled = false;
                    detailControls[(int)Eindex.CNendo].Enabled = false;
                    detailControls[(int)Eindex.CSeason].Enabled = false;

                    break;

                case EOperationMode.DELETE:
                case EOperationMode.SHOW:
                    EnablePanel(PanelHeader);
                    BT_SKUCheck.Enabled = false;
                    detailControls[(int)Eindex.SCCTenzikai].Enabled = false;
                    detailControls[(int)Eindex.SCCShiiresaki].Enabled = false;
                    detailControls[(int)Eindex.SCCBrand].Enabled = false;
                    detailControls[(int)Eindex.SCCSegment].Enabled = false;
                    detailControls[(int)Eindex.CNendo].Enabled = false;
                    detailControls[(int)Eindex.CSeason].Enabled = false;
                    detailControls[(int)Eindex.StartDate].Enabled = false;
                    break;

            }
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
                case 5:
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
                    ADD_SUB();
                    break;

                case 9://F10複写
                    CPY_SUB();
                    break;

                case 8: //F9:検索
                    EsearchKbn kbn = EsearchKbn.Null;

                    if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
                    {
                        return;
                    }

                   
                    break;
                case 10:   //F11
                    {	
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                            {
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                return;
                            }
                        else
                        {
                            if(ErrorCheck())
                            {
                                if (!String.IsNullOrEmpty(TB_StartDate.Text))
                                {
                                    //if (TB_StartDate.DateCheck())
                                    //{
                                        F11();
                                    //}
                                    //else
                                    //{
                                    //    bbl.ShowMessage("E103");
                                    //    TB_StartDate.Focus();
                                    //}
                                }
                                else
                                {
                                    bbl.ShowMessage("E102");
                                    TB_StartDate.Focus();
                                }
                            }
                        }
                       
                    }
                    break;
                case 11:    //F12:登録
                    {

                        if (OperationMode == EOperationMode.DELETE)
                        { 	
                            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                                return;
                        }
                        else
                        {	
                            if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                                return;
                        }
                        this.ExecSec();
                        break;
                    }
            }
        }
        private bool IsallUnChecked()
        {
            int w_Row;
            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {
                if (mGrid.g_DArray[w_Row].Chk == true)
                {
                    return true;
                }
            }
            return false;
         }

        private void Col(DataTable dt)
        {
            dt.Columns.Add("chkflg");
            dt.Columns.Add("JanCD");
            dt.Columns.Add("SKUCD");
            dt.Columns.Add("SKUName");
            dt.Columns.Add("ColorCD");
            dt.Columns.Add("ColorName");
            dt.Columns.Add("SizeCD");
            dt.Columns.Add("SizeName");
            dt.Columns.Add("HanbaiDateMonth");
            dt.Columns.Add("HanbaiDate");
            dt.Columns.Add("Shiiretanka");
            dt.Columns.Add("JoudaiTanka");
            dt.Columns.Add("SalePriceOutTax");
            dt.Columns.Add("Rank1");
            dt.Columns.Add("Rank2");
            dt.Columns.Add("Rank3");
            dt.Columns.Add("Rank4");
            dt.Columns.Add("Rank5");
            dt.Columns.Add("BrandCD");
            dt.Columns.Add("ExhibitionSegmentCD");
            dt.Columns.Add("TaniCD");
            dt.Columns.Add("TaxRateFlg");
            dt.Columns.Add("Remark");
            dt.Columns.Add("ExhibitioinCommonCD");
        }
        private void F11()
        {
            SKUChek();
            if (!IsallUnChecked())
            {
                bl.ShowMessage("E257");
                return;
            }
            DataTable dtrest = GetGridData();

            mt = GetEntity(dtrest);
            if (!tbl.M_Tenzikaishouhin_DeleteUpdate(mt))
            {
                bbl.ShowMessage("S001");
            }
                       
        }

        private DataTable GetGridData()
        {
            var dtrest = new DataTable();
            Col(dtrest);
            for (int w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {
                if (!String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD))
                {
                    dtrest.Rows.Add(new object[]{
                        mGrid.g_DArray[w_Row].Chk ? "1" : "0",
                        mGrid.g_DArray[w_Row].JANCD,
                        mGrid.g_DArray[w_Row].SKUCD,
                        mGrid.g_DArray[w_Row].SKUName,
                        mGrid.g_DArray[w_Row].ColorCD,
                        mGrid.g_DArray[w_Row].ColorName,
                        mGrid.g_DArray[w_Row].SizeCD,
                        mGrid.g_DArray[w_Row].SizeName,
                        mGrid.g_DArray[w_Row].HanbaiYoteiDateMonth,
                        
                     ((CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, w_Row].CellCtl).SelectedValue == null? ""
                      :((CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, w_Row].CellCtl).SelectedValue  ,
                     
                        mGrid.g_DArray[w_Row].Shiiretanka == "" ? "0" :mGrid.g_DArray[w_Row].Shiiretanka ,
                        mGrid.g_DArray[w_Row].JoutaiTanka == "" ? "0" :mGrid.g_DArray[w_Row].JoutaiTanka ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax1 == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax1 ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax2 == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax2 ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax3 == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax3 ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax4 == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax4 ,
                        mGrid.g_DArray[w_Row].SalePriceOutTax5 == "" ? "0" :mGrid.g_DArray[w_Row].SalePriceOutTax5 ,
                        mGrid.g_DArray[w_Row].BrandCD,
                        mGrid.g_DArray[w_Row].SegmentCD,
                        mGrid.g_DArray[w_Row].TaniCD,
                       ( (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, w_Row].CellCtl).SelectedValue  ==  null ? ""
                       :( (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, w_Row].CellCtl).SelectedValue,
                        mGrid.g_DArray[w_Row].Remark,
                        mGrid.g_DArray[w_Row].ExhibitionCommonCD,
                    });
                }
            }
              return dtrest;
         }

        private M_TenzikaiShouhin_Entity GetEntity(DataTable dtrest)
        {
            M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity
            {
                xml = bbl.DataTableToXml(dtrest),
                TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                LastSeason = detailControls[(int)Eindex.Season].Text,
                BrandCD = detailControls[(int)Eindex.SCBrand].Text,
                SegmentCD = detailControls[(int)Eindex.SCSegment].Text,
                ChangeDate= detailControls[(int)Eindex.StartDate].Text,
                InsertOperator = InOperatorCD,
                ProcessMode = ModeText,
                ProgramID = InProgramID,
                Key = InPcID + bbl.GetDate(),
                PC = InPcID,
            };

            return mt;
        }

        private M_TenzikaiShouhin_Entity GetTenzikai()
        {
            M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity
            {
                TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                LastSeason = detailControls[(int)Eindex.Season].Text,
                BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
            };
            return mt;
        }

        private M_TenzikaiShouhin_Entity GetCopyTenzikai()
        {
            M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity
            {
                TenzikaiName = detailControls[(int)Eindex.SCCTenzikai].Text,
                VendorCD = detailControls[(int)Eindex.SCCShiiresaki].Text,
                LastYearTerm = detailControls[(int)Eindex.CNendo].Text,
                LastSeason = detailControls[(int)Eindex.CSeason].Text,
                BranCDFrom = detailControls[(int)Eindex.SCCBrand].Text,
                SegmentCDFrom = detailControls[(int)Eindex.SCCSegment].Text,

            };
            return mt;
        }

        private void InsertUpdate(int Typ)
        {
            if(tbl.M_Tenzikaishouhin_InsertUpdate(mt,Typ))
            {
                bbl.ShowMessage("I101");
                ChangeOperationMode(OperationMode);
            }
            else
            {
                bbl.ShowMessage("S001");
            }

        }

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
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            for (int i = w_Row; i < mGrid.g_MK_Max_Row - 1; i++)
            {
                int w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                //次行をコピー
                mGrid.g_DArray[i] = mGrid.g_DArray[i + 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
            }

            int col = (int)ClsGridMasterTanzi.ColNO.JANCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット

            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            //フォーカスセット

            S_BodySeigyo(4, 2); ///
            //現在行へ
            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
            w_Act.Focus();
        }
        private void CPY_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            w_Act = this.ActiveControl;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

            if (mGrid.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            w_Row = w_CtlRow + Vsb_Mei_0.Value;

            //先頭行のとき、複写不可
            if (w_Row == 0)
                return;

            //画面より配列セット 
            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            int col = (int)ClsGridMasterTanzi.ColNO.JANCD;

            //コピー行より下の明細を1行ずつずらす（内容コピー）
            for (int i = mGrid.g_MK_Max_Row - 1; i >= w_Row; i--)
            {
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);          //行番号 退避

                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];

                //退避内容を戻す
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();          //行番号
                //mGrid.g_DArray[i].TenjiRow = null;
                if (i.Equals(w_Row))
                {
                    mGrid.g_DArray[i].TenzikaiRow = null;
                    //前の行をコピーしてできた新しい行
                    //mGrid.g_DArray[i].JuchuuNO = "";
                    //mGrid.g_DArray[i].juchuGyoNO = 0;
                    //mGrid.g_DArray[i].copyJuchuGyoNO = 0;
                    //mGrid.g_DArray[i].KariHikiateNO = "";
                }

                Grid_NotFocus(col, i);
            }

            //状態もコピー
            // ※ 前行と状態が違うとき注意、この部分変更要 (修正元のあるなしで 入力可能項目が変わる場合など)
            for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            {
                mGrid.g_MK_State[w_Col, w_Row] = mGrid.g_MK_State[w_Col, w_Row - 1];
            }

            string ymd =bbl.GetDate();

            if (string.IsNullOrWhiteSpace(ymd))
                ymd = bbl.GetDate();

            // CheckHikiate(w_Row, ymd);
            Grid_NotFocus(col, w_Row);
            

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, SC_Tenzikai, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);
        }

        private void ADD_SUB()
        {
            Control w_Act = ActiveControl;
            int w_Row;
            int w_Gyo;

            if (ActiveControl.GetType().Equals(typeof(CKM_Controls.CKM_Button)))
                w_Act = previousCtrl;

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
                w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);         

               
                mGrid.g_DArray[i] = mGrid.g_DArray[i - 1];
                mGrid.g_DArray[i].GYONO = w_Gyo.ToString();
              //  w_Act.BackColor = mGrid.F_GetBackColor_MK(w_Col, i);
            }
           
            w_Gyo = Convert.ToInt16(mGrid.g_DArray[w_Row].GYONO);
            // 一行クリア
            Array.Clear(mGrid.g_DArray, w_Row, 1);
            mGrid.g_DArray[w_Row].GYONO = w_Gyo.ToString();         
            mGrid.g_DArray[w_Row].TenzikaiRow = null;

            int col = (int)ClsGridMasterTanzi.ColNO.JANCD;
            Grid_NotFocus(col, w_Row);

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            S_BodySeigyo(4, 2);
            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl, w_Row, col, ActiveControl, Vsb_Mei_0, w_Row, col);

        }
     

        private void S_SetInit_Grid()
        {
            int W_CtlRow;  // Grid Properties

            if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;               // プログラムＭ内最大行数
                m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
            }
            //else
            //{
            //    mGrid.g_MK_Max_Row = ClsGridMitsumori.gc_P_GYO;
            //    m_EnableCnt = ClsGridMitsumori.gMxGyo;
            //}

            mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;

            // スクロールが取れるValueの最大値 (画面の最下行にデータの最下行が来た時点の Value)
            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

            // スクロールの設定
            this.Vsb_Mei_0.LargeChange = mGrid.g_MK_Ctl_Row - 1;
            this.Vsb_Mei_0.SmallChange = 1;
            this.Vsb_Mei_0.Minimum = 0;
            // Valueはこの値までとることは不可能にしないといけないが、LargeChangeの分を余分に入れないとスクロールバーを使用して最後までスクロールすることができなくなる
            this.Vsb_Mei_0.Maximum = mGrid.g_MK_MaxValue + this.Vsb_Mei_0.LargeChange - 1;


            // コントロールを配列にセット
            S_SetControlArray(); // g_mk_ctrl // Control Assigned

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
                            CKM_Controls.CKM_TextBox sctl = (CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Tag = W_CtlRow.ToString();
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Enter += new System.EventHandler(GridControl_Enter);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.Leave += new System.EventHandler(GridControl_Leave);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.EnabledChanged += new EventHandler(GridControl_EnableChnaged);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                        {
                            CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            sctl.Enter += new System.EventHandler(GridControl_Enter);
                            sctl.Leave += new System.EventHandler(GridControl_Leave);
                            sctl.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            sctl.Tag = W_CtlRow.ToString();
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(Button)))
                        {
                            Button btn = (Button)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;

                            //if (w_CtlCol == (int)ClsGridJuchuu.ColNO.Site)
                            //    btn.Click += new System.EventHandler(BTN_Site_Click);
                            //else
                            //    btn.Click += new System.EventHandler(BTN_Zaiko_Click);
                        }
                        else if (mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl.GetType().Equals(typeof(GridControl.clsGridCheckBox)))
                        {
                            GridControl.clsGridCheckBox check = (GridControl.clsGridCheckBox)mGrid.g_MK_Ctrl[w_CtlCol, W_CtlRow].CellCtl;
                            check.Tag = W_CtlRow.ToString();
                            check.Enter += new System.EventHandler(GridControl_Enter);
                            check.Leave += new System.EventHandler(GridControl_Leave);
                            check.KeyDown += new System.Windows.Forms.KeyEventHandler(GridControl_KeyDown);
                            check.Click += new System.EventHandler(CHK_Print_Click);
                        }
                    }
                }
            }  // handler Created

            // 明細部の状態(初期状態) をセット 
            mGrid.g_MK_State = new ClsGridMasterTanzi.ST_State_GridKihon[mGrid.g_MK_Ctl_Col, mGrid.g_MK_Max_Row]; // Cell Format


            // データ保持用配列の宣言
            mGrid.g_DArray = new ClsGridMasterTanzi.ST_DArray_Grid[mGrid.g_MK_Max_Row];  // Stored Array
            SetMultiColNo();
            // 行の色
            // 何行で色が切り替わるかの設定。  ここでは 2行一組で繰り返すので 2行分だけ設定
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.WHColor);//, "0");        // 1行目(奇数行) 白
            mGrid.g_MK_CtlRowBkColor.Add(ClsGridMasterTanzi.GridColor);//, "1");      // 2行目(偶数行) 水色

            // フォーカス移動順(表示列も含めて、すべての列を設定する)
            mGrid.g_MK_FocusOrder = new int[mGrid.g_MK_Ctl_Col];

            for (int i = (int)ClsGridMasterTanzi.ColNO.GYONO; i <= (int)ClsGridMasterTanzi.ColNO.Count - 1; i++)
            {
                mGrid.g_MK_FocusOrder[i] = i;
            }

            int tabindex = 1;

            // 項目の形式セット
            for (W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
                for (int W_CtlCol = 0; W_CtlCol < (int)ClsGridMasterTanzi.ColNO.Count; W_CtlCol++)
                {
                    switch (W_CtlCol)
                    {
                        case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                        case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).AllowMinus = true;//ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Price; //Ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).IntegerPart = 9; // ok
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).DecimalPlace = 0;
                            break;

                     
                        case (int)ClsGridMasterTanzi.ColNO.JANCD:
                            mGrid.SetProp_TANKA(ref mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl);      // 単価 JANCD_Detail
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 13;
                            //((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.JANCD;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                             ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 6;
                            // ((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.ブランド;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                             ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.MaxLength = 6;
                            //((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.Width = 50;
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).Value1 = "226";
                            //((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.商品分類;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).TxtCode.Width = 30;
                            ((((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl))).Value1 = "202";
                            ///((CKM_SearchControl)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Stype = CKM_SearchControl.SearchType.単位;
                            break;
                    }
                    switch (W_CtlCol)
                    {
                      
                        case (int)ClsGridMasterTanzi.ColNO.Remark:
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).MaxLength = 80;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Type = CKM_TextBox.Type.Normal;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).Ctrl_Byte = CKM_TextBox.Bytes.半全角;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SKUName:
                            // ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).MaxLength = 80;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).TextAlign = HorizontalAlignment.Left;
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SKUCD:
                            //   (Label)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl). = 30;
                            // ((Label)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).TextAlign = ContentAlignment.MiddleLeft;
                            ((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[W_CtlCol, W_CtlRow].CellCtl).TextAlign = HorizontalAlignment.Left;
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
                    scjan_1.TabStop = pTabStop;
                else
                    scjan_1.TabStop = false;

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
        private void GridControl_EnableChnaged(object sender, EventArgs e)
        {
            if (!(sender as CKM_TextBox).Enabled)
            {
                (sender as CKM_TextBox).Back_Color = CKM_TextBox.CKM_Color.DarkGrey;
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                EsearchKbn kbn = EsearchKbn.Null;
                Control setCtl = null;
                Control sc = ((Control)sender).Parent;

               
                if (((Search.CKM_SearchControl)sc).Name.Contains("scjan"))
                {
                    kbn = EsearchKbn.Product;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("brand"))
                {

                    kbn = EsearchKbn.Brand;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("segment"))
                {

                    kbn = EsearchKbn.Segment;
                }
                if (((Search.CKM_SearchControl)sc).Name.Contains("taniCD"))
                {

                    kbn = EsearchKbn.Tani;
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
        private void GridControl_KeyDown(object sender, KeyEventArgs e)
        {
           
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;


                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int CL = -1;
                    string ctlName = "";
                    if (w_ActCtl.Parent.GetType().Equals(typeof(Search.CKM_SearchControl)))
                        ctlName = w_ActCtl.Parent.Name.Substring(0, w_ActCtl.Parent.Name.LastIndexOf("_"));
                    else
                        ctlName = w_ActCtl.Name.Substring(0, w_ActCtl.Name.LastIndexOf("_"));

                    bool lastCell = false;
                    bool checkgridfirst = false;

                    switch (ctlName)
                    {
                        case "scjan":
                            CL = (int)ClsGridMasterTanzi.ColNO.JANCD;
                            checkgridfirst = true;
                            break;
                        case "sku":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUCD;
                            break;
                        case "shouhin":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUName;
                            break;
                        case "colcd":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorCD;
                            break;
                        case "colorname":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorName;
                            break;
                        case "sizecd":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeCD;
                            break;
                        case "sizeName":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeName;
                            break;
                        case "hyoteidatem":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth;
                            break;
                        case "hyoteidate":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi;
                            break;
                        case "shiire":
                            CL = (int)ClsGridMasterTanzi.ColNO.Shiiretanka;
                            break;
                        case "joutai":
                            CL = (int)ClsGridMasterTanzi.ColNO.JoutaiTanka;
                            break;
                        case "salepriceout":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax;
                            break;
                        case "salepriceout1":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1;
                            break;
                        case "salepriceout2":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2;
                            break;
                        case "salepriceout3":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3;
                            break;
                        case "salepriceout4":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4;
                            break;
                        case "salepriceout5":
                            CL = (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5;
                            break;
                        case "segment":
                            CL = (int)ClsGridMasterTanzi.ColNO.SegmentCD;
                            break;
                        case "brand":
                            CL = (int)ClsGridMasterTanzi.ColNO.BrandCD;
                            break;
                        case "taniCD":
                            CL = (int)ClsGridMasterTanzi.ColNO.TaniCD;
                            break;
                        case "taxrate":
                            CL = (int)ClsGridMasterTanzi.ColNO.TaxRateFlg;
                            break;
                        case "remark":
                            CL = (int)ClsGridMasterTanzi.ColNO.Remark;
                            break;

                        case "chk":
                            CL = (int)ClsGridMasterTanzi.ColNO.Chk;
                            break;

                    }

                    bool changeFlg = false;
                    mGrid.S_DispToArray(Vsb_Mei_0.Value);
                    bool checkall = CheckKey(-1);
                    
                    if (checkall)
                    {
                        if (CheckGrid(CL, w_Row) == false)
                        {
                            if (w_ActCtl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                            {
                                ((CKM_Controls.CKM_ComboBox)w_ActCtl).MoveNext = false;
                            }

                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            w_ActCtl.Focus();
                            return;
                        }

                        if (lastCell)
                        {
                            w_ActCtl.Focus();
                            return;
                        }

                        S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);
                    }

                   
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void S_Grid_0_Event_Enter(int pCol, int pRow, Control pErrSet, Control pMotoControl)
        {
            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, pErrSet, pRow, pCol, pMotoControl, this.Vsb_Mei_0);
        }
        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                if (w_ActCtl is GridControl.clsGridCheckBox chk && chk.Name.Contains("chk_"))
                {
                    if (chk.Parent is Panel pnl)
                    {
                        pnl.BackColor = Color.White; ;
                    }

                    //if (chk.Parent is TextBox pnl)
                    //{
                    //    pnl.BackColor = Color.White; ;
                    //}
                }
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
        private void GridControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                SetFuncKeyAll(this, "111111110101");
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
                w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                w_ActCtl.BackColor = ClsGridMasterTanzi.BKColor;

               

                if (ActiveControl is GridControl.clsGridCheckBox chk && chk.Name.Contains("chk_"))
                {
                    if (chk.Focus())
                    {
                        if (chk.Parent is Panel pnl)
                        {
                            pnl.BackColor = ClsGridMasterTanzi.BKColor; ;
                        }
                    }
                }
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

            //mGrid.g_MK_Ctrl(ClsGridMitsumori.ColNO.DELCK, pCtlRow).GVal(W_Del);

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
        private void S_SetControlArray()
        {
            mGrid.F_CtrlArray_MK(mGrid.g_MK_Ctl_Col, mGrid.g_MK_Ctl_Row);

            // 1>5行目
            try
            {
                for (int d = 0; d < 10; d++)
                {
                    int id = d + 1;
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TB, d].CellCtl = this.Controls.Find("TB_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.GYONO, d].CellCtl = this.Controls.Find("LB_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Chk, d].CellCtl = this.Controls.Find("chk_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JANCD, d].CellCtl = this.Controls.Find("scjan_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUCD, d].CellCtl = this.Controls.Find("sku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUName, d].CellCtl = this.Controls.Find("shouhin_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorCD, d].CellCtl = this.Controls.Find("colcd_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorName, d].CellCtl = this.Controls.Find("colorName_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeCD, d].CellCtl = this.Controls.Find("sizecd_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeName, d].CellCtl = this.Controls.Find("sizeName_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth, d].CellCtl = this.Controls.Find("hyoteidatem_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, d].CellCtl = this.Controls.Find("hyoteidate_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Shiiretanka, d].CellCtl = this.Controls.Find("shiire_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JoutaiTanka, d].CellCtl = this.Controls.Find("joutai_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax, d].CellCtl = this.Controls.Find("salepriceout_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1, d].CellCtl = this.Controls.Find("salepriceout1_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2, d].CellCtl = this.Controls.Find("salepriceout2_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3, d].CellCtl = this.Controls.Find("salepriceout3_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4, d].CellCtl = this.Controls.Find("salepriceout4_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5, d].CellCtl = this.Controls.Find("salepriceout5_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.BrandCD, d].CellCtl = this.Controls.Find("brand_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SegmentCD, d].CellCtl = this.Controls.Find("segment_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaniCD, d].CellCtl = this.Controls.Find("taniCD_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, d].CellCtl = this.Controls.Find("taxrate_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remark, d].CellCtl = this.Controls.Find("remark_" + id, true)[0];
                    // mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remarks, d].sCellCtl = this.Controls.Find("taxrate_" + id, true)[0];

                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message + ex.StackTrace;

            }

        }
       
        private void BT_Display_Click(object sender, EventArgs e)
        {
            switch (OperationMode)
            {
                case EOperationMode.INSERT:
                    if(ErrorCheck())
                    {
                        if (!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
                        {
                            
                            Clear(panel2);
                            ChangeFunKeys();
                            //Scr_Lock(1, 3, 0);
                            mt = new M_TenzikaiShouhin_Entity
                            {
                                TenzikaiName = detailControls[(int)Eindex.SCCTenzikai].Text,
                                VendorCD = detailControls[(int)Eindex.SCCShiiresaki].Text,
                                LastYearTerm = detailControls[(int)Eindex.CNendo].Text,
                                LastSeason = detailControls[(int)Eindex.CSeason].Text,
                                BranCDFrom = detailControls[(int)Eindex.SCCBrand].Text,
                                SegmentCDFrom = detailControls[(int)Eindex.SCCSegment].Text,

                            };
                            DataTable dtshow = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                            if (dtshow != null)
                            {
                                MesaiHyouJi(dtshow);
                                S_BodySeigyo(0, 1);
                                S_BodySeigyo(1, 1);
                                mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                                S_BodySeigyo(4, 0);
                                scjan_1.Focus();
                            }
                            else
                            {
                                bl.ShowMessage("E128");
                            }
                        }
                        else
                        {
                            scjan_1.Focus();
                        }
                    }
                    
                    break;
                case EOperationMode.UPDATE:
                    if (!ErrorCheck())
                    {
                        return;
                    }
                    Clear(panel2);
                    ChangeFunKeys();
                    mt = new M_TenzikaiShouhin_Entity
                    {
                        TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                        VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                        LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                        LastSeason = detailControls[(int)Eindex.Season].Text,
                        BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                        SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                    };
                    DataTable dtmain = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                    if (dtmain != null)
                    {
                        MesaiHyouJi(dtmain);
                        S_BodySeigyo(0, 1);
                        S_BodySeigyo(1, 1);
                        mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                        S_BodySeigyo(4, 0);
                        scjan_1.Focus();
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;

                case EOperationMode.DELETE:
                    if (!ErrorCheck())
                    {
                        return;
                    }
                    Clear(panel2);
                    //ChangeFunKeys();
                    mt = new M_TenzikaiShouhin_Entity
                    {
                        TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                        VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                        LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                        LastSeason = detailControls[(int)Eindex.Season].Text,
                        BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                        SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                    };
                    DataTable dtu = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                    if (dtu != null)
                    {
                        MesaiHyouJi(dtu);
                        S_BodySeigyo(0, 1);
                        S_BodySeigyo(1, 1);
                        mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                       // S_BodySeigyo(4, 0);
                        scjan_1.Focus();
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
                case EOperationMode.SHOW:
                    if (!ErrorCheck())
                    {
                        return;
                    }
                    Clear(panel2);
                    //ChangeFunKeys();
                    mt = new M_TenzikaiShouhin_Entity
                    {
                        TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                        VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                        LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                        LastSeason = detailControls[(int)Eindex.Season].Text,
                        BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                        SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                    };
                    DataTable dts = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
                    if (dts != null)
                    {
                        MesaiHyouJi(dts);
                        S_BodySeigyo(0, 1);
                        S_BodySeigyo(1, 1);
                        mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                       // S_BodySeigyo(4, 0);
                        scjan_1.Focus();
                    }
                    else
                    {
                        bl.ShowMessage("E128");
                    }
                    break;
            }
        }

        private void DisplayData(M_TenzikaiShouhin_Entity et)
        {
            DataTable dtshow = tbl.Mastertoroku_Tenzikaishouhin_Select(mt);
            if (dtshow != null)
            {
                MesaiHyouJi(dtshow);
                S_BodySeigyo(0, 1);
                S_BodySeigyo(1, 1);
                mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                S_BodySeigyo(4, 0);
                scjan_1.Focus();
            }
            else
            {
                bl.ShowMessage("E128");
            }
        }

        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            if (mGrid.F_Search_Ctrl_MK(ActiveControl, out int w_Col, out int w_CtlRow) == false)
            {
                return;
            }

            int w_Row = w_CtlRow + Vsb_Mei_0.Value;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);
            string date = bl.GetDate();
            switch (kbn)
            {
                case EsearchKbn.Product:
                    using (Search_Product frmProduct = new Search_Product(date))
                    {
                        frmProduct.JANCD = mGrid.g_DArray[w_Row].JANCD;
                        frmProduct.ShowDialog();
                        if (!frmProduct.flgCancel)
                        {
                           mGrid.g_DArray[w_Row].JANCD = frmProduct.JANCD;
                           mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            bool check = CheckKey(-1);
                            if (check)
                            {
                                CheckGrid((int)ClsGridMasterTanzi.ColNO.JANCD, w_Row, false, true);
                                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                                SendKeys.Send("{ENTER}");
                            }

                        }
                    }
                   
                    break;
                
                case EsearchKbn.Brand:
                    using (Search_Brand frmBrand = new Search_Brand())
                    {
                        frmBrand.parBrandCD = mGrid.g_DArray[w_Row].BrandCD;
                        frmBrand.ShowDialog();
                        if (!frmBrand.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].BrandCD = frmBrand.parBrandCD;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            bool check = CheckKey(-1);
                            if (check)
                            {
                                CheckGrid((int)ClsGridMasterTanzi.ColNO.BrandCD, w_Row, false, true);
                                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                                SendKeys.Send("{ENTER}");
                            }
                        }
                    }
                    break;
                case EsearchKbn.Segment:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {
                        frmMulti.parID = "226";
                        frmMulti.parKey = mGrid.g_DArray[w_Row].SegmentCD; 
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].SegmentCD = frmMulti.parKey;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            bool check = CheckKey(-1);
                            if (check)
                            {
                                CheckGrid((int)ClsGridMasterTanzi.ColNO.SegmentCD, w_Row, false, true);
                                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                                SendKeys.Send("{ENTER}");
                            }
                        }
                    }
                    break;

                case EsearchKbn.Tani:
                    using (Search_HanyouKey frmMulti = new Search_HanyouKey())
                    {

                        frmMulti.parID = "202";
                        frmMulti.parKey= mGrid.g_DArray[w_Row].TaniCD;
                        frmMulti.ShowDialog();
                        if (!frmMulti.flgCancel)
                        {
                            mGrid.g_DArray[w_Row].TaniCD = frmMulti.parKey;
                            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            bool check = CheckKey(-1);
                            if (check)
                            {
                                CheckGrid((int)ClsGridMasterTanzi.ColNO.TaniCD, w_Row, false, true);
                                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                                SendKeys.Send("{ENTER}");
                            }
                        }
                    }
                    break;
            }
        }
        private bool CheckGrid(int col, int row, bool chkAll = false, bool changeYmd = false, bool IsExec = false)
        {
            bool checkall = true;
            if (checkall)
            {
                try
                {
                    string ymd = bl.GetDate();
                    if (string.IsNullOrWhiteSpace(ymd))
                        ymd = bbl.GetDate();
                    if (!chkAll && !changeYmd) // check length
                    {
                        int w_CtlRow = row - Vsb_Mei_0.Value;

                        if (w_CtlRow < ClsGridMasterTanzi.gc_P_GYO)
                            if (mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl.GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
                            {
                                if (((CKM_Controls.CKM_TextBox)mGrid.g_MK_Ctrl[col, w_CtlRow].CellCtl).isMaxLengthErr)
                                    return false;
                            }
                    }
                    //if (IsExec ? !mGrid.g_DArray[row].Chk : false || string.IsNullOrEmpty(mGrid.g_DArray[row].JANCD))
                    //{
                    //    return true;
                    //}
                    switch (col)
                    {
                        case (int)ClsGridMasterTanzi.ColNO.JANCD:

                            if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JANCD))
                            {
                                Grid_Gyo_Clr(row);//1014
                                IsExec = false;
                                return true;                // return true;
                            }
                           


                            DataTable dtResult = bbl.SimpleSelect1("66", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), mGrid.g_DArray[row].JANCD);
                            if (dtResult.Rows.Count == 0)
                            {
                                bl.ShowMessage("E101");
                                return false;
                            }
                            M_TenzikaiShouhin_Entity mt = new M_TenzikaiShouhin_Entity
                            {
                                TenzikaiName = detailControls[(int)Eindex.SCTenzikai].Text,
                                VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                                LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                                LastSeason = detailControls[(int)Eindex.Season].Text,
                                BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                                SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
                                JANCD = mGrid.g_DArray[row].JANCD,
                            };
                            DataTable dt = tbl.M_Tenzikaishouhin_SelectForJancd(mt);
                            DataTable dtsku = null;
                            if (dt.Rows.Count > 0)
                            {
                                tbl.ShowMessage("E107");
                                return false;
                            }
                            else
                            {
                                M_SKU_Entity msku = new M_SKU_Entity
                                {
                                    JanCD = mGrid.g_DArray[row].JANCD,
                                    MainVendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                                    InStoreCD = StoreCD,

                                };
                                SKU_BL mbl = new SKU_BL();
                                dtsku = mbl.M_SKU_SelectByJanCD_ForTenzikaishouhin(msku);

                                DataRow selectRow = null;

                                if (dtsku != null)
                                {
                                    if (dtsku.Rows.Count > 0)
                                    {
                                        selectRow = dtsku.Rows[0];
                                    }
                                }
                                if (dtsku.Rows.Count > 0)
                                {
                                   
                                    mGrid.g_DArray[row].SKUCD = selectRow["SKUCD"].ToString();
                                    mGrid.g_DArray[row].SKUName = selectRow["SKUName"].ToString();
                                    mGrid.g_DArray[row].ColorCD = selectRow["ColorNO"].ToString();
                                    mGrid.g_DArray[row].ColorName = selectRow["ColorName"].ToString();
                                    mGrid.g_DArray[row].SizeCD = selectRow["SizeNo"].ToString();
                                    mGrid.g_DArray[row].SizeName = selectRow["SizeName"].ToString();
                                    mGrid.g_DArray[row].Shiiretanka = bbl.Z_SetStr(selectRow["SiireTanka"].ToString());
                                    mGrid.g_DArray[row].JoutaiTanka = bbl.Z_SetStr(selectRow["JoudaiTanka"].ToString());

                                    if (String.IsNullOrEmpty(mGrid.g_DArray[row].SizeName))
                                    {
                                        bbl.ShowMessage("E102");
                                        return false;
                                    }
                                    //mGrid.g_DArray[row].SKUCD = ActiveControl.Text;
                                    //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                                }

                            }
                            Grid_NotFocus(col, row);
                            break;

                        case (int)ClsGridMasterTanzi.ColNO.SKUCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SKUCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.g_DArray[row].SKUCD = ActiveControl.Text;
                              //  mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SKUName:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SKUName))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.g_DArray[row].SKUName = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.ColorCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ColorCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.g_DArray[row].ColorCD = ActiveControl.Text;
                                //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.ColorName:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].ColorName))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].ColorName = ActiveControl.Text;
                                //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SizeCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SizeCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.g_DArray[row].SizeCD = ActiveControl.Text;
                                //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SizeName:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SizeName))
                                {
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].SizeName = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].HanbaiYoteiDateMonth))
                                {
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                if (Convert.ToInt64(mGrid.g_DArray[row].HanbaiYoteiDateMonth) >= 13)
                                {
                                    bbl.ShowMessage("E117");
                                    return false;
                                }
                                //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].HanbaiYoteiBi))
                                {
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].Shiiretanka))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].JoutaiTanka))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].JoutaiTanka = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].SalePriceOutTax = ActiveControl.Text;
                                //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax1))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               //mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax2))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax3))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].SalePriceOutTax3 = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax4))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].SalePriceOutTax4 = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SalePriceOutTax5))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //mGrid.g_DArray[row].SalePriceOutTax5 = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].BrandCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }

                                DataTable dtB = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 11, mGrid.g_DArray[row].BrandCD);
                                if (dtB.Rows.Count == 0)
                                {
                                    bl.ShowMessage("E101");
                                    return false;
                                }
                                // mGrid.g_DArray[row].BrandCD = ActiveControl.Text;
                              //  mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].SegmentCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                ///DataTable dtS = bbl.SimpleSelect1("66", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), mGrid.g_DArray[row].SegmentCD);
                                DataTable dtS = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 13, mGrid.g_DArray[row].SegmentCD,"226");
                                if (dtS.Rows.Count == 0)
                                {
                                    bl.ShowMessage("E101");
                                    return false;
                                }
                                // mGrid.g_DArray[row].SegmentCD = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TaniCD))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                //DataTable dtS = bbl.Select_SearchName(DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), 13, mGrid.g_DArray[row].SegmentCD);
                                //if (dtT.Rows.Count == 0)
                                //{
                                //    bl.ShowMessage("E101");
                                //    return false;
                                //}
                                // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                        case (int)ClsGridMasterTanzi.ColNO.TaxRateFlg:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[row].TaxRateFlg))
                                {
                                    //Ｅ１０２
                                    bbl.ShowMessage("E102");
                                    return false;
                                }
                                // mGrid.g_DArray[row].TaxRateFlg = ActiveControl.Text;
                              //  mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;

                        case (int)ClsGridMasterTanzi.ColNO.Remark:
                            if (mGrid.g_MK_State[col, row].Cell_Enabled)
                            {
                                // mGrid.g_DArray[row].Remark = ActiveControl.Text;
                               // mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                            }
                            break;
                    }
                   // if (chkAll == false)
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                    return true;
                }
                catch (Exception ec) { MessageBox.Show(ec.Message); return true; }
            }
            else
            {
                return false;
            }
        }
        private void Grid_Gyo_Clr(int RW)  // 明細部１行クリア
        {
            string w_Gyo;

            mGrid.S_DispToArray(Vsb_Mei_0.Value);

            w_Gyo = mGrid.g_DArray[RW].GYONO;

            // 一行クリア
            Array.Clear(mGrid.g_DArray, RW, 1);

            mGrid.g_DArray[RW].GYONO = w_Gyo;

            //ZEI_SUB(); // 消費税計算
            //Kin_Kei(); // 再計算

            // JanCD列以外入力不可 (JanCDを入力した時点で他の列が入力可になるため)
              Grid_NotFocus((int)ClsGridMasterTanzi.ColNO.JANCD, RW);

            // 配列の内容を画面にセット
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }
        protected override void EndSec()
        {
            try
            {

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();

        }
        

        private void C_KeyDown(object sender, KeyEventArgs e)
        {
            // Processing when the Enter key is pressed
            // Check if the Return key is pressed
            // Alt or Ctrl key is pressed, do the original operation
            try
            {
                if ((e.KeyCode == Keys.Return) && ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {

                    int index = Array.IndexOf(detailControls, sender);

                    bool ret = CheckDetail(index);
                    if (ret)
                    {
                        if (index == (int)Eindex.KDenwa3 || index == (int)Eindex.HDenwa3 || index == (int)(Eindex.YoteiKinShuu))
                        //明細の先頭項目へ
                        {
                            MoveNextControl(e);
                            //   mGrid.F_MoveFocus((int)ClsGridBase.Gen_MK_FocusMove.MvSet, (int)ClsGridBase.Gen_MK_FocusMove.MvNxt, ActiveControl, -1, -1, ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridJuchuu.ColNO.JanCD);
                        }
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
                        //((Control)sender).Focus();
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeFunKeys()
        {
            if (OperationMode == EOperationMode.UPDATE)
            {
                S_BodySeigyo(1, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                //明細の先頭項目へ
                mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt, this.ActiveControl, -1, -1, this.ActiveControl, Vsb_Mei_0, Vsb_Mei_0.Value, (int)ClsGridMasterTanzi.ColNO.Chk);
                detailControls[(int)Eindex.SCTenzikai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                detailControls[(int)Eindex.SCTenzikai].Focus();
            }
            else if (OperationMode == EOperationMode.INSERT)
            {
                //複写コピー後
                //画面へデータセット後、明細部入力可
                Scr_Lock(2, 3, 0);
                S_BodySeigyo(1, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                detailControls[(int)Eindex.SCTenzikai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                detailControls[(int)Eindex.SCTenzikai].Focus();
                // detailControls[(int)Eindex.SCTenzikai].Enabled = ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = false;

            }
            else
            {
                S_BodySeigyo(2, 0);
                S_BodySeigyo(2, 1);
                //配列の内容を画面にセット
                mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

                previousCtrl.Focus();
            }

        }
        private bool CheckDetail(int index, bool set = true)
        {

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }
            MasterTouroku_TenzikaiShouhin_BL tbl = new MasterTouroku_TenzikaiShouhin_BL();
            Base_BL bl = new Base_BL();
            switch (index)
            {
                case (int)Eindex.SCTenzikai:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }

                   //var rresTen = bbl.SimpleSelect1("74", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), SC_Tenzikai.Text);

                    //var rresTen = SC_Tenzikai.SelectData();
                    //if (!rresTen)
                    //{
                    //    bl.ShowMessage("E101");
                    //    return false;
                    //}
                   
                    break;
                case (int)Eindex.SCShiiresaki:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        ChangeDate = bl.GetDate(),
                        VendorFlg = "1",
                        VendorCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();
                    SC_Vendor.ChangeDate = bl.GetDate();

                    var resul1 = SC_Vendor.SelectData();
                    if (!resul1)
                    {
                        bl.ShowMessage("E101");
                        return false;
                    }
                    //(detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mve.VendorName;
                    break;
                case (int)Eindex.Nendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.Season:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bbl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCBrand:   
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        M_Brand_Entity mbe = new M_Brand_Entity
                        {
                            ChangeDate = bbl.GetDate(),
                            BrandCD = detailControls[index].Text,
                            DeleteFlg = "0"
                        };
                        SC_Brand.ChangeDate = bbl.GetDate();

                        var bres = SC_Brand.SelectData();
                        if (!bres)
                        {
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    // (detailControls[(int)Eindex.SCBrand].Parent as CKM_SearchControl).LabelText = mbe.BrandName;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;
                case (int)Eindex.SCSegment:
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //M_MultiPorpose_Entity mpe = new M_MultiPorpose_Entity()
                        //{
                        //    ChangeDate = bl.GetDate(),
                        //    ID = "226",
                        //    Key = detailControls[index].Text,
                        //    DeleteFlg = "0"
                        //};
                        SC_Segment.ChangeDate = bl.GetDate();
                        SC_Segment.Value1 = "226";
                        var seres = SC_Segment.SelectData();
                        if (!seres)
                        {
                            bbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;
                case (int)Eindex.SCCTenzikai:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        bl.ShowMessage("E102");
                        detailControls[index].Focus();
                        return false;
                    }
                    var resTenC = SC_CopyTenzikai.SelectData();
                    if (!resTenC)
                    {
                        bl.ShowMessage("E101");
                        return false;
                    }
                    break;
                case (int)Eindex.SCCShiiresaki:

                    if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;
                case (int)Eindex.CNendo:
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;
                case (int)Eindex.CSeason:
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            bbl.ShowMessage("E102");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;
                case (int)Eindex.SCCBrand:
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            M_Brand_Entity mbe = new M_Brand_Entity
                            {
                                ChangeDate = bl.GetDate(),
                                BrandCD = detailControls[index].Text,
                                DeleteFlg = "0"
                            };
                            SC_copybrand.ChangeDate = bl.GetDate();

                            var bres = SC_copybrand.SelectData();
                            if (!bres)
                            {
                                bl.ShowMessage("E101");
                                return false;
                            }
                        }

                    }
                    break;
                case (int)Eindex.SCCSegment:
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //M_MultiPorpose_Entity mpe = new M_MultiPorpose_Entity()
                            //{
                            //    ChangeDate = bl.GetDate(),
                            //    ID = "226",
                            //    Key = detailControls[index].Text,
                            //    DeleteFlg = "0"
                            //};
                            SC_copysegmet.ChangeDate = bl.GetDate();
                            SC_copysegmet.Value1 = "226";
                            var seres = SC_copysegmet.SelectData();
                            if (!seres)
                            {
                                bbl.ShowMessage("E101");
                                return false;
                            }
                        }
                    }
                    break;
            }
            return true;
        }
        private bool CheckKey(int index, bool set = true)
        {
            if (index == -1)
            {
                var Con = new Control[] {
                    ((CKM_TextBox)detailControls[(int)Eindex.SCTenzikai]),
                    ((CKM_TextBox)detailControls[(int)Eindex.SCShiiresaki]),
                    (CKM_ComboBox)detailControls[(int)Eindex.Nendo],
                    (CKM_ComboBox)detailControls[(int)Eindex.Season],
                    };
                foreach (var c in Con)
                {
                    if (c is CKM_ComboBox co)
                    {
                        co.Require(true);
                    }
                    else if (c is CKM_TextBox ct)
                    {
                        ct.Require(true);
                    }
                }
                return RequireCheck(Con);

            }
            if (index == 1)
            {

                if (!string.IsNullOrWhiteSpace(detailControls[(int)Eindex.SCCTenzikai].Text))
                {
                    if (!RequireCheck(new Control[] { SC_CopyVendor.TxtCode, CB_Copyyear, CB_copyseason }))  
                        return false;
                }
            }

            return true;
        }
       
        protected void C_Enter(object sender, EventArgs e)
        {
           
            previousCtrl = this.ActiveControl;

            SetFuncKeyAll(this, "111111001001");
            if (ActiveControl.Name == "sc_Tenji")
            {
                previousCtrl = this.ActiveControl;
                if (OperationMode == EOperationMode.SHOW)
                    SetFuncKeyAll(this, "111111001000");

            }
            if (OperationMode == EOperationMode.UPDATE)
            {
                SetFuncKeyAll(this, "111111001011");
            }
            else
            {
                previousCtrl = null;
            }


            //if(OperationMode == EOperationMode.UPDATE)
            //{
            //   // SetFuncKeyAll(this, "111111001011");
            //}
        }

        private void MesaiHyouJi(DataTable dt)
        {
            SetTenjiGrid(dt, true);
        }
        private void SetTenjiGrid(DataTable dt, bool IsShow = false)
        {
            SetMultiColNo(dt);
            S_BodySeigyo(0, 1);
            if (IsShow)
                S_BodySeigyo(1, 1);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
        }
        private void SetMultiColNo(DataTable dt = null)
        {
            if (dt == null)
            {
                for (int w_Row = 0; w_Row < 999; w_Row++)
                {
                    mGrid.g_DArray[w_Row].GYONO = (w_Row + 1).ToString();
                }
            }
            else  // set Data from db
            {
                if(!checkmei)
                {
                      S_Clear_Grid();
                    int c = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        mGrid.g_DArray[c].JANCD = dr["JANCD"].ToString();
                        mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                        mGrid.g_DArray[c].SKUName = dr["商品名"].ToString();
                        mGrid.g_DArray[c].ColorCD = dr["カラーNO"].ToString();
                        mGrid.g_DArray[c].ColorName = dr["カラー名"].ToString();
                        mGrid.g_DArray[c].SizeCD = dr["サイズNO"].ToString();
                        mGrid.g_DArray[c].SizeName = dr["サイズ名"].ToString();
                        mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["販売予定日(月)"].ToString();
                        mGrid.g_DArray[c].HanbaiYoteiBi = dr["販売予定日"].ToString();
                        // mGrid.g_DArray[c].Shiiretanka = dr["仕入単価"].ToString();// bbl.Z_SetStr(selectRow["ShiireTanka"].ToString());
                        mGrid.g_DArray[c].Shiiretanka = bbl.Z_SetStr(dr["仕入単価"].ToString());
                        mGrid.g_DArray[c].JoutaiTanka = bbl.Z_SetStr(dr["上代単価"].ToString());
                        //CheckGrid((int)ClsGridMasterTanzi.ColNO.JANCD, c, true);
                        mGrid.g_DArray[c].SalePriceOutTax = bbl.Z_SetStr(dr["標準売上単価"].ToString());
                        mGrid.g_DArray[c].SalePriceOutTax1 =bbl.Z_SetStr(dr["ランク１単価"].ToString());
                        mGrid.g_DArray[c].SalePriceOutTax2 =bbl.Z_SetStr(dr["ランク２単価"].ToString());
                        mGrid.g_DArray[c].SalePriceOutTax3 =bbl.Z_SetStr(dr["ランク３単価"].ToString());
                        mGrid.g_DArray[c].SalePriceOutTax4 =bbl.Z_SetStr(dr["ランク４単価"].ToString());
                        mGrid.g_DArray[c].SalePriceOutTax5 = bbl.Z_SetStr(dr["ランク５単価"].ToString());
                        mGrid.g_DArray[c].BrandCD = dr["ブランドCD"].ToString();//
                        mGrid.g_DArray[c].SegmentCD = dr["セグメントCD"].ToString();
                        mGrid.g_DArray[c].TaniCD = dr["単位CD"].ToString();//
                        mGrid.g_DArray[c].TaxRateFlg = dr["税率区分"].ToString();//
                        mGrid.g_DArray[c].Remark = dr["備考"].ToString();//
                        mGrid.g_DArray[c].ExhibitionCommonCD = dr["ExhibitionCommonCD"].ToString();
                        
                    c++;
                    }
                }
                else
                {
                    int Res_Gyo = 0;
                    if (OperationMode == EOperationMode.UPDATE || OperationMode == EOperationMode.INSERT)
                      {

                        int w_Gyo = 0;
                        for (int i = mGrid.g_MK_Max_Row -1 ; i >= 0; i--)
                        {
                            if (!string.IsNullOrEmpty(mGrid.g_DArray[i].JANCD))
                            {
                                Res_Gyo = i;
                                break;
                            }
                            w_Gyo = Convert.ToInt16(mGrid.g_DArray[i].GYONO);
                        }
                        }
                        else
                            S_Clear_Grid();
                        int c = 0;

                    if (!string.IsNullOrEmpty(mGrid.g_DArray[0].JANCD))
                    {
                        c += (Res_Gyo + 1);
                    }
                    //if (Res_Gyo != 0)
                    //    {
                    //        c += (Res_Gyo + 1);
                    //    }

                    int w_Row;
                    DataTable dtrest = GetGridData();
                    
                        foreach (DataRow dr in dt.Rows)   // Meisai Dt
                        {
                          
                            if (dr[0] != DBNull.Value)
                            {
                            for (w_Row = 0; w_Row <= dtrest.Rows.Count; w_Row++)
                            {
                                if (mGrid.g_DArray[w_Row].JANCD == dr["JANCD"].ToString())
                                {

                                    bl.ShowMessage("E226");
                                    return;
                                }
                            }
                            mGrid.g_DArray[c].JANCD = dr["JANCD"].ToString();
                            mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                            mGrid.g_DArray[c].SKUName = dr["商品名"].ToString();
                            mGrid.g_DArray[c].ColorCD = dr["カラーNO"].ToString();
                            mGrid.g_DArray[c].ColorName = dr["カラー名"].ToString();
                            mGrid.g_DArray[c].SizeCD = dr["サイズNO"].ToString();
                            mGrid.g_DArray[c].SizeName = dr["サイズ名"].ToString();
                            mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["販売予定日(月)"].ToString();
                            mGrid.g_DArray[c].HanbaiYoteiBi = dr["販売予定日"].ToString();
                            mGrid.g_DArray[c].Shiiretanka = bbl.Z_SetStr(dr["仕入単価"].ToString());
                            mGrid.g_DArray[c].JoutaiTanka = bbl.Z_SetStr(dr["上代単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax = bbl.Z_SetStr(dr["標準売上単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax1 = bbl.Z_SetStr(dr["ランク１単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax2 = bbl.Z_SetStr(dr["ランク２単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax3 = bbl.Z_SetStr(dr["ランク３単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax4 = bbl.Z_SetStr(dr["ランク４単価"].ToString());
                            mGrid.g_DArray[c].SalePriceOutTax5 = bbl.Z_SetStr(dr["ランク５単価"].ToString());
                            mGrid.g_DArray[c].BrandCD = dr["ブランドCD"].ToString();//
                            mGrid.g_DArray[c].SegmentCD = dr["セグメントCD"].ToString();
                            mGrid.g_DArray[c].TaniCD = dr["単位CD"].ToString();//
                            mGrid.g_DArray[c].TaxRateFlg = dr["税率区分"].ToString();//
                            mGrid.g_DArray[c].Remark = dr["備考"].ToString();//
                            c++;
                            //}
                        }
                    }

                    //foreach (DataRow dr in dt.Rows)   // Meisai Dt
                    //    {
                        
                    //        mGrid.g_DArray[c].JANCD = dr["JANCD"].ToString();
                    //        mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                    //        mGrid.g_DArray[c].SKUName = dr["商品名"].ToString();
                    //        mGrid.g_DArray[c].ColorCD = dr["カラーNO"].ToString();
                    //        mGrid.g_DArray[c].ColorName = dr["カラー名"].ToString();
                    //        mGrid.g_DArray[c].SizeCD = dr["サイズNO"].ToString();
                    //        mGrid.g_DArray[c].SizeName = dr["サイズ名"].ToString();
                    //        mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["販売予定日(月)"].ToString();
                    //        mGrid.g_DArray[c].HanbaiYoteiBi = dr["販売予定日"].ToString();
                    //        mGrid.g_DArray[c].Shiiretanka = bbl.Z_SetStr(dr["仕入単価"].ToString());
                    //        mGrid.g_DArray[c].JoutaiTanka = bbl.Z_SetStr(dr["上代単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax = bbl.Z_SetStr(dr["標準売上単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax1 = bbl.Z_SetStr(dr["ランク１単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax2 = bbl.Z_SetStr(dr["ランク２単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax3 = bbl.Z_SetStr(dr["ランク３単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax4 = bbl.Z_SetStr(dr["ランク４単価"].ToString());
                    //        mGrid.g_DArray[c].SalePriceOutTax5 = bbl.Z_SetStr(dr["ランク５単価"].ToString());
                    //        mGrid.g_DArray[c].BrandCD = dr["ブランドCD"].ToString();//
                    //        mGrid.g_DArray[c].SegmentCD = dr["セグメントCD"].ToString();
                    //        mGrid.g_DArray[c].TaniCD = dr["単位CD"].ToString();//
                    //        mGrid.g_DArray[c].TaxRateFlg = dr["税率区分"].ToString();//
                    //        mGrid.g_DArray[c].Remark = dr["備考"].ToString();//
                    //        c++;
                    //    }
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
                            ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = Kbn == 0 ? true : false;
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
                            panel2.Enabled = Kbn == 0 ? true : false;
                            break;
                        }
                }
            }
        }
        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            if (pKBN == 4 && pGrid == 2)
            {
                if (OperationMode == EOperationMode.DELETE || OperationMode == EOperationMode.SHOW)
                    return;
            }
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

                                // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                                //  if (OperationMode != EOperationMode.SHOW && OperationMode != EOperationMode.DELETE)
                                mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, w_Row].Cell_Enabled = true;

                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Shiiretanka, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JoutaiTanka, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.BrandCD, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SegmentCD, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaniCD, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, w_Row].Cell_Enabled = true;
                                //mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Remark, w_Row].Cell_Enabled = true;



                            }

                        }
                        else
                        {
                            //IMT_DMY_0.Focus();

                            if (OperationMode == EOperationMode.INSERT)
                            {
                                Scr_Lock(0, 0, 0);
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                detailControls[(int)Eindex.SCTenzikai].Focus();
                                Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
                                F9Visible = false;
                                SetFuncKeyAll(this, "111111001001");
                            }
                            else if (OperationMode == EOperationMode.UPDATE)
                            {
                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                                Scr_Lock(1, mc_L_END, 1);
                                this.Vsb_Mei_0.TabStop = false;
                                SetFuncKeyAll(this, "111111001011");
                            }
                            else
                            {

                                detailControls[(int)Eindex.SCTenzikai].Enabled = true;
                                ((CKM_SearchControl)detailControls[(int)Eindex.SCTenzikai].Parent).BtnSearch.Enabled = true;
                                Scr_Lock(1, mc_L_END, 1);
                                this.Vsb_Mei_0.TabStop = false;
                                SetFuncKeyAll(this, "111111001010");
                            }
                            SetEnabled(false);
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
                                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JANCD))
                                {
                                    mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                                    continue;
                                }
                                if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].JANCD))
                                {
                                    for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                                    {
                                        switch (w_Col)
                                        {
                                            case (int)ClsGridMasterTanzi.ColNO.Chk:
                                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth:
                                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                                            case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                                            case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:
                                            case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                                            case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                                            case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                                            case (int)ClsGridMasterTanzi.ColNO.TaxRateFlg:
                                            case (int)ClsGridMasterTanzi.ColNO.Remark:
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_ReadOnly = false;
                                                mGrid.g_MK_State[w_Col, w_Row].Cell_Bold = false;
                                                break;

                                            //case (int)ClsGridMasterTanzi.ColNO.JANCD:
                                            //    //case (int)ClsGridTenjikai.ColNO.ShouName:
                                            //    {
                                            //        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
                                            //        break;
                                            //    }

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
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
                            panel2.Enabled = true;                  // ボディ部使用可
                            break;
                        }
                        else
                        {
                            //   Scr_Lock(0, 0, 0);


                            if (OperationMode == EOperationMode.DELETE)
                            {
                                //Scr_Lock(1, 3, 1);
                                SetFuncKeyAll(this, "111111000001");

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

                case 3:
                    Scr_Clr(1);
                    break;
                case 4:
                    if (pGrid == 1)
                    {
                        // 入力可の列の設定
                        for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            if (OperationMode != EOperationMode.UPDATE) mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = false;
                        }

                    }
                    else if (pGrid == 0)
                    {
                        for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
                            if (OperationMode != EOperationMode.UPDATE) mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                        }
                    }
                    else if (pGrid == 2)
                    {

                        for (w_Row = 0; w_Row <= 999; w_Row++)
                        {
                            if (m_EnableCnt - 1 < w_Row)
                                break;

                            if (!mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled && String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD))
                            {
                                mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                            }

                            if (String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD) && String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD))
                            {
                                mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Enabled = true;
                               // FlgChange(w_Row, false);
                            }
                            if (!String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD) && !String.IsNullOrEmpty(mGrid.g_DArray[w_Row].SKUCD))
                            {
                              //  FlgChange(w_Row, true);
                            }


                        }
                        //try
                        //{
                        //    for (int i = mGrid.g_MK_Max_Row - 1; i >= 0; i--)
                        //    {

                        //        if (!mGrid.g_MK_State[(int)ClsGridTenjikai.ColNO.SCJAN, i].Cell_Enabled && !String.IsNullOrEmpty(mGrid.g_DArray[i].SKUCD))
                        //        {
                        //            FlgChange(i, true);
                        //            break;
                        //        }
                        //    }
                        //}
                        //catch (Exception e){
                        //    var x = e.Message;
                        //}
                        mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                        panel2.Refresh();
                    }
                    break;
            }
        }
        private void SetEnabled(bool enabled)
        {
            //detailControls[(int)Eindex.KJuShou1].Enabled = enabled;
            //detailControls[(int)Eindex.KJuShou2].Enabled = enabled;

            //detailControls[(int)Eindex.HJuShou1].Enabled = enabled;
            //detailControls[(int)Eindex.HJuShou2].Enabled = enabled;
            //if (enabled)
            //{

            //}
            //else
            //{
            //    detailControls[(int)Eindex.KJuShou1].Text = "";
            //    detailControls[(int)Eindex.KJuShou2].Text = "";
            //    detailControls[(int)Eindex.HJuShou1].Text = "";
            //    detailControls[(int)Eindex.HJuShou2].Text = "";
            //}
        }
        private void sizecd_5_TextChanged(object sender, EventArgs e)
        {

        }
        private void SC_Tenzikai_Load(object sender, EventArgs e)
        {

        }
        private void CHK_Print_Click(object sender, EventArgs e)
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

                //画面より配列セット 
                mGrid.S_DispToArray(Vsb_Mei_0.Value);

                //if (w_Col == (int)ClsGridMasterTanzi.ColNO.ChoukuSou)
                //{
                //    bool Check = mGrid.g_DArray[w_Row].ChoukuSou;

                //    if (w_Row == 0 && Check)
                //        mGrid.g_DArray[w_Row].ChoukuSou = false;

                //    if (string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SCJAN))
                //        mGrid.g_DArray[w_Row].ChoukuSou = false;
                //}
                //else if (w_Col == (int)ClsGridTenjikai.ColNO.ChkTyokuso)
                //{
                //    //画面明細.直送checkbox＝onの場合
                //    bool Check = mGrid.g_DArray[w_Row].ChkTyokuso;
                //    if (Check)
                //    {
                //        mGrid.g_DArray[w_Row].ArrivePlanDate = "";
                //        mGrid.g_DArray[w_Row].KariHikiateNO = "";
                //    }
                //}

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


            if (pCol == (int)ClsGridMasterTanzi.ColNO.JANCD)
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[pRow].JANCD))
                {
                    for (w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
                    {
                        if (w_Col == (int)ClsGridMasterTanzi.ColNO.JANCD)
                            //JANCD使用可
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                        else
                            mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                    }

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
                            case (int)ClsGridMasterTanzi.ColNO.JANCD:
                                if (mGrid.g_DArray[pRow].GYONO == "0")
                                {
                                    mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                    //mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                }
                                else
                                {
                                    if (OperationMode != EOperationMode.UPDATE && OperationMode != EOperationMode.INSERT) mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = false;
                                    // mGrid.g_MK_State[w_Col, pRow].Cell_Bold = true;
                                }
                                break;
                            case (int)ClsGridMasterTanzi.ColNO.Chk:
                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth:
                            case (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi:
                            case (int)ClsGridMasterTanzi.ColNO.JoutaiTanka:
                            case (int)ClsGridMasterTanzi.ColNO.Shiiretanka:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4:
                            case (int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5:
                            case (int)ClsGridMasterTanzi.ColNO.BrandCD:
                            case (int)ClsGridMasterTanzi.ColNO.SegmentCD:
                            case (int)ClsGridMasterTanzi.ColNO.TaniCD:
                            case (int)ClsGridMasterTanzi.ColNO.TaxRateFlg:
                            case (int)ClsGridMasterTanzi.ColNO.Remark:
                                mGrid.g_MK_State[w_Col, pRow].Cell_Enabled = true;
                                mGrid.g_MK_State[w_Col, pRow].Cell_ReadOnly = false;
                                mGrid.g_MK_State[w_Col, pRow].Cell_Bold = false;
                                break;

                        }

                    }
                    w_AllFlg = false;

                }
            }


        }

        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            //Provider = Microsoft.ACE.OLEDB.12.0; Data Source = c:\myFolder\myExcel2007file.xlsx;
            //Extended Properties = "Excel 12.0 Xml;HDR=YES;IMEX=1";
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=No;IMEX=1';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    //sheet.Cells.NumberFormat = "@";
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Sheet1$]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dtexcel;
        }
        private void BT_meisai_Click(object sender, EventArgs e)
        {
            mGrid.S_DispToArray(Vsb_Mei_0.Value);
            checkmei = true;
            string filePath = string.Empty;
            string fileExt = string.Empty;
            if (!System.IO.Directory.Exists("C:\\SMS\\TenzikaiShouhin\\"))
            {
                System.IO.Directory.CreateDirectory("C:\\SMS\\TenzikaiShouhin\\");
            }
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file 
            file.InitialDirectory= "C:\\SMS\\TenzikaiShouhin\\";
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (!(fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0))
                {
                    bbl.ShowMessage("E137");
                    return ;
                }
                 DataTable   dtExcel = new DataTable();
                    dtExcel = ReadExcel(filePath, fileExt);

                    string[] colname = { "SKUCD", "JANCD", "商品名", "カラーNO", "カラー名", "サイズNO", "サイズ名", "販売予定日(月)", "販売予定日", "仕入単価", "標準売上単価", "ランク１単価", "ランク２単価", "ランク３単価", "ランク４単価", "ランク５単価", "ブランドCD", "セグメントCD", "単位CD", "税率区分", "備考" };
                            if (ColumnCheck(colname, dtExcel))
                            {
                                MesaiHyouJi(dtExcel);
                                S_BodySeigyo(0, 1);
                                mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
                                S_BodySeigyo(4, 0);
                                scjan_1.Focus();
                            }
                            else
                            {
                                tbl.ShowMessage("E137");
                            }
                        
                   // }
               // }
            }

            //Clear(panel2);
            ////ChangeFunKeys();

            //string filePath = string.Empty;
            //string fileExt = string.Empty;
            //OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  

            //   checkmei = true;
            //if (!System.IO.Directory.Exists("C:\\SMS\\TenzikaiShouhin\\"))
            //{
            //    System.IO.Directory.CreateDirectory("C:\\SMS\\TenzikaiShouhin\\");
            //}
            //openFileDialog1.InitialDirectory = "C:\\SMS\\TenzikaiShouhin\\";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            ////openFileDialog1.Filter = "Excel Worksheets|*.xlsx || Excel Worksheets|*.xls";
            //openFileDialog1.FileName = "";

            //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{

            //    var dt = ConvertToDataTable(openFileDialog1.FileName);
            //    if (dt == null)
            //    {
            //        return;
            //    }
            //    string[] colname = { "SKUCD", "JANCD", "商品名", "カラーNO", "カラー名", "サイズNO", "サイズ名", "販売予定日(月)", "販売予定日", "仕入単価", "標準売上単価", "ランク１単価", "ランク２単価", "ランク３単価", "ランク４単価", "ランク５単価", "ブランドCD", "セグメントCD", "単位CD", "税率区分", "備考" };
            //    if (ColumnCheck(colname, dt))
            //    {
            //        MesaiHyouJi(dt);
            //        S_BodySeigyo(0, 1);
            //        mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
            //        S_BodySeigyo(4, 0);
            //        scjan_1.Focus();
            //    }
            //    else
            //    {
            //        tbl.ShowMessage("E137");
            //    }

            //}

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

        private DataTable ConvertToDataTable(string FileName)
        {
            OleDbConnection oledbConn = null;
            DataTable res = null;

            try

            {

                string path = System.IO.Path.GetFullPath(FileName);

                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

                oledbConn.Open();

                OleDbCommand cmd = new OleDbCommand(); ;

                OleDbDataAdapter oleda = new OleDbDataAdapter();

                DataSet ds = new DataSet();

                DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = oledbConn;

                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                oleda = new OleDbDataAdapter(cmd);

                oleda.Fill(ds, "excelData");

                res = ds.Tables["excelData"];
            }

            catch (Exception ex)
            {
                bbl.ShowMessage("E137");
                return null;
            }
            oledbConn.Close();
            //res.Columns[res.Columns.Count - 2].ColumnName = "希望日1";
            //res.Columns[res.Columns.Count - 1].ColumnName = "希望日2";
            foreach (DataColumn c in res.Columns)
            {
                c.ColumnName = c.ColumnName.Trim();
            }

            if (res.Rows.Count == 0)
            {
                MessageBox.Show("No Data Exist");
                return null;
            }

            return res;

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
                w_Row = w_CtlRow + Vsb_Mei_0.Value;

                if (Math.Sign(this.Vsb_Mei_0.Value - mGrid.g_MK_DataValue) == -1)
                    // 上へスクロール
                    w_Dest = (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv;
                else
                    // 下へスクロール
                    w_Dest = (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvNxt;

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
                if (w_Dest == (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv)
                {
                    if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                        // その行の最後から探す
                        mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                }
                else if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, mGrid.g_MK_Ctrl[w_Col, w_CtlRow].CellCtl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, w_Row, w_Col) == false)
                {
                    if (mGrid.g_WheelFLG == true)
                    {
                       
                        if (mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, mGrid.g_MK_Ctrl[mGrid.g_MK_FocusOrder[0], w_CtlRow].CellCtl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]) == false)
                            mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvPrv, this.ActiveControl, w_Row, w_Col, this.ActiveControl, this.Vsb_Mei_0, mGrid.g_MK_Max_Row - 1, mGrid.g_MK_FocusOrder[mGrid.g_MK_Ctl_Col - 1]);
                    }
                    else
                        mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Dest, this.ActiveControl, w_Row, mGrid.g_MK_FocusOrder[0], this.ActiveControl, this.Vsb_Mei_0, w_Row, mGrid.g_MK_FocusOrder[0]);
                }
            }
        }
   
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ckM_TextBox43_TextChanged(object sender, EventArgs e)
        {

        }
        private DataTable ResTem()
        {
            int w_Row;
            var dtrest = new DataTable();
            Col(dtrest);
            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {
                if (!String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD))
                {
                    dtrest.Rows.Add(new object[]{
                        mGrid.g_DArray[w_Row].Chk ? "1" : "0",
                        mGrid.g_DArray[w_Row].JANCD,
                        mGrid.g_DArray[w_Row].SKUCD,
                        mGrid.g_DArray[w_Row].SKUName,
                        mGrid.g_DArray[w_Row].ColorCD,
                        mGrid.g_DArray[w_Row].ColorName,
                        mGrid.g_DArray[w_Row].SizeCD,
                        mGrid.g_DArray[w_Row].SizeName,
                        mGrid.g_DArray[w_Row].HanbaiYoteiDateMonth,
                        mGrid.g_DArray[w_Row].HanbaiYoteiBi,
                        mGrid.g_DArray[w_Row].Shiiretanka,
                        mGrid.g_DArray[w_Row].JoutaiTanka,
                        mGrid.g_DArray[w_Row].SalePriceOutTax,
                        mGrid.g_DArray[w_Row].SalePriceOutTax1,
                        mGrid.g_DArray[w_Row].SalePriceOutTax2,
                        mGrid.g_DArray[w_Row].SalePriceOutTax3,
                        mGrid.g_DArray[w_Row].SalePriceOutTax4,
                        mGrid.g_DArray[w_Row].SalePriceOutTax5,
                        mGrid.g_DArray[w_Row].BrandCD,
                        mGrid.g_DArray[w_Row].SegmentCD,
                        mGrid.g_DArray[w_Row].TaniCD,
                        mGrid.g_DArray[w_Row].TaxRateFlg,
                        mGrid.g_DArray[w_Row].Remark,
                        mGrid.g_DArray[w_Row].ExhibitionCommonCD,
                    });
                }
            }
            return dtrest;
        }
        private void SKUChek()
        {
            int w_Row;
            int w_Col;

            for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            {

                if (!String.IsNullOrEmpty(mGrid.g_DArray[w_Row].JANCD))
                {
                    M_SKU_Entity msku = new M_SKU_Entity
                    {
                        ExhibitionCommonCD = mGrid.g_DArray[w_Row].ExhibitionCommonCD,
                    };
                    DataTable dt = tbl.M_SKU_SelectForSKUCheck(msku);
                    if (dt.Rows.Count == 0)
                    {
                        //if (mGrid.g_DArray[w_Row].JANCD == "0000000000004" || mGrid.g_DArray[w_Row].JANCD == "0000000000005")
                        //{
                        //    mGrid.g_DArray[w_Row].Chk = true;
                        //}
                        //else
                        //{
                            mGrid.g_DArray[w_Row].Chk = false;
                            // SetVal(w_Row);
                       // }
                    }
                    else
                    {
                        mGrid.g_DArray[w_Row].Chk = true;

                    }
                 // S_BodySeigyo
                    mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
                }
                else
                {
                    break;
                }
            }
            var f = ResTem();
            if(f.Rows.Count >0 )
            {
                var dtr = ResTem().Select("chkflg < 2", "chkflg ASC, JanCD ASC, SKUCD ASC").CopyToDataTable();
                SetsSKU(dtr);
            }
        }
        private void SetsSKU(DataTable dt)
        {
            S_Clear_Grid();
            int c = 0;
            foreach (DataRow dr in dt.Rows)
            {
                mGrid.g_DArray[c].JANCD = dr["JanCD"].ToString();
                
                mGrid.g_DArray[c].SKUCD = dr["SKUCD"].ToString();
                mGrid.g_DArray[c].SKUName = dr["SKUName"].ToString();
                mGrid.g_DArray[c].ColorCD = dr["ColorCD"].ToString();
                mGrid.g_DArray[c].ColorName = dr["ColorName"].ToString();
                mGrid.g_DArray[c].SizeCD = dr["SizeCD"].ToString();
                mGrid.g_DArray[c].SizeName = dr["SizeName"].ToString();
                mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["HanbaiDateMonth"].ToString();
                mGrid.g_DArray[c].HanbaiYoteiBi = dr["HanbaiDate"].ToString();
                mGrid.g_DArray[c].Shiiretanka = dr["Shiiretanka"].ToString();//
                mGrid.g_DArray[c].JoutaiTanka = dr["JoudaiTanka"].ToString();//
                //CheckGrid((int)ClsGridMasterTanzi.ColNO.JANCD, c, true);
                mGrid.g_DArray[c].SalePriceOutTax = dr["SalePriceOutTax"].ToString();//
                mGrid.g_DArray[c].SalePriceOutTax1 = dr["Rank1"].ToString();//
                mGrid.g_DArray[c].SalePriceOutTax2 = dr["Rank2"].ToString();//
                mGrid.g_DArray[c].SalePriceOutTax3 = dr["Rank3"].ToString();//
                mGrid.g_DArray[c].SalePriceOutTax4 = dr["Rank4"].ToString();//
                mGrid.g_DArray[c].SalePriceOutTax5 = dr["Rank5"].ToString();//
                mGrid.g_DArray[c].BrandCD = dr["BrandCD"].ToString();//
                mGrid.g_DArray[c].SegmentCD = dr["ExhibitionSegmentCD"].ToString();
                mGrid.g_DArray[c].TaniCD = dr["TaniCD"].ToString();//
                mGrid.g_DArray[c].TaxRateFlg = dr["TaxRateFlg"].ToString();//
                mGrid.g_DArray[c].Remark = dr["Remark"].ToString();//
                mGrid.g_DArray[c].ExhibitionCommonCD = dr["ExhibitioinCommonCD"].ToString();

                mGrid.g_DArray[c].Chk = dr["chkflg"].ToString() == "1" ? true : false;
                if (!mGrid.g_DArray[c].Chk)
                {
                    SetVal(c);
                }
                c++;
            }
           // S_BodySeigyo(1, 0);
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);
            S_BodySeigyo(4, 0);
            scjan_1.Focus();
        }
        private void SetVal(int w_Row)
        {
            
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Chk, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.BrandCD, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SegmentCD, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaniCD, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Remark, w_Row].Cell_Enabled = false;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TB, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Chk, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JANCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SKUCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SKUName, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.ColorCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.ColorName, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SizeCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SizeName, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Shiiretanka, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.JoutaiTanka, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.BrandCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SegmentCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaniCD, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
            mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.Remark, w_Row].Cell_Color = GridBase.ClsGridBase.CheckColor;
        }
        private void BT_SKUCheck_Click(object sender, EventArgs e)
        {
            SKUChek();
        }
        private void SetRequireField()
        {
            SC_Tenzikai.TxtCode.Require(true);
            SC_Vendor.TxtCode.Require(true);
            CB_Year.Require(true);
            CB_Year.Require(true);
           // SC_CopyTenzikai.TxtCode.Require(true);
          //  SC_CopyVendor.TxtCode.Require(true);
          //  CB_Copyyear.Require(true);
           // CB_copyseason.Require(true);
        }
        protected override void ExecSec()
        {
            if (OperationMode == EOperationMode.SHOW)
            {
                return;
            }
            if (OperationMode != EOperationMode.SHOW) // KeysControl
            {
                
                if(!ErrorCheck())
                {
                    return;
                }
            }
            mGrid.S_DispToArray(Vsb_Mei_0.Value);
            for (int RW = 0; RW <= mGrid.g_MK_Max_Row - 1; RW++) // GridControl
            {
                if (string.IsNullOrWhiteSpace(mGrid.g_DArray[RW].JANCD) == false)
                {
                    for (int CL = (int)ClsGridMasterTanzi.ColNO.JANCD; CL < (int)ClsGridMasterTanzi.ColNO.Count; CL++)
                    {
                        if (CheckGrid(CL, RW, true, false, true) == false)
                        {
                            //Focusセット処理
                            ERR_FOCUS_GRID_SUB(CL, RW);
                            return;
                        }
                    }
                }
            }

            DataTable dtrest = GetGridData();
            if(dtrest.Rows.Count > 0)
            {
                mt = GetEntity(dtrest);

                switch (OperationMode)
                {
                    case EOperationMode.INSERT:
                            InsertUpdate(1);

                        break;
                    case EOperationMode.UPDATE:
                        InsertUpdate(2);
                        break;
                    case EOperationMode.DELETE:
                        Delete();
                        break;
                }

            }
            else
            {
                bl.ShowMessage("E128");
            }

            //更新後画面クリア
            ChangeOperationMode(base.OperationMode);
        }

        private void Delete()
        {
            if (tbl.M_Tenzikaishouhin_Delete(mt))
            {
                ChangeOperationMode(OperationMode);
                detailControls[0].Focus();
            }
            else
            {
                bbl.ShowMessage("S001");
            }

        }
        private void ERR_FOCUS_GRID_SUB(int pCol, int pRow)
        {
            Control w_Ctrl;
            bool w_Ret;
            int w_CtlRow;

            w_CtlRow = pRow - Vsb_Mei_0.Value;

            //配列の内容を画面へセット
            mGrid.S_DispFromArray(Vsb_Mei_0.Value, ref Vsb_Mei_0);

            w_Ctrl = Btn_F1;  /// Confirmed

            // IMT_DMY_0.Focus();       // エラー内容をハイライトにするため
            w_Ret = mGrid.F_MoveFocus((int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, (int)ClsGridMasterTanzi.Gen_MK_FocusMove.MvSet, w_Ctrl, -1, -1, this.ActiveControl, Vsb_Mei_0, pRow, pCol);

        }
        public bool ErrorCheck()
        {

            //if (!txtEndDate.DateCheck())
            //    return false;
            if (!RequireCheck(new Control[] { SC_Tenzikai.TxtCode, SC_Vendor.TxtCode, CB_Year,CB_Season })) //Step1
                return false;

            //if (!SC_Tenzikai.IsExists(2))
            //{
            //    bbl.ShowMessage("E101");
            //    SC_Tenzikai.Focus();
            //    return false;

            //}
             var dtResult = bbl.SimpleSelect1("75", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), SC_Vendor.TxtCode.Text);
            if (dtResult.Rows.Count == 0)
            {
                bbl.ShowMessage("E101");
                SC_Vendor.Focus();
                return false;

            }
           if(!String.IsNullOrEmpty(SC_Brand.TxtCode.Text))
            {
                if (!SC_Brand.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Brand.Focus();
                    return false;

                }
            }
           
           if(!String.IsNullOrEmpty(SC_Segment.TxtCode.Text))
            {
                if (!SC_Segment.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_Segment.Focus();
                    return false;

                }
            }
           

            if(!String.IsNullOrEmpty(SC_CopyTenzikai.TxtCode.Text))
            {
                if (!RequireCheck(new Control[] { SC_CopyVendor.TxtCode, SC_CopyVendor.TxtCode,CB_Copyyear,CB_copyseason })) //Step1
                   return false;

                if (!SC_CopyTenzikai.IsExists(2))
                {
                    bbl.ShowMessage("E101");
                    SC_CopyTenzikai.Focus();
                    return false;

                }

                var dtco = bbl.SimpleSelect1("75", DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-"), SC_CopyVendor.TxtCode.Text);
                if (dtco.Rows.Count == 0)
                {
                    bbl.ShowMessage("E101");
                    SC_CopyVendor.Focus();
                    return false;

                }
                if (!String.IsNullOrEmpty(SC_copybrand.TxtCode.Text))
                {
                    if (!SC_copybrand.IsExists(2))
                    {
                        bbl.ShowMessage("E101");
                        SC_copybrand.Focus();
                        return false;

                    }
                }

                if (!String.IsNullOrEmpty(SC_copysegmet.TxtCode.Text))
                {
                    if (!SC_copysegmet.IsExists(2))
                    {
                        bbl.ShowMessage("E101");
                        SC_copysegmet.Focus();
                        return false;

                    }
                }
            }
                if (!TB_StartDate.DateCheck())
                {
                    TB_StartDate.Focus();
                    return false;
                }
                  

            return true;
        }

     
    }
}
