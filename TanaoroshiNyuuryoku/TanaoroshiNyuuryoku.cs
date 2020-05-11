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
            InventoryDate
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
        
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        

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

                //起動時共通処理
                base.StartProgram();
                Btn_F9.Text = "";
                Btn_F9.Enabled = false;
                Btn_F12.Text = "F12:登録";
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

                CboSoukoCD.Bind(ymd, mse.StoreCD);
                
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
            detailControls = new Control[] { CboSoukoCD, ckM_TextBox1 };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
        
        /// <summary>
        /// コードチェック
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckDetail(int index)
        {

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

            }

            return true;

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
                    }
                }
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

            GvDetail.DataSource = null;
            GvDetail.Enabled = false;

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
                previousCtrl = this.ActiveControl;
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








