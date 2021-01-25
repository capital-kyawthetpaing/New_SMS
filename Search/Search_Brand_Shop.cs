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
using BL;
using Entity;

namespace Search
{
    public partial class Search_Brand_Shop : Search_Base
    {
        private const string ProNm = "ブランド検索";
        

        private enum EIndex : int
        {
            BrandName,
            MakerCD,
        }

        #region"公開プロパティ"
        public string parBrandCD = "";
        public string parBrandName = "";
        public string parMakerCD = "";
        public string parChangeDate = "";
        #endregion

        private Control[] detailControls;

        Brand_BL mbl;
        M_Brand_Entity mbe;

        public Search_Brand_Shop()
        {
            InitializeComponent();
            InitialControlArray();
          
            //HeaderTitleText = "ブランド";
            ProgramName = "ブランド";
            this.Text = ProNm;
        }

        private void Search_Brand_Shop_Load(object sender, EventArgs e)
        {
            try
            {
                mbl = new Brand_BL();
                Scr_Clr();

                radioButton1.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void Scr_Clr()
        {
            foreach (Control ctl in detailControls)
                ctl.Text = "";
            lblMakerName.Text = "";

            //初期値設定
            if (DateTime.TryParse(parChangeDate, out DateTime dt))
                ckM_Label1.Text = parChangeDate;
            else
                ckM_Label1.Text = mbl.GetDate();

            lblMakerName.Text = "";
            radioButton1.Checked = true;

            //if (!string.IsNullOrWhiteSpace(parMakerCD))
            //{
            //    = parMakerCD;
            //    CheckDetail((int)EIndex.MakerCD);
            //    ScMaker.TxtCode.Enabled = false;
            //}
        }

        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox2, ckM_TextBox3 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
                
            }

            //btnStoreCD.Click += new System.EventHandler(BtnSearch_Click);
            radioButton1.Enter += new System.EventHandler(RadioButton_Enter);
            radioButton2.Enter += new System.EventHandler(RadioButton_Enter);

        }


        private void RadioButton_Enter(object sender, EventArgs e)
        {
            try
            {
                //previousCtrl = this.ActiveControl;

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
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
                            //あたかもTabキーが押されたかのようにする
                            //Shiftが押されている時は前のコントロールのフォーカスを移動
                            this.ProcessTabKey(!e.Shift);
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


        private bool CheckDetail(int index)
        {
            switch (index)
            {
                case (int)EIndex.MakerCD:
                    //入力なければチェックなし
                    if (detailControls[index].Text == "")
                    {
                        lblMakerName.Text = "";
                        return true;
                    }
                    else
                    {
                        //仕入先マスタデータチェック
                        Vendor_BL mbl = new Vendor_BL();
                        M_Vendor_Entity mse = new M_Vendor_Entity
                        {
                            VendorCD = detailControls[index].Text,
                            ChangeDate = ckM_Label1.Text
                        };
                        bool ret = mbl.M_Vendor_SelectTop1(mse);
                        if (ret)
                        {
                            lblMakerName.Text = mse.VendorName;
                        }
                        else
                        {
                            //Ｅ１３６
                            mbl.ShowMessage("E136");
                            lblMakerName.Text = "";
                            return false;
                        }
                    }
                    break;

                case (int)EIndex.BrandName:

                    break;


            }

            return true;
        }

        private void dgvBrand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                    if (e.Control == false)
                    {
                        GetData();
                    }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        private void dgvBrand_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GetData();

            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                //EndSec();
            }
        }

        public override void FunctionProcess(int Index)
        {
            //base.FunctionProcess(Index);
            if (Index + 11 == 12)
            {
                GetData();
            }
        }

        private  void GetData()
        {
            if (dgvBrand.CurrentRow is null)
                return;

            parBrandCD = dgvBrand.CurrentRow.Cells["colBrandCD"].Value.ToString();
            parBrandName = dgvBrand.CurrentRow.Cells["colBrandName"].Value.ToString();
            parChangeDate = dgvBrand.CurrentRow.Cells["colChangeDate"].Value.ToString();
            parMakerCD = dgvBrand.CurrentRow.Cells["colMakerCD"].Value.ToString();

            EndSec();
        }

        protected  void ExecDisp()
        {
            for (int i = 0; i < detailControls.Length; i++)
                if (CheckDetail(i) == false)
                {
                    detailControls[i].Focus();
                    return;
                }

            bool ret = BindGrid();

            if (ret)
            {
                dgvBrand.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBrand.CurrentRow.Selected = true;
                dgvBrand.Enabled = true;
                dgvBrand.Focus();
            }
            else
            {
                mbl.ShowMessage("E128");
                dgvBrand.DataSource = null;
            }
        }

        private bool BindGrid()
        {
            mbe = GetEntity();
            DataTable dt = mbl.M_Brand_SelectAll(mbe);

            if (dt.Rows.Count == 0)
                return false;

            dgvBrand.DataSource = dt;
            return true;
        }

        private M_Brand_Entity GetEntity()
        {
            mbe = new M_Brand_Entity();
            mbe.DisplayKbn = radioButton1.Checked ? "0" : "1";
            mbe.ChangeDate = ckM_Label1.Text;
            mbe.MakerCD = detailControls[(int)EIndex.MakerCD].Text;
            mbe.BrandName = detailControls[(int)EIndex.BrandName].Text;

            return mbe;
        }

        protected override void EndSec()
        {
            this.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                ExecDisp();
            }
            catch(Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvDetail_Paint(object sender, PaintEventArgs e)
        {
            string[] monthes = { "", "メーカー (仕入先)", "" };
            for (int j = 2; j < 3;)
            {
                Rectangle r1 = this.dgvBrand.GetCellDisplayRectangle(j, -1, true);
                int w1 = this.dgvBrand.GetCellDisplayRectangle(j + 1, -1, true).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + w1 - 2;
                r1.Height = r1.Height - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dgvBrand.ColumnHeadersDefaultCellStyle.BackColor), r1);
                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(monthes[j / 2],
                this.dgvBrand.ColumnHeadersDefaultCellStyle.Font,
                new SolidBrush(this.dgvBrand.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
                j += 2;

            }
        }



    }
}
