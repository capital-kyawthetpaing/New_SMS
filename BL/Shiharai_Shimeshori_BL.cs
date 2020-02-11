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
  public  class Shiharai_Shimeshori_BL : Base_BL
    {
       
        D_PayCloseHistory_DL dpch_DL;
        Base_BL bbl;
        public Shiharai_Shimeshori_BL()
        {
            bbl = new Base_BL();
            dpch_DL = new D_PayCloseHistory_DL();
        }
        public bool Select_PaymentClose(D_PayCloseHistory_Entity dpch_entity,int Type)
        {
            if (dpch_DL.Select_PaymentClose(dpch_entity, Type).Rows.Count > 0)
                return true;

            return false;

        }
        public bool Insert_ShiHaRaiShime_PaymentClose(D_PayCloseHistory_Entity dpch_entity ,int InUpType)
        {
            return dpch_DL.Insert_ShiHaRaiShime_PaymentClose(dpch_entity, InUpType);
        }

        public DataTable  Check_PayCloseDate (string ChangeDate)
        {
            return bbl.SimpleSelect1("49", ChangeDate);
        }
    }
}
