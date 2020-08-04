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
//using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace MarkDownIchiran
{
    /// <summary>
    /// MarkDownIchiran マークダウン一覧
    /// </summary>
    internal partial class MarkDownIchiran : FrmMainForm
    {
        private const string ProID = "MarkDownIchiran";
        private const string ProNm = "マークダウン一覧";
        private const short mc_L_END = 3; // ロック用
        private const string MarkDown = "MarkDownNyuuryoku.exe";

        private enum EIndex : int
        {
            VendorCD,            
            chkNotAccount,
            chkAccounted,
            CostingDateFrom,
            CostingDateTo,
            PurchaseDateFrom,
            PurchaseDateTo,
            StaffCD,
            StoreCD,
        }

        private enum DColNo : int
        {
            VendorCD,
            VendorName,
            StaffName,
            MarkDownDate,
            CostingDate,
            MarkDownGaku,
            PurchaseDate,
            Comment,
            MarkDownNO,
            StaffCD,

            COUNT
        }

        private enum EColNo : int
        {
            StoreCD,
            StoreName,
            VendorCD,
            VendorName,
            StaffCD,
            StaffName,
            MarkDownDate,
            CostingDate,
            MarkDownGaku,
            PurchaseDate,
            Comment,

            COUNT
        }

        private Control[] detailControls;
       
        private MarkDownIchiran_BL mibl;
        private D_MarkDown_Entity dme;        
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避        

        public MarkDownIchiran()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.SHOW);   //照会プログラムとして起動
                this.InitialControlArray();

                //起動時共通処理
                base.StartProgram();
                Btn_F10.Text = "詳細(F10)";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                mibl = new MarkDownIchiran_BL();
                CboStoreCD.Bind(ymd);

                ckM_Message1.TextAlign = ContentAlignment.TopLeft;
                ckM_Message2.TextAlign = ContentAlignment.TopLeft;

                //画面クリア
                Scr_Clr(0);

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
                }

                detailControls[(int)EIndex.VendorCD].Focus(); 
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                EndSec();

            }
        }
        private void Bind(CKM_Controls.CKM_ComboBox combo, int kbn = 0)
        {
            DenominationKBN_BL dbl = new DenominationKBN_BL();
            M_DenominationKBN_Entity me = new M_DenominationKBN_Entity();
            if (kbn.Equals(0))
                me.SystemKBN = "2";

            DataTable dtNyukinhouhou = dbl.BindKbn(me, kbn);
            BindCombo(combo, "DenominationCD", "DenominationName", dtNyukinhouhou);
            ;
        }
        private void BindCombo(CKM_Controls.CKM_ComboBox combo, string key, string value, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            combo.DataSource = dt;
            combo.DisplayMember = value;
            combo.ValueMember = key;
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { scVendor.TxtCode, ckM_NotAccount, ckM_Accounted, txtCostingDate1, txtCostingDate2
                                            , txtPurchaseDate1,txtPurchaseDate2,ScStaff.TxtCode, CboStoreCD, };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                {
                    ((CheckBox)ctl).CheckedChanged += new System.EventHandler(CheckBox_CheckedChanged);
                    ((CheckBox)ctl).KeyDown += new System.Windows.Forms.KeyEventHandler(CheckBox_KeyDown);
                }
                else
                {
                    ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                    ctl.Enter += new System.EventHandler(DetailControl_Enter);
                }
                
            }
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
                case (int)EIndex.VendorCD:
                    //入力無くても良い
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        scVendor.LabelText = "";
                        return true;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = detailControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = bbl.GetDate()
                    };
                    Vendor_BL sbl = new Vendor_BL();
                    bool ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        if (mve.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            scVendor.LabelText = "";
                            scVendor.TxtCode.SelectAll();
                            return false;
                        }
                        scVendor.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        scVendor.LabelText = "";
                        scVendor.TxtCode.SelectAll();
                        return false;
                    }

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboStoreCD.MoveNext = false;
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            CboStoreCD.MoveNext = false;
                            bbl.ShowMessage("E141");
                            return false;
                        }
                    }

                    break;

                case (int)EIndex.chkNotAccount:
                    if (!ckM_NotAccount.Checked && !ckM_Accounted.Checked)
                    {
                        //Ｅ１１１
                        bbl.ShowMessage("E111");
                        return false;
                    }
                    break;

                case (int)EIndex.CostingDateFrom:
                case (int)EIndex.CostingDateTo:
                case (int)EIndex.PurchaseDateFrom:
                case (int)EIndex.PurchaseDateTo:
                    //入力不可の場合、チェックなし
                    if (!detailControls[index].Enabled)
                    {
                        return true;
                    }

                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    //大小チェック
                    //(From) ≧ (To)である場合Error
                    if (index == (int)EIndex.CostingDateTo || index == (int)EIndex.PurchaseDateTo)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                bbl.ShowMessage("E104");
                                return false;
                            }
                        }
                    }
                    break;

               
                case (int)EIndex.StaffCD:
                    //入力無くても良い
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
                        ScStaff.TxtCode.SelectAll();
                        return false;
                    }

                    break;

            }

            return true;

        }

        /// <summary>
        /// 表示処理
        /// </summary>
        protected override void ExecDisp()
        {
            //入力内容チェック
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }
                
            
            //表示処理
            dme = GetEntity();
            DataTable dt = mibl.D_MarkDown_SelectAll(dme);

            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();

                BtnAdd.Enabled = true;
                BtnExcel.Enabled = true;
            }
            else
            {
                bbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            try
            {
                ////詳細画面を起動します
                //FrmDetail frm = new FrmDetail();
                //doe.CollectPlanNO = GvDetail.CurrentRow.Cells["CollectPlanNO"].Value.ToString();
                //frm.dt = mibl.D_CollectPlan_SelectAllForSyosai(doe);
                //if(frm.dt.Rows.Count >0 )
                //    frm.ShowDialog();
                //else
                //{
                //    mibl.ShowMessage("E128");
                //}

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// EXCEL出力
        /// </summary>
        private void ExecOutput()
        {

            if (GvDetail.Rows.Count > 0)
            {
                string filePath = "";
                if (!ShowSaveFileDialog(InProgramNM, out filePath, 1))
                {
                    return;
                }

                //Excel出力
                this.Output_Excel(this.GvDetail, filePath);

                //ファイル出力が完了しました。
                bbl.ShowMessage("I203");
            }
        }

        private void Output_Excel(DataGridView dgv, string EXCEL_SAVE_PATH)
        {
            if (dgv.Rows.Count > 0)
            {   //Excel出力
                // EXCEL関連オブジェクトの定義
                Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook objWorkBook = objExcel.Workbooks.Add();
                Microsoft.Office.Interop.Excel.Worksheet objSheet = null;

                try
                {
                    objSheet = objWorkBook.Sheets[1];
                    objSheet.Name = "マークダウン一覧";
                    objSheet.Select();

                    objExcel.Visible = false;

                    // 現在日時を取得
                    //string timestanpText = bbl.GetDate();// String.Format(DateTime.Now, "yyyyMMddHHmmss");

                    //// 実行モジュールと同一フォルダのファイルを取得
                    //System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                    //string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + InProgramID;

                    // 保存ディレクトリとファイル名を設定
                    string saveFileName = EXCEL_SAVE_PATH;

                    // 項目名格納用リストを宣言
                    List<string> columnList = new List<string>();
                    // 項目名設定
                    columnList.Add("店舗CD");
                    columnList.Add("店舗名");
                    columnList.Add("仕入先CD");
                    columnList.Add("仕入先名");
                    columnList.Add("入力担当者CD");
                    columnList.Add("担当者名");
                    columnList.Add("登録日");
                    columnList.Add("MD計上日");
                    columnList.Add("MD額");
                    columnList.Add("仕入日");
                    columnList.Add("備考");

                    // シートの最大表示列項目数
                    int columnMaxNum = columnList.Count;
                    // シートの最大表示行項目数
                    int rowMaxNum = dgv.Rows.Count;

                    // セルのデータ取得用二次元配列を宣言
                    string[,] v = new string[rowMaxNum, columnMaxNum];

                    for (int row = 0; row <  rowMaxNum; row++)
                    {
                        v[row, (int)EColNo.StoreCD] = CboStoreCD.SelectedValue.ToString();
                        v[row, (int)EColNo.StoreName] = CboStoreCD.SelectedText.ToString();
                        v[row, (int)EColNo.VendorCD] = dgv.Rows[row].Cells[(int)DColNo.VendorCD].Value.ToString();
                        v[row, (int)EColNo.VendorName] = dgv.Rows[row].Cells[(int)DColNo.VendorName].Value.ToString();
                        v[row, (int)EColNo.StaffCD] = dgv.Rows[row].Cells[(int)DColNo.StaffCD].Value.ToString();
                        v[row, (int)EColNo.StaffName] = dgv.Rows[row].Cells[(int)DColNo.StaffName].Value.ToString();
                        if (dgv.Rows[row].Cells[(int)DColNo.MarkDownDate].Value != null)
                        {
                            v[row, (int)EColNo.MarkDownDate] = dgv.Rows[row].Cells[(int)DColNo.MarkDownDate].Value.ToString();
                        }
                        if (dgv.Rows[row].Cells[(int)DColNo.CostingDate].Value != null)
                        {
                            v[row, (int)EColNo.CostingDate] = dgv.Rows[row].Cells[(int)DColNo.CostingDate].Value.ToString();
                        }
                        v[row, (int)EColNo.MarkDownGaku] = bbl.Z_SetStr(dgv.Rows[row].Cells[(int)DColNo.MarkDownGaku].Value);
                        if (dgv.Rows[row].Cells[(int)DColNo.PurchaseDate].Value != null)
                        {
                            v[row, (int)EColNo.PurchaseDate] = dgv.Rows[row].Cells[(int)DColNo.PurchaseDate].Value.ToString();
                        }
                        if (dgv.Rows[row].Cells[(int)DColNo.Comment].Value != null)
                        {
                            v[row, (int)EColNo.Comment] = dgv.Rows[row].Cells[(int)DColNo.Comment].Value.ToString();
                        }
                    }

                    // ヘッダ出力
                    for (int i = 0; i < columnList.Count; i++)
                    {
                        // シートの一行目に項目を挿入
                        objSheet.Cells[1, i + 1] = columnList[i];

                        // 罫線を設定
                        //objWorkBook.Sheets[1].Cells[1, i + 1].Borders.LineStyle = true;
                        // 項目の表示行に背景色を設定
                        //objWorkBook.Sheets[1].Cells(1, i + 1).Interior.Color = Information.RGB(140, 140, 140);
                        // 文字のフォントを設定
                        objWorkBook.Sheets[1].Cells(1, i + 1).Font.Bold = true;
                    }

                    // EXCELにデータを範囲指定で転送
                    objWorkBook.Sheets[1].Range[objWorkBook.Sheets[1].Cells[2, 1], objWorkBook.Sheets[1].Cells[1 + dgv.Rows.Count, columnMaxNum]] = v;

                    // データの表示範囲に罫線を設定
                    //objWorkBook.Sheets[1].Range[objWorkBook.Sheets[1].Cells[2, 1], objWorkBook.Sheets[1].Cells[1 + dgv.Rows.Count, columnMaxNum]].Borders.LineStyle = true;

                    //// エクセル表示
                    //objExcel.Visible = true;
                    objExcel.DisplayAlerts = false;
                    objWorkBook.SaveAs(saveFileName);

                }
                finally
                {
                    // クローズ
                    if (objWorkBook != null)
                    {
                        objWorkBook.Close(false);
                    }
                    // EXCEL解放
                    Marshal.ReleaseComObject(objSheet);
                    Marshal.ReleaseComObject(objWorkBook);
                    objExcel.Quit();
                }
            }
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_MarkDown_Entity GetEntity()
        {
            dme = new D_MarkDown_Entity();
            dme.VendorCD = scVendor.TxtCode.Text;
            dme.StoreCD = CboStoreCD.SelectedValue.ToString();
            dme.StaffCD = ScStaff.TxtCode.Text;

            if (ckM_NotAccount.Checked)
            {
                dme.CostingDateFrom = detailControls[(int)EIndex.CostingDateFrom].Text;
                dme.CostingDateTo = detailControls[(int)EIndex.CostingDateTo].Text;
            }

            if (ckM_Accounted.Checked)
            {
                dme.PurchaseDateFrom = detailControls[(int)EIndex.PurchaseDateFrom].Text;
                dme.PurchaseDateTo = detailControls[(int)EIndex.PurchaseDateTo].Text;
            }            

            dme.ChkNotAccount = ckM_NotAccount.Checked ? "1" : "0";
            dme.ChkAccounted = ckM_Accounted.Checked ? "1" : "0";

            return dme;
        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
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

                BtnAdd.Enabled = false;
                BtnExcel.Enabled = false;

            }
            
        }

        private void ExecMarkDownNyuuryoku(string markdownNo)
        {
            try
            {
                //int w_Row;
                ////Control w_Act = this.ActiveControl;
                //Control w_Act = this.PreviousCtrl;
                //if (w_Act == null)
                //    return;

                //if (mGrid2.F_Search_Ctrl_MK(w_Act, out int w_Col, out int w_CtlRow) == false)
                //    return;

                //w_Row = w_CtlRow + Vsb_Mei_1.Value;

                ////「受注入力」を照会モードで起動（引数：該当行の受注番号）
                //string juchuuNo = mGrid2.g_DArray[w_Row].JuchuuNO;

                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + MarkDown;
                if (System.IO.File.Exists(filePath))
                {
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + markdownNo;
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
                case 0:     // F1:終了
                    {
                        break;
                    }

                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        break;
                    }

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
                        if (index == (int)EIndex.StaffCD)
                        {
                            ((Control)sender).Focus();
                        }
                        else
                        {
                            ProcessTabKey(!e.Shift);
                            //detailControls[index + 1].Focus();
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

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Array.IndexOf(detailControls, sender);
                if (index == (int)EIndex.chkNotAccount)
                {
                    txtCostingDate1.Enabled = ckM_NotAccount.Checked;
                    txtCostingDate2.Enabled = ckM_NotAccount.Checked;
                    txtCostingDate1.Text = "";
                    txtCostingDate2.Text = ckM_NotAccount.Checked ? bbl.GetDate() : "";
                }
                else if (index == (int)EIndex.chkAccounted)
                {
                    txtPurchaseDate1.Enabled = ckM_Accounted.Checked;
                    txtPurchaseDate2.Enabled = ckM_Accounted.Checked;
                    txtPurchaseDate1.Text = "";
                    txtPurchaseDate2.Text = ckM_Accounted.Checked ? bbl.GetDate() : "";
                }

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                        ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //あたかもTabキーが押されたかのようにする
                    //Shiftが押されている時は前のコントロールのフォーカスを移動
                    ProcessTabKey(!e.Shift);
                }
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        /// <summary>
        /// EXCELボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.ExecOutput();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            this.ExecMarkDownNyuuryoku("");
        }

        private void GvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GvDetail.CurrentRow != null && GvDetail.CurrentRow.Index >= 0)
            {
                string markDownNo = GvDetail.CurrentRow.Cells["colMarkDownNO"].Value.ToString();

                this.ExecMarkDownNyuuryoku(markDownNo);
            }
        }

        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //セルの列を確認
            if (e.ColumnIndex == (int)DColNo.VendorCD )
            {
                ////セルの値により、背景色を変更する
                //switch (e.Value)
                //{
                //    case "承認済":
                //        e.CellStyle.BackColor = ClsGridBase.GridColor;
                //        break;
                //    case "承認中":
                //    case "申請":
                //        e.CellStyle.BackColor = Color.PaleGoldenrod;
                //        break;
                //    case "却下":
                //        e.CellStyle.BackColor = Color.Pink;
                //        break;

                //}
            }
        }


        #endregion

        
    }
}








