using Entity;
using DL;
using System.Data;

namespace BL
{
    public class MasterTouroku_GinkouShiten_BL : Base_BL
    {
        M_Bank_DL mbdl;
        M_BankShiten_DL mbsdl;
        public MasterTouroku_GinkouShiten_BL()
        {
            mbdl = new M_Bank_DL();
            mbsdl = new M_BankShiten_DL();
        }

        public M_Bank_Entity M_Bank_Select(M_Bank_Entity mbe)
        {
            DataTable dtBank = mbdl.M_Bank_Select(mbe);
            if (dtBank.Rows.Count > 0)
            {
                mbe.BankName = dtBank.Rows[0]["BankName"].ToString();
                mbe.BankKana = dtBank.Rows[0]["BankKana"].ToString();

                return mbe;
            }
            return null;
        }

        public M_Bank_Entity M_Bank_ChangeDate_Select(M_Bank_Entity mb)
        {
            DataTable dt = mbdl.M_Bank_ChangeDate_Select(mb);
            if(dt.Rows.Count >0)
            {
                mb.ChangeDate = dt.Rows[0]["ChangeDate"].ToString();
                return mb;
            }
            return null;
        }

        public M_BankShiten_Entity M_BankShiten_Select(M_BankShiten_Entity mbse)
        {
            DataTable dtBankShiten = mbsdl.M_BankShiten_Select(mbse);
            if (dtBankShiten.Rows.Count > 0)
            {
                mbse.BranchName = dtBankShiten.Rows[0]["BranchName"].ToString();
                mbse.BranchKana = dtBankShiten.Rows[0]["BranchKana"].ToString();
                mbse.Remarks = dtBankShiten.Rows[0]["Remarks"].ToString();
                mbse.DeleteFlg = dtBankShiten.Rows[0]["DeleteFlg"].ToString();
                return mbse;
            }

            return null;
        }


        public bool M_BankShiten_InsertUpdate(M_BankShiten_Entity mbse, int mode)
        {
            return mbsdl.M_BankShiten_Insert_Update(mbse, mode);
        }

        public bool M_BankShiten_Delete(M_BankShiten_Entity mbse)
        {
            return mbsdl.M_BankShiten_Delete(mbse);
        }
    }
}
