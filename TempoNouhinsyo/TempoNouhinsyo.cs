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

namespace TempoNouhinsyo
{
    /// <summary>
    /// TempoNouhinsyo 店舗納品書
    /// </summary>
    internal partial class TempoNouhinsyo : FrmMainForm
    {
        private const string ProID = "TempoNouhinsyo";
        private const string ProNm = "店舗納品書・請求書";

        private enum EIndex : int
        {
            DayStart =0,
            DayEnd,
            StoreCD,
            StaffCD,
            CustomerCD,
            CustomerName,
            SalesNO,

        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private TempoNouhinsyo_BL mibl;
        private D_Sales_Entity dse;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldSalesNO = "";    //排他処理のため使用
        private string mOldCustomerCD = "";

        private string StartUpKBN = "";

        public TempoNouhinsyo()
        {
            InitializeComponent();
                    }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.PRINT);
                this.InitialControlArray();
                base.Btn_F10.Text = "";
                
                //起動時共通処理
                base.StartProgram();

                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                ScUriageNO.Value1 = InOperatorCD;
                ScUriageNO.Value2 = stores;
                ScCustomer.Value1 = "1";

                mibl = new TempoNouhinsyo_BL();
                CboStoreCD.Bind(string.Empty);


                SetFuncKeyAll(this, "100001000011");
                Scr_Clr(0);

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if (cmds.Length - 1 > (int)ECmdLine.PcID)
                {
                    detailControls[(int)EIndex.SalesNO].Text = cmds[(int)ECmdLine.PcID + 1];
                    StartUpKBN = cmds[cmds.Length - 1];

                    if (StartUpKBN == "1")
                    {
                        WindowState = FormWindowState.Minimized;
                        ShowInTaskbar = false;
                    }
                    PrintMode = EPrintMode.DIRECT;
                    PrintSec();

                    if (StartUpKBN == "1")
                    {
                        EndSec();
                    }
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
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2, CboStoreCD, ScStaff.TxtCode, ScCustomer.TxtCode, ckM_CustomerName, ScUriageNO.TxtCode };
            detailLabels = new Control[] { ScCustomer,ScStaff,ScUriageNO };
            searchButtons = new Control[] { ScCustomer.BtnSearch};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }

            ckM_RadioButton1.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton2.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            ckM_RadioButton3.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
        }
       

        private bool SelectAndInsertExclusive(string salesNo)
        {
            //DeleteExclusive();

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Uriage,
                Number = salesNo,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                detailControls[(int)EIndex.SalesNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldSalesNO = salesNo;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            if (mOldSalesNO == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach(DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity();
                    de.DataKBN = (int)Exclusive_BL.DataKbn.Uriage;
                    de.Number = dr["no"].ToString();

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }

            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Uriage,
                Number = mOldSalesNO,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldSalesNO = "";
        }
        
        /// <summary>
        /// お買上データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData(out DataTable  dtForUpdate)
        {
            string salesNo = detailControls[(int)EIndex.SalesNO].Text;
            dtForUpdate = null;

            for (int i = 0; i < detailControls.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(salesNo) && i != (int)EIndex.SalesNO)
                    continue;

                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return null;
                }
            }

            if (string.IsNullOrWhiteSpace(salesNo))
            {
                dse= GetSearchInfo();
            }
            else
            {
                //[D_Sales_SelectForPrint]
                dse = new D_Sales_Entity
                {
                    SalesNO = salesNo
                };

            }

            DataTable dt = mibl.D_Sales_SelectForPrint(dse);

            //以下の条件で売上データが存在しなければエラー (Error if record does not exist)Ｅ１３３
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

                foreach (DataRow row in dt.Rows)
                {
                    if (row["SalesRows"].ToString() == "1" || row["SalesRows"].ToString() == "")
                    {
                        bool ret = SelectAndInsertExclusive(row["SalesNO"].ToString());
                        if (!ret)
                            return null;

                        i++;
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["no"] = row["SalesNO"].ToString();
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                }
            }

            return dt;
        }
        protected override void PrintSec()
        {
            DataTable dtForUpdate = null;

            try
            {
                // レコード定義を行う
                DataTable table = CheckData(out dtForUpdate);

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
                TempoNouhinsyo_Report Report = new TempoNouhinsyo_Report();

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:
                        if (StartUpKBN == "1")
                        {
                            ret = DialogResult.No;
                        }
                        else { 
                            //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                            ret = bbl.ShowMessage("Q208");
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

                        bool result = OutputPDF(filePath, Report);
                        
                        //PDF出力が完了しました。
                        bbl.ShowMessage("I202");

                        break;
                }

                //更新処理
                //tableの売上番号だけ
                mibl.D_Sales_Update(dse, dtForUpdate, InOperatorCD, InPcID);
                
            }
            finally
            {
                DeleteExclusive(dtForUpdate);
            }

            //更新後画面そのまま
            detailControls[0].Focus();
        }

        /// <summary>
        /// HEAD部のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private bool CheckDetail(int index, bool set=true)
        {
            bool ret;
            switch (index)
            {
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = mibl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!mibl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        mibl.ShowMessage("E103");
                        return false;
                    }
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.DayEnd )
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                mibl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }
                    

                    break;

                case (int)EIndex.StaffCD:
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
                        ChangeDate = bbl.GetDate()
                    };
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
                    

                case (int)EIndex.CustomerCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return true;
                    }

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = bbl.GetDate()
                    };
                    Customer_BL sbl = new Customer_BL();
                     ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        if (mOldCustomerCD != detailControls[index].Text)
                        {

                            if (mce.VariousFLG == "1")
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                    detailControls[index + 1].Enabled = true;

                            }
                            else
                            {
                                detailControls[index + 1].Text = mce.CustomerName;
                                
                                //[M_Store_Select]
                                M_Store_Entity me = new M_Store_Entity
                                {
                                    StoreCD = mce.LastSalesStoreCD,
                                    ChangeDate = mce.LastSalesDate
                                };
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    mOldCustomerCD = detailControls[index].Text;    //位置確認

                    break;

                //case (int)EIndex.CustomerName:
                //    //入力可能の場合 入力必須(Entry required)
                //    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }
                //    break;

                case (int)EIndex.SalesNO:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    //[D_Sales_SelectData]
                    dse = new D_Sales_Entity();
                    dse.SalesNO = detailControls[(int)EIndex.SalesNO].Text;
                    DataTable dt = mibl.D_Sales_SelectData(dse, (short)EOperationMode.SHOW);
                    //以下の条件で売上入力ーが存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E133");
                        ScUriageNO.LabelText = "";
                        previousCtrl.Focus();
                        return false;
                    }
                    else
                    {
                        //権限がない場合（以下のSelectができない場合）Error
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E141");
                            ScUriageNO.LabelText = "";
                            previousCtrl.Focus();

                            return false;
                        }
                        
                    }

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

        private D_Sales_Entity GetSearchInfo()
        {
            dse = new D_Sales_Entity
            {
                SalesDateFrom = detailControls[(int)EIndex.DayStart].Text,
                SalesDateTo = detailControls[(int)EIndex.DayEnd].Text,
                StaffCD = ScStaff.TxtCode.Text,
                CustomerCD = ScCustomer.TxtCode.Text,
                //CustomerName = detailControls[(int)EIndex.CustomerName].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),            
            };

            if (ckM_RadioButton1.Checked)
            {
            }
            else if (ckM_RadioButton2.Checked)
            {
                //印刷済
                dse.PrintFLG = "1";
            }
            else
            {
                //未印刷
                dse.PrintFLG = "0";
            }

            return dse;
        }

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
                else if (ctl.GetType().Equals(typeof(Panel)))
                {
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex=-1;
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
            string ymd = mibl.GetDate();
            ScStaff.TxtCode.Text = InOperatorCD;

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = mibl.GetDate()
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

            detailControls[(int)EIndex.DayStart].Text = ymd;
            detailControls[(int)EIndex.DayEnd].Text = ymd;

            //未印刷が初期値
            ckM_RadioButton3.Checked = true;

            detailControls[0].Focus();
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            mOldCustomerCD = "";
            

            ScCustomer.LabelText = "";
            detailControls[(int)EIndex.CustomerName].Text = "";
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

                case 11:   
                        break;
                    
            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
            DeleteExclusive();

            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        #region "内部イベント"

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
                switch(index)
                    {
                    case (int)EIndex.CustomerCD:
                    case (int)EIndex.StaffCD:
                    case (int)EIndex.SalesNO:
                        F9Visible = true;
                        break;

                    default:
                        F9Visible = false;
                        break;
                }
                //SetFuncKeyAll(this, "111111000011");
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

                    detailControls[(int)EIndex.SalesNO].Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}








