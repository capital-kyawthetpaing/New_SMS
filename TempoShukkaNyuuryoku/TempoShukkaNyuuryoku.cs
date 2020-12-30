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
using Microsoft.VisualBasic;

namespace TempoShukkaNyuuryoku
{
    public partial class TempoShukkaNyuuryoku : ShopBaseForm
    {
        private const string TempoNouhinsyo = "TempoNouhinsyo.exe";
        private const string ZaikoSyokai = "ZaikoSyokai.exe";

        TempoShukkaNyuuryoku_BL tprg_Shukka_Bl = new TempoShukkaNyuuryoku_BL();
        DataTable dtJuchu;
        DataTable dtBottunDetails;
        DataTable dtBottunGroup;
        D_Sales_Entity dse;

        private Base.Client.FrmMainForm.EOperationMode OperationMode;
        private string mStoreCD;
        private string mJuchuNo;
        private string mUriageNo;
        private string mSaleDate;

        public TempoShukkaNyuuryoku()
        {
            InitializeComponent();
        }
        private void TempoShukkaNyuuryoku_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = "TempoRegiShukkaNyuuryoku";
                StartProgram();

                btnClose.Text = "終 了";

                SetRequireField();
                AddHandler();

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    mStoreCD = cmds[(int)ECmdLine.PcID + 1];   //
                    lblCustomerNo.Text = cmds[(int)ECmdLine.PcID + 2];   //会員番号
                    mJuchuNo = cmds[(int)ECmdLine.PcID + 3];  //受注番号

                    if (cmds.Length - 1 >= (int)ECmdLine.PcID + 4)
                        mUriageNo = cmds[(int)ECmdLine.PcID + 4];  //売上番号

                    lblSalesNO.Text = mUriageNo;
                }
                //[M_Customer_Select]
                M_Customer_Entity mce = new M_Customer_Entity
                {
                    CustomerCD = lblCustomerNo.Text,
                    ChangeDate = bbl.GetDate()
                };
                Customer_BL sbl = new Customer_BL();
                bool ret = sbl.M_Customer_Select(mce);
                if (ret)
                {
                    lblCusName.Text = mce.CustomerName;

                    if (mce.BillingType == "1")
                        btnSeikyu.Text = "即請求";
                    else
                        btnSeikyu.Text = "締請求";
                }
                else
                {
                    lblCusName.Text = "";
                }

                //Parameter.売上番号＝Nullの場合 新規モード(Insert Mode)
                if (string.IsNullOrWhiteSpace(mUriageNo))
                {
                    OperationMode = FrmMainForm.EOperationMode.INSERT;
                    lblSalesNO.Text = "";

                    //画面転送表00に従って、画面情報を表示
                    DispFromJuchuNO(mJuchuNo);
                }
                else
                {
                    //削除モード(Delete Mode)
                    OperationMode = FrmMainForm.EOperationMode.DELETE;
                    BtnP_text = "売上取消";

                    //画面転送表03に従って、画面情報を表示
                    DispFromJuchuNO(mJuchuNo, mUriageNo);
                }

                txtJanCD.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();
            }
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
            txtJanCD.Require(true);
        }
        private void AddHandler()
        {
            foreach (Control ctl in tableLayoutPanel2.Controls)
            {
                ctl.Click += new System.EventHandler(this.BtnGroup_Click);
            }
            foreach (Control ctl in tableLayoutPanel3.Controls)
            {
                ctl.Click += new System.EventHandler(this.BtnSyo_Click);
            }
        }
        private void DispFromJuchuNO(string juchuNo, string salesNo = "")
        {
            D_Juchuu_Entity dje = new D_Juchuu_Entity();
            dje.JuchuuNO = mJuchuNo;

            dtJuchu = tprg_Shukka_Bl.D_Juchu_Select(dje, (short)OperationMode, salesNo);

            if (dtJuchu.Rows.Count > 0)
            {
                if (OperationMode == FrmMainForm.EOperationMode.INSERT)
                {
                    txtSalesDate.Text = bbl.GetDate();
                }
                else
                {
                    txtSalesDate.Text = dtJuchu.Rows[0]["SalesDate"].ToString();
                    //txtSalesDate.Enabled = false;
                    //txtFirstCollectPlanDate.Enabled = false;
                    txtShippingSu.Enabled = false;
                    btnOk.Enabled = false;
                }
                //入金日を計算
                Save(3);

                //【Data Area】
                CheckData_M_StoreButtonDetailes();


                M_StoreBottunGroup_Entity msg = new M_StoreBottunGroup_Entity();
                msg.StoreCD = mStoreCD;
                msg.ProgramKBN = "1";

                StoreBottunGroup_BL sgbl = new StoreBottunGroup_BL();
                dtBottunGroup = sgbl.M_StoreButtonGroup_SelectAll(msg);

                DispFromButtonGroupTable();

                //【Data Area Detail】
                DispFromDataTable();

                //【Data Area】
                SetDataFromDataTable();

                Calkkin();
            }
            else
            {
                EndSec();
            }
        }

        private void DispFromDataTable(int gyoNo = 1)
        {

            Clear(pnlDetails);

            for (int i = 0; i < 3; i++)
            {
                int index = gyoNo - 1 + i;

                if (dtJuchu.Rows.Count <= index)
                    break;

                DataRow row = dtJuchu.Rows[index];

                switch (i)
                {
                    case 0:
                        lblDtGyo1.Text = (index + 1).ToString();
                        lblDtSKUName1.Text = row["SKUName"].ToString();
                        lblDtColorSize1.Text = row["ColorSizeName"].ToString();
                        lblDtNotPrintFLG1.Text = row["NotPrintFLG"].ToString().Equals("1") ? "※":"";
                        lblDtKSu1.Text = bbl.Z_SetStr(row["SyukkaKanouSu"]);
                        lblDtSSu1.Text = bbl.Z_SetStr(row["ShippingSu"]);
                        lblDtKin1.Text = "\\" + bbl.Z_SetStr(row["SalesGaku"]);
                        break;

                    case 1:
                        lblDtGyo2.Text = (index + 1).ToString();
                        lblDtSKUName2.Text = row["SKUName"].ToString();
                        lblDtColorSize2.Text = row["ColorSizeName"].ToString();
                        lblDtNotPrintFLG2.Text = row["NotPrintFLG"].ToString().Equals("1") ? "※" : "";
                        lblDtKSu2.Text = bbl.Z_SetStr(row["SyukkaKanouSu"]);
                        lblDtSSu2.Text = bbl.Z_SetStr(row["ShippingSu"]);
                        lblDtKin2.Text = "\\" + bbl.Z_SetStr(row["SalesGaku"]);
                        break;

                    case 2:
                        lblDtGyo3.Text = (index + 1).ToString();
                        lblDtSKUName3.Text = row["SKUName"].ToString();
                        lblDtColorSize3.Text = row["ColorSizeName"].ToString();
                        lblDtNotPrintFLG3.Text = row["NotPrintFLG"].ToString().Equals("1") ? "※" : "";
                        lblDtKSu3.Text = bbl.Z_SetStr(row["SyukkaKanouSu"]);
                        lblDtSSu3.Text = bbl.Z_SetStr(row["ShippingSu"]);
                        lblDtKin3.Text = "\\" + bbl.Z_SetStr(row["SalesGaku"]);
                        break;
                }
            }
        }

        private void Calkkin()
        {
            decimal kin = 0;
            decimal zei = 0;

            foreach (DataRow row in dtJuchu.Rows)
            {
                kin += bbl.Z_Set(row["SalesGaku"]);
                zei += bbl.Z_Set(row["SalesTax"]);
            }

            lblSumSalesGaku.Text = "\\" + bbl.Z_SetStr(kin);
            lblSumSalesTax.Text = "\\" + bbl.Z_SetStr(zei);
        }

        private void SetDataFromDataTable(int gyoNo = 1)
        {
            int index = gyoNo - 1;
            DataRow row = dtJuchu.Rows[index];

            txtJanCD.Text = row["JanCD"].ToString();
            txtJanCD.Tag = index;   //選択行の行番号を退避
            lblSKUName.Text = row["SKUName"].ToString();
            lblColorSize.Text = row["ColorSizeName"].ToString();
            lblJuchuuSuu.Text = bbl.Z_SetStr(row["JuchuuSuu"]);
            //lblSyukkaKanouSu.Text = bbl.Z_SetStr(row["SyukkaKanouSu"]);
            if (bbl.Z_SetStr(row["DirectFLG"]).Equals(1))
                lblSyukkaKanouSu.Text = bbl.Z_SetStr(bbl.Z_Set(row["JuchuuSuu"]) - bbl.Z_Set(row["DeliverySu"]));
            else
                lblSyukkaKanouSu.Text = bbl.Z_SetStr(bbl.Z_Set(row["HikiateSu"]) - bbl.Z_Set(row["DeliverySu"]));
            txtShippingSu.Text = bbl.Z_SetStr(row["ShippingSu"]);

            lblJuchuuUnitPrice.Text = "\\" + bbl.Z_SetStr(row["JuchuuUnitPrice"]);
            //lblJuchuuSuu.Text = "\\" + bbl.Z_SetStr(row["SalesGaku"]);
            lblSalesGaku.Text = "\\" + bbl.Z_SetStr(row["SalesGaku"]);
            lblSalesTax.Text = "\\" + bbl.Z_SetStr(row["SalesTax"]);
            lblJuchuuTaxRitsu.Text = bbl.Z_SetStr(row["JuchuuTaxRitsu"]) + "%";

            lblSyohinChuijiko.Text = row["SyohinChuijiko"].ToString();

            if (OperationMode == FrmMainForm.EOperationMode.DELETE)
            {
                //[M_Customer_Select]
                M_Customer_Entity mce = new M_Customer_Entity
                {
                    CustomerCD = lblCustomerNo.Text,
                    ChangeDate = row["SalesDate"].ToString()
                };
                Customer_BL sbl = new Customer_BL();
                bool ret = sbl.M_Customer_Select(mce);
                if (ret)
                {
                    lblCusName.Text = mce.CustomerName;
                }
                else
                {
                    lblCusName.Text = "";
                }

                if (row["BillingType"].ToString() == "1")
                    btnSeikyu.Text = "即請求";
                else
                    btnSeikyu.Text = "締請求";
            }
        }

        private void CheckData_M_StoreButtonDetailes(string GroupNO = "")
        {
            M_StoreBottunDetails_Entity msb = new M_StoreBottunDetails_Entity();
            msb.StoreCD = mStoreCD;
            msb.ProgramKBN = "1";
            msb.GroupNO = GroupNO;

            StoreButtonDetails_BL sbl = new StoreButtonDetails_BL();
            dtBottunDetails = sbl.M_StoreButtonDetails_SelectAll(msb);

            DispFromButtonDetailsTable();
        }

        private void DispFromButtonDetailsTable(int stHorizontal = 1)
        {
            int maxVertical = stHorizontal + 10;
            //DataRow[] rows = dtBottunDetails.Select(" Vertical >=" + stHorizontal + " AND Vertical <" + stHorizontal + 10);
            DataRow[] rows = dtBottunDetails.Select(" Horizontal >=" + stHorizontal + " AND Horizontal <" + maxVertical);

            //if (rows.Length == 0)
            //    return;

            Clear(tableLayoutPanel3);

            foreach (DataRow row in rows)
            {
                string Horizontal = row["Horizontal"].ToString();
                string Vertical = row["Vertical"].ToString();

                int index = Convert.ToInt16(Horizontal) - stHorizontal + 1;

                //btnSyoをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("btnSyo" + index.ToString() + Vertical, true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["BottunName"].ToString();

                    //MasterKBN=2:顧客 の場合（パネルが得意先CDの場合は）クリックしても何も行わない
                    if (row["MasterKBN"].ToString() == "2")
                        cs[0].Tag = "";
                    else
                        cs[0].Tag = row["JANCD"].ToString();
                }
            }
            btnSyoDown.Tag = stHorizontal;
        }
        private void DispFromButtonGroupTable(int stHorizontal = 1)
        {
            Clear(tableLayoutPanel2);

            DataRow[] rows = dtBottunGroup.Select(" GroupNO >=" + stHorizontal + " AND GroupNO <" + stHorizontal + 14);

            btnGrp1.Tag = stHorizontal;

            foreach (DataRow row in rows)
            {
                int GroupNO = Convert.ToInt16(row["GroupNO"]);
                int index = GroupNO - stHorizontal + 1;

                //btnGrpをさがす。子コントロールも検索する。
                Control[] cs = this.Controls.Find("btnGrp" + index.ToString(), true);
                if (cs.Length > 0)
                {
                    cs[0].Text = row["BottunName"].ToString();
                    cs[0].Tag = row["GroupNO"].ToString(); //
                }
            }

        }

        protected override void EndSec()
        {
            this.Close();
        }
        private bool ErrorCheck(int kbn = 0)
        {
            if (kbn == 1)
            {
                //JANCD
                if (!RequireCheck(new Control[] { txtJanCD }))
                {
                    return false;
                }

                if (!CheckWidth(1))
                    return false;

                if (!CheckWidth(2))
                    return false;

                //【Data Area Detail】に存在するJANCDで無い場合、Error
                DataRow[] rows = dtJuchu.Select("JANCD = '" + txtJanCD.Text + "'");
                if (rows.Length == 0)
                {
                    bbl.ShowMessage("E148");
                    txtJanCD.Focus();
                    return false;
                }
                else if (rows.Length > 1)
                {
                    //【Data Area Detail】に複数存在するJANCDの場合
                    //「該当する行が複数存在します。右の一覧から選択してください」
                    bbl.ShowMessage("E149");
                    txtJanCD.Focus();
                    return false;
                }
                else
                {
                    //【Data Area Detail】に１つだけ存在するJANCDの場合
                    //画面転送表01に従って、画面情報を表示
                    //rows[0]よりDataTableのRowIndexを求める
                    int rowindex = dtJuchu.Rows.IndexOf(rows[0]);
                    txtJanCD.Tag = rowindex;

                    SetDataFromDataTable(rowindex + 1);
                }
            }
            if (kbn == 2)
            {
                //出荷数
                //入力無くても良い(It is not necessary to input)
                //入力無い場合、0とする（When there is no input, it is set to 0）
                txtShippingSu.Text = bbl.Z_SetStr(txtShippingSu.Text.ToString());

                //入力された場合
                //出荷数＞	Form.出荷可能数の場合、Error Ｅ１５０
                if (bbl.Z_Set(txtShippingSu.Text) > bbl.Z_Set(lblSyukkaKanouSu.Text))
                {
                    bbl.ShowMessage("E150");
                    txtShippingSu.Focus();
                    return false;
                }

                //お買上額等の計算を行う
                //お買上額←form.単価×	出荷数
                //lblJuchuuSuu.Text = "\\" + bbl.Z_SetStr(bbl.Z_Set(lblJuchuuUnitPrice.Text.Replace("\\", "")) * bbl.Z_Set(txtShippingSu.Text));
                lblSalesGaku.Text = "\\" + bbl.Z_SetStr(bbl.Z_Set(lblJuchuuUnitPrice.Text.Replace("\\", "")) * bbl.Z_Set(txtShippingSu.Text));

                //うち税額 Function_消費税計算.out金額１
                int taxRateFLG = Convert.ToInt16(dtJuchu.Rows[(int)txtJanCD.Tag]["TaxRateFLG"]);
                string ymd = dtJuchu.Rows[(int)txtJanCD.Tag]["JuchuuDate"].ToString();

                //decimal zeinukiKin = bbl.GetZeinukiKingaku(bbl.Z_Set(lblJuchuuSuu.Text.Replace("\\", "")), taxRateFLG, ymd);
                //lblSalesTax.Text = "\\" + bbl.Z_SetStr(bbl.Z_Set(lblJuchuuSuu.Text.Replace("\\", "")) - zeinukiKin);
                decimal zeinukiKin = bbl.GetZeinukiKingaku(bbl.Z_Set(lblSalesGaku.Text.Replace("\\", "")), taxRateFLG, ymd);
                lblSalesTax.Text = "\\" + bbl.Z_SetStr(bbl.Z_Set(lblSalesGaku.Text.Replace("\\", "")) - zeinukiKin);
            }
            if (kbn == 0 || kbn == 3)
            {
                //お買上日
                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                if (string.IsNullOrWhiteSpace(txtSalesDate.Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    txtSalesDate.Focus();
                    return false;
                }

                txtSalesDate.Text = bbl.FormatDate(txtSalesDate.Text);

                //日付として正しいこと(Be on the correct date)Ｅ１０３
                if (!bbl.CheckDate(txtSalesDate.Text))
                {
                    //Ｅ１０３
                    bbl.ShowMessage("E103");
                    txtSalesDate.Focus();
                    return false;
                }

                //店舗の締日チェック
                //店舗締マスターで判断
                M_StoreClose_Entity mse = new M_StoreClose_Entity();
                mse.StoreCD = mStoreCD;
                mse.FiscalYYYYMM = txtSalesDate.Text.Replace("/", "").Substring(0, 6);
                bool ret = tprg_Shukka_Bl.CheckStoreClose(mse,true,false,false,false,true);
                if (!ret)
                {
                    txtSalesDate.Focus();
                    return false;
                }

                //お買上日から入金予定日を自動計算し表示する（お買上日が変更されたた再計算）
                if (txtSalesDate.Text != mSaleDate)
                {
                    Fnc_PlanDate_Entity fpe = new Fnc_PlanDate_Entity();
                    fpe.KaisyuShiharaiKbn = "0";    // "1";
                    fpe.CustomerCD = lblCustomerNo.Text;
                    fpe.ChangeDate = txtSalesDate.Text;
                    fpe.TyohaKbn = "0";

                    txtFirstCollectPlanDate.Text = bbl.Fnc_PlanDate(fpe);

                    mSaleDate = txtSalesDate.Text;
                }
            }
            if (kbn == 0 || kbn == 4)
            {
                //入金予定日
                //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                if (string.IsNullOrWhiteSpace(txtFirstCollectPlanDate.Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    txtFirstCollectPlanDate.Focus();
                    return false;
                }

                txtFirstCollectPlanDate.Text = bbl.FormatDate(txtFirstCollectPlanDate.Text);

            }

            return true;
        }
        public override void FunctionProcess(int index)
        {
            switch (index + 1)
            {
                case 2:
                    if (OperationMode == FrmMainForm.EOperationMode.DELETE)
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

                    Save();
                    break;
            }
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Sales_Entity GetEntity()
        {
            dse = new D_Sales_Entity();
            dse.JuchuuNO = mJuchuNo;
            dse.SalesNO = mUriageNo;
            dse.StoreCD = mStoreCD;

            dse.SalesDate = txtSalesDate.Text;
            dse.FirstCollectPlanDate = txtFirstCollectPlanDate.Text;
            dse.CustomerCD = lblCustomerNo.Text;

            //dje.HanbaiHontaiGaku = (bbl.Z_Set(lblKin1.Text) - (mZei8 + mZei10)).ToString();
            //dje.HanbaiTax8 = mZei8.ToString();
            if (btnSeikyu.Text == "即請求")
                dse.BillingType = "1";
            else
                dse.BillingType = "2";

            dse.SalesGaku = bbl.Z_SetStr(lblSumSalesGaku.Text.Replace("\\", ""));
            dse.SalesTax = bbl.Z_SetStr(lblSumSalesTax.Text.Replace("\\", ""));

            return dse;
        }
        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("JuchuuRows", typeof(int));
            dt.Columns.Add("ShippingSu", typeof(int));
            dt.Columns.Add("SalesSU", typeof(int));
            dt.Columns.Add("SalesGaku", typeof(decimal));
            dt.Columns.Add("SalesTax", typeof(decimal));
            dt.Columns.Add("ZaikoKBN", typeof(int));
            dt.Columns.Add("UpdateFlg", typeof(int));
        }

        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);

            foreach (DataRow row in dtJuchu.Rows)
            {
                if (bbl.Z_Set(row["ShippingSu"]) > 0)
                    dt.Rows.Add(row["JuchuuRows"]
                        , bbl.Z_Set(row["ShippingSu"])
                        , 0     //bbl.Z_Set(row["SalesSU"]) 未使用
                        , bbl.Z_Set(row["SalesGaku"])
                        , bbl.Z_Set(row["SalesTax"])
                        , bbl.Z_Set(row["ZaikoKBN"])
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
                    //合算表記の明細に対するエラーチェックをおこなう
                    for (int i= 0; i < dtJuchu.Rows.Count; i++)
                    {
                        DataRow row = dtJuchu.Rows[i];
                        if(row["NotPrintFLG"].ToString().Equals("1"))
                        {
                            DataRow[] AddRow = dtJuchu.Select("JuchuuRows = " + row["AddJuchuuRows"].ToString());
                            if(bbl.Z_Set(row["ShippingSu"]) != bbl.Z_Set(AddRow[0]["ShippingSu"]))
                            {
                                //Ｅ２１８				
                                bbl.ShowMessage("E218");

                                int rowindex = dtJuchu.Rows.IndexOf(row);
                                //txtJanCD.Tag = rowindex;

                                //SetDataFromDataTable(rowindex + 1);
                                DispFromDataTable(rowindex + 1);
                                lblDtGyo_Click(lblDtGyo1,new System.EventArgs());
                                
                                return false;
                            }
                        }
                    }

                    DataTable dt = GetGridEntity();

                    if (OperationMode == FrmMainForm.EOperationMode.INSERT)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            //更新対象なし
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }

                    //更新処理
                    dse = GetEntity();
                    bool ret = tprg_Shukka_Bl.PRC_TempoShukkaNyuuryoku(dse, dt, (short)OperationMode, InOperatorCD, InPcID);
                    if (ret)
                    {
                        if (OperationMode == FrmMainForm.EOperationMode.DELETE)
                            bbl.ShowMessage("I102");
                        else
                            bbl.ShowMessage("I101");
                    }

                    if (OperationMode == FrmMainForm.EOperationMode.INSERT)
                    {
                        if (bbl.ShowMessage("Q202") == DialogResult.Yes)
                        {
                            //印刷Program(TempoNouhinsyo)を起動
                            ExecPrint(dse.SalesNO);
                        }
                    }

                    EndSec();
                }

                return true;
            }
            return false;
        }
        private void ExecPrint(string no)
        {
            //Form.印刷CheckBox＝onの場合、印刷Program(TempoNouhinsyo)を起動					
            //EXEが存在しない時ｴﾗｰ
            // 実行モジュールと同一フォルダのファイルを取得
            System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + TempoNouhinsyo;
            if (System.IO.File.Exists(filePath))
            {
                //売上番号,起動区分＝１					
                string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + no + " 1";
                System.Diagnostics.Process.Start(filePath, cmdLine);
            }
            else
            {
                //ファイルなし
            }
        }
        /// <summary>
        /// 明細部在庫ボタンクリック時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Zaiko_Click(object sender, EventArgs e)
        {
            try
            {
                int w_Row = Convert.ToInt16(txtJanCD.Tag);

                string adminNo = dtJuchu.Rows[w_Row]["AdminNO"].ToString();

                //在庫照会を該当商品をパラメータに起動します					
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + ZaikoSyokai;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + adminNo;
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
                //EndSec();
            }
        }

        private bool CheckWidth(int type)
        {
            switch (type)
            {
                case 1:
                    string str = Encoding.GetEncoding(932).GetByteCount(txtJanCD.Text).ToString();
                    if (Convert.ToInt32(str) > 13)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }
                    break;

                case 2:
                    int byteCount = Encoding.UTF8.GetByteCount(txtJanCD.Text);//FullWidth_Case
                    int onebyteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount(txtJanCD.Text);//HalfWidth_Case
                    if (onebyteCount != byteCount)
                    {
                        MessageBox.Show("Bytes Count is Over!!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        return false;
                    }

                    break;
            }
            return true;
        }

        private void txtJanCD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (Save(1))
                    {
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void txtShippingSu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (Save(2))
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                int gyoNo = Convert.ToInt16(lblDtGyo1.Text);
                if (gyoNo - 3 > 0)
                    DispFromDataTable(gyoNo - 3);
                else
                    DispFromDataTable();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                int gyoNo = Convert.ToInt16(lblDtGyo1.Text);
                if (dtJuchu.Rows.Count >= gyoNo + 3)
                    DispFromDataTable(gyoNo + 3);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSyoUp_Click(object sender, EventArgs e)
        {
            try
            {
                int Horizontal = Convert.ToInt16(btnSyoDown.Tag);
                if (Horizontal - 10 > 0)
                    DispFromButtonDetailsTable(Horizontal - 10);
                else
                    DispFromButtonDetailsTable();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSyoDown_Click(object sender, EventArgs e)
        {
            try
            {
                int Horizontal = Convert.ToInt16(btnSyoDown.Tag);
                //if (dtBottunDetails.Rows.Count >= Horizontal + 10)
                DispFromButtonDetailsTable(Horizontal + 10);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGroupUp_Click(object sender, EventArgs e)
        {
            try
            {
                int Horizontal = Convert.ToInt16(btnGrp1.Tag);
                if (Horizontal - 14 > 0)
                    DispFromButtonGroupTable(Horizontal - 14);
                else
                    DispFromButtonGroupTable();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGroupDown_Click(object sender, EventArgs e)
        {
            try
            {
                int Horizontal = Convert.ToInt16(btnGrp1.Tag);
                //if (dtBottunGroup.Rows.Count >= Horizontal + 14)
                    DispFromButtonGroupTable(Horizontal + 14);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSeikyu_Click(object sender, EventArgs e)
        {
            try
            {
                //・「即請求」の場合にClickされると「締請求」に変更
                if (btnSeikyu.Text == "即請求")
                {
                    btnSeikyu.Text = "締請求";
                }
                //・「締請求」の場合にClickされると「即請求」に変更
                else
                {
                    btnSeikyu.Text = "即請求";
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
                //商品CD
                if (!Save(1))
                {
                    return;
                }
                //出荷数
                if (!Save(2))
                {
                    return;
                }

                //お買上額計にAdd
                //うち税額にADD

                //入力された 出荷数,お買上額,うち税額を	【Data Area Detail】に戻す（Set)
                int index = Convert.ToInt16(txtJanCD.Tag);
                DataRow row = dtJuchu.Rows[index];

                row["ShippingSu"] = bbl.Z_Set(txtShippingSu.Text);
                //row["SalesGaku"] = bbl.Z_Set(lblJuchuuSuu.Text.Replace("\\", ""));
                row["SalesGaku"] = bbl.Z_Set(lblSalesGaku.Text.Replace("\\", ""));
                row["SalesTax"] = bbl.Z_Set(lblSalesTax.Text.Replace("\\", ""));

                DispFromDataTable(Convert.ToInt16(lblDtGyo1.Text));

                Calkkin();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSalesDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    bool upd = txtSalesDate.Modified;

                    if (Save(3))
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFirstCollectPlanDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (Save(4))
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGroup_Click(object sender, EventArgs e)
        {
            try
            {
                string GroupNO = ((Button)sender).Tag.ToString();

                if (!string.IsNullOrWhiteSpace(GroupNO))
                {
                    Clear(tableLayoutPanel3);
                    CheckData_M_StoreButtonDetailes(GroupNO);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnSyo_Click(object sender, EventArgs e)
        {
            try
            {
                string JANCD = ((Button)sender).Tag.ToString();

                if (!string.IsNullOrWhiteSpace(JANCD))
                {
                    txtJanCD.Text = JANCD;
                    Save(1);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void lblDtGyo_Click(object sender, EventArgs e)
        {
            try
            {
                Label lblsender = (Label)sender;

                if (string.IsNullOrWhiteSpace(lblsender.Text))
                    return;

                int index = Convert.ToInt16(lblsender.Text) - 1;
                int kanouSu = Convert.ToInt16(dtJuchu.Rows[index]["SyukkaKanouSu"]);

                //【Data Area Detail】出荷可能数≦0の場合、Message表示Ｅ１５１
                if (kanouSu <= 0)
                {
                    bbl.ShowMessage("E151");
                    return;
                }
                //【Data Area Detail出荷可能数＞0の場合、画面転送表02に従って、画面情報を表示
                else
                {
                    SetDataFromDataTable(index + 1);

                    ClearBackColor(pnlDetails);

                    lblsender.BackColor = Color.FromArgb(255, 242, 204);

                    if (lblsender.Name == lblDtGyo1.Name)
                    {
                        lblGyoSelect1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSKUName1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtColorSize1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtNotPrintFLG1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKSu1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKin1.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSSu1.BackColor = Color.FromArgb(255, 242, 204);
                    }
                    else if (lblsender.Name == lblDtGyo2.Name)
                    {
                        lblGyoSelect2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSKUName2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtColorSize2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtNotPrintFLG2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKSu2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKin2.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSSu2.BackColor = Color.FromArgb(255, 242, 204);

                    }
                    else if (lblsender.Name == lblDtGyo3.Name)
                    {
                        lblGyoSelect3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSKUName3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtColorSize3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtNotPrintFLG3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKSu3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtKin3.BackColor = Color.FromArgb(255, 242, 204);
                        lblDtSSu3.BackColor = Color.FromArgb(255, 242, 204);

                    }
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearBackColor(Panel panel)
        {
            IEnumerable<Control> c = GetAllControls(panel);
            foreach (Control ctrl in c)
            {
                if (ctrl is Label)
                    ((Label)ctrl).BackColor = Color.Transparent;
            }
        }
        private void lblGyoSelect1_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo1, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect2_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo2, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void lblGyoSelect3_Click(object sender, EventArgs e)
        {
            try
            {
                lblDtGyo_Click(lblDtGyo3, new EventArgs());
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
    }
}
