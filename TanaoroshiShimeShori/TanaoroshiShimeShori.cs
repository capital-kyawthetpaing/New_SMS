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

namespace TanaoroshiShimeShori
{
    /// <summary>
    /// TanaoroshiShimeShori 棚卸締処理
    /// </summary>
    internal partial class TanaoroshiShimeShori : FrmMainForm
    {
        private const string ProID = "TanaoroshiShimeShori";
        private const string ProNm = "棚卸締処理";

        private enum EIndex : int
        {
            Syori,
            SoukoCD,
            InventoryDate,
            RackSt,
            RackEd,
            COUNT
        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private Tanaoroshi_BL tabl;
        private string InStoreCD;
        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避

        
        public TanaoroshiShimeShori()
        {
            InitializeComponent();
                    }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                base.InProgramID = ProID;
                base.InProgramNM=ProNm;

                this.SetFunctionLabel(EProMode.BATCH);
                this.InitialControlArray();
                
                //起動時共通処理
                base.StartProgram();

                Btn_F9.Text = "";
                Btn_F9.Enabled = false;

                tabl = new Tanaoroshi_BL();
                string ymd = bbl.GetDate();
                
                //スタッフマスター(M_Staff)に存在すること
                //[M_Staff]
                M_Staff_Entity mse = new M_Staff_Entity
                {
                    StaffCD = InOperatorCD,
                    ChangeDate = ymd
                };
                Staff_BL bl = new Staff_BL();
                bool ret = bl.M_Staff_Select(mse);

                InStoreCD = mse.StoreCD;

                CboSoukoName.Bind(ymd, InOperatorCD);
                BindCombo("KBN", "Name");
                
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
        private void BindCombo(string key, string value)
        {
            DataTable dt = new DataTable();
            // 列を追加します。
            dt.Columns.Add(key);
            dt.Columns.Add(value);
            DataRow datarow = dt.NewRow();
            datarow[key] = "1";
            datarow[value] = "棚卸締";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "棚卸締キャンセル";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "棚卸確定";
            dt.Rows.Add(datarow);

            DataRow dr = dt.NewRow();
            dr[key] = "-1";
            dt.Rows.InsertAt(dr, 0);
            cboSyori.DataSource = dt;
            cboSyori.DisplayMember = value;
            cboSyori.ValueMember = key;
        }
        protected override void ExecDisp()
        {
            D_InventoryProcessing_Entity dbe = new D_InventoryProcessing_Entity
            {
                SoukoCD = CboSoukoName.SelectedIndex > 0 ? CboSoukoName.SelectedValue.ToString():""
            };

            DataTable dt = tabl.D_InventoryProcessing_SelectAll(dbe);
            GvDetail.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                //GvDetail.Focus();
            }
            else
            {
                //ssbl.ShowMessage("E128");
            }
        }

        protected override void ExecSec()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i, false) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            bool ret;
            int syori = Convert.ToInt16(cboSyori.SelectedValue);
            string msgId = "";

            D_InventoryProcessing_Entity de = new D_InventoryProcessing_Entity
            {
                SoukoCD = CboSoukoName.SelectedValue.ToString(),
                InventoryDate = detailControls[(int)EIndex.InventoryDate].Text,
                FromRackNO = detailControls[(int)EIndex.RackSt].Text,
                ToRackNO = detailControls[(int)EIndex.RackEd].Text,
                Syori = syori.ToString(),
                StoreCD = InStoreCD,
                Operator = InOperatorCD,
                PC = InPcID
            };

            switch (syori)
            {
                //処理：棚卸締の場合
                case 1:
                    msgId = "Q101";
                    //棚卸済みデータのチェック
                    //以下の条件でデータが存在すれば、エラー
                    //①	※棚卸済み	
                    //[D_InventoryControl_Select]
                    ret = tabl.D_InventoryControl_Select(de);
                    if(ret)
                    {
                        bbl.ShowMessage("S019");
                        return;
                    }

                    //以下の条件でデータが存在しない時、エラー
                    //②	※倉庫棚番マスタ存在チェック	
                    ret = tabl.M_Location_SelectForTanaoroshi(de);
                    if (!ret)
                    {
                        bbl.ShowMessage("S013");
                        return;
                    }
                    break;

                case 2:
                case 3:
                    msgId = "Q103";
                    if (syori.Equals(3))
                        msgId = "Q104";

                    //以下の条件でデータが存在しない時、エラー		
                    //①	※棚卸未処理
                    de.InventoryKBN = "";
                    ret = tabl.D_InventoryProcessing_Select(de);
                    if (!ret)
                    {       
                        //InventoryKBN<>1の時エラー
                        if (!de.InventoryKBN.Equals("1"))
                        {
                            bbl.ShowMessage("S020");
                            return;
                        }
                    }

                    //以下の条件でデータが存在すれば、エラー		
                    //②	※棚卸確定済み
                    de.InventoryKBN = "3";
                    ret = tabl.D_InventoryProcessing_Select(de);
                    if (ret)
                    {
                        bbl.ShowMessage("S021");
                        return;
                    }
                    //以下の条件でデータを取得		
                    //③	※棚卸キャンセル済みまたは確定済み
                    de.InventoryKBN = "";
                    ret = tabl.D_InventoryProcessing_Select(de);
                    if (ret)
                    {
                        //InventoryKBN＝2,3の時エラー
                        if (de.InventoryKBN.Equals("2") )
                        {
                            bbl.ShowMessage("S020");
                            return;
                        }
                        else if (de.InventoryKBN.Equals("3"))
                        {
                            bbl.ShowMessage("S021");
                            return;
                        }
                    }
                    break;
            }

            if (bbl.ShowMessage(msgId) != DialogResult.Yes)
            {
                return;
            }

            //更新処理
            tabl.D_InventoryProcessing_Exec(de);

            ExecDisp();

            detailControls[(int)EIndex.Syori].Focus();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { cboSyori, CboSoukoName, ckM_TextBox1, ckM_TextBox3, ckM_TextBox2 };
            detailLabels = new Control[] {  };
            searchButtons = new Control[] { };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                ctl.Enter += new System.EventHandler(DetailControl_Enter);
            }
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

                    detailControls[index].Text = tabl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!tabl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        tabl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    break;

                case (int)EIndex.Syori:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        cboSyori.MoveNext = false;
                        return false;
                    }
                    break;

                case (int)EIndex.SoukoCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        CboSoukoName.MoveNext = false;
                        return false;
                    }
                    //ExecDisp();
                    break;
                case (int)EIndex.RackEd:
                    if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                        if (result < 0)
                        {
                            bbl.ShowMessage("E197");
                            detailControls[index].Focus();
                            return false;
                        }
                    }
                    break;

            }

            return true;
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
            string ymd = tabl.GetDate();

            cboSyori.SelectedValue = 1;
            if(CboSoukoName.Items.Count > 1)
                CboSoukoName.SelectedIndex = 1;

            detailControls[(int)EIndex.InventoryDate].Text = ymd;
            
            detailControls[(int)EIndex.Syori].Focus();
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
                            if (detailControls[index + 1].CanFocus)
                                detailControls[index + 1].Focus();

                            else
                                //あたかもTabキーが押されたかのようにする
                                //Shiftが押されている時は前のコントロールのフォーカスを移動
                                this.ProcessTabKey(!e.Shift);
                        }
                        else
                        {
                            Btn_F1.Focus();
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

                //SetFuncKeyAll(this, "111111000011");
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }        

        private void CboSoukoName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(CboSoukoName.SelectedIndex>0)
                ExecDisp();

                CboSoukoName.Focus();
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








