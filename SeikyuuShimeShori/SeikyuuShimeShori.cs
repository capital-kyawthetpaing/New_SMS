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

namespace SeikyuuShimeShori
{
    /// <summary>
    /// SeikyuuShimeShori 請求締処理（店舗）
    /// </summary>
    internal partial class SeikyuuShimeShori : FrmMainForm
    {
        private const string ProID = "SeikyuuShimeShori";
        private const string ProNm = "請求締処理（店舗）";

        private enum EIndex : int
        {
            StoreCD,
            Syori,
            SeqSDT,
            Simbi,
            CustomerCD,
            //CustomerName

        }

        private Control[] detailControls;
        private Control[] detailLabels;
        private Control[] searchButtons;
        
        private SeikyuuShimeShori_BL ssbl;

        private System.Windows.Forms.Control previousCtrl; // ｶｰｿﾙの元の位置を待避
        
        private string mOldCustomerCD = "";
        
        public SeikyuuShimeShori()
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

                ssbl = new SeikyuuShimeShori_BL();
                string ymd = bbl.GetDate();
                CboStoreCD.Bind(ymd);

                BindCombo("KBN", "Name");

                //基準日：Form.請求締日	(未入力時はToday)得意先区分：2	
                ScCustomer.Value1 = "2";

                SetFuncKeyAll(this, "100001000001");
                Scr_Clr(0);

                ExecDisp();

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
            datarow[value] = "請求締";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "2";
            datarow[value] = "請求締キャンセル";
            dt.Rows.Add(datarow);

            datarow = dt.NewRow();
            datarow[key] = "3";
            datarow[value] = "請求確定";
            dt.Rows.Add(datarow);

            //DataRow dr = dt.NewRow();
            //dr[key] = "-1";
            //dt.Rows.InsertAt(dr, 0);
            cboSyori.DataSource = dt;
            cboSyori.DisplayMember = value;
            cboSyori.ValueMember = key;
        }
        protected override void ExecDisp()
        {
            D_BillingProcessing_Entity dbe = new D_BillingProcessing_Entity
            {
                StoreCD = CboStoreCD.SelectedValue.ToString()
            };

            DataTable dt = ssbl.D_BillingProcessing_SelectAll(dbe);
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
            switch (syori)
            {
                //処理：請求締の場合
                case 1:
                    if (bbl.ShowMessage("Q101") != DialogResult.Yes)
                    {
                        return;
                    }
                    break;

                case 2:
                    if (bbl.ShowMessage("Q103") != DialogResult.Yes)
                    {
                        return;
                    }

                    break;

                case 3:
                    if (bbl.ShowMessage("Q104") != DialogResult.Yes)
                    {
                        return;
                    }

                    break;
            }

            D_CollectPlan_Entity dce = new D_CollectPlan_Entity
            {
                Syori = syori.ToString(),
                StoreCD = CboStoreCD.SelectedValue.ToString(),
                CustomerCD = detailControls[(int)EIndex.CustomerCD].Text,
                BillingDate = detailControls[(int)EIndex.SeqSDT].Text,
                BillingCloseDate = detailControls[(int)EIndex.Simbi].Text
            };

            ret = ssbl.D_CollectPlan_Check(dce);
            if (!ret)
            {
                bbl.ShowMessage(dce.MessageID);
                return;
            }

            //更新処理
            ssbl.D_BillingProcessing_Exec(dce, InOperatorCD, InPcID);

            ExecDisp();

            detailControls[(int)EIndex.Syori].Focus();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { CboStoreCD, cboSyori, ckM_TextBox1, ckM_TextBox2, ScCustomer.TxtCode};//, ckM_CustomerName 
            detailLabels = new Control[] { ScCustomer };
            searchButtons = new Control[] { ScCustomer.BtnSearch };

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
            bool ret;
            switch (index)
            {
                case (int)EIndex.Syori:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    break;

                case (int)EIndex.SeqSDT:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    detailControls[index].Text = ssbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!ssbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        ssbl.ShowMessage("E103");
                        return false;
                    }
                    //入力できる範囲内の日付であること
                    if (!bbl.CheckInputPossibleDate(detailControls[index].Text))
                    {
                        //Ｅ１１５
                        bbl.ShowMessage("E115");
                        return false;
                    }
                    ScCustomer.ChangeDate = detailControls[index].Text;
                    break;

                case (int)EIndex.Simbi:
                    //必須入力(Entry required)、入力なければエラー(If there is no input, an error)Ｅ１０２
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //Ｅ１０２
                        bbl.ShowMessage("E102");
                        return false;
                    }

                    //１～３１以外はエラー
                    int simebi = Convert.ToInt32(detailControls[index].Text);
                    if (simebi<1 || simebi >31)
                    {
                        //Ｅ１０３
                        bbl.ShowMessage("E103");
                        return false;
                    }
                    break;

                case (int)EIndex.CustomerCD:
                    //入力無くても良い(It is not necessary to input)
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return true;
                    }

                    string ymd = detailControls[(int)EIndex.SeqSDT].Text;
                    if (string.IsNullOrWhiteSpace(ymd))
                        ymd = bbl.GetDate();

                    //[M_Customer_Select]
                    M_Customer_Entity mce = new M_Customer_Entity
                    {
                        CustomerCD = detailControls[index].Text,
                        ChangeDate = ymd
                    };

                    Customer_BL sbl = new Customer_BL();
                     ret = sbl.M_Customer_Select(mce);
                    if (ret)
                    {
                        if (mOldCustomerCD != detailControls[index].Text)
                        {
                            ScCustomer.LabelText = mce.CustomerName;
                            if (mce.VariousFLG == "1")
                            {
                                //detailControls[index + 1].Text = mce.CustomerName;
                                //    detailControls[index + 1].Enabled = true;
                            }
                            else
                            {
                                //detailControls[index + 1].Text = mce.CustomerName;
                            }
                        }
                    }
                    else
                    {
                        bbl.ShowMessage("E101");
                        //顧客情報ALLクリア
                        ClearCustomerInfo();
                        return false;
                    }

                    mOldCustomerCD = detailControls[index].Text;    //位置確認

                    break;

                //case (int)EIndex.CustomerName:
                //    //入力可能の場合 入力必須(Entry required)
                //    if (detailControls[index].Enabled && string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        //Ｅ１０２
                //        bbl.ShowMessage("E102");
                //        return false;
                //    }
                //    break;

                case (int)EIndex.StoreCD:
                    //選択必須(Entry required)
                    if (!RequireCheck(new Control[] { detailControls[index] }))
                    {
                        return false;
                    }
                    else
                    {
                        if (!base.CheckAvailableStores(CboStoreCD.SelectedValue.ToString()))
                        {
                            bbl.ShowMessage("E141");
                            CboStoreCD.Focus();
                            return false;
                        }
                        ExecDisp();
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
            string ymd = ssbl.GetDate();

            //スタッフマスター(M_Staff)に存在すること
            //[M_Staff]
            M_Staff_Entity mse = new M_Staff_Entity
            {
                StaffCD = InOperatorCD,
                ChangeDate = ssbl.GetDate()
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

            cboSyori.SelectedValue = 1;
            detailControls[(int)EIndex.SeqSDT].Text = ymd;
            
            detailControls[(int)EIndex.Syori].Focus();
        }

        /// <summary>
        /// 顧客情報クリア処理
        /// </summary>
        private void ClearCustomerInfo()
        {
            mOldCustomerCD = "";

            ScCustomer.LabelText = "";
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

                int index = Array.IndexOf(detailControls, sender);
                switch(index)
                    {
                    case (int)EIndex.CustomerCD:
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

        private void DgvDetail_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //列ヘッダーかどうか調べる
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                //セルを描画する
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //行番号を描画する範囲を決定する
                //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //行番号を描画する
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                //描画が完了したことを知らせる
                e.Handled = true;
            }

        }

        private void CboStoreCD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CboStoreCD.SelectedIndex > 0)
                    ScCustomer.Value2 = CboStoreCD.SelectedValue.ToString();
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








