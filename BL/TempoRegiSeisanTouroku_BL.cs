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
    public class TempoRegiSeisanTouroku_BL:Base_BL
    {
        Base_DL bdl = new Base_DL();
        D_DepositHistory_Entity dde = new D_DepositHistory_Entity();
        D_DepositHistory_DL ddhdl = new D_DepositHistory_DL();
        D_Sales_DL dsdl = new D_Sales_DL();
        D_Juchuu_DL djdl = new D_Juchuu_DL();
        D_StoreCalculation_DL dscdl = new D_StoreCalculation_DL();
        public DataTable D_StoreData_Select()
        {
            return SimpleSelect1("26");
        }

        public DataTable D_DepositＨistory_Gaku_Select(D_DepositHistory_Entity dde)
        {
            return ddhdl.D_DepositＨistory_Gaku_Select(dde);
        }

        public DataTable D_Sales_DataSelect(D_Sales_Entity dse)
        {
            return dsdl.D_Sale_DataSelect(dse);
        }

        public DataTable D_JuChuu_DataSelect(D_Juchuu_Entity dje)
        {
            return djdl.D_JuChuu_DataSelect(dje);
        }

        public bool D_StoreCalculation_Insert_Update(D_StoreCalculation_Entity dsce)
        {
            return dscdl.D_StoreCalculation_Insert_Update(dsce);
        }

        public DataTable D_StoreCalculation_Select(D_StoreCalculation_Entity  dsce)
        {
            return dscdl.D_StoreCalculation_Select(dsce);
        }


    }
}
