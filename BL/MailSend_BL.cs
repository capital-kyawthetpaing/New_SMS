using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL;
using Entity;
using System.Data;

namespace BL
{
    public  class MailSend_BL : Base_BL
    {
        M_MultiPorpose_DL mppdl;
        D_Mail_DL dmdl;

        public MailSend_BL()
        {
            mppdl = new M_MultiPorpose_DL();
            dmdl = new D_Mail_DL();
        }
        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return mppdl.M_MultiPorpose_Select(MmultiporposeData);
        }

        public DataTable D_Mail_Select()
        {
            return dmdl.D_Mail_Select();
        }
        public bool D_MailSend_Update(int MailCount)
        {
            return dmdl.D_MailSend_Update(MailCount);
        }

        ///Mail Receive
        public DataTable ReceiveMailServer()
        {
            return dmdl.M_ReceiveMailServer_Select();
        }

        public DataTable ReceiveOfVendorMail(string AddressFrom)
        {
            return dmdl.M_ReceiveOfVendorMail_Select(AddressFrom);
        }
    }
}
