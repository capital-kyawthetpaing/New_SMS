using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
   public class MasterTouroku_GinkouKouza_BL:Base_BL
    {
        M_Bank_DL mbdl;
        M_Kouza_DL mkzdl;
        public MasterTouroku_GinkouKouza_BL()
        {
            mbdl = new M_Bank_DL();
            mkzdl = new M_Kouza_DL();
        }

        public bool M_Kouza_Insert_Update(M_Kouza_Entity mspe, int mode)
        {
            return mkzdl.M_Kouza_Insert_Update(mspe, mode);
        }

        public bool M_Kouza_Delete(M_Kouza_Entity mspe)
        {
            return mkzdl.M_Kouza_Delete(mspe);
        }

        public M_Kouza_Entity M_Kouza_Select(M_Kouza_Entity mspe)
        {
            DataTable dtKouza = mkzdl.M_Kouza_SelectByKouzaCD(mspe);
            if (dtKouza.Rows.Count > 0)
            {
                mspe.KouzaName = dtKouza.Rows[0]["KouzaName"].ToString();
                mspe.BankCD = dtKouza.Rows[0]["BankCD"].ToString();
                mspe.BankName = dtKouza.Rows[0]["BankName"].ToString();
                mspe.BranchCD = dtKouza.Rows[0]["BranchCD"].ToString();
                mspe.BranchName = dtKouza.Rows[0]["BranchName"].ToString();
                mspe.KouzaKBN = dtKouza.Rows[0]["KouzaKBN"].ToString();
                mspe.KouzaMeigi = dtKouza.Rows[0]["KouzaMeigi"].ToString();
                mspe.KouzaNO = dtKouza.Rows[0]["KouzaNO"].ToString();
                mspe.Print1 = dtKouza.Rows[0]["Print1"].ToString();
                mspe.Print2 = dtKouza.Rows[0]["Print2"].ToString();
                mspe.Print3 = dtKouza.Rows[0]["Print3"].ToString();
                mspe.Print4 = dtKouza.Rows[0]["Print4"].ToString();

                mspe.Fee11 = dtKouza.Rows[0]["Fee11"].ToString();
                mspe.Tax11 = dtKouza.Rows[0]["Tax11"].ToString();
                mspe.Amount1 = dtKouza.Rows[0]["Amount1"].ToString();
                mspe.Fee12 = dtKouza.Rows[0]["Fee12"].ToString();
                mspe.Tax12 = dtKouza.Rows[0]["Tax12"].ToString();

                mspe.Fee21 = dtKouza.Rows[0]["Fee21"].ToString();
                mspe.Tax21 = dtKouza.Rows[0]["Tax21"].ToString();
                mspe.Amount2 = dtKouza.Rows[0]["Amount2"].ToString();
                mspe.Fee22 = dtKouza.Rows[0]["Fee22"].ToString();
                mspe.Tax22 = dtKouza.Rows[0]["Tax22"].ToString();

                mspe.Fee31 = dtKouza.Rows[0]["Fee31"].ToString();
                mspe.Tax31 = dtKouza.Rows[0]["Tax31"].ToString();
                mspe.Amount3 = dtKouza.Rows[0]["Amount3"].ToString();
                mspe.Fee32 = dtKouza.Rows[0]["Fee32"].ToString();
                mspe.Tax32 = dtKouza.Rows[0]["Tax32"].ToString();

                mspe.CompanyCD = dtKouza.Rows[0]["CompanyCD"].ToString();
                mspe.CompanyName = dtKouza.Rows[0]["CompanyName"].ToString();

                mspe.Remarks = dtKouza.Rows[0]["Remarks"].ToString();
                mspe.DeleteFlg = dtKouza.Rows[0]["DeleteFlg"].ToString();
                return mspe;
            }
            return null;
        }
    }
}
