using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using BL;
using CKM_Controls;
using Entity;
using GridBase;
using Search;

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
            Vendor
        }

        private enum Eindex : int
        {
            SCTenzikai,
            SCShiiresaki,
            Nendo,
            ShiSon,
            SCBrand,
            SCSegment,
            SCCTenzikai,
            SCCShiiresaki,
            CNendo,
            CShiSon,
            SCCrand,
            SCCSegment,
            pnlKokyakuu,
            KDenwa1,
            KDenwa2,
            KDenwa3,
            SCHaiSoSaki,
            HJuShou1,
            btnHaisou,
            HJuShou2,
            pnlHaisou,
            HDenwa1,
            HDenwa2,
            HDenwa3,
            YoteiKinShuu,
            Count
        }
        private void MasterTouroku_TenzikaiShouhin_Load(object sender, EventArgs e)
        {
            InProgramID = "MasterTouroku_TenzikaiShouhin";
            StartProgram();
            this.InitialControlArray();
            this.S_SetInit_Grid();
            string ymd = bl.GetDate();
            CB_Year.Bind(ymd);
            CB_Season.Bind(ymd);
            CB_copyseason.Bind(ymd);
            CB_Copyyear.Bind(ymd);

        }

        private void BindCombo_Details()
        {
            for (int W_CtlRow = 0; W_CtlRow <= mGrid.g_MK_Ctl_Row - 1; W_CtlRow++)
            {
               //CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ShuukaSou, W_CtlRow].CellCtl;
                //sctl.Cbo_Type = CKM_ComboBox.CboType.出荷倉庫;
                //sctl.Bind(bbl.GetDate());
            }
        }
        private void MasterTouroku_TenzikaiShouhin_KeyUp(object sender, KeyEventArgs e)
        {
            MoveNextControl(e);

        }

      

        private void InitialControlArray()
        {

            detailControls = new Control[] { SC_Tenzikai.TxtCode,SC_Vendor.TxtCode,CB_Year, CB_Season,SC_Brand.TxtCode,SC_Segment.TxtCode,SC_CopyTenzikai.TxtCode,SC_CopyVendor.TxtCode,
                                             CB_Copyyear,CB_copyseason, SC_copybrand.TxtCode,SC_copysegmet.TxtCode,TB_InsertDateTimeF};
            detailLabels = new Control[] { };
            searchButtons = new Control[] { SC_Tenzikai.BtnSearch, SC_Vendor.BtnSearch, SC_Brand.BtnSearch, SC_Segment.BtnSearch, SC_CopyTenzikai.BtnSearch ,
                                            SC_CopyVendor.BtnSearch,SC_copybrand.BtnSearch,SC_copysegmet.BtnSearch };

            

            foreach (var c in detailControls)
            {
                c.KeyDown += C_KeyDown;
               // c.Enter += C_Enter;
                if (c is CKM_ComboBox cb && cb.Name == "cbo_Shuuka")
                {
                   // cb.SelectedIndexChanged += ShuukaSouko_SelectedIndexChanged;
                }
            }
        }
        private void S_SetInit_Grid()
        {
            int W_CtlRow;

            if (ClsGridMasterTanzi.gc_P_GYO <= ClsGridMasterTanzi.gMxGyo)
            {
                mGrid.g_MK_Max_Row = ClsGridMasterTanzi.gMxGyo;               
                m_EnableCnt = ClsGridMasterTanzi.gMxGyo;
            }


            mGrid.g_MK_Ctl_Row = ClsGridMasterTanzi.gc_P_GYO;
            mGrid.g_MK_Ctl_Col = ClsGridMasterTanzi.gc_MaxCL;

            mGrid.g_MK_MaxValue = mGrid.g_MK_Max_Row - mGrid.g_MK_Ctl_Row;

           

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
                        
                    }
                }
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

                //検索ボタンClick時
                if (((Search.CKM_SearchControl)sc).Name.Contains("scjan"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                }
                //else if (((Search.CKM_SearchControl)sc).Name.Substring(0, 9).Equals("IMT_VENCD"))
                //{
                //    //仕入先検索
                //    kbn = EsearchKbn.Vendor;
                //}
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
                        case "scjan":
                            CL = (int)ClsGridMasterTanzi.ColNO.JanCD;
                            break;
                        case "sku":

                            CL = (int)ClsGridMasterTanzi.ColNO.SKUName;
                            break;
                        case "shouhin":

                            CL = (int)ClsGridMasterTanzi.ColNO.ShouhinName;
                            break;
                        case "color":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorCD;
                            break;
                        case "colorname":
                            CL = (int)ClsGridMasterTanzi.ColNO.ColorName;
                            break;
                        case "size":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeCD;
                            break;
                        case "sizename":
                            CL = (int)ClsGridMasterTanzi.ColNO.SizeName;
                            break;
                        case "hanbaidatemonth":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiDateMonth;
                            break;
                        case "hanbaibi":
                            CL = (int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi;
                            break;
                        case "shiiretanka":
                            CL = (int)ClsGridMasterTanzi.ColNO.Shiiretanka;
                            break;
                        //case "IMN_ORGAK":
                        //    CL = (int)ClsGridJuchuu.ColNO.OrderGaku;
                        //    break;
                        case "joutaitanka":
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
                        case "joutaitank":
                            CL = (int)ClsGridMasterTanzi.ColNO.SegmentCD;
                            break;
                        case "joutaitanka1":
                            CL = (int)ClsGridMasterTanzi.ColNO.BrandCD;
                            break;
                        case "tanicd":
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
                  

                    //チェック処理
                   
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int CL, out int w_CtlRow) == false)
                    {
                        return;
                    }

                    //switch (CL)
                    //{
                    //    //case (int)ClsGridJuchuu.ColNO.NotPrintFLG:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkExpress:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkFuyo:
                    //    //case (int)ClsGridJuchuu.ColNO.ChkTyokuso:
                    //        if (e.Shift)
                    //            S_Grid_0_Event_ShiftTab(CL, w_Row, w_ActCtl, w_ActCtl);
                    //        else
                    //            S_Grid_0_Event_Enter(CL, w_Row, w_ActCtl, w_ActCtl);

                    //        break;
                    //}
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void GridControl_Leave(object sender, EventArgs e)
        {
            try
            {
                int w_Row;
                Control w_ActCtl;

                w_ActCtl = (Control)sender;
               // w_Row = System.Convert.ToInt32(w_ActCtl.Tag) + Vsb_Mei_0.Value;
                if (w_ActCtl is GridControl.clsGridCheckBox chk && chk.Name.Contains("chk_"))
                {
                    if (chk.Parent is Panel pnl)
                    {
                        pnl.BackColor = Color.White; ;
                    }
                }
                if (mGrid.F_Search_Ctrl_MK(w_ActCtl, out int w_Col, out int w_CtlRow) == false)
                {
                    return;
                }
                // 背景色
               // w_ActCtl.BackColor = mGrid.F_GetBackColor_MK(w_Col, w_Row);

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
                w_ActCtl.BackColor = ClsGridMasterTanzi.BKColor;

                //明細が出荷済の受注明細、または、売上済の受注明細である場合、削除できない（F7ボタンを使えないようにする）
                //出荷済・売上済の場合は行削除不可
                //if (!string.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].ShuukaSou))
                //{
                //    if (mGrid.g_DArray[w_Row].Syukka.Equals("出荷済") || mGrid.g_DArray[w_Row].Syukka.Equals("売上済"))
                //        Btn_F8.Text = "";
                //    else
                //        Btn_F8.Text = "行削除(F8)";
                //}
                //else
                //    Btn_F8.Text = "行削除(F8)";

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

               // Grid_Gotfocus(w_Col, w_Row, System.Convert.ToInt32(w_ActCtl.Tag));

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
           // Set_GridTabStop(true);

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
                for (int d = 0; d < 5; d++)
                {
                    int id = d + 1;
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.GYONO, d].CellCtl = this.Controls.Find("lbl_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JanCD , d].CellCtl = this.Controls.Find("scjan_" + id, true)[0];
                    //mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.AdminNo, d].CellCtl = new Control(); // Adminno hidden
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SKUName, d].CellCtl = this.Controls.Find("sku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ShouhinName, d].CellCtl = this.Controls.Find("shouhin_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorCD, d].CellCtl = this.Controls.Find("color_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.ColorName, d].CellCtl = this.Controls.Find("colorname_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeCD, d].CellCtl = this.Controls.Find("size_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SizeName, d].CellCtl = this.Controls.Find("sizename_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.HanbaiYoteiBi, d].CellCtl = this.Controls.Find("shuukayotei_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Shiiretanka, d].CellCtl = this.Controls.Find("choukusou_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.JoutaiTanka, d].CellCtl = this.Controls.Find("shuukasouko_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax, d].CellCtl = this.Controls.Find("empty_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax1, d].CellCtl = this.Controls.Find("hacchutanka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax2, d].CellCtl = this.Controls.Find("nyuukayotei_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax3, d].CellCtl = this.Controls.Find("juchuusuu_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax4, d].CellCtl = this.Controls.Find("teni_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SalePriceOutTax5, d].CellCtl = this.Controls.Find("hanbaitanka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.BrandCD, d].CellCtl = this.Controls.Find("zeinuJuchuugaka_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.SegmentCD, d].CellCtl = this.Controls.Find("zeikomijuchuugaku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaniCD, d].CellCtl = this.Controls.Find("ararigaku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.TaxRateFlg, d].CellCtl = this.Controls.Find("zeinu_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remark, d].CellCtl = this.Controls.Find("zeinutanku_" + id, true)[0];
                    mGrid.g_MK_Ctrl[(int)ClsGridMasterTanzi.ColNO.Remark, d].CellCtl = this.Controls.Find("chk_" + id, true)[0];
                  
                    //mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TorokuFlg, d].CellCtl = new Control();  //torokuflg  hidden
                    // mGrid.g_MK_Ctrl[(int)ClsGridTenjikai.ColNO.TaxRateFlg, d].CellCtl = new Control(); //taxrateflg  hidden
                }
            }
            catch (Exception ex)
            {
                var mes = ex.Message + ex.StackTrace;

            }

        }
        private void BT_Display_Click(object sender, EventArgs e)
        {
            
            mt = new M_TenzikaiShouhin_Entity
            {
              // TanKaCD=
                TanKaCD = detailControls[(int)Eindex.SCTenzikai].Text,
                VendorCD = detailControls[(int)Eindex.SCShiiresaki].Text,
                LastYearTerm = detailControls[(int)Eindex.Nendo].Text,
                LastSeason = detailControls[(int)Eindex.ShiSon].Text,
                BranCDFrom = detailControls[(int)Eindex.SCBrand].Text,
                SegmentCDFrom = detailControls[(int)Eindex.SCSegment].Text,
               
            };

            DataTable dtmain = tbl.Mastertoroku_Tenzikaishouhin_Select(mt,"2");
           
            MesaiHyouJi(dtmain);
        }

        private void SearchData(EsearchKbn kbn, Control setCtl)
        {

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
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
            //  throw new NotImplementedException();
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
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                       // ClearCustomerInfo(0);
                        detailControls[index].Focus();
                        return false;
                    }
                    //if (!CheckDependsOnDate(index))
                    //    return false;


                    break;

                //case (int)Eindex.SCTenzikai:    // ShiireSaki
                //    //Entry required
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        //顧客情報ALLクリア
                //       // ClearCustomerInfo(2);
                //        detailControls[index].Focus();
                //        return false;
                //    }
                //    M_TenzikaiShouhin_Entity mte = new M_TenzikaiShouhin_Entity
                //    {
                //        ChangeDate = bbl.GetDate(),
                //      //  VendorFlg = "1",
                //        JANCD = detailControls[index].Text,
                //        DeleteFlg = "0"
                //    };

                //    SC_Tenzikai.ChangeDate = bbl.GetDate();
                //    var tezires = SC_Tenzikai.SelectData();
                //    if (tezires )
                //    {
                //        bbl.ShowMessage("E101");
                //        // ClearCustomerInfo(2);
                //        return false;
                //    }
                //    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mte.TenzikaiName;
                //   //if (!CheckDe*/pendsOnDate(index))
                //       // return false;

                //    break;
                case (int)Eindex.SCShiiresaki:    // ShiireSaki
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                       // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        VendorFlg = "1",
                        VendorCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Vendor_BL vbl = new Vendor_BL();

                    // var  resul1 = vbl.M_Vendor_Select_Tenji(mve);
                    SC_Vendor.ChangeDate = bbl.GetDate();
                    
                    var resul1 = SC_Vendor.SelectData();
                    if (!resul1)
                    {
                        bbl.ShowMessage("E101");
                       // ClearCustomerInfo(2);
                        return false;
                    }
                    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mve.VendorName;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;
                case (int)Eindex.Nendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.ShiSon:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.SCBrand:    // ShiireSaki
                    //Entry required
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_Brand_Entity mbe = new M_Brand_Entity
                    {
                        ChangeDate = bbl.GetDate(),
                        
                        BrandCD = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                   // Base_BL bl = new Base_BL();
                    SC_Brand.ChangeDate = bbl.GetDate();

                    var  bres = SC_Brand.SelectData();
                    if (!bres)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mbe.BrandName;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;
                case (int)Eindex.SCSegment:    // ShiireSaki
                    //Entry required
                    Base_BL blb = new Base_BL();
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        // ClearCustomerInfo(2);
                        detailControls[index].Focus();
                        return false;
                    }
                    M_MultiPorpose_Entity mpe = new M_MultiPorpose_Entity()
                    {
                        ChangeDate = blb.GetDate(),
                        ID= "226",
                        Key = detailControls[index].Text,
                        DeleteFlg = "0"
                    };
                    Base_BL bl1 = new Base_BL();
                    SC_Segment.ChangeDate = blb.GetDate();
                    SC_Segment.Value1 = "226";
                    var seres = SC_Segment.SelectData();
                    if (!seres)
                    {
                        bbl.ShowMessage("E101");
                        // ClearCustomerInfo(2);
                        return false;
                    }
                    (detailControls[(int)Eindex.SCShiiresaki].Parent as CKM_SearchControl).LabelText = mpe.Char1;
                    //if (!CheckDependsOnDate(index))
                    //    return false;

                    break;
                case (int)Eindex.CNendo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
                case (int)Eindex.CShiSon:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        //顧客情報ALLクリア
                        detailControls[index].Focus();
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void MesaiHyouJi(DataTable dt)
        {
            //  
            //  S_BodySeigyo(1,1);
            //  S_BodySeigyo(3, 0);
            SetMultiColNo(dt);
            S_BodySeigyo(1, 1);
            mGrid.S_DispFromArray(this.Vsb_Mei_0.Value, ref this.Vsb_Mei_0);
            // DisablePanel(panel1);
            ///BindCombo_Details();
            DisablePanel(PanelHeader);
            chk_1.Focus();
        }
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
                    mGrid.g_DArray[c].JanCD = dr["JANCD"].ToString();
                    mGrid.g_DArray[c].SKUName = dr["SKUCD"].ToString();
                    mGrid.g_DArray[c].ShouhinName = dr["SKUName"].ToString();
                    mGrid.g_DArray[c].ColorCD = dr["ColorNo"].ToString();
                    mGrid.g_DArray[c].ColorName = dr["ColorName"].ToString();
                    mGrid.g_DArray[c].SizeCD = dr["SizeNO"].ToString();
                    mGrid.g_DArray[c].SizeName = dr["SizeName"].ToString();
                    mGrid.g_DArray[c].HanbaiYoteiDateMonth = dr["HanbaiYoteiDateMonth"].ToString();
                    (mGrid.g_DArray[c].HanbaiYoteiBi) = dr["HanbaiYoteiDate"].ToString();
                    // mGrid.g_DArray[c].Empty = dr["_3SKUName"].ToString();

                    mGrid.g_DArray[c].Shiiretanka = dr["SiireTanka"].ToString();//
                    mGrid.g_DArray[c].JoutaiTanka = dr["JoudaiTanka"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax = dr["SalePriceOutTax"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax1 = dr["TankKa1"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax2 = dr["TankKa2"].ToString();//

                    mGrid.g_DArray[c].SalePriceOutTax3 = dr["TankKa3"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax4 = dr["TankKa4"].ToString();//
                    mGrid.g_DArray[c].SalePriceOutTax5 = dr["TankKa5"].ToString();//
                    mGrid.g_DArray[c].BrandCD = dr["BrandCD"].ToString();//
                    mGrid.g_DArray[c].SegmentCD = dr["SegmentCD"].ToString();//

                    // mGrid.g_DArray[c].Chk = dr["_3SKUName"].ToString();   
                    mGrid.g_DArray[c].TaniCD = dr["TaniCD"].ToString();//
                    mGrid.g_DArray[c].TaxRateFlg = dr["TaxRateFlg"].ToString();//
                    mGrid.g_DArray[c].Remark = dr["Remark"].ToString();//

                  
                    c++;
                }
            }
        }

        private void S_BodySeigyo(short pKBN, short pGrid)
        {
            int w_Row;

            //switch (pKBN)
            //{
            //    case 0:
            //        {
            //            if (pGrid == 1)
            //            {
            //                // 入力可の列の設定
            //                for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            //                {
            //                    if (m_EnableCnt - 1 < w_Row)
            //                        break;

            //                    // 'Jancd列入力可 (Jancdを入力した時点で他の列が入力可になるため)
            //                    mGrid.g_MK_State[(int)ClsGridMasterTanzi.ColNO.SCJAN, w_Row].Cell_Enabled = true;
            //                }

            //            }
            //            else
            //            {
            //                //IMT_DMY_0.Focus();

            //                if (OperationMode == EOperationMode.INSERT)
            //                {
            //                    Scr_Lock(0, 0, 0);
            //                    detailControls[(int)Eindex.SCTenjiKai].Enabled = true;
            //                    detailControls[(int)Eindex.SCTenjiKai].Focus();
            //                    Scr_Lock(1, mc_L_END, 0);  // フレームのロック解除
            //                    detailControls[(int)Eindex.JuuChuuBi].Text = bbl.GetDate();
            //                    F9Visible = false;

            //                    SetFuncKeyAll(this, "111111001001");
            //                }
            //                else
            //                {
            //                    detailControls[(int)Eindex.SCTenjiKai].Enabled = true;
            //                    ((CKM_SearchControl)detailControls[(int)Eindex.SCTenjiKai].Parent).BtnSearch.Enabled = true;
            //                    Scr_Lock(1, mc_L_END, 1);   // フレームのロック
            //                    this.Vsb_Mei_0.TabStop = false;
            //                    SetFuncKeyAll(this, "111111001010");
            //                }

            //                SetEnabled(false);

            //            }
            //            break;
            //        }

            //    case 1:
            //        {
            //            if (pGrid == 1)
            //            {
            //                // 入力可の列の設定
            //                for (w_Row = mGrid.g_MK_State.GetLowerBound(1); w_Row <= mGrid.g_MK_State.GetUpperBound(1); w_Row++)
            //                {
            //                    if (m_EnableCnt - 1 < w_Row)
            //                        break;
            //                    if (!String.IsNullOrWhiteSpace(mGrid.g_DArray[w_Row].SKUCD))
            //                    {
            //                        for (int w_Col = mGrid.g_MK_State.GetLowerBound(0); w_Col <= mGrid.g_MK_State.GetUpperBound(0); w_Col++)
            //                        {
            //                            switch (w_Col)
            //                            {
            //                                case (int)ClsGridTenjikai.ColNO.SCJAN:
            //                                case (int)ClsGridTenjikai.ColNO.ShouName:
            //                                //case (int)ClsGridTenjikai.ColNO.Color:
            //                                //case (int)ClsGridTenjikai.ColNO.ColorName:
            //                                //case (int)ClsGridTenjikai.ColNO.Size:
            //                                //case (int)ClsGridTenjikai.ColNO.SizeName:
            //                                case (int)ClsGridTenjikai.ColNO.ShuukaYo:
            //                                case (int)ClsGridTenjikai.ColNO.ChoukuSou:
            //                                case (int)ClsGridTenjikai.ColNO.ShuukaSou:
            //                                case (int)ClsGridTenjikai.ColNO.HacchuTanka:
            //                                case (int)ClsGridTenjikai.ColNO.NyuuKayo:
            //                                case (int)ClsGridTenjikai.ColNO.JuchuuSuu:
            //                                //case (int)ClsGridTenjikai.ColNO.TenI:
            //                                case (int)ClsGridTenjikai.ColNO.HanbaiTanka:
            //                                //case (int)ClsGridTenjikai.ColNO.ZeinuJuchuu:
            //                                //case (int)ClsGridTenjikai.ColNO.zeikomijuchuu:
            //                                //case (int)ClsGridTenjikai.ColNO.ArariGaku:
            //                                //case (int)ClsGridTenjikai.ColNO.ZeiNu:
            //                                //case (int)ClsGridTenjikai.ColNO.ZeinuTanku:
            //                                case (int)ClsGridTenjikai.ColNO.Chk:
            //                                    //case (int)ClsGridTenjikai.ColNO.ShanaiBi:
            //                                    //case (int)ClsGridTenjikai.ColNO.ShagaiBi:
            //                                    //case (int)ClsGridTenjikai.ColNO.KobeTsu:
            //                                    {
            //                                        mGrid.g_MK_State[w_Col, w_Row].Cell_Enabled = true;
            //                                        break;
            //                                    }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                // txtStartDateFrom.Focus();
            //                SetFuncKeyAll(this, "111111000001");
            //                // btnDisplay.Enabled = false;
            //            }
            //            break;
            //        }

            //    case 2:
            //        {
            //            if (pGrid == 1)
            //            {
            //                panel2.Enabled = true;                  // ボディ部使用可
            //                break;
            //            }
            //            else
            //            {
            //                //  Scr_Lock(0, 0, 0);


            //                if (OperationMode == EOperationMode.DELETE)
            //                {
            //                    //Scr_Lock(1, 3, 1);
            //                    SetFuncKeyAll(this, "111111000011");
            //                    // Scr_Lock(0, 3, 1);
            //                }
            //                else
            //                {
            //                    SetFuncKeyAll(this, "111111000010");
            //                }

            //                // 削除時のみ、明細を参照できるように、スクロールバーのTabStopをTrueにする
            //                this.Vsb_Mei_0.TabStop = true;
            //            }

            //            break;
            //        }

            //    case 3:
            //        Scr_Clr(1);
            //        break;
            //    default:
            //        {
            //            break;
            //        }
            //}

        }
    }
}
