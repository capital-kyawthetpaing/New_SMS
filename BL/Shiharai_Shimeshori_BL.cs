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
        D_PayPlan_DL dpp_dl;
        
        Base_BL bbl;
        public Shiharai_Shimeshori_BL()
        {
            bbl = new Base_BL();
            dpch_DL = new D_PayCloseHistory_DL();
            dpp_dl = new D_PayPlan_DL();
        }

        public DataTable D_PayClose_Search(D_PayCloseHistory_Entity dpdpch_entity)
        {
            return dpch_DL.D_PayClose_Search(dpdpch_entity);
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


        public DataTable D_PayPlanValue_Select(D_PayPlan_Entity ddp_e,String type)
        {
            return dpp_dl.D_PayPlanValue_Select(ddp_e,type);
        }



    }
}
