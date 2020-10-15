using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace TanaoroshiSaihyou
{
    /// <summary>
    /// TanaoroshiSaihyou 棚卸差異表
    /// </summary>
    internal partial class TanaoroshiSaihyou : FrmMainForm
    {
        private const string ProID = "TanaoroshiSaihyou";
        private const string ProNm = "棚卸差異表";

        private enum EIndex : int
        {
            SoukoCD,
            InventoryDate,
            Rdo1,
            Rdo2,

        }
        private const int GYO_COUNT = 27;   //1ページのEXCEL行数
        private const int DATA_COUNT = 11;  //1ページのデータの行数
        private enum ECell : int
        {
            ProgramName = 1,

            RackNO = 1,
            SKUCD ,
            JANCD,
            SKUName,
            ColorName,
            SizeName,
            BrandName,

            TheoreticalQuantity,
            ActualQuantity,
            DifferenceQuantity
        }

        private enum ERow : int
        {
            ProgramName = 1,
            InventoryDate=2,
            Souko = 3,
            Title = 4,

            DetailStartRow = 5,

        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private Tanaoroshi_BL tabl;
        private D_Inventory_Entity die;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避


        public TanaoroshiSaihyou()
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
                base.Btn_F9.Text = "";
                base.Btn_F9.Enabled = false;
                base.Btn_F10.Text = "";
                base.Btn_F11.Text = "";
                base.Btn_F12.Text = "出力(F12)";

                //起動時共通処理
                base.StartProgram();    

                tabl = new Tanaoroshi_BL();
                string ymd = tabl.GetDate();

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);

                CboSoukoCD.Bind(ymd, InOperatorCD);

                SetFuncKeyAll(this, "100001000001");
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
            detailControls = new Control[] { CboSoukoCD, ckM_TextBox1 ,ckM_RadioButton1, ckM_RadioButton2
                         };
            detailLabels = new Control[] { };
            searchButtons = new Control[] { };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }       

        /// <summary>
        /// 棚卸データ取得処理
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

            //[D_Inventory_SelectForPrint]
            die = GetEntity();

            DataTable dt = tabl.D_Inventory_SelectForPrint(die);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
                previousCtrl.Focus();
                return null;
            }
            //else
            //{
            //    //明細にデータをセット
            //    int i = 0;
            //    dtForUpdate = new DataTable();
            //    dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (row["DisplayRows"].ToString() == "1" )
            //        {
            //            bool ret = SelectAndInsertExclusive(row["PurchaseNO"].ToString());
            //            if (!ret)
            //                return null;

            //            i++;
            //            // データを追加
            //            DataRow rowForUpdate;
            //            rowForUpdate = dtForUpdate.NewRow();
            //            rowForUpdate["no"] = row["PurchaseNO"].ToString();
            //            dtForUpdate.Rows.Add(rowForUpdate);
            //        }
            //    }
            //}
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
                if (!ShowSaveFileDialog(InProgramNM, out filePath, 1))
                {
                    return;
                }

                // 実行モジュールと同一フォルダのファイルを取得
                //Login_BL loginbl = new Login_BL();
                //string templateFilePath = loginbl.GetInformationOfIniFileByKey(InProgramID);

                ////テンプレートのファイル存在チェック
                //if (!System.IO.File.Exists(templateFilePath))
                //{
                //    //Todo:変更
                //    bbl.ShowMessage("E122");
                //    return;
                ////}

                ////テンプレートをコピーしてファイルを保存(上書きOK)
                //System.IO.File.Copy(@templateFilePath, @filePath, true);

                //Excel出力
                OutputExecelFromDataTable(table, filePath);

                ////更新処理
                ////tableの見積番号だけ            
                //tabl.D_Inventory_Update(dpe, dtForUpdate);

                //ログファイルへの更新
                bbl.L_Log_Insert(Get_L_Log_Entity());

                //ファイル出力が完了しました。
                bbl.ShowMessage("I203");
            }
            finally
            {
                //DeleteExclusive(dtForUpdate);
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
                    Microsoft.Office.Interop.Excel.Workbook objWorkBook = objExcel.Workbooks.Add();
                    try
                    {
                        // 現在日時を取得
                        //string timestanpText = DateTime.Now.ToString(" yyyy/MM/dd HH:mm");    //bbl.GetDate();

                        int count = 0;
                        int currentRow = (int)ERow.DetailStartRow;
                        int page = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (count == 0 )
                            {
                                objWorkBook.Sheets[1].Cells[(int)ERow.ProgramName + (page - 1) * GYO_COUNT, (int)ECell.ProgramName] = ProNm;
                                objWorkBook.Sheets[1].Cells[(int)ERow.InventoryDate + (page - 1) * GYO_COUNT, (int)ECell.ProgramName] = "棚卸日：";
                                objWorkBook.Sheets[1].Cells[(int)ERow.InventoryDate + (page - 1) * GYO_COUNT, (int)ECell.ProgramName + 1] = detailControls[(int)EIndex.InventoryDate].Text;
                                objWorkBook.Sheets[1].Cells[(int)ERow.Souko + (page - 1) * GYO_COUNT, (int)ECell.ProgramName] = "倉庫：";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Souko + (page - 1) * GYO_COUNT, (int)ECell.ProgramName+1] = CboSoukoCD.Text;
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.RackNO] = "棚番";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.SKUCD] = "SKUCD";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.JANCD] = "JANCD";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.SKUName] = "商品名";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.ColorName] = "カラー";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.SizeName] = "サイズ";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.BrandName] = "ブランド";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.TheoreticalQuantity] = "理論在庫";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.ActualQuantity] = "在庫数";
                                objWorkBook.Sheets[1].Cells[(int)ERow.Title + (page - 1) * GYO_COUNT, (int)ECell.DifferenceQuantity] = "差異";

                                objWorkBook.Sheets[1].Columns[(int)ECell.SKUCD].NumberFormat = "@";
                                objWorkBook.Sheets[1].Columns[(int)ECell.JANCD].NumberFormat = "@";
                            }

                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.RackNO] = row["RackNO"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.SKUCD] = row["SKUCD"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.JANCD] = row["JANCD"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.SKUName] = row["SKUName"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.ColorName] = row["ColorName"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.SizeName] = row["SizeName"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.BrandName] = row["BrandName"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.TheoreticalQuantity] = row["TheoreticalQuantity"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.ActualQuantity] = row["ActualQuantity"].ToString();
                            objWorkBook.Sheets[1].Cells[currentRow, (int)ECell.DifferenceQuantity] = row["DifferenceQuantity"].ToString();

                            currentRow++;
                            count++;
                        }

                        //// エクセル表示
                        //objExcel.Visible = true;
                        objWorkBook.SaveAs(EXCEL_SAVE_PATH);

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
            for (int i = 1; i <= (int)EIndex.InventoryDate; i++)
            {
                item += "," + detailControls[i].Text;
            }
            if (ckM_RadioButton1.Checked)
            {
                item += "," + ckM_RadioButton1.Text;
            }
            if (ckM_RadioButton2.Checked)
            {
                item += "," + ckM_RadioButton2.Text;
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
        private bool CheckDetail(int index, bool set=true)
        {
            switch (index)
            {
                case (int)EIndex.InventoryDate:

                        //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                        if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
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

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboSoukoCD.MoveNext = false;
                        return false;
                    }

                    break;
            }

            return true;
        }

        private D_Inventory_Entity GetEntity()
        {
            die = new D_Inventory_Entity
            {
                InventoryDate = detailControls[(int)EIndex.InventoryDate].Text,
                SoukoCD = CboSoukoCD.SelectedValue.ToString(),
            };

            if (ckM_RadioButton1.Checked)
                die.ChkSaiOnly = 1;
            else
                die.ChkSaiOnly = 0;

            die.KbnSai = 1;
            die.InsertOperator = InOperatorCD;
            die.PC = InPcID;

            return die;
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
                else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_RadioButton)))
                {
                    ckM_RadioButton1.Checked = true;
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
            if (CboSoukoCD.Items.Count > 1)
                CboSoukoCD.SelectedIndex = 1;

            string ymd = tabl.GetDate();

            detailControls[(int)EIndex.InventoryDate].Text = ymd;
         detailControls[0].Focus();
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
                            if(index== (int)EIndex.Rdo1 || index == (int)EIndex.Rdo2)
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








