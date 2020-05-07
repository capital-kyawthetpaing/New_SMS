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

namespace Mitsumorisyo
{
    /// <summary>
    /// Mitsumorisyo 見積書
    /// </summary>
    internal partial class Mitsumorisyo : FrmMainForm
    {
        private const string ProID = "Mitsumorisyo";
        private const string ProNm = "見積書";

        private enum EIndex : int
        {
            DayStart =0,
            DayEnd,
            StoreCD,
            InputStart,
            InputEnd,
            StaffCD,
            CustomerCD,
            CustomerName,
            MitsumoriName,
            MitsumoriNO,

        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private MitsumoriNyuuryoku_BL mibl;
        private D_Mitsumori_Entity dme;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldMitsumoriNo = "";    //排他処理のため使用
        private string mOldCustomerCD = "";


        public Mitsumorisyo()
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
                ScMitsumoriNO.Value1 = InOperatorCD;
                ScMitsumoriNO.Value2 = stores;
                ScCustomer.Value1 = "1";    //1:販売先

                mibl = new MitsumoriNyuuryoku_BL();
                CboStoreCD.Bind(string.Empty);

                SetFuncKeyAll(this, "100001000011");
                Scr_Clr(0);

                //コマンドライン引数を配列で取得する
                string[] cmds = System.Environment.GetCommandLineArgs();
                if(cmds.Length-1 > (int)ECmdLine.PcID)
                {
                    detailControls[(int)EIndex.MitsumoriNO].Text = cmds[cmds.Length - 1];
                    PrintSec();
                    EndSec();
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
            detailControls = new Control[] { ckM_TextBox1, ckM_TextBox2, CboStoreCD, ckM_TextBox3, ckM_TextBox4, ScStaff.TxtCode, ScCustomer.TxtCode, ckM_CustomerName, ckM_TextBox5, ScMitsumoriNO.TxtCode };
            detailLabels = new Control[] { ScCustomer,ScStaff,ScMitsumoriNO };
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
       

        private bool SelectAndInsertExclusive(string mitsumoriNo)
        {
            //DeleteExclusive();    一括で行う

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = mitsumoriNo,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                detailControls[(int)EIndex.MitsumoriNO].Focus();
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldMitsumoriNo = mitsumoriNo;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
       private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            if (mOldMitsumoriNo == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach(DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity();
                    de.DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori;
                    de.Number = dr["no"].ToString();

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }

            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = mOldMitsumoriNo,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldMitsumoriNo = "";
        }
        
        /// <summary>
        /// 見積データ取得処理
        /// </summary>
        /// <param name="set">画面展開なしの場合:falesに設定する</param>
        /// <returns></returns>
        private DataTable CheckData(out DataTable  dtForUpdate)
        {
            string mitsumoriNo = detailControls[(int)EIndex.MitsumoriNO].Text;
            dtForUpdate = null;

            for (int i = 0; i < detailControls.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(mitsumoriNo) && i != (int)EIndex.MitsumoriNO)
                    continue;

                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return null;
                }
            }

            if (string.IsNullOrWhiteSpace(mitsumoriNo))
            {
                dme= GetSearchInfo();
            }
            else
            {
                //[D_Mitsumori_SelectForPrint]
                dme = new D_Mitsumori_Entity
                {
                    MitsumoriNO = mitsumoriNo
                };

            }

            DataTable dt = mibl.D_Mitsumori_SelectForPrint(dme);

            //以下の条件で見積データが存在しなければエラー (Error if record does not exist)Ｅ１３３
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
                    if (row["DisplayRows"].ToString() == "1" || row["DisplayRows"].ToString() == "")
                    {
                        bool ret = SelectAndInsertExclusive(row["MitsumoriNo"].ToString());
                        if (!ret)
                            return null;

                        i++;
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["no"] = row["MitsumoriNo"].ToString();
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
                //table.WriteXmlSchema(ProID + ".xsd");

                //①保存した.xsdはプロジェクトに追加しておきます。
                DialogResult ret;
                Mitsumorisyo_Report Report = new Mitsumorisyo_Report();

                switch (PrintMode)
                {
                    case EPrintMode.DIRECT:

                        //Q208 印刷します。”はい”でプレビュー、”いいえ”で直接プリンターから印刷します。
                        ret = bbl.ShowMessage("Q208");
                        if (ret == DialogResult.Cancel)
                        {
                            return;
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
                //tableの見積番号だけ
                mibl.D_Mitsumori_Update(dme, dtForUpdate, InOperatorCD, InPcID);
                
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
                case (int)EIndex.InputStart:
                case (int)EIndex.InputEnd:
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
                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.InputEnd)
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

                case (int)EIndex.MitsumoriName:
                    break;
                case (int)EIndex.MitsumoriNO:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    //[D_Mitsumori_SelectData]
                    dme = new D_Mitsumori_Entity();
                    dme.MitsumoriNO = detailControls[(int)EIndex.MitsumoriNO].Text;
                    DataTable dt = mibl.D_Mitsumori_SelectData(dme, (short)EOperationMode.SHOW);
                    //以下の条件で見積入力ーが存在しなければエラー (Error if record does not exist)Ｅ１３３
                    if (dt.Rows.Count == 0)
                    {
                        bbl.ShowMessage("E133");
                        ScMitsumoriNO.LabelText = "";
                        previousCtrl.Focus();
                        return false;
                    }
                    else
                    {
                        //権限がない場合（以下のSelectができない場合）Error
                        if (!base.CheckAvailableStores(dt.Rows[0]["StoreCD"].ToString()))
                        {
                            bbl.ShowMessage("E141");
                            ScMitsumoriNO.LabelText = "";
                            previousCtrl.Focus();

                            return false;
                        }

                        ScMitsumoriNO.LabelText = dt.Rows[0]["MitsumoriName"].ToString();
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

        private D_Mitsumori_Entity GetSearchInfo()
        {
            dme = new D_Mitsumori_Entity
            {
                MitsumoriDateFrom = detailControls[(int)EIndex.DayStart].Text,
                MitsumoriDateTo = detailControls[(int)EIndex.DayEnd].Text,
                MitsumoriInputDateFrom = detailControls[(int)EIndex.InputStart].Text,
                MitsumoriInputDateTo = detailControls[(int)EIndex.InputEnd].Text,
                MitsumoriName = detailControls[(int)EIndex.MitsumoriName].Text,
                StaffCD = ScStaff.TxtCode.Text,
                CustomerCD = ScCustomer.TxtCode.Text,
                CustomerName = detailControls[(int)EIndex.CustomerName].Text,
                StoreCD = CboStoreCD.SelectedValue.ToString().Equals("-1") ? string.Empty : CboStoreCD.SelectedValue.ToString(),            
            };

            if (ckM_RadioButton1.Checked)
            {
            }
            else if (ckM_RadioButton2.Checked)
            {
                //印刷済
                dme.PrintFLG = "1";
            }
            else
            {
                //未印刷
                dme.PrintFLG = "0";
            }

            return dme;
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
                            ScCustomer.BtnSearch.Enabled = Kbn == 0 ? true : false;

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
                            if(index== (int)EIndex.MitsumoriName)
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                            else if (detailControls[index + 1].CanFocus)
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
                    case (int)EIndex.MitsumoriNO:
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

                    detailControls[(int)EIndex.MitsumoriNO].Focus();
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








