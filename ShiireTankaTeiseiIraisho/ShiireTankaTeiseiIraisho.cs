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
using System.Runtime.InteropServices; //EXCEL出力(必要)
using Microsoft.Office.Interop;//EXCEL出力(必要)

namespace ShiireTankaTeiseiIraisho
{
    /// <summary>
    /// ShiireTankaTeiseiIraisho 仕入単価訂正依頼書印刷
    /// </summary>
    internal partial class ShiireTankaTeiseiIraisho : FrmMainForm
    {
        private const string ProID = "ShiireTankaTeiseiIraisho";
        private const string ProNm = "仕入単価訂正依頼書印刷";

        private enum EIndex : int
        {
            StoreCD,
            ArrivalPlanDateFrom,
            ArrivalPlanDateTo,

            OrderCD,

            CheckBox3,
            CheckBox4,

        }
        private const int GYO_COUNT = 27;   //1ページのEXCEL行数
        private const int DATA_COUNT = 11;  //1ページのデータの行数
        private enum ECell : int
        {
            VendorName = 2,
            OrderPerson = 2,

            PurchaseDate = 1,
            VendorDeliveryNo = 8,
            MakerItem = 15,
            ColorName = 34,
            //SizeName,
            PurchaserUnitPrice = 47,
            OrderUnitPrice = 53,
            PurchaseSu = 59,

            Print1 = 2,
            Print2 = 2,
            Print3 = 2,
            Print4 = 2,
            ZIP = 54,
            CompanyName = 54,
            TEL = 54
        }

        private enum ERow : int
        {
            VendorName = 3,
            OrderPerson = 5,

            DetailStartRow = 12,

            Print1 = 23,
            Print2 = 24,
            Print3 = 25,
            Print4 = 26,
            Print5 = 27,
            ZIP = 25,
            CompanyName = 26,
            TEL = 27
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;

        private ShiireTankaTeiseiIraisho_BL stbl;
        private D_Purchase_Entity dpe;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避


        public ShiireTankaTeiseiIraisho()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.PRINT);
                this.InitialControlArray();
                base.Btn_F10.Text = "";
                base.Btn_F11.Text = "";
                base.Btn_F12.Text = "出力(F12)";
                //起動時共通処理
                base.StartProgram();

                stbl = new ShiireTankaTeiseiIraisho_BL();
                string ymd = stbl.GetDate();
                CboStoreCD.Bind(ymd);

                ScOrder.Value1 = "1";

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
        private void InitialControlArray()
        {
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1 ,ckM_TextBox2
                    , ScOrder.TxtCode,ChkMisyutsuryoku,ChkSyutsuryokuZumi
                         };
            detailLabels = new Control[] { ScOrder };
            searchButtons = new Control[] { ScOrder.BtnSearch };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }

        /// <summary>
        /// 仕入データ取得処理
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

            //どれかのチェックがあること	
            if (!ChkMisyutsuryoku.Checked && !ChkSyutsuryokuZumi.Checked)
            {
                //Ｅ１８０
                bbl.ShowMessage("E111");
                ChkMisyutsuryoku.Focus();
                return null;
            }

            //[D_Purchase_SelectForPrint]
            dpe = GetEntity();

            DataTable dt = stbl.D_Purchase_SelectForPrint(dpe);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                //bbl.ShowMessage("E133");
                //previousCtrl.Focus();
                stbl.ShowMessage("E128");
                ckM_TextBox1.Focus();
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
                    if (row["DisplayRows"].ToString() == "1")
                    {
                        bool ret = SelectAndInsertExclusive(row["PurchaseNO"].ToString());
                        if (!ret)
                            return null;

                        i++;
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["no"] = row["PurchaseNO"].ToString();
                        dtForUpdate.Rows.Add(rowForUpdate);
                    }
                }
            }
            return dt;
        }
        private bool SelectAndInsertExclusive(string No)
        {
            //DeleteExclusive();

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Shiire,
                Number = No,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(), dt.Rows[0]["Operator"].ToString());
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            if (dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity();
                    de.DataKBN = (int)Exclusive_BL.DataKbn.Shiire;
                    de.Number = dr["no"].ToString();

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }
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

                //Ｑ２０５				
                if (bbl.ShowMessage("Q205") != DialogResult.Yes)
                return;

                //EXCEL出力
                string filePath = "";
                //出力ファイル名の決定
                if (!ShowSaveFileDialog(InProgramNM, out filePath, 2)) // Flag 2 is for xls modified in Based by PTK 2020/11/05
                {
                    return;
                }

                // 実行モジュールと同一フォルダのファイルを取得
                Login_BL loginbl = new Login_BL();
                string templateFilePath = loginbl.GetInformationOfIniFileByKey(InProgramID);

                //テンプレートのファイル存在チェック
                if (!System.IO.File.Exists(templateFilePath))
                {
                    //Todo:変更
                    //bbl.ShowMessage("E122"); // 2020/1/05
                    bbl.ShowMessage("Please put Shiiretanka Format Sheet named \"ShiireTankaTeiseiIraisho.xls\" in Path C:\\Csv\\");
                    return;
                }

                //テンプレートをコピーしてファイルを保存(上書きOK)
                System.IO.File.Copy(@templateFilePath, @filePath, true);

                //Excel出力
                OutputExecelFromDataTable(table, filePath);

                //更新処理
                //tableの見積番号だけ   
                try
                {
                    stbl.D_Purchase_Update(dpe, dtForUpdate);
                }
                catch (Exception ex)
                {

                    var f = ex.Message;
                }

                //ログファイルへの更新
                bbl.L_Log_Insert(Get_L_Log_Entity());

                //ファイル出力が完了しました。
                bbl.ShowMessage("I203");
            }
            finally
            {
                DeleteExclusive(dtForUpdate);
            }
            //更新後画面そのまま
            detailControls[1].Focus();
        }

        private void OutputExecelFromDataTable(DataTable dt, string EXCEL_SAVE_PATH)
        {
            if (dt.Rows.Count > 0)
            {// Excelを参照設定する必要があります
             // [参照の追加],[COM],[Microsoft Excel *.* Object Library]
             // Imports Microsoft.Office.Interop (必要)
             // Imports System.Runtime.InteropServices (必要)

                //Excel出力
                // EXCEL関連オブジェクトの定義

                // Excelを起動する
                Microsoft.Office.Interop.Excel.Application objExcel = new Microsoft.Office.Interop.Excel.Application();
                objExcel.Visible = false;
                try
                {
                    //待機状態
                    Cursor.Current = Cursors.WaitCursor;

                    // ブック（ファイル）を開く
                    Microsoft.Office.Interop.Excel.Workbook objWorkBook = objExcel.Workbooks.Open(EXCEL_SAVE_PATH);
                    try
                    {
                        // 現在日時を取得
                        string timestanpText = DateTime.Now.ToString(" yyyy/MM/dd HH:mm");    //bbl.GetDate();

                        int count = 0;
                        int currentRow = (int)ERow.DetailStartRow;
                        int page = 1;
                        string breakKey = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            if (breakKey != "" && breakKey != row["VendorCD"].ToString())
                            {
                                count = DATA_COUNT;
                            }

                            if (count >= DATA_COUNT)
                            {
                                //行１からGYO_COUNTまでの行をこぴーする
                                objWorkBook.Sheets[1].Range("1:" + GYO_COUNT).Copy(objWorkBook.Sheets[1].Range("A" + (GYO_COUNT * page + 1)));

                                //Detail部をクリアする
                                currentRow = (int)ERow.DetailStartRow + page * GYO_COUNT;

                                objWorkBook.Sheets[1].Range("A" + currentRow + ":BZ" + (currentRow + DATA_COUNT - 1)).ClearContents();
                                page++;
                                count = 0;
                            }

                            if (count == 0)
                            {
                                breakKey = row["VendorCD"].ToString();

                                if (page == 1)
                                {
                                    objWorkBook.Sheets[1].Cells[2, 64] = timestanpText;
                                    objWorkBook.Sheets[1].Cells[(int)ERow.Print1, (int)ECell.Print1] = row["Print1"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.Print2, (int)ECell.Print2] = row["Print2"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.Print3, (int)ECell.Print3] = row["Print3"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.Print4, (int)ECell.Print4] = row["Print4"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.ZIP, (int)ECell.ZIP] = row["ZIP"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.CompanyName, (int)ECell.CompanyName] = row["CompanyName"].ToString();
                                    objWorkBook.Sheets[1].Cells[(int)ERow.TEL, (int)ECell.TEL] = row["TEL"].ToString();
                                }
                                objWorkBook.Sheets[1].Cells[(int)ERow.VendorName + (page - 1) * GYO_COUNT, (int)ECell.VendorName] = row["VendorName"].ToString();
                                objWorkBook.Sheets[1].Cells[(int)ERow.OrderPerson + (page - 1) * GYO_COUNT, (int)ECell.OrderPerson] = row["OrderPerson"].ToString();
                            }

                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.PurchaseDate] = row["PurchaseDate"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.VendorDeliveryNo] = row["VendorDeliveryNo"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.MakerItem] = row["MakerItem"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.ColorName] = row["ColorName"].ToString() + Environment.NewLine + row["SizeName"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.PurchaserUnitPrice] = row["PurchaserUnitPrice"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.OrderUnitPrice] = row["OrderUnitPrice"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.PurchaseSu] = row["PurchaseSu"].ToString();

                            currentRow++;
                            count++;
                        }

                        //// エクセル表示
                        //objExcel.Visible = true;
                        objWorkBook.Save();

                        // クローズ
                        objWorkBook.Close(false);
                        objExcel.Quit();
                    }
                    finally
                    {
                        if (objWorkBook != null)
                        {
                            try
                            {
                                //objWorkBook.Close();
                            }
                            finally
                            {
                                // EXCEL解放
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(objWorkBook);
                                objWorkBook = null/* TODO Change to default(_) if this is not a reference type */;
                            }
                        }
                    }
                }
                catch (Exception ex){
                    var msg = ex.Message;
                }
                finally
                {
                    if (objExcel != null)
                    {
                        try
                        {
                            //objExcel.Quit();
                        }
                        finally
                        {
                            // EXCEL解放
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(objExcel);
                            objExcel = null/* TODO Change to default(_) if this is not a reference type */;
                        }
                    }

                    //元に戻す
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            //画面指定項目をカンマ編集で羅列（ex."2019/07/01,2019/7/31,ABCDEFG,未出力"）
            string item = detailControls[0].Text;
            for (int i = 1; i <= (int)EIndex.OrderCD; i++)
            {
                item += "," + detailControls[i].Text;
            }
            if (ChkMisyutsuryoku.Checked)
            {
                item += "," + ChkMisyutsuryoku.Text;
            }
            if (ChkSyutsuryokuZumi.Checked)
            {
                item += "," + ChkSyutsuryokuZumi.Text;
            }

            L_Log_Entity lle = new L_Log_Entity
            {
                InsertOperator = this.InOperatorCD,
                PC = this.InPcID,
                Program = this.InProgramID,
                OperateMode = "",
                KeyItem = item
            };

            return lle;
        }
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
                case (int)EIndex.ArrivalPlanDateFrom:
                case (int)EIndex.ArrivalPlanDateTo:
                    if (index == (int)EIndex.ArrivalPlanDateFrom)
                    {
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                            return true;
                    }
                    else
                    {
                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                    }

                    string strYmd = "";
                    strYmd = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(strYmd))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    detailControls[index].Text = strYmd;

                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.ArrivalPlanDateTo)
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
                        ScOrder.ChangeDate = strYmd;
                    }
                    break;

                case (int)EIndex.OrderCD:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //情報ALLクリア
                        ClearCustomerInfo();
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
                    ret = sbl.M_Vendor_SelectTop1(mve);

                    if (ret)
                    {
                        if (mve.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            //顧客情報ALLクリア
                            ClearCustomerInfo();
                            return false;
                        }
                        ScOrder.LabelText = mve.VendorName;
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { CboStoreCD }))
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
                            CboStoreCD.Focus();
                            return false;
                        }

                    }

                    break;
            }

            return true;
        }
        /// <summary>
        /// 仕入先情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            ScOrder.LabelText = "";
        }
        private D_Purchase_Entity GetEntity()
        {
            dpe = new D_Purchase_Entity
            {
                PurchaseDateFrom = detailControls[(int)EIndex.ArrivalPlanDateFrom].Text,
                PurchaseDateTo = detailControls[(int)EIndex.ArrivalPlanDateTo].Text,

                StoreCD = CboStoreCD.SelectedValue.ToString(),
                VendorCD = detailControls[(int)EIndex.OrderCD].Text,
            };

            if (ChkMisyutsuryoku.Checked)
                dpe.ChkMisyutsuryoku = 1;
            else
                dpe.ChkMisyutsuryoku = 0;

            if (ChkSyutsuryokuZumi.Checked)
                dpe.ChkSyutsuryokuZumi = 1;
            else
                dpe.ChkSyutsuryokuZumi = 0;

            dpe.InsertOperator = InOperatorCD;
            dpe.PC = InPcID;

            return dpe;
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
            string ymd = stbl.GetDate();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = stbl.GetDate()
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                CboStoreCD.SelectedValue = mse.StoreCD;
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

            detailControls[(int)EIndex.ArrivalPlanDateTo].Text = ymd;
            ChkMisyutsuryoku.Checked = true;
            detailControls[1].Focus();
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
                            ScOrder.BtnSearch.Enabled = Kbn == 0 ? true : false;

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
                            if (index == (int)EIndex.OrderCD)
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

        private void BtnSubF11_Click(object sender, EventArgs e)
        {
            //表示ボタンClick時   
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

        private void DetailControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;

                int index = Array.IndexOf(detailControls, sender);
                switch (index)
                {
                    case (int)EIndex.OrderCD:
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

        #endregion

    }
}








