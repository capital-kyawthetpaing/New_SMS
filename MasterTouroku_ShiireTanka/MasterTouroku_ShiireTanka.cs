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
using System.Linq;
using System.IO;
//using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


namespace MasterTouroku_ShiireTanka
{
    /// <summary>
    /// MasterTouroku_ShiireTanka 仕入先別発注単価
    /// </summary>
    internal partial class MasterTouroku_ShiireTanka : FrmMainForm
    {
        private const string ProID = "MasterTouroku_ShiireTanka";
        private const string ProNm = "仕入先別発注単価";
        private const short mc_L_END = 3; // ロック用

        private enum EOpeMode : short
        {
            ITEM,
            SKU
        }
        private enum KIndex : int
        {
            VendorCD,
            BaseDate,
            StoreCD                
        }

        private enum SIndex : int
        {
            BrandCD,
            SportsCD,
            SegmentCD,
            YearTerm,
            Season,
            ChangeDate,
            MakerItem
        }

        private enum AIndex : int
        {
            ItemCD,
            ChangeDateAdd,
            Rate,
            PriceWithoutTax
        }

        private enum CIndex : int
        {
            CopyDate,
            CopyRate
        }

        private enum RBIndex : int
        {
            RdoCurrent,
            RdoHistory,
            RdoAllStores,
            RdoIndividualStore,
            RdoITEM,
            RdoSKU
        }

        private enum IColNo : int
        {
            Check,
            BrandName,
            SportsName,
            SegmentName,
            LastYearTerm,
            LastSeason,
            MakerItem,
            ItemCD,
            ItemName,
            ChangeDate,
            PriceOutTax,
            Rate,
            PriceWithoutTax,

            COUNT
        }

        private enum JColNo : int
        {
            Check,
            MakerItem,
            ItemCD,
            ItemName,
            SizeName,
            ColorName,
            SKUCD,
            ChangeDate,
            PriceOutTax,
            Rate,
            PriceWithoutTax,

            COUNT
        }

        private enum EColNo : int
        {
            VendorCD,
            StoreCD,
            ItemCD,
            ChangeDate,
            Rate,
            PriceWithoutTax,

            COUNT
        }

        private Control[] keyControls;
        private Control[] keyLabels;
        private Control[] selectControls1;
        private Control[] selectLabels1;
        private Control[] addControls;
        private Control[] addLabels;
        private Control[] selectControls2;
        private Control[] selectLabels2;
        private Control[] copyControls;

        private Control[] searchButtons;
        private Control[] buttons;
        private Control[] radioButtons;

        private MasterTouroku_ShiireTanka_BL msbl;
        private M_ItemOrderPrice_Entity mpe;
        private M_ITEM_Entity mie;
        private DataTable dtITEM;
        private DataTable dtSKU;
        private DataTable dtOldITEM;
        private DataTable dtOldSKU;

        private EOpeMode mOpeMode;
        private decimal mOldRate;
        private string mOldItemCd;
        private string mOldBaseDate;
        private DateTime mUpdateDateTime;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        
        public MasterTouroku_ShiireTanka()
        {
            InitializeComponent();
            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InProgramID = ProID;
                InProgramNM = ProNm;

                this.SetFunctionLabel(EProMode.MENTE);
                this.InitialControlArray();
                this.SetLabelWidth();
                mOpeMode = EOpeMode.ITEM;
                ModeText = "ITEM";
                ModeColor = Color.FromArgb(255, 192, 0);

                //起動時共通処理
                base.StartProgram();
                F2Visible = false;
                F3Visible = false;
                F4Visible = false;
                F5Visible = false;
                Btn_F10.Text = "取込(F10)";
                SetFuncKeyAll(this, "100001001010");

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                msbl = new MasterTouroku_ShiireTanka_BL();
                cboStoreCD.Bind(ymd);
                cboYearTerm1.Bind(ymd);
                cboSeason1.Bind(ymd);
                cboYearTerm2.Bind(ymd);
                cboSeason2.Bind(ymd);
                
                //検索用のパラメータ設定
                string stores = GetAllAvailableStores();
                scVendor.Value1 = "1";
                scSportsCD1.Value1 = "202";
                scSportsCD2.Value1 = "202";
                scSegmentCD1.Value1 = "203";
                scSegmentCD2.Value1 = "203";

                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = msbl.GetDate()
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);
                if (ret)
                {
                    cboStoreCD.SelectedValue = mse.StoreCD;
                }

                this.Scr_Clr(0);

                //明細部を入力不可とする
                this.Scr_Lock(1);

                keyControls[(int)KIndex.VendorCD].Focus();

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
            keyControls = new Control[] {scVendor.TxtCode, txtBaseDate, cboStoreCD };
            keyLabels = new Control[] { scVendor};

            selectControls1 = new Control[] { scBrandCD1.TxtCode ,scSportsCD1.TxtCode ,scSegmentCD1.TxtCode , cboYearTerm1
                    , cboSeason1, txtChangeDate1, scMakerItem1.TxtCode };
            selectLabels1 = new Control[] { scBrandCD1, scSportsCD1, scSegmentCD1 };

            selectControls2 = new Control[] { scBrandCD2.TxtCode ,scSportsCD2.TxtCode ,scSegmentCD2.TxtCode , cboYearTerm2
                    , cboSeason2, txtChangeDate2, scMakerItem2.TxtCode };
            selectLabels2 = new Control[] { scBrandCD2, scSportsCD2, scSegmentCD2 };

            addControls = new Control[] { scItem.TxtCode, txtChangeDateAdd, txtRate, txtOrderPrice};
            addLabels = new Control[] { scItem, lblPriceOutTax };

            copyControls = new Control[] { txtCopyDate, txtCopyRate };

            radioButtons = new Control[] { rdoCurrent, rdoHistory, rdoAllStores, rdoIndividualStore, rdoITEM, rdoSKU };
            searchButtons = new Control[] { scBrandCD1.BtnSearch, scSportsCD1.BtnSearch, scSegmentCD1.BtnSearch , scMakerItem1.BtnSearch
                                        , scItem.BtnSearch, scBrandCD2.BtnSearch, scSportsCD2.BtnSearch, scSegmentCD2.BtnSearch};
            buttons = new Control[] { btnInput, btnSearch, btnAdd, btnSelect, btnAllSelect, btnAllRelaese, btnCopy, btnUpdate, btnDelete };


            //イベント付与
            foreach (Control ctl in keyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyControl_KeyDown);
                ctl.Enter += new System.EventHandler(KeyControl_Enter);
            }

            foreach (Control ctl in selectControls1)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectControl1_KeyDown);
                ctl.Enter += new System.EventHandler(SelectControl1_Enter);
            }

            foreach (Control ctl in selectControls2)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectControl2_KeyDown);
                ctl.Enter += new System.EventHandler(SelectControl2_Enter);
            }

            foreach (Control ctl in addControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(AddControl_KeyDown);
                ctl.Enter += new System.EventHandler(AddControl_Enter);
            }

            foreach (Control ctl in copyControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(CopyControl_KeyDown);
                ctl.Enter += new System.EventHandler(CopyControl_Enter);
            }

            foreach (CKM_Controls.CKM_RadioButton ctl in radioButtons)
            {
                ctl.CheckedChanged += new System.EventHandler(RadioButton_CheckedChanged);
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(RadioButton_KeyDown);
            }
        }

        /// <summary>
        /// 検索有項目のラベル幅変更
        /// </summary>
        private void SetLabelWidth()
        {
            scBrandCD1.SetLabelWidth(180);
            scBrandCD2.SetLabelWidth(180);
            scSportsCD1.SetLabelWidth(180);
            scSportsCD2.SetLabelWidth(180);
            scSegmentCD1.SetLabelWidth(180);
            scSegmentCD2.SetLabelWidth(180);
        }

        /// <summary>
        /// データ取得処理
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            mpe = GetEntity();

            dtITEM = msbl.M_ItemOrderPrice_SelectFromItem(mpe);
            dtSKU = msbl.M_ItemOrderPrice_SelectFromSKU(mpe);

            //削除用に読込時のテーブル退避
            dtOldITEM = dtITEM.Copy();
            dtOldSKU = dtSKU.Copy();

            GvItem.DataSource = dtITEM;
            GvSku.DataSource = dtSKU;

            if (GvItem.Rows.Count > 0)
            {
                GvItem.SelectionMode = DataGridViewSelectionMode.CellSelect;
                GvItem.CurrentRow.Selected = true;
                GvItem.Enabled = true;
                //GvItem.Focus();
                //GvItem.CurrentCell = GvItem[(int)IColNo.Check, 0];
                GvItem.ReadOnly = false;

                //Gridの背景色変更
                this.Set_GridStyle();
            }

            GvItem.Visible = rdoITEM.Checked;
            GvSku.Visible = rdoSKU.Checked;

            //更新日付
            mUpdateDateTime = DateTime.Now;

            return true;
        }

        /// <summary>
        /// 一時テーブルからの明細表示処理
        /// </summary>
        private void DispFromTempTable()
        {
            DataTable dt = new DataTable();

            if (mOpeMode == EOpeMode.ITEM)
            {
                GvItem.CurrentCell = null;

                foreach (DataRow drITEM in dtITEM.Rows)
                {
                    if ((string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.BrandCD].Text) || drITEM["BrandCD"].ToString() == selectControls1[(int)SIndex.BrandCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.SportsCD].Text) || drITEM["SportsCD"].ToString() == selectControls1[(int)SIndex.SportsCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.SegmentCD].Text) || drITEM["SegmentCD"].ToString() == selectControls1[(int)SIndex.SegmentCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.YearTerm].Text) || drITEM["LastYearTerm"].ToString() == selectControls1[(int)SIndex.YearTerm].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.Season].Text) || drITEM["LastSeason"].ToString() == selectControls1[(int)SIndex.Season].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.ChangeDate].Text) || drITEM["ChangeDate"].ToString() == selectControls1[(int)SIndex.ChangeDate].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.MakerItem].Text) || drITEM["MakerItem"].ToString() == selectControls1[(int)SIndex.MakerItem].Text)
                        //&& drITEM["DelFlg"].ToString() == "0"
                        )
                    {

                        GvItem.Rows[(int)bbl.Z_Set(drITEM["TempKey"]) - 1].Visible = true;
                    }
                    else
                    {
                        GvItem.Rows[(int)bbl.Z_Set(drITEM["TempKey"]) - 1].Visible = false;
                    }
                }
            }
            else
            {
                GvSku.CurrentCell = null;
                foreach (DataRow drSKU in dtSKU.Rows)
                {
                    if ((string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.BrandCD].Text) || drSKU["BrandCD"].ToString() == selectControls1[(int)SIndex.BrandCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.SportsCD].Text) || drSKU["SportsCD"].ToString() == selectControls1[(int)SIndex.SportsCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.SegmentCD].Text) || drSKU["SegmentCD"].ToString() == selectControls1[(int)SIndex.SegmentCD].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.YearTerm].Text) || drSKU["LastYearTerm"].ToString() == selectControls1[(int)SIndex.YearTerm].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.Season].Text) || drSKU["LastSeason"].ToString() == selectControls1[(int)SIndex.Season].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.ChangeDate].Text) || drSKU["ChangeDate"].ToString() == selectControls1[(int)SIndex.ChangeDate].Text)
                        && (string.IsNullOrWhiteSpace(selectControls1[(int)SIndex.MakerItem].Text) || drSKU["MakerItem"].ToString() == selectControls1[(int)SIndex.MakerItem].Text)
                        //&& drSKU["DelFlg"].ToString() == "0"
                        )
                    {
                        GvSku.Rows[(int)bbl.Z_Set(drSKU["TempKey"]) - 1].Visible = true;
                    }
                    else
                    {
                        GvSku.Rows[(int)bbl.Z_Set(drSKU["TempKey"]) - 1].Visible = false;
                    }
                }
            }           
            this.Set_GridStyle();

        }
        /// <summary>
        /// Gridの背景色変更
        /// </summary>
        private void Set_GridStyle()
        {

            if (GvItem.Rows.Count > 0)
            {
                //入力項目以外の背景色をグレーにする
                for (int rw = 0; rw < GvItem.Rows.Count; rw++)
                {
                    for (int cl = 0; cl < (int)IColNo.COUNT; cl++)
                    {
                        if (cl != (int)IColNo.Check && cl != (int)IColNo.ChangeDate && cl != (int)IColNo.Rate && cl != (int)IColNo.PriceWithoutTax)
                        {
                            GvItem.Rows[rw].Cells[cl].Style.BackColor = Color.Silver;
                        }
                    }
                }

                for (int i = 0; i < (int)IColNo.COUNT; i++)
                {
                    if (i == (int)IColNo.Check || i == (int)IColNo.ChangeDate || i == (int)IColNo.Rate || i == (int)IColNo.PriceWithoutTax)
                    {
                        GvItem.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        GvItem.Columns[i].ReadOnly = true;
                    }
                }

                GvItem.CurrentCell = null;
            }
            if (GvSku.Rows.Count > 0)
            {
                //入力項目以外の背景色をグレーにする
                for (int rw = 0; rw < GvSku.Rows.Count; rw++)
                {
                    for (int cl = 0; cl < (int)JColNo.COUNT; cl++)
                    {
                        if (cl != (int)JColNo.Check && cl != (int)JColNo.ChangeDate && cl != (int)JColNo.Rate && cl != (int)JColNo.PriceWithoutTax)
                        {
                            GvSku.Rows[rw].Cells[cl].Style.BackColor = Color.Silver;
                        }
                    }
                }

                for (int i = 0; i < (int)JColNo.COUNT; i++)
                {
                    if (i == (int)JColNo.Check || i == (int)JColNo.ChangeDate || i == (int)JColNo.Rate || i == (int)JColNo.PriceWithoutTax)
                    {
                        GvSku.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        GvSku.Columns[i].ReadOnly = true;
                    }
                }

                GvSku.CurrentCell = null;
            }
            
        }

        /// <summary>
        /// 明細表示処理
        /// </summary>
        protected override void ExecDisp()
        {
            for (int i = 0; i < keyControls.Length ; i++)
            {
                if (CheckKey(i) == false)
                {
                    keyControls[i].Focus();
                    return;
                }

            }               

            bool ret = CheckData();
            if (ret)
            {
                //KEY部ロック
                this.Scr_Lock(0);
                selectControls1[(int)SIndex.BrandCD].Focus();
            }

            this.Scr_Clr(1);
        }

        /// <summary>
        /// 【抽出条件】にて一時テーブルより明細表示
        /// </summary>
        private void ExecDispFromWork()
        {
            for (int i = 0; i < selectControls1.Length; i++)
            {
                if (CheckSelect(i, 1) == false)
                {
                    selectControls1[i].Focus();
                    return;
                }
            }

            this.DispFromTempTable();
        }

        /// <summary>
        /// 明細追加処理
        /// </summary>
        private void ExecAdd()
        {
            for (int i = 0; i < addControls.Length; i++)
            {
                if (CheckAdd(i, true) == false)
                {
                    addControls[i].Focus();
                    return;
                }

            }

            //重複チェック
            if (!this.CheckDoubleForAdd(addControls[(int)AIndex.ChangeDateAdd].Text))
            {
                bbl.ShowMessage("E224");
                //((TextBox)addControls[(int)AIndex.ItemCD]).SelectAll();
                //addControls[(int)AIndex.ItemCD].Focus();
                return;
            }

            //ITEM　一時テーブルに追加
            int max = 0;
            if (dtITEM.Rows.Count > 0)
            {
                max = dtITEM.AsEnumerable().Max(x => (int)bbl.Z_Set(x["TempKey"]));
            }
                
            DataRow drItm = dtITEM.NewRow();
            drItm["Chk"] = "0";
            drItm["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
            drItm["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
            drItm["ITEMCD"] = addControls[(int)AIndex.ItemCD].Text;
            drItm["ITemName"] = mie.ITemName;
            drItm["MakerItem"] = mie.MakerItem;
            drItm["BrandCD"] = mie.BrandCD;
            drItm["BrandName"] = mie.BrandName;
            drItm["SportsCD"] = mie.SportsCD;
            drItm["SportsName"] = mie.SportsName;
            drItm["SegmentCD"] = mie.SegmentCD;
            drItm["SegmentName"] = mie.SegmentName;
            drItm["LastYearTerm"] = mie.LastYearTerm;
            drItm["LastSeason"] = mie.LastSeason;
            drItm["ChangeDate"] = addControls[(int)AIndex.ChangeDateAdd].Text;
            drItm["Rate"] = bbl.Z_Set(addControls[(int)AIndex.Rate].Text);
            drItm["PriceOutTax"] = bbl.Z_Set(lblPriceOutTax.Text);
            drItm["PriceWithoutTax"] = bbl.Z_Set(addControls[(int)AIndex.PriceWithoutTax].Text);
            drItm["InsertOperator"] = InOperatorCD;
            drItm["InsertDateTime"] = mUpdateDateTime;
            drItm["UpdateOperator"] = InOperatorCD;
            drItm["UpdateDateTime"] = mUpdateDateTime;
            drItm["DelFlg"] = "0";
            drItm["TempKey"] = max + 1;
            dtITEM.Rows.Add(drItm);

            //SKU　一時テーブルに追加
            //[M_SKU_Select]
            M_SKU_Entity mse = new M_SKU_Entity
            {
                MakerItem = mie.MakerItem,
                ChangeDate = bbl.FormatDate(addControls[(int)AIndex.ChangeDateAdd].Text)
            };
            DataTable dtM = msbl.M_SKU_SelectForShiireTanka(mse);
            if (dtM.Rows.Count > 0)
            {
                foreach (DataRow drM in dtM.Rows)
                {
                    //一時テーブル存在確認
                    DataRow[] selectDt = dtSKU.AsEnumerable()
                                        .Where(x => x["ITEMCD"].ToString() == drM["ITemCD"].ToString()
                                         && x["AdminNO"].ToString() == drM["AdminNO"].ToString()
                                         && x["LastYearTerm"].ToString() == drM["LastYearTerm"].ToString()
                                         && x["LastSeason"].ToString() == drM["LastSeason"].ToString()
                                         && x["ChangeDate"].ToString() == bbl.FormatDate(addControls[(int)AIndex.ChangeDateAdd].Text)).ToArray();

                    //税抜発注額＝税抜定価×（画面.掛率÷100）
                    //複数のItemがあった場合、定価が異なれば、再計算した結果をセット
                    decimal priceWithoutTax;
                    if (bbl.Z_Set(drM["PriceOutTax"]) == bbl.Z_Set(lblPriceOutTax.Text))
                    {
                        priceWithoutTax = bbl.Z_Set(addControls[(int)AIndex.PriceWithoutTax].Text);
                    }
                    else
                    {
                        priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(drM["PriceOutTax"]) * bbl.Z_Set(addControls[(int)AIndex.Rate].Text) / 100);
                    }
                    
                    //なければ、Insert
                    if (selectDt.Length == 0)
                    {
                        max = 0;
                        if (dtSKU.Rows.Count > 0)
                        {
                            max = dtSKU.AsEnumerable().Max(x => (int)bbl.Z_Set(x["TempKey"]));
                        }

                        DataRow drSKU = dtSKU.NewRow();
                        drSKU["Chk"] = "0";
                        drSKU["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
                        drSKU["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
                        drSKU["ITEMCD"] = drM["ITemCD"].ToString();
                        drSKU["ITemName"] = drM["ITemName"].ToString();
                        drSKU["AdminNO"] = bbl.Z_Set(drM["AdminNO"]);
                        drSKU["SKUCD"] = drM["SKUCD"].ToString();
                        drSKU["SizeName"] = drM["SizeName"].ToString();
                        drSKU["ColorName"] = drM["ColorName"].ToString();
                        drSKU["MakerItem"] = drM["MakerItem"].ToString();
                        drSKU["BrandCD"] = drM["BrandCD"].ToString();
                        drSKU["SportsCD"] = drM["SportsCD"].ToString();
                        drSKU["SegmentCD"] = drM["SegmentCD"].ToString();
                        drSKU["LastYearTerm"] = drM["LastYearTerm"].ToString();
                        drSKU["LastSeason"] = drM["LastSeason"].ToString();
                        drSKU["ChangeDate"] = addControls[(int)AIndex.ChangeDateAdd].Text;
                        drSKU["Rate"] = bbl.Z_Set(addControls[(int)AIndex.Rate].Text);
                        drSKU["PriceOutTax"] = bbl.Z_Set(drM["PriceOutTax"]);
                        drSKU["PriceWithoutTax"] = priceWithoutTax;
                        drSKU["InsertOperator"] = InOperatorCD;
                        drSKU["InsertDateTime"] = mUpdateDateTime;
                        drSKU["UpdateOperator"] = InOperatorCD;
                        drSKU["UpdateDateTime"] = mUpdateDateTime;
                        drSKU["DelFlg"] = "0";
                        drSKU["TempKey"] = max + 1;
                        dtSKU.Rows.Add(drSKU);

                    }
                    else
                    {
                        int row = (int)bbl.Z_Set(selectDt[0]["TempKey"]);

                        dtSKU.Rows[row - 1]["ITemName"] = drM["ITemName"].ToString();
                        dtSKU.Rows[row - 1]["SizeName"] = drM["SizeName"].ToString();
                        dtSKU.Rows[row - 1]["ColorName"] = drM["ColorName"].ToString();
                        dtSKU.Rows[row - 1]["MakerItem"] = drM["MakerItem"].ToString();
                        dtSKU.Rows[row - 1]["BrandCD"] = drM["BrandCD"].ToString();
                        dtSKU.Rows[row - 1]["SportsCD"] = drM["SportsCD"].ToString();
                        dtSKU.Rows[row - 1]["SegmentCD"] = drM["SegmentCD"].ToString();
                        dtSKU.Rows[row - 1]["LastYearTerm"] = drM["LastYearTerm"].ToString();
                        dtSKU.Rows[row - 1]["LastSeason"] = drM["LastSeason"].ToString();
                        dtSKU.Rows[row - 1]["Rate"] = bbl.Z_Set(addControls[(int)AIndex.Rate].Text);
                        dtSKU.Rows[row - 1]["PriceOutTax"] = bbl.Z_Set(drM["PriceOutTax"]);
                        dtSKU.Rows[row - 1]["PriceWithoutTax"] = priceWithoutTax;
                        dtSKU.Rows[row - 1]["UpdateOperator"] = InOperatorCD;
                        dtSKU.Rows[row - 1]["UpdateDateTime"] = mUpdateDateTime;
                        dtSKU.Rows[row - 1]["DelFlg"] = "0";
                    }
                }
            }

            //抽出条件にて明細表示
            this.DispFromTempTable();

            ((TextBox)addControls[(int)AIndex.ItemCD]).SelectAll();
            addControls[(int)AIndex.ItemCD].Focus();
        }

        /// <summary>
        /// 全選択/全解除
        /// </summary>
        /// <param name="value">true：全選択　false：全解除</param>
        private void SetAllCheckValue(bool value)
        {
            
            //ITEM 一時テーブル
            foreach (DataRow dr in dtITEM.Rows)
            {
                if (dr["DelFlg"].ToString() == "0")
                {
                    dr["Chk"] = value ? "1" : "0";
                }                
            }

            //SKU　一時テーブル
            foreach (DataRow dr in dtSKU.Rows)
            {
                if (dr["DelFlg"].ToString() == "0")
                {
                    dr["Chk"] = value ? "1" : "0";
                }                
            }

            //抽出条件にて明細表示
            this.DispFromTempTable();
        }
        
        /// <summary>
        /// 明細選択処理
        /// </summary>
        private void ExecSelect()
        {
            for (int i = 0; i < selectControls2.Length; i++)
            {
                if (CheckSelect(i, 2) == false)
                {
                    selectControls2[i].Focus();
                    return;
                }
            }

            for (int i = 0; i < dtITEM.Rows.Count ; i++)
            {                
                DataRow dr = dtITEM.Rows[i];

                if (GvItem.Rows[(int)bbl.Z_Set(dr["TempKey"]) - 1].Visible)
                {
                    if ((string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.BrandCD].Text) || dr["BrandCD"].ToString() == selectControls2[(int)SIndex.BrandCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.SportsCD].Text) || dr["SportsCD"].ToString() == selectControls2[(int)SIndex.SportsCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.SegmentCD].Text) || dr["SegmentCD"].ToString() == selectControls2[(int)SIndex.SegmentCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.YearTerm].Text) || dr["LastYearTerm"].ToString() == selectControls2[(int)SIndex.YearTerm].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.Season].Text) || dr["LastSeason"].ToString() == selectControls2[(int)SIndex.Season].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.ChangeDate].Text) || dr["ChangeDate"].ToString() == selectControls2[(int)SIndex.ChangeDate].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.MakerItem].Text) || dr["MakerItem"].ToString() == selectControls2[(int)SIndex.MakerItem].Text))
                    {
                        dr["Chk"] = "1";
                    }
                }
            }

            for (int i = 0; i < dtSKU.Rows.Count; i++)
            {
                DataRow dr = dtSKU.Rows[i];

                if (GvSku.Rows[(int)bbl.Z_Set(dr["TempKey"]) - 1].Visible)
                {
                    if ((string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.BrandCD].Text) || dr["BrandCD"].ToString() == selectControls2[(int)SIndex.BrandCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.SportsCD].Text) || dr["SportsCD"].ToString() == selectControls2[(int)SIndex.SportsCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.SegmentCD].Text) || dr["SegmentCD"].ToString() == selectControls2[(int)SIndex.SegmentCD].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.YearTerm].Text) || dr["LastYearTerm"].ToString() == selectControls2[(int)SIndex.YearTerm].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.Season].Text) || dr["LastSeason"].ToString() == selectControls2[(int)SIndex.Season].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.ChangeDate].Text) || dr["ChangeDate"].ToString() == selectControls2[(int)SIndex.ChangeDate].Text)
                    && (string.IsNullOrWhiteSpace(selectControls2[(int)SIndex.MakerItem].Text) || dr["MakerItem"].ToString() == selectControls2[(int)SIndex.MakerItem].Text))
                    {
                        dr["Chk"] = "1";
                    }
                }
            }
            
            //Grid再設定
            //this.Set_GridStyle();

            //選択項目クリア
            foreach (Control ctl in selectControls2)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in selectLabels2)
            {
                this.ctlClr(ctl);
            }
        }

        /// <summary>
        /// 明細複写処理
        /// </summary>
        private void ExecCopy()
        {
            for (int i = 0; i < copyControls.Length; i++)
            {
                if (CheckCopy(i) == false)
                {
                    copyControls[i].Focus();
                    return;
                }

            }

            //チェックONのみ取得
            DataRow[] chkDt = dtITEM.AsEnumerable()
                    .Where(x => x["Chk"].ToString() == "1").ToArray();

            if (chkDt.Length == 0)
            {
                //Ｅ１０２
                bbl.ShowMessage("E216");
                return;
            }

            //重複チェック
            foreach (DataRow dr in chkDt)
            { 
                if (!this.CheckDoubleForCopy(dr))
                {
                    bbl.ShowMessage("E224");
                    //((TextBox)copyControls[(int)CIndex.CopyDate]).SelectAll();
                    //copyControls[(int)CIndex.CopyDate].Focus();
                    return;
                }
            }

            //TempKey最大値取得
            int iMax = 0;
            if (dtITEM.Rows.Count > 0)
            {
                iMax = dtITEM.AsEnumerable().Max(x => (int)bbl.Z_Set(x["TempKey"]));
            }

            //TempKey最大値取得
            int　sMax = 0;
            if (dtSKU.Rows.Count > 0)
            {
                sMax = dtSKU.AsEnumerable().Max(x => (int)bbl.Z_Set(x["TempKey"]));
            }

            foreach (DataRow dr in chkDt)
            {
                //ITEMワークテーブルに追加
                iMax += 1;

                //複写改定日にてM_ITEM再取得
                mie = new M_ITEM_Entity
                {
                    ITemCD = dr["ITEMCD"].ToString(),
                    ChangeDate = bbl.FormatDate(copyControls[(int)CIndex.CopyDate].Text),
                    DeleteFlg = "0"
                };
                bool ret = msbl.M_ITEM_SelectForShiireTanka(mie);

                //税抜発注額＝税抜定価×（画面.掛率÷100）
                decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mie.PriceOutTax) * bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text) / 100);

                DataRow drItm = dtITEM.NewRow();
                drItm["Chk"] = "0";
                drItm["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
                drItm["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
                drItm["ITEMCD"] = dr["ITEMCD"].ToString();
                drItm["ITemName"] = mie.ITemName;
                drItm["MakerItem"] = mie.MakerItem;
                drItm["BrandCD"] = mie.BrandCD;
                drItm["BrandName"] = mie.BrandName;
                drItm["SportsCD"] = mie.SportsCD;
                drItm["SportsName"] = mie.SportsName;
                drItm["SegmentCD"] = mie.SegmentCD;
                drItm["SegmentName"] = mie.SegmentName;
                drItm["LastYearTerm"] = mie.LastYearTerm;
                drItm["LastSeason"] = mie.LastSeason;
                drItm["ChangeDate"] = copyControls[(int)CIndex.CopyDate].Text;
                drItm["Rate"] = bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text);
                drItm["PriceOutTax"] = bbl.Z_Set(mie.PriceOutTax);
                drItm["PriceWithoutTax"] = priceWithoutTax;
                drItm["InsertOperator"] = InOperatorCD;
                drItm["InsertDateTime"] = mUpdateDateTime;
                drItm["UpdateOperator"] = InOperatorCD;
                drItm["UpdateDateTime"] = mUpdateDateTime;
                drItm["DelFlg"] = "0";
                drItm["TempKey"] = iMax;
                dtITEM.Rows.Add(drItm);

                //SKUワークテーブルに追加
                //[M_SKU_Select]
                M_SKU_Entity mse = new M_SKU_Entity
                {
                    MakerItem = mie.MakerItem,
                    ChangeDate = bbl.FormatDate(copyControls[(int)CIndex.CopyDate].Text)
                };
                DataTable dtM = msbl.M_SKU_SelectForShiireTanka(mse);
                if (dtM.Rows.Count > 0)
                {
                    foreach (DataRow drM in dtM.Rows)
                    {
                        //ワークテーブル存在確認
                        DataRow[] selectDt = dtSKU.AsEnumerable()
                                       .Where(x => x["ITEMCD"].ToString() == drM["ITemCD"].ToString()
                                                && x["AdminNO"].ToString() == drM["AdminNO"].ToString()
                                                && x["LastYearTerm"].ToString() == drM["LastYearTerm"].ToString()
                                                && x["LastSeason"].ToString() == drM["LastSeason"].ToString()
                                                && x["ChangeDate"].ToString() == mse.ChangeDate).ToArray();
 
                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(bbl.Z_Set(drM["PriceOutTax"])) * bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text) / 100);


                        //なければ、Insert
                        if (selectDt.Length == 0)
                        {
                            sMax += 1;

                            DataRow drSKU = dtSKU.NewRow();
                            drSKU["Chk"] = "0";
                            drSKU["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
                            drSKU["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
                            drSKU["ITEMCD"] = drM["ITemCD"].ToString();
                            drSKU["ITemName"] = drM["ITemName"].ToString();
                            drSKU["AdminNO"] = bbl.Z_Set(drM["AdminNO"]);
                            drSKU["SKUCD"] = drM["SKUCD"].ToString();
                            drSKU["SizeName"] = drM["SizeName"].ToString();
                            drSKU["ColorName"] = drM["ColorName"].ToString();
                            drSKU["MakerItem"] = drM["MakerItem"].ToString();
                            drSKU["BrandCD"] = drM["BrandCD"].ToString();
                            drSKU["SportsCD"] = drM["SportsCD"].ToString();
                            drSKU["SegmentCD"] = drM["SegmentCD"].ToString();
                            drSKU["LastYearTerm"] = drM["LastYearTerm"].ToString();
                            drSKU["LastSeason"] = drM["LastSeason"].ToString();
                            drSKU["ChangeDate"] = copyControls[(int)CIndex.CopyDate].Text;
                            drSKU["Rate"] = bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text);
                            drSKU["PriceOutTax"] = bbl.Z_Set(bbl.Z_Set(drM["PriceOutTax"]));
                            drSKU["PriceWithoutTax"] = priceWithoutTax;
                            drSKU["InsertOperator"] = InOperatorCD;
                            drSKU["InsertDateTime"] = mUpdateDateTime;
                            drSKU["UpdateOperator"] = InOperatorCD;
                            drSKU["UpdateDateTime"] = mUpdateDateTime;
                            drSKU["DelFlg"] = "0";
                            drSKU["TempKey"] = sMax;
                            dtSKU.Rows.Add(drSKU);

                        }
                        else
                        {
                            int row = (int)bbl.Z_Set(selectDt[0]["TempKey"]);

                            dtSKU.Rows[row - 1]["ITemName"] = drM["ITemName"].ToString();
                            dtSKU.Rows[row - 1]["SizeName"] = drM["SizeName"].ToString();
                            dtSKU.Rows[row - 1]["ColorName"] = drM["ColorName"].ToString();
                            dtSKU.Rows[row - 1]["MakerItem"] = drM["MakerItem"].ToString();
                            dtSKU.Rows[row - 1]["BrandCD"] = drM["BrandCD"].ToString();
                            dtSKU.Rows[row - 1]["SportsCD"] = drM["SportsCD"].ToString();
                            dtSKU.Rows[row - 1]["SegmentCD"] = drM["SegmentCD"].ToString();
                            dtSKU.Rows[row - 1]["LastYearTerm"] = drM["LastYearTerm"].ToString();
                            dtSKU.Rows[row - 1]["LastSeason"] = drM["LastSeason"].ToString();
                            dtSKU.Rows[row - 1]["PriceOutTax"] = bbl.Z_Set(bbl.Z_Set(drM["PriceOutTax"]));
                            dtSKU.Rows[row - 1]["Rate"] = bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text);
                            dtSKU.Rows[row - 1]["PriceWithoutTax"] = priceWithoutTax;
                            dtSKU.Rows[row - 1]["UpdateOperator"] = InOperatorCD;
                            dtSKU.Rows[row - 1]["UpdateDateTime"] = mUpdateDateTime;
                            dtSKU.Rows[row - 1]["DelFlg"] = "0";
                        }
                    }
                }
            }

            //抽出条件にて明細表示
            this.DispFromTempTable();

            //((TextBox)copyControls[(int)CIndex.CopyDate]).SelectAll();
            //copyControls[(int)CIndex.CopyDate].Focus();
        }

        /// <summary>
        /// 明細変更処理
        /// </summary>
        private void ExecUpdate()
        {
            for (int i = 0; i < copyControls.Length; i++)
            {
                if (i == (int)CIndex.CopyRate)
                {
                    if (CheckCopy(i) == false)
                    {
                        copyControls[i].Focus();
                        return;
                    }
                }
            }

            //チェックONのみ取得
            DataRow[] chkDt = dtITEM.AsEnumerable()
                    .Where(x => x["Chk"].ToString() == "1").ToArray();

            if (chkDt.Length == 0)
            {
                //Ｅ１０２
                bbl.ShowMessage("E216");
                return;
            }

            foreach (DataRow dr in chkDt)
            {
                //税抜発注額＝税抜定価×（画面.掛率÷100）
                decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(dr["PriceOutTax"]) * bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text) / 100);

                //ITEM 一時テーブル変更
                DataRow[] selectDt = dtITEM.AsEnumerable()
                            　　　　.Where(x => bbl.Z_Set(x["TempKey"]) == bbl.Z_Set(dr["TempKey"])).ToArray();


                if (selectDt.Length > 0)
                {
                    int row = (int)bbl.Z_Set(selectDt[0]["TempKey"]);

                    dtITEM.Rows[row - 1]["Rate"] = bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text);
                    dtITEM.Rows[row - 1]["PriceWithoutTax"] = priceWithoutTax;
                    dtITEM.Rows[row - 1]["UpdateOperator"] = InOperatorCD;
                    dtITEM.Rows[row - 1]["UpdateDateTime"] = mUpdateDateTime;
                }

                //SKU 一時テーブル変更
                selectDt = dtSKU.AsEnumerable()
                           .Where(x => x["MakerItem"].ToString() == dr["MakerItem"].ToString()
                                    && x["ChangeDate"].ToString() == dr["ChangeDate"].ToString()).ToArray();

                if (selectDt.Length > 0)
                {
                    foreach (DataRow drSKU in selectDt)
                    {
                        int row = (int)bbl.Z_Set(drSKU["TempKey"]);

                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(drSKU["PriceOutTax"]) * bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text) / 100);

                        dtSKU.Rows[row - 1]["Rate"] = bbl.Z_Set(copyControls[(int)CIndex.CopyRate].Text);
                        dtSKU.Rows[row - 1]["PriceWithoutTax"] = priceWithoutTax;
                        dtSKU.Rows[row - 1]["UpdateOperator"] = InOperatorCD;
                        dtSKU.Rows[row - 1]["UpdateDateTime"] = mUpdateDateTime;
                    }
                }
            }

            //抽出条件にて明細表示
            this.DispFromTempTable();

            //((TextBox)copyControls[(int)CIndex.CopyDate]).SelectAll();
            //copyControls[(int)CIndex.CopyDate].Focus();
        }

        /// <summary>
        /// 明細削除処理
        /// </summary>
        private void ExecDelete()
        {
            //チェックONのみ取得
            DataRow[] chkDt = dtITEM.AsEnumerable()
                    .Where(x => x["Chk"].ToString() == "1").ToArray();

            if (chkDt.Length == 0)
            {
                //Ｅ１０２
                bbl.ShowMessage("E216");
                return;
            }

            //Ｑ１０２	
            if (bbl.ShowMessage("Q102") != DialogResult.Yes)
                return;

            foreach (DataRow dr in chkDt)
            {
                //SKU 一時テーブル変更
                //ITEMを先にするとdrの値が取得できないため、SKUを先に削除
                DataRow[] selectDt = dtSKU.AsEnumerable()
                           .Where(x => x["MakerItem"].ToString() == dr["MakerItem"].ToString()
                                    && x["ChangeDate"].ToString() == dr["ChangeDate"].ToString())
                           .OrderByDescending(x => x["TempKey"]).ToArray();

                if (selectDt.Length > 0)
                {
                    foreach (DataRow drSKU in selectDt)
                    {
                        int row = (int)bbl.Z_Set(drSKU["TempKey"]);

                        dtSKU.Rows[row - 1].Delete();
                    }
                    dtSKU.AcceptChanges();

                    //TempKey振り直し
                    for (int i = 0; i < dtSKU.Rows.Count; i++)
                    {
                        dtSKU.Rows[i]["TempKey"] = i + 1;
                    }
                }

                //ITEM 一時テーブル変更
                selectDt = dtITEM.AsEnumerable()
                            .Where(x => bbl.Z_Set(x["TempKey"]) == bbl.Z_Set(dr["TempKey"]))
                            .OrderByDescending(x => x["TempKey"]).ToArray();


                if (selectDt.Length > 0)
                {
                    int row = (int)bbl.Z_Set(selectDt[0]["TempKey"]);

                    dtITEM.Rows[row - 1].Delete();
                }
                dtITEM.AcceptChanges();

                //TempKey振り直し
                for (int i = 0; i < dtITEM.Rows.Count; i++)
                {
                    dtITEM.Rows[i]["TempKey"] = i + 1;
                }

            }

            //抽出条件にて明細表示
            this.DispFromTempTable();

            //((TextBox)copyControls[(int)CIndex.CopyDate]).SelectAll();
            //copyControls[(int)CIndex.CopyDate].Focus();
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        private void ExecInput()
        {
            string fileName = "";

            //　ファイルを選択
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "EXCELブック(*.xlsx;*.xls)|*.xlsx;*.xls|すべてのファイル(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = fileDialog.FileName;
            }
            else
            {
                return;
            }

            // 拡張子がExcelではない時
            if (Path.GetExtension(fileName).ToLower() != ".xlsx" && Path.GetExtension(fileName).ToLower() != ".xls")
            {
                bbl.ShowMessage("E137");
                btnInput.Focus();
                return;
            }

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            Microsoft.Office.Interop.Excel.Worksheet sheet = null;

            try
            {
                ExcelApp.Visible = false;

                // ファイルOPEN
                wb = ExcelApp.Workbooks.Open(fileName);

                sheet = wb.Sheets[1];
                sheet.Select();

                // 読み込む範囲を指定する
                Microsoft.Office.Interop.Excel.Range InputRange = sheet.Range[sheet.Cells[1, 1], sheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell)];

                var yCount = sheet.UsedRange.Rows.Count;
                var xCount = sheet.UsedRange.Columns.Count;

                // 指定された範囲のセルの値をオブジェクト型の配列に読み込む
                object[,] InputData = (System.Object[,])InputRange.Value;

                // フォーマットチェック
                if (InputData[1, (int)EColNo.VendorCD + 1].ToString() != "仕入先CD"
                    || InputData[1, (int)EColNo.StoreCD + 1].ToString() != "店舗CD"
                    || InputData[1, (int)EColNo.ChangeDate + 1].ToString() != "改定日"
                    || InputData[1, (int)EColNo.Rate + 1].ToString() != "掛率")
                {
                    bbl.ShowMessage("E137");
                    btnInput.Focus();
                    return;
                }

                //データクリア
                this.Scr_Clr(1);

                dtITEM.Clear();
                dtSKU.Clear();

                GvItem.DataSource = dtITEM;
                GvSku.DataSource = dtSKU;

                int iMax = 0;

                for (int row = 2; row < InputData.GetLength(0); row++)
                {
                    // 各行の値をListに変換（すべてnullは読み飛ばし）
                    List<string> data = new List<string>();
                    bool isEmpty = true;
                    for (int col = 1; col <= (int)EColNo.COUNT ; col++)
                    {
                        if (InputData[row, col] != null)
                        {
                            isEmpty = false;
                            break;
                        }
                    }

                    if (isEmpty)
                        continue;

                    for (int col = 1; col <= (int)EColNo.COUNT; col++)
                    {
                        if (InputData[row, col] != null)
                            data.Add(InputData[row, col].ToString());
                        else
                            data.Add("");
                    }

                    //データチェック(1行目はヘッダ）
                    if (!Check_ExcelData(data))
                        continue;

                    iMax += 1;

                    //ITEM　一時テーブルに追加   
                    DataRow drItm = dtITEM.NewRow();
                    drItm["Chk"] = "0";
                    drItm["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
                    drItm["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
                    drItm["ITEMCD"] = data[(int)EColNo.ItemCD].ToString();
                    drItm["ITemName"] = mie.ITemName;
                    drItm["MakerItem"] = mie.MakerItem;
                    drItm["BrandCD"] = mie.BrandCD;
                    drItm["BrandName"] = mie.BrandName;
                    drItm["SportsCD"] = mie.SportsCD;
                    drItm["SportsName"] = mie.SportsName;
                    drItm["SegmentCD"] = mie.SegmentCD;
                    drItm["SegmentName"] = mie.SegmentName;
                    drItm["LastYearTerm"] = mie.LastYearTerm;
                    drItm["LastSeason"] = mie.LastSeason;
                    drItm["ChangeDate"] = bbl.FormatDate(data[(int)EColNo.ChangeDate].ToString());
                    drItm["Rate"] = bbl.Z_Set(data[(int)EColNo.Rate]);
                    drItm["PriceOutTax"] = bbl.Z_Set(mie.PriceOutTax);
                    drItm["PriceWithoutTax"] = bbl.Z_Set(data[(int)EColNo.PriceWithoutTax]);
                    drItm["InsertOperator"] = InOperatorCD;
                    drItm["InsertDateTime"] = mUpdateDateTime;
                    drItm["UpdateOperator"] = InOperatorCD;
                    drItm["UpdateDateTime"] = mUpdateDateTime;
                    drItm["DelFlg"] = "0";
                    drItm["TempKey"] = iMax;
                    dtITEM.Rows.Add(drItm);

                    //SKU　一時テーブルに追加
                    //[M_SKU_Select]
                    M_SKU_Entity mse = new M_SKU_Entity
                    {
                        MakerItem = mie.MakerItem,
                        ChangeDate = bbl.FormatDate(data[(int)EColNo.ChangeDate].ToString())
                    };
                    DataTable dtM = msbl.M_SKU_SelectForShiireTanka(mse);
                    if (dtM.Rows.Count > 0)
                    {
                        foreach (DataRow drM in dtM.Rows)
                        {
                            //一時テーブル存在確認
                            DataRow[] selectDt = dtSKU.AsEnumerable()
                                                .Where(x => x["ITEMCD"].ToString() == drM["ITemCD"].ToString()
                                                 && x["AdminNO"].ToString() == drM["AdminNO"].ToString()
                                                 && x["LastYearTerm"].ToString() == drM["LastYearTerm"].ToString()
                                                 && x["LastSeason"].ToString() == drM["LastSeason"].ToString()
                                                 && x["ChangeDate"].ToString() == bbl.FormatDate(data[(int)EColNo.ChangeDate].ToString())).ToArray();

                            //なければ、Insert
                            if (selectDt.Length == 0)
                            {
                                int sMax = 0;
                                if (dtSKU.Rows.Count > 0)
                                {
                                    sMax = dtSKU.AsEnumerable().Max(x => (int)bbl.Z_Set(x["TempKey"]));
                                }

                                DataRow drSKU = dtSKU.NewRow();
                                drSKU["Chk"] = "0";
                                drSKU["VendorCD"] = keyControls[(int)KIndex.VendorCD].Text;
                                drSKU["StoreCD"] = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
                                drSKU["ITEMCD"] = drM["ITemCD"].ToString();
                                drSKU["ITemName"] = drM["ITemName"].ToString();
                                drSKU["AdminNO"] = bbl.Z_Set(drM["AdminNO"]);
                                drSKU["SKUCD"] = drM["SKUCD"].ToString();
                                drSKU["SizeName"] = drM["SizeName"].ToString();
                                drSKU["ColorName"] = drM["ColorName"].ToString();
                                drSKU["MakerItem"] = drM["MakerItem"].ToString();
                                drSKU["BrandCD"] = drM["BrandCD"].ToString();
                                drSKU["SportsCD"] = drM["SportsCD"].ToString();
                                drSKU["SegmentCD"] = drM["SegmentCD"].ToString();
                                drSKU["LastYearTerm"] = drM["LastYearTerm"].ToString();
                                drSKU["LastSeason"] = drM["LastSeason"].ToString();
                                drSKU["ChangeDate"] = bbl.FormatDate(data[(int)EColNo.ChangeDate].ToString());
                                drSKU["Rate"] = bbl.Z_Set(data[(int)EColNo.Rate]);
                                drSKU["PriceOutTax"] = bbl.Z_Set(drM["PriceOutTax"]);
                                drSKU["PriceWithoutTax"] = bbl.Z_Set(data[(int)EColNo.PriceWithoutTax]);
                                drSKU["InsertOperator"] = InOperatorCD;
                                drSKU["InsertDateTime"] = mUpdateDateTime;
                                drSKU["UpdateOperator"] = InOperatorCD;
                                drSKU["UpdateDateTime"] = mUpdateDateTime;
                                drSKU["DelFlg"] = "0";
                                drSKU["TempKey"] = sMax + 1;
                                dtSKU.Rows.Add(drSKU);

                            }
                            else
                            {
                                int rw = (int)bbl.Z_Set(selectDt[0]["TempKey"]);

                                dtSKU.Rows[rw - 1]["ITemName"] = drM["ITemName"].ToString();
                                dtSKU.Rows[rw - 1]["SizeName"] = drM["SizeName"].ToString();
                                dtSKU.Rows[rw - 1]["ColorName"] = drM["ColorName"].ToString();
                                dtSKU.Rows[rw - 1]["MakerItem"] = drM["MakerItem"].ToString();
                                dtSKU.Rows[rw - 1]["BrandCD"] = drM["BrandCD"].ToString();
                                dtSKU.Rows[rw - 1]["SportsCD"] = drM["SportsCD"].ToString();
                                dtSKU.Rows[rw - 1]["SegmentCD"] = drM["SegmentCD"].ToString();
                                dtSKU.Rows[rw - 1]["LastYearTerm"] = drM["LastYearTerm"].ToString();
                                dtSKU.Rows[rw - 1]["LastSeason"] = drM["LastSeason"].ToString();
                                dtSKU.Rows[rw - 1]["Rate"] = bbl.Z_Set(data[(int)EColNo.Rate]);
                                dtSKU.Rows[rw - 1]["PriceOutTax"] = bbl.Z_Set(drM["PriceOutTax"]);
                                dtSKU.Rows[rw - 1]["PriceWithoutTax"] = bbl.Z_Set(data[(int)EColNo.PriceWithoutTax]);
                                dtSKU.Rows[rw - 1]["UpdateOperator"] = InOperatorCD;
                                dtSKU.Rows[rw - 1]["UpdateDateTime"] = mUpdateDateTime;
                                dtSKU.Rows[rw - 1]["DelFlg"] = "0";
                            }
                        }
                    }

                }

                //抽出条件にて明細表示
                this.DispFromTempTable();
                
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
            finally
            {                
                if (wb != null)
                {
                    wb.Close(false);
                }
                Marshal.ReleaseComObject(sheet);
                Marshal.ReleaseComObject(wb);
                ExcelApp.Quit();
            }

            
        }

        /// <summary>
        /// KEY部のチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        private bool CheckKey(int index)
        {
            switch (index)
            {
                case (int)KIndex.VendorCD:
                    //入力必須(Entry required)
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        scVendor.LabelText = ""; 
                        return false;
                    }

                    //[M_Vendor_Select]
                    M_Vendor_Entity mve = new M_Vendor_Entity
                    {
                        VendorCD = keyControls[index].Text,
                        VendorFlg = "1",
                        ChangeDate = txtBaseDate.Text
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
                
                case (int)KIndex.BaseDate:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(keyControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(keyControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    keyControls[index].Text = bbl.FormatDate(keyControls[index].Text);

                    if (mOldBaseDate != keyControls[index].Text)
                    {
                        //コンボボックス再設定
                        string ymd = keyControls[index].Text;
                        cboStoreCD.Bind(ymd);
                        cboYearTerm1.Bind(ymd);
                        cboSeason1.Bind(ymd);
                        cboYearTerm2.Bind(ymd);
                        cboSeason2.Bind(ymd);
                    }
                    mOldBaseDate = keyControls[index].Text;

                    break;

                case (int)KIndex.StoreCD:
                    if (keyControls[index].Enabled)
                    {
                        //選択必須(Entry required)
                        if (!RequireCheck(new Control[] { keyControls[index] }))
                        {
                            return false;
                        }
                        else
                        {
                            if (!base.CheckAvailableStores(cboStoreCD.SelectedValue.ToString()))
                            {
                                bbl.ShowMessage("E141");
                                panelDetail.Focus();
                                return false;
                            }
                        }
                    }                   

                    break;

            }

            return true;
        }

        /// <summary>
        /// 【抽出条件】【選択】のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="kbn">1:【抽出条件】のコントロール　2：【選択】のコントロール</param>
        /// <returns></returns>
        private bool CheckSelect(int index, short kbn)
        {
            bool ret;
            Control ctl = kbn == 1 ? selectControls1[index] : selectControls2[index];

            switch (index)
            {
                case (int)SIndex.BrandCD:
                    ((Search.CKM_SearchControl)ctl.Parent).LabelText = "";

                    if (!string.IsNullOrWhiteSpace(ctl.Text))
                    {
                        //以下の条件でM_Brandが存在しない場合、エラー

                        //[M_Brand]
                        M_Brand_Entity mbe = new M_Brand_Entity
                        {
                            BrandCD = ctl.Text
                        };
                        Brand_BL bbl = new Brand_BL();
                        ret = bbl.M_Brand_Select(mbe);
                        if (ret)
                        {
                            ((Search.CKM_SearchControl)ctl.Parent).LabelText = mbe.BrandName;
                        }
                    }
                    break;

                case (int)SIndex.SportsCD:
                case (int)SIndex.SegmentCD:
                    ((Search.CKM_SearchControl)ctl.Parent).LabelText = "";
                    if (!string.IsNullOrWhiteSpace(ctl.Text))
                    {
                        //以下の条件でM_MultiPorposeが存在しない場合、エラー
                        string id = "";
                        if (index.Equals((int)SIndex.SportsCD))
                            id = MultiPorpose_BL.ID_SPORTS;
                        else
                            id = MultiPorpose_BL.ID_SegmentCD;

                        //[M_MultiPorpose]
                        M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                        {
                            ID = id,
                            Key = ctl.Text
                        };
                        MultiPorpose_BL mbl = new MultiPorpose_BL();
                        DataTable dt = mbl.M_MultiPorpose_Select(mme);
                        if (dt.Rows.Count > 0)
                        {
                            ((Search.CKM_SearchControl)ctl.Parent).LabelText = dt.Rows[0]["Char1"].ToString();
                        }
                    }
                    break;

                case (int)SIndex.ChangeDate:          
                    if (!string.IsNullOrWhiteSpace(ctl.Text))
                    {
                        //日付として正しいこと(Be on the correct date)Ｅ１０３
                        if (!bbl.CheckDate(ctl.Text))
                        {
                            //Ｅ１０３
                            bbl.ShowMessage("E103");
                            return false;
                        }

                        ctl.Text = bbl.FormatDate(ctl.Text);

                    }
                    break;

            }

            return true;
        }

        /// <summary>
        /// 【明細追加】のコードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <param name="blnAll">追加ボタン押下時</param>
        /// <returns></returns>
        private bool CheckAdd(int index, bool blnAll)
        {
            bool ret;
            switch (index)
            {
                case (int)AIndex.ItemCD:
                    scItem.LabelText = "";
                    lblPriceOutTax.Text = "";

                    //必須
                    if (string.IsNullOrWhiteSpace(addControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //[M_ITEM_Select]
                    string ymd;
                    if (string.IsNullOrWhiteSpace(addControls[(int)AIndex.ChangeDateAdd].Text))
                        ymd = bbl.FormatDate(keyControls[(int)KIndex.BaseDate].Text);
                    else
                        ymd = bbl.FormatDate(addControls[(int)AIndex.ChangeDateAdd].Text);

                    mie = new M_ITEM_Entity
                    {
                        ITemCD = addControls[index].Text,
                        ChangeDate = ymd
                    };
                    
                    ret = msbl.M_ITEM_SelectForShiireTanka(mie);
                    if (ret)
                    {
                        if (mie.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            scItem.TxtCode.SelectAll();
                            return false;
                        }
                        scItem.LabelText = mie.ITemName;
                        lblPriceOutTax.Text = bbl.Z_SetStr(mie.PriceOutTax);
                    }
                    else
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        scItem.TxtCode.SelectAll();
                        return false;
                    }
                    break;

                case (int)AIndex.ChangeDateAdd:
                    if (!blnAll)
                    {
                        if (string.IsNullOrWhiteSpace(addControls[index].Text))
                            addControls[index].Text = bbl.FormatDate(keyControls[(int)KIndex.BaseDate].Text);
                    }

                    if (string.IsNullOrWhiteSpace(addControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(addControls[index].Text))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        ((TextBox)addControls[index]).SelectAll();
                        return false;
                    }
                    addControls[index].Text = bbl.FormatDate(addControls[index].Text);

                    //M_ITEM再取得
                    mie = new M_ITEM_Entity
                    {
                        ITemCD = addControls[(int)AIndex.ItemCD].Text,
                        ChangeDate = bbl.FormatDate(addControls[index].Text)
                    };
                    
                    ret = msbl.M_ITEM_SelectForShiireTanka(mie);
                    if (ret)
                    {
                        if (mie.DeleteFlg == "1")
                        {
                            bbl.ShowMessage("E119");
                            scItem.TxtCode.SelectAll();
                            return false;
                        }
                        scItem.LabelText = mie.ITemName;
                        lblPriceOutTax.Text = bbl.Z_SetStr(mie.PriceOutTax);
                    }
                    else
                    {
                        //Ｅ１０１
                        bbl.ShowMessage("E101");
                        scItem.TxtCode.SelectAll();
                        return false;
                    }
                    break;

                case (int)AIndex.Rate:
                    if (bbl.Z_Set(addControls[index].Text) == 0)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        ((TextBox)addControls[index]).SelectAll();
                        return false;
                    }

                    if (mOldRate != bbl.Z_Set(addControls[index].Text) || mOldItemCd != addControls[(int)AIndex.ItemCD].Text)
                    {
                        mOldRate = bbl.Z_Set(addControls[index].Text);
                        mOldItemCd = addControls[(int)AIndex.ItemCD].Text;

                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        addControls[(int)AIndex.PriceWithoutTax].Text = bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(lblPriceOutTax.Text) * bbl.Z_Set(addControls[(int)AIndex.Rate].Text) / 100));
                    }
                    break;

                case (int)AIndex.PriceWithoutTax:
                    if (bbl.Z_Set(addControls[index].Text) == 0)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        ((TextBox)addControls[index]).SelectAll();
                        return false;
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// 【複写】のコードチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckCopy(int index)
        {
            switch (index)
            {
                case (int)CIndex.CopyDate:
                if (string.IsNullOrWhiteSpace(copyControls[index].Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    return　false;
                }

                //日付として正しいこと(Be on the correct date)Ｅ１０３
                if (!bbl.CheckDate(copyControls[index].Text))
                {
                    //Ｅ１０３
                    bbl.ShowMessage("E103");
                    return false; 
                }

                copyControls[index].Text = bbl.FormatDate(copyControls[index].Text);

                
                break;

                case (int)CIndex.CopyRate:
                    if (bbl.Z_Set(copyControls[index].Text) == 0)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 取込データチェック
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Check_ExcelData(List<string> data)
        {
            // 仕入先CDチェック
            if (data[(int)EColNo.VendorCD].ToString() != keyControls[(int)KIndex.VendorCD].Text)
            {
                bbl.ShowMessage("E230", "仕入先CD","仕入先CD");
                return false;
            }

            // 店舗CDチェック
            string StoreCd = rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString();
            if (data[(int)EColNo.StoreCD].ToString() != StoreCd)
            {
                bbl.ShowMessage("E234");
                return false;
            }

            // 改定日チェック
            if (string.IsNullOrWhiteSpace(data[(int)EColNo.ChangeDate].ToString()))
            {
                //Ｅ１０２
                bbl.ShowMessage("E103");
                return false;
            }

            string tmpYmd = data[(int)EColNo.ChangeDate].ToString().Replace("/","");
            string ymd = tmpYmd.Substring(0, 4) + "/" + tmpYmd.Substring(4, 2) + "/" + tmpYmd.Substring(6, 2);

            //日付として正しいこと(Be on the correct date)Ｅ１０３
            if (!bbl.CheckDate(ymd))
            {
                //Ｅ１０３
                bbl.ShowMessage("E103");
                return false;
            }
            ymd = bbl.FormatDate(data[(int)EColNo.ChangeDate].ToString());

            // ITEMチェック
            mie = new M_ITEM_Entity
            {
                ITemCD = data[(int)EColNo.ItemCD].ToString(),
                //ChangeDate = bbl.FormatDate(keyControls[(int)KIndex.BaseDate].Text),
                ChangeDate = ymd,
                DeleteFlg = "0"
            };

            if (!msbl.M_ITEM_SelectForShiireTanka(mie))
            {
                bbl.ShowMessage("E138", "Item");
                return false;
            }

            // 掛率数値チェック
            decimal rate;
            if (!Decimal.TryParse(data[(int)EColNo.Rate], out rate))
            {
                //Ｅ１０２
                bbl.ShowMessage("E190");
                return false;
            }

            // 発注単価数値チェック
            decimal tanka;
            if (!Decimal.TryParse(data[(int)EColNo.PriceWithoutTax], out tanka))
            {
                //Ｅ１０２
                bbl.ShowMessage("E190");
                return false;
            }

            // 掛率チェック
            if (bbl.Z_Set(data[(int)EColNo.Rate]) == 0)
            {
                //Ｅ１０２
                bbl.ShowMessage("E102");
                return false;
            }

            //重複チェック
            if (!this.CheckDoubleForAdd(ymd))
            {
                bbl.ShowMessage("E105");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録時全明細チェック
        /// </summary>
        /// <returns></returns>
        private bool Chk_All()
        {
            //すべての明細を表示後、チェックする
            GvItem.CommitEdit(DataGridViewDataErrorContexts.Commit);
            GvSku.CommitEdit(DataGridViewDataErrorContexts.Commit);
            foreach (Control ctl in selectControls1)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in selectLabels1)
            {
                this.ctlClr(ctl);
            }
            this.DispFromTempTable();

            //ITEM明細チェック
            for (int rw = 0; rw < GvItem.Rows.Count; rw++)
            {
                for (int cl = 0; cl < (int)IColNo.COUNT; cl++)
                {
                    if (!CheckItmDetail(rw, cl))
                    {
                        if (mOpeMode == EOpeMode.SKU)
                        {
                            rdoITEM.Checked = true;
                        }
                        GvItem.CurrentCell = GvItem[cl, rw];
                        GvItem.Focus();
                        return false;
                    }
                }        
            }

            //重複チェック
            int errRow;
            if (!CheckDouble(out errRow))
            {
                bbl.ShowMessage("E224");
                GvItem.CurrentCell = GvItem[(int)IColNo.ChangeDate, errRow];
                GvItem.Focus();
                return false;
            }

            //SKU明細チェック
            for (int rw = 0; rw < GvSku.Rows.Count; rw++)
            {
                for (int cl = 0; cl < (int)JColNo.COUNT; cl++)
                {
                    if (!CheckSkuDetail(rw, cl))
                    {
                        if (mOpeMode == EOpeMode.ITEM)
                        {
                            rdoSKU.Checked = true;
                        }
                        GvSku.CurrentCell = GvSku[cl, rw];
                        GvSku.Focus();
                        return false;
                    }
                }
            }            

            return true;
        }

        /// <summary>
        /// 更新日付を最新にする
        /// </summary>
        private void SetUpdateDate()
        {
            //更新日付を最新にする
            foreach (DataRow dr in dtITEM.Rows)
            {
                if ((DateTime)dr["InsertDateTime"] == mUpdateDateTime)
                {
                    dr["InsertDateTime"] = DateTime.Now;
                }
                if ((DateTime)dr["UpdateDateTime"] == mUpdateDateTime)
                {
                    dr["UpdateDateTime"] = DateTime.Now;
                }
            }

            foreach (DataRow dr in dtSKU.Rows)
            {
                if ((DateTime)dr["InsertDateTime"] == mUpdateDateTime)
                {
                    dr["InsertDateTime"] = DateTime.Now;
                }
                if ((DateTime)dr["UpdateDateTime"] == mUpdateDateTime)
                {
                    dr["UpdateDateTime"] = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 登録時ITM明細チェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckItmDetail(int row, int col)
        {
            String value = GvItem.Rows[row].Cells[col].Value.ToString();

            switch (col)
            {
                case (int)IColNo.ChangeDate:
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }                   

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(value))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    GvItem.Rows[row].Cells[col].Value = bbl.FormatDate(value);

                    //M_ITEMより再表示
                    this.DispFromItemForDetail(row, value);

                    ////改定日にてM_ITEM再取得
                    //mie = new M_ITEM_Entity
                    //{
                    //    ITemCD = GvItem.Rows[row].Cells[(int)IColNo.ItemCD].Value.ToString(),
                    //    ChangeDate = bbl.FormatDate(value),
                    //    DeleteFlg = "0"
                    //};
                    //bool ret = msbl.M_ITEM_SelectForShiireTanka(mie);

                    ////M_ITEMより再表示
                    //if (bbl.Z_Set(GvItem.Rows[row].Cells[(int)IColNo.PriceOutTax].Value) != bbl.Z_Set(mie.PriceOutTax))
                    //{
                    //    //税抜発注額＝税抜定価×（画面.掛率÷100）
                    //    //定価が変わった場合のみ再表示
                    //    decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mie.PriceOutTax) * bbl.Z_Set(GvItem.Rows[row].Cells[(int)IColNo.Rate].Value) / 100);
                    //    GvItem.Rows[row].Cells[(int)IColNo.PriceWithoutTax].Value = priceWithoutTax;
                    //}
                    //GvItem.Rows[row].Cells[(int)IColNo.ItemName].Value = mie.ITemName;
                    //GvItem.Rows[row].Cells[(int)IColNo.MakerItem].Value = mie.MakerItem;
                    //GvItem.Rows[row].Cells[(int)IColNo.BrandName].Value = mie.BrandName;
                    //GvItem.Rows[row].Cells[(int)IColNo.SportsName].Value = mie.SportsName;
                    //GvItem.Rows[row].Cells[(int)IColNo.SegmentName].Value = mie.SegmentName;
                    //GvItem.Rows[row].Cells[(int)IColNo.LastYearTerm].Value = mie.LastYearTerm;
                    //GvItem.Rows[row].Cells[(int)IColNo.LastSeason].Value = mie.LastSeason;
                    //GvItem.Rows[row].Cells[(int)IColNo.PriceOutTax].Value = bbl.Z_Set(mie.PriceOutTax);

                    //dtITEM.Rows[row - 1]["BrandCD"] = mie.BrandCD;
                    //dtITEM.Rows[row - 1]["SportsCD"] = mie.SportsCD;
                    //dtITEM.Rows[row - 1]["SegmentCD"] = mie.SegmentCD;

                    break;

                case (int)IColNo.Rate:
                    if (bbl.Z_Set(value) == 0)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;

                
            }
            return true;
        }

        /// <summary>
        /// 明細チェック時のITEM内容表示
        /// </summary>
        private void DispFromItemForDetail(int row, string date)
        {
            //改定日にてM_ITEM再取得
            mie = new M_ITEM_Entity
            {
                ITemCD = dtITEM.Rows[row]["ITemCD"].ToString(),
                ChangeDate = bbl.FormatDate(date),
                DeleteFlg = "0"
            };
            bool ret = msbl.M_ITEM_SelectForShiireTanka(mie);

            //M_ITEMより再表示
            if (bbl.Z_Set(dtITEM.Rows[row]["PriceOutTax"]) != bbl.Z_Set(mie.PriceOutTax))
            {
                //税抜発注額＝税抜定価×（画面.掛率÷100）
                //定価が変わった場合のみ再表示
                decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(mie.PriceOutTax) * bbl.Z_Set(dtITEM.Rows[row]["Rate"]) / 100);
                dtITEM.Rows[row]["PriceWithoutTax"] = priceWithoutTax;
            }
            dtITEM.Rows[row]["ITemName"] = mie.ITemName;
            dtITEM.Rows[row]["MakerItem"] = mie.MakerItem;
            dtITEM.Rows[row]["BrandName"] = mie.BrandName;
            dtITEM.Rows[row]["SportsName"] = mie.SportsName;
            dtITEM.Rows[row]["SegmentName"] = mie.SegmentName;
            dtITEM.Rows[row]["LastYearTerm"]= mie.LastYearTerm;
            dtITEM.Rows[row]["LastSeason"] = mie.LastSeason;
            dtITEM.Rows[row]["PriceOutTax"] = bbl.Z_Set(mie.PriceOutTax);
            dtITEM.Rows[row]["BrandCD"] = mie.BrandCD;
            dtITEM.Rows[row]["SportsCD"] = mie.SportsCD;
            dtITEM.Rows[row]["SegmentCD"] = mie.SegmentCD;
         
            //マスタ内容が反映されないため
            GvItem.Refresh();
        }

        /// <summary>
        /// 登録時SKU明細チェック
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool CheckSkuDetail(int row, int col)
        {
            String value = GvSku.Rows[row].Cells[col].Value.ToString();

            switch (col)
            {
                case (int)JColNo.ChangeDate:
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!bbl.CheckDate(value))
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }

                    GvSku.Rows[row].Cells[col].Value = bbl.FormatDate(value);

                    //M_SKUより再表示
                    this.DispFromSkuForDetail(row, value);

                    ////改定日にてM_SKU再取得
                    //M_SKU_Entity mse = new M_SKU_Entity
                    //{
                    //    AdminNO = dtSKU.Rows[row - 1]["AdminNO"].ToString(),
                    //    ChangeDate = bbl.FormatDate(value)
                    //};
                    //DataTable dtM = msbl.M_SKU_SelectForShiireTanka(mse);

                    //if (dtM.Rows.Count > 0)
                    //{
                    //    //M_SKUより再表示
                    //    if (bbl.Z_Set(dtSKU.Rows[row - 1]["PriceOutTax"]) != bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]))
                    //    {
                    //        //税抜発注額＝税抜定価×（画面.掛率÷100）
                    //        //定価が変わった場合のみ再表示
                    //        decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]) * bbl.Z_Set(GvSku.Rows[row].Cells[(int)JColNo.Rate].Value) / 100);
                    //        dtSKU.Rows[row - 1]["PriceWithoutTax"] = priceWithoutTax;
                    //    }
                    //    dtSKU.Rows[row - 1]["ITemName"] = dtM.Rows[0]["ITemName"].ToString();
                    //    dtSKU.Rows[row - 1]["SKUCD"] = dtM.Rows[0]["SKUCD"].ToString();
                    //    dtSKU.Rows[row - 1]["SizeName"] = dtM.Rows[0]["SizeName"].ToString();
                    //    dtSKU.Rows[row - 1]["ColorName"] = dtM.Rows[0]["ColorName"].ToString();
                    //    dtSKU.Rows[row - 1]["MakerItem"] = dtM.Rows[0]["MakerItem"].ToString();
                    //    dtSKU.Rows[row - 1]["BrandCD"] = dtM.Rows[0]["BrandCD"].ToString();
                    //    dtSKU.Rows[row - 1]["SportsCD"] = dtM.Rows[0]["SportsCD"].ToString();
                    //    dtSKU.Rows[row - 1]["SegmentCD"] = dtM.Rows[0]["SegmentCD"].ToString();
                    //    dtSKU.Rows[row - 1]["LastYearTerm"] = dtM.Rows[0]["LastYearTerm"].ToString();
                    //    dtSKU.Rows[row - 1]["LastSeason"] = dtM.Rows[0]["LastSeason"].ToString();
                    //    dtSKU.Rows[row - 1]["PriceOutTax"] = bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]);
                    //}                   

                    break;

                case (int)JColNo.Rate:
                    if (bbl.Z_Set(value) == 0)
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 明細チェック時のSKU内容表示
        /// </summary>
        /// <param name="row"></param>
        /// <param name="date"></param>
        private void DispFromSkuForDetail(int row, string date)
        {
            //改定日にてM_SKU再取得
            M_SKU_Entity mse = new M_SKU_Entity
            {
                AdminNO = dtSKU.Rows[row]["AdminNO"].ToString(),
                ChangeDate = bbl.FormatDate(date)
            };
            DataTable dtM = msbl.M_SKU_SelectForShiireTanka(mse);

            if (dtM.Rows.Count > 0)
            {
                //M_SKUより再表示
                if (bbl.Z_Set(dtSKU.Rows[row]["PriceOutTax"]) != bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]))
                {
                    //税抜発注額＝税抜定価×（画面.掛率÷100）
                    //定価が変わった場合のみ再表示
                    decimal priceWithoutTax = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]) * bbl.Z_Set(dtSKU.Rows[row]["Rate"]) / 100);
                    dtSKU.Rows[row]["PriceWithoutTax"] = priceWithoutTax;
                }
                dtSKU.Rows[row]["ITemName"] = dtM.Rows[0]["ITemName"].ToString();
                dtSKU.Rows[row]["SKUCD"] = dtM.Rows[0]["SKUCD"].ToString();
                dtSKU.Rows[row]["SizeName"] = dtM.Rows[0]["SizeName"].ToString();
                dtSKU.Rows[row]["ColorName"] = dtM.Rows[0]["ColorName"].ToString();
                dtSKU.Rows[row]["MakerItem"] = dtM.Rows[0]["MakerItem"].ToString();
                dtSKU.Rows[row]["BrandCD"] = dtM.Rows[0]["BrandCD"].ToString();
                dtSKU.Rows[row]["SportsCD"] = dtM.Rows[0]["SportsCD"].ToString();
                dtSKU.Rows[row]["SegmentCD"] = dtM.Rows[0]["SegmentCD"].ToString();
                dtSKU.Rows[row]["LastYearTerm"] = dtM.Rows[0]["LastYearTerm"].ToString();
                dtSKU.Rows[row]["LastSeason"] = dtM.Rows[0]["LastSeason"].ToString();
                dtSKU.Rows[row]["PriceOutTax"] = bbl.Z_Set(dtM.Rows[0]["PriceOutTax"]);
            }

            GvSku.Refresh();
        }

        /// <summary>
        /// 追加データ重複チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckDoubleForAdd(string ymd)
        {
            foreach (DataRow row in dtITEM.Rows)
            {
                if (String.Compare(row["MakerItem"].ToString(), mie.MakerItem, true) == 0
                    && row["ChangeDate"].ToString() == ymd
                    )
                    return false;
            }

            return true; 
        }

        /// <summary>
        /// 複写データ重複チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckDoubleForCopy(DataRow addRow)
        {
            foreach (DataRow row in dtITEM.Rows)
            {
                if (String.Compare(addRow["MakerItem"].ToString(), row["MakerItem"].ToString(), true) == 0
                && row["ChangeDate"].ToString() == copyControls[(int)CIndex.CopyDate].Text
                )

                return false;
            }     

            return true;
        }

        /// <summary>
        /// 重複チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckDouble(out int errRow)
        {
            errRow = -1;

            for (int row = 0; row < dtITEM.Rows.Count; row++)
            {
                for (int row2 = 0; row2 < dtITEM.Rows.Count; row2++)
                {
                    if (String.Compare(dtITEM.Rows[row]["MakerItem"].ToString(), dtITEM.Rows[row2]["MakerItem"].ToString(), true) == 0
                    && dtITEM.Rows[row]["ChangeDate"].ToString() == dtITEM.Rows[row2]["ChangeDate"].ToString()
                    && bbl.Z_Set(dtITEM.Rows[row]["TempKey"]) != bbl.Z_Set(dtITEM.Rows[row2]["TempKey"])
                    )
                    {
                        errRow = row;
                        return false;
                    }                       
                }
            }
            
            return true;
        }

        /// <summary>
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private M_ItemOrderPrice_Entity GetEntity()
        {
            mpe = new M_ItemOrderPrice_Entity
            {
                VendorCD = keyControls[(int)KIndex.VendorCD].Text,
                StoreCD =  rdoAllStores.Checked ? "0000" : cboStoreCD.SelectedValue.ToString(),
                BaseDate = keyControls[(int)KIndex.BaseDate].Text,
                DispKbn = rdoCurrent.Checked ? "1" : "2",

                //ヘッダ条件のみで取得
                //BrandCD = selectControls1[(int)SIndex.BrandCD].Text,
                //SportsCD = selectControls1[(int)SIndex.SportsCD].Text,
                //SegmentCD = selectControls1[(int)SIndex.SegmentCD].Text,
                //LastYearTerm = cboYearTerm1.SelectedIndex.Equals(-1) ? string.Empty : cboYearTerm1.SelectedValue.ToString(),
                //LastSeason = cboSeason1.SelectedIndex.Equals(-1) ? string.Empty : cboSeason1.SelectedValue.ToString(),
                //ChangeDate = selectControls1[(int)SIndex.ChangeDate].Text,
                //MakerItem = selectControls1[(int)SIndex.MakerItem].Text,
            };

            return mpe;
        }


        protected override void ExecSec()
        {
            try
            {
                //全明細チェック
                if (!Chk_All())
                {

                    return;
                }

                //Ｑ１０１	
                if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                    return;

                mpe = new M_ItemOrderPrice_Entity
                {
                    VendorCD = keyControls[(int)KIndex.VendorCD].Text,
                    Operator = InOperatorCD,
                    PC = InPcID
                };

                //更新日付を最新にする
                this.SetUpdateDate();

                //更新処理
                bool ret = msbl.PRC_MasterTouroku_ShiireTanka(mpe, dtOldITEM, dtOldSKU, dtITEM, dtSKU);

                if (ret)
                    bbl.ShowMessage("I101");
                else
                    bbl.ShowMessage("S002");

                //ITEMモードに
                this.ChangeOperationMode(EOpeMode.ITEM);

                this.Scr_Clr(0);
                this.Scr_Lock(1);
                keyControls[(int)KIndex.VendorCD].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        /// <summary>
        /// モード切替
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeOperationMode(EOpeMode mode)
        {
            mOpeMode = mode;

            switch (mOpeMode)
            {
                case EOpeMode.ITEM:
                    ModeColor = Color.FromArgb(255, 192, 0);
                    ModeText = "ITEM";
                    break;

                case EOpeMode.SKU:
                    ModeColor = Color.FromArgb(255, 255, 0);
                    ModeText = "SKU";
                    break;
            }

            Btn_F10.Enabled = mode == EOpeMode.ITEM;
            btnInput.Enabled = mode == EOpeMode.ITEM;
            pnlAddDetail.Enabled = mode == EOpeMode.ITEM;
            pnlSelect2.Enabled = mode == EOpeMode.ITEM;
            pnlCopy.Enabled = mode == EOpeMode.ITEM;
            
            GvItem.Visible = mode == EOpeMode.ITEM;
            GvSku.Visible = mode == EOpeMode.SKU;

        }

        /// <summary>
        /// 画面クリア(0:全項目、1:KEY部以外)
        /// </summary>
        /// <param name="Kbn"></param>
        private void Scr_Clr(short Kbn)
        {
            if (Kbn == 0)
            {
                foreach (Control ctl in keyControls)
                {
                    this.ctlClr(ctl);
                }

                foreach (Control ctl in keyLabels)
                {
                    this.ctlClr(ctl);
                }

                rdoCurrent.Checked = true;
                rdoAllStores.Checked = true;
                rdoITEM.Checked = true;                

                string ymd = bbl.GetDate();
                txtBaseDate.Text = ymd;
                mOldBaseDate = ymd;

                GvItem.DataSource = null;
                GvSku.DataSource = null;

                GvItem.Visible = true;
                GvSku.Visible = false;
            }
            
            foreach (Control ctl in selectControls1)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in selectLabels1)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in selectControls2)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in selectLabels2)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in addControls)
            {
                this.ctlClr(ctl);
            }

            foreach (Control ctl in addLabels)
            {
                this.ctlClr(ctl);
            }
            foreach (Control ctl in copyControls)
            {
                this.ctlClr(ctl);
            }

            mOldRate = 0;
            mOldItemCd = string.Empty;

        }

        /// <summary>
        /// コントロールのクリア
        /// </summary>
        /// <param name="ctl"></param>
        private void ctlClr(Control ctl)
        {
            if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
            {
                ((CKM_Controls.CKM_ComboBox)ctl).SelectedIndex = -1;
            }
            else if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_Button)) || ctl.GetType().Equals(typeof(Button)))
            {

            }
            else if (ctl.GetType().Equals(typeof(CKM_SearchControl)))
            {
                ((CKM_SearchControl)ctl).LabelText = "";
            }
            else
            {
                ctl.Text = "";
            }
        }

        /// <summary>
        /// 画面入力制御
        /// </summary>
        /// <param name="kbn">0：ヘッダ入力不可（明細入力可） 1:明細入力不可（ヘッダ入力可）</param>
        private void Scr_Lock(short kbn)
        {
            // ヘッダ部
            foreach (Control ctl in keyControls)
            {
                ctl.Enabled = kbn == 0 ? false : true;
            }
            cboStoreCD.Enabled = kbn == 0 ? false : rdoIndividualStore.Checked;
            scVendor.BtnSearch.Enabled = kbn == 0 ? false : true;
            BtnSubF11.Enabled = kbn == 0 ? false : true;

            foreach (Control ctl in radioButtons)
            {
                int index = Array.IndexOf(radioButtons, ctl);
                if (index < (int)RBIndex.RdoITEM)
                {
                    ctl.Enabled = kbn == 0 ? false : true;
                }
                else
                {
                    ctl.Enabled = kbn == 0 ? true : false;
                }
                
            }

            // 明細部
            foreach (Control ctl in selectControls1)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            foreach (Control ctl in addControls)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            foreach (Control ctl in selectControls2)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            foreach (Control ctl in copyControls)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            foreach (Control ctl in searchButtons)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            foreach (Control ctl in buttons)
            {
                ctl.Enabled = kbn == 0 ? true : false;
            }

            Btn_F10.Enabled = kbn == 0 ? true : false;
            Btn_F12.Enabled = kbn == 0 ? true : false;
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
                case 5: //F6:キャンセル
                    {
                        //Ｑ００４				
                        if (bbl.ShowMessage("Q004") != DialogResult.Yes)
                            return;

                        this.ChangeOperationMode(EOpeMode.ITEM);
                        this.Scr_Clr(0);
                        this.Scr_Lock(1);
                        keyControls[(int)KIndex.VendorCD].Focus();
                        break;
                    }
                case 9://F10
                    {
                        this.ExecInput();
                        break;

                    }
                    

                case 8: //F9:検索
                    break;

                case 11:    //F12:登録
                    {
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
            try
            {
                //DeleteExclusive(dtOrder);
            }
            catch(Exception ex)
            {
                //例外は無視する
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            this.Close();
            //アプリケーションを終了する
            //Application.Exit();
            //System.Environment.Exit(0);
        }

        #region "内部イベント"
        private void KeyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(keyControls, sender);
                    bool ret = CheckKey(index);
                    if (ret)
                    {
                        if (keyControls.Length  > index)
                        {
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            ProcessTabKey(!e.Shift);                          
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

        private void KeyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectControl1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //抽出条件のコントロールではない場合、選択のコントロール
                    int index = Array.IndexOf(selectControls1, sender);

                    bool ret = CheckSelect(index, 1);
                    if (ret)
                    {
                        if (selectControls1.Length > index)
                        {
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            ProcessTabKey(!e.Shift);
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

        private void SelectControl1_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectControl2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    //抽出条件のコントロールではない場合、選択のコントロール
                    int index = Array.IndexOf(selectControls2, sender);

                    bool ret = CheckSelect(index, 2);
                    if (ret)
                    {
                        if (selectControls2.Length > index)
                        {
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            ProcessTabKey(!e.Shift);
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

        private void SelectControl2_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void AddControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(addControls, sender);

                    bool ret = CheckAdd(index, false);
                    if (ret)
                    {

                        if (index == (int)AIndex.PriceWithoutTax)
                            btnAdd.Focus();
                        else if (addControls.Length - 1 > index)
                        {
                            if (addControls[index + 1].CanFocus)
                                addControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
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

        private void AddControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void CopyControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    int index = Array.IndexOf(copyControls, sender);
                    bool ret = CheckCopy(index);
                    if (ret)
                    {
                        if (index == (int)CIndex.CopyRate)
                            btnCopy.Focus();
                        else if (copyControls.Length - 1 > index)
                        {
                            if (copyControls[index + 1].CanFocus)
                                copyControls[index + 1].Focus();
                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                ProcessTabKey(!e.Shift);
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

        private void CopyControl_Enter(object sender, EventArgs e)
        {
            try
            {
                previousCtrl = this.ActiveControl;
                //SetFuncKeyAll(this, "111111001011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {  

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Array.IndexOf(radioButtons, sender);
                if (index == (int)RBIndex.RdoAllStores || index == (int)RBIndex.RdoIndividualStore)
                {
                    cboStoreCD.Enabled = rdoIndividualStore.Checked;
                    if (rdoAllStores.Checked)
                        cboStoreCD.SelectedIndex = -1;
                }
                else if (index == (int)RBIndex.RdoITEM || index == (int)RBIndex.RdoSKU)
                {
                    EOpeMode mode = rdoITEM.Checked ? EOpeMode.ITEM : EOpeMode.SKU;
                    ChangeOperationMode(mode);
                    this.DispFromTempTable();
                    selectControls1[(int)SIndex.BrandCD].Focus();
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

        private void GvItem_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //ヘッダをクリックすると背景色が元に戻るため、再セット
            this.Set_GridStyle();
        }

        private void GvSku_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Set_GridStyle();
        }

        private void GvItem_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                var dgv = (DataGridView)sender;
                if (dgv.IsCurrentCellDirty)
                {
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }        

        private void GvSku_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                var dgv = (DataGridView)sender;
                if (dgv.IsCurrentCellDirty)
                {
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void GvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            switch (col)
            {
                case (int)IColNo.ChangeDate:
                    string ymd = bbl.FormatDate(GvItem.Rows[row].Cells[col].Value.ToString());
                    if (bbl.CheckDate(ymd))
                    {
                        GvItem.Rows[row].Cells[col].Value = ymd;

                        //M_ITEMより再表示
                        this.DispFromItemForDetail(row, ymd);

                        dtITEM.Rows[row]["UpdateOperator"] = InOperatorCD;
                        dtITEM.Rows[row]["UpdateDateTime"] = mUpdateDateTime;
                    }

                    break;

                case (int)IColNo.Rate:

                    if (bbl.Z_Set(GvItem.Rows[row].Cells[col].Value) != bbl.Z_Set(dtITEM.Rows[row]["OldRate"]))
                    {
                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        GvItem.Rows[row].Cells[(int)IColNo.PriceWithoutTax].Value = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(GvItem.Rows[row].Cells[(int)IColNo.PriceOutTax].Value)
                                                                                * bbl.Z_Set(GvItem.Rows[row].Cells[col].Value) / 100);
                    }
                    dtITEM.Rows[row]["OldRate"] = bbl.Z_Set(GvItem.Rows[row].Cells[col].Value);

                    dtITEM.Rows[row]["UpdateOperator"] = InOperatorCD;
                    dtITEM.Rows[row]["UpdateDateTime"] = mUpdateDateTime;

                    break;
            }

        }

        private void GvSku_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            switch (col)
            {
                case (int)JColNo.ChangeDate:
                    string ymd = bbl.FormatDate(GvSku.Rows[row].Cells[col].Value.ToString());
                    if (bbl.CheckDate(ymd))
                    {
                        GvSku.Rows[row].Cells[col].Value = ymd;

                        //M_SKUより再表示
                        this.DispFromSkuForDetail(row, ymd);

                        dtSKU.Rows[row]["UpdateOperator"] = InOperatorCD;
                        dtSKU.Rows[row]["UpdateDateTime"] = mUpdateDateTime;
                    }

                    break;

                case (int)JColNo.Rate:
                    if (bbl.Z_Set(GvSku.Rows[row].Cells[col].Value) != bbl.Z_Set(dtSKU.Rows[row]["OldRate"]))
                    {
                        //税抜発注額＝税抜定価×（画面.掛率÷100）
                        GvSku.Rows[row].Cells[(int)JColNo.PriceWithoutTax].Value = GetResultWithHasuKbn((int)HASU_KBN.KIRISUTE, bbl.Z_Set(GvSku.Rows[row].Cells[(int)JColNo.PriceOutTax].Value)
                                                                                * bbl.Z_Set(GvSku.Rows[row].Cells[col].Value) / 100);
                    }
                    dtSKU.Rows[row]["OldRate"] = bbl.Z_Set(GvSku.Rows[row].Cells[col].Value);

                    dtSKU.Rows[row]["UpdateOperator"] = InOperatorCD;
                    dtSKU.Rows[row]["UpdateDateTime"] = mUpdateDateTime;

                    break;
            }

        }

        private void GvItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        private void GvSku_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        /// <summary>
        /// ヘッダ部表示ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubF11_Click(object sender, EventArgs e)
        {
            this.ExecDisp();
        }

        /// <summary>
        /// 【抽出条件】表示ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            this.ExecDispFromWork();
        }

        /// <summary>
        /// 追加ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ExecAdd();
        }

        /// <summary>
        /// 【選択】表示ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.ExecSelect();
        }

        /// <summary>
        /// 全選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            this.SetAllCheckValue(true);
        }

        /// <summary>
        /// 全解除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllRelaese_Click(object sender, EventArgs e)
        {
            this.SetAllCheckValue(false);
        }

        /// <summary>
        /// 複写ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.ExecCopy();
        }

        /// <summary>
        /// 変更ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.ExecUpdate();
        }

        /// <summary>
        /// 削除ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ExecDelete();
        }

        /// <summary>
        /// 取込ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInput_Click(object sender, EventArgs e)
        {
            this.ExecInput();
        }

        #endregion


    }
}








