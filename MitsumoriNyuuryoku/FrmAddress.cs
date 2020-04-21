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

namespace MitsumoriNyuuryoku
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

        private const string ProNm = "見積入力";

        private enum EIndex : int
        {
            ZipCD1,
            ZipCD2,
            Address1,
            Address2,
            Tel11,
            Tel12,
            Tel13
        }

        #region"公開プロパティ"
        public Address_Entity ade = new Address_Entity();
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

        //        public void ClearAddressInfo()
        //        {      
        //        parCustomerCD = "";
        //    parCustomerName = "";
        //parCustomerName2 = "";
        //        parZipCD1 = "";
        //            parZipCD2 = "";
        //            parAddress1 = "";
        //            parAddress2 = "";
        //            parTel11 = "";
        //            parTel12 = "";
        //            parTel13 = "";
        //        }
        private void InitialControlArray()
        {
            detailControls = new Control[] { ckM_TextBox6, ckM_TextBox5, ckM_TextBox4, ckM_TextBox7, ckM_TextBox1, ckM_TextBox2, ckM_TextBox3 };

            foreach (Control ctl in detailControls)
            {
                ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(DetailControl_KeyDown);
            }
        }
        /// <summary>
        /// 初期値設定
        /// </summary>
        private void InitScr()
        {
            detailControls[(int)EIndex.ZipCD1].Text = ade.ZipCD1;
            detailControls[(int)EIndex.ZipCD2].Text = ade.ZipCD2;
            detailControls[(int)EIndex.Address1].Text = ade.Address1;
            detailControls[(int)EIndex.Address2].Text = ade.Address2;
            detailControls[(int)EIndex.Tel11].Text = ade.Tel11;
            detailControls[(int)EIndex.Tel12].Text = ade.Tel12;
            detailControls[(int)EIndex.Tel13].Text = ade.Tel13;

            lblCustomerCD.Text = ade.CustomerCD;
            lblCustomerName.Text = ade.CustomerName;
            lblCustomerName2.Text = ade.CustomerName2;

            if (ade.VariousFLG == "0")
            {
                //入力不可とする。
                foreach (Control ctl in detailControls)
                    ctl.Enabled = false;
                
            }
            else
            {
                //以下の項目は入力可能にする。
                foreach (Control ctl in detailControls)
                    ctl.Enabled = true;

            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                InitScr();

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

            ade.ZipCD1 = detailControls[(int)EIndex.ZipCD1].Text;
            ade.ZipCD2 = detailControls[(int)EIndex.ZipCD2].Text;
            ade.Address1 = detailControls[(int)EIndex.Address1].Text;
            ade.Address2 = detailControls[(int)EIndex.Address2].Text;
            ade.Tel11 = detailControls[(int)EIndex.Tel11].Text;
            ade.Tel12 = detailControls[(int)EIndex.Tel12].Text;
            ade.Tel13 = detailControls[(int)EIndex.Tel13].Text;
            ade.CustomerCD = lblCustomerCD.Text;
            ade.CustomerName = lblCustomerName.Text;
            ade.CustomerName2 = lblCustomerName2.Text;

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
    }
}
