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
    public class StoreButtonDetails_BL : Base_BL
    {
        M_StoreButtonDetails_DL storedl;
        public StoreButtonDetails_BL()
        {
            storedl = new M_StoreButtonDetails_DL();
        }

        public DataTable M_StoreButtonDetails_SelectAll(M_StoreBottunDetails_Entity mse)
        {
            return storedl.M_StoreButtonDetails_SelectAll(mse);
        }
        
    }
}
