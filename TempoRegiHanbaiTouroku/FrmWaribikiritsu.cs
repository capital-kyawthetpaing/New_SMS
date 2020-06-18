using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Base.Client;
using Entity;
using BL;
using Search;
using Microsoft.VisualBasic;
using static Base.Client.FrmMainForm;

namespace TempoRegiHanbaiTouroku
{
    public partial class FrmWaribikiritsu : Form
    {
        Base_BL bbl = new Base_BL();
        public int btnSelect = 0;
        public string Tanka = "";
        public string JANCD = "";
        public string SKUName = "";
        public string Teika = "";
        public short HaspoMode;
        public int SaleExcludedFlg;
        public short SaleMode;

        TempoRegiHanbaiTouroku_BL tprg_Hanbai_Bl = new TempoRegiHanbaiTouroku_BL();
        DataTable dtJuchu;
        DataTable dtBottunDetails;
        DataTable dtBottunGroup;
        D_Sales_Entity dse;

        private Base.Client.FrmMainForm.EOperationMode OperationMode;
        private string mStoreCD;

        private string mJuchuuHontaiGaku;
        private int mTaxRateFLG;

        private string mUriageNo;
        private string mSaleDate;

        public FrmWaribikiritsu()
        {
            InitializeComponent();
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                SetRequireField();
                lblJANCD.Text = JANCD;
                lblSKUName.Text = SKUName;
                lblTeika.Text = bbl.Z_SetStr(Teika);
                lblTanka.Text = bbl.Z_SetStr(FrmMainForm.GetResultWithHasuKbn((int)HASU_KBN.SISYAGONYU, bbl.Z_Set(lblTeika.Text) * (100 - bbl.Z_Set(txtRitsu.Text)) / 100));

                if (HaspoMode.Equals(1))
                    SLblTeika.Text = "価　格";

                txtRitsu.Focus();
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
  
        private void SetRequireField()
        {
            txtRitsu.Require(true);
        }

        private bool ErrorCheck(int kbn = 0)
        {
            string ymd = bbl.GetDate();

            if (kbn <= 1)
            {
                //割引率
                //入力必須(Entry required)
                if (string.IsNullOrWhiteSpace(txtRitsu.Text))
                {
                    //Ｅ１０２
                    bbl.ShowMessage("E102");
                    txtRitsu.Focus();
                    return false;
                }
                txtRitsu.Text = bbl.Z_SetStr(txtRitsu.Text);

                //100以下であること
                if (bbl.Z_Set(txtRitsu.Text) > 100)
                {
                    bbl.ShowMessage("E102");
                    txtRitsu.Focus();
                    return false;
                }

                //単価=（100－割引率）/ 100	１円未満は四捨五入
                lblTanka.Text = bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.SISYAGONYU, bbl.Z_Set(lblTeika.Text) * (100-bbl.Z_Set(txtRitsu.Text))/100));
            }

            return true;
        }
       
        private bool Save(int kbn = 0)
        {
            if (ErrorCheck(kbn))
            {
                if (kbn == 0)
                {
                    //更新処理

                }

                return true;
            }
            return false;
        }

        private void txtShippingSu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Enterキー押下時処理
                //Returnキーが押されているか調べる
                //AltかCtrlキーが押されている時は、本来の動作をさせる
                if ((e.KeyCode == Keys.Return) &&
                    ((e.KeyCode & (Keys.Alt | Keys.Control)) == Keys.None))
                {
                    if (Save(1))
                        //あたかもTabキーが押されたかのようにする
                        //Shiftが押されている時は前のコントロールのフォーカスを移動
                        ProcessTabKey(!e.Shift);
                }
            }
            catch (Exception ex)
            {
                //エラー時共通処理
                MessageBox.Show(ex.Message);
            }
        }        

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnSelect = 1;
            this.Close();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (!Save())
                return;

            btnSelect = 2;

            //M_SKU.SaleExcludedFlg	＝0なら	
            if(SaleExcludedFlg.Equals(0))
            {
                int SaleRate = 0;
                //Saleモード＝１の場合							
                if (SaleMode.Equals(1))
                {
                    //特別割引率選択
                    FrmSpecialWaribiki frm = new FrmSpecialWaribiki();
                    frm.ShowDialog();
                    switch (frm.btnSelect)
                    {
                        case 1:
                            SaleRate = 20;
                            break;
                        case 2:
                            SaleRate = 10;
                            break;
                    }
                }
                lblTanka.Text =bbl.Z_SetStr(GetResultWithHasuKbn((int)HASU_KBN.SISYAGONYU, bbl.Z_Set(lblTanka.Text) * (100 - SaleRate) / 100)); //	←１円未満は四捨五入

            }
            //単価を元の画面に反映する
            Tanka = lblTanka.Text;
            this.Close();
        }
    }
}
