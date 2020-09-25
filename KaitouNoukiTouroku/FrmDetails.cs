using System;
using System.Data;
using System.Windows.Forms;
using BL;
using Base.Client;

namespace KaitouNoukiTouroku
{

    public partial class FrmDetail : FrmSubForm
    {
        public class Detail_Entity
        {
            public string OrderNo { get; set; }
            public string OrderRows { get; set; }
            public string JANCD { get; set; }
            public string MakerItem { get; set; }
            public string SKUName { get; set; }
            public string Suryo { get; set; }
            public DataTable DtDetail { get; set; }
            //public DataRow[] DataRows { get; set; }
        }
        private enum EIndex : int
        {
            Suryo1,
            ArrivalPlanDate1,
            ArrivalPlanMonth1,
            ArrivalPlanKbn1,
            Suryo2,
            ArrivalPlanDate2,
            ArrivalPlanMonth2,
            ArrivalPlanKbn2,
            Suryo3,
            ArrivalPlanDate3,
            ArrivalPlanMonth3,
            ArrivalPlanKbn3
        }
        private const string ProNm = "回答納期登録";

        #region"公開プロパティ"
        public Detail_Entity de = new Detail_Entity();
        public int GyoCount { get; set; }
        #endregion

        private KaitouNoukiTouroku_BL knbl;
        private Control[] detailControls;
        DataRow[] DataRows;
        private string[] oldVal = new string[(int)EIndex.ArrivalPlanKbn3+1];

        public FrmDetail(ref DataTable dt)
        {
            InitializeComponent();
            
            HeaderTitleText = ProNm;
            this.Text = ProNm;
            F9Visible = false;
            F11Visible = false;

            de.DtDetail = dt;
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitScr()
        {
            lblJANCD.Text = de.JANCD;
            lblMakerItem.Text = de.MakerItem;
            lblSKUName.Text = de.SKUName;
            lblTel1.Text = knbl.Z_SetStr( de.Suryo);

            //コンボボックス初期化
            int count = 0;
            DataRows = de.DtDetail.Select("OrderNo ='" + de.OrderNo + "' AND OrderRows =" + de.OrderRows , "ROWNUM");
            foreach (DataRow row in DataRows)
            {
                if (knbl.Z_Set(row["UpdateFlg"]) == 2)
                    continue;

                if (knbl.Z_Set(row["ArrivalPlanSu"]) != 0)
                    detailControls[(int)EIndex.Suryo1 + 4 * count].Text = knbl.Z_SetStr( row["ArrivalPlanSu"]);

                detailControls[(int)EIndex.Suryo1 + 4 * count].Tag = knbl.Z_SetStr(row["InstructionSu"]);

                if (!string.IsNullOrWhiteSpace( row["ArrivalPlanDate"].ToString()))
                    detailControls[(int)EIndex.ArrivalPlanDate1 + 4 * count].Text =Convert.ToDateTime( row["ArrivalPlanDate"]).ToShortDateString();

                if(knbl.Z_Set(row["ArrivalPlanMonth"])>0)
                    detailControls[(int)EIndex.ArrivalPlanMonth1 + 4 * count].Text = bbl.FormatDate(row["ArrivalPlanMonth"].ToString() + "01").Substring(0, 7);

                if (!string.IsNullOrWhiteSpace(row["ArrivalPlanCD"].ToString()))
                {
                    ComboBox combo = (ComboBox)detailControls[(int)EIndex.ArrivalPlanKbn1 + 4 * count];
                    combo.SelectedValue = row["ArrivalPlanCD"];
                }
                count++;
            }  

            for(int i=0; i<=(int)EIndex.ArrivalPlanKbn3; i++)
            {
                oldVal[i] = detailControls[i].Text;
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                knbl = new KaitouNoukiTouroku_BL();

                InitialControlArray();
                InitScr();
                detailControls[0].Focus();
                
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
        private void InitialControlArray()
        {

            string ymd = knbl.GetDate();

            detailControls = new Control[] { IMN_SURYO_0,IMT_ARIDT_0,IMN_GENER2_0,IMC_KBN_0,IMN_SURYO_1,IMT_ARIDT_1,IMN_GENER2_1,IMC_KBN_1
                ,IMN_SURYO_2,IMT_ARIDT_2,IMN_GENER2_2,IMC_KBN_2              };

            //イベント付与
            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);

                if (ctl.GetType().Equals(typeof(CKM_Controls.CKM_ComboBox)))
                {
                    CKM_Controls.CKM_ComboBox sctl = (CKM_Controls.CKM_ComboBox)ctl;
                    sctl.Bind(ymd);
                }
            }
        }
        protected override void ExecSec()
        {
            //チェック処理
            for (int i = 0; i <= (int)EIndex.ArrivalPlanKbn3; i++)
            {
                int gyo = (int)Math.Floor((decimal)(i/4));

                if (ExistsInput(gyo))
                {
                    if (CheckDetail(i) == false)
                    {
                        detailControls[i].Focus();
                        return;
                    }
                }
            }
            //重複行チェック
            //入荷予定日、予定年月、入荷予定状況が同じ行はエラー	
            if (ExistsInput(0))
            {
                if (detailControls[(int)EIndex.ArrivalPlanDate1].Text == detailControls[(int)EIndex.ArrivalPlanDate2].Text
                && detailControls[(int)EIndex.ArrivalPlanMonth1].Text == detailControls[(int)EIndex.ArrivalPlanMonth2].Text
                && detailControls[(int)EIndex.ArrivalPlanKbn1].Text == detailControls[(int)EIndex.ArrivalPlanKbn2].Text)
                {
                    knbl.ShowMessage("E105");
                    detailControls[(int)EIndex.ArrivalPlanDate2].Focus();
                    return;
                }
                else if (detailControls[(int)EIndex.ArrivalPlanDate1].Text == detailControls[(int)EIndex.ArrivalPlanDate3].Text
                   && detailControls[(int)EIndex.ArrivalPlanMonth1].Text == detailControls[(int)EIndex.ArrivalPlanMonth3].Text
                   && detailControls[(int)EIndex.ArrivalPlanKbn1].Text == detailControls[(int)EIndex.ArrivalPlanKbn3].Text)
                {
                    knbl.ShowMessage("E105");
                    detailControls[(int)EIndex.ArrivalPlanDate3].Focus();
                    return;
                }
            }
            if (ExistsInput(1))
            {
                if (detailControls[(int)EIndex.ArrivalPlanDate2].Text == detailControls[(int)EIndex.ArrivalPlanDate3].Text
                && detailControls[(int)EIndex.ArrivalPlanMonth2].Text == detailControls[(int)EIndex.ArrivalPlanMonth3].Text
                && detailControls[(int)EIndex.ArrivalPlanKbn2].Text == detailControls[(int)EIndex.ArrivalPlanKbn3].Text)
                {
                    knbl.ShowMessage("E105");
                    detailControls[(int)EIndex.ArrivalPlanDate3].Focus();
                    return;
                }
            }

            int count = 0;
            string firstArrivalPlanDate = "";
            for (int i=1;i<=3;i++)
            {
                DataRow row;
                bool add = false;

                if (ExistsInput(i-1))
                {
                    if (count < DataRows.Length)
                        row = DataRows[count];
                    else
                    {
                        row = de.DtDetail.NewRow();
                        //1行は必ずあるはずなのでコピー
                        row.ItemArray = DataRows[0].ItemArray;
                        row["StockNO"] = "";
                        row["ReserveNO"] = "";
                        row["ArrivalPlanNO"] = "";
                        row["num2"] = "0";

                        //de.DtDetail.ImportRow(row);
                        add = true;
                    }

                    row["ROWNUM"] = count+1;
                    row["ArrivalPlanSu"] = detailControls[(int)EIndex.Suryo1 + 4 * (i-1)].Text;
                    if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanDate1 + 4 * (i - 1)].Text))
                    {
                        row["ArrivalPlanDate"] = detailControls[(int)EIndex.ArrivalPlanDate1 + 4 * (i - 1)].Text;
                        if (string.IsNullOrWhiteSpace(firstArrivalPlanDate))
                            firstArrivalPlanDate = row["ArrivalPlanDate"].ToString();
                    }
                    else
                    {
                        row["ArrivalPlanDate"] = DBNull.Value;
                    }
                    
                    row["ArrivalPlanMonth"] = knbl.Z_Set(detailControls[(int)EIndex.ArrivalPlanMonth1 + 4 * (i - 1)].Text.Replace("/", ""));

                    ComboBox combo = (ComboBox)detailControls[(int)EIndex.ArrivalPlanKbn1 + 4 * (i - 1)];
                    string kbn = "";
                    string num2 = "0";

                    if (combo.SelectedIndex != -1)
                    {
                        if (!combo.SelectedValue.Equals("-1"))
                        {
                            kbn = combo.SelectedValue.ToString();
                            num2 = combo.Tag.ToString();
                        }
                    }
                    row["ArrivalPlanCD"] = kbn;
                    row["num2"] = num2;

                    row["UpdateFlg"] = "1";

                    if (add)
                    {
                        // DataTable に新規行を追加
                        de.DtDetail.Rows.Add(row);
                    }

                    count++;
                }
            }
            //DataTableから不要な行を削除？　Updateフラグを2にする
            if (count < DataRows.Length)
            {
                for (int index = count; index < DataRows.Length; index++)
                {
                    DataRow row = DataRows[index];
                    //最大3行入力できるが、すべて入力なしになればNull
                    //どれかに入力があるなら、その最初の行の値
                    row["ArrivalPlanDate"] = DBNull.Value;
                    if(!string.IsNullOrWhiteSpace(firstArrivalPlanDate))
                        row["ArrivalPlanDate"] = firstArrivalPlanDate;
                    row["UpdateFlg"] = "2";
                }
            }

            GyoCount = count;

            EndSec();
        }

      private  bool ExistsInput(int count)
        {
            bool ret = false;

            if(knbl.Z_Set(detailControls[(int)EIndex.Suryo1 + 4 * count].Text) != 0)
            {
                return true;
            }
            if(!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanDate1 + 4 * count].Text))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ArrivalPlanMonth1 + 4 * count].Text))
            {
                return true;
            }
            ComboBox combo = (ComboBox)detailControls[(int)EIndex.ArrivalPlanKbn1 + 4 * count];
            if (combo.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(combo.Text))
                return true;

            return ret;
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
                    bool ret = CheckDetail(Array.IndexOf(detailControls, sender));
                    if (ret)
                    {
                        if (detailControls.Length - 1 > Array.IndexOf(detailControls, sender))
                            detailControls[Array.IndexOf(detailControls, sender) + 1].Focus();

                        else
                        {
                            //BtnF12.Focus();
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
            }
        }

        private bool CheckDetail(int index, bool set=true)
        {
            string fmtYmd = "";

            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.Suryo1:
                case (int)EIndex.Suryo2:
                case (int)EIndex.Suryo3:
                    decimal sumSuryo = knbl.Z_Set(detailControls[(int)EIndex.Suryo1].Text) + knbl.Z_Set(detailControls[(int)EIndex.Suryo2].Text) + knbl.Z_Set(detailControls[(int)EIndex.Suryo3].Text);
                    if (knbl.Z_Set(de.Suryo) < sumSuryo)
                    {
                        //３行の数量合計が、メイン画面の数量より大きくなるとエラー
                        knbl.ShowMessage("E179");
                        detailControls[index].Focus();
                        return false;
                    }
                    //（既に出荷指示されている数より小さくはできません）
                    if (knbl.Z_Set(detailControls[index].Text) < knbl.Z_Set(detailControls[index].Tag)) //InstructionSu;
                    {
                        knbl.ShowMessage("E225");
                        detailControls[index].Focus();
                        return false;
                    }

                    detailControls[index].Text = knbl.Z_SetStr(detailControls[index].Text);
                    break;

                case (int)EIndex.ArrivalPlanDate1:
                case (int)EIndex.ArrivalPlanDate2:
                case (int)EIndex.ArrivalPlanDate3:
                    if (detailControls[index].Text != oldVal[index])
                    {
                        if (!knbl.ChkArrivePlanDate(detailControls[index].Text, ref fmtYmd))
                        {
                            return false;
                        }
                        detailControls[index].Text = fmtYmd;
                    }
                    break;

                case (int)EIndex.ArrivalPlanMonth1:
                case (int)EIndex.ArrivalPlanMonth2:
                case (int)EIndex.ArrivalPlanMonth3:
                    if (detailControls[index].Text != oldVal[index])
                    {
                        if (!knbl.ChkArrivalPlanMonth(detailControls[index].Text, detailControls[index - 1].Text, ref fmtYmd))
                        {
                            return false;
                        }
                        detailControls[index].Text = fmtYmd;
                    }
                    break;

                case (int)EIndex.ArrivalPlanKbn1:
                case (int)EIndex.ArrivalPlanKbn2:
                case (int)EIndex.ArrivalPlanKbn3:
                    ComboBox combo = (ComboBox)detailControls[index];
                    string kbn = "";
                    if (combo.SelectedIndex != -1)
                        if (!combo.SelectedValue.Equals("-1"))
                            kbn = combo.SelectedValue.ToString();

                    string num2 = "";
                    if (!knbl.ChkArrivalPlanKbn(kbn, detailControls[index-2].Text, detailControls[index-1].Text, out num2))
                    {
                        return false;
                    }
                    combo.Tag = num2;
                    break;
            }

            return true;
        }

    }
}
