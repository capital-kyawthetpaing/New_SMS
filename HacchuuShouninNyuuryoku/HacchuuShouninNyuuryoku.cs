using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using BL;
using Entity;
using Base.Client;
using Search;
using GridBase;

namespace HacchuuShouninNyuuryoku
{
    /// <summary>
    /// HacchuuShouninNyuuryoku 発注承認入力
    /// </summary>
    internal partial class HacchuuShouninNyuuryoku : FrmMainForm
    {
        private const string ProID = "HacchuuShouninNyuuryoku";
        private const string ProNm = "発注承認入力";
        private const short mc_L_END = 3; // ロック用
        private const string Hacchuu = "HacchuuNyuuryoku.exe";

        private enum EIndex : int
        {
            StoreCD,
            OrderDateFrom,
            OrderDateTo,
            CheckBox1,
            CheckBox2,
            CheckBox3,
           
        }

        private enum EColNo : int
        {
            Btn,     //
            OrderNO,
            OrderDate,
            Status,
            LastDay,
            LastOpe,
            Store,
            Operator,
            Vendor,
            SKUName,

            COUNT
        }

        private Control[] detailControls;
       
        private HacchuuShouninNyuuryoku_BL mibl;
        private D_Order_Entity doe;
        
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        
        private int W_ApprovalStageFLG;
        //private int mTaxFractionKBN;


        public HacchuuShouninNyuuryoku()
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
                Btn_F6.Text = "";
                Btn_F9.Text = "";

                //コンボボックス初期化
                string ymd = bbl.GetDate();
                mibl = new HacchuuShouninNyuuryoku_BL();
                CboStoreCD.Bind(ymd);
                
                string stores= GetAllAvailableStores();

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

                //承認用データを抽出(発注入力と同じ)
                HacchuuNyuuryoku_BL hbl = new HacchuuNyuuryoku_BL();
                W_ApprovalStageFLG = hbl.GetApprovalStageFLG(InOperatorCD);
                //この一時テーブルにレコードがない＝承認する立場にない。←画面に表示すべきデータがないというこちになります。
                if(W_ApprovalStageFLG == 0)
                {
                    EndSec();
                    return;
                }

                detailControls[(int)EIndex.OrderDateFrom].Text = ymd;
                detailControls[(int)EIndex.OrderDateTo].Text = ymd;

                detailControls[(int)EIndex.StoreCD].Focus(); 
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
            detailControls = new Control[] { CboStoreCD, ckM_TextBox1, ckM_TextBox2, ckM_CheckBox1, ckM_CheckBox2, ckM_CheckBox3 };

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
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
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
                            CboStoreCD.Focus();
                            return false;
                        }
                    }

                    break;
                case (int)EIndex.OrderDateFrom:
                case (int)EIndex.OrderDateTo:
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
                    if (index == (int)EIndex.OrderDateTo)
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
            DataTable dt = mibl.D_Order_SelectAllForSyonin(doe);

            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
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
                //発注入力を起動します
                //EXEが存在しない時ｴﾗｰ
                // 実行モジュールと同一フォルダのファイルを取得
                System.Uri u = new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string filePath = System.IO.Path.GetDirectoryName(u.LocalPath) + @"\" + Hacchuu;
                if (System.IO.File.Exists(filePath))
                {
                    string  OrderNO = GvDetail.CurrentRow.Cells["colOrderNO"].Value.ToString();
                    string cmdLine = InCompanyCD + " " + InOperatorCD + " " + InPcID + " " + OrderNO;
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
        /// 画面情報をセット
        /// </summary>
        /// <returns></returns>
        private D_Order_Entity GetEntity()
        {
            doe = new D_Order_Entity();
            doe.StoreCD = CboStoreCD.SelectedValue.ToString();

            doe.OrderDateFrom = detailControls[(int)EIndex.OrderDateFrom].Text;
            doe.OrderDateTo = detailControls[(int)EIndex.OrderDateTo].Text;

            HacchuuNyuuryoku_BL hbl = new HacchuuNyuuryoku_BL();
            W_ApprovalStageFLG = hbl.GetApprovalStageFLG(InOperatorCD, CboStoreCD.SelectedValue.ToString());
            doe.ApprovalStageFLG = W_ApprovalStageFLG.ToString();

            if (ckM_CheckBox1.Checked)
                doe.Misyonin = 1;
            else
                doe.Misyonin = 0;

            if (ckM_CheckBox2.Checked)
                doe.SyoninZumi = 1;
            else
                doe.SyoninZumi = 0;

            if (ckM_CheckBox3.Checked)
                doe.Kyakka = 1;
            else
                doe.Kyakka = 0;

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

            ckM_CheckBox1.Checked = true;
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
                        if (index == (int)EIndex.CheckBox3)
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
        
        //private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (e.KeyCode == Keys.Enter)
        //            if (e.Control == false)
        //            {
        //                this.ExecSec();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        //エラー時共通処理
        //        MessageBox.Show(ex.Message);
        //        //EndSec();
        //    }
        //}

        private void GvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == (int)EColNo.Btn)
                {
                    this.ExecSec();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //セルの列を確認
            if (e.ColumnIndex == (int)EColNo.Status )
            {
                //セルの値により、背景色を変更する
                switch (e.Value)
                {
                    case "承認済":
                        e.CellStyle.BackColor = ClsGridBase.GridColor;
                        break;
                    case "承認中":
                    case "申請":
                        e.CellStyle.BackColor = Color.PaleGoldenrod;
                        break;
                    case "却下":
                        e.CellStyle.BackColor = Color.Pink;
                        break;

                }
            }
        }

        #endregion

    }
}








