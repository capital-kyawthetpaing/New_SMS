using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;
using Entity;

namespace BL
{
   public class UriageMotochou_BL : Base_BL
    {
        D_MonthlyClaims_DL dmcdl;
        UriageMotochou_Entity ume;
        Base_BL bbl;
        DataTable dtCheck;

        public UriageMotochou_BL()
        {
            bbl = new Base_BL();
            dmcdl = new D_MonthlyClaims_DL();            
        }

        public bool CheckData(int type,string StoreCD,string YYYYMM)
        {
            switch (type)
            {
                case 1:
                    dtCheck = bbl.SimpleSelect1("47", "", StoreCD, YYYYMM);
                    break;
                case 2:
                    dtCheck = bbl.SimpleSelect1("48", "", StoreCD, YYYYMM);
                    break;
            }


            if (dtCheck.Rows.Count > 0)
                return true;
            else
                return false;
        }
        
        public DataTable UriageMotochou_PrintSelect(UriageMotochou_Entity ume)
        {
            return dmcdl.UriageMotochou_PrintSelect(ume);
        }
    }
}
