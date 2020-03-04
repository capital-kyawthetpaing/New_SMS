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
   public  class MasterTouroku_Brand_BL:Base_BL
    {
        M_Brand_DL bdl;
        public MasterTouroku_Brand_BL()
        {
            bdl = new M_Brand_DL();
        }
        public DataTable Brand_Select(M_Brand_Entity mbe)
        {
            return bdl.M_BrandSelect(mbe);
        }

        public bool M_Brand_Insert_Update(M_Brand_Entity mbe,int mode)
        {
            return bdl.M_Brand_Insert_Update(mbe,mode);
        }

        public bool M_Brand_Delete(M_Brand_Entity mbe)
        {
            return bdl.M_Brand_Delete(mbe);
        }


    }
}
