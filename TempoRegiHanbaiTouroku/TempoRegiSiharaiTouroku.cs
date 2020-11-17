using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;
using Search;
using Microsoft.VisualBasic;
using EPSON_TM30;
using DL;

namespace TempoRegiHanbaiTouroku
{
    public partial class TempoRegiSiharaiTouroku : ShopBaseForm
    {
        private const string TempoRegiRyousyuusyo = "TempoRegiRyousyuusyo.exe";
        private const string TempoRegiDataUpdate = "TempoRegiDataUpdate.exe";
        private const string ZaikoSyokai = "ZaikoSyokai.exe";

        private enum meCol : short
        {
            ALL
            , DISCOUNT
                ,MAEUKE
            , POINT
            , OTHER1
                ,cboOTHER1
            , OTHER2
                ,cboOTHER2
            , CARD
                , cboCARD
            , CATSH
            , AZUKARI
            , KAKE
        }
        private TempoRegiHanbaiTouroku_BL tprg_Hanbai_Bl = new TempoRegiHanbaiTouroku_BL();
        //private D_Juchuu_Entity dje;
        private D_Sales_Entity dse;
        private D_StorePayment_Entity dspe;

        private DataTable dtSales;      //明細部データ
        private DataTable dtUpdate;

        private Base.Client.FrmMainForm.EOperationMode OperationMode;

        private Control[] detailControls;

        public string CompanyCD
        {
            set { InCompanyCD = value; }
        }
        public string OperatorCD
        {
            set { InOperatorCD = value; }
        }
        public string PcID
        {
            set { InPcID = value; }
        }
        public DataTable dt
        {
            set { dtSales = value; }
        }
        public bool flgCancel = false;
        public int ParSaleRate { get; set; }
        private string mMaeuke ;
        EPSON_TM30.CashDrawerOpen cdo;
        private  string Up { get; set; }
       private   string Lp { get; set; }
        public TempoRegiSiharaiTouroku(Base.Client.FrmMainForm.EOperationMode mode, D_Sales_Entity dse1, D_StorePayment_Entity dspe1, EPSON_TM30.CashDrawerOpen cdo,string Up,string Lp)
        {
           
           this.Up = Up;
            this.Lp = Lp;
            this.cdo = cdo;
            InitializeComponent();

            //ckmShop_Label7.Visible = false;
            //lblZan.Visible = false;
            ShowCloseMessage = false;

            OperationMode = mode;
            dse = dse1;
            dspe = dspe1;
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = "TempoRegiHanbaiTouroku";
                StartProgram();

                SetRequireField();

                Bind(cboDenominationName1, 1);
                Bind(cboDenominationName2,1);
                Bind(cboCardDenominationCD);

                InitialControlArray();

                //領収書ボタンを押せないようにする
                btnRyosyusyo.Enabled = false;

                //決定ボタンを押せるようにする
                btnProcess.Enabled = true;

                //入金データからその会員から預かっている前受金を計算する
                D_Collect_Entity dce = new D_Collect_Entity();
                dce.StoreCD = StoreCD;
                dce.CollectCustomerCD = dse.CustomerCD;
                dce.AdvanceFLG = "1";

                 mMaeuke= tprg_Hanbai_Bl.GetMaeukeKin(dce);
                lblMaeZan.Text=mMaeuke;

                if (OperationMode != FrmMainForm.EOperationMode.DELETE && OperationMode != FrmMainForm.EOperationMode.UPDATE)
                {
                    lblHenpin.Visible = false;

                    if (OperationMode == FrmMainForm.EOperationMode.SHOW)
                    {
                        lblHenpin.Text = "返品";
                        lblHenpin.Visible = true;
                    }
                 
                    //第二画面（支払画面）表示	画面転送表02	
                    lblJuchuuTaxRitsu.Text = dse.SalesNO;
                    lblSalesGaku.Text = bbl.Z_SetStr(dse.SalesGaku);
                    lblZei.Text = bbl.Z_SetStr(dse.SalesTax);
                    txtDiscount.Text = "0";
                    lblSeikyuGaku.Text = bbl.Z_SetStr(dse.SalesGaku);
                    txtPoint.Text = "0";
                    txtCash.Text = "0";
                    txtAzukari.Text = "0";
                    lblRefund.Text = "0";
                    txtCard.Text = "0";
                    txtKake.Text = "0";
                    txtMaeuke.Text = "0";
                    txtOther1.Text = "0";
                    txtOther2.Text = "0";
                    lblShiharaiKei.Text = "0";
                    //lblZan.Text = bbl.Z_SetStr(dse.LastPoint);

                    txtDiscount.Focus();
                }
                else
                {
                    lblJuchuuTaxRitsu.Text = dse.SalesNO;
                    lblSalesGaku.Text = bbl.Z_SetStr(dse.SalesGaku);
                    lblZei.Text = bbl.Z_SetStr(dse.SalesTax);
                    txtDiscount.Text = bbl.Z_SetStr(dspe.DiscountAmount) ;
                    //lblSeikyuGaku.Text = bbl.Z_SetStr(dse.SalesGaku);
                    //値引き 
                    //ご請求額＝お買上額計－値引		
                    lblSeikyuGaku.Text = bbl.Z_SetStr(bbl.Z_Set(lblSalesGaku.Text) - bbl.Z_Set(txtDiscount.Text));

                    txtMaeuke.Text = bbl.Z_SetStr(dspe.AdvanceAmount);
                    txtPoint.Text = bbl.Z_SetStr(dspe.PointAmount);
                    txtCash.Text = bbl.Z_SetStr(dspe.CashAmount);
                    txtAzukari.Text = bbl.Z_SetStr(dspe.DepositAmount);
                    lblRefund.Text = bbl.Z_SetStr(dspe.RefundAmount);
                    txtCard.Text = bbl.Z_SetStr(dspe.CardAmount);
                    if(!string.IsNullOrWhiteSpace(dspe.CardDenominationCD))
                        cboCardDenominationCD.SelectedValue = dspe.CardDenominationCD;
                    txtOther1.Text = bbl.Z_SetStr(dspe.Denomination1Amount);

                    if (!string.IsNullOrWhiteSpace(dspe.Denomination1CD))
                        cboDenominationName1.SelectedValue = dspe.Denomination1CD;
                    txtOther2.Text = bbl.Z_SetStr(dspe.Denomination2Amount);

                    if (!string.IsNullOrWhiteSpace(dspe.Denomination2CD))
                        cboDenominationName2.SelectedValue = dspe.Denomination2CD;
                    txtKake.Text = bbl.Z_SetStr(dspe.CreditAmount);
                    lblShiharaiKei.Text = "0";
                    //lblZan.Text = bbl.Z_SetStr(dse.LastPoint);

                    Calkkin();

                    if (OperationMode == FrmMainForm.EOperationMode.UPDATE)
                    {
                        lblHenpin.Visible = false;
                        txtDiscount.Focus();
                        return;
                    }

                    btnProcess.Focus();
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
            detailControls = new Control[] {txtDiscount,txtMaeuke, txtPoint,txtOther1,cboDenominationName1, txtOther2,cboDenominationName2,txtCard,cboCardDenominationCD, txtCash,txtAzukari,txtKake };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                //ctl.Enter += new System.EventHandler(DetailControl_Enter);
                ctl.Text = "";
            }
        }

            private void Bind(CKM_Controls.CKMShop_ComboBox combo, int kbn = 0)
        {
            DenominationKBN_BL dbl = new DenominationKBN_BL();
            M_DenominationKBN_Entity me = new M_DenominationKBN_Entity();
            if(kbn.Equals(0))
                me.SystemKBN = "2";

            DataTable dtNyukinhouhou = dbl.BindKbn(me, kbn);
            BindCombo(combo, "DenominationCD", "DenominationName", dtNyukinhouhou);
            ;
        }
        private void BindCombo(CKM_Controls.CKMShop_ComboBox combo, string key, string value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            combo.DataSource = dt;
            combo.DisplayMember = value;
            combo.ValueMember = key;
        }

        /// <summary>
        /// Get All control 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private IEnumerable<Control> GetAllControls(Control root)
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

        private void Clear(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is Label)
                    ((Label)ctrl).Text = string.Empty;

                if (ctrl is Button)
                    ((Button)ctrl).Text = string.Empty;

                ctrl.Tag = "";
            }
        }
        private void SetRequireField()
        {
            txtKake.Require(true);
            txtDiscount.Require(true);
        }
      
        private void Calkkin()
        {            
            //お釣＝預り					－	現金
            lblRefund.Text = bbl.Z_SetStr(bbl.Z_Set(txtAzukari.Text) - bbl.Z_Set(txtCash.Text));

            //お支払計＝ポイント＋その他①＋	その他②＋	カード＋現金 ＋	掛＋前受金から
            lblShiharaiKei.Text = bbl.Z_SetStr(bbl.Z_Set(txtPoint.Text) + bbl.Z_Set(txtOther1.Text) + bbl.Z_Set(txtOther2.Text) + bbl.Z_Set(txtCard.Text)
                                + bbl.Z_Set(txtCash.Text) + bbl.Z_Set(txtKake.Text) + bbl.Z_Set(txtMaeuke.Text));

        }

        protected override void EndSec()
        {
            //if (btnProcess.Enabled)
            if (btnProcess.Text != "次の販売へ")
            {
                flgCancel = true;

            }
            else
            {
                flgCancel = false;
                try
                {
                    cdo.RemoveDisplay(true);
                    cdo.RemoveDisplay();
                }
                catch { }
            }
            this.Close();
        }

        private bool RequireComboCheck(Control[] ctrl)
        {
            foreach (Control c in ctrl)
            {
                 if (c is ComboBox)
                {
                    if (((ComboBox)c).SelectedIndex.Equals(-1))
                    {
                        bbl.ShowMessage("E184");
                        c.Focus();
                        return false;
                    }
                    if (((ComboBox)c).SelectedValue.Equals("-1"))
                    {
                        bbl.ShowMessage("E184");
                        c.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ErrorCheck(int kbn = 0)
        {
            string ymd = bbl.GetDate();

            if (kbn <= (int)meCol.DISCOUNT)
            {
                //値引き 
                //ご請求額＝お買上額計－値引		
                lblSeikyuGaku.Text = bbl.Z_SetStr(bbl.Z_Set(lblSalesGaku.Text) - bbl.Z_Set(txtDiscount.Text));
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.MAEUKE)
            {
                //大小チェック
                if (bbl.Z_Set(mMaeuke) < bbl.Z_Set(txtMaeuke.Text))
                {
                    //Ｅ２３９
                    bbl.ShowMessage("E239");
                    txtMaeuke.Focus();
                    return false;
                }

                //前受け金
                lblMaeZan.Text = bbl.Z_SetStr(bbl.Z_Set(mMaeuke) - bbl.Z_Set(txtMaeuke.Text));
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.POINT)
            {
                //ポイント
                //大小チェック
                if (bbl.Z_Set(dse.LastPoint) < bbl.Z_Set(txtPoint.Text))
                {
                    //Ｅ２４０				
                    bbl.ShowMessage("E240");
                    txtPoint.Focus();
                    return false;
                }

                //入力されたらポイント残＝ポイント残－ポイント
                //lblZan.Text = bbl.Z_SetStr(bbl.Z_Set(dse.LastPoint)-bbl.Z_Set(txtPoint.Text));
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.OTHER1)
            {
                //その他①がnot Null で、入金方法が未選択ならエラー
                if (bbl.Z_Set(txtOther1.Text) != 0)
                {
                    if (!RequireComboCheck(new Control[] { cboDenominationName1 }))
                    {
                        return false;
                    }

                    //その他①の入金方法とその他②が同じであればエラー
                    if (cboDenominationName1.SelectedIndex > 0)
                    {
                        if (cboDenominationName2.SelectedIndex > 0)
                        {
                            if (cboDenominationName1.SelectedValue.ToString() == cboDenominationName2.SelectedValue.ToString())
                            {
                                //Ｅ１８５				
                                bbl.ShowMessage("E185");
                                cboDenominationName2.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.OTHER2)
            {
                //その他②がnot Null で、入金方法が未選択ならエラー
                if (bbl.Z_Set(txtOther2.Text) != 0)
                {
                    if (!RequireComboCheck(new Control[] { cboDenominationName2 }))
                    {
                        return false;
                    }
                    //その他①の入金方法とその他②が同じであればエラー
                    if (cboDenominationName1.SelectedIndex > 0)
                    {
                        if (cboDenominationName2.SelectedIndex > 0)
                        {
                            if (cboDenominationName1.SelectedValue.ToString() == cboDenominationName2.SelectedValue.ToString())
                            {
                                //Ｅ１８５				
                                bbl.ShowMessage("E185");
                                cboDenominationName2.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.CARD)
            {

                //その他②がnot Null で、入金方法が未選択ならエラー
                if (bbl.Z_Set(txtCard.Text) != 0)
                {
                    if (!RequireComboCheck(new Control[] { cboCardDenominationCD }))
                    {
                        return false;
                    }
                }

            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.CATSH)
            {

            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.AZUKARI)
            {
                //大小チェック 
                //現金≠０の場合 現金＞預りならエラー
                if (bbl.Z_Set(txtCash.Text) != 0 && bbl.Z_Set(txtAzukari.Text) < bbl.Z_Set(txtCash.Text))
                {
                    //Ｅ２３７
                    bbl.ShowMessage("E237");
                    txtAzukari.Focus();
                    return false;
                }
                //現金＝０の場合 預り≠０ならエラー
                else if (bbl.Z_Set(txtCash.Text) == 0 && bbl.Z_Set(txtAzukari.Text) != 0)
                {
                    //Ｅ２３８
                    bbl.ShowMessage("E238");
                    txtAzukari.Focus();
                    return false;
                }

                //お釣＝預り					－	現金
                lblRefund.Text = bbl.Z_SetStr(bbl.Z_Set(txtAzukari.Text) - bbl.Z_Set(txtCash.Text));
            }
            if (kbn == (int)meCol.ALL || kbn == (int)meCol.KAKE)
            {
               
            }

            Calkkin();

            return true;
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    if (btnProcess.Text == "次の販売へ")
                        //「次の販売へ」を押すと第一画面へ移ります（入力エリアはクリアします）。

                        this.Close();
                    else
                        Save();
                    break;
            }
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private void GetEntity()
        {
            //dspe  = new  D_StorePayment_Entity();
            dspe.BillingAmount = bbl.Z_SetStr(lblSeikyuGaku.Text);
            dspe.PointAmount = bbl.Z_SetStr(txtPoint.Text);
            dspe.CashAmount = bbl.Z_SetStr(txtCash.Text);
            dspe.DepositAmount = bbl.Z_SetStr(txtAzukari.Text);
            dspe.RefundAmount = bbl.Z_SetStr(lblRefund.Text);

            if (cboCardDenominationCD.SelectedIndex != -1)
                if(!cboCardDenominationCD.SelectedValue.Equals("-1"))
                dspe.CardDenominationCD = cboCardDenominationCD.SelectedValue.ToString();

            dspe.CardAmount = bbl.Z_SetStr(txtCard.Text);
            dspe.DiscountAmount = bbl.Z_SetStr(txtDiscount.Text);
            dspe.CreditAmount = bbl.Z_SetStr(txtKake.Text);

            if (cboDenominationName1.SelectedIndex != -1)
                if (!cboDenominationName1.SelectedValue.Equals("-1"))
                    dspe.Denomination1CD = cboDenominationName1.SelectedValue.ToString();

            dspe.Denomination1Amount = bbl.Z_SetStr(txtOther1.Text);

            if (cboDenominationName2.SelectedIndex != -1)
                if (!cboDenominationName2.SelectedValue.Equals("-1"))
                    dspe.Denomination2CD = cboDenominationName2.SelectedValue.ToString();

            dspe.Denomination2Amount = bbl.Z_SetStr(txtOther2.Text);
            dspe.AdvanceAmount = bbl.Z_SetStr(txtMaeuke.Text);
            dspe.TotalAmount = bbl.Z_SetStr(lblShiharaiKei.Text);
            dspe.SalesRate = bbl.Z_SetStr(ParSaleRate);

        }
        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("JuchuuRows", typeof(int));
            dt.Columns.Add("DisplayRows", typeof(int));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("SKUCD", typeof(string));
            dt.Columns.Add("JanCD", typeof(string));
            //dt.Columns.Add("SKUName", typeof(string));
            //dt.Columns.Add("ColorName", typeof(string));
            //dt.Columns.Add("SizeName", typeof(string));
            dt.Columns.Add("JuchuuSuu", typeof(int));//SalesSU
            dt.Columns.Add("JuchuuUnitPrice", typeof(int));//UnitPrice
            dt.Columns.Add("TaniCD", typeof(string));
            dt.Columns.Add("JuchuuGaku", typeof(decimal));//SalesGaku
            dt.Columns.Add("JuchuuHontaiGaku", typeof(decimal));//SalesHontaiGaku
            dt.Columns.Add("JuchuuTax", typeof(decimal));//SalesTax
            dt.Columns.Add("JuchuuTaxRitsu", typeof(int));
            dt.Columns.Add("ProperGaku", typeof(decimal));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            int rowNo = 1;

            foreach (DataRow row in dtSales.Rows)
            {
                if (bbl.Z_Set(row["SalesRows"]) > 0)
                    dt.Rows.Add(row["SalesRows"]
                        , row["SalesRows"]
                        , bbl.Z_Set(row["AdminNO"])
                        , row["SKUCD"]
                        , row["JanCD"]
                        //, row["SKUName"]
                        //, row["ColorName"]
                        //, row["SizeName"]
                        , bbl.Z_Set(row["SalesSU"])
                        , bbl.Z_Set(row["SalesUnitPrice"])
                        , row["TaniCD"]
                        , bbl.Z_Set(row["SalesGaku"])
                        , bbl.Z_Set(row["SalesHontaiGaku"])
                        , bbl.Z_Set(row["SalesTax"])
                        , bbl.Z_Set(row["SalesTaxRitsu"])
                        , bbl.Z_Set(row["ProperGaku"])
                        //     , bbl.Z_Set(row["ZaikoKBN"])
                        , 0
                        );

            }

            return dt;
        }
        private bool Save(int kbn = 0)
        {
            if (ErrorCheck(kbn))
            {
                if (kbn == 0)
                {
                    //ご請求額＝お支払計でない場合、エラーカーソルは現金欄へ					
                    if (bbl.Z_Set(lblSeikyuGaku.Text) != bbl.Z_Set(lblShiharaiKei.Text))
                    {
                        //Ｅ１８３				
                        bbl.ShowMessage("E183");
                        txtCash.Focus();
                        return false;
                    }

                    try
                    {
                        //更新処理（通常販売）
                        GetEntity();
                        dtUpdate = GetGridEntity();
                        bool ret = tprg_Hanbai_Bl.PRC_TempoRegiHanbaiTouroku(dse, dspe, dtUpdate, (short)OperationMode);

                        lblJuchuuTaxRitsu.Text = dse.SalesNO;

                        string reissue = OperationMode == FrmMainForm.EOperationMode.INSERT ? "0" : "1";
                   
                        //レシート印字
                        //TempoRegiRyousyuusyo‗店舗領収書印刷
                        ExecPrint(dse.SalesNO, reissue);

                        //cdo.SetDisplay("");
                        //データ更新（レシート印刷やお釣りのやり取りする間にややこしい更新を行う）
                        //TempoRegiDataUpdate‗店舗レジデータ更新
                        ExecUpdate(dse.SalesNO);
                  
                    }
                    catch (Exception ex){
                        MessageBox.Show(ex.Message);
                    }
                  
               

                    //領収書ボタンを押せるようにする
                    btnRyosyusyo.Enabled = true;
                    btnRyosyusyo.Tag = dse.SalesNO;

                    //決定ボタンを押せないようにする
                    //btnProcess.Enabled = false;
                    btnProcess.Text = "次の販売へ";
                    btnClose.Enabled = false;

                }

                return true;
            }
            return false;
        }
        string a = "";
        string b = ""; 
        private bool ExecPrint(string no, string reissue)
        {
            //Parameter受取  OperatorCD←	Parameter受取 OperatorCD 
            //  ProcessingPC ←	ProcessingPC
            //  StoreCD	←StoreCD
            //  SalesNO	←売上 売上番号
            //  領収書FLG←	Form.領収書必要ONの場合、1。以外は0．															
            //	レシートFLG ←1
            //  領収書印字日付←												
            //↑	Table転送仕様Ａ で採番							
            //↑お客さんを待たせたくないので、																																						
            //　レシート発行を先にする		

            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoRegiRyousyuusyo;
            if (System.IO.File.Exists(filePath))
            {
                string receipte = "0";
                //領収書必要ONの場合、1。以外は0．													

                //2020/07/20 Y.Nishikawa CHG 引数を整備↓↓
                //string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + dse.StoreCD + " " + no + " " + receipte + " 1 " + bbl.GetDate() + " " + reissue;
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + no + " " + receipte + " 1 " + bbl.GetDate() + " " + reissue;
                //2020/07/20 Y.Nishikawa CHG 引数を整備↑↑
                a = filePath;
                b = cmdLine;
                try
                {

                    ///movedBegin
                    try
                    {
                        Parallel.Invoke(() => CDO_Open(), () => Printer_Open(filePath,  "", cmdLine));
                    }
                    catch (Exception ex) {

                        MessageBox.Show("Parallel function worked and cant dispose instance. . . " + ex.Message + Environment.NewLine+ ex.StackTrace.ToString());

                    }
                    cdo.SetDisplay(false, false, "", Up, Lp);
                }
                catch
                {

                }

                //try
                //{
                //    try
                //    {
                //        cdo.RemoveDisplay(true);
                //        cdo.RemoveDisplay(true);
                //    }
                //    catch
                //    {
                //        cdo.RemoveDisplay(true);
                //    }
                //    if (Base_DL.iniEntity.IsDM_D30Used)
                //    {
                //        EPSON_TM30.CashDrawerOpen op = new EPSON_TM30.CashDrawerOpen();  //2020_06_24 
                //        op.OpenCashDrawer(); //2020_06_24     << PTK
                //    }
                //    try
                //    {
                //        var pro = System.Diagnostics.Process.Start(filePath, cmdLine);
                //        pro.WaitForExit();
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(ex.Message + Environment.NewLine + cmdLine);

                //    }
                //    try
                //    {
                //        cdo.SetDisplay(true, true, "", "");
                //        cdo.RemoveDisplay(true);
                //        cdo.RemoveDisplay(true);
                //    }
                //    catch { }

                //    cdo.SetDisplay(false, false, "", Up, Lp);
                //}
                //catch
                //{
                //}
            }
            else
            {
                //EXEが存在しない時ｴﾗｰ
                return false;
            }
            return true;
        }

        protected void CDO_Open()
        {
            try
            {
                // cdo.RemoveDisplay(true);
                // cdo.RemoveDisplay(true);
            }
            catch
            {
            }
            if (Base_DL.iniEntity.IsDM_D30Used)
            {
                EPSON_TM30.CashDrawerOpen op = new EPSON_TM30.CashDrawerOpen(); //2020_06_24
                op.OpenCashDrawer(); //2020_06_24 << PTK
            }
            try
            {
                cdo.RemoveDisplay(true);
                cdo.RemoveDisplay(true);
            }
            catch { MessageBox.Show("CO. . . "); }
        }
        protected void Printer_Open(string filePath, string programID, string cmdLine)
        {
            if (programID == "")
            {
                var pro = System.Diagnostics.Process.Start(filePath, cmdLine);
                pro.WaitForExit();
            }
            else
            {
                var pro = System.Diagnostics.Process.Start(filePath + @"\" + programID + ".exe", cmdLine + "");
                pro.WaitForExit();
            }
            try
            {
                cdo.SetDisplay(true, true, "");
                cdo.RemoveDisplay(true);
                cdo.RemoveDisplay(true);
               // cdo.SetDisplay(false, false, "", Up, Lp);
            }
            catch
            {
                MessageBox.Show("P0. . .");
            }


        }
        private bool ExecUpdate(string no)
        {
            bool ret = tprg_Hanbai_Bl.PRC_TempoRegiDataUpdate(dse, (short)OperationMode);

            //// 実行モジュールと同一フォルダのファイルを取得
            //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoRegiDataUpdate;
            //if (System.IO.File.Exists(filePath))
            //{										            
            //    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + dse.StoreCD
            //                        + " " + no + " " + (int)OperationMode;
            //    System.Diagnostics.Process.Start(filePath, cmdLine);
            //}
            //else
            //{
            //    //EXEが存在しない時ｴﾗｰ
            //    return false;
            //}
            return true;
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
                    bool ret = Save(index+1);
                    if (ret)
                    {
                        if (detailControls.Length - 1 > index)
                        {
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
                        }
                        else
                        {
                            btnProcess.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //レシート印字
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoRegiRyousyuusyo;
                if (System.IO.File.Exists(filePath))
                {
                    string receipte = "1";
                    //領収書必要ONの場合、1。以外は0．													

                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + dse.StoreCD 
                        + " " + btnRyosyusyo.Tag.ToString() + " " + receipte + " 0 " + bbl.GetDate();
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMaeuke_Click(object sender, EventArgs e)
        {
            try
            {
                //ご請求額－その他①－	その他②－	カード	－	現金－	掛－ポイントを上限に、前受金残額を自動セット							
                decimal wMaeuke = bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCard.Text)
                              - bbl.Z_Set(txtCash.Text) - bbl.Z_Set(txtKake.Text) - bbl.Z_Set(txtPoint.Text);
                if (wMaeuke > 0)               
                {
                    //前受金を自動セットするときに前受金残額を上限にする
                    if (wMaeuke > bbl.Z_Set(mMaeuke))
                    {
                        wMaeuke = bbl.Z_Set(mMaeuke);
                    }
                    txtMaeuke.Text = bbl.Z_SetStr(wMaeuke);
                }

                lblMaeZan.Text = bbl.Z_SetStr(bbl.Z_Set(mMaeuke) - bbl.Z_Set(txtMaeuke.Text));

                //お支払計＝ポイント＋その他①＋その他②＋カード＋現金＋掛
                Calkkin();
        }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void btnPoint_Click(object sender, EventArgs e)
        {
            try
            {
                //ご請求額－その他①－その他②－カード－現金－掛を上限に、ポイント残額を自動セット
                decimal wPoint = bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCard.Text)
                                - bbl.Z_Set(txtCash.Text) - bbl.Z_Set(txtKake.Text) - bbl.Z_Set(txtMaeuke.Text);
                if (wPoint > 0)
                {
                    //ポイントを自動セットするときにポイント残額を上限にする
                    if (wPoint > bbl.Z_Set(dse.LastPoint))
                    {
                        wPoint = bbl.Z_Set(dse.LastPoint);
                    }
                    txtPoint.Text = bbl.Z_SetStr(wPoint);
                }

                //ポイント残 ＝	ポイント残－ポイント
                //lblZan.Text = bbl.Z_SetStr(bbl.Z_Set(dse.LastPoint) - bbl.Z_Set(txtPoint.Text));

                //お支払計＝ポイント＋その他①＋その他②＋カード＋現金＋掛
                Calkkin();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOther1_Click(object sender, EventArgs e)
        {
            try
            {
                //その他①＝ご請求額－その他②－カード－現金－	掛		－前受金から
                txtOther1.Text = bbl.Z_SetStr(bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCard.Text) - bbl.Z_Set(txtCash.Text) - bbl.Z_Set(txtKake.Text)
                                                - bbl.Z_Set(txtPoint.Text) - bbl.Z_Set(txtMaeuke.Text));

                Calkkin();		

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOther2_Click(object sender, EventArgs e)
        {
            try
            {
                //その他➁＝ご請求額－その他①－カード－現金－	掛－前受金から
                txtOther2.Text = bbl.Z_SetStr(bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtCard.Text) - bbl.Z_Set(txtCash.Text) - bbl.Z_Set(txtKake.Text)
                                     - bbl.Z_Set(txtPoint.Text) - bbl.Z_Set(txtMaeuke.Text));

                Calkkin();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCardClick(object sender, EventArgs e)
        {
            try
            {
                //カード＝	ご請求額－	その他①－	現金－	その他②－	掛	－前受金から	
                txtCard.Text = bbl.Z_SetStr(bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCash.Text) - bbl.Z_Set(txtKake.Text)
                                 - bbl.Z_Set(txtPoint.Text) - bbl.Z_Set(txtMaeuke.Text));

                Calkkin();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCatsh_Click(object sender, EventArgs e)
        {
            try
            {
                //現金＝	ご請求額－	その他①－	カード－	その他②－	掛－前受金から
                txtCash.Text = bbl.Z_SetStr(bbl.Z_Set(lblSeikyuGaku.Text)-bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCard.Text) - bbl.Z_Set(txtKake.Text)
                                         - bbl.Z_Set(txtPoint.Text) - bbl.Z_Set(txtMaeuke.Text));

                Calkkin();

                //カーソルは預りへ
                txtAzukari.Focus();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

        private void btnAzukari_Click(object sender, EventArgs e)
        {
            try
            {
                //預り	＝	現金
                txtAzukari.Text = bbl.Z_SetStr(txtCash.Text);
                //お釣              ＝	0
                lblRefund.Text = "0";
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKake_Click(object sender, EventArgs e)
        {
            try
            {
                //掛＝	ご請求額－	その他①－	その他②－	カード－現金		－前受金から
                txtKake.Text = bbl.Z_SetStr(bbl.Z_Set(lblSeikyuGaku.Text) - bbl.Z_Set(txtOther1.Text) - bbl.Z_Set(txtOther2.Text) - bbl.Z_Set(txtCard.Text) - bbl.Z_Set(txtCash.Text)
                                         - bbl.Z_Set(txtPoint.Text) - bbl.Z_Set(txtMaeuke.Text));

                Calkkin();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //ポイント残					を元に戻す		
                //lblZan.Text = bbl.Z_SetStr(dse.LastPoint);

                //ポイント、その他①②、現金、預り、カード、掛、お支払計を０に。
                txtDiscount.Text = "0";
                txtPoint.Text = "0";
                txtOther1.Text = "0";
                txtOther2.Text = "0";
                txtCash.Text = "0";
                txtCard.Text = "0";
                txtKake.Text = "0";
                txtMaeuke.Text = "0";
                lblShiharaiKei.Text = "0";
                txtAzukari.Text = "0";
                lblRefund.Text = "0";

                //カード、その他①②の選択をクリア
                cboDenominationName2.SelectedIndex = -1;
                cboCardDenominationCD.SelectedIndex = -1;
                cboDenominationName1.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }

        }

    }
}
