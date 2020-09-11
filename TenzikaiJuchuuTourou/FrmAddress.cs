using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;
using Entity;
using Base.Client;

namespace  TenzikaiJuchuuTourou
{

    public partial class FrmAddress : FrmSubForm
    {
        public class Address_Entity
        {
            public string VariousFLG { get; set; }
            public string CustomerCD { get; set; }
            public string CustomerName { get; set; }
            public string CustomerName2 { get; set; }

            public string ZipCD1 { get; set; }
            public string ZipCD2 { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Tel11 { get; set; }
            public string Tel12 { get; set; }
            public string Tel13 { get; set; }
        }

        private const string ProNm = "店舗受注入力";

        private enum EIndex : int
        {
            ZipCD1,
            ZipCD2,
            Address1,
            Address2,
            //Tel11,
            //Tel12,
            //Tel13
        }

        #region"公開プロパティ"
        public short kbn = 0;   //1:配送先
        public Address_Entity ade = new Address_Entity();
        public Address_Entity adeD = new Address_Entity();
        #endregion

        private Control[] detailControls;

        public FrmAddress()
        {
            InitializeComponent();

            InitialControlArray();

            HeaderTitleText = ProNm;
            this.Text = ProNm;
            F9Visible = false;
            F11Visible = false;
        }

        public void ClearAddressInfo(short kbn)
        {
            if(kbn.Equals(0))
                ade = new Address_Entity();
            else
                adeD = new Address_Entity();
        }
        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox6, ckM_TextBox5, ckM_TextBox4, ckM_TextBox7 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitScr(short kbn=0)
        {
            if (kbn.Equals(0))
            {
                lblCustomer.Visible = true;
                lblDelivery.Visible = false;

                detailControls[(int)EIndex.ZipCD1].Text = ade.ZipCD1;
                detailControls[(int)EIndex.ZipCD2].Text = ade.ZipCD2;
                detailControls[(int)EIndex.Address1].Text = ade.Address1;
                detailControls[(int)EIndex.Address2].Text = ade.Address2;
                lblTel1.Text = ade.Tel11;
                lblTel2.Text = ade.Tel12;
                lblTel3.Text = ade.Tel13;

                lblCustomerCD.Text = ade.CustomerCD;
                lblCustomerName.Text = ade.CustomerName;
                lblCustomerName2.Text = ade.CustomerName2;

                if (ade.VariousFLG == "0")
                {
                    //入力不可とする。
                    foreach (Control ctl in detailControls)
                        ctl.Enabled = false;

                    btnAddress.Enabled = false;
                }
                else
                {
                    //以下の項目は入力可能にする。
                    foreach (Control ctl in detailControls)
                        ctl.Enabled = true;

                }
            }
            else
            {
                lblCustomer.Visible = false;
                lblDelivery.Visible = true;

                detailControls[(int)EIndex.ZipCD1].Text = adeD.ZipCD1;
                detailControls[(int)EIndex.ZipCD2].Text = adeD.ZipCD2;
                detailControls[(int)EIndex.Address1].Text = adeD.Address1;
                detailControls[(int)EIndex.Address2].Text = adeD.Address2;
                lblTel1.Text = adeD.Tel11;
                lblTel2.Text = adeD.Tel12;
                lblTel3.Text = adeD.Tel13;

                lblCustomerCD.Text = adeD.CustomerCD;
                lblCustomerName.Text = adeD.CustomerName;
                lblCustomerName2.Text = adeD.CustomerName2;

                if (adeD.VariousFLG == "0")
                {
                    //入力不可とする。
                    foreach (Control ctl in detailControls)
                        ctl.Enabled = false;

                    btnAddress.Enabled = false;
                }
                else
                {
                    //以下の項目は入力可能にする。
                    foreach (Control ctl in detailControls)
                        ctl.Enabled = true;

                }
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InitScr(kbn);

                detailControls[0].Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }

        protected override void ExecSec()
        {
            CheckDetail((int)EIndex.ZipCD2, false);

            if (kbn.Equals(0))
            {
                ade.ZipCD1 = detailControls[(int)EIndex.ZipCD1].Text;
                ade.ZipCD2 = detailControls[(int)EIndex.ZipCD2].Text;
                ade.Address1 = detailControls[(int)EIndex.Address1].Text;
                ade.Address2 = detailControls[(int)EIndex.Address2].Text;
            }
            else
            {
                adeD.ZipCD1 = detailControls[(int)EIndex.ZipCD1].Text;
                adeD.ZipCD2 = detailControls[(int)EIndex.ZipCD2].Text;
                adeD.Address1 = detailControls[(int)EIndex.Address1].Text;
                adeD.Address2 = detailControls[(int)EIndex.Address2].Text;
            }

            EndSec();
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
            if (detailControls[index].GetType().Equals(typeof(CKM_Controls.CKM_TextBox)))
            {
                if (((CKM_Controls.CKM_TextBox)detailControls[index]).isMaxLengthErr)
                    return false;
            }

            switch (index)
            {
                case (int)EIndex.ZipCD1:
                    break;

                case (int)EIndex.ZipCD2:
                    //郵便番号1または2に入力があった場合
                    if (!string.IsNullOrWhiteSpace(detailControls[index].Text) || !string.IsNullOrWhiteSpace(detailControls[index - 1].Text))
                    {
                        //郵便番号変換マスター(M_ZipCode)に存在すること
                        //[M_ZipCode]
                        M_ZipCode_Entity mze = new M_ZipCode_Entity
                        {
                            ZipCD1 = detailControls[index - 1].Text,
                            ZipCD2 = detailControls[index].Text
                        };
                        ZipCode_BL mbl = new ZipCode_BL();
                        bool ret = mbl.M_ZipCode_SelectData(mze);
                        if (ret)
                        {
                            if (set)
                            {
                                detailControls[index + 1].Text = mze.Address1;
                                detailControls[index + 2].Text = mze.Address2;
                            }
                        }
                        else
                        {
                            //Ｅ１０１
                            mbl.ShowMessage("E101");
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            try
            {
                CheckDetail((int)EIndex.ZipCD2);
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }
    }
}
