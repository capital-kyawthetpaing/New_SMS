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

namespace SiharaiNyuuryoku
{
    public partial class ShiharaiNyuuryoku1 : FrmMainForm
    {
        SiharaiNyuuryoku_BL sibl = new SiharaiNyuuryoku_BL();
        M_Bank_Entity mbe = new M_Bank_Entity();
        M_MultiPorpose_Entity mme = new M_MultiPorpose_Entity();

        public ShiharaiNyuuryoku1()
        {
            InitializeComponent();
        }

        private void ShiharaiNyuuryoku1_Load(object sender, EventArgs e)
        {

        }

        private bool ErrorCheck()
        {
            if (!RequireCheck(new Control[] { ScPayee_CD.TxtCode }))
                return false;

            if (Convert.ToInt32(txtTransferAmount) > 0)
            {
                if (!RequireCheck(new Control[] { txtDepositType }))
                    return false;
            }
            else
            {
                if (!(Convert.ToInt32(txtDepositType.Text) == 1) || !(Convert.ToInt32(txtDepositType.Text) == 2))
                {
                    sibl.ShowMessage("102");
                    txtDepositType.Focus();
                    return false;
                }
            }

            if (Convert.ToInt32(txtTransferAmount) > 0)
            {
                if (!RequireCheck(new Control[] { txtAccountNo }))
                    return false;
            }

            if (Convert.ToInt32(txtTransferAmount) > 0)
            {
                if (!RequireCheck(new Control[] { txtNominee }))
                    return false;
            }

            if (Convert.ToInt32(txtTransferAmount) > 0)
            {
                if (!RequireCheck(new Control[] { txtFeeBurden }))
                    return false;
            }
            else
            {
                if (!(Convert.ToInt32(txtFeeBurden.Text) == 1) || !(Convert.ToInt32(txtFeeBurden.Text) == 2))
                {
                    sibl.ShowMessage("102");
                    txtFeeBurden.Focus();
                    return false;
                }
            }
            if (Convert.ToInt32(txtTransferAmount) > 0)
            {
                if (!RequireCheck(new Control[] { txtFeeAmount }))
                    return false;
            }
            if (Convert.ToInt32(txtBill) > 0)
            {
                if (!RequireCheck(new Control[] { txtBillNumber }))
                    return false;
            }
            if (Convert.ToInt32(txtBill) > 0)
            {
                if (!RequireCheck(new Control[] { txtDateBills }))
                    return false;
            }
            if (Convert.ToInt32(txtElectronicBond) > 0)
            {
                if (!RequireCheck(new Control[] { txtElectronicRecordNumber }))
                    return false;
            }
            if (Convert.ToInt32(txtElectronicBond) > 0)
            {
                if (!RequireCheck(new Control[] { txtDateElectronic }))
                    return false;
            }
            if (Convert.ToInt32(txtAccount1) > 0)
            {
                if (!RequireCheck(new Control[] { ScAccountCD.TxtCode }))
                    return false;
            }
            else
            {
                mme.ID = "217";
                mme.Key = ScAccountCD.TxtCode.Text;
                DataTable dtmm = new DataTable();
                dtmm = sibl.M_MultiPurpose_AccountSelect(mme);
                if(dtmm.Rows.Count == 0)
                {
                    sibl.ShowMessage("115");
                    ScAccountCD.SetFocus(1);
                    return false;
                }
                else
                {
                    ScAccountCD.LabelText = dtmm.Rows[0]["Char1"].ToString();
                }
            }
            if (Convert.ToInt32(txtAccount1) > 0)
            {
                if (!RequireCheck(new Control[] { SCAuxiliarySubject1.TxtCode }))
                    return false;
            }
            else
            {
                mme.ID = "218";
                mme.Key = SCAuxiliarySubject1.TxtCode.Text;
                mme.Char1 = ScAccountCD.TxtCode.Text;
                DataTable dtmm1 = new DataTable();
                dtmm1 = sibl.M_MultiPorpose_AuxiliarySelect(mme);
                if (dtmm1.Rows.Count == 0)
                {
                    sibl.ShowMessage("115");
                    SCAuxiliarySubject1.SetFocus(1);
                    return false;
                }
                else
                {
                    ScAccountCD.LabelText = dtmm1.Rows[0]["Char2"].ToString();
                }
            }
            if (Convert.ToInt32(txtOther2) > 0)
            {
                if (!RequireCheck(new Control[] { ScAccount2.TxtCode }))
                    return false;
            }
            else
            {
                mme.ID = "217";
                mme.Key = ScAccount2.TxtCode.Text;
                DataTable dtmm = new DataTable();
                dtmm = sibl.M_MultiPurpose_AccountSelect(mme);
                if (dtmm.Rows.Count == 0)
                {
                    sibl.ShowMessage("115");
                    ScAccount2.SetFocus(1);
                    return false;
                }
                else
                {
                    ScAccount2.LabelText = dtmm.Rows[0]["Char1"].ToString();
                }
            }
            if (Convert.ToInt32(txtOther2) > 0)
            {
                if (!RequireCheck(new Control[] { ScAuxiliarySubject2.TxtCode }))
                    return false;
            }
            else
            {
                mme.ID = "218";
                mme.Key = ScAuxiliarySubject2.TxtCode.Text;
                mme.Char1 = ScAccount2.TxtCode.Text;
                DataTable dtmm1 = new DataTable();
                dtmm1 = sibl.M_MultiPorpose_AuxiliarySelect(mme);
                if (dtmm1.Rows.Count == 0)
                {
                    sibl.ShowMessage("115");
                    ScAuxiliarySubject2.SetFocus(1);
                    return false;
                }
                else
                {
                    ScAuxiliarySubject2.LabelText = dtmm1.Rows[0]["Char2"].ToString();
                }
            }

            return true;
        }
    }
}
