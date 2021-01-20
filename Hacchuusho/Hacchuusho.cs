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
    
namespace Hacchuusho
{
    /// <summary>
    /// Hacchuusho 発注書印刷
    /// </summary>
    internal partial class Hacchuusho : FrmMainForm
    {
        #region const

        private const string ProID = "Hacchuusho";
        private const string ProNm = "発注書印刷";

        #endregion

        #region enum

        private enum EIndex : int
        {
            StoreCD,
            InsatuTaishou_Mihakkou,
            InsatuTaishou_Saihakkou,
            HacchuuDateFrom,
            HacchuuDateTo,
            Vendor,
            Staff,
            HacchuuNO,
            IsPrintMisshounin,
            IsPrintEDIHacchuu,
            InsatuShurui_Hacchhusho,
            InsatuShurui_NetHacchuu,
            InsatuShurui_Chokusou,
            InsatuShurui_Cancel,
            Print
        }
        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Vendor,
            Staff,
            HacchuuNO,
        }

        #endregion

        #region 変数

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private Hacchuusho_BL hsbl;
        //private D_Billing_Entity dse;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldVendorCD = "";

        private string StartUpKBN = "";

        #endregion

        #region コンストラクタ

        public Hacchuusho()
        {
            InitializeComponent();
        }

        #endregion

        #region イベント - Load

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.PRINT);
                this.InitialControlArray();
                base.Btn_F10.Text = "";

                //起動時共通処理
                base.StartProgram();

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScHacchuuNO.Value1 = InOperatorCD;
                ScHacchuuNO.Value2 = stores;

                hsbl = new Hacchuusho_BL();
                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd,"2");

                SetFuncKeyAll(this, "100001000011");
                Scr_Clr(0);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }

        #endregion

        #region イベント - コントロール

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
                            //if (ckM_RadioButton1.Checked)
                            //    ckM_RadioButton1.Focus();
                            //else
                            //    ckM_RadioButton3.Focus();
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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);
                switch (index)
                {
                    case (int)EIndex.Vendor:
                    case (int)EIndex.Staff:
                    case (int)EIndex.HacchuuNO:
                        F9Visible = true;
                        break;

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


                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

        #region イベント - ファンクションボタン

        /// <summary>
        /// 
        /// </summary>
        protected override void PrintSec()
        {
            // レコード定義を行う
            DataTable table = CheckData(out DataTable dtForUpdate);

            try
            {
                if (table == null)
                {
                    return;
                }
                //xsdファイルを保存します。

                //DB　---→　xsd　----→　クリスタルレポート

                //というデータの流れになります
                //table.TableName = ProID;
                //table.WriteXmlSchema("DataTable" + ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                Hacchuusho_Report Report = new Hacchuusho_Report();

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:
                        if (StartUpKBN == "1")
                        {
                            ret = DialogResult.No;
                        }
                        else
                        {
                            //Q202 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                            ret = bbl.ShowMessage("Q202");
                            if (ret == DialogResult.Cancel)
                            {
                                return;
                            }
                        }

                        // 印字データをセット
                        Report.SetDataSource(table);
                        Report.Refresh();

                        if (ret == DialogResult.Yes)
                        {
                            //プレビュー
                            var previewForm = new Viewer();
                            previewForm.CrystalReportViewer1.ShowPrintButton = true;
                            previewForm.CrystalReportViewer1.ReportSource = Report;
                            UpdateOrderD_04();
                            previewForm.ShowDialog();
                        }
                        else
                        {
                            int marginLeft = 360;
                            CrystalDecisions.Shared.PageMargins margin = Report.PrintOptions.PageMargins;
                            margin.leftMargin = marginLeft; // mmの指定をtwip単位に変換する
                            margin.topMargin = marginLeft;
                            margin.bottomMargin = marginLeft;//mmToTwip(marginLeft);
                            margin.rightMargin = marginLeft;

                            Report.PrintOptions.ApplyPageMargins(margin);
                            // プリンタに印刷
                            UpdateOrderD_04();
                            Report.PrintToPrinter(0, false, 0, 0);
                        }
                        break;

                    case EPrintMode.PDF:
                        if (bbl.ShowMessage("Q204") != DialogResult.Yes)
                        {
                            return;
                        }
                        string filePath = "";
                      
                        if (!ShowSaveFileDialog(InProgramNM, out filePath))
                        {
                            return;
                        }

                        // 印字データをセット
                        Report.SetDataSource(table);
                        Report.Refresh();
                        UpdateOrderD_04();
                        bool result = OutputPDF(filePath, Report);

                        //PDF出力が完了しました。
                        bbl.ShowMessage("I202");

                        break;
                }

                //ログ出力
                //L_Log_Entity le = new L_Log_Entity();
                //le.InsertOperator = this.InOperatorCD;
                //le.Program = this.InProgramID;
                //le.PC = this.InPcID;
                //le.OperateMode = null;
                //le.KeyItem = this.ScHacchuuNO.TxtCode.Text;
                //hsbl.L_Log_Insert(le);

                //更新処理
             //   hsbl.PRC_Hacchuusho_Register(this.InOperatorCD, this.CboStoreCD.SelectedValue.ToString(), this.ScStaff.TxtCode.Text, this.ScVendor.TxtCode.Text, this.ScHacchuuNO.TxtCode.Text);
            }
            finally
            {
                DeleteExclusive(dtForUpdate);
            }

            //更新後画面そのまま
            detailControls[1].Focus();
        }

        private void UpdateOrderD_04()
        {
            //ログ出力
            L_Log_Entity le = new L_Log_Entity();
            le.InsertOperator = this.InOperatorCD;
            le.Program = this.InProgramID;
            le.PC = this.InPcID;
            le.OperateMode = null;
            le.KeyItem = this.ScHacchuuNO.TxtCode.Text;
            hsbl.L_Log_Insert(le);

         //   更新処理
               hsbl.PRC_Hacchuusho_Register(this.InOperatorCD, this.CboStoreCD.SelectedValue.ToString(), this.ScStaff.TxtCode.Text, this.ScVendor.TxtCode.Text, this.ScHacchuuNO.TxtCode.Text);
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
                    {
                        break;
                    }
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        Scr_Clr(0);

                        break;
                    }
                case 8: //F9:検索
                    if (ActiveControl == this.ScVendor.TxtCode)
                    {
                        this.ScVendor.BtnSearch.PerformClick();
                    }
                    else if (ActiveControl == this.ScStaff.TxtCode)
                    {
                        this.ScStaff.BtnSearch.PerformClick();
                    }
                    else if (ActiveControl == this.ScHacchuuNO.TxtCode)
                    {
                        this.ScHacchuuNO.BtnSearch.PerformClick();
                    }

                    break;
                case 11:
                    break;

            }   //switch end

        }
        //private D_Billing_Entity GetSearchInfo()
        //{
        //    dse = new D_Billing_Entity
        //    {
        //        BillingCloseDate = detailControls[(int)EIndex.Simebi].Text,
        //        BillingVendorCD = ScVendor.TxtCode.Text,
        //        //VendorName = detailControls[(int)EIndex.VendorName].Text,
        //        StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),            
        //    };

        //    if (ckM_RadioButton1.Checked)
        //    {
        //        //印刷済
        //        dse.PrintFLG = "1";
        //    }
        //    else
        //    {
        //        //未印刷
        //        dse.PrintFLG = "0";
        //    }

        //    return dse;
        //}

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            DeleteExclusive();

            this.Close();
        }

        #endregion

        #region 初期処理

        private void InitialControlArray()
        {
            detailControls = new Control[] {CboStoreCD,chkInsatuTaishou_Mihakkou,chkInsatuTaishou_Saihakkou,txtHacchuuDateFrom,txtHacchuuDateTo, ScVendor.TxtCode, ScStaff.TxtCode, ScHacchuuNO.TxtCode, chkIsPrintMisshounin,chkIsPrintEDIHacchuu,chkInsatuShurui_Hacchhusho, chkInsatuShurui_NetHacchuu,chkInsatuShurui_Chokusou,chkInsatuShurui_Cancel };

            detailLabels = new Control[] { ScVendor, ScStaff};
            searchButtons = new Control[] { ScVendor.BtnSearch, ScStaff.BtnSearch, ScHacchuuNO.BtnSearch };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }

        #endregion

        #region クリア処理

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
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

            //初期値セット
            string ymd = hsbl.GetDate();
            ScStaff.TxtCode.Text = InOperatorCD;

            txtHacchuuDateFrom.Text = ymd.ToString();
            txtHacchuuDateTo.Text = ymd.ToString();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = hsbl.GetDate()
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
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
            }
            else
            {
                bbl.ShowMessage("E133");
                EndSec();
            }

            //未印刷が初期値
            chkInsatuTaishou_Mihakkou.Checked = true;
            chkInsatuShurui_Hacchhusho.Checked = true;
            chkInsatuShurui_NetHacchuu.Checked = true;
            chkInsatuShurui_Chokusou.Checked = true;
            chkInsatuShurui_Cancel.Checked = true;

            detailControls[(int)EIndex.StoreCD].Focus();
        }
        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearVendorInfo()
        {
            mOldVendorCD = string.Empty;
            ScVendor.LabelText = string.Empty;
        }

        #endregion

        #region 排他処理

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billingNo"></param>
        /// <returns></returns>
        private bool SelectAndInsertExclusive(string billingNo)
        {
            //DeleteExclusive();

            ////排他Tableに該当番号が存在するとError
            ////[D_Exclusive]
            //Exclusive_BL ebl = new Exclusive_BL();
            //D_Exclusive_Entity dee = new D_Exclusive_Entity
            //{
            //    DataKBN = (int)Exclusive_BL.DataKbn.Seikyu,
            //    Number = billingNo,
            //    Program = this.InProgramID,
            //    Operator = this.InOperatorCD,
            //    PC = this.InPcID
            //};

            //DataTable dt = ebl.D_Exclusive_Select(dee);

            //if (dt.Rows.Count > 0)
            //{
            //    bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
            //    return false;
            //}
            //else
            //{
            //    bool ret = ebl.D_Exclusive_Insert(dee);
            //    mOldBillingNo = billingNo;
            //    return ret;
            //}
            return true;
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            //if (mOldBillingNo == "" && dtForUpdate == null)
            //    return;

            //Exclusive_BL ebl = new Exclusive_BL();

            //if (dtForUpdate != null)
            //{
            //    foreach(DataRow dr in dtForUpdate.Rows)
            //    {
            //        D_Exclusive_Entity de = new D_Exclusive_Entity();
            //        de.DataKBN = (int)Exclusive_BL.DataKbn.Seikyu;
            //        de.Number = dr["no"].ToString();

            //        ebl.D_Exclusive_Delete(de);
            //    }
            //    return;
            //}

            //D_Exclusive_Entity dee = new D_Exclusive_Entity
            //{
            //    DataKBN = (int)Exclusive_BL.DataKbn.Seikyu,
            //    Number = mOldBillingNo,
            //};

            //bool ret = ebl.D_Exclusive_Delete(dee);

            //mOldBillingNo = "";
        }

        #endregion

        #region チェック処理

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set = true)
        {
            bool ret;
            switch (index)
            {
                case (int)EIndex.HacchuuDateFrom:
                    detailControls[index].Text = hsbl.FormatDate(detailControls[index].Text);

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!hsbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        hsbl.ShowMessage("E103");
                        return false;
                    }
                    break;
                case (int)EIndex.HacchuuDateTo:
                    detailControls[index].Text = hsbl.FormatDate(detailControls[index].Text);

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!hsbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        hsbl.ShowMessage("E103");
                        return false;
                    }

                    DateTime hacchuuDateFrom;
                    DateTime hacchuuDateTo;
                    DateTime.TryParse(hsbl.FormatDate(detailControls[(int)EIndex.HacchuuDateFrom].Text),out hacchuuDateFrom);
                    DateTime.TryParse(hsbl.FormatDate(detailControls[index].Text),out hacchuuDateTo);
                    if (hacchuuDateFrom > hacchuuDateTo)
                    {
                        hsbl.ShowMessage("E104");
                        return false;
                    }
                    break;

                case (int)EIndex.Staff:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScStaff.LabelText = "";
                        return true;
                    }

                    //スタッフマスター(M_Staff)に存在すること
                    //[M_Staff]
                    M_Staff_Entity mse = new M_Staff_Entity
                    {
                        StaffCD = detailControls[index].Text,
                    };
                    if (string.IsNullOrWhiteSpace(mse.ChangeDate))
                    {
                        mse.ChangeDate = bbl.GetDate();
                    }

                    Staff_BL bl = new Staff_BL();
                    ret = bl.M_Staff_Select(mse);
                    if (ret)
                    {
                        ScStaff.LabelText = mse.StaffName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ScStaff.LabelText = "";
                        return false;
                    }
                    break;


                case (int)EIndex.Vendor:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ClearVendorInfo();
                        return true;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mce = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                    };
                    if (string.IsNullOrWhiteSpace(mce.ChangeDate))
                    {
                        mce.ChangeDate = bbl.GetDate();
                    }

                    Vendor_BL sbl = new Vendor_BL();
                    ret = sbl.M_Vendor_SelectTop1(mce);
                    if (ret)
                    {
                        if (mOldVendorCD != detailControls[index].Text)
                        {
                            ScVendor.LabelText = mce.VendorName;
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        ClearVendorInfo();
                        return false;
                    }

                    mOldVendorCD = detailControls[index].Text;    //位置確認
                    break;

                case (int)EIndex.HacchuuNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    Hacchuusho_BL hbl = new Hacchuusho_BL();
                    var dt = hbl.PRC_Hacchuusho_D_Order_SelectByKey(detailControls[index].Text);
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E138","発注番号");
                        return false;
                    }

                    if (dt.Rows[0]["DeleteDateTime"] == null)
                    {
                        bbl.ShowMessage("E140");
                        return false;
                    }

                    //dt = hbl.PRC_Hacchuusho_M_AutorisationCheck();
                    //if (dt.Rows.Count == 0)
                    //{
                    //    bbl.ShowMessage("E139");
                    //    return false;
                    //}

                    mOldVendorCD = detailControls[index].Text;    //位置確認

                    break;

                case (int)EIndex.StoreCD:
                    if (CboStoreCD.SelectedValue.Equals("-1"))
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

        #endregion

        #region データ取得

        /// <summary>
        /// お買上データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData(out DataTable dtForUpdate)
        {
            dtForUpdate = null;

            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return null;
                }
            }

            if (chkInsatuTaishou_Mihakkou.Checked == false
                && chkInsatuTaishou_Saihakkou.Checked == false)
            {
                bbl.ShowMessage("E111");
                chkInsatuTaishou_Mihakkou.Focus();
                return null;
            }
            if (chkInsatuShurui_Hacchhusho.Checked == false
                && chkInsatuShurui_NetHacchuu.Checked == false
                && chkInsatuShurui_Chokusou.Checked == false
                && chkInsatuShurui_Cancel.Checked == false
               )
            {
                bbl.ShowMessage("E111");
                chkInsatuShurui_Hacchhusho.Focus();
                return null;
            }

            DataTable dt = hsbl.PRC_Hacchuusho_SelectData
                (
                    CboStoreCD.SelectedValue.ToString()
                  , chkInsatuTaishou_Mihakkou.Checked
                  , chkInsatuTaishou_Saihakkou.Checked
                  , txtHacchuuDateFrom.Text
                  , txtHacchuuDateTo.Text
                  , ScVendor.TxtCode.Text
                  , ScStaff.TxtCode.Text
                  , ScHacchuuNO.TxtCode.Text
                  , chkIsPrintMisshounin.Checked
                  , chkIsPrintEDIHacchuu.Checked
                  , chkInsatuShurui_Hacchhusho.Checked
                  , chkInsatuShurui_NetHacchuu.Checked
                  , chkInsatuShurui_Chokusou.Checked
                  , chkInsatuShurui_Cancel.Checked
                );
            //以下の条件で請求データが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
                previousCtrl.Focus();
                return null;
            }
            else
            {
                //明細にデータをセット
                int i = 0;
                dtForUpdate = new DataTable();
                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

                string oldBillingNO = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (row["OrderNO"].ToString() != oldBillingNO)
                    {
                        bool ret = SelectAndInsertExclusive(row["OrderNO"].ToString());
                        if (!ret)
                        {
                            return null;
                        }

                        i++;
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["no"] = row["OrderNO"].ToString();
                        dtForUpdate.Rows.Add(rowForUpdate);
                        oldBillingNO = row["OrderNO"].ToString();
                    }
                }
            }

            return dt;
        }

        #endregion

        private void chkInsatuTaishou_Saihakkou_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ckM_Label1_Click(object sender, EventArgs e)
        {

        }

        private void chkIsPrintEDIHacchuu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkInsatuShurui_Hacchhusho_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkInsatuShurui_NetHacchuu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkInsatuShurui_Chokusou_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkInsatuShurui_Cancel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkIsPrintMisshounin_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}