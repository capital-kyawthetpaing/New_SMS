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

namespace MailHistoryShoukai
{
    /// <summary>
    /// MailHstoryShoukai メール送信照会
    /// </summary>
    internal partial class MailHistoryShoukai : FrmMainForm
    {
        private const string ProID = "MailHistoryShoukai";
        private const string ProNm = "メール送信照会";
        private const string BatchProID = "MailSend.exe";
        private enum EIndex : int
        {
            BtnStart,
            BtnStop,
            MailDateSt,
            MailTimeSt,
            MailDateEd,
            MailTimeEd,
            Combo1,
            Combo2,
            Customer
        }

        private enum EColNo : int
        {
            ImportDateTime,
            Vendor,
            VendorName,
            ImportDetailsSu,
            ErrorSu,
            ImportFile,

            COUNT
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private MailHistoryShoukai_BL mibl;    
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避 
        private string mEdiMode;

        public MailHistoryShoukai()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);  
                this.InitialControlArray();

                Btn_F10.Text = "前へ(F10)";
                Btn_F11.Text = "次へ(F11)";
                Btn_F12.Text = "表示(F12)";

                //起動時共通処理
                base.StartProgram();
                
                mibl = new MailHistoryShoukai_BL();
                string ymd = mibl.GetDate();
                ScCustomer.SetLabelWidth(350);
                ScCustomer.Value1 = "1";
                ScCustomer.Value2 = "";
                ScCustomer.ChangeDate = ymd;

                BindCombo("KBN", "Name");

                Scr_Clr(0);

                //【受信履歴】
                //画面転送表01に従って、連携履歴情報を表示
                ExecSec();

                SetFuncKeyAll(this, "100001001111");
                detailControls[0].Focus(); 
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
            detailControls = new Control[] { BtnStart, BtnStop, ckM_TextBox1, ckM_TextBox3, ckM_TextBox2, ckM_TextBox4, ckM_ComboBox1,ckM_ComboBox2,ScCustomer.TxtCode };
            detailLabels = new Control[] { lblSendedDateTime,  lblCustomer, lblJuchuuNo, lblMailKBN, lblAddress1, lblAddress2, lblAddress3, lblMailSubject ,lblMailContent      };
            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        private void BindCombo(string key, string value)
        {
            DataTable dt = new DataTable();

            //1:受注連絡、3:督促、5:社内事務、7:発注、9:その他
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);
            AddRow(dt, key, value, "1", "受注連絡");
            AddRow(dt, key, value, "3", "督促");
            AddRow(dt, key, value, "5", "社内事務");
            AddRow(dt, key, value, "7", "発注");
            AddRow(dt, key, value, "9", "その他");

            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            ckM_ComboBox1.DataSource = dt;
            ckM_ComboBox1.DisplayMember = value;
            ckM_ComboBox1.ValueMember = key;

            //11:注文確認,12:入荷予定,
            //13:出荷予定,14:出荷案内,
            //15:入金御礼,16:アフターフォロー,
            //31:督促1回目,32:督促2回目,
            //33:督促3回目,34:督促4回目,
            //51:移動依頼
            //71:EDI発注(mail)
            //99:その他
            dt = new DataTable();
            dt.Columns.Add(key);
            dt.Columns.Add(value);
            AddRow(dt, key, value, "11", "注文確認");
            AddRow(dt, key, value, "12", "入荷予定");
            AddRow(dt, key, value, "13", "出荷予定");
            AddRow(dt, key, value, "14", "出荷案内");
            AddRow(dt, key, value, "15", "入金御礼");
            AddRow(dt, key, value, "16", "アフターフォロー");
            AddRow(dt, key, value, "31", "督促1回目");
            AddRow(dt, key, value, "32", "督促2回目");
            AddRow(dt, key, value, "33", "督促3回目");
            AddRow(dt, key, value, "34", "督促4回目");
            AddRow(dt, key, value, "51", "移動依頼");
            AddRow(dt, key, value, "71", "EDI発注(mail)");
            AddRow(dt, key, value, "99", "その他");

            dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            ckM_ComboBox2.DataSource = dt;
            ckM_ComboBox2.DisplayMember = value;
            ckM_ComboBox2.ValueMember = key;
        }
        private void AddRow(DataTable dt, string key, string value, string keyVal, string valueStr)
        {
            DataRow datarow = dt.NewRow();
            datarow[key] = keyVal;
            datarow[value] = valueStr;
            dt.Rows.Add(datarow);
        }
        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.MailDateSt:
                case (int)EIndex.MailDateEd:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    detailControls[index].Text = mibl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //大小チェック
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.MailDateEd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.MailDateSt].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[(int)EIndex.MailDateSt].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                return false;
                            }
                        }
                    }
                    break;

                case (int)EIndex.MailTimeSt:
                case (int)EIndex.MailTimeEd:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }
                    detailControls[index].Text = FormatTime(detailControls[index].Text);

                    //時刻として正しい
                    if (!CheckTime(detailControls[index].Text))
                    {
                        //Ｅ２１５
                        bbl.ShowMessage("E215");
                        return false;
                    }
                    //大小チェック
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.MailTimeEd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.MailTimeSt].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            if (detailControls[(int)EIndex.MailDateSt].Text == detailControls[(int)EIndex.MailDateEd].Text)
                            {
                                int result = detailControls[index].Text.CompareTo(detailControls[(int)EIndex.MailTimeSt].Text);
                                if (result < 0)
                                {
                                    bbl.ShowMessage("E104");
                                    return false;
                                }
                            }
                        }
                    }
                    break;

                case (int)EIndex.Customer:
                    if (detailControls[index].Enabled)
                    {
                        //入力無くても良い(It is not necessary to input)
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            return true;
                        }

                        string ymd = mibl.GetDate();
                        if (ScCustomer.Stype == CKM_SearchControl.SearchType.得意先)
                        {
                            //[M_Customer_Select]
                            M_Customer_Entity mce = new M_Customer_Entity
                            {
                                CustomerCD = detailControls[index].Text,
                                ChangeDate = ymd
                            };
                            Customer_BL sbl = new Customer_BL();
                            bool ret = sbl.M_Customer_Select(mce, 1);

                            if (ret)
                            {
                                if(mce.DeleteFlg.Equals("1"))
                                {

                                }
                                ScCustomer.LabelText = mce.CustomerName;
                            }
                        }
                        else
                        {
                            //[M_Vendor_Select]
                            M_Vendor_Entity mve = new M_Vendor_Entity
                            {
                                VendorCD = detailControls[index].Text,
                                VendorFlg = "1",
                                ChangeDate = ymd
                            };
                            Vendor_BL sbl = new Vendor_BL();
                            bool ret = sbl.M_Vendor_SelectTop1(mve);

                            if (ret)
                            {
                            }
                        }
                    }
                    break;
            }
            return true;

        }
        private string FormatTime(string time)
        {
            string formatStr = "";

            formatStr = bbl.Z_Set(time.Replace(":", "")).ToString("00:00");

            return formatStr;
        }
        private bool CheckTime(string time)
        {
            string strTime = bbl.GetDate() + " " + time + ":00";
            string format = "yyyy/MM/dd HH:mm:ss";
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            System.Globalization.DateTimeStyles dts = System.Globalization.DateTimeStyles.None;
            DateTime dateTime;

            if (DateTime.TryParseExact(strTime, format, ci, dts, out dateTime))
            {
                return true;
            }

            return false;
        }
        private void SetData(int rowIndex)
        {
            if (rowIndex < 0)
                return;

            foreach (Control ctl in detailLabels)
            {
                ctl.Text = "";
            }

            //画面転送表03
            lblSendedDateTime.Text = GvDetail.Rows[rowIndex].Cells["coIMailDateTime"].Value.ToString();
            string MailKBN = GvDetail.Rows[rowIndex].Cells["colMailKBN"].Value.ToString();

            switch (MailKBN)
            {
                case "11": lblMailKBN.Text = "注文確認"; break;
                case "12": lblMailKBN.Text = "入荷予定"; break;
                case "13": lblMailKBN.Text = "出荷予定"; break;
                case "14": lblMailKBN.Text = "出荷案内"; break;
                case "15": lblMailKBN.Text = "入金御礼"; break;
                case "16": lblMailKBN.Text = "アフターフォロー"; break;
                case "31": lblMailKBN.Text = "督促１回目"; break;
                case "32": lblMailKBN.Text = "督促２回目"; break;
                case "33": lblMailKBN.Text = "督促３回目"; break;
                case "34": lblMailKBN.Text = "督促４回目"; break;
                case "51": lblMailKBN.Text = "移動依頼"; break;
                case "71": lblMailKBN.Text = "EDI発注"; break;
                case "99": lblMailKBN.Text = "その他"; break;
            }
            lblJuchuuNo.Text = GvDetail.Rows[rowIndex].Cells["colNumber"].Value.ToString();

            D_Mail_Entity de = new D_Mail_Entity();
            de.MailCounter = GvDetail.Rows[rowIndex].Cells["colMailCounter"].Value.ToString();

            DataTable dt = mibl.D_MailAddress_SelectAll(de);
            foreach (DataRow row in dt.Rows)
            {
                switch (row["AddressKBN"].ToString())
                {
                    case "1":
                        lblAddress1.Text += (lblAddress1.Text == "" ? "":",") + row["Address"].ToString();
                        break;
                    case "2":
                        lblAddress2.Text += (lblAddress2.Text == "" ? "" : ",") + row["Address"].ToString();
                        break;
                    case "3":
                        lblAddress3.Text += (lblAddress3.Text == "" ? "" : ",") + row["Address"].ToString();
                        break;
                }
            }
            lblMailSubject.Text = GvDetail.Rows[rowIndex].Cells["colMailSubject"].Value.ToString();
            lblCustomer.Text = GvDetail.Rows[rowIndex].Cells["Customer"].Value.ToString();
            lblMailContent.Text = GvDetail.Rows[rowIndex].Cells["MailContent"].Value.ToString();
        }

        protected override void ExecSec()
        {
            try
            {
                //【取込履歴】
                M_MultiPorpose_Entity me = new M_MultiPorpose_Entity
                {
                    ID = MultiPorpose_BL.ID_Mail,
                    Key = "1"
                };

                MultiPorpose_BL mbl = new MultiPorpose_BL();
                DataTable dt = mbl.M_MultiPorpose_Select(me);
                if (dt.Rows.Count > 0)
                {
                    mEdiMode = dt.Rows[0]["Num1"].ToString();

                    if (mEdiMode == "1")
                    {
                        //汎用マスター.	数字型１＝１なら、「処理実行中」として青色		
                        lblEdiMode.Text = "処理実行中";
                        lblEdiMode.BackColor = Color.DeepSkyBlue;
                    }
                    else
                    {
                        //汎用マスター.	数字型１＝０なら、「処理停止中」として黄色				
                        lblEdiMode.Text = "処理停止中";
                        lblEdiMode.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    //Ｅ１０１
                    bbl.ShowMessage("E101");
                    EndSec();
                }

                for (int i = 0; i < detailControls.Length; i++)
                    if (CheckDetail(i) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }

                //履歴データ取得処理
                D_Mail_Entity de = new D_Mail_Entity
                {
                    
                    MailDateFrom = detailControls[(int)EIndex.MailDateSt].Text,
                    MailDateTo = detailControls[(int)EIndex.MailDateEd].Text,
                    MailTimeFrom = detailControls[(int)EIndex.MailTimeSt].Text != "" ? detailControls[(int)EIndex.MailDateSt].Text + " " + detailControls[(int)EIndex.MailTimeSt].Text + ":00":"",
                    MailTimeTo = detailControls[(int)EIndex.MailTimeEd].Text != "" ? detailControls[(int)EIndex.MailDateEd].Text + " " + detailControls[(int)EIndex.MailTimeEd].Text + ":00" : "",
                    MailType = ckM_ComboBox1.SelectedIndex > 0 ? ckM_ComboBox1.SelectedValue.ToString(): "0",
                    MailKBN = ckM_ComboBox2.SelectedIndex > 0 ? ckM_ComboBox2.SelectedValue.ToString() : "0",
                    CustomerCD = detailControls[(int)EIndex.Customer].Text,
                    VendorCD = detailControls[(int)EIndex.Customer].Text,
                };
                dt = mibl.D_Mail_SelectAll(de);

                GvDetail.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    GvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    GvDetail.CurrentRow.Selected = true;
                    GvDetail.Enabled = true;
                    GvDetail.ReadOnly = true;
                    GvDetail.Focus();
                }
                else
                {
                    bbl.ShowMessage("E128");
                    GvDetail.DataSource = null;
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void CheckProcess()
        {
            System.Diagnostics.Process[] ps =
             System.Diagnostics.Process.GetProcessesByName(BatchProID);

            if (ps.Length > 0)
            {
                //MessageBox.Show(strProcessName + "を検出しました。");
            }
            else
            {
                //BatchProIDを起動
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + BatchProID;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID;
                    System.Diagnostics.Process.Start(filePath, cmdLine);
                }
                else
                {
                    //ファイルなし
                }
            }

        }
        private void  UpdateM_MultiPorpose()
        {
            M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();
            mme.ID = MultiPorpose_BL.ID_Mail;
            mme.Key = "1";
            mme.UpdateOperator = InOperatorCD;
            mme.PC = InPcID;

            if (mEdiMode == "1")
            {
                //　記憶していた初期表示値が１
                //＆画面・在庫SKS連携処理の処理モードが「処理停止中」の場合
                if(lblEdiMode.Text== "処理停止中")
                {
                    mme.Num1 = "0";
                }
                else
                {
                    return;
                }
            }
            else
            {
                //　記憶していた初期表示値が０
                //＆画面・在庫SKS連携処理の処理モードが「処理実行中」の場合
                if (lblEdiMode.Text == "処理実行中")
                {
                    mme.Num1 = "1";
                }
                else
                {
                    return;
                }
            }

            mibl.M_MultiPorpose_Update(mme);
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
                    //radioButton1.Checked = true;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
                }
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
                {

                }
                else
                {
                    ctl.Text = "";
                }
            }
            foreach (Control ctl in detailLabels)
            {
                ctl.Text = "";
            }

            string ymd = mibl.GetDate();
            detailControls[(int)EIndex.MailDateSt].Text = ymd;
            detailControls[(int)EIndex.MailDateEd].Text = ymd;

            GvDetail.DataSource = null;
        }

        /// <summary>
        /// handle f1 to f12 click event
        /// implement base virtual function
        /// </summary>
        /// <param name="Index"></param>
        public override void FunctionProcess(int Index)
        {
            base.FunctionProcess(Index);
            int rowIndex = GvDetail.CurrentRow != null? GvDetail.CurrentRow.Index : 0;

            switch (Index)
            {
                case 0:     // F1:終了
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
                case 9://F10:前へ
                    if (rowIndex - 1 >= 0)
                    {
                        GvDetail.CurrentCell= GvDetail[0, rowIndex - 1];
                        SetData(rowIndex - 1);
                    }
                    break;

                case 10://F11:次へ
                    if (rowIndex + 1 < GvDetail.Rows.Count)
                    {
                        GvDetail.CurrentCell = GvDetail[0, rowIndex + 1];
                        SetData(rowIndex + 1);
                    }
                    break;

                case 11://F12:最新化
                    ExecSec();
                    break;
            }   //switch end

        }

        // ==================================================
        // 終了処理
        // ==================================================
        protected override void EndSec()
        {
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
                        if (index == (int)EIndex.Customer)
                        {
                            BtnSubF12.Focus();
                        }
                        else if (detailControls[index + 1].CanFocus)
                        {
                            detailControls[index + 1].Focus();
                        }
                        else
                        {
                            BtnSubF12.Focus();
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

        private void BtnSubF12_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
            try
            {
                FunctionProcess(FuncExec - 1);

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
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //現状、「処理実行中」の場合、何もしない												
                if (lblEdiMode.Text != "処理実行中")
                {
                    //現用、「処理停止中」(黄色)の場合、「処理実行中」(青色)とする																
                    lblEdiMode.Text = "処理実行中";
                    lblEdiMode.BackColor = Color.DeepSkyBlue;

                }
                //どちらの場合でもプログラム「MailSend.exe」が起動中でなければ起動する。
                //CheckProcess();

                //テーブル転送仕様Ａ、テーブル転送仕様Ｚに従って、更新処理。
                UpdateM_MultiPorpose();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                //現状、「処理停止中」の場合、何もしない												
                if (lblEdiMode.Text != "処理停止中")
                {
                    //現用、「処理実行中」(青色)の場合、「処理停止中」(黄色)とする																
                    lblEdiMode.Text = "処理停止中";
                    lblEdiMode.BackColor = Color.Yellow;

                }
                //テーブル転送仕様Ａ、テーブル転送仕様Ｚに従って、更新処理。
                UpdateM_MultiPorpose();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void ckM_ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                	
                if (ckM_ComboBox1.SelectedIndex > 0)
                {
                    switch (ckM_ComboBox1.SelectedValue)
                    {
                        case "1"://受注連絡or	督促の場合、	
                        case "3":
                            lblTitle.Text = "得意先";
                            ScCustomer.Stype = CKM_SearchControl.SearchType.得意先;
                            ScCustomer.Enabled = true;
                            ScCustomer.BtnSearch.Enabled = true;
                            break;
                        case "7"://発注
                            lblTitle.Text = "仕入先";
                            ScCustomer.Stype = CKM_SearchControl.SearchType.仕入先;
                            ScCustomer.Enabled = true;
                            ScCustomer.BtnSearch.Enabled = true;
                            break;
                        default:
                            ScCustomer.Enabled = false;
                            ScCustomer.TxtCode.Text = "";
                            ScCustomer.BtnSearch.Enabled = false;
                            ScCustomer.LabelText = "";
                            break;
                    }
                }
                else
                {
                    ScCustomer.Enabled = false;
                    ScCustomer.TxtCode.Text = "";
                    ScCustomer.BtnSearch.Enabled = false;
                    ScCustomer.LabelText = "";
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void GvDetail_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                SetData(e.RowIndex);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        #endregion

    }
}








