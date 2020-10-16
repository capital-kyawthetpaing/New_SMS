﻿using System;
using System.Data;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;

namespace TanaoroshiNyuuryoku
{
    /// <summary>
    /// TanaoroshiNyuuryoku 棚卸入力
    /// </summary>
    internal partial class TanaoroshiNyuuryoku : FrmMainForm
    {
        private const string ProID = "TanaoroshiNyuuryoku";
        private const string ProNm = "棚卸入力";
        private const short mc_L_END = 3; // ロック用

        private enum EIndex : int
        {
            SoukoCD,
            InventoryDate,
            RackNO,  //棚番
            JANCD,
            Suryo   //在庫数量
        }

        /// <summary>
        /// 検索の種類
        /// </summary>
        private enum EsearchKbn : short
        {
            Null,
            Product
        }
        private enum EColNo : int
        {
            RackNO,
            JanCD,
            SKUCD,
            SKUName,
            ColorName,
            SizeName,
            TheoreticalQuantity,
            ActualQuantity,
            DifferenceQuantity,
            AdminNO,

            COUNT
        }

        private Control[] detailControls;
       
        private Tanaoroshi_BL tabl;
        private D_Inventory_Entity doe;
        private string mAdminNO;

        public TanaoroshiNyuuryoku()
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
                ClearLabel();

                //起動時共通処理
                base.StartProgram();
                Btn_F9.Text = "";
                Btn_F9.Enabled = false;
                Btn_F12.Text = "登録(F12)";
                SetFuncKeyAll(this, "100001000011");

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                tabl = new Tanaoroshi_BL();

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
            detailControls = new Control[] { CboSoukoCD, ckM_TextBox1, ScFromRackNo.TxtCode, SC_ITEM_0.TxtCode, ckM_TextBox8 };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
            SC_ITEM_0.BtnSearch.Click += new System.EventHandler(BtnSearch_Click);
        }

        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {
            string ymd = bbl.GetDate();

            switch (index)
            {
                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    else
                    {
                        //if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        //{
                        //    bbl.ShowMessage("E141");
                        //    CboStoreCD.Focus();
                        //    return false;
                        //}
                    }

                    break;
                case (int)EIndex.InventoryDate:
                    //入力必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }

                    detailControls[index].Text = bbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    break;

                case (int)EIndex.RackNO:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        return true;
                    }

                    //倉庫棚番マスタに存在しない場合、Error
                    M_Location_Entity mle = new M_Location_Entity
                    {
                        SoukoCD = CboSoukoCD.SelectedIndex > 0 ? CboSoukoCD.SelectedValue.ToString() : "",
                        TanaCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };
                    bool ret = tabl.M_Location_SelectData(mle);
                    if (!ret)
                    {
                        bbl.ShowMessage("E101");
                        return false;
                    }
                    break;

                case (int)EIndex.Suryo:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        detailControls[(int)EIndex.RackNO].Text = "";
                        detailControls[(int)EIndex.JANCD].Text = "";
                        ClearLabel();
                        return true;
                    }
                    if (!RequireCheck(new Control[] { detailControls[(int)EIndex.RackNO] }))
                    {
                        return false;
                    }
                    if (!RequireCheck(new Control[] { detailControls[(int)EIndex.JANCD] }))
                    {
                        return false;
                    }
                    //入力された場合でエラーがないとき
                    //Footerの「棚番」「JANCD」の組み合わせが既に明細内に存在するときエラー
                    foreach (DataRow rw in ((DataTable)GvDetail.DataSource).Rows)
                    {
                        if (detailControls[(int)EIndex.RackNO].Text.Equals(rw["RackNO"].ToString()) && detailControls[(int)EIndex.JANCD].Text.Equals(rw["JANCD"].ToString()))
                        {
                            //Ｅ１９３
                            bbl.ShowMessage("E265");
                            return false;
                        }
                    }
                        break;

                case (int)EIndex.JANCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ClearLabel();
                        return true;
                    }

                    //入力がある場合、SKUマスターに存在すること
                    //[M_SKU]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        JanCD = detailControls[index].Text,
                        SetKBN = "0",
                        ChangeDate = ymd
                    };

                    SKU_BL mbl = new SKU_BL();
                    DataTable dt = mbl.M_SKU_SelectAll(mse);
                    DataRow selectRow = null;

                    if (dt.Rows.Count == 0)
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        ClearLabel();
                        return false;
                    }
                    else
                    {
                        selectRow = dt.Rows[0];
                    }

                    if (selectRow != null)
                    {
                        DataRow row = ((DataTable)GvDetail.DataSource).NewRow();
                       mAdminNO = selectRow["AdminNO"].ToString();
                        lblSKUCD.Text = selectRow["SKUCD"].ToString();
                        lblSKUName.Text = selectRow["SKUName"].ToString();
                        lblColorName.Text = selectRow["ColorName"].ToString();
                        lblSizeName.Text = selectRow["SizeName"].ToString();

                    }
                    break;

            }
            return true;

        }
        private void ClearLabel()
        {
            mAdminNO = "";
            lblSKUCD.Text = "";
            lblSKUName.Text = "";
            lblColorName.Text = "";
            lblSizeName.Text = "";
        }
        protected override void ExecDisp()
        {

            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            
            //更新処理
            doe = GetEntity();
            DataTable dt = tabl.D_Inventory_SelectAll(doe);

            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
                GvDetail.CurrentCell = GvDetail[(int)EColNo.ActualQuantity, 0];
                GvDetail.ReadOnly = false;
                for(int i=0; i< (int)EColNo.COUNT; i++)
                {
                    if(i == (int)EColNo.ActualQuantity)
                    {
                        GvDetail.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        GvDetail.Columns[i].ReadOnly = true;
                        //GvDetail.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.Silver;

                        //デフォルトのセルスタイル
                        DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle();
                        defaultCellStyle.Font = new System.Drawing.Font(GvDetail.Font,GvDetail.Font.Style | System.Drawing.FontStyle.Bold);
                        defaultCellStyle.BackColor = System.Drawing.Color.Silver;
                        GvDetail.Columns[i].DefaultCellStyle = defaultCellStyle;
                    }
                }
                for (int i = 0; i <= (int)EIndex.Suryo; i++)
                {
                    switch (i)
                    {
                        case (int)EIndex.SoukoCD:
                        case (int)EIndex.InventoryDate:
                            detailControls[i].Enabled = false;
                            break;
                        default:
                            detailControls[i].Enabled = true;
                            break;
                    }
                }
                ScFromRackNo.Value1 = CboSoukoCD.SelectedValue.ToString();
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
                DataTable dt = GetGridEntity();

                doe.SoukoCD = CboSoukoCD.SelectedValue.ToString();
                doe.InventoryDate = detailControls[(int)EIndex.InventoryDate].Text;
                doe.Operator = InOperatorCD;
                doe.PC = InPcID;

                //更新処理
                tabl.D_Inventory_Update(doe, dt);

                bbl.ShowMessage("I101");

                Scr_Clr(0);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
  
        /// <summary>
        /// 検索フォーム起動処理
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="setCtl"></param>
        private void SearchData(EsearchKbn kbn, Control setCtl)
        {
            switch (kbn)
            {
                case EsearchKbn.Product:
                    using (Search_Product frmProduct = new Search_Product(detailControls[(int)EIndex.InventoryDate].Text))
                    {
                            frmProduct.SKUCD = lblSKUCD.Text;
                            frmProduct.JANCD = detailControls[(int)EIndex.JANCD].Text;

                        frmProduct.ShowDialog();

                        if (!frmProduct.flgCancel)
                        {
                            ((Search.CKM_SearchControl)setCtl).TxtCode.Text = frmProduct.JANCD;

                            detailControls[(int)EIndex.JANCD].Text = frmProduct.JANCD;
                            lblSKUCD.Text = frmProduct.SKUCD;
                            mAdminNO = frmProduct.AdminNO;

                            setCtl.Focus();

                            //CheckDetail((int)EIndex.JANCD, true);
                            SendKeys.Send("{ENTER}");
                        }
                    }
                    break;
            }

        }
        private DataTable GetGridEntity()
        {
            DataTable dt = new DataTable();
            Para_Add(dt);
            int rowNo = 0;

            DataTable ds = (DataTable)GvDetail.DataSource;
            foreach (DataRow row in ds.Rows)
            {
                rowNo++;

                dt.Rows.Add(rowNo
                 , row["RackNO"].ToString()
                 , bbl.Z_Set(row["AdminNO"])
                 , bbl.Z_Set(row["ActualQuantity"])
                 , 0    //mGrid.g_DArray[RW].Update
                 );

            }
            return dt;
        }

        // -----------------------------------------------------------
        // パラメータ設定
        // -----------------------------------------------------------
        private void Para_Add(DataTable dt)
        {
            dt.Columns.Add("GyoNO", typeof(int));
            dt.Columns.Add("RackNO", typeof(string));
            dt.Columns.Add("AdminNO", typeof(int));
            dt.Columns.Add("ActualQuantity", typeof(int));
          
            dt.Columns.Add("UpdateFlg", typeof(int));
        }
        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Inventory_Entity GetEntity()
        {
            doe = new D_Inventory_Entity();
            doe.SoukoCD = CboSoukoCD.SelectedValue.ToString();
            doe.InventoryDate = detailControls[(int)EIndex.InventoryDate].Text;

            return doe;
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
                    ctl.Enabled = true;

                    if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_CheckBox)))
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                    else
                    {
                        ctl.Text = "";
                    }
                }

            }
            ClearLabel();

            GvDetail.DataSource = null;
            GvDetail.Enabled = false;

            if (CboSoukoCD.Items.Count > 1)
                CboSoukoCD.SelectedIndex = 1;
            string ymd = bbl.GetDate();
            detailControls[(int)EIndex.InventoryDate].Text = ymd;
            detailControls[(int)EIndex.SoukoCD].Focus();

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
                    if (PreviousCtrl.Name.Equals(SC_ITEM_0.Name))
                    {
                        //商品検索
                        SearchData(EsearchKbn.Product, PreviousCtrl);
                    }

                    break;

                case 11:    //F12:登録
                    {

                        //Ｑ１０１		
                        if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                            return;

                        this.ExecSec();
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
                        if (index == (int)EIndex.InventoryDate)
                        {
                           BtnSubF11.Focus();
                        }
                        else if (index == (int)EIndex.Suryo)
                        {
                            btnAdd.Focus();
                        }
                        else
                            detailControls[index + 1].Focus();

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
                PreviousCtrl = this.ActiveControl;
            }

            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnF11_Click(object sender, EventArgs e)
        {
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = ((DataTable)GvDetail.DataSource).NewRow();
                row["AdminNO"] = mAdminNO;
                row["JANCD"] = detailControls[(int)EIndex.JANCD].Text;
                row["SKUCD"] = lblSKUCD.Text;
                row["SKUName"] = lblSKUName.Text;
                row["ColorName"] = lblColorName.Text;
                row["SizeName"] = lblSizeName.Text;
                row["ActualQuantity"] = bbl.Z_SetStr( detailControls[(int)EIndex.Suryo].Text);
                row["RackNO"] = detailControls[(int)EIndex.RackNO].Text;
                ((DataTable)GvDetail.DataSource).Rows.Add(row);

                detailControls[(int)EIndex.JANCD].Text = "";
                detailControls[(int)EIndex.Suryo].Text = "";
                detailControls[(int)EIndex.RackNO].Text = "";
                ClearLabel();

                detailControls[(int)EIndex.RackNO].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
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
                if (((Search.CKM_SearchControl)sc).Name.Substring(0, 3).Equals("SC_"))
                {
                    //商品検索
                    kbn = EsearchKbn.Product;
                }

                setCtl = PreviousCtrl;


                if (kbn != EsearchKbn.Null)
                    SearchData(kbn, setCtl);

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void GvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = (DataTable)GvDetail.DataSource;
            if (e.ColumnIndex == GvDetail.Columns["colActualQuantity"].Index)
            {
                //Form.Detail.実在庫 －	Form.Detail.理論在庫 →	Form.Detail.差 にセット
                dt.Rows[e.RowIndex]["DifferenceQuantity"] = bbl.Z_Set(dt.Rows[e.RowIndex]["ActualQuantity"]) - bbl.Z_Set(dt.Rows[e.RowIndex]["TheoreticalQuantity"]);
            }
        }
        #endregion

    }
}








