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

namespace SyukkaShijisho
{
    /// <summary>
    /// SyukkaShijisho 出荷指示書
    /// </summary>
    internal partial class SyukkaShijisho : FrmMainForm
    {
        private const string ProID = "SyukkaShijisho";
        private const string ProNm = "出荷指示書";

        private enum EIndex : int
        {
            SoukoCD,

            ChkMihakko,
            DeliveryPlanDate,
            Chk1,
            Chk2,
            Chk3,
            Chk4,
            Chk5,

            CarrierCD,
            ChkSaihakko,
            InstructionNO,
            ChkNohinSeikyu,
            COUNT
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private ShukkaShijiTouroku_BL ssbl;
        private D_Instruction_Entity die;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        private string mOldInstructionNO = "";    //排他処理のため使用
        

        public SyukkaShijisho()
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

                string ymd = bbl.GetDate();
                ssbl = new ShukkaShijiTouroku_BL();
                CboSouko.Bind(ymd);
                CboCarrierCD.Bind(ymd);
          
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
            detailControls = new Control[] { CboSouko,ChkMihakko, ckM_TextBox1 ,ckM_CheckBox1,ckM_CheckBox2,ckM_CheckBox3,ckM_CheckBox4,ckM_CheckBox5
                    , CboCarrierCD, ChkSaihakko, ScInstructionNO.TxtCode, ChkNohinSeikyu
                         };
            detailLabels = new Control[] { ScInstructionNO };
            searchButtons = new Control[] { ScInstructionNO.BtnSearch};

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
        }
       

        private bool SelectAndInsertExclusive(string InstructionNO)
        {
            DeleteExclusive();

            //排他Tableに該当番号が存在するとError
            //[D_Exclusive]
            Exclusive_BL ebl = new Exclusive_BL();
            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.SyukkaShiji,
                Number = InstructionNO,
                Program = this.InProgramID,
                Operator = this.InOperatorCD,
                PC = this.InPcID
            };

            DataTable dt = ebl.D_Exclusive_Select(dee);

            if (dt.Rows.Count > 0)
            {
                bbl.ShowMessage("S004", dt.Rows[0]["Program"].ToString(),dt.Rows[0]["Operator"].ToString());
                return false;
            }
            else
            {
                bool ret = ebl.D_Exclusive_Insert(dee);
                mOldInstructionNO = InstructionNO;
                return ret;
            }
        }
        /// <summary>
        /// 排他処理データを削除する
        /// </summary>
        private void DeleteExclusive(DataTable dtForUpdate = null)
        {
            if (mOldInstructionNO == "" && dtForUpdate == null)
                return;

            Exclusive_BL ebl = new Exclusive_BL();

            if (dtForUpdate != null)
            {
                foreach (DataRow dr in dtForUpdate.Rows)
                {
                    D_Exclusive_Entity de = new D_Exclusive_Entity();
                    de.DataKBN = (int)Exclusive_BL.DataKbn.SyukkaShiji;
                    de.Number = dr["no"].ToString();

                    ebl.D_Exclusive_Delete(de);
                }
                return;
            }

            D_Exclusive_Entity dee = new D_Exclusive_Entity
            {
                DataKBN = (int)Exclusive_BL.DataKbn.Mitsumori,
                Number = mOldInstructionNO,
            };

            bool ret = ebl.D_Exclusive_Delete(dee);

            mOldInstructionNO = "";
        }
        /// <summary>
        /// 出荷指示データ取得処理
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
            //未発行分選択時、	通常～店舗間移動(CheckBox)のすべてがOFFの時、Error																
            bool okFlg = true;
            if (ChkMihakko.Checked)
            {
                okFlg = false;
                for (int i = (int)EIndex.Chk1; i <= (int)EIndex.Chk5; i++)
                {
                    if (((CheckBox)detailControls[i]).Checked)
                        okFlg = true;
                }
            }
            if (!okFlg)
            { 
                //Ｅ１８０
                bbl.ShowMessage("E180");
                detailControls[(int)EIndex.Chk1].Focus();
                return null;
            }

            //[D_Ins_SelectForPrint]
            die = GetEntity();

            DataTable dt = ssbl.D_Instruction_SelectForPrint(die);

            //以下の条件でデータが存在しなければエラー (Error if record does not exist)Ｅ１３３
            if (dt.Rows.Count == 0)
            {
                bbl.ShowMessage("E133");
                previousCtrl.Focus();
                return null;
            }
            else
            {   //明細にデータをセット
                int i = 0;
                dtForUpdate = new DataTable();
                dtForUpdate.Columns.Add("no", Type.GetType("System.String"));

                foreach (DataRow row in dt.Rows)
                {
                    if (mOldInstructionNO != row["InstructionNO"].ToString())
                    {
                        bool ret = SelectAndInsertExclusive(row["InstructionNO"].ToString());
                        if (!ret)
                            return null;

                        i++;
                        mOldInstructionNO = row["InstructionNO"].ToString();
                        // データを追加
                        DataRow rowForUpdate;
                        rowForUpdate = dtForUpdate.NewRow();
                        rowForUpdate["no"] = row["InstructionNO"].ToString();
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
                SyukkaShijisho_Report Report = new SyukkaShijisho_Report();

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
                ssbl.D_Instruction_Update(die, dtForUpdate);

                //ログファイルへの更新
                bbl.L_Log_Insert(Get_L_Log_Entity());
            }
            finally
            {
                DeleteExclusive(dtForUpdate);
            }

            //更新後画面そのまま
            detailControls[1].Focus();
        }

        /// <summary>
        /// get Log information
        /// print log
        /// </summary>
        private L_Log_Entity Get_L_Log_Entity()
        {
            string item = detailControls[0].Text;
            for (int i = 1; i < (int)EIndex.COUNT; i++)
            {
                switch (i)
                {
                    case (int)EIndex.Chk1:
                    case (int)EIndex.Chk2:
                    case (int)EIndex.Chk3:
                    case (int)EIndex.Chk4:
                    case (int)EIndex.Chk5:
                    case (int)EIndex.ChkMihakko:
                    case (int)EIndex.ChkSaihakko:
                    case (int)EIndex.ChkNohinSeikyu:
                        if (((CheckBox)detailControls[i]).Checked)
                        {
                            item += "," + detailControls[i].Text;
                        }
                        else
                        {
                            item += ",";
                        }
                        break;
                    default:
                        item += "," + detailControls[i].Text;
                        break;
                }
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
            bool ret;
            switch (index)
            {
                case (int)EIndex.DeliveryPlanDate:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //入力可能時
                        if (detailControls[index].Enabled)
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                        else
                            return true;
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

                case (int)EIndex.InstructionNO:                    
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //印刷対象　再発行分CheckBox＝ONの時
                     //入力必須(Entry required)
                        if (ChkSaihakko.Checked)
                        {
                            //Ｅ１０２
                            bbl.ShowMessage("E102");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    //[D_Instruction_Select]
                    D_Instruction_Entity de = new D_Instruction_Entity
                    {
                        InstructionNO = detailControls[index].Text
                        ,DeliverySoukoCD = CboSouko.SelectedValue.ToString().Equals("-1") ? string.Empty : CboSouko.SelectedValue.ToString(),
                    };
                    ret = ssbl.D_Instruction_Select(de);

                    if (!ret)
                    {
                        //該当データなし
                        bbl.ShowMessage("E128");
                        return false;
                    }

                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;                    
            }

            return true;
        }

        private D_Instruction_Entity GetEntity()
        {
            die = new D_Instruction_Entity
            {
                DeliveryPlanDate = detailControls[(int)EIndex.DeliveryPlanDate].Text,
                DeliverySoukoCD = CboSouko.SelectedValue.ToString().Equals("-1") ? string.Empty : CboSouko.SelectedValue.ToString(),
                InstructionNO = detailControls[(int)EIndex.InstructionNO].Text,
                CarrierCD = CboCarrierCD.SelectedIndex > 0 ? CboCarrierCD.SelectedValue.ToString(): string.Empty,
                UpdateOperator = InOperatorCD,
                PC = InPcID 
            };

            if (ChkMihakko.Checked)
                die.ChkMihakko = 1;
            else
                die.ChkMihakko = 0;

            if (ChkSaihakko.Checked)
                die.ChkSaihakko = 1;
            else
                die.ChkSaihakko = 0;

            if (ChkNohinSeikyu.Checked)
                die.ChkNohinSeikyu = 1;
            else
                die.ChkNohinSeikyu = 0;

            if (ckM_CheckBox1.Checked)
                die.Chk1 = 1;
            else
                die.Chk1 = 0;

            if (ckM_CheckBox2.Checked)
                die.Chk2 = 1;
            else
                die.Chk2 = 0;

            if (ckM_CheckBox3.Checked)
                die.Chk3 = 1;
            else
                die.Chk3 = 0;

            if (ckM_CheckBox4.Checked)
                die.Chk4 = 1;
            else
                die.Chk4 = 0;

            if (ckM_CheckBox5.Checked)
                die.Chk5 = 1;
            else
                die.Chk5 = 0;

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
                    ((CheckBox)ctl).Checked = true;
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
            string ymd = ssbl.GetDate();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = ymd
            };
            Staff_BL bl = new Staff_BL();
            bool ret = bl.M_Staff_Select(mse);
            if (ret)
            {
                //[M_Souko_Search]
                M_Souko_Entity me = new M_Souko_Entity
                {
                    StoreCD = mse.StoreCD,
                    SoukoType = "3",
                    ChangeDate = ymd,
                    DeleteFlg = "0",
                    searchType = "2"
                };

                DataTable mdt = ssbl.M_Souko_Search(me);
                if (mdt.Rows.Count > 0)
                {
                    CboSouko.SelectedValue = mdt.Rows[0]["SoukoCD"];
                }
                else
                {
                    bbl.ShowMessage("E133");
                    EndSec();
                }
            }

            ChkSaihakko.Checked = false;
            detailControls[(int)EIndex.InstructionNO].Enabled = false;
            detailControls[(int)EIndex.DeliveryPlanDate].Text = ymd;

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
                            ScInstructionNO.BtnSearch.Enabled = Kbn == 0 ? true : false;

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
                            if(index== (int)EIndex.COUNT-1)
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
                switch(index)
                    {
                    case (int)EIndex.InstructionNO:
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
        private void ChkMihakko_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // 印刷対象未発行分

                //チェックがONのときのみ入力可
                detailControls[(int)EIndex.DeliveryPlanDate].Enabled = ChkMihakko.Checked;
                detailControls[(int)EIndex.CarrierCD].Enabled = ChkMihakko.Checked;
                //通常～店舗間移動(CheckBox)を入力可に 初期値：ON
                for (int i = (int)EIndex.Chk1; i <= (int)EIndex.Chk5; i++)
                {
                    detailControls[i].Enabled = ChkMihakko.Checked;
                    ((CheckBox)detailControls[i]).Checked = ChkMihakko.Checked;
                }

                detailControls[(int)EIndex.InstructionNO].Enabled= !ChkMihakko.Checked;
                ScInstructionNO.BtnSearch.Enabled = !ChkMihakko.Checked;
                ChkSaihakko.Checked = !ChkMihakko.Checked;

                if (ChkMihakko.Checked)
                {
                    //CheckBox＝ONにした時
                    //出荷予定日 を入力可に                   初期値：Todayをセット
                    detailControls[(int)EIndex.DeliveryPlanDate].Text = bbl.GetDate();
                    detailControls[(int)EIndex.InstructionNO].Text = "";
                }
                else
                {
                    detailControls[(int)EIndex.DeliveryPlanDate].Text = "";
                    CboCarrierCD.SelectedIndex = -1;
                }


            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkSaihakko_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkSaihakko.Checked)
                {
                    //CheckBox＝ONにした時
                    ChkMihakko.Checked = false;
                }
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








