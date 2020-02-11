using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;

namespace Search
{
    public partial class Search_Product : FrmSubForm
    {
        private const string ProNm = "商品検索";

        private enum EIndex : int
        {
            VendorCd,
            BrandCD,
            SKUName,
            JanCD,
            SKUCD,
            CommentInStore,
            YearTerm,
            Season,
            CatalogNO,
            ReserveCD,
            NoticesCD,
            PostageCD,
            OrderAttentionCD, 
            SportsCD,
            //ClassificationA,
            //ClassificationB,
            //ClassificationC,
            TagName1,
            TagName2,
            TagName3,
            TagName4,
            TagName5,

            InstructionsNO,
            MakerItem,
            ITemCD,
            InputStart,
            InputEnd,
            UpdateStart,
            UpdateEnd,
            DayStart,
            DayEnd
        }
        public string Mode { get; set; }
        /// <summary>
        /// Mode=5の場合チェックONされたJANCDを起動元画面に戻す
        /// </summary>
        public List<string> list = new List<string>(); 
        public string AdminNO { get; set; }
        public string ITEM { get; set; }
        public string SKUCD { get; set; }
        public string MakerItem { get; set; }
        public string JANCD { get; set; }
        public string ChangeDate = string.Empty;

        private Control[] detailControls;
        M_SKU_Entity mse;
        SKU_BL sbl;

        public Search_Product(string changeDate)
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = "商品検索";
            this.Text = ProNm;

            CboYearTerm.Bind(changeDate);
            CboSeason.Bind(changeDate);
            cboReserveCD.Bind(changeDate);
            cboPostageCD.Bind(changeDate);
            cboNoticesCD.Bind(changeDate);
            cboOrderAttentionCD.Bind(changeDate);
            cboTagName1.Bind(changeDate);
            cboTagName2.Bind(changeDate);
            cboTagName3.Bind(changeDate);
            cboTagName4.Bind(changeDate);
            cboTagName5.Bind(changeDate);

            if (string.IsNullOrWhiteSpace(changeDate))
                changeDate = sbl.GetDate();

            this.ChangeDate = changeDate;
            this.lblChangeDate.Text = this.ChangeDate;

            sbl = new SKU_BL();
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] {ScMaker.TxtCode,ScBrand.TxtCode,ckM_Text_4,ckM_TextBox11, ckM_TextBox12, ckM_CustomerName
                ,CboYearTerm,CboSeason, ckM_TextBox10,cboReserveCD,cboNoticesCD,cboPostageCD,cboOrderAttentionCD
                ,ScSports.TxtCode,cboTagName1,cboTagName2,cboTagName3,cboTagName4,cboTagName5
                , ckM_TextBox9, ckM_TextBox5, ckM_TextBox8
                , ckM_TextBox1, ckM_TextBox2, ckM_TextBox3, ckM_TextBox4, ckM_TextBox7, ckM_TextBox6 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
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

        private M_SKU_Entity GetSearchInfo(M_SKUInfo_Entity msie)
        {

            string rsvCd = "";
            if (cboReserveCD.SelectedValue != null)
                if (!cboReserveCD.SelectedValue.Equals("-1"))
                rsvCd = cboReserveCD.SelectedValue.ToString();

            string posCd = "";
            if (cboPostageCD.SelectedValue != null)
                if (!cboPostageCD.SelectedValue.Equals("-1"))
                posCd = cboPostageCD.SelectedValue.ToString();

            string notCd = "";
            if (cboNoticesCD.SelectedValue != null)
                if (!cboNoticesCD.SelectedValue.Equals("-1"))
                notCd = cboNoticesCD.SelectedValue.ToString();

            string orACd = "";
            if (cboOrderAttentionCD.SelectedValue != null)
                if (!cboOrderAttentionCD.SelectedValue.Equals("-1"))
                orACd = cboOrderAttentionCD.SelectedValue.ToString();

            string tag1 = "";
            if (cboTagName1.SelectedValue != null)
                if(!cboTagName1.SelectedValue.Equals("-1"))
                    tag1 = cboTagName1.SelectedText;

            string tag2 = "";
            if (cboTagName2.SelectedValue != null)
                if (!cboTagName2.SelectedValue.Equals("-1"))
                tag2 = cboTagName2.SelectedText;

            string tag3 = "";
            if (cboTagName3.SelectedValue != null)
                if (!cboTagName3.SelectedValue.Equals("-1"))
                tag3 = cboTagName3.SelectedText;

            string tag4= "";
            if (cboTagName4.SelectedValue != null)
                if (!cboTagName4.SelectedValue.Equals("-1"))
                tag4 = cboTagName4.Text;

            string tag5 = "";
            if (cboTagName5.SelectedValue != null)
                if (!cboTagName5.SelectedValue.Equals("-1"))
                tag5 = cboTagName5.SelectedText;

            mse = new M_SKU_Entity
            {
                MainVendorCD = detailControls[(int)EIndex.VendorCd].Text,
                BrandCD = detailControls[(int)EIndex.BrandCD].Text,
                SKUName = detailControls[(int)EIndex.SKUName].Text,
                JanCD = detailControls[(int)EIndex.JanCD].Text,
                SKUCD = detailControls[(int)EIndex.SKUCD].Text,
                CommentInStore = detailControls[(int)EIndex.CommentInStore].Text,   //カンマ区切り

                ReserveCD = rsvCd,
                NoticesCD = notCd,
                PostageCD = posCd,
                OrderAttentionCD =orACd,
                SportsCD = detailControls[(int)EIndex.SportsCD].Text,
                TagName1 = cboTagName1.Text,
                TagName2 = cboTagName2.Text,
                TagName3 = cboTagName3.Text,
                TagName4 = cboTagName4.Text,
                TagName5 = cboTagName5.Text,
                //ClassificationA = scClassificationA.TxtCode.Text,
                //ClassificationB = scClassificationB.TxtCode.Text,
                //ClassificationC = scClassificationC.TxtCode.Text,

                MakerItem = detailControls[(int)EIndex.MakerItem].Text,     //カンマ区切り
                ITemCD = detailControls[(int)EIndex.ITemCD].Text,           //カンマ区切り
                InputDateFrom = detailControls[(int)EIndex.InputStart].Text,
                InputDateTo = detailControls[(int)EIndex.InputEnd].Text,
                UpdateDateFrom = detailControls[(int)EIndex.UpdateStart].Text,
                UpdateDateTo = detailControls[(int)EIndex.UpdateEnd].Text,
                ApprovalDateFrom = detailControls[(int)EIndex.DayStart].Text,
                ApprovalDateTo = detailControls[(int)EIndex.DayEnd].Text,
                ChangeDate = this.ChangeDate,

            };

            msie.YearTerm = CboYearTerm.SelectedValue.Equals("-1") ? string.Empty : CboYearTerm.SelectedValue.ToString();
            msie.Season = CboSeason.SelectedValue.Equals("-1") ? string.Empty : CboSeason.SelectedValue.ToString();
            msie.CatalogNO = detailControls[(int)EIndex.CatalogNO].Text;
            msie.InstructionsNO = detailControls[(int)EIndex.InstructionsNO].Text;

            if (ckM_RadioButton1.Checked)
                mse.OrOrAnd = "0";
            else
                mse.OrOrAnd = "1";

            if (ChkSearch.Checked)
            {
                mse.SearchFlg = "1";

                if (ckM_RadioButton3.Checked)
                    mse.ItemOrMaker = "0";
                else
                    mse.ItemOrMaker = "1";
            }
            else
            {
                mse.SearchFlg = "0";
            }

            return mse;
        }

        private void DgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
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

        private void GvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.ExecSec();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        protected override void ExecSec()
        {
            GetData();
            EndSec();
        }

        protected override void ExecDisp()
        {
            bool inputFlg = false;
            for (int i = 0; i < detailControls.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(detailControls[i].Text))
                    inputFlg = true;

                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }
            }

            if (inputFlg == false)
            {
                //全てが空白(入力が無い)場合、エラーとする。
                sbl.ShowMessage("E111");
                return;
            }

            M_SKUInfo_Entity msie = new M_SKUInfo_Entity();
            mse = GetSearchInfo(msie);

            DataTable dt = sbl.M_SKU_SelectForSearchProduct(mse, msie);
           

            if (dt.Rows.Count>0)
            {
                //ストアドで実装するように変更
                //if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.ITemCD].Text))
                //{
                //    string[] itemDt = detailControls[(int)EIndex.ITemCD].Text.Split(',');
                //    if (itemDt.Length > 0)
                //    {
                //        //foreach (DataRow dr in dt.Rows)
                //        for(int i=dt.Rows.Count-1; 0<=i; i--)
                //        {
                //            int index = Array.IndexOf(itemDt, dt.Rows[i]["ItemCD"].ToString());
                //            if (index < 0)
                //            {
                //                dt.Rows.Remove(dt.Rows[i]);
                //            }
                //        }
                //    }
                //}
                //if (!string.IsNullOrWhiteSpace(detailControls[(int)EIndex.MakerItem].Text))
                //{
                //    string[] makerDt = detailControls[(int)EIndex.MakerItem].Text.Split(',');
                //    if (makerDt.Length > 0)
                //    {
                //        for (int i = dt.Rows.Count - 1; 0 <= i; i--)
                //        {
                //            int index = Array.IndexOf(makerDt, dt.Rows[i]["MakerItem"].ToString());
                //            if (index < 0)
                //            {
                //                dt.Rows.Remove(dt.Rows[i]);
                //            }
                //        }
                //    }
                //}
                if (ChkSearch.Checked)
                {
                    string[] item = new string[dt.Rows.Count];
                    int itemCount = 0;

                    if (ckM_RadioButton3.Checked)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int index = Array.IndexOf(item, dr["ItemCD"].ToString());
                            if (index < 0)
                            {
                                item[itemCount] = dr["ItemCD"].ToString();
                                itemCount++;
                            }
                        }
                        string val = "";
                        for (int i = 0; i < itemCount; i++)
                            if (i == 0)
                                val += item[i];
                            else
                                val += "," + item[i];

                        detailControls[(int)EIndex.ITemCD].Text = val;
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int index = Array.IndexOf(item, dr["MakerItem"].ToString());
                            if (index < 0)
                            {
                                item[itemCount] = dr["MakerItem"].ToString();
                                itemCount++;
                            }
                        }
                        string val = "";
                        for (int i = 0; i < itemCount; i++)
                            if (i == 0)
                                val += item[i];
                            else
                                val += "," + item[i];

                        detailControls[(int)EIndex.MakerItem].Text = val;
                    }
                }
                GvDetail.DataSource = dt;
                GvDetail.ScrollBars = ScrollBars.Both;
                GvDetail.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                GvDetail.CurrentRow.Selected = true;
                GvDetail.Enabled = true;
                GvDetail.Focus();
            }
            else
            {
               sbl.ShowMessage("E128");
            }
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
                            ckM_CheckBox1.Focus();
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

                    btnSubF11.Focus();
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }

        }
        private bool CheckDetail(int index)
        {
            bool ret;

            switch (index)
            {
                case (int)EIndex.VendorCd:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScMaker.LabelText = "";
                        return true;
                    }
                    else
                    {
                        M_Vendor_Entity mve = new M_Vendor_Entity
                        {
                            VendorCD = detailControls[index].Text,
                            ChangeDate = string.IsNullOrWhiteSpace(this.ChangeDate) ? sbl.GetDate() : this.ChangeDate,
                            DeleteFlg = "0"
                        };
                        Vendor_BL vbl = new Vendor_BL();
                        ret = vbl.M_Vendor_Select(mve);
                        if (ret)
                        {
                            ScMaker.LabelText = mve.VendorName;
                        }
                        else
                        {
                            sbl.ShowMessage("E101");
                            ScMaker.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.BrandCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScBrand.LabelText = "";
                    }
                    else
                    {
                        //[M_MakerBrand]
                        M_MakerBrand_Entity mme = new M_MakerBrand_Entity
                        {
                            ChangeDate = string.IsNullOrWhiteSpace(this.ChangeDate) ? sbl.GetDate() : this.ChangeDate,
                            BrandCD = detailControls[index].Text
                        };
                        MakerBrand_BL mbl = new MakerBrand_BL();
                        ret = mbl.M_MakerBrand_Select(mme);
                        if (ret)
                        {
                            ScBrand.LabelText = mme.BrandName;
                        }
                        else
                        {
                            //Ｅ１０１
                            sbl.ShowMessage("E101");
                            ScBrand.LabelText = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.SportsCD:
                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                    {
                        ScSports.LabelText = "";
                    }
                    else
                    {
                        //以下の条件でM_MultiPorposeが存在しない場合、エラー

                        //[M_MultiPorpose]
                        M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity
                        {
                            ID = MultiPorpose_BL.ID_SPORTS,
                            Key = detailControls[index].Text,
                            ChangeDate = string.IsNullOrWhiteSpace(this.ChangeDate) ? sbl.GetDate() : this.ChangeDate
                        };
                        MultiPorpose_BL mbl = new MultiPorpose_BL();
                        DataTable dt = mbl.M_MultiPorpose_Select(mme);
                        if (dt.Rows.Count > 0)
                        {
                            ScSports.LabelText = dt.Rows[0]["Char1"].ToString();
                        }
                        else
                        {
                            //Ｅ１０１
                            sbl.ShowMessage("E101");
                            ScSports.LabelText = "";
                            return false;
                        }
                    }
                    break;

                //case (int)EIndex.ClassificationA:
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        scClassificationA.LabelText = "";
                //    }
                //    else
                //    {
                //        //以下の条件でM_ClassificationAが存在しない場合、エラー

                //        //[M_ClassificationA]
                //        M_Classification_Entity mce = new M_Classification_Entity
                //        {
                //            ClassificationA = detailControls[index].Text
                //        };
                //        Classification_BL mbl = new Classification_BL();
                //        ret = mbl.M_ClassificationA_Select(mce);
                //        if (ret)
                //        {
                //            scClassificationA.LabelText = mce.ClassificationAName;
                //        }
                //        else
                //        {
                //            //Ｅ１０１
                //            sbl.ShowMessage("E101");
                //            scClassificationA.LabelText = "";
                //            return false;
                //        }
                //    }
                //    break;
                //case (int)EIndex.ClassificationB:
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        scClassificationB.LabelText = "";
                //    }
                //    else
                //    {
                //        //以下の条件でM_ClassificationBが存在しない場合、エラー

                //        //[M_ClassificationB]
                //        M_Classification_Entity mce = new M_Classification_Entity
                //        {
                //            ClassificationA = detailControls[index - 1].Text,
                //            ClassificationB = detailControls[index].Text
                //        };
                //        Classification_BL mbl = new Classification_BL();
                //        ret = mbl.M_ClassificationB_Select(mce);
                //        if (ret)
                //        {
                //            scClassificationA.LabelText = mce.ClassificationAName;
                //            scClassificationB.LabelText = mce.ClassificationBName;
                //        }
                //        else
                //        {
                //            //Ｅ１０１
                //            sbl.ShowMessage("E101");
                //            scClassificationB.LabelText = "";
                //            return false;
                //        }
                //    }
                //    break;

                //case (int)EIndex.ClassificationC:
                //    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                //    {
                //        scClassificationC.LabelText = "";
                //    }
                //    else
                //    {
                //        //以下の条件でM_ClassificationCが存在しない場合、エラー

                //        //[M_ClassificationC]
                //        M_Classification_Entity mce = new M_Classification_Entity
                //        {
                //            ClassificationA = detailControls[index - 2].Text,
                //            ClassificationB = detailControls[index - 1].Text,
                //            ClassificationC = detailControls[index].Text
                //        };
                //        Classification_BL mbl = new Classification_BL();
                //        ret = mbl.M_ClassificationC_Select(mce);
                //        if (ret)
                //        {
                //            scClassificationA.LabelText = mce.ClassificationAName;
                //            scClassificationB.LabelText = mce.ClassificationBName;
                //            scClassificationC.LabelText = mce.ClassificationCName;
                //        }
                //        else
                //        {
                //            //Ｅ１０１
                //            sbl.ShowMessage("E101");
                //            scClassificationC.LabelText = "";
                //            return false;
                //        }
                //    }
                //    break;
                case (int)EIndex.DayStart:
                case (int)EIndex.DayEnd:
                case (int)EIndex.InputStart:
                case (int)EIndex.InputEnd:
                case (int)EIndex.UpdateStart:
                case (int)EIndex.UpdateEnd:

                    if (string.IsNullOrWhiteSpace(detailControls[index].Text))
                        return true;

                    detailControls[index].Text = this.sbl.FormatDate(detailControls[index].Text);

                    //日付として正しいこと(Be on the correct date)Ｅ１０３
                    if (!this.sbl.CheckDate(detailControls[index].Text))
                    {
                        //Ｅ１０３
                        this.sbl.ShowMessage("E103");
                        return false;
                    }
                    //見積日(From) ≧ 見積日(To)である場合Error
                    if (index == (int)EIndex.DayEnd || index == (int)EIndex.InputEnd || index == (int)EIndex.UpdateEnd)
                    {
                        if (!string.IsNullOrWhiteSpace(detailControls[index - 1].Text) && !string.IsNullOrWhiteSpace(detailControls[index].Text))
                        {
                            int result = detailControls[index].Text.CompareTo(detailControls[index - 1].Text);
                            if (result < 0)
                            {
                                //Ｅ１０６
                                this.sbl.ShowMessage("E104");
                                detailControls[index].Focus();
                                return false;
                            }
                        }
                    }

                    break;
            }

            return true;
        }

        private void GetData()
        {
            if(GvDetail.CurrentRow != null &&  GvDetail.CurrentRow.Index >= 0)
            { 
                ITEM = GvDetail.CurrentRow.Cells["ITEMCD"].Value.ToString();
                SKUCD = GvDetail.CurrentRow.Cells["colSKUCD"].Value.ToString();
                MakerItem = GvDetail.CurrentRow.Cells["colMakerItem"].Value.ToString();
                JANCD = GvDetail.CurrentRow.Cells["colJANCD"].Value.ToString();
                AdminNO = GvDetail.CurrentRow.Cells["colAdminNO"].Value.ToString();

                if (Mode == "5")   //Todo:コーディング
                    for (int i = 0; i < GvDetail.RowCount - 1; i++)
                    {
                        if (GvDetail.Rows[i].Cells["colCheck"].Value.Equals(true))
                        {
                            list.Add(GvDetail.Rows[i].Cells["colJANCD"].Value.ToString());
                        }
                    }
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
        /// <summary>
        /// 画面クリア
        /// </summary>
        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";

            ckM_RadioButton1.Checked = true;
            ckM_RadioButton3.Checked = true;

        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                //パラメータ　検索モード＝5の場合のみ入力可能に
                if (Mode != "5")
                    colCheck.Visible = false;

                Scr_Clr();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }
    }
}
